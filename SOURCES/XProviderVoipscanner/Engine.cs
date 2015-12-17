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

using XCommon;
using XProviderCommon;
using XORCISMModel;

using CookComputing.XmlRpc;
using System.Runtime.Serialization.Formatters.Binary;

using FSM.DotNetSSH;

namespace XProviderVoipscanner
{
    /// <summary>
    /// Copyright (C) 2012-2015 Jerome Athias
    /// XORCISM Plugin for VoIPScanner
    /// All trademarks and registered trademarks are the property of their respective owners.
    /// This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation; either version 2 of the License, or (at your option) any later version.
    /// 
    /// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
    /// 
    /// You should have received a copy of the GNU General Public License along with this program; if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
    /// </summary>
    public class Engine : MarshalByRefObject, XProviderCommon.IVulnerabilityDetector
    {
        public struct Response
        {
            public string A;
            public string B;
        }
     //   string baseurl = ConfigurationManager.AppSettings["VOIPSCANNER_BASEURL"];
     //   [XmlRpcUrl(baseurl)]
        //HARDCODED
        [XmlRpcUrl("https://voipscanner.com/voipscanner/voipscannerxmlrpc/handle")]
        
        public interface IToto : IXmlRpcProxy
        {
            [XmlRpcMethod("scan")]
            string[] scan(string username, string key, XmlRpcStruct targhet);

            [XmlRpcMethod("getresultsxml")]
            string[] getresultsxml(string username, string key, XmlRpcStruct scanparams);
        }

        public class Tracer : XmlRpcLogger
        {
            protected override void OnRequest(object sender, XmlRpcRequestEventArgs e)
            {
                DumpStream(e.RequestStream);
            }

            protected override void OnResponse(object sender, XmlRpcResponseEventArgs e)
            {
                DumpStream(e.ResponseStream);
            }

            private void DumpStream(Stream stm)
            {
                stm.Position = 0;
                TextReader trdr = new StreamReader(stm);
                String s = trdr.ReadLine();
                while (s != null)
                {
                    // Trace.WriteLine(s);
                    Console.WriteLine(s);
                    s = trdr.ReadLine();
                }
            }
        }

        public Engine()
        {
            TextWriterTraceListener tw;
            tw = new TextWriterTraceListener("XProviderVoIpScanner.log");   //Hardcoded
            Trace.Listeners.Add(tw);
            Trace.AutoFlush = true;
            Trace.IndentSize = 4;
        }

