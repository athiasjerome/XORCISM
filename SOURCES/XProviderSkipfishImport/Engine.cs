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

namespace XProviderSkipfishImport
{
    /// <summary>
    /// Copyright (C) 2012-2015 Jerome Athias
    /// XORCISM Plugin for SkipFish
    /// All trademarks and registered trademarks are the property of their respective owners.
    /// This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation; either version 2 of the License, or (at your option) any later version.
    /// 
    /// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
    /// 
    /// You should have received a copy of the GNU General Public License along with this program; if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
    /// </summary>
    public class Engine : MarshalByRefObject, XProviderCommon.IVulnerabilityImporter
    {
        public Engine()
        {
            TextWriterTraceListener tw;
            tw = new TextWriterTraceListener("XProviderSkipfishImport.log");    //Hardcoded
            Trace.Listeners.Add(tw);
            Trace.AutoFlush = true;
            Trace.IndentSize = 4;
        }

        public void Run(string data, int jobID, int AccountID)
        {
            Utils.Helper_Trace("XORCISM PROVIDER SkipfishImport", "Entering Run()");

            Utils.Helper_Trace("XORCISM PROVIDER SkipfishImport", string.Format("Creating an instance of SkipfishImportParser for AccountID="+AccountID.ToString()));

            SkipfishImportParser SkipfishImportParser = new SkipfishImportParser(data,AccountID,jobID);

            Utils.Helper_Trace("XORCISM PROVIDER SkipfishImport", string.Format("Parsing the data"));

            SkipfishImportParser.parse();

            Utils.Helper_Trace("XORCISM PROVIDER SkipfishImport", "Updating job status to FINISHED");

            XORCISMEntities model = new XORCISMEntities();
            var xJob = from j in model.JOB
                       where j.JobID == jobID
                       select j;

            JOB xJ = xJob.FirstOrDefault();
            xJ.Status = XCommon.STATUS.FINISHED.ToString();

                Utils.Helper_Trace("XORCISM PROVIDER SkipfishImport", "Changing the session to ServiceCategoryID=2");                
                var xSession = from s in model.SESSION
                                where s.SessionID == xJ.SessionID
                                select s;
                SESSION xS = xSession.FirstOrDefault();
                xS.ServiceCategoryID = 2;   //HARDCODED
                

            model.SaveChanges();
            Utils.Helper_Trace("XORCISM PROVIDER SkipfishImport", string.Format("End of data processing"));

            Utils.Helper_Trace("XORCISM PROVIDER SkipfishImport", "Leaving Run()");
        }
    }

    class SkipfishImportParser
    {
        private string m_data;
        private int m_AccountID;
        private int m_JobId;

        public SkipfishImportParser(string data, int AccountID, int jobid)
        {
            #region HackSkipFish

            //string filename;
            //filename = @"C:\SkipFishResults.xml";

            //XmlDocument doc = new XmlDocument();
            //doc.Load(filename);

            //Utils.Helper_Trace("XORCISM PROVIDER SkipfishImport", string.Format("HackFile should be located at : " + filename));
            //data = doc.InnerXml;

            #endregion

            // m_target = Helper_GetTarget(data);
            m_AccountID = AccountID;
            m_data = data;
            m_JobId=jobid;
        }

