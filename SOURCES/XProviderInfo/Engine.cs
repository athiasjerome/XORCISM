using System;
using System.Net;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net.Sockets;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Configuration;

using XORCISMModel;
using XCommon;
using XProviderCommon;

using System.Text.RegularExpressions;


//using FSM.DotNetSSH;

using System.Threading;


namespace XProviderInfo
{
    //TODO-DEBUG remove
    //public class program : Engine
    //{
        /* Partie à virer par la suite le run est independant ^^ */
        /*
        static void Main(string[] args)
        {
            Engine eng = new Engine();
            eng.Run("www.google.fr", 1, "1", "nop");
        }
        */
        /* Fin partie à virer par la suite */
    //}
    /// <summary>
    /// Copyright (C) 2012-2015 Jerome Athias
    /// Incomplete (demo) for OSINT. Add whatever you want, Shodan, censys, etc., etc.
    /// All trademarks and registered trademarks are the property of their respective owners.
    /// This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation; either version 2 of the License, or (at your option) any later version.
    /// 
    /// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
    /// 
    /// You should have received a copy of the GNU General Public License along with this program; if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
    /// </summary>
    public class Engine : MarshalByRefObject, XProviderCommon.IVulnerabilityDetector
    {
        private string m_data = string.Empty;
        private static int port;
        private static string address, username, password;
        //private static string prompt = "root@backtrack:";  //TODO Hardcoded
        //private static string prompt1 = prompt + "~#";
        //private static string promptend = "#";
        private static string prompt = "root";//@backtrack:";   //TODO Hardcoded
        private static string prompt1 = prompt;// + "~$";
        private static string promptend = "$";
        private int m_jobId;
        private string m_target;
        private XORCISMEntities m_model;

        public Engine()
        {
            TextWriterTraceListener tw;
            tw = new TextWriterTraceListener("XProviderInfo.log");  //Hardcoded
            Trace.Listeners.Add(tw);
            Trace.AutoFlush = true;
            Trace.IndentSize = 4;

            m_model = new XORCISMEntities();
        }

        public override object InitializeLifetimeService()
        {
            return null; //Allow infinite lifetime
        }

        //TODO
        //https://isc.sans.edu/ipinfo.html?ip=

