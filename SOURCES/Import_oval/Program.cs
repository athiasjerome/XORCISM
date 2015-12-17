using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

using XORCISMModel;
using XOVALModel;
using XVULNERABILITYModel;
using XWINDOWSModel;

using System.Text.RegularExpressions;
using System.IO;
using System.Net;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

//using ICSharpCode.SharpZipLib.Zip;
using System.IO.Compression;

using System.Threading.Tasks;
//using System.Collections.Concurrent;

namespace Import_oval
{
    static class Program
    {
        /// <summary>
        /// Copyright (C) 2014-2015 Jerome Athias
        /// Parser for MITRE OVAL XML file and import into an XORCISM database
        /// This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation; either version 2 of the License, or (at your option) any later version.
        /// 
        /// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
        /// 
        /// You should have received a copy of the GNU General Public License along with this program; if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
        /// </summary>
        
        public static XORCISMEntities model = new XORCISMEntities();
        public static XOVALEntities oval_model = new XOVALEntities();
        public static XVULNERABILITYEntities vuln_model = new XVULNERABILITYEntities();
        public static XWINDOWSEntities windows_model = new XWINDOWSEntities();
        
        //[STAThread]

        static void Main()
        {
            string sOVALVersion = "5.11.1";    //HARDCODED

            //TODO: Manage the StatusName of OVALDEFINITION as integers. Example: ACCEPTED=1, DEPRECATED=2... to save space

            //Parse an OVAL XML file and import the values into an XORCISM database
            ////https://oval.mitre.org/rep-data/5.10/org.mitre.oval/v/index.html
            //https://oval.cisecurity.org/repository/download

            //TODO
            //http://usgcb.nist.gov/usgcb_content.html

            //https://stackoverflow.com/questions/5940225/fastest-way-of-inserting-in-entity-framework
            model.Configuration.AutoDetectChangesEnabled = false;
            model.Configuration.ValidateOnSaveEnabled = false;

            oval_model.Configuration.AutoDetectChangesEnabled = false;
            oval_model.Configuration.ValidateOnSaveEnabled = false;

            vuln_model.Configuration.AutoDetectChangesEnabled = false;
            vuln_model.Configuration.ValidateOnSaveEnabled = false;

            int iVocabularyOVALID = 0;// 9; //OVAL 5.10.1
            //Create a Vocabulary for OVAL
            #region vocabularyoval
            try
            {
                iVocabularyOVALID = model.VOCABULARY.Where(o => o.VocabularyName == "OVAL" && o.VocabularyVersion == sOVALVersion).Select(o => o.VocabularyID).FirstOrDefault();
            }
            catch (Exception ex)
            {

            }
            if (iVocabularyOVALID <= 0)
            {
                XORCISMModel.VOCABULARY oVocabulary = new XORCISMModel.VOCABULARY();
                oVocabulary.CreatedDate = DateTimeOffset.Now;
                oVocabulary.VocabularyName = "OVAL";
                oVocabulary.VocabularyVersion = sOVALVersion;
                model.VOCABULARY.Add(oVocabulary);
                model.SaveChanges();
                iVocabularyOVALID = oVocabulary.VocabularyID;
                Console.WriteLine("DEBUG iVocabularyOVALID=" + iVocabularyOVALID);
            }
            else
            {
                //Update VOCABULARY
            }
            #endregion vocabularyoval
            //<oval:schema_version>5.10.1</oval:schema_version>

            XmlDocument doc= new XmlDocument();
            //Review http://vsecurity.com/download/papers/XMLDTDEntityAttacks.pdf

            //TODO  state_operator

            //TODO
            //https://oval.mitre.org/repository/data/LatestUpdates

            #region downloadoval

            //string sDownloadFileURL = "http://oval.mitre.org/rep-data/5.10/org.mitre.oval/oval.xml.zip";    //HARDCODED
            string sDownloadFileURL = "https://oval.cisecurity.org/repository/download/5.11.1/all/oval.xml.zip";    //HARDCODED
            string sDownloadFileName = "oval.xml.zip";    //TODO Hardcoded
            string sDownloadLocalFile = "oval.xml";
            string sDownloadLocalPath = "C:/nvdcve/";   //TODO: Hardcoded
            string sDownloadLocalFolder = @"C:\nvdcve\";//TODO: Hardcoded
            FileInfo fileInfo = new FileInfo(sDownloadLocalFolder + sDownloadFileName);
            HttpWebRequest webRequest = null;
            HttpWebResponse webResponse = null;
            Int64 fileSizeRemote = new Int64();
            long fileSizeLocal = 0;
            try
            {
                fileSizeLocal = fileInfo.Length;
                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                Console.WriteLine("DEBUG " + sDownloadLocalFolder + sDownloadFileName + " FileSize:" + fileSizeLocal);
                
            }
            catch (Exception exfileSizeLocal)
            {
                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                Console.WriteLine("Exception: exfileSizeLocal " + exfileSizeLocal.Message + " " + exfileSizeLocal.InnerException);
            }

            try
            {
                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                Console.WriteLine("DEBUG Getting filesize");
                webRequest = (HttpWebRequest)WebRequest.Create(new Uri(sDownloadFileURL));
                webRequest.Method = "HEAD";
                webRequest.Timeout = 2 * 60 * 1000;    //2 minutes  Hardcoded
                //webRequest.Credentials = CredentialCache.DefaultCredentials;

                webResponse = (HttpWebResponse)webRequest.GetResponse();
                fileSizeRemote = webResponse.ContentLength;
                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                Console.WriteLine("DEBUG " + sDownloadFileURL + " FileSize:" + fileSizeRemote);
                Console.WriteLine("DEBUG Last modified: " + webResponse.LastModified);
            }
            catch (Exception exGetSizeDownload)
            {
                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                Console.WriteLine("Exception: exGetSizeDownload " + exGetSizeDownload.Message + " " + exGetSizeDownload.InnerException);
            }

            if(fileSizeRemote<=0)
            {
                Console.WriteLine("ERROR: fileSizeRemote is incorrect: "+ fileSizeRemote);
            }

            if (fileSizeRemote == fileSizeLocal)
            {

            }
            else
            {
                //Download the file
                try
                {
                    //WebClient wc = new WebClient();
                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                    Console.WriteLine("DEBUG Downloading " + sDownloadFileName);

                    //wc.DownloadFile(sDownloadFileURL, sDownloadLocalPath + sDownloadFileName);
                    //wc.Dispose();

                    webRequest = (HttpWebRequest)WebRequest.Create(new Uri(sDownloadFileURL));
                    webRequest.Method = "GET";
                    //webRequest.Credentials = CredentialCache.DefaultCredentials;
                    webRequest.Timeout = 20 * 60 * 1000;    //20 minutes    Hardcoded
                    webResponse = (HttpWebResponse)webRequest.GetResponse();

                    Stream remoteStream = webResponse.GetResponseStream();
                    Stream localStream = File.Create(sDownloadLocalPath + sDownloadFileName);
                    // Allocate a 1k buffer
                    byte[] buffer = new byte[1024];
                    int bytesRead;

                    int bytesProcessed = 0;

                    // Simple do/while loop to read from stream until no bytes are returned
                    do
                    {
                        // Read data (up to 1k) from the stream
                        bytesRead = remoteStream.Read(buffer, 0, buffer.Length);
                        // Write the data to the local file
                        localStream.Write(buffer, 0, bytesRead);
                        // Increment total bytes processed
                        bytesProcessed += bytesRead;
                        //Console.WriteLine("DEBUG Downloading: " + bytesProcessed + " bytes processed");
                    } while (bytesRead > 0);

                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                    localStream.Dispose();
                    Console.WriteLine("DEBUG Download completed");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                    Console.WriteLine("Exception: while downloading\n" + ex.Message + " " + ex.InnerException);
                }

                //Extract Zip File

                try
                {
                    //FastZip fz = new FastZip(); //Now use .NET
                    //fz.ExtractZip(@"C:\nvdcve\cwec_v2.5.xml.zip", @"C:\nvdcve\", "");
                    //fz.ExtractZip(sDownloadLocalFolder + sDownloadFileName, sDownloadLocalFolder, "");
                    //with .NET 4.5
                    ZipArchive archive = ZipFile.Open(sDownloadLocalFolder + sDownloadFileName, ZipArchiveMode.Read);
                    //File.Delete(sDownloadLocalFolder + "\\oval_repository\\" + sDownloadLocalFile); //delete oval.xml   //HARDCODED
                    File.Delete(sDownloadLocalFolder + "\\" + sDownloadLocalFile); //delete oval.xml
                    archive.ExtractToDirectory(sDownloadLocalFolder);
                    Console.WriteLine("DEBUG Extraction Complete");
                }
                catch (Exception exUnzip)
                {
                    Console.WriteLine("Exception: exUnzip: " + exUnzip.Message + " " + exUnzip.InnerException);
                }
            }
            #endregion downloadoval

            //doc.Load(@"C:\nvdcve\cisco.pix.xml");
            //doc.Load(@"C:\nvdcve\microsoft.windows.server.2012.xml");
            //doc.Load(@"C:\nvdcve\red.hat.enterprise.linux.4.xml");
            //doc.Load(@"C:\nvdcve\ubuntu.6.06.xml");
            //doc.Load(@"C:\nvdcve\Norton.xml");
            //doc.Load(@"C:\nvdcve\2014-05-02-OVAL-def-1.xml");
            //doc.Load(@"microsoft.windows.2000.xml");
            //doc.Load(@"CVE-2012-0159.xml");
            //Console.WriteLine("DEBUG LOADING FILE " + sDownloadLocalFolder + sDownloadLocalFile);
            //TODO Create the folder
            try
            {
                //doc.Load(sDownloadLocalFolder + "\\oval_repository\\" + sDownloadLocalFile);    //Hardcoded
                doc.Load(sDownloadLocalFolder + "\\" + sDownloadLocalFile);
            }
            catch (Exception exLoadDowanloadedFile)
            {
                Console.WriteLine("Exception exLoadDowanloadedFile: " + exLoadDowanloadedFile.Message + " " + exLoadDowanloadedFile.InnerException);
                return;
            }

            //TODO Check that the XML is valid against XSD

            XmlNamespaceManager mgr = new XmlNamespaceManager(doc.NameTable);

            mgr.AddNamespace("oval", "http://oval.mitre.org/XMLSchema/oval-common-5");  //Hardcoded
            mgr.AddNamespace("oval-def", "http://oval.mitre.org/XMLSchema/oval-definitions-5"); //Hardcoded


            //For testing
            bool bImportOVALDefinitions = true;
            bool bImportOVALTests = true;
            bool bImportOVALObjects = true;
            bool bImportOVALStates = true;
            bool bImportOVALVariables = true;
            //---------------------------------

            if (bImportOVALDefinitions)
            {
                int iCptNode = 0;
                #region ovaldefinition
                //Note: this is not the best/fastest way to parse XML, but is clear/easy enough
                XmlNodeList nodesDefinitions = doc.SelectNodes("/oval-def:oval_definitions/oval-def:definitions/oval-def:definition", mgr);   //Hardcoded
                //Console.WriteLine(nodes1.Count);
                foreach (XmlNode nodeDefinition in nodesDefinitions)    //definition
                {
                    iCptNode++;

                    //Free memory sometimes (OutOfMemoryException)
                    #region freememory
                    if (iCptNode > 30) //TODO Hardcoded    Review
                    {
                        Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                        Console.WriteLine("DEBUG FREE MEMORY");
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
                            Console.WriteLine("Exception: DbEntityValidationExceptionGLOBALSAVE " + sb.ToString());
                        }
                        catch (Exception exGLOBALSAVE)
                        {
                            Console.WriteLine("Exception: exGLOBALSAVE " + exGLOBALSAVE.Message + " " + exGLOBALSAVE.InnerException);
                        }
                        model.Dispose();

                        model = new XORCISMEntities();
                        model.Configuration.AutoDetectChangesEnabled = false;
                        model.Configuration.ValidateOnSaveEnabled = false;

                        iCptNode = 1;
                    }
                    #endregion freememory

                    //<definition id="oval:org.mitre.oval:def:11958" version="3" class="inventory">
                    Console.WriteLine("DEBUG *********************************************");
                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                    string myDefID = nodeDefinition.Attributes["id"].InnerText;   //oval:org.mitre.oval:def:11958
                    Console.WriteLine("DEBUG OVALDefinitionID=" + myDefID);

                    string sDefVersion = nodeDefinition.Attributes["version"].InnerText; //3
                    Console.WriteLine("DEBUG version=" + sDefVersion);

                    string sDefDeprecated = "";
                    try
                    {
                        sDefDeprecated = nodeDefinition.Attributes["deprecated"].InnerText;   //true
                    }
                    catch (Exception ex)
                    {
                        //Console.WriteLine("Exception: sDefDeprecated " + ex.Message + " " + ex.InnerException);
                    }

                    List<int> ListPlatformID = new List<int>(); //List of PlatformID for relationships
                    List<int> ListProductID = new List<int>();  //List of ProductID for relationships


                    //XORCISMModel.OVALMETADATA myOVALMetadata=new OVALMETADATA();
                    XOVALModel.OVALCRITERIA myCriteria = new OVALCRITERIA();  //We attach one principal criteria to each definition

                    string sClassValue = nodeDefinition.Attributes["class"].InnerText;    //inventory     vulnerability   patch   compliance
                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                    Console.WriteLine("DEBUG sClassValue=" + sClassValue);

                    int iOVALClassEnumerationID = 0;
                    //TODO: could be hardcoded for performance
                    #region ovalclassenumeration
                    try
                    {
                        iOVALClassEnumerationID = oval_model.OVALCLASSENUMERATION.Where(o => o.ClassValue == sClassValue).Select(o => o.OVALClassEnumerationID).FirstOrDefault();
                    }
                    catch (Exception ex)
                    {
                        //e.g. table empty
                    }
                    //XORCISMModel.OVALCLASSENUMERATION myclass=null;
                    //myclass = oval_model.OVALCLASSENUMERATION.FirstOrDefault(o => o.ClassValue == sClassValue && o.VocabularyID == iVocabularyOVALID);
                    //if (myclass == null)
                    if (iOVALClassEnumerationID <= 0)
                    {
                        try
                        {
                            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                            Console.WriteLine(string.Format("DEBUG Adding new OVALCLASSENUMERATION [{0}] in table OVALCLASSENUMERATION", sClassValue));
                            OVALCLASSENUMERATION myclass = new OVALCLASSENUMERATION();
                            myclass.ClassValue = sClassValue;   //inventory     vulnerability
                            myclass.VocabularyID = iVocabularyOVALID;
                            myclass.CreatedDate = DateTimeOffset.Now;
                            myclass.timestamp = DateTimeOffset.Now;
                            oval_model.OVALCLASSENUMERATION.Add(myclass);
                            oval_model.SaveChanges();
                            iOVALClassEnumerationID = myclass.OVALClassEnumerationID;
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
                            Console.WriteLine("Exception: DbEntityValidationExceptionUPDATECAPEC " + sb.ToString());
                        }
                        catch (Exception exmyclass)
                        {
                            Console.WriteLine("Exception: exmyclass " + exmyclass.Message + " " + exmyclass.InnerException);
                        }
                    }
                    else
                    {
                        //Update OVALCLASSENUMERATION
                    }
                    #endregion ovalclassenumeration

                    OVALDEFINITION oOVALDefinition = new OVALDEFINITION();
                    Boolean bAddDefinition = false;
                    //Boolean bAddMetadata = false;
                    //int iDefVersion = Convert.ToInt32(sDefVersion);    //3
                    //int iOVALDefinitionID=
                    oOVALDefinition = oval_model.OVALDEFINITION.FirstOrDefault(o => o.OVALDefinitionIDPattern == myDefID && o.OVALDefinitionVersion == sDefVersion);    //VocabularyID
                    if (oOVALDefinition == null)
                    {
                        bAddDefinition = true;
                        //We need to Add the Metadata
                        //bAddMetadata = true;
                        Console.WriteLine("DEBUG Adding OVALDEFINITION");
                        try
                        {
                            oOVALDefinition = new OVALDEFINITION();
                            oOVALDefinition.CreatedDate = DateTimeOffset.Now;
                            oOVALDefinition.VocabularyID = iVocabularyOVALID;
                            oOVALDefinition.OVALDefinitionIDPattern = myDefID;
                            //NOTE: we still don't know the principal OVALCriteriaID (retieved/created later)
                            //TODO Review: create it here?
                            oOVALDefinition.OVALDefinitionVersion = sDefVersion;
                            oOVALDefinition.OVALClassEnumerationID = iOVALClassEnumerationID;
                            //oOVALDefinition.ClassValue = sClassValue;   //Removed
                            if (sDefDeprecated == "true")
                            {
                                oOVALDefinition.deprecated = true;
                            }
                            oOVALDefinition.timestamp = DateTimeOffset.Now;
                            //model.OVALDEFINITION.Attach(oOVALDefinition);
                            //model.Entry(oOVALDefinition).State = EntityState.Modified;
                            oval_model.OVALDEFINITION.Add(oOVALDefinition);
                            oval_model.SaveChanges();

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
                            Console.WriteLine("Exception: DbEntityValidationExceptionUPDATECAPEC " + sb.ToString());
                        }
                        catch (Exception exoOVALDefinition)
                        {
                            Console.WriteLine("Exception: exoOVALDefinition " + exoOVALDefinition.Message + " " + exoOVALDefinition.InnerException);
                        }
                    }
                    else
                    {
                        //Update OVALDEFINITION
                        //The definition exists
                        //We should have a metadata for it
                        /*
                        myOVALMetadata = oval_model.OVALMETADATA.FirstOrDefault(o => o.OVALMetadataID == myOVALDefinition.OVALMetadataID);
                        if (myOVALMetadata == null)
                        {
                            Console.WriteLine("ERROR: METADATA not found for OVALDEFINITION");
                            //return;
                            bAddMetadata = true;
                        }
                        */
                        try
                        {
                            oOVALDefinition.VocabularyID = iVocabularyOVALID;
                            //oOVALDefinition.OVALDefinitionVersion = iDefVersion;
                            oOVALDefinition.OVALClassEnumerationID = iOVALClassEnumerationID;
                            //oOVALDefinition.ClassValue = sClassValue;   //Removed
                            if (sDefDeprecated == "true")
                            {
                                oOVALDefinition.deprecated = true;
                            }
                            oOVALDefinition.timestamp = DateTimeOffset.Now;
                            oval_model.OVALDEFINITION.Attach(oOVALDefinition);
                            oval_model.Entry(oOVALDefinition).State = EntityState.Modified;
                            oval_model.SaveChanges();
                            Console.WriteLine("DEBUG Updated OVALDEFINITION " + oOVALDefinition.OVALDefinitionID);

                        }
                        catch (Exception exUpdateoOVALDefinition)
                        {
                            Console.WriteLine("Exception: exUpdateoOVALDefinition " + exUpdateoOVALDefinition.Message + " " + exUpdateoOVALDefinition.InnerException);
                        }
                    }

                    //Parallel.ForEach(XmlNode nodeDefinitionInfo in nodeDefinition.ChildNodes)
                    foreach(XmlNode nodeDefinitionInfo in nodeDefinition.ChildNodes)
                    {

                        Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                        Console.WriteLine("DEBUG nodeDefinitionInfo.Name=" + nodeDefinitionInfo.Name);
                        if (nodeDefinitionInfo.Name == "metadata")
                        {
                            foreach (XmlNode nodeMetadata in nodeDefinitionInfo.ChildNodes)
                            {
                                if (nodeMetadata.Name == "title")
                                {
                                    string sOVALDefinitionTitle = nodeMetadata.InnerText.Trim();
                                    Console.WriteLine("DEBUG sOVALDefinitionTitle=" + sOVALDefinitionTitle);
                                    //TODO check for typos
                                    oOVALDefinition.OVALDefinitionTitle = sOVALDefinitionTitle;
                                    //Norton Internet Security is installed
                                    //TODO Regex ELSA-2013:1813
                                    //http://linux.oracle.com/errata/ELSA-2014-0139.html
                                    //Note sReferenceSourceID should be =ELSA-2014:0139-00
                                    //TODO
                                    //CVE-2013-3163 (MS13-055)
                                    #region regexCVE
                                    Regex myRegexCVE = new Regex(@"CVE-(19|20)\d\d-(0\d{3}|[1-9]\d{3,})");
                                    //https://cve.mitre.org/cve/identifiers/tech-guidance.html
                                    string strTemp = myRegexCVE.Match(sOVALDefinitionTitle).ToString();    //TODO: can we have multiple CVEs here?
                                    if (strTemp != "")
                                    {
                                        string sCVEID = strTemp;    // "CVE-" + strTemp.Replace("'", "");
                                        //Console.WriteLine(fichiers[i]);
                                        Console.WriteLine("DEBUG CVE:" + sCVEID);

                                        //VULNERABILITY myCVE = new VULNERABILITY();
                                        //myCVE = model.VULNERABILITY.FirstOrDefault(o => o.VULReferentialID == sCVEID);
                                        ////int vulnID = myCVE.VulnerabilityID;
                                        int iVulnerabilityID = 0;
                                        try
                                        {
                                            iVulnerabilityID = vuln_model.VULNERABILITY.Where(o => o.VULReferentialID == sCVEID).Select(o => o.VulnerabilityID).FirstOrDefault();
                                        }
                                        catch (Exception ex)
                                        {

                                        }
                                        //Console.WriteLine("DEBUGTEST");
                                        //if (myCVE == null)
                                        if (iVulnerabilityID <= 0)
                                        {
                                            Console.WriteLine("ERROR: Problem: this CVE is unknown: " + sCVEID);
                                            //TODO Add VULNERABILITY

                                        }
                                        else
                                        {
                                            Console.WriteLine("DEBUG VulnerabilityID:" + iVulnerabilityID);
                                        }
                                    }
                                    #endregion regexCVE

                                    //APPLE-SA-2012-03-07-1 and APPLE-SA-2012-03-07-2
                                    //USN-1879-1
                                    //http://www.ubuntu.com/usn/usn-1335-1/
                                    //DSA-1549-1
                                    //KB2608646

                                    ////TODO check if we have a related CVE- here WARNING!!! "not related, different bug"...

                                    ////OVALDefinitionVulnerabilityRelationship = "Related";

                                }
                                if (nodeMetadata.Name == "description")
                                {
                                    oOVALDefinition.OVALDefinitionDescription = nodeMetadata.InnerText;
                                    //Norton Internet Security is installed

                                    //TODO check for typos

                                    //TODO RegexCVE
                                    //oval:org.mitre.oval:def:7878
                                    /*
                                    Insufficient checking for out-of-memory conditions results in null pointer dereferences (CVE-2008-3912). Incorrect error handling logic leads to memory leaks (CVE-2008-3913) and file descriptor leaks (CVE-2008-3914).
                                    */
                                }
                            }
                        }
                        if (nodeDefinitionInfo.Name == "criteria")
                        {
                            #region criteria
                            Boolean bAddCriteria = false;
                            int iOVALCriteriaID = 0;
                            if (bAddDefinition)
                            {
                                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                Console.WriteLine(string.Format("DEBUG Adding new OVALCRITERIA [{0}] in table OVALCRITERIA", myDefID));
                                myCriteria = new OVALCRITERIA();
                                //myCriteria.VocabularyID = iVocabularyOVALID;
                                myCriteria.comment = "";
                                //myCriteria.OperatorValue = "";    //Removed
                                bAddCriteria = true;

                            }
                            else
                            {
                                //Update OVALCRITERIA
                                //The definition exists, we should have a criteria for it
                                myCriteria = oval_model.OVALCRITERIA.FirstOrDefault(o => o.OVALCriteriaID == oOVALDefinition.OVALCriteriaID);
                                //myCriteria = myOVALDefinition.OVALCRITERIA;
                                if (myCriteria == null)
                                {
                                    Console.WriteLine("ERROR: no criteria for the OVALDefinition");
                                    myCriteria = new OVALCRITERIA();
                                    //myCriteria.CreatedDate = DateTimeOffset.Now;
                                    //myCriteria.VocabularyID = iVocabularyOVALID;
                                    myCriteria.comment = "";
                                    //myCriteria.OperatorValue = "";    //Removed
                                    //myCriteria.CreatedDate = DateTimeOffset.Now;

                                    //model.OVALCRITERIA.Add(myCriteria);
                                    //model.SaveChanges();
                                    //iOVALCriteriaID = myCriteria.OVALCriteriaID;
                                    bAddCriteria = true;
                                }
                                else
                                {
                                    //Update OVALCRITERIA
                                    //iOVALCriteriaID = myCriteria.OVALCriteriaID;
                                }
                                //Console.WriteLine("DEBUG iOVALCriteriaID=" + iOVALCriteriaID);
                            }
                            
                            try
                            {
                                string sOperatorValue = "";
                                try
                                {
                                    sOperatorValue = nodeDefinitionInfo.Attributes["operator"].InnerText;  //AND
                                }
                                catch (Exception exNOsOperatorValue)
                                {
                                    //TODO REVIEW
                                    Console.WriteLine("DEBUG ERROR? no operator, replacing by AND");
                                    sOperatorValue = "AND";
                                }
                                //myCriteria.OperatorValue = sOperatorValue;  // REMOVED

                                //TODO Hardcode that?
                                #region OPERATORENUMERATION
                                int iOperatorEnumerationID = 0;
                                try
                                {
                                    iOperatorEnumerationID = oval_model.OPERATORENUMERATION.Where(o => o.OperatorValue == sOperatorValue).Select(o => o.OperatorEnumerationID).FirstOrDefault();
                                }
                                catch (Exception exiOperatorEnumerationID)
                                {
                                    Console.WriteLine("Exception: exiOperatorEnumerationID " + exiOperatorEnumerationID.Message + " " + exiOperatorEnumerationID.InnerException);
                                }

                                if (iOperatorEnumerationID <= 0)
                                {
                                    try
                                    {
                                        XOVALModel.OPERATORENUMERATION oOperatorEnumeration = new XOVALModel.OPERATORENUMERATION();
                                        oOperatorEnumeration.CreatedDate = DateTimeOffset.Now;
                                        oOperatorEnumeration.OperatorValue = sOperatorValue;
                                        oOperatorEnumeration.VocabularyID = iVocabularyOVALID;
                                        oOperatorEnumeration.timestamp = DateTimeOffset.Now;
                                        oval_model.OPERATORENUMERATION.Add(oOperatorEnumeration);
                                        oval_model.SaveChanges();
                                        iOperatorEnumerationID = oOperatorEnumeration.OperatorEnumerationID;
                                    }
                                    catch (Exception exOPERATORENUMERATION)
                                    {
                                        Console.WriteLine("Exception: exOPERATORENUMERATION " + exOPERATORENUMERATION.Message + " " + exOPERATORENUMERATION.InnerException);
                                    }
                                }
                                else
                                {
                                    //Update OPERATORENUMERATION

                                }
                                #endregion OPERATORENUMERATION
                                //Update OVALCRITERIA
                                myCriteria.OperatorEnumerationID = iOperatorEnumerationID;
                                Console.WriteLine("DEBUG iOperatorEnumerationID=" + iOperatorEnumerationID);

                            }
                            catch (Exception exmyCriteriaOperatorValue)
                            {
                                Console.WriteLine("Exception: exmyCriteriaOperatorValue myCriteria.OperatorValue " + exmyCriteriaOperatorValue.Message + " " + exmyCriteriaOperatorValue.InnerException);
                            }
                           

                            try
                            {
                                //Update OVALCRITERIA
                                myCriteria.comment = nodeDefinitionInfo.Attributes["comment"].InnerText;
                            }
                            catch (Exception ex)
                            {

                                Console.WriteLine("NOTE: No comment for CRITERIA"); //DEBUG ERROR?
                                myCriteria.comment = "";
                            }

                            try
                            {

                                if (bAddCriteria)
                                {
                                    myCriteria.CreatedDate = DateTimeOffset.Now;

                                    oval_model.OVALCRITERIA.Add(myCriteria);
                                    oval_model.SaveChanges();
                                    //iOVALCriteriaID = myCriteria.OVALCriteriaID;

                                }


                                //Update OVALCRITERIA (or create it)
                                myCriteria.VocabularyID = iVocabularyOVALID;
                                myCriteria.timestamp = DateTimeOffset.Now;
                                oval_model.OVALDEFINITION.Attach(oOVALDefinition);
                                oval_model.Entry(oOVALDefinition).State = EntityState.Modified;
                                oval_model.SaveChanges();
                                iOVALCriteriaID = myCriteria.OVALCriteriaID;

                                //Console.WriteLine("DEBUG Update OVALDEFINITION with OVALCriteriaID=" + iOVALCriteriaID);
                                //Update OVALDEFINITION
                                oOVALDefinition.OVALCriteriaID = iOVALCriteriaID;   // myCriteria.OVALCriteriaID;
                                oOVALDefinition.timestamp = DateTimeOffset.Now;
                                //model.OVALDEFINITION.Attach(oOVALDefinition);
                                oval_model.Entry(oOVALDefinition).State = EntityState.Modified;
                                oval_model.SaveChanges();    //keep this
                                //Console.WriteLine("DEBUG Updated OVALDEFINITION with OVALCriteriaID=" + oOVALDefinition.OVALCriteriaID);


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
                                Console.WriteLine("Exception: DbEntityValidationExceptionUPDATECAPEC " + sb.ToString());
                            }
                            catch (Exception exAddToOVALCRITERIA)
                            {
                                Console.WriteLine("Exception: exAddToOVALCRITERIA " + exAddToOVALCRITERIA.Message + " " + exAddToOVALCRITERIA.InnerException);
                            }

                            //********************************************************************************************************************************************************
                            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                            Console.WriteLine("DEBUG calling OVALParseCriteria() iOVALCriteriaID=" + iOVALCriteriaID);
                            //OVALParseCriteria(model, node2, myCriteria, iVocabularyOVALID);
                            //OVALParseCriteria(node2, myCriteria, iVocabularyOVALID);
                            OVALParseCriteria(nodeDefinitionInfo, iOVALCriteriaID, iVocabularyOVALID);
                            #endregion criteria
                        }
                    }

                    /*
                    try
                    {
                        if (bAddMetadata)
                        {
                            Console.WriteLine(string.Format("DEBUG Adding new OVALMETADATA [{0}] in table OVALMETADATA", myDefID));
                            myOVALMetadata.CreatedDate = DateTimeOffset.Now;
                            myOVALMetadata.VocabularyID = iVocabularyOVALID;
                            model.OVALMETADATA.Add(myOVALMetadata);
                        }
                        else
                        {
                            //Update OVALMETADATA
                        }
                        myOVALMetadata.timestamp = DateTimeOffset.Now;
                        model.SaveChanges();
                    }
                    catch (Exception exAddToOVALMETADATA)
                    {
                        Console.WriteLine("Exception: exAddToOVALMETADATA " + exAddToOVALMETADATA.Message + " " + exAddToOVALMETADATA.InnerException);
                    }
                    */

                    //TODO: not optimized
                    //Parse it again to add links
                    foreach (XmlNode nodeDefinitionInfo in nodeDefinition.ChildNodes)
                    {
                        Console.WriteLine("DEBUG2 " + DateTimeOffset.Now);
                        Console.WriteLine("DEBUG2 nodeDefinitionInfo.Name=" + nodeDefinitionInfo.Name);
                        if (nodeDefinitionInfo.Name == "metadata")
                        {
                            foreach (XmlNode nodeMetadata in nodeDefinitionInfo.ChildNodes)
                            {
                                switch (nodeMetadata.Name)
                                {
                                    case "affected":
                                        //TODO: could be hardcoded for performance
                                        #region ovalosfamily
                                        string sOSFamily = nodeMetadata.Attributes["family"].InnerText; //pixos windows
                                        Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                        Console.WriteLine("DEBUG sOSFamily=" + sOSFamily);
                                        int iOSFamilyID = 0;
                                        try
                                        {
                                            iOSFamilyID = model.OSFAMILY.Where(o => o.FamilyName == sOSFamily).Select(o => o.OSFamilyID).FirstOrDefault();
                                        }
                                        catch (Exception ex)
                                        {

                                        }
                                        //XORCISMModel.OSFAMILY myOSFamily;
                                        //myOSFamily = model.OSFAMILY.FirstOrDefault(o => o.FamilyName == sOSFamily && o.VocabularyID == iVocabularyOVALID);
                                        //if (myOSFamily == null)
                                        if (iOSFamilyID <= 0)
                                        {
                                            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                            Console.WriteLine(string.Format("DEBUG Adding new OSFAMILY [{0}] in table OSFAMILY", sOSFamily));

                                            try
                                            {
                                                OSFAMILY myOSFamily = new OSFAMILY();
                                                myOSFamily.FamilyName = sOSFamily;  //windows
                                                myOSFamily.VocabularyID = iVocabularyOVALID;
                                                myOSFamily.CreatedDate = DateTimeOffset.Now;
                                                myOSFamily.timestamp = DateTimeOffset.Now;
                                                model.OSFAMILY.Add(myOSFamily);
                                                model.SaveChanges();
                                                iOSFamilyID = myOSFamily.OSFamilyID;
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
                                                Console.WriteLine("Exception: DbEntityValidationExceptionOSFAMILY " + sb.ToString());
                                            }
                                            catch (Exception exAddToOSFAMILY)
                                            {
                                                Console.WriteLine("Exception: exAddToOSFAMILY " + exAddToOSFAMILY.Message + " " + exAddToOSFAMILY.InnerException);
                                            }
                                        }
                                        else
                                        {
                                            //Update OSFAMILY
                                        }

                                        #region OVALDEFINITIONFAMILY
                                        int iOVALDefinitionFamilyID = 0;
                                        try
                                        {
                                            iOVALDefinitionFamilyID = oval_model.OVALDEFINITIONFAMILY.Where(o => o.OVALDefinitionID == oOVALDefinition.OVALDefinitionID && o.OSFamilyID == iOSFamilyID).Select(o => o.OVALDefinitionFamilyID).FirstOrDefault();
                                        }
                                        catch (Exception ex)
                                        {

                                        }

                                        //XORCISMModel.OVALDEFINITIONFAMILY oOVALDefinitionOSFamily;
                                        //oOVALDefinitionOSFamily = oval_model.OVALDEFINITIONFAMILY.FirstOrDefault(o => o.OVALDefinitionID==oOVALDefinition.OVALDefinitionID && o.OSFamilyID == iOSFamilyID);
                                        //if (oOVALDefinitionOSFamily == null)
                                        if (iOVALDefinitionFamilyID <= 0)
                                        {
                                            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                            Console.WriteLine(string.Format("DEBUG Adding new OVALDEFINITIONFAMILY [{0}] in table OVALDEFINITIONFAMILY", sOSFamily));

                                            try
                                            {
                                                OVALDEFINITIONFAMILY oOVALDefinitionOSFamily = new OVALDEFINITIONFAMILY();
                                                oOVALDefinitionOSFamily.OVALDefinitionID = oOVALDefinition.OVALDefinitionID;
                                                oOVALDefinitionOSFamily.OSFamilyID = iOSFamilyID;    // myOSFamily.OSFamilyID;

                                                oOVALDefinitionOSFamily.VocabularyID = iVocabularyOVALID;
                                                oOVALDefinitionOSFamily.CreatedDate = DateTimeOffset.Now;
                                                oOVALDefinitionOSFamily.timestamp = DateTimeOffset.Now;
                                                oval_model.OVALDEFINITIONFAMILY.Add(oOVALDefinitionOSFamily);
                                                oval_model.SaveChanges();
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
                                                Console.WriteLine("Exception: DbEntityValidationExceptionOVALDEFINITIONFAMILY " + sb.ToString());
                                            }
                                            catch (Exception exAddToOVALDEFINITIONFAMILY)
                                            {
                                                Console.WriteLine("Exception: exAddToOVALDEFINITIONFAMILY " + exAddToOVALDEFINITIONFAMILY.Message + " " + exAddToOVALDEFINITIONFAMILY.InnerException);
                                            }
                                        }
                                        else
                                        {
                                            //Update OVALDEFINITIONFAMILY
                                        }
                                        #endregion OVALDEFINITIONFAMILY
                                        #endregion ovalosfamily

                                        //OVALAFFECTEDFOROVALMETADATA
                                        //TODO: PRODUCTPLATFORM (done later in Vulnerability)
                                        foreach (XmlNode nodePlatform in nodeMetadata.ChildNodes)
                                        {
                                            if (nodePlatform.Name == "platform")
                                            {
                                                #region ovalplatform
                                                //OVALDEFINITIONPLATFORM
                                                string sPlatform = nodePlatform.InnerText.Trim();  //Cisco PIX     Microsoft Windows 8
                                                //Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                //Console.WriteLine("DEBUG sPlatform=" + sPlatform);
                                                //TODO: check for "typos"
                                                int iPlatformID = 0;
                                                try
                                                {
                                                    iPlatformID = model.PLATFORM.Where(o => o.PlatformName == sPlatform).Select(o => o.PlatformID).FirstOrDefault();  //Note: case insensitive
                                                }
                                                catch (Exception ex)
                                                {

                                                }
                                                //XORCISMModel.PLATFORM myPlatform;
                                                //myPlatform = model.PLATFORM.FirstOrDefault(o => o.PlatformName == sPlatform);// && o.VocabularyID == iVocabularyOVALID);   
                                                //if (myPlatform == null)
                                                if (iPlatformID <= 0)
                                                {
                                                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                    Console.WriteLine(string.Format("DEBUG Adding new PLATFORM [{0}] in table PLATFORM", sPlatform));

                                                    try
                                                    {
                                                        PLATFORM myPlatform = new PLATFORM();
                                                        myPlatform.PlatformName = sPlatform;
                                                        //TODO algo for PLATFORMMAPPING (OVAL/CCE/Metasploit)
                                                        myPlatform.VocabularyID = iVocabularyOVALID;
                                                        myPlatform.CreatedDate = DateTimeOffset.Now;
                                                        myPlatform.timestamp = DateTimeOffset.Now;
                                                        model.PLATFORM.Add(myPlatform);
                                                        model.SaveChanges();
                                                        iPlatformID = myPlatform.PlatformID;
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
                                                        Console.WriteLine("Exception: DbEntityValidationExceptionPLATFORM " + sb.ToString());
                                                    }
                                                    catch (Exception exAddToPLATFORM)
                                                    {
                                                        Console.WriteLine("Exception: exAddToPLATFORM " + exAddToPLATFORM.Message + " " + exAddToPLATFORM.InnerException);
                                                    }
                                                }
                                                else
                                                {
                                                    //Update PLATFORM
                                                    //TODO: the case could have changed
                                                    //sPlatform.Equals(o.PlatformName, StringComparison.Ordinal);

                                                }
                                                ListPlatformID.Add(iPlatformID);

                                                //OVALAFFECTEDPLATFORM


                                                int iOSFamilyPlatformID = 0;
                                                try
                                                {
                                                    iOSFamilyPlatformID = model.OSFAMILYPLATFORM.Where(o => o.OSFamilyID == iOSFamilyID && o.PlatformID == iPlatformID).Select(o => o.OSFamilyPlatformID).FirstOrDefault();
                                                }
                                                catch (Exception ex)
                                                {

                                                }
                                                if (iOSFamilyPlatformID <= 0)
                                                {
                                                    try
                                                    {
                                                        OSFAMILYPLATFORM oOSFamilyPlatform = new OSFAMILYPLATFORM();
                                                        oOSFamilyPlatform.CreatedDate = DateTimeOffset.Now;
                                                        oOSFamilyPlatform.OSFamilyID = iOSFamilyID;
                                                        oOSFamilyPlatform.PlatformID = iPlatformID;
                                                        oOSFamilyPlatform.VocabularyID = iVocabularyOVALID;
                                                        oOSFamilyPlatform.timestamp = DateTimeOffset.Now;
                                                        model.OSFAMILYPLATFORM.Add(oOSFamilyPlatform);
                                                        //model.SaveChanges();    //TEST PERFORMANCE
                                                        //iOSFamilyPlatformID=
                                                    }
                                                    catch (Exception exOSFAMILYPLATFORM)
                                                    {
                                                        Console.WriteLine("Exception: exOSFAMILYPLATFORM " + exOSFAMILYPLATFORM.Message + " " + exOSFAMILYPLATFORM.InnerException);
                                                    }
                                                }
                                                else
                                                {
                                                    //Update OSFAMILYPLATFORM
                                                    //Update VocabularyID?
                                                }

                                                int iOVALDefinitionPlatformID = 0;
                                                try
                                                {
                                                    iOVALDefinitionPlatformID = oval_model.OVALDEFINITIONPLATFORM.Where(o => o.OVALDefinitionID == oOVALDefinition.OVALDefinitionID && o.PlatformID == iPlatformID).Select(o => o.OVALDefinitionPlatformID).FirstOrDefault();
                                                }
                                                catch (Exception ex)
                                                {

                                                }

                                                //XORCISMModel.OVALDEFINITIONPLATFORM oOVALDefinitionPlatform;
                                                //oOVALDefinitionPlatform = oval_model.OVALDEFINITIONPLATFORM.FirstOrDefault(o => o.OVALDefinitionID == oOVALDefinition.OVALDefinitionID && o.PlatformID == iPlatformID);
                                                //if (oOVALDefinitionPlatform == null)
                                                if (iOVALDefinitionPlatformID <= 0)
                                                {
                                                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                    Console.WriteLine(string.Format("DEBUG Adding new OVALDEFINITIONPLATFORM [{0}] in table OVALDEFINITIONPLATFORM", sPlatform));

                                                    try
                                                    {
                                                        OVALDEFINITIONPLATFORM oOVALDefinitionPlatform = new OVALDEFINITIONPLATFORM();
                                                        oOVALDefinitionPlatform.OVALDefinitionID = oOVALDefinition.OVALDefinitionID;
                                                        oOVALDefinitionPlatform.PlatformID = iPlatformID;    // myPlatform.PlatformID;
                                                        oOVALDefinitionPlatform.VocabularyID = iVocabularyOVALID;
                                                        oOVALDefinitionPlatform.CreatedDate = DateTimeOffset.Now;
                                                        oOVALDefinitionPlatform.timestamp = DateTimeOffset.Now;
                                                        oval_model.OVALDEFINITIONPLATFORM.Add(oOVALDefinitionPlatform);
                                                        //oval_model.SaveChanges();    //TEST PERFORMANCE
                                                        //iOVALDefinitionPlatformID=
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
                                                        Console.WriteLine("Exception: DbEntityValidationExceptionOVALDEFINITIONPLATFORM " + sb.ToString());
                                                    }
                                                    catch (Exception exAddToOVALDEFINITIONPLATFORM)
                                                    {
                                                        Console.WriteLine("Exception: exAddToOVALDEFINITIONPLATFORM " + exAddToOVALDEFINITIONPLATFORM.Message + " " + exAddToOVALDEFINITIONPLATFORM.InnerException);
                                                    }
                                                }
                                                else
                                                {
                                                    //Update OVALDEFINITIONPLATFORM
                                                    //Update VocabularyID?
                                                }
                                                #endregion ovalplatform
                                            }
                                            else
                                            {
                                                if (nodePlatform.Name == "product")
                                                {
                                                    #region ovalproduct
                                                    string sProductName = nodePlatform.InnerText;   //Kaspersky Endpoint Security 8
                                                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                    Console.WriteLine("DEBUG sProductName=" + sProductName);
                                                    //NOTE: case insensitive
                                                    //TODO: check for "typos"
                                                    int iProductID = 0;
                                                    try
                                                    {
                                                        iProductID = model.PRODUCT.Where(o => o.ProductName == sProductName).Select(o => o.ProductID).FirstOrDefault();   //NOTE: case insensitive
                                                    }
                                                    catch (Exception ex)
                                                    {

                                                    }
                                                    //XORCISMModel.PRODUCT myProduct;
                                                    //myProduct = model.PRODUCT.FirstOrDefault(o => o.ProductName == sProductName);
                                                    //if (myProduct == null)
                                                    if (iProductID <= 0)
                                                    {
                                                        Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                        Console.WriteLine(string.Format("DEBUG Adding new PRODUCT [{0}] in table PRODUCT", sProductName));

                                                        try
                                                        {
                                                            PRODUCT myProduct = new PRODUCT();
                                                            myProduct.ProductName = sProductName;
                                                            //TODO: search for CPE (SWID)
                                                            myProduct.VocabularyID = iVocabularyOVALID;
                                                            myProduct.CreatedDate = DateTimeOffset.Now;
                                                            myProduct.timestamp = DateTimeOffset.Now;
                                                            model.PRODUCT.Add(myProduct);
                                                            model.SaveChanges();
                                                            iProductID = myProduct.ProductID;
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
                                                            Console.WriteLine("Exception: DbEntityValidationExceptionPRODUCT " + sb.ToString());
                                                        }
                                                        catch (Exception exAddToPRODUCT)
                                                        {
                                                            Console.WriteLine("Exception: exAddToPRODUCT " + exAddToPRODUCT.Message + " " + exAddToPRODUCT.InnerException);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //Update PRODUCT
                                                        //The case could have changed
                                                        //VocabularyID?
                                                    }
                                                    ListProductID.Add(iProductID);

                                                    //TODO PRODUCTPLATFORM

                                                    #region OVALDEFINITIONPRODUCT
                                                    int iOVALDefinitionProductID = 0;
                                                    try
                                                    {
                                                        iOVALDefinitionProductID = oval_model.OVALDEFINITIONPRODUCT.Where(o => o.OVALDefinitionID == oOVALDefinition.OVALDefinitionID && o.ProductID == iProductID).Select(o => o.OVALDefinitionProductID).FirstOrDefault();
                                                    }
                                                    catch (Exception ex)
                                                    {

                                                    }

                                                    //XORCISMModel.OVALDEFINITIONPRODUCT oOVALDefinitionProduct;
                                                    //oOVALDefinitionProduct = oval_model.OVALDEFINITIONPRODUCT.FirstOrDefault(o => o.OVALDefinitionID == oOVALDefinition.OVALDefinitionID && o.ProductID == iProductID);
                                                    //if (oOVALDefinitionProduct == null)
                                                    if (iOVALDefinitionProductID <= 0)
                                                    {
                                                        Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                        Console.WriteLine(string.Format("DEBUG Adding new OVALDEFINITIONPRODUCT [{0}] in table OVALDEFINITIONPRODUCT", sProductName));

                                                        try
                                                        {
                                                            OVALDEFINITIONPRODUCT oOVALDefinitionProduct = new OVALDEFINITIONPRODUCT();
                                                            oOVALDefinitionProduct.OVALDefinitionID = oOVALDefinition.OVALDefinitionID;
                                                            oOVALDefinitionProduct.ProductID = iProductID;   // myProduct.ProductID;
                                                            oOVALDefinitionProduct.VocabularyID = iVocabularyOVALID;
                                                            oOVALDefinitionProduct.CreatedDate = DateTimeOffset.Now;
                                                            oOVALDefinitionProduct.timestamp = DateTimeOffset.Now;
                                                            oval_model.OVALDEFINITIONPRODUCT.Add(oOVALDefinitionProduct);
                                                            //oval_model.SaveChanges();    //TEST PERFORMANCE
                                                            //iOVALDefinitionProductID=
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
                                                            Console.WriteLine("Exception: DbEntityValidationExceptionOVALDEFINITIONPRODUCT " + sb.ToString());
                                                        }
                                                        catch (Exception exAddToOVALDEFINITIONPRODUCT)
                                                        {
                                                            Console.WriteLine("Exception: exAddToOVALDEFINITIONPRODUCT " + exAddToOVALDEFINITIONPRODUCT.Message + " " + exAddToOVALDEFINITIONPRODUCT.InnerException);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //Update OVALDEFINITIONPRODUCT
                                                        //VocabularyID?
                                                    }
                                                    #endregion OVALDEFINITIONPRODUCT
                                                    #endregion ovalproduct
                                                }
                                                else
                                                {
                                                    Console.WriteLine("ERROR: Import_oval missing code for definition affected " + nodePlatform.Name);
                                                }
                                            }
                                        }
                                        break;

                                    case "reference":

                                        #region ovaldefinitionreference
                                        string sReferenceSource = nodeMetadata.Attributes["source"].InnerText;  //CVE   CPE     CCE
                                        Console.WriteLine("DEBUG sReferenceSource=" + sReferenceSource);
                                        string sReferenceSourceID = nodeMetadata.Attributes["ref_id"].InnerText;    //CVE-2008-3817     cpe:/o:ibm:aix:4.3  CCE-6224-0 or URL
                                        Console.WriteLine("DEBUG sReferenceSourceID=" + sReferenceSourceID);
                                        string sReferenceUrl = "";
                                        try
                                        {
                                            sReferenceUrl = nodeMetadata.Attributes["ref_url"].InnerText;
                                            //TODO Normalize
                                            //https://rhn.redhat.com/errata/RHSA-2014-0185.html
                                            //Note: sReferenceSourceID should be =RHSA-2014:0185-00
                                            //
                                            //http://cve.mitre.org/cgi-bin/cvename.cgi?name=CVE-2008-0006
                                            //https://www.redhat.com/security/data/cve/CVE-2013-6466.html
                                            //http://linux.oracle.com/cve/CVE-2013-5878.html
                                            //TODO
                                            //http://www.ubuntu.com/usn/usn-617-1/
                                            //http://www.debian.org/security/dsa-1549-1

                                            //TODO Normalize
                                            sReferenceUrl = sReferenceUrl.Replace("http://www.", "http://");
                                            sReferenceUrl = sReferenceUrl.Replace("https://www.", "https://");  //Review


                                            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                            Console.WriteLine("DEBUG sReferenceUrl=" + sReferenceUrl);
                                        }
                                        catch (Exception ex)
                                        {

                                        }
                                        if (sReferenceSource.ToLower() == "cve")
                                        {
                                            #region ovaldefinitionreferencecve
                                            //VULNERABILITY
                                            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                            Console.WriteLine("DEBUG Vulnerability=" + sReferenceSourceID); //CVE-2008-3817
                                            //TODO: double check that it is a correct CVEID (regex)

                                            #region VULNERABILITY
                                            //Check if the vulnerability (CVE) already exists in the database, otherwise add it
                                            int iVulnerabilityID = 0;
                                            try
                                            {
                                                iVulnerabilityID = vuln_model.VULNERABILITY.Where(o => o.VULReferential == "cve" && o.VULReferentialID == sReferenceSourceID).Select(o => o.VulnerabilityID).FirstOrDefault();
                                            }
                                            catch (Exception ex)
                                            {

                                            }
                                            //XORCISMModel.VULNERABILITY myVuln;
                                            //myVuln = model.VULNERABILITY.FirstOrDefault(o => o.VULReferential == "cve" && o.VULReferentialID == sReferenceSourceID);
                                            //if (myVuln == null)
                                            if (iVulnerabilityID <= 0)
                                            {
                                                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                Console.WriteLine(string.Format("DEBUG Adding new VULNERABILITY [{0}] in table VULNERABILITY", sReferenceSourceID));

                                                try
                                                {
                                                    VULNERABILITY myVuln = new VULNERABILITY();
                                                    myVuln.VULReferential = "cve";
                                                    //TODO: add the Title and Description
                                                    //TODO check if CVE- format is valid
                                                    myVuln.VULReferentialID = sReferenceSourceID;  //CVE-2008-3817
                                                    myVuln.VULURL = sReferenceUrl;
                                                    //http://linux.oracle.com/cve/CVE-2013-5878.html
                                                    myVuln.CreatedDate = DateTimeOffset.Now;
                                                    myVuln.timestamp = DateTimeOffset.Now;
                                                    myVuln.VocabularyID = iVocabularyOVALID;
                                                    vuln_model.VULNERABILITY.Add(myVuln);
                                                    vuln_model.SaveChanges();
                                                    iVulnerabilityID = myVuln.VulnerabilityID;
                                                }
                                                catch (Exception exAddToVULNERABILITY)
                                                {
                                                    Console.WriteLine("Exception: exAddToVULNERABILITY " + exAddToVULNERABILITY.Message + " " + exAddToVULNERABILITY.InnerException);
                                                }
                                            }
                                            else
                                            {
                                                //Update VULNERABILITY
                                            }
                                            #endregion VULNERABILITY

                                            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                            Console.WriteLine("DEBUG iVulnerabilityID=" + iVulnerabilityID);

                                            #region OVALDEFINITIONVULNERABILITY
                                            //OVALMETADATAVULNERABILITY
                                            int iOVALDefinitionVulnerabilityID = 0;
                                            try
                                            {
                                                iOVALDefinitionVulnerabilityID = oval_model.OVALDEFINITIONVULNERABILITY.Where(o => o.OVALDefinitionID == oOVALDefinition.OVALDefinitionID && o.VulnerabilityID == iVulnerabilityID).Select(o => o.OVALDefinitionVulnerabilityID).FirstOrDefault();
                                            }
                                            catch (Exception ex)
                                            {

                                            }
                                            //XORCISMModel.OVALMETADATAVULNERABILITY myOVALVuln;
                                            //myOVALVuln = oval_model.OVALMETADATAVULNERABILITY.FirstOrDefault(o => o.OVALMetadataID == myOVALMetadata.OVALMetadataID && o.VulnerabilityID == iVulnerabilityID);
                                            //if (myOVALVuln == null)
                                            if (iOVALDefinitionVulnerabilityID <= 0)
                                            {
                                                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                Console.WriteLine(string.Format("DEBUG Adding new OVALDEFINITIONVULNERABILITY [{0}] in table OVALDEFINITIONVULNERABILITY", sReferenceSourceID));

                                                try
                                                {
                                                    OVALDEFINITIONVULNERABILITY myOVALVuln = new OVALDEFINITIONVULNERABILITY();
                                                    myOVALVuln.OVALDefinitionID = oOVALDefinition.OVALDefinitionID;
                                                    myOVALVuln.VulnerabilityID = iVulnerabilityID;  // myVuln.VulnerabilityID;
                                                    myOVALVuln.VocabularyID = iVocabularyOVALID;
                                                    myOVALVuln.CreatedDate = DateTimeOffset.Now;
                                                    myOVALVuln.timestamp = DateTimeOffset.Now;
                                                    oval_model.OVALDEFINITIONVULNERABILITY.Add(myOVALVuln);
                                                    oval_model.SaveChanges();
                                                }
                                                catch (Exception exAddToOVALDEFINITIONTAVULNERABILITY)
                                                {
                                                    Console.WriteLine("Exception: exAddToOVALDEFINITIONTAVULNERABILITY " + exAddToOVALDEFINITIONTAVULNERABILITY.Message + " " + exAddToOVALDEFINITIONTAVULNERABILITY.InnerException);
                                                }
                                            }
                                            else
                                            {
                                                //Update OVALDEFINITIONVULNERABILITY
                                            }
                                            #endregion OVALDEFINITIONVULNERABILITY

                                            #region VULNERABILITYREFERENCE
                                            //could be https://www.redhat.com/security/data/cve/CVE-2013-6466.html
                                            int iReferenceID = 0;
                                            try
                                            {
                                                iReferenceID = model.REFERENCE.Where(o => o.ReferenceURL == sReferenceUrl).Select(o => o.ReferenceID).FirstOrDefault();
                                            }
                                            catch (Exception ex)
                                            {

                                            }
                                            if (iReferenceID > 0)
                                            {
                                                //Update REFERENCE
                                            }
                                            else
                                            {
                                                REFERENCE oReference = new REFERENCE();
                                                oReference.CreatedDate = DateTimeOffset.Now;
                                                oReference.ReferenceURL = sReferenceUrl;

                                                oReference.VocabularyID = iVocabularyOVALID;
                                                oReference.timestamp = DateTimeOffset.Now;
                                                model.REFERENCE.Add(oReference);
                                                model.SaveChanges();
                                                iReferenceID = oReference.ReferenceID;
                                            }

                                            int iVulnerabilityReferenceID = 0;
                                            try
                                            {
                                                iVulnerabilityReferenceID = vuln_model.VULNERABILITYFORREFERENCE.Where(o => o.VulnerabilityID == iVulnerabilityID && o.ReferenceID == iReferenceID).Select(o => o.VulnerabilityReferenceID).FirstOrDefault();
                                            }
                                            catch (Exception ex)
                                            {

                                            }
                                            if (iVulnerabilityReferenceID > 0)
                                            {
                                                //Update VULNERABILITYFORREFERENCE
                                            }
                                            else
                                            {
                                                try
                                                {
                                                    VULNERABILITYFORREFERENCE oVulnerabilityReference = new VULNERABILITYFORREFERENCE();
                                                    oVulnerabilityReference.CreatedDate = DateTimeOffset.Now;
                                                    oVulnerabilityReference.VulnerabilityID = iVulnerabilityID;
                                                    oVulnerabilityReference.ReferenceID = iReferenceID;

                                                    oVulnerabilityReference.VocabularyID = iVocabularyOVALID;
                                                    oVulnerabilityReference.timestamp = DateTimeOffset.Now;
                                                    vuln_model.VULNERABILITYFORREFERENCE.Add(oVulnerabilityReference);
                                                    vuln_model.SaveChanges();    //TEST PERFORMANCE
                                                    //iVulnerabilityReferenceID=
                                                }
                                                catch (Exception exVULNERABILITYFORREFERENCE)
                                                {
                                                    Console.WriteLine("Exception: exVULNERABILITYFORREFERENCE " + exVULNERABILITYFORREFERENCE.Message + " " + exVULNERABILITYFORREFERENCE.InnerException);
                                                }

                                            }
                                            #endregion VULNERABILITYREFERENCE

                                            //TODO: VULNERABILITYFORCPE

                                            foreach (int iPlatformID in ListPlatformID)
                                            {
                                                //Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                //Console.WriteLine("DEBUG iPlatformID=" + iPlatformID);

                                                #region VULNERABILITYFORPLATFORM
                                                try
                                                {

                                                    int iPlatformVulnerabilityID = 0;
                                                    try
                                                    {
                                                        iPlatformVulnerabilityID = vuln_model.VULNERABILITYFORPLATFORM.Where(o => o.PlatformID == iPlatformID && o.VulnerabilityID == iVulnerabilityID).Select(o => o.PlatformVulnerabilityID).FirstOrDefault();
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        //e.g. table VULNERABILITYFORPLATFORM empty
                                                    }
                                                    if (iPlatformVulnerabilityID <= 0)
                                                    {
                                                        Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                        Console.WriteLine("DEBUG Adding new VULNERABILITYFORPLATFORM");
                                                        try
                                                        {
                                                            VULNERABILITYFORPLATFORM oPlatformVulnerability = new VULNERABILITYFORPLATFORM();
                                                            oPlatformVulnerability.PlatformID = iPlatformID;
                                                            oPlatformVulnerability.VulnerabilityID = iVulnerabilityID;
                                                            oPlatformVulnerability.CreatedDate = DateTimeOffset.Now;
                                                            oPlatformVulnerability.VocabularyID = iVocabularyOVALID;
                                                            oPlatformVulnerability.timestamp = DateTimeOffset.Now;
                                                            vuln_model.VULNERABILITYFORPLATFORM.Add(oPlatformVulnerability);
                                                            vuln_model.SaveChanges();    //TEST PERFORMANCE
                                                            //iPlatformVulnerabilityID=
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
                                                            Console.WriteLine("Exception: DbEntityValidationExceptionUPDATECAPEC " + sb.ToString());
                                                        }
                                                        catch (Exception exoPlatformVulnerability)
                                                        {
                                                            Console.WriteLine("Exception: exoPlatformVulnerability " + exoPlatformVulnerability.Message + " " + exoPlatformVulnerability.InnerException);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //Update VULNERABILITYFORPLATFORM
                                                    }
                                                }
                                                catch (Exception exVULNERABILITYFORPLATFORM)
                                                {
                                                    Console.WriteLine("Exception: exVULNERABILITYFORPLATFORM " + exVULNERABILITYFORPLATFORM.Message + " " + exVULNERABILITYFORPLATFORM.InnerException);
                                                }
                                                #endregion VULNERABILITYFORPLATFORM

                                                #region PRODUCTPLATFORM
                                                foreach (int iProductID in ListProductID)
                                                {
                                                    int iProductPlatformID = 0;
                                                    try
                                                    {
                                                        iProductPlatformID = model.PRODUCTPLATFORM.Where(o => o.PlatformID == iPlatformID && o.ProductID == iProductID).Select(o => o.ProductPlaformID).FirstOrDefault();
                                                    }
                                                    catch (Exception ex)
                                                    {

                                                    }
                                                    if (iProductPlatformID <= 0)
                                                    {
                                                        Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                        Console.WriteLine("DEBUG Adding new PRODUCTPLATFORM");
                                                        try
                                                        {
                                                            PRODUCTPLATFORM oProductPlatform = new PRODUCTPLATFORM();
                                                            oProductPlatform.CreatedDate = DateTimeOffset.Now;
                                                            oProductPlatform.ProductID = iProductID;
                                                            oProductPlatform.PlatformID = iPlatformID;
                                                            oProductPlatform.VocabularyID = iVocabularyOVALID;
                                                            oProductPlatform.timestamp = DateTimeOffset.Now;
                                                            model.PRODUCTPLATFORM.Add(oProductPlatform);
                                                            //model.SaveChanges();    //TEST PERFORMANCE
                                                            //iProductPlatformID=
                                                        }
                                                        catch (Exception exPRODUCTPLATFORM)
                                                        {
                                                            Console.WriteLine("Exception: exPRODUCTPLATFORM " + exPRODUCTPLATFORM.Message + " " + exPRODUCTPLATFORM.InnerException);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //Update PRODUCTPLATFORM
                                                    }
                                                }
                                                #endregion PRODUCTPLATFORM
                                            }



                                            foreach (int iProductID in ListProductID)
                                            {
                                                #region VULNERABILITYFORPRODUCT
                                                int iProductVulnerabilityID = 0;
                                                try
                                                {
                                                    iProductVulnerabilityID = vuln_model.VULNERABILITYFORPRODUCT.Where(o => o.ProductID == iProductID && o.VulnerabilityID == iVulnerabilityID).Select(o => o.ProductVulnerabilityID).FirstOrDefault();
                                                }
                                                catch (Exception ex)
                                                {
                                                    //e.g. table empty
                                                }
                                                if (iProductVulnerabilityID <= 0)
                                                {
                                                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                    Console.WriteLine("DEBUG Adding new VULNERABILITYFORPRODUCT");
                                                    try
                                                    {
                                                        VULNERABILITYFORPRODUCT oProductVulnerability = new VULNERABILITYFORPRODUCT();
                                                        oProductVulnerability.ProductID = iProductID;
                                                        oProductVulnerability.VulnerabilityID = iVulnerabilityID;
                                                        oProductVulnerability.CreatedDate = DateTimeOffset.Now;
                                                        oProductVulnerability.VocabularyID = iVocabularyOVALID;
                                                        oProductVulnerability.timestamp = DateTimeOffset.Now;
                                                        vuln_model.VULNERABILITYFORPRODUCT.Add(oProductVulnerability);
                                                        vuln_model.SaveChanges();    //TEST PERFORMANCE
                                                        //iProductVulnerabilityID=
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
                                                        Console.WriteLine("Exception: DbEntityValidationExceptionUPDATECAPEC " + sb.ToString());
                                                    }
                                                    catch (Exception exoProductVulnerability)
                                                    {
                                                        Console.WriteLine("Exception: exoProductVulnerability " + exoProductVulnerability.Message + " " + exoProductVulnerability.InnerException);
                                                    }
                                                }
                                                else
                                                {
                                                    //Update VULNERABILITYFORPRODUCT
                                                }
                                                #endregion VULNERABILITYFORPRODUCT

                                            }


                                            //TODO OSFAMILY?
                                            #endregion ovaldefinitionreferencecve
                                        }
                                        else
                                        {

                                            if (sReferenceSource.ToLower() == "cpe")
                                            {
                                                Console.WriteLine("DEBUG ReferenceCPE");
                                                #region ovaldefinitionreferencecpe
                                                //OVALMETADATACPE
                                                //string sCPEID = nodeMetadata.Attributes["ref_id"].InnerText;    sReferenceSourceID
                                                //XORCISMModel.CPE myCPE;
                                                //myCPE = model.CPE.FirstOrDefault(o => o.CPEName == sReferenceSourceID);
                                                int iCPEID = 0;
                                                try
                                                {
                                                    iCPEID = model.CPE.Where(o => o.CPEName == sReferenceSourceID).Select(o => o.CPEID).FirstOrDefault();
                                                }
                                                catch (Exception ex)
                                                {
                                                    iCPEID = 0;
                                                }
                                                //if (myCPE == null)
                                                if (iCPEID <= 0)
                                                {
                                                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                    Console.WriteLine("DEBUG Adding new CPE " + sReferenceSourceID);

                                                    try
                                                    {
                                                        CPE myCPE = new CPE();
                                                        //TODO check if valid
                                                        myCPE.CPEName = sReferenceSourceID;
                                                        myCPE.VocabularyID = iVocabularyOVALID;
                                                        myCPE.CreatedDate = DateTimeOffset.Now;
                                                        myCPE.timestamp = DateTimeOffset.Now;
                                                        model.CPE.Add(myCPE);
                                                        model.SaveChanges();
                                                        iCPEID = myCPE.CPEID;
                                                    }
                                                    catch (Exception exAddToCPE)
                                                    {
                                                        Console.WriteLine("Exception: exAddToCPE " + exAddToCPE.Message + " " + exAddToCPE.InnerException);
                                                    }
                                                }
                                                else
                                                {
                                                    //Update CPE
                                                }

                                                //XORCISMModel.OVALDEFINITIONCPE myOVALDEFINITIONCPE;
                                                //myOVALDEFINITIONCPE = oval_model.OVALDEFINITIONCPE.FirstOrDefault(o => o.OVALDefinitionID == oOVALDefinition.OVALDefinitionID && o.CPEID == iCPEID);
                                                int iOVALDefinitionCPEID = 0;
                                                try
                                                {
                                                    iOVALDefinitionCPEID = oval_model.OVALDEFINITIONCPE.Where(o => o.OVALDefinitionID == oOVALDefinition.OVALDefinitionID && o.CPEID == iCPEID).Select(o => o.OVALDefinitionCPEID).FirstOrDefault();
                                                }
                                                catch (Exception ex)
                                                {
                                                    iOVALDefinitionCPEID = 0;
                                                }
                                                //if (myOVALDEFINITIONCPE == null)
                                                if (iOVALDefinitionCPEID <= 0)
                                                {
                                                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                    Console.WriteLine("DEBUG Adding new OVALDEFINITIONCPE " + sReferenceSourceID);

                                                    try
                                                    {
                                                        OVALDEFINITIONCPE myOVALDEFINITIONCPE = new OVALDEFINITIONCPE();
                                                        myOVALDEFINITIONCPE.OVALDefinitionID = oOVALDefinition.OVALDefinitionID;
                                                        myOVALDEFINITIONCPE.OVALDefinitionCPERelationship = "related"; //Review?
                                                        myOVALDEFINITIONCPE.CPEID = iCPEID; // myCPE.CPEID;
                                                        myOVALDEFINITIONCPE.VocabularyID = iVocabularyOVALID;
                                                        myOVALDEFINITIONCPE.CreatedDate = DateTimeOffset.Now;
                                                        myOVALDEFINITIONCPE.timestamp = DateTimeOffset.Now;
                                                        oval_model.OVALDEFINITIONCPE.Add(myOVALDEFINITIONCPE);
                                                        //oval_model.SaveChanges();    //TEST PERFORMANCE
                                                        //iOVALDefinitionCPEID=
                                                    }
                                                    catch (Exception exAddToOVALDEFINITIONCPE)
                                                    {
                                                        Console.WriteLine("Exception: exAddToOVALDEFINITIONCPE " + exAddToOVALDEFINITIONCPE.Message + " " + exAddToOVALDEFINITIONCPE.InnerException);
                                                    }
                                                }
                                                else
                                                {
                                                    //Update OVALDEFINITIONCPE
                                                }

                                                #region CPEFORPLATFORM
                                                foreach (int iPlatformID in ListPlatformID)
                                                {
                                                    int iCPEPlatformID = 0;
                                                    try
                                                    {
                                                        iCPEPlatformID = model.CPEFORPLATFORM.Where(o => o.CPEID == iCPEID && o.PlatformID == iPlatformID).Select(o => o.PlatformCPEID).FirstOrDefault();
                                                    }
                                                    catch (Exception ex)
                                                    {

                                                    }
                                                    if (iCPEPlatformID <= 0)
                                                    {
                                                        Console.WriteLine("DEBUG Adding CPEFORPLATFORM");
                                                        CPEFORPLATFORM oCPEPlatform = new CPEFORPLATFORM();
                                                        oCPEPlatform.CreatedDate = DateTimeOffset.Now;
                                                        oCPEPlatform.CPEID = iCPEID;
                                                        oCPEPlatform.PlatformID = iPlatformID;
                                                        oCPEPlatform.VocabularyID = iVocabularyOVALID;
                                                        oCPEPlatform.timestamp = DateTimeOffset.Now;
                                                        model.CPEFORPLATFORM.Add(oCPEPlatform);
                                                        //model.SaveChanges();    //TEST PERFORMANCE
                                                        //iCPEPlatformID=

                                                    }
                                                    else
                                                    {
                                                        //Update CPEFORPLATFORM
                                                    }
                                                }
                                                #endregion CPEFORPLATFORM


                                                #region CPEFORPRODUCT
                                                foreach (int iProductID in ListProductID)
                                                {
                                                    int iCPEProductID = 0;
                                                    try
                                                    {
                                                        iCPEProductID = model.CPEFORPRODUCT.Where(o => o.CPEID == iCPEID && o.ProductID == iProductID).Select(o => o.ProductCPEID).FirstOrDefault();
                                                    }
                                                    catch (Exception ex)
                                                    {

                                                    }
                                                    if (iCPEProductID <= 0)
                                                    {
                                                        Console.WriteLine("DEBUG Adding CPEFORPRODUCT");
                                                        CPEFORPRODUCT oCPEProduct = new CPEFORPRODUCT();
                                                        oCPEProduct.CreatedDate = DateTimeOffset.Now;
                                                        oCPEProduct.CPEID = iCPEID;
                                                        oCPEProduct.ProductID = iProductID;
                                                        oCPEProduct.VocabularyID = iVocabularyOVALID;
                                                        oCPEProduct.timestamp = DateTimeOffset.Now;
                                                        model.CPEFORPRODUCT.Add(oCPEProduct);
                                                        //model.SaveChanges();    //TEST PERFORMANCE
                                                        //iCPEProductID=

                                                    }
                                                    else
                                                    {
                                                        //Update CPEFORPRODUCT
                                                    }
                                                }
                                                #endregion CPEFORPRODUCT


                                                //TODO? VULNERABILITYFORCPE
                                                #endregion ovaldefinitionreferencecpe
                                            }
                                            else
                                            {
                                                #region OVALDEFINITIONREFERENCEANDCCE
                                                //OVALMETADATAREFERENCE
                                                //TODO Normalize
                                                //XORCISMModel.REFERENCE myReference;
                                                //myReference = model.REFERENCE.FirstOrDefault(o => o.Source == sReferenceSource && o.ReferenceSourceID == sReferenceSourceID && o.ReferenceURL == sReferenceUrl);
                                                int iReferenceID = 0;
                                                try
                                                {
                                                    //TODO Review microsoft.com MS
                                                    //http://support.microsoft.com/default.aspx?scid=fh;EN-US;sp
                                                    iReferenceID = model.REFERENCE.Where(o => o.Source == sReferenceSource && o.ReferenceSourceID == sReferenceSourceID && o.ReferenceURL == sReferenceUrl).Select(o => o.ReferenceID).FirstOrDefault();
                                                }
                                                catch (Exception ex)
                                                {
                                                    iReferenceID = 0;
                                                }
                                                //if (myReference == null)
                                                if (iReferenceID <= 0)
                                                {
                                                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                    Console.WriteLine(string.Format("DEBUG Adding new REFERENCE [{0}] in table REFERENCE", sReferenceUrl));

                                                    try
                                                    {
                                                        REFERENCE myReference = new REFERENCE();
                                                        myReference.Source = sReferenceSource;
                                                        //TODO REVIEW
                                                        if (sReferenceSourceID.Length < 51) //Hardcoded
                                                        {
                                                            myReference.ReferenceSourceID = sReferenceSourceID;
                                                        }
                                                        //TODO: ReferenceSource() see Import_all

                                                        //TODO: NORMALIZEREFERENCE() see Import_all
                                                        myReference.ReferenceURL = sReferenceUrl;

                                                        myReference.VocabularyID = iVocabularyOVALID;
                                                        myReference.CreatedDate = DateTimeOffset.Now;
                                                        myReference.timestamp = DateTimeOffset.Now;
                                                        model.REFERENCE.Add(myReference);
                                                        model.SaveChanges();
                                                        iReferenceID = myReference.ReferenceID;
                                                    }
                                                    catch (Exception exAddToREFERENCE)
                                                    {
                                                        Console.WriteLine("Exception: exAddToREFERENCE " + exAddToREFERENCE.Message + " " + exAddToREFERENCE.InnerException);
                                                    }
                                                }
                                                else
                                                {
                                                    //Update REFERENCE

                                                }

                                                //XORCISMModel.OVALDEFINITIONREFERENCE oOVALDefinitionReference;
                                                //oOVALDefinitionReference = oval_model.OVALDEFINITIONREFERENCE.FirstOrDefault(o => o.OVALDefinitionID == oOVALDefinition.OVALDefinitionID && o.ReferenceID == iReferenceID);
                                                int iOVALDefinitionReferenceID = 0;
                                                try
                                                {
                                                    iOVALDefinitionReferenceID = oval_model.OVALDEFINITIONREFERENCE.Where(o => o.OVALDefinitionID == oOVALDefinition.OVALDefinitionID && o.ReferenceID == iReferenceID).Select(o => o.OVALDefinitionReferenceID).FirstOrDefault();
                                                }
                                                catch (Exception ex)
                                                {
                                                    iOVALDefinitionReferenceID = 0;
                                                }
                                                //if (oOVALDefinitionReference == null)
                                                if (iOVALDefinitionReferenceID <= 0)
                                                {
                                                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                    Console.WriteLine(string.Format("DEBUG Adding new OVALDEFINITIONREFERENCE [{0}] in table OVALDEFINITIONREFERENCE", sReferenceUrl));

                                                    try
                                                    {
                                                        OVALDEFINITIONREFERENCE oOVALDefinitionReference = new OVALDEFINITIONREFERENCE();
                                                        oOVALDefinitionReference.OVALDefinitionID = oOVALDefinition.OVALDefinitionID;
                                                        oOVALDefinitionReference.ReferenceID = iReferenceID;    //myReference.ReferenceID;
                                                        oOVALDefinitionReference.VocabularyID = iVocabularyOVALID;
                                                        oOVALDefinitionReference.CreatedDate = DateTimeOffset.Now;
                                                        oOVALDefinitionReference.timestamp = DateTimeOffset.Now;
                                                        oval_model.OVALDEFINITIONREFERENCE.Add(oOVALDefinitionReference);
                                                        oval_model.SaveChanges();    //TEST PERFORMANCE
                                                        //iOVALDefinitionReferenceID = oOVALDefinitionReference.OVALDefinitionReferenceID;
                                                    }
                                                    catch (Exception exAddToOVALDEFINITIONREFERENCE)
                                                    {
                                                        Console.WriteLine("Exception: exAddToOVALDEFINITIONREFERENCE " + exAddToOVALDEFINITIONREFERENCE.Message + " " + exAddToOVALDEFINITIONREFERENCE.InnerException);
                                                    }
                                                }
                                                else
                                                {
                                                    //Update OVALDEFINITIONREFERENCE
                                                }

                                                if (sReferenceSource.ToLower() == "cce")
                                                {
                                                    #region ovaldefinitioncce
                                                    //<reference source="CCE" ref_id="CCE-6224-0" ref_url="http://cce.mitre.org/lists/data/downloads/cce-rhel4-5.20090506.xls"/>
                                                    //
                                                    //CCE
                                                    //OVALMETADATACCE

                                                    //XORCISMModel.CCE myCCE;
                                                    //myCCE = model.CCE.FirstOrDefault(o => o.cce_id == sReferenceSourceID);
                                                    int iCCEID = 0;
                                                    try
                                                    {
                                                        iCCEID = model.CCE.Where(o => o.cce_id == sReferenceSourceID).Select(o => o.CCEID).FirstOrDefault();
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        iCCEID = 0;
                                                    }
                                                    //if (myCCE == null)
                                                    if (iCCEID <= 0)
                                                    {
                                                        Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                        Console.WriteLine("DEBUG Unknown CCE " + sReferenceSourceID);
                                                        Console.WriteLine("DEBUG Adding new CCE");

                                                        try
                                                        {
                                                            CCE myCCE = new CCE();
                                                            myCCE.cce_id = sReferenceSourceID;
                                                            myCCE.VocabularyID = iVocabularyOVALID;
                                                            myCCE.CreatedDate = DateTimeOffset.Now;
                                                            myCCE.timestamp = DateTimeOffset.Now;
                                                            model.CCE.Add(myCCE);
                                                            model.SaveChanges();
                                                            iCCEID = myCCE.CCEID;
                                                        }
                                                        catch (Exception exAddToCCE)
                                                        {
                                                            Console.WriteLine("Exception: exAddToCCE " + exAddToCCE.Message + " " + exAddToCCE.InnerException);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //Update CCE
                                                    }

                                                    //XORCISMModel.OVALDEFINITIONCCE oOVALDefinitionCCE;
                                                    //oOVALDefinitionCCE = oval_model.OVALDEFINITIONCCE.FirstOrDefault(o => o.OVALDefinitionID == oOVALDefinition.OVALDefinitionID && o.CCEID == iCCEID);
                                                    int iOVALDefinitionCCEID = 0;
                                                    try
                                                    {
                                                        iOVALDefinitionCCEID = oval_model.OVALDEFINITIONCCE.Where(o => o.OVALDefinitionID == oOVALDefinition.OVALDefinitionID && o.CCEID == iCCEID).Select(o => o.OVALDefinitionCCEID).FirstOrDefault();
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        iOVALDefinitionCCEID = 0;
                                                    }
                                                    //if (oOVALDefinitionCCE == null)
                                                    if (iOVALDefinitionCCEID <= 0)
                                                    {
                                                        Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                        Console.WriteLine("DEBUG Adding new OVALDEFINITIONCCE");

                                                        try
                                                        {
                                                            OVALDEFINITIONCCE oOVALDefinitionCCE = new OVALDEFINITIONCCE();
                                                            oOVALDefinitionCCE.OVALDefinitionID = oOVALDefinition.OVALDefinitionID;
                                                            oOVALDefinitionCCE.CCEID = iCCEID;  // myCCE.CCEID;
                                                            oOVALDefinitionCCE.VocabularyID = iVocabularyOVALID;
                                                            oOVALDefinitionCCE.CreatedDate = DateTimeOffset.Now;
                                                            oOVALDefinitionCCE.timestamp = DateTimeOffset.Now;
                                                            oval_model.OVALDEFINITIONCCE.Add(oOVALDefinitionCCE);
                                                            oval_model.SaveChanges();    //TEST PERFORMANCE
                                                            //iOVALDefinitionCCEID=
                                                        }
                                                        catch (Exception exAddToOVALDEFINITIONCCE)
                                                        {
                                                            Console.WriteLine("Exception: exAddToOVALDEFINITIONCCE " + exAddToOVALDEFINITIONCCE.Message + " " + exAddToOVALDEFINITIONCCE.InnerException);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //Update OVALDEFINITIONCCE
                                                    }

                                                    //TODO: CCEFORCPE
                                                    //CCEREFERENCE?
                                                    //TODO? CCEFORPLATFORM  CCEFORPRODUCT
                                                    #endregion ovaldefinitioncce
                                                }
                                                #endregion OVALDEFINITIONREFERENCEANDCCE
                                            }
                                        }
                                        #endregion ovaldefinitionreference
                                        break;
                                    case "title":
                                        //done before
                                        break;
                                    case "description":
                                        //done before
                                        break;
                                    case "oval_repository":
                                        #region ovalrepository
                                        foreach (XmlNode nodeOvalRepo in nodeMetadata.ChildNodes)
                                        {
                                            switch (nodeOvalRepo.Name)
                                            {
                                                //dates

                                                case "status":
                                                    //Update OVALDEFINITION
                                                    Console.WriteLine("DEBUG Status=" + nodeOvalRepo.InnerText);
                                                    oOVALDefinition.StatusName = nodeOvalRepo.InnerText;    //ACCEPTED
                                                    try
                                                    {
                                                        oOVALDefinition.timestamp = DateTimeOffset.Now;
                                                        oval_model.OVALDEFINITION.Attach(oOVALDefinition);
                                                        oval_model.Entry(oOVALDefinition).State = EntityState.Modified;
                                                        oval_model.SaveChanges();
                                                    }
                                                    catch (Exception exOVALDEFINITIONStatus)
                                                    {
                                                        Console.WriteLine("Exception: exOVALDEFINITIONStatus " + exOVALDEFINITIONStatus.Message + " " + exOVALDEFINITIONStatus.InnerException);
                                                    }
                                                    break;


                                                case "affected_cpe_list":
                                                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                    Console.WriteLine("DEBUG affected_cpe_list");
                                                    #region affectedcpelist
                                                    foreach (XmlNode nodeOvalRepoCPE in nodeOvalRepo.ChildNodes)
                                                    {
                                                        if (nodeOvalRepoCPE.Name == "cpe")
                                                        {
                                                            string sCPEName = nodeOvalRepoCPE.InnerText;
                                                            sCPEName = sCPEName.Replace("cpe:///", "cpe:/");
                                                            sCPEName = sCPEName.Replace("cpe://", "cpe:/");
                                                            int iCPEID = 0;
                                                            try
                                                            {
                                                                iCPEID = model.CPE.Where(o => o.CPEName == sCPEName).Select(o => o.CPEID).FirstOrDefault();
                                                            }
                                                            catch (Exception ex)
                                                            {

                                                            }
                                                            if (iCPEID <= 0)
                                                            {
                                                                Console.WriteLine("DEBUG Adding CPE " + sCPEName);
                                                                CPE oCPE = new CPE();
                                                                oCPE.CreatedDate = DateTimeOffset.Now;
                                                                oCPE.CPEName = sCPEName;
                                                                oCPE.VocabularyID = iVocabularyOVALID;
                                                                oCPE.timestamp = DateTimeOffset.Now;
                                                                model.CPE.Add(oCPE);
                                                                model.SaveChanges();
                                                                iCPEID = oCPE.CPEID;
                                                            }
                                                            else
                                                            {
                                                                //Update CPE
                                                            }

                                                            int iOVALDEFINITIONCPEID = 0;
                                                            try
                                                            {
                                                                iOVALDEFINITIONCPEID = oval_model.OVALDEFINITIONCPE.Where(o => o.OVALDefinitionID == oOVALDefinition.OVALDefinitionID && o.CPEID == iCPEID).Select(o => o.OVALDefinitionCPEID).FirstOrDefault();
                                                            }
                                                            catch (Exception ex)
                                                            {

                                                            }
                                                            if (iOVALDEFINITIONCPEID <= 0)
                                                            {
                                                                Console.WriteLine("DEBUG Adding OVALDEFINITIONCPE");
                                                                OVALDEFINITIONCPE oOVALDefCPE = new OVALDEFINITIONCPE();
                                                                oOVALDefCPE.CreatedDate = DateTimeOffset.Now;
                                                                oOVALDefCPE.OVALDefinitionID = oOVALDefinition.OVALDefinitionID;
                                                                oOVALDefCPE.OVALDefinitionCPERelationship = "affected"; //Hardcoded
                                                                oOVALDefCPE.CPEID = iCPEID;
                                                                oOVALDefCPE.VocabularyID = iVocabularyOVALID;
                                                                oOVALDefCPE.timestamp = DateTimeOffset.Now;
                                                                oval_model.OVALDEFINITIONCPE.Add(oOVALDefCPE);
                                                                //oval_model.SaveChanges();    //TEST PERFORMANCE
                                                                //iOVALDEFINITIONCPEID=
                                                            }
                                                            else
                                                            {
                                                                //Update OVALDEFINITIONCPE
                                                            }
                                                        }
                                                        else
                                                        {
                                                            Console.WriteLine("ERROR: Missing code for nodeOvalRepoCPE.Name" + nodeOvalRepoCPE.Name);
                                                        }
                                                    }
                                                    #endregion affectedcpelist
                                                    break;

                                                case "dates":
                                                    #region OVALDEFINITIONCHANGE
                                                    //OVALDEFINITIONCHANGES
                                                    foreach (XmlNode nodeOvalDate in nodeOvalRepo.ChildNodes)
                                                    {
                                                        switch (nodeOvalDate.Name)
                                                        {
                                                            case "submitted":
                                                            case "modified":
                                                            case "status_change":
                                                                //<submitted date="2010-07-09T03:56:16-04:00">
                                                                //<modified comment="EDITED oval:org.mitre.oval:def:9999 - Expanded the vulnerability checks for RHEL 3, 4, and 5 to cover  CentOS 3, 4, 5 and Oracle Linux 4 and 5" date="2013-04-10T17:31:00.062-04:00">
                                                                //<status_change date="2010-07-28T14:22:35.831-04:00">DRAFT</status_change>
                                                                string sChangeTypeName = nodeOvalDate.Name; //submitted modified    status_change
                                                                string sChangeValue = "";
                                                                if (nodeOvalDate.Name == "status_change")
                                                                {
                                                                    sChangeValue = nodeOvalDate.InnerText;  //INTERIM
                                                                }

                                                                int iOVALDefChangeID = 0;
                                                                DateTimeOffset dChangeDate = DateTimeOffset.Now;
                                                                try
                                                                {
                                                                    //Review convert?
                                                                    //2010-07-28T14:22:35.831-04:00
                                                                    dChangeDate = DateTimeOffset.Parse(nodeOvalDate.Attributes["date"].Value, new System.Globalization.CultureInfo("EN-us"));
                                                                    iOVALDefChangeID = oval_model.OVALDEFINITIONCHANGE.Where(o => o.ChangeTypeName == sChangeTypeName && o.ChangeDate == dChangeDate && o.ChangeValue == sChangeValue).Select(o => o.OVALDefinitionChangeID).FirstOrDefault();
                                                                }
                                                                catch (Exception ex)
                                                                {

                                                                }
                                                                if (iOVALDefChangeID <= 0)
                                                                {
                                                                    OVALDEFINITIONCHANGE oOVALDefChange = new OVALDEFINITIONCHANGE();
                                                                    oOVALDefChange.CreatedDate = DateTimeOffset.Now;
                                                                    oOVALDefChange.ChangeTypeName = sChangeTypeName; //submitted modified    status_change
                                                                    oOVALDefChange.ChangeValue = sChangeValue;
                                                                    oOVALDefChange.ChangeDate = dChangeDate;
                                                                    try
                                                                    {
                                                                        oOVALDefChange.ChangeComment = nodeOvalDate.Attributes["comment"].InnerText;
                                                                    }
                                                                    catch (Exception ex)
                                                                    {

                                                                    }
                                                                    //TODO Other Attributes?

                                                                    oOVALDefChange.VocabularyID = iVocabularyOVALID;
                                                                    oOVALDefChange.timestamp = DateTimeOffset.Now;
                                                                    try
                                                                    {
                                                                        oval_model.OVALDEFINITIONCHANGE.Add(oOVALDefChange);
                                                                        oval_model.SaveChanges();    //needed for CHANGES
                                                                        iOVALDefChangeID = oOVALDefChange.OVALDefinitionChangeID;
                                                                    }
                                                                    catch (Exception exOVALDEFINITIONCHANGE)
                                                                    {
                                                                        Console.WriteLine("Exception: exOVALDEFINITIONCHANGE " + exOVALDEFINITIONCHANGE.Message + " " + exOVALDEFINITIONCHANGE.InnerException);
                                                                    }
                                                                    iOVALDefChangeID = oOVALDefChange.OVALDefinitionChangeID;
                                                                }
                                                                else
                                                                {
                                                                    //Update OVALDEFINITIONCHANGE
                                                                }

                                                                foreach (XmlNode nodeOvalSubmittedContributor in nodeOvalDate.ChildNodes)
                                                                {
                                                                    //<contributor organization="SCAP.com, LLC">Aharon Chernin</contributor>
                                                                    switch (nodeOvalSubmittedContributor.Name)
                                                                    {
                                                                        case "contributor":
                                                                            //TODO Get all the attributes
                                                                            int iOrganisationID = 0;
                                                                            int iPersonID = 0;
                                                                            #region organisation
                                                                            try
                                                                            {
                                                                                string sOrganisationName = nodeOvalSubmittedContributor.Attributes["organization"].Value;

                                                                                try
                                                                                {
                                                                                    iOrganisationID = model.ORGANISATION.Where(o => o.OrganisationName == sOrganisationName).Select(o => o.OrganisationID).FirstOrDefault();
                                                                                }
                                                                                catch (Exception ex)
                                                                                {

                                                                                }
                                                                                if (iOrganisationID <= 0)
                                                                                {
                                                                                    try
                                                                                    {
                                                                                        ORGANISATION oOrganisation = new ORGANISATION();
                                                                                        oOrganisation.CreatedDate = DateTimeOffset.Now;
                                                                                        oOrganisation.OrganisationName = sOrganisationName;
                                                                                        oOrganisation.CountryID = 2;    //TODO REMOVE
                                                                                        oOrganisation.VocabularyID = iVocabularyOVALID;
                                                                                        oOrganisation.timestamp = DateTimeOffset.Now;
                                                                                        model.ORGANISATION.Add(oOrganisation);
                                                                                        model.SaveChanges();
                                                                                        iOrganisationID = oOrganisation.OrganisationID;
                                                                                    }
                                                                                    catch (Exception exORGANISATION)
                                                                                    {
                                                                                        Console.WriteLine("Exception: exORGANISATION " + exORGANISATION.Message + " " + exORGANISATION.InnerException);
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    //Update ORGANISATION
                                                                                }

                                                                            }
                                                                            catch (Exception ex)
                                                                            {
                                                                                Console.WriteLine("NOTE: No Organization");

                                                                            }
                                                                            #endregion organisation

                                                                            #region person
                                                                            try
                                                                            {
                                                                                string sPersonName = nodeOvalSubmittedContributor.InnerText;
                                                                                try
                                                                                {
                                                                                    //TODO REVIEW, ENHANCE
                                                                                    iPersonID = model.PERSON.Where(o => o.FullName == sPersonName).Select(o => o.PersonID).FirstOrDefault();
                                                                                }
                                                                                catch (Exception ex)
                                                                                {

                                                                                }
                                                                                if (iPersonID <= 0)
                                                                                {
                                                                                    PERSON oPerson = new PERSON();
                                                                                    oPerson.CreatedDate = DateTimeOffset.Now;
                                                                                    oPerson.FullName = sPersonName;
                                                                                    //TODO FirstName, etc.
                                                                                    oPerson.VocabularyID = iVocabularyOVALID;
                                                                                    oPerson.timestamp = DateTimeOffset.Now;
                                                                                    model.PERSON.Add(oPerson);
                                                                                    model.SaveChanges();
                                                                                    iPersonID = oPerson.PersonID;
                                                                                }
                                                                                else
                                                                                {
                                                                                    //Update PERSON
                                                                                }
                                                                            }
                                                                            catch (Exception ex)
                                                                            {

                                                                            }
                                                                            #endregion person

                                                                            //TODO AUTHOR?

                                                                            #region PERSONFORORGANISATION
                                                                            int iOrganisationPersonID = 0;
                                                                            try
                                                                            {
                                                                                iOrganisationPersonID = model.PERSONFORORGANISATION.Where(o => o.OrganisationID == iOrganisationID && o.PersonID == iPersonID).Select(o => o.PersonOrganisationID).FirstOrDefault();
                                                                                if (iOrganisationPersonID <= 0)
                                                                                {
                                                                                    try
                                                                                    {
                                                                                        PERSONFORORGANISATION oOrganisationPerson = new PERSONFORORGANISATION();
                                                                                        oOrganisationPerson.CreatedDate = DateTimeOffset.Now;
                                                                                        oOrganisationPerson.OrganisationID = iOrganisationID;
                                                                                        oOrganisationPerson.relationshiptype = "works"; //TODO REVIEW Hardcoded
                                                                                        oOrganisationPerson.PersonID = iPersonID;
                                                                                        oOrganisationPerson.VocabularyID = iVocabularyOVALID;
                                                                                        oOrganisationPerson.timestamp = DateTimeOffset.Now;
                                                                                        model.PERSONFORORGANISATION.Add(oOrganisationPerson);
                                                                                        model.SaveChanges();    //TEST PERFORMANCE
                                                                                        //iOrganisationPersonID=
                                                                                    }
                                                                                    catch (Exception exPERSONFORORGANISATION)
                                                                                    {
                                                                                        Console.WriteLine("Exception: exPERSONFORORGANISATION " + exPERSONFORORGANISATION.Message + " " + exPERSONFORORGANISATION.InnerException);
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    //Update PERSONFORORGANISATION
                                                                                }
                                                                            }
                                                                            catch (Exception ex)
                                                                            {

                                                                            }
                                                                            #endregion PERSONFORORGANISATION

                                                                            #region OVALDEFINITIONCHANGES
                                                                            int iOVALDefChangesID = 0;
                                                                            try
                                                                            {
                                                                                //We assume that we found both OrganisationID and PersonID
                                                                                iOVALDefChangesID = oval_model.OVALDEFINITIONCHANGES.Where(o => o.OVALDefinitionID == oOVALDefinition.OVALDefinitionID && o.OVALDefinitionChangeID == iOVALDefChangesID && o.OrganisationID == iOrganisationID && o.PersonID == o.PersonID).Select(o => o.OVALDefinitionChangesID).FirstOrDefault();
                                                                                if (iOVALDefChangesID <= 0)
                                                                                {
                                                                                    OVALDEFINITIONCHANGES oOVALDefChanges = new OVALDEFINITIONCHANGES();
                                                                                    oOVALDefChanges.CreatedDate = DateTimeOffset.Now;
                                                                                    oOVALDefChanges.OVALDefinitionID = oOVALDefinition.OVALDefinitionID;
                                                                                    oOVALDefChanges.OVALDefinitionChangeID = iOVALDefChangeID;
                                                                                    oOVALDefChanges.OrganisationID = iOrganisationID;
                                                                                    oOVALDefChanges.PersonID = iPersonID;
                                                                                    //oOVALDefChanges.AuthorID=   //TODO?
                                                                                    oOVALDefChanges.VocabularyID = iVocabularyOVALID;
                                                                                    oOVALDefChanges.timestamp = DateTimeOffset.Now;
                                                                                    oval_model.OVALDEFINITIONCHANGES.Add(oOVALDefChanges);
                                                                                    oval_model.SaveChanges();    //TEST PERFORMANCE
                                                                                    //iOVALDefChangesID=
                                                                                }
                                                                                else
                                                                                {
                                                                                    //Update OVALDEFINITIONCHANGES
                                                                                }
                                                                            }
                                                                            catch (Exception ex)
                                                                            {

                                                                            }
                                                                            #endregion OVALDEFINITIONCHANGES
                                                                            break;

                                                                        default:
                                                                            if (nodeOvalSubmittedContributor.Name == "#text")
                                                                            {
                                                                                //Review
                                                                                //No DTD? whitespace-only text nodes?
                                                                                //https://stackoverflow.com/questions/12817018/getnodename-operation-on-an-xml-node-returns-text
                                                                            }
                                                                            else
                                                                            {
                                                                                Console.WriteLine("TODO: Missing code for nodeOvalSubmittedContributor.Name=" + nodeOvalSubmittedContributor.Name);
                                                                            }
                                                                            break;
                                                                    }
                                                                }
                                                                break;

                                                            default:
                                                                Console.WriteLine("TODO: Missing code for nodeOvalDate.Name=" + nodeOvalDate.Name);

                                                                break;
                                                        }
                                                    }
                                                    #endregion OVALDEFINITIONCHANGE
                                                    break;
                                                
                                                case "min_schema_version":
                                                    //Update OVALDEFINITION
                                                    Console.WriteLine("DEBUG min_schema_version=" + nodeOvalRepo.InnerText);
                                                    oOVALDefinition.min_schema_version = nodeOvalRepo.InnerText;    //5.4
                                                    try
                                                    {
                                                        oOVALDefinition.timestamp = DateTimeOffset.Now;
                                                        oval_model.OVALDEFINITION.Attach(oOVALDefinition);
                                                        oval_model.Entry(oOVALDefinition).State = EntityState.Modified;
                                                        oval_model.SaveChanges();
                                                    }
                                                    catch (Exception exOVALDEFINITIONmin_schema_version)
                                                    {
                                                        Console.WriteLine("Exception: exOVALDEFINITIONmin_schema_version " + exOVALDEFINITIONmin_schema_version.Message + " " + exOVALDEFINITIONmin_schema_version.InnerException);
                                                    }
                                                    break;
                                                
                                                default:
                                                    Console.WriteLine("TODO: Missing code for nodeOvalRepo.Name=" + nodeOvalRepo.Name);
                                                    
                                                    break;
                                            }
                                        }
                                        #endregion ovalrepository
                                        break;
                                    default:
                                        Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                        Console.WriteLine("ERROR: Missing code for nodeMetadata.Name=" + nodeMetadata.Name);

                                        break;
                                }
                            }
                        }
                    }

                    /*
                    if (bAddDefinition)
                    {
                        Console.WriteLine(string.Format("DEBUG Adding new OVALDEFINITION [{0}] in table OVALDEFINITION", myDefID));
                        myOVALDefinition = new OVALDEFINITION();
                        myOVALDefinition.CreatedDate = DateTimeOffset.Now;
                        //myOVALDefinition.VocabularyID = iVocabularyOVALID;
                    }
                    myOVALDefinition.OVALDefinitionIDPattern = myDefID;
                    myOVALDefinition.OVALDefinitionVersion = iDefVersion;
                    myOVALDefinition.OVALClassEnumerationID = iOVALClassEnumerationID;  // myclass.OVALClassEnumerationID;
                    myOVALDefinition.ClassValue = sClassValue;
                    if (sDefDeprecated.ToLower() == "true")
                    {
                        myOVALDefinition.deprecated = true;
                    }
                    else
                    {
                        myOVALDefinition.deprecated = false;    //default
                    }
                    myOVALDefinition.OVALMetadataID = myOVALMetadata.OVALMetadataID;
                    //myOVALDefinition.notes = "";
                    myOVALDefinition.OVALCriteriaID = myCriteria.OVALCriteriaID;
                    //myOVALDefinition.signature = "";
                    try
                    {
                        if (bAddDefinition)
                        {
                            myOVALDefinition.VocabularyID = iVocabularyOVALID;
                            model.OVALDEFINITION.Add(myOVALDefinition);
                        }
                        myOVALDefinition.timestamp = DateTimeOffset.Now;
                        model.SaveChanges();
                    }
                    catch (Exception exAddToOVALDEFINITION)
                    {
                        Console.WriteLine("Exception: exAddToOVALDEFINITION " + exAddToOVALDEFINITION.Message + " " + exAddToOVALDEFINITION.InnerException);
                    }
                    */

                    //}
                    //else
                    //{
                    //    //The OVALDEFINITION already exists
                    //    //TODO
                    //}



                }
                #endregion ovaldefinition
            }

            if (bImportOVALTests)
            {
                #region ovaltests
                #region freememory
                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                Console.WriteLine("DEBUG FREE MEMORY");
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
                    Console.WriteLine("Exception: DbEntityValidationExceptionGLOBALSAVE " + sb.ToString());
                }
                catch (Exception exGLOBALSAVE)
                {
                    Console.WriteLine("Exception: exGLOBALSAVE " + exGLOBALSAVE.Message + " " + exGLOBALSAVE.InnerException);
                }
                model.Dispose();

                model = new XORCISMEntities();
                model.Configuration.AutoDetectChangesEnabled = false;
                model.Configuration.ValidateOnSaveEnabled = false;
                #endregion freememory

                XmlNodeList nodesTests = doc.SelectNodes("/oval-def:oval_definitions/oval-def:tests", mgr); //Hardcoded
                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                Console.WriteLine("IMPORTING OVALTESTS");
                foreach (XmlNode nodesTest in nodesTests)
                {
                    foreach (XmlNode nodeINTEST in nodesTest)
                    {
                        //if (nodeINTEST.Name == "version_test" || nodeINTEST.Name == "line_test")
                        //{
                        string sTestID = nodeINTEST.Attributes["id"].InnerText; //oval:org.mitre.oval:tst:42478

                        string sTestVersion = nodeINTEST.Attributes["version"].InnerText;   //0
                        XOVALModel.OVALTEST myOVALTest;
                        int iTestVersion = Convert.ToInt32(sTestVersion);
                        myOVALTest = oval_model.OVALTEST.FirstOrDefault(o => o.OVALTestIDPattern == sTestID && o.OVALTestVersion == iTestVersion);
                        Boolean bAddOVALTEST = false;
                        if (myOVALTest == null)
                        {
                            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                            Console.WriteLine("DEBUG Adding OVALTEST" + " " + sTestID);
                            myOVALTest = new OVALTEST();
                            myOVALTest.OVALTestIDPattern = sTestID; //oval:org.mitre.oval:tst:42478
                            myOVALTest.OVALTestVersion = iTestVersion;
                            myOVALTest.CreatedDate = DateTimeOffset.Now;
                            myOVALTest.VocabularyID = iVocabularyOVALID;
                            //myOVALTest.EnumerationValue = "";
                            myOVALTest.comment = "";
                            bAddOVALTEST = true;
                        }
                        else
                        {
                            //Update OVATEST
                        }


                        //myOVALTest.DataTypeName = nodeINTEST.Name;      //Removed
                        //version_test  registry_test   file_test
                        Console.WriteLine("DEBUG DataTypeName=" + nodeINTEST.Name);
                        #region OVALTESTDATATYPE
                        int iOVALTestDataTypeID = 0;
                        try
                        {
                            iOVALTestDataTypeID = oval_model.OVALTESTDATATYPE.Where(o => o.OVALTestDataTypeName == nodeINTEST.Name).Select(o => o.OVALTestDataTypeID).FirstOrDefault();
                        }
                        catch (Exception ex)
                        {

                        }
                        if (iOVALTestDataTypeID <= 0)
                        {
                            OVALTESTDATATYPE oOVALTestDataType = new OVALTESTDATATYPE();
                            oOVALTestDataType.CreatedDate = DateTimeOffset.Now;
                            oOVALTestDataType.OVALTestDataTypeName = nodeINTEST.Name;
                            oOVALTestDataType.VocabularyID = iVocabularyOVALID;
                            oOVALTestDataType.timestamp = DateTimeOffset.Now;
                            oval_model.OVALTESTDATATYPE.Add(oOVALTestDataType);
                            oval_model.SaveChanges();
                            iOVALTestDataTypeID = oOVALTestDataType.OVALTestDataTypeID;
                        }
                        else
                        {
                            //Update OVALTESTDATATYPE
                        }
                        #endregion OVALTESTDATATYPE
                        myOVALTest.OVALTestDataTypeID = iOVALTestDataTypeID;

                        string sExistence = "";
                        try
                        {
                            sExistence = nodeINTEST.Attributes["check_existence"].InnerText; //at_least_one_exists
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("DEBUG NOTE: no check_existence, using at_least_one_exists");
                            sExistence = "at_least_one_exists"; //Hardcoded default
                        }

                        try
                        {
                            //Note: potentially could be hardcoded for performance
                            #region existenceenumeration
                            int iExistenceEnumerationID = 0;
                            try
                            {
                                iExistenceEnumerationID = model.EXISTENCEENUMERATION.Where(o => o.ExistenceValue == sExistence).Select(o => o.ExistenceEnumerationID).FirstOrDefault();
                            }
                            catch (Exception ex)
                            {

                            }
                            //XORCISMModel.EXISTENCEENUMERATION myExistence;
                            //myExistence = model.EXISTENCEENUMERATION.FirstOrDefault(o => o.ExistenceValue == sExistence);   //&& o.VocabularyID == iVocabularyOVALID
                            //if (myExistence == null)
                            if (iExistenceEnumerationID <= 0)
                            {
                                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                Console.WriteLine("DEBUG Adding EXISTENCEENUMERATION " + sExistence);
                                EXISTENCEENUMERATION myExistence = new EXISTENCEENUMERATION();
                                myExistence.ExistenceValue = sExistence;
                                myExistence.VocabularyID = iVocabularyOVALID;
                                myExistence.CreatedDate = DateTimeOffset.Now;
                                myExistence.timestamp = DateTimeOffset.Now;
                                try
                                {
                                    model.EXISTENCEENUMERATION.Add(myExistence);
                                    model.SaveChanges();
                                    iExistenceEnumerationID = myExistence.ExistenceEnumerationID;
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
                                    Console.WriteLine("Exception: DbEntityValidationExceptionREFERENCE " + sb.ToString());
                                }
                                catch (Exception exAddToEXISTENCEENUMERATION)
                                {
                                    Console.WriteLine("Exception: exAddToEXISTENCEENUMERATION " + exAddToEXISTENCEENUMERATION.Message + " " + exAddToEXISTENCEENUMERATION.InnerException);
                                }
                            }
                            else
                            {
                                //Update EXISTENCEENUMERATION
                            }
                            #endregion existenceenumeration

                            myOVALTest.ExistenceEnumerationID = iExistenceEnumerationID;    // myExistence.ExistenceEnumerationID;
                            //myOVALTest.ExistenceValue = sExistence;// Removed
                        }
                        catch (Exception exOVALTestCheckExistence)
                        {
                            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                            Console.WriteLine("Exception: exOVALTestCheckExistence " + exOVALTestCheckExistence.Message + " " + exOVALTestCheckExistence.InnerException);
                        }

                        string sCheck = "at_least_one_exists"; //Hardcoded Default
                        try
                        {
                            sCheck = nodeINTEST.Attributes["check"].InnerText;   //all   at least one
                        }
                        catch (Exception ex)
                        {
                            sCheck = "at_least_one_exists"; //Hardcoded Default
                        }
                        try
                        {

                            //Note: could be hardcoded for performance
                            #region checkenumeration
                            int iCheckEnumerationID = 0;
                            try
                            {
                                iCheckEnumerationID = model.CHECKENUMERATION.Where(o => o.EnumerationValue == sCheck).Select(o => o.CheckEnumerationID).FirstOrDefault();
                            }
                            catch (Exception ex)
                            {

                            }
                            //XORCISMModel.CHECKENUMERATION myCheck;
                            //myCheck = model.CHECKENUMERATION.FirstOrDefault(o => o.EnumerationValue == sCheck);   //&& o.VocabularyID == iVocabularyOVALID
                            //if (myCheck == null)
                            if (iCheckEnumerationID <= 0)
                            {
                                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                Console.WriteLine("DEBUG Adding CHECKENUMERATION " + sCheck);
                                CHECKENUMERATION myCheck = new CHECKENUMERATION();
                                myCheck.EnumerationValue = sCheck;  //all   at least one
                                myCheck.VocabularyID = iVocabularyOVALID;
                                myCheck.CreatedDate = DateTimeOffset.Now;
                                myCheck.timestamp = DateTimeOffset.Now;
                                try
                                {
                                    model.CHECKENUMERATION.Add(myCheck);
                                    model.SaveChanges();
                                    iCheckEnumerationID = myCheck.CheckEnumerationID;
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
                                    Console.WriteLine("Exception: DbEntityValidationExceptionREFERENCE " + sb.ToString());
                                }
                                catch (Exception exAddToCHECKENUMERATION)
                                {
                                    Console.WriteLine("Exception: exAddToCHECKENUMERATION " + exAddToCHECKENUMERATION.Message + " " + exAddToCHECKENUMERATION.InnerException);
                                }
                            }
                            else
                            {
                                //Update CHECKENUMERATION
                            }
                            #endregion checkenumeration

                            myOVALTest.CheckEnumerationID = iCheckEnumerationID;    // myCheck.CheckEnumerationID;
                            //myOVALTest.EnumerationValue = sCheck;   // Removed
                        }
                        catch (Exception exOVALTestCheck)
                        {
                            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                            Console.WriteLine("ERROR: Exception exOVALTestCheck " + exOVALTestCheck.Message + " " + exOVALTestCheck.InnerException);
                        }

                        #region operatorenumeration
                        int iOperatorEnumerationID = 0;
                        string sOperatorValue = "AND";  //Default   Remove  //Hardcoded
                        try
                        {
                            //Review   state_operator (see OVAL documentation)
                            //string sOperatorValue= nodeINTEST.Attributes["operator"].InnerText;
                            sOperatorValue = nodeINTEST.Attributes["state_operator"].InnerText;

                            try
                            {
                                iOperatorEnumerationID = oval_model.OPERATORENUMERATION.Where(o => o.OperatorValue == sOperatorValue).Select(o => o.OperatorEnumerationID).FirstOrDefault();
                            }
                            catch (Exception ex)
                            {
                                iOperatorEnumerationID = 0;
                            }
                            if (iOperatorEnumerationID <= 0)
                            {
                                XOVALModel.OPERATORENUMERATION oOperatorEnumeration = new XOVALModel.OPERATORENUMERATION();
                                oOperatorEnumeration.CreatedDate = DateTimeOffset.Now;
                                oOperatorEnumeration.OperatorValue = sOperatorValue;
                                oOperatorEnumeration.VocabularyID = iVocabularyOVALID;
                                oOperatorEnumeration.timestamp = DateTimeOffset.Now;
                                oval_model.OPERATORENUMERATION.Add(oOperatorEnumeration);
                                oval_model.SaveChanges();
                                iOperatorEnumerationID = oOperatorEnumeration.OperatorEnumerationID;
                            }
                            else
                            {
                                //Update OPERATORENUMERATION
                            }

                        }
                        catch (Exception exOPERATORENUMERATION1)
                        {
                            //TODO: Normal? when
                            //DEBUG sNameSpace=esx-def
                            //DEBUG DataTypeName = patch_test   patch53_test    patch56_test
                        //    Console.WriteLine("ERROR: Exception exOPERATORENUMERATION1 " + exOPERATORENUMERATION1.Message + " " + exOPERATORENUMERATION1.InnerException);
                        }

                        if (iOperatorEnumerationID <= 0)
                        {
                            //Default AND
                            try
                            {
                                iOperatorEnumerationID = oval_model.OPERATORENUMERATION.Where(o => o.OperatorValue == "AND").Select(o => o.OperatorEnumerationID).FirstOrDefault();
                            }
                            catch (Exception exOPERATORENUMERATION2)
                            {
                                Console.WriteLine("ERROR: Exception exOPERATORENUMERATION2 " + exOPERATORENUMERATION2.Message + " " + exOPERATORENUMERATION2.InnerException);
                                iOperatorEnumerationID = 0;
                            }
                            if (iOperatorEnumerationID <= 0)
                            {
                                try
                                {
                                    XOVALModel.OPERATORENUMERATION oOperatorEnumeration = new XOVALModel.OPERATORENUMERATION();
                                    oOperatorEnumeration.CreatedDate = DateTimeOffset.Now;
                                    oOperatorEnumeration.OperatorValue = "AND";
                                    oOperatorEnumeration.VocabularyID = iVocabularyOVALID;
                                    oOperatorEnumeration.timestamp = DateTimeOffset.Now;
                                    oval_model.OPERATORENUMERATION.Add(oOperatorEnumeration);
                                    oval_model.SaveChanges();
                                    iOperatorEnumerationID = oOperatorEnumeration.OperatorEnumerationID;
                                }
                                catch (Exception exOPERATORENUMERATION3)
                                {
                                    Console.WriteLine("ERROR: Exception exOPERATORENUMERATION3 " + exOPERATORENUMERATION3.Message + " " + exOPERATORENUMERATION3.InnerException);
                                }
                            }
                            myOVALTest.OperatorEnumerationID = iOperatorEnumerationID;  //AND
                        }
                        else
                        {
                            myOVALTest.OperatorEnumerationID = iOperatorEnumerationID;
                        }

                        #endregion operatorenumeration

                        try
                        {
                            myOVALTest.comment = nodeINTEST.Attributes["comment"].InnerText;    //Check if Norton Internet Security is installed
                            //TODO: check for "typos"
                        }
                        catch (Exception exOVALTestComment)
                        {
                            Console.WriteLine("ERROR? Exception exOVALTestComment " + exOVALTestComment.Message + " " + exOVALTestComment.InnerException);
                        }

                        try
                        {
                            string sTestDeprecated = nodeINTEST.Attributes["deprecated"].InnerText;
                            if (sTestDeprecated.ToLower() == "true")
                            {
                                myOVALTest.deprecated = true;
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                        //myOVALTest.notes
                        //myOVALTest.signature

                        //TODO: potentialy could be hardcoded for performance
                        #region ovaltestnamespace
                        string sNameSpace = "";
                        string sXMLNS = "";
                        try
                        {
                            sXMLNS = nodeINTEST.Attributes["xmlns"].InnerText;
                            //TODO: do it more generic
                            if (sXMLNS.Contains("#aix")) sNameSpace = "aix-def";
                            if (sXMLNS.Contains("#apache")) sNameSpace = "apache-def";
                            if (sXMLNS.Contains("#catos")) sNameSpace = "catos-def";
                            if (sXMLNS.Contains("#esx")) sNameSpace = "esx-def";
                            if (sXMLNS.Contains("#freebsd")) sNameSpace = "freebsd-def";
                            if (sXMLNS.Contains("#hpux")) sNameSpace = "hpux-def";
                            if (sXMLNS.Contains("#independent")) sNameSpace = "ind-def";
                            if (sXMLNS.Contains("#ios")) sNameSpace = "ios-def";
                            if (sXMLNS.Contains("#linux")) sNameSpace = "linux-def";
                            if (sXMLNS.Contains("#macos")) sNameSpace = "macos-def";
                            if (sXMLNS.Contains("#pixos")) sNameSpace = "pixos-def";
                            if (sXMLNS.Contains("#sharepoint")) sNameSpace = "sp-def";
                            if (sXMLNS.Contains("#solaris")) sNameSpace = "sol-def";
                            if (sXMLNS.Contains("#unix")) sNameSpace = "unix-def";
                            if (sXMLNS.Contains("#windows")) sNameSpace = "win-def";
                        }
                        catch(Exception exXMLNS)
                        {
                            Console.WriteLine("Exception exXMLNS nodeINTEST="+ nodeINTEST.InnerText+" " + exXMLNS.Message);
                        }
                        //TODO: mapping with other platforms vocabularies (i.e.: MSF/CCE)
                        //PLATFORMMAPPING?
                        if (sNameSpace == "")
                        {
                            Console.WriteLine("ERROR: Missing code Unknown OVALTEST Namespace: " + sXMLNS);
                        }
                        else
                        {
                            Console.WriteLine("DEBUG sNameSpace=" + sNameSpace);
                        }
                        //myOVALTest.Namespace = sNameSpace;  //Removed
                        // OVALNAMESPACE
                        int iOVALNamespaceID = 0;
                        try
                        {
                            iOVALNamespaceID = oval_model.OVALNAMESPACE.Where(o => o.OVALNamespaceName == sNameSpace).Select(o => o.OVALNamespaceID).FirstOrDefault();
                        }
                        catch (Exception ex)
                        {

                        }
                        if (iOVALNamespaceID <= 0)
                        {
                            OVALNAMESPACE oOVALNamespace = new OVALNAMESPACE();
                            oOVALNamespace.CreatedDate = DateTimeOffset.Now;
                            oOVALNamespace.OVALNamespaceName = sNameSpace;
                            oOVALNamespace.VocabularyID = iVocabularyOVALID;
                            oOVALNamespace.timestamp = DateTimeOffset.Now;
                            oval_model.OVALNAMESPACE.Add(oOVALNamespace);
                            oval_model.SaveChanges();
                            iOVALNamespaceID = oOVALNamespace.OVALNamespaceID;
                        }
                        else
                        {
                            //Update OVALNAMESPACE
                        }
                        #endregion ovaltestnamespace
                        myOVALTest.OVALNamespaceID = iOVALNamespaceID;

                        //myOVALTest.DataTypeName = nodeINTEST.Name;  //version_test    //Removed


                        try
                        {
                            if (bAddOVALTEST)
                            {
                                myOVALTest.VocabularyID = iVocabularyOVALID;
                                myOVALTest.CreatedDate = DateTimeOffset.Now;
                                oval_model.OVALTEST.Add(myOVALTest);
                            }
                            else
                            {
                                //Update OVALTEST
                            }
                            //attach?
                            myOVALTest.timestamp = DateTimeOffset.Now;
                            oval_model.SaveChanges();
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
                            Console.WriteLine("Exception: DbEntityValidationExceptionREFERENCE " + sb.ToString());
                        }
                        catch (Exception exAddToOVALTEST)
                        {
                            Console.WriteLine("Exception: exAddToOVALTEST " + exAddToOVALTEST.Message + " " + exAddToOVALTEST.InnerException);
                        }




                        foreach (XmlNode nodeTest in nodeINTEST)
                        {

                            switch (nodeTest.Name)
                            {
                                case "object":
                                    string sObjectIDPattern = nodeTest.Attributes["object_ref"].InnerText;
                                    #region ovalobject
                                    int iOVALObjectID = 0;
                                    try
                                    {
                                        iOVALObjectID = oval_model.OVALOBJECT.Where(o => o.OVALObjectIDPattern == sObjectIDPattern).Select(o => o.OVALObjectID).FirstOrDefault();
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                    //XORCISMModel.OVALOBJECT myOVALObject;
                                    //myOVALObject = oval_model.OVALOBJECT.FirstOrDefault(o => o.OVALObjectIDPattern == sObjectIDPattern);// && o.OVALObjectVersion == Convert.ToInt32(sTestVersion));
                                    //if (myOVALObject == null)
                                    if (iOVALObjectID <= 0)
                                    {
                                        Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                        Console.WriteLine("DEBUG Adding OVALOBJECT" + " " + sObjectIDPattern);
                                        OVALOBJECT myOVALObject = new OVALOBJECT();
                                        myOVALObject.OVALObjectIDPattern = sObjectIDPattern;
                                        myOVALObject.comment = "";
                                        myOVALObject.OVALNamespaceID = iOVALNamespaceID;    //default same as the Test
                                        //myOVALObject.Namespace = sNameSpace;    //Removed

                                        myOVALObject.VocabularyID = iVocabularyOVALID;
                                        myOVALObject.CreatedDate = DateTimeOffset.Now;
                                        try
                                        {
                                            myOVALObject.timestamp = DateTimeOffset.Now;
                                            oval_model.OVALOBJECT.Add(myOVALObject);
                                            oval_model.SaveChanges();
                                            iOVALObjectID = myOVALObject.OVALObjectID;
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
                                            Console.WriteLine("Exception: DbEntityValidationExceptionREFERENCE " + sb.ToString());
                                        }
                                        catch (Exception exAddToOVALOBJECT)
                                        {
                                            Console.WriteLine("Exception: exAddToOVALOBJECT " + exAddToOVALOBJECT.Message + " " + exAddToOVALOBJECT.InnerException);
                                        }
                                    }
                                    else
                                    {
                                        //Update OVALOBJECT
                                    }
                                    #endregion ovalobject

                                    #region ovaltestobject
                                    int iOVALTestObjectID = 0;
                                    try
                                    {
                                        iOVALTestObjectID = oval_model.OVALOBJECTFOROVALTEST.Where(o => o.OVALObjectID == iOVALObjectID && o.OVALTestID == myOVALTest.OVALTestID).Select(o => o.OVALTestObjectID).FirstOrDefault();
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                    //XORCISMModel.OVALOBJECTFOROVALTEST myOVALObjectForOVALTEST;
                                    //myOVALObjectForOVALTEST = oval_model.OVALOBJECTFOROVALTEST.FirstOrDefault(o => o.OVALObjectID == iOVALObjectID && o.OVALTestID == myOVALTest.OVALTestID);// && o.OVALObjectVersion == Convert.ToInt32(sTestVersion));
                                    //if (myOVALObjectForOVALTEST == null)
                                    if (iOVALTestObjectID <= 0)
                                    {
                                        Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                        Console.WriteLine("DEBUG Adding OVALOBJECTFOROVALTEST" + " " + sObjectIDPattern);
                                        OVALOBJECTFOROVALTEST myOVALObjectForOVALTEST = new OVALOBJECTFOROVALTEST();
                                        myOVALObjectForOVALTEST.OVALTestID = myOVALTest.OVALTestID;
                                        myOVALObjectForOVALTEST.OVALObjectID = iOVALObjectID;// myOVALObject.OVALObjectID;
                                        myOVALObjectForOVALTEST.CreatedDate = DateTimeOffset.Now;
                                        myOVALObjectForOVALTEST.VocabularyID = iVocabularyOVALID;
                                        myOVALObjectForOVALTEST.timestamp = DateTimeOffset.Now;
                                        try
                                        {
                                            oval_model.OVALOBJECTFOROVALTEST.Add(myOVALObjectForOVALTEST);
                                            oval_model.SaveChanges();
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
                                            Console.WriteLine("Exception: DbEntityValidationExceptionREFERENCE " + sb.ToString());
                                        }
                                        catch (Exception exAddToOVALOBJECTFOROVALTEST)
                                        {
                                            Console.WriteLine("Exception: exAddToOVALOBJECTFOROVALTEST " + exAddToOVALOBJECTFOROVALTEST.Message + " " + exAddToOVALOBJECTFOROVALTEST.InnerException);
                                        }
                                    }
                                    else
                                    {
                                        //Update OVALOBJECTFOROVALTEST
                                    }
                                    #endregion ovaltestobject
                                    break;

                                case "state":
                                    #region ovalstate
                                    string sStateIDPattern = nodeTest.Attributes["state_ref"].InnerText;
                                    int iOVALStateID = 0;
                                    try
                                    {
                                        iOVALStateID = oval_model.OVALSTATE.Where(o => o.OVALStateIDPattern == sStateIDPattern).Select(o => o.OVALStateID).FirstOrDefault();
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                    //XORCISMModel.OVALSTATE myOVALState;
                                    //myOVALState = oval_model.OVALSTATE.FirstOrDefault(o => o.OVALStateIDPattern == sStateIDPattern);// && o.OVALStateVersion == Convert.ToInt32(sTestVersion));
                                    //if (myOVALState == null)
                                    if (iOVALStateID <= 0)
                                    {
                                        Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                        Console.WriteLine("DEBUG Adding OVALSTATE" + " " + sStateIDPattern);
                                        OVALSTATE myOVALState = new OVALSTATE();
                                        myOVALState.OVALStateIDPattern = sStateIDPattern;
                                        myOVALState.comment = "";
                                        //We will obtain the OVALStateVersion later
                                        try
                                        {
                                            myOVALState.VocabularyID = iVocabularyOVALID;
                                            myOVALState.CreatedDate = DateTimeOffset.Now;
                                            myOVALState.timestamp = DateTimeOffset.Now;
                                            oval_model.OVALSTATE.Add(myOVALState);
                                            oval_model.SaveChanges();
                                            iOVALStateID = myOVALState.OVALStateID;
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
                                            Console.WriteLine("Exception: DbEntityValidationExceptionREFERENCE " + sb.ToString());
                                        }
                                        catch (Exception exAddToOVALSTATE)
                                        {
                                            Console.WriteLine("Exception: exAddToOVALSTATE " + exAddToOVALSTATE.Message + " " + exAddToOVALSTATE.InnerException);
                                        }
                                    }
                                    else
                                    {
                                        //Update OVALSTATE
                                    }
                                    #endregion ovalstate

                                    #region ovalteststate
                                    int iOVALTestStateID = 0;
                                    try
                                    {
                                        iOVALTestStateID = oval_model.OVALSTATEFOROVALTEST.Where(o => o.OVALStateID == iOVALStateID && o.OVALTestID == myOVALTest.OVALTestID).Select(o => o.OVALTestStateID).FirstOrDefault();
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                    //XORCISMModel.OVALSTATEFOROVALTEST myOVALStateForOVALTest;
                                    //myOVALStateForOVALTest = oval_model.OVALSTATEFOROVALTEST.FirstOrDefault(o => o.OVALStateID == iOVALStateID && o.OVALTestID == myOVALTest.OVALTestID);// && o.OVALObjectVersion == Convert.ToInt32(sTestVersion));
                                    //if (myOVALStateForOVALTest == null)
                                    if (iOVALTestStateID <= 0)
                                    {
                                        Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                        Console.WriteLine("DEBUG Adding OVALSTATEFOROVALTEST" + " " + sStateIDPattern);

                                        try
                                        {
                                            OVALSTATEFOROVALTEST myOVALStateForOVALTest = new OVALSTATEFOROVALTEST();
                                            myOVALStateForOVALTest.OVALTestID = myOVALTest.OVALTestID;
                                            myOVALStateForOVALTest.OVALStateID = iOVALStateID;// myOVALState.OVALStateID;
                                            myOVALStateForOVALTest.CreatedDate = DateTimeOffset.Now;
                                            myOVALStateForOVALTest.VocabularyID = iVocabularyOVALID;
                                            myOVALStateForOVALTest.timestamp = DateTimeOffset.Now;
                                            oval_model.OVALSTATEFOROVALTEST.Add(myOVALStateForOVALTest);
                                            oval_model.SaveChanges();    //TEST PERFORMANCE
                                            //iOVALTestStateID = myOVALStateForOVALTest.OVALTestStateID;
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
                                            Console.WriteLine("Exception: DbEntityValidationExceptionREFERENCE " + sb.ToString());
                                        }
                                        catch (Exception exAddToOVALSTATEFOROVALTEST)
                                        {
                                            Console.WriteLine("Exception: exAddToOVALSTATEFOROVALTEST " + exAddToOVALSTATEFOROVALTEST.Message + " " + exAddToOVALSTATEFOROVALTEST.InnerException);
                                        }
                                    }
                                    else
                                    {
                                        //Update OVALSTATEFOROVALTEST
                                    }
                                    #endregion ovalteststate
                                    break;

                                case "oval-def:notes":
                                    /*
                                    <oval-def:notes xmlns:oval1="http://oval.mitre.org/XMLSchema/oval-definitions-5">
                                        <oval-def:note>For "/tmp is readable by non-root users," use a compound test.</oval-def:note>
                                    </oval-def:notes>
                                    */
                                    string sOVALDefNotes = nodeTest.InnerText;
                                    sOVALDefNotes = sOVALDefNotes.Replace("<oval-def:note>", "");
                                    //TODO Review cleaning
                                    sOVALDefNotes = sOVALDefNotes.Replace("</oval-def:note>", " ");
                                    myOVALTest.notes = sOVALDefNotes.Trim();
                                    break;
                                default:
                                    Console.WriteLine("ERROR: Import_oval missing code for ovaltests " + nodeTest.Name);

                                    break;
                            }
                        }
                        //}
                        //else
                        //{
                        //    Console.WriteLine("Import_oval missing code for ovaltests "+node.Name);
                        //    //TODO
                        //    //oslevel_test
                        //    //fix_test
                        //}
                    }
                }
                #endregion ovaltests
            }

            if (bImportOVALObjects)
            {
                #region ovalobjects
                #region freememory
                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                Console.WriteLine("DEBUG FREE MEMORY");
                //FREE
                try
                {
                    oval_model.SaveChanges();
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
                    Console.WriteLine("Exception: DbEntityValidationExceptionGLOBALSAVE " + sb.ToString());
                }
                catch (Exception exGLOBALSAVE)
                {
                    Console.WriteLine("Exception: exGLOBALSAVE " + exGLOBALSAVE.Message + " " + exGLOBALSAVE.InnerException);
                }
                oval_model.Dispose();

                oval_model = new XOVALEntities();
                oval_model.Configuration.AutoDetectChangesEnabled = false;
                oval_model.Configuration.ValidateOnSaveEnabled = false;
                #endregion freememory

                XmlNodeList nodesObjects = doc.SelectNodes("/oval-def:oval_definitions/oval-def:objects", mgr); //Hardcoded
                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                Console.WriteLine("IMPORTING OVALOBJECTS");
                foreach (XmlNode nodesObject in nodesObjects)
                {
                    //TODO
                    //oslevel_object
                    //fix_object
                    //version_object
                    //line_object
                    //registry_object

                    //OVALBehavior  //<behaviors windows_view="32_bit"/>
                    //NOTE: All the objects should already exist
                    foreach (XmlNode nodeOBJECT in nodesObject)
                    {
                        int iRegistryEnumID = 0;
                        string sRegistryHive = "";
                        WINDOWSREGISTRYKEYOBJECT oWindowsRegistryKeyObject = null;
                        //See CybOX http://cybox.mitre.org/language/version2.1/xsddocs/objects/Win_Registry_Key_Object.html
                        //OVALOBJECTWINDOWSREGISTRYKEY

                        Console.WriteLine("DEBUG ###################################################################");
                        Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                        Console.WriteLine("DEBUG nodeOBJECT.Name=" + nodeOBJECT.Name); //version_object    line_object   registry_object
                        string sObjectIDPattern = nodeOBJECT.Attributes["id"].InnerText;    //oval:org.altx-soft.oval:obj:157662
                        Console.WriteLine("DEBUG sObjectIDPattern=" + sObjectIDPattern);
                        Boolean bAddObject = false;
                        string sOVALObjectComment = "";

                        XOVALModel.OVALOBJECT myOVALObject;
                        #region ovalobject
                        myOVALObject = oval_model.OVALOBJECT.FirstOrDefault(o => o.OVALObjectIDPattern == sObjectIDPattern);// && o.OVALObjectVersion == Convert.ToInt32(sTestVersion));
                        //NOTE: we will have the last version only
                        if (myOVALObject == null)
                        {
                            //Console.WriteLine("ERROR: OVALOBJECT not found "+sObjectIDPattern);
                            //Referenced in a variable
                            bAddObject = true;
                            myOVALObject = new OVALOBJECT();
                            myOVALObject.VocabularyID = iVocabularyOVALID;
                            myOVALObject.CreatedDate = DateTimeOffset.Now;
                            myOVALObject.OVALObjectIDPattern = sObjectIDPattern;
                        }
                        else
                        {
                            //Update OVALOBJECT
                        }

                        string sOVALObjectDataTypeName = nodeOBJECT.Name;    //version_object    line_object     registry_object
                        //Update OVALOBJECT
                        //myOVALObject.DataTypeName = sOVALObjectDataTypeName;    //Removed
                        #region ovalobjectdatatype
                        int iOVALObjectDataTypeID = 0;
                        try
                        {
                            iOVALObjectDataTypeID = oval_model.OVALOBJECTDATATYPE.Where(o => o.OVALObjectDataTypeName == sOVALObjectDataTypeName).Select(o => o.OVALObjectDataTypeID).FirstOrDefault();
                        }
                        catch (Exception ex)
                        {

                        }
                        if (iOVALObjectDataTypeID > 0)
                        {
                            //Update OVALOBJECTDATATYPE
                        }
                        else
                        {
                            OVALOBJECTDATATYPE oOVALObjectDataType = new OVALOBJECTDATATYPE();
                            oOVALObjectDataType.CreatedDate = DateTimeOffset.Now;
                            oOVALObjectDataType.OVALObjectDataTypeName = sOVALObjectDataTypeName;
                            oOVALObjectDataType.VocabularyID = iVocabularyOVALID;
                            oOVALObjectDataType.timestamp = DateTimeOffset.Now;
                            oval_model.OVALOBJECTDATATYPE.Add(oOVALObjectDataType);
                            oval_model.SaveChanges();
                            iOVALObjectDataTypeID = oOVALObjectDataType.OVALObjectDataTypeID;
                        }
                        #endregion ovalobjectdatatype
                        myOVALObject.OVALObjectDataTypeID = iOVALObjectDataTypeID;

                        int iObjectVersion = 0;
                        try
                        {
                            iObjectVersion = Convert.ToInt32(nodeOBJECT.Attributes["version"].InnerText);
                        }
                        catch (Exception exiObjectVersion)
                        {
                            Console.WriteLine("Exception: exiObjectVersion " + exiObjectVersion.Message + " " + exiObjectVersion.InnerException);
                        }
                        //Update OVALOBJECT
                        myOVALObject.OVALObjectVersion = iObjectVersion;

                        try
                        {
                            sOVALObjectComment = nodeOBJECT.Attributes["comment"].InnerText;
                            //This registry key identifies the system root.
                            //Update OVALOBJECT
                            myOVALObject.comment = sOVALObjectComment;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("NOTE: no comment for the OVALOBJECT " + sObjectIDPattern);
                            myOVALObject.comment = "";
                        }
                        try
                        {
                            string sObjectDeprecated = nodeOBJECT.Attributes["deprecated"].InnerText;
                            if (sObjectDeprecated.ToLower() == "true")
                            {
                                myOVALObject.deprecated = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            myOVALObject.deprecated = false;
                        }

                        //myOVALObject.Namespace    //xmlns="http://oval.mitre.org/XMLSchema/oval-definitions-5#pixos"  pixos-def
                        string sNameSpace = "";
                        string sXMLNS = nodeOBJECT.Attributes["xmlns"].InnerText;
                        //TODO: do it more generic
                        if (sXMLNS.Contains("#aix")) sNameSpace = "aix-def";
                        if (sXMLNS.Contains("#apache")) sNameSpace = "apache-def";
                        if (sXMLNS.Contains("#catos")) sNameSpace = "catos-def";
                        if (sXMLNS.Contains("#esx")) sNameSpace = "esx-def";
                        if (sXMLNS.Contains("#freebsd")) sNameSpace = "freebsd-def";
                        if (sXMLNS.Contains("#hpux")) sNameSpace = "hpux-def";
                        if (sXMLNS.Contains("#independent")) sNameSpace = "ind-def";
                        if (sXMLNS.Contains("#ios")) sNameSpace = "ios-def";
                        if (sXMLNS.Contains("#linux")) sNameSpace = "linux-def";
                        if (sXMLNS.Contains("#macos")) sNameSpace = "macos-def";
                        if (sXMLNS.Contains("#pixos")) sNameSpace = "pixos-def";
                        if (sXMLNS.Contains("#sharepoint")) sNameSpace = "sp-def";
                        if (sXMLNS.Contains("#solaris")) sNameSpace = "sol-def";
                        if (sXMLNS.Contains("#unix")) sNameSpace = "unix-def";
                        if (sXMLNS.Contains("#windows")) sNameSpace = "win-def";
                        //TODO: mapping
                        if (sNameSpace == "")
                        {
                            Console.WriteLine("ERROR: Unknown OBJECT Namespace: " + sXMLNS);
                        }
                        else
                        {
                            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                            Console.WriteLine("DEBUG OBJECT Namespace=" + sNameSpace);
                        }
                        //Update OVALOBJECT
                        //myOVALObject.Namespace = sNameSpace;    //Removed
                        #region OVALNAMESPACE
                        int iOVALNamespaceID = 0;
                        try
                        {
                            iOVALNamespaceID = oval_model.OVALNAMESPACE.Where(o => o.OVALNamespaceName == sNameSpace).Select(o => o.OVALNamespaceID).FirstOrDefault();
                        }
                        catch (Exception ex)
                        {

                        }
                        if (iOVALNamespaceID > 0)
                        {
                            //Update OVALNAMESPACE
                        }
                        else
                        {
                            OVALNAMESPACE oOVALNamespace = new OVALNAMESPACE();
                            oOVALNamespace.CreatedDate = DateTimeOffset.Now;
                            oOVALNamespace.OVALNamespaceName = sNameSpace;
                            oOVALNamespace.VocabularyID = iVocabularyOVALID;
                            oOVALNamespace.timestamp = DateTimeOffset.Now;
                            oval_model.OVALNAMESPACE.Add(oOVALNamespace);
                            oval_model.SaveChanges();
                            iOVALNamespaceID = oOVALNamespace.OVALNamespaceID;
                        }
                        #endregion OVALNAMESPACE
                        myOVALObject.OVALNamespaceID = iOVALNamespaceID;

                        try
                        {
                            if (bAddObject)
                            {
                                oval_model.OVALOBJECT.Add(myOVALObject);
                            }
                            myOVALObject.timestamp = DateTimeOffset.Now;
                            oval_model.OVALOBJECT.Attach(myOVALObject);
                            oval_model.Entry(myOVALObject).State = EntityState.Modified;
                            oval_model.SaveChanges();
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
                            Console.WriteLine("Exception: DbEntityValidationExceptionREFERENCE " + sb.ToString());
                        }
                        catch (Exception exAddToOVALOBJECT)
                        {
                            Console.WriteLine("Exception: exAddToOVALOBJECT " + exAddToOVALOBJECT.Message + " " + exAddToOVALOBJECT.InnerException);
                        }
                        #endregion ovalobject


                        //TODO (done after)
                        /*
                        switch(nodeOBJECT.Name)
                        {
                            case "registry_object":

                                break;

                            case "file_object":
                                //FILE
                                //PRODUCTFILE
                                break;

                            default:

                                break;
                        }
                        */


                        if (nodeOBJECT.ChildNodes.Count > 0)
                        {
                            //We need a record TODO REVIEW
                            XOVALModel.OVALOBJECTRECORD myOVALObjectRecord;
                            XOVALModel.OVALOBJECTRECORDFOROVALOBJECT myOVALObjectRecordForOVALObject;
                            myOVALObjectRecordForOVALObject = oval_model.OVALOBJECTRECORDFOROVALOBJECT.FirstOrDefault(o => o.OVALObjectID == myOVALObject.OVALObjectID);
                            if (myOVALObjectRecordForOVALObject == null)
                            {
                                //TODO
                                //OVAL COMPLEXBASE
                                myOVALObjectRecord = new OVALOBJECTRECORD();
                                //myOVALObjectRecord.DataTypeName = sOVALObjectDataTypeName;  //Removed   // nodeOBJECT.Name;    //line_object     registry_object  file_object
                                myOVALObjectRecord.OVALObjectDataTypeID = iOVALObjectDataTypeID;

                                //TODO
                                //Operation
                                //...
                                //mask

                                //myOVALObjectRecord.Namespace = sNameSpace;   //Removed  //pixos-def    windows
                                myOVALObjectRecord.OVALNamespaceID = iOVALNamespaceID;

                                try
                                {
                                    myOVALObjectRecord.VocabularyID = iVocabularyOVALID;
                                    myOVALObjectRecord.CreatedDate = DateTimeOffset.Now;
                                    myOVALObjectRecord.timestamp = DateTimeOffset.Now;
                                    oval_model.OVALOBJECTRECORD.Add(myOVALObjectRecord);
                                    oval_model.SaveChanges();
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
                                    Console.WriteLine("Exception: DbEntityValidationExceptionREFERENCE " + sb.ToString());
                                }
                                catch (Exception exAddToOVALOBJECTRECORD)
                                {
                                    Console.WriteLine("Exception: exAddToOVALOBJECTRECORD " + exAddToOVALOBJECTRECORD.Message + " " + exAddToOVALOBJECTRECORD.InnerException);
                                }


                                try
                                {
                                    myOVALObjectRecordForOVALObject = new OVALOBJECTRECORDFOROVALOBJECT();
                                    myOVALObjectRecordForOVALObject.OVALObjectID = myOVALObject.OVALObjectID;
                                    myOVALObjectRecordForOVALObject.OVALObjectRecordID = myOVALObjectRecord.OVALObjectRecordID;
                                    myOVALObjectRecordForOVALObject.VocabularyID = iVocabularyOVALID;
                                    myOVALObjectRecordForOVALObject.CreatedDate = DateTimeOffset.Now;
                                    myOVALObjectRecordForOVALObject.timestamp = DateTimeOffset.Now;
                                    oval_model.OVALOBJECTRECORDFOROVALOBJECT.Add(myOVALObjectRecordForOVALObject);
                                    oval_model.SaveChanges();
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
                                    Console.WriteLine("Exception: DbEntityValidationExceptionREFERENCE " + sb.ToString());
                                }
                                catch (Exception exAddToOVALOBJECTRECORDFOROVALOBJECT)
                                {
                                    Console.WriteLine("Exception: exAddToOVALOBJECTRECORDFOROVALOBJECT " + exAddToOVALOBJECTRECORDFOROVALOBJECT.Message + " " + exAddToOVALOBJECTRECORDFOROVALOBJECT.InnerException);
                                }
                            }
                            else
                            {
                                //Update OVALOBJECTRECORDFOROVALOBJECT
                                //Console.WriteLine(myOVALObjectRecordForOVALObject.OVALObjectRecordID);
                                myOVALObjectRecord = myOVALObjectRecordForOVALObject.OVALOBJECTRECORD;
                                //Console.WriteLine(myOVALObjectRecord.OVALObjectRecordID);
                                myOVALObjectRecord.OVALObjectDataTypeID = iOVALObjectDataTypeID;
                                myOVALObjectRecord.OVALNamespaceID = iOVALNamespaceID;
                                myOVALObjectRecord.VocabularyID = iVocabularyOVALID;
                                myOVALObjectRecord.timestamp = DateTimeOffset.Now;

                            }

                            int iOVALObjectRecordID = myOVALObjectRecord.OVALObjectRecordID;
                            string sOVALVariableIDPattern = ""; //1 maximum per object?
                            int iOVALVariableID = 0;

                            foreach (XmlNode nodeOBJECTFIELD in nodeOBJECT)
                            {
                                Console.WriteLine("DEBUG nodeOBJECTFIELD.Name=" + nodeOBJECTFIELD.Name);

                                //First: OVALOBJECTFIELD for all of them
                                #region OVALOBJECTFIELD
                                //We assume that we have only one OVALOBJECTFIELD for one DataType for one OVALOBJECTRECORD
                                XOVALModel.OVALOBJECTFIELD myOVALObjectField = null;
                                Boolean bAddOvalObjectField = false;
                                XOVALModel.OVALOBJECTFIELDFOROVALOBJECTRECORD myOVALObjectFieldForOVALObjectRecord;
                                Boolean bAddOvalObjectFieldForOvalObjectRecord = false;
                                myOVALObjectFieldForOVALObjectRecord = oval_model.OVALOBJECTFIELDFOROVALOBJECTRECORD.FirstOrDefault(o => o.OVALObjectRecordID == myOVALObjectRecord.OVALObjectRecordID && o.OVALOBJECTFIELD.FieldName == nodeOBJECTFIELD.Name);
                                if (myOVALObjectFieldForOVALObjectRecord == null)
                                {
                                    //We add a OVALOBJECTFIELD
                                    try
                                    {
                                        myOVALObjectField = new OVALOBJECTFIELD();
                                        myOVALObjectField.FieldName = "";
                                        myOVALObjectField.VocabularyID = iVocabularyOVALID;
                                        myOVALObjectField.CreatedDate = DateTimeOffset.Now;
                                        myOVALObjectField.timestamp = DateTimeOffset.Now;
                                        oval_model.OVALOBJECTFIELD.Add(myOVALObjectField);
                                        oval_model.SaveChanges();
                                        bAddOvalObjectField = true;

                                        bAddOvalObjectFieldForOvalObjectRecord = true;
                                        myOVALObjectFieldForOVALObjectRecord = new OVALOBJECTFIELDFOROVALOBJECTRECORD();
                                        myOVALObjectFieldForOVALObjectRecord.VocabularyID = iVocabularyOVALID;
                                        myOVALObjectFieldForOVALObjectRecord.CreatedDate = DateTimeOffset.Now;
                                        myOVALObjectFieldForOVALObjectRecord.OVALObjectFieldID = myOVALObjectField.OVALObjectFieldID;
                                    }
                                    catch (Exception exAddOVALOBJECTFIELD)
                                    {
                                        Console.WriteLine("Exception: exAddOVALOBJECTFIELD " + exAddOVALOBJECTFIELD.Message + " " + exAddOVALOBJECTFIELD.InnerException);
                                    }




                                }
                                else
                                {
                                    //Update OVALOBJECTFIELDFOROVALOBJECTRECORD
                                    myOVALObjectField = myOVALObjectFieldForOVALObjectRecord.OVALOBJECTFIELD;
                                }
                                try
                                {
                                    myOVALObjectFieldForOVALObjectRecord.timestamp = DateTimeOffset.Now;
                                }
                                catch(Exception myOVALObjectFieldForOVALObjectRecordTimestamp)
                                {
                                    Console.WriteLine("Exception: myOVALObjectFieldForOVALObjectRecordTimestamp " + myOVALObjectFieldForOVALObjectRecordTimestamp.Message + " " + myOVALObjectFieldForOVALObjectRecordTimestamp.InnerException);
                                }
                                //TODO
                                //

                                string sOVALObjectFieldName = nodeOBJECTFIELD.Name;    //show_subcommand    (behaviors) (hive) (key) (name)
                                Console.WriteLine("DEBUG sOVALObjectFieldName=" + nodeOBJECTFIELD.Name);
                                myOVALObjectField.FieldName = sOVALObjectFieldName;

                                try
                                {
                                    string sVarCheck = nodeOBJECTFIELD.Attributes["var_check"].InnerText;    //may not exist  //all
                                    //myOVALObjectField.VarCheck = sVarCheck; //Removed
                                    #region checkenumeration
                                    int iCheckEnumerationID = 0;
                                    try
                                    {
                                        iCheckEnumerationID = model.CHECKENUMERATION.Where(o => o.EnumerationValue == sVarCheck).Select(o => o.CheckEnumerationID).FirstOrDefault();
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                    if (iCheckEnumerationID <= 0)
                                    {
                                        CHECKENUMERATION oCheckEnumeration = new CHECKENUMERATION();
                                        oCheckEnumeration.CreatedDate = DateTimeOffset.Now;
                                        oCheckEnumeration.EnumerationValue = sVarCheck;
                                        oCheckEnumeration.VocabularyID = iVocabularyOVALID;
                                        oCheckEnumeration.timestamp = DateTimeOffset.Now;
                                        model.CHECKENUMERATION.Add(oCheckEnumeration);
                                        model.SaveChanges();
                                        iCheckEnumerationID = oCheckEnumeration.CheckEnumerationID;
                                    }
                                    else
                                    {
                                        //Update CHECKENUMERATION
                                    }

                                    #endregion checkenumeration
                                    myOVALObjectField.CheckEnumerationID = iCheckEnumerationID;
                                }
                                catch (Exception ex)
                                {

                                }


                                try
                                {
                                    //<path var_check="all" var_ref="oval:org.mitre.oval:var:1831"/>
                                    sOVALVariableIDPattern = nodeOBJECTFIELD.Attributes["var_ref"].InnerText;    //may not exist
                                    myOVALObjectField.VarRef = sOVALVariableIDPattern;

                                }
                                catch (Exception ex)
                                {

                                }

                                if (nodeOBJECTFIELD.Name == "var_ref")
                                {
                                    //<var_ref>oval:org.mitre.oval:var:1299</var_ref>
                                    sOVALVariableIDPattern = nodeOBJECTFIELD.InnerText;
                                    myOVALObjectField.VarRef = sOVALVariableIDPattern;
                                }
                                #region ovalvariableref
                                if (sOVALVariableIDPattern != "")
                                {
                                    Console.WriteLine("DEBUG var_ref=" + sOVALVariableIDPattern);
                                    //OVALVARIABLEFOROVALOBJECTFIELD
                                    //XORCISMModel.OVALVARIABLE myOVALVariable;
                                    //myOVALVariable = oval_model.OVALVARIABLE.FirstOrDefault(o => o.OVALVariableIDPattern == sOVALVariableIDPattern);
                                    try
                                    {
                                        iOVALVariableID = oval_model.OVALVARIABLE.Where(o => o.OVALVariableIDPattern == sOVALVariableIDPattern).Select(o => o.OVALVariableID).FirstOrDefault();
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                    //if (myOVALVariable == null)
                                    if (iOVALVariableID <= 0)
                                    {
                                        Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                        Console.WriteLine("DEBUG Adding OVALVARIABLE " + sOVALVariableIDPattern);

                                        try
                                        {
                                            OVALVARIABLE myOVALVariable = new OVALVARIABLE();
                                            myOVALVariable.OVALVariableIDPattern = sOVALVariableIDPattern;
                                            myOVALVariable.VocabularyID = iVocabularyOVALID;
                                            myOVALVariable.CreatedDate = DateTimeOffset.Now;
                                            myOVALVariable.timestamp = DateTimeOffset.Now;
                                            myOVALVariable.OVALVariableVersion = 1;
                                            //myOVALVariable.DataTypeName = "string"; //Hardcoded default //Removed
                                            #region OVALVARIABLEDATATYPE
                                            int iOVALVariableDataTypeID = 0;
                                            try
                                            {
                                                iOVALVariableDataTypeID = oval_model.OVALVARIABLEDATATYPE.Where(o => o.OVALVariableDataTypeName == "string").Select(o => o.OVALVariableDataTypeID).FirstOrDefault();
                                            }
                                            catch (Exception ex)
                                            {

                                            }
                                            if (iOVALVariableDataTypeID <= 0)
                                            {
                                                OVALVARIABLEDATATYPE oOVALVariableDataType = new OVALVARIABLEDATATYPE();
                                                oOVALVariableDataType.CreatedDate = DateTimeOffset.Now;
                                                oOVALVariableDataType.OVALVariableDataTypeName = "string";
                                                oOVALVariableDataType.VocabularyID = iVocabularyOVALID;
                                                oOVALVariableDataType.timestamp = DateTimeOffset.Now;
                                                oval_model.OVALVARIABLEDATATYPE.Add(oOVALVariableDataType);
                                                oval_model.SaveChanges();
                                                iOVALVariableDataTypeID = oOVALVariableDataType.OVALVariableDataTypeID;
                                            }
                                            else
                                            {
                                                //Update OVALVARIABLEDATATYPE
                                            }
                                            #endregion OVALVARIABLEDATATYPE
                                            myOVALVariable.OVALVariableDataTypeID = iOVALVariableDataTypeID;

                                            myOVALVariable.comment = "";
                                            //NameSpaceID?  //TODO
                                            oval_model.OVALVARIABLE.Add(myOVALVariable);
                                            oval_model.SaveChanges();
                                            iOVALVariableID = myOVALVariable.OVALVariableID;
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
                                            Console.WriteLine("Exception: DbEntityValidationExceptionREFERENCE " + sb.ToString());
                                        }
                                        catch (Exception exAddToOVALVARIABLE)
                                        {
                                            Console.WriteLine("Exception: exAddToOVALVARIABLE " + exAddToOVALVARIABLE.Message + " " + exAddToOVALVARIABLE.InnerException);
                                        }
                                    }
                                    else
                                    {
                                        //Update OVALVARIABLE
                                    }
                                }
                                #endregion ovalvariableref

                                string sDataTypeName = "string";    //default
                                myOVALObjectField.DataTypeName = "string";  // nodeOBJECTFIELD.Name;     //show_subcommand
                                try
                                {
                                    //<instance datatype="int">1</instance>
                                    myOVALObjectField.DataTypeName = nodeOBJECTFIELD.Attributes["datatype"].InnerText;  //int
                                }
                                catch (Exception ex)
                                {
                                    myOVALObjectField.DataTypeName = "string";
                                }
                                //TODO
                                //DataTypeID=


                                try
                                {
                                    //myOVALObjectField.OperationValue = nodeOBJECTFIELD.Attributes["operation"].InnerText; //equals
                                }
                                catch (Exception ex)
                                {

                                }

                                //TODO
                                //OVALENTITYATTRIBUTEGROUP
                                //myOVALObjectField.OperationValue = "equals";
                                try
                                {
                                    string sOperation = nodeOBJECTFIELD.Attributes["operation"].InnerText;  //pattern match
                                    //XORCISMModel.OPERATIONENUMERATION myOperation;
                                    //myOperation = model.OPERATIONENUMERATION.FirstOrDefault(o => o.OperationValue == sOperation);   //&&VocabularyID
                                    int iOperationEnumerationID = 0;
                                    //TODO? hardcode
                                    #region OPERATIONENUMERATION
                                    try
                                    {
                                        iOperationEnumerationID = model.OPERATIONENUMERATION.Where(o => o.OperationValue == sOperation).Select(o => o.OperationEnumerationID).FirstOrDefault();
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                    //if (myOperation == null)
                                    if (iOperationEnumerationID <= 0)
                                    {
                                        //Console.WriteLine("ERROR: Unknown Operation " + sOperation);
                                        OPERATIONENUMERATION oOperationEnumeration = new OPERATIONENUMERATION();
                                        oOperationEnumeration.CreatedDate = DateTimeOffset.Now;
                                        oOperationEnumeration.OperationValue = sOperation;
                                        oOperationEnumeration.VocabularyID = iVocabularyOVALID;
                                        oOperationEnumeration.timestamp = DateTimeOffset.Now;
                                        model.OPERATIONENUMERATION.Add(oOperationEnumeration);
                                        model.SaveChanges();
                                        iOperationEnumerationID = oOperationEnumeration.OperationEnumerationID;
                                    }
                                    else
                                    {
                                        //Update OPERATIONENUMERATION

                                    }
                                    myOVALObjectField.OperationEnumerationID = iOperationEnumerationID; // myOperation.OperationEnumerationID;
                                    #endregion OPERATIONENUMERATION
                                    //myOVALObjectField.OperationValue = sOperation;  // REMOVED

                                }
                                catch (Exception ex)
                                {
                                    myOVALObjectField.OperationEnumerationID = 1;   //TODO Hardcoded (equals)
                                    //myOVALObjectField.OperationValue = "equals";    // Hardcoded REMOVED
                                }
                                myOVALObjectField.FieldValue = nodeOBJECTFIELD.InnerText;   //show running-config

                                myOVALObjectField.timestamp = DateTimeOffset.Now;
                                //Namespace //pixos-def
                                myOVALObjectField.Namespace = sNameSpace;   //same as the object
                                //TODO OVALNAMESPACE

                                try
                                {
                                    if (bAddOvalObjectField)
                                    {
                                        //model.OVALOBJECTFIELD.Add(myOVALObjectField);
                                    }
                                    else
                                    {
                                        //Update OVALOBJECTFIELD
                                    }

                                    //model.SaveChanges();
                                    oval_model.SaveChanges();

                                }
                                //NOTE: Not perfect resolution of https://msdn.microsoft.com/en-us/data/jj592904
                                catch (DbUpdateConcurrencyException ex)
                                {
                                    //saveFailed = true;

                                    // Update original values from the database 
                                    var entry = ex.Entries.Single();
                                    entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                                }
                                catch (Exception exAddToOVALOBJECTFIELD)
                                {
                                    Console.WriteLine("Exception: exAddToOVALOBJECTFIELD " + exAddToOVALOBJECTFIELD.Message + " " + exAddToOVALOBJECTFIELD.InnerException);
                                }

                                myOVALObjectFieldForOVALObjectRecord.OVALObjectRecordID = myOVALObjectRecord.OVALObjectRecordID;
                                myOVALObjectFieldForOVALObjectRecord.OVALObjectFieldID = myOVALObjectField.OVALObjectFieldID;
                                try
                                {
                                    if (bAddOvalObjectFieldForOvalObjectRecord)
                                    {
                                        oval_model.OVALOBJECTFIELDFOROVALOBJECTRECORD.Add(myOVALObjectFieldForOVALObjectRecord);
                                    }
                                    else
                                    {
                                        //Update OVALOBJECTFIELDFOROVALOBJECTRECORD
                                    }
                                    myOVALObjectFieldForOVALObjectRecord.timestamp = DateTimeOffset.Now;
                                    oval_model.SaveChanges();
                                }
                                catch (Exception exAddToOVALOBJECTFIELDFOROVALOBJECTRECORD)
                                {
                                    Console.WriteLine("Exception: exAddToOVALOBJECTFIELDFOROVALOBJECTRECORD " + exAddToOVALOBJECTFIELDFOROVALOBJECTRECORD.Message + " " + exAddToOVALOBJECTFIELDFOROVALOBJECTRECORD.InnerException);
                                }
                                #endregion OVALOBJECTFIELD

                                //Then more specific
                                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                Console.WriteLine("DEBUG Specific nodeOBJECTFIELD.Name=" + nodeOBJECTFIELD.Name);

                                switch (nodeOBJECTFIELD.Name)
                                {
                                    case "behaviors":
                                        #region behaviors
                                        //<behaviors windows_view="32_bit"/>
                                        //OVALBEHAVIOR
                                        string sBehaviorKey = nodeOBJECTFIELD.Attributes[0].Name;   //windows_view
                                        Console.WriteLine("DEBUG sBehaviorKey: " + sBehaviorKey);
                                        //Note: BehaviorKey was BehaviorName
                                        string sBehaviorValue = nodeOBJECTFIELD.Attributes[0].InnerText;    //32_bit

                                        int iOVALBehaviorID = 0;
                                        try
                                        {
                                            iOVALBehaviorID = oval_model.OVALBEHAVIOR.Where(o => o.BehaviorKey == sBehaviorKey && o.BehaviorValue == sBehaviorValue).Select(o => o.OVALBehaviorID).FirstOrDefault();
                                        }
                                        catch (Exception ex)
                                        {

                                        }
                                        //XORCISMModel.OVALBEHAVIOR myBehavior;
                                        //myBehavior = oval_model.OVALBEHAVIOR.FirstOrDefault(o => o.BehaviorKey == sBehaviorKey && o.BehaviorValue == sBehaviorValue);
                                        //if (myBehavior == null)
                                        if (iOVALBehaviorID <= 0)
                                        {
                                            OVALBEHAVIOR myBehavior = new OVALBEHAVIOR();
                                            myBehavior.BehaviorKey = sBehaviorKey;  //windows_view
                                            myBehavior.BehaviorValue = sBehaviorValue;  //32_bit
                                            myBehavior.VocabularyID = iVocabularyOVALID;
                                            myBehavior.CreatedDate = DateTimeOffset.Now;
                                            myBehavior.timestamp = DateTimeOffset.Now;
                                            try
                                            {
                                                oval_model.OVALBEHAVIOR.Add(myBehavior);
                                                oval_model.SaveChanges();
                                                iOVALBehaviorID = myBehavior.OVALBehaviorID;
                                            }
                                            catch (Exception exAddToOVALBEHAVIOR)
                                            {
                                                Console.WriteLine("Exception: exAddToOVALBEHAVIOR " + exAddToOVALBEHAVIOR.Message + " " + exAddToOVALBEHAVIOR.InnerException);
                                            }
                                        }
                                        else
                                        {
                                            //Update OVALBEHAVIOR
                                        }

                                        int iOVALObjectBehavior = oval_model.OVALBEHAVIORFOROVALOBJECT.Where(o => o.OVALObjectID == myOVALObject.OVALObjectID && o.OVALBehaviorID == iOVALBehaviorID).Select(o => o.OVALObjectBehaviorID).FirstOrDefault();
                                        //XORCISMModel.OVALBEHAVIORFOROVALOBJECT myBehaviorForObject;
                                        //myBehaviorForObject = oval_model.OVALBEHAVIORFOROVALOBJECT.FirstOrDefault(o => o.OVALObjectID == myOVALObject.OVALObjectID && o.OVALBehaviorID == iOVALBehaviorID);
                                        //if (myBehaviorForObject == null)
                                        if (iOVALObjectBehavior <= 0)
                                        {
                                            OVALBEHAVIORFOROVALOBJECT myBehaviorForObject = new OVALBEHAVIORFOROVALOBJECT();
                                            myBehaviorForObject.OVALObjectID = myOVALObject.OVALObjectID;
                                            myBehaviorForObject.OVALBehaviorID = iOVALBehaviorID;   // myBehavior.OVALBehaviorID;
                                            myBehaviorForObject.VocabularyID = iVocabularyOVALID;
                                            myBehaviorForObject.CreatedDate = DateTimeOffset.Now;
                                            myBehaviorForObject.timestamp = DateTimeOffset.Now;
                                            try
                                            {
                                                oval_model.OVALBEHAVIORFOROVALOBJECT.Add(myBehaviorForObject);
                                                oval_model.SaveChanges();
                                            }
                                            catch (Exception exAddToOVALBEHAVIORFOROVALOBJECT)
                                            {
                                                Console.WriteLine("Exception: exAddToOVALBEHAVIORFOROVALOBJECT " + exAddToOVALBEHAVIORFOROVALOBJECT.Message + " " + exAddToOVALBEHAVIORFOROVALOBJECT.InnerException);
                                            }
                                        }
                                        else
                                        {
                                            //Update OVALBEHAVIORFOROVALOBJECT
                                        }

                                        #endregion behaviors
                                        break;

                                    case "hive":
                                        //<hive>HKEY_LOCAL_MACHINE</hive>
                                        #region registryhiveenum
                                        //if (sOVALObjectDataTypeName == "registry_object")
                                        sRegistryHive = nodeOBJECTFIELD.InnerText;
                                        iRegistryEnumID = model.REGISTRYHIVEENUM.Where(o => o.RegistryHiveName == sRegistryHive).Select(o => o.RegistryHiveEnumID).FirstOrDefault();
                                        if (iRegistryEnumID > 0)
                                        {
                                            //Update REGISTRYHIVEENUM
                                        }
                                        else
                                        {
                                            try
                                            {
                                                REGISTRYHIVEENUM oRegistryHiveEnum = new REGISTRYHIVEENUM();
                                                oRegistryHiveEnum.RegistryHiveName = sRegistryHive;
                                                oRegistryHiveEnum.VocabularyID = iVocabularyOVALID;
                                                oRegistryHiveEnum.CreatedDate = DateTimeOffset.Now;
                                                oRegistryHiveEnum.timestamp = DateTimeOffset.Now;
                                                model.REGISTRYHIVEENUM.Add(oRegistryHiveEnum);
                                                model.SaveChanges();
                                                iRegistryEnumID = oRegistryHiveEnum.RegistryHiveEnumID;
                                            }
                                            catch (Exception exoRegistryHiveEnum)
                                            {
                                                Console.WriteLine("Exception: exoRegistryHiveEnum " + exoRegistryHiveEnum.Message + " " + exoRegistryHiveEnum.InnerException);
                                            }
                                        }
                                        Console.WriteLine("DEBUG iRegistryEnumID=" + iRegistryEnumID);
                                        #endregion registryhiveenum
                                        break;

                                    case "key":
                                        #region ovalregistrykey
                                        //SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\N360
                                        //We check that it is a registry_object
                                        //Note: we could find a key in a metabase_object, without a hive
                                        //e.g. oval:org.mitre.oval:obj:495
                                        if (sOVALObjectDataTypeName == "registry_object")
                                        {
                                            string sRegistryKey = nodeOBJECTFIELD.InnerText;
                                            //TODO PATCH Hotfix\Q841373

                                            //TODO REVIEW
                                            //int iWindowsRegistryKeyObjectID = model.WINDOWSREGISTRYKEYOBJECT.Where(o => o.Full_Key == sRegistryKey).Select(o=>o.WindowsRegistryKeyObjectID).FirstOrDefault();
                                            oWindowsRegistryKeyObject = windows_model.WINDOWSREGISTRYKEYOBJECT.FirstOrDefault(o => o.Hive == sRegistryHive && o.Full_Key == sRegistryKey);
                                            //if (iWindowsRegistryKeyObjectID>0)
                                            if (oWindowsRegistryKeyObject != null)
                                            {
                                                //Update WINDOWSREGISTRYKEYOBJECT
                                            }
                                            else
                                            {
                                                try
                                                {
                                                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                    Console.WriteLine("DEBUG Adding WINDOWSREGISTRYKEYOBJECT " + sRegistryKey);
                                                    oWindowsRegistryKeyObject = new WINDOWSREGISTRYKEYOBJECT();
                                                    oWindowsRegistryKeyObject.Full_Key = sRegistryKey;
                                                    oWindowsRegistryKeyObject.Hive = sRegistryHive; //TODO? Remove?
                                                    oWindowsRegistryKeyObject.RegistryHiveID = iRegistryEnumID;
                                                    oWindowsRegistryKeyObject.VocabularyID = iVocabularyOVALID;
                                                    oWindowsRegistryKeyObject.CreatedDate = DateTimeOffset.Now;
                                                    oWindowsRegistryKeyObject.timestamp = DateTimeOffset.Now;
                                                    windows_model.WINDOWSREGISTRYKEYOBJECT.Add(oWindowsRegistryKeyObject);
                                                    windows_model.SaveChanges();
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
                                                    Console.WriteLine("Exception: DbEntityValidationExceptionUPDATECAPEC " + sb.ToString());
                                                }
                                                catch (Exception exoWindowsRegistryKeyObject)
                                                {
                                                    Console.WriteLine("Exception: exoWindowsRegistryKeyObject " + exoWindowsRegistryKeyObject.Message + " " + exoWindowsRegistryKeyObject.InnerException);
                                                }
                                            }

                                            string sRegistryKeyOperation = "";  //pattern match
                                            try
                                            {
                                                //<key operation="pattern match">
                                                sRegistryKeyOperation = nodeOBJECTFIELD.Attributes["operation"].InnerText;
                                            }
                                            catch (Exception ex)
                                            {

                                            }

                                            //OVALOBJECTWINDOWSREGISTRYKEY
                                            int iOVALObjectWindowsRegistryKeyID = oval_model.OVALOBJECTWINDOWSREGISTRYKEY.Where(o => o.OVALObjectID == myOVALObject.OVALObjectID && o.WindowsRegistryKeyObjectID == oWindowsRegistryKeyObject.WindowsRegistryKeyObjectID).Select(o => o.OVALObjectWindowsRegistryKeyID).FirstOrDefault();
                                            if (iOVALObjectWindowsRegistryKeyID <= 0)
                                            {
                                                try
                                                {
                                                    OVALOBJECTWINDOWSREGISTRYKEY oOVALObjectWindowsRegistryKey = new OVALOBJECTWINDOWSREGISTRYKEY();
                                                    oOVALObjectWindowsRegistryKey.OVALObjectID = myOVALObject.OVALObjectID;
                                                    oOVALObjectWindowsRegistryKey.WindowsRegistryKeyObjectID = oWindowsRegistryKeyObject.WindowsRegistryKeyObjectID;
                                                    oOVALObjectWindowsRegistryKey.operation = sRegistryKeyOperation;
                                                    oOVALObjectWindowsRegistryKey.CreatedDate = DateTimeOffset.Now;
                                                    oOVALObjectWindowsRegistryKey.VocabularyID = iVocabularyOVALID;
                                                    oOVALObjectWindowsRegistryKey.timestamp = DateTimeOffset.Now;
                                                    oval_model.OVALOBJECTWINDOWSREGISTRYKEY.Add(oOVALObjectWindowsRegistryKey);
                                                    oval_model.SaveChanges();
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
                                                    Console.WriteLine("Exception: DbEntityValidationExceptionREFERENCE " + sb.ToString());
                                                }
                                                catch (Exception exoOVALObjectWindowsRegistryKey)
                                                {
                                                    Console.WriteLine("Exception: exoOVALObjectWindowsRegistryKey " + exoOVALObjectWindowsRegistryKey.Message + " " + exoOVALObjectWindowsRegistryKey.InnerException);
                                                }
                                            }
                                        }
                                        #endregion ovalregistrykey
                                        break;

                                    case "name":
                                        if (sOVALObjectDataTypeName == "registry_object")
                                        {
                                            //<name>DisplayVersion</name>
                                            if (oWindowsRegistryKeyObject != null)    //Just to be sure
                                            {
                                                oWindowsRegistryKeyObject.Name = nodeOBJECTFIELD.InnerText;
                                                oWindowsRegistryKeyObject.comment = sOVALObjectComment;    //This registry key identifies the system root.
                                                //Regex for something?

                                                //TODO for compatibility with CybOX
                                                //REGISTRYVALUES


                                                oWindowsRegistryKeyObject.timestamp = DateTimeOffset.Now;
                                                windows_model.SaveChanges();
                                            }
                                            else
                                            {
                                                Console.WriteLine("ERROR: oWindowsRegistryKeyObject null for name");
                                            }
                                        }
                                        break;

                                    //TODO

                                    case "path":    //file_object
                                        //TODO check if file_object
                                        //<path var_ref="oval:org.mitre.oval:var:1346" var_check="all"/>
                                        //Done before
                                        /*
                                        try
                                        {
                                        
                                            string sOVALVariableIDPattern = nodeOBJECTFIELD.Attributes["var_ref"].InnerText;
                                            int iOVALVariableID = 0;
                                            try
                                            {
                                                iOVALVariableID = oval_model.OVALVARIABLE.FirstOrDefault(o => o.OVALVariableIDPattern == sOVALObjectVariableIDPattern).OVALVariableID;
                                            }
                                            catch(Exception ex)
                                            {
                                                //System.NullReferenceException
                                            }
                                        

                                        }
                                        catch(Exception exOVALObjectPath)
                                        {
                                            Console.WriteLine("Exception: exOVALObjectPath " + exOVALObjectPath.Message + " " + exOVALObjectPath.InnerException);
                                        }
                                        */
                                        break;


                                    case "filename":    //file_object
                                        //<filename>Ogl.dll</filename>
                                        #region OVALOBJECTFILE
                                        //FILE
                                        int iFileID = 0;
                                        string sFileName = nodeOBJECTFIELD.InnerText;
                                        string sFileNameLower = sFileName.ToLower();    //hlink.dll
                                        try
                                        {
                                            iFileID = model.FILE.Where(o => o.FileName == sFileNameLower).Select(o => o.FileID).FirstOrDefault();
                                        }
                                        catch (Exception ex)
                                        {

                                        }
                                        if (iFileID <= 0)
                                        {
                                            try
                                            {
                                                FILE oFile = new FILE();
                                                oFile.CreatedDate = DateTimeOffset.Now;
                                                oFile.FileName = sFileNameLower;
                                                oFile.VocabularyID = iVocabularyOVALID;
                                                oFile.timestamp = DateTimeOffset.Now;
                                                model.FILE.Add(oFile);
                                                model.SaveChanges();
                                                iFileID = oFile.FileID;
                                            }
                                            catch (Exception exFILE)
                                            {
                                                Console.WriteLine("Exception: exFILE " + exFILE.Message + " " + exFILE.InnerException);
                                            }
                                        }
                                        else
                                        {
                                            //Update FILE
                                        }
                                        //OVALOBJECTFILE

                                        int iOVALObjectFileID = 0;
                                        try
                                        {
                                            iOVALObjectFileID = oval_model.OVALOBJECTFILE.FirstOrDefault(o => o.OVALObjectID == myOVALObject.OVALObjectID && o.FileID == iFileID).OVALObjectFileID;
                                        }
                                        catch (Exception ex)
                                        {

                                        }
                                        if (iOVALObjectFileID <= 0)
                                        {
                                            try
                                            {
                                                OVALOBJECTFILE oOVALObjectFile = new OVALOBJECTFILE();
                                                oOVALObjectFile.OVALObjectID = myOVALObject.OVALObjectID;
                                                oOVALObjectFile.FileID = iFileID;
                                                oOVALObjectFile.OVALVariableID = iOVALVariableID; //Path
                                                oOVALObjectFile.CreatedDate = DateTimeOffset.Now;
                                                oOVALObjectFile.VocabularyID = iVocabularyOVALID;
                                                oOVALObjectFile.timestamp = DateTimeOffset.Now;
                                                oval_model.OVALOBJECTFILE.Add(oOVALObjectFile);
                                                oval_model.SaveChanges();    //TEST PERFORMANCE
                                                //iOVALObjectFileID=
                                            }
                                            catch (Exception exOVALOBJECTFILE)
                                            {
                                                Console.WriteLine("Exception: exOVALOBJECTFILE " + exOVALOBJECTFILE.Message + " " + exOVALOBJECTFILE.InnerException);
                                            }
                                        }
                                        else
                                        {
                                            //Update OVALOBJECTFILE
                                            //TODO something could have change (updated), e.g. the var_ref OVALVARIABLE Path
                                        }

                                        //TODO PRODUCTFILE

                                        #endregion OVALOBJECTFILE
                                        break;

                                    //TODO
                                    /*
                                    case "set":
                                        //OVALSET?
                                        <set>
                                            <object_reference>oval:org.mitre.oval:obj:15257</object_reference>
                                            <object_reference>oval:org.mitre.oval:obj:16406</object_reference>
                                            <filter action="include">oval:org.mitre.oval:ste:18296</filter>
                                        </set>
                                    

                                        break;
                                    */
                                    default:
                                        //TODO
                                        //base
                                        //swlist
                                        //swtype
                                        //area_patched
                                        //patch_base
                                        //patch_name
                                        //protocol
                                        //local_address
                                        //local_port
                                        //command
                                        //version
                                        //line (filename)
                                        //fmri
                                        Console.WriteLine("NOTE: Missing code for nodeOBJECTFIELD.Name=" + nodeOBJECTFIELD.Name);

                                        break;  //default
                                }
                            }
                        }


                    }
                }
                #endregion ovalobjects
            }

            if (bImportOVALStates)
            {
                #region ovalstates
                #region freememory
                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                Console.WriteLine("DEBUG FREE MEMORY");
                //FREE
                try
                {
                    oval_model.SaveChanges();
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
                    Console.WriteLine("Exception: DbEntityValidationExceptionGLOBALSAVE " + sb.ToString());
                }
                catch (Exception exGLOBALSAVE)
                {
                    Console.WriteLine("Exception: exGLOBALSAVE " + exGLOBALSAVE.Message + " " + exGLOBALSAVE.InnerException);
                }
                oval_model.Dispose();

                oval_model = new XOVALEntities();
                oval_model.Configuration.AutoDetectChangesEnabled = false;
                oval_model.Configuration.ValidateOnSaveEnabled = false;


                #endregion freememory

                XmlNodeList nodesStates = doc.SelectNodes("/oval-def:oval_definitions/oval-def:states", mgr);   //Hardcoded
                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                Console.WriteLine("IMPORTING OVALSTATES");
                foreach (XmlNode nodesState in nodesStates)
                {
                    //TODO
                    //oslevel_state
                    //fix_state
                    //registry_state
                    //States have been added before and so should all exist, but we need to verify the version
                    foreach (XmlNode nodeSTATE in nodesState)
                    {
                        Console.WriteLine("================================================================");
                        Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                        Console.WriteLine("DEBUG nodeSTATE.Name=" + nodeSTATE.Name);    //rpminfo_state
                        //TODO foreach attribute

                        //mask?

                        //version_state
                        //registry_state
                        string sStateDetailIDPattern = nodeSTATE.Attributes["id"].InnerText;    //oval:org.altx-soft.oval:ste:3
                        Console.WriteLine("DEBUG sStateDetailIDPattern=" + sStateDetailIDPattern);
                        //TODO Retrieve all the Attributes

                        int iStateDetailVersion = 0;
                        try
                        {
                            iStateDetailVersion = Convert.ToInt32(nodeSTATE.Attributes["version"].InnerText);
                        }
                        catch (Exception exiStateDetailVersion)
                        {
                            iStateDetailVersion = 0;
                            //Console.WriteLine("Exception: exiStateDetailVersion " + exiStateDetailVersion.Message + " " + exiStateDetailVersion.InnerException);
                        }

                        XOVALModel.OVALSTATE myOVALStateDetail = null;
                        //Search the OVALSTATE with the exact same version
                        #region findovalstate
                        IEnumerable<OVALSTATE> myOVALSTATES = oval_model.OVALSTATE.Where(o => o.OVALStateIDPattern == sStateDetailIDPattern);// && o.OVALStateVersion == Convert.ToInt32(sTestVersion));
                        int cptOVALSTATES = 0;
                        foreach (XOVALModel.OVALSTATE myOVALStateDetailSearch in myOVALSTATES)
                        {
                            cptOVALSTATES++;
                            if (myOVALStateDetailSearch.OVALStateVersion == iStateDetailVersion)
                            {
                                myOVALStateDetail = myOVALStateDetailSearch;
                                break;
                            }
                        }
                        if (myOVALStateDetail == null)
                        {
                            if (cptOVALSTATES == 1)
                            {
                                //We just have the one added before

                            }
                            foreach (XOVALModel.OVALSTATE myOVALStateDetailSearch in myOVALSTATES)
                            {
                                if (myOVALStateDetailSearch.OVALStateVersion == null)
                                {
                                    myOVALStateDetail = myOVALStateDetailSearch;
                                    break;
                                }
                            }
                        }
                        #endregion findovalstate

                        if (myOVALStateDetail == null)
                        {
                            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                            Console.WriteLine("ERROR: OVALState not found " + sStateDetailIDPattern);
                            //Note: this could happen if the OVALState was used in a set of an object
                            //e.g. oval:org.mitre.oval:ste:18296 in oval:org.mitre.oval:obj:15888
                            myOVALStateDetail = new OVALSTATE();
                            myOVALStateDetail.CreatedDate = DateTimeOffset.Now;
                        }
                        //else
                        //{
                        //Update OVALSTATE
                        Console.WriteLine("DEBUG OVALStateID=" + myOVALStateDetail.OVALStateID);

                        myOVALStateDetail.deprecated = false;
                        try
                        {
                            if (nodeSTATE.Attributes["deprecated"].InnerText == "true")
                            {
                                myOVALStateDetail.deprecated = true;
                            }

                        }
                        catch (Exception ex)
                        {
                            myOVALStateDetail.deprecated = false;
                        }

                        //myOVALStateDetail.OVALStateType = nodeSTATE.Name;   //Removed
                        //variable_state    registry_state
                        // OVALSTATETYPE
                        #region ovalstatetype
                        int iOVALStateTypeID = 0;
                        try
                        {
                            iOVALStateTypeID = oval_model.OVALSTATETYPE.Where(o => o.OVALStateTypeName == nodeSTATE.Name).Select(o => o.OVALStateTypeID).FirstOrDefault();
                        }
                        catch (Exception ex)
                        {

                        }
                        if (iOVALStateTypeID <= 0)
                        {
                            OVALSTATETYPE oOVALStateType = new OVALSTATETYPE();
                            oOVALStateType.CreatedDate = DateTimeOffset.Now;
                            oOVALStateType.OVALStateTypeName = nodeSTATE.Name;
                            oOVALStateType.VocabularyID = iVocabularyOVALID;
                            oOVALStateType.timestamp = DateTimeOffset.Now;
                            oval_model.OVALSTATETYPE.Add(oOVALStateType);
                            oval_model.SaveChanges();
                            iOVALStateTypeID = oOVALStateType.OVALStateTypeID;
                        }
                        else
                        {
                            //Update OVALSTATETYPE
                        }
                        myOVALStateDetail.OVALStateTypeID = iOVALStateTypeID;
                        #endregion ovalstatetype

                        myOVALStateDetail.OVALStateVersion = iStateDetailVersion;

                        try
                        {
                            //Version of NAV is less than 2012
                            myOVALStateDetail.comment = nodeSTATE.Attributes["comment"].InnerText;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                            Console.WriteLine("NOTE: OVALSTATE without comment");
                        }

                        //myOVALStateDetail.Namespace   //pixos-def xmlns="http://oval.mitre.org/XMLSchema/oval-definitions-5#pixos"
                        string sNameSpace = "";
                        string sXMLNS = nodeSTATE.Attributes["xmlns"].InnerText;
                        //TODO: do it more generic
                        if (sXMLNS.Contains("#aix")) sNameSpace = "aix-def";
                        if (sXMLNS.Contains("#apache")) sNameSpace = "apache-def";
                        if (sXMLNS.Contains("#catos")) sNameSpace = "catos-def";
                        if (sXMLNS.Contains("#esx")) sNameSpace = "esx-def";
                        if (sXMLNS.Contains("#freebsd")) sNameSpace = "freebsd-def";
                        if (sXMLNS.Contains("#hpux")) sNameSpace = "hpux-def";
                        if (sXMLNS.Contains("#independent")) sNameSpace = "ind-def";
                        if (sXMLNS.Contains("#ios")) sNameSpace = "ios-def";
                        if (sXMLNS.Contains("#linux")) sNameSpace = "linux-def";
                        if (sXMLNS.Contains("#macos")) sNameSpace = "macos-def";
                        if (sXMLNS.Contains("#pixos")) sNameSpace = "pixos-def";
                        if (sXMLNS.Contains("#sharepoint")) sNameSpace = "sp-def";
                        if (sXMLNS.Contains("#solaris")) sNameSpace = "sol-def";
                        if (sXMLNS.Contains("#unix")) sNameSpace = "unix-def";
                        if (sXMLNS.Contains("#windows")) sNameSpace = "win-def";
                        //TODO: mapping
                        if (sNameSpace == "")
                        {
                            Console.WriteLine("ERROR: Unknown STATE Namespace: " + sNameSpace);
                        }
                        else
                        {
                            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                            Console.WriteLine("DEBUG STATE Namespace=" + sNameSpace);
                        }

                        //myOVALStateDetail.Namespace = sNameSpace;   //Removed
                        #region ovalnamespace
                        // OVALNAMESPACE
                        int iOVALNameSpaceID = 0;
                        try
                        {
                            iOVALNameSpaceID = oval_model.OVALNAMESPACE.Where(o => o.OVALNamespaceName == sNameSpace).Select(o => o.OVALNamespaceID).FirstOrDefault();
                        }
                        catch (Exception ex)
                        {

                        }
                        if (iOVALNameSpaceID <= 0)
                        {
                            OVALNAMESPACE oOVALNameSpace = new OVALNAMESPACE();
                            oOVALNameSpace.CreatedDate = DateTimeOffset.Now;
                            oOVALNameSpace.OVALNamespaceName = sNameSpace;
                            oOVALNameSpace.VocabularyID = iVocabularyOVALID;
                            oOVALNameSpace.timestamp = DateTimeOffset.Now;
                            oval_model.OVALNAMESPACE.Add(oOVALNameSpace);
                            oval_model.SaveChanges();
                            iOVALNameSpaceID = oOVALNameSpace.OVALNamespaceID;
                        }
                        else
                        {
                            //Update OVALNAMESPACE
                        }
                        myOVALStateDetail.OVALNamespaceID = iOVALNameSpaceID;
                        #endregion ovalnamespace

                        #region operatorenumeration
                        try
                        {
                            //AND
                            string sOperatorValue = nodeSTATE.Attributes["operator"].InnerText;
                            //myOVALStateDetail.OperatorEnumerationValue=sOperatorValue;  //Removed

                            int iOperatorEnumerationID = 0;
                            try
                            {
                                iOperatorEnumerationID = oval_model.OPERATORENUMERATION.Where(o => o.OperatorValue == sOperatorValue).Select(o => o.OperatorEnumerationID).FirstOrDefault();
                            }
                            catch (Exception ex)
                            {

                            }
                            if (iOperatorEnumerationID <= 0)
                            {
                                XOVALModel.OPERATORENUMERATION oOperatorEnumeration = new XOVALModel.OPERATORENUMERATION();
                                oOperatorEnumeration.CreatedDate = DateTimeOffset.Now;
                                oOperatorEnumeration.OperatorValue = sOperatorValue;
                                oOperatorEnumeration.VocabularyID = iVocabularyOVALID;
                                oOperatorEnumeration.timestamp = DateTimeOffset.Now;
                                oval_model.OPERATORENUMERATION.Add(oOperatorEnumeration);
                                oval_model.SaveChanges();
                                iOperatorEnumerationID = oOperatorEnumeration.OperatorEnumerationID;
                            }
                            else
                            {
                                //Update OPERATORENUMERATION
                            }
                            myOVALStateDetail.OperatorEnumerationID = iOperatorEnumerationID;
                        }
                        catch (Exception ex)
                        {

                        }
                        #endregion operatorenumeration

                        myOVALStateDetail.timestamp = DateTimeOffset.Now;
                        oval_model.SaveChanges();
                        Console.WriteLine("DEBUG OVALSTATE Updated");

                        if (nodeSTATE.ChildNodes.Count > 0)
                        {
                            //We need a record
                            //<value datatype="version" operation="less than">19.0.0.0</value>
                            XOVALModel.OVALSTATERECORD myOVALStateRecord;
                            XOVALModel.OVALSTATERECORDFOROVALSTATE myOVALStateRecordForOVALState;
                            myOVALStateRecordForOVALState = oval_model.OVALSTATERECORDFOROVALSTATE.FirstOrDefault(o => o.OVALStateID == myOVALStateDetail.OVALStateID);
                            if (myOVALStateRecordForOVALState == null)
                            {
                                //TODO foreach Attributes
                                //TODO
                                //OVALSTATECOMPLEXBASE
                                myOVALStateRecord = new OVALSTATERECORD();
                                string sStateDataTypeName = nodeSTATE.Name;
                                //version_state line_state  variable_state  registry_state
                                //myOVALStateRecord.DataTypeName = sStateDataTypeName;    //Removed
                                myOVALStateRecord.OVALStateTypeID = iOVALStateTypeID;

                                //TODO
                                //Operation?
                                //...
                                //mask?

                                //myOVALStateRecord.Namespace = sNameSpace;      //Removed
                                //pixos-def
                                myOVALStateRecord.OVALNamespaceID = iOVALNameSpaceID;

                                myOVALStateRecord.VocabularyID = iVocabularyOVALID;
                                myOVALStateRecord.CreatedDate = DateTimeOffset.Now;
                                myOVALStateRecord.timestamp = DateTimeOffset.Now;
                                try
                                {
                                    oval_model.OVALSTATERECORD.Add(myOVALStateRecord);
                                    oval_model.SaveChanges();
                                }
                                catch (Exception exAddToOVALSTATERECORD)
                                {
                                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                    Console.WriteLine("Exception: exAddToOVALSTATERECORD " + exAddToOVALSTATERECORD.Message + " " + exAddToOVALSTATERECORD.InnerException);
                                }


                                try
                                {
                                    myOVALStateRecordForOVALState = new OVALSTATERECORDFOROVALSTATE();
                                    myOVALStateRecordForOVALState.OVALStateID = myOVALStateDetail.OVALStateID;
                                    myOVALStateRecordForOVALState.OVALStateRecordID = myOVALStateRecord.OVALStateRecordID;
                                    myOVALStateRecordForOVALState.VocabularyID = iVocabularyOVALID;
                                    myOVALStateRecordForOVALState.CreatedDate = DateTimeOffset.Now;
                                    myOVALStateRecordForOVALState.timestamp = DateTimeOffset.Now;
                                    oval_model.OVALSTATERECORDFOROVALSTATE.Add(myOVALStateRecordForOVALState);
                                    oval_model.SaveChanges();
                                }
                                catch (Exception exAddToOVALSTATERECORDFOROVALSTATE)
                                {
                                    Console.WriteLine("Exception: exAddToOVALSTATERECORDFOROVALSTATE " + exAddToOVALSTATERECORDFOROVALSTATE.Message + " " + exAddToOVALSTATERECORDFOROVALSTATE.InnerException);
                                }
                            }
                            else
                            {
                                //Update OVALSTATERECORDFOROVALSTATE
                                myOVALStateRecord = myOVALStateRecordForOVALState.OVALSTATERECORD;  //TODO Review needed?


                            }
                            //Update OVALSTATERECORDFOROVALSTATE
                            myOVALStateRecordForOVALState.timestamp = DateTimeOffset.Now;


                            foreach (XmlNode nodeSTATEFIELD in nodeSTATE)
                            {
                                /*
                                <value operation="pattern match">
                                ^The 2007 Microsoft Office Servers Service Pack 2 \(SP2\)( [\w\D]+)?$
                                </value>
                                */
                                //<release datatype="version" operation="less than">1.6</release>

                                //TODO
                                //We assume that we have only one OVALSTATEFIELD for one DataType for one OVALSTATERECORD
                                XOVALModel.OVALSTATEFIELD myOVALStateField;
                                Boolean bAddOvalStateField = false;
                                XOVALModel.OVALSTATEFIELDFOROVALSTATERECORD myOVALStateFieldForOVALStateRecord;
                                Boolean bAddOvalStateFieldForOvalStateRecord = false;
                                myOVALStateFieldForOVALStateRecord = oval_model.OVALSTATEFIELDFOROVALSTATERECORD.FirstOrDefault(o => o.OVALStateRecordID == myOVALStateRecord.OVALStateRecordID && o.OVALSTATEFIELD.FieldName == nodeSTATEFIELD.Name);
                                if (myOVALStateFieldForOVALStateRecord == null)
                                {
                                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                    Console.WriteLine("DEBUG Adding OVALSTATEFIELDFOROVALSTATERECORD");
                                    bAddOvalStateFieldForOvalStateRecord = true;
                                    myOVALStateFieldForOVALStateRecord = new OVALSTATEFIELDFOROVALSTATERECORD();

                                    //We add a OVALSTATEFIELD
                                    myOVALStateField = new OVALSTATEFIELD();
                                    myOVALStateField.CreatedDate = DateTimeOffset.Now;
                                    myOVALStateField.VocabularyID = iVocabularyOVALID;
                                    bAddOvalStateField = true;


                                }
                                else
                                {
                                    //Update OVALSTATEFIELDFOROVALSTATERECORD
                                    myOVALStateField = myOVALStateFieldForOVALStateRecord.OVALSTATEFIELD;
                                }

                                //TODO
                                //OVALENTITYATTRIBUTEGROUP
                                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                Console.WriteLine("DEBUG myOVALStateField.FieldName=" + nodeSTATEFIELD.Name);   //value release evr

                                //Update OVALSTATEFIELD
                                myOVALStateField.FieldName = nodeSTATEFIELD.Name;    //pix_release   show_subcommand config_line
                                //TODO
                                //deprecated?
                                //mask?
                                myOVALStateField.DataTypeName = "string";   //Hardcoded default
                                //version
                                try
                                {
                                    Console.WriteLine("DEBUG myOVALStateField.DataTypeName=" + nodeSTATEFIELD.Attributes["datatype"].InnerText);
                                    //Update OVALSTATEFIELD
                                    myOVALStateField.DataTypeName = nodeSTATEFIELD.Attributes["datatype"].InnerText;    //int   version
                                    //TODO? use a table

                                }
                                catch (Exception ex)
                                {

                                }

                                try
                                {
                                    //for variable_state
                                    string sOVALVariableIDPattern = nodeSTATEFIELD.Attributes["var_ref"].InnerText;
                                    //Update OVALSTATEFIELD
                                    //myOVALStateField.VarRef = sOVALVariableIDPattern;   // REMOVED
                                    //TODO REVIEW

                                    #region ovalvariable
                                    //XORCISMModel.OVALVARIABLE myOVALVariable;
                                    //myOVALVariable = oval_model.OVALVARIABLE.FirstOrDefault(o => o.OVALVariableIDPattern == sOVALVariableIDPattern);
                                    int iOVALVariableID = 0;
                                    try
                                    {
                                        iOVALVariableID = oval_model.OVALVARIABLE.FirstOrDefault(o => o.OVALVariableIDPattern == sOVALVariableIDPattern).OVALVariableID;
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                    //if (myOVALVariable == null)
                                    if (iOVALVariableID <= 0)
                                    {
                                        Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                        Console.WriteLine("DEBUG Adding OVALVariable " + sOVALVariableIDPattern + " for OVALSTATEFIELD");

                                        try
                                        {
                                            OVALVARIABLE myOVALVariable = new OVALVARIABLE();
                                            myOVALVariable.OVALVariableIDPattern = sOVALVariableIDPattern;
                                            //myOVALVariable.DataTypeName = "string"; //Hardcoded default   //Removed
                                            #region OVALVARIABLEDATATYPE
                                            int iOVALVariableDataTypeID = 0;
                                            try
                                            {
                                                iOVALVariableDataTypeID = oval_model.OVALVARIABLEDATATYPE.Where(o => o.OVALVariableDataTypeName == "string").Select(o => o.OVALVariableDataTypeID).FirstOrDefault();
                                            }
                                            catch (Exception ex)
                                            {

                                            }
                                            if (iOVALVariableDataTypeID <= 0)
                                            {
                                                OVALVARIABLEDATATYPE oOVALVariableDataType = new OVALVARIABLEDATATYPE();
                                                oOVALVariableDataType.CreatedDate = DateTimeOffset.Now;
                                                oOVALVariableDataType.OVALVariableDataTypeName = "string";
                                                oOVALVariableDataType.VocabularyID = iVocabularyOVALID;
                                                oOVALVariableDataType.timestamp = DateTimeOffset.Now;
                                                oval_model.OVALVARIABLEDATATYPE.Add(oOVALVariableDataType);
                                                oval_model.SaveChanges();
                                                iOVALVariableDataTypeID = oOVALVariableDataType.OVALVariableDataTypeID;
                                            }
                                            else
                                            {
                                                //Update OVALVARIABLEDATATYPE
                                            }
                                            #endregion OVALVARIABLEDATATYPE
                                            myOVALVariable.OVALVariableDataTypeID = iOVALVariableDataTypeID;

                                            myOVALVariable.OVALVariableVersion = 1; //Hardcoded
                                            myOVALVariable.comment = "";
                                            myOVALVariable.VocabularyID = iVocabularyOVALID;
                                            myOVALVariable.CreatedDate = DateTimeOffset.Now;
                                            myOVALVariable.timestamp = DateTimeOffset.Now;
                                            oval_model.OVALVARIABLE.Add(myOVALVariable);
                                            oval_model.SaveChanges();
                                            iOVALVariableID = myOVALVariable.OVALVariableID;
                                        }
                                        catch (Exception exAddToOVALVARIABLE)
                                        {
                                            Console.WriteLine("Exception: exAddToOVALVARIABLE " + exAddToOVALVARIABLE.Message + " " + exAddToOVALVARIABLE.InnerException);
                                        }
                                    }
                                    else
                                    {
                                        //Update OVALVARIABLE
                                    }
                                    #endregion ovalvariable
                                    //Update OVALSTATEFIELD
                                    myOVALStateField.OVALVariableID = iOVALVariableID;  // myOVALVariable.OVALVariableID;
                                }
                                catch (Exception ex)
                                {

                                }

                                //Update OVALSTATEFIELD
                                string sOperationValue = "equals"; //Hardcoded default
                                try
                                {
                                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                    sOperationValue = nodeSTATEFIELD.Attributes["operation"].InnerText;    //less than


                                }
                                catch (Exception ex)
                                {
                                    sOperationValue = "equals"; //Hardcoded default
                                }
                                //myOVALStateField.OperationValue = sOperationValue;  //Removed
                                Console.WriteLine("DEBUG myOVALStateField.OperationValue=" + sOperationValue);

                                #region operationenumeration
                                int iOperationEnumerationID = 0;
                                try
                                {
                                    iOperationEnumerationID = model.OPERATIONENUMERATION.Where(o => o.OperationValue == sOperationValue).Select(o => o.OperationEnumerationID).FirstOrDefault();
                                }
                                catch (Exception ex)
                                {

                                }
                                if (iOperationEnumerationID <= 0)
                                {
                                    OPERATIONENUMERATION oOperationEnumeration = new OPERATIONENUMERATION();
                                    //oOperationEnumeration.CreatedDate=
                                    oOperationEnumeration.OperationValue = sOperationValue;
                                    oOperationEnumeration.VocabularyID = iVocabularyOVALID;
                                    //oOperationEnumeration.timestamp=
                                    model.OPERATIONENUMERATION.Add(oOperationEnumeration);
                                    model.SaveChanges();
                                    iOperationEnumerationID = oOperationEnumeration.OperationEnumerationID;
                                }
                                else
                                {

                                }
                                myOVALStateField.OperationEnumerationID = iOperationEnumerationID;
                                #endregion operationenumeration
                                //pattern match
                                //TODO REGEX
                                myOVALStateField.OperationEnumerationID = iOperationEnumerationID;

                                try
                                {
                                    string sEnumerationValue = nodeSTATEFIELD.Attributes["entity_check"].InnerText;
                                    //default?
                                    //myOVALStateField.EnumerationValue = sEnumerationValue;   //Removed
                                    //all
                                    #region checkenumeration
                                    int iCheckEnumerationID = 0;
                                    try
                                    {
                                        iCheckEnumerationID = model.CHECKENUMERATION.Where(o => o.EnumerationValue == sEnumerationValue).Select(o => o.CheckEnumerationID).FirstOrDefault();
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                    if (iCheckEnumerationID <= 0)
                                    {
                                        CHECKENUMERATION oCheckEnumeration = new CHECKENUMERATION();
                                        oCheckEnumeration.CreatedDate = DateTimeOffset.Now;
                                        oCheckEnumeration.EnumerationValue = sEnumerationValue;
                                        oCheckEnumeration.VocabularyID = iVocabularyOVALID;
                                        oCheckEnumeration.timestamp = DateTimeOffset.Now;
                                        model.CHECKENUMERATION.Add(oCheckEnumeration);
                                        model.SaveChanges();
                                        iCheckEnumerationID = oCheckEnumeration.CheckEnumerationID;
                                    }
                                    else
                                    {
                                        //Update CHECKENUMERATION
                                    }
                                    #endregion checkenumeration
                                    myOVALStateField.CheckEnumerationID = iCheckEnumerationID;
                                }
                                catch (Exception ex)
                                {

                                }

                                myOVALStateField.FieldValue = nodeSTATEFIELD.InnerText; //19.0.0.0
                                try
                                {
                                    if (bAddOvalStateField)
                                    {
                                        oval_model.OVALSTATEFIELD.Add(myOVALStateField);
                                    }
                                    else
                                    {
                                        //Update OVALSTATEFIELD
                                    }
                                    //Update OVALSTATEFIELD
                                    //myOVALStateField.Namespace = sNameSpace;    //Removed
                                    myOVALStateField.OVALNamespaceID = iOVALNameSpaceID;
                                    myOVALStateField.VocabularyID = iVocabularyOVALID;
                                    myOVALStateField.timestamp = DateTimeOffset.Now;
                                    oval_model.SaveChanges();
                                }
                                catch (Exception exAddToOVALSTATEFIELD)
                                {
                                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                    Console.WriteLine("Exception: exAddToOVALSTATEFIELD " + exAddToOVALSTATEFIELD.Message + " " + exAddToOVALSTATEFIELD.InnerException);
                                }

                                //Update OVALSTATEFIELDFOROVALSTATERECORD
                                myOVALStateFieldForOVALStateRecord.OVALStateRecordID = myOVALStateRecord.OVALStateRecordID;
                                myOVALStateFieldForOVALStateRecord.OVALStateFieldID = myOVALStateField.OVALStateFieldID;
                                myOVALStateFieldForOVALStateRecord.timestamp = DateTimeOffset.Now;
                                try
                                {
                                    if (bAddOvalStateFieldForOvalStateRecord)
                                    {
                                        myOVALStateFieldForOVALStateRecord.CreatedDate = DateTimeOffset.Now;
                                        myOVALStateFieldForOVALStateRecord.VocabularyID = iVocabularyOVALID;
                                        oval_model.OVALSTATEFIELDFOROVALSTATERECORD.Add(myOVALStateFieldForOVALStateRecord);
                                    }
                                    else
                                    {
                                        //Update OVALSTATEFIELDFOROVALSTATERECORD
                                    }

                                    oval_model.SaveChanges();
                                }
                                catch (Exception exAddToOVALSTATEFIELDFOROVALSTATERECORD)
                                {
                                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                    Console.WriteLine("Exception: exAddToOVALSTATEFIELDFOROVALSTATERECORD " + exAddToOVALSTATEFIELDFOROVALSTATERECORD.Message + " " + exAddToOVALSTATEFIELDFOROVALSTATERECORD.InnerException);
                                }


                                //TODO We can have more info
                                //<field xmlns="http://oval.mitre.org/XMLSchema/oval-definitions-5" name="hotfixid" operation="case insensitive equals">KB2686509</field>
                                //in
                                /*
                                <wmi57_state xmlns="http://oval.mitre.org/XMLSchema/oval-definitions-5#windows" id="oval:org.mitre.oval:ste:19636" version="2" comment="The field hotfixid in the WMI request equals KB2686509">
                                    <result datatype="record" entity_check="at least one">
                                        <field xmlns="http://oval.mitre.org/XMLSchema/oval-definitions-5" name="hotfixid" operation="case insensitive equals">KB2686509</field>
                                    </result>
                                </wmi57_state>
                                */
                                foreach (XmlNode nodeSTATEFIELDFIELD in nodeSTATEFIELD)
                                {
                                    if (nodeSTATEFIELDFIELD.Name != "#text")
                                    {
                                        Console.WriteLine("ERROR: Missing code for nodeSTATEFIELDFIELD " + nodeSTATEFIELDFIELD.Name);
                                        //TODO
                                        //field
                                    }
                                }


                            }
                        }

                        //}
                    }
                }
                #endregion ovalstates
            }

            if (bImportOVALVariables)
            {
                #region ovalvariables
                #region freememory
                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                Console.WriteLine("DEBUG FREE MEMORY");
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
                    Console.WriteLine("Exception: DbEntityValidationExceptionGLOBALSAVE " + sb.ToString());
                }
                catch (Exception exGLOBALSAVE)
                {
                    Console.WriteLine("Exception: exGLOBALSAVE " + exGLOBALSAVE.Message + " " + exGLOBALSAVE.InnerException);
                }
                model.Dispose();

                model = new XORCISMEntities();
                model.Configuration.AutoDetectChangesEnabled = false;
                model.Configuration.ValidateOnSaveEnabled = false;
                #endregion freememory

                XmlNodeList nodesVariables = doc.SelectNodes("/oval-def:oval_definitions/oval-def:variables", mgr); //Hardcoded
                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                Console.WriteLine("IMPORTING OVALVARIABLES");
                foreach (XmlNode nodesVariable in nodesVariables)
                {

                    foreach (XmlNode nodeVARIABLE in nodesVariable)
                    {
                        //TODO: Retrieve all the Attributes
                        string sOVALVariableIDPattern = nodeVARIABLE.Attributes["id"].InnerText;
                        Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                        Console.WriteLine("DEBUG sOVALVariableIDPattern=" + sOVALVariableIDPattern);

                        XOVALModel.OVALVARIABLE myOVALVariable;
                        myOVALVariable = oval_model.OVALVARIABLE.FirstOrDefault(o => o.OVALVariableIDPattern == sOVALVariableIDPattern); //&& o.OVALVariableVersion ==
                        if (myOVALVariable == null)
                        {
                            Console.WriteLine("ERROR: OVALVariable not found " + sOVALVariableIDPattern);

                        }
                        else
                        {
                            //Update OVALVARIABLE
                            /*
                            //TODO: review this type of variable
                            switch (nodeVARIABLE.Name)
                            {
                                case "local_variable":

                                    break;
                                case "external_variable":

                                    break;
                                case "constant_variable":

                                    break;
                                default:
                                    Console.WriteLine("ERROR: Unknown OVALVariable type " + nodeVARIABLE.Name);
                                    break;
                            }
                            */

                            int iOVALVersion = Convert.ToInt32(nodeVARIABLE.Attributes["version"].InnerText);
                            myOVALVariable.OVALVariableVersion = iOVALVersion;

                            try
                            {
                                string sComment = nodeVARIABLE.Attributes["comment"].InnerText;
                                myOVALVariable.comment = sComment;
                                Console.WriteLine("DEBUG OVALVARIABLE.comment=" + sComment);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("NOTE: no comment for OVALVARIABLE");
                            }

                            string sOVALVariableDataTypeName = "string";    //Hardcoded default

                            //Removed
                            try
                            {
                                sOVALVariableDataTypeName = nodeVARIABLE.Attributes["datatype"].InnerText;
                                //evr_string    float
                            }
                            catch (Exception ex)
                            {
                                //myOVALVariable.DataTypeName = "string"; //default //Removed
                                sOVALVariableDataTypeName = "string";    //Hardcoded default
                            }

                            //myOVALVariable.DataTypeName = sOVALVariableDataTypeName; //Removed
                            //TODO? DATATYPE, SIMPLEDATATYPE
                            #region OVALVARIABLEDATATYPE
                            int iOVALVariableDataTypeID = 0;
                            try
                            {
                                iOVALVariableDataTypeID = oval_model.OVALVARIABLEDATATYPE.Where(o => o.OVALVariableDataTypeName == sOVALVariableDataTypeName).Select(o => o.OVALVariableDataTypeID).FirstOrDefault();
                            }
                            catch (Exception ex)
                            {

                            }
                            if (iOVALVariableDataTypeID <= 0)
                            {
                                try
                                {
                                    OVALVARIABLEDATATYPE oOVALVariableDataType = new OVALVARIABLEDATATYPE();
                                    oOVALVariableDataType.CreatedDate = DateTimeOffset.Now;
                                    oOVALVariableDataType.OVALVariableDataTypeName = sOVALVariableDataTypeName;
                                    oOVALVariableDataType.VocabularyID = iVocabularyOVALID;
                                    oOVALVariableDataType.timestamp = DateTimeOffset.Now;
                                    oval_model.OVALVARIABLEDATATYPE.Add(oOVALVariableDataType);
                                    oval_model.SaveChanges();
                                    iOVALVariableDataTypeID = oOVALVariableDataType.OVALVariableDataTypeID;
                                }
                                catch(Exception exOVALVariableDataType)
                                {
                                    Console.WriteLine("Exception exOVALVariableDataType " + exOVALVariableDataType.Message + " " + exOVALVariableDataType.InnerException);
                                }
                            }
                            else
                            {
                                //Update OVALVARIABLEDATATYPE
                            }
                            #endregion OVALVARIABLEDATATYPE
                            myOVALVariable.OVALVariableDataTypeID = iOVALVariableDataTypeID;

                            try
                            {
                                string sVariableDeprecated = nodeVARIABLE.Attributes["deprecated"].InnerText;
                                if (sVariableDeprecated.ToLower() == "true")
                                {
                                    myOVALVariable.deprecated = true;
                                }
                            }
                            catch (Exception ex)
                            {

                            }

                            //TODO Review, Hardcode
                            //myOVALVariable.Namespace = "oval-def"; //oval-def   //TODO HARDCODED    Removed
                            #region OVALNAMESPACE
                            int iOVALNamespaceID = 0;
                            try
                            {
                                iOVALNamespaceID = oval_model.OVALNAMESPACE.Where(o => o.OVALNamespaceName == "oval-def").Select(o => o.OVALNamespaceID).FirstOrDefault();
                            }
                            catch (Exception ex)
                            {

                            }
                            if (iOVALNamespaceID <= 0)
                            {
                                OVALNAMESPACE oOVALNamespace = new OVALNAMESPACE();
                                oOVALNamespace.CreatedDate = DateTimeOffset.Now;
                                oOVALNamespace.OVALNamespaceName = "oval-def";  //Hardcoded
                                oOVALNamespace.VocabularyID = iVocabularyOVALID;
                                oOVALNamespace.timestamp = DateTimeOffset.Now;
                                oval_model.OVALNAMESPACE.Add(oOVALNamespace);
                                oval_model.SaveChanges();
                                iOVALNamespaceID = oOVALNamespace.OVALNamespaceID;
                            }
                            else
                            {
                                //Update OVALNAMESPACE
                            }
                            #endregion OVALNAMESPACE
                            myOVALVariable.OVALNamespaceID = iOVALNamespaceID;

                            string sOVALVariableTypeName = nodeVARIABLE.Name;
                            //local_variable
                            //external_variable
                            //constant_variable
                            //myOVALVariable.VariableType = sOVALVariableTypeName;    //Removed
                            //TODO: could be hardcoded for performance
                            #region OVALVARIABLETYPE
                            int iOVALVariableTypeID = 0;
                            //Note: see also DATATYPE, SIMPLEDATATYPE
                            try
                            {
                                iOVALVariableTypeID = oval_model.OVALVARIABLETYPE.Where(o => o.OVALVariableTypeName == sOVALVariableTypeName).Select(o => o.OVALVariableTypeID).FirstOrDefault();
                            }
                            catch (Exception ex)
                            {

                            }
                            if (iOVALVariableTypeID <= 0)
                            {
                                OVALVARIABLETYPE oOVALVariableType = new OVALVARIABLETYPE();
                                oOVALVariableType.CreatedDate = DateTimeOffset.Now;
                                oOVALVariableType.OVALVariableTypeName = sOVALVariableTypeName;
                                oOVALVariableType.VocabularyID = iVocabularyOVALID;
                                oOVALVariableType.timestamp = DateTimeOffset.Now;
                                oval_model.OVALVARIABLETYPE.Add(oOVALVariableType);
                                oval_model.SaveChanges();
                                iOVALVariableTypeID = oOVALVariableType.OVALVariableTypeID;
                            }
                            else
                            {
                                //Update OVALVARIABLETYPE
                            }
                            #endregion OVALVARIABLETYPE
                            myOVALVariable.OVALVariableTypeID = iOVALVariableTypeID;

                            myOVALVariable.timestamp = DateTimeOffset.Now;
                            try
                            {
                                oval_model.OVALVARIABLE.Attach(myOVALVariable);
                                oval_model.Entry(myOVALVariable).State = EntityState.Modified;
                                oval_model.SaveChanges();
                                Console.WriteLine("DEBUG OVALVARIABLE Updated");
                            }
                            catch (Exception exOVALVARIABLE)
                            {
                                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                Console.WriteLine("Exception: exOVALVARIABLE " + exOVALVARIABLE.Message + " " + exOVALVARIABLE.InnerException);
                            }

                            //TODO
                            //OVALCOMPONENTGROUP
                            foreach (XmlNode nodeVARIABLEDETAIL in nodeVARIABLE)
                            {
                                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                Console.WriteLine("DEBUG nodeVARIABLEDETAIL=" + nodeVARIABLEDETAIL.Name);

                                //TODO Review (always needed? e.g. object_component)
                                //XORCISMModel.OVALCOMPONENTGROUP myOVALComponentGroup;
                                //myOVALComponentGroup = oval_model.OVALCOMPONENTGROUP.FirstOrDefault(o => o.OVALVariableID == myOVALVariable.OVALVariableID);
                                #region OVALCOMPONENTGROUP
                                int iOVALComponentGroupID = 0;
                                try
                                {
                                    iOVALComponentGroupID = oval_model.OVALCOMPONENTGROUP.Where(o => o.OVALVariableID == myOVALVariable.OVALVariableID).Select(o => o.OVALComponentGroupID).FirstOrDefault();
                                }
                                catch (Exception ex)
                                {
                                    iOVALComponentGroupID = 0;
                                }

                                //if (myOVALComponentGroup == null)
                                if (iOVALComponentGroupID <= 0)
                                {
                                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                    Console.WriteLine("DEBUG Adding a OVALCOMPONENTGROUP for OVALVARIABLE " + myOVALVariable.OVALVariableID);

                                    try
                                    {
                                        OVALCOMPONENTGROUP myOVALComponentGroup = new OVALCOMPONENTGROUP();
                                        myOVALComponentGroup.OVALVariableID = myOVALVariable.OVALVariableID;    //We attach the OVALCOMPONENTGROUP to the OVALVARIABLE
                                        myOVALComponentGroup.FunctionName = nodeVARIABLEDETAIL.Name;    //concat    //TODO: review
                                        myOVALComponentGroup.VocabularyID = iVocabularyOVALID;
                                        myOVALComponentGroup.CreatedDate = DateTimeOffset.Now;
                                        //TODO Review
                                        string sFunctionOperation = "equals";   //Default Hardcoded
                                        try
                                        {
                                            sFunctionOperation = nodeVARIABLE.Attributes["arithmetic_operation"].InnerText; //Hardcoded default     add   multiply
                                        }
                                        catch (Exception ex)
                                        {

                                        }
                                        myOVALComponentGroup.FunctionOperation = sFunctionOperation;
                                        //TODO? use a table (OVALFUNCTION)
                                        //Note: tables OVALFUNCTIONGROUP removed
                                        /*
                                        }
                                        catch (Exception ex)
                                        {

                                        }

                                        //DATETIME Format1 Format2
                                        try
                                        {
                                        */
                                        myOVALComponentGroup.timestamp = DateTimeOffset.Now;
                                        oval_model.OVALCOMPONENTGROUP.Attach(myOVALComponentGroup);
                                        oval_model.Entry(myOVALComponentGroup).State = EntityState.Modified;
                                        oval_model.OVALCOMPONENTGROUP.Add(myOVALComponentGroup);
                                        oval_model.SaveChanges();
                                        iOVALComponentGroupID = myOVALComponentGroup.OVALComponentGroupID;
                                    }
                                    catch (Exception exAddToOVALCOMPONENTGROUP)
                                    {
                                        Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                        Console.WriteLine("Exception: exAddToOVALCOMPONENTGROUP " + exAddToOVALCOMPONENTGROUP.Message + " " + exAddToOVALCOMPONENTGROUP.InnerException);
                                    }
                                }
                                else
                                {
                                    //Update OVALCOMPONENTGROUP
                                    //FunctionOperation
                                }
                                #endregion OVALCOMPONENTGROUP
                                Console.WriteLine("DEBUG iOVALComponentGroupID=" + iOVALComponentGroupID);

                                switch (nodeVARIABLEDETAIL.Name)
                                {
                                    //TODO
                                    //OVALFUNCTIONGROUP
                                    //OVALFUNCTIONGROUPFOROVALCOMPONENTGROUP
                                    //OVALCOMPONENTGROUPFOR*FUNCTION
                                    case "concat":
                                    case "arithmetic":
                                        #region concat
                                        //OVALCONCATFUNCTION


                                        foreach (XmlNode nodeVARIABLEDETAILCOMPONENT in nodeVARIABLEDETAIL)
                                        {
                                            //TODO
                                            switch (nodeVARIABLEDETAILCOMPONENT.Name)
                                            {
                                                case "literal_component":
                                                    /*
                                                    <literal_component>
                                                        \\winsxs\\(x86|amd64)_microsoft\.windows\.gdiplus_6595b64144ccf1df_.+$|\\WinSxS\\(x86|amd64)_Microsoft\.Windows\.GdiPlus_6595b64144ccf1df_.+$
                                                    </literal_component>
                                                    */
                                                    #region ovalliteralcomponent
                                                    //string sDataType = "string";    //Hardcoded default
                                                    //TODO Review
                                                    #region SIMPLEDATATYPE
                                                    int iSimpleTypeID = 0;
                                                    try
                                                    {
                                                        iSimpleTypeID = model.SIMPLEDATATYPE.Where(o => o.DataTypeName == "string").Select(o => o.SimpleDataTypeID).FirstOrDefault();
                                                    }
                                                    catch (Exception ex)
                                                    {

                                                    }
                                                    if (iSimpleTypeID <= 0)
                                                    {
                                                        SIMPLEDATATYPE oSimpleDataType = new SIMPLEDATATYPE();
                                                        //oSimpleDataType.CreatedDate=
                                                        oSimpleDataType.DataTypeName = "string";    //Hardcoded
                                                        oSimpleDataType.VocabularyID = iVocabularyOVALID;
                                                        //oSimpleDataType.timestamp=
                                                        model.SIMPLEDATATYPE.Add(oSimpleDataType);
                                                        model.SaveChanges();
                                                        iSimpleTypeID = oSimpleDataType.SimpleDataTypeID;
                                                    }
                                                    else
                                                    {
                                                        //Update SIMPLEDATATYPE
                                                    }
                                                    #endregion SIMPLEDATATYPE

                                                    string sLiteralComponentValue = nodeVARIABLEDETAILCOMPONENT.InnerText;
                                                    //XORCISMModel.OVALLITERALCOMPONENT myOVALLiteralComponent;
                                                    //myOVALLiteralComponent = oval_model.OVALLITERALCOMPONENT.FirstOrDefault(o => o.DataTypeName == sDataType && o.LiteralComponentValue == sLiteralComponentValue);
                                                    int iOVALLiteralComponentID = 0;
                                                    try
                                                    {
                                                        iOVALLiteralComponentID = oval_model.OVALLITERALCOMPONENT.Where(o => o.SimpleDataTypeID == iSimpleTypeID && o.LiteralComponentValue == sLiteralComponentValue).Select(o => o.OVALLiteralComponentID).FirstOrDefault();
                                                    }
                                                    catch (Exception ex)
                                                    {

                                                    }
                                                    //if (myOVALLiteralComponent == null)
                                                    if (iOVALLiteralComponentID <= 0)
                                                    {
                                                        Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                        Console.WriteLine("DEBUG Adding OVALLITERALCOMPONENT " + sLiteralComponentValue);

                                                        try
                                                        {
                                                            OVALLITERALCOMPONENT myOVALLiteralComponent = new OVALLITERALCOMPONENT();
                                                            myOVALLiteralComponent.CreatedDate = DateTimeOffset.Now;
                                                            //myOVALLiteralComponent.DataTypeName = sDataType;    //Removed
                                                            //TODO (Hardcode for performance)
                                                            //myOVALLiteralComponent.SimpleDataTypeID = 10  //Hardcoded string

                                                            myOVALLiteralComponent.SimpleDataTypeID = iSimpleTypeID;

                                                            myOVALLiteralComponent.LiteralComponentValue = sLiteralComponentValue;
                                                            myOVALLiteralComponent.VocabularyID = iVocabularyOVALID;
                                                            myOVALLiteralComponent.timestamp = DateTimeOffset.Now;
                                                            oval_model.OVALLITERALCOMPONENT.Add(myOVALLiteralComponent);
                                                            oval_model.SaveChanges();
                                                            iOVALLiteralComponentID = myOVALLiteralComponent.OVALLiteralComponentID;
                                                        }
                                                        catch (Exception exAddToOVALLITERALCOMPONENT)
                                                        {
                                                            Console.WriteLine("Exception: exAddToOVALLITERALCOMPONENT " + exAddToOVALLITERALCOMPONENT.Message + " " + exAddToOVALLITERALCOMPONENT.InnerException);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //Update OVALLITERALCOMPONENT
                                                    }

                                                    //OVALLITERALCOMPONENTFOROVALCOMPONENTGROUP
                                                    //XORCISMModel.OVALLITERALCOMPONENTFOROVALCOMPONENTGROUP myOVALLiteralComponentForOVALComponentGroup;
                                                    //myOVALLiteralComponentForOVALComponentGroup = oval_model.OVALLITERALCOMPONENTFOROVALCOMPONENTGROUP.FirstOrDefault(o => o.OVALComponentGroupID == myOVALComponentGroup.OVALComponentGroupID && o.OVALLiteralComponentID == myOVALLiteralComponent.OVALLiteralComponentID);
                                                    int iOVALComponentGroupLiteralComponentID = 0;
                                                    try
                                                    {
                                                        iOVALComponentGroupLiteralComponentID = oval_model.OVALLITERALCOMPONENTFOROVALCOMPONENTGROUP.Where(o => o.OVALComponentGroupID == iOVALComponentGroupID && o.OVALLiteralComponentID == iOVALLiteralComponentID).Select(o => o.OVALComponentGroupLiteralComponentID).FirstOrDefault();
                                                    }
                                                    catch (Exception ex)
                                                    {

                                                    }
                                                    //if (myOVALLiteralComponentForOVALComponentGroup == null)
                                                    if (iOVALComponentGroupLiteralComponentID <= 0)
                                                    {

                                                        try
                                                        {
                                                            OVALLITERALCOMPONENTFOROVALCOMPONENTGROUP myOVALLiteralComponentForOVALComponentGroup = new OVALLITERALCOMPONENTFOROVALCOMPONENTGROUP();
                                                            myOVALLiteralComponentForOVALComponentGroup.CreatedDate = DateTimeOffset.Now;
                                                            myOVALLiteralComponentForOVALComponentGroup.OVALComponentGroupID = iOVALComponentGroupID;   //myOVALComponentGroup.OVALComponentGroupID;
                                                            myOVALLiteralComponentForOVALComponentGroup.OVALLiteralComponentID = iOVALLiteralComponentID;   // myOVALLiteralComponent.OVALLiteralComponentID;
                                                            myOVALLiteralComponentForOVALComponentGroup.VocabularyID = iVocabularyOVALID;
                                                            myOVALLiteralComponentForOVALComponentGroup.timestamp = DateTimeOffset.Now;
                                                            oval_model.OVALLITERALCOMPONENTFOROVALCOMPONENTGROUP.Add(myOVALLiteralComponentForOVALComponentGroup);
                                                            //oval_model.SaveChanges();  //TEST PERFORMANCE
                                                            //iOVALComponentGroupLiteralComponentID=
                                                        }
                                                        catch (Exception exAddToOVALLITERALCOMPONENTFOROVALCOMPONENTGROUP)
                                                        {
                                                            Console.WriteLine("Exception: exAddToOVALLITERALCOMPONENTFOROVALCOMPONENTGROUP " + exAddToOVALLITERALCOMPONENTFOROVALCOMPONENTGROUP.Message + " " + exAddToOVALLITERALCOMPONENTFOROVALCOMPONENTGROUP.InnerException);
                                                        }

                                                    }
                                                    else
                                                    {
                                                        //Update OVALLITERALCOMPONENTFOROVALCOMPONENTGROUP
                                                    }
                                                    //myOVALLiteralComponentForOVALComponentGroup.timestamp = DateTimeOffset.Now;
                                                    //model.SaveChanges();
                                                    #endregion ovalliteralcomponent
                                                    break;

                                                case "object_component":
                                                    //Note: processed as OVALVARIABLECOMPONENT
                                                    #region OVALVARIABLECOMPONENT
                                                    string sItemField = nodeVARIABLEDETAILCOMPONENT.Attributes["item_field"].InnerText; //value
                                                    foreach (XmlAttribute myAttribute in nodeVARIABLEDETAILCOMPONENT.Attributes)
                                                    {
                                                        switch (myAttribute.Name)
                                                        {
                                                            case "object_ref":
                                                                string sOVALObjectRefIDPattern = myAttribute.InnerText;   //oval:org.mitre.oval:obj:281
                                                                int iOVALObjectRefID = 0;
                                                                try
                                                                {
                                                                    iOVALObjectRefID = oval_model.OVALOBJECT.Where(o => o.OVALObjectIDPattern == sOVALObjectRefIDPattern).Select(o => o.OVALObjectID).FirstOrDefault();
                                                                }
                                                                catch (Exception ex)
                                                                {

                                                                }
                                                                if (iOVALObjectRefID > 0)
                                                                {
                                                                    //Update OVALOBJECT
                                                                }
                                                                else
                                                                {
                                                                    OVALOBJECT oOVALOBJECTRef = new OVALOBJECT();
                                                                    oOVALOBJECTRef.CreatedDate = DateTimeOffset.Now;
                                                                    oOVALOBJECTRef.OVALObjectIDPattern = sOVALObjectRefIDPattern;
                                                                    oOVALOBJECTRef.VocabularyID = iVocabularyOVALID;
                                                                    oOVALOBJECTRef.timestamp = DateTimeOffset.Now;
                                                                    oval_model.OVALOBJECT.Add(oOVALOBJECTRef);
                                                                    oval_model.SaveChanges();
                                                                    iOVALObjectRefID = oOVALOBJECTRef.OVALObjectID;
                                                                }

                                                                //Review    iOVALComponentGroupID?

                                                                int iOVALVariableComponentID = 0;
                                                                try
                                                                {
                                                                    iOVALVariableComponentID = oval_model.OVALVARIABLECOMPONENT.Where(o => o.OVALVariableID == myOVALVariable.OVALVariableID && o.OVALObjectRefID == iOVALObjectRefID).Select(o => o.OVALVariableComponentID).FirstOrDefault();
                                                                }
                                                                catch (Exception ex)
                                                                {

                                                                }
                                                                if (iOVALVariableComponentID > 0)
                                                                {
                                                                    //Update OVALVARIABLECOMPONENT
                                                                    //TODO?
                                                                    //oOVALVariableComponentConcat.OVALItemFieldName = sItemField;
                                                                }
                                                                else
                                                                {
                                                                    OVALVARIABLECOMPONENT oOVALVariableComponentConcat = new OVALVARIABLECOMPONENT();
                                                                    oOVALVariableComponentConcat.CreatedDate = DateTimeOffset.Now;
                                                                    oOVALVariableComponentConcat.OVALVariableID = myOVALVariable.OVALVariableID;
                                                                    oOVALVariableComponentConcat.OVALObjectRefID = iOVALObjectRefID;
                                                                    oOVALVariableComponentConcat.OVALItemFieldName = sItemField;
                                                                    //NOTE: No OVALVariableRefID
                                                                    oOVALVariableComponentConcat.VocabularyID = iVocabularyOVALID;
                                                                    oOVALVariableComponentConcat.timestamp = DateTimeOffset.Now;
                                                                    oval_model.OVALVARIABLECOMPONENT.Add(oOVALVariableComponentConcat);
                                                                    oval_model.SaveChanges();
                                                                    iOVALVariableComponentID = oOVALVariableComponentConcat.OVALVariableComponentID;
                                                                }


                                                                int iOVALComponentGroupVariableComponentID = 0;
                                                                try
                                                                {
                                                                    iOVALComponentGroupVariableComponentID = oval_model.OVALVARIABLECOMPONENTFOROVALCOMPONENTGROUP.Where(o => o.OVALComponentGroupID == iOVALComponentGroupID && o.OVALVariableComponentID == iOVALVariableComponentID).Select(o => o.OVALComponentGroupVariableComponentID).FirstOrDefault();
                                                                }
                                                                catch (Exception ex)
                                                                {

                                                                }
                                                                if (iOVALComponentGroupVariableComponentID > 0)
                                                                {
                                                                    //Update OVALVARIABLECOMPONENTFOROVALCOMPONENTGROUP
                                                                    //
                                                                }
                                                                else
                                                                {
                                                                    OVALVARIABLECOMPONENTFOROVALCOMPONENTGROUP oOVALComponentGroupVariableComponent = new OVALVARIABLECOMPONENTFOROVALCOMPONENTGROUP();
                                                                    oOVALComponentGroupVariableComponent.CreatedDate = DateTimeOffset.Now;
                                                                    oOVALComponentGroupVariableComponent.OVALComponentGroupID = iOVALComponentGroupID;
                                                                    oOVALComponentGroupVariableComponent.OVALVariableComponentID = iOVALVariableComponentID;
                                                                    oOVALComponentGroupVariableComponent.VocabularyID = iVocabularyOVALID;
                                                                    oOVALComponentGroupVariableComponent.timestamp = DateTimeOffset.Now;
                                                                    oval_model.OVALVARIABLECOMPONENTFOROVALCOMPONENTGROUP.Add(oOVALComponentGroupVariableComponent);
                                                                    //oval_model.SaveChanges();    //TEST PERFORMANCE
                                                                    //iOVALComponentGroupVariableComponentID=
                                                                }

                                                                break;

                                                            case "item_field":
                                                                //Done before
                                                                break;
                                                            default:
                                                                Console.WriteLine("ERROR: Missing code for object_component myAttribute.Name=" + myAttribute.Name);

                                                                break;
                                                        }
                                                    }

                                                    #endregion OVALVARIABLECOMPONENT
                                                    break;

                                                case "variable_component":
                                                    #region OVALVARIABLECOMPONENT
                                                    //<variable_component var_ref="oval:org.mitre.oval:var:200"/>
                                                    foreach (XmlAttribute myAttribute in nodeVARIABLEDETAILCOMPONENT.Attributes)
                                                    {
                                                        switch (myAttribute.Name)
                                                        {
                                                            case "var_ref":
                                                                #region ovalvariablecomponentref
                                                                string sOVALVariableRefIDPattern = myAttribute.InnerText;
                                                                int iOVALVariableRefID = 0;
                                                                try
                                                                {
                                                                    iOVALVariableRefID = oval_model.OVALVARIABLE.Where(o => o.OVALVariableIDPattern == sOVALVariableRefIDPattern).Select(o => o.OVALVariableID).FirstOrDefault();
                                                                }
                                                                catch (Exception ex)
                                                                {

                                                                }
                                                                if (iOVALVariableRefID > 0)
                                                                {
                                                                    //Update OVALVARIABLE
                                                                }
                                                                else
                                                                {
                                                                    OVALVARIABLE oOVALVARIABLERef = new OVALVARIABLE();
                                                                    oOVALVARIABLERef.CreatedDate = DateTimeOffset.Now;
                                                                    oOVALVARIABLERef.OVALVariableIDPattern = sOVALVariableRefIDPattern;
                                                                    oOVALVARIABLERef.VocabularyID = iVocabularyOVALID;
                                                                    oOVALVARIABLERef.timestamp = DateTimeOffset.Now;
                                                                    oval_model.OVALVARIABLE.Add(oOVALVARIABLERef);
                                                                    oval_model.SaveChanges();
                                                                    iOVALVariableRefID = oOVALVARIABLERef.OVALVariableID;
                                                                }
                                                                #endregion ovalvariablecomponentref

                                                                //Review    iOVALComponentGroupID?

                                                                int iOVALVariableComponentID = 0;
                                                                try
                                                                {
                                                                    iOVALVariableComponentID = oval_model.OVALVARIABLECOMPONENT.Where(o => o.OVALVariableID == myOVALVariable.OVALVariableID && o.OVALVariableRefID == iOVALVariableRefID).Select(o => o.OVALVariableComponentID).FirstOrDefault();
                                                                }
                                                                catch (Exception ex)
                                                                {

                                                                }
                                                                if (iOVALVariableComponentID > 0)
                                                                {
                                                                    //Update OVALVARIABLECOMPONENT
                                                                }
                                                                else
                                                                {
                                                                    OVALVARIABLECOMPONENT oOVALVariableComponentConcat = new OVALVARIABLECOMPONENT();
                                                                    oOVALVariableComponentConcat.CreatedDate = DateTimeOffset.Now;
                                                                    oOVALVariableComponentConcat.OVALVariableID = myOVALVariable.OVALVariableID;
                                                                    oOVALVariableComponentConcat.OVALVariableRefID = iOVALVariableRefID;
                                                                    //TODO?
                                                                    //oOVALVariableComponentConcat.OVALItemFieldName =
                                                                    //NOTE: No OVALObjectRefID
                                                                    oOVALVariableComponentConcat.VocabularyID = iVocabularyOVALID;
                                                                    oOVALVariableComponentConcat.timestamp = DateTimeOffset.Now;
                                                                    oval_model.OVALVARIABLECOMPONENT.Add(oOVALVariableComponentConcat);
                                                                    oval_model.SaveChanges();
                                                                    iOVALVariableComponentID = oOVALVariableComponentConcat.OVALVariableComponentID;
                                                                }


                                                                int iOVALComponentGroupVariableComponentID = 0;
                                                                try
                                                                {
                                                                    iOVALComponentGroupVariableComponentID = oval_model.OVALVARIABLECOMPONENTFOROVALCOMPONENTGROUP.Where(o => o.OVALComponentGroupID == iOVALComponentGroupID && o.OVALVariableComponentID == iOVALVariableComponentID).Select(o => o.OVALComponentGroupVariableComponentID).FirstOrDefault();
                                                                }
                                                                catch (Exception ex)
                                                                {

                                                                }
                                                                if (iOVALComponentGroupVariableComponentID > 0)
                                                                {
                                                                    //Update OVALVARIABLECOMPONENTFOROVALCOMPONENTGROUP
                                                                }
                                                                else
                                                                {
                                                                    OVALVARIABLECOMPONENTFOROVALCOMPONENTGROUP oOVALComponentGroupVariableComponent = new OVALVARIABLECOMPONENTFOROVALCOMPONENTGROUP();
                                                                    oOVALComponentGroupVariableComponent.CreatedDate = DateTimeOffset.Now;
                                                                    oOVALComponentGroupVariableComponent.OVALComponentGroupID = iOVALComponentGroupID;
                                                                    oOVALComponentGroupVariableComponent.OVALVariableComponentID = iOVALVariableComponentID;
                                                                    oOVALComponentGroupVariableComponent.VocabularyID = iVocabularyOVALID;
                                                                    oOVALComponentGroupVariableComponent.timestamp = DateTimeOffset.Now;
                                                                    oval_model.OVALVARIABLECOMPONENTFOROVALCOMPONENTGROUP.Add(oOVALComponentGroupVariableComponent);
                                                                    //oval_model.SaveChanges();    //TEST PERFORMANCE
                                                                    //iOVALComponentGroupVariableComponentID=
                                                                }

                                                                break;
                                                            default:
                                                                Console.WriteLine("ERROR: Missing code for variable_component myAttribute.Name=" + myAttribute.Name);
                                                                break;
                                                        }
                                                    }
                                                    #endregion OVALVARIABLECOMPONENT
                                                    break;

                                                default:
                                                    //TODO

                                                    //escape_regex
                                                    /*
                                                    <escape_regex>
                                                        <object_component item_field="value" object_ref="oval:org.mitre.oval:obj:219"/>
                                                    </escape_regex>
                                                    */
                                                    //regex_capture
                                                    /*
                                                    <regex_capture pattern="^(1\.[5-9]|[2-9]\.[0-9])$">
                                                      <object_component object_ref="oval:org.mitre.oval:obj:26525" item_field="value"/>
                                                    </regex_capture>
                                                    */

                                                    //variable_component
                                                    //substring
                                                    Console.WriteLine("ERROR: Missing code for VARIABLEDETAILCOMPONENT " + nodeVARIABLEDETAILCOMPONENT.Name);
                                                    break;
                                            }
                                        }
                                        #endregion concat
                                        break;

                                    case "value":

                                        #region ovalvariablevalue
                                        string sVariableValue = nodeVARIABLEDETAIL.InnerText;
                                        //XORCISMModel.OVALVARIABLEVALUE myOVALVariableValue;
                                        //myOVALVariableValue = oval_model.OVALVARIABLEVALUE.FirstOrDefault(o => o.OVALVariableID == myOVALVariable.OVALVariableID && o.VALUE.ValueValue == sVariableValue);
                                        int iOVALVariableValueID = 0;
                                        try
                                        {
                                            //iOVALVariableValueID = oval_model.OVALVARIABLEVALUE.Where(o => o.OVALVariableID == myOVALVariable.OVALVariableID && o.VALUE.ValueValue == sVariableValue).Select(o => o.OVALVariableValueID).FirstOrDefault();
                                            iOVALVariableValueID = oval_model.OVALVARIABLEVALUE.Where(o => o.OVALVariableID == myOVALVariable.OVALVariableID && o.ValueValue == sVariableValue).Select(o => o.OVALVariableValueID).FirstOrDefault();
                                        }
                                        catch (Exception ex)
                                        {

                                        }
                                        //if (myOVALVariableValue == null)
                                        if (iOVALVariableValueID <= 0)
                                        {
                                            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                            Console.WriteLine("DEBUG Adding OVALVARIABLEVALUE");
                                            OVALVARIABLEVALUE myOVALVariableValue = new OVALVARIABLEVALUE();
                                            myOVALVariableValue.VocabularyID = iVocabularyOVALID;
                                            myOVALVariableValue.CreatedDate = DateTimeOffset.Now;

                                            //XORCISMModel.VALUE myValue;
                                            //myValue = model.VALUE.FirstOrDefault(o => o.ValueValue == sVariableValue);
                                            int iValueID = 0;
                                            try
                                            {
                                                iValueID = model.VALUE.Where(o => o.ValueValue == sVariableValue).Select(o => o.ValueID).FirstOrDefault();
                                            }
                                            catch (Exception ex)
                                            {

                                            }
                                            //if (myValue == null)
                                            if (iValueID <= 0)
                                            {
                                                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                Console.WriteLine("DEBUG Adding VALUE");
                                                VALUE myValue = new VALUE();
                                                myValue.ValueValue = sVariableValue;
                                                myValue.CreatedDate = DateTimeOffset.Now;
                                                //TODO VocabularyID?
                                                try
                                                {
                                                    myValue.timestamp = DateTimeOffset.Now;
                                                    model.VALUE.Add(myValue);
                                                    model.SaveChanges();
                                                    iValueID = myValue.ValueID;
                                                }
                                                catch (Exception exAddToVALUE)
                                                {
                                                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                    Console.WriteLine("Exception: exAddToVALUE " + exAddToVALUE.Message + " " + exAddToVALUE.InnerException);
                                                }
                                            }
                                            else
                                            {
                                                //Update VALUE
                                            }

                                            myOVALVariableValue.OVALVariableID = myOVALVariable.OVALVariableID;
                                            myOVALVariableValue.ValueID = iValueID; // myValue.ValueID;
                                            try
                                            {
                                                myOVALVariableValue.timestamp = DateTimeOffset.Now;
                                                oval_model.OVALVARIABLEVALUE.Add(myOVALVariableValue);
                                                //oval_model.SaveChanges();    //TEST PERFORMANCE
                                                //iOVALVariableValueID=
                                            }
                                            catch (Exception exAddToOVALVARIABLEVALUE)
                                            {
                                                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                Console.WriteLine("Exception: exAddToOVALVARIABLEVALUE " + exAddToOVALVARIABLEVALUE.Message + " " + exAddToOVALVARIABLEVALUE.InnerException);
                                            }
                                        }
                                        else
                                        {
                                            //Update OVALVARIABLEVALUE
                                        }
                                        #endregion ovalvariablevalue
                                        break;

                                    case "object_component":
                                        //Note: processed as OVALVARIABLECOMPONENT
                                        #region ovalvariablecomponent
                                        int iOVALObjectID = 0;
                                        OVALVARIABLECOMPONENT oOVALVariableComponent = null;
                                        string sItemFieldValue = "";
                                        //<object_component object_ref="oval:org.mitre.oval:obj:7417" item_field="value"/>
                                        foreach (XmlAttribute myAttribute in nodeVARIABLEDETAIL.Attributes)
                                        {
                                            switch (myAttribute.Name)
                                            {
                                                case "object_ref":
                                                    try
                                                    {
                                                        iOVALObjectID = oval_model.OVALOBJECT.Where(o => o.OVALObjectIDPattern == myAttribute.Value).Select(o => o.OVALObjectID).FirstOrDefault();
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.WriteLine("ERROR: OVALOBJECT not found " + myAttribute.Value);
                                                    }
                                                    oOVALVariableComponent = oval_model.OVALVARIABLECOMPONENT.FirstOrDefault(o => o.OVALVariableID == myOVALVariable.OVALVariableID && o.OVALObjectRefID == iOVALObjectID);
                                                    if (oOVALVariableComponent == null)
                                                    {
                                                        oOVALVariableComponent = new OVALVARIABLECOMPONENT();
                                                        oOVALVariableComponent.CreatedDate = DateTimeOffset.Now;
                                                        //oOVALVariableComponent.OVALVariableIDPattern = myOVALVariable.OVALVariableIDPattern;    //Removed
                                                        oOVALVariableComponent.OVALVariableID = myOVALVariable.OVALVariableID;
                                                        oOVALVariableComponent.OVALObjectRefID = iOVALObjectID;
                                                        oOVALVariableComponent.OVALItemFieldName = sItemFieldValue;
                                                        oOVALVariableComponent.VocabularyID = iVocabularyOVALID;
                                                        oOVALVariableComponent.timestamp = DateTimeOffset.Now;
                                                        oval_model.OVALVARIABLECOMPONENT.Add(oOVALVariableComponent);

                                                    }
                                                    else
                                                    {
                                                        //Update OVALVARIABLECOMPONENT
                                                        oOVALVariableComponent.OVALItemFieldName = sItemFieldValue;
                                                        oOVALVariableComponent.timestamp = DateTimeOffset.Now;
                                                    }

                                                    //TODO Review
                                                    //OVALVARIABLECOMPONENTFOR?

                                                    break;

                                                case "item_field":
                                                    //oOVALVariableComponent.OVALItemFieldName = myAttribute.Value;   //value
                                                    sItemFieldValue = myAttribute.Value;   //value
                                                    break;
                                                default:
                                                    Console.WriteLine("ERROR: Missing code for object_component myAttribute.Name=" + myAttribute.Name);
                                                    break;
                                            }
                                        }
                                        //Update OVALVARIABLECOMPONENT
                                        try
                                        {
                                            model.SaveChanges();
                                        }
                                        catch (Exception exOVALVARIABLECOMPONENT)
                                        {
                                            Console.WriteLine("Exception: exOVALVARIABLECOMPONENT " + exOVALVARIABLECOMPONENT.Message + " " + exOVALVARIABLECOMPONENT.InnerException);
                                        }
                                        #endregion ovalvariablecomponent
                                        break;

                                    default:
                                        Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                        Console.WriteLine("ERROR: Missing code for variables " + nodeVARIABLEDETAIL.Name);
                                        //TODO object_component OVALVARIABLECOMPONENT (should it be OVALVARIABLEOBJECTCOMPONENT?)
                                        break;
                                }
                            }
                        }
                    }
                }
                #endregion ovalvariables
            }

            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());

            model.SaveChanges();
            model.Dispose();
            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
            Console.WriteLine("COMPLETED OVAL");
        }

        //public static void OVALParseCriteria(XORCISMEntities model, XmlNode node2, XORCISMModel.OVALCRITERIA myCriteria, int iVocabularyID)
        //public static void OVALParseCriteria(XmlNode node2, XORCISMModel.OVALCRITERIA myCriteria, int iVocabularyID)
        public static void OVALParseCriteria(XmlNode node2, int iOVALCriteriaID, int iVocabularyID)
        {
            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
            Console.WriteLine("DEBUG in OVALParseCriteria() iOVALCriteriaID=" + iOVALCriteriaID);
            int iCptCriteria = 0;
            foreach (XmlNode nodeCriterion in node2.ChildNodes)
            {
                
                if (nodeCriterion.Name == "criteria")
                {
                    Console.WriteLine("DEBUG Parsing criteria");
                    #region ovalcriteriacriteria
                    iCptCriteria++; //TODO: This is not perfect
                    //TODO
                    //OVALCRITERIAFOROVALCRITERIA
                    XOVALModel.OVALCRITERIA myCriteriaSubject=null;
                    XOVALModel.OVALCRITERIAFOROVALCRITERIA myCriteriaForCriteria = new OVALCRITERIAFOROVALCRITERIA();
                    myCriteriaForCriteria = oval_model.OVALCRITERIAFOROVALCRITERIA.FirstOrDefault(o => o.OVALCriteriaRefID == iOVALCriteriaID && o.CriteriaRank == iCptCriteria);  //TODO Check the comment? VocabularyID?
                    int iOVALCriteriaSubjectID = 0;
                    if (myCriteriaForCriteria == null)
                    {
                        Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                        Console.WriteLine("DEBUG Adding new CRITERIA for CRITERIA");
                        myCriteriaSubject = new OVALCRITERIA();
                        myCriteriaSubject.VocabularyID = iVocabularyID;
                        myCriteriaSubject.CreatedDate = DateTimeOffset.Now;
                        try
                        {
                            string sOperatorValue= nodeCriterion.Attributes["operator"].InnerText;
                            //myCriteriaSubject.OperatorValue = sOperatorValue;   // REMOVED

                            //TODO: OperatorEnumerationID
                            //TODO Hardcode that?
                            #region OPERATORENUMERATION
                            int iOperatorEnumerationID = 0;
                            try
                            {
                                iOperatorEnumerationID = oval_model.OPERATORENUMERATION.Where(o => o.OperatorValue == sOperatorValue).Select(o=>o.OperatorEnumerationID).FirstOrDefault();
                            }
                            catch (Exception ex)
                            {

                            }

                            if (iOperatorEnumerationID <= 0)
                            {
                                try
                                {
                                    XOVALModel.OPERATORENUMERATION oOperatorEnumeration = new XOVALModel.OPERATORENUMERATION();
                                    oOperatorEnumeration.CreatedDate = DateTimeOffset.Now;
                                    oOperatorEnumeration.OperatorValue = sOperatorValue;
                                    oOperatorEnumeration.VocabularyID = iVocabularyID;
                                    oOperatorEnumeration.timestamp = DateTimeOffset.Now;
                                    oval_model.OPERATORENUMERATION.Add(oOperatorEnumeration);
                                    oval_model.SaveChanges();
                                    iOperatorEnumerationID = oOperatorEnumeration.OperatorEnumerationID;
                                }
                                catch (Exception exOPERATORENUMERATION)
                                {
                                    Console.WriteLine("Exception: exOPERATORENUMERATION " + exOPERATORENUMERATION.Message + " " + exOPERATORENUMERATION.InnerException);
                                }
                            }
                            else
                            {
                                //Update OPERATORENUMERATION

                            }
                            #endregion OPERATORENUMERATION
                            myCriteriaSubject.OperatorEnumerationID = iOperatorEnumerationID;
                        }
                        catch(Exception ex)
                        {
                            //Possibly no operator

                        }

                        try
                        {
                            string sCriteriaSubjectComment= nodeCriterion.Attributes["comment"].InnerText;
                            myCriteriaSubject.comment =sCriteriaSubjectComment;
                            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                            Console.WriteLine("DEBUG comment=" + sCriteriaSubjectComment);
                            //TODO?
                            //Sun Alert ID 102033
                            //HP Security Bulletin HPSBUX02308
                        }
                        catch (Exception ex)
                        {

                        }

                        //TODO
                        //negate
                        //applicabilitycheck
                        try
                        {
                            myCriteriaSubject.timestamp = DateTimeOffset.Now;
                            oval_model.OVALCRITERIA.Attach(myCriteriaSubject);
                            oval_model.Entry(myCriteriaSubject).State = EntityState.Modified;
                            oval_model.OVALCRITERIA.Add(myCriteriaSubject);
                            oval_model.SaveChanges();
                            iOVALCriteriaSubjectID = myCriteriaSubject.OVALCriteriaID;
                        }
                        catch (Exception exAddToOVALCRITERIA2)
                        {
                            Console.WriteLine("Exception: exAddToOVALCRITERIA2 " + exAddToOVALCRITERIA2.Message + " " + exAddToOVALCRITERIA2.InnerException);
                        }

                        Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                        Console.WriteLine("DEBUG Adding OVALCRITERIAFOROVALCRITERIA");
                        
                        try
                        {
                            myCriteriaForCriteria = new OVALCRITERIAFOROVALCRITERIA();
                            myCriteriaForCriteria.OVALCriteriaRefID = iOVALCriteriaID;  // myCriteria.OVALCriteriaID;
                            myCriteriaForCriteria.OVALCriteriaSubjectID = iOVALCriteriaSubjectID;   // myCriteriaSubject.OVALCriteriaID;
                            myCriteriaForCriteria.CriteriaRank = iCptCriteria;
                            myCriteriaForCriteria.CreatedDate = DateTimeOffset.Now;
                            myCriteriaForCriteria.VocabularyID = iVocabularyID;
                            myCriteriaForCriteria.timestamp = DateTimeOffset.Now;
                            oval_model.OVALCRITERIAFOROVALCRITERIA.Add(myCriteriaForCriteria);
                            oval_model.SaveChanges();    //TEST PERFORMANCE

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
                            Console.WriteLine("Exception: DbEntityValidationExceptionREFERENCE " + sb.ToString());
                        }
                        catch (Exception exAddToOVALCRITERIAFOROVALCRITERIA)
                        {
                            Console.WriteLine("Exception: exAddToOVALCRITERIAFOROVALCRITERIA " + exAddToOVALCRITERIAFOROVALCRITERIA.Message + " " + exAddToOVALCRITERIAFOROVALCRITERIA.InnerException);
                        }
                    }
                    else
                    {
                        //Update OVALCRITERIAFOROVALCRITERIA
                        Console.WriteLine("DEBUG OVALCRITERIAFOROVALCRITERIA exists");

                        myCriteriaSubject = oval_model.OVALCRITERIA.FirstOrDefault(o => o.OVALCriteriaID == myCriteriaForCriteria.OVALCriteriaSubjectID);
                        if (myCriteriaSubject == null)
                        {
                            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                            Console.WriteLine("ERROR: myCriteriaSubject not found!");
                        }
                        else
                        {
                            iOVALCriteriaSubjectID = myCriteriaSubject.OVALCriteriaID;
                        }
                    }

                    try
                    {
                        myCriteriaForCriteria.timestamp = DateTimeOffset.Now;
                        oval_model.SaveChanges();    //TEST PERFORMANCE

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
                        Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                        Console.WriteLine("Exception: DbEntityValidationExceptionmyCriteriaForCriteria " + sb.ToString());
                    }
                    catch (Exception exmyCriteriaForCriteria)
                    {
                        Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                        Console.WriteLine("Exception: exmyCriteriaForCriteria " + exmyCriteriaForCriteria.Message + " " + exmyCriteriaForCriteria.InnerException);
                    }
                    #endregion ovalcriteriacriteria

                    //****************************************************************************************************************************************************************
                    //OVALParseCriteria(model, nodeCriterion, myCriteriaSubject, iVocabularyID);
                    //OVALParseCriteria(nodeCriterion, myCriteriaSubject.OVALCriteriaID, iVocabularyID);
                    OVALParseCriteria(nodeCriterion, iOVALCriteriaSubjectID, iVocabularyID);
                    #region criterions1
                    //foreach (XmlNode nodeCriterionCC in nodeCriterion.ChildNodes)
                    //{
                    //    //TODO: this is "duplicate" code
                    //    if (nodeCriterionCC.Name == "criterion")
                    //    {
                    //        #region criterion
                    //        string sTestRef = nodeCriterionCC.Attributes["test_ref"].InnerText;
                    //        XORCISMModel.OVALCRITERION myCriterion;
                    //        myCriterion = oval_model.OVALCRITERION.FirstOrDefault(o => o.OVALTestIDPattern == sTestRef); //&& comment=
                    //        if (myCriterion == null)
                    //        {
                    //            Console.WriteLine(string.Format("Adding new OVALCRITERION [{0}] in table OVALCRITERION", sTestRef));
                    //            myCriterion = new OVALCRITERION();
                    //            myCriterion.OVALTestIDPattern = sTestRef;
                    //            //TODO: check if the test exists
                    //            myCriterion.negate = false;
                    //            try
                    //            {
                    //                if (nodeCriterionCC.Attributes["negate"].InnerText.ToLower() == "true")
                    //                {
                    //                    myCriterion.negate = true;
                    //                }
                    //            }
                    //            catch (Exception ex)
                    //            {

                    //            }

                    //            myCriterion.comment = nodeCriterionCC.Attributes["comment"].InnerText;
                    //            //myCriterion.applicabilitycheck;
                    //            model.AddToOVALCRITERION(myCriterion);
                    //            model.SaveChanges();
                    //        }

                    //        //OVALCRITERIONFOROVALCRITERIA
                    //        XORCISMModel.OVALCRITERIONFOROVALCRITERIA myOVALCriterionForOVALCriteria;
                    //        myOVALCriterionForOVALCriteria = oval_model.OVALCRITERIONFOROVALCRITERIA.FirstOrDefault(o => o.OVALCriteriaID == myCriteriaSubject.OVALCriteriaID && o.OVALCriterionID == myCriterion.OVALCriterionID);
                    //        if (myOVALCriterionForOVALCriteria == null)
                    //        {
                    //            Console.WriteLine("Adding OVALCRITERIONFOROVALCRITERIA");
                    //            myOVALCriterionForOVALCriteria = new OVALCRITERIONFOROVALCRITERIA();
                    //            myOVALCriterionForOVALCriteria.OVALCriteriaID = myCriteriaSubject.OVALCriteriaID;
                    //            myOVALCriterionForOVALCriteria.OVALCriterionID = myCriterion.OVALCriterionID;
                    //            model.AddToOVALCRITERIONFOROVALCRITERIA(myOVALCriterionForOVALCriteria);
                    //            model.SaveChanges();
                    //        }
                    //        #endregion criterion
                    //    }
                    //    else
                    //    {
                    //        if (nodeCriterionCC.Name == "extend_definition")
                    //        {
                    //            #region extenddefinition
                    //            string sDefinitionExtendedRefID = nodeCriterionCC.Attributes["definition_ref"].InnerText;
                    //            XORCISMModel.OVALDEFINITION myOVALExtendedDef;
                    //            myOVALExtendedDef = oval_model.OVALDEFINITION.FirstOrDefault(o => o.OVALDefinitionIDPattern == sDefinitionExtendedRefID);
                    //            if (myOVALExtendedDef == null)
                    //            {
                    //                Console.WriteLine("Import_oval the extended definition " + sDefinitionExtendedRefID + " does not exist");
                    //                Console.WriteLine("Adding new OVALDEFINITION " + sDefinitionExtendedRefID);
                    //                myOVALExtendedDef = new OVALDEFINITION();
                    //                myOVALExtendedDef.OVALDefinitionIDPattern = sDefinitionExtendedRefID;
                    //                //TODO: incomplete, will crash
                    //                model.AddToOVALDEFINITION(myOVALExtendedDef);
                    //                model.SaveChanges();
                    //            }

                    //            XORCISMModel.OVALEXTENDDEFINITION myOVALExtendDef;
                    //            myOVALExtendDef = oval_model.OVALEXTENDDEFINITION.FirstOrDefault(o => o.OVALDefinitionIDPattern == sDefinitionExtendedRefID);    //&& o.OVALDefinitionID == myOVALExtendedDef.OVALDefinitionID
                    //            if (myOVALExtendDef == null)
                    //            {
                    //                Console.WriteLine("Adding new OVALEXTENDDEFINITION " + sDefinitionExtendedRefID);
                    //                myOVALExtendDef = new OVALEXTENDDEFINITION();
                    //                myOVALExtendDef.OVALDefinitionID = myOVALExtendedDef.OVALDefinitionID;
                    //                myOVALExtendDef.OVALDefinitionIDPattern = sDefinitionExtendedRefID;
                    //                //myOVALExtendDef.negate
                    //                myOVALExtendDef.comment = nodeCriterionCC.Attributes["comment"].InnerText;
                    //                //myOVALExtendDef.applicabilitycheck
                    //                model.AddToOVALEXTENDDEFINITION(myOVALExtendDef);
                    //                model.SaveChanges();
                    //            }

                    //            //TODO
                    //            //XORCISMModel.OVALEXTENDDEFINITIONFORCRITERIA

                    //            #endregion extenddefinition
                    //        }
                    //        else
                    //        {
                    //            Console.WriteLine("Import_oval missing code for definition criteria1 : " + nodeCriterionCC.Name);
                    //        }
                    //    }
                    //}
                    #endregion criterions1
                
                }
                else
                {

                    #region criterions2
                    //TODO: this is "duplicate" code
                    if (nodeCriterion.Name == "criterion")
                    {
                        #region criterion
                        string sTestRef = nodeCriterion.Attributes["test_ref"].InnerText;
                        Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                        Console.WriteLine("DEBUG criterion test_ref=" + sTestRef);
                        //Regex myRegexOVALTEST = new Regex(@"oval:(.*?):tst:[0-9]*[0-9]"); //TODO Review
                        Regex myRegexOVALTEST = new Regex(@"oval:(.*?):tst:\d+"); //TODO Review

                        string sTemp = myRegexOVALTEST.Match(sTestRef).ToString();
                        if (sTemp == "")
                        {
                            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                            Console.WriteLine("ERROR: TODO Missing code the criterion is not an OVALTEST " + sTestRef);
                        }


                        
                        Boolean bNegateOVALTest = false;
                        #region ovaltest
                        //XORCISMModel.OVALTEST oOVALTest;
                        //oOVALTest = oval_model.OVALTEST.FirstOrDefault(o => o.OVALTestIDPattern == sTestRef);
                        int iOVALTestID = 0;
                        try
                        {
                            iOVALTestID = oval_model.OVALTEST.Where(o => o.OVALTestIDPattern == sTestRef).Select(o => o.OVALTestID).FirstOrDefault();
                        }
                        catch(Exception ex)
                        {

                        }
                        //if (oOVALTest == null)
                        if (iOVALTestID<=0)
                        {
                            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                            Console.WriteLine(string.Format("DEBUG Adding new OVALTEST [{0}] in table OVALTEST", sTestRef));
                            
                            //TODO
                            //foreach(in nodeCriterion.Attributes)
                            bNegateOVALTest = false;
                            try
                            {
                                if (nodeCriterion.Attributes["negate"].InnerText.ToLower() == "true")
                                {
                                    bNegateOVALTest = true;
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                            //TODO Review
                            string sOVALTestComment = "";
                            try
                            {
                                sOVALTestComment = nodeCriterion.Attributes["comment"].InnerText;
                                Console.WriteLine("DEBUG comment=" + sOVALTestComment);
                            }
                            catch(Exception ex)
                            {

                            }
                            //myCriterion.applicabilitycheck;
                            try
                            {
                                OVALTEST oOVALTest = new OVALTEST();
                                oOVALTest.VocabularyID = iVocabularyID;
                                oOVALTest.CreatedDate = DateTimeOffset.Now;
                                oOVALTest.OVALTestIDPattern = sTestRef;
                                oOVALTest.comment = sOVALTestComment;
                                //oOVALTest.EnumerationValue = "";    //TODO Review
                                
                                oOVALTest.timestamp = DateTimeOffset.Now;
                                oval_model.OVALTEST.Add(oOVALTest);
                                oval_model.SaveChanges();
                                iOVALTestID = oOVALTest.OVALTestID;
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
                                Console.WriteLine("Exception: DbEntityValidationExceptionREFERENCE " + sb.ToString());
                            }
                            catch (Exception exAddToOVALTEST)
                            {
                                Console.WriteLine("Exception: exAddToOVALTEST " + exAddToOVALTEST.Message + " " + exAddToOVALTEST.InnerException);
                            }
                        }
                        else
                        {
                            //Update OVALTEST
                        }
                        #endregion ovaltest

                        //OVALCRITERIONFOROVALCRITERIA
                        #region OVALCRITERIACRITERION
                        //XORCISMModel.OVALCRITERIACRITERION oOVALCriteriaCriterion;
                        //oOVALCriteriaCriterion = oval_model.OVALCRITERIACRITERION.FirstOrDefault(o => o.OVALCriteriaID == myCriteria.OVALCriteriaID && o.OVALTestID == iOVALTestID);
                        int iOVALCriteriaCriterionID = 0;
                        try
                        {
                            iOVALCriteriaCriterionID = oval_model.OVALCRITERIACRITERION.Where(o => o.OVALCriteriaID == iOVALCriteriaID && o.OVALTestID == iOVALTestID).Select(o => o.OVALCriteriaCriterionID).FirstOrDefault();
                        }
                        catch(Exception ex)
                        {

                        }
                        //if (oOVALCriteriaCriterion == null)
                        if (iOVALCriteriaCriterionID<=0)
                        {
                            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                            Console.WriteLine("DEBUG Adding OVALCRITERIACRITERION " + iOVALCriteriaID);// + " - " + myCriterion.comment);
                            
                            try
                            {
                                OVALCRITERIACRITERION oOVALCriteriaCriterion = new OVALCRITERIACRITERION();
                                oOVALCriteriaCriterion.OVALCriteriaID = iOVALCriteriaID;    // myCriteria.OVALCriteriaID;
                                oOVALCriteriaCriterion.negate = bNegateOVALTest;
                                oOVALCriteriaCriterion.OVALTestID = iOVALTestID;    // oOVALTest.OVALTestID;
                                oOVALCriteriaCriterion.VocabularyID = iVocabularyID;
                                oOVALCriteriaCriterion.CreatedDate = DateTimeOffset.Now;
                                oOVALCriteriaCriterion.timestamp = DateTimeOffset.Now;
                                oval_model.OVALCRITERIACRITERION.Add(oOVALCriteriaCriterion);
                                oval_model.SaveChanges();    //TEST PERFORMANCE
                                //iOVALCriteriaCriterionID=
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
                                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                Console.WriteLine("Exception: DbEntityValidationExceptionOVALCRITERIACRITERION " + sb.ToString());
                            }
                            catch (Exception exAddToOVALCRITERIACRITERION)
                            {
                                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                Console.WriteLine("Exception: exAddToOVALCRITERIACRITERION " + exAddToOVALCRITERIACRITERION.Message + " " + exAddToOVALCRITERIACRITERION.InnerException);
                            }
                        }
                        else
                        {
                            //Update OVALCRITERIONFOROVALCRITERIA
                        }
                        #endregion OVALCRITERIACRITERION
                        #endregion criterion
                    }
                    else
                    {
                        if (nodeCriterion.Name == "extend_definition")
                        {
                            #region extenddefinition
                            //What is the definition extended
                            string sDefinitionExtendedRefID = nodeCriterion.Attributes["definition_ref"].InnerText;
                            //XORCISMModel.OVALDEFINITION myOVALExtendedDef;
                            //myOVALExtendedDef = oval_model.OVALDEFINITION.FirstOrDefault(o => o.OVALDefinitionIDPattern == sDefinitionExtendedRefID);
                            int iOVALDefinitionID = 0;
                            try
                            {
                                iOVALDefinitionID = oval_model.OVALDEFINITION.Where(o => o.OVALDefinitionIDPattern == sDefinitionExtendedRefID).Select(o => o.OVALDefinitionID).FirstOrDefault();
                            }
                            catch(Exception ex)
                            {

                            }
                            //if (myOVALExtendedDef == null)
                            if (iOVALDefinitionID<=0)
                            {
                                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                Console.WriteLine("DEBUG Import_oval the extended definition " + sDefinitionExtendedRefID + " does not exist");
                                Console.WriteLine("DEBUG Adding new OVALDEFINITION " + sDefinitionExtendedRefID);
                                OVALDEFINITION myOVALExtendedDef = new OVALDEFINITION();
                                myOVALExtendedDef.OVALDefinitionIDPattern = sDefinitionExtendedRefID;

                                try
                                {
                                    string sOVALDefinitionComment = nodeCriterion.Attributes["comment"].InnerText;
                                    Console.WriteLine("DEBUG comment=" + sOVALDefinitionComment);
                                    myOVALExtendedDef.OVALDefinitionTitle = sOVALDefinitionComment; //TODO REVIEW
                                }
                                catch(Exception ex)
                                {

                                }
                                myOVALExtendedDef.VocabularyID = iVocabularyID;
                                myOVALExtendedDef.CreatedDate = DateTimeOffset.Now;
                                
                                try
                                {
                                    myOVALExtendedDef.timestamp = DateTimeOffset.Now;
                                    oval_model.OVALDEFINITION.Add(myOVALExtendedDef);
                                    oval_model.SaveChanges();
                                    iOVALDefinitionID = myOVALExtendedDef.OVALDefinitionID;
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
                                    Console.WriteLine("Exception: DbEntityValidationExceptionREFERENCE " + sb.ToString());
                                }
                                catch (Exception exAddToOVALDEFINITION)
                                {
                                    Console.WriteLine("Exception: exAddToOVALDEFINITION " + exAddToOVALDEFINITION.Message + " " + exAddToOVALDEFINITION.InnerException);
                                }
                            }
                            else
                            {
                                //Update OVALDEFINITION
                            }

                            /*
                            XORCISMModel.OVALEXTENDDEFINITION myOVALExtendDef;
                            //TODO negate applicabilitycheck
                            myOVALExtendDef = oval_model.OVALEXTENDDEFINITION.FirstOrDefault(o => o.OVALDefinitionIDPattern == sDefinitionExtendedRefID);    //&& o.OVALDefinitionID == myOVALExtendedDef.OVALDefinitionID
                            if (myOVALExtendDef == null)
                            {
                                Console.WriteLine("DEBUG Adding new OVALEXTENDDEFINITION " + sDefinitionExtendedRefID);
                                myOVALExtendDef = new OVALEXTENDDEFINITION();
                                myOVALExtendDef.OVALDefinitionID = myOVALExtendedDef.OVALDefinitionID;
                                myOVALExtendDef.OVALDefinitionIDPattern = sDefinitionExtendedRefID;
                                //myOVALExtendDef.VocabularyID = iVocabularyID; //TODO Uncomment
                                
                                //TODO CreatedDate
                                //myOVALExtendDef.negate
                                myOVALExtendDef.comment = nodeCriterion.Attributes["comment"].InnerText;
                                //myOVALExtendDef.applicabilitycheck
                                try
                                {
                                    //TODO timestamp
                                    model.OVALEXTENDDEFINITION.Add(myOVALExtendDef);
                                    model.SaveChanges();
                                }
                                catch (Exception exAddToOVALEXTENDDEFINITION)
                                {
                                    Console.WriteLine("Exception: exAddToOVALEXTENDDEFINITION " + exAddToOVALEXTENDDEFINITION.Message + " " + exAddToOVALEXTENDDEFINITION.InnerException);
                                }
                            }
                            */

                            #region OVALCRITERIAEXTENDDEFINITION
                            //XORCISMModel.OVALCRITERIAEXTENDDEFINITION oOVALCriteriaExtendDefinition = oval_model.OVALCRITERIAEXTENDDEFINITION.FirstOrDefault(o => o.OVALCriteriaID == myCriteria.OVALCriteriaID && o.OVALDefinitionID == iOVALDefinitionID);
                            int iOVALCriteriaExtendDefinitionID = 0;
                            try
                            {
                                iOVALCriteriaExtendDefinitionID = oval_model.OVALCRITERIAEXTENDDEFINITION.FirstOrDefault(o => o.OVALCriteriaID == iOVALCriteriaID && o.OVALDefinitionID == iOVALDefinitionID).OVALCriteriaExtendDefinitionID;
                            }
                            catch(Exception ex)
                            {

                            }
                            //if(oOVALCriteriaExtendDefinition!=null)
                            if (iOVALCriteriaExtendDefinitionID>0)
                            {
                                //Update OVALCRITERIAEXTENDDEFINITION
                            }
                            else
                            {
                                try
                                {
                                    OVALCRITERIAEXTENDDEFINITION oOVALCriteriaExtendDefinition = new OVALCRITERIAEXTENDDEFINITION();
                                    oOVALCriteriaExtendDefinition.OVALCriteriaID = iOVALCriteriaID; // myCriteria.OVALCriteriaID;
                                    oOVALCriteriaExtendDefinition.CreatedDate = DateTimeOffset.Now;
                                    oOVALCriteriaExtendDefinition.VocabularyID = iVocabularyID;
                                    oOVALCriteriaExtendDefinition.OVALDefinitionID = iOVALDefinitionID; // myOVALExtendedDef.OVALDefinitionID;
                                    //.comment = nodeCriterion.Attributes["comment"].InnerText;
                                    //negate    TODO
                                    oOVALCriteriaExtendDefinition.timestamp = DateTimeOffset.Now;
                                    oval_model.OVALCRITERIAEXTENDDEFINITION.Add(oOVALCriteriaExtendDefinition);
                                    oval_model.SaveChanges();    //TEST PERFORMANCE
                                    //iOVALCriteriaExtendDefinitionID=
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
                                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                    Console.WriteLine("Exception: DbEntityValidationExceptionOVALCriteriaExtendDefinition " + sb.ToString());
                                }
                                catch(Exception exoOVALCriteriaExtendDefinition)
                                {
                                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                    Console.WriteLine("Exception: exoOVALCriteriaExtendDefinition " + exoOVALCriteriaExtendDefinition.Message + " " + exoOVALCriteriaExtendDefinition.InnerException);
                                }
                            }
                            #endregion OVALCRITERIAEXTENDDEFINITION
                            #endregion extenddefinition
                        }
                        else
                        {
                            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                            Console.WriteLine("ERROR: Import_oval missing code for definition criteria2 : " + nodeCriterion.Name);

                        }
                    }
                    #endregion criterions2
                }
            }
        }
    }
}
