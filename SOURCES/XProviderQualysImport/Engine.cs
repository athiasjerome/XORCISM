using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Serialization;
using System.Configuration;
using System.Threading;
using System.Diagnostics;
using System.Reflection;

using System.Text.RegularExpressions;

using XCommon;
using XProviderCommon;
using XORCISMModel;
using XVULNERABILITYModel;

using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;

namespace XProviderQualysImport
{
    /// <summary>
    /// Copyright (C) 2012-2015 Jerome Athias
    /// Qualys results Import plugin for XORCISM
    /// All trademarks and registered trademarks are the property of their respective owners.
    /// This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation; either version 2 of the License, or (at your option) any later version.
    /// 
    /// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
    /// 
    /// You should have received a copy of the GNU General Public License along with this program; if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
    /// </summary>
    public class Engine : MarshalByRefObject, XProviderCommon.IVulnerabilityImporter
    {
        static int inerror = 0;
        public Engine()
        {
            TextWriterTraceListener tw;
            tw = new TextWriterTraceListener("XProviderQualysImport.log");  //Hardcoded

            Trace.AutoFlush     = true;
            Trace.IndentSize    = 4;
            Trace.Listeners.Add(tw);
        }

        public void Run(string data, int jobID, int AccountID)
        {
            inerror = 0;

            Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", "JobID:"+jobID+" Entering Run()");
            Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", string.Format("Creating an instance of QualysParser for AccountID=" + AccountID.ToString()));

            QualysParser QualysParser = new QualysParser(data, AccountID, jobID);
            if (inerror == 0)
            {
                Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", string.Format("JobID:" + jobID + " Parsing the data"));

                QualysParser.parse();

                Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", string.Format("JobID:" + jobID + " End of data processing"));
            }

            if (inerror == 0)
            {
                Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", "JobID:" + jobID + " Leaving Run()");

                QualysParser.UpdateJob(jobID);

                Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", string.Format("Updating job {0} status to FINISHED", jobID));

                XORCISMEntities model = new XORCISMEntities();
                XVULNERABILITYEntities vuln_model = new XVULNERABILITYEntities();

                var xJob = from j in model.JOB
                           where j.JobID == jobID
                           select j;

                JOB xJ = xJob.FirstOrDefault();
                xJ.Status = XCommon.STATUS.FINISHED.ToString();

                Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", "Changing the session to ServiceCategoryID=1");
                var xSession = from s in model.SESSION
                               where s.SessionID == xJ.SessionID
                               select s;
                SESSION xS = xSession.FirstOrDefault();
                xS.ServiceCategoryID = 1;

                model.SaveChanges();
            }
            inerror = 0;
        }

        class QualysParser
        {
            private string m_data;
            private int m_AccountID;
            private int m_jobId;

            public QualysParser(string data, int AccountID, int jobid)
            {
                inerror = 0;

                m_AccountID = AccountID;
                m_data = data;
                m_jobId = jobid;
            }

            public void parse()
            {
                Assembly a;
                a = Assembly.GetExecutingAssembly();

                Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", "Assembly location = " + a.Location);

                // ============================================
                // Parse the XML Document and populate the database
                // ============================================

                string protocol = string.Empty;
                int port = -1;
                string service = string.Empty;
                bool PatchUpgrade = false;
                string title;
                string MSPatch = "";
                string Solution;

                XORCISMEntities model = new XORCISMEntities();
                XVULNERABILITYEntities vuln_model = new XVULNERABILITYEntities();

                XmlDocument doc = new XmlDocument();

                try
                {
                    Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", "JobID:" + m_jobId + " Loading the XML document for parsing");

                    doc.LoadXml(m_data);
                    Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", string.Format("JobID:" + m_jobId + " m_data = {0}", m_data));
                    /*
                    <?xml version="1.0" encoding="UTF-8"?><!DOCTYPE SCAN SYSTEM "https://qualysapi.qualys.eu/scan-1.dtd"[]><SCAN value="None"><ERROR number="5000">
  Invalid value for 'ip' : ja-psi.fr. Invalid IP or range.
                     Invalid value for 'ip' : http://8.6.6.8/. Invalid CIDR format
                    */
                    /*
                    <RETURN status="FAILED" number="2001">
    Account Expired
                    */

                    /*
                    <IP value="1.6.6.8" status="down" /><ERROR number="3509">
  No host alive...
  </ERROR><HEADER><KEY value="USERNAME">xoruser</KEY><KEY value="COMPANY"><![CDATA[XORCISM]]></KEY><KEY value="DATE">2011-06-20T10:22:27Z</KEY><KEY value="TITLE"><![CDATA[N/A (no report saved)]]></KEY><KEY value="TARGET"><![CDATA[1.6.6.8]]></KEY><KEY value="DURATION">00:11:04</KEY><KEY value="SCAN_HOST">1.6.6.4 (Scanner 5.16.44-1, Web 6.19.17-3, Vulnerability Signatures 1.28.140-2)</KEY><KEY value="NBHOST_ALIVE">0</KEY><KEY value="NBHOST_TOTAL">1</KEY><KEY value="REPORT_TYPE">API (default option profile)</KEY><KEY value="OPTIONS"><![CDATA[Custom TCP Port List (1-1000), Custom UDP Port List (1-1000), Scan Dead Hosts, Authoritative Option: Off, parallel ML scaling disabled for appliances, Load balancer detection OFF, Ignore firewall-generated SYN-ACK packets, ICMP Host Discovery, Overall Performance: Normal, Hosts to Scan in Parallel - External Scanners: 15, Hosts to Scan in Parallel - Scanner Appliances: 30, Total Processes to Run in Parallel: 10, HTTP Processes to Run in Parallel: 10, Packet (Burst) Delay: Medium, Intensity: Normal]]></KEY><KEY value="STATUS">NOHOSTALIVE</KEY>
                    */

                    /*
                        <?xml version="1.0" encoding="UTF-8"?><!DOCTYPE SCAN SYSTEM "https://qualysapi.qualys.eu/scan-1.dtd"[]><SCAN value="None"><ERROR number="19001">
  Invalid value for 'option' : Normal_Policy. Invalid option profile name Normal_Policy, expecting one of (Initial Options, Payment Card Industry (PCI) Options, Qualys Top 20 Options, SANS20 Options)
  </ERROR></SCAN><!-- 
                    */

                    if (m_data.Contains("ERROR number"))
                    {
                        Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", string.Format("Updating job {0} status to ERROR", m_jobId));
                        var Q = from j in model.JOB
                                where j.JobID == m_jobId
                                select j;

                        JOB myJob = Q.FirstOrDefault();
                        myJob.Status = XCommon.STATUS.ERROR.ToString();
                        if (m_data.Contains("No host alive"))   //Hardcoded
                        {
                            myJob.ErrorReason = "No host alive";
                        }
                        myJob.DateEnd = DateTimeOffset.Now;
                        //image
                        System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
                        myJob.XmlResult = encoding.GetBytes(m_data);
                        model.SaveChanges();
                        //FREE MEMORY
                        model.Dispose();

                        XCommon.Utils.Helper_SendEmail("contact@hackenaton.org", "QUALYS IMPORT ERROR", "QUALYS ERROR for job:" + m_jobId+" "+myJob.ErrorReason);   //HARDCODED
                        inerror = 1;
                        return;
                    }

                }
                catch (Exception ex)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", string.Format("JobID:" + m_jobId + " ExceptionParse = {0} / {1}", ex.Message, ex.InnerException == null ? "" : ex.InnerException.Message));

                    Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", string.Format("Updating job {0} status to ERROR", m_jobId));
                    var Q = from j in model.JOB
                            where j.JobID == m_jobId
                            select j;

                    JOB myJob = Q.FirstOrDefault();
                    myJob.Status = XCommon.STATUS.ERROR.ToString();
                    myJob.DateEnd = DateTimeOffset.Now;
                    //image
                    System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
                    myJob.XmlResult = encoding.GetBytes(m_data);
                    model.SaveChanges();
                    //FREE MEMORY
                    model.Dispose();
                    
                    XCommon.Utils.Helper_SendEmail("contact@hackenaton.org", "QUALYS IMPORT ERROR", "QUALYS ERROR for job:" + m_jobId); //HARDCODED
                    inerror = 1;
                    return;
                }
                if (inerror == 1)
                {
                    return;
                }

