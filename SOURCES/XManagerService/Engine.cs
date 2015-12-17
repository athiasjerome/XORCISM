using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.ServiceModel;
using System.Threading;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using XORCISMModel;
using XCommon;
 
using NCrontab;

namespace XManagerService
{
    public class Engine
    {
        /// <summary>
        /// Copyright (C) 2012-2015 Jerome Athias
        /// Windows Service managing Agents (bots), Managing Sessions (groups of Jobs) of Plugins (connectors to Tools/Services) interacting with an XORCISM database
        /// All trademarks and registered trademarks are the property of their respective owners.
        /// This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation; either version 2 of the License, or (at your option) any later version.
        /// 
        /// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
        /// 
        /// You should have received a copy of the GNU General Public License along with this program; if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
        /// </summary>
        
        private Thread          m_Thread;

        //public Dictionary<int, Thread>  ListRunningSessionThread
        //{
        //    get { return m_ListRunningSessionThread; }
        //    set { m_ListRunningSessionThread = value; }
        //}

        private class LaunchSessionThreadInfo
        {
            public int          SessionID;
            public Thread       Thread;
            public List<JOB>    ListJob;

            public LaunchSessionThreadInfo(int sessionID, Thread thread)
            {
                this.SessionID  = sessionID;
                this.Thread     = thread;
                this.ListJob    = new List<JOB>();
            }
        }

        private Dictionary<int, LaunchSessionThreadInfo> m_ListRunningSessionThread;
        
        public Engine()
        {
         
        }
       
        public void Start()
        {
            // ==================================================================
            // Recovery mode : start again any previously running session threads
            // ==================================================================
            XORCISMEntities context;
            context = new XORCISMEntities();

            // Explicitly open the connection.
            //context.Connection.Open();

            #region RecoveryMode
            try
            {                
                string status;
                status = XCommon.STATUS.RUNNING.ToString();
                //Search in the database the Sessions with a Status of "Running"
                var runningSessions = from s in context.SESSION
                                      where s.Status == status
                                      select s;                
                //If any
                if (runningSessions.Count() > 0)
                {
                    //For each of the "Running" Sessions
                    foreach (SESSION oneSession in runningSessions.ToList())
                    {
                        //Depending on the ServiceCategory of the Session
                        switch (oneSession.ServiceCategoryID)
                        {
                            case 8: //HARDCODED
                                {
                                    //Monitoring
                                    //Dealing with the previous jobs
                                    //Searching the Jobs attached to the current Session
                                    var query = from j in context.JOB
                                                where j.SessionID == oneSession.SessionID
                                                select j;
                                    //Marks all the Session's Jobs as FINISHED
                                    foreach (JOB oneJob in query.ToList())
                                    {
                                        oneJob.Status = XCommon.STATUS.FINISHED.ToString();
                                        
                                        context.SaveChanges();
                                    }
                                    Utils.Helper_Trace("MANAGER ENGINE", string.Format("Session {0} will be recovered", oneSession.SessionID));
                                    //Marks the Session's status as IDLE (so it will be (re)started)
                                    oneSession.Status = XCommon.STATUS.IDLE.ToString();
                                    Utils.Helper_Trace("MANAGER ENGINE", "TRACE4");
                                    context.SaveChanges();

                                    break;
                                }
                            default:
                                {
                                    //Canceling the session (changing status)
                                    Utils.Helper_Trace("MANAGER ENGINE", string.Format("Session {0} TOCANCEL", oneSession.SessionID));
                                    oneSession.Status = XCommon.STATUS.TOCANCEL.ToString();
                                    
                                    context.SaveChanges();

                                    break;
                                }

                        }
                    }
                }
            }
            catch (ThreadAbortException exThreadAbort)
            {
                Utils.Helper_Trace("MANAGER ENGINE", string.Format("ThreadError in recovery : Exception = {0}", exThreadAbort.Message));
                return;
            }
            catch (Exception ex)
            {
                Utils.Helper_Trace("MANAGER ENGINE", string.Format("Error in recovery : Exception = {0} - {1}", ex.Message, ex.InnerException));

                //    return;
            }
            #endregion RecoveryMode

            // ==============================
            // Create the main polling thread
            // ==============================

            Utils.Helper_Trace("MANAGER ENGINE", "Launching main polling thread");

            m_ListRunningSessionThread = new Dictionary<int, LaunchSessionThreadInfo>();

            m_Thread = new Thread(new ThreadStart(FuncThread));
            m_Thread.Start();
        }

