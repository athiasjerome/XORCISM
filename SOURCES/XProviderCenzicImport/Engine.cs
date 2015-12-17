using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Serialization;
using System.Configuration;
using System.Threading;
using System.Diagnostics;
using System.Reflection;


using System.Text.RegularExpressions;

using XCommon;
using XProviderCommon;
using XORCISMModel;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

using System.Data;

using System.Data.SqlClient;

/*
 NOTES:
 IE http://1.22.333.4/Hailstorm.WS/ScanService.svc
 modifier le fichier hosts
 1.222.1.192	5b67af41664f946b
*/
namespace XProviderCenzicImport
{
    /// <summary>
    /// Copyright (C) 2012-2015 Jerome Athias
    /// XORCISM Plugin to import Cenzic Hailstorm (old version) scan results
    /// All trademarks and registered trademarks are the property of their respective owners.
    /// This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation; either version 2 of the License, or (at your option) any later version.
    /// 
    /// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
    /// 
    /// You should have received a copy of the GNU General Public License along with this program; if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
    /// </summary>
    public class Engine : MarshalByRefObject, XProviderCommon.IVulnerabilityImporter
    {
        static bool inerror = false;

        public Engine()
        {
            TextWriterTraceListener tw;
            tw = new TextWriterTraceListener("XProviderCenzicImport.log");  //Hardcoded

            Trace.AutoFlush = true;
            Trace.IndentSize = 4;
            Trace.Listeners.Add(tw);
        }

        //public void Run(string target, int jobID, string policy, string strategy)
        public void Run(string data, int jobID, int AccountID)
        {
            Utils.Helper_Trace("XORCISM PROVIDER Cenzic Import", "JobID:" + jobID + " Entering Run()");
            Utils.Helper_Trace("XORCISM PROVIDER Cenzic Import", string.Format("Creating an instance of CenzicParser for AccountID=" + AccountID.ToString()));

            CenzicParser CenzicParser = new CenzicParser(data, AccountID, jobID);
            inerror = false;
            if (!inerror)
            {
                Utils.Helper_Trace("XORCISM PROVIDER Cenzic Import", string.Format("JobID:" + jobID + " Parsing the data"));
                CenzicParser.parse();
                Utils.Helper_Trace("XORCISM PROVIDER Cenzic Import", string.Format("JobID:" + jobID + " End of data processing"));

                Utils.Helper_Trace("XORCISM PROVIDER Cenzic Import", string.Format("Updating job {0} status to FINISHED", jobID));

                CenzicParser.UpdateJob(jobID);

                XORCISMEntities model = new XORCISMEntities();
                var xJob = from j in model.JOB
                           where j.JobID == jobID
                           select j;

                JOB xJ = xJob.FirstOrDefault();
                xJ.Status = XCommon.STATUS.FINISHED.ToString();

                Utils.Helper_Trace("XORCISM PROVIDER Cenzic Import", "Changing the session to ServiceCategoryID=2");
                var xSession = from s in model.SESSION
                               where s.SessionID == xJ.SessionID
                               select s;
                SESSION xS = xSession.FirstOrDefault();
                xS.ServiceCategoryID = 2;

                model.SaveChanges();
            }
            else
            {
                Utils.Helper_Trace("XORCISM PROVIDER Cenzic Import", "JobID:" + jobID + " inerror");
            }
            Utils.Helper_Trace("XORCISM PROVIDER Cenzic Import", "JobID:" + jobID + " Leaving Run()");
        }

        
/*
        private VULNERABILITYFOUND PersisteVuln(string cve, string CenzicID, XmlNode diag, XmlNode consequence, XmlNode solution, int endPointID, string severity)
        {
            return null;
        }
*/
        private int SearchForCenzicID(string CenzicID)
        {
            return -1;
        }

        class CenzicParser
        {
            private string m_data;
            private int m_AccountID;
            private int m_jobId;

            public CenzicParser(string data, int AccountID, int jobid)
            {
                m_AccountID = AccountID;
                m_data = data;
                m_jobId = jobid;
            }

            

