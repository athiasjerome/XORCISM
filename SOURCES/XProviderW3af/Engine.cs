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

namespace XProviderw3af
{
    /*
    public class program : Engine
    {
        static void Main(string[] args)
        {
            Engine eng = new Engine();
            eng.Run("www.google.fr", 1, "1", "nop");
        }
    }
*/
    /// <summary>
    /// Copyright (C) 2012-2015 Jerome Athias
    /// XORCISM Plugin for w3af
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

            public Engine()
            {
                TextWriterTraceListener tw;
                tw = new TextWriterTraceListener("XProviderw3af.log");  //Hardcoded
                Trace.Listeners.Add(tw);
                Trace.AutoFlush = true;
                Trace.IndentSize = 4;
            }

            public override object InitializeLifetimeService()
            {
                return null; //Allow infinite lifetime
            }


            /* Choix de typage pour la var. policy
             * en string plutot que int car en fait il y a la possibilité de passer aussi des chaines de char : "aggressive"... 
             */
            public void Run(string target, int jobID, string policy, string strategy)
            {
                //TODO: Input Validation
                Utils.Helper_Trace("XORCISM PROVIDER W3AF", "Entering Run()");
                Utils.Helper_Trace("XORCISM PROVIDER W3AF", string.Format("Target = {0} , JobID = {1} , Policy = {2}", target, jobID, policy));

                /* On initialise une var */
                W3afParser w3afParser = null;

                /* Name of XML result */
                string xml_file;
                xml_file = string.Format("result_{0}_{1}.xml", DateTime.Now.Ticks, this.GetHashCode());

                try
                {
                    w3afParser = new W3afParser(target, jobID, policy, strategy, xml_file);
                }
                catch (Exception ex)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER W3AF", "JobID:" + jobID + "Exception w3afParser = " + ex.Message + " " + ex.InnerException);
                }

                string status = XCommon.STATUS.FINISHED.ToString();

                // ==============================
                // Have an instance of W3afScript
                // ==============================
                    // Create the script w3af
                    w3afParser.create_Script();


                // =================================================
                // Change the status of the job to FINISHED or ERROR
                // =================================================

