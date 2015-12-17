using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XORCISMModel;
using XVULNERABILITYModel;

using System.Net;
using System.Text.RegularExpressions;
using System.IO;

namespace Import_scipvuldb
{
    class Program
    {
        /// <summary>
        /// Copyright (C) 2014-2015 Jerome Athias
        /// Parser for SCIP.CH VULDB and import into an XORCISM database
        /// This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation; either version 2 of the License, or (at your option) any later version.
        /// 
        /// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
        /// 
        /// You should have received a copy of the GNU General Public License along with this program; if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
        /// </summary>
        
        public static XORCISMEntities model = new XORCISMEntities();
        public static XVULNERABILITYEntities vuln_nodel = new XVULNERABILITYEntities();
        public static int iVocabularySCIPID = 0;
        public static string sSource=string.Empty;
        public static string sSourceID =string.Empty;
        public static string sReferenceURL =string.Empty;

        static void Main(string[] args)
        {
            //https://stackoverflow.com/questions/5940225/fastest-way-of-inserting-in-entity-framework
            model.Configuration.AutoDetectChangesEnabled = false;
            model.Configuration.ValidateOnSaveEnabled = false;

            int iCptYear = DateTime.Now.Year;
            //XORCISMEntities model = new XORCISMEntities();

            //int iVocabularySCIPID = 0;// 1044;  //SCIP
            #region vocabularyscip
            try
            {
                iVocabularySCIPID = model.VOCABULARY.Where(o => o.VocabularyName == "SCIP").Select(o=>o.VocabularyID).FirstOrDefault();
            }
            catch (Exception ex)
            {

            }
            if (iVocabularySCIPID <= 0)
            {
                VOCABULARY oVocabulary = new VOCABULARY();
                oVocabulary.CreatedDate = DateTimeOffset.Now;
                oVocabulary.VocabularyName = "SCIP";
                model.VOCABULARY.Add(oVocabulary);
                model.SaveChanges();
                iVocabularySCIPID = oVocabulary.VocabularyID;
                Console.WriteLine("DEBUG iVocabularySCIPID=" + iVocabularySCIPID);
            }
            #endregion vocabularyscip


            while (iCptYear > 2003)
            {
                string sURI = "refmap" + iCptYear;
                Console.WriteLine("DEBUG *************************************************************");
                Console.WriteLine("DEBUG " + DateTimeOffset.Now.ToString());
                Console.WriteLine("DEBUG Working on " + sURI);

                string sDownloadFileURL = "http://www.scip.ch/en/?vuldb." + sURI;
                iCptYear--;
                

                HttpWebRequest webRequest = null;
                HttpWebResponse webResponse = null;
                webRequest = (HttpWebRequest)WebRequest.Create(new Uri(sDownloadFileURL));
                webRequest.Method = "GET";
                //webRequest.Credentials = CredentialCache.DefaultCredentials;
                //webRequest.Timeout = 20 * 60 * 1000;    //20 minutes
                webResponse = (HttpWebResponse)webRequest.GetResponse();
                StreamReader SR = new StreamReader(webResponse.GetResponseStream());
                string sResponseText = SR.ReadToEnd();
                //Console.WriteLine(sResponseText);

                SR.Close();
                webResponse.Close();

                StreamWriter swStreamWriter = new StreamWriter(sURI + ".txt");
                swStreamWriter.Write(sResponseText);
                swStreamWriter.Close();


                

                StreamReader srStreamReader = new StreamReader(sURI + ".txt");
                string sLine = srStreamReader.ReadLine();
                string sTemp = string.Empty;
                string sCurrentVULDB = string.Empty;
                string sCurrentCVE = string.Empty;
                int iVulnerabilityID = 0;

                Regex myRegexVULDB = new Regex(@"<a href=\""\?vuldb\.[0-9](.*?)\"""); //TODO Review
                //Regex myRegexCVE = new Regex("CVE-[0-9][0-9][0-9][0-9]-[0-9][0-9][0-9][0-9]");
                Regex myRegexCVE = new Regex(@"CVE-(19|20)\d\d-(0\d{3}|[1-9]\d{3,})");
                //https://cve.mitre.org/cve/identifiers/tech-guidance.html
                Regex myRegexSECTRACK = new Regex(@"securitytracker.com/id/(.*?)\"" "); //TODO Review
                Regex myRegexSECUNIA = new Regex(@"secunia.com/advisories/(.*?)\"" "); //TODO Review
                Regex myRegexBID = new Regex(@"securityfocus.com/bid/(.*?)\"" "); //TODO Review
                Regex myRegexXFORCE = new Regex(@"xforce.iss.net/xforce/xfdb/(.*?)\"" "); //TODO Review
                Regex myRegexOSVDB = new Regex(@"osvdb.org/[0-9](.*?)\"" "); //TODO Review

                while (sLine != null)
                {
                    sLine = sLine.Replace("securitytracker.com/id?", "securitytracker.com/id/");
                    //sLine = sLine.Replace("https://www.", "http://");
                    //sLine = sLine.Replace("http://www.", "http://");
                    sLine = sLine.Replace("osvdb.org/displayvuln.php?osvdbid=", "osvdb.org/");
                    sLine = sLine.Replace("osvdb.org/show/osvdb/", "osvdb.org/");
                    //TODO? microsoft.com MS

                    sTemp = myRegexVULDB.Match(sLine).ToString();
                    if (sTemp != "")
                    {
                        sTemp = sTemp.Replace("<a href=", "");
                        sTemp = sTemp.Replace("\"", "");
                        sTemp = sTemp.Replace("?vuldb.", "");
                        //TODO check if ok
                        sCurrentVULDB = sTemp;
                        Console.WriteLine("*************************************************************");
                        Console.WriteLine("DEBUG " + DateTimeOffset.Now.ToString());
                        Console.WriteLine("DEBUG SCIP VULDB:" + sCurrentVULDB);
                    }
                    else
                    {
                        sTemp = myRegexCVE.Match(sLine).ToString();
                        if (sTemp != "")
                        {
                            #region cve
                            sCurrentCVE = sTemp;
                            Console.WriteLine("DEBUG " + DateTimeOffset.Now.ToString());
                            Console.WriteLine("DEBUG CVE:" + sCurrentCVE);
                            //TODO double-check if it is real CVE-ID

                            try
                            {
                                iVulnerabilityID = vuln_nodel.VULNERABILITY.Where(o => o.VULReferential == "cve" && o.VULReferentialID == sCurrentCVE).Select(o => o.VulnerabilityID).FirstOrDefault();
                            }
                            catch (Exception exCVE)
                            {
                                //Console.WriteLine("Exception exCVE " + exCVE.Message + " " + exCVE.InnerException);

                            }
                            if (iVulnerabilityID <= 0)
                            {
                                try
                                {
                                    VULNERABILITY oVulnerability = new VULNERABILITY();
                                    oVulnerability.CreatedDate = DateTimeOffset.Now;
                                    oVulnerability.VocabularyID = iVocabularySCIPID;
                                    oVulnerability.VULReferential = "cve";
                                    oVulnerability.VULReferentialID = sCurrentCVE;
                                    oVulnerability.timestamp = DateTimeOffset.Now;
                                    vuln_nodel.VULNERABILITY.Add(oVulnerability);
                                    vuln_nodel.SaveChanges();

                                    iVulnerabilityID = oVulnerability.VulnerabilityID;
                                }
                                catch (System.Data.Entity.Validation.DbEntityValidationException e)
                                {
                                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                                    foreach (var eve in e.EntityValidationErrors)
                                    {
                                        sb.AppendLine(string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                                                        eve.Entry.Entity.GetType().Name,
                                                                        eve.Entry.State));
                                        foreach (var ve in eve.ValidationErrors)
                                        {
                                            sb.AppendLine(string.Format("- Property: \"{0}\", Error: \"{1}\"",
                                                                        ve.PropertyName,
                                                                        ve.ErrorMessage));
                                        }
                                    }
                                    //throw new DbEntityValidationException(sb.ToString(), e);
                                    Console.WriteLine("Exception DbEntityValidationExceptionUPDATECAPEC " + sb.ToString());
                                }
                                catch (Exception exSCIPCVE)
                                {
                                    Console.WriteLine("Exception exSCIPCVE " + exSCIPCVE.Message + " " + exSCIPCVE.InnerException);
                                }
                            }
                            else
                            {
                                //Update VULNERABILITY
                            }
                            Console.WriteLine("DEBUG " + DateTimeOffset.Now.ToString());
                            Console.WriteLine("DEBUG iVulnerabilityID=" + iVulnerabilityID);

                            sSource = "SCIP";
                            sSourceID = sCurrentVULDB;
                            sReferenceURL = "http://scip.ch/?vuldb." + sCurrentVULDB;
                            fAddReference(iVulnerabilityID);    //, sSource, sSourceID, sReferenceURL);
                            #endregion cve
                        }
                        else
                        {
                            //<td><a href="http://osvdb.org/3314" title="osvdb.org/3314">3314</a></td>
                            sTemp = myRegexOSVDB.Match(sLine).ToString();
                            if (sTemp != "")
                            {
                                #region osvdb
                                //Console.WriteLine(sTemp);
                                sSource = "OSVDB";
                                sSourceID = sTemp.Replace("osvdb.org/", "");
                                sSourceID = sSourceID.Replace("/", "");
                                sSourceID = sSourceID.Replace("\"", "").Trim();
                                //Console.WriteLine(sSourceID);
                                try
                                {
                                    int iTest = int.Parse(sSourceID);
                                    sReferenceURL = "http://osvdb.org/" + sSourceID;
                                    Console.WriteLine("DEBUG "+sReferenceURL);

                                    fAddReference(iVulnerabilityID);    //, sSource, sSourceID, sReferenceURL);
                                }
                                catch (Exception exSCIPOSVDBID)
                                {
                                    Console.WriteLine("Exception exSCIPOSVDBID " + sSourceID + " " + exSCIPOSVDBID.Message + " " + exSCIPOSVDBID.InnerException);
                                }

                                //TODO see Import_all
                                //fRequestOSVDB();
                                #endregion osvdb
                            }
                            else
                            {
                                #region securitytracker
                                ////http://securitytracker.com/id?1028074
                                //http://securitytracker.com/id/1029599
                                sTemp = myRegexSECTRACK.Match(sLine).ToString();
                                if (sTemp != "")
                                {
                                    //Console.WriteLine(sTemp);
                                    sSource = "SECTRACK";
                                    sSourceID = sTemp.Replace("securitytracker.com/id/", "");
                                    sSourceID = sSourceID.Replace("/", "");
                                    sSourceID = sSourceID.Replace("\"", "").Trim();
                                    //Console.WriteLine(sSourceID);
                                    sReferenceURL = "http://securitytracker.com/id/" + sSourceID;
                                    Console.WriteLine("DEBUG "+sReferenceURL);

                                    fAddReference(iVulnerabilityID);    //, sSource, sSourceID, sReferenceURL);

                                }
                                #endregion securitytracker
                                else
                                {
                                    #region secunia
                                    //http://secunia.com/advisories/58347
                                    sTemp = myRegexSECUNIA.Match(sLine).ToString();
                                    if (sTemp != "")
                                    {
                                        //Console.WriteLine(sTemp);
                                        sSource = "SECUNIA";
                                        sSourceID = sTemp.Replace("secunia.com/advisories/", "");
                                        sSourceID = sSourceID.Replace("/", "");
                                        sSourceID = sSourceID.Replace("\"", "").Trim();
                                        //Console.WriteLine(sSourceID);
                                        sReferenceURL = "http://secunia.com/advisories/" + sSourceID;
                                        Console.WriteLine("DEBUG "+sReferenceURL);

                                        fAddReference(iVulnerabilityID);    //, sSource, sSourceID, sReferenceURL);

                                    }
                                    #endregion secunia
                                    else
                                    {
                                        #region securityfocus
                                        //http://securityfocus.com/bid/123
                                        sTemp = myRegexBID.Match(sLine).ToString();
                                        if (sTemp != "")
                                        {
                                            //Console.WriteLine(sTemp);
                                            sSource = "BID";
                                            sSourceID = sTemp.Replace("securityfocus.com/bid/", "");
                                            sSourceID = sSourceID.Replace("/", "");
                                            sSourceID = sSourceID.Replace("\"", "").Trim();
                                            //Console.WriteLine(sSourceID);
                                            sReferenceURL = "http://securityfocus.com/bid/" + sSourceID;
                                            Console.WriteLine("DEBUG "+sReferenceURL);

                                            fAddReference(iVulnerabilityID);    //, sSource, sSourceID, sReferenceURL);

                                        }
                                        #endregion securityfocus
                                        else
                                        {
                                            #region xforce
                                            //http://xforce.iss.net/xforce/xfdb/123
                                            sTemp = myRegexXFORCE.Match(sLine).ToString();
                                            if (sTemp != "")
                                            {
                                                //Console.WriteLine(sTemp);
                                                sSource = "XF";
                                                sSourceID = sTemp.Replace("xforce.iss.net/xforce/xfdb/", "");
                                                sSourceID = sSourceID.Replace("/", "");
                                                sSourceID = sSourceID.Replace("\"", "").Trim();
                                                //Console.WriteLine(sSourceID);
                                                sReferenceURL = "http://xforce.iss.net/xforce/xfdb/" + sSourceID;
                                                Console.WriteLine("DEBUG "+sReferenceURL);

                                                fAddReference(iVulnerabilityID);    //, sSource, sSourceID, sReferenceURL);

                                            }
                                            #endregion xforce
                                            else
                                            {
                                                //TODO
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    sLine = srStreamReader.ReadLine();
                }

                srStreamReader.Close();
            }
            
            //FREE
            try
            {
                model.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException e)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                foreach (var eve in e.EntityValidationErrors)
                {
                    sb.AppendLine(string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                                    eve.Entry.Entity.GetType().Name,
                                                    eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        sb.AppendLine(string.Format("- Property: \"{0}\", Error: \"{1}\"",
                                                    ve.PropertyName,
                                                    ve.ErrorMessage));
                    }
                }
                //throw new DbEntityValidationException(sb.ToString(), e);
                Console.WriteLine("Exception DbEntityValidationExceptionFINALSAVE " + sb.ToString());
            }
            catch (Exception exFINALSAVE)
            {
                Console.WriteLine("Exception exFINALSAVE " + exFINALSAVE.Message + " " + exFINALSAVE.InnerException);
            }
            model.Dispose();
        }

        //static void fAddReference(XORCISMEntities model, int iVocabularySCIPID, int iVulnerabilityID, sSource, sSourceID, sReferenceURL)
        static void fAddReference(int iVulnerabilityID) //, sSource, sSourceID, sReferenceURL)
        {
            Console.WriteLine("DEBUG " + DateTimeOffset.Now.ToString());
            Console.WriteLine("DEBUG in fAddReference()");

            //TODO Normalize sReferenceURL

            //return;
            int iReferenceID = 0;
            try
            {
                iReferenceID = model.REFERENCE.Where(o => o.ReferenceURL == sReferenceURL).Select(o=>o.ReferenceID).FirstOrDefault();
            }
            catch (Exception ex)
            {

            }

            if (iReferenceID <= 0)
            {
                try
                {
                    REFERENCE oReference = new REFERENCE();
                    oReference.CreatedDate = DateTimeOffset.Now;
                    oReference.Source = sSource;    // "SECTRACK";
                    oReference.VocabularyID = iVocabularySCIPID;
                    oReference.ReferenceURL = sReferenceURL;
                    oReference.ReferenceSourceID = sSourceID;
                    oReference.ReferenceTitle = sSourceID;
                    oReference.timestamp = DateTimeOffset.Now;
                    model.REFERENCE.Add(oReference);
                    model.SaveChanges();
                    iReferenceID = oReference.ReferenceID;
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException e)
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        sb.AppendLine(string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                                        eve.Entry.Entity.GetType().Name,
                                                        eve.Entry.State));
                        foreach (var ve in eve.ValidationErrors)
                        {
                            sb.AppendLine(string.Format("- Property: \"{0}\", Error: \"{1}\"",
                                                        ve.PropertyName,
                                                        ve.ErrorMessage));
                        }
                    }
                    //throw new DbEntityValidationException(sb.ToString(), e);
                    Console.WriteLine("Exception DbEntityValidationExceptionUPDATECAPEC " + sb.ToString());
                }
                catch (Exception exReference)
                {
                    Console.WriteLine("Exception exReference " + exReference.Message + " " + exReference.InnerException);
                }
            }
            else
            {
                //Update REFERENCE
                //TODO Remove this for speed
                /*
                try
                {
                    REFERENCE oReference = model.REFERENCE.FirstOrDefault(o => o.ReferenceURL == sReferenceURL);
                    oReference.ReferenceSourceID = sSourceID;
                    model.SaveChanges();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException e)
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        sb.AppendLine(string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                                        eve.Entry.Entity.GetType().Name,
                                                        eve.Entry.State));
                        foreach (var ve in eve.ValidationErrors)
                        {
                            sb.AppendLine(string.Format("- Property: \"{0}\", Error: \"{1}\"",
                                                        ve.PropertyName,
                                                        ve.ErrorMessage));
                        }
                    }
                    //throw new DbEntityValidationException(sb.ToString(), e);
                    Console.WriteLine("Exception DbEntityValidationExceptionUPDATECAPEC " + sb.ToString());
                }
                catch(Exception exReferenceUpdate)
                {
                    Console.WriteLine("Exception exReferenceUpdate " + exReferenceUpdate.Message + " " + exReferenceUpdate.InnerException);
                }
                */
            }
            Console.WriteLine("DEBUG " + DateTimeOffset.Now.ToString());
            Console.WriteLine("DEBUG iReferenceID=" + iReferenceID);

            if (iVulnerabilityID > 0)
            {
                int iVulnerabilityReferenceID = 0;
                try
                {
                    iVulnerabilityReferenceID = vuln_nodel.VULNERABILITYFORREFERENCE.Where(o => o.ReferenceID == iReferenceID && o.VulnerabilityID == iVulnerabilityID).Select(o => o.VulnerabilityReferenceID).FirstOrDefault();
                }
                catch (Exception ex)
                {

                }

                if (iVulnerabilityReferenceID <= 0)
                {
                    try
                    {
                        VULNERABILITYFORREFERENCE oVulnerabilityReference = new VULNERABILITYFORREFERENCE();
                        oVulnerabilityReference.CreatedDate = DateTimeOffset.Now;
                        oVulnerabilityReference.ReferenceID = iReferenceID;
                        oVulnerabilityReference.VulnerabilityID = iVulnerabilityID;
                        oVulnerabilityReference.VocabularyID = iVocabularySCIPID;
                        oVulnerabilityReference.timestamp = DateTimeOffset.Now;
                        vuln_nodel.VULNERABILITYFORREFERENCE.Add(oVulnerabilityReference);
                        //vuln_nodel.SaveChanges();    //TEST PERFORMANCE
                    }
                    catch (System.Data.Entity.Validation.DbEntityValidationException e)
                    {
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        foreach (var eve in e.EntityValidationErrors)
                        {
                            sb.AppendLine(string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                                            eve.Entry.Entity.GetType().Name,
                                                            eve.Entry.State));
                            foreach (var ve in eve.ValidationErrors)
                            {
                                sb.AppendLine(string.Format("- Property: \"{0}\", Error: \"{1}\"",
                                                            ve.PropertyName,
                                                            ve.ErrorMessage));
                            }
                        }
                        //throw new DbEntityValidationException(sb.ToString(), e);
                        Console.WriteLine("Exception DbEntityValidationExceptionVULNERABILITYFORREFERENCE " + sb.ToString());
                    }
                    catch (Exception exoVulnerabilityReference)
                    {
                        Console.WriteLine("Exception exoVulnerabilityReference " + exoVulnerabilityReference.Message + " " + exoVulnerabilityReference.InnerException);
                    }
                }
                else
                {
                    //Update VULNERABILITYFORREFERENCE
                }
            }

            //TODO REFERENCEMAPPING

        }
    }
}
