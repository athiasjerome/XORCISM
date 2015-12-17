using System;
using System.Net;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
//using System.Xml;
//using System.Xml.Serialization;
//using System.Xml.XPath;
using System.Configuration;
using System.ComponentModel;

using XORCISMModel;
using XCommon;
using XProviderCommon;

using System.Text.RegularExpressions;

//using FSM.DotNetSSH;
using System.Threading;
using System.Runtime;

namespace XProviderSandcat
{
    /// <summary>
    /// Copyright (C) 2012-2015 Jerome Athias
    /// XORCISM Plugin for sandcat
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
            //  on utilise la variable strategy pour cibler notre action (Tuning)
            Engine eng = new Engine();       
            eng.Run("www.google.fr", 1, "1", "0");
        }
        // Fin partie à virer par la suite 
    }
    */
    /*
    I:\0LOGICIELS\0SECURITE\0ATTAQUE\0WEB\sandcatmini-4.2.5.0>SandcatCS.exe

SANDCATMINI 4.2.5.0 (c) 2011 Syhunt Cyber-Security Company
Type SandcatCS -about for more information.
________________________________________________________________

Usage: SandcatCS [target] [optional params]
Examples:
    SandcatCS www.somehost.com
    SandcatCS www.somehost.com:8080
    (if a port is not specified, 80 will be assigned.)

Optional parameters:
-sn:[session name]  (if not used, "[unixtime]-[target]" will be assigned)
-hm:[method name]   Hunt Method (if not used, "compndos" will be assigned
    Available Methods:
    appscan   (or as)   Web Application Scan; Gray Box
    structbf  (or sbf)  Web Structure Brute Force; Black Box
    codescan  (or cs)   Source Code Scan; White Box
    phptop5             OWASP PHP Top 5; Gray Box
    faultinj  (or fi)   Fault Injection; Gray Box
    sqlinj    (or sqli) SQL Injection; Gray Box
    xss                 Cross-Site Scripting; Gray Box
    servscan  (or ss)   Common Web Server Scan; Black Box
    top20     (or t20)  SANS Top 20; Black Box
    spider    (or spd)  Spider Only
    complete  (or cmp)  Complete Scan; Gray Box
    compnodos (or cnd)  Complete Scan, No DoS; Gray Box
    comppnoid (or cpn)  Complete Scan, Paranoid; Gray box

-emu:[browser name] Browser Emulation Mode (default: msie)
    Available Modes:
    chrome    (or c)    Google Chrome
    firefox   (or ff)   Mozilla Firefox
    msie      (or ie)   Internet Explorer
    opera     (or o)    Opera
    safari    (or s)    Safari

-surl:[path]        Sets a Start URL (eg. /index.php, if not used "/" will be as
signed)
-uf                 Ultra fast scan
-mnl:[n]            Sets the maximum number of links per server (default: 10000)

-mnr:[n]            Sets the maximum number of retries (default: 2)
-maxdepth:[n]       Sets the maximum crawling depth (default: unlimited)
-tmo:[ms]           Sets the timeout time (default: 8000)
-bb                 Enables the Sandcat WebDiver Browser Bot (Beta)
-def                Loads the default settings (ignores the settings from the cu
rrent Sandcat installation)
-rls                Remembers the last web structure of the scanned host
-ver:[v]            Sets the HTTP Version (default: 1.1)
-srcdir:[local dir] Sets a Target Code Folder (eg. "C:\www\docs\")
-evid               Enables the IDS Evasion
-evaf               Enables the WAF Evasion

Other parameters:
-nomc               Disables multi-core support
-nort               Disables request retries (in case of timeout)
-nojs               Disables JavaScript emulation and execution
-nogz               Disables GZIP compression support
-noka               Disables Keep-Alive
-nodos              Disables Denial-of-Service tests
-user:[username]    Sets a username for basic server authentication
-pass:[password]    Sets a password for basic server authentication
-wuser:[username]   Sets a username for web form authentication
-wpass:[password]   Sets a password for web form authentication
-clses              Clears all Sandcat sessions from the current Sandcat install
ation (asks confirmation)
-about              Displays information on the current version of Sandcat
-help (or /?)       Displays this list
    */
    public class Engine : MarshalByRefObject, XProviderCommon.IVulnerabilityDetector
    {
        private string m_data = string.Empty;
        private const int ERROR_FILE_NOT_FOUND = 2;
        private const int ERROR_ACCESS_DENIED = 5;

        public Engine()
        {
            TextWriterTraceListener tw;
            tw = new TextWriterTraceListener("XProviderSandcat.log");   //Hardcoded
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
            Utils.Helper_Trace("XORCISM PROVIDER SANDCAT", "Entering Run()");
            Utils.Helper_Trace("XORCISM PROVIDER SANDCAT", string.Format("Target = {0} , JobID = {1} , Policy = {2}, Strategy = {3}", target, jobID, policy, strategy));
            string targetmodified = target.ToLower().Replace("https://", "").Replace("http://", "");
            XORCISMEntities model = new XORCISMEntities();

            /* On initialise une var */
            //SandcatParser sandcatParser = null;
            string file="";
            Assembly a;
            a = Assembly.GetExecutingAssembly();
            file = string.Format("sandcat_{0}_{1}", DateTime.Now.Ticks, this.GetHashCode());
            Process process = new Process();
            try
            {
                //sandcatParser = new SandcatParser(target, jobID, policy, strategy);
                
                Utils.Helper_Trace("XORCISM PROVIDER SANDCAT", "SANDCAT Assembly location = " + a.Location);

                Utils.Helper_Trace("XORCISM PROVIDER SANDCAT", string.Format("JobID: {0} Results will be stored in directory [{1}]", jobID, file));

                string program;
                program = Path.GetDirectoryName(a.Location) + "\\sandcatmini-4.2.5.0\\SandcatCS.exe";   //HARDCODED

                Utils.Helper_Trace("XORCISM PROVIDER SANDCAT", string.Format("Using sandcat at '{0}'", program));

                process.StartInfo.UseShellExecute = true;

                try
                {
                    process.StartInfo.FileName = program;
                    process.StartInfo.Arguments = " " + targetmodified + " -sn " + file;    //HARDCODED
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = false;
                    process.StartInfo.CreateNoWindow = true;
                    // process.EnableRaisingEvents = true;
                    // process.Exited += new EventHandler(Process_Exited);
                    process.Start();
                    // Process.Start(vProgram,vIAnnotationLocal.Folder + vIAnnotationLocal.EntryPoint);
                }
                catch (Win32Exception vException)
                {
                    if (vException.NativeErrorCode == ERROR_FILE_NOT_FOUND)
                    {
                        Utils.Helper_Trace("XORCISM PROVIDER SANDCAT", string.Format("ERROR_FILE_NOT_FOUND : Exception = {0}", vException.Message));
                        //return null;
                    }
                    else if (vException.NativeErrorCode == ERROR_ACCESS_DENIED)
                    {
                        Utils.Helper_Trace("XORCISM PROVIDER SANDCAT", string.Format("ERROR_ACCESS_DENIED : Exception = {0}", vException.Message));
                        //return null;
                    }
                }
                catch (Exception ex)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER SANDCAT", "JobID:" + jobID + "Exception RunningSandcat = " + ex.Message + " " + ex.InnerException);
                }

                Utils.Helper_Trace("XORCISM PROVIDER SANDCAT", string.Format("sandcat is running"));
            }
            catch (Exception ex)
            {
                Utils.Helper_Trace("XORCISM PROVIDER SANDCAT", "JobID:" + jobID + "Exception SandcatParser = " + ex.Message + " " + ex.InnerException);
            }

            try
            {
                Utils.Helper_Trace("XORCISM PROVIDER SANDCAT", string.Format("Waiting for sandcat to finish"));

                process.WaitForExit(1800000);    //3 hours
            }
            catch (Exception vException)
            {
                Utils.Helper_Trace("XORCISM PROVIDER SANDCAT", string.Format("TimeException = {0}", vException.Message));
                //return null;
            }

            Utils.Helper_Trace("XORCISM PROVIDER SANDCAT", "sandcat has finished");
            StreamReader SR = process.StandardOutput;
            string strOutput = SR.ReadToEnd();
            Utils.Helper_Trace("XORCISM PROVIDER SANDCAT", string.Format("Output: " + strOutput));

            //demo.testfire.net [80]_Vulns.log
            string resultfile;
            resultfile = Path.Combine(Path.GetDirectoryName(a.Location), "\\sandcatmini-4.2.5.0\\Rep\\" + file + "\\" + targetmodified + " [80]_Vulns.log");    //HARDCODED
            /*
            "vname=search.aspx XSS",vpars=txtSearch,vlns=,vrisk=Medium,vpath=http://demo.testfire.net/search.aspx?txtSearch=[script]alert('Vulnerable')[/script],vstat=200,"f=Application Vuln.xrm"
            "vname=comment.aspx XSS",vpars=name,vlns=,vrisk=Medium,"vpath=http://demo.testfire.net/comment.aspx?cfile=comments.txt&name=[script]alert('Vulnerable')[/script]&email_addr=&subject=Sandcat&comments=&submit= Submit&reset= Clear Form",vstat=200,"f=Application Vuln (2).xrm"
            "vname=login.aspx XSS",vpars=uid,vlns=,vrisk=Medium,vpath=http://demo.testfire.net/bank/login.aspx?uid=[script]alert(document.cookie)[/script]&passw=&btnSubmit=Login,vstat=200,"f=Application Vuln (3).xrm"
            "vname=default.aspx Directory Traversal",vpars=content,vlns=,vrisk=High,vpath=http://demo.testfire.net/default.aspx?content=../../../../../../../../boot.ini%00inside_contact.htm,vstat=200,"f=Application Vuln (4).xrm"
            */

                
            try
            {
                StreamReader myfilereader = new StreamReader(resultfile);
                string ligne = myfilereader.ReadLine();
                while (ligne != null)
                {
                    Console.WriteLine(ligne);

                    VulnerabilityFound vulnerabilityFound = new VulnerabilityFound();
                    VulnerabilityEndPoint vulnerabilityEndPoint = new VulnerabilityEndPoint();

                    vulnerabilityFound.InnerXml = ligne;

                    vulnerabilityEndPoint.IpAdress = target;
                    vulnerabilityEndPoint.Port=80;  //TODO: à voir
                    vulnerabilityEndPoint.Protocol="TCP";   //HARDCODED
                    vulnerabilityEndPoint.Service="WWW";

                    string[] arInfo = new string[7];    
                    char[] splitter  = {','};            
                    arInfo = ligne.Split(splitter);
                    for (int x = 0; x < arInfo.Length; x++)
                    {
                        if (arInfo[x].Contains("vname"))
                        {
                            vulnerabilityFound.Title = arInfo[x].Replace("vname=", "").Replace("\"", "");
                        }
                        else
                        {
                            if (arInfo[x].Contains("vpars"))
                            {
                                vulnerabilityFound.VulnerableParameter = arInfo[x].Replace("vpars=", "").Replace("\"", "");
                            }
                            else
                            {
                                if (arInfo[x].Contains("vrisk"))
                                {
                                    vulnerabilityFound.Severity = arInfo[x].Replace("vrisk=", "").Replace("\"", "");
                                }
                                else
                                {
                                    if (arInfo[x].Contains("vpath"))
                                    {
                                        vulnerabilityFound.Url = arInfo[x].Replace("vpath=", "").Replace("\"", "");
                                    }
                                }
                            }
                        }
                    }
                    VulnerabilityPersistor.Persist(vulnerabilityFound, vulnerabilityEndPoint, jobID, "sandcat", model);

                    ligne = myfilereader.ReadLine();
                }
                myfilereader.Close();
            }
            catch (Exception ex)
            {
                Utils.Helper_Trace("XORCISM PROVIDER SANDCAT", "JobID:" + jobID + "Exception SandcatReader = " + ex.Message + " " + ex.InnerException + " " + resultfile);
            }
            

            string status = XCommon.STATUS.FINISHED.ToString();
            
            // =================================================
            // Change the status of the job to FINISHED or ERROR
            // =================================================
            /*
            if (sandcatParser.Parse() == false)
            {
                status = XCommon.STATUS.ERROR.ToString();
                Utils.Helper_Trace("XORCISM PROVIDER SANDCAT", string.Format("Updating job {0} status to ERROR", jobID));
                XCommon.Utils.Helper_SendEmail("contact@hackenaton.org", "Sandcat ERROR", "Sandcat ERROR for job:" + jobID);
            }
            else
            {
                Utils.Helper_Trace("XORCISM PROVIDER SANDCAT", string.Format("Updating job {0} status to FINISHED", jobID));
            }
            */
            try
            {
                
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
            //    sandcatParser = null;
            }
            catch (Exception ex)
            {
                Utils.Helper_Trace("XORCISM PROVIDER SANDCAT", "JobID:" + jobID + "Exception UpdateJob = " + ex.Message + " " + ex.InnerException);
            }

            Utils.Helper_Trace("XORCISM PROVIDER SANDCAT", "JobID:" + jobID + "Leaving Run()");
        }
    }

    class SandcatParser
    {
        private string m_target;
        private int m_jobId;
        private string m_policy;
        private string m_strategy;
        private string m_data = string.Empty;
        private XORCISMEntities m_model;
        
        public SandcatParser(string target, int jobId, string policy, string strategy)
        {
            m_jobId = jobId;
            m_target = target;
            m_policy = policy;
            m_strategy = strategy;
            m_model = new XORCISMEntities();
        }

        public bool Parse()
        {
            



            return true;
        }

    }
}