                //We should retrieve the target for an import
                string m_target = string.Empty;
                string patterntoken = "<IP value=(.*?) name=";
                MatchCollection matchesurl = Regex.Matches(m_data, patterntoken);
                foreach (Match match in matchesurl)
                {
                    m_target = match.Value.Replace("<IP value=", "").Replace(" name=", "");
                    m_target = m_target.Replace("\"", "");
                    //Console.WriteLine(mytoken);
                    Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", "target: " + m_target);
                }

                // =============================================
                // If necessary, create an asset in the database
                // =============================================
                //TODO  ipaddressIPv4
                var myass = from ass in model.ASSET
                            where ass.ipaddressIPv4 == m_target //&& ass.AccountID == m_AccountID
                            select ass;
                ASSET asset = myass.FirstOrDefault();

                if (asset == null)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", "Creates a new entry in table ASSET for this IP");
                    //TODO  ipaddressIPv4
                    asset = new ASSET();
                    //asset.AccountID = m_AccountID;
                    asset.AssetName = m_target;
                    asset.AssetDescription = m_target;
                    asset.ipaddressIPv4 = m_target;
                    asset.Enabled = true;
                    //asset.JobID = m_jobId;

                    model.ASSET.Add(asset);
                    model.SaveChanges();
                }
                else
                {
                    Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", "This IP already corresponds to an existing asset");
                }

                int m_assetId = asset.AssetID;
                int m_sessionId = (int)model.JOB.Single(x => x.JobID == m_jobId).SessionID;

                Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", "Creating ASSETINSESSION reference");
                ASSETSESSION assinsess = new ASSETSESSION();
                assinsess.AssetID = asset.AssetID;
                assinsess.SessionID = m_sessionId;  // model.JOB.Single(x => x.JobID == m_jobId).SessionID;
                model.ASSETSESSION.Add(assinsess);
                model.SaveChanges();

                Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", "Update JOB with ASSETINSESSIONID");
                JOB daJob = model.JOB.Single(x => x.JobID == m_jobId);
                daJob.AssetSessionID = assinsess.AssetSessionID;
                model.SaveChanges();

                /*
                var ipAsset = from o in model.JOB
                              where o.JobID == m_jobId
                              select o.ASSETSESSION.ASSET;

                ASSET tmpAsset = ipAsset.FirstOrDefault();

                var asset = from Assets in model.ASSET
                            //where Assets.IpAdress == tmpAsset.IpAdress
                            where Assets.AssetID == tmpAsset.AssetID
                            select Assets;
                ASSET myAsset = new ASSET();
                myAsset = asset.FirstOrDefault();

                var Session = from o in model.JOB
                              where o.JobID == m_jobId
                              select o;

                JOB theSession = Session.FirstOrDefault();
                int mySessionID = (int)theSession.SessionID;
                */
                int mySessionID = m_sessionId;

                Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", string.Format("JobID:" + m_jobId + " ASSET ID => {0}", m_assetId));

                //*********************************************************************************************************************************************
                //  OS
                string query = "/SCAN/IP";

                XmlNodeList report;
                report = null;
                try
                {
                    report = doc.SelectNodes(query);
                }
                catch (Exception ex)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", string.Format("JobID:" + m_jobId + " Error SelectNodes({0}) : Exception = {1}", query, ex.Message));
                }
                Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", string.Format("JobID:" + m_jobId + " Found {0} IP to parse", report.Count));
                foreach (XmlNode reportHost in report)
                {
                    string strTempo = "";
                    if (reportHost.Attributes["value"] != null)
                    {
                        strTempo = reportHost.Attributes["value"].InnerText.ToUpper();
                        Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", "IP:" + strTempo);
                    }
                    if (reportHost.Attributes["name"] != null)
                    {
                        strTempo = reportHost.Attributes["value"].InnerText.ToUpper();
                        Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", "name:" + strTempo);
                    }
                }

                query = "/SCAN/IP/OS";  //Hardcoded