        private void FuncThread()
        {
            XORCISMEntities context;
            context = new XORCISMEntities();

            // Explicitly open the connection.
            //context.Connection.Open();

            

            // =================
            // Main polling loop
            // =================

            try
            {
                while (true)    //Infinite loop
                {
                    // =============================================================================
                    // PHASE 0 : Look in table SESSION and let's see if there is something to cancel
                    // =============================================================================

                    #region Phase0
                    //Utils.Helper_Trace("MANAGER ENGINE", "Looking for new session to cancel (those with status = 'ToCancel')");

                    string status;
                    status = XCommon.STATUS.TOCANCEL.ToString();

                    XORCISMModel.SESSION cancelSession;
                    cancelSession = context.SESSION.FirstOrDefault(s => s.Status == status);

                    if (cancelSession != null)
                    {
                        // ===============================
                        // Abort the Launch Session thread
                        // ===============================

                        if (m_ListRunningSessionThread.ContainsKey(cancelSession.SessionID) == true)
                        {
                            LaunchSessionThreadInfo info;
                            info = m_ListRunningSessionThread[cancelSession.SessionID];

                            Thread musCanceledThread;
                            musCanceledThread = info.Thread;

                            musCanceledThread.Abort(cancelSession.SessionID.ToString());

                            // ==============================
                            // Launch a Cancel Session thread
                            // ==============================

                            Utils.Helper_Trace("MANAGER ENGINE", string.Format("Launching cancel session thread (sessionID={0})", cancelSession.SessionID));

                            cancelSession.Status = XCommon.STATUS.CANCELLING.ToString();

                            context.SaveChanges();

                            ParameterizedThreadStart ts;
                            ts = new ParameterizedThreadStart(FuncThreadCancelSession);

                            Thread thread;
                            thread = new Thread(ts);

                            thread.Start(info);
                        }
                        else
                        {
                            //Canceling after Manager crash/reboot
                            //Session is not in m_ListRunningSessionThread
                            Utils.Helper_Trace("MANAGER ENGINE", string.Format("Session {0} must be canceled", cancelSession.SessionID));
                            
                            //int accountID;
                            //accountID = (int)model.USERACCOUNT.FirstOrDefault(o => o.UserID == session.UserID).AccountID;

                            // =============================
                            // Cancel the jobs on the agents
                            // =============================

                            Utils.Helper_Trace("MANAGER ENGINE", string.Format("SESSION {0} : CANCELSESSION : Cancelling jobs on agents", cancelSession.SessionID));

                            var jobs = from jc in context.JOB
                                       where jc.SessionID == cancelSession.SessionID
                                       select jc;

                            foreach (JOB J in jobs.ToList())
                            {
                                Utils.Helper_Trace("MANAGER ENGINE", string.Format("SESSION {0} : CANCELSESSION : Handling job {1}", cancelSession.SessionID, J.JobID));

                                // ====================================
                                // Contact the agent and cancel the job
                                // ====================================

                                Utils.Helper_Trace("MANAGER ENGINE", string.Format("SESSION {0} : CANCELSESSION : Trying to contact the agent", cancelSession.SessionID));

                                try
                                {
                                    ServiceReferenceAgent.Service1Client service;
                                    service = new ServiceReferenceAgent.Service1Client();

                                    // TODO :
                                    // service.Endpoint.Address = bestAgent.IPAddress;     

                                    service.CancelJob(J.JobID);

                                    Utils.Helper_Trace("MANAGER ENGINE", string.Format("SESSION {0} : CANCELSESSION : The agent has been successfully contacted", cancelSession.SessionID));
                                }
                                catch (Exception ex)
                                {
                                    Utils.Helper_Trace("MANAGER ENGINE", string.Format("SESSION {0} : CANCELSESSION : Error contacting the agent. Exception = {1} {2}", cancelSession.SessionID, ex.Message, ex.InnerException));
                                    XCommon.Utils.Helper_SendEmail("athiasjerome@gmail.com", "MANAGER ENGINE ERROR", "CANCELSESSION : Error contacting the agent. Exception =" + ex.Message + " " + ex.InnerException);
                                    //return;
                                }                                
                            }

                            // =============================
                            // Update table SESSION (Status)
                            // =============================

                            Utils.Helper_Trace("MANAGER ENGINE", string.Format("SESSION {0} : CANCELSESSION : Updating status in table SESSION to CANCELED", cancelSession.SessionID));
                            try
                            {
                                cancelSession.Status = XCommon.STATUS.CANCELED.ToString();
                                cancelSession.DateEnd = DateTimeOffset.Now;

                                context.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                Utils.Helper_Trace("MANAGER ENGINE", string.Format("SESSION {0} : CANCELSESSION : Error CANCELED. Exception = {1}", cancelSession.SessionID, ex.Message));
                                //return;
                            } 
                            Utils.Helper_Trace("MANAGER ENGINE", string.Format("SESSION {0} : CANCELSESSION : Finished", cancelSession.SessionID));

                        }
                    }
                    #endregion Phase0

                    // =============================================================================
                    // PHASE 1 : Look in table SESSION and let's see if there is something to launch
                    // =============================================================================

                    Utils.Helper_Trace("MANAGER ENGINE", "Looking for new session to start (status IDLE)");    //DO NOT COMMENT THIS LINE

                    string Statut = XCommon.STATUS.IDLE.ToString();

                    var session = context.SESSION.FirstOrDefault(s => s.Status == Statut);

                    if (session != null)
                    {
                        int sessionID;
                        sessionID = session.SessionID;

                        //Check if the Account and User are still valid                            
                        USERACCOUNT user = null;
                        user = context.USERACCOUNT.SingleOrDefault(o => o.UserID == session.UserID);                        
                        if (user.ACCOUNT.ValidUntilDate!=null && user.ACCOUNT.ValidUntilDate < DateTimeOffset.Now)
                        {
                            Utils.Helper_Trace("MANAGER ENGINE", string.Format("Account not valid for session {0}", sessionID));
                            Utils.Helper_Trace("MANAGER ENGINE", string.Format("Changing session (sessionID={0}) to CANCELED", sessionID));

                            session.Status = XCommon.STATUS.CANCELED.ToString();
                            context.SaveChanges();
                        }
                        else
                        {
                            if (m_ListRunningSessionThread.ContainsKey(sessionID) == true)
                            {
                                Utils.Helper_Trace("MANAGER ENGINE", string.Format("Session {0} is supposed to be launched but a thread is already running for this session !", sessionID));
                            }
                            else
                            {
                                // =======================
                                // Launch a session thread
                                // =======================

                                Utils.Helper_Trace("MANAGER ENGINE", string.Format("Launching session (sessionID={0})", sessionID));

                                session.Status = XCommon.STATUS.RUNNING.ToString();
                                context.SaveChanges();

                                ParameterizedThreadStart managerThreadStart;
                                managerThreadStart = new ParameterizedThreadStart(FuncThreadLaunchSession);

                                Thread thread;
                                thread = new Thread(managerThreadStart);

                                LaunchSessionThreadInfo info;
                                info = new LaunchSessionThreadInfo(sessionID, thread);

                                thread.Start(info);

                                // ========================
                                // Put it in the dictionary
                                // ========================

                                m_ListRunningSessionThread.Add(sessionID, info);
                            }
                        }
                    }

                    // ===================================================================================
                    // PHASE 2 : Look in table SESSIONCRON and let's see if we have to create new sessions
                    // ===================================================================================

                    Statut = XCommon.STATUS.IDLE.ToString();

                    var q = context.SESSIONCRON.Where(o => o.Status == Statut);

                    foreach (SESSIONCRON sessionCron in q.ToList())
                    {
                        if (sessionCron.DateEnd==null || sessionCron.DateEnd > DateTime.Now)
                        {
                            //Check if the Account and User are still valid                            
                            USERACCOUNT user = null;
                            user = context.USERACCOUNT.SingleOrDefault(o => o.UserID == sessionCron.UserID);
                            if (user.ACCOUNT.ValidUntilDate != null && user.ACCOUNT.ValidUntilDate < DateTimeOffset.Now)
                            {
                                //Utils.Helper_Trace("MANAGER ENGINE", string.Format("Account not valid for entry {0} in table SESSIONCRON", sessionCron.SessionCronID));
                            }
                            else
                            {
                                CrontabSchedule schedule;
                                schedule = CrontabSchedule.Parse(sessionCron.CronExpression);

                                DateTimeOffset start = DateTimeOffset.Now;
                                DateTimeOffset end = start + TimeSpan.FromDays(2 * 360);

                                var occurrence = schedule.GetNextOccurrences(start, end).GetEnumerator();
                                occurrence.MoveNext();
                                //                        Utils.Helper_Trace("MANAGER ENGINE", "SessionCron "+sessionCron.SessionCronID+" Next occurrence=" + occurrence.Current.DayOfWeek.ToString() + " " + occurrence.Current.Day.ToString() + "/" + occurrence.Current.Month.ToString() + "/" + occurrence.Current.Year.ToString() + " " + occurrence.Current.Hour.ToString() + "H" + occurrence.Current.Minute.ToString() + ":" + occurrence.Current.Second.ToString());

                                TimeSpan ts;
                                ts = occurrence.Current - start;
                                if (ts.TotalSeconds <= 5.0)
                                {
                                    Utils.Helper_Trace("MANAGER ENGINE", string.Format("Cron expression for entry {0} in table SESSIONCRON has triggered an execution", sessionCron.SessionCronID));

                                    // ================================
                                    // Extract and parse the parameters
                                    // ================================

                                    Dictionary<string, object> dicoParameters;
                                    try
                                    {
                                        MemoryStream ms;
                                        ms = new MemoryStream(sessionCron.Parameters);

                                        BinaryFormatter bf;
                                        bf = new BinaryFormatter();

                                        dicoParameters = (Dictionary<string, object>)bf.Deserialize(ms);
                                    }
                                    catch (Exception e)
                                    {
                                        Utils.Helper_Trace("MANAGER SERVICE", string.Format("Exception while deserializing parameters : {0}", e.Message));
                                        return;
                                    }

                                    int[] tabAssetID = null;
                                    if (dicoParameters["ASSETS"] != null)
                                        tabAssetID = (int[])dicoParameters["ASSETS"];

                                    // ================================
                                    // Add a new entry in table SESSION
                                    // ================================

                                    Utils.Helper_Trace("MANAGER ENGINE", string.Format("Adding an entry in table SESSION"));

                                    SESSION tmpSession = new SESSION();
                                    //xxx
                                    try
                                    {
                                        tmpSession.UserID = sessionCron.UserID;
                                        tmpSession.Status = XCommon.STATUS.IDLE.ToString();
                                        tmpSession.ServiceCategoryID = sessionCron.ServiceCategoryID;
                                        tmpSession.DateStart = DateTimeOffset.Now;
                                        tmpSession.DateEnd = null;
                                        tmpSession.Parameters = sessionCron.Parameters;
                                        tmpSession.SessionCronID = sessionCron.SessionCronID;

                                        context.SESSION.Add(tmpSession);

                                        context.SaveChanges();

                                        //xxx
                                    }
                                    catch (Exception ex)
                                    {
                                        //xxx

                                        Utils.Helper_Trace("MANAGER ENGINE", string.Format("Error adding entry in table SESSION : Exception = {0} - {1}", ex.Message, ex.InnerException.Message));
                                        throw ex;
                                    }

                                    Utils.Helper_Trace("MANAGER ENGINE", string.Format("SessionID = {0}", tmpSession.SessionID));

                                    // ============================================
                                    // Add several entries in table ASSETSESSION
                                    // ============================================

                                    if (tabAssetID != null)
                                    {
                                        Utils.Helper_Trace("MANAGER ENGINE", string.Format("Adding {0} entries in table ASSETSESSION", tabAssetID.Count()));
                                        try
                                        {
                                            foreach (int assetID in tabAssetID)
                                            {
                                                ASSETSESSION tmpAinS = new ASSETSESSION();
                                                tmpAinS.SESSION = tmpSession;
                                                tmpAinS.AssetID = assetID;
                                                context.ASSETSESSION.Add(tmpAinS);
                                            }
                                            context.SaveChanges();
                                        }
                                        catch (Exception ex)
                                        {
                                            Utils.Helper_Trace("MANAGER ENGINE", string.Format("Error adding entries in table ASSETSESSION : Exception = {0}", ex.Message));
                                            throw ex;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    // =====
                    // Sleep
                    // =====

                    Thread.Sleep(5000); //Hardcoded
                }
            }
            catch (ThreadAbortException exThreadAbort)
            {
                //int SessionId;
                //SessionId=Convert.ToInt32((string)exThreadAbort.ExceptionState);
                //XORCISMModel.SESSION musBeCanceledSession;
                //musBeCanceledSession=context.SESSION.SingleOrDefault(s => s.SessionID == SessionId);
                //if (musBeCanceledSession != null)
                //{
                //    musBeCanceledSession.Status = XCommon.STATUS.TOCANCEL.ToString();
                //    context.SaveChanges();
                //}
                Utils.Helper_Trace("MANAGER ENGINE", string.Format("ThreadError in main polling loop : Exception = {0}", exThreadAbort.Message));
                //HARDCODED
                XCommon.Utils.Helper_SendEmail("contact@hackenaton.org", "ThreadError in XManager", "MyException = " + exThreadAbort.Message + " " + exThreadAbort.InnerException);
                return;
            }
            catch (Exception ex)
            {
                Utils.Helper_Trace("MANAGER ENGINE", string.Format("Error in main polling loop : Exception = {0} {1}", ex.Message, ex.InnerException));
                //HARDCODED
                XCommon.Utils.Helper_SendEmail("contact@hackenaton.org", "Error in XManager", "MyException = " + ex.Message + " " + ex.InnerException);
                return;
            }
        }

        private void FuncThreadLaunchSession(object context)
        {
            LaunchSessionThreadInfo info;
            info = (LaunchSessionThreadInfo)context;

            int sessionID;
            sessionID = info.SessionID;

            XORCISMEntities model = new XORCISMEntities();

            SESSION session;
            session = model.SESSION.SingleOrDefault(o => o.SessionID == sessionID);

            int accountID;
            accountID = (int)model.USERACCOUNT.FirstOrDefault(o => o.UserID == session.UserID).AccountID;

            // ===========================================
            // Determine the providers that we have to use
            // ===========================================

            var category = (from sessions in model.SESSION
                            where sessions.SessionID == sessionID
                            select sessions.ServiceCategoryID).First();

            Utils.Helper_Trace("MANAGER ENGINE", string.Format("SESSION {0} : THREADLAUNCHSESSION : ServiceCategoryID is {1}   AccountID is {2}", sessionID, category.Value, accountID));

            var ListProvider = from provider in model.PROVIDER
                               where provider.ServiceCategoryID == category && provider.PROVIDERSFORACCOUNT.Any(o => o.AccountID == accountID && o.ValidUntil >= DateTimeOffset.Now)
                               select provider.ProviderID;

            List<int> listProviderID;
            listProviderID = ListProvider.ToList<int>();

            Utils.Helper_Trace("MANAGER ENGINE", string.Format("SESSION {0} : THREADLAUNCHSESSION : Session will use {1} providers", sessionID, listProviderID.Count));
            int nbjoberror = 0;
            if (listProviderID.Count > 0)
            {
                // ================================
                // Add several entries in table JOB
                // ================================

                var ListAssetInSession = from AinS in model.ASSETSESSION
                                         where AinS.SessionID == sessionID
                                         select AinS.AssetSessionID;

                Utils.Helper_Trace("MANAGER ENGINE", string.Format("SESSION {0} : THREADLAUNCHSESSION : Session involves {1} assets", sessionID, ListAssetInSession.Count()));

                int count;
                count = ListAssetInSession.Count() * listProviderID.Count;

                Utils.Helper_Trace("MANAGER ENGINE", string.Format("SESSION {0} : THREADLAUNCHSESSION : Adding {1} entries in table JOB", sessionID, count));

                var param = from o in model.SESSION
                            where o.SessionID == sessionID
                            select o.Parameters;

                if (ListAssetInSession.Count() != 0)
                {
                    Utils.Helper_Trace("MANAGER ENGINE", string.Format("SESSION {0} : THREADLAUNCHSESSION : Case 1", sessionID));

                    foreach (int AinSID in ListAssetInSession)
                    {
                        foreach (int providerID in listProviderID)
                        {
                            try
                            {
                                JOB job = new JOB();
                                job.AssetSessionID = AinSID;
                                job.ProviderID = providerID;
                                job.DateStart = DateTimeOffset.Now;
                                job.Status = XCommon.STATUS.IDLE.ToString();
                                job.Parameters = param.First();
                                job.SessionID = sessionID;
                                model.JOB.Add(job);
                                //model.AddToJOB(job);
                                //model.SaveChanges();

                                info.ListJob.Add(job);
                                Utils.Helper_Trace("MANAGER ENGINE", string.Format("SESSION {0} : THREADLAUNCHSESSION : Added Job {1}", sessionID, job.JobID));
                            }
                            catch (Exception e1)
                            {
                                Utils.Helper_Trace("MANAGER ENGINE", string.Format("SESSION {0} : THREADLAUNCHSESSION : Exception AddingJob1 "+e1.Message+" "+e1.InnerException, sessionID));
                            }
                        }
                    }
                    model.SaveChanges();
                }
                else
                {
                    Utils.Helper_Trace("MANAGER ENGINE", string.Format("SESSION {0} : THREADLAUNCHSESSION : Case 2", sessionID));
                    switch ((int)category.Value)
                    {
                        case 10:
                            //nmap discovery
                            Utils.Helper_Trace("MANAGER ENGINE", string.Format("SESSION {0} : THREADLAUNCHSESSION : nmap discovery", sessionID));
                            break;
                        case 11:
                            Utils.Helper_Trace("MANAGER ENGINE", string.Format("SESSION {0} : THREADLAUNCHSESSION : import nessus", sessionID));
                            break;

                        case 14:
                            Utils.Helper_Trace("MANAGER ENGINE", string.Format("SESSION {0} : THREADLAUNCHSESSION : import netsparker", sessionID));
                            break;
                        case 15:
                            Utils.Helper_Trace("MANAGER ENGINE", string.Format("SESSION {0} : THREADLAUNCHSESSION : import acunetix", sessionID));
                            break;
                        default:
                            return;
                    }
                    foreach (int providerID in listProviderID)
                    {
                        try
                        {
                            JOB job = new JOB();
                            job.AssetSessionID = null;
                            job.ProviderID = providerID;
                            job.DateStart = DateTimeOffset.Now;
                            job.Status = XCommon.STATUS.IDLE.ToString();
                            job.Parameters = param.First();
                            job.SessionID = sessionID;
                            //model.JOB.AddObject(job);
                            model.JOB.Add(job);
                            model.SaveChanges();

                            info.ListJob.Add(job);
                            Utils.Helper_Trace("MANAGER ENGINE", string.Format("SESSION {0} : THREADLAUNCHSESSION : Added Job {1}", sessionID, job.JobID));
                        }
                        catch (Exception e1)
                        {
                            Utils.Helper_Trace("MANAGER ENGINE", string.Format("SESSION {0} : THREADLAUNCHSESSION : Exception AddingJob2 " + e1.Message + " " + e1.InnerException, sessionID));
                        }
                    }
                }
                //model.SaveChanges();

                // =========================================
                // Dispatch the jobs on the available agents
                // =========================================

                Utils.Helper_Trace("MANAGER ENGINE", string.Format("SESSION {0} : THREADLAUNCHSESSION : Dispatching jobs on agents", sessionID));

                foreach (JOB J in info.ListJob)
                {
                    Utils.Helper_Trace("MANAGER ENGINE", string.Format("SESSION {0} : THREADLAUNCHSESSION : Handling job {1}", sessionID, J.JobID));

                    // ======================================================
                    //TODO Get the agent with the lowest load (loadbalancing)
                    // ======================================================

                    //TODO
                    /*
                    var agent = from Ag in model.AGENT
                                where Ag.Status == "ENABLED"
                                select Ag;
                    if (agent == null)
                    {
                        Utils.Helper_Trace("MANAGER ENGINE", string.Format("SESSION {0} : THREADLAUNCHSESSION : There is no agent with status ENABLED", sessionID));
                        return;
                    }
                    */

                    //TODO
                    /*
                    var bestAgent = agent.ToList().OrderBy(c => c.Load).First();
                    bestAgent.Load++;

                    Utils.Helper_Trace("MANAGER ENGINE", string.Format("SESSION {0} : THREADLAUNCHSESSION : The best agent for this job is at {1} ({2})", sessionID, bestAgent.AgentID, bestAgent.IPAddress));
                    */

                    // ====================================
                    // Contact the agent and launch the job
                    // ====================================

                    Utils.Helper_Trace("MANAGER ENGINE", string.Format("SESSION {0} : THREADLAUNCHSESSION : Trying to contact the agent", sessionID));

                    try
                    {
                        ServiceReferenceAgent.Service1Client service;
                        service = new ServiceReferenceAgent.Service1Client();

                        // TODO :
                        // service.Endpoint.Address = bestAgent.IPAddress;      // TODO

                        service.LaunchJob((Guid)session.UserID, J.JobID);
                    }
                    catch (Exception ex)
                    {
                        Utils.Helper_Trace("MANAGER ENGINE", string.Format("SESSION {0} : THREADLAUNCHSESSION : Error contacting the agent. Exception = {1}", sessionID, ex.Message));
                        //HARDCODED
                        XCommon.Utils.Helper_SendEmail("athiasjerome@gmail.com", "MANAGER ENGINE ERROR", "THREADLAUNCHSESSION : Error contacting the agent. Exception =" + ex.Message + " " + ex.InnerException);
                        return;
                    }

                    Utils.Helper_Trace("MANAGER ENGINE", string.Format("SESSION {0} : THREADLAUNCHSESSION : The agent has been successfully contacted", sessionID));

                    // =====================================
                    //TODO Update table JOB (column AgentID)
                    // =====================================

                    Utils.Helper_Trace("MANAGER ENGINE", string.Format("SESSION {0} : THREADLAUNCHSESSION : TODO Updating table JOB (AgentID column)", sessionID));
                    //TODO
                    //J.AgentID = bestAgent.AgentID;

                    //TODO: TryCatch
                    model.SaveChanges();

                }

                // =================================
                // Wait until all jobs have finished
                // =================================

                Utils.Helper_Trace("MANAGER ENGINE", string.Format("SESSION {0} : THREADLAUNCHSESSION : Waiting until all jobs have finished", sessionID));
                
                try
                {
                    bool bFinished = false;
                    do
                    {
                        Thread.Sleep(10000);    //HARDCODED

                        bFinished = true;
                        nbjoberror = 0;

                        //                    Utils.Helper_Trace("MANAGER ENGINE", string.Format("SESSION {0} : THREADLAUNCHSESSION : There are {1} jobs in this session", sessionID, info.ListJob.Count.ToString()));

                        var myCurrentSession = from Sess in model.SESSION
                                               where Sess.SessionID == sessionID
                                               select Sess;
                        SESSION CurrentSession = myCurrentSession.ToList().First();

                        foreach (JOB J in info.ListJob)
                        {
                            int AllJobs = 0;

                            // ==========================
                            // Get the status of this job
                            // ==========================

                            var MyJob = from MyJobs in model.JOB
                                        where MyJobs.JobID == J.JobID
                                        select MyJobs.Status;

                            string jobStatus = (string)MyJob.ToList().First();

                            if (jobStatus != XCommon.STATUS.FINISHED.ToString() && jobStatus != XCommon.STATUS.ERROR.ToString())
                            {
                                bFinished = false;
                                break;
                            }
                            else
                            {
                                if (jobStatus == XCommon.STATUS.ERROR.ToString())
                                {
                                    nbjoberror++;
                                }
                                AllJobs++;
                            }

                            int pourcent = (100 / (int)MyJob.Count()) * AllJobs;

                            // CurrentSession.Status = pourcent + "%";

                            //model.SaveChanges();
                        }
                    }
                    while (bFinished == false);
                }
                catch (ThreadAbortException ex)
                {
                    Utils.Helper_Trace("MANAGER ENGINE", string.Format("SESSION {0} : THREADLAUNCHSESSION : Thread has been aborted", sessionID));
                    Utils.Helper_Trace("MANAGER ENGINE", string.Format("ThreadAbortException : Exception = {0}", ex.Message));
                    return;
                }

                Utils.Helper_Trace("MANAGER ENGINE", string.Format("SESSION {0} : THREADLAUNCHSESSION : All jobs have finished ({1} Errors)", sessionID, nbjoberror));
            }

            // =============================
            // Update table SESSION (Status)
            // =============================

            Utils.Helper_Trace("MANAGER ENGINE", string.Format("SESSION {0} : THREADLAUNCHSESSION : Updating status in table SESSION", sessionID));

            var mySession = from Sess in model.SESSION
                            where Sess.SessionID == sessionID
                            select Sess;
            SESSION MySession = mySession.ToList().First();

            string additionalmailMessage = string.Empty;
            //Dealing with jobs in error
            if (info.ListJob.Count == nbjoberror)
            {
                //TODO xxx

                MySession.Status = XCommon.STATUS.ERROR.ToString();
                Utils.Helper_Trace("MANAGER ENGINE", string.Format("SESSION {0} : THREADLAUNCHSESSION : All jobs in error: xxx", sessionID));
                //TODO xxx
            }
            else
            {
                MySession.Status = XCommon.STATUS.FINISHED.ToString();
            }
            MySession.DateEnd   = DateTimeOffset.Now;

            model.SaveChanges();            

            // ============
            // Notification
            // ============

            //TODO
            //XCommon.Utils.Helper_Notify(session.UserID.Value, "TASK_JOB", sessionID.ToString(), XCommon.RIGHT.MODIFY);

            //TODO
            /*
            string mailMessage = "Hello "+ session.aspnet_Membership.USERS.UserName +". Your Hackenaton session " + sessionID + " ("+ MySession.SERVICECATEGORY.ServiceCategoryName +") is completed with the status: "+ MySession.Status+". Assets scanned: "; //HARDCODED
            var myAssets = from assets in model.ASSET
                           join assetinsess in model.ASSETSESSION on assets.AssetID equals assetinsess.AssetID
                           where assetinsess.SessionID == sessionID
                           select assets;
            foreach (ASSET ass in myAssets)
            {
                //TODO  ipaddressIPv4
                mailMessage += ass.ipaddressIPv4 + " ";
            }
            mailMessage += "Completed in " + string.Format("{0:00}:{1:00}:{2:00}", (MySession.DateEnd.Value - MySession.DateStart.Value).Hours, (MySession.DateEnd.Value - MySession.DateStart.Value).Minutes, (MySession.DateEnd.Value - MySession.DateStart.Value).Seconds);
            mailMessage +=". Visit the Hackenaton website to display the Report.";
            mailMessage += additionalmailMessage;
            XCommon.Utils.Helper_SendEmail(session.aspnet_Membership.Email, "Hackenaton scan finished", mailMessage);   //HARDCODED
            */

            m_ListRunningSessionThread.Remove(sessionID);

            Utils.Helper_Trace("MANAGER ENGINE", string.Format("SESSION {0} : THREADLAUNCHSESSION : Finished", sessionID));
        }

        private void FuncThreadCancelSession(object context)
        {
            LaunchSessionThreadInfo info;
            info = (LaunchSessionThreadInfo)context;

            int sessionID;
            sessionID = info.SessionID;

            XORCISMEntities model = new XORCISMEntities();

            SESSION session;
            session = model.SESSION.SingleOrDefault(o => o.SessionID == sessionID);

            //int accountID;
            //accountID = (int)model.USERACCOUNT.FirstOrDefault(o => o.UserID == session.UserID).AccountID;

            // =============================
            // Cancel the jobs on the agents
            // =============================

            Utils.Helper_Trace("MANAGER ENGINE", string.Format("SESSION {0} : THREADCANCELSESSION : Cancelling jobs on agents", sessionID));

            foreach (JOB J in info.ListJob)
            {
                Utils.Helper_Trace("MANAGER ENGINE", string.Format("SESSION {0} : THREADCANCELSESSION : Handling job {1}", sessionID, J.JobID));

                // ====================================
                // Contact the agent and cancel the job
                // ====================================

                Utils.Helper_Trace("MANAGER ENGINE", string.Format("SESSION {0} : THREADCANCELSESSION : Trying to contact the agent", sessionID));

                try
                {
                    ServiceReferenceAgent.Service1Client service;
                    service = new ServiceReferenceAgent.Service1Client();

                    // TODO :
                    // service.Endpoint.Address = bestAgent.IPAddress;

                    service.CancelJob(J.JobID);
                }
                catch (Exception ex)
                {
                    Utils.Helper_Trace("MANAGER ENGINE", string.Format("SESSION {0} : THREADCANCELSESSION : Error contacting the agent. Exception = {1} {2}", sessionID, ex.Message, ex.InnerException));
                    //HARDCODED
                    XCommon.Utils.Helper_SendEmail("athiasjerome@gmail.com", "MANAGER ENGINE ERROR", "THREADCANCELSESSION : Error contacting the agent. Exception ="+ex.Message+" "+ex.InnerException);
                    return;
                }

                Utils.Helper_Trace("MANAGER ENGINE", string.Format("SESSION {0} : THREADCANCELSESSION : The agent has been successfully contacted", sessionID));
            }

            // =============================
            // Update table SESSION (Status)
            // =============================

            Utils.Helper_Trace("MANAGER ENGINE", string.Format("SESSION {0} : THREADCANCELSESSION : Updating status in table SESSION to CANCELED", sessionID));

            var mySession = from Sess in model.SESSION
                            where Sess.SessionID == sessionID
                            select Sess;
            SESSION MySession = mySession.ToList().First();

            MySession.Status = XCommon.STATUS.CANCELED.ToString();
            MySession.DateEnd = DateTimeOffset.Now;

            model.SaveChanges();
            try
            {
                m_ListRunningSessionThread.Remove(sessionID);
            }
            catch (Exception ex)
            {
                Utils.Helper_Trace("MANAGER ENGINE", string.Format("SESSION {0} : THREADCANCELSESSION : Error m_ListRunningSessionThread.Remove. Exception = {1}", sessionID, ex.Message));
                return;
            }


            Utils.Helper_Trace("MANAGER ENGINE", string.Format("SESSION {0} : THREADCANCELSESSION : Finished", sessionID));
        }

        private void service_LaunchJobCompleted(object sender, AsyncCompletedEventArgs e)
        {

        }
    }
}
