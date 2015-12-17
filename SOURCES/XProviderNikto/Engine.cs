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
using System.Xml.XPath;
using System.Configuration;

using XORCISMModel;
using XCommon;
using XProviderCommon;

using System.Text.RegularExpressions;

using FSM.DotNetSSH;
using System.Threading;

namespace XProviderNikto
{
    /// <summary>
    /// Copyright (C) 2012-2015 Jerome Athias
    /// Nikto plugin for XORCISM
    /// All trademarks and registered trademarks are the property of their respective owners.
    /// This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation; either version 2 of the License, or (at your option) any later version.
    /// 
    /// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
    /// 
    /// You should have received a copy of the GNU General Public License along with this program; if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
    /// </summary>
    /*
    public class program : Engine
    {
        // Partie à virer par la suite le run est independant ^^ 
        static void Main(string[] args)
        {
            //  on utilise la variable strategy pour cibler notre action (Tunning)
            Engine eng = new Engine();       
            eng.Run("www.google.fr", 1, "1", "0");
        }
        // Fin partie à virer par la suite 
    }
    */
    public class Engine : MarshalByRefObject, XProviderCommon.IVulnerabilityDetector
    {
        private string m_data = string.Empty;

        public Engine()
        {
            TextWriterTraceListener tw;
            tw = new TextWriterTraceListener("XProviderNikto.log"); //Hardcoded
            Trace.Listeners.Add(tw);
            Trace.AutoFlush = true;
            Trace.IndentSize = 4;
        }

        public override object InitializeLifetimeService()
        {
            return null; //Allow infinite lifetime
        }


        /* @default : port = "80"
         * @default : strategy | tunning ="x"
         */
        public void Run(string target, int jobID, string policy, string strategy) 
        {
            Utils.Helper_Trace("XORCISM PROVIDER NIKTO", "Entering Run()");
            Utils.Helper_Trace("XORCISM PROVIDER NIKTO", string.Format("Target = {0} , JobID = {1} , Policy = {2}, Strategy = {3}", target, jobID, policy, strategy));
            
            // delete spaces for multisite
            target = target.Replace(" ", ""); 
            
            /* On initialise une var */
            NiktoParser niktoParser = null;

            try
            {
                niktoParser = new NiktoParser(target, jobID, policy, strategy);
            }
            catch (Exception ex)
            {
                Utils.Helper_Trace("XORCISM PROVIDER NIKTO", "JobID:" + jobID + "Exception NiktoParser = " + ex.Message + " " + ex.InnerException);
            }

            string status = XCommon.STATUS.FINISHED.ToString();


            // =================================================
            // Change the status of the job to FINISHED or ERROR
            // =================================================

            if (niktoParser.Parse() == false)
            {
                status = XCommon.STATUS.ERROR.ToString();
                Utils.Helper_Trace("XORCISM PROVIDER NIKTO", string.Format("Updating job {0} status to ERROR", jobID));
                XCommon.Utils.Helper_SendEmail("contact@hackenaton.org", "Nikto ERROR", "Nikto ERROR for job:" + jobID);
            }
            else
            {
                Utils.Helper_Trace("XORCISM PROVIDER NIKTO", string.Format("Updating job {0} status to FINISHED", jobID));
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
                niktoParser = null;
            }
            catch (Exception ex)
            {
                Utils.Helper_Trace("XORCISM PROVIDER NIKTO", "JobID:" + jobID + "Exception UpdateJob = " + ex.Message + " " + ex.InnerException);
            }

            Utils.Helper_Trace("XORCISM PROVIDER NIKTO", "JobID:" + jobID + "Leaving Run()");
        }
    }

    class NiktoParser
    {
        private string m_target;
        private int m_jobId;
        private string m_policy;
        private string m_strategy;
        private string m_data = string.Empty;
        private XORCISMEntities m_model;

        // A way to save XML parsing
        System.Collections.ArrayList list_parse = new System.Collections.ArrayList();