            public void parse()
            {
                Assembly a;
                a = Assembly.GetExecutingAssembly();

                Utils.Helper_Trace("XORCISM PROVIDER Cenzic Import", "Assembly location = " + a.Location);

                XmlDocument doc = new XmlDocument();

                #region HackCenzic
                /*
                string filename;
                filename = @"C:\Cenzic_webscan.xml";             //Hardcoded   
                
                doc.Load(filename);

                Utils.Helper_Trace("XORCISM PROVIDER Cenzic Import", string.Format("HackFile should be located at : " + filename));
                */
                #endregion

                // ============================================
                // Parse the XML Document and populate the database
                // ============================================

                string protocol = string.Empty;
                //int port = -1;
                string service = string.Empty;
                //bool PatchUpgrade = false;
                //string title;
                //string MSPatch = "";
                //string Solution;

                m_data = m_data.Replace("Configurable format #", "Configurable");   //Hardcoded
                Utils.Helper_Trace("XORCISM PROVIDER Cenzic Import", string.Format("m_data = {0}", m_data));                
                try
                {
                    Utils.Helper_Trace("XORCISM PROVIDER Cenzic Import", "Loading the XML document");
                    
                    doc.LoadXml(m_data);
                    
                }
                catch (Exception ex)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER Cenzic Import", string.Format("Exception = {0} / {1}", ex.Message, ex.InnerException == null ? "" : ex.InnerException.Message));
                }
                

                XORCISMEntities model;
                model = new XORCISMEntities();

                string query = "/AssessmentRunData/SmartAttacks/SmartAttacksData";  //Hardcoded

                XmlNodeList report;
                report = null;
                try
                {
                    report = doc.SelectNodes(query);
                }
                catch (Exception ex)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER Cenzic Import", string.Format("Error SelectNodes({0}) : Exception = {1}", query, ex.Message));
                    return;
                }

                //We should retrieve the target for an import
                string m_target = string.Empty;
                string patterntoken = "<Url>(.*?)</Url>";
                MatchCollection matchesurl = Regex.Matches(m_data, patterntoken);
                foreach (Match match in matchesurl)
                {
                    m_target = match.Value.Replace("<Url>", "").Replace("</Url>", "");
                    //Console.WriteLine(mytoken);
                    Utils.Helper_Trace("XORCISM PROVIDER Cenzic Import", "target: " + m_target);
                }


