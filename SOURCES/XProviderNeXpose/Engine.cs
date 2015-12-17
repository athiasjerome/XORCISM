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
//using XVULNERABILITYModel;

using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

using FSM.DotNetSSH;

namespace XProviderRapid7
{
    //https://community.rapid7.com/docs/DOC-1461    EC2
    public class Engine : MarshalByRefObject, XProviderCommon.IVulnerabilityDetector
    {
        /// <summary>
        /// Copyright (C) 2012-2015 Jerome Athias
        /// XORCISM Plugin for Rapid7 NeXpose. Allows to launch scans on a remote scanner instance. Parses a report and imports it in XORCISM database
        /// All trademarks and registered trademarks are the property of their respective owners.
        /// This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation; either version 2 of the License, or (at your option) any later version.
        /// 
        /// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
        /// 
        /// You should have received a copy of the GNU General Public License along with this program; if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
        /// </summary>
        
        static int inerror = 0;

        static string ipserver = "192.168.1.1"; //HARDCODED

        public Engine()
        {
            TextWriterTraceListener tw;
            tw = new TextWriterTraceListener("XProviderRapid7.log");    //Hardcoded

            Trace.AutoFlush = true;
            Trace.IndentSize = 4;
            Trace.Listeners.Add(tw);
        }

        public void Run(string target, int jobID, string policy, string strategy)
        {
            //WARNING: OLD CODE, must be reviewved/revised - JA

            inerror = 0;
            Utils.Helper_Trace("XORCISM PROVIDER RAPID7", "Entering Run()");
            Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("Target = {0} , JobID = {1} , Policy = {2}, Strategy = {3}", target, jobID, policy, strategy));
            
            RAPID7Parser Rapid7Parser = new RAPID7Parser(target, jobID, policy, strategy);

            Rapid7Parser.DoIt(jobID);
            if (inerror == 0)
            {
                Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("Updating job {0} status to FINISHED", jobID));

                Rapid7Parser.UpdateJob(jobID);

