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


namespace XProviderQualys
{
    public class Engine : MarshalByRefObject, XProviderCommon.IVulnerabilityDetector
    {
        /// <summary>
        /// Copyright (C) 2012-2015 Jerome Athias
        /// XORCISM Plugin for Qualys. Allows to launch scans on a remote scanner instance. Parse a report and import it in XORCISM database
        /// All trademarks and registered trademarks are the property of their respective owners.
        /// This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation; either version 2 of the License, or (at your option) any later version.
        /// 
        /// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
        /// 
        /// You should have received a copy of the GNU General Public License along with this program; if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
        /// </summary>
        
        static int inerror = 0;
        public Engine()
        {
            TextWriterTraceListener tw;
            tw = new TextWriterTraceListener("XProviderQualys.log");

            Trace.AutoFlush     = true;
            Trace.IndentSize    = 4;
            Trace.Listeners.Add(tw);
        }

        public void Run(string target, int jobID, string policy, string strategy)
        {
            //WARNING: OLD CODE, must be reviewed/revised - JA

            inerror = 0;

            #region Last Version
            //Utils.Helper_Trace("XORCISM PROVIDER QUALYS", "Entering Run()");
            //List<VULNERABILITYFOUND> Myresult = Helper_Run(target, policy);
            //Utils.Helper_Trace("XORCISM PROVIDER QUALYS", "Leaving Run()");
            #endregion
            Utils.Helper_Trace("XORCISM PROVIDER QUALYS", "JobID:"+jobID+" Entering Run()");
            Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("Target = {0} , JobID = {1} , Policy = {2}, Strategy = {3}", target, jobID, policy, strategy));

            Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("JobID:" + jobID + " Creating an instance of QualysParser"));

            QualysParser QualysParser = new QualysParser(target, jobID, policy, strategy);
            if (inerror == 0)
            {
                Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("JobID:" + jobID + " Parsing the data"));