        public void Run(string target, int jobID, string policy, string Strategy)
        {
            Utils.Helper_Trace("XORCISM PROVIDER VOIPSCANNER", "Entering Run()");

            Utils.Helper_Trace("XORCISM PROVIDER VOIPSCANNER", string.Format("Target = {0} , JobID = {1} , Policy = {2}",target,jobID,policy));

            Assembly a;
            a = Assembly.GetExecutingAssembly();

            Utils.Helper_Trace("XORCISM PROVIDER VOIPSCANNER", "Assembly location = " + a.Location);

            // ==============
            // Launch the job
            // ==============

            #region With VoIPScanner

            const string username = "xorusertest";  //TODO Hardcoded
            //string username = ConfigurationManager.AppSettings["VOIPSCANNER_USERNAME"];

            const string key = "1943e197-0zae-4bxc-xd18-12345";
            //string key = ConfigurationManager.AppSettings["VOIPSCANNER_KEY"];
            //     const string baseurl = "https://voipscanner.com/voipscanner/voipscannerxmlrpc/handle";

            Utils.Helper_Trace("XORCISM PROVIDER VOIPSCANNER", string.Format("UserName = {0} , Key = {1}",username,key));

            XmlRpcStruct Xtarget = new XmlRpcStruct();

            XORCISMEntities model;
            model = new XORCISMEntities();

            Dictionary<string, object> parameters;

            var q = from x in model.JOB
                    where x.JobID == jobID
                    select x.Parameters;
            try
            {
                byte[] buffer;
                buffer = q.First();

                Utils.Helper_Trace("XORCISM PROVIDER VOIPSCANNER", string.Format("Size of parameters = {0} bytes", buffer.Length));

                MemoryStream ms;
                ms = new MemoryStream(buffer);

                BinaryFormatter bf;
                bf = new BinaryFormatter();

                parameters = (Dictionary<string, object>)bf.Deserialize(ms);
            }
            catch (Exception ex)
            {
                Utils.Helper_Trace("XORCISM PROVIDER VOIPSCANNER", string.Format("Error while deserializing parameters : Exception =  {0}", ex.Message));
                return;
            }

            string Sip;
            Sip = (string)parameters["SIP"];

            string Extrange;
            Extrange = (string)parameters["EXTRANGE"];

            try
            {                
                Xtarget.Add("hostname", target);
                if(string.IsNullOrEmpty(Sip) == false)
                    Xtarget.Add("sipport", Sip);
                if(string.IsNullOrEmpty(Extrange) == false)
                    Xtarget.Add("extrange", Extrange);  
            }
            catch(Exception ex)
            {
                Utils.Helper_Trace("XORCISM PROVIDER VOIPSCANNER", string.Format("Exception = {0}",ex.Message));
                // Que faire ?
            }

            Utils.Helper_Trace("XORCISM PROVIDER VOIPSCANNER", string.Format("Target = {0}",target));
            
            IToto proxy = XmlRpcProxyGen.Create<IToto>();

            Utils.Helper_Trace("XORCISM PROVIDER VOIPSCANNER", string.Format("Proxy = {0}",proxy));

            Tracer tracer = new Tracer();
            tracer.Attach(proxy);

            Utils.Helper_Trace("XORCISM PROVIDER VOIPSCANNER", "Param Okay");

            string[] res=null;

            try
            {
                res = proxy.scan(username, key, Xtarget);
            }
            catch (XmlRpcFaultException fex)
            {
                Utils.Helper_Trace("XORCISM PROVIDER VOIPSCANNER", string.Format("Fault Response: {0} {1}", fex.FaultCode, fex.FaultString));              
            }

            string scanuid=string.Empty;

            if (res[0] == "Success")
            {
                Utils.Helper_Trace("XORCISM PROVIDER VOIPSCANNER", "Success 1");
                scanuid = res[1];
            }
            else
            {
                Utils.Helper_Trace("XORCISM PROVIDER VOIPSCANNER", string.Format("Fail 1: {0}", res[0]));                
            }

            // ===================
            // Wait for completion
            // ===================

            XmlRpcStruct scanparams = new XmlRpcStruct();
            scanparams.Add("scanuid", scanuid);
            scanparams.Add("pretty", true);
            scanparams.Add("documented", true);
            // target.Add("upperBound", 139);

            bool finished = false;
            string xml = string.Empty;
            Utils.Helper_Trace("XORCISM PROVIDER VOIPSCANNER", "Waiting 45 seconds...");

            Thread.Sleep(45000);    //Hardcoded

            while (finished == false)
            {
                try
                {
                    Utils.Helper_Trace("XORCISM PROVIDER VOIPSCANNER", "Trying to get results");
                    res = proxy.getresultsxml(username, key, scanparams);
                }
                catch (XmlRpcFaultException fex)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER VOIPSCANNER", string.Format("Fault Response: {0} {1}", fex.FaultCode, fex.FaultString));                    
                }

                if (res[0] == "Success")
                {
                    Utils.Helper_Trace("XORCISM PROVIDER VOIPSCANNER", "Success 2");
                    xml = res[1];
                    Utils.Helper_Trace("XORCISM PROVIDER VOIPSCANNER", "res="+xml);
                    finished = true;
                }
                else
                {
                    //Wait
                    Utils.Helper_Trace("XORCISM PROVIDER VOIPSCANNER", string.Format("Fail 2: {0}", res[0]));
                    //    return null;
                    Thread.Sleep(30000);
                }
            }

            Utils.Helper_Trace("XORCISM PROVIDER VOIPSCANNER", "Results successfully downloaded");

            XmlDocument doc;
            doc = new XmlDocument();

            try
            {
                //TODO XML Validation
                doc.LoadXml(xml);
            }
            catch (Exception ex)
            {
                Utils.Helper_Trace("XORCISM PROVIDER VOIPSCANNER", string.Format("LoadXmlException = {0}", ex.Message));
                // Que faire ?
            }
            #endregion


            #region Without VoIPScanner
            /*
            XmlDocument doc = new XmlDocument();
            doc.Load(@"c:\VoIPScanner.xml");
            */
            #endregion


            Utils.Helper_Trace("XORCISM PROVIDER VOIPSCANNER", "Parsing the results");

