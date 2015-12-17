using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

using System.Text.RegularExpressions;

using XORCISMModel;
using XCommon;
using XProviderCommon;
using System;

namespace XProviderNessus
{
    public class Engine : MarshalByRefObject, XProviderCommon.IVulnerabilityImporter
    {
        /// <summary>
        /// Copyright (C) 2012-2015 Jerome Athias
        /// XORCISM Plugin for Tenable Nessus. Parses a Nessus report and imports it in XORCISM database
        /// All trademarks and registered trademarks are the property of their respective owners.
        /// This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation; either version 2 of the License, or (at your option) any later version.
        /// 
        /// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
        /// 
        /// You should have received a copy of the GNU General Public License along with this program; if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
        /// </summary>
        
        public Engine()
        {
            TextWriterTraceListener tw;
            tw = new TextWriterTraceListener("XProviderNessus.log");    //Hardcoded
            Trace.Listeners.Add(tw);
            Trace.AutoFlush = true;
            Trace.IndentSize = 4;
        }

        public void Run(string data, int jobID, int AccountID)
        {
            //WARNING: OLD CODE, should be reviewed and revised

            Utils.Helper_Trace("XORCISM PROVIDER NESSUS", "Entering Run()");

            Utils.Helper_Trace("XORCISM PROVIDER NESSUS", string.Format("Creating an instance of NessusParser for AccountID="+AccountID.ToString()));

            NessusParser NessusParser = new NessusParser(data,AccountID,jobID);

            Utils.Helper_Trace("XORCISM PROVIDER NESSUS", string.Format("Parsing the data"));

            NessusParser.parse();

            Utils.Helper_Trace("XORCISM PROVIDER NESSUS", "Updating job status to FINISHED");

            XORCISMEntities model = new XORCISMEntities();
            var xJob = from j in model.JOB
                       where j.JobID == jobID
                       select j;

            JOB xJ = xJob.FirstOrDefault();
            xJ.Status = XCommon.STATUS.FINISHED.ToString();

                Utils.Helper_Trace("XORCISM PROVIDER NESSUS", "Changing the session to ServiceCategoryID=1");                
                var xSession = from s in model.SESSION
                                where s.SessionID == xJ.SessionID
                                select s;
                SESSION xS = xSession.FirstOrDefault();
                xS.ServiceCategoryID = 1;               
                

            model.SaveChanges();
            Utils.Helper_Trace("XORCISM PROVIDER NESSUS", string.Format("End of data processing"));

            Utils.Helper_Trace("XORCISM PROVIDER NESSUS", "Leaving Run()");
        }
    }

    class NessusParser
    {
        private string m_data;
        private int m_AccountID;
        private int m_JobId;

        public NessusParser(string data, int AccountID, int jobid)
        {
            #region HackNessus

            //string filename;
            //filename = @"C:\NessusSampleResults.xml";

            //XmlDocument doc = new XmlDocument();
            //doc.Load(filename);

            //Utils.Helper_Trace("XORCISM PROVIDER NESSUS", string.Format("HackFile should be located at : " + filename));
            //data = doc.InnerXml;

            #endregion

            // m_target = Helper_GetTarget(data);
            m_AccountID = AccountID;
            m_data = data;
            m_JobId = jobid;
        }