                Utils.Helper_Trace("XORCISM PROVIDER RAPID7", "Leaving Run()");
            }
            inerror = 0;
        }

        
        private int SearchForRAPID7ID(string RAPID7ID)
        {
            return -1;
        }

        class RAPID7Parser
        {
            private string  m_target;
            private int     m_jobId;
            private string  m_policy;
            private string  m_data;
            private string m_strategy;

            public RAPID7Parser(string target, int jobID, string policy, string strategy)
            {
                m_target    = target;
                    
                m_jobId     = jobID;
                m_policy    = policy;
                m_strategy  = strategy;
            }

            public static bool ValidateServerCertificate(object sender,X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            {
                return true;
            }

            private string Helper_Request_ADD(string sessionID, string id, string SiteName, string Description, string HostName, string Credentials)
            {
                string MyPolicy;
                MyPolicy = string.Empty;
                //HARDCODED
                MyPolicy = "network-audit";
                switch (m_policy)
                {
                    case "Normal":
                        MyPolicy = "network-audit";
                        break;
                    case "Moderate":
                        MyPolicy = "full-audit";
                        break;
                    case "Intrusive":
                        MyPolicy = "exhaustive-audit";
                        break;
                    case "Pentest":
                        MyPolicy = "pentest-audit";
                        break;
                    case "Web":
                        MyPolicy = "web-audit"; //internet-audit
                        break;
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
                    default:
                        MyPolicy = "network-audit";
                        break;
                        //pentest-audit
                        //discovery
                        //aggressive-discovery
                        //dos-audit
                }

                string data;
                data = string.Empty;

                //HARDCODED
                data = "<SiteSaveRequest session-id=\"" + sessionID + "\">";
                data += "<Site id=\"" + id + "\" name=\"" + SiteName + "\" description=\"" + Description + "\">";
                data += "<Hosts><host>" + HostName + "</host></Hosts>";
                data += "<Credentials>" + Credentials + "</Credentials>";
                data += "<Alerting></Alerting>";
                data += "<ScanConfig configID=\"" + id + "\" name=\"XORCISM\" templateID=\"" + MyPolicy + "\"></ScanConfig>";   //Hardcoded
                data += "</Site>";
                data += "</SiteSaveRequest>";

                return data;
            }

            private string Helper_ReportSave(string sessionID, string ConfigID, string Name, string SiteID)
            {
                string data;
                data = string.Empty;
                //HARDCODED
                data = "<ReportSaveRequest session-id=\"" + sessionID + "\" generate-now=\"1\">";
                data += "<ReportConfig id=\"" + ConfigID + "\" name=\"" + Name + "\" template-id=\"xorcism\" format=\"qualys-xml\" owner=\"1\" timezone=\"America/New_York\">"; //Hardcoded
                data += "<Filters>";
                data += "<filter type=\"site\" id=\"" + SiteID + "\"></filter>";
                data += "</Filters>";
                data += "<Generate after-scan=\"0\" schedule=\"0\"></Generate>";
                data += "<Delivery><Storage StoreOnServer=\"1\"></Storage></Delivery>";

                data += "</ReportConfig>";
                data += "</ReportSaveRequest>";

                return data;
            }

            private void ResumeScan(string sessionID, string scanid)
            {
                ASCIIEncoding encoding = new ASCIIEncoding();
                //Hardcoded
                string data = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><LoginRequest session-id=\"" + sessionID + "\" scan-id=\"" + scanid + "!!!\"/>";
                byte[] Content = encoding.GetBytes(data);
                HttpWebRequest request;
                request = (HttpWebRequest)HttpWebRequest.Create("https://"+ipserver+":3780/api/1.1/xml");   //HARDCODED
                //ServicePointManager.CertificatePolicy = new AcceptAllCertificatePolicy();
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate);

                request.Method = "POST";
                request.ContentType = "text/xml";
                request.ContentLength = data.Length;
                Stream newStream = request.GetRequestStream();
                newStream.Write(Content.ToArray(), 0, Content.Length);
                newStream.Close();

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader SR = new StreamReader(response.GetResponseStream());
                string ResponseText = SR.ReadToEnd();

                XmlDocument doc = new XmlDocument();
                try
                {
                    doc.LoadXml(ResponseText);
                }
                catch (Exception ex)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("Error in ResumeScan : XmlException = {0}", ex.Message));
                }
                SR.Close();
            }

            public void DoIt(int jobID)
            {
                Assembly a;
                a = Assembly.GetExecutingAssembly();

                XORCISMEntities model;
                model = new XORCISMEntities();

                Utils.Helper_Trace("XORCISM PROVIDER RAPID7", "Assembly location = " + a.Location);

                //*************************************************************************************
                //For Web Scan, we have to change the policy to "Web"
                var CurrentSessionID = from j in model.JOB
                                       where j.JobID==jobID
                                       select j.SessionID;
                var sessioncategory = from s in model.SESSION
                                      where s.SessionID == (int)CurrentSessionID.FirstOrDefault()
                                      select s.ServiceCategoryID;
                if((int)sessioncategory.FirstOrDefault() == 2)  //HARDCODED
                {
                    Utils.Helper_Trace("XORCISM PROVIDER RAPID7", "Changing Policy to Web");
                    m_policy = "Web";   //HARDCODED
                }
                //*************************************************************************************

                string URI = string.Empty;
                string UrlScan = string.Empty;

                #region FAKE
                /*
                //URI = @"C:\Rapid7SampleReport.xml";
                URI = @"I:\XORCISM\sources\XAgentHost\bin\Debug\TEST01\NEXPOSE.XML";
                XmlDocument doc = new XmlDocument();
                */
                #endregion FAKE

                #region REAL
                
                string SessionID=string.Empty;
                string ScanID=string.Empty;
                string CFG=string.Empty;

                //int i = 0;
                string APIURL = "https://"+ipserver+":3780/api/1.1/xml";    //HARDCODED

                #region TESTCONNECTSERVER
                Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("Trying to connect to the server: " + APIURL));

                HttpWebRequest request;
                string ResponseText = "";
                StreamReader SR = null;
                HttpWebResponse response = null;

                try
                {
                    request = (HttpWebRequest)HttpWebRequest.Create(APIURL);
                    //ServicePointManager.CertificatePolicy = new AcceptAllCertificatePolicy();
                    ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate);
                    request.Method = "GET";
                    response = (HttpWebResponse)request.GetResponse();
                    SR = new StreamReader(response.GetResponseStream());
                    ResponseText = SR.ReadToEnd();
                }
                catch (Exception ex)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("Exception TESTCONNECTSERVER = {0}", ex.Message+" "+ex.InnerException));
                    //  Unable to connect to the remote server
                    ConnectServer();
                }

                #endregion


                #region LOGIN

                Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("Trying to login"));
                APIURL = "https://"+ipserver+":3780/api/1.1/xml";   //HARDCODED

                ASCIIEncoding encoding = new ASCIIEncoding();
                string data = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><LoginRequest user-id=\"JEROME\" password=\"CHANGEME\"/>";  //HARDCODED
                byte[] Content = encoding.GetBytes(data);
                
                Stream newStream;
                int cptrequestlogin = 0;
                while(cptrequestlogin < 20)
                {
                    try
                    {
                        cptrequestlogin++;
                        request = (HttpWebRequest)HttpWebRequest.Create(APIURL);
                        //ServicePointManager.CertificatePolicy = new AcceptAllCertificatePolicy();
                        ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate);

                        request.Method = "POST";
                        request.ContentType = "text/xml";
                        request.ContentLength = data.Length;
                        newStream = request.GetRequestStream();
                        newStream.Write(Content.ToArray(), 0, Content.Length);
                        newStream.Close();

                        response = (HttpWebResponse)request.GetResponse();
                        SR = new StreamReader(response.GetResponseStream());
                        ResponseText = SR.ReadToEnd();

                        Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("JobID: {0} Login response received : [{1}]", m_jobId, ResponseText));
                        cptrequestlogin = 999;  //Hardcoded

                        //<LoginResponse success="0">
                        //<Failure>
                        //<Exception>
                        //<message>Authorization required for API access</message>

                        //The API is not supported by this product edition
                        if (ResponseText.Contains("The API is not supported by this product edition"))  //HARDCODED
                        {
                            XCommon.Utils.Helper_SendEmail("contact@hackenaton.org", "Check Rapid7 license", "The API is not supported by this product edition");   //Hardcoded
                            ipserver = "192.168.1.1";   //HARDCODED
                        }

                        Regex objNaturalPattern = new Regex("Authorization required for API access");   //HARDCODED
                        string strTemp = objNaturalPattern.Match(ResponseText).ToString();
                        int cptretrylogin=0;
                        while (strTemp != "" && cptretrylogin < 50) //TODO: Review
                        {
                        //    if (strTemp != "")
                        //    {
                            try
                            {
                                cptretrylogin++;
                                Utils.Helper_Trace("XORCISM PROVIDER RAPID7", "JobID: "+m_jobId+"ERROR: Authorization required for API access");
                                Utils.Helper_Trace("XORCISM PROVIDER RAPID7", "Retrying Login..." + cptretrylogin);
                                Thread.Sleep(20000);
                                request = (HttpWebRequest)HttpWebRequest.Create(APIURL);
                                //ServicePointManager.CertificatePolicy = new AcceptAllCertificatePolicy();
                                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate);

                                request.Method = "POST";
                                request.ContentType = "text/xml";
                                request.ContentLength = data.Length;
                                newStream = request.GetRequestStream();
                                newStream.Write(Content.ToArray(), 0, Content.Length);
                                newStream.Close();
                                response = (HttpWebResponse)request.GetResponse();
                                SR = new StreamReader(response.GetResponseStream());
                                ResponseText = SR.ReadToEnd();

                                Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("JobID: {0} Login response received : [{1}]", m_jobId, ResponseText));
                                strTemp = objNaturalPattern.Match(ResponseText).ToString();
                            }
                            catch (Exception ex)
                            {
                                Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("JobID: {0} Exception Retry HttpWebRequestLOGIN = {1} {2}", m_jobId, ex.Message, ex.InnerException));
                                //  Unable to connect to the remote server
                                if (ex.Message.Contains("Unable to connect to the remote server") || ex.Message.Contains("connection was closed"))  //HARDCODED
                                {
                                    ConnectServer();
                                }
                            }
                        //    }
                        }
                    }
                    catch (Exception ex)
                    {
                        Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("JobID: {0} Exception HttpWebRequestLOGIN = {1} {2}", m_jobId, ex.Message, ex.InnerException));
                        //  Unable to connect to the remote server
                        //The underlying connection was closed: The connection was closed unexpectedly. 
                        //The operation has timed out

                        ipserver = "192.168.1.1";   //HARDCODED
                        Thread.Sleep(20000);    //Hardcoded
                        ConnectServer();
                    }
                }

                XmlDocument doc = new XmlDocument();
                try
                {
                    doc.LoadXml(ResponseText);
                    Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("JobID: {0} doc loaded", m_jobId));
                }
                catch (Exception ex)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("JobID: {0} Exception LoadXml(ResponseText) = {1} {2}", m_jobId, ex.Message, ex.InnerException));
                    //  Unable to connect to the remote server

                }
                SR.Close();
                XmlNode n1 = null;
                try
                {
                    n1 = doc.SelectSingleNode("/LoginResponse");    //HARDCODED

                    //MessageBox.Show(n.Attributes["session-id"].InnerText);

                    Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("JobID: {0} Session = {1}", m_jobId, n1.Attributes["session-id"].InnerText));

                    SessionID = n1.Attributes["session-id"].InnerText;
                }
                catch (Exception ex)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("JobID: {0} Exception SelectSingleNodeLoginResponse = {1} {2}", m_jobId, ex.Message, ex.InnerException));
                    //  Unable to connect to the remote server
                }
                
                #endregion

                #region CHECK IF THE ASSET EXISTS
                
                bool exist = false;
                string ID = string.Empty;
                XmlNodeList nlist;

                //TODO
                #endregion

                #region IF NECESSARY, CREATES AN ASSET ON NEXPOSE

                if (exist == true)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("Asset exists in NeXpose instance"));
                }
                else
                {
                    Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("Asset does not exist in NeXpose instance. Creating a new one."));                    

                    var tmpUser = from U in model.JOB
                                  where U.JobID == m_jobId
                                  select U.SESSION.UserID;

                    Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("User ID = {0}", tmpUser.FirstOrDefault()));

                    var tmpAccount = from Ac in model.USERACCOUNT
                                     where Ac.UserID == tmpUser.FirstOrDefault()
                                     select Ac.ACCOUNT;

                    ACCOUNT xAccount = tmpAccount.FirstOrDefault();

                    Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("Account ID = {0}", xAccount.AccountID));

                    ASSET xAsset = new ASSET();
                    try
                    {
                        //TODO  ipaddressIPv4
                        var tmpAsset = from A in model.ASSET
                                       where A.ipaddressIPv4 == m_target
                                       //&& A.AccountID == xAccount.AccountID
                                       select A;

                        xAsset = tmpAsset.FirstOrDefault();
                    }
                    catch (Exception ex)
                    {
                        Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("JobID: {0} Error xAsset : Exception = {1}", m_jobId, ex.Message));
                    }
                    //TODO  ipaddressIPv4
                    Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("JobID: {0} Asset ID = {1}    IP = {2}", m_jobId, xAsset.AssetID, xAsset.ipaddressIPv4));
                    //m_target = m_target.Replace("http://", "");
                    //m_target = m_target.Replace("https://", "");
                    //m_target = m_target.Replace("/", "");
                    ////m_target = m_target.Replace("www.", "");

                    if (m_target.Length - m_target.Replace(".", "").Length == 3 && m_target.Length - m_target.Replace(":", "").Length == 1)
                    {
                        //target like: 198.10.11.12:8089
                        char[] splitters = new char[] { ':' };
                        string[] laCase = m_target.Split(splitters);
                        m_target = laCase[0];
                        Utils.Helper_Trace("XORCISM PROVIDER RAPID7", "target without port: " + m_target);
                    }

                    string pattern = @"^\d\d?\d?\.\d\d?\d?\.\d\d?\d?\.\d\d?\d?$";   //TODO: IPv6
                    Regex check = new Regex(pattern);

                    if (check.IsMatch(m_target.Trim(), 0))
                    {
                        Utils.Helper_Trace("XORCISM PROVIDER RAPID7", "JobID:" + jobID + " target is an IP address");
                    }
                    else
                    {
                        Utils.Helper_Trace("XORCISM PROVIDER RAPID7", "JobID:" + jobID + " target: " + m_target + " is not an IP address");
                        if (!m_target.Contains("://"))
                            m_target = "http://" + m_target;

                        m_target=new Uri(m_target).Host;
                        Utils.Helper_Trace("XORCISM PROVIDER RAPID7", "JobID:" + jobID + " targetmodified: " + m_target);

                    }

                    var Credentials = from oCredentials in model.ASSETCREDENTIAL
                                      where oCredentials.AssetID == xAsset.AssetID
                                      select oCredentials;

                    string AssetCredentials;
                    AssetCredentials = string.Empty;

                    foreach (ASSETCREDENTIAL AC in Credentials.ToList())
                    {
                        switch (AC.AuthenticationType)
                        {
                            //HARDCODED
                            case "HTTP":
                                {
                                    AssetCredentials += "<adminCredentials service=\"http\" userid=\"" + AC.Username + "\" password=\"" + AC.Password + "\"/>";
                                    break;
                                }
                            case "SMB":
                                {
                                    AssetCredentials += "<adminCredentials service=\"cifs\" userid=\"" + AC.Username + "\" password=\"" + AC.Password + "\"/>";
                                    break;
                                }
                            case "SSH":
                                {
                                    AssetCredentials += "<adminCredentials service=\"ssh\" userid=\"" + AC.Username + "\" password=\"" + AC.Password + "\"/>";
                                    break;
                                }
                            case "Web":
                                {
                                    AssetCredentials += "<adminCredentials service=\"http\" userid=\"" + AC.Username + "\" password=\"" + AC.Password + "\"/>";
                                    break;
                                }
                            case "CVS":
                                {
                                    AssetCredentials += "<adminCredentials service=\"cvs\" userid=\"" + AC.Username + "\" password=\"" + AC.Password + "\"/>";
                                    break;
                                }
                            case "FTP":
                                {
                                    AssetCredentials += "<adminCredentials service=\"ftp\" userid=\"" + AC.Username + "\" password=\"" + AC.Password + "\"/>";
                                    break;
                                }
                            case "AS400":
                                {
                                    AssetCredentials += "<adminCredentials service=\"as400\" userid=\"" + AC.Username + "\" password=\"" + AC.Password + "\"/>";
                                    break;
                                }
                            case "NOTES":
                                {
                                    AssetCredentials += "<adminCredentials service=\"notes\" userid=\"" + AC.Username + "\" password=\"" + AC.Password + "\"/>";
                                    break;
                                }
                            case "MSSQL":
                                {
                                    AssetCredentials += "<adminCredentials service=\"tds\" userid=\"" + AC.Username + "\" password=\"" + AC.Password + "\"/>";
                                    break;
                                }
                            case "SYBASE":
                                {
                                    AssetCredentials += "<adminCredentials service=\"sybase\" userid=\"" + AC.Username + "\" password=\"" + AC.Password + "\"/>";
                                    break;
                                }
                            case "ORACLE":
                                {
                                    AssetCredentials += "<adminCredentials service=\"oracle\" userid=\"" + AC.Username + "\" password=\"" + AC.Password + "\"/>";
                                    break;
                                }
                            case "MYSQL":
                                {
                                    AssetCredentials += "<adminCredentials service=\"mysql\" userid=\"" + AC.Username + "\" password=\"" + AC.Password + "\"/>";
                                    break;
                                }
                            case "POP":
                                {
                                    AssetCredentials += "<adminCredentials service=\"pop\" userid=\"" + AC.Username + "\" password=\"" + AC.Password + "\"/>";
                                    break;
                                }
                            case "REXEC":
                                {
                                    AssetCredentials += "<adminCredentials service=\"remote execution\" userid=\"" + AC.Username + "\" password=\"" + AC.Password + "\"/>";
                                    break;
                                }
                            case "SNMP":
                                {
                                    AssetCredentials += "<adminCredentials service=\"snmp\" userid=\"" + AC.Username + "\" password=\"" + AC.Password + "\"/>";
                                    break;
                                }
                            case "TELNET":
                                {
                                    AssetCredentials += "<adminCredentials service=\"telnet\" userid=\"" + AC.Username + "\" password=\"" + AC.Password + "\"/>";
                                    break;
                                }
                        }
                    }

                    data = Helper_Request_ADD(SessionID, "-1", xAsset.AssetName + DateTime.Now.Ticks.ToString(), xAsset.AssetDescription, m_target, AssetCredentials); //random
                    
                    Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("JobID: {0} Request_ADD DATA = {1}", m_jobId, data));

                    // ================
                    // Send the request
                    // ================

                    Content = encoding.GetBytes(data);

                    try
                    {
                        request = (HttpWebRequest)HttpWebRequest.Create(APIURL);
                        //ServicePointManager.CertificatePolicy = new AcceptAllCertificatePolicy();
                        ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate);

                        request.Method = "POST";
                        request.ContentType = "text/xml";
                        request.ContentLength = data.Length;
                        newStream = request.GetRequestStream();
                        newStream.Write(Content.ToArray(), 0, Content.Length);
                        newStream.Close();

                        response = (HttpWebResponse)request.GetResponse();
                    }
                    catch (Exception ex)
                    {
                        Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("JobID: {0} Exception HttpWebRequestCREATE_ASSET = {1}", m_jobId, ex));
                    }

                    SR = new StreamReader(response.GetResponseStream());

                    ResponseText = SR.ReadToEnd();
                    //Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("JobID: {0} Request_ADD ResponseText = {1}", m_jobId, ResponseText));
                    //The specified site name is already in use by another site.
                    SR.Close();

                    //You have exceeded the licensed number of devices that can be scanned, or you are not authorized to scan this device range
                    Regex RegexError = new Regex("You have exceeded the licensed number of devices that can be scanned, or you are not authorized to scan this device range");  //HARDCODED
                    string strTemp = RegexError.Match(ResponseText).ToString();
                    if (strTemp != "")
                    {
                        Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("JobID: {0} XProviderRapid7 1515 : Setting job status to ERROR", m_jobId));
                        XCommon.Utils.Helper_SendEmail("contact@hackenaton.org", "Rapid7 number of assets exceeded", "JobID: "+m_jobId+" You have exceeded the licensed number of devices that can be scanned, or you are not authorized to scan this device range");   //Hardcoded

                        JOB job;
                        job = model.JOB.FirstOrDefault(o => o.JobID == jobID);

                        job.Status = XCommon.STATUS.ERROR.ToString();
                        job.DateEnd = DateTimeOffset.Now;
                        job.ErrorReason = "licenced number of devices exceeded";    //HARDCODED
                        model.SaveChanges();
                        return;
                    }

                    // =========================================================
                    // Parse the response and get the asset identifier on Rapid7
                    // =========================================================

                    RegexError = new Regex("The specified site name is already in use by another site.");   //HARDCODED
                    strTemp = RegexError.Match(ResponseText).ToString();
                    if (strTemp != "")
                    {
                        //Retrieve the SiteID of the existing site
                        //HARDCODED
                        data = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><SiteListingRequest session-id=\"" + SessionID + "\"></SiteListingRequest>";   //</SiteListingRequest>
                        Content = encoding.GetBytes(data);

                        try
                        {
                            request = (HttpWebRequest)HttpWebRequest.Create(APIURL);
                            //ServicePointManager.CertificatePolicy = new AcceptAllCertificatePolicy();
                            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate);

                            request.Method = "POST";
                            request.ContentType = "text/xml";
                            request.ContentLength = data.Length;
                            newStream = request.GetRequestStream();
                            newStream.Write(Content.ToArray(), 0, Content.Length);
                            newStream.Close();

                            response = (HttpWebResponse)request.GetResponse();

                        }
                        catch (Exception ex)
                        {
                            Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("JobID: {0} Exception SiteListingRequest = {1}", m_jobId, ex));

                            return;
                        }
                        SR = new StreamReader(response.GetResponseStream());

                        ResponseText = SR.ReadToEnd();
                        Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("JobID: {0} Request_SiteListingRequest ResponseText = {1}", m_jobId, ResponseText));
                        //
                        SR.Close();

                        doc = new XmlDocument();
                        try
                        {
                            doc.LoadXml(ResponseText);

                            //Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("ReponseText = {0} ",ResponseText));

                            n1 = doc.SelectSingleNode("/SiteListingResponse");  //HARDCODED
                            if (n1 == null)
                            {
                                Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("Error SiteListingResponse = {0}", doc.InnerXml));
                            }

                            foreach (XmlNode child in n1.ChildNodes)
                            {
                                if (child.Name.ToUpper() == "SITESUMMARY")
                                {
                                    if (child.Attributes["name"] != null)
                                    {
                                        if (child.Attributes["name"].InnerText == m_target)
                                        {
                                            ID = child.Attributes["id"].InnerText;
                                            Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("Site-ID01 = {0}", ID));
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("JobID: {0} Exception SiteSaveResponse = {1}", m_jobId, ex.Message));
                        }

                    }
                    else
                    {
                        doc = new XmlDocument();
                        try
                        {
                            doc.LoadXml(ResponseText);

                            //Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("ReponseText = {0} ",ResponseText));

                            n1 = doc.SelectSingleNode("/SiteSaveResponse"); //HARDCODED
                            if (n1 == null)
                            {
                                Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("Debug 2 = {0}", doc.InnerXml));
                            }

                            ID = n1.Attributes["site-id"].InnerText;
                            Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("Site-ID02 = {0}", ID));
                            
                        }
                        catch (Exception ex)
                        {
                            Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("JobID: {0} Exception SiteSaveResponse = {1}", m_jobId, ex.Message));
                        }
                    }
                }

                Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("JobID: {0} Asset identifier on Rapid7 = {1}", m_jobId, ID));
                
                #endregion

                #region LAUNCH THE SCAN
                
                Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("JobID: {0} Launching the scan", m_jobId));

                //HARDCODED
                data = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><SiteScanRequest session-id=\"" + SessionID + "\" site-id=\"" + ID + "\"/>";
                Content = encoding.GetBytes(data);

                request = (HttpWebRequest)HttpWebRequest.Create(APIURL);
                //ServicePointManager.CertificatePolicy = new AcceptAllCertificatePolicy();
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate);

                request.Method = "POST";
                request.ContentType = "text/xml";
                request.ContentLength = data.Length;
                newStream = request.GetRequestStream();
                newStream.Write(Content.ToArray(), 0, Content.Length);
                newStream.Close();
                try
                {
                    response = (HttpWebResponse)request.GetResponse();
                    SR = new StreamReader(response.GetResponseStream());
                    ResponseText = SR.ReadToEnd();
                    Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("JobID: {0} SiteScanResponse {1}", m_jobId, ResponseText));

                    doc = new XmlDocument();
                    doc.LoadXml(ResponseText);
                    SR.Close();
                    nlist = doc.SelectNodes("/SiteScanResponse/Scan");  //Hardcoded

                    ScanID = nlist[0].Attributes["scan-id"].InnerText;

                    Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("JobID: {0} ScanID on Rapid7 is {1}", m_jobId, ScanID));
                }
                catch (Exception ex)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("JobID: {0} Exception SiteScanResponse/Scan = {1} {2}", m_jobId, ex.Message, ex.InnerException));
                }
                                
                #endregion

                #region WAIT UNTIL THE SCAN IS COMPLETED

                Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("JobID: {0} Waiting until the scan completes", m_jobId));

                bool NotYet = true;
                while (NotYet)
                {
                    //TODO: Review and optimize

                    //Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("JobID: {0} Polling...", m_jobId));
                    //HARDCODED
                    data = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><ScanStatusRequest session-id=\"" + SessionID + "\" scan-id=\"" + ScanID + "\"/>";
                    Content = encoding.GetBytes(data);
                    
                    try
                    {
                        request = (HttpWebRequest)HttpWebRequest.Create("https://"+ipserver+":3780/api/1.1/xml");   //HARDCODED
                        //ServicePointManager.CertificatePolicy = new AcceptAllCertificatePolicy();
                        ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate);

                        request.Method = "POST";
                        request.ContentType = "text/xml";
                        request.ContentLength = data.Length;
                        newStream = request.GetRequestStream();
                        newStream.Write(Content.ToArray(), 0, Content.Length);
                        newStream.Close();
                        response = (HttpWebResponse)request.GetResponse();
                    }
                    catch (Exception ex)
                    {
                        Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("JobID: {0} Exception Polling = {1} {2}", m_jobId, ex.Message, ex.InnerException));
                        Utils.Helper_Trace("XORCISM PROVIDER RAPID7", "JobID: "+m_jobId+" Retrying...");

                        int cptretryscanstatus = 0;
                        int errorscanstatus = 1;
                        while (errorscanstatus == 1 && cptretryscanstatus < 20)
                        {                            
                            try
                            {
                                request = (HttpWebRequest)HttpWebRequest.Create("https://"+ipserver+":3780/api/1.1/xml");   //HARDCODED
                                //ServicePointManager.CertificatePolicy = new AcceptAllCertificatePolicy();
                                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate);

                                request.Method = "POST";
                                request.ContentType = "text/xml";
                                request.ContentLength = data.Length;
                                newStream = request.GetRequestStream();
                                newStream.Write(Content.ToArray(), 0, Content.Length);
                                newStream.Close();
                                response = (HttpWebResponse)request.GetResponse();
                                errorscanstatus = 0;
                            }
                            catch (Exception ex2)
                            {
                                //Unable to connect to the remote server System.Net.Sockets.SocketException (0x80004005): No connection could be made because the target machine actively refused it 192.168.1.1:3780
                                Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("JobID: {0} Exception Polling Retrying{1} = {2} {3}", m_jobId, cptretryscanstatus, ex2.Message, ex2.InnerException));

                                if (ex2.Message.Contains("Unable to connect to the remote server") || ex2.Message.Contains("connection was closed"))    //HARDCODED
                                {
                                    ConnectServer();
                                }

                                Utils.Helper_Trace("XORCISM PROVIDER RAPID7", "JobID: " + m_jobId + " Retrying...");
                                cptretryscanstatus++;
                                if (cptretryscanstatus == 20)
                                {
                                    JOB job;
                                    job = model.JOB.FirstOrDefault(o => o.JobID == jobID);

                                    job.Status = XCommon.STATUS.ERROR.ToString();
                                    job.DateEnd = DateTimeOffset.Now;
                                    job.ErrorReason = "Max Polling Retry";  //HARDCODED
                                    model.SaveChanges();
                                    NotYet = false;
                                    return;
                                }
                            }
                        }
                    }
                    try
                    {
                        SR = new StreamReader(response.GetResponseStream());
                        ResponseText = SR.ReadToEnd();
                    }
                    catch (Exception ex)
                    {
                        Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("JobID: {0} Exception PollingStreamReader = {1} {2}", m_jobId, ex.Message, ex.InnerException));                        
                    }

                    doc = new XmlDocument();
                    try
                    {
                        doc.LoadXml(ResponseText);
                        SR.Close();
                        n1 = doc.SelectSingleNode("/ScanStatusResponse");   //HARDCODED
                    }
                    catch (Exception ex)
                    {
                        Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("JobID: {0} Exception ScanStatusRequest-ResponseText = {1} {2}", m_jobId, ex.Message, ex.InnerException));
                        Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("ScanStatusResponse-ResponseText = [{0}]", ResponseText));
                        //FATAL: sorry, too many clients already
                    }

                    //Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("JobID: {0} ScanStatusResponse = [{1}]", m_jobId, n1.OuterXml));
                    //Session not found
                    if (n1.OuterXml.Contains("Session not found"))  //HARDCODED
                    {
                        Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("JobID: {0} XProviderRapid7 127 : Session not found Setting job status to ERROR", m_jobId));
                        //XORCISMEntities model;
                        //model = new XORCISMEntities();

                        JOB job;
                        job = model.JOB.FirstOrDefault(o => o.JobID == jobID);

                        job.Status = XCommon.STATUS.ERROR.ToString();
                        job.DateEnd = DateTimeOffset.Now;
                        job.ErrorReason = "Session not found";  //HARDCODED
                        model.SaveChanges();
                        NotYet = false;
                        XCommon.Utils.Helper_SendEmail("contact@hackenaton.org", "NEXPOSE ERROR", "NEXPOSE ERROR Session not found for job:" + jobID);  //Hardcoded
                        return;
                    }
                    else
                    {
                        //...
                        try
                        {
                            if (n1.Attributes["status"] == null || string.IsNullOrEmpty(n1.Attributes["status"].Value)) //HARDCODED
                            {
                                //No exception but error
                                Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("JobID: {0} ERROR status=null ScanStatusResponse-ResponseText = [{1}]", m_jobId, ResponseText));
                                if (ResponseText.Contains("Invalid scanID"))
                                {
                                    Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("JobID: {0} XProviderRapid7 007 : Invalid scanID Setting job status to ERROR", m_jobId));
                                    
                                    JOB job;
                                    job = model.JOB.FirstOrDefault(o => o.JobID == jobID);

                                    job.Status = XCommon.STATUS.ERROR.ToString();
                                    job.DateEnd = DateTimeOffset.Now;
                                    job.ErrorReason = "Invalid scanID"; //HARDCODED
                                    model.SaveChanges();

                                    return;
                                }
                                //Session not found
                                if (ResponseText.Contains("Session not found")) //HARDCODED
                                {
                                    Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("JobID: {0} XProviderRapid7 007 : Session not found Setting job status to ERROR", m_jobId));
                                    
                                    JOB job;
                                    job = model.JOB.FirstOrDefault(o => o.JobID == jobID);

                                    job.Status = XCommon.STATUS.ERROR.ToString();
                                    job.DateEnd = DateTimeOffset.Now;
                                    job.ErrorReason = "Session not found";  //HARDCODED
                                    model.SaveChanges();
                                    NotYet = false;
                                    return;
                                }
                                //ok we will retry
                                Thread.Sleep(60000);
                            }
                            else
                            {
                                if (n1.Attributes["status"].InnerText == "finished")    //HARCODED
                                {
                                    Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("JobID: {0} The scan has finished", m_jobId));
                                    NotYet = false;
                                }

                                if (n1.Attributes["status"].InnerText == "paused")  //HARDCODED
                                {
                                    Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("JobID: {0} The scan is paused, resuming the scan", m_jobId));
                                    ResumeScan(SessionID, ScanID);
                                }
                                else
                                {
                                    Thread.Sleep(60000);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("JobID: {0} Exception ScanStatusRequest02-ResponseText = {1} {2}", m_jobId, ex.Message, ex.InnerException));
                            //Thread was being aborted.
                            if (ex.Message.Contains("Thread was being aborted."))
                            {
                                Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("JobID: {0} XProviderRapid7 1518 : Setting job status to ERROR", m_jobId));
                                
                                JOB job;
                                job = model.JOB.FirstOrDefault(o => o.JobID == jobID);

                                job.Status = XCommon.STATUS.ERROR.ToString();
                                job.DateEnd = DateTimeOffset.Now;
                                job.ErrorReason = "Thread was being aborted.";  //HARDCODED
                                model.SaveChanges();
                                NotYet = false;
                                Logmeout(SessionID);

                                return;
                            }
                        }
                    }
                }

                #endregion COMPLETED

                #region GENERATES A REPORT

                Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("JobID: {0} Saving the configuration for a report definition", m_jobId));

                data = Helper_ReportSave(SessionID, "-1", SessionID, ID);

                Content = encoding.GetBytes(data);

                request = (HttpWebRequest)HttpWebRequest.Create("https://"+ipserver+":3780/api/1.1/xml");   //HARDCODED
                //ServicePointManager.CertificatePolicy = new AcceptAllCertificatePolicy();
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate);

                request.Method = "POST";
                request.ContentType = "text/xml";
                request.ContentLength = data.Length;
                newStream = request.GetRequestStream();
                newStream.Write(Content.ToArray(), 0, Content.Length);
                newStream.Close();
                try
                {
                    response = (HttpWebResponse)request.GetResponse();
                    SR = new StreamReader(response.GetResponseStream());
                    ResponseText = SR.ReadToEnd();
                }
                catch (Exception ex)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("JobID: {0} Exception GenerateReport = {1} {2}", m_jobId, ex.Message, ex.InnerException));
                }

                Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("JobID: {0} ResponseText = {1}", m_jobId, ResponseText));

                doc = new XmlDocument();
                try
                {
                    doc.LoadXml(ResponseText);
                    SR.Close();
                    n1 = doc.SelectSingleNode("/ReportSaveResponse");   //HARDCODED
                    
                }
                catch (Exception ex)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("JobID: {0} Exception ReportSaveResponse = {1} {2}", m_jobId, ex.Message, ex.InnerException));
                }
                try
                {
                    CFG = n1.Attributes["reportcfg-id"].InnerText;  //HARDCODED
                }
                catch (Exception ex)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("JobID: {0} Exception GenerateReportCFG = {1} {2}", m_jobId, ex.Message, ex.InnerException));
                }
                Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("JobID: {0} Report-cfg identifier on Rapid7 is {1} ", m_jobId, CFG));
                #endregion

                #region GET THE URL OF THE GENERATED REPORT
                
                Thread.Sleep(30000);

                Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("JobID: {0} Retrieving the URL of the report", m_jobId));

                data = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><ReportListingRequest session-id=\"" + SessionID + "\"/>";    //HARDCODED

                Content = encoding.GetBytes(data);

                request = (HttpWebRequest)HttpWebRequest.Create("https://"+ipserver+":3780/api/1.1/xml");   //HARDCODED
                //ServicePointManager.CertificatePolicy = new AcceptAllCertificatePolicy();
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate);

                request.Method = "POST";
                request.ContentType = "text/xml";
                request.ContentLength = data.Length;
                newStream = request.GetRequestStream();
                newStream.Write(Content.ToArray(), 0, Content.Length);
                newStream.Close();

                response = (HttpWebResponse)request.GetResponse();
                SR = new StreamReader(response.GetResponseStream());
                ResponseText = SR.ReadToEnd();

                Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("JobID: {0} ResponseText = {1}", m_jobId, ResponseText));

                doc = new XmlDocument();
                try
                {
                    doc.LoadXml(ResponseText);
                }
                catch (Exception ex)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("JobID: {0} Exception ReportListingResponse = {1} {2}", m_jobId, ex.Message, ex.InnerException));
                }  
                SR.Close();

                nlist = doc.SelectNodes("/ReportListingResponse/ReportConfigSummary");  //HARCODED

                foreach (XmlNode node in nlist)
                {
                    if (node.Attributes["cfg-id"].InnerText == CFG) //HARDCODED
                    {
                        Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("DEBUG Node : {0}", node.OuterXml));
                        //...
                        if (node.OuterXml.Contains("Failed"))
                        {
                            Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("JobID: {0} ErrorReportConfigSummary = Failed", m_jobId));
                            inerror = 1;
                            Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("JobID: {0} XProviderRapid7 ErrorReportConfigSummary : Setting job status to ERROR", m_jobId));
                            
                            JOB job;
                            job = model.JOB.FirstOrDefault(o => o.JobID == m_jobId);

                            job.Status = XCommon.STATUS.ERROR.ToString();
                            job.DateEnd = DateTimeOffset.Now;
                            job.ErrorReason = "Failed"; //HARDCODED
                            model.SaveChanges();

                            Logmeout(SessionID);
                            XCommon.Utils.Helper_SendEmail("contact@hackenaton.org", "RAPID7 ERROR", "RAPID7 ERROR for job:" + m_jobId);    //Hardcoded
                            return;
                        }
                        else
                        {
                            try
                            {
                                URI = "https://"+ipserver+":3780/login.html?nexposeccusername=jerome&nexposeccpassword=CHANGEME&loginRedir=";   //HARDCODED
                                UrlScan = node.Attributes["report-URI"].InnerText;  //HARDCODED
                            }
                            catch (Exception ex)
                            {
                                Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("JobID: {0} Exception = {1}", m_jobId, ex));
                                inerror = 1;
                                Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("JobID: {0} XProviderRapid7 1664 : Setting job status to ERROR", m_jobId));
                                
                                JOB job;
                                job = model.JOB.FirstOrDefault(o => o.JobID == jobID);

                                job.Status = XCommon.STATUS.ERROR.ToString();
                                job.DateEnd = DateTimeOffset.Now;
                                job.ErrorReason = ex.ToString();
                                model.SaveChanges();

                                Logmeout(SessionID);
                                XCommon.Utils.Helper_SendEmail("contact@hackenaton.org", "RAPID7 ERROR", "RAPID7 ERROR for job:" + m_jobId);    //Hardcoded
                                return;
                            }
                        }
                    }
                }
                if (inerror == 1)
                {
                    return;
                }
                Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("DEBUG JobID: {0} The URL of the report is [{1}]", m_jobId, URI));
                
                #endregion

                #region LOGOUT

                //data = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><LogoutRequest session-id=\"" + SessionID + "\"/>";
                //Content = encoding.GetBytes(data);
                //request = (HttpWebRequest)HttpWebRequest.Create("https://192.168.1.1:3780/api/1.1/xml");  //HARDCODED
                ////ServicePointManager.CertificatePolicy = new AcceptAllCertificatePolicy();
                //ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate);

                //request.Method = "POST";
                //request.ContentType = "text/xml";
                //request.ContentLength = data.Length;
                //newStream = request.GetRequestStream();
                //newStream.Write(Content.ToArray(), 0, Content.Length);
                //newStream.Close();

                //response = (HttpWebResponse)request.GetResponse();
                //SR = new StreamReader(response.GetResponseStream());
                //ResponseText = SR.ReadToEnd();
                ////MessageBox.Show("Logout OK");

                #endregion LOGOUT

                #endregion REAL

                #region DOWNLOADS THE REPORT

                String URLString = URI;
                XmlTextReader reader = new XmlTextReader(URLString);
                //TODO Review http://vsecurity.com/download/papers/XMLDTDEntityAttacks.pdf

                XmlDocument xDoc = new XmlDocument();
                
                //LoadXmlResult(URLString,UrlScan);

                //==========================================================================

                Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("JobID: {0} Downloading the report", m_jobId));

                string myUrl = URI + UrlScan;
                Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("URLBase = {0} & URLScan = {1}", URI, UrlScan));

                System.Text.UTF8Encoding objUTF8 = new System.Text.UTF8Encoding();

                request = (HttpWebRequest)WebRequest.Create(myUrl);
                request.UserAgent = "Mozilla/1.0 (Windows; U; Windows NT 6.1; fr; rv:1.2.3.4) Gecko/20150401 Firefox/5.6.7";  //HARDCODED
                request.CookieContainer = new CookieContainer();
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate);
                request.Credentials = CredentialCache.DefaultCredentials;
                request.Method = "POST";

                string postData = "nexposeccusername=jerome&nexposeccpassword=CHANGEME&login_button=Login&loginRedir=" + UrlScan;   //HARDCODED
                Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("PostData = {0}", postData));
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                // Get the response.
                response = (HttpWebResponse)request.GetResponse();
                dataStream = response.GetResponseStream();
                SR = new StreamReader(dataStream);
                string responseFromServer = SR.ReadToEnd();
                Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("DEBUG Response from server = [{0}]", responseFromServer));
                //m_data = responseFromServer;

                //==========================================================================


                m_data = responseFromServer;// xDoc.InnerText;

                Utils.Helper_Trace("XORCISM PROVIDER RAPID7", "JobID: "+m_jobId+" Report data successfully downloaded");
                
                #endregion

                #region ParseNexposeReport

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

                doc = new XmlDocument();
                inerror = 0;
                try
                {
                    Utils.Helper_Trace("XORCISM PROVIDER RAPID7", "Loading the XML document");
                    //TODO: Input Validation (XML)
                    doc.LoadXml(m_data);
                    Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("JobID: {0} m_data = {1}", m_jobId, m_data));
                }
                catch (Exception ex)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("JobID: {0} LoadingXML Exception = {1} / {2}", m_jobId, ex.Message, ex.InnerException == null ? "" : ex.InnerException.Message));
                    Utils.Helper_Trace("XORCISM PROVIDER RAPID7", "RetryingLoading the XML document");
                    try
                    {                        
                        Thread.Sleep(30000);

                        doc.LoadXml(m_data);
                        Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("JobID: {0} m_data = {1}", m_jobId, m_data));
                    }
                    catch (Exception ex2)
                    {
                        Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("JobID: {0} RetryingLoadingXML Exception = {1} / {2}", m_jobId, ex2.Message, ex2.InnerException == null ? "" : ex2.InnerException.Message));
                        Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("XProviderRapid7 1979: Setting job status to ERROR"));
                        
                        JOB job;
                        job = model.JOB.FirstOrDefault(o => o.JobID == jobID);

                        job.Status = XCommon.STATUS.ERROR.ToString();
                        job.DateEnd = DateTimeOffset.Now;
                        model.SaveChanges();
                        inerror = 1;

                        Logmeout(SessionID);
                        XCommon.Utils.Helper_SendEmail("contact@hackenaton.org", "RAPID7 ERROR", "RAPID7 ERROR for job:" + m_jobId);    //Hardcoded

                        return;
                    }
                    
                }
                if (inerror == 0)
                {

                    #region Parser

                    
                    string query = "/SCAN/IP/VULNS/CAT";    //HARDCODED

                    XmlNodeList report;
                    report = null;
                    try
                    {
                        report = doc.SelectNodes(query);
                    }
                    catch (Exception ex)
                    {
                        Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("Error SelectNodes({0}) : Exception = {1}", query, ex.Message));
                    }

                    Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("JobID: {0} Found {1} hosts to parse", m_jobId, report.Count));

                    foreach (XmlNode reportHost in report)
                    {
                        Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("Handling host with IP {0}", m_target));

                        // ===========================
                        // Handle every ReportItem tag
                        // ===========================

                        protocol = string.Empty;
                        port = -1;
                        //HARDCODED
                        if (reportHost.Attributes["protocol"] != null)
                            protocol = reportHost.Attributes["protocol"].InnerText.ToUpper();
                        if (reportHost.Attributes["port"] != null)
                            port = Convert.ToInt32(reportHost.Attributes["port"].InnerText);

                        Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("Protocol = [{0}]   Port = [{1}]", protocol, port));

                        VulnerabilityEndPoint vulnerabilityEndPoint = new VulnerabilityEndPoint();
                        vulnerabilityEndPoint.IpAdress = m_target;
                        vulnerabilityEndPoint.Protocol = protocol;
                        vulnerabilityEndPoint.Port = port;

                        foreach (XmlNode n in reportHost.ChildNodes)
                        {
                            XmlNodeList Childs = n.ChildNodes;

                            Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("Coucou 1"));

                            VulnerabilityFound vulnerabilityFound = new VulnerabilityFound();
                            vulnerabilityFound.ListItem = Helper_GetCVE(n);
                            vulnerabilityFound.ListReference = Helper_GetREFERENCE(n);  //TODO: Helper_GetCVE and Helper_GetREFERENCE could be mixed for only 1 parsing
                            vulnerabilityFound.InnerXml = n.OuterXml;
                            //HARDCODED
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
                            //TODO: Review and optimize
                            if (tempoTitle == "SQL Injection Vulnerability" || tempoTitle == "Cross Site Scripting Vulnerability") //"Web"  //HARDCODED
                            {
                                //...
                                try
                                {
                                    Regex RegexPattern = new Regex(@"<a.*?href=[""'](.*?)[""'].*?>", RegexOptions.Singleline);
                                    string myURL = RegexPattern.Match(tempoResult).ToString().Replace("<a href=\"", "").Replace("\">", "");
                                    Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("Extracted url = {0}", myURL));
                                    vulnerabilityFound.Url = myURL;

                                    //VulnerableParameter
                                    string pattern = "Injected into the &quot;(.*?)&quot; form parameter on";   //HARDCODED
                                    MatchCollection matches = Regex.Matches(tempoResult, pattern);
                                    foreach (Match match in matches)
                                    {
                                        vulnerabilityFound.VulnerableParameter = match.Value.Replace("Injected into the &quot;", "").Replace("&quot; form parameter on", "");   //HARDCODED
                                        vulnerabilityFound.VulnerableParameterType = "Post";
                                    }
                                    vulnerabilityFound.rawresponse = tempoResult; //Details
                                    
                                }
                                catch (Exception xx)
                                {

                                }
                            }

                            if (tempoTitle == "Blind SQL Injection")    //HARDCODED
                            {
                                //...
                                Regex RegexPattern = new Regex(@"<a.*?href=[""'](.*?)[""'].*?>", RegexOptions.Singleline);
                                string myURL = RegexPattern.Match(tempoResult).ToString().Replace("<a href=\"", "").Replace("\">", "");
                                Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("Extracted url = {0}", myURL));
                                vulnerabilityFound.Url = myURL;

                                //VulnerableParameter
                                string pattern = "Parameter <DIV CLASS=\"highlight\">(.*?)</DIV> behaves differently";  //HARDCODED
                                MatchCollection matches = Regex.Matches(tempoResult, pattern);
                                foreach (Match match in matches)
                                {
                                    vulnerabilityFound.VulnerableParameter = match.Value.Replace("Parameter <DIV CLASS=\"highlight\">", "").Replace("</DIV> behaves differently", "");  //HARDCODED                                
                                }
                                if (tempoTitle.Contains("using method POST"))   //HARDCODED
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
                                Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("Error parsing CVSS_BASE : Exception = {0}", ex.Message));
                            }

                            #region JEROME
                            PatchUpgrade = false;
                            title = HelperGetChildInnerText(n, "TITLE");
                            MSPatch = "";
                            Regex objNaturalPattern = new Regex("MS[0-9][0-9]-[0-9][0-9][0-9]");
                            MSPatch = objNaturalPattern.Match(title).ToString();
                            if (MSPatch != "")
                            {
                                Utils.Helper_Trace("XORCISM PROVIDER RAPID7", "MSPatch=" + MSPatch);
                                PatchUpgrade = true;
                            }
                            else
                            {
                                Solution = HelperGetChildInnerText(n, "SOLUTION");
                                //HARDCODED
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
                                //    Utils.Helper_Trace("XORCISM PROVIDER RAPID7", title);
                                //}
                            }

                            if (PatchUpgrade)
                            {
                                Utils.Helper_Trace("XORCISM PROVIDER RAPID7", "PatchUpgrade");
                            }
                            else
                            {
                                //    Utils.Helper_Trace("XORCISM PROVIDER RAPID7", "Solution: " + Solution);
                            }
                            vulnerabilityFound.PatchUpgrade = PatchUpgrade;
                            vulnerabilityFound.MSPatch = MSPatch;
                            #endregion


                            // ===========
                            // Persistance
                            // ===========

                            Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("JobID: {0} Persistance [{1}] [{2}] [{3}]", m_jobId, protocol, port, Helper_ListCVEToString(vulnerabilityFound.ListItem)));
                            int VulnID = VulnerabilityPersistor.Persist(vulnerabilityFound, vulnerabilityEndPoint, m_jobId, "Rapid7", model);

                            //TODO
                            //Mapping with CWE
                            /*
                            if (tempoTitle == "Blind SQL Injection")    //HARDCODED
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
                            if (xJob.SESSION.ServiceCategoryID == 4) // Compliance  //HARDCODED
                            {
                                #region Persist Compliances
                                List<int> Compliances = new List<int>();

                                Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("Node xml ==> {0}", n.InnerText));

                                Compliances = GetCompliance(n.InnerXml, reportHost.Attributes["value"].InnerText);
                                Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("Vulnerability persisted , VulnID = {0} & Compliance count = {1}", VulnID, Compliances.Count));

                                //TODO
                                /*
                                var V = from tmpVuln in model.VULNERABILITYFOUND
                                        where tmpVuln.VulnerabilityFoundID == VulnID
                                        select tmpVuln;

                                VULNERABILITYFOUND VF = V.FirstOrDefault();

                                foreach (int i in Compliances)
                                {
                                    Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("DEBUG Compliance Category => ", i));
                                    var C = from Comp in model.COMPLIANCECATEG
                                            where Comp.ComplianceCategID == i
                                            select Comp;

                                    COMPLIANCECATEG myCompliance = new COMPLIANCECATEG();
                                    myCompliance = C.FirstOrDefault();

                                    VF.COMPLIANCECATEG.Add(myCompliance);

                                    model.SaveChanges();
                                    Utils.Helper_Trace("XORCISM PROVIDER RAPID7", "Mapping Compliance-Vulnerability Added");
                                }
                                */
                                #endregion
                            }
                        }
                    }
                    #endregion

                    #endregion ParseNexposeReport

                    Logmeout(SessionID);
                }
            }

            private void Logmeout(string SessionID)
            {
                Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("JobID: {0} Logout", m_jobId));

                string data = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><LogoutRequest session-id=\"" + SessionID + "\"/>";    //HARDCODED
                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] Content = encoding.GetBytes(data);
                HttpWebRequest request;

                request = (HttpWebRequest)HttpWebRequest.Create("https://"+ipserver+":3780/api/1.1/xml");   //HARDCODED
                //ServicePointManager.CertificatePolicy = new AcceptAllCertificatePolicy();
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate);

                request.Method = "POST";
                request.ContentType = "text/xml";
                request.ContentLength = data.Length;
                Stream newStream = request.GetRequestStream();
                newStream.Write(Content.ToArray(), 0, Content.Length);
                newStream.Close();

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader SR = new StreamReader(response.GetResponseStream());
                string ResponseText = SR.ReadToEnd();

                Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("JobID: {0} LogoutResponse = {0}", m_jobId, ResponseText));

                SR.Close();
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

            private void ConnectServer()
            {
                //TODO: CHANGE THIS
                //NOTE: This is awesomely horrible ;-)

                int port;
                    string address, username, password;

                    //HARDCODED
                    port = 22;
                    address = "192.168.1.1";
                    username = "root";  //OMG!
                    password = "CHANGEME";

                    SshShell sshShell;
                    sshShell = new SshShell(address, username, password);
                    sshShell.RemoveTerminalEmulationCharacters = true;

                    Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("JobID: {0} Connecting via SSH to RAPID7 server at {1}", m_jobId, address));

                    string prompt;
                    prompt = "root@knox:";  //HARDCODED

                    //exec.Connect(address);
                    //exec.Login(username, password);
                    try
                    {
                        sshShell.Connect(port);
                        sshShell.Expect(prompt+"~#");
                    }
                    catch (Exception ex)
                    {
                        Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("JobID: {0} ConnectingERROR to RAPID7 server at {1} : " + ex.Message + " " + ex.InnerException, m_jobId, address));
                        //HARDCODED
                        address = "192.168.1.2";
                        username = "root";
                        password = "toor";  //Hello Kali
                        sshShell = new SshShell(address, username, password);
                        sshShell.RemoveTerminalEmulationCharacters = true;
                        prompt = "root";//@backtrack:";
                        Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("JobID: {0} Connecting to RAPID7 server at {1}", m_jobId, address));
                        try
                        {
                            sshShell.Connect(port);
                            sshShell.Expect(prompt);// + "~$");
                        }
                        catch (Exception ex2)
                        {
                            Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("JobID: {0} ConnectingERROR to RAPID7 server at {1} : " + ex2.Message + " " + ex2.InnerException, m_jobId, address));
                        }
                    }

                    Utils.Helper_Trace("XORCISM PROVIDER RAPID7", "Successfully connected via SSH to RAPID7 server");

                    string cmd1 = "cd /opt/rapid7/nexpose/nsc"; //HARDCODED
                    Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("Executing command [{0}]", cmd1));

                    string stdout = "";
                    //string stderr = "";
                    sshShell.WriteLine(cmd1);
                    //prompt = prompt+"/opt/rapid7/nexpose/nsc$";
                    //root@backtrack:/opt/rapid7/nexpose/nsc$
                    stdout = sshShell.Expect(prompt);

                    Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("START DUMP STDOUT"));
                    Utils.Helper_Trace("XORCISM PROVIDER RAPID7", stdout);
                    Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("END DUMP STDOUT"));
                    
                    cmd1 = "sudo ./nsc.sh"; //HARDCODED
                    Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("Executing command [{0}]", cmd1));

                    sshShell.WriteLine(cmd1);
                    sshShell.WriteLine(password);
                    //...
                    
                    Utils.Helper_Trace("XORCISM PROVIDER RAPID7", "Waiting until the NeXpose server starts...");
                    
                    Thread.Sleep(120000);   //Hardcoded
                    try
                    {
                        sshShell.Close();
                    }
                    catch (Exception ex)
                    {
                        Utils.Helper_Trace("XORCISM PROVIDER RAPID7", "Exception RestartNSC: " + ex.Message + " " + ex.InnerException);
                    }
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

                #region PCIDSS
                XmlNodeList list = doc.SelectNodes("/DetailVuln/PCI_FLAG"); //HARDCODED
                foreach (XmlNode n in list)
                {
                    if (n.InnerText == "1")
                    {
                        //TODO
                        //HARDCODED
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
                #endregion PCIDSS

                #region Compliance
                XmlNodeList nlist = doc.SelectNodes("/DetailVuln/COMPLIANCE/COMPLIANCE_INFO");  //HARDCODED

                foreach (XmlNode node in nlist)
                {
                    string[] SousCat = node.ChildNodes[1].InnerText.Split(new char[] { ' ' });

                    foreach (string SC in SousCat)
                    {
                        if (SC != "and")
                        {
                            //TODO
                            //...
                        }
                    }
                }
                #endregion Compliance

                return myIds;
            }

            private string Helper_ListCVEToString(List<VulnerabilityFound.Item> list)
            {
                string s = "";  //w00t

                foreach (VulnerabilityFound.Item item in list)
                    s = s + item.ID + ":" + item.Value + " / ";

                return s;
            }

            
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
                        
                            if (n.Name.ToUpper() == "CVE_ID_LIST")  //HARDCODED
                            {
                                Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("TEST CVE_ID_LIST"));
                                XmlNodeList mycves = n.ChildNodes;
                                foreach (XmlNode x in mycves)
                                {
                                    myCVEID = HelperGetChildInnerText(x, "ID");
                                    Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("TEST CVE: {0}", myCVEID));
                                    Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("TEST CVE2: {0}", x.InnerText));
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
                    Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("Error in Helper_GetCVE : Exception = {0}", ex.Message));
                }
                return l;
            }


            private List<VulnerabilityFound.Reference> Helper_GetREFERENCE(XmlNode node)
            {
                //...
                List<VulnerabilityFound.Reference> l = new List<VulnerabilityFound.Reference>();
                foreach (XmlNode child in node.ChildNodes)
                {
                    if (child.Name.ToUpper() == "VENDOR_REFERENCE_LIST")
                    {
                        Utils.Helper_Trace("XORCISM PROVIDER RAPID7", "VENDOR_REFERENCE_LIST");
                        foreach (XmlNode noderef in child.ChildNodes)
                        {
                            VulnerabilityFound.Reference refvuln = new VulnerabilityFound.Reference();
                            string refurl = HelperGetChildInnerText(noderef, "URL");
                            //HARDCODED
                            //TODO
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
                            Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("TEST REFERENCE: {0}", refurl));
                        }
                    }
                    else
                    {
                        //TODO
                        if (child.Name.ToUpper() == "BUGTRAQ_ID_LIST")  //HARDCODED
                        {
                            Utils.Helper_Trace("XORCISM PROVIDER RAPID7", "BUGTRAQ_ID_LIST");
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
                                Utils.Helper_Trace("XORCISM PROVIDER RAPID7", string.Format("TEST REFERENCEBID: {0}", refurl));
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
                    myJob.Status = XCommon.STATUS.FINISHED.ToString();  //COMPLETED
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
                    Utils.Helper_Trace("XORCISM PROVIDER RAPID7", "Exception = " + ex.Message);                    
                }
            }

            private void LoadXmlResult(string URLbase, string URL)
            {
                try
                {
                    string myUrl = URLbase + URL;
                    Utils.Helper_Trace("XORCISM PROVIDER RAPID7 - Loading XML Result", string.Format("URLBase = {0} & URLScan = {1}",URLbase,URL));
                    System.Text.UTF8Encoding objUTF8 = new System.Text.UTF8Encoding();
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(myUrl);
                    request.UserAgent = "Mozilla/1.0 (Windows; U; Windows NT 6.1; fr; rv:1.2.3.4) Gecko/20150401 Firefox/5.4.3";    //HARDCODED :)
                    request.CookieContainer = new CookieContainer();
                    ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate);
                    request.Credentials = CredentialCache.DefaultCredentials;
                    request.Method = "POST";
                    string postData = "nexposeccusername=jerome&nexposeccpassword=CHANGEME&login_button=Login&loginRedir="+URL; //HARDCODED
                    Utils.Helper_Trace("XORCISM PROVIDER RAPID7 - Loading XML Result", string.Format("PostData = {0}",postData));
                    byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.ContentLength = byteArray.Length;
                    Stream dataStream = request.GetRequestStream();
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();
                    // Get the response.
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    dataStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream);
                    string responseFromServer = reader.ReadToEnd();
                    Utils.Helper_Trace("XORCISM PROVIDER RAPID7 - Loading XML Result", string.Format("Response from server = {0}", responseFromServer));
                    m_data = responseFromServer;
                }
                catch (Exception ex)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER RAPID7", "Exception Load XML = " + ex.Message);
                    
                }
            }
        }

        internal class AcceptAllCertificatePolicy : ICertificatePolicy
        {
            //TODO
            public AcceptAllCertificatePolicy()
            {
            }

            public bool CheckValidationResult(ServicePoint sPoint, X509Certificate cert, WebRequest wRequest, int certProb)
            {
                //TODO
                // *** Always accept (this is BAD!)
                return true;
            }
        }
    }
}
