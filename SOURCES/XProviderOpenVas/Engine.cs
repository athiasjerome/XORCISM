using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Configuration;
using System.Threading;
using System.Text.RegularExpressions;

using XORCISMModel;
using XCommon;
using XProviderCommon;

//using FSM.DotNetSSH;

namespace XProviderOpenVas
{
    /// <summary>
    /// Copyright (C) 2012-2015 Jerome Athias
    /// OpenVAS plugin for XORCISM
    /// All trademarks and registered trademarks are the property of their respective owners.
    /// This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation; either version 2 of the License, or (at your option) any later version.
    /// 
    /// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
    /// 
    /// You should have received a copy of the GNU General Public License along with this program; if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
    /// </summary>
    public class Engine : MarshalByRefObject, XProviderCommon.IVulnerabilityDetector
    {        
        public Engine()
        {
            TextWriterTraceListener tw;
            tw = new TextWriterTraceListener("XProviderOpenVas.log");   //Hardcoded
            Trace.Listeners.Add(tw);
            Trace.AutoFlush = true;
            Trace.IndentSize = 4;           
        }

        public void Run(string target, int jobID, string policy, string strategy)
        {
            Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", "Entering Run()");
            Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("Target = {0} , JobID = {1} , Policy = {2}, Strategy = {3}", target, jobID, policy, strategy));

            OpenVasParser parser;
            parser = new OpenVasParser(target, jobID, policy,strategy);

            string status = XCommon.STATUS.FINISHED.ToString();
            if (parser.Parse() == false)
                status = XCommon.STATUS.ERROR.ToString();

            // =================================================
            // Change the status of the job to FINISHED or ERROR
            // =================================================

            Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("Updating job {0} status to FINISHED", jobID));

            XORCISMEntities model = new XORCISMEntities();
            var xJob = from j in model.JOB
                       where j.JobID == jobID
                       select j;