        public void parse()
        {
            Assembly a;
            a = Assembly.GetExecutingAssembly();

            Utils.Helper_Trace("XORCISM PROVIDER SkipfishImport", "Assembly location = " + a.Location);

            // ============================================
            // Parse the Document and populate the database
            // ============================================
            
            XORCISMEntities model;
            model = new XORCISMEntities();
        
            

            string ipAddress;
            ipAddress = "";
            string protocol="WWW";  //Hardcoded
            int port=80;

            Utils.Helper_Trace("XORCISM PROVIDER SkipfishImport", string.Format("Handling host with IP {0}", ipAddress));
                
            // =============================================
            // If necessary, create an asset in the database
            // =============================================
            //TODO
            var myass = from ass in model.ASSET
                        where ass.ipaddressIPv4 == ipAddress //&& ass.AccountID == m_AccountID
                        select ass;
            ASSET asset = myass.FirstOrDefault();

            if (asset == null)
            {
                Utils.Helper_Trace("XORCISM PROVIDER SkipfishImport", "Creates a new entry in table ASSET for this IP");

                asset = new ASSET();
                //asset.AccountID = m_AccountID;
                asset.AssetName = ipAddress;
                asset.AssetDescription = ipAddress;
                asset.ipaddressIPv4 = ipAddress;
                    asset.Enabled = true;
                //asset.JobID = m_JobId;

                model.ASSET.Add(asset);
                model.SaveChanges();
            }
            else
            {
                Utils.Helper_Trace("XORCISM PROVIDER SkipfishImport", "This IP already corresponds to an existing asset");                    
            }

            Utils.Helper_Trace("XORCISM PROVIDER SkipfishImport", "Creating ASSETINSESSION reference");
            ASSETSESSION assinsess = new ASSETSESSION();
            assinsess.AssetID = asset.AssetID;
            assinsess.SessionID = model.JOB.Single(x => x.JobID == m_JobId).SessionID;
            model.ASSETSESSION.Add(assinsess);
            model.SaveChanges();

            Utils.Helper_Trace("XORCISM PROVIDER SkipfishImport", "Update JOB with ASSETINSESSIONID");
            JOB daJob = model.JOB.Single(x => x.JobID == m_JobId);
            daJob.AssetSessionID = assinsess.AssetSessionID;
            model.SaveChanges();
            

            //**************************
            StreamReader monStreamReader = new StreamReader("samples.js");  //Hardcoded
            string curline = monStreamReader.ReadLine();
            bool issue_samples = false;
            int currentseverity = 0;
            string curvulntype = "";

            while (curline != null)
            {
                if (issue_samples)
                {
                    if (curline.Contains("'url':"))
                    {
                        Console.WriteLine(curvulntype);
                        Console.WriteLine(vulntypeSkipfish(curvulntype));
                        curline = curline.Trim();
                        char[] splitter1 = { ',' };
                        string[] words1 = curline.Split(splitter1);
                        string vulnurl = words1[0].Replace("{ 'url': '", "");
                        vulnurl = vulnurl.Substring(0, vulnurl.Length - 1);
                        Console.WriteLine(vulnurl);
                        string vulnparam = words1[1].Replace("'extra': '", "");
                        vulnparam = vulnparam.Substring(0, vulnparam.Length - 1).Trim();
                        Console.WriteLine(vulnparam);
                        string vulninfodir = words1[2].Replace("'dir': '", "");
                        vulninfodir = vulninfodir.Replace("' } ]", "");
                        vulninfodir = vulninfodir.Replace("' }", "").Trim();
                        Console.WriteLine(vulninfodir);

                        if (currentseverity > 0)
                        {
                            VulnerabilityEndPoint vulnerabilityEndPoint = new VulnerabilityEndPoint();
                            vulnerabilityEndPoint.IpAdress = ipAddress;
                            vulnerabilityEndPoint.Protocol = protocol;
                            vulnerabilityEndPoint.Port = port;

                            VulnerabilityFound vulnerabilityFound = new VulnerabilityFound();
                            //vulnerabilityFound.PatchUpgrade = PatchUpgrade;
                            //vulnerabilityFound.MSPatch = MSPatch;
                            vulnerabilityFound.Title = vulntypeSkipfish(curvulntype);
                            vulnerabilityFound.Severity = currentseverity.ToString();
                            vulnerabilityFound.Url = vulnurl;
                            //vulnerabilityFound.rawrequest=    vulninfodir+"/request.dat";
                            //vulnerabilityFound.rawresponse=   vulninfodir+"/response.dat";
                            vulnerabilityFound.Result = vulnparam;
                            

                            // ===========
                            // Persistance
                            // ===========

                            Utils.Helper_Trace("XORCISM PROVIDER SkipfishImport", string.Format("Persistance [{0}] [{1}] [{2}]", protocol, port, Helper_ListCVEToString(vulnerabilityFound.ListItem)));

                            int etat = VulnerabilityPersistor.Persist(vulnerabilityFound, vulnerabilityEndPoint, m_JobId, "skipfish", model);
                            if (etat == -1)
                            {
                                Utils.Helper_Trace("XORCISM PROVIDER SkipfishImport", string.Format("CANNOT IMPORT THIS ASSET !!!! "));
                            }
                        }
                        else
                        {
                            //TODO
                            /*
                            //severity=0
                            INFORMATION myinfo = new INFORMATION();
                            myinfo.Title = vulntypeSkipfish(curvulntype);
                            myinfo.Severity = currentseverity.ToString();
                            myinfo.Url = vulnurl;
                            //myinfo.rawrequest=    vulninfodir+"/request.dat";
                            //myinfo.rawresponse=   vulninfodir+"/response.dat";
                            myinfo.Result = vulnparam;
                            myinfo.JobID = m_JobId;

                            model.AddToINFORMATION(myinfo);
                            model.SaveChanges();    
                            */
                        }
                    }
                }

                if (curline.Contains("'severity': 4"))
                {
                    currentseverity = 4;
                    char[] splitter1 = { ',' };
                    string[] words1 = curline.Split(splitter1);
                    curvulntype = words1[1].Replace(" 'type': ", "");
                }
                if (curline.Contains("'severity': 3"))
                {
                    currentseverity = 3;
                    char[] splitter1 = { ',' };
                    string[] words1 = curline.Split(splitter1);
                    curvulntype = words1[1].Replace(" 'type': ", "");
                }
                if (curline.Contains("'severity': 2"))
                {
                    currentseverity = 2;
                    char[] splitter1 = { ',' };
                    string[] words1 = curline.Split(splitter1);
                    curvulntype = words1[1].Replace(" 'type': ", "");
                }
                if (curline.Contains("'severity': 1"))
                {
                    currentseverity = 1;
                    char[] splitter1 = { ',' };
                    string[] words1 = curline.Split(splitter1);
                    curvulntype = words1[1].Replace(" 'type': ", "");
                }
                if (curline.Contains("'severity': 0"))
                {
                    currentseverity = 0;
                    char[] splitter1 = { ',' };
                    string[] words1 = curline.Split(splitter1);
                    curvulntype = words1[1].Replace(" 'type': ", "");
                }
                //Where am I?
                if (curline.Contains("var issue_samples"))
                {
                    issue_samples = true;
                    /*
                    ligne = ligne.Trim();
                    char[] splitter1 = { ' ' };
                    string[] words1 = ligne.Split(splitter1);

                    cmd1 = "./msfcli " + words1[0].Trim() + " T";
                    */
                }
                curline = monStreamReader.ReadLine();
            }

            monStreamReader.Close();



                    
                


            

            // A VOIR
            // VulnerabilityPersistor.UpdateVulnerabilityJob(list_vulnerabilyFound,m_JobId,m_model);

        }



