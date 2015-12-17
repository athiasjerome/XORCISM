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
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

using FSM.DotNetSSH;

namespace XProviderRapid7Import
{
    /// <summary>
    /// Copyright (C) 2012-2015 Jerome Athias
    /// XORCISM Plugin for Rapid7 NeXpose. Parses a report and imports it in XORCISM database
    /// All trademarks and registered trademarks are the property of their respective owners.
    /// This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation; either version 2 of the License, or (at your option) any later version.
    /// 
    /// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
    /// 
    /// You should have received a copy of the GNU General Public License along with this program; if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
    /// </summary>
    //https://community.rapid7.com/docs/DOC-1461    EC2
    public class Engine : MarshalByRefObject, XProviderCommon.IVulnerabilityImporter
    {
        static int inerror = 0;
        public Engine()
        {
            TextWriterTraceListener tw;
            tw = new TextWriterTraceListener("XProviderRapid7Import.log");  //Hardcoded

            Trace.AutoFlush = true;
            Trace.IndentSize = 4;
            Trace.Listeners.Add(tw);
        }

        public void Run(string data, int jobID, int AccountID)
        {
            inerror = 0;
            Utils.Helper_Trace("XORCISM PROVIDER RAPID7 Import", "Entering Run()");
            Utils.Helper_Trace("XORCISM PROVIDER RAPID7 Import", string.Format("Creating an instance of RAPID7Parser for AccountID=" + AccountID.ToString()));

            RAPID7Parser Rapid7Parser = new RAPID7Parser(data, AccountID, jobID);

            Rapid7Parser.DoIt(jobID);
            if (inerror == 0)
            {
                Utils.Helper_Trace("XORCISM PROVIDER RAPID7 Import", string.Format("Updating job {0} status to FINISHED", jobID));

                Rapid7Parser.UpdateJob(jobID);

                XORCISMEntities model = new XORCISMEntities();
                var xJob = from j in model.JOB
                           where j.JobID == jobID
                           select j;

                JOB xJ = xJob.FirstOrDefault();
                xJ.Status = XCommon.STATUS.FINISHED.ToString();

                Utils.Helper_Trace("XORCISM PROVIDER RAPID7 Import", "Changing the session to ServiceCategoryID=1");
                var xSession = from s in model.SESSION
                               where s.SessionID == xJ.SessionID
                               select s;
                SESSION xS = xSession.FirstOrDefault();
                xS.ServiceCategoryID = 1;   //Hardcoded

                model.SaveChanges();

                Utils.Helper_Trace("XORCISM PROVIDER RAPID7 Import", "Leaving Run()");
            }
            inerror = 0;
        }

        /*
        private VULNERABILITYFOUND PersisteVuln(string cve, string RAPID7ID, XmlNode diag, XmlNode consequence, XmlNode solution, int endPointID, string severity)
        {   
            return null;
        }
        */

        private int SearchForRAPID7ID(string RAPID7ID)
        {
            return -1;
        }

        class RAPID7Parser
        {
            private string m_data;
            private int m_AccountID;
            private int m_jobId;

            public RAPID7Parser(string data, int AccountID, int jobid)
            {
                m_AccountID = AccountID;
                m_data = data;
                m_jobId = jobid;
            }

