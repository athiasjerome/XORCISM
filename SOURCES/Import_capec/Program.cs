using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.IO;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml;

using XORCISMModel;
using XATTACKModel;
using XVULNERABILITYModel;
using XTHREATModel;

//using ICSharpCode.SharpZipLib.Zip;
using System.IO.Compression;

using System.Text.RegularExpressions;

namespace Import_capec
{
    static class Program
    {
        /// <summary>
        /// Copyright (C) 2014-2015 Jerome Athias
        /// Parser for MITRE CAPEC XML file and import the values into an XORCISM database
        /// All trademarks and registered trademarks are the property of their respective owners.
        /// This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation; either version 2 of the License, or (at your option) any later version.
        /// 
        /// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
        /// 
        /// You should have received a copy of the GNU General Public License along with this program; if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
        /// </summary>
        
        [STAThread]
        static void Main()
        {
            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
            XORCISMEntities model= new XORCISMEntities();
            //https://stackoverflow.com/questions/5940225/fastest-way-of-inserting-in-entity-framework
            model.Configuration.AutoDetectChangesEnabled = false;
            model.Configuration.ValidateOnSaveEnabled = false;

            XATTACKEntities attack_model = new XATTACKEntities();
            attack_model.Configuration.AutoDetectChangesEnabled = false;
            attack_model.Configuration.ValidateOnSaveEnabled = false;

            XVULNERABILITYEntities vuln_model = new XVULNERABILITYEntities();
            vuln_model.Configuration.AutoDetectChangesEnabled = false;
            vuln_model.Configuration.ValidateOnSaveEnabled = false;

            XTHREATEntities threat_model = new XTHREATEntities();
            threat_model.Configuration.AutoDetectChangesEnabled = false;
            threat_model.Configuration.ValidateOnSaveEnabled = false;


            int iVocabularyCAPECID = 0;  //CAPEC 4
            string sCAPECversion = "2.8";   //TODO HARDCODED
            //
            #region vocabularycapec
            try
            {
                //HARDCODED
                iVocabularyCAPECID = model.VOCABULARY.Where(o => o.VocabularyName == "CAPEC" && o.VocabularyVersion == sCAPECversion).Select(o => o.VocabularyID).FirstOrDefault();
            }
            catch (Exception ex)
            {

            }
            if (iVocabularyCAPECID <= 0)
            {
                XORCISMModel.VOCABULARY oVocabulary = new XORCISMModel.VOCABULARY();
                
                oVocabulary.CreatedDate = DateTimeOffset.Now;
                oVocabulary.VocabularyName = "CAPEC";   //HARDCODED
                oVocabulary.VocabularyVersion = sCAPECversion;
                model.VOCABULARY.Add(oVocabulary);
                model.SaveChanges();
                iVocabularyCAPECID = oVocabulary.VocabularyID;
                
                Console.WriteLine("DEBUG iVocabularyCAPECID=" + iVocabularyCAPECID);
            }
            #endregion vocabularycapec

            //KillChain for CAPEC
            #region killchain
            int iKillChainCAPECID = 0;
            try
            {
                //HARDCODED
                iKillChainCAPECID = model.KILLCHAIN.Where(o => o.KillChainName == "CAPEC" && o.VocabularyID == iVocabularyCAPECID).Select(o => o.KillChainID).FirstOrDefault();
            }
            catch(Exception exKillChainCAPEC)
            {
                Console.WriteLine("Exception: exKillChainCAPEC " + exKillChainCAPEC.Message + " " + exKillChainCAPEC.InnerException);
            }
            if(iKillChainCAPECID<=0)
            {
                KILLCHAIN oKillChain = new KILLCHAIN();
                oKillChain.CreatedDate = DateTimeOffset.Now;
                oKillChain.KillChainName = "CAPEC"; //HARDCODED
                oKillChain.VocabularyID = iVocabularyCAPECID;
                oKillChain.timestamp = DateTimeOffset.Now;
                model.KILLCHAIN.Add(oKillChain);
                model.SaveChanges();
                iKillChainCAPECID = oKillChain.KillChainID;
            }
            else
            {
                //Update KILLCHAIN
            }
            #endregion killchain

            #region OSILayers
            //string sOSILayerNameInit = "";
            int iOSILayerInitID = 0;
            //HARDCODED
            string[] OSILayers= new string[7]{"Physical Layer", "Data Link Layer", "Network Layer", "Transport Layer", "Session Layer", "Presentation Layer", "Application Layer"};
            for (int i = 1; i < 7; i++)
            {
                try
                {
                    string sTemp = OSILayers[i];
                    iOSILayerInitID = model.OSILAYER.Where(o => o.OSILayerName == sTemp).Select(o => o.OSILayerID).FirstOrDefault();
                }
                catch (Exception exOSILayerInit)
                {
                    Console.WriteLine("Exception exOSILayerInit " + exOSILayerInit.Message + " " + exOSILayerInit.InnerException);
                }
                //if (sOSILayerNameInit==null)
                if (iOSILayerInitID<=0)
                {
                    try
                    {
                        OSILAYER oOSILAYER = new OSILAYER();
                        oOSILAYER.OSILayerID = i;
                        oOSILAYER.OSILayerName = OSILayers[i];
                        //oOSILAYER.CreatedDate = DateTimeOffset.Now;
                        oOSILAYER.VocabularyID = iVocabularyCAPECID;
                        model.OSILAYER.Add(oOSILAYER);
                        model.SaveChanges();
                    }
                    catch (Exception exOSILayerInitAdd)
                    {
                        Console.WriteLine("Exception exOSILayerInitAdd " + exOSILayerInitAdd.Message + " " + exOSILayerInitAdd.InnerException);
                    }
                }
                else
                {
                    
                }
            }
            #endregion OSILayers

            //********************************************************************************************************************************
            //Download file from the feed
            //HARDCODED
            string sDownloadFileName = "capec_v"+sCAPECversion+".zip";    //2.7
            string sDownloadFileURL = "http://capec.mitre.org/data/archive/" + sDownloadFileName;
            string sDownloadLocalPath = "C:/nvdcve/";   //HARDCODED
            string sDownloadLocalFolder = @"C:\nvdcve\";
            string sDownloadLocalFile = "capec_v"+sCAPECversion+".xml";   //2.7
            FileInfo fileInfo = null; //Used to get the local file size
            //FastZip fz = new FastZip(); //Now with .NET
            ////ICSharpCode.SharpZipLib.GZip

            HttpWebRequest webRequest = null;
            HttpWebResponse webResponse = null;
            Int64 fileSizeRemote = new Int64();
            long fileSizeLocal = 0;

            // Create new FileInfo object and get the Length.
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
                Console.WriteLine("Exception exfileSizeLocal " + exfileSizeLocal.Message + " " + exfileSizeLocal.InnerException);
            }

            try
            {
                Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                Console.WriteLine("DEBUG Requesting the size of " + sDownloadFileURL);
                webRequest = (HttpWebRequest)WebRequest.Create(new Uri(sDownloadFileURL));
                webRequest.Method = System.Net.WebRequestMethods.Http.Head;
                //webRequest.Credentials = CredentialCache.DefaultCredentials;
                webResponse = (HttpWebResponse)webRequest.GetResponse();
                fileSizeRemote = webResponse.ContentLength;
                webResponse.Close();
                Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                Console.WriteLine("DEBUG " + sDownloadFileURL + " FileSize:" + fileSizeRemote);
            }
            catch (Exception exGetDownloadFileSize)
            {
                Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                Console.WriteLine("Exception exGetDownloadFileSize " + exGetDownloadFileSize.Message + " " + exGetDownloadFileSize.InnerException);
            }

            if (fileSizeRemote == fileSizeLocal)
            {
                //We don't download the file
            }
            else
            {
                // Download the file
                try
                {
                    WebClient wc = new WebClient();
                    //NOTE: we could ask and use Gzip
                    //http://technet.rapaport.com/info/prices/samplecode/gzip_request_sample.aspx
                    Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                    Console.WriteLine("DEBUG Downloading " + sDownloadFileName);
                    wc.DownloadFile(sDownloadFileURL, sDownloadLocalPath + sDownloadFileName);
                    wc.Dispose();
                    Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                    Console.WriteLine("DEBUG Download is completed");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                    Console.WriteLine("Exception: Error while downloading\n" + ex.Message + " " + ex.InnerException);
                }

                //Extract Zip File
                try
                {
                    //TODO: check if zip file exists
                    //fz.ExtractZip(@"C:\nvdcve\capec_v2.1.zip", @"C:\nvdcve\", "");    //HARDCODED
                    //fz.ExtractZip(sDownloadLocalFolder + sDownloadFileName, sDownloadLocalFolder, "");
                    //Now with .NET 4.5
                    ZipArchive archive = ZipFile.Open(sDownloadLocalFolder + sDownloadFileName, ZipArchiveMode.Read);
                    //fileInfo.Delete();
                    archive.ExtractToDirectory(sDownloadLocalFolder);
                    Console.WriteLine("Extraction Complete !!!");
                }
                catch (Exception exUnzip)
                {
                    Console.WriteLine("Exception exUnzip: " + exUnzip.Message + " " + exUnzip.InnerException);
                }
            }


            XmlDocument doc= new XmlDocument();

            //NOTE: probably not the best/fastest (XmlReader) way to parse XML but easy/clear enough
            //WARNING: slow, should be mutli-threaded...
            
            try
            {
                //TODO SECURITY: validate with XSD...
                //doc.Load(@"C:\nvdcve\capec_v2.7.xml");
                doc.Load(sDownloadLocalFolder + sDownloadLocalFile);
            }
            catch (Exception exdocLoad)
            {
                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                Console.WriteLine("Exception exdocLoad :\n" + exdocLoad.Message + " " + exdocLoad.InnerException);
            }

            XmlNamespaceManager mgr = new XmlNamespaceManager(doc.NameTable);

            mgr.AddNamespace("capec", "http://capec.mitre.org/capec-2");    //Hardcoded

            XmlNodeList nodes1;
            //TODO  capec:Views
            nodes1 = doc.SelectNodes("capec:Attack_Pattern_Catalog/capec:Views/capec:View", mgr);
            #region capecview
            foreach (XmlNode nodeView in nodes1)    //capec:View
            {
                int iCAPECViewID = Int32.Parse(nodeView.Attributes["ID"].InnerText);  //3000
                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                Console.WriteLine("DEBUG CAPECVIEW="+iCAPECViewID);
                string sCAPECViewName = nodeView.Attributes["Name"].InnerText;  //Mechanisms of Attack
                string sCAPECViewStatus = nodeView.Attributes["Status"].InnerText;  //Draft

                ATTACKPATTERNVIEW oAttackPatternView = attack_model.ATTACKPATTERNVIEW.FirstOrDefault(o => o.ViewVocabularyID == iCAPECViewID);
                if (oAttackPatternView==null)
                {
                    oAttackPatternView = new ATTACKPATTERNVIEW();
                    oAttackPatternView.CreatedDate = DateTimeOffset.Now;

                    attack_model.ATTACKPATTERNVIEW.Add(oAttackPatternView);
                }
                //Update ATTACKPATTERNVIEW
                oAttackPatternView.ViewVocabularyID = iCAPECViewID;
                oAttackPatternView.AttackPatternViewName = sCAPECViewName;
                oAttackPatternView.ViewStatus = sCAPECViewStatus;
                oAttackPatternView.timestamp = DateTimeOffset.Now;
                try
                {
                    model.SaveChanges();
                }
                catch(Exception exoAttackPatternView)
                {
                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                    Console.WriteLine("Exception exoAttackPatternView " + exoAttackPatternView.Message + " " + exoAttackPatternView.InnerException);
                }

                foreach (XmlNode node2 in nodeView.ChildNodes)
                {
                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                    Console.WriteLine("DEBUG node2.Name="+node2.Name);
                    switch(node2.Name)
                    {
                        case "capec:View_Structure":
                            oAttackPatternView.View_Structure = node2.InnerText;  //Graph
                            break;
                        case "capec:View_Objective":
                            oAttackPatternView.AttackPatternViewDescription = CleaningCAPECString(node2.InnerText);
                            break;
                        case "capec:View_Filter":
                            oAttackPatternView.View_Filter = node2.InnerText;  //.//@Pattern_Abstraction='Meta'
                            break;
                        case "capec:Relationships":
                            #region ATTACKPATTERNVIEWRELATIONSHIP
                            foreach (XmlNode nodeRelationship in node2.ChildNodes)
                            {
                                switch(nodeRelationship.Name)
                                {
                                    case "capec:Relationship":
                                        //ATTACKPATTERNVIEWRELATIONSHIP oAttackPatternViewRelationship=null;
                                        string sTargetForm=string.Empty;
                                        string sNature=string.Empty;
                                        foreach (XmlNode nodeRelationshipInfo in nodeRelationship.ChildNodes)
                                        {
                                            switch (nodeRelationshipInfo.Name)
                                            {
                                                case "capec:Relationship_Views":
                                                    if(nodeRelationshipInfo.ChildNodes.Count > 1)
                                                    {
                                                        Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                                                        Console.WriteLine("ERROR: Multiple Relationship_View_ID in Relationship_Views");
                                                    }
                                                    /*
                                                    foreach (XmlNode nodeRelationshipInfoView in nodeRelationshipInfo.ChildNodes)
                                                    {
                                                        //<capec:Relationship_View_ID>1000</capec:Relationship_View_ID>
                                                        //TODO NOT NEEDED
                                                    }
                                                    */
                                                    break;

                                                case "capec:Relationship_Target_Form":
                                                    sTargetForm = nodeRelationshipInfo.InnerText;   //Category
                                                    break;

                                                case "capec:Relationship_Nature":
                                                    sNature = nodeRelationshipInfo.InnerText;   //HasMember
                                                    break;

                                                case "capec:Relationship_Target_ID":
                                                    string scapecidTarget = "CAPEC-" + nodeRelationshipInfo.InnerText;  //118
                                                    //Check that the targeted CAPEC exists as an ATTACKPATTERN
                                                    #region attackpattern
                                                    int iAttackPatternTargetedID = 0;
                                                    try
                                                    {
                                                        iAttackPatternTargetedID=attack_model.ATTACKPATTERN.Where(o => o.capec_id == scapecidTarget).Select(o=>o.AttackPatternID).FirstOrDefault();
                                                    }
                                                    catch(Exception ex)
                                                    {

                                                    }
                                                    if (iAttackPatternTargetedID>0)
                                                    {
                                                        //Update ATTACKPATTERN
                                                    }
                                                    else
                                                    {
                                                        try
                                                        {
                                                            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                            Console.WriteLine("DEBUG Adding new ATTACKPATTERN "+scapecidTarget+" found in capec:Relationship_Target_ID");
                                                            ATTACKPATTERN oAttackPatternTargeted = new ATTACKPATTERN();
                                                            oAttackPatternTargeted.capec_id = scapecidTarget;
                                                            oAttackPatternTargeted.VocabularyID = iVocabularyCAPECID;
                                                            oAttackPatternTargeted.CreatedDate = DateTimeOffset.Now;
                                                            oAttackPatternTargeted.timestamp = DateTimeOffset.Now;
                                                            attack_model.ATTACKPATTERN.Add(oAttackPatternTargeted);
                                                            attack_model.SaveChanges();
                                                            iAttackPatternTargetedID = oAttackPatternTargeted.AttackPatternID;
                                                        }
                                                        catch(Exception exoAttackPatternTargeted)
                                                        {
                                                            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                            Console.WriteLine("Exception exoAttackPatternTargeted " + exoAttackPatternTargeted.Message + " " + exoAttackPatternTargeted.InnerException);
                                                        }
                                                    }
                                                    #endregion attackpattern
                                                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                    Console.WriteLine("DEBUG "+scapecidTarget+" iAttackPatternTargetedID=" + iAttackPatternTargetedID);

                                                    //oAttackPatternViewRelationship = attack_model.ATTACKPATTERNVIEWRELATIONSHIP.FirstOrDefault(o => o.AttackPatternViewID == oAttackPatternView.AttackPatternViewID && o.AttackPatternID == iAttackPatternTargetedID);
                                                    int iAttackPatternViewRelationshipID=0;
                                                    try
                                                    {
                                                        //TODO? Test Relationship_Nature ((&& Relationship_Target_Form))
                                                        iAttackPatternViewRelationshipID= attack_model.ATTACKPATTERNVIEWRELATIONSHIP.Where(o => o.AttackPatternViewID == oAttackPatternView.AttackPatternViewID && o.AttackPatternID == iAttackPatternTargetedID).Select(o=>o.AttackPatternViewRelationshipID).FirstOrDefault();
                                                    }
                                                    catch(Exception ex)
                                                    {

                                                    }
                                                    //if (oAttackPatternViewRelationship==null)
                                                    if(iAttackPatternViewRelationshipID<=0)
                                                    {
                                                        try
                                                        {
                                                            ATTACKPATTERNVIEWRELATIONSHIP oAttackPatternViewRelationship = new ATTACKPATTERNVIEWRELATIONSHIP();
                                                            oAttackPatternViewRelationship.CreatedDate = DateTimeOffset.Now;
                                                            oAttackPatternViewRelationship.AttackPatternViewID = oAttackPatternView.AttackPatternViewID;    //View 1000
                                                            oAttackPatternViewRelationship.Relationship_Nature = sNature;   //HasMember
                                                            oAttackPatternViewRelationship.Relationship_Target_Form = sTargetForm;  //Category
                                                            oAttackPatternViewRelationship.AttackPatternID = iAttackPatternTargetedID;  //CAPEC-118
                                                            oAttackPatternViewRelationship.VocabularyID = iVocabularyCAPECID;
                                                            oAttackPatternViewRelationship.timestamp = DateTimeOffset.Now;
                                                            attack_model.ATTACKPATTERNVIEWRELATIONSHIP.Add(oAttackPatternViewRelationship);
                                                            attack_model.SaveChanges();
                                                            //iAttackPatternViewRelationshipID=
                                                        }
                                                        catch(Exception exoAttackPatternViewRelationship)
                                                        {
                                                            Console.WriteLine("Exception exoAttackPatternViewRelationship " + exoAttackPatternViewRelationship.Message + " " + exoAttackPatternViewRelationship.InnerException);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //Update ATTACKPATTERNVIEWRELATIONSHIP
                                                    }
                                                    break;

                                                default:
                                                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                    Console.WriteLine("TODO: Missing code for nodeRelationshipInfo " + nodeRelationshipInfo.Name);
                                                    break;
                                            }
                                        }
                                        break;

                                    default:
                                        Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                        Console.WriteLine("TODO: Missing code for nodeRelationship " + nodeRelationship.Name);
                                        break;
                                }
                            }
                            #endregion ATTACKPATTERNVIEWRELATIONSHIP
                            break;

                        case "capec:Content_History":
                            #region capeccontenthistoryview
                            foreach (XmlNode nodeContentHistory in node2.ChildNodes)
                            {
                                if (nodeContentHistory.Name == "capec:Submissions")
                                {
                                    #region capecviewsubmissions
                                    foreach (XmlNode nodeContentSubmission in nodeContentHistory.ChildNodes)
                                    {
                                        //<capec:Submission Submission_Source="Internal_CAPEC_Team">
                                        if (nodeContentSubmission.Name == "capec:Submission")
                                        {
                                            try
                                            {
                                                //TODO
                                                Console.WriteLine("TODO: ViewSubmission_Source=" + nodeContentSubmission.Attributes["Submission_Source"].InnerText);    //Hardcoded
                                            }
                                            catch (Exception exSubmissionView)
                                            {
                                                Console.WriteLine("Exception: View exSubmissionView No Submission_Source " + exSubmissionView.Message);
                                            }

                                            foreach (XmlNode nodeContentSubmissionInfo in nodeContentSubmission.ChildNodes)
                                            {
                                                switch (nodeContentSubmissionInfo.Name)
                                                {
                                                    case "capec:Submitter":
                                                        //CAPEC Content Team
                                                        Console.WriteLine("TODO: View capec:Submitter " + nodeContentSubmissionInfo.Name + "=" + nodeContentSubmissionInfo.InnerText);
                                                        break;
                                                    case "capec:Submitter_Organization":
                                                        //The MITRE Corporation
                                                        string sOrganisationName = nodeContentSubmissionInfo.InnerText.Trim();
                                                        Console.WriteLine("DEBUG: View capec:Submitter_Organization " + nodeContentSubmissionInfo.Name + "=" + sOrganisationName);
                                                        #region organization
                                                        int iOrganisationID = 0;
                                                        try
                                                        {
                                                            iOrganisationID = model.ORGANISATION.Where(o => o.OrganisationName == sOrganisationName).Select(o => o.OrganisationID).FirstOrDefault();
                                                        }
                                                        catch (Exception exOrganisation)
                                                        {
                                                            Console.WriteLine("Exception: View exOrganisation " + exOrganisation.Message + " " + exOrganisation.InnerException);
                                                        }
                                                        try
                                                        {
                                                            if (iOrganisationID <= 0)
                                                            {
                                                                ORGANISATION oOrganisation = new ORGANISATION();
                                                                oOrganisation.OrganisationName = sOrganisationName; //The MITRE Corporation
                                                                //HARDCODED
                                                                if (sOrganisationName.Contains("GFI")) { oOrganisation.OrganisationKnownAs = "GFI"; }   //GFI Software
                                                                if (sOrganisationName.Contains("MITRE")) { oOrganisation.OrganisationKnownAs = "MITRE"; }
                                                                if (sOrganisationName.Contains("SCAP.com")) { oOrganisation.OrganisationKnownAs = "SCAP"; } //SCAP.com, LLC
                                                                if (sOrganisationName.Contains("ThreatGuard")) { oOrganisation.OrganisationKnownAs = "ThreatGuard"; }   //ThreatGuard, Inc.
                                                                if (sOrganisationName.Contains("Hewlett-Packard")) { oOrganisation.OrganisationKnownAs = "HP"; }    //Hewlett-Packard
                                                                if (sOrganisationName.Contains("Symantec")) { oOrganisation.OrganisationKnownAs = "Symantec"; }    //Symantec Corporation
                                                                if (sOrganisationName.Contains("SecPod")) { oOrganisation.OrganisationKnownAs = "SecPod"; } //SecPod Technologies
                                                                if (sOrganisationName.Contains("Gideon")) { oOrganisation.OrganisationKnownAs = "Gideon"; }   //Gideon Technologies, Inc.
                                                                if (sOrganisationName.Contains("Secure Elements")) { oOrganisation.OrganisationKnownAs = "Secure Elements"; }   //Secure Elements, Inc.
                                                                if (sOrganisationName.Contains("Lumension")) { oOrganisation.OrganisationKnownAs = "Lumension"; }   //Lumension Security, Inc.
                                                                if (sOrganisationName.Contains("McAfee")) { oOrganisation.OrganisationKnownAs = "McAfee"; }  //McAfee, Inc.  (Intel Security)
                                                                if (sOrganisationName.Contains("BigFix")) { oOrganisation.OrganisationKnownAs = "BigFix"; }  //BigFix, Inc
                                                                if (sOrganisationName.Contains("National Institute of Standards and Technology")) { oOrganisation.OrganisationKnownAs = "NIST"; }  //National Institute of Standards and Technology
                                                                if (sOrganisationName.Contains("SAINT")) { oOrganisation.OrganisationKnownAs = "SAINT"; }   //SAINT Corporation
                                                                if (sOrganisationName.Contains("Pivotal")) { oOrganisation.OrganisationKnownAs = "Pivotal"; }   //Pivotal Security LLC
                                                                if (sOrganisationName.Contains("BAE")) { oOrganisation.OrganisationKnownAs = "BAE"; }   //BAE Systems Inc.

                                                                oOrganisation.VocabularyID = iVocabularyCAPECID;
                                                                oOrganisation.CreatedDate = DateTimeOffset.Now;
                                                                oOrganisation.timestamp = DateTimeOffset.Now;
                                                                model.ORGANISATION.Add(oOrganisation);
                                                                model.SaveChanges();
                                                                //iOrganisationID=
                                                            }
                                                            else
                                                            {
                                                                //Update ORGANISATION
                                                            }
                                                        }
                                                        catch (Exception exOrganisation2)
                                                        {
                                                            Console.WriteLine("Exception: View exOrganisation2 " + exOrganisation2.Message + " " + exOrganisation2.InnerException);
                                                        }
                                                        #endregion organization
                                                        break;
                                                    case "capec:Submission_Date":
                                                        //2014-06-23
                                                        Console.WriteLine("TODO: View capec:Submission_Date " + nodeContentSubmissionInfo.Name + "=" + nodeContentSubmissionInfo.InnerText);
                                                        //TODO:
                                                        //ATTACKPATTERN(VIEW).Submission_Date or ATTACKPATTERN(VIEW)REPOSITORY.Submission_Date
                                                        break;
                                                    default:
                                                        Console.WriteLine("ERROR: TODO Missing code for View nodeContentSubmissionInfo " + nodeContentSubmissionInfo.Name);
                                                        break;
                                                }
                                            }
                                        }
                                    }
                                    #endregion capecviewsubmissions
                                }
                                else
                                {
                                    if (nodeContentHistory.Name == "capec:Modifications")
                                    {
                                        #region capecviewmodifications
                                        foreach (XmlNode nodeContentModification in nodeContentHistory.ChildNodes)
                                        {
                                            //<capec:Modification Modification_Source="Internal_CAPEC_Team">
                                            if (nodeContentModification.Name == "capec:Modification")
                                            {
                                                try
                                                {
                                                    //TODO
                                                    Console.WriteLine("TODO: ViewModification_Source=" + nodeContentModification.Attributes["Modification_Source"].InnerText);    //Hardcoded
                                                }
                                                catch (Exception exModificationView)
                                                {
                                                    Console.WriteLine("Exception: View exModificationView No Modification_Source " + exModificationView.Message);
                                                }

                                                foreach (XmlNode nodeContentModificationInfo in nodeContentModification.ChildNodes)
                                                {
                                                    switch (nodeContentModificationInfo.Name)
                                                    {
                                                        case "capec:Modifier":
                                                            //CAPEC Content Team
                                                            Console.WriteLine("TODO: View capec:Modifier " + nodeContentModificationInfo.Name + "=" + nodeContentModificationInfo.InnerText);
                                                            break;
                                                        case "capec:Modifier_Organization":
                                                            //The MITRE Corporation
                                                            string sOrganisationName = nodeContentModificationInfo.InnerText.Trim();
                                                            Console.WriteLine("DEBUG: View capec:Modifier_Organization " + nodeContentModificationInfo.Name + "=" + sOrganisationName);
                                                            #region modifierorganization
                                                            int iOrganisationID = 0;
                                                            try
                                                            {
                                                                iOrganisationID = model.ORGANISATION.Where(o => o.OrganisationName == sOrganisationName).Select(o => o.OrganisationID).FirstOrDefault();
                                                            }
                                                            catch (Exception exModifierOrganisation)
                                                            {
                                                                Console.WriteLine("Exception: View exModifierOrganisation " + exModifierOrganisation.Message + " " + exModifierOrganisation.InnerException);
                                                            }
                                                            try
                                                            {
                                                                if (iOrganisationID <= 0)
                                                                {
                                                                    ORGANISATION oOrganisation = new ORGANISATION();
                                                                    oOrganisation.OrganisationName = sOrganisationName; //The MITRE Corporation
                                                                    //HARDCODED
                                                                    if (sOrganisationName.Contains("GFI")) { oOrganisation.OrganisationKnownAs = "GFI"; }   //GFI Software
                                                                    if (sOrganisationName.Contains("MITRE")) { oOrganisation.OrganisationKnownAs = "MITRE"; }
                                                                    if (sOrganisationName.Contains("SCAP.com")) { oOrganisation.OrganisationKnownAs = "SCAP"; } //SCAP.com, LLC
                                                                    if (sOrganisationName.Contains("ThreatGuard")) { oOrganisation.OrganisationKnownAs = "ThreatGuard"; }   //ThreatGuard, Inc.
                                                                    if (sOrganisationName.Contains("Hewlett-Packard")) { oOrganisation.OrganisationKnownAs = "HP"; }    //Hewlett-Packard
                                                                    if (sOrganisationName.Contains("Symantec")) { oOrganisation.OrganisationKnownAs = "Symantec"; }    //Symantec Corporation
                                                                    if (sOrganisationName.Contains("SecPod")) { oOrganisation.OrganisationKnownAs = "SecPod"; } //SecPod Technologies
                                                                    if (sOrganisationName.Contains("Gideon")) { oOrganisation.OrganisationKnownAs = "Gideon"; }   //Gideon Technologies, Inc.
                                                                    if (sOrganisationName.Contains("Secure Elements")) { oOrganisation.OrganisationKnownAs = "Secure Elements"; }   //Secure Elements, Inc.
                                                                    if (sOrganisationName.Contains("Lumension")) { oOrganisation.OrganisationKnownAs = "Lumension"; }   //Lumension Security, Inc.
                                                                    if (sOrganisationName.Contains("McAfee")) { oOrganisation.OrganisationKnownAs = "McAfee"; }  //McAfee, Inc.  (Intel Security)
                                                                    if (sOrganisationName.Contains("BigFix")) { oOrganisation.OrganisationKnownAs = "BigFix"; }  //BigFix, Inc
                                                                    if (sOrganisationName.Contains("National Institute of Standards and Technology")) { oOrganisation.OrganisationKnownAs = "NIST"; }  //National Institute of Standards and Technology
                                                                    if (sOrganisationName.Contains("SAINT")) { oOrganisation.OrganisationKnownAs = "SAINT"; }   //SAINT Corporation
                                                                    if (sOrganisationName.Contains("Pivotal")) { oOrganisation.OrganisationKnownAs = "Pivotal"; }   //Pivotal Security LLC
                                                                    if (sOrganisationName.Contains("BAE")) { oOrganisation.OrganisationKnownAs = "BAE"; }   //BAE Systems Inc.

                                                                    oOrganisation.VocabularyID = iVocabularyCAPECID;
                                                                    oOrganisation.CreatedDate = DateTimeOffset.Now;
                                                                    oOrganisation.timestamp = DateTimeOffset.Now;
                                                                    model.ORGANISATION.Add(oOrganisation);
                                                                    model.SaveChanges();
                                                                    //iOrganisationID=
                                                                }
                                                                else
                                                                {
                                                                    //Update ORGANISATION
                                                                }
                                                            }
                                                            catch (Exception exModifierOrganisation2)
                                                            {
                                                                Console.WriteLine("Exception: View exModifierOrganisation2 " + exModifierOrganisation2.Message + " " + exModifierOrganisation2.InnerException);
                                                            }
                                                            #endregion modifierorganization
                                                            break;
                                                        case "capec:Modification_Date":
                                                            //2014-06-23
                                                            Console.WriteLine("TODO: View capec:Modification_Date " + nodeContentModificationInfo.Name + "=" + nodeContentModificationInfo.InnerText);
                                                            //TODO:
                                                            //ATTACKPATTERN(VIEW)MODIFICATION.Modification_Date(s) or ATTACKPATTERN(VIEW)REPOSITORYMODIFICATION.Modification_Date(s)
                                                            break;
                                                        case "capec:Modification_Comment":
                                                            //Updated Related_Attack_Patterns
                                                            Console.WriteLine("TODO: View capec:Modification_Comment " + nodeContentModificationInfo.Name + "=" + nodeContentModificationInfo.InnerText);

                                                            break;
                                                        default:
                                                            Console.WriteLine("ERROR: TODO Missing code for View nodeContentModificationInfo " + nodeContentModificationInfo.Name);
                                                            break;
                                                    }
                                                }
                                            }
                                        }
                                        #endregion capecviewmodifications
                                    }
                                    else
                                    {
                                        Console.WriteLine("ERROR: TODO Missing code for View nodeContentHistory " + nodeContentHistory.Name);
                                    }
                                }
                            }
                            #endregion capeccontenthistoryview
                            break;

                        default:
                            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                            Console.WriteLine("ERROR: TODO Missing code for ATTACKPATTERNVIEW " + node2.Name);
                            //capec:Content_History
                                //<capec:Previous_Entry_Names>
                            break;
                    }
                }
            }
            #endregion capecview

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
                Console.WriteLine("Exception DbEntityValidationExceptionGLOBALSAVE " + sb.ToString());
            }
            catch (Exception exGLOBALSAVE)
            {
                Console.WriteLine("Exception exGLOBALSAVE " + exGLOBALSAVE.Message + " " + exGLOBALSAVE.InnerException);
            }
            model.Dispose();

            model = new XORCISMEntities();
            #endregion freememory

            nodes1 = doc.SelectNodes("capec:Attack_Pattern_Catalog/capec:Categories/capec:Category",mgr);
            //Console.WriteLine("DEBUG "+nodes1.Count);

            #region capeccategory
            string mycapecid = string.Empty;
            //string capecidsearch = string.Empty;
            string sCAPECID = string.Empty;
            string description = string.Empty;
            string sCleanDescriptionSummary = string.Empty;
            string sAttackPatternName = string.Empty;

            int iCptNode = 0;
            foreach (XmlNode node in nodes1)    //capec:Category
            {
                iCptNode++;

                //Free memory sometimes (OutOfMemoryException)
                
                if (iCptNode > 30) //TODO Hardcoded    Review
                {
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
                        Console.WriteLine("Exception DbEntityValidationExceptionGLOBALSAVE " + sb.ToString());
                    }
                    catch (Exception exGLOBALSAVE)
                    {
                        Console.WriteLine("Exception exGLOBALSAVE " + exGLOBALSAVE.Message + " " + exGLOBALSAVE.InnerException);
                    }
                    model.Dispose();

                    model = new XORCISMEntities();
                    model.Configuration.AutoDetectChangesEnabled = false;
                    model.Configuration.ValidateOnSaveEnabled = false;

                    #endregion freememory
                    iCptNode = 1;
                }
                


                mycapecid = node.Attributes["ID"].InnerText; //79
                //capecidsearch = "CAPEC-" + mycapecid;    //CAPEC-79
                sCAPECID = "CAPEC-" + mycapecid;
                Console.WriteLine("DEBUG ****************************************************************");
                Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                Console.WriteLine("DEBUG CATEGORY " + sCAPECID);
                
                description = "";
                if (node.ChildNodes[0].Name == "capec:Description")
                {
                    description=node.ChildNodes[0].ChildNodes[0].InnerText;    //Description_Summary   //TODO
                }
                else
                {
                    Console.WriteLine("DEBUG (Error) No Description for " + sCAPECID);  //Pattern_Abstraction="Meta"
                }
                //Console.WriteLine(description);

                //Cleaning
                sCleanDescriptionSummary = CleaningCAPECString(description);


                

                sAttackPatternName = node.Attributes["Name"].InnerText;
                if (sAttackPatternName.Trim() == "")
                {
                    Console.WriteLine("ERROR: Exception CAPEC Name is empty");
                }
                else
                {
                    Console.WriteLine("DEBUG Name=" + sAttackPatternName);
                }

                
                ATTACKPATTERN oAttackPattern = attack_model.ATTACKPATTERN.FirstOrDefault(o => o.capec_id == sCAPECID);
                if(oAttackPattern!=null)
                {
                    //Update ATTACKPATTERN
                    oAttackPattern.category = true;
                    oAttackPattern.AttackPatternName = sAttackPatternName;  // node.Attributes["Name"].InnerText;
                    oAttackPattern.PatternStatus = node.Attributes["Status"].InnerText;   //Draft   Deprecated
                    oAttackPattern.AttackPatternDescription = sCleanDescriptionSummary;
                    oAttackPattern.VocabularyID = iVocabularyCAPECID;
                    oAttackPattern.timestamp = DateTimeOffset.Now;
                    model.SaveChanges();
                }
                else
                {
                    Console.WriteLine(string.Format("DEBUG Adding new ATTACKPATTERN [{0}] in table ATTACKPATTERN", mycapecid));

                    oAttackPattern = new ATTACKPATTERN();
                    oAttackPattern.capec_id = sCAPECID;
                    oAttackPattern.category = true;
                    oAttackPattern.AttackPatternName = sAttackPatternName;  // node.Attributes["Name"].InnerText;
                    oAttackPattern.PatternStatus = node.Attributes["Status"].InnerText;   //Draft
                    //TODO
                    //Pattern_Abstraction
                    //Pattern_Completeness
                    oAttackPattern.AttackPatternDescription = sCleanDescriptionSummary;
                    //oAttackPattern.DescriptionSummary = description;
                    oAttackPattern.VocabularyID = iVocabularyCAPECID;
                    oAttackPattern.CreatedDate = DateTimeOffset.Now;
                    oAttackPattern.timestamp = DateTimeOffset.Now;
                    attack_model.ATTACKPATTERN.Add(oAttackPattern);
                    attack_model.SaveChanges();
                }

                //CWE
                foreach (XmlNode node2 in node.ChildNodes)
                {
                    //Console.WriteLine("DEBUG node2.Name="+node2.Name);
                    //capec:Description
                    //capec:Related_Weaknesses
                    //capec:Attack_Prerequisites
                    //capec:Resources_Required
                    //capec:Relationships
                    //capec:References
                    //capec:Other_Notes
                    //capec:Content_History
                    switch(node2.Name)
                    {
                        case "capec:Description":
                            //Done before
                            break;
                        
                        case "capec:Other_Notes":
                            #region capecnotes
                            int iCptNoteOrder = 0;
                            foreach (XmlNode nodeNote in node2.ChildNodes)  //<capec:Note>
                            {
                                if (nodeNote.Name.ToLower() != "capec:note")
                                {
                                    Console.WriteLine("ERROR: Missing code for nodeNote.Name=" + nodeNote.Name);
                                }
                                else
                                {
                                    iCptNoteOrder++;
                                    string sNoteText=CleaningCAPECString(nodeNote.InnerText);
                                    int iAttackPatternNoteID = 0;
                                    try
                                    {
                                        iAttackPatternNoteID = attack_model.ATTACKPATTERNNOTE.Where(o => o.NoteText == sNoteText).Select(o => o.AttackPatternNoteID).FirstOrDefault();
                                    }
                                    catch(Exception ex)
                                    {

                                    }
                                    if(iAttackPatternNoteID<=0)
                                    {
                                        ATTACKPATTERNNOTE oAttackPatternNote = new ATTACKPATTERNNOTE();
                                        oAttackPatternNote.CreatedDate = DateTimeOffset.Now;
                                        oAttackPatternNote.NoteText = sNoteText;
                                        oAttackPatternNote.VocabularyID = iVocabularyCAPECID;
                                        oAttackPatternNote.timestamp = DateTimeOffset.Now;
                                        attack_model.ATTACKPATTERNNOTE.Add(oAttackPatternNote);
                                        attack_model.SaveChanges();
                                        iAttackPatternNoteID = oAttackPatternNote.AttackPatternNoteID;
                                    }
                                    else
                                    {
                                        //Update ATTACKPATTERNNOTE
                                    }

                                    int iAttackPatternNotesID = 0;
                                    try
                                    {
                                        iAttackPatternNotesID = attack_model.ATTACKPATTERNNOTES.Where(o => o.AttackPatternID == oAttackPattern.AttackPatternID && o.AttackPatternNoteID == iAttackPatternNoteID).Select(o => o.AttackPatternNotesID).FirstOrDefault();
                                    }
                                    catch(Exception ex)
                                    {

                                    }
                                    if(iAttackPatternNotesID<=0)
                                    {
                                        ATTACKPATTERNNOTES oAttackPatternNotes = new ATTACKPATTERNNOTES();
                                        oAttackPatternNotes.CreatedDate = DateTimeOffset.Now;
                                        oAttackPatternNotes.AttackPatternID = oAttackPattern.AttackPatternID;
                                        oAttackPatternNotes.AttackPatternNoteID = iAttackPatternNoteID;
                                        oAttackPatternNotes.NoteOrder = iCptNoteOrder;
                                        oAttackPatternNotes.VocabularyID = iVocabularyCAPECID;
                                        oAttackPatternNotes.timestamp = DateTimeOffset.Now;
                                        attack_model.ATTACKPATTERNNOTES.Add(oAttackPatternNotes);
                                        //attack_model.SaveChanges();    //TEST PERFORMANCE
                                        //iAttackPatternNotesID=
                                    }
                                    else
                                    {
                                        //Update ATTACKPATTERNNOTES
                                        //iCptNoteOrder?
                                    }
                                }
                            }
                            #endregion capecnotes
                            break;

                        case "capec:Related_Weaknesses":
                            #region relatedweakness
                            foreach (XmlNode nodeWeakness in node2.ChildNodes)  //capec:Related_Weakness
                            {
                                string mycweid = "";
                                string sCWEID="";
                                string myrelationship = "";
                                //Console.WriteLine("DEBUG nodeWeakness.Name="+nodeWeakness.Name);   //capec:Related_Weakness
                                foreach (XmlNode node3 in nodeWeakness.ChildNodes)
                                {
                                    //Console.WriteLine("DEBUG node3.Name="+node3.Name);
                                    switch(node3.Name)
                                    {
                                        case "capec:CWE_ID":
                                            mycweid = node3.InnerText;
                                            sCWEID="CWE-" + mycweid;
                                            //Check if the CWE exists
                                            string sTestCWEID = "";
                                            try
                                            {
                                                sTestCWEID = model.CWE.Where(o => o.CWEID == sCWEID).Select(o=>o.CWEID).FirstOrDefault();
                                            }
                                            catch(Exception ex)
                                            {
                                                sTestCWEID = "";
                                            }
                                            if (sTestCWEID=="")
                                            {
                                                CWE oCWE = new CWE();
                                                oCWE.CreatedDate = DateTimeOffset.Now;
                                                oCWE.CWEID = sCWEID;
                                                oCWE.VocabularyID = iVocabularyCAPECID;
                                                oCWE.timestamp = DateTimeOffset.Now;
                                                model.CWE.Add(oCWE);
                                                model.SaveChanges();
                                            }
                                            break;

                                        case "capec:Weakness_Relationship_Type":
                                    
                                            #region capecweakness
                                            myrelationship = node3.InnerText;
                                            //Console.WriteLine("DEBUG "+mycweid);
                                            //Console.WriteLine("DEBUG "+myrelationship);

                                            //Replaced by ATTACKPATTERNCWE
                                            /*
                                            XORCISMModel.CWEFORCAPEC mycweforcapec;
                                            mycweforcapec = model.CWEFORCAPEC.FirstOrDefault(o => o.capec_id == "CAPEC-" + mycapecid && o.CWEID == "CWE-"+mycweid);
                                            if (mycweforcapec == null)
                                            {
                                                Console.WriteLine(string.Format("DEBUG Adding new CWEFORCAPEC [{0}] in table CWEFORCAPEC", mycapecid));
                                                try
                                                {
                                                    mycweforcapec = new CWEFORCAPEC();
                                                    mycweforcapec.capec_id = "CAPEC-" + mycapecid;
                                                    mycweforcapec.CWEID = "CWE-" + mycweid;
                                                    mycweforcapec.WeaknessRelationship = myrelationship;
                                                    mycweforcapec.VocabularyID = iVocabularyCAPECID;
                                                    mycweforcapec.CreatedDate = DateTimeOffset.Now;
                                                    mycweforcapec.timestamp = DateTimeOffset.Now;
                                                    model.CWEFORCAPEC.Add(mycweforcapec);
                                                    model.SaveChanges();
                                                }
                                                catch(Exception exmycweforcapec)
                                                {
                                                    Console.WriteLine("Exception exmycweforcapec " + exmycweforcapec.Message + " " + exmycweforcapec.InnerException);
                                                }
                                            }
                                            else
                                            {
                                                //Update CWEFORCAPEC
                                            }
                                            */

                                            int iAttackPatternCWEID = 0;
                                            try
                                            {

                                            }
                                            catch(Exception ex)
                                            {
                                                iAttackPatternCWEID = attack_model.ATTACKPATTERNCWE.Where(o => o.AttackPatternID == oAttackPattern.AttackPatternID && o.CWEID == sCWEID).Select(o=>o.AttackPatternCWEID).FirstOrDefault();
                                            }
                                            if(iAttackPatternCWEID<=0)
                                            {
                                                
                                                ATTACKPATTERNCWE oAttackPatternCWE = new ATTACKPATTERNCWE();
                                                oAttackPatternCWE.CreatedDate = DateTimeOffset.Now;
                                                oAttackPatternCWE.AttackPatternID = oAttackPattern.AttackPatternID;
                                                oAttackPatternCWE.CWEID = sCWEID;
                                                //Relationship  //Review
                                                oAttackPatternCWE.VocabularyID = iVocabularyCAPECID;
                                                oAttackPatternCWE.timestamp = DateTimeOffset.Now;
                                                attack_model.ATTACKPATTERNCWE.Add(oAttackPatternCWE);
                                                attack_model.SaveChanges();
                                                //iAttackPatternCWEID=
                                            }
                                            else
                                            {
                                                //Update ATTACKPATTERNCWE
                                            }
                                            #endregion capecweakness
                                            break;

                                        default:
                                            Console.WriteLine("ERROR: Missing code for node3.Name="+node3.Name);
                                            break;
                                    }
                                }
                            }
                            #endregion relatedweakness
                            break;

                        case "capec:Attack_Prerequisites":
                            #region AttackPrerequisite
                            foreach (XmlNode nodeAttackPrereq in node2.ChildNodes)  //capec:Attack_Prerequisite
                            {
                                //Console.WriteLine(nodeAttackPrereq.Name);
                                foreach (XmlNode node4 in nodeAttackPrereq.ChildNodes)
                                {
                                    //Console.WriteLine(node4.Name);
                                    //capec:Text
                                    if (node4.Name == "capec:Text")
                                    {
                                        //Cleaning
                                        string sCleanPrerequisiteText = CleaningCAPECString(node4.InnerText);
                                    
                                        //XORCISMModel.ATTACKPREREQUISITE myattackpreq;
                                        //myattackpreq = attack_model.ATTACKPREREQUISITE.FirstOrDefault(o => o.PrerequisiteText == sCleanPrerequisiteText);    // && o.VocabularyID == 4);
                                        int iAttackPrerequisiteID = 0;
                                        try
                                        {
                                            iAttackPrerequisiteID = attack_model.ATTACKPREREQUISITE.Where(o => o.PrerequisiteText == sCleanPrerequisiteText).Select(o => o.AttackPrerequisiteID).FirstOrDefault();
                                        }
                                        catch(Exception ex)
                                        {

                                        }
                                        //if (myattackpreq == null)
                                        if (iAttackPrerequisiteID<=0)
                                        {
                                            Console.WriteLine(string.Format("DEBUG Adding new ATTACKPREREQUISITE [{0}] in table ATTACKPREREQUISITE", mycapecid));
                                            
                                            try
                                            {
                                                ATTACKPREREQUISITE myattackpreq = new ATTACKPREREQUISITE();
                                                myattackpreq.PrerequisiteText = sCleanPrerequisiteText;
                                                //myattackpreq.PrerequisiteTextRaw = node4.InnerText;
                                                myattackpreq.VocabularyID = iVocabularyCAPECID;
                                                myattackpreq.CreatedDate = DateTimeOffset.Now;
                                                myattackpreq.timestamp = DateTimeOffset.Now;
                                                attack_model.ATTACKPREREQUISITE.Add(myattackpreq);

                                                attack_model.SaveChanges();
                                                iAttackPrerequisiteID = myattackpreq.AttackPrerequisiteID;
                                            }
                                            catch (Exception exAddToATTACKPREREQUISITE)
                                            {
                                                Console.WriteLine("Exception exAddToATTACKPREREQUISITE " + exAddToATTACKPREREQUISITE.Message + " " + exAddToATTACKPREREQUISITE.InnerException);
                                            }
                                        }
                                        else
                                        {
                                            //Updating ATTACKPREREQUISITE
                                            /*
                                            //Console.WriteLine("Updating ATTACKPREREQUISITE " + myattackpreq.AttackPrerequisiteID);
                                        
                                            myattackpreq.PrerequisiteText = sCleanPrerequisiteText;
                                            //myattackpreq.PrerequisiteTextRaw = node4.InnerText;
                                            myattackpreq.timestamp = DateTimeOffset.Now;
                                            */
                                        }
                                        /*
                                        try
                                        {
                                            model.SaveChanges();
                                        }
                                        catch (Exception exATTACKPREREQUISITE)
                                        {
                                            Console.WriteLine("Exception exATTACKPREREQUISITE " + exATTACKPREREQUISITE.Message + " " + exATTACKPREREQUISITE.InnerException);
                                        }
                                        */

                                        //XORCISMModel.ATTACKPREREQUISITEFORATTACKPATTERN myattackpreqforcapec;
                                        //myattackpreqforcapec = attack_model.ATTACKPREREQUISITEFORATTACKPATTERN.FirstOrDefault(o => o.capec_id == sCAPECID && o.AttackPrerequisiteID == iAttackPrerequisiteID);    // && o.VocabularyID == 4);
                                        int iAttackPatternPrerequisiteID = 0;
                                        try
                                        {
                                            iAttackPatternPrerequisiteID = attack_model.ATTACKPREREQUISITEFORATTACKPATTERN.Where(o => o.AttackPatternID == oAttackPattern.AttackPatternID && o.AttackPrerequisiteID == iAttackPrerequisiteID).Select(o => o.AttackPatternAttackPrerequisiteID).FirstOrDefault();
                                        }
                                        catch(Exception ex)
                                        {

                                        }
                                        //if (myattackpreqforcapec == null)
                                        if (iAttackPatternPrerequisiteID<=0)
                                        {
                                            Console.WriteLine(string.Format("DEBUG Adding new ATTACKPREREQUISITEFORATTACKPATTERN [{0}] in table ATTACKPREREQUISITEFORATTACKPATTERN", mycapecid));
                                        
                                            try
                                            {
                                                ATTACKPREREQUISITEFORATTACKPATTERN myattackpreqforcapec = new ATTACKPREREQUISITEFORATTACKPATTERN();
                                                myattackpreqforcapec.AttackPatternID = oAttackPattern.AttackPatternID;
                                                //myattackpreqforcapec.capec_id = sCAPECID;   //Removed
                                                myattackpreqforcapec.AttackPrerequisiteID = iAttackPrerequisiteID;  // myattackpreq.AttackPrerequisiteID;
                                                myattackpreqforcapec.CreatedDate = DateTimeOffset.Now;
                                                myattackpreqforcapec.timestamp = DateTimeOffset.Now;
                                                myattackpreqforcapec.VocabularyID = iVocabularyCAPECID;
                                                attack_model.ATTACKPREREQUISITEFORATTACKPATTERN.Add(myattackpreqforcapec);

                                                attack_model.SaveChanges();
                                                //iAttackPatternPrerequisiteID=
                                            }
                                            catch (Exception exAddToATTACKPREREQUISITEFORCAPEC)
                                            {
                                                Console.WriteLine("Exception exAddToATTACKPREREQUISITEFORCAPEC " + exAddToATTACKPREREQUISITEFORCAPEC.Message + " " + exAddToATTACKPREREQUISITEFORCAPEC.InnerException);
                                            }
                                        }
                                        else
                                        {
                                            //Update ATTACKPREREQUISITEFORATTACKPATTERN
                                        }
                                    }
                                }
                            }
                            #endregion AttackPrerequisite
                            break;

                        case "capec:Resources_Required":
                            #region resourcesrequired
                            foreach (XmlNode nodeResReq in node2.ChildNodes)  //capec:Resources_Required
                            {
                            
                                    //Console.WriteLine(node4.Name);
                                    //capec:Text
                                    if (nodeResReq.Name == "capec:Text")
                                    {
                                        //Cleaning
                                        string sCleanAttackResourceText = CleaningCAPECString(nodeResReq.InnerText);
                                    
                                        //XORCISMModel.ATTACKRESOURCE myattackres;
                                        //myattackres = attack_model.ATTACKRESOURCE.FirstOrDefault(o => o.AttackResourceText == sCleanAttackResourceText);    // && o.VocabularyID == 4);
                                        int iAttackResourceID = 0;
                                        try
                                        {
                                            iAttackResourceID = attack_model.ATTACKRESOURCE.Where(o => o.AttackResourceText == sCleanAttackResourceText).Select(o => o.AttackResourceID).FirstOrDefault();
                                        }
                                        catch(Exception ex)
                                        {

                                        }
                                        //if (myattackres == null)
                                        if (iAttackResourceID<=0)
                                        {
                                            Console.WriteLine(string.Format("DEBUG Adding new ATTACKRESOURCE [{0}] in table ATTACKRESOURCE", mycapecid));
                                            ATTACKRESOURCE myattackres = new ATTACKRESOURCE();
                                            myattackres.AttackResourceText = sCleanAttackResourceText;
                                            //myattackres.AttackResourceTextRaw = nodeResReq.InnerText;
                                            myattackres.VocabularyID = iVocabularyCAPECID;
                                            myattackres.CreatedDate = DateTimeOffset.Now;
                                            myattackres.timestamp = DateTimeOffset.Now;
                                            attack_model.ATTACKRESOURCE.Add(myattackres);

                                            attack_model.SaveChanges();
                                            iAttackResourceID = myattackres.AttackResourceID;
                                        }
                                        /*
                                        else
                                        {
                                            //Update ATTACKRESOURCE
                                            myattackres.AttackResourceText = sCleanAttackResourceText;
                                            myattackres.timestamp = DateTimeOffset.Now;
                                        }
                                        try
                                        {
                                            model.SaveChanges();
                                        }
                                        catch(Exception exmyattackres)
                                        {
                                            Console.WriteLine("Exception exmyattackres " + exmyattackres.Message + " " + exmyattackres.InnerException);
                                        }
                                        */

                                        //XORCISMModel.ATTACKRESOURCEFORATTACKPATTERN myattackresforcapec;
                                        //myattackresforcapec = attack_model.ATTACKRESOURCEFORATTACKPATTERN.FirstOrDefault(o => o.capec_id == sCAPECID && o.AttackResourceID == iAttackResourceID);    // && o.VocabularyID == 4);
                                        int iAttackPatternResourceID = 0;
                                        try
                                        {
                                            iAttackPatternResourceID = attack_model.ATTACKRESOURCEFORATTACKPATTERN.Where(o => o.AttackPatternID == oAttackPattern.AttackPatternID && o.AttackResourceID == iAttackResourceID).Select(o=>o.AttackPatternAttackResourceRequiredID).FirstOrDefault();
                                        }
                                        catch(Exception ex)
                                        {

                                        }
                                        //if (myattackresforcapec == null)
                                        if (iAttackPatternResourceID<=0)
                                        {
                                            Console.WriteLine(string.Format("DEBUG Adding new ATTACKRESOURCEFORCAPEC [{0}] in table ATTACKRESOURCEFORCAPEC", mycapecid));
                                            try
                                            {
                                                ATTACKRESOURCEFORATTACKPATTERN myattackresforcapec = new ATTACKRESOURCEFORATTACKPATTERN();
                                                myattackresforcapec.CreatedDate = DateTimeOffset.Now;
                                                myattackresforcapec.AttackPatternID = oAttackPattern.AttackPatternID;
                                                //myattackresforcapec.capec_id = sCAPECID;    //Removed
                                                myattackresforcapec.AttackResourceID = iAttackResourceID;   // myattackres.AttackResourceID;
                                                myattackresforcapec.timestamp = DateTimeOffset.Now;
                                                myattackresforcapec.VocabularyID = iVocabularyCAPECID;
                                                attack_model.ATTACKRESOURCEFORATTACKPATTERN.Add(myattackresforcapec);
                                                attack_model.SaveChanges();
                                                //iAttackPatternResourceID=
                                            }
                                            catch(Exception exmyattackresforcapec)
                                            {
                                                Console.WriteLine("Exception exmyattackresforcapec " + exmyattackresforcapec.Message + " " + exmyattackresforcapec.InnerException);
                                            }
                                        }
                                        else
                                        {
                                            //Update ATTACKRESOURCEFORATTACKPATTERN (ATTACKRESOURCEFORCAPEC)
                                        }
                                    }

                            }
                            #endregion resourcesrequired
                            break;

                        case "capec:Relationships":
                            #region relationships
                            foreach (XmlNode nodeRelation in node2.ChildNodes)  //capec:Relationship
                            {
                                string sTargetForm = "";
                                string sNature = "";
                                //Console.WriteLine(nodeRelation.Name);   //capec:Relationship
                                foreach (XmlNode node3 in nodeRelation.ChildNodes)
                                {
                                    //Relationship_Views    //TODO
                                    if (node3.Name == "capec:Relationship_Target_Form")
                                    {
                                        sTargetForm = node3.InnerText;
                                        //Category
                                        //Attack Pattern
                                    }
                                    if (node3.Name == "capec:Relationship_Nature")
                                    {
                                        sNature = node3.InnerText;
                                        //ChildOf
                                        //HasMember
                                    }
                                    if (node3.Name == "capec:Relationship_Target_ID")
                                    {
                                        string scapecidTarget = "CAPEC-" + node3.InnerText;
                                        //Check that the targeted CAPEC exists as an ATTACKPATTERN
                                        int iAttackPatternTargetedID = attack_model.ATTACKPATTERN.Where(o => o.capec_id == scapecidTarget).Select(o => o.AttackPatternID).FirstOrDefault();
                                        if (iAttackPatternTargetedID>0)
                                        {
                                            //Update ATTACKPATTERN
                                        }
                                        else
                                        {
                                            try
                                            {
                                                Console.WriteLine("DEBUG Adding new ATTACKPATTERN "+scapecidTarget+" found in capec:Relationship_Target_ID");
                                                ATTACKPATTERN oAttackPatternTargeted = new ATTACKPATTERN();
                                                oAttackPatternTargeted.capec_id = scapecidTarget;
                                                oAttackPatternTargeted.VocabularyID = iVocabularyCAPECID;
                                                oAttackPatternTargeted.CreatedDate = DateTimeOffset.Now;
                                                oAttackPatternTargeted.timestamp = DateTimeOffset.Now;
                                                attack_model.ATTACKPATTERN.Add(oAttackPatternTargeted);
                                                attack_model.SaveChanges();
                                                iAttackPatternTargetedID = oAttackPatternTargeted.AttackPatternID;
                                            }
                                            catch(Exception exoAttackPatternTargeted)
                                            {
                                                Console.WriteLine("Exception exoAttackPatternTargeted " + exoAttackPatternTargeted.Message + " " + exoAttackPatternTargeted.InnerException);
                                            }
                                        }
                                        Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                        Console.WriteLine("DEBUG "+scapecidTarget+" iAttackPatternTargetedID=" + iAttackPatternTargetedID);

                                        //Replaced by ATTACKPATTERN
                                        /*
                                        //Check if the related (targeted) CAPEC already exists, just in case
                                        CAPEC oCAPECTarget = model.CAPEC.FirstOrDefault(o => o.capec_id == scapecidTarget);
                                        if (oCAPECTarget != null)
                                        {
                                            //Update CAPEC
                                        }
                                        else
                                        {
                                            try
                                            {
                                                oCAPECTarget = new CAPEC();
                                                oCAPECTarget.capec_id = scapecidTarget;
                                                //TODO
                                                oCAPECTarget.CreatedDate = DateTimeOffset.Now;
                                                oCAPECTarget.timestamp = DateTimeOffset.Now;
                                                oCAPECTarget.VocabularyID = iVocabularyCAPECID;
                                                model.CAPEC.Add(oCAPECTarget);
                                                model.SaveChanges();
                                            }
                                            catch (Exception exoCAPECTarget)
                                            {
                                                Console.WriteLine("Exception exoCAPECTarget " + exoCAPECTarget.Message + " " + exoCAPECTarget.InnerException);
                                            }
                                        }
                                        */


                                        //Check that the relationship exists between the 2 CAPEC
                                        //if (sTargetForm == "Category")
                                        //{
                                            //if (mycapecid != "126" && mycapecid != "224" && mycapecid != "278")  //Because error    TODO HARCODED
                                            try
                                            {
                                                //XORCISMModel.ATTACKPATTERNRELATIONSHIP mycapecrel;
                                                //mycapecrel = attack_model.ATTACKPATTERNRELATIONSHIP.FirstOrDefault(o => o.AttackPatternRefID == oAttackPattern.AttackPatternID && o.AttackPatternSubjectID == iAttackPatternTargetedID);    // && o.VocabularyID == 4);
                                                int iAttackPatternRelationshipID = 0;
                                                try
                                                {
                                                    //Check also sNature
                                                    iAttackPatternRelationshipID = attack_model.ATTACKPATTERNRELATIONSHIP.Where(o => o.AttackPatternRefID == oAttackPattern.AttackPatternID && o.AttackPatternSubjectID == iAttackPatternTargetedID && o.RelationshipName == sNature).Select(o => o.AttackPatternRelationshipID).FirstOrDefault();
                                                }
                                                catch(Exception ex)
                                                {

                                                }
                                                //if (mycapecrel == null)
                                                if (iAttackPatternRelationshipID<=0)
                                                {
                                                    Console.WriteLine(string.Format("DEBUG Adding new ATTACKPATTERNRELATIONSHIP [CAPEC-{0}] target [{1}] in table ATTACKPATTERNRELATIONSHIP", mycapecid, scapecidTarget));
                                                    Console.WriteLine("DEBUG CAPEC-" + mycapecid + " oAttackPattern.AttackPatternID=" + oAttackPattern.AttackPatternID);
                                                    try
                                                    {
                                                        ATTACKPATTERNRELATIONSHIP mycapecrel = new ATTACKPATTERNRELATIONSHIP();
                                                        //
                                                        ////mycapecrel.capec_id = capecidsearch;
                                                        //mycapecrel.RelationshipNature = sNature;
                                                        mycapecrel.RelationshipName = sNature;
                                                        //mycapecrel.RelationshipTargetForm = sTargetForm;
                                                        //mycapecrel.RelationshipTargetID = scapecidTarget;
                                                        mycapecrel.AttackPatternRefID = oAttackPattern.AttackPatternID;
                                                        mycapecrel.AttackPatternSubjectID = iAttackPatternTargetedID;
                                                        mycapecrel.CreatedDate = DateTimeOffset.Now;
                                                        mycapecrel.timestamp = DateTimeOffset.Now;
                                                        mycapecrel.VocabularyID = iVocabularyCAPECID;
                                                        attack_model.ATTACKPATTERNRELATIONSHIP.Add(mycapecrel);
                                                        attack_model.SaveChanges();
                                                        //iAttackPatternRelationshipID=
                                                    }
                                                    catch (Exception exAddToCAPECRELATIONSHIP)
                                                    {
                                                        Console.WriteLine("Exception exAddToCAPECRELATIONSHIP " + exAddToCAPECRELATIONSHIP.Message + " " + exAddToCAPECRELATIONSHIP.InnerException);
                                                    }
                                                }
                                                else
                                                {
                                                    //Update ATTACKPATTERNRELATIONSHIP
                                                }
                                            }
                                            catch (Exception exRelationship_Target_ID)
                                            {
                                                Console.WriteLine("DEBUG mycapecid=" + mycapecid);
                                                Console.WriteLine("Exception exRelationship_Target_ID " + exRelationship_Target_ID.Message + " " + exRelationship_Target_ID.InnerException);
                                                //See before mycapecid != "126" && mycapecid != "224" && mycapecid != "278"

                                            }
                                        //}
                                        //if (sTargetForm == "Attack Pattern")
                                        //{
                                            //TODO

                                        //}
                                    }

                                }
                            }
                            #endregion relationships
                            break;
                        case "capec:References":
                            #region ATTACKPATTERNREFERENCEs
                            foreach (XmlNode nodeRef in node2.ChildNodes)  //capec:Reference
                            {
                                string sRefID = "";
                                try
                                {
                                    sRefID = nodeRef.Attributes["Reference_ID"].InnerText;
                                }
                                catch (Exception ex)
                                {
                                    string sIgnoreWarning = ex.Message;
                                    sRefID = "";
                                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                    Console.WriteLine("DEBUG Note: ATTACKPATTERNREFERENCE without Reference_ID for " + sCAPECID);
                                }

                                string sLocalRefID = "";
                                try
                                {
                                    sLocalRefID = nodeRef.Attributes["Local_Reference_ID"].InnerText;
                                }
                                catch (Exception ex)
                                {
                                    string sIgnoreWarning = ex.Message;
                                    sLocalRefID = "";
                                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                    Console.WriteLine("NOTE: ATTACKPATTERNREFERENCE without Local_Reference_ID for " + sCAPECID);
                                }

                                //TODO: Test if other attributes

                                REFERENCE oReference = null;
                                string sReference_Edition = "";
                                string sReference_Publisher = "";
                                string sReference_Publication = "";
                                string sReference_PubDate = "";
                                string sReference_Date = "";

                                ATTACKPATTERNREFERENCE mycapecref = new ATTACKPATTERNREFERENCE();
                                mycapecref = attack_model.ATTACKPATTERNREFERENCE.FirstOrDefault(o => o.AttackPatternID == oAttackPattern.AttackPatternID && o.Reference_ID == sRefID && o.Local_Reference_ID == sLocalRefID);    // && o.VocabularyID == 4);
                                if (mycapecref == null)
                                {
                                    Console.WriteLine(string.Format("DEBUG Adding new ATTACKPATTERNREFERENCE for [{0}] in table ATTACKPATTERNREFERENCE", sCAPECID));
                                    try
                                    {
                                        mycapecref = new ATTACKPATTERNREFERENCE();
                                        mycapecref.AttackPatternID = oAttackPattern.AttackPatternID;
                                        //NOTE: We still don't know the REFERENCE, so no ReferenceID
                                        mycapecref.Reference_ID = sRefID;
                                        mycapecref.Local_Reference_ID = sLocalRefID;

                                        mycapecref.CreatedDate = DateTimeOffset.Now;
                                        mycapecref.timestamp = DateTimeOffset.Now;
                                        mycapecref.VocabularyID = iVocabularyCAPECID;
                                        attack_model.ATTACKPATTERNREFERENCE.Add(mycapecref);
                                        attack_model.SaveChanges();
                                        Console.WriteLine("DEBUG Added new ATTACKPATTERNREFERENCE");
                                    }
                                    catch (Exception exAddATTACKPATTERNREFERENCE)
                                    {
                                        Console.WriteLine("Exception exAddATTACKPATTERNREFERENCE " + exAddATTACKPATTERNREFERENCE.Message + " " + exAddATTACKPATTERNREFERENCE.InnerException);
                                    }
                                }
                                else
                                {
                                    //Update ATTACKPATTERNREFERENCE

                                    //TODO (Remove? to optimize speed)

                                    try
                                    {
                                        mycapecref.Reference_ID = sRefID;
                                        mycapecref.Local_Reference_ID = sLocalRefID;
                                        mycapecref.timestamp = DateTimeOffset.Now;
                                        model.SaveChanges();
                                    }
                                    catch (Exception exmycapecrefUPDATE01)
                                    {
                                        Console.WriteLine("Exception exmycapecrefUPDATE01 " + exmycapecrefUPDATE01.Message + " " + exmycapecrefUPDATE01.InnerException);
                                    }

                                    oReference = model.REFERENCE.FirstOrDefault(o => o.ReferenceID == mycapecref.ReferenceID);
                                    /*
                                    int iReferenceID = 0;
                                    try
                                    {
                                        iReferenceID = model.REFERENCE.FirstOrDefault(o => o.ReferenceID == mycapecref.ReferenceID).ReferenceID;
                                    }
                                    catch(Exception ex)
                                    {

                                    }
                                    */
                                    if (oReference == null && !sAttackPatternName.ToLower().Contains("deprecated")) //HARDCODED
                                    {
                                        Console.WriteLine("ERROR: Reference was not found for ATTACKPATTERNREFERENCE for " + sCAPECID);
                                        Console.WriteLine("DEBUG mycapecref.AttackPatternReferenceID=" + mycapecref.AttackPatternReferenceID);
                                        Console.WriteLine("DEBUG Reference_ID sRefID=" + sRefID);
                                        Console.WriteLine("DEBUG Local_Reference_ID sLocalRefID=" + sLocalRefID);
                                    }
                                    else
                                    {
                                        //Update REFERENCE
                                    }

                                }

                                foreach (XmlNode nodeRefAtt in nodeRef.ChildNodes)
                                {
                                    Console.WriteLine("DEBUG nodeRefAtt.Name=" + nodeRefAtt.Name);
                                    try
                                    {
                                        switch (nodeRefAtt.Name)
                                        {
                                            case "capec:Reference_Author":
                                                string sRefAuthor = CleaningCAPECString(nodeRefAtt.InnerText);
                                                //AUTHOR myrefauthor = new AUTHOR();
                                                int iAuthorID = 0;
                                                try
                                                {
                                                    model.AUTHOR.Where(o => o.AuthorName == sRefAuthor).Select(o => o.AuthorID).FirstOrDefault();
                                                }
                                                catch (Exception ex)
                                                {

                                                }
                                                //myrefauthor = model.AUTHOR.FirstOrDefault(o => o.AuthorName == sRefAuthor);
                                                //if (myrefauthor == null)
                                                if (iAuthorID <= 0)
                                                {
                                                    Console.WriteLine(string.Format("DEBUG Adding new AUTHOR found in References of [{0}] in table AUTHOR", sCAPECID));
                                                    AUTHOR myrefauthor = new AUTHOR();
                                                    myrefauthor.AuthorName = CleaningCAPECString(sRefAuthor);
                                                    //TODO: check for PersonID in PERSON or OrganisationID
                                                    myrefauthor.CreatedDate = DateTimeOffset.Now;
                                                    myrefauthor.timestamp = DateTimeOffset.Now;
                                                    myrefauthor.VocabularyID = iVocabularyCAPECID;
                                                    model.AUTHOR.Add(myrefauthor);
                                                    model.SaveChanges();
                                                    iAuthorID = myrefauthor.AuthorID;
                                                }
                                                else
                                                {
                                                    //Update AUTHOR
                                                }
                                                if (oReference == null)
                                                {
                                                    oReference = new REFERENCE();
                                                    oReference.CreatedDate = DateTimeOffset.Now;

                                                    oReference.timestamp = DateTimeOffset.Now;
                                                    oReference.VocabularyID = iVocabularyCAPECID;
                                                    model.REFERENCE.Add(oReference);
                                                    model.SaveChanges();

                                                    //Update ATTACKPATTERNREFERENCE
                                                    //mycapecref.timestamp = DateTimeOffset.Now;
                                                    //mycapecref.ReferenceID = oReference.ReferenceID;
                                                    //model.SaveChanges();
                                                    Console.WriteLine("DEBUG ATTACKPATTERNREFERENCE updated with ReferenceID=" + oReference.ReferenceID);
                                                }
                                                else
                                                {
                                                    //Update REFERENCE
                                                }

                                                int iReferenceAuthorID = 0;
                                                try
                                                {
                                                    iReferenceAuthorID = model.REFERENCEAUTHOR.Where(o => o.ReferenceID == oReference.ReferenceID && o.AuthorID == iAuthorID).Select(o => o.ReferenceAuthorID).FirstOrDefault();
                                                }
                                                catch (Exception ex)
                                                {

                                                }
                                                //REFERENCEAUTHOR oReferenceAuthor = model.REFERENCEAUTHOR.FirstOrDefault(o => o.ReferenceID == oReference.ReferenceID && o.AuthorID == myrefauthor.AuthorID);
                                                //if (oReferenceAuthor==null)
                                                if (iReferenceAuthorID <= 0)
                                                {
                                                    REFERENCEAUTHOR oReferenceAuthor = new REFERENCEAUTHOR();
                                                    oReferenceAuthor.AuthorID = iAuthorID;// myrefauthor.AuthorID;
                                                    oReferenceAuthor.ReferenceID = oReference.ReferenceID;
                                                    oReferenceAuthor.CreatedDate = DateTimeOffset.Now;
                                                    oReferenceAuthor.timestamp = DateTimeOffset.Now;
                                                    oReferenceAuthor.VocabularyID = iVocabularyCAPECID;
                                                    model.REFERENCEAUTHOR.Add(oReferenceAuthor);
                                                    //model.SaveChanges();  //TEST PERFORMANCE
                                                    //iReferenceAuthorID=
                                                }
                                                else
                                                {
                                                    //Update REFERENCEAUTHOR
                                                }
                                                break;

                                            case "capec:Reference_Title":
                                                //TODO? Common Weakness Enumeration (CWE)
                                                if (oReference == null)
                                                {
                                                    oReference = new REFERENCE();
                                                    oReference.CreatedDate = DateTimeOffset.Now;
                                                    oReference.ReferenceTitle = CleaningCAPECString(nodeRefAtt.InnerText);
                                                    oReference.timestamp = DateTimeOffset.Now;
                                                    oReference.VocabularyID = iVocabularyCAPECID;

                                                    model.REFERENCE.Add(oReference);
                                                    model.SaveChanges();
                                                    Console.WriteLine("DEBUG Added new REFERENCE " + oReference.ReferenceID);

                                                    //Update ATTACKPATTERNREFERENCE
                                                    //mycapecref.timestamp = DateTimeOffset.Now;
                                                    //mycapecref.ReferenceID = oReference.ReferenceID;
                                                    //model.SaveChanges();
                                                    //Console.WriteLine("DEBUG ATTACKPATTERNREFERENCE updated with ReferenceID=" + oReference.ReferenceID);
                                                }
                                                else
                                                {
                                                    //Update REFERENCE
                                                    oReference.ReferenceTitle = CleaningCAPECString(nodeRefAtt.InnerText);
                                                    oReference.timestamp = DateTimeOffset.Now;
                                                    oReference.VocabularyID = iVocabularyCAPECID;
                                                    //model.SaveChanges();

                                                    //Update ATTACKPATTERNREFERENCE
                                                    //mycapecref.timestamp = DateTimeOffset.Now;
                                                    //mycapecref.ReferenceID = oReference.ReferenceID;
                                                    //model.SaveChanges();
                                                    //Console.WriteLine("DEBUG ATTACKPATTERNREFERENCE updated with ReferenceID=" + oReference.ReferenceID);
                                                }

                                                break;
                                            case "capec:Reference_Section":
                                                //TODO Review
                                                //CWE-119: Buffer Errors
                                                if (oReference != null) //TODO: comment
                                                {
                                                    //Update ATTACKPATTERNREFERENCE
                                                    mycapecref.Reference_Section = CleaningCAPECString(nodeRefAtt.InnerText);
                                                    mycapecref.timestamp = DateTimeOffset.Now;
                                                    mycapecref.ReferenceID = oReference.ReferenceID;
                                                    model.SaveChanges();
                                                    Console.WriteLine("DEBUG ATTACKPATTERNREFERENCE updated with ReferenceID=" + oReference.ReferenceID);
                                                }
                                                else
                                                {
                                                    //ERROR
                                                }
                                                break;

                                            case "capec:Reference_Edition":
                                                if (oReference != null)
                                                {
                                                    //Update REFERENCE
                                                    sReference_Edition = CleaningCAPECString(nodeRefAtt.InnerText);
                                                    oReference.Reference_Edition = sReference_Edition;
                                                    oReference.VocabularyID = iVocabularyCAPECID;
                                                    oReference.timestamp = DateTimeOffset.Now;

                                                    //Update ATTACKPATTERNREFERENCE
                                                    //mycapecref.timestamp = DateTimeOffset.Now;
                                                    //mycapecref.ReferenceID = oReference.ReferenceID;
                                                    //model.SaveChanges();
                                                    //Console.WriteLine("DEBUG ATTACKPATTERNREFERENCE updated with ReferenceID=" + oReference.ReferenceID);
                                                }
                                                else
                                                {
                                                    //ERROR
                                                }
                                                break;
                                            case "capec:Reference_Publisher":
                                                if (oReference != null)
                                                {
                                                    //Update REFERENCE
                                                    sReference_Publisher = CleaningCAPECString(nodeRefAtt.InnerText);
                                                    oReference.Reference_Publisher = sReference_Publisher;
                                                    oReference.VocabularyID = iVocabularyCAPECID;
                                                    oReference.timestamp = DateTimeOffset.Now;

                                                    //Update ATTACKPATTERNREFERENCE
                                                    //mycapecref.timestamp = DateTimeOffset.Now;
                                                    //mycapecref.ReferenceID = oReference.ReferenceID;
                                                    //model.SaveChanges();
                                                    //Console.WriteLine("DEBUG ATTACKPATTERNREFERENCE updated with ReferenceID=" + oReference.ReferenceID);
                                                }
                                                else
                                                {
                                                    //ERROR
                                                }
                                                break;
                                            case "capec:Reference_Publication":
                                                if (oReference != null)
                                                {
                                                    //Update REFERENCE
                                                    sReference_Publication = CleaningCAPECString(nodeRefAtt.InnerText);
                                                    oReference.Reference_Publication = sReference_Publication;
                                                    oReference.VocabularyID = iVocabularyCAPECID;
                                                    oReference.timestamp = DateTimeOffset.Now;

                                                    //Update ATTACKPATTERNREFERENCE
                                                    //mycapecref.timestamp = DateTimeOffset.Now;
                                                    //mycapecref.ReferenceID = oReference.ReferenceID;
                                                    //model.SaveChanges();
                                                    //Console.WriteLine("DEBUG ATTACKPATTERNREFERENCE updated with ReferenceID=" + oReference.ReferenceID);
                                                }
                                                break;
                                            case "capec:Reference_PubDate":
                                                if (oReference != null)
                                                {
                                                    //Update REFERENCE
                                                    sReference_PubDate = CleaningCAPECString(nodeRefAtt.InnerText);
                                                    oReference.Reference_PubDate = sReference_PubDate;
                                                    oReference.VocabularyID = iVocabularyCAPECID;
                                                    oReference.timestamp = DateTimeOffset.Now;

                                                    //Update ATTACKPATTERNREFERENCE
                                                    //mycapecref.timestamp = DateTimeOffset.Now;
                                                    //mycapecref.ReferenceID = oReference.ReferenceID;
                                                    //model.SaveChanges();
                                                    //Console.WriteLine("DEBUG ATTACKPATTERNREFERENCE updated with ReferenceID=" + oReference.ReferenceID);
                                                }
                                                break;
                                            case "capec:Reference_Link":
                                                string sReferenceURL = CleaningCAPECString(nodeRefAtt.InnerText);
                                                //Cleaning ReferenceURL
                                                //TODO Review (see NORMALIZEREFERENCE() in Import_all())    get the source
                                                sReferenceURL = sReferenceURL.Replace("http://www.", "http://");
                                                sReferenceURL = sReferenceURL.Replace("https://www.", "https://");
                                                sReferenceURL = sReferenceURL.Replace("http://osvdb.org/displayvuln.php?osvdbid=", "http://osvdb.org/");
                                                sReferenceURL = sReferenceURL.Replace("osvdb.org/show/osvdb/", "osvdb.org/");
                                                sReferenceURL = sReferenceURL.Replace("exploit-db.com/download/", "exploit-db.com/exploits/");
                                                sReferenceURL = sReferenceURL.Replace("securitytracker.com/id?", "securitytracker.com/id/");
                                                if (sReferenceURL.StartsWith("www."))
                                                {
                                                    sReferenceURL = "http://" + sReferenceURL;
                                                }
                                                if (oReference != null)
                                                {
                                                    //The REFERENCE exists
                                                    //Update REFERENCE
                                                    if (sReferenceURL.ToLower().Contains("http"))
                                                    {
                                                        oReference.ReferenceURL = sReferenceURL;
                                                        oReference.timestamp = DateTimeOffset.Now;
                                                        oReference.VocabularyID = iVocabularyCAPECID;

                                                        //Update ATTACKPATTERNREFERENCE
                                                        //mycapecref.timestamp = DateTimeOffset.Now;
                                                        //mycapecref.ReferenceID = oReference.ReferenceID;
                                                        //model.SaveChanges();
                                                        //Console.WriteLine("DEBUG ATTACKPATTERNREFERENCE updated with ReferenceID=" + oReference.ReferenceID);
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("ERROR: Reference_Link01 is not a URL " + sReferenceURL);
                                                    }
                                                }
                                                else
                                                {
                                                    //Check if Reference already exists in the database
                                                    oReference = model.REFERENCE.FirstOrDefault(o => o.ReferenceURL == sReferenceURL);
                                                    if (oReference == null)
                                                    {
                                                        oReference = new REFERENCE();
                                                        oReference.CreatedDate = DateTimeOffset.Now;
                                                        if (sReferenceURL.ToLower().Contains("http"))
                                                        {
                                                            oReference.ReferenceURL = sReferenceURL;
                                                            //TODO use ReferenceSource() of Import_all()
                                                            //TODO NORMALIZE
                                                        }
                                                        else
                                                        {
                                                            Console.WriteLine("ERROR: Reference_Link is not a URL " + sReferenceURL);
                                                        }
                                                        oReference.Reference_Edition = sReference_Edition;
                                                        oReference.Reference_Publication = sReference_Publication;
                                                        oReference.Reference_Publisher = sReference_Publisher;
                                                        oReference.Reference_PubDate = sReference_PubDate;
                                                        oReference.Reference_Date = sReference_Date;
                                                        //TODO: Search ISBN...
                                                        oReference.VocabularyID = iVocabularyCAPECID;
                                                        oReference.timestamp = DateTimeOffset.Now;
                                                        model.REFERENCE.Add(oReference);
                                                        model.SaveChanges();
                                                        Console.WriteLine("DEBUG Added a new REFERENCE " + oReference.ReferenceID);

                                                        //Update ATTACKPATTERNREFERENCE
                                                        //mycapecref.timestamp = DateTimeOffset.Now;
                                                        //mycapecref.ReferenceID = oReference.ReferenceID;
                                                        //model.SaveChanges();
                                                        //Console.WriteLine("DEBUG ATTACKPATTERNREFERENCE updated with ReferenceID=" + oReference.ReferenceID);
                                                    }
                                                    else
                                                    {
                                                        //Update REFERENCE
                                                        //Update the Reference found in the database
                                                        //TODO Review
                                                        if (sReference_Edition != "") oReference.Reference_Edition = sReference_Edition;
                                                        if (sReference_Publication != "") oReference.Reference_Publication = sReference_Publication;
                                                        if (sReference_Publisher != "") oReference.Reference_Publisher = sReference_Publisher;
                                                        if (sReference_PubDate != "") oReference.Reference_PubDate = sReference_PubDate;
                                                        //TODO: Search ISBN...
                                                        oReference.timestamp = DateTimeOffset.Now;

                                                        //Update ATTACKPATTERNREFERENCE
                                                        //mycapecref.timestamp = DateTimeOffset.Now;
                                                        //mycapecref.ReferenceID = oReference.ReferenceID;
                                                        //model.SaveChanges();
                                                        //Console.WriteLine("DEBUG ATTACKPATTERNREFERENCE updated with ReferenceID=" + oReference.ReferenceID);
                                                    }
                                                }
                                                break;

                                            case "capec:Reference_Date":
                                                if (oReference != null)
                                                {
                                                    //Update REFERENCE
                                                    sReference_Date = CleaningCAPECString(nodeRefAtt.InnerText);
                                                    oReference.Reference_Date = sReference_Date;
                                                    oReference.VocabularyID = iVocabularyCAPECID;
                                                    oReference.timestamp = DateTimeOffset.Now;
                                                    //Update ATTAACKPATTERNREFERENCE
                                                    //mycapecref.timestamp = DateTimeOffset.Now;
                                                    //mycapecref.ReferenceID = oReference.ReferenceID;
                                                    //model.SaveChanges();
                                                    //Console.WriteLine("DEBUG ATTACKPATTERNREFERENCE updated with ReferenceID=" + oReference.ReferenceID);
                                                }
                                                break;
                                            default:
                                                Console.WriteLine("ERROR: TODO Missing code for node2 nodeRefAtt.Name " + nodeRefAtt.Name);
                                                break;
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
                                        Console.WriteLine("Exception DbEntityValidationExceptionREFERENCE02 " + sb.ToString());
                                    }
                                    catch (Exception exnodeRefAtt02)
                                    {
                                        Console.WriteLine("Exception exnodeRefAtt02 for " + sCAPECID + " " + exnodeRefAtt02.Message + " " + exnodeRefAtt02.InnerException);
                                    }
                                }

                                //Update ATTACKPATTERNREFERENCE
                                mycapecref.timestamp = DateTimeOffset.Now;
                                mycapecref.ReferenceID = oReference.ReferenceID;
                                model.SaveChanges();
                                Console.WriteLine("DEBUG mycapecref.ReferenceID=" + mycapecref.ReferenceID);
                                mycapecref = null;
                            }
                            #endregion ATTACKPATTERNREFERENCEs
                            
                            break;
                        default:
                            Console.WriteLine("TODO: Missing Code for node2 " + node2.Name);
                            //capec:Content_History //TODO
                            break;
                    }
                }

            }
            #endregion capeccategory

            nodes1 = doc.SelectNodes("capec:Attack_Pattern_Catalog/capec:Attack_Patterns/capec:Attack_Pattern", mgr);
            #region AttackPattern
            foreach (XmlNode node in nodes1)    //capec:Attack_Pattern
            {
                mycapecid = node.Attributes["ID"].InnerText;    //79
                //capecidsearch = "CAPEC-" + mycapecid;
                sCAPECID = "CAPEC-" + mycapecid;
                Console.WriteLine("DEBUG ***************************************************************");
                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                Console.WriteLine("DEBUG Processing " + sCAPECID);

                /*
                description = "";
                try
                {
                    description=node.ChildNodes[0].ChildNodes[0].InnerText;    //Description_Summary   //TODO
                }
                catch (Exception exdescription)
                {
                    Console.WriteLine("ERROR: no Description for " + sCAPECID);
                }
                //Console.WriteLine(description);   //DEBUG
                */

                int myattackpatternid = 0;

                ATTACKPATTERN myattackpat;

                #region capecold
                //Table CAPEC Replaced by ATTACKPATTERN
                /*
                CAPEC mycapec;  //TODO Remove
                mycapec = model.CAPEC.FirstOrDefault(o => o.capec_id == sCAPECID);
                //Cleaning
                sCleanDescriptionSummary = CleaningCAPECString(description);
                
                if (mycapec == null)
                {
                    Console.WriteLine(string.Format("DEBUG Adding new CAPEC [{0}] for AttackPattern in table CAPEC", sCAPECID));

                    mycapec = new CAPEC();
                    mycapec.capec_id = sCAPECID;
                    mycapec.CategoryName = node.Attributes["Name"].InnerText;
                    try
                    {
                        mycapec.CapecStatus = node.Attributes["Status"].InnerText;   //Draft
                    }
                    catch (Exception exCapecStatus)
                    {
                        Console.WriteLine("ERROR: no CapecStatus for " + sCAPECID);
                    }
                    mycapec.DescriptionSummaryClean = sCleanDescriptionSummary;
                    mycapec.DescriptionSummary = description;
                    mycapec.CreatedDate = DateTimeOffset.Now;
                    mycapec.timestamp = DateTimeOffset.Now;
                    mycapec.VocabularyID = iVocabularyCAPECID;
                    try
                    {
                        model.CAPEC.Add(mycapec);
                        model.SaveChanges();
                    }
                    catch (Exception exAddToCAPEC1)
                    {
                        Console.WriteLine("Exception exAddToCAPEC1 " + exAddToCAPEC1.Message + " " + exAddToCAPEC1.InnerException);
                    }
                }
                else
                {
                    //(TODO) Update CAPEC
                    try
                    {
                        mycapec.CategoryName = node.Attributes["Name"].InnerText;
                        try
                        {
                            mycapec.CapecStatus = node.Attributes["Status"].InnerText;   //Draft
                        }
                        catch (Exception exCapecStatus)
                        {
                            Console.WriteLine("ERROR: no CapecStatus02 for CAPEC-" + mycapecid);
                        }
                        mycapec.DescriptionSummaryClean = sCleanDescriptionSummary;
                        mycapec.DescriptionSummary = description;
                        mycapec.timestamp = DateTimeOffset.Now;
                        mycapec.VocabularyID = iVocabularyCAPECID;
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
                    catch(Exception exUPDATECAPEC)
                    {
                        Console.WriteLine("Exception exUPDATECAPEC " + exUPDATECAPEC.Message + " " + exUPDATECAPEC.InnerException);
                    }
                }

                //TODO: model.SaveChanges(); here
                */
                #endregion capecold

                //**************************************************************************************************************
                //ATTACKPATTERN myattackpat;
                
                //Console.WriteLine("DEBUG "+mycapecid);
                
                string sAttackPatternDescription = "";
                try
                {
                    sAttackPatternDescription = node.ChildNodes[0].ChildNodes[0].ChildNodes[0].InnerText;    //<capec:Description><capec:Summary><capec:Text>   //TODO
                }
                catch(Exception exsAttackPatternDescription)
                {
                    Console.WriteLine("Exception exsAttackPatternDescription " + exsAttackPatternDescription.Message + " " + exsAttackPatternDescription.InnerException);
                }
                //Cleaning
                string sCleanAttackPatternDescription = CleaningCAPECString(sAttackPatternDescription);
                
                //string sAttackPatternName="";
                try
                {
                    sAttackPatternName = node.Attributes["Name"].InnerText;
                }
                catch(Exception exAttackPatternName)
                {
                    Console.WriteLine("ERROR: no AttackPatternName for CAPEC-"+mycapecid);
                }

                string sAttackPatternAbstraction = "";
                try
                {
                    sAttackPatternAbstraction = node.Attributes["Pattern_Abstraction"].InnerText;
                }
                catch (Exception exPatternAbstraction)
                {
                    Console.WriteLine("ERROR: no PatternAbstraction for CAPEC-" + mycapecid);
                }

                string sAttackPatternCompleteness = "";
                try
                {
                    sAttackPatternCompleteness = node.Attributes["Pattern_Completeness"].InnerText;
                }
                catch (Exception exPatternCompleteness)
                {
                    Console.WriteLine("ERROR: " + sCAPECID + " has no Pattern_Completeness " + exPatternCompleteness.Message);
                }

                string sAttackPatternStatus = "";
                try
                {
                    sAttackPatternStatus = node.Attributes["Status"].InnerText;
                }
                catch (Exception exPatternStatus)
                {
                    Console.WriteLine("ERROR: no PatternStatus for CAPEC-" + mycapecid);
                }

                myattackpat = attack_model.ATTACKPATTERN.FirstOrDefault(o => o.capec_id == sCAPECID);  //"CAPEC-" + mycapecid
                bool bAddAttackPattern = false;
                if (myattackpat == null)
                {
                    Console.WriteLine(string.Format("DEBUG Adding new ATTACKPATTERN [{0}] in table ATTACKPATTERN", mycapecid));

                    myattackpat = new ATTACKPATTERN();
                    myattackpat.CreatedDate = DateTimeOffset.Now;
                    myattackpat.capec_id = sCAPECID;    //"CAPEC-" + mycapecid

                    bAddAttackPattern = true;
                }
                else
                {
                    //Update ATTACKPATTERN
                    
                }
                //Update ATTACKPATTERN
                myattackpat.AttackPatternName = sAttackPatternName;
                myattackpat.AttackPatternDescription = sCleanAttackPatternDescription;
                //myattackpat.AttackPatternDescriptionRaw = sAttackPatternDescription;
                myattackpat.PatternAbstraction = sAttackPatternAbstraction;
                myattackpat.PatternCompleteness = sAttackPatternCompleteness;
                myattackpat.PatternStatus = sAttackPatternStatus;
                myattackpat.VocabularyID = iVocabularyCAPECID;

                if (bAddAttackPattern)
                {
                    attack_model.ATTACKPATTERN.Add(myattackpat);
                }

                try
                {
                    myattackpat.timestamp = DateTimeOffset.Now;
                    attack_model.SaveChanges();
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
                catch (Exception exATTACKPATTERN1)
                {
                    Console.WriteLine("Exception exATTACKPATTERN1 " + exATTACKPATTERN1.Message + " " + exATTACKPATTERN1.InnerException);
                }

                myattackpatternid = myattackpat.AttackPatternID;

                ATTACKPAYLOADFORATTACKPATTERN mypayloadforpattern = null;
                foreach (XmlNode nodeAP in node.ChildNodes)
                {
                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                    Console.WriteLine("DEBUG nodeAP.Name=" + nodeAP.Name);
                    /*
                    capec:Description
                    capec:Attack_Prerequisites
                    capec:Typical_Severity
                    capec:Typical_Likelihood_of_Exploit
                    capec:Methods_of_Attack
                    capec:Examples-Instances
                    capec:Attacker_Skills_or_Knowledge_Required
                    capec:Resources_Required
                    capec:Indicators-Warnings_of_Attack
                    capec:Solutions_and_Mitigations
                    capec:Attack_Motivation-Consequences
                    capec:Injection_Vector
                    capec:Payload
                    capec:Activation_Zone
                    capec:Payload_Activation_Impact
                    capec:Related_Weaknesses
                    capec:Related_Attack_Patterns
                    capec:Relevant_Security_Requirements
                    capec:Purposes
                    capec:CIA_Impact
                    capec:Technical_Context
                    capec:References
                    capec:Other_Notes
                    capec:Content_History
                    */

                    int iAttackMotivationConsequenceAdded = 0;
                    
                    switch (nodeAP.Name)
                    {
                        //TODO

                        case "capec:Description":
                            #region capecdescription
                            //Console.WriteLine("DEBUG capecdescription");
                            foreach (XmlNode nodeAttackPatternDescription in nodeAP.ChildNodes)
                            {
                                //Console.WriteLine("DEBUG nodeAttackPatternDescription " + nodeAttackPatternDescription.Name);
                                switch (nodeAttackPatternDescription.Name)
                                {
                                    
                                    case "capec:Summary":
                                    //    //Done before   //TODO: Review
                                        break;

                                    case "capec:Attack_Execution_Flow":
                                        #region attackexecutionflow
                                        Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                        Console.WriteLine("DEBUG Attack_Execution_Flow");

                                        //TODO: KILLCHAIN CAPEC

                                        foreach (XmlNode nodeAttackExecutionFlow in nodeAttackPatternDescription.ChildNodes)
                                        {
                                            //Console.WriteLine("DEBUG nodeAttackExecutionFlow " + nodeAttackExecutionFlow.Name);
                                            switch (nodeAttackExecutionFlow.Name)
                                            {
                                                case "capec:Attack_Phases":
                                                    //Console.WriteLine("DEBUG Attack_Phases");
                                                    int iAttackPhaseOrder = 0;
                                                    foreach (XmlNode nodeAttackPhases in nodeAttackExecutionFlow.ChildNodes)
                                                    {
                                                        iAttackPhaseOrder++;
                                                        //Console.WriteLine("DEBUG Attack_Phases " + nodeAttackPhases.Name);
                                                        switch (nodeAttackPhases.Name)
                                                        {
                                                            case "capec:Attack_Phase":
                                                                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                                Console.WriteLine("DEBUG capec:Attack_Phase");
                                                                int iAttackPhaseVocabularyID = 0;
                                                                try
                                                                {
                                                                    iAttackPhaseVocabularyID = Convert.ToInt32(nodeAttackPhases.Attributes["ID"].InnerText);
                                                                    
                                                                }
                                                                catch (Exception exiAttackPhaseVocabularyID)
                                                                {
                                                                    Console.WriteLine("Exception exiAttackPhaseVocabularyID " + exiAttackPhaseVocabularyID.Message + " " + exiAttackPhaseVocabularyID.InnerException);
                                                                }

                                                                
                                                                if(iAttackPhaseVocabularyID != iAttackPhaseOrder)
                                                                {
                                                                    //We could have a problem here (repository/XML file was not perfect, or has been updated)
                                                                    Console.WriteLine("DEBUG iAttackPhaseVocabularyID=" + iAttackPhaseVocabularyID);
                                                                    Console.WriteLine("DEBUG iAttackPhaseOrder=" + iAttackPhaseOrder);
                                                                }

                                                                ATTACKPHASEFORATTACKPATTERN oAttackPhaseForAttackPattern = null;
                                                                
                                                                string sAttackPhaseName = "";
                                                                try
                                                                {
                                                                    sAttackPhaseName = nodeAttackPhases.Attributes["Name"].InnerText.Trim();   //Explore    Experiment  Exploit (or ""...)
                                                                    //Cleaning?
                                                                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                                    Console.WriteLine("DEBUG sAttackPhaseName=" + sAttackPhaseName);
                                                                    //PHASE oPhase = null;
                                                                    int iPhaseID = 0;
                                                                    int iAttackPhaseID = 0;

                                                                    #region phase
                                                                    if (sAttackPhaseName != "")
                                                                    {
                                                                        //Use of the PHASE table
                                                                        //TODO: we could use Phase.PhaseID instead of AttackPhaseForAttackPattern.AttackPhaseName

                                                                        try
                                                                        {
                                                                            iPhaseID = model.PHASE.Where(o => o.PhaseName == sAttackPhaseName).Select(o=>o.PhaseID).FirstOrDefault();
                                                                        }
                                                                        catch (Exception ex)
                                                                        {

                                                                        }
                                                                        //oPhase = model.PHASE.FirstOrDefault(o => o.PhaseName == sAttackPhaseName);    //&& VocabularyID
                                                                        //if (oPhase != null)
                                                                        if (iPhaseID > 0)
                                                                        {
                                                                            //Update PHASE
                                                                        }
                                                                        else
                                                                        {
                                                                            
                                                                            try
                                                                            {
                                                                                Console.WriteLine("DEBUG Adding new PHASE");
                                                                                PHASE oPhase = new PHASE();
                                                                                oPhase.PhaseName = sAttackPhaseName;    //Explore   Experiment  Exploit
                                                                                oPhase.CreatedDate = DateTimeOffset.Now;
                                                                                oPhase.timestamp = DateTimeOffset.Now;
                                                                                oPhase.VocabularyID = iVocabularyCAPECID;
                                                                                model.PHASE.Add(oPhase);
                                                                                model.SaveChanges();
                                                                                iPhaseID = oPhase.PhaseID;
                                                                            }
                                                                            catch (Exception exoPhase)
                                                                            {
                                                                                Console.WriteLine("Exception exoPhase " + exoPhase.InnerException + " " + exoPhase.InnerException);
                                                                            }
                                                                        }

                                                                        try
                                                                        {
                                                                            iAttackPhaseID = attack_model.ATTACKPHASE.Where(o => o.AttackPhaseName == sAttackPhaseName).Select(o=>o.AttackPhaseID).FirstOrDefault();
                                                                        }
                                                                        catch(Exception ex)
                                                                        {

                                                                        }
                                                                        //oAttackPhase = attack_model.ATTACKPHASE.FirstOrDefault(o => o.AttackPhaseName == sAttackPhaseName);    //&& VocabularyID
                                                                        //if (oPhase != null)
                                                                        if (iAttackPhaseID > 0)
                                                                        {
                                                                            //Update ATTACKPHASE
                                                                        }
                                                                        else
                                                                        {
                                                                            Console.WriteLine("DEBUG Adding new ATTACKPHASE");
                                                                            ATTACKPHASE oAttackPhase = new ATTACKPHASE();
                                                                            oAttackPhase.AttackPhaseName = sAttackPhaseName;    //Explore   Experiment  Exploit
                                                                            oAttackPhase.PhaseID = iPhaseID;
                                                                            oAttackPhase.CreatedDate = DateTimeOffset.Now;
                                                                            oAttackPhase.timestamp = DateTimeOffset.Now;
                                                                            oAttackPhase.VocabularyID = iVocabularyCAPECID;
                                                                            try
                                                                            {
                                                                                attack_model.ATTACKPHASE.Add(oAttackPhase);
                                                                                attack_model.SaveChanges();
                                                                                iAttackPhaseID = oAttackPhase.AttackPhaseID;
                                                                            }
                                                                            catch (Exception exoPhase)
                                                                            {
                                                                                Console.WriteLine("Exception exoPhase " + exoPhase.InnerException + " " + exoPhase.InnerException);
                                                                            }
                                                                        }

                                                                        


                                                                    }
                                                                    #endregion phase

                                                                    //TODO ATTACKPHASE => KILLCHAINPHASE    iKillChainCAPECID

                                                                        //Cleaning?
                                                                        //TODO Review if we can/should test ==sAttackPhaseID and/or ==iAttackPhaseOrder?
                                                                        
                                                                        oAttackPhaseForAttackPattern = attack_model.ATTACKPHASEFORATTACKPATTERN.FirstOrDefault(o => o.AttackPatternID == myattackpatternid && o.VocabularyID == iVocabularyCAPECID && o.AttackPhaseOrder==iAttackPhaseOrder);
                                                                        if (oAttackPhaseForAttackPattern == null)
                                                                        {
                                                                            Console.WriteLine("DEBUG Adding new ATTACKPHASEFORATTACKPATTERN");
                                                                            oAttackPhaseForAttackPattern = new ATTACKPHASEFORATTACKPATTERN();
                                                                            //oAttackPhaseForAttackPattern.AttackPhaseName = sAttackPhaseName;  //Removed
                                                                            //Explore   Experiment  Exploit

                                                                                //oAttackPhaseForAttackPattern.PhaseID = iPhaseID;    // Removed
                                                                                oAttackPhaseForAttackPattern.AttackPhaseID = iAttackPhaseID;
                                                                            
                                                                            oAttackPhaseForAttackPattern.AttackPatternID = myattackpatternid;
                                                                            oAttackPhaseForAttackPattern.AttackPhaseVocabularyID = iAttackPhaseVocabularyID;
                                                                            oAttackPhaseForAttackPattern.AttackPhaseOrder = iAttackPhaseOrder;
                                                                            oAttackPhaseForAttackPattern.CreatedDate = DateTimeOffset.Now;
                                                                            oAttackPhaseForAttackPattern.timestamp = DateTimeOffset.Now;
                                                                            oAttackPhaseForAttackPattern.VocabularyID = iVocabularyCAPECID;
                                                                            try
                                                                            {
                                                                                attack_model.ATTACKPHASEFORATTACKPATTERN.Add(oAttackPhaseForAttackPattern);
                                                                                attack_model.SaveChanges();
                                                                            }
                                                                            catch (Exception exoAttackPhaseForAttackPattern)
                                                                            {
                                                                                Console.WriteLine("exception exoAttackPhaseForAttackPattern " + exoAttackPhaseForAttackPattern.Message + " " + exoAttackPhaseForAttackPattern.InnerException);
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            //Update ATTACKPHASEFORATTACKPATTERN
                                                                            /*
                                                                            if (oAttackPhaseForAttackPattern.AttackPhaseName != sAttackPhaseName)
                                                                            {
                                                                                Console.WriteLine("DEBUG POTENTIAL ERROR capec:Attack_Phase AttackPhaseName=" + oAttackPhaseForAttackPattern.AttackPhaseName + " sAttackPhaseName=" + sAttackPhaseName);
                                                                                oAttackPhaseForAttackPattern.AttackPhaseName = sAttackPhaseName;    //Removed

                                                                            }
                                                                            */
                                                                            if (oAttackPhaseForAttackPattern.AttackPhaseOrder != iAttackPhaseOrder)
                                                                            {
                                                                                Console.WriteLine("DEBUG POTENTIAL ERROR capec:Attack_Phase AttackPhaseOrder=" + oAttackPhaseForAttackPattern.AttackPhaseOrder + " iAttackPhaseOrder=" + iAttackPhaseOrder);
                                                                                oAttackPhaseForAttackPattern.AttackPhaseOrder = iAttackPhaseOrder;
                                                                            }
                                                                            //Note: this is often due to an update/fix

                                                                            oAttackPhaseForAttackPattern.AttackPhaseVocabularyID = iAttackPhaseVocabularyID;    //could have changed to
                                                                            oAttackPhaseForAttackPattern.VocabularyID = iVocabularyCAPECID;  //eventualy change in the future
                                                                            
                                                                                //oAttackPhaseForAttackPattern.PhaseID = iPhaseID;
                                                                            
                                                                            //Update ATTACKPHASEFORATTACKPATTERN
                                                                            oAttackPhaseForAttackPattern.AttackPhaseID = iAttackPhaseID;
                                                                            oAttackPhaseForAttackPattern.timestamp = DateTimeOffset.Now;
                                                                            attack_model.SaveChanges();
                                                                            Console.WriteLine("DEBUG ATTACKPHASEFORATTACKPATTERN UPDATED");
                                                                        }
                                                                    if (sAttackPhaseName != "")
                                                                    {

                                                                    }
                                                                    else
                                                                    {
                                                                        //NOTE: that was an issue in previous versions of CAPEC
                                                                        Console.WriteLine("ERROR: capec:Attack_Phase no sAttackPhaseName for "+sCAPECID);
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
                                                                    Console.WriteLine("Exception DbEntityValidationExceptionexAttackPhase " + sb.ToString());
                                                                }
                                                                catch (Exception exAttackPhase)
                                                                {
                                                                    Console.WriteLine("Exception exAttackPhase " + exAttackPhase.Message + " " + exAttackPhase.InnerException);
                                                                }

                                                                foreach (XmlNode nodeAttackPhase in nodeAttackPhases.ChildNodes)
                                                                {
                                                                    #region attackphaseinfos
                                                                    //Console.WriteLine("DEBUG nodeAttackPhase " + nodeAttackPhase.Name);
                                                                    switch (nodeAttackPhase.Name)
                                                                    {
                                                                        case "capec:Attack_Steps":
                                                                            int iAttackStepOrder = 0;
                                                                            foreach (XmlNode nodeAttackSteps in nodeAttackPhase.ChildNodes)
                                                                            {
                                                                                iAttackStepOrder++;
                                                                                //Console.WriteLine("DEBUG nodeAttackSteps " + nodeAttackSteps.Name);

                                                                                switch (nodeAttackSteps.Name)
                                                                                {
                                                                                    case "capec:Attack_Step":
                                                                                        //TODO
                                                                                        int iAttackStepVocabularyID = 0;
                                                                                        try
                                                                                        {
                                                                                            iAttackStepVocabularyID=Convert.ToInt32(nodeAttackSteps.Attributes["ID"].InnerText);
                                                                                        }
                                                                                        catch(Exception exiAttackStepVocabularyID)
                                                                                        {
                                                                                            Console.WriteLine("Exception exiAttackStepVocabularyID " + exiAttackStepVocabularyID.Message + " " + exiAttackStepVocabularyID.InnerException);
                                                                                        }

                                                                                        ATTACKSTEP oAttackStep = attack_model.ATTACKSTEP.FirstOrDefault(o => o.AttackPatternAttackPhaseID == oAttackPhaseForAttackPattern.AttackPatternAttackPhaseID && o.AttackStepOrder == iAttackStepOrder);
                                                                                        if (oAttackStep == null)
                                                                                        {
                                                                                            Console.WriteLine("DEBUG Adding new ATTACKSTEP");
                                                                                            try
                                                                                            {
                                                                                                oAttackStep = new ATTACKSTEP();
                                                                                                
                                                                                                oAttackStep.AttackPatternAttackPhaseID = oAttackPhaseForAttackPattern.AttackPatternAttackPhaseID;

                                                                                                //TODO Review
                                                                                                //oAttackStep.AttackPhaseID = oAttackPhaseForAttackPattern.AttackPhaseID;


                                                                                                oAttackStep.AttackStepOrder = iAttackStepOrder;
                                                                                                oAttackStep.AttackStepVocabularyID = iAttackStepVocabularyID;
                                                                                                oAttackStep.CreatedDate = DateTimeOffset.Now;
                                                                                                oAttackStep.timestamp = DateTimeOffset.Now;
                                                                                                oAttackStep.VocabularyID = iVocabularyCAPECID;
                                                                                                attack_model.ATTACKSTEP.Add(oAttackStep);
                                                                                                attack_model.SaveChanges();
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
                                                                                                Console.WriteLine("Exception DbEntityValidationExceptionADDATTACKSTEP " + sb.ToString());
                                                                                            }
                                                                                            catch (Exception exAddToATTACKSTEP)
                                                                                            {
                                                                                                Console.WriteLine("Exception exAddToATTACKSTEP " + exAddToATTACKSTEP.Message + " " + exAddToATTACKSTEP.InnerException);
                                                                                            }
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            //Update ATTACKSTEP
                                                                                            if (oAttackStep.AttackStepVocabularyID != iAttackStepVocabularyID)
                                                                                            {
                                                                                                Console.WriteLine("ERROR: oAttackStep.AttackStepVocabularyID=" + oAttackStep.AttackStepVocabularyID + " iAttackStepVocabularyID ="+iAttackStepVocabularyID);

                                                                                            }
                                                                                        }

                                                                                        foreach (XmlNode nodeAttackStep in nodeAttackSteps.ChildNodes)
                                                                                        {
                                                                                            //Console.WriteLine("DEBUG nodeAttackStep " + nodeAttackStep.Name);
                                                                                            string sAttackStepTitle = "";
                                                                                            string sAttackStepDescription = "";
                                                                                            

                                                                                            switch (nodeAttackStep.Name)
                                                                                            {
                                                                                                case "capec:Custom_Attack_Step":
                                                                                                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                                                                    Console.WriteLine("DEBUG capec:Custom_Attack_Step");
                                                                                                    foreach (XmlNode nodeAttackStepInfo in nodeAttackStep.ChildNodes)
                                                                                                    {
                                                                                                        //Console.WriteLine("DEBUG nodeAttackStepInfo " + nodeAttackStepInfo.Name);
                                                                                                        
                                                                                                        switch (nodeAttackStepInfo.Name)
                                                                                                        {
                                                                                                            case "capec:Attack_Step_Title": //Survey
                                                                                                                #region capecattacksteptitle
                                                                                                                sAttackStepTitle = CleaningCAPECString(nodeAttackStepInfo.InnerText);
                                                                                                                
                                                                                                                if (sAttackStepTitle == "") //Note: this is an issue
                                                                                                                {
                                                                                                                    Console.WriteLine("ERROR: NO sAttackStepTitle");
                                                                                                                }

                                                                                                                try
                                                                                                                {
                                                                                                                    //Update ATTACKSTEP
                                                                                                                    oAttackStep.Attack_Step_Title = CleaningCAPECString(sAttackStepTitle);
                                                                                                                    oAttackStep.timestamp = DateTimeOffset.Now;
                                                                                                                    attack_model.SaveChanges();
                                                                                                                    //TODO: Link to WASC? OWASP?
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
                                                                                                                    Console.WriteLine("Exception DbEntityValidationExceptionADDATTACKSTEP " + sb.ToString());
                                                                                                                }
                                                                                                                catch(Exception exAttack_Step_Title)
                                                                                                                {
                                                                                                                    Console.WriteLine("Exception exAttack_Step_Title " + exAttack_Step_Title.Message + " " + exAttack_Step_Title.InnerException);
                                                                                                                }

                                                                                                                break;
                                                                                                                #endregion capecattacksteptitle
                                                                                                            case "capec:Attack_Step_Description":
                                                                                                                #region capecattackstepdescription
                                                                                                                sAttackStepDescription = CleaningCAPECString(nodeAttackStepInfo.InnerText);
                                                                                                                try
                                                                                                                {
                                                                                                                    oAttackStep.Attack_Step_Description = sAttackStepDescription;
                                                                                                                    oAttackStep.timestamp = DateTimeOffset.Now;
                                                                                                                    attack_model.SaveChanges();
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
                                                                                                                    Console.WriteLine("Exception DbEntityValidationExceptionexAttack_Step_Description " + sb.ToString());
                                                                                                                }
                                                                                                                catch (Exception exAttack_Step_Description)
                                                                                                                {
                                                                                                                    Console.WriteLine("Exception exAttack_Step_Description " + exAttack_Step_Description.Message + " " + exAttack_Step_Description.InnerException);
                                                                                                                }
                                                                                                                break;
                                                                                                                #endregion capecattackstepdescription
                                                                                                            case "capec:Attack_Step_Techniques":
                                                                                                                #region attacksteptechniques
                                                                                                                foreach (XmlNode nodeAttackStepTechniques in nodeAttackStepInfo.ChildNodes)
                                                                                                                {
                                                                                                                    //Console.WriteLine("DEBUG nodeAttackStepTechniques " + nodeAttackStepTechniques.Name);
                                                                                                                    switch (nodeAttackStepTechniques.Name)
                                                                                                                    {
                                                                                                                        case "capec:Attack_Step_Technique":
                                                                                                                            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                                                                                            Console.WriteLine("DEBUG capec:Attack_Step_Technique");
                                                                                                                            //nodeAttackStepTechniques.Attributes["ID"].InnerText;
                                                                                                                            string sAttackStepTechniqueVocabularyID = "";
                                                                                                                            try
                                                                                                                            {
                                                                                                                                sAttackStepTechniqueVocabularyID = nodeAttackStepTechniques.Attributes["ID"].InnerText;
                                                                                                                            }
                                                                                                                            catch (Exception exiAttackStepTechniqueVocabularyID)
                                                                                                                            {
                                                                                                                                Console.WriteLine("Exception exiAttackStepTechniqueVocabularyID " + exiAttackStepTechniqueVocabularyID.Message + " " + exiAttackStepTechniqueVocabularyID.InnerException);
                                                                                                                            }

                                                                                                                            int iAttackStepTechniqueID = 0;
                                                                                                                            //ATTACKSTEPTECHNIQUE oAttackStepTechnique=null;
                                                                                                                            foreach (XmlNode nodeAttackStepTechnique in nodeAttackStepTechniques.ChildNodes)
                                                                                                                            {
                                                                                                                                //Console.WriteLine("DEBUG nodeAttackStepTechnique " + nodeAttackStepTechnique.Name);
                                                                                                                                
                                                                                                                                switch (nodeAttackStepTechnique.Name)
                                                                                                                                {
                                                                                                                                    case "capec:Attack_Step_Technique_Description":
                                                                                                                                        #region capecattacksteptechniquedescription
                                                                                                                                        string sAttackStepTechniqueDescription = CleaningCAPECString(nodeAttackStepTechnique.InnerText);

                                                                                                                                        int iAttackTechniqueID = 0;
                                                                                                                                        try
                                                                                                                                        {
                                                                                                                                            //iAttackTechniqueID = attack_model.ATTACKTECHNIQUE.FirstOrDefault(o => o.AttackTechniqueDescription == sAttackStepTechniqueDescription).AttackTechniqueID;
                                                                                                                                            iAttackTechniqueID = attack_model.ATTACKTECHNIQUE.Where(o => o.AttackTechniqueDescription == sAttackStepTechniqueDescription).Select(o=>o.AttackTechniqueID).FirstOrDefault();
                                                                                                                                        }
                                                                                                                                        catch(Exception ex)
                                                                                                                                        {

                                                                                                                                        }

                                                                                                                                        //ATTACKTECHNIQUE oAttackTechnique = attack_model.ATTACKTECHNIQUE.FirstOrDefault(o => o.AttackTechniqueDescription == sAttackStepTechniqueDescription);  //VocabularyID?
                                                                                                                                        //if (oAttackTechnique == null)
                                                                                                                                        if (iAttackTechniqueID<=0)
                                                                                                                                        {
                                                                                                                                            Console.WriteLine("DEBUG Adding new ATTACKTECHNIQUE " + sAttackStepTechniqueDescription);
                                                                                                                                            
                                                                                                                                            try
                                                                                                                                            {
                                                                                                                                                ATTACKTECHNIQUE oAttackTechnique = new ATTACKTECHNIQUE();
                                                                                                                                                oAttackTechnique.AttackTechniqueDescription = sAttackStepTechniqueDescription;
                                                                                                                                                oAttackTechnique.CreatedDate = DateTimeOffset.Now;
                                                                                                                                                oAttackTechnique.timestamp = DateTimeOffset.Now;
                                                                                                                                                oAttackTechnique.VocabularyID = iVocabularyCAPECID;
                                                                                                                                                //TODO: Review. Link to WASC? OWASP Testing Guide?
                                                                                                                                                attack_model.ATTACKTECHNIQUE.Add(oAttackTechnique);
                                                                                                                                                attack_model.SaveChanges();
                                                                                                                                                iAttackTechniqueID = oAttackTechnique.AttackTechniqueID;
                                                                                                                                            }
                                                                                                                                            catch (Exception exAddToATTACKTECHNIQUE)
                                                                                                                                            {
                                                                                                                                                Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                                                                                                                                                Console.WriteLine("Exception exAddToATTACKTECHNIQUE " + exAddToATTACKTECHNIQUE.Message + " " + exAddToATTACKTECHNIQUE.InnerException);
                                                                                                                                            }
                                                                                                                                        }
                                                                                                                                        else
                                                                                                                                        {
                                                                                                                                            //Update ATTACKTECHNIQUE
                                                                                                                                        }

                                                                                                                                        try
                                                                                                                                        {
                                                                                                                                            
                                                                                                                                            try
                                                                                                                                            {
                                                                                                                                                //iAttackStepTechniqueID = attack_model.ATTACKSTEPTECHNIQUE.FirstOrDefault(o => o.AttackStepID == oAttackStep.AttackStepID && o.AttackTechniqueID == iAttackTechniqueID).AttackStepTechniqueID;
                                                                                                                                                iAttackStepTechniqueID = attack_model.ATTACKSTEPTECHNIQUE.Where(o => o.AttackStepID == oAttackStep.AttackStepID && o.AttackTechniqueID == iAttackTechniqueID).Select(o=>o.AttackStepTechniqueID).FirstOrDefault();

                                                                                                                                            }
                                                                                                                                            catch(Exception ex)
                                                                                                                                            {

                                                                                                                                            }
                                                                                                                                            //oAttackStepTechnique = attack_model.ATTACKSTEPTECHNIQUE.FirstOrDefault(o => o.AttackStepID == oAttackStep.AttackStepID && o.AttackStepTechniqueID == iAttackTechniqueID);
                                                                                                                                            //if (oAttackStepTechnique == null)
                                                                                                                                            if (iAttackStepTechniqueID <= 0)
                                                                                                                                            {
                                                                                                                                                Console.WriteLine("DEBUG Adding new ATTACKSTEPTECHNIQUE");
                                                                                                                                                try
                                                                                                                                                {
                                                                                                                                                    ATTACKSTEPTECHNIQUE oAttackStepTechnique = new ATTACKSTEPTECHNIQUE();
                                                                                                                                                    oAttackStepTechnique.AttackStepID = oAttackStep.AttackStepID;
                                                                                                                                                    oAttackStepTechnique.AttackTechniqueID = iAttackTechniqueID;    // oAttackTechnique.AttackTechniqueID;
                                                                                                                                                    oAttackStepTechnique.AttackStepTechniqueVocabularyID = sAttackStepTechniqueVocabularyID;
                                                                                                                                                    oAttackStepTechnique.CreatedDate = DateTimeOffset.Now;
                                                                                                                                                    oAttackStepTechnique.timestamp = DateTimeOffset.Now;
                                                                                                                                                    oAttackStepTechnique.VocabularyID = iVocabularyCAPECID;
                                                                                                                                                    attack_model.ATTACKSTEPTECHNIQUE.Add(oAttackStepTechnique);
                                                                                                                                                    attack_model.SaveChanges();
                                                                                                                                                    iAttackStepTechniqueID = oAttackStepTechnique.AttackStepTechniqueID;
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
                                                                                                                                                    Console.WriteLine("Exception DbEntityValidationExceptionexAddToAttackStepTECHNIQUE " + sb.ToString());
                                                                                                                                                }
                                                                                                                                                catch (Exception exAddToAttackStepTECHNIQUE)
                                                                                                                                                {
                                                                                                                                                    Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                                                                                                                                                    Console.WriteLine("Exception exAddToAttackStepTECHNIQUE " + sCAPECID + " " + exAddToAttackStepTECHNIQUE.Message + " " + exAddToAttackStepTECHNIQUE.InnerException);
                                                                                                                                                }
                                                                                                                                            }
                                                                                                                                            else
                                                                                                                                            {
                                                                                                                                                //Update ATTACKSTEPTECHNIQUE
                                                                                                                                                //TODO
                                                                                                                                            }
                                                                                                                                            Console.WriteLine("DEBUG iAttackStepTechniqueID=" + iAttackStepTechniqueID);
                                                                                                                                        }
                                                                                                                                        catch (Exception exATTACKSTEPTECHNIQUE)
                                                                                                                                        {
                                                                                                                                            Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                                                                                                                                            Console.WriteLine("Exception exATTACKSTEPTECHNIQUE " + exATTACKSTEPTECHNIQUE.Message + " " + exATTACKSTEPTECHNIQUE.InnerException);
                                                                                                                                        }

                                                                                                                                        break;
                                                                                                                                        #endregion capecattacksteptechniquedescription

                                                                                                                                    case "capec:Environments":
                                                                                                                                        #region capecattacksteptechniqueenvironment
                                                                                                                                        //Need to split
                                                                                                                                        //env-
                                                                                                                                        MatchCollection matches = Regex.Matches(nodeAttackStepTechnique.InnerText, @"env-\w+");

                                                                                                                                        foreach (Match  match in matches)
                                                                                                                                        {
                                                                                                                                            string sEnvironmentID = match.Value.Trim();   //env-Web
                                                                                                                                            Console.WriteLine("DEBUG ENVIRONMENT RegexMatch:" + sEnvironmentID);
                                                                                                                                            
                                                                                                                                            int iEnvironmentID = 0;
                                                                                                                                            try
                                                                                                                                            {
                                                                                                                                                //iEnvironmentID = model.ENVIRONMENT.FirstOrDefault(o => o.CapecEnvironmentID == sEnvironmentID).EnvironmentID;
                                                                                                                                                iEnvironmentID = model.ENVIRONMENT.Where(o => o.CapecEnvironmentID == sEnvironmentID).Select(o=>o.EnvironmentID).FirstOrDefault();
                                                                                                                                            }
                                                                                                                                            catch(Exception ex)
                                                                                                                                            {

                                                                                                                                            }

                                                                                                                                            //ENVIRONMENT oEnvironment=model.ENVIRONMENT.FirstOrDefault(o=>o.CapecEnvironmentID==match.Value.Trim());
                                                                                                                                            //if(oEnvironment==null)
                                                                                                                                            if (iEnvironmentID<=0)
                                                                                                                                            {
                                                                                                                                                try
                                                                                                                                                {
                                                                                                                                                    ENVIRONMENT oEnvironment = new ENVIRONMENT();
                                                                                                                                                    oEnvironment.CapecEnvironmentID = sEnvironmentID;   //env-Web
                                                                                                                                                    oEnvironment.EnvironmentTitle = sEnvironmentID.Replace("env-", "");
                                                                                                                                                    oEnvironment.CreatedDate = DateTimeOffset.Now;
                                                                                                                                                    oEnvironment.timestamp = DateTimeOffset.Now;
                                                                                                                                                    oEnvironment.VocabularyID = iVocabularyCAPECID;
                                                                                                                                                    model.ENVIRONMENT.Add(oEnvironment);
                                                                                                                                                    model.SaveChanges();
                                                                                                                                                    iEnvironmentID = oEnvironment.EnvironmentID;
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
                                                                                                                                                    Console.WriteLine("Exception DbEntityValidationExceptionENVIRONMENT " + sb.ToString());
                                                                                                                                                }
                                                                                                                                                catch (Exception exENVIRONMENT)
                                                                                                                                                {
                                                                                                                                                    Console.WriteLine("Exception exENVIRONMENT " + exENVIRONMENT.Message + " " + exENVIRONMENT.InnerException);
                                                                                                                                                }
                                                                                                                                            }
                                                                                                                                            else
                                                                                                                                            {
                                                                                                                                                //Update ENVIRONMENT
                                                                                                                                            }

                                                                                                                                            try
                                                                                                                                            {
                                                                                                                                                int iAttackStepTechniqueEnvironmentID = 0;
                                                                                                                                                try
                                                                                                                                                {
                                                                                                                                                    //iAttackStepTechniqueEnvironmentID = attack_model.ATTACKSTEPTECHNIQUEENVIRONMENT.FirstOrDefault(o => o.AttackStepTechniqueID == iAttackStepTechniqueID && o.EnvironmentID == iEnvironmentID).AttackStepTechniqueEnvironmentID;
                                                                                                                                                    iAttackStepTechniqueEnvironmentID = attack_model.ATTACKSTEPTECHNIQUEENVIRONMENT.Where(o => o.AttackStepTechniqueID == iAttackStepTechniqueID && o.EnvironmentID == iEnvironmentID).Select(o=>o.AttackStepTechniqueEnvironmentID).FirstOrDefault();
                                                                                                                                                    
                                                                                                                                                }
                                                                                                                                                catch(Exception ex)
                                                                                                                                                {

                                                                                                                                                }

                                                                                                                                                //ATTACKSTEPTECHNIQUEENVIRONMENT oAttackStepTechniqueEnvironment = attack_model.ATTACKSTEPTECHNIQUEENVIRONMENT.FirstOrDefault(o => o.AttackStepTechniqueID == oAttackStepTechnique.AttackStepTechniqueID && o.EnvironmentID == oEnvironment.EnvironmentID);
                                                                                                                                                //if (oAttackStepTechniqueEnvironment == null)
                                                                                                                                                if (iAttackStepTechniqueEnvironmentID<=0)
                                                                                                                                                {
                                                                                                                                                    ATTACKSTEPTECHNIQUEENVIRONMENT oAttackStepTechniqueEnvironment = new ATTACKSTEPTECHNIQUEENVIRONMENT();
                                                                                                                                                    oAttackStepTechniqueEnvironment.AttackStepTechniqueID = iAttackStepTechniqueID; // oAttackStepTechnique.AttackStepTechniqueID;
                                                                                                                                                    oAttackStepTechniqueEnvironment.EnvironmentID = iEnvironmentID; // oEnvironment.EnvironmentID;
                                                                                                                                                    oAttackStepTechniqueEnvironment.CreatedDate = DateTimeOffset.Now;
                                                                                                                                                    oAttackStepTechniqueEnvironment.timestamp = DateTimeOffset.Now;
                                                                                                                                                    oAttackStepTechniqueEnvironment.VocabularyID=iVocabularyCAPECID;
                                                                                                                                                    attack_model.ATTACKSTEPTECHNIQUEENVIRONMENT.Add(oAttackStepTechniqueEnvironment);
                                                                                                                                                    attack_model.SaveChanges();
                                                                                                                                                    //iAttackStepTechniqueEnvironmentID=
                                                                                                                                                }
                                                                                                                                                else
                                                                                                                                                {
                                                                                                                                                    //Update ATTACKSTEPTECHNIQUEENVIRONMENT
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
                                                                                                                                                Console.WriteLine("Exception DbEntityValidationExceptionexoAttackStepTechniqueEnvironment " + sb.ToString());
                                                                                                                                            }
                                                                                                                                            catch (Exception exoAttackStepTechniqueEnvironment)
                                                                                                                                            {
                                                                                                                                                Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                                                                                                                                                Console.WriteLine("Exception exoAttackStepTechniqueEnvironment " + sCAPECID + " " + exoAttackStepTechniqueEnvironment.Message + " " + exoAttackStepTechniqueEnvironment.InnerException);
                                                                                                                                            }
                                                                                                                                        }
                                                                                                                                        break;
                                                                                                                                        #endregion capecattacksteptechniqueenvironment
                                                                                                                                    
                                                                                                                                    //TODO
                                                                                                                                    
                                                                                                                                    case "capec:Leveraged_Attack_Patterns":
                                                                                                                                        foreach (XmlNode nodeLeveragedAttackPattern in nodeAttackStepTechnique.ChildNodes)
                                                                                                                                        {
                                                                                                                                            try
                                                                                                                                            {
                                                                                                                                                string sCAPECIDLeveraged = "CAPEC-" + nodeLeveragedAttackPattern.InnerText; //132
                                                                                                                                                int iAttackPatternLeveragedID = 0;
                                                                                                                                                try
                                                                                                                                                {
                                                                                                                                                    iAttackPatternLeveragedID=attack_model.ATTACKPATTERN.Where(o => o.capec_id == sCAPECIDLeveraged).Select(o => o.AttackPatternID).FirstOrDefault(); //CAPEC-132
                                                                                                                                                }
                                                                                                                                                catch(Exception ex)
                                                                                                                                                {

                                                                                                                                                }
                                                                                                                                                if(iAttackPatternLeveragedID<=0)
                                                                                                                                                {
                                                                                                                                                    ATTACKPATTERN oAttackPatternLeveraged = new ATTACKPATTERN();
                                                                                                                                                    oAttackPatternLeveraged.capec_id = sCAPECIDLeveraged;
                                                                                                                                                    oAttackPatternLeveraged.CreatedDate = DateTimeOffset.Now;
                                                                                                                                                    oAttackPatternLeveraged.VocabularyID = iVocabularyCAPECID;
                                                                                                                                                    oAttackPatternLeveraged.timestamp = DateTimeOffset.Now;
                                                                                                                                                    attack_model.ATTACKPATTERN.Add(oAttackPatternLeveraged);
                                                                                                                                                    attack_model.SaveChanges();
                                                                                                                                                    iAttackPatternLeveragedID = oAttackPatternLeveraged.AttackPatternID;
                                                                                                                                                }
                                                                                                                                                else
                                                                                                                                                {
                                                                                                                                                    //Update ATTACKPATTERN
                                                                                                                                                }

                                                                                                                                                int iAttackPatternRelationID = 0;
                                                                                                                                                try
                                                                                                                                                {
                                                                                                                                                    //iAttackPatternRelationID=attack_model.ATTACKPATTERNRELATIONSHIP.FirstOrDefault(o => o.AttackPatternRefID == myattackpatternid && o.AttackPatternSubjectID == iAttackPatternLeveragedID).AttackPatternRelationshipID;
                                                                                                                                                    iAttackPatternRelationID = attack_model.ATTACKPATTERNRELATIONSHIP.Where(o => o.AttackPatternRefID == myattackpatternid && o.AttackPatternSubjectID == iAttackPatternLeveragedID).Select(o=>o.AttackPatternRelationshipID).FirstOrDefault();
                                                                                                                                                    
                                                                                                                                                }
                                                                                                                                                catch(Exception ex)
                                                                                                                                                {

                                                                                                                                                }

                                                                                                                                                //ATTACKPATTERNRELATIONSHIP oAttackPatternRelation = attack_model.ATTACKPATTERNRELATIONSHIP.Where(o => o.AttackPatternRefID == myattackpatternid && o.AttackPatternSubjectID == iAttackPatternLeveragedID).FirstOrDefault();
                                                                                                                                                //if(oAttackPatternRelation==null)
                                                                                                                                                if (iAttackPatternRelationID<=0)
                                                                                                                                                {
                                                                                                                                                    try
                                                                                                                                                    {
                                                                                                                                                        ATTACKPATTERNRELATIONSHIP oAttackPatternRelation = new ATTACKPATTERNRELATIONSHIP();
                                                                                                                                                        oAttackPatternRelation.AttackPatternRefID = myattackpatternid;
                                                                                                                                                        oAttackPatternRelation.AttackPatternSubjectID = iAttackPatternLeveragedID;
                                                                                                                                                        oAttackPatternRelation.RelationshipName = "Leverage";   //Hardcoded
                                                                                                                                                        oAttackPatternRelation.VocabularyID = iVocabularyCAPECID;
                                                                                                                                                        oAttackPatternRelation.CreatedDate = DateTimeOffset.Now;
                                                                                                                                                        oAttackPatternRelation.timestamp = DateTimeOffset.Now;
                                                                                                                                                        attack_model.ATTACKPATTERNRELATIONSHIP.Add(oAttackPatternRelation);
                                                                                                                                                        attack_model.SaveChanges();
                                                                                                                                                    }
                                                                                                                                                    catch(Exception exoAttackPatternRelation)
                                                                                                                                                    {
                                                                                                                                                        Console.WriteLine("Exception exoAttackPatternRelation " + exoAttackPatternRelation.Message + " " + exoAttackPatternRelation.InnerException);
                                                                                                                                                    }
                                                                                                                                                }
                                                                                                                                                else
                                                                                                                                                {
                                                                                                                                                    //Update ATTACKPATTERNRELATIONSHIP
                                                                                                                                                }

                                                                                                                                                int iAttackStepTechniqueLeveragedPatternID = 0;
                                                                                                                                                try
                                                                                                                                                {
                                                                                                                                                    //iAttackStepTechniqueLeveragedPatternID = attack_model.ATTACKSTEPTECHNIQUELEVERAGEDPATTERN.FirstOrDefault(o => o.AttackStepTechniqueID == iAttackStepTechniqueID && o.AttackPatternID == iAttackPatternLeveragedID).AttackStepTechniqueLeveragedPatternID;
                                                                                                                                                    iAttackStepTechniqueLeveragedPatternID = attack_model.ATTACKSTEPTECHNIQUELEVERAGEDPATTERN.Where(o => o.AttackStepTechniqueID == iAttackStepTechniqueID && o.AttackPatternID == iAttackPatternLeveragedID).Select(o=>o.AttackStepTechniqueLeveragedPatternID).FirstOrDefault();
                                                                                                                                                    
                                                                                                                                                }
                                                                                                                                                catch(Exception ex)
                                                                                                                                                {

                                                                                                                                                }
                                                                                                                                                
                                                                                                                                                //ATTACKSTEPTECHNIQUELEVERAGEDPATTERN oAttackStepTechniqueLeveragedPattern = attack_model.ATTACKSTEPTECHNIQUELEVERAGEDPATTERN.Where(o => o.AttackStepTechniqueID == oAttackStepTechnique.AttackStepTechniqueID && o.AttackPatternID == iAttackPatternLeveragedID).FirstOrDefault();
                                                                                                                                                //if(oAttackStepTechniqueLeveragedPattern==null)
                                                                                                                                                if (iAttackStepTechniqueLeveragedPatternID<=0)
                                                                                                                                                {
                                                                                                                                                    try
                                                                                                                                                    {
                                                                                                                                                        ATTACKSTEPTECHNIQUELEVERAGEDPATTERN oAttackStepTechniqueLeveragedPattern = new ATTACKSTEPTECHNIQUELEVERAGEDPATTERN();
                                                                                                                                                        oAttackStepTechniqueLeveragedPattern.AttackStepTechniqueID = iAttackStepTechniqueID;
                                                                                                                                                        oAttackStepTechniqueLeveragedPattern.AttackPatternID = iAttackPatternLeveragedID;
                                                                                                                                                        oAttackStepTechniqueLeveragedPattern.LeveragedAttackPatternOrder = Convert.ToInt32(sAttackStepTechniqueVocabularyID);    //<capec:Attack_Step_Technique ID="2">
                                                                                                                                                        oAttackStepTechniqueLeveragedPattern.VocabularyID = iVocabularyCAPECID;
                                                                                                                                                        oAttackStepTechniqueLeveragedPattern.CreatedDate = DateTimeOffset.Now;
                                                                                                                                                        oAttackStepTechniqueLeveragedPattern.timestamp = DateTimeOffset.Now;
                                                                                                                                                        attack_model.ATTACKSTEPTECHNIQUELEVERAGEDPATTERN.Add(oAttackStepTechniqueLeveragedPattern);
                                                                                                                                                        attack_model.SaveChanges();
                                                                                                                                                    }
                                                                                                                                                    catch(Exception exoAttackStepTechniqueLeveragedPattern)
                                                                                                                                                    {
                                                                                                                                                        Console.WriteLine("Exception exoAttackStepTechniqueLeveragedPattern " + exoAttackStepTechniqueLeveragedPattern.Message + " " + exoAttackStepTechniqueLeveragedPattern.InnerException);
                                                                                                                                                    }
                                                                                                                                                }
                                                                                                                                                else
                                                                                                                                                {
                                                                                                                                                    //Update ATTACKSTEPTECHNIQUELEVERAGEDPATTERN
                                                                                                                                                }

                                                                                                                                            }
                                                                                                                                            catch(Exception exLeveragedAttackPattern)
                                                                                                                                            {
                                                                                                                                                Console.WriteLine("Exception exLeveragedAttackPattern " + exLeveragedAttackPattern.Message + " " + exLeveragedAttackPattern.InnerException);
                                                                                                                                            }
                                                                                                                                        }
                                                                                                                                        break;
                                                                                                                                    
                                                                                                                                    default:
                                                                                                                                        Console.WriteLine("ERROR: Missing code for nodeAttackStepTechnique " + nodeAttackStepTechnique.Name);
                                                                                                                                        break;
                                                                                                                                }
                                                                                                                            }
                                                                                                                            break;
                                                                                                                        
                                                                                                                        default:
                                                                                                                            Console.WriteLine("ERROR: Missing code for nodeAttackStepTechniques " + nodeAttackStepTechniques.Name);
                                                                                                                            break;
                                                                                                                    }
                                                                                                                }
                                                                                                                break;
                                                                                                            #endregion attacksteptechniques
                                                                                                            
                                                                                                            case "capec:Indicators":
                                                                                                                #region capecindicators
                                                                                                                foreach (XmlNode nodeAttackStepIndicators in nodeAttackStepInfo.ChildNodes)
                                                                                                                {
                                                                                                                    //Console.WriteLine("DEBUG nodeAttackStepIndicators " + nodeAttackStepIndicators.Name);
                                                                                                                    switch (nodeAttackStepIndicators.Name)
                                                                                                                    {
                                                                                                                        case "capec:Indicator":
                                                                                                                            //TODO: Attack susceptibility, Link to CCE/OVAL?
                                                                                                                            //TODO: try catch
                                                                                                                            string sAttackStepIndicatorVocabularyID=nodeAttackStepIndicators.Attributes["ID"].InnerText;
                                                                                                                            string sAttackStepIndicatorType=nodeAttackStepIndicators.Attributes["type"].InnerText;  //Negative
                                                                                                                            string sAttackStepIndicatorDescription="";

                                                                                                                            //ATTACKSTEPINDICATOR oAttackStepIndicator = null;
                                                                                                                            int iAttackStepIndicatorID = 0;
                                                                                                                            foreach (XmlNode nodeAttackStepIndicator in nodeAttackStepIndicators.ChildNodes)
                                                                                                                            {
                                                                                                                                //Console.WriteLine("DEBUG nodeAttackStepIndicator " + nodeAttackStepIndicator.Name);
                                                                                                                                switch (nodeAttackStepIndicator.Name)
                                                                                                                                {
                                                                                                                                    case "capec:Indicator_Description":
                                                                                                                                        sAttackStepIndicatorDescription = CleaningCAPECString(nodeAttackStepIndicator.InnerText);
                                                                                                                                        
                                                                                                                                        //oAttackStepIndicator = attack_model.ATTACKSTEPINDICATOR.FirstOrDefault(o => o.AttackStepIndicatorDescription == sAttackStepIndicatorDescription);
                                                                                                                                        try
                                                                                                                                        {
                                                                                                                                            iAttackStepIndicatorID = attack_model.ATTACKSTEPINDICATOR.Where(o => o.AttackStepIndicatorDescription == sAttackStepIndicatorDescription).Select(o => o.AttackStepIndicatorID).FirstOrDefault();
                                                                                                                                        }
                                                                                                                                        catch(Exception ex)
                                                                                                                                        {

                                                                                                                                        }
                                                                                                                                        //if (oAttackStepIndicator == null)
                                                                                                                                        if (iAttackStepIndicatorID<=0)
                                                                                                                                        {
                                                                                                                                            

                                                                                                                                            try
                                                                                                                                            {
                                                                                                                                                ATTACKSTEPINDICATOR oAttackStepIndicator = new ATTACKSTEPINDICATOR();
                                                                                                                                                oAttackStepIndicator.AttackStepID = oAttackStep.AttackStepID;
                                                                                                                                                oAttackStepIndicator.AttackStepIndicatorDescription = sAttackStepIndicatorDescription;
                                                                                                                                                oAttackStepIndicator.AttackStepIndicatorType = sAttackStepIndicatorType;
                                                                                                                                                oAttackStepIndicator.AttackStepIndicatorVocabularyID = sAttackStepIndicatorVocabularyID;
                                                                                                                                                oAttackStepIndicator.VocabularyID = iVocabularyCAPECID;
                                                                                                                                                oAttackStepIndicator.CreatedDate = DateTimeOffset.Now;
                                                                                                                                                oAttackStepIndicator.timestamp = DateTimeOffset.Now;
                                                                                                                                                attack_model.ATTACKSTEPINDICATOR.Add(oAttackStepIndicator);
                                                                                                                                                attack_model.SaveChanges();
                                                                                                                                                iAttackStepIndicatorID = oAttackStepIndicator.AttackStepIndicatorID;
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
                                                                                                                                                Console.WriteLine("Exception DbEntityValidationExceptionATTACKSTEPINDICATOR " + sb.ToString());
                                                                                                                                            }
                                                                                                                                            catch (Exception exAddToAttackStepINDICATOR)
                                                                                                                                            {
                                                                                                                                                Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                                                                                                                                                Console.WriteLine("Exception exAddToAttackStepINDICATOR " + exAddToAttackStepINDICATOR.Message + " " + exAddToAttackStepINDICATOR.InnerException);
                                                                                                                                            }
                                                                                                                                        }
                                                                                                                                        else
                                                                                                                                        {
                                                                                                                                            //Update ATTACKSTEPINDICATOR
                                                                                                                                        }

                                                                                                                                        break;

                                                                                                                                    case "capec:Environments":
                                                                                                                                        //Console.WriteLine("ERROR: Missing code for nodeAttackStepIndicator capec:Environments " + nodeAttackStepIndicator.InnerText);
                                                                                                                                        //regex env-
                                                                                                                                        //ENVIRONMENT
                                                                                                                                        MatchCollection matches = Regex.Matches(nodeAttackStepIndicator.InnerText, @"env-\w+");
                                                                                                                                        foreach (Match match in matches)
                                                                                                                                        {
                                                                                                                                            string sEnvironmentName = match.Value.Trim(); //env-ClientServer
                                                                                                                                            int iEnvironmentID = 0;
                                                                                                                                            try
                                                                                                                                            {
                                                                                                                                                //iEnvironmentID = model.ENVIRONMENT.FirstOrDefault(o => o.CapecEnvironmentID == match.Value.Trim()).EnvironmentID;
                                                                                                                                                iEnvironmentID = model.ENVIRONMENT.Where(o => o.CapecEnvironmentID == sEnvironmentName).Select(o => o.EnvironmentID).FirstOrDefault();
                                                                                                                                                
                                                                                                                                            }
                                                                                                                                            catch (Exception ex)
                                                                                                                                            {

                                                                                                                                            }

                                                                                                                                            //ENVIRONMENT oEnvironment = model.ENVIRONMENT.FirstOrDefault(o => o.CapecEnvironmentID == match.Value.Trim());
                                                                                                                                            //if (oEnvironment == null)
                                                                                                                                            if (iEnvironmentID<=0)
                                                                                                                                            {
                                                                                                                                                try
                                                                                                                                                {
                                                                                                                                                    ENVIRONMENT oEnvironment = new ENVIRONMENT();
                                                                                                                                                    oEnvironment.CapecEnvironmentID = sEnvironmentName; // match.Value.Trim();
                                                                                                                                                    oEnvironment.EnvironmentTitle = sEnvironmentName.Replace("env-", "");
                                                                                                                                                    //oEnvironment.EnvironmentDescription=    //TODO
                                                                                                                                                    oEnvironment.CreatedDate = DateTimeOffset.Now;
                                                                                                                                                    oEnvironment.timestamp = DateTimeOffset.Now;
                                                                                                                                                    oEnvironment.VocabularyID = iVocabularyCAPECID;
                                                                                                                                                    model.ENVIRONMENT.Add(oEnvironment);
                                                                                                                                                    model.SaveChanges();
                                                                                                                                                    iEnvironmentID = oEnvironment.EnvironmentID;
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
                                                                                                                                                    Console.WriteLine("Exception DbEntityValidationExceptionENVIRONMENT " + sb.ToString());
                                                                                                                                                }
                                                                                                                                                catch(Exception exENVIRONMENT)
                                                                                                                                                {
                                                                                                                                                    Console.Write("Exception exENVIRONMENT " + exENVIRONMENT.Message + " " + exENVIRONMENT.InnerException);
                                                                                                                                                }
                                                                                                                                            }
                                                                                                                                            else
                                                                                                                                            {
                                                                                                                                                //Update ENVIRONMENT
                                                                                                                                            }

                                                                                                                                            int iAttackStepIndicatorEnvironmentID = 0;
                                                                                                                                            try
                                                                                                                                            {
                                                                                                                                                //iAttackStepIndicatorEnvironmentID = attack_model.ATTACKSTEPINDICATORENVIRONMENT.FirstOrDefault(o => o.AttackStepIndicatorID == oAttackStepIndicator.AttackStepIndicatorID && o.EnvironmentID == iEnvironmentID).AttackStepIndicatorEnvironmentID;
                                                                                                                                                iAttackStepIndicatorEnvironmentID = attack_model.ATTACKSTEPINDICATORENVIRONMENT.Where(o => o.AttackStepIndicatorID == iAttackStepIndicatorID && o.EnvironmentID == iEnvironmentID).Select(o=>o.AttackStepIndicatorEnvironmentID).FirstOrDefault();
                                                                                                                                                
                                                                                                                                            }
                                                                                                                                            catch(Exception ex)
                                                                                                                                            {

                                                                                                                                            }

                                                                                                                                            //ATTACKSTEPINDICATORENVIRONMENT oAttackStepIndicatorEnvironment = attack_model.ATTACKSTEPINDICATORENVIRONMENT.FirstOrDefault(o => o.AttackStepIndicatorID == oAttackStepIndicator.AttackStepIndicatorID && o.EnvironmentID == iEnvironmentID);
                                                                                                                                            //if(oAttackStepIndicatorEnvironment==null)
                                                                                                                                            if (iAttackStepIndicatorEnvironmentID<=0)
                                                                                                                                            {
                                                                                                                                                try
                                                                                                                                                {
                                                                                                                                                    ATTACKSTEPINDICATORENVIRONMENT oAttackStepIndicatorEnvironment = new ATTACKSTEPINDICATORENVIRONMENT();
                                                                                                                                                    oAttackStepIndicatorEnvironment.AttackStepIndicatorID = iAttackStepIndicatorID; // oAttackStepIndicator.AttackStepIndicatorID;
                                                                                                                                                    oAttackStepIndicatorEnvironment.EnvironmentID = iEnvironmentID; // oEnvironment.EnvironmentID;
                                                                                                                                                    oAttackStepIndicatorEnvironment.CreatedDate = DateTimeOffset.Now;
                                                                                                                                                    oAttackStepIndicatorEnvironment.timestamp = DateTimeOffset.Now;
                                                                                                                                                    oAttackStepIndicatorEnvironment.VocabularyID = iVocabularyCAPECID;
                                                                                                                                                    attack_model.ATTACKSTEPINDICATORENVIRONMENT.Add(oAttackStepIndicatorEnvironment);
                                                                                                                                                    attack_model.SaveChanges();    //TEST PERFORMANCE
                                                                                                                                                    //iAttackStepIndicatorEnvironmentID=
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
                                                                                                                                                    Console.WriteLine("Exception DbEntityValidationExceptionexATTACKSTEPINDICATORENVIRONMENT " + sb.ToString());
                                                                                                                                                }
                                                                                                                                                catch(Exception exATTACKSTEPINDICATORENVIRONMENT)
                                                                                                                                                {
                                                                                                                                                    Console.WriteLine("Exception exATTACKSTEPINDICATORENVIRONMENT " + exATTACKSTEPINDICATORENVIRONMENT.Message + " " + exATTACKSTEPINDICATORENVIRONMENT.InnerException);
                                                                                                                                                }
                                                                                                                                            }
                                                                                                                                            else
                                                                                                                                            {
                                                                                                                                                //Update ATTACKSTEPINDICATORENVIRONMENT
                                                                                                                                            }
                                                                                                                                        }

                                                                                                                                        ////ATTACKSTEPENVIRONMENT?
                                                                                                                                        //Console.WriteLine(nodeAttackStepIndicator.InnerText);
                                                                                                                                        break;

                                                                                                                                    default:
                                                                                                                                        Console.WriteLine("ERROR: Missing code for nodeAttackStepIndicator " + nodeAttackStepIndicator.Name);
                                                                                                                                        break;
                                                                                                                                }

                                                                                                                                //ATTACKSTEPINDICATOR oAttackStepIndicator=attack_model.ATTACKSTEPINDICATOR.FirstOrDefault(o=>o.AttackStepID==oAttackStep.AttackStepID && );
                                                                                                                                //TODO

                                                                                                                            }
                                                                                                                            break;
                                                                                                                        default:
                                                                                                                            Console.WriteLine("ERROR: Missing code for " + nodeAttackStepIndicators.Name);
                                                                                                                            break;
                                                                                                                    }

                                                                                                                }
                                                                                                                break;
                                                                                                                #endregion capecindicators

                                                                                                            case "capec:Outcomes":
                                                                                                                #region capecoutcomes
                                                                                                                foreach (XmlNode nodeAttackOutcomes in nodeAttackStepInfo.ChildNodes)
                                                                                                                {
                                                                                                                    //Console.WriteLine("DEBUG nodeAttackOutcomes " + nodeAttackOutcomes.Name);
                                                                                                                    switch (nodeAttackOutcomes.Name)
                                                                                                                    {
                                                                                                                        case "capec:Outcome":
                                                                                                                            string iOutcomeID= nodeAttackOutcomes.Attributes["ID"].InnerText;
                                                                                                                            string sOutcomeType= nodeAttackOutcomes.Attributes["type"].InnerText;  //Success

                                                                                                                            //capec:Outcome_Description
                                                                                                                                string sOutcomeDescription = CleaningCAPECString(nodeAttackOutcomes.InnerText);

                                                                                                                                int iAttackStepOutcomeID = 0;
                                                                                                                                try
                                                                                                                                {
                                                                                                                                    //iAttackStepOutcomeID = attack_model.ATTACKSTEPOUTCOME.FirstOrDefault(o => o.OutcomeDescription == sOutcomeDescription).AttackStepOutcomeID;
                                                                                                                                    iAttackStepOutcomeID = attack_model.ATTACKSTEPOUTCOME.Where(o => o.OutcomeDescription == sOutcomeDescription).Select(o=>o.AttackStepOutcomeID).FirstOrDefault();
                                                                                                                                    
                                                                                                                                }
                                                                                                                                catch(Exception ex)
                                                                                                                                {

                                                                                                                                }

                                                                                                                                //ATTACKSTEPOUTCOME oAttackStepOutcome = attack_model.ATTACKSTEPOUTCOME.FirstOrDefault(o => o.OutcomeDescription == sOutcomeDescription);  //TODO iOutcomeID
                                                                                                                                //if (oAttackStepOutcome == null)
                                                                                                                                if (iAttackStepOutcomeID<=0)
                                                                                                                                {
                                                                                                                                    //Note: Exception when no AttackStepID
                                                                                                                                    try
                                                                                                                                    {
                                                                                                                                        ATTACKSTEPOUTCOME oAttackStepOutcome = new ATTACKSTEPOUTCOME();
                                                                                                                                        oAttackStepOutcome.AttackStepID = oAttackStep.AttackStepID;
                                                                                                                                        //TODO: iOutcomeID
                                                                                                                                        oAttackStepOutcome.OutcomeType = sOutcomeType;
                                                                                                                                        oAttackStepOutcome.OutcomeDescription = sOutcomeDescription;
                                                                                                                                        oAttackStepOutcome.CreatedDate = DateTimeOffset.Now;
                                                                                                                                        oAttackStepOutcome.timestamp = DateTimeOffset.Now;
                                                                                                                                        oAttackStepOutcome.VocabularyID = iVocabularyCAPECID;
                                                                                                                                        attack_model.ATTACKSTEPOUTCOME.Add(oAttackStepOutcome);
                                                                                                                                        attack_model.SaveChanges();
                                                                                                                                        //iAttackStepOutcomeID=
                                                                                                                                    }
                                                                                                                                    catch (Exception exAddToAttackStepOUTCOME)
                                                                                                                                    {
                                                                                                                                        Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                                                                                                                                        Console.WriteLine("Exception exAddToAttackStepOUTCOME " + exAddToAttackStepOUTCOME.Message + " " + exAddToAttackStepOUTCOME.InnerException);
                                                                                                                                    }
                                                                                                                                }
                                                                                                                                else
                                                                                                                                {
                                                                                                                                    //Update ATTACKSTEPOUTCOME
                                                                                                                                }

                                                                                                                            break;

                                                                                                                        default:
                                                                                                                            Console.WriteLine("ERROR: Missing code for nodeAttackOutcomes " + nodeAttackOutcomes.Name);
                                                                                                                            break;
                                                                                                                    }
                                                                                                                }
                                                                                                                break;
                                                                                                                #endregion capecoutcomes
                                                                                                            case "capec:Security_Controls":
                                                                                                                #region capecsecuritycontrols
                                                                                                                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                                                                                Console.WriteLine("DEBUG capec:Security_Controls");
                                                                                                                int iSCcount = 0;
                                                                                                                foreach (XmlNode nodeSecurityControl in nodeAttackStepInfo.ChildNodes)
                                                                                                                {
                                                                                                                    string sSecurityControlID = "";
                                                                                                                    string sSecurityControlType = "";
                                                                                                                    //Console.WriteLine("DEBUG nodeSecurityControl " + nodeSecurityControl.Name);
                                                                                                                    switch (nodeSecurityControl.Name)
                                                                                                                    {
                                                                                                                        case "capec:Security_Control":
                                                                                                                            iSCcount++;
                                                                                                                            try
                                                                                                                            {
                                                                                                                                sSecurityControlID=nodeSecurityControl.Attributes["ID"].InnerText;
                                                                                                                            }
                                                                                                                            catch(Exception exnodeSecurityControlID)
                                                                                                                            {
                                                                                                                                Console.WriteLine("ERROR: "+sCAPECID+" no ID for nodeSecurityControl");
                                                                                                                            }
                                                                                                                            
                                                                                                                            //NOTE: we build our own ID
                                                                                                                            string sCAPECSecurityControlID = sCAPECID+"-"+iAttackStepVocabularyID+"-"+sSecurityControlID;    //nodeSecurityControl.Attributes["ID"].InnerText;
                                                                                                                            try
                                                                                                                            {
                                                                                                                                sSecurityControlType = nodeSecurityControl.Attributes["type"].InnerText; //Preventative
                                                                                                                                //TODO table SECURITYCONTROLTYPE
                                                                                                                            }
                                                                                                                            catch (Exception exSecurityControlType)
                                                                                                                            {
                                                                                                                                Console.WriteLine("ERROR: " + sCAPECID + " no exSecurityControlType for nodeSecurityControl");
                                                                                                                            }
                                                                                                                            string sSecurityControlDescription = "";

                                                                                                                            foreach (XmlNode nodeSecurityControlInfo in nodeSecurityControl.ChildNodes)
                                                                                                                            {
                                                                                                                                switch (nodeSecurityControlInfo.Name)
                                                                                                                                {
                                                                                                                                    case "capec:Security_Control_Description":
                                                                                                                                        sSecurityControlDescription = CleaningCAPECString(nodeSecurityControlInfo.InnerText);
                                                                                                                                        if (sSecurityControlDescription.Trim()=="")
                                                                                                                                        {
                                                                                                                                            Console.WriteLine("ERROR: "+sCAPECID+" sSecurityControlDescription " + nodeSecurityControlInfo.InnerText);
                                                                                                                                        }
                                                                                                                                        break;

                                                                                                                                    default:
                                                                                                                                        Console.WriteLine("ERROR: Missing code for "+sCAPECID+" nodeSecurityControlInfo " + nodeSecurityControlInfo.Name);
                                                                                                                                        break;
                                                                                                                                }
                                                                                                                            }

                                                                                                                            int iSecurityControlTypeID = 0;
                                                                                                                            //SECURITYCONTROLTYPE oSCType = null;
                                                                                                                            if (sSecurityControlType!="")
                                                                                                                            {
                                                                                                                                //TODO: Review this for mapping (i.e. with NIST SP 800-53, or Orange book)
                                                                                                                                
                                                                                                                                try
                                                                                                                                {
                                                                                                                                    //iSecurityControlTypeID = model.SECURITYCONTROLTYPE.FirstOrDefault(o => o.SecurityControlTypeName == sSecurityControlType).SecurityControlTypeID;
                                                                                                                                    iSecurityControlTypeID = model.SECURITYCONTROLTYPE.Where(o => o.SecurityControlTypeName == sSecurityControlType).Select(o=>o.SecurityControlTypeID).FirstOrDefault();
                                                                                                                                    
                                                                                                                                }
                                                                                                                                catch(Exception ex)
                                                                                                                                {

                                                                                                                                }

                                                                                                                                //oSCType = model.SECURITYCONTROLTYPE.FirstOrDefault(o => o.SecurityControlTypeName == sSecurityControlType);
                                                                                                                                //if(oSCType==null)
                                                                                                                                if (iSecurityControlTypeID<=0)
                                                                                                                                {
                                                                                                                                    try
                                                                                                                                    {
                                                                                                                                        SECURITYCONTROLTYPE oSCType = new SECURITYCONTROLTYPE();
                                                                                                                                        oSCType.CreatedDate = DateTimeOffset.Now;
                                                                                                                                        oSCType.SecurityControlTypeName = sSecurityControlType;
                                                                                                                                        oSCType.VocabularyID = iVocabularyCAPECID;
                                                                                                                                        oSCType.timestamp = DateTimeOffset.Now;
                                                                                                                                        model.SECURITYCONTROLTYPE.Add(oSCType);
                                                                                                                                        model.SaveChanges();
                                                                                                                                        iSecurityControlTypeID = oSCType.SecurityControlTypeID;
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
                                                                                                                                        Console.WriteLine("Exception DbEntityValidationExceptionmyattackpreq " + sb.ToString());
                                                                                                                                    }
                                                                                                                                    //TODO catch(exception
                                                                                                                                }
                                                                                                                                else
                                                                                                                                {
                                                                                                                                    //Update SECURITYCONTROLTYPE
                                                                                                                                }
                                                                                                                            }

                                                                                                                            if (sSecurityControlDescription.Trim() == "")
                                                                                                                            {
                                                                                                                                //TODO: Possible to retrieve the SECURITYCONTROLFAMILY? Parent?
                                                                                                                                //SECURITYCONTROL oSecurityControl = model.SECURITYCONTROL.FirstOrDefault(o => o.SecurityControlVocabularyID == sCAPECSecurityControlID);
                                                                                                                                SECURITYCONTROL oSecurityControl = model.SECURITYCONTROL.FirstOrDefault(o => o.SecurityControlDescription == sSecurityControlDescription);
                                                                                                                            
                                                                                                                                if (oSecurityControl == null)
                                                                                                                                {
                                                                                                                                    try
                                                                                                                                    {
                                                                                                                                        oSecurityControl = new SECURITYCONTROL();
                                                                                                                                        oSecurityControl.CreatedDate = DateTimeOffset.Now;
                                                                                                                                        oSecurityControl.SecurityControlVocabularyID = sCAPECSecurityControlID;
                                                                                                                                        oSecurityControl.VocabularyID = iVocabularyCAPECID;
                                                                                                                                        //if (oSCType != null)
                                                                                                                                        if (iSecurityControlTypeID>0)
                                                                                                                                        {
                                                                                                                                            oSecurityControl.SecurityControlTypeID = iSecurityControlTypeID;    // oSecurityControl.SecurityControlTypeID;
                                                                                                                                        }
                                                                                                                                        oSecurityControl.SecurityControlName = "";  //Required
                                                                                                                                        oSecurityControl.SecurityControlDescription = sSecurityControlDescription;
                                                                                                                                        oSecurityControl.timestamp = DateTimeOffset.Now;
                                                                                                                                        model.SECURITYCONTROL.Add(oSecurityControl);
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
                                                                                                                                        Console.WriteLine("Exception DbEntityValidationExceptionmyattackpreq " + sb.ToString());
                                                                                                                                    }
                                                                                                                                    //TODO catch(exception
                                                                                                                                }
                                                                                                                                else
                                                                                                                                {
                                                                                                                                    //Update SECURITYCONTROL
                                                                                                                                    //TODO REVIEW
                                                                                                                                    try
                                                                                                                                    {
                                                                                                                                        //if (oSCType != null)
                                                                                                                                        if (iSecurityControlTypeID > 0)
                                                                                                                                        {
                                                                                                                                            oSecurityControl.SecurityControlTypeID = iSecurityControlTypeID;    // oSecurityControl.SecurityControlTypeID;
                                                                                                                                        }
                                                                                                                                        //oSecurityControl.SecurityControlVocabularyID = sCAPECSecurityControlID; //REMOVED
                                                                                                                                        oSecurityControl.SecurityControlDescription = sSecurityControlDescription;
                                                                                                                                        oSecurityControl.timestamp = DateTimeOffset.Now;
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
                                                                                                                                        Console.WriteLine("Exception DbEntityValidationExceptionmyattackpreq " + sb.ToString());
                                                                                                                                    }
                                                                                                                                    //TODO catch(exception
                                                                                                                                }

                                                                                                                                int iAttackPatternSecurityControlID = 0;
                                                                                                                                try
                                                                                                                                {
                                                                                                                                    //iAttackPatternSecurityControlID = attack_model.ATTACKPATTERNSECURITYCONTROL.FirstOrDefault(o => o.AttackPatternID == myattackpatternid && o.SecurityControlID == oSecurityControl.SecurityControlID).AttackPatternSecurityControlID;
                                                                                                                                    iAttackPatternSecurityControlID = attack_model.ATTACKPATTERNSECURITYCONTROL.Where(o => o.AttackPatternID == myattackpatternid && o.SecurityControlID == oSecurityControl.SecurityControlID).Select(o => o.AttackPatternSecurityControlID).FirstOrDefault();
                                                                                                                                    
                                                                                                                                }
                                                                                                                                catch(Exception ex)
                                                                                                                                {

                                                                                                                                }

                                                                                                                                //ATTACKPATTERNSECURITYCONTROL oAttackPatternSC = attack_model.ATTACKPATTERNSECURITYCONTROL.FirstOrDefault(o=>o.AttackPatternID==myattackpatternid && o.SecurityControlID==oSecurityControl.SecurityControlID);
                                                                                                                                //if (oAttackPatternSC==null)
                                                                                                                                if (iAttackPatternSecurityControlID <= 0)
                                                                                                                                {
                                                                                                                                    try
                                                                                                                                    {
                                                                                                                                        ATTACKPATTERNSECURITYCONTROL oAttackPatternSC = new ATTACKPATTERNSECURITYCONTROL();
                                                                                                                                        oAttackPatternSC.CreatedDate = DateTimeOffset.Now;
                                                                                                                                        oAttackPatternSC.AttackPatternID = myattackpatternid;
                                                                                                                                        oAttackPatternSC.SecurityControlID = oSecurityControl.SecurityControlID;
                                                                                                                                        oAttackPatternSC.SecurityControlType = sSecurityControlType;    //TODO: maybe remove
                                                                                                                                        oAttackPatternSC.SecurityControlTypeID = iSecurityControlTypeID;
                                                                                                                                        
                                                                                                                                        oAttackPatternSC.AttackPatternSecurityControlOrder = iSCcount;
                                                                                                                                        oAttackPatternSC.VocabularyID = iVocabularyCAPECID;
                                                                                                                                        //oAttackPatternSC.AttackPatternSecurityControlVocabularyID = sSecurityControlID;   //TODO: needed?
                                                                                                                                        oAttackPatternSC.timestamp = DateTimeOffset.Now;
                                                                                                                                        attack_model.ATTACKPATTERNSECURITYCONTROL.Add(oAttackPatternSC);
                                                                                                                                        attack_model.SaveChanges();
                                                                                                                                        //iAttackPatternSecurityControlID=
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
                                                                                                                                        Console.WriteLine("Exception DbEntityValidationExceptionmyattackpreq " + sb.ToString());
                                                                                                                                    }
                                                                                                                                    //TODO catch(exception
                                                                                                                                }
                                                                                                                                else
                                                                                                                                {
                                                                                                                                    //Update ATTACKPATTERNSECURITYCONTROL

                                                                                                                                }

                                                                                                                                int iAttackStepSecurityControlID = 0;
                                                                                                                                try
                                                                                                                                {
                                                                                                                                    //iAttackStepSecurityControlID = attack_model.ATTACKSTEPSECURITYCONTROL.FirstOrDefault(o => o.AttackStepID == oAttackStep.AttackStepID && o.SecurityControlID == oSecurityControl.SecurityControlID).AttackStepSecurityControlID;
                                                                                                                                    iAttackStepSecurityControlID = attack_model.ATTACKSTEPSECURITYCONTROL.Where(o => o.AttackStepID == oAttackStep.AttackStepID && o.SecurityControlID == oSecurityControl.SecurityControlID).Select(o=>o.AttackStepSecurityControlID).FirstOrDefault();
                                                                                                                                    
                                                                                                                                }
                                                                                                                                catch(Exception ex)
                                                                                                                                {

                                                                                                                                }

                                                                                                                                //ATTACKSTEPSECURITYCONTROL oAttackStepSC = attack_model.ATTACKSTEPSECURITYCONTROL.FirstOrDefault(o => o.AttackStepID == oAttackStep.AttackStepID && o.SecurityControlID == oSecurityControl.SecurityControlID);
                                                                                                                                //if (oAttackStepSC == null)
                                                                                                                                if (iAttackStepSecurityControlID<=0)
                                                                                                                                {
                                                                                                                                    try
                                                                                                                                    {
                                                                                                                                        ATTACKSTEPSECURITYCONTROL oAttackStepSC = new ATTACKSTEPSECURITYCONTROL();
                                                                                                                                        oAttackStepSC.CreatedDate = DateTimeOffset.Now;
                                                                                                                                        oAttackStepSC.AttackStepID = oAttackStep.AttackStepID;
                                                                                                                                        oAttackStepSC.SecurityControlID = oSecurityControl.SecurityControlID;
                                                                                                                                        oAttackStepSC.VocabularyID = iVocabularyCAPECID;
                                                                                                                                        //oAttackStepSC.AttackPatternSecurityControlVocabularyID = sSecurityControlID;   //TODO: needed?
                                                                                                                                        oAttackStepSC.timestamp = DateTimeOffset.Now;
                                                                                                                                        attack_model.ATTACKSTEPSECURITYCONTROL.Add(oAttackStepSC);
                                                                                                                                        attack_model.SaveChanges();
                                                                                                                                        //iAttackStepSecurityControlID=
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
                                                                                                                                        Console.WriteLine("Exception DbEntityValidationExceptionmyattackpreq " + sb.ToString());
                                                                                                                                    }
                                                                                                                                    //TODO catch(exception
                                                                                                                                }
                                                                                                                                else
                                                                                                                                {
                                                                                                                                    //Update ATTACKSTEPSECURITYCONTROL

                                                                                                                                }
                                                                                                                            }

                                                                                                                            //TODO: Link to CCE/OVAL?
                                                                                                                            break;
                                                                                                                        default:
                                                                                                                            Console.WriteLine("ERROR: Missing code for " + nodeSecurityControl.Name);
                                                                                                                            break;
                                                                                                                    }
                                                                                                                }
                                                                                                                break;
                                                                                                                #endregion capecsecuritycontrols

                                                                                                            default:
                                                                                                                Console.WriteLine("ERROR: Missing code for nodeAttackStepInfo " + nodeAttackStepInfo.Name);
                                                                                                                break;
                                                                                                        }
                                                                                                    }
                                                                                                    break;
                                                                                                default:
                                                                                                    Console.WriteLine("ERROR: Missing code for nodeAttackStep " + nodeAttackStep.Name);
                                                                                                    break;
                                                                                            }
                                                                                        }
                                                                                        break;
                                                                                    default:
                                                                                        Console.WriteLine("ERROR: Missing code for nodeAttackSteps " + nodeAttackSteps.Name);
                                                                                        break;
                                                                                }
                                                                            }
                                                                            break;
                                                                        default:
                                                                            Console.WriteLine("ERROR: Missing code for nodeAttackPhase " + nodeAttackPhase.Name);
                                                                            break;

                                                                    }
                                                                    #endregion attackphaseinfos
                                                                }
                                                                break;
                                                            default:
                                                                Console.WriteLine("ERROR: Missing code for nodeAttackPhases " + nodeAttackPhases.Name);
                                                                break;
                                                        }
                                                    }
                                                    break;
                                                default:
                                                    Console.WriteLine("ERROR: nodeAttackExecutionFlow Missing code for " + nodeAttackExecutionFlow.Name);
                                                    break;
                                            }
                                        }
                                        break;
                                        #endregion attackexecutionflow

                                    default:
                                        Console.WriteLine("ERROR: nodeAttackPatternDescription Missing code for " + nodeAttackPatternDescription.Name);
                                        break;
                                }
                            }
                            //capec:Attack_Execution_Flow

                            break;
                            #endregion capecdescription

                        case "capec:Attack_Prerequisites":
                            #region AttackPrerequisite
                            foreach (XmlNode nodeAttackPrereq in nodeAP.ChildNodes)  //capec:Attack_Prerequisite
                            {
                                //Console.WriteLine(nodeAttackPrereq.Name);
                                foreach (XmlNode node4 in nodeAttackPrereq.ChildNodes)
                                {
                                    //Console.WriteLine(node4.Name);
                                    //capec:Text
                                    if (node4.Name == "capec:Text")
                                    {
                                        //Cleaning
                                        string sCleanPrerequisiteText = CleaningCAPECString(node4.InnerText);
                                        
                                        int iAttackPrerequisiteID=0;
                                        try
                                        {
                                            iAttackPrerequisiteID = attack_model.ATTACKPREREQUISITE.Where(o => o.PrerequisiteText == sCleanPrerequisiteText).Select(o=>o.AttackPrerequisiteID).FirstOrDefault();
                                        }
                                        catch(Exception ex)
                                        {

                                        }

                                        //XORCISMModel.ATTACKPREREQUISITE myattackpreq;
                                        //myattackpreq = attack_model.ATTACKPREREQUISITE.FirstOrDefault(o => o.PrerequisiteText == sCleanPrerequisiteText);    // && o.VocabularyID == 4);
                                        //if (myattackpreq == null)
                                        if (iAttackPrerequisiteID<=0)
                                        {
                                            try
                                            {
                                                Console.WriteLine(string.Format("DEBUG Adding new ATTACKPREREQUISITE [{0}] in table ATTACKPREREQUISITE", mycapecid));
                                                ATTACKPREREQUISITE myattackpreq = new ATTACKPREREQUISITE();
                                                myattackpreq.PrerequisiteText = sCleanPrerequisiteText;
                                                //myattackpreq.PrerequisiteTextRaw = node4.InnerText;
                                                myattackpreq.VocabularyID = iVocabularyCAPECID;
                                                myattackpreq.CreatedDate = DateTimeOffset.Now;
                                                myattackpreq.timestamp = DateTimeOffset.Now;
                                                attack_model.ATTACKPREREQUISITE.Add(myattackpreq);
                                                attack_model.SaveChanges();
                                                iAttackPrerequisiteID = myattackpreq.AttackPrerequisiteID;
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
                                                Console.WriteLine("Exception DbEntityValidationExceptionmyattackpreq " + sb.ToString());
                                            }
                                            catch (Exception exmyattackpreq)
                                            {
                                                Console.WriteLine("Exception exmyattackpreq " + exmyattackpreq.Message + " " + exmyattackpreq.InnerException);
                                            }
                                        }
                                        else
                                        {
                                            //UPDATE ATTACKPREREQUISITE
                                            //myattackpreq.PrerequisiteText = sCleanPrerequisiteText;
                                            //myattackpreq.timestamp = DateTimeOffset.Now;
                                        }



                                        int iAttackPatternPrerequisiteID = 0;
                                        try
                                        {
                                            iAttackPatternPrerequisiteID = attack_model.ATTACKPREREQUISITEFORATTACKPATTERN.Where(o => o.AttackPatternID == myattackpatternid && o.AttackPrerequisiteID == iAttackPrerequisiteID).Select(o=>o.AttackPatternAttackPrerequisiteID).FirstOrDefault();
                                        }
                                        catch(Exception ex)
                                        {

                                        }

                                        //XORCISMModel.ATTACKPREREQUISITEFORATTACKPATTERN myattackpreqforcapec;
                                        //myattackpreqforcapec = attack_model.ATTACKPREREQUISITEFORATTACKPATTERN.FirstOrDefault(o => o.capec_id == sCAPECID && o.AttackPrerequisiteID == iAttackPrerequisiteID);    // && o.VocabularyID == 4);
                                        //if (myattackpreqforcapec == null)
                                        if (iAttackPatternPrerequisiteID<=0)
                                        {
                                            Console.WriteLine(string.Format("DEBUG Adding new ATTACKPREREQUISITEFORATTACKPATTERN [{0}] in table ATTACKPREREQUISITEFORATTACKPATTERN", mycapecid));
                                            
                                            try
                                            {
                                                ATTACKPREREQUISITEFORATTACKPATTERN myattackpreqforcapec = new ATTACKPREREQUISITEFORATTACKPATTERN();
                                                //myattackpreqforcapec.capec_id = sCAPECID; //Removed
                                                myattackpreqforcapec.AttackPatternID = myattackpatternid;
                                                myattackpreqforcapec.AttackPrerequisiteID = iAttackPrerequisiteID;  // myattackpreq.AttackPrerequisiteID;
                                                myattackpreqforcapec.CreatedDate = DateTimeOffset.Now;
                                                myattackpreqforcapec.timestamp = DateTimeOffset.Now;
                                                myattackpreqforcapec.VocabularyID = iVocabularyCAPECID;
                                                attack_model.ATTACKPREREQUISITEFORATTACKPATTERN.Add(myattackpreqforcapec);
                                                attack_model.SaveChanges();
                                                //iAttackPatternPrerequisiteID=
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
                                                Console.WriteLine("Exception DbEntityValidationExceptionexAddToATTACKPREREQUISITEFORCAPEC " + sb.ToString());
                                            }
                                            catch (Exception exAddToATTACKPREREQUISITEFORCAPEC)
                                            {
                                                Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                                                Console.WriteLine("Exception exAddToATTACKPREREQUISITEFORCAPEC " + exAddToATTACKPREREQUISITEFORCAPEC.Message + " " + exAddToATTACKPREREQUISITEFORCAPEC.InnerException);
                                            }
                                        }
                                        else
                                        {
                                            //Update ATTACKPREREQUISITEFORATTACKPATTERN
                                        }
                                    }
                                }
                            }
                            #endregion AttackPrerequisite

                            break;
                        case "capec:Typical_Severity":
                            try
                            {
                                //mycapec.TypicalSeverity = nodeAP.InnerText;
                                //model.SaveChanges();
                                //Update ATTACKPATTERN
                                myattackpat.TypicalSeverity = nodeAP.InnerText;
                                myattackpat.timestamp = DateTimeOffset.Now;
                                //model.SaveChanges();  //Note: will be done later somewhere :)
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
                            catch(Exception exTypicalSeverity)
                            {
                                Console.WriteLine("Exception exTypicalSeverity " + exTypicalSeverity.Message + " " + exTypicalSeverity.InnerException);
                            }
                            break;


                        //TODO
                        
                        case "capec:Attack_Motivation-Consequences":
                            #region Attack_Motivation-Consequences
                            int iAttackConsequenceOrder = 0;
                            foreach (XmlNode nodeAMC in nodeAP.ChildNodes)  //capec:Attack_Motivation-Consequence
                            {
                                iAttackConsequenceOrder++;
                                int consid = 0;
                                //capec:Attack_Motivation-Consequence
                                //TODO: Review (order because no ID)  AttackPatternID
                                ATTACKPATTERNATTACKCONSEQUENCE oCAPECAttackConsequence = attack_model.ATTACKPATTERNATTACKCONSEQUENCE.FirstOrDefault(o => o.AttackPatternID == myattackpatternid && o.CAPECAttackConsequenceOrder == iAttackConsequenceOrder);
                                if(oCAPECAttackConsequence == null)
                                {
                                    Console.WriteLine(string.Format("DEBUG Adding new ATTACKPATTERNATTACKCONSEQUENCE [{0}] in table ATTACKPATTERNATTACKCONSEQUENCE", mycapecid));
                                    oCAPECAttackConsequence = new ATTACKPATTERNATTACKCONSEQUENCE();
                                    //oCAPECAttackConsequence.capec_id = sCAPECID;    //Removed
                                    oCAPECAttackConsequence.AttackPatternID=myattackpatternid;
                                    oCAPECAttackConsequence.CAPECAttackConsequenceOrder=iAttackConsequenceOrder;
                                    oCAPECAttackConsequence.CreatedDate = DateTimeOffset.Now;
                                    oCAPECAttackConsequence.timestamp = DateTimeOffset.Now;
                                    oCAPECAttackConsequence.VocabularyID = iVocabularyCAPECID;

                                    attack_model.ATTACKPATTERNATTACKCONSEQUENCE.Add(oCAPECAttackConsequence);
                                    attack_model.SaveChanges();
                                    
                                }
                                else
                                {
                                    //Update ATTACKPATTERNATTACKCONSEQUENCE (CAPECATTACKCONSEQUENCE)
                                    oCAPECAttackConsequence.CAPECAttackConsequenceOrder = iAttackConsequenceOrder;  //TODO: Review/Remove?
                                    oCAPECAttackConsequence.timestamp = DateTimeOffset.Now;
                                    oCAPECAttackConsequence.VocabularyID = iVocabularyCAPECID;
                                    model.SaveChanges();
                                }
                                consid = oCAPECAttackConsequence.AttackPatternAttackConsequenceID;

                                foreach (XmlNode nodeAMCchild in nodeAMC.ChildNodes)
                                {
                                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                    Console.WriteLine("DEBUG nodeAMCchild.Name=" + nodeAMCchild.Name);
                                    //capec:Consequence_Scope
                                    //capec:Consequence_Technical_Impact
                                    switch (nodeAMCchild.Name)
                                    {
                                        case "capec:Consequence_Scope":
                                            #region consequencescope
                                            int iAttackScopeID = 0;
                                            //TODO? Cleaning
                                            //ATTACKSCOPE oAttackScope;
                                            //oAttackScope = attack_model.ATTACKSCOPE.FirstOrDefault(o => o.ConsequenceScope == nodeAMCchild.InnerText);    //Note: CAPEC/CWE shared
                                            //if (oAttackScope == null)
                                            try
                                            {
                                                iAttackScopeID = attack_model.ATTACKSCOPE.Where(o => o.ConsequenceScope == nodeAMCchild.InnerText).Select(o=>o.AttackScopeID).FirstOrDefault();
                                            }
                                            catch(Exception ex)
                                            {

                                            }
                                            if (iAttackScopeID<=0)
                                            {
                                                Console.WriteLine(string.Format("DEBUG Adding new ATTACKCONSEQUENCESCOPE [{0}] in table ATTACKSCOPE", mycapecid));
                                                ATTACKSCOPE oAttackScope = new ATTACKSCOPE();
                                                oAttackScope.ConsequenceScope = nodeAMCchild.InnerText;
                                                oAttackScope.VocabularyID = iVocabularyCAPECID;
                                                oAttackScope.CreatedDate = DateTimeOffset.Now;
                                                oAttackScope.timestamp = DateTimeOffset.Now;
                                                attack_model.ATTACKSCOPE.Add(oAttackScope);
                                                attack_model.SaveChanges();
                                                iAttackScopeID = oAttackScope.AttackScopeID;
                                            }
                                            else
                                            {
                                                //Update ATTACKSCOPE
                                            }


                                            //CAPECATTACKCONSEQUENCESCOPE oCAPECAttackConsequenceScope=null;
                                            //oCAPECAttackConsequenceScope = model.CAPECATTACKCONSEQUENCESCOPE.FirstOrDefault(o => o.CAPECAttackConsequenceID == consid && o.AttackScopeID == oAttackScope.AttackScopeID);    // && o.VocabularyID == 4);
                                            //if (oCAPECAttackConsequenceScope == null)
                                            int iCAPECAttackConsequenceScopeID=0;
                                            try
                                            {
                                                iCAPECAttackConsequenceScopeID=attack_model.ATTACKPATTERNATTACKCONSEQUENCESCOPE.Where(o => o.AttackPatternAttackConsequenceID == consid && o.AttackScopeID == iAttackScopeID).Select(o=>o.AttackPatternAttackConsequenceScopeID).FirstOrDefault();
                                            }
                                            catch(Exception ex)
                                            {

                                            }
                                            if (iCAPECAttackConsequenceScopeID<=0)
                                            {
                                                Console.WriteLine(string.Format("DEBUG Adding new ATTACKPATTERNATTACKCONSEQUENCESCOPE [{0}] in table ATTACKPATTERNATTACKCONSEQUENCESCOPE", mycapecid));
                                                
                                                try
                                                {
                                                    ATTACKPATTERNATTACKCONSEQUENCESCOPE oCAPECAttackConsequenceScope = new ATTACKPATTERNATTACKCONSEQUENCESCOPE();
                                                    oCAPECAttackConsequenceScope.AttackPatternAttackConsequenceID = consid;
                                                    oCAPECAttackConsequenceScope.AttackScopeID = iAttackScopeID;    // oAttackScope.AttackScopeID;
                                                    oCAPECAttackConsequenceScope.CreatedDate = DateTimeOffset.Now;
                                                    oCAPECAttackConsequenceScope.timestamp = DateTimeOffset.Now;
                                                    oCAPECAttackConsequenceScope.VocabularyID = iVocabularyCAPECID;
                                                    attack_model.ATTACKPATTERNATTACKCONSEQUENCESCOPE.Add(oCAPECAttackConsequenceScope);
                                                    //attack_model.SaveChanges();    //TEST PERFORMANCE
                                                    //iCAPECAttackConsequenceScopeID=
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
                                                    Console.WriteLine("Exception DbEntityValidationExceptionexAddToCAPECATTACKCONSEQUENCESCOPE " + sb.ToString());
                                                }
                                                catch (Exception exAddToCAPECATTACKCONSEQUENCESCOPE)
                                                {
                                                    Console.WriteLine("Exception exAddToCAPECATTACKCONSEQUENCESCOPE " + exAddToCAPECATTACKCONSEQUENCESCOPE.Message + " " + exAddToCAPECATTACKCONSEQUENCESCOPE.InnerException);
                                                }
                                            }
                                            else
                                            {
                                                //Update CAPECATTACKCONSEQUENCESCOPE
                                            }
                                            #endregion consequencescope
                                            break;

                                        case "capec:Consequence_Technical_Impact":
                                            #region consequencetechnicalimpact
                                            //Cleaning
                                            string sCleanConsequenceTechnicalImpact = CleaningCAPECString(nodeAMCchild.InnerText);
                                            int iAttackTechnicalImpactID = 0;
                                            try
                                            {
                                                //Note: CAPEC/CWE shared
                                                iAttackTechnicalImpactID = attack_model.ATTACKTECHNICALIMPACT.Where(o => o.ConsequenceTechnicalImpact == sCleanConsequenceTechnicalImpact).Select(o=>o.AttackTechnicalImpactID).FirstOrDefault();
                                            }
                                            catch(Exception ex)
                                            {

                                            }

                                            //ATTACKTECHNICALIMPACT oAttackTechnicalImpact = new ATTACKTECHNICALIMPACT();
                                            //oAttackTechnicalImpact = attack_model.ATTACKTECHNICALIMPACT.FirstOrDefault(o => o.ConsequenceTechnicalImpact == sCleanConsequenceTechnicalImpact);    //Note: CAPEC/CWE shared
                                            //if (oAttackTechnicalImpact == null)
                                            if (iAttackTechnicalImpactID<=0)
                                            {
                                                Console.WriteLine(string.Format("DEBUG Adding new ATTACKTECHNICALIMPACT [{0}] in table ATTACKTECHNICALIMPACT", mycapecid));
                                                
                                                try
                                                {
                                                    ATTACKTECHNICALIMPACT oAttackTechnicalImpact = new ATTACKTECHNICALIMPACT();
                                                    oAttackTechnicalImpact.ConsequenceTechnicalImpact = sCleanConsequenceTechnicalImpact;
                                                    //oAttackTechnicalImpact.ConsequenceTechnicalImpactRaw = nodeAMCchild.InnerText;

                                                    oAttackTechnicalImpact.CreatedDate = DateTimeOffset.Now;
                                                    oAttackTechnicalImpact.timestamp = DateTimeOffset.Now;
                                                    oAttackTechnicalImpact.VocabularyID = iVocabularyCAPECID;
                                                    attack_model.ATTACKTECHNICALIMPACT.Add(oAttackTechnicalImpact);
                                                    attack_model.SaveChanges();
                                                    iAttackTechnicalImpactID = oAttackTechnicalImpact.AttackTechnicalImpactID;
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
                                                    Console.WriteLine("Exception DbEntityValidationExceptionexoAttackTechnicalImpact " + sb.ToString());
                                                }
                                                catch (Exception exoAttackTechnicalImpact)
                                                {
                                                    Console.WriteLine("Exception exoAttackTechnicalImpact " + exoAttackTechnicalImpact.Message + " " + exoAttackTechnicalImpact.InnerException);
                                                }
                                            }
                                            else
                                            {
                                                //Update ATTACKTECHNICALIMPACT
                                                //oAttackTechnicalImpact.ConsequenceTechnicalImpact = sCleanConsequenceTechnicalImpact;

                                                //oAttackTechnicalImpact.timestamp = DateTimeOffset.Now;
                                            }
                                            

                                            //CAPECATTACKTECHNICALIMPACT oCAPECAttackTechnicalImpact = null;
                                            //oCAPECAttackTechnicalImpact = model.CAPECATTACKTECHNICALIMPACT.FirstOrDefault(o => o.CAPECAttackConsequenceID == consid && o.AttackTechnicalImpactID == oAttackTechnicalImpact.AttackTechnicalImpactID);
                                            //if (oCAPECAttackTechnicalImpact == null)
                                            int iCAPECAttackTechnicalImpactID = 0;
                                            try
                                            {
                                                iCAPECAttackTechnicalImpactID=attack_model.ATTACKPATTERNATTACKTECHNICALIMPACT.Where(o => o.AttackPatternAttackConsequenceID == consid && o.AttackTechnicalImpactID == iAttackTechnicalImpactID).Select(o=>o.AttackPatternAttackTechnicalImpactID).FirstOrDefault();
                                            }
                                            catch(Exception ex)
                                            {

                                            }
                                            if(iCAPECAttackTechnicalImpactID<=0)
                                            {
                                                Console.WriteLine(string.Format("DEBUG Adding new ATTACKPATTERNATTACKTECHNICALIMPACT [{0}] in table ATTACKPATTERNATTACKTECHNICALIMPACT", mycapecid));
                                                try
                                                {
                                                    ATTACKPATTERNATTACKTECHNICALIMPACT oCAPECAttackTechnicalImpact = new ATTACKPATTERNATTACKTECHNICALIMPACT();
                                                    oCAPECAttackTechnicalImpact.AttackPatternAttackConsequenceID = consid; //myattackcons.AttackConsequenceID;
                                                    oCAPECAttackTechnicalImpact.AttackTechnicalImpactID = iAttackTechnicalImpactID; // oAttackTechnicalImpact.AttackTechnicalImpactID;
                                                    oCAPECAttackTechnicalImpact.CreatedDate = DateTimeOffset.Now;
                                                    oCAPECAttackTechnicalImpact.timestamp = DateTimeOffset.Now;
                                                    oCAPECAttackTechnicalImpact.VocabularyID = iVocabularyCAPECID;
                                                    attack_model.ATTACKPATTERNATTACKTECHNICALIMPACT.Add(oCAPECAttackTechnicalImpact);
                                                    //attack_model.SaveChanges();    //TEST PERFORMANCE
                                                    //iCAPECAttackTechnicalImpactID=
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
                                                    Console.WriteLine("Exception DbEntityValidationExceptionexoCAPECAttackTechnicalImpact " + sb.ToString());
                                                }
                                                catch (Exception exoCAPECAttackTechnicalImpact)
                                                {
                                                    Console.WriteLine("Exception exoCAPECAttackTechnicalImpact " + exoCAPECAttackTechnicalImpact.Message + " " + exoCAPECAttackTechnicalImpact.InnerException);
                                                }
                                            }
                                            else
                                            {
                                                //Update CAPECATTACKTECHNICALIMPACT
                                            }
                                            #endregion consequencetechnicalimpact
                                            break;

                                        case "capec:Consequence_Note":
                                        //TODO: Review
                                        //ATTACKCONSEQUENCE
                                            string sAttackConsequenceName=CleaningCAPECString(nodeAMCchild.InnerText);  //Information Leakage
                                            int iAttackConsequenceID = 0;
                                            try
                                            {
                                                iAttackConsequenceID = attack_model.ATTACKCONSEQUENCE.Where(o => o.Consequence == sAttackConsequenceName).Select(o=>o.AttackConsequenceID).FirstOrDefault();
                                            }
                                            catch(Exception ex)
                                            {
                                                iAttackConsequenceID = 0;
                                            }
                                            if (iAttackConsequenceID<=0)
                                            {
                                                try
                                                {
                                                    ATTACKCONSEQUENCE oAttackConsequence = new ATTACKCONSEQUENCE();
                                                    oAttackConsequence.CreatedDate = DateTimeOffset.Now;
                                                    oAttackConsequence.Consequence = sAttackConsequenceName;
                                                    oAttackConsequence.VocabularyID = iVocabularyCAPECID;
                                                    oAttackConsequence.timestamp = DateTimeOffset.Now;
                                                    attack_model.ATTACKCONSEQUENCE.Add(oAttackConsequence);
                                                    attack_model.SaveChanges();
                                                    iAttackConsequenceID = oAttackConsequence.AttackConsequenceID;
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
                                                    Console.WriteLine("Exception DbEntityValidationExceptionexoATTACKCONSEQUENCE " + sb.ToString());
                                                }
                                                catch (Exception exoATTACKCONSEQUENCE)
                                                {
                                                    Console.WriteLine("Exception exoATTACKCONSEQUENCE " + exoATTACKCONSEQUENCE.Message + " " + exoATTACKCONSEQUENCE.InnerException);
                                                }
                                            }
                                            else
                                            {
                                                //Update ATTACKCONSEQUENCE
                                            }

                                            //Update ATTACKPATTERNATTACKCONSEQUENCE (CAPECATTACKCONSEQUENCE)
                                            try
                                            {
                                                oCAPECAttackConsequence.AttackConsequenceID = iAttackConsequenceID;
                                                // Removed
                                                //oCAPECAttackConsequence.Consequence_Note = sAttackConsequenceName; //Information Leakage
                                                oCAPECAttackConsequence.timestamp = DateTimeOffset.Now;
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
                                                Console.WriteLine("Exception DbEntityValidationExceptionATTACKPATTERNATTACKCONSEQUENCE02 " + sb.ToString());
                                            }
                                            catch (Exception exATTACKPATTERNATTACKCONSEQUENCE02)
                                            {
                                                Console.WriteLine("Exception exATTACKPATTERNATTACKCONSEQUENCE02 " + exATTACKPATTERNATTACKCONSEQUENCE02.Message + " " + exATTACKPATTERNATTACKCONSEQUENCE02.InnerException);
                                            }
                                            break;

                                        default:
                                            Console.WriteLine("ERROR: TODO Missing code for capecConsequence " + nodeAMCchild.Name);
                                            break;
                                    }
                                }
                            }
                            #endregion Attack_Motivation-Consequences
                            break;
                        

                        case "capec:Related_Attack_Patterns":
                            #region Related_Attack_Patterns
                            string sTargetForm = "";
                            //string sNature = "";    //WARNING we can have multiple relationships materialized by multiple capec:Relationship_Nature
                            List<string> ListRelationshipNature = new List<string>();
                            //Console.WriteLine(nodeRelation.Name);   //capec:Relationship
                            foreach (XmlNode node3 in nodeAP.ChildNodes)
                            {
                                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                Console.WriteLine("DEBUG node3.name=" + node3.Name);    //capec:Related_Attack_Pattern
                                switch(node3.Name)
                                {
                                    
                                    case "capec:Relationship_Target_Form":
                                    
                                        sTargetForm = node3.InnerText;
                                        //Category
                                        //Attack Pattern
                                        break;
                                    case "capec:Relationship_Nature":
                                    
                                        //sNature = node3.InnerText;
                                        ListRelationshipNature.Add(node3.InnerText);
                                        //ChildOf
                                        //HasMember
                                        break;
                                    case "capec:Relationship_Target_ID":
                                    
                                        string scapecidTarget = "CAPEC-" + node3.InnerText;

                                        //TODO
                                        //Check if the related CAPEC exists

                                        //Check if the ATTACKPATTERN exists
                                        int iAttackPatternSubjectID = 0;
                                        try
                                        {
                                            iAttackPatternSubjectID = attack_model.ATTACKPATTERN.Where(o => o.capec_id == scapecidTarget).Select(o=>o.AttackPatternID).FirstOrDefault();
                                        }
                                        catch(Exception ex)
                                        {

                                        }

                                        //ATTACKPATTERN oAttackPatternRelated = attack_model.ATTACKPATTERN.FirstOrDefault(o => o.capec_id == scapecidTarget);
                                        //if (oAttackPatternRelated==null)
                                        if (iAttackPatternSubjectID<=0)
                                        {
                                            try
                                            {
                                                ATTACKPATTERN oAttackPatternRelated = new ATTACKPATTERN();
                                                oAttackPatternRelated.capec_id = scapecidTarget;
                                                //TODO
                                                oAttackPatternRelated.VocabularyID = iVocabularyCAPECID;
                                                oAttackPatternRelated.CreatedDate = DateTimeOffset.Now;
                                                oAttackPatternRelated.timestamp = DateTimeOffset.Now;
                                                attack_model.ATTACKPATTERN.Add(oAttackPatternRelated);
                                                attack_model.SaveChanges();
                                                iAttackPatternSubjectID = oAttackPatternRelated.AttackPatternID;
                                            }
                                            catch(Exception exoAttackPatternRelated)
                                            {
                                                Console.WriteLine("Exception exoAttackPatternRelated " + exoAttackPatternRelated.Message + " " + exoAttackPatternRelated.InnerException);
                                            }
                                        }
                                        else
                                        {
                                            //Update ATTACKPATTERN
                                        }

                                        //if (sTargetForm == "Category")
                                        //{
                                            //TODO REVIEW Hardcoded
                                            //if (mycapecid != "126" && mycapecid != "224" && mycapecid != "278")  //Because error    TODO HARCODED
                                            //{
                                                int iAttackPatternRelationshipID = 0;
                                                try
                                                {
                                                    iAttackPatternRelationshipID = attack_model.ATTACKPATTERNRELATIONSHIP.Where(o => o.AttackPatternRefID == myattackpatternid && o.AttackPatternSubjectID == iAttackPatternSubjectID).Select(o=>o.AttackPatternRelationshipID).FirstOrDefault();
                                                }
                                                catch(Exception ex)
                                                {

                                                }
                                                //XORCISMModel.ATTACKPATTERNRELATIONSHIP mycapecrel;
                                                //mycapecrel = attack_model.ATTACKPATTERNRELATIONSHIP.FirstOrDefault(o => o.AttackPatternRefID == myattackpatternid && o.AttackPatternSubjectID == oAttackPatternRelated.AttackPatternID);    // && o.VocabularyID == 4);
                                                //if (mycapecrel == null)
                                                if (iAttackPatternRelationshipID<=0)
                                                {
                                                    try
                                                    {
                                                        Console.WriteLine(string.Format("DEBUG25 Adding new CAPECRELATIONSHIP [{0}] in table CAPECRELATIONSHIP", mycapecid));
                                                        ATTACKPATTERNRELATIONSHIP mycapecrel = new ATTACKPATTERNRELATIONSHIP();
                                                        //TODO REVIEW THIS
                                                        //mycapecrel.capec_id = sCAPECID;
                                                        //TODO AttackPatternID
                                                        //mycapecrel.RelationshipNature = sNature;
                                                        //mycapecrel.RelationshipTargetForm = sTargetForm;
                                                        //mycapecrel.RelationshipTargetID = scapecidTarget;
                                                        mycapecrel.AttackPatternRefID = myattackpatternid;
                                                        mycapecrel.AttackPatternSubjectID = iAttackPatternSubjectID;    // oAttackPatternRelated.AttackPatternID;
                                                        //mycapecrel.RelationshipName //TODO
                                                        mycapecrel.CreatedDate = DateTimeOffset.Now;
                                                        mycapecrel.timestamp = DateTimeOffset.Now;
                                                        mycapecrel.VocabularyID = iVocabularyCAPECID;
                                                        attack_model.ATTACKPATTERNRELATIONSHIP.Add(mycapecrel);
                                                        attack_model.SaveChanges();
                                                    }
                                                    catch(Exception exmycapecrel)
                                                    {
                                                        Console.WriteLine("Exception exmycapecrel " + exmycapecrel.Message + " " + exmycapecrel.InnerException);
                                                    }
                                                }
                                                else
                                                {
                                                    //Update CAPECRELATIONSHIP
                                                }
                                            //}
                                        //}
                                        //if (sTargetForm == "Attack Pattern")
                                        //{
                                            //TODO

                                        //}
                                        break;
                                    case "capec:Related_Attack_Pattern":
                                        int iAttackView = 0;
                                        int iAttackPatternViewID = 0;
                                        int iTargetID = 0;
                                        string sRelationshipTargetForm = "";
                                        //string sRelationshipNature = "";    //WARNING we can have multiple relationships materialized by multiple capec:Relationship_Nature
                                        List<string> lListRelationshipNature = new List<string>();

                                        int iAttackPatternTargetID = 0;

                                        foreach (XmlNode nodeRAP in node3.ChildNodes)
                                        {
                                            string sOrdinal = "";
                                            Console.WriteLine("DEBUG nodeRAP.Name=" + nodeRAP.Name);
                                            switch(nodeRAP.Name)
                                            {
                                                case "capec:Relationship_Views":
                                                    #region relationshipviews
                                                    foreach (XmlNode nodeRAPV in nodeRAP.ChildNodes)
                                                    {
                                                        if (nodeRAPV.Name == "capec:Relationship_View_ID")
                                                        {
                                                            //<capec:Relationship_View_ID Ordinal="Primary">1000</capec:Relationship_View_ID>
                                                            
                                                            try
                                                            {
                                                                iAttackView = Int32.Parse(nodeRAPV.InnerText);  //1000
                                                            }
                                                            catch(Exception exiAttackView)
                                                            {
                                                                Console.WriteLine("Exception exiAttackView " + exiAttackView.Message + " " + exiAttackView.InnerException);
                                                            }
                                                            Console.WriteLine("DEBUG iAttackView="+iAttackView);
                                                            
                                                            try
                                                            {
                                                                sOrdinal = nodeRAPV.Attributes["Ordinal"].InnerText;
                                                            }
                                                            catch(Exception ex)
                                                            {

                                                            }

                                                            try
                                                            {
                                                                iAttackPatternViewID = attack_model.ATTACKPATTERNVIEW.Where(o => o.ViewVocabularyID == iAttackView).Select(o=>o.AttackPatternViewID).FirstOrDefault();
                                                            }
                                                            catch(Exception ex)
                                                            {
                                                                //e.g. empty table
                                                            }
                                                            if (iAttackPatternViewID <= 0)
                                                            {
                                                                try
                                                                {
                                                                    ATTACKPATTERNVIEW oAttackPatternView = new ATTACKPATTERNVIEW();
                                                                    oAttackPatternView.CreatedDate = DateTimeOffset.Now;
                                                                    oAttackPatternView.ViewVocabularyID = iAttackView;
                                                                    
                                                                    oAttackPatternView.VocabularyID = iVocabularyCAPECID;
                                                                    oAttackPatternView.timestamp = DateTimeOffset.Now;
                                                                    attack_model.ATTACKPATTERNVIEW.Add(oAttackPatternView);
                                                                    attack_model.SaveChanges();
                                                                    iAttackPatternViewID=oAttackPatternView.AttackPatternViewID;
                                                                }
                                                                catch (Exception exoAttackPatternView)
                                                                {
                                                                    Console.WriteLine("Exception exoAttackPatternView " + exoAttackPatternView.Message + " " + exoAttackPatternView.InnerException);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                //Update ATTACKPATTERNVIEW
                                                            }
                                                        }
                                                        else
                                                        {
                                                            Console.WriteLine("ERROR: Missing code forcapec:Related_Attack_Pattern "+ nodeRAPV.Name);
                                                        }
                                                    }
                                                    #endregion relationshipviews
                                                    break;

                                                case "capec:Relationship_Target_Form":
                                                    sRelationshipTargetForm = nodeRAP.InnerText; //Attack Pattern
                                                    Console.WriteLine("DEBUG sRelationshipTargetForm=" + sRelationshipTargetForm);
                                                    break;

                                                case "capec:Relationship_Nature":
                                                    //sRelationshipNature = nodeRAP.InnerText; //ChildOf
                                                    lListRelationshipNature.Add(nodeRAP.InnerText);
                                                    //Console.WriteLine("DEBUG sRelationshipNature=" + sRelationshipNature);
                                                    Console.WriteLine("DEBUG sRelationshipNature=" + nodeRAP.InnerText);

                                                    break;

                                                case "capec:Relationship_Target_ID":
                                                    //Note: we assume that it is the end
                                                    iTargetID = 0;
                                                    try
                                                    {
                                                        iTargetID = Int32.Parse(nodeRAP.InnerText);   //122 (CAPEC-ID)
                                                        string sCAPECTargetID="CAPEC-" + iTargetID;
                                                        try
                                                        {
                                                            iAttackPatternTargetID = attack_model.ATTACKPATTERN.Where(o => o.capec_id == sCAPECTargetID).Select(o=>o.AttackPatternID).FirstOrDefault();
                                                        }
                                                        catch(Exception ex)
                                                        {

                                                        }

                                                        if (iAttackPatternTargetID <= 0)
                                                        {
                                                            try
                                                            {
                                                                Console.WriteLine("DEBUG Adding new ATTACKPATTERN for CAPEC-" + iTargetID);
                                                                ATTACKPATTERN oAttackPattern = new ATTACKPATTERN();
                                                                oAttackPattern.CreatedDate = DateTimeOffset.Now;
                                                                oAttackPattern.capec_id = "CAPEC-" + iTargetID;

                                                                oAttackPattern.VocabularyID = iVocabularyCAPECID;
                                                                oAttackPattern.timestamp = DateTimeOffset.Now;
                                                                attack_model.ATTACKPATTERN.Add(oAttackPattern);
                                                                attack_model.SaveChanges();
                                                                iAttackPatternTargetID = oAttackPattern.AttackPatternID;
                                                            }
                                                            catch (Exception exoAttackPatternTarget)
                                                            {
                                                                Console.WriteLine("Exception exoAttackPatternTarget " + exoAttackPatternTarget.Message + " " + exoAttackPatternTarget.InnerException);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            //Update ATTACKPATTERN
                                                        }
                                                        Console.WriteLine("DEBUG iAttackPatternTargetID=" + iAttackPatternTargetID);



                                                    }
                                                    catch(Exception exiTargetID)
                                                    {
                                                        Console.WriteLine("Exception exiTargetID " + exiTargetID.Message + " " + exiTargetID.InnerException);
                                                    }


                                                    #region attackpatternviewrelationship
                                                    int iAttackPatternViewRelationshipID = 0;
                                                    try
                                                    {
                                                        //iAttackPatternViewRelationshipID = attack_model.ATTACKPATTERNVIEWRELATIONSHIP.Where(o => o.AttackPatternViewID == iAttackPatternViewID && o.Relationship_Target_Form == sRelationshipTargetForm && o.Relationship_Nature == sRelationshipNature && o.AttackPatternID == iAttackPatternTargetID).Select(o => o.AttackPatternViewRelationshipID).FirstOrDefault();
                                                        iAttackPatternViewRelationshipID = attack_model.ATTACKPATTERNVIEWRELATIONSHIP.Where(o => o.AttackPatternViewID == iAttackPatternViewID && o.Relationship_Target_Form == sRelationshipTargetForm && o.AttackPatternID == iAttackPatternTargetID).Select(o => o.AttackPatternViewRelationshipID).FirstOrDefault();
                                                        
                                                    }
                                                    catch(Exception ex)
                                                    {

                                                    }

                                                    //ATTACKPATTERNVIEWRELATIONSHIP oAttackPatternRelationship = attack_model.ATTACKPATTERNVIEWRELATIONSHIP.FirstOrDefault(o => o.AttackPatternViewID == iAttackPatternViewID && o.Relationship_Target_Form==sRelationshipTargetForm && o.Relationship_Nature==sRelationshipNature && o.AttackPatternID==iAttackPatternID);
                                                    //if(oAttackPatternRelationship==null)
                                                    if (iAttackPatternViewRelationshipID<=0)
                                                    {
                                                        try
                                                        {
                                                            ATTACKPATTERNVIEWRELATIONSHIP oAttackPatternRelationship = new ATTACKPATTERNVIEWRELATIONSHIP();
                                                            oAttackPatternRelationship.CreatedDate = DateTimeOffset.Now;
                                                            oAttackPatternRelationship.AttackPatternViewID = iAttackPatternViewID;
                                                            oAttackPatternRelationship.Relationship_Target_Form = sRelationshipTargetForm;  //TODO Remove?
                                                            //oAttackPatternRelationship.Relationship_Nature = sRelationshipNature;   //TODO Remove?
                                                            oAttackPatternRelationship.AttackPatternID = iAttackPatternTargetID;
                                                            oAttackPatternRelationship.Ordinal = sOrdinal;
                                                            oAttackPatternRelationship.VocabularyID = iVocabularyCAPECID;
                                                            oAttackPatternRelationship.timestamp = DateTimeOffset.Now;
                                                            attack_model.ATTACKPATTERNVIEWRELATIONSHIP.Add(oAttackPatternRelationship);
                                                            //attack_model.SaveChanges();    //TEST PERFORMANCE
                                                            //iAttackPatternViewRelationshipID=
                                                        }
                                                        catch(Exception exoAttackPatternRelationship)
                                                        {
                                                            Console.WriteLine("Exception exoAttackPatternRelationship " + exoAttackPatternRelationship.Message + " " + exoAttackPatternRelationship.InnerException);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //Update ATTACKPATTERNVIEWRELATIONSHIP
                                                        //TODO
                                                    }
                                                    #endregion attackpatternviewrelationship

                                                    #region attackpatternrelationship
                                                    //if (sRelationshipTargetForm=="Attack Pattern")  //Category  //Hardcoded
                                                    //{
                                                    foreach (string sRelationshipNature in lListRelationshipNature)
                                                    {
                                                        int iMyAttackPatternRelationshipID = 0;
                                                        try
                                                        {
                                                            iMyAttackPatternRelationshipID = attack_model.ATTACKPATTERNRELATIONSHIP.Where(o => o.AttackPatternRefID == myattackpatternid && o.RelationshipName == sRelationshipNature && o.AttackPatternSubjectID == iAttackPatternTargetID).Select(o => o.AttackPatternRelationshipID).FirstOrDefault();
                                                        }
                                                        catch (Exception ex)
                                                        {

                                                        }
                                                        if (iMyAttackPatternRelationshipID > 0)
                                                        {
                                                            //Update ATTACKPATTERNRELATIONSHIP
                                                        }
                                                        else
                                                        {
                                                            ATTACKPATTERNRELATIONSHIP oAttackPatternRelationship = new ATTACKPATTERNRELATIONSHIP();
                                                            oAttackPatternRelationship.CreatedDate = DateTimeOffset.Now;
                                                            oAttackPatternRelationship.AttackPatternRefID = myattackpatternid;
                                                            oAttackPatternRelationship.RelationshipName = sRelationshipNature;  //ChildOf   CanPrecede
                                                            oAttackPatternRelationship.AttackPatternSubjectID = iAttackPatternTargetID;
                                                            oAttackPatternRelationship.VocabularyID = iVocabularyCAPECID;
                                                            oAttackPatternRelationship.timestamp = DateTimeOffset.Now;
                                                            attack_model.ATTACKPATTERNRELATIONSHIP.Add(oAttackPatternRelationship);
                                                            attack_model.SaveChanges();
                                                            //iMyAttackPatternRelationshipID=
                                                        }
                                                    }
                                                    //}
                                                    #endregion attackpatternrelationship
                                                    break;

                                                case "capec:Relationship_Description":
                                                    //TODO Review
                                                    string sCAPECVIEWRelationshipDescription = nodeRAP.InnerText;
                                                    //<capec:Text>Log injection attack pattern is one of the components of the current attack pattern</capec:Text>
                                                    sCAPECVIEWRelationshipDescription = sCAPECVIEWRelationshipDescription.Replace("<capec:Text>", "");
                                                    sCAPECVIEWRelationshipDescription = sCAPECVIEWRelationshipDescription.Replace("</capec:Text>", "");
                                                    sCAPECVIEWRelationshipDescription = CleaningCAPECString(sCAPECVIEWRelationshipDescription);

                                                    //ATTACKPATTERNVIEWRELATIONSHIP oAttackPatternRelationship2 = attack_model.ATTACKPATTERNVIEWRELATIONSHIP.FirstOrDefault(o => o.AttackPatternViewID == iAttackPatternViewID && o.Relationship_Target_Form == sRelationshipTargetForm && o.Relationship_Nature == sRelationshipNature && o.AttackPatternID == iAttackPatternTargetID);
                                                    ATTACKPATTERNVIEWRELATIONSHIP oAttackPatternRelationship2 = attack_model.ATTACKPATTERNVIEWRELATIONSHIP.FirstOrDefault(o => o.AttackPatternViewID == iAttackPatternViewID && o.Relationship_Target_Form == sRelationshipTargetForm && o.AttackPatternID == iAttackPatternTargetID);
                                                    
                                                    if(oAttackPatternRelationship2==null)
                                                    {
                                                        Console.WriteLine("ERROR: oAttackPatternRelationship2");
                                                    }
                                                    else
                                                    {
                                                        //Update ATTACKPATTERNVIEWRELATIONSHIP
                                                        oAttackPatternRelationship2.Relationship_Description = sCAPECVIEWRelationshipDescription;
                                                        attack_model.SaveChanges();
                                                    }
                                                    break;

                                                default:
                                                    Console.WriteLine("ERROR: Missing code for capec:Related_Attack_Pattern " + nodeRAP.Name);
                                                    break;
                                            }

                                            
                                        }
                                        break;
                                    default:
                                        Console.WriteLine("ERROR: Missing code for capec:Related_Attack_Patterns "+node3.Name);
                                        break;
                                }
                            }
                            #endregion Related_Attack_Patterns
                            break;
                        
                        case "capec:Keywords":
                            #region keywords
                            foreach (XmlNode nodeKeyword in nodeAP.ChildNodes)  //capec:Keyword
                            {
                                if(nodeKeyword.Name!="capec:Keyword")
                                {
                                    Console.WriteLine("ERROR: Missing Code for capec:Keyword " + nodeKeyword.Name);
                                }
                                else
                                {
                                    string sTagValue=nodeKeyword.InnerText; //Ping
                                    sTagValue=CleaningCAPECString(sTagValue).Trim();
                                    int iTagID = 0;
                                    try
                                    {
                                        iTagID = model.TAG.Where(o => o.TagValue == sTagValue).Select(o => o.TagID).FirstOrDefault();
                                    }
                                    catch(Exception ex)
                                    {

                                    }

                                    if (iTagID <= 0)
                                    {
                                        try
                                        {
                                            TAG oTag = new TAG();
                                            oTag.TagType = "string";    //Keyword
                                            oTag.TagValue = sTagValue;
                                            oTag.VocabularyID = iVocabularyCAPECID;
                                            oTag.CreatedDate = DateTimeOffset.Now;
                                            oTag.timestamp = DateTimeOffset.Now;
                                            model.TAG.Add(oTag);
                                            model.SaveChanges();
                                            iTagID = oTag.TagID;
                                        }
                                        catch (Exception exTAG)
                                        {
                                            Console.WriteLine("Exception exTAG " + exTAG.Message + " " + exTAG.InnerException);
                                        }
                                    }

                                    int iAttackPatternTagID = 0;
                                    try
                                    {
                                        iAttackPatternTagID = attack_model.ATTACKPATTERNTAG.Where(o => o.AttackPatternID == myattackpatternid && o.TagID == iTagID).Select(o=>o.AttackPatternTagID).FirstOrDefault();
                                    }
                                    catch(Exception ex)
                                    {

                                    }
                                    //ATTACKPATTERNTAG oAttackPatternTag = attack_model.ATTACKPATTERNTAG.FirstOrDefault(o => o.AttackPatternID == myattackpatternid && o.TagID == iTagID);
                                    //if(oAttackPatternTag==null)
                                    if (iAttackPatternTagID<=0)
                                    {
                                        try
                                        {
                                            ATTACKPATTERNTAG oAttackPatternTag = new ATTACKPATTERNTAG();
                                            oAttackPatternTag.AttackPatternID = myattackpatternid;
                                            oAttackPatternTag.TagID = iTagID;
                                            oAttackPatternTag.VocabularyID = iVocabularyCAPECID;
                                            oAttackPatternTag.CreatedDate = DateTimeOffset.Now;
                                            oAttackPatternTag.timestamp = DateTimeOffset.Now;
                                            attack_model.ATTACKPATTERNTAG.Add(oAttackPatternTag);
                                            attack_model.SaveChanges();
                                            //iAttackPatternTagID=
                                        }
                                        catch(Exception exAttackPatternTag)
                                        {
                                            Console.WriteLine("Exception exAttackPatternTag " + exAttackPatternTag.Message + " " + exAttackPatternTag.InnerException);
                                        }
                                    }
                                    else
                                    {
                                        //Update ATTACKPATTERNTAG
                                    }
                                }
                            }
                            #endregion keywords
                            break;

                        case "capec:References":
                            #region ATTACKPATTERNREFERENCEs
                            foreach (XmlNode nodeRef in nodeAP.ChildNodes)  //capec:Reference
                            {
                                string sRefID = "";
                                try
                                {
                                    sRefID = nodeRef.Attributes["Reference_ID"].InnerText;
                                }
                                catch (Exception ex)
                                {
                                    string sIgnoreWarning = ex.Message;
                                    sRefID = "";
                                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                    Console.WriteLine("DEBUG Note: ATTACKPATTERNREFERENCE without Reference_ID for " + sCAPECID);
                                }

                                string sLocalRefID = "";
                                try
                                {
                                    sLocalRefID = nodeRef.Attributes["Local_Reference_ID"].InnerText;
                                }
                                catch (Exception ex)
                                {
                                    string sIgnoreWarning = ex.Message;
                                    sLocalRefID = "";
                                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                    Console.WriteLine("ERROR: ATTACKPATTERNREFERENCE without Local_Reference_ID for " + sCAPECID);
                                }

                                REFERENCE oReference = null;
                                string sReference_Edition = "";
                                string sReference_Publisher = "";
                                string sReference_Publication = "";
                                string sReference_PubDate = "";
                                string sReference_Date = "";

                                ATTACKPATTERNREFERENCE mycapecref = attack_model.ATTACKPATTERNREFERENCE.FirstOrDefault(o => o.AttackPatternID == myattackpatternid && o.Reference_ID == sRefID && o.Local_Reference_ID == sLocalRefID);    // && o.VocabularyID == 4);
                                if (mycapecref == null)
                                {
                                    
                                }
                                else
                                {
                                    //Update ATTACKPATTERNREFERENCE

                                    //TODO (Remove? to optimize speed)

                                    try
                                    {

                                        mycapecref.Reference_ID = sRefID;
                                        mycapecref.Local_Reference_ID = sLocalRefID;
                                        mycapecref.timestamp = DateTimeOffset.Now;
                                        model.SaveChanges();    
                                    }
                                    catch (Exception exmycapecrefUPDATE01)
                                    {
                                        Console.WriteLine("Exception exmycapecrefUPDATE01 " + exmycapecrefUPDATE01.Message + " " + exmycapecrefUPDATE01.InnerException);
                                    }

                                    oReference = model.REFERENCE.FirstOrDefault(o => o.ReferenceID == mycapecref.ReferenceID);
                                    /*
                                    int iReferenceID = 0;
                                    try
                                    {
                                        iReferenceID = model.REFERENCE.FirstOrDefault(o => o.ReferenceID == mycapecref.ReferenceID).ReferenceID;
                                    }
                                    catch(Exception ex)
                                    {

                                    }
                                    */
                                    if(oReference==null)
                                    {
                                        Console.WriteLine("NOTE: Reference not found for ATTACKPATTERNREFERENCE for "+sCAPECID);
                                        Console.WriteLine("DEBUG mycapecref.AttackPatternReferenceID=" + mycapecref.AttackPatternReferenceID);
                                        Console.WriteLine("DEBUG Reference_ID sRefID=" + sRefID);
                                        Console.WriteLine("DEBUG Local_Reference_ID sLocalRefID=" + sLocalRefID);
                                    }
                                    else
                                    {
                                        //Update REFERENCE
                                    }

                                }

                                Console.WriteLine("DEBUG Parsing the Reference Info");
                                string sReferenceSection = "";
                                #region referenceinfo
                                foreach (XmlNode nodeRefAtt in nodeRef.ChildNodes)
                                {
                                    Console.WriteLine("DEBUG nodeRefAtt.Name=" + nodeRefAtt.Name);
                                    try
                                    {
                                        
                                        switch (nodeRefAtt.Name)
                                        {
                                            case "capec:Reference_Author":
                                                string sRefAuthor = CleaningCAPECString(nodeRefAtt.InnerText);
                                                //AUTHOR myrefauthor = new AUTHOR();
                                                int iAuthorID=0;
                                                try
                                                {
                                                    iAuthorID=model.AUTHOR.Where(o => o.AuthorName == sRefAuthor).Select(o => o.AuthorID).FirstOrDefault();
                                                }
                                                catch(Exception ex)
                                                {

                                                }
                                                //myrefauthor = model.AUTHOR.FirstOrDefault(o => o.AuthorName == sRefAuthor);
                                                //if (myrefauthor == null)
                                                if (iAuthorID<=0)
                                                {
                                                    Console.WriteLine(string.Format("DEBUG Adding new AUTHOR found in References of [{0}] in table AUTHOR", sCAPECID));
                                                    AUTHOR myrefauthor = new AUTHOR();
                                                    myrefauthor.AuthorName = sRefAuthor;
                                                    //TODO: check for PersonID in PERSON or OrganisationID
                                                    myrefauthor.CreatedDate = DateTimeOffset.Now;
                                                    myrefauthor.timestamp = DateTimeOffset.Now;
                                                    myrefauthor.VocabularyID = iVocabularyCAPECID;
                                                    model.AUTHOR.Add(myrefauthor);
                                                    model.SaveChanges();
                                                    iAuthorID = myrefauthor.AuthorID;
                                                }
                                                else
                                                {
                                                    //Update AUTHOR
                                                }
                                                if (oReference==null)
                                                {
                                                    
                                                    oReference = new REFERENCE();
                                                    oReference.CreatedDate = DateTimeOffset.Now;

                                                    oReference.timestamp = DateTimeOffset.Now;
                                                    oReference.VocabularyID = iVocabularyCAPECID;
                                                    model.REFERENCE.Add(oReference);
                                                    model.SaveChanges();

                                                    Console.WriteLine("DEBUG New REFERENCE Added "+oReference.ReferenceID);
                                                    //Update ATTACKPATTERNREFERENCE
                                                    //mycapecref.timestamp = DateTimeOffset.Now;
                                                    //mycapecref.ReferenceID = oReference.ReferenceID;
                                                    //model.SaveChanges();
                                                    //Console.WriteLine("DEBUG ATTACKPATTERNREFERENCE Updated with ReferenceID=" + oReference.ReferenceID);
                                                    //Console.WriteLine("DEBUG " + mycapecref.timestamp);
                                                }
                                                else
                                                {
                                                    //Update REFERENCE

                                                    //Update ATTACKPATTERNREFERENCE
                                                    //mycapecref.timestamp = DateTimeOffset.Now;
                                                    //mycapecref.ReferenceID = oReference.ReferenceID;
                                                    //model.SaveChanges();
                                                    //Console.WriteLine("DEBUG ATTACKPATTERNREFERENCE Updated with ReferenceID=" + oReference.ReferenceID);
                                                    //Console.WriteLine("DEBUG " + mycapecref.timestamp);
                                                }

                                                int iReferenceAuthorID =0;
                                                try
                                                {
                                                    iReferenceAuthorID=model.REFERENCEAUTHOR.Where(o => o.ReferenceID == oReference.ReferenceID && o.AuthorID == iAuthorID).Select(o => o.ReferenceAuthorID).FirstOrDefault();
                                                }
                                                catch(Exception ex)
                                                {

                                                }
                                                //REFERENCEAUTHOR oReferenceAuthor = model.REFERENCEAUTHOR.FirstOrDefault(o => o.ReferenceID == oReference.ReferenceID && o.AuthorID == myrefauthor.AuthorID);
                                                //if (oReferenceAuthor==null)
                                                if (iReferenceAuthorID<=0)
                                                {
                                                    REFERENCEAUTHOR oReferenceAuthor = new REFERENCEAUTHOR();
                                                    oReferenceAuthor.AuthorID = iAuthorID;// myrefauthor.AuthorID;
                                                    oReferenceAuthor.ReferenceID = oReference.ReferenceID;
                                                    oReferenceAuthor.CreatedDate = DateTimeOffset.Now;
                                                    oReferenceAuthor.timestamp = DateTimeOffset.Now;
                                                    oReferenceAuthor.VocabularyID = iVocabularyCAPECID;
                                                    model.REFERENCEAUTHOR.Add(oReferenceAuthor);
                                                    model.SaveChanges();
                                                    //iReferenceAuthorID=
                                                }
                                                else
                                                {
                                                    //Update REFERENCEAUTHOR
                                                }
                                                break;

                                            case "capec:Reference_Title":
                                                if (oReference == null)
                                                {
                                                    oReference = new REFERENCE();
                                                    oReference.CreatedDate = DateTimeOffset.Now;
                                                    oReference.ReferenceTitle = CleaningCAPECString(nodeRefAtt.InnerText);
                                                    oReference.timestamp = DateTimeOffset.Now;
                                                    oReference.VocabularyID = iVocabularyCAPECID;
                                                
                                                    model.REFERENCE.Add(oReference);
                                                    model.SaveChanges();
                                                    Console.WriteLine("DEBUG New REFERENCE Added " + oReference.ReferenceID);
                                                    //Update ATTACKPATTERNREFERENCE
                                                    //mycapecref.timestamp = DateTimeOffset.Now;
                                                    //mycapecref.ReferenceID = oReference.ReferenceID;
                                                    //model.SaveChanges();
                                                    //Console.WriteLine("DEBUG ATTACKPATTERNREFERENCE Updated with ReferenceID=" + oReference.ReferenceID);
                                                    //Console.WriteLine("DEBUG " + mycapecref.timestamp);
                                                }
                                                else
                                                {
                                                    //Update REFERENCE
                                                    oReference.ReferenceTitle = CleaningCAPECString(nodeRefAtt.InnerText);
                                                    oReference.timestamp = DateTimeOffset.Now;
                                                    oReference.VocabularyID = iVocabularyCAPECID;
                                                    model.SaveChanges();

                                                    Console.WriteLine("DEBUG REFERENCE known " + oReference.ReferenceID);
                                                    //Update ATTACKPATTERNREFERENCE
                                                    //mycapecref.timestamp = DateTimeOffset.Now;
                                                    //mycapecref.ReferenceID = oReference.ReferenceID;
                                                    //model.SaveChanges();
                                                    //Console.WriteLine("DEBUG ATTACKPATTERNREFERENCE Updated with ReferenceID=" + oReference.ReferenceID);
                                                    //Console.WriteLine("DEBUG " + mycapecref.timestamp);
                                                }
                                                
                                                break;
                                            case "capec:Reference_Section":
                                                //TODO Review
                                                if (oReference != null) //TODO: comment
                                                {
                                                    sReferenceSection = CleaningCAPECString(nodeRefAtt.InnerText);
                                                    /*
                                                    //Update ATTACKPATTERNREFERENCE
                                                    mycapecref.Reference_Section = CleaningCAPECString(nodeRefAtt.InnerText);
                                                    mycapecref.timestamp = DateTimeOffset.Now;
                                                    mycapecref.ReferenceID = oReference.ReferenceID;
                                                    model.SaveChanges();
                                                    Console.WriteLine("DEBUG ATTACKPATTERNREFERENCE Updated with ReferenceID=" + oReference.ReferenceID);
                                                    Console.WriteLine("DEBUG mycapecref.timestamp=" + mycapecref.timestamp);
                                                    */
                                                }
                                                else
                                                {
                                                    //ERROR
                                                }
                                                break;

                                            case "capec:Reference_Edition":
                                                if (oReference != null)
                                                {
                                                    //Update REFERENCE
                                                    sReference_Edition = CleaningCAPECString(nodeRefAtt.InnerText);
                                                    oReference.Reference_Edition = sReference_Edition;
                                                    oReference.VocabularyID = iVocabularyCAPECID;
                                                    oReference.timestamp = DateTimeOffset.Now;

                                                    //Update ATTACKPATTERNREFERENCE
                                                    //mycapecref.timestamp = DateTimeOffset.Now;
                                                    //mycapecref.ReferenceID = oReference.ReferenceID;
                                                    //model.SaveChanges();
                                                    //Console.WriteLine("DEBUG ATTACKPATTERNREFERENCE Updated with ReferenceID=" + oReference.ReferenceID);
                                                    //Console.WriteLine("DEBUG " + mycapecref.timestamp);
                                                }
                                                else
                                                {
                                                    //ERROR
                                                }
                                                break;
                                            case "capec:Reference_Publisher":
                                                if (oReference != null)
                                                {
                                                    //Update REFERENCE
                                                    sReference_Publisher = CleaningCAPECString(nodeRefAtt.InnerText);
                                                    oReference.Reference_Publisher = sReference_Publisher;
                                                    oReference.VocabularyID = iVocabularyCAPECID;
                                                    oReference.timestamp = DateTimeOffset.Now;

                                                    //Update ATTACKPATTERNREFERENCE
                                                    //mycapecref.timestamp = DateTimeOffset.Now;
                                                    //mycapecref.ReferenceID = oReference.ReferenceID;
                                                    //model.SaveChanges();
                                                    //Console.WriteLine("DEBUG ATTACKPATTERNREFERENCE Updated with ReferenceID=" + oReference.ReferenceID);
                                                    //Console.WriteLine("DEBUG " + mycapecref.timestamp);
                                                }
                                                else
                                                {
                                                    //ERROR
                                                }
                                                break;
                                            case "capec:Reference_Publication":
                                                if (oReference != null)
                                                {
                                                    //Update REFERENCE
                                                    sReference_Publication = CleaningCAPECString(nodeRefAtt.InnerText);
                                                    oReference.Reference_Publication = sReference_Publication;
                                                    oReference.VocabularyID = iVocabularyCAPECID;
                                                    oReference.timestamp = DateTimeOffset.Now;

                                                    //Update ATTACKPATTERNREFERENCE
                                                    //mycapecref.timestamp = DateTimeOffset.Now;
                                                    //mycapecref.ReferenceID = oReference.ReferenceID;
                                                    //model.SaveChanges();
                                                    //Console.WriteLine("DEBUG ATTACKPATTERNREFERENCE Updated with ReferenceID=" + oReference.ReferenceID);
                                                    //Console.WriteLine("DEBUG " + mycapecref.timestamp);
                                                }
                                                break;
                                            case "capec:Reference_PubDate":
                                                if (oReference != null)
                                                {
                                                    //Update REFERENCE
                                                    sReference_PubDate = CleaningCAPECString(nodeRefAtt.InnerText);
                                                    oReference.Reference_PubDate = sReference_PubDate;
                                                    oReference.VocabularyID = iVocabularyCAPECID;
                                                    oReference.timestamp = DateTimeOffset.Now;

                                                    //Update ATTACKPATTERNREFERENCE
                                                    //mycapecref.timestamp = DateTimeOffset.Now;
                                                    //mycapecref.ReferenceID = oReference.ReferenceID;
                                                    //model.SaveChanges();
                                                    //Console.WriteLine("DEBUG ATTACKPATTERNREFERENCE Updated with ReferenceID=" + oReference.ReferenceID);
                                                    //Console.WriteLine("DEBUG " + mycapecref.timestamp);
                                                }
                                                break;
                                            case "capec:Reference_Link":
                                                string sReferenceURL = CleaningCAPECString(nodeRefAtt.InnerText);
                                                //Cleaning ReferenceURL
                                                //TODO Review (see NORMALIZEREFERENCE() in Import_all())    get the source
                                                sReferenceURL = sReferenceURL.Replace("http://www.", "http://");
                                                sReferenceURL = sReferenceURL.Replace("https://www.", "https://");
                                                sReferenceURL = sReferenceURL.Replace("http://osvdb.org/displayvuln.php?osvdbid=", "http://osvdb.org/");
                                                sReferenceURL = sReferenceURL.Replace("osvdb.org/show/osvdb/", "osvdb.org/");
                                                sReferenceURL = sReferenceURL.Replace("exploit-db.com/download/", "exploit-db.com/exploits/");
                                                sReferenceURL = sReferenceURL.Replace("securitytracker.com/id?", "securitytracker.com/id/");
                                                if(sReferenceURL.StartsWith("www."))
                                                {
                                                    sReferenceURL = "http://" + sReferenceURL;
                                                }
                                                if (oReference != null)
                                                {
                                                    //The REFERENCE exists
                                                    //Update REFERENCE
                                                    if (sReferenceURL.ToLower().Contains("http"))
                                                    {
                                                        oReference.ReferenceURL = sReferenceURL;
                                                        oReference.timestamp = DateTimeOffset.Now;
                                                        oReference.VocabularyID = iVocabularyCAPECID;

                                                        //Update ATTACKPATTERNREFERENCE
                                                        //mycapecref.timestamp = DateTimeOffset.Now;
                                                        //mycapecref.ReferenceID = oReference.ReferenceID;
                                                        //model.SaveChanges();
                                                        //Console.WriteLine("DEBUG ATTACKPATTERNREFERENCE Updated with ReferenceID=" + oReference.ReferenceID);
                                                        //Console.WriteLine("DEBUG " + mycapecref.timestamp);
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("ERROR: Reference_Link01 is not a URL " + sReferenceURL);
                                                    }
                                                }
                                                else
                                                {
                                                    //Check if Reference already exists in the database
                                                    oReference = model.REFERENCE.FirstOrDefault(o => o.ReferenceURL == sReferenceURL);
                                                    if (oReference == null)
                                                    {
                                                        oReference = new REFERENCE();
                                                        oReference.CreatedDate = DateTimeOffset.Now;
                                                        if (sReferenceURL.ToLower().Contains("http"))
                                                        {
                                                            oReference.ReferenceURL = sReferenceURL;
                                                            //TODO use ReferenceSource() of Import_all()
                                                            //TODO NORMALIZE
                                                        }
                                                        else
                                                        {
                                                            Console.WriteLine("ERROR: Reference_Link is not a URL " + sReferenceURL);
                                                        }
                                                        oReference.Reference_Edition = sReference_Edition;
                                                        oReference.Reference_Publication = sReference_Publication;
                                                        oReference.Reference_Publisher = sReference_Publisher;
                                                        oReference.Reference_PubDate = sReference_PubDate;
                                                        oReference.Reference_Date = sReference_Date;
                                                        //TODO: Search ISBN...
                                                        oReference.VocabularyID = iVocabularyCAPECID;
                                                        oReference.timestamp = DateTimeOffset.Now;
                                                        model.REFERENCE.Add(oReference);
                                                        model.SaveChanges();
                                                        Console.WriteLine("DEBUG New REFERENCE Added " + oReference.ReferenceID);
                                                        //Update ATTACKPATTERNREFERENCE
                                                        //mycapecref.timestamp = DateTimeOffset.Now;
                                                        //mycapecref.ReferenceID = oReference.ReferenceID;
                                                        //model.SaveChanges();
                                                        //Console.WriteLine("DEBUG ATTACKPATTERNREFERENCE Updated with ReferenceID=" + oReference.ReferenceID);
                                                        //Console.WriteLine("DEBUG " + mycapecref.timestamp);
                                                    }
                                                    else
                                                    {
                                                        //Update REFERENCE
                                                        //Update the Reference found in the database
                                                        //TODO Review
                                                        if(sReference_Edition!="") oReference.Reference_Edition = sReference_Edition;
                                                        if (sReference_Publication != "") oReference.Reference_Publication = sReference_Publication;
                                                        if (sReference_Publisher != "") oReference.Reference_Publisher = sReference_Publisher;
                                                        if (sReference_PubDate != "") oReference.Reference_PubDate = sReference_PubDate;
                                                        //TODO: Search ISBN...
                                                        oReference.timestamp = DateTimeOffset.Now;

                                                        //Update ATTACKPATTERNREFERENCE
                                                        //mycapecref.timestamp = DateTimeOffset.Now;
                                                        //mycapecref.ReferenceID = oReference.ReferenceID;
                                                        //model.SaveChanges();
                                                        //Console.WriteLine("DEBUG ATTACKPATTERNREFERENCE Updated with ReferenceID=" + oReference.ReferenceID);
                                                        //Console.WriteLine("DEBUG " + mycapecref.timestamp);
                                                    }
                                                }
                                                break;

                                            case "capec:Reference_Date":
                                                if (oReference != null)
                                                {
                                                    //Update REFERENCE
                                                    sReference_Date = CleaningCAPECString(nodeRefAtt.InnerText);
                                                    oReference.Reference_Date = sReference_Date;
                                                    oReference.VocabularyID = iVocabularyCAPECID;
                                                    oReference.timestamp = DateTimeOffset.Now;
                                                    //Update ATTACKPATTERNREFERENCE
                                                    //mycapecref.timestamp = DateTimeOffset.Now;
                                                    //mycapecref.ReferenceID = oReference.ReferenceID;
                                                    //model.SaveChanges();
                                                    //Console.WriteLine("DEBUG ATTACKPATTERNREFERENCE Updated with ReferenceID=" + oReference.ReferenceID);
                                                    //Console.WriteLine("DEBUG " + mycapecref.timestamp);
                                                }
                                                break;
                                            default:
                                                Console.WriteLine("ERROR: TODO Missing code for nodeRefAtt.Name " + nodeRefAtt.Name);
                                                break;
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
                                        Console.WriteLine("Exception DbEntityValidationExceptionREFERENCE " + sb.ToString());
                                    }
                                    catch(Exception exnodeRefAtt)
                                    {
                                        Console.WriteLine("Exception exnodeRefAtt for "+sCAPECID+" " + exnodeRefAtt.Message + " " + exnodeRefAtt.InnerException);
                                    }
                                }
                                #endregion referenceinfo

                                if (mycapecref == null)
                                {
                                    Console.WriteLine(string.Format("DEBUG Adding new ATTACKPATTERNREFERENCE for [{0}] in table ATTACKPATTERNREFERENCE", sCAPECID));
                                    try
                                    {
                                        mycapecref = new ATTACKPATTERNREFERENCE();
                                        mycapecref.AttackPatternID = myattackpatternid;
                                        mycapecref.ReferenceID = oReference.ReferenceID;
                                        mycapecref.Reference_Section = sReferenceSection;
                                        mycapecref.Reference_ID = sRefID;
                                        mycapecref.Local_Reference_ID = sLocalRefID;

                                        mycapecref.CreatedDate = DateTimeOffset.Now;
                                        mycapecref.timestamp = DateTimeOffset.Now;
                                        mycapecref.VocabularyID = iVocabularyCAPECID;
                                        attack_model.ATTACKPATTERNREFERENCE.Add(mycapecref);
                                        attack_model.SaveChanges();
                                        Console.WriteLine("DEBUG Added new ATTACKPATTERNREFERENCE " + mycapecref.AttackPatternReferenceID);
                                    }
                                    catch (Exception exAddATTACKPATTERNREFERENCE)
                                    {
                                        Console.WriteLine("Exception exAddATTACKPATTERNREFERENCE " + exAddATTACKPATTERNREFERENCE.Message + " " + exAddATTACKPATTERNREFERENCE.InnerException);
                                    }
                                }
                                try
                                {
                                    //Update ATTACKPATTERNREFERENCE
                                    mycapecref.ReferenceID = oReference.ReferenceID;
                                    mycapecref.Reference_Section = sReferenceSection;
                                    mycapecref.timestamp = DateTimeOffset.Now;
                                    model.SaveChanges();
                                    Console.WriteLine("DEBUG ATTACKPATTERNREFERENCE Updated with ReferenceID=" + oReference.ReferenceID);
                                    
                                }
                                catch(Exception exATTACKPATTERNREF)
                                {
                                    Console.WriteLine("Exception exATTACKPATTERNREF " + exATTACKPATTERNREF.Message + " " + exATTACKPATTERNREF.InnerException);
                                }
                            }
                            #endregion ATTACKPATTERNREFERENCEs
                            break;

                        case "capec:Other_Notes":
                            Console.WriteLine("ERROR: Missing code for capec:Other_Notes");
                            //TODO
                            break;
                        case "capec:CIA_Impact":
                            #region CAPECCIA_Impact
                            XORCISMModel.CIAIMPACTFORATTACKPATTERN myciaimpactforcapec;
                            myciaimpactforcapec = model.CIAIMPACTFORATTACKPATTERN.FirstOrDefault(o => o.AttackPatternID == myattackpatternid);
                            if (myciaimpactforcapec == null)
                            {
                                Console.WriteLine(string.Format("DEBUG Adding new CIAIMPACTFORATTACKPATTERN [{0}] in table CIAIMPACTFORATTACKPATTERN", mycapecid));
                                try
                                {
                                    myciaimpactforcapec = new CIAIMPACTFORATTACKPATTERN();
                                    myciaimpactforcapec.AttackPatternID = myattackpatternid;
                                    //myciaimpactforcapec.capec_id = sCAPECID;    //Removed
                                    myciaimpactforcapec.CreatedDate = DateTimeOffset.Now;
                                    myciaimpactforcapec.timestamp = DateTimeOffset.Now;
                                    myciaimpactforcapec.VocabularyID = iVocabularyCAPECID;
                                    model.CIAIMPACTFORATTACKPATTERN.Add(myciaimpactforcapec);
                                    model.SaveChanges();
                                }
                                catch(Exception exmyciaimpactforcapec)
                                {
                                    Console.WriteLine("Exception exmyciaimpactforcapec " + exmyciaimpactforcapec.Message + " " + exmyciaimpactforcapec.InnerException);
                                }
                            }
                            else
                            {
                                //Update CIAIMPACTFORCAPEC
                                myciaimpactforcapec.timestamp = DateTimeOffset.Now;
                            }

                            foreach (XmlNode nodeCIA in nodeAP.ChildNodes)
                            {
                                switch (nodeCIA.Name)
                                {
                                    case "capec:Confidentiality_Impact":
                                        myciaimpactforcapec.Confidentiality_Impact = nodeCIA.InnerText;
                                        break;
                                    case "capec:Integrity_Impact":
                                        myciaimpactforcapec.Integrity_Impact = nodeCIA.InnerText;
                                        break;
                                    case "capec:Availability_Impact":
                                        myciaimpactforcapec.Availability_Impact = nodeCIA.InnerText;
                                        break;
                                    default:
                                        Console.WriteLine("ERROR: Missing code for CIAImpactForCapec " + nodeCIA.Name);
                                        break;
                                }
                            }
                            try
                            {
                                model.SaveChanges();
                            }
                            catch(Exception exCIAImpact)
                            {
                                Console.WriteLine("Exception exCIAImpact " + exCIAImpact.Message + " " + exCIAImpact.InnerException);
                            }
                            #endregion CAPECCIA_Impact
                            break;

                        case "capec:Purposes":
                            #region CAPECPurposes
                            foreach (XmlNode nodePurpose in nodeAP.ChildNodes)
                            {
                                switch (nodePurpose.Name)
                                {
                                    case "capec:Purpose":
                                        string sAttackPurposeName = nodePurpose.InnerText.Trim();
                                        int iAttackPurposeID = 0;
                                        try
                                        {
                                            iAttackPurposeID = attack_model.ATTACKPURPOSE.Where(o => o.AttackPurposeName == sAttackPurposeName).Select(o=>o.AttackPurposeID).FirstOrDefault();
                                        }
                                        catch(Exception ex)
                                        {

                                        }
                                        //XORCISMModel.ATTACKPURPOSE myattackpurpose;
                                        //myattackpurpose = attack_model.ATTACKPURPOSE.FirstOrDefault(o => o.AttackPurposeName == sAttackPurposeName);   //&& o.VocabularyID == 4
                                        //if (myattackpurpose == null)
                                        if (iAttackPurposeID<=0)
                                        {
                                            Console.WriteLine(string.Format("DEBUG Adding new ATTACKPURPOSE [{0}] in table ATTACKPURPOSE", mycapecid));
                                            try
                                            {
                                                ATTACKPURPOSE myattackpurpose = new ATTACKPURPOSE();
                                                myattackpurpose.AttackPurposeName = sAttackPurposeName;
                                                myattackpurpose.VocabularyID = iVocabularyCAPECID;
                                                myattackpurpose.CreatedDate = DateTimeOffset.Now;
                                                myattackpurpose.timestamp = DateTimeOffset.Now;
                                                attack_model.ATTACKPURPOSE.Add(myattackpurpose);
                                                attack_model.SaveChanges();
                                                iAttackPurposeID = myattackpurpose.AttackPurposeID;
                                            }
                                            catch(Exception exmyattackpurpose)
                                            {
                                                Console.WriteLine("Exception exmyattackpurpose " + exmyattackpurpose.Message + " " + exmyattackpurpose.InnerException);
                                            }
                                        }
                                        else
                                        {
                                            //Update ATTACKPURPOSE
                                        }

                                        int iAttackPatternPurposeID = 0;
                                        try
                                        {
                                            iAttackPatternPurposeID = attack_model.ATTACKPURPOSEFORATTACKPATTERN.Where(o => o.AttackPurposeID == iAttackPurposeID && o.AttackPatternID == myattackpatternid).Select(o=>o.AttackPatternPurposeID).FirstOrDefault();
                                        }
                                        catch(Exception ex)
                                        {

                                        }
                                        //XORCISMModel.ATTACKPURPOSEFORATTACKPATTERN myattackpurposeforattackpattern;
                                        //myattackpurposeforattackpattern = attack_model.ATTACKPURPOSEFORATTACKPATTERN.FirstOrDefault(o => o.AttackPurposeID == iAttackPurposeID && o.AttackPatternID == myattackpatternid);  //&& o.capec_id = sCAPECID
                                        //if (myattackpurposeforattackpattern == null)
                                        if (iAttackPatternPurposeID<=0)
                                        {
                                            Console.WriteLine(string.Format("DEBUG Adding new ATTACKPURPOSEFORATTACKPATTERN [{0}] in table ATTACKPURPOSEFORATTACKPATTERN", mycapecid));
                                            try
                                            {
                                                ATTACKPURPOSEFORATTACKPATTERN myattackpurposeforattackpattern = new ATTACKPURPOSEFORATTACKPATTERN();
                                                myattackpurposeforattackpattern.AttackPurposeID = iAttackPurposeID; // myattackpurpose.AttackPurposeID;
                                                myattackpurposeforattackpattern.AttackPatternID = myattackpatternid;
                                                //myattackpurposeforattackpattern.capec_id = sCAPECID;    //Removed
                                                myattackpurposeforattackpattern.CreatedDate = DateTimeOffset.Now;
                                                myattackpurposeforattackpattern.timestamp = DateTimeOffset.Now;
                                                myattackpurposeforattackpattern.VocabularyID = iVocabularyCAPECID;
                                                attack_model.ATTACKPURPOSEFORATTACKPATTERN.Add(myattackpurposeforattackpattern);
                                                //attack_model.SaveChanges();    //TEST PERFORMANCE
                                                //iAttackPatternPurposeID=
                                            }
                                            catch(Exception exmyattackpurposeforattackpattern)
                                            {
                                                Console.WriteLine("Exception exmyattackpurposeforattackpattern " + exmyattackpurposeforattackpattern.Message + " " + exmyattackpurposeforattackpattern.InnerException);
                                            }
                                        }
                                        else
                                        {
                                            //Update ATTACKPURPOSEFORATTACKPATTERN
                                        }
                                        break;
                                    default:
                                        Console.WriteLine("ERROR: Missing code for capec:Purposes " + nodePurpose.Name);
                                        break;
                                }
                            }
                            #endregion CAPECPurposes
                            break;
                        case "capec:Technical_Context":
                            #region CAPECTechnical_Context
                            //Optimization because no update
                            //TECHNICALCONTEXT mytechnicalcontext;
                            //mytechnicalcontext = model.TECHNICALCONTEXT.FirstOrDefault(o => o.capec_id == sCAPECID);
                            int technicalcontextid = 0;
                            try
                            {
                                technicalcontextid = model.TECHNICALCONTEXT.Where(o => o.AttackPatternID == myattackpatternid).Select(o => o.TechnicalContextID).FirstOrDefault();
                            }
                            catch(Exception ex)
                            {

                            }
                            //if (mytechnicalcontext == null)
                            if (technicalcontextid <= 0)
                            {
                                Console.WriteLine(string.Format("DEBUG Adding new TECHNICALCONTEXT [{0}] in table TECHNICALCONTEXT", mycapecid));
                                TECHNICALCONTEXT mytechnicalcontext = new TECHNICALCONTEXT();
                                mytechnicalcontext.CreatedDate = DateTimeOffset.Now;
                                //mytechnicalcontext.capec_id = sCAPECID; //Removed
                                mytechnicalcontext.AttackPatternID = myattackpatternid;
                                mytechnicalcontext.VocabularyID = iVocabularyCAPECID;
                                mytechnicalcontext.timestamp = DateTimeOffset.Now;
                                model.TECHNICALCONTEXT.Add(mytechnicalcontext);
                                model.SaveChanges();
                                technicalcontextid = mytechnicalcontext.TechnicalContextID;
                            }
                            else
                            {
                                //Update TECHNICALCONTEXT
                            }
                            
                            
                            foreach (XmlNode nodeTechCon in nodeAP.ChildNodes)
                            {
                                switch (nodeTechCon.Name)
                                {
                                    case "capec:Architectural_Paradigms":
                                        #region capecarchitecturalparadigms
                                        foreach (XmlNode nodeTechCon2 in nodeTechCon.ChildNodes)
                                        {
                                            //TODO: test if nodeTechCon2.Name=="capec:Architectural_Paradigm"
                                            //TODO: nodeTechCon2.InnerText could be null
                                            int iArchitecturalParadigmID = 0;
                                            try
                                            {
                                                iArchitecturalParadigmID = model.ARCHITECTURALPARADIGM.Where(o => o.ArchitecturalParadigmName == nodeTechCon2.InnerText).Select(o=>o.ArchitecturalParadigmID).FirstOrDefault();
                                            }
                                            catch(Exception ex)
                                            {

                                            }
                                            
                                            //XORCISMModel.ARCHITECTURALPARADIGM myarchiparadigm;
                                            //myarchiparadigm = model.ARCHITECTURALPARADIGM.FirstOrDefault(o => o.ArchitecturalParadigmName == nodeTechCon2.InnerText);   //&& o.VocabularyID == 4
                                            //if (myarchiparadigm == null)
                                            if (iArchitecturalParadigmID<=0)
                                            {
                                                Console.WriteLine(string.Format("DEBUG Adding new ARCHITECTURALPARADIGM [{0}] in table ARCHITECTURALPARADIGM", mycapecid));
                                                ARCHITECTURALPARADIGM myarchiparadigm = new ARCHITECTURALPARADIGM();
                                                myarchiparadigm.ArchitecturalParadigmName = nodeTechCon2.InnerText; //TODO? Cleaning
                                                myarchiparadigm.VocabularyID = iVocabularyCAPECID;
                                                myarchiparadigm.CreatedDate = DateTimeOffset.Now;
                                                myarchiparadigm.timestamp = DateTimeOffset.Now;
                                                model.ARCHITECTURALPARADIGM.Add(myarchiparadigm);
                                                model.SaveChanges();
                                                iArchitecturalParadigmID = myarchiparadigm.ArchitecturalParadigmID;
                                            }
                                            else
                                            {
                                                //Update ARCHITECTURALPARADIGM
                                            }

                                            int iTechnicalContextArchitecturalParadigmID=0;
                                            try
                                            {
                                                iTechnicalContextArchitecturalParadigmID = model.ARCHITECTURALPARADIGMFORTECHNICALCONTEXT.Where(o => o.ArchitecturalParadigmID == iArchitecturalParadigmID && o.TechnicalContextID == technicalcontextid).Select(o=>o.TechnicalContextArchitecturalParadigmID).FirstOrDefault();
                                            }
                                            catch(Exception ex)
                                            {

                                            }

                                            //XORCISMModel.ARCHITECTURALPARADIGMFORTECHNICALCONTEXT myarchiparadigmfortechcontext;
                                            //myarchiparadigmfortechcontext = model.ARCHITECTURALPARADIGMFORTECHNICALCONTEXT.FirstOrDefault(o => o.ArchitecturalParadigmID == iArchitecturalParadigmID && o.TechnicalContextID == technicalcontextid);
                                            //if (myarchiparadigmfortechcontext == null)
                                            if (iTechnicalContextArchitecturalParadigmID<=0)
                                            {
                                                Console.WriteLine(string.Format("DEBUG Adding new ARCHITECTURALPARADIGMFORTECHNICALCONTEXT [{0}] in table ARCHITECTURALPARADIGMFORTECHNICALCONTEXT", mycapecid));
                                                ARCHITECTURALPARADIGMFORTECHNICALCONTEXT myarchiparadigmfortechcontext = new ARCHITECTURALPARADIGMFORTECHNICALCONTEXT();
                                                myarchiparadigmfortechcontext.ArchitecturalParadigmID = iArchitecturalParadigmID;   // myarchiparadigm.ArchitecturalParadigmID;
                                                myarchiparadigmfortechcontext.TechnicalContextID = technicalcontextid;
                                                myarchiparadigmfortechcontext.CreatedDate = DateTimeOffset.Now;
                                                myarchiparadigmfortechcontext.VocabularyID = iVocabularyCAPECID;
                                                myarchiparadigmfortechcontext.timestamp = DateTimeOffset.Now;
                                                model.ARCHITECTURALPARADIGMFORTECHNICALCONTEXT.Add(myarchiparadigmfortechcontext);
                                                model.SaveChanges();
                                                //iTechnicalContextArchitecturalParadigmID=
                                            }
                                            else
                                            {
                                                //Update ARCHITECTURALPARADIGMFORTECHNICALCONTEXT
                                            }
                                        }
                                        break;
                                        #endregion capecarchitecturalparadigms
                                    case "capec:Frameworks":
                                        #region capecframeworks
                                        foreach (XmlNode nodeTechCon2 in nodeTechCon.ChildNodes)
                                        {
                                            //TODO: test if nodeTechCon2.Name=="capec:Framework"
                                            //TODO: nodeTechCon2.InnerText could be null
                                            int iFrameworkID = 0;
                                            try
                                            {
                                                iFrameworkID = model.FRAMEWORK.Where(o => o.FrameworkName == nodeTechCon2.InnerText).Select(o=>o.FrameworkID).FirstOrDefault();
                                            }
                                            catch(Exception ex)
                                            {

                                            }

                                            //XORCISMModel.FRAMEWORK myframework;
                                            //myframework = model.FRAMEWORK.FirstOrDefault(o => o.FrameworkName == nodeTechCon2.InnerText);   //&& o.VocabularyID == 4
                                            //if (myframework == null)
                                            if (iFrameworkID<=0)
                                            {
                                                Console.WriteLine(string.Format("DEBUG Adding new FRAMEWORK [{0}] in table FRAMEWORK", mycapecid));
                                                FRAMEWORK myframework = new FRAMEWORK();
                                                myframework.FrameworkName = nodeTechCon2.InnerText; //TODO? Cleaning
                                                myframework.VocabularyID = iVocabularyCAPECID;
                                                myframework.CreatedDate = DateTimeOffset.Now;
                                                myframework.timestamp = DateTimeOffset.Now;
                                                model.FRAMEWORK.Add(myframework);
                                                model.SaveChanges();
                                                iFrameworkID = myframework.FrameworkID;
                                            }

                                            int iTechnicalContextFrameworkID = 0;
                                            try
                                            {
                                                iTechnicalContextFrameworkID = model.FRAMEWORKFORTECHNICALCONTEXT.Where(o => o.FrameworkID == iFrameworkID && o.TechnicalContextID == technicalcontextid).Select(o=>o.TechnicalContextFrameworkID).FirstOrDefault();
                                            }
                                            catch(Exception ex)
                                            {

                                            }
                                            //XORCISMModel.FRAMEWORKFORTECHNICALCONTEXT myframeworkfortechcontext;
                                            //myframeworkfortechcontext = model.FRAMEWORKFORTECHNICALCONTEXT.FirstOrDefault(o => o.FrameworkID == iFrameworkID && o.TechnicalContextID == technicalcontextid);
                                            //if (myframeworkfortechcontext == null)
                                            if (iTechnicalContextFrameworkID<=0)
                                            {
                                                Console.WriteLine(string.Format("DEBUG Adding new FRAMEWORKFORTECHNICALCONTEXT [{0}] in table FRAMEWORKFORTECHNICALCONTEXT", mycapecid));
                                                FRAMEWORKFORTECHNICALCONTEXT myframeworkfortechcontext = new FRAMEWORKFORTECHNICALCONTEXT();
                                                myframeworkfortechcontext.FrameworkID = iFrameworkID;   // myframework.FrameworkID;
                                                myframeworkfortechcontext.TechnicalContextID = technicalcontextid;
                                                myframeworkfortechcontext.CreatedDate = DateTimeOffset.Now;
                                                myframeworkfortechcontext.VocabularyID = iVocabularyCAPECID;
                                                myframeworkfortechcontext.timestamp = DateTimeOffset.Now;
                                                model.FRAMEWORKFORTECHNICALCONTEXT.Add(myframeworkfortechcontext);
                                                model.SaveChanges();
                                                //iTechnicalContextFrameworkID=
                                            }
                                            else
                                            {
                                                //Update FRAMEWORKFORTECHNICALCONTEXT
                                            }
                                        }
                                        break;
                                        #endregion capecframeworks
                                    case "capec:Platforms":
                                        #region capecplatforms
                                        foreach (XmlNode nodeTechCon2 in nodeTechCon.ChildNodes)
                                        {
                                            //TODO: test if nodeTechCon2.Name=="capec:Platform"
                                            //TODO: nodeTechCon2.InnerText could be null
                                            int iPlatformID = 0;
                                            try
                                            {
                                                iPlatformID = model.PLATFORM.Where(o => o.PlatformName == nodeTechCon2.InnerText).Select(o=>o.PlatformID).FirstOrDefault();
                                            }
                                            catch(Exception ex)
                                            {

                                            }
                                            //XORCISMModel.PLATFORM myplatform;
                                            //myplatform = model.PLATFORM.FirstOrDefault(o => o.PlatformName == nodeTechCon2.InnerText);   //&& o.VocabularyID == 4
                                            //if (myplatform == null)
                                            if (iPlatformID<=0)
                                            {
                                                Console.WriteLine(string.Format("DEBUG Adding new PLATFORM [{0}] in table PLATFORM", mycapecid));
                                                PLATFORM myplatform = new PLATFORM();
                                                myplatform.PlatformName = nodeTechCon2.InnerText;   //TODO? Cleaning
                                                myplatform.VocabularyID = iVocabularyCAPECID;
                                                myplatform.CreatedDate = DateTimeOffset.Now;
                                                myplatform.timestamp = DateTimeOffset.Now;
                                                model.PLATFORM.Add(myplatform);
                                                model.SaveChanges();
                                                iPlatformID = myplatform.PlatformID;
                                            }
                                            else
                                            {
                                                //Update PLATFORM
                                            }

                                            int iTechnicalContextPlatformID = 0;
                                            try
                                            {
                                                iTechnicalContextPlatformID = model.PLATFORMFORTECHNICALCONTEXT.Where(o => o.PlatformID == iPlatformID && o.TechnicalContextID == technicalcontextid).Select(o=>o.TechnicalContextPlatformID).FirstOrDefault();
                                            }
                                            catch(Exception ex)
                                            {

                                            }
                                            //XORCISMModel.PLATFORMFORTECHNICALCONTEXT myplatformfortechcontext;
                                            //myplatformfortechcontext = model.PLATFORMFORTECHNICALCONTEXT.FirstOrDefault(o => o.PlatformID == iPlatformID && o.TechnicalContextID == technicalcontextid);
                                            //if (myplatformfortechcontext == null)
                                            if (iTechnicalContextPlatformID<=0)
                                            {
                                                Console.WriteLine(string.Format("DEBUG Adding new PLATFORMFORTECHNICALCONTEXT [{0}] in table PLATFORMFORTECHNICALCONTEXT", mycapecid));
                                                PLATFORMFORTECHNICALCONTEXT myplatformfortechcontext = new PLATFORMFORTECHNICALCONTEXT();
                                                myplatformfortechcontext.PlatformID = iPlatformID;  // myplatform.PlatformID;
                                                myplatformfortechcontext.TechnicalContextID = technicalcontextid;
                                                myplatformfortechcontext.CreatedDate = DateTimeOffset.Now;
                                                myplatformfortechcontext.timestamp = DateTimeOffset.Now;
                                                myplatformfortechcontext.VocabularyID = iVocabularyCAPECID;
                                                model.PLATFORMFORTECHNICALCONTEXT.Add(myplatformfortechcontext);
                                                model.SaveChanges();
                                                //iTechnicalContextPlatformID=
                                            }
                                            else
                                            {
                                                //Update PLATFORMFORTECHNICALCONTEXT
                                            }
                                        }
                                        break;
                                        #endregion capecplatforms
                                    case "capec:Languages":
                                        #region capeclanguages
                                        foreach (XmlNode nodeTechCon2 in nodeTechCon.ChildNodes)
                                        {
                                            //TODO: test if nodeTechCon2.Name=="capec:Language"
                                            //TODO: nodeTechCon2.InnerText could be null
                                            int iLanguageID = 0;
                                            try
                                            {
                                                iLanguageID = model.LANGUAGE.Where(o => o.LanguageName == nodeTechCon2.InnerText).Select(o=>o.LanguageID).FirstOrDefault();
                                            }
                                            catch(Exception ex)
                                            {

                                            }
                                            //XORCISMModel.LANGUAGE mylanguage;
                                            //mylanguage = model.LANGUAGE.FirstOrDefault(o => o.LanguageName == nodeTechCon2.InnerText);   //&& o.VocabularyID == 4
                                            //if (mylanguage == null)
                                            if (iLanguageID<=0)
                                            {
                                                Console.WriteLine(string.Format("DEBUG Adding new LANGUAGE [{0}] in table LANGUAGE", nodeTechCon2.InnerText));
                                                LANGUAGE mylanguage = new LANGUAGE();
                                                mylanguage.LanguageName = nodeTechCon2.InnerText;   //TODO? Cleaning
                                                mylanguage.VocabularyID = iVocabularyCAPECID;
                                                mylanguage.CreatedDate = DateTimeOffset.Now;
                                                mylanguage.timestamp = DateTimeOffset.Now;
                                                model.LANGUAGE.Add(mylanguage);
                                                model.SaveChanges();
                                                iLanguageID = mylanguage.LanguageID;
                                            }
                                            else
                                            {
                                                //Update LANGUAGE
                                            }

                                            //TODO? LANGUAGEMAPPING

                                            int iTechnicalContextLanguageID = 0;
                                            try
                                            {
                                                iTechnicalContextLanguageID = model.LANGUAGEFORTECHNICALCONTEXT.Where(o => o.LanguageID == iLanguageID && o.TechnicalContextID == technicalcontextid).Select(o=>o.TechnicalContextLanguageID).FirstOrDefault();
                                            }
                                            catch(Exception ex)
                                            {

                                            }
                                            //XORCISMModel.LANGUAGEFORTECHNICALCONTEXT myplatformfortechcontext;
                                            //myplatformfortechcontext = model.LANGUAGEFORTECHNICALCONTEXT.FirstOrDefault(o => o.LanguageID == iLanguageID && o.TechnicalContextID == technicalcontextid);
                                            //if (myplatformfortechcontext == null)
                                            if (iTechnicalContextLanguageID<=0)
                                            {
                                                Console.WriteLine(string.Format("DEBUG Adding new LANGUAGEFORTECHNICALCONTEXT [{0}] in table LANGUAGEFORTECHNICALCONTEXT", mycapecid));
                                                LANGUAGEFORTECHNICALCONTEXT myplatformfortechcontext = new LANGUAGEFORTECHNICALCONTEXT();
                                                myplatformfortechcontext.LanguageID = iLanguageID;
                                                myplatformfortechcontext.TechnicalContextID = technicalcontextid;
                                                myplatformfortechcontext.CreatedDate = DateTimeOffset.Now;
                                                myplatformfortechcontext.timestamp = DateTimeOffset.Now;
                                                myplatformfortechcontext.VocabularyID = iVocabularyCAPECID;
                                                model.LANGUAGEFORTECHNICALCONTEXT.Add(myplatformfortechcontext);
                                                model.SaveChanges();
                                                //iTechnicalContextLanguageID=
                                            }
                                            else
                                            {
                                                //Update LANGUAGEFORTECHNICALCONTEXT
                                            }
                                        }
                                        break;
                                        #endregion capeclanguages
                                    default:
                                        Console.WriteLine("ERROR: Missing code for Technical_Context " + nodeTechCon.Name);
                                        break;
                                }
                            }
                            #endregion CAPECTechnical_Context
                            break;
                        case "capec:Related_Weaknesses":
                            #region capecrelatedweakness
                            foreach (XmlNode nodeWeakness in nodeAP.ChildNodes)  //capec:Related_Weakness
                            {
                                string mycweid = "";
                                string myrelationship = "";
                                //Console.WriteLine(nodeWeakness.Name);   //capec:Related_Weakness
                                foreach (XmlNode node3 in nodeWeakness.ChildNodes)
                                {
                                    //Console.WriteLine(node3.Name);
                                    //capec:CWE_ID
                                    if (node3.Name == "capec:CWE_ID")
                                    {
                                        mycweid = node3.InnerText;  //285
                                        Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                        Console.WriteLine("DEBUG mycweid=" + mycweid);
                                    }
                                    //capec:Weakness_Relationship_Type
                                    if (node3.Name == "capec:Weakness_Relationship_Type")
                                    {
                                        myrelationship = node3.InnerText;   //Targeted
                                        //Console.WriteLine(mycweid);
                                        //Console.WriteLine(myrelationship);

                                        //Check if the CWE exists
                                        string sCWEID = "";
                                        try
                                        {
                                            sCWEID=model.CWE.Where(o=>o.CWEID=="CWE-"+mycweid).Select(o=>o.CWEID).FirstOrDefault();
                                        }
                                        catch(Exception ex)
                                        {
                                            sCWEID = "";
                                        }
                                        if(sCWEID!="" && sCWEID!=null)
                                        {
                                            //Update CWE
                                        }
                                        else
                                        {
                                            sCWEID="CWE-" + mycweid;
                                            CWE oCWE = new CWE();
                                            oCWE.CWEID = sCWEID;
                                            oCWE.CreatedDate = DateTimeOffset.Now;
                                            oCWE.timestamp = DateTimeOffset.Now;
                                            oCWE.VocabularyID = iVocabularyCAPECID;
                                            model.CWE.Add(oCWE);
                                            model.SaveChanges();
                                            Console.WriteLine("DEBUG Added " + sCWEID);
                                        }
                                        Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                        Console.WriteLine("DEBUG CWE for Relationship=" + sCWEID);

                                        int iCAPECCWEID = 0;
                                        try
                                        {
                                            iCAPECCWEID = attack_model.ATTACKPATTERNCWE.Where(o => o.AttackPatternID == myattackpatternid && o.CWEID == sCWEID).Select(o => o.AttackPatternCWEID).FirstOrDefault();
                                        }
                                        catch(Exception ex)
                                        {

                                        }
                                        //CWEFORCAPEC mycweforcapec=null;
                                        //Optimization because no update
                                        //mycweforcapec = model.CWEFORCAPEC.FirstOrDefault(o => o.capec_id == sCAPECID && o.CWEID == "CWE-" + mycweid);
                                        //if (mycweforcapec == null)
                                        if (iCAPECCWEID <= 0)
                                        {
                                            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                            Console.WriteLine(string.Format("DEBUG Adding new ATTACKPATTERNCWE [{0}] in table ATTACKPATTERNCWE", mycapecid));
                                            //CWEFORCAPEC Replaced by ATTACKPATTERNCWE
                                            ATTACKPATTERNCWE mycweforcapec = new ATTACKPATTERNCWE();
                                            mycweforcapec.CreatedDate = DateTimeOffset.Now;
                                            //mycweforcapec.capec_id = sCAPECID;  // Removed
                                            mycweforcapec.AttackPatternID = myattackpatternid;
                                            mycweforcapec.CWEID = sCWEID;   // "CWE-" + mycweid;
                                            mycweforcapec.WeaknessRelationship = myrelationship;
                                            
                                            mycweforcapec.VocabularyID = iVocabularyCAPECID;
                                            mycweforcapec.timestamp = DateTimeOffset.Now;
                                            attack_model.ATTACKPATTERNCWE.Add(mycweforcapec);

                                            attack_model.SaveChanges();
                                            //iCAPECCWEID=
                                        }
                                        else
                                        {
                                            //Update CWEFORCAPEC
                                            //mycweforcapec.WeaknessRelationship = myrelationship;  //TODO?
                                        }
                                    }
                                }
                            }
                            #endregion capecrelatedweakness
                            break;
                        case "capec:Payload":
                            #region CAPECPayload
                            foreach (XmlNode nodePayloadAtt in nodeAP.ChildNodes)
                            {
                                //TODO: test if nodePayloadAtt.Name=="capec:Text"
                                string sPayloadText = CleaningCAPECString(nodePayloadAtt.InnerText);
                                
                                //ATTACKPAYLOAD mypayload=null;
                                //Optimization because no update
                                //mypayload = attack_model.ATTACKPAYLOAD.FirstOrDefault(o => o.PayloadText == sPayloadText);   //&& o.VocabularyID == 4
                                int iAttackPayloadID = attack_model.ATTACKPAYLOAD.Where(o => o.PayloadText == sPayloadText).Select(o=>o.AttackPayloadID).FirstOrDefault();
                                //if (mypayload == null)
                                if (iAttackPayloadID == 0)
                                {
                                    try
                                    {
                                        Console.WriteLine(string.Format("DEBUG Adding new ATTACKPAYLOAD [{0}] in table ATTACKPAYLOAD", mycapecid));
                                        ATTACKPAYLOAD mypayload = new ATTACKPAYLOAD();
                                        mypayload.PayloadText = sPayloadText;
                                        mypayload.CreatedDate = DateTimeOffset.Now;
                                        mypayload.timestamp = DateTimeOffset.Now;
                                        mypayload.VocabularyID = iVocabularyCAPECID;
                                        attack_model.ATTACKPAYLOAD.Add(mypayload);
                                        attack_model.SaveChanges();
                                        iAttackPayloadID = mypayload.AttackPayloadID;
                                    }
                                    catch (Exception exATTACKPAYLOAD)
                                    {
                                        Console.WriteLine("Exception exATTACKPAYLOAD " + exATTACKPAYLOAD.Message + " " + exATTACKPAYLOAD.InnerException);
                                    }
                                }
                                else
                                {
                                    //Update ATTACKPAYLOAD
                                }

                                ////Optimization because no update
                                mypayloadforpattern = attack_model.ATTACKPAYLOADFORATTACKPATTERN.FirstOrDefault(o => o.AttackPayloadID == iAttackPayloadID && o.AttackPatternID == myattackpatternid);   //&& o.capec_id == sCAPECID
                                ////int iAttackPatternPayloadID = attack_model.ATTACKPAYLOADFORATTACKPATTERN.Where(o => o.AttackPayloadID == iAttackPayloadID && o.AttackPatternID == myattackpatternid).Select(o=>o.AttackPatternPayloadID).FirstOrDefault();
                                if (mypayloadforpattern == null)
                                ////if (iAttackPatternPayloadID == 0)
                                {
                                    try
                                    {
                                        Console.WriteLine(string.Format("DEBUG Adding new ATTACKPAYLOADFORATTACKPATTERN [{0}] in table ATTACKPAYLOADFORATTACKPATTERN", mycapecid));
                                        mypayloadforpattern = new ATTACKPAYLOADFORATTACKPATTERN();
                                        mypayloadforpattern.CreatedDate = DateTimeOffset.Now;
                                        mypayloadforpattern.AttackPayloadID = iAttackPayloadID; // mypayload.AttackPayloadID;
                                        mypayloadforpattern.AttackPatternID = myattackpatternid;
                                        //mypayloadforpattern.capec_id = sCAPECID;    //Removed
                                        mypayloadforpattern.timestamp = DateTimeOffset.Now;
                                        mypayloadforpattern.VocabularyID = iVocabularyCAPECID;
                                        attack_model.ATTACKPAYLOADFORATTACKPATTERN.Add(mypayloadforpattern);
                                        attack_model.SaveChanges();
                                    }
                                    catch(Exception exATTACKPAYLOADFORATTACKPATTERN)
                                    {
                                        Console.WriteLine("Exception exATTACKPAYLOADFORATTACKPATTERN " + exATTACKPAYLOADFORATTACKPATTERN.Message + " " + exATTACKPAYLOADFORATTACKPATTERN.InnerException);
                                    }
                                }
                                else
                                {
                                    //Update ATTACKPAYLOADFORATTACKPATTERN
                                }
                            }
                            #endregion CAPECPayload
                            break;

                        //TODO
                        case "capec:Payload_Activation_Impact":
                            #region capecpayloadactivationimpact
                            foreach (XmlNode nodePayloadActivationImpactInfo in nodeAP.ChildNodes)
                            {
                                if (nodePayloadActivationImpactInfo.Name != "capec:Description")
                                {
                                    Console.WriteLine("ERROR: Missing code for nodePayloadActivationImpactInfo " + nodePayloadActivationImpactInfo.Name);
                                }
                                else
                                {
                                    string sPayloadActivationImpact = "";
                                    try
                                    {
                                        sPayloadActivationImpact=CleaningCAPECString(nodePayloadActivationImpactInfo.InnerText);
                                        //////TODO PAYLOADACTIVATIONIMPACT
                                    }
                                    catch (Exception exPayload_Activation_Impact)
                                    {
                                        Console.WriteLine("Exception exPayload_Activation_Impact01 " + exPayload_Activation_Impact.Message + " " + exPayload_Activation_Impact.InnerException);
                                        sPayloadActivationImpact = "";
                                    }

                                    try
                                    {
                                        //TODO
                                        //ATTACKPAYLOADIMPACT oAttackPayloadImpact = null;
                                        int iAttackPayloadImpactID = 0;
                                        try
                                        {
                                            iAttackPayloadImpactID=attack_model.ATTACKPAYLOADIMPACT.Where(o => o.PayloadActivationImpactDescription == sPayloadActivationImpact).Select(o => o.AttackPayloadImpactID).FirstOrDefault();
                                        }
                                        catch(Exception ex)
                                        {

                                        }
                                        if (iAttackPayloadImpactID <= 0)
                                        {
                                            ATTACKPAYLOADIMPACT oAttackPayloadImpact = new ATTACKPAYLOADIMPACT();
                                            oAttackPayloadImpact.CreatedDate = DateTimeOffset.Now;
                                            oAttackPayloadImpact.PayloadActivationImpactDescription = sPayloadActivationImpact;
                                            oAttackPayloadImpact.timestamp = DateTimeOffset.Now;
                                            oAttackPayloadImpact.VocabularyID = iVocabularyCAPECID;
                                            attack_model.ATTACKPAYLOADIMPACT.Add(oAttackPayloadImpact);
                                            attack_model.SaveChanges();
                                            iAttackPayloadImpactID = oAttackPayloadImpact.AttackPayloadImpactID;
                                        }
                                        else
                                        {
                                            //Update ATTACKPAYLOADIMPACT
                                        }

                                        if (mypayloadforpattern != null)
                                        {
                                            //Update ATTACKPAYLOADFORATTACKPATTERN
                                            //mypayloadforpattern.Payload_Activation_Impact = sPayloadActivationImpact;   // Removed
                                            mypayloadforpattern.AttackPayloadImpactID = iAttackPayloadImpactID;
                                            mypayloadforpattern.timestamp = DateTimeOffset.Now;
                                            attack_model.SaveChanges();
                                        }
                                        else
                                        {
                                            //TODO
                                            //oAttackPattern.Payload_Activation_Impact = sPayloadActivationImpact;
                                            //oAttackPattern.timestamp = DateTimeOffset.Now;
                                            //attack_model.SaveChanges();
                                        }
                                    }
                                    catch (Exception exPayload_Activation_Impact)
                                    {
                                        Console.WriteLine("Exception exPayload_Activation_Impact02 " + sCAPECID+" "+ exPayload_Activation_Impact.Message + " " + exPayload_Activation_Impact.InnerException);
                                    }

                                    //TODO?
                                    //ATTACKPAYLOADIMPACT
                                    //ATTACKPAYLOADIMPACTFORATTACKPATTERN

                                }
                            }

                            break;
                            #endregion capecpayloadactivationimpact
                        //TODO
                        case "capec:Obfuscation_Techniques":
                            #region obfuscationtechnique
                            foreach (XmlNode nodeObfuscationTechnique in nodeAP.ChildNodes)
                            {
                                foreach (XmlNode nodeObfuscationTechniqueInfo in nodeObfuscationTechnique.ChildNodes)
                                {
                                    if (nodeObfuscationTechniqueInfo.Name == "capec:Description")
                                    {
                                        string sObfuscationtechniqueDescription = CleaningCAPECString(nodeObfuscationTechniqueInfo.InnerText);
                                        //Optimization because no update
                                        //OBFUSCATIONTECHNIQUE oObfuscationTechnique = model.OBFUSCATIONTECHNIQUE.FirstOrDefault(o => o.ObfuscationTechniqueDescription == sObfuscationtechniqueDescription);
                                        int iObfuscationTechniqueID = 0;
                                        try
                                        {
                                            iObfuscationTechniqueID=model.OBFUSCATIONTECHNIQUE.Where(o => o.ObfuscationTechniqueDescription == sObfuscationtechniqueDescription).Select(o => o.ObfuscationTechniqueID).FirstOrDefault();
                                        }
                                        catch(Exception ex)
                                        {

                                        }
                                        //if (oObfuscationTechnique == null)
                                        if (iObfuscationTechniqueID <= 0)
                                        {
                                            OBFUSCATIONTECHNIQUE oObfuscationTechnique = new OBFUSCATIONTECHNIQUE();
                                            oObfuscationTechnique.ObfuscationTechniqueName = "";    //Note: Maybe TODO Review
                                            oObfuscationTechnique.ObfuscationTechniqueDescription = sObfuscationtechniqueDescription;
                                            oObfuscationTechnique.VocabularyID = iVocabularyCAPECID;
                                            oObfuscationTechnique.CreatedDate = DateTimeOffset.Now;
                                            oObfuscationTechnique.timestamp = DateTimeOffset.Now;
                                            model.OBFUSCATIONTECHNIQUE.Add(oObfuscationTechnique);
                                            model.SaveChanges();
                                            iObfuscationTechniqueID = oObfuscationTechnique.ObfuscationTechniqueID;
                                        }
                                        else
                                        {
                                            //Update OBFUSCATIONTECHNIQUE
                                        }

                                        //Optimization because no update
                                        //ATTACKPATTERNOBFUSCATIONTECHNIQUE oAttackPatternObfuscationTechnique = attack_model.ATTACKPATTERNOBFUSCATIONTECHNIQUE.FirstOrDefault(o => o.AttackPatternID == myattackpatternid && o.ObfuscationTechniqueID == iObfuscationTechniqueID);
                                        int iAttackPatternObfuscationTechniqueID = 0;
                                        try
                                        {
                                            iAttackPatternObfuscationTechniqueID=attack_model.ATTACKPATTERNOBFUSCATIONTECHNIQUE.Where(o => o.AttackPatternID == myattackpatternid && o.ObfuscationTechniqueID == iObfuscationTechniqueID).Select(o => o.AttackPatternObfuscationTechniqueID).FirstOrDefault();
                                        }
                                        catch(Exception ex)
                                        {

                                        }
                                        //if (oAttackPatternObfuscationTechnique == null)
                                        if (iAttackPatternObfuscationTechniqueID <= 0)
                                        {
                                            ATTACKPATTERNOBFUSCATIONTECHNIQUE oAttackPatternObfuscationTechnique = new ATTACKPATTERNOBFUSCATIONTECHNIQUE();
                                            oAttackPatternObfuscationTechnique.ObfuscationTechniqueID = iObfuscationTechniqueID;    // oObfuscationTechnique.ObfuscationTechniqueID;
                                            oAttackPatternObfuscationTechnique.AttackPatternID=myattackpatternid;
                                            oAttackPatternObfuscationTechnique.CreatedDate=DateTimeOffset.Now;
                                            oAttackPatternObfuscationTechnique.timestamp=DateTimeOffset.Now;
                                            oAttackPatternObfuscationTechnique.VocabularyID=iVocabularyCAPECID;
                                            attack_model.ATTACKPATTERNOBFUSCATIONTECHNIQUE.Add(oAttackPatternObfuscationTechnique);
                                            attack_model.SaveChanges();
                                            //iAttackPatternObfuscationTechniqueID = oAttackPatternObfuscationTechnique.AttackPatternObfuscationTechniqueID;
                                        }
                                        else
                                        {
                                            //Update ATTACKPATTERNOBFUSCATIONTECHNIQUE
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("ERROR: TODO Missing code for nodeObfuscationTechniqueInfo " + nodeObfuscationTechniqueInfo.Name);
                                    }
                                }
                            }
                            break;
                            #endregion obfuscationtechnique

                        case "capec:Typical_Likelihood_of_Exploit":
                            #region CAPECExploitLikelihood
                            EXPLOITLIKELIHOODFORATTACKPATTERN oCAPECLikelihood = null;
                            //Note: Replace CAPECEXPLOITLIKELIHOOD
                            foreach (XmlNode nodeLikelihoodExploit in nodeAP.ChildNodes)
                            {
                                switch (nodeLikelihoodExploit.Name)
                                {
                                    case "capec:Likelihood":

                                        //Console.WriteLine("DEBUG TODO: code for AttackPattern " + nodeAP.Name);
                                        string sExploitLikelihood = CleaningCAPECString(nodeLikelihoodExploit.InnerText.Trim());
                                        //Optimization because no update
                                        //EXPLOITLIKELIHOOD oExploitLikelihood = model.EXPLOITLIKELIHOOD.FirstOrDefault(o => o.Likelihood == sExploitLikelihood);  //TODO: && o.VocabularyID==
                                        int iExploitLikelihoodID = 0;
                                        try
                                        {
                                            iExploitLikelihoodID = model.EXPLOITLIKELIHOOD.Where(o => o.Likelihood == sExploitLikelihood).Select(o => o.ExploitLikelihoodID).FirstOrDefault();  //TODO: && o.VocabularyID==
                                        }
                                        catch (Exception ex)
                                        {

                                        }
                                        //if (oExploitLikelihood != null)
                                        if (iExploitLikelihoodID > 0)
                                        {
                                            //Update EXPLOITLIKELIHOOD
                                        }
                                        else
                                        {

                                            try
                                            {
                                                EXPLOITLIKELIHOOD oExploitLikelihood = new EXPLOITLIKELIHOOD();
                                                oExploitLikelihood.Likelihood = sExploitLikelihood;
                                                oExploitLikelihood.CreatedDate = DateTimeOffset.Now;
                                                oExploitLikelihood.timestamp = DateTimeOffset.Now;
                                                oExploitLikelihood.VocabularyID = iVocabularyCAPECID;
                                                model.EXPLOITLIKELIHOOD.Add(oExploitLikelihood);
                                                model.SaveChanges();
                                                Console.WriteLine("DEBUG AddToEXPLOITLIKELIHOOD " + sExploitLikelihood);
                                                iExploitLikelihoodID = oExploitLikelihood.ExploitLikelihoodID;
                                            }
                                            catch (Exception exAddToEXPLOITLIKELIHOOD)
                                            {
                                                Console.WriteLine("Exception exAddToEXPLOITLIKELIHOOD " + exAddToEXPLOITLIKELIHOOD.Message + " " + exAddToEXPLOITLIKELIHOOD.InnerException);
                                            }
                                        }

                                        ////Optimization because no update
                                        oCAPECLikelihood = model.EXPLOITLIKELIHOODFORATTACKPATTERN.FirstOrDefault(o => o.ExploitLikelihoodID == iExploitLikelihoodID && o.AttackPatternID == myattackpatternid);
                                        //int iAttackPatternExploitLikelihoodID = model.EXPLOITLIKELIHOODFORATTACKPATTERN.Where(o => o.ExploitLikelihoodID == iExploitLikelihoodID && o.capec_id == sCAPECID && o.AttackPatternID == myattackpatternid).Select(o=>o.AttackPatternExploitLikelihoodID).FirstOrDefault();
                                        if (oCAPECLikelihood != null)
                                        //if (iAttackPatternExploitLikelihoodID != 0)
                                        {
                                            //Update EXPLOITLIKELIHOODFORATTACKPATTERN
                                        }
                                        else
                                        {
                                            try
                                            {
                                                oCAPECLikelihood = new EXPLOITLIKELIHOODFORATTACKPATTERN();
                                                //oCAPECLikelihood.capec_id = sCAPECID;   //Removed
                                                oCAPECLikelihood.ExploitLikelihoodID = iExploitLikelihoodID;    // oExploitLikelihood.ExploitLikelihoodID;
                                                oCAPECLikelihood.AttackPatternID = myattackpatternid;
                                                oCAPECLikelihood.CreatedDate = DateTimeOffset.Now;
                                                oCAPECLikelihood.VocabularyID = iVocabularyCAPECID;
                                                oCAPECLikelihood.timestamp = DateTimeOffset.Now;
                                                model.EXPLOITLIKELIHOODFORATTACKPATTERN.Add(oCAPECLikelihood);
                                                model.SaveChanges();    //TEST PERFORMANCE
                                            }
                                            catch (Exception exAddToEXPLOITLIKELIHOODFORATTACKPATTERN)
                                            {
                                                Console.WriteLine("Exception exAddToEXPLOITLIKELIHOODFORATTACKPATTERN " + exAddToEXPLOITLIKELIHOODFORATTACKPATTERN.Message + " " + exAddToEXPLOITLIKELIHOODFORATTACKPATTERN.InnerException);
                                            }
                                        }
                                        break;

                                    case "capec:Explanation":
                                        //Console.WriteLine("DEBUG TODO: code for AttackPattern Typical_Likelihood_of_Exploit " + nodeLikelihoodExploit.Name);
                                        //TODO: not clean
                                        //We assume that we will find only one <capec:Text>DEADBEEF</capec:Text>
                                        string sLikelihoodExplanation = CleaningCAPECString(nodeLikelihoodExploit.InnerText);

                                        try
                                        {
                                            //Update EXPLOITLIKELIHOODFORATTACKPATTERN
                                            oCAPECLikelihood.Explanation = sLikelihoodExplanation;
                                            oCAPECLikelihood.timestamp = DateTimeOffset.Now;
                                            //oCAPECLikelihood.VocabularyID = iVocabularyCAPECID;
                                            model.SaveChanges();
                                        }
                                        catch (Exception exLikelihoodExplanation)
                                        {
                                            Console.WriteLine("Exception exLikelihoodExplanation " + exLikelihoodExplanation.Message + " " + exLikelihoodExplanation.InnerException);
                                        }
                                        break;

                                    default:
                                        Console.WriteLine("ERROR: Missing code for nodeLikelihoodExploit.Name=" + nodeLikelihoodExploit.Name);
                                        break;
                                }
                            }
                            #endregion CAPECExploitLikelihood
                            break;

                        case "capec:Solutions_and_Mitigations":
                            #region CAPECMitigations
                            foreach (XmlNode nodeSolutionMitigation in nodeAP.ChildNodes)
                            {
                                string sSolutionMitigationText = CleaningCAPECString(nodeSolutionMitigation.InnerText);
                                //Optimization because no update
                                //MITIGATION oMitigation = model.MITIGATION.FirstOrDefault(o => o.SolutionMitigationText == sSolutionMitigationText);
                                int iMitigationID = 0;
                                try
                                {
                                    iMitigationID=model.MITIGATION.Where(o => o.SolutionMitigationText == sSolutionMitigationText).Select(o => o.MitigationID).FirstOrDefault();
                                }
                                catch(Exception ex)
                                {

                                }
                                //TODO: Link with REQUIREMENT, CCE/OVAL, SECURITYCONTROL...?
                                //if (oMitigation != null)
                                if (iMitigationID > 0)
                                {
                                    //Update MITIGATION
                                }
                                else
                                {
                                    try
                                    {
                                        MITIGATION oMitigation = new MITIGATION();
                                        oMitigation.SolutionMitigationText = sSolutionMitigationText;
                                        oMitigation.VocabularyID = iVocabularyCAPECID;
                                        oMitigation.CreatedDate = DateTimeOffset.Now;
                                        oMitigation.timestamp = DateTimeOffset.Now;
                                        model.MITIGATION.Add(oMitigation);
                                        model.SaveChanges();
                                        Console.WriteLine("DEBUG AddToMITIGATION " + sSolutionMitigationText);
                                        iMitigationID = oMitigation.MitigationID;
                                    }
                                    catch (Exception exAddToMITIGATION)
                                    {
                                        Console.WriteLine("Exception exAddToMITIGATION " + exAddToMITIGATION.Message + " " + exAddToMITIGATION.InnerException);
                                    }
                                }

                                //Optimization because no update
                                //MITIGATIONFORATTACKPATTERN oCAPECMitigation = model.MITIGATIONFORATTACKPATTERN.FirstOrDefault(o => o.MitigationID == iMitigationID && o.AttackPatternID == myattackpatternid && o.capec_id == sCAPECID);  //TODO: && VocabularyID==
                                int iAttackPatternMitigationID = 0;
                                try
                                {
                                    iAttackPatternMitigationID=model.MITIGATIONFORATTACKPATTERN.Where(o => o.MitigationID == iMitigationID && o.AttackPatternID == myattackpatternid).Select(o => o.AttackPatternMitigationID).FirstOrDefault();  //TODO: && VocabularyID==
                                }
                                catch(Exception ex)
                                {

                                }
                                //if (oCAPECMitigation != null)
                                if (iAttackPatternMitigationID > 0)
                                {
                                    //Update MITIGATIONFORATTACKPATTERN
                                }
                                else
                                {
                                    
                                    try
                                    {
                                        MITIGATIONFORATTACKPATTERN oCAPECMitigation = new MITIGATIONFORATTACKPATTERN();
                                        oCAPECMitigation.CreatedDate = DateTimeOffset.Now;
                                        //oCAPECMitigation.capec_id = sCAPECID;   //Removed
                                        oCAPECMitigation.AttackPatternID = myattackpatternid;
                                        oCAPECMitigation.MitigationID = iMitigationID;  // oMitigation.MitigationID;
                                        oCAPECMitigation.VocabularyID = iVocabularyCAPECID;
                                        oCAPECMitigation.timestamp = DateTimeOffset.Now;
                                        model.MITIGATIONFORATTACKPATTERN.Add(oCAPECMitigation);
                                        //model.SaveChanges();    //TEST PERFORMANCE
                                        //iAttackPatternMitigationID=
                                    }
                                    catch (Exception exAddToMITIGATIONFORATTACKPATTERN)
                                    {
                                        Console.WriteLine("Exception exAddToMITIGATIONFORATTACKPATTERN " + exAddToMITIGATIONFORATTACKPATTERN.Message + " " + exAddToMITIGATIONFORATTACKPATTERN.InnerException);
                                    }
                                }
                            }
                            break;
                            #endregion CAPECMitigations

                        //TODO
                        case "capec:Methods_of_Attack":
                            #region CAPECAttackMethod
                            foreach (XmlNode nodeAttackMethod in nodeAP.ChildNodes)
                            {
                                switch (nodeAttackMethod.Name)
                                {
                                    case "capec:Method_of_Attack":
                                        string sAttackMethodTitle=CleaningCAPECString(nodeAttackMethod.InnerText);
                                        Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                        Console.WriteLine("DEBUG sAttackMethodTitle=" + sAttackMethodTitle);
                                        //Optimization because no update
                                        //ATTACKMETHOD oAttackMethod = attack_model.ATTACKMETHOD.FirstOrDefault(o => o.AttackMethodTitle == sAttackMethodTitle);
                                        int iAttackMethodID=0;
                                        try
                                        {
                                            iAttackMethodID = attack_model.ATTACKMETHOD.Where(o => o.AttackMethodTitle == sAttackMethodTitle).Select(o => o.AttackMethodID).FirstOrDefault();
                                        }
                                        catch(Exception ex)
                                        {

                                        }
                                        //if (oAttackMethod == null)
                                        if (iAttackMethodID <= 0)
                                        {
                                            try
                                            {
                                                ATTACKMETHOD oAttackMethod = new ATTACKMETHOD();
                                                oAttackMethod.AttackMethodTitle = sAttackMethodTitle;
                                                oAttackMethod.CreatedDate = DateTimeOffset.Now;
                                                oAttackMethod.timestamp = DateTimeOffset.Now;
                                                oAttackMethod.VocabularyID = iVocabularyCAPECID;
                                                attack_model.ATTACKMETHOD.Add(oAttackMethod);
                                                attack_model.SaveChanges();
                                                iAttackMethodID = oAttackMethod.AttackMethodID;
                                                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                Console.WriteLine("DEBUG Added ATTACKMETHOD");
                                            }
                                            catch (Exception exoAttackMethod)
                                            {
                                                Console.WriteLine("Exception exoAttackMethod " + exoAttackMethod.Message + " " + exoAttackMethod.InnerException);
                                            }
                                        }
                                        else
                                        {
                                            //Update ATTACKMETHOD
                                        }

                                        //Optimization because no update
                                        //ATTACKMETHODFORATTACKPATTERN oCAPECAttackMethod = attack_model.ATTACKMETHODFORATTACKPATTERN.FirstOrDefault(o => o.AttackPatternID == myattackpat.AttackPatternID);
                                        int iAttackPatternMethodID =0;
                                        try
                                        {
                                            iAttackPatternMethodID = attack_model.ATTACKMETHODFORATTACKPATTERN.Where(o => o.AttackPatternID == myattackpat.AttackPatternID).Select(o => o.AttackPatternMethodID).FirstOrDefault();
                                        }
                                        catch(Exception ex)
                                        {

                                        }
                                        //if (oCAPECAttackMethod == null)
                                        if (iAttackPatternMethodID <= 0)
                                        {
                                            try
                                            {
                                                ATTACKMETHODFORATTACKPATTERN oCAPECAttackMethod = new ATTACKMETHODFORATTACKPATTERN();
                                                oCAPECAttackMethod.AttackPatternID = myattackpat.AttackPatternID;
                                                //oCAPECAttackMethod.capec_id = sCAPECID; //Removed
                                                oCAPECAttackMethod.AttackMethodID = iAttackMethodID;    // oAttackMethod.AttackMethodID;
                                                oCAPECAttackMethod.CreatedDate = DateTimeOffset.Now;
                                                oCAPECAttackMethod.timestamp = DateTimeOffset.Now;
                                                oCAPECAttackMethod.VocabularyID = iVocabularyCAPECID;
                                                attack_model.ATTACKMETHODFORATTACKPATTERN.Add(oCAPECAttackMethod);
                                                //attack_model.SaveChanges();    //TEST PERFORMANCE
                                                //iAttackPatternMethodID=
                                                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                Console.WriteLine("DEBUG Added ATTACKMETHODFORATTACKPATTERN");
                                            }
                                            catch (Exception exoCAPECAttackMethod)
                                            {
                                                Console.WriteLine("exception exoCAPECAttackMethod " + exoCAPECAttackMethod.Message + " " + exoCAPECAttackMethod.InnerException);
                                            }
                                        }
                                        else
                                        {
                                            //Update ATTACKMETHODFORATTACKPATTERN
                                        }

                                        break;

                                    default:
                                        Console.WriteLine("ERROR: TODO Missing code for nodeAttackMethod " + nodeAttackMethod.Name);
                                        break;
                                }
                            }
                            break;
                            #endregion CAPECAttackMethod

                        case "capec:Examples-Instances":
                            //ATTACKEXAMPLE
                            //TODO
                            //Note: we build our own ID
                            int iExampleInstanceID = 0;
                            #region attackexampleinstances
                            
                            foreach (XmlNode nodeAttackExampleInstance in nodeAP.ChildNodes)
                            {
                                //Console.WriteLine("DEBUG nodeAttackExampleInstance " + nodeAttackExampleInstance.Name);
                                switch (nodeAttackExampleInstance.Name)
                                {
                                    case "capec:Example-Instance":
                                        iExampleInstanceID++;
                                        string sCAPECAttackExampleID = sCAPECID + "-"+iExampleInstanceID;   //TODO REVIEW
                                        string sCAPECExampleInstanceDescription = "";
                                        //ATTACKEXAMPLE oAttackExample = null;
                                        int iAttackExampleID = 0;

                                        foreach (XmlNode nodeAttackExampleInstanceInfo in nodeAttackExampleInstance.ChildNodes)
                                        {
                                            //Console.WriteLine("DEBUG nodeAttackExampleInstanceInfo " + nodeAttackExampleInstanceInfo.Name);
                                            switch (nodeAttackExampleInstanceInfo.Name)
                                            {
                                                case "capec:Example-Instance_Description":
                                                    sCAPECExampleInstanceDescription = CleaningCAPECString(nodeAttackExampleInstanceInfo.InnerText);
                                                    //TODO

                                                    //Note: Reference can be included

                                                    //oAttackExample = attack_model.ATTACKEXAMPLE.FirstOrDefault(o => o.AttackExampleDescription == sCAPECExampleInstanceDescription);   //TODO: VocabularyID?
                                                    iAttackExampleID=0;
                                                    try
                                                    {
                                                        iAttackExampleID = attack_model.ATTACKEXAMPLE.Where(o => o.AttackExampleDescription == sCAPECExampleInstanceDescription).Select(o => o.AttackExampleID).FirstOrDefault();   //TODO: VocabularyID?
                                                    }
                                                    catch(Exception ex)
                                                    {

                                                    }
                                                    //if (oAttackExample == null)
                                                    if (iAttackExampleID <= 0)
                                                    {
                                                        try
                                                        {
                                                            ATTACKEXAMPLE oAttackExample = new ATTACKEXAMPLE();
                                                            oAttackExample.AttackExampleDescription = sCAPECExampleInstanceDescription;
                                                            oAttackExample.CreatedDate = DateTimeOffset.Now;
                                                            oAttackExample.timestamp = DateTimeOffset.Now;
                                                            oAttackExample.VocabularyID = iVocabularyCAPECID;
                                                            attack_model.ATTACKEXAMPLE.Add(oAttackExample);
                                                            attack_model.SaveChanges();
                                                            iAttackExampleID = oAttackExample.AttackExampleID;
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
                                                        //TODO: catch(exception
                                                    }
                                                    else
                                                    {
                                                        //Update ATTACKEXAMPLE
                                                    }

                                                    int iAttackPatternExampleID = 0;
                                                    try
                                                    {
                                                        iAttackPatternExampleID = attack_model.ATTACKEXAMPLEFORATTACKPATTERN.Where(o => o.AttackPatternID == myattackpatternid && o.AttackExampleID == iAttackExampleID).Select(o => o.AttackExampleForAttackPatternID).FirstOrDefault();
                                                    }
                                                    catch(Exception ex)
                                                    {

                                                    }
                                                    if (iAttackPatternExampleID<=0)
                                                    {
                                                        try
                                                        {
                                                            ATTACKEXAMPLEFORATTACKPATTERN oAttackPatternExample = new ATTACKEXAMPLEFORATTACKPATTERN();
                                                            oAttackPatternExample.CreatedDate = DateTimeOffset.Now;
                                                            oAttackPatternExample.AttackPatternID = myattackpatternid;
                                                            oAttackPatternExample.AttackExampleID = iAttackExampleID;
                                                            oAttackPatternExample.VocabularyID = iVocabularyCAPECID;
                                                            oAttackPatternExample.timestamp = DateTimeOffset.Now;
                                                            attack_model.ATTACKEXAMPLEFORATTACKPATTERN.Add(oAttackPatternExample);
                                                            //attack_model.SaveChanges();    //TEST PERFORMANCE
                                                            //iAttackPatternExampleID=
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
                                                            Console.WriteLine("Exception DbEntityValidationExceptionexATTACKEXAMPLEFORATTACKPATTERN " + sb.ToString());
                                                        }
                                                        catch(Exception exATTACKEXAMPLEFORATTACKPATTERN)
                                                        {
                                                            Console.WriteLine("Exception exATTACKEXAMPLEFORATTACKPATTERN " + exATTACKEXAMPLEFORATTACKPATTERN.Message + " " + exATTACKEXAMPLEFORATTACKPATTERN.InnerException);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //Update ATTACKEXAMPLEFORATTACKPATTERN
                                                    }
                                                    break;

                                                case "capec:Example-Instance_Related_Vulnerabilities":
                                                    //TODO
                                                    //VULNERABILITYFORATTACKEXAMPLE

                                                    //TODO
                                                    //VULNERABILITYFORATTACKPATTERN

                                                    foreach (XmlNode nodeAttackExampleInstanceVUL in nodeAttackExampleInstanceInfo.ChildNodes)
                                                    {
                                                        //Console.WriteLine("DEBUG nodeAttackExampleInstanceVUL " + nodeAttackExampleInstanceVUL.Name);
                                                        switch (nodeAttackExampleInstanceVUL.Name)
                                                        {
                                                            case "capec:Example-Instance_Related_Vulnerability":
                                                                string sRelatedVUL = nodeAttackExampleInstanceVUL.InnerText;
                                                                //<capec:Text>CVE-2006-0231</capec:Text>
                                                                sRelatedVUL = sRelatedVUL.Replace("<capec:Text>", "");
                                                                sRelatedVUL = sRelatedVUL.Replace("</capec:Text>", "");

                                                                //Regex myRegexCVE = new Regex("CVE-[0-9][0-9][0-9][0-9]-[0-9][0-9][0-9][0-9]");
                                                                Regex myRegexCVE = new Regex(@"CVE-(19|20)\d\d-(0\d{3}|[1-9]\d{3,})");
                                                                //https://cve.mitre.org/cve/identifiers/tech-guidance.html
                                                                string strTemp = myRegexCVE.Match(sRelatedVUL).ToString();
                                                                if (strTemp != "")
                                                                {
                                                                    string sCVEID = strTemp;
                                                                    //Console.WriteLine("DEBUG Example-Instance_Related_Vulnerability CVE:" + sCVEID);
                                                                    //VULNERABILITY oCVE = model.VULNERABILITY.FirstOrDefault(o => o.VULReferential == "cve" && o.VULReferentialID == sCVEID);
                                                                    int iVulnerabilityID = 0;
                                                                    try
                                                                    {
                                                                        iVulnerabilityID=vuln_model.VULNERABILITY.Where(o => o.VULReferential == "cve" && o.VULReferentialID == sCVEID).Select(o => o.VulnerabilityID).FirstOrDefault();
                                                                    }
                                                                    catch(Exception ex)
                                                                    {

                                                                    }
                                                                    //if (oCVE == null)
                                                                    if (iVulnerabilityID <= 0)
                                                                    {
                                                                        try
                                                                        {
                                                                            VULNERABILITY oCVE = new VULNERABILITY();
                                                                            oCVE.VULReferential = "cve";
                                                                            oCVE.VULReferentialID = sCVEID;

                                                                            oCVE.CreatedDate = DateTimeOffset.Now;
                                                                            oCVE.timestamp = DateTimeOffset.Now;
                                                                            oCVE.VocabularyID = iVocabularyCAPECID;
                                                                            vuln_model.VULNERABILITY.Add(oCVE);
                                                                            vuln_model.SaveChanges();
                                                                            iVulnerabilityID = oCVE.VulnerabilityID;
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
                                                                        //TODO catch(exception
                                                                    }
                                                                    else
                                                                    {
                                                                        //Update VULNERABILITY
                                                                    }

                                                                    #region VULNERABILITYFORATTACKEXAMPLE
                                                                    //VULNERABILITYFORATTACKEXAMPLE oAttackExampleVulnerability = model.VULNERABILITYFORATTACKEXAMPLE.FirstOrDefault(o => o.AttackExampleID == oAttackExample.AttackExampleID && o.VulnerabilityID == iVulnerabilityID);
                                                                    int iAttackExampleVulnerabilityID = 0;
                                                                    try
                                                                    {
                                                                        iAttackExampleVulnerabilityID=vuln_model.VULNERABILITYFORATTACKEXAMPLE.Where(o => o.AttackExampleID == iAttackExampleID && o.VulnerabilityID == iVulnerabilityID).Select(o => o.AttackExampleVulnerabilityID).FirstOrDefault();
                                                                    }
                                                                    catch(Exception ex)
                                                                    {

                                                                    }
                                                                    //if (oAttackExampleVulnerability == null)
                                                                    if (iAttackExampleVulnerabilityID <= 0)
                                                                    {
                                                                        try
                                                                        {
                                                                            VULNERABILITYFORATTACKEXAMPLE oAttackExampleVulnerability = new VULNERABILITYFORATTACKEXAMPLE();
                                                                            oAttackExampleVulnerability.AttackExampleID = iAttackExampleID; // oAttackExample.AttackExampleID;
                                                                            oAttackExampleVulnerability.VulnerabilityID = iVulnerabilityID; // oCVE.VulnerabilityID;
                                                                            oAttackExampleVulnerability.CreatedDate = DateTimeOffset.Now;
                                                                            oAttackExampleVulnerability.timestamp = DateTimeOffset.Now;
                                                                            oAttackExampleVulnerability.VocabularyID = iVocabularyCAPECID;
                                                                            vuln_model.VULNERABILITYFORATTACKEXAMPLE.Add(oAttackExampleVulnerability);
                                                                            vuln_model.SaveChanges();    //TEST PERFORMANCE
                                                                            //iAttackExampleVulnerabilityID=
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
                                                                        //TODO catch(exception
                                                                    }
                                                                    else
                                                                    {
                                                                        //Update VULNERABILITYFORATTACKEXAMPLE
                                                                    }
                                                                    #endregion VULNERABILITYFORATTACKEXAMPLE

                                                                    #region VULNERABILITYFORATTACKPATTERN
                                                                    //VULNERABILITYFORATTACKPATTERN oAttackPatternVulnerability = model.VULNERABILITYFORATTACKPATTERN.FirstOrDefault(o => o.AttackPatternID == myattackpatternid && o.VulnerabilityID==iVulnerabilityID);
                                                                    int iAttackPatternVulnerabilityID = 0;
                                                                    try
                                                                    {
                                                                        iAttackPatternVulnerabilityID = vuln_model.VULNERABILITYFORATTACKPATTERN.Where(o => o.AttackPatternID == myattackpatternid && o.VulnerabilityID == iVulnerabilityID).Select(o => o.AttackPatternVulnerabilityID).FirstOrDefault();
                                                                    }
                                                                    catch(Exception ex)
                                                                    {

                                                                    }
                                                                    //if (oAttackPatternVulnerability == null)
                                                                    if (iAttackPatternVulnerabilityID == 0)
                                                                    {
                                                                        try
                                                                        {
                                                                            VULNERABILITYFORATTACKPATTERN oAttackPatternVulnerability = new VULNERABILITYFORATTACKPATTERN();
                                                                            oAttackPatternVulnerability.AttackPatternID = myattackpatternid;
                                                                            oAttackPatternVulnerability.VulnerabilityID = iVulnerabilityID; // oCVE.VulnerabilityID;
                                                                            oAttackPatternVulnerability.CreatedDate = DateTimeOffset.Now;
                                                                            oAttackPatternVulnerability.timestamp = DateTimeOffset.Now;
                                                                            oAttackPatternVulnerability.VocabularyID = iVocabularyCAPECID;
                                                                            //TODO: CAPECID, CVEID
                                                                            vuln_model.VULNERABILITYFORATTACKPATTERN.Add(oAttackPatternVulnerability);
                                                                            vuln_model.SaveChanges();    //TEST PERFORMANCE
                                                                            //iAttackPatternVulnerabilityID=
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
                                                                        //TODO catch(exception
                                                                    }
                                                                    else
                                                                    {
                                                                        //Update VULNERABILITYFORATTACKPATTERN
                                                                    }
                                                                    #endregion VULNERABILITYFORATTACKPATTERN
                                                                }
                                                                else
                                                                {
                                                                    //Vulnerability_ID is not a CVE
                                                                    //i.e.: Microsoft Security Bulletin MS04-028
                                                                    //TODO
                                                                }
                                                                break;

                                                            default:
                                                                Console.WriteLine("ERROR: TODO Missing code for nodeAttackExampleInstanceVUL " + nodeAttackExampleInstanceVUL.Name);
                                                                break;
                                                        }
                                                    }

                                                    break;
                                                default:
                                                    Console.WriteLine("ERROR: TODO Missing code for nodeAttackExampleInstanceInfo " + nodeAttackExampleInstanceInfo.Name);
                                                    break;
                                            }
                                        }

                                        //TODO
                                        //ATTACKEXAMPLE oAttackExample = attack_model.ATTACKEXAMPLE.FirstOrDefault(o => o.AttackExampleVocabularyID == sCAPECAttackExampleID);


                                        break;
                                    default:
                                        Console.WriteLine("ERROR: TODO Missing code for nodeAttackExampleInstance " + nodeAttackExampleInstance.Name);
                                        break;
                                }
                            }
                            #endregion attackexampleinstances
                            break;

                        //TODO
                        
                        case "capec:Attacker_Skills_or_Knowledge_Required":
                            #region attackskills
                            int iAttackSkillcount = 0;
                            foreach (XmlNode nodeAttackerSkill in nodeAP.ChildNodes)
                            {
                                if (nodeAttackerSkill.Name == "capec:Attacker_Skill_or_Knowledge_Required")
                                {
                                    iAttackSkillcount++;
                                    string sAttackerSkillLevel = "";
                                    string sAttackerSkillDescription = "";
                                    foreach (XmlNode nodeAttackerSkillInfo in nodeAttackerSkill)
                                    {
                                        switch(nodeAttackerSkillInfo.Name)
                                        {
                                            case "capec:Skill_or_Knowledge_Level":
                                                sAttackerSkillLevel = nodeAttackerSkillInfo.InnerText;  //Medium
                                                break;
                                            case "capec:Skill_or_Knowledge_Type":
                                                sAttackerSkillDescription = CleaningCAPECString(nodeAttackerSkillInfo.InnerText);
                                                break;
                                            default:
                                                Console.WriteLine("ERROR: Missing code for nodeAttackerSkillInfo " + nodeAttackerSkillInfo.Name);
                                                break;
                                        }
                                    }

                                    //THREATACTORSKILLFORATTACKPATTERN oAttackPatternSkill = model.THREATACTORSKILLFORATTACKPATTERN.FirstOrDefault(o => o.AttackPatternID == myattackpatternid && o.Skill_or_Knowledge_Type == sAttackerSkillDescription);
                                    int iAttackPatternThreatActorSkillID = 0;
                                    try
                                    {
                                        iAttackPatternThreatActorSkillID=threat_model.THREATACTORSKILLFORATTACKPATTERN.Where(o => o.AttackPatternID == myattackpatternid && o.Skill_or_Knowledge_Type == sAttackerSkillDescription).Select(o => o.AttackPatternRequiredSkillID).FirstOrDefault();
                                    }
                                    catch(Exception ex)
                                    {

                                    }
                                    //if (oAttackPatternSkill == null)
                                    if (iAttackPatternThreatActorSkillID <= 0)
                                    {
                                        try
                                        {
                                            THREATACTORSKILLFORATTACKPATTERN oAttackPatternSkill = new THREATACTORSKILLFORATTACKPATTERN();
                                            oAttackPatternSkill.CreatedDate = DateTimeOffset.Now;
                                            oAttackPatternSkill.AttackPatternID = myattackpatternid;
                                            //oAttackPatternSkill.capec_id = sCAPECID;    //Removed
                                            oAttackPatternSkill.AttackPatternRequiredSkillOrder = iAttackSkillcount;
                                            oAttackPatternSkill.Skill_or_Knowledge_Level = sAttackerSkillLevel;
                                            oAttackPatternSkill.Skill_or_Knowledge_Type = sAttackerSkillDescription;
                                            oAttackPatternSkill.VocabularyID = iVocabularyCAPECID;
                                            oAttackPatternSkill.timestamp = DateTimeOffset.Now;
                                            threat_model.THREATACTORSKILLFORATTACKPATTERN.Add(oAttackPatternSkill);
                                            threat_model.SaveChanges();    //TEST PERFORMANCE
                                            //iAttackPatternThreatActorSkillID=
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
                                        //TODO: catch(Exception
                                    }
                                    else
                                    {
                                        //Update THREATACTORSKILLFORATTACKPATTERN

                                    }

                                }
                                else
                                {
                                    Console.WriteLine("ERROR: Missing code for nodeAttackerSkill " + nodeAttackerSkill.Name);
                                }
                            }
                            break;
                            #endregion attackskills

                        case "capec:Related_Guidelines":
                            #region capecrelatedguidelines
                            foreach (XmlNode nodeRelatedGuideline in nodeAP.ChildNodes)
                            {
                                string sGuideLineText = CleaningCAPECString(nodeRelatedGuideline.InnerText);
                                //Optimization because no update
                                //GUIDELINE oGuideline = model.GUIDELINE.FirstOrDefault(o => o.GuidelineText == sGuideLineText);
                                int iGuidelineID = 0;
                                try
                                {
                                    iGuidelineID = model.GUIDELINE.Where(o => o.GuidelineText == sGuideLineText).Select(o=>o.GuidelineID).FirstOrDefault();
                                }
                                catch(Exception ex)
                                {

                                }
                                //if (oGuideline == null)
                                if (iGuidelineID == 0)
                                {
                                    Console.WriteLine("DEBUG Adding GUIDELINE " + sGuideLineText);
                                    GUIDELINE oGuideline = new GUIDELINE();
                                    oGuideline.GuidelineText = sGuideLineText;
                                    oGuideline.CreatedDate = DateTimeOffset.Now;
                                    oGuideline.timestamp = DateTimeOffset.Now;
                                    oGuideline.VocabularyID = iVocabularyCAPECID;
                                    model.GUIDELINE.Add(oGuideline);
                                    model.SaveChanges();
                                    iGuidelineID = oGuideline.GuidelineID;
                                }
                                else
                                {
                                    //Update GUIDELINE
                                }

                                //Optimization because no update
                                //GUIDELINEFORATTACKPATTERN oAttackPatternGuideline = model.GUIDELINEFORATTACKPATTERN.FirstOrDefault(o => o.GuidelineID == iGuidelineID && o.AttackPatternID == myattackpatternid);
                                int iAttackPatternGuidelineID = 0;
                                try
                                {
                                    iAttackPatternGuidelineID = model.GUIDELINEFORATTACKPATTERN.Where(o => o.GuidelineID == iGuidelineID && o.AttackPatternID == myattackpatternid).Select(o=>o.AttackPatternGuidelineID).FirstOrDefault();
                                }
                                catch(Exception ex)
                                {

                                }
                                //if (oAttackPatternGuideline == null)
                                if (iAttackPatternGuidelineID <= 0)
                                {
                                    GUIDELINEFORATTACKPATTERN oAttackPatternGuideline = new GUIDELINEFORATTACKPATTERN();
                                    oAttackPatternGuideline.AttackPatternID = myattackpatternid;
                                    oAttackPatternGuideline.GuidelineID = iGuidelineID; // oGuideline.GuidelineID;
                                    oAttackPatternGuideline.CreatedDate = DateTimeOffset.Now;
                                    oAttackPatternGuideline.timestamp = DateTimeOffset.Now;
                                    oAttackPatternGuideline.VocabularyID = iVocabularyCAPECID;
                                    model.GUIDELINEFORATTACKPATTERN.Add(oAttackPatternGuideline);
                                    model.SaveChanges();
                                    //iAttackPatternGuidelineID = oAttackPatternGuideline.AttackPatternGuidelineID;
                                }
                                else
                                {
                                    //Update GUIDELINEFORATTACKPATTERN
                                }
                            }
                            break;
                            #endregion capecrelatedguidelines

                        
                        case "capec:Relevant_Security_Requirements":
                            #region capecrelatedrequirements
                            foreach (XmlNode nodeRelatedSecurityRequirement in nodeAP.ChildNodes)
                            {
                                switch (nodeRelatedSecurityRequirement.Name)
                                {
                                    case "capec:Relevant_Security_Requirement":
                                        string sSecurityRequirementDescription=CleaningCAPECString(nodeRelatedSecurityRequirement.InnerText);
                                        //Optimization because no update
                                        //SECURITYREQUIREMENT oSecurityRequirement = model.SECURITYREQUIREMENT.FirstOrDefault(o => o.SecurityRequirementDescription == sSecurityRequirementDescription);
                                        int iSecurityRequirementID =0;
                                        try
                                        {
                                            iSecurityRequirementID = model.SECURITYREQUIREMENT.Where(o => o.SecurityRequirementDescription == sSecurityRequirementDescription).Select(o=>o.SecurityRequirementID).FirstOrDefault();
                                        }
                                        catch(Exception ex)
                                        {

                                        }
                                        //if (oSecurityRequirement == null)
                                        if (iSecurityRequirementID <= 0)
                                        {
                                            Console.WriteLine("DEBUG Adding SECURITYREQUIREMENT " + sSecurityRequirementDescription);
                                            SECURITYREQUIREMENT oSecurityRequirement = new SECURITYREQUIREMENT();
                                            //oSecurityRequirement.SecurityRequirementTitle = "";
                                            oSecurityRequirement.SecurityRequirementDescription = sSecurityRequirementDescription;
                                            oSecurityRequirement.CreatedDate = DateTimeOffset.Now;
                                            oSecurityRequirement.timestamp = DateTimeOffset.Now;
                                            oSecurityRequirement.VocabularyID = iVocabularyCAPECID;
                                            model.SECURITYREQUIREMENT.Add(oSecurityRequirement);
                                            model.SaveChanges();
                                            iSecurityRequirementID = oSecurityRequirement.SecurityRequirementID;
                                        }
                                        else
                                        {
                                            //Update SECURITYREQUIREMENT
                                        }

                                        //Optimization because no update
                                        //SECURITYREQUIREMENTFORATTACKPATTERN oCAPECSecurityRequirement = model.SECURITYREQUIREMENTFORATTACKPATTERN.FirstOrDefault(o => o.AttackPatternID == myattackpatternid && o.SecurityRequirementID == iSecurityRequirementID);
                                        int iAttackPatternSecurityRequirementID=0;
                                        try
                                        {
                                            iAttackPatternSecurityRequirementID = model.SECURITYREQUIREMENTFORATTACKPATTERN.Where(o => o.AttackPatternID == myattackpatternid && o.SecurityRequirementID == iSecurityRequirementID).Select(o=>o.AttackPatternSecurityRequirementID).FirstOrDefault();
                                        }
                                        catch(Exception ex)
                                        {

                                        }
                                        //if (oCAPECSecurityRequirement == null)
                                        if (iAttackPatternSecurityRequirementID <= 0)
                                        {
                                            SECURITYREQUIREMENTFORATTACKPATTERN oCAPECSecurityRequirement = new SECURITYREQUIREMENTFORATTACKPATTERN();
                                            oCAPECSecurityRequirement.AttackPatternID = myattackpatternid;
                                            oCAPECSecurityRequirement.SecurityRequirementID = iSecurityRequirementID;   // oSecurityRequirement.SecurityRequirementID;
                                            oCAPECSecurityRequirement.CreatedDate = DateTimeOffset.Now;
                                            oCAPECSecurityRequirement.timestamp = DateTimeOffset.Now;
                                            oCAPECSecurityRequirement.VocabularyID = iVocabularyCAPECID;
                                            model.SECURITYREQUIREMENTFORATTACKPATTERN.Add(oCAPECSecurityRequirement);
                                            model.SaveChanges();
                                            //iAttackPatternSecurityRequirement = oCAPECSecurityRequirement.AttackPatternSecurityRequirementID;
                                        }
                                        else
                                        {
                                            //Update SECURITYREQUIREMENTFORATTACKPATTERN
                                        }
                                        break;

                                    default:
                                        Console.WriteLine("ERROR: TODO Missing code for nodeRelatedSecurityRequirement " + nodeRelatedSecurityRequirement.Name);
                                        break;
                                }
                            }
                            break;
                            #endregion capecrelatedrequirements
                        
                        case "capec:Related_Security_Principles":
                            #region capecrelatedprinciples
                            foreach (XmlNode nodeRelatedSecurityPrinciple in nodeAP.ChildNodes)
                            {
                                switch (nodeRelatedSecurityPrinciple.Name)
                                {
                                    case "capec:Related_Security_Principle":
                                        string sSecurityPrinciple = CleaningCAPECString(nodeRelatedSecurityPrinciple.InnerText);
                                        //Optimization because no update
                                        //SECURITYPRINCIPLE oSecurityPrinciple = model.SECURITYPRINCIPLE.FirstOrDefault(o => o.SecurityPrincipleName == sSecurityPrinciple);
                                        int iSecurityPrincipleID =0;
                                        try
                                        {
                                            iSecurityPrincipleID=model.SECURITYPRINCIPLE.Where(o => o.SecurityPrincipleName == sSecurityPrinciple).Select(o => o.SecurityPrincipleID).FirstOrDefault();
                                        }
                                        catch(Exception ex)
                                        {

                                        }
                                        //if (oSecurityPrinciple == null)
                                        if (iSecurityPrincipleID == 0)
                                        {
                                            SECURITYPRINCIPLE oSecurityPrinciple = new SECURITYPRINCIPLE();
                                            oSecurityPrinciple.SecurityPrincipleName = sSecurityPrinciple;
                                            oSecurityPrinciple.VocabularyID = iVocabularyCAPECID;
                                            oSecurityPrinciple.CreatedDate = DateTimeOffset.Now;
                                            oSecurityPrinciple.timestamp = DateTimeOffset.Now;
                                            model.SECURITYPRINCIPLE.Add(oSecurityPrinciple);
                                            model.SaveChanges();
                                            iSecurityPrincipleID = oSecurityPrinciple.SecurityPrincipleID;
                                        }
                                        else
                                        {
                                            //Update SECURITYPRINCIPLE
                                        }

                                        //Optimization because no update
                                        //SECURITYPRINCIPLEFORATTACKPATTERN oCAPECSecurityprinciple = model.SECURITYPRINCIPLEFORATTACKPATTERN.FirstOrDefault(o => o.AttackPatternID == myattackpatternid && o.SecurityPrincipleID == iSecurityPrincipleID);
                                        int iAttackPatternSecurityPrincipleID =0;
                                        try
                                        {
                                            iAttackPatternSecurityPrincipleID=model.SECURITYPRINCIPLEFORATTACKPATTERN.Where(o => o.AttackPatternID == myattackpatternid && o.SecurityPrincipleID == iSecurityPrincipleID).Select(o=>o.AttackPatternSecurityPrincipleID).FirstOrDefault();
                                        }
                                        catch(Exception ex)
                                        {

                                        }
                                        //if (oCAPECSecurityprinciple == null)
                                        if (iAttackPatternSecurityPrincipleID <= 0)
                                        {
                                            SECURITYPRINCIPLEFORATTACKPATTERN oCAPECSecurityprinciple = new SECURITYPRINCIPLEFORATTACKPATTERN();
                                            oCAPECSecurityprinciple.SecurityPrincipleID = iSecurityPrincipleID;   // oSecurityPrinciple.SecurityPrincipleID;
                                            oCAPECSecurityprinciple.AttackPatternID = myattackpatternid;
                                            //oCAPECSecurityprinciple.capec_id = sCAPECID;    //Removed
                                            oCAPECSecurityprinciple.CreatedDate = DateTimeOffset.Now;
                                            oCAPECSecurityprinciple.timestamp = DateTimeOffset.Now;
                                            oCAPECSecurityprinciple.VocabularyID = iVocabularyCAPECID;
                                            model.SECURITYPRINCIPLEFORATTACKPATTERN.Add(oCAPECSecurityprinciple);
                                            model.SaveChanges();
                                            //iAttackPatternSecurityPrincipleID = oCAPECSecurityprinciple.AttackPatternSecurityPrincipleID;
                                        }
                                        else
                                        {
                                            //Update SECURITYPRINCIPLEFORATTACKPATTERN
                                        }

                                        break;

                                    default:
                                        Console.WriteLine("ERROR: TODO Missing code for nodeRelatedSecurityPrinciple " + nodeRelatedSecurityPrinciple.Name);
                                        break;
                                }
                            }
                            break;
                            #endregion capecrelatedprinciples
                        
                        case "capec:Injection_Vector":
                            #region capecinjectionvector
                            string sInjectionVectorText = CleaningCAPECString(nodeAP.InnerText);
                            //Optimization because no update
                            //INJECTIONVECTOR oInjectionVector = model.INJECTIONVECTOR.FirstOrDefault(o => o.InjectionVectorText == sInjectionVectorText);
                            int iInjectionVectorID =0;
                            try
                            {
                                iInjectionVectorID=model.INJECTIONVECTOR.Where(o => o.InjectionVectorText == sInjectionVectorText).Select(o => o.InjectionVectorID).FirstOrDefault();
                            }
                            catch(Exception ex)
                            {

                            }
                            //if (oInjectionVector == null)
                            if (iInjectionVectorID <= 0)
                            {
                                INJECTIONVECTOR oInjectionVector = new INJECTIONVECTOR();
                                oInjectionVector.InjectionVectorText = sInjectionVectorText;
                                oInjectionVector.CreatedDate = DateTimeOffset.Now;
                                oInjectionVector.timestamp = DateTimeOffset.Now;
                                oInjectionVector.VocabularyID = iVocabularyCAPECID;
                                model.INJECTIONVECTOR.Add(oInjectionVector);
                                model.SaveChanges();
                                iInjectionVectorID = oInjectionVector.InjectionVectorID;
                            }
                            else
                            {
                                //Update INJECTIONVECTOR
                            }

                            //Optimization because no update
                            //INJECTIONVECTORFORATTACKPATTERN oAttackPatternInjectionVector = model.INJECTIONVECTORFORATTACKPATTERN.FirstOrDefault(o => o.AttackPatternID == myattackpatternid && o.InjectionVectorID == iInjectionVectorID);
                            int iAttackPatternInjectionVectorID =0;
                            try
                            {
                                iAttackPatternInjectionVectorID=model.INJECTIONVECTORFORATTACKPATTERN.Where(o => o.AttackPatternID == myattackpatternid && o.InjectionVectorID == iInjectionVectorID).Select(o => o.AttackPatternInjectionVectorID).FirstOrDefault();
                            }
                            catch(Exception ex)
                            {

                            }
                            //if (oAttackPatternInjectionVector == null)
                            if (iAttackPatternInjectionVectorID <= 0)
                            {
                                INJECTIONVECTORFORATTACKPATTERN oAttackPatternInjectionVector = new INJECTIONVECTORFORATTACKPATTERN();
                                oAttackPatternInjectionVector.AttackPatternID = myattackpatternid;
                                oAttackPatternInjectionVector.InjectionVectorID = iInjectionVectorID;   // oInjectionVector.InjectionVectorID;
                                oAttackPatternInjectionVector.CreatedDate = DateTimeOffset.Now;
                                oAttackPatternInjectionVector.timestamp = DateTimeOffset.Now;
                                oAttackPatternInjectionVector.VocabularyID = iVocabularyCAPECID;
                                model.INJECTIONVECTORFORATTACKPATTERN.Add(oAttackPatternInjectionVector);
                                model.SaveChanges();
                                //iAttackPatternInjectionVectorID = oAttackPatternInjectionVector.AttackPatternInjectionVectorID;
                            }
                            else
                            {
                                //Update INJECTIONVECTORFORATTACKPATTERN
                            }

                            break;
                            #endregion capecinjectionvector
                        
                        case "capec:Activation_Zone":
                            #region capecactivationzone
                            string sActivationZoneText = CleaningCAPECString(nodeAP.InnerText);
                            //Optimization because no update
                            //ACTIVATIONZONE oActivationZone = model.ACTIVATIONZONE.FirstOrDefault(o => o.ActivationZoneText == sActivationZoneText);
                            int iActivationZoneID =0;
                            try
                            {
                                iActivationZoneID=model.ACTIVATIONZONE.Where(o => o.ActivationZoneText == sActivationZoneText).Select(o => o.ActivationZoneID).FirstOrDefault();
                            }
                            catch (Exception ex)
                            {

                            }
                            if (iActivationZoneID <= 0)
                            //if (oActivationZone == null)
                            {
                                ACTIVATIONZONE oActivationZone = new ACTIVATIONZONE();
                                oActivationZone.ActivationZoneText = sActivationZoneText;
                                oActivationZone.CreatedDate = DateTimeOffset.Now;
                                oActivationZone.timestamp = DateTimeOffset.Now;
                                oActivationZone.VocabularyID = iVocabularyCAPECID;
                                model.ACTIVATIONZONE.Add(oActivationZone);
                                model.SaveChanges();
                                iActivationZoneID = oActivationZone.ActivationZoneID;
                            }
                            else
                            {
                                //Update ACTIVATIONZONE
                            }

                            //Optimization because no update
                            //ACTIVATIONZONEFORATTACKPATTERN oAttackPatternActivationZone = model.ACTIVATIONZONEFORATTACKPATTERN.FirstOrDefault(o => o.ActivationZoneID == iActivationZoneID && o.AttackPatternID == myattackpatternid);
                            int iAttackPatternActivationZoneID=0;
                            try
                            {
                                iAttackPatternActivationZoneID=model.ACTIVATIONZONEFORATTACKPATTERN.Where(o => o.ActivationZoneID == iActivationZoneID && o.AttackPatternID == myattackpatternid).Select(o => o.AttackPatternActivationZoneID).FirstOrDefault();
                            }
                            catch(Exception ex)
                            {
                                
                            }
                            //if (oAttackPatternActivationZone == null)
                            if (iAttackPatternActivationZoneID <= 0)
                            {
                                ACTIVATIONZONEFORATTACKPATTERN oAttackPatternActivationZone = new ACTIVATIONZONEFORATTACKPATTERN();
                                oAttackPatternActivationZone.AttackPatternID = myattackpatternid;
                                oAttackPatternActivationZone.ActivationZoneID = iActivationZoneID;  // oActivationZone.ActivationZoneID;
                                oAttackPatternActivationZone.CreatedDate = DateTimeOffset.Now;
                                oAttackPatternActivationZone.timestamp = DateTimeOffset.Now;
                                oAttackPatternActivationZone.VocabularyID = iVocabularyCAPECID;
                                model.ACTIVATIONZONEFORATTACKPATTERN.Add(oAttackPatternActivationZone);
                                model.SaveChanges();
                                //iAttackPatternActivationZoneID=
                            }
                            else
                            {
                                //Update ACTIVATIONZONEFORATTACKPATTERN
                            }
                            break;
                            #endregion capecactivationzone
                        
                        case "capec:Probing_Techniques":
                            #region capecprobingtechniques
                            foreach (XmlNode nodeProbingTechnique in nodeAP.ChildNodes)
                            {
                                if(nodeProbingTechnique.Name!="capec:Probing_Technique")
                                {
                                    Console.WriteLine("ERROR: TODO Missing code for nodeProbingTechnique " + nodeProbingTechnique.Name);
                                }
                                else
                                {
                                    foreach (XmlNode nodeProbingTechniqueInfo in nodeProbingTechnique.ChildNodes)
                                    {
                                        if(nodeProbingTechniqueInfo.Name!="capec:Description")
                                        {
                                            Console.WriteLine("ERROR: TODO Missing code for nodeProbingTechniqueInfo " + nodeProbingTechniqueInfo.Name);
                                        }
                                        else
                                        {
                                            string sProbingTechniqueDescription=CleaningCAPECString(nodeProbingTechniqueInfo.InnerText);
                                            //TODO: replace PROBINGTECHNIQUE by ATTACKTECHNIQUE?
                                            //Optimization because no update
                                            //PROBINGTECHNIQUE oProbingTechnique = model.PROBINGTECHNIQUE.FirstOrDefault(o => o.ProbingTechniqueDescription == sProbingTechniqueDescription);
                                            int iProbingTechniqueID = 0;
                                            try
                                            {
                                                iProbingTechniqueID=model.PROBINGTECHNIQUE.Where(o => o.ProbingTechniqueDescription == sProbingTechniqueDescription).Select(o => o.ProbingTechniqueID).FirstOrDefault();
                                            }
                                            catch(Exception ex)
                                            {

                                            }
                                            //if (oProbingTechnique == null)
                                            if (iProbingTechniqueID <= 0)
                                            {
                                                PROBINGTECHNIQUE oProbingTechnique = new PROBINGTECHNIQUE();
                                                oProbingTechnique.ProbingTechniqueDescription = sProbingTechniqueDescription;
                                                //TODO
                                                //oProbingTechnique.TechniqueID=
                                                oProbingTechnique.CreatedDate = DateTimeOffset.Now;
                                                oProbingTechnique.VocabularyID = iVocabularyCAPECID;
                                                oProbingTechnique.timestamp = DateTimeOffset.Now;
                                                model.PROBINGTECHNIQUE.Add(oProbingTechnique);
                                                model.SaveChanges();
                                                iProbingTechniqueID = oProbingTechnique.ProbingTechniqueID;
                                            }
                                            else
                                            {
                                                //Update PROBINGTECHNIQUE
                                            }

                                            //TODO
                                            //TODO: replace by ATTACKTECHNIQUEFORATTACKPATTERN
                                            //PROBINGTECHNIQUEFORATTACKPATTERN oAttackPatternProbingTechnique = model.PROBINGTECHNIQUEFORATTACKPATTERN.FirstOrDefault(o => o.AttackPatternID == myattackpatternid && o.ProbingTechniqueID == iProbingTechniqueID);
                                            int iAttackPatternProbingTechniqueID = 0;
                                            try
                                            {
                                                iAttackPatternProbingTechniqueID=model.PROBINGTECHNIQUEFORATTACKPATTERN.Where(o => o.AttackPatternID == myattackpatternid && o.ProbingTechniqueID == iProbingTechniqueID).Select(o => o.AttackPatternProbingTechniqueID).FirstOrDefault();
                                            }
                                            catch(Exception ex)
                                            {

                                            }
                                            //if (oAttackPatternProbingTechnique == null)
                                            if (iAttackPatternProbingTechniqueID <= 0)
                                            {
                                                PROBINGTECHNIQUEFORATTACKPATTERN oAttackPatternProbingTechnique = new PROBINGTECHNIQUEFORATTACKPATTERN();
                                                oAttackPatternProbingTechnique.ProbingTechniqueID = iProbingTechniqueID;    // oProbingTechnique.ProbingTechniqueID;
                                                oAttackPatternProbingTechnique.AttackPatternID = myattackpatternid;
                                                //TODO
                                                
                                                oAttackPatternProbingTechnique.CreatedDate = DateTimeOffset.Now;
                                                oAttackPatternProbingTechnique.VocabularyID = iVocabularyCAPECID;
                                                oAttackPatternProbingTechnique.timestamp = DateTimeOffset.Now;
                                                model.PROBINGTECHNIQUEFORATTACKPATTERN.Add(oAttackPatternProbingTechnique);
                                                model.SaveChanges();
                                                //iAttackPatternProbingTechniqueID=
                                            }
                                            else
                                            {
                                                //Update PROBINGTECHNIQUEFORATTACKPATTERN
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                            #endregion capecprobingtechniques

                        case "capec:Resources_Required":
                            #region capecresourcesrequired
                            string sResourceRequired = CleaningCAPECString(nodeAP.InnerText);
                            if (sResourceRequired != "")    //Not: we could have just <capec:Resources_Required/>
                            {
                                //Optimization because no update
                                //ATTACKRESOURCE oAttackRessource = attack_model.ATTACKRESOURCE.FirstOrDefault(o => o.AttackResourceText == sResourceRequired);
                                int iAttackResourceID = 0;
                                try
                                {
                                    iAttackResourceID = attack_model.ATTACKRESOURCE.Where(o => o.AttackResourceText == sResourceRequired).Select(o => o.AttackResourceID).FirstOrDefault();
                                }
                                catch (Exception ex)
                                {

                                }
                                //if (oAttackRessource == null)
                                if (iAttackResourceID <= 0)
                                {
                                    ATTACKRESOURCE oAttackRessource = new ATTACKRESOURCE();
                                    oAttackRessource.AttackResourceText = sResourceRequired;
                                    //oAttackRessource.AttackResourceTextRaw = nodeAP.InnerText;
                                    oAttackRessource.CreatedDate = DateTimeOffset.Now;
                                    oAttackRessource.timestamp = DateTimeOffset.Now;
                                    oAttackRessource.VocabularyID = iVocabularyCAPECID;
                                    attack_model.ATTACKRESOURCE.Add(oAttackRessource);
                                    attack_model.SaveChanges();
                                    iAttackResourceID = oAttackRessource.AttackResourceID;
                                }
                                else
                                {
                                    //Update ATTACKRESOURCE
                                }

                                //ATTACKRESOURCEFORCAPEC oAttackPatternResourceRequired = attack_model.ATTACKRESOURCEFORCAPEC.FirstOrDefault(o => o.capec_id == sCAPECID && o.AttackResourceID == iAttackResourceID);
                                int iCAPECAttackResourceID = 0;
                                try
                                {
                                    iCAPECAttackResourceID = attack_model.ATTACKRESOURCEFORATTACKPATTERN.Where(o => o.AttackPatternID == myattackpatternid && o.AttackResourceID == iAttackResourceID).Select(o => o.AttackPatternAttackResourceRequiredID).FirstOrDefault();
                                }
                                catch (Exception ex)
                                {

                                }
                                //if (oAttackPatternResourceRequired == null)
                                if (iCAPECAttackResourceID <= 0)
                                {
                                    ATTACKRESOURCEFORATTACKPATTERN oAttackPatternResourceRequired = new ATTACKRESOURCEFORATTACKPATTERN();
                                    oAttackPatternResourceRequired.AttackResourceID = iAttackResourceID;    // oAttackRessource.AttackResourceID;
                                    //oAttackPatternResourceRequired.capec_id = sCAPECID; //Removed
                                    oAttackPatternResourceRequired.AttackPatternID = myattackpatternid;
                                    oAttackPatternResourceRequired.CreatedDate = DateTimeOffset.Now;
                                    oAttackPatternResourceRequired.timestamp = DateTimeOffset.Now;
                                    oAttackPatternResourceRequired.VocabularyID = iVocabularyCAPECID;
                                    attack_model.ATTACKRESOURCEFORATTACKPATTERN.Add(oAttackPatternResourceRequired);
                                    //attack_model.SaveChanges();    //TEST PERFORMANCE
                                    //iCAPECAttackResourceID=
                                }
                                else
                                {
                                    //Update ATTACKRESOURCEFORATTACKPATTERN
                                }
                            }
                            break;
                            #endregion capecresourcesrequired

                        //TODO
                        case "capec:Related_Vulnerabilities":
                            #region relatedvulns
                            foreach (XmlNode nodeRelatedVulnerability in nodeAP.ChildNodes)
                            {
                                if (nodeRelatedVulnerability.Name == "capec:Related_Vulnerability")
                                {
                                    VULNERABILITY oVulnerability = null;
                                    bool bNewVulnerability = false;
                                    string sCVEID="";
                                    foreach (XmlNode nodeRelatedVulnerabilityInfo in nodeRelatedVulnerability)
                                    {
                                        switch(nodeRelatedVulnerabilityInfo.Name)
                                        {
                                            case "capec:Vulnerability_ID":
                                                sCVEID=nodeRelatedVulnerabilityInfo.InnerText;
                                                //Regex myRegexCVE = new Regex("CVE-[0-9][0-9][0-9][0-9]-[0-9][0-9][0-9][0-9]");
                                                Regex myRegexCVE = new Regex(@"CVE-(19|20)\d\d-(0\d{3}|[1-9]\d{3,})");
                                                //https://cve.mitre.org/cve/identifiers/tech-guidance.html
                                                string strTemp = myRegexCVE.Match(sCVEID).ToString();
                                                if (strTemp != "")
                                                {
                                                    //It is a CVE
                                                    oVulnerability = vuln_model.VULNERABILITY.FirstOrDefault(o => o.VULReferentialID == sCVEID); //CVE-2007-2139
                                                }
                                                else
                                                {
                                                    //It is not a CVE
                                                    //Microsoft Security Bulletin MS04-028
                                                    //TODO: Parse and extract VULReferentialID
                                                    oVulnerability = vuln_model.VULNERABILITY.FirstOrDefault(o => o.VULName == sCVEID);
                                                }
                                                if (oVulnerability==null)
                                                {
                                                    bNewVulnerability = true;
                                                    try
                                                    {
                                                        oVulnerability = new VULNERABILITY();
                                                        oVulnerability.CreatedDate = DateTimeOffset.Now;
                                                        if (sCVEID.ToUpper().StartsWith("CVE-"))
                                                        {
                                                            oVulnerability.VULReferential = "cve";
                                                            oVulnerability.VULReferentialID = sCVEID;
                                                        }
                                                        else
                                                        {
                                                            ////Microsoft Security Bulletin MS04-028
                                                            oVulnerability.VULName = sCVEID;
                                                            //TODO VULReferential
                                                            oVulnerability.VULReferential = "Microsoft";
                                                            //TODO PATCH    VULNERABILITYPATCH
                                                        }
                                                        oVulnerability.VocabularyID = iVocabularyCAPECID;
                                                        oVulnerability.timestamp = DateTimeOffset.Now;
                                                        vuln_model.VULNERABILITY.Add(oVulnerability);
                                                        vuln_model.SaveChanges();
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
                                                    //TODO catch(exception
                                                }
                                                else
                                                {
                                                    //Update VULNERABILITY
                                                }
                                                break;

                                            case "capec:Vulnerability_Description":
                                                if (bNewVulnerability)
                                                {
                                                    try
                                                    {
                                                        //Update VULNERABILITY
                                                        oVulnerability.VULDescription = CleaningCAPECString(nodeRelatedVulnerabilityInfo.InnerText);
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
                                                    //TODO catch(exception
                                                }
                                                break;

                                            default:
                                                Console.WriteLine("ERROR: Missing code " + sCAPECID + " for nodeRelatedVulnerabilityInfo " + nodeRelatedVulnerabilityInfo.Name);
                                                break;
                                        }
                                    }

                                    int iAttackPatternVulnerabilityID = 0;
                                    try
                                    {
                                        iAttackPatternVulnerabilityID = vuln_model.VULNERABILITYFORATTACKPATTERN.Where(o => o.AttackPatternID == myattackpatternid && o.VulnerabilityID == oVulnerability.VulnerabilityID).Select(o => o.AttackPatternVulnerabilityID).FirstOrDefault();
                                    }
                                    catch(Exception ex)
                                    {

                                    }
                                    //VULNERABILITYFORATTACKPATTERN oAttackPatternVUL = model.VULNERABILITYFORATTACKPATTERN.FirstOrDefault(o => o.AttackPatternID == myattackpatternid && o.VulnerabilityID == oVulnerability.VulnerabilityID);
                                    //if(oAttackPatternVUL==null)
                                    if (iAttackPatternVulnerabilityID<=0)
                                    {
                                        try
                                        {
                                            VULNERABILITYFORATTACKPATTERN oAttackPatternVUL = new VULNERABILITYFORATTACKPATTERN();
                                            oAttackPatternVUL.CreatedDate = DateTimeOffset.Now;
                                            oAttackPatternVUL.AttackPatternID = myattackpatternid;
                                            //oAttackPatternVUL.capec_id = sCAPECID;  //Removed
                                            oAttackPatternVUL.VulnerabilityID = oVulnerability.VulnerabilityID;
                                            if (sCVEID.ToUpper().StartsWith("CVE-"))
                                            {
                                                oAttackPatternVUL.CVEID = sCVEID;   //TODO Review Remove?
                                            }
                                            oAttackPatternVUL.VocabularyID = iVocabularyCAPECID;
                                            oAttackPatternVUL.timestamp = DateTimeOffset.Now;
                                            vuln_model.VULNERABILITYFORATTACKPATTERN.Add(oAttackPatternVUL);
                                            vuln_model.SaveChanges();
                                            //iAttackPatternVulnerabilityID=
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
                                        //TODO catch(exception
                                    }
                                    else
                                    {
                                        //Update VULNERABILITYFORATTACKPATTERN

                                    }
                                }
                                else
                                {
                                    Console.WriteLine("ERROR: Missing code " + sCAPECID + " for nodeRelatedVulnerability " + nodeRelatedVulnerability.Name);
                                }
                            }
                            break;
                            #endregion relatedvulns
                        
                        case "capec:Indicators-Warnings_of_Attack":
                            #region indicatorswarnings
                            foreach (XmlNode nodeIndicatorWarningAttack in nodeAP.ChildNodes)
                            {
                                if (nodeIndicatorWarningAttack.Name != "capec:Indicator-Warning_of_Attack")
                                {
                                    Console.WriteLine("ERROR: TODO Missing code for nodeIndicatorWarningAttack " + nodeIndicatorWarningAttack.Name);
                                }
                                else
                                {
                                    foreach (XmlNode nodeIndicatorWarningAttackInfo in nodeIndicatorWarningAttack)
                                    {
                                        if (nodeIndicatorWarningAttackInfo.Name != "capec:Description")
                                        {
                                            Console.WriteLine("ERROR: TODO Missing code "+sCAPECID+" for nodeIndicatorWarningAttackInfo " + nodeIndicatorWarningAttackInfo.Name);
                                        }
                                        else
                                        {
                                            string sIndicatorWarningAttack = CleaningCAPECString(nodeIndicatorWarningAttackInfo.InnerText);
                                            //ATTACKPATTERNINDICATORWARNING oAttackPatternIndicatorWarning = attack_model.ATTACKPATTERNINDICATORWARNING.FirstOrDefault(o => o.AttackPatternID == myattackpatternid && o.IndicatorWarningAttack == sIndicatorWarningAttack);
                                            int iAttackPatternIndicatorID = 0;
                                            try
                                            {
                                                iAttackPatternIndicatorID=attack_model.ATTACKPATTERNINDICATORWARNING.Where(o => o.AttackPatternID == myattackpatternid && o.IndicatorWarningAttack == sIndicatorWarningAttack).Select(o => o.AttackPatternIndicatorWarningID).FirstOrDefault();
                                            }
                                            catch(Exception ex)
                                            {

                                            }
                                            //if (oAttackPatternIndicatorWarning == null)
                                            if (iAttackPatternIndicatorID <= 0)
                                            {
                                                try
                                                {
                                                    ATTACKPATTERNINDICATORWARNING oAttackPatternIndicatorWarning = new ATTACKPATTERNINDICATORWARNING();
                                                    oAttackPatternIndicatorWarning.CreatedDate = DateTimeOffset.Now;
                                                    oAttackPatternIndicatorWarning.AttackPatternID = myattackpatternid;
                                                    oAttackPatternIndicatorWarning.IndicatorWarningAttack = sIndicatorWarningAttack;
                                                    oAttackPatternIndicatorWarning.VocabularyID = iVocabularyCAPECID;
                                                    oAttackPatternIndicatorWarning.timestamp = DateTimeOffset.Now;
                                                    //isEncrypted
                                                    attack_model.ATTACKPATTERNINDICATORWARNING.Add(oAttackPatternIndicatorWarning);
                                                    attack_model.SaveChanges();
                                                    //iAttackPatternIndicatorID=
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
                                                    Console.WriteLine("Exception DbEntityValidationExceptionATTACKPATTERNINDICATORWARNING " + sb.ToString());
                                                }
                                                //TODO catch(exception
                                            }
                                            else
                                            {
                                                //Update ATTACKPATTERNINDICATORWARNING
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                            #endregion indicatorswarnings

                        case "capec:Target_Attack_Surface":
                            #region ATTACKSURFACE
                            //ATTACKSURFACEFORATTACKPATTERN oAttackPatternAttackSurface = attack_model.ATTACKSURFACEFORATTACKPATTERN.FirstOrDefault(o => o.AttackPatternID == myattackpatternid);
                            int iAttackSurfaceID = 0;
                            int iAttackPatternAttackSurfaceID =0;
                            try
                            {
                                iAttackPatternAttackSurfaceID=attack_model.ATTACKSURFACEFORATTACKPATTERN.Where(o => o.AttackPatternID == myattackpatternid).Select(o => o.AttackPatternSurfaceID).FirstOrDefault();
                            }
                            catch(Exception ex)
                            {

                            }
                            //if (oAttackPatternAttackSurface == null)
                            if (iAttackPatternAttackSurfaceID <= 0)
                            {
                                
                                try
                                {
                                    ATTACKSURFACE oAttackSurface = new ATTACKSURFACE();
                                    oAttackSurface.CreatedDate = DateTimeOffset.Now;

                                    oAttackSurface.VocabularyID = iVocabularyCAPECID;
                                    oAttackSurface.timestamp = DateTimeOffset.Now;
                                    attack_model.ATTACKSURFACE.Add(oAttackSurface);
                                    attack_model.SaveChanges();
                                    iAttackSurfaceID = oAttackSurface.AttackSurfaceID;
                                }
                                catch (Exception exATTACKSURFACE)
                                {
                                    Console.WriteLine("Exception exATTACKSURFACE " + exATTACKSURFACE.Message + " " + exATTACKSURFACE.InnerException);
                                }

                                
                                
                                try
                                {
                                    ATTACKSURFACEFORATTACKPATTERN oAttackPatternAttackSurface = new ATTACKSURFACEFORATTACKPATTERN();
                                    oAttackPatternAttackSurface.CreatedDate = DateTimeOffset.Now;
                                    oAttackPatternAttackSurface.AttackPatternID = myattackpatternid;
                                    oAttackPatternAttackSurface.AttackSurfaceID = iAttackSurfaceID; // oAttackSurface.AttackSurfaceID;
                                    oAttackPatternAttackSurface.VocabularyID = iVocabularyCAPECID;
                                    oAttackPatternAttackSurface.timestamp = DateTimeOffset.Now;
                                    attack_model.ATTACKSURFACEFORATTACKPATTERN.Add(oAttackPatternAttackSurface);
                                    attack_model.SaveChanges();
                                }
                                catch (Exception exATTACKSURFACEFORATTACKPATTERN)
                                {
                                    Console.WriteLine("Exception exATTACKSURFACEFORATTACKPATTERN " + exATTACKSURFACEFORATTACKPATTERN.Message + " " + exATTACKSURFACEFORATTACKPATTERN.InnerException);
                                }
                            }
                            else
                            {
                                iAttackSurfaceID = attack_model.ATTACKSURFACEFORATTACKPATTERN.Where(o => o.AttackPatternID == myattackpatternid).Select(o => o.AttackSurfaceID).FirstOrDefault();
                            }

                            foreach (XmlNode nodeAttackSurface in nodeAP.ChildNodes)
                            {
                                if (nodeAttackSurface.Name == "capec:Target_Attack_Surface_Description")
                                {
                                    foreach (XmlNode nodeAttackSurfaceInfo in nodeAttackSurface.ChildNodes)
                                    {
                                        switch(nodeAttackSurfaceInfo.Name)
                                        {
                                            case "capec:Targeted_OSI_Layers":
                                                foreach(XmlNode nodeAttackSurfaceOSILayer in nodeAttackSurfaceInfo.ChildNodes)
                                                {
                                                    string sTargetedLayer=nodeAttackSurfaceOSILayer.InnerText;  //Network Layer
                                                    int iOSILayerID = 0;
                                                    try
                                                    {
                                                        iOSILayerID = model.OSILAYER.Where(o => o.OSILayerName == sTargetedLayer).Select(o => o.OSILayerID).FirstOrDefault();
                                                    }
                                                    catch(Exception ex)
                                                    {

                                                    }
                                                    if (iOSILayerID <= 0)
                                                    {
                                                        try
                                                        {
                                                            OSILAYER oOSILAYER = new OSILAYER();
                                                            oOSILAYER.OSILayerName = sTargetedLayer;
                                                            //oOSILAYER.CreatedDate = DateTimeOffset.Now;
                                                            oOSILAYER.VocabularyID = iVocabularyCAPECID;
                                                            model.OSILAYER.Add(oOSILAYER);
                                                            model.SaveChanges();
                                                            iOSILayerID = oOSILAYER.OSILayerID;
                                                        }
                                                        catch(Exception exoOSILAYER)
                                                        {
                                                            Console.WriteLine("Exception exoOSILAYER " + exoOSILAYER.Message + " " + exoOSILAYER.InnerException);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //Update OSILAYER
                                                    }

                                                    int iAttackSurfaceOSILayerID = 0;
                                                    try
                                                    {
                                                        iAttackSurfaceOSILayerID = model.OSILAYERFORATTACKSURFACE.Where(o => o.OSILayerID == iOSILayerID && o.AttackSurfaceID == iAttackSurfaceID).Select(o=>o.AttackSurfaceOSILayerID).FirstOrDefault();
                                                    }
                                                    catch(Exception ex)
                                                    {

                                                    }
                                                    //OSILAYERFORATTACKSURFACE oAttackSurfaceOSILayer = null;
                                                    //oAttackSurfaceOSILayer = model.OSILAYERFORATTACKSURFACE.FirstOrDefault(o => o.OSILayerID == iOSILayerID && o.AttackSurfaceID==iAttackSurfaceID);
                                                    //if(oAttackSurfaceOSILayer==null)
                                                    if (iAttackSurfaceOSILayerID<=0)
                                                    {
                                                        try
                                                        {
                                                            OSILAYERFORATTACKSURFACE oAttackSurfaceOSILayer = new OSILAYERFORATTACKSURFACE();
                                                            oAttackSurfaceOSILayer.OSILayerID = iOSILayerID;
                                                            oAttackSurfaceOSILayer.AttackSurfaceID = iAttackSurfaceID;
                                                            oAttackSurfaceOSILayer.CreatedDate = DateTimeOffset.Now;
                                                            oAttackSurfaceOSILayer.timestamp = DateTimeOffset.Now;
                                                            oAttackSurfaceOSILayer.VocabularyID = iVocabularyCAPECID;
                                                            model.OSILAYERFORATTACKSURFACE.Add(oAttackSurfaceOSILayer);
                                                            model.SaveChanges();
                                                            //iAttackSurfaceOSILayerID=
                                                        }
                                                        catch(Exception exoAttackSurfaceOSILayer)
                                                        {
                                                            Console.WriteLine("Exception exoAttackSurfaceOSILayer " + exoAttackSurfaceOSILayer.Message + " " + exoAttackSurfaceOSILayer.InnerException);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //Update OSILAYERFORATTACKSURFACE
                                                    }
                                                }
                                                break;
                                            
                                            case "capec:Target_Attack_Surface_Localities":
                                                //ATTACKSURFACELOCALITY
                                                foreach (XmlNode nodeAttackSurfaceLocality in nodeAttackSurfaceInfo.ChildNodes)
                                                {
                                                    string sAttackSurfaceLocalityName=CleaningCAPECString(nodeAttackSurfaceLocality.InnerText);  //Server-side
                                                    
                                                    int iAttackSurfaceLocalityID = 0;
                                                    try
                                                    {
                                                        iAttackSurfaceLocalityID = attack_model.ATTACKSURFACELOCALITY.Where(o => o.AttackSurfaceLocalityName == sAttackSurfaceLocalityName).Select(o => o.AttackSurfaceLocalityID).FirstOrDefault();
                                                    }
                                                    catch(Exception ex)
                                                    {

                                                    }
                                                    //ATTACKSURFACELOCALITY oAttackSurfaceLocality = attack_model.ATTACKSURFACELOCALITY.FirstOrDefault(o => o.AttackSurfaceLocalityName == sAttackSurfaceLocalityName);
                                                    //if(oAttackSurfaceLocality==null)
                                                    if (iAttackSurfaceLocalityID<=0)
                                                    {
                                                        try
                                                        {
                                                            ATTACKSURFACELOCALITY oAttackSurfaceLocality = new ATTACKSURFACELOCALITY();
                                                            oAttackSurfaceLocality.AttackSurfaceLocalityName = sAttackSurfaceLocalityName;
                                                            oAttackSurfaceLocality.CreatedDate = DateTimeOffset.Now;
                                                            oAttackSurfaceLocality.timestamp = DateTimeOffset.Now;
                                                            oAttackSurfaceLocality.VocabularyID = iVocabularyCAPECID;
                                                            attack_model.ATTACKSURFACELOCALITY.Add(oAttackSurfaceLocality);
                                                            attack_model.SaveChanges();
                                                            iAttackSurfaceLocalityID = oAttackSurfaceLocality.AttackSurfaceLocalityID;
                                                        }
                                                        catch(Exception exoAttackSurfaceLocality)
                                                        {
                                                            Console.WriteLine("Exception exoAttackSurfaceLocality " + exoAttackSurfaceLocality.Message + " " + exoAttackSurfaceLocality.InnerException);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //Update ATTACKSURFACELOCALITY
                                                    }

                                                    int iAttackSurfaceAttackSurfaceLocalityID = 0;
                                                    try
                                                    {
                                                        iAttackSurfaceAttackSurfaceLocalityID = attack_model.ATTACKSURFACELOCALITYFORATTACKSURFACE.Where(o => o.AttackSurfaceID == iAttackSurfaceID && o.AttackSurfaceLocalityID == iAttackSurfaceLocalityID).Select(o => o.AttackSurfaceLocalitiesID).FirstOrDefault();
                                                    }
                                                    catch(Exception ex)
                                                    {

                                                    }
                                                    //ATTACKSURFACELOCALITYFORATTACKSURFACE oAttackLocalitySurface = attack_model.ATTACKSURFACELOCALITYFORATTACKSURFACE.FirstOrDefault(o => o.AttackSurfaceID == iAttackSurfaceID && o.AttackSurfaceLocalityID == iAttackSurfaceLocalityID);
                                                    //if(oAttackLocalitySurface==null)
                                                    if (iAttackSurfaceAttackSurfaceLocalityID<=0)
                                                    {
                                                        try
                                                        {
                                                            ATTACKSURFACELOCALITYFORATTACKSURFACE oAttackLocalitySurface = new ATTACKSURFACELOCALITYFORATTACKSURFACE();
                                                            oAttackLocalitySurface.AttackSurfaceID = iAttackSurfaceID;
                                                            oAttackLocalitySurface.AttackSurfaceLocalityID = iAttackSurfaceLocalityID;  // oAttackSurfaceLocality.AttackSurfaceLocalityID;
                                                            oAttackLocalitySurface.CreatedDate = DateTimeOffset.Now;
                                                            oAttackLocalitySurface.timestamp = DateTimeOffset.Now;
                                                            oAttackLocalitySurface.VocabularyID = iVocabularyCAPECID;
                                                            attack_model.ATTACKSURFACELOCALITYFORATTACKSURFACE.Add(oAttackLocalitySurface);
                                                            attack_model.SaveChanges();
                                                            //iAttackSurfaceAttackSurfaceLocalityID=
                                                        }
                                                        catch(Exception exoAttackLocalitySurface)
                                                        {
                                                            Console.WriteLine("Exception exoAttackLocalitySurface " + exoAttackLocalitySurface.Message + " " + exoAttackLocalitySurface.InnerException);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //Update ATTACKSURFACELOCALITYFORATTACKSURFACE
                                                    }
                                                }
                                                break;
                                            
                                            case "capec:Target_Attack_Surface_Types":
                                                //ATTACKSURFACETYPE
                                                foreach (XmlNode nodeTargetAttackSurfaceType in nodeAttackSurfaceInfo.ChildNodes)
                                                {
                                                    string sAttackSurfaceTypeName = CleaningCAPECString(nodeTargetAttackSurfaceType.InnerText);  //Host
                                                    
                                                    int iAttackSurfaceTypeID = 0;
                                                    try
                                                    {
                                                        iAttackSurfaceTypeID = attack_model.ATTACKSURFACETYPE.Where(o => o.AttackSurfaceTypeName == sAttackSurfaceTypeName).Select(o => o.AttackSurfaceTypeID).FirstOrDefault();
                                                    }
                                                    catch(Exception ex)
                                                    {

                                                    }
                                                    //ATTACKSURFACETYPE oAttackSurfaceType = attack_model.ATTACKSURFACETYPE.FirstOrDefault(o => o.AttackSurfaceTypeName == sAttackSurfaceTypeName);
                                                    //if(oAttackSurfaceType==null)
                                                    if (iAttackSurfaceTypeID<=0)
                                                    {
                                                        try
                                                        {
                                                            ATTACKSURFACETYPE oAttackSurfaceType = new ATTACKSURFACETYPE();
                                                            oAttackSurfaceType.AttackSurfaceTypeName = sAttackSurfaceTypeName;
                                                            oAttackSurfaceType.CreatedDate = DateTimeOffset.Now;
                                                            oAttackSurfaceType.timestamp = DateTimeOffset.Now;
                                                            oAttackSurfaceType.VocabularyID = iVocabularyCAPECID;
                                                            attack_model.ATTACKSURFACETYPE.Add(oAttackSurfaceType);
                                                            attack_model.SaveChanges();
                                                            iAttackSurfaceTypeID = oAttackSurfaceType.AttackSurfaceTypeID;
                                                        }
                                                        catch(Exception exoAttackSurfaceType)
                                                        {
                                                            Console.WriteLine("Exception exoAttackSurfaceType " + exoAttackSurfaceType.Message + " " + exoAttackSurfaceType.InnerException);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //Update ATTACKSURFACETYPE
                                                    }

                                                    int iAttackSurfaceAttackSurfaceTypeID = 0;
                                                    try
                                                    {
                                                        iAttackSurfaceAttackSurfaceTypeID = attack_model.ATTACKSURFACETYPEFORATTACKSURFACE.Where(o => o.AttackSurfaceID == iAttackSurfaceID && o.AttackSurfaceTypeID == iAttackSurfaceTypeID).Select(o => o.AttackSurfaceTypesID).FirstOrDefault();
                                                    }
                                                    catch(Exception ex)
                                                    {

                                                    }
                                                    //ATTACKSURFACETYPEFORATTACKSURFACE oAttackSurfaceTypes=attack_model.ATTACKSURFACETYPEFORATTACKSURFACE.FirstOrDefault(o=>o.AttackSurfaceID==iAttackSurfaceID && o.AttackSurfaceTypeID==iAttackSurfaceTypeID);
                                                    //if(oAttackSurfaceTypes==null)
                                                    if (iAttackSurfaceAttackSurfaceTypeID<=0)
                                                    {
                                                        try
                                                        {
                                                            ATTACKSURFACETYPEFORATTACKSURFACE oAttackSurfaceTypes = new ATTACKSURFACETYPEFORATTACKSURFACE();
                                                            oAttackSurfaceTypes.AttackSurfaceID = iAttackSurfaceID;
                                                            oAttackSurfaceTypes.AttackSurfaceTypeID = iAttackSurfaceTypeID; // oAttackSurfaceType.AttackSurfaceTypeID;
                                                            oAttackSurfaceTypes.CreatedDate = DateTimeOffset.Now;
                                                            oAttackSurfaceTypes.timestamp = DateTimeOffset.Now;
                                                            oAttackSurfaceTypes.VocabularyID = iVocabularyCAPECID;
                                                            attack_model.ATTACKSURFACETYPEFORATTACKSURFACE.Add(oAttackSurfaceTypes);
                                                            attack_model.SaveChanges();
                                                            //iAttackSurfaceAttackSurfaceTypeID=
                                                        }
                                                        catch(Exception exoAttackSurfaceTypes)
                                                        {
                                                            Console.WriteLine("Exception exoAttackSurfaceTypes " + exoAttackSurfaceTypes.Message + " " + exoAttackSurfaceTypes.InnerException);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //Update ATTACKSURFACETYPEFORATTACKSURFACE
                                                    }
                                                }
                                                break;
                                            
                                            //TODO
                                            
                                            case "capec:Target_Functional_Services":
                                                //ATTACKSURFACESERVICE
                                                //ATTACKSURFACESERVICEPROTOCOL
                                                foreach (XmlNode nodeTargetFunctionalService in nodeAttackSurfaceInfo.ChildNodes)
                                                {
                                                    //<capec:Target_Functional_Service ID="1" Name="None">
                                                    //TODO: Review? (Name)
                                                    foreach (XmlNode nodeTargetFunctionalServiceInfo in nodeTargetFunctionalService.ChildNodes)
                                                    {
                                                        if (nodeTargetFunctionalServiceInfo.Name != "capec:Protocol")
                                                        {
                                                            Console.WriteLine("ERROR: Missing code for nodeTargetFunctionalServiceInfo " + nodeTargetFunctionalServiceInfo.Name);
                                                        }
                                                        else
                                                        {
                                                            //<capec:Protocol ID="1" Name="Internet Control Messaging Protocol">
                                                            PROTOCOL oProtocol = null;
                                                            try
                                                            {
                                                                string sProtocolName = CleaningCAPECString(nodeTargetFunctionalServiceInfo.Attributes["Name"].InnerText);
                                                                //Internet Control Messaging Protocol
                                                                oProtocol = model.PROTOCOL.FirstOrDefault(o => o.ProtocolName == sProtocolName);
                                                                if(oProtocol==null)
                                                                {
                                                                    oProtocol = new PROTOCOL();
                                                                    oProtocol.ProtocolName = sProtocolName;
                                                                    oProtocol.VocabularyID = iVocabularyCAPECID;
                                                                    oProtocol.CreatedDate = DateTimeOffset.Now;
                                                                    oProtocol.timestamp = DateTimeOffset.Now;
                                                                    model.PROTOCOL.Add(oProtocol);
                                                                    model.SaveChanges();
                                                                }
                                                                else
                                                                {
                                                                    //Update PROTOCOL
                                                                }
                                                                //TODO: ATTACKSURFACESERVICEPROTOCOL

                                                                
                                                            }
                                                            catch(Exception exProtocolName)
                                                            {
                                                                Console.WriteLine("Exception exProtocolName " + exProtocolName.Message + " " + exProtocolName.InnerException);
                                                            }

                                                            foreach (XmlNode nodeProtocolInfo in nodeTargetFunctionalServiceInfo.ChildNodes)
                                                            {
                                                                switch(nodeProtocolInfo.Name)
                                                                {
                                                                    case "capec:Protocol_Structure":
                                                                        #region protocolstructure
                                                                        foreach (XmlNode nodeProtocolStructureInfo in nodeProtocolInfo.ChildNodes)
                                                                        {
                                                                            string sProtocolFieldName = string.Empty;
                                                                            string sProtocolFieldDescription = string.Empty;
                                                                            string sProtocolOperationCode = string.Empty;
                                                                            string sProtocolData = string.Empty;
                                                                            string sProtocolFlagValue = string.Empty;
                                                                                
                                                                            if (nodeProtocolStructureInfo.Name != "capec:Protocol_Header")
                                                                            {
                                                                                Console.WriteLine("ERROR: Missing Code for Protocol_Structure " + nodeProtocolStructureInfo.Name);
                                                                            }
                                                                            else
                                                                            {
                                                                                
                                                                                foreach (XmlNode nodeProtocolHeaderInfo in nodeProtocolStructureInfo.ChildNodes)
                                                                                {
                                                                                    switch(nodeProtocolHeaderInfo.Name)
                                                                                    {
                                                                                        case "capec:Protocol_RFC":
                                                                                            //TODO: Review, use of PROTOCOLREFERENCE in case multiple RFCs for a PROTOCOL
                                                                                            string sRFCID=nodeProtocolHeaderInfo.InnerText; //RFC 792
                                                                                            //TODO: Add a REFERENCE for the RFC

                                                                                            sRFCID=sRFCID.ToLower().Trim();
                                                                                            sRFCID=sRFCID.Replace("rfc","").Trim(); //Hardcoded
                                                                                            try
                                                                                            {
                                                                                                //Update PROTOCOL
                                                                                                oProtocol.ProtocolRFC = sRFCID;
                                                                                                oProtocol.VocabularyID = iVocabularyCAPECID;
                                                                                                oProtocol.timestamp = DateTimeOffset.Now;
                                                                                                model.SaveChanges();
                                                                                            }
                                                                                            catch(Exception exProtocolRFC)
                                                                                            {
                                                                                                Console.WriteLine("Exception exProtocolRFC " + exProtocolRFC.Message + " " + exProtocolRFC.InnerException);
                                                                                            }
                                                                                            break;
                                                                                        case "capec:Protocol_Field_Name":
                                                                                            sProtocolFieldName = CleaningCAPECString(nodeProtocolHeaderInfo.InnerText);
                                                                                            break;
                                                                                        case "capec:Protocol_Field_Description":
                                                                                            sProtocolFieldDescription = CleaningCAPECString(nodeProtocolHeaderInfo.InnerText);
                                                                                            break;
                                                                                        case "capec:Protocol_Operation_Code":
                                                                                            sProtocolOperationCode = CleaningCAPECString(nodeProtocolHeaderInfo.InnerText);
                                                                                            break;
                                                                                        case "capec:Protocol_Data":
                                                                                            sProtocolData = CleaningCAPECString(nodeProtocolHeaderInfo.InnerText);
                                                                                            break;
                                                                                        case "capec:Protocol_Flag_Value":
                                                                                            sProtocolFlagValue = CleaningCAPECString(nodeProtocolHeaderInfo.InnerText);
                                                                                            break;
                                                                                        default:
                                                                                            Console.WriteLine("ERROR: Missing Code for Protocol_Header " + nodeProtocolHeaderInfo.Name);
                                                                                            break;
                                                                                    }
                                                                                }

                                                                                

                                                                            }

                                                                            //PROTOCOLHEADER
                                                                            #region protocolheader
                                                                            try
                                                                            {
                                                                                if (sProtocolFieldName != string.Empty)
                                                                                {
                                                                                    PROTOCOLHEADER oProtocolHeader = null;
                                                                                    oProtocolHeader = model.PROTOCOLHEADER.FirstOrDefault(o => o.Protocol_Field_Name == sProtocolFieldName);    //TODO: Review
                                                                                    if (oProtocolHeader == null)
                                                                                    {
                                                                                        oProtocolHeader = new PROTOCOLHEADER();
                                                                                        oProtocolHeader.Protocol_Field_Name = sProtocolFieldName;
                                                                                        oProtocolHeader.Protocol_Field_Description = sProtocolFieldDescription;
                                                                                        oProtocolHeader.Protocol_Operation_Code = sProtocolOperationCode;
                                                                                        oProtocolHeader.Protocol_Data = sProtocolData;
                                                                                        oProtocolHeader.Protocol_Flag_Value = sProtocolFlagValue;
                                                                                        oProtocolHeader.VocabularyID = iVocabularyCAPECID;
                                                                                        oProtocolHeader.CreatedDate = DateTimeOffset.Now;
                                                                                        oProtocolHeader.timestamp = DateTimeOffset.Now;
                                                                                        model.PROTOCOLHEADER.Add(oProtocolHeader);
                                                                                        model.SaveChanges();
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        //Update PROTOCOLHEADER
                                                                                    }

                                                                                    //TODO: PROTOCOLHEADERS

                                                                                }
                                                                            }
                                                                            catch(Exception exProtocolHeader)
                                                                            {
                                                                                Console.WriteLine("Exception exProtocolHeader " + exProtocolHeader.Message + " " + exProtocolHeader.InnerException);
                                                                            }
                                                                            #endregion protocolheader
                                                                        }
                                                                        #endregion protocolstructure
                                                                        break;
                                                                    case "capec:Related_Protocols":
                                                                        #region relatedprotocols
                                                                        foreach (XmlNode nodeRelatedProtocol in nodeProtocolInfo.ChildNodes)
                                                                        {
                                                                            //<capec:Related_Protocol Name="Internet Protocol" RFC="791">
                                                                            string sRelatedProtocolName=nodeRelatedProtocol.Attributes["Name"].InnerText;
                                                                            sRelatedProtocolName=CleaningCAPECString(sRelatedProtocolName).Trim();

                                                                            string sRelatedProtocolRFC = string.Empty;
                                                                            try
                                                                            {
                                                                                sRelatedProtocolRFC = nodeRelatedProtocol.Attributes["RFC"].InnerText;
                                                                            }
                                                                            catch(Exception exRelatedProtocolRFC)
                                                                            {
                                                                                Console.WriteLine("NOTE: exRelatedProtocolRFC No RFC specified for capec:Related_Protocol " + sRelatedProtocolName+"\n"+ exRelatedProtocolRFC.Message);
                                                                            }

                                                                            int iRelatedProtocolID = 0;
                                                                            try
                                                                            {
                                                                                iRelatedProtocolID = model.PROTOCOL.Where(o => o.ProtocolName == sRelatedProtocolName).Select(o=>o.ProtocolID).FirstOrDefault();
                                                                            }
                                                                            catch(Exception ex)
                                                                            {

                                                                            }
                                                                            //PROTOCOL oRelatedProtocol = model.PROTOCOL.FirstOrDefault(o => o.ProtocolName == sRelatedProtocolName);
                                                                            //if (oRelatedProtocol == null)
                                                                            if(iRelatedProtocolID<=0)
                                                                            {
                                                                                //Search again by Abbreviation
                                                                                iRelatedProtocolID = model.PROTOCOL.Where(o => o.ProtocolAbbreviation == sRelatedProtocolName).Select(o=>o.ProtocolID).FirstOrDefault();
                                                                            }
                                                                            try
                                                                            {
                                                                                if (iRelatedProtocolID <= 0)
                                                                                {
                                                                                    PROTOCOL oRelatedProtocol = new PROTOCOL();
                                                                                    oRelatedProtocol.ProtocolName = sRelatedProtocolName;
                                                                                    oRelatedProtocol.ProtocolRFC = sRelatedProtocolRFC;
                                                                                    oRelatedProtocol.VocabularyID = iVocabularyCAPECID;
                                                                                    oRelatedProtocol.CreatedDate = DateTimeOffset.Now;
                                                                                    oRelatedProtocol.timestamp = DateTimeOffset.Now;
                                                                                    model.PROTOCOL.Add(oRelatedProtocol);
                                                                                    model.SaveChanges();
                                                                                    iRelatedProtocolID = oRelatedProtocol.ProtocolID;
                                                                                }
                                                                                else
                                                                                {
                                                                                    //Update PROTOCOL Related
                                                                                    /*
                                                                                    if (oRelatedProtocol.ProtocolRFC != sRelatedProtocolRFC)
                                                                                    {
                                                                                        oRelatedProtocol.ProtocolRFC = sRelatedProtocolRFC;
                                                                                        oRelatedProtocol.VocabularyID = iVocabularyCAPECID;
                                                                                        oRelatedProtocol.timestamp = DateTimeOffset.Now;
                                                                                        model.SaveChanges();
                                                                                        Console.WriteLine("DEBUG Note: RelatedProtocolRFC updated: " + oRelatedProtocol.ProtocolRFC + "=>" + sRelatedProtocolRFC);
                                                                                    }
                                                                                    */
                                                                                }
                                                                            }
                                                                            catch(Exception exRelatedProtocol)
                                                                            {
                                                                                Console.WriteLine("Exception exRelatedProtocol " + exRelatedProtocol.Message + " " + exRelatedProtocol.InnerException);
                                                                            }

                                                                            foreach (XmlNode nodeRelatedProtocolInfo in nodeRelatedProtocol.ChildNodes)
                                                                            {
                                                                                if (nodeRelatedProtocolInfo.Name != "capec:Relationship_Type")
                                                                                {
                                                                                    Console.WriteLine("ERROR: Missing Code for capec:Related_Protocol " + nodeRelatedProtocolInfo.Name);
                                                                                }
                                                                                else
                                                                                {
                                                                                    string sProtocolRelationshipType = nodeRelatedProtocolInfo.InnerText;   //Uses Protocol
                                                                                    sProtocolRelationshipType = CleaningCAPECString(sProtocolRelationshipType).Trim();

                                                                                    //PROTOCOLFORPROTOCOL oProtocolRelationship = model.PROTOCOLFORPROTOCOL.FirstOrDefault(o => o.ProtocolRefID == oProtocol.ProtocolID && o.ProtocolSubjectID == iRelatedProtocolID && o.ProtocolRelationshipName == sProtocolRelationshipType);
                                                                                    int iProtocolRelationshipID = 0;
                                                                                    try
                                                                                    {
                                                                                        iProtocolRelationshipID = model.PROTOCOLFORPROTOCOL.Where(o => o.ProtocolRefID == oProtocol.ProtocolID && o.ProtocolSubjectID == iRelatedProtocolID && o.ProtocolRelationshipName == sProtocolRelationshipType).Select(o=>o.ProtocolRelationshipID).FirstOrDefault();
                                                                                    }
                                                                                    catch(Exception ex)
                                                                                    {

                                                                                    }
                                                                                    try
                                                                                    {
                                                                                        //if (oProtocolRelationship == null)
                                                                                        if (iProtocolRelationshipID<=0)
                                                                                        {
                                                                                            PROTOCOLFORPROTOCOL oProtocolRelationship = new PROTOCOLFORPROTOCOL();
                                                                                            oProtocolRelationship.ProtocolRefID = oProtocol.ProtocolID;
                                                                                            oProtocolRelationship.ProtocolRelationshipName = sProtocolRelationshipType; //Uses Protocol
                                                                                            oProtocolRelationship.ProtocolSubjectID = iRelatedProtocolID;   // oRelatedProtocol.ProtocolID;
                                                                                            oProtocolRelationship.VocabularyID = iVocabularyCAPECID;
                                                                                            oProtocolRelationship.CreatedDate = DateTimeOffset.Now;
                                                                                            oProtocolRelationship.timestamp = DateTimeOffset.Now;
                                                                                            model.PROTOCOLFORPROTOCOL.Add(oProtocolRelationship);
                                                                                            model.SaveChanges();
                                                                                            //iProtocolRelationshipID=
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            //Update PROTOCOLFORPROTOCOL
                                                                                        }
                                                                                    }
                                                                                    catch(Exception exPROTOCOLFORPROTOCOL)
                                                                                    {
                                                                                        Console.WriteLine("Exception exPROTOCOLFORPROTOCOL " + exPROTOCOLFORPROTOCOL.Message + " " + exPROTOCOLFORPROTOCOL.InnerException);
                                                                                    }
                                                                                }
                                                                            }
                                                                            
                                                                        }
                                                                        #endregion relatedprotocols
                                                                        break;
                                                                    default:
                                                                        Console.WriteLine("ERROR: Missing Code for Target_Functional_Service Protocol " + nodeProtocolInfo.Name);
                                                                        break;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                break;
                                            
                                            default:
                                                Console.WriteLine("ERROR: Missing code for nodeAttackSurfaceInfo " + nodeAttackSurfaceInfo.Name);
                                                break;
                                        }
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("ERROR: Missing code for nodeAttackSurface " + nodeAttackSurface.Name);
                                }
                            }
                            break;
                        #endregion ATTACKSURFACE

                        case "capec:Content_History":
                            #region capeccontenthistory
                            foreach (XmlNode nodeContentHistory in nodeAP.ChildNodes)
                            {
                                if (nodeContentHistory.Name == "capec:Submissions")
                                {
                                    #region capecsubmissions
                                    foreach (XmlNode nodeContentSubmission in nodeContentHistory.ChildNodes)
                                    {
                                        //<capec:Submission Submission_Source="Internal_CAPEC_Team">
                                        if (nodeContentSubmission.Name == "capec:Submission")
                                        {
                                            try
                                            {
                                                //TODO
                                                Console.WriteLine("TODO: Submission_Source=" + nodeContentSubmission.Attributes["Submission_Source"].InnerText);    //Hardcoded
                                            }
                                            catch (Exception exSubmission)
                                            {
                                                Console.WriteLine("Exception: exSubmission No Submission_Source " + exSubmission.Message);
                                            }

                                            foreach (XmlNode nodeContentSubmissionInfo in nodeContentSubmission.ChildNodes)
                                            {
                                                switch (nodeContentSubmissionInfo.Name)
                                                {
                                                    case "capec:Submitter":
                                                        //CAPEC Content Team
                                                        Console.WriteLine("TODO: capec:Submitter " + nodeContentSubmissionInfo.Name + "=" + nodeContentSubmissionInfo.InnerText);
                                                        break;
                                                    case "capec:Submitter_Organization":
                                                        //The MITRE Corporation
                                                        string sOrganisationName = nodeContentSubmissionInfo.InnerText.Trim();
                                                        //Console.WriteLine("TODO: capec:Submitter_Organization " + nodeContentSubmissionInfo.Name + "=" + sOrganisationName);
                                                        #region organization
                                                        int iOrganisationID = 0;
                                                        try
                                                        {
                                                            iOrganisationID = model.ORGANISATION.Where(o => o.OrganisationName == sOrganisationName).Select(o => o.OrganisationID).FirstOrDefault();
                                                        }
                                                        catch (Exception exOrganisation)
                                                        {
                                                            Console.WriteLine("Exception: exOrganisation " + exOrganisation.Message + " " + exOrganisation.InnerException);
                                                        }
                                                        try
                                                        {
                                                            if (iOrganisationID <= 0)
                                                            {
                                                                ORGANISATION oOrganisation = new ORGANISATION();
                                                                oOrganisation.OrganisationName = sOrganisationName; //The MITRE Corporation
                                                                //HARDCODED
                                                                if (sOrganisationName.Contains("GFI")) { oOrganisation.OrganisationKnownAs = "GFI"; }   //GFI Software
                                                                if (sOrganisationName.Contains("MITRE")) { oOrganisation.OrganisationKnownAs = "MITRE"; }
                                                                if (sOrganisationName.Contains("SCAP.com")) { oOrganisation.OrganisationKnownAs = "SCAP"; } //SCAP.com, LLC
                                                                if (sOrganisationName.Contains("ThreatGuard")) { oOrganisation.OrganisationKnownAs = "ThreatGuard"; }   //ThreatGuard, Inc.
                                                                if (sOrganisationName.Contains("Hewlett-Packard")) { oOrganisation.OrganisationKnownAs = "HP"; }    //Hewlett-Packard
                                                                if (sOrganisationName.Contains("Symantec")) { oOrganisation.OrganisationKnownAs = "Symantec"; }    //Symantec Corporation
                                                                if (sOrganisationName.Contains("SecPod")) { oOrganisation.OrganisationKnownAs = "SecPod"; } //SecPod Technologies
                                                                if (sOrganisationName.Contains("Gideon")) { oOrganisation.OrganisationKnownAs = "Gideon"; }   //Gideon Technologies, Inc.
                                                                if (sOrganisationName.Contains("Secure Elements")) { oOrganisation.OrganisationKnownAs = "Secure Elements"; }   //Secure Elements, Inc.
                                                                if (sOrganisationName.Contains("Lumension")) { oOrganisation.OrganisationKnownAs = "Lumension"; }   //Lumension Security, Inc.
                                                                if (sOrganisationName.Contains("McAfee")) { oOrganisation.OrganisationKnownAs = "McAfee"; }  //McAfee, Inc.  (Intel Security)
                                                                if (sOrganisationName.Contains("BigFix")) { oOrganisation.OrganisationKnownAs = "BigFix"; }  //BigFix, Inc
                                                                if (sOrganisationName.Contains("National Institute of Standards and Technology")) { oOrganisation.OrganisationKnownAs = "NIST"; }  //National Institute of Standards and Technology
                                                                if (sOrganisationName.Contains("SAINT")) { oOrganisation.OrganisationKnownAs = "SAINT"; }   //SAINT Corporation
                                                                if (sOrganisationName.Contains("Pivotal")) { oOrganisation.OrganisationKnownAs = "Pivotal"; }   //Pivotal Security LLC
                                                                if (sOrganisationName.Contains("BAE")) { oOrganisation.OrganisationKnownAs = "BAE"; }   //BAE Systems Inc.

                                                                oOrganisation.VocabularyID = iVocabularyCAPECID;
                                                                oOrganisation.CreatedDate = DateTimeOffset.Now;
                                                                oOrganisation.timestamp = DateTimeOffset.Now;
                                                                model.ORGANISATION.Add(oOrganisation);
                                                                model.SaveChanges();
                                                                //iOrganisationID=
                                                            }
                                                            else
                                                            {
                                                                //Update ORGANISATION
                                                            }
                                                        }
                                                        catch (Exception exOrganisation2)
                                                        {
                                                            Console.WriteLine("Exception: exOrganisation2 " + exOrganisation2.Message + " " + exOrganisation2.InnerException);
                                                        }
                                                        #endregion organization
                                                        break;
                                                    case "capec:Submission_Date":
                                                        //2014-06-23
                                                        Console.WriteLine("TODO: capec:Submission_Date " + nodeContentSubmissionInfo.Name + "=" + nodeContentSubmissionInfo.InnerText);
                                                        //TODO:
                                                        //ATTACKPATTERN.Submission_Date or ATTACKPATTERNREPOSITORY.Submission_Date
                                                        break;
                                                    default:
                                                        Console.WriteLine("ERROR: TODO Missing code for nodeContentSubmissionInfo " + nodeContentSubmissionInfo.Name);
                                                        break;
                                                }
                                            }
                                        }
                                    }
                                    #endregion capecsubmissions
                                }
                                else
                                {
                                    if (nodeContentHistory.Name == "capec:Modifications")
                                    {
                                        #region capecmodifications
                                        foreach (XmlNode nodeContentModification in nodeContentHistory.ChildNodes)
                                        {
                                            //<capec:Modification Modification_Source="Internal_CAPEC_Team">
                                            if (nodeContentModification.Name == "capec:Modification")
                                            {
                                                try
                                                {
                                                    //TODO
                                                    Console.WriteLine("TODO: Modification_Source=" + nodeContentModification.Attributes["Modification_Source"].InnerText);    //Hardcoded
                                                }
                                                catch (Exception exModificationView)
                                                {
                                                    Console.WriteLine("Exception: exModificationView No Modification_Source " + exModificationView.Message);
                                                }

                                                foreach (XmlNode nodeContentModificationInfo in nodeContentModification.ChildNodes)
                                                {
                                                    switch (nodeContentModificationInfo.Name)
                                                    {
                                                        case "capec:Modifier":
                                                            //CAPEC Content Team
                                                            Console.WriteLine("TODO: capec:Modifier " + nodeContentModificationInfo.Name + "=" + nodeContentModificationInfo.InnerText);
                                                            break;
                                                        case "capec:Modifier_Organization":
                                                            //The MITRE Corporation
                                                            string sOrganisationName = nodeContentModificationInfo.InnerText.Trim();
                                                            Console.WriteLine("DEBUG: capec:Modifier_Organization " + nodeContentModificationInfo.Name + "=" + sOrganisationName);
                                                            #region modifierorganization
                                                            int iOrganisationID = 0;
                                                            try
                                                            {
                                                                iOrganisationID = model.ORGANISATION.Where(o => o.OrganisationName == sOrganisationName).Select(o => o.OrganisationID).FirstOrDefault();
                                                            }
                                                            catch (Exception exModifierOrganisation)
                                                            {
                                                                Console.WriteLine("Exception: exModifierOrganisation " + exModifierOrganisation.Message + " " + exModifierOrganisation.InnerException);
                                                            }
                                                            try
                                                            {
                                                                if (iOrganisationID <= 0)
                                                                {
                                                                    ORGANISATION oOrganisation = new ORGANISATION();
                                                                    oOrganisation.OrganisationName = sOrganisationName; //The MITRE Corporation
                                                                    //HARDCODED
                                                                    if (sOrganisationName.Contains("GFI")) { oOrganisation.OrganisationKnownAs = "GFI"; }   //GFI Software
                                                                    if (sOrganisationName.Contains("MITRE")) { oOrganisation.OrganisationKnownAs = "MITRE"; }
                                                                    if (sOrganisationName.Contains("SCAP.com")) { oOrganisation.OrganisationKnownAs = "SCAP"; } //SCAP.com, LLC
                                                                    if (sOrganisationName.Contains("ThreatGuard")) { oOrganisation.OrganisationKnownAs = "ThreatGuard"; }   //ThreatGuard, Inc.
                                                                    if (sOrganisationName.Contains("Hewlett-Packard")) { oOrganisation.OrganisationKnownAs = "HP"; }    //Hewlett-Packard
                                                                    if (sOrganisationName.Contains("Symantec")) { oOrganisation.OrganisationKnownAs = "Symantec"; }    //Symantec Corporation
                                                                    if (sOrganisationName.Contains("SecPod")) { oOrganisation.OrganisationKnownAs = "SecPod"; } //SecPod Technologies
                                                                    if (sOrganisationName.Contains("Gideon")) { oOrganisation.OrganisationKnownAs = "Gideon"; }   //Gideon Technologies, Inc.
                                                                    if (sOrganisationName.Contains("Secure Elements")) { oOrganisation.OrganisationKnownAs = "Secure Elements"; }   //Secure Elements, Inc.
                                                                    if (sOrganisationName.Contains("Lumension")) { oOrganisation.OrganisationKnownAs = "Lumension"; }   //Lumension Security, Inc.
                                                                    if (sOrganisationName.Contains("McAfee")) { oOrganisation.OrganisationKnownAs = "McAfee"; }  //McAfee, Inc.  (Intel Security)
                                                                    if (sOrganisationName.Contains("BigFix")) { oOrganisation.OrganisationKnownAs = "BigFix"; }  //BigFix, Inc
                                                                    if (sOrganisationName.Contains("National Institute of Standards and Technology")) { oOrganisation.OrganisationKnownAs = "NIST"; }  //National Institute of Standards and Technology
                                                                    if (sOrganisationName.Contains("SAINT")) { oOrganisation.OrganisationKnownAs = "SAINT"; }   //SAINT Corporation
                                                                    if (sOrganisationName.Contains("Pivotal")) { oOrganisation.OrganisationKnownAs = "Pivotal"; }   //Pivotal Security LLC
                                                                    if (sOrganisationName.Contains("BAE")) { oOrganisation.OrganisationKnownAs = "BAE"; }   //BAE Systems Inc.

                                                                    oOrganisation.VocabularyID = iVocabularyCAPECID;
                                                                    oOrganisation.CreatedDate = DateTimeOffset.Now;
                                                                    oOrganisation.timestamp = DateTimeOffset.Now;
                                                                    model.ORGANISATION.Add(oOrganisation);
                                                                    model.SaveChanges();
                                                                    //iOrganisationID=
                                                                }
                                                                else
                                                                {
                                                                    //Update ORGANISATION
                                                                }
                                                            }
                                                            catch (Exception exModifierOrganisation2)
                                                            {
                                                                Console.WriteLine("Exception: exModifierOrganisation2 " + exModifierOrganisation2.Message + " " + exModifierOrganisation2.InnerException);
                                                            }
                                                            #endregion modifierorganization
                                                            break;
                                                        case "capec:Modification_Date":
                                                            //2014-06-23
                                                            Console.WriteLine("TODO: capec:Modification_Date " + nodeContentModificationInfo.Name + "=" + nodeContentModificationInfo.InnerText);
                                                            //TODO:
                                                            //ATTACKPATTERNMODIFICATION.Modification_Date(s) or ATTACKPATTERNREPOSITORYMODIFICATION.Modification_Date(s)
                                                            break;
                                                        case "capec:Modification_Comment":
                                                            //Updated Related_Attack_Patterns
                                                            Console.WriteLine("TODO: capec:Modification_Comment " + nodeContentModificationInfo.Name + "=" + nodeContentModificationInfo.InnerText);

                                                            break;
                                                        default:
                                                            Console.WriteLine("ERROR: TODO Missing code for nodeContentModificationInfo " + nodeContentModificationInfo.Name);
                                                            break;
                                                    }
                                                }
                                            }
                                        }
                                        #endregion capecmodifications
                                    }
                                    else
                                    {
                                        Console.WriteLine("ERROR: TODO Missing code for nodeContentHistory " + nodeContentHistory.Name);
                                    }
                                }
                            }
                            #endregion capeccontenthistory
                            break;
                        
                        default:
                            Console.WriteLine("ERROR: TODO Missing code for AttackPattern " + nodeAP.Name);
                            //#comment
                            Console.WriteLine("ERROR: TODO Missing code for AttackPattern " + nodeAP.InnerText);
                            
                            break;
                    }


                }

            }
            #endregion AttackPattern

            nodes1 = doc.SelectNodes("capec:Attack_Pattern_Catalog/capec:Environments/capec:Environment", mgr);
            #region Environment
            foreach (XmlNode node in nodes1)    //capec:Environment
            {
                string sEnvID = node.Attributes["ID"].InnerText;
                string sEnvironmentTitle = "";
                string sEnvironmentDescription = "";
                
                foreach (XmlNode nodeEnvAtt in node)
                {
                    switch (nodeEnvAtt.Name)
                    {
                        case "capec:Environment_Title":
                            sEnvironmentTitle = CleaningCAPECString(nodeEnvAtt.InnerText);
                            
                            break;
                        case "capec:Environment_Description":
                            sEnvironmentDescription = CleaningCAPECString(nodeEnvAtt.InnerText);
                            
                            break;
                        default:
                            Console.WriteLine("ERROR: Missing code for nodeEnvAtt01 capec:Environment " + nodeEnvAtt.Name);
                            break;
                    }
                }

                ENVIRONMENT myenv = model.ENVIRONMENT.FirstOrDefault(o => o.CapecEnvironmentID == sEnvID);
                if (myenv == null)
                {
                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                    Console.WriteLine("DEBUG Adding new ENVIRONMENT " + sEnvID);

                    myenv = new ENVIRONMENT();
                    myenv.CapecEnvironmentID = sEnvID;
                    myenv.EnvironmentTitle = sEnvironmentTitle;
                    myenv.EnvironmentDescription = sEnvironmentDescription;
                    myenv.VocabularyID = iVocabularyCAPECID;
                    myenv.CreatedDate = DateTimeOffset.Now;
                    
                    model.ENVIRONMENT.Add(myenv);
                    
                }
                else
                {
                    //Update ENVIRONMENT
                    myenv.EnvironmentTitle = sEnvironmentTitle;
                    myenv.EnvironmentDescription = sEnvironmentDescription;
                }
                myenv.timestamp = DateTimeOffset.Now;
                model.SaveChanges();
            }

            #endregion Environment

            //FREE
            /*
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
            catch(Exception exFINALSAVE)
            {
                Console.WriteLine("Exception exFINALSAVE " + exFINALSAVE.Message + " " + exFINALSAVE.InnerException);
            }
            model.Dispose();
            model = null;
            */

            Console.WriteLine("DEBUG "+DateTimeOffset.Now);
            Console.WriteLine("DEBUG IMPORT CAPEC FINISHED");

            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
        }
    
        static string CleaningCAPECString(string sStringToClean)
        {

            //Cleaning
            sStringToClean = sStringToClean.Replace("<capec:Text>", "");
            sStringToClean = sStringToClean.Replace("</capec:Text>", "");
            sStringToClean = sStringToClean.Replace("<capec:text>", "");
            sStringToClean = sStringToClean.Replace("</capec:text>", "");
            //Remove CLRF
            sStringToClean = sStringToClean.Replace("\r\n", " ");
            sStringToClean = sStringToClean.Replace("\n", " ");
            sStringToClean = sStringToClean.Replace("\t", " "); //TAB
            while (sStringToClean.Contains("  "))
            {
                sStringToClean = sStringToClean.Replace("  ", " ");
            }
            return sStringToClean.Trim();
        }

        static void fAddOrganisation(string sOrganisationName)
        {
            //TODO

        }

    }
}
