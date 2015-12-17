using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Diagnostics;
using System.Xml.Serialization;
using System.IO;

using XORCISMModel;
using XCommon;

namespace XManagerService
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
    public class Service1 : IService1 
    {
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        //public CompositeType GetDataUsingDataContract(CompositeType composite)
        //{
        //    if (composite == null)
        //    {
        //        throw new ArgumentNullException("composite");
        //    }
        //    if (composite.BoolValue)
        //    {
        //        composite.StringValue += "Suffix";
        //    }
        //    return composite;
        //}

        public int CreateSession(int serviceCategoryID, Guid userID, byte[] parameters, Decimal PeasCount, Decimal PeasValue)
        {
            //Where Magic happens
            Utils.Helper_Trace("MANAGER SERVICE", "Entering CreateSession()");
            
            Utils.Helper_Trace("MANAGER SERVICE", string.Format("ServiceCategoryID = {0}", serviceCategoryID));
            Utils.Helper_Trace("MANAGER SERVICE", string.Format("UserID            = {0}", userID.ToString()));

            Dictionary<string, object> dicoParameters;

            try
            {
                MemoryStream ms;
                ms = new MemoryStream(parameters);

                BinaryFormatter bf;
                bf = new BinaryFormatter();

                dicoParameters = (Dictionary<string, object>)bf.Deserialize(ms);
            }
            catch (Exception e)
            {
                Utils.Helper_Trace("MANAGER SERVICE", string.Format("Exception while deserializing parameters : {0}", e.Message));
                return -1;
            }

            Utils.Helper_Trace("MANAGER SERVICE", string.Format("Size of parameters = {0} bytes", parameters.Length));

            int[] tabAssetID        = null;
            int MaxPages;
            string FileName         = string.Empty;
            string nmapAddress      = string.Empty;
            string cronExpression   = string.Empty;
            string sip              = string.Empty;
            string extrange         = string.Empty;
            string policy           = string.Empty;
            string strategie        = string.Empty;
           
            switch (serviceCategoryID)  //TODO Hardcoded
            {
                case 1:             // Vulnerability Assessment
                    tabAssetID      = (int[])dicoParameters["ASSETS"];
                    cronExpression  = (string)dicoParameters["CRONEXPRESSION"];
                    policy          = (string)dicoParameters["POLICY"];
                    strategie       = (string)dicoParameters["STRATEGY"];

                    Utils.Helper_Trace("MANAGER SERVICE", string.Format("Number of assets  = {0}", tabAssetID.Length));
                    Utils.Helper_Trace("MANAGER SERVICE", string.Format("Cron Expression   = {0}", cronExpression));

                    break;

                case 2:             //WAS   (Web Application Scanning)
                    tabAssetID      = (int[])dicoParameters["ASSETS"];
                    cronExpression  = (string)dicoParameters["CRONEXPRESSION"];
                    policy          = (string)dicoParameters["POLICY"];
                    strategie       = (string)dicoParameters["STRATEGY"];

                    Utils.Helper_Trace("MANAGER SERVICE", string.Format("Number of assets  = {0}", tabAssetID.Length));
                    Utils.Helper_Trace("MANAGER SERVICE", string.Format("Cron Expression   = {0}", cronExpression));

                    break;
                case 3:
                    Utils.Helper_Trace("MANAGER SERVICE", "NO parameters defined in XManagerService Service1.cs for service category 3");

                    break;
                case 4:             //PCI DSS
                    tabAssetID      = (int[])dicoParameters["ASSETS"];
                    cronExpression  = (string)dicoParameters["CRONEXPRESSION"];
                    policy          = "PCI DSS";
                    strategie       = "Compliance PCI DSS"; //(string)dicoParameters["STRATEGY"];

                    Utils.Helper_Trace("MANAGER SERVICE", string.Format("Number of assets  = {0}", tabAssetID.Length));
                    Utils.Helper_Trace("MANAGER SERVICE", string.Format("Cron Expression   = {0}", cronExpression));

                    break;
                case 5:
                    Utils.Helper_Trace("MANAGER SERVICE", "NO parameters defined in XManagerService Service1.cs for service category 5");
                    
                    break;
                case 6:             // VOIP Scanner
                    tabAssetID = (int[])dicoParameters["ASSETS"];
                    sip = (string)dicoParameters["SIP"];
                    extrange = (string)dicoParameters["EXTRANGE"];
                    Utils.Helper_Trace("MANAGER SERVICE", string.Format("Number of assets  = {0}", tabAssetID.Length));
                    break;

                case 7:             // Web Anti-malware Monitoring
                    tabAssetID = (int[])dicoParameters["ASSETS"];
                    cronExpression = (string)dicoParameters["CRONEXPRESSION"];
                    MaxPages = (int)dicoParameters["MaxPages"];
                    Utils.Helper_Trace("MANAGER SERVICE", string.Format("Service Malware Monitoring:Number of assets  = {0}", tabAssetID.Length));
                    Utils.Helper_Trace("MANAGER SERVICE", string.Format("Cron Expression   = {0}", cronExpression));
                    Utils.Helper_Trace("MANAGER SERVICE", string.Format("Service Malware Monitoring:MaxPages  = {0}", MaxPages));
                    break;

                case 8:             // Web Site Monitoring
                    tabAssetID = (int[])dicoParameters["ASSETS"];
                    Utils.Helper_Trace("MANAGER SERVICE", string.Format("Number of assets  = {0}", tabAssetID.Length));
                    break;

                case 9:
                    Utils.Helper_Trace("MANAGER SERVICE", "NO parameters defined in XManagerService Service1.cs for service category 9");

                    break;
                case 10:            // Discovery

                    nmapAddress = (string)dicoParameters["TARGET"];
                    Utils.Helper_Trace("MANAGER SERVICE", string.Format("Target = {0}", nmapAddress));
                    break;

                case 11:
                case 12:
                case 13:             // Import
                case 14:
                case 15:
                    FileName = (string)dicoParameters["FILENAME"];
                    Utils.Helper_Trace("MANAGER SERVICE", string.Format("Import File : Filename = {0}", FileName));
                    break;

                case 16:             //Information Gathering (OSINT)
                    tabAssetID = (int[])dicoParameters["ASSETS"];
                    cronExpression = (string)dicoParameters["CRONEXPRESSION"];
                    policy = (string)dicoParameters["POLICY"];
                    strategie = (string)dicoParameters["STRATEGY"];

                    Utils.Helper_Trace("MANAGER SERVICE", string.Format("Number of assets  = {0}", tabAssetID.Length));
                    Utils.Helper_Trace("MANAGER SERVICE", string.Format("Cron Expression   = {0}", cronExpression));

                    break;
                
            }

            XORCISMEntities context = new XORCISMEntities();

            // ===============================================
            // Add a new entry in table SESSION or SESSIONCRON
            // ===============================================

            int id;

            if (cronExpression == "")
            {
                // ================================
                // Add a new entry in table SESSION
                // ================================

                Utils.Helper_Trace("MANAGER SERVICE", string.Format("Adding an entry in table SESSION"));

                SESSION tmpSession = new SESSION();
                //Price
                try
                {
                    tmpSession.UserID               = userID;
                    tmpSession.Status = XCommon.STATUS.IDLE.ToString();
                    tmpSession.ServiceCategoryID    = serviceCategoryID;
                    tmpSession.DateEnd              = null;
                    tmpSession.DateStart            = DateTimeOffset.Now;
                    tmpSession.Parameters           = parameters;
                    //Price

                    context.SESSION.Add(tmpSession);

                    context.SaveChanges();
                    //Price
                }
                catch (Exception ex)
                {
                    /*
                    USER user;
                    user = context.USERS.SingleOrDefault(u => u.UserId == userID);
                    ACCOUNT userAccount;
                    userAccount = context.USERACCOUNT.SingleOrDefault(o => o.UserID == user.UserId).ACCOUNT;

                    //Price
                    */
                    Utils.Helper_Trace("MANAGER SERVICE", string.Format("Error adding entry in table SESSION : Exception = {0} - {1}", ex.Message, ex.InnerException.Message));
                    throw ex;
                }

                Utils.Helper_Trace("MANAGER SERVICE", string.Format("SessionID = {0}", tmpSession.SessionID));
                //Random random = new Random();                
                try
                {
                //    tmpSession.SessionID = tmpSession.SessionID + random.Next(20, 200);
                //    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Utils.Helper_Trace("MANAGER SERVICE", string.Format("Error random SESSION : Exception = {0} - {1}", ex.Message, ex.InnerException.Message));
                }
                Utils.Helper_Trace("MANAGER SERVICE", string.Format("NewRandomSessionID = {0}", tmpSession.SessionID));

                id = tmpSession.SessionID;

                // ============================================
                // Add several entries in table ASSETSESSION
                // ============================================

                if (tabAssetID != null)
                {
                    Utils.Helper_Trace("MANAGER SERVICE", string.Format("Adding {0} entries in table ASSETSESSION", tabAssetID.Count()));

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
                        Utils.Helper_Trace("MANAGER SERVICE", string.Format("Error adding entries in table ASSETSESSION : Exception = {0}", ex.Message));
                        throw ex;
                    }
                }
            }
            else
            {
                Utils.Helper_Trace("MANAGER SERVICE", string.Format("Adding an entry in table SESSIONCRON"));

                SESSIONCRON tmpSessionCron = new SESSIONCRON();
                //Price
                try
                {
                    tmpSessionCron.UserID               = userID;
                    tmpSessionCron.CronExpression       = cronExpression;
                    tmpSessionCron.Parameters           = parameters;
                    tmpSessionCron.Status = XCommon.STATUS.IDLE.ToString();
                    tmpSessionCron.ServiceCategoryID    = serviceCategoryID;
                    tmpSessionCron.DateStart            = DateTimeOffset.Now;         //TODO Non il faut que ce soit les dates de start/end du cron A VOIR TODO
                    tmpSessionCron.DateEnd              = null;
                    //Price
                    
                    context.SESSIONCRON.Add(tmpSessionCron);

                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Utils.Helper_Trace("MANAGER SERVICE", string.Format("Error adding entry in table SESSIONCRON : Exception = {0} - {1}", ex.Message, ex.InnerException.Message));
                    throw ex;
                }

                Utils.Helper_Trace("MANAGER SERVICE", string.Format("SessionCronID = {0}", tmpSessionCron.SessionCronID));

                id = tmpSessionCron.SessionCronID;
            }

            Utils.Helper_Trace("MANAGER SERVICE", "Leaving CreateSession()");

            // Finished
            return id;
        }

        public bool CancelSession(int sessionID)
        {
            Utils.Helper_Trace("MANAGER SERVICE", "Entering CancelSession");

            XORCISMModel.XORCISMEntities model;
            model = new XORCISMModel.XORCISMEntities();

            try
            {
                SESSION session;
                session = model.SESSION.SingleOrDefault(s => s.SessionID == sessionID);

                session.Status = XCommon.STATUS.TOCANCEL.ToString();

                model.SaveChanges();                
            }
            catch (Exception ex)
            {
                Utils.Helper_Trace("MANAGER SERVICE", "Exception = " + ex.Message);
                return false;
            }

            Utils.Helper_Trace("MANAGER SERVICE", "Leaving CancelSession");

            // Finished
            return true;
        }

        public bool RestartSession(int sessionID)
        {
            Utils.Helper_Trace("MANAGER SERVICE", "Entering RestartSession");

            XORCISMModel.XORCISMEntities model;
            model = new XORCISMModel.XORCISMEntities();

            try
            {
                SESSION session;
                session = model.SESSION.SingleOrDefault(s => s.SessionID == sessionID);

                if (session.Status != XCommon.STATUS.ERROR.ToString() && session.Status != XCommon.STATUS.CANCELED.ToString())
                {
                    return false;
                }

                session.Status = XCommon.STATUS.IDLE.ToString();

                model.SaveChanges();                
            }
            catch (Exception ex)
            {
                Utils.Helper_Trace("MANAGER SERVICE", "Exception = " + ex.Message);
                return false;
            }

            Utils.Helper_Trace("MANAGER SERVICE", "Leaving RestartSession");
            
            // Finished
            return true;
        }
    }
}
