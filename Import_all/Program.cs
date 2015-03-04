using System;
using System.Net;
using System.Globalization;
using System.Xml;
using System.Threading;
using System.Diagnostics;
using System.IO;
//using ICSharpCode.SharpZipLib.Zip;
using System.IO.Compression;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;


using OpenQA.Selenium;
using OpenQA.Selenium.IE;   //cloudflare bypass

using System.Configuration;
using System.Data.SqlClient;

using System.Text.RegularExpressions;
using XORCISMModel;
using XVULNERABILITYModel;
using XATTACKModel;
using XOVALModel;

namespace Import_all
{
    class Program
    {
        /// <summary>
        /// Copyright (C) 2013-2015 Jerome Athias
        /// Downloader and Parser for MITRE/NIST XML files (CWE, CVE) and import the values into an XORCISM database
        /// also get information from OSVDB (use at your own risks), exploit-db, SAINT, Metasploit...
        /// This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation; either version 2 of the License, or (at your option) any later version.
        /// 
        /// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
        /// 
        /// You should have received a copy of the GNU General Public License along with this program; if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
        /// </summary>

        //Use of the Selenium Driver as Global to keep Cloudflare happy
        public static OpenQA.Selenium.Chrome.ChromeDriver driver = null;

        public static int iVocabularyCVENVD = 0;// 1048;
        public static int iVocabularyCWEID = 0;// 20;
        public static int iVocabularyOVALID = 0;// 9;
        public static int iVocabularyOSVDBID = 0;// 1045;
        public static int iVocabularyMetasploitID = 0;// 1046;
        public static int iVocabularyEXPLOITDBID = 0;// 1047;
        public static int iVocabularyBID = 0;// 1049;
        public static int iVocabularySAINT = 0;// 1051;

        public static bool bRequestOSVDB = false;   //Get info from OSVDB.org
        public static bool bRequestExploitDB = false;    //Get info from EXPLOIT-DB.com
        public static bool bRequestSecurityfocus = false;    //Get info from securityfocus.com

        public static XORCISMEntities model = new XORCISMEntities();
        public static XVULNERABILITYModel.XVULNERABILITYEntities vuln_model=new XVULNERABILITYModel.XVULNERABILITYEntities();
        public static XOVALModel.XOVALEntities oval_model = new XOVALModel.XOVALEntities();
        public static XATTACKModel.XATTACKEntities attack_model = new XATTACKModel.XATTACKEntities();

        static void Main(string[] args)
        {
            //TODO: Encrypting Configuration Information Using Protected Configuration
            //http://msdn.microsoft.com/en-us/library/53tyfkaw%28v=vs.110%29.aspx

            //TODO: Pre-generating views
            //http://blog.3d-logic.com/2013/12/14/using-pre-generated-views-without-having-to-pre-generate-views-ef6/
            //http://msdn.microsoft.com/en-us/library/vstudio/bb896240(v=vs.100).aspx

            //XORCISMEntities model = new XORCISMEntities();
            //https://stackoverflow.com/questions/5940225/fastest-way-of-inserting-in-entity-framework
            model.Configuration.AutoDetectChangesEnabled = false;
            model.Configuration.ValidateOnSaveEnabled = false;

            //HARDCODED ***
            string sCWEVersion = "2.8";

            #region vocabularies
            //************************************************************************************
            //CVE
            #region vocabularycve
            try
            {
                iVocabularyCVENVD = model.VOCABULARY.Where(o => o.VocabularyName == "CVE").Select(o=>o.VocabularyID).FirstOrDefault();
            }
            catch(Exception ex)
            {

            }
            if (iVocabularyCVENVD <= 0)
            {
                XORCISMModel.VOCABULARY oVocabulary = new XORCISMModel.VOCABULARY();
                oVocabulary.CreatedDate = DateTimeOffset.Now;
                oVocabulary.VocabularyName = "CVE";
                model.VOCABULARY.Add(oVocabulary);
                model.SaveChanges();
                iVocabularyCVENVD = oVocabulary.VocabularyID;
                Console.WriteLine("DEBUG iVocabularyCVENVD=" + iVocabularyCVENVD);
            }
            #endregion vocabularycve
            //************************************************************************************
            //CWE
            #region vocabularycwe
            try
            {
                iVocabularyCWEID = model.VOCABULARY.Where(o => o.VocabularyName == "CWE" && o.VocabularyVersion==sCWEVersion).Select(o=>o.VocabularyID).FirstOrDefault();
            }
            catch (Exception ex)
            {

            }
            if (iVocabularyCWEID <= 0)
            {
                XORCISMModel.VOCABULARY oVocabulary = new XORCISMModel.VOCABULARY();
                oVocabulary.CreatedDate = DateTimeOffset.Now;
                oVocabulary.VocabularyName = "CWE";
                oVocabulary.VocabularyVersion = sCWEVersion;
                model.VOCABULARY.Add(oVocabulary);
                model.SaveChanges();
                iVocabularyCWEID = oVocabulary.VocabularyID;
                Console.WriteLine("DEBUG iVocabularyCWEID=" + iVocabularyCWEID);
            }
            #endregion vocabularycwe
            //************************************************************************************
            //OVAL
            #region vocabularyoval
            try
            {
                iVocabularyOVALID = model.VOCABULARY.Where(o => o.VocabularyName == "OVAL").Select(o=>o.VocabularyID).FirstOrDefault();
            }
            catch (Exception ex)
            {

            }
            if (iVocabularyOVALID <= 0)
            {
                XORCISMModel.VOCABULARY oVocabulary = new XORCISMModel.VOCABULARY();
                oVocabulary.CreatedDate = DateTimeOffset.Now;
                oVocabulary.VocabularyName = "OVAL";
                model.VOCABULARY.Add(oVocabulary);
                model.SaveChanges();
                iVocabularyOVALID = oVocabulary.VocabularyID;
                Console.WriteLine("DEBUG iVocabularyOVALID=" + iVocabularyOVALID);
            }
            #endregion vocabularyoval
            //************************************************************************************
            //OSVDB
            #region vocabularyosvdb
            try
            {
                iVocabularyOSVDBID = model.VOCABULARY.Where(o => o.VocabularyName == "OSVDB").Select(o=>o.VocabularyID).FirstOrDefault();
            }
            catch (Exception ex)
            {

            }
            if (iVocabularyOSVDBID <= 0)
            {
                XORCISMModel.VOCABULARY oVocabulary = new XORCISMModel.VOCABULARY();
                oVocabulary.CreatedDate = DateTimeOffset.Now;
                oVocabulary.VocabularyName = "OSVDB";
                model.VOCABULARY.Add(oVocabulary);
                model.SaveChanges();
                iVocabularyOSVDBID = oVocabulary.VocabularyID;
                Console.WriteLine("DEBUG iVocabularyOSVDBID=" + iVocabularyOSVDBID);
            }
            #endregion vocabularyosvdb
            //************************************************************************************
            //BID Securityfocus.com
            #region vocabularybid
            try
            {
                iVocabularyBID = model.VOCABULARY.Where(o => o.VocabularyName == "BID").Select(o=>o.VocabularyID).FirstOrDefault();
            }
            catch (Exception ex)
            {

            }
            if (iVocabularyBID <= 0)
            {
                XORCISMModel.VOCABULARY oVocabulary = new XORCISMModel.VOCABULARY();
                oVocabulary.CreatedDate = DateTimeOffset.Now;
                oVocabulary.VocabularyName = "BID";
                model.VOCABULARY.Add(oVocabulary);
                model.SaveChanges();
                iVocabularyBID = oVocabulary.VocabularyID;
                Console.WriteLine("DEBUG iVocabularyBID=" + iVocabularyBID);
            }
            #endregion vocabularybid
            //************************************************************************************
            //EXPLOIT-DB
            #region vocabularyexploitdb
            try
            {
                iVocabularyEXPLOITDBID = model.VOCABULARY.Where(o => o.VocabularyName == "EXPLOIT-DB").Select(o=>o.VocabularyID).FirstOrDefault();
            }
            catch (Exception ex)
            {

            }
            if (iVocabularyEXPLOITDBID <= 0)
            {
                XORCISMModel.VOCABULARY oVocabulary = new XORCISMModel.VOCABULARY();
                oVocabulary.CreatedDate = DateTimeOffset.Now;
                oVocabulary.VocabularyName = "EXPLOIT-DB";
                model.VOCABULARY.Add(oVocabulary);
                model.SaveChanges();
                iVocabularyEXPLOITDBID = oVocabulary.VocabularyID;
                Console.WriteLine("DEBUG iVocabularyEXPLOITDBID=" + iVocabularyEXPLOITDBID);
            }
            #endregion vocabularyexploitdb
            //************************************************************************************
            //Metasploit
            #region vocabularymetasploit
            try
            {
                iVocabularyMetasploitID = model.VOCABULARY.Where(o => o.VocabularyName == "Metasploit").Select(o=>o.VocabularyID).FirstOrDefault();
            }
            catch (Exception ex)
            {

            }
            if (iVocabularyMetasploitID <= 0)
            {
                XORCISMModel.VOCABULARY oVocabulary = new XORCISMModel.VOCABULARY();
                oVocabulary.CreatedDate = DateTimeOffset.Now;
                oVocabulary.VocabularyName = "Metasploit";
                model.VOCABULARY.Add(oVocabulary);
                model.SaveChanges();
                iVocabularyMetasploitID = oVocabulary.VocabularyID;
                Console.WriteLine("DEBUG iVocabularyMetasploitID=" + iVocabularyMetasploitID);
            }
            #endregion vocabularymetasploit
            //************************************************************************************
            //SAINT
            #region vocabularysaint
            try
            {
                iVocabularySAINT = model.VOCABULARY.Where(o => o.VocabularyName == "SAINT").Select(o=>o.VocabularyID).FirstOrDefault();
            }
            catch (Exception ex)
            {

            }
            if (iVocabularySAINT <= 0)
            {
                XORCISMModel.VOCABULARY oVocabulary = new XORCISMModel.VOCABULARY();
                oVocabulary.CreatedDate = DateTimeOffset.Now;
                oVocabulary.VocabularyName = "SAINT";
                model.VOCABULARY.Add(oVocabulary);
                model.SaveChanges();
                iVocabularySAINT = oVocabulary.VocabularyID;
                Console.WriteLine("DEBUG iVocabularySAINT=" + iVocabularySAINT);
            }
            #endregion vocabularysaint
            #endregion vocabularies

            Console.WriteLine("DEBUG "+DateTimeOffset.Now);
            try
            {
                //Updating Metasploit Framework
                System.Diagnostics.Process.Start(@"C:\metasploit\dev_msfupdate.bat");   //TODO HARDCODED
            }
            catch (Exception exUpdateMetasploit)
            {
                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                Console.WriteLine("Exception: exUpdateMetasploit " + exUpdateMetasploit.Message + " " + exUpdateMetasploit.InnerException);
            }
            
            //********************************************************************************************************************************
            //Download file from the feeds
            //TODO: Retrieve the last version automaticaly
            string sDownloadFileURL = string.Empty;     //"http://cwe.mitre.org/data/xml/cwec_v2.X.xml.zip"
            string sDownloadFileName = string.Empty;    //"cwec_v2.5.xml.zip"
            string sDownloadLocalPath = "C:/nvdcve/";   //TODO: Hardcoded
            string sDownloadLocalFolder = @"C:\nvdcve\";//TODO: Hardcoded
            string sDownloadLocalFile = string.Empty;   //"cwec_v2.6.xml"
            FileInfo fileInfo=null; //Used to get the local file size
            //FastZip fz = new FastZip(); //Now use .NET
            //ICSharpCode.SharpZipLib.GZip
            HttpWebRequest webRequest = null;
            HttpWebResponse webResponse = null;
            Int64 fileSizeRemote = new Int64();
            long fileSizeLocal = 0;

            //********************************************************************************************************************************
            //CWE
            sDownloadFileURL = "http://cwe.mitre.org/data/xml/cwec_v"+sCWEVersion+".xml.zip";
            sDownloadFileName = "cwec_v" + sCWEVersion + ".xml.zip";
            sDownloadLocalFile = "cwec_v" + sCWEVersion + ".xml";
            
          
            #region CWEDownloadImport
            // Create new FileInfo object and get the Length.
            fileInfo = new FileInfo(sDownloadLocalFolder+sDownloadFileName);
            try
            {
                fileSizeLocal = fileInfo.Length;
                Console.WriteLine("DEBUG " + sDownloadLocalFolder + sDownloadFileName + " FileSize:" + fileSizeLocal);
            }
            catch (Exception exfileSizeLocal)
            {
                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                Console.WriteLine("Exception: exfileSizeLocal " + exfileSizeLocal.Message + " " + exfileSizeLocal.InnerException);
            }

            try
            {
                Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                Console.WriteLine("DEBUG Getting filesize");
                webRequest = (HttpWebRequest)WebRequest.Create(new Uri(sDownloadFileURL));
                webRequest.Method = System.Net.WebRequestMethods.Http.Head;
                //webRequest.Credentials = CredentialCache.DefaultCredentials;
                webRequest.Timeout = 10*60*1000;    //10 minutes Hardcoded
                webResponse = (HttpWebResponse)webRequest.GetResponse();
                fileSizeRemote = webResponse.ContentLength;
                Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                Console.WriteLine("DEBUG " + sDownloadFileURL + " FileSize:" + fileSizeRemote);
            }
            catch (Exception exGetSizeDownload)
            {
                Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                Console.WriteLine("Exception: exGetSizeDownload " + exGetSizeDownload.Message + " " + exGetSizeDownload.InnerException);
            }

            if (fileSizeRemote == fileSizeLocal)
            {

            }
            else
            {
                // Download the file
                // http://cwe.mitre.org/data/index.html
                try
                {
                    WebClient wc = new WebClient();
                    //NOTE: we could ask/use Gzip
                    //http://technet.rapaport.com/info/prices/samplecode/gzip_request_sample.aspx
                    ////Console.WriteLine("Downloading cwec_v2.0.xml.zip");
                    ////wc.DownloadFile("http://cwe.mitre.org/data/xml/cwec_v2.0.xml.zip", "C:/nvdcve/cwec_v2.0.xml.zip");
                    Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                    Console.WriteLine("DEBUG Downloading " + sDownloadFileName);
                    wc.DownloadFile(sDownloadFileURL, sDownloadLocalPath + sDownloadFileName);
                    wc.Dispose();
                    Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                    Console.WriteLine("DEBUG Download completed");
                    ////Console.WriteLine("Download is completed", "info", MessageBoxButtons.OK, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);



                }
                catch (Exception ex)
                {
                    Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                    Console.WriteLine("ERROR: while downloading\n" + ex.Message + " " + ex.InnerException);
                }

                //Extract Zip File
                
                try
                {
                    //fz.ExtractZip(@"C:\nvdcve\cwec_v2.5.xml.zip", @"C:\nvdcve\", "");
                    //fz.ExtractZip(sDownloadLocalFolder + sDownloadFileName, sDownloadLocalFolder, "");
                    //With .NET 4.5

                    ZipArchive archive = ZipFile.Open(sDownloadLocalFolder + sDownloadFileName, ZipArchiveMode.Read);
                    archive.ExtractToDirectory(sDownloadLocalFolder);
                    archive.Dispose();
                    Console.WriteLine("DEBUG Extraction Complete");
                }
                catch (Exception exUnzip)
                {
                    Console.WriteLine("Exception: exUnzip: " + exUnzip.Message + " " + exUnzip.InnerException);
                }
            }

            Console.WriteLine("DEBUG "+DateTimeOffset.Now);
            
            if (fileSizeRemote != fileSizeLocal)
            {
                //            Console.WriteLine("Importing cwec_v1.12.xml");
                Console.WriteLine("DEBUG Importing " + sDownloadLocalFile);
                try
                {
                    Import_cwes(sDownloadLocalFolder + sDownloadLocalFile);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception: An error occurred :\n" + ex.Message + " " + ex.InnerException);
                }
                // Helper_ImportFile_cwe(@"C:\nvdcve\cwec_v1.8.xml");
                Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                Console.WriteLine("DEBUG IMPORT CWEs COMPLETED");

            }
            else
            {
                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                Console.WriteLine("DEBUG NOT IMPORTING CWEs (Same File Sizes)");
            }

            #endregion CWEDownloadImport

            
            //****************************************************************************************************************************************
            
            //****************************************************************************************************************************************

            //http://nvd.nist.gov/download.cfm

            //****************************************************************************************************************************************
            //CVE
            if (bRequestOSVDB)
            {
                driver = new OpenQA.Selenium.Chrome.ChromeDriver();
                driver.Navigate().GoToUrl("http://osvdb.org");  //Get the Cookie
            }

            #region CVEDownloadImport
            bool bUpdateCVEReferences = false;

            #region CVErecent
            sDownloadFileURL = "http://static.nvd.nist.gov/feeds/xml/cve/nvdcve-2.0-recent.xml";
            sDownloadFileName = "nvdcve-2.0-recent.xml";    //TODO Hardcoded
            sDownloadLocalFile = "nvdcve-2.0-recent.xml";
            fileInfo = new FileInfo(sDownloadLocalFolder + sDownloadFileName);
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
                Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                Console.WriteLine("DEBUG Getting filesize");
                webRequest = (HttpWebRequest)WebRequest.Create(new Uri(sDownloadFileURL));
                webRequest.Method = "HEAD";
                webRequest.UserAgent = "X";
                webRequest.Timeout = 20 * 60 * 1000;    //20 minutes Hardcoded
                webRequest.KeepAlive = true;
                //webRequest.Credentials = CredentialCache.DefaultCredentials;

                webResponse = (HttpWebResponse)webRequest.GetResponse();
                fileSizeRemote = webResponse.ContentLength;
                Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                Console.WriteLine("DEBUG " + sDownloadFileURL + " FileSize:" + fileSizeRemote);
                webResponse.Close();
            }
            catch (Exception exGetSizeDownload)
            {
                Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                Console.WriteLine("Exception: exGetSizeDownload " + exGetSizeDownload.Message + " " + exGetSizeDownload.InnerException);
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
                    Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                    Console.WriteLine("DEBUG Downloading " + sDownloadFileName);

                    //wc.DownloadFile(sDownloadFileURL, sDownloadLocalPath + sDownloadFileName);
                    //wc.Dispose();

                    webRequest = (HttpWebRequest)WebRequest.Create(new Uri(sDownloadFileURL));
                    webRequest.Method = "GET";
                    webRequest.UserAgent = "X";
                    webRequest.Headers.Add(System.Net.HttpRequestHeader.AcceptEncoding, "gzip");
                    //webRequest.Credentials = CredentialCache.DefaultCredentials;
                    webRequest.Timeout = 30 * 60 * 1000;    //30 minutes Hardcoded
                    webRequest.KeepAlive = true;
                    webResponse = (HttpWebResponse)webRequest.GetResponse();
                    Stream remoteStream = null;
                    //The following is to check if the server sending the response supports Gzip
                    if (webResponse.Headers.Get("Content-Encoding") != null &&
                    webResponse.Headers.Get("Content-Encoding").ToLower() == "gzip")
                    {
                        Console.WriteLine("DEBUG Using Gzip");
                        remoteStream = new GZipStream(webResponse.GetResponseStream(), CompressionMode.Decompress);
                    }
                    else
                    {
                        remoteStream = webResponse.GetResponseStream();
                    }
                    
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
                    webResponse.Close();
                    Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                    localStream.Dispose();
                    Console.WriteLine("DEBUG Download completed");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                    Console.WriteLine("Exception: while downloading\n" + ex.Message + " " + ex.InnerException);
                }


            }

            try
            {
                Import_cve(sDownloadLocalFolder + sDownloadLocalFile);
            }
            catch (Exception ex)
            {
                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                Console.WriteLine("Exception: Import_cve :\n" + ex.Message + " " + ex.InnerException);
            }
            #endregion CVErecent

            //************************************

            #region CVEmodified
            sDownloadFileURL = "http://static.nvd.nist.gov/feeds/xml/cve/nvdcve-2.0-modified.xml";
            sDownloadFileName = "nvdcve-2.0-modified.xml";  //TODO Hardcoded
            sDownloadLocalFile = "nvdcve-2.0-modified.xml";
            fileInfo = new FileInfo(sDownloadLocalFolder + sDownloadFileName);
            try
            {
                fileSizeLocal = fileInfo.Length;
                Console.WriteLine("DEBUG " + sDownloadLocalFolder + sDownloadFileName + " FileSize:" + fileSizeLocal);
            }
            catch (Exception exfileSizeLocal)
            {
                Console.WriteLine("Exception: exfileSizeLocal " + exfileSizeLocal.Message + " " + exfileSizeLocal.InnerException);
            }

            try
            {
                Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                Console.WriteLine("DEBUG Getting filesize");
                webRequest = (HttpWebRequest)WebRequest.Create(new Uri(sDownloadFileURL));
                webRequest.Method = "HEAD";
                webRequest.UserAgent = "X";
                webRequest.KeepAlive = true;
                //webRequest.Credentials = CredentialCache.DefaultCredentials;
                webRequest.Timeout = 20 * 60 * 1000;    //20 minutes Hardcoded
                webResponse = (HttpWebResponse)webRequest.GetResponse();
                fileSizeRemote = webResponse.ContentLength;
                Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                Console.WriteLine("DEBUG " + sDownloadFileURL + " FileSize:" + fileSizeRemote);

                webResponse.Close();
            }
            catch (Exception exGetSizeDownload)
            {
                Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                Console.WriteLine("Exception: exGetSizeDownload " + exGetSizeDownload.Message + " " + exGetSizeDownload.InnerException);
            }

            if (fileSizeRemote == fileSizeLocal)
            {

            }
            else
            {
                //Download the file
                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                Console.WriteLine("Downloading " + sDownloadFileName);
                /*
                try
                {
                    WebClient wc = new WebClient();
                    
                    wc.DownloadFile(sDownloadFileURL, sDownloadLocalPath + sDownloadFileName);
                    wc.Dispose();
                    Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                    Console.WriteLine("Download completed");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                    Console.WriteLine("ERROR: while downloading\n" + ex.Message + " " + ex.InnerException);

                    Console.WriteLine("DEBUG Trying DownloadMethod2");
                */
                    try
                    {
                        webRequest = (HttpWebRequest)WebRequest.Create(new Uri(sDownloadFileURL));
                        webRequest.Method = "GET";
                        //webRequest.Credentials = CredentialCache.DefaultCredentials;
                        webRequest.UserAgent = "X";
                        webRequest.KeepAlive = true;
                        //webRequest.Timeout = Timeout.Infinite;
                        webRequest.Timeout = 45 * 60 * 1000;    //45 minutes Hardcoded
                            webRequest.ReadWriteTimeout = 45 * 60 * 1000;    //45 minutes Hardcoded (default 5 minutes)
                        webRequest.Headers.Add(System.Net.HttpRequestHeader.AcceptEncoding, "gzip");
                        webResponse = (HttpWebResponse)webRequest.GetResponse();

                        Stream remoteStream = null;
                        //The following is to check if the server sending the response supports Gzip
                        if (webResponse.Headers.Get("Content-Encoding") != null &&
                        webResponse.Headers.Get("Content-Encoding").ToLower() == "gzip")
                        {
                            Console.WriteLine("DEBUG Using Gzip");
                            remoteStream = new GZipStream(webResponse.GetResponseStream(), CompressionMode.Decompress);
                        }
                        else
                        {
                            remoteStream = webResponse.GetResponseStream();
                        }
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
                        webResponse.Close();
                        Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                        localStream.Dispose();
                        Console.WriteLine("DEBUG Download is completed");
                    }
                    catch(Exception exDownloadMethod2)
                    {
                        Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                        Console.WriteLine("Exception: exDownloadMethod2 " + exDownloadMethod2.Message + " " + exDownloadMethod2.InnerException);
                    }
                //}


            }

            try
            {
                Import_cve(sDownloadLocalFolder + sDownloadLocalFile);
            }
            catch (Exception ex)
            {
                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                Console.WriteLine("Exception: An error occurred :\n" + ex.Message + " " + ex.InnerException);
            }
            #endregion CVEmodified

            //************************************
            

            //************************************
            int iCurrentYear = DateTime.Now.Year;
            //TODO https://cve.mitre.org/data/downloads/
            //Note: CVRF from 1999

            #region CVEALL
            for (int iYear=iCurrentYear;iYear>2001;iYear--)
            {
                //TODO: test the best link (speed and last update date)
                //http://static.nvd.nist.gov/feeds/xml/cve/nvdcve-2.0-2014.xml
                //https://nvd.nist.gov/static/feeds/xml/cve/nvdcve-2.0-2014.xml
                //http://nvd.nist.gov/download/nvdcve-2014.xml
 
                sDownloadFileURL = "http://static.nvd.nist.gov/feeds/xml/cve/nvdcve-2.0-" + iYear + ".xml";
                sDownloadFileName = "nvdcve-2.0-" + iYear + ".xml"; //TODO No .gz or .zip?
                sDownloadLocalFile = "nvdcve-2.0-" + iYear + ".xml";    //TODO Hardcoded
                bool iErrorTimeout = false;
            
                fileInfo = new FileInfo(sDownloadLocalFolder+sDownloadFileName);
                Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                try
                {
                    fileSizeLocal = fileInfo.Length;
                    Console.WriteLine("DEBUG " + sDownloadLocalFolder + sDownloadFileName + " FileSize:" + fileSizeLocal);
                }
                catch (Exception exfileSizeLocal)
                {
                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                    Console.WriteLine("Exception: exfileSizeLocal " + exfileSizeLocal.Message + " " + exfileSizeLocal.InnerException);
                }

                Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                Console.WriteLine("DEBUG Getting file size");
                try
                {
                    webRequest = (HttpWebRequest)WebRequest.Create(new Uri(sDownloadFileURL));
                    webRequest.Method = "HEAD";
                    //webRequest.Credentials = CredentialCache.DefaultCredentials;
                    webRequest.UserAgent = "X";
                    webRequest.KeepAlive = true;
                    //webRequest.Timeout = Timeout.Infinite;
                    webRequest.Timeout = 20*60*1000;    //20 minutes  //TODO Hardcoded
                    webResponse = (HttpWebResponse)webRequest.GetResponse();
                    fileSizeRemote = webResponse.ContentLength;
                    Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                    Console.WriteLine("DEBUG " + sDownloadFileURL + " FileSize:" + fileSizeRemote);
                    webResponse.Close();
                }
                catch (Exception exGetSizeDownload)
                {
                    Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                    Console.WriteLine("Exception: exGetSizeDownload " + sDownloadFileURL+" "+ exGetSizeDownload.Message + " " + exGetSizeDownload.InnerException);
                    if(exGetSizeDownload.Message.Contains("timed out"))
                    {
                        iErrorTimeout=true;
                    }
                }
                Console.WriteLine("DEBUG "+DateTimeOffset.Now);

                if (fileSizeRemote == fileSizeLocal)// || iErrorTimeout)
                {
                    Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                    Console.WriteLine("DEBUG NOT IMPORTING " + sDownloadFileName);
                }
                else
                {
                    //Download the file
                    try
                    {
                        /*
                        WebClient wc = new WebClient();
                        Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                        Console.WriteLine("Downloading " + sDownloadFileName);
                        wc.DownloadFile(sDownloadFileURL, sDownloadLocalPath + sDownloadFileName);
                        // 
                        wc.Dispose();
                        */
                        //TODO http://www.codeguru.com/columns/dotnettips/article.php/c7005/Downloading-Files-with-the-WebRequest-and-WebResponse-Classes.htm
                        Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                        Console.WriteLine("DEBUG Downloading " + sDownloadFileName);
                        webRequest = (HttpWebRequest)WebRequest.Create(new Uri(sDownloadFileURL));
                        //webRequest.Credentials = CredentialCache.DefaultCredentials;
                        webRequest.UserAgent = "X";
                        //webRequest.Timeout = Timeout.Infinite;
                        webRequest.KeepAlive = true;
                        webRequest.Timeout = 45*60*1000;    //45 minutes    //TODO Hardcoded
                        //    webRequest.ReadWriteTimeout = 45 * 60 * 1000;    //45 minutes Hardcoded (default 5 minutes)
                        webRequest.Headers.Add(System.Net.HttpRequestHeader.AcceptEncoding, "gzip");
                        webResponse = (HttpWebResponse)webRequest.GetResponse();

                        Stream remoteStream = null;
                        //The following is to check if the server sending the response supports Gzip
                        if (webResponse.Headers.Get("Content-Encoding") != null &&
                        webResponse.Headers.Get("Content-Encoding").ToLower() == "gzip")
                        {
                            remoteStream = new GZipStream(webResponse.GetResponseStream(), CompressionMode.Decompress);
                        }
                        else
                        {
                            remoteStream = webResponse.GetResponseStream();
                        }
                        
                        Stream localStream = File.Create(sDownloadLocalPath + sDownloadFileName);
                        // Allocate a 1k buffer
                        byte[] buffer = new byte[1024];
                        int bytesRead;

                        int bytesProcessed = 0;

                        // Simple do/while loop to read from stream until no bytes are returned
                        do
                        {
                            // Read data (up to 1k) from the stream
                            bytesRead = remoteStream.Read (buffer, 0, buffer.Length);
                            // Write the data to the local file
                            localStream.Write (buffer, 0, bytesRead);
                            // Increment total bytes processed
                            bytesProcessed += bytesRead;
                            //Console.WriteLine("DEBUG Downloading: " + bytesProcessed + " bytes processed");
                        } while (bytesRead > 0);
                        webResponse.Close();
                        Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                        localStream.Dispose();
                        Console.WriteLine("DEBUG Download is completed");
                        //Console.WriteLine("Download is completed", "info", MessageBoxButtons.OK, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                        Console.WriteLine("Exception: Error while downloading\n" + ex.Message + " " + ex.InnerException);
                    }
                }

                if (fileSizeRemote == fileSizeLocal)// || iErrorTimeout)
                {
                    Console.WriteLine("NOT IMPORTING CVE because same Files Size");
                }
                else
                {
                    try
                    {
                        Import_cve(sDownloadLocalFolder + sDownloadLocalFile);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                        Console.WriteLine("Exception: An error occurred :\n" + ex.Message + " " + ex.InnerException);
                    }
                }
            }
            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
            Console.WriteLine("IMPORT CVE COMPLETED");
            #endregion CVEALL

            #endregion CVEDownloadImport

            try
            {
                if (bRequestOSVDB)
                {
                    driver.Quit();
                }
            }
            catch(Exception exDriverQuit)
            {
                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                Console.WriteLine("Exception: exDriverQuit " + exDriverQuit.Message + " " + exDriverQuit.InnerException);
            }

            //****************************************************************************************************************************************
            
            ListCVE();
            Console.WriteLine("DEBUG "+DateTimeOffset.Now);
            Console.WriteLine("DEBUG CVEs updated!!!");

            //*********************************************************************************************************************************************************
            //CVE Reference Key/Maps
            #region CVErefmaps
            //http://cve.mitre.org/data/refs/index.html
            sDownloadFileURL = "http://cve.mitre.org/data/refs/refmap/allrefmaps.zip";
            sDownloadFileName = "allrefmaps.zip";
            //sDownloadLocalFile = "";  //TODO: contains multiple files
            fileInfo = new FileInfo(sDownloadLocalFolder+sDownloadFileName);
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
                webRequest = (HttpWebRequest)WebRequest.Create(new Uri(sDownloadFileURL));
                webRequest.Method = System.Net.WebRequestMethods.Http.Head;
                //webRequest.Credentials = CredentialCache.DefaultCredentials;
                //timeout
                webResponse = (HttpWebResponse)webRequest.GetResponse();
                fileSizeRemote = webResponse.ContentLength;
                Console.WriteLine("DEBUG " + sDownloadFileURL + " FileSize:" + fileSizeRemote);
            }
            catch (Exception exGetSizeDownload)
            {
                Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                Console.WriteLine("Exception: exGetSizeDownload " + exGetSizeDownload.Message + " " + exGetSizeDownload.InnerException);
            }

            if (fileSizeRemote == fileSizeLocal)
            {

            }
            else
            {
                //Download the file
                try
                {
                    WebClient wc = new WebClient();
                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                    Console.WriteLine("DEBUG Downloading allrefmaps.zip");
                    //TODO Hardcoded
                    wc.DownloadFile("http://cve.mitre.org/data/refs/refmap/allrefmaps.zip", "C:/nvdcve/allrefmaps.zip");
                    //TODO? method with stream

                    // 
                    wc.Dispose();
                    //Console.WriteLine("Download is completed", "info", MessageBoxButtons.OK, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                    Console.WriteLine("ERROR: while downloading\n" + ex.Message + " " + ex.InnerException);
                    //TODO: Download Method2
                }

                //Extract Zip File
                //FastZip fz = new FastZip();
                try
                {
                    //TODO Hardcoded
                    //fz.ExtractZip(@"C:\nvdcve\allrefmaps.zip", @"C:\nvdcve\", "");
                    ZipArchive archive = ZipFile.Open(@"C:\nvdcve\allrefmaps.zip", ZipArchiveMode.Read);
                    //archive.ExtractToDirectory(@"C:\nvdcve\");
                    foreach (ZipArchiveEntry file in archive.Entries)
                    {
                        string completeFileName = Path.Combine(@"C:\nvdcve\", file.FullName);
                        if (file.Name == "")
                        {// Assuming Empty for Directory
                            Directory.CreateDirectory(Path.GetDirectoryName(completeFileName));
                            continue;
                        }
                        Console.WriteLine("DEBUG Extracting " + completeFileName);
                        file.ExtractToFile(completeFileName, true);
                    }
                    Console.WriteLine("DEBUG Extraction Complete !!!");
                }
                catch (Exception exUnzipallrefmaps)
                {
                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                    Console.WriteLine("Exception: exUnzipallrefmaps: " + exUnzipallrefmaps.Message + " " + exUnzipallrefmaps.InnerException);
                    //The file 'C:\nvdcve\index.html' already exists.
                }
            }

            try
            {
                Import_CVEEXPLOITDB();
            }
            catch (Exception exImport_CVEEXPLOITDB)
            {
                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                Console.WriteLine("Exception: exImport_CVEEXPLOITDB: " + exImport_CVEEXPLOITDB.Message + " " + exImport_CVEEXPLOITDB.InnerException);
            }
            #endregion CVErefmaps
            //*********************************************************************************************************************************************************

            Import_saint();

            Import_metasploit();

            if (bRequestExploitDB)
            {
                Import_exploitdb();
            }

            //*********************************************************************************************************************************************************
            //CPE
            #region CPEDownloadImport
            //http://nvd.nist.gov/cpe.cfm
            sDownloadFileURL = "http://static.nvd.nist.gov/feeds/xml/cpe/dictionary/official-cpe-dictionary_v2.3.xml.gz";
            sDownloadFileName = "official-cpe-dictionary_v2.3.xml.gz";  //TODO HARDCODED
            sDownloadLocalFile = "official-cpe-dictionary_v2.3.xml";
            fileInfo = new FileInfo(sDownloadLocalFolder + sDownloadFileName);
            try
            {
                fileSizeLocal = fileInfo.Length;
                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                Console.WriteLine("DEBUG " + sDownloadLocalFolder + sDownloadFileName + " FileSize:" + fileSizeLocal);
            }
            catch (Exception exfileSizeLocal)
            {
                Console.WriteLine("Exception: exfileSizeLocal " + exfileSizeLocal.Message + " " + exfileSizeLocal.InnerException);
            }

            try
            {
                Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                Console.WriteLine("DEBUG Getting filesize");
                webRequest = (HttpWebRequest)WebRequest.Create(new Uri(sDownloadFileURL));
                webRequest.Method = "HEAD";
                //webRequest.Credentials = CredentialCache.DefaultCredentials;
                //timeout
                webResponse = (HttpWebResponse)webRequest.GetResponse();
                fileSizeRemote = webResponse.ContentLength;
                Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                Console.WriteLine("DEBUG " + sDownloadFileURL + " FileSize:" + fileSizeRemote);
                webResponse.Close();
            }
            catch (Exception exGetSizeDownload)
            {
                Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                Console.WriteLine("Exception: exGetSizeDownload " + exGetSizeDownload.Message + " " + exGetSizeDownload.InnerException);
            }

            if (fileSizeRemote == fileSizeLocal)
            {

            }
            else
            {
                //Download the file
                try
                {
                    WebClient wc = new WebClient();
                    ////Console.WriteLine("Downloading official-cpe-dictionary_v2.2.xml");
                    ////wc.DownloadFile("http://static.nvd.nist.gov/feeds/xml/cpe/dictionary/official-cpe-dictionary_v2.2.xml", "C:/nvdcve/official-cpe-dictionary_v2.2.xml");
                    Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                    Console.WriteLine("DEBUG Downloading " + sDownloadFileName);
                    wc.DownloadFile(sDownloadFileURL, sDownloadLocalPath + sDownloadFileName);
                    wc.Dispose();
                    Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                    Console.WriteLine("DEBUG Download completed");
                    //Console.WriteLine("Download is completed", "info", MessageBoxButtons.OK, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                    Console.WriteLine("ERROR: while downloading\n" + ex.Message + " " + ex.InnerException);
                }

                try
                {
                    fileInfo = new FileInfo(sDownloadLocalFolder + sDownloadFileName);
                    //fz.ExtractZip(sDownloadLocalFolder + sDownloadFileName, sDownloadLocalFolder, "");
                    DecompressGZip(fileInfo);
                    Console.WriteLine("DEBUG Extraction Completed");
                }
                catch (Exception exUnzip)
                {
                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                    Console.WriteLine("Exception: exUnzip: " + exUnzip.Message + " " + exUnzip.InnerException);
                }
            }

            if (fileSizeRemote == fileSizeLocal)
            {
                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                Console.WriteLine("DEBUG NOTE IMPORTANT: Not importing CPEs as same files size and it is a long process."); //NOTE: We assume that the CPEs were previously imported correctly
                //(TODO)
            }
            else
            {
                try
                {
                    Import_cpe(sDownloadLocalFolder + sDownloadLocalFile);
                    Console.WriteLine("DEBUG CPEs updated!!!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                    Console.WriteLine("Exception: An error occured :\n" + ex.Message + " " + ex.InnerException);
                }
            }
            Console.WriteLine("DEBUG "+DateTimeOffset.Now);
            Console.WriteLine("DEBUG IMPORT CPE COMPLETED");
            #endregion CPEDownloadImport

            //FREE
            model.Dispose();
        }


        static private void Import_cpe(string filepath)
        {
            Console.WriteLine("DEBUG *************************************************");
            Console.WriteLine("DEBUG "+DateTimeOffset.Now);
            Console.WriteLine("IMPORT CPE STARTED");

            XORCISMEntities model= new XORCISMEntities();
            model.Configuration.AutoDetectChangesEnabled = false;
            model.Configuration.ValidateOnSaveEnabled = false;

            XmlDocument docXML= new XmlDocument();
            try
            {
                docXML.Load(filepath);
            }
            catch (Exception exLoadCPEXML)
            {
                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                Console.WriteLine("Exception: exLoadCPEXML :\n" + exLoadCPEXML.Message + " " + exLoadCPEXML.InnerException);
                return;
            }

            XmlNamespaceManager mgrXML = new XmlNamespaceManager(docXML.NameTable);

            mgrXML.AddNamespace("z", "http://cpe.mitre.org/dictionary/2.0");
            mgrXML.AddNamespace("meta", "http://scap.nist.gov/schema/cpe-dictionary-metadata/0.2");

            XmlNodeList list;
            list = docXML.SelectNodes("/z:cpe-list/z:cpe-item", mgrXML);

            foreach (XmlNode node in list)
            {
                //TODO: Review with different versions
                Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                //foreach(

                    //CPEREFERENCE

                XmlNode nodeTitle;
                nodeTitle = node.SelectSingleNode("z:title", mgrXML);

                XmlNode nodeMetadata;
                nodeMetadata = node.SelectSingleNode("meta:item-metadata", mgrXML); //Note: Modified in 2.3

                XORCISMModel.CPE oCPE;
                string sCPEName = node.Attributes["name"].InnerText;
                oCPE = model.CPE.FirstOrDefault(o => o.CPEName == sCPEName);
                if (oCPE == null)
                {
                    // Console.WriteLine(string.Format("CPE [{0}] not found in table CPE", sCPEName));
                    Console.WriteLine(string.Format("DEBUG Adding new CPE [{0}] in table CPE", sCPEName));
                    try
                    {
                        oCPE = new CPE();
                        oCPE.CPEName = sCPEName;
                        oCPE.CPETitle = nodeTitle.InnerText;
                        oCPE.CreatedDate = DateTimeOffset.Now;
                        oCPE.timestamp = DateTimeOffset.Now;
                        //cpe.VocabularyID= //TODO
                        try
                        {
                            //Version < 2.3
                            oCPE.NVDID = Convert.ToInt32(nodeMetadata.Attributes["nvd-id"].InnerText);
                            oCPE.ModificationDate = DateTime.Parse(nodeMetadata.Attributes["modification-date"].InnerText, new System.Globalization.CultureInfo("EN-us"));
                            oCPE.Status = nodeMetadata.Attributes["status"].InnerText;
                        }
                        catch (Exception exAddToCPEVersion)
                        {
                            string sIgnoreWarning = exAddToCPEVersion.Message;
                            //Console.WriteLine("Exception: exAddToCPEVersion " + exAddToCPEVersion.Message + " " + exAddToCPEVersion.InnerException);
                        }
                        //TODO
                        model.CPE.Add(oCPE);
                        //model.SaveChanges();
                    }
                    catch (Exception exAddToCPE)
                    {
                        Console.WriteLine("Exception: exAddToCPE " + exAddToCPE.Message + " " + exAddToCPE.InnerException);
                    }
                }
                else
                {
                    //Update CPE
                    //Needed? (optimize)
                    try
                    {
                        //Console.WriteLine(string.Format("Updating CPE [{0}] in table CPE", sCPEName));
                        oCPE.CPETitle = nodeTitle.InnerText;    //TODO: Cleaning?
                        try
                        {
                            //Remove?
                            //For version < 2.3
                            oCPE.NVDID = Convert.ToInt32(nodeMetadata.Attributes["nvd-id"].InnerText);
                            oCPE.ModificationDate = DateTime.Parse(nodeMetadata.Attributes["modification-date"].InnerText, new System.Globalization.CultureInfo("EN-us"));
                            oCPE.Status = nodeMetadata.Attributes["status"].InnerText;
                        }
                        catch (Exception exUpdateCPEVersion)
                        {
                            string sIgnoreWarning = exUpdateCPEVersion.Message;
                            //Console.WriteLine("Exception: exUpdateCPEVersion " + exUpdateCPEVersion.Message + " " + exUpdateCPEVersion.InnerException);
                        }
                        
                        //TODO: more?


                    }
                    catch(Exception exUpdateCPE)
                    {
                        Console.WriteLine("Exception: exUpdateCPE " + exUpdateCPE.Message + " " + exUpdateCPE.InnerException);
                    }
                }
                try
                {
                    oCPE.timestamp = DateTimeOffset.Now;
                    model.SaveChanges();
                }
                catch (Exception exCPE)
                {
                    Console.WriteLine("Exception: exCPE " + exCPE.Message + " " + exCPE.InnerException);
                }
            }
            
            //FREE
            model.Dispose();
            model = null;
            Console.WriteLine("DEBUG "+DateTimeOffset.Now);
            Console.WriteLine("IMPORT CPE COMPLETED");
        }

        static private void Import_cve(string filepath)
        {
            //Trace.WriteLine(string.Format("Handling file [{0}]", Path.GetFileName(filepath)));

            bool bUpdateCVEReferences = false;    //TODO

            StreamWriter monStreamWriter = null;
            /*
            try
            {
                monStreamWriter = new StreamWriter("jerome2.log");
                Console.WriteLine("DEBUG jerome2.log opened");
            }
            catch(Exception exStreamWriter)
            {
                Console.WriteLine("Exception: exStreamWriter jerome2.log " + exStreamWriter.Message + " " + exStreamWriter.InnerException);
            }
            */
            XORCISMEntities model= new XORCISMEntities();
            model.Configuration.AutoDetectChangesEnabled = false;
            model.Configuration.ValidateOnSaveEnabled = false;

            CVSSCalculator oCVSSCalculator;
            oCVSSCalculator = new CVSSCalculator();

            XmlDocument docXML;
            docXML = new XmlDocument();
            try
            {
                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                docXML.Load(filepath);
                Console.WriteLine("DEBUG FILE LOADED " + filepath);
            }
            catch (Exception exLoadCVEXML)
            {
                Console.WriteLine("Exception: exLoadCVEXML :\n" + exLoadCVEXML.Message + " " + exLoadCVEXML.InnerException);
                monStreamWriter.Close();
                Console.WriteLine("DEBUG jerome2.log closed");
                return;
            }

            //TODO Hardcoded
            XmlNamespaceManager mgr = new XmlNamespaceManager(docXML.NameTable);
            mgr.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            mgr.AddNamespace("cvss", "http://scap.nist.gov/schema/cvss-v2/0.2");
            mgr.AddNamespace("cpe-lang", "http://cpe.mitre.org/language/2.0");
            mgr.AddNamespace("vuln", "http://scap.nist.gov/schema/vulnerability/0.4");
            mgr.AddNamespace("z", "http://scap.nist.gov/schema/feed/vulnerability/2.0");
            mgr.AddNamespace("schemaLocation", "http://scap.nist.gov/schema/feed/vulnerability/2.0 http://nvd.nist.gov/schema/nvd-cve-feed_2.0.xsd");


            XmlNodeList nodesCVE=null;  //entries
            try
            {
                nodesCVE = docXML.SelectNodes("/z:nvd/z:entry", mgr);
            }
            catch (Exception exSelectNodes)
            {
                Console.WriteLine("Exception: exLoadCVEXML :\n" + exSelectNodes.Message + " " + exSelectNodes.InnerException);
                monStreamWriter.Close();
                Console.WriteLine("DEBUG jerome2.log closed");
            }
            //int count = 0;
            //int progress = -1;

            foreach (XmlNode nodeCVE in nodesCVE)   //entry
            {
                string sCVEID = nodeCVE.Attributes["id"].InnerText;
                Console.WriteLine("DEBUG ***************************************************************");
                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                Console.WriteLine("DEBUG PROCESSING " + sCVEID);
                bool shouldsearchexploit = true;   //TODO change it

                try
                {
                    monStreamWriter = new StreamWriter("jerome2.log");  //Hardcoded
                    Console.WriteLine("DEBUG jerome2.log has been opened");
                }
                catch (Exception exStreamWriter2)
                {
                    Console.WriteLine("Exception: exStreamWriter2 jerome2.log " + exStreamWriter2.Message + " " + exStreamWriter2.InnerException);
                }

                // =========================
                #region cvss
                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                Console.WriteLine("DEBUG CVSS START");
                XmlNode nodeCVSSBaseMetrics=null;
                try
                {
                    nodeCVSSBaseMetrics = nodeCVE.SelectSingleNode("vuln:cvss/cvss:base_metrics", mgr);
                }
                catch(Exception exnodeCVSSBaseMetrics)
                {
                    Console.WriteLine("Exception: exnodeCVSSBaseMetrics " + exnodeCVSSBaseMetrics.Message + " " + exnodeCVSSBaseMetrics.InnerException);
                }
                double baseScore = 0.0;
                double impactSubscore = 0.0;
                double exploitabilitySubscore = 0.0;
                CVSSCalculator.AccessComplexity accessComplexity = CVSSCalculator.AccessComplexity.Medium;
                CVSSCalculator.Authentication authentication = CVSSCalculator.Authentication.None;
                CVSSCalculator.AccessVector accessVector = CVSSCalculator.AccessVector.Local;
                CVSSCalculator.ConfidentialityImpact confidentialityImpact = CVSSCalculator.ConfidentialityImpact.None;
                CVSSCalculator.IntegrityImpact integrityImpact = CVSSCalculator.IntegrityImpact.None;
                CVSSCalculator.AvailabilityImpact availabilityImpact = CVSSCalculator.AvailabilityImpact.None;

                if (nodeCVSSBaseMetrics == null)
                {
                    Console.WriteLine("DEBUG NO CVSS");
                    //continue; (jerome2.log needs to be closed)
                }
                else
                {
                    
                    double scoreFromFile = 0.0;

                    CultureInfo ci = new CultureInfo("en-US");

                    foreach (XmlNode nodeCVSSMetric in nodeCVSSBaseMetrics.ChildNodes)
                    {
                        switch (nodeCVSSMetric.LocalName)
                        {
                            case "score":
                                scoreFromFile = Convert.ToDouble(nodeCVSSMetric.InnerText, ci);
                                break;

                            case "access-vector":
                                accessVector = Helper_ParseAccessVector(nodeCVSSMetric.InnerText);
                                break;

                            case "access-complexity":
                                accessComplexity = Helper_ParseAccessComplexity(nodeCVSSMetric.InnerText);
                                break;

                            case "authentication":
                                authentication = Helper_ParseAuthentication(nodeCVSSMetric.InnerText);
                                break;

                            case "confidentiality-impact":
                                confidentialityImpact = Helper_ParseConfidentialiyImpact(nodeCVSSMetric.InnerText);
                                break;

                            case "integrity-impact":
                                integrityImpact = Helper_ParseIntegrityImpact(nodeCVSSMetric.InnerText);
                                break;

                            case "availability-impact":
                                availabilityImpact = Helper_ParseAvailabilityImpact(nodeCVSSMetric.InnerText);
                                break;
                            case "source":
                                //TODO? Missing code
                                break;
                            case "generated-on-datetime":
                                //TODO? Missing code
                                break;
                            default:
                                Console.WriteLine("ERROR: Missing code for nodeCVSSMetric " + nodeCVSSMetric.LocalName);
                                break;
                        }
                    }

                    

                    oCVSSCalculator.Calculate(accessComplexity, authentication, accessVector, confidentialityImpact, integrityImpact, availabilityImpact, out baseScore, out impactSubscore, out exploitabilitySubscore);

                    baseScore = Helper_Round(baseScore);
                    impactSubscore = Helper_Round(impactSubscore);
                    exploitabilitySubscore = Helper_Round(exploitabilitySubscore);

                    if (baseScore != scoreFromFile)
                    {
                        //                    Console.WriteLine("ERROR: CVSS "+cveid + " " + scoreFromFile + " " + baseScore);
                    }
                }
                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                Console.WriteLine("DEBUG END CVSS");
                #endregion cvss

                // ===============

                XmlNode nodeCVESummary;
                nodeCVESummary = nodeCVE.SelectSingleNode("vuln:summary", mgr);

                XmlNode nodeCVEPublishedDate;
                nodeCVEPublishedDate = nodeCVE.SelectSingleNode("vuln:published-datetime", mgr);

                XmlNode nodeCVEModifiedDate;
                nodeCVEModifiedDate = nodeCVE.SelectSingleNode("vuln:last-modified-datetime", mgr);
                Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                Console.WriteLine("DEBUG END SUMMARY");

                // =================================
                // Create or reuse the vulnerability
                // =================================
                #region vulnerability
                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                Console.WriteLine("DEBUG VULNERABILITYCVE");
                VULNERABILITY oCVE=null;
                int iVulnerabilityID = 0;
                try
                {
                    //TODO: cve hardcoded => VULRefentialID
                    oCVE = vuln_model.VULNERABILITY.FirstOrDefault(o => o.VULReferential == "cve" && o.VULReferentialID == sCVEID);
                    if (oCVE == null)
                    {
                        try
                        {
                            oCVE = new VULNERABILITY();
                            oCVE.VULReferential = "cve";
                            oCVE.VULReferentialID = sCVEID;
                            oCVE.CreatedDate = DateTimeOffset.Now;
                            //oCVE.timestamp = DateTimeOffset.Now;
                            //oCVE.VocabularyID = iVocabularyCVENVD;
                            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                            Console.WriteLine("DEBUG AddToVULNERABILITY " + sCVEID);
                            vuln_model.VULNERABILITY.Add(oCVE);
                            //TODO : Search exploit-db in this case only
                            shouldsearchexploit = true;
                        }
                        catch(Exception exVULNERABILITY44)
                        {
                            Console.WriteLine("Exception: exVULNERABILITY44 " + exVULNERABILITY44.Message + " " + exVULNERABILITY44.InnerException);
                        }
                    }
                    else
                    {
                        Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                        Console.WriteLine("DEBUG CVE known " + sCVEID+" "+oCVE.VulnerabilityID);
                        //Update VULNERABILITY

                    }

                    try
                    {
                        //TODO if update, test VULModifiedDate

                        //oCVE.VULReferential = "cve";
                        //oCVE.VULReferentialID = sCVEID;
                        //oCVE.ValueMD5 = CreateMD5Hash(cveid);
                        oCVE.VULDescription = nodeCVESummary.InnerText;  //Cleaning?
                        oCVE.VULConsequence = "";
                        oCVE.VULSolution = "";

                        oCVE.VULPublishedDate = DateTimeOffset.Parse(nodeCVEPublishedDate.InnerText, new System.Globalization.CultureInfo("EN-us"));
                        oCVE.VULModifiedDate = DateTimeOffset.Parse(nodeCVEModifiedDate.InnerText, new System.Globalization.CultureInfo("EN-us"));
                        oCVE.CVSSBaseScore = baseScore;
                        oCVE.CVSSImpactSubscore = impactSubscore;
                        oCVE.CVSSExploitabilitySubscore = exploitabilitySubscore;
                        oCVE.CVSSMetricAccessVector = accessVector.ToString();
                        oCVE.CVSSMetricAccessComplexity = accessComplexity.ToString();
                        oCVE.CVSSMetricAuthentication = authentication.ToString();
                        oCVE.CVSSMetricConfImpact = confidentialityImpact.ToString();
                        oCVE.CVSSMetricIntegImpact = integrityImpact.ToString();
                        oCVE.CVSSMetricAvailImpact = availabilityImpact.ToString();
                        oCVE.VocabularyID = iVocabularyCVENVD;    //TODO? CVE
                        oCVE.timestamp = DateTimeOffset.Now;
                        try
                        {
                            vuln_model.SaveChanges();
                            iVulnerabilityID = oCVE.VulnerabilityID;
                            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                            Console.WriteLine("DEBUG CVE CVSS updated");
                        }
                        catch (Exception exoCVE)
                        {
                            Console.WriteLine("Exception: exoCVE " + exoCVE.Message + " " + exoCVE.InnerException);
                        }
                    }
                    catch(Exception exoCVE02)
                    {
                        Console.WriteLine("Exception: exoCVE02 " + exoCVE02.Message + " " + exoCVE02.InnerException);
                    }
                }
                catch(Exception exoCVE)
                {
                    Console.WriteLine("Exception: exoCVE " + exoCVE.Message + " " + exoCVE.InnerException);
                }
                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                Console.WriteLine("DEBUG END VULNERABILITY");
                #endregion vulnerability

                // ===============
                // Handle the CPEs
                // ===============

                
                #region vulnerableconfiguration
                //vulnerable-configuration
                //VULNERABLECONFIGURATION
                //Note: a VULNERABLECONFIGURATION is constructed like an OVALDEFINITION
                //VULNERABLECONFIGURATIONCPE
                //CPELOGICALTEST
                //Note: a CPELOGICALTEST is constructed like an OVALCRITERIA
                ////TODO?: we can have a CPELOGICALTEST for a CPELOGICALTEST (like OVALCRITERION) according to cpe-specification 2.1
                ////http://cpe.mitre.org/files/cpe-specification_2.1.pdf

                int iVulnerableConfigurationOrder = 0;
                Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                Console.WriteLine("DEBUG VULNERABLECONFIGURATION");
                
                XmlNodeList nodelistVulnerableConfigurations = null;
                try
                {
                    nodelistVulnerableConfigurations = nodeCVE.SelectNodes("vuln:vulnerable-configuration", mgr);
                    //for each vulnerable configuration
                    foreach (XmlNode nodeVulnConfig in nodelistVulnerableConfigurations)
                    {
                        iVulnerableConfigurationOrder++;
                        int iVulnConfigID = 0;
                        try
                        {
                            iVulnConfigID = model.VULNERABLECONFIGURATION.Where(o => o.VulnerabilityID == oCVE.VulnerabilityID && o.ConfigurationOrder == iVulnerableConfigurationOrder).Select(o => o.VulnerableConfigurationID).FirstOrDefault();
                        }
                        catch(Exception ex)
                        {

                        }
                        if(iVulnConfigID<=0)
                        {
                            Console.WriteLine("DEBUG Adding VULNERABLECONFIGURATION " + iVulnerableConfigurationOrder);
                            VULNERABLECONFIGURATION oVulnConfig = new VULNERABLECONFIGURATION();
                            oVulnConfig.CreatedDate = DateTimeOffset.Now;
                            oVulnConfig.VulnerabilityID = oCVE.VulnerabilityID;
                            oVulnConfig.ConfigurationOrder = iVulnerableConfigurationOrder;
                            oVulnConfig.VocabularyID = iVocabularyCVENVD;
                            oVulnConfig.timestamp = DateTimeOffset.Now;
                            model.VULNERABLECONFIGURATION.Add(oVulnConfig);
                            model.SaveChanges();
                            iVulnConfigID = oVulnConfig.VulnerableConfigurationID;
                        }
                        else
                        {
                            //Update VULNERABLECONFIGURATION
                        }

                        fctParseVulnerableConfigurationTest(nodeVulnConfig, iVulnConfigID);

                        
                    }
                }
                catch(Exception exVulnerableConfiguration)
                {
                    Console.WriteLine("Exception: exVulnerableConfiguration " + exVulnerableConfiguration.Message + " " + exVulnerableConfiguration.InnerException);
                }
                #endregion vulnerableconfiguration

                #region CPEs
                Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                Console.WriteLine("DEBUG CVECPEs"); //TODO: optimize
                XmlNodeList nodelistVulnerableSoftwares = null;
                try
                {
                    nodelistVulnerableSoftwares = nodeCVE.SelectNodes("vuln:vulnerable-software-list/vuln:product", mgr);

                    foreach (XmlNode nodeCPE in nodelistVulnerableSoftwares)
                    {
                        string sCPEName = nodeCPE.InnerText;
                        //cpe:/a:ibm:security_appscan:8.8:-:standard

                        //Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                        //Console.WriteLine("DEBUG Searching CPE "+sCPEName);

                        //CPE cpe = null;
                        //Optimization because no update
                        //cpe = model.CPE.FirstOrDefault(o => o.CPEName == sCPEName);
                        //if (cpe == null)
                        int iCPEID = 0;
                        try
                        {
                            //iCPEID=model.CPE.Where(o => o.CPEName == sCPEName).Select(o => o.CPEID).FirstOrDefault();
                            iCPEID = model.CPE.Where(o => o.CPEName == sCPEName).Select(o=>o.CPEID).FirstOrDefault();
                        }
                        catch(Exception ex)
                        {

                        }
                        if (iCPEID <= 0)
                        {
                            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                            Console.WriteLine("DEBUG Adding CPE "+sCPEName);
                            //Trace.WriteLine(string.Format("CPE [{0}] not found in table CPE (Adding it)", sCPEName));


                            try
                            {
                                CPE cpe = new CPE();
                                cpe.CPEName = sCPEName;
                                cpe.CreatedDate = DateTimeOffset.Now;
                                cpe.timestamp = DateTimeOffset.Now;
                                cpe.VocabularyID=iVocabularyCVENVD;
                                model.CPE.Add(cpe);
                                model.SaveChanges();
                                iCPEID = cpe.CPEID;
                            }
                            catch (Exception exCPE)
                            {
                                Console.WriteLine("Exception: exCPE " + exCPE.Message + " " + exCPE.InnerException);
                            }
                        }
                        else
                        {
                            //Update CPE
                        }

                        //if (cpe.VULNERABILITY.Contains(oCVE) == false)
                        //    cpe.VULNERABILITY.Add(oCVE);
                        //Optimization because no update
                        //VULNERABILITYFORCPE oVULCPE = null;
                        //oVULCPE=model.VULNERABILITYFORCPE.FirstOrDefault(o => o.VulnerabilityID == oCVE.VulnerabilityID && o.CPEID == iCPEID);
                        int iCPEVulnerabilityID = 0;
                        try
                        {
                            //iCPEVulnerabilityID=model.VULNERABILITYFORCPE.Where(o => o.VulnerabilityID == oCVE.VulnerabilityID && o.CPEID == iCPEID).Select(o => o.CPEVulnerabilityID).FirstOrDefault();
                            iCPEVulnerabilityID = vuln_model.VULNERABILITYFORCPE.Where(o => o.VulnerabilityID == iVulnerabilityID && o.CPEID == iCPEID).Select(o => o.CPEVulnerabilityID).FirstOrDefault();
                            
                        }
                        catch(Exception ex)
                        {

                        }
                        //if (oVULCPE != null)
                        if (iCPEVulnerabilityID>0)
                        {
                            //Update VULNERABILITYFORCPE
                            //TODO
                        }
                        else
                        {
                            try
                            {
                                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                Console.WriteLine("DEBUG Adding VULNERABILITYFORCPE");
                                VULNERABILITYFORCPE oVULCPE = new VULNERABILITYFORCPE();
                                oVULCPE.CPEID = iCPEID; //cpe.CPEID;
                                oVULCPE.VulnerabilityID = iVulnerabilityID; // oCVE.VulnerabilityID;
                                oVULCPE.CreatedDate = DateTimeOffset.Now;
                                oVULCPE.timestamp = DateTimeOffset.Now;
                                oVULCPE.isKnownVulnerable = true;   //TODO Review (something else?)
                                oVULCPE.VocabularyID = iVocabularyCVENVD;
                                vuln_model.VULNERABILITYFORCPE.Add(oVULCPE);
                                vuln_model.SaveChanges();
                            }
                            catch (Exception exVULNERABILITYFORCPE)
                            {
                                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                Console.WriteLine("Exception: exVULNERABILITYFORCPE " + exVULNERABILITYFORCPE.Message + " " + exVULNERABILITYFORCPE.InnerException);
                            }
                        }


                    }
                }
                catch(Exception exVULCPE)
                {
                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                    Console.WriteLine("Exception: exVULCPE " + exVULCPE.Message + " " + exVULCPE.InnerException);
                }
                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                Console.WriteLine("DEBUG END CPEs");
                #endregion CPEs

                // ===============
                // Handle the CWEs
                // ===============
                #region CWEs
                Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                Console.WriteLine("DEBUG CVECWEs");
                try
                {
                    XmlNodeList nodesCWE = nodeCVE.SelectNodes("vuln:cwe", mgr);
                    foreach (XmlNode nodeCWE in nodesCWE)
                    {
                        string sCWEID;
                        sCWEID = nodeCWE.Attributes["id"].InnerText;
                        //Console.WriteLine("CWEID:" + s);
                        if (sCWEID != "")
                        {
                            //Optimization because no update
                            //CWE oCWE = null;
                            //oCWE=model.CWE.Where(o => o.CWEID == sCWEID).FirstOrDefault();
                            string sCWEIDTest = model.CWE.Where(o => o.CWEID == sCWEID).Select(o=>o.CWEID).FirstOrDefault();
                            //if (oCWE == null)
                            if (sCWEIDTest == null)
                            {
                                
                                    //XORCISMModel.CWE oCWE;
                                    CWE oCWE = new CWE();
                                    oCWE.CWEID = sCWEID;
                                    //TODO A VOIR
                                    oCWE.CWEName = "";
                                    oCWE.CWEDescriptionSummary = "";
                                    //oCWE.CWEDescriptionSummaryClean = "";
                                    oCWE.CreatedDate = DateTimeOffset.Now;
                                    oCWE.timestamp = DateTimeOffset.Now;
                                    oCWE.VocabularyID = iVocabularyCVENVD;
                                    //TODO: Try Catch
                                    //oCVE.CWE.Add(oCWE);
                                    model.CWE.Add(oCWE);
                                    model.SaveChanges();
                                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                    Console.WriteLine("DEBUG CWEID: " + sCWEID + " added");

                                 
                            }
                            else
                            {
                                //Update CWE
                                //CWE exists
                            }
                            
                            //VULNERABILITYFORCWE oVULCWE = null;
                            //Optimization because no update
                            //oVULCWE=model.VULNERABILITYFORCWE.FirstOrDefault(o => o.CWEID == sCWEID && o.VulnerabilityID == oCVE.VulnerabilityID);
                            int iCWEVulnerabilityID = 0;
                            try
                            {
                                //iCWEVulnerabilityID=model.VULNERABILITYFORCWE.Where(o => o.CWEID == sCWEID && o.VulnerabilityID == oCVE.VulnerabilityID).Select(o => o.CWEVulnerabilityID).FirstOrDefault();
                                iCWEVulnerabilityID = vuln_model.VULNERABILITYFORCWE.Where(o => o.CWEID == sCWEID && o.VulnerabilityID == iVulnerabilityID).Select(o => o.CWEVulnerabilityID).FirstOrDefault();
                                        
                            }
                            catch(Exception ex)
                            {

                            }
                            //if (oVULCWE == null)
                            if (iCWEVulnerabilityID<=0)
                            {
                                VULNERABILITYFORCWE oVULCWE = new VULNERABILITYFORCWE();
                                oVULCWE.CWEID = sCWEID;
                                oVULCWE.VulnerabilityID = iVulnerabilityID; // oCVE.VulnerabilityID;
                                oVULCWE.CreatedDate = DateTimeOffset.Now;
                                oVULCWE.timestamp = DateTimeOffset.Now;
                                oVULCWE.VocabularyID = iVocabularyCVENVD;
                                vuln_model.VULNERABILITYFORCWE.Add(oVULCWE);
                                vuln_model.SaveChanges();
                            }
                            else
                            {
                                //Update VULNERABILITYFORCWE
                            }
                            
                        }
                    }
                }
                catch(Exception exCWEs)
                {
                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                    Console.WriteLine("Exception: exCWEs " + exCWEs.Message + " " + exCWEs.InnerException);
                }
                #endregion CWEs

                // =====================
                // Handle the references
                // =====================
                #region CVEReferences
                try
                {
                    //oCVE.REFERENCE.Clear();
                    Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                    Console.WriteLine("DEBUG CVEReferences");

                    #region searchBIDForCVE
                    //TODO Check LastDateChecked
                    //Request POST to http://www.securityfocus.com/bid
                    //string CVElook = sCVEID;
                    string ResponseText = "";
                    //string MyCookie = "";
                    
                    try
                    {
                        

                        if (bRequestSecurityfocus)
                        {
                            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                            Console.WriteLine("DEBUG SearchBIDForCVE " + sCVEID);

                            StreamReader SR = null;
                            HttpWebResponse response = null;

                            HttpWebRequest request;
                            string sURLSearchBIDForCVE = "http://www.securityfocus.com/bid";    //?CVE=" + CVElook;

                            Console.WriteLine("DEBUG Request to " + sURLSearchBIDForCVE);

                            request = (HttpWebRequest)HttpWebRequest.Create(sURLSearchBIDForCVE);
                            request.Method = "POST";
                            string postData = "op=display_list&c=12&vendor=&title=&version=&CVE=" + sCVEID;    //TODO Hardcoded
                            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                            request.ContentLength = byteArray.Length;
                            // Get the request stream.
                            Stream dataStream = request.GetRequestStream();
                            // Write the data to the request stream.
                            dataStream.Write(byteArray, 0, byteArray.Length);
                            // Close the Stream object.
                            dataStream.Close();

                            response = (HttpWebResponse)request.GetResponse();
                            SR = new StreamReader(response.GetResponseStream());
                            ResponseText = SR.ReadToEnd();

                            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                            Console.WriteLine("DEBUG Response obtained");

                            response.Close();
                            SR.Close();

                            //TODO save the file
                            string sCurrentPath = Directory.GetCurrentDirectory();
                            System.IO.File.WriteAllText(sCurrentPath + @"\securityfocus\" + sCVEID + ".txt", ResponseText);

                        }

                        

                        //Console.WriteLine("DEBUG TODO ResponseText=" + ResponseText);

                        //TODO read the file


                        //TODO: collect the Vendors

                        /*
                        <a href="/bid/50832"><span class="headline">Ubuntu Update Manager Insecure Temporary Directory Creation Vulnerability</span></a><br/>
            <span class="date">2011-11-28</span><br/>
            <a href="/bid/50832">http://www.securityfocus.com/bid/50832</a><br/><br/>
                        */

                        Regex myRegex = new Regex("<span class=\"headline\">(.*?)<br/><br/>", RegexOptions.Singleline);
                        MatchCollection myBIDs = myRegex.Matches(ResponseText);
                        foreach (Match matchBID in myBIDs)
                        {
                            foreach (Capture capture01 in matchBID.Captures)
                            {
                                //Console.WriteLine(capture01.Value);

                                myRegex = new Regex("<span class=\"headline\">(.*?)</span>");
                                string sBIDTitle = myRegex.Match(capture01.Value).ToString();
                                sBIDTitle = sBIDTitle.Replace("<span class=\"headline\">", "");
                                sBIDTitle = sBIDTitle.Replace("</span>", "");
                                Console.WriteLine("DEBUG sBIDTitle=" + sBIDTitle);
                                //TODO search CVE-

                                myRegex = new Regex("<span class=\"date\">(.*?)</span>");
                                string sBIDDate = myRegex.Match(capture01.Value).ToString();
                                sBIDDate = sBIDDate.Replace("<span class=\"date\">", "");
                                sBIDDate = sBIDDate.Replace("</span>", "");
                                Console.WriteLine("DEBUG sBIDDate=" + sBIDDate);

                                myRegex = new Regex("bid/(.*?)\">");
                                string sBIDID = myRegex.Match(capture01.Value).ToString();
                                sBIDID = sBIDID.Replace("bid/", "");
                                sBIDID = sBIDID.Replace("\">", "");
                                Console.WriteLine("DEBUG sBIDID=" + sBIDID);

                                //TODO Check if the values are coherent
                                try
                                {
                                    int iReferenceID = 0;
                                    REFERENCE oReferenceBID = model.REFERENCE.Where(o => o.Source == "BID" && o.ReferenceSourceID == sBIDID).FirstOrDefault();
                                    if (oReferenceBID == null)
                                    {
                                        oReferenceBID = new REFERENCE();
                                        oReferenceBID.Source = "BID";
                                        oReferenceBID.ReferenceSourceID = sBIDID;
                                        oReferenceBID.ReferenceTitle = sBIDTitle;
                                        oReferenceBID.ReferenceURL = "http://securityfocus.com/bid/" + sBIDID;
                                        oReferenceBID.Reference_Date = sBIDDate;
                                        oReferenceBID.CreatedDate = DateTimeOffset.Now;
                                        oReferenceBID.VocabularyID=iVocabularyBID; //BID securityfocus
                                        oReferenceBID = REFERENCENormalize(model,oReferenceBID);
                                        model.REFERENCE.Add(oReferenceBID);
                                        model.SaveChanges();
                                    }
                                    else
                                    {
                                        //Update REFERENCE BID
                                        
                                        oReferenceBID.ReferenceTitle = sBIDTitle;
                                        oReferenceBID = REFERENCENormalize(model,oReferenceBID);

                                    }

                                    //oReferenceBID.LastCheckedDate = DateTimeOffset.Now;
                                    oReferenceBID.timestamp = DateTimeOffset.Now;
                                    model.SaveChanges();
                                    iReferenceID = oReferenceBID.ReferenceID;

                                    #region VULNERABILITYFORREFERENCE
                                    int iVulnerabilityReferenceID = 0;
                                    try
                                    {
                                        iVulnerabilityReferenceID = vuln_model.VULNERABILITYFORREFERENCE.Where(o => o.VulnerabilityID == iVulnerabilityID && o.ReferenceID == iReferenceID).Select(o => o.VulnerabilityReferenceID).FirstOrDefault();
                                    }
                                    catch (Exception exVulnerabilityReferenceID)
                                    {
                                        iVulnerabilityReferenceID = 0;
                                    }
                                    if (iVulnerabilityReferenceID <= 0)
                                    {
                                        VULNERABILITYFORREFERENCE oVULReference = new VULNERABILITYFORREFERENCE();
                                        oVULReference.ReferenceID = iReferenceID;
                                        oVULReference.VulnerabilityID = iVulnerabilityID;   // oCVE.VulnerabilityID;
                                        oVULReference.CreatedDate = DateTimeOffset.Now;
                                        oVULReference.timestamp = DateTimeOffset.Now;
                                        
                                        oVULReference.VocabularyID=iVocabularyBID;   //BID securityfocus
                                        vuln_model.VULNERABILITYFORREFERENCE.Add(oVULReference);
                                        vuln_model.SaveChanges();
                                        //iVulnerabilityReferenceID=
                                    }
                                    else
                                    {
                                        //Update VULNERABILITYFORREFERENCE
                                    }
                                    #endregion VULNERABILITYFORREFERENCE
                                }
                                catch(Exception exReferenceBID)
                                {
                                    Console.WriteLine("Exception: exReferenceBID " + exReferenceBID.Message + " " + exReferenceBID.InnerException);
                                }

                                

                            }
                        }

                    }
                    catch (Exception exsearchBIDForCVE)
                    {
                        Console.WriteLine("Exception: exsearchBIDForCVE " + exsearchBIDForCVE.Message + " " + exsearchBIDForCVE.InnerException);
                        //Console.WriteLine(string.Format("Exception = {0}. Retrying...", ex));
                        //SearchBIDForCVE(CVElook, monStreamWriter);
                        
                    }
                    #endregion searchBIDForCVE


                    XmlNodeList nodesReferences = nodeCVE.SelectNodes("vuln:references", mgr);
                    foreach (XmlNode nodeReference in nodesReferences)
                    {
                        string sReferenceSource = string.Empty;
                        string sReferenceTitle = string.Empty;
                        string sReferenceType = string.Empty;
                        string sReferenceURL = string.Empty;
                        try
                        {
                            sReferenceSource = nodeReference.SelectSingleNode("vuln:source", mgr).InnerText;
                            //Uri uri = new Uri(sReferenceSource);
                            //string requested = uri.Scheme + uri.SchemeDelimiter + uri.Host + ":" + uri.Port;
                            //string output = uri.GetLeftPart(UriPartial.Authority);
                            //TODO
                            sReferenceSource = sReferenceSource.Replace("http://www.", "http://");
                            sReferenceSource = sReferenceSource.Replace("https://www.", "https://");
                            //TODO getsource()

                            sReferenceTitle = nodeReference.SelectSingleNode("vuln:reference", mgr).InnerText;
                            sReferenceTitle = sReferenceTitle.Replace("http://www.", "http://");
                            sReferenceTitle = sReferenceTitle.Replace("https://www.", "https://");
                            
                            sReferenceType = nodeReference.Attributes["reference_type"].InnerText;
                            //TODO? store in a table
                            sReferenceURL = nodeReference.SelectSingleNode("vuln:reference", mgr).Attributes["href"].InnerText;
                        }
                        catch (Exception exnodeReference01)
                        {
                            Console.WriteLine("Exception: exnodeReference01 " + exnodeReference01.Message + " " + exnodeReference01.InnerException);
                        }
                        /*
                        sReferenceURL = sReferenceURL.Replace("http://osvdb.org", "http://www.osvdb.org");
                        sReferenceURL = sReferenceURL.Replace("http://www.osvdb.org/displayvuln.php?osvdbid=", "http://www.osvdb.org/");
                        sReferenceURL = sReferenceURL.Replace("http://securitytracker.com", "http://www.securitytracker.com");
                        sReferenceURL = sReferenceURL.Replace("http://secunia.com", "http://www.secunia.com");
                        sReferenceURL = sReferenceURL.Replace("http://exploit-db.com", "http://www.exploit-db.com");
                        sReferenceURL = sReferenceURL.Replace("http://milw0rm.com", "http://www.milw0rm.com");
                        sReferenceURL = sReferenceURL.Replace("http://securityfocus.com", "http://www.securityfocus.com");
                        //securityreason.com
                        //attrition.org
                        //http://www.osvdb.org
                        */
                        //Clean ReferenceURL
                        sReferenceURL = sReferenceURL.Replace("http://www.", "http://");    //TODO: Review, e.g. www.exploit-db.com
                        sReferenceURL = sReferenceURL.Replace("https://www.", "https://");
                        sReferenceURL = sReferenceURL.Replace("http://osvdb.org/displayvuln.php?osvdbid=", "http://osvdb.org/");
                        sReferenceURL = sReferenceURL.Replace("http://osvdb.org/show/osvdb/", "http://osvdb.org/");
                        sReferenceURL = sReferenceURL.Replace("exploit-db.com/download/", "exploit-db.com/exploits/");
                        //TODO
                        sReferenceSource = ReferenceSource(sReferenceURL);

                        //https://github.com/rapid7/metasploit-framework/blob/master/modules/exploits/windows/fileformat/microp_mppl.rb
                        //TODO? Get references, etc. from the MSF modules

                        //REFERENCE oReference = null;
                        int iReferenceID = 0;
                        try
                        {
                            ////Optimization because no update
                            
                            Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                            Console.WriteLine("DEBUG REFERENCE Method1");
                            ////oReference = model.REFERENCE.FirstOrDefault(o => o.Source == sReferenceSource && o.ReferenceTitle == sReferenceTitle && o.ReferenceURL == sReferenceURL);
                            try
                            {
                                //TODO RegexMS microsoft.com?
                                iReferenceID = model.REFERENCE.Where(o => o.ReferenceURL == sReferenceURL).Select(o=>o.ReferenceID).FirstOrDefault();
                            }
                            catch(Exception ex)
                            {

                            }
                            //oReference = model.REFERENCE.FirstOrDefault(o => o.ReferenceURL == sReferenceURL);
                            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                            Console.WriteLine("DEBUG sReferenceURL=" + sReferenceURL);

                            /*
                            Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                            Console.WriteLine("DEBUG REFERENCE Method2");
                            try
                            {
                                //TODO: Encrypting Configuration Information Using Protected Configuration
                                //http://msdn.microsoft.com/en-us/library/53tyfkaw%28v=vs.110%29.aspx
                                //AppSettings
                                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["XORCISMConnectionString"].ConnectionString.ToString());
                                //SqlConnection con = new SqlConnection(Properties.Settings.Default.myConnectionString);
                                con.Open();

                                string cmdText = "select ReferenceID from REFERENCE where ReferenceURL='" + sReferenceURL + "'";

                                using (SqlCommand cmd = new SqlCommand(cmdText, con))
                                using (SqlDataReader dr = cmd.ExecuteReader())
                                {
                                    while (dr.Read())
                                    {
                                        iReferenceID = iReferenceID;
                                    }
                                }
                                con.Close();
                            }
                            catch(Exception exSqlCommand)
                            {
                                Console.WriteLine("Exception: exSqlCommand " + exSqlCommand.Message + " " + exSqlCommand.InnerException);
                            }
                            
                             */
                            
                            Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                            Console.WriteLine("DEBUG REFERENCE processing");

                            //Note: && o.Type == type removed because leading to duplicates UNKNOWN/VENDOR_ADVISORY
                            //if (oReference == null)
                            //{
                                ////Check if o.Url == url here to avoid duplicates
                                //oReference = model.REFERENCE.FirstOrDefault(o => o.ReferenceURL == sReferenceURL);
                                //if (oReference == null)
                                if (iReferenceID<=0)
                                {
                                    
                                    try
                                    {
                                        REFERENCE oReference = new REFERENCE();
                                        oReference.Source = sReferenceSource;
                                        oReference.ReferenceTitle = sReferenceTitle;
                                        //TODO
                                        //if exploit-db
                                        //if securityfocus BID
                                        //oReference.ReferenceSourceID=

                                        oReference.Type = sReferenceType;
                                        oReference.ReferenceURL = sReferenceURL;            //Normalized after
                                        oReference.CreatedDate = DateTimeOffset.Now;
                                        oReference.timestamp = DateTimeOffset.Now;
                                        oReference.VocabularyID = iVocabularyCVENVD;
                                        oReference = REFERENCENormalize(model,oReference);
                                        model.REFERENCE.Add(oReference);
                                        model.SaveChanges();    ////NOTE: Here because we don't update
                                        iReferenceID = oReference.ReferenceID;
                                    }
                                    catch(Exception exREFERENCEAdd)
                                    {
                                        Console.WriteLine("Exception: exREFERENCEAdd " + exREFERENCEAdd.Message + " " + exREFERENCEAdd.InnerException);
                                    }
                                }
                                else
                                {
                                    //Update REFERENCE

                                    //TODO REMOVE
                                    //oReference = REFERENCENormalize(model,oReference);
                                    //model.SaveChanges();

                                    
                                    //TODO: if(bUpdateCVEReferences)
                                    /*
                                    //TODO: Update if Type="UNKNOWN" => VENDOR_ADVISORY
                                    //Console.WriteLine("ERROR: A REFERENCE already exists for the same URL but different Source or Title");
                                    //Update Reference
                                    if (oReference.Source.Trim() == "")
                                    {
                                        oReference.Source = sReferenceSource;
                                        Console.WriteLine("ERROR: The empty source is replaced by " + sReferenceSource);
                                    }
                                    //TODO
                                    if (oReference.Source != sReferenceSource)
                                    {
                                        Console.WriteLine("ERROR: Source in DB=" + oReference.Source + " Source in File=" + sReferenceSource);

                                    }
                                    
                                    if (oReference.ReferenceTitle != sReferenceTitle)
                                    {
                                        //TODO Review
                                        string sTempURL = oReference.ReferenceTitle.Replace("http://www.", "http://");
                                        sTempURL = sTempURL.Replace("https://www.", "https://");
                                        if (sTempURL != sReferenceTitle)
                                        {
                                            Console.WriteLine("ERROR: ReferenceTitle in DB=" + oReference.ReferenceTitle + " ReferenceTitle in File=" + sReferenceTitle);
                                        }
                                        else
                                        {
                                            //oReference.ReferenceTitle URL was not shortened
                                            Console.WriteLine("DEBUG Note: oReference.ReferenceTitle URL was not shortened");
                                            oReference.ReferenceTitle = sReferenceTitle;
                                            //oReference.timestamp = DateTimeOffset.Now;
                                            //model.SaveChanges();
                                        }
                                    }
                                    //TODO
                                    //Go to the URL and catch the Title of the page (at least Secunia, OSVDB, exploit-db...)

                                    */
                                }


                            /*
                            }
                            else
                            {
                                //TODO: Update if Type="UNKNOWN" => VENDOR_ADVISORY
                            }
                            */

                            //NOTE: Don't update each CVE because too slow
                                //TODO if(bUpdateCVEReferences)
                            /*
                            try
                            {
                                oReference.timestamp = DateTimeOffset.Now;
                                model.SaveChanges();
                                Console.WriteLine("DEBUG CVE UPDATED");
                            }
                            catch (Exception exCVEReference)
                            {
                                Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                                Console.WriteLine("Exception: exCVEReference " + exCVEReference.Message + " " + exCVEReference.InnerException);
                            }
                            */
                            Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                            Console.WriteLine("DEBUG VULNERABILITYFORREFERENCE");
                            //oCVE.REFERENCE.Add(reference);
                            #region VULNERABILITYFORREFERENCE
                            //Optimization because no update
                            //VULNERABILITYFORREFERENCE oVULReference = model.VULNERABILITYFORREFERENCE.FirstOrDefault(o => o.VulnerabilityID == oCVE.VulnerabilityID && o.ReferenceID == oReference.ReferenceID);
                            //if (oVULReference == null)
                            int iVulnerabilityReferenceID = 0;
                            try
                            {
                                iVulnerabilityReferenceID = vuln_model.VULNERABILITYFORREFERENCE.Where(o => o.VulnerabilityID == iVulnerabilityID && o.ReferenceID == iReferenceID).Select(o => o.VulnerabilityReferenceID).FirstOrDefault();
                            }
                            catch (Exception exVulnerabilityReferenceID)
                            {
                                iVulnerabilityReferenceID = 0;
                            }
                            if (iVulnerabilityReferenceID<=0)
                            {
                                VULNERABILITYFORREFERENCE oVULReference = new VULNERABILITYFORREFERENCE();
                                oVULReference.ReferenceID = iReferenceID;   // oReference.ReferenceID;
                                oVULReference.VulnerabilityID = iVulnerabilityID;
                                oVULReference.CreatedDate = DateTimeOffset.Now;
                                oVULReference.timestamp = DateTimeOffset.Now;
                                
                                oVULReference.VocabularyID = iVocabularyCVENVD;
                                vuln_model.VULNERABILITYFORREFERENCE.Add(oVULReference);
                                vuln_model.SaveChanges();
                                //iVulnerabilityReferenceID=
                            }
                            else
                            {
                                //Update VULNERABILITYFORREFERENCE
                            }
                            #endregion VULNERABILITYFORREFERENCE
                        }
                        catch (Exception exReference)
                        {
                            Console.WriteLine("Exception: exReference " + exReference.Message + " " + exReference.InnerException);

                        }


                        //EXPLOIT
                        Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                        Console.WriteLine("DEBUG EXPLOIT");
                        //***
                        //http://exploit-db.com/exploits/
                        Regex myRegex = new Regex("exploit-db.com[^<>]*");  //TODO REVIEW
                        string strTemp = myRegex.Match(sReferenceURL).ToString();
                        if (strTemp != "")
                        {
                            sReferenceURL = sReferenceURL.Replace("https://", "http://");
                            sReferenceURL = sReferenceURL.Replace("http://www.", "http://");
                            sReferenceURL = sReferenceURL.Replace("/download/", "/exploits/");
                            //Check if the sploit already exists in the db
                            //Optimization because no update
                            //EXPLOIT oExploit = model.EXPLOIT.FirstOrDefault(o => o.Referential == "exploit-db" && o.Location == sReferenceURL);
                            //TODO REVIEW use of location   sEDBID
                            int iExploitID = 0;
                            try
                            {
                                iExploitID=model.EXPLOIT.Where(o => o.ExploitReferential == "exploit-db" && o.ExploitLocation == sReferenceURL).Select(o => o.ExploitID).FirstOrDefault();
                            }
                            catch(Exception ex)
                            {

                            }
                            //int sExploitID = 0;

                            //if(oExploit==null)
                            if (iExploitID <= 0)
                            {
                                //ExploitID = syn.ToList().First().ExploitID;
                                EXPLOIT oExploit = new EXPLOIT();
                                oExploit.ExploitReferential = "exploit-db";
                                string sEDBID = sReferenceURL.Replace("http://exploit-db.com/exploits/", "");
                                oExploit.ExploitRefID = sEDBID;
                                oExploit.ExploitLocation = sReferenceURL;
                                oExploit.CreatedDate = DateTimeOffset.Now;
                                oExploit.timestamp = DateTimeOffset.Now;
                                oExploit.VocabularyID = iVocabularyCVENVD;
                                model.EXPLOIT.Add(oExploit);
                                model.SaveChanges();

                                Console.WriteLine("DEBUG Added Exploit EDBID:" + sEDBID + " (minimum information)");
                                iExploitID = oExploit.ExploitID;

                                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                Console.WriteLine("DEBUG Search more information for exploit EDBID:" + sEDBID);
                                //                SearchExploits(cveid, monStreamWriter);
                                SearchExploitsForCVE(sCVEID, monStreamWriter);  //jerome2.log?
                                //Here can't read jerome2.log because locked
                                monStreamWriter.Close();
                                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                Console.WriteLine("DEBUG jerome2.log closed");

                                ParseEXPLOITDB("jerome2.log", false);  //bAppend=false to overwite the file
                                InsertEXPLOITDB();

                                //Search securityfocus
                                //SearchBIDForCVE(sCVEID, monStreamWriter);

                            }
                            else
                            {
                                //Update EXPLOIT
                            }
                            //sExploitID = oExploit.ExploitID;

                            //EXPLOITFORCPE
                            #region EXPLOITFORCPE
                            Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                            Console.WriteLine("DEBUG EXPLOITFORCPE");
                            foreach (XmlNode nodeCPE in nodelistVulnerableSoftwares)
                            {
                                string sCPEID = nodeCPE.InnerText;
                                int iCPEID = model.CPE.Where(o => o.CPEName == sCPEID).Select(o => o.CPEID).FirstOrDefault();
                                if (iCPEID <= 0)
                                {
                                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                    Console.WriteLine("ERROR: the CPE does not exist in the CPE table");
                                    //TODO
                                }

                                //Optimization because no update
                                //EXPLOITFORCPE oCPEExploit = model.EXPLOITFORCPE.FirstOrDefault(o => o.CPEID == sCPEID && o.ExploitID == iExploitID);
                                int iCPEExploitID = 0;
                                try
                                {
                                    iCPEExploitID = model.EXPLOITFORCPE.Where(o => o.CPEID == iCPEID && o.ExploitID == iExploitID).Select(o => o.CPEExploitID).FirstOrDefault();
                                }
                                catch (Exception ex)
                                {

                                }
                                //if (oCPEExploit == null)
                                if (iCPEExploitID <= 0)
                                {
                                    EXPLOITFORCPE oCPEExploit = new EXPLOITFORCPE();
                                    oCPEExploit.CPEID = iCPEID;
                                    //Console.WriteLine("DEBUG sEDBID=" + sEDBID);
                                    Console.WriteLine("DEBUG CPEName=" + sCPEID);
                                    oCPEExploit.CPEName = sCPEID;
                                    oCPEExploit.ExploitID = iExploitID;
                                    oCPEExploit.CreatedDate = DateTimeOffset.Now;
                                    oCPEExploit.timestamp = DateTimeOffset.Now;
                                    oCPEExploit.VocabularyID = iVocabularyCVENVD;
                                    model.EXPLOITFORCPE.Add(oCPEExploit);
                                    model.SaveChanges();
                                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                    Console.WriteLine("DEBUG Added EXPLOITFORCPE");
                                }
                                else
                                {
                                    //Update EXPLOITFORCPE

                                }
                            }
                            #endregion EXPLOITFORCPE

                            #region EXPLOITFORVULNERABILITY
                            Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                            Console.WriteLine("DEBUG EXPLOITFORVULNERABILITY");
                            //Optimization because no update
                            //EXPLOITFORVULNERABILITY oVulnerabilityExploit = model.EXPLOITFORVULNERABILITY.FirstOrDefault(o => o.VulnerabilityID == oCVE.VulnerabilityID && o.ExploitID == iExploitID);
                            int iVulnerabilityExploitID = 0;
                            try
                            {
                                iVulnerabilityExploitID = model.EXPLOITFORVULNERABILITY.Where(o => o.VulnerabilityID == iVulnerabilityID && o.ExploitID == iExploitID).Select(o => o.VulnerabilityExploitID).FirstOrDefault();
                            }
                            catch (Exception ex)
                            {

                            }
                            //if (oVulnerabilityExploit == null)
                            if (iVulnerabilityExploitID <= 0)
                            {

                                try
                                {
                                    EXPLOITFORVULNERABILITY oVulnerabilityExploit = new EXPLOITFORVULNERABILITY();
                                    oVulnerabilityExploit.VulnerabilityID = iVulnerabilityID;
                                    oVulnerabilityExploit.ExploitID = iExploitID;   //sploit.ExploitID;
                                    oVulnerabilityExploit.CreatedDate = DateTimeOffset.Now;
                                    oVulnerabilityExploit.timestamp = DateTimeOffset.Now;
                                    oVulnerabilityExploit.VocabularyID = iVocabularyCVENVD;
                                    model.EXPLOITFORVULNERABILITY.Add(oVulnerabilityExploit);
                                    model.SaveChanges();
                                    //iVulnerabilityExploitID=
                                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                    Console.WriteLine("DEBUG Added EXPLOITFORVULNERABILITY");
                                }
                                catch (Exception exAddToEXPLOITFORVULNERABILITY1)
                                {
                                    Console.WriteLine("Exception: exAddToEXPLOITFORVULNERABILITY1 " + exAddToEXPLOITFORVULNERABILITY1.Message + " " + exAddToEXPLOITFORVULNERABILITY1.InnerException);
                                }
                            }
                            #endregion EXPLOITFORVULNERABILITY

                            #region EXPLOITFORREFERENCE
                            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                            Console.WriteLine("DEBUG EXPLOITFORREFERENCE");
                            //Optimization because no update
                            //EXPLOITFORREFERENCE oExploitReference = model.EXPLOITFORREFERENCE.FirstOrDefault(o => o.ExploitID == iExploitID && o.ReferenceID == oReference.ReferenceID);
                            int iExploitReferenceID = 0;
                            try
                            {
                                iExploitReferenceID = model.EXPLOITFORREFERENCE.Where(o => o.ExploitID == iExploitID && o.ReferenceID == iReferenceID).Select(o => o.ExploitReferenceID).FirstOrDefault();
                            }
                            catch (Exception ex)
                            {

                            }
                            //if (oExploitReference == null)
                            if (iExploitReferenceID <= 0)
                            {
                                EXPLOITFORREFERENCE oExploitReference = new EXPLOITFORREFERENCE();
                                oExploitReference.ExploitID = iExploitID;
                                oExploitReference.ReferenceID = iReferenceID;   // oReference.ReferenceID;
                                oExploitReference.CreatedDate = DateTimeOffset.Now;
                                oExploitReference.timestamp = DateTimeOffset.Now;
                                oExploitReference.VocabularyID = iVocabularyCVENVD;
                                model.EXPLOITFORREFERENCE.Add(oExploitReference);
                                model.SaveChanges();
                                //iExploitReferenceID=
                                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                Console.WriteLine("DEBUG Added EXPLOITFORREFERENCE");
                            }
                            else
                            {
                                //Update EXPLOITFORREFERENCE
                            }
                            #endregion EXPLOITFORREFERENCE
                        }
                        else
                        {
                            //*********************************************************************
                            #region exploitmilw0rm
                            myRegex = new Regex("milw0rm.com[^<>]*");   //TODO REVIEW
                            strTemp = myRegex.Match(sReferenceURL).ToString();
                            if (strTemp != "")
                            {
                                //Check if the sploit already exist in the db
                                int iExploitID = 0;
                                try
                                {
                                    iExploitID=model.EXPLOIT.Where(o => o.ExploitReferential == "milw0rm" && o.ExploitLocation == sReferenceURL).Select(o => o.ExploitID).FirstOrDefault();
                                }
                                catch (Exception ex)
                                {

                                }
                                /*
                                int ExploitID = 0;
                                //TODO Review Optimize?
                                var syn = from S in model.EXPLOIT
                                          where S.ExploitReferential.Equals("milw0rm")
                                          && S.ExploitLocation.Equals(sReferenceURL)
                                          select S.ExploitID;
                                if (syn.Count() == 0)
                                */
                                if (iExploitID<=0)
                                {
                                    //ExploitID = syn.ToList().First().ExploitID;
                                    EXPLOIT sploit = new EXPLOIT();
                                    sploit.ExploitReferential = "milw0rm";
                                    sploit.ExploitRefID = sReferenceURL.Replace("http://milw0rm.com/exploits/", "");
                                    sploit.ExploitLocation = sReferenceURL;
                                    sploit.CreatedDate = DateTimeOffset.Now;
                                    sploit.timestamp = DateTimeOffset.Now;
                                    sploit.VocabularyID = iVocabularyCVENVD;
                                    model.EXPLOIT.Add(sploit);
                                    model.SaveChanges();
                                    //ExploitID = sploit.ExploitID;
                                    iExploitID = sploit.ExploitID;
                                    #region oldremove
                                    /*
                                    try
                                    {
                                        EXPLOITFORVULNERABILITY sploitvuln = new EXPLOITFORVULNERABILITY();
                                        sploitvuln.VulnerabilityID = oCVE.VulnerabilityID;
                                        sploitvuln.ExploitID = ExploitID;   //sploit.ExploitID;
                                        sploitvuln.CreatedDate = DateTimeOffset.Now;
                                        sploitvuln.timestamp = DateTimeOffset.Now;
                                        sploitvuln.VocabularyID = iVocabularyCVENVD;
                                        model.EXPLOITFORVULNERABILITY.Add(sploitvuln);
                                        model.SaveChanges();
                                    }
                                    catch (Exception exAddToEXPLOITFORVULNERABILITY)
                                    {
                                        Console.WriteLine("Exception: exAddToEXPLOITFORVULNERABILITY " + exAddToEXPLOITFORVULNERABILITY.Message + " " + exAddToEXPLOITFORVULNERABILITY.InnerException);
                                    }

                                    EXPLOITFORREFERENCE sploitreference = new EXPLOITFORREFERENCE();
                                    sploitreference.ExploitID = ExploitID;
                                    sploitreference.ReferenceID = iReferenceID; //oReference.ReferenceID;
                                    sploitreference.VocabularyID = iVocabularyCVENVD;
                                    sploitreference.CreatedDate = DateTimeOffset.Now;
                                    sploitreference.timestamp = DateTimeOffset.Now;
                                    model.EXPLOITFORREFERENCE.Add(sploitreference);
                                    model.SaveChanges();
                                    */
                                    #endregion oldremove
                                }
                                else
                                {
                                    //Update EXPLOIT
                                }

                                //EXPLOITFORCPE
                                #region EXPLOITFORCPE
                                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                Console.WriteLine("DEBUG EXPLOITFORCPE");
                                foreach (XmlNode nodeCPE in nodelistVulnerableSoftwares)
                                {
                                    string sCPEID = nodeCPE.InnerText;
                                    int iCPEID = model.CPE.Where(o => o.CPEName == sCPEID).Select(o => o.CPEID).FirstOrDefault();
                                    if (iCPEID <= 0)
                                    {
                                        Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                        Console.WriteLine("ERROR: the CPE does not exist in the CPE table");
                                        //TODO
                                    }

                                    //Optimization because no update
                                    //EXPLOITFORCPE oCPEExploit = model.EXPLOITFORCPE.FirstOrDefault(o => o.CPEID == sCPEID && o.ExploitID == iExploitID);
                                    int iCPEExploitID = 0;
                                    try
                                    {
                                        iCPEExploitID = model.EXPLOITFORCPE.Where(o => o.CPEID == iCPEID && o.ExploitID == iExploitID).Select(o => o.CPEExploitID).FirstOrDefault();
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                    //if (oCPEExploit == null)
                                    if (iCPEExploitID <= 0)
                                    {
                                        EXPLOITFORCPE oCPEExploit = new EXPLOITFORCPE();
                                        oCPEExploit.CPEID = iCPEID;
                                        //Console.WriteLine("DEBUG sEDBID=" + sEDBID);
                                        Console.WriteLine("DEBUG CPEName=" + sCPEID);
                                        oCPEExploit.CPEName = sCPEID;
                                        oCPEExploit.ExploitID = iExploitID;
                                        oCPEExploit.CreatedDate = DateTimeOffset.Now;
                                        oCPEExploit.timestamp = DateTimeOffset.Now;
                                        oCPEExploit.VocabularyID = iVocabularyCVENVD;
                                        model.EXPLOITFORCPE.Add(oCPEExploit);
                                        model.SaveChanges();
                                        Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                        Console.WriteLine("DEBUG Added EXPLOITFORCPE");
                                    }
                                    else
                                    {
                                        //Update EXPLOITFORCPE

                                    }
                                }
                                #endregion EXPLOITFORCPE

                                #region EXPLOITFORVULNERABILITY
                                Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                                Console.WriteLine("DEBUG EXPLOITFORVULNERABILITY");
                                //Optimization because no update
                                //EXPLOITFORVULNERABILITY oVulnerabilityExploit = model.EXPLOITFORVULNERABILITY.FirstOrDefault(o => o.VulnerabilityID == oCVE.VulnerabilityID && o.ExploitID == iExploitID);
                                int iVulnerabilityExploitID = 0;
                                try
                                {
                                    iVulnerabilityExploitID = model.EXPLOITFORVULNERABILITY.Where(o => o.VulnerabilityID == iVulnerabilityID && o.ExploitID == iExploitID).Select(o => o.VulnerabilityExploitID).FirstOrDefault();
                                }
                                catch (Exception ex)
                                {

                                }
                                //if (oVulnerabilityExploit == null)
                                if (iVulnerabilityExploitID <= 0)
                                {

                                    try
                                    {
                                        EXPLOITFORVULNERABILITY oVulnerabilityExploit = new EXPLOITFORVULNERABILITY();
                                        oVulnerabilityExploit.VulnerabilityID = iVulnerabilityID;
                                        oVulnerabilityExploit.ExploitID = iExploitID;   //sploit.ExploitID;
                                        oVulnerabilityExploit.CreatedDate = DateTimeOffset.Now;
                                        oVulnerabilityExploit.timestamp = DateTimeOffset.Now;
                                        oVulnerabilityExploit.VocabularyID = iVocabularyCVENVD;
                                        model.EXPLOITFORVULNERABILITY.Add(oVulnerabilityExploit);
                                        model.SaveChanges();
                                        Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                        Console.WriteLine("DEBUG Added EXPLOITFORVULNERABILITY");
                                    }
                                    catch (Exception exAddToEXPLOITFORVULNERABILITY1)
                                    {
                                        Console.WriteLine("Exception: exAddToEXPLOITFORVULNERABILITY1 " + exAddToEXPLOITFORVULNERABILITY1.Message + " " + exAddToEXPLOITFORVULNERABILITY1.InnerException);
                                    }
                                }
                                #endregion EXPLOITFORVULNERABILITY

                                #region EXPLOITFORREFERENCE
                                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                Console.WriteLine("DEBUG EXPLOITFORREFERENCE");
                                //Optimization because no update
                                //EXPLOITFORREFERENCE oExploitReference = model.EXPLOITFORREFERENCE.FirstOrDefault(o => o.ExploitID == iExploitID && o.ReferenceID == oReference.ReferenceID);
                                int iExploitReferenceID = 0;
                                try
                                {
                                    iExploitReferenceID = model.EXPLOITFORREFERENCE.Where(o => o.ExploitID == iExploitID && o.ReferenceID == iReferenceID).Select(o => o.ExploitReferenceID).FirstOrDefault();
                                }
                                catch (Exception ex)
                                {

                                }
                                //if (oExploitReference == null)
                                if (iExploitReferenceID <= 0)
                                {
                                    EXPLOITFORREFERENCE oExploitReference = new EXPLOITFORREFERENCE();
                                    oExploitReference.ExploitID = iExploitID;
                                    oExploitReference.ReferenceID = iReferenceID;   // oReference.ReferenceID;
                                    oExploitReference.CreatedDate = DateTimeOffset.Now;
                                    oExploitReference.timestamp = DateTimeOffset.Now;
                                    oExploitReference.VocabularyID = iVocabularyCVENVD;
                                    model.EXPLOITFORREFERENCE.Add(oExploitReference);
                                    model.SaveChanges();
                                    //iExploitReferenceID=
                                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                    Console.WriteLine("DEBUG Added EXPLOITFORREFERENCE");
                                }
                                else
                                {
                                    //Update EXPLOITFORREFERENCE
                                }
                                #endregion EXPLOITFORREFERENCE
                            }
                            #endregion exploitmilw0rm
                            else
                            {
                                //*********************************************************************
                                #region exploitbid
                                myRegex = new Regex("securityfocus.com/bid/[^<>]*/exploit");    //TODO Review
                                strTemp = myRegex.Match(sReferenceURL).ToString();
                                if (strTemp != "")
                                {
                                    //Check if the sploit already exist in the db
                                    int iExploitID = 0;
                                    try
                                    {
                                        iExploitID = model.EXPLOIT.Where(o => o.ExploitReferential == "securityfocus" && o.ExploitLocation == sReferenceURL).Select(o => o.ExploitID).FirstOrDefault();
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                    /*
                                    int iExploitID = 0;
                                    var syn = from S in model.EXPLOIT
                                              where S.ExploitReferential.Equals("securityfocus")
                                              && S.ExploitLocation.Equals(sReferenceURL)
                                              select S.ExploitID;
                                    if (syn.Count() == 0)
                                    */
                                    if (iExploitID<=0)
                                    {
                                        //ExploitID = syn.ToList().First().ExploitID;
                                        EXPLOIT sploit = new EXPLOIT();
                                        sploit.ExploitReferential = "securityfocus";
                                        string tempRefID = sReferenceURL.Replace("http://securityfocus.com/bid/", "");
                                        sploit.ExploitRefID = tempRefID.Replace("/exploit", "");
                                        sploit.ExploitLocation = sReferenceURL;
                                        sploit.CreatedDate = DateTimeOffset.Now;
                                        sploit.timestamp = DateTimeOffset.Now;
                                        sploit.VocabularyID = iVocabularyCVENVD;
                                        model.EXPLOIT.Add(sploit);
                                        model.SaveChanges();
                                        iExploitID = sploit.ExploitID;

                                        //TODO
                                        //EXPLOITFORCPE
                                        /*
                                        foreach(XmlNode in nodelistVulnerableSoftwares)
                                        {

                                        }
                                        */



                                        #region oldremove
                                        /*
                                        try
                                        {
                                            EXPLOITFORVULNERABILITY sploitvuln = new EXPLOITFORVULNERABILITY();
                                            sploitvuln.VulnerabilityID = oCVE.VulnerabilityID;
                                            sploitvuln.ExploitID = iExploitID;   //sploit.ExploitID;
                                            sploitvuln.VocabularyID = iVocabularyCVENVD;
                                            sploitvuln.CreatedDate = DateTimeOffset.Now;
                                            sploitvuln.timestamp = DateTimeOffset.Now;
                                            model.EXPLOITFORVULNERABILITY.Add(sploitvuln);
                                            model.SaveChanges();
                                        }
                                        catch (Exception exAddToEXPLOITFORVULNERABILITY)
                                        {
                                            Console.WriteLine("Exception: exAddToEXPLOITFORVULNERABILITY " + exAddToEXPLOITFORVULNERABILITY.Message + " " + exAddToEXPLOITFORVULNERABILITY.InnerException);
                                        }



                                        try
                                        {
                                            EXPLOITFORREFERENCE sploitreference = new EXPLOITFORREFERENCE();
                                            sploitreference.ExploitID = iExploitID;
                                            sploitreference.ReferenceID = iReferenceID; // oReference.ReferenceID;
                                            sploitreference.VocabularyID = iVocabularyCVENVD;
                                            sploitreference.CreatedDate = DateTimeOffset.Now;
                                            sploitreference.timestamp = DateTimeOffset.Now;
                                            model.EXPLOITFORREFERENCE.Add(sploitreference);
                                            model.SaveChanges();
                                        }
                                        catch (Exception exAddToEXPLOITFORREFERENCE)
                                        {
                                            Console.WriteLine("Exception: exAddToEXPLOITFORREFERENCE " + exAddToEXPLOITFORREFERENCE.Message + " " + exAddToEXPLOITFORREFERENCE.InnerException);
                                        }
                                        */
                                        #endregion oldremove
                                    }
                                    else
                                    {
                                        //Update EXPLOIT
                                    }

                                    //EXPLOITFORCPE
                                    #region EXPLOITFORCPE
                                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                    Console.WriteLine("DEBUG EXPLOITFORCPE");
                                    foreach (XmlNode nodeCPE in nodelistVulnerableSoftwares)
                                    {
                                        string sCPEID = nodeCPE.InnerText;
                                        int iCPEID = model.CPE.Where(o => o.CPEName == sCPEID).Select(o => o.CPEID).FirstOrDefault();
                                        if (iCPEID <= 0)
                                        {
                                            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                            Console.WriteLine("ERROR: the CPE does not exist in the CPE table");
                                            //TODO
                                        }

                                        //Optimization because no update
                                        //EXPLOITFORCPE oCPEExploit = model.EXPLOITFORCPE.FirstOrDefault(o => o.CPEID == sCPEID && o.ExploitID == iExploitID);
                                        int iCPEExploitID = 0;
                                        try
                                        {
                                            iCPEExploitID = model.EXPLOITFORCPE.Where(o => o.CPEID == iCPEID && o.ExploitID == iExploitID).Select(o => o.CPEExploitID).FirstOrDefault();
                                        }
                                        catch (Exception ex)
                                        {

                                        }
                                        //if (oCPEExploit == null)
                                        if (iCPEExploitID <= 0)
                                        {
                                            EXPLOITFORCPE oCPEExploit = new EXPLOITFORCPE();
                                            oCPEExploit.CPEID = iCPEID;
                                            //Console.WriteLine("DEBUG sEDBID=" + sEDBID);
                                            Console.WriteLine("DEBUG CPEName=" + sCPEID);
                                            oCPEExploit.CPEName = sCPEID;
                                            oCPEExploit.ExploitID = iExploitID;
                                            oCPEExploit.CreatedDate = DateTimeOffset.Now;
                                            oCPEExploit.timestamp = DateTimeOffset.Now;
                                            oCPEExploit.VocabularyID = iVocabularyCVENVD;
                                            model.EXPLOITFORCPE.Add(oCPEExploit);
                                            model.SaveChanges();
                                            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                            Console.WriteLine("DEBUG Added EXPLOITFORCPE");
                                        }
                                        else
                                        {
                                            //Update EXPLOITFORCPE

                                        }
                                    }
                                    #endregion EXPLOITFORCPE

                                    #region EXPLOITFORVULNERABILITY
                                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                    Console.WriteLine("DEBUG EXPLOITFORVULNERABILITY");
                                    //Optimization because no update
                                    //EXPLOITFORVULNERABILITY oVulnerabilityExploit = model.EXPLOITFORVULNERABILITY.FirstOrDefault(o => o.VulnerabilityID == oCVE.VulnerabilityID && o.ExploitID == iExploitID);
                                    int iVulnerabilityExploitID = 0;
                                    try
                                    {
                                        iVulnerabilityExploitID = model.EXPLOITFORVULNERABILITY.Where(o => o.VulnerabilityID == iVulnerabilityID && o.ExploitID == iExploitID).Select(o => o.VulnerabilityExploitID).FirstOrDefault();
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                    //if (oVulnerabilityExploit == null)
                                    if (iVulnerabilityExploitID <= 0)
                                    {

                                        try
                                        {
                                            EXPLOITFORVULNERABILITY oVulnerabilityExploit = new EXPLOITFORVULNERABILITY();
                                            oVulnerabilityExploit.VulnerabilityID =iVulnerabilityID;
                                            oVulnerabilityExploit.ExploitID = iExploitID;   //sploit.ExploitID;
                                            oVulnerabilityExploit.CreatedDate = DateTimeOffset.Now;
                                            oVulnerabilityExploit.timestamp = DateTimeOffset.Now;
                                            oVulnerabilityExploit.VocabularyID = iVocabularyCVENVD;
                                            model.EXPLOITFORVULNERABILITY.Add(oVulnerabilityExploit);
                                            model.SaveChanges();
                                            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                            Console.WriteLine("DEBUG Added EXPLOITFORVULNERABILITY");
                                        }
                                        catch (Exception exAddToEXPLOITFORVULNERABILITY1)
                                        {
                                            Console.WriteLine("Exception: exAddToEXPLOITFORVULNERABILITY1 " + exAddToEXPLOITFORVULNERABILITY1.Message + " " + exAddToEXPLOITFORVULNERABILITY1.InnerException);
                                        }
                                    }
                                    #endregion EXPLOITFORVULNERABILITY

                                    #region EXPLOITFORREFERENCE
                                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                    Console.WriteLine("DEBUG EXPLOITFORREFERENCE");
                                    //Optimization because no update
                                    //EXPLOITFORREFERENCE oExploitReference = model.EXPLOITFORREFERENCE.FirstOrDefault(o => o.ExploitID == iExploitID && o.ReferenceID == oReference.ReferenceID);
                                    int iExploitReferenceID = 0;
                                    try
                                    {
                                        iExploitReferenceID = model.EXPLOITFORREFERENCE.Where(o => o.ExploitID == iExploitID && o.ReferenceID == iReferenceID).Select(o => o.ExploitReferenceID).FirstOrDefault();
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                    //if (oExploitReference == null)
                                    if (iExploitReferenceID <= 0)
                                    {
                                        EXPLOITFORREFERENCE oExploitReference = new EXPLOITFORREFERENCE();
                                        oExploitReference.ExploitID = iExploitID;
                                        oExploitReference.ReferenceID = iReferenceID;   // oReference.ReferenceID;
                                        oExploitReference.CreatedDate = DateTimeOffset.Now;
                                        oExploitReference.timestamp = DateTimeOffset.Now;
                                        oExploitReference.VocabularyID = iVocabularyCVENVD;
                                        model.EXPLOITFORREFERENCE.Add(oExploitReference);
                                        model.SaveChanges();
                                        //iExploitReferenceID=
                                        Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                        Console.WriteLine("DEBUG Added EXPLOITFORREFERENCE");
                                    }
                                    else
                                    {
                                        //Update EXPLOITFORREFERENCE
                                    }
                                    #endregion EXPLOITFORREFERENCE
                                }
                                #endregion exploitbid
                                else
                                {
                                    //TODO: other exploits referentials?
                                    //http://1337day.com/exploits/20112

                                }
                            }
                        }
                        


                        //oReference = null;
                    }
                }
                catch (Exception exCVEReferences)
                {
                    Console.WriteLine("Exception: exCVEReferences " + exCVEReferences.Message + " " + exCVEReferences.InnerException);
                }
                #endregion CVEReferences

                /*
                try
                {
                    model.SaveChanges();
                }
                catch (Exception exCVEReferencesSave)
                {
                    Console.WriteLine("Exception: exCVEReferencesSave " + exCVEReferencesSave.Message + " " + exCVEReferencesSave.InnerException);
                }
                */

                // ========
                // Progress
                // ========
                /*
                count++;

                int dummy;
                dummy = (count * 100) / nodes.Count;

                if (progress != dummy)
                    Trace.WriteLine(string.Format("{0} %", dummy));

                progress = dummy;
                */

                if (shouldsearchexploit)
                {
                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                    Console.WriteLine("DEBUG Search exploits for " + sCVEID);
                    //                SearchExploits(cveid, monStreamWriter);
                    SearchExploitsForCVE(sCVEID, monStreamWriter);  //jerome2.log
                    //Here can't read jerome2.log because locked
                    monStreamWriter.Close();
                    Console.WriteLine("DEBUG jerome2.log closed");

                    ParseEXPLOITDB("jerome2.log", false);  //bAppend=false to overwite the file
                    InsertEXPLOITDB();
                }
                else
                {
                    monStreamWriter.Close();
                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                    Console.WriteLine("DEBUG jerome2.log has been closed");
                }

                //Search securityfocus
                //SearchBIDForCVE(sCVEID, monStreamWriter);

            }

            //FREE
            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
            Console.WriteLine("DEBUG FREE MEMORY");
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
                Console.WriteLine("Exception: DbEntityValidationExceptionFINALSAVECVE " + sb.ToString());
            }
            catch (Exception exFINALSAVE)
            {
                Console.WriteLine("Exception: exFINALSAVECVE " + exFINALSAVE.Message + " " + exFINALSAVE.InnerException);
            }
            model.Dispose();
            model = null;
            /*
            try
            {
                monStreamWriter.Close();
                Console.WriteLine("DEBUG jerome2.log closed");
            }
            catch (Exception exmonStreamWriterClose)
            {
                Console.WriteLine("Exception: exmonStreamWriterClose jerome2.log " + exmonStreamWriterClose.Message + " " + exmonStreamWriterClose.InnerException);
            }
            */
        }

        static private double Helper_Round(double x)
        {
            double r = x * 10.0 - Math.Truncate(x * 10.0);

            if (r > 0.5)
                return (Math.Truncate(x * 10.0) + 1.0) / 10.0;
            else
                return Math.Truncate(x * 10.0) / 10.0;
        }

        static private string CreateMD5Hash(string input)
        {
            // Use input string to calculate MD5 hash
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
                // To force the hex string to lower-case letters instead of
                // upper-case, use he following line instead:
                // sb.Append(hashBytes[i].ToString("x2")); 
            }
            return sb.ToString();
        }

        public static void fctParseVulnerableConfigurationTest(XmlNode nodeVulnConfig, int iVulnConfigID, int iLogicalTestLevel = 1, int iLogicalTestOrder = 0)
        {
            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
            Console.WriteLine("DEBUG in fctParseVulnerableConfigurationTest()");
            int iLogicalTestLevelLoop = iLogicalTestLevel;
            foreach (XmlNode nodeCPEtest in nodeVulnConfig.ChildNodes)
            {
                
                //int iLogicalTestOrder = 0;
                //foreach (XmlNode nodeCPE in nodeCPEtest.ChildNodes)
                //{
                    XmlNode nodeCPE=nodeCPEtest;
                    //Console.WriteLine("DEBUG nodeCPE.Name=" + nodeCPE.Name);
                    switch (nodeCPE.Name)
                    {
                        case "cpe-lang:fact-ref":
                            //<cpe-lang:fact-ref name="cpe:/o:google:android:2.1"/>
                            try
                            {
                                string sCPEName = nodeCPE.Attributes["name"].InnerText;
                                //Search if the CPE already exists in the database
                                //Console.WriteLine("DEBUG Search sCPEName=" + sCPEName);
                                #region cpe
                                int iCPEID = 0;
                                try
                                {
                                    iCPEID = model.CPE.Where(o => o.CPEName == sCPEName).Select(o => o.CPEID).FirstOrDefault();
                                }
                                catch (Exception ex)
                                {
                                    iCPEID = 0;
                                }
                                if (iCPEID <= 0)
                                {
                                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                    Console.WriteLine("DEBUG Adding new CPE " + sCPEName);
                                    CPE oCPE = new CPE();
                                    oCPE.CreatedDate = DateTimeOffset.Now;
                                    oCPE.CPEName = sCPEName;
                                    oCPE.VocabularyID = iVocabularyCVENVD;
                                    oCPE.timestamp = DateTimeOffset.Now;
                                    model.CPE.Add(oCPE);
                                    model.SaveChanges();
                                    iCPEID = oCPE.CPEID;
                                }
                                else
                                {
                                    //Update CPE
                                }
                                #endregion cpe

                                //TODO: Can we use iCPEID here instead of vuln:vulnerable-software-list vuln:product???
                                //VULNERABILITYFORCPE

                                #region VULNERABLECONFIGURATIONCPE
                                int iVulnConfigCPEID = 0;
                                try
                                {
                                    //TODO? && ==LogicalTestLevelOrder
                                    //iVulnConfigCPEID = model.VULNERABLECONFIGURATIONCPE.Where(o => o.VulnerableConfigurationID == iVulnConfigID && o.CPEID == iCPEID && o.CPELogicalTestID == iCPETESTID).Select(o => o.VulnerableConfigurationCPEID).FirstOrDefault();
                                    iVulnConfigCPEID = model.VULNERABLECONFIGURATIONCPE.Where(o => o.VulnerableConfigurationID == iVulnConfigID && o.CPEID == iCPEID).Select(o => o.VulnerableConfigurationCPEID).FirstOrDefault();
                                    
                                }
                                catch (Exception ex)
                                {
                                    iVulnConfigCPEID = 0;
                                }
                                if (iVulnConfigCPEID <= 0)
                                {
                                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                    Console.WriteLine("DEBUG Adding VULNERABLECONFIGURATIONCPE");
                                    Console.WriteLine("DEBUG sCPEName=" + sCPEName);
                                    Console.WriteLine("DEBUG iLogicalTestLevel=" + iLogicalTestLevel);
                                    Console.WriteLine("DEBUG iLogicalTestOrder=" + iLogicalTestOrder);
                                    VULNERABLECONFIGURATIONCPE oVulnConfigCPE = new VULNERABLECONFIGURATIONCPE();
                                    oVulnConfigCPE.CreatedDate = DateTimeOffset.Now;
                                    oVulnConfigCPE.VulnerableConfigurationID = iVulnConfigID;
                                    oVulnConfigCPE.LogicalTestLevel = iLogicalTestLevel-1;
                                    //oVulnConfigCPE.LogicalTestLevelOrder = iLogicalTestOrder;
                                    //oVulnConfigCPE.CPELogicalTestID = iCPETESTID;
                                    oVulnConfigCPE.CPEID = iCPEID;
                                    oVulnConfigCPE.VocabularyID = iVocabularyCVENVD;    //CPE?
                                    oVulnConfigCPE.timestamp = DateTimeOffset.Now;
                                    model.VULNERABLECONFIGURATIONCPE.Attach(oVulnConfigCPE);
                                    model.Entry(oVulnConfigCPE).State = EntityState.Modified;
                                    model.VULNERABLECONFIGURATIONCPE.Add(oVulnConfigCPE);
                                    model.SaveChanges();    //TEST PERFORMANCE
                                    //iVulnConfigCPEID=
                                }
                                else
                                {
                                    //Update VULNERABLECONFIGURATIONCPE
                                    
                                    //Console.WriteLine("DEBUG iVulnConfigCPEID=" + iVulnConfigCPEID);
                                }
                                #endregion VULNERABLECONFIGURATIONCPE
                                
                            }
                            catch (Exception exCPEFactRefName)
                            {
                                Console.WriteLine("Exception: exCPEFactRefName " + exCPEFactRefName.Message + " " + exCPEFactRefName.InnerException);
                            }
                            break;

                        case "cpe-lang:logical-test":
                            #region CPELOGICALTEST
                            //<cpe-lang:logical-test negate="false" operator="OR">
                            string sNegate = "false";
                            try
                            {
                                sNegate = nodeCPEtest.Attributes["negate"].InnerText;
                            }
                            catch (Exception ex)
                            {
                                sNegate = "false";
                            }
                            bool bNegate = false;
                            if (sNegate == "true")
                            {
                                bNegate = true;
                            }

                            string sOperatorValue = "OR";
                            try
                            {
                                sOperatorValue = nodeCPEtest.Attributes["operator"].InnerText;
                            }
                            catch (Exception ex)
                            {
                                sOperatorValue = "OR";
                            }
                            Console.WriteLine("DEBUG sOperatorValue=" + sOperatorValue);
                            #region operatorenumeration
                            int iOperatorEnumerationID = 0;
                            try
                            {
                                iOperatorEnumerationID = model.OPERATORENUMERATION.Where(o => o.OperatorValue == sOperatorValue).Select(o => o.OperatorEnumerationID).FirstOrDefault();
                            }
                            catch (Exception ex)
                            {

                            }
                            if (iOperatorEnumerationID <= 0)
                            {
                                XOVALModel.OPERATORENUMERATION oOperatorEnumeration = new XOVALModel.OPERATORENUMERATION();
                                oOperatorEnumeration.CreatedDate = DateTimeOffset.Now;
                                oOperatorEnumeration.OperatorValue = sOperatorValue;
                                oOperatorEnumeration.VocabularyID = iVocabularyCVENVD;    //CPE?
                                oOperatorEnumeration.timestamp = DateTimeOffset.Now;
                                oval_model.OPERATORENUMERATION.Add(oOperatorEnumeration);
                                oval_model.SaveChanges();
                                iOperatorEnumerationID = oOperatorEnumeration.OperatorEnumerationID;
                            }
                            else
                            {
                                //Update OPERATORENUMERATION
                            }
                            #endregion operatorenumeration

                            //TODO? test if other Attributes

                            int iCPETESTID = 0;
                            try
                            {
                                iCPETESTID = model.CPELOGICALTEST.Where(o => o.negate == bNegate && o.OperatorEnumerationID == iOperatorEnumerationID).Select(o => o.CPELogicalTestID).FirstOrDefault();
                            }
                            catch (Exception ex)
                            {

                            }
                            if (iCPETESTID <= 0)
                            {
                                CPELOGICALTEST oCPELogicalTest = new CPELOGICALTEST();
                                oCPELogicalTest.CreatedDate = DateTimeOffset.Now;
                                oCPELogicalTest.negate = bNegate;
                                oCPELogicalTest.OperatorEnumerationID = iOperatorEnumerationID;
                                oCPELogicalTest.VocabularyID = iVocabularyCVENVD;   //CPE?
                                oCPELogicalTest.timestamp = DateTimeOffset.Now;
                                model.CPELOGICALTEST.Add(oCPELogicalTest);
                                model.SaveChanges();
                                iCPETESTID = oCPELogicalTest.CPELogicalTestID;
                            }
                            else
                            {
                                //Update CPELOGICALTEST
                            }
                            #endregion CPELOGICALTEST
                            iLogicalTestOrder++;    //TODO REVIEW (needed? vs TestLevel)
                            Console.WriteLine("DEBUG iLogicalTestOrder=" + iLogicalTestOrder);
                            Console.WriteLine("DEBUG nodeCPEtest.ChildNodes.Count=" + nodeCPEtest.ChildNodes.Count);

                            
                            #region logicaltest
                            int iVulnConfigLevel1ID = 0;
                            try
                            {
                                iVulnConfigLevel1ID = model.VULNERABLECONFIGURATIONCPE.Where(o => o.VulnerableConfigurationID == iVulnConfigID && o.LogicalTestLevel == iLogicalTestLevelLoop && o.CPELogicalTestID == iCPETESTID).Select(o => o.VulnerableConfigurationCPEID).FirstOrDefault();
                            }
                            catch (Exception ex)
                            {

                            }
                            if (iVulnConfigLevel1ID <= 0)
                            {
                                Console.WriteLine("DEBUG Adding VULNERABLECONFIGURATIONCPELEVEL "+iLogicalTestLevel);
                                VULNERABLECONFIGURATIONCPE oVulnConfigCPE = new VULNERABLECONFIGURATIONCPE();
                                oVulnConfigCPE.CreatedDate = DateTimeOffset.Now;
                                oVulnConfigCPE.VulnerableConfigurationID = iVulnConfigID;
                                oVulnConfigCPE.LogicalTestLevel = iLogicalTestLevelLoop;    // iLogicalTestLevel;
                                oVulnConfigCPE.LogicalTestLevelOrder = iLogicalTestOrder;
                                oVulnConfigCPE.CPELogicalTestID = iCPETESTID;
                                //oVulnConfigCPE.CPEID = iCPEID;  //NO CPE for Level=1
                                oVulnConfigCPE.VocabularyID = iVocabularyCVENVD;    //CPE?
                                oVulnConfigCPE.timestamp = DateTimeOffset.Now;
                                model.VULNERABLECONFIGURATIONCPE.Add(oVulnConfigCPE);
                                model.SaveChanges();    //TEST PERFORMANCE
                                iVulnConfigLevel1ID = oVulnConfigCPE.VulnerableConfigurationCPEID;
                            }
                            else
                            {
                                //Update VULNERABLECONFIGURATIONCPE
                            }
                            #endregion logicaltest
                            

                            //TODO REVIEW
                            iLogicalTestLevel++;    //2
                            //iLogicalTestOrder++;
                            //TODO REVIEW
                            fctParseVulnerableConfigurationTest(nodeCPE, iVulnConfigID, iLogicalTestLevel); //, iLogicalTestOrder);


                            break;

                        default:
                            Console.WriteLine("ERROR: Missing code for nodeCPE.Name=" + nodeCPE.Name);
                            break;
                    }

                //}
            }
        }

        public static void DecompressGZip(FileInfo fileToDecompress)
        {
            using (FileStream originalFileStream = fileToDecompress.OpenRead())
            {
                string currentFileName = fileToDecompress.FullName;
                string newFileName = currentFileName.Remove(currentFileName.Length - fileToDecompress.Extension.Length);

                using (FileStream decompressedFileStream = File.Create(newFileName))
                {
                    using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(decompressedFileStream);
                        Console.WriteLine("Decompressed: {0}", fileToDecompress.Name);
                    }
                }
            }
        }

        static private void ListCVE()
        {
            XVULNERABILITYModel.XVULNERABILITYEntities vuln_model=null;
            try
            {
                vuln_model = new XVULNERABILITYModel.XVULNERABILITYEntities();
                vuln_model.Configuration.AutoDetectChangesEnabled = false;
                vuln_model.Configuration.ValidateOnSaveEnabled = false;
            }
            catch (Exception exListCVEModel)
            {
                Console.WriteLine("Exception: exListCVEModel " + exListCVEModel.Message + " " + exListCVEModel.InnerException);
            }
            //model = new XVULNERABILITYModel.XVULNERABILITYEntities();

            StreamWriter monStreamWriter = new StreamWriter("cves.txt");

            try
            {
                var syn = from S in vuln_model.VULNERABILITY
                          where S.VULReferential.Equals("cve")
                          orderby S.VULReferentialID descending
                          select S.VULReferentialID;

                //syn.ToList().First().ID;
                foreach (var mycve in syn)
                {
                    //Console.WriteLine(mycve.Value);
                    string myCVE2 = string.Empty;
                    //myCVE2 = mycve.VULReferentialID.Replace("CVE-", "");
                    myCVE2 = mycve.Replace("CVE-", "");
                    //Console.WriteLine(myCVE2);
                    //SearchExploits(myCVE2);
                    monStreamWriter.WriteLine(myCVE2);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("ListCVE Exception = {0}", ex));
            }
            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
            Console.WriteLine("DEBUG cves.txt closed");
            monStreamWriter.Close();
            Console.WriteLine("DEBUG FINISHED ListCVE");

            //FREE
            vuln_model.Dispose();
            vuln_model = null;
        }

        static private CVSSCalculator.AccessVector Helper_ParseAccessVector(string s)
        {
            switch (s)
            {
                case "LOCAL":
                    return CVSSCalculator.AccessVector.Local;
                case "ADJACENT_NETWORK":
                    return CVSSCalculator.AccessVector.LocalNetwork;
                case "NETWORK":
                    return CVSSCalculator.AccessVector.Network;
                default:
                    Console.WriteLine("ERROR: Missing code for Helper_ParseAccessVector " + s);
                    break;
            }

            throw new Exception();
        }

        static private CVSSCalculator.AccessComplexity Helper_ParseAccessComplexity(string s)
        {
            switch (s)
            {
                case "HIGH":
                    return CVSSCalculator.AccessComplexity.High;
                case "MEDIUM":
                    return CVSSCalculator.AccessComplexity.Medium;
                case "LOW":
                    return CVSSCalculator.AccessComplexity.Low;
                default:
                    Console.WriteLine("ERROR: Missing code for Helper_ParseAccessComplexity " + s);
                    break;
            }

            throw new Exception();
        }

        static private CVSSCalculator.Authentication Helper_ParseAuthentication(string s)
        {
            switch (s)
            {
                case "MULTIPLE_INSTANCES":
                    return CVSSCalculator.Authentication.MultipleIntances;
                case "SINGLE_INSTANCE":
                    return CVSSCalculator.Authentication.SingleInstance;
                case "NONE":
                    return CVSSCalculator.Authentication.None;
                default:
                    Console.WriteLine("ERROR: Missing code for Helper_ParseAuthentication " + s);
                    break;
            }

            throw new Exception();
        }

        static private CVSSCalculator.ConfidentialityImpact Helper_ParseConfidentialiyImpact(string s)
        {
            switch (s)
            {
                case "NONE":
                    return CVSSCalculator.ConfidentialityImpact.None;
                case "PARTIAL":
                    return CVSSCalculator.ConfidentialityImpact.Partial;
                case "COMPLETE":
                    return CVSSCalculator.ConfidentialityImpact.Complete;
                default:
                    Console.WriteLine("ERROR: Missing code for Helper_ParseConfidentialiyImpact " + s);
                    break;
            }

            throw new Exception();
        }

        static private CVSSCalculator.IntegrityImpact Helper_ParseIntegrityImpact(string s)
        {
            switch (s)
            {
                case "NONE":
                    return CVSSCalculator.IntegrityImpact.None;
                case "PARTIAL":
                    return CVSSCalculator.IntegrityImpact.Partial;
                case "COMPLETE":
                    return CVSSCalculator.IntegrityImpact.Complete;
                default:
                    Console.WriteLine("ERROR: Missing code for Helper_ParseIntegrityImpact " + s);
                    break;
            }

            throw new Exception();
        }

        static private CVSSCalculator.AvailabilityImpact Helper_ParseAvailabilityImpact(string s)
        {
            switch (s)
            {
                case "NONE":
                    return CVSSCalculator.AvailabilityImpact.None;
                case "PARTIAL":
                    return CVSSCalculator.AvailabilityImpact.Partial;
                case "COMPLETE":
                    return CVSSCalculator.AvailabilityImpact.Complete;
                default:
                    Console.WriteLine("ERROR: Missing code for Helper_ParseAvailabilityImpact " + s);
                    break;
            }

            throw new Exception();
        }

        //*******************************************************************
        static private void Import_cwes(string filepath)
        {
            Console.WriteLine("DEBUG "+DateTimeOffset.Now);
            XmlDocument docXML= new XmlDocument();
            //TODO: Security controls/checks
            //TODO: XSD validation
            //TODO: ...
            docXML.Load(filepath);
            Console.WriteLine("DEBUG "+DateTimeOffset.Now);
            Console.WriteLine("DEBUG CWE file loaded");
            

            XmlNodeList nodesCategory;
            nodesCategory = docXML.SelectNodes("/Weakness_Catalog/Categories/Category");
            ImportFile_cwe(nodesCategory);

            XmlNodeList nodesWeakness;
            nodesWeakness = docXML.SelectNodes("/Weakness_Catalog/Weaknesses/Weakness");
            ImportFile_cwe(nodesWeakness);

            XmlNodeList nodesCompound;
            nodesCompound = docXML.SelectNodes("/Weakness_Catalog/Compound_Elements/Compound_Element");
            ImportFile_cwe(nodesCompound);

            //TODO: Free...
            docXML = null;
        }

        static private void ImportFile_cwe(XmlNodeList nodes)
        {
            Console.WriteLine("DEBUG "+DateTimeOffset.Now);
            Console.WriteLine("DEBUG ImportFile_cwe");
            int iCWEVocabularyID = iVocabularyCWEID;// 20;  //TODO: Hardcoded
            XORCISMEntities model=new XORCISMEntities();
            model.Configuration.AutoDetectChangesEnabled = false;
            model.Configuration.ValidateOnSaveEnabled = false;

            //Make sure that we have at least one valid MitigationID in the database
            int iDefaultMitigationID = 0;
            try
            {
                iDefaultMitigationID = model.MITIGATION.Select(o => o.MitigationID).FirstOrDefault();
            }
            catch(Exception ex)
            {

            }
            if (iDefaultMitigationID <= 0)
            {
                Console.WriteLine("DEBUG Adding first MITIGATION");
                MITIGATION oMitigation = new MITIGATION();
                oMitigation.CreatedDate = DateTimeOffset.Now;
                oMitigation.SolutionMitigationText = "";
                oMitigation.VocabularyID = iCWEVocabularyID;
                oMitigation.timestamp = DateTimeOffset.Now;
                model.MITIGATION.Add(oMitigation);
                model.SaveChanges();
                iDefaultMitigationID = oMitigation.MitigationID;
            }
            else
            {
                //Update MITIGATION
            }

            foreach (XmlNode nodeCWE in nodes)
            {
                /*
                if (nodeCWE.Attributes["ID"].InnerText == "119")    //TODO: why this?
                {
                    int z = 0;
                }
                */
                string sCWEID = "CWE-" + nodeCWE.Attributes["ID"].InnerText;
                Console.WriteLine("DEBUG =================================================================");
                Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                Console.WriteLine("DEBUG " + sCWEID);
                string sCWEName = nodeCWE.Attributes["Name"].InnerText;
                string sCWEStatus = nodeCWE.Attributes["Status"].InnerText;
                string sCWEAbstraction = string.Empty;
                //TODO: CWEURL
                try
                {
                    sCWEAbstraction = nodeCWE.Attributes["Weakness_Abstraction"].InnerText;
                }
                catch (Exception ex)
                {
                    string sIgnoreWarning = ex.Message;
                    //Console.WriteLine("Exception: WeaknessAbstraction: " + ex.Message + " " + ex.InnerException);
                    //Object reference not set to an instance of an object. 
                }
                string sCWEDescription = nodeCWE.ChildNodes[0].ChildNodes[0].InnerText;
                string sCWEDescriptionClean = CleaningCWEString(sCWEDescription);

                string sCWECausalNature = string.Empty;  //TODO
                string sCWEURL = string.Empty;  //TODO
                Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                Console.WriteLine("DEBUG CWE Request");
                CWE CWEObject = model.CWE.FirstOrDefault(o => o.CWEID == sCWEID);
                Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                Console.WriteLine("DEBUG CWE Request Completed");
                if (CWEObject == null)
                {
                    //Adding the CWE
                    CWE oCWE;
                    oCWE = new CWE();
                    oCWE.CWEID = sCWEID;
                    oCWE.CreatedDate = DateTimeOffset.Now;
                    oCWE.CWEName = sCWEName;
                    oCWE.CWEStatus = sCWEStatus;
                    oCWE.CWEAbstraction = sCWEAbstraction;
                    oCWE.CWEDescriptionSummary = sCWEDescriptionClean;
                    //oCWE.CWEDescriptionSummaryClean = sCWEDescriptionClean;
                    //TODO complete
                        oCWE.CWECausalNature = sCWECausalNature;
                        oCWE.VocabularyID = iCWEVocabularyID;
                    oCWE.timestamp = DateTimeOffset.Now;
                    //oCWE.CWEURL =   //TODO
                    try
                    {
                        model.CWE.Attach(oCWE);
                        model.Entry(oCWE).State = EntityState.Modified; //EntityState.Added;
                        model.CWE.Add(oCWE);
                        model.SaveChanges();
                        Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                        Console.WriteLine("DEBUG AddToCWE " + sCWEID);
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
                        Console.WriteLine("Exception: DbEntityValidationExceptionAddToCWE" + sb.ToString());
                    }
                    catch (Exception exAddToCWE)
                    {
                        Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                        Console.WriteLine("Exception: exAddToCWE " + exAddToCWE.InnerException + " " + exAddToCWE.Message);
                    }
                    CWEObject = oCWE;
                }
                else
                {
                    //Update CWE
                    Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                    Console.WriteLine("DEBUG Updating " + sCWEID);
                    CWEObject.CWEName = sCWEName;
                    CWEObject.CWEStatus = sCWEStatus;
                    CWEObject.CWEAbstraction = sCWEAbstraction;
                    CWEObject.CWEDescriptionSummary = sCWEDescriptionClean;
                    //CWEObject.CWEDescriptionSummaryClean = sCWEDescriptionClean;
                    //TODO: complete
                        CWEObject.CWECausalNature = sCWECausalNature;
                        CWEObject.VocabularyID = iCWEVocabularyID;
                    CWEObject.timestamp = DateTimeOffset.Now;
                    //CWEObject.CWEURL =    //TODO
                }
                try
                {
                    model.CWE.Attach(CWEObject);
                    model.Entry(CWEObject).State = EntityState.Modified;
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
                    Console.WriteLine("Exception: DbEntityValidationExceptionexCWE " + sb.ToString());
                }
                catch (Exception exCWE)
                {
                    Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                    Console.WriteLine("Exception: exCWE " + exCWE.InnerException + " " + exCWE.Message);
                }
                foreach (XmlNode nodeCWEinfo in nodeCWE)
                {
                    Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                    Console.WriteLine("DEBUG: nodeCWEinfo: " + nodeCWEinfo.Name);
                    
                    switch (nodeCWEinfo.Name)
                    {
                        case "Description":
                            //Done before
                            break;
                        case "Relationships":
                        #region CWERelationships
                            try
                            {
                                //TODO? Delete and recreate
                                foreach (XmlNode nodeCWERelationship in nodeCWEinfo)
                                {
                                    bool bCWERelationshipCategory = false;
                                    string sCWERelationshipNature = "";
                                    string sCWERelationshipTargetCWEID = "";
                                    foreach (XmlNode nodeCWERelationshipItem in nodeCWERelationship)
                                    {
                                        if (nodeCWERelationshipItem.Name == "Relationship_Target_Form")
                                        {
                                            if (nodeCWERelationshipItem.InnerText == "Category")
                                            {
                                                bCWERelationshipCategory = true;
                                            }
                                        }
                                        else
                                        {
                                            if (nodeCWERelationshipItem.Name == "Relationship_Nature")
                                            {
                                                sCWERelationshipNature = nodeCWERelationshipItem.InnerText; //ChildOf
                                            }
                                            else
                                            {
                                                if (nodeCWERelationshipItem.Name == "Relationship_Target_ID")
                                                {
                                                    
                                                    sCWERelationshipTargetCWEID = "CWE-" + nodeCWERelationshipItem.InnerText; //519
                                                    //Check if TargetCWEID exists
                                                    
                                                    string sCWETargetID = "";
                                                    try
                                                    {
                                                        sCWETargetID = model.CWE.Where(o => o.CWEID == sCWERelationshipTargetCWEID).Select(o => o.CWEID).FirstOrDefault();
                                                    }
                                                    catch(Exception ex)
                                                    {
                                                        sCWETargetID = "";
                                                    }
                                                    if(sCWETargetID=="" || sCWETargetID == null)
                                                    {
                                                        CWE OCWETarget = new CWE();
                                                        OCWETarget.CWEID = sCWERelationshipTargetCWEID;
                                                        OCWETarget.CreatedDate = DateTimeOffset.Now;
                                                        OCWETarget.VocabularyID = iVocabularyCWEID;
                                                        OCWETarget.timestamp = DateTimeOffset.Now;
                                                        model.CWE.Add(OCWETarget);
                                                        model.SaveChanges();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (bCWERelationshipCategory)
                                    {
                                        //CWERELATIONSHIPCATEGORY CWERelationCategory = model.CWERELATIONSHIPCATEGORY.Where(o => o.CWEID == sCWEID && o.RelationshipTargetCWEID == sCWERelationshipTargetCWEID).FirstOrDefault();
                                        int iCWERelationshipCategoryID=0;
                                        try
                                        {
                                            iCWERelationshipCategoryID = model.CWERELATIONSHIPCATEGORY.Where(o => o.CWEID == sCWEID && o.RelationshipTargetCWEID == sCWERelationshipTargetCWEID).Select(o=>o.CWERelationshipCategoryID).FirstOrDefault();
                                        }
                                        catch(Exception ex)
                                        {

                                        }
                                        //if (CWERelationCategory != null)
                                        if(iCWERelationshipCategoryID>0)
                                        {
                                            //Update CWERELATIONSHIPCATEGORY
                                            //TODO: Test if same RelationshipNature
                                            //if not: delete and recreate
                                        }
                                        else
                                        {
                                            CWERELATIONSHIPCATEGORY CWERelationCategory = new CWERELATIONSHIPCATEGORY();
                                            CWERelationCategory.CreatedDate=DateTimeOffset.Now;
                                            CWERelationCategory.CWEID = sCWEID;
                                            CWERelationCategory.RelationshipNature = sCWERelationshipNature;
                                            CWERelationCategory.RelationshipTargetCWEID = sCWERelationshipTargetCWEID;
                                            CWERelationCategory.VocabularyID=iVocabularyCWEID;
                                            CWERelationCategory.timestamp=DateTimeOffset.Now;
                                            model.CWERELATIONSHIPCATEGORY.Add(CWERelationCategory);
                                            //model.SaveChanges();    //TEST PERFORMANCE
                                            //iCWERelationshipCategoryID=
                                            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                            Console.WriteLine("DEBUG AddToCWERELATIONSHIPCATEGORY " + sCWEID + " " + sCWERelationshipNature + " " + sCWERelationshipTargetCWEID);
                                        }
                                    }
                                }
                            }
                            catch (Exception exRelationships)
                            {
                                Console.WriteLine("Exception: exRelationships: " + exRelationships.Message + " " + exRelationships.InnerException);
                            }
                            break;
                        #endregion CWERelationships

                        case "Related_Attack_Patterns":
                        #region CWERelated_Attack_Patterns
                            try
                            {
                                foreach (XmlNode nodeCWEAttackPattern in nodeCWEinfo)
                                {
                                    string sCAPECversion = "";
                                    try
                                    {
                                        sCAPECversion = nodeCWEAttackPattern.Attributes["CAPEC_Version"].InnerText;  //2.1
                                    }
                                    catch (Exception exsCAPECversion)
                                    {
                                        string sIgnoreWarning = exsCAPECversion.Message;
                                        Console.WriteLine("ERROR: sCAPECversion not found");
                                    }
                                    foreach (XmlNode nodeCWECAPEC in nodeCWEAttackPattern)
                                    {
                                        string sCAPECID = "CAPEC-"+nodeCWECAPEC.InnerText;  //TODO: Check that it is a CAPEC
                                        //Check if the CAPEC (ATTACKPATTERN) exists
                                        int iAttackPatternID = 0;
                                        try
                                        {
                                            iAttackPatternID = attack_model.ATTACKPATTERN.Where(o => o.capec_id == sCAPECID).Select(o => o.AttackPatternID).FirstOrDefault();
                                        }
                                        catch(Exception ex)
                                        {

                                        }
                                        if(iAttackPatternID<=0)
                                        {
                                            ATTACKPATTERN oAttackPattern = new ATTACKPATTERN();
                                            oAttackPattern.CreatedDate = DateTimeOffset.Now;
                                            oAttackPattern.capec_id = sCAPECID;
                                            oAttackPattern.VocabularyID = iCWEVocabularyID;
                                            oAttackPattern.timestamp = DateTimeOffset.Now;
                                            attack_model.ATTACKPATTERN.Add(oAttackPattern);
                                            attack_model.SaveChanges();
                                            iAttackPatternID = oAttackPattern.AttackPatternID;
                                        }
                                        else
                                        {
                                            //Update ATTACKPATTERN (CAPEC)
                                        }
                                        // Replaced by ATTACKPATTERNCWE, then ATTACKPATTERNWEAKNESS
                                        //CWEFORCAPEC CWECAPEC = model.CWEFORCAPEC.Where(o => o.CWEID == sCWEID && o.capec_id == sCAPECID).FirstOrDefault();  //TODO: sCAPECversion
                                        int iCWECAPECID=0;
                                        try
                                        {
                                            iCWECAPECID = attack_model.ATTACKPATTERNCWE.Where(o => o.CWEID == sCWEID && o.AttackPatternID == iAttackPatternID).Select(o => o.AttackPatternCWEID).FirstOrDefault();
                                        }
                                        catch(Exception ex)
                                        {

                                        }
                                        //if (CWECAPEC != null)
                                        if(iCWECAPECID>0)
                                        {
                                            //Update CWEFORCAPEC
                                            //NOTE: Already exists. WeaknessRelationship = "Targeted"?
                                        }
                                        else
                                        {
                                            
                                            try
                                            {
                                                ATTACKPATTERNCWE CWECAPEC = new ATTACKPATTERNCWE();
                                                CWECAPEC.CreatedDate = DateTimeOffset.Now;
                                                CWECAPEC.CWEID = sCWEID;
                                                CWECAPEC.AttackPatternID = iAttackPatternID;
                                                //CWECAPEC.capec_id = sCAPECID;
                                                //Note: we don't have a Weakness_Relationship_Type like in CAPEC
                                                CWECAPEC.WeaknessRelationship = "Targeted"; //Default Hardcoded
                                                CWECAPEC.VocabularyID = iCWEVocabularyID;
                                                CWECAPEC.timestamp = DateTimeOffset.Now;
                                                attack_model.ATTACKPATTERNCWE.Add(CWECAPEC);
                                                attack_model.SaveChanges();    //TEST PERFORMANCE
                                                //iCWECAPECID=
                                                Console.WriteLine("DEBUG AddToCWEFORCAPEC " + sCWEID + " " + sCAPECID);
                                            }
                                            catch (Exception exCWEAttackPattern1)
                                            {
                                                Console.WriteLine("Exception: exCWEAttackPattern1: " + sCWEID + " " + "CAPEC-" + sCAPECID + " " + exCWEAttackPattern1.Message + " " + exCWEAttackPattern1.InnerException);
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception exCWEAttackPattern)
                            {
                                Console.WriteLine("Exception: exCWEAttackPattern: " + exCWEAttackPattern.Message + " " + exCWEAttackPattern.InnerException);
                            }
                            break;
                        #endregion CWERelated_Attack_Patterns

                        case "Taxonomy_Mappings":
                        #region CWETaxonomy_Mappings
                            try
                            {
                                int iTaxonomyID = 0;
                                int iVocabularyID = 0;

                                foreach (XmlNode nodeCWETaxonomy in nodeCWEinfo)
                                {
                                    string sCWEMappedTaxonomyName = string.Empty;
                                    try
                                    {
                                        sCWEMappedTaxonomyName = CleaningCWEString(nodeCWETaxonomy.Attributes["Mapped_Taxonomy_Name"].InnerText);
                                        Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                                        Console.WriteLine("DEBUG sCWEMappedTaxonomyName=" + sCWEMappedTaxonomyName);
                                        //OWASP Top Ten 2004
                                        //TODO? 2010    2013
                                    }
                                    catch (Exception exsCWEMappedTaxonomyName)
                                    {
                                        Console.WriteLine("Exception: exsCWEMappedTaxonomyName: " + exsCWEMappedTaxonomyName.Message + " " + exsCWEMappedTaxonomyName.InnerException);
                                    }

                                    //VOCABULARY oVocabulary = model.VOCABULARY.Where(o => o.VocabularyName == sCWEMappedTaxonomyName).FirstOrDefault();
                                    //VOCABULARY oVocabulary = model.VOCABULARY.FirstOrDefault(o => o.VocabularyName == sCWEMappedTaxonomyName);
                                    //int iVocabularyID=0;
                                    try
                                    {
                                        iVocabularyID= model.VOCABULARY.Where(o => o.VocabularyName == sCWEMappedTaxonomyName).Select(o=>o.VocabularyID).FirstOrDefault();
                                    }
                                    catch(Exception ex)
                                    {

                                    }
                                    //if (oVocabulary != null)
                                    if(iVocabularyID>0)
                                    {
                                        //Update VOCABULARY
                                    }
                                    else
                                    {
                                        try
                                        {
                                            XORCISMModel.VOCABULARY oVocabulary = new XORCISMModel.VOCABULARY();
                                            oVocabulary.VocabularyName = sCWEMappedTaxonomyName;
                                            oVocabulary.CreatedDate = DateTimeOffset.Now;
                                            oVocabulary.timestamp = DateTimeOffset.Now;
                                            
                                            model.VOCABULARY.Add(oVocabulary);
                                            model.SaveChanges();
                                            iVocabularyID=oVocabulary.VocabularyID;
                                            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                            Console.WriteLine("DEBUG: AddToVOCABULARY " + sCWEID + " " + sCWEMappedTaxonomyName);
                                        }
                                        catch (Exception exAddToVOCABULARY)
                                        {
                                            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                            Console.WriteLine("Exception: exAddToVOCABULARY: " + exAddToVOCABULARY.Message + " " + exAddToVOCABULARY.InnerException);
                                        }
                                    }

                                    //TAXONOMY oTaxonomy = model.TAXONOMY.Where(o => o.TaxonomyName == sCWEMappedTaxonomyName).FirstOrDefault();  //TODO: VocabularyID?
                                    //int iTaxonomyID=0;
                                    try
                                    {
                                        iTaxonomyID= model.TAXONOMY.Where(o => o.TaxonomyName == sCWEMappedTaxonomyName).Select(o=>o.TaxonomyID).FirstOrDefault();
                                    }
                                    catch(Exception ex)
                                    {

                                    }
                                    //if (oTaxonomy != null)
                                    if(iTaxonomyID>0)
                                    {
                                        //Update TAXONOMY
                                    }
                                    else
                                    {
                                        try
                                        {
                                            TAXONOMY oTaxonomy = new TAXONOMY();
                                            oTaxonomy.TaxonomyName = sCWEMappedTaxonomyName;
                                            oTaxonomy.CreatedDate = DateTimeOffset.Now;
                                            oTaxonomy.timestamp = DateTimeOffset.Now;
                                            oTaxonomy.VocabularyID = iVocabularyID; //oVocabulary.VocabularyID;
                                            model.TAXONOMY.Add(oTaxonomy);
                                            model.SaveChanges();
                                            iTaxonomyID = oTaxonomy.TaxonomyID;
                                            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                            Console.WriteLine("DEBUG AddToTAXONOMY " + sCWEID + " " + sCWEMappedTaxonomyName);
                                        }
                                        catch (Exception exAddToTAXONOMY)
                                        {
                                            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                            Console.WriteLine("Exception: exAddToTAXONOMY: " + exAddToTAXONOMY.Message + " " + exAddToTAXONOMY.InnerException);
                                        }
                                    }

                                    switch(sCWEMappedTaxonomyName)
                                    //if (sCWEMappedTaxonomyName == "WASC") //2.0
                                    {
                                        case "WASC":    //2.0
                                            #region taxonomywasc
                                            //http://projects.webappsec.org/w/page/13246975/Threat%20Classification%20Taxonomy%20Cross%20Reference%20View
                                            foreach (XmlNode nodeCWEWASC in nodeCWETaxonomy)
                                            {
                                                //<Mapped_Node_Name>Server Misconfiguration </Mapped_Node_Name>
                                                //<Mapped_Node_ID>14</Mapped_Node_ID>
                                                if (nodeCWEWASC.Name == "Mapped_Node_ID")
                                                {
                                                
                                                    string sWASCRefID = nodeCWEWASC.InnerText;
                                                    if (sWASCRefID.Length < 2)
                                                    {
                                                        sWASCRefID = "0" + sWASCRefID;
                                                    }
                                                    sWASCRefID = "WASC-" + sWASCRefID;
                                                    //WASC WASCObject = model.WASC.Where(o => o.WASCRefID == sWASCRefID).FirstOrDefault();
                                                    int iWASCID = 0;
                                                    try
                                                    {
                                                        iWASCID = model.WASC.Where(o => o.WASCRefID == sWASCRefID).Select(o=>o.WASCID).FirstOrDefault();
                                                    }
                                                    catch(Exception ex)
                                                    {

                                                    }
                                                    //if (WASCObject == null)
                                                    if (iWASCID<=0)
                                                    {
                                                        Console.WriteLine("ERROR: WASC unknown (will be added) " + sWASCRefID+" please update the WASC table manually");
                                                        WASC oWASC = new WASC();
                                                        oWASC.CreatedDate=DateTimeOffset.Now;
                                                        oWASC.WASCRefID=sWASCRefID;
                                                        oWASC.WASCThreatType = "";
                                                        oWASC.VocabularyID=iVocabularyCWEID;
                                                        oWASC.timestamp=DateTimeOffset.Now;
                                                        model.WASC.Add(oWASC);
                                                        model.SaveChanges();
                                                        Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                        Console.WriteLine("DEBUG WASC Added");
                                                        iWASCID = oWASC.WASCID;
                                                    }
                                                    else
                                                    {
                                                        //int iWASCID = WASCObject.WASCID;
                                                        //WASCCWE WASCforCWE = model.WASCCWE.Where(o => o.CWEID == sCWEID && o.WASCID == iWASCID).FirstOrDefault();
                                                        int iWASCCWEID = 0;
                                                        try
                                                        {
                                                            iWASCCWEID = model.WASCCWE.Where(o => o.CWEID == sCWEID && o.WASCID == iWASCID).Select(o=>o.WASCCWEID).FirstOrDefault();
                                                        }
                                                        catch(Exception ex)
                                                        {

                                                        }
                                                        //if (WASCforCWE != null)
                                                        if (iWASCCWEID>0)
                                                        {
                                                            //Update WASCCWE
                                                        }
                                                        else
                                                        {
                                                            WASCCWE WASCforCWE = new WASCCWE();
                                                            WASCforCWE.CreatedDate = DateTimeOffset.Now;
                                                            WASCforCWE.CWEID = sCWEID;
                                                            WASCforCWE.WASCID = iWASCID;
                                                            WASCforCWE.VocabularyID = iVocabularyCWEID;
                                                            WASCforCWE.timestamp = DateTimeOffset.Now;
                                                            model.WASCCWE.Add(WASCforCWE);
                                                            //model.SaveChanges();    //TEST PERFORMANCE
                                                            //iWASCCWEID=
                                                            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                            Console.WriteLine("DEBUG AddToWASCCWE " + sCWEID + " " + sWASCRefID);
                                                        }
                                                    }
                                                }
                                            }
                                            #endregion taxonomywasc
                                            break;

                                        //TODO
                                        
                                        case "OWASP Top Ten 2004":
                                            //Note: The generic Taxonomy mapping (see below) could be used
                                            #region taxonomyowasptop2004
                                            string sTaxonomyMappedNodeName = string.Empty;
                                            string sTaxonomyMappedNodeID = string.Empty;
                                            string sTaxonomyMappingFit = string.Empty;
                                            //OWASPTOP10 oOWASPTOP10 = new OWASPTOP10();
                                            int iOWASPTOP10ID = 0;
                                            foreach (XmlNode nodeCWEOWASPTOP2004 in nodeCWETaxonomy)
                                            {
                                                switch (nodeCWEOWASPTOP2004.Name)
                                                {
                                                    case "Mapped_Node_Name":
                                                        //Insecure Configuration Management
                                                        sTaxonomyMappedNodeName = CleaningCWEString(nodeCWEOWASPTOP2004.InnerText);
                                                        //Console.WriteLine("DEBUG " + sCWEID + " sTaxonomyMappedNodeName:" + sTaxonomyMappedNodeName);
                                                        break;
                                                    case "Mapped_Node_ID":
                                                        //A10
                                                        sTaxonomyMappedNodeID = CleaningCWEString(nodeCWEOWASPTOP2004.InnerText);
                                                        Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                        Console.WriteLine("DEBUG " + sCWEID + " sTaxonomyMappedNodeID:" + sTaxonomyMappedNodeID);
                                                        //oOWASPTOP10 = model.OWASPTOP10.Where(o => o.YearTop10 == 2004 && o.OWASPName == sTaxonomyMappedNodeName).FirstOrDefault();
                                                        //int iOWASPTOP10ID = 0;
                                                        try
                                                        {
                                                            iOWASPTOP10ID = model.OWASPTOP10.Where(o => o.YearTop10 == 2004 && o.OWASPName == sTaxonomyMappedNodeName).Select(o=>o.OWASPTOP10ID).FirstOrDefault();
                                                        }
                                                        catch(Exception ex)
                                                        {

                                                        }
                                                        //if (oOWASPTOP10 != null)
                                                        if(iOWASPTOP10ID>0)
                                                        {
                                                            //Update OWASPTOP10 2004
                                                            /*
                                                            int iOWASPTOP10Rank = 0;
                                                            try
                                                            {
                                                                iOWASPTOP10Rank = Convert.ToInt32(sTaxonomyMappedNodeID.Replace("A", ""));
                                                            }
                                                            catch(Exception exiOWASPTOP10Rank)
                                                            {
                                                                Console.WriteLine("Exception: exiOWASPTOP10Rank " + exiOWASPTOP10Rank.Message + " " + exiOWASPTOP10Rank.InnerException);
                                                            }
                                                            oOWASPTOP10.Rank = iOWASPTOP10Rank;
                                                            oOWASPTOP10.timestamp = DateTimeOffset.Now;
                                                            model.SaveChanges();
                                                            */
                                                        }
                                                        else
                                                        {
                                                            OWASPTOP10 oOWASPTOP10 = new OWASPTOP10();
                                                            oOWASPTOP10.OWASPName = sTaxonomyMappedNodeName;
                                                            oOWASPTOP10.YearTop10 = 2004;
                                                            oOWASPTOP10.CreatedDate = DateTimeOffset.Now;
                                                            oOWASPTOP10.VocabularyID = iVocabularyCWEID;
                                                            oOWASPTOP10.timestamp = DateTimeOffset.Now;
                                                            int iOWASPTOP10Rank = 0;
                                                            try
                                                            {
                                                                iOWASPTOP10Rank=Convert.ToInt32(sTaxonomyMappedNodeID.Replace("A", ""));
                                                            }
                                                            catch (Exception exiOWASPTOP10Rank)
                                                            {
                                                                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                                Console.WriteLine("Exception: exiOWASPTOP10Rank " + exiOWASPTOP10Rank.Message + " " + exiOWASPTOP10Rank.InnerException);
                                                            }
                                                            oOWASPTOP10.Rank = iOWASPTOP10Rank;
                                                            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                            Console.WriteLine("DEBUG AddToOWASPTOP10 2004: " + sTaxonomyMappedNodeName);
                                                            model.OWASPTOP10.Add(oOWASPTOP10);
                                                            model.SaveChanges();
                                                            iOWASPTOP10ID = oOWASPTOP10.OWASPTOP10ID;
                                                        }

                                                        break;

                                                    case "Mapping_Fit":
                                                        sTaxonomyMappingFit = CleaningCWEString(nodeCWEOWASPTOP2004.InnerText);

                                                        break;

                                                    default:
                                                        Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                        Console.WriteLine("ERROR: Missing code for CWETaxonomy_MappingNode: " + nodeCWEOWASPTOP2004.Name);
                                                        
                                                        break;
                                                }
                                            }

                                            //CWEFOROWASPTOP10 oCWEOWASPTOP10 = model.CWEFOROWASPTOP10.FirstOrDefault(o => o.CWEID == sCWEID && o.OWASPTOP10ID == oOWASPTOP10.OWASPTOP10ID);
                                            int iCWEFOROWASPTOP10ID = 0;
                                            try
                                            {
                                                iCWEFOROWASPTOP10ID = model.CWEFOROWASPTOP10.Where(o => o.CWEID == sCWEID && o.OWASPTOP10ID == iOWASPTOP10ID).Select(o=>o.CWEOWASPTOP10ID).FirstOrDefault();
                                            }
                                            catch(Exception ex)
                                            {

                                            }
                                            //if (oCWEOWASPTOP10 == null)
                                            if (iCWEFOROWASPTOP10ID<=0)
                                            {
                                                CWEFOROWASPTOP10 oCWEOWASPTOP10 = new CWEFOROWASPTOP10();
                                                oCWEOWASPTOP10.OWASPTOP10ID = iOWASPTOP10ID;   //oOWASPTOP10.OWASPTOP10ID;
                                                oCWEOWASPTOP10.CWEID = sCWEID;
                                                oCWEOWASPTOP10.Mapping_Fit = sTaxonomyMappingFit;
                                                oCWEOWASPTOP10.CreatedDate = DateTimeOffset.Now;
                                                oCWEOWASPTOP10.timestamp = DateTimeOffset.Now;
                                                oCWEOWASPTOP10.VocabularyID = iCWEVocabularyID;
                                                model.CWEFOROWASPTOP10.Add(oCWEOWASPTOP10);
                                                //model.SaveChanges();    //TEST PERFORMANCE
                                                //iCWEFOROWASPTOP10ID=
                                            }
                                            else
                                            {
                                                //Update CWEFOROWASPTOP10
                                            }

                                            //TODO See also: OWASPTOP10MAPPING

                                            #endregion taxonomyowasptop2004
                                            break;
                                        

                                        //TODO
                                        /*
                                        case "PLOVER":

                                            break;
                                        */

                                        default:
                                            //TODO
                                            //TAXONOMY  VOCABULARY    (STANDARD)

                                            //REFERENCE

                                            //Console.WriteLine("ERROR: Missing code for CWETaxonomy_Mapping: " + sCWEMappedTaxonomyName);
                                            TAXONOMYNODE oTaxonomyNode = null;
                                            CWETAXONOMYNODE oCWETaxonomyNode = null;
                                            foreach (XmlNode nodeCWETaxonomyNode in nodeCWETaxonomy)
                                            {
                                                string sTaxonomyNodeName=CleaningCWEString(nodeCWETaxonomyNode.InnerText);
                                                //Integer coercion error

                                                if (nodeCWETaxonomyNode.Name == "Mapped_Node_Name")
                                                {
                                                    oTaxonomyNode = model.TAXONOMYNODE.FirstOrDefault(o => o.TaxonomyID == iTaxonomyID && o.TaxonomyNodeName == sTaxonomyNodeName);
                                                    if (oTaxonomyNode != null)
                                                    {
                                                        //Update TAXONOMYNODE
                                                    }
                                                    else
                                                    {
                                                        
                                                        try
                                                        {
                                                            oTaxonomyNode = new TAXONOMYNODE();
                                                            oTaxonomyNode.TaxonomyID = iTaxonomyID; // oTaxonomy.TaxonomyID;
                                                            oTaxonomyNode.TaxonomyNodeName = sTaxonomyNodeName;
                                                            oTaxonomyNode.CreatedDate = DateTimeOffset.Now;
                                                            oTaxonomyNode.timestamp = DateTimeOffset.Now;
                                                            oTaxonomyNode.VocabularyID = iVocabularyID; // oVocabulary.VocabularyID;
                                                            model.TAXONOMYNODE.Add(oTaxonomyNode);
                                                            model.SaveChanges();
                                                            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                            Console.WriteLine("DEBUG AddToTAXONOMYNODE ");// + sTaxonomyNodeName);
                                                        }
                                                        catch (Exception exAddToTAXONOMYNODE)
                                                        {
                                                            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                            Console.WriteLine("Exception: AddToTAXONOMYNODE " + exAddToTAXONOMYNODE.Message + " " + exAddToTAXONOMYNODE.InnerException);
                                                        }
                                                    }

                                                    oCWETaxonomyNode = model.CWETAXONOMYNODE.FirstOrDefault(o => o.CWEID == sCWEID && o.TaxonomyNodeID == oTaxonomyNode.TaxonomyNodeID);
                                                    if (oCWETaxonomyNode != null)
                                                    {
                                                        //Update CWETAXONOMYNODE
                                                    }
                                                    else
                                                    {
                                                        
                                                        try
                                                        {
                                                            oCWETaxonomyNode = new CWETAXONOMYNODE();
                                                            oCWETaxonomyNode.CWEID = sCWEID;
                                                            oCWETaxonomyNode.TaxonomyNodeID = oTaxonomyNode.TaxonomyNodeID;
                                                            oCWETaxonomyNode.CreatedDate = DateTimeOffset.Now;
                                                            oCWETaxonomyNode.timestamp = DateTimeOffset.Now;
                                                            oCWETaxonomyNode.VocabularyID = iVocabularyID;  // oVocabulary.VocabularyID;
                                                            model.CWETAXONOMYNODE.Add(oCWETaxonomyNode);
                                                            //model.SaveChanges();    //TEST PERFORMANCE
                                                            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                            Console.WriteLine("DEBUG AddToCWETAXONOMYNODE " + sCWEID);// + " " + oTaxonomyNode.TaxonomyNodeName);
                                                        }
                                                        catch (Exception exAddToCWETAXONOMYNODE)
                                                        {
                                                            Console.WriteLine("Exception: exAddToCWETAXONOMYNODE: " + exAddToCWETAXONOMYNODE.Message + " " + exAddToCWETAXONOMYNODE.InnerException);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    //TODO: switch
                                                    if (nodeCWETaxonomyNode.Name == "Mapped_Node_ID")
                                                    {
                                                        //INT02-C
                                                        //Update TAXONOMYNODE
                                                        oTaxonomyNode.TaxonomyMappedNodeID = nodeCWETaxonomyNode.InnerText;
                                                        //model.SaveChanges();    //TEST PERFORMANCE
                                                    }
                                                    else
                                                    {
                                                        if (nodeCWETaxonomyNode.Name == "Mapping_Fit")
                                                        {
                                                            //Update CWETAXONOMYNODE
                                                            oCWETaxonomyNode.Mapping_Fit = nodeCWETaxonomyNode.InnerText.Trim();
                                                            model.SaveChanges();
                                                        }
                                                        else
                                                        {
                                                            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                            Console.WriteLine("ERROR: Missing code for " + nodeCWETaxonomyNode.Name);
                                                        }
                                                    }
                                                }
                                            }
                                            break;
                                    }
                                    /*
                                    else
                                    {
                                        Console.WriteLine("ERROR: Missing code for CWETaxonomy_Mapping: " + sCWEMappedTaxonomyName);
                                    }
                                    */
                                }
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
                                Console.WriteLine("Exception: DbEntityValidationExceptionexCWETaxonomy_Mapping " + sb.ToString());
                            }
                            catch (Exception exCWETaxonomy_Mapping)
                            {
                                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                Console.WriteLine("Exception: exCWETaxonomy_Mapping: " + exCWETaxonomy_Mapping.Message + " " + exCWETaxonomy_Mapping.InnerException);
                            }
                            break;
                        #endregion CWETaxonomy_Mappings

                        case "Applicable_Platforms":
                        #region CWEApplicable_Platforms
                            try
                            {
                                foreach (XmlNode nodeCWEPlatform in nodeCWEinfo)
                                {
                                    //<Languages>
                                    switch(nodeCWEPlatform.Name)
                                    {
                                        case "Languages":
                                            #region languages
                                            foreach (XmlNode nodeCWEPlatformLanguage in nodeCWEPlatform)
                                            {
                                                string sPrevalence = string.Empty;
                                                //<Language_Class Language_Class_Description="All"/>
                                                //<Language Prevalence="Often" Language_Name="C"/>
                                                //<Language Prevalence="Often" Language_Name="C++"/>
                                                //<Language Language_Name="Assembly"/>
                                                //<Language_Class Language_Class_Description="Languages without memory management support"/>
                                                switch (nodeCWEPlatformLanguage.Name)
                                                {
                                                    case "Language":
                                                        try
                                                        {
                                                            sPrevalence = nodeCWEPlatformLanguage.Attributes["Prevalence"].InnerText;
                                                        }
                                                        catch (Exception exNoPrevalence)
                                                        {
                                                            string sIgnoreWarning = exNoPrevalence.Message;
                                                        }
                                                        string sLanguage = nodeCWEPlatformLanguage.Attributes["Language_Name"].InnerText;
                                                        //Cleaning?
                                                        //LANGUAGE oLanguage = model.LANGUAGE.Where(o => o.LanguageName == sLanguage).FirstOrDefault();
                                                        int iLanguageID = 0;
                                                        try
                                                        {
                                                            iLanguageID = model.LANGUAGE.Where(o => o.LanguageName == sLanguage).Select(o=>o.LanguageID).FirstOrDefault();
                                                        }
                                                        catch(Exception ex)
                                                        {

                                                        }
                                                        //if (oLanguage != null)
                                                        if (iLanguageID>0)
                                                        {
                                                            //Update LANGUAGE
                                                        }
                                                        else
                                                        {
                                                            //Insert a new Language
                                                            
                                                            try
                                                            {
                                                                LANGUAGE oLanguage = new LANGUAGE();
                                                                oLanguage.CreatedDate = DateTimeOffset.Now;
                                                                oLanguage.LanguageName = sLanguage;
                                                                oLanguage.VocabularyID = iCWEVocabularyID;
                                                                oLanguage.timestamp = DateTimeOffset.Now;
                                                                model.LANGUAGE.Add(oLanguage);
                                                                model.SaveChanges();
                                                                iLanguageID = oLanguage.LanguageID;
                                                                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                                Console.WriteLine("DEBUG Added Language: " + sLanguage);
                                                            }
                                                            catch (Exception exAddToLANGUAGE)
                                                            {
                                                                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                                Console.WriteLine("Exception: exAddToLANGUAGE " + exAddToLANGUAGE.Message);
                                                            }
                                                        }

                                                        //CWELANGUAGE oCWELANGUAGE;
                                                        //oCWELANGUAGE = model.CWELANGUAGE.FirstOrDefault(o => o.CWEID == sCWEID && o.LanguageID == oLanguage.LanguageID);
                                                        int iCWELanguageID = 0;
                                                        try
                                                        {
                                                            iCWELanguageID = model.CWELANGUAGE.Where(o => o.CWEID == sCWEID && o.LanguageID == iLanguageID).Select(o=>o.CWELanguageID).FirstOrDefault();
                                                        }
                                                        catch(Exception ex)
                                                        {

                                                        }
                                                        //if (oCWELANGUAGE == null)
                                                        if (iCWELanguageID<=0)
                                                        {
                                                            try
                                                            {
                                                                CWELANGUAGE oCWELANGUAGE = new CWELANGUAGE();
                                                                oCWELANGUAGE.CWEID = sCWEID;
                                                                oCWELANGUAGE.LanguageID = iLanguageID;  // oLanguage.LanguageID;
                                                                oCWELANGUAGE.Prevalence = sPrevalence;
                                                                oCWELANGUAGE.CreatedDate=DateTimeOffset.Now;
                                                                oCWELANGUAGE.timestamp=DateTimeOffset.Now;
                                                                oCWELANGUAGE.VocabularyID = iCWEVocabularyID;
                                                                model.CWELANGUAGE.Add(oCWELANGUAGE);
                                                                //model.SaveChanges();    //TEST PERFORMANCE
                                                                //iCWELanguageID=
                                                            }
                                                            catch (Exception exAddToCWELANGUAGE)
                                                            {
                                                                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                                Console.WriteLine("Exception: exAddToCWELANGUAGE " + exAddToCWELANGUAGE.Message);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            //Update CWELANGUAGE
                                                        }
                                                        break;
                                                    case "Language_Class":
                                                        string sLanguageClassDescription = nodeCWEPlatformLanguage.Attributes["Language_Class_Description"].InnerText;
                                                        //LANGUAGECLASS oLanguageClass = model.LANGUAGECLASS.Where(o => o.LanguageClassDescription == sLanguageClassDescription).FirstOrDefault();
                                                        int iLanguageClassID = 0;
                                                        try
                                                        {
                                                            iLanguageClassID = model.LANGUAGECLASS.Where(o => o.LanguageClassDescription == sLanguageClassDescription).Select(o=>o.LanguageClassID).FirstOrDefault();
                                                        }
                                                        catch(Exception ex)
                                                        {

                                                        }
                                                        //if (oLanguageClass != null)
                                                        if (iLanguageClassID>0)
                                                        {
                                                            //Update LANGUAGECLASS
                                                        }
                                                        else
                                                        {
                                                            LANGUAGECLASS oLanguageClass = new LANGUAGECLASS();
                                                            oLanguageClass.LanguageClassDescription = sLanguageClassDescription;
                                                            oLanguageClass.VocabularyID = iCWEVocabularyID;
                                                            model.LANGUAGECLASS.Add(oLanguageClass);
                                                            model.SaveChanges();
                                                            iLanguageClassID = oLanguageClass.LanguageClassID;
                                                        }

                                                        //CWELANGUAGECLASS oCWELanguageClass = model.CWELANGUAGECLASS.FirstOrDefault(o => o.CWEID == sCWEID && o.LanguageClassID == iLanguageClassID);
                                                        int iCWELanguageClassID = 0;
                                                        try
                                                        {
                                                            iCWELanguageClassID = model.CWELANGUAGECLASS.Where(o => o.CWEID == sCWEID && o.LanguageClassID == iLanguageClassID).Select(o=>o.CWELanguageClassID).FirstOrDefault();
                                                        }
                                                        catch(Exception ex)
                                                        {

                                                        }
                                                        //if (oCWELanguageClass == null)
                                                        if (iCWELanguageClassID<=0)
                                                        {
                                                            CWELANGUAGECLASS oCWELanguageClass = new CWELANGUAGECLASS();
                                                            oCWELanguageClass.CWEID = sCWEID;
                                                            oCWELanguageClass.LanguageClassID = iLanguageClassID;   // oLanguageClass.LanguageClassID;
                                                            oCWELanguageClass.CreatedDate = DateTimeOffset.Now;
                                                            oCWELanguageClass.timestamp = DateTimeOffset.Now;
                                                            oCWELanguageClass.VocabularyID = iCWEVocabularyID;
                                                            model.CWELANGUAGECLASS.Add(oCWELanguageClass);
                                                            //model.SaveChanges();    //TEST PERFORMANCE
                                                            //iCWELanguageClassID=
                                                        }
                                                        else
                                                        {
                                                            //Update CWELANGUAGECLASS
                                                        }

                                                        break;
                                                    default:
                                                        Console.WriteLine("ERROR: Missing code for nodeCWEPlatformLanguage " + nodeCWEPlatformLanguage.Name);
                                                        break;
                                                }
                                            }
                                            break;
                                            #endregion languages

                                        case "Technology_Classes":
                                            #region technologyclasses
                                            foreach (XmlNode nodeCWETechnologyClass in nodeCWEPlatform)
                                            {
                                                string sTechnology = nodeCWETechnologyClass.Attributes["Technology_Name"].InnerText;
                                                //TECHNOLOGY oTechnology = model.TECHNOLOGY.FirstOrDefault(o => o.TechnologyName == sTechnology);
                                                int iTechnologyID = 0;
                                                try
                                                {
                                                    iTechnologyID = model.TECHNOLOGY.Where(o => o.TechnologyName == sTechnology).Select(o=>o.TechnologyID).FirstOrDefault();
                                                }
                                                catch(Exception ex)
                                                {

                                                }
                                                //if (oTechnology == null)
                                                if (iTechnologyID<=0)
                                                {
                                                    TECHNOLOGY oTechnology = new TECHNOLOGY();
                                                    oTechnology.TechnologyName = sTechnology;
                                                    oTechnology.CreatedDate = DateTimeOffset.Now;
                                                    oTechnology.timestamp = DateTimeOffset.Now;
                                                    oTechnology.VocabularyID = iCWEVocabularyID;
                                                    model.TECHNOLOGY.Add(oTechnology);
                                                    model.SaveChanges();
                                                    iTechnologyID = oTechnology.TechnologyID;
                                                }
                                                else
                                                {
                                                    //Update TECHNOLOGY
                                                }

                                                string sCWETechnologyPrevalence = "";
                                                try
                                                {
                                                    sCWETechnologyPrevalence = nodeCWETechnologyClass.Attributes["Prevalence"].InnerText;
                                                }
                                                catch (Exception exsCWETechnologyPrevalence)
                                                {
                                                    Console.WriteLine("DEBUG no Prevalence for Technology for " + sCWEID);
                                                }

                                                //CWETECHNOLOGY oCWETechnology = model.CWETECHNOLOGY.FirstOrDefault(o => o.CWEID == sCWEID && o.TechnologyID == iTechnologyID);
                                                int iCWETechnologyID = 0;
                                                try
                                                {
                                                    iCWETechnologyID = model.CWETECHNOLOGY.Where(o => o.CWEID == sCWEID && o.TechnologyID == iTechnologyID).Select(o=>o.CWETechnologyID).FirstOrDefault();
                                                }
                                                catch(Exception ex)
                                                {

                                                }
                                                //if (oCWETechnology == null)
                                                if (iCWETechnologyID<=0)
                                                {
                                                    CWETECHNOLOGY oCWETechnology = new CWETECHNOLOGY();
                                                    oCWETechnology.TechnologyID = iTechnologyID; // oTechnology.TechnologyID;
                                                    oCWETechnology.CWEID = sCWEID;
                                                    oCWETechnology.CreatedDate = DateTimeOffset.Now;
                                                    oCWETechnology.timestamp = DateTimeOffset.Now;

                                                    model.CWETECHNOLOGY.Add(oCWETechnology);
                                                    //model.SaveChanges();    //TEST PERFORMANCE
                                                }
                                                else
                                                {
                                                    //Update CWETECHNOLOGY
                                                }
                                            }
                                            break;
                                            #endregion technologyclasses

                                        
                                        case "Platform_Notes":
                                            CWEObject.Platform_Notes = CleaningCWEString(nodeCWEPlatform.InnerText);
                                            model.SaveChanges();
                                            break;

                                        //Architectural_Paradigms
                                        //TODO
                                        //CWEARCHITECTURALPARADIGM
                                        case "Architectural_Paradigms":
                                            #region architecturalparadigm
                                            foreach (XmlNode nodeCWEArchitecturalParadigm in nodeCWEPlatform)
                                            {
                                                if (nodeCWEArchitecturalParadigm.Name != "Architectural_Paradigm")
                                                {
                                                    Console.WriteLine("ERROR: Missing code for nodeCWEArchitecturalParadigm " + nodeCWEArchitecturalParadigm.Name);
                                                }
                                                else
                                                {
                                                    try
                                                    {
                                                        string sArchitecturalParadigm=nodeCWEArchitecturalParadigm.Attributes["Architectural_Paradigm_Name"].InnerText;
                                                        //Cleaning?
                                                        //TODO: Check if other attributes
                                                        //ARCHITECTURALPARADIGM oArchitectureParadigm = model.ARCHITECTURALPARADIGM.FirstOrDefault(o => o.ArchitecturalParadigmName == sArchitecturalParadigm);
                                                        int iArchitecturalParadigmID = 0;
                                                        try
                                                        {
                                                            iArchitecturalParadigmID = model.ARCHITECTURALPARADIGM.Where(o => o.ArchitecturalParadigmName == sArchitecturalParadigm).Select(o=>o.ArchitecturalParadigmID).FirstOrDefault();
                                                        }
                                                        catch(Exception ex)
                                                        {

                                                        }
                                                        //if(oArchitectureParadigm==null)
                                                        if (iArchitecturalParadigmID<=0)
                                                        {
                                                            ARCHITECTURALPARADIGM oArchitectureParadigm = new ARCHITECTURALPARADIGM();
                                                            oArchitectureParadigm.ArchitecturalParadigmName=sArchitecturalParadigm;
                                                            oArchitectureParadigm.CreatedDate=DateTimeOffset.Now;
                                                            oArchitectureParadigm.VocabularyID=iCWEVocabularyID;
                                                            oArchitectureParadigm.timestamp=DateTimeOffset.Now;
                                                            model.ARCHITECTURALPARADIGM.Add(oArchitectureParadigm);
                                                            model.SaveChanges();
                                                            iArchitecturalParadigmID = oArchitectureParadigm.ArchitecturalParadigmID;
                                                        }
                                                        else
                                                        {
                                                            //Update ARCHITECTURALPARADIGM
                                                        }

                                                        //CWEARCHITECTURALPARADIGM oCWEArchitecturalParadigm = model.CWEARCHITECTURALPARADIGM.FirstOrDefault(o => o.CWEID == sCWEID && o.CWEArchitecturalParadigmID == iArchitecturalParadigmID);
                                                        int ioCWEArchitecturalParadigmID = 0;
                                                        try
                                                        {
                                                            ioCWEArchitecturalParadigmID = model.CWEARCHITECTURALPARADIGM.Where(o => o.CWEID == sCWEID && o.CWEArchitecturalParadigmID == iArchitecturalParadigmID).Select(o=>o.CWEArchitecturalParadigmID).FirstOrDefault();
                                                        }
                                                        catch(Exception ex)
                                                        {

                                                        }
                                                        //if(oCWEArchitecturalParadigm==null)
                                                        if (ioCWEArchitecturalParadigmID<=0)
                                                        {
                                                            CWEARCHITECTURALPARADIGM oCWEArchitecturalParadigm = new CWEARCHITECTURALPARADIGM();
                                                            oCWEArchitecturalParadigm.CreatedDate = DateTimeOffset.Now;
                                                            oCWEArchitecturalParadigm.CWEID = sCWEID;
                                                            oCWEArchitecturalParadigm.ArchitecturalParadigmID = iArchitecturalParadigmID;   // oArchitectureParadigm.ArchitecturalParadigmID;
                                                            oCWEArchitecturalParadigm.VocabularyID = iCWEVocabularyID;
                                                            oCWEArchitecturalParadigm.timestamp = DateTimeOffset.Now;
                                                            model.CWEARCHITECTURALPARADIGM.Add(oCWEArchitecturalParadigm);
                                                            model.SaveChanges();
                                                        }
                                                        else
                                                        {
                                                            //Update CWEARCHITECTURALPARADIGM
                                                        }
                                                    }
                                                    catch(Exception exnodeCWEArchitecturalParadigm)
                                                    {
                                                        Console.WriteLine("Exception: exnodeCWEArchitecturalParadigm " + exnodeCWEArchitecturalParadigm.Message + " " + exnodeCWEArchitecturalParadigm.InnerException);
                                                    }
                                                }
                                            }
                                            break;
                                            #endregion architecturalparadigm

                                        case "Operating_Systems":
                                            #region operatingsystems
                                            foreach (XmlNode nodeCWEOS in nodeCWEPlatform)
                                            {
                                                switch(nodeCWEOS.Name)
                                                {
                                                    case "Operating_System":
                                                        string sOperating_System_Name = nodeCWEOS.Attributes["Operating_System_Name"].InnerText;
                                                        //Windows 2000
                                                        //Windows XP
                                                        //Windows Vista
                                                        //Mac OS X
                                                        //TODO REVIEW
                                                        string sOSShortName=sOperating_System_Name.Replace("Windows ","");
                                                        sOSShortName=sOSShortName.Replace("Mac OS X", "OSX");

                                                        string sPrevalence = "";
                                                        try
                                                        {
                                                            sPrevalence = nodeCWEOS.Attributes["Prevalence"].InnerText;
                                                        }
                                                        catch(Exception exOSPrevalence)
                                                        {
                                                            Console.WriteLine("ERROR: no Prevalence for OS " + sOperating_System_Name+" "+sCWEID);
                                                        }

                                                        //OS oOS = model.OS.FirstOrDefault(o => o.Operating_System_Name == sOperating_System_Name);
                                                        int iOSID =0;
                                                        try
                                                        {
                                                            iOSID=model.OS.Where(o => o.Operating_System_Name == sOperating_System_Name).Select(o => o.OSID).FirstOrDefault();
                                                        }
                                                        catch(Exception ex)
                                                        {

                                                        }
                                                        //if (oOS == null)
                                                        if (iOSID <= 0)
                                                        {
                                                           
                                                            try
                                                            {
                                                                OS oOS = new OS();
                                                                oOS.Operating_System_Name = sOperating_System_Name;
                                                                oOS.OSname = sOSShortName;
                                                                oOS.CreatedDate = DateTimeOffset.Now;
                                                                oOS.timestamp = DateTimeOffset.Now;
                                                                oOS.VocabularyID = iCWEVocabularyID;
                                                                model.OS.Add(oOS);
                                                                model.SaveChanges();
                                                                iOSID = oOS.OSID;
                                                            }
                                                            catch(Exception exOS)
                                                            {
                                                                Console.WriteLine("Exception: exOS " + exOS.Message + " " + exOS.InnerException);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            //At least one record for this OS
                                                            //Update OS
                                                        }

                                                        //CWEOS oCWEOS = model.CWEOS.FirstOrDefault(o => o.CWEID == sCWEID && o.Operating_System_Name == sOperating_System_Name);
                                                        int iCWEOSID =0;
                                                        try
                                                        {
                                                            iCWEOSID=model.CWEOS.Where(o => o.CWEID == sCWEID && o.Operating_System_Name == sOperating_System_Name).Select(o=>o.CWEOSID).FirstOrDefault();
                                                        }
                                                        catch(Exception ex)
                                                        {

                                                        }
                                                        //if (oCWEOS == null)
                                                        if (iCWEOSID <= 0)
                                                        {
                                                            CWEOS oCWEOS = new CWEOS();
                                                            oCWEOS.CWEID = sCWEID;
                                                            
                                                            oCWEOS.Operating_System_Name = sOperating_System_Name;
                                                            oCWEOS.Prevalence = sPrevalence;
                                                            oCWEOS.CreatedDate = DateTimeOffset.Now;
                                                            oCWEOS.timestamp = DateTimeOffset.Now;
                                                            oCWEOS.VocabularyID = iCWEVocabularyID;
                                                            model.CWEOS.Add(oCWEOS);
                                                            model.SaveChanges();
                                                        }
                                                        else
                                                        {
                                                            //Update CWEOS
                                                        }
                                                        break;

                                                    case "Operating_System_Class":
                                                        string sOperating_System_Class_Description = nodeCWEOS.Attributes["Operating_System_Class_Description"].InnerText;
                                                        //TODO: see if we can use the OSFAMILY table (OVAL)
                                                        //OSCLASS oOSClass = model.OSCLASS.FirstOrDefault(o => o.Operating_System_Class_Description == sOperating_System_Class_Description);
                                                        int iOSClassID =0;
                                                        try
                                                        {
                                                            iOSClassID=model.OSCLASS.Where(o => o.Operating_System_Class_Description == sOperating_System_Class_Description).Select(o=>o.OSClassID).FirstOrDefault();
                                                        }
                                                        catch(Exception ex)
                                                        {

                                                        }
                                                        //if (oOSClass == null)
                                                        if (iOSClassID == 0)
                                                        {
                                                            OSCLASS oOSClass = new OSCLASS();
                                                            oOSClass.Operating_System_Class_Description = sOperating_System_Class_Description;
                                                            oOSClass.CreatedDate = DateTimeOffset.Now;
                                                            oOSClass.timestamp = DateTimeOffset.Now;
                                                            oOSClass.VocabularyID = iCWEVocabularyID;
                                                            model.OSCLASS.Add(oOSClass);
                                                            model.SaveChanges();
                                                            iOSClassID = oOSClass.OSClassID;
                                                        }

                                                        string sOSClassPrevalence = "";
                                                        try
                                                        {
                                                            sOSClassPrevalence = nodeCWEOS.Attributes["Prevalence"].InnerText;
                                                        }
                                                        catch(Exception exOSClassPrevalence)
                                                        {
                                                            Console.WriteLine("DEBUG no Prevalence for OSClass for " + sCWEID);
                                                        }

                                                        //CWEOSCLASS oCWEOSClass = model.CWEOSCLASS.FirstOrDefault(o => o.CWEID == sCWEID && o.OSClassID == iOSClassID);
                                                        int iCWEOSClassID =0;
                                                        try
                                                        {
                                                            iCWEOSClassID=model.CWEOSCLASS.Where(o => o.CWEID == sCWEID && o.OSClassID == iOSClassID).Select(o => o.CWEOSClassID).FirstOrDefault();
                                                        }
                                                        catch(Exception ex)
                                                        {

                                                        }
                                                        //if (oCWEOSClass == null)
                                                        if (iCWEOSClassID <= 0)
                                                        {
                                                            CWEOSCLASS oCWEOSClass = new CWEOSCLASS();
                                                            oCWEOSClass.OSClassID = iOSClassID; // oOSClass.OSClassID;
                                                            oCWEOSClass.CWEID = sCWEID;
                                                            oCWEOSClass.Prevalence = sOSClassPrevalence;
                                                            oCWEOSClass.CreatedDate = DateTimeOffset.Now;
                                                            oCWEOSClass.timestamp = DateTimeOffset.Now;
                                                            oCWEOSClass.VocabularyID = iCWEVocabularyID;
                                                            model.CWEOSCLASS.Add(oCWEOSClass);
                                                            model.SaveChanges();
                                                        }
                                                        else
                                                        {
                                                            //Update CWEOSCLASS
                                                        }
                                                        break;

                                                    default:
                                                        Console.WriteLine("ERROR: Missing code for " + nodeCWEOS.Name);
                                                        break;
                                                }
                                            }
                                            break;
                                            #endregion operatingsystems

                                        default:
                                            Console.WriteLine("ERROR: Missing code for nodeCWEPlatform " + nodeCWEPlatform.Name);
                                            //Operating_Systems
                                            //Platform_Notes
                                            break;
                                    }
                                }
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
                                Console.WriteLine("Exception: DbEntityValidationExceptionexCWEPlatform " + sb.ToString());
                            }
                            catch (Exception exCWEPlatform)
                            {
                                Console.WriteLine("Exception: exCWEPlatform: " + exCWEPlatform.Message + " " + exCWEPlatform.InnerException);
                            }
                            break;
                        #endregion CWEApplicable_Platforms

                        case "Alternate_Terms":
                        #region CWEAlternate_Terms
                            foreach (XmlNode nodeCWEAlternateTerm in nodeCWEinfo)
                            {
                                Console.WriteLine("DEBUG: nodeCWEAlternateTerm");
                                CWEALTERNATETERM oCWEAlternateTerm=null;

                                foreach (XmlNode nodeTerm in nodeCWEAlternateTerm)
                                {
                                    switch (nodeTerm.Name)
                                    {
                                        case "Term":
                                            string sAlternateTerm = CleaningCWEString(nodeTerm.InnerText); //API Abuse
                                            //string sAlternateTermClean = CleaningCWEString(sAlternateTerm);
                                            Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                                            Console.WriteLine("DEBUG sAlternateTerm=" + sAlternateTerm);
                                            oCWEAlternateTerm = new CWEALTERNATETERM();
                                            oCWEAlternateTerm = model.CWEALTERNATETERM.Where(o => o.AlternateTerm == sAlternateTerm).FirstOrDefault();
                                            /*
                                            int iCWEAlternateTermID=0;
                                            try
                                            {
                                                iCWEAlternateTermID= model.CWEALTERNATETERM.FirstOrDefault(o => o.AlternateTerm == sAlternateTerm).CWEAlternateTermID;
                                            }
                                            catch(Exception ex)
                                            {

                                            }
                                            
                                            if(iCWEAlternateTermID>0)
                                            */
                                            if (oCWEAlternateTerm != null)
                                            {
                                                //Update CWEALTERNATETERM
                                                /*
                                                //Check if it is the same VocabularyID
                                                oCWEAlternateTerm.AlternateTermClean = sAlternateTermClean;
                                                oCWEAlternateTerm.timestamp = DateTimeOffset.Now;
                                                model.SaveChanges();
                                                */
                                            }
                                            else
                                            {
                                                //ALTERNATETERM
                                                oCWEAlternateTerm = new CWEALTERNATETERM();
                                                oCWEAlternateTerm.AlternateTerm = sAlternateTerm;
                                                //oCWEAlternateTerm.AlternateTermClean = sAlternateTermClean; //Removed
                                                oCWEAlternateTerm.CWEID = sCWEID;
                                                oCWEAlternateTerm.VocabularyID = iCWEVocabularyID;
                                                oCWEAlternateTerm.CreatedDate = DateTimeOffset.Now;
                                                oCWEAlternateTerm.timestamp = DateTimeOffset.Now;
                                                model.CWEALTERNATETERM.Add(oCWEAlternateTerm);
                                                model.SaveChanges();
                                                //iCWEAlternateTermID=oCWEAlternateTerm.CWEAlternateTermID;
                                                Console.WriteLine("DEBUG AddToCWEALTERNATETERM " + sAlternateTerm + " " + sCWEID);
                                            }
                                            break;

                                        case "Alternate_Term_Description":
                                            string sAlternateTermDefinition = CleaningCWEString(nodeTerm.InnerText);
                                            //Cleaning
                                            sAlternateTermDefinition = sAlternateTermDefinition.Replace("<Text>", "");
                                            sAlternateTermDefinition = sAlternateTermDefinition.Replace("</Text>", "");
                                            
                                            //Update CWEALTERNATETERM
                                            oCWEAlternateTerm.AlternateTermDescription = sAlternateTermDefinition;
                                            //model.SaveChanges();    //TEST PERFORMANCE
                                            break;

                                        default:
                                            Console.WriteLine("ERROR: Missing Code for nodeCWEAlternateTerm "+nodeTerm.Name);
                                            break;
                                    }

                                }
                            }
                            break;
                        #endregion CWEAlternate_Terms

                        
                        
                        case "Terminology_Notes":
                            string sTerminologyNotes = nodeCWEinfo.InnerText;
                            sTerminologyNotes = sTerminologyNotes.Replace("<Terminology_Note>", "");
                            sTerminologyNotes = sTerminologyNotes.Replace("</Terminology_Note>", "");
                            sTerminologyNotes = sTerminologyNotes.Replace("<Text>", "");
                            sTerminologyNotes = sTerminologyNotes.Replace("</Text>", "");
                            sTerminologyNotes = CleaningCWEString(sTerminologyNotes);
                            //Update CWE
                            CWEObject.Terminology_Notes = sTerminologyNotes.Trim();
                            //model.SaveChanges();    //TEST PERFORMANCE
                            break;
                        

                        case "Time_of_Introduction":
                        #region CWETime_Introduction
                            //TODO
                            foreach (XmlNode nodeCWEIntroductionPhase in nodeCWEinfo)
                            {
                                if (nodeCWEIntroductionPhase.Name == "Introductory_Phase")
                                {
                                    Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                                    Console.WriteLine("DEBUG nodeCWEIntroductionPhase");
                                    
                                    string sIntroductionPhase = nodeCWEIntroductionPhase.InnerText.Trim(); //Architecture and Design
                                    Console.WriteLine("DEBUG sIntroductionPhase=" + sIntroductionPhase);

                                    int iPhaseID=0;
                                    try
                                    {
                                        iPhaseID=model.PHASE.Where(o=>o.PhaseName==sIntroductionPhase).Select(o=>o.PhaseID).FirstOrDefault();
                                    }
                                    catch(Exception ex)
                                    {

                                    }
                                    if(iPhaseID<=0)
                                    {
                                        PHASE oPhase=new PHASE();
                                        oPhase.CreatedDate=DateTimeOffset.Now;
                                        oPhase.PhaseName=sIntroductionPhase;
                                        oPhase.VocabularyID=iVocabularyCWEID;
                                        oPhase.timestamp=DateTimeOffset.Now;
                                        model.PHASE.Add(oPhase);
                                        model.SaveChanges();
                                        iPhaseID=oPhase.PhaseID;
                                    }
                                    else
                                    {
                                        //Update PHASE
                                    }
                                    
                                    //Optimization because no update
                                    //CWETIMEOFINTRODUCTION oCWETimeOfIntroduction = model.CWETIMEOFINTRODUCTION.FirstOrDefault(o => o.IntroductoryPhase == sIntroductionPhase && o.CWEID == sCWEID);
                                    int iCWETimeOfIntroductionID =0;
                                    try
                                    {
                                        iCWETimeOfIntroductionID=model.CWETIMEOFINTRODUCTION.Where(o => o.IntroductoryPhase == sIntroductionPhase && o.CWEID == sCWEID).Select(o=>o.CWETimeOfIntroductionID).FirstOrDefault();
                                    }
                                    catch(Exception ex)
                                    {

                                    }
                                    //if (oCWETimeOfIntroduction != null)
                                    if (iCWETimeOfIntroductionID != 0)
                                    {
                                        //Update CWETIMEOFINTRODUCTION
                                        //TODO: Check if it is the same VocabularyID
                                        //Update if needed
                                        /*
                                        oCWETimeOfIntroduction.timestamp = DateTimeOffset.Now;
                                        try
                                        {
                                            model.SaveChanges();
                                        }
                                        catch (Exception exCWETIMEOFINTRODUCTIONChange)
                                        {
                                            Console.WriteLine("Exception: exCWETIMEOFINTRODUCTIONChange " + exCWETIMEOFINTRODUCTIONChange.Message + " " + exCWETIMEOFINTRODUCTIONChange.InnerException);
                                        }
                                        */
                                    }
                                    else
                                    {
                                        try
                                        {
                                            CWETIMEOFINTRODUCTION oCWETimeOfIntroduction = new CWETIMEOFINTRODUCTION();
                                            oCWETimeOfIntroduction.CreatedDate=DateTimeOffset.Now;
                                            oCWETimeOfIntroduction.IntroductoryPhase = sIntroductionPhase;
                                            oCWETimeOfIntroduction.CWEID = sCWEID;
                                            oCWETimeOfIntroduction.PhaseID=iPhaseID;
                                            oCWETimeOfIntroduction.VocabularyID = iCWEVocabularyID;
                                        
                                            oCWETimeOfIntroduction.timestamp = DateTimeOffset.Now;
                                            model.CWETIMEOFINTRODUCTION.Add(oCWETimeOfIntroduction);
                                            //model.SaveChanges();    //TEST PERFORMANCE
                                            //iCWETimeOfIntroductionID=
                                            Console.WriteLine("DEBUG: AddToCWETIMEOFINTRODUCTION " + sIntroductionPhase + " " + sCWEID);
                                        }
                                        catch (Exception exAddToCWETIMEOFINTRODUCTION)
                                        {
                                            Console.WriteLine("Exception: exAddToCWETIMEOFINTRODUCTION " + exAddToCWETIMEOFINTRODUCTION.Message + " " + exAddToCWETIMEOFINTRODUCTION.InnerException);
                                        }
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("ERROR: Missing code for " + nodeCWEIntroductionPhase.Name);
                                }
                            }
                            break;
                        #endregion CWETime_Introduction

                        case "Likelihood_of_Exploit":
                        #region CWELikelihood_of_Exploit
                            try
                            {
                                string sLikelihood = nodeCWEinfo.InnerText; //High to Very High
                                Console.WriteLine("DEBUG: sLikelihood=" + sLikelihood);
                                //EXPLOITLIKELIHOOD oExploitLikelihood = model.EXPLOITLIKELIHOOD.FirstOrDefault(o => o.Likelihood == sLikelihood);
                                int iExploitLikelihoodID = model.EXPLOITLIKELIHOOD.Where(o => o.Likelihood == sLikelihood).Select(o=>o.ExploitLikelihoodID).FirstOrDefault();
                                //if (oExploitLikelihood != null)
                                if (iExploitLikelihoodID != 0)
                                {
                                    //Update EXPLOITLIKELIHOOD
                                }
                                else
                                {
                                    
                                    try
                                    {
                                        EXPLOITLIKELIHOOD oExploitLikelihood = new EXPLOITLIKELIHOOD();
                                        oExploitLikelihood.CreatedDate = DateTimeOffset.Now;
                                        oExploitLikelihood.Likelihood = sLikelihood;
                                        //LikelihoodDescription
                                        oExploitLikelihood.VocabularyID = iCWEVocabularyID;
                                        oExploitLikelihood.timestamp = DateTimeOffset.Now;
                                        model.EXPLOITLIKELIHOOD.Add(oExploitLikelihood);
                                        model.SaveChanges();
                                        Console.WriteLine("DEBUG AddToEXPLOITLIKELIHOOD " + oExploitLikelihood);
                                        iExploitLikelihoodID = oExploitLikelihood.ExploitLikelihoodID;
                                    }
                                    catch (Exception exAddToEXPLOITLIKELIHOOD)
                                    {
                                        Console.WriteLine("Exception: AddToEXPLOITLIKELIHOOD " + exAddToEXPLOITLIKELIHOOD.Message + " " + exAddToEXPLOITLIKELIHOOD.InnerException);
                                    }
                                }

                                //EXPLOITLIKELIHOODFORCWE oExploitLikelihoodForCWE = model.EXPLOITLIKELIHOODFORCWE.FirstOrDefault(o => o.ExploitLikelihoodID == iExploitLikelihoodID && o.CWEID == sCWEID);
                                int iCWEExploitLikelihoodID = model.EXPLOITLIKELIHOODFORCWE.Where(o => o.ExploitLikelihoodID == iExploitLikelihoodID && o.CWEID == sCWEID).Select(o=>o.ExploitLikelihoodForCWEID).FirstOrDefault();
                                //if (oExploitLikelihoodForCWE != null)
                                if (iCWEExploitLikelihoodID != 0)
                                {
                                    //Update EXPLOITLIKELIHOODFORCWE
                                }
                                else
                                {
                                    try
                                    {
                                        EXPLOITLIKELIHOODFORCWE oExploitLikelihoodForCWE = new EXPLOITLIKELIHOODFORCWE();
                                        oExploitLikelihoodForCWE.CWEID = sCWEID;
                                        oExploitLikelihoodForCWE.ExploitLikelihoodID = iExploitLikelihoodID;    // oExploitLikelihood.ExploitLikelihoodID;
                                        
                                        oExploitLikelihoodForCWE.CreatedDate = DateTimeOffset.Now;
                                        oExploitLikelihoodForCWE.timestamp = DateTimeOffset.Now;
                                        oExploitLikelihoodForCWE.VocabularyID = iCWEVocabularyID;
                                        model.EXPLOITLIKELIHOODFORCWE.Add(oExploitLikelihoodForCWE);
                                        //model.SaveChanges();    //TEST PERFORMANCE
                                        //iCWEExploitLikelihoodID=
                                        //Console.WriteLine("DEBUG: AddToEXPLOITLIKELIHOODFORCWE " + sCWEID + " " + sLikelihood);
                                    }
                                    catch (Exception exAddToEXPLOITLIKELIHOODFORCWE)
                                    {
                                        Console.WriteLine("Exception: exAddToEXPLOITLIKELIHOODFORCWE " + exAddToEXPLOITLIKELIHOODFORCWE.Message + " " + exAddToEXPLOITLIKELIHOODFORCWE.InnerException);
                                    }
                                }
                            }
                            catch (Exception exCWELikelihood_of_Exploit)
                            {
                                Console.WriteLine("Exception: exCWELikelihood_of_Exploit " + exCWELikelihood_of_Exploit.Message + " " + exCWELikelihood_of_Exploit.InnerException);
                            }
                            break;
                        #endregion CWELikelihood_of_Exploit

                        //TODO
                        
                        case "Common_Consequences":
                        #region CWECommon_Consequences
                            //TODO: Review this because no "ID" for an ATTACKCONSEQUENCE, could be reclassified as CWECONSEQUENCE
                            //We will use the rank of the Common_Consequence in the CWE XML file as an ID
                            //ATTACKCONSEQUENCEFORCWE
                            ////ATTACKCONSEQUENCESCOPEFORCWE
                            int iRankConsequence = 0;
                            foreach (XmlNode nodeCommonConsequence in nodeCWEinfo)
                            {
                                iRankConsequence++;
                                Console.WriteLine("DEBUG nodeCommonConsequence "+iRankConsequence);
                                
                                //TODO: we will have a problem if more than 1 Common_Consequence
                                //ATTACKCONSEQUENCE oAttackConsequence = new ATTACKCONSEQUENCE(); //TODO: could be reclassified as CWECONSEQUENCE
                                //NOTE: Shared between CWE and CAPEC
                                //TODO: Review (order because no ID)  AttackPatternID
                                //Note: will be updated later with consequence_notes
                                CWEATTACKCONSEQUENCE oCWEAttackConsequence = model.CWEATTACKCONSEQUENCE.FirstOrDefault(o => o.CWEID == sCWEID && o.CWEAttackConsequenceOrder == iRankConsequence);
                                
                                if (oCWEAttackConsequence != null)
                                {
                                    ////TODO
                                    //oAttackConsequence = model.ATTACKCONSEQUENCE.Where(o => o.AttackConsequenceID == oCWEAttackConsequence.AttackConsequenceID).FirstOrDefault();

                                    //TODO
                                    //Update CWEATTACKCONSEQUENCE

                                }
                                else
                                {
                                    try
                                    {
                                        oCWEAttackConsequence = new CWEATTACKCONSEQUENCE();
                                        oCWEAttackConsequence.CWEID = sCWEID;
                                        //oCWEAttackConsequence.AttackConsequenceID = oAttackConsequence.AttackConsequenceID;
                                        oCWEAttackConsequence.CWEAttackConsequenceOrder = iRankConsequence;
                                        oCWEAttackConsequence.CreatedDate = DateTimeOffset.Now;
                                        oCWEAttackConsequence.timestamp = DateTimeOffset.Now;
                                        oCWEAttackConsequence.VocabularyID = iCWEVocabularyID;
                                        model.CWEATTACKCONSEQUENCE.Add(oCWEAttackConsequence);
                                        model.SaveChanges();
                                        Console.WriteLine("DEBUG AddToCWEAttackConsequence " + sCWEID);
                                    }
                                    catch (Exception exAddToCWEAttackConsequence)
                                    {
                                        Console.WriteLine("Exception: exAddToCWEAttackConsequence: " + exAddToCWEAttackConsequence.Message + " " + exAddToCWEAttackConsequence.InnerException);
                                    }
                                    //Console.WriteLine("DEBUG: AddToCWEAttackConsequence " + sCWEID + " " + oAttackConsequence.AttackConsequenceID);
                                }

                                //************************************************************************
                                foreach (XmlNode nodeCommonConsequenceInfo in nodeCommonConsequence)
                                {
                                    switch (nodeCommonConsequenceInfo.Name)
                                    {
                                        case "Consequence_Scope":
                                            string sConsequenceScope = CleaningCWEString(nodeCommonConsequenceInfo.InnerText);
                                            
                                            //ATTACKSCOPE oAttackConsequenceScope = model.ATTACKSCOPE.FirstOrDefault(o => o.ConsequenceScope == sConsequenceScope); //Note: CAPEC/CWE shared
                                            int iAttackScopeID=0;
                                            try
                                            {
                                                iAttackScopeID = attack_model.ATTACKSCOPE.Where(o => o.ConsequenceScope == sConsequenceScope).Select(o => o.AttackScopeID).FirstOrDefault(); //Note: CAPEC/CWE shared
                                            }
                                            catch(Exception ex)
                                            {

                                            }
                                            //if (oAttackConsequenceScope != null)
                                            if (iAttackScopeID != 0)
                                            {
                                                //Update ATTACKSCOPE

                                            }
                                            else
                                            {
                                                
                                                try
                                                {
                                                    ATTACKSCOPE oAttackConsequenceScope = new ATTACKSCOPE();
                                                    oAttackConsequenceScope.ConsequenceScope = sConsequenceScope;
                                                    oAttackConsequenceScope.CreatedDate = DateTimeOffset.Now;
                                                    oAttackConsequenceScope.timestamp = DateTimeOffset.Now;
                                                    oAttackConsequenceScope.VocabularyID = iCWEVocabularyID;
                                                    attack_model.ATTACKSCOPE.Add(oAttackConsequenceScope);
                                                    attack_model.SaveChanges();
                                                    //Console.WriteLine("DEBUG: AddToATTACKSCOPE " + sConsequenceScope);
                                                    iAttackScopeID = oAttackConsequenceScope.AttackScopeID;
                                                }
                                                catch (Exception exAddToATTACKSCOPE)
                                                {
                                                    Console.WriteLine("Exception: exAddToATTACKSCOPE " + exAddToATTACKSCOPE.Message + " " + exAddToATTACKSCOPE.InnerException);
                                                }
                                            }

                                            //CWEATTACKCONSEQUENCESCOPE oAttackConsequenceScopeForAttackConsequence = model.CWEATTACKCONSEQUENCESCOPE.FirstOrDefault(o => o.AttackScopeID == iAttackScopeID && o.CWEAttackConsequenceID == oCWEAttackConsequence.CWEAttackConsequenceID);
                                            int iCWEAttackScopeID =0;
                                            try
                                            {
                                                iCWEAttackScopeID=model.CWEATTACKCONSEQUENCESCOPE.Where(o => o.AttackScopeID == iAttackScopeID && o.CWEAttackConsequenceID == oCWEAttackConsequence.CWEAttackConsequenceID).Select(o=>o.CWEAttackConsequenceScopeID).FirstOrDefault();
                                            }
                                            catch(Exception ex)
                                            {

                                            }
                                            //if (oAttackConsequenceScopeForAttackConsequence != null)
                                            if (iCWEAttackScopeID > 0)
                                            {
                                                //Update CWEATTACKCONSEQUENCESCOPE
                                            }
                                            else
                                            {
                                                
                                                try
                                                {
                                                    CWEATTACKCONSEQUENCESCOPE oAttackConsequenceScopeForAttackConsequence = new CWEATTACKCONSEQUENCESCOPE();
                                                    oAttackConsequenceScopeForAttackConsequence.AttackScopeID = iAttackScopeID; // oAttackConsequenceScope.AttackScopeID;
                                                    oAttackConsequenceScopeForAttackConsequence.CWEAttackConsequenceID = oCWEAttackConsequence.CWEAttackConsequenceID;
                                                    oAttackConsequenceScopeForAttackConsequence.CreatedDate = DateTimeOffset.Now;
                                                    oAttackConsequenceScopeForAttackConsequence.timestamp = DateTimeOffset.Now;
                                                    oAttackConsequenceScopeForAttackConsequence.VocabularyID = iCWEVocabularyID;
                                                    model.CWEATTACKCONSEQUENCESCOPE.Add(oAttackConsequenceScopeForAttackConsequence);
                                                    //model.SaveChanges();    //TEST PERFORMANCE
                                                    //iCWEAttackScopeID=
                                                    //Console.WriteLine("DEBUG AddToCWEATTACKCONSEQUENCESCOPE " + sCWEID + " " + sConsequenceScope);
                                                }
                                                catch (Exception exAddToCWEATTACKCONSEQUENCESCOPE)
                                                {
                                                    Console.WriteLine("Exception: exAddToCWEATTACKCONSEQUENCESCOPE " + exAddToCWEATTACKCONSEQUENCESCOPE.Message + " " + exAddToCWEATTACKCONSEQUENCESCOPE.InnerException);
                                                }
                                            }
                                            break;

                                        case "Consequence_Technical_Impact":
                                            //NOTE: Also in CAPEC
                                            string sTechnicalImpact = CleaningCWEString(nodeCommonConsequenceInfo.InnerText);
                                            
                                            //ATTACKTECHNICALIMPACT oAttackTechnicalImpact = model.ATTACKTECHNICALIMPACT.FirstOrDefault(o => o.ConsequenceTechnicalImpact == sTechnicalImpact);    //Note: CAPEC/CWE shared
                                            int iAttackTechnicalImpactID=0;
                                            try
                                            {
                                                iAttackTechnicalImpactID = attack_model.ATTACKTECHNICALIMPACT.Where(o => o.ConsequenceTechnicalImpact == sTechnicalImpact).Select(o => o.AttackTechnicalImpactID).FirstOrDefault();    //Note: CAPEC/CWE shared
                                            }
                                            catch(Exception ex)
                                            {

                                            }
                                            //if (oAttackTechnicalImpact != null)
                                            if (iAttackTechnicalImpactID != 0)
                                            {
                                                //Update ATTACKTECHNICALIMPACT
                                            }
                                            else
                                            {
                                                
                                                try
                                                {
                                                    ATTACKTECHNICALIMPACT oAttackTechnicalImpact = new ATTACKTECHNICALIMPACT();
                                                    oAttackTechnicalImpact.ConsequenceTechnicalImpact = sTechnicalImpact;
                                                    //oAttackTechnicalImpact.ConsequenceTechnicalImpactRaw = nodeCommonConsequenceInfo.InnerText;
                                                    oAttackTechnicalImpact.CreatedDate = DateTimeOffset.Now;
                                                    oAttackTechnicalImpact.timestamp = DateTimeOffset.Now;
                                                    oAttackTechnicalImpact.VocabularyID = iCWEVocabularyID;
                                                    attack_model.ATTACKTECHNICALIMPACT.Add(oAttackTechnicalImpact);
                                                    attack_model.SaveChanges();
                                                    iAttackTechnicalImpactID = oAttackTechnicalImpact.AttackTechnicalImpactID;
                                                }
                                                catch (Exception exAddToATTACKTECHNICALIMPACT)
                                                {
                                                    Console.WriteLine("Exception: exAddToATTACKTECHNICALIMPACT " + exAddToATTACKTECHNICALIMPACT.Message + " " + exAddToATTACKTECHNICALIMPACT.InnerException);
                                                }
                                                Console.WriteLine("DEBUG AddToATTACKTECHNICALIMPACT " + sTechnicalImpact);
                                            }

                                            //CWEATTACKTECHNICALIMPACT oAttackTechnicalImpactForAttackConsequence = model.CWEATTACKTECHNICALIMPACT.FirstOrDefault(o => o.CWEAttackConsequenceID == oCWEAttackConsequence.CWEAttackConsequenceID && o.AttackTechnicalImpactID == iAttackTechnicalImpactID);
                                            int iCWEAttackTechnicalImpactID=0;
                                            try
                                            {
                                                iCWEAttackTechnicalImpactID = model.CWEATTACKTECHNICALIMPACT.Where(o => o.CWEAttackConsequenceID == oCWEAttackConsequence.CWEAttackConsequenceID && o.AttackTechnicalImpactID == iAttackTechnicalImpactID).Select(o => o.CWEAttackTechnicalImpactID).FirstOrDefault();
                                            }
                                            catch(Exception ex)
                                            {

                                            }
                                            //if (oAttackTechnicalImpactForAttackConsequence != null)
                                            if (iCWEAttackTechnicalImpactID > 0)
                                            {
                                                //Update CWEATTACKTECHNICALIMPACT
                                            }
                                            else
                                            {
                                                
                                                try
                                                {
                                                    CWEATTACKTECHNICALIMPACT oAttackTechnicalImpactForAttackConsequence = new CWEATTACKTECHNICALIMPACT();
                                                    oAttackTechnicalImpactForAttackConsequence.CWEAttackConsequenceID = oCWEAttackConsequence.CWEAttackConsequenceID;
                                                    oAttackTechnicalImpactForAttackConsequence.AttackTechnicalImpactID = iAttackTechnicalImpactID;  // oAttackTechnicalImpact.AttackTechnicalImpactID;
                                                    oAttackTechnicalImpactForAttackConsequence.CreatedDate = DateTimeOffset.Now;
                                                    oAttackTechnicalImpactForAttackConsequence.timestamp = DateTimeOffset.Now;
                                                    oAttackTechnicalImpactForAttackConsequence.VocabularyID = iCWEVocabularyID;
                                                    model.CWEATTACKTECHNICALIMPACT.Add(oAttackTechnicalImpactForAttackConsequence);
                                                    //model.SaveChanges();    //TEST PERFORMANCE
                                                    //Console.WriteLine("DEBUG: AddToCWEATTACKTECHNICALIMPACT " + sCWEID + " " + sTechnicalImpact);
                                                    //iCWEAttackTechnicalImpactID = oAttackTechnicalImpactForAttackConsequence.CWEAttackTechnicalImpactID;
                                                }
                                                catch (Exception exAddToCWEATTACKTECHNICALIMPACT)
                                                {
                                                    Console.WriteLine("Exception: exAddToCWEATTACKTECHNICALIMPACT " + exAddToCWEATTACKTECHNICALIMPACT.Message + " " + exAddToCWEATTACKTECHNICALIMPACT.InnerException);
                                                }
                                            }
                                            break;
                                        case "Consequence_Note":
                                            string sAttackConsequenceNoteText = string.Empty;
                                            //try
                                            //{
                                                sAttackConsequenceNoteText=nodeCommonConsequenceInfo.InnerText;
                                                sAttackConsequenceNoteText = sAttackConsequenceNoteText.Replace("<Text>","");
                                                sAttackConsequenceNoteText = sAttackConsequenceNoteText.Replace("</Text>", "");
                                                sAttackConsequenceNoteText = CleaningCWEString(sAttackConsequenceNoteText);
                                                //Update CWEATTACKCONSEQUENCE
                                                oCWEAttackConsequence.Consequence_Note = sAttackConsequenceNoteText;
                                                oCWEAttackConsequence.timestamp = DateTimeOffset.Now;
                                                //model.SaveChanges();    //TEST PERFORMANCE

                                                //OLD CODE  TODO: Remove?
                                                /*
                                                //TODO: VocabularyID?
                                                ATTACKCONSEQUENCENOTE oAttackConsequenceNote = model.ATTACKCONSEQUENCENOTE.Where(o => o.VocabularyID == iCWEVocabularyID && o.AttackConsequenceNoteText == sAttackConsequenceNoteText).FirstOrDefault();
                                                if (oAttackConsequenceNote != null)
                                                {
                                                    oAttackConsequenceNote.timestamp = DateTimeOffset.Now;
                                                    //Console.WriteLine("DEBUG ATTACKCONSEQUENCENOTE " + oAttackConsequenceNote.AttackConsequenceNoteID+" "+ sAttackConsequenceNoteText);
                                                }
                                                else
                                                {
                                                    oAttackConsequenceNote = new ATTACKCONSEQUENCENOTE();
                                                    oAttackConsequenceNote.AttackConsequenceNoteText = sAttackConsequenceNoteText;
                                                    oAttackConsequenceNote.CreatedDate = DateTimeOffset.Now;
                                                    oAttackConsequenceNote.timestamp = DateTimeOffset.Now;
                                                    oAttackConsequenceNote.VocabularyID = iCWEVocabularyID;
                                                    try
                                                    {
                                                        model.AddToATTACKCONSEQUENCENOTE(oAttackConsequenceNote);
                                                        model.SaveChanges();
                                                        Console.WriteLine("DEBUG AddToATTACKCONSEQUENCENOTE: " + sAttackConsequenceNoteText);
                                                    }
                                                    catch (Exception exAddToATTACKCONSEQUENCENOTE)
                                                    {
                                                        Console.WriteLine("Exception: exAddToATTACKCONSEQUENCENOTE " + exAddToATTACKCONSEQUENCENOTE.Message + " " + exAddToATTACKCONSEQUENCENOTE.InnerException);
                                                    }
                                                    
                                                }
                                                ATTACKCONSEQUENCENOTES oAttackConsequenceNotes = model.ATTACKCONSEQUENCENOTES.Where(o => o.AttackConsequenceID == oAttackConsequence.AttackConsequenceID && o.AttackConsequenceNoteID == oAttackConsequenceNote.AttackConsequenceNoteID).FirstOrDefault();
                                                if (oAttackConsequenceNotes != null)
                                                {
                                                    oAttackConsequenceNotes.timestamp = DateTimeOffset.Now;

                                                }
                                                else
                                                {
                                                    oAttackConsequenceNotes = new ATTACKCONSEQUENCENOTES();
                                                    oAttackConsequenceNotes.AttackConsequenceID = oAttackConsequence.AttackConsequenceID;
                                                    oAttackConsequenceNotes.AttackConsequenceNoteID = oAttackConsequenceNote.AttackConsequenceNoteID;
                                                    oAttackConsequenceNotes.CreatedDate = DateTimeOffset.Now;
                                                    oAttackConsequenceNotes.timestamp = DateTimeOffset.Now;
                                                    oAttackConsequenceNotes.VocabularyID = iCWEVocabularyID;
                                                    try
                                                    {
                                                        model.AddToATTACKCONSEQUENCENOTES(oAttackConsequenceNotes);
                                                        model.SaveChanges();
                                                        Console.WriteLine("DEBUG AddToATTACKCONSEQUENCENOTES");
                                                    }
                                                    catch (Exception exAddToATTACKCONSEQUENCENOTES)
                                                    {
                                                        Console.WriteLine("Exception: exAddToATTACKCONSEQUENCENOTES " + exAddToATTACKCONSEQUENCENOTES.Message + " " + exAddToATTACKCONSEQUENCENOTES.InnerException);
                                                    }
                                                    
                                                }
                                                */

                                            break;
                                        default:
                                            Console.WriteLine("ERROR: Missing code for nodeCommonConsequenceInfo: " + nodeCommonConsequenceInfo.Name);
                                            break;
                                    }
                                }

                            }
                            break;
                        #endregion CWECommon_Consequences
                        
                        
                        case "Detection_Methods":
                            #region detectionmethod
                            //TODO
                            //CWEDETECTIONMETHOD
                            foreach (XmlNode nodeCWEDetectionMethod in nodeCWEinfo)
                            {
                                if (nodeCWEDetectionMethod.Name != "Detection_Method")
                                {
                                    Console.WriteLine("ERROR: Missing code for " + nodeCWEDetectionMethod.Name);
                                }
                                else
                                {
                                    string sDetectionMethodID = string.Empty;
                                    try
                                    {
                                        sDetectionMethodID = nodeCWEDetectionMethod.Attributes["Detection_Method_ID"].InnerText; //DM-7
                                    }
                                    catch (Exception ex)
                                    {
                                        string sIgnoreWarning = ex.Message;
                                    }
                                    DETECTIONMETHOD oDetetectionMethod = new DETECTIONMETHOD();
                                    CWEDETECTIONMETHOD oCWEDetectionMethod = new CWEDETECTIONMETHOD();
                                    foreach (XmlNode nodeCWEDetectionMethodInfo in nodeCWEDetectionMethod)
                                    {
                                        switch (nodeCWEDetectionMethodInfo.Name)
                                        {
                                            case "Method_Name":
                                                string sDetectionMethodName = CleaningCWEString(nodeCWEDetectionMethodInfo.InnerText);  //Manual Static Analysis
                                                oDetetectionMethod = model.DETECTIONMETHOD.FirstOrDefault(o => o.DetectionMethodName == sDetectionMethodName);   //TODO: VocabularyID?
                                                if (oDetetectionMethod != null)
                                                {
                                                    //Update DETECTIONMETHOD
                                                    //TODO: update if needed

                                                }
                                                else
                                                {
                                                    oDetetectionMethod = new DETECTIONMETHOD();
                                                    oDetetectionMethod.DetectionMethodName = sDetectionMethodName;
                                                    oDetetectionMethod.VocabularyID = iCWEVocabularyID;
                                                    oDetetectionMethod.DetectionMethodVocabularyID = sDetectionMethodID;
                                                    //TODO: CreatedDate... Effectiveness?
                                                    oDetetectionMethod.timestamp = DateTimeOffset.Now;
                                                    //TODO: Try Catch...
                                                    model.DETECTIONMETHOD.Add(oDetetectionMethod);
                                                    model.SaveChanges();
                                                }

                                                oCWEDetectionMethod = model.CWEDETECTIONMETHOD.FirstOrDefault(o => o.CWEID == sCWEID && o.DetectionMethodID == oDetetectionMethod.DetectionMethodID);
                                                if (oCWEDetectionMethod != null)
                                                {
                                                    //Update CWEDETECTIONMETHOD
                                                    //TODO: update if needed
                                                }
                                                else
                                                {
                                                    oCWEDetectionMethod = new CWEDETECTIONMETHOD();
                                                    oCWEDetectionMethod.CWEID = sCWEID;
                                                    oCWEDetectionMethod.DetectionMethodID = oDetetectionMethod.DetectionMethodID;
                                                    oCWEDetectionMethod.CreatedDate = DateTimeOffset.Now;
                                                    oCWEDetectionMethod.timestamp = DateTimeOffset.Now;
                                                    oCWEDetectionMethod.VocabularyID = iCWEVocabularyID;
                                                    //TODO: Try Catch...
                                                    model.CWEDETECTIONMETHOD.Add(oCWEDetectionMethod);
                                                    model.SaveChanges();
                                                }

                                                break;
                                            case "Method_Description":
                                                string sDetectionMethodDescription = CleaningCWEString(nodeCWEDetectionMethodInfo.InnerText);
                                                
                                                //TODO: Review this
                                                sDetectionMethodDescription = sDetectionMethodDescription.Replace("<text>","");
                                                sDetectionMethodDescription = sDetectionMethodDescription.Replace("</text>", "");
                                                sDetectionMethodDescription = sDetectionMethodDescription.Replace("<Text>","");
                                                sDetectionMethodDescription = sDetectionMethodDescription.Replace("</Text>", "");
                                                /*
                                                //Remove CLRF
                                                sDetectionMethodDescription = sDetectionMethodDescription.Replace("\r\n", " ");
                                                sDetectionMethodDescription = sDetectionMethodDescription.Replace("\n", " ");
                                                while (sDetectionMethodDescription.Contains("  "))
                                                {
                                                    sDetectionMethodDescription = sDetectionMethodDescription.Replace("  ", " ");
                                                }
                                                */
                                                ////oDetetectionMethod.DetectionMethodDescription = sDetectionMethodDescription;
                                                ////oDetetectionMethod.timestamp = DateTimeOffset.Now;

                                                //Update CWEDETECTIONMETHOD
                                                oCWEDetectionMethod.CWEDetectionMethodDescription = sDetectionMethodDescription.Trim();
                                                oCWEDetectionMethod.timestamp = DateTimeOffset.Now;
                                                //TODO: Try Catch
                                                model.SaveChanges();

                                                break;

                                            case "Method_Effectiveness":
                                                //Update CWEDETECTIONMETHOD
                                                //TODO table EFFECTIVENESS?
                                                oCWEDetectionMethod.CWEDetectionMethodEffectiveness = CleaningCWEString(nodeCWEDetectionMethodInfo.InnerText);  //Moderate
                                                oCWEDetectionMethod.timestamp = DateTimeOffset.Now;
                                                //TODO: Try Catch
                                                model.SaveChanges();
                                                break;
                                            case "Method_Effectiveness_Notes":
                                                string sMethodEffectivenessNotes = CleaningCWEString(nodeCWEDetectionMethodInfo.InnerText);
                                                
                                                //TODO: Review this
                                                sMethodEffectivenessNotes = sMethodEffectivenessNotes.Replace("<text>", "");
                                                sMethodEffectivenessNotes = sMethodEffectivenessNotes.Replace("</text>", "");
                                                sMethodEffectivenessNotes = sMethodEffectivenessNotes.Replace("<Text>", "");
                                                sMethodEffectivenessNotes = sMethodEffectivenessNotes.Replace("</Text>", "");
                                                /*
                                                //Remove CLRF
                                                sMethodEffectivenessNotes = sMethodEffectivenessNotes.Replace("\r\n", " ");
                                                sMethodEffectivenessNotes = sMethodEffectivenessNotes.Replace("\n", " ");
                                                while (sMethodEffectivenessNotes.Contains("  "))
                                                {
                                                    sMethodEffectivenessNotes = sMethodEffectivenessNotes.Replace("  ", " ");
                                                }
                                                */
                                                //Update CWEDETECTIONMETHOD
                                                oCWEDetectionMethod.CWEDetectionMethodEffectivenessNotes = sMethodEffectivenessNotes.Trim();
                                                oCWEDetectionMethod.timestamp = DateTimeOffset.Now;
                                                //TODO: Try Catch
                                                model.SaveChanges();
                                                break;
                                            default:
                                                Console.WriteLine("ERROR: Missing code for " + nodeCWEDetectionMethodInfo.Name);
                                                break;
                                        }
                                    }
                                }
                            }
                            break;
                            #endregion detectionmethod
                        
                        case "Potential_Mitigations":
                            //TODO:Review this
                            #region CWEMitigation
                            int iCWEMitigationCount = 0;
                            //MITIGATIONSTRATEGYFORMITIGATION
                            foreach (XmlNode nodeCWEMitigation in nodeCWEinfo)
                            {
                                if (nodeCWEMitigation.Name != "Mitigation")
                                {
                                    Console.WriteLine("ERROR: Missing code for Mitigation " + nodeCWEMitigation.Name);

                                }
                                else
                                {
                                    iCWEMitigationCount++;
                                    //MITIGATIONFORCWE oCWEMitigation;
                                    string sCWEMitigationPhaseName = string.Empty;
                                    string sCWEMitigationStrategyName = string.Empty;
                                    string sMitigationID = "";
                                    string sEffectiveness = "";
                                    try
                                    {
                                        sMitigationID = nodeCWEMitigation.Attributes["Mitigation_ID"].InnerText;
                                    }
                                    catch (Exception exsMitigationID)
                                    {
                                        Console.WriteLine("DEBUG Note: no Mitigation_ID for " + sCWEID);
                                        sMitigationID = "JA-"+iCWEMitigationCount.ToString();   //TODO REVIEW (needs a CWE update)
                                    }
                                    //TODO
                                    
                                    MITIGATIONFORCWE oCWEMitigation=null;
                                    MITIGATION oMitigation = null;
                                    try
                                    {
                                        //sMitigationID = nodeCWEMitigation.Attributes["Mitigation_ID"].InnerText;
                                        //TODO: VocabularyID?
                                        oCWEMitigation = model.MITIGATIONFORCWE.FirstOrDefault(o => o.CWEID==sCWEID && o.VocabularyID == iCWEVocabularyID && o.MitigationVocabularyID == sMitigationID);
                                        if (oCWEMitigation == null)
                                        {
                                            try
                                            {
                                                oCWEMitigation = new MITIGATIONFORCWE();
                                                oCWEMitigation.CWEID = sCWEID;
                                                oCWEMitigation.MitigationID = iDefaultMitigationID;    //(we don't have the correct MitigationID yet)
                                                //TODO
                                                oCWEMitigation.VocabularyID = iCWEVocabularyID;
                                                oCWEMitigation.MitigationVocabularyID = sMitigationID;
                                                oCWEMitigation.CreatedDate = DateTimeOffset.Now;
                                                oCWEMitigation.timestamp = DateTimeOffset.Now;
                                                model.MITIGATIONFORCWE.Add(oCWEMitigation);
                                                model.SaveChanges();
                                            }
                                            catch (Exception exMITIGATIONFORCWE)
                                            {
                                                Console.WriteLine("Exception: exMITIGATIONFORCWE " + exMITIGATIONFORCWE.Message + " " + exMITIGATIONFORCWE.InnerException);
                                            }
                                        }
                                        else
                                        {
                                            //Update MITIGATIONFORCWE
                                        }
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                    

                                    //TODO: Review this as we could have multiple ones for the same Mitigation, not perfect
                                    //MITIGATIONPHASE oMitigationPhase = null;
                                    int iMitigationPhaseID = 0;
                                    MITIGATIONPHASEFORMITIGATION oMitigationMitigationPhase = null;
                                    //MITIGATIONSTRATEGY oMitigationStrategy = null;
                                    int iMitigationStrategyID = 0;
                                    MITIGATIONSTRATEGYFORMITIGATION oMitigationMitigationStrategy = null;
                                    //EFFECTIVENESS oEffectiveness = null;
                                    int iEffectivenessID = 0;
                                    string sMitigationEffectivenessNotes = "";
                                    foreach (XmlNode nodeCWEMitigationInfo in nodeCWEMitigation)
                                    {
                                        Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                                        Console.WriteLine("DEBUG nodeCWEMitigationInfo " + nodeCWEMitigationInfo.Name);
                                        if (nodeCWEMitigationInfo.Name != "Mitigation_Description")
                                        {
                                            switch(nodeCWEMitigationInfo.Name)
                                            {
                                                case "Mitigation_Phase":
                                                    #region mitigationphase
                                                    sCWEMitigationPhaseName=CleaningCWEString(nodeCWEMitigationInfo.InnerText);
                                                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                    Console.WriteLine("DEBUG sCWEMitigationPhaseName=" + sCWEMitigationPhaseName);
                                                    int iPhaseID = 0;
                                                    try
                                                    {
                                                        iPhaseID = model.PHASE.Where(o => o.PhaseName == sCWEMitigationPhaseName).Select(o=>o.PhaseID).FirstOrDefault();
                                                    }
                                                    catch(Exception ex)
                                                    {
                                                        iPhaseID = 0;
                                                    }
                                                    if (iPhaseID <= 0)
                                                    {
                                                        try
                                                        {
                                                            PHASE oPhase = new PHASE();
                                                            oPhase.CreatedDate = DateTimeOffset.Now;
                                                            oPhase.PhaseName = sCWEMitigationPhaseName;
                                                            oPhase.VocabularyID = iCWEVocabularyID;
                                                            oPhase.timestamp = DateTimeOffset.Now;
                                                            model.PHASE.Add(oPhase);
                                                            model.SaveChanges();
                                                            iPhaseID = oPhase.PhaseID;
                                                        }
                                                        catch (Exception exPHASE)
                                                        {
                                                            Console.WriteLine("Exception: exPHASE " + exPHASE.Message + " " + exPHASE.InnerException);
                                                        }
                                                    }

                                                    //oMitigationPhase = model.MITIGATIONPHASE.Where(o => o.MitigationPhaseName == sCWEMitigationPhaseName).FirstOrDefault();
                                                    try
                                                    {
                                                        iMitigationPhaseID = model.MITIGATIONPHASE.Where(o => o.MitigationPhaseName == sCWEMitigationPhaseName).Select(o=>o.MitigationPhaseID).FirstOrDefault();
                                                    }
                                                    catch(Exception ex)
                                                    {
                                                        iMitigationPhaseID = 0;
                                                    }
                                                    //if (oMitigationPhase != null)
                                                    if (iMitigationPhaseID>0)
                                                    {
                                                        //Update MITIGATIONPHASE
                                                        // REMOVE
                                                        //oMitigationPhase.PhaseID = iPhaseID;
                                                        //model.SaveChanges();
                                                    }
                                                    else
                                                    {
                                                        try
                                                        {
                                                            MITIGATIONPHASE oMitigationPhase = new MITIGATIONPHASE();
                                                            oMitigationPhase.CreatedDate = DateTimeOffset.Now;
                                                            oMitigationPhase.PhaseID = iPhaseID;
                                                            oMitigationPhase.MitigationPhaseName = sCWEMitigationPhaseName;
                                                            oMitigationPhase.VocabularyID = iCWEVocabularyID;
                                                            oMitigationPhase.timestamp = DateTimeOffset.Now;
                                                            model.MITIGATIONPHASE.Add(oMitigationPhase);
                                                            model.SaveChanges();
                                                            iMitigationPhaseID = oMitigationPhase.MitigationPhaseID;
                                                        }
                                                        catch(Exception exMITIGATIONPHASE)
                                                        {
                                                            Console.WriteLine("Exception: exMITIGATIONPHASE " + exMITIGATIONPHASE.Message + " " + exMITIGATIONPHASE.InnerException);
                                                        }
                                                        Console.WriteLine("DEBUG AddToMITIGATIONPHASE: " + sCWEMitigationPhaseName);
                                                    }

                                                    if (oMitigation != null)
                                                    {
                                                        /*
                                                        int iMitigationMitigationPhaseID = 0;
                                                        try
                                                        {
                                                            iMitigationMitigationPhaseID = model.MITIGATIONPHASEFORMITIGATION.FirstOrDefault(o => o.MitigationID == oMitigation.MitigationID && o.MitigationPhaseID == oMitigationPhase.MitigationPhaseID).MitigationMitigationPhaseID;
                                                        }
                                                        catch(Exception ex)
                                                        {

                                                        }
                                                        */

                                                        oMitigationMitigationPhase = model.MITIGATIONPHASEFORMITIGATION.FirstOrDefault(o => o.MitigationID == oMitigation.MitigationID && o.MitigationPhaseID == iMitigationPhaseID);
                                                        //if (iMitigationMitigationPhaseID<=0)
                                                        if (oMitigationMitigationPhase == null)
                                                        {
                                                            try
                                                            {
                                                                oMitigationMitigationPhase = new MITIGATIONPHASEFORMITIGATION();
                                                                oMitigationMitigationPhase.CreatedDate = DateTimeOffset.Now;
                                                                oMitigationMitigationPhase.MitigationID = iDefaultMitigationID;    //(we don't have the correct MitigationID yet)   oMitigation.MitigationID;
                                                                oMitigationMitigationPhase.MitigationPhaseID = iMitigationPhaseID;  // oMitigationPhase.MitigationPhaseID;
                                                                oMitigationMitigationPhase.VocabularyID = iCWEVocabularyID;
                                                                oMitigationMitigationPhase.timestamp = DateTimeOffset.Now;
                                                                model.MITIGATIONPHASEFORMITIGATION.Add(oMitigationMitigationPhase);
                                                                model.SaveChanges();

                                                            }
                                                            catch (Exception exMITIGATIONPHASEFORMITIGATION)
                                                            {
                                                                Console.WriteLine("Exception: exMITIGATIONPHASEFORMITIGATION " + exMITIGATIONPHASEFORMITIGATION.Message + " " + exMITIGATIONPHASEFORMITIGATION.InnerException);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            //Update MITIGATIONPHASEFORMITIGATION
                                                        }
                                                    }

                                                    #endregion mitigationphase
                                                    break;

                                                //TODO
                                                case "Mitigation_Strategy":
                                                    #region mitigationstrategy
                                                    sCWEMitigationStrategyName = CleaningCWEString(nodeCWEMitigationInfo.InnerText);
                                                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                    Console.WriteLine("DEBUG sCWEMitigationStrategyName=" + sCWEMitigationStrategyName);
                                                    //oMitigationStrategy = model.MITIGATIONSTRATEGY.Where(o => o.MitigationStrategyName == sCWEMitigationStrategyName).FirstOrDefault();
                                                    try
                                                    {
                                                        iMitigationStrategyID = model.MITIGATIONSTRATEGY.Where(o => o.MitigationStrategyName == sCWEMitigationStrategyName).Select(o=>o.MitigationStrategyID).FirstOrDefault();
                                                    }
                                                    catch(Exception ex)
                                                    {
                                                        iMitigationStrategyID = 0;
                                                    }
                                                    //if (oMitigationStrategy != null)
                                                    if (iMitigationStrategyID>0)
                                                    {
                                                        //Update MITIGATIONSTRATEGY
                                                    }
                                                    else
                                                    {
                                                        try
                                                        {
                                                            MITIGATIONSTRATEGY oMitigationStrategy = new MITIGATIONSTRATEGY();
                                                            oMitigationStrategy.MitigationStrategyName = sCWEMitigationStrategyName;
                                                            oMitigationStrategy.VocabularyID = iCWEVocabularyID;
                                                            oMitigationStrategy.CreatedDate = DateTimeOffset.Now;
                                                            oMitigationStrategy.timestamp = DateTimeOffset.Now;
                                                            model.MITIGATIONSTRATEGY.Add(oMitigationStrategy);
                                                            model.SaveChanges();
                                                            iMitigationStrategyID = oMitigationStrategy.MitigationStrategyID;
                                                            Console.WriteLine("DEBUG AddToMITIGATIONSTRATEGY: " + sCWEMitigationStrategyName);
                                                        }
                                                        catch (Exception exMITIGATIONSTRATEGY)
                                                        {
                                                            Console.WriteLine("Exception: exMITIGATIONSTRATEGY " + exMITIGATIONSTRATEGY.Message + " " + exMITIGATIONSTRATEGY.InnerException);
                                                        }
                                                    }

                                                    try
                                                    {
                                                        //UPDATE MITIGATIONFORCWE
                                                        //oCWEMitigation.Mitigation_Strategy = sCWEMitigationStrategyName;    //Removed
                                                        oCWEMitigation.MitigationStrategyID = iMitigationStrategyID;
                                                        oCWEMitigation.timestamp = DateTimeOffset.Now;
                                                        model.SaveChanges();
                                                    }
                                                    catch (Exception exMitigation_Strategy)
                                                    {
                                                        Console.WriteLine("Exception: exMitigation_Strategy " + exMitigation_Strategy.Message + " " + exMitigation_Strategy.InnerException);
                                                    }

                                                    if (oMitigation != null)
                                                    {
                                                        oMitigationMitigationStrategy = model.MITIGATIONSTRATEGYFORMITIGATION.FirstOrDefault(o => o.MitigationID == oMitigation.MitigationID && o.MitigationStrategyID == iMitigationStrategyID);
                                                        /*
                                                        int iMitigationMitigationStrategyID = 0;
                                                        try
                                                        {
                                                            iMitigationMitigationStrategyID = model.MITIGATIONSTRATEGYFORMITIGATION.FirstOrDefault(o => o.MitigationID == oMitigation.MitigationID && o.MitigationStrategyID == oMitigationStrategy.MitigationStrategyID).MitigationMitigationStrategyID;
                                                        }
                                                        catch(Exception ex)
                                                        {

                                                        }
                                                        if (iMitigationMitigationStrategyID<=0)
                                                        */
                                                        if (oMitigationMitigationStrategy == null)
                                                        {
                                                            try
                                                            {
                                                                oMitigationMitigationStrategy = new MITIGATIONSTRATEGYFORMITIGATION();
                                                                oMitigationMitigationStrategy.CreatedDate = DateTimeOffset.Now;
                                                                //NOTE (we don't have the correct MitigationID yet)
                                                                oMitigationMitigationStrategy.MitigationID = iDefaultMitigationID;    //    oMitigation.MitigationID;
                                                                oMitigationMitigationStrategy.MitigationStrategyID = iMitigationStrategyID; // oMitigationStrategy.MitigationStrategyID;
                                                                oMitigationMitigationStrategy.VocabularyID = iCWEVocabularyID;
                                                                oMitigationMitigationStrategy.timestamp = DateTimeOffset.Now;
                                                                model.MITIGATIONSTRATEGYFORMITIGATION.Add(oMitigationMitigationStrategy);
                                                                model.SaveChanges();

                                                            }
                                                            catch (Exception exMITIGATIONSTRATEGYFORMITIGATION)
                                                            {
                                                                Console.WriteLine("Exception: exMITIGATIONSTRATEGYFORMITIGATION " + exMITIGATIONSTRATEGYFORMITIGATION.Message + " " + exMITIGATIONSTRATEGYFORMITIGATION.InnerException);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            //Update MITIGATIONSTRATEGYFORMITIGATION
                                                        }
                                                    }
                                                    
                                                    #endregion mitigationstrategy
                                                    break;
                                             
                                                case "Mitigation_Effectiveness":
                                                    #region mitigationeffectiveness
                                                    //MITIGATIONEFFECTIVENESS
                                                    sEffectiveness=CleaningCWEString(nodeCWEMitigationInfo.InnerText);
                                                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                    Console.WriteLine("DEBUG sEffectiveness=" + sEffectiveness);
                                                    //oEffectiveness = model.EFFECTIVENESS.FirstOrDefault(o => o.EffectivenessName == sEffectiveness);
                                                    try
                                                    {
                                                        iEffectivenessID = model.EFFECTIVENESS.Where(o => o.EffectivenessName == sEffectiveness).Select(o=>o.EffectivenessID).FirstOrDefault();
                                                    }
                                                    catch(Exception ex)
                                                    {

                                                    }
                                                    //if(oEffectiveness==null)
                                                    if (iEffectivenessID<=0)
                                                    {
                                                        try
                                                        {
                                                            EFFECTIVENESS oEffectiveness = new EFFECTIVENESS();
                                                            oEffectiveness.EffectivenessName = sEffectiveness;
                                                            oEffectiveness.CreatedDate = DateTimeOffset.Now;
                                                            oEffectiveness.timestamp = DateTimeOffset.Now;
                                                            oEffectiveness.VocabularyID = iCWEVocabularyID;
                                                            model.EFFECTIVENESS.Add(oEffectiveness);
                                                            model.SaveChanges();
                                                            iEffectivenessID = oEffectiveness.EffectivenessID;
                                                        }
                                                        catch(Exception exEFFECTIVENESS)
                                                        {
                                                            Console.WriteLine("Exception: exEFFECTIVENESS " + exEFFECTIVENESS.Message + " " + exEFFECTIVENESS.InnerException);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //Update EFFECTIVENESS
                                                    }
                                                    Console.WriteLine("DEBUG iEffectivenessID=" + iEffectivenessID);

                                                    try
                                                    {
                                                        //Update MITIGATIONFORCWE
                                                        //oCWEMitigation.CWEMitigationEffectiveness = sEffectiveness; // Removed
                                                        oCWEMitigation.EffectivenessID = iEffectivenessID;
                                                        oCWEMitigation.timestamp = DateTimeOffset.Now;
                                                        model.SaveChanges();
                                                    }
                                                    catch (Exception exCWEMitigationEffectiveness)
                                                    {
                                                        Console.WriteLine("Exception: exCWEMitigationEffectiveness " + exCWEMitigationEffectiveness.Message + " " + exCWEMitigationEffectiveness.InnerException);
                                                    }

                                                    //TODO MITIGATIONEFFECTIVENESS

                                                    #endregion mitigationeffectiveness
                                                    break;
                                                
                                                case "Mitigation_Effectiveness_Notes":
                                                    sMitigationEffectivenessNotes = CleaningCWEString(nodeCWEMitigationInfo.InnerText);
                                                    sMitigationEffectivenessNotes=sMitigationEffectivenessNotes.Replace("<Text>","");
                                                    sMitigationEffectivenessNotes=sMitigationEffectivenessNotes.Replace("</Text>","");
                                                    try
                                                    {
                                                        oCWEMitigation.CWEMitigationEffectivenessNotes = sMitigationEffectivenessNotes;
                                                        oCWEMitigation.timestamp = DateTimeOffset.Now;
                                                        model.SaveChanges();
                                                    }
                                                    catch(Exception exCWEMitigationEffectivenessNotes)
                                                    {
                                                        Console.WriteLine("Exception: exCWEMitigationEffectivenessNotes " + exCWEMitigationEffectivenessNotes.Message + " " + exCWEMitigationEffectivenessNotes.InnerException);
                                                    }
                                                    break;
                                                
                                                default:
                                                    Console.WriteLine("ERROR: Missing code for nodeCWEMitigationInfo " + nodeCWEMitigationInfo.Name);
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            //Mitigation_Description
                                            #region mitigationdescription
                                            foreach (XmlNode nodeCWEMitigationDescriptionText in nodeCWEMitigationInfo)
                                            {
                                                //TODO Review: Problem if many
                                                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                Console.WriteLine("DEBUG nodeCWEMitigationDescriptionText");
                                                if (nodeCWEMitigationDescriptionText.Name != "Text")
                                                {
                                                    //TODO
                                                    Console.WriteLine("ERROR: Missing code for nodeCWEMitigationDescriptionText " + nodeCWEMitigationDescriptionText.Name);
                                                    //Block
                                                    //<Block Block_Nature="Good_Code">
                                                    //<Block Block_Nature="Bad_Code">
                                                    //<Block Block_Nature="Numeric_List">
                                                    //MITIGATIONCODE

                                                }
                                                else
                                                {
                                                    //TODO
                                                    //MITIGATIONFORCWE
                                                    string sCWEMitigationDescription = nodeCWEMitigationDescriptionText.InnerText;
                                                    string sCWEMitigationDescriptionClean = CleaningCWEString(sCWEMitigationDescription);
                                                    
                                                    oCWEMitigation = model.MITIGATIONFORCWE.Where(o => o.CWEID == sCWEID).FirstOrDefault();
                                                    if (oCWEMitigation != null)
                                                    {
                                                        //Update MITIGATIONFORCWE
                                                        //TODO: check if it is the good mitigation
                                                        //sMitigationID
                                                        //TODO: Update if needed
                                                        
                                                        try
                                                        {
                                                            oCWEMitigation.VocabularyID = iCWEVocabularyID;

                                                            if (sCWEMitigationPhaseName.Trim() != "")
                                                            {
                                                                //oCWEMitigation.Mitigation_Phase = sCWEMitigationPhaseName;  //Removed
                                                                oCWEMitigation.MitigationPhaseID = iMitigationPhaseID;
                                                            }
                                                            if (sCWEMitigationStrategyName.Trim() != "")
                                                            {
                                                                //oCWEMitigation.Mitigation_Strategy = sCWEMitigationStrategyName;    //Removed
                                                                oCWEMitigation.MitigationStrategyID = iMitigationStrategyID;
                                                            }
                                                            oCWEMitigation.MitigationVocabularyID = sMitigationID;
                                                            if (iEffectivenessID > 0)
                                                            {
                                                                oCWEMitigation.EffectivenessID = iEffectivenessID;
                                                            }
                                                            oCWEMitigation.CWEMitigationEffectivenessNotes = sMitigationEffectivenessNotes;
                                                            oCWEMitigation.timestamp = DateTimeOffset.Now;
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
                                                            Console.WriteLine("Exception: DbEntityValidationExceptionMITIGATIONFORCWEUpdate05 " + sb.ToString());
                                                        }
                                                        catch (Exception exMITIGATIONFORCWEUpdate)
                                                        {
                                                            Console.WriteLine("Exception: exMITIGATIONFORCWEUpdate05 " + exMITIGATIONFORCWEUpdate.Message + " " + exMITIGATIONFORCWEUpdate.InnerException);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //No MITIGATIONFORCWE exists for the CWE
                                                        oMitigation = model.MITIGATION.Where(o => o.SolutionMitigationText == sCWEMitigationDescriptionClean).FirstOrDefault();  //TODO: VocabularyID    sMitigationID
                                                        if (oMitigation != null)
                                                        {
                                                            #region updatemitigation
                                                            //Update MITIGATION
                                                            //TODO: check VocabularyID? ...
                                                            //TODO: Update if needed
                                                            //TODO: sMitigationID if unique
                                                            
                                                            try
                                                            {
                                                                oMitigation.MitigationVocabularyID = sMitigationID; //TODO: could be renamed to MitigationSourceID
                                                                //iMitigationStrategyID //TODO
                                                                //oMitigation.Mitigation_Effectiveness = sEffectiveness;    // Removed
                                                                oMitigation.EffectivenessID = iEffectivenessID;
                                                                oMitigation.Mitigation_Effectiveness_Notes = sMitigationEffectivenessNotes;
                                                                oMitigation.timestamp = DateTimeOffset.Now;
                                                                oMitigation.VocabularyID = iCWEVocabularyID;
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
                                                                Console.WriteLine("Exception: DbEntityValidationExceptionMITIGATIONUpdate " + sb.ToString());
                                                            }
                                                            catch (Exception exMITIGATIONUpdate)
                                                            {
                                                                Console.WriteLine("Exception: exMITIGATIONUpdate " + exMITIGATIONUpdate.Message + " " + exMITIGATIONUpdate.InnerException);
                                                            }

                                                            
                                                            try
                                                            {
                                                                //Update MITIGATIONFORCWE
                                                                oCWEMitigation.MitigationID = oMitigation.MitigationID;
                                                                oCWEMitigation.EffectivenessID = iEffectivenessID;
                                                                oCWEMitigation.CWEMitigationEffectivenessNotes = sMitigationEffectivenessNotes;
                                                                oCWEMitigation.timestamp = DateTimeOffset.Now;

                                                                //Update MITIGATIONPHASEFORMITIGATION
                                                                oMitigationMitigationPhase.MitigationID = oMitigation.MitigationID;
                                                                oMitigationMitigationPhase.timestamp = DateTimeOffset.Now;

                                                                //Update MITIGATIONSTRATEGYFORMITIGATION
                                                                oMitigationMitigationStrategy.MitigationID = oMitigation.MitigationID;
                                                                oMitigationMitigationStrategy.timestamp = DateTimeOffset.Now;
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
                                                                Console.WriteLine("Exception: DbEntityValidationExceptionCWEMitigationEffectivenessNotes " + sb.ToString());
                                                            }
                                                            catch(Exception exCWEMitigationEffectivenessNotes)
                                                            {
                                                                Console.WriteLine("Exception: exCWEMitigationEffectivenessNotes " + exCWEMitigationEffectivenessNotes.Message + " " + exCWEMitigationEffectivenessNotes.InnerException);
                                                            }

                                                            
                                                            #endregion updatemitigation
                                                        }
                                                        else
                                                        {
                                                            #region createmitigation
                                                            try
                                                            {
                                                                oMitigation = new MITIGATION();

                                                                oMitigation.SolutionMitigationText = sCWEMitigationDescriptionClean;
                                                                ////oMitigation.MitigationVocabularyID = 20;  //TODO
                                                                oMitigation.MitigationVocabularyID = sMitigationID; //TODO: could be renamed to MitigationSourceID
                                                                //NOTE: is it unique?   //MIT-6
                                                                //NOTE: Mitigation Phase? Mitigation Strategy?
                                                                oMitigation.CreatedDate = DateTimeOffset.Now;
                                                                oMitigation.timestamp = DateTimeOffset.Now;
                                                                oMitigation.VocabularyID = iCWEVocabularyID;
                                                                //oMitigation.Mitigation_Effectiveness = sEffectiveness;    //Removed
                                                                oMitigation.EffectivenessID=iEffectivenessID;
                                                                oMitigation.Mitigation_Effectiveness_Notes = sMitigationEffectivenessNotes;
                                                                model.MITIGATION.Add(oMitigation);
                                                                model.SaveChanges();
                                                                Console.WriteLine("DEBUG AddToMITIGATION02 " + sCWEMitigationDescriptionClean);
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
                                                                Console.WriteLine("Exception: DbEntityValidationExceptionMITIGATION02 " + sb.ToString());
                                                            }
                                                            catch(Exception exMITIGATIONAdd)
                                                            {
                                                                Console.WriteLine("Exception: exMITIGATIONAdd02 " + exMITIGATIONAdd.Message + " " + exMITIGATIONAdd.InnerException);
                                                            }
                                                            //TODO
                                                            //MITIGATIONCODE

                                                            #endregion createmitigation
                                                        }

                                                        if (sCWEMitigationPhaseName.Trim() != "")
                                                        {
                                                            #region MITIGATIONPHASEFORMITIGATION
                                                            //Optimization because no update
                                                            //MITIGATIONPHASEFORMITIGATION oMitigationPhaseForMitigation = model.MITIGATIONPHASEFORMITIGATION.FirstOrDefault(o => o.MitigationID == oMitigation.MitigationID && o.MitigationPhaseID == oMitigationPhase.MitigationPhaseID);
                                                            int iMitigationPhaseForMitigationID = 0;
                                                            try
                                                            {
                                                                iMitigationPhaseForMitigationID = model.MITIGATIONPHASEFORMITIGATION.Where(o => o.MitigationID == oMitigation.MitigationID && o.MitigationPhaseID == iMitigationPhaseID).Select(o => o.MitigationMitigationPhaseID).FirstOrDefault();
                                                            }
                                                            catch (Exception ex)
                                                            {

                                                            }
                                                            //if (oMitigationPhaseForMitigation != null)
                                                            if (iMitigationPhaseForMitigationID > 0)
                                                            {
                                                                //Update MITIGATIONPHASEFORMITIGATION
                                                                //TODO: update if needed

                                                            }
                                                            else
                                                            {

                                                                try
                                                                {
                                                                    MITIGATIONPHASEFORMITIGATION oMitigationPhaseForMitigation = new MITIGATIONPHASEFORMITIGATION();
                                                                    oMitigationPhaseForMitigation.MitigationID = oMitigation.MitigationID;
                                                                    oMitigationPhaseForMitigation.MitigationPhaseID = iMitigationPhaseID;   // oMitigationPhase.MitigationPhaseID;
                                                                    //
                                                                    oMitigationPhaseForMitigation.CreatedDate = DateTimeOffset.Now;
                                                                    oMitigationPhaseForMitigation.VocabularyID = iCWEVocabularyID;
                                                                    oMitigationPhaseForMitigation.timestamp = DateTimeOffset.Now;
                                                                    model.MITIGATIONPHASEFORMITIGATION.Add(oMitigationPhaseForMitigation);
                                                                    model.SaveChanges();
                                                                }
                                                                catch (Exception exMITIGATIONPHASEFORMITIGATION)
                                                                {
                                                                    Console.WriteLine("Exception: exMITIGATIONPHASEFORMITIGATION " + exMITIGATIONPHASEFORMITIGATION.Message + " " + exMITIGATIONPHASEFORMITIGATION.InnerException);
                                                                }
                                                                Console.WriteLine("DEBUG AddToMITIGATIONPHASEFORMITIGATION");
                                                            }
                                                            #endregion MITIGATIONPHASEFORMITIGATION

                                                            //Update MITIGATIONFORCWE
                                                            oCWEMitigation.MitigationPhaseID = iMitigationPhaseID;
                                                            oCWEMitigation.timestamp = DateTimeOffset.Now;
                                                            model.SaveChanges();
                                                        }

                                                        if (sCWEMitigationStrategyName.Trim() != "")
                                                        {
                                                            //Update MITIGATIONFORCWE
                                                            // Removed
                                                            //oCWEMitigation.Mitigation_Strategy = sCWEMitigationStrategyName;    // sCWEMitigationPhaseName;
                                                            oCWEMitigation.MitigationStrategyID = iMitigationStrategyID;
                                                            oCWEMitigation.timestamp = DateTimeOffset.Now;
                                                            model.SaveChanges();

                                                            #region MITIGATIONSTRATEGYFORMITIGATION
                                                            //Optimization because no update
                                                            //MITIGATIONSTRATEGYFORMITIGATION oMitigationStrategyForMitigation = model.MITIGATIONSTRATEGYFORMITIGATION.FirstOrDefault(o => o.MitigationID == oMitigation.MitigationID && o.MitigationStrategyID == oMitigationStrategy.MitigationStrategyID);
                                                            int iMitigationStrategyForMitigationID = 0;
                                                            try
                                                            {
                                                                iMitigationStrategyForMitigationID = model.MITIGATIONSTRATEGYFORMITIGATION.Where(o => o.MitigationID == oMitigation.MitigationID && o.MitigationStrategyID == iMitigationStrategyID).Select(o => o.MitigationMitigationStrategyID).FirstOrDefault();
                                                            }
                                                            catch (Exception ex)
                                                            {

                                                            }
                                                            //if (oMitigationStrategyForMitigation != null)
                                                            if (iMitigationStrategyForMitigationID > 0)
                                                            {
                                                                //Update MITIGATIONSTRATEGYFORMITIGATION
                                                                //TODO: update if needed

                                                            }
                                                            else
                                                            {

                                                                try
                                                                {
                                                                    MITIGATIONSTRATEGYFORMITIGATION oMitigationStrategyForMitigation = new MITIGATIONSTRATEGYFORMITIGATION();
                                                                    oMitigationStrategyForMitigation.MitigationID = oMitigation.MitigationID;
                                                                    oMitigationStrategyForMitigation.MitigationStrategyID = iMitigationStrategyID;  // oMitigationStrategy.MitigationStrategyID;

                                                                    oMitigationStrategyForMitigation.CreatedDate = DateTimeOffset.Now;
                                                                    oMitigationStrategyForMitigation.timestamp = DateTimeOffset.Now;
                                                                    oMitigationStrategyForMitigation.VocabularyID = iCWEVocabularyID;
                                                                    model.MITIGATIONSTRATEGYFORMITIGATION.Add(oMitigationStrategyForMitigation);
                                                                    model.SaveChanges();
                                                                    Console.WriteLine("DEBUG AddToMITIGATIONStrategyFORMITIGATION");
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
                                                                    Console.WriteLine("Exception: DbEntityValidationExceptionMITIGATIONSTRATEGYFORMITIGATION " + sb.ToString());
                                                                }
                                                                catch (Exception exMITIGATIONSTRATEGYFORMITIGATION)
                                                                {
                                                                    Console.WriteLine("Exception: exMITIGATIONSTRATEGYFORMITIGATION " + exMITIGATIONSTRATEGYFORMITIGATION.Message + " " + exMITIGATIONSTRATEGYFORMITIGATION.InnerException);
                                                                }
                                                            }
                                                            #endregion MITIGATIONSTRATEGYFORMITIGATION
                                                        }

                                                        if (sEffectiveness.Trim() != "")
                                                        {
                                                            //Update MITIGATIONFORCWE
                                                            //oCWEMitigation.CWEMitigationEffectiveness = sEffectiveness; // Removed
                                                            oCWEMitigation.EffectivenessID = iEffectivenessID;
                                                            oCWEMitigation.timestamp = DateTimeOffset.Now;
                                                            try
                                                            {
                                                                model.SaveChanges();
                                                            }
                                                            catch (Exception exCWEMitigationEffectiveness)
                                                            {
                                                                Console.WriteLine("Exception: exCWEMitigationEffectiveness " + exCWEMitigationEffectiveness.Message + " " + exCWEMitigationEffectiveness.InnerException);
                                                            }

                                                            #region mitigationeffectiveness
                                                            //Optimization because no update
                                                            //TODO Is this table really needed? Remove
                                                            //MITIGATIONEFFECTIVENESS oMitigationEffectiveness = model.MITIGATIONEFFECTIVENESS.FirstOrDefault(o => o.MitigationID == oMitigation.MitigationID && o.EffectivenessID == oEffectiveness.EffectivenessID);
                                                            int iMitigationEffectivenessID = 0;
                                                            try
                                                            {
                                                                iMitigationEffectivenessID = model.MITIGATIONEFFECTIVENESS.Where(o => o.MitigationID == oMitigation.MitigationID && o.EffectivenessID == iEffectivenessID).Select(o => o.MitigationEffectivenessID).FirstOrDefault();
                                                            }
                                                            catch (Exception ex)
                                                            {

                                                            }
                                                            //if (oMitigationEffectiveness == null)
                                                            if (iMitigationEffectivenessID <= 0)
                                                            {

                                                                try
                                                                {
                                                                    MITIGATIONEFFECTIVENESS oMitigationEffectiveness = new MITIGATIONEFFECTIVENESS();
                                                                    oMitigationEffectiveness.EffectivenessID = iEffectivenessID;    // oEffectiveness.EffectivenessID;
                                                                    oMitigationEffectiveness.MitigationID = oMitigation.MitigationID;
                                                                    oMitigationEffectiveness.CreatedDate = DateTimeOffset.Now;
                                                                    oMitigationEffectiveness.timestamp = DateTimeOffset.Now;
                                                                    oMitigationEffectiveness.VocabularyID = iCWEVocabularyID;
                                                                    model.MITIGATIONEFFECTIVENESS.Add(oMitigationEffectiveness);
                                                                    model.SaveChanges();
                                                                    //iMitigationEffectivenessID=
                                                                }
                                                                catch (Exception exMITIGATIONEFFECTIVENESS)
                                                                {
                                                                    Console.WriteLine("Exception: exMITIGATIONEFFECTIVENESS " + exMITIGATIONEFFECTIVENESS.Message + " " + exMITIGATIONEFFECTIVENESS.InnerException);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                //Update MITIGATIONEFFECTIVENESS
                                                            }
                                                            #endregion mitigationeffectiveness
                                                        }

                                                        #region MITIGATIONFORCWE
                                                        try
                                                        {
                                                            oCWEMitigation = new MITIGATIONFORCWE();
                                                            oCWEMitigation.CWEID = sCWEID;
                                                            oCWEMitigation.MitigationID = oMitigation.MitigationID;
                                                            oCWEMitigation.CreatedDate = DateTimeOffset.Now;
                                                            oCWEMitigation.EffectivenessID = iEffectivenessID;
                                                            if (sCWEMitigationPhaseName.Trim() != "")
                                                            {
                                                                //oCWEMitigation.Mitigation_Phase = sCWEMitigationPhaseName;  //Removed
                                                                oCWEMitigation.MitigationPhaseID = iMitigationPhaseID;
                                                            }
                                                            if (sCWEMitigationStrategyName.Trim() != "")
                                                            {
                                                                //oCWEMitigation.Mitigation_Strategy = sCWEMitigationStrategyName;    //Removed
                                                                oCWEMitigation.MitigationStrategyID = iMitigationStrategyID;
                                                            }
                                                            oCWEMitigation.VocabularyID = iCWEVocabularyID;
                                                            oCWEMitigation.timestamp = DateTimeOffset.Now;
                                                            model.MITIGATIONFORCWE.Add(oCWEMitigation);
                                                            model.SaveChanges();
                                                            Console.WriteLine("DEBUG AddToMITIGATIONFORCWE: " + sCWEID + " " + sCWEMitigationDescriptionClean);
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
                                                            Console.WriteLine("Exception: DbEntityValidationExceptionMITIGATIONFORCWEAdd " + sb.ToString());
                                                        }
                                                        catch (Exception exMITIGATIONFORCWEAdd)
                                                        {
                                                            Console.WriteLine("Exception: exMITIGATIONFORCWEAdd " + exMITIGATIONFORCWEAdd.Message + " " + exMITIGATIONFORCWEAdd.InnerException);
                                                        }
                                                        #endregion MITIGATIONFORCWE

                                                        
                                                    }



                                                }
                                            }
                                            #endregion mitigationdescription
                                        }
                                    }
                                }
                            }
                            break;
                        #endregion CWEMitigation
                        
                        //TODO
                        
                        case "Weakness_Ordinalities":
                            #region CWEOrdinality
                            //CWEORDINALITY
                            foreach (XmlNode nodeCWEWeaknessOrdinality in nodeCWEinfo)
                            {
                                CWEORDINALITY oCWEOrdinality = null;
                                if (nodeCWEWeaknessOrdinality.Name != "Weakness_Ordinality")
                                {
                                    Console.WriteLine("ERROR: Missing code for nodeCWEWeaknessOrdinality " + nodeCWEWeaknessOrdinality.Name);

                                }
                                else
                                {
                                    foreach (XmlNode nodeOrdinality in nodeCWEWeaknessOrdinality)
                                    {
                                        if (nodeOrdinality.Name != "Ordinality")
                                        {
                                            if (nodeOrdinality.Name == "Ordinality_Description")
                                            {
                                                if (oCWEOrdinality != null)
                                                {
                                                    oCWEOrdinality.Ordinality_Description = CleaningCWEString(nodeOrdinality.InnerText);
                                                }
                                                else
                                                {
                                                    Console.WriteLine("ERROR: for Ordinality_Description "+sCWEID);
                                                }
                                            }
                                            else
                                            {
                                                Console.WriteLine("ERROR: Missing code for nodeOrdinality " + nodeOrdinality.Name);
                                            }
                                        }
                                        else
                                        {
                                            string sWeaknessOrdinality=CleaningCWEString(nodeOrdinality.InnerText);    //Primary
                                            oCWEOrdinality = model.CWEORDINALITY.FirstOrDefault(o => o.CWEID == sCWEID && o.WeaknessOrdinality == sWeaknessOrdinality);
                                            if (oCWEOrdinality == null)
                                            {
                                                oCWEOrdinality = new CWEORDINALITY();
                                                oCWEOrdinality.CreatedDate = DateTimeOffset.Now;
                                                oCWEOrdinality.timestamp = DateTimeOffset.Now;
                                                oCWEOrdinality.WeaknessOrdinality = sWeaknessOrdinality;
                                                oCWEOrdinality.CWEID = sCWEID;
                                                oCWEOrdinality.VocabularyID = iCWEVocabularyID;
                                                model.CWEORDINALITY.Add(oCWEOrdinality);
                                                model.SaveChanges();
                                            }
                                            else
                                            {
                                                //Update CWEORDINALITY
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                            #endregion CWEOrdinality

                        case "Background_Details":
                            //CWEObject.CWEBackgroundDetails
                            //TODO
                            string sCWEBackgroundDetails = nodeCWEinfo.InnerText;
                            sCWEBackgroundDetails = sCWEBackgroundDetails.Replace("<Background_Detail>", "");
                            sCWEBackgroundDetails = sCWEBackgroundDetails.Replace("</Background_Detail>", "");
                            sCWEBackgroundDetails = sCWEBackgroundDetails.Replace("<Text>", "");
                            sCWEBackgroundDetails = sCWEBackgroundDetails.Replace("</Text>", "");
                            sCWEBackgroundDetails = CleaningCWEString(sCWEBackgroundDetails);
                            
                            CWEObject.CWEBackgroundDetails = sCWEBackgroundDetails;
                            try
                            {
                                //Update CWE

                                model.SaveChanges();
                            }
                            catch(Exception exCWEBackgroundDetails)
                            {
                                Console.WriteLine("Exception: exCWEBackgroundDetails " + exCWEBackgroundDetails.Message + " " + exCWEBackgroundDetails.InnerException);
                            }
                            break;
                        
                        
                        //TODO
                        case "Causal_Nature":
                            CWEObject.CWECausalNature = CleaningCWEString(nodeCWEinfo.InnerText);  //Explicit
                            try
                            {
                                model.SaveChanges();
                                Console.WriteLine("DEBUG: CausualNature: " + nodeCWEinfo.InnerText);
                            }
                            catch (Exception exCausualNature)
                            {
                                Console.WriteLine("Exception: exCausualNature " + exCausualNature.Message + " " + exCausualNature.InnerException);
                            }
                            break;
                        
                        
                        case "Demonstrative_Examples":
                            //TODO
                            //CWEDEMONSTRATIVEEXAMPLE
                            //Could be MISUSECASE?  <Block Block_Nature="Bad_Code">
                            int iCptExample = 0;

                            #region CWEDemonstrativeExample
                            foreach (XmlNode nodeCWEDemonstrativeExample in nodeCWEinfo)
                            {
                                switch(nodeCWEDemonstrativeExample.Name)
                                {
                                
                                    case "Demonstrative_Example":
                                        string sDemonstrativeExampleGUID = "";
                                        iCptExample++;
                                        try
                                        {
                                            sDemonstrativeExampleGUID = nodeCWEDemonstrativeExample.Attributes["Demonstrative_Example_ID"].InnerText;
                                        }
                                        catch (Exception exsDemonstrativeExampleGUID)
                                        {
                                            string sIgnoreWarning = exsDemonstrativeExampleGUID.Message;
                                            //Console.WriteLine("Exception: exsDemonstrativeExampleGUID " + exsDemonstrativeExampleGUID.Message + " " + exsDemonstrativeExampleGUID.InnerException);
                                            sDemonstrativeExampleGUID = sCWEID + "-"+iCptExample; //TODO: Workaround
                                        }

                                        DEMONSTRATIVEEXAMPLE oDemonstrativeExample = model.DEMONSTRATIVEEXAMPLE.FirstOrDefault(o => o.DemonstrativeExampleGUID == sDemonstrativeExampleGUID);   //TODO: Check VocabularyID
                                        //int iDemonstrativeExampleID = model.DEMONSTRATIVEEXAMPLE.Where(o => o.DemonstrativeExampleGUID == sDemonstrativeExampleGUID).Select(o=>o.DemonstrativeExampleID).FirstOrDefault();   //TODO: Check VocabularyID
                                        if (oDemonstrativeExample != null)
                                        //if (iDemonstrativeExampleID != 0)
                                        {
                                            //Update DEMONSTRATIVEEXAMPLE (done later)
                                            //TODO: Check if any change?

                                        }
                                        else
                                        {
                                        
                                            try
                                            {
                                                oDemonstrativeExample = new DEMONSTRATIVEEXAMPLE();
                                                oDemonstrativeExample.DemonstrativeExampleGUID = sDemonstrativeExampleGUID;
                                                oDemonstrativeExample.VocabularyID = iCWEVocabularyID;
                                                oDemonstrativeExample.CreatedDate = DateTimeOffset.Now;
                                                oDemonstrativeExample.timestamp = DateTimeOffset.Now;
                                                model.DEMONSTRATIVEEXAMPLE.Add(oDemonstrativeExample);
                                                //Console.WriteLine("DEBUG AddToDEMONSTRATIVEEXAMPLE: " + sCWEID + " " + sDemonstrativeExampleGUID);
                                                //iDemonstrativeExampleID = oDemonstrativeExample.DemonstrativeExampleID;
                                            }
                                            catch (Exception exAddToDEMONSTRATIVEEXAMPLE)
                                            {
                                                Console.WriteLine("Exception: exAddToDEMONSTRATIVEEXAMPLE " + exAddToDEMONSTRATIVEEXAMPLE.Message + " " + exAddToDEMONSTRATIVEEXAMPLE.InnerException);
                                            }
                                        }

                                        string sDemonstrativeExampleIntroText = string.Empty;
                                        string sDemonstrativeExampleBody = string.Empty;    //TODO: Review this/details in it

                                        foreach (XmlNode nodeCWEDemonstrativeExampleInfo in nodeCWEDemonstrativeExample)
                                        {
                                            switch (nodeCWEDemonstrativeExampleInfo.Name)
                                            {
                                                case "Intro_Text":
                                                    sDemonstrativeExampleIntroText = CleaningCWEString(nodeCWEDemonstrativeExampleInfo.InnerText);
                                                    break;
                                                case "Example_Body":
                                                    sDemonstrativeExampleBody = CleaningCWEString(nodeCWEDemonstrativeExampleInfo.InnerText);
                                                    //TODO: Cleaning Review
                                                    //TODO: Language in it  <Code_Example_Language>C</Code_Example_Language>
                                                    //TODO: Code in it <Block Block_Nature="Bad_Code">  
                                                    //DEMONSTRATIVEEXAMPLECODE
                                                    break;

                                                //TODO
                                                
                                                case "Demonstrative_Example_References":
                                                    //foreach Reference
                                                    foreach (XmlNode nodeDemoExReference in nodeCWEDemonstrativeExampleInfo)
                                                    {
                                                        //string sRefAuthor = "";
                                                        //int iAuthorID = 0;
                                                        List<int> ListAuthors=new List<int>();
                                                        string sReference_Title = "";
                                                        string sReference_Date = "";
                                                        string sReference_Link = "";    //URL
                                                        string sReference_Edition = "";
                                                        string sReference_Publisher = "";
                                                        string sReference_Publication = "";
                                                        string sReference_PubDate = "";

                                                        string sReference_Section = "";
                                                        //Others?

                                                        foreach (XmlNode nodeDemoExReferenceInfo in nodeDemoExReference)
                                                        {
                                                            switch(nodeDemoExReferenceInfo.Name)
                                                            {
                                                                case "Reference_Author":
                                                                    string sRefAuthor = CleaningCWEString(nodeDemoExReferenceInfo.InnerText);

                                                                    #region AUTHOR
                                                                    int iAuthorID=0;
                                                                    try
                                                                    {
                                                                        iAuthorID=model.AUTHOR.Where(o => o.AuthorName == sRefAuthor).Select(o => o.AuthorID).FirstOrDefault();
                                                                    }
                                                                    catch(Exception ex)
                                                                    {

                                                                    }
                                                                    if (iAuthorID<=0)
                                                                    {
                                                                        Console.WriteLine("DEBUG Adding new AUTHOR found in References");
                                                                        AUTHOR myrefauthor = new AUTHOR();
                                                                        myrefauthor.AuthorName = sRefAuthor;
                                                                        //TODO: check for PersonID in PERSON or OrganisationID
                                                                        myrefauthor.CreatedDate = DateTimeOffset.Now;
                                                                        myrefauthor.timestamp = DateTimeOffset.Now;
                                                                        myrefauthor.VocabularyID = iVocabularyCWEID;
                                                                        model.AUTHOR.Add(myrefauthor);
                                                                        model.SaveChanges();
                                                                        iAuthorID = myrefauthor.AuthorID;
                                                                    }
                                                                    else
                                                                    {
                                                                        //Update AUTHOR
                                                                    }
                                                                    #endregion AUTHOR

                                                                    ListAuthors.Add(iAuthorID);
                                                                    break;
                                                                case "Reference_Title":
                                                                    sReference_Title = CleaningCWEString(nodeDemoExReferenceInfo.InnerText);
                                                                    break;
                                                                case "Reference_Date":
                                                                    sReference_Date = CleaningCWEString(nodeDemoExReferenceInfo.InnerText);
                                                                    break;
                                                                case "Reference_Link":
                                                                    sReference_Link = CleaningCWEString(nodeDemoExReferenceInfo.InnerText);
                                                                    //TODO REVIEW
                                                                    //TODO Normalize
                                                                    sReference_Link = sReference_Link.Replace("http://www.", "http://");
                                                                    sReference_Link = sReference_Link.Replace("https://www.", "https://");
                                                                    break;
                                                                case "Reference_PubDate":
                                                                    sReference_PubDate = CleaningCWEString(nodeDemoExReferenceInfo.InnerText);
                                                                    break;
                                                                case "Reference_Publisher":
                                                                    sReference_Publisher = CleaningCWEString(nodeDemoExReferenceInfo.InnerText);
                                                                    break;
                                                                case "Reference_Publication":
                                                                    sReference_Publication = CleaningCWEString(nodeDemoExReferenceInfo.InnerText);
                                                                    break;
                                                                default:
                                                                    Console.WriteLine("ERROR: Missing code for nodeDemoExReferenceInfo " + nodeDemoExReferenceInfo.Name);
                                                                    break;
                                                            }
                                                        }

                                                        int iReferenceID = 0;
                                                        try
                                                        {
                                                            //TODO Review ==?
                                                            if (sReference_Link != "")
                                                            {
                                                                iReferenceID = model.REFERENCE.Where(o => o.ReferenceURL == sReference_Link).Select(o => o.ReferenceID).FirstOrDefault();
                                                            }
                                                            else
                                                            {

                                                                iReferenceID = model.REFERENCE.Where(o => o.ReferenceTitle == sReference_Title).Select(o => o.ReferenceID).FirstOrDefault();
                                                            }
                                                        }
                                                        catch(Exception ex)
                                                        {

                                                        }
                                                        if(iReferenceID<=0)
                                                        {
                                                            REFERENCE oReference = new REFERENCE();
                                                            oReference.CreatedDate = DateTimeOffset.Now;
                                                            oReference.ReferenceTitle = sReference_Title;
                                                            oReference.ReferenceURL = sReference_Link;
                                                            oReference.Reference_Date = sReference_Date;
                                                            oReference.Reference_PubDate = sReference_PubDate;
                                                            oReference.Reference_Publisher = sReference_Publisher;
                                                            oReference.Reference_Publication = sReference_Publication;
                                                            oReference.VocabularyID = iVocabularyCWEID;
                                                            oReference.timestamp = DateTimeOffset.Now;
                                                            model.REFERENCE.Add(oReference);
                                                            model.SaveChanges();
                                                            iReferenceID = oReference.ReferenceID;
                                                        }
                                                        else
                                                        {
                                                            //Update REFERENCE
                                                            //TODO
                                                        }

                                                        //TODO: could we have a Reference without any Author?
                                                        foreach(int iAuthorID in ListAuthors)
                                                        {
                                                            int iReferenceAuthorID = 0;
                                                            try
                                                            {
                                                                iReferenceAuthorID = model.REFERENCEAUTHOR.Where(o => o.ReferenceID == iReferenceID && o.AuthorID == iAuthorID).Select(o => o.ReferenceAuthorID).FirstOrDefault();
                                                            }
                                                            catch(Exception ex)
                                                            {

                                                            }
                                                            if(iReferenceAuthorID<=0)
                                                            {
                                                                REFERENCEAUTHOR oRefAuthor = new REFERENCEAUTHOR();
                                                                oRefAuthor.CreatedDate = DateTimeOffset.Now;
                                                                oRefAuthor.ReferenceID = iReferenceID;
                                                                oRefAuthor.AuthorID = iAuthorID;
                                                                oRefAuthor.VocabularyID = iVocabularyCWEID;
                                                                oRefAuthor.timestamp = DateTimeOffset.Now;
                                                                model.REFERENCEAUTHOR.Add(oRefAuthor);
                                                                //model.SaveChanges();    //TEST PERFORMANCE
                                                                //iReferenceAuthorID=
                                                            }
                                                            else
                                                            {
                                                                //Update REFERENCEAUTHOR
                                                            }
                                                        }
                                                    }
                                                    break;
                                                

                                                default:
                                                    Console.WriteLine("ERROR: Missing code for nodeCWEDemonstrativeExampleInfo " + nodeCWEDemonstrativeExampleInfo.Name);
                                                    break;
                                            }

                                        }

                                        
                                        try
                                        {
                                            //UPDATE DEMONSTRATIVEEXAMPLE
                                            oDemonstrativeExample.DemonstrativeExampleIntroText = sDemonstrativeExampleIntroText;
                                            oDemonstrativeExample.DemonstrativeExampleBody = sDemonstrativeExampleBody;
                                            oDemonstrativeExample.timestamp = DateTimeOffset.Now;
                                            model.SaveChanges();
                                        }
                                        catch (Exception exDemonstrativeExample)
                                        {
                                            Console.WriteLine("Exception: exDemonstrativeExample " + exDemonstrativeExample.Message + " " + exDemonstrativeExample.InnerException);
                                        }

                                        //CWEDEMONSTRATIVEEXAMPLE oCWEDemonstrativeExample = model.CWEDEMONSTRATIVEEXAMPLE.FirstOrDefault(o => o.CWEID == sCWEID && o.DemonstrativeExampleID == oDemonstrativeExample.DemonstrativeExampleID);
                                        int iCWEDemonstrativeExampleID = model.CWEDEMONSTRATIVEEXAMPLE.Where(o => o.CWEID == sCWEID && o.DemonstrativeExampleID == oDemonstrativeExample.DemonstrativeExampleID).Select(o=>o.CWEDemonstrativeExampleID).FirstOrDefault();
                                        //if (oCWEDemonstrativeExample != null)
                                        if (iCWEDemonstrativeExampleID != 0)
                                        {
                                            //Update CWEDEMONSTRATIVEEXAMPLE
                                        }
                                        else
                                        {
                                        
                                        
                                            try
                                            {
                                                CWEDEMONSTRATIVEEXAMPLE oCWEDemonstrativeExample = new CWEDEMONSTRATIVEEXAMPLE();
                                                oCWEDemonstrativeExample.CreatedDate = DateTimeOffset.Now;
                                                oCWEDemonstrativeExample.CWEID = sCWEID;
                                                oCWEDemonstrativeExample.DemonstrativeExampleID = oDemonstrativeExample.DemonstrativeExampleID;
                                                oCWEDemonstrativeExample.VocabularyID = iCWEVocabularyID;
                                                oCWEDemonstrativeExample.timestamp = DateTimeOffset.Now;
                                                model.CWEDEMONSTRATIVEEXAMPLE.Add(oCWEDemonstrativeExample);
                                                model.SaveChanges();    //TEST PERFORMANCE
                                                Console.WriteLine("DEBUG AddToCWEDEMONSTRATIVEEXAMPLE: " + sCWEID + " " + sDemonstrativeExampleGUID);
                                            }
                                            catch (Exception exAddToCWEDEMONSTRATIVEEXAMPLE)
                                            {
                                                Console.WriteLine("Exception: exAddToCWEDEMONSTRATIVEEXAMPLE " + exAddToCWEDEMONSTRATIVEEXAMPLE.Message + " " + exAddToCWEDEMONSTRATIVEEXAMPLE.InnerException);
                                            }

                                        }
                                        break;

                                    case "":

                                        break;

                                    default:
                                        Console.WriteLine("ERROR: Missing code for Demonstrative_Examples " + nodeCWEDemonstrativeExample.Name);
                                        break;

                                }
                            }
                            break;
                            #endregion CWEDemonstrativeExample

                        
                        case "Observed_Examples":
                            //TODO
                            //VULNERABILITYFORCWE
                            foreach (XmlNode nodeCWEObservedExample in nodeCWEinfo)
                            {

                                foreach (XmlNode nodeCWEObservedExampleInfo in nodeCWEinfo)
                                {
                                    
                                    if (nodeCWEObservedExampleInfo.Name == "Observed_Example_Reference")
                                    {
                                        //CVE-2005-2146
                                        string sExampleRef = nodeCWEObservedExampleInfo.InnerText;
                                        if(sExampleRef.StartsWith("CVE-"))
                                        {
                                            //Optimization because no update
                                            //VULNERABILITY oVulnerability=model.VULNERABILITY.FirstOrDefault(o=>o.VULReferential=="cve" && o.VULReferentialID==sExampleRef);
                                            int iVulnerabilityID = 0;
                                            try
                                            {
                                                iVulnerabilityID=vuln_model.VULNERABILITY.Where(o => o.VULReferential == "cve" && o.VULReferentialID == sExampleRef).Select(o => o.VulnerabilityID).FirstOrDefault();
                                            }
                                            catch(Exception ex)
                                            {

                                            }
                                            //if (oVulnerability == null)
                                            if (iVulnerabilityID <= 0)
                                            {
                                                Console.WriteLine("ERROR: Missing CVE " + sExampleRef + " (adding it)");
                                                VULNERABILITY oVulnerability = new VULNERABILITY();
                                                    oVulnerability.VULReferential = "cve";
                                                    oVulnerability.VULReferentialID = sExampleRef;
                                                    oVulnerability.CreatedDate = DateTimeOffset.Now;
                                                    oVulnerability.timestamp = DateTimeOffset.Now;
                                                    oVulnerability.VocabularyID = iCWEVocabularyID;
                                                    vuln_model.VULNERABILITY.Add(oVulnerability);
                                                    vuln_model.SaveChanges();
                                                iVulnerabilityID = oVulnerability.VulnerabilityID;
                                            }
                                            else
                                            {
                                                //Update VULNERABILITY
                                            }

                                            //Optimization because no update
                                            //VULNERABILITYFORCWE oVulnerabilityForCWE = model.VULNERABILITYFORCWE.FirstOrDefault(o => o.CWEID == sCWEID && o.VulnerabilityID == iVulnerabilityID); //oVulnerability.VulnerabilityID
                                            int iCWEVulnerabilityID = 0;
                                            try
                                            {
                                                iCWEVulnerabilityID=vuln_model.VULNERABILITYFORCWE.Where(o => o.CWEID == sCWEID && o.VulnerabilityID == iVulnerabilityID).Select(o => o.CWEVulnerabilityID).FirstOrDefault();
                                            }
                                            catch(Exception ex)
                                            {

                                            }
                                            //if (oVulnerabilityForCWE == null)
                                            if (iCWEVulnerabilityID <= 0)
                                            {
                                                VULNERABILITYFORCWE oVulnerabilityForCWE = new VULNERABILITYFORCWE();
                                                oVulnerabilityForCWE.CWEID = sCWEID;
                                                oVulnerabilityForCWE.VulnerabilityID = iVulnerabilityID;    // oVulnerability.VulnerabilityID;
                                                oVulnerabilityForCWE.CreatedDate = DateTimeOffset.Now;
                                                oVulnerabilityForCWE.timestamp = DateTimeOffset.Now;
                                                oVulnerabilityForCWE.VocabularyID = iCWEVocabularyID;
                                                vuln_model.VULNERABILITYFORCWE.Add(oVulnerabilityForCWE);
                                                vuln_model.SaveChanges();    //TEST PERFORMANCE
                                                //iCWEVulnerabilityID=
                                            }
                                            else
                                            {
                                                //Update VULNERABILITYFORCWE
                                            }
                                        }
                                        else
                                        {
                                            //Not a CVE-
                                            Console.WriteLine("ERROR: Missing code for Observed_Example_Reference " + sExampleRef);
                                        }
                                    }
                                    //Observed_Example_Description
                                }
                            }
                            break;
                        

                        case "Theoretical_Notes":
                            //CWETHEORETICALNOTE
                            #region CWETheoreticalNotes
                            foreach (XmlNode nodeCWETheoreticalNote in nodeCWEinfo)
                            {
                                string sCWETheoreticalNote = CleaningCWEString(nodeCWETheoreticalNote.InnerText);

                                int iTheoreticalNoteID = 0;
                                try
                                {
                                    iTheoreticalNoteID = model.THEORETICALNOTE.Where(o => o.TheoreticalNoteText == sCWETheoreticalNote).Select(o=>o.TheoreticalNoteID).FirstOrDefault();
                                }
                                catch(Exception ex)
                                {

                                }
                                //THEORETICALNOTE oTheoreticalNote = model.THEORETICALNOTE.FirstOrDefault(o => o.TheoreticalNoteText == sCWETheoreticalNote);
                                //if (oTheoreticalNote != null)
                                if (iTheoreticalNoteID>0)
                                {
                                    /*
                                    //Update THEORETICALNOTE with Clean
                                    oTheoreticalNote.TheoreticalNoteTextClean = CleaningCWEString(sCWETheoreticalNote);
                                    oTheoreticalNote.timestamp = DateTimeOffset.Now;
                                    model.SaveChanges();
                                    */
                                }
                                else
                                {
                                    Console.WriteLine("DEBUG Adding THEORETICALNOTE");
                                    THEORETICALNOTE oTheoreticalNote = new THEORETICALNOTE();
                                    oTheoreticalNote.CreatedDate = DateTimeOffset.Now;
                                    oTheoreticalNote.TheoreticalNoteText = sCWETheoreticalNote;
                                    //oTheoreticalNote.TheoreticalNoteTextClean = CleaningCWEString(sCWETheoreticalNote);
                                    oTheoreticalNote.timestamp = DateTimeOffset.Now;
                                    oTheoreticalNote.VocabularyID = iCWEVocabularyID;
                                    
                                    model.THEORETICALNOTE.Add(oTheoreticalNote);
                                    model.SaveChanges();
                                    iTheoreticalNoteID = oTheoreticalNote.TheoreticalNoteID;
                                }

                                //CWETHEORETICALNOTE oCWETHeoreticalNote = model.CWETHEORETICALNOTE.FirstOrDefault(o => o.CWEID == sCWEID && o.TheoreticalNoteID == oTheoreticalNote.TheoreticalNoteID);  //TODO: VocabularyID?
                                int iCWETheoreticalNoteID = 0;
                                try
                                {
                                    iCWETheoreticalNoteID=model.CWETHEORETICALNOTE.Where(o => o.CWEID == sCWEID && o.TheoreticalNoteID == iTheoreticalNoteID).Select(o => o.CWETheoreticalNoteID).FirstOrDefault();  //TODO: VocabularyID?
                                }
                                catch(Exception ex)
                                {

                                }
                                //if (oCWETHeoreticalNote != null)
                                if (iCWETheoreticalNoteID != 0)
                                {
                                    //Update CWETHEORETICALNOTE
                                }
                                else
                                {
                                    CWETHEORETICALNOTE oCWETHeoreticalNote = new CWETHEORETICALNOTE();
                                    oCWETHeoreticalNote.CWEID = sCWEID;
                                    oCWETHeoreticalNote.TheoreticalNoteID = iTheoreticalNoteID;
                                    oCWETHeoreticalNote.timestamp = DateTimeOffset.Now;
                                    oCWETHeoreticalNote.VocabularyID = iCWEVocabularyID;
                                    oCWETHeoreticalNote.CreatedDate = DateTimeOffset.Now;
                                    //TODO: Try Catch...
                                    model.CWETHEORETICALNOTE.Add(oCWETHeoreticalNote);
                                    //model.SaveChanges();    //TEST PERFORMANCE
                                    //iCWETheoreticalNoteID=
                                }

                            }
                            break;
                            #endregion CWETheoreticalNotes

                        //TODO
                        
                        case "Functional_Areas":
                            foreach (XmlNode nodeFunctionalArea in nodeCWEinfo)
                            {
                                if (nodeFunctionalArea.Name != "Functional_Area")
                                {
                                    Console.WriteLine("ERROR: Missing code for Functional_Areas " + nodeFunctionalArea.Name);

                                }
                                else
                                {
                                    string sFunctionalAreaName=CleaningCWEString(nodeFunctionalArea.InnerText).Trim();
                                    //Optimization because no update
                                    //FUNCTIONALAREA oFunctionalArea = model.FUNCTIONALAREA.FirstOrDefault(o => o.FunctionalAreaName == sFunctionalAreaName);
                                    int iFunctionalAreaID = 0;
                                    try
                                    {
                                        iFunctionalAreaID=model.FUNCTIONALAREA.Where(o => o.FunctionalAreaName == sFunctionalAreaName).Select(o => o.FunctionalAreaID).FirstOrDefault();
                                    }
                                    catch(Exception ex)
                                    {

                                    }
                                    //if (oFunctionalArea == null)
                                    if (iFunctionalAreaID <= 0)
                                    {
                                        FUNCTIONALAREA oFunctionalArea = new FUNCTIONALAREA();
                                        oFunctionalArea.FunctionalAreaName = sFunctionalAreaName;
                                        oFunctionalArea.CreatedDate = DateTimeOffset.Now;
                                        oFunctionalArea.timestamp = DateTimeOffset.Now;
                                        oFunctionalArea.VocabularyID = iCWEVocabularyID;
                                        model.FUNCTIONALAREA.Add(oFunctionalArea);
                                        model.SaveChanges();
                                        iFunctionalAreaID = oFunctionalArea.FunctionalAreaID;
                                    }
                                    else
                                    {
                                        //Update FUNCTIONALAREA
                                    }

                                    //Optimization because no update
                                    //CWEFUNCTIONALAREA oCWEFunctionalArea = model.CWEFUNCTIONALAREA.FirstOrDefault(o => o.CWEFunctionalAreaID ==iFunctionalAreaID && o.CWEID == sCWEID); //oFunctionalArea.FunctionalAreaID
                                    int iCWEFunctionalAreaID = 0;
                                    try
                                    {
                                        iCWEFunctionalAreaID=model.CWEFUNCTIONALAREA.Where(o => o.CWEFunctionalAreaID == iFunctionalAreaID && o.CWEID == sCWEID).Select(o => o.CWEFunctionalAreaID).FirstOrDefault();
                                    }
                                    catch(Exception ex)
                                    {

                                    }
                                    //if (oCWEFunctionalArea == null)
                                    if (iCWEFunctionalAreaID == 0)
                                    {
                                        CWEFUNCTIONALAREA oCWEFunctionalArea = new CWEFUNCTIONALAREA();
                                        oCWEFunctionalArea.CWEID = sCWEID;
                                        oCWEFunctionalArea.FunctionalAreaID = iFunctionalAreaID;    // oFunctionalArea.FunctionalAreaID;
                                        oCWEFunctionalArea.CreatedDate = DateTimeOffset.Now;
                                        oCWEFunctionalArea.timestamp = DateTimeOffset.Now;
                                        oCWEFunctionalArea.VocabularyID = iCWEVocabularyID;
                                        model.CWEFUNCTIONALAREA.Add(oCWEFunctionalArea);
                                        //model.SaveChanges();    //TEST PERFORMANCE
                                        //iCWEFunctionalAreaID=
                                    }
                                    else
                                    {
                                        //Update CWEFUNCTIONALAREA
                                    }
                                }
                            }
                            break;
                        

                        case "Affected_Resources":
                            //TODO
                            //CWEAFFECTEDRESOURCE
                            #region CWEAffectedResource
                            foreach (XmlNode nodeCWEAffectedResource in nodeCWEinfo)
                            {
                                Console.WriteLine("DEBUG: nodeCWEAffectedResource");
                                string sAffectedResource =CleaningCWEString(nodeCWEAffectedResource.InnerText); //Memory
                                //Console.WriteLine("DEBUG: sAffectedResource=" + sAffectedResource);

                                //AFFECTEDRESOURCE oAffectedResource = model.AFFECTEDRESOURCE.FirstOrDefault(o => o.AffectedResourceName == sAffectedResource);
                                int iAffectedResourceID = model.AFFECTEDRESOURCE.Where(o => o.AffectedResourceName == sAffectedResource).Select(o=>o.AffectedResourceID).FirstOrDefault();
                                //if (oAffectedResource != null)
                                if (iAffectedResourceID != 0)
                                {
                                    //Update AFFECTEDRESOURCE
                                    //TODO: Check if it is the same VocabularyID
                                }
                                else
                                {
                                    
                                    try
                                    {
                                        AFFECTEDRESOURCE oAffectedResource = new AFFECTEDRESOURCE();
                                        oAffectedResource.AffectedResourceName = sAffectedResource;
                                        //oAffectedResource.CWEID = sCWEID; //TODO
                                        oAffectedResource.CreatedDate = DateTimeOffset.Now;
                                        oAffectedResource.timestamp = DateTimeOffset.Now;
                                        oAffectedResource.VocabularyID = iCWEVocabularyID;
                                        model.AFFECTEDRESOURCE.Add(oAffectedResource);
                                        model.SaveChanges();
                                        //Console.WriteLine("DEBUG: AddToAFFECTEDRESOURCE " + sAffectedResource + " " + sCWEID);
                                        iAffectedResourceID = oAffectedResource.AffectedResourceID;
                                    }
                                    catch (Exception exAddToAFFECTEDRESOURCE)
                                    {
                                        Console.WriteLine("Exception: exAddToAFFECTEDRESOURCE " + exAddToAFFECTEDRESOURCE.Message + " " + exAddToAFFECTEDRESOURCE.InnerException);
                                    }
                                }

                                //CWEAFFECTEDRESOURCE oCWEAffectedResource = model.CWEAFFECTEDRESOURCE.FirstOrDefault(o => o.AffectedResourceID == iAffectedResourceID && o.CWEID == sCWEID);
                                int iCWEAffectedResourceID = 0;
                                try
                                {
                                    iCWEAffectedResourceID=model.CWEAFFECTEDRESOURCE.Where(o => o.AffectedResourceID == iAffectedResourceID && o.CWEID == sCWEID).Select(o => o.CWEAffectedResourceID).FirstOrDefault();
                                }
                                catch(Exception ex)
                                {

                                }
                                //if (oCWEAffectedResource != null)
                                if (iCWEAffectedResourceID != 0)
                                {
                                    //Update CWEAFFECTEDRESOURCE
                                }
                                else
                                {
                                    
                                    try
                                    {
                                        CWEAFFECTEDRESOURCE oCWEAffectedResource = new CWEAFFECTEDRESOURCE();
                                        oCWEAffectedResource.CWEID = sCWEID;
                                        oCWEAffectedResource.AffectedResourceID = iAffectedResourceID;  // oAffectedResource.AffectedResourceID;
                                        oCWEAffectedResource.CreatedDate = DateTimeOffset.Now;
                                        oCWEAffectedResource.timestamp = DateTimeOffset.Now;
                                        oCWEAffectedResource.VocabularyID = iCWEVocabularyID;
                                        model.CWEAFFECTEDRESOURCE.Add(oCWEAffectedResource);
                                        //model.SaveChanges();    //TEST PERFORMANCE
                                        //iCWEAffectedResourceID=
                                        //Console.WriteLine("DEBUG: AddToCWEAFFECTEDRESOURCE " + sAffectedResource + " " + sCWEID);
                                    }
                                    catch (Exception exAddToCWEAFFECTEDRESOURCE)
                                    {
                                        Console.WriteLine("Exception: exAddToCWEAFFECTEDRESOURCE " + exAddToCWEAFFECTEDRESOURCE.Message + " " + exAddToCWEAFFECTEDRESOURCE.InnerException);
                                    }
                                }
                            }
                            break;
                            #endregion CWEAffectedResource

                        case "References":
                            #region CWEReferences
                            foreach (XmlNode nodeCWEReference in nodeCWEinfo)
                            {
                                Console.WriteLine("DEBUG: nodeCWEReference");
                                string sCWEReferenceSourceID = "";
                                try
                                {
                                    sCWEReferenceSourceID = nodeCWEReference.Attributes["Reference_ID"].InnerText;   //REF-17
                                }
                                catch (Exception ex)
                                {
                                    //Could be null
                                    string sIgnoreWarning = ex.Message;
                                }

                                if(sCWEReferenceSourceID!="")
                                {
                                    string sReferenceSection = string.Empty;
                                    string sReferenceEdition = "";
                                    string sReferencePublisher = "";
                                    string sReferencePublication = "";
                                    string sReferencePubDate = "";
                                    string sReferenceDate = "";

                                    //Note: not perfect (Reference_Title, Reference_Section, Reference_Link should be the first lines)
                                    //TODO: Review. Maybe a first loop to retrieve everything
                                    bool bReferenceExists = false;
                                    REFERENCE oReference = model.REFERENCE.FirstOrDefault(o => o.ReferenceSourceID == sCWEReferenceSourceID);   //TODO: VocabularyID
                                    if (oReference != null)
                                    {
                                        //Update REFERENCE
                                        bReferenceExists = true;
                                    }
                                    else
                                    {
                                        
                                        try
                                        {
                                            oReference = new REFERENCE();
                                            oReference.ReferenceSourceID = sCWEReferenceSourceID;
                                            oReference.CreatedDate = DateTimeOffset.Now;
                                            oReference.timestamp = DateTimeOffset.Now;
                                            oReference.Source = "CWE";
                                            oReference.VocabularyID = iCWEVocabularyID;
                                            //TODO: review this (more details? CreationObject, lang...)
                                            
                                            model.REFERENCE.Add(oReference);
                                            model.SaveChanges();
                                            //oReference = REFERENCENormalize(model, oReference);
                                        }
                                        catch (Exception exAddToREFERENCE)
                                        {
                                            Console.WriteLine("Exception: exAddToREFERENCE " + exAddToREFERENCE.Message + " " + exAddToREFERENCE.InnerException);
                                        }
                                    }
                                    foreach (XmlNode nodeCWEReferenceInfo in nodeCWEReference)
                                    {
                                        switch (nodeCWEReferenceInfo.Name)
                                        {
                                            case "Reference_Author":
                                                //Optimization because no update
                                                //AUTHOR oAuthor = model.AUTHOR.FirstOrDefault(o => o.AuthorName == nodeCWEReferenceInfo.InnerText);
                                                string sAuthor = CleaningCWEString(nodeCWEReferenceInfo.InnerText);
                                                int iAuthorID =0;
                                                try
                                                {
                                                    iAuthorID = model.AUTHOR.Where(o => o.AuthorName == sAuthor).Select(o=>o.AuthorID).FirstOrDefault();
                                                }
                                                catch(Exception ex)
                                                {

                                                }
                                                //if (oAuthor != null)
                                                if (iAuthorID != 0)
                                                {

                                                }
                                                else
                                                {
                                                    
                                                    try
                                                    {
                                                        AUTHOR oAuthor = new AUTHOR();  //TODO Cleaning
                                                        oAuthor.AuthorName = sAuthor;   // nodeCWEReferenceInfo.InnerText;
                                                        oAuthor.CreatedDate = DateTimeOffset.Now;
                                                        oAuthor.timestamp = DateTimeOffset.Now;
                                                        //TODO
                                                        oAuthor.VocabularyID = iCWEVocabularyID;
                                                        model.AUTHOR.Add(oAuthor);
                                                        model.SaveChanges();
                                                        iAuthorID = oAuthor.AuthorID;
                                                    }
                                                    catch(Exception exAddToAUTHOR)
                                                    {
                                                        Console.WriteLine("Exception: exAddToAUTHOR " + exAddToAUTHOR.Message + " " + exAddToAUTHOR.InnerException);
                                                    }
                                                }

                                                //Optimization because no update
                                                //REFERENCEAUTHOR oReferenceAuthor = model.REFERENCEAUTHOR.FirstOrDefault(o => o.ReferenceID == oReference.ReferenceID && o.AuthorID == iAuthorID);   //oAuthor.AuthorID
                                                int iReferenceAuthorID=0;
                                                try
                                                {
                                                    iReferenceAuthorID=model.REFERENCEAUTHOR.Where(o => o.ReferenceID == oReference.ReferenceID && o.AuthorID == iAuthorID).Select(o => o.ReferenceAuthorID).FirstOrDefault();   //oAuthor.AuthorID
                                                }
                                                catch(Exception ex)
                                                {

                                                }
                                                //if (oReferenceAuthor != null)
                                                if (iReferenceAuthorID != 0)
                                                {
                                                    //Update REFERENCEAUTHOR
                                                }
                                                else
                                                {
                                                    
                                                    try
                                                    {
                                                        REFERENCEAUTHOR oReferenceAuthor = new REFERENCEAUTHOR();
                                                        oReferenceAuthor.CreatedDate = DateTimeOffset.Now;
                                                        oReferenceAuthor.ReferenceID = oReference.ReferenceID;
                                                        oReferenceAuthor.AuthorID = iAuthorID;  // oAuthor.AuthorID;
                                                        oReferenceAuthor.VocabularyID = iCWEVocabularyID;
                                                        oReferenceAuthor.timestamp = DateTimeOffset.Now;
                                                        //TODO: Review
                                                        model.REFERENCEAUTHOR.Add(oReferenceAuthor);
                                                        //model.SaveChanges();    //TEST PERFORMANCE
                                                        //iReferenceAuthorID=
                                                    }
                                                    catch (Exception exAddToREFERENCEAUTHOR)
                                                    {
                                                        Console.WriteLine("Exception: exAddToREFERENCEAUTHOR " + exAddToREFERENCEAUTHOR.Message + " " + exAddToREFERENCEAUTHOR.InnerException);
                                                    }
                                                }

                                                break;
                                            case "Reference_Title":
                                                string sReferenceTitle = CleaningCWEString(nodeCWEReferenceInfo.InnerText);
                                                if (bReferenceExists && oReference.ReferenceTitle != sReferenceTitle)
                                                {
                                                    Console.WriteLine("ERROR: We were working with the Reference " + oReference.ReferenceTitle + " but it is in fact " + sReferenceTitle);
                                                }
                                                else
                                                {
                                                    //Update REFERENCE
                                                    oReference.ReferenceTitle = sReferenceTitle;
                                                    //TODO Parse, regex
                                                    try
                                                    {
                                                        model.SaveChanges();    //Update the just added Reference
                                                    }
                                                    catch (Exception exReferenceTitle)
                                                    {
                                                        Console.WriteLine("Exception: exReferenceTitle " + exReferenceTitle.Message + " " + exReferenceTitle.InnerException);                                                   
                                                    }
                                                }
                                                break;
                                            case "Reference_Link":
                                                //TODO: Review this
                                                string sURLReferenceLink=nodeCWEReferenceInfo.InnerText;
                                                //TODO NORMALIZE
                                                sURLReferenceLink = sURLReferenceLink.Replace("http://www.", "http://");
                                                sURLReferenceLink = sURLReferenceLink.Replace("https://www.", "https://");
                                                sURLReferenceLink = sURLReferenceLink.Replace("http://osvdb.org/displayvuln.php?osvdbid=", "http://osvdb.org/");
                                                sURLReferenceLink = sURLReferenceLink.Replace("http://osvdb.org/show/osvdb/", "http://osvdb.org/");
                                                sURLReferenceLink = sURLReferenceLink.Replace("securitytracker.com/id?", "securitytracker.com/id/");

                                                //Update REFERENCE
                                                oReference.ReferenceURL = sURLReferenceLink;
                                                try
                                                {
                                                    model.SaveChanges();    //Update the just added Reference
                                                }
                                                catch (Exception exReferenceURL)
                                                {
                                                    Console.WriteLine("Exception: exReferenceURL " + exReferenceURL.Message + " " + exReferenceURL.InnerException);
                                                }
                                                break;
                                            case "Reference_Section":
                                                sReferenceSection = CleaningCWEString(nodeCWEReferenceInfo.InnerText);
                                                //Console.WriteLine("DEBUG sReferenceSection=" + sReferenceSection);
                                                break;
                                            case "Reference_Publication":
                                                sReferencePublication = CleaningCWEString(nodeCWEReferenceInfo.InnerText);
                                                break;
                                            case "Reference_Publisher":
                                                sReferencePublisher = CleaningCWEString(nodeCWEReferenceInfo.InnerText);
                                                break;
                                            case "Reference_Edition":
                                                sReferenceEdition = CleaningCWEString(nodeCWEReferenceInfo.InnerText);
                                                break;
                                            case "Reference_PubDate":
                                                sReferencePubDate = CleaningCWEString(nodeCWEReferenceInfo.InnerText);
                                                break;
                                            
                                            case "Reference_Date":
                                                sReferenceDate=nodeCWEReferenceInfo.InnerText;
                                                break;
                                            
                                            default:
                                                Console.WriteLine("ERROR: Missing Code for nodeCWEReferenceInfo " + nodeCWEReferenceInfo.Name);
                                                break;
                                        }
                                    }

                                    //Update the REFERENCE
                                    oReference.Reference_Edition = sReferenceEdition;
                                    oReference.Reference_PubDate = sReferencePubDate;
                                    oReference.Reference_Publication = sReferencePublication;
                                    oReference.Reference_Publisher = sReferencePublisher;
                                    oReference.Reference_Date = sReferenceDate;
                                    oReference.VocabularyID = iCWEVocabularyID;
                                    oReference.timestamp = DateTimeOffset.Now;
                                    model.SaveChanges();


                                    CWEREFERENCE oCWEReference = model.CWEREFERENCE.FirstOrDefault(o => o.CWEID == sCWEID && o.ReferenceID == oReference.ReferenceID);  //TODO: VocabularyID?
                                    if (oCWEReference != null)
                                    {
                                        //Update CWEREFERENCE
                                        oCWEReference.timestamp = DateTimeOffset.Now;
                                        oCWEReference.Reference_Section = sReferenceSection;
                                        oCWEReference.LocalReferenceID = sCWEReferenceSourceID;
                                        oCWEReference.VocabularyID = iCWEVocabularyID;
                                        //model.SaveChanges();    //TEST PERFORMANCE
                                        Console.WriteLine("DEBUG sReferenceSection updated");
                                        
                                    }
                                    else
                                    {
                                        
                                        try
                                        {
                                            oCWEReference = new CWEREFERENCE();
                                            oCWEReference.CWEID = sCWEID;
                                            oCWEReference.ReferenceID = oReference.ReferenceID;
                                            oCWEReference.CreatedDate = DateTimeOffset.Now;
                                            oCWEReference.timestamp = DateTimeOffset.Now;
                                            oCWEReference.Reference_Section = sReferenceSection;
                                            oCWEReference.VocabularyID = iCWEVocabularyID;
                                            model.CWEREFERENCE.Add(oCWEReference);
                                            model.SaveChanges();
                                        }
                                        catch (Exception exAddToCWEREFERENCE04)
                                        {
                                            Console.WriteLine("Exception: exAddToCWEREFERENCE04 " + exAddToCWEREFERENCE04.Message + " " + exAddToCWEREFERENCE04.InnerException);
                                        }
                                    }
                                }
                            }
                            break;
                            #endregion CWEReferences

                        //TODO
                        
                        case "White_Box_Definitions":
                            
                            foreach (XmlNode nodeWhiteBoxDefinition in nodeCWEinfo)
                            {
                                if (nodeWhiteBoxDefinition.Name != "White_Box_Definition")
                                {
                                    Console.WriteLine("ERROR: Missing code for White_Box_Definitions " + nodeWhiteBoxDefinition.Name);

                                }
                                else
                                {
                                    //TODO: Cleaning? <Text>    <Block>
                                    //Update CWE
                                    CWEObject.White_Box_Definitions = nodeWhiteBoxDefinition.InnerText;
                                    //TODO: Multiple?
                                    model.SaveChanges();
                                }
                            }
                            break;
                        

                        //TODO
                        
                        case "Other_Notes":
                            //
                            foreach (XmlNode nodeNote in nodeCWEinfo)
                            {
                                if (nodeNote.Name != "Note")
                                {
                                    Console.WriteLine("ERROR: Missing code for Other_Notes " + nodeNote.Name);

                                }
                                else
                                {
                                    string sNote = nodeNote.InnerText;
                                    //Cleaning
                                    sNote = sNote.Replace("<Text>", "");
                                    sNote = sNote.Replace("</Text>", "");
                                    sNote = sNote.Replace("<text>", "");
                                    sNote = sNote.Replace("</text>", "");
                                    sNote = CleaningCWEString(sNote);

                                    //Update CWE
                                    CWEObject.Other_Notes = sNote;
                                    model.SaveChanges();

                                }
                            }
                            break;
                        

                        //TODO
                        
                        case "Research_Gaps":
                            foreach (XmlNode nodeResearchGap in nodeCWEinfo)
                            {
                                if (nodeResearchGap.Name != "Research_Gap")
                                {
                                    Console.WriteLine("ERROR: Missing code for Research_Gaps " + nodeResearchGap.Name);

                                }
                                else
                                {
                                    string sResearchGap = nodeResearchGap.InnerText;
                                    //Cleaning
                                    sResearchGap = sResearchGap.Replace("<Text>", "");
                                    sResearchGap = sResearchGap.Replace("</Text>", "");
                                    sResearchGap = CleaningCWEString(sResearchGap);

                                    //Update CWE
                                    CWEObject.Research_Gaps = sResearchGap;
                                    model.SaveChanges();
                                    //TODO: Multiple?
                                }
                            }
                            break;

                        case "Modes_of_Introduction":
                            #region modesofintroduction
                            foreach (XmlNode nodeModeIntroduction in nodeCWEinfo)
                            {
                                if (nodeModeIntroduction.Name != "Mode_of_Introduction")
                                {
                                    Console.WriteLine("ERROR: Missing code for nodeModeIntroduction " + nodeModeIntroduction.Name);
                                }
                                else
                                {
                                    string sModeIntroduction=CleaningCWEString(nodeModeIntroduction.InnerText); //TODO Review

                                    //CWEMODEOFINTRODUCTION oCWEModeIntroduction = model.CWEMODEOFINTRODUCTION.FirstOrDefault(o => o.CWEID == sCWEID && o.ModeOfIntroductionDescription == sModeIntroduction);
                                    int iCWEModeOfIntroductionID = 0;
                                    try
                                    {
                                        iCWEModeOfIntroductionID=model.CWEMODEOFINTRODUCTION.Where(o => o.CWEID == sCWEID && o.ModeOfIntroductionDescription == sModeIntroduction).Select(o => o.CWEModeOfIntroductionID).FirstOrDefault();
                                    }
                                    catch(Exception ex)
                                    {

                                    }
                                    //if (oCWEModeIntroduction == null)
                                    if (iCWEModeOfIntroductionID == 0)
                                    {
                                        CWEMODEOFINTRODUCTION oCWEModeIntroduction = new CWEMODEOFINTRODUCTION();
                                        oCWEModeIntroduction.ModeOfIntroductionDescription = sModeIntroduction;
                                        oCWEModeIntroduction.CWEID = sCWEID;
                                        oCWEModeIntroduction.CreatedDate = DateTimeOffset.Now;
                                        oCWEModeIntroduction.timestamp = DateTimeOffset.Now;
                                        oCWEModeIntroduction.VocabularyID = iCWEVocabularyID;
                                        model.CWEMODEOFINTRODUCTION.Add(oCWEModeIntroduction);
                                        model.SaveChanges();
                                    }
                                    else
                                    {
                                        //Update CWEMODEOFINTRODUCTION
                                    }
                                }
                            }
                            #endregion modesofintroduction
                            break;

                        case "Relevant_Properties":
                            #region relevantproperties
                            foreach (XmlNode nodeRelevantProperty in nodeCWEinfo)
                            {
                                //TODO? one table for this
                                if (nodeRelevantProperty.Name != "Relevant_Property")
                                {
                                    Console.WriteLine("ERROR: Missing code for nodeRelevantProperty " + nodeRelevantProperty.Name);

                                }
                                else
                                {
                                    //Cleaning?
                                    string sRelevantProperty=nodeRelevantProperty.InnerText;    //Equivalence   Uniqueness
                                    //CWERELEVANTPROPERTY oCWERelevantProperty = model.CWERELEVANTPROPERTY.FirstOrDefault(o => o.CWEID == sCWEID && o.Relevant_Property == sRelevantProperty);
                                    int iCWERelevantPropertyID = 0;
                                    try
                                    {
                                        iCWERelevantPropertyID = model.CWERELEVANTPROPERTY.Where(o => o.CWEID == sCWEID && o.Relevant_Property == sRelevantProperty).Select(o => o.CWERelevantPropertyID).FirstOrDefault();
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                    //if (oCWERelevantProperty == null)
                                    if (iCWERelevantPropertyID <= 0)
                                    {
                                        CWERELEVANTPROPERTY oCWERelevantProperty = new CWERELEVANTPROPERTY();
                                        oCWERelevantProperty.CWEID = sCWEID;
                                        oCWERelevantProperty.Relevant_Property = sRelevantProperty;
                                        oCWERelevantProperty.CreatedDate = DateTimeOffset.Now;
                                        oCWERelevantProperty.timestamp = DateTimeOffset.Now;
                                        oCWERelevantProperty.VocabularyID = iCWEVocabularyID;
                                        model.CWERELEVANTPROPERTY.Add(oCWERelevantProperty);
                                        //model.SaveChanges();    //TEST PERFORMANCE
                                        //iCWERelevantPropertyID=
                                    }
                                    else
                                    {
                                        //Update CWERELEVANTPROPERTY
                                    }
                                }
                            }
                            #endregion relevantproperties
                            break;

                        
                        case "Enabling_Factors_for_Exploitation":
                            #region factorexploitation
                            //CWEENABLINGFACTOREXPLOITATION
                            foreach (XmlNode nodeExploitationFactor in nodeCWEinfo)
                            {
                                if (nodeExploitationFactor.Name != "Enabling_Factor_for_Exploitation")
                                {
                                    Console.WriteLine("ERROR: Missing code for nodeExploitationFactor " + nodeExploitationFactor.Name);

                                }
                                else
                                {
                                    string sExploitationFactor = nodeExploitationFactor.InnerText;
                                    sExploitationFactor = sExploitationFactor.Replace("<Text>", "");
                                    sExploitationFactor = sExploitationFactor.Replace("</Text>", "");
                                    sExploitationFactor = sExploitationFactor.Replace("<text>", "");
                                    sExploitationFactor = sExploitationFactor.Replace("</text>", "");
                                    sExploitationFactor = CleaningCWEString(sExploitationFactor);
                                    Console.WriteLine("DEBUG sExploitationFactor=" + sExploitationFactor);
                                    int iExploitationFactorID = 0;
                                    try
                                    {
                                        //Review ExploitationFactorName
                                        iExploitationFactorID = model.EXPLOITATIONFACTOR.Where(o => o.ExploitationFactorDescription == sExploitationFactor).Select(o => o.ExploitationFactorID).FirstOrDefault();
                                    }
                                    catch(Exception ex)
                                    {

                                    }
                                    if(iExploitationFactorID<=0)
                                    {
                                        try
                                        {
                                            EXPLOITATIONFACTOR oExploitationFactor = new EXPLOITATIONFACTOR();
                                            oExploitationFactor.CreatedDate = DateTimeOffset.Now;
                                            //TODO REVIEW
                                            //oExploitationFactor.ExploitationFactorName = sExploitationFactor;
                                            oExploitationFactor.ExploitationFactorDescription = sExploitationFactor;
                                            oExploitationFactor.VocabularyID = iVocabularyCWEID;
                                            oExploitationFactor.timestamp = DateTimeOffset.Now;
                                            model.EXPLOITATIONFACTOR.Add(oExploitationFactor);
                                            model.SaveChanges();
                                            iExploitationFactorID = oExploitationFactor.ExploitationFactorID;
                                        }
                                        catch (Exception exEXPLOITATIONFACTOR)
                                        {
                                            Console.WriteLine("Exception exEXPLOITATIONFACTOR " + exEXPLOITATIONFACTOR.Message + " " + exEXPLOITATIONFACTOR.InnerException);
                                        }
                                    }
                                    else
                                    {
                                        //Update EXPLOITATIONFACTOR
                                    }

                                    int iCWEExploitationFactor = 0;
                                    try
                                    {
                                        iCWEExploitationFactor = model.CWEEXPLOITATIONFACTOR.Where(o => o.CWEID == sCWEID && o.ExploitationFactorID == iExploitationFactorID).Select(o => o.CWEExploitationFactorID).FirstOrDefault();
                                    }
                                    catch(Exception ex)
                                    {

                                    }
                                    if(iCWEExploitationFactor<=0)
                                    {
                                        CWEEXPLOITATIONFACTOR oCWEExploitationFactor = new CWEEXPLOITATIONFACTOR();
                                        oCWEExploitationFactor.CreatedDate = DateTimeOffset.Now;
                                        oCWEExploitationFactor.CWEID = sCWEID;
                                        oCWEExploitationFactor.ExploitationFactorID = iExploitationFactorID;
                                        oCWEExploitationFactor.VocabularyID = iVocabularyCWEID;
                                        oCWEExploitationFactor.timestamp = DateTimeOffset.Now;
                                        model.CWEEXPLOITATIONFACTOR.Add(oCWEExploitationFactor);
                                        //model.SaveChanges();    //TEST PERFORMANCE
                                        //iCWEExploitationFactor=
                                    }
                                    else
                                    {
                                        //Update CWEEXPLOITATIONFACTOR
                                    }

                                }
                            }
                            #endregion factorexploitation
                            break;
                        
                        default:
                            //TODO
                            Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                            if (nodeCWEinfo.Name != "Content_History" && nodeCWEinfo.Name != "Maintenance_Notes")
                            {
                                if (nodeCWEinfo.Name != "Relationship_Notes")
                                {
                                    Console.WriteLine("ERROR: nodeCWEinfo Missing code for: " + nodeCWEinfo.Name);   //TODO
                                }
                            }
                            //Content_History
                            //Relationship_Notes
                            break;
                    }
                }

            }

            //FREE
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
                Console.WriteLine("Exception: DbEntityValidationExceptionFINALSAVECWE " + sb.ToString());
            }
            catch (Exception exFINALSAVE)
            {
                Console.WriteLine("Exception: exFINALSAVECWE " + exFINALSAVE.Message + " " + exFINALSAVE.InnerException);
            }
            model.Dispose();
            model = null;
        }

        
        static private void Import_saint()
        {
            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
            Console.WriteLine("Import SAINT...");
            string filename;
            //http://www.saintcorporation.com/xml/exploits.xml
            
            try
            {
                WebClient wc = new WebClient();
                wc.DownloadFile("http://www.saintcorporation.com/xml/exploits.xml", "C:/nvdcve/exploits.xml");
                // 
                wc.Dispose();
                //Console.WriteLine("Download is completed", "info", MessageBoxButtons.OK, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: while downloading SAINT exploits.xml\n" + ex.Message);
            }
            //filename = @"c:\exploits.xml";
            filename = @"C:\nvdcve\exploits.xml";   //TODO Hardcoded

            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(filename);
            }
            catch(Exception exLoadSAINT)
            {
                Console.WriteLine("Exception: exLoadSAINT " + exLoadSAINT.Message + " " + exLoadSAINT.InnerException);
                return;
            }

            string query = "/xml/body/exploits";
            XmlNode report;
            report = doc.SelectSingleNode(query);

            XORCISMEntities model=new XORCISMEntities();
            model.Configuration.AutoDetectChangesEnabled = false;
            model.Configuration.ValidateOnSaveEnabled = false;

            foreach (XmlNode n in report.ChildNodes)
            {
                //if (n.Name.ToUpper() == "exploit".ToUpper() && n.ChildNodes != null && n.ChildNodes.Count > 0)
                //{
                    EXPLOIT sploit = new EXPLOIT();
                    string myRefID=n.Attributes["id"].InnerText;
                    sploit.ExploitRefID = myRefID;
                    sploit.ExploitName = n.Attributes["id"].InnerText;
                    sploit.ExploitLocation = n.Attributes["id"].InnerText;
                    sploit.ExploitReferential = "saint";
                    sploit.ExploitDescription = HelperGetChildInnerText(n, "description");
                    //sploit.saint_id = HelperGetChildInnerText(n, "saint_id");   //TODO REVIEW CHANGE
                    string sSaintID = HelperGetChildInnerText(n, "saint_id");
                    sploit.ExploitRefID = sSaintID;
                    sploit.ExploitType = HelperGetChildInnerText(n, "type");
                    //Search the VulnerabilityID
                    string myCVE = HelperGetChildInnerText(n, "cve");
                    int vulnID = 0;
                    if (myCVE != "")
                    {
                        var syn = from S in vuln_model.VULNERABILITY
                                  where S.VULReferential.Equals("cve")
                                  && S.VULReferentialID.Equals(myCVE)
                                  select S.VulnerabilityID;
                        if (syn.Count() != 0)
                        {
                            vulnID = syn.ToList().First();  //.VulnerabilityID;
                            //                        Console.WriteLine("VulnerabilityID of " + myCVE + " is:" + vulnID);
                        }
                        else
                        {
                            Console.WriteLine("ERROR: Import_saint_exploits CVE not found! " + myCVE);
                            
                            try
                            {
                                //CANDIDATE
                                VULNERABILITY canCVE = new VULNERABILITY();
                                canCVE.VULReferential = "cve";
                                canCVE.VULReferentialID = myCVE;
                                canCVE.VULDescription = "CANDIDATE";
                                canCVE.VocabularyID = iVocabularySAINT;
                                canCVE.CreatedDate = DateTimeOffset.Now;
                                canCVE.timestamp = DateTimeOffset.Now;
                                vuln_model.VULNERABILITY.Add(canCVE);
                                vuln_model.SaveChanges();
                                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                Console.WriteLine("DEBUG CVE added as CANDIDATE");
                                vulnID = canCVE.VulnerabilityID;
                            }
                            catch (Exception exAddToVULNERABILITYCAN)
                            {
                                Console.WriteLine("Exception: exAddToVULNERABILITYCAN " + exAddToVULNERABILITYCAN.Message + " " + exAddToVULNERABILITYCAN.InnerException);
                            }

                            //    return;
                        }
                    }

                    //Check if the exploit already exists in the database
                    var syna = from S in model.EXPLOIT
                               where S.ExploitReferential.Equals("saint")
                               && S.ExploitRefID.Equals(myRefID)
                               select S.ExploitID;
                    if (syna.Count() == 0)
                    {
                        Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                        Console.WriteLine("DEBUG Adding new SAINT exploit " + myRefID);
                        sploit.CreatedDate = DateTimeOffset.Now;
                        sploit.VocabularyID=iVocabularySAINT;
                        model.EXPLOIT.Add(sploit);
                    }
                    else
                    {
                        sploit.ExploitID = syna.ToList().First();   //.ExploitID;
                    }
                    try
                    {
                        sploit.VocabularyID = iVocabularySAINT;
                        sploit.timestamp = DateTimeOffset.Now;
                        model.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Exception: AddToEXPLOIT : " + ex);
                        return;
                    }

                    if (vulnID != 0)
                    {
                        //Check if EXPLOITFORVULNERABILITY already exists in the database
                        var synj = from S in model.EXPLOITFORVULNERABILITY
                                   where S.VulnerabilityID.Equals(vulnID)
                                   && S.ExploitID.Equals(sploit.ExploitID)
                                   select S.VulnerabilityExploitID;
                        if (synj.Count() == 0)
                        {
                            
                            try
                            {
                                EXPLOITFORVULNERABILITY sploitvuln = new EXPLOITFORVULNERABILITY();
                                sploitvuln.VulnerabilityID = vulnID;
                                sploitvuln.ExploitID = sploit.ExploitID;
                                Console.WriteLine("DEBUG New exploit for vulnID " + vulnID);
                                //TODO: Send email
                                sploitvuln.VocabularyID=iVocabularySAINT;
                                sploitvuln.CreatedDate = DateTimeOffset.Now;
                                sploitvuln.timestamp = DateTimeOffset.Now;
                                model.EXPLOITFORVULNERABILITY.Add(sploitvuln);
                                model.SaveChanges();
                            }
                            catch (Exception exAddToEXPLOITFORVULNERABILITY09)
                            {
                                Console.WriteLine("Exception: exAddToEXPLOITFORVULNERABILITY09 : " + exAddToEXPLOITFORVULNERABILITY09.Message + " " + exAddToEXPLOITFORVULNERABILITY09.InnerException);
                            }
                        }
                    }

                    //****************************************************************
                    //  OSVDB
                    string myOSVDB = HelperGetChildInnerText(n, "osvdb");
                    if (myOSVDB != "")
                    {
                        //Check if the OSVDB reference already exists in the database
                        //TODO Review optimize
                        int osvdbID = 0;
                        var syn2 = from S in model.REFERENCE
                              where S.Source.Equals("OSVDB")
                              && S.ReferenceSourceID.Equals(myOSVDB)
                              select S.ReferenceID;

                        REFERENCE RefJA = new REFERENCE();
                        if (syn2.Count() != 0)
                        {
                            //Update REFERENCE OSVDB (To fix Url)
                            //TODO Remove/comment
                            /*
                            
                            RefJA.ReferenceID = osvdbID;
                            RefJA.ReferenceSourceID = myOSVDB;
                            RefJA.ReferenceURL = "http://osvdb.org/" + myOSVDB;
                            //TODO Search more info on osvdb?
                            RefJA.VocabularyID=iVocabularySAINT;
                            RefJA.timestamp = DateTimeOffset.Now;
                            RefJA = REFERENCENormalize(model, RefJA);
                            model.SaveChanges();
                            */
                            osvdbID = syn2.ToList().First();    //.ReferenceID;
                        }
                        else
                        {
                            //Add the OSVDB Reference                            
                            RefJA.Source = "OSVDB";
                            RefJA.ReferenceSourceID = myOSVDB;
                            RefJA.ReferenceTitle = myOSVDB;
                            RefJA.ReferenceURL = "http://osvdb.org/" + myOSVDB;
                            //TODO Search more info on osvdb?
                            RefJA.CreatedDate = DateTimeOffset.Now;
                            RefJA.timestamp = DateTimeOffset.Now;
                            RefJA.VocabularyID = iVocabularySAINT;
                            model.REFERENCE.Add(REFERENCENormalize(model, RefJA));
                            model.SaveChanges();
                            osvdbID = RefJA.ReferenceID;
                            Console.WriteLine("DEBUG Added OSVDB REFERENCE " + osvdbID);
                        }

                        //Check if the EXPLOITFORREFERENCE already exists in the database
                        //TODO optimize
                        var syn3 = from S in model.EXPLOITFORREFERENCE
                                   where S.ExploitID.Equals(sploit.ExploitID)
                                   && S.ReferenceID.Equals(osvdbID)
                                   select S.ExploitReferenceID;
                        if (syn3.Count() == 0)
                        {
                            Console.WriteLine("DEBUG Adding EXPLOITFORREFERENCE");
                            try
                            {
                                EXPLOITFORREFERENCE sploitref = new EXPLOITFORREFERENCE();
                                sploitref.ExploitID = sploit.ExploitID;
                                sploitref.ReferenceID = osvdbID;
                                sploitref.CreatedDate = DateTimeOffset.Now;
                                sploitref.timestamp = DateTimeOffset.Now;
                                sploitref.VocabularyID = iVocabularySAINT;
                                model.EXPLOITFORREFERENCE.Add(sploitref);
                                model.SaveChanges();    //TEST PERFORMANCE
                            }
                            catch(Exception exSAINTEXPLOITFORREFERENCEOSVDB)
                            {
                                Console.WriteLine("Exception: exSAINTEXPLOITFORREFERENCEOSVDB " + exSAINTEXPLOITFORREFERENCEOSVDB.Message + " " + exSAINTEXPLOITFORREFERENCEOSVDB.InnerException);
                            }
                        }
                        else
                        {
                            //Update EXPLOITFORREFERENCE
                        }
                    }

                    //****************************************************************
                    //  BID
                    string myBID = HelperGetChildInnerText(n, "bid");
                    if (myBID != "")
                    {
                        //Check if the BID reference already exists in the database
                        //TODO optimize
                        int bidID = 0;
                        var syn2 = from S in model.REFERENCE
                                   where S.Source.Equals("BID")
                                   && S.ReferenceSourceID.Equals(myBID)
                                   select S.ReferenceID;
                        if (syn2.Count() != 0)
                        {
                            bidID = syn2.ToList().First();  //.ReferenceID;
                            //Update REFERENCE BID
                        }
                        else
                        {
                            //Add the BID Reference
                            REFERENCE RefJA = new REFERENCE();
                            RefJA.Source = "BID";
                            RefJA.ReferenceSourceID = myBID;
                            RefJA.ReferenceTitle = myBID;   //Should be updated at some point
                            RefJA.ReferenceURL = "http://securityfocus.com/bid/" + myBID;
                            RefJA.CreatedDate = DateTimeOffset.Now;
                            RefJA.timestamp = DateTimeOffset.Now;
                            RefJA.VocabularyID=iVocabularySAINT;
                            model.REFERENCE.Add(REFERENCENormalize(model, RefJA));
                            model.SaveChanges();
                            bidID = RefJA.ReferenceID;
                        }

                        //Check if the EXPLOITFORREFERENCE already exists in the database
                        var syn3 = from S in model.EXPLOITFORREFERENCE
                                   where S.ExploitID.Equals(sploit.ExploitID)
                                   && S.ReferenceID.Equals(bidID)
                                   select S.ExploitReferenceID;
                        if (syn3.Count() == 0)
                        {
                            try
                            {
                                EXPLOITFORREFERENCE sploitref = new EXPLOITFORREFERENCE();
                                sploitref.ExploitID = sploit.ExploitID;
                                sploitref.ReferenceID = bidID;
                                sploitref.VocabularyID = iVocabularySAINT;
                                sploitref.CreatedDate = DateTimeOffset.Now;
                                sploitref.timestamp = DateTimeOffset.Now;
                                model.EXPLOITFORREFERENCE.Add(sploitref);
                                model.SaveChanges();
                            }
                            catch (Exception exSAINTEXPLOITFORREFERENCEBID)
                            {
                                Console.WriteLine("Exception: exSAINTEXPLOITFORREFERENCEBID " + exSAINTEXPLOITFORREFERENCEBID.Message + " " + exSAINTEXPLOITFORREFERENCEBID.InnerException);
                            }
                        }
                        else
                        {
                            //Update EXPLOITFORREFERENCE
                        }



                    }

                    
                    #region exploitreferencems
                    Regex RegexMS = new Regex("MS[0-9][0-9]-[0-9][0-9][1-9]", RegexOptions.IgnoreCase);
                    string strTemp = RegexMS.Match(sploit.ExploitName).ToString();
                    if (strTemp == "")
                    {
                        strTemp = RegexMS.Match(sploit.ExploitDescription).ToString();
                    }
                    if (strTemp == "")
                    {
                        strTemp = RegexMS.Match(sSaintID).ToString();
                    }
                    if (strTemp == "")
                    {
                        //Special regex
                        //<saint_id>win_patch_ms04011</saint_id>
                        Regex RegexMSSaint = new Regex("MS[0-9][0-9][0-9][0-9][1-9]", RegexOptions.IgnoreCase);
                        string strTemp2 = RegexMSSaint.Match(sSaintID).ToString();
                        if (strTemp2 != "")
                        {
                            strTemp = strTemp2.Substring(0, 4) + "-" + strTemp2.Substring(4, 3);
                        }
                    }
                    if (strTemp != "")
                    {
                        strTemp = strTemp.ToUpper();
                        Console.WriteLine("DEBUG MSpatch=" + strTemp);

                        int iReferenceMSID = 0;
                        try
                        {
                            //MS
                            iReferenceMSID = model.REFERENCE.Where(o => o.ReferenceTitle == strTemp).Select(o=>o.ReferenceID).FirstOrDefault();
                        }
                        catch (Exception ex)
                        {

                        }
                        if (iReferenceMSID <= 0)
                        {
                            Console.WriteLine("DEBUG Adding a REFERENCE for MS " + strTemp);
                            try
                            {
                                REFERENCE oReferenceMS = new REFERENCE();
                                oReferenceMS.CreatedDate = DateTimeOffset.Now;
                                oReferenceMS.Source = "MS";
                                oReferenceMS.ReferenceSourceID = strTemp;
                                oReferenceMS.ReferenceTitle = strTemp;
                                oReferenceMS.ReferenceURL = "https://technet.microsoft.com/library/security/" + strTemp;
                                //"http://www.microsoft.com/technet/security/bulletin/" + strTemp;
                                oReferenceMS.VocabularyID = iVocabularySAINT;   //SAINT
                                oReferenceMS.timestamp = DateTimeOffset.Now;
                                model.REFERENCE.Add(oReferenceMS);
                                model.SaveChanges();
                                iReferenceMSID = oReferenceMS.ReferenceID;
                            }
                            catch (Exception exoReferenceMS)
                            {
                                Console.WriteLine("Exception: exoReferenceMS " + exoReferenceMS.Message + " " + exoReferenceMS.InnerException);
                            }
                        }

                        int iExploitReferenceID = 0;
                        try
                        {
                            iExploitReferenceID = model.EXPLOITFORREFERENCE.Where(o => o.ExploitID == sploit.ExploitID && o.ReferenceID == iReferenceMSID).Select(o=>o.ExploitReferenceID).FirstOrDefault();
                        }
                        catch (Exception ex)
                        {

                        }
                        if (iExploitReferenceID <= 0)
                        {
                            try
                            {
                                EXPLOITFORREFERENCE oExploitReferenceMS = new EXPLOITFORREFERENCE();
                                oExploitReferenceMS.CreatedDate = DateTimeOffset.Now;
                                oExploitReferenceMS.ExploitID = sploit.ExploitID;
                                oExploitReferenceMS.ReferenceID = iReferenceMSID;
                                oExploitReferenceMS.VocabularyID = iVocabularySAINT;    //SAINT
                                oExploitReferenceMS.timestamp = DateTimeOffset.Now;
                                model.EXPLOITFORREFERENCE.Add(oExploitReferenceMS);
                                model.SaveChanges();    //TEST PERFORMANCE

                            }
                            catch (Exception exoExploitReferenceMS)
                            {
                                Console.WriteLine("Exception: exoExploitReferenceMS " + exoExploitReferenceMS.Message + " " + exoExploitReferenceMS.InnerException);
                            }
                        }
                        else
                        {
                            //Update EXPLOITFORREFERENCE
                        }

                    }
                    #endregion exploitreferencems

                    
                //}
            }

            //TODO
            //http://www.saintcorporation.com/cgi-bin/demo_doc.pl?doc_name=cve_2013.html

            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
            Console.WriteLine("DEBUG FINISHED Import SAINT");
            //FREE
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
                Console.WriteLine("Exception: DbEntityValidationExceptionFINALSAVESAINT " + sb.ToString());
            }
            catch (Exception exFINALSAVE)
            {
                Console.WriteLine("Exception: exFINALSAVESAINT " + exFINALSAVE.Message + " " + exFINALSAVE.InnerException);
            }
            model.Dispose();
            model = null;
        }
            
        static private string HelperGetChildInnerText(XmlNode n, string ChildName)
        {
            foreach (XmlNode child in n.ChildNodes)
            {
                if (child.Name.ToUpper() == ChildName.ToUpper())
                    return child.InnerText;
            }
            return string.Empty;
        }

        //***************************************************************************************
        static private void Import_CVEAIXAPAR(string sSourceFilePath = @"C:\nvdcve\source-AIXAPAR.html")
        {
            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
            Console.WriteLine("DEBUG Import AIXAPAR from CVE...");

            XORCISMEntities model=new XORCISMEntities();
            model.Configuration.AutoDetectChangesEnabled = false;
            model.Configuration.ValidateOnSaveEnabled = false;

            Regex myRegexAIXAPAR = new Regex("AIXAPAR:[^<>]*");
            Regex myRegexCVE = new Regex("CVE-[0-9][0-9][0-9][0-9]-[0-9][0-9][0-9][0-9]");  //TODO Review \d+

            StreamReader monStreamReader = new StreamReader(sSourceFilePath);
            string strTemp = string.Empty;
            string sAIXAPARID = string.Empty;
            REFERENCE oREFERENCE = new REFERENCE();
            string sCVEID = string.Empty;
            string ligne = monStreamReader.ReadLine();
            //Console.WriteLine(ligne);
            while (ligne != null)
            {
                strTemp = myRegexAIXAPAR.Match(ligne).ToString();
                if (strTemp != "")
                {
                    strTemp = strTemp.Replace("AIXAPAR:", "");
                    sAIXAPARID = strTemp.Replace("</td>", "");
                    //Console.WriteLine(fichiers[i]);
                    Console.WriteLine("DEBUG sAIXAPARID:" + sAIXAPARID);

                    //Check if the REFERENCE exists
                    oREFERENCE = new REFERENCE();
                    oREFERENCE = model.REFERENCE.FirstOrDefault(o => o.Source == "AIXAPAR" && o.ReferenceTitle == sAIXAPARID);
                    if (oREFERENCE == null)
                    {
                        //TODO: Check by Url
                        try
                        {
                            oREFERENCE = new REFERENCE();
                            oREFERENCE.Source = "AIXAPAR";
                            oREFERENCE.ReferenceSourceID = sAIXAPARID;
                            oREFERENCE.ReferenceTitle = sAIXAPARID;
                            oREFERENCE.ReferenceURL = "http://www-1.ibm.com/support/search.wss?rs=0&q=" + sAIXAPARID + "&apar=only";
                            //TODO: VocabularyID=CVE
                            oREFERENCE.CreatedDate = DateTimeOffset.Now;
                            oREFERENCE.timestamp = DateTimeOffset.Now;
                            //Here don't normalize to keep AIXAPAR, otherwise IBM   //TODO Review
                            model.REFERENCE.Add(oREFERENCE);
                            model.SaveChanges();
                            Console.WriteLine("DEBUG Added REFERENCE for AIXAPAR:" + sAIXAPARID);

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Exception: AIXAPAR " + ex.Message + " " + ex.InnerException);
                        }
                    }
                    else
                    {
                        //Update REFERENCE
                    }
                    Console.WriteLine("DEBUG ReferenceID:" + oREFERENCE.ReferenceID);
                }
                else
                {
                    //Update REFERENCE
                    strTemp = myRegexCVE.Match(ligne).ToString();
                    if (strTemp != "")
                    {
                        sCVEID = strTemp;    // "CVE-" + strTemp.Replace("'", "");
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
                        catch(Exception ex)
                        {

                        }
                        //Console.WriteLine("DEBUGTEST");
                        //if (myCVE == null)
                        if (iVulnerabilityID<=0)
                        {
                            Console.WriteLine("ERROR: Problem: this CVE is unknown :" + sCVEID);
                            //TODO Add VULNERABILITY
                        }
                        else
                        {
                            Console.WriteLine("DEBUG VulnerabilityID:" + iVulnerabilityID);
                        }
                        //Mapping VULNERABILITYFORREFERENCE
                        //Check if the VULNERABILITYFORREFERENCE already exists
                        ////var VulnRef = myCVE.REFERENCE.FirstOrDefault(o => o.ReferenceID == oREFERENCE.ReferenceID);
                        //VULNERABILITYFORREFERENCE oVULREFERENCE = model.VULNERABILITYFORREFERENCE.FirstOrDefault(o => o.VulnerabilityID == myCVE.VulnerabilityID && o.ReferenceID == oREFERENCE.ReferenceID);
                        int iVulnerabilityReferenceID = 0;
                        try
                        {
                            iVulnerabilityReferenceID = vuln_model.VULNERABILITYFORREFERENCE.Where(o => o.VulnerabilityID == iVulnerabilityID && o.ReferenceID == oREFERENCE.ReferenceID).Select(o=>o.VulnerabilityReferenceID).FirstOrDefault();
                        }
                        catch(Exception ex)
                        {

                        }
                        
                        //if (oVULREFERENCE == null)
                        if (iVulnerabilityReferenceID<=0)
                        {
                            try
                            {
                                //myCVE.REFERENCE.Add(oREFERENCE);
                                VULNERABILITYFORREFERENCE oVULREFERENCE = new VULNERABILITYFORREFERENCE();
                                oVULREFERENCE.VulnerabilityID = iVulnerabilityID; // myCVE.VulnerabilityID;
                                oVULREFERENCE.ReferenceID = oREFERENCE.ReferenceID;
                                oVULREFERENCE.CreatedDate = DateTimeOffset.Now;
                                oVULREFERENCE.timestamp = DateTimeOffset.Now;
                                oVULREFERENCE.VocabularyID = iVocabularyCVENVD;
                                vuln_model.VULNERABILITYFORREFERENCE.Add(oVULREFERENCE);
                                vuln_model.SaveChanges();
                                Console.WriteLine("DEBUG Added VULNERABILITYFORREFERENCE for AIXAPAR:" + sAIXAPARID + " " + sCVEID);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Exception: VULNERABILITYFORREFERENCEAIXAPAR " + ex.Message + " " + ex.InnerException);
                            }
                        }
                        else
                        {
                            //Update VULNERABILITYFORREFERENCE
                        }
                    }
                }

                ligne = monStreamReader.ReadLine();

            }//EOF
            monStreamReader.Close();

            Console.WriteLine("AIXAPAR/CVE IMPORT FINISHED - SECURE?");
            //FREE
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
                Console.WriteLine("Exception: DbEntityValidationExceptionFINALSAVEAIXPAR " + sb.ToString());
            }
            catch (Exception exFINALSAVE)
            {
                Console.WriteLine("Exception: exFINALSAVEAIXPAR " + exFINALSAVE.Message + " " + exFINALSAVE.InnerException);
            }
            model.Dispose();
            model = null;
        }

        //***************************************************************************************
        static private void Import_CVEEXPLOITDB()
        {
            //TODO
            //http://cve.mitre.org/data/refs/index.html
            //http://cve.mitre.org/data/refs/refmap/allrefmaps.zip
            //http://cve.mitre.org/data/refs/refmap/source-EXPLOIT-DB.html
            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
            Console.WriteLine("Import EXPLOIT-DB from CVE...");

            XORCISMEntities model=new XORCISMEntities();
            model.Configuration.AutoDetectChangesEnabled = false;
            model.Configuration.ValidateOnSaveEnabled = false;

            Regex myRegexEXPLOITDB = new Regex("EXPLOIT-DB:[^<>]*");
            Regex myRegexCVE = new Regex("CVE-[0-9][0-9][0-9][0-9]-[0-9][0-9][0-9][0-9]");  //TODO Review \d+

            StreamReader monStreamReader = new StreamReader(@"C:\nvdcve\source-EXPLOIT-DB.html");
            string strTemp = string.Empty;
            string sEXPLOITDBID = string.Empty;
            int ExploitID = 0;
            string sCVEID = string.Empty;
            string ligne = monStreamReader.ReadLine();
            //Console.WriteLine(ligne);
            while (ligne != null)
            {

                strTemp = myRegexEXPLOITDB.Match(ligne).ToString();
                if (strTemp != "")
                {
                    strTemp = strTemp.Replace("EXPLOIT-DB:", "");
                    sEXPLOITDBID = strTemp.Replace("</td>", "");
                    //Console.WriteLine(fichiers[i]);
                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                    Console.WriteLine("DEBUG EXPLOIT-DB:" + sEXPLOITDBID);

                    
                    
                    //Check if the sploit already exist in the db
                    //TODO optimize
                    var syn = from S in model.EXPLOIT
                              where S.ExploitReferential.Equals("exploit-db")
                              && S.ExploitRefID.Equals(sEXPLOITDBID)
                              select S.ExploitID;
                    if (syn.Count() != 0)
                    {
                        ExploitID = syn.ToList().First();   //.ExploitID;
                        //Update EXPLOIT
                    }
                    else
                    {
                        //TODO: Visit the exploit page to find EXPLOITFORREFERENCE (osvdb), Name...
                        try
                        {
                            EXPLOIT sploit = new EXPLOIT();
                            sploit.ExploitReferential = "exploit-db";
                            sploit.ExploitRefID = sEXPLOITDBID;
                            //sploit.Name = sploitname; //TODO Search on exploit-db
                            sploit.ExploitLocation = "http://exploit-db.com/exploits/" + sEXPLOITDBID;
                            
                            //sploit.Date = sploitdate;
                            //sploit.Verification
                            //sploit.Platform = sploitplatform;
                            //sploit.Author = sploitauthor;
                            sploit.CreatedDate = DateTimeOffset.Now;
                            sploit.timestamp = DateTimeOffset.Now;
                            sploit.VocabularyID = iVocabularyCVENVD;
                            model.EXPLOIT.Add(sploit);
                            model.SaveChanges();
                            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                            Console.WriteLine("DEBUG Added EXPLOIT EXPLOIT-DB:" + sEXPLOITDBID);
                            ExploitID = sploit.ExploitID;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Exception: AddToEXPLOIT" + ex.Message + " " + ex.InnerException);
                        }


                    }
                    /*
                    try
                    {
                        model.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Exception: AddToEXPLOITSaveChanges" + ex.Message+" "+ex.InnerException);
                    }
                    */
                }
                else
                {
                    strTemp = myRegexCVE.Match(ligne).ToString();
                    if (strTemp != "")
                    {
                        //strTemp = strTemp.Replace("'CVE', '", "");
                        sCVEID = strTemp;    // "CVE-" + strTemp.Replace("'", "");
                        //Console.WriteLine(fichiers[i]);
                        Console.WriteLine("DEBUG CVE:" + sCVEID);
                        /*
                        var myCVE = from QR in model.VULNERABILITY
                                              where QR.Value.Equals(sCVEID)
                                              select QR;
                        int vulnID = myCVE.ToList().First().VulnerabilityID;
                        */
                        //VULNERABILITY myCVE = new VULNERABILITY();
                        int vulnID = 0;
                        try
                        {
                            //TODO optimize
                            //myCVE = model.VULNERABILITY.FirstOrDefault(o => o.VULReferentialID == sCVEID);
                            try
                            {
                                vulnID = vuln_model.VULNERABILITY.Where(o => o.VULReferentialID == sCVEID).Select(o=>o.VulnerabilityID).FirstOrDefault();
                            }
                            catch(Exception ex)
                            {

                            }
                            //if (myCVE != null)
                            if (vulnID>0)
                            {
                                //vulnID = myCVE.VulnerabilityID;
                                //Update VULNERABILITY
                            }
                            else
                            {
                                Console.WriteLine("Error: CVE not found. sCVEID:" + sCVEID+". Adding it");
                                
                                try
                                {
                                    VULNERABILITY myCVE = new VULNERABILITY();
                                    //TODO: Review this
                                    myCVE.VULReferentialID = sCVEID;
                                    myCVE.VULReferential = "cve";
                                    myCVE.CreatedDate = DateTimeOffset.Now;
                                    myCVE.timestamp = DateTimeOffset.Now;
                                    myCVE.VocabularyID = iVocabularyCVENVD;
                                    vuln_model.VULNERABILITY.Add(myCVE);
                                    vuln_model.SaveChanges();
                                    vulnID = myCVE.VulnerabilityID;
                                }
                                catch (Exception exAddToVULNERABILITYMissing)
                                {
                                    Console.WriteLine("Exception: exAddToVULNERABILITYMissing " + exAddToVULNERABILITYMissing.Message + " " + exAddToVULNERABILITYMissing.InnerException);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Exception: CVE not found. sCVEID:" + sCVEID + ex.Message + " " + ex.InnerException);
                        }

                        //Console.WriteLine("DEBUGTEST");

                        
                        //Check if the EXPLOITFORVULNERABILITY exists in the database
                        //TODO optimize
                        var syn2 = from S in model.EXPLOITFORVULNERABILITY
                                   where S.VulnerabilityID.Equals(vulnID)
                                   && S.ExploitID.Equals(ExploitID)
                                   select S.VulnerabilityExploitID;
                        if (syn2.Count() != 0)
                        {
                            //Update EXPLOITFORVULNERABILITY
                        }
                        else
                        {
                            
                            try
                            {
                                EXPLOITFORVULNERABILITY sploitvuln = new EXPLOITFORVULNERABILITY();
                                sploitvuln.VulnerabilityID = vulnID;    //CVE
                                sploitvuln.ExploitID = ExploitID;   //sploit.ExploitID;
                                sploitvuln.CreatedDate = DateTimeOffset.Now;
                                sploitvuln.timestamp = DateTimeOffset.Now;
                                sploitvuln.VocabularyID = iVocabularyCVENVD;
                                model.EXPLOITFORVULNERABILITY.Add(sploitvuln);
                                model.SaveChanges();
                                Console.WriteLine("DEBUG Added EXPLOIT-DB:" + sEXPLOITDBID + " (ExploitID=" + ExploitID + ") for " + sCVEID + " (vulnID=" + vulnID + ")");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Exception: AddToEXPLOITFORVULNERABILITY" + ex.Message+" "+ex.InnerException);
                            }
                        }



                    }
                }
                ligne = monStreamReader.ReadLine();

            }//EOF
            monStreamReader.Close();

            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
            Console.WriteLine("DEBUG EXPLOIT-DB/CVE IMPORT FINISHED - HACK THEM ALL!");
            //FREE
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
                Console.WriteLine("Exception: DbEntityValidationExceptionFINALSAVEEXPLOITDB " + sb.ToString());
            }
            catch (Exception exFINALSAVE)
            {
                Console.WriteLine("Exception: exFINALSAVEEXPLOITDB " + exFINALSAVE.Message + " " + exFINALSAVE.InnerException);
            }
            model.Dispose();
            model = null;
        }

        //***************************************************************************************
        static private void Import_metasploit()
        {
            Console.WriteLine("DEBUG ***********************************************");
            Console.WriteLine("DEBUG "+DateTimeOffset.Now);
            Console.WriteLine("DEBUG Import Metasploit...");
            //TODO: Update Metasploit
            //C:\metasploit\dev_msfupdate.bat

            XORCISMEntities model=new XORCISMEntities();
            model.Configuration.AutoDetectChangesEnabled = false;
            model.Configuration.ValidateOnSaveEnabled = false;

            Regex myRegexCVE = new Regex("'CVE', '[0-9][0-9][0-9][0-9]-[0-9][0-9][0-9][0-9]'"); //TODO REVIEW \d+
            Regex myRegexOSVDB = new Regex("'OSVDB', '[^<>]*'");
            Regex myRegexBID = new Regex("'BID', '[^<>]*'");
            Regex myRegexURL = new Regex("'URL', '[^<>]*'");
            //TODO
            Regex myRegexEDB = new Regex("'EDB', '[^<>]*'");    //exploit-db
            
            //TODO
            //String[] fichiers = Directory.GetFiles(@"C:\Program Files (x86)\Rapid7\framework\msf3\modules\exploits", "*.rb", SearchOption.AllDirectories);
            String[] fichiers = Directory.GetFiles(@"C:\metasploit\apps\pro\msf3\modules\exploits", "*.rb", SearchOption.AllDirectories);   //TODO Hardcoded
            
            for (int i = 0 ; i < fichiers.Length ; i++)
            {
                string strTemp = string.Empty;
                string myCVE = string.Empty;
                string myOSVDB = string.Empty;
                string myBID = string.Empty;
                string myEDB = string.Empty;
                string myURL1 = string.Empty;
                string myURL2 = string.Empty;
                string myURL3 = string.Empty;
                string myURL4 = string.Empty;
                string myURL5 = string.Empty;
                string myURL6 = string.Empty;
                string myURL7 = string.Empty;
                string myURL8 = string.Empty;
                string myURL9 = string.Empty;

                //Console.Out.WriteLine(fichiers[i]);
                Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                Console.WriteLine("DEBUG Parsing Metasploit module "+fichiers[i]);

                StreamReader monStreamReader = new StreamReader(fichiers[i]);
                string ligne = monStreamReader.ReadLine();    

                while (ligne != null)
                {
                    //TODO
                    Regex myRegex = new Regex("'Name'");
                    strTemp = myRegex.Match(ligne).ToString();
                    if (strTemp != "")
                    {
                        myRegex = new Regex("=> '[^<>]*',");
                        strTemp = myRegex.Match(ligne).ToString();
                        strTemp = strTemp.Replace("=> '", "");
                        strTemp = strTemp.Replace("',", "");
                        if (strTemp.Trim() == "")
                        {
                            //Example: cydia_default_ssh.rb
                            myRegex = new Regex("=> \"[^<>]*\",");
                            strTemp = myRegex.Match(ligne).ToString();
                            strTemp = strTemp.Replace("=> \"", "");
                            strTemp = strTemp.Replace("\",", "");
                        }
                        Console.WriteLine("DEBUG Metasploit module Name:" + strTemp);
                    }

                    //TODO
                    myRegex = new Regex("'Author'");        
                    strTemp = myRegex.Match(ligne).ToString();
                    if (strTemp != "")
                    {
                        myRegex = new Regex(Regex.Escape("[")+" '[^<>]*' ],");
                        strTemp = myRegex.Match(ligne).ToString();
                        strTemp = strTemp.Replace("[ '", "");
                        strTemp = strTemp.Replace("' ],", "");
                        Console.WriteLine("DEBUG Metasploit module Author:"+strTemp);
                        //TODO: split (multiple author)
                        //skape', 'trew
                        //hdm', 'cazz

                        //TODO multiple lines
                        /*
                        'Author'         =>
                        [
                          'hdm'
                        ],  
                        */
                    }

                    //TODO
                    myRegex = new Regex("'Platform'");
                    strTemp = myRegex.Match(ligne).ToString();
                    if (strTemp != "")
                    {
                        myRegex = new Regex("=> '[^<>]*',");
                        strTemp = myRegex.Match(ligne).ToString();
                        strTemp = strTemp.Replace("=> '", "");
                        strTemp = strTemp.Replace("',", "");
                        if (strTemp.Trim() == "")
                        {
                            myRegex = new Regex("=> \"[^<>]*\",");
                            strTemp = myRegex.Match(ligne).ToString();
                            strTemp = strTemp.Replace("=> \"", "");
                            strTemp = strTemp.Replace("\",", "");
                        }
                        if (strTemp.Trim() == "")
                        {
                            myRegex = new Regex("=> '^<>]*',");
                            strTemp = myRegex.Match(ligne).ToString();
                            strTemp = strTemp.Replace("=> '", "");
                            strTemp = strTemp.Replace("',", "");
                        }
                        Console.WriteLine("DEBUG Metasploit module Platform:" + strTemp);
                    }
                    //TODO
                    //'AIX'      => '5.1'


                    //TODO REVIEW
                    myRegex = new Regex("'DisclosureDate'");
                    //'DisclosureDate' => 'Oct 07 2009'
                    strTemp = myRegex.Match(ligne).ToString();
                    if (strTemp != "")
                    {
                        myRegex = new Regex("=> '[^<>]*',");
                        strTemp = myRegex.Match(ligne).ToString();
                        strTemp = strTemp.Replace("=> '", "");
                        strTemp = strTemp.Replace("',", "");
                        if (strTemp.Trim() == "")
                        {
                            myRegex = new Regex("=> \"[^<>]*\",");
                            strTemp = myRegex.Match(ligne).ToString();
                            strTemp = strTemp.Replace("=> \"", "");
                            strTemp = strTemp.Replace("\",", "");
                        }
                        if (strTemp.Trim() == "")
                        {
                            myRegex = new Regex("=> '^<>]*',");
                            strTemp = myRegex.Match(ligne).ToString();
                            strTemp = strTemp.Replace("=> '", "");
                            strTemp = strTemp.Replace("',", "");
                        }
                        Console.WriteLine("DEBUG Metasploit module DisclosureDate:" + strTemp);
                    }

                    strTemp = myRegexCVE.Match(ligne).ToString();
                    if (strTemp != "")
                    {
                        strTemp = strTemp.Replace("'CVE', '", "");
                        myCVE = "CVE-"+strTemp.Replace("'", "");
                        //Console.WriteLine(fichiers[i]);
                        Console.WriteLine("DEBUG Metasploit module CVE:"+myCVE);
                    }
                    else
                    {
                        strTemp = myRegexOSVDB.Match(ligne).ToString();
                        if (strTemp != "")
                        {
                            strTemp = strTemp.Replace("'OSVDB', '", "");
                            myOSVDB = strTemp.Replace("'", "");
                            //Cleaning 
                            ////myOSVDB = myOSVDB.Replace("http://osvdb.org", "http://www.osvdb.org");
                            myOSVDB = myOSVDB.Replace("https://www.", "http://");   //Review
                            myOSVDB = myOSVDB.Replace("http://www.", "http://");
                            myOSVDB = myOSVDB.Replace("http://osvdb.org/displayvuln.php?osvdbid=", "http://osvdb.org/");
                            myOSVDB = myOSVDB.Replace("http://osvdb.org/show/osvdb/", "http://osvdb.org/");
                            //TODO Normalize
                            //Console.WriteLine(fichiers[i]);
                            Console.WriteLine("DEBUG Metasploit module OSVDB:"+myOSVDB);
                        }
                        else
                        {
                            strTemp = myRegexBID.Match(ligne).ToString();
                            if (strTemp != "")
                            {
                                strTemp = strTemp.Replace("'BID', '", "");
                                myBID = strTemp.Replace("'", "");
                                //Console.WriteLine(fichiers[i]);
                                Console.WriteLine("DEBUG Metasploit module BID:"+myBID);
                            }
                            else
                            {
                                strTemp = myRegexURL.Match(ligne).ToString();
                                if (strTemp == "")
                                {
                                    //TODO
                                    //exploit-db
                                    //[ 'EDB', '23831' ]
                                    //Console.WriteLine("ERROR: (TODO) " + ligne + " does not match a URL");

                                }
                                else
                                {
                                    strTemp = strTemp.Replace("'URL', '", "");
                                    strTemp = strTemp.Replace("http://www.", "http://");
                                    strTemp = strTemp.Replace("https://www.", "https://");
                                    strTemp = strTemp.Replace("http://osvdb.org/displayvuln.php?osvdbid=", "http://osvdb.org/");
                                    strTemp = strTemp.Replace("http://osvdb.org/show/osvdb/", "http://osvdb.org/");
                                    //TODO Normalize
                                    //securityreason.com
                                    //attrition.org
                                    if (myURL1 == "")
                                    {
                                        myURL1 = strTemp.Replace("'", "");
                                        Console.WriteLine("DEBUG Metasploit module URL1:" + myURL1);
                                    }
                                    else
                                    {
                                        if (myURL2 == "")
                                        {
                                            myURL2 = strTemp.Replace("'", "");
                                            Console.WriteLine("DEBUG Metasploit module URL2:" + myURL2);
                                        }
                                        else
                                        {
                                            if (myURL3 == "")
                                            {
                                                myURL3 = strTemp.Replace("'", "");
                                                Console.WriteLine("DEBUG Metasploit module URL3:" + myURL3);
                                            }
                                            else
                                            {
                                                if (myURL4 == "")
                                                {
                                                    myURL4 = strTemp.Replace("'", "");
                                                    Console.WriteLine("DEBUG Metasploit module URL4:" + myURL4);
                                                }
                                                else
                                                {
                                                    if (myURL5 == "")
                                                    {
                                                        myURL5 = strTemp.Replace("'", "");
                                                        Console.WriteLine("DEBUG Metasploit module URL5:" + myURL5);
                                                    }
                                                    else
                                                    {
                                                        if (myURL6 == "")
                                                        {
                                                            myURL6 = strTemp.Replace("'", "");
                                                            //Console.WriteLine("URL6:" + myURL6);
                                                        }
                                                        else
                                                        {
                                                            if (myURL7 == "")
                                                            {
                                                                myURL7 = strTemp.Replace("'", "");
                                                                //Console.WriteLine("URL7:" + myURL7);
                                                            }
                                                            else
                                                            {
                                                                if (myURL8 == "")
                                                                {
                                                                    myURL8 = strTemp.Replace("'", "");
                                                                    //Console.WriteLine("URL8:" + myURL8);
                                                                }
                                                                else
                                                                {
                                                                    if (myURL9 == "")
                                                                    {
                                                                        myURL9 = strTemp.Replace("'", "");
                                                                        //Console.WriteLine("URL9:" + myURL9);
                                                                    }
                                                                    else
                                                                    {
                                                                        Console.WriteLine("ERROR: URL10 FOUND!!! in " + fichiers[i]);
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    
                    //TODO EXPLOITPARAMETER ...



                    ligne = monStreamReader.ReadLine();
                }
                //EOF
                
                

                //int vulnID = 0;
                int iVulnerabilityID = 0;
                if (myCVE != "")
                {
                    // optimize
                    //VULNERABILITY oCVE = model.VULNERABILITY.FirstOrDefault(o => o.VULReferential == "cve" && o.VULReferentialID == myCVE);
                    
                    try
                    {
                        iVulnerabilityID = vuln_model.VULNERABILITY.Where(o => o.VULReferential == "cve" && o.VULReferentialID == myCVE).Select(o=>o.VulnerabilityID).FirstOrDefault();
                    }
                    catch(Exception ex)
                    {

                    }
                    //if (oCVE!=null)
                    if (iVulnerabilityID>0)
                    {
                        //Update VULNERABILITY
                        //vulnID = oCVE.VulnerabilityID;
                    }
                    else
                    {
                        Console.WriteLine("DEBUG CVE not found! " + myCVE + " " + fichiers[i]+" adding it as CANDIDATE");
                        //CVE-2009-0695     CANDIDATE
                        //CVE-2005-0491     CANDIDATE
                        //CVE-2005-0260     CANDIDATE
                        //CVE-2005-0043     CANDIDATE
                        //CVE-2005-0455
                        //CVE-2009-3028
                        //CVE-2005-0059
                        //CVE-2005-0308
                        //CVE-2005-0277
                        //CVE-2005-0353

                        VULNERABILITY oCVE = new VULNERABILITY();
                        oCVE.VULReferential = "cve";
                        oCVE.VULReferentialID = myCVE;
                        oCVE.VULDescription = "CANDIDATE";
                        //TODO URL?
                        oCVE.CreatedDate = DateTimeOffset.Now;
                        oCVE.timestamp = DateTimeOffset.Now;
                        //canCVE.VocabularyID=  //TODO metasploit
                        vuln_model.VULNERABILITY.Add(oCVE);
                        vuln_model.SaveChanges();
                        //vulnID = oCVE.VulnerabilityID;
                        iVulnerabilityID = oCVE.VulnerabilityID;
                        //    return;
                    }
                }

                string myRefID = Path.GetFileName(fichiers[i]); //TODO Review vs exploit-db
                //Check if the exploit already exists in the database
                //TODO optimize
                //EXPLOIT sploit = model.EXPLOIT.FirstOrDefault(o => o.ExploitReferential == "metasploit" && o.ExploitRefID == myRefID);
                int iExploitID = 0;
                try
                {
                    iExploitID = model.EXPLOIT.Where(o => o.ExploitReferential == "metasploit" && o.ExploitRefID == myRefID).Select(o=>o.ExploitID).FirstOrDefault();
                }
                catch(Exception ex)
                {

                }
                //if (sploit==null)
                if (iExploitID<=0)
                {
                    

                    try
                    {
                        EXPLOIT sploit = new EXPLOIT();
                        sploit.ExploitRefID = myRefID;
                        sploit.ExploitName = Path.GetFileName(fichiers[i]);
                        sploit.ExploitReferential = "metasploit";   //Hardcoded
                        sploit.ExploitLocation = Path.GetFullPath(fichiers[i]);    //TODO Review
                        sploit.CreatedDate = DateTimeOffset.Now;
                        sploit.VocabularyID = iVocabularyMetasploitID;
                        sploit.timestamp = DateTimeOffset.Now;
                        model.EXPLOIT.Add(sploit);
                        model.SaveChanges();
                        iExploitID = sploit.ExploitID;
                    }
                    catch(Exception exMETASPLOIT)
                    {
                        Console.WriteLine("Exception: exMETASPLOIT " + exMETASPLOIT.Message + " " + exMETASPLOIT.InnerException);
                    }
                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                    //Console.WriteLine("DEBUG AddToEXPLOIT : " + sploit.ExploitLocation);
                    
                }
                else
                {
                    //Update EXPLOIT
                    //sploit.timestamp = DateTimeOffset.Now;
                    //sploit.VocabularyID = iVocabularyMetasploitID;
                }
                

                //if (vulnID != 0)
                if (iVulnerabilityID>0)
                {
                    //Check if EXPLOITFORVULNERABILITY already exists in the database
                    //TODO optimize
                    //EXPLOITFORVULNERABILITY sploitvuln = model.EXPLOITFORVULNERABILITY.FirstOrDefault(o => o.ExploitID == sploit.ExploitID && o.VulnerabilityID == vulnID);
                    int iExploitVulnerabilityID = 0;
                    try
                    {
                        iExploitVulnerabilityID = model.EXPLOITFORVULNERABILITY.Where(o => o.ExploitID == iExploitID && o.VulnerabilityID == iVulnerabilityID).Select(o=>o.VulnerabilityExploitID).FirstOrDefault();
                    }
                    catch(Exception ex)
                    {

                    }
                    //if (sploitvuln == null)
                    if (iExploitVulnerabilityID<=0)
                    {
                        
                        try
                        {
                            Console.WriteLine("DEBUG New exploit for vulnID " + iVulnerabilityID);
                            EXPLOITFORVULNERABILITY sploitvuln = new EXPLOITFORVULNERABILITY();
                            sploitvuln.VulnerabilityID = iVulnerabilityID;
                            sploitvuln.ExploitID = iExploitID;
                            sploitvuln.CreatedDate = DateTimeOffset.Now;
                            //TODO send email
                            sploitvuln.timestamp = DateTimeOffset.Now;

                            sploitvuln.VocabularyID = iVocabularyMetasploitID;

                            model.EXPLOITFORVULNERABILITY.Add(sploitvuln);
                            //model.SaveChanges();    //TEST PERFORMANCE
                            //iExploitVulnerabilityID=
                        }
                        catch (Exception AddToEXPLOITFORVULNERABILITY05)
                        {
                            Console.WriteLine("Exception: AddToEXPLOITFORVULNERABILITY05 : " + AddToEXPLOITFORVULNERABILITY05.Message + " " + AddToEXPLOITFORVULNERABILITY05.InnerException);
                        }
                    }
                    else
                    {
                        //Update EXPLOITFORVULNERABILITY
                    }
                }

                //****************************************************************
                //  OSVDB
                if (myOSVDB != "")
                {
                    //Check if the OSVDB reference already exists in the database
                    //int osvdbID = 0;
                    
                    //REFERENCE RefJA = model.REFERENCE.FirstOrDefault(o => o.Source == "OSVDB" && o.ReferenceSourceID == myOSVDB);
                    int iReferenceID = 0;
                    try
                    {
                        iReferenceID = model.REFERENCE.Where(o => o.Source == "OSVDB" && o.ReferenceSourceID == myOSVDB).Select(o=>o.ReferenceID).FirstOrDefault();
                    }
                    catch(Exception ex)
                    {

                    }
                    //if (RefJA != null)
                    if (iReferenceID>0)
                    {
                        //Update REFERENCE
                        //TODO Remove/Comment
                        /*
                        //RefJA.ReferenceURL = "http://osvdb.org/" + myOSVDB;
                        RefJA.timestamp = DateTimeOffset.Now;
                        //RefJA.VocabularyID= //TODO metasploit
                        RefJA = REFERENCENormalize(model, RefJA);
                        model.SaveChanges();
                        */
                    }
                    else
                    {
                        //Add the OSVDB Reference
                        try
                        {
                            Console.WriteLine("DEBUG Adding OSVDB REFERENCE");
                            REFERENCE RefJA = new REFERENCE();
                            RefJA.Source = "OSVDB"; //Hardcoded
                            RefJA.ReferenceSourceID = myOSVDB;
                            RefJA.ReferenceTitle = myOSVDB;
                            RefJA.ReferenceURL = "http://osvdb.org/" + myOSVDB;
                            RefJA.CreatedDate = DateTimeOffset.Now;
                            RefJA.timestamp = DateTimeOffset.Now;
                            RefJA.VocabularyID = iVocabularyMetasploitID;
                            model.REFERENCE.Attach(RefJA);
                            model.Entry(RefJA).State = EntityState.Modified;
                            model.REFERENCE.Add(REFERENCENormalize(model, RefJA));

                            model.SaveChanges();
                            iReferenceID = RefJA.ReferenceID;
                        }
                        catch(Exception exREFERENCEOSVDB)
                        {
                            Console.WriteLine("Exception exREFERENCEOSVDB " + exREFERENCEOSVDB.Message + " " + exREFERENCEOSVDB.InnerException);
                        }
                    }
                    //osvdbID = RefJA.ReferenceID;


                    //Check if the EXPLOITFORREFERENCE already exists in the database
                    //TODO optimize
                    //EXPLOITFORREFERENCE sploitref = model.EXPLOITFORREFERENCE.FirstOrDefault(o => o.ExploitID == iExploitID && o.ReferenceID == iReferenceID);
                    int iExploitReferenceID = 0;
                    try
                    {
                        iExploitReferenceID = model.EXPLOITFORREFERENCE.Where(o => o.ExploitID == iExploitID && o.ReferenceID == iReferenceID).Select(o=>o.ExploitReferenceID).FirstOrDefault();
                    }
                    catch(Exception ex)
                    {

                    }
                    //if (sploitref == null)
                    if (iExploitReferenceID<=0)
                    {
                        try
                        {
                            EXPLOITFORREFERENCE sploitref = new EXPLOITFORREFERENCE();
                            sploitref.ExploitID = iExploitID;
                            sploitref.ReferenceID = iReferenceID;   // osvdbID;
                            sploitref.CreatedDate = DateTimeOffset.Now;
                            sploitref.timestamp = DateTimeOffset.Now;
                            sploitref.VocabularyID = iVocabularyMetasploitID;
                            model.EXPLOITFORREFERENCE.Add(sploitref);
                            model.SaveChanges();    //TEST PERFORMANCE
                            //iExploitReferenceID=
                        }
                        catch(Exception exEXPLOITFORREFERENCEMSF)
                        {
                            Console.WriteLine("Exception exEXPLOITFORREFERENCEMSF " + exEXPLOITFORREFERENCEMSF.Message + " " + exEXPLOITFORREFERENCEMSF.InnerException);
                        }
                    }
                    else
                    {
                        //Update EXPLOITFORREFERENCE
                    }
                }

                //****************************************************************
                //  BID
                if (myBID != "")
                {
                    //Check if the BID reference already exists in the database
                    // optimize
                    //int bidID = 0;
                    //REFERENCE RefJA = model.REFERENCE.FirstOrDefault(o => o.Source == "BID" && o.ReferenceSourceID == myBID);
                    int iReferenceID = 0;
                    try
                    {
                        iReferenceID = model.REFERENCE.Where(o => o.Source == "BID" && o.ReferenceSourceID == myBID).Select(o => o.ReferenceID).FirstOrDefault();
                    }
                    catch (Exception ex)
                    {

                    }
                    //if (RefJA != null)
                    if (iReferenceID > 0)
                    {
                        //Update REFERENCE
                        //bidID = RefJA.ReferenceID;
                        //RefJA = REFERENCENormalize(model, RefJA);
                    }
                    else
                    {
                        //Add the BID Reference
                        Console.WriteLine("DEBUG Adding BID REFERENCE");
                        REFERENCE RefJA = new REFERENCE();
                        RefJA.Source = "BID";   //Hardcoded
                        RefJA.ReferenceSourceID = myBID;
                        RefJA.ReferenceTitle = myBID;   //Will be updated with the real title at some point
                        RefJA.ReferenceURL = "http://securityfocus.com/bid/" + myBID;
                        RefJA.CreatedDate = DateTimeOffset.Now;
                        RefJA.timestamp = DateTimeOffset.Now;
                        RefJA.VocabularyID = iVocabularyMetasploitID;
                        model.REFERENCE.Add(REFERENCENormalize(model,RefJA));
                        model.SaveChanges();
                        //bidID = RefJA.ReferenceID;
                        iReferenceID = RefJA.ReferenceID;
                    }

                    //Check if the EXPLOITFORREFERENCE already exists in the database
                    // optimize
                    //EXPLOITFORREFERENCE sploitref = model.EXPLOITFORREFERENCE.FirstOrDefault(o => o.ExploitID == sploit.ExploitID && o.ReferenceID == bidID);
                    int iExploitReferenceID = 0;
                    try
                    {
                        iExploitReferenceID = model.EXPLOITFORREFERENCE.Where(o => o.ExploitID == iExploitID && o.ReferenceID == iReferenceID).Select(o => o.ExploitReferenceID).FirstOrDefault();
                    }
                    catch (Exception ex)
                    {

                    }
                    //if (sploitref == null)
                    if (iExploitReferenceID <= 0)
                    {
                        EXPLOITFORREFERENCE sploitref = new EXPLOITFORREFERENCE();
                        sploitref.ExploitID = iExploitID;
                        sploitref.ReferenceID = iReferenceID;   // osvdbID;
                        sploitref.CreatedDate = DateTimeOffset.Now;
                        sploitref.timestamp = DateTimeOffset.Now;
                        sploitref.VocabularyID = iVocabularyMetasploitID;
                        model.EXPLOITFORREFERENCE.Add(sploitref);
                        //model.SaveChanges();    //TEST PERFORMANCE
                        //iExploitReferenceID=
                    }
                    else
                    {
                        //Update EXPLOITFORREFERENCE
                    }
                }

                //****************************************************************
                #region metasploitreferences
                //  URL1
                //TODO work with an array of URLs
                if (myURL1 != "")
                {
                    //Check if the URL reference already exists in the database
                    int urlID = 0;
                    var syn2 = from S in model.REFERENCE
                                where S.ReferenceURL.Equals(myURL1)
                                select S.ReferenceID;
                    if (syn2.Count() != 0)
                    {
                        urlID = syn2.ToList().First();  //.ReferenceID;
                        //Update REFERENCE
                        //RefJA = REFERENCENormalize(model,RefJA);
                    }
                    else
                    {
                        //Add the URL Reference
                        REFERENCE RefJA = new REFERENCE();
                        RefJA.Source = ReferenceSource(myURL1);
                        RefJA.ReferenceTitle = myURL1;
                        RefJA.ReferenceURL = myURL1;
                        RefJA.CreatedDate = DateTimeOffset.Now;
                        RefJA.timestamp = DateTimeOffset.Now;
                        RefJA.VocabularyID = iVocabularyMetasploitID;
                        model.REFERENCE.Add(REFERENCENormalize(model,RefJA));
                        model.SaveChanges();
                        urlID = RefJA.ReferenceID;
                    }

                    //Check if the EXPLOITFORREFERENCE already exists in the database
                    var syn3 = from S in model.EXPLOITFORREFERENCE
                                where S.ExploitID.Equals(iExploitID)
                                && S.ReferenceID.Equals(urlID)
                                select S.ExploitReferenceID;
                    if (syn3.Count() == 0)
                    {
                        EXPLOITFORREFERENCE sploitref = new EXPLOITFORREFERENCE();
                        sploitref.ExploitID = iExploitID;
                        sploitref.ReferenceID = urlID;
                        sploitref.CreatedDate = DateTimeOffset.Now;
                        sploitref.timestamp = DateTimeOffset.Now;
                        sploitref.VocabularyID = iVocabularyMetasploitID;
                        model.EXPLOITFORREFERENCE.Add(sploitref);   //TEST PERFORMANCE
                        model.SaveChanges();
                    }

                    //****************************************************************
                    //  URL2
                    if (myURL2 != "")
                    {
                        //Check if the URL reference already exists in the database
                        urlID = 0;
                        var syn22 = from S in model.REFERENCE
                                    where S.ReferenceURL.Equals(myURL2)
                                    select S.ReferenceID;
                        if (syn22.Count() != 0)
                        {
                            urlID = syn22.ToList().First(); //.ReferenceID;
                            //Update REFERENCE
                            //RefJA = REFERENCENormalize(model,RefJA);
                        }
                        else
                        {
                            //Add the URL Reference
                            REFERENCE RefJA = new REFERENCE();
                            RefJA.Source = ReferenceSource(myURL2);
                            RefJA.ReferenceTitle = myURL2;
                            RefJA.ReferenceURL = myURL2;
                            RefJA.CreatedDate = DateTimeOffset.Now;
                            RefJA.timestamp = DateTimeOffset.Now;
                            RefJA.VocabularyID = iVocabularyMetasploitID;
                            model.REFERENCE.Add(REFERENCENormalize(model,RefJA));
                            model.SaveChanges();
                            urlID = RefJA.ReferenceID;
                        }

                        //Check if the EXPLOITFORREFERENCE already exists in the database
                        var syn32 = from S in model.EXPLOITFORREFERENCE
                                    where S.ExploitID.Equals(iExploitID)
                                    && S.ReferenceID.Equals(urlID)
                                    select S.ExploitReferenceID;
                        if (syn32.Count() == 0)
                        {
                            EXPLOITFORREFERENCE sploitref = new EXPLOITFORREFERENCE();
                            sploitref.ExploitID = iExploitID;
                            sploitref.ReferenceID = urlID;
                            sploitref.CreatedDate = DateTimeOffset.Now;
                            sploitref.timestamp = DateTimeOffset.Now;
                            sploitref.VocabularyID = iVocabularyMetasploitID;
                            model.EXPLOITFORREFERENCE.Add(sploitref);   //TEST PERFORMANCE
                            model.SaveChanges();
                        }


                        //****************************************************************
                        //  URL3
                        if (myURL3 != "")
                        {
                            //Check if the URL reference already exists in the database
                            urlID = 0;
                            var syn23 = from S in model.REFERENCE
                                        where S.ReferenceURL.Equals(myURL3)
                                        select S.ReferenceID;
                            if (syn23.Count() != 0)
                            {
                                urlID = syn23.ToList().First(); //.ReferenceID;
                            }
                            else
                            {
                                //Add the URL Reference
                                REFERENCE RefJA = new REFERENCE();
                                RefJA.Source = ReferenceSource(myURL3);
                                RefJA.ReferenceTitle = myURL3;
                                RefJA.ReferenceURL = myURL3;
                                RefJA.CreatedDate = DateTimeOffset.Now;
                                RefJA.timestamp = DateTimeOffset.Now;
                                RefJA.VocabularyID = iVocabularyMetasploitID;
                                model.REFERENCE.Add(REFERENCENormalize(model,RefJA));
                                model.SaveChanges();
                                urlID = RefJA.ReferenceID;
                            }

                            //Check if the EXPLOITFORREFERENCE already exists in the database
                            var syn33 = from S in model.EXPLOITFORREFERENCE
                                        where S.ExploitID.Equals(iExploitID)
                                        && S.ReferenceID.Equals(urlID)
                                        select S.ExploitReferenceID;
                            if (syn33.Count() == 0)
                            {
                                EXPLOITFORREFERENCE sploitref = new EXPLOITFORREFERENCE();
                                sploitref.ExploitID = iExploitID;
                                sploitref.ReferenceID = urlID;
                                sploitref.CreatedDate = DateTimeOffset.Now;
                                sploitref.timestamp = DateTimeOffset.Now;
                                sploitref.VocabularyID = iVocabularyMetasploitID;
                                model.EXPLOITFORREFERENCE.Add(sploitref);
                                model.SaveChanges();
                            }

                            //****************************************************************
                            //  URL4
                            if (myURL4 != "")
                            {
                                //Check if the URL reference already exists in the database
                                urlID = 0;
                                var syn24 = from S in model.REFERENCE
                                            where S.ReferenceURL.Equals(myURL4)
                                            select S.ReferenceID;
                                if (syn24.Count() != 0)
                                {
                                    urlID = syn24.ToList().First(); //.ReferenceID;
                                }
                                else
                                {
                                    //Add the URL Reference
                                    REFERENCE RefJA = new REFERENCE();
                                    RefJA.Source = ReferenceSource(myURL4);
                                    RefJA.ReferenceTitle = myURL4;
                                    RefJA.ReferenceURL = myURL4;
                                    RefJA.CreatedDate = DateTimeOffset.Now;
                                    RefJA.timestamp = DateTimeOffset.Now;
                                    RefJA.VocabularyID = iVocabularyMetasploitID;
                                    model.REFERENCE.Add(REFERENCENormalize(model,RefJA));
                                    model.SaveChanges();
                                    urlID = RefJA.ReferenceID;
                                }

                                //Check if the EXPLOITFORREFERENCE already exists in the database
                                var syn34 = from S in model.EXPLOITFORREFERENCE
                                            where S.ExploitID.Equals(iExploitID)
                                            && S.ReferenceID.Equals(urlID)
                                            select S.ExploitReferenceID;
                                if (syn34.Count() == 0)
                                {
                                    EXPLOITFORREFERENCE sploitref = new EXPLOITFORREFERENCE();
                                    sploitref.ExploitID = iExploitID;
                                    sploitref.ReferenceID = urlID;
                                    sploitref.CreatedDate = DateTimeOffset.Now;
                                    sploitref.timestamp = DateTimeOffset.Now;
                                    sploitref.VocabularyID = iVocabularyMetasploitID;
                                    model.EXPLOITFORREFERENCE.Add(sploitref);
                                    model.SaveChanges();
                                }

                                //****************************************************************
                                //  URL5
                                if (myURL5 != "")
                                {
                                    //Check if the URL reference already exists in the database
                                    urlID = 0;
                                    var syn25 = from S in model.REFERENCE
                                                where S.ReferenceURL.Equals(myURL5)
                                                select S.ReferenceID;
                                    if (syn25.Count() != 0)
                                    {
                                        urlID = syn25.ToList().First(); //.ReferenceID;
                                    }
                                    else
                                    {
                                        //Add the URL Reference
                                        REFERENCE RefJA = new REFERENCE();
                                        RefJA.Source = ReferenceSource(myURL5);
                                        RefJA.ReferenceTitle = myURL5;
                                        RefJA.ReferenceURL = myURL5;
                                        RefJA.CreatedDate = DateTimeOffset.Now;
                                        RefJA.timestamp = DateTimeOffset.Now;
                                        RefJA.VocabularyID = iVocabularyMetasploitID;
                                        model.REFERENCE.Add(REFERENCENormalize(model,RefJA));
                                        model.SaveChanges();
                                        urlID = RefJA.ReferenceID;
                                    }

                                    //Check if the EXPLOITFORREFERENCE already exists in the database
                                    var syn35 = from S in model.EXPLOITFORREFERENCE
                                                where S.ExploitID.Equals(iExploitID)
                                                && S.ReferenceID.Equals(urlID)
                                                select S.ExploitReferenceID;
                                    if (syn35.Count() == 0)
                                    {
                                        EXPLOITFORREFERENCE sploitref = new EXPLOITFORREFERENCE();
                                        sploitref.ExploitID = iExploitID;
                                        sploitref.ReferenceID = urlID;
                                        sploitref.CreatedDate = DateTimeOffset.Now;
                                        sploitref.timestamp = DateTimeOffset.Now;
                                        sploitref.VocabularyID = iVocabularyMetasploitID;
                                        model.EXPLOITFORREFERENCE.Add(sploitref);
                                        model.SaveChanges();
                                    }

                                    //****************************************************************
                                    //  URL6
                                    if (myURL6 != "")
                                    {
                                        //Check if the URL reference already exists in the database
                                        urlID = 0;
                                        var syn26 = from S in model.REFERENCE
                                                    where S.ReferenceURL.Equals(myURL6)
                                                    select S;
                                        if (syn26.Count() != 0)
                                        {
                                            urlID = syn26.ToList().First().ReferenceID;
                                        }
                                        else
                                        {
                                            //Add the URL Reference
                                            REFERENCE RefJA = new REFERENCE();
                                            RefJA.Source = ReferenceSource(myURL6);
                                            RefJA.ReferenceTitle = myURL6;
                                            RefJA.ReferenceURL = myURL6;
                                            RefJA.CreatedDate = DateTimeOffset.Now;
                                            RefJA.timestamp = DateTimeOffset.Now;
                                            RefJA.VocabularyID = iVocabularyMetasploitID;
                                            model.REFERENCE.Add(REFERENCENormalize(model,RefJA));
                                            model.SaveChanges();
                                            urlID = RefJA.ReferenceID;
                                        }

                                        //Check if the EXPLOITFORREFERENCE already exists in the database
                                        var syn36 = from S in model.EXPLOITFORREFERENCE
                                                    where S.ExploitID.Equals(iExploitID)
                                                    && S.ReferenceID.Equals(urlID)
                                                    select S;
                                        if (syn36.Count() == 0)
                                        {
                                            EXPLOITFORREFERENCE sploitref = new EXPLOITFORREFERENCE();
                                            sploitref.ExploitID = iExploitID;
                                            sploitref.ReferenceID = urlID;
                                            sploitref.CreatedDate = DateTimeOffset.Now;
                                            sploitref.timestamp = DateTimeOffset.Now;
                                            sploitref.VocabularyID = iVocabularyMetasploitID;
                                            model.EXPLOITFORREFERENCE.Add(sploitref);
                                            model.SaveChanges();
                                        }

                                        //****************************************************************
                                        //  URL7
                                        if (myURL7 != "")
                                        {
                                            //Check if the URL reference already exists in the database
                                            urlID = 0;
                                            var syn27 = from S in model.REFERENCE
                                                        where S.ReferenceURL.Equals(myURL7)
                                                        select S;
                                            if (syn27.Count() != 0)
                                            {
                                                urlID = syn27.ToList().First().ReferenceID;
                                            }
                                            else
                                            {
                                                //Add the URL Reference
                                                REFERENCE RefJA = new REFERENCE();
                                                RefJA.Source = ReferenceSource(myURL7);
                                                RefJA.ReferenceTitle = myURL7;
                                                RefJA.ReferenceURL = myURL7;
                                                RefJA.CreatedDate = DateTimeOffset.Now;
                                                RefJA.timestamp = DateTimeOffset.Now;
                                                RefJA.VocabularyID = iVocabularyMetasploitID;
                                                model.REFERENCE.Add(REFERENCENormalize(model,RefJA));
                                                model.SaveChanges();
                                                urlID = RefJA.ReferenceID;
                                            }

                                            //Check if the EXPLOITFORREFERENCE already exists in the database
                                            var syn37 = from S in model.EXPLOITFORREFERENCE
                                                        where S.ExploitID.Equals(iExploitID)
                                                        && S.ReferenceID.Equals(urlID)
                                                        select S;
                                            if (syn37.Count() == 0)
                                            {
                                                EXPLOITFORREFERENCE sploitref = new EXPLOITFORREFERENCE();
                                                sploitref.ExploitID = iExploitID;
                                                sploitref.ReferenceID = urlID;
                                                sploitref.CreatedDate = DateTimeOffset.Now;
                                                sploitref.timestamp = DateTimeOffset.Now;
                                                sploitref.VocabularyID = iVocabularyMetasploitID;
                                                model.EXPLOITFORREFERENCE.Add(sploitref);
                                                model.SaveChanges();
                                            }

                                            //****************************************************************
                                            //  URL8
                                            if (myURL8 != "")
                                            {
                                                //Check if the URL reference already exists in the database
                                                urlID = 0;
                                                var syn28 = from S in model.REFERENCE
                                                            where S.ReferenceURL.Equals(myURL8)
                                                            select S;
                                                if (syn28.Count() != 0)
                                                {
                                                    urlID = syn28.ToList().First().ReferenceID;
                                                }
                                                else
                                                {
                                                    //Add the URL Reference
                                                    REFERENCE RefJA = new REFERENCE();
                                                    RefJA.Source = ReferenceSource(myURL8);
                                                    RefJA.ReferenceTitle = myURL8;
                                                    RefJA.ReferenceURL = myURL8;
                                                    RefJA.CreatedDate = DateTimeOffset.Now;
                                                    RefJA.timestamp = DateTimeOffset.Now;
                                                    //RefJA.VocabularyID= //TODO metasploit
                                                    model.REFERENCE.Add(REFERENCENormalize(model,RefJA));
                                                    model.SaveChanges();
                                                    urlID = RefJA.ReferenceID;
                                                }

                                                //Check if the EXPLOITFORREFERENCE already exists in the database
                                                var syn38 = from S in model.EXPLOITFORREFERENCE
                                                            where S.ExploitID.Equals(iExploitID)
                                                            && S.ReferenceID.Equals(urlID)
                                                            select S;
                                                if (syn38.Count() == 0)
                                                {
                                                    EXPLOITFORREFERENCE sploitref = new EXPLOITFORREFERENCE();
                                                    sploitref.ExploitID = iExploitID;
                                                    sploitref.ReferenceID = urlID;
                                                    sploitref.CreatedDate = DateTimeOffset.Now;
                                                    sploitref.timestamp = DateTimeOffset.Now;
                                                    sploitref.VocabularyID = iVocabularyMetasploitID;
                                                    model.EXPLOITFORREFERENCE.Add(sploitref);
                                                    model.SaveChanges();
                                                }

                                                //****************************************************************
                                                //  URL9
                                                if (myURL9 != "")
                                                {
                                                    //Check if the URL reference already exists in the database
                                                    urlID = 0;
                                                    var syn29 = from S in model.REFERENCE
                                                                where S.ReferenceURL.Equals(myURL9)
                                                                select S;
                                                    if (syn29.Count() != 0)
                                                    {
                                                        urlID = syn29.ToList().First().ReferenceID;
                                                    }
                                                    else
                                                    {
                                                        //Add the URL Reference
                                                        REFERENCE RefJA = new REFERENCE();
                                                        RefJA.Source = ReferenceSource(myURL9);
                                                        RefJA.ReferenceTitle = myURL9;
                                                        RefJA.ReferenceURL = myURL9;
                                                        RefJA.CreatedDate = DateTimeOffset.Now;
                                                        RefJA.timestamp = DateTimeOffset.Now;
                                                        RefJA.VocabularyID = iVocabularyMetasploitID;
                                                        model.REFERENCE.Add(REFERENCENormalize(model,RefJA));
                                                        model.SaveChanges();
                                                        urlID = RefJA.ReferenceID;
                                                    }

                                                    //Check if the EXPLOITFORREFERENCE already exists in the database
                                                    var syn39 = from S in model.EXPLOITFORREFERENCE
                                                                where S.ExploitID.Equals(iExploitID)
                                                                && S.ReferenceID.Equals(urlID)
                                                                select S;
                                                    if (syn39.Count() == 0)
                                                    {
                                                        EXPLOITFORREFERENCE sploitref = new EXPLOITFORREFERENCE();
                                                        sploitref.ExploitID = iExploitID;
                                                        sploitref.ReferenceID = urlID;
                                                        sploitref.CreatedDate = DateTimeOffset.Now;
                                                        sploitref.timestamp = DateTimeOffset.Now;
                                                        sploitref.VocabularyID = iVocabularyMetasploitID;
                                                        model.EXPLOITFORREFERENCE.Add(sploitref);
                                                        model.SaveChanges();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion metasploitreferences

                monStreamReader.Close();

            }

            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
            Console.WriteLine("DEBUG METASPLOIT IMPORT FINISHED - ENJOY DUDE!");
            //FREE
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
                Console.WriteLine("Exception: DbEntityValidationExceptionFINALSAVEMETASPLOIT " + sb.ToString());
            }
            catch (Exception exFINALSAVE)
            {
                Console.WriteLine("Exception: exFINALSAVEMETASPLOIT " + exFINALSAVE.Message + " " + exFINALSAVE.InnerException);
            }
            model.Dispose();
            model = null;
        }

        static private void ParseExploit(string ExploitPath)
        {

        }

        static string CleaningCWEString(string sStringToClean)
        {

            //Cleaning
            //sStringToClean = sStringToClean.Replace("<Text>", "");
            //sStringToClean = sStringToClean.Replace("</Text>", "");
            //Remove CLRF
            sStringToClean = sStringToClean.Replace("\r\n", " ");
            sStringToClean = sStringToClean.Replace("\n", " ");
            sStringToClean = sStringToClean.Replace("\t", " "); //TAB
            /*
            //C# escape characters
            \' for a single quote
            \" for a double quote
            \\ for a backslash
            \0 for a null character
            \a for an alert character
            \b for a backspace
            \f for a form feed
            \n for a new line
            \r for a carriage return
            \t for a horizontal tab
            \v for a vertical tab
            \uxxxx for a unicode character hex value (e.g. \u0020)
            \x is the same as \u, but you don't need leading zeroes (.g. \x20)
            \Uxxxxxxxx for a unicode character hex value (longer form needed for generating surrogates)
            */
            while (sStringToClean.Contains("  "))
            {
                sStringToClean = sStringToClean.Replace("  ", " ");
            }

            return sStringToClean.Trim();
        }


        static private string ReferenceSource(string theurl)
        {
            //Retrieve the ReferenceSource from a URL
            theurl = theurl.Replace("http://www.", "http://");
            theurl = theurl.Replace("https://www.", "https://");
            if(theurl.StartsWith("/"))
            {
                Console.WriteLine("ERROR: ReferenceSource theurl=" + theurl);
            }

            //TODO: http://cve.mitre.org/data/refs/index.html
            
            string strTemp=string.Empty;
            string mySource="MISC";

            Regex myRegex = new Regex("http://cert.org[^<>]*");
            strTemp = myRegex.Match(theurl).ToString();
            if (strTemp != "")
            {
                mySource = "CERT";
            }
            else
            {
                myRegex = new Regex("http://us-cert.org[^<>]*");
                strTemp = myRegex.Match(theurl).ToString();
                if (strTemp != "")
                {
                    mySource = "CERT";  //TODO REVIEW vs US-CERT
                }
                else
                {
                    myRegex = new Regex("http://kb.cert.org[^<>]*");
                    strTemp = myRegex.Match(theurl).ToString();
                    if (strTemp != "")
                    {
                        mySource = "CERT-VN";
                    }
                    else
                    {
                        myRegex = new Regex("http://support.microsoft.com[^<>]*");
                        strTemp = myRegex.Match(theurl).ToString();
                        if (strTemp != "")
                        {
                            mySource = "MSKB";
                        }
                        else
                        {
                            //xforce.iss.net/xforce/xfdb/123
                            myRegex = new Regex("http://xforce.iss.net[^<>]*");
                            strTemp = myRegex.Match(theurl).ToString();
                            if (strTemp != "")
                            {
                                mySource = "XF";
                            }
                            else
                            {
                                myRegex = new Regex("http://ciac.org[^<>]*");
                                strTemp = myRegex.Match(theurl).ToString();
                                if (strTemp != "")
                                {
                                    mySource = "CIAC";
                                }
                                else
                                {
                                    myRegex = new Regex("ciac.llnl.gov[^<>]*");
                                    strTemp = myRegex.Match(theurl).ToString();
                                    if (strTemp != "")
                                    {
                                        mySource = "CIAC";
                                    }
                                    else
                                    {
                                        myRegex = new Regex("freebsd.org[^<>]*");
                                        strTemp = myRegex.Match(theurl).ToString();
                                        if (strTemp != "")
                                        {
                                            mySource = "FREEBSD";
                                        }
                                        else
                                        {
                                            myRegex = new Regex("sunsolve.sun.com[^<>]*");
                                            strTemp = myRegex.Match(theurl).ToString();
                                            if (strTemp != "")
                                            {
                                                mySource = "SUN";   //SUNBUG    SUNALERT
                                            }
                                            else
                                            {
                                                myRegex = new Regex("ntbugtraq[^<>]*");
                                                strTemp = myRegex.Match(theurl).ToString();
                                                if (strTemp != "")
                                                {
                                                    mySource = "NTBUGTRAQ";
                                                }
                                                else
                                                {
                                                    myRegex = new Regex("bugtraq[^<>]*");
                                                    strTemp = myRegex.Match(theurl).ToString();
                                                    if (strTemp != "")
                                                    {
                                                        mySource = "BUGTRAQ";
                                                    }
                                                    else
                                                    {
                                                        myRegex = new Regex("hpalert[^<>]*");
                                                        strTemp = myRegex.Match(theurl).ToString();
                                                        if (strTemp != "")
                                                        {
                                                            mySource = "HP";
                                                        }
                                                        else
                                                        {
                                                            myRegex = new Regex("auscert.org[^<>]*");
                                                            strTemp = myRegex.Match(theurl).ToString();
                                                            if (strTemp != "")
                                                            {
                                                                mySource = "AUSCERT";
                                                            }
                                                            else
                                                            {
                                                                myRegex = new Regex("patches.sgi.com[^<>]*");
                                                                strTemp = myRegex.Match(theurl).ToString();
                                                                if (strTemp != "")
                                                                {
                                                                    mySource = "SGI";
                                                                }
                                                                else
                                                                {
                                                                    myRegex = new Regex("trustix.org[^<>]*");
                                                                    strTemp = myRegex.Match(theurl).ToString();
                                                                    if (strTemp != "")
                                                                    {
                                                                        mySource = "TRUSTIX";
                                                                    }
                                                                    else
                                                                    {
                                                                        myRegex = new Regex("redhat.com[^<>]*");
                                                                        strTemp = myRegex.Match(theurl).ToString();
                                                                        if (strTemp != "")
                                                                        {
                                                                            mySource = "REDHAT";
                                                                        }
                                                                        else
                                                                        {
                                                                            myRegex = new Regex("debian.org[^<>]*");
                                                                            strTemp = myRegex.Match(theurl).ToString();
                                                                            if (strTemp != "")
                                                                            {
                                                                                mySource = "DEBIAN";
                                                                            }
                                                                            else
                                                                            {
                                                                                myRegex = new Regex("mandriva.com[^<>]*");
                                                                                strTemp = myRegex.Match(theurl).ToString();
                                                                                if (strTemp != "")
                                                                                {
                                                                                    mySource = "MANDRAKE";
                                                                                }
                                                                                else
                                                                                {
                                                                                    myRegex = new Regex("secunia.com[^<>]*");
                                                                                    strTemp = myRegex.Match(theurl).ToString();
                                                                                    if (strTemp != "")
                                                                                    {
                                                                                        mySource = "SECUNIA";
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        myRegex = new Regex(".sco.com[^<>]*");
                                                                                        strTemp = myRegex.Match(theurl).ToString();
                                                                                        if (strTemp != "")
                                                                                        {
                                                                                            mySource = "SCO";
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            myRegex = new Regex("ibm.com[^<>]*apar");
                                                                                            strTemp = myRegex.Match(theurl).ToString();
                                                                                            if (strTemp != "")
                                                                                            {
                                                                                                mySource = "AIXAPAR";
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                myRegex = new Regex("/aix/");
                                                                                                strTemp = myRegex.Match(theurl).ToString();
                                                                                                if (strTemp != "")
                                                                                                {
                                                                                                    mySource = "AIXAPAR";
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    myRegex = new Regex("ciac.llnl.gov[^<>]*");
                                                                                                    strTemp = myRegex.Match(theurl).ToString();
                                                                                                    if (strTemp != "")
                                                                                                    {
                                                                                                        mySource = "CIAC";
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        myRegex = new Regex("caldera.com[^<>]*");
                                                                                                        strTemp = myRegex.Match(theurl).ToString();
                                                                                                        if (strTemp != "")
                                                                                                        {
                                                                                                            mySource = "CALDERA";
                                                                                                        }
                                                                                                        else
                                                                                                        {
                                                                                                            myRegex = new Regex("calderasystems.com[^<>]*");
                                                                                                            strTemp = myRegex.Match(theurl).ToString();
                                                                                                            if (strTemp != "")
                                                                                                            {
                                                                                                                mySource = "CALDERA";
                                                                                                            }
                                                                                                            else
                                                                                                            {
                                                                                                                myRegex = new Regex("full-disclosure[^<>]*");
                                                                                                                strTemp = myRegex.Match(theurl).ToString();
                                                                                                                if (strTemp != "")
                                                                                                                {
                                                                                                                    mySource = "FULLDISC";
                                                                                                                }
                                                                                                                else
                                                                                                                {
                                                                                                                    myRegex = new Regex("eeye.com[^<>]*");
                                                                                                                    strTemp = myRegex.Match(theurl).ToString();
                                                                                                                    if (strTemp != "")
                                                                                                                    {
                                                                                                                        mySource = "EEYE";
                                                                                                                    }
                                                                                                                    else
                                                                                                                    {
                                                                                                                        myRegex = new Regex("microsoft.com[^<>]*");
                                                                                                                        strTemp = myRegex.Match(theurl).ToString();
                                                                                                                        if (strTemp != "")
                                                                                                                        {
                                                                                                                            mySource = "MS";
                                                                                                                        }
                                                                                                                        else
                                                                                                                        {
                                                                                                                            myRegex = new Regex("cisco.com[^<>]*");
                                                                                                                            strTemp = myRegex.Match(theurl).ToString();
                                                                                                                            if (strTemp != "")
                                                                                                                            {
                                                                                                                                mySource = "CISCO";
                                                                                                                            }
                                                                                                                            else
                                                                                                                            {
                                                                                                                                myRegex = new Regex("vupen.com[^<>]*");
                                                                                                                                strTemp = myRegex.Match(theurl).ToString();
                                                                                                                                if (strTemp != "")
                                                                                                                                {
                                                                                                                                    mySource = "VUPEN";
                                                                                                                                }
                                                                                                                                else
                                                                                                                                {
                                                                                                                                    myRegex = new Regex("securitytracker.com[^<>]*");
                                                                                                                                    strTemp = myRegex.Match(theurl).ToString();
                                                                                                                                    if (strTemp != "")
                                                                                                                                    {
                                                                                                                                        mySource = "SECTRACK";
                                                                                                                                    }
                                                                                                                                    else
                                                                                                                                    {
                                                                                                                                        myRegex = new Regex("fedoraproject[^<>]*");
                                                                                                                                        strTemp = myRegex.Match(theurl).ToString();
                                                                                                                                        if (strTemp != "")
                                                                                                                                        {
                                                                                                                                            mySource = "FEDORA";
                                                                                                                                        }
                                                                                                                                        else
                                                                                                                                        {
                                                                                                                                            myRegex = new Regex("openwall.com/lists[^<>]*");
                                                                                                                                            strTemp = myRegex.Match(theurl).ToString();
                                                                                                                                            if (strTemp != "")
                                                                                                                                            {
                                                                                                                                                mySource = "MLIST";
                                                                                                                                            }
                                                                                                                                            else
                                                                                                                                            {
                                                                                                                                                myRegex = new Regex("securityreason.com[^<>]*");
                                                                                                                                                strTemp = myRegex.Match(theurl).ToString();
                                                                                                                                                if (strTemp != "")
                                                                                                                                                {
                                                                                                                                                    mySource = "SREASONRES";
                                                                                                                                                }
                                                                                                                                                else
                                                                                                                                                {
                                                                                                                                                    myRegex = new Regex("exploit-db.com[^<>]*");
                                                                                                                                                    strTemp = myRegex.Match(theurl).ToString();
                                                                                                                                                    if (strTemp != "")
                                                                                                                                                    {
                                                                                                                                                        mySource = "EXPLOIT-DB";    //TODO: Test if EXPLOIT (vs tutorial)
                                                                                                                                                    }
                                                                                                                                                    else
                                                                                                                                                    {
                                                                                                                                                        myRegex = new Regex("ubuntu.com[^<>]*");
                                                                                                                                                        strTemp = myRegex.Match(theurl).ToString();
                                                                                                                                                        if (strTemp != "")
                                                                                                                                                        {
                                                                                                                                                            mySource = "UBUNTU";
                                                                                                                                                        }
                                                                                                                                                        else
                                                                                                                                                        {
                                                                                                                                                            myRegex = new Regex("slackware.com[^<>]*");
                                                                                                                                                            strTemp = myRegex.Match(theurl).ToString();
                                                                                                                                                            if (strTemp != "")
                                                                                                                                                            {
                                                                                                                                                                mySource = "SLACKWARE";
                                                                                                                                                            }
                                                                                                                                                            else
                                                                                                                                                            {
                                                                                                                                                                myRegex = new Regex("allaire.com[^<>]*");
                                                                                                                                                                strTemp = myRegex.Match(theurl).ToString();
                                                                                                                                                                if (strTemp != "")
                                                                                                                                                                {
                                                                                                                                                                    mySource = "ALLAIRE";
                                                                                                                                                                }
                                                                                                                                                                else
                                                                                                                                                                {
                                                                                                                                                                    myRegex = new Regex("apple.com[^<>]*");
                                                                                                                                                                    strTemp = myRegex.Match(theurl).ToString();
                                                                                                                                                                    if (strTemp != "")
                                                                                                                                                                    {
                                                                                                                                                                        mySource = "APPLE";
                                                                                                                                                                    }
                                                                                                                                                                    else
                                                                                                                                                                    {
                                                                                                                                                                        myRegex = new Regex("atstake.com[^<>]*");
                                                                                                                                                                        strTemp = myRegex.Match(theurl).ToString();
                                                                                                                                                                        if (strTemp != "")
                                                                                                                                                                        {
                                                                                                                                                                            mySource = "ATSTAKE";
                                                                                                                                                                        }
                                                                                                                                                                        else
                                                                                                                                                                        {
                                                                                                                                                                            myRegex = new Regex(".bea.com[^<>]*");  //TODO Review
                                                                                                                                                                            strTemp = myRegex.Match(theurl).ToString();
                                                                                                                                                                            if (strTemp != "")
                                                                                                                                                                            {
                                                                                                                                                                                mySource = "BEA";
                                                                                                                                                                            }
                                                                                                                                                                            else
                                                                                                                                                                            {
                                                                                                                                                                                myRegex = new Regex("bindview.com[^<>]*");
                                                                                                                                                                                strTemp = myRegex.Match(theurl).ToString();
                                                                                                                                                                                if (strTemp != "")
                                                                                                                                                                                {
                                                                                                                                                                                    mySource = "BINDVIEW";
                                                                                                                                                                                }
                                                                                                                                                                                else
                                                                                                                                                                                {
                                                                                                                                                                                    
                                                                                                                                                                                    myRegex = new Regex("securityfocus.com[^<>]*");
                                                                                                                                                                                    strTemp = myRegex.Match(theurl).ToString();
                                                                                                                                                                                    if (strTemp != "")
                                                                                                                                                                                    {
                                                                                                                                                                                        if (theurl.Contains("/bid"))
                                                                                                                                                                                        {
                                                                                                                                                                                            mySource = "BID";   //As used for example in Import_saint()
                                                                                                                                                                                        }
                                                                                                                                                                                        else
                                                                                                                                                                                        {
                                                                                                                                                                                            mySource = "BUGTRAQ";   //TODO REVIEW
                                                                                                                                                                                        }
                                                                                                                                                                                    }
                                                                                                                                                                                    else
                                                                                                                                                                                    {
                                                                                                                                                                                        myRegex = new Regex("conectiva.com[^<>]*");
                                                                                                                                                                                        strTemp = myRegex.Match(theurl).ToString();
                                                                                                                                                                                        if (strTemp != "")
                                                                                                                                                                                        {
                                                                                                                                                                                            mySource = "CONECTIVA";
                                                                                                                                                                                        }
                                                                                                                                                                                        else
                                                                                                                                                                                        {
                                                                                                                                                                                            myRegex = new Regex("linuxsecurity.com[^<>]*");
                                                                                                                                                                                            strTemp = myRegex.Match(theurl).ToString();
                                                                                                                                                                                            if (strTemp != "")
                                                                                                                                                                                            {
                                                                                                                                                                                                mySource = "ENGARDE";
                                                                                                                                                                                            }
                                                                                                                                                                                            else
                                                                                                                                                                                            {
                                                                                                                                                                                                myRegex = new Regex("fedoranews[^<>]*");
                                                                                                                                                                                                strTemp = myRegex.Match(theurl).ToString();
                                                                                                                                                                                                if (strTemp != "")
                                                                                                                                                                                                {
                                                                                                                                                                                                    mySource = "FEDORA";
                                                                                                                                                                                                }
                                                                                                                                                                                                else
                                                                                                                                                                                                {
                                                                                                                                                                                                    myRegex = new Regex("fedora[^<>]*");
                                                                                                                                                                                                    strTemp = myRegex.Match(theurl).ToString();
                                                                                                                                                                                                    if (strTemp != "")
                                                                                                                                                                                                    {
                                                                                                                                                                                                        mySource = "FEDORA";
                                                                                                                                                                                                    }
                                                                                                                                                                                                    else
                                                                                                                                                                                                    {
                                                                                                                                                                                                        myRegex = new Regex("FreeBSD[^<>]*");   //TODO Review lowercase
                                                                                                                                                                                                        strTemp = myRegex.Match(theurl).ToString();
                                                                                                                                                                                                        if (strTemp != "")
                                                                                                                                                                                                        {
                                                                                                                                                                                                            mySource = "FREEBSD";
                                                                                                                                                                                                        }
                                                                                                                                                                                                        else
                                                                                                                                                                                                        {
                                                                                                                                                                                                            myRegex = new Regex("fulldisclosure[^<>]*");
                                                                                                                                                                                                            strTemp = myRegex.Match(theurl).ToString();
                                                                                                                                                                                                            if (strTemp != "")
                                                                                                                                                                                                            {
                                                                                                                                                                                                                mySource = "FULLDISC";
                                                                                                                                                                                                            }
                                                                                                                                                                                                            else
                                                                                                                                                                                                            {
                                                                                                                                                                                                                myRegex = new Regex("gentoo.org[^<>]*");
                                                                                                                                                                                                                strTemp = myRegex.Match(theurl).ToString();
                                                                                                                                                                                                                if (strTemp != "")
                                                                                                                                                                                                                {
                                                                                                                                                                                                                    mySource = "GENTOO";
                                                                                                                                                                                                                }
                                                                                                                                                                                                                else
                                                                                                                                                                                                                {
                                                                                                                                                                                                                    myRegex = new Regex("hp.com[^<>]*");   //TODO Review
                                                                                                                                                                                                                    strTemp = myRegex.Match(theurl).ToString();
                                                                                                                                                                                                                    if (strTemp != "")
                                                                                                                                                                                                                    {
                                                                                                                                                                                                                        mySource = "HP";
                                                                                                                                                                                                                    }
                                                                                                                                                                                                                    else
                                                                                                                                                                                                                    {
                                                                                                                                                                                                                        myRegex = new Regex("idefense.com[^<>]*");
                                                                                                                                                                                                                        strTemp = myRegex.Match(theurl).ToString();
                                                                                                                                                                                                                        if (strTemp != "")
                                                                                                                                                                                                                        {
                                                                                                                                                                                                                            mySource = "IDEFENSE";
                                                                                                                                                                                                                        }
                                                                                                                                                                                                                        else
                                                                                                                                                                                                                        {
                                                                                                                                                                                                                            myRegex = new Regex("immunix.org[^<>]*");
                                                                                                                                                                                                                            strTemp = myRegex.Match(theurl).ToString();
                                                                                                                                                                                                                            if (strTemp != "")
                                                                                                                                                                                                                            {
                                                                                                                                                                                                                                mySource = "IMMUNIX";
                                                                                                                                                                                                                            }
                                                                                                                                                                                                                            else
                                                                                                                                                                                                                            {
                                                                                                                                                                                                                                myRegex = new Regex("iss.net[^<>]*");
                                                                                                                                                                                                                                strTemp = myRegex.Match(theurl).ToString();
                                                                                                                                                                                                                                if (strTemp != "")
                                                                                                                                                                                                                                {
                                                                                                                                                                                                                                    mySource = "ISS";   //"XF"
                                                                                                                                                                                                                                }
                                                                                                                                                                                                                                else
                                                                                                                                                                                                                                {
                                                                                                                                                                                                                                    myRegex = new Regex("jvndb.jvn.jp[^<>]*");
                                                                                                                                                                                                                                    strTemp = myRegex.Match(theurl).ToString();
                                                                                                                                                                                                                                    if (strTemp != "")
                                                                                                                                                                                                                                    {
                                                                                                                                                                                                                                        mySource = "JVNDB";
                                                                                                                                                                                                                                    }
                                                                                                                                                                                                                                    else
                                                                                                                                                                                                                                    {
                                                                                                                                                                                                                                        myRegex = new Regex("jvn.jp[^<>]*");
                                                                                                                                                                                                                                        strTemp = myRegex.Match(theurl).ToString();
                                                                                                                                                                                                                                        if (strTemp != "")
                                                                                                                                                                                                                                        {
                                                                                                                                                                                                                                            mySource = "JVN";
                                                                                                                                                                                                                                        }
                                                                                                                                                                                                                                        else
                                                                                                                                                                                                                                        {
                                                                                                                                                                                                                                            myRegex = new Regex("l0pht.com[^<>]*");
                                                                                                                                                                                                                                            strTemp = myRegex.Match(theurl).ToString();
                                                                                                                                                                                                                                            if (strTemp != "")
                                                                                                                                                                                                                                            {
                                                                                                                                                                                                                                                mySource = "L0PHT";
                                                                                                                                                                                                                                            }
                                                                                                                                                                                                                                            else
                                                                                                                                                                                                                                            {
                                                                                                                                                                                                                                                myRegex = new Regex("lopht.com[^<>]*");
                                                                                                                                                                                                                                                strTemp = myRegex.Match(theurl).ToString();
                                                                                                                                                                                                                                                if (strTemp != "")
                                                                                                                                                                                                                                                {
                                                                                                                                                                                                                                                    mySource = "L0PHT";
                                                                                                                                                                                                                                                }
                                                                                                                                                                                                                                                else
                                                                                                                                                                                                                                                {
                                                                                                                                                                                                                                                    myRegex = new Regex("mandrake.com[^<>]*");
                                                                                                                                                                                                                                                    strTemp = myRegex.Match(theurl).ToString();
                                                                                                                                                                                                                                                    if (strTemp != "")
                                                                                                                                                                                                                                                    {
                                                                                                                                                                                                                                                        mySource = "MANDRAKE";  //MANDRIVA
                                                                                                                                                                                                                                                    }
                                                                                                                                                                                                                                                    else
                                                                                                                                                                                                                                                    {
                                                                                                                                                                                                                                                        myRegex = new Regex("milw0rm.com[^<>]*");
                                                                                                                                                                                                                                                        strTemp = myRegex.Match(theurl).ToString();
                                                                                                                                                                                                                                                        if (strTemp != "")
                                                                                                                                                                                                                                                        {
                                                                                                                                                                                                                                                            mySource = "MILW0RM";
                                                                                                                                                                                                                                                            //TODO: Exploit?
                                                                                                                                                                                                                                                        }
                                                                                                                                                                                                                                                        else
                                                                                                                                                                                                                                                        {
                                                                                                                                                                                                                                                            myRegex = new Regex(".nai.com[^<>]*");  //TODO Review
                                                                                                                                                                                                                                                            strTemp = myRegex.Match(theurl).ToString();
                                                                                                                                                                                                                                                            if (strTemp != "")
                                                                                                                                                                                                                                                            {
                                                                                                                                                                                                                                                                mySource = "NAI";
                                                                                                                                                                                                                                                            }
                                                                                                                                                                                                                                                            else
                                                                                                                                                                                                                                                            {
                                                                                                                                                                                                                                                                myRegex = new Regex("netbsd.org[^<>]*");
                                                                                                                                                                                                                                                                strTemp = myRegex.Match(theurl).ToString();
                                                                                                                                                                                                                                                                if (strTemp != "")
                                                                                                                                                                                                                                                                {
                                                                                                                                                                                                                                                                    mySource = "NETBSD";
                                                                                                                                                                                                                                                                }
                                                                                                                                                                                                                                                                else
                                                                                                                                                                                                                                                                {
                                                                                                                                                                                                                                                                    myRegex = new Regex("NetBSD.org[^<>]*");    //TODO Review lowercase
                                                                                                                                                                                                                                                                    strTemp = myRegex.Match(theurl).ToString();
                                                                                                                                                                                                                                                                    if (strTemp != "")
                                                                                                                                                                                                                                                                    {
                                                                                                                                                                                                                                                                        mySource = "NETBSD";
                                                                                                                                                                                                                                                                    }
                                                                                                                                                                                                                                                                    else
                                                                                                                                                                                                                                                                    {
                                                                                                                                                                                                                                                                        myRegex = new Regex("openbsd.org[^<>]*");
                                                                                                                                                                                                                                                                        strTemp = myRegex.Match(theurl).ToString();
                                                                                                                                                                                                                                                                        if (strTemp != "")
                                                                                                                                                                                                                                                                        {
                                                                                                                                                                                                                                                                            mySource = "OPENBSD";
                                                                                                                                                                                                                                                                        }
                                                                                                                                                                                                                                                                        else
                                                                                                                                                                                                                                                                        {
                                                                                                                                                                                                                                                                            myRegex = new Regex("openpkg.com[^<>]*");
                                                                                                                                                                                                                                                                            strTemp = myRegex.Match(theurl).ToString();
                                                                                                                                                                                                                                                                            if (strTemp != "")
                                                                                                                                                                                                                                                                            {
                                                                                                                                                                                                                                                                                mySource = "OPENPKG";
                                                                                                                                                                                                                                                                            }
                                                                                                                                                                                                                                                                            else
                                                                                                                                                                                                                                                                            {
                                                                                                                                                                                                                                                                                myRegex = new Regex("suse.com[^<>]*");
                                                                                                                                                                                                                                                                                strTemp = myRegex.Match(theurl).ToString();
                                                                                                                                                                                                                                                                                if (strTemp != "")
                                                                                                                                                                                                                                                                                {
                                                                                                                                                                                                                                                                                    mySource = "SUSE";
                                                                                                                                                                                                                                                                                }
                                                                                                                                                                                                                                                                                else
                                                                                                                                                                                                                                                                                {
                                                                                                                                                                                                                                                                                    myRegex = new Regex("novell.com[^<>]*");
                                                                                                                                                                                                                                                                                    strTemp = myRegex.Match(theurl).ToString();
                                                                                                                                                                                                                                                                                    if (strTemp != "")
                                                                                                                                                                                                                                                                                    {
                                                                                                                                                                                                                                                                                        mySource = "SUSE";  //TODO Review
                                                                                                                                                                                                                                                                                    }
                                                                                                                                                                                                                                                                                    else
                                                                                                                                                                                                                                                                                    {
                                                                                                                                                                                                                                                                                        myRegex = new Regex("/suse/[^<>]*");
                                                                                                                                                                                                                                                                                        strTemp = myRegex.Match(theurl).ToString();
                                                                                                                                                                                                                                                                                        if (strTemp != "")
                                                                                                                                                                                                                                                                                        {
                                                                                                                                                                                                                                                                                            mySource = "SUSE";  //TODO Review
                                                                                                                                                                                                                                                                                        }
                                                                                                                                                                                                                                                                                        else
                                                                                                                                                                                                                                                                                        {
                                                                                                                                                                                                                                                                                            myRegex = new Regex("suse.de[^<>]*");
                                                                                                                                                                                                                                                                                            strTemp = myRegex.Match(theurl).ToString();
                                                                                                                                                                                                                                                                                            if (strTemp != "")
                                                                                                                                                                                                                                                                                            {
                                                                                                                                                                                                                                                                                                mySource = "SUSE";
                                                                                                                                                                                                                                                                                            }
                                                                                                                                                                                                                                                                                            else
                                                                                                                                                                                                                                                                                            {
                                                                                                                                                                                                                                                                                                myRegex = new Regex("opensuse.org[^<>]*");
                                                                                                                                                                                                                                                                                                strTemp = myRegex.Match(theurl).ToString();
                                                                                                                                                                                                                                                                                                if (strTemp != "")
                                                                                                                                                                                                                                                                                                {
                                                                                                                                                                                                                                                                                                    mySource = "SUSE";
                                                                                                                                                                                                                                                                                                }
                                                                                                                                                                                                                                                                                                else
                                                                                                                                                                                                                                                                                                {
                                                                                                                                                                                                                                                                                                    myRegex = new Regex("turbolinux.com[^<>]*");
                                                                                                                                                                                                                                                                                                    strTemp = myRegex.Match(theurl).ToString();
                                                                                                                                                                                                                                                                                                    if (strTemp != "")
                                                                                                                                                                                                                                                                                                    {
                                                                                                                                                                                                                                                                                                        mySource = "TURBO";
                                                                                                                                                                                                                                                                                                    }
                                                                                                                                                                                                                                                                                                    else
                                                                                                                                                                                                                                                                                                    {
                                                                                                                                                                                                                                                                                                        myRegex = new Regex("ubuntulinux.org[^<>]*");
                                                                                                                                                                                                                                                                                                        strTemp = myRegex.Match(theurl).ToString();
                                                                                                                                                                                                                                                                                                        if (strTemp != "")
                                                                                                                                                                                                                                                                                                        {
                                                                                                                                                                                                                                                                                                            mySource = "UBUNTU";
                                                                                                                                                                                                                                                                                                        }
                                                                                                                                                                                                                                                                                                        else
                                                                                                                                                                                                                                                                                                        {
                                                                                                                                                                                                                                                                                                            myRegex = new Regex("attrition.org/pipermail/vim[^<>]*");
                                                                                                                                                                                                                                                                                                            strTemp = myRegex.Match(theurl).ToString();
                                                                                                                                                                                                                                                                                                            if (strTemp != "")
                                                                                                                                                                                                                                                                                                            {
                                                                                                                                                                                                                                                                                                                mySource = "VIM";
                                                                                                                                                                                                                                                                                                            }
                                                                                                                                                                                                                                                                                                            else
                                                                                                                                                                                                                                                                                                            {
                                                                                                                                                                                                                                                                                                                myRegex = new Regex("vuln-dev[^<>]*");
                                                                                                                                                                                                                                                                                                                strTemp = myRegex.Match(theurl).ToString();
                                                                                                                                                                                                                                                                                                                if (strTemp != "")
                                                                                                                                                                                                                                                                                                                {
                                                                                                                                                                                                                                                                                                                    mySource = "VULN-DEV";
                                                                                                                                                                                                                                                                                                                }
                                                                                                                                                                                                                                                                                                                else
                                                                                                                                                                                                                                                                                                                {
                                                                                                                                                                                                                                                                                                                    myRegex = new Regex("vulnwatch[^<>]*");
                                                                                                                                                                                                                                                                                                                    strTemp = myRegex.Match(theurl).ToString();
                                                                                                                                                                                                                                                                                                                    if (strTemp != "")
                                                                                                                                                                                                                                                                                                                    {
                                                                                                                                                                                                                                                                                                                        mySource = "VULNWATCH";
                                                                                                                                                                                                                                                                                                                    }
                                                                                                                                                                                                                                                                                                                    else
                                                                                                                                                                                                                                                                                                                    {
                                                                                                                                                                                                                                                                                                                        myRegex = new Regex("frsirt[^<>]*");
                                                                                                                                                                                                                                                                                                                        strTemp = myRegex.Match(theurl).ToString();
                                                                                                                                                                                                                                                                                                                        if (strTemp != "")
                                                                                                                                                                                                                                                                                                                        {
                                                                                                                                                                                                                                                                                                                            mySource = "VUPEN"; //TODO Review (+ k-otik)
                                                                                                                                                                                                                                                                                                                        }
                                                                                                                                                                                                                                                                                                                        else
                                                                                                                                                                                                                                                                                                                        {
                                                                                                                                                                                                                                                                                                                            myRegex = new Regex("win2ksecadvice[^<>]*");
                                                                                                                                                                                                                                                                                                                            strTemp = myRegex.Match(theurl).ToString();
                                                                                                                                                                                                                                                                                                                            if (strTemp != "")
                                                                                                                                                                                                                                                                                                                            {
                                                                                                                                                                                                                                                                                                                                mySource = "WIN2KSEC";
                                                                                                                                                                                                                                                                                                                            }
                                                                                                                                                                                                                                                                                                                            else
                                                                                                                                                                                                                                                                                                                            {
                                                                                                                                                                                                                                                                                                                                myRegex = new Regex("mitre.org[^<>]*");
                                                                                                                                                                                                                                                                                                                                strTemp = myRegex.Match(theurl).ToString();
                                                                                                                                                                                                                                                                                                                                if (strTemp != "")
                                                                                                                                                                                                                                                                                                                                {
                                                                                                                                                                                                                                                                                                                                    mySource = "MITRE";
                                                                                                                                                                                                                                                                                                                                }
                                                                                                                                                                                                                                                                                                                                else
                                                                                                                                                                                                                                                                                                                                {
                                                                                                                                                                                                                                                                                                                                    myRegex = new Regex("packetstormsecurity[^<>]*");
                                                                                                                                                                                                                                                                                                                                    strTemp = myRegex.Match(theurl).ToString();
                                                                                                                                                                                                                                                                                                                                    if (strTemp != "")
                                                                                                                                                                                                                                                                                                                                    {
                                                                                                                                                                                                                                                                                                                                        mySource = "PACKETSTORM";
                                                                                                                                                                                                                                                                                                                                    }
                                                                                                                                                                                                                                                                                                                                    else
                                                                                                                                                                                                                                                                                                                                    {
                                                                                                                                                                                                                                                                                                                                        myRegex = new Regex("ibm.com[^<>]*");
                                                                                                                                                                                                                                                                                                                                        strTemp = myRegex.Match(theurl).ToString();
                                                                                                                                                                                                                                                                                                                                        if (strTemp != "")
                                                                                                                                                                                                                                                                                                                                        {
                                                                                                                                                                                                                                                                                                                                            mySource = "IBM";
                                                                                                                                                                                                                                                                                                                                        }
                                                                                                                                                                                                                                                                                                                                        else
                                                                                                                                                                                                                                                                                                                                        {
                                                                                                                                                                                                                                                                                                                                            myRegex = new Regex("osvdb.org[^<>]*");
                                                                                                                                                                                                                                                                                                                                            strTemp = myRegex.Match(theurl).ToString();
                                                                                                                                                                                                                                                                                                                                            if (strTemp != "")
                                                                                                                                                                                                                                                                                                                                            {
                                                                                                                                                                                                                                                                                                                                                mySource = "OSVDB";
                                                                                                                                                                                                                                                                                                                                            }
                                                                                                                                                                                                                                                                                                                                            else
                                                                                                                                                                                                                                                                                                                                            {
                                                                                                                                                                                                                                                                                                                                                myRegex = new Regex("us-cert.gov[^<>]*");
                                                                                                                                                                                                                                                                                                                                                strTemp = myRegex.Match(theurl).ToString();
                                                                                                                                                                                                                                                                                                                                                if (strTemp != "")
                                                                                                                                                                                                                                                                                                                                                {
                                                                                                                                                                                                                                                                                                                                                    mySource = "US-CERT";   //TODO Review vs CERT
                                                                                                                                                                                                                                                                                                                                                }
                                                                                                                                                                                                                                                                                                                                                else
                                                                                                                                                                                                                                                                                                                                                {
                                                                                                                                                                                                                                                                                                                                                    myRegex = new Regex("scip.ch/?vuldb[^<>]*");
                                                                                                                                                                                                                                                                                                                                                    strTemp = myRegex.Match(theurl).ToString();
                                                                                                                                                                                                                                                                                                                                                    if (strTemp != "")
                                                                                                                                                                                                                                                                                                                                                    {
                                                                                                                                                                                                                                                                                                                                                        mySource = "SCIP";
                                                                                                                                                                                                                                                                                                                                                    }
                                                                                                                                                                                                                                                                                                                                                    else
                                                                                                                                                                                                                                                                                                                                                    {
                                                                                                                                                                                                                                                                                                                                                        myRegex = new Regex("scaprepo.com/view.jsp?id=[^<>]*");
                                                                                                                                                                                                                                                                                                                                                        strTemp = myRegex.Match(theurl).ToString();
                                                                                                                                                                                                                                                                                                                                                        if (strTemp != "")
                                                                                                                                                                                                                                                                                                                                                        {
                                                                                                                                                                                                                                                                                                                                                            mySource = "SCAPREPO";
                                                                                                                                                                                                                                                                                                                                                        }
                                                                                                                                                                                                                                                                                                                                                        else
                                                                                                                                                                                                                                                                                                                                                        {
                                                                                                                                                                                                                                                                                                                                                            myRegex = new Regex("adobe.com/[^<>]*");
                                                                                                                                                                                                                                                                                                                                                            //http://adobe.com/support/security/bulletins/apsb13-14.html
                                                                                                                                                                                                                                                                                                                                                            strTemp = myRegex.Match(theurl).ToString();
                                                                                                                                                                                                                                                                                                                                                            if (strTemp != "")
                                                                                                                                                                                                                                                                                                                                                            {
                                                                                                                                                                                                                                                                                                                                                                mySource = "ADOBE";
                                                                                                                                                                                                                                                                                                                                                            }
                                                                                                                                                                                                                                                                                                                                                            else
                                                                                                                                                                                                                                                                                                                                                            {
                                                                                                                                                                                                                                                                                                                                                                myRegex = new Regex("scn.sap.com/[^<>]*");
                                                                                                                                                                                                                                                                                                                                                                strTemp = myRegex.Match(theurl).ToString();
                                                                                                                                                                                                                                                                                                                                                                if (strTemp != "")
                                                                                                                                                                                                                                                                                                                                                                {
                                                                                                                                                                                                                                                                                                                                                                    mySource = "SAP";
                                                                                                                                                                                                                                                                                                                                                                }
                                                                                                                                                                                                                                                                                                                                                                else
                                                                                                                                                                                                                                                                                                                                                                {
                                                                                                                                                                                                                                                                                                                                                                    myRegex = new Regex("service.sap.com/[^<>]*");
                                                                                                                                                                                                                                                                                                                                                                    strTemp = myRegex.Match(theurl).ToString();
                                                                                                                                                                                                                                                                                                                                                                    //https://service.sap.com/sap/support/notes/1816536
                                                                                                                                                                                                                                                                                                                                                                    if (strTemp != "")
                                                                                                                                                                                                                                                                                                                                                                    {
                                                                                                                                                                                                                                                                                                                                                                        mySource = "SAP";
                                                                                                                                                                                                                                                                                                                                                                    }
                                                                                                                                                                                                                                                                                                                                                                    else
                                                                                                                                                                                                                                                                                                                                                                    {
                                                                                                                                                                                                                                                                                                                                                                        Console.WriteLine("NOTE: no SOURCE found for " + theurl);
                                                                                                                                                                                                                                                                                                                                                                    }
                                                                                                                                                                                                                                                                                                                                                                }
                                                                                                                                                                                                                                                                                                                                                            }
                                                                                                                                                                                                                                                                                                                                                        }
                                                                                                                                                                                                                                                                                                                                                    }
                                                                                                                                                                                                                                                                                                                                                }
                                                                                                                                                                                                                                                                                                                                            }
                                                                                                                                                                                                                                                                                                                                        }
                                                                                                                                                                                                                                                                                                                                    }
                                                                                                                                                                                                                                                                                                                                }
                                                                                                                                                                                                                                                                                                                            }
                                                                                                                                                                                                                                                                                                                        }
                                                                                                                                                                                                                                                                                                                    }
                                                                                                                                                                                                                                                                                                                }
                                                                                                                                                                                                                                                                                                            }
                                                                                                                                                                                                                                                                                                        }
                                                                                                                                                                                                                                                                                                    }
                                                                                                                                                                                                                                                                                                }
                                                                                                                                                                                                                                                                                            }
                                                                                                                                                                                                                                                                                        }
                                                                                                                                                                                                                                                                                    }
                                                                                                                                                                                                                                                                                }
                                                                                                                                                                                                                                                                            }
                                                                                                                                                                                                                                                                        }
                                                                                                                                                                                                                                                                    }
                                                                                                                                                                                                                                                                }
                                                                                                                                                                                                                                                            }
                                                                                                                                                                                                                                                        }
                                                                                                                                                                                                                                                    }
                                                                                                                                                                                                                                                }
                                                                                                                                                                                                                                            }
                                                                                                                                                                                                                                        }
                                                                                                                                                                                                                                    }
                                                                                                                                                                                                                                }
                                                                                                                                                                                                                            }
                                                                                                                                                                                                                        }
                                                                                                                                                                                                                    }
                                                                                                                                                                                                                }
                                                                                                                                                                                                            }
                                                                                                                                                                                                        }
                                                                                                                                                                                                    }
                                                                                                                                                                                                }
                                                                                                                                                                                            }
                                                                                                                                                                                        }
                                                                                                                                                                                    }
                                                                                                                                                                                }
                                                                                                                                                                            }
                                                                                                                                                                        }
                                                                                                                                                                    }
                                                                                                                                                                }
                                                                                                                                                            }
                                                                                                                                                        }
                                                                                                                                                    }
                                                                                                                                                }
                                                                                                                                            }
                                                                                                                                        }
                                                                                                                                    }
                                                                                                                                }
                                                                                                                            }
                                                                                                                        }
                                                                                                                    }
                                                                                                                }
                                                                                                            }
                                                                                                        }
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (mySource == "MISC")
            {
                Console.WriteLine("NOTE: MISC source for :" + theurl);
            }
            return mySource;
        
        }

        static public string[] ExtractURLs(string str)
        {
            // match.Groups["name"].Value - URL Name
            // match.Groups["url"].Value - URI
            string RegexPattern = @"<a.*?href=[""'](?<url>.*?)[""'].*?>(?<name>.*?)</a>";

            // Find matches.
            System.Text.RegularExpressions.MatchCollection matches
                = System.Text.RegularExpressions.Regex.Matches(str, RegexPattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            string[] MatchList = new string[matches.Count];

            // Report on each match.
            int c = 0;
            foreach (System.Text.RegularExpressions.Match match in matches)
            {
                MatchList[c] = match.Groups["url"].Value;
                c++;
            }

            return MatchList;
        }

        static private void Import_exploitdb()
        {
            //Search on exploit-db.com (EDB) using a list of CVEs in a text file
            Console.WriteLine("DEBUG "+DateTimeOffset.Now);
            Console.WriteLine("Import_exploitdb");
            string ligne = string.Empty;

            //*****************************************************************************************************
            //ListCVE();

            //*****************************************************************************************************
            //Searching Exploits
            
            StreamReader monStreamReader = new StreamReader("cves.txt");
            
            StreamWriter monStreamWriter = new StreamWriter("jerome.log");
            try
            {
                ligne = monStreamReader.ReadLine();    

                while (ligne != null)
                //while (ligne != "1999-1000")
                //CVE-2000-0288
                {
                    Console.WriteLine("DEBUG ligne="+ligne);
                    SearchExploitsForCVE(ligne, monStreamWriter);
                    ligne = monStreamReader.ReadLine();
                    Thread.Sleep(5000); //Hardcoded
                }                
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Jerome Exception = {0}", ex));
            }

            monStreamReader.Close();
            monStreamWriter.Close();
            Console.WriteLine("DEBUG "+DateTimeOffset.Now);
            Console.WriteLine("DEBUG FINISHED Import_exploitdb");
            

            //************************************************************************************************************
            //  PARSING EXPLOIT-DB

            ParseEXPLOITDB("jerome.log");   //big file

            //*****************************************************************************************************
            //  INSERT SQL
            InsertEXPLOITDB();

            
            //FREE
            //model.Dispose();
            //model = null;
        }

        static private void SearchBIDForCVE(string CVElook, StreamWriter monStreamWriter, int iRetry=0)
        {
            //TODO
            //Request POST to http://www.securityfocus.com/bid
            string ResponseText = "";
            //string MyCookie = "";

            if (bRequestSecurityfocus)
            {
                try
                {
                    if (iRetry > 2)   //TODO Hardcoded
                    {
                        return;
                    }

                    Console.WriteLine("DEBUG SearchBIDForCVE " + CVElook);
                    string sURLSearchBIDForCVE = "http://www.securityfocus.com/bid";    //?CVE=" + CVElook;

                    if (bRequestSecurityfocus)
                    {
                        Console.WriteLine("DEBUG Request to " + sURLSearchBIDForCVE);

                        StreamReader SR = null;
                        HttpWebResponse response = null;

                        HttpWebRequest request;
                        request = (HttpWebRequest)HttpWebRequest.Create(sURLSearchBIDForCVE);
                        request.Method = "POST";
                        string postData = "op=display_list&c=12&vendor=&title=&version=&CVE=" + CVElook;    //HARDCODED
                        byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                        request.ContentLength = byteArray.Length;
                        // Get the request stream.
                        Stream dataStream = request.GetRequestStream();
                        // Write the data to the request stream.
                        dataStream.Write(byteArray, 0, byteArray.Length);
                        // Close the Stream object.
                        dataStream.Close();

                        response = (HttpWebResponse)request.GetResponse();
                        SR = new StreamReader(response.GetResponseStream());
                        ResponseText = SR.ReadToEnd();

                        SR.Close();
                        response.Close();

                        string sCurrentPath = Directory.GetCurrentDirectory();
                        System.IO.File.WriteAllText(sCurrentPath + @"\securityfocus\" + CVElook + ".txt", ResponseText);    //Hardcoded

                    }


                    //Console.WriteLine("DEBUG TODO ResponseText=" + ResponseText);
                    //TODO: collect the Vendors

                    /*
                    <a href="/bid/50832"><span class="headline">Ubuntu Update Manager Insecure Temporary Directory Creation Vulnerability</span></a><br/>
	    <span class="date">2011-11-28</span><br/>
	    <a href="/bid/50832">http://www.securityfocus.com/bid/50832</a><br/><br/>
                    */

                    //HARDCODED
                    Regex myRegex = new Regex("<span class=\"headline\">(.*?)<br/><br/>", RegexOptions.Singleline);
                    MatchCollection myBIDs = myRegex.Matches(ResponseText);
                    foreach (Match matchBID in myBIDs)
                    {
                        foreach (Capture capture01 in matchBID.Captures)
                        {
                            //Console.WriteLine(capture01.Value);

                            myRegex = new Regex("<span class=\"headline\">(.*?)</span>");
                            string sBIDTitle = myRegex.Match(capture01.Value).ToString();
                            sBIDTitle = sBIDTitle.Replace("<span class=\"headline\">", "");
                            sBIDTitle = sBIDTitle.Replace("</span>", "");
                            Console.WriteLine("DEBUG sBIDTitle=" + sBIDTitle);

                            myRegex = new Regex("<span class=\"date\">(.*?)</span>");
                            string sBIDDate = myRegex.Match(capture01.Value).ToString();
                            sBIDDate = sBIDDate.Replace("<span class=\"date\">", "");
                            sBIDDate = sBIDDate.Replace("</span>", "");
                            Console.WriteLine("DEBUG sBIDDate=" + sBIDDate);

                            myRegex = new Regex("bid/(.*?)\">");
                            string sBIDID = myRegex.Match(capture01.Value).ToString();
                            sBIDID = sBIDID.Replace("bid/", "");
                            sBIDID = sBIDID.Replace("\">", "");
                            Console.WriteLine("DEBUG sBIDID=" + sBIDID);

                        }
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                    Console.WriteLine(string.Format("Exception = {0}. Retrying...", ex));
                    iRetry++;
                    SearchBIDForCVE(CVElook, monStreamWriter, iRetry);
                    return;
                }
            }
        }

        static private void SearchExploitsForCVE(string CVElook, StreamWriter monStreamWriter, int iRetry=0)
        {
            //TODO check the date of the file to reduce requests
            
            //2010-2204
            //Stream newStream;
            string ResponseText = "";
            //string MyCookie = "";
            string sCurrentPath = Directory.GetCurrentDirectory();
            

            if (bRequestExploitDB)
            {
                if (iRetry > 2)   //TODO Hardcoded
                {
                    return;
                }
                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                Console.WriteLine("DEBUG SearchExploitsForCVE");

                try
                {
                    string sURLExploitSearch = "http://www.exploit-db.com/search/?action=search&filter_cve=" + CVElook.Replace("CVE-", "");
                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                    Console.WriteLine("DEBUG Request to " + sURLExploitSearch);

                    StreamReader SR = null;
                    HttpWebResponse response = null;

                    HttpWebRequest request;
                    request = (HttpWebRequest)HttpWebRequest.Create(sURLExploitSearch);
                    request.Method = "GET";
                    //request.ContentType = "application/xml";
                    /*
                    string postData = "filter_cve=2010-2204";
                    byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                    request.ContentLength = byteArray.Length;
                    Stream dataStream = request.GetRequestStream();
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();
                    */

                    response = (HttpWebResponse)request.GetResponse();
                    SR = new StreamReader(response.GetResponseStream());
                    ResponseText = SR.ReadToEnd();

                    SR.Close();
                    response.Close();

                    System.IO.File.WriteAllText(sCurrentPath + @"\exploit-db\" + CVElook + ".txt", ResponseText);   //Hardcoded
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                    Console.WriteLine(string.Format("Exception = {0}. Retrying...", ex));
                    iRetry++;
                    SearchExploitsForCVE(CVElook, monStreamWriter, iRetry);
                    return;
                }
            }

            //Read the file
            try
            {
                ResponseText = System.IO.File.ReadAllText(sCurrentPath + @"\exploit-db\" + CVElook + ".txt");
            }
            catch (Exception exReadAllText)
            {
                //TODO?
            }

            if (ResponseText.Contains("No results") || ResponseText=="")
            {
                //No luck
                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                Console.WriteLine("DEBUG No results");
            }
            else
            {
                

                //HtmlDocument myHTML = new HtmlDocument();

                //RegexOptions options = RegexOptions.Multiline;
                Regex objNaturalPattern = new Regex("<tbody[^>]*>(.*?)</tbody>", RegexOptions.Singleline);  //Hardcoded
                string myExploits = string.Empty;
                myExploits = objNaturalPattern.Match(ResponseText).ToString();

                //Console.WriteLine("DEBUG myExploits "+myExploits);

                /*
                <tr>
				    <td class="list_explot_date">2010-06-29</td>

				    <td class="list_explot_dlink">
											    <a  href="http://www.exploit-db.com/download/14121">
										    <img src="http://www.exploit-db.com/wp-content/themes/exploit/images/edb-download.png" alt="Exploit Code Downloads" title="Exploit Code Downloads" style="padding:0px 3px 0px 3px" border="0" /></a>
				    </td>
				    <td class="list_explot_app">
									    	    <a  href="http://www.exploit-db.com/application/14121">
				    		    <img src="http://www.exploit-db.com/wp-content/themes/exploit/images/application_error.png" alt="Download Vulnerable Application" title="Download Vulnerable Application" style="padding:0px 3px 0px 3px" border="0" />
				    	    </a>
									    </td>

				    <td class="list_explot_verification">
											    <img src="http://www.exploit-db.com/wp-content/themes/exploit/images/accept.png" width="16" height="16" alt="Verified" title="Verified" />
									    </td>
				    <td class="list_explot_description">
											    <a  href="http://www.exploit-db.com/exploits/14121"> Adobe Reader 9.3.2 (CoolType.dll) Remote Memory Corruption / DoS Vulnerability</a>
									    </td>
				    <td class="list_explot_clicks">
					    1907				</td>

				    <td class="list_explot_platform">
											    <a  href="http://www.exploit-db.com/platform/?p=multiple">multiple</a>
									    </td>
				    <td class="list_explot_author">
					    <a  href="http://www.exploit-db.com/author/?a=1361" title="LiquidWorm">
													    LiquidWorm											</a>
				    </td>
			    </tr>

                */

                MatchCollection myTRs;
                MatchCollection myTDs;
                //            StreamWriter monStreamWriter = new StreamWriter("jerome.log");
                try
                {
                    if (monStreamWriter != null)
                    {
                        monStreamWriter.WriteLine("CVE-" + CVElook.Replace("CVE-", ""));
                        monStreamWriter.WriteLine(myExploits);
                    }
                }
                catch(Exception exWriteExploits)
                {
                    Console.WriteLine("Exception: exWriteExploits " + exWriteExploits.Message + " " + exWriteExploits.InnerException);
                }
                //Console.WriteLine("ecriture");

                #region codenotneeded
                //This will be done by ParseEXPLOITDB()

                /*
                Regex myRegex01 = new Regex("<tr[^>]*>(.*?)</tr>", RegexOptions.Singleline);
                myTRs = myRegex01.Matches(myExploits);
                foreach (Match match01 in myTRs)
                {
                    //monStreamWriter.WriteLine(match01.Captures);
                    foreach (Capture capture01 in match01.Captures)
                    {
                        //Console.WriteLine(capture01.Value);
                        Regex myRegex02 = new Regex("<td[^>]*>(.*?)</td>", RegexOptions.Singleline);
                        myTDs = myRegex02.Matches(myExploits);
                        foreach (Match match02 in myTDs)
                        {
                            foreach (Capture capture02 in match02.Captures)
                            {
                                //Console.WriteLine(capture02.Value);

                                Regex RegexPattern = new Regex(@"<a.*?href=[""'](.*?)[""'].*?>(.*?)</a>", RegexOptions.Singleline);
                                string myLink = RegexPattern.Match(capture02.Value).ToString();

                                Console.WriteLine("DEBUG TODO myLink="+myLink);
                                //TODO: we could get the name of the exploit from here
                                //TODO: we could get the platform of the exploit from here
                                //TODO: we could get the author of the exploit from here

                                RegexPattern = new Regex(@"<a.*?href=[""'](.*?)[""'].*?>", RegexOptions.Singleline);
                                string myURL = RegexPattern.Match(myLink).ToString();
                                Console.WriteLine("DEBUG TODO myURL=" + myURL);
                                //TODO: we could get the platform of the exploit from here
                                //TODO: we could get the author of the exploit from here

                                //<a href="http://www.exploit-db.com/exploits/32568">
                                Regex RegexEDBID = new Regex("exploit-db.com/exploits/[^<>]*");
                                string sEDBID = RegexEDBID.Match(myURL).ToString();
                                if (sEDBID != "")
                                {
                                    //Clean exploit-db ID
                                    sEDBID = sEDBID.Replace("\"", "");
                                    sEDBID = sEDBID.Replace(">", "").Trim();
                                    Console.WriteLine("DEBUG FOUND EDB-ID:" + sEDBID);

                                    XORCISMEntities modelX= new XORCISMEntities();
                                    EXPLOIT oCVEEXPLOIT = modelX.EXPLOIT.Where(o => o.Referential == "exploit-db" && o.RefID == sEDBID).FirstOrDefault();
                                    if(oCVEEXPLOIT!=null)
                                    {
                                        //Update EXPLOIT
                                    }
                                    else
                                    {
                                        oCVEEXPLOIT = new EXPLOIT();
                                        oCVEEXPLOIT.Referential = "exploit-db";
                                        oCVEEXPLOIT.RefID = sEDBID;
                                        oCVEEXPLOIT.Location = "http://www.exploit-db.com/download/" + sEDBID;
                                        oCVEEXPLOIT.CreatedDate = DateTimeOffset.Now;
                                        oCVEEXPLOIT.timestamp = DateTimeOffset.Now;
                                        try
                                        {
                                            modelX.EXPLOIT.Add(oCVEEXPLOIT);
                                            modelX.SaveChanges();
                                            Console.WriteLine("DEBUG Added new CVEEXPLOIT exploit-db ID " + sEDBID);
                                        }
                                        catch(Exception exoCVEEXPLOIT)
                                        {
                                            Console.WriteLine("Exception: exoCVEEXPLOIT " + exoCVEEXPLOIT.Message + " " + exoCVEEXPLOIT.InnerException);
                                        }
                                    }

                                    int iVulnerabilityID = modelX.VULNERABILITY.Where(o => o.VULReferentialID == CVElook).Select(o => o.VulnerabilityID).FirstOrDefault();
                                    EXPLOITFORVULNERABILITY oEXPLOITFORCVE = modelX.EXPLOITFORVULNERABILITY.Where(o => o.VulnerabilityID == iVulnerabilityID && o.ExploitID == oCVEEXPLOIT.ExploitID).FirstOrDefault();
                                    if(oEXPLOITFORCVE!=null)
                                    {
                                        //Update EXPLOITFORVULNERABILITY
                                    }
                                    else
                                    {
                                        oEXPLOITFORCVE = new EXPLOITFORVULNERABILITY();
                                        oEXPLOITFORCVE.ExploitID=oCVEEXPLOIT.ExploitID;
                                        oEXPLOITFORCVE.VulnerabilityID=iVulnerabilityID;
                                        //oEXPLOITFORCVE.VocabularyID=  //TODO CVE
                                        //CollectionMethodID    //TODO
                                        oEXPLOITFORCVE.CreatedDate=DateTimeOffset.Now;
                                        oEXPLOITFORCVE.timestamp=DateTimeOffset.Now;
                                        try
                                        {
                                            modelX.EXPLOITFORVULNERABILITY.Add(oEXPLOITFORCVE);
                                            modelX.SaveChanges();
                                            Console.WriteLine("DEBUG Added new EXPLOITFORVULNERABILITY");
                                        }
                                        catch(Exception exoEXPLOITFORCVE)
                                        {
                                            Console.WriteLine("Exception: exoEXPLOITFORCVE " + exoEXPLOITFORCVE.Message + " " + exoEXPLOITFORCVE.InnerException);
                                        }
                                    }

                                }

                                RegexPattern = new Regex(@">(.*?)</a>", RegexOptions.Singleline);
                                string myContent = RegexPattern.Match(myLink).ToString();
                                Console.WriteLine("DEBUG TODO myContent=" + myContent);
                                //TODO: we could get the name of the exploit from here
                            }
                        }
                    }
                }
                */
                #endregion codenotneeded
            }
            //monStreamWriter.Close();
        }

        static private void ParseEXPLOITDB(string sInputFile="jerome2.log", Boolean bAppend=true)
        {
            //This function parse a file where the response to a search request on exploit-db is stored
            //Extract the information found from the result page

            //StreamReader monStreamReader = new StreamReader("jerome.log");
            //StreamWriter monStreamWriter = new StreamWriter("CVE-EXPLOITS.txt");
            StreamReader monStreamReader = new StreamReader(@sInputFile);  //jerome.log (big file) or jerome2.log (just for 1 CVE)
            StreamWriter monStreamWriter = new StreamWriter("CVE-EXPLOITS.txt", bAppend);   //Hardcoded

            string ligne = monStreamReader.ReadLine();
            string curCVE = string.Empty;
            string curSploit = string.Empty;    //int
            string strTemp = string.Empty;

            while (ligne != null)
            {
                Regex myRegexCVE = new Regex("CVE-[0-9][0-9][0-9][0-9]-[0-9][0-9][0-9][0-9]"); //TODO review \d+

                strTemp = myRegexCVE.Match(ligne).ToString();
                if (strTemp != "")
                {
                    curCVE = strTemp;
                    monStreamWriter.WriteLine(curCVE.Trim());
                    Console.WriteLine("DEBUG ParseEXPLOITDB curCVE=" + strTemp);

                }

                //Hardcoded
                Regex myRegex = new Regex("<td class=\"list_explot_date\">[^<>]*</td>");
                strTemp = myRegex.Match(ligne).ToString();
                if (strTemp != "")
                {
                    //monStreamWriter.WriteLine(curCVE);
                    strTemp = strTemp.Replace("<td class=\"list_explot_date\">", "");
                    strTemp = strTemp.Replace("</td>", "");
                    monStreamWriter.WriteLine(strTemp.Trim());
                    Console.WriteLine("DEBUG ParseEXPLOITDB date=" + strTemp);
                }

                myRegex = new Regex("exploit-db.com/download/[^<>]*\">");
                strTemp = myRegex.Match(ligne).ToString();
                if (strTemp != "")
                {
                    strTemp = strTemp.Replace("<a  href=\"", "");
                    strTemp = strTemp.Replace("\">", "");
                    monStreamWriter.WriteLine(strTemp.Trim());
                    curSploit = strTemp.Trim().Replace("http://www.exploit-db.com/download/", "");
                    curSploit = curSploit.Replace("https://www.exploit-db.com/download/", "");
                    curSploit = curSploit.Replace("http://exploit-db.com/download/", "");
                    curSploit = curSploit.Replace("https://exploit-db.com/download/", "");
                    curSploit = curSploit.Replace("exploit-db.com/download/", "");
                    monStreamWriter.WriteLine(curSploit);
                    Console.WriteLine("DEBUG ParseEXPLOITDB curSploit=" + curSploit);
                }

                //TODO: Verified
                if(ligne.Contains("alt=\"Verified\""))
                {

                }

                Regex myRegexEXPLOITDB = new Regex("http://www.exploit-db.com/exploits/[^<>]*");   //Description (name)
                strTemp = myRegexEXPLOITDB.Match(ligne).ToString();
                if (strTemp != "")
                {
                    myRegex = new Regex("\">[^<>]*");
                    strTemp = myRegexEXPLOITDB.Match(ligne).ToString();
                    strTemp = strTemp.Replace("\">", "");
                    strTemp = strTemp.Replace("CVE-", "CVE:");  //TODO Review
                    monStreamWriter.WriteLine(strTemp.Trim());
                    Console.WriteLine("DEBUG ParseEXPLOITDB name=" + strTemp);
                    //TODO
                    //MS13-005
                    //PATCH
                }

                myRegex = new Regex("http://www.exploit-db.com/platform/[^<>]*\">");
                strTemp = myRegex.Match(ligne).ToString();
                if (strTemp != "")
                {
                    strTemp = strTemp.Replace("<a  href=\"", "");
                    strTemp = strTemp.Replace("</a>", "");
                    strTemp = strTemp.Replace("http://www.exploit-db.com/platform/?p=", "");
                    strTemp = strTemp.Replace("\">", "");
                    monStreamWriter.WriteLine(strTemp.Trim());
                    Console.WriteLine("DEBUG ParseEXPLOITDB platform=" + strTemp);
                }

                myRegex = new Regex("http://www.exploit-db.com/author/[^<>]*\"");
                strTemp = myRegex.Match(ligne).ToString();
                if (strTemp != "")
                {
                    strTemp = strTemp.Replace("<a  href=\"", "");
                    strTemp = strTemp.Replace("</a>", "");
                    strTemp = strTemp.Replace("http://www.exploit-db.com/author/?a=", "");

                    myRegex = new Regex("[^<>]*\" title=");
                    string strTemp2 = string.Empty;
                    strTemp2 = myRegex.Match(strTemp).ToString();
                    strTemp2 = strTemp2.Replace("\" title=", "");
                    monStreamWriter.WriteLine(strTemp2.Trim());
                    Console.WriteLine("DEBUG ParseEXPLOITDB authorid=" + strTemp2);

                    myRegex = new Regex("title=\"[^<>]*\"");
                    strTemp = myRegex.Match(strTemp).ToString();
                    strTemp = strTemp.Replace("title=\"", "");
                    strTemp = strTemp.Replace("\"", "");
                    monStreamWriter.WriteLine(strTemp.Trim());
                    Console.WriteLine("DEBUG ParseEXPLOITDB author=" + strTemp);
                }

                ligne = monStreamReader.ReadLine();
            }
            monStreamReader.Close();
            monStreamWriter.Close();
            Console.WriteLine("DEBUG FINISHED MASTER");

        }

        static private REFERENCE REFERENCENormalize(XORCISMEntities model, REFERENCE oReference)
        {
            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
            Console.WriteLine("DEBUG in REFERENCENormalize()");
            string strTemp = string.Empty;
            Regex myRegex = null;

            //Cleaning ReferenceURL
            strTemp = oReference.ReferenceURL;

            strTemp = strTemp.Replace("<a href=\"", "");
            strTemp = strTemp.Replace("target=\"_blank\"", "");
            strTemp = strTemp.Replace("\"", "");
            strTemp = strTemp.Replace(">", "").Trim();
            strTemp = strTemp.Replace("http://www.", "http://");
            strTemp = strTemp.Replace("https://www.", "https://");
            if (strTemp.StartsWith("/show/osvdb/"))
            {
                strTemp = strTemp.Replace("/show/osvdb/", "http://osvdb.org/");
            }
            strTemp = strTemp.Replace("http://osvdb.org/show/osvdb/", "http://osvdb.org/");
            strTemp = strTemp.Replace("http://osvdb.org/displayvuln.php?osvdbid=", "http://osvdb.org/");
            strTemp = strTemp.Replace("securitytracker.com/id?", "securitytracker.com/id/");
            strTemp = strTemp.Replace("exploit-db.com/download/", "exploit-db.com/exploits/");

            if (strTemp.StartsWith("www."))
            {
                strTemp = "http://" + strTemp;
            }
            if (strTemp.StartsWith("/"))
            {
                Console.WriteLine("ERROR: BAD REFERENCEURL " + strTemp);
            }

            string sNormalizedReferenceURL = strTemp;
            oReference.ReferenceURL = sNormalizedReferenceURL;
            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
            Console.WriteLine("DEBUG Normalized ReferenceURL=" + sNormalizedReferenceURL);

            //http://scaprepo.com/view.jsp?id=oval:org.secpod.oval:def:701638
            myRegex = new Regex("http://scaprepo.com/view.jsp?id=[^<>]*");
            strTemp = myRegex.Match(sNormalizedReferenceURL).ToString();
            if (strTemp != "")
            {
                oReference.Source = "SCAPREPO";
                string sSourceID = sNormalizedReferenceURL.Replace("http://scaprepo.com/view.jsp?id=", "");
                sSourceID = sSourceID.Replace("/", ""); //at the end
                oReference.ReferenceSourceID = sSourceID;
            }
            else
            {
                #region ovaldef
                //oval:org.secpod.oval:def:701638
                myRegex = new Regex("oval:[^<>]*:def:[0-9]*");  //TODO REVIEW
                strTemp = myRegex.Match(sNormalizedReferenceURL).ToString();
                if (strTemp != "")
                {
                    Console.WriteLine("DEBUG OVALDEF Found " + strTemp);
                    int iOVALDefinitionID = 0;
                    try
                    {
                        iOVALDefinitionID = oval_model.OVALDEFINITION.FirstOrDefault(o => o.OVALDefinitionIDPattern == strTemp).OVALDefinitionID;
                    }
                    catch (Exception ex)
                    {

                    }

                    if (iOVALDefinitionID <= 0)
                    {
                        try
                        {

                            OVALDEFINITION oOVALDefinition = new OVALDEFINITION();
                            oOVALDefinition.CreatedDate = DateTimeOffset.Now;
                            oOVALDefinition.OVALDefinitionIDPattern = strTemp;
                            //We don't have more information
                            oOVALDefinition.timestamp = DateTimeOffset.Now;
                            //oOVALDefinition.VocabularyID=   //TODO
                            oval_model.OVALDEFINITION.Add(oOVALDefinition);
                            oval_model.SaveChanges();

                            //TODO
                            //OVALDEFINITIONREFERENCE

                            //TODO
                            //OVALDEFINITIONVULNERABILITY

                        }
                        catch (Exception exoOVALDefinition)
                        {
                            Console.WriteLine("Exception: exoOVALDefinition " + exoOVALDefinition.Message + " " + exoOVALDefinition.InnerException);
                        }
                    }
                    else
                    {
                        //Update OVALDEFINITION
                    }
                }
                #endregion ovaldef
                else
                {
                    #region secunia
                    //http://secunia.com/advisories/54013/
                    myRegex = new Regex("http://secunia.com/advisories/[^<>]*");
                    strTemp = myRegex.Match(sNormalizedReferenceURL).ToString();
                    if (strTemp != "")
                    {
                        oReference.Source = "SECUNIA";
                        string sSourceID = sNormalizedReferenceURL.Replace("http://secunia.com/advisories/", "");
                        sSourceID = sSourceID.Replace("/", ""); //at the end
                        oReference.ReferenceSourceID = sSourceID;
                    }
                    #endregion secunia
                    else
                    {
                        #region osvdb
                        myRegex = new Regex("http://osvdb.org/[^<>]*");
                        strTemp = myRegex.Match(sNormalizedReferenceURL).ToString();
                        if (strTemp != "")
                        {
                            oReference.Source = "OSVDB";
                            string sSourceID = sNormalizedReferenceURL.Replace("http://osvdb.org/", "");
                            //TODO Review
                            if (sSourceID.StartsWith("ref"))
                            {
                                //http://osvdb.org/ref/92/winarchiver-overflow.txt
                            }
                            else
                            {
                                sSourceID = sSourceID.Replace("/", "");
                                //TODO i.e. test if integer (e.g. bad OSVDBID on exploit-db EDBID:31347 OSVDBID:2014-0038
                                try
                                {
                                    int iTest = int.Parse(sSourceID);
                                    oReference.ReferenceSourceID = sSourceID;
                                }
                                catch (Exception exBadOSVDBID)
                                {
                                    Console.WriteLine("Exception: exBadOSVDBID " + sSourceID + " " + exBadOSVDBID.Message + " " + exBadOSVDBID.InnerException);
                                }

                                if (bRequestOSVDB)
                                {
                                    //Check if we went to this URL recently to avoid too many requests
                                    DateTimeOffset DateLastRequest = DateTimeOffset.Now;
                                    try
                                    {
                                        DateLastRequest = (DateTimeOffset)oReference.LastCheckedDate;
                                        Console.WriteLine("DEBUG DateLastRequest=" + DateLastRequest.ToString());
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                    if ((DateTimeOffset.Now - DateLastRequest).Hours > 4) ; //TODO Hardcoded
                                    {
                                        fRequestOSVDB(sNormalizedReferenceURL, sSourceID, model, oReference);   //, oVULNERABILITYOSVDB);
                                    }
                                }
                            }
                        }
                        #endregion osvdb
                        else
                        {
                            #region exploitdb
                            myRegex = new Regex("http://exploit-db.com/exploits/[^<>]*");   //TODO Review int
                            strTemp = myRegex.Match(sNormalizedReferenceURL).ToString();
                            if (strTemp != "")
                            {
                                oReference.Source = "EXPLOIT-DB";
                                string sSourceID = sNormalizedReferenceURL.Replace("http://exploit-db.com/exploits/", "");
                                sSourceID = sSourceID.Replace("/", "");
                                oReference.ReferenceSourceID = sSourceID;
                                oReference.Type = "EXPLOIT";
                                //TODO Check if EXPLOIT exists
                                //bRequestEXPLOITDB
                                //LastChecked
                                //TODO search more info

                                //Regex RegexMS = new Regex("MS[0-9][0-9]-[0-9][0-9][1-9]");

                            }
                            #endregion exploitdb
                            else
                            {
                                #region cve
                                //http://cve.mitre.org/cgi-bin/cvename.cgi?name=2013-1710
                                //http://cve.mitre.org/cgi-bin/cvename.cgi?name=2013-1806
                                myRegex = new Regex(@"http://cve.mitre.org/cgi-bin/cvename.cgi\?name=[^<>]*");
                                strTemp = myRegex.Match(sNormalizedReferenceURL).ToString();
                                if (strTemp != "")
                                {
                                    oReference.Source = "MITRE";
                                    string sSourceID = sNormalizedReferenceURL.Replace("http://cve.mitre.org/cgi-bin/cvename.cgi?name=", "CVE-");
                                    sSourceID = sSourceID.Replace("/", "");
                                    oReference.ReferenceSourceID = sSourceID;
                                    Console.WriteLine("DEBUG CVE identified " + sSourceID);
                                    //TODO Check/Add to VULNERABILITY

                                }
                                #endregion cve
                                else
                                {
                                    #region securityfocus
                                    //http://securityfocus.com/bid/57756
                                    myRegex = new Regex("http://securityfocus.com/bid/[^<>]*");
                                    strTemp = myRegex.Match(sNormalizedReferenceURL).ToString();
                                    if (strTemp != "")
                                    {
                                        oReference.Source = "BID";
                                        string sSourceID = sNormalizedReferenceURL.Replace("http://securityfocus.com/bid/", "");
                                        sSourceID = sSourceID.Replace("/", "");
                                        oReference.ReferenceSourceID = sSourceID;
                                        //TODO search for more info

                                    }
                                    #endregion securityfocus
                                    else
                                    {
                                        #region securitytracker
                                        //http://securitytracker.com/id/1028074
                                        myRegex = new Regex("http://securitytracker.com/id/[^<>]*");
                                        strTemp = myRegex.Match(sNormalizedReferenceURL).ToString();
                                        if (strTemp != "")
                                        {
                                            oReference.Source = "SECTRACK";
                                            string sSourceID = sNormalizedReferenceURL.Replace("http://securitytracker.com/id/", "");
                                            sSourceID = sSourceID.Replace("/", "");
                                            sSourceID = sSourceID.Replace("?", "");
                                            oReference.ReferenceSourceID = sSourceID;
                                        }
                                        #endregion securitytracker
                                        else
                                        {
                                            #region scip
                                            //http://scip.ch/en/?vuldb.13134
                                            myRegex = new Regex("http://scip.ch/en/?vuldb.[^<>]*");
                                            strTemp = myRegex.Match(sNormalizedReferenceURL).ToString();
                                            if (strTemp != "")
                                            {
                                                oReference.Source = "SCIP";
                                                string sSourceID = sNormalizedReferenceURL.Replace("http://scip.ch/en/?vuldb.", "");
                                                sSourceID = sSourceID.Replace("/", "");
                                                oReference.ReferenceSourceID = sSourceID;
                                            }
                                            #endregion scip
                                            else
                                            {
                                                #region xforce
                                                //http://xforce.iss.net/xforce/xfdb/71494
                                                myRegex = new Regex("http://xforce.iss.net/xforce/xfdb/[^<>]*");
                                                strTemp = myRegex.Match(sNormalizedReferenceURL).ToString();
                                                if (strTemp != "")
                                                {
                                                    oReference.Source = "XF";
                                                    string sSourceID = sNormalizedReferenceURL.Replace("http://xforce.iss.net/xforce/xfdb/", "");
                                                    sSourceID = sSourceID.Replace("/", "");
                                                    oReference.ReferenceSourceID = sSourceID;
                                                }
                                                #endregion xforce
                                                else
                                                {
                                                    #region uscert
                                                    //http://us-cert.gov/cas/techalerts/TA13-008A.html
                                                    //http://us-cert.gov/cas/techalerts/TA12-010A.html
                                                    ////http://cert.org/advisories/TA13-010A.html   //TODO
                                                    ////http://ics-cert.us-cert.gov/advisories/ICSA-14-058-01 //TODO
                                                    myRegex = new Regex("http://us-cert.gov/cas/techalerts/[^<>]*");    //TODO REVIEW
                                                    strTemp = myRegex.Match(sNormalizedReferenceURL).ToString();
                                                    if (strTemp != "")
                                                    {
                                                        oReference.Source = "US-CERT";
                                                        string sSourceID = sNormalizedReferenceURL.Replace("http://us-cert.gov/cas/techalerts/", "");
                                                        sSourceID = sSourceID.Replace(".html", "");
                                                        sSourceID = sSourceID.Replace("/", "");
                                                        oReference.ReferenceSourceID = sSourceID;   //TA12-010A
                                                    }
                                                    #endregion uscert
                                                    else
                                                    {
                                                        //http://technet.microsoft.com/security/bulletin/MS14-001
                                                        //https://technet.microsoft.com/en-us/security/bulletin/MS13-071
                                                        //https://technet.microsoft.com/en-us/library/security/ms04-011.aspx
                                                        //http://www.microsoft.com/technet/security/Bulletin/MS11-056.mspx
                                                        //http://technet.microsoft.com/security/bulletin/MS11-091
                                                        if(sNormalizedReferenceURL.ToLower().Contains("microsoft.com"))
                                                        {
                                                            oReference.Source = "MS";

                                                            Regex RegexMS = new Regex("MS[0-9][0-9]-[0-9][0-9][1-9]", RegexOptions.IgnoreCase);
                                                            strTemp = RegexMS.Match(sNormalizedReferenceURL).ToString();
                                                            if (strTemp != "")
                                                            {
                                                                strTemp = strTemp.ToUpper();
                                                                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                                Console.WriteLine("DEBUG MSpatch=" + strTemp);
                                                                oReference.ReferenceTitle = strTemp;
                                                                oReference.ReferenceSourceID = strTemp;
                                                                //TODO PATCH?
                                                            }
                                                        }
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
                            }
                        }
                    }
                }
            }

            //TODO
            //http://support.apple.com/kb/HT5282
            //http://adobe.com/support/security/bulletins/apsb13-14.html
            //http://1337day.com/exploits/20112
            //http://zerodayinitiative.com/advisories/ZDI-13-087/
            //https://github.com/rapid7/metasploit-framework/blob/master/modules/exploits/windows/fileformat/apple_quicktime_rdrf.rb
            //http://metasploit.com/modules/exploit/multi/browser/java_jre17_jmxbean
            //http://coresecurity.com/advisories/mac-osx-server-directoryservice-buffer-overflow
            
            //http://support.microsoft.com/default.aspx?scid=kb;EN-US;2864063
            //http://ibm.com/support/docview.wss?uid=swg24034507
            //http://cert.org/advisories/TA13-010A.html
            
            //http://rhn.redhat.com/errata/RHSA-2013-0626.html
            //https://bugzilla.redhat.com/show_bug.cgi?id=894934
            //http://pastebin.com/raw.php?i=cUG2ayjh
            //http://jvn.jp/cert/JVNTA13-010A/index.html
            //http://jvndb.jvn.jp/jvndb/JVNDB-2014-000001
            //https://trustwave.com/spiderlabs/advisories/TWSL2012-023.txt
            //http://vmware.com/security/advisories/VMSA-2013-0009.html
            //https://hkcert.org/my_url/en/alert/14031209
            //http://mandriva.com/security/advisories?name=MDVSA-2014:079
            //http://madirish.net/554
            //http://kb.cert.org/vuls/id/489228
            //https://kb.isc.org/article/AA-01085
            //http://kb.juniper.net/InfoCenter/index?page=content&id=JSA10608
            //http://ics-cert.us-cert.gov/advisories/ICSA-14-058-01
            //http://www-01.ibm.com/support/docview.wss?uid=swg21663023
            //https://htbridge.com/advisory/HTB23194
            //http://ec-cube.net/info/weakness/weakness.php?id=56
            //http://labs.integrity.pt/advisories/cve-2013-3319/
            //http://scn.sap.com/docs/DOC-8218
            oReference.timestamp = DateTimeOffset.Now;
            return oReference;
        }

        static private void fRequestOSVDB(string sReferenceURL, string sOSVDBID, XORCISMEntities model, REFERENCE oReference=null,  VULNERABILITY oVULNERABILITYOSVDB=null)
        {
            //************************************************************************
            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
            Console.WriteLine("DEBUG Searching more info on OSVDB " + sReferenceURL);
            
            //Check if we went to this URL recently to avoid too many requests
            DateTimeOffset DateLastRequest = DateTimeOffset.Now;
            try
            {
                DateLastRequest = (DateTimeOffset)oReference.LastCheckedDate;
                Console.WriteLine("DEBUG DateLastRequest=" + DateLastRequest.ToString());
            }
            catch (Exception ex)
            {

            }

            if ((DateTimeOffset.Now - DateLastRequest).Hours < 4)   //TODO Hardcoded
            {
                Console.WriteLine("DEBUG Request done recently. Aborting");
                return;
            }
            else
            {
                Console.WriteLine("DEBUG Info: Request done "+(DateTimeOffset.Now - DateLastRequest).Hours.ToString()+" hours ago");
            }

            string ResponseText = string.Empty;
            Regex myRegex = null;
            string strTemp = string.Empty;

            try
            {
                string sCurrentPath = Directory.GetCurrentDirectory();

                if (bRequestOSVDB)
                {
                    #region osvdbrequest
                    /*
                    request = (HttpWebRequest)HttpWebRequest.Create(sReferenceURL);
                    request.Method = "GET";
                    request.AllowAutoRedirect = true;
                    request.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.0; en-US; rv:1.4) Gecko/20030624 Netscape/7.1 (ax)";

                    try
                    {
                        response = (HttpWebResponse)request.GetResponse();
                        SR = new StreamReader(response.GetResponseStream());
                        ResponseText = SR.ReadToEnd();
                        SR.Close();
                        response.Close();
                    }
                    catch (WebException ex)
                    {
                        using (var sr = new StreamReader(ex.Response.GetResponseStream()))
                            ResponseText = sr.ReadToEnd();
                    */

                    driver.Navigate().GoToUrl(sReferenceURL);
                    ResponseText = driver.PageSource;

                    if (ResponseText.Contains("cloudflare") && ResponseText.Contains("5 seconds"))
                    {
                        Console.WriteLine("DEBUG cloudflare wait for 5 seconds detected");
                        ////IWebDriver driver = new InternetExplorerDriver(); //can't get the cookie values
                        //OpenQA.Selenium.Chrome.ChromeDriver driver = new OpenQA.Selenium.Chrome.ChromeDriver();

                        driver.Navigate().GoToUrl(sReferenceURL);

                        Thread.Sleep(10000);    //TODO Review

                        ResponseText = driver.PageSource;
                        //Console.WriteLine(driver.PageSource);

                        //TODO get the cookie cf_clearance for future requests
                        // And now output all the available cookies for the current URL
                        //Domain: .osvdb.org
                        try
                        {
                            /*
                            foreach(OpenQA.Selenium.Cookie OSVDBCookie in driver.Manage().Cookies.AllCookies)
                            {
                                Console.WriteLine("DEBUG OSVDBCookie.Name="+OSVDBCookie.Name);
                                Console.WriteLine("DEBUG OSVDBCookie.Value=" + OSVDBCookie.Value);
                                Console.WriteLine("DEBUG OSVDBCookie.ToString=" + OSVDBCookie.ToString());
                            }
                            */

                            //Should work, just need to be global
                            /*
                            OpenQA.Selenium.Cookie OSVDBCookiecf_clearance = driver.Manage().Cookies.GetCookieNamed("cf_clearance");
                            string OSVDBCookiecf_clearanceValue = OSVDBCookiecf_clearance.Value;

                            OpenQA.Selenium.Cookie OSVDBCookie__cfduid = driver.Manage().Cookies.GetCookieNamed("__cfduid");
                            string OSVDBCookie__cfduidValue = OSVDBCookiecf_clearance.Value;

                            OpenQA.Selenium.Cookie OSVDBCookie_session_id = driver.Manage().Cookies.GetCookieNamed("cf_clearance");
                            string OSVDBCookie_session_idValue = OSVDBCookie_session_id.Value;
                            */
                        }
                        catch (Exception exSeleniumCookie)
                        {
                            Console.WriteLine("Exception: exSeleniumCookie " + exSeleniumCookie.Message + " " + exSeleniumCookie.InnerException);
                        }
                        //__cfduid
                        //_session_id

                        /*
                        try
                        {
                            driver.Quit();
                        }
                        catch(Exception exDriverQuit)
                        {
                            Console.WriteLine("Exception: exDriverQuit " + exDriverQuit.Message + " " + exDriverQuit.InnerException);
                        }
                        */
                    }
                    //}


                    
                    System.IO.File.WriteAllText(sCurrentPath + @"\osvdb\" + sOSVDBID + ".txt", ResponseText);
                    #endregion osvdbrequest
                }


                //NOTE: Code from Import_OSVDB

                #region parseosvdb
                Console.WriteLine("DEBUG PARSING OSVDB FILE");
                string sSourceFilePath = sCurrentPath + @"\osvdb\" + sOSVDBID + ".txt";
                StreamReader StreamReaderOSVDB = new StreamReader(sSourceFilePath);

                try
                {
                    if (oReference != null)
                    {
                        //Update REFERENCE
                        oReference.ReferenceFilePath = sCurrentPath + @"\osvdb\" + sOSVDBID + ".txt";
                        //oReference.Type=    //? TODO
                        //oReference.lang = "US"; //? TODO
                        //oReference.LocaleID=    ;   //TODO
                        oReference.LastCheckedDate = DateTimeOffset.Now;
                        oReference.timestamp = DateTimeOffset.Now;
                        model.SaveChanges();
                    }

                    if (oVULNERABILITYOSVDB != null)
                    {
                        //oVULNERABILITYOSVDB.LastCheckDate
                        oVULNERABILITYOSVDB.timestamp = DateTimeOffset.Now;
                        model.SaveChanges();
                    }
                }
                catch (Exception exParseOSVDB01)
                {
                    Console.WriteLine("Exception: exParseOSVDB01 " + exParseOSVDB01.Message + " " + exParseOSVDB01.InnerException);
                }


                string lineOSVDB = StreamReaderOSVDB.ReadLine();
                //Console.WriteLine(ligne);
                string sInfoToSearch = "";
                string sOSVDBDescription = "";  //Could be multi lines
                string sOSVDBSolution = "";  //Could be multi lines
                //Regex myRegex = null;
                //string strTemp = string.Empty;
                while (lineOSVDB != null)
                {
                    //Technique 1: line by line
                    ResponseText = lineOSVDB;
                    //Console.WriteLine(lineOSVDB);

                    if (lineOSVDB.Contains("<title>"))
                    {
                        sInfoToSearch = "title";
                        Console.WriteLine("DEBUG OSVDB Parsing " + sInfoToSearch);
                    }

                    //***********************************************************************************
                    #region switchparseosvdb
                    switch (sInfoToSearch)
                    {
                        case "products":
                            //TODO
                            //<td 
                            //href="/vendor/

                            //TODO: retrieve CPEs
                            //VULNERABILITYFORCPE
                            //oVULNERABILITYOSVDB
                            sInfoToSearch = "";
                            break;
                        case "credit":
                            //TODO

                            sInfoToSearch = "";
                            break;
                        case "cvss":
                            //TODO
                            //oVULNERABILITYOSVDB.CVSSBaseScore
                            sInfoToSearch = "";
                            break;
                        case "references":
                            if (lineOSVDB != "</li>")
                            {
                                //TODO:
                                //<li>Exploit Database:
                                //<li>CVE ID:
                                //Related OSVDB ID:
                                //Metasploit URL:
                                #region osvdbcve
                                myRegex = new Regex("CVE-[0-9][0-9][0-9][0-9]-[0-9][0-9][0-9][0-9]"); //TODO review
                                strTemp = myRegex.Match(lineOSVDB).ToString();
                                if (strTemp == "")
                                {
                                    //We do another test to find a CVE
                                    myRegex = new Regex(@"cvename.cgi\?name=[0-9][0-9][0-9][0-9]-[0-9][0-9][0-9][0-9]"); //TODO review
                                    strTemp = myRegex.Match(lineOSVDB).ToString();
                                    if (strTemp != "")
                                    {
                                        strTemp = "CVE-" + strTemp.Replace("cvename.cgi?name=", "");
                                        Console.WriteLine("DEBUG CVE FOUND " + strTemp);
                                    }
                                }
                                if (strTemp != "")
                                {
                                    //We maybe have found a relationship between a CVE and OSVDB via exploit-db
                                    string sCVEID = strTemp;
                                    //First check if this CVE is known
                                    //TODO: cve hardcoded => VULRefentialID
                                    VULNERABILITY oCVEOSVDB = vuln_model.VULNERABILITY.FirstOrDefault(o => o.VULReferential == "cve" && o.VULReferentialID == sCVEID);
                                    if (oCVEOSVDB == null)
                                    {
                                        oCVEOSVDB = new VULNERABILITY();
                                        oCVEOSVDB.VULReferential = "cve";
                                        oCVEOSVDB.VULReferentialID = sCVEID;
                                        oCVEOSVDB.CreatedDate = DateTimeOffset.Now;
                                        oCVEOSVDB.timestamp = DateTimeOffset.Now;
                                        oCVEOSVDB.VocabularyID = iVocabularyOSVDBID;

                                        try
                                        {
                                            vuln_model.VULNERABILITY.Add(oCVEOSVDB);
                                            vuln_model.SaveChanges();
                                            Console.WriteLine("DEBUG OSVDB AddToVULNERABILITY " + sCVEID);
                                        }
                                        catch (Exception exoCVEOSVDB)
                                        {
                                            Console.WriteLine("Exception: exoCVEOSVDB " + exoCVEOSVDB.Message + " " + exoCVEOSVDB.InnerException);
                                        }

                                    }

                                    VULNERABILITYFORREFERENCE oOSVDBFORCVE = vuln_model.VULNERABILITYFORREFERENCE.Where(o => o.VulnerabilityID == oCVEOSVDB.VulnerabilityID && o.ReferenceID == oReference.ReferenceID).FirstOrDefault();
                                    if (oOSVDBFORCVE != null)
                                    {
                                        //Update VULNERABILITYFORREFERENCE
                                    }
                                    else
                                    {

                                        try
                                        {
                                            //First save the (new) oReference to get a ReferenceID
                                            model.SaveChanges();

                                            oOSVDBFORCVE = new VULNERABILITYFORREFERENCE();
                                            oOSVDBFORCVE.VulnerabilityID = oCVEOSVDB.VulnerabilityID;
                                            oOSVDBFORCVE.ReferenceID = oReference.ReferenceID;
                                            oOSVDBFORCVE.VocabularyID = iVocabularyOSVDBID;

                                            oOSVDBFORCVE.CreatedDate = DateTimeOffset.Now;
                                            oOSVDBFORCVE.timestamp = DateTimeOffset.Now;

                                            vuln_model.VULNERABILITYFORREFERENCE.Add(oOSVDBFORCVE);
                                            vuln_model.SaveChanges();
                                        }
                                        catch (Exception exoOSVDBFORCVE)
                                        {
                                            Console.WriteLine("Exception: exoOSVDBFORCVE " + exoOSVDBFORCVE.Message + " " + exoOSVDBFORCVE.InnerException);
                                        }
                                    }

                                    //TODO: the same for oVULNERABILITYOSVDB
                                }
                                #endregion osvdbcve

                                //<li>Secunia Advisory ID:
                                //<li>Bugtraq ID:
                                //<li>Other Advisory URL:
                                //<li>Mail List Post:
                                //<li>Generic Exploit URL:
                                //<li>Vendor URL:
                                //<li>Vendor Specific News/Changelog Entry:

                                myRegex = new Regex(@"<a.*?href=[""'](.*?)[""'].*?>", RegexOptions.Singleline);
                                strTemp = myRegex.Match(ResponseText).ToString();
                                if (strTemp != "")
                                {
                                    //Cleaning ReferenceURL
                                    strTemp = strTemp.Replace("<a href=\"", "");
                                    strTemp = strTemp.Replace("target=\"_blank\"", "");
                                    strTemp = strTemp.Replace("\"", "");
                                    strTemp = strTemp.Replace(">", "").Trim();
                                    if (strTemp.StartsWith("/show/osvdb/"))
                                    {
                                        strTemp = strTemp.Replace("/show/osvdb/", "http://osvdb.org/");
                                    }
                                    strTemp = strTemp.Replace("displayvuln.php?osvdbid=", "").Trim();
                                    Console.WriteLine("DEBUG Reference URL: " + strTemp);
                                    //TODO
                                    //MS13-022
                                    //PATCH
                                    //TODO
                                    //http://cve.mitre.org/cgi-bin/cvename.cgi?name=2013-5948
                                    //CVE

                                    //TODO: REFERENCEMAPPING (+VULNERABILITYREFERENCE +EXPLOITFORREFERENCE)
                                    string sReferenceSource = ReferenceSource(strTemp);
                                    //Cleaning ReferenceURL
                                    sReferenceURL = strTemp;
                                    sReferenceURL = sReferenceURL.Replace("http://www.", "http://");    //TODO: Review, e.g. www.exploit-db.com
                                    sReferenceURL = sReferenceURL.Replace("https://www.", "https://");
                                    sReferenceURL = sReferenceURL.Replace("http://osvdb.org/show/osvdb/", "http://osvdb.org/");   //Related OSVDB ID:
                                    sReferenceURL = sReferenceURL.Replace("http://osvdb.org/displayvuln.php?osvdbid=", "http://osvdb.org/");
                                    Console.WriteLine("DEBUG oReferenceFromOSVDB");
                                    REFERENCE oReferenceFromOSVDB = null;
                                    if (sReferenceURL.ToLower().Contains("microsoft.com"))
                                    {
                                        //Do this because so many different URLs for the same MS bulletin
                                        //http://technet.microsoft.com/en-us/security/bulletin/MS11-074
                                        //https://technet.microsoft.com/en-us/library/security/ms11-074.aspx
                                        //http://www.microsoft.com/technet/security/Bulletin/MS11-074.mspx
                                        //...
                                        Regex RegexMS = new Regex("MS[0-9][0-9]-[0-9][0-9][1-9]", RegexOptions.IgnoreCase);
                                        strTemp = RegexMS.Match(sReferenceURL).ToString();
                                        if (strTemp != "")
                                        {
                                            strTemp = strTemp.ToUpper();
                                            oReferenceFromOSVDB = model.REFERENCE.Where(o => o.ReferenceTitle == strTemp).FirstOrDefault();
                                        }
                                        else
                                        {
                                            //TODO?
                                            //http://www.microsoft.com/technet/support/kb.asp?ID=249599
                                            //http://www.microsoft.com/technet/security/advisory/903144.mspx
                                            //http://technet.microsoft.com/security/advisory/2719615
                                            //http://technet.microsoft.com/en-us/security/cc405107.aspx#EHD
                                            ////http://blogs.technet.com/srd/archive/2009/08/11/ms09-037-why-we-are-using-cve-s-already-used-in-ms09-035.aspx
                                            //http://blogs.technet.com/b/mmpc/archive/2011/04/12/analysis-of-the-cve-2011-0611-adobe-flash-player-vulnerability-exploitation.aspx

                                        }
                                    }
                                    else
                                    {
                                        oReferenceFromOSVDB=model.REFERENCE.Where(o => o.ReferenceURL == sReferenceURL).FirstOrDefault();
                                    }
                                    if (oReferenceFromOSVDB != null)
                                    {
                                        //Update REFERENCE
                                        //TODO Remove?
                                        oReferenceFromOSVDB = REFERENCENormalize(model, oReferenceFromOSVDB);

                                    }
                                    else
                                    {
                                        oReferenceFromOSVDB = new REFERENCE();
                                        oReferenceFromOSVDB.ReferenceURL = sReferenceURL;
                                        oReferenceFromOSVDB.Source = sReferenceSource;
                                        oReferenceFromOSVDB.CreatedDate = DateTimeOffset.Now;
                                        oReferenceFromOSVDB.timestamp = DateTimeOffset.Now;
                                        oReferenceFromOSVDB.VocabularyID = iVocabularyOSVDBID;
                                        try
                                        {
                                            model.REFERENCE.Add(REFERENCENormalize(model, oReferenceFromOSVDB));
                                            //model.SaveChanges();
                                        }
                                        catch (Exception exoReferenceFromOSVDB)
                                        {
                                            Console.WriteLine("Exception: exoReferenceFromOSVDB " + exoReferenceFromOSVDB.Message + " " + exoReferenceFromOSVDB.InnerException);
                                        }
                                    }
                                    try
                                    {
                                        model.SaveChanges();
                                    }
                                    catch (Exception exoReferenceFromOSVDB02)
                                    {
                                        Console.WriteLine("Exception: exoReferenceFromOSVDB02 " + exoReferenceFromOSVDB02.Message + " " + exoReferenceFromOSVDB02.InnerException);
                                    }

                                    //OSVDBREFERENCE
                                    Console.WriteLine("DEBUG oReferenceMapping");
                                    REFERENCEMAPPING oReferenceMapping = model.REFERENCEMAPPING.Where(o => o.ReferenceRefID == oReference.ReferenceID && o.ReferenceSubjectID == oReferenceFromOSVDB.ReferenceID).FirstOrDefault();
                                    if (oReferenceMapping != null)
                                    {
                                        //Update REFERENCEMAPPING
                                    }
                                    else
                                    {
                                        oReferenceMapping = model.REFERENCEMAPPING.Where(o => o.ReferenceSubjectID == oReference.ReferenceID && o.ReferenceRefID == oReferenceFromOSVDB.ReferenceID).FirstOrDefault();
                                        if (oReferenceMapping != null)
                                        {
                                            //Update REFERENCEMAPPING
                                        }
                                        else
                                        {

                                            try
                                            {
                                                //First save the (new) oReference to get a ReferenceID
                                                model.SaveChanges();

                                                oReferenceMapping = new REFERENCEMAPPING();
                                                oReferenceMapping.ReferenceRefID = oReference.ReferenceID;    //OSVDB ID
                                                oReferenceMapping.ReferenceSubjectID = oReferenceFromOSVDB.ReferenceID;
                                                oReferenceMapping.RelationShipText = "OSVDB";
                                                oReferenceMapping.VocabularyID = iVocabularyOSVDBID;
                                                //oReferenceMapping.ReferenceMappingDescription   //TODO
                                                oReferenceMapping.CreatedDate = DateTimeOffset.Now;
                                                oReferenceMapping.timestamp = DateTimeOffset.Now;

                                                model.REFERENCEMAPPING.Add(oReferenceMapping);
                                                model.SaveChanges();
                                            }
                                            catch (Exception exoReferenceMapping)
                                            {
                                                Console.WriteLine("Exception: exoReferenceMapping " + exoReferenceMapping.Message + " " + exoReferenceMapping.InnerException);
                                            }
                                        }
                                    }

                                    //TODO for oVULNERABILITYOSVDB

                                }
                            }
                            if (lineOSVDB.Contains("</tbody>"))
                            {
                                sInfoToSearch = "";
                            }
                            break;

                        case "location":
                            if (lineOSVDB != "<br>")
                            {
                                strTemp = lineOSVDB.Trim();
                                Console.WriteLine("DEBUG OSVDB Location=" + strTemp);
                            }
                            else
                            {
                                sInfoToSearch = "";
                            }
                            break;

                        case "attacktype":
                            if (lineOSVDB != "<br>")
                            {
                                strTemp = lineOSVDB.Trim();
                                if (strTemp.EndsWith(","))
                                {
                                    strTemp = strTemp.Replace(",", "");
                                }
                                Console.WriteLine("DEBUG OSVDB Attack Type=" + strTemp);
                                //Race Condition
                                //Input Manipulation
                                //TODO: map with CWE
                                //VULNERABILITYFORCWE
                                //oVULNERABILITYOSVDB
                            }
                            else
                            {
                                sInfoToSearch = "";
                            }
                            break;

                        case "impact":
                            if (lineOSVDB != "<br>")
                            {
                                strTemp = lineOSVDB.Trim();
                                if (strTemp.EndsWith(","))
                                {
                                    strTemp = strTemp.Replace(",", "");
                                }
                                Console.WriteLine("DEBUG OSVDB Impact=" + strTemp);
                                //TODO: mapping (e.g. VERIS?)
                                //VULNERABILITY-IMPACT
                                //EXPLOIT-IMPACT
                                if (oVULNERABILITYOSVDB != null)
                                {
                                    try
                                    {
                                        oVULNERABILITYOSVDB.VULConsequence = strTemp;
                                        oVULNERABILITYOSVDB.timestamp = DateTimeOffset.Now;
                                        model.SaveChanges();
                                    }
                                    catch (Exception exImpactoVULNERABILITYOSVDB)
                                    {
                                        Console.WriteLine("Exception: exImpactoVULNERABILITYOSVDB " + exImpactoVULNERABILITYOSVDB.Message + " " + exImpactoVULNERABILITYOSVDB.InnerException);
                                    }
                                }
                            }
                            else
                            {
                                sInfoToSearch = "";
                            }
                            break;

                        case "solution":
                            if (lineOSVDB != "<br>")
                            {
                                strTemp = lineOSVDB.Trim();
                                if (strTemp.EndsWith(","))
                                {
                                    strTemp = strTemp.Replace(",", "");
                                }
                                Console.WriteLine("DEBUG OSVDB Solution=" + strTemp);
                                if (oVULNERABILITYOSVDB != null)
                                {
                                    if (strTemp.Contains("Workaround"))
                                    {
                                        oVULNERABILITYOSVDB.VULRemediationAvailable = true;
                                        oVULNERABILITYOSVDB.timestamp = DateTimeOffset.Now;
                                        model.SaveChanges();
                                    }
                                    if (strTemp.Contains("Upgrade"))
                                    {
                                        oVULNERABILITYOSVDB.VULPatchAvailable = true;
                                        oVULNERABILITYOSVDB.timestamp = DateTimeOffset.Now;
                                        model.SaveChanges();
                                    }
                                }
                                //TODO VULNERABILITYRECOMMENDATION
                                //oVULNERABILITYOSVDB

                            }
                            else
                            {
                                sInfoToSearch = "";
                            }
                            break;

                        case "exploit":
                            if (lineOSVDB != "<br>")
                            {
                                strTemp = lineOSVDB.Trim();
                                if (strTemp.EndsWith(","))
                                {
                                    strTemp = strTemp.Replace(",", "");
                                }
                                Console.WriteLine("DEBUG OSVDB Exploit=" + strTemp);
                            }
                            else
                            {
                                sInfoToSearch = "";
                            }
                            break;

                        case "disclosure":
                            if (lineOSVDB != "<br>")
                            {
                                strTemp = lineOSVDB.Trim();
                                if (strTemp.EndsWith(","))
                                {
                                    strTemp = strTemp.Replace(",", "");
                                }
                                Console.WriteLine("DEBUG OSVDB Disclosure=" + strTemp);
                            }
                            else
                            {
                                sInfoToSearch = "";
                            }
                            break;

                        case "osvdb":
                            if (lineOSVDB != "<br>")
                            {
                                strTemp = lineOSVDB.Trim();
                                if (strTemp.EndsWith(","))
                                {
                                    strTemp = strTemp.Replace(",", "");
                                }
                                Console.WriteLine("DEBUG OSVDB=" + strTemp);
                            }
                            else
                            {
                                sInfoToSearch = "";
                            }
                            break;

                        case "description":
                            myRegex = new Regex("<p>[^<>]*</p>");
                            strTemp = myRegex.Match(ResponseText).ToString();
                            if (strTemp != "")
                            {
                                strTemp = strTemp.Replace("<p>", "");
                                strTemp = strTemp.Replace("</p>", "");
                                if (sOSVDBDescription == "")
                                {
                                    sOSVDBDescription = strTemp;
                                }
                                else
                                {
                                    sOSVDBDescription += " " + strTemp;
                                }
                                //Console.WriteLine("DEBUG OSVDB Description=" + strTemp);
                                //sInfoToSearch = "";
                            }
                            if (lineOSVDB.Contains("</tbody>"))
                            {
                                sInfoToSearch = "";
                                oReference.ReferenceDescription = sOSVDBDescription;
                                //TODO OSVDBERROR
                                //&lt;em style='font-weight:bold;'&gt;(Description Provided by &lt;a href="http://cve.mitre.org/cgi-bin/cvename.cgi?name=2011-3152" target="_blank"&gt;CVE&lt;/a&gt;)&lt;/em&gt; : DistUpgrade/DistUpgradeFetcherCore.py in Update Manager before 1:0.87.31.1, 1:0.134.x before 1:0.134.11.1, 1:0.142.x before 1:0.142.23.1, 1:0.150.x before 1:0.150.5.1, and 1:0.152.x before 1:0.152.25.5 on Ubuntu 8.04 through 11.10 does not verify the GPG signature before extracting an upgrade tarball, which allows man-in-the-middle attackers to (1) create or overwrite arbitrary files via a directory traversal attack using a crafted tar file, or (2) bypass authentication via a crafted meta-release file.


                                oReference.timestamp = DateTimeOffset.Now;
                                if (oVULNERABILITYOSVDB != null)
                                {
                                    try
                                    {
                                        oVULNERABILITYOSVDB.VULDescription = sOSVDBDescription;
                                        oVULNERABILITYOSVDB.timestamp = DateTimeOffset.Now;
                                    }
                                    catch (Exception exDescriptionoVULNERABILITYOSVDB)
                                    {
                                        Console.WriteLine("Exception: exDescriptionoVULNERABILITYOSVDB " + exDescriptionoVULNERABILITYOSVDB.Message + " " + exDescriptionoVULNERABILITYOSVDB.InnerException);
                                    }
                                }
                                model.SaveChanges();
                                Console.WriteLine("DEBUG OSVDB Description=" + sOSVDBDescription);
                            }
                            break;

                        case "disclosure date":
                            myRegex = new Regex("[1-2][0-9][0-9][0-9]-[0-9][0-9]-[0-9][0-9]");
                            strTemp = myRegex.Match(ResponseText).ToString();
                            if (strTemp != "")
                            {
                                Console.WriteLine("DEBUG OSVDB Disclosure Date=" + strTemp);
                                oReference.Reference_Date = strTemp;
                                oReference.timestamp = DateTimeOffset.Now;
                                model.SaveChanges();

                                //TODO
                                //oVULNERABILITYOSVDB.VULPublishedDate

                                sInfoToSearch = "";
                            }
                            break;

                        case "solutiondescription":
                            myRegex = new Regex("<p>[^<>]*</p>");
                            strTemp = myRegex.Match(ResponseText).ToString();
                            if (strTemp != "")
                            {
                                strTemp = strTemp.Replace("<p>", "");
                                strTemp = strTemp.Replace("</p>", "");
                                if (sOSVDBSolution == "")
                                {
                                    sOSVDBSolution = strTemp;
                                }
                                else
                                {
                                    sOSVDBSolution += " " + strTemp;
                                }
                                //Console.WriteLine("DEBUG OSVDB Solution=" + strTemp);
                                //sInfoToSearch = "";
                            }
                            if (lineOSVDB.Contains("</tbody>"))
                            {
                                sInfoToSearch = "";
                                Console.WriteLine("DEBUG OSVDB Solution=" + sOSVDBSolution);
                                oReference.notes = sOSVDBSolution;
                                oReference.timestamp = DateTimeOffset.Now;
                                if (oVULNERABILITYOSVDB != null)
                                {
                                    oVULNERABILITYOSVDB.VULSolution = sOSVDBSolution;
                                    oVULNERABILITYOSVDB.timestamp = DateTimeOffset.Now;
                                }
                                model.SaveChanges();
                            }
                            break;

                        case "title":
                            //OSVDB Title
                            myRegex = new Regex("<title>[^<>]*</title>");
                            strTemp = myRegex.Match(ResponseText).ToString();
                            if (strTemp != "")
                            {
                                string sReferenceOSVDBTitle = strTemp;
                                sReferenceOSVDBTitle = sReferenceOSVDBTitle.Replace("<title>", "");
                                sReferenceOSVDBTitle = sReferenceOSVDBTitle.Replace("</title>", "");
                                sReferenceOSVDBTitle = sReferenceOSVDBTitle.Replace(sOSVDBID + ":", "").Trim();
                                Console.WriteLine("DEBUG OSVDB Title=" + sReferenceOSVDBTitle);

                                oReference.ReferenceTitle = sReferenceOSVDBTitle;
                                oReference.timestamp = DateTimeOffset.Now;
                                if (oVULNERABILITYOSVDB != null)
                                {
                                    oVULNERABILITYOSVDB.VULName = sReferenceOSVDBTitle;
                                    oVULNERABILITYOSVDB.timestamp = DateTimeOffset.Now;
                                }
                                model.SaveChanges();
                                sInfoToSearch = "";
                            }
                            break;
                        default:
                            break;
                    }
                    #endregion switchparseosvdb

                    #region parseosvdb
                    //Console.WriteLine("DEBUG parseosvdb");
                    //****************************************************************************
                    if (lineOSVDB.Contains(">Disclosure Date<"))
                    {
                        sInfoToSearch = "disclosure date";
                        Console.WriteLine("DEBUG OSVDB Parsing " + sInfoToSearch);
                    }
                    //Time to Patch
                    //Days of Exposure
                    if (lineOSVDB.Contains("Description</h1>"))
                    {
                        sInfoToSearch = "description";
                        Console.WriteLine("DEBUG OSVDB Parsing " + sInfoToSearch);
                    }
                    //Classification</h1>
                    if (lineOSVDB.Contains("<b>Location</b>"))
                    {
                        sInfoToSearch = "location";
                        Console.WriteLine("DEBUG OSVDB Parsing " + sInfoToSearch);
                    }
                    if (lineOSVDB.Contains("<b>Attack Type</b>"))
                    {
                        sInfoToSearch = "attacktype";
                        Console.WriteLine("DEBUG OSVDB Parsing " + sInfoToSearch);
                    }
                    if (lineOSVDB.Contains("<b>Impact</b>"))
                    {
                        sInfoToSearch = "impact";
                        Console.WriteLine("DEBUG OSVDB Parsing " + sInfoToSearch);
                    }
                    if (lineOSVDB.Contains("<b>Solution</b>"))
                    {
                        sInfoToSearch = "solution";
                        Console.WriteLine("DEBUG OSVDB Parsing " + sInfoToSearch);
                    }
                    if (lineOSVDB.Contains("<b>Exploit</b>"))
                    {
                        sInfoToSearch = "exploit";
                        Console.WriteLine("DEBUG OSVDB Parsing " + sInfoToSearch);
                    }
                    if (lineOSVDB.Contains("<b>Disclosure</b>"))
                    {
                        sInfoToSearch = "disclosure";
                        Console.WriteLine("DEBUG OSVDB Parsing " + sInfoToSearch);
                    }
                    if (lineOSVDB.Contains("<b>OSVDB</b>"))
                    {
                        sInfoToSearch = "osvdb";
                        Console.WriteLine("DEBUG OSVDB Parsing " + sInfoToSearch);
                    }
                    if (lineOSVDB.Contains("Solution</h1>"))
                    {
                        sInfoToSearch = "solutiondescription";
                        Console.WriteLine("DEBUG OSVDB Parsing " + sInfoToSearch);
                    }
                    if (lineOSVDB.Contains("Products</h1>"))
                    {
                        sInfoToSearch = "products";
                        Console.WriteLine("DEBUG OSVDB Parsing " + sInfoToSearch);
                    }
                    if (lineOSVDB.Contains("References</h1>"))
                    {
                        sInfoToSearch = "references";
                        Console.WriteLine("DEBUG OSVDB Parsing " + sInfoToSearch);
                    }
                    if (lineOSVDB.Contains("Credit</h1>"))
                    {
                        sInfoToSearch = "credit";
                        Console.WriteLine("DEBUG OSVDB Parsing " + sInfoToSearch);
                    }
                    if (lineOSVDB.Contains("Score</h1>"))   //CVSSv2 Score
                    {
                        sInfoToSearch = "score";
                        Console.WriteLine("DEBUG OSVDB Parsing " + sInfoToSearch);
                    }
                    if (lineOSVDB.Contains("Comments</h1>"))
                    {
                        sInfoToSearch = "comments";
                        Console.WriteLine("DEBUG OSVDB Parsing " + sInfoToSearch);
                    }
                    #endregion parseosvdb

                    lineOSVDB = StreamReaderOSVDB.ReadLine();
                }

                StreamReaderOSVDB.Close();
                Console.WriteLine("DEBUG OSVDB PARSED");
                #endregion parseosvdb

            }
            catch (Exception exSearchInfoOnOSVDB)
            {
                Console.WriteLine("Exception: exSearchInfoOnOSVDB " + exSearchInfoOnOSVDB.Message + " " + exSearchInfoOnOSVDB.InnerException);
            }


        }
    
        static private void InsertEXPLOITDB()
        {
            Console.WriteLine("DEBUG InsertEXPLOITDB()");
            //StreamReader 
            //StreamReader monStreamReader = new StreamReader("CVE-EXPLOITS.txt");
            StreamReader monStreamReader = new StreamReader("CVE-EXPLOITS.txt");
            string ligne = monStreamReader.ReadLine();
            string curCVE = string.Empty;
            int vulnID = 0;
            int cpt = 0;
            string strTemp = string.Empty;
            string sploitrefid = string.Empty;
            string sploitname = string.Empty;
            string sploitlocation = string.Empty;
            DateTime sploitdate = DateTime.Now;    //=string.Empty;
            string sploitplatform = string.Empty;
            string sploitauthor = string.Empty;
            XORCISMEntities model=new XORCISMEntities();
            model.Configuration.AutoDetectChangesEnabled = false;
            model.Configuration.ValidateOnSaveEnabled = false;

            while (ligne != null)
            {
                if (cpt == 7)
                {
                    sploitauthor = ligne;
                    cpt = 8;
                }
                if (cpt == 6)
                {
                    //sploitauthorid = ligne;
                    cpt = 7;
                }
                if (cpt == 5)
                {
                    sploitplatform = ligne;
                    cpt = 6;
                }
                if (cpt == 4)
                {
                    sploitname = ligne.Replace("CVE:", "CVE-");
                    cpt = 5;
                }
                if (cpt == 3)
                {
                    try
                    {
                        //                        sploitrefid = Convert.ToInt32(ligne);
                        sploitrefid = ligne;
                    }
                    catch (FormatException ex)
                    {
                        string sIgnoreWarning = ex.Message;
                        Console.WriteLine("Exception: sploitrefid Input string is not a sequence of digits:" + ligne);
                    }
                    catch (OverflowException ex)
                    {
                        string sIgnoreWarning = ex.Message;
                        Console.WriteLine("Exception: sploitrefid The number cannot fit in an Int32:" + ligne);
                    }
                    cpt = 4;
                }
                if (cpt == 2)
                {
                    sploitlocation = ligne.Replace("exploit-db.com/download", "exploit-db.com/exploits");
                    
                    cpt = 3;
                }
                if (cpt == 1)
                {
                    try
                    {
                        sploitdate = Convert.ToDateTime(ligne + "T00:00:00.9843750-04:00"); //Hardcoded
                    }
                    catch (FormatException ex)
                    {
                        string sIgnoreWarning = ex.Message;
                        Console.WriteLine("Exception: sploitdate Input string is not a date.");
                    }
                    catch (OverflowException ex)
                    {
                        string sIgnoreWarning = ex.Message;
                        Console.WriteLine("Exception: sploitdate The number cannot fit in a date.");
                    }

                    cpt = 2;    //Next line is the URL
                }



                Regex myRegexCVE = new Regex("CVE-[0-9][0-9][0-9][0-9]-[0-9][0-9][0-9][0-9]"); //TODO review \d+

                strTemp = myRegexCVE.Match(ligne).ToString();
                if (strTemp != "")
                {
                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                    Console.WriteLine("DEBUG regex found " + strTemp);
                    curCVE = strTemp;
                    //Search the VulnerabilityID
                    var synx = from S in vuln_model.VULNERABILITY
                               where S.VULReferential.Equals("cve")
                               && S.VULReferentialID.Equals(curCVE)
                               select S.VulnerabilityID;
                    if (synx.Count() != 0)
                    {
                        vulnID = synx.ToList().First(); //.VulnerabilityID;
                        //                        Console.WriteLine("VulnerabilityID of " + curCVE + " is:" + vulnID);
                    }

                    //cpt = 1;
                }

                if(cpt==8)
                {
                    if (curCVE != "")   //Just in case
                    {
                        EXPLOIT sploit = new EXPLOIT();
                        sploit.ExploitReferential = "exploit-db";
                        sploit.ExploitRefID = sploitrefid;
                        sploit.ExploitName = sploitname;
                        Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                        Console.WriteLine("DEBUG sploitname=" + sploitname);    //TODO? &lt;
                        
                        sploitlocation = sploitlocation.Replace("http://www.", "http://");  //TODO review, e.g. www.exploit-db.com
                        sploitlocation = sploitlocation.Replace("https://www.", "https://");
                        sploitlocation = sploitlocation.Replace("/download/", "/exploits/");    //TODO Review Hardcoded
                        if (sploitlocation.ToLower().StartsWith("exploit-db"))
                        {
                            sploitlocation = "http://" + sploitlocation;
                        }
                        sploit.ExploitLocation = sploitlocation;
                        Console.WriteLine("DEBUG sploitlocation=" + sploitlocation);
                        sploit.Date = sploitdate;
                        //sploit.Verification
                        sploit.Platform = sploitplatform;
                        Console.WriteLine("DEBUG sploitplatform=" + sploitplatform);
                        sploit.Author = sploitauthor;
                        Console.WriteLine("DEBUG sploitauthor=" + sploitauthor);
                        //TODO AUTHOR table

                        //Check if the sploit already exists in the db
                        //TODO REVIEW REMOVE sploitlocation?
                        int ExploitID = 0;
                        var syn = from S in model.EXPLOIT
                                  where S.ExploitReferential.Equals("exploit-db")
                                  && S.ExploitRefID.Equals(sploitrefid)
                                  && S.ExploitLocation.Equals(sploitlocation)
                                  select S.ExploitID;
                        if (syn.Count() != 0)
                        {
                            //Update EXPLOIT
                            ExploitID = syn.ToList().First();   //.ExploitID;
                            //Update EXPLOIT
                            //TODO REVIEW
                            sploitlocation = sploit.ExploitLocation;
                            sploitlocation = sploitlocation.Replace("http://www.", "http://");
                            sploitlocation = sploitlocation.Replace("https://www.", "https://");
                            sploitlocation = sploitlocation.Replace("/download/", "/exploits/");    //TODO REVIEW Hardcoded
                            if (sploitlocation.ToLower().StartsWith("exploit-db"))
                            {
                                sploitlocation = "http://" + sploitlocation;
                            }
                            sploit.ExploitLocation = sploitlocation;   //TODO review
                            sploit.timestamp = DateTimeOffset.Now;
                        }
                        else
                        {
                            sploit.CreatedDate = DateTimeOffset.Now;
                            //sploit.VocabularyID=    //TODO
                            sploit.timestamp = DateTimeOffset.Now;
                            model.EXPLOIT.Add(sploit);
                            Console.WriteLine("DEBUG Added New Exploit! " + sploitlocation);
                        }

                        try
                        {
                            model.SaveChanges();
                            if (ExploitID == 0)
                            {
                                ExploitID = sploit.ExploitID;
                            }
                        }
                        catch (Exception exAddToEXPLOIT02)
                        {
                            Console.WriteLine("Exception: exAddToEXPLOIT02 " + exAddToEXPLOIT02.Message + " " + exAddToEXPLOIT02.InnerException);
                        }

                        #region exploitreferencems
                        Regex RegexMS = new Regex("MS[0-9][0-9]-[0-9][0-9][1-9]", RegexOptions.IgnoreCase);
                        strTemp = RegexMS.Match(sploit.ExploitName).ToString();
                        if (strTemp != "")
                        {
                            strTemp = strTemp.ToUpper();
                            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                            Console.WriteLine("DEBUG MSpatch=" + strTemp);

                            int iReferenceMSID = 0;
                            try
                            {
                                iReferenceMSID = model.REFERENCE.FirstOrDefault(o => o.ReferenceTitle == strTemp).ReferenceID;
                            }
                            catch(Exception ex)
                            {

                            }
                            if (iReferenceMSID <= 0)
                            {
                                try
                                {
                                    REFERENCE oReferenceMS = new REFERENCE();
                                    oReferenceMS.CreatedDate = DateTimeOffset.Now;
                                    oReferenceMS.Source = "MS";
                                    oReferenceMS.ReferenceSourceID = strTemp;
                                    oReferenceMS.ReferenceTitle = strTemp;
                                    oReferenceMS.ReferenceURL = "https://technet.microsoft.com/library/security/" + strTemp;
                                    //"http://www.microsoft.com/technet/security/bulletin/" + strTemp;
                                    oReferenceMS.VocabularyID = iVocabularyEXPLOITDBID;
                                    oReferenceMS.timestamp = DateTimeOffset.Now;
                                    model.REFERENCE.Add(oReferenceMS);
                                    model.SaveChanges();
                                    iReferenceMSID = oReferenceMS.ReferenceID;
                                }
                                catch (Exception exoReferenceMS)
                                {
                                    Console.WriteLine("Exception: exoReferenceMS " + exoReferenceMS.Message + " " + exoReferenceMS.InnerException);
                                }
                            }

                            #region EXPLOITFORREFERENCE
                            int iExploitReferenceID = 0;
                            try
                            {
                                iExploitReferenceID = model.EXPLOITFORREFERENCE.FirstOrDefault(o => o.ExploitID == sploit.ExploitID && o.ReferenceID == iReferenceMSID).ExploitReferenceID;
                            }
                            catch(Exception ex)
                            {

                            }
                            if(iExploitReferenceID<=0)
                            {
                                try
                                {
                                    EXPLOITFORREFERENCE oExploitReferenceMS = new EXPLOITFORREFERENCE();
                                    oExploitReferenceMS.CreatedDate = DateTimeOffset.Now;
                                    oExploitReferenceMS.ExploitID = sploit.ExploitID;
                                    oExploitReferenceMS.ReferenceID = iReferenceMSID;
                                    oExploitReferenceMS.VocabularyID = iVocabularyEXPLOITDBID;
                                    oExploitReferenceMS.timestamp = DateTimeOffset.Now;
                                    model.EXPLOITFORREFERENCE.Add(oExploitReferenceMS);
                                    model.SaveChanges();
                                }
                                catch(Exception exoExploitReferenceMS)
                                {
                                    Console.WriteLine("Exception: exoExploitReferenceMS " + exoExploitReferenceMS.Message + " " + exoExploitReferenceMS.InnerException);
                                }
                            }
                            else
                            {
                                //Update EXPLOITFORREFERENCE
                            }
                            #endregion EXPLOITFORREFERENCE
                        }
                        #endregion exploitreferencems

                        //TODO check LastDateChecked
                        //Check the Date and content of the local exploit file
                        
                        string sCurrentPath = Directory.GetCurrentDirectory();
                        string sExploitDBLocalFile = sCurrentPath + @"\exploit-db\" + sploitrefid + ".txt";
                        FileInfo fileInfo = new FileInfo(@sExploitDBLocalFile);
                        if(fileInfo.Exists)
                        {
                            if((DateTime.Now - fileInfo.LastWriteTime).Hours < 12)  //TODO Hardcoded
                            {
                                using (StreamReader sr = new StreamReader(@sExploitDBLocalFile))
                                {
                                    string contents = sr.ReadToEnd();
                                    if (contents.Contains("EDB-ID"))
                                    {
                                        bRequestExploitDB = false;
                                    }
                                }
                            }
                        }

                        if(bRequestExploitDB)
                        {
                            #region requestexploitdb
                            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                            Console.WriteLine("DEBUG Visit the exploit-db page to find EXPLOITFORREFERENCE (osvdb) ...");
                            HttpWebRequest request;
                            HttpWebResponse response = null;
                            string ResponseText = "";
                            //string MyCookie = "";
                            StreamReader SR = null;
                            Console.WriteLine("DEBUG sploitlocation=" + sploitlocation);
                            try
                            {
                                request = (HttpWebRequest)HttpWebRequest.Create(sploitlocation);
                                request.Method = "GET";
                                //request.AllowAutoRedirect = true;
                                request.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.0; en-US; rv:1.4) Gecko/20030624 Netscape/7.1 (ax)";

                                response = (HttpWebResponse)request.GetResponse();
                                SR = new StreamReader(response.GetResponseStream());
                                ResponseText = SR.ReadToEnd();
                                SR.Close();
                                response.Close();

                                if (!ResponseText.Contains("EDB-ID"))
                                {
                                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                    Console.WriteLine("ERROR: ResponseText NOT OK");
                                    //Console.WriteLine(ResponseText);

                                    //Try again
                                    Thread.Sleep(5000);
                                    request = (HttpWebRequest)HttpWebRequest.Create(sploitlocation);
                                    request.Method = "GET";
                                    //request.AllowAutoRedirect = true;
                                    request.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.0; en-US; rv:1.4) Gecko/20030624 Netscape/7.1 (ax)";

                                    response = (HttpWebResponse)request.GetResponse();
                                    SR = new StreamReader(response.GetResponseStream());
                                    ResponseText = SR.ReadToEnd();
                                    SR.Close();
                                    response.Close();
                                }
                                if (!ResponseText.Contains("EDB-ID"))
                                {
                                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                    Console.WriteLine("ERROR: ResponseText STILL NOT OK");
                                    //Console.WriteLine(ResponseText);
                                }
                                else
                                {
                                    System.IO.File.WriteAllText(@sExploitDBLocalFile, ResponseText);

                                    //TODO
                                    //Update EXPLOIT?
                                    if (ResponseText.Contains("alt=\"Verified\""))
                                    {
                                        try
                                        {
                                            //Update EXPLOIT
                                            sploit.Verification = true;
                                            sploit.timestamp = DateTimeOffset.Now;
                                            model.SaveChanges();
                                        }
                                        catch (Exception exSploitUpdate)
                                        {
                                            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                            Console.WriteLine("Exception: exSploitUpdate " + exSploitUpdate.Message + " " + exSploitUpdate.InnerException);
                                        }
                                    }
                                    //TODO
                                    //EXPLOITLANGUAGE


                                    //TODO?
                                    //Update REFERENCE

                                    ResponseText = ResponseText.Replace("osvdb.org/displayvuln.php?osvdbid=", "osvdb.org/show/osvdb/");
                                    Regex myRegexOSVDB = new Regex("http://osvdb.org/show/osvdb/[^<>]*\"");
                                    strTemp = myRegexOSVDB.Match(ResponseText).ToString();
                                    if (strTemp != "")
                                    {
                                        string sOSVDBID = strTemp;
                                        sOSVDBID = sOSVDBID.Replace("http://osvdb.org/show/osvdb/", "");
                                        sOSVDBID = sOSVDBID.Replace("\" target=\"_blank\"", "").Trim();

                                        Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                        Console.WriteLine("DEBUG OSVDB found on EXPLOIT-DB:" + sOSVDBID);
                                        string sReferenceURL = "http://osvdb.org/" + sOSVDBID;

                                        REFERENCE oReference = model.REFERENCE.FirstOrDefault(o => o.Source == "OSVDB" && o.ReferenceURL == sReferenceURL);
                                        if (oReference == null)
                                        {
                                            oReference = new REFERENCE();
                                            oReference.Source = "OSVDB";
                                            oReference.ReferenceSourceID = sOSVDBID;
                                            oReference.ReferenceURL = sReferenceURL;
                                            //oReference.SourceTrustLevelID   //TODO High
                                            oReference.CreatedDate = DateTimeOffset.Now;
                                            oReference.timestamp = DateTimeOffset.Now;
                                            oReference.VocabularyID = iVocabularyEXPLOITDBID;
                                            try
                                            {
                                                model.REFERENCE.Add(REFERENCENormalize(model,oReference));
                                                model.SaveChanges();
                                            }
                                            catch (Exception exREFOSDVDB)
                                            {
                                                Console.WriteLine("Exception: exREFOSDVDB " + exREFOSDVDB.Message + " " + exREFOSDVDB.InnerException);
                                            }
                                        }
                                        else
                                        {
                                            //Update REFERENCE
                                            //TODO Remove
                                            /*
                                            oReference.ReferenceSourceID = sOSVDBID;
                                            //oReference.SourceTrustLevelID   //TODO High
                                            oReference.VocabularyID = iVocabularyEXPLOITDBID;
                                            oReference = REFERENCENormalize(model,oReference);
                                            model.SaveChanges();
                                            */
                                        }

                                        //OSVDB is a Vulnerability
                                        //NOTE: used later in fRequestOSVDB() TODO REVIEW
                                        VULNERABILITY oVULNERABILITYOSVDB = vuln_model.VULNERABILITY.Where(o => o.VULReferential == "osvdb" && o.VULReferentialID == sOSVDBID).FirstOrDefault();
                                        int iVulnerabilityID = 0;
                                        /*
                                        try
                                        {
                                            iVulnerabilityID = model.VULNERABILITY.FirstOrDefault(o => o.VULReferential == "osvdb" && o.VULReferentialID == sOSVDBID).VulnerabilityID;
                                        }
                                        catch(Exception ex)
                                        {

                                        }
                                        */
                                        if (oVULNERABILITYOSVDB != null)
                                        //if (iVulnerabilityID>0)
                                        {
                                            //Update VULNERABILITY OSVDB
                                            iVulnerabilityID = oVULNERABILITYOSVDB.VulnerabilityID;
                                        }
                                        else
                                        {
                                            oVULNERABILITYOSVDB = new VULNERABILITY();
                                            oVULNERABILITYOSVDB.VULReferential = "osvdb";
                                            oVULNERABILITYOSVDB.VULReferentialID = sOSVDBID;
                                            //The details will be added later when parsing the OSVDB file
                                            oVULNERABILITYOSVDB.VocabularyID = iVocabularyEXPLOITDBID;
                                            oVULNERABILITYOSVDB.CreatedDate = DateTimeOffset.Now;
                                            oVULNERABILITYOSVDB.timestamp = DateTimeOffset.Now;
                                            try
                                            {
                                                vuln_model.VULNERABILITY.Add(oVULNERABILITYOSVDB);
                                                vuln_model.SaveChanges();
                                                iVulnerabilityID = oVULNERABILITYOSVDB.VulnerabilityID;
                                            }
                                            catch (Exception exoVULNERABILITYOSVDB)
                                            {
                                                Console.WriteLine("Exception: oVULNERABILITYOSVDB " + exoVULNERABILITYOSVDB.Message + " " + exoVULNERABILITYOSVDB.InnerException);
                                            }
                                        }

                                        //NOTE: for VULNERABILITYMAPPING involving a CVE, the VulnerabilityRefID will be the one of the CVE
                                        //VULNERABILITYMAPPING oVULMAPPINGOSVDBCVE = model.VULNERABILITYMAPPING.Where(o => o.VulnerabilityRefID == vulnID && o.VulnerabilitySubjectID == iVulnerabilityID).FirstOrDefault();
                                        int iVulnerabilityMappingID = 0;
                                        try
                                        {
                                            iVulnerabilityMappingID = vuln_model.VULNERABILITYMAPPING.FirstOrDefault(o => o.VulnerabilityRefID == vulnID && o.VulnerabilitySubjectID == iVulnerabilityID).VulnerabilityMappingID;
                                        }
                                        catch(Exception ex)
                                        {

                                        }
                                        //if (oVULMAPPINGOSVDBCVE != null)
                                        if (iVulnerabilityMappingID>0)
                                        {
                                            //Update VULNERABILITYMAPPING
                                        }
                                        else
                                        {
                                            VULNERABILITYMAPPING oVULMAPPINGOSVDBCVE = new VULNERABILITYMAPPING();
                                            oVULMAPPINGOSVDBCVE.VulnerabilityRefID = vulnID; //CVE
                                            oVULMAPPINGOSVDBCVE.VulnerabilitySubjectID = iVulnerabilityID;  // oVULNERABILITYOSVDB.VulnerabilityID;   //OSVDB
                                            oVULMAPPINGOSVDBCVE.RelationshipText = "OSVDB";
                                            oVULMAPPINGOSVDBCVE.VocabularyID = iVocabularyOSVDBID;
                                            //oVULMAPPINGOSVDBCVE.ConfidenceLevelID   //TODO High
                                            oVULMAPPINGOSVDBCVE.CreatedDate = DateTimeOffset.Now;
                                            oVULMAPPINGOSVDBCVE.timestamp = DateTimeOffset.Now;
                                            try
                                            {
                                                vuln_model.VULNERABILITYMAPPING.Add(oVULMAPPINGOSVDBCVE);
                                                vuln_model.SaveChanges();
                                                //iVulnerabilityMappingID=
                                            }
                                            catch (Exception exoVULMAPPINGOSVDBCVE)
                                            {
                                                Console.WriteLine("Exception: exoVULMAPPINGOSVDBCVE " + exoVULMAPPINGOSVDBCVE.Message + " " + exoVULMAPPINGOSVDBCVE.InnerException);
                                            }
                                        }


                                        //EXPLOITFORREFERENCE oExploitReference = model.EXPLOITFORREFERENCE.FirstOrDefault(o => o.ExploitID == sploit.ExploitID && o.ReferenceID == oReference.ReferenceID);
                                        int iReferenceExploitID = 0;
                                        try
                                        {
                                            iReferenceExploitID=model.EXPLOITFORREFERENCE.Where(o => o.ExploitID == sploit.ExploitID && o.ReferenceID == oReference.ReferenceID).Select(o => o.ExploitReferenceID).FirstOrDefault();
                                        }
                                        catch(Exception ex)
                                        {

                                        }
                                        //if (oExploitReference == null)
                                        if (iReferenceExploitID <= 0)
                                        {
                                            EXPLOITFORREFERENCE oExploitReference = new EXPLOITFORREFERENCE();
                                            oExploitReference.ReferenceID = oReference.ReferenceID; //OSVDB
                                            oExploitReference.ExploitID = ExploitID;
                                            oExploitReference.CreatedDate = DateTimeOffset.Now;
                                            oExploitReference.timestamp = DateTimeOffset.Now;
                                            oExploitReference.VocabularyID = iVocabularyEXPLOITDBID;
                                            try
                                            {
                                                model.EXPLOITFORREFERENCE.Add(oExploitReference);
                                                model.SaveChanges();
                                                //iReferenceExploitID=
                                            }
                                            catch (Exception exEXPLOITFORREFERENCEOSVDB)
                                            {
                                                Console.WriteLine("Exception: exEXPLOITFORREFERENCEOSVDB " + exEXPLOITFORREFERENCEOSVDB.Message + " " + exEXPLOITFORREFERENCEOSVDB.InnerException);
                                            }
                                        }
                                        else
                                        {
                                            //Update EXPLOITFORREFERENCE
                                        }

                                        //VULNERABILITYFORREFERENCE oVulnerabilityReference = model.VULNERABILITYFORREFERENCE.FirstOrDefault(o => o.VulnerabilityID == vulnID && o.ReferenceID == oReference.ReferenceID);
                                        int iReferenceVulnerabilityID = 0;
                                        try
                                        {
                                            iReferenceVulnerabilityID = vuln_model.VULNERABILITYFORREFERENCE.Where(o => o.VulnerabilityID == vulnID && o.ReferenceID == oReference.ReferenceID).Select(o => o.VulnerabilityReferenceID).FirstOrDefault();
                                        }
                                        catch(Exception ex)
                                        {

                                        }
                                        //if (oVulnerabilityReference == null)
                                        if (iReferenceVulnerabilityID <= 0)
                                        {
                                            VULNERABILITYFORREFERENCE oVulnerabilityReference = new VULNERABILITYFORREFERENCE();
                                            oVulnerabilityReference.ReferenceID = oReference.ReferenceID;   //OSVDB
                                            oVulnerabilityReference.VulnerabilityID = vulnID;   //CVE
                                            oVulnerabilityReference.CreatedDate = DateTimeOffset.Now;
                                            oVulnerabilityReference.timestamp = DateTimeOffset.Now;
                                            oVulnerabilityReference.VocabularyID = iVocabularyEXPLOITDBID;
                                            try
                                            {
                                                vuln_model.VULNERABILITYFORREFERENCE.Add(oVulnerabilityReference);
                                                vuln_model.SaveChanges();
                                                //iReferenceVulnerabilityID=
                                            }
                                            catch (Exception exVULNERABILITYFORREFERENCEOSVDB)
                                            {
                                                Console.WriteLine("Exception: exVULNERABILITYFORREFERENCEOSVDB " + exVULNERABILITYFORREFERENCEOSVDB.Message + " " + exVULNERABILITYFORREFERENCEOSVDB.InnerException);
                                            }
                                        }
                                        else
                                        {
                                            //Update VULNERABILITYFORREFERENCE
                                        }

                                        fRequestOSVDB(sReferenceURL, sOSVDBID, model, oReference, oVULNERABILITYOSVDB);
                                    }   //if (strTemp != "")
                                }
                            }
                            catch(Exception exSearchOSVDBonEDB)
                            {
                                Console.WriteLine("Exception: exSearchOSVDBonEDB " + exSearchOSVDBonEDB.Message + " " + exSearchOSVDBonEDB.InnerException);
                            }
                            #endregion requestexploitdb
                        }

                        //Check if the EXPLOITFORVULNERABILITY exists in the database
                        Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                        Console.WriteLine("DEBUG ExploitID=" + ExploitID);
                        var syn2 = from S in model.EXPLOITFORVULNERABILITY
                                   where S.VulnerabilityID.Equals(vulnID)
                                   && S.ExploitID.Equals(ExploitID)
                                   select S.VulnerabilityExploitID;
                        if (syn2.Count() != 0)
                        {
                            //Update EXPLOITFORVULNERABILITY
                        }
                        else
                        {
                            
                            try
                            {
                                EXPLOITFORVULNERABILITY sploitvuln = new EXPLOITFORVULNERABILITY();
                                sploitvuln.VulnerabilityID = vulnID;
                                sploitvuln.ExploitID = ExploitID;   //sploit.ExploitID;
                                sploitvuln.CreatedDate = DateTimeOffset.Now;
                                sploitvuln.timestamp = DateTimeOffset.Now;
                                //sploitvuln.VocabularyID=    //TODO
                                model.EXPLOITFORVULNERABILITY.Add(sploitvuln);
                                model.SaveChanges();
                                Console.WriteLine("DEBUG new EXPLOITFORVULNERABILITY added");
                            }
                            catch (Exception exAddToEXPLOITFORVULNERABILITY06)
                            {
                                Console.WriteLine("Exception: exAddToEXPLOITFORVULNERABILITY06 " + exAddToEXPLOITFORVULNERABILITY06.Message + " " + exAddToEXPLOITFORVULNERABILITY06.InnerException);
                            }
                        }
                    }

                    cpt = 1;    //8 -> 1
                }

                //In case we had an error before
                if(ligne.StartsWith("CVE-") && cpt==0)
                {
                    cpt = 1;    //Next line is the date
                }

                ligne = monStreamReader.ReadLine();
            }
            monStreamReader.Close();    //CVE-EXPLOITS.txt

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
                Console.WriteLine("Exception: DbEntityValidationExceptionFINALSAVEINSERTEXPLOITDB " + sb.ToString());
            }
            catch (Exception exFINALSAVE)
            {
                Console.WriteLine("Exception: exFINALSAVEINSETEXPLOITDB " + exFINALSAVE.Message + " " + exFINALSAVE.InnerException);
            }
            model.Dispose();

            Console.WriteLine("DEBUG FINISHED BOSS!");
        }
    }
}