                QualysParser.parse();

                Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("JobID:" + jobID + " End of data processing"));
            }

            if (inerror == 0)
            {
                Utils.Helper_Trace("XORCISM PROVIDER QUALYS", "JobID:" + jobID + " Leaving Run()");

                QualysParser.UpdateJob(jobID);

                Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("Updating job {0} status to FINISHED", jobID));
            }
            inerror = 0;
        }

        class QualysParser
        {
            private string m_strategy;
            private string m_target;
            private int    m_jobId;
            private string m_policy;
            private string m_data;

            public QualysParser(string target, int jobID, string policy, string strategy)
            {
                inerror = 0;

                #region ParsePolicy
                //InfosPolicy info = new InfosPolicy(policy);
                //info.Parse_NetworkCapabilities();
                #endregion ParsePolicy

                #region HackQualys
                /*
                string filename;
                filename = @"C:\QualysSampleResults.xml";
                
                XmlDocument doc = new XmlDocument();
                doc.Load(filename);

                Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("HackFile should be located at : " + filename));
                */
                #endregion HackQualys



                #region RealRequest

                #region CreateAsset

                if (target.Length - target.Replace(".", "").Length == 3 && target.Length - target.Replace(":", "").Length == 1)
                {
                    //target like: 198.10.11.12:8089
                    char[] splitters = new char[] { ':' };
                    string[] laCase = target.Split(splitters);
                    target = laCase[0];
                    Utils.Helper_Trace("XORCISM PROVIDER QUALYS", "target without port: " + target);
                }
                //Check if we have an IP address
                //string pattern = @"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\.
                //([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$";
                string pattern = @"^\d\d?\d?\.\d\d?\d?\.\d\d?\d?\.\d\d?\d?$";   //TODO: IPv6
                //create our Regular Expression object
                Regex check = new Regex(pattern);

                if (check.IsMatch(target.Trim(), 0))
                {
                    Utils.Helper_Trace("XORCISM PROVIDER QUALYS", "JobID:" + jobID + " target is an IP address");
                }
                else
                {
                    try
                    {
                        //It should be a domain name
                        Utils.Helper_Trace("XORCISM PROVIDER QUALYS", "JobID:" + jobID + " target: "+target+" is not an IP address");
                        // = target.Replace("http://", "");
                        //target = target.Replace("https://", "");
                        //target = target.Replace("/", "");
                        if (!target.Contains("://"))
                            target = "http://" + target;

                        target = new Uri(target).Host;
                        Utils.Helper_Trace("XORCISM PROVIDER QUALYS", "JobID:" + jobID + " targetmodified: " + target);
                        if (check.IsMatch(target.Trim(), 0))
                        {
                            Utils.Helper_Trace("XORCISM PROVIDER QUALYS", "JobID:" + jobID + " targetmodified is an IP address");
                        }
                        else
                        {
                            IPHostEntry ipEntry = Dns.GetHostEntry(target);
                            IPAddress[] addr = ipEntry.AddressList;
                            target = addr[0].ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("JobID:" + jobID + " Dns.GetHostEntry Exception = {0} / {1}", ex.Message, ex.InnerException == null ? "" : ex.InnerException.Message));

                        Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("Updating job {0} status to ERROR", m_jobId));
                        inerror = 1;
                        XORCISMEntities model;
                        model = new XORCISMEntities();

                        var Q = from j in model.JOB
                                where j.JobID == m_jobId
                                select j;

                        JOB myJob = Q.FirstOrDefault();
                        myJob.Status = XCommon.STATUS.ERROR.ToString();
                        myJob.DateEnd = DateTime.Now;
                        //image
                        System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
                        myJob.XmlResult = encoding.GetBytes(m_data);
                        model.SaveChanges();
                        //FREE MEMORY
                        model.Dispose();

                        XCommon.Utils.Helper_SendEmail("contact@hackenaton.org", "QUALYS ERROR", "QUALYS ERROR for job:" + m_jobId);
                        return;
                    }
                }

                if (inerror!=0)
                {
                    return;
                }

                string Url = "https://qualysapi.qualys.eu/msp/asset_ip.php?action=add&host_ips=" + target;

                Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("JobID:" + jobID + " Sending request for creating a new asset[{0}]", Url));

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(Url);
                request.Credentials = new NetworkCredential("xoruser", "ndYgo45Wsi");
                
                
                //request.Timeout = Timeout.Infinite;

                HttpWebResponse response;
                Stream strm=null;
                try
                {
                    response = (HttpWebResponse)request.GetResponse();                
                    strm = response.GetResponseStream();
                    //<RETURN status="FAILED" number="5000">Invalid value for 'host_ips' : http://81.192.102.8/. Invalid CIDR format</RETURN>
                    

                }
                catch (Exception ex)
                {
                    //The operation has timed out
                    Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("JobID:" + jobID + " GetResponseStream00 Exception = {0} / {1}", ex.Message, ex.InnerException == null ? "" : ex.InnerException.Message));
                    if(ex.Message.Contains("The operation has timed out"))
                    {
                        int cptretrycreateasset=0;
                        while(cptretrycreateasset < 10)
                        {
                            cptretrycreateasset++;
                            try
                            {
                                Thread.Sleep(10000);
                                Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("JobID:" + jobID + " Resending request for creating a new asset[{0}]", Url));

                                request = (HttpWebRequest)HttpWebRequest.Create(Url);
                                request.Credentials = new NetworkCredential("xoruser", "gygribr62o");  //TODO Hardcoded
                                
                                response = (HttpWebResponse)request.GetResponse();                
                                strm = response.GetResponseStream();
                                cptretrycreateasset=1000;   //if ok
                            }
                            catch (Exception exretry)
                            {
                                //The operation has timed out
                                Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("JobID:" + jobID + " GetResponseStream"+cptretrycreateasset+" Exception = {0} / {1}", exretry.Message, exretry.InnerException == null ? "" : exretry.InnerException.Message));
                            }
                        }
                    }
                }

                XmlDocument doc = new XmlDocument();
                try
                {
                    doc.Load(strm);
                }
                catch (Exception ex)
                {
                    //The operation has timed out
                    Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("JobID:" + jobID + " doc.LoadCreateAsset00 Exception = {0} / {1}", ex.Message, ex.InnerException == null ? "" : ex.InnerException.Message));
                    Utils.Helper_Trace("XORCISM PROVIDER QUALYS", "strm=" + strm);
                    //if (ex.Message.Contains("The operation has timed out"))
                    //{
                        int cptretryloaddoccreateasset = 0;
                        while (cptretryloaddoccreateasset < 10)
                        {
                            cptretryloaddoccreateasset++;
                            try
                            {
                                doc.Load(strm);
                                cptretryloaddoccreateasset = 1000;
                            }
                            catch (Exception exretry)
                            {
                                Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("JobID:" + jobID + " doc.LoadCreateAsset" + cptretryloaddoccreateasset + " Exception = {0} / {1}", exretry.Message, exretry.InnerException == null ? "" : exretry.InnerException.Message));
                                Utils.Helper_Trace("XORCISM PROVIDER QUALYS", "strm=" + strm);
                            }
                        }
                    //}
                }

                Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("JobID:" + jobID + " doc loaded: "+doc.OuterXml));
                /*
                <?xml version="1.0" encoding="UTF-8"?><!DOCTYPE GENERIC_RETURN SYSTEM "https://qualysapi.qualys.eu/generic_return.dtd"[]><!-- This report was generated with an evaluation version of QualysGuard //--><GENERIC_RETURN><API name="asset_ip.php" username="xoruser" at="2011-02-25T17:08:51Z" /><RETURN status="FAILED" number="5000">Invalid value for 'host_ips' : ja-psi.fr. Invalid IP or range.</RETURN></GENERIC_RETURN><!-- This report was generated with an evaluation version of QualysGuard //--><!-- CONFIDENTIAL AND PROPRIETARY INFORMATION. Qualys provides the QualysGuard Service "As Is," without any warranty of any kind. Qualys makes no warranty that the information contained in this report is complete or error-free. Copyright 2011, Qualys, Inc. //-->
                */
                //<RETURN status="FAILED" number="5000">Invalid value for 'host_ips' : http://81.192.102.8/. Invalid CIDR format</RETURN>
                if(doc.OuterXml.Contains("FAILED"))
                {
                    Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("Updating job {0} status to ERROR", m_jobId));
                    XORCISMEntities model;
                    model = new XORCISMEntities();

                    var Q = from j in model.JOB
                            where j.JobID == m_jobId
                            select j;

                    JOB myJob = Q.FirstOrDefault();
                    myJob.Status = XCommon.STATUS.ERROR.ToString();
                    myJob.DateEnd = DateTime.Now;
                    //image
                    System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
                    myJob.XmlResult = encoding.GetBytes(m_data);
                    model.SaveChanges();
                    //FREE MEMORY
                    model.Dispose();
                    
                    XCommon.Utils.Helper_SendEmail("contact@hackenaton.org", "QUALYS ERROR", "QUALYS ERROR for job:" + m_jobId);
                    return;
                }
                
                #endregion CreateAsset

                #region Scan
                
                Url = "https://qualysapi.qualys.eu/msp/scan.php";   //HARDCODED
                Url += ("?ip=" + target);

                Url += GetOptionProfile(jobID, policy, strategy);

                Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("JobID:" + jobID + " Sending request [{0}]", Url));
                
                try
                {
                    request = (HttpWebRequest)HttpWebRequest.Create(Url);
                    request.Credentials = new NetworkCredential("jerome", "changeme");  //TODO Hardcoded
                    //request.Timeout = Timeout.Infinite;

                    response = (HttpWebResponse)request.GetResponse();
                    strm = response.GetResponseStream();
                }
                catch (Exception ex)
                {
                    //The operation has timed out
                    Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("JobID:" + jobID + " GetResponseStream05 Exception = {0} / {1}", ex.Message, ex.InnerException == null ? "" : ex.InnerException.Message));
                }

                Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("JobID:" + jobID + " Response received"));

                doc = new XmlDocument();
                int timeoutload = 0;
                try
                {
                    doc.Load(strm);
                }
                catch (Exception ex)
                {
                    //The operation has timed out
                    Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("JobID:" + jobID + " doc.Load Exception = {0} / {1}", ex.Message, ex.InnerException == null ? "" : ex.InnerException.Message));
                    Utils.Helper_Trace("XORCISM PROVIDER QUALYS", "JobID:" + jobID + " strm=" + strm);
                    timeoutload = 1;
                }
                if (timeoutload > 0)
                {
                    while (timeoutload < 10)
                    {
                        try
                        {
                            doc.Load(strm);
                            timeoutload = 200;  //exit
                        }
                        catch (Exception ex)
                        {
                            //The operation has timed out
                            Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("JobID:" + jobID + " doc.Load Exception " + timeoutload + "= {0} / {1}", ex.Message, ex.InnerException == null ? "" : ex.InnerException.Message));
                            Utils.Helper_Trace("XORCISM PROVIDER QUALYS", "strm=" + strm);  //HARDCODED
                            timeoutload++;
                        }
                    }
                }

                                
                #endregion Scan

                #endregion RealRequest

                m_data      = doc.InnerXml;
                m_target    = target;
                m_jobId     = jobID;
                m_policy    = policy;
                m_strategy  = strategy;
            }

            public void parse() //ParseQualysReport()
            {
                Assembly a;
                a = Assembly.GetExecutingAssembly();

                Utils.Helper_Trace("XORCISM PROVIDER QUALYS", "Assembly location = " + a.Location);

                // ============================================
                // Parse the XML Document and feed the database
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
                    Utils.Helper_Trace("XORCISM PROVIDER QUALYS", "JobID:" + m_jobId + " Loading the XML document for parsing");

                    doc.LoadXml(m_data);
                    Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("JobID:" + m_jobId + " m_data = {0}", m_data));
                    
                    if (m_data.Contains("ERROR number"))
                    {
                        Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("Updating job {0} status to ERROR", m_jobId));
                        var Q = from j in model.JOB
                                where j.JobID == m_jobId
                                select j;

                        JOB myJob = Q.FirstOrDefault();
                        myJob.Status = XCommon.STATUS.ERROR.ToString();
                        if (m_data.Contains("No host alive"))   //HARDCODED
                        {
                            myJob.ErrorReason = "No host alive";
                        }
                        myJob.DateEnd = DateTime.Now;
                        //image
                        System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
                        myJob.XmlResult = encoding.GetBytes(m_data);
                        model.SaveChanges();
                        //FREE MEMORY
                        model.Dispose();

                        XCommon.Utils.Helper_SendEmail("contact@hackenaton.org", "QUALYS ERROR", "QUALYS ERROR for job:" + m_jobId+" "+myJob.ErrorReason);
                        inerror = 1;
                        return;
                    }

                }
                catch (Exception ex)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("JobID:" + m_jobId + " ExceptionParse = {0} / {1}", ex.Message, ex.InnerException == null ? "" : ex.InnerException.Message));

                    Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("Updating job {0} status to ERROR", m_jobId));
                    var Q = from j in model.JOB
                            where j.JobID == m_jobId
                            select j;

                    JOB myJob = Q.FirstOrDefault();
                    myJob.Status = XCommon.STATUS.ERROR.ToString();
                    myJob.DateEnd = DateTime.Now;
                    //image
                    System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
                    myJob.XmlResult = encoding.GetBytes(m_data);
                    model.SaveChanges();
                    //FREE MEMORY
                    model.Dispose();
                    
                    XCommon.Utils.Helper_SendEmail("contact@hackenaton.org", "QUALYS ERROR", "QUALYS ERROR for job:" + m_jobId);
                    inerror = 1;
                    return;
                }
                if (inerror == 1)
                {
                    return;
                }

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

                Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("JobID:" + m_jobId + " ASSET ID => {0}", myAsset.AssetID));

                //*********************************************************************************************************************************************
                //  OS
                string query = "/SCAN/IP";  //HARDCODED

                XmlNodeList report;
                report = null;
                try
                {
                    report = doc.SelectNodes(query);
                }
                catch (Exception ex)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("JobID:" + m_jobId + " Error SelectNodes({0}) : Exception = {1}", query, ex.Message));
                }
                Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("JobID:" + m_jobId + " Found {0} IP to parse", report.Count));
                foreach (XmlNode reportHost in report)
                {
                    string strTempo = "";
                    if (reportHost.Attributes["value"] != null)
                    {
                        strTempo = reportHost.Attributes["value"].InnerText.ToUpper();
                        Utils.Helper_Trace("XORCISM PROVIDER QUALYS", "IP:" + strTempo);
                    }
                    if (reportHost.Attributes["name"] != null)
                    {
                        strTempo = reportHost.Attributes["value"].InnerText.ToUpper();
                        Utils.Helper_Trace("XORCISM PROVIDER QUALYS", "name:" + strTempo);
                    }
                }

                query = "/SCAN/IP/OS";  //HARDCODED

                //XmlNodeList report;
                report = null;
                try
                {
                    report = doc.SelectNodes(query);
                }
                catch (Exception ex)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("JobID:" + m_jobId + " Error SelectNodes({0}) : Exception = {1}", query, ex.Message));
                }
                Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("Found {0} OS to parse", report.Count));
                foreach (XmlNode reportHost in report)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER QUALYS", "OS:" + reportHost.InnerText);
                }

                query = "/SCAN/IP/NETBIOS_HOSTNAME";    //HARDCODED

                //XmlNodeList report;
                report = null;
                try
                {
                    report = doc.SelectNodes(query);
                }
                catch (Exception ex)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("JobID:" + m_jobId + " Error SelectNodes({0}) : Exception = {1}", query, ex.Message));
                }
                Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("Found {0} NETBIOS_HOSTNAME to parse", report.Count));
                foreach (XmlNode reportHost in report)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER QUALYS", "NETBIOS_HOSTNAME:" + reportHost.InnerText);
                }

                //*********************************************************************************************************************************************
                //  INFOS
                query = "/SCAN/IP/INFOS/CAT";   //HARDCODED

                //XmlNodeList report;
                report = null;
                try
                {
                    report = doc.SelectNodes(query);
                }
                catch (Exception ex)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("JobID:" + m_jobId + " Error SelectNodes({0}) : Exception = {1}", query, ex.Message));
                }
                Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("Found {0} INFOS to parse", report.Count));
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



                    Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("JobID:" + m_jobId + " Protocol = [{0}]   Port = [{1}]", protocol, port));

                    int theEndPointID = 0;
                    //Check if the endpoint already exists
                    var EP = from Epoint in model.ENDPOINT
                             where Epoint.AssetID == myAsset.AssetID && Epoint.SessionID == mySessionID
                             select Epoint;
                    foreach (ENDPOINT E in EP.ToList())
                    {
                        //TODO  ProtocolID  PortID
                        if (E.ProtocolName == protocol && E.PortNumber == port)
                        {
                            theEndPointID = E.EndPointID;
                            break;
                        }
                    }
                    if (theEndPointID == 0)
                    {
                        Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("JobID:" + m_jobId + " Could not find the endpoint"));
                    //    Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("Creating a new one (port={0}, proto={1}, service={2})", port, protocol, service));

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
                                Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("JobID:" + m_jobId + " Error PORTreferential Exception = {0}", ex.Message + " " + ex.InnerException));
                            }                
                     //   }

                            Utils.Helper_Trace("XORCISM PROVIDER QUALYS", "JobID:" + m_jobId + " Adding endpoint:" + protocol + "/" + port + " (" + service + ")");

                        ENDPOINT newEndPoint = new ENDPOINT();
                        newEndPoint.AssetID = myAsset.AssetID;
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
                        Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("JobID:" + m_jobId + " Endpoint found: {0}", theEndPointID));
                    }


                    foreach (XmlNode n in reportHost.ChildNodes)
                    {
                        //TODO
                        

                    }
                }



                //*********************************************************************************************************************************************
                //  SERVICES

                query = "/SCAN/IP/SERVICES/CAT";    //HARDCODED

                //XmlNodeList report;
                report = null;
                try
                {
                    report = doc.SelectNodes(query);
                }
                catch (Exception ex)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("JobID:" + m_jobId + " Error SelectNodes({0}) : Exception = {1}", query, ex.Message));
                }

                Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("JobID:" + m_jobId + " Found {0} SERVICES to parse", report.Count));
                                
                
                foreach (XmlNode reportHost in report)
                {
                    //Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("Handling host with IP {0}", m_target));
                    //Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("reportHost: {0}", reportHost.InnerText));

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

                    Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("JobID:" + m_jobId + " Protocol = [{0}]   Port = [{1}]", protocol, port));
                    
                    int theEndPointID = 0;
                    //Check if the endpoint already exists
                    var EP = from Epoint in model.ENDPOINT
                             where Epoint.AssetID == myAsset.AssetID && Epoint.SessionID == mySessionID
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
                        Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("JobID:" + m_jobId + " Could not find the endpoint"));

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
                                    Utils.Helper_Trace("XORCISM PROVIDER QUALYS", "JobID:" + m_jobId + " The service was:" + service + " for:" + protocol + "/" + port + " replacing by referential:" + thePort.DefaultServiceName.Trim());
                                    service = thePort.DefaultServiceName.Trim();
                                }
                                else
                                {
                                    Utils.Helper_Trace("XORCISM PROVIDER QUALYS", "JobID:" + m_jobId + " No service found in PORT referential for port:" + protocol + "/" + port + " service:" + service);
                                }
                            }
                            catch (Exception ex)
                            {
                                Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("JobID:" + m_jobId + " Error PORTreferential Exception = {0}", ex.Message));
                            }
                        //}
                        //else
                        //{
                            //TODO
                            //Verify that the PORT/PROTOCOL/SERVICE is the same as the referential

                        //}

                            Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("JobID:" + m_jobId + " Creating a new one (port={0}, proto={1}, service={2})", port, protocol, service));

                        ENDPOINT newEndPoint = new ENDPOINT();
                        newEndPoint.AssetID = myAsset.AssetID;
                        newEndPoint.ProtocolName = protocol;
                        newEndPoint.PortNumber = port;
                        newEndPoint.Service = service;
                        newEndPoint.SessionID = mySessionID;

                        model.ENDPOINT.Add(newEndPoint);
                        model.SaveChanges();
                    }
                    else
                    {
                        Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("JobID:" + m_jobId + " Endpoint found: {0}", theEndPointID));
                    }

                    foreach (XmlNode n in reportHost.ChildNodes)
                    {
                        XmlNodeList Childs = n.ChildNodes;
                        List<int> myExploits = new List<int>();
                        //Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("Coucou 1"));


                        //TODO
                        
                    }

                }

                //*********************************************************************************************************************************************
                //  VULNS

                query = "/SCAN/IP/VULNS/CAT";   //HARDCODED

                //XmlNodeList report;
                report = null;
                try
                {
                    report = doc.SelectNodes(query);
                }
                catch (Exception ex)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("JobID:" + m_jobId + " Error SelectNodes({0}) : Exception = {1}", query, ex.Message));
                }

                Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("JobID:" + m_jobId + " Found {0} VULNS to parse", report.Count));
                service = "";
                foreach (XmlNode reportHost in report)
                {
                    //Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("Handling host with IP {0}", m_target));
                    Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("JobID:" + m_jobId + " reportHost: {0}", reportHost.InnerText));

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
                            Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("Error PORTreferential Exception = {0}", ex.Message));
                        }
