using System;
using System.Net;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data;

using XORCISMModel;
using XTHREATModel;
using XINCIDENTModel;
using XMALWAREModel;
using XATTACKModel;

//using ICSharpCode.SharpZipLib.Zip;
using System.IO.Compression;

using System.Xml.Schema;
using System.Reflection;

namespace Import_stix
{
    static class Program
    {
        /// <summary>
        /// Copyright (C) 2014-2015 Jerome Athias
        /// Import the MITRE STIX default vocabularies enumeration values in an XORCISM database
        /// This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation; either version 2 of the License, or (at your option) any later version.
        /// 
        /// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
        /// 
        /// You should have received a copy of the GNU General Public License along with this program; if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
        /// </summary>
        [STAThread]
        static void Main()
        {
            string sSTIXVersion="1.1.1";    //HARDCODED TODO

            XORCISMEntities model= new XORCISMEntities();
            model.Configuration.AutoDetectChangesEnabled = false;
            model.Configuration.ValidateOnSaveEnabled = false;

            XTHREATEntities threat_model = new XTHREATEntities();
            threat_model.Configuration.AutoDetectChangesEnabled = false;
            threat_model.Configuration.ValidateOnSaveEnabled = false;


            XINCIDENTEntities incident_model = new XINCIDENTEntities();
            incident_model.Configuration.AutoDetectChangesEnabled = false;
            incident_model.Configuration.ValidateOnSaveEnabled = false;

            XMALWAREEntities malware_model = new XMALWAREEntities();
            malware_model.Configuration.AutoDetectChangesEnabled = false;
            malware_model.Configuration.ValidateOnSaveEnabled = false;

            XATTACKEntities attack_model = new XATTACKEntities();
            attack_model.Configuration.AutoDetectChangesEnabled = false;
            attack_model.Configuration.ValidateOnSaveEnabled = false;


            int iVocabularySTIXID = 0;  // 1;  //STIX
            #region vocabularySTIX
            try
            {
                iVocabularySTIXID = model.VOCABULARY.Where(o => o.VocabularyName == "STIX" && o.VocabularyVersion == sSTIXVersion).Select(o => o.VocabularyID).FirstOrDefault();
            }
            catch (Exception ex)
            {

            }
            if (iVocabularySTIXID <= 0)
            {
                VOCABULARY oVocabulary = new VOCABULARY();
                oVocabulary.CreatedDate = DateTimeOffset.Now;
                oVocabulary.VocabularyName = "STIX";
                oVocabulary.VocabularyVersion = sSTIXVersion;
                model.VOCABULARY.Add(oVocabulary);
                model.SaveChanges();
                iVocabularySTIXID = oVocabulary.VocabularyID;
                Console.WriteLine("DEBUG iVocabularySTIXID=" + iVocabularySTIXID);
            }
            #endregion vocabularySTIX


            try
            {
                
                //http://stix.mitre.org/XMLSchema/default_vocabularies/1.1.1/stix_default_vocabularies.xsd
                //TODO: download the file

                //Hardcoded
                XmlTextReader reader = new XmlTextReader(@"C:\nvdcve\stix_default_vocabularies.xsd");
                //TODO Review http://vsecurity.com/download/papers/XMLDTDEntityAttacks.pdf
                //TODO: Validate XSD
                reader.Read();

                //Note: we only import the last version of each Enumeration (the 1st found should be the last version)
                int iAssetTypeEnum = 0;
                int iAttackerInfrastructure = 0;
                int iSystemTypeEnum = 0;
                int iPackageIntentEnum = 0;
                int iHighMediumLowEnum = 0; //IMPORTANCE
                int iMalwareTypeEnum = 0;
                int iIndicatorTypeEnum = 0;
                int iCOAStageEnum = 0;
                int iCampaignStatusEnum = 0;
                int iIncidentStatusEnum = 0;
                //SECURITYCOMPROMISE vs INCIDENTCOMPROMISE
                int iSecurityCompromiseEnum = 0;

                int iDiscoveryMethodEnum = 0;   //vs INCIDENTDISCOVERYMETHOD

                int iAvailabilityLossTypeEnum = 0;
                //AVAILABILITYLOSSTYPE vs INCIDENTIMPACTAVAILABILITYVARIETY vs INCIDENTIMPACTLOSSVARIETY (VERIS)
                //LossDurationEnum  INCIDENTIMPACTAVAILABILITYLOSSDURATION
                int iOwnershipClassEnum = 0;
                
                int iManagementClassEnum = 0;
                //MANAGEMENT
                int iLocationClassEnum = 0;
                //ImpactQualificationEnum   INCIDENTIMPACTRATING? (VERIS)
                int iImpactQualificationEnum = 0;
                int iImpactRatingEnum = 0;
                int iInformationTypeEnum = 0;
                int iThreatActorTypeEnum = 0;
                int iMotivationEnum = 0;
                int iIntendedEffectEnum = 0;
                int iPlanningAndOperationalSupportEnum = 0;
                int iIncidentEffectEnum = 0;
                int iAttackerToolTypeEnum = 0;
                int iIncidentCategoryEnum = 0;
                int iLossPropertyEnum = 0;
                int iCourseOfActionTypeEnum = 0;
                int iThreatActorSophisticationEnum = 0;
                int iInformationSourceRoleEnum = 0;
                int iLossDurationEnum = 0;

                string readerNodeType = "";
                string readerAttributeName = "";
                string sCurrentEnum="";

                // If the node has value
                while (reader.Read())
                {
                    bool bEnumerationProcessed = false;
                    // Move to first element
                    reader.MoveToElement();

                    //Console.WriteLine("XmlTextReader Properties Test");
                    //Console.WriteLine("===================");
                    // Read this element's properties and display them on console
                    readerNodeType = reader.NodeType.ToString();
                    if (readerNodeType != "Whitespace")
                    {
                        string readerName = reader.Name;
                        if (readerName == "xs:simpleType")
                        {
                            if (reader.HasAttributes)
                            {
                                readerAttributeName = reader.GetAttribute("name");
                                if (readerAttributeName.Contains("Enum-"))
                                {
                                    Console.WriteLine("DEBUG readerAttributeName=" + readerAttributeName);

                                    //Get the EnumerationName and Version
                                    //ActionNameEnum-1.1
                                    string[] words = readerAttributeName.Split('-');
                                    string sEnumerationName = words[0];
                                    string sEnumerationVersion = words[1];

                                    //Check if we have this EnumerationVersion in the database
                                    //First check the Version
                                    XORCISMModel.VERSION oVersion;
                                    int iVersionID = 0;
                                    try
                                    {
                                        iVersionID = model.VERSION.FirstOrDefault(o => o.VersionValue == sEnumerationVersion).VersionID;
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                    if (iVersionID <= 0)
                                    {
                                        oVersion = new VERSION();
                                        oVersion.VersionValue = sEnumerationVersion;
                                        oVersion.VocabularyID = iVocabularySTIXID;
                                        model.VERSION.Add(oVersion);
                                        model.SaveChanges();
                                        iVersionID = oVersion.VersionID;
                                    }
                                    else
                                    {
                                        //Update VERSION
                                    }

                                    XORCISMModel.ENUMERATIONVERSION oEnumerationVersion;
                                    int iEnumerationVersionID = 0;
                                    try
                                    {
                                        iEnumerationVersionID = model.ENUMERATIONVERSION.FirstOrDefault(o => o.EnumerationName == sEnumerationName && o.VersionID == iVersionID).EnumerationVersionID;
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                    if (iEnumerationVersionID <= 0)
                                    {
                                        oEnumerationVersion = new ENUMERATIONVERSION();
                                        oEnumerationVersion.EnumerationName = sEnumerationName;
                                        oEnumerationVersion.VersionID = iVersionID;
                                        oEnumerationVersion.VocabularyID = iVocabularySTIXID;
                                        model.ENUMERATIONVERSION.Add(oEnumerationVersion);
                                        model.SaveChanges();
                                        iEnumerationVersionID = oEnumerationVersion.EnumerationVersionID;
                                    }
                                    else
                                    {
                                        //Update ENUMERATIONVERSION
                                    }

                                    #region identifyenumeration
                                    if (readerAttributeName.Contains("AssetTypeEnum-"))
                                    {
                                        iAssetTypeEnum++;
                                        sCurrentEnum = "AssetTypeEnum"; //ASSETVARIETY VERIS
                                        bEnumerationProcessed = true;
                                    }
                                    if (readerAttributeName.Contains("AttackerInfrastructureTypeEnum-"))
                                    {
                                        iAttackerInfrastructure++;
                                        sCurrentEnum = "AttackerInfrastructureTypeEnum";    //THREATACTORINFRASTRUCTURE
                                        bEnumerationProcessed = true;
                                    }
                                    if (readerAttributeName.Contains("SystemTypeEnum-"))
                                    {
                                        iSystemTypeEnum++;
                                        sCurrentEnum = "SystemTypeEnum";    //VERIS
                                        bEnumerationProcessed = true;
                                    }
                                    if (readerAttributeName.Contains("PackageIntentEnum-"))
                                    {
                                        iPackageIntentEnum++;
                                        sCurrentEnum = "PackageIntentEnum";
                                        bEnumerationProcessed = true;
                                    }
                                    if (readerAttributeName.Contains("MalwareTypeEnum-"))
                                    {
                                        iMalwareTypeEnum++;
                                        sCurrentEnum = "MalwareTypeEnum";
                                        bEnumerationProcessed = true;
                                    }
                                    if (readerAttributeName.Contains("IndicatorTypeEnum-"))
                                    {
                                        iIndicatorTypeEnum++;
                                        sCurrentEnum = "IndicatorTypeEnum";
                                        bEnumerationProcessed = true;
                                    }
                                    if (readerAttributeName.Contains("COAStageEnum-"))
                                    {
                                        iCOAStageEnum++;
                                        sCurrentEnum = "COAStageEnum";
                                        bEnumerationProcessed = true;
                                    }
                                    if (readerAttributeName.Contains("CampaignStatusEnum-"))
                                    {
                                        iCampaignStatusEnum++;
                                        sCurrentEnum = "CampaignStatusEnum";    //THREATCAMPAIGNSTATUS
                                        bEnumerationProcessed = true;
                                    }
                                    if (readerAttributeName.Contains("IncidentStatusEnum-"))
                                    {
                                        iIncidentStatusEnum++;
                                        sCurrentEnum = "IncidentStatusEnum";
                                        bEnumerationProcessed = true;
                                    }
                                    if (readerAttributeName.Contains("DiscoveryMethodEnum-"))
                                    {
                                        iDiscoveryMethodEnum++;
                                        sCurrentEnum = "DiscoveryMethodEnum";
                                        bEnumerationProcessed = true;
                                    }
                                    if (readerAttributeName.Contains("LocationClassEnum-"))
                                    {
                                        iLocationClassEnum++;
                                        sCurrentEnum = "LocationClassEnum"; //ASSETLOCATION
                                        bEnumerationProcessed = true;
                                    }
                                    if (readerAttributeName.Contains("ImpactQualificationEnum-"))
                                    {
                                        iImpactQualificationEnum++;
                                        sCurrentEnum = "ImpactQualificationEnum";
                                        bEnumerationProcessed = true;
                                    }
                                    if (readerAttributeName.Contains("ImpactRatingEnum-"))
                                    {
                                        iImpactRatingEnum++;
                                        sCurrentEnum = "ImpactRatingEnum";
                                        bEnumerationProcessed = true;
                                    }
                                    if (readerAttributeName.Contains("InformationTypeEnum-"))
                                    {
                                        iInformationTypeEnum++;
                                        sCurrentEnum = "InformationTypeEnum";
                                        bEnumerationProcessed = true;
                                    }
                                    if (readerAttributeName.Contains("ThreatActorTypeEnum-"))
                                    {
                                        iThreatActorTypeEnum++;
                                        sCurrentEnum = "ThreatActorTypeEnum";   //THREATACTORVARIETY    VERIS
                                        bEnumerationProcessed = true;
                                    }
                                    if (readerAttributeName.Contains("MotivationEnum-"))
                                    {
                                        iMotivationEnum++;
                                        sCurrentEnum = "MotivationEnum";    //THREATMOTIVE
                                        bEnumerationProcessed = true;
                                    }
                                    if (readerAttributeName.Contains("IntendedEffectEnum-"))
                                    {
                                        iIntendedEffectEnum++;
                                        sCurrentEnum = "IntendedEffectEnum";    //THREATINTENDEDEFFET
                                        bEnumerationProcessed = true;
                                    }
                                    if (readerAttributeName.Contains("PlanningAndOperationalSupportEnum-"))
                                    {
                                        iPlanningAndOperationalSupportEnum++;
                                        sCurrentEnum = "PlanningAndOperationalSupportEnum"; //THREATACTORPAOS
                                        bEnumerationProcessed = true;
                                    }
                                    if (readerAttributeName.Contains("IncidentEffectEnum-"))
                                    {
                                        iIncidentEffectEnum++;
                                        sCurrentEnum = "IncidentEffectEnum";
                                        bEnumerationProcessed = true;
                                    }
                                    if (readerAttributeName.Contains("AttackerToolTypeEnum-"))
                                    {
                                        iAttackerToolTypeEnum++;
                                        sCurrentEnum = "AttackerToolTypeEnum";  //ATTACKTOOLTYPE
                                        bEnumerationProcessed = true;
                                    }
                                    if (readerAttributeName.Contains("IncidentCategoryEnum-"))
                                    {
                                        iIncidentCategoryEnum++;
                                        sCurrentEnum = "IncidentCategoryEnum";
                                        bEnumerationProcessed = true;
                                    }
                                    if (readerAttributeName.Contains("LossPropertyEnum-"))
                                    {
                                        iLossPropertyEnum++;
                                        sCurrentEnum = "LossPropertyEnum";
                                        bEnumerationProcessed = true;
                                    }
                                    if (readerAttributeName.Contains("ThreatActorSophisticationEnum-"))
                                    {
                                        iThreatActorSophisticationEnum++;
                                        sCurrentEnum = "ThreatActorSophisticationEnum";
                                        bEnumerationProcessed = true;
                                    }
                                    if (readerAttributeName.Contains("InformationSourceRoleEnum-"))
                                    {
                                        iInformationSourceRoleEnum++;
                                        sCurrentEnum = "InformationSourceRoleEnum";
                                        bEnumerationProcessed = true;
                                    }
                                    if (readerAttributeName.Contains("HighMediumLowEnum-"))
                                    {
                                        iHighMediumLowEnum++;
                                        sCurrentEnum = "HighMediumLowEnum"; //IMPORTANCE
                                        bEnumerationProcessed = true;
                                    }
                                    if (readerAttributeName.Contains("CourseOfActionTypeEnum-"))
                                    {
                                        iCourseOfActionTypeEnum++;
                                        sCurrentEnum = "CourseOfActionTypeEnum";
                                        bEnumerationProcessed = true;
                                    }
                                    if (readerAttributeName.Contains("SecurityCompromiseEnum-"))
                                    {
                                        iSecurityCompromiseEnum++;
                                        sCurrentEnum = "SecurityCompromiseEnum";
                                        bEnumerationProcessed = true;
                                    }
                                    if (readerAttributeName.Contains("ManagementClassEnum-"))
                                    {
                                        iManagementClassEnum++;
                                        sCurrentEnum = "ManagementClassEnum";
                                        bEnumerationProcessed = true;
                                    }
                                    if (readerAttributeName.Contains("OwnershipClassEnum-"))
                                    {
                                        iOwnershipClassEnum++;
                                        sCurrentEnum = "OwnershipClassEnum";
                                        bEnumerationProcessed = true;
                                    }
                                    if (readerAttributeName.Contains("AvailabilityLossTypeEnum-"))
                                    {
                                        iAvailabilityLossTypeEnum++;
                                        sCurrentEnum = "AvailabilityLossTypeEnum";
                                        bEnumerationProcessed = true;
                                    }
                                    if (readerAttributeName.Contains("LossDurationEnum-"))
                                    {
                                        iLossDurationEnum++;
                                        sCurrentEnum = "LossDurationEnum";
                                        //bEnumerationProcessed = true; //TODO      TIMEUNIT?
                                    }
                                    #endregion identifyenumeration
                                }
                                if (!bEnumerationProcessed)
                                {
                                    Console.WriteLine("ERROR Missing Code for " + readerAttributeName);
                                    sCurrentEnum = "";
                                }
                            }
                        }

                        #region enumeration
                        //TODO Documentation
                        if (readerName == "xs:enumeration")
                        {
                            //Console.WriteLine("Name:" + reader.Name);
                            //Console.WriteLine("Base URI:" + reader.BaseURI);
                            //Console.WriteLine("Local Name:" + reader.LocalName);
                            //Console.WriteLine("Attribute Count:" + reader.AttributeCount.ToString());

                            //Console.WriteLine("Depth:" + reader.Depth.ToString());
                            //Console.WriteLine("Line Number:" + reader.LineNumber.ToString());
                            //Console.WriteLine("Node Type:" + reader.NodeType.ToString());     //Element
                            string readerValue = reader.Value.ToString();
                            //if (readerValue != ""){
                            //    Console.WriteLine("Attribute Value:" + reader.Value.ToString());
                            //}
                            //Console.WriteLine(reader.ReadInnerXml());
                            //Console.WriteLine(reader.ReadAttributeValue());
                            if (reader.HasAttributes)
                            {
                                string readerAttributeValue = reader.GetAttribute("value");
                                //Console.WriteLine(readerAttributeValue);
                                if (iAssetTypeEnum == 1 && sCurrentEnum == "AssetTypeEnum")
                                {
                                    #region AssetTypeEnum
                                    //Console.WriteLine(readerAttributeValue);
                                    //ASSETVARIETY  VERIS
                                    //VocabularyID=1    //STIX
                                    int iAssetVarietyID = 0;
                                    try
                                    {
                                        //Check if already exists in the database
                                        iAssetVarietyID = model.ASSETVARIETY.FirstOrDefault(o => o.AssetVarietyName == readerAttributeValue).AssetVarietyID;  // && o.VocabularyID == 1)
                                    }
                                    catch(Exception ex)
                                    {

                                    }
                                    //XORCISMModel.ASSETVARIETY tassetvariety = new ASSETVARIETY();
                                    //tassetvariety = model.ASSETVARIETY.FirstOrDefault(o => o.AssetVarietyName == readerAttributeValue && o.VocabularyID == 1);
                                    //if (tassetvariety == null)
                                    if (iAssetVarietyID<=0)
                                    {
                                        ASSETVARIETY tassetvariety = new ASSETVARIETY();
                                        tassetvariety.VocabularyID = iVocabularySTIXID;  //STIX
                                        tassetvariety.AssetVarietyName = readerAttributeValue;
                                        tassetvariety.CreatedDate = DateTimeOffset.Now;
                                        tassetvariety.timestamp = DateTimeOffset.Now;
                                        model.ASSETVARIETY.Add(tassetvariety);
                                        model.SaveChanges();
                                    }
                                    else
                                    {
                                        //Update ASSETVARIETY
                                    }
                                    #endregion AssetTypeEnum
                                }
                                else
                                {
                                    if (iAttackerInfrastructure == 1 && sCurrentEnum == "AttackerInfrastructure")
                                    {
                                        #region AttackInfrastructure
                                        //Console.WriteLine(readerAttributeValue);
                                        //THREATACTORINFRASTRUCTURE
                                        //VocabularyID=1    //STIX
                                        int iThreatActorInfrastructureID = 0;
                                        try
                                        {
                                            iThreatActorInfrastructureID = threat_model.THREATACTORINFRASTRUCTURE.FirstOrDefault(o => o.AttackerInfrastructureName == readerAttributeValue).ThreatActorInfrastructureID;   // && o.VocabularyID == 1);
                                        }
                                        catch(Exception ex)
                                        {

                                        }
                                        //XORCISMModel.THREATACTORINFRASTRUCTURE tactorinfra = new THREATACTORINFRASTRUCTURE();
                                        //tactorinfra = model.THREATACTORINFRASTRUCTURE.FirstOrDefault(o => o.AttackerInfrastructureName == readerAttributeValue && o.VocabularyID == 1);
                                        //if (tactorinfra == null)
                                        if (iThreatActorInfrastructureID<=0)
                                        {
                                            THREATACTORINFRASTRUCTURE tactorinfra = new THREATACTORINFRASTRUCTURE();
                                            tactorinfra.VocabularyID = iVocabularySTIXID;  //STIX
                                            tactorinfra.AttackerInfrastructureName = readerAttributeValue;
                                            tactorinfra.CreatedDate = DateTimeOffset.Now;
                                            tactorinfra.timestamp = DateTimeOffset.Now;
                                            threat_model.THREATACTORINFRASTRUCTURE.Add(tactorinfra);
                                            threat_model.SaveChanges();
                                        }
                                        else
                                        {
                                            //Update THREATACTORINFRASTRUCTURE
                                        }
                                        #endregion AttackInfrastructure
                                    }
                                    else
                                    {
                                        if (iSystemTypeEnum == 1 && sCurrentEnum == "SystemTypeEnum")
                                        {
                                            #region SystemTypeEnum
                                            //Console.WriteLine(readerAttributeValue);
                                            //SYSTEMTYPE
                                            //VocabularyID=1    //STIX
                                            int iSystemTypeID = 0;
                                            try
                                            {
                                                iSystemTypeID = model.SYSTEMTYPE.FirstOrDefault(o => o.SystemTypeName == readerAttributeValue).SystemTypeID;  // && o.VocabularyID == 1);
                                            }
                                            catch(Exception ex)
                                            {

                                            }
                                            //XORCISMModel.SYSTEMTYPE tsystemtype = new SYSTEMTYPE();
                                            //tsystemtype = model.SYSTEMTYPE.FirstOrDefault(o => o.SystemTypeName == readerAttributeValue && o.VocabularyID == 1);
                                            //if (tsystemtype == null)
                                            if (iSystemTypeID<=0)
                                            {
                                                SYSTEMTYPE tsystemtype = new SYSTEMTYPE();
                                                tsystemtype.VocabularyID = iVocabularySTIXID;  //STIX
                                                tsystemtype.SystemTypeName = readerAttributeValue;
                                                tsystemtype.CreatedDate = DateTimeOffset.Now;
                                                tsystemtype.timestamp = DateTimeOffset.Now;
                                                model.SYSTEMTYPE.Add(tsystemtype);
                                                model.SaveChanges();
                                            }
                                            else
                                            {
                                                //Update SYSTEMTYPE
                                            }
                                            #endregion SystemTypeEnum
                                        }
                                        else
                                        {
                                            if (iPackageIntentEnum == 1 && sCurrentEnum == "PackageIntentEnum")
                                            {
                                                #region PackageIntentEnum
                                                //Console.WriteLine(readerAttributeValue);
                                                int iPackageIntentID = 0;
                                                try
                                                {
                                                    iPackageIntentID = model.PACKAGEINTENT.FirstOrDefault(o => o.PackageIntentName == readerAttributeValue).PackageIntentID;  // && o.VocabularyID == 1);
                                                }
                                                catch (Exception ex)
                                                {

                                                }
                                                if (iPackageIntentID <= 0)
                                                {
                                                    PACKAGEINTENT oPackageIntent = new PACKAGEINTENT();
                                                    oPackageIntent.VocabularyID = iVocabularySTIXID;  //STIX
                                                    oPackageIntent.PackageIntentName = readerAttributeValue;
                                                    oPackageIntent.CreatedDate = DateTimeOffset.Now;
                                                    oPackageIntent.timestamp = DateTimeOffset.Now;
                                                    model.PACKAGEINTENT.Add(oPackageIntent);
                                                    model.SaveChanges();
                                                    //iPackageIntentID=
                                                }
                                                else
                                                {
                                                    //Update PACKAGEINTENT
                                                }
                                                #endregion PackageIntentEnum
                                            }
                                            else
                                            {
                                                if (iMalwareTypeEnum == 1 && sCurrentEnum == "MalwareTypeEnum")
                                                {
                                                    #region MalwareTypeEnum
                                                    //Console.WriteLine(readerAttributeValue);
                                                    int iMalwareTypeID = 0;
                                                    try
                                                    {
                                                        iMalwareTypeID = malware_model.MALWARETYPE.FirstOrDefault(o => o.MalwareTypeName == readerAttributeValue).MalwareTypeID;  // && o.VocabularyID == 1);
                                                    }
                                                    catch (Exception ex)
                                                    {

                                                    }
                                                    if (iMalwareTypeID <= 0)
                                                    {
                                                        MALWARETYPE oMalwareType = new MALWARETYPE();
                                                        oMalwareType.VocabularyID = iVocabularySTIXID;  //STIX
                                                        oMalwareType.MalwareTypeName = readerAttributeValue;
                                                        oMalwareType.CreatedDate = DateTimeOffset.Now;
                                                        oMalwareType.timestamp = DateTimeOffset.Now;
                                                        malware_model.MALWARETYPE.Add(oMalwareType);
                                                        malware_model.SaveChanges();
                                                        //iMalwareTypeID=
                                                    }
                                                    else
                                                    {
                                                        //Update MALWARETYPE
                                                    }
                                                    #endregion MalwareTypeEnum
                                                }
                                                else
                                                {
                                                    if (iIndicatorTypeEnum == 1 && sCurrentEnum == "IndicatorTypeEnum")
                                                    {
                                                        #region IndicatorTypeEnum
                                                        //Console.WriteLine(readerAttributeValue);
                                                        int iIndicatorTypeID = 0;
                                                        try
                                                        {
                                                            iIndicatorTypeID = model.INDICATORTYPE.FirstOrDefault(o => o.IndicatorTypeName == readerAttributeValue).IndicatorTypeID;  // && o.VocabularyID == 1);
                                                        }
                                                        catch (Exception ex)
                                                        {

                                                        }
                                                        if (iIndicatorTypeID <= 0)
                                                        {
                                                            INDICATORTYPE oMalwareType = new INDICATORTYPE();
                                                            oMalwareType.VocabularyID = iVocabularySTIXID;  //STIX
                                                            oMalwareType.IndicatorTypeName = readerAttributeValue;
                                                            oMalwareType.CreatedDate = DateTimeOffset.Now;
                                                            oMalwareType.timestamp = DateTimeOffset.Now;
                                                            model.INDICATORTYPE.Add(oMalwareType);
                                                            model.SaveChanges();
                                                            //iMalwareTypeID=
                                                        }
                                                        else
                                                        {
                                                            //Update INDICATORTYPE
                                                        }
                                                        #endregion IndicatorTypeEnum
                                                    }
                                                    else
                                                    {
                                                        if (iCampaignStatusEnum == 1 && sCurrentEnum == "CampaignStatusEnum")
                                                        {
                                                            #region CampaignStatusEnum
                                                            //Console.WriteLine(readerAttributeValue);
                                                            int iThreatCampaignStatusID = 0;
                                                            try
                                                            {
                                                                iThreatCampaignStatusID = threat_model.THREATCAMPAIGNSTATUS.FirstOrDefault(o => o.CampaignStatus == readerAttributeValue).ThreatCampaignStatusID;  // && o.VocabularyID == 1);
                                                            }
                                                            catch (Exception ex)
                                                            {

                                                            }
                                                            if (iThreatCampaignStatusID <= 0)
                                                            {
                                                                THREATCAMPAIGNSTATUS oThreatCampaignStatus = new THREATCAMPAIGNSTATUS();
                                                                oThreatCampaignStatus.VocabularyID = iVocabularySTIXID;  //STIX
                                                                oThreatCampaignStatus.CampaignStatus = readerAttributeValue;
                                                                oThreatCampaignStatus.CreatedDate = DateTimeOffset.Now;
                                                                oThreatCampaignStatus.timestamp = DateTimeOffset.Now;
                                                                threat_model.THREATCAMPAIGNSTATUS.Add(oThreatCampaignStatus);
                                                                threat_model.SaveChanges();
                                                                //iThreatCampaignStatusID=
                                                            }
                                                            else
                                                            {
                                                                //Update THREATCAMPAIGNSTATUS
                                                            }
                                                            #endregion CampaignStatusEnum
                                                        }
                                                        else
                                                        {
                                                            if (iIncidentStatusEnum == 1 && sCurrentEnum == "IncidentStatusEnum")
                                                            {
                                                                #region IncidentStatusEnum
                                                                //Console.WriteLine(readerAttributeValue);
                                                                int iIncidentStatusID = 0;
                                                                try
                                                                {
                                                                    iIncidentStatusID = incident_model.INCIDENTSTATUS.FirstOrDefault(o => o.IncidentStatusName == readerAttributeValue).IncidentStatusID;  // && o.VocabularyID == 1);
                                                                }
                                                                catch (Exception ex)
                                                                {

                                                                }
                                                                if (iIncidentStatusID <= 0)
                                                                {
                                                                    INCIDENTSTATUS oIncidentStatus = new INCIDENTSTATUS();
                                                                    oIncidentStatus.VocabularyID = iVocabularySTIXID;  //STIX
                                                                    oIncidentStatus.IncidentStatusName = readerAttributeValue;
                                                                    oIncidentStatus.CreatedDate = DateTimeOffset.Now;
                                                                    oIncidentStatus.timestamp = DateTimeOffset.Now;
                                                                    incident_model.INCIDENTSTATUS.Add(oIncidentStatus);
                                                                    incident_model.SaveChanges();
                                                                    //iIncidentStatusID=
                                                                }
                                                                else
                                                                {
                                                                    //Update INCIDENTSTATUS
                                                                }
                                                                #endregion IncidentStatusEnum
                                                            }
                                                            else
                                                            {
                                                                if (iDiscoveryMethodEnum == 1 && sCurrentEnum == "DiscoveryMethodEnum")
                                                                {
                                                                    #region DiscoveryMethodEnum
                                                                    //Console.WriteLine(readerAttributeValue);
                                                                    int iDiscoveryMethodID = 0;
                                                                    try
                                                                    {
                                                                        iDiscoveryMethodID = model.DISCOVERYMETHOD.FirstOrDefault(o => o.DiscoveryMethodName == readerAttributeValue).DiscoveryMethodID;  // && o.VocabularyID == 1);
                                                                    }
                                                                    catch (Exception ex)
                                                                    {

                                                                    }
                                                                    if (iDiscoveryMethodID <= 0)
                                                                    {
                                                                        DISCOVERYMETHOD oDiscoveryMethod = new DISCOVERYMETHOD();
                                                                        oDiscoveryMethod.VocabularyID = iVocabularySTIXID;  //STIX
                                                                        oDiscoveryMethod.DiscoveryMethodName = readerAttributeValue;
                                                                        oDiscoveryMethod.CreatedDate = DateTimeOffset.Now;
                                                                        oDiscoveryMethod.timestamp = DateTimeOffset.Now;
                                                                        model.DISCOVERYMETHOD.Add(oDiscoveryMethod);
                                                                        model.SaveChanges();
                                                                        //iDiscoveryMethodID=
                                                                    }
                                                                    else
                                                                    {
                                                                        //Update DISCOVERYMETHOD
                                                                    }
                                                                    #endregion DiscoveryMethodEnum
                                                                }
                                                                else
                                                                {
                                                                    if (iLocationClassEnum == 1 && sCurrentEnum == "LocationClassEnum")
                                                                    {
                                                                        #region LocationClassEnum
                                                                        //Console.WriteLine(readerAttributeValue);
                                                                        int iAssetLocationID = 0;
                                                                        try
                                                                        {
                                                                            iAssetLocationID = model.ASSETLOCATION.FirstOrDefault(o => o.AssetLocationType == readerAttributeValue).AssetLocationID;  // && o.VocabularyID == 1);
                                                                        }
                                                                        catch (Exception ex)
                                                                        {

                                                                        }
                                                                        if (iAssetLocationID <= 0)
                                                                        {
                                                                            ASSETLOCATION oAssetLocation = new ASSETLOCATION();
                                                                            oAssetLocation.VocabularyID = iVocabularySTIXID;  //STIX
                                                                            oAssetLocation.AssetLocationType = readerAttributeValue;
                                                                            oAssetLocation.CreatedDate = DateTimeOffset.Now;
                                                                            oAssetLocation.timestamp = DateTimeOffset.Now;
                                                                            model.ASSETLOCATION.Add(oAssetLocation);
                                                                            model.SaveChanges();
                                                                            //iAssetLocationID=
                                                                        }
                                                                        else
                                                                        {
                                                                            //Update ASSETLOCATION
                                                                        }
                                                                        #endregion LocationClassEnum
                                                                    }
                                                                    else
                                                                    {
                                                                        if (iImpactQualificationEnum == 1 && sCurrentEnum == "ImpactQualificationEnum")
                                                                        {
                                                                            #region ImpactQualificationEnum
                                                                            //Console.WriteLine(readerAttributeValue);
                                                                            int iImpactQualificationID = 0;
                                                                            try
                                                                            {
                                                                                iImpactQualificationID = model.IMPACTQUALIFICATION.FirstOrDefault(o => o.ImpactQualificationName == readerAttributeValue).ImpactQualificationID;  // && o.VocabularyID == 1);
                                                                            }
                                                                            catch (Exception ex)
                                                                            {

                                                                            }
                                                                            if (iImpactQualificationID <= 0)
                                                                            {
                                                                                IMPACTQUALIFICATION oImpactQualification = new IMPACTQUALIFICATION();
                                                                                oImpactQualification.VocabularyID = iVocabularySTIXID;  //STIX
                                                                                oImpactQualification.ImpactQualificationName = readerAttributeValue;
                                                                                oImpactQualification.CreatedDate = DateTimeOffset.Now;
                                                                                oImpactQualification.timestamp = DateTimeOffset.Now;
                                                                                model.IMPACTQUALIFICATION.Add(oImpactQualification);
                                                                                model.SaveChanges();
                                                                                //iImpactQualificationID=
                                                                            }
                                                                            else
                                                                            {
                                                                                //Update IMPACTQUALIFICATION
                                                                            }
                                                                            #endregion ImpactQualificationEnum
                                                                        }
                                                                        else
                                                                        {
                                                                            if (iImpactRatingEnum == 1 && sCurrentEnum == "ImpactRatingEnum")
                                                                            {
                                                                                #region ImpactRatingEnum
                                                                                //Console.WriteLine(readerAttributeValue);
                                                                                int iImpactRatingID = 0;
                                                                                try
                                                                                {
                                                                                    iImpactRatingID = model.IMPACTRATING.FirstOrDefault(o => o.ImpactRatingName == readerAttributeValue).ImpactRatingID;  // && o.VocabularyID == 1);
                                                                                }
                                                                                catch (Exception ex)
                                                                                {

                                                                                }
                                                                                if (iImpactRatingID <= 0)
                                                                                {
                                                                                    IMPACTRATING oImpactQualification = new IMPACTRATING();
                                                                                    oImpactQualification.VocabularyID = iVocabularySTIXID;  //STIX
                                                                                    oImpactQualification.ImpactRatingName = readerAttributeValue;
                                                                                    oImpactQualification.CreatedDate = DateTimeOffset.Now;
                                                                                    oImpactQualification.timestamp = DateTimeOffset.Now;
                                                                                    model.IMPACTRATING.Add(oImpactQualification);
                                                                                    model.SaveChanges();
                                                                                    //iImpactRatingID=
                                                                                }
                                                                                else
                                                                                {
                                                                                    //Update IMPACTRATING
                                                                                }
                                                                                #endregion ImpactRatingEnum
                                                                            }
                                                                            else
                                                                            {
                                                                                if (iInformationTypeEnum == 1 && sCurrentEnum == "InformationTypeEnum")
                                                                                {
                                                                                    #region InformationTypeEnum
                                                                                    //Console.WriteLine(readerAttributeValue);
                                                                                    int iInformationTypeID = 0;
                                                                                    try
                                                                                    {
                                                                                        iInformationTypeID = model.INFORMATIONTYPE.FirstOrDefault(o => o.InformationTypeName == readerAttributeValue).InformationTypeID;  // && o.VocabularyID == 1);
                                                                                    }
                                                                                    catch (Exception ex)
                                                                                    {

                                                                                    }
                                                                                    if (iInformationTypeID <= 0)
                                                                                    {
                                                                                        INFORMATIONTYPE oInformationType = new INFORMATIONTYPE();
                                                                                        oInformationType.VocabularyID = iVocabularySTIXID;  //STIX
                                                                                        oInformationType.InformationTypeName = readerAttributeValue;
                                                                                        oInformationType.CreatedDate = DateTimeOffset.Now;
                                                                                        oInformationType.timestamp = DateTimeOffset.Now;
                                                                                        model.INFORMATIONTYPE.Add(oInformationType);
                                                                                        model.SaveChanges();
                                                                                        //iInformationTypeID=
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        //Update INFORMATIONTYPE
                                                                                    }
                                                                                    #endregion InformationTypeEnum
                                                                                }
                                                                                else
                                                                                {
                                                                                    if (iThreatActorTypeEnum == 1 && sCurrentEnum == "ThreatActorTypeEnum")
                                                                                    {
                                                                                        #region ThreatActorTypeEnum
                                                                                        //Console.WriteLine(readerAttributeValue);
                                                                                        int iThreatActorVarietyID = 0;
                                                                                        //NOTE: See also THREATAGENTCATEGORY (OWASP)
                                                                                        try
                                                                                        {
                                                                                            //NOTE: comes from VERIS
                                                                                            iThreatActorVarietyID =threat_model.THREATACTORVARIETY.FirstOrDefault(o => o.ActorVariety == readerAttributeValue).ThreatActorVarietyID;  // && o.VocabularyID == 1);
                                                                                        }
                                                                                        catch (Exception ex)
                                                                                        {

                                                                                        }
                                                                                        if (iThreatActorVarietyID <= 0)
                                                                                        {
                                                                                            THREATACTORVARIETY oThreatActorType = new THREATACTORVARIETY();
                                                                                            oThreatActorType.VocabularyID = iVocabularySTIXID;  //STIX
                                                                                            oThreatActorType.ActorVariety = readerAttributeValue;
                                                                                            if (readerAttributeValue.ToLower().Contains("insid"))
                                                                                            {
                                                                                                //Insider Threat
                                                                                                oThreatActorType.InternalVariety = true;
                                                                                            }
                                                                                            oThreatActorType.CreatedDate = DateTimeOffset.Now;
                                                                                            oThreatActorType.timestamp = DateTimeOffset.Now;
                                                                                            threat_model.THREATACTORVARIETY.Add(oThreatActorType);
                                                                                            threat_model.SaveChanges();
                                                                                            //iThreatActorVarietyID=
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            //Update THREATACTORVARIETY
                                                                                        }
                                                                                        #endregion ThreatActorTypeEnum
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        if (iMotivationEnum == 1 && sCurrentEnum == "MotivationEnum")
                                                                                        {
                                                                                            #region MotivationEnum
                                                                                            //Console.WriteLine(readerAttributeValue);
                                                                                            int iThreatMotiveID = 0;
                                                                                            //NOTE: See also MOTIVATION (//TODO)
                                                                                            try
                                                                                            {
                                                                                                //NOTE: comes from VERIS
                                                                                                iThreatMotiveID = threat_model.THREATMOTIVE.FirstOrDefault(o => o.motive == readerAttributeValue).ThreatMotiveID;  // && o.VocabularyID == 1);
                                                                                            }
                                                                                            catch (Exception ex)
                                                                                            {

                                                                                            }
                                                                                            if (iThreatMotiveID <= 0)
                                                                                            {
                                                                                                THREATMOTIVE oThreatMotive = new THREATMOTIVE();
                                                                                                oThreatMotive.VocabularyID = iVocabularySTIXID;  //STIX
                                                                                                oThreatMotive.motive = readerAttributeValue;
                                                                                                oThreatMotive.CreatedDate = DateTimeOffset.Now;
                                                                                                oThreatMotive.timestamp = DateTimeOffset.Now;
                                                                                                threat_model.THREATMOTIVE.Add(oThreatMotive);
                                                                                                threat_model.SaveChanges();
                                                                                                //iThreatMotiveID=
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                //Update THREATMOTIVE
                                                                                            }
                                                                                            #endregion MotivationEnum
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            if (iIntendedEffectEnum == 1 && sCurrentEnum == "IntendedEffectEnum")
                                                                                            {
                                                                                                #region IntendedEffectEnum
                                                                                                //Console.WriteLine(readerAttributeValue);
                                                                                                int iThreatIntendedEffectID = 0;
                                                                                                try
                                                                                                {
                                                                                                    iThreatIntendedEffectID = threat_model.THREATINTENDEDEFFECT.FirstOrDefault(o => o.IntendedEffectName == readerAttributeValue).ThreatIntendedEffectID;  // && o.VocabularyID == 1);
                                                                                                }
                                                                                                catch (Exception ex)
                                                                                                {

                                                                                                }
                                                                                                if (iThreatIntendedEffectID <= 0)
                                                                                                {
                                                                                                    THREATINTENDEDEFFECT oIntendedEffect = new THREATINTENDEDEFFECT();
                                                                                                    oIntendedEffect.VocabularyID = iVocabularySTIXID;  //STIX
                                                                                                    oIntendedEffect.IntendedEffectName = readerAttributeValue;
                                                                                                    oIntendedEffect.CreatedDate = DateTimeOffset.Now;
                                                                                                    oIntendedEffect.timestamp = DateTimeOffset.Now;
                                                                                                    threat_model.THREATINTENDEDEFFECT.Add(oIntendedEffect);
                                                                                                    threat_model.SaveChanges();
                                                                                                    //iThreatIntendedEffectID=
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    //Update THREATINTENDEDEFFECT
                                                                                                }
                                                                                                #endregion IntendedEffectEnum
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                if (iPlanningAndOperationalSupportEnum == 1 && sCurrentEnum == "PlanningAndOperationalSupportEnum")
                                                                                                {
                                                                                                    #region PlanningAndOperationalSupportEnum
                                                                                                    //Console.WriteLine(readerAttributeValue);
                                                                                                    int iThreatActorPAOSID = 0;
                                                                                                    try
                                                                                                    {
                                                                                                        iThreatActorPAOSID = threat_model.THREATACTORPAOS.FirstOrDefault(o => o.PlanningAndOperationalSupport == readerAttributeValue).ThreatActorPAOSID;  // && o.VocabularyID == 1);
                                                                                                    }
                                                                                                    catch (Exception ex)
                                                                                                    {

                                                                                                    }
                                                                                                    if (iThreatActorPAOSID <= 0)
                                                                                                    {
                                                                                                        THREATACTORPAOS oThreatActorPAOS = new THREATACTORPAOS();
                                                                                                        oThreatActorPAOS.VocabularyID = iVocabularySTIXID;  //STIX
                                                                                                        oThreatActorPAOS.PlanningAndOperationalSupport = readerAttributeValue;
                                                                                                        oThreatActorPAOS.CreatedDate = DateTimeOffset.Now;
                                                                                                        oThreatActorPAOS.timestamp = DateTimeOffset.Now;
                                                                                                        threat_model.THREATACTORPAOS.Add(oThreatActorPAOS);
                                                                                                        threat_model.SaveChanges();
                                                                                                        //iThreatActorPAOSID=
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        //Update THREATACTORPAOS
                                                                                                    }
                                                                                                    #endregion PlanningAndOperationalSupportEnum
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    if (iIncidentEffectEnum == 1 && sCurrentEnum == "IncidentEffectEnum")
                                                                                                    {
                                                                                                        #region IncidentEffectEnum
                                                                                                        //Console.WriteLine(readerAttributeValue);
                                                                                                        int iIncidentEffectID = 0;
                                                                                                        try
                                                                                                        {
                                                                                                            iIncidentEffectID = incident_model.INCIDENTEFFECT.FirstOrDefault(o => o.PossibleEffect == readerAttributeValue).IncidentEffectID;  // && o.VocabularyID == 1);
                                                                                                        }
                                                                                                        catch (Exception ex)
                                                                                                        {

                                                                                                        }
                                                                                                        if (iIncidentEffectID <= 0)
                                                                                                        {
                                                                                                            INCIDENTEFFECT oIncidentEffect = new INCIDENTEFFECT();
                                                                                                            oIncidentEffect.VocabularyID = iVocabularySTIXID;  //STIX
                                                                                                            oIncidentEffect.PossibleEffect = readerAttributeValue;
                                                                                                            oIncidentEffect.CreatedDate = DateTimeOffset.Now;
                                                                                                            oIncidentEffect.timestamp = DateTimeOffset.Now;
                                                                                                            incident_model.INCIDENTEFFECT.Add(oIncidentEffect);
                                                                                                            incident_model.SaveChanges();
                                                                                                            //iIncidentEffectID=
                                                                                                        }
                                                                                                        else
                                                                                                        {
                                                                                                            //Update INCIDENTEFFECT
                                                                                                        }
                                                                                                        #endregion IncidentEffectEnum
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        if (iAttackerToolTypeEnum == 1 && sCurrentEnum == "AttackerToolTypeEnum")
                                                                                                        {
                                                                                                            #region AttackerToolTypeEnum
                                                                                                            //Console.WriteLine(readerAttributeValue);
                                                                                                            int iAttackToolTypeID = 0;
                                                                                                            try
                                                                                                            {
                                                                                                                iAttackToolTypeID = attack_model.ATTACKTOOLTYPE.FirstOrDefault(o => o.AttackToolTypeName == readerAttributeValue).AttackToolTypeID;  // && o.VocabularyID == 1);
                                                                                                            }
                                                                                                            catch (Exception ex)
                                                                                                            {

                                                                                                            }
                                                                                                            if (iAttackToolTypeID <= 0)
                                                                                                            {
                                                                                                                ATTACKTOOLTYPE oAttackToolType = new ATTACKTOOLTYPE();
                                                                                                                oAttackToolType.VocabularyID = iVocabularySTIXID;  //STIX
                                                                                                                oAttackToolType.AttackToolTypeName = readerAttributeValue;
                                                                                                                oAttackToolType.CreatedDate = DateTimeOffset.Now;
                                                                                                                oAttackToolType.timestamp = DateTimeOffset.Now;
                                                                                                                attack_model.ATTACKTOOLTYPE.Add(oAttackToolType);
                                                                                                                attack_model.SaveChanges();
                                                                                                                //iAttackToolTypeID=
                                                                                                            }
                                                                                                            else
                                                                                                            {
                                                                                                                //Update ATTACKTOOLTYPE
                                                                                                            }
                                                                                                            #endregion AttackerToolTypeEnum
                                                                                                        }
                                                                                                        else
                                                                                                        {
                                                                                                            if (iIncidentCategoryEnum == 1 && sCurrentEnum == "IncidentCategoryEnum")
                                                                                                            {
                                                                                                                #region IncidentCategoryEnum
                                                                                                                //Console.WriteLine(readerAttributeValue);
                                                                                                                int iIncidentCategoryID = 0;
                                                                                                                try
                                                                                                                {
                                                                                                                    iIncidentCategoryID = incident_model.INCIDENTCATEGORY.FirstOrDefault(o => o.IncidentCategoryName == readerAttributeValue).IncidentCategoryID;  // && o.VocabularyID == 1);
                                                                                                                }
                                                                                                                catch (Exception ex)
                                                                                                                {

                                                                                                                }
                                                                                                                if (iIncidentCategoryID <= 0)
                                                                                                                {
                                                                                                                    INCIDENTCATEGORY oIncidentCategory = new INCIDENTCATEGORY();
                                                                                                                    oIncidentCategory.VocabularyID = iVocabularySTIXID;  //STIX
                                                                                                                    oIncidentCategory.IncidentCategoryName = readerAttributeValue;
                                                                                                                    oIncidentCategory.CreatedDate = DateTimeOffset.Now;
                                                                                                                    oIncidentCategory.timestamp = DateTimeOffset.Now;
                                                                                                                    incident_model.INCIDENTCATEGORY.Add(oIncidentCategory);
                                                                                                                    incident_model.SaveChanges();
                                                                                                                    //iIncidentCategoryID=
                                                                                                                }
                                                                                                                else
                                                                                                                {
                                                                                                                    //Update INCIDENTCATEGORY
                                                                                                                }
                                                                                                                #endregion IncidentCategoryEnum
                                                                                                            }
                                                                                                            else
                                                                                                            {
                                                                                                                if (iLossPropertyEnum == 1 && sCurrentEnum == "LossPropertyEnum")
                                                                                                                {
                                                                                                                    #region LossPropertyEnum
                                                                                                                    //Console.WriteLine(readerAttributeValue);
                                                                                                                    int iLossPropertyID = 0;
                                                                                                                    try
                                                                                                                    {
                                                                                                                        iLossPropertyID = model.LOSSPROPERTY.FirstOrDefault(o => o.LossPropertyName == readerAttributeValue).LossPropertyID;  // && o.VocabularyID == 1);
                                                                                                                    }
                                                                                                                    catch (Exception ex)
                                                                                                                    {

                                                                                                                    }
                                                                                                                    if (iLossPropertyID <= 0)
                                                                                                                    {
                                                                                                                        LOSSPROPERTY oLossProperty = new LOSSPROPERTY();
                                                                                                                        oLossProperty.VocabularyID = iVocabularySTIXID;  //STIX
                                                                                                                        oLossProperty.LossPropertyName = readerAttributeValue;
                                                                                                                        oLossProperty.CreatedDate = DateTimeOffset.Now;
                                                                                                                        oLossProperty.timestamp = DateTimeOffset.Now;
                                                                                                                        model.LOSSPROPERTY.Add(oLossProperty);
                                                                                                                        model.SaveChanges();
                                                                                                                        //iLossPropertyID=
                                                                                                                    }
                                                                                                                    else
                                                                                                                    {
                                                                                                                        //Update LOSSPROPERTY
                                                                                                                    }
                                                                                                                    #endregion LossPropertyEnum
                                                                                                                }
                                                                                                                else
                                                                                                                {
                                                                                                                    if (iThreatActorSophisticationEnum == 1 && sCurrentEnum == "ThreatActorSophisticationEnum")
                                                                                                                    {
                                                                                                                        #region ThreatActorSophisticationEnum
                                                                                                                        //Console.WriteLine(readerAttributeValue);
                                                                                                                        int iThreatActorSophisticationID = 0;
                                                                                                                        try
                                                                                                                        {
                                                                                                                            iThreatActorSophisticationID = threat_model.THREATACTORSOPHISTICATION.FirstOrDefault(o => o.ThreatActorSophisticationName == readerAttributeValue).ThreatActorSophisticationID;  // && o.VocabularyID == 1);
                                                                                                                        }
                                                                                                                        catch (Exception ex)
                                                                                                                        {

                                                                                                                        }
                                                                                                                        if (iThreatActorSophisticationID <= 0)
                                                                                                                        {
                                                                                                                            THREATACTORSOPHISTICATION oThreatActorSophistication = new THREATACTORSOPHISTICATION();
                                                                                                                            oThreatActorSophistication.VocabularyID = iVocabularySTIXID;  //STIX
                                                                                                                            oThreatActorSophistication.ThreatActorSophisticationName = readerAttributeValue;
                                                                                                                            oThreatActorSophistication.CreatedDate = DateTimeOffset.Now;
                                                                                                                            oThreatActorSophistication.timestamp = DateTimeOffset.Now;
                                                                                                                            threat_model.THREATACTORSOPHISTICATION.Add(oThreatActorSophistication);
                                                                                                                            threat_model.SaveChanges();
                                                                                                                            //iThreatActorSophisticationID=
                                                                                                                        }
                                                                                                                        else
                                                                                                                        {
                                                                                                                            //Update THREATACTORSOPHISTICATION
                                                                                                                        }
                                                                                                                        #endregion ThreatActorSophisticationEnum
                                                                                                                    }
                                                                                                                    else
                                                                                                                    {
                                                                                                                        if (iInformationSourceRoleEnum == 1 && sCurrentEnum == "InformationSourceRoleEnum")
                                                                                                                        {
                                                                                                                            #region InformationSourceRoleEnum
                                                                                                                            //Console.WriteLine(readerAttributeValue);
                                                                                                                            int iInformationSourceRoleEnumID = 0;
                                                                                                                            try
                                                                                                                            {
                                                                                                                                iInformationSourceRoleEnumID = model.INFORMATIONSOURCEROLE.FirstOrDefault(o => o.InformationSourceRoleName == readerAttributeValue).InformationSourceRoleID;  // && o.VocabularyID == 1);
                                                                                                                            }
                                                                                                                            catch (Exception ex)
                                                                                                                            {

                                                                                                                            }
                                                                                                                            if (iInformationSourceRoleEnumID <= 0)
                                                                                                                            {
                                                                                                                                INFORMATIONSOURCEROLE oInformationSourceRole = new INFORMATIONSOURCEROLE();
                                                                                                                                oInformationSourceRole.VocabularyID = iVocabularySTIXID;  //STIX
                                                                                                                                oInformationSourceRole.InformationSourceRoleName = readerAttributeValue;
                                                                                                                                oInformationSourceRole.CreatedDate = DateTimeOffset.Now;
                                                                                                                                oInformationSourceRole.timestamp = DateTimeOffset.Now;
                                                                                                                                model.INFORMATIONSOURCEROLE.Add(oInformationSourceRole);
                                                                                                                                model.SaveChanges();
                                                                                                                                //iInformationSourceRoleEnumID=
                                                                                                                            }
                                                                                                                            else
                                                                                                                            {
                                                                                                                                //Update THREATACTORSOPHISTICATION
                                                                                                                            }
                                                                                                                            #endregion InformationSourceRoleEnum
                                                                                                                        }
                                                                                                                        else
                                                                                                                        {
                                                                                                                            if (iHighMediumLowEnum == 1 && sCurrentEnum == "HighMediumLowEnum")
                                                                                                                            {
                                                                                                                                #region HighMediumLowEnum
                                                                                                                                //Console.WriteLine(readerAttributeValue);
                                                                                                                                int iImportanceID = 0;
                                                                                                                                try
                                                                                                                                {
                                                                                                                                    iImportanceID = model.IMPORTANCE.FirstOrDefault(o => o.ImportanceLevel == readerAttributeValue).ImportanceID;  // && o.VocabularyID == 1);
                                                                                                                                }
                                                                                                                                catch (Exception ex)
                                                                                                                                {

                                                                                                                                }
                                                                                                                                if (iImportanceID <= 0)
                                                                                                                                {
                                                                                                                                    IMPORTANCE oImportance = new IMPORTANCE();
                                                                                                                                    oImportance.VocabularyID = iVocabularySTIXID;  //STIX
                                                                                                                                    oImportance.ImportanceLevel = readerAttributeValue;
                                                                                                                                    oImportance.CreatedDate = DateTimeOffset.Now;
                                                                                                                                    oImportance.timestamp = DateTimeOffset.Now;
                                                                                                                                    model.IMPORTANCE.Add(oImportance);
                                                                                                                                    model.SaveChanges();
                                                                                                                                    //iImportanceID=
                                                                                                                                }
                                                                                                                                else
                                                                                                                                {
                                                                                                                                    //Update IMPORTANCE
                                                                                                                                }
                                                                                                                                #endregion HighMediumLowEnum
                                                                                                                            }
                                                                                                                            else
                                                                                                                            {
                                                                                                                                if (iCourseOfActionTypeEnum == 1 && sCurrentEnum == "CourseOfActionTypeEnum")
                                                                                                                                {
                                                                                                                                    #region CourseOfActionTypeEnum
                                                                                                                                    //Console.WriteLine(readerAttributeValue);
                                                                                                                                    int iCourseOfActionTypeID = 0;
                                                                                                                                    try
                                                                                                                                    {
                                                                                                                                        iCourseOfActionTypeID = model.COURSEOFACTIONTYPE.FirstOrDefault(o => o.CourseOfActionTypeName == readerAttributeValue).CourseOfActionTypeID;  // && o.VocabularyID == 1);
                                                                                                                                    }
                                                                                                                                    catch (Exception ex)
                                                                                                                                    {

                                                                                                                                    }
                                                                                                                                    if (iCourseOfActionTypeID <= 0)
                                                                                                                                    {
                                                                                                                                        COURSEOFACTIONTYPE oCOAType = new COURSEOFACTIONTYPE();
                                                                                                                                        oCOAType.VocabularyID = iVocabularySTIXID;  //STIX
                                                                                                                                        oCOAType.CourseOfActionTypeName = readerAttributeValue;
                                                                                                                                        oCOAType.CreatedDate = DateTimeOffset.Now;
                                                                                                                                        oCOAType.timestamp = DateTimeOffset.Now;
                                                                                                                                        model.COURSEOFACTIONTYPE.Add(oCOAType);
                                                                                                                                        model.SaveChanges();
                                                                                                                                        //iCourseOfActionTypeID=
                                                                                                                                    }
                                                                                                                                    else
                                                                                                                                    {
                                                                                                                                        //Update COURSEOFACTIONTYPE
                                                                                                                                    }
                                                                                                                                    #endregion CourseOfActionTypeEnum
                                                                                                                                }
                                                                                                                                else
                                                                                                                                {
                                                                                                                                    if (iSecurityCompromiseEnum == 1 && sCurrentEnum == "SecurityCompromiseEnum")
                                                                                                                                    {
                                                                                                                                        #region SecurityCompromiseEnum
                                                                                                                                        //Console.WriteLine(readerAttributeValue);
                                                                                                                                        int iSecurityCompromiseID = 0;
                                                                                                                                        try
                                                                                                                                        {
                                                                                                                                            iSecurityCompromiseID = model.SECURITYCOMPROMISEENUM.FirstOrDefault(o => o.SecurityCompromiseEnumName == readerAttributeValue).SecurityCompromiseEnumID;  // && o.VocabularyID == 1);
                                                                                                                                        }
                                                                                                                                        catch (Exception ex)
                                                                                                                                        {

                                                                                                                                        }
                                                                                                                                        if (iSecurityCompromiseID <= 0)
                                                                                                                                        {
                                                                                                                                            //TODO Review vs INCIDENTCOMPROMISE
                                                                                                                                            SECURITYCOMPROMISEENUM oSecurityCompromise = new SECURITYCOMPROMISEENUM();
                                                                                                                                            oSecurityCompromise.VocabularyID = iVocabularySTIXID;  //STIX
                                                                                                                                            oSecurityCompromise.SecurityCompromiseEnumName = readerAttributeValue;
                                                                                                                                            oSecurityCompromise.CreatedDate = DateTimeOffset.Now;
                                                                                                                                            oSecurityCompromise.timestamp = DateTimeOffset.Now;
                                                                                                                                            model.SECURITYCOMPROMISEENUM.Add(oSecurityCompromise);
                                                                                                                                            model.SaveChanges();
                                                                                                                                            //iSecurityCompromiseID=
                                                                                                                                        }
                                                                                                                                        else
                                                                                                                                        {
                                                                                                                                            //Update SECURITYCOMPROMISE
                                                                                                                                        }
                                                                                                                                        #endregion SecurityCompromiseEnum
                                                                                                                                    }
                                                                                                                                    else
                                                                                                                                    {
                                                                                                                                        if (iManagementClassEnum == 1 && sCurrentEnum == "ManagementClassEnum")
                                                                                                                                        {
                                                                                                                                            #region ManagementClassEnum
                                                                                                                                            //Console.WriteLine(readerAttributeValue);
                                                                                                                                            int iManagementID = 0;
                                                                                                                                            try
                                                                                                                                            {
                                                                                                                                                iManagementID = model.MANAGEMENT.FirstOrDefault(o => o.ManagementName == readerAttributeValue).ManagementID;  // && o.VocabularyID == 1);
                                                                                                                                            }
                                                                                                                                            catch (Exception ex)
                                                                                                                                            {

                                                                                                                                            }
                                                                                                                                            if (iManagementID <= 0)
                                                                                                                                            {
                                                                                                                                                MANAGEMENT oManagement = new MANAGEMENT();
                                                                                                                                                oManagement.VocabularyID = iVocabularySTIXID;  //STIX
                                                                                                                                                oManagement.ManagementName = readerAttributeValue;
                                                                                                                                                oManagement.CreatedDate = DateTimeOffset.Now;
                                                                                                                                                oManagement.timestamp = DateTimeOffset.Now;
                                                                                                                                                model.MANAGEMENT.Add(oManagement);
                                                                                                                                                model.SaveChanges();
                                                                                                                                                //iManagementID=
                                                                                                                                            }
                                                                                                                                            else
                                                                                                                                            {
                                                                                                                                                //Update MANAGEMENT
                                                                                                                                            }
                                                                                                                                            #endregion ManagementClassEnum
                                                                                                                                        }
                                                                                                                                        else
                                                                                                                                        {
                                                                                                                                            if (iOwnershipClassEnum == 1 && sCurrentEnum == "OwnershipClassEnum")
                                                                                                                                            {
                                                                                                                                                #region OwnershipClassEnum
                                                                                                                                                //Console.WriteLine(readerAttributeValue);
                                                                                                                                                int iOwnershipID = 0;
                                                                                                                                                try
                                                                                                                                                {
                                                                                                                                                    iOwnershipID = model.OWNERSHIP.FirstOrDefault(o => o.OwnershipName == readerAttributeValue).OwnershipID;  // && o.VocabularyID == 1);
                                                                                                                                                }
                                                                                                                                                catch (Exception ex)
                                                                                                                                                {

                                                                                                                                                }
                                                                                                                                                if (iOwnershipID <= 0)
                                                                                                                                                {
                                                                                                                                                    OWNERSHIP oOwnership = new OWNERSHIP();
                                                                                                                                                    oOwnership.VocabularyID = iVocabularySTIXID;  //STIX
                                                                                                                                                    oOwnership.OwnershipName = readerAttributeValue;
                                                                                                                                                    oOwnership.CreatedDate = DateTimeOffset.Now;
                                                                                                                                                    oOwnership.timestamp = DateTimeOffset.Now;
                                                                                                                                                    model.OWNERSHIP.Add(oOwnership);
                                                                                                                                                    model.SaveChanges();
                                                                                                                                                    //iOwnershipID=
                                                                                                                                                }
                                                                                                                                                else
                                                                                                                                                {
                                                                                                                                                    //Update OWNERSHIP
                                                                                                                                                }
                                                                                                                                                #endregion OwnershipClassEnum
                                                                                                                                            }
                                                                                                                                            else
                                                                                                                                            {
                                                                                                                                                if (iAvailabilityLossTypeEnum == 1 && sCurrentEnum == "AvailabilityLossTypeEnum")
                                                                                                                                                {
                                                                                                                                                    #region AvailabilityLossTypeEnum
                                                                                                                                                    //Console.WriteLine(readerAttributeValue);
                                                                                                                                                    int iAvailabilityLossTypeID = 0;
                                                                                                                                                    try
                                                                                                                                                    {
                                                                                                                                                        iAvailabilityLossTypeID = model.AVAILABILITYLOSSTYPE.FirstOrDefault(o => o.AvailabilityLossTypeName == readerAttributeValue).AvailabilityLossTypeID;  // && o.VocabularyID == 1);
                                                                                                                                                    }
                                                                                                                                                    catch (Exception ex)
                                                                                                                                                    {

                                                                                                                                                    }
                                                                                                                                                    if (iAvailabilityLossTypeID <= 0)
                                                                                                                                                    {
                                                                                                                                                        AVAILABILITYLOSSTYPE oAVAILABILITYLOSSTYPE = new AVAILABILITYLOSSTYPE();
                                                                                                                                                        oAVAILABILITYLOSSTYPE.VocabularyID = iVocabularySTIXID;  //STIX
                                                                                                                                                        oAVAILABILITYLOSSTYPE.AvailabilityLossTypeName = readerAttributeValue;
                                                                                                                                                        oAVAILABILITYLOSSTYPE.CreatedDate = DateTimeOffset.Now;
                                                                                                                                                        oAVAILABILITYLOSSTYPE.timestamp = DateTimeOffset.Now;
                                                                                                                                                        model.AVAILABILITYLOSSTYPE.Add(oAVAILABILITYLOSSTYPE);
                                                                                                                                                        model.SaveChanges();
                                                                                                                                                        //iAvailabilityLossTypeID=
                                                                                                                                                    }
                                                                                                                                                    else
                                                                                                                                                    {
                                                                                                                                                        //Update AVAILABILITYLOSSTYPE
                                                                                                                                                    }
                                                                                                                                                    #endregion AvailabilityLossTypeEnum
                                                                                                                                                }
                                                                                                                                                else
                                                                                                                                                {
                                                                                                                                                    if (iLossDurationEnum == 1 && sCurrentEnum == "LossDurationEnum")
                                                                                                                                                    {
                                                                                                                                                        #region LossDurationEnum
                                                                                                                                                        //Console.WriteLine(readerAttributeValue);
                                                                                                                                                        
                                                                                                                                                        
                                                                                                                                                        //TODO

                                                                                                                                                        #endregion LossDurationEnum
                                                                                                                                                    }
                                                                                                                                                    else
                                                                                                                                                    {

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
                        #endregion enumeration
                    }
                }



                //XmlSchema myschema = XmlSchema.Read(reader, null);
//                XmlSchema myschema = XmlSchema.Read(reader, new ValidationEventHandler(ShowCompileError));
                
                //myschema.Compile(new ValidationEventHandler(ShowCompileError));
                //if (myschema.IsCompiled){
                
                    //DisplayObjects(myschema);
                //}
                //myschema.Write(Console.Out);

                //FREE
                model.Dispose();
                model = null;
            }
            catch (XmlSchemaException e)
            {
                Console.WriteLine("Exception LineNumber = {0}", e.LineNumber);
                Console.WriteLine("LinePosition = {0}", e.LinePosition);
                Console.WriteLine("Message = {0}", e.Message);
                Console.WriteLine("Source = {0}", e.Source);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message + " " + ex.InnerException);
            }
            
        

            //AssetTypeEnum-1.0
            //ASSETVARIETY
            //VocabularyID=1


            //AttackerInfrastructure
            //THREATACTORINFRASTRUCTURE
            //VocabularyID=1

            //SystemTypeEnum
            //SYSTEMTYPE
            //VocabularyID=1


            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
        }

        private static void DisplayObjects(object o) {
            DisplayObjects(o, "");
        }
        private static void DisplayObjects(object o, string indent) {
            Console.WriteLine("{0}{1}", indent, o.ToString());

            foreach (PropertyInfo property in o.GetType().GetProperties()) {
                if (property.PropertyType.FullName == "System.Xml.Schema.XmlSchemaObjectCollection") {
            
                    XmlSchemaObjectCollection childObjectCollection = (XmlSchemaObjectCollection) property.GetValue(o, null);
                
                    foreach (XmlSchemaObject schemaObject in childObjectCollection) {
                        DisplayObjects(schemaObject, indent + "\t");
                    }
                }
            }
        }
        private static void ShowCompileError(object sender, ValidationEventArgs e) {
            Console.WriteLine("Validation Error: {0}", e.Message);
        }
    }
    
}
