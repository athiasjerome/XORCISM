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
using System.Configuration;

using XORCISMModel;
using XCommon;
using XProviderCommon;

using System.Text.RegularExpressions;

//using Tamir.SharpSsh;

//using Rebex.Net;
//using Rebex.TerminalEmulation;

using FSM.DotNetSSH;

using System.Threading;

namespace XProviderSaint
{
    /// <summary>
    /// Copyright (C) 2012-2015 Jerome Athias
    /// XORCISM Plugin for SAINT
    /// All trademarks and registered trademarks are the property of their respective owners.
    /// This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation; either version 2 of the License, or (at your option) any later version.
    /// 
    /// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
    /// 
    /// You should have received a copy of the GNU General Public License along with this program; if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
    /// </summary>
    public class Engine : MarshalByRefObject, XProviderCommon.IVulnerabilityDetector
    {
        private string m_data=string.Empty;

        public Engine()
        {
            TextWriterTraceListener tw;
            tw = new TextWriterTraceListener("XProviderSaint.log"); //Hardcoded
            Trace.Listeners.Add(tw);
            Trace.AutoFlush = true;
            Trace.IndentSize = 4;           
        }

        public override object InitializeLifetimeService()
        {
            return null; //Allow infinite lifetime
        }

        public void Run(string target, int jobID, string policy, string strategy)
        {
            Utils.Helper_Trace("XORCISM PROVIDER SAINT", "Entering Run()");
            Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("Target = {0} , JobID = {1} , Policy = {2}, Strategy = {3}", target, jobID, policy, strategy));

            SaintParser saintParser=null;
            try
            {
                saintParser = new SaintParser(target, jobID, policy,strategy);
            }
            catch (Exception ex)
            {
                Utils.Helper_Trace("XORCISM PROVIDER SAINT", "JobID:" + jobID + "Exception SaintParser = " + ex.Message + " " + ex.InnerException);
            }

            string status = XCommon.STATUS.FINISHED.ToString();            