            VoIPScannerParser parser = new VoIPScannerParser(doc,jobID);
            parser.parse();

            Utils.Helper_Trace("XORCISM PROVIDER VOIPSCANNER", "Using svmap.py from sipvicious");
            string address = "111.222.333.444"; //TODO Hardcoded
            //string username = "root";
            string password = "toor";
            string prompt = "root";

            SshShell sshShell;
            sshShell = new SshShell(address, "root", password);
            sshShell.RemoveTerminalEmulationCharacters = true;

            Utils.Helper_Trace("XORCISM PROVIDER SIPVICIOUS", string.Format("JobID: {0} Connecting to SIPVICIOUS server at {1}", jobID, address));

            try
            {
                sshShell.Connect(22);
                //sshShell.Expect(prompt+"~#");
                sshShell.Expect(prompt);// + "~$");
            }
            catch (Exception ex)
            {
                Utils.Helper_Trace("XORCISM PROVIDER SIPVICIOUS", string.Format("JobID: {0} ConnectingERROR to SIPVICIOUS server at {1} : " + ex.Message + " " + ex.InnerException, jobID, address));
            }

            string cmd1 = "cd /home/root/tools/sipvicious/";    //Hardcoded
            sshShell.WriteLine(cmd1);
            Thread.Sleep(1000); //Hardcoded
            string stdout = sshShell.Expect(prompt);
            Utils.Helper_Trace("XORCISM PROVIDER SIPVICIOUS", string.Format("JobID: {0} START DUMP STDOUT01", jobID));
            Utils.Helper_Trace("XORCISM PROVIDER SIPVICIOUS", stdout);

            Thread.Sleep(1000);
            sshShell.WriteLine("./svmap.py "+target);   //Hardcoded
            Thread.Sleep(30000);    //Hardcoded
            stdout = sshShell.Expect(prompt);
            Utils.Helper_Trace("XORCISM PROVIDER SIPVICIOUS", string.Format("JobID: {0} START DUMP STDOUT02", jobID));
            Utils.Helper_Trace("XORCISM PROVIDER SIPVICIOUS", stdout);

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
                myInformation.Title = "";

                model.AddToINFORMATION(myInformation);
                model.SaveChanges();
            */

            Utils.Helper_Trace("XORCISM PROVIDER VOIPSCANNER", "Update job status to FINISHED");

            var j = from xJob in model.JOB
                    where xJob.JobID == jobID
                    select xJob;

            JOB J = j.FirstOrDefault();
            J.Status = XCommon.STATUS.FINISHED.ToString();

            model.SaveChanges();

            //FREE MEMORY
            parser = null;
            J = null;
            model.Dispose();
            

