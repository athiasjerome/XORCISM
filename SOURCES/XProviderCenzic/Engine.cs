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

using System.Data;

using System.Data.SqlClient;

/*
 NOTES:
 IE http://111.222.333.444/Hailstorm.WS/ScanService.svc
 modifier le fichier hosts
 111.222.333.444	6667af4137f940b
*/
namespace XProviderCenzic
{
    /// <summary>
    /// Copyright (C) 2012-2015 Jerome Athias
    /// XORCISM Plugin for Cenzic Hailstorm (old version, using harcoded API requests)
    /// All trademarks and registered trademarks are the property of their respective owners.
    /// This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation; either version 2 of the License, or (at your option) any later version.
    /// 
    /// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
    /// 
    /// You should have received a copy of the GNU General Public License along with this program; if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
    /// </summary>
    public class Engine : MarshalByRefObject, XProviderCommon.IVulnerabilityDetector
    {
        static bool inerror = false;

        public Engine()
        {
            TextWriterTraceListener tw;
            tw = new TextWriterTraceListener("XProviderCenzic.log");    //Hardcoded

            Trace.AutoFlush = true;
            Trace.IndentSize = 4;
            Trace.Listeners.Add(tw);
        }

        public void Run(string target, int jobID, string policy, string strategy)
        {
            Utils.Helper_Trace("XORCISM PROVIDER Cenzic", "Entering Run()");
            Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("Target = {0} , JobID = {1} , Policy = {2}, Strategy = {3}", target, jobID, policy, strategy));

            CenzicParser CenzicParser = new CenzicParser(target, jobID, policy, strategy);
            inerror = false;
            CenzicParser.DoIt(jobID, target);
            if (!inerror)
            {
                Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("JobID:" + jobID + " Parsing the data"));
                CenzicParser.parse();
                Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("JobID:" + jobID + " End of data processing"));

                Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("Updating job {0} status to FINISHED", jobID));

                CenzicParser.UpdateJob(jobID);
            }
            else
            {
                Utils.Helper_Trace("XORCISM PROVIDER Cenzic", "JobID:" + jobID + " inerror");
            }
            Utils.Helper_Trace("XORCISM PROVIDER Cenzic", "JobID:" + jobID + " Leaving Run()");
        }

        
