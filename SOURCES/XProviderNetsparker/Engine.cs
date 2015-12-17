using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Xml;
using System.Xml.Serialization;

using System.Text.RegularExpressions;

using XORCISMModel;
using XCommon;
using XProviderCommon;


namespace XProviderNetsparker
{
    /// <summary>
    /// Copyright (C) 2012-2015 Jerome Athias
    /// Netsparker plugin for XORCISM
    /// All trademarks and registered trademarks are the property of their respective owners.
    /// This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation; either version 2 of the License, or (at your option) any later version.
    /// 
    /// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
    /// 
    /// You should have received a copy of the GNU General Public License along with this program; if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
    /// </summary>
    public class Engine : MarshalByRefObject, IVulnerabilityImporter
    {
        public Engine()
        {
            TextWriterTraceListener tw;
            tw = new TextWriterTraceListener("XProviderNetsparker.log");    //Hardcoded
            Trace.Listeners.Add(tw);
            Trace.AutoFlush = true;
            Trace.IndentSize = 4;
        }

        public void Run(string data, int jobID, int AccountID)
        {
            Utils.Helper_Trace("XORCISM PROVIDER NETSPARKER", "Entering Run()");

            Utils.Helper_Trace("XORCISM PROVIDER NETSPARKER", string.Format("Creating An instance of NetsparkerParser for AccountID=" + AccountID.ToString()));

            NetsparkerParser NetsparkerParser = new NetsparkerParser(data, AccountID, jobID);

            Utils.Helper_Trace("XORCISM PROVIDER NETSPARKER", string.Format("Parsing the data"));

            NetsparkerParser.parse();

            Utils.Helper_Trace("XORCISM PROVIDER NETSPARKER", "Updating job status to FINISHED");

            XORCISMEntities model = new XORCISMEntities();
            var xJob = from j in model.JOB
                       where j.JobID == jobID
                       select j;

            JOB xJ = xJob.FirstOrDefault();
            xJ.Status = XCommon.STATUS.FINISHED.ToString();

                Utils.Helper_Trace("XORCISM PROVIDER NETSPARKER", "Changing the session to ServiceCategoryID=2");
                var xSession = from s in model.SESSION
                               where s.SessionID == xJ.SessionID
                               select s;
                SESSION xS = xSession.FirstOrDefault();
                xS.ServiceCategoryID = 2;

            model.SaveChanges();
            Utils.Helper_Trace("XORCISM PROVIDER NETSPARKER", string.Format("End of data processing"));

            Utils.Helper_Trace("XORCISM PROVIDER NETSPARKER", "Leaving Run()");
        }            
    }

