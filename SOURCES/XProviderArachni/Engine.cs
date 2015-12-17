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

namespace XProviderArachni
{
    /// <summary>
    /// Copyright (C) 2012-2015 Jerome Athias (and a friend, youknowwhoyouare ;p)
    /// XORCISM Plugin for Arachni (Import a result file in an XORCISM database)
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
        //TODO Partie à virer par la suite le run est independant ^^ 
        static void Main(string[] args)
        {
            Engine eng = new Engine();
            eng.Run("www.google.fr", 1, "1", "nop");
        }
         //Fin partie à virer par la suite
    }
    */
    public class Engine : MarshalByRefObject, XProviderCommon.IVulnerabilityDetector
    {
        private string m_data = string.Empty;

        public Engine()
        {
            TextWriterTraceListener tw;
            tw = new TextWriterTraceListener("XProviderArachni.log");   //Hardcoded
            Trace.Listeners.Add(tw);
            Trace.AutoFlush = true;
            Trace.IndentSize = 4;
        }

        public override object InitializeLifetimeService()
        {
            return null; //Allow infinite lifetime
        }

        
        /* Choix de typage pour la var. policy
         * j'ai laissé en string plutot que int car en fait il y a la possibilité de passer aussi des chaines de char : "aggressive"... 
         */ 
        public void Run(string target, int jobID, string policy, string strategy)
        {
            Utils.Helper_Trace("XORCISM PROVIDER ARACHNI", "Entering Run()");
            Utils.Helper_Trace("XORCISM PROVIDER ARACHNI", string.Format("Target = {0} , JobID = {1} , Policy = {2}", target, jobID, policy));

           /* On initialise une var */
            //ArachniParser arachniParser = new ArachniParser("www.google.fr", 1, "1", "nop");
            ArachniParser arachniParser = null;
            Utils.Helper_Trace("XORCISM PROVIDER ARACHNI", "initializing");
            try
            {
                arachniParser = new ArachniParser(target, jobID, policy, strategy);
            }
            catch (Exception ex)
            {
                Utils.Helper_Trace("XORCISM PROVIDER ARACHNI", "JobID:" + jobID + "Exception ArachniParser = " + ex.Message + " " + ex.InnerException);
            }

            string status = XCommon.STATUS.FINISHED.ToString();

            // =================================================
            // Change the status of the job to FINISHED or ERROR
            // =================================================

            if (arachniParser.Parse() == false)
            {
                status = XCommon.STATUS.ERROR.ToString();
                Utils.Helper_Trace("XORCISM PROVIDER ARACHNI", string.Format("Updating job {0} status to ERROR", jobID));
                XCommon.Utils.Helper_SendEmail("contact@hackenaton.org", "Arachni ERROR", "Arachni ERROR for job:" + jobID);
            }
            else
            {
                Utils.Helper_Trace("XORCISM PROVIDER ARACHNI", string.Format("Updating job {0} status to FINISHED", jobID));
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


                arachniParser = null;
            }
            catch (Exception ex)
            {
                Utils.Helper_Trace("XORCISM PROVIDER ARACHNI", "JobID:" + jobID + "Exception UpdateJob = " + ex.Message + " " + ex.InnerException);
            }

            Utils.Helper_Trace("XORCISM PROVIDER ARACHNI", "JobID:" + jobID + "Leaving Run()");
        }
    }

    class ArachniParser
    {
        private string m_target;
        private XORCISMEntities m_model;
        private int m_jobId;
        private string m_policy;
        private string m_data = string.Empty;

        // A way to save XML parsing
        System.Collections.ArrayList list_parse = new System.Collections.ArrayList();

        public ArachniParser(string target, int jobId, string policy, string strategy)
        {
            m_jobId = jobId;
            m_target = target;
            m_model = new XORCISMEntities();
            m_policy = policy;
        }

        public bool Parse()
        {
            Utils.Helper_Trace("XORCISM PROVIDER ARACHNI", "Parse()");
            Assembly a;
            /* A way for loading XMLfile */
            XPathNavigator nav;
            XPathNavigator nav1;
            XPathDocument docNav;
            XPathNodeIterator NodeIter1;
            String strExpression1;


            a = Assembly.GetExecutingAssembly();
            Utils.Helper_Trace("XORCISM PROVIDER ARACHNI", "ARACHNI Assembly location = " + a.Location);


            /* Name of XML result */
            string file;
            file = string.Format("result_{0}_{1}.xml", DateTime.Now.Ticks, this.GetHashCode());

            Utils.Helper_Trace("XORCISM PROVIDER ARACHNI", string.Format("JobID: {0} Results will be stored in file [{1}]", m_jobId, file));


            /* SSH instructions & declarations */
            //HARDCODED
            int port=22;
            string address, username, password;
            string prompt;
            
            address = "192.168.1.2";    //TODO Hardcoded
                username = "root";
                password = "toor";
                prompt = "root";//@backtrack:"; //Kali...

            SshShell sshShell;
            sshShell = new SshShell(address, username, password);
            sshShell.RemoveTerminalEmulationCharacters = true;

            Utils.Helper_Trace("XORCISM PROVIDER ARACHNI", string.Format("JobID: {0} Connecting to ARACHNI server at {1}", m_jobId, address));
            
            try{
                sshShell.Connect(port);
                //sshShell.Expect(prompt+"~#");
                sshShell.Expect(prompt);// + "~$");
            }
            catch (Exception ex)
            {
                Utils.Helper_Trace("XORCISM PROVIDER ARACHNI", string.Format("JobID: {0} ConnectingERROR to ARACHNI server at {1} : " + ex.Message + " " + ex.InnerException, m_jobId, address));
                address = "192.168.1.2";    //TODO hardcoded
                username = "root";
                password = "toor";
                prompt = "root";//@backtrack:";
                sshShell = new SshShell(address, username, password);
                sshShell.RemoveTerminalEmulationCharacters = true;
                
                Utils.Helper_Trace("XORCISM PROVIDER ARACHNI", string.Format("JobID: {0} Connecting to ARACHNI server at {1}", m_jobId, address));
                try
                {
                    sshShell.Connect(port);
                    sshShell.Expect(prompt);// + "~$");
                }
                catch (Exception ex2)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER ARACHNI", string.Format("JobID: {0} ConnectingERROR to ARACHNI server at {1} : "+ex2.Message+" "+ex2.InnerException, m_jobId, address));
                }
            }
            Utils.Helper_Trace("XORCISM PROVIDER ARACHNI", string.Format("JobID: {0} Successfully connected to ARACHNI server", m_jobId));

            //string output;
            string stdout = "";
            //string stderr = "";

            /* Command 1 */
            string cmd1;

            /* See for provider m_model */

            /* For an URL */
            cmd1 = string.Format("arachni {1} --report='xml:outfile={2}'", m_policy, m_target, file);

            Utils.Helper_Trace("XORCISM PROVIDER ARACHNI", string.Format("JobID: {0} Executing command [{1}]", m_jobId, cmd1));

            sshShell.WriteLine(cmd1);
            stdout = sshShell.Expect(prompt);
            
            Utils.Helper_Trace("XORCISM PROVIDER ARACHNI", string.Format("JobID: {0} START DUMP STDOUT01", m_jobId));
            Utils.Helper_Trace("XORCISM PROVIDER ARACHNI", stdout);


            string localOutputFile;
            localOutputFile = Path.GetTempFileName();

            // HACK :
            // outputfile = "634244542240861588_39608125_output";

            Utils.Helper_Trace("XORCISM PROVIDER ARACHNI", string.Format("Downloading results via SFTP to [{0}]", localOutputFile));

            try
            {
                Sftp ftp;
                ftp = new Sftp(address, username, password);
                ftp.OnTransferStart += new FileTransferEvent(ftp_OnTransferStart);
                ftp.OnTransferProgress += new FileTransferEvent(ftp_OnTransferProgress);
                ftp.OnTransferEnd += new FileTransferEvent(ftp_OnTransferEnd);

                ftp.Connect(port);

                ftp.Get("/" + file, localOutputFile);

                ftp.Close();
            }
            catch (Exception ex)
            {
                Utils.Helper_Trace("XORCISM PROVIDER ARACHNI", string.Format("Exception = {0} / {1}", ex.Message, ex.InnerException == null ? "" : ex.InnerException.Message));
                return false;
            }

            Utils.Helper_Trace("XORCISM PROVIDER ARACHNI", "Loading the xml document");


            /* SAMPLE of XML structure 
             * 
             *  <?xml version="
             *  <arachni_report>
             *        <title>...</title>
             *        <generated_on>...</generated_on>
             *        <report_false_positives>http://github.com/Zapotek/arachni/issues</report_false_positives>
             *        <system>
             *          <version...</version>
             *          <revision>...</revision>
             *          <start_datetime>...</start_datetime>
             *          <finish_datetime>...</finish_datetime>
             *          <delta_time>...</delta_time>
             *          <url>...</url>
             *          <user_agent>...</user_agent>
             *          <audited_elements>
             *            <element>...</element>
             *          </audited_elements>
             *          <modules>
             *           <module name="..."/>
             *          </modules>
             *          <filters>
             *            <exclude>
             *            </exclude>
             *            <include>
             *              <regexp>...</regexp>
             *            </include>
             *            <redundant>
             *            </redundant>
             *          </filters>
             *          <cookies>
             *            <cookie name="..." value="..." />
             *          </cookies>
             *        </system>
             *        <issues>
             *          <issue>
             *            <name>...</name>
             *            <url>...</url>
             *            <element>...</element>
             *            <method>...</method>
             *            <tags>
             *             <tag name="..." />
             *           </tags>
             *           <variable>..</variable>
             *            <description>...</description>
             *            <manual_verification...</manual_verification>
             *            <references>
             *              <reference name="..." url="..." />
             *            </references>
             *            <variations>
             *              <variation>
             *                <url>...</url>
             *                <injected>...</injected>
             *                <regexp_match>...</regexp_match>
             *                <headers>
             *                  <request>
             *                    <field name="..." value="..." />
             *                  </request>
             *                  <response>
             *                    <field name="..." value="..." />
             *                  </response>
             *                </headers>
             *                <html>...</html>
             *             <variation>
             *           <variations>
             *      ...
             *  </arachni_report>
            */

            try
            {
                docNav = new XPathDocument(localOutputFile); // for test : result_634521969362210000_41014879.xml || URL file : file
                nav = docNav.CreateNavigator();
                nav1 = docNav.CreateNavigator();
                // If all is OK!
                Utils.Helper_Trace("XORCISM PROVIDER ARACHNI", string.Format("JobID: {0} Successfully loaded XML file : [{1}] ", m_jobId, file));

                // Go to parse
                try
                {
                    // For read all plugin's
                    strExpression1 = "/arachni_report/title | /arachni_report/generated_on | /arachni_report/report_false_positives | /arachni_report/system/start_datetime | /arachni_report/system/finish_datetime | /arachni_report/system/delta_time | /arachni_report/system/url | /arachni_report/system/audited_elements/element | /arachni_report/issues/issue/name | /arachni_report/issues/issue/url | /arachni_report/issues/issue/element | /arachni_report/issues/issue/method | /arachni_report/issues/issue/tags/tag/@name | /arachni_report/issues/issue/variable | /arachni_report/issues/issue/description | /arachni_report/issues/issue/manual_verification | /arachni_report/issues/issue/references/reference/@name | /arachni_report/issues/issue/references/reference/@url | /arachni_report/issues/issue/variations/variation/url | /arachni_report/issues/issue/variations/variation/injected | /arachni_report/issues/issue/variations/variation/regexp_match | /arachni_report/issues/issue/variations/variation/headers/request/field/@name | /arachni_report/issues/issue/variations/variation/headers/request/field/@value | /arachni_report/issues/issue/variations/variation/headers/response/field/@value | /arachni_report/issues/issue/variations/variation/headers/response/field/@name | /arachni_report/issues/issue/variations/variation/html"; 
                    NodeIter1 = nav1.Select(strExpression1);
                    while (NodeIter1.MoveNext())
                    {
                        switch ((string)NodeIter1.Current.Name)
                        {
                            case "title":
                                Utils.Helper_Trace("XORCISM PROVIDER ARACHNI", string.Format("JobID: {0} XML PARSE - TITLE : [{1}] ", m_jobId, NodeIter1.Current.Value));
                                break;
                            case "generated_on":
                                Utils.Helper_Trace("XORCISM PROVIDER ARACHNI", string.Format("JobID: {0} XML PARSE - GENERATED-TIME : [{1}] ", m_jobId, NodeIter1.Current.Value));
                                break;
                            case "report_false_positives":
                                Utils.Helper_Trace("XORCISM PROVIDER ARACHNI", string.Format("JobID: {0} XML PARSE - REPORT : [{1}] ", m_jobId, NodeIter1.Current.Value));
                                break;
                            case "start_datetime":
                                Utils.Helper_Trace("XORCISM PROVIDER ARACHNI", string.Format("JobID: {0} XML PARSE - START-TIME : [{1}] ", m_jobId, NodeIter1.Current.Value));
                                break;
                            case "finish_datetime":
                                Utils.Helper_Trace("XORCISM PROVIDER ARACHNI", string.Format("JobID: {0} XML PARSE - FINISH-TIME : [{1}] ", m_jobId, NodeIter1.Current.Value));
                                break;
                            case "delta_time":
                                Utils.Helper_Trace("XORCISM PROVIDER ARACHNI", string.Format("JobID: {0} XML PARSE - DELTA-TIME : [{1}] ", m_jobId, NodeIter1.Current.Value));
                                break;
                            case "element":
                                Utils.Helper_Trace("XORCISM PROVIDER ARACHNI", string.Format("JobID: {0} XML PARSE - ELEMENT : [{1}] ", m_jobId, NodeIter1.Current.Value));
                                break;
                            case "name":
                                Utils.Helper_Trace("XORCISM PROVIDER ARACHNI", string.Format("JobID: {0} XML PARSE - ISSUE-NAME : [{1}] ", m_jobId, NodeIter1.Current.Value));
                                break;
                            case "url":
                                Utils.Helper_Trace("XORCISM PROVIDER ARACHNI", string.Format("JobID: {0} XML PARSE - ISSUE-URL : [{1}] ", m_jobId, NodeIter1.Current.Value));
                                break;
                            case "method":
                                Utils.Helper_Trace("XORCISM PROVIDER ARACHNI", string.Format("JobID: {0} XML PARSE - ISSUE-METHOD : [{1}] ", m_jobId, NodeIter1.Current.Value));
                                break;
                            case "modules":
                                Utils.Helper_Trace("XORCISM PROVIDER ARACHNI", string.Format("JobID: {0} XML PARSE - MODULE : [{1}] ", m_jobId, NodeIter1.Current.Value));
                                break;
                        }
                        list_parse.Add((string)NodeIter1.Current.Value);
                    };
                }
                catch (System.Exception ex)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER ARACHNI", "JobID:" + m_jobId + "Exception Parsing XML PLUGIN'S = " + ex.Message + " " + ex.InnerException);
                }
            }
            catch (System.Exception ex)
            {
                Utils.Helper_Trace("XORCISM PROVIDER ARACHNI", "JobID:" + m_jobId + "Exception LOADING XML = " + ex.Message + " " + ex.InnerException);
            }
            Utils.Helper_Trace("XORCISM PROVIDER ARACHNI", string.Format("JobID: {0} XML PARSE successfull for file : [{1}] ", m_jobId, file));

            aff_list();
            // Pause
            Console.ReadLine();

            sshShell.Close();
            sshShell = null;
            return true;
        }

        // show content's list
        public void aff_list()
        {
            int i = 0;

            while (i < list_parse.Count)
            {
                Console.WriteLine("XORCISM PROVIDER ARACHNI Result index : {0} => [{1}] ", i, list_parse[i]);
                i++;
            }
        }

        void ftp_OnTransferEnd(string src, string dst, int transferredBytes, int totalBytes, string message)
        {
            Utils.Helper_Trace("XORCISM PROVIDER ARACHNI", "SFTP transfer finished");
        }

        void ftp_OnTransferProgress(string src, string dst, int transferredBytes, int totalBytes, string message)
        {

        }

        void ftp_OnTransferStart(string src, string dst, int transferredBytes, int totalBytes, string message)
        {
            Utils.Helper_Trace("XORCISM PROVIDER ARACHNI", "SFTP transfer started");
        }

    }
}
