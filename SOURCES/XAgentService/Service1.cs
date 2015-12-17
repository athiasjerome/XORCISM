using System;
using System.Net;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.ServiceModel;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.Xml;
using System.IO;

using XCommon;
using XPluginFramework;
using XORCISMModel;
using XProviderCommon;

using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

//using FSM.DotNetSSH;
 
namespace XAgentService
{
    /// <summary>
    /// Copyright (C) 2012-2015 Jerome Athias
    /// XORCISM Agent Service: Microsoft Windows service dealing with Tasks/Sessions/Jobs from an XORCISM database and interacting with various Tools/Services...
    /// NOTE: completely dirty and insecure
    /// This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation; either version 2 of the License, or (at your option) any later version.
    /// 
    /// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
    /// 
    /// You should have received a copy of the GNU General Public License along with this program; if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
    /// </summary>
    public class Service1 : IService1
    {
        private static Dictionary<int, ThreadContext> g_MapThread = new Dictionary<int, ThreadContext>();
        private static PluginManager pluginManager = new PluginManager("Plugins");  //Plugins are "connectors" for various Tools
        //private static MarshalByRefObject o;   //http://msdn.microsoft.com/en-us/magazine/cc300474.aspx
        private static List<string> pluginsList=new List<string>();

        private class ThreadContext
        {
            public Guid     UserID;
            public int      JobID;
            public Thread   Thread;

            public ThreadContext(Guid userID, int jobID)
            {
                this.UserID = userID;
                this.JobID  = jobID;
            }
        }

        public void LaunchJob(Guid userID, int jobID)
        {
            Utils.Helper_Trace("AGENT SERVICE", "Entering LaunchJob()");

            ThreadContext threadContext;
            threadContext = new ThreadContext(userID, jobID);

            Thread tJobThread;
            tJobThread = new Thread(new ParameterizedThreadStart(Helper_TreadFunc));
            threadContext.Thread = tJobThread;
            try
            {
                //tJobThread.IsBackground = true;//test
                tJobThread.Start(threadContext);
            }
            catch (ThreadStateException tse)
            {
                Utils.Helper_Trace("AGENT SERVICE", string.Format("Error in LaunchJob. ThreadStateException = {0} {1}", tse.Message, tse.InnerException));
            }
            catch (Exception tse)
            {
                Utils.Helper_Trace("AGENT SERVICE", string.Format("Error in LaunchJob. Exception = {0} {1}", tse.Message, tse.InnerException));
            }

            g_MapThread.Add(jobID, threadContext);

            Utils.Helper_Trace("AGENT SERVICE", "Leaving LaunchJob()");
        }

        public bool CancelJob(int jobID)
        {
            Utils.Helper_Trace("AGENT SERVICE", "Entering CancelJob()");

            if (g_MapThread.ContainsKey(jobID) == false)
            {
                Utils.Helper_Trace("AGENT SERVICE", string.Format("Job {0} has not been found in map", jobID));

                try
                {
                    //Updating the status of the Running Jobs
                    Utils.Helper_Trace("AGENT SERVICE", "Updating the status of the Running Jobs...");
                    XORCISMEntities model;
                    model = new XORCISMEntities();
                    string Status = XCommon.STATUS.RUNNING.ToString();

                    var myRunningJobs = from rj in model.JOB
                                        where rj.JobID == jobID && rj.Status == Status
                                        select rj;

                    foreach (JOB J in myRunningJobs.ToList())
                    {
                        J.Status = XCommon.STATUS.CANCELED.ToString();
                        J.DateEnd = DateTimeOffset.Now;

                        model.SaveChanges();
                        Utils.Helper_Trace("AGENT SERVICE", string.Format("Job {0} canceled", J.JobID));
                    }
                    model.Dispose();
                    Utils.Helper_Trace("AGENT SERVICE", "Status updated");
                }
                catch (Exception ex)
                {
                    Utils.Helper_Trace("AGENT SERVICE", string.Format("Error in CancelJob. Exception = {0} {1}", ex.Message, ex.InnerException));
                    return false;
                }

                Utils.Helper_Trace("AGENT SERVICE", "Leaving CancelJob()");
                return true;
            }
            else
            {
                try
                {
                    ThreadContext threadContext;
                    threadContext = g_MapThread[jobID];

                    threadContext.Thread.Abort();

                    g_MapThread.Remove(jobID);
                    
                }
                catch (Exception ex)
                {
                    Utils.Helper_Trace("AGENT SERVICE", string.Format("Error in CancelJob threadContext. Exception = {0} {1}", ex.Message, ex.InnerException));
                    return false;
                }
            }

            Utils.Helper_Trace("AGENT SERVICE", "Leaving CancelJob()");

            // Finished
            return true;
        }