        static SshShell connect()
        {
            port = 22;
            address = "192.168.1.2";    //TODO Hardcoded
                username = "root";
                password = "toor";

            SshShell sshShell;
            sshShell = new SshShell(address, username, password);
            sshShell.RemoveTerminalEmulationCharacters = true;

            //string prompt;
            //prompt = "root@backtrack:~#";

            //exec.Connect(address);
            //exec.Login(username, password);
            try
            {
                sshShell.Connect(port);                
                string stdout=sshShell.Expect(prompt);
                //Utils.Helper_Trace("XORCISM PROVIDER XINFO", "stdoutconnect: " + stdout);
            }
            catch (Exception ex)
            {
                Utils.Helper_Trace("XORCISM PROVIDER XINFO", "Error in sshconnect to "+address+" "+ex.Message+" "+ex.InnerException);
            }

            Console.WriteLine("connected");

            string cmd1 = "cd /home/root/tools/metasploitsvn";  //TODO Hardcoded

            sshShell.WriteLine(cmd1);
            //prompt1 = prompt + "~/tools/metasploitsvn" + promptend;
            prompt1 = "/tools/metasploitsvn";   //HARDCODED
            sshShell.Expect(prompt1);

            //==============================================

            cmd1 = "svn update";    //Hardcoded

            sshShell.WriteLine(cmd1);
            Thread.Sleep(60000);
            sshShell.Expect(prompt1);

            return sshShell;
        }

        
        public void Run(string target, int jobID, string policy, string strategy)
        {
            m_jobId = jobID;
            m_target = target;
            Utils.Helper_Trace("XORCISM PROVIDER XINFO", "Entering Run()");
            Utils.Helper_Trace("XORCISM PROVIDER XINFO", string.Format("Target = {0} , JobID = {1} , Policy = {2}", target, jobID, policy));

            //Check if we have an IP address
                //string pattern = @"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\.
                //([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$";
                string pattern = @"^\d\d?\d?\.\d\d?\d?\.\d\d?\d?\.\d\d?\d?$";   //TODO IPv6...
                //create our Regular Expression object
                Regex check = new Regex(pattern);

                if (check.IsMatch(target.Trim(), 0))
                {
                    Utils.Helper_Trace("XORCISM PROVIDER XINFO", "JobID:" + jobID + " target is an IP address");
                }
                else
                {
                    try
                    {
                        //It should be a domain name
                        Utils.Helper_Trace("XORCISM PROVIDER XINFO", "JobID:" + jobID + " target: " + target + " is not an IP address");
                        // = target.Replace("http://", "");
                        //target = target.Replace("https://", "");
                        //target = target.Replace("/", "");
                        if (!target.Contains("://"))
                            target = "http://" + target;
                        //TODO? HTTPS
                        target = new Uri(target).Host;
                        Utils.Helper_Trace("XORCISM PROVIDER XINFO", "JobID:" + jobID + " targetmodified: " + target);
                        if (check.IsMatch(target.Trim(), 0))
                        {
                            Utils.Helper_Trace("XORCISM PROVIDER XINFO", "JobID:" + jobID + " targetmodified is an IP address");
                        }
                        else
                        {
                            //crackme.cenzic.com
                            string[] sectons = target.Split(new char[] { '.' });

                            if (sectons.Length == 3)
                            {
                                //target = string.Join(".", sectons, 1, 2);
                                whois_info(string.Join(".", sectons, 1, 2));
                                Thread.Sleep(30000);    //Hardcoded
                                search_email_collector(string.Join(".", sectons, 1, 2));
                            }
                            else
                            {
                                whois_info(target);
                                Thread.Sleep(30000);    //Hardcoded
                                search_email_collector(target);
                            }

                            IPHostEntry ipEntry = Dns.GetHostEntry(target);
                            IPAddress[] addr = ipEntry.AddressList;
                            target = addr[0].ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        Utils.Helper_Trace("XORCISM PROVIDER XINFO", string.Format("JobID:" + jobID + " Dns.GetHostEntry Exception = {0} / {1}", ex.Message, ex.InnerException == null ? "" : ex.InnerException.Message));
                    }
                }

            


            XINFOParser infoParser = null;
            try
            {
                infoParser = new XINFOParser(target, jobID, policy, strategy);
            }
            catch (Exception ex)
            {
                Utils.Helper_Trace("XORCISM PROVIDER XINFO", "JobID:" + jobID + "Exception Parser = " + ex.Message + " " + ex.InnerException);
            }

            string status = XCommon.STATUS.FINISHED.ToString();


            // =================================================
            // Change the status of the job to FINISHED or ERROR
            // =================================================

            if (infoParser.Parse() == false)
            {
                status = XCommon.STATUS.ERROR.ToString();
                Utils.Helper_Trace("XORCISM PROVIDER XINFO", string.Format("Updating job {0} status to ERROR", jobID));
                XCommon.Utils.Helper_SendEmail("contact@hackenaton.org", "XINFO ERROR", "XINFO ERROR for job:" + jobID);
            }
            else
            {
                Utils.Helper_Trace("XORCISM PROVIDER XINFO", string.Format("Updating job {0} status to FINISHED", jobID));
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
                  infoParser = null;
            }
            catch (Exception ex)
            {
                Utils.Helper_Trace("XORCISM PROVIDER XINFO", "JobID:" + jobID + "Exception UpdateJob = " + ex.Message + " " + ex.InnerException);
            }

            Utils.Helper_Trace("XORCISM PROVIDER XINFO", "JobID:" + jobID + "Leaving Run()");
        }

        public void whois_info(string domainname)
        {
            Utils.Helper_Trace("XORCISM PROVIDER XINFO", "JobID:" + m_jobId + " whois on : " + domainname);
        //            http://reports.internic.net/cgi/whois?whois_nic=xorcism.org&type=domain
            TcpClient tcpWhois=null;
            NetworkStream nsWhois;
            BufferedStream bfWhois=null;
            StreamWriter swSend;
            StreamReader srReceive;

            try
            {
                // The TcpClient should connect to the who-is server, on port 43 (default who-is)
                tcpWhois = new TcpClient("whois.internic.net", 43); //Hardcoded
                // Set up the network stream
                nsWhois = tcpWhois.GetStream();
                // Hook up the buffered stream to the network stream
                bfWhois = new BufferedStream(nsWhois);
            }
            catch(Exception ex)
            {
                Utils.Helper_Trace("XORCISM PROVIDER XINFO", "JobID:" + m_jobId + " WhoisError: Could not open a connection to the Who-Is server. " + ex.Message + " " + ex.InnerException);
            }

            // Send to the server the hostname that we want to get information on
            swSend = new StreamWriter(bfWhois);
            swSend.WriteLine(domainname);
            swSend.Flush();
            
            //TODO
            /*
            try
            {
                srReceive = new StreamReader(bfWhois);
                string strResponse;
                // Read the response line by line
                //TODO: Review Update
                while ((strResponse = srReceive.ReadLine()) != null)
                {
                    //txtResponse.Text += strResponse + "\r\n";
                    if (strResponse.Contains("Domain Name:"))
                    {
                        INFORMATION newInfo = new INFORMATION();
                        newInfo.Title = "Domain Name";
                        newInfo.Result = strResponse;
                        newInfo.JobID = m_jobId;
                        m_model.INFORMATION.Add(newInfo);
                        m_model.SaveChanges();
                    }
                    if (strResponse.Contains("Registrar:"))
                    {
                        INFORMATION newInfo = new INFORMATION();
                        newInfo.Title = "Registrar";
                        newInfo.Result = strResponse;
                        newInfo.JobID = m_jobId;
                        m_model.INFORMATION.Add(newInfo);
                        m_model.SaveChanges();
                    }
                    if (strResponse.Contains("Whois Server:"))
                    {
                        INFORMATION newInfo = new INFORMATION();
                        newInfo.Title = "Whois Server";
                        newInfo.Result = strResponse;
                        newInfo.JobID = m_jobId;
                        m_model.INFORMATION.Add(newInfo);
                        m_model.SaveChanges();
                    }
                    if (strResponse.Contains("Referral URL:"))
                    {
                        INFORMATION newInfo = new INFORMATION();
                        newInfo.Title = "Referral URL";
                        newInfo.Result = strResponse;
                        newInfo.JobID = m_jobId;
                        m_model.AddToINFORMATION(newInfo);
                        m_model.SaveChanges();
                    }
                    if (strResponse.Contains("Name Server:"))
                    {
                        INFORMATION newInfo = new INFORMATION();
                        newInfo.Title = "Name Server";
                        newInfo.Result = strResponse;
                        newInfo.JobID = m_jobId;
                        m_model.AddToINFORMATION(newInfo);
                        m_model.SaveChanges();
                    }
                    if (strResponse.Contains("Status:"))
                    {
                        INFORMATION newInfo = new INFORMATION();
                        newInfo.Title = "Status";
                        newInfo.Result = strResponse;
                        newInfo.JobID = m_jobId;
                        m_model.AddToINFORMATION(newInfo);
                        m_model.SaveChanges();
                    }
                    if (strResponse.Contains("Updated Date:"))
                    {
                        INFORMATION newInfo = new INFORMATION();
                        newInfo.Title = "Updated Date";
                        newInfo.Result = strResponse;
                        newInfo.JobID = m_jobId;
                        m_model.AddToINFORMATION(newInfo);
                        m_model.SaveChanges();
                    }
                    if (strResponse.Contains("Creation Date:"))
                    {
                        INFORMATION newInfo = new INFORMATION();
                        newInfo.Title = "Creation Date";
                        newInfo.Result = strResponse;
                        newInfo.JobID = m_jobId;
                        m_model.AddToINFORMATION(newInfo);
                        m_model.SaveChanges();
                    }
                    if (strResponse.Contains("Expiration Date:"))
                    {
                        INFORMATION newInfo = new INFORMATION();
                        newInfo.Title = "Expiration Date";
                        newInfo.Result = strResponse;
                        newInfo.JobID = m_jobId;
                        m_model.AddToINFORMATION(newInfo);
                        m_model.SaveChanges();
                    }
                }
            }
            catch(Exception ex)
            {
                Utils.Helper_Trace("XORCISM PROVIDER XINFO", "JobID:" + m_jobId + " WhoisError: Could not read data from the Who-Is server. " + ex.Message + " " + ex.InnerException);
            }
            */

            // We're done with the connection
            tcpWhois.Close();
        }


        public void esearchy(string domainname)
        {
            //http://www.securityaegis.com/esearchy-my-new-favorite-osint-script/
            Utils.Helper_Trace("XORCISM PROVIDER XINFO", "JobID:" + m_jobId + " esearchy on : " + domainname);                   

            //TODO
        }

        public void search_email_collector(string domainname)
        {
            Utils.Helper_Trace("XORCISM PROVIDER XINFO", "JobID:" + m_jobId + " search_email_collector on : " + domainname);

            SshShell sshShell = connect();

            string stdout = "";
            //string stderr = "";

            string cmd1 = "cd /home/root/tools/metasploitsvn";  //TODO Hardcoded
            //prompt1 = prompt + "~/tools/metasploitsvn" + promptend;

            sshShell.WriteLine(cmd1);
            Thread.Sleep(2000);
            stdout = sshShell.Expect(prompt);
            Utils.Helper_Trace("XORCISM PROVIDER XINFO", "JobID:" + m_jobId + " stdout001: " + stdout);
            //TODO Hardcoded
            cmd1 = "./msfcli auxiliary/gather/search_email_collector DOMAIN=" + domainname + " E";  //Hardcoded

            sshShell.WriteLine(cmd1);
            Thread.Sleep(60000);
            stdout = sshShell.Expect(prompt);

            /*
            DOMAIN => target.com
            [*] Harvesting emails .....
            [*] Searching Google for email addresses from target.com
            [*] Extracting emails from Google search results...
            [*] Searching Bing email addresses from target.com
            [*] Extracting emails from Bing search results...
            [*] Searching Yahoo for email addresses from target.com
            [*] Extracting emails from Yahoo search results...
            [*] Located 2 email addresses for target.com
            [*]     contact@target.com
            [*]     boss@target.com
            [*] Auxiliary module execution completed
            */

            XORCISMEntities model = new XORCISMEntities();            

            string[] mytab = Regex.Split(stdout, "\r\n");
            string mymail = "";
            foreach (string line in mytab)
            {
                if (line.Contains("@" + domainname))
                {
                    mymail = line.Replace("[*]", "").Trim();
                    Console.WriteLine(mymail);

                    //TODO
                    /*
                    //Check if we already collected this email
                    var Q = from e in model.INFORMATION
                            where e.JobID == m_jobId && e.Title == "Email" && e.Result == mymail
                            select e;

                    INFORMATION newInfo = Q.FirstOrDefault();

                    if (newInfo == null)
                    {
                        Utils.Helper_Trace("XORCISM PROVIDER XINFO", "JobID:" + m_jobId + "Email collected: " + mymail);

                        newInfo = new INFORMATION();
                        newInfo.Title = "Email";
                        newInfo.Result = mymail;
                        newInfo.JobID = m_jobId;
                        m_model.AddToINFORMATION(newInfo);
                        m_model.SaveChanges();
                    }
                    */

                }
            }


            Utils.Helper_Trace("XORCISM PROVIDER XINFO", "JobID:" + m_jobId + " esearchy on : " + domainname);
            cmd1="esearchy -q \"@"+ domainname+"\" --enable-all --disable-bing --disable-yahoo -m 500 -w "+m_target;    //company   //HARDCODED
            //TODO Review

            Utils.Helper_Trace("XORCISM PROVIDER XINFO", "JobID:" + m_jobId + " esearchy command : " + cmd1);

            sshShell.WriteLine(cmd1);
            Thread.Sleep(60000);
            stdout = sshShell.Expect(prompt);

            Utils.Helper_Trace("XORCISM PROVIDER XINFO", "JobID:" + m_jobId + "esearchy stdout: " + stdout);

            mytab = Regex.Split(stdout, "\r\n");
            mymail = "";
            foreach (string line in mytab)
            {
                //TODO
                /*
                if (line.ToLower().EndsWith("@" + domainname.ToLower()))
                {
                    mymail = line.Trim();
                    Console.WriteLine(mymail);

                    //Check if we already collected this email
                    var Q = from e in model.INFORMATION
                            where e.JobID == m_jobId && e.Title == "Email" && e.Result == mymail
                            select e;

                    INFORMATION newInfo = Q.FirstOrDefault();

                    if (newInfo == null)
                    {
                        Utils.Helper_Trace("XORCISM PROVIDER XINFO", "JobID:" + m_jobId + "Email collected: " + mymail);
                        newInfo = new INFORMATION();
                        newInfo.Title = "Email";
                        newInfo.Result = mymail;
                        newInfo.JobID = m_jobId;
                        m_model.AddToINFORMATION(newInfo);
                        m_model.SaveChanges();
                    }
                }
                else
                {
                    
                    //Kerry Davis -> http://www.spoke.com/info/pC4F8IB/KerryDavis
                    //Tom Bui -> http://www.linkedin.com/pub/tom-bui/2/329/168
                    //Mark Behm profiles -> http://www.linkedin.com/pub/dir/Mark/Behm
                    
                    if (line.Contains("-> http://"))
                    {
                        try
                        {
                            mytab = Regex.Split(line, " -> ");
                            Utils.Helper_Trace("XORCISM PROVIDER XINFO", "JobID:" + m_jobId + "People collected: " + line);
                            INFORMATION newInfo = new INFORMATION();
                            newInfo.Title = "People";
                            newInfo.Result = mytab[0];
                            newInfo.Url = mytab[1];
                            newInfo.JobID = m_jobId;
                            m_model.AddToINFORMATION(newInfo);
                            m_model.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            Utils.Helper_Trace("XORCISM PROVIDER XINFO", "JobID:" + m_jobId + "Exception esearchy: " + ex.Message + " " + ex.InnerException);
                        }
                    }
                    else
                    {
                        if (line.StartsWith(m_target))
                        {
                            //TODO : URLs
                        }
                    }
                }
                */
            }

            sshShell.Close();
        }
    }


    class XINFOParser
    {
        private string m_target;
        private XORCISMEntities m_model;
        private int m_jobId;
        private string m_policy;
        private string m_data = string.Empty;

        // A way to save XML parsing
        System.Collections.ArrayList list_parse = new System.Collections.ArrayList();

        public XINFOParser(string target, int jobId, string policy, string strategy)
        {
            m_jobId = jobId;
            m_target = target;
            m_model = new XORCISMEntities();
            m_policy = policy;
        }

        public bool Parse()
        {
            Assembly a;
            /* A way for loading XMLfile */
            XPathNavigator nav;
            XPathNavigator nav1;
            XPathDocument docNav;
            XPathNodeIterator NodeIter1;
            String strExpression1;


            a = Assembly.GetExecutingAssembly();
            Utils.Helper_Trace("XORCISM PROVIDER XINFO", "XINFO Assembly location = " + a.Location);


            return true;
        }
    }
}

 