        public NiktoParser(string target, int jobId, string policy, string strategy)
        {
            m_jobId = jobId;
            m_target = target;
            m_policy = policy;
            m_strategy = strategy;
            m_model = new XORCISMEntities();
        }

        public bool Parse()
        {
            Assembly a;
            /* A way for loading XMLfile */            

            a = Assembly.GetExecutingAssembly();
            Utils.Helper_Trace("XORCISM PROVIDER NIKTO", "NIKTO Assembly location = " + a.Location);

            /* Name of XML result */
            string file;
            file = string.Format("result_{0}_{1}.xml", DateTime.Now.Ticks, this.GetHashCode());

            Utils.Helper_Trace("XORCISM PROVIDER NIKTO", string.Format("JobID: {0} Results will be stored in file [{1}]", m_jobId, file));

            /* SSH instructions & declarations */
            int port=22;
            string address, username, password;
            string prompt;
            /*
            address = "111.222.333.444";
            username = "root";
            password = "toor";
            prompt = "root@xmachine:";
            */
            //HARDCODED
            address = "111.222.333.444";
                username = "root";
                password = "toor";
                prompt = "root"; //@backtrack:

            SshShell sshShell;
            sshShell = new SshShell(address, username, password);
//            sshShell.RemoveTerminalEmulationCharacters = true;

            Utils.Helper_Trace("XORCISM PROVIDER NIKTO", string.Format("JobID: {0} Connecting to NIKTO server at {1}", m_jobId, address));
            try
            {
                sshShell.Connect(port);
                //sshShell.Expect(prompt+"~#");
                sshShell.Expect(prompt);// + "~$");
            }
            catch (Exception ex)
            {
                Utils.Helper_Trace("XORCISM PROVIDER NIKTO", "Error in sshconnection to " + address + " " + ex.Message + " " + ex.InnerException);
            }

            // if ssh connection
            if (sshShell.Connected)
            {
                Utils.Helper_Trace("XORCISM PROVIDER NIKTO", string.Format("JobID: {0} Successfully connected to NIKTO server", m_jobId));

                //string output;
                string stdout = "";

                /* Command 1 */
                string cmd1;

                //cmd1 = "cd /home/tools/nikto-2.1.4";
                cmd1 = "cd /home/root/tools/nikto-2.1.4";   //HARDCODED
                sshShell.WriteLine(cmd1);
                //prompt = prompt+"/home/tools/nikto-2.1.4#";
                //prompt = "tools/nikto-2.1.4$";

                stdout = sshShell.Expect(prompt);

                /* See for provider m_model */

                /* For an URL */
                /*
                 * @ -T => tunning or strategy 
                 * @ -C all => to force check all possible dirs
                 */ 
                //cmd1 = string.Format("nikto -Format XML -o {2} -host {1} -T {3} -C all", m_policy, m_target, file, m_strategy); //-g -e 6
                cmd1 = string.Format("./nikto.pl -Format XML -o {2} -host {1} -C all", m_policy, m_target, file); //-g -e 6     //HARDCODED

                Utils.Helper_Trace("XORCISM PROVIDER NIKTO", string.Format("JobID: {0} Executing command [{1}]", m_jobId, cmd1));

                sshShell.WriteLine(cmd1);
                stdout = sshShell.Expect(prompt);

                Utils.Helper_Trace("XORCISM PROVIDER NIKTO", string.Format("JobID: {0} START DUMP STDOUT01", m_jobId));
                Utils.Helper_Trace("XORCISM PROVIDER NIKTO", stdout);

                string localOutputFile;
                localOutputFile = Path.GetTempFileName();

                Utils.Helper_Trace("XORCISM PROVIDER NIKTO", string.Format("Downloading results via SFTP to [{0}]", localOutputFile));

                nikto_get_result(file, localOutputFile);

                Utils.Helper_Trace("XORCISM PROVIDER NIKTO", string.Format("JobID: {0} XML PARSE successfull for file : [{1}] ", m_jobId, file));

                //aff_list();
                // Pause
                Console.ReadLine();

                sshShell.Close();
                sshShell = null;
                return true;
            }
            else
            {
                Utils.Helper_Trace("XORCISM PROVIDER NIKTO", string.Format("JobID: {0} Connection failed to NIKTO server", m_jobId));
                return false;
            }
        }