/*
        private VULNERABILITYFOUND PersisteVuln(string cve, string CenzicID, XmlNode diag, XmlNode consequence, XmlNode solution, int endPointID, string severity)
        {
            return null;
        }
*/
        private int SearchForCenzicID(string CenzicID)
        {
            return -1;
        }

        class CenzicParser
        {
            private string m_target;
            private int m_jobId;
            private string m_policy;
            private string m_data;
            private string m_strategy;
            private int m_assetId;
            private int m_sessionId;

            public CenzicParser(string target, int jobID, string policy, string strategy)
            {
                m_target = target;
                m_jobId = jobID;
                m_policy = policy;
                m_strategy = strategy;
            }

            /*******************************************************************************************************************
            Notes on Authentication and Security

            The use of this Web service assumes SSL encryption for all communications.
             * The “clientId” is considered confidential information that should not be disclosed 
             * unless there is no concern about unauthorized requests to the Web service.
             * Additional security may be enforced by configuring Microsoft IIS to require client side certificates. 
             * Implementation of clients which supply these certificates, and installation of the certificates on the clients 
             * is the responsibility of the client developer.
            *******************************************************************************************************************/

            public void DoIt(int jobID, string myTarget)
            {
                Assembly a;
                a = Assembly.GetExecutingAssembly();

                XORCISMEntities model;
                model = new XORCISMEntities();

                Utils.Helper_Trace("XORCISM PROVIDER Cenzic", "Assembly location = " + a.Location);

                string URI = string.Empty;
                string UrlScan = string.Empty;

                //http://111.222.1.192/Hailstorm.WS/ScanService.svc
                //6667af4137f746b
                //Modify the hosts file

                HailstormWebService.IScanService MyCenzicService = new HailstormWebService.ScanServiceClient("cenzicHttpEndpoint");

                Utils.Helper_Trace("XORCISM PROVIDER Cenzic", "OK FOR ME");
                
                string CenzicStatuses = string.Empty;
                
//                CenzicStatuses = MyCenzicService.GetUpdatedStatuses();
//                Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("CenzicStatuses = {0}", CenzicStatuses));

                /*
                byte[] toto;
                toto = MyCenzicService.GetReport("1", "2", "", "ScanIntralot", false);
                Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("GetReport = {0}", toto.ToString()));
                */
                //DataSet ds = new DataSet();
                long ticks;
                ticks = DateTime.Now.Ticks;

                string requestId;
                //requestId = string.Format("{0}_{1}", ticks, this.GetHashCode());
                requestId = string.Format("{0}", ticks);

                Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("JobID:" + jobID + " QueueAssessmentRun - Target={0} - RequestId={1}", myTarget, requestId));

                //Checking for parameters (credentials)
                var tmpUser = from U in model.JOB
                                  where U.JobID == m_jobId
                                  select U.SESSION.UserID;

                Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("JobID:" + jobID + " User ID = {0}", tmpUser.FirstOrDefault()));

                    var tmpAccount = from Ac in model.USERACCOUNT
                                     where Ac.UserID == tmpUser.FirstOrDefault()
                                     select Ac.ACCOUNT;

                    ACCOUNT xAccount = tmpAccount.FirstOrDefault();

                    Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("JobID:" + jobID + " Account ID = {0}", xAccount.AccountID));
                    //TODO  ipaddressIPv4
                    var tmpAsset = from A in model.ASSET
                                   where A.ipaddressIPv4 == m_target
                                   //&& A.AccountID == xAccount.AccountID
                                   select A;

                    ASSET xAsset = tmpAsset.FirstOrDefault();
                    //TODO  ipaddressIPv4
                    Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("JobID:" + jobID + " Asset ID = {0}    IP = {1}", xAsset.AssetID, xAsset.ipaddressIPv4));
                    m_assetId = xAsset.AssetID;

                    var mySession = from j in model.JOB
                                  where j.JobID == m_jobId
                                  select j.SessionID;
                    m_sessionId = (int)mySession.FirstOrDefault();

                    var Credentials = from oCredentials in model.ASSETCREDENTIAL
                                      where oCredentials.AssetID == xAsset.AssetID
                                      select oCredentials;

                    string AssetCredentials;
                    AssetCredentials = string.Empty;

                    foreach (ASSETCREDENTIAL AC in Credentials.ToList())
                    {
                        switch (AC.AuthenticationType)
                        {
                            case "HTTP":
                                {
                                    AssetCredentials = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
                                    AssetCredentials += "<Parameters><Login><UserName>" + AC.Username + "</UserName><Password>" + AC.Password + "</Password></Login>";
                                    AssetCredentials += "</Parameters>";
                                    Utils.Helper_Trace("XORCISM PROVIDER Cenzic", "Using HTTP Credentials");
                                    break;
                                }
                            case "Web":
                                {
                                    AssetCredentials = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
                                    AssetCredentials += "<Parameters><Login><UserName>" + AC.Username + "</UserName><Password>" + AC.Password + "</Password></Login>";
                                    AssetCredentials += "</Parameters>";
                                    Utils.Helper_Trace("XORCISM PROVIDER Cenzic", "Using Web Credentials");
                                    break;
                                }
                        }
                    }

                try
                {
                    string myTargetHTTP=string.Empty;
                    if (myTarget.ToLower().Contains("http://") || myTarget.ToLower().Contains("https://"))
                    {
                        myTargetHTTP = myTarget;
                    }
                    else
                    {
                        myTargetHTTP = "http://" + myTarget;
                        Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("myTargetHTTP={0}", myTargetHTTP));
                    }
                    //MyCenzicService.QueueAssessmentRun("Jerome Athias @ XORCISM", requestId, "http://www.xorcism.org", "Best Practices Turbo", "");
                    //MyCenzicService.QueueAssessmentRun("1", requestId, "http://www.xorcism.org", "Best Practices Turbo", "");
                    string MyPolicy;
                    MyPolicy = string.Empty;
                    //HARDCODED
                    MyPolicy = "Best Practices_Turbo";
                    switch (m_policy)
                    {
                        case "Normal":
                            MyPolicy = "Best Practices_Turbo";
                            break;
                        case "Moderate":
                            MyPolicy = "OWASP-2010_Turbo";
                            break;
                        case "Intrusive":
                            MyPolicy = "OWASP-2010_Turbo";
                            break;
                        case "Web":
                            MyPolicy = "OWASP-2010_Turbo"; //internet-audit
                            break;
                        case "PCI DSS":
                            MyPolicy = "PCI_Turbo";
                            break;
                        case "HIPAA":
                            MyPolicy = "HIPAA_Turbo";
                            break;
                        case "GLBA":
                            MyPolicy = "GLBA_Turbo";
                            break;
                        default:
                            MyPolicy = "Best Practices_Turbo";
                            break;
                    }
                    MyCenzicService.QueueAssessmentRun("1", requestId, myTargetHTTP, MyPolicy, AssetCredentials);
                    //Best Practives_Turbo
                    //OWASP_Turbo
                    //OWASP-2010_Turbo
                    //HIPAA_Turbo
                    //GLBA_Turbo
                    //PCI_Turbo
                }
                catch (Exception ex)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("JobID:" + jobID + " Error in QueueAssessmentRun : Exception = {0} {1}", ex.Message, ex.InnerException));
                    //A scan is currently in progress.  You cannot request a rescan until the current scan finishes.
                    //There was no endpoint listening at http://666.222.1.192/Hailstorm.WS/ScanService.svc that could accept the message. This is often caused by an incorrect address or SOAP action. See InnerException, if present, for more details.
                    //The content type multipart/related; type="application/xop+xml"
                    //if (ex.Message.Contains("xop+xml"))
                    if (ex.Message.Contains("perenoël"))
                    {
                        //Continue since the scan is launched, despite of the error
                    }
                    else
                    {
                        Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("Updating job {0} status to ERROR", jobID));
                        XCommon.Utils.Helper_SendEmail("contact@hackenaton.org", "Error Cenzic", "Error in QueueAssessmentRun : Exception = " + ex.Message + " " + ex.InnerException);

                        var Q = from j in model.JOB
                                where j.JobID == jobID
                                select j;

                        JOB myJob = Q.FirstOrDefault();
                        myJob.Status = XCommon.STATUS.ERROR.ToString();
                        myJob.DateEnd = DateTimeOffset.Now;
                        Regex objNaturalPattern = new Regex("A scan is currently in progress");
                        string strError = objNaturalPattern.Match(ex.Message).ToString();
                        if (strError != "")
                        {
                            myJob.ErrorReason = "A scan is currently in progress";
                            XCommon.Utils.Helper_SendEmail("contact@hackenaton.org", "Error Cenzic double scan", "A scan is currently in progress for the asset: " + m_target);
                        }
                        objNaturalPattern = new Regex("There was no endpoint listening");
                        strError = objNaturalPattern.Match(ex.Message).ToString();
                        if (strError != "")
                        {
                            myJob.ErrorReason = "There was no endpoint listening";
                            XCommon.Utils.Helper_SendEmail("contact@hackenaton.org", "Check Cenzic Server", "Please check that Cenzic server is running");
                            //Email Service : Error sending email to the administrator : Transaction failed. The server response was: 5.7.1 <contact@hackenaton.org>: Relay access denied
                        }

                        model.SaveChanges();

                        Utils.Helper_Trace("XORCISM PROVIDER Cenzic", "JobID:" + jobID + " Leaving Run()");
                        inerror = true;
                        return;
                    }
                }

                //CenzicStatuses = MyCenzicService.GetUpdatedStatuses();
                //Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("CenzicStatuses = {0}", CenzicStatuses));
                /*
                <?xml version="1.0" encoding="utf-8"?>
                <ReturnValue>
                  <ReferenceDateTime>1/28/2011 7:53:52 AM</ReferenceDateTime>
                  <ScanStatuses>
                    <ScanStatus>
                      <ClientId>1</ClientId>
                      <RequestId>634318177766262387</RequestId>
                      <Status>success</Status>
                      <Description />
                      <PercentComplete />
                    </ScanStatus>
                    <ScanStatus>
                      <ClientId>1</ClientId>
                      <RequestId>634318304350332582</RequestId>
                      <Status>Queued</Status>
                      <Description />
                      <PercentComplete />
                    </ScanStatus>
                  </ScanStatuses>
                </ReturnValue>
                */
              
                Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("Waiting until the scan finishes"));

                bool NotYet = true;
                while (NotYet)
                {
                    //TODO: check if the session is canceled

                    Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("JobID:" + jobID + " Polling..."));
                    int cptretrypoll = 0;
                    while (cptretrypoll < 10)
                    {
                        try
                        {
                            CenzicStatuses = MyCenzicService.GetStatus("1", requestId, "");
                            cptretrypoll = 10;  //endwhile
                        }
                        catch (Exception ex)
                        {
                            Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("JobID:" + jobID + " ErrorPolling: " + ex.Message + " " + ex.InnerException));
                            //There was no endpoint listening at
                            Thread.Sleep(120000);
                            cptretrypoll++;
                        }
                    }
                    Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("JobID:" + jobID + " GetStatus = {0}", CenzicStatuses));
                    /*
                    <?xml version="1.0" encoding="utf-8"?>
                    <ReturnValue>
                      <Status>Queued</Status>
                      <Description />
                      <PercentComplete>0</PercentComplete>
                    </ReturnValue>
                    */
                    //Queued, Waiting, Running, success, Incomplete, failure
                    XmlDocument doc = new XmlDocument();
                    try
                    {
                        doc.LoadXml(CenzicStatuses);
                        //SR.Close();
                        
                    }
                    catch (Exception ex)
                    {
                        Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("JobID:" + jobID + " Exception GetStatus = {0}", ex));
                    }
                            
                    Regex objNaturalPattern = new Regex("<Status>[^<>]*</Status>");
                    string CurrentStatus = string.Empty;
                    CurrentStatus = objNaturalPattern.Match(CenzicStatuses).ToString();
                    if (CurrentStatus != "")
                    {
                        Utils.Helper_Trace("XORCISM PROVIDER Cenzic", "JobID:" + jobID + " CurrentStatus=" + CurrentStatus);
                    }

                    if (CurrentStatus.ToLower() == "<status>success</status>" || CurrentStatus.ToLower() == "<status>complete</status>")
                    {
                        Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("JobID:" + jobID + " The scan has finished"));
                        NotYet = false;
                    }
                    else
                    {
                        if (CurrentStatus.ToLower() == "<status>failure</status>")
                        {
                            Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("JobID:" + jobID + " The scan failed"));
                            NotYet = true;
                            inerror = true;

                            //XORCISMEntities model = new XORCISMEntities();
                            var Q = from o in model.JOB
                                    where o.JobID == jobID
                                    select o;
                            JOB myJob = Q.FirstOrDefault();
                            myJob.Status = XCommon.STATUS.ERROR.ToString();
                            myJob.ErrorReason = "failure";
                            myJob.DateEnd = DateTimeOffset.Now;
                            model.SaveChanges();

                            return;
                        }
                        Thread.Sleep(300000);
                    }                                           
                }

                CenzicStatuses = MyCenzicService.GetResults("1", requestId, "");
                Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("JobID:" + jobID + " GetResults for requestId:{1} = {0}", CenzicStatuses, requestId));
                m_data = CenzicStatuses;
                /*
                CenzicStatuses = MyCenzicService.GetResults("1", "634318177766262387", "");
                Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("GetResults for requestId:{1} = {0}", CenzicStatuses, "634318177766262387"));

                CenzicStatuses = MyCenzicService.GetResults("1", "634318304350332582", "");
                Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("GetResults for requestId:{1} = {0}", CenzicStatuses, "634318304350332582"));

                CenzicStatuses = MyCenzicService.GetResults("1", "634319094942031841", "");
                Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("GetResults for requestId:{1} = {0}", CenzicStatuses, "634319094942031841"));

                CenzicStatuses = MyCenzicService.GetResults("1", "634319058323917404", "");
                Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("GetResults for requestId:{1} = {0}", CenzicStatuses, "634319058323917404"));

                CenzicStatuses = MyCenzicService.GetResults("1", "634318467341825162", "");
                Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("GetResults for requestId:{1} = {0}", CenzicStatuses, "634318467341825162"));

                CenzicStatuses = MyCenzicService.GetResults("1", "634319046050405399", "");
                Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("GetResults for requestId:{1} = {0}", CenzicStatuses, "634319046050405399"));

                CenzicStatuses = MyCenzicService.GetResults("1", "634319119641834588", "");
                Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("GetResults for requestId:{1} = {0}", CenzicStatuses, "634319119641834588"));

                CenzicStatuses = MyCenzicService.GetResults("1", "634318989893613415", "");
                Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("GetResults for requestId:{1} = {0}", CenzicStatuses, "634318989893613415"));

                CenzicStatuses = MyCenzicService.GetResults("1", "634318327288844590", "");
                Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("GetResults for requestId:{1} = {0}", CenzicStatuses, "634318327288844590"));

                CenzicStatuses = MyCenzicService.GetResults("1", "634319066825343658", "");
                Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("GetResults for requestId:{1} = {0}", CenzicStatuses, "634319066825343658"));

                CenzicStatuses = MyCenzicService.GetResults("1", "634319129409083243", "");
                Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("GetResults for requestId:{1} = {0}", CenzicStatuses, "634319129409083243"));

                CenzicStatuses = MyCenzicService.GetResults("1", "634319152953029880", "");
                Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("GetResults for requestId:{1} = {0}", CenzicStatuses, "634319152953029880"));

                CenzicStatuses = MyCenzicService.GetResults("1", "634319072788084707", "");
                Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("GetResults for requestId:{1} = {0}", CenzicStatuses, "634319072788084707"));

                CenzicStatuses = MyCenzicService.GetResults("1", "634319147696309213", "");
                Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("GetResults for requestId:{1} = {0}", CenzicStatuses, "634319147696309213"));

                CenzicStatuses = MyCenzicService.GetResults("1", "634319076660046171", "");
                Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("GetResults for requestId:{1} = {0}", CenzicStatuses, "634319076660046171"));

                CenzicStatuses = MyCenzicService.GetResults("1", "634318348947073370", "");
                Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("GetResults for requestId:{1} = {0}", CenzicStatuses, "634318348947073370"));

                CenzicStatuses = MyCenzicService.GetResults("1", "634319051309296191", "");
                Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("GetResults for requestId:{1} = {0}", CenzicStatuses, "634319051309296191"));
                */
                /*
                <?xml version="1.0" encoding="utf-8"?>
                <AssessmentRunData RequestId="634318304350332582">
                  <SmartAttacks />
                  <ASMSummaryData />
                </AssessmentRunData>
                */

                
            }


            public void parse()
            {
                Assembly a;
                a = Assembly.GetExecutingAssembly();

                Utils.Helper_Trace("XORCISM PROVIDER CENZIC", "Assembly location = " + a.Location);

                XmlDocument doc = new XmlDocument();

                #region HackCenzic
                /*
                string filename;
                filename = @"C:\Cenzic_webscan.xml";                
                
                doc.Load(filename);

                Utils.Helper_Trace("XORCISM PROVIDER CENZIC", string.Format("HackFile should be located at : " + filename));
                */
                #endregion

                // ============================================
                // Parse the XML Document and populate the database
                // ============================================

                string protocol = string.Empty;
                //int port = -1;
                string service = string.Empty;
                //bool PatchUpgrade = false;
                //string title;
                //string MSPatch = "";
                //string Solution;

                m_data = m_data.Replace("Configurable format #", "Configurable");
                Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("m_data = {0}", m_data));                
                try
                {
                    Utils.Helper_Trace("XORCISM PROVIDER Cenzic", "Loading the XML document");
                    
                    doc.LoadXml(m_data);
                    
                }
                catch (Exception ex)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("JobID:" + m_jobId + " Exception = {0} / {1}", ex.Message, ex.InnerException == null ? "" : ex.InnerException.Message));
                }
                

                XORCISMEntities model;
                model = new XORCISMEntities();

                string query = "/AssessmentRunData/SmartAttacks/SmartAttacksData";  //Hardcoded

                XmlNodeList report;
                report = null;
                try
                {
                    report = doc.SelectNodes(query);
                }
                catch (Exception ex)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("JobID:" + m_jobId + " Error SelectNodes({0}) : Exception = {1}", query, ex.Message));
                }



                int myPort = 80;
                if (m_target.Contains("https://"))
                {
                    myPort = 443;
                }
                //Check if we have a custom port, ex: http://10.20.30.40:8080/test
                string strTargetTest = m_target;
                strTargetTest = strTargetTest.Replace("http://", "");
                strTargetTest = strTargetTest.Replace("https://", "");
                try
                {
                    if (strTargetTest.Contains(":"))
                    {
                        char[] splitter = { ':' };
                        string[] strSplit = strTargetTest.Split(splitter);
                        strTargetTest = strSplit[1];
                        if (strTargetTest.Contains("/"))
                        {
                            strSplit = strTargetTest.Split(new Char[] { '/' });
                            strTargetTest = strSplit[0];
                        }
                        try
                        {
                            myPort = Convert.ToInt32(strTargetTest);
                        }
                        catch (FormatException e)
                        {
                            Utils.Helper_Trace("XORCISM PROVIDER Cenzic", strTargetTest + " is not a sequence of digits.");
                        }
                        Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("Custom Port:{0}", strTargetTest));
                    }
                }
                catch (Exception ex)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("JobID:" + m_jobId + " Error in strTargetTest : Exception = {0}", ex.Message));
                }

                VulnerabilityEndPoint vulnerabilityEndPoint = new VulnerabilityEndPoint();
                vulnerabilityEndPoint.IpAdress = m_target;
                vulnerabilityEndPoint.Protocol = "TCP"; // "http";
                vulnerabilityEndPoint.Port = myPort;
                vulnerabilityEndPoint.Service = "WWW";

                int myEndpointID = 0;
                var testEndpoint = from e in model.ENDPOINT
                                   where e.AssetID == m_assetId && e.SessionID == m_sessionId
                                   select e;
                if (testEndpoint.Count() == 0)
                {
                    ENDPOINT newEndpoint = new ENDPOINT();
                    newEndpoint.AssetID = m_assetId;
                    newEndpoint.SessionID = m_sessionId;
                    newEndpoint.ProtocolName = "TCP"; // "http";
                    newEndpoint.PortNumber = myPort;
                    newEndpoint.Service = "WWW";
                    model.ENDPOINT.Add(newEndpoint);
                    model.SaveChanges();
                    myEndpointID = newEndpoint.EndPointID;
                }
                else
                {
                    myEndpointID = testEndpoint.FirstOrDefault().EndPointID;
                }
                Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("myEndpointID:{0}", myEndpointID));



                Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("JobID:" + m_jobId + " Found {0} SmartAttacks to parse", report.Count));

                foreach (XmlNode reportHost in report)
                {
                    // ==================================
                    // Handle every SmartAttacksData tag
                    // ==================================

                    
                    string myInnerXml = string.Empty;
                    string myTitle = string.Empty;
                    string myDescription = string.Empty;
                    string myConsequence = string.Empty;
                    string myResult = string.Empty;
                    string mySolution = string.Empty;

                    string myCVE = string.Empty;
                    MatchCollection myCVEs;
                    string myPCI = string.Empty;
                    string myMessage = string.Empty;

                    foreach (XmlNode n in reportHost.ChildNodes)
                    {
                        //SmartAttackInfo
                        //ReportItems
                        XmlNodeList Childs = n.ChildNodes;

                        Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("Coucou 1"));
                        try
                        {
                            if (n.Name == "SmartAttackInfo")
                            {
                                myInnerXml = n.OuterXml;
                                myTitle = HelperGetChildInnerText(n, "SmartAttackName");
                                Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("JobID:" + m_jobId + " Found SmartAttackName:{0}", myTitle));
                                Regex myRegex = new Regex("PCI [0-9].[0-9].[0-9]");

                                myPCI = myRegex.Match(myTitle).ToString();
                                if (myPCI != "")
                                {
                                    Utils.Helper_Trace("XORCISM PROVIDER Cenzic", "PCI=" + myPCI);
                                }

                                //Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("myInnerXml:{0}", myInnerXml));
                                myDescription = HelperGetChildInnerText(n, "Description");
                                myConsequence = HelperGetChildInnerText(n, "HowItWorks");
                                myResult = HelperGetChildInnerText(n, "Impact");
                                mySolution = HelperGetChildInnerText(n, "Remediation");
                            }
                        }
                        catch (Exception ex)
                        {
                            Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("JobID:" + m_jobId + " Error in SmartAttackInfo : Exception = {0}", ex.Message));
                        }
                        if (n.Name == "ReportItems")
                        {
                            Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("Coucou 2"));
                            foreach (XmlNode x in n.ChildNodes)
                            {
                                //ReportItem
                                foreach (XmlNode ReportItem in x.ChildNodes)
                                {
                                    myMessage = "";
                                    if (ReportItem.Name == "ReportItemType")
                                    {
                                        //Pass
                                        if (ReportItem.InnerText == "Information")
                                        {
                                            try
                                            {
                                                //TODO
                                                /*

                                                Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("Information"));
                                                INFORMATION newInformation = new INFORMATION();
                                                //newInformation.InnerXml
                                                newInformation.Title = myTitle;
                                                newInformation.Description = myDescription;
                                                newInformation.Consequence = myConsequence;
                                                newInformation.Result = myResult;
                                                newInformation.Solution = mySolution;
                                                newInformation.Severity = HelperGetChildInnerText(x, "Severity");
                                                newInformation.HarmScore = int.Parse(HelperGetChildInnerText(x, "HarmScore"));
                                                myMessage = HelperGetChildInnerText(x, "Message");
                                                newInformation.Message = myMessage;
                                                //TODO A FAIRE
                                                //Matching avec les références
                                                //http://www.securityfocus.com/bid/43140/info 
                                                //http://www.securityfocus.com/bid/43140/solution 
                                                newInformation.Url = HelperGetChildInnerText(x, "Url");
                                                newInformation.rawrequest = HelperGetChildInnerText(x, "HttpRequest");
                                                newInformation.rawresponse = HelperGetChildInnerText(x, "HttpResponse");
                                                if (myPCI != "")
                                                {
                                                    newInformation.PCI_FLAG = true;
                                                }
                                                newInformation.JobID = m_jobId;
                                                newInformation.EndPointID = myEndpointID;
                                                model.AddToINFORMATION(newInformation);
                                                model.SaveChanges();
                                                */
                                            }
                                            catch (Exception ex)
                                            {
                                                Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("JobID:" + m_jobId + " Error in Information : Exception = {0}. {1}", ex.Message, ex.InnerException));
                                            }
                                        }
                                        if (ReportItem.InnerText == "Warning")
                                        {
                                            try
                                            {
                                                Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("Warning"));
                                                VulnerabilityFound vulnerabilityFound = new VulnerabilityFound();
                                                vulnerabilityFound.InnerXml = myInnerXml;
                                                vulnerabilityFound.Title = myTitle;
                                                Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("Adding SmartAttackName:{0}", myTitle));
                                                vulnerabilityFound.Description = myDescription;
                                                vulnerabilityFound.Consequence = myConsequence;
                                                vulnerabilityFound.Result = myResult;
                                                vulnerabilityFound.Solution = mySolution;

                                                if (myPCI != "")
                                                {
                                                    vulnerabilityFound.PCI_FLAG = true;
                                                }

                                                //ReportItemCreateDate
                                                vulnerabilityFound.Severity = HelperGetChildInnerText(x, "Severity");
                                                //Low, Medium, High
                                                //Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("WARNING Severity:{0}", HelperGetChildInnerText(x, "Severity")));
                                                vulnerabilityFound.HarmScore = int.Parse(HelperGetChildInnerText(x, "HarmScore"));
                                                //Count
                                                myMessage=HelperGetChildInnerText(x, "Message");
                                                //vulnerabilityFound.Message = myMessage; //not exact because same VULNERABILITY will have various Messages
                                                vulnerabilityFound.rawresponse = myMessage;
                                            
                                                    //Regex objNaturalPattern = new Regex("CVE-[0-9][0-9][0-9][0-9]-[0-9][0-9][0-9][0-9]");
                                                Regex myRegexCVE = new Regex(@"CVE-(19|20)\d\d-(0\d{3}|[1-9]\d{3,})");
                                                //https://cve.mitre.org/cve/identifiers/tech-guidance.html    

                                                    /*
                                                    myCVE = objNaturalPattern.Match(myMessage).ToString();                                               
                                                    if (myCVE != "")
                                                    {
                                                        Utils.Helper_Trace("XORCISM PROVIDER Cenzic", "CVE=" + myCVE);
                                                    }
                                                    */
                                                    List<VulnerabilityFound.Item> l;
                                                    l = new List<VulnerabilityFound.Item>();
                                                    myCVEs = myRegexCVE.Matches(myMessage);
                                                    foreach (Match match in myCVEs)
                                                    {
                                                        foreach (Capture capture in match.Captures)
                                                        {
                                                            Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("Index={0}, CVE={1}", capture.Index, capture.Value));
                                                            VulnerabilityFound.Item item;
                                                            item = new VulnerabilityFound.Item();
                                                            item.Value = capture.Value;
                                                            item.ID = "cve";
                                                            l.Add(item);
                                                        }
                                                    }
                                                    vulnerabilityFound.ListItem = l;


                                                vulnerabilityFound.Url = HelperGetChildInnerText(x, "Url");
                                                Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("Url={0}", HelperGetChildInnerText(x, "Url")));
                                                vulnerabilityFound.rawrequest = HelperGetChildInnerText(x, "HttpRequest");
                                                //vulnerabilityFound.rawresponse = HelperGetChildInnerText(x, "HttpResponse");
                                                //StructuredData

                                                //*** Compliances? voir en bas
                                                //http://www.cenzic.com/downloads/Cenzic_CWE.pdf
                                                int VulnID = VulnerabilityPersistor.Persist(vulnerabilityFound, vulnerabilityEndPoint, m_jobId, "cenzic", model);
                                            }
                                            catch (Exception ex)
                                            {
                                                Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("JobID:" + m_jobId + " Error in Warning : Exception = {0}. {1}", ex.Message, ex.InnerException));
                                            }
                                        }
                                        if (ReportItem.InnerText == "Vulnerable")
                                        {
                                            try
                                            {
                                                Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("Vulnerable"));
                                                VulnerabilityFound vulnerabilityFound = new VulnerabilityFound();
                                                vulnerabilityFound.InnerXml = myInnerXml;
                                                vulnerabilityFound.Title = myTitle;
                                                Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("Adding SmartAttackName:{0}", myTitle));
                                                vulnerabilityFound.Description = myDescription;
                                                vulnerabilityFound.Consequence = myConsequence;
                                                vulnerabilityFound.Result = myResult;
                                                vulnerabilityFound.Solution = mySolution;

                                                //ReportItemCreateDate
                                                vulnerabilityFound.Severity = HelperGetChildInnerText(x, "Severity");
                                                //Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("VULNERABLE Severity:{0}", HelperGetChildInnerText(x, "Severity")));
                                                vulnerabilityFound.HarmScore = int.Parse(HelperGetChildInnerText(x, "HarmScore"));
                                                //Count
                                                myMessage = HelperGetChildInnerText(x, "Message");
                                                //vulnerabilityFound.Message = myMessage;
                                                vulnerabilityFound.rawresponse = myMessage;

                                                    //Regex objNaturalPattern = new Regex("CVE-[0-9][0-9][0-9][0-9]-[0-9][0-9][0-9][0-9]");
                                                    Regex objNaturalPattern = new Regex(@"CVE-[0-9][0-9][0-9][0-9]-\d+");
                                                    
                                                    /*
                                                    myCVE = objNaturalPattern.Match(myMessage).ToString();
                                                    if (myCVE != "")
                                                    {
                                                        Utils.Helper_Trace("XORCISM PROVIDER Cenzic", "CVE=" + myCVE);
                                                    }
                                                    */
                                                
                                                    List<VulnerabilityFound.Item> l;
                                                    l = new List<VulnerabilityFound.Item>();
                                                    myCVEs = objNaturalPattern.Matches(myMessage);
                                                    foreach (Match match in myCVEs)
                                                    {
                                                        foreach (Capture capture in match.Captures)
                                                        {
                                                            Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("Index={0}, CVE={1}", capture.Index, capture.Value));
                                                            VulnerabilityFound.Item item;
                                                            item = new VulnerabilityFound.Item();
                                                            item.Value = capture.Value;
                                                            item.ID = "cve";
                                                            l.Add(item);
                                                        }
                                                    }
                                                    vulnerabilityFound.ListItem = l;
                                            
                                                vulnerabilityFound.Url = HelperGetChildInnerText(x, "Url");
                                                Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("Url={0}", HelperGetChildInnerText(x, "Url")));
                                                vulnerabilityFound.rawrequest = HelperGetChildInnerText(x, "HttpRequest");
                                                //vulnerabilityFound.rawresponse = HelperGetChildInnerText(x, "HttpResponse");                                            
                                                //StructuredData

                                                if (myPCI != "")
                                                {
                                                    vulnerabilityFound.PCI_FLAG = true;
                                                    int VulnID = VulnerabilityPersistor.Persist(vulnerabilityFound, vulnerabilityEndPoint, m_jobId, "cenzic", model);


                                                    //TODO
                                                    /*
                                                    List<int> myIds = new List<int>();
                                                    var id = from o in model.COMPLIANCECATEG
                                                             where o.Title == myTitle &&
                                                             o.COMPLIANCE.Title == "PCIDSS"
                                                             select o.ComplianceCategID;
                                                    int Id = id.FirstOrDefault();

                                                    myIds.Add(Id);

                                                    List<int> Compliances = new List<int>();
                                                    Compliances = myIds;
                                                    Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("Vulnerability persisted , VulnID = {0} & Compliance count = {1}", VulnID, Compliances.Count));
                                                    var V = from tmpVuln in model.VULNERABILITYFOUND
                                                            where tmpVuln.VulnerabilityFoundID == VulnID
                                                            select tmpVuln;

                                                    VULNERABILITYFOUND VF = V.FirstOrDefault();

                                                    foreach (int i in Compliances)
                                                    {
                                                        Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("Categorie Compliance => ", i));
                                                        var C = from Comp in model.COMPLIANCECATEG
                                                                where Comp.ComplianceCategID == i
                                                                select Comp;

                                                        COMPLIANCECATEG myCompliance = new COMPLIANCECATEG();
                                                        myCompliance = C.FirstOrDefault();

                                                        VF.COMPLIANCECATEG.Add(myCompliance);

                                                        model.SaveChanges();
                                                        Utils.Helper_Trace("XORCISM PROVIDER Cenzic", "Mapping Compliance-Vulnerability Added");
                                                    }
                                                    */
                                                }
                                                else
                                                {
                                                    int VulnID = VulnerabilityPersistor.Persist(vulnerabilityFound, vulnerabilityEndPoint, m_jobId, "cenzic", model);
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("Error in Vulnerable : Exception = {0}. {1}", ex.Message, ex.InnerException));
                                            }
                                        }
                                    }
                                }
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

            public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            {
                return true;
            }

            //void QueueAssessmentRun(string clientId, string requestId, string url, string assessmentType, string parameters)
            private string Helper_Request_ADD(string sessionID, string id, string SiteName, string Description, string HostName, string Credentials)
            {
                string MyPolicy;
                MyPolicy = string.Empty;
                /*
                Assessment Type
                This can be a keyword or a template name.
                If a keyword is used, the lookup table request_assessment_type_lookup must contain a record mapping of this keyword to a valid template name.
                */
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
                    default:
                        MyPolicy = "network-audit";
                        break;
                }

                string data;
                data = string.Empty;
                //HARDCODED CREDENTIALS
                data = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
                data += "<Parameters>";
                data += "<Login>";
                data += "<UserName>user123</UserName>";
                data += "<Password>password1234</Password>";
                data += "</Login>";
                data += "<Contact>";
                data += "<Email>joesmith@cenzic.com</Email>";
                data += "<CustomerName>JoeSmith</CustomerName>";
                data += "<CompanyName>FakeCompanyName</CompanyName>";
                data += "</Contact>";
                data += "<RunAppAnalysis>true</RunAppAnalysis>";
                data += "<ApplicationName>My Application</ApplicationName>";
                data += "</Parameters>";

                return data;
            }

            //string GetStatus(string clientId, string requestId, string parameters)
            /*
            Purpose: Returns the current assessment status using the client Id and request Id.
            Parameters: The "parameters" is reserved for future use.
            */
            private string Helper_GetStatus(string sessionID, string id, string SiteName, string Description, string HostName, string Credentials)
            {
                string data;
                data = string.Empty;
                
                data = "<?xml version=\"1.0\" encoding=\"utf-8\"?> ";
                data += "<ReturnValue>";
                data += "<RequestId>xxxxxxx</RequestId>";
                data += "<Status>Processing</Status>";
                data += "<Description>The assessment request has been sent to Hailstorm for processing.</Description>";
                data += "<IsActive>True</IsActive>";
                data += "<NextAction>Terminate</NextAction>";
                data += "<EndTime>04/01/2010 5:40:00 PM</EndTime>";
                data += "</ReturnValue>";

                return data;
            }

            




























            private string Helper_ReportSave(string sessionID, string ConfigID, string Name, string SiteID)
            {
                string data;
                data = string.Empty;

                data = "<ReportSaveRequest session-id=\"" + sessionID + "\" generate-now=\"1\">";
                data += "<ReportConfig id=\"" + ConfigID + "\" name=\"" + Name + "\" template-id=\"audit-report\" format=\"ns-xml\" owner=\"1\" timezone=\"America/New_York\">";

                data += "<Filters><filter type=\"site\" id=\"" + SiteID + "\"></filter>";
                data += "<filter type=\"vuln-severity\" id=\"1\">1</filter></Filters>";
                data += "<Generate after-scan=\"0\" schedule=\"0\"></Generate>";
                data += "<Delivery><Storage StoreOnServer=\"1\"></Storage></Delivery>";

                data += "</ReportConfig>";
                data += "</ReportSaveRequest>";

                return data;
            }

            private void ResumeScan(string sessionID, string scanid)
            {
                ASCIIEncoding encoding = new ASCIIEncoding();
                string data = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><LoginRequest session-id=\"" + sessionID + "\" scan-id=\"" + scanid + "!!!\"/>";
                byte[] Content = encoding.GetBytes(data);
                HttpWebRequest request;
                request = (HttpWebRequest)HttpWebRequest.Create("https://111.222.333.444:3780/api/1.1/xml");
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

                }
                SR.Close();
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
                    XmlNodeList nodes = node.ChildNodes;
                    foreach (XmlNode n in nodes)
                    {
                        if (n.Attributes["type"] != null)
                        {
                            VulnerabilityFound.Item item = new VulnerabilityFound.Item();
                            item.ID = n.Attributes["type"].InnerText;
                            item.Value = n.InnerText;
                            l.Add(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER Cenzic", string.Format("JobID:" + m_jobId + " Error in Helper_GetCVE : Exception = {0}", ex.Message));
                }
                return l;
            }

            public void UpdateJob(int JobId)
            {
                XORCISMEntities model = new XORCISMEntities();
                var Q = from o in model.JOB
                        where o.JobID == JobId
                        select o;
                JOB myJob = Q.FirstOrDefault();
                myJob.Status = XCommon.STATUS.FINISHED.ToString();
                myJob.DateEnd = DateTimeOffset.Now;
                model.SaveChanges();
            }
            /*
            private void LoadXmlResult(string URLbase, string URL)
            {
                string myUrl = URLbase + URL;
                Utils.Helper_Trace("XORCISM PROVIDER Cenzic - Loading XML Result", string.Format("URLBase = {0} & URLScan = {1}", URLbase, URL));
                System.Text.UTF8Encoding objUTF8 = new System.Text.UTF8Encoding();
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(myUrl);
                request.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.1; fr; rv:1.9.2.10) Gecko/20100914 Firefox/3.6.10";
                request.CookieContainer = new CookieContainer();
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate);
                request.Credentials = CredentialCache.DefaultCredentials;
                request.Method = "POST";
                string postData = "nexposeccusername=nxadmin&nexposeccpassword=NEXPOSE!!!&login_button=Login&loginRedir=" + URL;
                Utils.Helper_Trace("XORCISM PROVIDER Cenzic - Loading XML Result", string.Format("PostData = {0}", postData));
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
                Utils.Helper_Trace("XORCISM PROVIDER Cenzic - Loading XML Result", string.Format("Response from server = {0}", responseFromServer));
                m_data = responseFromServer;
            }
            */
        }

        internal class AcceptAllCertificatePolicy : ICertificatePolicy
        {
            public AcceptAllCertificatePolicy()
            {
            }

            public bool CheckValidationResult(ServicePoint sPoint,
               X509Certificate cert, WebRequest wRequest, int certProb)
            {
                // *** Always accept
                return true;
            }
        }
        /*
        class MyTestService : HailstormWebService.ScanServiceClient
        {   
            protected override WebRequest GetWebRequest(Uri uri)
            {
                HttpWebRequest webRequest = (HttpWebRequest)base.GetWebRequest(uri);
                //Setting KeepAlive to false
                webRequest.KeepAlive = false;
                return webRequest;
            }
        }
        */
    }
}
