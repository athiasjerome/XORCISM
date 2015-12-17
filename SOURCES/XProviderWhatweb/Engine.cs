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


namespace XProviderWhatweb
{
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
    /// Whatweb plugin for XORCISM
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
        private int maxtry = 0;

        public Engine()
        {
            TextWriterTraceListener tw;
            tw = new TextWriterTraceListener("XProviderWhatweb.log");   //Hardcoded
            Trace.Listeners.Add(tw);
            Trace.AutoFlush = true;
            Trace.IndentSize = 4;
        }

        public override object InitializeLifetimeService()
        {
            return null; //Allow infinite lifetime
        }

        
        /* Choix de typage pour la var. policy
         * j'ai laissé en string plutot que int car en fait il y a la possibilitée de passer aussi des chaines de char : "aggressive"... 
         */ 
        public void Run(string target, int jobID, string policy, string strategy)
        {
            Utils.Helper_Trace("XORCISM PROVIDER WHATWEB", "Entering Run()");
            maxtry = 0;
            Utils.Helper_Trace("XORCISM PROVIDER WHATWEB", string.Format("Target = {0} , JobID = {1} , Policy = {2}", target, jobID, policy));

           /* On initialise une var */
           // WhatwebParser whatwebParser = new WhatwebParser("www.google.fr", 1, "1", "nop");
           WhatwebParser whatwebParser = null;
            try
            {
                whatwebParser = new WhatwebParser(target, jobID, policy, strategy);
            }
            catch (Exception ex)
            {
                Utils.Helper_Trace("XORCISM PROVIDER WHATWEB", "JobID:" + jobID + "Exception WhatwebParser = " + ex.Message + " " + ex.InnerException);
            }

            string status = XCommon.STATUS.FINISHED.ToString();


            // =================================================
            // Change the status of the job to FINISHED or ERROR
            // =================================================

            if (whatwebParser.Parse() == false)
            {
                status = XCommon.STATUS.ERROR.ToString();
                Utils.Helper_Trace("XORCISM PROVIDER WHATWEB", string.Format("Updating job {0} status to ERROR", jobID));
                XCommon.Utils.Helper_SendEmail("contact@hackenaton.org", "Whatweb ERROR", "Whatweb ERROR for job:" + jobID);
            }
            else
            {
                Utils.Helper_Trace("XORCISM PROVIDER WHATWEB", string.Format("Updating job {0} status to FINISHED", jobID));
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
                whatwebParser = null;
            }
            catch (Exception ex)
            {
                Utils.Helper_Trace("XORCISM PROVIDER WHATWEB", "JobID:" + jobID + "Exception UpdateJob = " + ex.Message + " " + ex.InnerException);
            }

            Utils.Helper_Trace("XORCISM PROVIDER WHATWEB", "JobID:" + jobID + "Leaving Run()");
        }
    }


    class WhatwebParser
    {
        private string m_target;
        private XORCISMEntities m_model;
        private int m_jobId;
        private string m_policy;
        private string m_data = string.Empty;

        // A way to save XML parsing
        System.Collections.ArrayList list_parse = new System.Collections.ArrayList();

        public WhatwebParser(string target, int jobId, string policy, string strategy)
        {
            m_jobId = jobId;
            m_target = target;
            m_model = new XORCISMEntities();
            m_policy = policy;
        }

        public bool Parse()
        {
            Assembly a;
            
            a = Assembly.GetExecutingAssembly();
            Utils.Helper_Trace("XORCISM PROVIDER WHATWEB", "WHATWEB Assembly location = " + a.Location);
            

            /* Name of XML result */
            string file;
            file = string.Format("result_{0}_{1}.xml", DateTime.Now.Ticks, this.GetHashCode());

            Utils.Helper_Trace("XORCISM PROVIDER WHATWEB", string.Format("JobID: {0} Results will be stored in file [{1}]", m_jobId, file));
            

            /* SSH instructions & declarations */
            int port=22;
            string address, username, password;
            string prompt;
            /*
            address = "192.168.79.129"; //111.222.333.444
            username = "root"; //jerome 
            password = "burnass"; //jerome
            //prompt = "root@ubuntu:~#";            
            */
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

            SshShell sshShell;
            sshShell = new SshShell(address, username, password);
            sshShell.RemoveTerminalEmulationCharacters = true;

            Utils.Helper_Trace("XORCISM PROVIDER WHATWEB", string.Format("JobID: {0} Connecting to WHATWEB server at {1}", m_jobId, address));            
            
            try
            {
                sshShell.Connect(port);
                //sshShell.Expect(prompt+"~#");
                sshShell.Expect(prompt);// + "~$");
            }
            catch (Exception ex)
            {
                Utils.Helper_Trace("XORCISM PROVIDER WHATWEB", string.Format("JobID: {0} ConnectingERROR to WHATWEB server at {1} : " + ex.Message + " " + ex.InnerException, m_jobId, address));
                address = "111.222.333.444";
                username = "root";
                password = "toor";
                //prompt = "root@backtrack:";
                sshShell = new SshShell(address, username, password);
                sshShell.RemoveTerminalEmulationCharacters = true;
                
                Utils.Helper_Trace("XORCISM PROVIDER WHATWEB", string.Format("JobID: {0} Connecting to WHATWEB server at {1}", m_jobId, address));
                try
                {
                    sshShell.Connect(port);
                    sshShell.Expect(prompt);// + "~$");
                }
                catch (Exception ex2)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER WHATWEB", string.Format("JobID: {0} ConnectingERROR to WHATWEB server at {1} : "+ex2.Message+" "+ex2.InnerException, m_jobId, address));
                }
            }

            Utils.Helper_Trace("XORCISM PROVIDER WHATWEB", string.Format("JobID: {0} Successfully connected to WHATWEB server", m_jobId));

            //string output;
            string stdout = "";
            //string stderr = "";

            /* Command 1 */
            string cmd1;

            //cmd1 = "cd /home/tools/whatweb-0.4.7";
            cmd1 = "cd /home/root/tools/whatweb-0.4.7/";
            sshShell.WriteLine(cmd1);
            //prompt = prompt+"/home/tools/whatweb-0.4.7#";
            //prompt = "root";//@backtrack:~/tools/whatweb-0.4.7$";
            //prompt = "tools/whatweb-0.4.7";
            stdout = sshShell.Expect(prompt);
            Utils.Helper_Trace("XORCISM PROVIDER WHATWEB", string.Format("JobID: {0} START DUMP STDOUT01", m_jobId));
            Utils.Helper_Trace("XORCISM PROVIDER WHATWEB", stdout);

            cmd1 = "sudo /usr/local/rvm/bin/rvm use 1.8.7";
            sshShell.WriteLine(cmd1);
            Thread.Sleep(2000);
            stdout = sshShell.Expect(prompt);
            Utils.Helper_Trace("XORCISM PROVIDER WHATWEB", string.Format("JobID: {0} START DUMP STDOUT02", m_jobId));
            Utils.Helper_Trace("XORCISM PROVIDER WHATWEB", stdout);
            sshShell.WriteLine(password);
            Thread.Sleep(2000);
            stdout = sshShell.Expect(prompt);
            Utils.Helper_Trace("XORCISM PROVIDER WHATWEB", string.Format("JobID: {0} START DUMP STDOUT03", m_jobId));
            Utils.Helper_Trace("XORCISM PROVIDER WHATWEB", stdout);

            /* See for provider m_model */
            
            /* For an URL */
            /*
            AGGRESSION LEVELS:
  --aggression, -a=LEVEL The aggression level controls the trade-off between
                        speed/stealth and reliability. Default: 1
                        Aggression levels are:
        1 (Passive)     Make one HTTP request per target. Except for redirects.
        2 (Polite)      Reserved for future use
        3 (Aggressive)  Triggers aggressive plugin functions only when a
                        plugin matches passively.
        4 (Heavy)       Trigger aggressive functions for all plugins. Guess a
                        lot of URLs like Nikto.
            */
            string agressionlevel = "1";
            if (m_policy == "Moderate") agressionlevel = "3";
            if (m_policy == "Intrusive") agressionlevel = "4";
            if (m_policy == "PCI DSS") agressionlevel = "3";

            cmd1 = string.Format("sudo ./whatweb -r -a {0} {1} --log-xml={2}", agressionlevel, m_target, file);

            Utils.Helper_Trace("XORCISM PROVIDER WHATWEB", string.Format("JobID: {0} Executing command [{1}]", m_jobId, cmd1));

            sshShell.WriteLine(cmd1);
            Thread.Sleep(2000);
            stdout = "";
            string localOutputFile;
            localOutputFile = Path.GetTempFileName();
            //ON ATTEND PLUS LE PROMPT CAR DES FOIS CA VIENT PAS - DEBUG A VOIR
            Thread.Sleep(60000);
            /*
                stdout = sshShell.Expect(prompt);

                Utils.Helper_Trace("XORCISM PROVIDER WHATWEB", string.Format("JobID: {0} START DUMP STDOUT04", m_jobId));
                Utils.Helper_Trace("XORCISM PROVIDER WHATWEB", stdout);
                //whatweb: command not found
                //http://www.marocannonces.com/ ERROR: Timed out execution expired

                // HACK :
                // outputfile = "634244542240861588_39608125_output";

                if (stdout.Contains("bson_ext gem is in your load path"))
                {
                    Utils.Helper_Trace("XORCISM PROVIDER WHATWEB", string.Format("JobID: {0} ExecutingAGAIN command [{1}]", m_jobId, cmd1));
                    //We were not root?
                    sshShell.WriteLine(cmd1);
                    Thread.Sleep(2000);
                    stdout = sshShell.Expect(prompt);

                    Utils.Helper_Trace("XORCISM PROVIDER WHATWEB", string.Format("JobID: {0} START DUMP STDOUT05", m_jobId));
                    Utils.Helper_Trace("XORCISM PROVIDER WHATWEB", stdout);
                }
            */

            if (stdout.Contains("bson_ext gem is in your load path"))
            {
                //TODO: voir cette erreur
                
            }
            else
            {
                whatweb_get_result(m_jobId, file, localOutputFile,1);
            }
                

//            aff_list();
            // Pause
            Console.ReadLine();

            sshShell.Close();
            sshShell = null;
            return true;
        }

        bool whatweb_get_result(int thejobid, string file, string localOutputFile, int maxtrynb)
        {
            /* A way for loading XMLfile */
            int maxtry = maxtrynb;
            XPathNavigator nav;
            XPathNavigator nav1;
            XPathDocument docNav;
            XPathNodeIterator NodeIter1;
            String strExpression1;

            Utils.Helper_Trace("XORCISM PROVIDER WHATWEB", string.Format("{0} Downloading results via SFTP to [{1}]", thejobid, localOutputFile));

            try
            {
                Sftp ftp;
                ftp = new Sftp("111.222.333.444", "root", "toor");
                ftp.OnTransferStart += new FileTransferEvent(ftp_OnTransferStart);
                ftp.OnTransferProgress += new FileTransferEvent(ftp_OnTransferProgress);
                ftp.OnTransferEnd += new FileTransferEvent(ftp_OnTransferEnd);

                ftp.Connect(22);

                ftp.Get("/home/root/tools/whatweb-0.4.7/" + file, localOutputFile);

                ftp.Close();
            }
            catch (Exception ex)
            {
                Utils.Helper_Trace("XORCISM PROVIDER WHATWEB", string.Format("Exception = {0} / {1}", ex.Message, ex.InnerException == null ? "" : ex.InnerException.Message));
                return false;
            }

            Utils.Helper_Trace("XORCISM PROVIDER WHATWEB", "Loading the xml document");

            /* SAMPLE of XML structure 
             * 
             *  <?xml version="
             *  <log>
             *  <target>
             *      <uri>http://exemple.com</uri>
             *      <http-status>xxx</http-status>
             *      <plugin>
             *          <name>plugin's name</name>
             *          <string>result of plugin</string>
             *      <plugin>
             *      <plugin>
             *          <name>...</name>
             *          <string>...</string>
             *      </plugin>
             *  </target>
             *  </log>
             *  <target>
             *      .....
             *  </target>
             *  
            */

            try
            {
                docNav = new XPathDocument(localOutputFile); // for test : result_634521969362210000_41014879.xml || URL file : file

                nav = docNav.CreateNavigator();
                nav1 = docNav.CreateNavigator();
                // If all is OK!
                Utils.Helper_Trace("XORCISM PROVIDER WHATWEB", string.Format("JobID: {0} Successfully loaded XML file : [{1}] ", m_jobId, file));

                // Go to parse
                //TODO
                /*
                try
                {
                    // To read all plugins
                    strExpression1 = "/log/target/uri | /log/target/http-status | /log/target/plugin/name | /log/target/plugin/version | /log/target/plugin/string | /log/target/plugin/modules | /log/target/plugin/modules/module";
                    NodeIter1 = nav1.Select(strExpression1);
                    INFORMATION newInfo = null;
                    while (NodeIter1.MoveNext())
                    {
                        switch ((string)NodeIter1.Current.Name)
                        {
                            case "name":
                                Utils.Helper_Trace("XORCISM PROVIDER WHATWEB", string.Format("JobID: {0} XML PARSE - PLUGIN NAME : [{1}] ", m_jobId, NodeIter1.Current.Value));
                                if (newInfo != null)
                                {
                                    try
                                    {
                                        //Check if the current information already exists
                                        var checkinfo = from i in m_model.INFORMATION
                                                        where i.JobID == m_jobId && i.Title == newInfo.Title && i.Description == newInfo.Description && i.Result == newInfo.Result
                                                        select i;
                                        if (checkinfo.Count() < 1)
                                        {
                                            m_model.INFORMATION.Add(newInfo);
                                            m_model.SaveChanges();
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Utils.Helper_Trace("XORCISM PROVIDER WHATWEB", "JobID:" + m_jobId + "Exception adding newInfo = " + ex.Message + " " + ex.InnerException);
                                    }
                                }
                                newInfo = new INFORMATION();
                                newInfo.Title = NodeIter1.Current.Value;
                                newInfo.JobID = m_jobId;
                                newInfo.Description = null;
                                newInfo.Result = null;
                                break;
                            case "os":
                                Utils.Helper_Trace("XORCISM PROVIDER WHATWEB", string.Format("JobID: {0} XML PARSE - OS : [{1}] ", m_jobId, NodeIter1.Current.Value));
                                newInfo.Description = NodeIter1.Current.Value;
                                break;
                            case "string":
                                Utils.Helper_Trace("XORCISM PROVIDER WHATWEB", string.Format("JobID: {0} XML PARSE - PLUGIN CONTENT : [{1}] ", m_jobId, NodeIter1.Current.Value));
                                newInfo.Result += NodeIter1.Current.Value + " ";
                                break;
                            case "version":
                                Utils.Helper_Trace("XORCISM PROVIDER WHATWEB", string.Format("JobID: {0} XML PARSE - VERSION : [{1}] ", m_jobId, NodeIter1.Current.Value));
                                newInfo.Description = NodeIter1.Current.Value;
                                break;
                            case "uri":
                                Utils.Helper_Trace("XORCISM PROVIDER WHATWEB", string.Format("JobID: {0} XML PARSE - URI : [{1}] ", m_jobId, NodeIter1.Current.Value));
                                break;
                            case "http-status":
                                Utils.Helper_Trace("XORCISM PROVIDER WHATWEB", string.Format("JobID: {0} XML PARSE - HTTP-STATUS : [{1}] ", m_jobId, NodeIter1.Current.Value));
                                break;
                            case "modules":
                                Utils.Helper_Trace("XORCISM PROVIDER WHATWEB", string.Format("JobID: {0} XML PARSE - MODULES : [{1}] ", m_jobId, NodeIter1.Current.Value));
                                break;
                            case "module":
                                Utils.Helper_Trace("XORCISM PROVIDER WHATWEB", string.Format("JobID: {0} XML PARSE - MODULE : [{1}] ", m_jobId, NodeIter1.Current.Value));
                                newInfo.Result += NodeIter1.Current.Value + " ";
                                break;
                            default:
                                Utils.Helper_Trace("XORCISM PROVIDER WHATWEB", string.Format("JobID: {0} XML PARSE - {1} : [{2}] ", m_jobId, (string)NodeIter1.Current.Name, NodeIter1.Current.Value));
                                break;
                        }
                        //                        list_parse.Add((string)NodeIter1.Current.Value);
                    };
                    //Last one
                    if (newInfo != null)
                    {
                        try
                        {
                            m_model.INFORMATION.Add(newInfo);
                            m_model.SaveChanges();

                            maxtry = 999;
                        }
                        catch (Exception ex)
                        {
                            Utils.Helper_Trace("XORCISM PROVIDER WHATWEB", "JobID:" + m_jobId + "Exception adding last newInfo = " + ex.Message + " " + ex.InnerException);
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER WHATWEB", "JobID:" + m_jobId + "Exception Parsing XML PLUGIN'S = " + ex.Message + " " + ex.InnerException);
                }
                */
            }
            catch (System.Exception ex)
            {
                Utils.Helper_Trace("XORCISM PROVIDER WHATWEB", "JobID:" + m_jobId + "Exception LOADING XML = " + ex.Message + " - " + ex.InnerException);
                //Unexpected end of file has occurred. The following elements are not closed: log
                if (ex.Message.Contains("The following elements are not closed: log"))
                {
                    try
                    {
                        StreamReader myfilereader = new StreamReader(localOutputFile);
                        string newlocalOutputFile = Path.GetTempFileName();
                        Utils.Helper_Trace("XORCISM PROVIDER WHATWEB", "Writing new file: " + newlocalOutputFile);
                        StreamWriter mynewfile = new StreamWriter(newlocalOutputFile);
                        string ligne = myfilereader.ReadLine();
                        while (ligne != null)
                        {
                            if (ligne.Contains("</target>"))
                            {
                                mynewfile.WriteLine("</log>");
                            }
                            mynewfile.WriteLine(ligne);
                            ligne = myfilereader.ReadLine();
                        }
                        myfilereader.Close();
                        mynewfile.Close();
                        localOutputFile = newlocalOutputFile;
                    }
                    catch (Exception ex2)
                    {
                        Utils.Helper_Trace("XORCISM PROVIDER WHATWEB", "JobID:" + m_jobId + "Exception FIXING XML = " + ex2.Message + " - " + ex2.InnerException);
                    }
                }
                else
                {
                    //Retry
                    Thread.Sleep(60000);
                }
                maxtry++;
                if (maxtry < 60)
                {
                    Thread.Sleep(60000);
                    whatweb_get_result(m_jobId, file, localOutputFile,maxtry);
                }
            }
            Utils.Helper_Trace("XORCISM PROVIDER WHATWEB", string.Format("JobID: {0} XML PARSE successfull for file : [{1}] ", m_jobId, file));
            maxtry = 999;
            return true;
        }

        // show content's list
        public void aff_list()
        {
            int i = 0;

            while (i < list_parse.Count)
            {
                Console.WriteLine("XORCISM PROVIDER WHATWEB Result index : {0} => [{1}] ", i, list_parse[i]);
                i++;
            }
        }



        void ftp_OnTransferEnd(string src, string dst, int transferredBytes, int totalBytes, string message)
        {
            Utils.Helper_Trace("XORCISM PROVIDER WHATWEB", "SFTP transfer finished");
        }

        void ftp_OnTransferProgress(string src, string dst, int transferredBytes, int totalBytes, string message)
        {

        }

        void ftp_OnTransferStart(string src, string dst, int transferredBytes, int totalBytes, string message)
        {
            Utils.Helper_Trace("XORCISM PROVIDER WHATWEB", "SFTP transfer started");
        }
       

    }
}

 