//                    }

                        Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("JobID:" + m_jobId + " Protocol = [{0}]   Port = [{1}]    Service = [{2}]", protocol, port, service));

                    VulnerabilityEndPoint vulnerabilityEndPoint = new VulnerabilityEndPoint();
                    vulnerabilityEndPoint.IpAdress  = m_target;
                    vulnerabilityEndPoint.Protocol  = protocol;
                    vulnerabilityEndPoint.Port      = port;
                    vulnerabilityEndPoint.Service = service;

                    foreach (XmlNode n in reportHost.ChildNodes)
                    {
                        XmlNodeList Childs = n.ChildNodes;
                        List<int> myExploits=new List<int>();
                        //Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("Coucou 1"));

                        VulnerabilityFound vulnerabilityFound = new VulnerabilityFound();
                        vulnerabilityFound.ListItem     = Helper_GetCVE(n);
                        vulnerabilityFound.ListReference = Helper_GetREFERENCE(n);  //TODO: Helper_GetCVE and Helper_GetREFERENCE could be mixed for only 1 parsing
                        vulnerabilityFound.InnerXml     = n.OuterXml;
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
                            Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("JobID:" + m_jobId + " Error parsing CVSS_BASE : Exception = {0}", ex.Message));
                            Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("CVSS_BASE =", HelperGetChildInnerText(n, "CVSS_BASE")));
                        }
                        try
                        {
                            string strCorrelation = HelperGetChildInnerText(n, "CORRELATION");
                            if (strCorrelation != "")
                            {
                                Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("CORRELATION=", strCorrelation));
                                
                                Regex RegexCVE =new Regex("CVE-[0-9][0-9][0-9][0-9]-[0-9][0-9][0-9][0-9]"); //TODO: UPDATE WITH NEW FORMAT
                                MatchCollection myCVES = RegexCVE.Matches(strCorrelation);
                                
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
                                        Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("Index={0}, Sploit={1}", capture.Index, sploitlocation));
                                        string sploitrefid = sploitlocation.Replace("http://www.exploit-db.com/exploits/", "");
                                        sploitrefid = sploitrefid.Replace("http://exploit-db.com/exploits/", "");
                                        
                                        Utils.Helper_Trace("XORCISM PROVIDER QUALYS", "sploitrefid=" + sploitrefid);
                                            string myCVE = myCVES[cpt].Value;
                                        cpt++;
                                        Utils.Helper_Trace("XORCISM PROVIDER QUALYS", "CVE=" + myCVE);
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
                                            Utils.Helper_Trace("XORCISM PROVIDER QUALYS", "Exploit already exists in the database");
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
                                                Utils.Helper_Trace("XORCISM PROVIDER QUALYS", "ExploitFormatException: " + ex);
                                            }
                                            ExploitID = sploit.ExploitID;
                                            Utils.Helper_Trace("XORCISM PROVIDER QUALYS", "New exploit added in the database");
                                        }
                                        myExploits.Add(ExploitID);
                                        string CVEcorrect = RegexCVE.Match(myCVE).ToString();
                                        if (CVEcorrect != "")
                                        {
                                            //Check if EXPLOITFORVULNERABILITY (CVE) exists in the database
                                            int myCVEID=0;
                                            XVULNERABILITYModel.VULNERABILITY vs1;

                                            vs1 = vuln_model.VULNERABILITY.FirstOrDefault(o => o.VULReferential == "cve" && o.VULReferentialID == myCVE);
                                            if (vs1 == null)
                                            {
                                                //The CVE does not exist in the database
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
                                                    Utils.Helper_Trace("XORCISM PROVIDER QUALYS", "Added EXPLOITFORVULNERABILITY");
                                                }
                                                catch (FormatException ex)
                                                {
                                                    Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("AddToEXPLOITFORVULNERABILITY" + ex));
                                                }
                                            }
                                        }
                                    }
                                }
                            }


                        }
                        catch (Exception ex)
                        {
                            Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("Error parsing Correlation : Exception = {0}", ex.Message));
                        }
                        
                        #region JEROME
                        PatchUpgrade = false;
                        title = HelperGetChildInnerText(n, "TITLE");
                        MSPatch = "";
                        Regex objNaturalPattern = new Regex("MS[0-9][0-9]-[0-9][0-9][0-9]");
                        MSPatch = objNaturalPattern.Match(title).ToString();
                        if (MSPatch != "")
                        {
                            Utils.Helper_Trace("XORCISM PROVIDER QUALYS", "MSPatch=" + MSPatch);
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
                            //    Utils.Helper_Trace("XORCISM PROVIDER QUALYS", title);
                            //}
                        }

                        if (PatchUpgrade)
                        {
                            Utils.Helper_Trace("XORCISM PROVIDER QUALYS", "PatchUpgrade");
                        }
                        else
                        {
                            //    Utils.Helper_Trace("XORCISM PROVIDER QUALYS", "Solution: " + Solution);
                        }
                        vulnerabilityFound.PatchUpgrade = PatchUpgrade;
                        vulnerabilityFound.MSPatch = MSPatch;
                        #endregion

                        
                        // ===========
                        // Persistance
                        // ===========

                        Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("Persistance [{0}] [{1}] [{2}] [{3}]", protocol, port, service, Helper_ListCVEToString(vulnerabilityFound.ListItem)));
                        int VulnID = VulnerabilityPersistor.Persist(vulnerabilityFound, vulnerabilityEndPoint, m_jobId, "qualys", model);


                        //Check if EXPLOITFORVULNERABILITY exists in the database
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
                                    Utils.Helper_Trace("XORCISM PROVIDER QUALYS", "Added EXPLOITFORVULNERABILITY");
                                }
                                catch (FormatException ex)
                                {
                                    Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("AddToEXPLOITFORVULNERABILITY: ", ex));
                                }
                            }
                        }

                        var xTmpJob = from j in model.JOB
                                   where j.JobID == m_jobId
                                   select j;

                        JOB xJob = xTmpJob.FirstOrDefault();
                        if (xJob.SESSION.ServiceCategoryID == 4) //HARCODED Compliance
                        {
                            //TODO
                            
                        }
                    }
                }

                //TODO
                // VulnerabilityPersistor.UpdateVulnerabilityJob(list_vulnerabilyFound,m_JobId,m_model);
            }

            private List<int> GetCompliance(string xml, string ValTitle)
            {
                List<int> myIds = new List<int>();
                string tmp = "<DetailVuln>";
                tmp += xml;
                tmp += "</DetailVuln>";

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(tmp);

                XORCISMEntities model = new XORCISMEntities();

                #region PCI
                XmlNodeList list = doc.SelectNodes("/DetailVuln/PCI_FLAG"); //HARCODED
                foreach (XmlNode n in list)
                {
                    if (n.InnerText == "1")
                    {
                        //TODO
                        
                    }
                }
                #endregion
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
                            Utils.Helper_Trace("XORCISM PROVIDER QUALYS", "CORRELATION FOUND: " + child.InnerText);
                            foreach (XmlNode childcorrel in child.ChildNodes)
                            {
                                Utils.Helper_Trace("XORCISM PROVIDER QUALYS", "CORRELATION CHILD: " + childcorrel.Name);
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
                        Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("TEST CVE: {0}", myCVEID));
                        item.ID = "cve";
                        l.Add(item);
                }
                return l;
            }

            private List<VulnerabilityFound.Reference> Helper_GetREFERENCE(XmlNode node)
            {
                
                List<VulnerabilityFound.Reference> l = new List<VulnerabilityFound.Reference>();
                foreach (XmlNode child in node.ChildNodes)
                {
                    if (child.Name.ToUpper() == "VENDOR_REFERENCE_LIST")
                    {
                        Utils.Helper_Trace("XORCISM PROVIDER QUALYS", "VENDOR_REFERENCE_LIST");
                        foreach (XmlNode noderef in child.ChildNodes)
                        {
                            VulnerabilityFound.Reference refvuln = new VulnerabilityFound.Reference();
                            string refurl = HelperGetChildInnerText(noderef, "URL");
                            //HARDCODED
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
                            Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("TEST REFERENCE: {0}", refurl));
                        }
                    }
                    else
                    {
                        if (child.Name.ToUpper() == "BUGTRAQ_ID_LIST")
                        {
                            Utils.Helper_Trace("XORCISM PROVIDER QUALYS", "BUGTRAQ_ID_LIST");
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
                                Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("TEST REFERENCEBID: {0}", refurl));
                            }
                        }
                        //TODO: Complete with code from Import_all

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
                myJob.DateEnd = DateTime.Now;
                
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
                        Profile = "&option=";   //HARDCODED
                        Profile += partUrl;
                    }
                    
                }
                catch (Exception ex)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER QUALYS", string.Format("GetOptionProfile Exception = {0} / {1}", ex.Message, ex.InnerException == null ? "" : ex.InnerException.Message));
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
                // *** Always accept
                return true;
            }
        }

        #region Last Version
        //private List<VULNERABILITYFOUND> Helper_Run(string ipadress, string policy)
        private List<FINDING> Helper_Run(string ipadress, string policy)
        {
            
            return null;
        }

        
        private int SearchForQualysID(string qualysID)
        {
            
            return -1;
        }
        #endregion
    }
}