        private string vulntypeSkipfish(string curvulntype)
        {
            //From index.html   var issue_desc= {
            string vulntype = "";

            //HARDCODED
            switch (curvulntype)
            {
                case "10101": vulntype = "SSL certificate issuer information";
                    break;
                case "10201": vulntype = "New HTTP cookie added";
                    break;
                case "10202": vulntype = "New 'Server' header value seen";
                    break;
                case "10203": vulntype = "New 'Via' header value seen";
                    break;
                case "10204": vulntype = "New 'X-*' header value seen";
                    break;
                case "10205": vulntype = "New 404 signature seen";
                    break;
                case "10401": vulntype = "Resource not directly accessible";
                    break;
                case "10402": vulntype = "HTTP authentication required";
                    break;
                case "10403": vulntype = "Server error triggered";
                    break;
                case "10501": vulntype = "All external links";
                    break;
                case "10502": vulntype = "External URL redirector";
                    break;
                case "10503": vulntype = "All e-mail addresses";
                    break;
                case "10504": vulntype = "Links to unknown protocols";
                    break;
                case "10505": vulntype = "Unknown form field (can't autocomplete)";
                    break;
                case "10601": vulntype = "HTML form (not classified otherwise)";
                    break;
                case "10602": vulntype = "Password entry form - consider brute-force";
                    break;
                case "10603": vulntype = "File upload form";
                    break;
                case "10701": vulntype = "User-supplied link rendered on a page";
                    break;
                case "10801": vulntype = "Incorrect or missing MIME type (low risk)";
                    break;
                case "10802": vulntype = "Generic MIME used (low risk)";
                    break;
                case "10803": vulntype = "Incorrect or missing charset (low risk)";
                    break;
                case "10804": vulntype = "Conflicting MIME / charset info (low risk)";
                    break;
                case "10901": vulntype = "Numerical filename - consider enumerating";
                    break;
                case "10902": vulntype = "OGNL-like parameter behavior";
                    break;
                case "20101": vulntype = "Resource fetch failed";
                    break;
                case "20102": vulntype = "Limits exceeded fetch suppressed";
                    break;
                case "20201": vulntype = "Directory behavior checks failed (no brute force)";
                    break;
                case "20202": vulntype = "Parent behavior checks failed (no brute force)";
                    break;
                case "20203": vulntype = "IPS filtering enabled";
                    break;
                case "20204": vulntype = "IPS filtering disabled again";
                    break;
                case "20205": vulntype = "Response varies randomly skipping checks";
                    break;
                case "20301": vulntype = "Node should be a directory detection error?";
                    break;
                case "30101": vulntype = "HTTP credentials seen in URLs";
                    break;
                case "30201": vulntype = "SSL certificate expired or not yet valid";
                    break;
                case "30202": vulntype = "Self-signed SSL certificate";
                    break;
                case "30203": vulntype = "SSL certificate host name mismatch";
                    break;
                case "30204": vulntype = "No SSL certificate data found";
                    break;
                case "30301": vulntype = "Directory listing restrictions bypassed";
                    break;
                case "30401": vulntype = "Redirection to attacker-supplied URLs";
                    break;
                case "30402": vulntype = "Attacker-supplied URLs in embedded content (lower risk)";
                    break;
                case "30501": vulntype = "External content embedded on a page (lower risk)";
                    break;
                case "30502": vulntype = "Mixed content embedded on a page (lower risk)";
                    break;
                case "30601": vulntype = "HTML form with no apparent XSRF protection";
                    break;
                case "30602": vulntype = "JSON response with no apparent XSSI protection";
                    break;
                case "30701": vulntype = "Incorrect caching directives (lower risk)";
                    break;
                case "30801": vulntype = "User-controlled response prefix (BOM / plugin attacks)";
                    break;
                case "40101": vulntype = "XSS vector in document body";
                    break;
                case "40102": vulntype = "XSS vector via arbitrary URLs";
                    break;
                case "40103": vulntype = "HTTP response header splitting";
                    break;
                case "40104": vulntype = "Attacker-supplied URLs in embedded content (higher risk)";
                    break;
                case "40201": vulntype = "External content embedded on a page (higher risk)";
                    break;
                case "40202": vulntype = "Mixed content embedded on a page (higher risk)";
                    break;
                case "40301": vulntype = "Incorrect or missing MIME type (higher risk)";
                    break;
                case "40302": vulntype = "Generic MIME type (higher risk)";
                    break;
                case "40304": vulntype = "Incorrect or missing charset (higher risk)";
                    break;
                case "40305": vulntype = "Conflicting MIME / charset info (higher risk)";
                    break;
                case "40401": vulntype = "Interesting file";
                    break;
                case "40402": vulntype = "Interesting server message";
                    break;
                case "40501": vulntype = "Directory traversal / file inclusion possible";
                    break;
                case "40601": vulntype = "Incorrect caching directives (higher risk)";
                    break;
                case "40701": vulntype = "Password form submits from or to non-HTTPS page";
                    break;
                case "50101": vulntype = "Server-side XML injection vector";
                    break;
                case "50102": vulntype = "Shell injection vector";
                    break;
                case "50103": vulntype = "Query injection vector";
                    break;
                case "50104": vulntype = "Format string vector";
                    break;
                case "50105": vulntype = "Integer overflow vector";
                    break;
                case "50201": vulntype = "SQL query or similar syntax in parameters";
                    break;
                case "50301": vulntype = "PUT request accepted";
                    break;

            }

            return vulntype;
        }