            Utils.Helper_Trace("XORCISM PROVIDER VOIPSCANNER", string.Format("Leaving Run()"));
        }

    }
    class VoIPScannerParser
    {
        private string m_target;
        private XmlDocument m_rawXml;
        private XORCISMEntities m_model;
        private int m_JobId;
        public VoIPScannerParser(XmlDocument rawXml,int jobId)
        {
            //TODO XML Validation
            try
            {
                m_target = rawXml.SelectNodes("/scan/results/hosts/host")[0].Attributes["ipaddr"].InnerText;    //Hardcoded
            }
            catch (Exception ex)
            {
                Utils.Helper_Trace("XORCISM PROVIDER VOIPSCANNER", string.Format("rawXml.SelectNodes error: {0} {1}", ex.Message, ex.InnerException));
                //return;
            }
            m_rawXml = rawXml;
            m_model = new XORCISMEntities();
            m_JobId = jobId;
        }
        private XmlNode HelperHasChild(XmlNode n, string ChildName)
        {
            foreach (XmlNode child in n.ChildNodes)
            {

                if (child.Name.ToUpper() == ChildName.ToUpper())
                    return child;
            }
            return null;
        }
        public void parse()
        {
            Assembly a;
            try
            {
                a = Assembly.GetExecutingAssembly();
                Utils.Helper_Trace("XORCISM PROVIDER VOIPSCANNER", "Assembly location = " + a.Location);

                Utils.Helper_Trace("XORCISM PROVIDER VOIPSCANNER", string.Format("Getting the vulnerabilities of " + m_target));
                Helper_GetVulnerabilities(m_rawXml, m_target);
                Utils.Helper_Trace("XORCISM PROVIDER VOIPSCANNER", string.Format("Leaving Parse()"));

                //FREE MEMORY
                a = null;
            }
            catch (Exception ex)
            {
                Utils.Helper_Trace("XORCISM PROVIDER VOIPSCANNER", string.Format("Parsing error: {0} {1}", ex.Message, ex.InnerException));
                //return;
            }
        }
        private void Helper_GetVulnerabilities(XmlDocument s, string ipadress)
        {
            //TODO
            //List<VULNERABILITYFOUND> list_vulnerabilyFound = new List<VULNERABILITYFOUND>() ;
            string query = "/scan/results/hosts/host";  //Hardcoded
            XmlNode host;
            try
            {
                host = s.SelectNodes(query)[0];

                if(HelperHasChild(host,"protocol")!=null)
                {
                   XmlNode protocol = HelperHasChild(host, "protocol");
                   if (HelperHasChild(protocol, "port") != null)
                   {
                       XmlNode port = HelperHasChild(protocol, "port");                 

                       VulnerabilityEndPoint VoIPScannerEndPoint = new VulnerabilityEndPoint();
                       VoIPScannerEndPoint.Port = Convert.ToInt32(port.Attributes["id"].InnerText);
                       VoIPScannerEndPoint.Protocol = protocol.Attributes["name"].InnerText.Trim().ToUpper();
                       VoIPScannerEndPoint.IpAdress = m_target;
                       Utils.Helper_Trace("XORCISM PROVIDER VOIPSCANNER",string.Format("Getting new vulnerability from the current endPoint(IpAdress={0},Port={1},Protocol={2})",VoIPScannerEndPoint.IpAdress,VoIPScannerEndPoint.Port.ToString(),VoIPScannerEndPoint.Protocol));

                       if (HelperHasChild(port, "pwd") != null)
                       {
                           XmlNode pwd = HelperHasChild(port, "pwd");
                           foreach (XmlNode extension in pwd.ChildNodes)
                           {
                               VulnerabilityFound detail = new VulnerabilityFound();
                               detail.Description = string.Format("The {0} SIP user has {1} as password", extension.Attributes["name"].InnerText, extension.Attributes["password"].InnerText);
                               detail.InnerXml = extension.OuterXml;
                          
                               VulnerabilityPersistor.Persist(detail, VoIPScannerEndPoint, m_JobId, "voipscanner", m_model);
                           }
                           //FREE MEMORY
                           pwd = null;
                       }
                   }
                }

                //FREE MEMORY
                host = null;
                //list_vulnerabilyFound = null;
            }
            catch (Exception ex)
            {
                Utils.Helper_Trace("XORCISM PROVIDER VOIPSCANNER", string.Format("Error SelectNodes {0}:{1} {2}", query, ex.Message, ex.InnerException));
                //return;
            }
        }
    }

    class _VulnerabilityDetail
    {
        public List<string> ListCve { get { return m_listCve; } set { m_listCve = value; } }
        public string InnerXml { get { return m_InnerXml; } set { m_InnerXml = value; } }
        public string Description { get { return m_Description; } set { m_Description = value; } }
        public string Consequence { get { return m_Consequence; } set { m_Consequence = value; } }
        public string Solution { get { return m_Solution; } set { m_Solution = value; } }
        public string Severity { get { return m_Severity; } set { m_Severity = value; } }

        private List<string> m_listCve;
        private string m_InnerXml;
        private string m_Description;
        private string m_Consequence;
        private string m_Solution;
        private string m_Severity;
        public _VulnerabilityDetail()
        {
            m_listCve = new List<string>();
            m_InnerXml = string.Empty;
            m_Description = string.Empty;
            m_Consequence = string.Empty;
            m_Solution = string.Empty;
            m_Severity = string.Empty;
        }
    }
    class _VulnerabilityEndPoint
    {
        #region Properties
        public string Protocol { get { return m_protocol; } set { m_protocol = value; } }
        public int Port { get { return m_port; } set { m_port = value; } }
        public string IpAdress { get { return m_IpAdress; } set { m_IpAdress = value; } }
        #endregion

        #region Private Members
        private int m_port;
        private string m_protocol;
        private string m_IpAdress;
        #endregion

        public _VulnerabilityEndPoint()
        {
            m_port = -1;
            m_protocol = string.Empty;
            m_IpAdress = string.Empty;
        }
    }
}