            public void DoIt(int jobID)
            {
                Assembly a;
                a = Assembly.GetExecutingAssembly();

                XORCISMEntities model;
                model = new XORCISMEntities();

                Utils.Helper_Trace("XORCISM PROVIDER RAPID7 Import", "Assembly location = " + a.Location);

                //*************************************************************************************
                //For Web Scan, we have to change the policy to "Web"
                /*
                var CurrentSessionID = from j in model.JOB
                                       where j.JobID==jobID
                                       select j.SessionID;
                var sessioncategory = from s in model.SESSION
                                      where s.SessionID == (int)CurrentSessionID.FirstOrDefault()
                                      select s.ServiceCategoryID;
                if((int)sessioncategory.FirstOrDefault() == 2)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER RAPID7 Import", "Changing Policy to Web");
                    m_policy = "Web";
                }
                */
                //*************************************************************************************

                string URI = string.Empty;
                string UrlScan = string.Empty;

                #region FAKE
                /*
                //URI = @"C:\Rapid7SampleReport.xml";
                URI = @"I:\XORCISM\sources\XAgentHost\bin\Debug\TEST01\NEXPOSE.XML";
                XmlDocument doc = new XmlDocument();
                */
                #endregion

                #region REAL
                
                string SessionID=string.Empty;
                string ScanID=string.Empty;
                string CFG=string.Empty;

                #endregion

                #region PARSE THE REPORT

                // ===================================================
                // Parses the XML Document and populates the database
                // ===================================================

                string protocol = string.Empty;
                int port = -1;
                string service = string.Empty;
                bool PatchUpgrade = false;
                string title;
                string MSPatch = "";
                string Solution;

                XmlDocument doc = new XmlDocument();
                inerror = 0;
                try
                {
                    Utils.Helper_Trace("XORCISM PROVIDER RAPID7 Import", "Loading the XML document");

                    doc.LoadXml(m_data);
                    Utils.Helper_Trace("XORCISM PROVIDER RAPID7 Import", string.Format("JobID: {0} m_data = {1}", m_jobId, m_data));
                }
                catch (Exception ex)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER RAPID7 Import", string.Format("JobID: {0} LoadingXML Exception = {1} / {2}", m_jobId, ex.Message, ex.InnerException == null ? "" : ex.InnerException.Message));
                    Utils.Helper_Trace("XORCISM PROVIDER RAPID7 Import", "RetryingLoading the XML document");
                    try
                    {                        
                        doc.LoadXml(m_data);
                        Utils.Helper_Trace("XORCISM PROVIDER RAPID7 Import", string.Format("JobID: {0} m_data = {1}", m_jobId, m_data));
                    }
                    catch (Exception ex2)
                    {
                        Utils.Helper_Trace("XORCISM PROVIDER RAPID7 Import", string.Format("JobID: {0} RetryingLoadingXML Exception = {1} / {2}", m_jobId, ex2.Message, ex2.InnerException == null ? "" : ex2.InnerException.Message));
                        Utils.Helper_Trace("XORCISM PROVIDER RAPID7 Import", string.Format("XProviderRapid7 1979: Setting job status to ERROR"));
                        //A connection attempt failed because the connected party did not properly respond after a period of time, or established connection failed because connected host has failed to respond 63.38.95.108:443            (Qualys)
                        //XORCISMEntities model;
                        //model = new XORCISMEntities();

                        JOB job;
                        job = model.JOB.FirstOrDefault(o => o.JobID == jobID);

                        job.Status = XCommon.STATUS.ERROR.ToString();
                        job.DateEnd = DateTimeOffset.Now;
                        model.SaveChanges();
                        inerror = 1;
                        
                        XCommon.Utils.Helper_SendEmail("contact@hackenaton.org", "RAPID7 Import ERROR", "RAPID7 ERROR for job:" + m_jobId+" Exception: "+ex2.Message+" "+ex2.InnerException);   //Hardcoded

                        return;
                    }
                    
                }
                if (inerror == 0)
                {

                    #region Parser

                    //XORCISMEntities model;
                    //model = new XORCISMEntities();


                    //We should retrieve the target for an import
                    string m_target = string.Empty;
                    //Hardcoded
                    string patterntoken = "<IP value=(.*?) name=";
                    MatchCollection matchesurl = Regex.Matches(m_data, patterntoken);
                    foreach (Match match in matchesurl)
                    {
                        m_target = match.Value.Replace("<IP value=", "").Replace(" name=", "");
                        m_target = m_target.Replace("\"", "");
                        //Console.WriteLine(mytoken);
                        Utils.Helper_Trace("XORCISM PROVIDER RAPID7 Import", "target: " + m_target);
                    }

                    // ===============================================
                    // If necessary, creates an asset in the database
                    // ===============================================
                    //TODO  ipaddressIPv4
                    var myass = from ass in model.ASSET
                                where ass.ipaddressIPv4 == m_target //&& ass.AccountID == m_AccountID
                                select ass;
                    ASSET asset = myass.FirstOrDefault();

                    if (asset == null)
                    {
                        Utils.Helper_Trace("XORCISM PROVIDER RAPID7 Import", "Creates a new entry in table ASSET for this IP");
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
                        Utils.Helper_Trace("XORCISM PROVIDER RAPID7 Import", "This IP already corresponds to an existing asset");
                    }

                    int m_assetId = asset.AssetID;
                    int m_sessionId = (int)model.JOB.Single(x => x.JobID == m_jobId).SessionID;

                    Utils.Helper_Trace("XORCISM PROVIDER RAPID7 Import", "Creating ASSETINSESSION reference");
                    ASSETSESSION assinsess = new ASSETSESSION();
                    assinsess.AssetID = asset.AssetID;
                    assinsess.SessionID = m_sessionId;  // model.JOB.Single(x => x.JobID == m_jobId).SessionID;
                    model.ASSETSESSION.Add(assinsess);
                    model.SaveChanges();

                    Utils.Helper_Trace("XORCISM PROVIDER RAPID7 Import", "Updates JOB with ASSETINSESSIONID");
                    JOB daJob = model.JOB.Single(x => x.JobID == m_jobId);
                    daJob.AssetSessionID = assinsess.AssetSessionID;
                    model.SaveChanges();





                    string query = "/SCAN/IP/VULNS/CAT";    //Hardcoded

                    XmlNodeList report;
                    report = null;
                    try
                    {
                        report = doc.SelectNodes(query);
                    }
                    catch (Exception ex)
                    {
                        Utils.Helper_Trace("XORCISM PROVIDER RAPID7 Import", string.Format("Error SelectNodes({0}) : Exception = {1}", query, ex.Message));
                    }

                    Utils.Helper_Trace("XORCISM PROVIDER RAPID7 Import", string.Format("JobID: {0} Found {1} hosts to parse", m_jobId, report.Count));

                    foreach (XmlNode reportHost in report)
                    {
                        Utils.Helper_Trace("XORCISM PROVIDER RAPID7 Import", string.Format("Handling host with IP {0}", m_target));

                        // =============================
                        // Handles every ReportItem tag
                        // =============================

                        protocol = string.Empty;
                        port = -1;
                        //Hardcoded
                        if (reportHost.Attributes["protocol"] != null)
                            protocol = reportHost.Attributes["protocol"].InnerText.ToUpper();
                        if (reportHost.Attributes["port"] != null)
                            port = Convert.ToInt32(reportHost.Attributes["port"].InnerText);

                        Utils.Helper_Trace("XORCISM PROVIDER RAPID7 Import", string.Format("Protocol = [{0}]   Port = [{1}]", protocol, port));

                        VulnerabilityEndPoint vulnerabilityEndPoint = new VulnerabilityEndPoint();
                        vulnerabilityEndPoint.IpAdress = m_target;
                        vulnerabilityEndPoint.Protocol = protocol;
                        vulnerabilityEndPoint.Port = port;

                        foreach (XmlNode n in reportHost.ChildNodes)
                        {
                            XmlNodeList Childs = n.ChildNodes;

                            Utils.Helper_Trace("XORCISM PROVIDER RAPID7 Import", string.Format("Coucou 1"));

                            VulnerabilityFound vulnerabilityFound = new VulnerabilityFound();
                            vulnerabilityFound.ListItem = Helper_GetCVE(n);
                            vulnerabilityFound.ListReference = Helper_GetREFERENCE(n);  //TODO: Helper_GetCVE and Helper_GetREFERENCE could be mixed for only 1 parsing
                            vulnerabilityFound.InnerXml = n.OuterXml;
                            //Hardcoded
                            vulnerabilityFound.Description = HelperGetChildInnerText(n, "DIAGNOSIS");
                            vulnerabilityFound.Solution = HelperGetChildInnerText(n, "SOLUTION");
                            vulnerabilityFound.Severity = n.Attributes["severity"].Value;
                            vulnerabilityFound.Consequence = HelperGetChildInnerText(n, "CONSEQUENCE");
                            string tempoResult = HelperGetChildInnerText(n, "RESULT");
                            vulnerabilityFound.Result = tempoResult;
                            //vulnerabilityFound.ModifiedDate = HelperGetChildInnerText(n, "LAST_UPDATE");
                            if (HelperGetChildInnerText(n, "PCI_FLAG") == "1")
                            {
                                vulnerabilityFound.PCI_FLAG = true;
                            }
                            string tempoTitle=HelperGetChildInnerText(n, "TITLE");
                            vulnerabilityFound.Title = tempoTitle;
                            if (tempoTitle == "SQL Injection Vulnerability" || tempoTitle == "Cross Site Scripting Vulnerability") //"Web"
                            {
                                /*
                                <RESULT><![CDATA[<p>
                                <p>Injected into the &quot;txtAnnualIncome&quot; form parameter on 
                                <a href="http://crackme.cenzic.com/Kelev/view/updateloanrequest.php">http://crackme.cenzic.com/Kelev/view/updateloanrequest.php</a>: </p>
                                <pre>125: 				&lt;td&gt;&amp;nbsp;&lt;/td&gt;
                                126: 			&lt;/tr&gt;
                                127: 			&lt;tr&gt;
                                128: 				&lt;td&gt;&amp;nbsp;&lt;/td&gt;
                                129: ...BodyGeneral&quot;&gt;	<DIV CLASS="highlight">You have an error in your SQL syntax</DIV>.  Check the manu...</pre></p>]]></RESULT>
                                */
                                try
                                {
                                    Regex RegexPattern = new Regex(@"<a.*?href=[""'](.*?)[""'].*?>", RegexOptions.Singleline);
                                    string myURL = RegexPattern.Match(tempoResult).ToString().Replace("<a href=\"", "").Replace("\">", "");
                                    Utils.Helper_Trace("XORCISM PROVIDER RAPID7 Import", string.Format("Extracted url = {0}", myURL));
                                    vulnerabilityFound.Url = myURL;

                                    //VulnerableParameter
                                    string pattern = "Injected into the &quot;(.*?)&quot; form parameter on";   //Hardcoded
                                    MatchCollection matches = Regex.Matches(tempoResult, pattern);
                                    foreach (Match match in matches)
                                    {
                                        vulnerabilityFound.VulnerableParameter = match.Value.Replace("Injected into the &quot;", "").Replace("&quot; form parameter on", "");
                                        vulnerabilityFound.VulnerableParameterType = "Post";
                                    }
                                    vulnerabilityFound.rawresponse = tempoResult; //Details
                                    
                                }
                                catch (Exception xx)
                                {

                                }
                            }

                            if (tempoTitle == "Blind SQL Injection")
                            {
                                /*
                                <RESULT><![CDATA[<p>
                                <p>
                                <p>Found blind SQL injection on 
                                <a href="http://www.webscantest.com/shutterdb/search_by_name.php">http://www.webscantest.com/shutterdb/search_by_name.php</a> using method POST</p>
                                <pre>Parameter <DIV CLASS="highlight">name</DIV> behaves differently with the following payloads:</pre>
                                <ul>
                                <li><DIV CLASS="highlight">Rake&#39;&#39; OR 1=1 #&#39; OR &#39;76450&#39;=&#39;76450</DIV></li>
                                <li><DIV CLASS="highlight">Rake&#39;&#39; OR 1=1 #&#39; AND &#39;76450&#39;=&#39;76451</DIV></li></ul></p></p>]]></RESULT>
                                */
                                Regex RegexPattern = new Regex(@"<a.*?href=[""'](.*?)[""'].*?>", RegexOptions.Singleline);
                                string myURL = RegexPattern.Match(tempoResult).ToString().Replace("<a href=\"", "").Replace("\">", "");
                                Utils.Helper_Trace("XORCISM PROVIDER RAPID7 Import", string.Format("Extracted url = {0}", myURL));
                                vulnerabilityFound.Url = myURL;

                                //VulnerableParameter
                                string pattern = "Parameter <DIV CLASS=\"highlight\">(.*?)</DIV> behaves differently";
                                MatchCollection matches = Regex.Matches(tempoResult, pattern);
                                foreach (Match match in matches)
                                {
                                    vulnerabilityFound.VulnerableParameter = match.Value.Replace("Parameter <DIV CLASS=\"highlight\">", "").Replace("</DIV> behaves differently", "");                                    
                                }
                                if (tempoTitle.Contains("using method POST"))
                                {
                                    vulnerabilityFound.VulnerableParameterType = "Post";
                                }
                                else
                                {
                                    vulnerabilityFound.VulnerableParameterType = "Get";
                                }
                                vulnerabilityFound.rawresponse = tempoResult; //Details
                            }

                            try
                            {
                                //string myCVSS = HelperGetChildInnerText(n, "CVSS_BASE").ToString("#,##0.00");
                                vulnerabilityFound.CVSSBaseScore = float.Parse(HelperGetChildInnerText(n, "CVSS_BASE"), System.Globalization.CultureInfo.InvariantCulture);
                            }
                            catch (Exception ex)
                            {
                                Utils.Helper_Trace("XORCISM PROVIDER RAPID7 Import", string.Format("Error parsing CVSS_BASE : Exception = {0}", ex.Message));
                            }

                            #region JEROME
                            PatchUpgrade = false;
                            title = HelperGetChildInnerText(n, "TITLE");
                            MSPatch = "";
                            Regex objNaturalPattern = new Regex("MS[0-9][0-9]-[0-9][0-9][0-9]");
                            MSPatch = objNaturalPattern.Match(title).ToString();
                            if (MSPatch != "")
                            {
                                Utils.Helper_Trace("XORCISM PROVIDER RAPID7 Import", "MSPatch=" + MSPatch);
                                PatchUpgrade = true;
                            }
                            else
                            {
                                //HARDCODED
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
                                if (Solution.Contains("Download and apply the upgrade"))
                                {
                                    PatchUpgrade = true;
                                }
                                //////////////////////////
                                //if (PatchUpgrade)
                                //{
                                //    Utils.Helper_Trace("XORCISM PROVIDER RAPID7 Import", title);
                                //}
                            }

                            if (PatchUpgrade)
                            {
                                Utils.Helper_Trace("XORCISM PROVIDER RAPID7 Import", "PatchUpgrade");
                            }
                            else
                            {
                                //    Utils.Helper_Trace("XORCISM PROVIDER RAPID7 Import", "Solution: " + Solution);
                            }
                            vulnerabilityFound.PatchUpgrade = PatchUpgrade;
                            vulnerabilityFound.MSPatch = MSPatch;
                            #endregion


                            // ===========
                            // Persistance
                            // ===========

                            Utils.Helper_Trace("XORCISM PROVIDER RAPID7 Import", string.Format("JobID: {0} Persistance [{1}] [{2}] [{3}]", m_jobId, protocol, port, Helper_ListCVEToString(vulnerabilityFound.ListItem)));
                            int VulnID = VulnerabilityPersistor.Persist(vulnerabilityFound, vulnerabilityEndPoint, m_jobId, "Rapid7", model);

                            //Mapping with CWE
                            /*
                            if (tempoTitle == "Blind SQL Injection")
                            {
                                CWE newCWE = new CWE();
                                newCWE.CWEID = "";
                                newCWE.Title = "";
                                newCWE.VULNERABILITY
                            }
                            */
                            var xTmpJob = from j in model.JOB
                                          where j.JobID == m_jobId
                                          select j;

                            JOB xJob = xTmpJob.FirstOrDefault();
                            if (xJob.SESSION.ServiceCategoryID == 4) // Compliance
                            {
                                #region Persists Compliances
                                List<int> Compliances = new List<int>();

                                Utils.Helper_Trace("XORCISM PROVIDER RAPID7 Import", string.Format("Node xml ==> {0}", n.InnerText));

                                Compliances = GetCompliance(n.InnerXml, reportHost.Attributes["value"].InnerText);
                                Utils.Helper_Trace("XORCISM PROVIDER RAPID7 Import", string.Format("Vulnerability persisted , VulnID = {0} & Compliance count = {1}", VulnID, Compliances.Count));

                                //TODO
                                /*
                                var V = from tmpVuln in model.VULNERABILITYFOUND
                                        where tmpVuln.VulnerabilityFoundID == VulnID
                                        select tmpVuln;

                                VULNERABILITYFOUND VF = V.FirstOrDefault();

                                foreach (int i in Compliances)
                                {
                                    Utils.Helper_Trace("XORCISM PROVIDER RAPID7 Import", string.Format("Categorie Compliance => ", i));
                                    var C = from Comp in model.COMPLIANCECATEG
                                            where Comp.ComplianceCategID == i
                                            select Comp;

                                    COMPLIANCECATEG myCompliance = new COMPLIANCECATEG();
                                    myCompliance = C.FirstOrDefault();

                                    VF.COMPLIANCECATEG.Add(myCompliance);

                                    model.SaveChanges();
                                    Utils.Helper_Trace("XORCISM PROVIDER RAPID7 Import", "Mapping Compliance-Vulnerability Added");
                                }
                                */
                                #endregion
                            }
                        }
                    }
                    #endregion

                    #region NeXposeSimpleXMLParser
                    /*
                //devices
                string query = "/NeXposeSimpleXML/devices/device";
                XmlNodeList report;
                report = doc.SelectNodes(query);
                foreach (XmlNode reportHost in report)
                {
                    if (reportHost.Name.ToUpper() == "DESCRIPTION")
                    {
                        Utils.Helper_Trace("XORCISM PROVIDER RAPID7 Import", "OS = " + reportHost.InnerText.Trim());
                    }
                }

                query = "/NeXposeSimpleXML/devices/device/vulnerabilities/vulnerability";   //Hardcoded
                report = doc.SelectNodes(query);

                foreach (XmlNode reportHost in report)
                {
                    // ===========================
                    // Handle every ReportItem tag
                    // ===========================

                    protocol = string.Empty;
                    port = -1;
                    service = string.Empty;

                    VulnerabilityEndPoint vulnerabilityEndPoint = new VulnerabilityEndPoint();
                    vulnerabilityEndPoint.IpAdress = m_target;
                    vulnerabilityEndPoint.Protocol = protocol;
                    vulnerabilityEndPoint.Port = port;
                    vulnerabilityEndPoint.Service = service;

                    VulnerabilityFound vulnerabilityFound = new VulnerabilityFound();
                    vulnerabilityFound.ListItem = Helper_GetCVE(reportHost);
                    vulnerabilityFound.InnerXml = reportHost.OuterXml;
                    vulnerabilityFound.Description = reportHost.Attributes["id"].InnerText;
                    vulnerabilityFound.Solution = "";

                    #region JEROME

                    PatchUpgrade = false;
                    title = reportHost.Attributes["id"].InnerText;
                    MSPatch = "";
                    Regex objNaturalPattern = new Regex("MS[0-9][0-9]-[0-9][0-9][0-9]");
                    MSPatch = objNaturalPattern.Match(title).ToString();
                    if (MSPatch != "")
                    {
                        Utils.Helper_Trace("XORCISM PROVIDER RAPID7 Import", "MSPatch=" + MSPatch);
                        PatchUpgrade = true;
                    }
                    vulnerabilityFound.PatchUpgrade = PatchUpgrade;
                    vulnerabilityFound.MSPatch = MSPatch;
                    #endregion

                    // ===========
                    // Persistance
                    // ===========

                    VulnerabilityPersistor.Persist(vulnerabilityFound, vulnerabilityEndPoint, m_jobId, "Rapid7", model);
                }

                //SERVICES
                query = "/NeXposeSimpleXML/devices/device/services/service";        //Hardcoded         
                report = doc.SelectNodes(query);

                foreach (XmlNode reportHost in report)
                {
                    protocol = string.Empty;
                    port = -1;
                    service = string.Empty;
                    PatchUpgrade = false;
                    MSPatch = "";
                    VulnerabilityEndPoint vulnerabilityEndPoint = new VulnerabilityEndPoint();
                    try
                    {                        
                        vulnerabilityEndPoint.IpAdress = m_target;
                        vulnerabilityEndPoint.Protocol = reportHost.Attributes["protocol"].InnerText;
                        vulnerabilityEndPoint.Port = Convert.ToInt32(reportHost.Attributes["port"].InnerText);
                        vulnerabilityEndPoint.Service = reportHost.Attributes["name"].InnerText;
                    }
                    catch (Exception ex)
                    {
                        Utils.Helper_Trace("XORCISM PROVIDER RAPID7 Import", string.Format("Error in Services-vulnerabilityEndPoint : Exception = {0}", ex.Message));
                    }

                    VulnerabilityFound vulnerabilityFound = new VulnerabilityFound();
                    try
                    {
                        vulnerabilityFound.ListItem = Helper_GetCVE(reportHost);
                        vulnerabilityFound.InnerXml = reportHost.OuterXml;
                        if (reportHost.Attributes["id"] != null)
                        {
                            vulnerabilityFound.Description = reportHost.Attributes["id"].InnerText;
                            title = reportHost.Attributes["id"].InnerText;

                            Regex objNaturalPattern = new Regex("MS[0-9][0-9]-[0-9][0-9][0-9]");
                            MSPatch = objNaturalPattern.Match(title).ToString();
                            if (MSPatch != "")
                            {
                                Utils.Helper_Trace("XORCISM PROVIDER RAPID7 Import", "MSPatch=" + MSPatch);
                                PatchUpgrade = true;
                            }
                        }
                        else
                        {
                            vulnerabilityFound.Description = "";
                        }
                        vulnerabilityFound.Solution = "";
                    }
                    catch (Exception ex)
                    {
                        Utils.Helper_Trace("XORCISM PROVIDER RAPID7 Import", string.Format("Error in Services-vulnerabilityFound : Exception = {0}", ex.Message));
                    }
                    #region JEROME
                    
                    vulnerabilityFound.PatchUpgrade = PatchUpgrade;
                    vulnerabilityFound.MSPatch = MSPatch;
                    #endregion

                    // ===========
                    // Persistance
                    // ===========

                    VulnerabilityPersistor.Persist(vulnerabilityFound, vulnerabilityEndPoint, m_jobId, "Rapid7", model);
                }
                */
                    #endregion

                    #endregion

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

            private List<int> GetCompliance(string xml, string ValTitle)
            {
                List<int> myIds = new List<int>();
                //Hardcoded
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
                    //TODO
                    /*
                    if (n.InnerText == "1")
                    {
                        var id = from o in model.COMPLIANCECATEG
                                 where o.Title == ValTitle &&
                                 o.COMPLIANCE.Title == "PCIDSS"
                                 select o.ComplianceCategID;
                        int Id = id.FirstOrDefault();

                        myIds.Add(Id);
                        //MessageBox.Show("[" + ValTitle + "] Id Compliance Categorie = " + Id);
                    }
                    */
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
                            //TODO Translation
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

            //private string HelperGetChildInnerText(XmlNode n, string ChildName)
            //{
            //    foreach (XmlNode child in n.ChildNodes)
            //    {
            //        if (child.Name.ToUpper() == ChildName.ToUpper())
            //            return child.InnerText;
            //    }

            //    return string.Empty;
            //}

            private List<VulnerabilityFound.Item> Helper_GetCVE(XmlNode node)
            {                
                List<VulnerabilityFound.Item> l;
                l = new List<VulnerabilityFound.Item>();
                try
                {
                    string myCVEID = "";
                    XmlNodeList nodes = node.ChildNodes;    //Childs of CAT/VULN
                    foreach (XmlNode n in nodes)
                    {
                        /*
                        if (n.Attributes["type"] != null)
                        {
                            VulnerabilityFound.Item item = new VulnerabilityFound.Item();
                            item.ID = n.Attributes["type"].InnerText;
                            item.Value = n.InnerText;
                            l.Add(item);
                        }
                        */
                        
                            if (n.Name.ToUpper() == "CVE_ID_LIST")
                            {
                                Utils.Helper_Trace("XORCISM PROVIDER RAPID7 Import", string.Format("TEST CVE_ID_LIST"));
                                XmlNodeList mycves = n.ChildNodes;
                                foreach (XmlNode x in mycves)
                                {
                                    myCVEID = HelperGetChildInnerText(x, "ID");
                                    Utils.Helper_Trace("XORCISM PROVIDER RAPID7 Import", string.Format("TEST CVE: {0}", myCVEID));
                                    Utils.Helper_Trace("XORCISM PROVIDER RAPID7 Import", string.Format("TEST CVE2: {0}", x.InnerText));
                                    VulnerabilityFound.Item item = new VulnerabilityFound.Item();
                                    item.ID = "cve";
                                    item.Value = myCVEID;
                                    l.Add(item);
                                }
                            }                    
                        

                    }                    
                }
                catch (Exception ex)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER RAPID7 Import", string.Format("Error in Helper_GetCVE : Exception = {0}", ex.Message));
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
                    if (child.Name.ToUpper() == "VENDOR_REFERENCE_LIST")
                    {
                        Utils.Helper_Trace("XORCISM PROVIDER RAPID7 Import", "VENDOR_REFERENCE_LIST");
                        foreach (XmlNode noderef in child.ChildNodes)
                        {
                            //HARDCODED
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
                            Utils.Helper_Trace("XORCISM PROVIDER RAPID7 Import", string.Format("TEST REFERENCE: {0}", refurl));
                        }
                    }
                    else
                    {
                        if (child.Name.ToUpper() == "BUGTRAQ_ID_LIST")
                        {
                            Utils.Helper_Trace("XORCISM PROVIDER RAPID7 Import", "BUGTRAQ_ID_LIST");
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
                                Utils.Helper_Trace("XORCISM PROVIDER RAPID7 Import", string.Format("TEST REFERENCEBID: {0}", refurl));
                            }
                        }
                    }
                }

                return l;
            }



            public void UpdateJob(int JobId)
            {
                try
                {
                    XORCISMEntities model = new XORCISMEntities();
                    var Q = from o in model.JOB
                            where o.JobID == JobId
                            select o;
                    JOB myJob = Q.FirstOrDefault();
                    myJob.Status = XCommon.STATUS.FINISHED.ToString();
                    myJob.DateEnd = DateTimeOffset.Now;
                    //image
                    System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
                    myJob.XmlResult = encoding.GetBytes(m_data);
                    model.SaveChanges();
                    //FREE
                    model.Dispose();
                    model = null;

                    
                }
                catch (Exception ex)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER RAPID7 Import", "Exception = " + ex.Message);                    
                }
            }

        }

        internal class AcceptAllCertificatePolicy : ICertificatePolicy
        {
            public AcceptAllCertificatePolicy()
            {
            }

            public bool CheckValidationResult(ServicePoint sPoint, X509Certificate cert, WebRequest wRequest, int certProb)
            {
                //TODO
                // *** Always accepts !!!
                return true;
            }
        }
    }
}