        public void parse()
        {
            Assembly a;
            a = Assembly.GetExecutingAssembly();

            Utils.Helper_Trace("XORCISM PROVIDER NESSUS", "Assembly location = " + a.Location);

            // ============================================
            // Parse the XML Document and populate the database
            // ============================================

            XmlDocument doc = new XmlDocument();

            doc.LoadXml(m_data);

            XORCISMEntities model;
            model = new XORCISMEntities();

            string query = "/NessusClientData_v2/Report";

            XmlNode report;
            report = doc.SelectSingleNode(query);

            Utils.Helper_Trace("XORCISM PROVIDER NESSUS", string.Format("Found {0} hosts to parse", report.ChildNodes.Count));

            foreach (XmlNode reportHost in report.ChildNodes)
            {
                string ipAddress;
                ipAddress = reportHost.Attributes["name"].InnerText;

                Utils.Helper_Trace("XORCISM PROVIDER NESSUS", string.Format("Handling host with IP {0}", ipAddress));

                // =============================================
                // If necessary, create an asset in the database
                // =============================================
                //TODO  ipaddressIPv4
                var myass = from ass in model.ASSET
                            where ass.ipaddressIPv4 == ipAddress //&& ass.AccountID == m_AccountID
                            select ass;
                ASSET asset = myass.FirstOrDefault();

                if (asset == null)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER NESSUS", "Creates a new entry in table ASSET for this IP");

                    asset = new ASSET();
                    //asset.AccountID = m_AccountID;
                    asset.AssetName = ipAddress;
                    asset.AssetDescription = ipAddress;
                    //TODO  ipaddressIPv4
                    asset.ipaddressIPv4 = ipAddress;
                    asset.Enabled = true;
                    //asset.JobID = m_JobId;

                    model.ASSET.Add(asset);
                    model.SaveChanges();
                }
                else
                {
                    Utils.Helper_Trace("XORCISM PROVIDER NESSUS", "This IP already corresponds to an existing asset");
                }

                Utils.Helper_Trace("XORCISM PROVIDER NESSUS", "Creating ASSETINSESSION reference");
                ASSETSESSION assinsess = new ASSETSESSION();
                assinsess.AssetID = asset.AssetID;
                assinsess.SessionID = model.JOB.Single(x => x.JobID == m_JobId).SessionID;
                model.ASSETSESSION.Add(assinsess);
                model.SaveChanges();

                Utils.Helper_Trace("XORCISM PROVIDER NESSUS", "Update JOB with ASSETINSESSIONID");
                JOB daJob = model.JOB.Single(x => x.JobID == m_JobId);
                daJob.AssetSessionID = assinsess.AssetSessionID;
                model.SaveChanges();


                // =============================
                // Handles every ReportItem tag
                // =============================

                foreach (XmlNode n in reportHost.ChildNodes)
                {
                    //Hardcoded
                    if (n.Name.ToUpper() == "ReportItem".ToUpper() && n.ChildNodes != null && n.ChildNodes.Count > 0)
                    {
                        string protocol = n.Attributes["protocol"].InnerText.ToUpper();
                        int port = Convert.ToInt32(n.Attributes["port"].InnerText);
                        //svc_name
                        //pluginID
                        //pluginName
                        //pluginFamily
                        //risk_factor

                        VulnerabilityEndPoint vulnerabilityEndPoint = new VulnerabilityEndPoint();
                        vulnerabilityEndPoint.IpAdress = ipAddress;
                        vulnerabilityEndPoint.Protocol = protocol;
                        vulnerabilityEndPoint.Port = port;

                        VulnerabilityFound vulnerabilityFound = new VulnerabilityFound();
                        vulnerabilityFound.ListItem = Helper_GetCVE(n);
                        vulnerabilityFound.ListReference = Helper_GetREFERENCE(n);  //TODO: Helper_GetCVE and Helper_GetREFERENCE could be mixed for only 1 parsing
                        vulnerabilityFound.InnerXml = n.OuterXml;
                        vulnerabilityFound.Description = HelperGetChildInnerText(n, "description");
                        vulnerabilityFound.Solution = HelperGetChildInnerText(n, "solution");
                        vulnerabilityFound.Title = HelperGetChildInnerText(n, "synopsis");
                        vulnerabilityFound.rawresponse = HelperGetChildInnerText(n, "plugin_output");
                        vulnerabilityFound.Result = HelperGetChildInnerText(n, "plugin_output");
                        vulnerabilityFound.Severity = n.Attributes["severity"].InnerText;   //1
                        //vulnerabilityFound.Severity = HelperGetChildInnerText(n, "risk_factor");  //None  Low
                        if (HelperGetChildInnerText(n, "exploit_available") == "true")
                        {
                            vulnerabilityFound.Exploitable = true;
                        }
                        //exploitability_ease   Exploits are available
                        //exploit_framework_canvas
                        //exploit_framework_metasploit
                        //exploit_framework_core
                        //metasploit_name
                        //canvas_package

                        //cvss_vector
                        //cvss_temporal_score
                        try
                        {
                            vulnerabilityFound.CVSSBaseScore = float.Parse(HelperGetChildInnerText(n, "cvss_base_score"), System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch (Exception ex)
                        {
                            Utils.Helper_Trace("XORCISM PROVIDER NESSUS", string.Format("Error parsing CVSS_BASE : Exception = {0}", ex.Message));
                            Utils.Helper_Trace("XORCISM PROVIDER NESSUS", string.Format("CVSS_BASE =", HelperGetChildInnerText(n, "cvss_base_score")));
                        }

                        bool PatchUpgrade = false;
                        string MSPatch = "";
                        string title;
                        string Solution;
                        //patch_publication_date
                        if (HelperGetChildInnerText(n, "patch_publication_date") != "")
                        {
                            PatchUpgrade = true;
                        }
                        title = n.Attributes["pluginName"].InnerText;
                        Regex objNaturalPattern = new Regex("MS[0-9][0-9]-[0-9][0-9][0-9]");
                        MSPatch = objNaturalPattern.Match(title).ToString();
                        if (MSPatch != "")
                        {
                            Utils.Helper_Trace("XORCISM PROVIDER NESSUS", "MSPatch=" + MSPatch);
                            PatchUpgrade = true;
                        }

                        //Hardcoded
                        Solution = HelperGetChildInnerText(n, "solution");
                        if (Solution.Contains(" upgrade to "))
                        {
                            PatchUpgrade = true;
                        }
                        if (Solution.Contains("Upgrade "))
                        {
                            PatchUpgrade = true;
                        }
                        if (Solution.Contains("has released a set of patches"))
                        {
                            PatchUpgrade = true;
                        }
                        if (Solution.Contains("Apply the appropriate patch"))
                        {
                            PatchUpgrade = true;
                        }

                        //<patch_publication_date>

                        vulnerabilityFound.PatchUpgrade = PatchUpgrade;
                        vulnerabilityFound.MSPatch = MSPatch;

                        // ===========
                        // Persistance
                        // ===========

                        Utils.Helper_Trace("XORCISM PROVIDER NESSUS", string.Format("Persistance [{0}] [{1}] [{2}]", protocol, port, Helper_ListCVEToString(vulnerabilityFound.ListItem)));

                        int etat = VulnerabilityPersistor.Persist(vulnerabilityFound, vulnerabilityEndPoint, m_JobId, "nessus", model);
                        if (etat == -1)
                        {
                            Utils.Helper_Trace("XORCISM PROVIDER NESSUS", string.Format("CANNOT IMPORT THIS ASSET !!!! "));
                        }
                    }
                }


            }

            // TODO
            // VulnerabilityPersistor.UpdateVulnerabilityJob(list_vulnerabilyFound,m_JobId,m_model);

        }

        private string Helper_ListCVEToString(List<VulnerabilityFound.Item> list)
        {
            string s = "";

            foreach (VulnerabilityFound.Item item in list)
                s = s + item.ID + ":" + item.Value + " / ";

            return s;
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

        private List<VulnerabilityFound.Item> Helper_GetCVE(XmlNode reportItem)
        {
            List<VulnerabilityFound.Item> l = new List<VulnerabilityFound.Item>();

            foreach (XmlNode child in reportItem.ChildNodes)
            {
                if (child.Name.ToUpper() == "cve".ToUpper())
                {
                    VulnerabilityFound.Item item = new VulnerabilityFound.Item();
                    item.Value = child.InnerText;
                    item.ID = "cve";
                    l.Add(item);
                }
            }

            return l;
        }

        private List<VulnerabilityFound.Reference> Helper_GetREFERENCE(XmlNode reportItem)
        {
            List<VulnerabilityFound.Reference> l = new List<VulnerabilityFound.Reference>();

            foreach (XmlNode child in reportItem.ChildNodes)
            {
                //HARDCODED
                if (child.Name.ToUpper() == "bid".ToUpper())
                {
                    VulnerabilityFound.Reference Reference = new VulnerabilityFound.Reference();
                    Reference.Source = "BID";
                    Reference.Title = child.InnerText;
                    Reference.Url = "http://www.securityfocus.com/bid/" + child.InnerText;
                    Utils.Helper_Trace("XORCISM PROVIDER NESSUS", string.Format("TEST REFERENCEBID: {0}", "http://www.securityfocus.com/bid/" + child.InnerText));
                    l.Add(Reference);
                }
                else
                {
                    if (child.Name.ToUpper() == "see_also".ToUpper())
                    {
                        VulnerabilityFound.Reference Reference = new VulnerabilityFound.Reference();
                        Reference.Source = "NESSUS";
                        Reference.Title = child.InnerText;
                        Reference.Url = child.InnerText;
                        Utils.Helper_Trace("XORCISM PROVIDER NESSUS", string.Format("TEST REFERENCENESSUS: {0}", child.InnerText));
                        //TODO: check these unknown references/source
                        l.Add(Reference);
                    }
                    if (child.Name.ToUpper() == "xref".ToUpper())
                    {
                        if (child.InnerText.Contains("OSVDB"))
                        {
                            VulnerabilityFound.Reference Reference = new VulnerabilityFound.Reference();
                            Reference.Source = "OSVDB";
                            Reference.Title = child.InnerText.Replace("OSVDB:", "");
                            Reference.Url = "http://www.osvdb.org/" + child.InnerText.Replace("OSVDB:", "");
                            Utils.Helper_Trace("XORCISM PROVIDER NESSUS", string.Format("TEST REFERENCEOSVDB: {0}", "http://www.osvdb.org/" + child.InnerText.Replace("OSVDB:", "")));
                            l.Add(Reference);
                        }
                        else
                        {
                            if (child.InnerText.Contains("Secunia"))
                            {
                                VulnerabilityFound.Reference Reference = new VulnerabilityFound.Reference();
                                Reference.Source = "SECUNIA";
                                Reference.Title = child.InnerText.Replace("Secunia:", "");
                                Reference.Url = "http://secunia.com/advisories/" + child.InnerText.Replace("Secunia:", "");
                                Utils.Helper_Trace("XORCISM PROVIDER NESSUS", string.Format("TEST REFERENCESECUNIA: {0}", "http://secunia.com/advisories/" + child.InnerText.Replace("Secunia:", "")));
                                l.Add(Reference);
                            }
                            else
                            {
                                if (child.InnerText.Contains("EDB-ID"))
                                {
                                    VulnerabilityFound.Reference Reference = new VulnerabilityFound.Reference();
                                    Reference.Source = "EXPLOIT-DB";
                                    Reference.Title = child.InnerText.Replace("EDB-ID:", "");
                                    Reference.Url = "http://www.exploit-db.com/exploits/" + child.InnerText.Replace("EDB-ID:", "");
                                    Utils.Helper_Trace("XORCISM PROVIDER NESSUS", string.Format("TEST REFERENCEEXPLOIT-DB: {0}", "http://www.exploit-db.com/exploits/" + child.InnerText.Replace("EDB-ID:", "")));
                                    l.Add(Reference);
                                }
                                else
                                {
                                    VulnerabilityFound.Reference Reference = new VulnerabilityFound.Reference();
                                    Reference.Source = "NESSUS";
                                    Reference.Title = child.InnerText;
                                    Reference.Url = child.InnerText;
                                    Utils.Helper_Trace("XORCISM PROVIDER NESSUS", string.Format("TEST REFERENCENESSUS: {0}", child.InnerText));
                                    //TODO: check these unknown references/source
                                    l.Add(Reference);
                                }
                            }
                        }
                    }
                }
            }

            return l;
        }

    }
}

