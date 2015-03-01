using System;
using System.Net;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Threading;

using System.Text.RegularExpressions;

using XORCISMModel;
using XCommon;
using XProviderCommon;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace XProviderNessusScan
{
    public class Engine : MarshalByRefObject, XProviderCommon.IVulnerabilityDetector
    {
        /// <summary>
        /// Copyright (C) 2012-2015 Jerome Athias
        /// XORCISM Plugin for Tenable Nessus. Allows to launch scans on a remote Nessus scanner instance. Parse a Nessus report and import it in XORCISM database
        /// All trademarks and registered trademarks are the property of their respective owners.
        /// This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation; either version 2 of the License, or (at your option) any later version.
        /// 
        /// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
        /// 
        /// You should have received a copy of the GNU General Public License along with this program; if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
        /// </summary>
        
        public Engine()
        {
            TextWriterTraceListener tw;
            tw = new TextWriterTraceListener("XProviderNessusScan.log");
            Trace.Listeners.Add(tw);
            Trace.AutoFlush = true;
            Trace.IndentSize = 4;
        }

        //public void Run(string data, int jobID, int AccountID)
        public void Run(string target, int jobID, string policy, string strategy)
        {
            //WARNING: OLD CODE, should be reviewed/revised - JA

            Utils.Helper_Trace("XORCISM PROVIDER NESSUS SCAN", "JobID:" + jobID + "Entering Run()");

            //Utils.Helper_Trace("XORCISM PROVIDER NESSUS SCAN", string.Format("Creating an instance of NessusParser for AccountID="+AccountID.ToString()));
            Utils.Helper_Trace("XORCISM PROVIDER NESSUS SCAN", string.Format("Target = {0} , JobID = {1} , Policy = {2}, Strategy = {3}", target, jobID, policy, strategy));

            //NessusParser NessusParser = new NessusParser(data,AccountID,jobID);
            NessusParser NessusParser = new NessusParser(target, jobID, policy, strategy);

            Utils.Helper_Trace("XORCISM PROVIDER NESSUS SCAN", string.Format("JobID:" + jobID + " Parsing the data"));

            NessusParser.parse();

            Utils.Helper_Trace("XORCISM PROVIDER NESSUS SCAN", "JobID:" + jobID + "Updating job status to FINISHED");

            XORCISMEntities model = new XORCISMEntities();
            var xJob = from j in model.JOB
                       where j.JobID == jobID
                       select j;

            JOB xJ = xJob.FirstOrDefault();
            xJ.Status = XCommon.STATUS.FINISHED.ToString();
            
            model.SaveChanges();
            Utils.Helper_Trace("XORCISM PROVIDER NESSUS SCAN", string.Format("End of data processing"));

            Utils.Helper_Trace("XORCISM PROVIDER NESSUS SCAN", "Leaving Run()");
        }
    }

    class NessusParser
    {
        private string m_data;
        private int m_AccountID;
        private int m_JobId;

        //public NessusParser(string data, int AccountID, int jobid)
        public NessusParser(string target, int jobID, string policy, string strategy)
        {
            string Url = "https://CHANGEME.svc.nessus.org"; //HARDCODED

            Stream stream;
            long seqint = DateTime.Now.Ticks;
            string snessus = "login=jerome@xorcism.org&password=changeme&seq=" + seqint;   //TODO Hardcoded
            byte[] buffer;
            buffer = Encoding.UTF8.GetBytes(snessus);
            HttpWebRequest req;
            req = (HttpWebRequest)WebRequest.Create(Url + "/login");
            ////ServicePointManager.CertificatePolicy = new AcceptAllCertificatePolicy();
            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate);

            req.Method = "POST";
            req.ContentLength = buffer.Length;
            req.ContentType = "application/x-www-form-urlencoded";
            stream = req.GetRequestStream();
            stream.Write(buffer, 0, buffer.Length);
            stream.Close();

            HttpWebResponse resp;
            resp = (HttpWebResponse)req.GetResponse();
            StreamReader SR = new StreamReader(resp.GetResponseStream());
            string ResponseTextNessus = SR.ReadToEnd();

            XmlDocument docnessus = new XmlDocument();
            //Console.WriteLine("DEBUG ResponseTextNessus="+ResponseTextNessus);
            
            Utils.Helper_Trace("XORCISM PROVIDER NESSUS SCAN", "JobID:"+jobID+" LoginResponse: "+ResponseTextNessus);

            string mytoken = string.Empty;
            string patterntoken = "<token>(.*?)</token>";
            MatchCollection matchestoken = Regex.Matches(ResponseTextNessus, patterntoken);
            foreach (Match match in matchestoken)
            {
                mytoken = match.Value.Replace("<token>", "").Replace("</token>", "");
                //Console.WriteLine(mytoken);
                Utils.Helper_Trace("XORCISM PROVIDER NESSUS SCAN", "JobID:" + jobID + " token: " + mytoken);
            }

            //***************************************************************************************************************
            //List the policies
            //***************************************************************************************************************
            /*
            
            */
            //return;


            //***************************************************************************************************************
            //Launch the scan
            //***************************************************************************************************************
            XORCISMEntities model;
            model = new XORCISMEntities();
            var sessionidjob = from j in model.JOB
                               where j.JobID == jobID
                               select j.SessionID;

            int sessidjob = (int)sessionidjob.FirstOrDefault();

            var servicecat = from s in model.SESSION
                             where s.SessionID == sessidjob
                             select s.ServiceCategoryID;
            string servicecategory = servicecat.FirstOrDefault().ToString();

            string MyPolicy;
            MyPolicy = string.Empty;
            //HARDCODED
            if (servicecategory == "1")
            {
                MyPolicy = "1"; //Perimeter Scan (fast)
            }
            else
            {
                MyPolicy = "3"; //Web App Tests
            }
            switch (policy)
            {
                case "Normal":
                    //MyPolicy = "network-audit";
                    break;
                case "Moderate":
                    //MyPolicy = "full-audit";
                    break;
                case "Intrusive":
                    //MyPolicy = "exhaustive-audit";
                    if (servicecategory == "1")
                    {
                        MyPolicy = "2"; //Perimeter Scan (exhaustive)
                    }
                    else
                    {
                        MyPolicy = "4"; //Web App Tests (exhaustive)
                    }
                    break;
                case "Pentest":
                    //MyPolicy = "pentest-audit";
                    if (servicecategory == "1")
                    {
                        MyPolicy = "2"; //Perimeter Scan (exhaustive)
                    }
                    else
                    {
                        MyPolicy = "4"; //Web App Tests (exhaustive)
                    }
                    break;
                case "Web":
                    //MyPolicy = "web-audit"; //internet-audit
                    if (servicecategory == "1")
                    {
                        MyPolicy = "2"; //Perimeter Scan (exhaustive)
                    }
                    else
                    {
                        MyPolicy = "4"; //Web App Tests (exhaustive)
                    }
                    break;
                /*
                case "PCI DSS":
                    MyPolicy = "pci-audit";
                    break;
                case "HIPAA":
                    MyPolicy = "hipaa-audit";
                    break;
                case "SOX":
                    MyPolicy = "sox-audit";
                    break;
                case "SCADA":
                    MyPolicy = "scada";
                    break;
                */
                default:
                    //MyPolicy = "network-audit";
                    break;
                //pentest-audit
                //discovery
                //aggressive-discovery
                //dos-audit
            }

            seqint = DateTime.Now.Ticks;
            snessus = "target=" + target + "&scan_name=" + seqint + "&seq=" + seqint + "&token=" + mytoken + "&policy_id=" + MyPolicy;
            buffer = Encoding.UTF8.GetBytes(snessus);
            req = (HttpWebRequest)WebRequest.Create(Url + "/scan/new");
            ////ServicePointManager.CertificatePolicy = new AcceptAllCertificatePolicy();
            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate);

            req.Method = "POST";
            req.ContentLength = buffer.Length;
            req.ContentType = "application/x-www-form-urlencoded";
            stream = req.GetRequestStream();
            stream.Write(buffer, 0, buffer.Length);
            stream.Close();

            resp = (HttpWebResponse)req.GetResponse();
            SR = new StreamReader(resp.GetResponseStream());
            ResponseTextNessus = SR.ReadToEnd();

            docnessus = new XmlDocument();
            //Console.WriteLine(ResponseTextNessus);
            /*
            
            */
            /*
            docnessus.Load(ResponseTextNessus);

            Utils.Helper_Trace("XORCISM PROVIDER NESSUS SCAN", string.Format("NewScanResponse received : {0}", docnessus.OuterXml));
            Console.WriteLine("DEBUG="+docnessus.OuterXml);
            */


            //***************************************************************************************************************
            //List the running scans
            //***************************************************************************************************************

            //get the uuid of the scan with the 1st request
            Thread.Sleep(30000);

            //            seqint = DateTime.Now.Ticks;
            snessus = "&token=" + mytoken;
            buffer = Encoding.UTF8.GetBytes(snessus);
            req = (HttpWebRequest)WebRequest.Create(Url + "/scan/list");
            ////ServicePointManager.CertificatePolicy = new AcceptAllCertificatePolicy();
            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate);

            req.Method = "POST";
            req.ContentLength = buffer.Length;
            req.ContentType = "application/x-www-form-urlencoded";
            stream = req.GetRequestStream();
            stream.Write(buffer, 0, buffer.Length);
            stream.Close();

            resp = (HttpWebResponse)req.GetResponse();
            SR = new StreamReader(resp.GetResponseStream());
            ResponseTextNessus = SR.ReadToEnd();
            //Search the order of the current scan
            patterntoken = "<readableName>(.*?)</readableName>";
            matchestoken = Regex.Matches(ResponseTextNessus, patterntoken);
            string scanmatch = string.Empty;
            int scanorder = 0;
            int myscanorder = 0;
            foreach (Match match in matchestoken)
            {
                scanorder++;
                scanmatch = match.Value.Replace("<readableName>", "").Replace("</readableName>", "");
                if (scanmatch == seqint.ToString())
                {
                    myscanorder = scanorder;
                }
            }

            patterntoken = "<uuid>(.*?)</uuid>";
            matchestoken = Regex.Matches(ResponseTextNessus, patterntoken);
            string scanuuid = string.Empty;
            scanorder = 0;
            foreach (Match match in matchestoken)
            {
                scanorder++;
                if (scanorder == myscanorder)
                {
                    scanmatch = match.Value.Replace("<uuid>", "").Replace("</uuid>", "");
                    scanuuid = scanmatch;
                }
            }
            
            //Wait until the scan is completed
            //TODO: optimize this
            bool scanfinished = false;
            while (scanfinished != true)
            {
                //Console.WriteLine("XORCISM PROVIDER NESSUS SCAN", string.Format("Polling..."));
                Utils.Helper_Trace("XORCISM PROVIDER NESSUS SCAN", "Jobid=" + jobID + " Polling...");

                Thread.Sleep(30000);

                //            seqint = DateTime.Now.Ticks;
                snessus = "&token=" + mytoken;
                buffer = Encoding.UTF8.GetBytes(snessus);
                req = (HttpWebRequest)WebRequest.Create(Url + "/scan/list");
                ////ServicePointManager.CertificatePolicy = new AcceptAllCertificatePolicy();
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate);

                req.Method = "POST";
                req.ContentLength = buffer.Length;
                req.ContentType = "application/x-www-form-urlencoded";
                stream = req.GetRequestStream();
                stream.Write(buffer, 0, buffer.Length);
                stream.Close();

                resp = (HttpWebResponse)req.GetResponse();
                SR = new StreamReader(resp.GetResponseStream());
                ResponseTextNessus = SR.ReadToEnd();
                if (ResponseTextNessus.Contains(seqint.ToString()))
                {

                }
                else
                {
                    scanfinished = true;
                }

            }
            //Console.WriteLine("NESSUS SCAN FINISHED");
            Utils.Helper_Trace("XORCISM PROVIDER NESSUS SCAN", "Jobid=" + jobID + " NESSUS SCAN FINISHED");


            //***************************************************************************************************************
            //List the reports
            //***************************************************************************************************************

            //            seqint = DateTime.Now.Ticks;
            snessus = "&token=" + mytoken;
            buffer = Encoding.UTF8.GetBytes(snessus);
            req = (HttpWebRequest)WebRequest.Create(Url + "/report/list");
            ////ServicePointManager.CertificatePolicy = new AcceptAllCertificatePolicy();
            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate);

            req.Method = "POST";
            req.ContentLength = buffer.Length;
            req.ContentType = "application/x-www-form-urlencoded";
            stream = req.GetRequestStream();
            stream.Write(buffer, 0, buffer.Length);
            stream.Close();

            resp = (HttpWebResponse)req.GetResponse();
            SR = new StreamReader(resp.GetResponseStream());
            ResponseTextNessus = SR.ReadToEnd();

            docnessus = new XmlDocument();
            


            //***************************************************************************************************************
            //Download the report
            //***************************************************************************************************************

            //            seqint = DateTime.Now.Ticks;
            snessus = "&token=" + mytoken;
            buffer = Encoding.UTF8.GetBytes(snessus);
            req = (HttpWebRequest)WebRequest.Create(Url + "/file/report/download/?report=" + scanuuid);
            ////ServicePointManager.CertificatePolicy = new AcceptAllCertificatePolicy();
            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate);

            req.Method = "POST";
            req.ContentLength = buffer.Length;
            req.ContentType = "application/x-www-form-urlencoded";
            stream = req.GetRequestStream();
            stream.Write(buffer, 0, buffer.Length);
            stream.Close();

            resp = (HttpWebResponse)req.GetResponse();
            SR = new StreamReader(resp.GetResponseStream());
            ResponseTextNessus = SR.ReadToEnd();

            //docnessus = new XmlDocument();
            //Console.WriteLine(ResponseTextNessus);
            StreamWriter monStreamWriter = new StreamWriter("nessus_report" + seqint + ".nessus");
            monStreamWriter.Write(ResponseTextNessus);
            monStreamWriter.Close();
            Utils.Helper_Trace("XORCISM PROVIDER NESSUS SCAN", "JobID:"+jobID+" Result saved in nessus_report" + seqint + ".nessus");
            string data = ResponseTextNessus;
            Utils.Helper_Trace("XORCISM PROVIDER NESSUS SCAN", "JobID:" + jobID + " data=" + data);
            

            //***************************************************************************************************************
            //Logout
            //***************************************************************************************************************
            seqint = DateTime.Now.Ticks;
            snessus = "token=" + mytoken;
            buffer = Encoding.UTF8.GetBytes(snessus);
            req = (HttpWebRequest)WebRequest.Create(Url + "/logout");
            ////ServicePointManager.CertificatePolicy = new AcceptAllCertificatePolicy();
            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate);

            req.Method = "POST";
            //req.Headers.Add("Cookie", "token=" + mytoken);
            req.ContentLength = buffer.Length;
            req.ContentType = "application/x-www-form-urlencoded";
            stream = req.GetRequestStream();
            stream.Write(buffer, 0, buffer.Length);
            stream.Close();

            resp = (HttpWebResponse)req.GetResponse();
            SR = new StreamReader(resp.GetResponseStream());
            ResponseTextNessus = SR.ReadToEnd();
            //Console.WriteLine("LogoutResponse=" + ResponseTextNessus);
            Utils.Helper_Trace("XORCISM PROVIDER NESSUS SCAN", "Jobid="+jobID+ " LogoutResponse=" + ResponseTextNessus);
            
            
            m_data = data;
            m_JobId = jobID;



        }

        

        public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        public void parse() //ParseNessusReport()
        {
            Assembly a;
            a = Assembly.GetExecutingAssembly();

            Utils.Helper_Trace("XORCISM PROVIDER NESSUS SCAN", "Assembly location = " + a.Location);

            // ============================================
            // Parse the XML Document and feed the database
            // ============================================
            
            XmlDocument doc = new XmlDocument();

            doc.LoadXml(m_data);

            XORCISMEntities model;
            model = new XORCISMEntities();
        
            string query = "/NessusClientData_v2/Report";   //HARDCODED

            XmlNode report;
            report = doc.SelectSingleNode(query);

            Utils.Helper_Trace("XORCISM PROVIDER NESSUS SCAN", string.Format("Found {0} hosts to parse", report.ChildNodes.Count));

            foreach (XmlNode reportHost in report.ChildNodes)
            {
                string ipAddress;
                ipAddress = reportHost.Attributes["name"].InnerText;

                Utils.Helper_Trace("XORCISM PROVIDER NESSUS SCAN", string.Format("Handling host with IP {0}", ipAddress));
                
                // =============================================
                // If necessary, create an asset in the database
                // =============================================
                //TODO
                var myass = from ass in model.ASSET
                          where ass.ipaddressIPv4 == ipAddress //&& ass.AccountID == m_AccountID
                          select ass;
                ASSET asset = myass.FirstOrDefault();

                if (asset == null)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER NESSUS SCAN", "Create a new entry in the ASSET table for this IP Address");

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
                    Utils.Helper_Trace("XORCISM PROVIDER NESSUS SCAN", "This IP already corresponds to an existing asset");                    
                }

                
                // ===========================
                // Handle every ReportItem tag
                // ===========================
   
                foreach (XmlNode n in reportHost.ChildNodes)
                {
                    if (n.Name.ToUpper() == "ReportItem".ToUpper() && n.ChildNodes != null && n.ChildNodes.Count > 0)
                    {                            
                        string protocol = n.Attributes["protocol"].InnerText.ToUpper();
                        int port        = Convert.ToInt32(n.Attributes["port"].InnerText);
                        //svc_name
                        //pluginID
                        //pluginName
                        //pluginFamily
                        //risk_factor

                        VulnerabilityEndPoint vulnerabilityEndPoint = new VulnerabilityEndPoint();
                        vulnerabilityEndPoint.IpAdress  = ipAddress;
                        vulnerabilityEndPoint.Protocol  = protocol;
                        vulnerabilityEndPoint.Port      = port;

                        VulnerabilityFound vulnerabilityFound = new VulnerabilityFound();
                        vulnerabilityFound.ListItem     = Helper_GetCVE(n);
                        vulnerabilityFound.ListReference = Helper_GetREFERENCE(n);  //TODO: Helper_GetCVE and Helper_GetREFERENCE could be mixed for only 1 parsing
                        vulnerabilityFound.InnerXml     = n.OuterXml;
                        vulnerabilityFound.Description  = HelperGetChildInnerText(n, "description");
                        vulnerabilityFound.Solution     = HelperGetChildInnerText(n, "solution");
                        vulnerabilityFound.Title        = HelperGetChildInnerText(n, "synopsis");
                        vulnerabilityFound.rawresponse  = HelperGetChildInnerText(n, "plugin_output");
                        vulnerabilityFound.Result = HelperGetChildInnerText(n, "plugin_output");
                        vulnerabilityFound.Severity = n.Attributes["severity"].InnerText;   //1
                        //vulnerabilityFound.Severity = HelperGetChildInnerText(n, "risk_factor");  //None  Low
                        if (HelperGetChildInnerText(n, "exploit_available") == "true")
                        {
                            vulnerabilityFound.Exploitable = true;
                        }
                        //exploitability_ease   Exploits are available
                        //exploit_framework_canvas
                        //exploit_framework_metasploit
                        //exploit_framework_core
                        //metasploit_name
                        //canvas_package

                        //cvss_vector
                        //cvss_temporal_score
                        try
                        {
                            vulnerabilityFound.CVSSBaseScore = float.Parse(HelperGetChildInnerText(n, "cvss_base_score"), System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch (Exception ex)
                        {
                            Utils.Helper_Trace("XORCISM PROVIDER NESSUS SCAN", string.Format("Error parsing CVSS_BASE : Exception = {0}", ex.Message));
                            Utils.Helper_Trace("XORCISM PROVIDER NESSUS SCAN", string.Format("CVSS_BASE =", HelperGetChildInnerText(n, "cvss_base_score")));
                        }

                        bool PatchUpgrade = false;
                        string MSPatch = "";
                        string title;                        
                        string Solution;
                        //patch_publication_date
                        if (HelperGetChildInnerText(n, "patch_publication_date") != "")
                        {
                            PatchUpgrade = true;
                        }
                        title = n.Attributes["pluginName"].InnerText;
                        Regex objNaturalPattern = new Regex("MS[0-9][0-9]-[0-9][0-9][0-9]");
                        MSPatch = objNaturalPattern.Match(title).ToString();
                        if (MSPatch != "")
                        {
                            Utils.Helper_Trace("XORCISM PROVIDER NESSUS SCAN", "MSPatch=" + MSPatch);
                            PatchUpgrade = true;
                        }
                        
                        Solution = HelperGetChildInnerText(n, "solution");
                        if (Solution.Contains(" upgrade to "))
                        {
                            PatchUpgrade = true;
                        }
                        if (Solution.Contains("Upgrade "))
                        {
                            PatchUpgrade = true;
                        }
                        if (Solution.Contains("has released a set of patches"))
                        {
                            PatchUpgrade = true;
                        }
                        if (Solution.Contains("Apply the appropriate patch"))
                        {
                            PatchUpgrade = true;
                        }

                        //<patch_publication_date>

                        vulnerabilityFound.PatchUpgrade = PatchUpgrade;
                        vulnerabilityFound.MSPatch = MSPatch;
                            
                        // ===========
                        // Persistance
                        // ===========

                        Utils.Helper_Trace("XORCISM PROVIDER NESSUS SCAN", string.Format("Persistance [{0}] [{1}] [{2}]", protocol, port, Helper_ListCVEToString(vulnerabilityFound.ListItem)));

                        int etat = VulnerabilityPersistor.Persist(vulnerabilityFound, vulnerabilityEndPoint, m_JobId, "nessus", model);
                        if (etat == -1)
                        {
                            Utils.Helper_Trace("XORCISM PROVIDER NESSUS SCAN", string.Format("CANNOT IMPORT THIS ASSET !!!! "));
                        }
                    }
                }


            }

            //TODO
            // VulnerabilityPersistor.UpdateVulnerabilityJob(list_vulnerabilyFound,m_JobId,m_model);

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
                    return child.InnerText;
            }

            return string.Empty;
        }

        private List<VulnerabilityFound.Item> Helper_GetCVE(XmlNode reportItem)
        {
            List<VulnerabilityFound.Item> l = new List<VulnerabilityFound.Item>();

            foreach (XmlNode child in reportItem.ChildNodes)
            {
                if (child.Name.ToUpper() == "cve".ToUpper())
                {
                    VulnerabilityFound.Item item = new VulnerabilityFound.Item();
                    item.Value = child.InnerText;
                    item.ID = "cve";
                    l.Add(item);
                }
            }

            return l;
        }

        private List<VulnerabilityFound.Reference> Helper_GetREFERENCE(XmlNode reportItem)
        {
            List<VulnerabilityFound.Reference> l = new List<VulnerabilityFound.Reference>();

            foreach (XmlNode child in reportItem.ChildNodes)
            {
                if (child.Name.ToUpper() == "bid".ToUpper())
                {
                    VulnerabilityFound.Reference Reference = new VulnerabilityFound.Reference();
                    Reference.Source = "BID";
                    Reference.Title = child.InnerText;
                    Reference.Url = "http://www.securityfocus.com/bid/" + child.InnerText;
                    Utils.Helper_Trace("XORCISM PROVIDER NESSUS SCAN", string.Format("TEST REFERENCEBID: {0}", "http://www.securityfocus.com/bid/" + child.InnerText));
                    l.Add(Reference);
                }
                else
                {
                    if (child.Name.ToUpper() == "see_also".ToUpper())
                    {
                        VulnerabilityFound.Reference Reference = new VulnerabilityFound.Reference();
                        Reference.Source = "NESSUS";
                        Reference.Title = child.InnerText;
                        Reference.Url = child.InnerText;
                        Utils.Helper_Trace("XORCISM PROVIDER NESSUS SCAN", string.Format("TEST REFERENCENESSUS: {0}", child.InnerText));
                        //TODO: check these unknown references/source
                        l.Add(Reference);
                    }
                    if (child.Name.ToUpper() == "xref".ToUpper())
                    {
                        if (child.InnerText.Contains("OSVDB"))
                        {
                            VulnerabilityFound.Reference Reference = new VulnerabilityFound.Reference();
                            Reference.Source = "OSVDB";
                            Reference.Title = child.InnerText.Replace("OSVDB:", "");
                            Reference.Url = "http://www.osvdb.org/" + child.InnerText.Replace("OSVDB:", "");
                            Utils.Helper_Trace("XORCISM PROVIDER NESSUS SCAN", string.Format("TEST REFERENCEOSVDB: {0}", "http://www.osvdb.org/" + child.InnerText.Replace("OSVDB:", "")));
                            l.Add(Reference);
                        }
                        else
                        {
                            if (child.InnerText.Contains("Secunia"))
                            {
                                VulnerabilityFound.Reference Reference = new VulnerabilityFound.Reference();
                                Reference.Source = "SECUNIA";
                                Reference.Title = child.InnerText.Replace("Secunia:", "");
                                Reference.Url = "http://secunia.com/advisories/" + child.InnerText.Replace("Secunia:", "");
                                Utils.Helper_Trace("XORCISM PROVIDER NESSUS SCAN", string.Format("TEST REFERENCESECUNIA: {0}", "http://secunia.com/advisories/" + child.InnerText.Replace("Secunia:", "")));
                                l.Add(Reference);
                            }
                            else
                            {
                                if (child.InnerText.Contains("EDB-ID"))
                                {
                                    VulnerabilityFound.Reference Reference = new VulnerabilityFound.Reference();
                                    Reference.Source = "EXPLOIT-DB";
                                    Reference.Title = child.InnerText.Replace("EDB-ID:", "");
                                    Reference.Url = "http://www.exploit-db.com/exploits/" + child.InnerText.Replace("EDB-ID:", "");
                                    Utils.Helper_Trace("XORCISM PROVIDER NESSUS SCAN", string.Format("TEST REFERENCEEXPLOIT-DB: {0}", "http://www.exploit-db.com/exploits/" + child.InnerText.Replace("EDB-ID:", "")));
                                    l.Add(Reference);
                                }
                                else
                                {
                                    VulnerabilityFound.Reference Reference = new VulnerabilityFound.Reference();
                                    Reference.Source = "NESSUS";
                                    Reference.Title = child.InnerText;
                                    Reference.Url = child.InnerText;
                                    Utils.Helper_Trace("XORCISM PROVIDER NESSUS SCAN", string.Format("TEST REFERENCENESSUS: {0}", child.InnerText));
                                    //TODO: check these unknown references/sources
                                    l.Add(Reference);
                                }
                            }
                        }
                    }
                }
            }

            return l;
        }

        
    }

    
}

