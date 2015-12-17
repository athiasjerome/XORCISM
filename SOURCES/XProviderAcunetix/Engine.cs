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


namespace XProviderAcunetix
{
    /// <summary>
    /// Copyright (C) 2012-2015 Jerome Athias
    /// XORCISM Plugin for Acunetix (Import a result file in an XORCISM database)
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
            tw = new TextWriterTraceListener("XProviderAcunetix.log");  //Hardcoded
            Trace.Listeners.Add(tw);
            Trace.AutoFlush = true;
            Trace.IndentSize = 4;
        }

        public void Run(string data, int jobID, int AccountID)
        {
            Utils.Helper_Trace("XORCISM PROVIDER ACUNETIX", "Entering Run()");

            Utils.Helper_Trace("XORCISM PROVIDER ACUNETIX", string.Format("Creating An instance of AcunetixParser for AccountID=" + AccountID.ToString()));

            AcunetixParser AcunetixParser = new AcunetixParser(data, AccountID, jobID);

            Utils.Helper_Trace("XORCISM PROVIDER ACUNETIX", string.Format("Parsing the data"));

            AcunetixParser.parse();

            Utils.Helper_Trace("XORCISM PROVIDER ACUNETIX", "Updating job status to FINISHED");

            XORCISMEntities model = new XORCISMEntities();
            var xJob = from j in model.JOB
                       where j.JobID == jobID
                       select j;

            JOB xJ = xJob.FirstOrDefault();
            xJ.Status = XCommon.STATUS.FINISHED.ToString();

                Utils.Helper_Trace("XORCISM PROVIDER ACUNETIX", "Changing the session to ServiceCategoryID=2");
                var xSession = from s in model.SESSION
                               where s.SessionID == xJ.SessionID
                               select s;
                SESSION xS = xSession.FirstOrDefault();
                xS.ServiceCategoryID = 2;

            model.SaveChanges();
            Utils.Helper_Trace("XORCISM PROVIDER ACUNETIX", string.Format("The data end of data processing"));

            Utils.Helper_Trace("XORCISM PROVIDER ACUNETIX", "Leaving Run()");
        }            
    }

    class AcunetixParser
    {
        private string m_data;
        private int m_AccountID;
        private int m_JobId;
        public AcunetixParser(string data, int AccountID, int jobid)
        {
            #region Hack
            /*
            string filename;
            filename = @"C:\Program Files (x86)\Acunetix\Web Vulnerability Scanner 7\results.xml";  //Hardcoded

            XmlDocument doc = new XmlDocument();
            //TODO: Input/XML Validation
            doc.Load(filename);

            Utils.Helper_Trace("XORCISM PROVIDER ACUNETIX", string.Format("HackFile should be located at : " + filename));
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

            Utils.Helper_Trace("XORCISM PROVIDER ACUNETIX", "Assembly location = " + a.Location);

            // ============================================
            // Parse the XML Document and populate the database
            // ============================================

         //   Utils.Helper_Trace("XORCISM PROVIDER ACUNETIX", "data = " + m_data);

            XmlDocument doc = new XmlDocument();

            doc.LoadXml(m_data);

            XORCISMEntities model;
            model = new XORCISMEntities();

            string query = "/ScanGroup/Scan";   //Hardcoded

            XmlNode report;
            report = doc.SelectSingleNode(query);

            string ipAddress = string.Empty;
            ipAddress = HelperGetChildInnerText(report, "StartURL");    //Hardcoded
            if (ipAddress.Substring(ipAddress.Length-1, 1) == "/")
            {
                ipAddress=ipAddress.Substring(0,ipAddress.Length-1);
            }
            Utils.Helper_Trace("XORCISM PROVIDER ACUNETIX", string.Format("Handling host with IP {0}", ipAddress));

            // ===============================================
            // If necessary, creates an asset in the database
            // ===============================================
            //TODO
            var myass = from ass in model.ASSET
                        where ass.ipaddressIPv4 == ipAddress //&& ass.AccountID == m_AccountID
                        select ass;
            ASSET asset = myass.FirstOrDefault();

            if (asset == null)
            {
                Utils.Helper_Trace("XORCISM PROVIDER ACUNETIX", "Creates a new entry in table ASSET for this IP");

                asset = new ASSET();
                //asset.AccountID = m_AccountID;
                asset.AssetName = ipAddress;
                asset.AssetDescription = ipAddress;
                asset.ipaddressIPv4 = ipAddress;
                    asset.Enabled = true;
                //asset.JobID = m_JobId;

                model.ASSET.Add(asset);
                model.SaveChanges();
            }
            else
            {
                Utils.Helper_Trace("XORCISM PROVIDER ACUNETIX", "This IP already corresponds to an existing asset");
            }

            Utils.Helper_Trace("XORCISM PROVIDER ACUNETIX", "Creating ASSETINSESSION reference");
            ASSETSESSION assinsess = new ASSETSESSION();
            assinsess.AssetID = asset.AssetID;
            assinsess.SessionID = model.JOB.Single(x => x.JobID == m_JobId).SessionID;
            model.ASSETSESSION.Add(assinsess);
            model.SaveChanges();

            Utils.Helper_Trace("XORCISM PROVIDER ACUNETIX", "Update JOB with ASSETINSESSIONID");
            JOB daJob = model.JOB.Single(x => x.JobID == m_JobId);
            daJob.AssetSessionID = assinsess.AssetSessionID;
            model.SaveChanges();

            Utils.Helper_Trace("XORCISM PROVIDER ACUNETIX", "VULNERABILITIES FOUND");
            query = "/ScanGroup/Scan/ReportItems";

            report = doc.SelectSingleNode(query);

            foreach (XmlNode n in report.ChildNodes)
            {
                if (n.Name.ToUpper() == "ReportItem".ToUpper() && n.ChildNodes != null && n.ChildNodes.Count > 0)
                {                    
                        //TODOs HARDCODED
                        VulnerabilityEndPoint vulnerabilityEndPoint = new VulnerabilityEndPoint();
                        vulnerabilityEndPoint.IpAdress = ipAddress;
                        vulnerabilityEndPoint.Protocol = "TCP";// "http";    //https ... A VOIR
                        vulnerabilityEndPoint.Port = 80;    //443 ... A VOIR

                        VulnerabilityFound vulnerabilityFound = new VulnerabilityFound();
                        //vulnerabilityFound.ListItem = Helper_GetCVE(n);

                        vulnerabilityFound.InnerXml = n.OuterXml;
                        //To eliminate VULNERABILITY (Value) duplicates:
                        /*
                        string pattern = @"ReportItem id=""\d\d?\d?""";
                        string s = Regex.Replace(n.OuterXml, pattern, "ReportItem id=\"0\"");
                        vulnerabilityFound.InnerXml = s;
                        */
                        string url = HelperGetChildInnerText(n, "Affects"); //Server
                        vulnerabilityFound.Url = url;
                        if(url.ToLower().Contains("https://"))
                        {
                            vulnerabilityEndPoint.Port = 443;
                        }
                        Utils.Helper_Trace("XORCISM PROVIDER ACUNETIX", string.Format("Url: {0}", url));
                        vulnerabilityFound.Type = HelperGetChildInnerText(n, "Type");
                        if (HelperGetChildInnerText(n, "IsFalsePositive") == "False")
                        {
                            vulnerabilityFound.IsFalsePositive = false;
                        }
                        else
                        {
                            vulnerabilityFound.IsFalsePositive = true;
                        }
                            vulnerabilityFound.Title = HelperGetChildInnerText(n, "Name");
                            //ModuleName
                            //Affects
                            vulnerabilityFound.Description = HelperGetChildInnerText(n, "Description");
                            //Extract the CVEs
                            List<VulnerabilityFound.Item> ListCVEs = new List<VulnerabilityFound.Item>();
                            //MatchCollection matches = Regex.Matches(HelperGetChildInnerText(n, "Description"), "CVE-[0-9][0-9][0-9][0-9]-[0-9][0-9][0-9][0-9]");
                            MatchCollection matches = Regex.Matches(HelperGetChildInnerText(n, "Description"), @"CVE-(19|20)\d\d-(0\d{3}|[1-9]\d{3,})");    //myRegexCVE
                            //https://cve.mitre.org/cve/identifiers/tech-guidance.html        

                            foreach (Match match in matches)
                            {
                                Utils.Helper_Trace("XORCISM PROVIDER ACUNETIX", string.Format("CVE: {0}", match.Groups[1].Value));
                                VulnerabilityFound.Item item;
                                item = new VulnerabilityFound.Item();
                                item.ID = "cve";
                                item.Value = match.Groups[1].Value;
                                ListCVEs.Add(item);
                            }

                            string mySeverity=HelperGetChildInnerText(n, "Severity");
                            switch (mySeverity)
                            {
                                    //HARDCODED
                                case "high":
                                    mySeverity="High";
                                    break;
                                case "medium":
                                    mySeverity="Medium";
                                    break;
                                case "low":
                                    mySeverity="Low";
                                    break;
                                //case "info"

                            }

                            vulnerabilityFound.Severity = mySeverity;
                            Utils.Helper_Trace("XORCISM PROVIDER ACUNETIX", string.Format("Severity: {0}", mySeverity));
                            string DetailsAnalysis = HelperGetChildInnerText(n, "Details");
                            if (DetailsAnalysis.Contains("URL encoded GET"))
                            {
                                vulnerabilityFound.VulnerableParameterType = "GET"; //should be Querystring for Netsparker
                                var regex = new Regex(@"URL encoded GET input <b><font color=""dark"">(.*?)</font></b>");
                                var match = regex.Match(DetailsAnalysis);
                                if (match.Success)
                                {
                                    Utils.Helper_Trace("XORCISM PROVIDER ACUNETIX", string.Format("VulnerableParameter: {0}", match.Groups[1].Value));
                                    vulnerabilityFound.VulnerableParameter = match.Groups[1].Value;
                                    regex = new Regex(@"was set to <b><font color=""dark"">(.*?)</font></b>");
                                    match = regex.Match(DetailsAnalysis);
                                    if (match.Success)
                                    {
                                        Utils.Helper_Trace("XORCISM PROVIDER ACUNETIX", string.Format("VulnerableParameterValue: {0}", match.Groups[1].Value));
                                        vulnerabilityFound.VulnerableParameterValue = match.Groups[1].Value;
                                    }
                                }
                            }
                            if (DetailsAnalysis.Contains("URL encoded POST"))
                            {
                                vulnerabilityFound.VulnerableParameterType = "POST"; //should be Post for Netsparker
                                var regex = new Regex(@"URL encoded POST input <b><font color=""dark"">(.*?)</font></b>");
                                var match = regex.Match(DetailsAnalysis);
                                if (match.Success)
                                {
                                    Utils.Helper_Trace("XORCISM PROVIDER ACUNETIX", string.Format("VulnerableParameter: {0}", match.Groups[1].Value));
                                    vulnerabilityFound.VulnerableParameter = match.Groups[1].Value;
                                    regex = new Regex(@"was set to <b><font color=""dark"">(.*?)</font></b>");
                                    match = regex.Match(DetailsAnalysis);
                                    if (match.Success)
                                    {
                                        Utils.Helper_Trace("XORCISM PROVIDER ACUNETIX", string.Format("VulnerableParameterValue: {0}", match.Groups[1].Value));
                                        vulnerabilityFound.VulnerableParameterValue = match.Groups[1].Value;
                                    }
                                }
                            }
                        //vulnerabilityFound.VulnerableParameterType = HelperGetChildInnerText(n, "vulnerableparametertype");
                        //vulnerabilityFound.VulnerableParameter = HelperGetChildInnerText(n, "vulnerableparameter");
                            //in <Details>:
                            //URL encoded GET input <b><font color="dark">id</font></b> was set to <b><font color="dark">4-2+2*3-6</font></b>
                            //URL encoded GET input <b><font color="dark">id</font></b> was set to <b><font color="dark">1'</font></b><br/>Error message found: <pre wrap="virtual"><font color="blue">supplied argument is not a valid MySQL result</font></pre>
                            //URL encoded POST input <b><font color="dark">name</font></b> was set to <b><font color="dark">'&quot;()&amp;%1&lt;ScRiPt &gt;prompt(983150)&lt;/ScRiPt&gt;</font></b>
                        //vulnerabilityFound.VulnerableParameterValue = HelperGetChildInnerText(n, "vulnerableparametervalue");

                        List<VulnerabilityFound.Reference> ListReferences = new List<VulnerabilityFound.Reference>();
                        foreach (XmlNode nchild in n.ChildNodes)
                        {
                            if (nchild.Name.ToUpper() == "TechnicalDetails".ToUpper() && nchild.ChildNodes != null && nchild.ChildNodes.Count > 0)
                            {
                                //rawrequest
                                vulnerabilityFound.rawrequest = HelperGetChildInnerText(nchild, "Request");
                                //rawresponse
                                vulnerabilityFound.rawresponse = HelperGetChildInnerText(nchild, "Response");
                            }
                            if (nchild.Name.ToUpper() == "References".ToUpper() && nchild.ChildNodes != null && nchild.ChildNodes.Count > 0)
                            {
                                foreach (XmlNode reference in nchild)
                                {
                                    /*
                                    REFERENCE myReference = new REFERENCE();
                                    myReference.Source = HelperGetChildInnerText(reference, "Database");
                                    myReference.Url = HelperGetChildInnerText(reference, "URL");
                                    
                                    model.AddToREFERENCE(myReference);
                                    */

                                    VulnerabilityFound.Reference refvuln= new VulnerabilityFound.Reference();
                                    refvuln.Title = HelperGetChildInnerText(reference, "Database");
                                    string refurl = HelperGetChildInnerText(reference, "URL").ToLower();
                                    refvuln.Url = refurl;
                                    refvuln.Source = HelperGetChildInnerText(reference, "Database");
                                    //Try to harmonise the Source with the other imports (ie: exploits)
                                    //HARDCODED
                                    //TODO: Use a Common Function
                                    if (refurl.Contains("/bugtraq/"))
                                        refvuln.Source = "BUGTRAQ";
                                    if (refurl.Contains("marc.theaimsgroup.com/?l=bugtraq"))
                                        refvuln.Source = "BUGTRAQ";
                                    if(refurl.Contains("securityfocus.com/bid"))
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
                                    ListReferences.Add(refvuln);
                                }
                            }
                        }
                        vulnerabilityFound.ListReference = ListReferences;
                        vulnerabilityFound.ListItem = ListCVEs;
                        vulnerabilityFound.Result = HelperGetChildInnerText(n, "Details");
                        vulnerabilityFound.Consequence = HelperGetChildInnerText(n, "Impact");
                        vulnerabilityFound.Solution = HelperGetChildInnerText(n, "Recommendation");
                        //DetailedInformation
                        vulnerabilityFound.DetailedInformation = HelperGetChildInnerText(n, "DetailedInformation");

                        //TODO
                        bool PatchUpgrade = false;
                        string MSPatch = "";

                                                
                        int etat = VulnerabilityPersistor.Persist(vulnerabilityFound, vulnerabilityEndPoint, m_JobId, "acunetix", model);
                        if (etat == -1)
                        {
                            Utils.Helper_Trace("XORCISM PROVIDER ACUNETIX", string.Format("CANNOT IMPORT THIS ASSET !!!! "));
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