        public bool nikto_get_result(string file, string localOuputFile)
        {
            XPathNavigator nav;
            XPathNavigator nav1;
            XPathDocument docNav;
            XPathNodeIterator NodeIter1;
            String strExpression1;

            try
            {
                Sftp ftp;
                ftp = new Sftp("111.222.333.444", "root", "toor");  //Hardcoded
                
                ftp.OnTransferStart += new FileTransferEvent(ftp_OnTransferStart);
                ftp.OnTransferProgress += new FileTransferEvent(ftp_OnTransferProgress);
                ftp.OnTransferEnd += new FileTransferEvent(ftp_OnTransferEnd);

                ftp.Connect(22);

                ftp.Get("/home/root/tools/nikto-2.1.4/" + file, localOuputFile);    //Hardcoded

                ftp.Close();
            }
            catch (Exception ex)
            {
                Utils.Helper_Trace("XORCISM PROVIDER NIKTO", string.Format("Exception = {0} / {1}", ex.Message, ex.InnerException == null ? "" : ex.InnerException.Message));
                return false;
            }

            Utils.Helper_Trace("XORCISM PROVIDER NIKTO", "Loading the xml document");

            /* SAMPLE of XML structure 
             * 
             *  <?xml version="1.0" ?>
             *  <!DOCTYPE niktoscan SYSTEM "/usr/share/doc/nikto/nikto.dtd">
             *  <niktoscan options="-Format XML -o result_634550673449458000_35287174.xml -host TARGET -T x" version="2.1.1" nxmlversion="1.1">
             *  <scandetails targetip="IP_target" targethostname="URL_Target" targetport="80" targetbanner="gws" starttime="DATE/time" 
             *      sitename="http://URL:80/" siteip="http://IP:80/" hostheader="URL">
             *      
             *  <item id="999996" osvdbid="0" osvdblink="http://osvdb.org/0" method="GET">
             *  <description><![CDATA[robots.txt contains 268 entries which should be manually viewed.]]></description>
             *  <uri><![CDATA[/robots.txt]]></uri>
             *  <namelink><![CDATA[http://URL:80/robots.txt]]></namelink>
             *  <iplink><![CDATA[http://IP:80/robots.txt]]></iplink>
             *  </item>
             *  
             *  <item>... </item>
             * 
             *  <statistics elapsed="1535" itemsfound="11" itemstested="3818" endtime="DATE/time" />
             *  </scandetails>
             *  <statistics hoststotal="1" />
             *  </niktoscan>
             *
             */

            try
            {
                docNav = new XPathDocument(localOuputFile); // for test : result_634550673449458000_35287174.xml || URL file : file
                nav = docNav.CreateNavigator();
                nav1 = docNav.CreateNavigator();
                // If all is OK!
                Utils.Helper_Trace("XORCISM PROVIDER NIKTO", string.Format("JobID: {0} Successfully loaded XML file : [{1}] ", m_jobId, file));

                // Go to parse
                try
                {
                    // To read all results
                    strExpression1 = "/niktoscan/scandetails/@targetip | /niktoscan/scandetails/@targethostname | /niktoscan/scandetails/@targetport | /niktoscan/scandetails/@targetbanner | /niktoscan/scandetails/@sitename | /niktoscan/scandetails/@siteip";
                    NodeIter1 = nav1.Select(strExpression1);
                    while (NodeIter1.MoveNext())
                    {
                        // For headers
                        switch ((string)NodeIter1.Current.Name)
                        {
                                //Hardcoded
                            case "targetip":
                                Utils.Helper_Trace("XORCISM PROVIDER NIKTO", string.Format("JobID: {0} XML PARSE - TARGET IP : [{1}] ", m_jobId, NodeIter1.Current.Value));
                                break;
                            case "targethostname":
                                Utils.Helper_Trace("XORCISM PROVIDER NIKTO", string.Format("JobID: {0} XML PARSE - TARGET HOSTNAME : [{1}] ", m_jobId, NodeIter1.Current.Value));
                                break;
                            case "targetport":
                                Utils.Helper_Trace("XORCISM PROVIDER NIKTO", string.Format("JobID: {0} XML PARSE - TARGET PORT : [{1}] ", m_jobId, NodeIter1.Current.Value));
                                break;
                            case "targetbanner":
                                Utils.Helper_Trace("XORCISM PROVIDER NIKTO", string.Format("JobID: {0} XML PARSE - TARGET BANNER : [{1}] ", m_jobId, NodeIter1.Current.Value));
                                break;
                            case "sitename":
                                Utils.Helper_Trace("XORCISM PROVIDER NIKTO", string.Format("JobID: {0} XML PARSE - SITE NAME : [{1}] ", m_jobId, NodeIter1.Current.Value));
                                break;
                            case "siteip":
                                Utils.Helper_Trace("XORCISM PROVIDER NIKTO", string.Format("JobID: {0} XML PARSE - SITE IP : [{1}] ", m_jobId, NodeIter1.Current.Value));
                                break;
                        }
                        list_parse.Add((string)NodeIter1.Current.Value);
                    };

                    //Hardcoded
                    strExpression1 = "/niktoscan/scandetails/item/@id | /niktoscan/scandetails/item/@osvdbid | /niktoscan/scandetails/item/@osvdblink | /niktoscan/scandetails/item/description | /niktoscan/scandetails/item/uri | /niktoscan/scandetails/item/namelink | /niktoscan/scandetails/item/iplink";
                    NodeIter1 = nav1.Select(strExpression1);
                    
                    //TODO
                    /*
                    INFORMATION newInfo = null;
                    while (NodeIter1.MoveNext())
                    {
                        // For each Items
                        switch ((string)NodeIter1.Current.Name)
                        {
                            case "id":
                                Utils.Helper_Trace("XORCISM PROVIDER NIKTO", string.Format("JobID: {0} XML PARSE - ITEM ID : [{1}] ", m_jobId, NodeIter1.Current.Value));
                                if (newInfo != null)
                                {
                                    try
                                    {
                                        m_model.AddToINFORMATION(newInfo);
                                        m_model.SaveChanges();
                                    }
                                    catch (Exception ex)
                                    {
                                        Utils.Helper_Trace("XORCISM PROVIDER WHATWEB", "JobID:" + m_jobId + "Exception adding newInfo = " + ex.Message + " " + ex.InnerException);
                                    }
                                }
                                newInfo = new INFORMATION();
                                newInfo.Title = NodeIter1.Current.Value;
                                newInfo.JobID = m_jobId;
                                break;
                            case "osvdbid":
                                Utils.Helper_Trace("XORCISM PROVIDER NIKTO", string.Format("JobID: {0} XML PARSE - OSVDB ID : [{1}] ", m_jobId, NodeIter1.Current.Value));

                                break;
                            case "osvdblink":
                                Utils.Helper_Trace("XORCISM PROVIDER NIKTO", string.Format("JobID: {0} XML PARSE - OSVDB LINK : [{1}] ", m_jobId, NodeIter1.Current.Value));
                                break;
                            case "description":
                                Utils.Helper_Trace("XORCISM PROVIDER NIKTO", string.Format("JobID: {0} XML PARSE - DESCRIPTION : [{1}] ", m_jobId, NodeIter1.Current.Value));
                                newInfo.Description = NodeIter1.Current.Value;
                                //Todo: parse regex CAN-2004-0885. OSVDB-10637
                                break;
                            case "uri":
                                Utils.Helper_Trace("XORCISM PROVIDER NIKTO", string.Format("JobID: {0} XML PARSE - URI : [{1}] ", m_jobId, NodeIter1.Current.Value));
                                break;
                            case "namelink":
                                Utils.Helper_Trace("XORCISM PROVIDER NIKTO", string.Format("JobID: {0} XML PARSE - NAME LINK : [{1}] ", m_jobId, NodeIter1.Current.Value));
                                newInfo.Url = NodeIter1.Current.Value;
                                break;
                            case "iplink":
                                Utils.Helper_Trace("XORCISM PROVIDER NIKTO", string.Format("JobID: {0} XML PARSE - IP LINK : [{1}] ", m_jobId, NodeIter1.Current.Value));
                                break;
                        }
                        list_parse.Add((string)NodeIter1.Current.Value);
                    };
                    //Last one
                    if (newInfo != null)
                    {
                        try
                        {
                            m_model.AddToINFORMATION(newInfo);
                            m_model.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            Utils.Helper_Trace("XORCISM PROVIDER WHATWEB", "JobID:" + m_jobId + "Exception adding last newInfo = " + ex.Message + " " + ex.InnerException);
                        }
                    }
                    */

                    //Hardcoded
                    strExpression1 = "/niktoscan/scandetails/statistics/@elapsed | /niktoscan/scandetails/statistics/@itemsfound | /niktoscan/scandetails/statistics/@itemstested | /niktoscan/statistics/@hoststotal";
                    NodeIter1 = nav1.Select(strExpression1);
                    while (NodeIter1.MoveNext())
                    {
                        // For each statictics
                        switch ((string)NodeIter1.Current.Name)
                        {
                            case "elapsed":
                                Utils.Helper_Trace("XORCISM PROVIDER NIKTO", string.Format("JobID: {0} XML PARSE - ELAPSED : [{1}] ", m_jobId, NodeIter1.Current.Value));
                                break;
                            case "itemsfound":
                                Utils.Helper_Trace("XORCISM PROVIDER NIKTO", string.Format("JobID: {0} XML PARSE - ITEMS FOUND : [{1}] ", m_jobId, NodeIter1.Current.Value));
                                break;
                            case "itemstested":
                                Utils.Helper_Trace("XORCISM PROVIDER NIKTO", string.Format("JobID: {0} XML PARSE - ITEMS TESTED : [{1}] ", m_jobId, NodeIter1.Current.Value));
                                break;
                            case "targetbanner":
                                Utils.Helper_Trace("XORCISM PROVIDER NIKTO", string.Format("JobID: {0} XML PARSE - TARGET BANNER : [{1}] ", m_jobId, NodeIter1.Current.Value));
                                break;
                            case "hoststotal":
                                Utils.Helper_Trace("XORCISM PROVIDER NIKTO", string.Format("JobID: {0} XML PARSE - NUMBER OF HOST : [{1}] ", m_jobId, NodeIter1.Current.Value));
                                break;
                        }
                        list_parse.Add((string)NodeIter1.Current.Value);
                    };
                }
                catch (System.Exception ex)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER NIKTO", "JobID:" + m_jobId + "Exception Parsing XML PLUGIN'S = " + ex.Message + " " + ex.InnerException);
                }
            }
            catch (System.Exception ex)
            {
                Utils.Helper_Trace("XORCISM PROVIDER NIKTO", "JobID:" + m_jobId + "Exception LOADING XML "+localOuputFile+"= " + ex.Message + " " + ex.InnerException);

                //Retry
                Thread.Sleep(120000);   //Hardcoded
                nikto_get_result(file, localOuputFile);
            }
            return true;
        }

            // show content's list
            public void aff_list()
            {
                int i = 0;

                while (i < list_parse.Count)
                {
                    Console.WriteLine("XORCISM PROVIDER NIKTO Result index : {0} => [{1}] ", i, list_parse[i]);
                    i++;
                }
            }


            void ftp_OnTransferEnd(string src, string dst, int transferredBytes, int totalBytes, string message)
            {
                Utils.Helper_Trace("XORCISM PROVIDER NIKTO", "SFTP transfer finished");
            }

            void ftp_OnTransferProgress(string src, string dst, int transferredBytes, int totalBytes, string message)
            {

            }

            void ftp_OnTransferStart(string src, string dst, int transferredBytes, int totalBytes, string message)
            {
                Utils.Helper_Trace("XORCISM PROVIDER NIKTO", "SFTP transfer started");
            }
    }
}