        private void Helper_TreadFunc(object context)
        {
            try
            {
                Utils.Helper_Trace("AGENT SERVICE", "TRACE25");
            }
            catch (ThreadStartException tex)
            {
                Utils.Helper_Trace("AGENT SERVICE", string.Format("Error in Helper_TreadFunc. ThreadStartException = {0} {1}", tex.Message, tex.InnerException));
            }
            catch (Exception ex)
            {
                Utils.Helper_Trace("AGENT SERVICE", string.Format("Error in Helper_TreadFunc. Exception = {0} {1}", ex.Message, ex.InnerException));
                //return false;
            }
        
            try
            {
                ThreadContext threadContext;
                threadContext = (ThreadContext)context;

                Guid userID;
                userID = threadContext.UserID;

                int jobID;
                jobID = threadContext.JobID;

                Utils.Helper_Trace("AGENT SERVICE", string.Format("JOBID {0} : Entering thread", jobID));

                XORCISMEntities model;
                model = new XORCISMEntities();

                // Explicitly open the connection.    
                //model.Connection.Open();

//                PluginManager pluginManager;
                //pluginManager = new PluginManager("Plugins");

                // ===========
                // Get the job
                // ===========
                string provider;
                JOB myJob;
                try
                {
                    var J = from jobs in model.JOB
                            where jobs.JobID == jobID
                            select jobs;

                    myJob = J.ToList().First();
                    provider = myJob.PROVIDER.PluginReference;
                }
                catch (Exception ex)
                {
                    Utils.Helper_Trace("AGENT SERVICE", string.Format("JOBID {0} : Error getting the job : Exception =  {1}", jobID, ex.Message+" "+ex.InnerException));
                    return;
                }

                Utils.Helper_Trace("AGENT SERVICE", string.Format("{0} : JOBID {1} : Provider = {0}", provider, jobID));

                // ============================
                // Load the DLL of the provider
                // ============================
                string sPath = "";
                try
                {
                    sPath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                }
                catch (Exception ex)
                {
                    Utils.Helper_Trace("AGENT SERVICE", string.Format("{0} : JOBID {1} : Error locating AssemblyPathLocation : Exception =  {2}", provider, jobID, ex.Message+" "+ex.InnerException));
                    return;
                }

                var plugin = "";
                try
                {
                    plugin = (from job in model.JOB
                              where job.JobID == jobID
                              select job.PROVIDER.PluginReference).FirstOrDefault();
                    //TODO: some validation
                    Utils.Helper_Trace("AGENT SERVICE", string.Format("{0} : JOBID {1} : Loading plugin '{2}'", provider, jobID, plugin + ".dll"));
                }
                catch (Exception ex)
                {
                    Utils.Helper_Trace("AGENT SERVICE", string.Format("{0} : JOBID {1} : Error selecting plugin : Exception =  {2}", provider, jobID, ex.Message));
                    return;
                }
                //Debug
                /*
                foreach (string s in pluginsList)
                {
                    Utils.Helper_Trace("AGENT SERVICE", string.Format("{0} : Plugin loaded : {1}", provider, s));
                }
                */
                if (pluginsList.Contains(plugin))
                {
                    Utils.Helper_Trace("AGENT SERVICE", string.Format("{0} : JOBID {1} : Plugin already loaded : {2}", provider, jobID, plugin));
                }
                else
                {
                    try
                    {

                        //pluginManager.LoadUserAssembly(plugin + ".dll");
                        pluginManager.LoadUserAssembly(sPath + "\\Plugins\\" + plugin + ".dll");   //TODO Hardcoded
                        pluginsList.Add(plugin);
                        //Debug
                        /*
                        foreach (string s in pluginsList)
                        {
                            Utils.Helper_Trace("AGENT SERVICE", string.Format("{0} : New Plugin loaded : {1}", provider, s));
                        }
                        */
                        Utils.Helper_Trace("AGENT SERVICE", string.Format("{0} : JOBID {1} : Loading plugin successfully loaded : {2}", provider, jobID, plugin));

                    }
                    catch (Exception ex)
                    {
                        Utils.Helper_Trace("AGENT SERVICE", string.Format("{0} : JOBID {1} : Error loading plugin : Exception =  {2}", provider, jobID, ex.Message));
                        return;
                    }
                }
                
                // Debug
                /*
                Utils.Helper_Trace("AGENT SERVICE", string.Format("{0} : JOBID {1} : Enumering types in plugin", provider, jobID));
                
                foreach (string type in pluginManager.Types)
                {
                    Utils.Helper_Trace("AGENT SERVICE", string.Format("{0} : JOBID {1} :       Type = {2}", provider, jobID, type.ToString()));
                }
                */
                // ======================================
                // Create an instance of the Engine class
                // ======================================

                Utils.Helper_Trace("AGENT SERVICE", string.Format("{0} : JOBID {1} : Creating an instance of class {2}.Engine", provider, jobID, plugin));

                string typeName;
                typeName = plugin + ".Engine";  //Hardcoded

                MarshalByRefObject oMarshalObject;

                try
                {
                    oMarshalObject = pluginManager.CreateInstance(typeName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.CreateInstance, new object[] { });
                    
                }
                catch (Exception ex)
                {
                    Utils.Helper_Trace("AGENT SERVICE", string.Format("{0} : JOBID {1} : Error creating instance of class {2}.Engine : Exception =  {3}", provider, jobID, plugin, ex.Message));
                    return;
                }
                
                // =======================================
                // Change the status of the job to RUNNING
                // =======================================

                myJob.Status = XCommon.STATUS.RUNNING.ToString();
                model.SaveChanges();

                // ===============
                // Call the method
                // ===============

                //Get the parameters/values for the Job
                var q = from x in model.JOB
                        where x.JobID == jobID
                        select x.Parameters;

                Dictionary<string, object> parameters;
                byte[] buffer;
                MemoryStream ms;
                BinaryFormatter bf;
                try
                {
                    
                    buffer = q.First();

                    Utils.Helper_Trace("AGENT SERVICE", string.Format("{0} : JOBID {1} : Size of parameters = {2} bytes", provider, jobID, buffer.Length));

                    
                    ms = new MemoryStream(buffer);

                    
                    bf = new BinaryFormatter();

                    parameters = (Dictionary<string, object>)bf.Deserialize(ms);
                    
                }
                catch (Exception ex)
                {
                    Utils.Helper_Trace("AGENT SERVICE", string.Format("{0} : JOBID {1} : Error while deserializing parameters : Exception =  {2}", provider, jobID, ex.Message));
                    return;
                }

                foreach (string key in parameters.Keys)
                {
                    Utils.Helper_Trace("AGENT SERVICE", string.Format("{0} : Parameter key = {1}", provider, key));
                }

                Utils.Helper_Trace("AGENT SERVICE", string.Format("{0} : JOBID {1} : Calling the method", provider, jobID));

                //Run a malware scanner (against a website)
                #region MalwareDetect
                /*
                //TODO: Find providers
                if (oMarshalObject is IMalwareDetector)
                {
                    Utils.Helper_Trace("AGENT SERVICE", string.Format("{0} : JOBID {1} : Plugin immplements IMalwareDetector", provider, jobID));

                    IMalwareDetector IMyInterface;
                    IMyInterface = (IMalwareDetector)o;

                    // ===================
                    // Call the Run method
                    // ===================

                    //string policy;
                    //policy = (string)parameters["POLICY"];
                    int maxPageCount;
                    maxPageCount = (int)parameters["MaxPages"];
                    //TODO  ipaddressIPv4
                    var ipAddress = from job in model.JOB
                                    where job.JobID == jobID
                                    select job.ASSETSESSION.ASSET.ipaddressIPv4;
                    //TODO: IPv6, etc.
                    Utils.Helper_Trace("AGENT SERVICE", string.Format("{0} : IP Address = {1}", provider, ipAddress.First()));

                    try
                    {
                        IMyInterface.Run(ipAddress.First(), jobID, maxPageCount);
                        //IMyInterface.Run(ipAddress.First(), jobID, maxPageCount);
                    }
                    catch (ThreadAbortException ex)
                    {
                        Utils.Helper_Trace("AGENT SERVICE", string.Format("{0} : Error in IMalwareDetector.Run() : ThreadAbortException = {1} {2}", provider, ex.Message, ex.InnerException));

                        Utils.Helper_Trace("AGENT SERVICE", string.Format("{0} : Setting job {1} status to CANCELED", provider, jobID));
                        Helper_SetJobStatus(jobID, XCommon.STATUS.CANCELED);

                        Utils.Helper_Trace("AGENT SERVICE", string.Format("{0} : Leaving LaunchJob", provider));
                        return;
                    }
                    catch (Exception ex)
                    {
                        Utils.Helper_Trace("AGENT SERVICE", string.Format("{0} : Error in IMalwareDetector.Run() : Exception = {1} {2}", provider, ex.Message, ex.InnerException));

                        XCommon.Utils.Helper_SendEmail("contact@hackenaton.org", provider + " job=" + jobID + " Error in IMalwareDetector.Run", "MyException = " + ex.Message + " " + ex.InnerException);
                        Utils.Helper_Trace("AGENT SERVICE", string.Format("{0} : Setting job {1} status to ERROR", provider, jobID));
                        Helper_SetJobStatus(jobID, XCommon.STATUS.ERROR);

                        Utils.Helper_Trace("AGENT SERVICE", string.Format("{0} : Leaving LaunchJob", provider));
                        return;
                    }


                }
                
                */
                #endregion MalwareDetect

                //TODO
                //Use (external) services to identify if an Asset (e.g. website/domain) is in a Blacklist
                #region IBlacklistedDetector
                /*
                if (oMarshalObject is IBlacklistedDetector)
                {
                    Utils.Helper_Trace("AGENT SERVICE", string.Format("{0} : Plugin immplements IBlacklistedDetector", provider));

                    IBlacklistedDetector IMyInterface;
                    IMyInterface = (IBlacklistedDetector)o;

                    // ===================
                    // Call the Run method
                    // ===================

                    //string policy;
                    //policy = (string)parameters["POLICY"];

                    //Utils.Helper_Trace("AGENT SERVICE", string.Format("Policy = {0}", policy));
                    //TODO  ipaddressIPv4
                    var ipAddress = from job in model.JOB
                                    where job.JobID == jobID
                                    select job.ASSETSESSION.ASSET.ipaddressIPv4;

                    // Utils.Helper_Trace("AGENT SERVICE:IBlacklistedDetector", string.Format("Entering in Run with ipAdress=" + ipAddress.First()));

                    try
                    {

                        //string fileName = string.Empty;
                        //fileName = (string)parameters["FILENAME"];


                        //var Q = from myUser in model.USERACCOUNT
                        //        where myUser.UserID == userID
                        //        select myUser;

                        //int AccountCurrentUser = (int)Q.First().AccountID;
                        //var ipAddress = from job in model.JOB
                        //           where job.JobID == jobID
                        //           select job.ASSETSESSION.ASSET.IpAdress;


                        IMyInterface.Run(ipAddress.First(), jobID);
                    }
                    catch (ThreadAbortException ex)
                    {
                        Utils.Helper_Trace("AGENT SERVICE", string.Format("{0} : Error in IBlacklistedDetector.Run() : ThreadAbortException = {1}", provider, ex.Message));

                        Utils.Helper_Trace("AGENT SERVICE", string.Format("{0} : Setting job {1} status to CANCELED", provider, jobID));
                        Helper_SetJobStatus(jobID, XCommon.STATUS.CANCELED);

                        Utils.Helper_Trace("AGENT SERVICE", string.Format("{0} : Leaving LaunchJob", provider));
                        return;
                    }
                    catch (Exception ex)
                    {
                        Utils.Helper_Trace("AGENT SERVICE", string.Format("{0} : Error in IBlacklistedDetector.Run() : Exception = {1}", provider, ex.Message));

                        Utils.Helper_Trace("AGENT SERVICE", string.Format("{0} : Setting job {1} status to ERROR", provider, jobID));
                        Helper_SetJobStatus(jobID, XCommon.STATUS.ERROR);

                        Utils.Helper_Trace("AGENT SERVICE", string.Format("{0} : Leaving LaunchJob", provider));
                        return;
                    }

                    //// ============================================
                    //// Import the vulnerabilities into the database
                    //// ============================================

                    //Utils.Helper_Trace("AGENT SERVICE", string.Format("Importing vulnerabilities into the database"));

                    //var J = from jobs in model.JOB
                    //        where jobs.JobID == jobID
                    //        select jobs;

                    //JOB myJob = J.ToList().First();
                    //myJob.Status = "RUNNING";

                    //model.SaveChanges();
                }
                */
                #endregion

                //Import vulnerabilities from a Vulnerability Report (e.g. an ouput file from a Vulnerability Scanner)
                #region IVulnerabilityImporter

                if (oMarshalObject is IVulnerabilityImporter)
                {
                    Utils.Helper_Trace("AGENT SERVICE", string.Format("{0} : Plugin implements IVulnerabilityImporter", provider));

                    IVulnerabilityImporter IMyInterface;
                    IMyInterface = (IVulnerabilityImporter)oMarshalObject;

                    // ===================
                    // Call the Run method
                    // ===================

                    //string policy;
                    //policy = (string)parameters["POLICY"];
                    
                    //Utils.Helper_Trace("AGENT SERVICE", string.Format("Policy = {0}", policy));
                    //TODO  ipaddressIPv4
                    var ipAddress = from job in model.JOB
                                    where job.JobID == jobID
                                    select job.ASSETSESSION.ASSET.ipaddressIPv4;

                    try
                    {
                        XmlDocument doc = new XmlDocument();    //HARDCODED Assuming that only XML files are provided as input here

                        string fileName = string.Empty;
                        fileName = (string)parameters["FILENAME"];
                        doc.Load(fileName); //TODO: Input/XML validation

                        #region hack
                        //TODO-DEBUG
                        //doc.Load(@"c:\nessus_win2008.nessus");  //@"webscantest.com_80.xml"   //HARDCODED
                        #endregion hack

                        var Q = from myUser in model.USERACCOUNT
                                where myUser.UserID == userID
                                select myUser;

                        int AccountCurrentUser = (int)Q.First().AccountID;

                        IMyInterface.Run(doc.InnerXml, jobID, AccountCurrentUser);
                         
                        // IMyInterface.Run(fileName, jobID,AccountCurrentUser);
                        

                    }
                    catch (ThreadAbortException ex)
                    {
                        Utils.Helper_Trace("AGENT SERVICE", string.Format("{0} : Error in IVulnerabilityImporter.Run() : ThreadAbortException = {1} {2}", provider, ex.Message, ex.InnerException));

                        Utils.Helper_Trace("AGENT SERVICE", string.Format("{0} : Setting job {1} status to CANCELED", provider, jobID));
                        Helper_SetJobStatus(jobID, XCommon.STATUS.CANCELED);
                        XCommon.Utils.Helper_SendEmail("contact@hackenaton.org", provider + " job=" + jobID + " ThreadAbortException in IVulnerabilityImporter", "MyException = " + ex.Message + " " + ex.InnerException);

                        Utils.Helper_Trace("AGENT SERVICE", string.Format("{0} : Leaving LaunchJob", provider));
                        return;
                    }
                    catch (Exception ex)
                    {
                        Utils.Helper_Trace("AGENT SERVICE", string.Format("{0} : Error in IVulnerabilityImporter.Run() : Exception = {1} {2}", provider, ex.Message, ex.InnerException));

                        Utils.Helper_Trace("AGENT SERVICE", string.Format("{0} : Setting job {1} status to ERROR", provider, jobID));
                        Helper_SetJobStatus(jobID, XCommon.STATUS.ERROR);
                        XCommon.Utils.Helper_SendEmail("contact@hackenaton.org", provider + " job=" + jobID + " Error in IVulnerabilityImporter", "MyException = " + ex.Message + " " + ex.InnerException);

                        Utils.Helper_Trace("AGENT SERVICE", string.Format("{0} : Leaving LaunchJob", provider));
                        return;
                    }

                    // ============================================
                    // Import the vulnerabilities into the database
                    // ============================================

                    //Utils.Helper_Trace("AGENT SERVICE", string.Format("Importing vulnerabilities into the database"));

                    //foreach (VULNERABILITYFOUND V in VulnsFound)
                    //{
                    //    var tmpV = from vuln in model.VULNERABILITYFOUND
                    //               where vuln.ID == V.ID
                    //               select vuln;

                    //    VULNERABILITYFOUND TmpV = tmpV.ToList().First();
                    //    TmpV.JobID = jobID;

                    //    model.SaveChanges();
                    //}

                    //var J = from jobs in model.JOB
                    //        where jobs.JobID == jobID
                    //        select jobs;

                    //JOB myJob = J.ToList().First();
                    //myJob.Status = 0;

                    //model.SaveChanges();
                }

                #endregion
                
                //Example: nmap
                #region IScannable

                if (oMarshalObject is IScannable)
                {
                    Utils.Helper_Trace("AGENT SERVICE", string.Format("{0} : Plugin implements IScannable.", provider));
                
                    IScannable IMyInterface;
                    IMyInterface = (IScannable)oMarshalObject;

                    List<ScannableAsset> tab;

                    try
                    {
                        string target;
                        //var T = from t in model.JOB
                        //        where t.JobID == jobID
                        //        select t.ASSETSESSION.ASSET.IpAdress;
                        //target = T.ToList().FirstOrDefault();

                        // var q = model.JOB.Select(x => x.JobID == jobID);
                        // model.JOB.Select(

                        target = (string)parameters["TARGET"];

                        Utils.Helper_Trace("AGENT SERVICE", string.Format("{0} : Target = {1}", provider, target));

                        tab = IMyInterface.DiscoverHost(target, HostStatus.Alive, jobID);
                        if (tab == null)
                        {
                            Utils.Helper_Trace("AGENT SERVICE", string.Format("DiscoverHost reported an error"));
                            return;
                        }

                        Utils.Helper_Trace("AGENT SERVICE", string.Format("DiscoverHost has finished and returned {0} assets", tab.Count));
                    }
                    catch (ThreadAbortException ex)
                    {
                        Utils.Helper_Trace("AGENT SERVICE", string.Format("Error in IScannable.DiscoverHost() : ThreadAbortException = {0} {1}", ex.Message, ex.InnerException));

                        Utils.Helper_Trace("AGENT SERVICE", string.Format("{0} : Setting job {1} status to CANCELED", provider, jobID));
                        Helper_SetJobStatus(jobID, XCommon.STATUS.CANCELED);

                        Utils.Helper_Trace("AGENT SERVICE", string.Format("{0} : Leaving LaunchJob", provider));
                        return;
                    }
                    catch (Exception ex)
                    {
                        Utils.Helper_Trace("AGENT SERVICE", string.Format("Error in IScannable.DiscoverHost() : Exception = {0} {1}", ex.Message, ex.InnerException));

                        XCommon.Utils.Helper_SendEmail("contact@hackenaton.org", provider + " job=" + jobID + " Error in IScannable.DiscoverHost", "MyException = " + ex.Message + " " + ex.InnerException);

                        Utils.Helper_Trace("AGENT SERVICE", string.Format("{0} : Setting job {1} status to ERROR", provider, jobID));
                        Helper_SetJobStatus(jobID, XCommon.STATUS.ERROR);

                        Utils.Helper_Trace("AGENT SERVICE", string.Format("{0} : Leaving LaunchJob", provider));
                        return;
                    }

                    // ===========================================================
                    //TODO
                    /*
                    foreach (ScannableAsset oo in tab)
                    {
                        
                        Host myHost = new Host();

                        myHost.IpAdress = "";
                        myHost.MacAdress = "";

                        foreach (ScannableAddress Adr in oo.Addresses)
                        {
                            switch (Adr.AddressType)
                            {
                                case ScannableAddress.ADDRESSTYPE.ipv4: myHost.IpAdress = Adr.Address; break;
                                case ScannableAddress.ADDRESSTYPE.mac: myHost.MacAdress = Adr.Address; break;
                            }
                        }

                        myHost.OsName = oo.OS.OSName;
                        myHost.JobID = jobID;

                        model.AddToHost(myHost);
                        model.SaveChanges();
                        Utils.Helper_Trace("AGENT SERVICE", string.Format("Inserting Host : {0} into table host", myHost.HostID));
                        foreach (ScannablePort Sp in oo.Ports)
                        {
                            HostEndPoint Hendpoint = new HostEndPoint();
                            Hendpoint.HostID = myHost.HostID;
                            Hendpoint.HostPort = Sp.Port;
                            Hendpoint.HostProtocol = Sp.Protocol.ToString();
                            Hendpoint.HostService = Sp.Service;
                            Hendpoint.HostVersion = Sp.Version;
                            model.AddToHostEndPoint(Hendpoint);
                        }
                        model.SaveChanges();
                        Utils.Helper_Trace("AGENT SERVICE", string.Format("Host : {0} has {1}", myHost.IpAdress, myHost.HostEndPoint.Count));

                    }

                    //Utils.Helper_Trace("AGENT SERVICE", string.Format("Updating status to RUNNING in table JOB"));

                    //var J = from jobs in model.JOB
                    //        where jobs.JobID == jobID
                    //        select jobs;

                    //JOB myJob = J.ToList().First();
                    //myJob.Status = "RUNNING";

                    //model.SaveChanges();
                    */
                }
                
                #endregion
                

                #region IVulnerabilityDetector

                if (oMarshalObject is IVulnerabilityDetector)
                {                    
                    Utils.Helper_Trace("AGENT SERVICE", string.Format("{0} : Plugin implements IVulnerabilityDetector", provider));
                    
                    IVulnerabilityDetector IMyInterface;
                    IMyInterface = (IVulnerabilityDetector)oMarshalObject;

                    // ===================
                    // Call the Run method
                    // ===================

                    string policy;
                    string strategie;
                    //Scan "Profiles" (e.g.: Web/Quick Scan)
                    policy = (string)parameters["POLICY"];
                    strategie = (string)parameters["STRATEGY"];

                    Utils.Helper_Trace("AGENT SERVICE", string.Format("{0} : Policy = {1} & Strategy = {2}", provider, policy, strategie));

                    var ipAddress = from job in model.JOB
                                    where job.JobID == jobID
                                    select job.ASSETSESSION.ASSET.ipaddressIPv4;
                    try
                    {
                        IMyInterface.Run(ipAddress.First(), jobID, policy, strategie);
                    }
                    catch (ThreadAbortException ex)
                    {
                        Utils.Helper_Trace("AGENT SERVICE", string.Format("{0} : Error in IVulnerabilityDetector.Run() : ThreadAbortException = {1} {2}", provider, ex.Message, ex.InnerException));

                        Utils.Helper_Trace("AGENT SERVICE", string.Format("{0} : Setting job {1} status to CANCELLED", provider, jobID));
                        Helper_SetJobStatus(jobID, XCommon.STATUS.CANCELED);

                        Utils.Helper_Trace("AGENT SERVICE", string.Format("{0} : Leaving LaunchJob", provider));
                        return;
                    }
                    catch (Exception ex)
                    {
                        Utils.Helper_Trace("AGENT SERVICE", string.Format("{0} Job:{1} Error in IVulnerabilityDetector.Run() : MyException = {2} {3}", provider, jobID, ex.Message, ex.InnerException));
                        //MyException = Unable to connect to the remote server
                        //Exception of type 'System.OutOfMemoryException' was thrown.
                        XCommon.Utils.Helper_SendEmail("contact@hackenaton.org", provider+" job="+jobID+" Error in IVulnerabilityDetector.Run", "MyException = " + ex.Message+" "+ ex.InnerException);

                        Utils.Helper_Trace("AGENT SERVICE", string.Format("{0} : Setting job {1} status to ERROR", provider, jobID));
                        Helper_SetJobStatus(jobID, XCommon.STATUS.ERROR);

                        Utils.Helper_Trace("AGENT SERVICE", string.Format("{0} : Leaving LaunchJob", provider));
                        return;
                    }
                    
                    #region Old_Code_That update Job using ListVulnerability
                    // ============================================
                    // Import the vulnerabilities into the database
                    // ============================================

                    //Utils.Helper_Trace("AGENT SERVICE", string.Format("Importing vulnerabilities into the database"));

                    //foreach (VULNERABILITYFOUND V in VulnsFound)
                    //{
                    //    var tmpV = from vuln in model.VULNERABILITYFOUND
                    //               where vuln.ID == V.ID
                    //               select vuln;

                    //    VULNERABILITYFOUND TmpV = tmpV.ToList().First();
                    //    TmpV.JobID = jobID;

                    //    model.SaveChanges();
                    //}

                    //var J = from jobs in model.JOB
                    //        where jobs.JobID == jobID
                    //        select jobs;

                    //JOB myJob = J.ToList().First();
                    //myJob.Status = 0;

                    //model.SaveChanges();
                    #endregion
                    
                }
                #endregion
                
                //Availibity checks (e.g.: Pingdom)
                #region IWebSiteMonitoring
                if (oMarshalObject is IWebSiteMonitor)
                {
                    Utils.Helper_Trace("AGENT SERVICE", string.Format("{0} : Plugin implements IWebSiteMonitor", provider));

                    IWebSiteMonitor IMyInterface;
                    IMyInterface = (IWebSiteMonitor)oMarshalObject;

                    // ===================
                    // Call the Run method
                    // ===================

                    var ipAddress = from job in model.JOB
                                    where job.JobID == jobID
                                    select job.ASSETSESSION.ASSET.ipaddressIPv4;

                    try
                    {
                        // TODO : DD & DF via IHM   A VOIR

                        DateTime DD = new DateTime(2011, 4, 1); //TODO Hardcoded
                        DateTime DF = new DateTime(2011, 4, 1); //TODO Hardcoded
                        string sessionID = IMyInterface.Run(ipAddress.First(), jobID, DD, DF, userID);

                    }
                    catch (ThreadAbortException ex)
                    {
                        Utils.Helper_Trace("AGENT SERVICE", string.Format("{0} : Error in IWebSiteMonitor.Run() : ThreadAbortException = {1}", provider, ex.Message));

                        Utils.Helper_Trace("AGENT SERVICE", string.Format("{0} : Setting job {1} status to CANCELED", provider, jobID));
                        Helper_SetJobStatus(jobID, XCommon.STATUS.CANCELED);

                        Utils.Helper_Trace("AGENT SERVICE", string.Format("{0} : Leaving LaunchJob", provider));
                        return;
                    }
                    catch (Exception ex)
                    {
                        Utils.Helper_Trace("AGENT SERVICE", string.Format("{0} : Error in IWebSiteMonitor.Run() : Exception = {1}", provider, ex.Message));

                        Utils.Helper_Trace("AGENT SERVICE", string.Format("{0} : Setting job {1} status to ERROR", provider, jobID));
                        Helper_SetJobStatus(jobID, XCommon.STATUS.ERROR);

                        Utils.Helper_Trace("AGENT SERVICE", string.Format("{0} : Leaving LaunchJob", provider));
                        return;
                    }

                    #region Fake, using GridPanel
                    #endregion

                    #region Import the vulnerabilities into the database

                    // TODO

                    #endregion

                    //#region update job's status
                    //var J = from jobs in model.JOB
                    //        where jobs.JobID == jobID
                    //        select jobs;

                    //JOB myJob = J.ToList().First();
                    //myJob.Status = "FINISHED";

                    //model.SaveChanges();
                    //#endregion

                }

                #endregion
                
                Utils.Helper_Trace("AGENT SERVICE", string.Format("{0} : JOBID {1} : Leaving thread", provider, jobID));

                g_MapThread.Remove(jobID);
                //FREE MEMORY
                ms.Dispose();
                parameters = null;
                buffer = null;
                context = null;
//                pluginManager.Stop();   //Unload
                
                //AppDomain.Unload("Plugins");

            }
            catch (Exception ex)
            {
                Utils.Helper_Trace("AGENT SERVICE", string.Format("Error in Helper_TreadFunc threadContext. Exception = {0} {1}", ex.Message, ex.InnerException));
                //return false;
            }

            
        }