            // =================================================
            // Change the status of the job to FINISHED or ERROR
            // =================================================
            if (saintParser.Parse() == false)
            {
                status = XCommon.STATUS.ERROR.ToString();
                Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("Updating job {0} status to ERROR", jobID));
                XCommon.Utils.Helper_SendEmail("contact@hackenaton.org", "Saint ERROR", "Saint ERROR for job:"+jobID);  //Hardcoded
            }
            else
            {
                Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("Updating job {0} status to FINISHED", jobID));
            }
            try
            {
                XORCISMEntities model = new XORCISMEntities();
                var Q = from j in model.JOB
                           where j.JobID == jobID
                           select j;

                JOB myJob = Q.FirstOrDefault();
                myJob.Status = status;
                myJob.DateEnd = DateTimeOffset.Now;
                //image
                System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
                myJob.XmlResult = encoding.GetBytes(m_data);
                model.SaveChanges();
                //FREE MEMORY
                model.Dispose();
                saintParser = null;
            }
            catch (Exception ex)
            {
                Utils.Helper_Trace("XORCISM PROVIDER SAINT", "JobID:"+jobID+"Exception UpdateJob = " + ex.Message+" "+ex.InnerException);                
            }

            Utils.Helper_Trace("XORCISM PROVIDER SAINT", "JobID:" + jobID + "Leaving Run()");
        }
    }

    class SaintParser
    {
        private string          m_target;
        private XORCISMEntities m_model;
        private int             m_jobId;
        private string          m_policy;
        private string          m_strategy;
        private string m_data=string.Empty;

        public SaintParser(string target,int jobId, string policy, string strategy)
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

            Utils.Helper_Trace("XORCISM PROVIDER SAINT", "SAINT Assembly location = " + a.Location);

            XmlDocument doc = new XmlDocument();

            #region WITH SAINT (VERSION 1)
            /*
            string folder;
            folder = string.Format("result_{0}_{1}", DateTime.Now.Ticks, this.GetHashCode());
            Utils.Helper_Trace("XORCISM PROVIDER SAINT", "FolderName is ="+folder);
            //_SaintSettings settings = _SaintSettings.getInstance();

            //Utils.Helper_Trace("XORCISM PROVIDER SAINT", "Connecting to remote server by SSH" + settings.SaintIP);

           // SshStream sshStream = new SshStream(settings.SaintIP, settings.SaintLogin, settings.SaintPassword);
            //string saintIP = ConfigurationManager.AppSettings["SAINT_IP"];
            //string saintLogin = ConfigurationManager.AppSettings["SAINT_LOGIN"];
            //string saintPassword = ConfigurationManager.AppSettings["SAINT_PASSWORD"];

           // SshStream sshStream = new SshStream(saintIP, saintLogin, saintPassword);

            // Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("saintIP={0},saintLogin={1},saintPassword={2}", saintIP, saintLogin, saintPassword));

            SshStream sshStream = new SshStream("1.2.3.4", "saint", "SAINT!!!");

            //SshExec exec;
            //exec = new SshExec("1.2.3.4", "saint");
            //exec.Password = "SAINT!!!";

            //exec.Connect();

            //Utils.Helper_Trace("XORCISM PROVIDER SAINT", "Successfully connected to remote server");

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
            // sw.WriteLine("SAINT!!!");
            // Utils.Helper_Trace("XORCISM PROVIDER SAINT", "Password = SAINT!!!");
            // Utils.Helper_Trace("XORCISM PROVIDER SAINT", "Executing SAINT Query:" + sw.ToString());

            sw.WriteLine(cmd2);
            sw.Flush();

            //Utils.Helper_Trace("XORCISM PROVIDER SAINT", "Sleep 10000 : BEGIN");
            ////Thread.Sleep(10000);
            //Utils.Helper_Trace("XORCISM PROVIDER SAINT", "Sleep 10000 : END");
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
                        Utils.Helper_Trace("XORCISM PROVIDER SAINT", "XmlReport Found");
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
                Utils.Helper_Trace("XORCISM PROVIDER SAINT", "Exception = " + ex.Message);
            }

            Utils.Helper_Trace("XORCISM PROVIDER SAINT", "Parsing Saint Result into Xml");

            doc.LoadXml(VulnerabilityXml);
            */
            #endregion

            #region WITH SAINT (VERSION 2)

            /*
            string folder;
            folder = string.Format("result_{0}_{1}", DateTime.Now.Ticks, this.GetHashCode());

            Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("Results will be stored in folder [{0}]",folder));

            string address = "111.222.333.444";

            SshExec exec;
            //exec = new SshExec("1.2.3.4", "saint");
            //exec.Password = "SAINT!!!";
            exec = new SshExec(address, "root");
            exec.Password = "toor";

            Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("Connecting to SAINT server at {0}", address));

            exec.Connect();

            Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("Successfully connected to SAINT server"));

            string output;
            string  stdout = "";
            string  stderr = "";

            // =========
            // Command 0 (test)
            // =========

            string cmd0;

            cmd0 = string.Format("sudo ls -al");

            Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("Executing command [{0}]", cmd0));

            stdout = ""; stderr = "";
            output = exec.RunCommand2(cmd0, ref stdout, ref stderr).ToString();

            Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("START DUMP STDOUT"));
            Utils.Helper_Trace("XORCISM PROVIDER SAINT", stdout);
            Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("END DUMP STDOUT"));

            Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("START DUMP STDERR"));
            Utils.Helper_Trace("XORCISM PROVIDER SAINT", stderr);
            Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("END DUMP STDERR"));

            // =========
            // Command 1
            // =========

            string cmd1;

            string commande = string.Empty;

            var Provider = from o in m_model.JOB
                            where o.JobID == m_jobId
                            select o.PROVIDER;

            PROVIDER CurrentProvider = Provider.FirstOrDefault();

            Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("Strategy = {0} - Policy = {1} - Provider = {2}", m_strategy,m_policy,CurrentProvider.Title));

            var cmd = from o in m_model.PARAMETERSFORPROVIDER
                      where o.Policy == m_policy && 
                      o.Strategy == m_strategy && 
                      o.ProviderID == CurrentProvider.ProviderID
                      select o.Parameters;

            string CommandeToUse = "\"" + cmd.FirstOrDefault() + "\"";

            // HACK
            CommandeToUse = "\"" + "-i -a 3 -Q -d" + "\"";
            m_target = "www.target.com";

            // cmd1 = string.Format("sudo ./mysaint1.sh {0} {1} {2}", folder, m_target, CommandeToUse);
            cmd1 = string.Format("sudo ./mysaint1.sh {0} {1}", folder, m_target);

            Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("Executing command [{0}]", cmd1));

            stdout = ""; stderr = "";
            output = exec.RunCommand2(cmd1, ref stdout, ref stderr).ToString();

            Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("START DUMP STDOUT"));
            Utils.Helper_Trace("XORCISM PROVIDER SAINT", stdout);
            Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("END DUMP STDOUT"));

            Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("START DUMP STDERR"));
            Utils.Helper_Trace("XORCISM PROVIDER SAINT", stderr);
            Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("END DUMP STDERR"));

            // if (sb.ToString().Contains("udp_scan: no response from" + m_target + "; host is offline or UDP is filtered"))
            // throw new Exception("udp_scan: no response from" + m_target + "; host is offline or UDP is filtered");

            // =========
            // Command 2
            // =========

            string cmd2;
            cmd2 = string.Format("sudo ./mysaint2.sh {0} ", folder);

            Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("Executing command [{0}]", cmd2));

            stdout = ""; stderr = "";
            output = exec.RunCommand2(cmd2, ref stdout, ref stderr).ToString();

            Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("START DUMP STDOUT"));
            Utils.Helper_Trace("XORCISM PROVIDER SAINT", stdout);
            Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("END DUMP STDOUT"));

            Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("START DUMP STDERR"));
            Utils.Helper_Trace("XORCISM PROVIDER SAINT", stderr);
            Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("END DUMP STDERR"));

            try
            {
                Utils.Helper_Trace("XORCISM PROVIDER SAINT", "Loading the xml document");

                doc.LoadXml(stdout);
            }
            catch (Exception ex)
            {
                Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("Exception = {0} / {1}", ex.Message, ex.InnerException == null ? "" : ex.InnerException.Message));
                return false;
            }
            */

            #endregion

            #region WITH SAINT (VERSION 3)
            
            string folder;
            folder = string.Format("result_{0}_{1}", DateTime.Now.Ticks, this.GetHashCode());

            Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("JobID: {0} Results will be stored in folder [{1}]", m_jobId, folder));
            int port=22;
            string address, username, password;
            string prompt;
            /*
            //OVH
            address = "111.222.333.444";
            username = "root";
            password = "toor";
            prompt = "root@xmachine:";
            */

            address = "111.222.333.444";
            username = "root";
            password = "toor";
            prompt = "root";//@backtrack:";

            //Ssh exec;
            //exec = new Ssh();
            //exec.Timeout = -1;

            SshShell sshShell;
            sshShell = new SshShell(address, username, password);
            sshShell.RemoveTerminalEmulationCharacters = true;

            Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("JobID: {0} Connecting to SAINT server at {1}", m_jobId, address));

            //exec.Connect(address);
            //exec.Login(username, password);
            try
            {
                sshShell.Connect(port);
                //sshShell.Expect(prompt+"~#");
                sshShell.Expect(prompt);
            }
            catch (Exception ex)
            {
                Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("JobID: {0} ConnectingERROR to SAINT server at {1} : " + ex.Message + " " + ex.InnerException, m_jobId, address));
                address = "111.222.333.444";
                username = "root";
                password = "toor";
                prompt = "root";//@backtrack:";
                sshShell = new SshShell(address, username, password);
                sshShell.RemoveTerminalEmulationCharacters = true;
                
                Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("JobID: {0} Connecting to SAINT server at {1}", m_jobId, address));
                try
                {
                    sshShell.Connect(port);
                    sshShell.Expect(prompt);
                }
                catch (Exception ex2)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("JobID: {0} ConnectingERROR to SAINT server at {1} : " + ex2.Message + " " + ex2.InnerException, m_jobId, address));
                }
            }

            Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("JobID: {0} Successfully connected to SAINT server", m_jobId));

            //string output;
            string stdout = "";
            string stderr = "";

            // =========
            // Command 0 (test)
            // =========

                    //string cmd0;
                    //cmd0 = string.Format("sudo ls -al");

                    //Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("Executing command [{0}]", cmd0));

                    ////stdout = exec.RunCommand(cmd0);
                    //sshShell.WriteLine(cmd0);
                    //stdout = sshShell.Expect(prompt);
                       
                    //Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("START DUMP STDOUT"));
                    //Utils.Helper_Trace("XORCISM PROVIDER SAINT", stdout);
                    //Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("END DUMP STDOUT"));

                    //Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("START DUMP STDERR"));
                    //Utils.Helper_Trace("XORCISM PROVIDER SAINT", stderr);
                    //Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("END DUMP STDERR"));
            



            // =========
            // Command 1
            // =========

            string cmd1;

            string commande = string.Empty;

            var Provider = from o in m_model.JOB
                           where o.JobID == m_jobId
                           select o.PROVIDER;

            PROVIDER CurrentProvider = Provider.FirstOrDefault();

            var Session = from o in m_model.JOB
                           where o.JobID == m_jobId
                           select o;

            JOB theSession = Session.FirstOrDefault();
            int mySessionID = (int)theSession.SessionID;

            Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("Strategy = {0} - Policy = {1} - Provider = {2} - Session = {3}", m_strategy, m_policy, CurrentProvider.ProviderName, mySessionID));

            var cmd = from o in m_model.PARAMETERSFORPROVIDER
                      where o.Policy == m_policy &&
                      o.Strategy == m_strategy &&
                      o.ProviderID == CurrentProvider.ProviderID
                      select o.Parameters;

            string CommandeToUse = cmd.FirstOrDefault();

            //CREDENTIALS
            var tmpUser = from U in m_model.JOB
                          where U.JobID == m_jobId
                          select U.SESSION.UserID;

            Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("User ID = {0}", tmpUser.FirstOrDefault()));

            var tmpAccount = from Ac in m_model.USERACCOUNT
                             where Ac.UserID == tmpUser.FirstOrDefault()
                             select Ac.ACCOUNT;

            ACCOUNT xAccount = tmpAccount.FirstOrDefault();

            Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("Account ID = {0}", xAccount.AccountID));
            /*
            var tmpAsset = from A in m_model.ASSET
                           where A.IpAdress == m_target &&
                           A.AccountID == xAccount.AccountID
                           select A;
            */
            var ipAsset = from o in m_model.JOB
                          where o.JobID == m_jobId
                          select o.ASSETSESSION.ASSET;

            ASSET tmpAsset = new ASSET();
            try
            {
                tmpAsset = ipAsset.FirstOrDefault();
                //TODO  ipaddressIPv4
                Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("JobID: {0} tmpAsset ID = {1}    IP = {2}", m_jobId, tmpAsset.AssetID, tmpAsset.ipaddressIPv4));
            }
            catch (Exception ex)
            {
                Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("JobID: {0} tmpAsset Exception = {1} / {2}", m_jobId, ex.Message, ex.InnerException == null ? "" : ex.InnerException.Message));
                return false;
            }

            var asset = from Assets in m_model.ASSET
                        //where Assets.IpAdress == tmpAsset.IpAdress
                        where Assets.AssetID == tmpAsset.AssetID
                        select Assets;
            //ASSET myAsset = new ASSET();
            //myAsset = asset.FirstOrDefault();
            ASSET xAsset = new ASSET();
            try
            {
                xAsset = asset.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("JobID: {0} xAsset Exception = {1} / {2}", m_jobId, ex.Message, ex.InnerException == null ? "" : ex.InnerException.Message));
                return false;
            }
            //TODO  ipaddressIPv4
            Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("JobID: {0} xAsset ID = {1}    IP = {2}", m_jobId, xAsset.AssetID, xAsset.ipaddressIPv4));

            var Credentials = from oCredentials in m_model.ASSETCREDENTIAL
                              where oCredentials.AssetID == xAsset.AssetID
                              select oCredentials;

            string AssetCredentials;
            AssetCredentials = string.Empty;

            foreach (ASSETCREDENTIAL AC in Credentials.ToList())
            {
                switch (AC.AuthenticationType)
                {
                    case "SMB":
                        {
                            AssetCredentials += "smb:"+AC.Username+"%"+AC.Password;
                            //smb_user  ???
                            break;
                        }
                    case "SSH":
                        {
                            AssetCredentials += "ssh:" + AC.Username + "%" + AC.Password;
                            break;
                        }
                    case "ORACLE":
                        {
                            AssetCredentials += "oracle:" + AC.Username + "%" + AC.Password;
                            break;
                        }
                    case "HTTP":
                        {
                            AssetCredentials += "basic:" + AC.Username + "%" + AC.Password;
                            break;
                        }
                    //**************
                    case "MSSQL":
                        {
                            AssetCredentials += "mssql:" + AC.Username + "%" + AC.Password;
                            break;
                        }
                    case "MYSQL":
                        {
                            AssetCredentials += "mysql:" + AC.Username + "%" + AC.Password;
                            break;
                        }

                }
            }
            if (AssetCredentials != "")
            {
                CommandeToUse += " -L " + AssetCredentials;
            }

            string scanningtarget = m_target;

            if (scanningtarget.Length - scanningtarget.Replace(".", "").Length == 3 && scanningtarget.Length - scanningtarget.Replace(":", "").Length == 1)
            {
                //target like: 198.10.11.12:8089
                char[] splitters = new char[] { ':' };
                string[] laCase = scanningtarget.Split(splitters);
                scanningtarget = laCase[0];
                Utils.Helper_Trace("XORCISM PROVIDER SAINT", "target without port: "+scanningtarget);
            }

            //Check if we have an IP address
            //string pattern = @"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\.
            //([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$";
            string pattern = @"\d\d?\d?\.\d\d?\d?\.\d\d?\d?\.\d\d?\d?"; //TODO IPv6
            //create our Regular Expression object
            Regex check = new Regex(pattern);

            if (check.IsMatch(m_target.Trim(), 0))
            {
                Utils.Helper_Trace("XORCISM PROVIDER SAINT", "target is an IP address");
            }
            else
            {
                //It should be a domain name due to problem ie: www.vulnerabilitydatabase.com (Unlicensed host)
                Utils.Helper_Trace("XORCISM PROVIDER SAINT", "target is not an IP address");
                //scanningtarget = m_target.Replace("http://", "");
                //scanningtarget = scanningtarget.Replace("https://", "");
                try
                {
                    if (!m_target.Contains("://"))
                        scanningtarget = "http://" + m_target;
                    /*
                    scanningtarget = new Uri(scanningtarget).Host;
                    Utils.Helper_Trace("XORCISM PROVIDER SAINT", "JobID:" + m_jobId + " targetmodified: " + scanningtarget);
                    if (check.IsMatch(scanningtarget.Trim(), 0))
                    {
                        Utils.Helper_Trace("XORCISM PROVIDER SAINT", "JobID:" + m_jobId + " targetmodified is an IP address");
                    }
                    else
                    {
                        IPHostEntry ipEntry = Dns.GetHostEntry(scanningtarget);
                        IPAddress[] addr = ipEntry.AddressList;
                        scanningtarget = addr[0].ToString();
                    }
                    */
                }
                catch (Exception ex)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER SAINT", "Exception Dns.GetHostEntry "+ex.Message+" "+ex.InnerException);

                }
            }








            // HACK
            // CommandeToUse = "\"" + "-i -a 3 -Q -d" + "\"";
            // m_target = "www.target.com";

            // cmd1 = string.Format("sudo ./mysaint1.sh {0} {1} {2}", folder, m_target, CommandeToUse);
            // cmd1 = string.Format("sudo ./mysaint1.sh {0} {1}", folder, m_target);

            //cmd1 = string.Format("sudo sh -c \"cd /usr/share/saint ; ./saint {0} -d {1} -L smb:root%MyMP8qt {2}\"", CommandeToUse, folder, m_target);
