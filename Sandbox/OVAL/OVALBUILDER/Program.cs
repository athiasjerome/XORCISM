using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;

//using System.Data;
using System.Data.Entity;
//using System.Xml;

using XORCISMModel;
using XOVALModel;
using XVULNERABILITYModel;

using System.Net;
using System.Net.Http;  //For MS API
//using System.Net.Http.Headers;  //For MS API

using System.IO;
using System.Text.RegularExpressions;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
//using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;   //For MS popup
//using OpenQA.Selenium.Remote;

using System.Web;
//using System.Data.Entity;
using System.Globalization;
using System.Diagnostics;   //ProcessStartInfo

using System.Threading; //Sleep

namespace OVALBuilder
{
    class Program
    {
        //NOTE: This tool could require a Microsoft (Business/Education) Account (i.e. Office 365) and Chrome (used by SeleniumHQ Driver) to be authenticated with it. Also a MS Update API Key could be needed
        /// <summary>
        /// Copyright (C) 2017 Jerome Athias. Version 0.1 ALPHA (aka "The Dirty One") ((Dirty Deeds Done Dirt Cheap))
        /// (PoC) OVAL Vulnerability Definition Generator (Microsoft Vulnerabilities Checks) for Microsoft Windows Products (using an XORCISM database)
        /// Note: If you don't want to use a (XORCISM) database (i.e. directly use CIS OVAL github repo): replace the Linq queries by "XPath" queries against your OVAL (github/XML) repo
        ///
        /// This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation; either version 2 of the License, or (at your option) any later version.
        /// 
        /// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
        /// 
        /// You should have received a copy of the GNU General Public License along with this program; if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
		/// All trademarks, product names, company names, publisher name and their respective logos cited herein are the property of their respective owners.
        /// </summary>

        public static string sMSUpdateAPIKey = "CHANGEME";  //HARDCODED

        public static string sLocalPathForMSKBfiles = "E:\\MSKB\\"; //HARDCODED Local Path to save MSCatalogKBfiles (.cab, .msu, etc.)
        public static string sLocalOVALRepoPath = "E:\\OVALRepo\\repository\\"; //Hardcoded Path
        public static Boolean bDebugCPE = false;
        public static Boolean bForceDecompression = false;   //Enforce/not (re)decompression of .cab, .msu, etc.
        public static Boolean bDebugDecompression = false;  //Print/not debug info regarding decompression (see 7zip & expand)
        public static Boolean bDebugFilesFound = false;     //Print all files found in the decompressed cabs...
        public static Boolean bDebugFileSelection = false;
        public static Boolean bUseManifestFilesVersions = false;    //Use file versions from .manifest files (not reliable)
        public static bool bSaveSpace = false;  //Save local space by zeroing the KB files (cab, msu, msi, etc.) after decompression: TODO   and delete non-interesting extracted files (e.g.: .mui)
        public static bool bDownloadOnlyDeltaUpdates = true;    //Download only Dela Updates (so not Cumulative) when available (less requests, less download, less space used)
        public static bool bDEBUGMODE = true; //true: do not copy generated files (definition, tests, objects...) to your local github OVALRepo
        public static bool bWriteOVAL = true;  //i.e. false for testing
        public static bool bDEBUGMODEINXML = false; //true: write debug information directly in the OVAL Definition XML file generated
        public static bool bUseOnlyCPEs = true;   //true: include only the products found in the CPEs list of a CVE (this is for when a MS bulletin includes various CVEs) (do not work on patch tuesday while NVD CVE still not available)
        public static int iSleepMore = 0;   //2000;    //Adjust for slow connection

        #region globalvariables
        public static XORCISMEntities model = new XORCISMEntities();
        public static XOVALEntities oval_model = new XOVALEntities();
        public static XVULNERABILITYEntities vuln_model = new XVULNERABILITYEntities();
        //TODO: Retrieve this from the database
        public static int iOSFamilyWindowsID = 114;    //HARDCODED windows
        public static int iOVALClassEnumerationInventoryID = 4;    //HARDCODED inventory


        public static List<string> lMainProductsNames = new List<string>();     //TODO Could use the MS API
        public static List<string> lFileNamesToSearch = new List<string>();     //We use a list to avoid "errors" (i.e. Win32k.sys vs Win32kfull.sys) e.g.: gdiplus.dll, ogl.dll, etc.
        public static List<string> lFileNamesNOTToSearch = new List<string>();  //Excluded filenames. Useful, for example, when 1 (cumulative) KB covers 2-3 CVEs (for 2-3 different files)
        public static string sFileNameToSearchGlobal = "";
        public static List<string> lFilesToUse = new List<string>();    //For cosmetic

        //TODO: These lists are for the function fParseKBForFiles() => create/use a class to return them
        public static List<string> lFilenames = new List<string>();
        public static List<string> lFileversions = new List<string>();
        public static List<string> lFiledates = new List<string>();
        public static List<string> lFileplatforms = new List<string>();

        public static List<string> lProductsMicrosoft = new List<string>();   //products names "compatible" with Microsoft MS- KB pages

        public static List<string> lOVALDefInventoryNotRetrieved = new List<string>();  //To avoid useless queries (CIS community please help there)
        public static List<string> lVisitedURLs = new List<string>();  //To avoid useless (duplicate) requests
        public static List<string> lMSCatalogURLFile = new List<string>();
        public static Dictionary<string, string> dKBURLFile = new Dictionary<string, string>();       //To avoid parsing the same KBfile multiple times
        public static Dictionary<string, string> dProductFile = new Dictionary<string, string>();

        public static string sProductFoundDEADBEEFGlobal = "";

        public static Regex _htmlRegex = new Regex("<.*?>", RegexOptions.Compiled); //Remove HTML from strings  Compiled regular expression for performance.

        public static string sCurrentPath = Directory.GetCurrentDirectory();
        
        public static List<string> lProducts = new List<string>();    //simplest names for the products (used to find the OVAL Inventory Definitions)
        public static List<string> lProductsCPEs = new List<string>();    //products names retrieved from the CPEs (this is for bUseOnlyCPEs)
        public static string sVULDescription = string.Empty;

        public static int iPlatformWin2000 = 0;
        public static int iPlatformWinVista = 0;
        public static int iPlatformWinXP = 0;
        public static int iPlatformWin7 = 0;
        public static int iPlatformWin8 = 0;
        public static int iPlatformWin81 = 0;
        public static int iPlatformWin10 = 0;
        public static int iPlatformWin10v1511 = 0;    //Microsoft Windows 10 version 1511
        public static int iPlatformWin2003 = 0;
        public static int iPlatformWin2003R2 = 0;
        public static int iPlatformWin2008 = 0;
        public static int iPlatformWin2008R2 = 0;
        public static int iPlatformWin2012 = 0;
        public static int iPlatformWin2012R2 = 0;
        public static int iPlatformWin2016 = 0;

        public static bool bPatchJustAdded = true; //not perfect
        #endregion globalvariables

        static void Main(string[] args) //CVE-2016-7295 Clfs.sys
        {
            //TODO: Input Validation
            //TODO:    Limited Distribution Release (LDR) vs GDR
            //TODO: NOTE:   Does not support this complexity level   oval:org.cisecurity:def:411 VBScript 5.8 installed + vulnerable IE + vulnerable Windows OS + vulnerable file version
            #region xorcismdb
            model.Configuration.AutoDetectChangesEnabled = false;  //Speed
            model.Configuration.ValidateOnSaveEnabled = false;
            oval_model.Configuration.AutoDetectChangesEnabled = false;  //Speed
            oval_model.Configuration.ValidateOnSaveEnabled = false;
            vuln_model.Configuration.AutoDetectChangesEnabled = false;  //Speed
            vuln_model.Configuration.ValidateOnSaveEnabled = false;

                #region hardcoding
                    Dictionary<string, int> dPlatforms = new Dictionary<string, int>();
                    Dictionary<string, int> dOVALPlatforms = new Dictionary<string, int>();

                    #region platforms
                    

                    int iPlatformID = 0;
                    string[] PLATFORMS = { "Microsoft Windows 2000", "Microsoft Windows Vista", "Microsoft Windows XP", "Microsoft Windows 7", "Microsoft Windows 8", "Microsoft Windows 8.1", "Microsoft Windows 10", "Microsoft Windows 10 version 1511", "Microsoft Windows Server 2003", "Microsoft Windows Server 2003 R2", "Microsoft Windows Server 2008", "Microsoft Windows Server 2008 R2", "Microsoft Windows Server 2012", "Microsoft Windows Server 2012 R2", "Microsoft Windows Server 2016" };  //Hardcoded
                    
                    foreach (string sPlatform in PLATFORMS)
                    {
                        #region platform
                        try
                        {
                            iPlatformID = model.PLATFORM.Where(o => o.PlatformName == sPlatform).Select(o => o.PlatformID).FirstOrDefault();  //Note: case insensitive
                            //iPlatformID = oval_model.PLATFORM.Where(o => o.PlatformName == sPlatform).Select(o => o.PlatformID).FirstOrDefault();  //Note: case insensitive
                        }
                        catch (Exception exiPlatformID)
                        {
                            Console.WriteLine("Exception: exiPlatformID: " + exiPlatformID.Message + " " + exiPlatformID.InnerException);
                        }
                        if (iPlatformID <= 0)
                        {
                            //Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                            Console.WriteLine(string.Format("DEBUG Adding new PLATFORM [{0}] in table PLATFORM", sPlatform));

                            try
                            {
                                XORCISMModel.PLATFORM myPlatform = new XORCISMModel.PLATFORM();
                                myPlatform.PlatformName = sPlatform;
                                //myPlatform.VocabularyID = iVocabularyOVALID;
                                myPlatform.CreatedDate = DateTimeOffset.Now;
                                myPlatform.timestamp = DateTimeOffset.Now;
                                //model.Entry(myPlatform).State = EntityState.Detached;
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
                        #endregion platform

                        switch (sPlatform)
                        {
                            case "Microsoft Windows 2000":
                                iPlatformWin2000 = iPlatformID;
                                break;
                            case "Microsoft Windows Vista":
                                iPlatformWinVista = iPlatformID;
                                break;
                            case "Microsoft Windows XP":
                                iPlatformWinXP = iPlatformID;
                                break;
                            case "Microsoft Windows 7":
                                iPlatformWin7 = iPlatformID;
                                break;
                            case "Microsoft Windows 8":
                                iPlatformWin8 = iPlatformID;
                                break;
                            case "Microsoft Windows 8.1":
                                iPlatformWin81 = iPlatformID;
                                break;
                            case "Microsoft Windows 10":
                                iPlatformWin10 = iPlatformID;
                                break;
                            case "Microsoft Windows 10 version 1511":
                                iPlatformWin10v1511 = iPlatformID;
                                break;
                            case "Microsoft Windows Server 2003":
                                iPlatformWin2003 = iPlatformID;
                                break;
                            case "Microsoft Windows Server 2003 R2":
                                iPlatformWin2003R2 = iPlatformID;
                                break;
                            case "Microsoft Windows Server 2008":
                                iPlatformWin2008 = iPlatformID;
                                break;
                            case "Microsoft Windows Server 2008 R2":
                                iPlatformWin2008R2 = iPlatformID;
                                break;
                            case "Microsoft Windows Server 2012":
                                iPlatformWin2012 = iPlatformID;
                                break;
                            case "Microsoft Windows Server 2012 R2":
                                iPlatformWin2012R2 = iPlatformID;
                                break;
                            case "Microsoft Windows Server 2016":
                                iPlatformWin2016 = iPlatformID;
                                break;
                            default:
                                //ERROR
                                break;
                        }
                    }

                    //==================================================================================================================================================
                    int iOVALPlatformWin2000 = 0;
                    int iOVALPlatformWinVista = 0;
                    int iOVALPlatformWinXP = 0;
                    int iOVALPlatformWin7 = 0;
                    int iOVALPlatformWin8 = 0;
                    int iOVALPlatformWin81 = 0;
                    int iOVALPlatformWin10 = 0;
                    int iOVALPlatformWin10v1511 = 0;
                    int iOVALPlatformWin2003 = 0;
                    int iOVALPlatformWin2003R2 = 0;
                    int iOVALPlatformWin2008 = 0;
                    int iOVALPlatformWin2008R2 = 0;
                    int iOVALPlatformWin2012 = 0;
                    int iOVALPlatformWin2012R2 = 0;
                    int iOVALPlatformWin2016 = 0;
                    
                    int iOVALPlatformID = 0;
                    foreach (string sPlatform in PLATFORMS)
                    {
                        #region ovalplatform
                        try
                        {
                            //iPlatformID = model.PLATFORM.Where(o => o.PlatformName == sPlatform).Select(o => o.PlatformID).FirstOrDefault();  //Note: case insensitive
                            iOVALPlatformID = oval_model.PLATFORM.Where(o => o.PlatformName == sPlatform).Select(o => o.PlatformID).FirstOrDefault();  //Note: case insensitive
                        }
                        catch (Exception exiOVALPlatformID)
                        {
                            Console.WriteLine("Exception: exiOVALPlatformID: " + exiOVALPlatformID.Message + " " + exiOVALPlatformID.InnerException);
                        }
                        if (iOVALPlatformID <= 0)
                        {
                            //Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                            Console.WriteLine(string.Format("DEBUG Adding new PLATFORM [{0}] in table OVALPLATFORM", sPlatform));

                            try
                            {
                                XOVALModel.PLATFORM myPlatform = new XOVALModel.PLATFORM();
                                myPlatform.PlatformName = sPlatform;
                                //myPlatform.VocabularyID = iVocabularyOVALID;
                                myPlatform.CreatedDate = DateTimeOffset.Now;
                                //    myPlatform.timestamp = DateTimeOffset.Now;
                                //model.Entry(myPlatform).State = EntityState.Detached;
                                oval_model.PLATFORM.Add(myPlatform);
                                oval_model.SaveChanges();
                                iOVALPlatformID = myPlatform.PlatformID;
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
                            catch (Exception exAddToOVALPLATFORM)
                            {
                                Console.WriteLine("Exception: exAddToOVALPLATFORM " + exAddToOVALPLATFORM.Message + " " + exAddToOVALPLATFORM.InnerException);
                            }
                        }
                        #endregion ovalplatform

                        switch (sPlatform)
                        {
                            case "Microsoft Windows 2000":
                                iOVALPlatformWin2000 = iOVALPlatformID;
                                break;
                            case "Microsoft Windows Vista":
                                iOVALPlatformWinVista = iOVALPlatformID;
                                break;
                            case "Microsoft Windows XP":
                                iOVALPlatformWinXP = iOVALPlatformID;
                                break;
                            case "Microsoft Windows 7":
                                iOVALPlatformWin7 = iOVALPlatformID;
                                break;
                            case "Microsoft Windows 8":
                                iOVALPlatformWin8 = iOVALPlatformID;
                                break;
                            case "Microsoft Windows 8.1":
                                iOVALPlatformWin81 = iOVALPlatformID;
                                break;
                            case "Microsoft Windows 10":
                                iOVALPlatformWin10 = iOVALPlatformID;
                                break;
                            case "Microsoft Windows 10 version 1511":
                                iOVALPlatformWin10v1511 = iOVALPlatformID;
                                break;
                            case "Microsoft Windows Server 2003":
                                iOVALPlatformWin2003 = iOVALPlatformID;
                                break;
                            case "Microsoft Windows Server 2003 R2":
                                iOVALPlatformWin2003R2 = iOVALPlatformID;
                                break;
                            case "Microsoft Windows Server 2008":
                                iOVALPlatformWin2008 = iOVALPlatformID;
                                break;
                            case "Microsoft Windows Server 2008 R2":
                                iOVALPlatformWin2008R2 = iOVALPlatformID;
                                break;
                            case "Microsoft Windows Server 2012":
                                iOVALPlatformWin2012 = iOVALPlatformID;
                                break;
                            case "Microsoft Windows Server 2012 R2":
                                iOVALPlatformWin2012R2 = iOVALPlatformID;
                                break;
                            case "Microsoft Windows Server 2016":
                                iOVALPlatformWin2016 = iOVALPlatformID;
                                break;
                            default:
                                //ERROR
                                break;
                        }
                    }
                    #endregion platforms

                #endregion hardcoding
            
            #region mssql
            /*
            try
            {
                model.Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED");
                vuln_model.Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED");
            }
            catch (Exception exNOLOCK)
            {
                Console.WriteLine("Exception: exNOLOCK " + exNOLOCK.Message + " " + exNOLOCK.InnerException);
            }
            */
            #endregion mssql
            #endregion xorcismdb

            string sMainProductName = "";           //<h2 class="subheading">Cumulative Security Update for Microsoft Edge (3199057)</h2> => microsoft edge
            string sMainProductNameToTitle = "";    //For cosmetic later on => Microsoft Edge

            //TESTS
            string sCVE = string.Empty; //"CVE-2017-0281";
            //For Patch Tuesday (= no info/CPEs in the CVE yet)
            string sForceMS = "guidance";   // "17-014";//17-012";      //WARNING   HARDCODED   "guidance"
            string sForceKB = string.Empty; //"2596904";
                                            //sProductFoundDEADBEEFGlobal = "Microsoft Office 2007 Service Pack 3" + "DEADBEEF";
                                            //lMainProductsNames.Add("Microsoft Office 2007 Service Pack 3");

            string sForceVULDescription = "";   //"Microsoft Office Remote Code Vulnerability"; //"Office 2007";


            if (sForceMS != "")
            {
                bUseOnlyCPEs = false;   //(we don't know them yet on Patch Tuesday)
                #region forceMS
                try
                {
                    //For XORCISM
                    int iReferenceID = 0;

                    string sNewMSURL = "http://technet.microsoft.com/security/bulletin/ms" + sForceMS;
                    if(sForceMS=="guidance")    //HARDCODED!!!
                    {
                        sNewMSURL = "https://portal.msrc.microsoft.com/en-US/security-guidance/advisory/" + sCVE;
                    }

                    try
                    {
                        iReferenceID = model.REFERENCE.FirstOrDefault(o => o.ReferenceURL == sNewMSURL).ReferenceID;   //Hardcoded
                    }
                    catch (Exception ex)
                    {

                    }
                    if (iReferenceID <= 0)
                    {
                        Console.WriteLine("DEBUG Adding REFERENCE1");
                        REFERENCE oReference = new REFERENCE();
                        oReference.ReferenceURL = sNewMSURL;   //HARDCODEDMS (Review? https)
                        oReference.CreatedDate = DateTimeOffset.Now;
                        oReference.timestamp = DateTimeOffset.Now;
                        if (sForceMS != "guidance")
                        {
                            oReference.ReferenceSourceID = "MS" + sForceMS; //hardcoded
                            oReference.ReferenceTitle = "MS" + sForceMS;
                        }
                        else
                        {
                            oReference.ReferenceTitle = sCVE;
                        }
                        oReference.Source = "MS";
                        
                        oReference.VocabularyID = 1;    //hardcoded
                        model.REFERENCE.Add(oReference);
                        model.Entry(oReference).State = EntityState.Added;
                        model.SaveChanges();
                        iReferenceID = oReference.ReferenceID;
                    }

                    int iVulnID = 0;
                    try
                    {
                        iVulnID = vuln_model.VULNERABILITY.FirstOrDefault(o => o.VULReferentialID == sCVE).VulnerabilityID;
                    }
                    catch (Exception ex)
                    {

                    }
                    if (iVulnID <= 0)
                    {
                        VULNERABILITY oVuln = new VULNERABILITY();
                        oVuln.CreatedDate = DateTimeOffset.Now;
                        oVuln.VULReferential = "cve";   //hardcoded
                        oVuln.VULReferentialID = sCVE;
                        oVuln.VULDescription = sForceVULDescription;    // "Edge";// "Internet Explorer";// "Adobe Flash Player"; //"LSASS service";// sCVE;   //IMPORTANT: Put something there for Patch Tuesday
                        //oVuln.VocabularyID=
                        oVuln.timestamp = DateTimeOffset.Now;
                        vuln_model.VULNERABILITY.Add(oVuln);
                        vuln_model.Entry(oVuln).State = EntityState.Added;
                        vuln_model.SaveChanges();
                        iVulnID = oVuln.VulnerabilityID;
                        Console.WriteLine("DEBUG Added VULNERABILITY");
                    }

                    int iVulnerabilityReferenceID = 0;
                    try
                    {
                        iVulnerabilityReferenceID = vuln_model.VULNERABILITYFORREFERENCE.FirstOrDefault(o => o.VulnerabilityID == iVulnID && o.ReferenceID == iReferenceID).VulnerabilityReferenceID;
                    }
                    catch (Exception ex)
                    {

                    }
                    if (iVulnerabilityReferenceID <= 0)
                    {
                        VULNERABILITYFORREFERENCE oVulnRef = new VULNERABILITYFORREFERENCE();
                        oVulnRef.CreatedDate = DateTimeOffset.Now;
                        oVulnRef.VulnerabilityID = iVulnID;
                        oVulnRef.ReferenceID = iReferenceID;
                        //oVulnRef.VocabularyID=
                        oVulnRef.timestamp = DateTimeOffset.Now;
                        vuln_model.VULNERABILITYFORREFERENCE.Add(oVulnRef);
                        vuln_model.Entry(oVulnRef).State = EntityState.Added;
                        vuln_model.SaveChanges();
                        Console.WriteLine("DEBUG Added VULNERABILITYFORREFERENCE");
                    }

                }
                catch (Exception exForceMS)
                {
                    Console.WriteLine("Exception: exForceMS " + exForceMS.Message + " " + exForceMS.InnerException);
                    return;
                }
                #endregion forceMS
            }

            //HARDCODED (Known unknown/missing OVAL Inventory Definitions)
            lOVALDefInventoryNotRetrieved.Add("excel services");
            lOVALDefInventoryNotRetrieved.Add("word automation services");
            lOVALDefInventoryNotRetrieved.Add("microsoft .net framework 3.5.1");
            lOVALDefInventoryNotRetrieved.Add("windows 10 rtm");
            //microsoft office online server
            //System Center 2012 SP1 - Operation Manager 

            Console.WriteLine("OVALBuilder v0.1 Alpha - Jerome Athias");
            #region getarguments
            //TODO: Input Validation!
            try
            {
                if (args.Length < 1)
                {
                    Console.WriteLine("ERROR: No argument specified.");
                    Console.WriteLine("Usage: OVALBuilder.exe CVEID (Filename) (-xFilenameExcluded)");
                    Console.WriteLine("Examples:");
                    Console.WriteLine("OVALBuilder.exe CVE-2017-0002");
                    Console.WriteLine("OVALBuilder.exe CVE-2016-7212 Asycfilt.dll");
                    Console.WriteLine("OVALBuilder.exe CVE-2017-0254 -xRiched20.dll");
                    return;
                }
                if (args[0].Trim() != "")
                {
                    //TODO:Regex for Input Validation
                    //Lazy WEAK validation
                    if (!args[0].ToLower().StartsWith("cve-"))
                    {
                        Console.WriteLine("ERROR: CVE not accepted.");
                        return;
                    }
                    sCVE = args[0];
                }
                foreach(string sArgument in args)
                {
                    if(sArgument.ToLower().StartsWith("kb="))
                    {
                        sForceKB = sArgument.Replace("kb", "").Replace("=", "");
                    }
                    else
                    {
                        if(sArgument.ToLower().StartsWith("-x"))    //Files to exclude  i.e.:   -xRiched20.dll
                        {
                            lFileNamesNOTToSearch.Add(sArgument.ToLower().Replace("-x",""));
                        }
                    }
                }
            }
            catch (Exception exArgCVE)
            {
                Console.WriteLine("ERROR: CVE not specified.");
                //return;
            }
            try
            {
                Directory.CreateDirectory(sCVE);    //hardcoded
            }
            catch (Exception exCreateDirectoryCVE)
            {
                //Console.WriteLine("Exception: exCreateDirectoryCVE " + exCreateDirectoryCVE.Message + " " + exCreateDirectoryCVE.InnerException);
                //already exists
            }

            string sMSID = "";  //MS16-148  Retrieved later

            string sFileNameToSearch = "";  //If it's known and specified. i.e.: mso.dll, msi.dll
            try
            {
                if (args[1] != "")
                {
                    //sFileNameToSearch = args[1];
                    //Regex for Input Validation
                    sFileNameToSearch = fSearchVulnerableFilename(args[1]).ToLower();
                    if (sFileNameToSearch == "")
                    {
                        Console.WriteLine("ERROR: FileName not accepted.");
                        return;
                    }
                    else
                    {
                        sFileNameToSearchGlobal = sFileNameToSearch;
                        lFileNamesToSearch.Add(sFileNameToSearch.ToLower());
                    }
                }
            }
            catch (Exception exArgFilename)
            {
                Console.WriteLine("NOTE: FilenameToSearch not specified.");
                //return;
            }
            #endregion getarguments

            if (sFileNameToSearch != "") Console.WriteLine("DEBUG: sFileNameToSearch SPECIFIED: " + sFileNameToSearch);
            Regex myRegexFileNameToSearch = new Regex("");    //We could decide to use a Regex.   i.e.: win32*.sys    //TODO
            
            #region retrievelatestIDs
            //TODO Enhance that (will crash if not exist)
            int iTestID = 1;    //TODO
            StreamReader myStreamReader = new StreamReader("TestID.txt");   //HARDCODED
            iTestID = Int32.Parse(myStreamReader.ReadLine());
            myStreamReader.Close();

            int iStateID = 1;    //TODO
            myStreamReader = new StreamReader("StateID.txt");
            iStateID = Int32.Parse(myStreamReader.ReadLine());
            myStreamReader.Close();

            int iObjectID = 1;    //TODO
            myStreamReader = new StreamReader("ObjectID.txt");
            iObjectID = Int32.Parse(myStreamReader.ReadLine());
            myStreamReader.Close();

            #endregion retrievelatestIDs
            
            Console.WriteLine("DEBUG START " + DateTimeOffset.Now);
            Console.WriteLine("DEBUG " + sCVE);
            Console.WriteLine("DEBUG bUseOnlyCPEs=" + bUseOnlyCPEs);
            
            dProductFile = new Dictionary<string, string>();

            //Search the CVE with Microsoft Security Update API
            ////        fMakeMSRCRequest(DateTime.Now.Year.ToString()+ "-"+ DateTime.Now.ToString("MMM"));
            //fMakeMSRCRequest(sCVE);

            if (lProductsMicrosoft.Count == 0)
            {
                //HARDCODED FOR TESTS (and if the CVE does not yet provide the CPEs/Products, i.e. Patch Tuesday) - this would be obtained from the MS API/CVRF
                #region hardcodedmicrosoftproducts
                if (!lProductsMicrosoft.Contains("windows server 2016")) lProductsMicrosoft.Add("windows server 2016");
                if (!lProductsMicrosoft.Contains("windows server 2012 r2")) lProductsMicrosoft.Add("windows server 2012 r2");
                if (!lProductsMicrosoft.Contains("windows server 2012")) lProductsMicrosoft.Add("windows server 2012");

                if (!lProductsMicrosoft.Contains("windows server 2008 itanium-based edition")) lProductsMicrosoft.Add("windows server 2008 itanium-based edition");

                if (!lProductsMicrosoft.Contains("windows server 2008 r2 sp1")) lProductsMicrosoft.Add("windows server 2008 r2 sp1");
                if (!lProductsMicrosoft.Contains("windows server 2008 r2")) lProductsMicrosoft.Add("windows server 2008 r2");

                if (!lProductsMicrosoft.Contains("windows server 2008 sp2")) lProductsMicrosoft.Add("windows server 2008 sp2");
                if (!lProductsMicrosoft.Contains("windows server 2008")) lProductsMicrosoft.Add("windows server 2008");

                if (!lProductsMicrosoft.Contains("windows 8.1")) lProductsMicrosoft.Add("windows 8.1");
                if (!lProductsMicrosoft.Contains("windows rt 8.1")) lProductsMicrosoft.Add("windows rt 8.1");   //8.1 rt?
                //if (!lProductsMicrosoft.Contains("windows 8")) lProductsMicrosoft.Add("windows 8");
                if (!lProductsMicrosoft.Contains("windows 7 sp1")) lProductsMicrosoft.Add("windows 7 sp1");
                if (!lProductsMicrosoft.Contains("windows 7")) lProductsMicrosoft.Add("windows 7");

                if (!lProductsMicrosoft.Contains("windows vista sp1")) lProductsMicrosoft.Add("windows vista sp1");
                if (!lProductsMicrosoft.Contains("windows vista sp2")) lProductsMicrosoft.Add("windows vista sp2");
                if (!lProductsMicrosoft.Contains("windows vista service pack 2")) lProductsMicrosoft.Add("windows vista service pack 2");
                if (!lProductsMicrosoft.Contains("windows vista")) lProductsMicrosoft.Add("windows vista");

                if (!lProductsMicrosoft.Contains("windows xp")) lProductsMicrosoft.Add("windows xp");   //Embedded

                if (!lProductsMicrosoft.Contains("windows 10 version 1511")) lProductsMicrosoft.Add("windows 10 version 1511");
                if (!lProductsMicrosoft.Contains("windows 10 version 1607")) lProductsMicrosoft.Add("windows 10 version 1607");
                if (!lProductsMicrosoft.Contains("windows 10 version 1703")) lProductsMicrosoft.Add("windows 10 version 1703");
                if (!lProductsMicrosoft.Contains("windows 10")) lProductsMicrosoft.Add("windows 10");

                if (!lProductsMicrosoft.Contains("hyper-v")) lProductsMicrosoft.Add("hyper-v");
                //pdf library
                //smb server
                //uniscribe
                if (!lProductsMicrosoft.Contains("dvd maker")) lProductsMicrosoft.Add("dvd maker");
                //directshow

                //office
                //Microsoft Office Compatibility Pack Service Pack 3
                if (!lProductsMicrosoft.Contains("excel 2007 sp3")) lProductsMicrosoft.Add("excel 2007 sp3");
                if (!lProductsMicrosoft.Contains("excel viewer sp3")) lProductsMicrosoft.Add("excel viewer sp3");
                if (!lProductsMicrosoft.Contains("excel viewer sp2")) lProductsMicrosoft.Add("excel viewer sp2");
                if (!lProductsMicrosoft.Contains("excel viewer sp1")) lProductsMicrosoft.Add("excel viewer sp1");
                if (!lProductsMicrosoft.Contains("excel viewer 2003")) lProductsMicrosoft.Add("excel viewer 2003");
                if (!lProductsMicrosoft.Contains("excel viewer")) lProductsMicrosoft.Add("excel viewer");
                if (!lProductsMicrosoft.Contains("word viewer")) lProductsMicrosoft.Add("word viewer");
                if (!lProductsMicrosoft.Contains("word 2016")) lProductsMicrosoft.Add("word 2016");

                if (!lProductsMicrosoft.Contains("outlook 2007")) lProductsMicrosoft.Add("outlook 2007");
                if (!lProductsMicrosoft.Contains("outlook 2010")) lProductsMicrosoft.Add("outlook 2010");
                if (!lProductsMicrosoft.Contains("outlook 2013")) lProductsMicrosoft.Add("outlook 2013");
                if (!lProductsMicrosoft.Contains("outlook 2016")) lProductsMicrosoft.Add("outlook 2016");

                if (!lProductsMicrosoft.Contains("onenote 2007")) lProductsMicrosoft.Add("onenote 2007");
                if (!lProductsMicrosoft.Contains("onenote 2010")) lProductsMicrosoft.Add("onenote 2010");

                if (!lProductsMicrosoft.Contains("office 2007")) lProductsMicrosoft.Add("office 2007");
                if (!lProductsMicrosoft.Contains("office 2010")) lProductsMicrosoft.Add("office 2010");
                if (!lProductsMicrosoft.Contains("office 2013")) lProductsMicrosoft.Add("office 2013"); //rt
                if (!lProductsMicrosoft.Contains("office 2016")) lProductsMicrosoft.Add("office 2016");
                //office online server

                if (!lProductsMicrosoft.Contains("exchange server 2010")) lProductsMicrosoft.Add("exchange server 2010");
                if (!lProductsMicrosoft.Contains("exchange server 2013")) lProductsMicrosoft.Add("exchange server 2013");
                if (!lProductsMicrosoft.Contains("exchange server 2016")) lProductsMicrosoft.Add("exchange server 2016");

                if (!lProductsMicrosoft.Contains("sharepoint server 2010")) lProductsMicrosoft.Add("sharepoint server 2010");
                if (!lProductsMicrosoft.Contains("sharepoint foundation 2013")) lProductsMicrosoft.Add("sharepoint foundation 2013");
                //            if (!lProductsMicrosoft.Contains("sharepoint enterprise server 2016")) lProductsMicrosoft.Add("sharepoint enterprise server 2016");

                if (!lProductsMicrosoft.Contains("sharepoint server 2016")) lProductsMicrosoft.Add("sharepoint server 2016");

                if (!lProductsMicrosoft.Contains("biztalk server")) lProductsMicrosoft.Add("biztalk server");

                if (!lProductsMicrosoft.Contains("office web apps 2010")) lProductsMicrosoft.Add("office web apps 2010");

                if (!lProductsMicrosoft.Contains("live meeting 2007 add-in")) lProductsMicrosoft.Add("live meeting 2007 add-in");

                if (!lProductsMicrosoft.Contains("lync 2010")) lProductsMicrosoft.Add("lync 2010"); //(admin level install)
                if (!lProductsMicrosoft.Contains("lync 2013 sp1")) lProductsMicrosoft.Add("lync 2013 sp1");
                if (!lProductsMicrosoft.Contains("skype for business 2016")) lProductsMicrosoft.Add("skype for business 2016");
                if (!lProductsMicrosoft.Contains("skype for business")) lProductsMicrosoft.Add("skype for business");
                if (!lProductsMicrosoft.Contains("skype")) lProductsMicrosoft.Add("skype");

                if (!lProductsMicrosoft.Contains("visio 2010")) lProductsMicrosoft.Add("visio 2010");
                if (!lProductsMicrosoft.Contains("visio 2013")) lProductsMicrosoft.Add("visio 2013");

                //microsoft browser
                if (!lProductsMicrosoft.Contains("internet explorer 9")) lProductsMicrosoft.Add("internet explorer 9");
                if (!lProductsMicrosoft.Contains("internet explorer 10")) lProductsMicrosoft.Add("internet explorer 10");
                if (!lProductsMicrosoft.Contains("internet explorer 11")) lProductsMicrosoft.Add("internet explorer 11");
                if (!lProductsMicrosoft.Contains("internet explorer")) lProductsMicrosoft.Add("internet explorer");
                if (!lProductsMicrosoft.Contains("microsoft edge")) lProductsMicrosoft.Add("microsoft edge");
                //if (!lProductsMicrosoft.Contains("edge")) lProductsMicrosoft.Add("edge");
                if (!lProductsMicrosoft.Contains("jscript")) lProductsMicrosoft.Add("jscript");
                if (!lProductsMicrosoft.Contains("vbscript")) lProductsMicrosoft.Add("vbscript");

                if (!lProductsMicrosoft.Contains(".net framework")) lProductsMicrosoft.Add(".net framework");

                if (!lProductsMicrosoft.Contains("adobe flash player")) lProductsMicrosoft.Add("adobe flash player");
                if (!lProductsMicrosoft.Contains("silverlight")) lProductsMicrosoft.Add("silverlight");
                #endregion hardcodedmicrosoftproducts
            }

            
            //Search the CVE in the XORCISM database
            //#region searchCVEinXORCISM
            VULNERABILITY oVulnerability = vuln_model.VULNERABILITY.FirstOrDefault(o => o.VULReferentialID == sCVE);    //Referential=="cve"
            if (oVulnerability != null)
            {
                //Console.WriteLine("DEBUG VulnerabilityID:" + oVulnerability.VulnerabilityID);
                Console.WriteLine("DEBUG VulnerabilityDescription: " + oVulnerability.VULDescription);
                string sVulnerabilityDescriptionLower = "";
                try
                {
                    sVulnerabilityDescriptionLower = oVulnerability.VULDescription.ToLower();
                    //if (sVulnerabilityDescriptionLower.Contains(" for mac")) return;
                }
                catch (Exception exVULDescription)
                {
                    sVulnerabilityDescriptionLower = "";
                }

                if (sFileNameToSearch == "")    //Not specified in arguments
                {
                    Console.WriteLine("DEBUG Launching_fSearchVulnerableFilename");
                    sFileNameToSearch = fSearchVulnerableFilename(sVulnerabilityDescriptionLower);   //Search a filename dll|exe|ocx|sys... in the CVE Description
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                }

                //If not specified by argument, nor found in the CVE description; try to identify what is the FileNameToSearch from the Vulnerability (CVE) Description
                #region getFileToSearchFromVulnerabilityDescription
                //if(sFileNameToSearch=="")   //Now use a list
                //{
                //HARDCODED
                //TODO: Use Statistical Analysis/AI with XORCISM (based on the OVAL Vulnerability Definitions of the OVALRepo)
                //TODO: See MS Glossary https://technet.microsoft.com/library/security/dn848375.aspx

                if (!lFileNamesToSearch.Contains("win32k.sys")) lFileNamesToSearch.Add("win32k.sys");       //Full/Desktop Multi-User Win32 Driver
                if (!lFileNamesToSearch.Contains("advapi32.dll")) lFileNamesToSearch.Add("advapi32.dll");   //MAPI
                if (!lFileNamesToSearch.Contains("gdi32.dll")) lFileNamesToSearch.Add("gdi32.dll");
                if (!lFileNamesToSearch.Contains("ntdll.dll")) lFileNamesToSearch.Add("ntdll.dll");         //NT Layer DLL
                if (!lFileNamesToSearch.Contains("shell32.dll")) lFileNamesToSearch.Add("shell32.dll");     //Windows Shell Common Dll      (Microsoft Malware Protection Engine Helper (shell32))
                //Windows Library Loading Remote Code Execution Vulnerability
                if (!lFileNamesToSearch.Contains("catsrvut.dll")) lFileNamesToSearch.Add("catsrvut.dll");   //COM+ Configuration Catalog Server Utilities
                if (!lFileNamesToSearch.Contains("comsvcs.dll")) lFileNamesToSearch.Add("comsvcs.dll");     //COM+ Services
                //Ntvdm64.dll
                //Wow64.dll
                //Wow32.dll
                //Winload.exe
                if (!lFileNamesToSearch.Contains("windowsbase.dll")) lFileNamesToSearch.Add("windowsbase.dll");
                if (!lFileNamesToSearch.Contains("user32.dll")) lFileNamesToSearch.Add("user32.dll");
                if (!lFileNamesToSearch.Contains("rpcrt4.dll")) lFileNamesToSearch.Add("rpcrt4.dll");   //RPC Local     local RPC

                if (sVulnerabilityDescriptionLower.Contains("flash") && !lFileNamesToSearch.Contains("flash.ocx")) lFileNamesToSearch.Add("flash.ocx"); //todo: regex? +pepper
                
                if (sVulnerabilityDescriptionLower.Contains("lsass")) sFileNameToSearch = "lsasrv.dll"; //Lsass.exe //Local Security Authority Subsystem Service
                if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                if (sVulnerabilityDescriptionLower.Contains("crypto driver")) sFileNameToSearch = "bcrypt.dll";
                if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                //cryptngc.dll
                if (sVulnerabilityDescriptionLower.Contains("dfs"))
                {
                    sFileNameToSearch = "dfscli.dll";   //dfsclient     Distributed File System Client DLL
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                    sFileNameToSearch = "dfsc.sys";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                }
                if (sVulnerabilityDescriptionLower.Contains("openssl")) sFileNameToSearch = "ssleay32.dll";
                if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                if (sVulnerabilityDescriptionLower.Contains("schannel")) sFileNameToSearch = "schannel.dll";
                if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                if (sVulnerabilityDescriptionLower.Contains("wordpad")) sFileNameToSearch = "wordpad.exe";  //msointl30.dll
                if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                if (sVulnerabilityDescriptionLower.Contains("profsvc")) sFileNameToSearch = "profsvc.dll";  //userenv.dll
                if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                if (sVulnerabilityDescriptionLower.Contains("network location awareness")) sFileNameToSearch = "nlasvc.dll";
                if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                if (sVulnerabilityDescriptionLower.Contains("group policy")) sFileNameToSearch = "gpsvc.dll";
                if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                if (sVulnerabilityDescriptionLower.Contains("webdav")) sFileNameToSearch = "mrxdav.sys";    //driver
                if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                if (sVulnerabilityDescriptionLower.Contains("dns server")) sFileNameToSearch = "dns.exe";
                if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                if (sVulnerabilityDescriptionLower.Contains("activex")) sFileNameToSearch = "msadcf.dll";
                if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                if (sVulnerabilityDescriptionLower.Contains("sharepoint") && !sVulnerabilityDescriptionLower.Contains("office")) sFileNameToSearch = "wsssetup.dll";
                if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);

                if (sVulnerabilityDescriptionLower.Contains("smbv1"))
                {
                    sFileNameToSearch = "srv2.sys";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                    sFileNameToSearch = "srv.sys";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                    //srvnet.sys  netevent.dll
                }
                if (sVulnerabilityDescriptionLower.Contains("telnet service")) sFileNameToSearch = "tlntsess.exe";
                if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                if (sVulnerabilityDescriptionLower.Contains("internet authentication service")) sFileNameToSearch = "iassam.dll";   //(wiassam.dll)
                if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                if (sVulnerabilityDescriptionLower.Contains("ts webproxy") || sVulnerabilityDescriptionLower.Contains("tswbprxy")) sFileNameToSearch = "tswbprxy.exe";
                if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                if (sVulnerabilityDescriptionLower.Contains("photo-decoder")) sFileNameToSearch = "wmphoto.dll";
                if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                if (sVulnerabilityDescriptionLower.Contains("color management module")) sFileNameToSearch = "icm32.dll";
                if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                //Remote Desktop Protocol (RDP)     rdpcorets.dll   Rdpudd.dll

                if (sVulnerabilityDescriptionLower.Contains("ole dll") && sVulnerabilityDescriptionLower.Contains("office")) sFileNameToSearch = "mso.dll";
                //if (sVulnerabilityDescriptionLower.Contains("office")) sFileNameToSearch = "mso.dll";
                if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                //sharepoint    sword.dll
                //Office Web Apps   msoserver.dll
                //Word Viewer   Wordview.exe
                if (sFileNameToSearch == "" && sVulnerabilityDescriptionLower.Contains("office")) sFileNameToSearch = "riched20.dll";

                if (sFileNameToSearch == "" && sVulnerabilityDescriptionLower.Contains("sharepoint")) sFileNameToSearch = "wsssetup.dll";
                if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);

                if (sVulnerabilityDescriptionLower.Contains("windows installer")) sFileNameToSearch = "msi.dll";
                if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                if (sVulnerabilityDescriptionLower.Contains("windows com")) sFileNameToSearch = "rpcss.dll";    //Marshaler
                if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                if (sVulnerabilityDescriptionLower.Contains("kernel"))
                {
                    sFileNameToSearch = "dxgkrnl.sys";  //kernel-mode driver
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                    sFileNameToSearch = "tcpip.sys";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                    sFileNameToSearch = "afd.sys";  //winsock
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);

                    sFileNameToSearch = "kernelbase.dll";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                    sFileNameToSearch = "kernel32.dll";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);

                    sFileNameToSearch = "ksecdd.sys";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                    sFileNameToSearch = "cng.sys";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                    sFileNameToSearch = "dfsc.sys";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                    sFileNameToSearch = "win32k.sys";//Windows kernel-mode driver    //TODO: change that for Regex?
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                    sFileNameToSearch = "win32kbase.sys";//Windows kernel-mode driver    //TODO: change that for Regex?
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                    sFileNameToSearch = "win32kfull.sys";//Windows kernel-mode driver    //TODO: change that for Regex?
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                    //Vista
                    sFileNameToSearch = "ntoskrnl.exe";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                }
                if (sVulnerabilityDescriptionLower.Contains("hyper-v"))
                {
                    sFileNameToSearch = "ntoskrnl.exe";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                    sFileNameToSearch = "storvsp.sys";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                    sFileNameToSearch = "isoparser.sys";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);

                    sFileNameToSearch = "vmicrdv.dll";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                    //Vmicvss.dll
                    //Vmicshutdown.dll
                    //Vmictimesync.dll
                    //Vmicheartbeat.dll
                    //Vmickvpexchange.dll
                    //passthruparser.sys
                    //vhdparser.sys
                }

                //Graphics Device Interface (aka GDI or GDI+)
                //The GDI component
                if (sVulnerabilityDescriptionLower.Contains(" gdi ") || sVulnerabilityDescriptionLower.Contains("graphics") || sVulnerabilityDescriptionLower.Contains("truetype") || sVulnerabilityDescriptionLower.Contains("uniscribe"))  //graphics component     Graphics Device Interface (GDI)
                {
                    ////if (sVulnerabilityDescriptionLower.Contains("graphics component")) 
                    //    sFileNameToSearch = "presentationfontcache.exe.config";//Windows font library    .Net Framework
                    //if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);

                    sFileNameToSearch = "usp10.dll";//
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);

                    //D2d1.dll
                    //Fntcache.dll
                    //Dwrite.dll

                    sFileNameToSearch = "gdi32full.dll";//
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);

                    sFileNameToSearch = "gdi32.dll";//
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);

                    //if (sVulnerabilityDescriptionLower.Contains("graphics component")) 
                    sFileNameToSearch = "ogl.dll";//
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);

                    
                    if (!sVulnerabilityDescriptionLower.Contains("true type"))
                    {
                        sFileNameToSearch = "win32k.sys";//Review ?
                        if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                    }
                    //Fntcache.dll
                    //if (sVulnerabilityDescriptionLower.Contains("graphics component")) 
                    sFileNameToSearch = "gdiplus.dll";//True Type font
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);

                }

                if (sVulnerabilityDescriptionLower.Contains("dvd maker")) sFileNameToSearch = "dvdmaker.exe";
                if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);

                if (sVulnerabilityDescriptionLower.Contains("win32k")) sFileNameToSearch = "win32k.sys";
                if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                if (sVulnerabilityDescriptionLower.Contains("directx")) sFileNameToSearch = "win32k.sys";
                if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                if (sVulnerabilityDescriptionLower.Contains("directx")) sFileNameToSearch = "dxgkrnl.sys";
                if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                //d3d12.dll
                if (sVulnerabilityDescriptionLower.Contains("common log file system driver")) sFileNameToSearch = "clfs.sys";
                if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                if (sVulnerabilityDescriptionLower.Contains("print spooler")) sFileNameToSearch = "localspl.dll";   //Winprint.dll  Win32spl.dll ... i.e. https://support.microsoft.com/en-us/help/3170455/ms16-087-description-of-the-security-update-for-windows-print-spooler-components-july-12,-2016
                if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                if (sVulnerabilityDescriptionLower.Contains("wpad")) sFileNameToSearch = "ws2_32.dll"; //Winhttp.dll The Web Proxy Auto Discovery (WPAD) protocol
                if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                if (sVulnerabilityDescriptionLower.Contains("windows error reporting")) sFileNameToSearch = "wer.dll";
                if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                if (sVulnerabilityDescriptionLower.Contains("netlogon")) sFileNameToSearch = "netlogon.dll";    //(wnetlogon.dll)
                if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                if (sVulnerabilityDescriptionLower.Contains("mount manager")) sFileNameToSearch = "mountmgr.sys";
                if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                //Emdmgmt.dll
                if (sVulnerabilityDescriptionLower.Contains("msxml")) sFileNameToSearch = "msxml3.dll";
                if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);

                //SMB Server    iSNS Server
                if (sVulnerabilityDescriptionLower.Contains("smb server") || sVulnerabilityDescriptionLower.Contains("isns server")) sFileNameToSearch = "imjpmig.dll";
                if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                if (sVulnerabilityDescriptionLower.Contains("smb server") || sVulnerabilityDescriptionLower.Contains("isns server")) sFileNameToSearch = "imjppdmg.exe";
                if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                
                if (sVulnerabilityDescriptionLower.Contains("chakra javascript engine")) sFileNameToSearch = "chakra.dll";
                if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                if (sVulnerabilityDescriptionLower.Contains("windows diagnostics hub")) sFileNameToSearch = "diagnosticshub.standardcollector.runtime.dll";    //Standard Collector Service Diagnosticshub.standardcollector.runtime.dll
                if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                //Kerberos.dll
                //Netlogon.dll
                //Winlogon.exe
                //Gdiplus.dll

                if (sVulnerabilityDescriptionLower.Contains("png parsing")) sFileNameToSearch = "gdiplus.dll";
                if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                if (sVulnerabilityDescriptionLower.Contains("png parsing")) sFileNameToSearch = "windowscodecs.dll";
                if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                if (sVulnerabilityDescriptionLower.Contains("windows text services")) sFileNameToSearch = "msctf.dll";  //WTS   wmsctf.dll
                if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);

                if (sVulnerabilityDescriptionLower.Contains("task scheduler")) sFileNameToSearch = "ubpm.dll";
                if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                
                //OpenType Font     OpenFont    Windows GDI, Adobe Reader, Microsoft DirectWrite library, and Windows Presentation Foundation
                if (sVulnerabilityDescriptionLower.Contains("adobe type manager library")) sFileNameToSearch = "atmfd.dll"; //atmlib.dll
                if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);

                if (sVulnerabilityDescriptionLower.Contains("adobe flash player")) sFileNameToSearch = "flash.ocx";
                //^[Ff][Ll][Aa][Ss][Hh].*\.[Oo][Cc][Xx]$
                if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                //swflash.ocx
                //TODO: Google Chrome + Pepper Flash    pepflashplayer.dll 

                if (sVulnerabilityDescriptionLower.Contains("silverlight"))
                {
                    sFileNameToSearch = "mscorlib.dll";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);

                    sFileNameToSearch = "silverlight_developer.exe";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                    sFileNameToSearch = "silverlight_developer_x64.exe";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                    sFileNameToSearch = "silverlight_x64.exe";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                    sFileNameToSearch = "silverlight.exe";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                }

                if (sVulnerabilityDescriptionLower.Contains(".net framework"))  //truetype
                {
                    sFileNameToSearch = "mscorlib.dll";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                    //MsCoRWks.dll
                    sFileNameToSearch = "system.componentmodel.dataannotations.dll";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                    sFileNameToSearch = "mscormmc.dll";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                    sFileNameToSearch = "system.deployment.resources.dll";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                    sFileNameToSearch = "dfdll.dll";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                    sFileNameToSearch = "system.deployment.dll";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                    sFileNameToSearch = "windowsbase.dll";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                    sFileNameToSearch = "presentationframework.dll";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                    sFileNameToSearch = "presentationcore.dll";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                    //reachframework.dll
                    //...
                }

                if (sVulnerabilityDescriptionLower.Contains("internet explorer") || sVulnerabilityDescriptionLower.Contains("microsoft browser"))
                {
                    sFileNameToSearch = "iexplore.exe";   //htmlrendering
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                    //jscript
                    //vbscript
                    sFileNameToSearch = "ieuser.exe";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                    sFileNameToSearch = "urlmon.dll";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                    sFileNameToSearch = "ieframe.dll";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                    sFileNameToSearch = "wininet.dll";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                    sFileNameToSearch = "jsproxy.dll";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);

                    sFileNameToSearch = "edgehtml.dll";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);

                    //Internet Messaging API
                    sFileNameToSearch = "inetres.dll";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                    sFileNameToSearch = "inetcomm.dll";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                    
                    sFileNameToSearch = "mshtml.dll";   //htmlrendering
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                }

                if (sVulnerabilityDescriptionLower.Contains("scripting engine") || sVulnerabilityDescriptionLower.Contains("microsoft edge"))   //Chakra JavaScript engine in Microsoft Edge
                {
                    //bootux.dll
                    sFileNameToSearch = "cortanaapi.dll";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                    //rascmak   cmak.exe    (dism.exe)
                    sFileNameToSearch = "shell32.dll";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                    sFileNameToSearch = "ieframe.dll";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                    sFileNameToSearch = "edgehtml.dll";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                }

                if (sVulnerabilityDescriptionLower.Contains("tcp/ip") || sVulnerabilityDescriptionLower.Contains("windows filtering platform") || sVulnerabilityDescriptionLower.Contains("wfp") || sVulnerabilityDescriptionLower.Contains("icmp") || sVulnerabilityDescriptionLower.Contains("dhcp") || sVulnerabilityDescriptionLower.Contains("ipsec") || sVulnerabilityDescriptionLower.Contains("igmp") || sVulnerabilityDescriptionLower.Contains("sack") || sVulnerabilityDescriptionLower.Contains("ipv6") || sVulnerabilityDescriptionLower.Contains("arp"))
                {
                    sFileNameToSearch = "tcpip.sys";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                }
                if (sVulnerabilityDescriptionLower.Contains("xadm") || sVulnerabilityDescriptionLower.Contains("active directory") || sVulnerabilityDescriptionLower.Contains("adc")) //connector   xgen    exchange 2000
                {
                    sFileNameToSearch = "ntdsa.dll";    //Windows Directory System Agent (DSA)
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                    sFileNameToSearch = "adc.exe";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                }
                if (sVulnerabilityDescriptionLower.Contains("atapi") || sVulnerabilityDescriptionLower.Contains("ata channel") || sVulnerabilityDescriptionLower.Contains("sata") || sVulnerabilityDescriptionLower.Contains("oobe") || sVulnerabilityDescriptionLower.Contains("ide/ata")) //mmc   pata
                {
                    sFileNameToSearch = "intelide.sys"; //Intel PCI IDE Driver
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                    sFileNameToSearch = "atapi.sys";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                }
                if (sVulnerabilityDescriptionLower.Contains("odbc"))
                {
                    sFileNameToSearch = "odbcbcp.dll";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                    sFileNameToSearch = "odbc32.dll";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                }
                if (sVulnerabilityDescriptionLower.Contains("sql server"))
                {
                    sFileNameToSearch = "msgprox.dll";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                    sFileNameToSearch = "logread.exe";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                    sFileNameToSearch = "bcp.exe";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                    sFileNameToSearch = "sqlaccess.dll";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                    //sqldmo.dll
                    //sqlmaint.exe
                    //Sqlncli.dll
                    //Sqlncli10.dll
                    //Sqloledb.dll  //mdac  odbc
                    //Microsoft.AnalysisServices.AdomdClient.dll        //Microsoft Analysis Services ADOMD.Net interfaces
                    //Msadomdx.dll      //Microsoft XML for Analysis SDK Service Component
                    //Msasxpress.dll    //Microsoft SQL Server Analysis Services Compression Library
                    //Msmdlocal.dll     //Microsoft SQL Server Analysis Services
                    //Msmdpump.dll      //Microsoft SQL Server Analysis Services IIS Pump
                    //Msmdredir.dll     //Microsoft Analysis Services SQL Browser redirector
                    //Msmdsrv.exe       //Microsoft SQL Server Analysis Services
                    //Msmgdsrv.dll      //Microsoft SQL Server Analysis Services Managed Module
                    //Msolap90.dll      //Microsoft OLE DB Provider for Analysis Services
                    //Mssdi98.dll       //Microsoft SQL Server Debug Interface
                    //MsSearch.exe      //Microsoft PKM Search Service
                    //Mssqlsystemresource.ldf   //SQL Server Resource Database (transaction log file)
                    //Mssqlsystemresource.mdf   //SQL Server Resource Database (data file)
                    //Ntwdblib.dll      //Microsoft SQL Server DbLib Client Library
                    //Odsole70.dll      //OleAut driver DLL containing SQL Server sp_OA extended stored procedures
                    //osql.exe          //SQL Query Tool
                    sFileNameToSearch = "sqldiag.exe";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                    sFileNameToSearch = "sqlagent.exe"; //sqlagent90.exe
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                    sFileNameToSearch = "sqlsrv32.dll";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                    sFileNameToSearch = "sqlservr.exe";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                }
                if (sVulnerabilityDescriptionLower.Contains("com+"))
                {
                    sFileNameToSearch = "es.dll";   //event system
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                    sFileNameToSearch = "ole32.dll";    //Microsoft OLE for Windows
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                    sFileNameToSearch = "catsrv.dll";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                }
                if (sVulnerabilityDescriptionLower.Contains("mapi") || sVulnerabilityDescriptionLower.Contains("nav 2009"))
                {
                    sFileNameToSearch = "cdo.dll";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                }
                //dwrite.dll    directwrite
                if (sVulnerabilityDescriptionLower.Contains("dynamics crm"))
                {
                    sFileNameToSearch = "mscrmcustom.dll";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                    sFileNameToSearch = "crmhotfix.cdf";    //!!!
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                    sFileNameToSearch = "crmmsg.dll";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                }
                if (sVulnerabilityDescriptionLower.Contains("dns"))
                {
                    sFileNameToSearch = "dns.exe";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                }
                if (sVulnerabilityDescriptionLower.Contains("event system"))
                {
                    sFileNameToSearch = "es.dll";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                }
                if (sVulnerabilityDescriptionLower.Contains("volume shadow copy") || sVulnerabilityDescriptionLower.Contains("vss"))
                {
                    sFileNameToSearch = "eventcls.dll";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                    sFileNameToSearch = "ftdisk.sys";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                }
                if (sVulnerabilityDescriptionLower.Contains("event log") || sVulnerabilityDescriptionLower.Contains("system log") || sVulnerabilityDescriptionLower.Contains("security log"))
                {
                    sFileNameToSearch = "eventlog.dll";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                }
                //fntcache.dll      fontcache
                //hal.dll   Windows Hardware Abstraction Layer (HAL) DLL
                //http.sys
                if (sVulnerabilityDescriptionLower.Contains("infopath"))
                {
                    sFileNameToSearch = "infopath.exe";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                }
                if (sVulnerabilityDescriptionLower.Contains("scsi"))
                {
                    sFileNameToSearch = "iscsilog.dll";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                    sFileNameToSearch = "msiscsi.sys";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                    sFileNameToSearch = "iscsicpl.exe";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                }
                if (sVulnerabilityDescriptionLower.Contains("kerberos") || sVulnerabilityDescriptionLower.Contains("ticket") || sVulnerabilityDescriptionLower.Contains("directaccess"))
                {
                    sFileNameToSearch = "kerberos.dll";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                }
                //kernel32.dll functions
                if (sVulnerabilityDescriptionLower.Contains("print"))
                {
                    sFileNameToSearch = "localspl.dll";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                    //spoolsv.exe
                    sFileNameToSearch = "spoolss.dll";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                }
                if (sVulnerabilityDescriptionLower.Contains("netlogon"))
                {
                    sFileNameToSearch = "netlogon.dll";   //Windows Net Logon Services
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                }
                if (sVulnerabilityDescriptionLower.Contains("lsass") || sVulnerabilityDescriptionLower.Contains("dtls") || sVulnerabilityDescriptionLower.Contains("oscp")) //event id  //branchcache
                {
                    //Msv1_0.dll    //Microsoft Authentication Package v1.0
                    sFileNameToSearch = "lsass.exe";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                    sFileNameToSearch = "lsasrv.dll";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                }
                
                if (sVulnerabilityDescriptionLower.Contains("data access") || sVulnerabilityDescriptionLower.Contains("ado"))
                {
                    sFileNameToSearch = "msado15.dll";  //Microsoft Data Access - ActiveX Data Objects (ADO)
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                }
                if (sVulnerabilityDescriptionLower.Contains("outlook"))
                {
                    sFileNameToSearch = "mspst32.dll";  //MAPI  //Microsoft Personal Folder / Address Book Service Provider
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                    sFileNameToSearch = "msmapi32.dll";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                    sFileNameToSearch = "outilib.dll";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                    sFileNameToSearch = "outlook.exe";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                }
                if (sVulnerabilityDescriptionLower.Contains("exchange"))
                {
                    sFileNameToSearch = "mdbmsg.dll";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                    sFileNameToSearch = "microsoft.exchange.clients.owa2.server.dll";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                }
                if (sVulnerabilityDescriptionLower.Contains("owa")) //Outlook Web Access
                {
                    sFileNameToSearch = "owaauth.dll";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                }
                if (sVulnerabilityDescriptionLower.Contains("msdtc") || sVulnerabilityDescriptionLower.Contains("ms dtc"))
                {
                    sFileNameToSearch = "mtxclu.dll";   //DTC (Distributed Transaction Coordinator) and MTS (Microsoft Transaction Server ) Clustering Support
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                }
                if (sVulnerabilityDescriptionLower.Contains("partition manager") || sVulnerabilityDescriptionLower.Contains("fusion-io"))   //driver
                {
                    sFileNameToSearch = "partmgr.sys";   //Windows Partition Manager
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                }

                if (sVulnerabilityDescriptionLower.Contains("xbox"))
                {
                    sFileNameToSearch = "xblauthmanager.dll";
                    if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                    //windows.gaming.xboxlive.storage.dll
                }
                //}


                foreach(string sExcludedFilename in lFileNamesNOTToSearch)
                {
                    lFileNamesToSearch.Remove(sExcludedFilename);
                }

                #endregion getFileToSearchFromVulnerabilityDescription

                //=================================================================================================================================================================================
                if (sFileNameToSearch != "") Console.WriteLine("DEBUG sFileNameToSearch retrieved from CVE description: " + sFileNameToSearch);


                //Check if there is already at least one (Vulnerability) OVALDEFINITION for the VULNERABILITY (CVE) in the XORCISM database (/OVAL repo)
                int iOVALDefVulnID = 0;
                //OVALDEFINITIONVULNERABILITY oOVALDefForVuln = oval_model.OVALDEFINITIONVULNERABILITY.FirstOrDefault(o => o.VulnerabilityID == oVulnerability.VulnerabilityID);
                try
                {
                    iOVALDefVulnID = oval_model.OVALDEFINITIONVULNERABILITY.FirstOrDefault(o => o.VulnerabilityID == oVulnerability.VulnerabilityID).OVALDefinitionVulnerabilityID;
                }
                catch(Exception ex)
                {

                }
                //if (oOVALDefForVuln != null)
                if(iOVALDefVulnID > 0)
                {
                    Console.WriteLine("DEBUG OVALDEFINITIONVULNERABILITY already exists");//: " + oOVALDefForVuln.OVALDEFINITION.OVALDefinitionIDPattern);
                    //CVE-2016-7289     oval:org.cisecurity:def:1639
                    try
                    {
                        //Update VULNERABILITY
                        oVulnerability.hasOVALDefinition = true;
                        oVulnerability.isMicrosoft = true;
                        oVulnerability.timestamp = DateTimeOffset.Now;
                        vuln_model.Entry(oVulnerability).State = EntityState.Modified;
                        vuln_model.SaveChanges();
                    }
                    catch(Exception exUpdateVulnerabilityHasOVALDef)
                    {
                        Console.WriteLine("Exception: exUpdateVulnerabilityHasOVALDef " + exUpdateVulnerabilityHasOVALDef.Message + " " + exUpdateVulnerabilityHasOVALDef.InnerException);
                    }
                    return;   //If you want to stop here
                }
                else
                {
                    Console.WriteLine("DEBUG OVALDEFINITIONVULNERABILITY UNKNOWN");
                }

                bool bMacCVE = false;
                if (sVulnerabilityDescriptionLower.Contains("for mac")) bMacCVE = true; //Hardcoded We don't deal with these ones
                int iCptCPENotMac = 0;

                //Analysis of the CPEs from the CVE
                #region cpesanalysis
                foreach (VULNERABILITYFORCPE oVulnCPE in oVulnerability.VULNERABILITYFORCPE)    //.OrderByDescending())
                {
                    //Console.WriteLine("DEBUG Vulnerability CPEID:" + oVulnCPE.CPEID);
                    CPE oCPE = model.CPE.FirstOrDefault(o => o.CPEID == oVulnCPE.CPEID);
                    if (bDebugCPE)
                    {
                        Console.WriteLine("DEBUG ======================================================================================================");
                        Console.WriteLine("DEBUG Vulnerability CPEName:" + oCPE.CPEName);
                    }
                    if (oCPE.CPEName.Contains("redhat")) continue;  //Hardcoded Happens for Adobe Flash Player
                    if (oVulnerability.VULNERABILITYFORCPE.Count() == 1 && oCPE.CPEName.Contains("for_mac")) continue;  //Hardcoded i.e. CVE-2016-7300

                    //We search for the CPEs (from the CPE database) matching our "CPE" (pattern from the CVE)
                    List<CPE> CPEs = model.CPE.Where(o => o.CPEName.StartsWith(oCPE.CPEName) && o.deprecated != true).OrderByDescending(o => o.CPEName).ToList();
                    //For   cpe:/o:microsoft:windows_7:-:sp1    =>
                    //cpe:/o:microsoft:windows_7:-:sp1:x32  Microsoft Windows 7 32-bit Service Pack 1
                    //cpe:/o:microsoft:windows_7:-:sp1:x86  Microsoft Windows 7 x86 Service Pack 1
                    //cpe:/o:microsoft:windows_7:-:sp1:x64  Microsoft Windows 7 64-bit Service Pack 1 (initial release)

                    //NOTE: The OVALDefinition.title/comment does not match the CPETitle

                    //string sCPETitle = "";
                    foreach (CPE oCPEMatch in CPEs)
                    {
                        #region loopcpeslist
                        if (bDebugCPE)
                        {
                            Console.WriteLine("DEBUG -----------------------------------------------------------------------------------------");
                            Console.WriteLine("DEBUG CPEMatch CPEName=" + oCPEMatch.CPEName);       //cpe:/o:microsoft:windows_7:-:sp1:x32
                            //Console.WriteLine("DEBUG CPEMatch CPETitle=" + sCPETitle);          //Microsoft Windows 7 32-bit Service Pack 1
                        }
                        string[] CPESplit = oCPEMatch.CPEName.Split(':');
                        //Build a ProductName compliant with Microsoft KB pages naming "convention"
                        string sProductToAdd = CPESplit[3].Replace("_", " ");       //Hardcoded windows_7 => windows 7
                        if (sProductToAdd.Contains(" for mac") || oCPEMatch.CPEName.EndsWith(":mac"))
                        {
                            continue;           //Hardcoded //We don't want it
                        }
                        else
                        {
                            iCptCPENotMac++;
                        }

                        try
                        {
                            string sProductToAddVersion = "";
                            try
                            {
                                sProductToAddVersion = CPESplit[4].Replace("_", " ");  //i.e. 2016
                            }
                            catch (Exception exCPEVersion)
                            {
                                //No Version in the CPEName
                            }
                            if (!sProductToAdd.Contains(" ") || sProductToAddVersion.StartsWith("20"))//hardcoded
                            {
                                //Just one word is not enough
                                //i.e.: office
                                if (sProductToAddVersion != "" && sProductToAddVersion != "-")//hardcoded
                                {
                                    sProductToAdd += " " + sProductToAddVersion;
                                }
                                else
                                {
                                    //i.e.  edge
                                    sProductToAdd = CPESplit[2] + " " + sProductToAdd;  //microsoft
                                }
                            }
                        }
                        catch (Exception exProductToAddVersion)
                        {

                        }
                        if (bDebugCPE) Console.WriteLine("DEBUG sProductToAddCPE=" + sProductToAdd);
                        if (!lProducts.Contains(sProductToAdd)) lProducts.Add(sProductToAdd);    //Hardcoded    (cpe always ToLower())
                        if (!lProductsMicrosoft.Contains(sProductToAdd)) lProductsMicrosoft.Add(sProductToAdd); //We add the basic product name

                        if (sProductToAdd == "windows 2003 server") //Thanks CPE...
                        {
                            sProductToAdd = "windows server 2003";
                            if (!lProducts.Contains(sProductToAdd)) lProducts.Add(sProductToAdd);    //Hardcoded    (cpe always ToLower())
                            if (!lProductsMicrosoft.Contains(sProductToAdd)) lProductsMicrosoft.Add(sProductToAdd); //We add the basic product name

                        }

                        //Try to Identify the MainProducts from Here
                        if (!sProductToAdd.StartsWith("windows"))
                        {
                            //JJJ
                            ////if (!lMainProductsNames.Contains("windows")) lMainProductsNames.Add("windows");


                            //lync 2013 sp1
                            string sMainProductNameFromCPE = string.Empty;
                            if (sProductToAdd.Contains("flash player")) //Hardcoded
                            {
                                sMainProductNameFromCPE = sProductToAdd;
                            }
                            else
                            {
                                sMainProductNameFromCPE = "microsoft " + fRemoveYear(sProductToAdd);    //HARDCODED
                                sMainProductNameFromCPE = sMainProductNameFromCPE.Replace("microsoft microsoft ", "microsoft ");
                                for (int iSP = 1; iSP < 6; iSP++)   //Hardcoded
                                {
                                    sMainProductNameFromCPE = sMainProductNameFromCPE.Replace(" sp" + iSP.ToString(), "");
                                }
                            }

                            if (!lMainProductsNames.Contains(sMainProductNameFromCPE.ToLower())) lMainProductsNames.Add(sMainProductNameFromCPE.ToLower());

                        }

                        //HARDCODED We will often find "Office" in the Microsoft webpages' Titles
                        if (sProductToAdd.Contains("excel")) lProductsMicrosoft.Add(sProductToAdd.Replace("excel", "office")); //This for the parsing of the KB pages
                        if (sProductToAdd.Contains("word ")) lProductsMicrosoft.Add(sProductToAdd.Replace("word ", "office ")); //This for the parsing of the KB pages
                        if (sProductToAdd.Contains("publisher")) lProductsMicrosoft.Add(sProductToAdd.Replace("publisher", "office ")); //This for the parsing of the KB pages
                        //access
                        //powerpoint
                        //visio
                        //...

                        string sProductMicrosoftToAdd = sProductToAdd;
                        //To be "Microsoft KB compliant"
                        if (oCPEMatch.CPEName.Contains("1511")) sProductMicrosoftToAdd += " version 1511"; //Hardcoded
                        if (oCPEMatch.CPEName.Contains("1607")) sProductMicrosoftToAdd += " version 1607"; //Hardcoded
                        if (oCPEMatch.CPEName.Contains("1703")) sProductMicrosoftToAdd += " version 1703"; //Hardcoded
                        if (oCPEMatch.CPEName.Contains("r2")) sProductMicrosoftToAdd += " r2"; //Hardcoded
                        //Windows RT 8.1
                        if (oCPEMatch.CPEName.Contains("sp1")) sProductMicrosoftToAdd += " sp1"; //Hardcoded
                        if (oCPEMatch.CPEName.Contains("sp2")) sProductMicrosoftToAdd += " sp2"; //Hardcoded
                        if (oCPEMatch.CPEName.Contains("sp3")) sProductMicrosoftToAdd += " sp3"; //Hardcoded
                        if (oCPEMatch.CPEName.Contains("sp4")) sProductMicrosoftToAdd += " sp4"; //Hardcoded
                        if (oCPEMatch.CPEName.Contains("sp5")) sProductMicrosoftToAdd += " sp5"; //Hardcoded
                        if (oCPEMatch.CPEName.Contains("sp6")) sProductMicrosoftToAdd += " sp6"; //Hardcoded
                        //TODO: Enhance that
                        if (bDebugCPE) Console.WriteLine("DEBUG sProductMicrosoftToAdd=" + sProductMicrosoftToAdd);
                        if (!lProductsMicrosoft.Contains(sProductMicrosoftToAdd)) lProductsMicrosoft.Add(sProductMicrosoftToAdd);    //Hardcoded
                        if (!lProductsCPEs.Contains(sProductMicrosoftToAdd)) lProductsCPEs.Add(sProductMicrosoftToAdd);    //Hardcoded
                        #endregion loopcpeslist
                    }
                }
                #endregion cpesanalysis


                if (bUseOnlyCPEs && bMacCVE && iCptCPENotMac == 0)
                {
                    Console.WriteLine("DEBUG RETURN1");
                    return;  //e.g. CVE-2015-6123
                }

                //***************************************************************************************************************************************
                //Load the Microsoft Products (known as Platforms in OVAL, i.e.: Windows Server 2012)
                List<string> lPlatforms = oval_model.PLATFORM.Where(o => o.PlatformName.StartsWith("Microsoft")).Select(o => o.PlatformName).ToList();//hardcoded
                foreach (string sPlatformName in lPlatforms)
                {
                    if (!lProductsMicrosoft.Contains(sPlatformName.ToLower())) lProductsMicrosoft.Add(sPlatformName.ToLower());
                }

                //Console.WriteLine("DEBUG LOADING lKnownMicrosoftProducts " + DateTimeOffset.Now);
                List<string> lKnownMicrosoftProducts = oval_model.PRODUCT.Where(o => o.ProductName.ToLower().Contains("microsoft") || o.ProductName.ToLower().Contains("windows")).Select(o => o.ProductName).ToList();//hardcoded
                foreach (string sMicrosoftProductName in lKnownMicrosoftProducts)
                {
                    if (!lProductsMicrosoft.Contains(sMicrosoftProductName.ToLower())) lProductsMicrosoft.Add(sMicrosoftProductName.ToLower());
                }
                //Console.WriteLine("DEBUG LOADED lKnownMicrosoftProducts " + DateTimeOffset.Now);


                List<string> lVulnReferencesVisited = new List<string>();

                //Get information from the VULNERABILITYREFERENCEs
                IEnumerable<VULNERABILITYFORREFERENCE> VulnReferences = vuln_model.VULNERABILITYFORREFERENCE.Where(o => o.VulnerabilityID == oVulnerability.VulnerabilityID);
                foreach (VULNERABILITYFORREFERENCE oVulnRef in VulnReferences.ToList())
                {
                    try
                    {
                        REFERENCE oReference = model.REFERENCE.FirstOrDefault(o => o.ReferenceID == oVulnRef.ReferenceID);
                        if (!lVulnReferencesVisited.Contains(oReference.ReferenceURL))
                        {
                            if (oReference.ReferenceURL.ToLower().Contains("technet.microsoft.com/security/bulletin/ms") || oReference.ReferenceURL.ToLower().Contains("security-guidance/advisory"))    //HARDCODEDMS
                            {
                                Console.WriteLine("DEBUG VULNERABILITYFORREFERENCE " + oReference.ReferenceURL);
                                lVulnReferencesVisited.Add(oReference.ReferenceURL);

                                //http://technet.microsoft.com/security/bulletin/MS16-130
                                //https://portal.msrc.microsoft.com/en-US/security-guidance/advisory/CVE-2017-0194
                                Console.WriteLine("DEBUG +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
                                //Console.WriteLine("DEBUG ReferenceID=" + oReference.ReferenceID);
                                //Force English
                                string sURLMS = oReference.ReferenceURL.Replace("security/bulletin/", "en-us/library/security/").Replace("http:", "https:");   //HARDCODEDMS
                                if (sURLMS.EndsWith("MS16-148")) continue;  //for Mac   //Hardcoded

                                //https://technet.microsoft.com/en-us/library/security/ms16-130(.aspx)
                                Console.WriteLine("DEBUG ReferenceModified: " + sURLMS);//oReference.ReferenceURL);
                                //We need to retrieve the KBID
                                #region getKBfomMSpage
                                //TODO IMPORTANT: REVIEW if Better way to obtain this that from Microsoft website scraping? (i.e. API)

                                string ResponseText = "";
                                //string MyCookie = "";

                                try
                                {
                                    if (oReference.ReferenceSourceID != null) sMSID = oReference.ReferenceSourceID;
                                }
                                catch (Exception exMSID)
                                {
                                    Console.WriteLine("NOTE: oReference.ReferenceSourceID is NULL/empty");  //Note: could be in ReferenceTitle
                                    sMSID = ""; //not null
                                }

                                if (sMSID.Trim() == "")
                                {
                                    //Retrieve it from the URL
                                    Regex RegexMSPatch = new Regex("MS[0-9][0-9]-[0-9][0-9][0-9]", RegexOptions.IgnoreCase);
                                    string strTemp = RegexMSPatch.Match(sURLMS).ToString();
                                    if (strTemp != "")
                                    {
                                        sMSID = strTemp.Trim().ToUpper();
                                    }
                                    else
                                    {
                                        //Console.WriteLine("NOTE: MSID not retrieved");
                                        sMSID = "temp"; //Hardcoded
                                    }

                                }
                                //"A programmer is just a tool which converts caffeine into code" CLIP- Stellvertreter
                                //Console.WriteLine("DEBUG sMSID=" + sMSID);
                                string sMSFilePath = sCurrentPath + @"\MS\" + sMSID + ".txt";    //HARDCODED Local Path to save the MS- page
                                FileInfo fileInfo = new FileInfo(sMSFilePath);
                                try
                                {
                                    //Commented out so we get any update
                                    //if (!fileInfo.Exists)   //We never visited/saved this MS- page
                                    //{
                                    /*
                                    //Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                    Console.WriteLine("DEBUG Request to " + oReference.ReferenceURL);

                                    StreamReader SR = null;
                                    HttpWebResponse response = null;

                                    HttpWebRequest request;
                                    request = (HttpWebRequest)HttpWebRequest.Create(oReference.ReferenceURL);
                                    request.Method = "GET";

                                    response = (HttpWebResponse)request.GetResponse();
                                    SR = new StreamReader(response.GetResponseStream());
                                    ResponseText = SR.ReadToEnd();

                                    SR.Close();
                                    response.Close();

                                    System.IO.File.WriteAllText(sMSFilePath, ResponseText);   ////Hardcoded Save the MS- page locally
                                    */
                                    //DesiredCapabilities handlSSLErr = DesiredCapabilities.Chrome();
                                    //handlSSLErr.SetCapability(CapabilityType.AcceptSslCertificates, true);

                                    ChromeOptions options = new ChromeOptions();
                                    options.AddArgument("test-type");
                                    using (var driver = new ChromeDriver(options))
                                    //using (var driver = new RemoteWebDriver())
                                    {
                                        //driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
                                        driver.Navigate().GoToUrl(sURLMS);
                                        Thread.Sleep(1000 + iSleepMore); //Hardcoded

                                        try
                                        {
                                            if (driver.PageSource.Contains("Please read and acknowledge"))  //HARDCODEDMS    eula
                                            {
                                                String checkbox = "//input[@type='checkbox']";
                                                IWebElement elementToClick = driver.FindElement(By.XPath(checkbox));
                                                elementToClick.Click();
                                                Thread.Sleep(1000); //Hardcoded


                                                //driver.FindElementByClassName("btn btn-primary").Click();
                                                String button = "//input[@type='button']";
                                                elementToClick = driver.FindElement(By.XPath(button));  //<input type="button" ng-click="vm.save()" class="btn btn-primary" value="Accept" ng-disabled="eulaForm.$invalid" />
                                                elementToClick.Click();
                                                Thread.Sleep(1000); //Hardcoded

                                                driver.Navigate().GoToUrl(sURLMS);
                                                Thread.Sleep(3000 + iSleepMore); //Hardcoded

                                            }
                                        }
                                        catch (Exception exEULA)
                                        {
                                            Console.WriteLine("Exception exEULA: " + exEULA.Message + " " + exEULA.InnerException);
                                        }
                                        
                                        System.IO.File.WriteAllText(sMSFilePath, HttpUtility.HtmlDecode(driver.PageSource).Replace("\u00A0", " "));   //Hardcoded &nbsp;
                                    }
                                    lVisitedURLs.Add(sURLMS);
                                    //}
                                }
                                catch (Exception exFileInfo)
                                {
                                    Console.WriteLine("Exception: exFileInfo " + exFileInfo.Message + " " + exFileInfo.InnerException);
                                }
                                #endregion getKBfomMSpage

                                //Parse the MS file referenced in the CVE
                                try
                                {
                                    ResponseText = System.IO.File.ReadAllText(sMSFilePath);   ////Hardcoded
                                    if(ResponseText.ToLower().Contains("directshow"))
                                    {
                                        if (!lFileNamesToSearch.Contains("quartz.dll")) lFileNamesToSearch.Add("quartz.dll");   //DirectX
                                        if (!lFileNamesToSearch.Contains("ksuser.dll")) lFileNamesToSearch.Add("ksuser.dll");   //kernelsupport
                                        if (!lFileNamesToSearch.Contains("devenum.dll")) lFileNamesToSearch.Add("devenum.dll");
                                        if (!lFileNamesToSearch.Contains("qdvd.dll")) lFileNamesToSearch.Add("qdvd.dll");
                                        if (!lFileNamesToSearch.Contains("evr.dll")) lFileNamesToSearch.Add("evr.dll"); //enahncedvideorenderer
                                    }
                                }
                                catch (Exception exReadMSFile)
                                {
                                    Console.WriteLine("Exception: exReadMSFile " + exReadMSFile.Message + " " + exReadMSFile.InnerException);
                                }


                                //Try to identify the Main Product(s)   NOTE: known if using the MS API
                                //<h2 class="subheading">Cumulative Security Update for Microsoft Edge (3199057)</h2>
                                string sMSTitle = "";
                                #region analyzeMSPageTitleH2
                                try
                                {
                                    //Console.WriteLine("DEBUG AnalyzeMSPageTitleH2 START " + DateTimeOffset.Now);
                                    //<h2 class="form-control-static flat-bottom ng-binding">CVE-2017-0077 | Dxgkrnl.sys Elevation of Privilege Vulnerability</h2>
                                    //Regex myRegexMSTitle = new Regex("<h2 class=\"subheading\">(.*?)</h2>", RegexOptions.Singleline);   //HARDCODEDMSHTML
                                    Regex myRegexMSTitle = new Regex("<h2 class=\"(.*?)</h2>", RegexOptions.Singleline);   //HARDCODEDMSHTML

                                    //Cumulative Security Update for Internet Explorer (4013073)
                                    //CVE-2017-0077 | Dxgkrnl.sys Elevation of Privilege Vulnerability
                                    try
                                    {
                                        sMSTitle = myRegexMSTitle.Match(ResponseText).ToString();    //Take just the 1st one
                                        string[] TitleSplit = sMSTitle.Split(new string[] { ">" }, StringSplitOptions.None);
                                        sMSTitle = TitleSplit[1];
                                        Console.WriteLine("DEBUG sMSTitle=" + sMSTitle.Replace("</h2", ""));

                                        TitleSplit = sMSTitle.Split(new string[] { " " }, StringSplitOptions.None);
                                        foreach (string sMSTitleWord in TitleSplit)
                                        {
                                            if (sMSTitleWord.EndsWith(".dll") || sMSTitleWord.EndsWith(".exe") || sMSTitleWord.EndsWith(".sys") || sMSTitleWord.EndsWith(".ocx"))    //HARDCODED
                                            {
                                                //if (sFileNameToSearch == "")
                                                //{
                                                    sFileNameToSearch = sMSTitleWord.ToLower();
                                                    Console.WriteLine("DEBUG FileNameToSearch form MSTitle=" + sMSTitleWord);
                                                    if (!lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                                                //}
                                            }
                                        }

                                        //Graphics Component
                                        if (sMSTitle.ToLower().Contains("graphics component"))   //HARDCODED
                                        {
                                            if (sFileNameToSearch == "")
                                            {
                                                sFileNameToSearch = "win32k.sys";
                                                if (!lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);

                                                //gdi32.dll
                                                sFileNameToSearch = "gdiplus.dll";
                                                if (!lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                                                //Fontsub.dll
                                                //Fntcache.dll
                                            }
                                        }
                                        if (sMSTitle.ToLower().Contains("gdi component"))   //HARDCODED
                                        {
                                            if (sFileNameToSearch == "")
                                            {
                                                sFileNameToSearch = "gdi32full.dll";
                                                if (!lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);

                                                sFileNameToSearch = "gdi32.dll";
                                                if (!lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);

                                            }
                                        }
                                        //...


                                        foreach(string sExcludedFilename in lFileNamesNOTToSearch)
                                        {
                                            lFileNamesToSearch.Remove(sExcludedFilename);
                                        }
                                    }
                                    catch (Exception exMSTitleSplit)
                                    {
                                        Console.WriteLine("Exception: exMSTitleSplit " + exMSTitleSplit.Message + " " + exMSTitleSplit.InnerException);
                                    }

                                    
                                    //Console.WriteLine("DEBUG lProductsMicrosoft.Count=" + lProductsMicrosoft.Count);
                                    foreach (string sMSProductName in lProductsMicrosoft)
                                    {
                                        if (sMSTitle.ToLower().Contains(sMSProductName)) //Known Microsoft Product
                                        {
                                            sMainProductName = sMSProductName;
                                            Console.WriteLine("DEBUG sMainProductName=" + sMainProductName);
                                            sMainProductNameToTitle = new CultureInfo("en-US").TextInfo.ToTitleCase(sMainProductName);
                                            if (!lMainProductsNames.Contains(sMainProductNameToTitle.ToLower())) lMainProductsNames.Add(sMainProductNameToTitle.ToLower());

                                            Console.WriteLine("DEBUG sFileNameToSearchHERE=" + sFileNameToSearch);

                                            bUseOnlyCPEs = false;   //i.e. Internet Explorer, Edge
                                            //Console.WriteLine("DEBUG bUseOnlyCPEs=false");

                                            //Note: sMSTitle could also contain the name of a file    //i.e. MS16-144 : Mise à jour de sécurité pour Hlink.dll pour Internet Explorer datée du 13 décembre 2016
                                            if (sFileNameToSearch == "")
                                            {
                                                sFileNameToSearch = fSearchVulnerableFilename(sMSTitle);
                                                if (sFileNameToSearch != "") Console.WriteLine("DEBUG: sFileNameToSearch retrieved from MS title: " + sFileNameToSearch);
                                                if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                                            }

                                            //TODO REVIEW - Use XORCISM with AI
                                            //if (sFileNameToSearch == "")
                                            //{
                                            //In case of multiple products and MS page (i.e. Internet Explorer and Edge)
                                            string sFileNameToSearchHardcoded = fHardcodeVulnerableFilename(sMSProductName);
                                            if (sFileNameToSearchHardcoded.Trim() != "" && sFileNameToSearchHardcoded != sFileNameToSearch)
                                            {
                                                //sFileNameToSearch = sFileNameToSearchHardcoded; //TODO  Review
                                                ////sFileNameToSearch = fHardcodeVulnerableFilename(sMSProductName);
                                                if (sFileNameToSearch != "") Console.WriteLine("DEBUG: sFileNameToSearch hardcoded from MS title: " + sFileNameToSearch);
                                                //if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                                                if (sFileNameToSearchHardcoded != "" && !lFileNamesToSearch.Contains(sFileNameToSearchHardcoded)) lFileNamesToSearch.Add(sFileNameToSearchHardcoded);

                                            }
                                            //}
                                        }
                                    }

                                    //TODO  (add stuff in fHardcodeVulnerableFilename() //i.e. Graphics Component
                                    /*
                                    //Note: sMSTitle could also indicates the name of a file
                                    if (sFileNameToSearch == "")
                                    {
                                        sFileNameToSearch = fHardcodeVulnerableFilename(sMSTitle);    //i.e. Graphics Component
                                        if (sFileNameToSearch != "") Console.WriteLine("DEBUG: sFileNameToSearch retrieved from MS title2: " + sFileNameToSearch);
                                        if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                                    }
                                    */
                                }
                                catch (Exception exMSTitle)
                                {
                                    Console.WriteLine("Exception: exMSTitle " + exMSTitle.Message + " " + exMSTitle.InnerException);
                                }
                                
                                //Console.WriteLine("DEBUG analyzeMSPageTitleH2 END " + DateTimeOffset.Now);
                                #endregion analyzeMSPageTitleH2


                                if (sForceMS != "" || oReference.ReferenceURL.ToLower().Contains("security-guidance/advisory"))
                                {
                                    //Console.WriteLine("DEBUG sForceMSCatalog");
                                    //Security Update for Microsoft Office (4013241)
                                    //https://support.microsoft.com/help/4013241

                                    //https://catalog.update.microsoft.com/v7/site/Search.aspx?q=KB4012204
                                    //                            Regex myRegexKBCatalogURL = new Regex("https://catalog.update.microsoft.com/v7/site/Search.aspx\?q=KB(\d+)\">", RegexOptions.Singleline);   //HardcodedMS //TODO Review
                                    Regex myRegexKBCatalogURL = new Regex(@"catalog.update.microsoft.com/v7/site/Search.aspx\?q=KB(\d+)", RegexOptions.Singleline);   //HardcodedMS //TODO Review

                                    List<string> lKBCatalogURLs = new List<string>();

                                    //Extract the KB links from the MS- page
                                    if (sMSTitle.Contains("Microsoft Malware Protection Engine"))    //Microsoft Windows Defender        Microsoft Security Essentials       Microsoft Forefront Security for SharePoint
                                    {
                                        sFileNameToSearch = "mrt.exe"; //HardcodedMS
                                        if (sFileNameToSearch != "" && !lFileNamesToSearch.Contains(sFileNameToSearch)) lFileNamesToSearch.Add(sFileNameToSearch);
                                        //Trick
                                        ResponseText = ResponseText + "catalog.update.microsoft.com/v7/site/Search.aspx?q=KB890830"; //HARDCODEDMS
                                        //TODO: We would have to use registry_state/registry_object and not mrt.exe
                                        //Windows Defender EngineVersion
                                        //Security Essentials EngineVersion
                                        //Forefront Security for SharePoint EngineVersion
                                    }

                                    List<string> lKBNumbers = new List<string>();
                                    MatchCollection matchKBCatalogURLs = myRegexKBCatalogURL.Matches(ResponseText);
                                    foreach (Match matchKBCatalogURL in matchKBCatalogURLs)  //For each KB link found
                                    {
                                        if (!lKBCatalogURLs.Contains(matchKBCatalogURL.ToString()))
                                        {
                                            
                                            string sKBNumber = matchKBCatalogURL.ToString().Replace("catalog.update.microsoft.com/v7/site/Search.aspx?q=", "").ToUpper();   //HardcodedMS
                                            Console.WriteLine("DEBUG sKBNumber=" + sKBNumber);
                                            lKBNumbers.Add(sKBNumber);//.Replace("KB",""));

                                            //We check if: 1) We already have this KB (and its files) in the database   2) We have this KB locally
                                            
                                            int iPatchID = 0;
                                            try
                                            {
                                                iPatchID = model.PATCH.Where(o => o.PatchVocabularyID == sKBNumber).FirstOrDefault().PatchID;
                                            }
                                            catch(Exception ex)
                                            {

                                            }
                                            //TODO
                                            if (iPatchID > 0 && !bPatchJustAdded)   //if just added, we potentially still did not downloaded all of them
                                            {
                                                //covfefe
                                                string[] directoryEntries = Directory.GetDirectories(sLocalPathForMSKBfiles, sKBNumber + "*");
                                                foreach (string directoryPath in directoryEntries)
                                                {
                                                    Console.WriteLine("DEBUG KBDirectory=" + directoryPath);
                                                    //KB3071756-Windows_Server_2008_R2_for_Itanium-ia64
                                                    string sProductName = string.Empty;
                                                    
                                                    //Here we need to retrieve the ProductName from the KB directory name
                                                    string sDirectoryPathTest = directoryPath.ToLower().Replace("_", " ").Replace(" embedded", "").Replace(" standard", "").Replace(" enterprise","");  //rt?

                                                    if (sDirectoryPathTest.Contains("windows 10 version 1607")) sProductName = "windows 10 version 1607";
                                                    //TODO  x86   x64   sp?
                                                    if (sDirectoryPathTest.Contains("windows 10 version 1511")) sProductName = "windows 10 version 1511";
                                                    if (sDirectoryPathTest.Contains("windows server 2016")) sProductName = "windows server 2016";
                                                    if (sDirectoryPathTest.Contains("windows server 2012 r2")) sProductName = "windows server 2012 r2";
                                                    if (sProductName == "" && sDirectoryPathTest.Contains("windows server 2012")) sProductName = "windows server 2012";
                                                    if (sDirectoryPathTest.Contains("windows server 2008 r2")) sProductName = "windows server 2008 r2";
                                                    if (sProductName == "" && sDirectoryPathTest.Contains("windows server 2008")) sProductName = "windows server 2008"; //Windows6.0
                                                    if (sProductName == "" && sDirectoryPathTest.Contains("windows 10")) sProductName = "windows 10";

                                                    //rt?
                                                    if (sProductName == "" && sDirectoryPathTest.Contains("windows 8.1")) sProductName = "windows 8.1";
                                                    if (sProductName == "" && sDirectoryPathTest.Contains("windows 8")) sProductName = "windows 8";
                                                    //!!!   Windows_Embedded_Standard_7
                                                    if (sProductName == "" && sDirectoryPathTest.Contains("windows 7")) sProductName = "windows 7"; //Windows6.1
                                                    if (sProductName == "" && sDirectoryPathTest.Contains("windows vista")) sProductName = "windows vista";
                                                    if (sProductName == "" && (sDirectoryPathTest.Contains("windows xp") || sDirectoryPathTest.Contains("windowsxp"))) sProductName = "windows xp";
                                                    //TODO!!!
                                                    //...
                                                    string sProductNameReduced = sProductName;//.ToLower().Replace("microsoft ", "").Replace(" for itanium-based systems", " itanium-based edition").Replace("x86", "").Replace("x64", "").Trim();  //Hardcoded    //i.e. windows vista x86

                                                    if (sDirectoryPathTest.Contains("x86"))
                                                    {
                                                        sProductName += " x86";
                                                    }
                                                    else
                                                    {
                                                        if (sDirectoryPathTest.Contains("x64")) //amd64
                                                        {
                                                            sProductName += " x64";
                                                        }
                                                    }
                                                    //foreach (string sProductFound in lProductsFoundInMSAPI)
                                                    Console.WriteLine("DEBUG sProductNameIdentified=" + sProductName);
                                                    sProductFoundDEADBEEFGlobal = sProductName; //TODO: We would need to know the Service Pack... (to be retrieved from the database/or VulnDescription)

                                                    //sFileInfoNeededFound = fDecompressKBAndSearchFiles(sMSKBFilePathTarget, sMSCatalogFileNameLocalPath, sProductName, sProductNameReduced);
                                                    //Note: sMSCatalogFileNameLocalPath could be guessed (directoryPath + ".cab"/".msu"...)
                                                    string sFileInfoNeededFound = fDecompressKBAndSearchFiles(directoryPath, "", sProductName, sProductNameReduced, sFileNameToSearch);// sFileNameToSearchReplaced);
                                                }
                                            }
                                            else
                                            {
                                                //We need to visit Uncle M
                                                Console.WriteLine("DEBUG matchKBCatalogURL=" + matchKBCatalogURL);
                                                lKBCatalogURLs.Add(matchKBCatalogURL.ToString());
                                                try
                                                {
                                                    ////                                //fMSKBCatalogPage(sKBNumber, sFileNameToSearchReplaced, sProductFound, sKBURLFinal2, sProductFoundDEADBEEF);
                                                    //fMSKBCatalogPage(sKBNumber, sFileNameToSearch, "", "http://www." + matchKBCatalogURL.ToString(), "");
                                                    fMSKBCatalogPage(sKBNumber, sFileNameToSearch, "", "https://" + matchKBCatalogURL.ToString(), "", oVulnerability.VulnerabilityID, sMSTitle);

                                                }
                                                catch (Exception exfMSKBCatalogPage1)
                                                {
                                                    Console.WriteLine("Exception: exfMSKBCatalogPage1 " + exfMSKBCatalogPage1.Message + " " + exfMSKBCatalogPage1.InnerException);
                                                }
                                            }
                                        }
                                    }


                                    Regex myRegexKBNumberExtract = new Regex(@"\(\d+\)", RegexOptions.Singleline);
                                    MatchCollection KBNumbers = myRegexKBNumberExtract.Matches(ResponseText);
                                    //List<string> lKBNumbers = new List<string>();
                                    foreach (Match KBNumber in KBNumbers)
                                    {
                                        string sKBNumber = "KB" + KBNumber.ToString().Replace("(", "").Replace(")", "");   //hardcoded
                                        if (sKBNumber.Length > 5)
                                        {
                                            if (!lKBNumbers.Contains(sKBNumber))
                                            {
                                                Console.WriteLine("DEBUG KBNumberExtracted: " + sKBNumber);
                                                lKBNumbers.Add(sKBNumber);
                                                //We check if: 1) We already have this KB (and its files) in the database   2) We have this KB locally
                                                //TODO 1)
                                                
                                                int iPatchID = 0;
                                                try
                                                {
                                                    iPatchID = model.PATCH.Where(o => o.PatchVocabularyID == sKBNumber).FirstOrDefault().PatchID;
                                                }
                                                catch(Exception ex)
                                                {

                                                }
                                                //TODO
                                                if (iPatchID > 0 && !bPatchJustAdded)   //if just added, we potentially still did not downloaded all of them
                                                {
                                                    //covfefe
                                                    string[] directoryEntries = Directory.GetDirectories(sLocalPathForMSKBfiles, sKBNumber + "*");
                                                    foreach (string directoryPath in directoryEntries)
                                                    {
                                                        Console.WriteLine("DEBUG KBDirectory=" + directoryPath);
                                                        //KB3071756-Windows_Server_2008_R2_for_Itanium-ia64
                                                        string sProductName = string.Empty;

                                                        //Here we need to retrieve the ProductName from the KB directory name
                                                        string sDirectoryPathTest = directoryPath.ToLower().Replace("_", " ").Replace(" embedded", "").Replace(" standard", "").Replace(" enterprise", "");  //rt?

                                                        if (sDirectoryPathTest.Contains("windows 10 version 1607")) sProductName = "windows 10 version 1607";
                                                        //TODO  x86   x64   sp?
                                                        if (sDirectoryPathTest.Contains("windows 10 version 1511")) sProductName = "windows 10 version 1511";
                                                        if (sDirectoryPathTest.Contains("windows server 2016")) sProductName = "windows server 2016";
                                                        if (sDirectoryPathTest.Contains("windows server 2012 r2")) sProductName = "windows server 2012 r2";
                                                        if (sProductName == "" && sDirectoryPathTest.Contains("windows server 2012")) sProductName = "windows server 2012";
                                                        if (sDirectoryPathTest.Contains("windows server 2008 r2")) sProductName = "windows server 2008 r2";
                                                        if (sProductName == "" && sDirectoryPathTest.Contains("windows server 2008")) sProductName = "windows server 2008";
                                                        if (sProductName == "" && sDirectoryPathTest.Contains("windows 10")) sProductName = "windows 10";

                                                        //rt?
                                                        if (sProductName == "" && sDirectoryPathTest.Contains("windows 8.1")) sProductName = "windows 8.1";
                                                        if (sProductName == "" && sDirectoryPathTest.Contains("windows 8")) sProductName = "windows 8";
                                                        if (sProductName == "" && sDirectoryPathTest.Contains("windows 7")) sProductName = "windows 7";
                                                        if (sProductName == "" && sDirectoryPathTest.Contains("windows vista")) sProductName = "windows vista";
                                                        if (sProductName == "" && (sDirectoryPathTest.Contains("windows xp") || sDirectoryPathTest.Contains("windowsxp"))) sProductName = "windows xp";
                                                        //TODO!!!
                                                        //...
                                                        string sProductNameReduced = sProductName;//.ToLower().Replace("microsoft ", "").Replace(" for itanium-based systems", " itanium-based edition").Replace("x86", "").Replace("x64", "").Trim();  //Hardcoded    //i.e. windows vista x86

                                                        if (sDirectoryPathTest.Contains("x86"))
                                                        {
                                                            sProductName += " x86";
                                                        }
                                                        else
                                                        {
                                                            if (sDirectoryPathTest.Contains("x64")) //amd64
                                                            {
                                                                sProductName += " x64";
                                                            }
                                                        }
                                                        //foreach (string sProductFound in lProductsFoundInMSAPI)
                                                        Console.WriteLine("DEBUG sProductNameIdentified=" + sProductName);
                                                        sProductFoundDEADBEEFGlobal = sProductName; //TODO: We would need to know the Service Pack... (to be retrieved from the database/or VulnDescription)

                                                        //sFileInfoNeededFound = fDecompressKBAndSearchFiles(sMSKBFilePathTarget, sMSCatalogFileNameLocalPath, sProductName, sProductNameReduced);
                                                        //Note: sMSCatalogFileNameLocalPath could be guessed (directoryPath + ".cab"/".msu"...)
                                                        string sFileInfoNeededFound = fDecompressKBAndSearchFiles(directoryPath, "", sProductName, sProductNameReduced, sFileNameToSearch);// sFileNameToSearchReplaced);
                                                    }
                                                }
                                                else
                                                {
                                                    //We need to visit Uncle M
                                                    try
                                                    {
                                                        if (!sKBNumber.ToUpper().StartsWith("KB")) sKBNumber = "KB" + sKBNumber;
                                                        fMSKBCatalogPage(sKBNumber, sFileNameToSearch, "", "https://catalog.update.microsoft.com/v7/site/Search.aspx?q=" + sKBNumber, "", oVulnerability.VulnerabilityID, sMSTitle);   //HARDCODEDMS
                                                    }
                                                    catch (Exception exfMSKBCatalogPage2)
                                                    {
                                                        Console.WriteLine("Exception: exfMSKBCatalogPage2 " + exfMSKBCatalogPage2.Message + " " + exfMSKBCatalogPage2.InnerException);
                                                    }
                                                }
                                            }
                                        }

                                    }
                                }

                                sVULDescription = oVulnerability.VULDescription;

                                //Regex myRegexKBURL = new Regex("support.microsoft.com/kb/(.*?)\">", RegexOptions.Singleline);   //HardcodedMS //TODO Review
                                Regex myRegexKBURL = new Regex("support.microsoft.com/kb/\\d+", RegexOptions.Singleline);   //HardcodedMS //TODO Review


                                //Extract the KB links from the MS- page
                                MatchCollection myKBURLs = myRegexKBURL.Matches(ResponseText);

                                foreach (Match matchKBURL in myKBURLs)  //For each KB link found
                                {
                                    string sKBURL = "https://" + matchKBURL.Value.Replace("\">", ""); //Hardcoded
                                    sKBURL = sKBURL.Replace("microsoft.com/kb", "microsoft.com/en-us/kb");  //Enforce English version

                                    if (!lVisitedURLs.Contains(sKBURL))
                                    {
                                        lVisitedURLs.Add(sKBURL);
                                        Console.WriteLine("DEBUG KBURL: " + sKBURL);
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                    try
                                    {
                                        fAnalyzeKBURL(sKBURL, sFileNameToSearch, sVULDescription, sMainProductName, oVulnerability.VulnerabilityID, "");
                                    }
                                    catch (Exception exfAnalyzeKBURL)
                                    {
                                        Console.WriteLine("Exception: exfAnalyzeKBURL " + exfAnalyzeKBURL.Message + " " + exfAnalyzeKBURL.InnerException);
                                    }
                                    if (oReference.ReferenceURL.ToLower().Contains("security-guidance/advisory"))
                                    {

                                    }
                                    else
                                    {
                                        Console.WriteLine("DEBUG break");
                                        break;  //TODO: Review For now we just work with the 1st BigKBlink found in the initial MS- page of the Vulnerability. Note that others could appear in the comments
                                    }

                                }
                            }
                        }
                    }
                    catch(Exception exVulnRefLoop)
                    {
                        Console.WriteLine("Exception: exVulnRefLoop " + exVulnRefLoop.Message + " " + exVulnRefLoop.InnerException);
                    }
                }
            }
            else
            {
                if (sForceKB == string.Empty)
                {
                    Console.WriteLine("ERROR: UNKNOWN VULNERABILITY");
                    return;
                }
                else
                {
                    //Console.WriteLine("DEBUG ForceKBURL: " + sKBURL);
                    fAnalyzeKBURL("https://support.microsoft.com/en-us/kb/" + sForceKB);  //TODO: sFileNameToSearch   sVULDescription (=sForceVULDescription)     sMainProductName
                }
            }


            if (bWriteOVAL)
            {
                //********************************************************************************************************************************
                //********************************************************************************************************************************
                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                
                Console.WriteLine("DEBUG WRITING THE OVAL XML");
                //BigFix
                //OpenVAS
                //********************************************************************************************************************************

                #region writeoval
                StreamWriter monStreamWriter = null;

                //Reorder dProductFile by files
                //windows server 2012 20161116 Clfs.sys 6.2.9200.22034
                //Dictionary<string, string> dFileProduct = new Dictionary<string, string>();
                List<string> lFileProduct = new List<string>();
                Dictionary<string, string> dFileVersionOVALState = new Dictionary<string, string>();   //To avoid queries for the same file

                string sOVALDefinitionFilename = "";
                string sMyOVALDefinitionID = "";
                try
                {
                    sMyOVALDefinitionID = "oval_org.xorcism_def_" + sCVE.ToLower().Replace("cve", "").Replace("-", "");    //Hardcoded
                    string sMyOVALDefinitionID2 = sMyOVALDefinitionID.Replace("_", ":");//.Replace(".",":");
                    string sMyOVALDefinitionTitle = "Vulnerability";    //Default
                    #region ovaldefinitiontitle
                    //HARDCODED
                    try
                    {
                        if (oVulnerability.VULDescription.Contains(" aka "))
                        {
                            string[] sVULDescriptionSplit = oVulnerability.VULDescription.Split(new string[] { " aka " }, StringSplitOptions.None);
                            sMyOVALDefinitionTitle = sVULDescriptionSplit[1].Replace("\"", "").Replace(".", "").Trim();
                        }
                    }
                    catch (Exception exMyOVALDefinitionTitle)
                    {

                    }

                    sMyOVALDefinitionTitle = sMyOVALDefinitionTitle + " - " + sCVE;
                    if (sMSID != "" && sMSID != "temp") sMyOVALDefinitionTitle = sMyOVALDefinitionTitle + " (" + sMSID + ")";
                    #endregion ovaldefinitiontitle

                    //repository\\definitions\\vulnerability\\
                    sOVALDefinitionFilename = sMyOVALDefinitionID + ".xml";
                    monStreamWriter = new StreamWriter(sCVE + "\\" + sOVALDefinitionFilename);    //Hardcoded Path
                    monStreamWriter.WriteLine("<oval-def:definition xmlns:oval-def=\"http://oval.mitre.org/XMLSchema/oval-definitions-5\" class=\"vulnerability\" id=\"" + sMyOVALDefinitionID2 + "\" version=\"0\">"); //Hardcoded
                    monStreamWriter.WriteLine("\t<oval-def:metadata>");
                    monStreamWriter.WriteLine("\t\t<oval-def:title>" + sMyOVALDefinitionTitle + "</oval-def:title>");
                    monStreamWriter.WriteLine("\t\t<oval-def:affected family=\"windows\">");

                    //Platforms*
                    //We list all the Platforms
                    //And do other things...
                    List<string> lPlatforms = new List<string>();
                    List<string> lAllProducts = new List<string>();
                    Dictionary<string, string> dProductOVALInventoryDefinition = new Dictionary<string, string>();

                    //List<string> lMainProductsToRemove = new List<string>();
                    List<string> lMainProductsImproved = new List<string>();
                    foreach (string sMainProductFound in lMainProductsNames)    //TODO: NOT optimized
                    {
                        //    Console.WriteLine("DEBUG sMainProductFoundRemaining=" + sMainProductFound);
                        //bool bFileFoundForMainProduct = false;
                        try
                        {
                            #region getplatforms
                            //Get the known Platforms of the Products
                            foreach (var x in dProductFile.OrderBy(o => o.Key))
                            {
                                /*
                                if (x.Key.Contains(sMainProductFound))   //microsoft lync 2013 contains microsoft lync 2013
                                {
                                    bFileFoundForMainProduct = true;
                                }
                                */

                                //Console.WriteLine("DEBUG Searching Platforms for (Product): " + x.Key);
                                //internet explorer 9 x86DEADBEEFwindows vista x86
                                //DEADBEEFwindows 7 x86
                                //microsoft .net framework 3.5 x64DEADBEEFwindows 10 version 1607 x64 20161005 gdiplus.dll 10.0.14393.321
                                //microsoft lync 2013 x64DEADBEEFmicrosoft lync basic 2013 x64 20160913 lync.lync.exe 15.0.4867.1000
                                string[] sxKeySplit = x.Key.Split(new string[] { "DEADBEEF" }, StringSplitOptions.None);
                                string sMyProductName = sxKeySplit[0];  //microsoft lync 2013 x64

                                if (sMyProductName.Trim() == "" || sMyProductName == "windows kernel" || sMyProductName == "microsoft management console" || sMyProductName.Contains("active directory federation services"))  //HARDCODEDMSWINSERVICES because they are not Products   //TODO Improve (services...)
                                {
                                    //Add "windows kernel" to lMainProductsImproved?
                                    try
                                    {
                                        sMyProductName = sxKeySplit[1];
                                    }
                                    catch (Exception exKeySplit1)
                                    {
                                        Console.WriteLine("Exception: exKeySplit1 " + exKeySplit1.Message + " " + exKeySplit1.InnerException);
                                    }
                                }

                                if (sMyProductName.StartsWith("windows") && !sMyProductName.Contains("kernel"))   //HARDCODED
                                {
                                    //The Product (windows xxx) is a Platform
                                    string sPlatformName = new CultureInfo("en-US").TextInfo.ToTitleCase(sMyProductName).Replace(" Sp", " SP").Replace(" Rt", " RT");   //HARDCODED
                                    sPlatformName = sPlatformName.Replace("X86", "x86");
                                    sPlatformName = sPlatformName.Replace("X64", "x64");
                                    sPlatformName = fGetPlatformName(sPlatformName);

                                    if (!lPlatforms.Contains(sPlatformName) && !lPlatforms.Contains("Microsoft " + sPlatformName))
                                    {
                                        lPlatforms.Add("Microsoft " + sPlatformName);
                                    }
                                }
                                else//not windows xxx
                                {

                                    string sProductName = new CultureInfo("en-US").TextInfo.ToTitleCase(sMyProductName).Replace(" Sp", " SP").Replace(" Rt", " RT");   //HARDCODED

                                    sProductName = sProductName.Replace("X86", "x86");
                                    sProductName = sProductName.Replace("X64", "x64");
                                    sProductName = fGetPlatformName(sProductName);

                                    if (!lMainProductsImproved.Contains(sProductName.ToLower()))
                                    {
                                        //If we don't have an OVALInventoryDefinition for this Product, we drop it
                                        OVALDEFINITION oOVALInventoryDefinition = new OVALDEFINITION();
                                        if (sProductName.ToLower() != "joker")
                                        {
                                            //TODO: List/Dictionary
                                            if (!lOVALDefInventoryNotRetrieved.Contains(sProductName.ToLower()))
                                            {
                                                oOVALInventoryDefinition = fGetOVALInventoryDefinitionForProduct(sProductName.ToLower()); //hardcoded
                                            }
                                        }
                                        if (oOVALInventoryDefinition != null || sProductName.ToLower() == "joker")
                                        {
                                            lMainProductsImproved.Add(sProductName.ToLower());
                                            Console.WriteLine("DEBUG sMainProductFoundImproved=" + sProductName.ToLower());
                                        }
                                        else
                                        {
                                            //ERROR Printed by the function
                                            //ERROR: OVALDefInventory not retrieved for
                                            continue;
                                        }
                                    }

                                    if (!lOVALDefInventoryNotRetrieved.Contains(sProductName))
                                    {
                                        if (!lAllProducts.Contains(sProductName) && !lAllProducts.Contains("Microsoft " + sProductName))    //We did not do that before
                                        {
                                            //Console.WriteLine("DEBUG sProductName1=" + sProductName);
                                            Console.WriteLine("DEBUG Adding to lAllProducts sProductName1=" + sProductName);
                                            lAllProducts.Add(sProductName);
                                            if (sProductName == "JOKER") continue;  //HARDCODED

                                            //We can retrieve the Known Platform for the Product (if needed), but we don't want ALL of them (ideally just the ones found)
                                            if (lPlatforms.Count == 0)
                                            {
                                                if (x.Key.Contains("windows"))
                                                {
                                                    try
                                                    {
                                                        sMyProductName = sxKeySplit[1];
                                                    }
                                                    catch (Exception exProductNameKeySplit1)
                                                    {
                                                        Console.WriteLine("Exception: exProductNameKeySplit1 " + exProductNameKeySplit1.Message + " " + exProductNameKeySplit1.InnerException);
                                                    }
                                                    if (sMyProductName.StartsWith("windows") && !sMyProductName.Contains("kernel"))   //HARDCODED (kernel not needed here?)
                                                    {
                                                        //The Product (windows xxx) is a Platform
                                                        string sPlatformName = new CultureInfo("en-US").TextInfo.ToTitleCase(sMyProductName).Replace(" Sp", " SP").Replace(" Rt", " RT");   //HARDCODED
                                                        sPlatformName = sPlatformName.Replace("X86", "x86");
                                                        sPlatformName = sPlatformName.Replace("X64", "x64");
                                                        sPlatformName = fGetPlatformName(sPlatformName);

                                                        if (!lPlatforms.Contains(sPlatformName) && !lPlatforms.Contains("Microsoft " + sPlatformName))
                                                        {
                                                            lPlatforms.Add("Microsoft " + sPlatformName);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    #region searchplatformsforproduct
                                                    sProductName = sProductName.Replace("x86", "").Replace("x64", "");  //Hardcoded
                                                    int iProductOVALID = 0;
                                                    try
                                                    {
                                                        iProductOVALID = oval_model.PRODUCT.Where(o => o.ProductName == sProductName || o.ProductName == "Microsoft " + sProductName).FirstOrDefault().ProductID;
                                                    }
                                                    catch (Exception ex)
                                                    {

                                                    }
                                                    if (iProductOVALID > 0)
                                                    {
                                                        var ProductPlatforms = oval_model.PRODUCTPLATFORM.Where(o => o.ProductID == iProductOVALID);
                                                        foreach (XOVALModel.PRODUCTPLATFORM oProductPlatform in ProductPlatforms)
                                                        {
                                                            if (oProductPlatform.PLATFORM.PlatformName.ToLower().Contains("windows"))   //Exclude Apple Mac OS X
                                                            {
                                                                string sPlatformNameTemp = oProductPlatform.PLATFORM.PlatformName.Replace("(x86)", "").Replace("(x64)", "").Replace("(Ia-64)", "").Replace("Itanium", "").Trim();
                                                                if (!lPlatforms.Contains(sPlatformNameTemp)) lPlatforms.Add(sPlatformNameTemp);
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (sProductName.ToLower().Contains("service pack"))
                                                        {
                                                            //2nd try
                                                            string sProductNameTemp = sProductName;
                                                            for (int i = 1; i <= 9; i++)
                                                            {
                                                                sProductNameTemp = sProductNameTemp.Replace(" service pack " + i, "");
                                                            }

                                                            iProductOVALID = 0;
                                                            try
                                                            {
                                                                iProductOVALID = oval_model.PRODUCT.Where(o => o.ProductName == sProductNameTemp || o.ProductName == "Microsoft " + sProductNameTemp).FirstOrDefault().ProductID;
                                                            }
                                                            catch (Exception ex)
                                                            {

                                                            }
                                                            if (iProductOVALID > 0)
                                                            {
                                                                var ProductPlatforms = oval_model.PRODUCTPLATFORM.Where(o => o.ProductID == iProductOVALID);
                                                                foreach (XOVALModel.PRODUCTPLATFORM oProductPlatform in ProductPlatforms)
                                                                {
                                                                    if (oProductPlatform.PLATFORM.PlatformName.ToLower().Contains("windows"))   //Exclude Apple Mac OS X
                                                                    {
                                                                        string sPlatformNameTemp = oProductPlatform.PLATFORM.PlatformName.Replace("(x86)", "").Replace("(x64)", "").Replace("(Ia-64)", "").Replace("Itanium", "").Trim();
                                                                        if (!lPlatforms.Contains(sPlatformNameTemp)) lPlatforms.Add(sPlatformNameTemp);
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                Console.WriteLine("DEBUG PROBLEM? no known Platforms for the Product1 " + sProductNameTemp); //x.Key);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            Console.WriteLine("DEBUG PROBLEM? no known Platforms for the Product2 " + sMyProductName); //x.Key);
                                                        }
                                                    }
                                                    #endregion searchplatformsforproduct
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("NOTE: NOT Adding to lAllProducts sProductName1=" + sProductName + " because in lOVALDefInventoryNotRetrieved");
                                    }
                                }

                                string[] FileInfoSplit = x.Value.Split(' ');
                                ////dFileProduct.Add(FileInfoSplit[1] + " " + FileInfoSplit[2], x.Key);
                                //lFileProduct.Add(FileInfoSplit[1] + " " + FileInfoSplit[2] + "JEROME" + x.Key);    //Hardcoded :-)
                                string sFileProductLine = string.Empty;
                                try
                                {
                                    sFileProductLine = FileInfoSplit[1] + " " + FileInfoSplit[2] + "JEROME" + x.Key;    //Hardcoded :-)
                                }
                                catch (Exception exFileProductLineSplit)
                                {
                                    Console.WriteLine("Exception: exFileProductLineSplit " + exFileProductLineSplit.Message + " " + exFileProductLineSplit.InnerException);
                                }
                                //Console.WriteLine("DEBUG sFileProductLine=" + sFileProductLine);
                                if (!lFileProduct.Contains(sFileProductLine))
                                {

                                    //Console.WriteLine("DEBUG sxKeySplit[1]="+sxKeySplit[1]);
                                    string sProductName2 = sxKeySplit[1];
                                    if (sProductName2 != "" && !sProductName2.StartsWith("windows"))
                                    {
                                        //If we don't have an OVALInventoryDefinition for this Product, we drop it
                                        OVALDEFINITION oOVALInventoryDefinition = new OVALDEFINITION();
                                        if (!lOVALDefInventoryNotRetrieved.Contains(sProductName2.ToLower()))
                                        {
                                            oOVALInventoryDefinition = fGetOVALInventoryDefinitionForProduct(sProductName2.ToLower());
                                        }

                                        Console.WriteLine("DEBUG sProductName2=" + sProductName2);
                                        if (oOVALInventoryDefinition != null)// || sProductName2.Trim()=="")
                                        {
                                            //We keep the fileline?
                                            if (dProductOVALInventoryDefinition.ContainsValue(oOVALInventoryDefinition.OVALDefinitionTitle))
                                            {
                                                Console.WriteLine("DEBUG Same OVALDefinitionTitle we don't keep the FileLine");
                                                //Case:
                                                /*
                                                NOTE: OVALDefInventory not retrieved for sProductNameModified=microsoft lync basic 2013 x86
                                                NOTE: OVALDefInventory Best Match1: Microsoft Lync Basic 2013 is installed
                                                NOTE: OVALDefInventory not retrieved for sProductNameModified=microsoft lync basic 2013 x64
                                                NOTE: OVALDefInventory Best Match1: Microsoft Lync Basic 2013 is installed
                                                */
                                                continue;
                                            }
                                            else
                                            {
                                                try
                                                {
                                                    dProductOVALInventoryDefinition.Add(sProductName2, oOVALInventoryDefinition.OVALDefinitionTitle);
                                                }
                                                catch (Exception exProductOVALDefAdd)
                                                {

                                                }
                                            }

                                        }
                                        else
                                        {
                                            //ERROR Printed by the function
                                            //ERROR: OVALDefInventory not retrieved for
                                            Console.WriteLine("DEBUG Not keeping the FileLine");
                                            continue;
                                        }

                                    }

                                    Console.WriteLine("DEBUG lFileProductAdd " + sFileProductLine);
                                    lFileProduct.Add(sFileProductLine);
                                }
                                //bbb
                            }

                            #endregion getplatforms
                        }
                        catch (Exception exGetPlatforms)
                        {
                            Console.WriteLine("Exception: exGetPlatforms " + exGetPlatforms.Message + " " + exGetPlatforms.InnerException);
                        }
                        //if (!bFileFoundForMainProduct) lMainProductsToRemove.Add(sMainProductFound);
                    }
                    /*
                    foreach (string sMainProductToRemove in lMainProductsToRemove)
                    {
                        Console.WriteLine("DEBUG Eliminating sMainProduct=" + sMainProductToRemove);
                        lMainProductsNames.Remove(sMainProductToRemove);
                    }
                    */
                    Console.WriteLine("DEBUG Replacing lMainProductsNames by lMainProductsImproved");
                    lMainProductsNames = lMainProductsImproved;


                    //Write all the Platforms
                    foreach (string sPlatformName in lPlatforms.OrderBy(o => o))
                    {
                        monStreamWriter.WriteLine("\t\t\t<oval-def:platform>" + sPlatformName + "</oval-def:platform>");
                    }

                    //Write all the Products
                    #region writeallproducts
                    //foreach (var x in dProductFile.OrderBy(o => o.Key))
                    foreach (string sProductFinal in lAllProducts.OrderBy(o => o))
                    {
                        if (sProductFinal == "JOKER") continue;
                        //TODO before?
                        string sProductFinalX = sProductFinal.Replace("Service Pack 1", "");
                        sProductFinalX = sProductFinalX.Replace("Service Pack 2", "");
                        sProductFinalX = sProductFinalX.Replace("Service Pack 3", "");

                        Console.WriteLine("DEBUG Writing sProductFinal=" + sProductFinalX);
                        monStreamWriter.WriteLine("\t\t\t<oval-def:product>" + sProductFinalX.Trim() + "</oval-def:product>");

                    }
                    #endregion writeallproducts

                    monStreamWriter.WriteLine("\t\t</oval-def:affected>");
                    monStreamWriter.WriteLine("\t\t<oval-def:reference ref_id=\"" + sCVE + "\" ref_url=\"http://cve.mitre.org/cgi-bin/cvename.cgi?name=" + sCVE + "\" source=\"CVE\" />");
                    try
                    {
                        monStreamWriter.WriteLine("\t\t<oval-def:description>" + oVulnerability.VULDescription + "</oval-def:description>");
                    }
                    catch (Exception exVulnDef)
                    {
                        //i.e. for ForceKB
                        monStreamWriter.WriteLine("\t\t<oval-def:description>" + sVULDescription + "</oval-def:description>");
                    }
                    monStreamWriter.WriteLine("\t\t<oval-def:oval_repository>");
                    monStreamWriter.WriteLine("\t\t\t<oval-def:dates>");
                    monStreamWriter.WriteLine("\t\t\t\t<oval-def:submitted date=\"" + DateTimeOffset.Now.ToString("yyyy-MM-ddTHH:mm:ssK") + "\">");
                    monStreamWriter.WriteLine("\t\t\t\t\t<oval-def:contributor organization=\"FRHACK\">Jerome Athias</oval-def:contributor>");
                    monStreamWriter.WriteLine("\t\t\t\t</oval-def:submitted>");
                    monStreamWriter.WriteLine("\t\t\t</oval-def:dates>");
                    monStreamWriter.WriteLine("\t\t\t<oval-def:status>INITIAL SUBMISSION</oval-def:status>");
                    //<oval-def:min_schema_version>5.10</oval-def:min_schema_version>
                    monStreamWriter.WriteLine("\t\t</oval-def:oval_repository>");
                    monStreamWriter.WriteLine("\t</oval-def:metadata>");
                    string sTab = "";
                    string sDebug = "";

                    bool bWroteCriteria0 = false;
                    bool bWroteCriteria1Global = false;
                    bool bWroteCriteria2Global = false;
                    string sSaveTabCriteria2Global = "";
                    int iCounterCorrectProductsTotalDef = 0;

                    #region criteria0
                    if (lMainProductsNames.Count > 1)    //i.e. internet explorer    +   edge
                    {
                        if (bDEBUGMODEINXML) sDebug = "0";
                        monStreamWriter.WriteLine("\t<oval-def:criteria" + sDebug + " operator=\"OR\">");
                        sTab = "\t";
                        bWroteCriteria0 = true;
                    }
                    else
                    {
                        //Console.WriteLine("DEBUG NOT Writing Criteria0 lMainProductsNames<=1 Add 1337");
                        //if (bDEBUGMODEINXML) monStreamWriter.WriteLine("DEBUG NOT Writing Criteria0 lMainProductsNames<=1 Add 1337");
                        if (lMainProductsNames.Count == 0) lMainProductsNames.Add("1337");  //HARDCODEDJA (we need at least 1 for the foreach)
                    }
                    #endregion criteria0

                    int iCounterMainProductsProcessed = 0;    //For EndCriteria0
                    foreach (string sCurrentMainProductNameToTile in lMainProductsNames)
                    {
                        iCounterMainProductsProcessed++;
                        //HARDCODED
                        if (sCurrentMainProductNameToTile.ToLower() == "internet explorer") continue;   //TODO Review Enhance (the one from the MS Title) we will have ones with the version number
                        //if (sCurrentMainProductNameToTile.ToLower() == ".net framework") continue;   //TODO Review Enhance (the one from the MS Title) we will have ones with the version number
                        if (sCurrentMainProductNameToTile.ToLower() == "jscript") continue;   //TODO Review Enhance (the one from the MS Title) we will have ones with the version number
                        if (sCurrentMainProductNameToTile.ToLower() == "vbscript") continue;   //TODO Review Enhance (the one from the MS Title) we will have ones with the version number
                        //TODO Review   microsoft office

                        bool bWroteCriteria2 = false;
                        //Console.WriteLine("DEBUG bWroteCriteria2=false");
                        string sSaveTabCriteria1 = "";  //For EndCriteria1
                        string sSaveTabCriteria2 = "";  //For EndCriteria2


                        Console.WriteLine("DEBUG ****************************************************************************************");
                        Console.WriteLine("DEBUG sCurrentMainProductNameToTile=" + sCurrentMainProductNameToTile);
                        sMainProductNameToTitle = new CultureInfo("en-US").TextInfo.ToTitleCase(sCurrentMainProductNameToTile);

                        //Because they are not Products (components or services)
                        //HARDCODEDMSWINSERVICES
                        if (sMainProductNameToTitle == "Windows Kernel") sMainProductNameToTitle = "1337";  //HARDCODEDJA
                        if (sMainProductNameToTitle == "Microsoft Management Console") sMainProductNameToTitle = "1337";  //HARDCODEDJA (probably not needed)
                        if (sMainProductNameToTitle.Contains("Active Directory Federation Services")) sMainProductNameToTitle = "1337";  //HARDCODEDJA (probably not needed)


                        //int iCounterCorrectProductsTotalDef = 0;
                        int iCounterDifferentPlatforms = 0;


                        #region whiterabbit
                        #region sMainProductNameToTitle
                        if (sMainProductNameToTitle != "")// || lMainProductsNames.Count > 1) //Hardcoded  JJJ
                        {

                            //How many correct Products and different Platforms do we have?
                            #region countcorrectproducts
                            bool bFileCorrectProductContainsWindows = false;    //For cosmetic
                            for (int i = 0; i < lFileProduct.Count; i++)
                            {
                                string sFileProduct = lFileProduct[i];
                                Console.WriteLine("DEBUG lFileProduct[i]=" + lFileProduct[i]);
                                if (sFileProduct.Contains("WARNING: No file found!")) continue;   //HARDCODED    Bypass    //TODO Review
                                if (sCurrentMainProductNameToTile != "1337" && !sFileProduct.ToLower().Contains(sCurrentMainProductNameToTile.ToLower())) //TODO REVIEW
                                {
                                    //Bypass
                                }
                                else
                                {
                                    string[] FileProductSplit = sFileProduct.Split(new string[] { "JEROME" }, StringSplitOptions.None); //Hardcoded :-)
                                    string sCurrentFile = FileProductSplit[0];
                                    //Console.WriteLine("DEBUG sCurrentFile0=" + sCurrentFile);
                                    string sCurrentProduct = FileProductSplit[1];
                                    //Console.WriteLine("DEBUG sCurrentProduct0=" + sCurrentProduct);
                                    string[] CurrenProductSplitDEADBEEF = sCurrentProduct.Split(new string[] { "DEADBEEF" }, StringSplitOptions.None);
                                    sCurrentProduct = CurrenProductSplitDEADBEEF[1];
                                    if (sCurrentMainProductNameToTile != "" && sCurrentMainProductNameToTile != "1337" && sCurrentProduct.ToLower().Contains(sCurrentMainProductNameToTile.ToLower()))  //TODO Review
                                    {
                                        //Bypass
                                    }
                                    else
                                    {
                                        //Console.WriteLine("DEBUG CorrectProduct=" + sFileProduct);
                                        if (sFileProduct.Contains("windows")) bFileCorrectProductContainsWindows = true;
                                        iCounterCorrectProductsTotalDef++;
                                    }

                                    string sNextFile = "";
                                    //string sNextProduct = "";
                                    if (i == lFileProduct.Count - 1)
                                    {
                                        //End of the list

                                    }
                                    else
                                    {
                                        string sNextFileProduct = lFileProduct[i + 1];
                                        string[] NextFileProductSplit = sNextFileProduct.Split(new string[] { "JEROME" }, StringSplitOptions.None); //Hardcoded :-)
                                        sNextFile = NextFileProductSplit[0];
                                        //Console.WriteLine("DEBUG sNextFile0=" + sNextFile);
                                        if (sNextFile == sCurrentFile)
                                        {
                                            //Retrieve (in advance) all the Products with the same file/version for the criteria comment
                                            //TODO: add them in a list for later (when looking for the Path in OVALVARIABLEs)
                                            #region countdifferentplatforms
                                            for (int y = i; y < lFileProduct.Count - 1; y++)
                                            {
                                                sNextFileProduct = lFileProduct[y + 1];
                                                //Console.WriteLine("DEBUG sNextFileProductFar0=" + sNextFileProduct);
                                                NextFileProductSplit = sNextFileProduct.Split(new string[] { "JEROME" }, StringSplitOptions.None); //Hardcoded :-)
                                                string sNextFileFar = NextFileProductSplit[0];
                                                //Console.WriteLine("DEBUG sNextFileFar0=" + sNextFileFar);
                                                if (sNextFileFar == sCurrentFile)
                                                {

                                                }
                                                else
                                                {
                                                    if (sCurrentMainProductNameToTile != "" && sNextFileProduct.ToLower().Contains(sCurrentMainProductNameToTile.ToLower()))
                                                    {
                                                        //The next file/version is different BUT it's for the same MainProduct
                                                        iCounterDifferentPlatforms += 1;
                                                    }
                                                    break;
                                                }
                                            }
                                            #endregion countdifferentplatforms
                                        }
                                        else
                                        {
                                            //sCriteriaComment = new CultureInfo("en-US").TextInfo.ToTitleCase(sCurrentProduct).Replace("Sp ", "SP "); ;
                                            //if (!bWroteCriteria3) Console.WriteLine("DEBUG sCriteriaComment1=" + sCriteriaComment);
                                        }
                                    }
                                }
                            }
                            Console.WriteLine("DEBUG iCounterCorrectProductsTotalDef=" + iCounterCorrectProductsTotalDef);
                            Console.WriteLine("DEBUG iCounterDifferentPlatforms=" + iCounterDifferentPlatforms);
                            #endregion countcorrectproducts

                            if (sMainProductNameToTitle != "1337" && sCurrentMainProductNameToTile.ToUpper() != "JOKER")
                            {
                                #region fixtab
                                //If something went wrong i.e. OVALInventoryDefinition not found...
                                //FIXTAB
                                //if(bWroteCriteria0 && sTab.Length==0)
                                //{
                                Console.WriteLine("DEBUG FIXTAB");
                                if (bDEBUGMODEINXML) monStreamWriter.WriteLine("DEBUG FIXTAB");
                                sTab = "";
                                if (bWroteCriteria0) sTab = sTab + "\t";
                                if (bWroteCriteria1Global) sTab = sTab + "\t";
                                if (bWroteCriteria2Global) sTab = sTab + "\t";

                                //}
                                #endregion fixtab

                                string sCriteria1Comment = sMainProductNameToTitle;
                                if (bFileCorrectProductContainsWindows) sCriteria1Comment = sCriteria1Comment + " + Windows OS";
                                sCriteria1Comment = sCriteria1Comment + " + file version";
                                //sMainProductNameToTitle + " + Windows OS + file version
                                Console.WriteLine("DEBUG Write Criteria1 sMainProductNameToTitle!=1337 CriteriaMainProductNameToTitle " + sCriteria1Comment);
                                if (bDEBUGMODEINXML) monStreamWriter.WriteLine("DEBUG Write Criteria1 sMainProductNameToTitle!=1337");
                                if (bDEBUGMODEINXML) sDebug = "1";
                                monStreamWriter.WriteLine(sTab + "\t<oval-def:criteria" + sDebug + " comment=\"" + sCriteria1Comment + "\" operator=\"AND\">");
                                //<oval-def:extend_definition comment="Microsoft Edge is installed" definition_ref="oval:org.cisecurity:def:2" />
                                if (iCounterMainProductsProcessed == 1) bWroteCriteria1Global = true;
                                sSaveTabCriteria1 = sTab + "\t";

                            }
                            else
                            {
                                Console.WriteLine("DEBUG NOT Writing Criteria1 sMainProductNameToTitle=1337 or JOKER");
                                if (bDEBUGMODEINXML) monStreamWriter.WriteLine("DEBUG NOT Writing Criteria1 sMainProductNameToTitle=1337 or JOKER");
                                sSaveTabCriteria1 = sTab;
                                /*
                                if(bWroteCriteria0)
                                {
                                    sTab = "\t\t";
                                    Console.WriteLine("DEBUG sTab is two TABs");
                                }
                                else
                                {
                                    sTab = "\t";
                                    Console.WriteLine("DEBUG sTab is one TAB");
                                }
                                */
                            }


                            #region OVALInventoryDefinitionForMainProduct
                            //1st try   (will work with "Edition")
                            OVALDEFINITION oOVALInventoryDefinitionForMainProduct = new OVALDEFINITION();
                            if (sMainProductNameToTitle != "1337" && sCurrentMainProductNameToTile != "joker")    //HARDCODED
                            {
                                try
                                {
                                    //TODO: Use List/Dictionary 
                                    if (!lOVALDefInventoryNotRetrieved.Contains(sCurrentMainProductNameToTile))
                                    {

                                        //oOVALInventoryDefinitionForMainProduct = oval_model.OVALDEFINITION.Where(o => o.OVALDefinitionTitle.ToLower().Contains(sMainProductName) && o.OVALDefinitionTitle.ToLower().Contains(" is installed")).OrderByDescending(o => o.OVALDefinitionVersion).FirstOrDefault(); //Hardcoded
                                        oOVALInventoryDefinitionForMainProduct = oval_model.OVALDEFINITION.Where(o => o.OVALClassEnumerationID == iOVALClassEnumerationInventoryID && o.OSFamilyID == iOSFamilyWindowsID && o.OVALDefinitionTitle.ToLower().Contains(sCurrentMainProductNameToTile) && o.OVALDefinitionTitle.ToLower().Contains(" is installed")).OrderByDescending(o => o.OVALDefinitionVersion).FirstOrDefault(); //Hardcoded
                                        if (oOVALInventoryDefinitionForMainProduct == null && sCurrentMainProductNameToTile.Contains("microsoft sharepoint"))   //HARDCODED
                                        {
                                            oOVALInventoryDefinitionForMainProduct = oval_model.OVALDEFINITION.Where(o => o.OVALClassEnumerationID == iOVALClassEnumerationInventoryID && o.OSFamilyID == iOSFamilyWindowsID && o.OVALDefinitionTitle.ToLower().Contains(sCurrentMainProductNameToTile.Replace("microsoft sharepoint", "microsoft office sharepoint")) && o.OVALDefinitionTitle.ToLower().Contains(" is installed")).OrderByDescending(o => o.OVALDefinitionVersion).FirstOrDefault(); //Hardcoded
                                            if (oOVALInventoryDefinitionForMainProduct == null)
                                            {
                                                oOVALInventoryDefinitionForMainProduct = oval_model.OVALDEFINITION.Where(o => o.OVALClassEnumerationID == iOVALClassEnumerationInventoryID && o.OSFamilyID == iOSFamilyWindowsID && o.OVALDefinitionTitle.ToLower().Contains(sCurrentMainProductNameToTile.Replace("microsoft sharepoint", "microsoft office sharepoint").Replace(" service pack ", " sp")) && o.OVALDefinitionTitle.ToLower().Contains(" is installed")).OrderByDescending(o => o.OVALDefinitionVersion).FirstOrDefault(); //Hardcoded
                                            }
                                        }
                                        if (oOVALInventoryDefinitionForMainProduct == null)
                                        {
                                            oOVALInventoryDefinitionForMainProduct = oval_model.OVALDEFINITION.Where(o => o.OVALClassEnumerationID == iOVALClassEnumerationInventoryID && o.OSFamilyID == iOSFamilyWindowsID && o.OVALDefinitionTitle.ToLower().Contains(sCurrentMainProductNameToTile.Replace(" service pack ", " sp")) && o.OVALDefinitionTitle.ToLower().Contains(" is installed")).OrderByDescending(o => o.OVALDefinitionVersion).FirstOrDefault(); //Hardcoded
                                        }
                                        if (oOVALInventoryDefinitionForMainProduct == null)
                                        {
                                            if (!lOVALDefInventoryNotRetrieved.Contains(sCurrentMainProductNameToTile.ToLower()))
                                            {
                                                oOVALInventoryDefinitionForMainProduct = fGetOVALInventoryDefinitionForProduct(sCurrentMainProductNameToTile);
                                            }
                                        }

                                        if (oOVALInventoryDefinitionForMainProduct != null)
                                        {
                                            Console.WriteLine("DEBUG Write OVALInventoryDefinitionForMainProduct " + oOVALInventoryDefinitionForMainProduct.OVALDefinitionTitle);
                                            if (bDEBUGMODEINXML) monStreamWriter.WriteLine("DEBUG Write OVALInventoryDefinitionForMainProduct");
                                            //extend_definition1
                                            if (bDEBUGMODEINXML) sDebug = "1";
                                            monStreamWriter.WriteLine(sTab + "\t\t<oval-def:extend_definition" + sDebug + " comment=\"" + oOVALInventoryDefinitionForMainProduct.OVALDefinitionTitle + "\" definition_ref=\"" + oOVALInventoryDefinitionForMainProduct.OVALDefinitionIDPattern + "\" />");
                                            //iCounterExtendDefinition2Wrote++;
                                        }
                                        else
                                        {
                                            //Console.WriteLine("ERROR: OVALInventoryDefinitionForMainProduct NOT FOUND for: " + sMainProductName);
                                            Console.WriteLine("ERROR: OVALInventoryDefinitionForMainProduct NOT FOUND for: " + sCurrentMainProductNameToTile);
                                            if (bDEBUGMODEINXML) monStreamWriter.WriteLine("ERROR: OVALInventoryDefinitionForMainProduct NOT FOUND for: " + sCurrentMainProductNameToTile);
                                        }
                                    }
                                }
                                catch (Exception exoOVALInventoryDefinitionForMainProduct)
                                {
                                    Console.WriteLine("Exception: exoOVALInventoryDefinitionForMainProduct " + exoOVALInventoryDefinitionForMainProduct.Message + " " + exoOVALInventoryDefinitionForMainProduct.InnerException);
                                }
                            }
                            else
                            {

                            }
                            #endregion OVALInventoryDefinitionForMainProduct

                            if (sMainProductNameToTitle != "1337" && sCurrentMainProductNameToTile != "joker" && iCounterCorrectProductsTotalDef > 1)
                            {
                                Console.WriteLine("DEBUG Adding Tab1 !1337 !joker iCounterCorrectProductsTotalDef>1");
                                if (bDEBUGMODEINXML) monStreamWriter.WriteLine("DEBUG Adding Tab1 !1337 iCounterCorrectProductsTotalDef>1");
                                sTab += "\t";
                            }
                            else
                            {
                                Console.WriteLine("DEBUG NOT Adding Tab1 iCounterCorrectProductsTotalDef>1");
                                if (bDEBUGMODEINXML) monStreamWriter.WriteLine("DEBUG NOT Adding Tab1 iCounterCorrectProductsTotalDef>1");

                            }
                        }
                        #endregion sMainProductNameToTitle

                        #region criteria2

                        if (iCounterCorrectProductsTotalDef > 1)
                        {
                            if (sMainProductNameToTitle != "1337" && sCurrentMainProductNameToTile != "joker" && iCounterDifferentPlatforms < 1)  //sMainProductNameToTitle != "" && 
                            {
                                if (sTab.Length > 0)
                                {
                                    Console.WriteLine("DEBUG Removing Tab1 iCounterCorrectProductsTotalDef>1 iCounterDifferentPlatforms<1");
                                    if (bDEBUGMODEINXML) monStreamWriter.WriteLine("DEBUG Removing Tab1 iCounterCorrectProductsTotalDef>1 iCounterDifferentPlatforms<1");
                                    sTab = sTab.Remove(sTab.Length - 1);
                                }
                                else
                                {
                                    Console.WriteLine("NOTE: Cannot Remove Tab1");
                                }
                                Console.WriteLine("DEBUG NOT WritingCriteria2 sMainProductNameToTitle!=1337 iCounterDifferentPlatforms<1");
                                if (bDEBUGMODEINXML) monStreamWriter.WriteLine("DEBUG NOT WritingCriteria2 sMainProductNameToTitle!=1337 iCounterDifferentPlatforms<1");
                            }
                            else
                            {
                                #region writecriteria2
                                if (bDEBUGMODEINXML) sDebug = "2";
                                if (lFileProduct[0].Contains("windows"))
                                {
                                    if (bDEBUGMODEINXML) monStreamWriter.WriteLine("DEBUG Write Criteria2 windows iCounterCorrectProductsTotalDef>1 iCounterDifferentPlatforms>=1?");
                                    Console.WriteLine("DEBUG Write Criteria2 windows iCounterCorrectProductsTotalDef>1 iCounterDifferentPlatforms>=1? CriteriaVulnWindowsFileVersion Check for installation of vulnerable Windows OS + vulnerable file version");
                                    monStreamWriter.WriteLine(sTab + "\t<oval-def:criteria" + sDebug + " comment=\"Check for installation of vulnerable Windows OS + vulnerable file version\" operator=\"OR\">");
                                }
                                else
                                {
                                    if (bDEBUGMODEINXML) monStreamWriter.WriteLine("DEBUG Write Criteria2 not windows");
                                    Console.WriteLine("DEBUG Write Criteria2 not windows iCounterCorrectProductsTotalDef>1 iCounterDifferentPlatforms>=1? CriteriaVulnProductFileVersion Check for installation of vulnerable product + vulnerable file version");
                                    monStreamWriter.WriteLine(sTab + "\t<oval-def:criteria" + sDebug + " comment=\"Check for installation of vulnerable product + vulnerable file version\" operator=\"OR\">");
                                }
                                Console.WriteLine("DEBUG iCounterMainProductsProcessed=" + iCounterMainProductsProcessed);
                                if (iCounterMainProductsProcessed == 1)
                                {
                                    bWroteCriteria2Global = true;
                                    Console.WriteLine("DEBUG bWroteCriteria2Global=true");
                                    sSaveTabCriteria2Global = sTab;
                                }
                                bWroteCriteria2 = true;
                                //Console.WriteLine("DEBUG bWroteCriteria2=true");
                                sSaveTabCriteria2 = sTab;

                                /*
                                if(1==2 && iCounterDifferentPlatforms>0)    //TODO
                                {
                                    Console.WriteLine("DEBUG Adding Tab34");
                                    if (bDEBUGMODEINXML) monStreamWriter.WriteLine("DEBUG Adding Tab34");
                                    //if(lMainProductsNames.Count > 1)
                                    sTab = sTab + "\t";
                                }
                                else
                                {
                                    //if(lMainProductsNames.Count > 1)
                                    Console.WriteLine("DEBUG NOT Adding Tab34");
                                    if (bDEBUGMODEINXML) monStreamWriter.WriteLine("DEBUG NOT Adding Tab34");
                                }
                                */
                                #endregion writecriteria2
                            }
                        }
                        else
                        {
                            Console.WriteLine("DEBUG NOT Writing Criteria2 iCounterCorrectProductsTotalDef=1");
                            if (bDEBUGMODEINXML) monStreamWriter.WriteLine("DEBUG NOT Writing Criteria2 iCounterCorrectProductsTotalDef=1");
                        }
                        #endregion criteria2

                        lFileProduct = lFileProduct.OrderByDescending(o => o).ToList(); //For grouping by file version
                                                                                        //string sCurrentFile = "";
                                                                                        //bool bCurrentFileChanged = false;

                        bool bWroteCriteria3 = false;
                        string sSaveTabCriteria3 = "";  //For EndCriteria3
                        bool bWroteCriteria4 = false;
                        string sSaveTabCriteria4 = "";  //For EndCriteria4
                        bool bWroteCriterion = false;

                        string sCriteriaComment = "";
                        bool bBypassBlock = false;              //In case we used the "Best Match" and because we don't want duplicated extend_definition in one criteria (i.e. the same for x86+x64)
                        int iCounterExtendDefinition2Wrote = 0;
                        List<string> lOVALExtendDefinitionsWritten = new List<string>();    //Due to bug    microsoft .net framework 4.5    vs  microsoft .net framework 4.5.1

                        //foreach (var x in dProductFile.OrderBy(o => o.Key))
                        //foreach (var x in dFileProduct.OrderBy(o => o.Key))
                        //foreach (string sFileProduct in lFileProduct.OrderByDescending(o=>o))

                        int iCounterCorrectProducts = 0;
                        for (int i = 0; i < lFileProduct.Count; i++)
                        {
                            bool bBypassExtendDef = false;      //In case we used the "Best Match" and because we don't want duplicated extend_definition in one criteria

                            string sFileProduct = lFileProduct[i];
                            if (sCurrentMainProductNameToTile != "" && sCurrentMainProductNameToTile != "1337" && !sFileProduct.ToLower().Contains(sCurrentMainProductNameToTile.ToLower()))
                            {
                                //TODO: microsoft .net framework 4.5    vs  microsoft .net framework 4.5.1
                                Console.WriteLine("DEBUG Bypass the sFileProduct=" + lFileProduct[i] + " does not matching sCurrentMainProductNameToTile=" + sCurrentMainProductNameToTile);
                                continue;
                            }
                            if (iCounterCorrectProducts == 0) iCounterCorrectProducts = 1;

                            //Console.WriteLine("DEBUG sFileProduct=" + sFileProduct);
                            string[] FileProductSplit = sFileProduct.Split(new string[] { "JEROME" }, StringSplitOptions.None); //Hardcoded :-)
                            string sCurrentFile = FileProductSplit[0];
                            Console.WriteLine("DEBUG sCurrentFile1=" + sCurrentFile);

                            //Console.WriteLine("DEBUG sCurrentFile1=" + sCurrentFile);
                            string sCurrentProduct = FileProductSplit[1];   //Microsoft Windows 10 Version 1511 (x86)

                            Console.WriteLine("DEBUG sCurrentProduct1=" + sCurrentProduct);

                            string[] CurrenProductSplitDEADBEEF = sCurrentProduct.Split(new string[] { "DEADBEEF" }, StringSplitOptions.None);

                            if (CurrenProductSplitDEADBEEF[1] != "")
                            {
                                sCurrentProduct = CurrenProductSplitDEADBEEF[1];
                            }
                            else
                            {
                                sCurrentProduct = CurrenProductSplitDEADBEEF[0];

                            }

                            #region analyzenextfileproduct
                            string sNextFile = "";
                            string sNextProduct = "";
                            //string sNextMainProduct = "";   //=Platform?
                            #region hardcodedforcosmetic
                            bool bIdenticalProductsForCriteria = false; //For cosmetic
                            int iIdenticalProductsForCriteria = 0;
                            if (sCurrentProduct.Contains("ersion 1511")) iIdenticalProductsForCriteria = 1;
                            if (sCurrentProduct.Contains("ersion 1607")) iIdenticalProductsForCriteria = 1;
                            if (sCurrentProduct.Contains("ersion 1703")) iIdenticalProductsForCriteria = 1;
                            //TODO  Service Pack
                            #endregion hardcodedforcosmetic
                            if (i == lFileProduct.Count - 1)
                            {
                                //End of the list
                                if (lFileProduct.Count == 1)    //Only 1 file/1 product in the Definition
                                {
                                    //iCounterCorrectProducts==1;
                                    sCriteriaComment = fGetPlatformName(sCurrentProduct, true);
                                    sCriteriaComment = new CultureInfo("en-US").TextInfo.ToTitleCase(sCriteriaComment).Replace("Sp ", "SP "); //hardcoded
                                }
                            }
                            else
                            {
                                string sNextFileProduct = lFileProduct[i + 1];
                                string[] NextFileProductSplit = sNextFileProduct.Split(new string[] { "JEROME" }, StringSplitOptions.None); //Hardcoded :-)
                                sNextFile = NextFileProductSplit[0];
                                Console.WriteLine("DEBUG sNextFile=" + sNextFile);
                                if (sNextFile == sCurrentFile && !bWroteCriteria3)
                                {

                                    //if (!bWroteCriteria3)
                                    //{
                                    //Retrieve (in advance) all the Products with the same file/version for the criteria comment
                                    //TODO: add them in a list for later (when looking for the Path in OVALVARIABLEs)
                                    #region buildcriteriacomment
                                    for (int y = i; y < lFileProduct.Count - 1; y++)
                                    {
                                        //Microsoft Windows 10 Version 1511 (x86)
                                        if (y == i) sCriteriaComment = fGetPlatformName(sCurrentProduct, true); //Windows 10
                                        sNextFileProduct = lFileProduct[y + 1];
                                        //Console.WriteLine("DEBUG sNextFileProductFar=" + sNextFileProduct);
                                        NextFileProductSplit = sNextFileProduct.Split(new string[] { "JEROME" }, StringSplitOptions.None); //Hardcoded :-)
                                        string sNextFileFar = NextFileProductSplit[0];
                                        //Console.WriteLine("DEBUG sNextFileFar=" + sNextFileFar);
                                        if (sNextFileFar == sCurrentFile)
                                        {
                                            //Console.WriteLine("DEBUG sNextFileFar == sCurrentFile");
                                            sNextProduct = NextFileProductSplit[1];
                                            string[] NextProductSplitDEADBEEF = sNextProduct.Split(new string[] { "DEADBEEF" }, StringSplitOptions.None);
                                            sNextProduct = NextProductSplitDEADBEEF[1];
                                            Console.WriteLine("DEBUG sNextProductSplitDEADBEEF=" + sNextProduct);
                                            string sNextProductPlatform = fGetPlatformName(sNextProduct, true); //Microsoft Windows 10 Version 1511 (x64)
                                            Console.WriteLine("DEBUG sNextProductfGetPlatformName=" + sNextProductPlatform);    //Windows 10
                                            if (sCurrentMainProductNameToTile != "" && sNextProductPlatform.ToLower().Contains(sCurrentMainProductNameToTile.ToLower()))
                                            {
                                                //i.e.: internet explorer 9 contains internet explorer
                                                //Bypass
                                            }
                                            else
                                            {
                                                //TODO Enhance  HARDCODED
                                                //office
                                                //.net framework
                                                bool bBypassProduct = false;
                                                if (sCurrentMainProductNameToTile.Contains("internet explorer") && sNextProductPlatform.ToLower().Contains("internet explorer"))
                                                {
                                                    //i.e.: internet explorer 9   vs  internet explorer 11
                                                    bBypassProduct = true;
                                                }
                                                //TODO REVIEW
                                                if (sCurrentMainProductNameToTile.Contains("jscript") && sNextProductPlatform.ToLower().Contains("jscript")) bBypassProduct = true;     //hardcoded
                                                if (sCurrentMainProductNameToTile.Contains("vbscript") && sNextProductPlatform.ToLower().Contains("vbscript")) bBypassProduct = true;   //hardcoded

                                                if (!bBypassProduct)
                                                {
                                                    iCounterCorrectProducts++;
                                                    Console.WriteLine("DEBUG iCounterCorrectProductsHERE=" + iCounterCorrectProducts);
                                                    if (!sCriteriaComment.Contains(sNextProductPlatform))
                                                    {
                                                        ////TODO Windows Server 2016/Windows 10 || Windows 10/Windows Server 2016 ==> Windows 10 Version 1607/Windows Server 2016
                                                        sCriteriaComment = sCriteriaComment + "/" + sNextProductPlatform;
                                                        //Console.WriteLine("DEBUG sCriteriaComment2=" + sCriteriaComment);
                                                    }
                                                    else
                                                    {

                                                        //Hardcoded for cosmetic
                                                        if (sCurrentProduct.Contains("ersion 1511") && sNextProduct.Contains("ersion 1511")) iIdenticalProductsForCriteria++;
                                                        if ((sCurrentProduct.Contains("windows server 2016") || sCurrentProduct.Contains("ersion 1607")) && sNextProduct.Contains("ersion 1607")) iIdenticalProductsForCriteria++;
                                                        //1703
                                                    }
                                                }
                                            }
                                        }
                                        else//The next file/version is different
                                        {
                                            if (sCurrentMainProductNameToTile != "" && sNextFileProduct.ToLower().Contains(sCurrentMainProductNameToTile.ToLower()))
                                            {
                                                //The next file/version is different BUT it's for the same MainProduct
                                                iCounterDifferentPlatforms += 1;
                                            }
                                            break;  //TODO Review
                                        }
                                    }

                                    sCriteriaComment = new CultureInfo("en-US").TextInfo.ToTitleCase(sCriteriaComment).Replace("Sp ", "SP ").Replace(" Service Pack ", " SP");  //Hardcoded cosmetic
                                    #region hardcodedforcosmetic2
                                    Console.WriteLine("DEBUG iIdenticalProductsForCriteria=" + iIdenticalProductsForCriteria);
                                    if (iIdenticalProductsForCriteria == 2)
                                    {
                                        if (sCriteriaComment == "Windows 10")
                                        {
                                            if (sCurrentProduct.Contains("ersion 1511")) sCriteriaComment = "Windows 10 Version 1511";
                                            if (sCurrentProduct.Contains("ersion 1607")) sCriteriaComment = "Windows 10 Version 1607";
                                            if (sCurrentProduct.Contains("ersion 1703")) sCriteriaComment = "Windows 10 Version 1703";

                                        }
                                        else
                                        {
                                            if (sCriteriaComment == "Windows Server 2016/Windows 10") sCriteriaComment = "Windows 10 Version 1607/Windows Server 2016";
                                        }
                                    }
                                    #endregion hardcodedforcosmetic2

                                    Console.WriteLine("DEBUG sCriteriaComment3=" + sCriteriaComment);

                                    #endregion buildcriteriacomment
                                }
                                else
                                {
                                    sCriteriaComment = new CultureInfo("en-US").TextInfo.ToTitleCase(sCurrentProduct);
                                    sCriteriaComment = sCriteriaComment.Replace("Sp ", "SP ");  //Hardcoded cosmetic
                                    if (!bWroteCriteria3)
                                    {
                                        Console.WriteLine("DEBUG sCriteriaComment1=" + sCriteriaComment);
                                        Console.WriteLine("DEBUG iCounterCorrectProducts1=" + iCounterCorrectProducts);
                                        //if (bDEBUGMODEINXML) monStreamWriter.WriteLine("DEBUG sCriteriaComment1");
                                    }
                                }
                            }
                            #endregion analyzenextfileproduct

                            bool bSkipExtendDefinition = false;
                            if (sCurrentMainProductNameToTile != "" && sCurrentProduct.ToLower().Contains(sCurrentMainProductNameToTile.ToLower()))
                            {
                                //internet explorer 9 contains internet explorer
                                Console.WriteLine("DEBUG Bypass the sCurrentProduct=" + sCurrentProduct + " matching sCurrentMainProductNameToTile=" + sCurrentMainProductNameToTile);
                                //continue;
                                //bBypassExtendDef = true;
                                bSkipExtendDefinition = true;
                            }
                            else
                            {

                            }

                            string sProductName = new CultureInfo("en-US").TextInfo.ToTitleCase(sCurrentProduct).Replace(" Sp", " SP").Replace(" Rt", "RT");   //Hardcoded cosmetic
                            sProductName = sProductName.Replace("X86", "x86");   //Hardcoded cosmetic
                            sProductName = sProductName.Replace("X64", "x64");   //Hardcoded cosmetic

                            string sProductNameModified = sProductName.ToLower();
                            //TODO Lazy Patch
                            sProductNameModified = sProductNameModified.Replace("windows 10 version 1703 (64-bit)", "windows 10 version 1703 (x64)");
                            string sOVALDefInventoryTitle = sProductName + " is installed"; //Hardcoded

                            //Retrieve the OVAL Inventory Definition ID for the current Product
                            OVALDEFINITION oOVALInventoryDefinition = new OVALDEFINITION();
                            if (!lOVALDefInventoryNotRetrieved.Contains(sProductNameModified.ToLower()))
                            {
                                oOVALInventoryDefinition = fGetOVALInventoryDefinitionForProduct(sProductNameModified);
                            }
                            if (oOVALInventoryDefinition == null) Console.WriteLine("ERROR: OVALDefInventory not retrieved for sOVALDefInventoryTitle=" + sOVALDefInventoryTitle);
                            /*
                            if (!bSkipExtendDefinition)
                            {

                            }
                            */
                            #region ovalinventorydefinitionnotnull
                            if ((oOVALInventoryDefinition != null || bSkipExtendDefinition) && !bBypassBlock)
                            {
                                //If we have multiple products with the same file version
                                if (sNextFile == sCurrentFile && !bBypassExtendDef && !bSkipExtendDefinition)
                                {
                                    //NOTE: we assume we will find the next oOVALInventoryDefinition!
                                    if (!bWroteCriteria3)
                                    {
                                        if (iCounterCorrectProducts > 1 || lFileProduct.Count == 1)
                                        {
                                            ////monStreamWriter.WriteLine("\t\t<oval-def:criteria comment=\"" + sProductName + " + file version\" operator=\"AND\">");
                                            //monStreamWriter.WriteLine("\t\t<oval-def:criteria comment=\"Product is installed + file version\" operator=\"AND\">");
                                            Console.WriteLine("DEBUG Write Criteria3 " + sCriteriaComment.Replace(" Sp ", " SP ") + " is installed + file version");
                                            if (bDEBUGMODEINXML) monStreamWriter.WriteLine("DEBUG Write Criteria3");
                                            if (bDEBUGMODEINXML) sDebug = "3";
                                            monStreamWriter.WriteLine(sTab + "\t\t<oval-def:criteria" + sDebug + " comment=\"" + sCriteriaComment.Replace(" Sp ", " SP ") + " is installed + file version\" operator=\"AND\">");
                                            sSaveTabCriteria3 = sTab;

                                            Console.WriteLine("DEBUG Adding Tab3");
                                            if (bDEBUGMODEINXML) monStreamWriter.WriteLine("DEBUG Adding Tab3");
                                            sTab = sTab + "\t";
                                            Console.WriteLine("DEBUG bWroteCriteria3=true");
                                            if (bDEBUGMODEINXML) monStreamWriter.WriteLine("DEBUG bWroteCriteria3=true");
                                            bWroteCriteria3 = true;
                                        }
                                        else
                                        {
                                            Console.WriteLine("DEBUG NOT Writing Criteria3 !bWroteCriteria3 iCounterCorrectProducts<=1");
                                            if (bDEBUGMODEINXML) monStreamWriter.WriteLine("DEBUG NOT Writing Criteria3 !bWroteCriteria3 iCounterCorrectProducts<=1");
                                        }
                                    }
                                }
                                else
                                {
                                    /*
                                    Console.WriteLine("DEBUG bSkipExtendDefinition = " + bSkipExtendDefinition);
                                    if (!bSkipExtendDefinition)
                                    {
                                        Console.WriteLine("DEBUG bWroteCriteria3=false sNextFile!=sCurrentFile or bBypassExtendDef or bSkipExtendDefinition");
                                        if (bDEBUGMODEINXML) monStreamWriter.WriteLine("DEBUG bWroteCriteria3=false sNextFile!=sCurrentFile or bBypassExtendDef or bSkipExtendDefinition");
                                        Console.WriteLine("DEBUG bWroteCriteria3=false");
                                        if (bDEBUGMODEINXML) monStreamWriter.WriteLine("DEBUG bWroteCriteria3=false");
                                        bWroteCriteria3 = false;
                                    }
                                    */
                                }

                                #region criteria4
                                if (!bWroteCriteria4)
                                {
                                    if (bWroteCriteria3)
                                    {
                                        Console.WriteLine("DEBUG Write Criteria4 !bWroteCriteria4 bWroteCriteria3 " + sCriteriaComment.Replace(" Sp ", " SP ") + " is installed");
                                        if (bDEBUGMODEINXML) monStreamWriter.WriteLine("DEBUG Write Criteria4 !bWroteCriteria4 bWroteCriteria3");
                                        if (bDEBUGMODEINXML) sDebug = "4";
                                        monStreamWriter.WriteLine(sTab + "\t\t<oval-def:criteria" + sDebug + " comment=\"" + sCriteriaComment.Replace(" Sp ", " SP ") + " is installed\" operator=\"OR\">");
                                        sSaveTabCriteria4 = sTab;
                                        Console.WriteLine("DEBUG bWroteCriteria4=true");
                                        if (bDEBUGMODEINXML) monStreamWriter.WriteLine("DEBUG bWroteCriteria4=true");
                                        bWroteCriteria4 = true;
                                    }
                                    else
                                    {
                                        if (bBypassExtendDef)
                                        {
                                            Console.WriteLine("DEBUG Write Criteria4b !bWroteCriteria4 !bWroteCriteria3 bBypassExtendDef");
                                            if (bDEBUGMODEINXML) monStreamWriter.WriteLine("DEBUG Write Criteria4b !bWroteCriteria4 !bWroteCriteria3 bBypassExtendDef");
                                            if (bDEBUGMODEINXML) sDebug = "4";
                                            monStreamWriter.WriteLine(sTab + "\t\t<oval-def:criteria" + sDebug + " comment=\"" + sProductName.Replace(" Sp ", " SP ").Replace(" x86", "").Replace(" x64", "") + " + file version\" operator=\"AND\">");
                                            sSaveTabCriteria4 = sTab;
                                            Console.WriteLine("DEBUG bWroteCriteria4=true");
                                            if (bDEBUGMODEINXML) monStreamWriter.WriteLine("DEBUG bWroteCriteria4=true");
                                            bWroteCriteria4 = true;

                                        }
                                        else
                                        {
                                            if (bSkipExtendDefinition)
                                            {
                                                Console.WriteLine("DEBUG NOT Writing Criteria4b NOT !bBypassExtendDef bSkipExtendDefinition");

                                                if (iCounterCorrectProducts == 1)
                                                {
                                                    if (sTab.Length > 0)
                                                    {
                                                        Console.WriteLine("DEBUG Removing Tab4b");
                                                        if (bDEBUGMODEINXML) monStreamWriter.WriteLine("DEBUG Removing Tab4b");
                                                        sTab = sTab.Remove(sTab.Length - 1);
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("NOTE: Cannot Remove Tab4b");
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (iCounterCorrectProductsTotalDef == 1)
                                                {
                                                    Console.WriteLine("DEBUG NOT Writing Criteria4c iCounterCorrectProductsTotalDef=1 !bBypassExtendDef");
                                                    if (bDEBUGMODEINXML) monStreamWriter.WriteLine("DEBUG NOT Writing Criteria4c iCounterCorrectProductsTotalDef=1 !bBypassExtendDef");
                                                    if (sTab.Length > 0)
                                                    {
                                                        Console.WriteLine("DEBUG Removing Tab4c iCounterCorrectProductsTotalDef=1");
                                                        if (bDEBUGMODEINXML) monStreamWriter.WriteLine("DEBUG Removing Tab4c iCounterCorrectProductsTotalDef=1");
                                                        sTab = sTab.Remove(sTab.Length - 1);
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("NOTE: Cannot Remove Tab4c");
                                                    }
                                                }
                                                else
                                                {
                                                    Console.WriteLine("DEBUG Write Criteria4d iCounterCorrectProducts>1 !bBypassExtendDef");
                                                    if (bDEBUGMODEINXML) monStreamWriter.WriteLine("DEBUG Write Criteria4d iCounterCorrectProducts>1 !bBypassExtendDef");
                                                    if (bDEBUGMODEINXML) sDebug = "4";
                                                    monStreamWriter.WriteLine(sTab + "\t\t<oval-def:criteria" + sDebug + " comment=\"" + sProductName.Replace(" Sp ", " SP ") + " + file version\" operator=\"AND\">");
                                                    sSaveTabCriteria4 = sTab;
                                                    Console.WriteLine("DEBUG bWroteCriteria4=true");
                                                    if (bDEBUGMODEINXML) monStreamWriter.WriteLine("DEBUG bWroteCriteria4=true");
                                                    bWroteCriteria4 = true;
                                                }
                                            }
                                        }
                                    }
                                }
                                #endregion criteria4

                                //if (!bBypassExtendDef)
                                //{
                                if (!bSkipExtendDefinition && !lOVALExtendDefinitionsWritten.Contains(oOVALInventoryDefinition.OVALDefinitionIDPattern))
                                {

                                    //extend_definition2
                                    if (bDEBUGMODEINXML) sDebug = "2";
                                    Console.WriteLine("DEBUG Write extend_definition NOT bSkipExtendDefinition");
                                    iCounterExtendDefinition2Wrote++;
                                    monStreamWriter.WriteLine(sTab + "\t\t\t<oval-def:extend_definition" + sDebug + " comment=\"" + oOVALInventoryDefinition.OVALDefinitionTitle + "\" definition_ref=\"" + oOVALInventoryDefinition.OVALDefinitionIDPattern + "\" />");
                                    lOVALExtendDefinitionsWritten.Add(oOVALInventoryDefinition.OVALDefinitionIDPattern);
                                }
                                //}
                                //else
                                //{
                                //    if (!bWroteCriteria4) monStreamWriter.WriteLine(sTab + "\t\t\t<oval-def:extend_definition comment=\"" + oOVALInventoryDefinition.OVALDefinitionTitle + "\" definition_ref=\"" + oOVALInventoryDefinition.OVALDefinitionIDPattern + "\" />");
                                //}

                                if (sNextFile != sCurrentFile || bSkipExtendDefinition)// || (sNextProduct != sCurrentProduct && !(sCurrentProduct.Contains("windows") && sNextProduct.Contains("windows"))))// || bBypassExtendDef)
                                {
                                    if (bWroteCriteria4 && iCounterExtendDefinition2Wrote != 1)
                                    {
                                        sTab = sSaveTabCriteria4;
                                        Console.WriteLine("DEBUG WriteEnd Criteria4 sTab.Length=" + sTab.Length);
                                        if (bDEBUGMODEINXML) monStreamWriter.WriteLine("DEBUG WriteEnd Criteria4 sTab.Length=" + sTab.Length);
                                        if (bDEBUGMODEINXML) sDebug = "40";
                                        monStreamWriter.WriteLine(sTab + "\t\t</oval-def:criteria" + sDebug + ">");   //Criteria4

                                    }
                                    else
                                    {
                                        Console.WriteLine("DEBUG NOT Writing EndCriteria4 !bWroteCriteria4 !bWroteCriterion");
                                        if (bDEBUGMODEINXML) monStreamWriter.WriteLine("DEBUG NOT Writing EndCriteria4 !bWroteCriteria4");
                                    }

                                    Console.WriteLine("DEBUG iCounterCorrectProducts2=" + iCounterCorrectProducts);
                                    Console.WriteLine("DEBUG iCounterExtendDefinition2Wrote2=" + iCounterExtendDefinition2Wrote);

                                    if (iCounterCorrectProducts != 1)// || iCounterExtendDefinition2Wrote!=1) //sMainProductName != "" && 
                                    {
                                        //For the criterion
                                        if (sTab.Length > 0)
                                        {
                                            Console.WriteLine("DEBUG Removing TabX iCounterCorrectProducts!=1");
                                            if (bDEBUGMODEINXML) monStreamWriter.WriteLine("DEBUG Removing TabX iCounterCorrectProducts!=1");
                                            sTab = sTab.Remove(sTab.Length - 1);
                                        }
                                        /*
                                        if(iCounterDifferentPlatforms>1)
                                        {
                                            Console.WriteLine("DEBUG sTab+ iCounterDifferentPlatforms>1");
                                            if (bDEBUGMODEINXML) monStreamWriter.WriteLine("DEBUG sTab+ iCounterDifferentPlatforms>1");
                                            sTab = sTab + "\t";
                                        }
                                        */
                                    }
                                    else
                                    {
                                        //if (bDEBUGMODEINXML) monStreamWriter.WriteLine("DEBUG sTab empty iCounterCorrectProducts=1");
                                        //Console.WriteLine("DEBUG sTab empty iCounterCorrectProducts=1");
                                        //sTab = "";
                                    }

                                    #region criterion
                                    string[] FileInfos = FileProductSplit[0].Split(' ');
                                    //wwlibcxm.dll 14.0.7177.5000
                                    //WARNING: No file found!
                                    //Check if excel.exe version is less than 15.0.4885.1000
                                    string sFileName = FileInfos[0];
                                    string sFileVersion = FileInfos[1];
                                    string sTestComment = "Check if " + sFileName + " version is less than " + sFileVersion;    //Hardcoded
                                    StreamWriter myStreamWriterOVALTEST = null;
                                    //Check if this Test already exists
                                    OVALTEST oOVALTest = new OVALTEST();
                                    try
                                    {
                                        oOVALTest = oval_model.OVALTEST.FirstOrDefault(o => o.comment.ToLower().Contains(sFileName) && o.comment.ToLower().Contains(" is less than " + sFileVersion) && o.deprecated != true && !o.comment.ToLower().Contains("equal") && !o.comment.ToLower().Contains("greater"));
                                        //Check if ssleay32.dll 1.0.1 version is greater than or equal 1.0.1 and less than 1.0.1j on ProgramFilesDir
                                    }
                                    catch (Exception exOVALTest)
                                    {

                                    }
                                    string sOVALTestID = "";
                                    string sOVALTestFilename = "";
                                    if (oOVALTest != null)  //The OVALTest is known
                                    {
                                        Console.WriteLine("DEBUG Write Criterion " + sTestComment);
                                        //bWroteCriterion = true;
                                        sOVALTestID = oOVALTest.OVALTestIDPattern;  //oval:org.cisecurity:tst:1 OR  oval:org.xorcism:tst:1
                                        monStreamWriter.WriteLine(sTab + "\t\t\t<oval-def:criterion comment=\"" + sTestComment + "\" test_ref=\"" + oOVALTest.OVALTestIDPattern + "\" />");

                                        //Is that an xorcism test?
                                        if (oOVALTest.OVALTestIDPattern.Contains("xorcism"))    //Hardcoded
                                        {
                                            //oval:org.xorcism:tst:1
                                            sOVALTestFilename = oOVALTest.OVALTestIDPattern.Replace(":", "_") + ".xml";
                                            //oval_org.xorcism_tst_1.xml
                                            //Where are the file(s)? (we want them in our current (CVE-XXX) directory to build a definition package

                                            //Test if exists in current folder first
                                            FileInfo fileInfo = new FileInfo(sCVE + "\\" + sOVALTestFilename);
                                            if (!fileInfo.Exists)
                                            {
                                                #region findovaltestfilesandcopythemtolocaldirectory
                                                //var allFiles = Directory.GetFiles(sCurrentPath, "*.xml", SearchOption.AllDirectories);
                                                var allFiles = Directory.GetFiles(sCurrentPath, sOVALTestFilename, SearchOption.AllDirectories);
                                                if (allFiles.Count() > 0)
                                                {
                                                    try
                                                    {
                                                        System.IO.File.Copy(allFiles.First(), sCVE + "\\" + sOVALTestFilename, false);  //do not overwrite

                                                        //Now that we have the OVALTestFile in our current CVE directory,
                                                        //Do we have also the OVALObjectFile and OVALStateFile?
                                                        try
                                                        {
                                                            //StreamReader myStreamReaderOVALTestFile = new StreamReader(sOVALTestFilename);
                                                            string sOVALTestFileContent = System.IO.File.ReadAllText(sCVE + "\\" + sOVALTestFilename);   ////Hardcoded

                                                            Regex myRegexObjectRef = new Regex("object_ref=\"(.*?)\"");
                                                            string sOVALObjectIDinOVALTest = myRegexObjectRef.Match(sOVALTestFileContent).ToString();
                                                            if (sOVALObjectIDinOVALTest.Contains("xorcism"))
                                                            {
                                                                //We find and copy in current CVE directory the OVALObjectFile
                                                                string sOVALObjectFileName = sOVALObjectIDinOVALTest.Replace("object_ref=", "").Replace("\"", "");
                                                                //oval:org.xorcism:obj:1
                                                                sOVALObjectFileName = sOVALObjectFileName.Replace(":", "_") + ".xml";
                                                                //sOVALObjectFileName = sOVALObjectFileName.Replace("org_xorcism", "org.xorcism");
                                                                //oval_org.xorcism_obj_1.xml
                                                                fileInfo = new FileInfo(sCVE + "\\" + sOVALObjectFileName);
                                                                if (!fileInfo.Exists)
                                                                {
                                                                    allFiles = Directory.GetFiles(sCurrentPath, sOVALObjectFileName, SearchOption.AllDirectories);
                                                                    if (allFiles.Count() > 0)
                                                                    {
                                                                        try
                                                                        {
                                                                            System.IO.File.Copy(allFiles.First(), sCVE + "\\" + sOVALObjectFileName, false);
                                                                        }
                                                                        catch (Exception exCopyOVALObjectFileToCurrentCVEDirectory)
                                                                        {
                                                                            Console.WriteLine("Exception: exCopyOVALObjectFileToCurrentCVEDirectory " + exCopyOVALObjectFileToCurrentCVEDirectory.Message + " " + exCopyOVALObjectFileToCurrentCVEDirectory.InnerException);
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        Console.WriteLine("ERROR: Could not find the OVALObjectFile " + sOVALObjectFileName);
                                                                    }
                                                                }

                                                            }
                                                            //else  //No need of the OVALObjectFile in current directory

                                                            Regex myRegexStateRef = new Regex("state_ref=\"(.*?)\"");
                                                            string sOVALStateIDinOVALTest = myRegexStateRef.Match(sOVALTestFileContent).ToString();
                                                            if (sOVALStateIDinOVALTest.Contains("xorcism"))
                                                            {
                                                                //We find and copy in current CVE directory the OVALStateFile
                                                                string sOVALStateFileName = sOVALStateIDinOVALTest.Replace("state_ref=", "").Replace("\"", "");
                                                                //oval:org.xorcism:ste:1
                                                                sOVALStateFileName = sOVALStateFileName.Replace(":", "_") + ".xml";
                                                                //sOVALStateFileName = sOVALStateFileName.Replace("org_xorcism", "org.xorcism");
                                                                //oval_org.xorcism_ste_1.xml
                                                                allFiles = Directory.GetFiles(sCurrentPath, sOVALStateFileName, SearchOption.AllDirectories);
                                                                if (allFiles.Count() > 0)
                                                                {
                                                                    try
                                                                    {
                                                                        System.IO.File.Copy(allFiles.First(), sCVE + "\\" + sOVALStateFileName, false);
                                                                    }
                                                                    catch (Exception exCopyOVALStateFileToCurrentCVEDirectory)
                                                                    {
                                                                        Console.WriteLine("Exception: exCopyOVALStateFileToCurrentCVEDirectory " + exCopyOVALStateFileToCurrentCVEDirectory.Message + " " + exCopyOVALStateFileToCurrentCVEDirectory.InnerException);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    Console.WriteLine("ERROR: Could not find the OVALStateFile " + sOVALStateFileName);

                                                                }
                                                            }
                                                            //else  //No need of the OVALStateFile in current directory


                                                            //jjj
                                                            //myStreamReaderOVALTestFile.Close();
                                                        }
                                                        catch (Exception exReadOVALTestFile)
                                                        {
                                                            Console.WriteLine("Exception: exReadOVALTestFile " + exReadOVALTestFile.Message + " " + exReadOVALTestFile.InnerException);
                                                        }

                                                    }
                                                    catch (IOException exCopyOVALTestFileToCurrentCVEDirectorycopyError)
                                                    {
                                                        Console.WriteLine("Exception: exCopyOVALTestFileToCurrentCVEDirectorycopyError " + exCopyOVALTestFileToCurrentCVEDirectorycopyError.Message);
                                                    }
                                                    catch (Exception exCopyOVALTestFileToCurrentCVEDirectory)
                                                    {
                                                        Console.WriteLine("Exception: exCopyOVALTestFileToCurrentCVEDirectory " + exCopyOVALTestFileToCurrentCVEDirectory.Message + " " + exCopyOVALTestFileToCurrentCVEDirectory.InnerException);
                                                    }
                                                }
                                                else
                                                {
                                                    Console.WriteLine("ERROR: Could not find the OVALTestFile " + sOVALTestFilename);
                                                }

                                                #endregion findovaltestfilesandcopythemtolocaldirectory
                                            }
                                        }

                                    }
                                    else
                                    {
                                        //Create a new Test
                                        sOVALTestID = "oval:org.xorcism:tst:" + iTestID;
                                        sOVALTestFilename = "oval_org.xorcism_tst_" + iTestID + ".xml";

                                        #region criterionxorcism
                                        Console.WriteLine("DEBUG Write CriterionXORCISM");
                                        monStreamWriter.WriteLine(sTab + "\t\t\t<oval-def:criterion comment=\"" + sTestComment + "\" test_ref=\"oval:org.xorcism:tst:" + iTestID + "\" />");    //Hardcoded
                                        //TODO: Check for Limited Distribution Release (LDR) file version

                                        //For XORCISM
                                        #region addovaltestxorcism
                                        try
                                        {
                                            oOVALTest = new OVALTEST();
                                            oOVALTest.CreatedDate = DateTimeOffset.Now;
                                            oOVALTest.OVALTestIDPattern = "oval:org.xorcism:tst:" + iTestID;    //Hardcoded
                                            oOVALTest.OVALTestVersion = 0;  //hardcoded
                                            //TODO
                                            //oOVALTest.ExistenceEnumerationID=
                                            //oOVALTest.CheckEnumerationID=
                                            //oOVALTest.OperatorEnumerationID=
                                            oOVALTest.comment = sTestComment;
                                            //oOVALTest.VocabularyID=
                                            //oOVALTest.OVALObjectID=
                                            //oOVALTest.OVALStateID=
                                            //oOVALTest.OVALNamespaceID=
                                            //oOVALTest.OVALTestDataTypeID=
                                            oOVALTest.deprecated = false;
                                            oOVALTest.timestamp = DateTimeOffset.Now;
                                            oval_model.OVALTEST.Add(oOVALTest);
                                            oval_model.Entry(oOVALTest).State = EntityState.Added;
                                            oval_model.SaveChanges();
                                        }
                                        catch (Exception exOVALTESTAdd)
                                        {
                                            Console.WriteLine("Exception: exOVALTESTAdd " + exOVALTESTAdd.Message + " " + exOVALTESTAdd.InnerException);
                                        }
                                        #endregion addovaltestxorcism

                                        //WRITE THE OVAL TEST
                                        //repository\\tests\\windows\\file_test\\0000\\
                                        //string sOVALTestFilename = "oval_org.xorcism_tst_" + iTestID + ".xml";
                                        myStreamWriterOVALTEST = new StreamWriter(sCVE + "\\" + sOVALTestFilename);    //Hardcoded Path
                                        myStreamWriterOVALTEST.WriteLine("<file_test xmlns=\"http://oval.mitre.org/XMLSchema/oval-definitions-5#windows\" check=\"all\" check_existence=\"at_least_one_exists\" comment=\"" + sTestComment + "\" id=\"oval:org.xorcism:tst:" + iTestID + "\" version=\"0\">");

                                        iTestID++;  //Increment ID Test
                                        //Save the new latest XORCISM TestID
                                        StreamWriter myStreamWriter = new StreamWriter("TestID.txt");
                                        myStreamWriter.WriteLine(iTestID);
                                        myStreamWriter.Close();

                                        //Copy the ovaltest file to the OVALRepo     overwrite the destination file if it already exists.
                                        if (!bDEBUGMODE)
                                        {
                                            try
                                            {
                                                System.IO.File.Copy(sCVE + "\\" + sOVALTestFilename, sLocalOVALRepoPath + "tests\\windows\\file_test\\0000\\" + sOVALTestFilename, true);
                                            }
                                            catch (Exception exCopyOVALTestFile)
                                            {
                                                Console.WriteLine("Exception: exCopyOVALTestFile " + exCopyOVALTestFile.Message + " " + exCopyOVALTestFile.InnerException);
                                            }
                                        }

                                        //<object object_ref="oval:org.mitre.oval:obj:26571" />
                                        //For XORCISM
                                        #region ovalobject
                                        int iFileID = fXORCISMAddFILE(sFileName);

                                        Console.WriteLine("DEBUG iFileID=" + iFileID);

                                        //int iOVALObjectFileID = 0;
                                        OVALOBJECTFILE oOVALObjectFile = new OVALOBJECTFILE();
                                        try
                                        {
                                            //if (!bAddFile) 
                                            //Do we know an OVAL Object for the file?
                                            //iOVALObjectFileID = oval_model.OVALOBJECTFILE.FirstOrDefault(o => o.FileID == iFileID).OVALObjectFileID;
                                            oOVALObjectFile = oval_model.OVALOBJECTFILE.FirstOrDefault(o => o.FileID == iFileID);
                                        }
                                        catch (Exception ex)
                                        {

                                        }
                                        if (oOVALObjectFile != null)
                                        {
                                            try
                                            {
                                                myStreamWriterOVALTEST.WriteLine("\t<object object_ref=\"" + oOVALObjectFile.OVALOBJECT.OVALObjectIDPattern + "\" />");
                                            }
                                            catch (Exception exmyStreamWriterOVALTEST1)
                                            {
                                                Console.WriteLine("Exception: exmyStreamWriterOVALTEST1 " + exmyStreamWriterOVALTEST1.Message + " " + exmyStreamWriterOVALTEST1.InnerException);
                                            }
                                        }
                                        else
                                        {
                                            try
                                            {
                                                //Console.WriteLine("ERROR: OVALOBJECTFILE NOT FOUND!");
                                                //Create the new Object
                                                /*
                                                <file_object xmlns="http://oval.mitre.org/XMLSchema/oval-definitions-5#windows" comment="Object holds Clfs.sys file" id="oval:org.cisecurity:obj:470" version="7">
                                                  <path var_check="all" var_ref="oval:org.mitre.oval:var:200" />    //Windows System32 directory
                                                  <filename>Clfs.sys</filename>
                                                </file_object>
                                                */

                                                //TODO
                                                //Add OVALOBJECT in XORCISM

                                                //Add OVALOBJECTFILE in XORCISM

                                                //WRITE THE OVAL OBJECT
                                                //repository\\objects\\windows\\file_object\\0000\\
                                                string sOVALObjectFilename = "oval_org.xorcism_obj_" + iObjectID + ".xml";
                                                StreamWriter myStreamWriterOVALOBJECT = new StreamWriter(@sCVE + "\\" + sOVALObjectFilename);    //Hardcoded
                                                myStreamWriterOVALOBJECT.WriteLine("<file_object xmlns=\"http://oval.mitre.org/XMLSchema/oval-definitions-5#windows\" comment=\"Object holds " + sFileName + " file\" id=\"oval:org.xorcism:obj:" + iObjectID + "\" version=\"0\">");
                                                //myStreamWriterOVALOBJECT.WriteLine("\t"); //TODO!!! Retrieve the path of the file (product) //xaz

                                                //Now we need the Path of the Product. First; look in the OVALVARIABLES

                                                #region productpath
                                                //oval:org.cisecurity:var:49    comment=Microsoft Publisher 2010 directory
                                                //foreach(string sProduct in lProducts)
                                                //{
                                                //WARNING: should we use sProductToAdd? sProductToAddReplaced?
                                                string sProductNameForVariable = "";
                                                if (sMainProductName != "")
                                                {
                                                    sProductNameForVariable = sMainProductName; //microsoft edge
                                                }
                                                else
                                                {
                                                    sProductNameForVariable = fGetPlatformName(sProductName, false).ToLower();
                                                    sProductNameForVariable = sProductNameForVariable.Replace("microsoft ", "");    //Hardcoded
                                                }

                                                Console.WriteLine("DEBUG sProductNameForVariable=" + sProductNameForVariable);  //TODO  windows kernel


                                                List<OVALVARIABLE> OVALVariables = oval_model.OVALVARIABLE.Where(o => o.comment.ToLower().Contains(sProductNameForVariable) && o.deprecated != true).ToList();   //&& .Contains("directory") or "path" or "folder" -version
                                                Dictionary<string, int> dOVALVariablesRelatedToProductPath = new Dictionary<string, int>();
                                                foreach (OVALVARIABLE oOVALVariable in OVALVariables)
                                                {
                                                    int iConfidence = 50;
                                                    string sVariableComment = oOVALVariable.comment.ToLower();
                                                    //HARDCODED
                                                    if (sVariableComment.Contains("directory")) iConfidence += 10;
                                                    if (sVariableComment.Contains("path")) iConfidence += 10;
                                                    if (sVariableComment.Contains("folder")) iConfidence += 10;
                                                    if (sVariableComment.Contains("32bit and 64bit")) iConfidence += 25;    //shared
                                                    if (sVariableComment.Contains("32bit") && sProductName.Contains("x86")) iConfidence += 25;    //shared
                                                    if (sVariableComment.Contains("32 bit") && sProductName.Contains("x86")) iConfidence += 25;    //shared

                                                    if (sVariableComment.Contains("install")) iConfidence += 20;  //install   installroot installation
                                                                                                                  //shared        The shared Office 2007 directory for both 32bit and 64bit
                                                    if (sVariableComment.Contains(sProductNameForVariable.ToLower() + " installation directory")) iConfidence += 50;
                                                    if (sVariableComment.Contains(sProductName.ToLower() + " installation directory")) iConfidence += 100;
                                                    if (sVariableComment.Contains(".dll")) iConfidence -= 10;
                                                    if (sVariableComment.Contains(".exe")) iConfidence -= 10;
                                                    //32bit and 64bit       x86 x64
                                                    //Console.WriteLine("DEBUG oOVALVariable.OVALVariableIDPattern=" + oOVALVariable.OVALVariableIDPattern);
                                                    //Console.WriteLine("DEBUG oOVALVariable.comment=" + oOVALVariable.comment);
                                                    dOVALVariablesRelatedToProductPath.Add(oOVALVariable.OVALVariableIDPattern, iConfidence);
                                                }
                                                if (dOVALVariablesRelatedToProductPath.Count() > 0)
                                                {
                                                    var item = dOVALVariablesRelatedToProductPath.OrderByDescending(x => x.Value).First();  //Order by Confidence
                                                    Console.WriteLine("DEBUG Best Match OVALVariable: " + item.Key);
                                                    //<path var_check="all" var_ref="oval:org.mitre.oval:var:200" />    //Windows System32 directory
                                                    myStreamWriterOVALOBJECT.WriteLine("\t<path var_check=\"all\" var_ref=\"" + item.Key + "\" />");    //at least one
                                                }
                                                else
                                                {
                                                    Console.WriteLine("ERROR: NO OVALVariable (path) Found");
                                                }

                                                if (OVALVariables.Count() == 0)
                                                {
                                                    //If path still not found; look in the OVALOBJECTs, i.e. registry_object oval:org.mitre.oval:obj:23941    comment=Publisher 2010 InstallRoot path
                                                    //WARNING: should we use sProductToAdd?
                                                    //OVALOBJECTFILE.PathValue?
                                                    List<OVALOBJECT> OVALObjects = oval_model.OVALOBJECT.Where(o => o.comment.Contains(sProductNameForVariable) && o.deprecated != true).ToList();   //&& .Contains("directory") or "path" or "folder" -version
                                                                                                                                                                                                     //var OVALObjects = oval_model.OVALOBJECT.Where(o => o.comment.Contains(sProduct)).GroupBy(o => o.OVALObjectIDPattern).ToDictionary(g => g.Key, g => g.Max(o => o.OVALObjectVersion)).ToList();   //&& .Contains("directory") or "path" or "folder" -version
                                                    Dictionary<string, int> dOVALObjectsRelatedToProductPath = new Dictionary<string, int>();
                                                    foreach (OVALOBJECT oOVALObject in OVALObjects)
                                                    {
                                                        int iConfidence = 50;
                                                        string sObjectComment = oOVALObject.comment.ToLower();
                                                        //HARDCODED
                                                        if (sObjectComment.Contains("directory")) iConfidence += 10;
                                                        if (sObjectComment.Contains("path")) iConfidence += 10;
                                                        if (sObjectComment.Contains("folder")) iConfidence += 10;
                                                        if (sObjectComment.Contains("install")) iConfidence += 50;  //install   installroot installation
                                                        if (sObjectComment.Contains(sProductNameForVariable.ToLower() + " installation directory")) iConfidence += 100;
                                                        if (sObjectComment.Contains(sProductName.ToLower() + " installation directory")) iConfidence += 200;
                                                        if (sObjectComment.Contains(".dll")) iConfidence -= 10;
                                                        if (sObjectComment.Contains(".exe")) iConfidence -= 10;
                                                        //32bit and 64bit       x86 x64
                                                        //Console.WriteLine("DEBUG oOVALObject.OVALObjectIDPattern=" + oOVALObject.OVALObjectIDPattern);
                                                        //Console.WriteLine("DEBUG oOVALObject.comment=" + oOVALObject.comment);
                                                        dOVALObjectsRelatedToProductPath.Add(oOVALObject.OVALObjectIDPattern, iConfidence);
                                                    }
                                                    if (dOVALObjectsRelatedToProductPath.Count() > 0)
                                                    {
                                                        var item = dOVALObjectsRelatedToProductPath.OrderByDescending(y => y.Value).First();
                                                        Console.WriteLine("DEBUG Best Match OVALObject: " + item.Key);

                                                        //Now what is the Path of this object? (needed?)
                                                        //TODO

                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("ERROR: NO OVALObject Found");
                                                    }

                                                }

                                                //}
                                                #endregion productpath


                                                myStreamWriterOVALOBJECT.WriteLine("\t<filename>" + sFileName + "</filename>");
                                                myStreamWriterOVALOBJECT.WriteLine("</file_object>");
                                                myStreamWriterOVALOBJECT.Close();

                                                iObjectID++;    //Increment ID Object
                                                                //Save the new latest XORCISM ObjectID
                                                myStreamWriter = new StreamWriter("ObjectID.txt");
                                                myStreamWriter.WriteLine(iObjectID);
                                                myStreamWriter.Close();

                                                //Copy the ovalobject file to the OVALRepo     overwrite the destination file if it already exists.
                                                if (!bDEBUGMODE)
                                                {
                                                    try
                                                    {
                                                        System.IO.File.Copy(sCVE + "\\" + sOVALObjectFilename, sLocalOVALRepoPath + "objects\\windows\\file_object\\0000\\" + sOVALObjectFilename, true);
                                                    }
                                                    catch (Exception exCopyOVALObjectFile)
                                                    {
                                                        Console.WriteLine("Exception: exCopyOVALObjectFile " + exCopyOVALObjectFile.Message + " " + exCopyOVALObjectFile.InnerException);
                                                    }
                                                }

                                                myStreamWriterOVALTEST.WriteLine("\t<object object_ref=\"oval:org.xorcism:obj:" + iObjectID + "\" />");
                                            }
                                            catch (Exception exmyStreamWriterOVALOBJECT)
                                            {
                                                Console.WriteLine("Exception: exmyStreamWriterOVALOBJECT " + exmyStreamWriterOVALOBJECT.Message + " " + exmyStreamWriterOVALOBJECT.InnerException);
                                            }
                                        }

                                        #endregion ovalobject

                                        //<state state_ref="oval:org.cisecurity:ste:1851" />
                                        #region ovalstate
                                        if (!dFileVersionOVALState.ContainsKey(sFileVersion))   //TODO Review!
                                        {
                                            #region filestate
                                            try
                                            {
                                                //Search if we already have a OVALSTATE file_state for this version
                                                //NOTE: it needs to be for the same object!? //Ref. https://github.com/CISecurity/OVALRepo/pull/516
                                                //For that: check in OVALTEST.OVALStateID and/or OVALSTATEFOROVALTEST (comment)
                                                #region lastminuteadd
                                                string sOVALStateIDPattern = "";
                                                bool bGoodOVALStateFound = false;
                                                try
                                                {
                                                    IEnumerable<OVALSTATE> myOVALSTATES = oval_model.OVALSTATE.Where(o => o.comment.Contains(" less than " + sFileVersion) && o.deprecated != true);    //hardcoded
                                                    foreach (XOVALModel.OVALSTATE myOVALStateDetailSearch in myOVALSTATES)
                                                    {
                                                        try
                                                        {
                                                            IEnumerable<OVALTEST> myOVALTESTs = oval_model.OVALTEST.Where(o => o.OVALStateID == myOVALStateDetailSearch.OVALStateID && o.deprecated != true);
                                                            foreach (XOVALModel.OVALTEST myOVALTestSearch in myOVALTESTs)
                                                            {
                                                                if (myOVALTestSearch.comment.ToLower().Contains(sFileName))
                                                                {
                                                                    bGoodOVALStateFound = true;
                                                                    sOVALStateIDPattern = myOVALStateDetailSearch.OVALStateIDPattern;
                                                                    break;  //We just need 1 confirmation
                                                                }
                                                            }

                                                            if (bGoodOVALStateFound)
                                                            {
                                                                break;
                                                            }
                                                            else
                                                            {
                                                                IEnumerable<OVALSTATEFOROVALTEST> myOVALSTATEsForTESTs = oval_model.OVALSTATEFOROVALTEST.Where(o => o.OVALStateID == myOVALStateDetailSearch.OVALStateID);
                                                                foreach (XOVALModel.OVALSTATEFOROVALTEST myOVALStateForTestSearch in myOVALSTATEsForTESTs)
                                                                {
                                                                    if (myOVALStateForTestSearch.OVALTEST.comment.ToLower().Contains(sFileName))
                                                                    {
                                                                        bGoodOVALStateFound = true;
                                                                        sOVALStateIDPattern = myOVALStateDetailSearch.OVALStateIDPattern;
                                                                        break;  //We just need 1 confirmation
                                                                    }
                                                                }
                                                            }

                                                            if (bGoodOVALStateFound)
                                                            {
                                                                break;  //Good to go
                                                            }
                                                        }
                                                        catch (Exception ex)
                                                        {

                                                        }
                                                    }
                                                }
                                                catch (Exception exRetrieveGoodOVALState)
                                                {

                                                }
                                                #endregion lastminuteadd

                                                //comment=State matches version less than 14.0.7177.5000
                                                //version 	datatype=version | operation=less than | value=14.0.7177.5000
                                                /*
                                                OVALSTATE oOVALState = oval_model.OVALSTATE.FirstOrDefault(o => o.comment.Contains(" less than " + sFileVersion));
                                                if (oOVALState != null)
                                                {
                                                    Console.WriteLine("DEBUG OVALSTATE Found: " + oOVALState.OVALStateIDPattern);
                                            
                                                    myStreamWriterOVALTEST.WriteLine("\t<state state_ref=\"" + oOVALState.OVALStateIDPattern + "\" />");
                                                    dFileVersionOVALState.Add(sFileVersion, oOVALState.OVALStateIDPattern);
                                                }
                                                */
                                                if (bGoodOVALStateFound)
                                                {
                                                    Console.WriteLine("DEBUG OVALSTATE Found: " + sOVALStateIDPattern);

                                                    myStreamWriterOVALTEST.WriteLine("\t<state state_ref=\"" + sOVALStateIDPattern + "\" />");
                                                    //dFileVersionOVALState.Add(sFileVersion, sOVALStateIDPattern);   //TODO Review!
                                                }
                                                else
                                                {
                                                    Console.WriteLine("NOTE: No Good (for same object) OVALSTATE Found");
                                                    //Create one
                                                    //TODO
                                                    /*
                                                        <file_state xmlns="http://oval.mitre.org/XMLSchema/oval-definitions-5#windows" comment="State matches version less than 6.0.6002.19717" id="oval:com.dtcc:ste:466" version="0">
                                                            <version datatype="version" operation="less than">6.0.6002.19717</version>
                                                        </file_state>
                                                    */

                                                    //For XORCISM
                                                    //Add OVALSTATE in XORCISM
                                                    #region addovalstatexorcism
                                                    try
                                                    {
                                                        OVALSTATE oOVALState = new OVALSTATE();
                                                        oOVALState.CreatedDate = DateTimeOffset.Now;
                                                        oOVALState.OVALStateIDPattern = "oval:org.xorcism:ste:" + iStateID; //hardcoded
                                                        oOVALState.OVALStateVersion = 0;    //hardcoded
                                                        //TODO
                                                        //oOVALState.OVALStateTypeID=
                                                        //oOVALState.OperatorEnumerationID=
                                                        oOVALState.comment = "State matches version less than " + sFileVersion; //hardcoded
                                                        oOVALState.deprecated = false;
                                                        //oOVALState.OVALNamespaceID=
                                                        ////oOVALState.OVALStateFieldID=
                                                        //oOVALState.VocabularyID=
                                                        oOVALState.timestamp = DateTimeOffset.Now;
                                                        oval_model.OVALSTATE.Add(oOVALState);
                                                        oval_model.Entry(oOVALState).State = EntityState.Added;
                                                        oval_model.SaveChanges();
                                                    }
                                                    catch (Exception exOVALSTATEAdd)
                                                    {
                                                        Console.WriteLine("Exception: exOVALSTATEAdd " + exOVALSTATEAdd.Message + " " + exOVALSTATEAdd.InnerException);
                                                    }
                                                    #endregion addovalstatexorcism

                                                    //dFileVersionOVALState.Add(sFileVersion, "oval:org.xorcism:ste:" + iStateID);    //TODO Review!

                                                    //WRITE THE OVAL STATE
                                                    //X:\\OVALRepo\\OVALRepo\\repository\\states\\windows\\file_state\\0000\\
                                                    string sOVALStateFilename = "oval_org.xorcism_ste_" + iStateID + ".xml";
                                                    StreamWriter myStreamWriterOVALSTATE = new StreamWriter(sCVE + "\\" + sOVALStateFilename);    //Hardcoded
                                                    myStreamWriterOVALSTATE.WriteLine("<file_state xmlns=\"http://oval.mitre.org/XMLSchema/oval-definitions-5#windows\" comment=\"State matches version less than " + sFileVersion + "\" id=\"oval:org.xorcism:ste:" + iStateID + "\" version=\"0\">");
                                                    myStreamWriterOVALSTATE.WriteLine("\t<version datatype=\"version\" operation=\"less than\">" + sFileVersion + "</version>");
                                                    myStreamWriterOVALSTATE.WriteLine("</file_state>");
                                                    myStreamWriterOVALSTATE.Close();
                                                    //Write the state in the test
                                                    myStreamWriterOVALTEST.WriteLine("\t<state state_ref=\"oval:org.xorcism:ste:" + iStateID + "\" />");

                                                    iStateID++; //Increment ID State
                                                    //Save the new latest XORCISM StateID
                                                    myStreamWriter = new StreamWriter("StateID.txt");
                                                    myStreamWriter.WriteLine(iStateID);
                                                    myStreamWriter.Close();

                                                    //Copy the ovalstate file to the OVALRepo     overwrite the destination file if it already exists.
                                                    if (!bDEBUGMODE)
                                                    {
                                                        try
                                                        {
                                                            System.IO.File.Copy(sCVE + "\\" + sOVALStateFilename, sLocalOVALRepoPath + "states\\windows\\file_state\\0000\\" + sOVALStateFilename, true);
                                                        }
                                                        catch (Exception exCopyOVALStateFile)
                                                        {
                                                            Console.WriteLine("Exception: exCopyOVALStateFile " + exCopyOVALStateFile.Message + " " + exCopyOVALStateFile.InnerException);
                                                        }
                                                    }
                                                }
                                            }
                                            catch (Exception exFileState)
                                            {
                                                Console.WriteLine("Exception: exFileState " + exFileState.Message + " " + exFileState.InnerException);
                                            }
                                            #endregion filestate
                                        }
                                        else
                                        {
                                            myStreamWriterOVALTEST.WriteLine("\t<state state_ref=\"" + dFileVersionOVALState[sFileVersion] + "\" />");
                                        }
                                        #endregion ovalstate

                                        myStreamWriterOVALTEST.WriteLine("</file_test>");
                                        myStreamWriterOVALTEST.Close();
                                        #endregion criterionxorcism
                                    }
                                    #endregion criterion
                                    bWroteCriterion = true;

                                    if (bWroteCriteria4 && iCounterExtendDefinition2Wrote == 1)//iCounterCorrectProducts == 1)// && bWroteCriterion
                                    {
                                        sTab = sSaveTabCriteria4;
                                        Console.WriteLine("DEBUG WriteEnd Criteria4 sNextFile!=sCurrentFile sTab.Length=" + sTab.Length);
                                        if (bDEBUGMODEINXML) monStreamWriter.WriteLine("DEBUG WriteEnd Criteria4 sNextFile!=sCurrentFile sTab.Length=" + sTab.Length);
                                        if (bDEBUGMODEINXML) sDebug = "4";
                                        monStreamWriter.WriteLine(sTab + "\t\t</oval-def:criteria" + sDebug + ">");   //Criteria4

                                    }
                                    iCounterExtendDefinition2Wrote = 0;
                                    iCounterCorrectProducts = 0;

                                    //if (iCounterCorrectProducts <= 1)
                                    if (!bWroteCriteria3)
                                    {
                                        Console.WriteLine("DEBUG NOT Writing EndCriteria3");
                                        if (bDEBUGMODEINXML) monStreamWriter.WriteLine("DEBUG NOT Writing EndCriteria3");
                                    }
                                    else
                                    {
                                        sTab = sSaveTabCriteria3;
                                        Console.WriteLine("DEBUG WriteEnd Criteria3");
                                        if (bDEBUGMODEINXML) monStreamWriter.WriteLine("DEBUG WriteEnd Criteria3");
                                        if (bDEBUGMODEINXML) sDebug = "3";
                                        monStreamWriter.WriteLine(sTab + "\t\t</oval-def:criteria" + sDebug + ">");
                                    }
                                    bWroteCriteria3 = false;
                                    bWroteCriteria4 = false;

                                    /*
                                    if (bWroteCriteria2)
                                    {
                                        sTab = sSaveTabCriteria2;
                                        Console.WriteLine("DEBUG WriteEnd Criteria2");
                                        if (bDEBUGMODEINXML) monStreamWriter.WriteLine("DEBUG WriteEnd Criteria2");
                                        monStreamWriter.WriteLine(sTab + "\t</oval-def:criteria>");
                                        bWroteCriteria2 = false;
                                    }
                                    else
                                    {
                                        Console.WriteLine("DEBUG NOT Writing Criteria2");
                                    }
                                    */
                                }
                            }
                            #endregion ovalinventorydefinitionnotnull

                            if (bBypassExtendDef)
                            {
                                bBypassBlock = true;
                            }
                            else
                            {
                                bBypassBlock = false;
                            }

                        }

                        //if (bWroteCriteria2 && (lMainProductsNames.Count<2) || iCounterMainProductsProcessed == lMainProductsNames.Count)
                        if (bWroteCriteria2 && !(bWroteCriteria2Global && iCounterMainProductsProcessed == 1))// && iCounterMainProductsProcessed == lMainProductsNames.Count)
                        {
                            Console.WriteLine("DEBUG WriteEnd Criteria2");
                            if (bDEBUGMODEINXML) monStreamWriter.WriteLine("DEBUG WriteEnd Criteria2");
                            if (bDEBUGMODEINXML) sDebug = "20";
                            monStreamWriter.WriteLine(sTab + "\t</oval-def:criteria" + sDebug + ">");
                            //bWroteCriteria2Global = false;
                            bWroteCriteria2 = false;

                        }
                        else
                        {
                            Console.WriteLine("DEBUG NOT Writing Criteria2");
                        }

                        if (sMainProductNameToTitle != "1337" && sCurrentMainProductNameToTile != "joker") //(lMainProductsNames.Count > 1)
                        {
                            Console.WriteLine("DEBUG WriteEnd Criteria1 sMainProductNameToTitle!=1337");//lMainProductsNames.Count>1 (=NOT 1337)");
                            if (bDEBUGMODEINXML) monStreamWriter.WriteLine("DEBUG WriteEnd Criteria1 sMainProductNameToTitle!=1337");
                            if (bDEBUGMODEINXML) sDebug = "1";
                            monStreamWriter.WriteLine(sSaveTabCriteria1 + "</oval-def:criteria" + sDebug + ">");  //Note: always 2tabs?
                            //sTab = "\t";

                            if (iCounterCorrectProducts == 1)
                            {
                                //Console.WriteLine("DEBUG NOT Writing EndCriteria1 iCounterCorrectProducts=1");
                                //if (bDEBUGMODEINXML) monStreamWriter.WriteLine("DEBUG NOT Writing EndCriteria1 iCounterCorrectProducts=1");

                                Console.WriteLine("DEBUG REINIT sTAB to one iCounterCorrectProducts=1");
                                if (bDEBUGMODEINXML) monStreamWriter.WriteLine("DEBUG REINIT sTAB to one iCounterCorrectProducts=1");
                                sTab = "\t";
                            }
                        }
                        else
                        {
                            //1337  joker
                            Console.WriteLine("DEBUG NOT Writing EndCriteria1");
                            if (bDEBUGMODEINXML) monStreamWriter.WriteLine("DEBUG NOT Writing EndCriteria1");
                        }
                        #endregion whiterabbit
                    }

                    //FINAL
                    if (bWroteCriteria2Global)// && iCounterMainProductsProcessed == lMainProductsNames.Count)
                    {
                        Console.WriteLine("DEBUG WriteEnd Final Criteria2");
                        if (bDEBUGMODEINXML) monStreamWriter.WriteLine("DEBUG WriteEnd Final Criteria2");
                        if (bDEBUGMODEINXML) sDebug = "22";
                        monStreamWriter.WriteLine(sSaveTabCriteria2Global + "\t</oval-def:criteria" + sDebug + ">");
                        //bWroteCriteria2Global = false;
                        ////bWroteCriteria2 = false;

                    }
                    else
                    {
                        Console.WriteLine("DEBUG NOT Writing Final Criteria2");
                    }

                    //TODO  bWroteCriteria1Global

                    //if (sMainProductNameToTitle != "")// && sMainProductNameToTitle != "1337")  //TODO: && we found the OVAL Inventory Definition for it
                    if (lMainProductsNames.Count <= 1)
                    {
                        Console.WriteLine("DEBUG NOT Writing EndCriteria0");
                        if (bDEBUGMODEINXML) monStreamWriter.WriteLine("DEBUG NOT Writing EndCriteria0");
                    }
                    else
                    {
                        //lMainProductsNames.Count > 1
                        if (iCounterMainProductsProcessed == lMainProductsNames.Count)    //Last MainProduct
                        {
                            Console.WriteLine("DEBUG WriteEnd Criteria0");
                            if (bDEBUGMODEINXML) monStreamWriter.WriteLine("DEBUG WriteEnd Criteria0");
                            if (bDEBUGMODEINXML) sDebug = "0";
                            monStreamWriter.WriteLine("\t</oval-def:criteria" + sDebug + ">");    //Note: always 1 tab
                        }
                    }

                    //Console.WriteLine("DEBUG WriteEnd CriteriaFinal");
                    //monStreamWriter.WriteLine("\t</oval-def:criteria>");
                    monStreamWriter.WriteLine("</oval-def:definition>");
                    monStreamWriter.Close();

                    //Copy the OVAL Vulnerability Definition file to the OVALRepo     overwrite the destination file if it already exists.
                    if (!bDEBUGMODE)
                    {
                        try
                        {
                            System.IO.File.Copy(sCVE + "\\" + sOVALDefinitionFilename, sLocalOVALRepoPath + "definitions\\vulnerability\\" + sOVALDefinitionFilename, true);
                        }
                        catch (Exception exCopyOVALDefinitionFile)
                        {
                            Console.WriteLine("Exception: exCopyOVALDefinitionFile " + exCopyOVALDefinitionFile.Message + " " + exCopyOVALDefinitionFile.InnerException);
                        }
                    }
                }
                catch (Exception exStreamWriterOVALXML)
                {
                    Console.WriteLine("Exception: exStreamWriterOVALXML " + exStreamWriterOVALXML.Message + " " + exStreamWriterOVALXML.InnerException);
                }
                #endregion writeoval
            }

            Console.WriteLine("DEBUG END " + DateTimeOffset.Now);

            /*
            try
            {
                Console.WriteLine("DEBUG Validating the generated OVAL Vulnerability Definition");
                // Use ProcessStartInfo class   //TODO use fStartProcess()
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.CreateNoWindow = false;
                startInfo.UseShellExecute = true;
                startInfo.FileName = "python.exe";
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.Arguments = "X:\\OVALRepo\\OVALRepo\\scripts\\validate_oval_definitions_files.py "+sCVE+"\\"+sMyOVALDefinitionID + ".xml";    //HARDCODED Path

                try
                {
                    // Start the process with the info we specified.
                    // Call WaitForExit and then the using statement will close.
                    using (Process exeProcess = Process.Start(startInfo))
                    {
                        exeProcess.WaitForExit();
                    }
                }
                catch
                {
                    // Log error.
                }
            }
            catch (Exception exValidateOVAL)
            {
                Console.WriteLine("Exception: exValidateOVAL " + exValidateOVAL.Message + " " + exValidateOVAL.InnerException);
            }
            */
        }





        public static void fAnalyzeKBURL(string sKBURL, string sFileNameToSearch = "", string sVULDescription = "", string sMainProductName = "", int iVulnerabilityID=0, string sMSTitle="")
        {
            string ResponseText = string.Empty;
            string sKBNumber = sKBURL.Replace("https://support.microsoft.com/en-us/kb/", "");

            string sBIGKBFilePath = sCurrentPath + @"\MS\KB" + sKBNumber + ".txt";    //HARDCODED Local Path to save the KB page
            Console.WriteLine("DEBUG sBIGKBFilePath=" + sBIGKBFilePath);

            

            #region AddPatchToXORCISM
            #region patch
            int iPatchID = 0;
            try
            {
                try
                {
                    //iPatchID = model.PATCH.Where(o => o.PatchVocabularyID == "KB" + sKBNumber).FirstOrDefault().PatchID;
                    iPatchID = model.PATCH.Where(o => o.PatchVocabularyID == sKBNumber).FirstOrDefault().PatchID;
                }
                catch (Exception ex)
                {

                }
                if (iPatchID <= 0)
                {
                    Console.WriteLine("DEBUG Adding PATCH");
                    PATCH oPatch = new PATCH();
                    oPatch.CreatedDate = DateTimeOffset.Now;
                    //oPatch.PatchVocabularyID = "KB" + sKBNumber;
                    oPatch.PatchVocabularyID = sKBNumber;
                    oPatch.PatchTitle = sMSTitle.Replace("</h2>", "").Replace("</h2", "");  //Hardcoded
                                                                        //oPatch.PatchDescription=  //TODO
                                                                        //oPatch.VocabularyID=
                    model.PATCH.Add(oPatch);
                    model.SaveChanges();
                    iPatchID = oPatch.PatchID;
                    bPatchJustAdded = true;
                }
                else
                {
                    //Update PATCH
                }
            }
            catch (Exception exAddPatch)
            {
                Console.WriteLine("Exception: exAddPatch " + exAddPatch.Message + " " + exAddPatch.InnerException+ "\nsMSTitle="+ sMSTitle);
            }
            #endregion patch

            #region PATCHREFERENCES
            int iReferenceID = 0;
            try
            {
                try
                {
                    iReferenceID = model.REFERENCE.Where(o => o.ReferenceURL == "https://catalog.update.microsoft.com/v7/site/Search.aspx?q=KB" + sKBNumber).FirstOrDefault().ReferenceID;    //HARDCODEDMS
                }
                catch (Exception ex)
                {

                }
                if (iReferenceID <= 0)
                {
                    REFERENCE oReferenceKB = new REFERENCE();
                    oReferenceKB.CreatedDate = DateTimeOffset.Now;
                    oReferenceKB.ReferenceURL = "https://catalog.update.microsoft.com/v7/site/Search.aspx?q=KB" + sKBNumber;    //HARDCODEDMS
                    //oReferenceKB.ReferenceSourceID = "KB" + sKBNumber;
                    oReferenceKB.ReferenceSourceID = sKBNumber;
                    oReferenceKB.Source = "Microsoft";
                    //oReferenceKB.SourceTrustLevelID=    //High
                    //oReferenceKB.SourceTrustReasonID=   //Trusted Vendor
                    //oReferenceKB.VocabularyID=
                    //... todo
                    model.REFERENCE.Add(oReferenceKB);
                    model.SaveChanges();
                    iReferenceID = oReferenceKB.ReferenceID;
                }
            }
            catch (Exception exReference)
            {
                Console.WriteLine("Exception: exReference " + exReference.Message + " " + exReference.InnerException);
            }

            if (iPatchID > 0 && iReferenceID > 0)
            {
                try
                {
                    int iPatchReferenceID = 0;
                    try
                    {
                        iPatchReferenceID = model.PATCHREFERENCE.Where(o => o.PatchID == iPatchID && o.ReferenceID == iReferenceID).FirstOrDefault().PatchReferenceID;
                    }
                    catch (Exception ex)
                    {

                    }
                    if (iPatchReferenceID <= 0)
                    {
                        PATCHREFERENCE oPatchReference = new PATCHREFERENCE();
                        oPatchReference.CreatedDate = DateTimeOffset.Now;
                        oPatchReference.PatchID = iPatchID;
                        oPatchReference.ReferenceID = iReferenceID;
                        //oPatchReference.VocabularyID=
                        model.PATCHREFERENCE.Add(oPatchReference);
                        model.SaveChanges();
                    }
                }
                catch (Exception exPatchReference)
                {
                    Console.WriteLine("Exception: exPatchReference " + exPatchReference.Message + " " + exPatchReference.InnerException);
                }
            }
            #endregion PATCHREFERENCES

            #region VulnerabilityPatch
            int iVulnerabilityPatchID = 0;
            try
            {
                if (iVulnerabilityID > 0 && iPatchID > 0)
                {
                    try
                    {
                        //iVulnerabilityPatchID = vuln_model.VULNERABILITYPATCH.Where(o => o.VulnerabilityID == oVulnerability.VulnerabilityID && o.PatchID == iPatchID).FirstOrDefault().VulnerabilityPatchID;
                        iVulnerabilityPatchID = vuln_model.VULNERABILITYPATCH.Where(o => o.VulnerabilityID == iVulnerabilityID && o.PatchID == iPatchID).FirstOrDefault().VulnerabilityPatchID;
                    }
                    catch (Exception ex)
                    {

                    }
                    if (iVulnerabilityPatchID <= 0)
                    {
                        VULNERABILITYPATCH oVulnPatch = new VULNERABILITYPATCH();
                        oVulnPatch.CreatedDate = DateTimeOffset.Now;
                        oVulnPatch.VulnerabilityID = iVulnerabilityID;
                        oVulnPatch.PatchID = iPatchID;
                        //oVulnPatch.VocabularyID=
                        vuln_model.VULNERABILITYPATCH.Add(oVulnPatch);
                        vuln_model.SaveChanges();
                    }
                }
            }
            catch (Exception exVulnerabilityPatch)
            {
                Console.WriteLine("Exception: exVulnerabilityPatch " + exVulnerabilityPatch.Message + " " + exVulnerabilityPatch.InnerException);
            }
            #endregion VulnerabilityPatch
            #endregion AddPatchToXORCISM

            #region getMSKBPageLocalCopy
            //try
            //{
            FileInfo fileInfo = new FileInfo(sBIGKBFilePath);
            //}
            //catch (Exception exFileInfoBIGKB)
            //{
            //    Console.WriteLine("Exception: exFileInfoBIGKB " + exFileInfoBIGKB.Message + " " + exFileInfoBIGKB.InnerException);
            //}

            //TODO?
            //if (!fileInfo.Exists)   //We never visited/saved locally the KB page
            //{
                Console.WriteLine("DEBUG Request to " + sKBURL);
                //JavaScript used here, so need a 'browser'
                try
                {
                    using (var driver = new ChromeDriver())
                    {
                        //driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(60);
                        driver.Navigate().GoToUrl(sKBURL);
                        Thread.Sleep(10000 + iSleepMore); //Hardcoded   //TODO!

                        //TODO: Review internationalization (test French)
                        //é \u00E9      éditions
                        System.IO.File.WriteAllText(sBIGKBFilePath, HttpUtility.HtmlDecode(driver.PageSource).Replace("\u00A0", " ").Replace("\u00E9", "e").Replace("\u00C0", ""));   ////Hardcoded remove &nbsp; (HttpUtility.HtmlDecode("htmlcode") ?)   à
                    }
                    lVisitedURLs.Add(sKBURL);
                }
                catch (Exception exRequestKBURL)
                {
                    Console.WriteLine("Exception: exRequestKBURL " + exRequestKBURL.Message + " " + exRequestKBURL.InnerException);
                    //Try Again
                    try
                    {
                        using (var driver = new ChromeDriver())
                        {
                            //driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(120);
                            driver.Navigate().GoToUrl(sKBURL);
                            Thread.Sleep(10000 + iSleepMore); //Hardcoded   //TODO!

                            //TODO: Review internationalization (test French)
                            //é \u00E9      éditions
                            System.IO.File.WriteAllText(sBIGKBFilePath, HttpUtility.HtmlDecode(driver.PageSource).Replace("\u00A0", " ").Replace("\u00E9", "e").Replace("\u00C0", ""));   ////Hardcoded remove &nbsp; (HttpUtility.HtmlDecode("htmlcode") ?)   à
                        }
                        lVisitedURLs.Add(sKBURL);
                    }
                    catch (Exception exRequestKBURL2)
                    {
                        Console.WriteLine("Exception: exRequestKBURL2 " + exRequestKBURL2.Message + " " + exRequestKBURL2.InnerException);
                    }
                }
            //}
            #endregion getMSKBPageLocalCopy


            //Parse the big KB file (containing links to other individual KBs with files information for each Product)
            try
            {
                ResponseText = System.IO.File.ReadAllText(sBIGKBFilePath);   ////Hardcoded Save the KB page locally
            }
            catch (Exception exReadBIGKB)
            {
                Console.WriteLine("Exception: exReadBIGKB " + exReadBIGKB.Message + " " + exReadBIGKB.InnerException);
            }

            //Dictionary<string, string> dKBURLFile = new Dictionary<string, string>();       //To avoid parsing the same KBfile multiple times
            dKBURLFile = new Dictionary<string, string>();       //To avoid parsing the same KBfile multiple times
                                                                 ////Dictionary<string, string> dFileVersionOVALState = new Dictionary<string, string>();   //To avoid sql requests for the same file

            //***********************************************************************************************************************************

            //Parse the big KB file (with links to other individual KBs with files information)
            //    <title ng-bind="title" class="ng-binding">Description of the security update for Skype for Business 2016: May 9, 2017</title>
            Regex myRegexTitle = new Regex("<title(.*?)</title>", RegexOptions.Singleline);
            string sMSPageTitle = myRegexTitle.Match(ResponseText).ToString();
            Regex myRegexTitle2 = new Regex(">(.*?)</title>", RegexOptions.Singleline);
            sMSPageTitle = myRegexTitle2.Match(sMSPageTitle).ToString();
            if(sMSPageTitle!="")
            {
                Console.WriteLine("DEBUG sMSPageTitle=" + sMSPageTitle.Replace("</title>", ""));
                sMSPageTitle = sMSPageTitle.Replace("2007 Microsoft Office Suite", "Microsoft Office 2007");    //HARDCODEDMS
                if (sMSPageTitle.ToLower().Contains("office online server") && !lFileNamesToSearch.Contains("wacserver.dll")) lFileNamesToSearch.Add("wacserver.dll");


                //Search products names in the page's title
                string sMicrosoftProductFromTitle = string.Empty;
                foreach (string sMicrosoftProduct in lProductsMicrosoft)
                {
                    
                    if (sMSPageTitle.ToLower().Contains(sMicrosoftProduct.ToLower().Replace("microsoft ", "")))
                    {
                        //Console.WriteLine("DEBUG MicrosoftProduct retrieved from title: " + sMicrosoftProduct);
                        if (sMicrosoftProduct.Length > sMicrosoftProductFromTitle.Length)// || sMicrosoftProductFromTitle != sMicrosoftProduct)     //TODO REVIEW
                        {
                            sMicrosoftProductFromTitle = sMicrosoftProduct;
                        }
                    }
                }

                if (sMicrosoftProductFromTitle != "" && !lMainProductsNames.Contains(sMicrosoftProductFromTitle))
                {
                    //Any SP specified?
                    //<strong>Note</strong> To apply this security update, you must have the release version of <a href="http://support.microsoft.com/kb/2880551">Service Pack 1 for Microsoft SharePoint Foundation 2013</a> installed on the computer.</p>
                    //To apply this cumulative update, you must have Microsoft SharePoint Server 2013 Service Pack 1 installed.
                    //Applies to
                    //TODO
                    Regex myRegexTitleSP = new Regex("Service Pack(.*?)installed", RegexOptions.Singleline);
                    string sMSPageTitleSP = myRegexTitleSP.Match(ResponseText).ToString();
                    if(sMSPageTitleSP!="")
                    {
                        if (sMSPageTitleSP.Contains("Service Pack 1")) sMicrosoftProductFromTitle = sMicrosoftProductFromTitle + " Service Pack 1";
                        if (sMSPageTitleSP.Contains("Service Pack 2")) sMicrosoftProductFromTitle = sMicrosoftProductFromTitle + " Service Pack 2";
                        if (sMSPageTitleSP.Contains("Service Pack 3")) sMicrosoftProductFromTitle = sMicrosoftProductFromTitle + " Service Pack 3";
                        if (sMSPageTitleSP.Contains("Service Pack 4")) sMicrosoftProductFromTitle = sMicrosoftProductFromTitle + " Service Pack 4";
                        if (sMSPageTitleSP.Contains("Service Pack 5")) sMicrosoftProductFromTitle = sMicrosoftProductFromTitle + " Service Pack 5";
                        if (sMSPageTitleSP.Contains("Service Pack 6")) sMicrosoftProductFromTitle = sMicrosoftProductFromTitle + " Service Pack 6";

                    }

                    Console.WriteLine("DEBUG sMicrosoftProductFromTitleFinal: " + sMicrosoftProductFromTitle);
                    sProductFoundDEADBEEFGlobal = sMicrosoftProductFromTitle + "DEADBEEF";  //Hardcoded
                    //Console.WriteLine("DEBUG sProductFoundDEADBEEFGlobalFromTitle: " + sProductFoundDEADBEEFGlobal);
                    lMainProductsNames.Add(sMicrosoftProductFromTitle);

                    //TODO: FavoriteFiles
                    if (sMicrosoftProductFromTitle.ToLower().Contains("web apps") && !lFileNamesToSearch.Contains("msoserver.dll")) lFileNamesToSearch.Add("msoserver.dll");
                    if (sMicrosoftProductFromTitle.ToLower().Contains("word") && !lFileNamesToSearch.Contains("winword.exe")) lFileNamesToSearch.Add("winword.exe");
                    if (sMicrosoftProductFromTitle.ToLower().Contains("word viewer") && !lFileNamesToSearch.Contains("winword.exe")) lFileNamesToSearch.Add("wordview.exe");
                    if (sMicrosoftProductFromTitle.ToLower().Contains("sharepoint") && !lFileNamesToSearch.Contains("sword.dll")) lFileNamesToSearch.Add("sword.dll");
                    if (sMicrosoftProductFromTitle.ToLower().Contains("project") && !lFileNamesToSearch.Contains("microsoft.office.project.server.pwa.applicationpages.dll")) lFileNamesToSearch.Add("microsoft.office.project.server.pwa.applicationpages.dll");   //Review
                    //...

                }

            }

            ////if (ResponseText.Contains("<h3 class=\"section-title ng-binding\">File Information</h3>"))   //HARDCODEDMS
            //if (ResponseText.Contains(">File Information</h3>") || ResponseText.Contains(">File information</h3>"))   //HARDCODEDMS     (<strong>)
            if (ResponseText.Contains(">File Information</") || ResponseText.Contains(">File information</"))   //HARDCODEDMS     (<strong>)
            {
                //<h3 class="ng-scope">File information</h3>
                Console.WriteLine("DEBUG FileInformationH3");
                #region fileinformationh3
                //e.g.  Adobe Flash Player
                //Regex myRegexBlockH3FileInformation = new Regex("<h3 class=\"section-title ng-binding\">File Information</h3>(.*?)</section>", RegexOptions.Singleline); //HARDCODEDMSHTML
                Regex myRegexBlockH3FileInformation = new Regex(">File Information</(.*?)</section>", RegexOptions.IgnoreCase | RegexOptions.Singleline); //HARDCODEDMSHTML

                string sBlockH3FileInformation = myRegexBlockH3FileInformation.Match(ResponseText).ToString();
                //Console.WriteLine("DEBUG sBlockH3FileInformation=" + sBlockH3FileInformation);

                string sBlockH3FileInformationKBURL = "";
                if (sBlockH3FileInformation == "")
                {
                    //<p>See <a href="https://support.microsoft.com/kb/4014329"><u>Microsoft Knowledge Base article 4014329</u></a></p>
                    Regex myRegexBlockH3FileInformationKBURL = new Regex(">File Information</(.*?)</tr>", RegexOptions.IgnoreCase | RegexOptions.Singleline); //HARDCODEDMSHTML
                    sBlockH3FileInformationKBURL = myRegexBlockH3FileInformationKBURL.Match(ResponseText).ToString();
                    //Console.WriteLine("DEBUG sBlockH3FileInformationKBURL=" + sBlockH3FileInformationKBURL);
                }
                if (sBlockH3FileInformationKBURL != "" && sBlockH3FileInformationKBURL.Contains("support.microsoft.com/kb/"))
                {
                    Regex myRegexKBURLMS = new Regex("support.microsoft.com/kb/\\d+", RegexOptions.Singleline);   //HardcodedMS //TODO Review
                    string sFileInformationKBURL = myRegexKBURLMS.Match(sBlockH3FileInformationKBURL).ToString();
                    Console.WriteLine("DEBUG sFileInformationKBURL=" + sFileInformationKBURL);
                    sKBNumber = sFileInformationKBURL.Replace("https://", "").Replace("support.microsoft.com/kb/", "");
                    Console.WriteLine("DEBUG Calling BluePill sFileNameToSearch=" + sFileNameToSearch);
                    //TODO007
                    string sFileDateVersion = string.Empty;
                    try
                    {
                        sFileDateVersion=fBluePill(sKBNumber, sFileNameToSearch, "");  // sProductFound);
                    }
                    catch(Exception exCallBluePill)
                    {
                        Console.WriteLine("Exception: exCallBluePill " + exCallBluePill.Message + " " + exCallBluePill.InnerException);
                    }
                    if (sFileDateVersion != string.Empty)
                    {
                        Console.WriteLine("DEBUG sFileDateVersionCatalog=" + sFileDateVersion);
                        //TODO Review
                        if (!dKBURLFile.ContainsKey(sFileInformationKBURL)) dKBURLFile.Add(sFileInformationKBURL, sFileDateVersion.ToLower());
                        if (!lFilesToUse.Contains(sFileNameToSearch.ToLower())) lFilesToUse.Add(sFileNameToSearch.ToLower());   //We keep track of the Files that we use so we can harmonize them in the final XML. e.g.: 20161005 10.0.14393.321 win32kbase.sys and 20161005 10.0.14393.321 win32kfull.sys (same date/version) if we already used win32kfull.sys we would give it preference
                                                                                                                                /*
                                                                                                                                if (sProductFoundDEADBEEF == "") sProductFoundDEADBEEF = sProductFoundDEADBEEFGlobal;

                                                                                                                                if (!dProductFile.ContainsKey(sProductFoundDEADBEEF))
                                                                                                                                {
                                                                                                                                    Console.WriteLine("DEBUG dProductFile.Add " + sProductFoundDEADBEEF);
                                                                                                                                    dProductFile.Add(sProductFoundDEADBEEF, sFileDateVersion.ToLower());
                                                                                                                                }
                                                                                                                                */
                    }
                    //}
                    //else//Apparently we already visited the KB page, downloaded and extracted the KBfile
                    //{
                    //jack
                    //}

                    if (sFileDateVersion == string.Empty)
                    {
                        Console.WriteLine("WARNING: No file found!!!");
                        if (!dKBURLFile.ContainsKey(sFileInformationKBURL))
                        {
                            dKBURLFile.Add(sFileInformationKBURL, "WARNING: No file found!");  //Hardcoded //So we don't parse it again
                        }
                    }
                }

                lFilenames = new List<string>();
                lFileversions = new List<string>();
                lFiledates = new List<string>();
                lFileplatforms = new List<string>();
                string sFileNameToSearchReplaced = sFileNameToSearch;
                string sKBURLFinal2 = sKBURL;


                //if(sBlockH3FileInformationKBURL.Contains("download.microsoft.com/download/"))   //.csv
                if (ResponseText.Contains("download.microsoft.com/download/"))   //.csv
                {
                    Regex myRegexDownloadCSVlink = new Regex("download.microsoft.com/(.*?).csv", RegexOptions.Singleline);    //HARDCODEDMS
                    string sRegexDownloadCSVlink = myRegexDownloadCSVlink.Match(ResponseText).ToString(); ////.ToLower()
                    if (sRegexDownloadCSVlink.Trim() != "")
                    {
                        Console.WriteLine("DEBUG CallingfParseCSV sFileNameToSearch=" + sFileNameToSearch);
                        fParseCSV(sRegexDownloadCSVlink, sFileNameToSearch);    //sFileNameToSearchReplaced


                        Dictionary<string, Dictionary<string, string>> dFileInfos = fSortFileInfos(sFileNameToSearch); //new Dictionary<string, Dictionary<string, string>>();

                        //fFileInfosRetrievedByParsing(dFileInfos, sFileNameToSearch, sProductFoundDEADBEEF, sKBURLFinal2);
                        if (sProductFoundDEADBEEFGlobal == "")
                        {
                            sProductFoundDEADBEEFGlobal = "DEADBEEF";
                        }
                        fFileInfosRetrievedByParsing(dFileInfos, sFileNameToSearch, sFileNameToSearchReplaced, sProductFoundDEADBEEFGlobal, sKBURLFinal2);
                    }
                    
                }

                
                //<span class="link-expand-text ng-binding" id="">  Windows Server 2012 file information  </span>
                Regex myRegexBlockFileInfo = new Regex("<span class=\"link-expand-text ng-binding\" id=\"\">(.*?)</div></div>", RegexOptions.Singleline); //HARDCODEDMSHTML
                                                                                                                                                          //<a title=" Windows Server 2012 file information " class="link-expand bold" role="button" href="" ng-click="toggle()" ng-attr-title="{{panel.title}}">
                Regex myRegexBlockFileInfo2 = new Regex("<a title=\"(.*?)file information", RegexOptions.Singleline); //HARDCODEDMSHTML

                MatchCollection myKBBlock = myRegexBlockFileInfo.Matches(sBlockH3FileInformation);
                if (myKBBlock.Count <= 0)
                {
                    //Try 2
                    myKBBlock = myRegexBlockFileInfo2.Matches(sBlockH3FileInformation);

                }
                if (myKBBlock.Count > 0)
                {
                    foreach (Match myKBProductBlock in myKBBlock) //for each product block
                    {
                        Regex myRegexBlockFileInfoProduct = new Regex("<span class=\"link-expand-text ng-binding\" id=\"\">(.*?)</span>", RegexOptions.Singleline); //HARDCODEDMSHTML
                                                                                                                                                                    //<span class="link-expand-text ng-binding" id="">  Windows 8.1 and Windows Server 2012 R2 file information  </span>
                        string sHyperlinkText = myRegexBlockFileInfoProduct.Match(myKBProductBlock.ToString()).ToString();
                        if (sHyperlinkText.Trim() == "") sHyperlinkText = myKBProductBlock.ToString().Replace("<a title=\"", "").Trim();    //Hardcoded 
                        sHyperlinkText = StripTagsRegexCompiled(sHyperlinkText).Replace("file information", "").Trim();

                        List<string> lProductsFoundInH3Title = new List<string>();
                        string sProductsInHyperlink = sHyperlinkText;
                        //Retrieve the Products Name(s) from the BlockH3
                        //lProductsFoundInH3Title = fGetExactProductName(sHyperlinkText, sProductsInHyperlink, sProductsInHyperlink, oVulnerability.VULDescription, lProductsMicrosoft, sMainProductName);
                        lProductsFoundInH3Title = fGetExactProductName(sHyperlinkText, sProductsInHyperlink, sProductsInHyperlink, sVULDescription, lProductsMicrosoft, sMainProductName);

                        foreach (string sProductInBlock in lProductsFoundInH3Title)
                        {
                            Console.WriteLine("DEBUG sProductInBlockH3=" + sProductInBlock);

                            
                            Regex myRegexBlockH3Table = new Regex("</h4><table class=\"table ng-scope\">(.*?)</table>", RegexOptions.Singleline); //HARDCODEDMSHTML
                            MatchCollection myKBTable = myRegexBlockH3Table.Matches(sBlockH3FileInformation);
                            foreach (Match myKBProductBlockTable in myKBTable) //for each product block
                            {
                                Console.WriteLine("DEBUG myKBProductBlockH3=" + myKBProductBlockTable);
                                fParseKBForFiles(myKBProductBlockTable.ToString(), lFileNamesToSearch, 2);

                                //string sProductFoundDEADBEEF = sCurrentMainProductNameExtendedModified + "DEADBEEF" + sProductFound;   //HARDCODEDJA Separator Product|Platform
                                string sProductFoundDEADBEEF = sMainProductName + "DEADBEEF" + sProductInBlock;   //HARDCODEDJA Separator Product|Platform

                                Console.WriteLine("DEBUG sProductFoundDEADBEEFH3=" + sProductFoundDEADBEEF);


                                //TODO: Duplicate code
                                ////Sort the arrays to get the file with most recent date
                                #region sortfileinfos
                                ////Array.Sort(sFiledates, sFilenames);
                                Dictionary<string, Dictionary<string, string>> dFileInfos = new Dictionary<string, Dictionary<string, string>>();
                                try
                                {
                                    Console.WriteLine("DEBUG lFilenames.CountA=" + lFilenames.Count);
                                    
                                    bool bJustOneFilename = true;   //Used to replace the sFileNameToSearch with the unique filename found
                                    for (int i = 0; i < lFilenames.Count; i++)
                                    {
                                        try
                                        {
                                            if(i>0)
                                            {
                                                if (lFilenames[i - 1] != lFilenames[i]) bJustOneFilename = false;
                                            }
                                            //TODO  lFileplatforms
                                            Dictionary<string, string> dTemp = new Dictionary<string, string>();
                                            dTemp.Add(lFilenames[i].ToLower(), lFileversions[i]);
                                            //IMPORTANT: if 2+ InterestingFiles with the same Dates:
                                            //1)    => take the sFileNameToSearch   (i.e. chakra.dll vs edgehtml.dll)
                                            if (dFileInfos.ContainsKey(lFiledates[i]))  //Same FileDate
                                            {
                                                Dictionary<string, string> dTemp2 = new Dictionary<string, string>();
                                                dTemp2 = dFileInfos[lFiledates[i]];
                                                if (dTemp2.First().Key == sFileNameToSearch)    //TODO: FavoriteFile
                                                {
                                                    //That's our favorite option (the one FileName specified in arguments)
                                                    //We keep it
                                                }
                                                else
                                                {
                                                    if (lFilesToUse.Contains(dTemp2.First().Key) || lFileNamesToSearch.Contains(dTemp2.First().Key))
                                                    {
                                                        //2nd best option
                                                        //we keep it
                                                        
                                                    }
                                                    else
                                                    {
                                                        //We replace it
                                                        dFileInfos[lFiledates[i]] = dTemp;
                                                    }
                                                }
                                            }
                                            //2)    => take the file with the highest version (for this file)
                                            //i.e.:
                                            //Win32k.sys
                                            //Win32kbase.sys
                                            //Win32kfull.sys
                                            //TODO? Use XORCISM FILEVERSION
                                            dFileInfos.Add(lFiledates[i], dTemp);
                                        }
                                        catch (Exception exAdddFileInfos)
                                        {
                                            //Duplicate keys (same FileDate)

                                        }
                                    }
                                    if (lFilenames.Count > 0 && bJustOneFilename)
                                    {
                                        //Replace sFileNameToSearch
                                        sFileNameToSearch = lFilenames[0];
                                        Console.WriteLine("DEBUG ReplacingFileNameToSearch by: " + sFileNameToSearch);
                                    }
                                }
                                catch (Exception exLoopFilenames)
                                {
                                    Console.WriteLine("Exception: exLoopFilenames " + exLoopFilenames.Message + " " + exLoopFilenames.InnerException);
                                }
                                // Use OrderBy method on the Date   //TODO Review
                                /*
                                foreach (var item in dFileInfos.OrderByDescending(i => i.Key))
                                {
                                    //Console.WriteLine("DEBUG " + item);   //[17-Nov-2016, System.Collections.Generic.Dictionary`2[System.String,System.String]]
                                    Dictionary<string, string> dTemp = new Dictionary<string, string>();
                                    dTemp = item.Value;
                                    Console.WriteLine("DEBUG " + item.Key + " " + dTemp.First().Key+ " " + dTemp.First().Value);
                                    //17-Nov-2016 ptxt9.dll 14.0.7177.5000

                                    //FILE
                                    //FILEPRODUCT
                                    //OVALOBJECTFILE
                                    //...
                                }
                                */

                                #endregion sortfileinfos


                                if (dFileInfos.Count() > 0)
                                {
                                    #region fileinfosretrievedbyparsing
                                    //TODO REVIEW NOT ALWAYS THE FIRST ONE IS THE GOOD ONE (take the one specified, or the highest version for a file)
                                    var itemFileInfo = dFileInfos.OrderByDescending(i => i.Key).First();    //By Default (latest date)
                                    string sFileInfoLatestDate = itemFileInfo.Key;
                                    if(bDebugFileSelection) Console.WriteLine("DEBUG sFileInfoLatestDate=" + sFileInfoLatestDate);  //Could this be wrong if 1st one selected from KB page?

                                    Dictionary<string, string> dTempFilenameFileversion3 = new Dictionary<string, string>();
                                    dTempFilenameFileversion3 = itemFileInfo.Value;    //filename version



                                    if (dTempFilenameFileversion3.First().Key == sFileNameToSearch || lFileNamesToSearch.Contains(dTempFilenameFileversion3.First().Key) || lFilesToUse.Contains(dTempFilenameFileversion3.First().Key))
                                    {
                                        //That's our favorite option (the one FileName specified in arguments OR one from our list of files)
                                        if(bDebugFileSelection) Console.WriteLine("JA1");
                                        //TODO: BUT we could be looking for multiple files    e.g. CVE-2016-0002  vbscript.dll + jscript.dll
                                        //TODO: FavoriteFile
                                    }
                                    else
                                    {
                                        //Console.WriteLine("JA9");
                                        var itemsFileInfo = dFileInfos.OrderByDescending(i => i.Key);   //By Date Desc
                                        int iCptItems = 0;
                                        foreach (var itemFileInforeview in itemsFileInfo)
                                        {
                                            iCptItems++;
                                            if (bDebugFileSelection) Console.WriteLine("JA8");
                                            if (iCptItems != 1) //Skip the first one equal to itemFileInfo (Default)
                                            {
                                                if (bDebugFileSelection) Console.WriteLine("JA7");
                                                if (itemFileInforeview.Key == itemFileInfo.Key)  //Same Dates
                                                {
                                                    if (bDebugFileSelection) Console.WriteLine("JA2");
                                                    //Check the FileName
                                                    Dictionary<string, string> dTempFilenameFileversion2 = new Dictionary<string, string>();
                                                    dTempFilenameFileversion2 = itemFileInforeview.Value;
                                                    if (dTempFilenameFileversion2.First().Key == sFileNameToSearch || lFileNamesToSearch.Contains(dTempFilenameFileversion2.First().Key) || lFilesToUse.Contains(dTempFilenameFileversion2.First().Key))
                                                    {
                                                        //We keep our favorite option (the one FileName specified in arguments or in our list)
                                                        if (sFileNameToSearch != string.Empty && sFileNameToSearch != "" && dTempFilenameFileversion2.First().Key == sFileNameToSearch)
                                                        {
                                                            //BEST Option
                                                            itemFileInfo = itemFileInforeview;
                                                            //TODO: if we are looking for multiple files
                                                            //TODO: FavoriteFile
                                                            if (bDebugFileSelection) Console.WriteLine("JA3");
                                                            //break;
                                                        }
                                                        else
                                                        {
                                                            dTempFilenameFileversion3 = new Dictionary<string, string>();
                                                            dTempFilenameFileversion3 = itemFileInfo.Value;    //filename version
                                                            if (dTempFilenameFileversion3.First().Key == sFileNameToSearch)
                                                            {
                                                                //Previous is better
                                                                if (bDebugFileSelection) Console.WriteLine("JA0");
                                                            }
                                                            else
                                                            {
                                                                itemFileInfo = itemFileInforeview;
                                                                //TODO: if we are looking for multiple files
                                                                //lFilesToUse
                                                                if (bDebugFileSelection) Console.WriteLine("JA33");
                                                                //break;
                                                            }


                                                        }
                                                    }
                                                    else//Not FileNameToSearch nor FileToUse
                                                    {
                                                        dTempFilenameFileversion3 = new Dictionary<string, string>();
                                                        dTempFilenameFileversion3 = itemFileInfo.Value;    //filename version
                                                                                                           //Take the highest version number
                                                        string v1 = dTempFilenameFileversion3.First().Value;
                                                        string v2 = dTempFilenameFileversion2.First().Value;

                                                        var version1 = new Version(v1);
                                                        var version2 = new Version(v2);

                                                        var result = version1.CompareTo(version2);
                                                        if (result > 0)
                                                        {
                                                            //Console.WriteLine("version1 is greater");
                                                            //Console.WriteLine("DEBUG Previous version found " + v1 + " > new version found " + v2 + " so we keep the old one");
                                                        }
                                                        else
                                                        {
                                                            if (bDebugFileSelection) Console.WriteLine("JA13");
                                                            itemFileInfo = itemFileInforeview;
                                                        }

                                                    }

                                                }
                                                else//Different Dates
                                                {
                                                    try
                                                    {
                                                        //Check the FileName
                                                        Dictionary<string, string> dTempFilenameFileversion2 = new Dictionary<string, string>();
                                                        dTempFilenameFileversion2 = itemFileInforeview.Value;
                                                        if (dTempFilenameFileversion2.First().Key == sFileNameToSearch || lFileNamesToSearch.Contains(dTempFilenameFileversion2.First().Key) || lFilesToUse.Contains(dTempFilenameFileversion2.First().Key))
                                                        {
                                                            //the one FileName specified in arguments OR one from our list of files
                                                            //TODO: FavoriteFile
                                                            if (bDebugFileSelection) Console.WriteLine("JA24");

                                                            //Compare the dates difference
                                                            //itemFileInforeview.Key == itemFileInfo.Key
                                                            int iLatestDate = Int32.Parse(sFileInfoLatestDate);
                                                            int iCurrentFileDate = Int32.Parse(itemFileInforeview.Key);
                                                            if (iLatestDate - iCurrentFileDate <= 40) //HARDCODED 40 days
                                                            {
                                                                //We keep our favorite option (the one FileName specified in arguments or in our list)
                                                                itemFileInfo = itemFileInforeview;
                                                                if (bDebugFileSelection) Console.WriteLine("JA25");
                                                                if (dTempFilenameFileversion2.First().Key == sFileNameToSearch || dTempFilenameFileversion2.First().Key == sFileNameToSearchReplaced) break;  //else we continue to search for sFileNameToSearch
                                                            }
                                                            else
                                                            {
                                                                if (bDebugFileSelection) Console.WriteLine("JA26");
                                                                break;  //The file is 'too old'
                                                            }

                                                        }
                                                        else
                                                        {
                                                            //We keep the latest one
                                                            if (bDebugFileSelection) Console.WriteLine("JA4");
                                                            //break;
                                                        }
                                                    }
                                                    catch (Exception exFileInfoDifferentDates)
                                                    {
                                                        Console.WriteLine("Exception: exFileInfoDifferentDates " + exFileInfoDifferentDates.Message + " " + exFileInfoDifferentDates.InnerException);
                                                    }
                                                }
                                            }

                                        }
                                        //To be sure
                                        dTempFilenameFileversion3 = new Dictionary<string, string>();
                                        dTempFilenameFileversion3 = itemFileInfo.Value;
                                    }

                                    //Dictionary<string, string> dTemp3 = new Dictionary<string, string>();
                                    //dTemp3 = new Dictionary<string, string>();
                                    //dTemp3 = item.Value;

                                    string sFileVersion = dTempFilenameFileversion3.First().Value; //We could be wrong

                                    string sFileInfoNeeded = itemFileInfo.Key + " " + dTempFilenameFileversion3.First().Key + " " + sFileVersion;  //date filename version


                                    //Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                    Console.WriteLine("DEBUG FileToUse1A: " + sFileInfoNeeded);
                                    //17-Nov-2016 ptxt9.dll 14.0.7177.5000
                                    if (dProductFile.ContainsKey(sProductFoundDEADBEEF))
                                    {
                                        #region keeplowestversion2
                                        //TODO Review for LDR/GDR
                                        //Console.WriteLine("ERROR02: TODO dProductFile already contains " + sProduct);
                                        //Analyze the situation: at the end, we will keep the lowest version number
                                        if (dProductFile[sProductFoundDEADBEEF] == sFileInfoNeeded)
                                        {
                                            //No issue there*
                                        }
                                        else
                                        {
                                            //Get back the file's information previously collected
                                            string[] PreviousFileInfoNeededSplit = dProductFile[sProductFoundDEADBEEF].Split(' ');
                                            //Is it the same file name?
                                            if (PreviousFileInfoNeededSplit[1] == dTempFilenameFileversion3.First().Key)
                                            {
                                                #region comparefilesversions
                                                //Compare the versions  using Version class //https://stackoverflow.com/questions/7568147/compare-version-numbers-without-using-split-function
                                                string v1 = PreviousFileInfoNeededSplit[2]; //"1.23.56.1487";
                                                string v2 = sFileVersion;   //"1.24.55.487";

                                                var version1 = new Version(v1);
                                                var version2 = new Version(v2);

                                                var result = version1.CompareTo(version2);
                                                if (result > 0)
                                                {
                                                    //Console.WriteLine("version1 is greater");
                                                    //We replace the value in the dictionary
                                                    Console.WriteLine("DEBUG Previous version found " + v1 + " > new version found " + v2 + " so we keep the new one");
                                                    dProductFile[sProductFoundDEADBEEF] = sFileInfoNeeded;
                                                }
                                                else if (result < 0)
                                                {
                                                    //Console.WriteLine("version2 is greater");
                                                    //We keep version1
                                                }
                                                else
                                                {
                                                    //same as before*
                                                    //Console.WriteLine("versions are equal");
                                                    //We should be ok (we don't compare the dates)
                                                }
                                                #endregion comparefilesversions
                                            }
                                            else//different files
                                            {
                                                //TODO!!! Review this (we could use the Dates, or Hardcoding...)
                                                if (sFileNameToSearchReplaced != "")
                                                {
                                                    //We keep the new one
                                                    //i.e.: usp10.dll   =>  mso.dll
                                                    dProductFile[sProductFoundDEADBEEF] = sFileInfoNeeded;
                                                }
                                                else
                                                {
                                                    //HARDCODED
                                                    if (sFileInfoNeeded.Contains("mso.dll")) dProductFile[sProductFoundDEADBEEF] = sFileInfoNeeded;

                                                }
                                            }
                                        }
                                        #endregion keeplowestversion2
                                    }
                                    else
                                    {
                                        dProductFile.Add(sProductFoundDEADBEEF, sFileInfoNeeded.ToLower());
                                    }
                                    try
                                    {
                                        dKBURLFile.Add(sKBURLFinal2, sFileInfoNeeded.ToLower());
                                    }
                                    catch (Exception ex)
                                    {
                                        //Duplicates
                                    }
                                    if (!lFilesToUse.Contains(dTempFilenameFileversion3.First().Key.ToLower())) lFilesToUse.Add(dTempFilenameFileversion3.First().Key.ToLower());   //We keep track of the Files that we use so we can harmonize them in the final XML. e.g.: 20161005 10.0.14393.321 win32kbase.sys and 20161005 10.0.14393.321 win32kfull.sys (same date/version) if we already used win32kfull.sys we would give it preference

                                    /*
                                    if (!dFileVersionOVALState.ContainsKey(sFileVersion))
                                    {
                                        #region filestate
                                        //Search if we already have a OVALSTATE file_state for this version
                                        //comment=State matches version less than 14.0.7177.5000
                                        //version 	datatype=version | operation=less than | value=14.0.7177.5000
                                        OVALSTATE oOVALState = oval_model.OVALSTATE.FirstOrDefault(o => o.comment.Contains(" less than " + sFileVersion));
                                        if (oOVALState != null)
                                        {
                                            Console.WriteLine("DEBUG OVALSTATE Found: " + oOVALState.OVALStateIDPattern);
                                            dFileVersionOVALState.Add(sFileVersion, oOVALState.OVALStateIDPattern);
                                        }
                                        else
                                        {
                                            Console.WriteLine("ERROR: No OVALSTATE Found");
                                            //Create one
                                        }

                                        #endregion filestate
                                    }
                                    */
                                    #endregion fileinfosretrievedbyparsing
                                }
                                else
                                {
                                    Console.WriteLine("WARNING: No file found!");
                                }



                                //date  file    version
                                //string sFileInfoNeeded = 

                                ////dProductFile.Add(sProductFoundDEADBEEF, dKBURLFile[sKBURLFinal2]);
                                //dProductFile.Add(sProductFoundDEADBEEF, sFileInfoNeeded.ToLower());


                            }

                        }

                    }
                }
                else
                {
                    if (sBlockH3FileInformation.Contains("File identifier"))//HARDCODEDMS
                    {
                        //File informations are in this page
                        lFilenames = new List<string>();
                        lFileversions = new List<string>();
                        lFiledates = new List<string>();
                        lFileplatforms = new List<string>();

                        Regex myRegexTR = new Regex("<tr(.*?)</tr>", RegexOptions.Singleline);
                        MatchCollection myKBTR = myRegexTR.Matches(sBlockH3FileInformation);
                        int iColumnIndexFileIdentifier = 0;
                        int iColumnIndexFileName = 0;
                        int iColumnIndexFileVersion = 0;
                        int iColumnIndexFileDate = 0;
                        string sFileIdentifier = string.Empty;
                        foreach (Match myTR in myKBTR)
                        {
                            int iColumnIndex = 0;

                            if (myTR.ToString().Contains("</th>"))
                            {
                                Regex myRegexTH = new Regex("<th(.*?)</th>", RegexOptions.Singleline);
                                MatchCollection myKBTH = myRegexTH.Matches(myTR.ToString());
                                foreach (Match myTH in myKBTH)
                                {
                                    iColumnIndex++;
                                    if (myTH.ToString().Contains("File identifier"))
                                    {
                                        iColumnIndexFileIdentifier = iColumnIndex;    //1
                                    }
                                    if (myTH.ToString().Contains("File name"))
                                    {
                                        iColumnIndexFileName = iColumnIndex;    //2
                                    }
                                    if (myTH.ToString().Contains("File version"))
                                    {
                                        iColumnIndexFileVersion = iColumnIndex;    //3
                                    }
                                    //File size
                                    if (myTH.ToString().Contains("Date"))
                                    {
                                        iColumnIndexFileDate = iColumnIndex;    //5
                                    }
                                    //Time

                                }
                            }
                            else
                            {
                                //</td>
                                Regex myRegexTD = new Regex("<td(.*?)</td>", RegexOptions.Singleline);
                                MatchCollection myKBTD = myRegexTD.Matches(myTR.ToString());
                                foreach (Match myTD in myKBTD)
                                {
                                    iColumnIndex++;
                                    string sColumnValue = myTD.ToString().Replace("<td>", "").Replace("</td>", "");
                                    if (sColumnValue.Contains("File identifier"))
                                    {
                                        iColumnIndexFileIdentifier = iColumnIndex;    //1
                                    }
                                    if (sColumnValue.Contains("File name"))
                                    {
                                        iColumnIndexFileName = iColumnIndex;    //2
                                    }
                                    //...
                                    if (iColumnIndex == iColumnIndexFileIdentifier)
                                    {
                                        //Save it in case the File name is empty
                                        sFileIdentifier = sColumnValue;
                                    }
                                    if (iColumnIndex == iColumnIndexFileName)
                                    {
                                        if (sColumnValue.Trim() != "")
                                        {
                                            lFilenames.Add(sColumnValue);
                                        }
                                        else
                                        {
                                            if (sFileIdentifier.Trim() != "")
                                            {
                                                lFilenames.Add(sFileIdentifier);
                                            }
                                            else
                                            {
                                                Console.WriteLine("ERROR: EmptyFileName while parsing the page");
                                            }
                                        }
                                        //Console.WriteLine("DEBUG TableFileName=" + sColumnValue);
                                    }
                                    if (iColumnIndex == iColumnIndexFileVersion)
                                    {
                                        lFileversions.Add(sColumnValue);
                                        //Console.WriteLine("DEBUG TableFileVersion=" + sColumnValue);
                                    }
                                    if (iColumnIndex == iColumnIndexFileDate)
                                    {
                                        string sDateForSorting = fGetDateForSorting(sColumnValue);
                                        lFiledates.Add(sDateForSorting);
                                        //Console.WriteLine("DEBUG TableFileDate=" + sDateForSorting);
                                    }
                                }
                            }
                        }


                        Dictionary<string, Dictionary<string, string>> dFileInfos = fSortFileInfos(sFileNameToSearch); //new Dictionary<string, Dictionary<string, string>>();

                        //fFileInfosRetrievedByParsing(dFileInfos, sFileNameToSearch, sProductFoundDEADBEEF, sKBURLFinal2);
                        if(sProductFoundDEADBEEFGlobal == "")
                        {
                            sProductFoundDEADBEEFGlobal = "DEADBEEF";
                        }
                        fFileInfosRetrievedByParsing(dFileInfos, sFileNameToSearch, sFileNameToSearchReplaced, sProductFoundDEADBEEFGlobal, sKBURLFinal2);

                    }
                }
                #endregion fileinformationh3
            }
            else
            {

                //Note: ResponseText = System.IO.File.ReadAllText(sBIGKBFilePath);   ////Hardcoded
                Regex myRegexKBProductH4 = new Regex("<h4 class=\"sbody-h4\">(.*?)</h4>", RegexOptions.Singleline); //HARDCODEDMSHTML
                MatchCollection myKBProductH4s = myRegexKBProductH4.Matches(ResponseText);
                foreach (Match myKBProductH4 in myKBProductH4s) //for each product block
                {
                    #region blockproducth4

                    #region blockfoundforoneproduct
                    Console.WriteLine("DEBUG -------------------------------------------------------------");

                    //Console.WriteLine("DEBUG Block: " + myKBProductH4.ToString());  //Note: internationalization (i.e.: éditions)
                    string sHyperlinkText = myKBProductH4.ToString().ToLower();
                    Console.WriteLine("DEBUG sHyperlinkTextRAW=" + sHyperlinkText);

                    #region ignoreandcleanH4
                    //HARDCODEDMS
                    if (sHyperlinkText.Contains(" for mac")) continue;   //|| sHyperlinkText.Contains(" pour mac")) continue;    //French
                    if (sHyperlinkText.Contains("security central")) continue;
                    if (sHyperlinkText.Contains("detection and deployment guidance")) continue;
                    if (sHyperlinkText.Contains("windows server update services")) continue;
                    if (sHyperlinkText.Contains("microsoft baseline security analyzer")) continue;
                    if (sHyperlinkText.Contains("systems management server")) continue;
                    if (sHyperlinkText.Contains("deployment information")) continue;
                    if (sHyperlinkText.Contains("file hash information")) continue;
                    //if (sHyperlinkText.Contains("additional information about this security update")) continue;
                    //additional information about service branch
                    if (sHyperlinkText.Contains("additional information about")) continue;
                    if (sHyperlinkText.Contains("how to determine")) continue;

                    if (sHyperlinkText.Contains("known issues")) continue;
                    //method 1: install the recommended update
                    //method 2: only install the updates that you want
                    if (sHyperlinkText.Contains("method ")) continue;
                    if (sHyperlinkText.Contains("restart requirement")) continue;
                    //verification of the update installation
                    if (sHyperlinkText.Contains("verification ")) continue;


                    //Cleaning
                    sHyperlinkText = sHyperlinkText.Replace("security update for ", ""); //Hardcoded
                    sHyperlinkText = sHyperlinkText.Replace("delta update for ", ""); //Hardcoded

                    //file information
                    sHyperlinkText = sHyperlinkText.Replace("(all editions)", ""); //Hardcoded
                                                                                   //////sHyperlinkText = sHyperlinkText.Replace("(toutes les éditions)", "").ToLower().Trim(); //Hardcoded French
                                                                                   ////sHyperlinkText = sHyperlinkText.Replace("(toutes les editions)", ""); //Hardcoded French
                                                                                   ////sHyperlinkText = sHyperlinkText.Replace("(toutes les versions)", ""); //Hardcoded French
                                                                                   ////sHyperlinkText = sHyperlinkText.Replace(" et autres logiciels", ""); //Hardcoded French
                    sHyperlinkText = sHyperlinkText.Replace(" and other software", ""); //Hardcoded
                    sHyperlinkText = sHyperlinkText.Replace(" (excluding .net)", "");
                    sHyperlinkText = sHyperlinkText.Replace(" (.net framework installations)", "");
                    //(all supported releases)

                    sHyperlinkText = sHyperlinkText.Replace("<br />", "");
                    sHyperlinkText = sHyperlinkText.Replace("<h4 class=\"sbody-h4\">", ""); //Hardcoded
                    sHyperlinkText = sHyperlinkText.Replace("</h4>", "").Trim();

                    //HARDCODEDMS
                    if (sHyperlinkText == "file information") continue;
                    if (sHyperlinkText == "file name") continue;
                    if (sHyperlinkText == "scenario 1") continue;
                    if (sHyperlinkText == "scenario 2") continue;
                    if (sHyperlinkText == "scenario 3") continue;
                    if (sHyperlinkText.Contains("mitigation for known issues")) continue;
                    if (sHyperlinkText.Contains("configuring group policy")) continue;
                    if (sHyperlinkText.Contains("frequently asked questions")) continue;
                    if (sHyperlinkText == "installation switches") continue;
                    if (sHyperlinkText == "restart requirements") continue;
                    if (sHyperlinkText == "removal information") continue;
                    if (sHyperlinkText == "registry key verification") continue;
                    if (sHyperlinkText == "additional information about this update") continue;
                    if (sHyperlinkText == "additional information about this security update") continue;
                    if (sHyperlinkText == "microsoft download center") continue;
                    if (sHyperlinkText == "microsoft update") continue;
                    if (sHyperlinkText == "reference table") continue;
                    #endregion ignoreandcleanH4

                    bool bFileInformationBlock = false;
                    //if (sHyperlinkText.Contains("file information")) continue;
                    if (sHyperlinkText.EndsWith(" file information"))
                    {
                        sHyperlinkText = sHyperlinkText.Replace(" file information", ""); //Hardcoded
                        bFileInformationBlock = true;
                        Console.WriteLine("DEBUG bFileInformationBlock=true");
                    }

                    if (sHyperlinkText == "") continue;

                    //Microsoft Office 2007
                    //the 2007 microsoft office suite
                    if (sHyperlinkText == "the 2007 microsoft office suite") sHyperlinkText = "microsoft office 2007";  //HardcodedMS
                    Console.WriteLine("DEBUG sHyperlinkTextCleaned1=" + sHyperlinkText);
                    //TODO  all affected .net framework versions

                    string sProductsInHyperlink = sHyperlinkText;

                    List<string> lProductsFoundInH4Title = new List<string>();
                    //Retrieve the Products Name(s) from the BlockH4
                    //lProductsFoundInH4Title = fGetExactProductName(sHyperlinkText, sProductsInHyperlink, sProductsInHyperlink, oVulnerability.VULDescription, lProductsMicrosoft, sMainProductName);
                    lProductsFoundInH4Title = fGetExactProductName(sHyperlinkText, sProductsInHyperlink, sProductsInHyperlink, sVULDescription, lProductsMicrosoft, sMainProductName);


                    foreach (string sProductInBlock in lProductsFoundInH4Title)
                    {
                        Console.WriteLine("DEBUG sProductInBlockH4=" + sProductInBlock);

                        sKBNumber = "";

                        Regex myRegexKBProductBlock = new Regex(Regex.Escape(myKBProductH4.ToString()) + "(.*?)</td></tr></tbody>", RegexOptions.Singleline);   //HARDCODEDMSHTML
                        string myKBProductBlock = myRegexKBProductBlock.Match(ResponseText).ToString();
                        //Console.WriteLine("DEBUG myKBProductBlock=" + myKBProductBlock);
                        if (myKBProductBlock.Contains("File information</span></td><td class=\"sbody-td\">Not applicable</td>"))  //HARDCODEDMSHTML
                        {
                            Console.WriteLine("NOTE: File Information Not applicable");
                            //(also catched below)
                            continue;
                        }

                        //*****************************************************************************************
                        //We analyze the Block Security update file name
                        List<string> lKBToParse = new List<string>();
                        if (bFileInformationBlock)//Not a Block Security Update
                        {
                            //The File Information are in this current MS- page (with no Blocks Security Updates)
                            //<h5 class="sbody-h5 text-subtitle">For all supported x64-based versions</h5><div class="kb-collapsible kb-collapsible-collapsed"><div class="table-responsive"><table class="sbody-table table"><tbody><tr class="sbody-tr"><th class="sbody-th">File name</th><th class="sbody-th">File version</th><th class="sbody-th">File size</th><th class="sbody-th">Date</th><th class="sbody-th">Time</th><th class="sbody-th">Platform</th></tr>

                            lFilenames = new List<string>();
                            lFileversions = new List<string>();
                            lFiledates = new List<string>();
                            lFileplatforms = new List<string>();

                            //TODO?
                            //excludeproduct
                            //...
                            //see below

                            ////fParseKBForFiles(sKBFileContent, sFileNameToSearchReplaced);
                            //fParseKBForFiles(myKBProductBlock, sFileNameToSearch);
                            fParseKBForFiles(myKBProductBlock, lFileNamesToSearch); //Now use a list

                        }
                        else
                        {
                            #region blocksecurityupdatefilename
                            #region getblocksecurityupdate
                            //US
                            string sKBSecurityUpdateInformationBlock = "Security update file name";  //(s)HARDCODEDMSHTML
                            Regex myRegexKBSecurityUpdateInformationBlock = new Regex(sKBSecurityUpdateInformationBlock + "(.*?)Installation switches", RegexOptions.Singleline);
                            if (myKBProductBlock.Contains(sKBSecurityUpdateInformationBlock))
                            {
                                sKBSecurityUpdateInformationBlock = myRegexKBSecurityUpdateInformationBlock.Match(myKBProductBlock).ToString();
                            }
                            else
                            {
                                //Internationalization
                                /*
                                //FR
                                sKBSecurityUpdateInformationBlock = "de fichier de la mise";//Nom(s) <snip> a jour de securite";  //Hardcoded french
                                myRegexKBSecurityUpdateInformationBlock = new Regex(sKBSecurityUpdateInformationBlock + "(.*?)Commutateurs", RegexOptions.Singleline); //Commutateurs d'installation
                                if (myKBProductBlock.Contains(sKBSecurityUpdateInformationBlock))
                                {
                                    sKBSecurityUpdateInformationBlock = myRegexKBSecurityUpdateInformationBlock.Match(myKBProductBlock).ToString();
                                }
                                else
                                {
                                */
                                if (sHyperlinkText.Contains(" rt"))
                                {
                                    //Example:  windows rt 8.1  Not applicable
                                    Console.WriteLine("NOTE: sKBSecurityUpdateInformationBlock not found!");
                                }
                                else
                                {
                                    Console.WriteLine("WARNING: sKBSecurityUpdateInformationBlock not found!");

                                    //TODO: go directly to Informations sur les fichiers
                                }
                                continue;
                                //}
                            }
                            #endregion getblocksecurityupdate

                            //We extract the security updates from this block
                            #region blocksecurityupdates

                            Regex myRegexKBSecurityUpdate = new Regex("<td class=\"sbody-td\">(.*?)</td>", RegexOptions.Singleline);    //HARDCODEDMSHTML
                            MatchCollection myKBSecurityUpdates = myRegexKBSecurityUpdate.Matches(sKBSecurityUpdateInformationBlock);

                            foreach (Match matchKBSecurityUpdate in myKBSecurityUpdates)    //For each block/line security update
                            {
                                string sKBSecurityUpdateLower = matchKBSecurityUpdate.ToString().ToLower();
                                if (sKBSecurityUpdateLower.Length < 40) continue;   //Hardcoded "<td class=\"sbody-td\"></td>" ...
                                if (sKBSecurityUpdateLower.Contains("security update file names")) continue;    //HARDCODEDMS

                                Console.WriteLine("DEBUG matchKBSecurityUpdate=" + matchKBSecurityUpdate);
                                //TODO For GDR update of SQL Server     For CU update of SQL Server
                                //TODO For Microsoft IME (Japanese) 2007 Service Pack 3
                                //TODO if(matchKBSecurityUpdate.Contains("For all supported editions of") Windows Server 2012 ==> add Itanium to lProductsFoundInSecurityUpdate

                                //Reinitialization of the variables
                                string sFileNameToSearchReplaced = sFileNameToSearch;
                                //TODO? now lFileNamesToSearch


                                #region excludeproduct1
                                if (bUseOnlyCPEs)
                                {
                                    Boolean bProductExcluded = true;
                                    foreach (string sProductCPE in lProductsCPEs)
                                    {
                                        //Console.WriteLine("DEBUG For ProductExcluded Analysis sProductCPE=" + sProductCPE);
                                        //Check if the Product in the KBSecurityUpdate matches one in the CVE's CPEs list
                                        //if (sHyperlinkText.Contains(sProductCPE) || sKBSecurityUpdateLower.Contains(sProductCPE) || sKBSecurityUpdateLower.Replace("pack de compatibilite microsoft office","office compatibility pack").Replace(" service pack ", " sp").Contains(sProductCPE))
                                        if (sHyperlinkText.Contains(sProductCPE))
                                        {
                                            bProductExcluded = false;
                                            break;
                                        }
                                        if (sKBSecurityUpdateLower.Contains(sProductCPE))
                                        {
                                            bProductExcluded = false;
                                            break;
                                        }
                                        //Example: (CVE-2017-0004)
                                        //CPEs list:
                                        //windows server 2008 r2 sp1
                                        //windows server 2008 sp2
                                        //sKBSecurityUpdateLower=
                                        //For all supported 32-bit editions of Windows Server 2008      (Thank you MS...)
                                        //sProductInBlockH4=windows server 2008
                                        if (sProductCPE.Contains(sProductInBlock) && sKBSecurityUpdateLower.Contains("all supported"))   //HARDCODED TODO Review (all editions?...)
                                        {
                                            //TODO: add itanium
                                            bProductExcluded = false;
                                            break;
                                        }
                                        if (sKBSecurityUpdateLower.Replace("pack de compatibilite microsoft office", "office compatibility pack").Replace(" service pack ", " sp").Contains(sProductCPE))    //Hardcoded + French
                                        {
                                            bProductExcluded = false;
                                            break;
                                        }
                                        //Word Automation Services
                                        //Microsoft Office Web Apps

                                    }
                                    if (bProductExcluded)
                                    {
                                        Console.WriteLine("DEBUG: *** EXCLUDED by bProductExcluded1 ***");
                                        continue;
                                    }
                                }
                                #endregion excludeproduct1




                                #region getmainproductnameextended
                                List<string> lProductsFoundInSecurityUpdate = new List<string>();
                                //Retrieve all the Products Names in the Security Update Block Line
                                //lProductsFoundInSecurityUpdate = fGetExactProductName(matchKBSecurityUpdate.ToString(), sProductInBlock, sProductInBlock, oVulnerability.VULDescription, lProductsMicrosoft, sMainProductName);
                                lProductsFoundInSecurityUpdate = fGetExactProductName(matchKBSecurityUpdate.ToString(), sProductInBlock, sProductInBlock, sVULDescription, lProductsMicrosoft, sMainProductName);

                                List<string> lMainProductsNamesExtended = new List<string>();

                                string sMainProductNameExtended = "";
                                if (sMainProductName != "")// || lMainProductsNames.Count()>1)    //i.e.  internet explorer
                                {
                                    foreach (string sProductFound in lProductsFoundInSecurityUpdate)
                                    {
                                        if (sProductFound.Contains(sMainProductName))// && sProductFound != sMainProductName)   //i.e   internet explorer 9 contains internet explorer
                                        {
                                            sMainProductNameExtended = sProductFound;
                                            if (!lMainProductsNamesExtended.Contains(sProductFound)) lMainProductsNamesExtended.Add(sProductFound);
                                            //break;?
                                        }
                                    }
                                    //Thanks MS...
                                    if (sMainProductNameExtended == "")
                                    {
                                        sMainProductNameExtended = sMainProductName;
                                        if (!lMainProductsNamesExtended.Contains(sMainProductName)) lMainProductsNamesExtended.Add(sMainProductName);

                                    }
                                }
                                Console.WriteLine("DEBUG sMainProductNameExtended=" + sMainProductNameExtended);

                                if (sMainProductNameExtended == "" && lMainProductsNames.Count() > 1)
                                {
                                    foreach (string sProductFound in lProductsFoundInSecurityUpdate)
                                    {
                                        foreach (string sMainProduct in lMainProductsNames)
                                        {
                                            if (sProductFound.Contains(sMainProduct))// && sProductFound != sMainProductName)   //i.e   internet explorer 9 contains internet explorer
                                            {
                                                sMainProductNameExtended = sProductFound;
                                                if (!lMainProductsNamesExtended.Contains(sProductFound)) lMainProductsNamesExtended.Add(sProductFound);

                                                //break?
                                            }
                                        }
                                        //if (sMainProductNameExtended != "") break;?
                                    }
                                }
                                if (sMainProductNameExtended == "")
                                {
                                    sMainProductNameExtended = "JOKER"; //HARDCODEDJA
                                    if (!lMainProductsNames.Contains("JOKER")) lMainProductsNames.Add("JOKER");
                                    if (!lMainProductsNamesExtended.Contains("JOKER")) lMainProductsNamesExtended.Add("JOKER");

                                }
                                Console.WriteLine("DEBUG sMainProductNameExtended2=" + sMainProductNameExtended);
                                #endregion getmainproductnameextended



                                foreach (string sCurrentMainProductNameExtended in lMainProductsNamesExtended)//Because we could have found multiple products e.g. .net framework 4.5/4.5.1/4.5.2
                                {
                                    string sCurrentMainProductNameExtendedModified = sCurrentMainProductNameExtended;
                                    foreach (string sProductFound in lProductsFoundInSecurityUpdate)
                                    {
                                        Console.WriteLine("DEBUG sProductFoundInSecurityUpdate=" + sProductFound);  //windows server 2008 x86
                                        if (sCurrentMainProductNameExtendedModified != "" && !sProductFound.StartsWith("windows") && !sProductFound.StartsWith("microsoft windows"))    //microsoft silverlight 5 when installed on all supported 32-bit releases of microsoft windows
                                        {
                                            Console.WriteLine("DEBUG Excluded13");
                                            continue;
                                        }

                                        #region excludeproduct2
                                        if (bUseOnlyCPEs)
                                        {
                                            Boolean bProductExcluded = true;
                                            foreach (string sProductCPE in lProductsCPEs)
                                            {
                                                if (sProductFound.Contains(sProductCPE) || sProductFound.Replace(" service pack ", " sp").Contains(sProductCPE)) //Hardcoded
                                                {
                                                    bProductExcluded = false;
                                                    break;
                                                }
                                                if (sProductFound.Contains(sProductCPE.Replace(" enterprise", "")) || sProductFound.Replace(" service pack ", " sp").Contains(sProductCPE.Replace(" enterprise", "")))  //Hardcoded
                                                {
                                                    bProductExcluded = false;
                                                    break;
                                                }
                                                //TODO Review (see the "same" function used before) Note: no x86/x64/itanium in sProductCPE as of today

                                                if ((sProductCPE.Contains(sProductInBlock) || sProductCPE.Contains(sProductFound.Replace(" service pack ", " sp"))) && sKBSecurityUpdateLower.Contains("all supported"))   //HARDCODED TODO Review (all editions?...)
                                                {
                                                    bProductExcluded = false;
                                                    break;
                                                }
                                            }
                                            if (bProductExcluded)
                                            {
                                                Console.WriteLine("DEBUG: $$$ EXCLUDED by bProductExcluded2 $$$");
                                                continue;
                                            }
                                        }
                                        #endregion excludeproduct2

                                        #region analyzeproductfoundforfilenametosearch
                                        //TODO XORCISM with AI

                                        //sFileNameToSearchReplaced = fHardcodeVulnerableFilename(sProductFound);
                                        List<string> lFilesForProduct = new List<string>();
                                        lFilesForProduct = fGetFilenamesToSearchForProduct(sProductFound);  //TODO Review do this if StartsWith("windows")? (and not windows kernel...)
                                                                                                            //TODO Review   now use lFileNamesToSearch?

                                        //TODO we could have the FileNameToSearch here
                                        //i.e. usp102010-kb2889841-fullfile-x86-glb.exe
                                        if (sKBSecurityUpdateLower.Contains("usp10"))
                                        {
                                            sFileNameToSearchReplaced = "usp10.dll";
                                            if (!lFileNamesToSearch.Contains("usp10.dll")) lFileNamesToSearch.Add("usp10.dll");
                                            Console.WriteLine("DEBUG sFileNameToSearchReplaced1=" + sFileNameToSearchReplaced);
                                        }
                                        //KB3118394-Word_Viewer-gdiplus.cab
                                        if (sKBSecurityUpdateLower.Contains("gdiplus"))
                                        {
                                            sFileNameToSearchReplaced = "gdiplus.dll";
                                            if (!lFileNamesToSearch.Contains("gdiplus.dll")) lFileNamesToSearch.Add("gdiplus.dll");
                                            Console.WriteLine("DEBUG sFileNameToSearchReplaced1=" + sFileNameToSearchReplaced);
                                        }
                                        //mso2016-kb3127986-fullfile-x64-glb.exe
                                        //mso.dll
                                        //xlsrvapp2007-kb3127892-fullfile-x86-glb.exe
                                        //excel2007-kb3128019-fullfile-x86-glb.exe
                                        //word2007-kb3128025-fullfile-x86-glb.exe

                                        //sFileNameToSearchReplaced = fFilenameToSearchReplacedHardcoded(sProductFound);

                                        #region filenametosearchhardcoded

                                        if (sFileNameToSearchReplaced == "" && sProductFound.Contains("excel viewer"))
                                        {
                                            sFileNameToSearchReplaced = "xlview.exe"; //Hardcoded
                                            if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                                        }
                                        if (sFileNameToSearchReplaced == "" && sProductFound.Contains("excel services"))
                                        {
                                            sFileNameToSearchReplaced = "xlsrvintl.dll"; //Hardcoded    (good luck with .resx...)
                                            if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                                            sFileNameToSearchReplaced = "xlsrv.dll"; //Hardcoded
                                            if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                                        }
                                        //word automation services
                                        if (sFileNameToSearchReplaced == "" && sProductFound.Contains("excel"))
                                        {
                                            sFileNameToSearchReplaced = "excel.exe"; //Hardcoded
                                            if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                                        }
                                        if (sFileNameToSearchReplaced == "" && sProductFound.Contains("powerpoint"))
                                        {
                                            sFileNameToSearchReplaced = "ppcore.dll"; //Hardcoded
                                            if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                                            sFileNameToSearchReplaced = "powerpnt.exe"; //Hardcoded
                                            if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                                        }

                                        if (sFileNameToSearchReplaced == "" && sProductFound.Contains("word viewer"))
                                        {
                                            sFileNameToSearchReplaced = "wordview.exe"; //Hardcoded
                                            if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                                        }
                                        if (sProductFound.Contains("word"))
                                        {
                                            sFileNameToSearchReplaced = "winword.exe"; //Hardcoded
                                            if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                                        }
                                        if (sFileNameToSearchReplaced == "" && sProductFound.Contains("publisher"))
                                        {
                                            sFileNameToSearchReplaced = "mspub.exe"; //Hardcoded
                                            if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                                        }
                                        if (sFileNameToSearchReplaced == "" && (sProductFound.Contains("skype") || sProductFound.Contains("lync")))
                                        {
                                            sFileNameToSearchReplaced = "lync.exe"; //Hardcoded     lync99.exe      lynchtmlconv.exe        lmaddins.dll
                                            if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                                        }
                                        if (sFileNameToSearchReplaced == "" && sProductFound.Contains("access"))
                                        {
                                            sFileNameToSearchReplaced = "msaccess.exe"; //Hardcoded
                                            if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                                        }
                                        if (sFileNameToSearchReplaced == "" && sProductFound.Contains("infopath"))
                                        {
                                            sFileNameToSearchReplaced = "infopath.exe"; //Hardcoded
                                            if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                                        }
                                        if (sFileNameToSearchReplaced == "" && sProductFound.Contains("onenote"))
                                        {
                                            sFileNameToSearchReplaced = "onenote.exe"; //Hardcoded
                                            if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                                        }
                                        if (sProductFound.Contains("project"))
                                        {
                                            sFileNameToSearchReplaced = "winproj.exe"; //Hardcoded
                                            if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                                        }
                                        if (sProductFound.Contains("visio"))
                                        {
                                            sFileNameToSearchReplaced = "visio.exe"; //Hardcoded
                                            if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                                        }
                                        if (sFileNameToSearchReplaced == "" && sProductFound.Contains("office compatibility pack"))// || sProductFound.Contains("pack de compatibilite")))    //Hardcoded + French
                                        {
                                            //TODO: was it Excel?
                                            //xlconv2007-kb3128022-fullfile-x86-glb.exe
                                            if (sKBSecurityUpdateLower.Contains("xlconv"))
                                            {
                                                sFileNameToSearchReplaced = "xl12cnv.exe"; //Hardcoded
                                                if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);

                                                sFileNameToSearchReplaced = "excelcnv.exe"; //Hardcoded //excelcnv.exe|excelconv.exe
                                                if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                                            }
                                            //wordconv2007-kb3128024-fullfile-x86-glb.exe
                                            if (sKBSecurityUpdateLower.Contains("wordconv"))
                                            {
                                                sFileNameToSearchReplaced = "wrd12cnv.dll"; //Hardcoded
                                                if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);

                                                sFileNameToSearchReplaced = "wordcnv.dll"; //Hardcoded
                                                if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                                            }
                                        }
                                        //Microsoft Office Web Apps 2010 (all versions)
                                        if (sKBSecurityUpdateLower.Contains("web apps"))
                                        {
                                            sFileNameToSearchReplaced = "msoserver.dll"; //Hardcoded
                                                                                         //sword.dll
                                            if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                                            //Wdsrv.conversion.office.msoserver.dll
                                        }
                                        if (sFileNameToSearchReplaced == "" && sProductFound.Contains("sharepoint server")) //sharepoint foundation?
                                        {
                                            if (sKBSecurityUpdateLower.Contains("xlsrv"))
                                            {
                                                sFileNameToSearchReplaced = "xlsrv.dll";
                                                if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                                            }
                                            if (sKBSecurityUpdateLower.Contains("wdsrv"))
                                            {
                                                sFileNameToSearchReplaced = "sword.dll";
                                                if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                                            }
                                            if (sKBSecurityUpdateLower.Contains("word automation"))
                                            {
                                                sFileNameToSearchReplaced = "sword.dll";
                                                if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                                            }
                                            //if (sFileNameToSearchReplaced == "" && oVulnerability.VULDescription.ToLower().Contains("word")) sFileNameToSearchReplaced = "sword.dll";
                                            if (sFileNameToSearchReplaced == "" && sVULDescription.ToLower().Contains("word")) sFileNameToSearchReplaced = "sword.dll";

                                            //TODO: if other Products Found contains word/excel
                                            if (sFileNameToSearchReplaced == "")
                                            {
                                                sFileNameToSearchReplaced = "sword.dll";
                                                if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                                                //Wdsrv.conversion.sword.dll
                                            }
                                            //stswel.dll
                                            //wwintl.dll
                                            //vutils.dll
                                        }
                                        if (sFileNameToSearchReplaced == "" && sKBSecurityUpdateLower.Contains("mso"))
                                        {
                                            sFileNameToSearchReplaced = "mso.dll";
                                            if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                                        }
                                        //oart.dll
                                        //oartconv.dll


                                        //For Word Automation Services on supported editions of Microsoft SharePoint Server 2010 Service Pack 2:
                                        //msoserver.dll     sword.dll
                                        //For Excel Services on supported editions of Microsoft SharePoint Server 2010 Service Pack 2:
                                        //xlsrv.dll

                                        if (sKBSecurityUpdateLower.Contains("hyperlink object library"))
                                        {
                                            sFileNameToSearchReplaced = "hlink.dll";
                                            if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                                        }
                                        if (sKBSecurityUpdateLower.Contains("skype") || sKBSecurityUpdateLower.Contains("lync"))
                                        {

                                            if (!lFileNamesToSearch.Contains("lync.lync.exe")) lFileNamesToSearch.Add("lync.lync.exe");
                                        }

                                        if (sKBSecurityUpdateLower.Contains("office"))
                                        {
                                            if (!lFileNamesToSearch.Contains("wwlibcxm.dll")) lFileNamesToSearch.Add("wwlibcxm.dll");
                                            if (sFileNameToSearchReplaced == "")
                                            {
                                                sFileNameToSearchReplaced = "usp10.dll";
                                            }
                                            if (!lFileNamesToSearch.Contains("usp10.dll")) lFileNamesToSearch.Add("usp10.dll");
                                        }
                                        #endregion filenametosearchhardcoded

                                        //if (sFileNameToSearchReplaced != "") Console.WriteLine("DEBUG sFileNameToSearchReplaced=" + sFileNameToSearchReplaced);
                                        #endregion analyzeproductfoundforfilenametosearch

                                        //TODO Review
                                        if (sCurrentMainProductNameExtended == "microsoft office")
                                        {
                                            Console.WriteLine("DEBUG CAUSE microsoft office: sCurrentMainProductNameExtended=");
                                            sCurrentMainProductNameExtendedModified = "";
                                        }

                                        //TODO?
                                        /*
                                        if(sCurrentMainProductNameExtended==sProductFound)
                                        {
                                            Console.WriteLine("DEBUG sCurrentMainProductNameExtended=sProductFound: continue");
                                            continue;
                                        }
                                        */

                                        string sProductFoundDEADBEEF = sCurrentMainProductNameExtendedModified + "DEADBEEF" + sProductFound;   //HARDCODEDJA Separator Product|Platform
                                        Console.WriteLine("DEBUG sProductFoundDEADBEEF=" + sProductFoundDEADBEEF);
                                        if (sFileNameToSearchReplaced == "" && sProductFoundDEADBEEF.Contains("jscript"))
                                        {
                                            sFileNameToSearchReplaced = "jscript.dll";    //HARDCODED

                                        }
                                        //Jscript9.dll
                                        if (sFileNameToSearchReplaced == "" && sProductFoundDEADBEEF.Contains("vbcript")) sFileNameToSearchReplaced = "vbcript.dll";    //HARDCODED
                                        if (sFileNameToSearchReplaced != "" && !lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);

                                        #region getKBnumber
                                        //Console.WriteLine("DEBUG Get the KB number");
                                        //Get the KB number(s) from the Security Update Block Line
                                        //Windows10.0-KB3205383-x86.msu
                                        //Windows8-RT-KB</span><span class="text-base">3205408-x64.msu
                                        //<span class="text-base">word2016-kb<span class="text-base">3118331</span>-fullfile-x64-glb.exe</span>
                                        //  OR
                                        //https://catalog.update.microsoft.com/v7/site/search.aspx?q=kb3197874
                                        /*
                                        string sMyKB = sKBSecurityUpdateLower.Replace("</span><span class=\"text-base\">", "");
                                        sMyKB = sMyKB.Replace("<span class=\"text-base\">", "");
                                        sMyKB = sMyKB.Replace("</span>", "");
                                        //https://catalog.update.microsoft.com/v7/site/search.aspx?q=kb3197874<br /></span>monthly roll-up
                                        sMyKB = sMyKB.Replace("<br />", "");
                                        sMyKB = sMyKB.Replace("monthly roll-up", "");  ////Gives us a "-"
                                        sMyKB = sMyKB.Replace("monthly rollup", "");  //Thanks MS...
                                        */
                                        // Remove HTML from string with compiled Regex.
                                        string sMyKB = StripTagsRegexCompiled(sKBSecurityUpdateLower);

                                        //Special case: kb24286772010-kb3128032-fullfile-x86-glb.exe
                                        //Regex myRegexKBNumberExtract = new Regex(@"kb\d+", RegexOptions.Singleline);
                                        Regex myRegexKBNumberExtract = new Regex(@"kb(\d{7})-", RegexOptions.Singleline);

                                        MatchCollection KBNumbers = myRegexKBNumberExtract.Matches(sMyKB.Replace(" ", ""));

                                        if (KBNumbers.Count == 0)
                                        {
                                            //Try2
                                            myRegexKBNumberExtract = new Regex(@"q=kb(\d{7})", RegexOptions.Singleline);
                                            KBNumbers = myRegexKBNumberExtract.Matches(sMyKB);
                                            if (KBNumbers.Count == 0)
                                            {
                                                //Try3  kb3023914_adminconsole_amd64.msp
                                                myRegexKBNumberExtract = new Regex(@"kb(\d{7})_", RegexOptions.Singleline);
                                                KBNumbers = myRegexKBNumberExtract.Matches(sMyKB);

                                                if (KBNumbers.Count == 0)
                                                {
                                                    //Try4
                                                    //(3188397) //TODO REVIEW
                                                    myRegexKBNumberExtract = new Regex(@"\(\d+\)", RegexOptions.Singleline);
                                                    KBNumbers = myRegexKBNumberExtract.Matches(sMyKB);

                                                    if (KBNumbers.Count == 0)
                                                    {
                                                        //Try5  more than 7 digits
                                                        //e.g. kb303081453-win10-rtm-x64-tsl.msu
                                                        myRegexKBNumberExtract = new Regex(@"kb\d+", RegexOptions.Singleline);
                                                        KBNumbers = myRegexKBNumberExtract.Matches(sMyKB);

                                                        if (KBNumbers.Count == 0)
                                                        {
                                                            //Next try
                                                            //TODO
                                                            Console.WriteLine("ERROR: No KBnumber found");
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        #endregion getKBnumber

                                        foreach (Match matchKBnumber in KBNumbers)  //For each KB number found in the Security Update Block Line
                                        {
                                            //TODO Regex \d
                                            //sKBNumber = matchKBnumber.ToString().ToLower().Replace("kb", "").Replace("-", "").Replace("</span>", "");    //HARDCODED //3205408
                                            sKBNumber = matchKBnumber.ToString().Replace("kb", ""); //hardcoded
                                            sKBNumber = sKBNumber.ToString().Replace("-", ""); //hardcoded
                                            sKBNumber = sKBNumber.ToString().Replace("_", ""); //hardcoded
                                            sKBNumber = sKBNumber.ToString().Replace("q=", ""); //hardcoded
                                            sKBNumber = sKBNumber.ToString().Replace("(", ""); //hardcoded
                                            sKBNumber = sKBNumber.ToString().Replace(")", ""); //hardcoded
                                                                                               //Console.WriteLine("DEBUG sKBNumber1=" + sKBNumber);
                                                                                               //if (!lKBToParse.Contains(sKBNumber)) lKBToParse.Add(sKBNumber);

                                            Console.WriteLine("DEBUG sFileNameToSearchReplaced2=" + sFileNameToSearchReplaced);

                                            string sKBURLFinal2 = "https://support.microsoft.com/en-us/kb/" + sKBNumber; //Hardcoded NOTE: we will be redirected
                                            Console.WriteLine("DEBUG sKBURLFinal2=" + sKBURLFinal2);  //https://support.microsoft.com/en-us/kb/3114395

                                            #region findkbfilenameandversion
                                            //DEBUG matchKBSecurityUpdate=<td class="sbody-td">For all supported 32-bit editions of Windows 8.1:<br /><span class="text-base">Windows8.1-KB3192392-x86.msu<br /></span>Security Only</td>
                                            //This is to avoid going to MS catalog website if we already have downloaded the KBfile localy
                                            string sMSKBfilename = string.Empty;
                                            string[] KBFileExtensions = { ".cab", ".msi", ".msp", ".msu", ".exe" };   //HARDCODEDMS
                                            foreach (string sExtensionCheck in KBFileExtensions)
                                            {
                                                //TODO: .exe    office2003-kb3118394-fullfile-enu.exe
                                                if (sKBSecurityUpdateLower.Contains(sExtensionCheck))
                                                {
                                                    //TODO: cleaner     "<" ==> "</td>"?
                                                    Regex myRegexExtractKBfilename = new Regex("<span class=\"text-base\">(.*?)" + sExtensionCheck + "<", RegexOptions.Singleline);    //HARDCODEDMSHTML
                                                    sMSKBfilename = myRegexExtractKBfilename.Match(sKBSecurityUpdateLower).ToString();
                                                    sMSKBfilename = StripTagsRegexCompiled(sMSKBfilename);
                                                    //sMSKBfilename = sMSKBfilename.Replace("<span class=\"text-base\">", "").Replace("</span>", "").Replace("<br />", "").Replace("<", "").Replace(" ","");   //HARDCODEDMSHTML
                                                    sMSKBfilename = sMSKBfilename.Replace("<", "").Replace(" ", "").Trim();   //HARDCODEDMSHTML

                                                    break;
                                                }
                                            }

                                            string sFileDateVersion = string.Empty;

                                            if (sMSKBfilename != string.Empty)
                                            {
                                                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                Console.WriteLine("DEBUG sMSKBfilename=" + sMSKBfilename);  //Windows8.1-KB3192392-x86.msu

                                                #region searchfileinfoinKBfolder
                                                //Search its location, if it exists
                                                //Note: we assume it was already extracted (by bluepill), and so we search for a directory
                                                try
                                                {
                                                    string[] directoryEntries = Directory.GetDirectories(sLocalPathForMSKBfiles, Path.GetFileNameWithoutExtension(sMSKBfilename), SearchOption.AllDirectories);
                                                    //expect just 1
                                                    foreach (string directoryPath in directoryEntries)
                                                    {
                                                        //Search into the directory (where the KBfile was extracted) for the FileNameToSearch (i.e. gdiplus.dll) (((or its manifest, version.txt)))...
                                                        //string[] fileEntries = Directory.GetFiles(directoryPath);
                                                        //foreach (string fileName in fileEntries)
                                                        foreach (string fileName in Directory.EnumerateFiles(directoryPath, "*", SearchOption.AllDirectories))  //TODO? Filter extension
                                                        {
                                                            //TODO  Review this filter (better location elsewhere)
                                                            if (sProductFound.Contains("x86") && !fileName.Contains("x86") && !fileName.ToLower().Contains("32-bit")) continue;
                                                            if (sProductFound.Contains("x64") && !fileName.Contains("x64") && !fileName.ToLower().Contains("64-bit")) continue;
                                                            if (sProductFound.Contains("x64") && fileName.Contains("x86_")) continue;
                                                            if (sProductFound.Contains("itanium") && !fileName.Contains("ia64") && !fileName.ToLower().Contains("itanium")) continue;
                                                            //ia-64
                                                            string sProductNameReduced = sProductFound.ToLower().Replace("microsoft ", "").Replace("x86", "").Replace("x64", "").Trim();  //Hardcoded    //i.e. windows vista x86

                                                            //string sProductNameReduced = sProductName.Replace("x86", "").Replace("x64", "");  //Hardcoded
                                                            if (!fileName.ToLower().Contains(sProductNameReduced.Replace(" ", "_").ToLower()))  //TODO Review
                                                            {
                                                                //Not the right KBfile
                                                                continue;
                                                                //Console.WriteLine("DEBUG ")
                                                            }

                                                            //if (Path.GetExtension(fileName) == Path.GetExtension(sFileNameToSearchReplaced) || Path.GetExtension(fileName) == ".manifest")  //Hardcoded
                                                            //{
                                                            foreach (string sFileNameSearched in lFileNamesToSearch)
                                                            {
                                                                //TODO Review: we could have nultiple files/versions
                                                                sFileDateVersion = fCheckFileAndGetVersion(fileName, sFileNameSearched);    //sFileNameToSearchReplaced);
                                                                                                                                            //if (sFileDateVersion != string.Empty) break;
                                                            }
                                                            //}
                                                            //if (sFileDateVersion != string.Empty) break;
                                                        }
                                                        //if (sFileDateVersion != string.Empty) break;
                                                    }
                                                }
                                                catch (Exception exSearchFileInfoInKBFolder)
                                                {
                                                    Console.WriteLine("Exception: exSearchFileInfoInKBFolder " + exSearchFileInfoInKBFolder.Message + " " + exSearchFileInfoInKBFolder.InnerException);
                                                }
                                                #endregion searchfileinfoinKBfolder
                                            }
                                            #endregion findkbfilenameandversion

                                            if (sFileDateVersion != string.Empty)
                                            {
                                                Console.WriteLine("DEBUG sFileDateVersionFinal=" + sFileDateVersion);
                                                //TODO Review
                                                if (!dKBURLFile.ContainsKey(sKBURLFinal2)) dKBURLFile.Add(sKBURLFinal2, sFileDateVersion.ToLower());

                                                ////TODO lFilesToUse.Add();   //We keep track of the Files that we use so we can harmonize them in the final XML. e.g.: 20161005 10.0.14393.321 win32kbase.sys and 20161005 10.0.14393.321 win32kfull.sys (same date/version) if we already used win32kfull.sys we would give it preference

                                            }
                                            else
                                            {
                                                //TODO?
                                                //bluepill here (if we want to always download the MSKBfiles)
                                            }

                                            #region parsekbforfiles
                                            try
                                            {
                                                //Chances are that we already found and parsed it before
                                                //To avoid parsing the same KBfile multiple times
                                                if (dKBURLFile.ContainsKey(sKBURLFinal2)) //We already know the file for this KBurl //(we will continue;)
                                                {
                                                    Console.WriteLine("DEBUG FileToUse3: " + dKBURLFile[sKBURLFinal2]);
                                                    //17-Nov-2016 ptxt9.dll 14.0.7177.5000
                                                    if (dKBURLFile[sKBURLFinal2] != "WARNING: No file found3!")    //Hardcoded
                                                    {
                                                        if (dProductFile.ContainsKey(sProductFoundDEADBEEF))
                                                        {
                                                            Console.WriteLine("DEBUG dProductFile already contains " + sProductFoundDEADBEEF);

                                                            #region keeplowestversion1
                                                            //Console.WriteLine("ERROR02: TODO dProductFile already contains " + sProduct);
                                                            //Analyze the situation: at the end, we will keep the lowest version number
                                                            if (dProductFile[sProductFoundDEADBEEF] == dKBURLFile[sKBURLFinal2])
                                                            {
                                                                //No issue there*
                                                            }
                                                            else
                                                            {
                                                                //Get back the file's information previously added to dKBURLFile
                                                                string[] PreviousFileInfoInKBSplit = dKBURLFile[sKBURLFinal2].Split(' ');
                                                                string sFileVersion = PreviousFileInfoInKBSplit[2];

                                                                //Get back the file's information previously collected for the product
                                                                string[] PreviousFileInfoNeededSplit = dProductFile[sProductFoundDEADBEEF].Split(' ');
                                                                //Is it the same files names
                                                                if (PreviousFileInfoNeededSplit[1] == PreviousFileInfoInKBSplit[1])
                                                                {
                                                                    #region comparefilesversions1
                                                                    //Compare the versions  using Version class //https://stackoverflow.com/questions/7568147/compare-version-numbers-without-using-split-function
                                                                    string v1 = PreviousFileInfoNeededSplit[2]; //"1.23.56.1487";
                                                                    string v2 = sFileVersion;   //"1.24.55.487";

                                                                    var version1 = new Version(v1);
                                                                    var version2 = new Version(v2);

                                                                    var result = version1.CompareTo(version2);
                                                                    if (result > 0)
                                                                    {
                                                                        //Console.WriteLine("version1 is greater");
                                                                        //We replace the value in the dictionary
                                                                        Console.WriteLine("DEBUG1: Previous version found " + v1 + " > new version found " + v2 + " so we keep the new one");
                                                                        dProductFile[sProductFoundDEADBEEF] = dKBURLFile[sKBURLFinal2];
                                                                    }
                                                                    else if (result < 0)
                                                                    {
                                                                        //Console.WriteLine("version2 is greater");
                                                                        //We keep version1
                                                                    }
                                                                    else
                                                                    {
                                                                        //same as before*
                                                                        //Console.WriteLine("versions are equal");
                                                                        //We should be ok (we don't compare the dates)
                                                                    }
                                                                    #endregion comparefilesversions1
                                                                }
                                                                else//different files
                                                                {
                                                                    //TODO!!! Review this (we could use the Dates, or Hardcoding...)
                                                                    if (sFileNameToSearchReplaced != "")
                                                                    {
                                                                        //We keep the new one
                                                                        //i.e.: usp10.dll   =>  mso.dll
                                                                        dProductFile[sProductFoundDEADBEEF] = dKBURLFile[sKBURLFinal2];
                                                                    }
                                                                    else
                                                                    {
                                                                        //HARDCODED
                                                                        if (dKBURLFile[sKBURLFinal2].Contains("mso.dll")) dProductFile[sProductFoundDEADBEEF] = dKBURLFile[sKBURLFinal2];

                                                                    }
                                                                }
                                                            }
                                                            #endregion keeplowestversion1
                                                        }
                                                        else
                                                        {
                                                            dProductFile.Add(sProductFoundDEADBEEF, dKBURLFile[sKBURLFinal2]);
                                                        }
                                                    }
                                                    //continue;  //WARNING: filestate    productpath
                                                }
                                                else
                                                {
                                                    #region parsefinalkbforfiles
                                                    //Download the (final) KB webpage if needed
                                                    string sKBFilePath = sCurrentPath + @"\MS\KB" + sKBURLFinal2.Replace("https://support.microsoft.com/", "") + ".txt";    //HARDCODED Path
                                                    sKBFilePath = sKBFilePath.Replace("en-us/", "");    //Hardcoded
                                                                                                        //sKBFilePath = sKBFilePath.Replace("fr-fr/", "");    //Hardcoded French (internationalization)
                                                    sKBFilePath = sKBFilePath.Replace("kb/", "");   //Hardcoded

                                                    //fileInfo = new FileInfo(sKBFilePath);
                                                    //if (!fileInfo.Exists)   //Commented because MS often updates this stuff, and they can support few HTTP requests
                                                    //{
                                                    if (!lVisitedURLs.Contains(sKBURLFinal2))
                                                    {
                                                        Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                        Console.WriteLine("DEBUG Request to sKBURLFinal2=" + sKBURLFinal2);
                                                        //JavaScript used here, so need a 'browser'
                                                        //OpenQA.Selenium.Chrome.ChromeDriver driver = null;
                                                        try
                                                        {
                                                            using (var driver = new ChromeDriver())
                                                            {
                                                                //driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
                                                                driver.Navigate().GoToUrl(sKBURLFinal2);
                                                                Thread.Sleep(2000 + iSleepMore); //Hardcoded Needed?
                                                                                    //IWebElement username = wait.Until(ExpectedConditions.ElementIsVisible(By.Name("loginfmt")));                                      

                                                                System.IO.File.WriteAllText(sKBFilePath, HttpUtility.HtmlDecode(driver.PageSource).Replace("\u00A0", " "));   //Hardcoded &nbsp;
                                                            }
                                                            lVisitedURLs.Add(sKBURLFinal2);
                                                        }
                                                        catch (Exception exRequestKBURLFinal2)
                                                        {
                                                            Console.WriteLine("Exception: exRequestKBURLFinal2 " + exRequestKBURLFinal2.Message + " " + exRequestKBURLFinal2.InnerException);
                                                        }
                                                    }
                                                    //}

                                                    //Parse the (final) KB for identifying updated files
                                                    string sKBFileContent = System.IO.File.ReadAllText(sKBFilePath);   ////Hardcoded
                                                    int iCptColumns = 0;
                                                    int iCptColumnFileIdentifier = 0;   //Used when "File name" is empty...
                                                    int iCptColumnFileName = 0;
                                                    int iCptColumnFileVersion = 0;
                                                    int iCptColumnDate = 0;
                                                    int iCptColumnPlatform = 0;
                                                    //int iCptFileLines = 0;
                                                    //List<string> lFilenames = new List<string>();     //Now global
                                                    //List<string> lFileversions = new List<string>();
                                                    //List<string> lFiledates = new List<string>();
                                                    //List<string> lFileplatforms = new List<string>();
                                                    lFilenames = new List<string>();
                                                    lFileversions = new List<string>();
                                                    lFiledates = new List<string>();
                                                    lFileplatforms = new List<string>();

                                                    //Note: the updated files are listed directly in the KB (see parsekbforfiles) - OR - in a .CSV
                                                    //http://download.microsoft.com/download/D/D/D/DDD8E2A6-B254-44E2-90D4-3E37CC58AE5F/3205383.csv
                                                    Regex myRegexDownloadCSVlink = new Regex("download.microsoft.com/(.*?).csv", RegexOptions.Singleline);    //HARDCODEDMS
                                                    string sRegexDownloadCSVlink = myRegexDownloadCSVlink.Match(sKBFileContent).ToString(); ////.ToLower()
                                                    if (sRegexDownloadCSVlink != "")    //We found a CSV
                                                    {
                                                        fParseCSV(sRegexDownloadCSVlink, sFileNameToSearchReplaced);
                                                    }
                                                    else
                                                    {
                                                        //Parse KB file
                                                        if (sFileNameToSearchReplaced != "" && !lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                                                        //fParseKBForFiles(sKBFileContent, sFileNameToSearchReplaced);  //Now use a list
                                                        fParseKBForFiles(sKBFileContent, lFileNamesToSearch);
                                                    }

                                                    //TODO: Duplicate code
                                                    ////Sort the arrays to get the file with most recent date
                                                    ////Array.Sort(sFiledates, sFilenames);
                                                    Dictionary<string, Dictionary<string, string>> dFileInfos = fSortFileInfos(sFileNameToSearch); //new Dictionary<string, Dictionary<string, string>>();
                                                    
                                                    if (dFileInfos.Count() > 0)
                                                    {
                                                        fFileInfosRetrievedByParsing(dFileInfos, sFileNameToSearch, sFileNameToSearchReplaced, sProductFoundDEADBEEF, sKBURLFinal2);

                                                    }
                                                    else
                                                    {
                                                        //Console.WriteLine("WARNING: No file found!");
                                                        //dKBURLFile.Add(sKBURLFinal2, "WARNING: No file found!");  //Hardcoded //So we don't parse it again

                                                        //Method 1: Microsoft Update Catalog
                                                        //Method 2: Microsoft Download Center
                                                        //TODO  http://www.catalog.update.microsoft.com/Search.aspx?q=KB3208481
                                                        //string sKBCatalogURL = "http://www.catalog.update.microsoft.com/Search.aspx?q=KB" + sKBNumber;
                                                        //Resistance is futile


                                                        fMSKBCatalogPage(sKBNumber, sFileNameToSearchReplaced, sProductFound, sKBURLFinal2, sProductFoundDEADBEEF, iVulnerabilityID, sMSTitle);

                                                        #region mscatalogpage
                                                        /*
                                                        string sKBCatalogFilePath = sCurrentPath + @"\MS\KBCAT" + sKBNumber + ".txt";   //HARDCODED Path

                                                        //Check if File exists
                                                        fileInfo = new FileInfo(sKBCatalogFilePath);
                                                        if (!fileInfo.Exists)//We assume that if we already visited this page, everything worked well in bluepill...
                                                        {
                                                            Console.WriteLine("DEBUG Request to " + sKBCatalogURL);


                                                            //JavaScript used here, so need a 'browser'
                                                            ////OpenQA.Selenium.Chrome.ChromeDriver driver = null;
                                                            //using (var driver = new ChromeDriver())
                                                            //{
                                                            //    driver.Navigate().GoToUrl(sKBCatalogURL);
                                                            //    System.IO.File.WriteAllText(sKBCatalogFilePath, HttpUtility.HtmlDecode(driver.PageSource).Replace("\u00A0", " "));   //Hardcoded &nbsp;
                                                            //    //driver.FindElementByClassName("flatLightBlueButton").Click();
                                                            //}


                                                            Console.WriteLine("DEBUG " + sKBCatalogURL);
                                                            //Parse the Catalog Page File


                                                            //TODO Review we could have multiple files/versions (i.e. LDR/GDR)
                                                            sFileDateVersion = fBluePill(sKBNumber, sFileNameToSearchReplaced, sProductFound);
                                                            if (sFileDateVersion != string.Empty)
                                                            {
                                                                //TODO Review
                                                                if (!dKBURLFile.ContainsKey(sKBURLFinal2)) dKBURLFile.Add(sKBURLFinal2, sFileDateVersion.ToLower());
                                                                if (!lFilesToUse.Contains(sFileNameToSearchReplaced.ToLower())) lFilesToUse.Add(sFileNameToSearchReplaced.ToLower());   //We keep track of the Files that we use so we can harmonize them in the final XML. e.g.: 20161005 10.0.14393.321 win32kbase.sys and 20161005 10.0.14393.321 win32kfull.sys (same date/version) if we already used win32kfull.sys we would give it preference

                                                                if (!dProductFile.ContainsKey(sProductFoundDEADBEEF)) dProductFile.Add(sProductFoundDEADBEEF, sFileDateVersion.ToLower());
                                                            }
                                                        }
                                                        else//Apparently we already visited the KB page, downloaded and extracted the KBfile
                                                        {
                                                            //jack
                                                        }

                                                        if (sFileDateVersion == string.Empty)
                                                        {
                                                            Console.WriteLine("WARNING: No file found!");
                                                            dKBURLFile.Add(sKBURLFinal2, "WARNING: No file found!");  //Hardcoded //So we don't parse it again
                                                        }
                                                        */
                                                        #endregion mscatalogpage

                                                    }
                                                    #endregion parsefinalkbforfiles
                                                }
                                            }
                                            catch (Exception exParseKBForFiles)
                                            {
                                                Console.WriteLine("Exception: exParseKBForFiles " + exParseKBForFiles.Message + " " + exParseKBForFiles.InnerException);
                                            }
                                            #endregion parsekbforfiles
                                        }
                                        //}
                                        //}
                                        //}
                                        //else  //Monthly rollup    //Correctif cumulatif mensuel
                                    }
                                }
                            }
                            #endregion blocksecurityupdates
                            #endregion blocksecurityupdatefilename
                        }

                    }
                    #endregion blockfoundforoneproduct
                    //}
                    #endregion blockproducth4
                }
            }
            //}
            //}   //foreach(string sProduct in lProductsMicrosoft)



            Console.WriteLine("DEBUG #########################################################################");
            //Display of all the interesting files information collected
            if (bDebugFileSelection)
            {
                foreach (var x in dProductFile.OrderByDescending(o => o.Value))
                {
                    Console.WriteLine("DEBUG dProductFile: " + x.Key + "-" + x.Value);
                }
            }

            /*
            //We eliminate the MainProducts where we found no file, and we improve the list
            List<string> lMainProductsToRemove = new List<string>();
            List<string> lMainProductsImproved = new List<string>();

            foreach (string sMainProductFound in lMainProductsNames)
            {
                Console.WriteLine("DEBUG sMainProductFoundRemaining=" + sMainProductFound);
                bool bFileFoundForMainProduct = false;
                foreach (var x in dProductFile)//.OrderByDescending(o => o.Value))
                {
                    //Console.WriteLine("DEBUG dProductFile: " + x.Key + " " + x.Value);
                    if (x.Key.Contains(sMainProductFound))   //microsoft lync 2013 contains microsoft lync 2013
                    {
                        bFileFoundForMainProduct = true;
                        //break;
                        //
                        string[] sxKeySplit = x.Key.Split(new string[] { "DEADBEEF" }, StringSplitOptions.None);
                        string sMyProductName = sxKeySplit[0];
                        if (sMyProductName.Trim() == "" || sMyProductName == "windows kernel")  //Hardcoded
                        {
                            sMyProductName = sxKeySplit[1];
                        }

                    }
                }
                if (!bFileFoundForMainProduct) lMainProductsToRemove.Add(sMainProductFound);
            }
            foreach(string sMainProductToRemove in lMainProductsToRemove)
            {
                Console.WriteLine("DEBUG Eliminating sMainProduct=" + sMainProductToRemove);
                lMainProductsNames.Remove(sMainProductToRemove);
            }
            */
            
        }

        public static void fFileInfosRetrievedByParsing(Dictionary<string, Dictionary<string, string>> dFileInfos, string sFileNameToSearch, string sFileNameToSearchReplaced, string sProductFoundDEADBEEF, string sKBURLFinal2)
        {

            #region fileinfosretrievedbyparsing
            //TODO REVIEW NOT ALWAYS THE FIRST ONE IS THE GOOD ONE (take the one specified, or the highest version for a file)
            var itemFileInfo = dFileInfos.OrderByDescending(i => i.Key).First();    //By Default (latest date)
            string sFileInfoLatestDate = itemFileInfo.Key;
            Console.WriteLine("DEBUG sFileInfoLatestDate=" + sFileInfoLatestDate);  //Could this be wrong if 1st one selected from KB page?

            Dictionary<string, string> dTempFilenameFileversion3 = new Dictionary<string, string>();
            dTempFilenameFileversion3 = itemFileInfo.Value;    //filename version
        
            if (dTempFilenameFileversion3.First().Key == sFileNameToSearch || lFileNamesToSearch.Contains(dTempFilenameFileversion3.First().Key) || lFilesToUse.Contains(dTempFilenameFileversion3.First().Key))
            {
                //That's our favorite option (the one FileName specified in arguments OR one from our list of files)
                if(bDebugFileSelection) Console.WriteLine("JA1");
                //TODO: BUT we could be looking for multiple files    e.g. CVE-2016-0002  vbscript.dll + jscript.dll
            }
            else
            {
                //Console.WriteLine("JA9");
                var itemsFileInfo = dFileInfos.OrderByDescending(i => i.Key);   //By Date Desc
                int iCptItems = 0;
                foreach (var itemFileInforeview in itemsFileInfo)
                {
                    iCptItems++;
                    if(bDebugFileSelection) Console.WriteLine("JA8");
                    if (iCptItems != 1) //Skip the first one equal to itemFileInfo (Default)
                    {
                        if (bDebugFileSelection) Console.WriteLine("JA7");
                        if (itemFileInforeview.Key == itemFileInfo.Key)  //Same Dates
                        {
                            if (bDebugFileSelection) Console.WriteLine("JA2");
                            //Check the FileName
                            Dictionary<string, string> dTempFilenameFileversion2 = new Dictionary<string, string>();
                            dTempFilenameFileversion2 = itemFileInforeview.Value;
                            if (dTempFilenameFileversion2.First().Key == sFileNameToSearch || lFileNamesToSearch.Contains(dTempFilenameFileversion2.First().Key) || lFilesToUse.Contains(dTempFilenameFileversion2.First().Key))
                            {
                                //We keep our favorite option (the one FileName specified in arguments or in our list)
                                if (sFileNameToSearch != string.Empty && sFileNameToSearch != "" && dTempFilenameFileversion2.First().Key == sFileNameToSearch)
                                {
                                    //BEST Option
                                    itemFileInfo = itemFileInforeview;
                                    //TODO: if we are looking for multiple files
                                    if (bDebugFileSelection) Console.WriteLine("JA3");
                                    //break;
                                }
                                else
                                {
                                    dTempFilenameFileversion3 = new Dictionary<string, string>();
                                    dTempFilenameFileversion3 = itemFileInfo.Value;    //filename version
                                    if (dTempFilenameFileversion3.First().Key == sFileNameToSearch)
                                    {
                                        //Previous is better
                                        if (bDebugFileSelection) Console.WriteLine("JA0");
                                    }
                                    else
                                    {
                                        itemFileInfo = itemFileInforeview;
                                        //TODO: if we are looking for multiple files
                                        //lFilesToUse
                                        if (bDebugFileSelection) Console.WriteLine("JA33");
                                        //break;
                                    }
                                }
                            }
                            else
                            {
                                dTempFilenameFileversion3 = new Dictionary<string, string>();
                                dTempFilenameFileversion3 = itemFileInfo.Value;    //filename version
                                                                                    //Take the highest version number
                                string v1 = dTempFilenameFileversion3.First().Value;
                                string v2 = dTempFilenameFileversion2.First().Value;

                                var version1 = new Version(v1);
                                var version2 = new Version(v2);

                                var result = version1.CompareTo(version2);
                                if (result > 0)
                                {
                                    //Console.WriteLine("version1 is greater");
                                    //Console.WriteLine("DEBUG: Previous version found " + v1 + " > new version found " + v2 + " so we keep the old one");
                                }
                                else
                                {
                                    if (bDebugFileSelection) Console.WriteLine("JA13");
                                    itemFileInfo = itemFileInforeview;
                                }

                            }
                        }
                        else//Different Dates
                        {
                            try
                            {
                                //Check the FileName
                                Dictionary<string, string> dTempFilenameFileversion2 = new Dictionary<string, string>();
                                dTempFilenameFileversion2 = itemFileInforeview.Value;
                                if (dTempFilenameFileversion2.First().Key == sFileNameToSearch || lFileNamesToSearch.Contains(dTempFilenameFileversion2.First().Key) || lFilesToUse.Contains(dTempFilenameFileversion2.First().Key))
                                {
                                    //the one FileName specified in arguments OR one from our list of files
                                    if (bDebugFileSelection) Console.WriteLine("JA24");

                                    //Compare the dates difference
                                    //itemFileInforeview.Key == itemFileInfo.Key
                                    int iLatestDate = Int32.Parse(sFileInfoLatestDate);
                                    int iCurrentFileDate = Int32.Parse(itemFileInforeview.Key);
                                    if (iLatestDate - iCurrentFileDate <= 40) //HARDCODED 40 days
                                    {
                                        //We keep our favorite option (the one FileName specified in arguments or in our list)
                                        itemFileInfo = itemFileInforeview;
                                        if (bDebugFileSelection) Console.WriteLine("JA25");
                                        if (dTempFilenameFileversion2.First().Key == sFileNameToSearch || dTempFilenameFileversion2.First().Key == sFileNameToSearchReplaced) break;  //else we continue to search for sFileNameToSearch
                                    }
                                    else
                                    {
                                        if (bDebugFileSelection) Console.WriteLine("JA26");
                                        break;  //The file is 'too old'
                                    }
                                }
                                else
                                {
                                    //We keep the latest one
                                    if (bDebugFileSelection) Console.WriteLine("JA4");
                                    //break;
                                }
                            }
                            catch (Exception exFileInfoDifferentDates)
                            {
                                Console.WriteLine("Exception: exFileInfoDifferentDates " + exFileInfoDifferentDates.Message + " " + exFileInfoDifferentDates.InnerException);
                            }
                        }
                    }

                }
                //To be sure
                dTempFilenameFileversion3 = new Dictionary<string, string>();
                dTempFilenameFileversion3 = itemFileInfo.Value;
            }

            //Dictionary<string, string> dTemp3 = new Dictionary<string, string>();
            //dTemp3 = new Dictionary<string, string>();
            //dTemp3 = item.Value;

            string sFileVersion = dTempFilenameFileversion3.First().Value; //We could be wrong

            string sFileInfoNeeded = itemFileInfo.Key + " " + dTempFilenameFileversion3.First().Key + " " + sFileVersion;  //date filename version


            //Console.WriteLine("DEBUG " + DateTimeOffset.Now);
            Console.WriteLine("DEBUG FileToUse1B: " + sFileInfoNeeded);
            //17-Nov-2016 ptxt9.dll 14.0.7177.5000
            if (dProductFile.ContainsKey(sProductFoundDEADBEEF))
            {
                #region keeplowestversion2
                //TODO Review for LDR/GDR
                //Console.WriteLine("ERROR02: TODO dProductFile already contains " + sProduct);
                //Analyze the situation: at the end, we will keep the lowest version number
                if (dProductFile[sProductFoundDEADBEEF] == sFileInfoNeeded)
                {
                    //No issue there*
                }
                else
                {
                    //Get back the file's information previously collected
                    string[] PreviousFileInfoNeededSplit = dProductFile[sProductFoundDEADBEEF].Split(' ');
                    //Is it the same file name?
                    if (PreviousFileInfoNeededSplit[1] == dTempFilenameFileversion3.First().Key)
                    {
                        string v1 = string.Empty;
                        string v2 = string.Empty;

                        try
                        {
                            #region comparefilesversions
                            //Compare the versions  using Version class //https://stackoverflow.com/questions/7568147/compare-version-numbers-without-using-split-function
                            v1 = PreviousFileInfoNeededSplit[2]; //"1.23.56.1487";
                            v2 = sFileVersion;   //"1.24.55.487";

                            var version1 = new Version(v1);
                            if (v2 != "")
                            {
                                var version2 = new Version(v2);

                                var result = version1.CompareTo(version2);
                                if (result > 0)
                                {
                                    //Console.WriteLine("version1 is greater");
                                    //We replace the value in the dictionary
                                    Console.WriteLine("DEBUG: Previous version found " + v1 + " > new version found " + v2 + " so we keep the new one");
                                    dProductFile[sProductFoundDEADBEEF] = sFileInfoNeeded;
                                }
                                else if (result < 0)
                                {
                                    //Console.WriteLine("version2 is greater");
                                    //We keep version1
                                }
                                else
                                {
                                    //same as before*
                                    //Console.WriteLine("versions are equal");
                                    //We should be ok (we don't compare the dates)
                                }
                            }
                            else
                            {
                                //We keep version1
                            }

                            #endregion comparefilesversions
                        }
                        catch(Exception exCompareFilesVersions)
                        {
                            Console.WriteLine("Exception: exCompareFilesVersions " + exCompareFilesVersions.Message + " " + exCompareFilesVersions.InnerException+" v1="+v1+" v2="+v2);
                        }
                    }
                    else//different files
                    {
                        //TODO!!! Review this (we could use the Dates, or Hardcoding...)
                        if (sFileNameToSearchReplaced != "")
                        {
                            //We keep the new one
                            //i.e.: usp10.dll   =>  mso.dll
                            dProductFile[sProductFoundDEADBEEF] = sFileInfoNeeded;
                        }
                        else
                        {
                            //HARDCODED
                            if (sFileInfoNeeded.Contains("mso.dll")) dProductFile[sProductFoundDEADBEEF] = sFileInfoNeeded;

                        }
                    }
                }
                #endregion keeplowestversion2
            }
            else
            {
                dProductFile.Add(sProductFoundDEADBEEF, sFileInfoNeeded.ToLower());
            }
            dKBURLFile.Add(sKBURLFinal2, sFileInfoNeeded.ToLower());
            if (!lFilesToUse.Contains(dTempFilenameFileversion3.First().Key.ToLower())) lFilesToUse.Add(dTempFilenameFileversion3.First().Key.ToLower());   //We keep track of the Files that we use so we can harmonize them in the final XML. e.g.: 20161005 10.0.14393.321 win32kbase.sys and 20161005 10.0.14393.321 win32kfull.sys (same date/version) if we already used win32kfull.sys we would give it preference

            /*
            if (!dFileVersionOVALState.ContainsKey(sFileVersion))
            {
                #region filestate
                //Search if we already have a OVALSTATE file_state for this version
                //comment=State matches version less than 14.0.7177.5000
                //version 	datatype=version | operation=less than | value=14.0.7177.5000
                OVALSTATE oOVALState = oval_model.OVALSTATE.FirstOrDefault(o => o.comment.Contains(" less than " + sFileVersion));
                if (oOVALState != null)
                {
                    Console.WriteLine("DEBUG OVALSTATE Found: " + oOVALState.OVALStateIDPattern);
                    dFileVersionOVALState.Add(sFileVersion, oOVALState.OVALStateIDPattern);
                }
                else
                {
                    Console.WriteLine("ERROR: No OVALSTATE Found");
                    //Create one
                }

                #endregion filestate
            }
            */
            #endregion fileinfosretrievedbyparsing
                                                        
        }

        public static Dictionary<string, Dictionary<string, string>> fSortFileInfos(string sFileNameToSearch="")
        {
            
            #region sortfileinfos
            ////Array.Sort(sFiledates, sFilenames);
            Dictionary<string, Dictionary<string, string>> dFileInfos = new Dictionary<string, Dictionary<string, string>>();
            try
            {
                Console.WriteLine("DEBUG lFilenames.CountB=" + lFilenames.Count);
                for (int i = 0; i < lFilenames.Count; i++)
                {
                    try
                    {
                        //TODO  lFileplatforms
                        Dictionary<string, string> dTemp = new Dictionary<string, string>();
                        dTemp.Add(lFilenames[i].ToLower(), lFileversions[i]);
                        //IMPORTANT: if 2+ InterestingFiles with the same Dates:
                        //1)    => take the sFileNameToSearch   (i.e. chakra.dll vs edgehtml.dll)
                        if (dFileInfos.ContainsKey(lFiledates[i]))  //Same FileDate
                        {
                            Dictionary<string, string> dTemp2 = new Dictionary<string, string>();
                            dTemp2 = dFileInfos[lFiledates[i]];
                            if (dTemp2.First().Key == sFileNameToSearch && !lFileNamesNOTToSearch.Contains(dTemp2.First().Key))
                            {
                                //That's our favorite option (the one FileName specified in arguments)
                                //We keep it
                            }
                            else
                            {
                                if ((lFilesToUse.Contains(dTemp2.First().Key) || lFileNamesToSearch.Contains(dTemp2.First().Key)) && !lFileNamesNOTToSearch.Contains(dTemp2.First().Key))
                                {
                                    //2nd best option
                                    //we keep it

                                    if (lFilenames.Count == 1)
                                    {
                                        sFileNameToSearch = dTemp2.First().Key;  //If we found just 1 file, we use it as our sFileNameToSearch
                                        Console.WriteLine("DEBUG ReplacingFileNameToSearch by " + sFileNameToSearch);
                                    }
                                    //TODO: bOnlyOneFile
                                }
                                else
                                {
                                    //We replace it
                                    dFileInfos[lFiledates[i]] = dTemp;
                                }
                            }
                        }
                        //2)    => take the file with the highest version (for this file)
                        //i.e.:
                        //Win32k.sys
                        //Win32kbase.sys
                        //Win32kfull.sys
                        //TODO? Use XORCISM FILEVERSION
                        dFileInfos.Add(lFiledates[i], dTemp);
                    }
                    catch (Exception exAdddFileInfos)
                    {
                        //Duplicate keys (same FileDate)

                    }
                }
            }
            catch (Exception exLoopFilenames)
            {
                Console.WriteLine("Exception: exLoopFilenames " + exLoopFilenames.Message + " " + exLoopFilenames.InnerException);
            }
            // Use OrderBy method on the Date   //TODO Review
            /*
            foreach (var item in dFileInfos.OrderByDescending(i => i.Key))
            {
                //Console.WriteLine("DEBUG " + item);   //[17-Nov-2016, System.Collections.Generic.Dictionary`2[System.String,System.String]]
                Dictionary<string, string> dTemp = new Dictionary<string, string>();
                dTemp = item.Value;
                Console.WriteLine("DEBUG " + item.Key + " " + dTemp.First().Key+ " " + dTemp.First().Value);
                //17-Nov-2016 ptxt9.dll 14.0.7177.5000

                //FILE
                //FILEPRODUCT
                //OVALOBJECTFILE
                //...
            }
            */

            #endregion sortfileinfos

            return dFileInfos;
        }


        public static void fMSKBCatalogPage(string sKBNumber, string sFileNameToSearchReplaced, string sProductFound, string sKBURLFinal2, string sProductFoundDEADBEEF, int iVulnerabilityID, string sMSTitle="")
        {
            if (sKBNumber.Length < 5)
            {
                Console.WriteLine("NOTE: BADKBNumber "+sKBNumber);
                return;
            }
            if (!sKBNumber.ToUpper().StartsWith("KB")) sKBNumber = "KB" + sKBNumber;
            string sFileDateVersion = string.Empty;
            string sKBCatalogURL = "http://www.catalog.update.microsoft.com/Search.aspx?q=" + sKBNumber; //HARDCODEDMS

                                                                        
            string sKBCatalogFilePath = sCurrentPath + @"\MS\KBCAT" + sKBNumber + ".txt";   //HARDCODED Path

            #region AddPatchToXORCISM
            #region patch
            int iPatchID = 0;
            try
            {
                try
                {
                    iPatchID = model.PATCH.Where(o => o.PatchVocabularyID == sKBNumber).FirstOrDefault().PatchID;
                }
                catch(Exception ex)
                {

                }
                if (iPatchID <= 0)
                {
                    Console.WriteLine("DEBUG Adding PATCH");
                    PATCH oPatch = new PATCH();
                    oPatch.CreatedDate = DateTimeOffset.Now;
                    oPatch.PatchVocabularyID = sKBNumber;
                    oPatch.PatchTitle = sMSTitle.Replace("</h2>", "");  //Hardcoded
                                                                        //oPatch.PatchDescription=  //TODO
                                                                        //oPatch.VocabularyID=
                    model.PATCH.Add(oPatch);
                    model.SaveChanges();
                    iPatchID = oPatch.PatchID;
                    bPatchJustAdded = true;
                }
                else
                {
                    //Update PATCH
                }
            }
            catch (Exception exAddPatch)
            {
                Console.WriteLine("Exception: exAddPatch " + exAddPatch.Message + " " + exAddPatch.InnerException);
            }
            #endregion patch

            #region PATCHREFERENCES
            int iReferenceID = 0;
            try
            {
                try {
                    iReferenceID = model.REFERENCE.Where(o => o.ReferenceURL == "https://catalog.update.microsoft.com/v7/site/Search.aspx?q=" + sKBNumber).FirstOrDefault().ReferenceID;    //HARDCODEDMS
                }
                catch (Exception ex)
                {

                }
                if (iReferenceID <= 0)
                {
                    REFERENCE oReferenceKB = new REFERENCE();
                    oReferenceKB.CreatedDate = DateTimeOffset.Now;
                    oReferenceKB.ReferenceURL = "https://catalog.update.microsoft.com/v7/site/Search.aspx?q=" + sKBNumber;    //HARDCODEDMS
                    oReferenceKB.ReferenceSourceID = sKBNumber;
                    oReferenceKB.Source = "Microsoft";
                    //oReferenceKB.SourceTrustLevelID=    //High
                    //oReferenceKB.SourceTrustReasonID=   //Trusted Vendor
                    //oReferenceKB.VocabularyID=
                    //... todo
                    model.REFERENCE.Add(oReferenceKB);
                    model.SaveChanges();
                    iReferenceID = oReferenceKB.ReferenceID;
                }
            }
            catch (Exception exReference)
            {
                Console.WriteLine("Exception: exReference " + exReference.Message + " " + exReference.InnerException);
            }

            if (iPatchID > 0 && iReferenceID > 0)
            {
                try
                {
                    int iPatchReferenceID = 0;
                    try {
                        iPatchReferenceID=model.PATCHREFERENCE.Where(o => o.PatchID == iPatchID && o.ReferenceID == iReferenceID).FirstOrDefault().PatchReferenceID;
                    }
                    catch (Exception ex)
                    {

                    }
                    if (iPatchReferenceID <= 0)
                    {
                        PATCHREFERENCE oPatchReference = new PATCHREFERENCE();
                        oPatchReference.CreatedDate = DateTimeOffset.Now;
                        oPatchReference.PatchID = iPatchID;
                        oPatchReference.ReferenceID = iReferenceID;
                        //oPatchReference.VocabularyID=
                        model.PATCHREFERENCE.Add(oPatchReference);
                        model.SaveChanges();
                    }
                }
                catch (Exception exPatchReference)
                {
                    Console.WriteLine("Exception: exPatchReference " + exPatchReference.Message + " " + exPatchReference.InnerException);
                }
            }
            #endregion PATCHREFERENCES

            #region VulnerabilityPatch
            int iVulnerabilityPatchID = 0;
            try
            {
                try
                {
                    //iVulnerabilityPatchID = vuln_model.VULNERABILITYPATCH.Where(o => o.VulnerabilityID == oVulnerability.VulnerabilityID && o.PatchID == iPatchID).FirstOrDefault().VulnerabilityPatchID;
                    iVulnerabilityPatchID = vuln_model.VULNERABILITYPATCH.Where(o => o.VulnerabilityID == iVulnerabilityID && o.PatchID == iPatchID).FirstOrDefault().VulnerabilityPatchID;
                }
                catch(Exception ex)
                {

                }
                if (iVulnerabilityPatchID <= 0)
                {
                    VULNERABILITYPATCH oVulnPatch = new VULNERABILITYPATCH();
                    oVulnPatch.CreatedDate = DateTimeOffset.Now;
                    oVulnPatch.VulnerabilityID = iVulnerabilityID;
                    oVulnPatch.PatchID = iPatchID;
                    //oVulnPatch.VocabularyID=
                    vuln_model.VULNERABILITYPATCH.Add(oVulnPatch);
                    vuln_model.SaveChanges();
                }
            }
            catch (Exception exVulnerabilityPatch)
            {
                Console.WriteLine("Exception: exVulnerabilityPatch " + exVulnerabilityPatch.Message + " " + exVulnerabilityPatch.InnerException);
            }
            #endregion VulnerabilityPatch
            #endregion AddPatchToXORCISM


            //Check if File exists
            FileInfo fileInfo = new FileInfo(sKBCatalogFilePath);
            //if (!fileInfo.Exists)//We assume that if we already visited this page, everything worked well in bluepill...
            //{
                //Console.WriteLine("DEBUG Request to " + sKBCatalogURL);

                /*
                //JavaScript used here, so need a 'browser'
                //OpenQA.Selenium.Chrome.ChromeDriver driver = null;
                using (var driver = new ChromeDriver())
                {
                    driver.Navigate().GoToUrl(sKBCatalogURL);
                    System.IO.File.WriteAllText(sKBCatalogFilePath, HttpUtility.HtmlDecode(driver.PageSource).Replace("\u00A0", " "));   //Hardcoded &nbsp;
                    //driver.FindElementByClassName("flatLightBlueButton").Click();
                }
                */

                //Console.WriteLine("DEBUG " + sKBCatalogURL);
                //Parse the Catalog Page File


                //TODO Review we could have multiple files/versions (i.e. LDR/GDR)
                Console.WriteLine("DEBUG Calling BluePill2 sFileNameToSearchReplaced="+ sFileNameToSearchReplaced);
                try
                {
                    sFileDateVersion = fBluePill(sKBNumber, sFileNameToSearchReplaced, sProductFound);
                    //20160910 win32k.sys 6.0.6002.19693
                }
                catch (Exception exCallBluePill2)
                {
                    Console.WriteLine("Exception: exCallBluePill2 " + exCallBluePill2.Message + " " + exCallBluePill2.InnerException);
                }
                if (sFileDateVersion != string.Empty)
                {
                    Console.WriteLine("DEBUG sFileDateVersionCatalog=" + sFileDateVersion);
                    //TODO Review
                    if (!dKBURLFile.ContainsKey(sKBURLFinal2)) dKBURLFile.Add(sKBURLFinal2, sFileDateVersion.ToLower());    //We keep track of the Files information found in a page (so we would not have to (re)analyze the same page twice)
                    if (!lFilesToUse.Contains(sFileNameToSearchReplaced.ToLower())) lFilesToUse.Add(sFileNameToSearchReplaced.ToLower());   //We keep track of the Files that we use so we can harmonize them in the final XML. e.g.: 20161005 10.0.14393.321 win32kbase.sys and 20161005 10.0.14393.321 win32kfull.sys (same date/version) if we already used win32kfull.sys we would give it preference (for cosmetic)

                    if (sProductFoundDEADBEEF == "") sProductFoundDEADBEEF = sProductFoundDEADBEEFGlobal;

                    if (!dProductFile.ContainsKey(sProductFoundDEADBEEF))
                    {
                        Console.WriteLine("DEBUG dProductFile.Add " + sProductFoundDEADBEEF);
                        dProductFile.Add(sProductFoundDEADBEEF, sFileDateVersion.ToLower());
                    }

                #region PATCHFILEinXORCISM
                try
                {
                    string[] sFileDateVersionSplit = sFileDateVersion.Split(' ');
                    int iFileID = 0;
                    try
                    {
                        iFileID = fXORCISMAddFILE(sFileDateVersionSplit[1], sFileDateVersionSplit[2], sFileDateVersionSplit[0]);
                    }
                    catch(Exception exFileNotFound)
                    {
                        Console.WriteLine("Exception: exFileNotFound " + exFileNotFound.Message + " " + exFileNotFound.InnerException);
                    }
                    /*
                    try
                    {
                        iFileID = model.FILE.Where(o => o.FileName == sFileDateVersionSplit[1]).FirstOrDefault().FileID;
                    }
                    catch(Exception ex)
                    {

                    }
                    if(iFileID<=0)
                    {
                        Console.WriteLine("DEBUG Adding FILE "+ sFileDateVersionSplit[1]);
                        try
                        {
                            FILE oFile = new FILE();
                            oFile.CreatedDate = DateTimeOffset.Now;
                            oFile.FileName = sFileDateVersionSplit[1];
                            //oFile.VocabularyID=   //Microsoft?
                            model.FILE.Add(oFile);
                            model.SaveChanges();
                            iFileID = oFile.FileID;
                        }
                        catch(Exception exAddFILE44)
                        {
                            Console.WriteLine("Exception: exAddFILE44 " + exAddFILE44.Message + " " + exAddFILE44.InnerException);
                        }

                    }
                    //TODO: FILEVERSION
                    */
                    if (iPatchID > 0 && iFileID > 0)
                    {
                        int iPlatformID = 0;
                        //int iOVALPlatformID = 0;
                        #region platformxorcism
                        if (sProductFound.Contains("windows"))
                        {
                            if (sProductFound.Contains("windows server 2016"))
                            {
                                iPlatformID = iPlatformWin2016;
                            }
                            else
                            {
                                if (sProductFound.Contains("windows server 2012 r2"))
                                {
                                    iPlatformID = iPlatformWin2012R2;
                                }
                                else
                                {
                                    if (sProductFound.Contains("windows server 2012"))
                                    {
                                        iPlatformID = iPlatformWin2012;
                                    }
                                    else
                                    {
                                        if (sProductFound.Contains("windows server 2008 r2"))
                                        {
                                            iPlatformID = iPlatformWin2008R2;
                                        }
                                        else
                                        {
                                            if (sProductFound.Contains("windows server 2008"))
                                            {
                                                iPlatformID = iPlatformWin2008;
                                            }
                                            else
                                            {
                                                if (sProductFound.Contains("windows server 2003 r2"))
                                                {
                                                    iPlatformID = iPlatformWin2003R2;
                                                }
                                                else
                                                {
                                                    if (sProductFound.Contains("windows server 2003"))
                                                    {
                                                        iPlatformID = iPlatformWin2003;
                                                    }
                                                    else
                                                    {
                                                        if (sProductFound.Contains("windows 10 version 1511"))  //Review
                                                        {
                                                            iPlatformID = iPlatformWin10v1511;
                                                        }
                                                        else
                                                        {
                                                            if (sProductFound.Contains("windows 10"))
                                                            {
                                                                iPlatformID = iPlatformWin10;
                                                            }
                                                            else
                                                            {
                                                                if (sProductFound.Contains("windows 8.1"))
                                                                {
                                                                    iPlatformID = iPlatformWin81;
                                                                }
                                                                else
                                                                {
                                                                    if (sProductFound.Contains("windows 8"))
                                                                    {
                                                                        iPlatformID = iPlatformWin8;
                                                                    }
                                                                    else
                                                                    {
                                                                        if (sProductFound.Contains("windows 7"))
                                                                        {
                                                                            iPlatformID = iPlatformWin7;
                                                                        }
                                                                        else
                                                                        {
                                                                            if (sProductFound.Contains("windows xp"))
                                                                            {
                                                                                iPlatformID = iPlatformWinXP;
                                                                            }
                                                                            else
                                                                            {
                                                                                if (sProductFound.Contains("windows vista"))
                                                                                {
                                                                                    iPlatformID = iPlatformWinVista;
                                                                                }
                                                                                else
                                                                                {
                                                                                    if (sProductFound.Contains("windows 2000"))
                                                                                    {
                                                                                        iPlatformID = iPlatformWin2000;
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        //ERROR?
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
                        #endregion platformxorcism

                        //TODO  PRODUCT
                        int iProductID = 0;


                        //int iPatchFileID = 0;
                        PATCHFILE oPatchFile = null;
                        try
                        {
                            //iPatchFileID = model.PATCHFILE.Where(o => o.PatchID == iPatchID && o.FileID == iFileID).FirstOrDefault().PatchFileID;
                            oPatchFile = model.PATCHFILE.Where(o => o.PatchID == iPatchID && o.FileID == iFileID).FirstOrDefault();
                        }
                        catch (Exception ex)
                        {

                        }
                        //if (iPatchFileID <= 0)
                        if(oPatchFile==null)
                        {
                            try
                            {
                                oPatchFile = new PATCHFILE();
                                oPatchFile.CreatedDate = DateTimeOffset.Now;
                                oPatchFile.PatchID = iPatchID;
                                oPatchFile.FileID = iFileID;
                                if (iPlatformID > 0) oPatchFile.PlatformID = iPlatformID;
                                if (iProductID > 0) oPatchFile.ProductID = iProductID;
                                //oPatchFile.VocabularyID=
                                model.PATCHFILE.Add(oPatchFile);
                                model.SaveChanges();
                            }
                            catch (Exception exAddPATCHFILE)
                            {
                                Console.WriteLine("Exception: exAddPATCHFILE44 iPatchID="+iPatchID+" iFileID="+iFileID+ " " + exAddPATCHFILE.Message + " " + exAddPATCHFILE.InnerException);
                            }
                        }
                        else
                        {
                            //Update PATCHFILE
                            try
                            {
                                if (iPlatformID > 0) oPatchFile.PlatformID = iPlatformID;
                                if (iProductID > 0) oPatchFile.ProductID = iProductID;
                                oPatchFile.timestamp = DateTimeOffset.Now;
                                model.Entry(oPatchFile).State = EntityState.Modified;
                                model.SaveChanges();
                            }
                            catch(Exception exUpdatePATCHFILE)
                            {
                                Console.WriteLine("Exception: exUpdatePATCHFILE " + exUpdatePATCHFILE.Message + " " + exUpdatePATCHFILE.InnerException);
                            }
                        }
                    }
                }
                catch(Exception exXORCISMPATCHFILE)
                {
                    Console.WriteLine("Exception: exXORCISMPATCHFILE " + exXORCISMPATCHFILE.Message + " " + exXORCISMPATCHFILE.InnerException);
                }
                #endregion PATCHFILEinXORCISM
            }
            //}
            //else//Apparently we already visited the KB page, downloaded and extracted the KBfile
            //{
            //jack
            //}

            if (sFileDateVersion == string.Empty)
            {
                Console.WriteLine("WARNING: No file found!!!");
                if (!dKBURLFile.ContainsKey(sKBURLFinal2))
                {
                    dKBURLFile.Add(sKBURLFinal2, "WARNING: No file found!");  //Hardcoded //So we don't parse it again
                }
            }

        }

        //TODO: Review the parameters!
        public static List<string> fGetExactProductName(string sHyperlinkText, string sProduct, string sProductToAdd, string sVulnDescription, List<string> lProductsMicrosoft, string sTheMainProductName)
        {
            Console.WriteLine("DEBUG fGetExactProductName");
            sHyperlinkText = sHyperlinkText.ToLower();
            Console.WriteLine("DEBUG sHyperlinkText=" + sHyperlinkText);

            //for internet explorer 9 for all supported 32-bit editions of windows vista
            Console.WriteLine("DEBUG sProduct=" + sProduct);

            //Method1
            //Try to clean the input to retrieve the product name(s)
            //Review WebUtility.HtmlDecode()
            string sHyperlinkTextCleaned = sHyperlinkText;
            //sHyperlinkTextCleaned = sHyperlinkTextCleaned.Replace("<td class=\"sbody-td\">pour le ", "");  //Hardcoded French
            //sHyperlinkTextCleaned = sHyperlinkTextCleaned.Replace("<td class=\"sbody-td\">pour ", "");  //Hardcoded French
            sHyperlinkTextCleaned = sHyperlinkTextCleaned.Replace("<td class=\"sbody-td\">for the ", "");   //HARDCODEDMSHTML
            sHyperlinkTextCleaned = sHyperlinkTextCleaned.Replace("<td class=\"sbody-td\">for ", "");
            sHyperlinkTextCleaned = sHyperlinkTextCleaned.Replace("<td class=\"sbody-td\">", "");
            if (sHyperlinkTextCleaned.Contains(":<br />"))
            {
                string[] HyperlinkTextCleaned = sHyperlinkTextCleaned.Split(new string[] { ":<br />" }, StringSplitOptions.None);    //Hardcoded
                sHyperlinkTextCleaned = HyperlinkTextCleaned[0];
            }
            sHyperlinkTextCleaned = sHyperlinkTextCleaned.Replace(":", "");
            if (sHyperlinkTextCleaned.Contains("<span"))
            {
                string[] HyperlinkTextCleaned = sHyperlinkTextCleaned.Split(new string[] { "<span" }, StringSplitOptions.None);    //Hardcoded
                sHyperlinkTextCleaned = HyperlinkTextCleaned[0];
            }
            
            sHyperlinkTextCleaned = sHyperlinkTextCleaned.Replace("<br />", "");


            //Remove everything in brackets/parenthesis
            sHyperlinkTextCleaned = Regex.Replace(sHyperlinkTextCleaned, @" ?\(.*?\)", string.Empty);
            

            //For JScript 5.8 and VBScript 5.8 on all supported Server Core installations of x64-based editions of Windows Server 2008 R2
            //sHyperlinkTextCleaned = sHyperlinkTextCleaned.Replace("(32-bit)", "");
            //sHyperlinkTextCleaned = sHyperlinkTextCleaned.Replace("(64-bit)", "");
            //sHyperlinkTextCleaned = sHyperlinkTextCleaned.Replace("(32-bit editions)", "");
            //sHyperlinkTextCleaned = sHyperlinkTextCleaned.Replace("(64-bit editions)", "");
            //sHyperlinkTextCleaned = sHyperlinkTextCleaned.Replace("(32-bit edition)", "");
            //sHyperlinkTextCleaned = sHyperlinkTextCleaned.Replace("(64-bit edition)", "");
            ////sHyperlinkTextCleaned = sHyperlinkTextCleaned.Replace("(editions 32 bits)", "");    //Hardcoded French
            //sHyperlinkTextCleaned = sHyperlinkTextCleaned.Replace("(editions 64 bits)", "");    //Hardcoded French
            //sHyperlinkTextCleaned = sHyperlinkTextCleaned.Replace("(edition 32 bits)", "");    //Hardcoded French
            //sHyperlinkTextCleaned = sHyperlinkTextCleaned.Replace("(edition 64 bits)", "");    //Hardcoded French
            //sHyperlinkTextCleaned = sHyperlinkTextCleaned.Replace("(toutes les versions)", ""); //Hardcoded French
            //sHyperlinkTextCleaned = sHyperlinkTextCleaned.Replace("(all versions)", "");
            //sHyperlinkTextCleaned = sHyperlinkTextCleaned.Replace("(all supported versions)", "");
            //editions
            //releases
            sHyperlinkTextCleaned = Regex.Replace(sHyperlinkTextCleaned, @"all supported (.*?)s of ", string.Empty);
            //on supported editions of
            sHyperlinkTextCleaned = Regex.Replace(sHyperlinkTextCleaned, @"supported (.*?)s of ", string.Empty);   //keep "on"  or  "in"
            sHyperlinkTextCleaned = sHyperlinkTextCleaned.Replace(" when it is installed", ""); //keep "on"
            //for all supported x86-based versions of windows 8
            //on all supported server core installations of x64-based editions of 
            sHyperlinkTextCleaned = sHyperlinkTextCleaned.Replace("x64-based editions of ", "");
            sHyperlinkTextCleaned = sHyperlinkTextCleaned.Replace(" only", "");

            //sHyperlinkTextCleaned = sHyperlinkTextCleaned.Replace("all supported itanium-based editions of ", "");
            //sHyperlinkTextCleaned = sHyperlinkTextCleaned.Replace("all supported x64-based editions of ", "");
            //sHyperlinkTextCleaned = sHyperlinkTextCleaned.Replace("all supported server core installations of x64-based editions of ", ""); //keep "on"
            //sHyperlinkTextCleaned = sHyperlinkTextCleaned.Replace("all supported 32-bit editions of ", "");
            //sHyperlinkTextCleaned = sHyperlinkTextCleaned.Replace("all supported 64-bit editions of ", "");
            //sHyperlinkTextCleaned = sHyperlinkTextCleaned.Replace("all supported editions of ", "");    //in
            ////sHyperlinkTextCleaned = sHyperlinkTextCleaned.Replace("sur les editions prises en charge de ", ""); //Hardcoded French

            //sHyperlinkTextCleaned = sHyperlinkTextCleaned.Replace("(user level install)","");
            //sHyperlinkTextCleaned = sHyperlinkTextCleaned.Replace("(admin level install)", "");
            sHyperlinkTextCleaned = sHyperlinkTextCleaned.Replace(" for 32-bit systems", "");
            sHyperlinkTextCleaned = sHyperlinkTextCleaned.Replace(" for x64-based systems", "");
            sHyperlinkTextCleaned = sHyperlinkTextCleaned.Replace("for ", "");

            //microsoft live meeting 2007, microsoft lync 2010, microsoft lync 2010 attendee, microsoft lync 2013 (skype for business), and microsoft lync basic 2013 (skype for business basic)
            //sHyperlinkTextCleaned = sHyperlinkTextCleaned.Replace(" (skype for business basic)","");
            //sHyperlinkTextCleaned = sHyperlinkTextCleaned.Replace(" (skype for business)", "");

            //microsoft exchange server 2013 cumulative update 8 and microsoft exchange server 2013 cumulative update 9
            //sHyperlinkTextCleaned = Regex.Replace(sHyperlinkTextCleaned, @"cumulative update \d", string.Empty);

            sHyperlinkTextCleaned = sHyperlinkTextCleaned.Replace(", and ", " and ");   //For below split

            //adobe flash player for windows 8.1 (kb3201860)
            /*
            try
            {
                Regex myRegexKBNumberInParenthesis = new Regex(@"(kb\d+)", RegexOptions.Singleline);
                string sKBNumberFound = myRegexKBNumberInParenthesis.Match(sHyperlinkTextCleaned).ToString();
                if(sKBNumberFound.Length>0) sHyperlinkTextCleaned = sHyperlinkTextCleaned.Replace(sKBNumberFound, "").Replace("()", "").Trim();
            }
            catch(Exception exRegexKBNumberInParenthesis)
            {
                Console.WriteLine("Exception: exRegexKBNumberInParenthesis " + exRegexKBNumberInParenthesis.Message + " " + exRegexKBNumberInParenthesis.InnerException);
            }
            //for microsoft live meeting 2007 console (3189647):
            try
            {
                Regex myRegexKBNumberInParenthesis2 = new Regex(@"(\d+)", RegexOptions.Singleline);
                string sKBNumberFound = myRegexKBNumberInParenthesis2.Match(sHyperlinkTextCleaned).ToString();
                if (sKBNumberFound.Length > 0) sHyperlinkTextCleaned = sHyperlinkTextCleaned.Replace(sKBNumberFound, "").Replace("()", "").Trim();
            }
            catch (Exception myRegexKBNumberInParenthesis2)
            {
                Console.WriteLine("Exception: myRegexKBNumberInParenthesis2 " + myRegexKBNumberInParenthesis2.Message + " " + myRegexKBNumberInParenthesis2.InnerException);
            }
            */
            ////word automation services sur les editions prises en charge de microsoft sharepoint server 2010 service pack 2


            sHyperlinkTextCleaned = sHyperlinkTextCleaned.Trim();
            Console.WriteLine("DEBUG sHyperlinkTextCleaned=" + sHyperlinkTextCleaned);  //internet explorer 9 for windows vista
            //microsoft .net framework 4.6/4.6.1 on windows server 2012 r2
            //internet explorer 11 in windows 8.1

            if (sHyperlinkTextCleaned.Contains(".net framework") && !lMainProductsNames.Contains(".net framework")) lMainProductsNames.Add(".net framework");
            //foreach(string sProductMicrosoftKnown in lProductsMicrosoft)
            //{
                //JJJ
                //TODO  lMainProductsNames.Add
            //}

            //If just one product:
            //Hardcoded thanks to MS... Special case
            if (sProduct == "windows server 2016" && sHyperlinkText.Contains("all supported x64-based editions of windows 10") && sHyperlinkTextCleaned == "windows 10")
            {
                //We keep windows server 2016 (x64)
            }
            else
            {
                //Check if VulnerabilityDescription contains just that (for more confidence of proper cleaning)
                if (sVulnDescription.ToLower().Contains(sHyperlinkTextCleaned)) sProduct = sHyperlinkTextCleaned;
                //Check if lProductsMicrosoft (from CPEs) contains just that (for more confidence of proper cleaning)
                //Warning: sharepoint enterprise server 2016            microsoft word 2016 vs word 2016
                if (lProductsMicrosoft.Contains(sHyperlinkTextCleaned)) sProduct = sHyperlinkTextCleaned;
                //TODO: ckeck if XORCISM.PRODUCT contains just that
            }

            //Method2 (not exhaustive, but just for more confidence)
            //HARDCODED
            //microsoft office 2016
            //For Microsoft Word 2016 (32-bit edition)
            //"office compatibility pack"
            if (sProduct.Contains("2007 microsoft office system")) sProduct = "microsoft office 2007";  //Thanks MS
            if (sProduct.Contains("2007 microsoft excel suite")) sProduct = "microsoft excel 2007";  //Thanks MS
            if (sProduct.Contains("2007 microsoft powerpoint suite")) sProduct = "microsoft powerpoint 2007";  //Thanks MS
            if (sProduct.Contains("2007 microsoft visio suite")) sProduct = "microsoft visio 2007";  //Thanks MS
            if (sProduct.Contains("2007 microsoft word suite")) sProduct = "microsoft word 2007";  //Thanks MS
            if (sProduct.Contains("2007 microsoft access suite")) sProduct = "microsoft access 2007";  //Thanks MS
            if (sProduct.Contains("2007 microsoft project suite")) sProduct = "microsoft project 2007";  //Thanks MS
            if (sProduct.Contains("2007 microsoft onenote suite")) sProduct = "microsoft onenote 2007";  //Thanks MS
            if (sProduct.Contains("2007 microsoft infopath suite")) sProduct = "microsoft infopath 2007";  //Thanks MS


            if (sProduct.Contains("office") && !sProduct.Contains("compatibility pack") && sHyperlinkText.Contains("office compatibility pack")) sProduct = sProduct.Replace("office", "office compatibility pack");   //Hardcoded French
            //if (sProduct.Contains("office") && sHyperlinkText.Contains("pack de compatibilite microsoft office")) sProduct = sProduct.Replace("office", "office compatibility pack");   //Hardcoded French
            if (sProduct.Contains("office") && sHyperlinkText.Contains("word viewer")) sProduct = sProduct.Replace("office", "word viewer");
            //word automation services

            if (sProduct.Contains("office") && sHyperlinkText.Contains(" word")) sProduct = sProduct.Replace("office", "word"); //i.e. not wordconv
            //if (sProduct.Contains("office") && sHyperlinkText.Contains("excel services")) sProduct = sProduct.Replace("office", "excel services");
            if (sProduct.Contains("office") && sHyperlinkText.Contains("excel services")) sProduct = sProduct.Replace("office", "sharepoint server");

            if (sProduct.Contains("office") && sHyperlinkText.Contains("excel viewer")) sProduct = sProduct.Replace("office", "excel viewer");
            if (sProduct.Contains("excel viewer suite")) sProduct = sProduct.Replace("excel viewer suite", "excel viewer");

            if (sProduct.Contains("office") && sHyperlinkText.Contains(" excel")) sProduct = sProduct.Replace("office", "excel");
            if (sProduct.Contains("office") && sHyperlinkText.Contains(" powerpoint viewer")) sProduct = sProduct.Replace("office", "powerpoint viewer");
            if (sProduct.Contains("office") && sHyperlinkText.Contains(" powerpoint")) sProduct = sProduct.Replace("office", "powerpoint");

            if (sProduct.Contains("office") && sHyperlinkText.Contains(" publisher")) sProduct = sProduct.Replace("office", "publisher");
            if (sProduct.Contains("office") && sHyperlinkText.Contains(" access")) sProduct = sProduct.Replace("office", "access");
            if (sProduct.Contains("office") && sHyperlinkText.Contains(" outlook")) sProduct = sProduct.Replace("office", "outlook");
            if (sProduct.Contains("office") && sHyperlinkText.Contains(" visio viewer")) sProduct = sProduct.Replace("office", "visio viewer");
            if (sProduct.Contains("office") && sHyperlinkText.Contains(" visio")) sProduct = sProduct.Replace("office", "visio");
            if (sProduct.Contains("office") && sHyperlinkText.Contains(" project")) sProduct = sProduct.Replace("office", "project");
            if (sProduct.Contains("office") && sHyperlinkText.Contains(" groove")) sProduct = sProduct.Replace("office", "groove");
            if (sProduct.Contains("office") && sHyperlinkText.Contains(" onenote")) sProduct = sProduct.Replace("office", "onenote");
            if (sProduct.Contains("office") && sHyperlinkText.Contains(" interconnect")) sProduct = sProduct.Replace("office", "interconnect");
            if (sProduct.Contains("office") && sHyperlinkText.Contains(" infopath")) sProduct = sProduct.Replace("office", "infopath");

            if (sProduct.Contains("office") && sHyperlinkText.Contains(" sharepoint server") && !sProduct.Contains("sharepoint server")) sProduct = sProduct.Replace("office", "sharepoint server");   //sharepoint enterprise server?
            if (sProduct.Contains("office") && sHyperlinkText.Contains(" sharepoint designer")) sProduct = sProduct.Replace("office", "sharepoint designer");

            if (sProduct.Contains("office compatibility pack")) sProduct=fRemoveYear(sProduct);
            
            //TODO?
            //sProduct = Regex.Replace(sProduct, @"for all supported (.*?)s of ", string.Empty);
            sProduct = Regex.Replace(sProduct, @"all supported (.*?)s of ", string.Empty);
            sProduct = sProduct.Replace(" for windows (all supported releases)", "");
            if (sProduct.StartsWith("for ")) sProduct = sProduct.Replace("for ", "");

            //jscript
            //vbscript
            //TODO: add more
            Console.WriteLine("DEBUG sProductReplaced=" + sProduct);

            List<string> lProductsFoundToAdd = new List<string>();

            //if (sHyperlinkText.Contains(" and ") || sHyperlinkText.Contains(" et ") || sHyperlinkText.Contains(" for ") || sHyperlinkText.Contains(" in "))    //Hardcoded+french
            if (sHyperlinkTextCleaned.Contains(" and ") || sHyperlinkTextCleaned.Contains(" for ") || sHyperlinkTextCleaned.Contains(" in ") || sHyperlinkTextCleaned.Contains(" when installed on ") || sHyperlinkTextCleaned.Contains(" on "))    //Hardcoded+french
            {
                #region multipleproductnamesinhyperlink
                //At least two products in there. boring...
                Console.WriteLine("DEBUG Multiple products in hyperlink/title");

                //1st Split: on "and"
                string[] HyperlinkTextSplitAnd=null;
                //TODO Review: skype for business 2016 and skype for business basic 2016
                if (sHyperlinkTextCleaned.Contains(" and "))
                {
                    HyperlinkTextSplitAnd = sHyperlinkTextCleaned.Split(new string[] { " and " }, StringSplitOptions.None);    //Hardcoded
                    //if (sHyperlinkTextCleaned.Contains(" et ")) HyperlinkTextSplitAnd = sHyperlinkTextCleaned.Split(new string[] { " et " }, StringSplitOptions.None);    //Hardcoded French
                }
                else//
                {
                    if (sHyperlinkTextCleaned.Contains(" for ")) HyperlinkTextSplitAnd = sHyperlinkTextCleaned.Split(new string[] { " for " }, StringSplitOptions.None);
                }
                if (sHyperlinkTextCleaned.Contains(" in ")) HyperlinkTextSplitAnd = sHyperlinkTextCleaned.Split(new string[] { " in " }, StringSplitOptions.None);//Hardcoded
                if (sHyperlinkTextCleaned.Contains(" when installed on "))
                {
                    HyperlinkTextSplitAnd = sHyperlinkTextCleaned.Split(new string[] { " when installed on " }, StringSplitOptions.None);//Hardcoded
                }
                else
                {
                    if (sHyperlinkTextCleaned.Contains(" on ")) HyperlinkTextSplitAnd = sHyperlinkTextCleaned.Split(new string[] { " on " }, StringSplitOptions.None);//Hardcoded
                }
                bool bHyperlinkTextContainssProduct = false;
                #region productafterand
                string sAfterAND = HyperlinkTextSplitAnd[1].Trim();
                //Windows Server 2016:<br /><span class="text-base">Windows10.0-KB3213986-x64.msu</span></td>
                //Cleaning  TODO Review WebUtility.HtmlDecode()
                if (sAfterAND.Contains("<span"))
                {
                    string[] AfterAndSplit = sAfterAND.Split(new string[] { "<span" }, StringSplitOptions.None);    //Hardcoded
                    try
                    {
                        sAfterAND = AfterAndSplit[0];
                    }
                    catch (Exception exAfterAndSplit)
                    {

                    }
                }
                sAfterAND = sAfterAND.Replace("<span", "");
                sAfterAND = sAfterAND.Replace("<br />", "");
                sAfterAND = sAfterAND.Replace(":", "").ToLower();
                Console.WriteLine("DEBUG sAfterAND cleaned=" + sAfterAND);

                Regex myRegexVersionString=new Regex(@"\d+(?:\.\d+)+");
                string sVersion = myRegexVersionString.Match(sAfterAND).ToString();
                //Console.WriteLine("DEBUG sProductX Version=" + sProductVersion);
                if (sVersion.Trim() == sAfterAND)
                {
                    //Just a version number after and
                    //TODO Improve
                    if(sHyperlinkTextCleaned.Contains(".net framework"))    //Hardcoded
                    {
                        sAfterAND = "microsoft .net framework " + sVersion;
                    }
                    else
                    {
                        Console.WriteLine("ERROR: sAfterANDVersion");
                    }

                }



                if (sAfterAND.Trim() == sProduct)    //product name found after the "and"   //TODO: not needed anymore
                {
                    //i.e.  windows server 2012 r2
                    //we will keep sProductToAdd=sProduct
                    //bHyperlinkTextContainssProduct = true;
                    sAfterAND = fAddx86x64(sAfterAND, sHyperlinkText);
                    lProductsFoundToAdd.Add(sAfterAND);
                    Console.WriteLine("DEBUG Product found after AND");
                }
                else
                {
                    if (sAfterAND.Contains(" on "))
                    {
                        //vbscript 5.8 on windows server 2008 r2    (was vbscript 5.8 on all supported server core installations of x64-based editions of windows server 2008 r2)
                        string[] AfterAndSplitOn = sAfterAND.Split(new string[] { " on " }, StringSplitOptions.None);    //Hardcoded
                        try
                        {
                            sAfterAND = AfterAndSplitOn[0];
                            //TODO? fGetProductToAdd
                            sAfterAND = fAddx86x64(sAfterAND, sHyperlinkText);
                            lProductsFoundToAdd.Add(sAfterAND);

                            //TODO?
                            sAfterAND = AfterAndSplitOn[1];
                            //TODO? fGetProductToAdd
                            sAfterAND = fAddx86x64(sAfterAND, sHyperlinkText);
                            lProductsFoundToAdd.Add(sAfterAND);

                        }
                        catch (Exception exAfterAndSplit)
                        {

                        }
                    }
                    else
                    {
                        //i.e   sProduct="windows server 2012"  HyperlinkTextSplit[1]="windows server 2012 r2"
                        //we will update sProductToAdd later? TODO Review
                        //sProductToAdd = fGetProductToAdd(sProduct, sProductToAdd, HyperlinkTextSplitAnd[1].Trim());
                        //Console.WriteLine("DEBUG TODO HyperlinkTextSplitAnd[1]=" + HyperlinkTextSplitAnd[1]);
                        //Console.WriteLine("DEBUG TODO sAfterAND=" + sAfterAND);

                    
                        //sProductToAdd = fGetProductToAdd(sProduct, sProductToAdd, sHyperlinkTextProduct.Trim());
                        //sAfterAND = fGetProductToAdd(sProduct, sProductToAdd, sAfterAND);
                        
                        sAfterAND = fAddx86x64(sAfterAND, sHyperlinkText);
                        if (!lOVALDefInventoryNotRetrieved.Contains(sAfterAND))
                        {
                            lProductsFoundToAdd.Add(sAfterAND);
                        }
                    }
                }
                #endregion productafterand

                if (!bHyperlinkTextContainssProduct)
                {
                    if (HyperlinkTextSplitAnd[0].Contains(",") || HyperlinkTextSplitAnd[0].Contains(" and ")) //Hardcoded (Note: we eliminated ", and") We test " and " in case it was " on " before
                    {
                        //windows 8.1, windows rt 8.1
                        Console.WriteLine("DEBUG ProductsInComma");
                        //2nd split on ,
                        //Note: no detection nor split on "/" here
                        //Microsoft .NET Framework versions 4.5, 4.5.1, and 4.5.2
                        #region productsincomma
                        //Multiple products before the "and"
                        string[] HyperlinkTextSplitComma = null;
                        if (HyperlinkTextSplitAnd[0].Contains(","))
                        {
                            HyperlinkTextSplitComma = HyperlinkTextSplitAnd[0].Split(','); //Hardcoded
                        }
                        else
                        {
                            HyperlinkTextSplitComma = HyperlinkTextSplitAnd[0].Split(new string[] { " and " }, StringSplitOptions.None);    //Hardcoded
                        }

                        int iCptProductVersionComma = 0;
                        string sFirstProductVersionComma = string.Empty;
                        foreach (string sHyperlinkTextProduct in HyperlinkTextSplitComma)
                        {
                            iCptProductVersionComma++;
                            //.NET Framework versions 4.5, 4.5.1, and 4.5.2
                            //if (sHyperlinkTextProduct.Contains(" versions")) sHyperlinkTextProduct = sHyperlinkTextProduct.Replace(" versions", "");

                            Console.WriteLine("DEBUG sProductInComma=" + sHyperlinkTextProduct);
                            if (iCptProductVersionComma == 1) sFirstProductVersionComma = sHyperlinkTextProduct.Replace(" versions", "");
                            //.NET Framework versions 4.5

                            if (sHyperlinkTextProduct.Trim() == sProduct)   //TODO: not needed anymore
                            {
                                //We keep sProductToAdd=sProduct
                                sProduct = fAddx86x64(sProduct, sHyperlinkText);
                                lProductsFoundToAdd.Add(sProduct);
                                //bHyperlinkTextContainssProduct = true;
                                //break;
                            }
                            else
                            {
                                if (sHyperlinkTextProduct.Contains(" and "))
                                {
                                    //4.5.1 and 4.5.2
                                    string[] HyperlinkTextSplitComma2 = null;
                                    HyperlinkTextSplitComma2 = sHyperlinkTextProduct.Split(new string[] { " and " }, StringSplitOptions.None);    //Hardcoded
                                    foreach (string sHyperlinkTextProduct2 in HyperlinkTextSplitComma2)
                                    {
                                        //sHyperlinkTextCleaned=microsoft .net framework versions 4.5, 4.5.1 and 4.5.2 on windows server 2012
                                        //sFirstProductVersionComma=.NET Framework 4.5
                                        string sNewProductVersion = sFirstProductVersionComma.Substring(0, sFirstProductVersionComma.LastIndexOf(' '));
                                        //TODO Review!
                                        Console.WriteLine("DEBUG sNewProductVersionConstructed=" + sNewProductVersion + " " + sHyperlinkTextProduct2);
                                        sProductToAdd = fGetProductToAdd(sNewProductVersion + " " + sHyperlinkTextProduct2, sHyperlinkTextProduct.Trim(), sHyperlinkTextProduct.Trim(), sTheMainProductName);
                                        sProductToAdd = fAddx86x64(sProductToAdd, sHyperlinkText);
                                        //sProductToAdd = fAddx86x64(sHyperlinkTextProduct.Trim(), sHyperlinkText);
                                        lProductsFoundToAdd.Add(sProductToAdd);

                                    }

                                }
                                else
                                {
                                    //***
                                    sProductToAdd = fGetProductToAdd(sHyperlinkTextProduct.Replace(" versions", "").Trim(), sHyperlinkTextProduct.Trim(), sHyperlinkTextProduct.Trim(), sTheMainProductName);
                                    sProductToAdd = fAddx86x64(sProductToAdd, sHyperlinkText);
                                    //sProductToAdd = fAddx86x64(sHyperlinkTextProduct.Trim(), sHyperlinkText);
                                    lProductsFoundToAdd.Add(sProductToAdd);
                                }
                            }
                        }
                        #endregion productsincomma
                    }
                    else
                    {
                        sProduct = HyperlinkTextSplitAnd[0].Trim();
                        Console.WriteLine("DEBUG Just one product before AND");
                        Console.WriteLine("DEBUG sProductX=" + sProduct);
                        //Console.WriteLine("DEBUG sProductToAddX=" + sProductToAdd);
                        //microsoft .net framework 4.6/4.6.1
                        //microsoft .net framework 4.5/4.5.1/4.5.2
                        if(sProduct.Contains("/"))//hardcoded
                        {
                            Console.WriteLine("DEBUG sProductX contains a /");
                            string[] ProductSplitSplash=sProduct.Split('/');
                            sProductToAdd = fGetProductToAdd(ProductSplitSplash[0], ProductSplitSplash[0], HyperlinkTextSplitAnd[0].Trim(), sTheMainProductName); //Review
                            sProductToAdd = fAddx86x64(sProductToAdd, sHyperlinkText);
                            lProductsFoundToAdd.Add(sProductToAdd);

                            //Regex myRegexVersionString=new Regex(@"\d+(?:\.\d+)+");
                            string sProductVersion=myRegexVersionString.Match(ProductSplitSplash[0]).ToString();
                            Console.WriteLine("DEBUG sProductX Version=" + sProductVersion);
                            if(sProductVersion.Trim()!="")
                            {
                                for (int iCptProductVersion = 1; iCptProductVersion < ProductSplitSplash.Length; iCptProductVersion++)
                                {
                                    string sProductVersion2 = myRegexVersionString.Match(ProductSplitSplash[iCptProductVersion]).ToString();
                                    if (sProductVersion2.Trim() != "")
                                    {
                                        Console.WriteLine("DEBUG sProductX OtherVersion=" + sProductVersion);
                                        
                                        //if (sProductVersion2.Length == ProductSplitSplash[iCptProductVersion].Length) //we just have a version number right the /
                                        //{
                                            //Use Version2 for replacing  Version1 and adding the Product2
                                            Console.WriteLine("DEBUG sProductX" + iCptProductVersion + "=" + ProductSplitSplash[0].Replace(sProductVersion, sProductVersion2));
                                            sProductToAdd = fGetProductToAdd(ProductSplitSplash[0].Replace(sProductVersion, sProductVersion2), ProductSplitSplash[0].Replace(sProductVersion, sProductVersion2), HyperlinkTextSplitAnd[0].Trim(), sTheMainProductName); //Review
                                            sProductToAdd = fAddx86x64(sProductToAdd, sHyperlinkText);
                                            lProductsFoundToAdd.Add(sProductToAdd);
                                        /*
                                        }
                                        else
                                        {
                                            //We have more than just a version number right the /
                                            Console.WriteLine("ERROR: ERROR45 Not Managed: " + ProductSplitSplash[iCptProductVersion]);
                                        }
                                        */
                                    }
                                }
                            }
                            else
                            {
                                //We found no version number left the /
                                 Console.WriteLine("ERROR: ERROR40 Not Managed: "+ProductSplitSplash[0]);
                            }
                        }
                        else
                        {
                            //No /
                            sProductToAdd = fGetProductToAdd(sProduct, sProduct, HyperlinkTextSplitAnd[0].Trim(), sTheMainProductName); //Review
                            sProductToAdd = fAddx86x64(sProductToAdd, sHyperlinkText);
                            lProductsFoundToAdd.Add(sProductToAdd);
                        }
                    }
                }

                if (bHyperlinkTextContainssProduct)
                {
                    //We keep sProductToAdd=sProduct
                    sProductToAdd = sProduct;
                    sProductToAdd = fAddx86x64(sProductToAdd, sHyperlinkText);
                    lProductsFoundToAdd.Add(sProductToAdd);
                }
                else
                {
                    //***
                    //sProductToAdd has been updated with ONE (only the last one) new better product match
                    //Console.WriteLine("DEBUG sProductToAdd has been updated with ONE (only the last one) new better product match");
                }
                #endregion multipleproductnamesinhyperlink
            }
            else
            {
                //Just one product in there. Thx God
                Console.WriteLine("DEBUG Just one Product");
                //HARCODED AGAIN
                //Enhance sProduct in sProductToAdd for better match    //Note: use Replace() so we keep the potential Service Pack/SP
                sProductToAdd = fGetProductToAdd(sProduct, sProduct, sHyperlinkText, sTheMainProductName);
                sProductToAdd = fAddx86x64(sProductToAdd, sHyperlinkText);
                lProductsFoundToAdd.Add(sProductToAdd);
            }

            Console.WriteLine("DEBUG sProductToAdd2=" + sProductToAdd);
            //return sProductToAdd;
            return lProductsFoundToAdd;
        }


        public static string fAddx86x64(string sProductName, string sText)
        {
            if (!sProductName.EndsWith("2016")) //TODO Review   Windows Server 2016 vs Office 2016
            {
                //amd64
                if ((sText.Contains("x64") || sText.Contains("64 bit")) && !sProductName.Contains("x64"))   //Hardcoded
                {
                    sProductName = sProductName + " x64";
                }
                else
                {
                    if ((sText.Contains("32-bit") || sText.Contains("32 bit")) && !sProductName.Contains("x86"))    //Hardcoded
                    {
                        sProductName = sProductName + " x86";
                    }
                }
            }
            return sProductName;
        }


        public static string fGetProductToAdd(string sProduct, string sProductToAdd, string sHyperlinkText, string sMyMainProductName="")
        {
            //HARCODED AGAIN
            //Enhance sProduct in sProductToAdd for better match    //Note: use Replace() so we keep the potential Service Pack/SP
            switch (sProduct)
            {
                //windows 8.1
                //windows rt 8.1
                case "windows 10":
                    //RTM
                    if (sHyperlinkText.Contains("windows 10 version 1511")) sProductToAdd = sProduct.Replace("windows 10", "windows 10 version 1511");
                    if (sHyperlinkText.Contains("windows 10 version 1607")) sProductToAdd = sProduct.Replace("windows 10", "windows 10 version 1607");
                    if (sHyperlinkText.Contains("windows 10 version 1703")) sProductToAdd = sProduct.Replace("windows 10", "windows 10 version 1703");

                    break;
                case "windows server 2008":
                    if (sHyperlinkText.Contains("windows server 2008 r2"))
                    {
                        sProductToAdd = sProduct.Replace("windows server 2008", "windows server 2008 r2");
                        if (sHyperlinkText.Contains("itanium-based edition")) sProductToAdd = sProduct.Replace("windows server 2008 r2", "windows server 2008 r2 itanium-based edition");
                    }
                    else
                    {
                        if (sHyperlinkText.Contains("itanium-based edition")) sProductToAdd = sProduct.Replace("windows server 2008", "windows server 2008 itanium-based edition");
                    }
                    //Microsoft Windows Server 2008 R2 Itanium-Based Edition is installed
                    break;
                case "windows server 2008 r2":
                    if (sHyperlinkText.Contains("itanium-based edition")) sProductToAdd = sProduct.Replace("windows server 2008 r2", "windows server 2008 r2 itanium-based edition");
                    //Microsoft Windows Server 2008 R2 Itanium-Based Edition is installed
                    break;
                case "windows server 2012":
                    if (sHyperlinkText.Contains("windows server 2012 r2")) sProductToAdd = sProduct.Replace("windows server 2012", "windows server 2012 r2");

                    break;
                case "windows server 2016":
                    //one day maybe
                    if (sHyperlinkText.Contains("windows server 2016 r2")) sProductToAdd = sProduct.Replace("windows server 2016", "windows server 2016 r2");
                    break;

                //TODO add more
                //for mac
                //office web apps
                //
                default:
                    //Do nothing for now
                    break;
            }

            if (sHyperlinkText.EndsWith(" rt") && !sProductToAdd.EndsWith(" rt")) sProductToAdd = sProductToAdd + " rt";    //Hardcoded
            sProductToAdd = sProductToAdd.Replace(" enterprise", "");   //HARDCODED

            //Service Packs added to the end of sProductToAdd
            if (sHyperlinkText.Contains(" sp1") && !sProductToAdd.Contains("sp1") && !sProductToAdd.Contains("service pack 1")) sProductToAdd += " sp1";
            if (sHyperlinkText.Contains(" sp2") && !sProductToAdd.Contains("sp2") && !sProductToAdd.Contains("service pack 2")) sProductToAdd += " sp2";
            if (sHyperlinkText.Contains(" sp3") && !sProductToAdd.Contains("sp3") && !sProductToAdd.Contains("service pack 3")) sProductToAdd += " sp3";
            if (sHyperlinkText.Contains(" sp4") && !sProductToAdd.Contains("sp4") && !sProductToAdd.Contains("service pack 4")) sProductToAdd += " sp4";
            if (sHyperlinkText.Contains(" sp5") && !sProductToAdd.Contains("sp5") && !sProductToAdd.Contains("service pack 5")) sProductToAdd += " sp5";
            if (sHyperlinkText.Contains(" sp6") && !sProductToAdd.Contains("sp6") && !sProductToAdd.Contains("service pack 6")) sProductToAdd += " sp6";
            if (sHyperlinkText.Contains("service pack 1") && !sProductToAdd.Contains("sp1") && !sProductToAdd.Contains("service pack 1")) sProductToAdd += " sp1";
            if (sHyperlinkText.Contains("service pack 2") && !sProductToAdd.Contains("sp2") && !sProductToAdd.Contains("service pack 2")) sProductToAdd += " sp2";
            if (sHyperlinkText.Contains("service pack 3") && !sProductToAdd.Contains("sp3") && !sProductToAdd.Contains("service pack 3")) sProductToAdd += " sp3";
            if (sHyperlinkText.Contains("service pack 4") && !sProductToAdd.Contains("sp4") && !sProductToAdd.Contains("service pack 4")) sProductToAdd += " sp4";
            if (sHyperlinkText.Contains("service pack 5") && !sProductToAdd.Contains("sp5") && !sProductToAdd.Contains("service pack 5")) sProductToAdd += " sp5";
            if (sHyperlinkText.Contains("service pack 6") && !sProductToAdd.Contains("sp6") && !sProductToAdd.Contains("service pack 6")) sProductToAdd += " sp6";

            Console.WriteLine("DEBUG fGetProductToAdd() sProductToAddJA=" + sProductToAdd);
            if (!lOVALDefInventoryNotRetrieved.Contains(sProductToAdd))
            {
                if (sMyMainProductName != "")
                {
                    if (sProductToAdd.Contains(sMyMainProductName))  //i.e. internet explorer 9 contains internet explorer
                    {
                        if (!lMainProductsNames.Contains(sProductToAdd.ToLower()))
                        {
                            Console.WriteLine("DEBUG fGetProductToAdd() Adding sProductToAdd to lMainProductsNames");
                            lMainProductsNames.Add(sProductToAdd.ToLower());
                        }
                    }
                }
            }
            return sProductToAdd;
        }


        public static string fGetPlatformName(string sProductName, bool bKeepSP=false)
        {
            //HARDCODED for your pleasure...
            sProductName = sProductName.Replace(" Version 1511", "");
            sProductName = sProductName.Replace(" version 1511", "");
            sProductName = sProductName.Replace("(version 1511)", "");
            sProductName = sProductName.Replace(" Version 1607", "");
            sProductName = sProductName.Replace(" version 1607", "");
            sProductName = sProductName.Replace("(version 1607)", "");
            sProductName = sProductName.Replace(" Version 1703", "");
            sProductName = sProductName.Replace(" version 1703", "");
            sProductName = sProductName.Replace("(version 1703)", "");


            sProductName = sProductName.Replace(" x86", "");
            sProductName = sProductName.Replace(" x64", "");
            sProductName = sProductName.Replace(" (x86)", "");
            sProductName = sProductName.Replace(" (x64)", "");
            sProductName = sProductName.Replace(" (32-Bit)", "");
            sProductName = sProductName.Replace(" (64-Bit)", "");
            sProductName = sProductName.Replace(" (32-bit)", "");
            sProductName = sProductName.Replace(" (64-bit)", "");
            sProductName = sProductName.Replace(" Itanium-Based Edition", "");
            sProductName = sProductName.Replace(" itanium-based edition", "");
            sProductName = sProductName.Replace(" Itanium","");
            sProductName = sProductName.Replace(" (Ia-64)", "");
            sProductName = sProductName.Replace(" ia64", "");
            sProductName = sProductName.Replace(" RTM", "");
            sProductName = sProductName.Replace(" rtm", "");
            sProductName = sProductName.Replace(" RT", "");
            sProductName = sProductName.Replace(" rt", "");
            //R2?
            //...

            for (int iSP = 1; iSP < 6; iSP++)   //Hardcoded
            {
                //sOVALDefInventoryTitleModified = sOVALDefInventoryTitleModified.Replace(" sp" + iYear.ToString(), "");
                if (bKeepSP)
                {
                    sProductName = sProductName.Replace(" Service Pack" + iSP.ToString(), " SP" + iSP.ToString());
                }
                else
                {
                    sProductName = sProductName.Replace(" SP" + iSP.ToString(), "");
                    sProductName = sProductName.Replace(" Service Pack" + iSP.ToString(), "");
                }
            }
            return sProductName;
        }


        public static string fRemoveYear(string sProductToAdd)
        {
            //TODO: Replace with a cleaner Regex
            //Used with Excel Viewer, Word Viewer, Microsoft Office Compatibility Pack...
            for (int iYear = 2000; iYear <= DateTime.Now.Year; iYear++)
            {
                sProductToAdd = sProductToAdd.Replace(" "+iYear.ToString(), "");
            }
            return sProductToAdd;
        }


        public static string fHardcodeVulnerableFilename(string sTextInput)
        {
            //string sFileNameFound = "";
            string sFileNameToSearch = "";

            if (sFileNameToSearch == "" && sTextInput.Contains("edge")) sFileNameToSearch = "edgehtml.dll"; // "mshtml.dll";   ////urlmon.dll   //HARDCODED
            if (sFileNameToSearch == "" && sTextInput.Contains("internet explorer")) sFileNameToSearch = "mshtml.dll";   //HARDCODED
            if (sFileNameToSearch == "" && sTextInput.Contains("flash player")) sFileNameToSearch = "flash.ocx";   //HARDCODED
            //Flashplayerapp.exe
            //Flashutil_activex.dll
            //Flashutil_activex.exe
            if (sFileNameToSearch == "" && sTextInput.Contains("jscript")) sFileNameToSearch = "jscript.dll";   //HARDCODED
            if (sFileNameToSearch == "" && sTextInput.Contains("vbscript")) sFileNameToSearch = "vbscript.dll";   //HARDCODED

            //if (sFileNameToSearch == "" && sTextInput.Contains("Graphics Component"))   //TODO

            if(sFileNameToSearch!="") Console.WriteLine("DEBUG sFileNameToSearchHardcoded=" + sFileNameToSearch);
            //return sFileNameFound;
            return sFileNameToSearch;
        }


        public static List<string> fGetFilenamesToSearchForProduct(string sProductNameInput, int iProductIDInput = 0)
        {
            //string sFilenameToSearch = "";
            List<string> lProductFiles = new List<string>();
            sProductNameInput = sProductNameInput.Replace("x86", "");   //Hardcoded
            sProductNameInput = sProductNameInput.Replace("x64", "");   //Hardcoded
            sProductNameInput = sProductNameInput.Replace("ia64", "");   //Hardcoded
            sProductNameInput = sProductNameInput.Trim();

            //This function looks into XORCISM for retrieving a Filename for a Product
            //This could be based on a Filename used in the past in an OVAL Definition for the Product
            //Note: Windows is a Platform, not a Product
            if (sProductNameInput.StartsWith("windows") && !sProductNameInput.Contains("kernel"))    //HARDCODED
            {
                //TODO? Search PLATFORMFILE

            }
            else
            {
                //HARDCODED
                if (sProductNameInput == "microsoft windows hyperlink object library") sProductNameInput = "Hyperlink Object Library";
                //microsoft word viewer 2007
                if (sProductNameInput.Contains("microsoft word viewer")) sProductNameInput="microsoft word viewer";
                sProductNameInput = sProductNameInput.Replace("skype business", "skype for business");

                int iProductID = 0;
                if (iProductIDInput == 0)
                {
                    try
                    {
                        //iProductID = oval_model.PRODUCT.Where(o => o.ProductName == sProductNameInput).FirstOrDefault().ProductID;
                        iProductID = model.PRODUCT.Where(o => o.ProductName == sProductNameInput || o.ProductName == "Microsoft "+sProductNameInput).FirstOrDefault().ProductID;
                    }
                    catch (Exception ex)
                    {

                    }
                    if (iProductID <= 0)//Try2
                    {
                        if (sProductNameInput.Contains(" sp") || sProductNameInput.Contains(" service pack "))
                        {
                            if (sProductNameInput.Contains(" sp"))
                            {
                                sProductNameInput = sProductNameInput.Replace(" sp", " service pack ");
                            }
                            else
                            {
                                if (sProductNameInput.Contains(" service pack ")) sProductNameInput = sProductNameInput.Replace(" service pack ", " sp");
                            }
                            try
                            {
                                iProductID = model.PRODUCT.Where(o => o.ProductName == sProductNameInput || o.ProductName == "Microsoft " + sProductNameInput).FirstOrDefault().ProductID;
                            }
                            catch (Exception ex)
                            {

                            }
                            if (iProductID <= 0)//Try3
                            {
                                if (sProductNameInput.Contains(" sp1")) sProductNameInput = sProductNameInput.Replace(" sp1", "");
                                if (sProductNameInput.Contains(" sp2")) sProductNameInput = sProductNameInput.Replace(" sp2", "");
                                if (sProductNameInput.Contains(" sp3")) sProductNameInput = sProductNameInput.Replace(" sp3", "");
                                if (sProductNameInput.Contains(" sp4")) sProductNameInput = sProductNameInput.Replace(" sp4", "");
                                if (sProductNameInput.Contains(" sp5")) sProductNameInput = sProductNameInput.Replace(" sp5", "");
                                if (sProductNameInput.Contains(" sp6")) sProductNameInput = sProductNameInput.Replace(" sp6", "");
                                if (sProductNameInput.Contains(" service pack 1")) sProductNameInput = sProductNameInput.Replace(" service pack 1", "");
                                if (sProductNameInput.Contains(" service pack 2")) sProductNameInput = sProductNameInput.Replace(" service pack 2", "");
                                if (sProductNameInput.Contains(" service pack 3")) sProductNameInput = sProductNameInput.Replace(" service pack 3", "");
                                if (sProductNameInput.Contains(" service pack 4")) sProductNameInput = sProductNameInput.Replace(" service pack 4", "");
                                if (sProductNameInput.Contains(" service pack 5")) sProductNameInput = sProductNameInput.Replace(" service pack 5", "");
                                if (sProductNameInput.Contains(" service pack 6")) sProductNameInput = sProductNameInput.Replace(" service pack 6", "");
                                try
                                {
                                    iProductID = model.PRODUCT.Where(o => o.ProductName == sProductNameInput || o.ProductName == "Microsoft " + sProductNameInput).FirstOrDefault().ProductID;
                                }
                                catch (Exception ex)
                                {

                                }

                            }

                        }
                    }
                }
                else
                {
                    iProductID = iProductIDInput;
                }

                if (iProductID <= 0)
                {
                    Console.WriteLine("WARNING: Unknown Product in XORCISM: " + sProductNameInput + " ID:" + iProductIDInput);
                }
                else
                {
                    /*
                    var OVALDefinitionsForProduct = oval_model.OVALDEFINITIONPRODUCT.Where(o => o.ProductID == iProductID);
                    foreach (OVALDEFINITIONPRODUCT oOVALDefinitionForProduct in OVALDefinitionsForProduct)
                    {

                    }
                    */

                    var ProductFiles = model.PRODUCTFILE.Where(o => o.ProductID == iProductID);
                    foreach (PRODUCTFILE oProductFile in ProductFiles)
                    {
                        if (!lProductFiles.Contains(oProductFile.FILE.FileName.ToLower())) lProductFiles.Add(oProductFile.FILE.FileName.ToLower());
                    }
                }
            }

            //return sFilenameToSearch;
            return lProductFiles;
        }


        public static void fParseCSV(string sRegexDownloadCSVlink, string sFileNameToSearchReplaced)
        {
            List<string> lFilesForProduct = new List<string>();
            string ResponseText = string.Empty;
            int iCptColumns = 0;
            int iCptColumnFileIdentifier = 0;   //Used when "File name" is empty...
            int iCptColumnFileName = 0;
            int iCptColumnFileVersion = 0;
            int iCptColumnDate = 0;
            int iCptColumnPlatform = 0;

            #region parsecsv
            string[] sKBCSVFileNameSplit = sRegexDownloadCSVlink.ToString().Split('/');
            string sKBCSVFileName = sKBCSVFileNameSplit[sKBCSVFileNameSplit.Count() - 1];
            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
            Console.WriteLine("DEBUG sKBCSVFileName: " + sKBCSVFileName);

            #region downloadcsv
            //string ResponseTextKBCSV = "";
            //string MyCookie = "";
            //sCurrentPath = Directory.GetCurrentDirectory();
            string sKBCSVPath = sCurrentPath + @"\MS\" + sKBCSVFileName;    //HARDCODED
                                                                            //FileInfo fileInfo = new FileInfo(sKBCSVPath);
            FileInfo fileInfo = new FileInfo(sKBCSVPath);
            try
            {
                if (!fileInfo.Exists)
                {
                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                    Console.WriteLine("DEBUG Request to " + sRegexDownloadCSVlink);

                    StreamReader SR = null;
                    HttpWebResponse response = null;

                    HttpWebRequest request;
                    request = (HttpWebRequest)HttpWebRequest.Create("http://" + sRegexDownloadCSVlink);
                    request.Method = "GET";

                    response = (HttpWebResponse)request.GetResponse();
                    SR = new StreamReader(response.GetResponseStream());
                    ResponseText = SR.ReadToEnd();

                    SR.Close();
                    response.Close();

                    System.IO.File.WriteAllText(sKBCSVPath, ResponseText);   ////Hardcoded
                }
            }
            catch (Exception exFileInfoKBCSV)
            {
                Console.WriteLine("Exception: exFileInfoKBCSV " + exFileInfoKBCSV.Message + " " + exFileInfoKBCSV.InnerException);
            }
            #endregion downloadcsv

            //Parse the KBCSV file
            //ResponseTextKBCSV = System.IO.File.ReadAllText(sKBCSVPath);   ////Hardcoded
            /*
            x86 Windows 10
            File name,File version,File size,Date,Time,Platform,
            "Microsoft.dll","10.0.14393.576","21,504","09-Dec-2016","09:54","x86",
            */
            fileInfo = new FileInfo(sKBCSVPath);
            try
            {
                if (fileInfo.Exists)
                {
                    #region parsecsv
                    StreamReader srStreamReader = new StreamReader(sKBCSVPath);
                    string sCSVline = srStreamReader.ReadLine();
                    sFileNameToSearchReplaced = sFileNameToSearchReplaced.ToLower();
                    try
                    {
                        bool bStop = false;
                        while (sCSVline != null && !bStop)
                        {
                            //NOTE: TODO? for now we don't deal with the product       x86 Windows 10  x64 Windows 10   (could be for server 2016 too)      For all supported ia64-based versions
                            if (sCSVline.Contains(","))
                            {
                                string[] CSVlineSplit = sCSVline.Split(new string[] { "\"," }, StringSplitOptions.None);
                                //Regex.Split(input, @"\",");
                                if (sCSVline.Contains("Additional file information"))
                                {
                                    bStop = true;
                                }
                                else
                                {
                                    if (sCSVline.Contains("File name") && sCSVline.Contains("File version") && !sCSVline.Contains("hash")) //HardcodedMSCSV
                                    {
                                        /*
                                        //for now we don't deal with the product       x86 Windows 10  x64 Windows 10
                                        if (iCptColumnFileName != 0 && iCptColumnFileVersion != 0)
                                        {
                                            bStop = true;
                                            break;
                                        }
                                        */

                                        //Console.WriteLine("DEBUG CSVColumnsNamesLine=" + sCSVline);
                                        CSVlineSplit = sCSVline.Split(',');
                                        //Column names
                                        #region getcolumnsindex

                                        for (iCptColumns = 0; iCptColumns < CSVlineSplit.Count(); iCptColumns++)
                                        {
                                            ////File identifier
                                            if (CSVlineSplit[iCptColumns].Replace("\"", "") == "File identifier") iCptColumnFileIdentifier = iCptColumns;
                                            //File name
                                            if (CSVlineSplit[iCptColumns].Replace("\"", "") == "File name") iCptColumnFileName = iCptColumns;
                                            //File version
                                            if (CSVlineSplit[iCptColumns].Replace("\"", "") == "File version") iCptColumnFileVersion = iCptColumns;
                                            //File size
                                            //Date
                                            if (CSVlineSplit[iCptColumns].Replace("\"", "") == "Date") iCptColumnDate = iCptColumns;
                                            //Time
                                            //Platform
                                            if (CSVlineSplit[iCptColumns].Replace("\"", "") == "Platform") iCptColumnPlatform = iCptColumns;
                                            ////SP requirement
                                            ////Service branch
                                        }
                                        //Console.WriteLine("DEBUG iCptColumnFileName=" + iCptColumnFileName);
                                        //Console.WriteLine("DEBUG iCptColumnFileVersion=" + iCptColumnFileVersion);
                                        //Console.WriteLine("DEBUG iCptColumnDate=" + iCptColumnDate);
                                        #endregion getcolumnsindex
                                    }
                                    else
                                    {
                                        //Normal line
                                        bool bShittyCSV = false;

                                        #region normalcsvline
                                        if (sCSVline.Contains(".dll") || sCSVline.Contains(".exe") || sCSVline.Contains(".sys") || sCSVline.Contains(".ocx") || sCSVline.Contains(".aspx") || sCSVline.Contains(".flt")) //HARDCODED
                                        {
                                            /*
                                            if (!sCSVline.Contains("\","))
                                            {
                                                bShittyCSV = true;
                                                //Console.WriteLine("DEBUG Shitty CSV!");
                                                ////Example:
                                                ////File name,File version,File size,Date,Time,Platform,SP requirement,Service branch
                                                ////Adsmsext.dll,6.1.7601.23545,226304,42625,0.864583333,IA-64,None,Not applicable
                                                ////    => Enjoy the , in the file size!    Thanks Micro....
                                                if (bShittyCSV) CSVlineSplit = sCSVline.Split(',');
                                            }

                                            //    Console.WriteLine("DEBUG File Name=" + sKBFileInfoValue);
                                            //We don't want .png    idx_dll manifest...
                                            string sKBFileInfoValue = CSVlineSplit[iCptColumnFileName].Replace("\"", "").ToLower();
                                            if(sKBFileInfoValue.Contains(","))
                                            {
                                                //Fu**ing Shi**ty CSV
                                                //Mscormmc.dll,2.0.50727.8671,"94,360",21-Oct-15,1:54,x86,None,Not applicable
                                                CSVlineSplit = sCSVline.Split(',');
                                                sKBFileInfoValue = CSVlineSplit[iCptColumnFileName].Replace("\"", "").ToLower();
                                                //TODO What Date will we get?

                                            }
                                            */

                                            CSVlineSplit = fCSVParse(sCSVline).ToArray();
                                            string sKBFileInfoValue = CSVlineSplit[iCptColumnFileName]; //ToLower?



                                            //Console.WriteLine("DEBUG sKBFileInfoValue=" + sKBFileInfoValue);
                                            bool bInterestingFile = false;
                                            if (sKBFileInfoValue.Trim() == "") sKBFileInfoValue = CSVlineSplit[iCptColumnFileIdentifier].Replace("\"", "").ToLower();
                                            if (sKBFileInfoValue.EndsWith(".dll") || sKBFileInfoValue.EndsWith(".exe") || sKBFileInfoValue.EndsWith(".sys") || sKBFileInfoValue.EndsWith(".ocx") || sKBFileInfoValue.EndsWith(".aspx") || sKBFileInfoValue.EndsWith(".flt")) //HARDCODED .config?
                                            {
                                                //BAD
                                                //bInterestingFile = true;
                                                //lFilenames.Add(sKBFileInfoValue);


                                                if (sFileNameToSearchReplaced == "" || sFileNameToSearchReplaced == string.Empty)
                                                {
                                                    if (lFilesForProduct.Count == 0 && lFileNamesToSearch.Count == 0)
                                                    {
                                                        //We take any/all the files
                                                        bInterestingFile = true;
                                                        if (sKBFileInfoValue.Contains("|")) Console.WriteLine("WARNING: PipeInsKBFileInfoValue");
                                                        lFilenames.Add(sKBFileInfoValue);
                                                    }
                                                    else
                                                    {
                                                        if ((lFilesForProduct.Contains(sKBFileInfoValue) || lFileNamesToSearch.Contains(sKBFileInfoValue)) && !lFileNamesNOTToSearch.Contains(sKBFileInfoValue))
                                                        {
                                                            Console.WriteLine("DEBUG sKBFileInfoValue found in lFilesForProduct1 " + sKBFileInfoValue);
                                                            bInterestingFile = true;
                                                            lFilenames.Add(sKBFileInfoValue);
                                                        }
                                                        else
                                                        {
                                                            if (lFilesToUse.Contains(sKBFileInfoValue.ToLower()) && !lFileNamesNOTToSearch.Contains(sKBFileInfoValue))
                                                            {
                                                                Console.WriteLine("DEBUG sKBFileInfoValue found in lFilesToUse1 " + sKBFileInfoValue);
                                                                bInterestingFile = true;
                                                                lFilenames.Add(sKBFileInfoValue);
                                                            }
                                                            else
                                                            {
                                                                //Not an interesting file
                                                                ////Add it anyway because we don't want 0
                                                                //bInterestingFile = true;
                                                                //lFilenames.Add(sKBFileInfoValue);
                                                            }
                                                        }
                                                    }
                                                }
                                                else//sFileNameToSearchReplaced has a value
                                                {
                                                    if ((sFileNameToSearchReplaced == sKBFileInfoValue || lFileNamesToSearch.Contains(sKBFileInfoValue)) && !lFileNamesNOTToSearch.Contains(sKBFileInfoValue))
                                                    {
                                                        //We found (one of) our file
                                                        bInterestingFile = true;
                                                        lFilenames.Add(sKBFileInfoValue);

                                                    }
                                                    else
                                                    {
                                                        //excelcnv.exe|excelconv.exe
                                                        if (sKBFileInfoValue.Contains("|"))// && sKBFileInfoValue.Contains(sFileNameToSearchReplaced))
                                                        {
                                                            if (sKBFileInfoValue.Contains(sFileNameToSearchReplaced) && !lFileNamesNOTToSearch.Contains(sFileNameToSearchReplaced))
                                                            {
                                                                //We found our file
                                                                bInterestingFile = true;
                                                                lFilenames.Add(sFileNameToSearchReplaced);
                                                            }
                                                            else
                                                            {
                                                                foreach (string sFileNameToSearchFromList in lFileNamesToSearch)
                                                                {
                                                                    if (sKBFileInfoValue.Contains(sFileNameToSearchFromList) && !lFileNamesNOTToSearch.Contains(sFileNameToSearchFromList))
                                                                    {
                                                                        bInterestingFile = true;
                                                                        lFilenames.Add(sFileNameToSearchReplaced);
                                                                        break;  //we take just 1 match
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        else//No | in sKBFileInfoValue
                                                        {
                                                            if (lFileNamesToSearch.Contains(sKBFileInfoValue) && !lFileNamesNOTToSearch.Contains(sKBFileInfoValue))
                                                            {
                                                                if(bDebugFileSelection) Console.WriteLine("DEBUG sKBFileInfoValue found in lFileNamesToSearch2 " + sKBFileInfoValue);
                                                                bInterestingFile = true;
                                                                lFilenames.Add(sKBFileInfoValue);
                                                            }
                                                            else
                                                            {
                                                                //TODO Review if we want this   //xaz
                                                                if (lFilesForProduct.Contains(sKBFileInfoValue) && !lFileNamesNOTToSearch.Contains(sKBFileInfoValue))
                                                                {
                                                                    if (bDebugFileSelection) Console.WriteLine("DEBUG sKBFileInfoValue found in lFilesForProduct2 " + sKBFileInfoValue);
                                                                    bInterestingFile = true;
                                                                    lFilenames.Add(sKBFileInfoValue);
                                                                }
                                                                else
                                                                {
                                                                    if (lFilesToUse.Contains(sKBFileInfoValue.ToLower()) && !lFileNamesNOTToSearch.Contains(sKBFileInfoValue))
                                                                    {
                                                                        if (bDebugFileSelection) Console.WriteLine("DEBUG sKBFileInfoValue found in lFilesToUse2 " + sKBFileInfoValue);
                                                                        bInterestingFile = true;
                                                                        lFilenames.Add(sKBFileInfoValue);
                                                                    }
                                                                    else
                                                                    {
                                                                        foreach (string sFileNameToSearchFromList in lFileNamesToSearch)
                                                                        {
                                                                            if (sKBFileInfoValue.Contains(sFileNameToSearchFromList) && !lFileNamesNOTToSearch.Contains(sFileNameToSearchFromList))   //i.e. wac.office.riched20.dll
                                                                            {
                                                                                bInterestingFile = true;
                                                                                lFilenames.Add(sFileNameToSearchReplaced);
                                                                                break;  //we take just 1 match  ?
                                                                            }
                                                                        }

                                                                        //We ignore the file
                                                                        ////Add it anyway because we don't want 0
                                                                        //bInterestingFile = true;
                                                                        //lFilenames.Add(sKBFileInfoValue);
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                if (bInterestingFile)
                                                {
                                                    string sInterestingFileVersion = CSVlineSplit[iCptColumnFileVersion].Replace("\"", "");
                                                    //lFileversions.Add(sInterestingFileVersion);
                                                    Regex myRegexVersionNumber = new Regex(@"\d+(?:\.\d+)+");   //TODO Review Improve
                                                    string sInterestingFileVersionParsed = sInterestingFileVersion;
                                                    try
                                                    {
                                                        sInterestingFileVersionParsed = myRegexVersionNumber.Match(sInterestingFileVersion).ToString();  //NOTE: Removes (vistasp2_ldr.161027-0600)
                                                    }
                                                    catch(Exception exParseInterestingFileVersion)
                                                    {
                                                        Console.WriteLine("Exception: exParseInterestingFileVersion " + exParseInterestingFileVersion.Message + " " + exParseInterestingFileVersion.InnerException);
                                                    }
                                                    lFileversions.Add(sInterestingFileVersionParsed);

                                                    try
                                                    {
                                                        //Console.WriteLine("DEBUG File Date=" + sKBFileInfoValue);
                                                        string sDateForSorting = "";

                                                        if (bShittyCSV)
                                                        {
                                                            if (CSVlineSplit[iCptColumnDate] != "")
                                                            {
                                                                int iExcelDate;
                                                                bool result = Int32.TryParse(CSVlineSplit[iCptColumnDate], out iExcelDate);
                                                                if (result)
                                                                {
                                                                    //The Date is an integer    Thanks MS...
                                                                    //DateTime dt = DateTime.FromOADate(Int32.Parse(CSVlineSplit[iCptColumnDate]));   //39938 => 05/05/2009
                                                                    DateTime dt = DateTime.FromOADate(iExcelDate);   //39938 => 05/05/2009

                                                                    sDateForSorting = dt.ToString("yyyyMMdd");//, CultureInfo.InvariantCulture);
                                                                                                              //Console.WriteLine("DEBUG sDateForSorting=" + sDateForSorting);
                                                                }
                                                                else
                                                                {
                                                                    //20-Dec-16
                                                                    if (CSVlineSplit[iCptColumnDate].Contains("-"))
                                                                    {
                                                                        sDateForSorting = fGetDateForSorting(CSVlineSplit[iCptColumnDate]);
                                                                    }
                                                                    else
                                                                    {
                                                                        Console.WriteLine("ERROR: Excel Date Format NOT SUPPORTED: " + CSVlineSplit[iCptColumnDate]);
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                //No Date specified for this line/file in the CSV
                                                            }
                                                        }
                                                        else
                                                        {
                                                            sDateForSorting = fGetDateForSorting(CSVlineSplit[iCptColumnDate].Replace("\"", ""));
                                                        }
                                                        lFiledates.Add(sDateForSorting);

                                                        //Console.WriteLine("DEBUG InterestingFile=" + sDateForSorting + " " + sInterestingFileVersion+" "+sKBFileInfoValue);
                                                        //For XORCISM
                                                        //if (bInterestingFile)
                                                        //{
                                                        fXORCISMAddFILE(sKBFileInfoValue, sInterestingFileVersion, sDateForSorting);
                                                        //}
                                                    }
                                                    catch (Exception exFileDate1)
                                                    {
                                                        Console.WriteLine("Exception: exFileDate1 " + exFileDate1.Message + " " + exFileDate1.InnerException);
                                                        Console.WriteLine("ERROR: File Date=" + CSVlineSplit[iCptColumnDate]);
                                                    }
                                                    //Add even if empty (to avoid exception later)
                                                    lFileplatforms.Add(CSVlineSplit[iCptColumnPlatform].Replace("\"", ""));   //x86   x64 IA-64   Not applicable
                                                    
                                                }
                                            }
                                        }
                                        #endregion normalcsvline
                                    }
                                }
                            }
                            sCSVline = srStreamReader.ReadLine();
                        }
                        srStreamReader.Close();
                        //dispose
                    }
                    catch (Exception exParseKBCSVFile)
                    {
                        Console.WriteLine("Exception: exParseKBCSVFile " + exParseKBCSVFile.Message + " " + exParseKBCSVFile.InnerException);
                    }
                    #endregion parsecsv
                }
                //else  404 from MS?
            }
            catch (Exception exParseKBCSV)
            {
                Console.WriteLine("Exception: exParseKBCSV " + exParseKBCSV.Message + " " + exParseKBCSV.InnerException);
            }
            #endregion parsecsv

        }

        //public static void fParseKBForFiles(string sKBFileContent, string sFileNameToSearchReplaced)
        public static void fParseKBForFiles(string sKBFileContent, List<string> lFileNamesToSearch, int iParseKBMode=1)
        {
            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
            Console.WriteLine("DEBUG fParseKBForFiles()");
            //Parse a KB page content (local txt file) and get the Files+Information (Dates...)
            int iCptColumns = 0;
            int iCptColumnFileIdentifier = 0;   //Used when "File name" is empty...
            int iCptColumnFileName = 0;
            int iCptColumnFileVersion = 0;
            int iCptColumnDate = 0;
            int iCptColumnPlatform = 0;

            string sColumnClassTR = " class=\"sbody-tr\"";  //HARDCODEDMS
            string sColumnClassTH = " class=\"sbody-th\"";
            string sColumnClassTD = " class=\"sbody-td\"";


            int iCptFileLines = 0;

            #region parsekbforfiles
            //For all supported x86-based versions of Publisher 2010
            //For all supported x64-based versions of Publisher 2010

            //<tr class="sbody-tr"><th class="sbody-th">File identifier</th><th class="sbody-th">File name</th><th class="sbody-th">File version</th><th class="sbody-th">File size</th><th class="sbody-th">Date</th><th class="sbody-th">Time</th></tr>
            //<tr class="sbody-tr"><td class="sbody-td">morph9.dll</td><td class="sbody-td">morph9.dll</td><td class="sbody-td">14.0.7162.5000</td><td class="sbody-td">457,944</td><td class="sbody-td">14-Oct-2015</td><td class="sbody-td">11:57</td></tr>
            //vs
            //<tr><td><strong class="sbody-strong">File name</strong></td><td><strong class="sbody-strong">File version</strong></td><td><strong class="sbody-strong">File size</strong></td><td><strong class="sbody-strong">Date</strong></td><td><strong class="sbody-strong">Time</strong></td><td><strong class="sbody-strong">Platform</strong></td></tr>
            //<tr><td>Flash.ocx</td><td>24.0.0.221</td><td>28,321,272</td><td>06-Feb-2017</td><td>19:43</td><td>x64</td></tr>
            if(iParseKBMode==2)
            {
                sColumnClassTR = "";
                sColumnClassTH = "";
                sColumnClassTD = "";
            }
            
            //Regex myRegexKBFile = new Regex("<tr class=\"sbody-tr\">(.*?)</tr>", RegexOptions.Singleline); //HARDCODEDMSHTML
            //Regex myRegexKBFileInfoName = new Regex("<th class=\"sbody-th\">(.*?)</th>", RegexOptions.Singleline); //HARDCODEDMSHTML
            //Regex myRegexKBFileInfoValue = new Regex("<td class=\"sbody-td\">(.*?)</td>", RegexOptions.Singleline); //HARDCODEDMSHTML
            Regex myRegexKBFile = new Regex("<tr"+sColumnClassTR+">(.*?)</tr>", RegexOptions.Singleline); //HARDCODEDMSHTML
            Regex myRegexKBFileInfoName = new Regex("<th" + sColumnClassTH + ">(.*?)</th>", RegexOptions.Singleline); //HARDCODEDMSHTML
            if(iParseKBMode==2)
            {
                myRegexKBFileInfoName = new Regex("<td><strong class=\"sbody-strong\">(.*?)</strong></td>", RegexOptions.Singleline); //HARDCODEDMSHTML
            }
            Regex myRegexKBFileInfoValue = new Regex("<td" + sColumnClassTD + ">(.*?)</td>", RegexOptions.Singleline); //HARDCODEDMSHTML


            MatchCollection myKBFiles = myRegexKBFile.Matches(sKBFileContent);
            if (myKBFiles.Count == 0) Console.WriteLine("ERROR: myKBFiles.Count=0");
            foreach (Match matchKBFile in myKBFiles)    //For each line/row/file
            {
                //Console.WriteLine("DEBUG myRegexFile=" + matchKBFile);
                if (matchKBFile.Value.Contains("th class") || (iParseKBMode == 2 && iCptColumnPlatform == 0)) //HARDCODEDMSHTML
                {
                    //Column names
                    //Console.WriteLine("DEBUG ColumnNames");
                    #region getcolumnsindex
                    iCptColumns = 0;
                    iCptFileLines = 0;
                    MatchCollection myKBFileInfoNames = myRegexKBFileInfoName.Matches(matchKBFile.Value);
                    foreach (Match matchKBFileInfoName in myKBFileInfoNames)
                    {
                        
                        iCptColumns++;
                        //string sKBFileInfoName = matchKBFileInfoName.Value.Replace("<th class=\"sbody-th\">", "");  //HARDCODEDMSHTML
                        //sKBFileInfoName = sKBFileInfoName.Replace("</th>", "");
                        string sKBFileInfoName = StripTagsRegexCompiled(matchKBFileInfoName.Value);

                        //Console.WriteLine("DEBUG sKBFileInfoName=" + sKBFileInfoName);
                        //Package Name
                        //Package Hash SHA 1
                        //Package Hash SHA 2

                        
                        //File identifier
                        if (sKBFileInfoName == "File identifier") iCptColumnFileIdentifier = iCptColumns;   //HARDCODEDMSHTML
                        //File name
                        if (sKBFileInfoName == "File name") iCptColumnFileName = iCptColumns;               //HARDCODEDMSHTML
                        //File version
                        if (sKBFileInfoName == "File version") iCptColumnFileVersion = iCptColumns;         //HARDCODEDMSHTML
                        //File size
                        //Date
                        if (sKBFileInfoName == "Date") iCptColumnDate = iCptColumns;                        //HARDCODEDMSHTML
                        //Time
                        //Platform
                        if (sKBFileInfoName == "Platform") iCptColumnPlatform = iCptColumns;                //HARDCODEDMSHTML
                        //SP requirement
                        //Service branch
                    }
                    #endregion getcolumnsindex
                    Console.WriteLine("DEBUG iCptColumnFileName=" + iCptColumnFileName);
                }
                else
                {
                    //File infos
                    #region fileinfoinkbpage
                    iCptColumns = 0;
                    iCptFileLines++;
                    bool bInterestingFile = false;
                    string sKBFileIdentifierValue = "";

                    MatchCollection myKBFileInfoValues = myRegexKBFileInfoValue.Matches(matchKBFile.Value);
                    foreach (Match matchKBFileInfoValue in myKBFileInfoValues)  //foreach column
                    {
                        iCptColumns++;
                        //cleaning
                        //string sKBFileInfoValue = matchKBFileInfoValue.Value.Replace("<td class=\"sbody-td\">", "");    //HARDCODEDMSHTML
                        //sKBFileInfoValue = sKBFileInfoValue.Replace("</td>", "");
                        string sKBFileInfoValue = StripTagsRegexCompiled(matchKBFileInfoValue.Value);

                        sKBFileInfoValue = sKBFileInfoValue.ToLower();
                        
                        //Console.WriteLine("DEBUG sKBFileInfoValue=" + sKBFileInfoValue);
                        //publisher2010-kb3114395-fullfile-x86-glb.exe
                        //7234DF1B51092843D7EC2EB46831F7E5747ECB78
                        //...

                        //Console.WriteLine("DEBUG iCptColumns=" + iCptColumns);
                        //Console.WriteLine("DEBUG sKBFileInfoValue=" + sKBFileInfoValue);
                        
                        if (iCptColumns == iCptColumnFileIdentifier)
                        {
                            sKBFileIdentifierValue = sKBFileInfoValue;
                            if (sKBFileInfoValue.Contains("|")) Console.WriteLine("WARNING: PipeInsKBFileIdentifierValue");
                        }
                        //morph9.dll
                        //morph9.dll
                        if (iCptColumns == iCptColumnFileName)
                        {
                            //    Console.WriteLine("DEBUG File Name=" + sKBFileInfoValue);
                            if (sKBFileInfoValue.Trim() == "") sKBFileInfoValue = sKBFileIdentifierValue;
                            sKBFileInfoValue = sKBFileInfoValue.ToLower();
                            //We don't want .png    idx_dll manifest...
                            //Review: Regex
                            if (sKBFileInfoValue.EndsWith(".dll") || sKBFileInfoValue.EndsWith(".exe") || sKBFileInfoValue.EndsWith(".sys") || sKBFileInfoValue.EndsWith(".ocx") || sKBFileInfoValue.EndsWith(".aspx") || sKBFileInfoValue.EndsWith(".flt")) //HARDCODED others? .config? ...
                            {
                                //Console.WriteLine("DEBUG sKBFileInfoValue=" + sKBFileInfoValue);
                                //if (sFileNameToSearchReplaced == "")
                                if (lFileNamesToSearch.Count == 0 || iCptFileLines == 1)    //We don't know what File we are looking for OR we will take the first one (sometimes the only one i.e. KB3118311 wwlibcxm.dll)
                                {
                                    bInterestingFile = true;
                                    if (sKBFileInfoValue.Contains("|"))//excelcnv.exe|excelconv.exe
                                    {
                                        Console.WriteLine("NOTE: PipeInsKBFileInfoValue1 we keep only one filename");
                                        lFilenames.Add(sKBFileInfoValue.Split('|')[0]);
                                    }
                                    else
                                    {
                                        if(!lFileNamesNOTToSearch.Contains(sKBFileInfoValue)) lFilenames.Add(sKBFileInfoValue);
                                    }
                                }
                                else
                                {
                                    //excelcnv.exe|excelconv.exe
                                    if (sKBFileInfoValue.Contains("|"))// && sKBFileInfoValue.Contains(sFileNameToSearchReplaced))
                                    {
                                        foreach (string sFileNameToSearchFromList in lFileNamesToSearch)
                                        {
                                            if (sKBFileInfoValue.Contains(sFileNameToSearchFromList) && !lFileNamesNOTToSearch.Contains(sFileNameToSearchFromList))   //NOTE: we could be wrong
                                            {
                                                bInterestingFile = true;
                                                lFilenames.Add(sFileNameToSearchFromList);
                                                sKBFileInfoValue = sFileNameToSearchFromList;   //For below XORCISM
                                                //break;  //we take just 1 match
                                            }
                                        }
                                    }
                                    else
                                    {
                                        ////if (sFileNameToSearchReplaced.ToLower() == sKBFileInfoValue.ToLower())
                                        //if (lFileNamesToSearch.Contains(sKBFileInfoValue.ToLower()))    //NOTE: We could be wrong. i.e.: https://support.microsoft.com/en-us/help/3114395/ms16-148-description-of-the-security-update-for-publisher-2010-december-13,-2016
                                        //{
                                            if (sKBFileInfoValue.Contains("|"))//excelcnv.exe|excelconv.exe
                                            {
                                                Console.WriteLine("NOTE: PipeInsKBFileInfoValue2 we keep only one filename");
                                                bInterestingFile = true;
                                                lFilenames.Add(sKBFileInfoValue.Split('|')[0]);
                                            }
                                            else
                                            {
                                                if (!lFileNamesNOTToSearch.Contains(sKBFileInfoValue))
                                                {
                                                    bInterestingFile = true;
                                                    lFilenames.Add(sKBFileInfoValue);
                                                }
                                            }
                                        //}
                                        //else  We ignore the file
                                    }
                                }

                                //For XORCISM
                                if (bInterestingFile)
                                {
                                    int iFileID = fXORCISMAddFILE(sKBFileInfoValue);
                                }
                            }
                        }

                        if (bInterestingFile)
                        {
                            //Console.WriteLine("DEBUG sKBFileInfoValueInteresting=" + sKBFileInfoValue);
                            #region getfileinfo
                            //14.0.7162.5000
                            if (iCptColumns == iCptColumnFileVersion)
                            {
                                //    Console.WriteLine("DEBUG File Version=" + sKBFileInfoValue);
                                //lFileversions.Add(sKBFileInfoValue);
                                Regex myRegexVersionNumber = new Regex(@"\d+(?:\.\d+)+");   //TODO Review Improve
                                lFileversions.Add(myRegexVersionNumber.Match(sKBFileInfoValue.Replace(",",".")).ToString());  //NOTE: Removes (vistasp2_ldr.161027-0600)

                            }
                            //457,944
                            //14-Oct-2015
                            if (iCptColumns == iCptColumnDate)
                            {
                                try
                                {
                                    //Console.WriteLine("DEBUG File Date=" + sKBFileInfoValue);
                                    string sDateForSorting = fGetDateForSorting(sKBFileInfoValue);
                                    lFiledates.Add(sDateForSorting);
                                }
                                catch (Exception exFileDate)
                                {
                                    Console.WriteLine("Exception: exFileDate " + exFileDate.Message + " " + exFileDate.InnerException);
                                    Console.WriteLine("ERROR: File Date=" + sKBFileInfoValue);
                                }
                            }
                            //11:57
                            //...
                            if (iCptColumns == iCptColumnPlatform)
                            {
                                lFileplatforms.Add(sKBFileInfoValue);   //x86   x64 IA-64   Not applicable
                            }
                            #endregion getfileinfo
                        }
                    }
                    #endregion fileinfoinkbpage
                }
            }
            #endregion parsekbforfiles
        }


        public static string fSearchVulnerableFilename(string sTextInput)
        {
            string sFileNameFound = "";
            //Regex myRegexDLLFileName = new Regex(@"\w*\.(dll|exe|ocx|sys)", RegexOptions.Singleline);   //Hardcoded
            Regex myRegexDLLFileName = new Regex(@"[A-Za-z0-9.]*\.(dll|exe|ocx|sys|config|aspx)", RegexOptions.Singleline);   //HARDCODED   .flt
            try
            {
                sFileNameFound = myRegexDLLFileName.Match(sTextInput).ToString().Trim();    //Take just the 1st one
                if(sFileNameFound!="") Console.WriteLine("DEBUG sFileNameFound: " + sFileNameFound);
            }
            catch (Exception exmyRegexDLLFileName)
            {

            }
            if (sFileNameFound=="" && sTextInput.Contains("notepad")) sFileNameFound = "notepad.exe";

            if (sTextInput.Contains("different vulnerability than")) //a different vulnerability than CVE-2016-3266, CVE-2016-3376, and CVE-2016-7185
            {
                //If we already know the filenames covered by the other CVEs, we can exclude them to obtain our target filename
                //This will be simplified by OVALDEFINITIONFILE
                Console.WriteLine("DEBUG DifferentVulnerabilityThan");
                string[] TempSplit = sTextInput.Split(new string[] { "different vulnerability than" }, StringSplitOptions.None); //HARDCODEDNVDCVE
                Regex myRegexCVE = new Regex(@"CVE-(19|20)\d\d-(0\d{3}|[1-9]\d{3,})");
                MatchCollection myCVEs = myRegexCVE.Matches(TempSplit[1].ToUpper());
                foreach (Match matchCVE in myCVEs)
                {
                    Console.WriteLine("DEBUG matchCVE=" + matchCVE);
                    string sMyCVE = matchCVE.ToString();
                    int iVulnerabilityID = 0;
                    try
                    {
                        iVulnerabilityID = vuln_model.VULNERABILITY.Where(o => o.VULReferentialID == sMyCVE).FirstOrDefault().VulnerabilityID; //&& o.VULReferential=="cve"
                    }
                    catch (Exception ex)
                    {

                    }
                    if (iVulnerabilityID > 0)
                    {
                        int iOVALDefinitionID = 0;
                        try
                        {
                            //TODO Review (versions)
                            iOVALDefinitionID = (int)oval_model.OVALDEFINITIONVULNERABILITY.Where(o => o.VulnerabilityID == iVulnerabilityID).FirstOrDefault().OVALDefinitionID;  //Note that we could have different definitions for the same vuln
                        }
                        catch (Exception ex)
                        {

                        }
                        if (iOVALDefinitionID > 0)
                        {
                            Console.WriteLine("DEBUG iOVALDefinitionID=" + iOVALDefinitionID);
                            //int iOVALDefinitionID = 0;
                            int iOVALCriteriaID = 0;
                            try
                            {
                                iOVALCriteriaID = (int)oval_model.OVALDEFINITION.Where(o => o.OVALDefinitionID == iOVALDefinitionID).FirstOrDefault().OVALCriteriaID;
                            }
                            catch (Exception ex)
                            {

                            }
                            if (iOVALCriteriaID > 0)
                            {
                                Console.WriteLine("DEBUG iOVALCriteriaID=" + iOVALCriteriaID);
                                IEnumerable<OVALCRITERIAFOROVALCRITERIA> OVALDefinitionCriterias = oval_model.OVALCRITERIAFOROVALCRITERIA.Where(o => o.OVALCriteriaRefID == iOVALCriteriaID);
                                //int iCptCriteria = 0;
                                foreach (OVALCRITERIAFOROVALCRITERIA oOVALCriteriaCriteria in OVALDefinitionCriterias)
                                {
                                    foreach (OVALCRITERIACRITERION oOVALCriteriaCriterion in oOVALCriteriaCriteria.OVALCRITERIA1.OVALCRITERIACRITERION)
                                    {
                                        //OVALTESTs
                                        string sTestType = oOVALCriteriaCriterion.OVALTEST.OVALTESTDATATYPE.OVALTestDataTypeName.ToUpper();
                                        
                                        Console.WriteLine("DEBUG " + "  " + sTestType + " " + oOVALCriteriaCriterion.OVALTEST.OVALTestIDPattern + " version=" + oOVALCriteriaCriterion.OVALTEST.OVALTestVersion);
                                        Console.WriteLine("DEBUG " + "  " + oOVALCriteriaCriterion.OVALTEST.comment);
                                        /*
                                        //oOVALCriteriaCriterion.OVALTEST.ExistenceEnumerationID
                                        //oOVALCriteriaCriterion.OVALTEST.CheckEnumerationID
                                        //oOVALCriteriaCriterion.OVALTEST.OperatorEnumerationID
                                        */

                                        if (sTestType == "file_test")   //Hardcoded
                                        {
                                            //Console.WriteLine("DEBUG fDisplayCriteria5");
                                            //OVALOBJECTs
                                            IEnumerable<OVALOBJECTFOROVALTEST> OVALObjects = oval_model.OVALOBJECTFOROVALTEST.Where(o => o.OVALTestID == oOVALCriteriaCriterion.OVALTEST.OVALTestID);

                                            foreach (OVALOBJECTFOROVALTEST oObject in OVALObjects)
                                            {
                                                Console.WriteLine("DEBUG " + "    " + oObject.OVALOBJECT.OVALObjectIDPattern);
                                                IEnumerable<OVALOBJECTFILE> OVALObjectFiles = oval_model.OVALOBJECTFILE.Where(o => o.OVALObjectID == oObject.OVALObjectID);
                                                foreach (OVALOBJECTFILE oObjectFile in OVALObjectFiles)
                                                {
                                                    string sFileName = model.FILE.Where(o => o.FileID == oObjectFile.FileID).FirstOrDefault().FileName.ToLower();
                                                    Console.WriteLine("DEBUG sFileName=" + sFileName + " used for " + matchCVE.ToString());
                                                    if (!lFileNamesNOTToSearch.Contains(sFileName)) lFileNamesNOTToSearch.Add(sFileName);
                                                }
                                            }
                                        }
                                    }
                                    
                                    //fDisplayCriteria(oOVALCriteriaCriteria.OVALCRITERIA1.OVALCriteriaID, sTab2);
                                }

                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("ERROR: Unknown CVE " + sMyCVE);
                    }
                }
            }

            if (lFileNamesNOTToSearch.Contains(sFileNameFound.ToLower()))
            {
                //UNLIKELY
                return "";
            }
            else
            {
                return sFileNameFound.ToLower();
            }
        }


        public static OVALDEFINITION fGetOVALInventoryDefinitionForProduct(string sProductNameModified)
        {
            if (lOVALDefInventoryNotRetrieved.Contains(sProductNameModified) || sProductNameModified.Trim()=="") return null;

            string sProductNameInput = sProductNameModified.ToLower();
            Console.WriteLine("DEBUG fGetOVALInventoryDefinitionForProduct sProductNameModified=" + sProductNameModified);
            //HARDCODED (because OVAL Definition Titles are not standardized); not optimized
            //oval:org.cisecurity:def:  vs  oval:org.mitre.oval:def:
            //i.e.: Microsoft Windows 10 (32-bit) is installed    vs  Microsoft Windows 10 (x86) is installed

            OVALDEFINITION oOVALInventoryDefinition = new OVALDEFINITION();
            List<string> lProductNames = new List<string>();    //Used for performing various modifications on the ProductName

            //if (sProductNameModified == "windows server 2016 x64") sProductNameModified = "windows server 2016";    //Hardcoded - TODO: Review bBypassExtendDef1
            if (sProductNameModified.EndsWith("2016 x64")) sProductNameModified = sProductNameModified.Replace(" 2016 x64", "");    //Hardcoded - TODO: Review bBypassExtendDef1    (Windows Server 2016 vs Office 2016)

            //microsoft xml core services 3.0
            //microsoft xml core services 6.0
            if (sProductNameModified.Contains("microsoft xml core services") && sProductNameModified.EndsWith(".0")) sProductNameModified = sProductNameModified.Replace(".0", "");

            lProductNames.Add(sProductNameModified);
            string sProductNameModifiedTemp = sProductNameModified.Replace(" x86", " (x86)");
            if (!lProductNames.Contains(sProductNameModifiedTemp)) lProductNames.Add(sProductNameModifiedTemp);      //mitre
            sProductNameModifiedTemp = sProductNameModified.Replace(" x86", " (32-bit)");
            if (!lProductNames.Contains(sProductNameModifiedTemp)) lProductNames.Add(sProductNameModifiedTemp);   //cisecurity
            sProductNameModifiedTemp = sProductNameModified.Replace(" x64", " (x64)");
            if (!lProductNames.Contains(sProductNameModifiedTemp)) lProductNames.Add(sProductNameModifiedTemp);      //mitre
            sProductNameModifiedTemp = sProductNameModified.Replace(" x64", " (64-bit)");
            if (!lProductNames.Contains(sProductNameModifiedTemp)) lProductNames.Add(sProductNameModifiedTemp);   //cisecurity

            sProductNameModifiedTemp = sProductNameModified.Replace(" ia64", " (ia-64)");
            if (!lProductNames.Contains(sProductNameModifiedTemp)) lProductNames.Add(sProductNameModifiedTemp);
            sProductNameModifiedTemp = sProductNameModified.Replace(" itanium-based edition", " for itanium");
            if (!lProductNames.Contains(sProductNameModifiedTemp)) lProductNames.Add(sProductNameModifiedTemp);
            sProductNameModifiedTemp = sProductNameModified.Replace(" for itanium sp1", " sp1 for itanium");
            if (!lProductNames.Contains(sProductNameModifiedTemp)) lProductNames.Add(sProductNameModifiedTemp);
            sProductNameModifiedTemp = sProductNameModified.Replace(" systems", " edition");
            if (!lProductNames.Contains(sProductNameModifiedTemp)) lProductNames.Add(sProductNameModifiedTemp);
            
            sProductNameModifiedTemp = sProductNameModified.Replace("microsoft sharepoint", "microsoft office sharepoint");   //server    designer    services...
            if (!lProductNames.Contains(sProductNameModifiedTemp)) lProductNames.Add(sProductNameModifiedTemp);

            sProductNameModifiedTemp = sProductNameModified.Replace("microsoft onenote", "microsoft office onenote");
            if (!lProductNames.Contains(sProductNameModifiedTemp)) lProductNames.Add(sProductNameModifiedTemp);

            //project server
            sProductNameModifiedTemp = sProductNameModified.Replace("microsoft project", "microsoft office project");
            if (!lProductNames.Contains(sProductNameModifiedTemp)) lProductNames.Add(sProductNameModifiedTemp);
            sProductNameModifiedTemp = sProductNameModified.Replace("microsoft office project", "microsoft office project professional");  //standard
            if (!lProductNames.Contains(sProductNameModifiedTemp)) lProductNames.Add(sProductNameModifiedTemp);
            sProductNameModifiedTemp = sProductNameModified.Replace("microsoft office project professional", "microsoft project professional");  //standard
            if (!lProductNames.Contains(sProductNameModifiedTemp)) lProductNames.Add(sProductNameModifiedTemp);
            sProductNameModifiedTemp = sProductNameModified.Replace("microsoft project", "microsoft office project professional");  //standard
            if (!lProductNames.Contains(sProductNameModifiedTemp)) lProductNames.Add(sProductNameModifiedTemp);
            sProductNameModifiedTemp = sProductNameModified.Replace("microsoft project", "microsoft project professional"); //standard
            if (!lProductNames.Contains(sProductNameModifiedTemp)) lProductNames.Add(sProductNameModifiedTemp);

            sProductNameModifiedTemp = sProductNameModified.Replace("skype business", "skype for business");
            if (!lProductNames.Contains(sProductNameModifiedTemp)) lProductNames.Add(sProductNameModifiedTemp);

            sProductNameModifiedTemp = sProductNameModified.Replace("microsoft onenote", "microsoft office onenote");   //2007
            if (!lProductNames.Contains(sProductNameModifiedTemp)) lProductNames.Add(sProductNameModifiedTemp);

            sProductNameModifiedTemp = sProductNameModified.Replace("microsoft excel web apps", "microsoft office web apps");   //
            if (!lProductNames.Contains(sProductNameModifiedTemp)) lProductNames.Add(sProductNameModifiedTemp);
            

            //Microsoft Windows Server 2003 SP1 for Itanium is installed
            //Microsoft Windows Server 2008 R2 Itanium-Based Edition Service Pack 1 Release Candidate is installed

            //TODO Regex
            
            if (sProductNameModified.Contains(" service pack"))
            {
                for (int iSP = 1; iSP < 6; iSP++) //Hardcoded
                {
                    sProductNameModifiedTemp = sProductNameModified.Replace(" service pack " + iSP, " sp" + iSP);
                    if (!lProductNames.Contains(sProductNameModifiedTemp)) lProductNames.Add(sProductNameModifiedTemp);

                    sProductNameModifiedTemp = sProductNameModified.Replace(" service pack " + iSP+" x64", " x64 service pack " + iSP);
                    if (!lProductNames.Contains(sProductNameModifiedTemp)) lProductNames.Add(sProductNameModifiedTemp);
                    sProductNameModifiedTemp = sProductNameModified.Replace(" service pack " + iSP + " x64", " x64 edition service pack " + iSP);
                    if (!lProductNames.Contains(sProductNameModifiedTemp)) lProductNames.Add(sProductNameModifiedTemp);
                    
                    sProductNameModifiedTemp = sProductNameModified.Replace(" service pack " + iSP + " x86", " (32-bit) service pack " + iSP);
                    if (!lProductNames.Contains(sProductNameModifiedTemp)) lProductNames.Add(sProductNameModifiedTemp);
                }
            }
            else
            {
                if (sProductNameModified.Contains(" sp"))
                {
                    for (int iSP = 1; iSP < 6; iSP++) //Hardcoded
                    {
                        sProductNameModifiedTemp = sProductNameModified.Replace(" sp" + iSP, " service pack " + iSP);
                        if (!lProductNames.Contains(sProductNameModifiedTemp)) lProductNames.Add(sProductNameModifiedTemp);
                    }
                }
            }

            //1st try   (will work with "Edition")
            foreach (string sProductNameTry in lProductNames)
            {
                //Console.WriteLine("DEBUG sProductNameTry=" + sProductNameTry);
                //TODO: && o.StatusName=="ACCEPTED" && o.deprecated!=1      min_schema_version
                if (sProductNameModified.Contains(" sp") || sProductNameModified.Contains(" service pack"))
                {
                    #region searchovalinventorydefinitionSP
                    oOVALInventoryDefinition = oval_model.OVALDEFINITION.Where(o => o.OVALClassEnumerationID == iOVALClassEnumerationInventoryID && o.OSFamilyID == iOSFamilyWindowsID && o.OVALDefinitionTitle.ToLower().Contains(sProductNameTry) && o.OVALDefinitionTitle.ToLower().Contains(" installed") && o.deprecated != true && (o.OVALDefinitionTitle.ToLower().Contains(" sp") || o.OVALDefinitionTitle.ToLower().Contains(" service pack"))).OrderByDescending(o => o.OVALDefinitionVersion).FirstOrDefault(); //Hardcoded
                    if ((sProductNameTry.Contains(" sp") || sProductNameTry.Contains(" service pack")) && oOVALInventoryDefinition == null)
                    {
                        //Without x86 x64
                        //HARDCODED
                        string sProductNameTryModified = sProductNameTry;
                        sProductNameTryModified = sProductNameTryModified.Replace(" (32-bit)", "");
                        sProductNameTryModified = sProductNameTryModified.Replace(" (x86)", "");
                        sProductNameTryModified = sProductNameTryModified.Replace(" (64-bit)", "");
                        sProductNameTryModified = sProductNameTryModified.Replace(" (x64)", "");
                        sProductNameTryModified = sProductNameTryModified.Replace(" x86", "");
                        sProductNameTryModified = sProductNameTryModified.Replace(" x64", "");
                        sProductNameTryModified = sProductNameTryModified.Replace(" ia64", "");
                        sProductNameTryModified = sProductNameTryModified.Replace(" itanium-based edition", "");
                        sProductNameTryModified = sProductNameTryModified.Replace(" release candidate", "");

                        oOVALInventoryDefinition = oval_model.OVALDEFINITION.Where(o => o.OVALClassEnumerationID == iOVALClassEnumerationInventoryID && o.OSFamilyID == iOSFamilyWindowsID && o.OVALDefinitionTitle.ToLower().Contains(sProductNameTryModified) && o.OVALDefinitionTitle.ToLower().Contains(" installed") && o.deprecated != true && (o.OVALDefinitionTitle.ToLower().Contains(" sp") || o.OVALDefinitionTitle.ToLower().Contains(" service pack"))).OrderByDescending(o => o.OVALDefinitionVersion).FirstOrDefault(); //Hardcoded

                        if (oOVALInventoryDefinition == null)
                        {
                            //Console.WriteLine("DEBUG Continue1");
                            continue;
                        }
                    }
                    if (oOVALInventoryDefinition != null)
                    {
                        //Is it the same SP? (it should be)

                        //Console.WriteLine("DEBUG Break5");
                        break;
                    }
                    
                    //SP vs Service Pack
                    oOVALInventoryDefinition = oval_model.OVALDEFINITION.Where(o => o.OVALClassEnumerationID == iOVALClassEnumerationInventoryID && o.OSFamilyID == iOSFamilyWindowsID && o.OVALDefinitionTitle.ToLower().Contains(sProductNameTry.Replace(" sp", " service pack ")) && o.OVALDefinitionTitle.ToLower().Contains(" installed") && o.deprecated != true).OrderByDescending(o => o.OVALDefinitionVersion).FirstOrDefault(); //Hardcoded
                    if (oOVALInventoryDefinition != null)
                    {
                        Console.WriteLine("DEBUG Break4");
                        break;
                    }

                    if (sProductNameTry.Contains("service pack"))
                    {
                        oOVALInventoryDefinition = oval_model.OVALDEFINITION.Where(o => o.OVALClassEnumerationID == iOVALClassEnumerationInventoryID && o.OSFamilyID == iOSFamilyWindowsID && o.OVALDefinitionTitle.ToLower().Contains(sProductNameTry.Replace(" service pack ", " sp")) && o.OVALDefinitionTitle.ToLower().Contains(" installed") && o.deprecated != true).OrderByDescending(o => o.OVALDefinitionVersion).FirstOrDefault(); //Hardcoded
                        if (oOVALInventoryDefinition != null)
                        {
                            if(bDebugFilesFound) Console.WriteLine("DEBUG Break3");
                            break;
                        }

                        Regex myRegexServicePack = new Regex(@"service pack \d", RegexOptions.Singleline);
                        string sProductNameTryServicePack = myRegexServicePack.Match(sProductNameTry).ToString();
                        oOVALInventoryDefinition = oval_model.OVALDEFINITION.Where(o => o.OVALClassEnumerationID == iOVALClassEnumerationInventoryID && o.OSFamilyID == iOSFamilyWindowsID && o.OVALDefinitionTitle.ToLower().Contains(sProductNameTry.Replace(" " + sProductNameTryServicePack, "")) && o.OVALDefinitionTitle.ToLower().Contains(sProductNameTryServicePack) && o.OVALDefinitionTitle.ToLower().Contains(" installed") && o.deprecated != true).OrderByDescending(o => o.OVALDefinitionVersion).FirstOrDefault(); //Hardcoded
                        if (oOVALInventoryDefinition != null)
                        {
                            Console.WriteLine("DEBUG Break2");
                            break;
                        }
                    }
                    #endregion searchovalinventorydefinitionSP
                }
                else
                {
                    //NO SP, NO Service Pack
                    oOVALInventoryDefinition = oval_model.OVALDEFINITION.Where(o => o.OVALClassEnumerationID == iOVALClassEnumerationInventoryID && o.OSFamilyID == iOSFamilyWindowsID && o.OVALDefinitionTitle.ToLower().Contains(sProductNameTry) && o.OVALDefinitionTitle.ToLower().Contains(" installed") && o.deprecated != true && !o.OVALDefinitionTitle.ToLower().Contains(" sp") && !o.OVALDefinitionTitle.ToLower().Contains(" service pack")).OrderByDescending(o => o.OVALDefinitionVersion).FirstOrDefault(); //Hardcoded
                    if (oOVALInventoryDefinition == null)
                    {

                        oOVALInventoryDefinition = oval_model.OVALDEFINITION.Where(o => o.OVALClassEnumerationID == iOVALClassEnumerationInventoryID && o.OSFamilyID == iOSFamilyWindowsID && o.OVALDefinitionTitle.ToLower().Contains(sProductNameTry.Replace(" x64","").Replace(" x86","")) && o.OVALDefinitionTitle.ToLower().Contains(" installed") && o.deprecated != true && !o.OVALDefinitionTitle.ToLower().Contains(" sp") && !o.OVALDefinitionTitle.ToLower().Contains(" service pack")).OrderByDescending(o => o.OVALDefinitionVersion).FirstOrDefault(); //Hardcoded
                    
                    }
                }
                if (oOVALInventoryDefinition != null)
                {
                    Console.WriteLine("DEBUG Break1");
                    break;
                }
                

                //lOVALInventoryDefinitions.Add()
            }

            if (oOVALInventoryDefinition != null)
            {

            }
            else
            {
                Console.WriteLine("NOTE: OVALDefInventory not retrieved for sProductNameModified=" + sProductNameModified);
                string sProductNameTry = sProductNameModified;
                //Microsoft Office 2013 SP1 x86 is installed
                //Microsoft Office 2013 SP1 x64 is installed
                //Retrieving the Best Match (TODO: Not optimized)

                //Without x86 x64
                //string sOVALDefInventoryTitleModified = sOVALDefInventoryTitle.ToLower().Replace(" x86", "").Replace(" x64", "");
                //HARDCODED
                sProductNameModified = sProductNameModified.Replace(" (32-bit)", "");
                sProductNameModified = sProductNameModified.Replace(" (x86)", "");
                sProductNameModified = sProductNameModified.Replace(" (64-bit)", "");
                sProductNameModified = sProductNameModified.Replace(" (x64)", "");
                sProductNameModified = sProductNameModified.Replace(" x86", "");
                sProductNameModified = sProductNameModified.Replace(" x64", "");
                sProductNameModified = sProductNameModified.Replace(" ia64", "");
                sProductNameModified = sProductNameModified.Replace(" itanium-based edition", "");
                sProductNameModified = sProductNameModified.Replace(" release candidate", "");

                oOVALInventoryDefinition = oval_model.OVALDEFINITION.Where(o => o.OVALClassEnumerationID == iOVALClassEnumerationInventoryID && o.OSFamilyID == iOSFamilyWindowsID && o.OVALDefinitionTitle.ToLower().Contains(sProductNameModified) && o.OVALDefinitionTitle.ToLower().Contains(" installed") && o.deprecated != true).OrderByDescending(o => o.OVALDefinitionVersion).FirstOrDefault(); //Hardcoded
                

                //Console.WriteLine("DEBUG sProductNameModifiedForOVALInventory=" + sProductNameModified);
                /*
                if (sProductNameModified.Contains(" sp") || sProductNameModified.Contains(" service pack"))
                {
                    oOVALInventoryDefinition = oval_model.OVALDEFINITION.Where(o => o.OVALClassEnumerationID == iOVALClassEnumerationInventoryID && o.OSFamilyID == iOSFamilyWindowsID && o.OVALDefinitionTitle.ToLower().Contains(sProductNameModified) && o.OVALDefinitionTitle.ToLower().Contains(" installed") && o.deprecated != true && (o.OVALDefinitionTitle.ToLower().Contains(" sp") || o.OVALDefinitionTitle.ToLower().Contains(" service pack"))).OrderByDescending(o => o.OVALDefinitionVersion).FirstOrDefault(); //Hardcoded
                }
                else
                {
                    oOVALInventoryDefinition = oval_model.OVALDEFINITION.Where(o => o.OVALClassEnumerationID == iOVALClassEnumerationInventoryID && o.OSFamilyID == iOSFamilyWindowsID && o.OVALDefinitionTitle.ToLower().Contains(sProductNameModified) && o.OVALDefinitionTitle.ToLower().Contains(" installed") && o.deprecated != true).OrderByDescending(o => o.OVALDefinitionVersion).FirstOrDefault(); //Hardcoded
                }
                */
                if (oOVALInventoryDefinition != null)
                {
                    
                    if (sProductNameTry.Contains("x86") || sProductNameTry.Contains("x64"))
                    {
                        if (oOVALInventoryDefinition.OVALDefinitionTitle.Contains("x64") && sProductNameTry.Contains("x86"))    //SP1 x64   vs  x64 SP1
                        {
                            //Fix it
                            oOVALInventoryDefinition = oval_model.OVALDEFINITION.Where(o => o.OVALClassEnumerationID == iOVALClassEnumerationInventoryID && o.OSFamilyID == iOSFamilyWindowsID && o.OVALDefinitionTitle.ToLower().Contains(sProductNameModified + " x86") && o.OVALDefinitionTitle.ToLower().Contains(" installed") && o.deprecated != true).OrderByDescending(o => o.OVALDefinitionVersion).FirstOrDefault(); //Hardcoded
                        }
                        else
                        {
                            if (oOVALInventoryDefinition.OVALDefinitionTitle.Contains("x86") && sProductNameTry.Contains("x64"))    //SP1 x86   vs  x86 SP1
                            {
                                //(Apparently never happens)
                                //Fix it
                                oOVALInventoryDefinition = oval_model.OVALDEFINITION.Where(o => o.OVALClassEnumerationID == iOVALClassEnumerationInventoryID && o.OSFamilyID == iOSFamilyWindowsID && o.OVALDefinitionTitle.ToLower().Contains(sProductNameModified + " x64") && o.OVALDefinitionTitle.ToLower().Contains(" installed") && o.deprecated != true).OrderByDescending(o => o.OVALDefinitionVersion).FirstOrDefault(); //Hardcoded
                            }
                            else
                            {
                                //if (!bBypassBlock && !bSkipExtendDefinition)
                                //{
                                    //bBypassExtendDef = true;    //TODO Review
                                    //Console.WriteLine("DEBUG bBypassExtendDef1 = true");
                                //}
                            }
                        }
                    }
                    Console.WriteLine("NOTE: OVALDefInventory Best Match1: " + oOVALInventoryDefinition.OVALDefinitionTitle);

                    //break;
                }
                else
                {
                    //SP vs Service Pack
                    if (sProductNameModified.Contains(" sp") || sProductNameModified.Contains(" service pack"))
                    {
                        //Try Removing SPx
                        for (int iSP = 1; iSP < 6; iSP++)   //Hardcoded
                        {
                            //sOVALDefInventoryTitleModified = sOVALDefInventoryTitleModified.Replace(" sp" + iYear.ToString(), "");
                            sProductNameModified = sProductNameModified.Replace(" sp" + iSP.ToString(), "");
                            sProductNameModified = sProductNameModified.Replace(" service pack" + iSP.ToString(), "");
                        }
                        oOVALInventoryDefinition = oval_model.OVALDEFINITION.Where(o => o.OVALClassEnumerationID == iOVALClassEnumerationInventoryID && o.OSFamilyID == iOSFamilyWindowsID && o.OVALDefinitionTitle.ToLower().Contains(sProductNameModified) && o.OVALDefinitionTitle.ToLower().Contains(" installed") && o.deprecated != true).OrderByDescending(o => o.OVALDefinitionVersion).FirstOrDefault(); //Hardcoded
                    }
                    if (oOVALInventoryDefinition != null)
                    {
                        Console.WriteLine("NOTE: OVALDefInventory Best Match2: " + oOVALInventoryDefinition.OVALDefinitionTitle);
                        //break;
                    }
                    else
                    {
                        if (sProductNameTry.Contains(" sp") || sProductNameTry.Contains(" service pack"))
                        {
                            //Try Without the Year  i.e. Office 2016 => Office
                            //sOVALDefInventoryTitleModified = fRemoveYear(sOVALDefInventoryTitleModified);   //i.e. Microsoft Word Viewer is installed
                            sProductNameModified = fRemoveYear(sProductNameModified);   //i.e. Microsoft Word Viewer is installed

                            oOVALInventoryDefinition = oval_model.OVALDEFINITION.Where(o => o.OVALClassEnumerationID == iOVALClassEnumerationInventoryID && o.OSFamilyID == iOSFamilyWindowsID && o.OVALDefinitionTitle.ToLower().Contains(sProductNameModified) && o.OVALDefinitionTitle.ToLower().Contains(" installed") && o.deprecated != true).OrderByDescending(o => o.OVALDefinitionVersion).FirstOrDefault(); //Hardcoded
                            if (oOVALInventoryDefinition != null)
                            {
                                Console.WriteLine("WARNING: OVALDefInventory Best Match3: " + oOVALInventoryDefinition.OVALDefinitionTitle);
                                //break;
                            }
                            else
                            {
                                //Console.WriteLine("ERROR: OVALDefInventory not retrieved for sOVALDefInventoryTitle=" + sOVALDefInventoryTitle);
                            }
                        }
                        else
                        {
                            Console.WriteLine("ERROR: OVALDefInventory not retrieved");// for sOVALDefInventoryTitle=" + sOVALDefInventoryTitle);
                            if (!lOVALDefInventoryNotRetrieved.Contains(sProductNameInput))
                            {
                                Console.WriteLine("DEBUG Adding " + sProductNameInput + " to lOVALDefInventoryNotRetrieved");
                                lOVALDefInventoryNotRetrieved.Add(sProductNameInput);
                            }
                        }
                    }
                }
            }
            //}
            //}
            
            return oOVALInventoryDefinition;
        }

        public static int fXORCISMAddPatch(string sKBnumber, string sMSCatalogSecurityUpdateText, string sMSCatalogFileName, int iPatchID = 0)
        {
            #region addpatchxorcism
            //int iPatchID = 0;
            try
            {
                if(iPatchID==0) iPatchID = model.PATCH.Where(o => o.PatchVocabularyID == sKBnumber).FirstOrDefault().PatchID;
            }
            catch (Exception ex)
            {

            }
            if (iPatchID <= 0)
            {
                try
                {
                    Console.WriteLine("DEBUG Adding new PATCH");
                    PATCH oPatch = new PATCH();
                    oPatch.CreatedDate = DateTimeOffset.Now;
                    oPatch.PatchVocabularyID = sKBnumber;
                    //oPatch.VocabularyID=  //Microsoft?
                    oPatch.PatchTitle = sMSCatalogSecurityUpdateText;
                    model.PATCH.Add(oPatch);
                    model.SaveChanges();
                    bPatchJustAdded = true;
                }
                catch (Exception exAddingPATCH)
                {
                    Console.WriteLine("Exception: exAddingPATCH " + exAddingPATCH.Message + " " + exAddingPATCH.InnerException);
                }
            }

            int iFileID = 0;
            try
            {
                iFileID = model.FILE.Where(o => o.FileName == sMSCatalogFileName).FirstOrDefault().FileID;
            }
            catch (Exception ex)
            {

            }
            if (iFileID <= 0)
            {
                Console.WriteLine("DEBUG Adding KBFILE " + sMSCatalogFileName);
                try
                {
                    FILE oFile = new FILE();
                    oFile.CreatedDate = DateTimeOffset.Now;
                    oFile.FileName = sMSCatalogFileName;    //Original (MS site) KB Filename    or  Local KB Filename
                                                            //oFile.VocabularyID=   //Microsoft?
                    model.FILE.Add(oFile);
                    model.SaveChanges();
                    iFileID = oFile.FileID;
                }
                catch (Exception exAddKBFILE)
                {
                    Console.WriteLine("Exception: exAddKBFILE " + exAddKBFILE.Message + " " + exAddKBFILE.InnerException);
                }

            }
            //TODO: FILEVERSION
            if (iPatchID > 0 && iFileID > 0)
            {
                int iPatchFileID = 0;
                try
                {
                    iPatchFileID = model.PATCHFILE.Where(o => o.PatchID == iPatchID && o.FileID == iFileID).FirstOrDefault().PatchFileID;
                }
                catch (Exception ex)
                {

                }
                if (iPatchFileID <= 0)
                {
                    try
                    {
                        PATCHFILE oPatchFile = new PATCHFILE();
                        oPatchFile.CreatedDate = DateTimeOffset.Now;
                        oPatchFile.PatchID = iPatchID;
                        oPatchFile.FileID = iFileID;
                        //oPatchFile.VocabularyID=
                        //oPatchFile.PlatformID=    //TODO
                        //oPatchFile=   //TODO
                        model.PATCHFILE.Add(oPatchFile);
                        model.SaveChanges();
                    }
                    catch (Exception exAddPATCHFILE)
                    {
                        Console.WriteLine("Exception: exAddPATCHFILE " + exAddPATCHFILE.Message + " " + exAddPATCHFILE.InnerException);
                    }
                }
                else
                {
                    //Update PATCHFILE
                }
            }
            #endregion addpatchxorcism
            return iPatchID;
        }

        public static int fXORCISMAddFILE(string sKBFileInfoValue, string sInterestingFileVersion="", string sDateForSorting="")
        {
            int iFileID = 0;
            try
            {
                iFileID = model.FILE.FirstOrDefault(o => o.FileName == sKBFileInfoValue).FileID;
            }
            catch (Exception ex)
            {

            }
            try
            {
                if (iFileID <= 0)
                {
                    Console.WriteLine("DEBUG Adding FILE2 " + sKBFileInfoValue);
                    FILE oFile = new FILE();
                    oFile.CreatedDate = DateTimeOffset.Now;
                    oFile.FileName = sKBFileInfoValue;
                    //oFile.VocabularyID=
                    oFile.timestamp = DateTimeOffset.Now;
                    model.FILE.Add(oFile);
                    model.Entry(oFile).State = EntityState.Added;
                    model.SaveChanges();
                    iFileID = oFile.FileID;
                }
                //TODO
                //FILEVERSION
                /*
                if (sFileNameToSearchReplaced != "")
                {
                    //PRODUCTFILE

                }
                */
            }
            catch (Exception exAddFile)
            {
                Console.WriteLine("Exception: exAddFile " + exAddFile.Message + " " + exAddFile.InnerException);
            }
            if (sInterestingFileVersion != "")
            {
                int iFileVersionID = 0;
                try
                {
                    iFileVersionID = model.FILEVERSION.Where(o => o.FileID == iFileID && o.VersionValue == sInterestingFileVersion).FirstOrDefault().FileVersionID;
                }
                catch(Exception ex)
                {

                }
                if(iFileVersionID<=0)
                {
                    try
                    {
                        FILEVERSION oFileVersion = new FILEVERSION();
                        oFileVersion.CreatedDate = DateTimeOffset.Now;
                        oFileVersion.FileID = iFileID;
                        oFileVersion.VersionValue = sInterestingFileVersion;
                        //oFileVersion.VersionID=
                        //oFileVersion.FileDate?...
                        //oFileVersion.VocabularyID=
                        //Confidence
                        oFileVersion.timestamp = DateTimeOffset.Now;
                        model.FILEVERSION.Add(oFileVersion);
                        model.Entry(oFileVersion).State = EntityState.Added;
                        model.SaveChanges();
                        //iFileVersionID = oFileVersion.FileVersionID;
                    }
                    catch(Exception exAddFILEVERSION)
                    {
                        Console.WriteLine("Exception: exAddFILEVERSION " + exAddFILEVERSION.Message + " " + exAddFILEVERSION.InnerException);
                    }

                    //TODO  PRODUCTFILEVERSION
                }
            }
            return iFileID;
        }


        public static string fGetDateForSorting(string sKBFileInfoValue)
        {
            string sDateForSorting = "";
            if (sKBFileInfoValue.Trim() == "")
            {
                //File date not specified by Microsoft
                //sDateForSorting = "1970-01-01"; //Hardcoded default
                sDateForSorting = "19700101"; //Hardcoded default
            }
            else
            {
                try
                {
                    //TODO...!
                    string[] DateSplit = sKBFileInfoValue.ToLower().Split('-');   //16-nov-2016

                    if (DateSplit[2].Length == 2)
                    {
                        sDateForSorting = "20"+DateSplit[2];  //Year    //Hardcoded
                    }
                    else
                    {
                        //DateSplit[2].Length == 4
                        sDateForSorting = DateSplit[2];  //Year
                    }

                    switch (DateSplit[1])    //Month
                    {
                        case "dec":
                            sDateForSorting = sDateForSorting + "12";
                            break;
                        case "nov":
                            sDateForSorting = sDateForSorting + "11";
                            break;
                        case "oct":
                            sDateForSorting = sDateForSorting + "10";
                            break;
                        case "sep":
                            sDateForSorting = sDateForSorting + "09";
                            break;
                        case "aug":
                            sDateForSorting = sDateForSorting + "08";
                            break;
                        case "jul":
                            sDateForSorting = sDateForSorting + "07";
                            break;
                        case "jun":
                            sDateForSorting = sDateForSorting + "06";
                            break;
                        case "may":
                            sDateForSorting = sDateForSorting + "05";
                            break;
                        case "apr":
                            sDateForSorting = sDateForSorting + "04";
                            break;
                        case "mar":
                            sDateForSorting = sDateForSorting + "03";
                            break;
                        case "feb":
                            sDateForSorting = sDateForSorting + "02";
                            break;
                        case "jan":
                            sDateForSorting = sDateForSorting + "01";
                            break;
                        default:
                            Console.WriteLine("ERROR: fGetDateForSorting sKBFileInfoValue=" + sKBFileInfoValue + " DateSplit[1]=" + DateSplit[1]);
                            break;
                    }
                    if (DateSplit[0].Length == 1)
                    {
                        sDateForSorting += "0"+DateSplit[0];    //Day
                    }
                    else
                    {
                        sDateForSorting += DateSplit[0];    //Day
                    }
                }
                catch(Exception exDateForSorting)
                {
                    Console.WriteLine("Exception: exDateForSorting sKBFileInfoValue=" + sKBFileInfoValue + " "+exDateForSorting.Message + " " + exDateForSorting.InnerException);
                }
            }
            return sDateForSorting; //yyyyMMdd
        }


        public static string StripTagsRegexCompiled(string source)
        {
            return _htmlRegex.Replace(source, string.Empty);
        }


        public static string GetSafeFilename(string filename)
        {
            return string.Join("_", filename.Split(Path.GetInvalidFileNameChars()));
        }


        static async void fMakeMSRCRequest(string sCVE) //sCVEID)   //TODO
        {
            //*** WORK IN PROGRESS! ... ***

            try
            {

                /*
                var client = new HttpClient();
                var queryString = HttpUtility.ParseQueryString(string.Empty);

                // Request headers
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));//ACCEPT header

                client.DefaultRequestHeaders.Add("api-key", sMSUpdateAPIKey);

                // Request parameters
                queryString["api-version"] = "2016-08-01";  //HARDCODEDMS
                var uri = "https://api.msrc.microsoft.com/cvrf/" + sCVEID + "?" + queryString;
                Console.WriteLine("DEBUG MSRCRequest to " + uri);

                var response = await client.GetAsync(uri);
                System.IO.File.WriteAllText(sCVEID + "-CVRF.xml", response.ToString());
                */


                #region getMSRCAffectedSoftwares
                try
                {
                    //string sMSRCOutputFile = "MSRCAffectedSoftwares" + DateTime.Now.Year + "-" + DateTime.Now.ToString("MMM") + ".txt";   //Hardcoded
                    string sMSRCOutputFile = "MSRCAffectedSoftwares2017-May.txt";
                    FileInfo fileInfo = new FileInfo(Directory.GetCurrentDirectory() + "\\" + sMSRCOutputFile);
                    if (!fileInfo.Exists)   //NOTE: (IMPORTANT) This will prevent updates
                    {
                        //Use Powershell https://github.com/Microsoft/MSRC-Microsoft-Security-Updates-API/
                        fStartProcess("powershell.exe", Directory.GetCurrentDirectory() + "\\GetMSRCAffectedSoftwares.ps1");
                    }
                    //Parse the output file
                    //            FullProductName: Windows 7 for 32-bit Systems Service Pack 1
                    //            KBArticle       : 4019264
                    //            CVE             : CVE-2017-0190
                    //            Severity        : Important
                    //            Impact          : Information Disclosure
                    //            RestartRequired : Yes
                    //            Supercedence    : 4015549
                    //            CvssScoreSet    : @{ base = 4.20; temporal = 3.80; vector = CVSS:3.0 / AV:L / AC:L / PR:H / UI:R / S:U / C:H / I:N / A:N / E:P / RL:O / RC:C}

                    List<string> lProductsMicrosoft = new List<string>();   //products names compatible with Microsoft MS- KB pages
                    List<string> lKBNumbers = new List<string>();
                    string sFullProductName = string.Empty;
                    string sKBArticle = string.Empty;
                    string sMSCVE = string.Empty;
                    string[] LineSplit = null;

                    #region readmsrcoutput
                    StreamReader srStreamReader = new StreamReader(sMSRCOutputFile);
                    string sLine = srStreamReader.ReadLine();
                    while (sLine != null)// && !bStop)
                    {
                        if (sLine.StartsWith("FullProductName"))
                        {
                            LineSplit = sLine.Split(new string[] { ":" }, StringSplitOptions.None);
                            sFullProductName = LineSplit[1].Trim();
                            //Console.WriteLine("DEBUG FullProductName=" + sFullProductName);
                            //    if (!lProductsMicrosoft.Contains(sFullProductName)) lProductsMicrosoft.Add(sFullProductName);
                            //Reinit
                            sKBArticle = string.Empty;
                            sMSCVE = string.Empty;

                        }
                        else
                        {
                            if (sLine.StartsWith("KBArticle"))
                            {
                                LineSplit = sLine.Split(new string[] { ":" }, StringSplitOptions.None);
                                sKBArticle = LineSplit[1].Trim();   //{3191863, 3191881}
                                                                    //Console.WriteLine("DEBUG sKBArticle=" + sKBArticle);
                            }
                            else
                            {
                                if (sLine.StartsWith("CVE"))
                                {
                                    LineSplit = sLine.Split(new string[] { ":" }, StringSplitOptions.None);
                                    sMSCVE = LineSplit[1].Trim();
                                    //Console.WriteLine("DEBUG sMSCVE=" + sMSCVE);
                                    if (sCVE != "" && sMSCVE == sCVE)
                                    {
                                        if (!lProductsMicrosoft.Contains(sFullProductName)) lProductsMicrosoft.Add(sFullProductName);
                                        if (!lMainProductsNames.Contains(sFullProductName)) lMainProductsNames.Add(sFullProductName);
                                        if (!lKBNumbers.Contains(sKBArticle)) lKBNumbers.Add(sKBArticle);

                                    }
                                }
                            }
                        }

                        sLine = srStreamReader.ReadLine();
                    }
                    srStreamReader.Close();
                    #endregion readmsrcoutput

                    foreach (string sProductNameMS in lProductsMicrosoft.OrderBy(o => o))
                    {
                        Console.WriteLine("DEBUG sProductNameMS=" + sProductNameMS); //Windows Server 2008 R2 for x64-based Systems Service Pack 1 (Server Core installation)

                        #region MSProduct2OVAL
                        //!!!   Windows Defender on Windows Server 2008 R2 for Itanium-Based Systems Service Pack 1
                        string[] ProductNameSplit = new string[] { "" };
                        try
                        {
                            if (sProductNameMS.ToLower().Contains(" on "))
                            {
                                //split
                                ProductNameSplit = sProductNameMS.Split(new string[] { " on " }, StringSplitOptions.None);
                            }
                            else
                            {
                                ProductNameSplit[0] = sProductNameMS.ToLower();
                            }
                        }
                        catch (Exception exProductNameSplit)
                        {
                            Console.WriteLine("Exception: exProductNameSplit " + exProductNameSplit.Message + " " + exProductNameSplit.InnerException);
                        }
                        try
                        {
                            foreach (string sProductName in ProductNameSplit)
                            {
                                string sProductNameOVAL = sProductName.ToLower().Replace("windows rt 8.1", "windows 8.1");
                                if (sProductName.ToLower().Contains("windows 10"))
                                {
                                    if (sProductName.ToLower().Contains("version 1703")) //oval:org.cisecurity:def:2082
                                    {
                                        //Microsoft Windows 10 Version 1703 (x86) is installed
                                        //Microsoft Windows 10 Version 1703 (x64) is installed
                                        sProductNameOVAL = sProductNameOVAL.Replace("for 32-bit systems", "(x86)");
                                        sProductNameOVAL = sProductNameOVAL.Replace("for x64-based systems", "(x64)");
                                    }
                                    else
                                    {
                                        sProductNameOVAL = sProductNameOVAL.Replace("for 32-bit systems", "(32-bit)");      //oval: org.cisecurity:def:380
                                        sProductNameOVAL = sProductNameOVAL.Replace("for x64-based systems", "(64-bit)");   //oval:org.cisecurity:def:377
                                                                                                                            //Microsoft Windows 10 Version 1511 (32-bit) is installed       oval:org.cisecurity:def:379     vs  oval:org.cisecurity:def:699
                                                                                                                            //Microsoft Windows 10 Version 1511 (64-bit) is installed       oval:org.cisecurity:def:378     vs  oval:org.cisecurity:def:698
                                                                                                                            //Microsoft Windows 10 Version 1607 (32-bit) is installed       oval:org.cisecurity:def:1377
                                                                                                                            //Microsoft Windows 10 Version 1607 (64-bit) is installed       oval:org.cisecurity:def:1379
                                    }
                                }
                                else
                                {
                                    if (sProductName.ToLower().Contains("windows 8.1")) //oval:org.mitre.oval:def:18863
                                    {
                                        sProductNameOVAL = sProductNameOVAL.Replace("for 32-bit systems", "(x86)");     //oval:org.mitre.oval:def:20924
                                        sProductNameOVAL = sProductNameOVAL.Replace("for x64-based systems", "(x64)");  //oval:org.mitre.oval:def:20956
                                    }
                                    else
                                    {
                                        if (sProductName.ToLower().Contains("windows 8"))   //oval:org.mitre.oval:def:15732
                                        {
                                            //Note: not preview
                                            sProductNameOVAL = sProductNameOVAL.Replace("for 32-bit systems", "(x86)");     //oval:org.mitre.oval:def:14914
                                            sProductNameOVAL = sProductNameOVAL.Replace("for x64-based systems", "(x64)");  //oval:org.mitre.oval:def:15571
                                        }
                                        else
                                        {
                                            if (sProductName.ToLower().Contains("windows server 2016"))
                                            {
                                                //Note: remove (Server Core installation)
                                                sProductNameOVAL = "windows server 2016";  //oval:org.cisecurity:def:1269
                                            }
                                            else
                                            {
                                                if (sProductName.ToLower().Contains("windows server 2012 r2"))
                                                {
                                                    //Note: remove (Server Core installation)
                                                    sProductNameOVAL = "windows server 2012 r2";  //oval:org.mitre.oval:def:18858
                                                }
                                                else
                                                {
                                                    if (sProductName.ToLower().Contains("windows server 2012"))
                                                    {
                                                        //Note: remove (Server Core installation)
                                                        sProductNameOVAL = "windows server 2012";  //oval:org.mitre.oval:def:16359  vs  oval:org.mitre.oval:def:15585   (64-bit)
                                                    }
                                                    else
                                                    {
                                                        if (sProductName.ToLower().Contains("windows 7"))
                                                        {
                                                            sProductNameOVAL = sProductNameOVAL.Replace("for 32-bit systems", "(32-bit)");
                                                            sProductNameOVAL = sProductNameOVAL.Replace("for x64-based systems", "x64");
                                                        }
                                                        else
                                                        {
                                                            if (sProductName.ToLower().Contains("windows vista"))
                                                            {
                                                                sProductNameOVAL = sProductNameOVAL.Replace("for 32-bit systems", "(32-bit)");
                                                                sProductNameOVAL = sProductNameOVAL.Replace("for x64-based systems", "x64 edition");
                                                            }
                                                            else
                                                            {
                                                                //Windows Server 2008 for Itanium-Based Systems Service Pack 2
                                                                if (sProductName.ToLower().Contains("windows 2008"))
                                                                {
                                                                    //Windows Server 2008 for 32-bit Systems Service Pack 2 (Server Core installation)
                                                                    sProductNameOVAL = sProductNameOVAL.Replace("for 32-bit systems", "(32-bit)");
                                                                    sProductNameOVAL = sProductNameOVAL.Replace("for x64-based systems", "x64");
                                                                    sProductNameOVAL = sProductNameOVAL.Replace("for itanium-based systems", "itanium-based edition");

                                                                }
                                                                else
                                                                {
                                                                    Console.WriteLine("NOTE: INCOMPLETE-HARDCODING");
                                                                    //office, word, sharepoint, skype, etc.

                                                                    //By Default
                                                                    sProductNameOVAL = sProductNameOVAL.Replace("(32-bit editions)", "x86");
                                                                    sProductNameOVAL = sProductNameOVAL.Replace("(32-bit edition)", "x86");
                                                                    sProductNameOVAL = sProductNameOVAL.Replace("(32-bit)", "x86");
                                                                    sProductNameOVAL = sProductNameOVAL.Replace("(64-bit editions)", "x64");
                                                                    sProductNameOVAL = sProductNameOVAL.Replace("(64-bit edition)", "x64");
                                                                    sProductNameOVAL = sProductNameOVAL.Replace("(64-bit)", "x64");

                                                                    sProductNameOVAL = sProductNameOVAL.Replace("service pack ", "sp");

                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                Console.WriteLine("DEBUG sProductNameOVAL=" + sProductNameOVAL);
                                
                                //NOTE: Here we can generate the CPEs
                                if (!lProductsMicrosoft.Contains(sProductNameOVAL)) lProductsMicrosoft.Add(sProductNameOVAL);
                                //TODO: lFileNamesToSearch
                            }
                        }
                        catch (Exception exProductNameSplitLoop)
                        {
                            Console.WriteLine("Exception: exProductNameSplitLoop " + exProductNameSplitLoop.Message + " " + exProductNameSplitLoop.InnerException);
                        }
                        #endregion MSProduct2OVAL
                    }
                    foreach (string sKBNumber in lKBNumbers.OrderBy(o => o))
                    {
                        Console.WriteLine("DEBUG sKBNumber=" + sKBArticle);
                        //TODO: lKBNumbersToSearch
                    }
                }
                catch (Exception exgetMSRCAffectedSoftwares)
                {
                    Console.WriteLine("Exception: exgetMSRCAffectedSoftwares " + exgetMSRCAffectedSoftwares.Message + " " + exgetMSRCAffectedSoftwares.InnerException);
                }
                #endregion getMSRCAffectedSoftwares


            }
            catch (Exception exMakeMSRCRequest)
            {
                Console.WriteLine("Exception: exMakeMSRCRequest " + exMakeMSRCRequest.Message + " " + exMakeMSRCRequest.InnerException);
            }

        }


        public static string fGetSafeFilename(string filename)
        {
            return string.Join("", filename.Split(Path.GetInvalidFileNameChars()));
        }


        public static string fBluePill(string sKBnumber, string sFileNameToSearchReplaced, string sProductName)
        {
            Console.WriteLine("DEBUG "+DateTimeOffset.Now+" BluePillLoad");
            if (sKBnumber.Length < 5)
            {
                Console.WriteLine("NOTE: BADBlueKBNumber " + sKBnumber);
                return "";
            }
            #region bluepill
            //Visit the MS Catalog / Download the MSKBfiles / Extract them (with filters) / Search for the file we are looking for

            string sFileNameToSearchReplacedExtension = Path.GetExtension(sFileNameToSearchReplaced);
            string sFileInfoNeededFound = string.Empty;
            string sProductNameReduced = sProductName.ToLower().Replace("microsoft ", "").Replace(" for itanium-based systems", " itanium-based edition").Replace("x86", "").Replace("x64", "").Trim();  //Hardcoded    //i.e. windows vista x86
            //ia64

            var driver = new ChromeDriver();    //Selenium
            //var driver = new InternetExplorerDriver();
            //driver.Manage().Timeouts().ImplicitWait=TimeSpan.FromSeconds(120);
            //driver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(120));
            //driver.Manage().Timeouts().SetScriptTimeout(TimeSpan.FromSeconds(120));
            lVisitedURLs.Add("https://www.catalog.update.microsoft.com/Search.aspx?q=" + sKBnumber);

            string baseWindowHandle = string.Empty;
            bool bLoadedProperly = false;
            string sResponse = string.Empty;
            while (!bLoadedProperly)
            {
                driver.Navigate().GoToUrl("https://www.catalog.update.microsoft.com/Search.aspx?q=" + sKBnumber);    //HARDCODEDMS
                Thread.Sleep(3000 + iSleepMore); //Hardcoded
                baseWindowHandle = driver.CurrentWindowHandle;

                //Save the Page Locally
                string sKBCatalogFilePath = sCurrentPath + @"\MS\KBCAT" + sKBnumber + ".txt";   //HARDCODED Path
                try
                {
                    sResponse = HttpUtility.HtmlDecode(driver.PageSource).Replace("\u00A0", " ");
                    System.IO.File.WriteAllText(sKBCatalogFilePath, sResponse);   //Hardcoded &nbsp;
                    if (!sResponse.Contains("8DDD0010"))    //The website has encountered a problem
                    {
                        bLoadedProperly = true;
                    }
                }
                catch (Exception exBluePillWriteMSCatalogPage)
                {
                    Console.WriteLine("Exception: exBluePillWriteMSCatalogPage " + exBluePillWriteMSCatalogPage.Message + " " + exBluePillWriteMSCatalogPage.InnerException);
                }

            }

            //For saving bandwith and local space
            bool bOnlyDelta = false;
            if(bDownloadOnlyDeltaUpdates)
            {
                int iDeltas = Regex.Matches(sResponse, "Delta").Count;  //Delta Update      //HardcodedMS
                if (iDeltas>0 && iDeltas== Regex.Matches(sResponse, "Cumulative").Count)    //Cumulative Update     //Review
                {
                    Console.WriteLine("DEBUG OnlyDelta");
                    bOnlyDelta = true;
                }
            }

            //IWebElement element = driver.FindElementByClassName("flatLightBlueButton");//.Click();

            //NOTE: (IMPORTANT) We will download and extract ALL the MSKBfiles from the MS Catalog Results page for the KBnumber (we will filter with sProductName to return just the one FileInfo we are looking for)
            foreach (IWebElement element in driver.FindElementsByClassName("flatLightBlueButton"))  //HARDCODEDMS Download Button
            {
                PopupWindowFinder finder = new PopupWindowFinder(driver);
                string popupWindowHandle = finder.Click(element);
                //driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(120);
                driver.SwitchTo().Window(popupWindowHandle);    //Switch to popup

                bLoadedProperly = false;
                while (!bLoadedProperly)
                {
                    Thread.Sleep(2000 + iSleepMore);
                    if (driver.PageSource.Contains("8DDD0010")) //HardcodedMS   //8DDD0010  The website has encountered a problem
                    {
                        //Reload the popup
                        Console.WriteLine("DEBUG Reload PopUp");
                        driver.Close();
                        driver.SwitchTo().Window(baseWindowHandle);

                        finder = new PopupWindowFinder(driver);
                        popupWindowHandle = finder.Click(element);
                        driver.SwitchTo().Window(popupWindowHandle);    //Switch to popup
                        Thread.Sleep(2000 + iSleepMore);
                    }
                    else
                    {
                        bLoadedProperly = true;
                    }
                }

                try
                {
                    Console.WriteLine("DEBUG " + DateTimeOffset.Now+" PopUp");
                    Thread.Sleep(1000 + iSleepMore);  //Hardcoded
                    string sMSCatalogSecurityUpdateText = "";
                    string sMainProductNameCatalog = "";

                    //IWebElement webElement = driver.FindElementByClassName("textTopTitlePadding textBold textSubHeadingColor"); //HARDCODED //driver.FindElementByTagName("div");
                    for (int i=1; i<5; i ++)
                    {
                        if (driver.FindElementsById("downloadFiles").Count() == 0)
                        {
                            Console.WriteLine("DEBUG Sleep");
                            Thread.Sleep(5000 + iSleepMore);
                        }
                    }

                    bool bSkipNotDelta = false;
                    foreach (IWebElement webelementDiv in driver.FindElementsById("downloadFiles")) //HARDCODEDMS   //Expect 1  //driver.FindElementsByTagName("div"))
                    {
                        string sMSCatalogDownloadDiv = "";
                        try
                        {
                            sMSCatalogDownloadDiv = webelementDiv.GetAttribute("innerHTML");
                        }
                        catch(Exception ex)
                        {
                            //stale element reference: element is not attached to the page document
                        }
                        if (sMSCatalogDownloadDiv=="")
                        {
                            Console.WriteLine("DEBUG Sleep2");
                            Thread.Sleep(5000 + iSleepMore);
                            try
                            {
                                sMSCatalogDownloadDiv = webelementDiv.GetAttribute("innerHTML");
                            }
                            catch (Exception ex)
                            {
                                //stale element reference: element is not attached to the page document
                            }
                            if (sMSCatalogDownloadDiv == "")
                            {
                                Console.WriteLine("DEBUG Sleep3");
                                Thread.Sleep(5000 + iSleepMore);
                                try
                                {
                                    sMSCatalogDownloadDiv = webelementDiv.GetAttribute("innerHTML");
                                }
                                catch (Exception ex)
                                {
                                    //stale element reference: element is not attached to the page document
                                }
                            }
                        }
                        //TODO? Feature packs   Drivers
                        if (sMSCatalogDownloadDiv.Contains("Security") || sMSCatalogDownloadDiv.Contains("Quality") || sMSCatalogDownloadDiv.Contains("Delta") || !sMSCatalogDownloadDiv.Contains("farm-deployment")) //HARDCODEDMS   i.e. Security Only Quality Update   /   Security Monthly Quality Rollup / Security Update for
                        {
                            
                            Console.WriteLine("DEBUG sMSCatalogDownloadDiv=" + sMSCatalogDownloadDiv);
                            if(bOnlyDelta && !sMSCatalogDownloadDiv.Contains("Delta"))
                            {
                                Console.WriteLine("DEBUG Skip Not Delta");
                                bSkipNotDelta = true;
                                continue;
                            }
                            

                            /*
                            //DEBUG <div class="textTopTitlePadding textBold textSubHeadingColor">Security Update for Lync 2010 Attendee - Administrator level installation (KB3188400)</div><
                            div><a title="attendeeadmin_673fbeec3476cc5abf80c1c03c668889ae8282ba.cab" href="
                            http://download.windowsupdate.com/c/msdownload/update/software/secu/2016/09/atte
                            ndeeadmin_673fbeec3476cc5abf80c1c03c668889ae8282ba.cab">attendeeadmin_673fbeec34
                            76cc5abf80c1c03c668889ae8282ba.cab</a></div>
                            DEBUG Security Update for Lync 2010 Attendee - Administrator level installation
                            (KB3188400)
                            */
                            Regex myRegexMSCatalogSecurityUpdateText = new Regex("textSubHeadingColor\">(.*?)</div>");  //HARDCODEDMS
                            sMSCatalogSecurityUpdateText = myRegexMSCatalogSecurityUpdateText.Match(sMSCatalogDownloadDiv).ToString();
                            sMSCatalogSecurityUpdateText = sMSCatalogSecurityUpdateText.Replace("textSubHeadingColor\">", "").Replace("</div>", "").Replace(",", "").Replace("(", "").Replace(")", "");
                            //sMSCatalogSecurityUpdateText = fGetSafeFilename(sMSCatalogSecurityUpdateText.Replace("KB"+sKBnumber, "").Trim());   //Hardcoded
                            sMSCatalogSecurityUpdateText = fGetSafeFilename(sMSCatalogSecurityUpdateText.Replace(sKBnumber, "").Trim());

                            if (sMSCatalogSecurityUpdateText == "")
                            {
                                //Wait and retry
                                Thread.Sleep(5000 + iSleepMore);  //Hardcoded
                                sMSCatalogDownloadDiv = webelementDiv.GetAttribute("innerHTML");
                                sMSCatalogSecurityUpdateText = myRegexMSCatalogSecurityUpdateText.Match(sMSCatalogDownloadDiv).ToString();
                                sMSCatalogSecurityUpdateText = sMSCatalogSecurityUpdateText.Replace("textSubHeadingColor\">", "").Replace("</div>", "").Replace(",", "").Replace("(", "").Replace(")", "");
                                //sMSCatalogSecurityUpdateText = fGetSafeFilename(sMSCatalogSecurityUpdateText.Replace("KB" + sKBnumber, "").Trim());   //Hardcoded
                                sMSCatalogSecurityUpdateText = fGetSafeFilename(sMSCatalogSecurityUpdateText.Replace(sKBnumber, "").Trim());

                                if (sMSCatalogSecurityUpdateText == "")
                                {
                                    Console.WriteLine("sMSCatalogDownloadDiv=" + sMSCatalogDownloadDiv);
                                    if (sMSCatalogDownloadDiv.Contains("farm-deployment")) continue;
                                    if (sMSCatalogDownloadDiv.Contains("Windows 10 Version 1511"))
                                    {
                                        //Cumulative Update for Windows 10 Version 1511 for x64-based Systems (KB4013198)
                                        sMSCatalogSecurityUpdateText = "Windows 10 Version 1511";
                                    }
                                    if (sMSCatalogDownloadDiv.Contains("Windows 10 Version 1607"))
                                    {
                                        sMSCatalogSecurityUpdateText = "Windows 10 Version 1607";
                                    }
                                    if (sMSCatalogDownloadDiv.Contains("Windows 10 Version 1703"))
                                    {
                                        sMSCatalogSecurityUpdateText = "Windows 10 Version 1703";
                                    }
                                    if (sMSCatalogDownloadDiv.Contains("Windows Server 2016"))
                                    {
                                        sMSCatalogSecurityUpdateText = "Windows Server 2016";
                                    }
                                }
                            }
                            Console.WriteLine("DEBUG sMSCatalogSecurityUpdateText=" + sMSCatalogSecurityUpdateText);
                            //Security Update for Windows Vista for x64-based Systems
                            //March 2017 Security Only Quality Update for Windows Embedded 8 Standard
                            //March 2017 Security Monthly Quality Rollup for Windows 8.1 for x64-based Systems
                            sProductFoundDEADBEEFGlobal = "";   //Review

                            foreach (string sMSProductName in lProductsMicrosoft)
                            {
                                //Microsoft Office Excel 2007
                                if (sMSCatalogSecurityUpdateText.ToLower().Replace(" embedded standard", "").Replace(" embedded", "").Contains(sMSProductName) || sMSCatalogSecurityUpdateText.ToLower().Contains(sMSProductName.Replace(" Office","")) || (sMSCatalogSecurityUpdateText.ToLower().Contains("wes09") && sMSProductName.ToLower()=="windows xp")) //Known Microsoft Product  //HARDCODEDMS
                                {
                                    if (sMSProductName == "microsoft office" && sMSCatalogSecurityUpdateText.ToLower().Contains("excel")) continue; //because we want Excel
                                    if (sMSProductName == "microsoft office" && sMSCatalogSecurityUpdateText.ToLower().Contains("word")) continue; //because we want Word
                                    if (sProductFoundDEADBEEFGlobal.Length > sMSProductName.Length + 8) continue;   //DEADBEEF  we keep the longest (previous) one


                                    //Security Update for WES09 and POSReady 2009       KB3216916-WES09_and_POSReady_2009-windowsxp-kb3216916-x86-embedded-fra.exe
                                    //sMainProductName = sMSProductName;  //TODO REVIEW

                                    sMainProductNameCatalog = sMSProductName;
                                    #region fixMainProductNameCatalog
                                    if (sMSCatalogSecurityUpdateText.ToLower().Contains("windows server 2008 r2") && sMSProductName == "windows server 2008") sMainProductNameCatalog = "windows server 2008 r2";

                                    //TODO? fGetProductToAdd()
                                    if (sMSCatalogSecurityUpdateText.Contains(" x64") && !sMSProductName.Contains(" x64"))
                                    {
                                        if (sMSProductName == "windows server 2008" || sMSProductName == "windows server 2012" || sMSProductName.Contains("windows 10"))
                                        {
                                            if (sMainProductNameCatalog == "windows server 2008 r2")
                                            {
                                                sMainProductNameCatalog = "windows server 2008 r2 x64";
                                            }
                                            else
                                            {
                                                //sMainProductNameCatalog = sMSProductName + " (64-bit)";   //HARDCODEDOVAL
                                                if (sMSProductName.Contains("windows 10 version 1703"))
                                                {
                                                    if (!sMainProductNameCatalog.Contains("(x64)")) sMainProductNameCatalog = sMainProductNameCatalog + " (x64)";   //HARDCODEDOVAL
                                                }
                                                else
                                                {
                                                    if (!sMainProductNameCatalog.Contains("(64-bit)")) sMainProductNameCatalog = sMainProductNameCatalog + " (64-bit)";   //HARDCODEDOVAL
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (sMSProductName == "windows 8.1")
                                            {
                                                sMainProductNameCatalog = sMSProductName + " (x64)";
                                            }
                                            else
                                            {
                                                //sMainProductNameCatalog = sMSProductName + " x64";
                                                sMainProductNameCatalog = sMainProductNameCatalog + " x64";

                                            }
                                        }
                                    }
                                    if (sMSCatalogSecurityUpdateText.Contains(" x86") && !sMSProductName.Contains(" x86"))
                                    {
                                        if (sMSProductName == "windows vista" || sMSProductName == "windows server 2008" || sMSProductName == "windows 7" || sMSProductName.Contains("windows 10"))
                                        {
                                            //sMainProductNameCatalog = sMSProductName + " (32-bit)";   //HARDCODEDOVAL
                                            if(!sMainProductNameCatalog.Contains("(32-bit)")) sMainProductNameCatalog = sMainProductNameCatalog + " (32-bit)";   //HARDCODEDOVAL

                                        }
                                        else
                                        {
                                            if (sMSProductName == "windows 8.1")
                                            {
                                                sMainProductNameCatalog = sMSProductName + " (x86)";
                                            }
                                            else
                                            {
                                                //sMainProductNameCatalog = sMSProductName + " x86";
                                                sMainProductNameCatalog = sMainProductNameCatalog + " x86";
                                            }
                                        }
                                    }

                                    if (sMSCatalogSecurityUpdateText.ToLower().Contains(" itanium") && !sMSProductName.Contains(" itanium"))
                                    {
                                        if (sMSProductName == "windows server 2008")
                                        {
                                            if (sMainProductNameCatalog == "windows server 2008 r2")
                                            {
                                                sMainProductNameCatalog = "windows server 2008 r2 itanium";
                                            }
                                            else
                                            {
                                                //sMainProductNameCatalog = sMSProductName + " (ia-64)";   //HARDCODEDOVAL
                                                sMainProductNameCatalog = sMainProductNameCatalog + " (ia-64)";   //HARDCODEDOVAL
                                            }
                                        }
                                        else
                                        {
                                            sMainProductNameCatalog = sMSProductName + " itanium"; //ia64
                                        }
                                    }
                                    #endregion fixMainProductNameCatalog

                                    #region hardcodedfilenamesoffice
                                    //office
                                    if (!lFileNamesToSearch.Contains("msoserver.dll")) lFileNamesToSearch.Add("msoserver.dll"); //Web Apps Server 2013
                                    if (!lFileNamesToSearch.Contains("sword.dll")) lFileNamesToSearch.Add("sword.dll"); //Web Apps Server 2010
                                    if (!lFileNamesToSearch.Contains("xlsrv.dll")) lFileNamesToSearch.Add("xlsrv.dll"); //sharepoint
                                    
                                    if (!lFileNamesToSearch.Contains("wwlib.dll")) lFileNamesToSearch.Add("wwlib.dll");
                                        if (!lFileNamesToSearch.Contains("wwlibcxm.dll")) lFileNamesToSearch.Add("wwlibcxm.dll");

                                    if (!lFileNamesToSearch.Contains("wordcnv.dll")) lFileNamesToSearch.Add("wordcnv.dll"); //compatibility pack

                                    if (sMSProductName.Contains("excel"))
                                    {
                                        if (!lFileNamesToSearch.Contains("xl12cnv.exe")) lFileNamesToSearch.Add("xl12cnv.exe");
                                        if (!lFileNamesToSearch.Contains("excel.exe")) lFileNamesToSearch.Add("excel.exe");

                                    }
                                    if (sMSProductName.Contains("word"))
                                    {
                                        if (!lFileNamesToSearch.Contains("wrd12cnv.dll")) lFileNamesToSearch.Add("wrd12cnv.dll");
                                        if (!lFileNamesToSearch.Contains("winword.exe")) lFileNamesToSearch.Add("winword.exe");
                                    }
                                    if (!lFileNamesToSearch.Contains("wsssetup.dll")) lFileNamesToSearch.Add("wsssetup.dll"); //sharepoint foundation
                                    #endregion hardcodedfilenamesoffice

                                    //if (sProductFoundDEADBEEFGlobal == "") 
                                    //sProductFoundDEADBEEFGlobal = "DEADBEEF" + sMSProductName;   //Hardcoded
                                    if (sProductFoundDEADBEEFGlobal.Length < sMainProductNameCatalog.Length+8 || sProductFoundDEADBEEFGlobal == "microsoft office")  //HARDCODED
                                    {

                                        Console.WriteLine("DEBUG sMainProductNameCatalog1=" + sMainProductNameCatalog);   //sMainProductName);
                                        //windows 10 version 1607 (64-bit)
                                        sProductFoundDEADBEEFGlobal = "DEADBEEF" + sMainProductNameCatalog;   //Hardcoded
                                    }

                                    string sMainProductNameToTitle = new CultureInfo("en-US").TextInfo.ToTitleCase(sMSProductName);
                                    if (!lMainProductsNames.Contains(sMainProductNameToTitle.ToLower())) lMainProductsNames.Add(sMainProductNameToTitle.ToLower());

                                }
                            }

                            if(sMainProductNameCatalog=="")
                            {
                                //Special case
                                if (sMSCatalogSecurityUpdateText.ToLower().Replace(" embedded", "").Contains("windows 8")) //Hardcoded  windows 8 but not windows 8.1
                                {
                                    //sMainProductName = sMSProductName;  //TODO REVIEW
                                    sMainProductNameCatalog = "windows 8";
                                    if (sMainProductNameCatalog.Contains("x64")) sMainProductNameCatalog = sMainProductNameCatalog + " x64";

                                    Console.WriteLine("DEBUG sMainProductNameCatalog2=" + sMainProductNameCatalog);   //sMainProductName);
                                    //if (sProductFoundDEADBEEFGlobal == "") 
                                    sProductFoundDEADBEEFGlobal = "DEADBEEF" + sMainProductNameCatalog;   //Hardcoded

                                    string sMainProductNameToTitle = new CultureInfo("en-US").TextInfo.ToTitleCase("windows 8");
                                    if (!lMainProductsNames.Contains(sMainProductNameToTitle.ToLower())) lMainProductsNames.Add(sMainProductNameToTitle.ToLower());

                                }
                            }

                            if (sMSCatalogSecurityUpdateText.Contains("Microsoft Office SharePoint Server 2007")) sMainProductNameCatalog = "Microsoft Office SharePoint Server 2007";

                            if (sMSCatalogSecurityUpdateText.Contains("Microsoft Office Excel")) sMainProductNameCatalog = "Microsoft Excel";   //Viewer    2007
                            if (sMSCatalogSecurityUpdateText.Contains("Microsoft Excel 2007")) sMainProductNameCatalog = "Microsoft Excel 2007";
                            if (sMSCatalogSecurityUpdateText.Contains("Microsoft Excel 2010")) sMainProductNameCatalog = "Microsoft Excel 2010";
                            if (sMSCatalogSecurityUpdateText.Contains("Microsoft Excel 2013")) sMainProductNameCatalog = "Microsoft Excel 2013";
                            if (sMSCatalogSecurityUpdateText.Contains("Microsoft Excel 2016")) sMainProductNameCatalog = "Microsoft Excel 2016";
                            if (sMSCatalogSecurityUpdateText.Contains("Microsoft Word 2007")) sMainProductNameCatalog = "Microsoft Word 2007";
                            if (sMSCatalogSecurityUpdateText.Contains("Microsoft Office Word 2007")) sMainProductNameCatalog = "Microsoft Word 2007";

                            if (sMSCatalogSecurityUpdateText.Contains("Microsoft Word 2010")) sMainProductNameCatalog = "Microsoft Word 2010";  //32-Bit Edition
                            if (sMSCatalogSecurityUpdateText.Contains("Microsoft Word 2013")) sMainProductNameCatalog = "Microsoft Word 2013";
                            if (sMSCatalogSecurityUpdateText.Contains("Microsoft Word 2016")) sMainProductNameCatalog = "Microsoft Word 2016";
                            if (sMSCatalogSecurityUpdateText.Contains("Microsoft Office Web Apps 2010")) sMainProductNameCatalog = "Microsoft Office Web Apps 2010";
                            if (sMSCatalogSecurityUpdateText.Contains("Microsoft Office Web Apps 2013")) sMainProductNameCatalog = "Microsoft Web Apps Server 2013";

                            Console.WriteLine("DEBUG sMainProductNameCatalog10=" + sMainProductNameCatalog);   //sMainProductName);
                            
                        }
                    }

                    if (!bSkipNotDelta)
                    {
                        if (sMSCatalogSecurityUpdateText == "")
                        {
                            Console.WriteLine("ERROR: sMSCatalogSecurityUpdateText not retrieved");

                        }

                        try
                        {
                            //TODO: multiple files (multiple locales/languages en-us    x-none)   i.e.    http://www.catalog.update.microsoft.com/Search.aspx?q=KB3085483
                            IWebElement webElement = driver.FindElementByTagName("a");  //We assume we just have 1 link in the MS Catalog DownloadDialog.aspx popup //HARDCODED
                            string sMSCatalogURLFile = webElement.GetAttribute("href").ToString();  //HARDCODED
                            Console.WriteLine("DEBUG sMSCatalogURLFile=" + sMSCatalogURLFile);
                            if (lMSCatalogURLFile.Contains(sMSCatalogURLFile))  //mpsyschk
                            {
                                //Skip
                            }
                            else
                            {
                                //    lMSCatalogURLFile.Add(sMSCatalogURLFile);
                                if (!sMSCatalogURLFile.EndsWith("home.aspx"))    //HARDCODEDMS   http://www.catalog.update.microsoft.com/home.aspx
                                {
                                    //TODO: PATCHREFERENCE

                                    string[] MSCatalogURLFileSplit = sMSCatalogURLFile.Split('/');
                                    string sMSCatalogFileName = MSCatalogURLFileSplit[MSCatalogURLFileSplit.Length - 1];
                                    Console.WriteLine("DEBUG sMSCatalogFileName=" + sMSCatalogFileName);
                                    
                                    #region fixsMainProductNameCatalog
                                    if (sMSCatalogSecurityUpdateText == "")
                                    {
                                        //TODO REVIEW
                                        if (sMSCatalogFileName.Contains("windows10"))
                                        {
                                            sMainProductNameCatalog = "windows 10";
                                            if (sMSCatalogFileName.Contains("-x86"))
                                            {
                                                sMainProductNameCatalog = sMainProductNameCatalog + " (32-bit)";// " x86"; //HARDCODED
                                            }
                                            else
                                            {
                                                if (sMSCatalogFileName.Contains("-x64"))
                                                {
                                                    sMainProductNameCatalog = sMainProductNameCatalog + " (64-bit)";//" x64";
                                                }
                                                else
                                                {
                                                    if (sMSCatalogFileName.Contains("-ia64")) sMainProductNameCatalog = sMainProductNameCatalog + " ia64";
                                                }
                                            }
                                            Console.WriteLine("DEBUG sMainProductNameCatalog3=" + sMainProductNameCatalog);   //sMainProductName);
                                                                                                                              //if (sProductFoundDEADBEEFGlobal == "") 
                                            sProductFoundDEADBEEFGlobal = "DEADBEEF" + sMainProductNameCatalog;   //Hardcoded

                                            string sMainProductNameToTitle = new CultureInfo("en-US").TextInfo.ToTitleCase(sMainProductNameCatalog);
                                            if (!lMainProductsNames.Contains(sMainProductNameToTitle.ToLower())) lMainProductsNames.Add(sMainProductNameToTitle.ToLower());
                                        }

                                        if (sMSCatalogFileName.Contains("windows8.1"))
                                        {
                                            sMainProductNameCatalog = "windows 8.1";
                                            if (sMSCatalogFileName.Contains("-x86"))
                                            {
                                                sMainProductNameCatalog = sMainProductNameCatalog + " (x86)";// " x86"; //HARDCODED
                                            }
                                            else
                                            {
                                                if (sMSCatalogFileName.Contains("-x64")) sMainProductNameCatalog = sMainProductNameCatalog + " (x64)";//" x64";
                                            }                                                                                                     //if (sMSCatalogFileName.Contains("-ia64")) sMainProductNameCatalog = sMainProductNameCatalog + " ia64";

                                            Console.WriteLine("DEBUG sMainProductNameCatalog3=" + sMainProductNameCatalog);   //sMainProductName);
                                                                                                                              //if (sProductFoundDEADBEEFGlobal == "") 
                                            sProductFoundDEADBEEFGlobal = "DEADBEEF" + sMainProductNameCatalog;   //Hardcoded

                                            string sMainProductNameToTitle = new CultureInfo("en-US").TextInfo.ToTitleCase(sMainProductNameCatalog);
                                            if (!lMainProductsNames.Contains(sMainProductNameToTitle.ToLower())) lMainProductsNames.Add(sMainProductNameToTitle.ToLower());
                                        }

                                        //windows8-rt

                                        if (sMSCatalogFileName.Contains("windowsxp"))
                                        {
                                            sMainProductNameCatalog = "windows xp";
                                            if (sMSCatalogFileName.Contains("-x86"))
                                            {
                                                sMainProductNameCatalog = sMainProductNameCatalog + " (x86)";// " x86"; //HARDCODED
                                            }
                                            else
                                            {
                                                if (sMSCatalogFileName.Contains("-x64")) sMainProductNameCatalog = sMainProductNameCatalog + " (x64)";//" x64";
                                            }                                                                                                  //if (sMSCatalogFileName.Contains("-ia64")) sMainProductNameCatalog = sMainProductNameCatalog + " ia64";

                                            Console.WriteLine("DEBUG sMainProductNameCatalog3=" + sMainProductNameCatalog);   //sMainProductName);
                                                                                                                              //if (sProductFoundDEADBEEFGlobal == "") 
                                            sProductFoundDEADBEEFGlobal = "DEADBEEF" + sMainProductNameCatalog;   //Hardcoded

                                            string sMainProductNameToTitle = new CultureInfo("en-US").TextInfo.ToTitleCase(sMainProductNameCatalog);
                                            if (!lMainProductsNames.Contains(sMainProductNameToTitle.ToLower())) lMainProductsNames.Add(sMainProductNameToTitle.ToLower());
                                        }
                                    }
                                    //HARDCODED!!!
                                    

                                    if (sMainProductNameCatalog.Contains("windows 10") && sMSCatalogFileName.Contains("-x86") && !sMainProductNameCatalog.Contains("(32-bit)"))
                                    {
                                        sMainProductNameCatalog = sMainProductNameCatalog + " (32-bit)";
                                    }
                                    else
                                    {
                                        if (sMainProductNameCatalog == "windows 8.1" && sMSCatalogFileName.Contains("-x86"))
                                        {
                                            sMainProductNameCatalog = "windows 8.1 (x86)";
                                        }
                                        else
                                        {
                                            if (sMainProductNameCatalog == "windows 7" && sMSCatalogFileName.Contains("-x86"))
                                            {
                                                sMainProductNameCatalog = "windows 7 (32-bit)";
                                            }
                                            else
                                            {
                                                if (sMainProductNameCatalog == "windows server 2008" && sMSCatalogFileName.Contains("-x86"))
                                                {
                                                    sMainProductNameCatalog = "windows server 2008 (32-bit)";
                                                }
                                                else
                                                {
                                                    if (sMainProductNameCatalog == "windows vista" && sMSCatalogFileName.Contains("-x86"))
                                                    {
                                                        sMainProductNameCatalog = "windows vista (32-bit)";
                                                    }
                                                    else
                                                    {
                                                        if (sMainProductNameCatalog == "windows server 2012")
                                                        {
                                                            if (sMSCatalogSecurityUpdateText.ToLower().Contains("windows server 2012 r2"))
                                                            {
                                                                sMainProductNameCatalog = "windows server 2012 r2";
                                                                if (sMSCatalogFileName.Contains("-x64"))
                                                                {
                                                                    sMainProductNameCatalog = sMainProductNameCatalog + " x64";
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (sMSCatalogFileName.Contains("-x64"))
                                                                {
                                                                    sMainProductNameCatalog = sMainProductNameCatalog + " (64-bit)";
                                                                }
                                                            }

                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        if (sMSCatalogSecurityUpdateText.ToLower().Contains("microsoft office compatibility pack"))
                                        {
                                            sMainProductNameCatalog = "microsoft office compatibility pack";
                                            if (sMSCatalogSecurityUpdateText.ToLower().Contains("service pack 1")) sMainProductNameCatalog = sMainProductNameCatalog + " sp1";
                                            if (sMSCatalogSecurityUpdateText.ToLower().Contains("service pack 2")) sMainProductNameCatalog = sMainProductNameCatalog + " sp2";
                                            if (sMSCatalogSecurityUpdateText.ToLower().Contains("service pack 3")) sMainProductNameCatalog = sMainProductNameCatalog + " sp3";
                                            //TODO
                                        }
                                    }
                                    #endregion fixsMainProductNameCatalog
                                    
                                    Console.WriteLine("DEBUG sMainProductNameCatalog4=" + sMainProductNameCatalog);   //sMainProductName);
                                                                                                                      //if (sProductFoundDEADBEEFGlobal == "") 

                                    //TODO: (HERE?) PATCHPRODUCT

                                    sProductFoundDEADBEEFGlobal = "DEADBEEF" + sMainProductNameCatalog;   //Hardcoded

                                    #region filenametosearchhardcoded
                                    if (sFileNameToSearchReplaced == "" && sMSCatalogFileName.Contains("offowc"))
                                    {
                                        sFileNameToSearchReplaced = "offowc.dll"; //Hardcoded   Microsoft Office XP Web Components
                                        if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                                    }
                                    if (sFileNameToSearchReplaced == "" && (sMSCatalogSecurityUpdateText.Contains("excel viewer") || sMSCatalogFileName.Contains("xlview")))
                                    {
                                        sFileNameToSearchReplaced = "xlview.exe"; //Hardcoded
                                        if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                                    }
                                    if (sFileNameToSearchReplaced == "" && sMSCatalogSecurityUpdateText.Contains("excel services"))
                                    {
                                        sFileNameToSearchReplaced = "xlsrvintl.dll"; //Hardcoded    (good luck with .resx...)
                                        if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                                        sFileNameToSearchReplaced = "xlsrv.dll"; //Hardcoded
                                        if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                                    }
                                    //word automation services
                                    if (sFileNameToSearchReplaced == "" && (sMSCatalogSecurityUpdateText.Contains("excel") || sMSCatalogFileName.Contains("excel")))
                                    {
                                        sFileNameToSearchReplaced = "excel.exe"; //Hardcoded
                                        if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                                    }
                                    if (sFileNameToSearchReplaced == "" && (sMSCatalogSecurityUpdateText.Contains("exchange") || sMSCatalogFileName.Contains("exchange")))
                                    {
                                        sFileNameToSearchReplaced = "Microsoft.Exchange.Clients.Owa.dll"; //Hardcoded
                                        if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                                        sFileNameToSearchReplaced = "Microsoft.Exchange.Clients.Owa2.Server.dll"; //Hardcoded
                                        if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                                        sFileNameToSearchReplaced = "Microsoft.Exchange.Management.ControlPanel.dll"; //Hardcoded
                                        if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                                        sFileNameToSearchReplaced = "Microsoft.Exchange.Monitoring.ActiveMonitoring.Local.Components.dll"; //Hardcoded
                                        if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                                        //vsword.dll
                                        //...
                                        sFileNameToSearchReplaced = "owaauth.dll"; //Hardcoded
                                        if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                                    }
                                    if (sMSCatalogSecurityUpdateText.Contains("powerpoint") || sMSCatalogFileName.Contains("powerpoint"))
                                    {
                                        //if (sFileNameToSearchReplaced == "") sFileNameToSearchReplaced = "ppcore.dll"; //Hardcoded
                                        //if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                                        if (!lFileNamesToSearch.Contains("ppcore.dll")) lFileNamesToSearch.Add("ppcore.dll");
                                        //pptpia.dll
                                        if (sFileNameToSearchReplaced == "") sFileNameToSearchReplaced = "powerpnt.exe"; //Hardcoded
                                        if (!lFileNamesToSearch.Contains("powerpnt.exe")) lFileNamesToSearch.Add("powerpnt.exe");

                                    }

                                    //silverlight

                                    if (sFileNameToSearchReplaced == "" && sMSCatalogSecurityUpdateText.Contains("word viewer"))
                                    {
                                        sFileNameToSearchReplaced = "wordview.exe"; //Hardcoded
                                        if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                                    }
                                    if (sMSCatalogSecurityUpdateText.Contains("word"))
                                    {
                                        sFileNameToSearchReplaced = "winword.exe"; //Hardcoded
                                        if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                                    }
                                    if (sFileNameToSearchReplaced == "" && sMSCatalogSecurityUpdateText.Contains("publisher"))
                                    {
                                        sFileNameToSearchReplaced = "mspub.exe"; //Hardcoded
                                        if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                                    }
                                    if (sFileNameToSearchReplaced == "" && (sMSCatalogSecurityUpdateText.Contains("skype") || sMSCatalogSecurityUpdateText.Contains("lync") || sMSCatalogFileName.Contains("lync")))
                                    {
                                        sFileNameToSearchReplaced = "lync.exe"; //Hardcoded     lync99.exe      lynchtmlconv.exe        lmaddins.dll
                                        if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                                    }
                                    if (sFileNameToSearchReplaced == "" && (sMSCatalogSecurityUpdateText.Contains("access") || sMSCatalogFileName.Contains("access")))
                                    {
                                        sFileNameToSearchReplaced = "msaccess.exe"; //Hardcoded
                                        if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                                    }
                                    if (sFileNameToSearchReplaced == "" && (sMSCatalogSecurityUpdateText.Contains("gdi") || sMSCatalogFileName.Contains("gdiplus")))
                                    {
                                        sFileNameToSearchReplaced = "gdiplus.dll"; //Hardcoded
                                        if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                                    }
                                    if (sFileNameToSearchReplaced == "" && (sMSCatalogSecurityUpdateText.Contains("infopath") || sMSCatalogFileName.Contains("infopath")))
                                    {
                                        sFileNameToSearchReplaced = "infopath.exe"; //Hardcoded
                                        if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                                    }
                                    if (sFileNameToSearchReplaced == "" && (sMSCatalogSecurityUpdateText.Contains("onenote") || sMSCatalogFileName.Contains("onenote")))
                                    {
                                        sFileNameToSearchReplaced = "onenote.exe"; //Hardcoded
                                        if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                                    }
                                    if (sFileNameToSearchReplaced == "" && (sMSCatalogSecurityUpdateText.Contains("outlook") || sMSCatalogFileName.Contains("outlook")))
                                    {
                                        //mapir.dll
                                        sFileNameToSearchReplaced = "outllibr.dll"; //Hardcoded
                                        if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                                        //outlctl.dll
                                        //outlmime.dll
                                        //outlph.dll
                                        //outlrpc.dll
                                        //outlvba.dll
                                        sFileNameToSearchReplaced = "outlook.exe"; //Hardcoded
                                        if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                                    }
                                    if (sMSCatalogSecurityUpdateText.Contains("powerpoint"))
                                    {

                                        sFileNameToSearchReplaced = "powerpnt.exe"; //Hardcoded
                                        if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                                    }
                                    if (sMSCatalogSecurityUpdateText.Contains("project"))
                                    {
                                        sFileNameToSearchReplaced = "winproj.exe"; //Hardcoded
                                        if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                                    }
                                    if (sMSCatalogSecurityUpdateText.Contains("visio") || sMSCatalogFileName.Contains("visio"))
                                    {
                                        sFileNameToSearchReplaced = "visio.exe"; //Hardcoded
                                        if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                                    }
                                    if (sFileNameToSearchReplaced == "" && sMSCatalogSecurityUpdateText.Contains("office compatibility pack"))// || sMSCatalogSecurityUpdateText.Contains("pack de compatibilite")))    //Hardcoded + French
                                    {
                                        //TODO: was it Excel?
                                        //xlconv2007-kb3128022-fullfile-x86-glb.exe
                                        if (sMSCatalogFileName.Contains("xlconv"))
                                        {
                                            sFileNameToSearchReplaced = "xl12cnv.exe"; //Hardcoded
                                            if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);

                                            sFileNameToSearchReplaced = "excelcnv.exe"; //Hardcoded //excelcnv.exe|excelconv.exe
                                            if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                                        }
                                        //wordconv2007-kb3128024-fullfile-x86-glb.exe
                                        if (sMSCatalogFileName.Contains("wordconv"))
                                        {
                                            sFileNameToSearchReplaced = "wrd12cnv.dll"; //Hardcoded
                                            if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);

                                            sFileNameToSearchReplaced = "wordcnv.dll"; //Hardcoded
                                            if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                                        }
                                    }
                                    //Microsoft Office Web Apps 2010 (all versions)
                                    if (sMSCatalogSecurityUpdateText.Contains("web apps"))
                                    {
                                        sFileNameToSearchReplaced = "msoserver.dll"; //Hardcoded
                                                                                     //sword.dll
                                        if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                                        //Wdsrv.conversion.office.msoserver.dll
                                    }
                                    if (sFileNameToSearchReplaced == "" && sMSCatalogSecurityUpdateText.Contains("sharepoint server")) //sharepoint foundation?
                                    {
                                        if (sMSCatalogFileName.Contains("xlsrv"))
                                        {
                                            sFileNameToSearchReplaced = "xlsrv.dll";
                                            if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                                        }
                                        if (sMSCatalogFileName.Contains("wdsrv"))
                                        {
                                            sFileNameToSearchReplaced = "sword.dll";
                                            if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                                        }
                                        if (sMSCatalogFileName.Contains("word automation"))
                                        {
                                            sFileNameToSearchReplaced = "sword.dll";
                                            if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                                        }
                                        //if (sFileNameToSearchReplaced == "" && oVulnerability.VULDescription.ToLower().Contains("word")) sFileNameToSearchReplaced = "sword.dll";
                                        //TODO: if other Products Found contains word/excel
                                        if (sFileNameToSearchReplaced == "")
                                        {
                                            sFileNameToSearchReplaced = "sword.dll";
                                            if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                                            //Wdsrv.conversion.sword.dll
                                        }
                                        //stswel.dll
                                        //wwintl.dll
                                        //vutils.dll
                                    }
                                    if (sFileNameToSearchReplaced == "" && sMSCatalogFileName.Contains("mso"))
                                    {
                                        sFileNameToSearchReplaced = "msointl.dll";
                                        if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                                        sFileNameToSearchReplaced = "msorec.exe";
                                        if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                                        sFileNameToSearchReplaced = "msores.dll";
                                        if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                                        sFileNameToSearchReplaced = "mso.dll";
                                        if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                                    }
                                    if (sFileNameToSearchReplaced == "" && sMSCatalogFileName.Contains("mscomctl"))
                                    {
                                        sFileNameToSearchReplaced = "mscomctl.ocx";
                                        if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                                    }
                                    //oart.dll
                                    //oartconv.dll


                                    //For Word Automation Services on supported editions of Microsoft SharePoint Server 2010 Service Pack 2:
                                    //msoserver.dll     sword.dll
                                    //For Excel Services on supported editions of Microsoft SharePoint Server 2010 Service Pack 2:
                                    //xlsrv.dll

                                    if (sMSCatalogSecurityUpdateText.Contains("hyperlink object library"))
                                    {
                                        sFileNameToSearchReplaced = "hlink.dll";
                                        if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                                    }
                                    if (sMSCatalogFileName.Contains("skype") || sMSCatalogFileName.Contains("lync") || sMSCatalogFileName.Contains("attendee") || sMSCatalogFileName.Contains("ogl"))
                                    {
                                        //ocpptview.dll

                                        sFileNameToSearchReplaced = "ogl.dll";
                                        if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                                        if (!lFileNamesToSearch.Contains("lync.lync.exe")) lFileNamesToSearch.Add("lync.lync.exe");
                                    }

                                    if (sMSCatalogSecurityUpdateText.Contains("office"))
                                    {
                                        if (!lFileNamesToSearch.Contains("wwlibcxm.dll")) lFileNamesToSearch.Add("wwlibcxm.dll");
                                        if (sFileNameToSearchReplaced == "")
                                        {
                                            sFileNameToSearchReplaced = "usp10.dll";
                                        }
                                        if (!lFileNamesToSearch.Contains("usp10.dll")) lFileNamesToSearch.Add("usp10.dll");
                                    }

                                    //internet explorer
                                    //ie8   ie9 ie10    ie11
                                    //edge
                                    #endregion filenametosearchhardcoded
                                    
                                    if (!sKBnumber.ToUpper().StartsWith("KB")) sKBnumber = "KB" + sKBnumber;  //hardcoded

                                    int iPatchID = 0;
                                    try
                                    {
                                        iPatchID = fXORCISMAddPatch(sKBnumber, sMSCatalogSecurityUpdateText, sMSCatalogFileName, 0);
                                    }
                                    catch(Exception exAddPatchMSXORCISM)
                                    {
                                        Console.WriteLine("Exception: exAddPatchMSXORCISM " + exAddPatchMSXORCISM.Message + " " + exAddPatchMSXORCISM.InnerException);
                                    }

                                    #region cleanshortmscatalogfilename
                                    sMSCatalogFileName = sKBnumber + "-" + sMSCatalogSecurityUpdateText + "-" + sMSCatalogFileName.Replace("-" + sKBnumber.ToLower(), "");

                                    //Cleaning  Shorten it a bit  //TODO Review Improve for less than 248/260 characters
                                    sMSCatalogFileName = sMSCatalogFileName.Replace("Security Monthly Quality Rollup for ", "");   //HardcodedMS
                                    sMSCatalogFileName = sMSCatalogFileName.Replace("Security Only Quality Update for ", "");   //HardcodedMS
                                    sMSCatalogFileName = sMSCatalogFileName.Replace("Security and Quality Rollup for ", "");   //HardcodedMS
                                    sMSCatalogFileName = sMSCatalogFileName.Replace("Security Only Update for ", "");   //HardcodedMS
                                    sMSCatalogFileName = sMSCatalogFileName.Replace("Security Update Rollup 10 For ", "");   //HardcodedMS
                                    sMSCatalogFileName = sMSCatalogFileName.Replace("Security Update Rollup 10 for ", "");   //HardcodedMS
                                    sMSCatalogFileName = sMSCatalogFileName.Replace("Security Update Rollup For ", "");   //HardcodedMS
                                    sMSCatalogFileName = sMSCatalogFileName.Replace("Security Update Rollup for ", "");   //HardcodedMS
                                    sMSCatalogFileName = sMSCatalogFileName.Replace("Security Update For ", "");   //HardcodedMS
                                    sMSCatalogFileName = sMSCatalogFileName.Replace("Security Update for ", "");   //HardcodedMS
                                    sMSCatalogFileName = sMSCatalogFileName.Replace("Update For ", "");   //HardcodedMS
                                    sMSCatalogFileName = sMSCatalogFileName.Replace("Update for ", "");   //HardcodedMS
                                    sMSCatalogFileName = sMSCatalogFileName.Replace("Cumulative ", "");   //HardcodedMS
                                    sMSCatalogFileName = sMSCatalogFileName.Replace("Delta ", "");   //HardcodedMS
                                    sMSCatalogFileName = sMSCatalogFileName.Replace("_delta", "");   //HardcodedMS
                                    sMSCatalogFileName = sMSCatalogFileName.Replace(" delta", "");   //HardcodedMS

                                    sMSCatalogFileName = sMSCatalogFileName.Replace(" for x64-based Systems", "");   //HardcodedMS  (x64 in filename)
                                    sMSCatalogFileName = sMSCatalogFileName.Replace(" for x64", "");   //HardcodedMS    (x64 in filename)
                                    sMSCatalogFileName = sMSCatalogFileName.Replace(" based Systems", "");   //HardcodedMS
                                    sMSCatalogFileName = sMSCatalogFileName.Replace("-based Systems", "");   //HardcodedMS
                                    sMSCatalogFileName = sMSCatalogFileName.Replace(" Service Pack ", " SP");   //HardcodedMS
                                    sMSCatalogFileName = sMSCatalogFileName.Replace(" x64 Edition", " x64");   //HardcodedMS
                                    sMSCatalogFileName = sMSCatalogFileName.Replace(" for x86", " x86");   //HardcodedMS

                                    sMSCatalogFileName = sMSCatalogFileName.Replace("-windows6.0", "");   //HardcodedMS
                                    sMSCatalogFileName = sMSCatalogFileName.Replace("-windows6.1", "");   //HardcodedMS
                                    sMSCatalogFileName = sMSCatalogFileName.Replace("-windows8.1", "");   //HardcodedMS
                                    sMSCatalogFileName = sMSCatalogFileName.Replace("-windows8", "");   //HardcodedMS
                                    sMSCatalogFileName = sMSCatalogFileName.Replace("-windows10.0", "");   //HardcodedMS


                                    //TODO  TODO
                                    int iCurrentYear = DateTime.Now.Year;
                                    string[] monthNames = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
                                    //["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
                                    for (int i = 0; i < 10; i++)
                                    {
                                        iCurrentYear -= i;
                                        int iMonth = 1;
                                        foreach (string sMonth in monthNames)
                                        {
                                            //sMSCatalogFileName = sMSCatalogFileName.Replace("April 2017 ", "");
                                            sMSCatalogFileName = sMSCatalogFileName.Replace(sMonth + " " + iCurrentYear + " ", "");
                                            //TODO? KB890830-Windows_Malicious_Software_Removal_Tool_for_Windows_8_8.1_10_and_Windows_Server_2012_2012_R2_2016_x64-February_2017-windows-x64-v5.45-delta
                                            //sMSCatalogFileName = sMSCatalogFileName.Replace("2017-04 ", "");
                                            if (iMonth < 10)
                                            {
                                                sMSCatalogFileName = sMSCatalogFileName.Replace(iCurrentYear + "-0" + iMonth, "");
                                            }
                                            else
                                            {
                                                sMSCatalogFileName = sMSCatalogFileName.Replace(iCurrentYear + "-" + iMonth, "");
                                            }
                                            iMonth++;
                                        }
                                    }
                                    
                                    //_2438d75e9a0f6e38e61f992e70c832bc706a2b27.
                                    //_e585ffbfd1059b9b2c3a1c89da3b316145dc9d36.
                                    //Remove the SHA1 sum (Note that this will help later not redownloading the MSKBfile and finding the FileNameToSearch without bluepill)
                                    Regex myRegexSHA1 = new Regex("_([a-f0-9]{40})"); //Hardcoded
                                    sMSCatalogFileName = myRegexSHA1.Replace(sMSCatalogFileName, "");

                                    sMSCatalogFileName = sMSCatalogFileName.Replace(" - ", "-");
                                    sMSCatalogFileName = sMSCatalogFileName.Replace("--", "-");
                                    sMSCatalogFileName = sMSCatalogFileName.Replace(" ", "_");
                                    sMSCatalogFileName = sMSCatalogFileName.Replace("-_", "-");
                                    sMSCatalogFileName = sMSCatalogFileName.Replace("_-", "-");
                                    sMSCatalogFileName = sMSCatalogFileName.Replace("--", "-");
                                    sMSCatalogFileName = sMSCatalogFileName.Replace("_x86-x86", "-x86");
                                    sMSCatalogFileName = sMSCatalogFileName.Replace("_x64-x64", "-x64");
                                    sMSCatalogFileName = sMSCatalogFileName.Replace("_x86-rt-x86", "-rt-x86");
                                    #endregion cleanshortmscatalogfilename

                                    sMSCatalogFileName = fGetSafeFilename(sMSCatalogFileName);
                                    Console.WriteLine("DEBUG sMSCatalogFileName2=" + sMSCatalogFileName);   //KB3183431-Windows_Vista-x86.msu   (was windows6.0-kb3183431-x86_e7c78348dd1f8e9074266e58b7b603e34cff57b4.msu)

                                    try
                                    {
                                        fXORCISMAddPatch(sKBnumber, sMSCatalogSecurityUpdateText, sMSCatalogFileName, iPatchID);
                                    }
                                    catch (Exception exAddPatchLocalXORCISM)
                                    {
                                        Console.WriteLine("Exception: exAddPatchLocalXORCISM " + exAddPatchLocalXORCISM.Message + " " + exAddPatchLocalXORCISM.InnerException);
                                    }

                                    #region replacefilenametosearch
                                    if (sMSCatalogFileName.Contains("xlconv")) sFileNameToSearchReplaced = "xl12cnv.exe";
                                    if (sMSCatalogFileName.Contains("wordconv"))
                                    {
                                        sFileNameToSearchReplaced = "wrd12cnv.dll";
                                        if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                                        sFileNameToSearchReplaced = "wordcnv.dll"; //Hardcoded
                                        if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                                    }
                                    if (sMSCatalogFileName.Contains("xlview"))
                                    {
                                        sFileNameToSearchReplaced = "xlview.exe";
                                        if (!lFileNamesToSearch.Contains("xlview.exe")) lFileNamesToSearch.Add("xlview.exe");
                                    }
                                    if (sMSCatalogFileName.Contains("wordview"))
                                    {
                                        sFileNameToSearchReplaced = "wordview.exe";
                                        if (!lFileNamesToSearch.Contains("wordview.exe")) lFileNamesToSearch.Add("wordview.exe");
                                    }

                                    //TODO We could have the OS from here
                                    //KB4012583-Windows_Vista-windows6.0-kb4012583-x64.msu

                                    if (sFileNameToSearchReplaced.Trim() == "")
                                    {
                                        if (sMainProductNameCatalog.Contains("excel") || sMSCatalogFileName.Contains("excel"))
                                        {
                                            sFileNameToSearchReplaced = "excel.exe";
                                            if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                                        }
                                        if (sMainProductNameCatalog.Contains("word") || sMSCatalogFileName.Contains("word"))
                                        {
                                            sFileNameToSearchReplaced = "winword.exe";
                                            if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                                        }
                                    }

                                    //TODO We could have the FileNameToSearch here
                                    //i.e. KB3127995-Word_Viewer_KB-usp10.cab
                                    if (sMSCatalogFileName.Contains("usp10"))
                                    {
                                        sFileNameToSearchReplaced = "usp10.dll";
                                        if (!lFileNamesToSearch.Contains("usp10.dll")) lFileNamesToSearch.Add("usp10.dll");
                                    }
                                    if (sMSCatalogFileName.Contains("gdiplus"))
                                    {
                                        sFileNameToSearchReplaced = "gdiplus.dll";
                                        if (!lFileNamesToSearch.Contains("gdiplus.dll")) lFileNamesToSearch.Add("gdiplus.dll");
                                    }
                                    if (sMSCatalogFileName.Contains("xlconv"))
                                    {
                                        sFileNameToSearchReplaced = "xl12cnv.exe";
                                        if (!lFileNamesToSearch.Contains("xl12cnv.exe")) lFileNamesToSearch.Add("xl12cnv.exe");
                                        //XL12CNVP.DLL
                                        //XLCALL32.DLL
                                    }
                                    //xlview
                                    if (sMSCatalogFileName.Contains("xlview"))
                                    {
                                        sFileNameToSearchReplaced = "xlview.exe";
                                        if (!lFileNamesToSearch.Contains("xlview.exe")) lFileNamesToSearch.Add("xlview.exe");
                                    }
                                    if (sMSCatalogFileName.Contains("wordconv"))
                                    {
                                        sFileNameToSearchReplaced = "wrd12cnv.dll";
                                        if (!lFileNamesToSearch.Contains("wrd12cnv.dll")) lFileNamesToSearch.Add("wrd12cnv.dll");
                                    }
                                    if (sMSCatalogFileName.Contains("wordview"))
                                    {
                                        sFileNameToSearchReplaced = "wordview.exe";
                                        if (!lFileNamesToSearch.Contains("wordview.exe")) lFileNamesToSearch.Add("wordview.exe");
                                    }
                                    #endregion replacefilenametosearch

                                    if (sFileNameToSearchGlobal != "")
                                    {
                                        //Force only one filename (specified in argument)
                                        lFileNamesToSearch.Clear();
                                        lFileNamesToSearch.Add(sFileNameToSearchGlobal);
                                    }

                                    Console.WriteLine("DEBUG sFileNameToSearchReplaced2=" + sFileNameToSearchReplaced);


                                    string sMSCatalogFileNameLocalPath = sLocalPathForMSKBfiles + sMSCatalogFileName; //HARDCODED Local Path to save MSCatalogKBfile (.cab, .msp, msi, msp...)
                                    FileInfo fileInfo2 = new FileInfo(sMSCatalogFileNameLocalPath);
                                    try
                                    {
                                        if (!fileInfo2.Exists)   //We never downloaded/saved this MSCatalogKBfile
                                        {
                                            if (sMSCatalogFileNameLocalPath.Contains("farm-deployment"))    //HARDCODED
                                            {
                                                Console.WriteLine("NOTE: Skipping download");
                                            }
                                            else
                                            {
                                                #region downloadMSKBfile
                                                if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                                                {
                                                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                    Console.WriteLine("DEBUG Downloading");
                                                    using (System.Net.WebClient client = new System.Net.WebClient())
                                                    {
                                                        try
                                                        {
                                                            //Download MSCatalogKBfile
                                                            ////client.DownloadFileAsync(new Uri(sMSCatalogURLFile), "X:\\test.cab");    //Hardcoded
                                                            //client.DownloadFileAsync(new Uri(sMSCatalogURLFile), sMSCatalogFileNameLocalPath);    //Hardcoded
                                                            client.DownloadFile(new Uri(sMSCatalogURLFile), sMSCatalogFileNameLocalPath);    //Hardcoded

                                                        }
                                                        catch (Exception exDownloadMSCatalogKBfile)
                                                        {
                                                            //MessageBox.Show(ex.Message);
                                                            Console.WriteLine("Exception: exDownloadMSCatalogKBfile " + exDownloadMSCatalogKBfile.Message + " " + exDownloadMSCatalogKBfile.InnerException);
                                                        }
                                                    }
                                                }
                                                //else not connected
                                                #endregion downloadMSKBfile
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("DEBUG MSKBFile already downloaded");
                                        }

                                        if (!sMSCatalogFileNameLocalPath.Contains("farm-deployment"))    //HARDCODED
                                        {
                                            //Decompress the KB and search for interesting files
                                            string sMSKBFileExtension = Path.GetExtension(sMSCatalogFileName);  //.cab  .exe    .msi    .msp    .msu
                                            string sMSKBFilePathTarget = sMSCatalogFileNameLocalPath.Replace(sMSKBFileExtension, "");
                                            sFileInfoNeededFound = fDecompressKBAndSearchFiles(sMSKBFilePathTarget, sMSCatalogFileNameLocalPath, sProductName, sProductNameReduced, sFileNameToSearchReplaced);
                                        }
                                    }
                                    catch (Exception exsMSCatalogFileNameLocalPath)
                                    {
                                        Console.WriteLine("Exception: exsMSCatalogFileNameLocalPath " + exsMSCatalogFileNameLocalPath.Message + " " + exsMSCatalogFileNameLocalPath.InnerException);
                                    }
                                }

                            }
                        }
                        catch (Exception exMSCatalogDownload)
                        {
                            Console.WriteLine("Exception: exMSCatalogDownload " + exMSCatalogDownload.Message + " " + exMSCatalogDownload.InnerException);
                        }
                    }
                }
                catch (Exception exDownloadCatalog)
                {
                    Console.WriteLine("Exception: exDownloadCatalog " + exDownloadCatalog.Message + " " + exDownloadCatalog.InnerException);
                }

                driver.Close();
                driver.SwitchTo().Window(baseWindowHandle);

            }
            driver.Dispose();
            #endregion bluepill
            
            Console.WriteLine("DEBUG BluePillsFileInfoNeededFoundFinal=" + sFileInfoNeededFound);    //final one
            return sFileInfoNeededFound;
        }

        //******************************************************************************************************************************************************************************************************
        public static string fDecompressKBAndSearchFiles(string sMSKBFilePathTarget, string sMSCatalogFileNameLocalPath, string sProductName, string sProductNameReduced, string sFileNameToSearchReplaced)
        {
            //Decompress a downloaded KB and search for interesting files
            string sFileInfoNeededFound = string.Empty;
            #region decompresskbandfindfileandversion
            //string sMSKBFileExtension = Path.GetExtension(sMSCatalogFileName);  //.cab  .exe    .msi    .msp    .msu
            //string sMSKBFilePathTarget = sMSCatalogFileNameLocalPath.Replace(sMSKBFileExtension, "");

            //For special cases
            string sProductNameCustomized = sProductNameReduced;
            sProductNameCustomized = sProductNameCustomized.Replace("windows 7", "windows embedded standard 7");
            sProductNameCustomized = sProductNameCustomized.Replace("windows 8", "windows embedded 8");    //Windows_Embedded_8_Standard-rt-x64

            //Shorten it a bit?
            //sMSKBFilePathTarget = fGetSafeFilename(sMSKBFilePathTarget);
            //TODO fGetSafeDirectoryname()

            #region decompresskb1
            try
            {
                if (!Directory.Exists(sMSKBFilePathTarget) || (bForceDecompression && sMSCatalogFileNameLocalPath!=""))//never decompressed
                {
                    //1st level of decompression
                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                    Console.WriteLine("DEBUG DecompressKB1");
                    try
                    {
                        //TODO Filter extensions    e.g. exclude WSUSSCAN.cab
                        //Extract everything
                        fStartProcess(@"C:\Program Files\7-Zip\7z.exe", "e " + sMSCatalogFileNameLocalPath + " -o" + sMSKBFilePathTarget + " -aos");// + " *.dll -r");    //Hardcoded Path
                    }
                    catch (Exception exDecompressMSKB1)
                    {
                        Console.WriteLine("Exception: exDecompressMSKB1 " + exDecompressMSKB1.Message + " " + exDecompressMSKB1.InnerException);
                    }
                }
                //else  already done decompress1
            }
            catch (Exception exDecompressKB1)
            {
                Console.WriteLine("Exception: exDecompressKB1 " + exDecompressKB1.Message + " " + exDecompressKB1.InnerException);
            }
            #endregion decompresskb1

            //What's in this decompressed1 directory?
            // Process the list of files found in the directory.
            #region searchindecompresskb1
            try
            {
                Console.WriteLine("DEBUG Search in DecompressKB1 " + sMSKBFilePathTarget);
                List<string> lFileInfoFound = new List<string>();
                //string[] fileEntries = Directory.GetFiles(sMSKBFilePathTarget); //TODO? Directory.EnumerateFiles(sMSKBFilePathTarget, "*.*", SearchOption.AllDirectories))
                //foreach (string fileName in fileEntries)
                foreach (string fileName in Directory.EnumerateFiles(sMSKBFilePathTarget, "*", SearchOption.AllDirectories))
                {
                    if (fileName.ToLower().Contains("wsusscan.cab")) continue;    //HARDCODEDMS we don't use it
                    if (fileName.ToLower().Contains("inventoryrules.cab")) continue;    //HARDCODEDMS we don't use it


                    //ProcessFile(fileName);
                    string sMSKBFileExtension2 = Path.GetExtension(fileName);   //Could be empty
                                                                                //string sMSKBFilePathTarget2 = fGetSafeFilename(fileName.Replace(sMSKBFileExtension2, ""));
                    string sMSKBFilePathTarget2 = string.Empty;
                    if (sMSKBFileExtension2 != string.Empty)
                    {
                        sMSKBFilePathTarget2 = fileName.Replace(sMSKBFileExtension2, ""); //TODO IMPROVE
                    }
                    else
                    {
                        sMSKBFilePathTarget2 = fileName;
                    }

                    if (sMSKBFileExtension2 == ".cab" || sMSKBFileExtension2 == ".msi" || sMSKBFileExtension2 == ".msp" || sMSKBFileExtension2 == ".msu")    //HARDCODEDMS   not .exe?
                    {
                        #region decompresskb2
                        string s7zipArgument = string.Empty;
                        if (!Directory.Exists(sMSKBFilePathTarget2) || bForceDecompression)
                        {
                            Console.WriteLine("DEBUG DecompressKB2");
                            //2nd level of decompression

                            try
                            {
                                //TODO Filter extensions correctly
                                //fStartProcess(@"C:\Program Files\7-Zip\7z.exe", "e " + fileName + " -o" + sMSKBFilePathTarget2);// + " *.dll -r");    //Hardcoded Path
                                //All files with extension
                                //fStartProcess(@"C:\Program Files\7-Zip\7z.exe", "e " + fileName + " -o" + sMSKBFilePathTarget2 + " *.* -r");    //Hardcoded Path
                                //fStartProcess(@"C:\Program Files\7-Zip\7z.exe", "e " + fileName + " -o" + sMSKBFilePathTarget2 + " -i!*.cab -i!*.msi -i!*.msp -i!*.msu -i!*"+ Path.GetFileNameWithoutExtension(sFileNameToSearchReplaced) +"*.manifest -i*" + sFileNameToSearchReplacedExtension + " -r");    //Hardcoded Path not .exe?

                                //fStartProcess(@"C:\Program Files\7-Zip\7z.exe", "e " + fileName + " -o" + sMSKBFilePathTarget2 + " " + sFileNameToSearchReplaced + " -i!*CAB* -i!*.cab -i!*.msi -i!*.msp -i!*.msu -i!*" + Path.GetFileNameWithoutExtension(sFileNameToSearchReplaced) + "*.manifest" + " -aos -r");    //Hardcoded Path not .exe?
                                if (sMSKBFileExtension2 == ".cab")   //Intra-Package Delta (IPD)
                                {
                                    Directory.CreateDirectory(sMSKBFilePathTarget2);
                                    fStartProcess("expand", fileName + " -F:*.cab " + sMSKBFilePathTarget2); //i.e. msxml30 vs msxml3.dll

                                    foreach (string sFileNameSearched in lFilesToUse)
                                    {
                                        //fStartProcess("expand", fileName + " -F:" + sFileNameSearched + " " + sMSKBFilePathTarget2);
                                        fStartProcess("expand", fileName + " -F:" + Path.GetFileNameWithoutExtension(sFileNameSearched) + "* " + sMSKBFilePathTarget2); //i.e. msxml30 vs msxml3.dll

                                        /*
                                        //Check if it worked
                                        string[] FilesFound = Directory.GetFiles(sMSKBFilePathTarget2, sFileNameSearched, SearchOption.AllDirectories);
                                        if(FilesFound.Count() > 0)
                                        {

                                        }
                                        else{
                                            //In case this did not work (0x80070002)    But less reliable
                                            fStartProcess("expand", fileName + " -F:*" + Path.GetFileNameWithoutExtension(sFileNameSearched) + "*.manifest" + " " + sMSKBFilePathTarget2);   //sFileNameToSearchReplaced
                                        }
                                        */
                                    }
                                    foreach (string sFileNameSearched in lFileNamesToSearch)
                                    {
                                        //fStartProcess("expand", fileName + " -F:" + sFileNameSearched + " " + sMSKBFilePathTarget2);
                                        fStartProcess("expand", fileName + " -F:" + Path.GetFileNameWithoutExtension(sFileNameSearched) + "* " + sMSKBFilePathTarget2); //i.e. msxml30 vs msxml3.dll

                                        /*
                                        //Check if it worked
                                        string[] FilesFound = Directory.GetFiles(sMSKBFilePathTarget2, sFileNameSearched, SearchOption.AllDirectories);
                                        if (FilesFound.Count() > 0)
                                        {

                                        }
                                        else
                                        {
                                            //In case this did not work (0x80070002)    But less reliable
                                            fStartProcess("expand", fileName + " -F:*" + Path.GetFileNameWithoutExtension(sFileNameSearched) + "*.manifest" + " " + sMSKBFilePathTarget2);   //sFileNameToSearchReplaced
                                        }
                                        */
                                    }
                                }
                                else//Not .cab
                                {
                                    //We sometimes can get information from this file (i.e. .msp Details Title= Update for Microsoft Office 2016 (KB2345678) (excel.msp 16.0.4432.1003))
                                    //FileInfo fiKBUpdateFile = new FileInfo(fileName);
                                    //fiKBUpdateFile.Attributes
                                    //FileAttributes.Archive
                                    //FileAttributes.Compressed
                                    if (sMSKBFileExtension2 == ".msi" || fileName.ToLower().Contains("exchange"))
                                    {
                                        //Extract everything
                                        fStartProcess(@"C:\Program Files\7-Zip\7z.exe", "e " + fileName + " -o" + sMSKBFilePathTarget2 + " * -r");    //Hardcoded Path
                                    }
                                    else
                                    {
                                        s7zipArgument = "e " + fileName + " -o" + sMSKBFilePathTarget2;
                                        foreach (string sFileNameSearched in lFilesToUse)
                                        {
                                            if (sFileNameSearched.Trim() != "") s7zipArgument = s7zipArgument + " -i!" + sFileNameSearched;// +" -i!*" + Path.GetFileNameWithoutExtension(sFileNameSearched) + "*.manifest";
                                        }
                                        foreach (string sFileNameSearched in lFileNamesToSearch)
                                        {
                                            if (sFileNameSearched.Trim() != "") s7zipArgument = s7zipArgument + " -i!" + sFileNameSearched;// +" -i!*" + Path.GetFileNameWithoutExtension(sFileNameSearched) + "*.manifest";
                                        }
                                        s7zipArgument = s7zipArgument + " -i!*_CAB -i!*_CAB* -i!*.cab -i!*.msi -i!*.msp -i!*.msu" + " -aos -r"; //PATCH_CAB
                                        fStartProcess(@"C:\Program Files\7-Zip\7z.exe", s7zipArgument);
                                    }
                                }
                                //sFileNameToSearchReplacedExtension
                            }
                            catch (Exception exDecompressMSKB2)
                            {
                                Console.WriteLine("Exception: exDecompressMSKB2 " + exDecompressMSKB2.Message + " " + exDecompressMSKB2.InnerException);
                                Console.WriteLine("DEBUG s7zipArgument=" + s7zipArgument);
                            }
                        }
                        //else  //already done decompress2
                        #endregion decompresskb2

                        //if (Directory.Exists(sMSKBFilePathTarget2))
                        //{
                        //Search into it
                        #region searchindecompresskb2
                        try
                        {
                            Console.WriteLine("DEBUG Search in DecompressKB2 " + sMSKBFilePathTarget2);
                            //string[] fileEntries2 = Directory.GetFiles(sMSKBFilePathTarget2); //TODO? Directory.EnumerateFiles(sMSKBFilePathTarget2, "*.*", SearchOption.AllDirectories))
                            //foreach (string fileName2 in fileEntries2)
                            foreach (string fileName2 in Directory.EnumerateFiles(sMSKBFilePathTarget2, "*", SearchOption.AllDirectories))
                            {
                                string sMyFilenameExtension2 = Path.GetExtension(fileName2);
                                //HARDCODED Extensions
                                if (sMyFilenameExtension2 == ".dll" || sMyFilenameExtension2 == ".sys" || sMyFilenameExtension2 == ".exe" || sMyFilenameExtension2 == ".ocx" || sMyFilenameExtension2 == ".flt") //.sapx? ...   .manifest
                                {
                                    ////Ignore    .dll.mui    .man    .ptxml  ...
                                    //Console.WriteLine("DEBUG filename2=" + fileName2);
                                }
                                else
                                {
                                    //Not interesting file
                                    if (bSaveSpace && !fileName2.ToLower().Contains("cab"))
                                    {
                                        try
                                        {
                                            //Console.WriteLine("DEBUG DELETE22 " + fileName2);
                                            File.Delete(fileName2);
                                            continue;
                                        }
                                        catch (Exception exDeleteFile)
                                        {

                                        }
                                    }
                                }

                                //TODO Review: here we assume that all the FileNamesToSearch in lFileNamesToSearch have the same extension (.dll, .ocx, .sys, .exe, .config)
                                //if (1 == 1 || Path.GetExtension(fileName2) == Path.GetExtension(sFileNameToSearchReplaced))// || Path.GetExtension(fileName2) == ".manifest")    //Hardcoded
                                if (!Path.GetFileName(fileName2).Contains("_CAB") && !fileName2.EndsWith(".cab")) //PATCH_CAB
                                {
                                    //TODO  Review this filter (better location elsewhere)
                                    if (sProductName.Contains("x86") && !fileName2.Contains("x86") && !fileName2.ToLower().Contains("32-bit")) continue;
                                    if (sProductName.Contains("x64") && !fileName2.Contains("x64") && !fileName2.Contains("amd64") && !fileName2.Contains("wow64") && !fileName2.ToLower().Contains("64-bit")) continue;    //amd64
                                    if (sProductName.Contains("x64") && fileName2.Contains("x86_")) continue;   //i.e. Windows6.0-KB3208481-x64\x86_microsoft-windows-hlink_31bf3856ad364e35_6.0.6002.24043_none_5a09d1247a7d880d\hlink.dll
                                    if (sProductName.Contains("itanium") && !fileName2.Contains("ia64") && !fileName2.ToLower().Contains("itanium"))
                                    {
                                        Console.WriteLine("DEBUG Reviewia-64");
                                        continue;  //wow64
                                    }                                                                                                                           //TODO "SP" (Service Pack)

                                    //string sProductNameReduced = sProductName.ToLower().Replace("microsoft ", "").Replace("x86", "").Replace("x64", "").Trim();  //Hardcoded    //i.e. windows vista x86
                                    //string sProductNameReduced = sProductName.Replace("x86", "").Replace("x64", "");  //Hardcoded
                                    sProductNameReduced = sProductNameReduced.Replace("(", "").Replace(")", "").Replace("32-bit", "").Replace("64-bit", "").Trim();
                                    sProductNameReduced = sProductNameReduced.Replace("adobe flash player", "adobe flash");
                                    //Console.WriteLine("DEBUG sProductNameReduced=" + sProductNameReduced);
                                    if (!fileName2.ToLower().Contains(sProductNameReduced.ToLower().Replace(" ", "_")) && !fileName2.ToLower().Contains(sProductNameReduced.ToLower().Replace(" ", "-")) && !fileName2.ToLower().Contains(sProductNameCustomized.ToLower().Replace(" ", "_")))  //TODO Review
                                    {
                                        //Not the right KBfile
                                        Console.WriteLine("DEBUG Not the right KBfile2 " + fileName2);
                                        continue;
                                    }

                                    try
                                    {
                                        //fCheckFileAndGetVersion(fileName2, sFileNameToSearchReplaced);
                                        //We check if the current file is of interest (in our list of files to search for)
                                        foreach (string sFileNameSearched in lFileNamesToSearch)    //TODO Review and Optimize
                                        {
                                            //Console.WriteLine("DEBUG CheckFile2 " + sFileNameSearched);
                                            string sCurrentFilenameWithoutPath = Path.GetFileName(fileName2);
                                            if (sCurrentFilenameWithoutPath.ToLower().Contains(sFileNameSearched.ToLower()) && !sCurrentFilenameWithoutPath.EndsWith(".mui"))   //Hardcoded exclusion   //TODO? version.txt / .manifest
                                            {
                                                sFileInfoNeededFound = fCheckFileAndGetVersion(fileName2, sFileNameSearched);
                                                if (sFileInfoNeededFound != "" && bDebugFileSelection) Console.WriteLine("DEBUG BluePillsFileInfoNeededFoundTest=" + sFileInfoNeededFound);
                                                //TODO Review sometimes multiple files, is that the good one/version?
                                                if (sFileInfoNeededFound != string.Empty)
                                                {
                                                    //20170208 gdi32.dll 6.2.9200.22084
                                                    if (!lFileInfoFound.Contains(sFileInfoNeededFound)) lFileInfoFound.Add(sFileInfoNeededFound);   //Note: We add Limited Distribution Release (LDR) file/version AND GDR
                                                    if (!dProductFile.ContainsKey(sProductFoundDEADBEEFGlobal))
                                                    {
                                                        Console.WriteLine("DEBUG dProductFile.Add " + sProductFoundDEADBEEFGlobal);
                                                        dProductFile.Add(sProductFoundDEADBEEFGlobal, sFileInfoNeededFound.ToLower());
                                                        Console.WriteLine("DEBUG BluePillsFileInfoNeededFound2=" + sFileInfoNeededFound);
                                                    }
                                                    else
                                                    {
                                                        #region keeplowestversion8
                                                        //TODO Review for LDR/GDR
                                                        //Console.WriteLine("ERROR02: TODO dProductFile already contains " + sProduct);
                                                        //Analyze the situation: at the end, we will keep the lowest version number
                                                        if (dProductFile[sProductFoundDEADBEEFGlobal] == sFileInfoNeededFound.ToLower())
                                                        {
                                                            //No issue there*
                                                        }
                                                        else
                                                        {
                                                            //Get back the file's information previously collected
                                                            string[] PreviousFileInfoNeededSplit = dProductFile[sProductFoundDEADBEEFGlobal].Split(' ');
                                                            string[] CurrentFileInfoNeededSplit = sFileInfoNeededFound.ToLower().Split(' ');

                                                            //Is it the same file name?
                                                            if (PreviousFileInfoNeededSplit[1] == CurrentFileInfoNeededSplit[1])
                                                            {
                                                                #region comparefilesversions
                                                                //Compare the versions  using Version class //https://stackoverflow.com/questions/7568147/compare-version-numbers-without-using-split-function
                                                                string v1 = PreviousFileInfoNeededSplit[2]; //"1.23.56.1487";
                                                                string v2 = CurrentFileInfoNeededSplit[2];   //"1.24.55.487";

                                                                var version1 = new Version(v1);
                                                                var version2 = new Version(v2);

                                                                var result = version1.CompareTo(version2);
                                                                if (result > 0)
                                                                {
                                                                    //Console.WriteLine("version1 is greater");
                                                                    //We replace the value in the dictionary
                                                                    if (bDebugFileSelection) Console.WriteLine("DEBUG: Previous version found " + v1 + " > new version found " + v2 + " so we keep the new one (lowest version)");
                                                                    //TODO: LDR/GDR
                                                                    //20160909 win32k.sys 6.0.6002.24017    >   20160910 win32k.sys 6.0.6002.19693
                                                                    dProductFile[sProductFoundDEADBEEFGlobal] = sFileInfoNeededFound;
                                                                }
                                                                else if (result < 0)
                                                                {
                                                                    //Console.WriteLine("version2 is greater");
                                                                    //We keep version1
                                                                    //TODO: LDR/GDR
                                                                }
                                                                else
                                                                {
                                                                    //same as before*
                                                                    //Console.WriteLine("versions are equal");
                                                                    //We should be ok (we don't compare the dates)
                                                                }
                                                                #endregion comparefilesversions
                                                            }
                                                            else//different files
                                                            {
                                                                //TODO!!! Review this (we could use the Dates, or Hardcoding...)
                                                                //TODO FavoriteFiles
                                                                if (sFileNameToSearchReplaced != "" || (sFileNameToSearchGlobal != "" && CurrentFileInfoNeededSplit[1] == sFileNameToSearchGlobal))
                                                                {
                                                                    if (!lFileNamesNOTToSearch.Contains(CurrentFileInfoNeededSplit[1]))
                                                                    {
                                                                        //We keep the new one
                                                                        //i.e.: usp10.dll   =>  mso.dll
                                                                        dProductFile[sProductFoundDEADBEEFGlobal] = sFileInfoNeededFound;
                                                                        if (bDebugFileSelection) Console.WriteLine("DEBUG BluePillsFileInfoNeededFound22=" + sFileInfoNeededFound);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    //HARDCODED
                                                                    if (sFileInfoNeededFound.Contains("mso.dll")) dProductFile[sProductFoundDEADBEEFGlobal] = sFileInfoNeededFound;

                                                                }
                                                            }
                                                        }
                                                        #endregion keeplowestversion8
                                                    }

                                                    if (bDebugFilesFound) Console.WriteLine("DEBUG Break35");
                                                    
                                                    break;
                                                }
                                            }
                                            //else
                                            //{
                                            //Not interesting file
                                            //We don't delete it here
                                            //}
                                        }
                                        if (bSaveSpace && lFileInfoFound.Count <= 0) // && !fileName2.ToLower().Contains("cab"))    //TODO? +!Contains
                                        {
                                            try
                                            {
                                                //Console.WriteLine("DEBUG DELETE23 " + fileName2);
                                                File.Delete(fileName2);
                                            }
                                            catch (Exception exDeleteFile)
                                            {

                                            }
                                        }

                                    }
                                    catch (Exception exCheckFileInDecompressKB2)
                                    {
                                        Console.WriteLine("Exception: exCheckFileInDecompressKB2 " + exCheckFileInDecompressKB2.Message + " " + exCheckFileInDecompressKB2.InnerException);
                                    }

                                }
                                else//It is _CAB or .cab
                                {
                                    //if (Path.GetFileName(fileName2).Contains("_CAB") || fileName2.EndsWith(".cab"))   //HARDCODEDMS   //_CAB    i.e. PATCH_CAB
                                    //{
                                    if (!fileName2.Contains("inventoryrules.cab"))  //HardcodedMS
                                    {
                                        Console.WriteLine("DEBUG " + DateTime.Now + " DecompressKB3 " + sMSKBFilePathTarget2);
                                        #region decompresskb3
                                        //http://www.sqldbadiaries.com/2014/08/22/how-to-extract-the-contents-of-msp-files/
                                        //string sMSKBFilePathTarget3 = fileName2;

                                        //3rd level of decompression
                                        //fStartProcess(@"C:\Program Files\7-Zip\7z.exe", "e " + fileName2 + " -o" + sMSKBFilePathTarget3 + " " + sFileNameToSearchReplaced + " -i!*CAB* -i!*.cab -i!*.msi -i!*.msp -i!*.msu -i!*" + Path.GetFileNameWithoutExtension(sFileNameToSearchReplaced) + "*.manifest" + " -r");    //Hardcoded Path not .exe?
                                        //Extract evrything
                                        fStartProcess(@"C:\Program Files\7-Zip\7z.exe", "e " + fileName2 + " -o" + sMSKBFilePathTarget2 + " -aos -r");    //Hardcoded Path

                                        #endregion decompresskb3
                                        //Form there, we could find/use the (biggest extracted file), or *_CAB_*\.rsrc\version.txt

                                        #region searchindecompresskb3
                                        try
                                        {
                                            Console.WriteLine("DEBUG Search in DecompressKB3 " + sMSKBFilePathTarget2);
                                            if (sMSKBFilePathTarget2.ToLower().Contains("mso"))
                                            {
                                                if (!lFileNamesToSearch.Contains("mso.dll")) lFileNamesToSearch.Add("mso.dll");
                                                //ietag.dll
                                            }
                                            if (sMSKBFilePathTarget2.ToLower().Contains("mscomctl"))    //mscomctlocx
                                            {
                                                if (!lFileNamesToSearch.Contains("mscomctl.ocx")) lFileNamesToSearch.Add("mscomctl.ocx");
                                                if (!lFileNamesToSearch.Contains("mscomctl.dll")) lFileNamesToSearch.Add("mscomctl.dll");
                                            }
                                            if (sMSKBFilePathTarget2.ToLower().Contains("powerpoint"))
                                            {
                                                if (!lFileNamesToSearch.Contains("powerpnt.exe")) lFileNamesToSearch.Add("powerpnt.exe");
                                                //ietag.dll
                                            }
                                            if (sMSKBFilePathTarget2.ToLower().Contains("sts")) //sharepoint
                                            {
                                                if (!lFileNamesToSearch.Contains("stsom.dll")) lFileNamesToSearch.Add("stsom.dll");
                                                //...
                                            }
                                            if (sMSKBFilePathTarget2.ToLower().Contains("wasrvmui")) //sharepoint
                                            {
                                                if (!lFileNamesToSearch.Contains("waintlr.dll")) lFileNamesToSearch.Add("waintlr.dll");
                                                //WDSRV.INTLRESOURCES.DLL
                                                //WDSRV.CONVERSION.WORD.WWINTL.dll

                                                //...
                                            }
                                            //wacwfe?
                                            if (sMSKBFilePathTarget2.ToLower().Contains("xlsrv")) //xlsrvmui
                                            {
                                                if (!lFileNamesToSearch.Contains("xlsrv.dll")) lFileNamesToSearch.Add("xlsrv.dll");
                                                if (!lFileNamesToSearch.Contains("xlsrvintl.dll")) lFileNamesToSearch.Add("xlsrvintl.dll");
                                                //...
                                            }
                                            if (sMSKBFilePathTarget2.ToLower().Contains("webconsole")) //System Center 2012 SP1 - Operation Manager
                                            {
                                                if (!lFileNamesToSearch.Contains("Microsoft.EnterpriseManagement.Presentation.WebConsole.dll")) lFileNamesToSearch.Add("Microsoft.EnterpriseManagement.Presentation.WebConsole.dll");
                                            }
                                            if (sMSKBFilePathTarget2.ToLower().Contains("vbe6")) //Office
                                            {
                                                if (!lFileNamesToSearch.Contains("vbe6.dll")) lFileNamesToSearch.Add("vbe6.dll");
                                                //vbeui.dll
                                            }
                                            if (sMSKBFilePathTarget2.ToLower().Contains("vbe7")) //Office
                                            {
                                                if (!lFileNamesToSearch.Contains("vbe7.dll")) lFileNamesToSearch.Add("vbe7.dll");
                                                //vbeui.dll
                                            }
                                            if (sMSKBFilePathTarget2.ToLower().Contains("word"))
                                            {
                                                if (!lFileNamesToSearch.Contains("winword.exe")) lFileNamesToSearch.Add("winword.exe");
                                                //wrd12cnv.dll
                                                //wwlib.dll
                                            }
                                            if (sMSKBFilePathTarget2.ToLower().Contains("msptls"))
                                            {
                                                if (!lFileNamesToSearch.Contains("msptls.dll")) lFileNamesToSearch.Add("msptls.dll");
                                            }
                                            if (sMSKBFilePathTarget2.ToLower().Contains("ieawsdc"))
                                            {
                                                if (!lFileNamesToSearch.Contains("ieawsdc.dll")) lFileNamesToSearch.Add("ieawsdc.dll");
                                            }
                                            if (sMSKBFilePathTarget2.ToLower().Contains("lddmcore"))
                                            {
                                                if (!lFileNamesToSearch.Contains("dxgkrnl.sys")) lFileNamesToSearch.Add("dxgkrnl.sys");
                                            }

                                            /*
                                            string[] fileEntries3 = Directory.GetFiles(sMSKBFilePathTarget3);
                                            foreach (string fileName3 in fileEntries3)
                                            {
                                                fCheckFileAndGetVersion(fileName2, sFileNameToSearchReplaced);
                                            }
                                            */
                                            foreach (string fileName3 in Directory.EnumerateFiles(sMSKBFilePathTarget2, "*", SearchOption.AllDirectories))
                                            {
                                                //TODO  Review this filter (better location elsewhere)
                                                if (sProductName.Contains("x86") && !fileName3.Contains("x86") && !fileName3.ToLower().Contains("32-bit")) continue;
                                                if (sProductName.Contains("x64") && !fileName3.Contains("x64") && !fileName3.Contains("amd64") && !fileName3.Contains("wow64") && !fileName3.ToLower().Contains("64-bit")) continue;
                                                if (sProductName.Contains("x64") && fileName3.Contains("x86_")) continue;
                                                if (sProductName.Contains("itanium") && !fileName3.Contains("ia64") && !fileName3.ToLower().Contains("itanium")) continue;  //wow64
                                                                                                                                                                            //TODO "SP" (Service Pack)

                                                //string sProductNameReduced = sProductName.Replace("x86", "").Replace("x64", "");  //Hardcoded
                                                if (!fileName3.ToLower().Contains(sProductNameReduced.ToLower().Replace(" ", "_")) && !fileName3.ToLower().Contains(sProductNameReduced.ToLower().Replace(" ", "-")) && !fileName3.ToLower().Contains(sProductNameCustomized.ToLower().Replace(" ", "_")))  //TODO Review    //Windows_Embedded_Standard_7
                                                {
                                                    //Not the right KBfile
                                                    Console.WriteLine("DEBUG Not the right KBfile3 " + fileName3);
                                                    continue;
                                                    //Console.WriteLine("DEBUG ")
                                                }

                                                //fCheckFileAndGetVersion(fileName3, sFileNameToSearchReplaced);
                                                string sFileInfoNeededFoundTemp = sFileInfoNeededFound;
                                                foreach (string sFileNameSearched in lFileNamesToSearch)
                                                {
                                                    sFileInfoNeededFound = fCheckFileAndGetVersion(fileName3, sFileNameSearched);

                                                    //Review
                                                    if (sFileInfoNeededFound.EndsWith(".x86")) sFileInfoNeededFound = sFileInfoNeededFound.Replace(".x86", "");
                                                    if (sFileInfoNeededFound.EndsWith(".x64")) sFileInfoNeededFound = sFileInfoNeededFound.Replace(".x64", "");

                                                    //TODO Review sometimes multiple files, is that the good one/version?

                                                    /*
                                                    if (!dProductFile.ContainsKey(sProductFoundDEADBEEFGlobal))
                                                    {
                                                        Console.WriteLine("DEBUG dProductFile.Add " + sProductFoundDEADBEEFGlobal);
                                                        dProductFile.Add(sProductFoundDEADBEEFGlobal, sFileInfoNeededFound.ToLower());
                                                        Console.WriteLine("DEBUG BluePillsFileInfoNeededFound2=" + sFileInfoNeededFound);
                                                    }
                                                    */

                                                    if (sFileInfoNeededFound != string.Empty)
                                                    {
                                                        if (sFileInfoNeededFoundTemp != "" && sFileInfoNeededFoundTemp != sFileInfoNeededFound)
                                                        {
                                                            //TODO FavoriteFile
                                                            if (fileName3.ToLower().Contains("word") && !fileName3.ToLower().Contains("viewer") && sFileInfoNeededFoundTemp == "winword.exe")// && sFileInfoNeededFound == "winword.exe")
                                                            {
                                                                //We keep our favorite file
                                                            }
                                                            else
                                                            {
                                                                sFileInfoNeededFoundTemp = sFileInfoNeededFound;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            sFileInfoNeededFoundTemp = sFileInfoNeededFound;
                                                        }

                                                        if (!lFileInfoFound.Contains(sFileInfoNeededFound)) lFileInfoFound.Add(sFileInfoNeededFound);
                                                        Console.WriteLine("DEBUG sFileInfoNeededFound3=" + sFileInfoNeededFound);
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        //Not interesting file      .cab
                                                        //TODO Review
                                                        /*
                                                        if (bSaveSpace)
                                                        {
                                                            try
                                                            {
                                                                Console.WriteLine("DEBUG DELETE3 " + fileName);
                                                                File.Delete(fileName3);
                                                            }
                                                            catch (Exception exDeleteFile)
                                                            {

                                                            }
                                                        }
                                                        */
                                                    }
                                                }
                                                if (sFileInfoNeededFound == "" && sFileInfoNeededFoundTemp != "") sFileInfoNeededFound = sFileInfoNeededFoundTemp;

                                            }
                                        }
                                        catch (Exception exSearchInDecompressKB3)
                                        {
                                            Console.WriteLine("Exception: exSearchInDecompressKB3 " + exSearchInDecompressKB3.Message + " " + exSearchInDecompressKB3.InnerException);
                                        }
                                        #endregion searchindecompresskb3
                                    }
                                    //}
                                }
                            }
                            //}
                        }
                        catch (ArgumentException exSearchInDecompressKB2ArgumentException)
                        {
                            Console.WriteLine("Exception: exSearchInDecompressKB2ArgumentException " + exSearchInDecompressKB2ArgumentException.Message + " " + exSearchInDecompressKB2ArgumentException.InnerException);
                        }
                        catch (PathTooLongException exSearchInDecompressKB2PathTooLong)
                        {
                            Console.WriteLine("Exception: exSearchInDecompressKB2PathTooLong " + exSearchInDecompressKB2PathTooLong.Message + " " + exSearchInDecompressKB2PathTooLong.InnerException);
                        }
                        catch (Exception exSearchInDecompressKB2)
                        {
                            Console.WriteLine("Exception: exSearchInDecompressKB2 " + exSearchInDecompressKB2.Message + " " + exSearchInDecompressKB2.InnerException);
                            Console.WriteLine("DEBUG s7zipArgument2=" + s7zipArgument);
                        }
                        #endregion searchindecompresskb2
                    }
                    else//not .cab .msi .msu .msp (.exe?)
                    {
                        //.dll? .ocx?   .sys?...
                        //TODO Review: here we assume that all the FileNamesToSearch in lFileNamesToSearch have the same extension (.dll, .ocx, .sys, .exe)
                        //TODO Review   Contains(sFileNameToSearchReplaced)?
                        if (Path.GetExtension(fileName) == Path.GetExtension(sFileNameToSearchReplaced))// || Path.GetExtension(fileName) == ".manifest")  //Hardcoded
                        {
                            //TODO  Review this filter (better location elsewhere)
                            if (sProductName.Contains("x86") && !fileName.Contains("x86") && !fileName.ToLower().Contains("32-bit")) continue;
                            if (sProductName.Contains("x64") && !fileName.Contains("x64") && !fileName.Contains("amd64") && !fileName.Contains("wow64") && !fileName.ToLower().Contains("64-bit")) continue;
                            if (sProductName.Contains("x64") && fileName.Contains("x86_")) continue;
                            if (sProductName.Contains("itanium") && !fileName.Contains("ia64") && !fileName.ToLower().Contains("itanium")) continue;    //wow64
                            sProductNameReduced = sProductNameReduced.Replace("adobe flash player", "adobe flash");

                            //string sProductNameReduced = sProductName.Replace("x86", "").Replace("x64", "");  //Hardcoded
                            try
                            {
                                if (!fileName.ToLower().Contains(sProductNameReduced.Replace(" ", "_").ToLower()) && !fileName.ToLower().Contains(sProductNameReduced.Replace(" ", "-").ToLower()) && !fileName.ToLower().Contains(sProductNameCustomized.Replace(" ", "_").ToLower()))  //TODO Review
                                {
                                    //Not the right KBfile
                                    Console.WriteLine("DEBUG Not the right KBfile0 " + fileName);
                                    continue;
                                    //Console.WriteLine("DEBUG ")
                                }
                            }
                            catch (Exception exContinue1)
                            {
                                Console.WriteLine("Exception: exContinue1 " + exContinue1.Message + " " + exContinue1.InnerException);
                            }

                            try
                            {
                                string sFileInfoNeededFoundTemp = sFileInfoNeededFound;
                                //ziza
                                //fCheckFileAndGetVersion(fileName, sFileNameToSearchReplaced);
                                foreach (string sFileNameSearched in lFileNamesToSearch)
                                {
                                    sFileInfoNeededFound = fCheckFileAndGetVersion(fileName, sFileNameSearched);
                                    //TODO Review sometimes multiple files, is that the good one/version?
                                    if (sFileInfoNeededFound != string.Empty)
                                    {
                                        sFileInfoNeededFoundTemp = sFileInfoNeededFound;
                                        Console.WriteLine("DEBUG BluePillsFileInfoNeededFound1=" + sFileInfoNeededFound);
                                        if (!lFileInfoFound.Contains(sFileInfoNeededFound)) lFileInfoFound.Add(sFileInfoNeededFound);

                                        if (!dProductFile.ContainsKey(sProductFoundDEADBEEFGlobal))
                                        {
                                            Console.WriteLine("DEBUG dProductFile.Add1 " + sProductFoundDEADBEEFGlobal);
                                            dProductFile.Add(sProductFoundDEADBEEFGlobal, sFileInfoNeededFound.ToLower());
                                        }

                                        Console.WriteLine("DEBUG break1");
                                        break;
                                    }

                                }
                                if (sFileInfoNeededFound == "" && sFileInfoNeededFoundTemp != "") sFileInfoNeededFound = sFileInfoNeededFoundTemp;

                            }
                            catch (Exception exCheckFileInDecompressKB1)
                            {
                                Console.WriteLine("Exception: exCheckFileInDecompressKB1 " + exCheckFileInDecompressKB1.Message + " " + exCheckFileInDecompressKB1.InnerException);
                            }
                        }
                        else
                        {
                            //TODO Review

                            //Not interesting file
                            string sMyFilenameExtension = Path.GetExtension(fileName);
                            //if (bSaveSpace && (sMyFilenameExtension == string.Empty || sMyFilenameExtension == null))  //... not .dll, .exe, .sys ...
                            if (bSaveSpace && (sMyFilenameExtension != ".exe" && sMyFilenameExtension != ".dll" && sMyFilenameExtension != ".ocx" && sMyFilenameExtension != ".sys"))  //... .flt? .aspx?
                            {
                                try
                                {
                                    //Console.WriteLine("DEBUG DELETE1 " + fileName);
                                    File.Delete(fileName);
                                }
                                catch (Exception exDeleteFile)
                                {

                                }
                            }

                        }
                    }

                }
                try
                {
                    string[] FileInfoNeededFoundSplit = sFileInfoNeededFound.Split(' ');
                    if (lFileInfoFound.Count == 1)
                    {
                        if (FileInfoNeededFoundSplit[1] != "")
                        {
                            //TODO FavoriteFile
                            //20160924 ogl.dll 4.0.7577.4521
                            Console.WriteLine("DEBUG sFileNameToSearchReplacedByFound1=" + sFileNameToSearchReplaced);
                            sFileNameToSearchReplaced = FileInfoNeededFoundSplit[1];
                        }
                    }
                    if (lFileInfoFound.Count > 1 && sFileNameToSearchReplaced != "")
                    {
                        lFileInfoFound = lFileInfoFound.OrderByDescending(o => o).ToList(); //date  filename    version
                        foreach (string sFileInfoReviewed in lFileInfoFound) //TODO: Here we could manage LDR/GDR
                        {
                            FileInfoNeededFoundSplit = sFileInfoReviewed.Split(' ');
                            if (!lFileNamesNOTToSearch.Contains(FileInfoNeededFoundSplit[1]))
                            {
                                if (sFileInfoReviewed.Contains(sFileNameToSearchReplaced))
                                {
                                    sFileInfoNeededFound = sFileInfoReviewed;   //Will be the final one
                                    Console.WriteLine("DEBUG sFileInfoNeededFoundFinal=" + sFileInfoNeededFound);
                                    break;
                                }
                            }
                        }
                    }
                }
                catch (Exception exFileInfoNeededFoundSplit)
                {
                    Console.WriteLine("Exception: exFileInfoNeededFoundSplit " + exFileInfoNeededFoundSplit.Message + " " + exFileInfoNeededFoundSplit.InnerException);
                }

            }
            catch (Exception exSearchInDecompressKB1)
            {
                Console.WriteLine("Exception: exSearchInDecompressKB1 " + exSearchInDecompressKB1.Message + " " + exSearchInDecompressKB1.InnerException);
            }
            #endregion searchindecompresskb1
            #endregion decompresskbandfindfileandversion

            return sFileInfoNeededFound;
        }

        public static string fCheckFileAndGetVersion(string fileName, string sFileNameToSearchReplaced)
        {
            //Console.WriteLine("DEBUG fCheckFileAndGetVersion " + DateTimeOffset.Now);
            //Note: fileName is a path
            string sCurrentFilenameWithoutPath = Path.GetFileName(fileName);
            //Console.WriteLine("DEBUG fCheckFileAndGetVersionfileName=" + sCurrentFilenameWithoutPath);

            string sFilenameFound = string.Empty;
            string sFileFoundVersionNumber = string.Empty;// "";
            string sFileFoundDate = string.Empty;
            Regex myRegexVersionNumber = new Regex(@"\d+(?:\.\d+)+");   //TODO Review Improve
            //^\d{1,3}\.\d{1,3}(?:\.\d{1,6})?$

            //if (fileName.ToLower().EndsWith(sFileNameToSearchReplaced.ToLower()))   //? .Contains() instead
            if (sCurrentFilenameWithoutPath.ToLower().Contains(sFileNameToSearchReplaced.ToLower()) && !sCurrentFilenameWithoutPath.EndsWith(".mui"))   //Hardcoded exclusion
            {
                //We found our file
                //Console.WriteLine("DEBUG FileFound: " + fileName);
                sFilenameFound = sCurrentFilenameWithoutPath.ToLower();
                if (sFilenameFound.EndsWith(".x86")) sFilenameFound = sFilenameFound.Replace(".x86", "");
                if (sFilenameFound.EndsWith(".x64")) sFilenameFound = sFilenameFound.Replace(".x64", "");

                FileVersionInfo myFileVersionInfo = FileVersionInfo.GetVersionInfo(fileName);
                //    Console.WriteLine("DEBUG FileFound: " + fileName + "\nFile Description: " + myFileVersionInfo.FileDescription + "\nVersion number: " + myFileVersionInfo.FileVersion);
                //sFileFoundVersionNumber = myFileVersionInfo.FileVersion; //(ProductVersion?)
                //built by: lcs_se_w14_main(rtbldlab)
                //6.0.6002.19721 (vistasp2_gdr.161120-0600)
                //6.0.6002.24043 (vistasp2_ldr.161120-0600)
                FileInfo myFileInfo = new System.IO.FileInfo(fileName);
                sFileFoundDate = myFileInfo.LastWriteTime.ToString("yyyyMMdd");

                //Cleaning
                try
                {
                    sFileFoundVersionNumber = myRegexVersionNumber.Match(myFileVersionInfo.FileVersion.Replace(",", ".")).ToString();

                    string sCurrentFileExtension = Path.GetExtension(fileName);
                    if (sCurrentFileExtension == ".dll" || sCurrentFileExtension == ".exe" || sCurrentFileExtension == ".ocx" || sCurrentFileExtension == ".sys")   //Hardcoded     (Review for .Net Framework)
                    {
                        try
                        {
                            fXORCISMAddFILE(sFilenameFound, sFileFoundVersionNumber, sFileFoundDate);
                        }
                        catch(Exception exXAddFile)
                        {
                            Console.WriteLine("Exception: exXAddFile " + exXAddFile.Message + " " + exXAddFile.InnerException);
                        }
                        #region addfileversiontodatabase
                        /*
                        int iFileID = 0;
                        int iFileVersionID = 0;
                        try
                        {
                            try
                            {
                                iFileID = model.FILE.FirstOrDefault(o => o.FileName == sFilenameFound).FileID;
                            }
                            catch (Exception ex)
                            {

                            }
                            if (iFileID <= 0)
                            {
                                FILE oFile = new FILE();
                                oFile.CreatedDate = DateTimeOffset.Now;
                                oFile.FileName = sFilenameFound;
                                //oFile.VocabularyID=
                                oFile.timestamp = DateTimeOffset.Now;
                                model.FILE.Add(oFile);
                                model.SaveChanges();
                                iFileID = oFile.FileID;
                            }
                            try
                            {
                                iFileVersionID = model.FILEVERSION.FirstOrDefault(o => o.FileID == iFileID && o.VersionValue == sFileFoundVersionNumber).FileVersionID;
                            }
                            catch (Exception ex)
                            {

                            }
                            if (iFileVersionID <= 0)
                            {
                                FILEVERSION oFileVersion = new FILEVERSION();
                                oFileVersion.CreatedDate = DateTimeOffset.Now;
                                oFileVersion.FileID = iFileID;
                                oFileVersion.VersionValue = sFileFoundVersionNumber;
                                //oFileVersion.VocabularyID=
                                oFileVersion.timestamp = DateTimeOffset.Now;
                                model.FILEVERSION.Add(oFileVersion);
                                model.SaveChanges();

                            }
                        }
                        catch (Exception exFileVersionDB)
                        {
                            Console.WriteLine("Exception: exFileVersionDB " + exFileVersionDB.Message + " " + exFileVersionDB.InnerException+ "\nsFilenameFound="+ sFilenameFound+"\nsFileFoundVersionNumber =" + sFileFoundVersionNumber);
                        }
                        */
                        #endregion addfileversiontodatabase
                    }
                }
                catch (Exception exRegexFileFoundVersionNumber)
                {
                    Console.WriteLine("Exception: exRegexFileFoundVersionNumber fileName="+ fileName + "\n" + exRegexFileFoundVersionNumber.Message + " " + exRegexFileFoundVersionNumber.InnerException);  //cortanaapi.dll
                }
                
                //break;
            }
            else
            {
                if (sCurrentFilenameWithoutPath == "version.txt")   //Hardcoded of course   (from the _CAB)
                {
                    //TODO: Probably not needed anymore
                    #region versiontxt
                    // Read the file as one string.
                    string sText = System.IO.File.ReadAllText(@fileName);
                    if (sText.ToLower().Contains(sFileNameToSearchReplaced.ToLower()))   //i.e. VALUE "OriginalFilename",  "gdiplus.dll"
                    {
                        Console.WriteLine("DEBUG Found " + fileName);
                        string[] lines = System.IO.File.ReadAllLines(@fileName);

                        foreach (string line in lines)
                        {
                            //FILEVERSION    11,0,8435,0
                            if (line.StartsWith("FILEVERSION")) //HARDCODEDMS
                            {
                                sFileFoundVersionNumber = line.Replace("FILEVERSION", "").Trim().Replace(",", ".");  //Hardcoded
                                //TODO Improve :-)  Regex
                                sFileFoundVersionNumber = myRegexVersionNumber.Match(sFileFoundVersionNumber).ToString();

                                //Date?
                                break;
                            }
                        }
                    }
                    #endregion versiontxt
                }
                else
                {
                    //if (Path.GetExtension(fileName)==".manifest" && fileName.ToLower().Contains(Path.GetFileNameWithoutExtension(sFileNameToSearchReplaced)))
                    
                    if (bUseManifestFilesVersions && Path.GetExtension(sCurrentFilenameWithoutPath) == ".manifest" && sCurrentFilenameWithoutPath.ToLower().Contains(Path.GetFileNameWithoutExtension(sFileNameToSearchReplaced.ToLower())))    //os-kernel
                    {
                        //i.e.: amd64_microsoft.windows.gdiplus.systemcopy_31bf3856ad364e35_6.3.9600.18470_none_d79f99a9ca00398e.manifest
                        //vs amd64_microsoft.windows.gdiplus_6595b64144ccf1df_1.1.9600.18470_none_9331b0df474a1995.manifest
                        //x86_microsoft.windows.gdiplus.systemcopy_31bf3856ad364e35_6.3.9600.18470_none_7b80fe2611a2c858.manifest
                        //TODO: x86 vs amd64 (x64)
                        //if (sCurrentFilenameWithoutPath.Contains("systemcopy")) //HARDCODEDMS   TODO Review (if not, we will have multiple ones, so multiple versions)
                        //{
                        sFilenameFound = sCurrentFilenameWithoutPath.ToLower();
                        sFileFoundVersionNumber = myRegexVersionNumber.Match(sCurrentFilenameWithoutPath).ToString();

                        //Console.WriteLine("DEBUG FileManifest: " + sCurrentFilenameWithoutPath + '\n' + " sFileFoundVersionNumber=" + sFileFoundVersionNumber);
                        FileInfo myFileInfo = new System.IO.FileInfo(fileName); //NOTE: Date of the manifest file
                        sFileFoundDate = myFileInfo.LastWriteTime.ToString("yyyyMMdd");
                        //}
                    }
                    else
                    {
                        //Is that our file? i.e. from _CAB
                        FileVersionInfo myFileVersionInfo = FileVersionInfo.GetVersionInfo(fileName);
                        //Console.WriteLine("DEBUG FileTest: " + myFileVersionInfo.FileDescription + '\n' + "Version number: " + myFileVersionInfo.FileVersion);
                        if (myFileVersionInfo.OriginalFilename == sFileNameToSearchReplaced)    //TODO? Contains
                        {
                            //Bingo
                            sFilenameFound = sCurrentFilenameWithoutPath.ToLower();
                            if (sFilenameFound.EndsWith(".x86")) sFilenameFound = sFilenameFound.Replace(".x86", "");
                            if (sFilenameFound.EndsWith(".x64")) sFilenameFound = sFilenameFound.Replace(".x64", "");

                            Console.WriteLine("DEBUG FileRetrieved: "+ sFilenameFound +" "+ myFileVersionInfo.FileDescription + '\n' + "Version number: " + myFileVersionInfo.FileVersion);
                            //sFileFoundVersionNumber = myFileVersionInfo.FileVersion;
                            //Just to be sure
                            sFileFoundVersionNumber = myRegexVersionNumber.Match(myFileVersionInfo.FileVersion).ToString();
                            if (sFileFoundVersionNumber == string.Empty) sFileFoundVersionNumber = myFileVersionInfo.FileVersion;

                            FileInfo myFileInfo = new System.IO.FileInfo(fileName);
                            sFileFoundDate = myFileInfo.LastWriteTime.ToString("yyyyMMdd");

                        }
                        else
                        {
                            //TODO Review
                            /*
                            //Not interesting file
                            if(bSaveSpace)
                            {
                                try
                                {
                                    Console.WriteLine("DEBUG DELETE0 " + fileName);
                                    File.Delete(fileName);
                                }
                                catch(Exception exDeleteFile)
                                {
                                    
                                }
                            }
                            */
                        }
                    }
                }
            }

            //return sFileFoundVersionNumber;
            if (sFileFoundVersionNumber == string.Empty)
            {
                //Console.WriteLine("DEBUG FileVersionNumberUnknown");
                return string.Empty;
            }
            else
            {
                //string sFileInfoFound = sFileFoundDate + " " + sFileNameToSearchReplaced + " " + sFileFoundVersionNumber;  //i.e.  //20161231 ptxt9.dll 14.0.7177.5000
                string sFileInfoFound = sFileFoundDate + " " + sFilenameFound + " " + sFileFoundVersionNumber;  //i.e.  //20161231 ptxt9.dll 14.0.7177.5000

                if (bDebugFilesFound) Console.WriteLine("DEBUG FileInfoFound: " + sFileInfoFound);
                return sFileInfoFound;
            }
        }


        public static List<string> fCSVParse(string line)
        {
            const char escapeChar = '"';
            const char splitChar = ',';
            bool inEscape = false;
            bool priorEscape = false;

            List<string> result = new List<string>();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];
                switch (c)
                {
                    case escapeChar:
                        if (!inEscape)
                            inEscape = true;
                        else
                        {
                            if (!priorEscape)
                            {
                                if (i + 1 < line.Length && line[i + 1] == escapeChar)
                                    priorEscape = true;
                                else
                                    inEscape = false;
                            }
                            else
                            {
                                sb.Append(c);
                                priorEscape = false;
                            }
                        }
                        break;
                    case splitChar:
                        if (inEscape) //if in escape
                            sb.Append(c);
                        else
                        {
                            result.Add(sb.ToString());
                            sb.Length = 0;
                        }
                        break;
                    default:
                        sb.Append(c);
                        break;
                }
            }

            if (sb.Length > 0)
                result.Add(sb.ToString());

            return result;
        }

        /*
        public static string fFilenameToSearchReplacedHardcoded(string sFileNameToSearchReplaced, string sProductFound)
        {
            #region filenametosearchhardcoded

            if (sFileNameToSearchReplaced == "" && sProductFound.Contains("excel viewer"))
            {
                sFileNameToSearchReplaced = "xlview.exe"; //Hardcoded
                if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
            }
            if (sFileNameToSearchReplaced == "" && sProductFound.Contains("excel services"))
            {
                sFileNameToSearchReplaced = "xlsrvintl.dll"; //Hardcoded    (good luck with .resx...)
                if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                sFileNameToSearchReplaced = "xlsrv.dll"; //Hardcoded
                if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
            }
            //word automation services
            if (sFileNameToSearchReplaced == "" && sProductFound.Contains("excel"))
            {
                sFileNameToSearchReplaced = "excel.exe"; //Hardcoded
                if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
            }
            if (sFileNameToSearchReplaced == "" && sProductFound.Contains("powerpoint"))
            {
                sFileNameToSearchReplaced = "ppcore.dll"; //Hardcoded
                if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                sFileNameToSearchReplaced = "powerpnt.exe"; //Hardcoded
                if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);

            }

            if (sFileNameToSearchReplaced == "" && sProductFound.Contains("word viewer"))
            {
                sFileNameToSearchReplaced = "wordview.exe"; //Hardcoded
                if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
            }
            if (sProductFound.Contains("word"))
            {
                sFileNameToSearchReplaced = "winword.exe"; //Hardcoded
                if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
            }
            if (sFileNameToSearchReplaced == "" && sProductFound.Contains("publisher"))
            {
                sFileNameToSearchReplaced = "mspub.exe"; //Hardcoded
                if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
            }
            if (sFileNameToSearchReplaced == "" && (sProductFound.Contains("skype") || sProductFound.Contains("lync")))
            {
                sFileNameToSearchReplaced = "lync.exe"; //Hardcoded     lync99.exe      lynchtmlconv.exe        lmaddins.dll
                if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
            }
            if (sFileNameToSearchReplaced == "" && sProductFound.Contains("access"))
            {
                sFileNameToSearchReplaced = "msaccess.exe"; //Hardcoded
                if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
            }
            if (sFileNameToSearchReplaced == "" && sProductFound.Contains("infopath"))
            {
                sFileNameToSearchReplaced = "infopath.exe"; //Hardcoded
                if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
            }
            if (sFileNameToSearchReplaced == "" && sProductFound.Contains("onenote"))
            {
                sFileNameToSearchReplaced = "onenote.exe"; //Hardcoded
                if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
            }
            if (sProductFound.Contains("project"))
            {
                sFileNameToSearchReplaced = "winproj.exe"; //Hardcoded
                if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
            }
            if (sProductFound.Contains("visio"))
            {
                sFileNameToSearchReplaced = "visio.exe"; //Hardcoded
                if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
            }
            if (sFileNameToSearchReplaced == "" && sProductFound.Contains("office compatibility pack"))// || sProductFound.Contains("pack de compatibilite")))    //Hardcoded + French
            {
                //TODO: was it Excel?
                //xlconv2007-kb3128022-fullfile-x86-glb.exe
                if (sKBSecurityUpdateLower.Contains("xlconv"))
                {
                    sFileNameToSearchReplaced = "xl12cnv.exe"; //Hardcoded
                    if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);

                    sFileNameToSearchReplaced = "excelcnv.exe"; //Hardcoded //excelcnv.exe|excelconv.exe
                    if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                }
                //wordconv2007-kb3128024-fullfile-x86-glb.exe
                if (sKBSecurityUpdateLower.Contains("wordconv"))
                {
                    sFileNameToSearchReplaced = "wrd12cnv.dll"; //Hardcoded
                    if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);

                    sFileNameToSearchReplaced = "wordcnv.dll"; //Hardcoded
                    if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                }
            }
            //Microsoft Office Web Apps 2010 (all versions)
            if (sKBSecurityUpdateLower.Contains("web apps"))
            {
                sFileNameToSearchReplaced = "msoserver.dll"; //Hardcoded
                                                             //sword.dll
                if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                //Wdsrv.conversion.office.msoserver.dll
            }
            if (sFileNameToSearchReplaced == "" && sProductFound.Contains("sharepoint server")) //sharepoint foundation?
            {
                if (sKBSecurityUpdateLower.Contains("xlsrv"))
                {
                    sFileNameToSearchReplaced = "xlsrv.dll";
                    if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                }
                if (sKBSecurityUpdateLower.Contains("wdsrv"))
                {
                    sFileNameToSearchReplaced = "sword.dll";
                    if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                }
                if (sKBSecurityUpdateLower.Contains("word automation"))
                {
                    sFileNameToSearchReplaced = "sword.dll";
                    if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                }
                if (sFileNameToSearchReplaced == "" && oVulnerability.VULDescription.ToLower().Contains("word")) sFileNameToSearchReplaced = "sword.dll";
                //TODO: if other Products Found contains word/excel
                if (sFileNameToSearchReplaced == "")
                {
                    sFileNameToSearchReplaced = "sword.dll";
                    if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
                    //Wdsrv.conversion.sword.dll
                }
                //stswel.dll
                //wwintl.dll
                //vutils.dll
            }
            if (sFileNameToSearchReplaced == "" && sKBSecurityUpdateLower.Contains("mso"))
            {
                sFileNameToSearchReplaced = "mso.dll";
                if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
            }
            //oart.dll
            //oartconv.dll


            //For Word Automation Services on supported editions of Microsoft SharePoint Server 2010 Service Pack 2:
            //msoserver.dll     sword.dll
            //For Excel Services on supported editions of Microsoft SharePoint Server 2010 Service Pack 2:
            //xlsrv.dll

            if (sKBSecurityUpdateLower.Contains("hyperlink object library"))
            {
                sFileNameToSearchReplaced = "hlink.dll";
                if (!lFileNamesToSearch.Contains(sFileNameToSearchReplaced)) lFileNamesToSearch.Add(sFileNameToSearchReplaced);
            }
            if (sKBSecurityUpdateLower.Contains("skype") || sKBSecurityUpdateLower.Contains("lync"))
            {

                if (!lFileNamesToSearch.Contains("lync.lync.exe")) lFileNamesToSearch.Add("lync.lync.exe");
            }

            if (sKBSecurityUpdateLower.Contains("office"))
            {
                if (!lFileNamesToSearch.Contains("wwlibcxm.dll")) lFileNamesToSearch.Add("wwlibcxm.dll");
                if (sFileNameToSearchReplaced == "")
                {
                    sFileNameToSearchReplaced = "usp10.dll";
                }
                if (!lFileNamesToSearch.Contains("usp10.dll")) lFileNamesToSearch.Add("usp10.dll");
            }
            #endregion filenametosearchhardcoded

            return lFileNamesToSearch;
        }
        */

        public static void fStartProcess(string sEXEFilePath, string sArguments)
        {
            try
            {
                if(bDebugDecompression) Console.WriteLine("DEBUG Starting Process: " + sEXEFilePath + " " + sArguments);
                //Process.Start("X:\\SOURCES\\OVALBuilder\\bin\\Release\\OVALBuilder.exe");
                //const string sOVALBuilderArgument = sCVEID;

                // Use ProcessStartInfo class
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.CreateNoWindow = false;
                startInfo.UseShellExecute = false;
                startInfo.FileName = sEXEFilePath;  //"X:\\SOURCES\\OVALBuilder\\bin\\Release\\OVALBuilder.exe";
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.Arguments = sArguments;   // sCVEID;// +" > " + sCVEID + ".txt";
                if (!bDebugDecompression)
                {
                    startInfo.RedirectStandardOutput = true;
                }

                try
                {
                    // Start the process with the info we specified.
                    // Call WaitForExit and then the using statement will close.
                    using (Process exeProcess = Process.Start(startInfo))
                    {
                        // To avoid deadlocks, always read the output stream first and then wait.
                        string output = exeProcess.StandardOutput.ReadToEnd();
                        exeProcess.WaitForExit();
                    }
                }
                catch
                {
                    // Log error.
                }
            }
            catch (Exception exStartProcess)
            {
                Console.WriteLine("Exception: exStartProcess " + exStartProcess.Message + " " + exStartProcess.InnerException);
            }
            //Thread.Sleep(60000); //Hardcoded
        }
    }
}
