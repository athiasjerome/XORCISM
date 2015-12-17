using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using System.Xml;
using System.Xml.Schema;

using System.Linq;
using System.Data;
//using XORCISMModel;
using System.Text.RegularExpressions;

using System.Reflection;

namespace STIX
{
    class Program
    {
        /// <summary>
        /// Copyright (C) 2014-2015 Jerome Athias
        /// TEST/DEBUG ONLY tool to play with STIX XML files parsing (in the context of an XORCISM database)
        /// This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation; either version 2 of the License, or (at your option) any later version.
        /// 
        /// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
        /// 
        /// You should have received a copy of the GNU General Public License along with this program; if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
        /// </summary>
        static void Main(string[] args)
        {
            //TODO; Use args[0]
            //TODO: Validate the XML file with XSD
            /*
            string xsd_file = filename.Substring(0, filename.Length - 3) + "xsd";
            XmlSchema xsd = new XmlSchema();
            xsd.SourceUri = xsd_file;

            XmlSchemaSet ss = new XmlSchemaSet();
            ss.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);
            ss.Add(null, xsd_file);
            if (ss.Count > 0)
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ValidationType = ValidationType.Schema;
                settings.Schemas.Add(ss);
                settings.Schemas.Compile();
                settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);
                XmlTextReader r = new XmlTextReader(filename2);
                using (XmlReader reader = XmlReader.Create(r, settings))
                {
                    while (reader.Read())
                    {
                    }
                }
            }
            */

            try
            {
                /*
                int iVocabularySTIXID = 0;  // 1;  //STIX
                #region vocabularySTIX
                try
                {
                    iVocabularySTIXID = model.VOCABULARY.Where(o => o.VocabularyName == "STIX").Select(o => o.VocabularyID).FirstOrDefault();
                }
                catch (Exception ex)
                {

                }
                if (iVocabularySTIXID <= 0)
                {
                    VOCABULARY oVocabulary = new VOCABULARY();
                    oVocabulary.CreatedDate = DateTimeOffset.Now;
                    oVocabulary.VocabularyName = "STIX";
                    model.VOCABULARY.Add(oVocabulary);
                    model.SaveChanges();
                    iVocabularySTIXID = oVocabulary.VocabularyID;
                    Console.WriteLine("DEBUG iVocabularySTIXID=" + iVocabularySTIXID);
                }
                #endregion vocabularySTIX
                */


                //XmlTextReader readerXML = new XmlTextReader(@"C:\nvdcve\STIX_Indicator_Snort.xml");   //TODO: Hardcoded
                // Set the reader settings.
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreProcessingInstructions = true;
                settings.IgnoreWhitespace = true;
                settings.ProhibitDtd = false;      // Must set this to read DTDs
                //TODO Review http://vsecurity.com/download/papers/XMLDTDEntityAttacks.pdf
                //To validate XSD
                //settings.Schemas.Add(null, xsdFilePath);
                settings.ValidationType = ValidationType.Schema;
                //settings.ValidationEventHandler += new System.Xml.Schema.ValidationEventHandler(settings_ValidationEventHandler);


                string sInputFilePath = string.Empty;
                //https://stix.mitre.org/language/version1.0.1/samples.html
                //HARDCODED
                //sInputFilePath = @"C:\nvdcve\STIX_Indicator_Snort.xml";
                //sInputFilePath = @"C:\nvdcve\STIX_Malware_Sample.xml";
                sInputFilePath = @"C:\nvdcve\STIX_Domain_Watchlist.xml";

                using (XmlReader readerXML = XmlReader.Create(@sInputFilePath, settings))   //TODO: Hardcoded
                {
                    string readerNodeType = string.Empty;
                    string readerName = string.Empty;
                    string readerAttributeName = string.Empty;
                    string readerValue = string.Empty;
                    int iNodeLevel = 0;
                    string sNodeLevelText = "";
                    string sCurrentNodeLevelText = "";
                    string sCyboxVersion = string.Empty;
                    string sSTIXPackageVersion = string.Empty;
                    string sSTIXPackageID = string.Empty;
                    string sSTIXPackageTimestamp = string.Empty;

                    while (readerXML.Read())
                    {
                        iNodeLevel++;
                        //Console.WriteLine("DEBUG sNodeLevelText=" + sNodeLevelText);
                        switch (readerXML.NodeType)
                        {
                            case XmlNodeType.Element: // The node is an element.
                                readerName = readerXML.Name;
                                Console.WriteLine("DEBUG readerName=" + readerName);

                                if (iNodeLevel == 1)
                                {
                                    #region stixpackage
                                    if (readerName != "stix:STIX_Package")   //Note: case sensitive
                                    {
                                        Console.WriteLine(DateTimeOffset.Now.ToString());
                                        Console.WriteLine("ERROR Not a valid STIX_Package");
                                        return;
                                    }
                                    else
                                    {
                                        #region stixversion
                                        try
                                        {
                                            sSTIXPackageVersion=readerXML.GetAttribute("version");
                                            switch(sSTIXPackageVersion)
                                            {
                                                case "1.1.1":
                                                //case "1.0.1":
                                                    Console.WriteLine("DEBUG STIX_Package version " + sSTIXPackageVersion);
                                                    break;
                                                default:
                                                    Console.WriteLine("ERROR STIX_Package version " + sSTIXPackageVersion + " not supported.");
                                                    //return;
                                                    break;
                                            }
                                        }
                                        catch(Exception exSTIXPackageVersion)
                                        {
                                            Console.WriteLine(DateTimeOffset.Now.ToString());
                                            Console.WriteLine("Exception exSTIXPackageVersion " + exSTIXPackageVersion.Message + " " + exSTIXPackageVersion.InnerException);
                                            return;
                                        }
                                        #endregion stixversion

                                        #region stixpackageid
                                        try
                                        {
                                            sSTIXPackageID = readerXML.GetAttribute("id");
                                            Console.WriteLine("DEBUG STIX_Package id " + sSTIXPackageID);
                                        }
                                        catch (Exception exSTIXPackageID)
                                        {
                                            Console.WriteLine(DateTimeOffset.Now.ToString());
                                            Console.WriteLine("Exception exSTIXPackageID " + exSTIXPackageID.Message + " " + exSTIXPackageID.InnerException);
                                            return;
                                        }
                                        #endregion stixpackageid

                                        #region stixpackagetimestamp
                                        try
                                        {
                                            sSTIXPackageTimestamp = readerXML.GetAttribute("timestamp");
                                            Console.WriteLine("DEBUG STIX_Package timestamp " + sSTIXPackageTimestamp);
                                        }
                                        catch (Exception exSTIXPackageTimestamp)
                                        {
                                            Console.WriteLine(DateTimeOffset.Now.ToString());
                                            Console.WriteLine("Exception exSTIXPackageTimestamp " + exSTIXPackageTimestamp.Message + " " + exSTIXPackageTimestamp.InnerException);
                                            return;
                                        }
                                        #endregion stixpackagetimestamp
                                    }
                                    #endregion stixpackage
                                }

                                //**********************************************************************
                                #region readerName
                                switch (readerName)
                                {
                                    case "stix:STIX_Header":
                                        //if (readerName == "stix:STIX_Header")   //Note: case sensitive
                                        //{
                                        if (iNodeLevel != 2)
                                        {
                                            Console.WriteLine(DateTimeOffset.Now.ToString());
                                            Console.WriteLine("ERROR STIX_Header in a wrong position");
                                            return;
                                        }
                                        else
                                        {
                                            sNodeLevelText = "STIX_Header";
                                            Console.WriteLine("DEBUG sNodeLevelText=" + sNodeLevelText);
                                            Console.WriteLine("********************************************************************");
                                        }
                                        //}
                                        break;

                                    case "stix:Title":
                                        //**********************************************************************
                                        //if (readerName == "stix:Title")
                                        //{
                                        //Example SNORT Indicator
                                        //Example STIX document for sharing a Malware Sample
                                            if(sNodeLevelText == "STIX_Header")
                                            {
                                                //readerValue = readerXML.ReadElementContentAsString();
                                                //Console.WriteLine("DEBUG Title=" + readerValue);
                                            }
                                            else
                                            {
                                                Console.WriteLine("ERROR Missing code for stix:Title");
                                            }

                                        //}
                                        break;

                                    case "stix:Package_Intent":
                                        //**********************************************************************
                                        Console.WriteLine("DEBUG stix:Package_Intent");
                                        #region packageintent
                                        //if (readerName == "stix:Package_Intent")
                                        //{
                                        if (sNodeLevelText != "STIX_Header")
                                        {
                                            Console.WriteLine(DateTimeOffset.Now.ToString());
                                            Console.WriteLine("ERROR Package_Intent not in STIX_Header");
                                            return;
                                        }
                                        else
                                        {
                                            try
                                            {
                                                if (readerXML.GetAttribute("xsi:type") != "stixVocabs:PackageIntentVocab-1.0")  //Hardcoded
                                                {
                                                    //TODO: Review (new version)
                                                    Console.WriteLine(DateTimeOffset.Now.ToString());
                                                    Console.WriteLine("ERROR Package_Intent type not supported");
                                                    return;
                                                }
                                                //Console.WriteLine(readerXML.ReadElementContentAsString());
                                            }
                                            catch (Exception exPackage_IntentType)
                                            {
                                                Console.WriteLine(DateTimeOffset.Now.ToString());
                                                Console.WriteLine("Exception exPackage_IntentType " + exPackage_IntentType.Message + " " + exPackage_IntentType.InnerException);
                                                return;
                                            }

                                            try
                                            {
                                                readerValue = readerXML.ReadElementContentAsString();
                                                Console.WriteLine("DEBUG Package_Intent=" + readerValue);  //Indicators - Network Activity
                                            }
                                            catch (Exception exPackage_IntentReadElementContentAsString)
                                            {
                                                Console.WriteLine(DateTimeOffset.Now.ToString());
                                                Console.WriteLine("Exception exPackage_IntentReadElementContentAsString " + exPackage_IntentReadElementContentAsString.Message + " " + exPackage_IntentReadElementContentAsString.InnerException);
                                                return;
                                            }
                                        }
                                        //}
                                        #endregion packageintent
                                        break;

                                    case "stix:Description":
                                        if (sNodeLevelText == "STIX_Header")
                                        {
                                            readerValue = readerXML.ReadElementContentAsString();
                                            Console.WriteLine("DEBUG Description=" + readerValue);
                                        }
                                        else
                                        {
                                            Console.WriteLine("ERROR Missing code for stix:Title");
                                        }
                                        break;

                                    case "stix:Observables":
                                        //**********************************************************************
                                        #region observables
                                        //if (readerName == "stix:Observables")
                                        //{
                                        sNodeLevelText = "Observables";
                                        Console.WriteLine("********************************************************************");
                                        
                                        #region cyboxversion
                                        try
                                        {
                                            sCyboxVersion = readerXML.GetAttribute("cybox_major_version");
                                            try
                                            {
                                                sCyboxVersion += "." + readerXML.GetAttribute("cybox_minor_version");

                                            }
                                            catch (Exception exCyboxMinorVersion)
                                            {
                                                Console.WriteLine(DateTimeOffset.Now.ToString());
                                                Console.WriteLine("Exception exCyboxMinorVersion " + exCyboxMinorVersion.Message + " " + exCyboxMinorVersion.InnerException);
                                                //return; //TODO?
                                            }
                                        }
                                        catch (Exception exCyboxMajorVersion)
                                        {
                                            Console.WriteLine(DateTimeOffset.Now.ToString());
                                            Console.WriteLine("Exception exCyboxMajorVersion " + exCyboxMajorVersion.Message + " " + exCyboxMajorVersion.InnerException);
                                            //return; //TODO?
                                        }
                                        #endregion cyboxversion
                                        //}
                                        #endregion observables
                                        break;

                                    case "cybox:Observable":
                                        #region observable
                                        //if (readerName == "cybox:Observable")
                                        //{
                                        if (sNodeLevelText != "Observables")
                                        {
                                            Console.WriteLine(DateTimeOffset.Now.ToString());
                                            Console.WriteLine("ERROR Observable not in Observables");
                                            return;
                                        }
                                        else
                                        {
                                            //fParseCyboxObservable()
                                            try
                                            {
                                                string sObservableID = readerXML.GetAttribute("id");
                                                //example:Observable-3845653a-242a-4821-9f4d-31737a6e3ddd
                                                Console.WriteLine("DEBUG Observable ID " + sObservableID);
                                                //Console.WriteLine(readerXML.ReadElementContentAsString());

                                            }
                                            catch (Exception exObservableID)
                                            {
                                                Console.WriteLine(DateTimeOffset.Now.ToString());
                                                Console.WriteLine("Exception exObservableID " + exObservableID.Message + " " + exObservableID.InnerException);
                                                return;
                                            }
                                        }
                                        //}
                                        #endregion observable
                                        break;

                                    case "cybox:Object":
                                        #region object
                                        //if (readerName == "cybox:Object")
                                        //{
                                        /*
                                        if (sNodeLevelText != "Observables")    //TODO review?
                                        {
                                            //could be in indicator:Observable
                                            Console.WriteLine(DateTimeOffset.Now.ToString());
                                            Console.WriteLine("ERROR Object not in Observables");
                                            return;
                                        }
                                        else
                                        {
                                        */
                                            //fParseCyboxObject()
                                            try
                                            {
                                                string sObjectID = readerXML.GetAttribute("id");
                                                //example:Object-eb3402ee-34ac-40de-bc6d-10528eda3774
                                                Console.WriteLine("DEBUG Object ID " + sObjectID);
                                                //Console.WriteLine(readerXML.ReadElementContentAsString());

                                            }
                                            catch (Exception exObjectID)
                                            {
                                                Console.WriteLine(DateTimeOffset.Now.ToString());
                                                Console.WriteLine("Exception exObjectID " + exObjectID.Message + " " + exObjectID.InnerException);
                                                return;
                                            }
                                        //}
                                        //}
                                        #endregion object
                                        break;

                                    case "cybox:Properties":
                                        #region cyboxproperties
                                        //if (readerName == "cybox:Properties")
                                        //{
                                        /*
                                        if (sNodeLevelText != "Observables")    //TODO review?
                                        {
                                            Console.WriteLine(DateTimeOffset.Now.ToString());
                                            Console.WriteLine("ERROR Cybox Properties not in Observables");
                                            return;
                                        }
                                        else
                                        {
                                        */
                                            //fParseCyboxProperties()
                                            string sPropertiesType = string.Empty;
                                            try
                                            {
                                                sPropertiesType = readerXML.GetAttribute("xsi:type");
                                                //ArtifactObj:ArtifactObjectType
                                                Console.WriteLine("DEBUG Cybox Properties type " + sPropertiesType);
                                                //Console.WriteLine(readerXML.ReadElementContentAsString());

                                            }
                                            catch (Exception exPropertiesType)
                                            {
                                                Console.WriteLine(DateTimeOffset.Now.ToString());
                                                Console.WriteLine("Exception exPropertiesType " + exPropertiesType.Message + " " + exPropertiesType.InnerException);
                                                return;
                                            }

                                            //Note: if exception before, we returned
                                            switch (sPropertiesType)
                                            {
                                                case "ArtifactObj:ArtifactObjectType":
                                                    #region artifactobject
                                                    sNodeLevelText = "ArtifactObj:ArtifactObjectType";
                                                    Console.WriteLine("********************************************************************");
                                                    //TODO: list all the attributes and switch
                                                    try
                                                    {
                                                        string sContentType = readerXML.GetAttribute("content_type");
                                                        //application/zip
                                                        Console.WriteLine("DEBUG ArtifactObjectType content type " + sContentType);
                                                        //Console.WriteLine(readerXML.ReadElementContentAsString());
                                                        //TODO: verify that the content-type is correct

                                                    }
                                                    catch (Exception exContentType)
                                                    {
                                                        Console.WriteLine(DateTimeOffset.Now.ToString());
                                                        Console.WriteLine("Exception exContentType " + exContentType.Message + " " + exContentType.InnerException);
                                                        return;
                                                    }
                                                    #endregion artifactobject
                                                    break;

                                                case "FileObj:FileObjectType":
                                                    #region fileobject
                                                    sCurrentNodeLevelText = "FileObj:FileObjectType";
                                                    Console.WriteLine("********************************************************************");
                                                    //TODO: list all the attributes and switch
                                                    
                                                    #endregion fileobject
                                                    break;

                                                case "DomainNameObj:DomainNameObjectType":
                                                    //<cybox:Properties xsi:type="DomainNameObj:DomainNameObjectType" type="FQDN">
                                                    sCurrentNodeLevelText = "DomainNameObj:DomainNameObjectType";
                                                    Console.WriteLine("********************************************************************");
                                                    //TODO

                                                    break;

                                                default:
                                                    Console.WriteLine("ERROR Missing code for sPropertiesType " + sPropertiesType);
                                                    break;
                                            }

                                            //Review the Attributes of the cybox:Properties
                                            //start at 1 because we got the xsi:type already
                                            for (int iAtt = 1; iAtt < readerXML.AttributeCount; iAtt++)
                                            {
                                                readerXML.MoveToAttribute(iAtt);
                                                Console.WriteLine("DEBUG cybox:Properties AttributeName=" + readerXML.Name);
                                                Console.WriteLine("DEBUG cybox:Properties AttributeValue=" + readerXML.Value);
                                                switch (readerXML.Name)
                                                {
                                                    //type

                                                    default:
                                                        Console.WriteLine("ERROR Missing code for cybox:Properties attribute " + readerXML.Name);
                                                        break;
                                                }
                                            }
                                            
                                        //}
                                        //}
                                        #endregion cyboxproperties
                                        break;

                                    case "ArtifactObj:Packaging":
                                        #region artifactobjpackaging
                                        //if (readerName == "ArtifactObj:Packaging")
                                        //{
                                        if (sNodeLevelText != "ArtifactObj:ArtifactObjectType")  //"Observables")    //TODO review?
                                        {
                                            Console.WriteLine(DateTimeOffset.Now.ToString());
                                            Console.WriteLine("ERROR ArtifactObj Packaging not in Observables");
                                            return;
                                        }
                                        else
                                        {
                                            sCurrentNodeLevelText = "Packaging";
                                            //Review the Attributes of the Packaging
                                            #region packagingattributes
                                            for (int iAtt = 0; iAtt < readerXML.AttributeCount; iAtt++)
                                            {
                                                readerXML.MoveToAttribute(iAtt);
                                                Console.WriteLine("DEBUG AttributeName=" + readerXML.Name);
                                                Console.WriteLine("DEBUG AttributeValue=" + readerXML.Value);
                                                switch (readerXML.Name)
                                                {
                                                    case "is_encrypted":    //true
                                                        if (readerXML.Value != "true" && readerXML.Value != "false")
                                                        {
                                                            Console.WriteLine(DateTimeOffset.Now.ToString());
                                                            Console.WriteLine("ERROR Incorrect Packaging value for the boolean is_encrypted: " + readerXML.Value);
                                                            return;
                                                        }
                                                        else
                                                        {
                                                            Console.WriteLine("DEBUG Packaging is_encrypted: " + readerXML.Value);
                                                        }
                                                        break;
                                                    case "is_compressed":
                                                        if (readerXML.Value != "true" && readerXML.Value != "false")
                                                        {
                                                            Console.WriteLine(DateTimeOffset.Now.ToString());
                                                            Console.WriteLine("ERROR Incorrect Packaging value for the boolean is_compressed: " + readerXML.Value);
                                                            return;
                                                        }
                                                        else
                                                        {
                                                            Console.WriteLine("DEBUG Packaging is_compressed: " + readerXML.Value);
                                                        }
                                                        break;

                                                    default:
                                                        Console.WriteLine("ERROR Missing code for Packaging attribute " + readerXML.Name);
                                                        break;
                                                }
                                            }
                                            #endregion packagingattributes
                                        }
                                        //}
                                        #endregion artifactobjpackaging
                                        break;

                                    case "ArtifactObj:Encoding":
                                        #region artifactobjencoding
                                        //if (readerName == "ArtifactObj:Encoding")
                                        //{
                                        if (sCurrentNodeLevelText != "Packaging")  //"Observables")    //TODO review? Packaging
                                        {
                                            Console.WriteLine(DateTimeOffset.Now.ToString());
                                            Console.WriteLine("ERROR ArtifactObj Encoding not in Packaging");
                                            return;
                                        }
                                        else
                                        {
                                            //Review the Attributes of the Encoding
                                            #region encodingattributes
                                            for (int iAtt = 0; iAtt < readerXML.AttributeCount; iAtt++)
                                            {
                                                readerXML.MoveToAttribute(iAtt);
                                                Console.WriteLine("DEBUG AttributeName=" + readerXML.Name);
                                                Console.WriteLine("DEBUG AttributeValue=" + readerXML.Value);
                                                switch (readerXML.Name)
                                                {
                                                    case "algorithm":    //Base64   //Note: case sensitive
                                                        //TODO: verify that the algorithm is correct
                                                        Console.WriteLine("DEBUG Encoding algorithm: " + readerXML.Value);

                                                        break;

                                                    default:
                                                        Console.WriteLine("ERROR Missing code for Encoding attribute " + readerXML.Name);
                                                        //TODO
                                                        //character_set
                                                        //custom_character_set_ref
                                                        break;
                                                }
                                            }
                                            #endregion encodingattributes
                                        }
                                        //}
                                        #endregion artifactobjencoding
                                        break;

                                    case "ArtifactObj:Encryption":
                                        #region artifactobjencryption
                                        //if (readerName == "ArtifactObj:Encryption")
                                        //{
                                        if (sCurrentNodeLevelText != "Packaging")  //"Observables")    //TODO review? Packaging
                                        {
                                            Console.WriteLine(DateTimeOffset.Now.ToString());
                                            Console.WriteLine("ERROR ArtifactObj Encryption not in Packaging");
                                            return;
                                        }
                                        else
                                        {
                                            //Review the Attributes of the Encryption
                                            #region encryptionattributes
                                            for (int iAtt = 0; iAtt < readerXML.AttributeCount; iAtt++)
                                            {
                                                readerXML.MoveToAttribute(iAtt);
                                                Console.WriteLine("DEBUG AttributeName=" + readerXML.Name);
                                                Console.WriteLine("DEBUG AttributeValue=" + readerXML.Value);
                                                switch (readerXML.Name)
                                                {
                                                    case "encryption_mechanism":    //PasswordProtected   //Note: case sensitive
                                                        //TODO: verify that the encryption_mechanism is correct
                                                        Console.WriteLine("DEBUG Encryption encryption_mechanism: " + readerXML.Value);

                                                        break;
                                                    case "encryption_key":    //test
                                                        Console.WriteLine("DEBUG Encryption encryption_key: " + readerXML.Value);

                                                        break;
                                                    default:
                                                        Console.WriteLine("ERROR Missing code for Encryption attribute " + readerXML.Name);
                                                        //TODO
                                                        //encryption_mechanism_ref
                                                        //encryption_key_ref
                                                        break;
                                                }
                                            }
                                            #endregion encryptionattributes
                                        }
                                        //}
                                        #endregion artifactobjencryption
                                        break;

                                    case "ArtifactObj:Raw_Artifact":
                                        #region rawartifact
                                        /*
                                        <ArtifactObj:Raw_Artifact datatype="string">
                                        */
                                        if (sNodeLevelText != "ArtifactObj:ArtifactObjectType")  //"Observables")    //TODO review?
                                        {
                                            Console.WriteLine(DateTimeOffset.Now.ToString());
                                            Console.WriteLine("ERROR ArtifactObj Raw_Artifact not in ArtifactObjectType");
                                            return;
                                        }
                                        else
                                        {
                                            //Read the attributes
                                            for (int iAtt = 0; iAtt < readerXML.AttributeCount; iAtt++)
                                            {
                                                readerXML.MoveToAttribute(iAtt);
                                                Console.WriteLine("DEBUG Raw_Artifact AttributeName=" + readerXML.Name);
                                                Console.WriteLine("DEBUG Raw_Artifact AttributeValue=" + readerXML.Value);
                                                switch (readerXML.Name)
                                                {
                                                    case "datatype":
                                                        //string
                                                        Console.WriteLine("DEBUG Raw_Artifact datatype=" + readerXML.Value);
                                                        break;

                                                    default:
                                                        Console.WriteLine("ERROR Missing code for Raw_Artifact " + readerXML.Name);
                                                        break;
                                                }
                                            }
                                        }
                                        #endregion rawartifact
                                        break;

                                    //**********************************************************************
                                    case "cybox:Related_Objects":
                                        //TODO Attributes
                                        break;

                                    case "cybox:Related_Object":
                                        //TODO Attributes
                                        //<cybox:Related_Object id="example:Object-e2f9bed6-711b-4078-9639-160e0bfd8b45">

                                        break;

                                    case "cybox:Relationship":
                                        //<cybox:Relationship xsi:type="cyboxVocabs:ObjectRelationshipVocab-1.0">Characterized_By</cybox:Relationship>
                                        //TODO
                                        switch(sNodeLevelText)
                                        {
                                            case "":

                                                break;
                                            default:
                                                Console.WriteLine("ERROR Missing code for cybox:Relationship " + sNodeLevelText);
                                                break;
                                        }
                                        break;
                                    //**********************************************************************
                                    //**********************************************************************

                                    case "FileObj:File_Name":
                                        //<FileObj:File_Name>sample.zip</FileObj:File_Name>
                                        //TODO
                                        if (sCurrentNodeLevelText != "FileObj:FileObjectType")
                                        {
                                            Console.WriteLine("ERROR FileObj:File_Name not in FileObj:FileObjectType");
                                            return;
                                        }
                                        else
                                        {
                                            //TODO
                                        }
                                        break;

                                    //**********************************************************************
                                    //**********************************************************************
                                    case "DomainNameObj:Value":
                                        //<DomainNameObj:Value condition="Equals" apply_condition="ANY">malicious1.example.com##comma##malicious2.example.com##comma##malicious3.example.com</DomainNameObj:Value>
                                        if (sCurrentNodeLevelText != "DomainNameObj:DomainNameObjectType")
                                        {
                                            Console.WriteLine("ERROR DomainNameObj:Value not in DomainNameObj:DomainNameObjectType");

                                            return;
                                        }
                                        else
                                        {
                                            //TODO
                                            //Review the Attributes of the DomainNameObj:Value
                                            for (int iAtt = 0; iAtt < readerXML.AttributeCount; iAtt++)
                                            {
                                                readerXML.MoveToAttribute(iAtt);
                                                Console.WriteLine("DEBUG DomainNameObj:Value AttributeName=" + readerXML.Name);
                                                Console.WriteLine("DEBUG DomainNameObj:Value AttributeValue=" + readerXML.Value);
                                                switch (readerXML.Name)
                                                {
                                                    //condition="Equals"

                                                    //apply_condition="ANY"

                                                    default:
                                                        Console.WriteLine("ERROR Missing code for DomainNameObj:Value attribute " + readerXML.Name);
                                                        break;
                                                }
                                            }

                                        }
                                        break;

                                    //**********************************************************************
                                    //**********************************************************************
                                    case "stix:Indicators":
                                        //if (readerName == "stix:Indicators")
                                        //{
                                        sNodeLevelText = "stix:Indicators";
                                        //}
                                        break;

                                    //**********************************************************************
                                    case "stix:Indicator":
                                        #region indicator
                                        //if (readerName == "stix:Indicator")
                                        //{
                                        if (sNodeLevelText != "stix:Indicators")
                                        {
                                            Console.WriteLine(DateTimeOffset.Now.ToString());
                                            Console.WriteLine("ERROR Indicator not in Indicators");
                                            return;
                                        }
                                        else
                                        {
                                            //fParseSTIXIndicator()
                                            //Review the Attributes of the Indicator
                                            for (int iAtt = 0; iAtt < readerXML.AttributeCount; iAtt++)
                                            {
                                                readerXML.MoveToAttribute(iAtt);
                                                Console.WriteLine("DEBUG Indicator AttributeName=" + readerXML.Name);
                                                Console.WriteLine("DEBUG Indicator AttributeValue=" + readerXML.Value);
                                                switch (readerXML.Name)
                                                {
                                                    case "xsi:type":    //indicator:IndicatorType

                                                        break;
                                                    case "id":  //example:Indicator-ad560917-6ede-4abb-a4aa-994568a2abf4

                                                        break;
                                                    case "timestamp":   //2014-02-20T09:00:00.000000Z

                                                        break;
                                                    default:
                                                        Console.WriteLine("ERROR Missing code for Indicator attribute " + readerXML.Name);
                                                        break;
                                                }
                                            }
                                        }
                                        //}
                                        break;

                                    case "indicator:Type":
                                        //if (readerName == "indicator:Type")
                                        //{
                                        try
                                        {
                                            string sSTIXIndicatorType = readerXML.GetAttribute("xsi:type");
                                            //stixVocabs:IndicatorTypeVocab-1.1
                                            //TODO: Verify that the type is correct
                                            Console.WriteLine("DEBUG indicator:Type "+readerXML.ReadElementContentAsString());
                                            //Exfiltration
                                            //TODO: Verify that the value is correct
                                        }
                                        catch (Exception exSTIXIndicatorType)
                                        {
                                            Console.WriteLine(DateTimeOffset.Now.ToString());
                                            Console.WriteLine("Exception exSTIXIndicatorType " + exSTIXIndicatorType.Message + " " + exSTIXIndicatorType.InnerException);
                                            return;
                                        }
                                        //}
                                        break;

                                    case "indicator:Description":
                                        Console.WriteLine("DEBUG indicator:Description " + readerXML.ReadElementContentAsString());
                                        break;

                                    case "indicator:Observable":
                                    //<indicator:Observable id="example:Observable-87c9a5bb-d005-4b3e-8081-99f720fad62b">
                                        #region indicatorobservable
                                        //{
                                        //fParseSTIXTestMechanism()
                                        for (int iAtt = 0; iAtt < readerXML.AttributeCount; iAtt++)
                                        {
                                            readerXML.MoveToAttribute(iAtt);
                                            Console.WriteLine("DEBUG indicator:Observable AttributeName=" + readerXML.Name);
                                            Console.WriteLine("DEBUG indicator:Observable AttributeValue=" + readerXML.Value);
                                            switch (readerXML.Name)
                                            {
                                                case "xsi:type":    //
                                                    //TODO: check that the type is correct
                                                    break;
                                                case "id":  //example:Observable-87c9a5bb-d005-4b3e-8081-99f720fad62b

                                                    break;
                                                default:
                                                    Console.WriteLine("ERROR Missing code for Indicator Observable " + readerXML.Name);
                                                    break;
                                            }
                                        }
                                        //}
                                        #endregion indicatorobservable
                                        break;
                                    case "indicator:Test_Mechanisms":
                                        //if (readerName == "indicator:Test_Mechanisms")
                                        //{
                                        //sNodeLevelText = "Test_Mechanisms";   //TODO?

                                        //}
                                        break;

                                    case "indicator:Test_Mechanism":
                                        //if (readerName == "indicator:Test_Mechanism")
                                        #region testmechanism
                                        //{
                                        //fParseSTIXTestMechanism()
                                        for (int iAtt = 0; iAtt < readerXML.AttributeCount; iAtt++)
                                        {
                                            readerXML.MoveToAttribute(iAtt);
                                            Console.WriteLine("DEBUG indicator:Test_Mechanism AttributeName=" + readerXML.Name);
                                            Console.WriteLine("DEBUG indicator:Test_Mechanism AttributeValue=" + readerXML.Value);
                                            switch (readerXML.Name)
                                            {
                                                case "xsi:type":    //testMechSnort:SnortTestMechanismType
                                                    //TODO: check that the type is correct
                                                    break;
                                                case "id":  //example:TestMechanism-5f5fde43-ee30-4582-afaa-238a672f70b1

                                                    break;
                                                default:
                                                    Console.WriteLine("ERROR Missing code for Indicator Test_Mechanism " + readerXML.Name);
                                                    break;
                                            }
                                        }
                                        //}
                                        #endregion testmechanism

                                        #endregion indicator
                                        break;

                                    default:
                                        if (readerName != "stix:STIX_Package")
                                        {
                                            Console.WriteLine("ERROR Missing code for readerName " + readerName);
                                        }
                                        break;
                                }
                                #endregion readerName
                                break;

                            case XmlNodeType.Text: //Display the text in each element.
                                readerValue = readerXML.Value;
                                if (readerValue.Contains("##comma##"))
                                {
                                    string[] words = Regex.Split(readerValue, "##comma##");
                                    foreach (string word in words)
                                    {
                                        Console.WriteLine("DEBUG readerName=" + readerName + " for readerValueText=" + word);
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("DEBUG readerName=" + readerName + " for readerValueText=" + readerValue);
                                }
                                //Console.WriteLine("DEBUG readerValueText="+readerValue);
                                break;
                            
                            case XmlNodeType.EndElement: //Check the end of the element.
                                #region endelement
                                //Console.WriteLine("DEBUG EndElement="+readerXML.Name);
                                switch (readerXML.Name)
                                {
                                    case "stix:STIX_Header":
                                        if (sNodeLevelText != "STIX_Header")
                                        {
                                            Console.WriteLine(DateTimeOffset.Now.ToString());
                                            Console.WriteLine("ERROR STIX_Header not ended correctly");
                                            return;
                                        }
                                        break;

                                    //TODO

                                    default:

                                        break;
                                }
                                #endregion endelement
                                break;
                            case XmlNodeType.CDATA:
                            case XmlNodeType.Comment:
                            case XmlNodeType.XmlDeclaration:
                                //TODO
                                //Console.WriteLine("DEBUG Missing code for " + readerXML.Value);
                                break;

                            case XmlNodeType.DocumentType:
                                Console.WriteLine("DEBUG DocumentType=" + readerXML.Name + " - " + readerXML.Value);
                                break;
                        }
                    }
                }
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
        }
    }
}