                //XmlNodeList report;
                report = null;
                try
                {
                    report = doc.SelectNodes(query);
                }
                catch (Exception ex)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", string.Format("JobID:" + m_jobId + " Error SelectNodes({0}) : Exception = {1}", query, ex.Message));
                }
                Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", string.Format("Found {0} OS to parse", report.Count));
                foreach (XmlNode reportHost in report)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", "OS:" + reportHost.InnerText);
                }

                query = "/SCAN/IP/NETBIOS_HOSTNAME";    //Hardcoded

                //XmlNodeList report;
                report = null;
                try
                {
                    report = doc.SelectNodes(query);
                }
                catch (Exception ex)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", string.Format("JobID:" + m_jobId + " Error SelectNodes({0}) : Exception = {1}", query, ex.Message));
                }
                Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", string.Format("Found {0} NETBIOS_HOSTNAME to parse", report.Count));
                foreach (XmlNode reportHost in report)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", "NETBIOS_HOSTNAME:" + reportHost.InnerText);
                }

                //*********************************************************************************************************************************************
                //  INFOS
                query = "/SCAN/IP/INFOS/CAT";   //Hardcoded

                //XmlNodeList report;
                report = null;
                try
                {
                    report = doc.SelectNodes(query);
                }
                catch (Exception ex)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", string.Format("JobID:" + m_jobId + " Error SelectNodes({0}) : Exception = {1}", query, ex.Message));
                }
                Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", string.Format("Found {0} INFOS to parse", report.Count));
                foreach (XmlNode reportHost in report)
                {
                    // ===========================
                    // Handle every ReportItem tag
                    // ===========================

                    protocol = string.Empty;
                    port = -1;
                    if (reportHost.Attributes["protocol"] != null)
                        protocol = reportHost.Attributes["protocol"].InnerText.Trim().ToUpper();
                    if (reportHost.Attributes["port"] != null)
                        port = Convert.ToInt32(reportHost.Attributes["port"].InnerText.Trim());



                    Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", string.Format("JobID:" + m_jobId + " Protocol = [{0}]   Port = [{1}]", protocol, port));

                    int theEndPointID = 0;
                    //Check if the endpoint already exists
                    var EP = from Epoint in model.ENDPOINT
                             where Epoint.AssetID == m_assetId && Epoint.SessionID == mySessionID
                             select Epoint;
                    foreach (ENDPOINT E in EP.ToList())
                    {
                        if (E.ProtocolName == protocol && E.PortNumber == port)
                        {
                            theEndPointID = E.EndPointID;
                            break;
                        }
                    }
                    if (theEndPointID == 0)
                    {
                        Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", string.Format("JobID:" + m_jobId + " Could not find the endpoint"));
                    //    Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", string.Format("Creating a new one (port={0}, proto={1}, service={2})", port, protocol, service));

                    //    if (service == "")
                    //    {
                            try
                            {
                                //Use the PORT referential table to retrieve the service
                                var ports = from portref in model.PORT
                                            where portref.DefaultProtocolName == protocol && portref.Port_Value == port
                                            select portref;
                                if (ports.Count() > 0)
                                {
                                    PORT thePort = ports.FirstOrDefault();
                                    service = thePort.DefaultServiceName.Trim();
                                }
                            }
                            catch (Exception ex)
                            {
                                Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", string.Format("JobID:" + m_jobId + " Error PORTreferential Exception = {0}", ex.Message + " " + ex.InnerException));
                            }                
                     //   }

                            Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", "JobID:" + m_jobId + " Adding endpoint:" + protocol + "/" + port + " (" + service + ")");

                        ENDPOINT newEndPoint = new ENDPOINT();
                        newEndPoint.AssetID = m_assetId;
                        newEndPoint.ProtocolName = protocol;
                        newEndPoint.PortNumber = port;
                        newEndPoint.Service = service;
                        newEndPoint.SessionID = mySessionID;

                        model.ENDPOINT.Add(newEndPoint);
                        model.SaveChanges();
                        theEndPointID = newEndPoint.EndPointID;
                    }
                    else
                    {
                        Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", string.Format("JobID:" + m_jobId + " Endpoint found: {0}", theEndPointID));
                    }


                    foreach (XmlNode n in reportHost.ChildNodes)
                    {
                        XmlNodeList Childs = n.ChildNodes;
                        List<int> myExploits = new List<int>();
                        //Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", string.Format("Coucou 1"));


                        //TODO
                        /*
                        INFORMATION myInformation = new INFORMATION();
                        //VulnerabilityFound vulnerabilityFound = new VulnerabilityFound();
                        myInformation.EndPointID = theEndPointID;
                        myInformation.JobID = m_jobId;
                        //vulnerabilityFound.ListItem = Helper_GetCVE(n);
                        //vulnerabilityFound.ListReference = Helper_GetREFERENCE(n);  //TODO: Helper_GetCVE and Helper_GetREFERENCE could be mixed for only 1 parsing
                        //vulnerabilityFound.InnerXml = n.OuterXml;

                        myInformation.Description = HelperGetChildInnerText(n, "DIAGNOSIS");
                        myInformation.Solution = HelperGetChildInnerText(n, "SOLUTION");
                        myInformation.Severity = n.Attributes["severity"].Value;
                        myInformation.Consequence = HelperGetChildInnerText(n, "CONSEQUENCE");
                        myInformation.Result = HelperGetChildInnerText(n, "RESULT");
                        myInformation.ModifiedDate = DateTime.Parse(HelperGetChildInnerText(n, "LAST_UPDATE"));
                        if (HelperGetChildInnerText(n, "PCI_FLAG") == "1")
                        {
                            myInformation.PCI_FLAG = true;
                        }
                        myInformation.Title = HelperGetChildInnerText(n, "TITLE");

                        model.AddToINFORMATION(myInformation);
                        model.SaveChanges();
                        */
                    }
                }



                //*********************************************************************************************************************************************
                //  SERVICES

                query = "/SCAN/IP/SERVICES/CAT";    //Hardcoded

                //XmlNodeList report;
                report = null;
                try
                {
                    report = doc.SelectNodes(query);
                }
                catch (Exception ex)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", string.Format("JobID:" + m_jobId + " Error SelectNodes({0}) : Exception = {1}", query, ex.Message));
                }

                Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", string.Format("JobID:" + m_jobId + " Found {0} SERVICES to parse", report.Count));
                                
                
                foreach (XmlNode reportHost in report)
                {
                    //Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", string.Format("Handling host with IP {0}", m_target));
                    //Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", string.Format("reportHost: {0}", reportHost.InnerText));

                    // ===========================
                    // Handle every ReportItem tag
                    // ===========================

                    protocol = string.Empty;
                    port = -1;
                    service = "";
                    if (reportHost.Attributes["protocol"] != null)
                        protocol = reportHost.Attributes["protocol"].InnerText.Trim().ToUpper();
                    if (reportHost.Attributes["port"] != null)
                        port = Convert.ToInt32(reportHost.Attributes["port"].InnerText.Trim());

                    Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", string.Format("JobID:" + m_jobId + " Protocol = [{0}]   Port = [{1}]", protocol, port));
                    
                    int theEndPointID = 0;
                    //Check if the endpoint already exists
                    var EP = from Epoint in model.ENDPOINT
                             where Epoint.AssetID == m_assetId && Epoint.SessionID == mySessionID
                             select Epoint;
                    foreach (ENDPOINT E in EP.ToList())
                    {
                        if (E.ProtocolName == protocol && E.PortNumber == port)
                        {
                            theEndPointID = E.EndPointID;
                            break;
                        }
                    }
                    if (theEndPointID == 0)
                    {
                        Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", string.Format("JobID:" + m_jobId + " Could not find the endpoint"));

                        //if (service == "")
                        //{
                            try
                            {
                                //Use the PORT referential table to retrieve the service
                                var ports = from portref in model.PORT
                                            where portref.DefaultProtocolName == protocol && portref.Port_Value == port
                                            select portref;
                                if (ports.Count() > 0)
                                {
                                    PORT thePort = ports.FirstOrDefault();
                                    Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", "JobID:" + m_jobId + " The service was:" + service + " for:" + protocol + "/" + port + " replacing by referential:" + thePort.DefaultServiceName.Trim());
                                    service = thePort.DefaultServiceName.Trim();
                                }
                                else
                                {
                                    Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", "JobID:" + m_jobId + " No service found in PORT referential for port:" + protocol + "/" + port + " service:" + service);
                                }
                            }
                            catch (Exception ex)
                            {
                                Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", string.Format("JobID:" + m_jobId + " Error PORTreferential Exception = {0}", ex.Message));
                            }
                        //}
                        //else
                        //{
                            //Verify that the PORT/PROTOCOL/SERVICE is the same as the referential

                        //}

                            Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", string.Format("JobID:" + m_jobId + " Creating a new one (port={0}, proto={1}, service={2})", port, protocol, service));

                        ENDPOINT newEndPoint = new ENDPOINT();
                        newEndPoint.AssetID = m_assetId;
                        newEndPoint.ProtocolName = protocol;
                        newEndPoint.PortNumber = port;
                        newEndPoint.Service = service;
                        newEndPoint.SessionID = mySessionID;

                        model.ENDPOINT.Add(newEndPoint);
                        model.SaveChanges();
                    }
                    else
                    {
                        Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", string.Format("JobID:" + m_jobId + " Endpoint found: {0}", theEndPointID));
                    }

                    foreach (XmlNode n in reportHost.ChildNodes)
                    {
                        XmlNodeList Childs = n.ChildNodes;
                        List<int> myExploits = new List<int>();
                        //Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", string.Format("Coucou 1"));


                        //TODO
                        /*
                        INFORMATION myInformation = new INFORMATION();
                        //VulnerabilityFound vulnerabilityFound = new VulnerabilityFound();
                        myInformation.EndPointID = theEndPointID;
                        myInformation.JobID = m_jobId;
                        //vulnerabilityFound.ListItem = Helper_GetCVE(n);
                        //vulnerabilityFound.ListReference = Helper_GetREFERENCE(n);  //TODO: Helper_GetCVE and Helper_GetREFERENCE could be mixed for only 1 parsing
                        //vulnerabilityFound.InnerXml = n.OuterXml;

                        myInformation.Description = HelperGetChildInnerText(n, "DIAGNOSIS");
                        myInformation.Solution = HelperGetChildInnerText(n, "SOLUTION");
                        myInformation.Severity = n.Attributes["severity"].Value;
                        myInformation.Consequence = HelperGetChildInnerText(n, "CONSEQUENCE");
                        myInformation.Result = HelperGetChildInnerText(n, "RESULT");
                        myInformation.ModifiedDate = DateTime.Parse(HelperGetChildInnerText(n, "LAST_UPDATE"));
                        if (HelperGetChildInnerText(n, "PCI_FLAG") == "1")
                        {
                            myInformation.PCI_FLAG = true;
                        }
                        myInformation.Title = HelperGetChildInnerText(n, "TITLE");

                        model.AddToINFORMATION(myInformation);
                        model.SaveChanges();
                        */
                    }

                }

                //*********************************************************************************************************************************************
                //  VULNERABILITIES

                query = "/SCAN/IP/VULNS/CAT";   //Hardcoded

                //XmlNodeList report;
                report = null;
                try
                {
                    report = doc.SelectNodes(query);
                }
                catch (Exception ex)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", string.Format("JobID:" + m_jobId + " Error SelectNodes({0}) : Exception = {1}", query, ex.Message));
                }

                Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", string.Format("JobID:" + m_jobId + " Found {0} VULNS to parse", report.Count));
                service = "";
                foreach (XmlNode reportHost in report)
                {
                    //Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", string.Format("Handling host with IP {0}", m_target));
                    Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", string.Format("JobID:" + m_jobId + " reportHost: {0}", reportHost.InnerText));

                    // ===========================
                    // Handle every ReportItem tag
                    // ===========================

                    protocol = string.Empty;
                    port = -1;

                    if (reportHost.Attributes["protocol"] != null)
                        protocol = reportHost.Attributes["protocol"].InnerText.ToUpper();
                    if (reportHost.Attributes["port"] != null)
                        port = Convert.ToInt32(reportHost.Attributes["port"].InnerText);

//                    if (service == "")
//                    {
                        try
                        {
                            //Use the PORT referential table to retrieve the service
                            var ports = from portref in model.PORT
                                        where portref.DefaultProtocolName == protocol && portref.Port_Value == port
                                        select portref;
                            if (ports.Count() > 0)
                            {
                                PORT thePort = ports.FirstOrDefault();
                                service = thePort.DefaultServiceName.Trim();
                            }
                        }
                        catch (Exception ex)
                        {
                            Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", string.Format("Error PORTreferential Exception = {0}", ex.Message));
                        }
//                    }

                        Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", string.Format("JobID:" + m_jobId + " Protocol = [{0}]   Port = [{1}]    Service = [{2}]", protocol, port, service));

                    VulnerabilityEndPoint vulnerabilityEndPoint = new VulnerabilityEndPoint();
                    vulnerabilityEndPoint.IpAdress  = m_target;
                    vulnerabilityEndPoint.Protocol  = protocol;
                    vulnerabilityEndPoint.Port      = port;
                    vulnerabilityEndPoint.Service = service;

                    foreach (XmlNode n in reportHost.ChildNodes)
                    {
                        XmlNodeList Childs = n.ChildNodes;
                        List<int> myExploits=new List<int>();
                        //Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", string.Format("Coucou 1"));

                        VulnerabilityFound vulnerabilityFound = new VulnerabilityFound();
                        vulnerabilityFound.ListItem     = Helper_GetCVE(n);
                        vulnerabilityFound.ListReference = Helper_GetREFERENCE(n);  //TODO: Helper_GetCVE and Helper_GetREFERENCE could be mixed for only 1 parsing
                        vulnerabilityFound.InnerXml     = n.OuterXml;
                        //HARDCODED
                        vulnerabilityFound.Description  = HelperGetChildInnerText(n, "DIAGNOSIS");
                        vulnerabilityFound.Solution     = HelperGetChildInnerText(n, "SOLUTION");
                        vulnerabilityFound.Severity     = n.Attributes["severity"].Value;
                        vulnerabilityFound.Consequence  = HelperGetChildInnerText(n, "CONSEQUENCE");
                        vulnerabilityFound.Result       = HelperGetChildInnerText(n, "RESULT");
                        vulnerabilityFound.ModifiedDate = DateTime.Parse(HelperGetChildInnerText(n, "LAST_UPDATE"));
                        if (HelperGetChildInnerText(n, "PCI_FLAG") == "1")
                        {
                            vulnerabilityFound.PCI_FLAG = true;
                        }
                        vulnerabilityFound.Title        = HelperGetChildInnerText(n, "TITLE"); 
                        try
                        {
                            vulnerabilityFound.CVSSBaseScore = float.Parse(HelperGetChildInnerText(n, "CVSS_BASE"), System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch (Exception ex)
                        {
                            Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", string.Format("JobID:" + m_jobId + " Error parsing CVSS_BASE : Exception = {0}", ex.Message));
                            Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", string.Format("CVSS_BASE =", HelperGetChildInnerText(n, "CVSS_BASE")));
                        }
                        try
                        {
                            //<CORRELATION><EXPLOITABILITY><EXPLT_SRC><SRC_NAME><![CDATA[The Exploit-DB]]></SRC_NAME><EXPLT_LIST><EXPLT><REF><![CDATA[CVE-2004-0230]]></REF><DESC><![CDATA[MS Windows 2K/XP TCP Connection Reset Remote Attack Tool - The Exploit-DB Ref : 276]]></DESC><LINK><![CDATA[http://www.exploit-db.com/exploits/276]]></LINK></EXPLT><EXPLT><REF><![CDATA[CVE-2004-0230]]></REF><DESC><![CDATA[TCP Connection Reset Remote Exploit - The Exploit-DB Ref : 291]]></DESC><LINK><![CDATA[http://www.exploit-db.com/exploits/291]]></LINK></EXPLT></EXPLT_LIST></EXPLT_SRC></EXPLOITABILITY></CORRELATION>
                            string strCorrelation = HelperGetChildInnerText(n, "CORRELATION");
                            if (strCorrelation != "")
                            {
                                Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", string.Format("CORRELATION=", strCorrelation));
                                //The Exploit-DBCVE-2004-0230MS Windows 2K/XP TCP Connection Reset Remote Attack Tool - The Exploit-DB Ref : 276http://www.exploit-db.com/exploits/276CVE-2004-0230TCP Connection Reset Remote Exploit - The Exploit-DB Ref : 291http://www.exploit-db.com/exploits/291
                                /*
                                XmlNode Correl = n.SelectSingleNode("/CORRELATION");
                                foreach (XmlNode child in Correl.ChildNodes)
                                {
                                    Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", string.Format("CORRELATION CHILD:", child.Name));
                                    //if (child.Name.ToUpper() == ChildName.ToUpper())
                                    //    return child.InnerText;
                                }
                                */
                                //Regex RegexCVE =new Regex("CVE-[0-9][0-9][0-9][0-9]-[0-9][0-9][0-9][0-9]");
                                Regex myRegexCVE = new Regex(@"CVE-(19|20)\d\d-(0\d{3}|[1-9]\d{3,})");
                                //https://cve.mitre.org/cve/identifiers/tech-guidance.html
                                MatchCollection myCVES = myRegexCVE.Matches(strCorrelation);
                                /*
                                    foreach (Match match in myCVES)
                                    {
                                        foreach (Capture capture in match.Captures)
                                        {
                                            Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", string.Format("Index={0}, CVE={1}", capture.Index, capture.Value));
                                        }
                                    }
                                */
                                Regex RegexExploit = new Regex("http://www.exploit-db.com/exploits/[0-9]+");
                                MatchCollection mySploits = RegexExploit.Matches(strCorrelation);
                                int cpt = 0;
                                //List<int> myExploitsTemp=new List<int>();
                                foreach (Match match in mySploits)
                                {
                                    foreach (Capture capture in match.Captures)
                                    {
                                        vulnerabilityFound.Exploitable = true;
                                        string sploitlocation = capture.Value;
                                        Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", string.Format("Index={0}, Sploit={1}", capture.Index, sploitlocation));
                                        string sploitrefid = sploitlocation.Replace("http://www.exploit-db.com/exploits/", "");
                                        sploitrefid = sploitrefid.Replace("http://exploit-db.com/exploits/", "");
                                        Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", "sploitrefid=" + sploitrefid);
                                            string myCVE = myCVES[cpt].Value;
                                        cpt++;
                                        Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", "CVE=" + myCVE);
                                        //Check if the sploit already exist in the db
                                        int ExploitID = 0;
                                        var syn = from S in model.EXPLOIT
                                                  where S.ExploitReferential.Equals("exploit-db")
                                                  && S.ExploitRefID.Equals(sploitrefid)
                                                  && S.ExploitLocation.Equals(sploitlocation)
                                                  select S;
                                        if (syn.Count() != 0)
                                        {
                                            ExploitID = syn.ToList().First().ExploitID;
                                            Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", "Exploit already exists in the database");
                                        }
                                        else
                                        {
                                            EXPLOIT sploit = new EXPLOIT();
                                            sploit.ExploitReferential = "exploit-db";
                                            sploit.ExploitRefID = sploitrefid;
                                            //sploit.Name = sploitname;
                                            sploit.ExploitLocation = sploitlocation;
                                            //sploit.Date = sploitdate;
                                            //sploit.Verification
                                            //sploit.Platform = sploitplatform;
                                            //sploit.Author = sploitauthor;

                                            model.EXPLOIT.Add(sploit);
                                            try
                                            {
                                                model.SaveChanges();
                                            }
                                            catch (FormatException ex)
                                            {
                                                Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", "ExploitFormatException: " + ex);
                                            }
                                            ExploitID = sploit.ExploitID;
                                            Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", "New exploit added in the database");
                                        }
                                        myExploits.Add(ExploitID);
                                        string CVEcorrect = myRegexCVE.Match(myCVE).ToString();
                                        if (CVEcorrect != "")
                                        {
                                            //Check if EXPLOITFORVULNERABILITY (CVE) exist in the database
                                            int myCVEID=0;
                                            XVULNERABILITYModel.VULNERABILITY vs1;

                                            vs1 = vuln_model.VULNERABILITY.FirstOrDefault(o => o.VULReferential == "cve" && o.VULReferentialID == myCVE);
                                            if (vs1 == null)
                                            {
                                                //The CVE doesn't exist in the database
                                                vs1.VULReferential = "cve";
                                                vs1.VULReferentialID = myCVE;
                                                vuln_model.VULNERABILITY.Add(vs1);
                                                vuln_model.SaveChanges();
                                                
                                            }
                                            myCVEID = vs1.VulnerabilityID;

                                            var synev = from S in model.EXPLOITFORVULNERABILITY
                                                  where S.ExploitID.Equals(ExploitID) 
                                                  && S.VulnerabilityID.Equals(myCVEID)
                                                  select S;
                                            if (synev.Count() == 0)
                                            {
                                                EXPLOITFORVULNERABILITY sploitvuln = new EXPLOITFORVULNERABILITY();
                                                sploitvuln.VulnerabilityID = myCVEID;
                                                sploitvuln.ExploitID = ExploitID;
                                                try
                                                {
                                                    model.EXPLOITFORVULNERABILITY.Add(sploitvuln);
                                                    model.SaveChanges();
                                                    Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", "Added EXPLOITFORVULNERABILITY");
                                                }
                                                catch (FormatException ex)
                                                {
                                                    Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", string.Format("AddToEXPLOITFORVULNERABILITY" + ex));
                                                }
                                            }
                                        }
                                    }
                                }
                            }


                        }
                        catch (Exception ex)
                        {
                            Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", string.Format("Error parsing Correlation : Exception = {0}", ex.Message));
                        }
                        ////<CVE_ID_LIST><CVE_ID><ID><![CDATA[CVE-2002-0510]]></ID><URL><![CDATA[http://cve.mitre.org/cgi-bin/cvename.cgi?name=CVE-2002-0510]]></URL></CVE_ID></CVE_ID_LIST>
                        //<BUGTRAQ_ID_LIST><BUGTRAQ_ID><ID><![CDATA[4314]]></ID><URL><![CDATA[http://www.securityfocus.com/bid/4314]]></URL></BUGTRAQ_ID></BUGTRAQ_ID_LIST>

                        #region JEROME
                        PatchUpgrade = false;
                        title = HelperGetChildInnerText(n, "TITLE");
                        MSPatch = "";
                        Regex objNaturalPattern = new Regex("MS[0-9][0-9]-[0-9][0-9][0-9]");
                        MSPatch = objNaturalPattern.Match(title).ToString();
                        if (MSPatch != "")
                        {
                            Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", "MSPatch=" + MSPatch);
                            PatchUpgrade = true;
                        }
                        else
                        {
                            Solution = HelperGetChildInnerText(n, "SOLUTION");
                            //WARNING:   no vendor supplied patches available
                            if (Solution.Contains("A security update for multiple vulnerabilities on Microsoft Windows systems is available for download from Microsoft security bulletin"))
                            {
                                PatchUpgrade = true;
                            }
                            if (Solution.Contains("Following are links for downloading patches to fix the vulnerabilities"))
                            {
                                PatchUpgrade = true;
                            }
                            if (Solution.Contains("http://www.microsoft.com/downloads/details.aspx?FamilyId="))
                            {
                                PatchUpgrade = true;
                            }
                            if (Solution.Contains("Upgrade to "))  //"Upgrade to the latest version of"
                            {
                                PatchUpgrade = true;
                            }
                            if (Solution.Contains(" released a patch"))
                            {
                                PatchUpgrade = true;
                            }
                            if (Solution.Contains(" released patches"))
                            {
                                PatchUpgrade = true;
                            }
                            if (Solution.Contains(" released some patches"))
                            {
                                PatchUpgrade = true;
                            }
                            if (Solution.Contains(" issued updates"))
                            {
                                PatchUpgrade = true;
                            }
                            if (Solution.Contains(" issued fixes"))
                            {
                                PatchUpgrade = true;
                            }
                            if (Solution.Contains(" resolved in newer releases of"))
                            {
                                PatchUpgrade = true;
                            }
                            if (Solution.Contains(" been resolved in "))
                            {
                                PatchUpgrade = true;
                            }
                            if (Solution.Contains("has been fixed in "))
                            {
                                PatchUpgrade = true;
                            }
                            if (Solution.Contains("was fixed in "))
                            {
                                PatchUpgrade = true;
                            }
                            if (Solution.Contains("fixes are available"))
                            {
                                PatchUpgrade = true;
                            }
                            if (Solution.Contains("containing patches"))
                            {
                                PatchUpgrade = true;
                            }
                            if (Solution.Contains("The following patches are available"))
                            {
                                PatchUpgrade = true;
                            }
                            if (Solution.Contains("A newer update"))
                            {
                                PatchUpgrade = true;
                            }
                            if (Solution.Contains("Install patch"))
                            {
                                PatchUpgrade = true;
                            }
                            if (Solution.Contains("Microsoft patches"))
                            {
                                PatchUpgrade = true;
                            }
                            if (Solution.Contains("NT 4.0 patch"))
                            {
                                PatchUpgrade = true;
                            }
                            if (Solution.Contains("Server patch"))
                            {
                                PatchUpgrade = true;
                            }
                            //////////////////////////
                            //if (PatchUpgrade)
                            //{
                            //    Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", title);
                            //}
                        }

                        if (PatchUpgrade)
                        {
                            Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", "PatchUpgrade");
                        }
                        else
                        {
                            //    Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", "Solution: " + Solution);
                        }
                        vulnerabilityFound.PatchUpgrade = PatchUpgrade;
                        vulnerabilityFound.MSPatch = MSPatch;
                        #endregion

                        
                        // ===========
                        // Persistance
                        // ===========

                        Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", string.Format("Persistance [{0}] [{1}] [{2}] [{3}]", protocol, port, service, Helper_ListCVEToString(vulnerabilityFound.ListItem)));
                        int VulnID = VulnerabilityPersistor.Persist(vulnerabilityFound, vulnerabilityEndPoint, m_jobId, "qualys", model);


                        //Check if EXPLOITFORVULNERABILITY exist in the database
                        foreach (int sID in myExploits)
                        {
                            var syn2 = from S in model.EXPLOITFORVULNERABILITY
                                       where S.VulnerabilityID.Equals(VulnID)
                                       && S.ExploitID.Equals(sID)
                                       select S;
                            if (syn2.Count() != 0)
                            {

                            }
                            else
                            {
                                EXPLOITFORVULNERABILITY sploitvuln = new EXPLOITFORVULNERABILITY();
                                sploitvuln.VulnerabilityID = VulnID;
                                sploitvuln.ExploitID = sID;   //sploit.ExploitID;
                                try
                                {
                                    model.EXPLOITFORVULNERABILITY.Add(sploitvuln);
                                    model.SaveChanges();
                                    Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", "Added EXPLOITFORVULNERABILITY");
                                }
                                catch (FormatException ex)
                                {
                                    Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", string.Format("AddToEXPLOITFORVULNERABILITY: ", ex));
                                }
                            }
                        }

                        var xTmpJob = from j in model.JOB
                                   where j.JobID == m_jobId
                                   select j;

                        JOB xJob = xTmpJob.FirstOrDefault();
                        if (xJob.SESSION.ServiceCategoryID == 4) // Compliance
                        {
                            //TODO
                            /*

                            #region Persist Compliances
                            List<int> Compliances = new List<int>();

                            Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", string.Format("Node xml ==> {0}", n.InnerText));

                            Compliances = GetCompliance(n.InnerXml, reportHost.Attributes["value"].InnerText);
                            Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", string.Format("Vulnerability persisted , VulnID = {0} & Compliance count = {1}", VulnID, Compliances.Count));

                            var V = from tmpVuln in model.VULNERABILITYFOUND
                                    where tmpVuln.VulnerabilityFoundID == VulnID
                                    select tmpVuln;

                            VULNERABILITYFOUND VF = V.FirstOrDefault();

                            foreach (int i in Compliances)
                            {
                                Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", string.Format("Categorie Compliance => ", i));
                                var C = from Comp in model.COMPLIANCECATEG
                                        where Comp.ComplianceCategID == i
                                        select Comp;

                                COMPLIANCECATEG myCompliance = new COMPLIANCECATEG();
                                myCompliance = C.FirstOrDefault();

                                VF.COMPLIANCECATEG.Add(myCompliance);

                                model.SaveChanges();
                                Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", "Mapping Compliance-Vulnerability Added");
                            }
                            
                            #endregion
                            */
                        }
                    }
                }

                // A VOIR
                // VulnerabilityPersistor.UpdateVulnerabilityJob(list_vulnerabilyFound,m_JobId,m_model);
            }

            private List<int> GetCompliance(string xml, string ValTitle)
            {
                List<int> myIds = new List<int>();
                //HARDCODED
                string tmp = "<DetailVuln>";
                tmp += xml;
                tmp += "</DetailVuln>";

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(tmp);

                XORCISMEntities model = new XORCISMEntities();

                #region PCI
                XmlNodeList list = doc.SelectNodes("/DetailVuln/PCI_FLAG"); //Hardcoded
                foreach (XmlNode n in list)
                {
                    if (n.InnerText == "1")
                    {
                        //TODO
                        /*
                        var id = from o in model.COMPLIANCECATEG
                                 where o.Title == ValTitle &&
                                 o.COMPLIANCE.Title == "PCIDSS"
                                 select o.ComplianceCategID;
                        int Id = id.FirstOrDefault();
                        
                        myIds.Add(Id);
                        //MessageBox.Show("[" + ValTitle + "] Id Compliance Categorie = " + Id);
                        */
                    }
                }
                #endregion

                #region Compliance
                XmlNodeList nlist = doc.SelectNodes("/DetailVuln/COMPLIANCE/COMPLIANCE_INFO");  //Hardcoded

                foreach (XmlNode node in nlist)
                {
                    string[] SousCat = node.ChildNodes[1].InnerText.Split(new char[] { ' ' });

                    foreach (string SC in SousCat)
                    {
                        if (SC != "and")
                        {
                            //TODO
                            /*
                            string message = "Categorie = " + node.FirstChild.InnerText;
                            message += " | Sous Categorie = " + SC;
                            message += " | Description = ...";
                            message += " | VULNERABILITY = " + ValTitle;
                            //MessageBox.Show(message);
                            var Q = from o in model.COMPLIANCECATEG
                                    where o.Title == SC
                                    select o.ComplianceCategID;
                            int Id = Q.FirstOrDefault();
                            myIds.Add(Id);
                            */
                        }
                    }
                }
                #endregion
                return myIds;
            }

            private string Helper_ListCVEToString(List<VulnerabilityFound.Item> list)
            {
                string s = "";

                foreach (VulnerabilityFound.Item item in list)
                    s = s + item.ID + ":" + item.Value + " / ";

                return s;
            }

            private string HelperGetChildInnerText(XmlNode n, string ChildName)
            {
                foreach (XmlNode child in n.ChildNodes)
                {
                    if (child.Name.ToUpper() == ChildName.ToUpper())
                    {
                        /*
                        if (ChildName == "CORRELATION")
                        {
                            Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", "CORRELATION FOUND: " + child.InnerText);
                            foreach (XmlNode childcorrel in child.ChildNodes)
                            {
                                Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", "CORRELATION CHILD: " + childcorrel.Name);
                            }
                            return child.InnerXml;
                        }
                        else
                        {
                        */
                            return child.InnerText;
                        //}
                    }
                }
                return string.Empty;
            }

            private List<VulnerabilityFound.Item> Helper_GetCVE(XmlNode node)
            {
                List<VulnerabilityFound.Item> l= new List<VulnerabilityFound.Item>();

                if(node.Attributes["cveid"] == null)
                    return l;

                string s;
                s = node.Attributes["cveid"].InnerText;

                string[] tab;
                tab = s.Split(new char[] { ',' });

                string myCVEID = "";
                foreach (string x in tab)
                {                    
                        VulnerabilityFound.Item item;
                        item = new VulnerabilityFound.Item();
                        myCVEID = x.Trim();
                        item.Value = myCVEID;
                        Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", string.Format("TEST CVE: {0}", myCVEID));
                        item.ID = "cve";
                        l.Add(item);
                }
                return l;
            }

            private List<VulnerabilityFound.Reference> Helper_GetREFERENCE(XmlNode node)
            {
                /*
                <VENDOR_REFERENCE_LIST>
                  <VENDOR_REFERENCE>
                    <ID><![CDATA[MS03-034]]></ID>
                    <URL><![CDATA[http://www.microsoft.com/technet/security/Bulletin/MS03-034.mspx]]></URL>
                  </VENDOR_REFERENCE>
                </VENDOR_REFERENCE_LIST>
                <CVE_ID_LIST>
                  <CVE_ID>
                    <ID><![CDATA[CVE-2003-0661]]></ID>
                    <URL><![CDATA[http://cve.mitre.org/cgi-bin/cvename.cgi?name=CVE-2003-0661]]></URL>
                  </CVE_ID>
                </CVE_ID_LIST>
                <BUGTRAQ_ID_LIST>
                  <BUGTRAQ_ID>
                    <ID><![CDATA[8532]]></ID>
                    <URL><![CDATA[http://www.securityfocus.com/bid/8532]]></URL>
                  </BUGTRAQ_ID>
                </BUGTRAQ_ID_LIST>
                */
                List<VulnerabilityFound.Reference> l = new List<VulnerabilityFound.Reference>();
                foreach (XmlNode child in node.ChildNodes)
                {
                    //HARDCODED
                    if (child.Name.ToUpper() == "VENDOR_REFERENCE_LIST")
                    {
                        Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", "VENDOR_REFERENCE_LIST");
                        foreach (XmlNode noderef in child.ChildNodes)
                        {
                            VulnerabilityFound.Reference refvuln = new VulnerabilityFound.Reference();
                            string refurl = HelperGetChildInnerText(noderef, "URL");
                            if (refurl.Contains("/bugtraq/"))
                                refvuln.Source = "BUGTRAQ";
                            if (refurl.Contains("marc.theaimsgroup.com/?l=bugtraq"))
                                refvuln.Source = "BUGTRAQ";
                            if (refurl.Contains("securityfocus.com/bid"))
                                refvuln.Source = "BID";
                            if (refurl.Contains("osvdb.org/"))
                                refvuln.Source = "OSVDB";
                            if (refurl.Contains("xforce.iss.net/"))
                                refvuln.Source = "XF";
                            if (refurl.Contains("www.iss.net/"))
                                refvuln.Source = "XF";
                            if (refurl.Contains("www.ciac.org/"))
                                refvuln.Source = "CIAC";
                            if (refurl.Contains("ciac.llnl.gov/"))
                                refvuln.Source = "CIAC";
                            if (refurl.Contains("www.cert.org/"))
                                refvuln.Source = "CERT";
                            if (refurl.Contains("sunsolve.sun.org/"))
                                refvuln.Source = "SUN";
                            if (refurl.Contains("sunsolve.sun.com/"))
                                refvuln.Source = "SUN";
                            if (refurl.Contains("patches.sgi.com/"))
                                refvuln.Source = "SGI";
                            if (refurl.Contains("microsoft.com/default.aspx?scid=kb"))
                                refvuln.Source = "MSKB";
                            if (refurl.Contains("ftp.sco.com/"))
                                refvuln.Source = "SCO";
                            if (refurl.Contains("www.trustix.org/"))
                                refvuln.Source = "TRUSTIX";
                            if (refurl.Contains("ftp.freebsd.org/"))
                                refvuln.Source = "FREEBSD";
                            if (refurl.Contains("www.secunia.com/"))
                                refvuln.Source = "SECUNIA";
                            if (refurl.Contains("www.vupen.com/"))
                                refvuln.Source = "VUPEN";
                            if (refurl.Contains("www.securitytracker.com/"))
                                refvuln.Source = "SECTRACK";
                            if (refurl.Contains("www.redhat.com/"))
                                refvuln.Source = "REDHAT";
                            if (refurl.Contains("www.exploit-db.com/"))
                                refvuln.Source = "EXPLOIT-DB";
                            if (refurl.Contains("www.milw0rm.com/"))
                                refvuln.Source = "MILW0RM";
                            if (refurl.Contains("www.microsoft.com/"))
                                refvuln.Source = "MS";
                            if (refurl.Contains("seclists.org/fulldisclosure"))
                                refvuln.Source = "FULLDISC";

                            refvuln.Title = HelperGetChildInnerText(noderef, "ID");
                            refvuln.Url = refurl;
                            l.Add(refvuln);
                            Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", string.Format("TEST REFERENCE: {0}", refurl));
                        }
                    }
                    else
                    {
                        if (child.Name.ToUpper() == "BUGTRAQ_ID_LIST")
                        {
                            Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", "BUGTRAQ_ID_LIST");
                            foreach (XmlNode noderef in child.ChildNodes)
                            {
                                VulnerabilityFound.Reference Reference = new VulnerabilityFound.Reference();
                                string refurl = HelperGetChildInnerText(noderef, "URL");
                                Reference.Source = "BID";
                                if (refurl.Contains("www.microsoft.com/"))
                                    Reference.Source = "MS";

                                Reference.Title = HelperGetChildInnerText(noderef, "ID");
                                Reference.Url = refurl;
                                l.Add(Reference);
                                Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", string.Format("TEST REFERENCEBID: {0}", refurl));
                            }
                        }
                    }
                }

                return l;
            }

            public void UpdateJob(int JobId)
            {
                string Statut = XCommon.STATUS.FINISHED.ToString();

                XORCISMEntities model = new XORCISMEntities();
                var Q = from o in model.JOB
                        where o.JobID == JobId
                        select o;
                JOB myJob = Q.FirstOrDefault();

                myJob.Status = Statut;
                myJob.DateEnd = DateTimeOffset.Now;
                
                //image
                System.Text.UTF8Encoding  encoding=new System.Text.UTF8Encoding();                
                myJob.XmlResult = encoding.GetBytes(m_data);
                
                model.SaveChanges();
            }

            private string GetOptionProfile(int jobID, string policy, string strategy)
            {
                string Profile = string.Empty;
                try
                {                    
                    XORCISMEntities model = new XORCISMEntities();

                    var Provider = from o in model.JOB
                                   where o.JobID == jobID
                                   select o.PROVIDER;

                    PROVIDER CurrentProvider = Provider.FirstOrDefault();

                    var x = from o in model.PARAMETERSFORPROVIDER
                            where o.Policy == policy && 
                            o.ProviderID == CurrentProvider.ProviderID &&
                            o.Strategy == strategy
                            select o;

                    string partUrl = x.FirstOrDefault().Parameters;

                    if(string.IsNullOrEmpty(partUrl) == false)
                    {
                        Profile = "&option=";
                        Profile += partUrl;
                    }
                    
                }
                catch (Exception ex)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER QUALYS Import", string.Format("GetOptionProfile Exception = {0} / {1}", ex.Message, ex.InnerException == null ? "" : ex.InnerException.Message));
                }
                return Profile;
            }
        }

        internal class AcceptAllCertificatePolicy : ICertificatePolicy
        {
            public AcceptAllCertificatePolicy()
            {
            }

            public bool CheckValidationResult(ServicePoint sPoint,
               X509Certificate cert, WebRequest wRequest, int certProb)
            {
                //TODO
                // *** Always accept!
                return true;
            }
        }

        private int SearchForQualysID(string qualysID)
        {
            //
            /*
            XORCISMEntities model = new XORCISMEntities();
            
            var Q = from o in model.VULNERABILITYSYNONYM
                    where o.Referential=="qualys" && 
                    o.Value == qualysID
                    select o;
            
            if (Q.ToList().Count == 0)
            {
                
                return 0;
            }
            else
            {
                
                return Q.ToList().First().ID;
            }
            */

            return -1;
        }
        
    }
}