    class NetsparkerParser
    {
        private string m_data;
        private int m_AccountID;
        private int m_JobId;
        public NetsparkerParser(string data, int AccountID, int jobid)
        {
            #region Hack
            /*
            string filename;
            filename = @"c:\webscantest.com_80.xml";

            XmlDocument doc = new XmlDocument();
            doc.Load(filename);

            Utils.Helper_Trace("XORCISM PROVIDER NETSPARKER", string.Format("HackFile should be located at : " + filename));
            data = doc.InnerXml;
            */
            #endregion

            // m_target = Helper_GetTarget(data);
            m_AccountID = AccountID;
            m_data = data;
            m_JobId=jobid;
        }
        public void parse()
        {
            Assembly a;
            a = Assembly.GetExecutingAssembly();

            Utils.Helper_Trace("XORCISM PROVIDER NETSPARKER", "Assembly location = " + a.Location);

            // ===================================================
            // Parses the XML Document and populates the database
            // ===================================================

         //   Utils.Helper_Trace("XORCISM PROVIDER NETSPARKER", "data = " + m_data);

            XmlDocument doc = new XmlDocument();
            //TODO: Input Validation (XML)
            doc.LoadXml(m_data);

            XORCISMEntities model;
            model = new XORCISMEntities();

            string query = "/netsparker/target";    //Hardcoded

            XmlNode report;
            report = doc.SelectSingleNode(query);

            string ipAddress = string.Empty;
            ipAddress = HelperGetChildInnerText(report, "url"); //Hardcoded
            if (ipAddress.Substring(ipAddress.Length-1, 1) == "/")
            {
                ipAddress=ipAddress.Substring(0,ipAddress.Length-1);
            }
            Utils.Helper_Trace("XORCISM PROVIDER NETSPARKER", string.Format("Handling host with IP {0}", ipAddress));

            // ===============================================
            // If necessary, creates an asset in the database
            // ===============================================

            //TODO  ipaddressIPv4
            var myass = from ass in model.ASSET
                        where ass.ipaddressIPv4 == ipAddress //&& ass.AccountID == m_AccountID
                        select ass;
            ASSET asset = myass.FirstOrDefault();

            if (asset == null)
            {
                Utils.Helper_Trace("XORCISM PROVIDER NETSPARKER", "Creates a new entry in table ASSET for this IP");

                asset = new ASSET();
                //asset.AccountID = m_AccountID;
                asset.AssetName = ipAddress;
                asset.AssetDescription = ipAddress;
                //TODO  ipaddressIPv4
                asset.ipaddressIPv4 = ipAddress;
                    asset.Enabled = true;
                //asset.JobID = m_JobId;

                model.ASSET.Add(asset);
                model.SaveChanges();
            }
            else
            {
                Utils.Helper_Trace("XORCISM PROVIDER NETSPARKER", "This IP already corresponds to an existing asset");
            }

            Utils.Helper_Trace("XORCISM PROVIDER NETSPARKER", "Creating ASSETINSESSION reference");
            ASSETSESSION assinsess = new ASSETSESSION();
            assinsess.AssetID = asset.AssetID;
            assinsess.SessionID = model.JOB.Single(x => x.JobID == m_JobId).SessionID;
            model.ASSETSESSION.Add(assinsess);
            model.SaveChanges();

            Utils.Helper_Trace("XORCISM PROVIDER NETSPARKER", "Update JOB with ASSETINSESSIONID");
            JOB daJob = model.JOB.Single(x => x.JobID == m_JobId);
            daJob.AssetSessionID = assinsess.AssetSessionID;
            model.SaveChanges();

            Utils.Helper_Trace("XORCISM PROVIDER NETSPARKER", "VULNERABILITIES FOUND");
            query = "/netsparker";  //Hardcoded

            report = doc.SelectSingleNode(query);

            foreach (XmlNode n in report.ChildNodes)
            {
                //Hardcoded
                if (n.Name.ToUpper() == "vulnerability".ToUpper() && n.ChildNodes != null && n.ChildNodes.Count > 0)
                {
                    if (n.Attributes["confirmed"].InnerText == "True")
                    {
                        VulnerabilityEndPoint vulnerabilityEndPoint = new VulnerabilityEndPoint();
                        vulnerabilityEndPoint.IpAdress = ipAddress;
                        vulnerabilityEndPoint.Protocol = "TCP";// "http";    //https ... A VOIR
                        vulnerabilityEndPoint.Port = 80;    //443 ... A VOIR

                        VulnerabilityFound vulnerabilityFound = new VulnerabilityFound();
                        //vulnerabilityFound.ListItem = Helper_GetCVE(n);
                        vulnerabilityFound.InnerXml = n.OuterXml;
                        string url = HelperGetChildInnerText(n, "url");
                        vulnerabilityFound.Url = url;
                        if(url.ToLower().Contains("https://"))
                        {
                            vulnerabilityEndPoint.Port = 443;
                        }
                        Utils.Helper_Trace("XORCISM PROVIDER NETSPARKER", string.Format("Url: {0}", url));
                            //vulnerabilityFound.Type = HelperGetChildInnerText(n, "type");
                            vulnerabilityFound.Title = HelperGetChildInnerText(n, "type");
                            vulnerabilityFound.Description = HelperGetChildInnerText(n, "type");
                            
                        vulnerabilityFound.Severity = HelperGetChildInnerText(n, "severity");
                        Utils.Helper_Trace("XORCISM PROVIDER NETSPARKER", string.Format("Severity: {0}", HelperGetChildInnerText(n, "severity")));
                        vulnerabilityFound.VulnerableParameterType = HelperGetChildInnerText(n, "vulnerableparametertype");
                        vulnerabilityFound.VulnerableParameter = HelperGetChildInnerText(n, "vulnerableparameter");
                        vulnerabilityFound.VulnerableParameterValue = HelperGetChildInnerText(n, "vulnerableparametervalue");
                        //rawrequest
                        //rawresponse
                        //extrainformation  
                        //  <info name="Found E-mails">postmaster@webscantest.com</info>
                        //  <info name="Identified Internal Path(s)">/var/www/webscantest/vulnsite/picshare/upload.pl</info>
                        vulnerabilityFound.Consequence = HelperGetChildInnerText(n, "extrainformation");

                        bool PatchUpgrade = false;
                        string MSPatch = "";

                        /*
                        <classification>
                            <OWASP>A1</OWASP>
                            <WASC>19</WASC>
                            <CWE>89</CWE>
                            <CAPEC>66</CAPEC>
                        </classification>
                        */
                        foreach (XmlNode classif in n.ChildNodes)
                        {
                            //Utils.Helper_Trace("XORCISM PROVIDER NETSPARKER", "classif n.ChildNodes: " + classif.Name);
                            if (classif.Name.ToUpper() == "classification".ToUpper() && classif.ChildNodes != null && classif.ChildNodes.Count > 0)
                            {
                                Utils.Helper_Trace("XORCISM PROVIDER NETSPARKER", "OWASP: "+HelperGetChildInnerText(classif, "OWASP"));
                                Utils.Helper_Trace("XORCISM PROVIDER NETSPARKER", "WASC: "+HelperGetChildInnerText(classif, "WASC"));
                                Utils.Helper_Trace("XORCISM PROVIDER NETSPARKER", "CWE: "+HelperGetChildInnerText(classif, "CWE"));
                                Utils.Helper_Trace("XORCISM PROVIDER NETSPARKER", "CAPEC: "+HelperGetChildInnerText(classif, "CAPEC"));
                            }
                        }

                        
                        int etat = VulnerabilityPersistor.Persist(vulnerabilityFound, vulnerabilityEndPoint, m_JobId, "netsparker", model);
                        if (etat == -1)
                        {
                            Utils.Helper_Trace("XORCISM PROVIDER NETSPARKER", string.Format("CANNOT IMPORT THIS ASSET !!!! "));
                        }
                    }
                }
            }
        }

        private string HelperGetChildInnerText(XmlNode n, string ChildName)
        {
            foreach (XmlNode child in n.ChildNodes)
            {
                if (child.Name.ToUpper() == ChildName.ToUpper())
                    return child.InnerText;
            }
            return string.Empty;
        }
    }
}