            JOB xJ = xJob.FirstOrDefault();
            xJ.Status   = status;
            xJ.DateEnd  = DateTime.Now;
            model.SaveChanges();

            Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", "Leaving Run()");
        }
    }

    class OpenVasParser
    {
        private string          m_target;
        private XORCISMEntities m_model;
        private int             m_jobId;
        private string          m_policy;
        private string          m_strategy;

        public OpenVasParser(string target, int jobId, string policy, string strategy)
        {
            m_jobId = jobId; 
            m_target=target;
            m_model= new XORCISMEntities();
            m_policy = policy;
            m_strategy = strategy;
        }

        public bool Parse()
        {
            Assembly a;
            a = Assembly.GetExecutingAssembly();

            Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", "Assembly location = " + a.Location);

            XmlDocument doc = new XmlDocument();

            #region WITH OPENVAS (VERSION 1)
            /*
            string folder;
            folder = string.Format("result_{0}_{1}", DateTime.Now.Ticks, this.GetHashCode());
            Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", "FolderName is ="+folder);
            //_SaintSettings settings = _SaintSettings.getInstance();

            //Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", "Connecting to remote server by SSH" + settings.SaintIP);

           // SshStream sshStream = new SshStream(settings.SaintIP, settings.SaintLogin, settings.SaintPassword);
            //string saintIP = ConfigurationManager.AppSettings["SAINT_IP"];
            //string saintLogin = ConfigurationManager.AppSettings["SAINT_LOGIN"];
            //string saintPassword = ConfigurationManager.AppSettings["SAINT_PASSWORD"];

           // SshStream sshStream = new SshStream(saintIP, saintLogin, saintPassword);

            // Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("saintIP={0},saintLogin={1},saintPassword={2}", saintIP, saintLogin, saintPassword));

            SshStream sshStream = new SshStream("1.2.3.4", "saint", "OPENVAS!!!");

            //SshExec exec;
            //exec = new SshExec("1.2.3.4", "saint");
            //exec.Password = "OPENVAS!!!";

            //exec.Connect();

            //Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", "Successfully connected to remote server");

            string cmd1;
            string cmd2;
            //string output;

            //cmd = string.Format("cd /usr/share/saint");
            //output = exec.RunCommand(cmd);

            //cmd = string.Format("ls -al");
            //output = exec.RunCommand(cmd);

            
            
            // cmd = string.Format("sudo ./saint -i -a 3 -Q -d {0} -L smb:root%7MyMP8qt {1}", folder, m_target);
            cmd1 = string.Format("sudo ./mysaint.sh {0} {1}", folder, m_target);
            cmd2 = string.Format("sudo ./mysaint2.sh {0} ", folder);
            //string  sdout=string.Empty;
            //string serror=string.Empty;
           
            //output = exec.RunCommand(cmd,ref sdout,ref serror).ToString();

            //cmd = string.Format("sudo ./mysaint2.sh {0}", folder);
            //cmd = string.Format("sudo ./bin/saintwriter -c full.cf -d {0} -f 7", folder);
            //output = exec.RunCommand(cmd, ref sdout, ref serror).ToString();


            StreamWriter sw = new StreamWriter(sshStream);
            StreamReader sr = new StreamReader(sshStream);
            sw.AutoFlush = true;

            //sw.WriteLine("cd /usr/share/saint");

            //sw.WriteLine(string.Format("sudo ./saint -i -a 3 -Q -d {0} -L smb:root%7MyMP8qt {1}", folder, m_target));
            sw.WriteLine(cmd1);
            sw.Flush();
            
            // Thread.Sleep(10000);
            // sw.WriteLine("OPENVAS!!!");
            // Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", "Password = OPENVAS!!!");
            // Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", "Executing OPENVAS Query:" + sw.ToString());

            sw.WriteLine(cmd2);
            sw.Flush();

            //Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", "Sleep 10000 : BEGIN");
            ////Thread.Sleep(10000);
            //Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", "Sleep 10000 : END");
            //sw.WriteLine(string.Format("sudo ./bin/saintwriter -c full.cf -d {0} -f 7", folder));

            bool finished = false;

            string s = string.Empty;
            string VulnerabilityXml = string.Empty;

            try
            {
                char[] buffer = new char[2048];//1024

                StringBuilder sb;
                sb = new StringBuilder();

                while (finished == false)
                {

                    int n;
                    n = sr.Read(buffer, 0, 1024);

                    sb.Append(buffer, 0, n);

                    StringBuilder sb2;
                    sb2 = new StringBuilder();
                    sb2.Append(buffer, 0, n);
                    Debug.WriteLine(sb2.ToString());
                    
                    if (sb.ToString().Contains("udp_scan: no response from" + m_target + "; host is offline or UDP is filtered"))
                    {
                        throw new Exception("udp_scan: no response from" + m_target + "; host is offline or UDP is filtered");
                        
                    }

                    if (sb.ToString().Contains("</report>"))
                    {
                        Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", "XmlReport Found");
                        finished = true;
                        int startIndex = sb.ToString().IndexOf("<report>");
                        int endIndex = sb.ToString().IndexOf("</report>");
                        VulnerabilityXml = sb.ToString().Substring(startIndex, endIndex - startIndex);
                        VulnerabilityXml += "</report>";
                    }

                }

                Debug.WriteLine(sb.ToString());
            }
            catch (Exception ex)
            {
                Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", "Exception = " + ex.Message);
            }

            Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", "Parsing Saint Result into Xml");

            doc.LoadXml(VulnerabilityXml);
            */
            #endregion

            #region WITH OPENVAS (VERSION 2)

            long ticks;
            ticks = DateTime.Now.Ticks;

            string inputfile;
            inputfile = string.Format("{0}_{1}_input", ticks, this.GetHashCode());

            string outputfile;
            outputfile = string.Format("{0}_{1}_output", ticks, this.GetHashCode());

            Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("JobID:" + m_jobId + " Input file  = [{0}]", inputfile));
            Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("JobID:" + m_jobId + " Output file = [{0}]", outputfile));

            int port;
            string address, username, password;
            /*
            port = 22222;
            address = "1.2.3.4";
            username = "openvas";
            password = "vabiargash";
            */

            port = 22;
            address = "111.222.333.444";
            username = "root";
            password = "toor";

            Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("Connecting to OPENVAS server at {0} on port {1}", address, port));

            string prompt;
            //prompt = "openvas@linux-jnx2:~>";
            prompt = "root@xmachine:~#";

            SshShell sshShell;
            sshShell = new SshShell(address, username, password);
            sshShell.RemoveTerminalEmulationCharacters = true;

            sshShell.Connect(port);

            sshShell.Expect(prompt);

            Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("Successfully connected to OPENVAS server"));

            //string output;
            string stdout = "";
            //string stderr = "";

            // =========
            // Command 0 (test)
            // =========

            string cmd;
            cmd = string.Format("cd /home");    //Hardcoded
            sshShell.WriteLine(cmd);
            prompt = "root@xmachine:/home#";
            stdout = sshShell.Expect(prompt);            

            //Update openvas plugins:
            //openvas-nvt-sync
            cmd = string.Format("openvas-nvt-sync");    //Hardcoded

            Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("JobID:" + m_jobId + " Executing command [{0}]", cmd));

            sshShell.WriteLine(cmd);
            stdout = sshShell.Expect(prompt);

            Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("START DUMP STDOUT"));
            Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", stdout);
            Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("END DUMP STDOUT"));           

            // =========
            // Command 1 : Generate the input file
            // =========

            cmd = string.Format("echo \"{0}\" > {1}", m_target, inputfile);

            Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("Executing command [{0}]", cmd));

            sshShell.WriteLine(cmd);
            stdout = sshShell.Expect(prompt);

            Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("START DUMP STDOUT"));
            Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", stdout);
            Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("END DUMP STDOUT"));

            // =========
            // Command 2 : Scan
            // =========

            cmd = string.Format("openvas-client --output-type=xml -x --batch-mode=localhost 9391 openvas tcurstantv {0} {1}", inputfile, outputfile);   //Hardcoded
            //cmd = string.Format("RunOpenVAS.sh {0} {1}", inputfile, outputfile);

            Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("JobID:" + m_jobId + " Executing command [{0}]", cmd));

            sshShell.WriteLine(cmd);
            stdout = sshShell.Expect(prompt);

            Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("START DUMP STDOUT"));
            Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", stdout);
            //OpenVAS-Client : Could not open a connection to localhost
            //==> we should launch openvasd
            Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("END DUMP STDOUT"));

            if(stdout.Contains("Could not open a connection to localhost"))
            {
                Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("Executing command [openvassd]"));

                sshShell.WriteLine("openvassd");
                stdout = sshShell.Expect(prompt);

                //Relaunching the scan
                Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("Executing command [{0}]", cmd));

                sshShell.WriteLine(cmd);
                stdout = sshShell.Expect(prompt);
            }

            /*
            string[] tab;
            tab = stdout.Split(new char[] { '\n' });

            int pid;
            pid = Convert.ToInt32(tab[1]);

            Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("PID = [{0}]", pid));

            // =========
            // Command 3 : Wait for completion
            // =========

            while (true)
            {
                Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", "Checking for completion...");

                // cmd = string.Format("ps ax | grep {0} | cut -f1 -d\" \"", pid);
                cmd = string.Format("ps ax | grep {0}", pid);

                sshShell.WriteLine(cmd);
                stdout = sshShell.Expect(prompt);

                stdout = stdout.Replace("\r", "");

                tab = stdout.Split(new char[] { '\n' });

                bool bFound = false;
                foreach (string dummy in tab)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("   Line [{0}]", dummy.Trim()));

                    string[] tt;
                    tt = dummy.Trim().Split(new char[] { ' ' });

                    if (tt[0] == pid.ToString())
                        bFound = true;
                }

                if(bFound == false)
                    break;

                Thread.Sleep(10000);
            }

            Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", "Scan has completed");
            */

            // =========
            // Command 4 : Get results
            // =========

            string localOutputFile;
            localOutputFile = Path.GetTempFileName();

            // HACK :
            // outputfile = "634244542240861588_39608125_output";

            Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("Downloading results via SFTP to [{0}]", localOutputFile));

            try
            {
                Sftp ftp;
                ftp = new Sftp(address, username, password);
                ftp.OnTransferStart += new FileTransferEvent(ftp_OnTransferStart);
                ftp.OnTransferProgress += new FileTransferEvent(ftp_OnTransferProgress);
                ftp.OnTransferEnd += new FileTransferEvent(ftp_OnTransferEnd);

                ftp.Connect(port);

                ftp.Get(outputfile, localOutputFile);

                ftp.Close();
            }
            catch (Exception ex)
            {
                Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("Exception = {0} / {1}", ex.Message, ex.InnerException == null ? "" : ex.InnerException.Message));
                return false;
            }

            Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", "Loading the xml document");

            // HACK
            // string localOutputFile = @"C:\Users\Jerome\AppData\Local\Temp\tmpCEA4.tmp";

            try
            {
                doc.XmlResolver = null;
                doc.Load(localOutputFile);
            }
            catch (Exception ex)
            {
                Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("Exception = {0} / {1}", ex.Message, ex.InnerException == null ? "" : ex.InnerException.Message));
                return false;
            }

            #endregion

            #region WITH OPENVAS (VERSION 3)
            /*
            string folder;
            folder = string.Format("result_{0}_{1}", DateTime.Now.Ticks, this.GetHashCode());

            Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("Results will be stored in folder [{0}]", folder));

            string address, username, password;

            address = "111.222.333.444";
            username = "root";
            password = "toor";

            // address = "1.2.3.4";
            // username = "saint";
            // password = "OPENVAS!!!";

            Ssh exec;
            exec = new Ssh();
            exec.Timeout = -1;

            Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("Connecting to OPENVAS server at {0}", address));

            exec.Connect(address);
            exec.Login(username, password);

            Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("Successfully connected to OPENVAS server"));

            string output;
            string stdout = "";
            string stderr = "";

            // =========
            // Command 0 (test)
            // =========

            string cmd0;

            cmd0 = string.Format("sudo ls -al");

            Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("Executing command [{0}]", cmd0));

            stdout = exec.RunCommand(cmd0);
                       
            Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("START DUMP STDOUT"));
            Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", stdout);
            Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("END DUMP STDOUT"));

            Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("START DUMP STDERR"));
            Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", stderr);
            Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("END DUMP STDERR"));

            // =========
            // Command 1
            // =========

            string cmd1;

            string commande = string.Empty;

            var Provider = from o in m_model.JOB
                           where o.JobID == m_jobId
                           select o.PROVIDER;

            PROVIDER CurrentProvider = Provider.FirstOrDefault();

            Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("Strategy = {0} - Policy = {1} - Provider = {2}", m_strategy, m_policy, CurrentProvider.Title));

            var cmd = from o in m_model.PARAMETERSFORPROVIDER
                      where o.Policy == m_policy &&
                      o.Strategy == m_strategy &&
                      o.ProviderID == CurrentProvider.ProviderID
                      select o.Parameters;

            string CommandeToUse = cmd.FirstOrDefault();

            // HACK
            // CommandeToUse = "\"" + "-i -a 3 -Q -d" + "\"";
            // m_target = "www.target.com";

            // cmd1 = string.Format("sudo ./mysaint1.sh {0} {1} {2}", folder, m_target, CommandeToUse);
            // cmd1 = string.Format("sudo ./mysaint1.sh {0} {1}", folder, m_target);

            cmd1 = string.Format("sudo sh -c \"cd /usr/share/saint ; ./saint {0} -d {1} -L smb:root%MyMP8qt {2}\"", CommandeToUse, folder, m_target);

            Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("Executing command [{0}]", cmd1));

            stdout = exec.RunCommand(cmd1);

            Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("START DUMP STDOUT"));
            Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", stdout);
            Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("END DUMP STDOUT"));

            Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("START DUMP STDERR"));
            Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", stderr);
            Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("END DUMP STDERR"));

            // if (sb.ToString().Contains("udp_scan: no response from" + m_target + "; host is offline or UDP is filtered"))
            // throw new Exception("udp_scan: no response from" + m_target + "; host is offline or UDP is filtered");

            // =========
            // Command 1 bis
            // =========

            string cmd11;

            cmd11 = string.Format("sudo sh -c \"cd /usr/share/saint ; chmod -R a+r results\"");

            Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("Executing command [{0}]", cmd11));

            stdout = exec.RunCommand(cmd11);

            Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("START DUMP STDOUT"));
            Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", stdout);
            Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("END DUMP STDOUT"));

            Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("START DUMP STDERR"));
            Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", stderr);
            Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("END DUMP STDERR"));

            // =========
            // Command 2
            // =========

            string cmd2;
            cmd2 = string.Format("sudo ./mysaint2.sh {0} ", folder);

            cmd2 = string.Format("sudo sh -c \"cd /usr/share/saint ; ./bin/saintwriter -c full.cf -d {0} -f 7\"", folder);

            Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("Executing command [{0}]", cmd2));

            stdout = exec.RunCommand(cmd2);

            Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("START DUMP STDOUT"));
            Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", stdout);
            Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("END DUMP STDOUT"));

            Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("START DUMP STDERR"));
            Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", stderr);
            Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("END DUMP STDERR"));

            int index;
            index = stdout.IndexOf("<?xml version=");
            if (index == -1)
            {
                Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("Report does not contain XML data"));
                return false;
            }

            stdout = stdout.Substring(index);

            try
            {
                Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", "Loading the xml document");

                doc.LoadXml(stdout);
            }
            catch (Exception ex)
            {
                Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("Exception = {0} / {1}", ex.Message, ex.InnerException == null ? "" : ex.InnerException.Message));
                return false;
            }
            */

            #endregion

            #region WITHOUT OPENVAS

            //string filename;
            //filename = @"D:\Resultats_OpenVAS.xml";

            //Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("Using file '{0}'", filename));

            //doc.Load(filename);

            #endregion

            try
            {
                Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", "Getting vulnerabilities");

                 Helper_GetVulnerabilities(doc, m_target);
            }
            catch (Exception ex)
            {
                Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("Exception = {0} / {1}", ex.Message, ex.InnerException == null ? "" : ex.InnerException.Message));
                return false;
            }

            // Finshed
            return true;
        }

        void ftp_OnTransferEnd(string src, string dst, int transferredBytes, int totalBytes, string message)
        {
            Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", "SFTP transfer finished");
        }

        void ftp_OnTransferProgress(string src, string dst, int transferredBytes, int totalBytes, string message)
        {

        }

        void ftp_OnTransferStart(string src, string dst, int transferredBytes, int totalBytes, string message)
        {
            Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", "SFTP transfer started");
        }
       
        private void Helper_GetProtocolAndPort(XmlNode xn, VulnerabilityEndPoint vulnerabilityEndPoint)
        {
            vulnerabilityEndPoint.Port = -1;
            vulnerabilityEndPoint.Protocol = string.Empty;          
            string[] tab=xn.InnerText.Trim().Split(new char[]{':'});
            if (tab != null)
            {
                if (tab[0].Trim().ToUpper() == "Service".ToUpper())
                {
                    try
                    {
                      vulnerabilityEndPoint.Port= Convert.ToInt32(tab[1]);
                      vulnerabilityEndPoint.Protocol = tab[2].Split(new char[] { '\n', '\t' })[0].ToUpper();
                    }
                    catch (Exception ex)
                    {
                        Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("Error in Helper_GetProtocolAndPort : Exception = {0}", ex.Message));
                        vulnerabilityEndPoint.Protocol = tab[1].Split(new char[] { '\n', '\t' })[0].ToUpper();
                    }
                }
            }
            
            // protocol = xn.InnerText.Split(new char[] { ':' })[1].Trim().Split(new char[] { ' ' })[0].Split(new char[] { '\n', '\t', '\r' })[0];
        }

        private void Helper_GetVulnerabilities(XmlDocument s, string ipadress)
        {
            List<VulnerabilityFound> list_VulnerabilityFound;
            list_VulnerabilityFound = new List<VulnerabilityFound>();

            XmlNodeList portNodes;
            portNodes = s.SelectNodes("/openvas-report/results/result/ports/port"); //Hardcoded

            Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("There are {0} port nodes to process",portNodes.Count));

            foreach(XmlNode portNode in portNodes)
            {
                string protocol = portNode.Attributes["protocol"].Value.ToUpper();

                int port = -1;
                if(portNode.Attributes["portid"] != null)
                    port = Convert.ToInt32(portNode.Attributes["portid"].Value);

                Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("Processing port {0} protocol {1}",port, protocol));

                VulnerabilityEndPoint vulnerabilityEndPoint;
                vulnerabilityEndPoint = new VulnerabilityEndPoint();
                vulnerabilityEndPoint.IpAdress  = m_target;
                vulnerabilityEndPoint.Protocol  = protocol;
                vulnerabilityEndPoint.Port      = port;

                XmlNode ServiceNode = portNode.SelectSingleNode("service");
                vulnerabilityEndPoint.Service = ServiceNode.Attributes["name"].Value.ToUpper();

                foreach(XmlNode informationNode in portNode.SelectNodes("information"))
                {
                    string severity = informationNode.SelectSingleNode("severity").InnerText;
                    //<severity>Log Message</severity>  : Information => should be ignored
                    //<severity>Security Note</severity>
                    //<severity>Security Warning</severity>
                    string nvtId=informationNode.SelectSingleNode("id").InnerText;
                    
                    Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("   Handling nvtid {0}", nvtId));

                    XmlNode nvtNode;
                    nvtNode = s.SelectSingleNode("/openvas-report/nvts/nvt[@oid='" + nvtId + "']");

                    string title        = nvtNode.SelectSingleNode("name").InnerText;
                    string summary      = nvtNode.SelectSingleNode("summary").InnerText;
                    string risk         = nvtNode.SelectSingleNode("risk").InnerText;
                    string cve_Value    = nvtNode.SelectSingleNode("cve_id").InnerText;
                    string bid_Value = nvtNode.SelectSingleNode("bugtraq_id").InnerText;

                    Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("      Title = [{0}]", title));
                    Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("      Summary = [{0}]", summary));
                    Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("      Risk    = [{0}]", risk));
                    Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("      CVE     = [{0}]", cve_Value));

                    VulnerabilityFound vulnerabilityFound;
                    vulnerabilityFound = new VulnerabilityFound();
                    vulnerabilityFound.InnerXml     = nvtNode.InnerXml;
                    vulnerabilityFound.Title = title;
                    vulnerabilityFound.Description  = summary;
                    vulnerabilityFound.DetailedInformation = informationNode.SelectSingleNode("data").InnerText;
                    vulnerabilityFound.Consequence = informationNode.SelectSingleNode("data").InnerText;
                    //TODO: regex parse     OWASP:OWASP-CM-006

                    //vulnerabilityFound.Severity     = risk;
                    //Risk Could be:
                    //None, Unknown, Informational, Low, Medium, High
                    switch (risk) {
				        case "None":
                            vulnerabilityFound.Severity = "1";
                            break;
				        case "Unknown":
                            vulnerabilityFound.Severity = "1";
                            break;
                        case "Informational":
                            vulnerabilityFound.Severity = "2";
                            break;
                        case "Low":
                            vulnerabilityFound.Severity = "3";
                            break;
                        case "Medium":
                            vulnerabilityFound.Severity = "4";
                            break;
                        case "High":
                            vulnerabilityFound.Severity = "5";
                            break;
                    }

                    if(cve_Value.Trim().ToUpper() != "NOCVE")
                    {
                        string[] list_Cve_Value;
                        list_Cve_Value=cve_Value.Split(new char[]{','});

                        foreach(string cve in list_Cve_Value)
                        {
                            VulnerabilityFound.Item cve_Item;
                            cve_Item = new VulnerabilityFound.Item();
                            cve_Item.ID     = "cve";
                            cve_Item.Value  = cve;

                            vulnerabilityFound.ListItem.Add(cve_Item);
                        }
                    }

                    if (bid_Value.Trim().ToUpper() != "NOBID")
                    {
                        string[] list_bid_Value;
                        list_bid_Value = bid_Value.Split(new char[] { ',' });

                        foreach (string bid in list_bid_Value)
                        {
                            VulnerabilityFound.Reference bid_Reference;
                            bid_Reference = new VulnerabilityFound.Reference();
                            bid_Reference.Source = "BID";
                            bid_Reference.Title = bid;
                            bid_Reference.Url = "http://www.securityfocus.com/bid/" + bid;

                            vulnerabilityFound.ListReference.Add(bid_Reference);
                        }
                    }

                    VulnerabilityPersistor.Persist(vulnerabilityFound, vulnerabilityEndPoint, m_jobId, "OpenVas", m_model);                   
                }                   
            }
        }

        /*
        private void Helper_GetVulnerabilities(XmlDocument s, string ipadress)
        {
            List<VulnerabilityFound> list_VulnerabilityFound;
            list_VulnerabilityFound = new List<VulnerabilityFound>();

            XmlNodeList nvtsNodes;
            nvtsNodes = s.SelectNodes("/openvas-report/nvts/nvt");

            Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("There are {0} nvts nodes to process", nvtsNodes.Count));

            foreach (XmlNode nvtNode in nvtsNodes)
            {
                string nvtId;
                nvtId = nvtNode.Attributes["oid"].InnerText;

                Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("Handling nvtid {0}", nvtId));

                string summary      = nvtNode.SelectSingleNode("summary").InnerText;
                string risk         = nvtNode.SelectSingleNode("risk").InnerText;
                string cve_Value    = nvtNode.SelectSingleNode("cve_id").InnerText;

                Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("      Summary = [{0}]", summary));
                Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("      Risk    = [{0}]", risk));
                Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("      CVE     = [{0}]", cve_Value));

                VulnerabilityFound vulnerabilityFound;
                vulnerabilityFound = new VulnerabilityFound();
                vulnerabilityFound.InnerXml = nvtNode.InnerXml;
                vulnerabilityFound.Description = summary;
                vulnerabilityFound.Severity = risk;
                if (cve_Value.Trim().ToUpper() != "NOCVE")
                {
                    string[] list_Cve_Value;
                    list_Cve_Value = cve_Value.Split(new char[] { ',' });

                    foreach (string cve in list_Cve_Value)
                    {
                        VulnerabilityFound.Item cve_Item;
                        cve_Item = new VulnerabilityFound.Item();
                        cve_Item.ID     = "cve";
                        cve_Item.Value  = cve;

                        vulnerabilityFound.ListItem.Add(cve_Item);
                    }
                }

                string protocol = portNode.Attributes["protocol"].Value;

                int port = -1;
                if (portNode.Attributes["portid"] != null)
                    port = Convert.ToInt32(portNode.Attributes["portid"].Value);

                Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("Processing port {0} protocol {1}", port, protocol));

                VulnerabilityEndPoint vulnerabilityEndPoint;
                vulnerabilityEndPoint = new VulnerabilityEndPoint();
                vulnerabilityEndPoint.IpAdress = m_target;
                vulnerabilityEndPoint.Protocol = protocol;
                vulnerabilityEndPoint.Port = port;

                foreach (XmlNode informationNode in portNode.SelectNodes("information"))
                {
                    string nvtId = informationNode.SelectSingleNode("id").InnerText;
                    
                    

                    VulnerabilityPersistor.Persist(vulnerabilityFound, vulnerabilityEndPoint, m_jobId, "OpenVas", m_model);
                }
            }
        }                    
        */

        private List<VulnerabilityFound.Item> splitCVE(string s)
        {
            List<VulnerabilityFound.Item> r = new List<VulnerabilityFound.Item>();
            string[] tab = s.Split(new char[] { ' ', '\n', '\r', 't' });
            foreach(string n in tab)
            {
                VulnerabilityFound.Item item;
                item = new VulnerabilityFound.Item();
                item.ID = "cve";
                item.Value = n;
                r.Add(item);
            }
            return r;
        }

        //private List<VULNERABILITYFOUND> Helper_PersistVulnerability(VulnerabilityFound detail, VulnerabilityEndPoint endpoint)
        //{
            //
            /*
            // ===============================================
            // If necessary, create an EndPoint and get its id
            // ===============================================

            int theEndPointID = 0;

            Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("Searching for an asset with IP address {0}", endpoint.IpAdress));

            var asset = from Assets in m_model.ASSET
                        where Assets.IpAdress == endpoint.IpAdress
                        select Assets;
            ASSET myAsset = asset.ToList().FirstOrDefault();

            Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("Retrieving the endpoints"));

            ENDPOINT endPoint = new ENDPOINT();
            var EP = from Epoint in m_model.ENDPOINT
                     where Epoint.AssetID == myAsset.AssetID
                     select Epoint;

            Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("Found {0} enpoints", EP.ToList().Count));

            if (EP.ToList().Count == 0)
            {
                Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("No endpoint found, creating a new one (port={0}, proto={1})", endpoint.Port, endpoint.Protocol));

                ENDPOINT tmpEP = new ENDPOINT();
                tmpEP.Port      = (int?)endpoint.Port;
                tmpEP.Protocol  = endpoint.Protocol;
                tmpEP.AssetID   = myAsset.AssetID;

                m_model.AddToENDPOINT(tmpEP);
                m_model.SaveChanges();

                theEndPointID = tmpEP.EndPointID;
            }
            else
            {
                Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("Looking for the right endpoint"));

                foreach (ENDPOINT E in EP.ToList())
                {
                    if (E.Protocol == endpoint.Protocol && E.Port == endpoint.Port)
                    {
                        theEndPointID = E.EndPointID;
                        break;
                    }
                }


                if (theEndPointID == 0)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("Coulnd not find the endpoint"));

                    Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("Creating a new one (port={0}, proto={1})", endpoint.Port, endpoint.Protocol));

                    ENDPOINT tmpEP = new ENDPOINT();
                    tmpEP.Port      = (int?)endpoint.Port;
                    tmpEP.Protocol  = endpoint.Protocol;
                    tmpEP.AssetID   = myAsset.AssetID;

                    m_model.AddToENDPOINT(tmpEP);
                    m_model.SaveChanges();

                    theEndPointID = tmpEP.EndPointID;
                }
            }

            // ============

            List<VULNERABILITYFOUND> list;
            list = new List<VULNERABILITYFOUND>();

            // =========================
            // In case there is no CVEID
            // =========================

            if (detail.ListItem.Count == 0)
            {
                Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("Searching for a Saint entry in VULNERABILITYSYNONYM"));

                var syn1 = from S in m_model.VULNERABILITYSYNONYM
                           where S.Referential.Equals("saint") &&
                           S.Value == detail.InnerXml
                           select S;

                VULNERABILITYSYNONYM VS;

                if (syn1.Count() == 0)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("No entry found, creating a new Saint entry in VULNERABILITYSYNONYM"));

                    VS = new VULNERABILITYSYNONYM();
                    VS.Referential = "saint";
                    VS.Value = detail.InnerXml;
                    VS.Description = detail.Description;
                    VS.Consequence = detail.Consequence;
                    VS.Solution = detail.Solution;
                    // VS.Severity     = detail.Severity;

                    m_model.AddToVULNERABILITYSYNONYM(VS);

               //     Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("Coucou 33"));

                    m_model.SaveChanges();

                    VS.Parent = VS.ID;

                    m_model.SaveChanges();

               //     Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("Coucou 44"));
                }
                else
                {
                    VS = syn1.First();
                }

                // =====================

                VULNERABILITYFOUND MyVuln = new VULNERABILITYFOUND();
                MyVuln.VulnerabilityID  = VS.ID;
                MyVuln.EndPointID       = theEndPointID;
                
                m_model.AddToVULNERABILITYFOUND(MyVuln);
                m_model.SaveChanges();

            //    Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("Coucou 55"));

                list.Add(MyVuln);
            }

            // ====================================
            // In case there are at least one CVEID
            // ====================================

            foreach (VulnerabilityFound.Item item in detail.ListItem)
            {
                Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("Handling CVE {0}", item.Value));

                // ========================================
                // Search VULNERABILITYSYNONYM for this CVE
                // ========================================

                 var syn = from S in m_model.VULNERABILITYSYNONYM
                          where S.Referential.Equals("cve") &&
                          S.Value.Equals(item.Value)
                          select S;
				  
                if (syn.Count() == 0)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("Error : CVEID {0} not found in VULNERABILITYSYNONYM", item.Value));
                    continue;
                }

                Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("This CVE matches to ID {0} in table VULNERABILITYSYNONYM", syn.ToList().First().ID));

                Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("Creating a new entry in table VULNERABILITYFOUND"));

                VULNERABILITYFOUND MyVuln = new VULNERABILITYFOUND();
                MyVuln.VulnerabilityID  = syn.ToList().First().ID;
                MyVuln.EndPointID       = theEndPointID;

                m_model.AddToVULNERABILITYFOUND(MyVuln);
                m_model.SaveChanges();

                list.Add(MyVuln);

                // ===============================================================
                // If necessary, create an entry in VULNERABILITYSYNONYM for Saint
                // ===============================================================

                Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("Searching for a Saint entry in VULNERABILITYSYNONYM"));

                var syn1 = from S in m_model.VULNERABILITYSYNONYM
                           where S.Referential.Equals("saint") &&
                           S.Value == detail.InnerXml
                           select S;
                if (syn1.Count() != 0)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("Entry found !!!!!!!!!!!!!!!!!"));


                    //if(syn1.ToList().First().Parent != MyVuln.VulnerabilityID)
                    //{
                    //    Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("GROS BUG"));
                    //    continue;
                    //}                      
                }
                else
                {
                    Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("No entry found, creating a new Saint entry in VULNERABILITYSYNONYM"));

                    VULNERABILITYSYNONYM VS = new VULNERABILITYSYNONYM();
                    VS.Referential  = "saint";
                    VS.Value        = detail.InnerXml;
                    VS.Description  = detail.Description;
                    VS.Consequence  = detail.Consequence;
                    VS.Solution     = detail.Solution;
                    VS.Parent       = MyVuln.VulnerabilityID;

                    MyVuln.Severity = detail.Severity;

                    m_model.AddToVULNERABILITYSYNONYM(VS);

                //    Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("Coucou 1"));

                    m_model.SaveChanges();

                //    Utils.Helper_Trace("XORCISM PROVIDER OPENVAS", string.Format("Coucou 2"));
                }
            }
            return list;
            */

            //return null;
        //}
    }
    
    class _VulnerabilityEndPoint
    {
        #region Properties
        public string Protocol { get { return m_protocol; } set { m_protocol = value; } }
        public int Port { get { return m_port; } set { m_port=value;} }
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