//            cmd1 = string.Format("sudo sh -c \"cd /usr/share/saint ; ./saint {0} -d {1} {2}\"", CommandeToUse, folder, m_target);
            cmd1 = string.Format("sudo sh -c \"cd /usr/share/saint ; ./saint {0} -d {1} {2}\"", CommandeToUse, folder, scanningtarget);

            Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("JobID: {0} Executing command [{1}]", m_jobId, cmd1));

            //stdout = exec.RunCommand(cmd1);
            sshShell.WriteLine(cmd1);
            stdout = sshShell.Expect(prompt);
            sshShell.WriteLine(password);
            stdout = sshShell.Expect(prompt);


            Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("JobID: {0} START DUMP STDOUT01", m_jobId));
            Utils.Helper_Trace("XORCISM PROVIDER SAINT", stdout);

            /*
             * udp_scan: no response from ll1-2-3-102-81.ll81.xxx; host is offline or UDP is filtered
             * 
                sudo sh -c "cd /usr/share/saint ; ./saint -a 2 -b -TT -X -d res 
                ult_634370180691981451_56524389 1.2.3.4"
                udp_scan: no response from xxx.yyy.zzz; host is offline or UDP is filtered
                udp_scan: no response from xxx.yyy.zzz; host is offline or UDP is filtered

                xxx.yyy.zzz:
                   Information:
                      Microsoft Windows Vista SP0 or SP1 or Server 2008 SP1
                      NS351289
                      OS=[Windows Server 2008 R2 Enterprise 7600] Server=[Windows Server 2008 R2 Enterprise 6.1]
                      Web Directories: /svn/, 
                   Potential Problems:
                      ICMP timestamp requests enabled
                      Microsoft SQL Server vulnerable version
                      Possible internet facing database on port 1433
                      Possible vulnerability in Microsoft Terminal Server
                      SSL certificate is self signed
                      SSL certificate is signed with weak hash function: SHA1
                      TCP timestamp requests enabled
                   Services:
                      1433/TCP
                      2103/TCP
                      2107/TCP
                      3389/TCP
                      912/TCP
                      SMB
                      WWW
                      WWW (non-standard port 8443)
                      eklogin (2105/TCP)
                      epmap (135/TCP)
                      microsoft-ds (445/TCP)
                root@xmachine:~#     
            */
            Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("JobID: {0} END DUMP STDOUT01", m_jobId));
            int cptretrylogin = 0;
            int currentscanner = 0;
            while (stdout.Contains("Unlicensed target host") && cptretrylogin < 50) //Hardcoded
            {
                //Retry
                cptretrylogin++;
                if (currentscanner < 5)
                {
                    currentscanner++;
                }
                else
                {
                    Thread.Sleep(20000);
                }
                cmd1 = string.Format("sudo sh -c \"cd /usr/share/saint"+currentscanner+" ; ./saint {0} -d {1} {2}\"", CommandeToUse, folder, scanningtarget);   //Hardcoded
                Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("JobID: {0} RETRYING" + cptretrylogin + " Executing command [{1}]", m_jobId, cmd1));

                //stdout = exec.RunCommand(cmd1);
                sshShell.WriteLine(cmd1);
                stdout = sshShell.Expect(prompt);

                Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("JobID: {0} START DUMP STDOUT02", m_jobId));
                Utils.Helper_Trace("XORCISM PROVIDER SAINT", stdout);
                Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("JobID: {0} END DUMP STDOUT02", m_jobId));
                
            }
            if (stdout.Contains("Unlicensed target host"))
            {
//                XCommon.Utils.Helper_SendEmail("contact@hackenaton.org", "Saint Unlicensed target host", "Unlicensed target host: " + m_target+" jobID: "+m_jobId);
                XCommon.Utils.Helper_SendEmail("contact@hackenaton.org", "Saint Unlicensed target host", "Unlicensed target host: " + scanningtarget + " jobID: " + m_jobId);   //Hardcoded
            }

            //These informations are useful
            //TODO A FAIRE: store in SESSION or INFORMATION or ASSETINFORMATION

            Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("JobID: {0} ASSET ID => {1}", m_jobId, xAsset.AssetID));

            string[] myLines = stdout.Split(new char[]{'\n'});
            bool startServices = false;
            bool startInformation = false;
            string information = string.Empty;
            for (int cpt = 1; cpt < myLines.Length-1; cpt++)
            {
                if (startInformation)
                {
                    information += myLines[cpt].Trim();
                }
                if (startServices)
                {
                    string strTemp = myLines[cpt].Trim();
                    Utils.Helper_Trace("XORCISM PROVIDER SAINT", "strTemp=" + strTemp);   //NNTP (Usenet news)
                    string strService="";
                    string strPort="-1";
                    port = -1;
                    string strProtocol="";
                    if (strTemp.Contains("("))
                    {
                        string[] tab = strTemp.Split(new Char[] { '(' });
                        strService = tab[0].Trim().ToUpper();

                        if (tab[1].Contains("/"))
                        {
                            tab = tab[1].Split(new Char[] { '/' });
                            strPort = tab[0];
                        }
                        else
                        {
                            strPort = tab[1].Replace(")", "");  //Usenet news
                            strPort = strPort.Replace("non-standard port", "").Trim();  //Hardcoded
                            if (int.TryParse(strPort, out port) == false)
                            {
                                // String is not a number.
                                port = -1;
                                strPort = "-1";
                                strService = strTemp.ToUpper();
                            }
                        }
                    }
                    else
                    {
                        Utils.Helper_Trace("XORCISM PROVIDER SAINT", "TRACE2");
                        if (strTemp.Contains("/"))
                        {
                            Utils.Helper_Trace("XORCISM PROVIDER SAINT", "TRACE3");
                            string[] tab = strTemp.Split(new Char[] { '/' });
                            strPort = tab[0].Trim();
                        }
                        else
                        {
                            strService = strTemp.Trim().ToUpper();
                            //WWW => 80
                            //...
                        }
                    }

                    if (strTemp.Contains("TCP"))
                    {
                        strProtocol = "TCP";
                    }
                    if (strTemp.Contains("UDP"))
                    {
                        strProtocol = "UDP";
                    }

                    if(strPort.Trim()=="")
                    {
                        strPort="-1";
                    }
                    try
                    {
                        if (int.TryParse(strPort, out port) == false)
                        {
                            // String is not a number.
                            port = -1;
                        }
                        else
                        {
                            port = Convert.ToInt32(strPort);
                        }
                    }
                    catch (Exception ex)
                    {
                        //WWW (Secure)
                        Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("Exception strPort = {0} / {1}", ex.Message, ex.InnerException == null ? "" : ex.InnerException.Message));
                        port = -1;
                        //    return false;
                    }

                    if (strService == "")
                    {
                        Utils.Helper_Trace("XORCISM PROVIDER SAINT", "TRACE4");
                        //Use the PORT referential table to retrieve the service
                        var ports = from portref in m_model.PORT
                                    where portref.DefaultProtocolName == strProtocol && portref.Port_Value == port
                                    select portref;
                        if (ports.Count() > 0)
                        {
                            PORT thePort = ports.FirstOrDefault();
                            strService = thePort.DefaultServiceName.Trim();
                            Utils.Helper_Trace("XORCISM PROVIDER SAINT", "Service found from referential:" + strService);
                        }
                    }
                    else
                    {
                        //Complete the PORT referential table
                        if (strTemp.Contains("non-standard port"))
                        {
                            if (strProtocol == "")
                            {
                                Utils.Helper_Trace("XORCISM PROVIDER SAINT", "TRACE4BIS");
                                var ports = from portref in m_model.PORT
                                            where portref.DefaultServiceName == strService
                                            select portref;

                                if (ports.Count() != 0)
                                {
                                    PORT thePort = ports.FirstOrDefault();
                                    strProtocol = thePort.DefaultProtocolName.Trim();
                                }
                            }
                        }
                        else
                        {
                            if (port != -1)
                            {
                                Utils.Helper_Trace("XORCISM PROVIDER SAINT", "TRACE5");
                                var ports = from portref in m_model.PORT
                                            where portref.DefaultProtocolName == strProtocol && portref.Port_Value == port && portref.DefaultServiceName == strService
                                            select portref;

                                if (ports.Count() == 0)
                                {
                                    PORT newPortReference = new PORT();
                                    newPortReference.Port_Value = port;
                                    newPortReference.DefaultProtocolName = strProtocol.Trim();
                                    newPortReference.DefaultServiceName = strService.Trim();

                                    m_model.PORT.Add(newPortReference);
                                    m_model.SaveChanges();
                                    Utils.Helper_Trace("XORCISM PROVIDER SAINT", "Adding new referential: Port:" + port+ " Protocol:"+ strProtocol +" Service:"+strService);
                                }
                            }
                            else
                            {
                                //Looking for the default port/protocol
                                if (strProtocol == "")
                                {
                                    Utils.Helper_Trace("XORCISM PROVIDER SAINT", "TRACE6");
                                    var ports = from portref in m_model.PORT
                                                where portref.DefaultServiceName == strService
                                                select portref;

                                    if (ports.Count() != 0)
                                    {
                                        PORT thePort = ports.FirstOrDefault();
                                        strPort = thePort.Port_Value.ToString();
                                        Utils.Helper_Trace("XORCISM PROVIDER SAINT", "port/protocol found from referential: Port:" + strPort + " Protocol:" + strProtocol);
                                        port = Convert.ToInt32(thePort.Port_Value);
                                        strProtocol = thePort.DefaultProtocolName.Trim();                                        
                                    }
                                }
                                else
                                {
                                    Utils.Helper_Trace("XORCISM PROVIDER SAINT", "TRACE7");
                                    var ports = from portref in m_model.PORT
                                                where portref.DefaultServiceName == strService && portref.DefaultProtocolName == strProtocol
                                                select portref;

                                    if (ports.Count() != 0)
                                    {                                        
                                        PORT thePort = ports.FirstOrDefault();
                                        strPort = thePort.Port_Value.ToString();
                                        Utils.Helper_Trace("XORCISM PROVIDER SAINT", "port found from referential: Port:" + strPort);
                                        port = Convert.ToInt32(thePort.Port_Value);                                        
                                    }
                                }
                            }
                        }
                    }

                    Utils.Helper_Trace("XORCISM PROVIDER SAINT", "Service:"+strService);
                    Utils.Helper_Trace("XORCISM PROVIDER SAINT", "Port:" + strPort);
                    Utils.Helper_Trace("XORCISM PROVIDER SAINT", "Protocol:" + strProtocol);

                    int theEndPointID = 0;
                    if (int.TryParse(strPort, out port) == false)
                    {
                        // String is not a number.
                        port = -1;
                    }
                    else
                    {
                        port = Convert.ToInt32(strPort);
                    }
                    //Check if the endpoint already exists
                    var EP = from Epoint in m_model.ENDPOINT
                             where Epoint.AssetID == xAsset.AssetID && Epoint.SessionID == mySessionID
                             select Epoint;
                    if (strService == "HTTPS")
                    {
                        strService = "WWW (SECURE)";
                        if (port == -1)
                        {
                            port = 443;
                        }
                    }
                    foreach (ENDPOINT E in EP.ToList())
                    {
                        if (E.ProtocolName == strProtocol && E.PortNumber == port && E.Service == strService)
                        {
                            theEndPointID = E.EndPointID;
                            break;
                        }
                    }
                    if (theEndPointID == 0)
                    {
                        Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("Could not find the endpoint"));
                        Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("Creating a new one (port={0}, proto={1}, service={2})", port, strProtocol, strService));

                        ENDPOINT newEndPoint = new ENDPOINT();
                        newEndPoint.AssetID = xAsset.AssetID;
                        newEndPoint.ProtocolName = strProtocol;
                        newEndPoint.PortNumber = port;
                        newEndPoint.Service = strService;
                        newEndPoint.SessionID = mySessionID;

                        m_model.ENDPOINT.Add(newEndPoint);
                        m_model.SaveChanges();
                        theEndPointID = newEndPoint.EndPointID;
                    }
                    else
                    {
                        Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("JobID: {0} Endpoint found: {1}", m_jobId, theEndPointID));
                    }

                }
                if (myLines[cpt].Trim() == "Services:")
                {
                    startServices = true;
                    startInformation = false;
                }
                if (myLines[cpt].Trim() == "Information:")
                {
                    //startServices = false;
                    startInformation = true;
                }
            }
            Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("information:"+information));

            Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("JobID: {0} START DUMP STDERR", m_jobId));
            Utils.Helper_Trace("XORCISM PROVIDER SAINT", stderr);
            Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("JobID: {0} END DUMP STDERR", m_jobId));

            // if (sb.ToString().Contains("udp_scan: no response from" + m_target + "; host is offline or UDP is filtered"))
            // throw new Exception("udp_scan: no response from" + m_target + "; host is offline or UDP is filtered");

            


            // =========
            // Command 1 bis
            // =========

            string cmd11;
            if (currentscanner < 1)
            {
                cmd11 = string.Format("sudo sh -c \"cd /usr/share/saint ; chmod -R a+r results\""); //Hardcoded
            }
            else
            {
                cmd11 = string.Format("sudo sh -c \"cd /usr/share/saint"+currentscanner+" ; chmod -R a+r results\"");
            }

            Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("JobID: {0} Executing command [{1}]", m_jobId, cmd11));

            //stdout = exec.RunCommand(cmd11);
            sshShell.WriteLine(cmd11);
            stdout = sshShell.Expect(prompt);
            if (stdout.Contains("password for root"))   //Hardcoded
            {
                sshShell.WriteLine(password);
                stdout = sshShell.Expect(prompt);
            }

            Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("JobID: {0} START DUMP STDOUT", m_jobId));
            Utils.Helper_Trace("XORCISM PROVIDER SAINT", stdout);
            Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("JobID: {0} END DUMP STDOUT", m_jobId));
            /*
            Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("START DUMP STDERR"));
            Utils.Helper_Trace("XORCISM PROVIDER SAINT", stderr);
            Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("END DUMP STDERR"));
            */
            // =========
            // Command 2
            // =========

            string cmd2;
            //cmd2 = string.Format("sudo ./mysaint2.sh {0} ", folder);
            if (currentscanner < 1)
            {
                cmd2 = string.Format("sudo sh -c \"cd /usr/share/saint ; ./bin/saintwriter -c full.cf -d {0} -f 7\"", folder);  //Hardcoded
            }
            else
            {
                cmd2 = string.Format("sudo sh -c \"cd /usr/share/saint"+currentscanner+" ; ./bin/saintwriter -c full.cf -d {0} -f 7\"", folder);
            }

            Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("JobID: {0} Executing command2 [{1}]", m_jobId, cmd2));

            //stdout = exec.RunCommand(cmd2);
            sshShell.WriteLine(cmd2);
            stdout = sshShell.Expect(prompt);
            if (stdout.Contains("password for root"))   //Hardcoded
            {
                sshShell.WriteLine(password);
                stdout = sshShell.Expect(prompt);
            }

            Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("JobID: {0} START DUMP STDOUT", m_jobId));
            Utils.Helper_Trace("XORCISM PROVIDER SAINT", stdout);
            Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("JobID: {0} END DUMP STDOUT", m_jobId));
            /*
            Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("START DUMP STDERR"));
            Utils.Helper_Trace("XORCISM PROVIDER SAINT", stderr);
            Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("END DUMP STDERR"));
            */
            sshShell.Close();
            sshShell = null;

            int index;
            index = stdout.IndexOf("<?xml version=");
            if (index == -1)
            {
                Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("JobID: {0} Report does not contain XML data", m_jobId));
                return false;
            }

            stdout = stdout.Substring(index);
            stdout = stdout.Replace("root@backtrack:~$", "");   //Hardcoded
            stdout = stdout.Replace(prompt + "~#", "");
            stdout = stdout.Replace(prompt + "~$", "");            
            stdout = stdout.Replace(prompt, "");

            try
            {
                Utils.Helper_Trace("XORCISM PROVIDER SAINT", "JobID: "+m_jobId+" Loading the xml document");
                m_data = stdout;
                doc.LoadXml(stdout);
            }
            catch (Exception ex)
            {
                Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("JobID: {0} ExceptionLoadingXML = {1} / {2}", m_jobId, ex.Message, ex.InnerException == null ? "" : ex.InnerException.Message));
                //An error occurred while updating the entries. See the inner exception for details. / Timeout expired.  The timeout period elapsed prior to completion of the operation or the server is not responding.
                return false;
            }
            
            #endregion


            #region WITHOUT SAINT
            /*
            string filename;
            filename = @"I:\XORCISM\sources\XAgentHost\bin\Debug\TEST01\SAINT.XML";

            Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("Using file '{0}'", filename));

            doc.Load(filename);
            */
            #endregion

            try
            {
                Utils.Helper_Trace("XORCISM PROVIDER SAINT", "JobID: " + m_jobId + " Getting vulnerabilities");

                Helper_GetVulnerabilities(doc, m_target, xAsset.AssetID);
            }
            catch (Exception ex)
            {
                Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("JobID: {0} ExceptionGettingVulns = {1} / {2}", m_jobId, ex.Message, ex.InnerException == null ? "" : ex.InnerException.Message));
                                
                return false;
            }

            Utils.Helper_Trace("XORCISM PROVIDER SAINT", "Finished");

            // Finished
            //FREE MEMORY
            doc = null;
            stdout = string.Empty;
            m_model.Dispose();

            return true;
        }
       
        private void Helper_GetProtocolAndPort(XmlNode xn, VulnerabilityEndPoint vulnerabilityEndPoint)
        {
            vulnerabilityEndPoint.Port = -1;
            vulnerabilityEndPoint.Protocol = string.Empty;
            vulnerabilityEndPoint.Service = string.Empty;
            //Service: 8443:TCP
            /*
            Service: ssh
				sent: gzip -V
                received:
                gzip 1.3.12
            */
            string[] tab=xn.InnerText.Trim().Split(new char[]{':'});
            if (tab != null)
            {
                //if (tab[0].Trim().ToUpper() == "Service".ToUpper())
                //  http://en.wikipedia.org/wiki/List_of_TCP_and_UDP_port_numbers
                if (tab[0].Trim().ToUpper() == "SERVICE")
                {
                    try
                    {
                        string myService = string.Empty;
                        string myPort = string.Empty;
                        string myProtocol = string.Empty;
                        //Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("Port = {0}", tab[2]));
                        //Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("Protocole = {0}", tab[2]));
                        myService = tab[1].Split('\n')[0].Trim().ToUpper();
                        vulnerabilityEndPoint.Service = myService;
                        if (tab.Length > 2)
                        {
                            if (tab[2].Trim() != "")
                            {
                                //Service: 8443:TCP
                                myPort = tab[1].Split('\n')[0];
                                vulnerabilityEndPoint.Port = Int32.Parse(myPort);
                                myProtocol = tab[2].Split('\n')[0].Trim(); //TCP
                                vulnerabilityEndPoint.Protocol = myProtocol;
                                myService = "";
                                vulnerabilityEndPoint.Service = "";
                            }
                        }

                        Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("Service = {0}", myService));
                        Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("Port = {0}", myPort));
                        Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("Protocol = {0}", myProtocol));
                    }
                    catch (Exception ex)
                    {
                        Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("JobID: {0} Error in Helper_GetProtocolAndPort : Exception = {1}", m_jobId, ex.Message));
                        Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("xn.InnerText = {0}", xn.InnerText));
                        //vulnerabilityEndPoint.Protocol= tab[1].Split(new char[]{'\n','\t'})[0];
                        vulnerabilityEndPoint.Service = tab[1].Split(new char[] { '\n', '\t' })[0].Trim().ToUpper();
                        Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("Service={0}", vulnerabilityEndPoint.Service));
                        /*
                        if (vulnerabilityEndPoint.Service == "HTTP")
                        {
                            vulnerabilityEndPoint.Service = "WWW";
                        }
                        if (vulnerabilityEndPoint.Service == "HTTPS")
                        {
                            vulnerabilityEndPoint.Service = "WWW (SECURE)";
                        }
                        */
                    }
                }
            }
            
            // protocol = xn.InnerText.Split(new char[] { ':' })[1].Trim().Split(new char[] { ' ' })[0].Split(new char[] { '\n', '\t', '\r' })[0];
        }

        private void Helper_GetVulnerabilities(XmlDocument s, string ipadress, int assetid)
        {
            Utils.Helper_Trace("XORCISM PROVIDER SAINT", "JobID = " + m_jobId + " in Helper_GetVulnerabilities");
            //List<VULNERABILITYFOUND> list_vulnerabilyFound;
            //list_vulnerabilyFound = new List<VULNERABILITYFOUND>();
           
            string query = "/report/details/host_info"; //Hardcoded

            XmlNode hostInfoNode;
            hostInfoNode = s.SelectSingleNode(query);

            bool PatchUpgrade = false;
            string title="";
            string MSPatch = "";
            string Solution;

            ASSETINFORMATION AssetInfo = new ASSETINFORMATION();
            AssetInfo.JobID = m_jobId;

            foreach (XmlNode n in hostInfoNode.ChildNodes)
            {                
                if (n.Name.ToUpper() == "HOSTNAME")
                {
                    Utils.Helper_Trace("XORCISM PROVIDER SAINT", "hostname = " + n.InnerText.Trim());
                    AssetInfo.hostname = n.InnerText.Trim();
                }
                if (n.Name.ToUpper() == "HOSTTYPE")
                {
                    Utils.Helper_Trace("XORCISM PROVIDER SAINT", "OS = " + n.InnerText.Trim());
                    AssetInfo.hosttype = n.InnerText.Trim();
                }
                if (n.Name.ToUpper() == "NETBIOS")
                {
                    Utils.Helper_Trace("XORCISM PROVIDER SAINT", "NETBIOS = " + n.InnerText.Trim());
                    AssetInfo.netbios = n.InnerText.Trim();
                }

                
                //if (n.Name.ToUpper() == "vulnerability".ToUpper())
                if (n.Name.ToUpper() == "VULNERABILITY")
                {
                    VulnerabilityFound vulnerabilityFound = new VulnerabilityFound();
                    VulnerabilityEndPoint vulnerabilityEndPoint = new VulnerabilityEndPoint();

                    vulnerabilityFound.InnerXml = n.OuterXml;

                    vulnerabilityEndPoint.IpAdress = m_target;

                    PatchUpgrade = false;
                    MSPatch = "";

                    foreach (XmlNode xn in n.ChildNodes)
                    {
                        if (xn.Name.ToUpper() == "severity".ToUpper())
                           vulnerabilityFound.Severity = xn.InnerText.Trim();

                        if (xn.Name.ToUpper() == "impact".ToUpper())
                            vulnerabilityFound.Consequence = xn.InnerText.Trim();

                        if (xn.Name.ToUpper() == "resolution".ToUpper())
                        {
                            vulnerabilityFound.Solution = xn.InnerText.Trim();

                            Solution = xn.InnerText.Trim();
                            if (Solution.Contains(" upgrade to "))
                            {
                                PatchUpgrade = true;
                            }
                            if (Solution.Contains(" upgraded to ")) //should be 
                            {
                                PatchUpgrade = true;
                            }
                            if (Solution.Contains("Upgrade "))
                            {
                                PatchUpgrade = true;
                            }
                            if (Solution.Contains("install the needed updates"))
                            {
                                PatchUpgrade = true;
                            }
                            if (Solution.Contains("apply one of the fixes"))
                            {
                                PatchUpgrade = true;
                            }
                            if (Solution.Contains("apply the patch"))
                            {
                                PatchUpgrade = true;
                            }
                        }

                        if (xn.Name.ToUpper() == "description".ToUpper())
                        {
                            string mydescription = xn.InnerText.Trim();
                            title = xn.InnerText.Trim();
                            vulnerabilityFound.Description = title;
                            //Cross-site scripting vulnerability in query parameter to /search.jsp
                            if (title.Contains("Cross-site scripting vulnerability in "))
                            {
                                //VulnerableParameter
                                string pattern = "Cross-site scripting vulnerability in (.*?) parameter to ";
                                MatchCollection matches = Regex.Matches(title, pattern);
                                foreach (Match match in matches)
                                {
                                    vulnerabilityFound.VulnerableParameter = match.Value.Replace("Cross-site scripting vulnerability in ", "").Replace(" parameter to ", "");
                                    vulnerabilityFound.VulnerableParameterType = "Post";    //TODO: à voir
                                    Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("Vulnerable parameter = {0}", match.Value.Replace("Cross-site scripting vulnerability in ", "").Replace(" parameter to ", "")));
                                }
                                pattern = " parameter to /(.*?)";
                                matches = Regex.Matches(title, pattern);
                                foreach (Match match in matches)
                                {
                                    string vulnurl=m_target + match.Value.Replace(" parameter to ", "");
                                    vulnerabilityFound.Url = vulnurl.Replace("//","/");
                                    Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("Vulnerable url = {0}", vulnurl.Replace("//", "/")));
                                }
                            }

                            vulnerabilityFound.Title = title;
                            
                            Regex objNaturalPattern = new Regex("MS[0-9][0-9]-[0-9][0-9][0-9]");
                            MSPatch = objNaturalPattern.Match(title).ToString();
                            if (MSPatch != "")
                            {
                                Utils.Helper_Trace("XORCISM PROVIDER SAINT", "MSPatch=" + MSPatch);
                                PatchUpgrade = true;                                
                            }
                        }

                        if (xn.Name.ToUpper() == "cve".ToUpper())
                            vulnerabilityFound.ListItem = splitCVE(xn.InnerText);

                        if (xn.Name.ToUpper() == "vuln_details".ToUpper())
                            Helper_GetProtocolAndPort(xn, vulnerabilityEndPoint);
                        /*
                        try
                        {
                            if (xn.Name.ToUpper() == "cvss_base_score".ToUpper())
                                vulnerabilityFound.CVSSBaseScore = float.Parse(xn.InnerText.Trim());
                        }
                        catch (Exception ex)
                        {
                            Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("Exception cvss_base_score = {0} {1}", xn.InnerText, ex));
                        }
                        if (xn.Name.ToUpper() == "exploit_available".ToUpper())
                        {
                            if (xn.InnerText == "yes")
                            {
                                
                            }
                        }
                        if (xn.Name.ToUpper() == "pci_compliant_v".ToUpper())
                        {
                            if (xn.InnerText == "PASS")
                            {
                                vulnerabilityFound.PCI_FLAG = true;
                            }
                            if (xn.InnerText == "FAIL")
                            {
                                vulnerabilityFound.PCI_FLAG = false;
                            }
                        }
                        */
                        //cvss_base_vector
                        //exploit_available
                        //cpe
                        //bid
                        /*
                        <exploit_available>no</exploit_available>
			            <cvss_base_score>0.0</cvss_base_score>
			            <cvss_base_vector>(AV:L/AC:L/Au:N/C:N/I:N/A:N)</cvss_base_vector>
			            <cpe>cpe:/o:linux:linux_kernel cpe:/o:apple:mac_os cpe:/o:ibm:aix cpe:/o:hp:tru64 cpe:/o:ibm:os2 cpe:/o:novell:netware cpe:/o:apple:mac_os_x cpe:/o:hp:hp-ux cpe:/o:windriver:bsdos</cpe>
			            <osvdb>95</osvdb>
                        <bid> 1924 3440 3445 5711 5712 5713 14259</bid>
			            <confirmed>confirmed</confirmed>
			            <service>icmp</service>
			            <pci_compliant_v>PASS</pci_compliant_v>
			            <pci_severity>low</pci_severity>
                        
                        <confirmed>inferred</confirmed>
			            <service>1433:TCP</service>
			            <pci_compliant_v>FAIL</pci_compliant_v>
                        */
                    }

                    vulnerabilityFound.PatchUpgrade = PatchUpgrade;
                    vulnerabilityFound.MSPatch = MSPatch;


                    //Searching more details
                    string query2 = "/report/overview/vulnerabilities";

                    XmlNode hostInfoNode2;
                    hostInfoNode2 = s.SelectSingleNode(query2);
                    bool itisthegoodvuln = false;
                    foreach (XmlNode n2 in hostInfoNode2.ChildNodes)
                    {

                        //if (n.Name.ToUpper() == "vulnerability".ToUpper())
                        if (n2.Name.ToUpper() == "VULNERABILITY")
                        {
                            //vulnerabilityFound.InnerXml = n2.OuterXml;

                         //   vulnerabilityEndPoint.IpAdress = m_target;

                         //   PatchUpgrade = false;
                         //   MSPatch = "";

                            foreach (XmlNode xn in n2.ChildNodes)
                            {
                                //if (xn.Name.ToUpper() == "severity".ToUpper())
                                //    vulnerabilityFound.Severity = xn.InnerText.Trim();

                                //if (xn.Name.ToUpper() == "impact".ToUpper())
                                //    vulnerabilityFound.Consequence = xn.InnerText.Trim();
                                /*
                                if (xn.Name.ToUpper() == "resolution".ToUpper())
                                {
                                //    vulnerabilityFound.Solution = xn.InnerText.Trim();

                                    Solution = xn.InnerText.Trim();
                                    if (Solution.Contains(" upgrade to "))
                                    {
                                        PatchUpgrade = true;
                                    }
                                    if (Solution.Contains(" upgraded to ")) //should be 
                                    {
                                        PatchUpgrade = true;
                                    }
                                    if (Solution.Contains("Upgrade "))
                                    {
                                        PatchUpgrade = true;
                                    }
                                    if (Solution.Contains("install the needed updates"))
                                    {
                                        PatchUpgrade = true;
                                    }
                                    if (Solution.Contains("apply one of the fixes"))
                                    {
                                        PatchUpgrade = true;
                                    }
                                    if (Solution.Contains("apply the patch"))
                                    {
                                        PatchUpgrade = true;
                                    }
                                }
                                */
                                if (xn.Name.ToUpper() == "description".ToUpper())
                                {
                                //    vulnerabilityFound.Description = xn.InnerText.Trim();

                                    //title = xn.InnerText.Trim();
                                    if(title == xn.InnerText.Trim())
                                    {
                                        itisthegoodvuln=true;
                                    }
                                    /*
                                    Regex objNaturalPattern = new Regex("MS[0-9][0-9]-[0-9][0-9][0-9]");
                                    MSPatch = objNaturalPattern.Match(title).ToString();
                                    if (MSPatch != "")
                                    {
                                        Utils.Helper_Trace("XORCISM PROVIDER SAINT", "MSPatch=" + MSPatch);
                                        PatchUpgrade = true;
                                    }
                                    */
                                }

                                //if (xn.Name.ToUpper() == "cve".ToUpper())
                                //    vulnerabilityFound.ListItem = splitCVE(xn.InnerText);

                                if (xn.Name.ToUpper() == "vuln_details".ToUpper())
                                {
                                    vulnerabilityFound.Result = xn.InnerText.Trim();
                                    vulnerabilityFound.rawresponse = xn.InnerText.Trim();
                                }
                                //    Helper_GetProtocolAndPort(xn, vulnerabilityEndPoint);
                                if (itisthegoodvuln)
                                {
                                    try
                                    {
                                        if (xn.Name.ToUpper() == "cvss_base_score".ToUpper())
                                        {
                                            vulnerabilityFound.CVSSBaseScore = float.Parse(xn.InnerText.Trim());    //.Replace(".",","));
                                            Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("cvss_base_score = {0}", vulnerabilityFound.CVSSBaseScore));
                                            if (vulnerabilityFound.CVSSBaseScore > 10)
                                            {
                                                vulnerabilityFound.CVSSBaseScore = vulnerabilityFound.CVSSBaseScore / 10;
                                                Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("cvss_base_score corrected = {0}", vulnerabilityFound.CVSSBaseScore));
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("Exception cvss_base_score = {0} {1}", xn.InnerText, ex));
                                    }
                                    if (xn.Name.ToUpper() == "exploit_available".ToUpper())
                                    {
                                        if (xn.InnerText == "yes")
                                        {
                                            vulnerabilityFound.Exploitable = true;
                                        }
                                    }
                                    if (xn.Name.ToUpper() == "pci_compliant_v".ToUpper())
                                    {
                                        if (xn.InnerText == "PASS")
                                        {
                                            vulnerabilityFound.PCI_FLAG = true;
                                        }
                                        if (xn.InnerText == "FAIL")
                                        {
                                            vulnerabilityFound.PCI_FLAG = false;
                                        }
                                    }

                                    //cvss_base_vector
                                    //exploit_available
                                    //cpe
                                    //bid
                                    /*
                                    <exploit_available>no</exploit_available>
			                        <cvss_base_score>0.0</cvss_base_score>
			                        <cvss_base_vector>(AV:L/AC:L/Au:N/C:N/I:N/A:N)</cvss_base_vector>
			                        <cpe>cpe:/o:linux:linux_kernel cpe:/o:apple:mac_os cpe:/o:ibm:aix cpe:/o:hp:tru64 cpe:/o:ibm:os2 cpe:/o:novell:netware cpe:/o:apple:mac_os_x cpe:/o:hp:hp-ux cpe:/o:windriver:bsdos</cpe>
			                        <osvdb>95</osvdb>
                                    <bid> 1924 3440 3445 5711 5712 5713 14259</bid>
			                        <confirmed>confirmed</confirmed>
			                        <service>icmp</service>
			                        <pci_compliant_v>PASS</pci_compliant_v>
			                        <pci_severity>low</pci_severity>
                        
                                    <confirmed>inferred</confirmed>
			                        <service>1433:TCP</service>
			                        <pci_compliant_v>FAIL</pci_compliant_v>
                                    */
                                }
                            }

                            vulnerabilityFound.PatchUpgrade = PatchUpgrade;
                            vulnerabilityFound.MSPatch = MSPatch;              
                        }
                    }


                    //More information for PCI-DSS : TODO A FAIRE
                    /*
                    </vulnerabilities>
                    <special_notes_list>
		                <host>
			                <ipaddr>68.233.193.132</ipaddr>
			                <pci_note>Due to increased risk to the cardholder data environment when remote access software is present, please 1) justify the business need for this software to the ASV and 2) confirm it is either implemented securely per Appendix C or disabled/ removed. Please consult your ASV if you have questions about this Special Note.</pci_note>
			                <pci_noted_item>remote access ports: 3389,</pci_noted_item>
		                </host>
	                </special_notes_list>
                    */


                    
                    // VulnerabilityPersistor.Persist(vulnerabilityDetail, vulnerabilityEndPoint, m_jobId,"saint",m_model, out MyVulnList);
                    VulnerabilityPersistor.Persist(vulnerabilityFound, vulnerabilityEndPoint, m_jobId, "saint", m_model);

                    // list_vulnerabilyFound.AddRange(MyVulnList);                    
                }                    
            }
            m_model.ASSETINFORMATION.Add(AssetInfo);
            m_model.SaveChanges();

            //FREE MEMORY
            m_model.Dispose();

        }

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
        private List<FINDING> Helper_PersistVulnerability(VulnerabilityFound detail, VulnerabilityEndPoint endpoint)
        {
            
            /*
            // ===============================================
            // If necessary, creates an EndPoint and get its id
            // ===============================================

            int theEndPointID = 0;

            Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("Searching for an asset with IP address {0}", endpoint.IpAdress));

            var asset = from Assets in m_model.ASSET
                        where Assets.IpAdress == endpoint.IpAdress
                        select Assets;
            ASSET myAsset = asset.ToList().FirstOrDefault();

            Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("Retrieving the endpoints"));

            ENDPOINT endPoint = new ENDPOINT();
            var EP = from Epoint in m_model.ENDPOINT
                     where Epoint.AssetID == myAsset.AssetID
                     select Epoint;

            Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("Found {0} enpoints", EP.ToList().Count));

            if (EP.ToList().Count == 0)
            {
                Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("No endpoint found, creating a new one (port={0}, proto={1})", endpoint.Port, endpoint.Protocol));

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
                Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("Looking for the right endpoint"));

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
                    Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("Coulnd not find the endpoint"));

                    Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("Creating a new one (port={0}, proto={1})", endpoint.Port, endpoint.Protocol));

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
                Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("Searching for a Saint entry in VULNERABILITYSYNONYM"));

                var syn1 = from S in m_model.VULNERABILITYSYNONYM
                           where S.Referential.Equals("saint") &&
                           S.Value == detail.InnerXml
                           select S;

                VULNERABILITYSYNONYM VS;

                if (syn1.Count() == 0)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("No entry found, creating a new Saint entry in VULNERABILITYSYNONYM"));

                    VS = new VULNERABILITYSYNONYM();
                    VS.Referential = "saint";
                    VS.Value = detail.InnerXml;
                    VS.Description = detail.Description;
                    VS.Consequence = detail.Consequence;
                    VS.Solution = detail.Solution;
                    // VS.Severity     = detail.Severity;

                    m_model.AddToVULNERABILITYSYNONYM(VS);

               //     Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("Coucou 33"));

                    m_model.SaveChanges();

                    VS.Parent = VS.ID;

                    m_model.SaveChanges();

               //     Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("Coucou 44"));
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

            //    Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("Coucou 55"));

                list.Add(MyVuln);
            }

            // ====================================
            // In case there are at least one CVEID
            // ====================================

            foreach (VulnerabilityFound.Item item in detail.ListItem)
            {
                Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("Handling CVE {0}", item.Value));

                // ========================================
                // Search VULNERABILITYSYNONYM for this CVE
                // ========================================

                 var syn = from S in m_model.VULNERABILITYSYNONYM
                          where S.Referential.Equals("cve") &&
                          S.Value.Equals(item.Value)
                          select S;
				  
                if (syn.Count() == 0)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("Error : CVEID {0} not found in VULNERABILITYSYNONYM", item.Value));
                    continue;
                }

                Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("This CVE matches to ID {0} in table VULNERABILITYSYNONYM", syn.ToList().First().ID));

                Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("Creating a new entry in table VULNERABILITYFOUND"));

                VULNERABILITYFOUND MyVuln = new VULNERABILITYFOUND();
                MyVuln.VulnerabilityID  = syn.ToList().First().ID;
                MyVuln.EndPointID       = theEndPointID;

                m_model.AddToVULNERABILITYFOUND(MyVuln);
                m_model.SaveChanges();

                list.Add(MyVuln);

                // ===============================================================
                // If necessary, create an entry in VULNERABILITYSYNONYM for Saint
                // ===============================================================

                Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("Searching for a Saint entry in VULNERABILITYSYNONYM"));

                var syn1 = from S in m_model.VULNERABILITYSYNONYM
                           where S.Referential.Equals("saint") &&
                           S.Value == detail.InnerXml
                           select S;
                if (syn1.Count() != 0)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("Entry found !!!!!!!!!!!!!!!!!"));


                    //if(syn1.ToList().First().Parent != MyVuln.VulnerabilityID)
                    //{
                    //    Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("GROS BUG"));
                    //    continue;
                    //}                      
                }
                else
                {
                    Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("No entry found, creating a new Saint entry in VULNERABILITYSYNONYM"));

                    VULNERABILITYSYNONYM VS = new VULNERABILITYSYNONYM();
                    VS.Referential  = "saint";
                    VS.Value        = detail.InnerXml;
                    VS.Description  = detail.Description;
                    VS.Consequence  = detail.Consequence;
                    VS.Solution     = detail.Solution;
                    VS.Parent       = MyVuln.VulnerabilityID;

                    MyVuln.Severity = detail.Severity;

                    m_model.AddToVULNERABILITYSYNONYM(VS);

                //    Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("Coucou 1"));

                    m_model.SaveChanges();

                //    Utils.Helper_Trace("XORCISM PROVIDER SAINT", string.Format("Coucou 2"));
                }
            }
            return list;
            */

            return null;
        }
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

    class _SaintSettings
    {
        #region  Properties
        public string SaintLogin { get { return m_saintLogin; } set { m_saintLogin = value; } }
        public string SaintPassword { get { return m_saintPassword; } set { m_saintPassword = value; } }
        public string SaintIP { get { return m_saintIP; } set { m_saintIP = value; } }
        public string OSLogin { get { return m_windowsLogin; } set { m_windowsLogin = value; } }
        public string OSPassword { get { return m_windowsPassword; } set { m_windowsPassword = value; } }
        #endregion

        #region Privates Members
        private string m_saintLogin;
        private string m_saintPassword;
        private string m_saintIP;
        private string m_windowsLogin;
        private string m_windowsPassword;
        #endregion

        #region static Members
        static _SaintSettings instance;
        #endregion

        private _SaintSettings()
        {
        //    m_saintLogin = "saint";
        //    m_saintPassword = "SAINT!!!";
        //    m_saintIP = "1.2.3.4";
            /*
            m_saintIP = ConfigurationManager.AppSettings["SAINT_IP"];
            m_saintLogin = ConfigurationManager.AppSettings["SAINT_LOGIN"];
            m_saintPassword = ConfigurationManager.AppSettings["SAINT_PASSWORD"];
            */
        }
        public static _SaintSettings getInstance()
        {
            if (instance == null)
                instance = new _SaintSettings();
            return instance;
        }   
    }
    
}