        private void Helper_SetJobStatus(int jobID, XCommon.STATUS status)
        {
            XORCISMEntities model;
            model = new XORCISMEntities();

            JOB job;
            job = model.JOB.FirstOrDefault(o => o.JobID == jobID);

            job.Status = status.ToString();
            job.DateEnd = DateTimeOffset.Now;

            model.SaveChanges();
            //FREE MEMORY
            model.Dispose();
            job = null;
        }

        internal class AcceptAllCertificatePolicy : ICertificatePolicy
        {
            public AcceptAllCertificatePolicy()
            {
            }

            public bool CheckValidationResult(ServicePoint sPoint,
               X509Certificate cert, WebRequest wRequest, int certProb)
            {
                //TODO Hardcoded Review *** Always accept!!!
                return true;
            }
        }
    }
}

//How to get the available RAM and the cpu usage in percents?

//These methods are very usefull in order to monitor the system and particulary the amount of the available RAM in MB (MegaBytes) and the cpu usage in percents.

///*
//First you have to create the 2 performance counters
//using the System.Diagnostics.PerformanceCounter class.
//*/

//protected PerformanceCounter cpuCounter;
//protected PerformanceCounter ramCounter;

//cpuCounter = new PerformanceCounter();

//cpuCounter.CategoryName = "Processor";
//cpuCounter.CounterName = "% Processor Time";
//cpuCounter.InstanceName = "_Total";

//ramCounter = new PerformanceCounter("Memory", "Available MBytes");

///*
//Call this method every time you need to know
//the current cpu usage.
//*/

//public string getCurrentCpuUsage(){
//            cpuCounter.NextValue()+"%";
//}

///*
//Call this method every time you need to get
//the amount of the available RAM in Mb
//*/
//public string getAvailableRAM(){
//            ramCounter.NextValue()+"Mb";
//} 