                if (w3afParser.Parse(xml_file) == false)
                {
                    status = XCommon.STATUS.ERROR.ToString();
                    Utils.Helper_Trace("XORCISM PROVIDER W3AF", string.Format("Updating job {0} status to ERROR", jobID));
                    XCommon.Utils.Helper_SendEmail("contact@hackenaton.org", "W3af ERROR", "W3af ERROR for job:" + jobID);
                }
                else
                {
                    Utils.Helper_Trace("XORCISM PROVIDER W3AF", string.Format("Updating job {0} status to FINISHED", jobID));
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


                    w3afParser = null;
                }
                catch (Exception ex)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER W3AF", "JobID:" + jobID + "Exception UpdateJob = " + ex.Message + " " + ex.InnerException);
                }

                Utils.Helper_Trace("XORCISM PROVIDER W3AF", "JobID:" + jobID + "Leaving Run()");
            }
        }

        class W3afParser
        {
            private string m_target;
            private XORCISMEntities m_model;
            private int m_jobId;
            private string m_policy;
            private string m_data = string.Empty;
            private string m_file;
            private string m_strategy;
            private W3afScript w3afScript;

            // A way to save XML parsing
            System.Collections.ArrayList list_parse = new System.Collections.ArrayList();

            public W3afParser(string target, int jobId, string policy, string strategy, string file)
            {
                m_jobId = jobId;
                m_target = target;
                m_model = new XORCISMEntities();
                m_policy = policy;
                m_file = file;
                m_strategy = strategy;
            }

            public bool Parse(string m_file)
            {
                Assembly a;
                /* A way for loading XMLfile */
                XPathNavigator nav;
                XPathNavigator nav1;
                XPathDocument docNav;
                XPathNodeIterator NodeIter1;
                String strExpression1;


                a = Assembly.GetExecutingAssembly();
                Utils.Helper_Trace("XORCISM PROVIDER W3AF", "W3AF Assembly location = " + a.Location);


                /* Name of XML result */
                //string file;
                //file = string.Format("result_{0}_{1}.xml", DateTime.Now.Ticks, this.GetHashCode());

                Utils.Helper_Trace("XORCISM PROVIDER W3AF", string.Format("JobID: {0} Results will be stored in file [{1}]", m_jobId, m_file));

                /* SSH instructions & declarations */
                int port;
                string address, username, password, prompt;

                //HARDCODED
                port = 22;
                
                address = "111.222.333.444";
                username = "root";
                password = "toor";
                prompt = "root";

                SshShell sshShell;
                sshShell = new SshShell(address, username, password);
                sshShell.RemoveTerminalEmulationCharacters = true;

                Utils.Helper_Trace("XORCISM PROVIDER W3AF", string.Format("JobID: {0} Connecting to W3AF server at {1}", m_jobId, address));
                try{
                    sshShell.Connect(port);
                    sshShell.Expect(prompt);
                }
                catch (Exception ex2)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER W3AF", string.Format("JobID: {0} ConnectingERROR to W3AF server at {1} : " + ex2.Message + " " + ex2.InnerException, m_jobId, address));
                }
                Utils.Helper_Trace("XORCISM PROVIDER W3AF", string.Format("JobID: {0} Successfully connected to W3AF server", m_jobId));

                //string output;
                string stdout = "";
                //string stderr = "";

                /* Command 1 */
                string cmd;
                cmd = "cd /home/root/tools/w3af/";  //Hardcoded
                sshShell.WriteLine(cmd);
                
                // We create the real script file
                string scriptfile = w3afScript.getScriptFile();

                stdout = sshShell.Expect(prompt);

                Utils.Helper_Trace("XORCISM PROVIDER W3AF", string.Format("Uploading script " + scriptfile+" to "+scriptfile.Replace(Path.GetTempPath(), "") + " via SFTP"));

                try
                {
                    Sftp ftp;
                    ftp = new Sftp("111.222.333.444", "root", "toor");  //HARDCODED
                    ftp.OnTransferStart += new FileTransferEvent(ftp_OnTransferStart);
                    ftp.OnTransferProgress += new FileTransferEvent(ftp_OnTransferProgress);
                    ftp.OnTransferEnd += new FileTransferEvent(ftp_OnTransferEnd);

                    ftp.Connect(22);

                    ftp.Put(scriptfile, "/home/root/tools/w3af/" + scriptfile.Replace(Path.GetTempPath(), ""));  //HARDCODED

                    ftp.Close();
                }
                catch (Exception ex)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER W3AF", string.Format("Exception = {0} / {1}", ex.Message, ex.InnerException == null ? "" : ex.InnerException.Message));
                    return false;
                }


                cmd = string.Format("./w3af_console -s {0}", scriptfile.Replace(Path.GetTempPath(),""));
                Utils.Helper_Trace("XORCISM PROVIDER W3AF", "Executing command: " + cmd);
                sshShell.WriteLine(cmd);
                //stdout = sshShell.Expect(prompt);
                stdout = sshShell.Expect("Scan finished in");
                /*
                Scan finished in 2 hours 3 minutes 5 seconds.
                w3af>>>
                */

                Utils.Helper_Trace("XORCISM PROVIDER W3AF", string.Format("JobID: {0} START DUMP STDOUT01", m_jobId));
                Utils.Helper_Trace("XORCISM PROVIDER W3AF", stdout);

                string localOutputFile;
                localOutputFile = Path.GetTempFileName();
                Utils.Helper_Trace("XORCISM PROVIDER W3AF", string.Format("Downloading results via SFTP to [{0}]", localOutputFile));

                try
                {
                    Sftp ftp;
                    ftp = new Sftp("111.222.333.444", "root", "toor");  //HARDCODED
                    ftp.OnTransferStart += new FileTransferEvent(ftp_OnTransferStart);
                    ftp.OnTransferProgress += new FileTransferEvent(ftp_OnTransferProgress);
                    ftp.OnTransferEnd += new FileTransferEvent(ftp_OnTransferEnd);

                    ftp.Connect(22);

                    ftp.Get("/home/root/tools/w3af/" + m_file, localOutputFile);  //HARDCODED

                    ftp.Close();
                }
                catch (Exception ex)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER W3AF", string.Format("Exception = {0} / {1}", ex.Message, ex.InnerException == null ? "" : ex.InnerException.Message));
                    return false;
                }


                /* Here sample XML
                 *  <?xml version="1.0" encoding="UTF-8"?>
                    <w3afrun start="..." startstr="..." xmloutputversion="1.00">
                        <scaninfo target="TARGET">
                        <audit>
                            <plugin name="..."/>
                            ...
                        </audit>
                        <bruteforce/>
                        <grep>
                            <plugin name="..."/>
                            ...
                        </grep>
                        <evasion/>
                        <output>
                            <plugin name="FILE TYPE">
                                <config parameter="FILENAME" value="PATH"/>
                            </plugin>
                        </output>
                        <mangle/>
                        <discovery>
                            <plugin name="..."/>
                            ...
                        </discovery>
                        </scaninfo>
                        <vulnerability method="..." name="..." severity="Low" url="..." var="...">EXPLANATION</vulnerability>
                        <information id="[...]" name="..." url="...">EXPLANATION</information>
                        <error caller="PLUGIN">EXPLANATION</error>
                    </w3afrun>
                 */

                try
                {
                    docNav = new XPathDocument(localOutputFile); // for test : result_634521969362210000_41014879.xml || URL file : file
                    nav = docNav.CreateNavigator();
                    nav1 = docNav.CreateNavigator();
                    // If all is OK!
                    Utils.Helper_Trace("XORCISM PROVIDER W3AF", string.Format("JobID: {0} Successfully loaded XML file : [{1}] ", m_jobId, localOutputFile));

                    // Go to parse
                    try
                    {
                        // To read all plugins
                        //HARDCODED
                        strExpression1 = "/w3afrun/@startstr | /w3afrun/scaninfo/@target | /w3afrun/scaninfo/audit/plugin/@name | /w3afrun/scaninfo/grep/plugin/@name | /w3afrun/scaninfo/output/plugin/@name | /w3afrun/scaninfo/output/plugin/config/@parameter | /w3afrun/scaninfo/output/plugin/config/@value | /w3afrun/scaninfo/discovery/plugin/@name | /w3afrun/vulnerability/@method | /w3afrun/vulnerability/@name | /w3afrun/vulnerability/@severity | /w3afrun/vulnerability/@url | /w3afrun/vulnerability/@var | /w3afrun/vulnerability | /w3afrun/information/@name | /w3afrun/information/@id | /w3afrun/information/@url | /w3afrun/information | /w3afrun/error/@caller | /w3afrun/error";
                        NodeIter1 = nav1.Select(strExpression1);
                        while (NodeIter1.MoveNext())
                        {
                            switch ((string)NodeIter1.Current.Name)
                            {
                                case "startstr":
                                    Utils.Helper_Trace("XORCISM PROVIDER W3AF", string.Format("JobID: {0} XML PARSE - START-TIME : [{1}] ", m_jobId, NodeIter1.Current.Value));
                                    break;
                                case "target":
                                    Utils.Helper_Trace("XORCISM PROVIDER W3AF", string.Format("JobID: {0} XML PARSE - TARGET : [{1}] ", m_jobId, NodeIter1.Current.Value));
                                    break;
                                case "method":
                                    Utils.Helper_Trace("XORCISM PROVIDER W3AF", string.Format("JobID: {0} XML PARSE - VULNERABILITY - METHOD : [{1}] ", m_jobId, NodeIter1.Current.Value));
                                    break;
                                case "name":
                                    Utils.Helper_Trace("XORCISM PROVIDER W3AF", string.Format("JobID: {0} XML PARSE - VULNERABILITY - NAME : [{1}] ", m_jobId, NodeIter1.Current.Value));
                                    break;
                                case "severity":
                                    Utils.Helper_Trace("XORCISM PROVIDER W3AF", string.Format("JobID: {0} XML PARSE - VULNERABILITY - SEVERITY : [{1}] ", m_jobId, NodeIter1.Current.Value));
                                    break;
                                case "url":
                                    Utils.Helper_Trace("XORCISM PROVIDER W3AF", string.Format("JobID: {0} XML PARSE - VULNERABILITY - URL : [{1}] ", m_jobId, NodeIter1.Current.Value));
                                    break;
                                case "var":
                                    Utils.Helper_Trace("XORCISM PROVIDER W3AF", string.Format("JobID: {0} XML PARSE - VULNERABILITY - VAR : [{1}] ", m_jobId, NodeIter1.Current.Value));
                                    break;
                                case "vulnerability":
                                    Utils.Helper_Trace("XORCISM PROVIDER W3AF", string.Format("JobID: {0} XML PARSE - VULNERABILITY - EXPLANATION : [{1}] ", m_jobId, NodeIter1.Current.Value));
                                    break;
                                case "information":
                                    Utils.Helper_Trace("XORCISM PROVIDER W3AF", string.Format("JobID: {0} XML PARSE - VULNERABILITY - INFORMATION : [{1}] ", m_jobId, NodeIter1.Current.Value));
                                    break;
                                case "error":
                                    Utils.Helper_Trace("XORCISM PROVIDER W3AF", string.Format("JobID: {0} XML PARSE - VULNERABILITY - ERROR : [{1}] ", m_jobId, NodeIter1.Current.Value));
                                    break;
                            }
                            list_parse.Add((string)NodeIter1.Current.Value);
                        };
                    }
                    catch (System.Exception ex)
                    {
                        Utils.Helper_Trace("XORCISM PROVIDER W3AF", "JobID:" + m_jobId + "Exception Parsing XML PLUGIN'S = " + ex.Message + " " + ex.InnerException);
                    }
                }
                catch (System.Exception ex)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER W3AF", "JobID:" + m_jobId + "Exception LOADING XML = " + ex.Message + " " + ex.InnerException);
                }
                Utils.Helper_Trace("XORCISM PROVIDER W3AF", string.Format("JobID: {0} XML PARSE successfull for file : [{1}] ", m_jobId, localOutputFile));

                //aff_list();
                // Pause
                Console.ReadLine();

               /* sshShell.Close();
                sshShell = null;*/
                return true;
            }

            public void create_Script()
            {
                // ==========================================
                // We can create the script TO SCAN WITH W3AF
                // ==========================================
                try
                {
                    w3afScript = new W3afScript(m_policy, m_strategy, m_target, m_file);
                }
                catch (Exception ex)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER W3AF", "JobID:" + m_jobId + "Exception w3afScript = " + ex.Message + " " + ex.InnerException);
                }

                //w3afScript.aff_script();
            }

            // Accessor for script
            public W3afScript getInstanceOfScript()
            {
                return w3afScript;
            }

            // show content's list
            public void aff_list()
            {
                int i = 0;

                while (i < list_parse.Count)
                {
                    Console.WriteLine("XORCISM PROVIDER W3AF Result index : {0} => [{1}] ", i, list_parse[i]);
                    i++;
                }
            }

            void ftp_OnTransferEnd(string src, string dst, int transferredBytes, int totalBytes, string message)
            {
                Utils.Helper_Trace("XORCISM PROVIDER W3AF", "SFTP transfer finished");
            }

            void ftp_OnTransferProgress(string src, string dst, int transferredBytes, int totalBytes, string message)
            {

            }

            void ftp_OnTransferStart(string src, string dst, int transferredBytes, int totalBytes, string message)
            {
                Utils.Helper_Trace("XORCISM PROVIDER W3AF", "SFTP transfer started");
            }
        }



        // This class creates the script for W3af Parser and it's a way to modify variables in the scan
        class W3afScript
        {
            private string ms_policy;
            private string ms_strategy;
            private string ms_target;
            private string ms_xmlfile;
            private System.Collections.ArrayList script_List;

            // URL script File 
            string fileLoc = Path.GetTempPath() + string.Format("script_{0}.xml", DateTime.Now.Ticks);

            // Constructor
            /* It's easy to pass more parameters in order to make some changes */
            public W3afScript(string policy, string strategy, string target, string xmlfile)
            {
                ms_policy = policy;
                ms_strategy = strategy;
                ms_target = target;
                ms_xmlfile = xmlfile;

                // La bidouille du Siou ! A little french joke !
                script_List = new System.Collections.ArrayList();

                // Here we can make change with policy and strategy
                // Not implemented Yet

                // Beginning of the script 
                //HARDCODED
                script_List.Add("plugins");
                script_List.Add("output xmlFile");
                script_List.Add("output");
                script_List.Add("output config xmlFile");
                script_List.Add("set fileName " + ms_xmlfile); // Put the xml file output
                script_List.Add("set verbose True");
                script_List.Add("back");

                // Discovery
                /* We can test what type of Discovery we want in order to optimize the scan */
                
                // Big internet application with no flash and/or no javascript
                /*
                 * I'm looking for a way to know waht type of web application we scan in order to choose !
                 */
                //script_List.Add("discovery all,!spiderMan, !fingerGoogle, !fingerMSN, !fingerPKS, !MSNSpider, !googleSpider, !phishtank, !googleSafeBrowsing");
                //script_List.Add("discovery");
                //script_List.Add("back");
                    // Big internet application with flash and/or javascript and no webservices
                    //script_List.Add("discovery all, !wordnet , !googleSets, !wsdlFinder");
                    //script_List.Add("discovery");
                    //script_List.Add("back");

                // !!!!!! For the moment ! until we find a way to know what type of internet application we scan
                //HARDCODED
                script_List.Add("discovery all,!spiderMan");
                script_List.Add("discovery");
                script_List.Add("back");

                // Audit
                //HARDCODED
                script_List.Add("audit all"); // A long time !
                script_List.Add("audit");
                script_List.Add("back");

                // Exploitation ! Not implemented Yet ! but it can ;-)

                // Target
                //HARDCODED
                script_List.Add("target");
                script_List.Add("set target " + ms_target); // Put the file target
                script_List.Add("back");

                // Go !
                script_List.Add("start");
            }

            // Create a real file script
            public String getScriptFile()
            {
                    using (StreamWriter sw = new StreamWriter(fileLoc))
                    {
                        for (int i = 0; i < this.script_List.Count; i++)
                        {
                            sw.Write(script_List[i] + "\n");
                            Utils.Helper_Trace("XORCISM PROVIDER W3AF", string.Format("CREATING SCRIPT FILE: {0} ", script_List[i]));
                        }
                    }

                return fileLoc;
            }

            // show content's list
            public void aff_script()
            {
                int i = 0;

                while (i < script_List.Count)
                {
                    Console.WriteLine("XORCISM PROVIDER W3AF Result index : {0} => [{1}] ", i, script_List[i]);
                    i++;
                }
            }

            // Accessors
            public System.Collections.ArrayList getAlist()
            {
                return script_List;
            }

            
        }
    }