        private string Helper_ListCVEToString(List<VulnerabilityFound.Item> list)
        {
            string s = "";

            foreach (VulnerabilityFound.Item item in list)
                s = s + item.ID + ":" + item.Value + " / ";

            return s;
        }

        /*
        private void Helper_PersistAsset(_Asset asset)
        {
            ASSET a = new ASSET();
            a.AccountID = asset.AccountID;
            a.IpAdress = asset.IpAdress;

            var assetInDB = from Assets in m_model.ASSET
                            where Assets.IpAdress == asset.IpAdress
                        select Assets;
            if (assetInDB == null||assetInDB.ToList().Count==0)
            {
                Utils.Helper_Trace("XORCISM PROVIDER SkipfishImport", "the asset doesn't exit .So We create a new one");
                m_model.AddToASSET(a);
                m_model.SaveChanges();
            }
            else
            {
                Utils.Helper_Trace("XORCISM PROVIDER SkipfishImport", "the asset already exists .");
            }
        }
        */

        /*
        private bool HelperHasChild(XmlNode n, string ChildName)
        {
            foreach (XmlNode child in n.ChildNodes)
            {

                if (child.Name.ToUpper() == ChildName.ToUpper())
                    return true;

            }
            return false;
        }
        */

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

            //HARDCODED
            //TODO: Replace with a global (i.e. in XUtils) Function
            foreach (XmlNode child in reportItem.ChildNodes)
            {
                if (child.Name.ToUpper() == "bid".ToUpper())
                {
                    VulnerabilityFound.Reference Reference = new VulnerabilityFound.Reference();
                    Reference.Source = "BID";
                    Reference.Title = child.InnerText;
                    Reference.Url = "http://www.securityfocus.com/bid/" + child.InnerText;
                    Utils.Helper_Trace("XORCISM PROVIDER SkipfishImport", string.Format("TEST REFERENCEBID: {0}", "http://www.securityfocus.com/bid/" + child.InnerText));
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
                        Utils.Helper_Trace("XORCISM PROVIDER SkipfishImport", string.Format("TEST REFERENCENESSUS: {0}", child.InnerText));
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
                            Utils.Helper_Trace("XORCISM PROVIDER SkipfishImport", string.Format("TEST REFERENCEOSVDB: {0}", "http://www.osvdb.org/" + child.InnerText.Replace("OSVDB:", "")));
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
                                Utils.Helper_Trace("XORCISM PROVIDER SkipfishImport", string.Format("TEST REFERENCESECUNIA: {0}", "http://secunia.com/advisories/" + child.InnerText.Replace("Secunia:", "")));
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
                                    Utils.Helper_Trace("XORCISM PROVIDER SkipfishImport", string.Format("TEST REFERENCEEXPLOIT-DB: {0}", "http://www.exploit-db.com/exploits/" + child.InnerText.Replace("EDB-ID:", "")));
                                    l.Add(Reference);
                                }
                                else
                                {
                                    VulnerabilityFound.Reference Reference = new VulnerabilityFound.Reference();
                                    Reference.Source = "NESSUS";
                                    Reference.Title = child.InnerText;
                                    Reference.Url = child.InnerText;
                                    Utils.Helper_Trace("XORCISM PROVIDER SkipfishImport", string.Format("TEST REFERENCENESSUS: {0}", child.InnerText));
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

        /*
        private List<VULNERABILITYFOUND> Helper_PersistVulnerability(VulnerabilityFound detail, VulnerabilityEndPoint SkipfishImportEndPoint)
        {

            int theEndPointID = 0;

            Utils.Helper_Trace("XORCISM PROVIDER SkipfishImport", string.Format("Searching for an asset with IP address {0}", SkipfishImportEndPoint.IpAdress));

            var asset = from Assets in m_model.ASSET
                        where Assets.IpAdress == SkipfishImportEndPoint.IpAdress
                        select Assets;
            ASSET myAsset = asset.ToList().FirstOrDefault();

            Utils.Helper_Trace("XORCISM PROVIDER SkipfishImport", string.Format("Retreiving for the endpoints"));

            ENDPOINT endPoint = new ENDPOINT();
            var EP = from Epoint in m_model.ENDPOINT
                     where Epoint.AssetID == myAsset.AssetID
                     select Epoint;

            Utils.Helper_Trace("XORCISM PROVIDER SkipfishImport", string.Format("Found {0} enpoints", EP.ToList().Count));

            if (EP.ToList().Count == 0)
            {
                Utils.Helper_Trace("XORCISM PROVIDER SkipfishImport", string.Format("No endpoint found, creating a new one (port={0}, proto={1})", SkipfishImportEndPoint.Port, SkipfishImportEndPoint.Protocol));

                ENDPOINT tmpEP = new ENDPOINT();
                tmpEP.Port      = (int?)NessusEndPoint.Port;
                tmpEP.Protocol  = SkipfishImportEndPoint.Protocol;
                tmpEP.AssetID   = myAsset.AssetID;

                m_model.AddToENDPOINT(tmpEP);
                m_model.SaveChanges();

                theEndPointID = tmpEP.EndPointID;
            }
            else
            {
                Utils.Helper_Trace("XORCISM PROVIDER SkipfishImport", string.Format("Looking for the right endpoint"));

                foreach (ENDPOINT E in EP.ToList())
                {
                    if (E.Protocol == SkipfishImportEndPoint.Protocol && E.Port == SkipfishImportEndPoint.Port)
                    {
                        theEndPointID = E.EndPointID;
                        break;
                    }
                }


                if (theEndPointID == 0)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER SkipfishImport", string.Format("Coulnd not find the endpoint"));

                    Utils.Helper_Trace("XORCISM PROVIDER SkipfishImport", string.Format("Creating a new one (port={0}, proto={1})", SkipfishImportEndPoint.Port, SkipfishImportEndPoint.Protocol));

                    ENDPOINT tmpEP = new ENDPOINT();
                    tmpEP.Port      = (int?)NessusEndPoint.Port;
                    tmpEP.Protocol  = SkipfishImportEndPoint.Protocol;
                    tmpEP.AssetID   = myAsset.AssetID;

                    m_model.AddToENDPOINT(tmpEP);
                    m_model.SaveChanges();

                    theEndPointID = tmpEP.EndPointID;
                }
            }

            // ============

            List<VULNERABILITYFOUND> list;
            list = new List<VULNERABILITYFOUND>();

            // =========================
            // In case there is no CVEID
            // =========================

            if (detail.ListItem.Count == 0)
            {
                Utils.Helper_Trace("XORCISM PROVIDER SkipfishImport", string.Format("Searching for an SkipfishImport entry table VULNERABILITYSYNONYM"));

                var syn1 = from S in m_model.VULNERABILITYSYNONYM
                           where S.Referential.Equals("nessus") &&
                           S.Value == detail.InnerXml
                           select S;

                VULNERABILITYSYNONYM VS;

                if (syn1.Count() == 0)
                {

                    Utils.Helper_Trace("XORCISM PROVIDER SkipfishImport", string.Format("No entry found, creatring a new SkipfishImport entry in VULNERABILITYSYNONYM"));

                    VS = new VULNERABILITYSYNONYM();
                    VS.Referential = "nessus";
                    VS.Value = detail.InnerXml;
                    VS.Description = detail.Description;
                    VS.Consequence = detail.Consequence;
                    VS.Solution = detail.Solution;
                    // VS.Severity     = detail.Severity;

                    m_model.AddToVULNERABILITYSYNONYM(VS);

                    Utils.Helper_Trace("XORCISM PROVIDER SkipfishImport", string.Format("Coucou 3"));

                    m_model.SaveChanges();

                    VS.Parent = VS.ID;

                    m_model.SaveChanges();

                    Utils.Helper_Trace("XORCISM PROVIDER SkipfishImport", string.Format("Coucou 4"));
                }
                else
                {
                    VS = syn1.First();
                }

                // =====================

                VULNERABILITYFOUND MyVuln = new VULNERABILITYFOUND();
                MyVuln.VulnerabilityID  = VS.ID;
                MyVuln.EndPointID       = theEndPointID;
                
                m_model.AddToVULNERABILITYFOUND(MyVuln);
                m_model.SaveChanges();

                Utils.Helper_Trace("XORCISM PROVIDER SkipfishImport", string.Format("Coucou 5"));

                list.Add(MyVuln);
            }

            // ====================================
            // In case there are at least one CVEID
            // ====================================

            foreach (VulnerabilityFound.Item item in detail.ListItem)
            {
                Utils.Helper_Trace("XORCISM PROVIDER SkipfishImport", string.Format("Handling CVE {0}", item.Value));

                // ========================================
                // Search VULNERABILITYSYNONYM for this CVE
                // ========================================

                var syn = from S in m_model.VULNERABILITYSYNONYM
                          where S.Referential.Equals("cve") &&
                          S.Value.Equals(item.Value)
                          select S;

                 
                if (syn.Count() == 0)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER SkipfishImport", string.Format("Error : CVEID {0} not found in VULNERABILITYSYNONYM", item.Value));
                    continue;
                }

                Utils.Helper_Trace("XORCISM PROVIDER SkipfishImport", string.Format("This CVE corresponds to ID {0} in table VULNERABILITYSYNONYM", syn.ToList().First().ID));

                Utils.Helper_Trace("XORCISM PROVIDER SkipfishImport", string.Format("Creating a new entry in table VULNERABILITYFOUND"));

                VULNERABILITYFOUND MyVuln = new VULNERABILITYFOUND();
                MyVuln.VulnerabilityID  = syn.ToList().First().ID;
                MyVuln.EndPointID       = theEndPointID;

                m_model.AddToVULNERABILITYFOUND(MyVuln);
                m_model.SaveChanges();

                list.Add(MyVuln);

                // ==========================================================================
                // If necessary, creates an entry in VULNERABILITYSYNONYM for SkipfishImport
                // ==========================================================================

                Utils.Helper_Trace("XORCISM PROVIDER SkipfishImport", string.Format("Searching for an SkipfishImport entry table VULNERABILITYSYNONYM"));

                var syn1 = from S in m_model.VULNERABILITYSYNONYM
                           where S.Referential.Equals("skipfish") &&
                           S.Value == detail.InnerXml
                           select S;
                if (syn1.Count() != 0)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER SkipfishImport", string.Format("Entry found !!!!!!!!!!!!!!!!!"));

                    //if(syn1.ToList().First().Parent != MyVuln.VulnerabilityID)
                    //{
                    //    Utils.Helper_Trace("XORCISM PROVIDER SkipfishImport", string.Format("GROS BUG"));
                    //    continue;
                    //}                    
                }
                else
                {
                    
                    Utils.Helper_Trace("XORCISM PROVIDER SkipfishImport", string.Format("No entry found, creatring a new SkipfishImport entry in VULNERABILITYSYNONYM"));

                    VULNERABILITYSYNONYM VS = new VULNERABILITYSYNONYM();
                    VS.Referential  = "nessus";
                    VS.Value        = detail.InnerXml;
                    VS.Description  = detail.Description;
                    VS.Consequence  = detail.Consequence;
                    VS.Solution     = detail.Solution;
                    VS.Parent       = MyVuln.VulnerabilityID;

                    MyVuln.Severity = detail.Severity;

                    m_model.AddToVULNERABILITYSYNONYM(VS);

                    Utils.Helper_Trace("XORCISM PROVIDER SkipfishImport", string.Format("Coucou 1"));

                    m_model.SaveChanges();

                    Utils.Helper_Trace("XORCISM PROVIDER SkipfishImport", string.Format("Coucou 2"));
                }

               
            }

            return list;

         }
         */
    }

    /*
    class _Asset
    {
        public string IpAdress
        {
            get{return m_IpAdress;}
            set{m_IpAdress=value;}
        }
        public int AccountID
        {
            get{return m_AccountID;}
            set{m_AccountID=value;}
        }
        private  string m_IpAdress;
        private  int m_AccountID;
        public _Asset()
        {
            m_AccountID=0;
            m_IpAdress=string.Empty;
        }
    }
    */
}