                int myPort = 80;
                if (m_target.Contains("https://"))
                {
                    myPort = 443;
                }
                //Check if we have a custom port, ex: http://10.20.30.40:8080/test
                string strTargetTest = m_target;
                strTargetTest = strTargetTest.Replace("http://", "");
                strTargetTest = strTargetTest.Replace("https://", "");
                try
                {
                    if (strTargetTest.Contains(":"))
                    {
                        char[] splitter = { ':' };
                        string[] strSplit = strTargetTest.Split(splitter);
                        strTargetTest = strSplit[1];
                        if (strTargetTest.Contains("/"))
                        {
                            strSplit = strTargetTest.Split(new Char[] { '/' });
                            strTargetTest = strSplit[0];
                        }
                        try
                        {
                            myPort = Convert.ToInt32(strTargetTest);
                        }
                        catch (FormatException e)
                        {
                            Utils.Helper_Trace("XORCISM PROVIDER Cenzic Import", strTargetTest + " is not a sequence of digits.");
                        }
                        Utils.Helper_Trace("XORCISM PROVIDER Cenzic Import", string.Format("Custom Port:{0}", strTargetTest));
                    }
                    else
                    {
                        if (strTargetTest.Contains("/"))
                        {
                            string[] strSplit = strTargetTest.Split(new Char[] { '/' });
                            strTargetTest = strSplit[0];
                            if (m_target.Contains("https://"))
                            {
                                m_target = "https://" + strTargetTest;
                            }
                            if (m_target.Contains("http://"))
                            {
                                m_target = "http://" + strTargetTest;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER Cenzic Import", string.Format("Error in strTargetTest : Exception = {0}", ex.Message));
                }


                Utils.Helper_Trace("XORCISM PROVIDER Cenzic Import", "the m_target=" + m_target);

                // ===============================================
                // If necessary, creates an asset in the database
                // ===============================================
                //TODO
                var myass = from ass in model.ASSET
                            where ass.ipaddressIPv4 == m_target //&& ass.AccountID == m_AccountID
                            select ass;
                ASSET asset = myass.FirstOrDefault();

                if (asset == null)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER Cenzic Import", "Creates a new entry in table ASSET for this IP");

                    asset = new ASSET();
                    //asset.AccountID = m_AccountID;
                    asset.AssetName = m_target;
                    asset.AssetDescription = m_target;
                    asset.ipaddressIPv4 = m_target;
                    asset.Enabled = true;
                    //asset.JobID = m_jobId;

                    model.ASSET.Add(asset);
                    model.SaveChanges();
                }
                else
                {
                    Utils.Helper_Trace("XORCISM PROVIDER Cenzic Import", "This IP already corresponds to an existing asset");
                }

                int m_assetId = asset.AssetID;
                int m_sessionId = (int)model.JOB.Single(x => x.JobID == m_jobId).SessionID;

                Utils.Helper_Trace("XORCISM PROVIDER Cenzic Import", "Creating ASSETINSESSION reference");
                ASSETSESSION assinsess = new ASSETSESSION();
                assinsess.AssetID = asset.AssetID;
                assinsess.SessionID = m_sessionId;  // model.JOB.Single(x => x.JobID == m_jobId).SessionID;
                model.ASSETSESSION.Add(assinsess);
                model.SaveChanges();

                Utils.Helper_Trace("XORCISM PROVIDER Cenzic Import", "Update JOB with ASSETINSESSIONID");
                JOB daJob = model.JOB.Single(x => x.JobID == m_jobId);
                daJob.AssetSessionID = assinsess.AssetSessionID;
                model.SaveChanges();


                VulnerabilityEndPoint vulnerabilityEndPoint = new VulnerabilityEndPoint();
                vulnerabilityEndPoint.IpAdress = m_target;
                vulnerabilityEndPoint.Protocol = "TCP"; // "http";
                vulnerabilityEndPoint.Port = myPort;
                vulnerabilityEndPoint.Service = "WWW";

                int myEndpointID = 0;
                var testEndpoint = from e in model.ENDPOINT
                                   where e.AssetID == m_assetId && e.SessionID == m_sessionId
                                   select e;
                if (testEndpoint.Count() == 0)
                {
                    ENDPOINT newEndpoint = new ENDPOINT();
                    newEndpoint.AssetID = m_assetId;
                    newEndpoint.SessionID = m_sessionId;
                    newEndpoint.ProtocolName = "TCP"; // "http";
                    newEndpoint.PortNumber = myPort;
                    newEndpoint.Service = "WWW";
                    model.ENDPOINT.Add(newEndpoint);
                    model.SaveChanges();
                    myEndpointID = newEndpoint.EndPointID;
                }
                else
                {
                    myEndpointID = testEndpoint.FirstOrDefault().EndPointID;
                }
                Utils.Helper_Trace("XORCISM PROVIDER Cenzic Import", string.Format("myEndpointID:{0}", myEndpointID));



                Utils.Helper_Trace("XORCISM PROVIDER Cenzic Import", string.Format("JobID:" + m_jobId + " Found {0} SmartAttacks to parse", report.Count));

                foreach (XmlNode reportHost in report)
                {
                    // ==================================
                    // Handle every SmartAttacksData tag
                    // ==================================

                    
                    string myInnerXml = string.Empty;
                    string myTitle = string.Empty;
                    string myDescription = string.Empty;
                    string myConsequence = string.Empty;
                    string myResult = string.Empty;
                    string mySolution = string.Empty;

                    string myCVE = string.Empty;
                    MatchCollection myCVEs;
                    string myPCI = string.Empty;
                    string myMessage = string.Empty;

                    foreach (XmlNode n in reportHost.ChildNodes)
                    {
                        //SmartAttackInfo
                        //ReportItems
                        XmlNodeList Childs = n.ChildNodes;

                        Utils.Helper_Trace("XORCISM PROVIDER Cenzic Import", string.Format("Coucou 1"));
                        try
                        {
                            if (n.Name == "SmartAttackInfo")
                            {
                                myInnerXml = n.OuterXml;
                                myTitle = HelperGetChildInnerText(n, "SmartAttackName");
                                Utils.Helper_Trace("XORCISM PROVIDER Cenzic Import", string.Format("JobID:" + m_jobId + " Found SmartAttackName:{0}", myTitle));
                                Regex myRegex = new Regex("PCI [0-9].[0-9].[0-9]");

                                myPCI = myRegex.Match(myTitle).ToString();
                                if (myPCI != "")
                                {
                                    Utils.Helper_Trace("XORCISM PROVIDER Cenzic Import", "PCI=" + myPCI);
                                }

                                //Utils.Helper_Trace("XORCISM PROVIDER Cenzic Import", string.Format("myInnerXml:{0}", myInnerXml));
                                //Hardcoded
                                myDescription = HelperGetChildInnerText(n, "Description");
                                myConsequence = HelperGetChildInnerText(n, "HowItWorks");
                                myResult = HelperGetChildInnerText(n, "Impact");
                                mySolution = HelperGetChildInnerText(n, "Remediation");
                            }
                        }
                        catch (Exception ex)
                        {
                            Utils.Helper_Trace("XORCISM PROVIDER Cenzic Import", string.Format("JobID:" + m_jobId + " Error in SmartAttackInfo : Exception = {0}", ex.Message));
                        }
                        if (n.Name == "ReportItems")
                        {
                            Utils.Helper_Trace("XORCISM PROVIDER Cenzic Import", string.Format("Coucou 2"));
                            foreach (XmlNode x in n.ChildNodes)
                            {
                                //HARDCODED
                                //ReportItem
                                foreach (XmlNode ReportItem in x.ChildNodes)
                                {
                                    myMessage = "";
                                    if (ReportItem.Name == "ReportItemType")
                                    {
                                        //Pass
                                        if (ReportItem.InnerText == "Information")
                                        {
                                            try
                                            {
                                                //TODO
                                                /*
                                                Utils.Helper_Trace("XORCISM PROVIDER Cenzic Import", string.Format("Information"));
                                                INFORMATION newInformation = new INFORMATION();
                                                //newInformation.InnerXml
                                                newInformation.Title = myTitle;
                                                newInformation.Description = myDescription;
                                                newInformation.Consequence = myConsequence;
                                                newInformation.Result = myResult;
                                                newInformation.Solution = mySolution;
                                                newInformation.Severity = HelperGetChildInnerText(x, "Severity");
                                                newInformation.HarmScore = int.Parse(HelperGetChildInnerText(x, "HarmScore"));
                                                myMessage = HelperGetChildInnerText(x, "Message");
                                                newInformation.Message = myMessage;
                                                //TODO A FAIRE
                                                //Matching avec les références
                                                //http://www.securityfocus.com/bid/43140/info 
                                                //http://www.securityfocus.com/bid/43140/solution 
                                                newInformation.Url = HelperGetChildInnerText(x, "Url");
                                                newInformation.rawrequest = HelperGetChildInnerText(x, "HttpRequest");
                                                newInformation.rawresponse = HelperGetChildInnerText(x, "HttpResponse");
                                                if (myPCI != "")
                                                {
                                                    newInformation.PCI_FLAG = true;
                                                }
                                                newInformation.JobID = m_jobId;
                                                newInformation.EndPointID = myEndpointID;
                                                model.AddToINFORMATION(newInformation);
                                                model.SaveChanges();
                                                */
                                            }
                                            catch (Exception ex)
                                            {
                                                Utils.Helper_Trace("XORCISM PROVIDER Cenzic Import", string.Format("JobID:" + m_jobId + " Error in Information : Exception = {0}. {1}", ex.Message, ex.InnerException));
                                            }
                                        }
                                        if (ReportItem.InnerText == "Warning")
                                        {
                                            try
                                            {
                                                Utils.Helper_Trace("XORCISM PROVIDER Cenzic Import", string.Format("Warning"));
                                                VulnerabilityFound vulnerabilityFound = new VulnerabilityFound();
                                                vulnerabilityFound.InnerXml = myInnerXml;
                                                vulnerabilityFound.Title = myTitle;
                                                Utils.Helper_Trace("XORCISM PROVIDER Cenzic Import", string.Format("Adding SmartAttackName:{0}", myTitle));
                                                vulnerabilityFound.Description = myDescription;
                                                vulnerabilityFound.Consequence = myConsequence;
                                                vulnerabilityFound.Result = myResult;
                                                vulnerabilityFound.Solution = mySolution;

                                                if (myPCI != "")
                                                {
                                                    vulnerabilityFound.PCI_FLAG = true;
                                                }

                                                //ReportItemCreateDate
                                                vulnerabilityFound.Severity = HelperGetChildInnerText(x, "Severity");
                                                //Low, Medium, High
                                                //Utils.Helper_Trace("XORCISM PROVIDER Cenzic Import", string.Format("WARNING Severity:{0}", HelperGetChildInnerText(x, "Severity")));
                                                vulnerabilityFound.HarmScore = int.Parse(HelperGetChildInnerText(x, "HarmScore"));
                                                //Count
                                                myMessage=HelperGetChildInnerText(x, "Message");
                                                //vulnerabilityFound.Message = myMessage; //not exact because same VULNERABILITY will have various Messages
                                                vulnerabilityFound.rawresponse = myMessage;
                                            
                                                    //Regex objNaturalPattern = new Regex("CVE-[0-9][0-9][0-9][0-9]-[0-9][0-9][0-9][0-9]");
                                                    Regex myRegexCVE = new Regex(@"CVE-(19|20)\d\d-(0\d{3}|[1-9]\d{3,})");  //TODO: Update this?
                                                    //https://cve.mitre.org/cve/identifiers/tech-guidance.html
                                                    /*
                                                    myCVE = objNaturalPattern.Match(myMessage).ToString();                                               
                                                    if (myCVE != "")
                                                    {
                                                        Utils.Helper_Trace("XORCISM PROVIDER Cenzic Import", "CVE=" + myCVE);
                                                    }
                                                    */
                                                    List<VulnerabilityFound.Item> l;
                                                    l = new List<VulnerabilityFound.Item>();
                                                    myCVEs = myRegexCVE.Matches(myMessage);
                                                    foreach (Match match in myCVEs)
                                                    {
                                                        foreach (Capture capture in match.Captures)
                                                        {
                                                            Utils.Helper_Trace("XORCISM PROVIDER Cenzic Import", string.Format("Index={0}, CVE={1}", capture.Index, capture.Value));
                                                            VulnerabilityFound.Item item;
                                                            item = new VulnerabilityFound.Item();
                                                            item.Value = capture.Value;
                                                            item.ID = "cve";
                                                            l.Add(item);
                                                        }
                                                    }
                                                    vulnerabilityFound.ListItem = l;


                                                vulnerabilityFound.Url = HelperGetChildInnerText(x, "Url");
                                                Utils.Helper_Trace("XORCISM PROVIDER Cenzic Import", string.Format("Url={0}", HelperGetChildInnerText(x, "Url")));
                                                vulnerabilityFound.rawrequest = HelperGetChildInnerText(x, "HttpRequest");
                                                //vulnerabilityFound.rawresponse = HelperGetChildInnerText(x, "HttpResponse");
                                                //StructuredData

                                                //*** Compliances? voir en bas
                                                //http://www.cenzic.com/downloads/Cenzic_CWE.pdf
                                                int VulnID = VulnerabilityPersistor.Persist(vulnerabilityFound, vulnerabilityEndPoint, m_jobId, "cenzic", model);
                                            }
                                            catch (Exception ex)
                                            {
                                                Utils.Helper_Trace("XORCISM PROVIDER Cenzic Import", string.Format("JobID:" + m_jobId + " Error in Warning : Exception = {0}. {1}", ex.Message, ex.InnerException));
                                            }
                                        }
                                        if (ReportItem.InnerText == "Vulnerable")
                                        {
                                            try
                                            {
                                                Utils.Helper_Trace("XORCISM PROVIDER Cenzic Import", string.Format("Vulnerable"));
                                                VulnerabilityFound vulnerabilityFound = new VulnerabilityFound();
                                                vulnerabilityFound.InnerXml = myInnerXml;
                                                vulnerabilityFound.Title = myTitle;
                                                Utils.Helper_Trace("XORCISM PROVIDER Cenzic Import", string.Format("Adding SmartAttackName:{0}", myTitle));
                                                vulnerabilityFound.Description = myDescription;
                                                vulnerabilityFound.Consequence = myConsequence;
                                                vulnerabilityFound.Result = myResult;
                                                vulnerabilityFound.Solution = mySolution;

                                                //ReportItemCreateDate
                                                vulnerabilityFound.Severity = HelperGetChildInnerText(x, "Severity");
                                                //Utils.Helper_Trace("XORCISM PROVIDER Cenzic Import", string.Format("VULNERABLE Severity:{0}", HelperGetChildInnerText(x, "Severity")));
                                                vulnerabilityFound.HarmScore = int.Parse(HelperGetChildInnerText(x, "HarmScore"));
                                                //Count
                                                myMessage = HelperGetChildInnerText(x, "Message");
                                                //vulnerabilityFound.Message = myMessage;
                                                vulnerabilityFound.rawresponse = myMessage;

                                                    //Regex objNaturalPattern = new Regex("CVE-[0-9][0-9][0-9][0-9]-[0-9][0-9][0-9][0-9]");
                                                Regex myRegexCVE = new Regex(@"CVE-(19|20)\d\d-(0\d{3}|[1-9]\d{3,})");
                                                //https://cve.mitre.org/cve/identifiers/tech-guidance.html
                                                    /*
                                                    myCVE = objNaturalPattern.Match(myMessage).ToString();
                                                    if (myCVE != "")
                                                    {
                                                        Utils.Helper_Trace("XORCISM PROVIDER Cenzic Import", "CVE=" + myCVE);
                                                    }
                                                    */
                                                
                                                    List<VulnerabilityFound.Item> l;
                                                    l = new List<VulnerabilityFound.Item>();
                                                    myCVEs = myRegexCVE.Matches(myMessage);
                                                    foreach (Match match in myCVEs)
                                                    {
                                                        foreach (Capture capture in match.Captures)
                                                        {
                                                            Utils.Helper_Trace("XORCISM PROVIDER Cenzic Import", string.Format("Index={0}, CVE={1}", capture.Index, capture.Value));
                                                            VulnerabilityFound.Item item;
                                                            item = new VulnerabilityFound.Item();
                                                            item.Value = capture.Value;
                                                            item.ID = "cve";
                                                            l.Add(item);
                                                        }
                                                    }
                                                    vulnerabilityFound.ListItem = l;
                                            
                                                vulnerabilityFound.Url = HelperGetChildInnerText(x, "Url");
                                                Utils.Helper_Trace("XORCISM PROVIDER Cenzic Import", string.Format("Url={0}", HelperGetChildInnerText(x, "Url")));
                                                vulnerabilityFound.rawrequest = HelperGetChildInnerText(x, "HttpRequest");
                                                //vulnerabilityFound.rawresponse = HelperGetChildInnerText(x, "HttpResponse");                                            
                                                //StructuredData

                                                if (myPCI != "")
                                                {
                                                    //TODO
                                                    /*
                                                    vulnerabilityFound.PCI_FLAG = true;
                                                    int VulnID = VulnerabilityPersistor.Persist(vulnerabilityFound, vulnerabilityEndPoint, m_jobId, "cenzic", model);

                                                    List<int> myIds = new List<int>();
                                                    var id = from o in model.COMPLIANCECATEG
                                                             where o.Title == myTitle &&
                                                             o.COMPLIANCE.Title == "PCIDSS"
                                                             select o.ComplianceCategID;
                                                    int Id = id.FirstOrDefault();

                                                    myIds.Add(Id);

                                                    List<int> Compliances = new List<int>();
                                                    Compliances = myIds;
                                                    Utils.Helper_Trace("XORCISM PROVIDER Cenzic Import", string.Format("Vulnerability persisted , VulnID = {0} & Compliance count = {1}", VulnID, Compliances.Count));
                                                    var V = from tmpVuln in model.VULNERABILITYFOUND
                                                            where tmpVuln.VulnerabilityFoundID == VulnID
                                                            select tmpVuln;

                                                    VULNERABILITYFOUND VF = V.FirstOrDefault();

                                                    foreach (int i in Compliances)
                                                    {
                                                        Utils.Helper_Trace("XORCISM PROVIDER Cenzic Import", string.Format("Categorie Compliance => ", i));
                                                        var C = from Comp in model.COMPLIANCECATEG
                                                                where Comp.ComplianceCategID == i
                                                                select Comp;

                                                        COMPLIANCECATEG myCompliance = new COMPLIANCECATEG();
                                                        myCompliance = C.FirstOrDefault();

                                                        VF.COMPLIANCECATEG.Add(myCompliance);

                                                        model.SaveChanges();
                                                        Utils.Helper_Trace("XORCISM PROVIDER Cenzic Import", "Mapping Compliance-Vulnerability Added");
                                                    }
                                                    */
                                                }
                                                else
                                                {
                                                    int VulnID = VulnerabilityPersistor.Persist(vulnerabilityFound, vulnerabilityEndPoint, m_jobId, "cenzic", model);
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Utils.Helper_Trace("XORCISM PROVIDER Cenzic Import", string.Format("Error in Vulnerable : Exception = {0}. {1}", ex.Message, ex.InnerException));
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }












            private string HelperGetChildInnerText(XmlNode n, string ChildName)
            {
                foreach (XmlNode child in n.ChildNodes)
                {
                    if (child.Name.ToUpper() == ChildName.ToUpper())
                        return child.InnerText;
                }
                return string.Empty;
            }

            public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            {
                return true;
            }

            private string Helper_ListCVEToString(List<VulnerabilityFound.Item> list)
            {
                string s = "";

                foreach (VulnerabilityFound.Item item in list)
                    s = s + item.ID + ":" + item.Value + " / ";

                return s;
            }

            private List<VulnerabilityFound.Item> Helper_GetCVE(XmlNode node)
            {
                List<VulnerabilityFound.Item> l;
                l = new List<VulnerabilityFound.Item>();
                try
                {
                    XmlNodeList nodes = node.ChildNodes;
                    foreach (XmlNode n in nodes)
                    {
                        if (n.Attributes["type"] != null)
                        {
                            VulnerabilityFound.Item item = new VulnerabilityFound.Item();
                            item.ID = n.Attributes["type"].InnerText;
                            item.Value = n.InnerText;
                            l.Add(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER Cenzic Import", string.Format("JobID:" + m_jobId + " Error in Helper_GetCVE : Exception = {0}", ex.Message));
                }
                return l;
            }

            public void UpdateJob(int JobId)
            {
                XORCISMEntities model = new XORCISMEntities();
                var Q = from o in model.JOB
                        where o.JobID == JobId
                        select o;
                JOB myJob = Q.FirstOrDefault();
                myJob.Status = XCommon.STATUS.FINISHED.ToString();
                myJob.DateEnd = DateTimeOffset.Now;
                model.SaveChanges();
            }
            
        }

        internal class AcceptAllCertificatePolicy : ICertificatePolicy
        {
            public AcceptAllCertificatePolicy()
            {
            }

            public bool CheckValidationResult(ServicePoint sPoint,
               X509Certificate cert, WebRequest wRequest, int certProb)
            {
                // *** Always accept
                return true;
            }
        }
        /*
        class MyTestService : HailstormWebService.ScanServiceClient
        {   
            protected override WebRequest GetWebRequest(Uri uri)
            {
                HttpWebRequest webRequest = (HttpWebRequest)base.GetWebRequest(uri);
                //Setting KeepAlive to false
                webRequest.KeepAlive = false;
                return webRequest;
            }
        }
        */
    }
}
