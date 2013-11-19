using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

using XORCISMModel;

namespace Import_oval
{
    static class Program
    {
        /// <summary>
        /// Copyright (C) 2013 Jerome Athias
        /// Parser for OVAL XML file and import the values into an XORCISM database
        /// This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation; either version 2 of the License, or (at your option) any later version.
        /// 
        /// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
        /// 
        /// You should have received a copy of the GNU General Public License along with this program; if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Parse an OVAL XML file and import the values into an XORCISM database
            //https://oval.mitre.org/rep-data/5.10/org.mitre.oval/v/index.html

            //VocabularyID=9    //TODO  HARDCODED
            int myVocabularyID = 9; //OVAL 5.10.1


            XmlDocument doc;
            doc = new XmlDocument();
            //doc.Load(@"C:\nvdcve\cisco.pix.xml");
            //doc.Load(@"C:\nvdcve\microsoft.windows.server.2012.xml");
            //doc.Load(@"C:\nvdcve\red.hat.enterprise.linux.4.xml");
            doc.Load(@"C:\nvdcve\ubuntu.6.06.xml");

            XmlNamespaceManager mgr = new XmlNamespaceManager(doc.NameTable);

            mgr.AddNamespace("oval", "http://oval.mitre.org/XMLSchema/oval-common-5");
            mgr.AddNamespace("oval-def", "http://oval.mitre.org/XMLSchema/oval-definitions-5");


            XmlNodeList nodes1;
            nodes1 = doc.SelectNodes("/oval-def:oval_definitions/oval-def:definitions/oval-def:definition", mgr);
            //Console.WriteLine(nodes1.Count);

            XORCISMEntities model;
            model = new XORCISMEntities();
            #region ovaldefinition
            //Note: this not the best/fastest way to parse XML, but is clear/easy enough
            foreach (XmlNode node in nodes1)    //definition
            {
                string myDefID = node.Attributes["id"].InnerText;
                Console.WriteLine("DEBUG: "+myDefID);
                string myDefVersion = node.Attributes["version"].InnerText;
                string sDefDeprecated = "";
                try
                {
                    sDefDeprecated = node.Attributes["deprecated"].InnerText;   //true
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception sDefDeprecated " + ex.Message + " " + ex.InnerException);
                }

                XORCISMModel.OVALMETADATA myOVALMetadata=new OVALMETADATA();
                XORCISMModel.OVALCRITERIA myCriteria = new OVALCRITERIA();

                string sClassValue = node.Attributes["class"].InnerText;
                XORCISMModel.OVALCLASSENUMERATION myclass;
                myclass = model.OVALCLASSENUMERATION.FirstOrDefault(o => o.ClassValue == sClassValue && o.VocabularyID == myVocabularyID);
                if (myclass == null)
                {
                    Console.WriteLine(string.Format("Adding new OVALCLASSENUMERATION [{0}] in table OVALCLASSENUMERATION", sClassValue));
                    myclass = new OVALCLASSENUMERATION();
                    myclass.ClassValue = sClassValue;
                    myclass.VocabularyID = myVocabularyID;
                    model.AddToOVALCLASSENUMERATION(myclass);
                    model.SaveChanges();
                }
                
                XORCISMModel.OVALDEFINITION myOVALDefinition;
                Boolean bAddDefinition = false;
                Boolean bAddMetadata = false;
                int iDefVersion = Convert.ToInt32(myDefVersion);
                myOVALDefinition = model.OVALDEFINITION.FirstOrDefault(o => o.OVALDefinitionIDPattern == myDefID && o.OVALDefinitionVersion == iDefVersion);
                if (myOVALDefinition == null)
                {
                    bAddDefinition = true;
                    //We need to Add the Metadata
                    bAddMetadata = true;
                }
                else
                {
                    //The definition exists
                    //We should have a metadata for it
                    myOVALMetadata = model.OVALMETADATA.FirstOrDefault(o => o.OVALMetadataID == myOVALDefinition.OVALMetadataID);
                    if (myOVALMetadata == null)
                    {
                        Console.WriteLine("ERROR METADATA not found for OVALDEFINITION");
                        bAddMetadata = true;
                    }
                }
                    foreach (XmlNode node2 in node.ChildNodes)
                    {
                        if (node2.Name == "metadata")
                        {
                            foreach (XmlNode nodeMetadata in node2.ChildNodes)
                            {
                                if (nodeMetadata.Name == "title")
                                {
                                    myOVALMetadata.Title = nodeMetadata.InnerText;
                                }
                                if (nodeMetadata.Name == "description")
                                {
                                    myOVALMetadata.Description = nodeMetadata.InnerText;
                                }
                            }
                        }
                        if (node2.Name == "criteria")
                        {
                            Boolean bAddCriteria = false;
                            if (bAddDefinition)
                            {
                                Console.WriteLine(string.Format("Adding new OVALCRITERIA [{0}] in table OVALCRITERIA", myDefID));
                                myCriteria = new OVALCRITERIA();
                                bAddCriteria = true;
                            }
                            else
                            {
                                //The definition exists, we should have a criteria for it
                                myCriteria = model.OVALCRITERIA.FirstOrDefault(o => o.OVALCriteriaID == myOVALDefinition.OVALCriteriaID);
                                //myCriteria = myOVALDefinition.OVALCRITERIA;
                                if (myCriteria == null)
                                {
                                    Console.WriteLine("ERROR no criteria for the OVALDefinition");
                                    bAddCriteria = true;
                                }
                            }
                            try
                            {
                                myCriteria.OperatorValue = node2.Attributes["operator"].InnerText;  //AND
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Exception myCriteria.OperatorValue " + ex.Message + " " + ex.InnerException);
                            }
                            myCriteria.comment = "";
                            try
                            {
                                myCriteria.comment = node2.Attributes["comment"].InnerText;
                            }
                            catch (Exception ex)
                            {

                            }

                            try
                            {
                                if (bAddCriteria)
                                {
                                    model.AddToOVALCRITERIA(myCriteria);
                                }
                                model.SaveChanges();
                            }
                            catch(Exception exAddToOVALCRITERIA)
                            {
                                Console.WriteLine("Exception exAddToOVALCRITERIA " + exAddToOVALCRITERIA.Message + " " + exAddToOVALCRITERIA.InnerException);
                            }
                            //**************************************
                            OVALParseCriteria(model, node2, myCriteria);


                        }
                    }

                    try
                    {
                        if (bAddMetadata)
                        {
                            Console.WriteLine(string.Format("Adding new OVALMETADATA [{0}] in table OVALMETADATA", myDefID));
                            model.AddToOVALMETADATA(myOVALMetadata);
                        }
                        model.SaveChanges();
                    }
                    catch (Exception exAddToOVALMETADATA)
                    {
                        Console.WriteLine("Exception exAddToOVALMETADATA " + exAddToOVALMETADATA.Message + " " + exAddToOVALMETADATA.InnerException);
                    }
                    
                    //TODO: not optimized
                    //Parse it again to add links
                    foreach (XmlNode node2 in node.ChildNodes)
                    {
                        if (node2.Name == "metadata")
                        {
                            foreach (XmlNode nodeMetadata in node2.ChildNodes)
                            {
                                if (nodeMetadata.Name == "affected")
                                {
                                    //OVALAFFECTED
                                    //OSFamily
                                    //OVALAFFECTEDPLATFORM
                                    string sOSFamily = nodeMetadata.Attributes["family"].InnerText; //pixos
                                    XORCISMModel.OSFAMILY myOSFamily;
                                    myOSFamily = model.OSFAMILY.FirstOrDefault(o => o.FamilyName == sOSFamily && o.VocabularyID == myVocabularyID);
                                    if (myOSFamily == null)
                                    {
                                        Console.WriteLine(string.Format("Adding new OSFAMILY [{0}] in table OSFAMILY", sOSFamily));
                                        myOSFamily = new OSFAMILY();
                                        myOSFamily.FamilyName = sOSFamily;
                                        myOSFamily.VocabularyID = myVocabularyID;
                                        try
                                        {
                                            model.AddToOSFAMILY(myOSFamily);
                                            model.SaveChanges();
                                        }
                                        catch (Exception exAddToOSFAMILY)
                                        {
                                            Console.WriteLine("Exception exAddToOSFAMILY " + exAddToOSFAMILY.Message + " " + exAddToOSFAMILY.InnerException);
                                        }
                                    }

                                    XORCISMModel.OVALAFFECTED myOVALAffected;
                                    myOVALAffected = model.OVALAFFECTED.FirstOrDefault(o => o.OSFamilyID == myOSFamily.OSFamilyID);
                                    if (myOVALAffected == null)
                                    {
                                        Console.WriteLine(string.Format("Adding new OVALAFFECTED [{0}] in table OVALAFFECTED", sOSFamily));
                                        myOVALAffected = new OVALAFFECTED();
                                        myOVALAffected.OSFamilyID = myOSFamily.OSFamilyID;
                                        try
                                        {
                                            model.AddToOVALAFFECTED(myOVALAffected);
                                            model.SaveChanges();
                                        }
                                        catch (Exception exAddToOVALAFFECTED)
                                        {
                                            Console.WriteLine("Exception exAddToOVALAFFECTED " + exAddToOVALAFFECTED.Message + " " + exAddToOVALAFFECTED.InnerException);
                                        }
                                    }

                                    //OVALAFFECTEDFOROVALMETADATA

                                    foreach (XmlNode nodePlatform in nodeMetadata.ChildNodes)
                                    {
                                        if (nodePlatform.Name == "platform")
                                        {
                                            //OVALAFFECTEDPLATFORM
                                            string sPlatform = nodePlatform.InnerText;  //Cisco PIX
                                            XORCISMModel.PLATFORM myPlatform;
                                            myPlatform = model.PLATFORM.FirstOrDefault(o => o.PlatformName == sPlatform);// && o.VocabularyID == myVocabularyID);   
                                            if (myPlatform == null)
                                            {
                                                Console.WriteLine(string.Format("Adding new PLATFORM [{0}] in table PLATFORM", sPlatform));
                                                myPlatform = new PLATFORM();
                                                myPlatform.PlatformName = sPlatform;
                                                myPlatform.VocabularyID = myVocabularyID;
                                                try
                                                {
                                                    model.AddToPLATFORM(myPlatform);
                                                    model.SaveChanges();
                                                }
                                                catch (Exception exAddToPLATFORM)
                                                {
                                                    Console.WriteLine("Exception exAddToPLATFORM " + exAddToPLATFORM.Message + " " + exAddToPLATFORM.InnerException);
                                                }
                                            }

                                            //OVALAFFECTEDPLATFORM
                                            XORCISMModel.OVALAFFECTEDPLATFORM myAffectedPlatform;
                                            myAffectedPlatform = model.OVALAFFECTEDPLATFORM.FirstOrDefault(o => o.OVALAffectedID == myOVALAffected.OVALAffectedID && o.PlatformID == myPlatform.PlatformID);
                                            if (myAffectedPlatform == null)
                                            {
                                                Console.WriteLine(string.Format("Adding new OVALAFFECTEDPLATFORM [{0}] in table OVALAFFECTEDPLATFORM", sPlatform));
                                                myAffectedPlatform = new OVALAFFECTEDPLATFORM();
                                                myAffectedPlatform.OVALAffectedID = myOVALAffected.OVALAffectedID;
                                                myAffectedPlatform.PlatformID = myPlatform.PlatformID;
                                                try
                                                {
                                                    model.AddToOVALAFFECTEDPLATFORM(myAffectedPlatform);
                                                    model.SaveChanges();
                                                }
                                                catch (Exception exAddToOVALAFFECTEDPLATFORM)
                                                {
                                                    Console.WriteLine("Exception exAddToOVALAFFECTEDPLATFORM " + exAddToOVALAFFECTEDPLATFORM.Message + " " + exAddToOVALAFFECTEDPLATFORM.InnerException);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (nodePlatform.Name == "product")
                                            {
                                                string sProductName = nodePlatform.InnerText;   //Kaspersky Endpoint Security 8
                                                XORCISMModel.PRODUCT myProduct;
                                                myProduct = model.PRODUCT.FirstOrDefault(o => o.ProductName == sProductName);
                                                if (myProduct == null)
                                                {
                                                    Console.WriteLine(string.Format("Adding new PRODUCT [{0}] in table PRODUCT", sProductName));
                                                    myProduct = new PRODUCT();
                                                    myProduct.ProductName = sProductName;
                                                    //TODO: search for CPE (SWID)
                                                    try
                                                    {
                                                        model.AddToPRODUCT(myProduct);
                                                        model.SaveChanges();
                                                    }
                                                    catch (Exception exAddToPRODUCT)
                                                    {
                                                        Console.WriteLine("Exception exAddToPRODUCT " + exAddToPRODUCT.Message + " " + exAddToPRODUCT.InnerException);
                                                    }
                                                }

                                                XORCISMModel.OVALAFFECTEDPRODUCT myAffectedProduct;
                                                myAffectedProduct = model.OVALAFFECTEDPRODUCT.FirstOrDefault(o => o.OVALAffectedID == myOVALAffected.OVALAffectedID && o.ProductID == myProduct.ProductID);
                                                if (myAffectedProduct == null)
                                                {
                                                    Console.WriteLine(string.Format("Adding new OVALAFFECTEDPRODUCT [{0}] in table OVALAFFECTEDPRODUCT", sProductName));
                                                    myAffectedProduct = new OVALAFFECTEDPRODUCT();
                                                    myAffectedProduct.OVALAffectedID = myOVALAffected.OVALAffectedID;
                                                    myAffectedProduct.ProductID = myProduct.ProductID;
                                                    try
                                                    {
                                                        model.AddToOVALAFFECTEDPRODUCT(myAffectedProduct);
                                                        model.SaveChanges();
                                                    }
                                                    catch (Exception exAddToOVALAFFECTEDPRODUCT)
                                                    {
                                                        Console.WriteLine("Exception exAddToOVALAFFECTEDPRODUCT " + exAddToOVALAFFECTEDPRODUCT.Message + " " + exAddToOVALAFFECTEDPRODUCT.InnerException);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                Console.WriteLine("Import_oval missing code for definition affected " + nodePlatform.Name);
                                            }
                                        }
                                    }
                                }
                                if (nodeMetadata.Name == "reference")
                                {
                                    string sReferenceSource = nodeMetadata.Attributes["source"].InnerText;
                                    string sReferenceSourceID = nodeMetadata.Attributes["ref_id"].InnerText;    //CVE-2008-3817     cpe:/o:ibm:aix:4.3
                                    string sReferenceUrl = "";
                                    try
                                    {
                                        sReferenceUrl = nodeMetadata.Attributes["ref_url"].InnerText;
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                    if (sReferenceSource.ToLower() == "cve")
                                    {
                                        //VULNERABILITY
                                        XORCISMModel.VULNERABILITY myVuln;
                                        myVuln = model.VULNERABILITY.FirstOrDefault(o => o.VULReferential == "cve" && o.VULReferentialID == sReferenceSourceID);
                                        if (myVuln == null)
                                        {
                                            Console.WriteLine(string.Format("Adding new VULNERABILITY [{0}] in table VULNERABILITY", sReferenceSourceID));
                                            myVuln = new VULNERABILITY();
                                            myVuln.VULReferential = "cve";
                                            myVuln.VULReferentialID = sReferenceSourceID;  //CVE-2008-3817
                                            myVuln.VULURL = sReferenceUrl;
                                            try
                                            {
                                                model.AddToVULNERABILITY(myVuln);
                                                model.SaveChanges();
                                            }
                                            catch (Exception exAddToVULNERABILITY)
                                            {
                                                Console.WriteLine("Exception exAddToVULNERABILITY " + exAddToVULNERABILITY.Message + " " + exAddToVULNERABILITY.InnerException);
                                            }
                                        }

                                        //OVALMETADATAVULNERABILITY
                                        XORCISMModel.OVALMETADATAVULNERABILITY myOVALVuln;
                                        myOVALVuln = model.OVALMETADATAVULNERABILITY.FirstOrDefault(o => o.OVALMetadataID == myOVALMetadata.OVALMetadataID && o.VulnerabilityID == myVuln.VulnerabilityID);
                                        if (myOVALVuln == null)
                                        {
                                            Console.WriteLine(string.Format("Adding new OVALMETADATAVULNERABILITY [{0}] in table OVALMETADATAVULNERABILITY", sReferenceSourceID));
                                            myOVALVuln = new OVALMETADATAVULNERABILITY();
                                            myOVALVuln.OVALMetadataID = myOVALMetadata.OVALMetadataID;
                                            myOVALVuln.VulnerabilityID = myVuln.VulnerabilityID;
                                            try
                                            {
                                                model.AddToOVALMETADATAVULNERABILITY(myOVALVuln);
                                                model.SaveChanges();
                                            }
                                            catch (Exception exAddToOVALMETADATAVULNERABILITY)
                                            {
                                                Console.WriteLine("Exception exAddToOVALMETADATAVULNERABILITY " + exAddToOVALMETADATAVULNERABILITY.Message + " " + exAddToOVALMETADATAVULNERABILITY.InnerException);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (sReferenceSource.ToLower() == "cpe")
                                        {
                                            //OVALMETADATACPE
                                            //string sCPEID = nodeMetadata.Attributes["ref_id"].InnerText;    sReferenceSourceID
                                            XORCISMModel.CPE myCPE;
                                            myCPE = model.CPE.FirstOrDefault(o => o.CPEID == sReferenceSourceID);
                                            if (myCPE == null)
                                            {
                                                Console.WriteLine("Adding new CPE " + sReferenceSourceID);
                                                myCPE = new CPE();
                                                myCPE.CPEID = sReferenceSourceID;
                                                try
                                                {
                                                    model.AddToCPE(myCPE);
                                                    model.SaveChanges();
                                                }
                                                catch (Exception exAddToCPE)
                                                {
                                                    Console.WriteLine("Exception exAddToCPE " + exAddToCPE.Message + " " + exAddToCPE.InnerException);
                                                }
                                            }

                                            XORCISMModel.OVALMETADATACPE myOVALMETADATACPE;
                                            myOVALMETADATACPE = model.OVALMETADATACPE.FirstOrDefault(o => o.OVALMetadataID == myOVALMetadata.OVALMetadataID && o.CPEID == sReferenceSourceID);
                                            if (myOVALMETADATACPE == null)
                                            {
                                                Console.WriteLine("Adding new OVALMETADATACPE " + sReferenceSourceID);
                                                myOVALMETADATACPE = new OVALMETADATACPE();
                                                myOVALMETADATACPE.OVALMetadataID = myOVALMetadata.OVALMetadataID;
                                                myOVALMETADATACPE.CPEID = sReferenceSourceID;
                                                try
                                                {
                                                    model.AddToOVALMETADATACPE(myOVALMETADATACPE);
                                                    model.SaveChanges();
                                                }
                                                catch (Exception exAddToOVALMETADATACPE)
                                                {
                                                    Console.WriteLine("Exception exAddToOVALMETADATACPE " + exAddToOVALMETADATACPE.Message + " " + exAddToOVALMETADATACPE.InnerException);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //OVALMETADATAREFERENCE
                                            XORCISMModel.REFERENCE myReference;
                                            myReference = model.REFERENCE.FirstOrDefault(o => o.Source == sReferenceSource && o.ReferenceSourceID == sReferenceSourceID && o.Url == sReferenceUrl);
                                            if (myReference == null)
                                            {
                                                Console.WriteLine(string.Format("Adding new REFERENCE [{0}] in table REFERENCE", sReferenceUrl));
                                                myReference = new REFERENCE();
                                                myReference.Source = sReferenceSource;
                                                myReference.ReferenceSourceID = sReferenceSourceID;
                                                myReference.Url = sReferenceUrl;
                                                try
                                                {
                                                    model.AddToREFERENCE(myReference);
                                                    model.SaveChanges();
                                                }
                                                catch (Exception exAddToREFERENCE)
                                                {
                                                    Console.WriteLine("Exception exAddToREFERENCE " + exAddToREFERENCE.Message + " " + exAddToREFERENCE.InnerException);
                                                }
                                            }

                                            XORCISMModel.OVALMETADATAREFERENCE myOVALMetaReference;
                                            myOVALMetaReference = model.OVALMETADATAREFERENCE.FirstOrDefault(o => o.OVALMetadataID == myOVALMetadata.OVALMetadataID && o.ReferenceID == myReference.ReferenceID);
                                            if (myOVALMetaReference == null)
                                            {
                                                Console.WriteLine(string.Format("Adding new OVALMETADATAREFERENCE [{0}] in table OVALMETADATAREFERENCE", sReferenceUrl));
                                                myOVALMetaReference = new OVALMETADATAREFERENCE();
                                                myOVALMetaReference.OVALMetadataID = myOVALMetadata.OVALMetadataID;
                                                myOVALMetaReference.ReferenceID = myReference.ReferenceID;
                                                try
                                                {
                                                    model.AddToOVALMETADATAREFERENCE(myOVALMetaReference);
                                                    model.SaveChanges();
                                                }
                                                catch (Exception exAddToOVALMETADATAREFERENCE)
                                                {
                                                    Console.WriteLine("Exception exAddToOVALMETADATAREFERENCE " + exAddToOVALMETADATAREFERENCE.Message + " " + exAddToOVALMETADATAREFERENCE.InnerException);
                                                }
                                            }

                                            if (sReferenceSource.ToLower() == "cce")
                                            {
                                                //<reference source="CCE" ref_id="CCE-6224-0" ref_url="http://cce.mitre.org/lists/data/downloads/cce-rhel4-5.20090506.xls"/>
                                                //TODO
                                                //CCE
                                                //OVALMETADATACCE
                                                XORCISMModel.CCE myCCE;
                                                myCCE = model.CCE.FirstOrDefault(o => o.cce_id == sReferenceSourceID);
                                                if (myCCE == null)
                                                {
                                                    Console.WriteLine("Unknown CCE " + sReferenceSourceID);
                                                    Console.WriteLine("Adding new CCE");
                                                    myCCE = new CCE();
                                                    myCCE.cce_id = sReferenceSourceID;
                                                    try
                                                    {
                                                        model.AddToCCE(myCCE);
                                                        model.SaveChanges();
                                                    }
                                                    catch (Exception exAddToCCE)
                                                    {
                                                        Console.WriteLine("Exception exAddToCCE " + exAddToCCE.Message + " " + exAddToCCE.InnerException);
                                                    }
                                                }

                                                XORCISMModel.OVALMETADATACCE myOVALCCE;
                                                myOVALCCE = model.OVALMETADATACCE.FirstOrDefault(o => o.OVALMetadataID == myOVALMetadata.OVALMetadataID && o.cce_id == sReferenceSourceID);
                                                if (myOVALCCE == null)
                                                {
                                                    Console.WriteLine("Adding new OVALMETADATACCE");
                                                    myOVALCCE = new OVALMETADATACCE();
                                                    myOVALCCE.OVALMetadataID = myOVALMetadata.OVALMetadataID;
                                                    myOVALCCE.cce_id = sReferenceSourceID;
                                                    myOVALCCE.timestamp = DateTimeOffset.Now;
                                                    try
                                                    {
                                                        model.AddToOVALMETADATACCE(myOVALCCE);
                                                        model.SaveChanges();
                                                    }
                                                    catch (Exception exAddToOVALMETADATACCE)
                                                    {
                                                        Console.WriteLine("Exception exAddToOVALMETADATACCE " + exAddToOVALMETADATACCE.Message + " " + exAddToOVALMETADATACCE.InnerException);
                                                    }
                                                }
                                            }

                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (bAddDefinition)
                    {
                        Console.WriteLine(string.Format("Adding new OVALDEFINITION [{0}] in table OVALDEFINITION", myDefID));
                        myOVALDefinition = new OVALDEFINITION();
                    }
                    myOVALDefinition.OVALDefinitionIDPattern = myDefID;
                    myOVALDefinition.OVALDefinitionVersion = iDefVersion;
                    myOVALDefinition.OVALClassEnumerationID = myclass.OVALClassEnumerationID;
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
                            model.AddToOVALDEFINITION(myOVALDefinition);
                        }
                        model.SaveChanges();
                    }
                    catch (Exception exAddToOVALDEFINITION)
                    {
                        Console.WriteLine("Exception exAddToOVALDEFINITION " + exAddToOVALDEFINITION.Message + " " + exAddToOVALDEFINITION.InnerException);
                    }
                //}
                //else
                //{
                //    //The OVALDEFINITION already exists
                //    //TODO
                //}



            }
            #endregion ovaldefinition

            #region ovaltests
            nodes1 = doc.SelectNodes("/oval-def:oval_definitions/oval-def:tests", mgr);
            foreach (XmlNode node in nodes1)
            {
                foreach (XmlNode nodeINTEST in node)
                {
                    //if (nodeINTEST.Name == "version_test" || nodeINTEST.Name == "line_test")
                    //{
                        string sTestID = nodeINTEST.Attributes["id"].InnerText;
                        string sTestVersion = nodeINTEST.Attributes["version"].InnerText;
                        XORCISMModel.OVALTEST myOVALTest;
                        int iTestVersion = Convert.ToInt32(sTestVersion);
                        myOVALTest = model.OVALTEST.FirstOrDefault(o => o.OVALTestIDPattern == sTestID && o.OVALTestVersion == iTestVersion);
                        Boolean bAddOVALTEST = false;
                        if (myOVALTest == null)
                        {
                            Console.WriteLine("Adding OVALTEST" + " " + sTestID);
                            myOVALTest = new OVALTEST();
                            bAddOVALTEST = true;
                        }
                            myOVALTest.OVALTestIDPattern = sTestID;
                            myOVALTest.OVALTestVersion = iTestVersion;
                            myOVALTest.DataTypeName = nodeINTEST.Name;  //version_test

                            string sExistence = nodeINTEST.Attributes["check_existence"].InnerText;
                            XORCISMModel.EXISTENCEENUMERATION myExistence;
                            myExistence = model.EXISTENCEENUMERATION.FirstOrDefault(o => o.ExistenceValue == sExistence);   //&& o.VocabularyID == myVocabularyID
                            if (myExistence == null)
                            {
                                Console.WriteLine("Adding EXISTENCEENUMERATION " + sExistence);
                                myExistence = new EXISTENCEENUMERATION();
                                myExistence.ExistenceValue = sExistence;
                                myExistence.VocabularyID = myVocabularyID;
                                try
                                {
                                    model.AddToEXISTENCEENUMERATION(myExistence);
                                    model.SaveChanges();
                                }
                                catch (Exception exAddToEXISTENCEENUMERATION)
                                {
                                    Console.WriteLine("Exception exAddToEXISTENCEENUMERATION " + exAddToEXISTENCEENUMERATION.Message + " " + exAddToEXISTENCEENUMERATION.InnerException);
                                }
                            }
                            myOVALTest.ExistenceEnumerationID = myExistence.ExistenceEnumerationID;
                            myOVALTest.ExistenceValue = sExistence;

                            string sCheck = nodeINTEST.Attributes["check"].InnerText;
                            XORCISMModel.CHECKENUMERATION myCheck;
                            myCheck = model.CHECKENUMERATION.FirstOrDefault(o => o.EnumerationValue == sCheck);   //&& o.VocabularyID == myVocabularyID
                            if (myCheck == null)
                            {
                                Console.WriteLine("Adding CHECKENUMERATION " + sCheck);
                                myCheck = new CHECKENUMERATION();
                                myCheck.EnumerationValue = sCheck;
                                myCheck.VocabularyID = myVocabularyID;
                                try
                                {
                                    model.AddToCHECKENUMERATION(myCheck);
                                    model.SaveChanges();
                                }
                                catch (Exception exAddToCHECKENUMERATION)
                                {
                                    Console.WriteLine("Exception exAddToCHECKENUMERATION " + exAddToCHECKENUMERATION.Message + " " + exAddToCHECKENUMERATION.InnerException);
                                }
                            }
                            myOVALTest.CheckEnumerationID = myCheck.CheckEnumerationID;
                            myOVALTest.EnumerationValue = sCheck;
                            //TODO
                            //myOVALTest.OperatorEnumerationID=
                            //myOVALTest.OperatorValue = nodeINTEST.Attributes["operator"].InnerText;
                            myOVALTest.comment = nodeINTEST.Attributes["comment"].InnerText;
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
                            string sNameSpace = "";
                            string sXMLNS = nodeINTEST.Attributes["xmlns"].InnerText;
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
                            //TODO: mapping with other platforms vocabularies (i.e.: MSF)
                            if (sNameSpace == "")
                            {
                                Console.WriteLine("DEBUG: Unknown OVALTEST Namespace: " + sXMLNS);
                            }
                            myOVALTest.Namespace = sNameSpace;

                            myOVALTest.DataTypeName = nodeINTEST.Name;  //version_test
                            try
                            {
                                if (bAddOVALTEST)
                                {
                                    model.AddToOVALTEST(myOVALTest);
                                }
                                model.SaveChanges();
                            }
                            catch (Exception exAddToOVALTEST)
                            {
                                Console.WriteLine("Exception exAddToOVALTEST " + exAddToOVALTEST.Message + " " + exAddToOVALTEST.InnerException);
                            }
                        

                        foreach (XmlNode nodeTest in nodeINTEST)
                        {
                            switch (nodeTest.Name)
                            {
                                case "object":
                                    string sObjectIDPattern = nodeTest.Attributes["object_ref"].InnerText;
                                    XORCISMModel.OVALOBJECT myOVALObject;
                                    myOVALObject = model.OVALOBJECT.FirstOrDefault(o => o.OVALObjectIDPattern == sObjectIDPattern);// && o.OVALObjectVersion == Convert.ToInt32(sTestVersion));
                                    if (myOVALObject == null)
                                    {
                                        Console.WriteLine("Adding OVALOBJECT" + " " + sObjectIDPattern);
                                        myOVALObject = new OVALOBJECT();
                                        myOVALObject.OVALObjectIDPattern = sObjectIDPattern;
                                        myOVALObject.comment = "";
                                        try
                                        {
                                            model.AddToOVALOBJECT(myOVALObject);
                                            model.SaveChanges();
                                        }
                                        catch (Exception exAddToOVALOBJECT)
                                        {
                                            Console.WriteLine("Exception exAddToOVALOBJECT " + exAddToOVALOBJECT.Message + " " + exAddToOVALOBJECT.InnerException);
                                        }
                                    }

                                    XORCISMModel.OVALOBJECTFOROVALTEST myOVALObjectForOVALTEST;
                                    myOVALObjectForOVALTEST = model.OVALOBJECTFOROVALTEST.FirstOrDefault(o => o.OVALObjectID == myOVALObject.OVALObjectID && o.OVALTestID == myOVALTest.OVALTestID);// && o.OVALObjectVersion == Convert.ToInt32(sTestVersion));
                                    if (myOVALObjectForOVALTEST == null)
                                    {
                                        Console.WriteLine("Adding OVALOBJECTFOROVALTEST" + " " + sObjectIDPattern);
                                        myOVALObjectForOVALTEST = new OVALOBJECTFOROVALTEST();
                                        myOVALObjectForOVALTEST.OVALTestID = myOVALTest.OVALTestID;
                                        myOVALObjectForOVALTEST.OVALObjectID = myOVALObject.OVALObjectID;
                                        //TODO: timestamp...
                                        try
                                        {
                                            model.AddToOVALOBJECTFOROVALTEST(myOVALObjectForOVALTEST);
                                            model.SaveChanges();
                                        }
                                        catch (Exception exAddToOVALOBJECTFOROVALTEST)
                                        {
                                            Console.WriteLine("Exception exAddToOVALOBJECTFOROVALTEST " + exAddToOVALOBJECTFOROVALTEST.Message + " " + exAddToOVALOBJECTFOROVALTEST.InnerException);
                                        }
                                    }

                                    break;
                                case "state":
                                    string sStateIDPattern = nodeTest.Attributes["state_ref"].InnerText;
                                    XORCISMModel.OVALSTATE myOVALState;
                                    myOVALState = model.OVALSTATE.FirstOrDefault(o => o.OVALStateIDPattern == sStateIDPattern);// && o.OVALStateVersion == Convert.ToInt32(sTestVersion));
                                    if (myOVALState == null)
                                    {
                                        Console.WriteLine("Adding OVALSTATE" + " " + sStateIDPattern);
                                        myOVALState = new OVALSTATE();
                                        myOVALState.OVALStateIDPattern = sStateIDPattern;
                                        myOVALState.comment = "";
                                        try
                                        {
                                            model.AddToOVALSTATE(myOVALState);
                                            model.SaveChanges();
                                        }
                                        catch (Exception exAddToOVALSTATE)
                                        {
                                            Console.WriteLine("Exception exAddToOVALSTATE " + exAddToOVALSTATE.Message + " " + exAddToOVALSTATE.InnerException);
                                        }
                                    }
                                    XORCISMModel.OVALSTATEFOROVALTEST myOVALStateForOVALTest;
                                    myOVALStateForOVALTest = model.OVALSTATEFOROVALTEST.FirstOrDefault(o => o.OVALStateID == myOVALState.OVALStateID && o.OVALTestID == myOVALTest.OVALTestID);// && o.OVALObjectVersion == Convert.ToInt32(sTestVersion));
                                    if (myOVALStateForOVALTest == null)
                                    {
                                        Console.WriteLine("Adding OVALSTATEFOROVALTEST" + " " + sStateIDPattern);
                                        myOVALStateForOVALTest = new OVALSTATEFOROVALTEST();
                                        myOVALStateForOVALTest.OVALTestID = myOVALTest.OVALTestID;
                                        myOVALStateForOVALTest.OVALStateID = myOVALState.OVALStateID;
                                        //TODO: timestamp...
                                        try
                                        {
                                            model.AddToOVALSTATEFOROVALTEST(myOVALStateForOVALTest);
                                            model.SaveChanges();
                                        }
                                        catch (Exception exAddToOVALSTATEFOROVALTEST)
                                        {
                                            Console.WriteLine("Exception exAddToOVALSTATEFOROVALTEST " + exAddToOVALSTATEFOROVALTEST.Message + " " + exAddToOVALSTATEFOROVALTEST.InnerException);
                                        }
                                    }
                                    break;
                                default:
                                    Console.WriteLine("Import_oval missing code for ovaltests " + nodeTest.Name);
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


            #region ovalobjects
            nodes1 = doc.SelectNodes("/oval-def:oval_definitions/oval-def:objects", mgr);
            foreach (XmlNode node in nodes1)
            {
                //TODO
                //oslevel_object
                //fix_object
                //version_object
                //line_object

                //OVALBehavior  //<behaviors windows_view="32_bit"/>
                //All the objects should already exist
                foreach (XmlNode nodeOBJECT in node)
                {
                    //Console.WriteLine(nodeOBJECT.Name); //version_object    line_object
                    string sObjectIDPattern = nodeOBJECT.Attributes["id"].InnerText;
                    Boolean bAddObject = false;
                    XORCISMModel.OVALOBJECT myOVALObject;
                    myOVALObject = model.OVALOBJECT.FirstOrDefault(o => o.OVALObjectIDPattern == sObjectIDPattern);// && o.OVALObjectVersion == Convert.ToInt32(sTestVersion));
                    if (myOVALObject == null)
                    {
                        //Console.WriteLine("ERROR OVALOBJECT not found "+sObjectIDPattern);
                        //Referenced in a variable
                        bAddObject = true;
                        myOVALObject = new OVALOBJECT();
                        myOVALObject.OVALObjectIDPattern = sObjectIDPattern;
                    }
                    
                        myOVALObject.DataTypeName = nodeOBJECT.Name;    //version_object    line_object
                        int iObjectVersion = Convert.ToInt32(nodeOBJECT.Attributes["version"].InnerText);
                        myOVALObject.OVALObjectVersion = iObjectVersion;
                        try
                        {
                            myOVALObject.comment = nodeOBJECT.Attributes["comment"].InnerText;
                        }
                        catch (Exception ex)
                        {

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

                        }
                        //myOVALObject.Namespace    //xmlns="http://oval.mitre.org/XMLSchema/oval-definitions-5#pixos"  pixos-def
                        string sNameSpace = "";
                        string sXMLNS=nodeOBJECT.Attributes["xmlns"].InnerText;
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
                            Console.WriteLine("Unknown OBJECT Namespace: " + sXMLNS);
                        }
                        myOVALObject.Namespace = sNameSpace;

                        try
                        {
                            if (bAddObject)
                            {
                                model.AddToOVALOBJECT(myOVALObject);
                            }
                            model.SaveChanges();
                        }
                        catch (Exception exAddToOVALOBJECT)
                        {
                            Console.WriteLine("Exception exAddToOVALOBJECT " + exAddToOVALOBJECT.Message + " " + exAddToOVALOBJECT.InnerException);
                        }

                        if (nodeOBJECT.ChildNodes.Count > 0)
                        {
                            //We need a record
                            XORCISMModel.OVALOBJECTRECORD myOVALObjectRecord;
                            XORCISMModel.OVALOBJECTRECORDFOROVALOBJECT myOVALObjectRecordForOVALObject;
                            myOVALObjectRecordForOVALObject = model.OVALOBJECTRECORDFOROVALOBJECT.FirstOrDefault(o => o.OVALObjectID == myOVALObject.OVALObjectID);
                            if (myOVALObjectRecordForOVALObject == null)
                            {
                                //TODO
                                //OVAL COMPLEXBASE
                                myOVALObjectRecord = new OVALOBJECTRECORD();
                                myOVALObjectRecord.DataTypeName = nodeOBJECT.Name;    //line_object
                                //Operation
                                //...
                                //mask
                                myOVALObjectRecord.Namespace = sNameSpace;   //pixos-def
                                try
                                {
                                    model.AddToOVALOBJECTRECORD(myOVALObjectRecord);
                                    model.SaveChanges();
                                }
                                catch (Exception exAddToOVALOBJECTRECORD)
                                {
                                    Console.WriteLine("Exception exAddToOVALOBJECTRECORD " + exAddToOVALOBJECTRECORD.Message + " " + exAddToOVALOBJECTRECORD.InnerException);
                                }

                                myOVALObjectRecordForOVALObject = new OVALOBJECTRECORDFOROVALOBJECT();
                                myOVALObjectRecordForOVALObject.OVALObjectID = myOVALObject.OVALObjectID;
                                myOVALObjectRecordForOVALObject.OVALObjectRecordID = myOVALObjectRecord.OVALObjectRecordID;
                                myOVALObjectRecordForOVALObject.timestamp = DateTimeOffset.Now;
                                try
                                {
                                    model.AddToOVALOBJECTRECORDFOROVALOBJECT(myOVALObjectRecordForOVALObject);
                                    model.SaveChanges();
                                }
                                catch (Exception exAddToOVALOBJECTRECORDFOROVALOBJECT)
                                {
                                    Console.WriteLine("Exception exAddToOVALOBJECTRECORDFOROVALOBJECT " + exAddToOVALOBJECTRECORDFOROVALOBJECT.Message + " " + exAddToOVALOBJECTRECORDFOROVALOBJECT.InnerException);
                                }
                            }
                            else
                            {
                                //Console.WriteLine(myOVALObjectRecordForOVALObject.OVALObjectRecordID);
                                myOVALObjectRecord = myOVALObjectRecordForOVALObject.OVALOBJECTRECORD;
                                //Console.WriteLine(myOVALObjectRecord.OVALObjectRecordID);
                            }
                            int iOVALObjectRecordID = myOVALObjectRecord.OVALObjectRecordID;
                            foreach (XmlNode nodeOBJECTFIELD in nodeOBJECT)
                            {
                                //TODO
                                //<behaviors windows_view="32_bit"/>
                                if (nodeOBJECTFIELD.Name == "behaviors")
                                {
                                    //TODO
                                    //OVALBEHAVIOR
                                    string sBehaviorKey = nodeOBJECTFIELD.Attributes[0].Name;   //windows_view
                                    Console.WriteLine("DEBUG sBehaviorKey: "+sBehaviorKey);
                                    //Note: BehaviorKey was BehaviorName
                                    string sBehaviorValue = nodeOBJECTFIELD.Attributes[0].InnerText;    //32_bit
                                    XORCISMModel.OVALBEHAVIOR myBehavior;
                                    myBehavior = model.OVALBEHAVIOR.FirstOrDefault(o => o.BehaviorKey == sBehaviorKey && o.BehaviorValue == sBehaviorValue);
                                    if (myBehavior == null)
                                    {
                                        myBehavior = new OVALBEHAVIOR();
                                        myBehavior.BehaviorKey = sBehaviorKey;
                                        myBehavior.BehaviorValue = sBehaviorValue;
                                        try
                                        {
                                            model.AddToOVALBEHAVIOR(myBehavior);
                                            model.SaveChanges();
                                        }
                                        catch (Exception exAddToOVALBEHAVIOR)
                                        {
                                            Console.WriteLine("Exception exAddToOVALBEHAVIOR " + exAddToOVALBEHAVIOR.Message + " " + exAddToOVALBEHAVIOR.InnerException);
                                        }
                                    }

                                    XORCISMModel.OVALBEHAVIORFOROVALOBJECT myBehaviorForObject;
                                    myBehaviorForObject = model.OVALBEHAVIORFOROVALOBJECT.FirstOrDefault(o => o.OVALObjectID == myOVALObject.OVALObjectID && o.OVALBehaviorID == myBehavior.OVALBehaviorID);
                                    if (myBehaviorForObject == null)
                                    {
                                        myBehaviorForObject = new OVALBEHAVIORFOROVALOBJECT();
                                        myBehaviorForObject.OVALObjectID = myOVALObject.OVALObjectID;
                                        myBehaviorForObject.OVALBehaviorID = myBehavior.OVALBehaviorID;
                                        try
                                        {
                                            model.AddToOVALBEHAVIORFOROVALOBJECT(myBehaviorForObject);
                                            model.SaveChanges();
                                        }
                                        catch (Exception exAddToOVALBEHAVIORFOROVALOBJECT)
                                        {
                                            Console.WriteLine("Exception exAddToOVALBEHAVIORFOROVALOBJECT " + exAddToOVALBEHAVIORFOROVALOBJECT.Message + " " + exAddToOVALBEHAVIORFOROVALOBJECT.InnerException);
                                        }
                                    }
                                }
                                else
                                {
                                    //TODO
                                    //We assume that we have only one OVALOBJECTFIELD for one DataType for one OVALOBJECTRECORD
                                    XORCISMModel.OVALOBJECTFIELD myOVALObjectField;
                                    Boolean bAddOvalObjectField = false;
                                    XORCISMModel.OVALOBJECTFIELDFOROVALOBJECTRECORD myOVALObjectFieldForOVALObjectRecord;
                                    Boolean bAddOvalObjectFieldForOvalObjectRecord = false;
                                    myOVALObjectFieldForOVALObjectRecord = model.OVALOBJECTFIELDFOROVALOBJECTRECORD.FirstOrDefault(o => o.OVALObjectRecordID == myOVALObjectRecord.OVALObjectRecordID && o.OVALOBJECTFIELD.FieldName == nodeOBJECTFIELD.Name);
                                    if (myOVALObjectFieldForOVALObjectRecord == null)
                                    {
                                        bAddOvalObjectFieldForOvalObjectRecord = true;
                                        myOVALObjectFieldForOVALObjectRecord = new OVALOBJECTFIELDFOROVALOBJECTRECORD();
                                        //We add a OVALOBJECTFIELD
                                        myOVALObjectField = new OVALOBJECTFIELD();
                                        bAddOvalObjectField = true;


                                    }
                                    else
                                    {
                                        myOVALObjectField = myOVALObjectFieldForOVALObjectRecord.OVALOBJECTFIELD;
                                    }
                                    myOVALObjectFieldForOVALObjectRecord.timestamp = DateTimeOffset.Now;
                                    //TODO
                                    //
                                    myOVALObjectField.FieldName = nodeOBJECTFIELD.Name;    //show_subcommand
                                    try
                                    {
                                        string sVarCheck = nodeOBJECTFIELD.Attributes["var_check"].InnerText;    //may not exist  //all
                                        //TODO
                                        //CheckEnumerationID
                                        myOVALObjectField.VarCheck = sVarCheck;
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                    string sOVALVariableIDPattern = "";
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
                                    if (sOVALVariableIDPattern != "")
                                    {
                                        //OVALVARIABLEFOROVALOBJECTFIELD
                                        XORCISMModel.OVALVARIABLE myOVALVariable;
                                        myOVALVariable = model.OVALVARIABLE.FirstOrDefault(o => o.OVALVariableIDPattern == sOVALVariableIDPattern);
                                        if (myOVALVariable == null)
                                        {
                                            Console.WriteLine("Adding OVALVARIABLE " + sOVALVariableIDPattern);
                                            myOVALVariable = new OVALVARIABLE();
                                            myOVALVariable.OVALVariableIDPattern = sOVALVariableIDPattern;
                                            //TODO
                                            myOVALVariable.OVALVariableVersion = 1;
                                            myOVALVariable.DataTypeName = "string"; //Hardcoded default
                                            myOVALVariable.comment = "";
                                            try
                                            {
                                                model.AddToOVALVARIABLE(myOVALVariable);
                                                model.SaveChanges();
                                            }
                                            catch (Exception exAddToOVALVARIABLE)
                                            {
                                                Console.WriteLine("Exception exAddToOVALVARIABLE " + exAddToOVALVARIABLE.Message + " " + exAddToOVALVARIABLE.InnerException);
                                            }
                                        }
                                    }

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
                                    try
                                    {
                                        //myOVALObjectField.OperationValue = nodeOBJECTFIELD.Attributes["operation"].InnerText; //equals
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                    //TODO
                                    //OVALENTITYATTRIBUTEGROUP
                                    myOVALObjectField.OperationValue = "equals";
                                    try
                                    {
                                        string sOperation = nodeOBJECTFIELD.Attributes["operation"].InnerText;  //pattern match
                                        XORCISMModel.OPERATIONENUMERATION myOperation;
                                        myOperation = model.OPERATIONENUMERATION.FirstOrDefault(o => o.OperationValue == sOperation);   //&&VocabularyID
                                        if (myOperation == null)
                                        {
                                            Console.WriteLine("ERROR Unknown Operation " + sOperation);
                                        }
                                        else
                                        {
                                            myOVALObjectField.OperationEnumerationID = myOperation.OperationEnumerationID;
                                        }
                                        myOVALObjectField.OperationValue = sOperation;

                                    }
                                    catch (Exception ex)
                                    {
                                        myOVALObjectField.OperationEnumerationID = 1;   //Hardcoded
                                        myOVALObjectField.OperationValue = "equals";
                                    }
                                    myOVALObjectField.FieldValue = nodeOBJECTFIELD.InnerText;   //show running-config
                                    
                                    myOVALObjectField.timestamp = DateTimeOffset.Now;
                                    //Namespace //pixos-def
                                    myOVALObjectField.Namespace = sNameSpace;   //same as the object

                                    try
                                    {
                                        if (bAddOvalObjectField)
                                        {
                                            model.AddToOVALOBJECTFIELD(myOVALObjectField);
                                        }

                                        model.SaveChanges();
                                    }
                                    catch (Exception exAddToOVALOBJECTFIELD)
                                    {
                                        Console.WriteLine("Exception exAddToOVALOBJECTFIELD " + exAddToOVALOBJECTFIELD.Message + " " + exAddToOVALOBJECTFIELD.InnerException);
                                    }

                                    myOVALObjectFieldForOVALObjectRecord.OVALObjectRecordID = myOVALObjectRecord.OVALObjectRecordID;
                                    myOVALObjectFieldForOVALObjectRecord.OVALObjectFieldID = myOVALObjectField.OVALObjectFieldID;
                                    try
                                    {
                                        if (bAddOvalObjectFieldForOvalObjectRecord)
                                        {
                                            model.AddToOVALOBJECTFIELDFOROVALOBJECTRECORD(myOVALObjectFieldForOVALObjectRecord);
                                        }
                                        model.SaveChanges();
                                    }
                                    catch (Exception exAddToOVALOBJECTFIELDFOROVALOBJECTRECORD)
                                    {
                                        Console.WriteLine("Exception exAddToOVALOBJECTFIELDFOROVALOBJECTRECORD " + exAddToOVALOBJECTFIELDFOROVALOBJECTRECORD.Message + " " + exAddToOVALOBJECTFIELDFOROVALOBJECTRECORD.InnerException);
                                    }
                                }
                            }
                        }
                    

                }
            }
            #endregion ovalobjects

            #region ovalstates
            nodes1 = doc.SelectNodes("/oval-def:oval_definitions/oval-def:states", mgr);
            foreach (XmlNode node in nodes1)
            {
                //TODO
                //oslevel_state
                //fix_state
                //States have been added before and so should all exist
                foreach (XmlNode nodeSTATE in node)
                {
                    //Console.WriteLine(nodeSTATE.Name);
                    //version_state
                    string sStateDetailIDPattern = nodeSTATE.Attributes["id"].InnerText;
                    XORCISMModel.OVALSTATE myOVALStateDetail;
                    myOVALStateDetail = model.OVALSTATE.FirstOrDefault(o => o.OVALStateIDPattern == sStateDetailIDPattern);// && o.OVALStateVersion == Convert.ToInt32(sTestVersion));
                    if (myOVALStateDetail == null)
                    {
                        Console.WriteLine("ERROR OVALState not found " + sStateDetailIDPattern);
                    }
                    else
                    {
                        myOVALStateDetail.OVALStateType = nodeSTATE.Name;   //variable_state
                        int iStateDetailVersion = Convert.ToInt32(nodeSTATE.Attributes["version"].InnerText);
                        myOVALStateDetail.OVALStateVersion = iStateDetailVersion;
                        try
                        {
                            myOVALStateDetail.comment = nodeSTATE.Attributes["comment"].InnerText;
                        }
                        catch (Exception ex)
                        {

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
                            Console.WriteLine("Unknown STATE Namespace: " + sNameSpace);
                        }
                        myOVALStateDetail.Namespace = sNameSpace;
                        model.SaveChanges();

                        if (nodeSTATE.ChildNodes.Count > 0)
                        {
                            //We need a record
                            XORCISMModel.OVALSTATERECORD myOVALStateRecord;
                            XORCISMModel.OVALSTATERECORDFOROVALSTATE myOVALStateRecordForOVALState;
                            myOVALStateRecordForOVALState = model.OVALSTATERECORDFOROVALSTATE.FirstOrDefault(o => o.OVALStateID == myOVALStateDetail.OVALStateID);
                            if (myOVALStateRecordForOVALState == null)
                            {
                                //TODO
                                //OVALSTATECOMPLEXBASE
                                myOVALStateRecord = new OVALSTATERECORD();
                                myOVALStateRecord.DataTypeName = nodeSTATE.Name;    //version_state line_state  variable_state
                                //Operation
                                //...
                                //mask
                                myOVALStateRecord.Namespace = sNameSpace;   //pixos-def
                                try
                                {
                                    model.AddToOVALSTATERECORD(myOVALStateRecord);
                                    model.SaveChanges();
                                }
                                catch (Exception exAddToOVALSTATERECORD)
                                {
                                    Console.WriteLine("Exception exAddToOVALSTATERECORD " + exAddToOVALSTATERECORD.Message + " " + exAddToOVALSTATERECORD.InnerException);
                                }

                                myOVALStateRecordForOVALState = new OVALSTATERECORDFOROVALSTATE();
                                myOVALStateRecordForOVALState.OVALStateID = myOVALStateDetail.OVALStateID;
                                myOVALStateRecordForOVALState.OVALStateRecordID = myOVALStateRecord.OVALStateRecordID;
                                try
                                {
                                    model.AddToOVALSTATERECORDFOROVALSTATE(myOVALStateRecordForOVALState);
                                    model.SaveChanges();
                                }
                                catch (Exception exAddToOVALSTATERECORDFOROVALSTATE)
                                {
                                    Console.WriteLine("Exception exAddToOVALSTATERECORDFOROVALSTATE " + exAddToOVALSTATERECORDFOROVALSTATE.Message + " " + exAddToOVALSTATERECORDFOROVALSTATE.InnerException);
                                }
                            }
                            else
                            {
                                myOVALStateRecord = myOVALStateRecordForOVALState.OVALSTATERECORD;
                            }
                            myOVALStateRecordForOVALState.timestamp = DateTimeOffset.Now;
                            foreach(XmlNode nodeSTATEFIELD in nodeSTATE)
                            {
                                //TODO
                                //We assume that we have only one OVALSTATEFIELD for one DataType for one OVALSTATERECORD
                                XORCISMModel.OVALSTATEFIELD myOVALStateField;
                                Boolean bAddOvalStateField = false;
                                XORCISMModel.OVALSTATEFIELDFOROVALSTATERECORD myOVALStateFieldForOVALStateRecord;
                                Boolean bAddOvalStateFieldForOvalStateRecord = false;
                                myOVALStateFieldForOVALStateRecord = model.OVALSTATEFIELDFOROVALSTATERECORD.FirstOrDefault(o => o.OVALStateRecordID == myOVALStateRecord.OVALStateRecordID && o.OVALSTATEFIELD.FieldName == nodeSTATEFIELD.Name);
                                if (myOVALStateFieldForOVALStateRecord == null)
                                {
                                    Console.WriteLine("Adding OVALSTATEFIELDFOROVALSTATERECORD");
                                    bAddOvalStateFieldForOvalStateRecord = true;
                                    myOVALStateFieldForOVALStateRecord = new OVALSTATEFIELDFOROVALSTATERECORD();
                                    //We add a OVALSTATEFIELD
                                    myOVALStateField = new OVALSTATEFIELD();
                                    bAddOvalStateField = true;

                                    
                                }
                                else
                                {
                                    myOVALStateField = myOVALStateFieldForOVALStateRecord.OVALSTATEFIELD;
                                }
                                //TODO
                                //OVALENTITYATTRIBUTEGROUP
                                myOVALStateField.FieldName = nodeSTATEFIELD.Name;    //pix_release   show_subcommand config_line
                                myOVALStateField.DataTypeName = "string";   // nodeSTATEFIELD.Name;
                                try
                                {
                                    myOVALStateField.DataTypeName = nodeSTATEFIELD.Attributes["datatype"].InnerText;    //int
                                }
                                catch (Exception ex)
                                {

                                }
                                try
                                {
                                    //for variable_state
                                    string sOVALVariableIDPattern = nodeSTATEFIELD.Attributes["var_ref"].InnerText;
                                    myOVALStateField.VarRef = sOVALVariableIDPattern;
                                    
                                    XORCISMModel.OVALVARIABLE myOVALVariable;
                                    myOVALVariable = model.OVALVARIABLE.FirstOrDefault(o => o.OVALVariableIDPattern == sOVALVariableIDPattern);
                                    if (myOVALVariable == null)
                                    {
                                        Console.WriteLine("Adding OVALVariable " + sOVALVariableIDPattern + " for OVALSTATEFIELD");
                                        myOVALVariable = new OVALVARIABLE();
                                        myOVALVariable.OVALVariableIDPattern = sOVALVariableIDPattern;
                                        myOVALVariable.DataTypeName = "string"; //Hardcoded default
                                        myOVALVariable.OVALVariableVersion = 1;
                                        myOVALVariable.comment = "";
                                        try
                                        {
                                            model.AddToOVALVARIABLE(myOVALVariable);
                                            model.SaveChanges();
                                        }
                                        catch (Exception exAddToOVALVARIABLE)
                                        {
                                            Console.WriteLine("Exception exAddToOVALVARIABLE " + exAddToOVALVARIABLE.Message + " " + exAddToOVALVARIABLE.InnerException);
                                        }
                                    }
                                    myOVALStateField.OVALVariableID = myOVALVariable.OVALVariableID;
                                }
                                catch (Exception ex)
                                {

                                }
                                myOVALStateField.OperationValue = "equals";
                                try
                                {
                                    myOVALStateField.OperationValue = nodeSTATEFIELD.Attributes["operation"].InnerText;
                                    //TODO
                                    //myOVALStateField.OperationEnumerationID
                                }
                                catch (Exception ex)
                                {
                                    myOVALStateField.OperationEnumerationID = 1;    //Hardcoded
                                    myOVALStateField.OperationValue = "equals";
                                }
                                try
                                {
                                    myOVALStateField.EnumerationValue = nodeSTATEFIELD.Attributes["entity_check"].InnerText;    //all
                                    //TODO
                                    //myOVALStateField.CheckEnumerationID
                                }
                                catch (Exception ex)
                                {

                                }
                                myOVALStateField.FieldValue = nodeSTATEFIELD.InnerText;
                                try
                                {
                                    if (bAddOvalStateField)
                                    {
                                        model.AddToOVALSTATEFIELD(myOVALStateField);
                                    }
                                    myOVALStateField.Namespace = sNameSpace;
                                    model.SaveChanges();
                                }
                                catch (Exception exAddToOVALSTATEFIELD)
                                {
                                    Console.WriteLine("Exception exAddToOVALSTATEFIELD " + exAddToOVALSTATEFIELD.Message + " " + exAddToOVALSTATEFIELD.InnerException);
                                }

                                myOVALStateFieldForOVALStateRecord.OVALStateRecordID = myOVALStateRecord.OVALStateRecordID;
                                myOVALStateFieldForOVALStateRecord.OVALStateFieldID = myOVALStateField.OVALStateFieldID;
                                myOVALStateFieldForOVALStateRecord.timestamp = DateTimeOffset.Now;
                                try
                                {
                                    if (bAddOvalStateFieldForOvalStateRecord)
                                    {
                                        model.AddToOVALSTATEFIELDFOROVALSTATERECORD(myOVALStateFieldForOVALStateRecord);
                                    }
                                    model.SaveChanges();
                                }
                                catch (Exception exAddToOVALSTATEFIELDFOROVALSTATERECORD)
                                {
                                    Console.WriteLine("Exception exAddToOVALSTATEFIELDFOROVALSTATERECORD " + exAddToOVALSTATEFIELDFOROVALSTATERECORD.Message + " " + exAddToOVALSTATEFIELDFOROVALSTATERECORD.InnerException);
                                }
                            }
                        }

                    }
                }
            }
            #endregion ovalstates


            #region ovalvariables
            nodes1 = doc.SelectNodes("/oval-def:oval_definitions/oval-def:variables", mgr);
            foreach (XmlNode node in nodes1)
            {
                //TODO
                foreach (XmlNode nodeVARIABLE in node)
                {
                    string sOVALVariableIDPattern = nodeVARIABLE.Attributes["id"].InnerText;
                    XORCISMModel.OVALVARIABLE myOVALVariable;
                    myOVALVariable = model.OVALVARIABLE.FirstOrDefault(o => o.OVALVariableIDPattern == sOVALVariableIDPattern); //&& o.OVALVariableVersion ==
                    if (myOVALVariable == null)
                    {
                        Console.WriteLine("ERROR OVALVariable not found " + sOVALVariableIDPattern);
                    }
                    else
                    {
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
                                Console.WriteLine("Unknown OVALVariable type " + nodeVARIABLE.Name);
                                break;
                        }
                        int iOVALVersion = Convert.ToInt32(nodeVARIABLE.Attributes["version"].InnerText);
                        myOVALVariable.OVALVariableVersion = iOVALVersion;
                        try
                        {
                            myOVALVariable.comment = nodeVARIABLE.Attributes["comment"].InnerText;
                        }
                        catch (Exception ex)
                        {

                        }
                        
                        try
                        {
                            myOVALVariable.DataTypeName = nodeVARIABLE.Attributes["datatype"].InnerText;    //evr_string    float
                        }
                        catch (Exception ex)
                        {
                            myOVALVariable.DataTypeName = "string";
                        }
                        try
                        {
                            string sVariableDeprecated=nodeVARIABLE.Attributes["deprecated"].InnerText;
                            if(sVariableDeprecated.ToLower() == "true")
                            {
                                myOVALVariable.deprecated = true;
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                        myOVALVariable.Namespace = "oval-def"; //oval-def   //TODO HARDCODED
                        myOVALVariable.VariableType = nodeVARIABLE.Name;
                        model.SaveChanges();

                        //TODO
                        //OVALCOMPONENTGROUP
                        foreach (XmlNode nodeVARIABLEDETAIL in nodeVARIABLE)
                        {
                            XORCISMModel.OVALCOMPONENTGROUP myOVALComponentGroup;
                            myOVALComponentGroup = model.OVALCOMPONENTGROUP.FirstOrDefault(o => o.OVALVariableID == myOVALVariable.OVALVariableID);
                            if (myOVALComponentGroup == null)
                            {
                                Console.WriteLine("Adding a OVALCOMPONENTGROUP for OVALVARIABLE " + myOVALVariable.OVALVariableID);
                                myOVALComponentGroup = new OVALCOMPONENTGROUP();
                                myOVALComponentGroup.OVALVariableID = myOVALVariable.OVALVariableID;
                                myOVALComponentGroup.FunctionName = nodeVARIABLEDETAIL.Name;    //concat    //TODO: review
                                try
                                {
                                    myOVALComponentGroup.FunctionOperation = nodeVARIABLE.Attributes["arithmetic_operation"].InnerText; //add   multiply
                                }
                                catch (Exception ex)
                                {

                                }
                                //DATETIME Format1 Format2
                                try
                                {
                                    model.AddToOVALCOMPONENTGROUP(myOVALComponentGroup);
                                    model.SaveChanges();
                                }
                                catch (Exception exAddToOVALCOMPONENTGROUP)
                                {
                                    Console.WriteLine("Exception exAddToOVALCOMPONENTGROUP " + exAddToOVALCOMPONENTGROUP.Message + " " + exAddToOVALCOMPONENTGROUP.InnerException);
                                }
                            }

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
                                        switch (nodeVARIABLEDETAILCOMPONENT.Name)
                                        {
                                            case "literal_component":
                                                //TODO
                                                string sDataType = "string";

                                                string sLiteralComponentValue = nodeVARIABLEDETAILCOMPONENT.InnerText;
                                                XORCISMModel.OVALLITERALCOMPONENT myOVALLiteralComponent;
                                                myOVALLiteralComponent = model.OVALLITERALCOMPONENT.FirstOrDefault(o => o.DataTypeName == sDataType && o.LiteralComponentValue == sLiteralComponentValue);
                                                if (myOVALLiteralComponent == null)
                                                {
                                                    Console.WriteLine("Adding OVALLITERALCOMPONENT " + sLiteralComponentValue);
                                                    myOVALLiteralComponent = new OVALLITERALCOMPONENT();
                                                    
                                                    myOVALLiteralComponent.DataTypeName = sDataType;
                                                    //TODO
                                                    //myOVALLiteralComponent.SimpleDataTypeID = 10  //Hardcoded string
                                                    myOVALLiteralComponent.LiteralComponentValue = sLiteralComponentValue;
                                                    try
                                                    {
                                                        model.AddToOVALLITERALCOMPONENT(myOVALLiteralComponent);
                                                        model.SaveChanges();
                                                    }
                                                    catch (Exception exAddToOVALLITERALCOMPONENT)
                                                    {
                                                        Console.WriteLine("Exception exAddToOVALLITERALCOMPONENT " + exAddToOVALLITERALCOMPONENT.Message + " " + exAddToOVALLITERALCOMPONENT.InnerException);
                                                    }
                                                }
                                                
                                                //OVALLITERALCOMPONENTFOROVALCOMPONENTGROUP
                                                XORCISMModel.OVALLITERALCOMPONENTFOROVALCOMPONENTGROUP myOVALLiteralComponentForOVALComponentGroup;
                                                myOVALLiteralComponentForOVALComponentGroup = model.OVALLITERALCOMPONENTFOROVALCOMPONENTGROUP.FirstOrDefault(o => o.OVALComponentGroupID == myOVALComponentGroup.OVALComponentGroupID && o.OVALLiteralComponentID == myOVALLiteralComponent.OVALLiteralComponentID);
                                                if (myOVALLiteralComponentForOVALComponentGroup == null)
                                                {
                                                    myOVALLiteralComponentForOVALComponentGroup = new OVALLITERALCOMPONENTFOROVALCOMPONENTGROUP();
                                                    myOVALLiteralComponentForOVALComponentGroup.OVALComponentGroupID = myOVALComponentGroup.OVALComponentGroupID;
                                                    myOVALLiteralComponentForOVALComponentGroup.OVALLiteralComponentID = myOVALLiteralComponent.OVALLiteralComponentID;
                                                    try
                                                    {
                                                        model.AddToOVALLITERALCOMPONENTFOROVALCOMPONENTGROUP(myOVALLiteralComponentForOVALComponentGroup);
                                                        //model.SaveChanges();
                                                    }
                                                    catch (Exception exAddToOVALLITERALCOMPONENTFOROVALCOMPONENTGROUP)
                                                    {
                                                        Console.WriteLine("Exception exAddToOVALLITERALCOMPONENTFOROVALCOMPONENTGROUP " + exAddToOVALLITERALCOMPONENTFOROVALCOMPONENTGROUP.Message + " " + exAddToOVALLITERALCOMPONENTFOROVALCOMPONENTGROUP.InnerException);
                                                    }

                                                }
                                                myOVALLiteralComponentForOVALComponentGroup.timestamp = DateTimeOffset.Now;
                                                model.SaveChanges();
                                                break;
                                            case "object_component":
                                                //TODO
                                                string sOVALObjectIDPattern = nodeVARIABLEDETAILCOMPONENT.Attributes["object_ref"].InnerText;
                                                string sItemField = nodeVARIABLEDETAILCOMPONENT.Attributes["item_field"].InnerText;
                                                //Console.WriteLine("Missing code for object_component");
                                                XORCISMModel.OVALOBJECTCOMPONENT myOVALObjectComponent;
                                                myOVALObjectComponent = model.OVALOBJECTCOMPONENT.FirstOrDefault(o => o.OVALObjectIDPattern == sOVALObjectIDPattern && o.OVALItemEntityName == sItemField);
                                                if (myOVALObjectComponent == null)
                                                {
                                                    Console.WriteLine("Adding OVALOBJECTCOMPONENT " + sOVALObjectIDPattern + " " + sItemField);
                                                    myOVALObjectComponent = new OVALOBJECTCOMPONENT();
                                                    //OVALObjectID
                                                    XORCISMModel.OVALOBJECT myOVALObjectForComponent;
                                                    myOVALObjectForComponent = model.OVALOBJECT.FirstOrDefault(o => o.OVALObjectIDPattern == sOVALObjectIDPattern);
                                                    if (myOVALObjectForComponent == null)
                                                    {
                                                        Console.WriteLine("ERROR OVALOBJECT " + sOVALObjectIDPattern + "not found for OVALOBJECTCOMPONENT");
                                                    }
                                                    myOVALObjectComponent.OVALObjectID = myOVALObjectForComponent.OVALObjectID;
                                                    myOVALObjectComponent.OVALObjectIDPattern = sOVALObjectIDPattern;
                                                    myOVALObjectComponent.OVALItemEntityName = sItemField;
                                                    try
                                                    {
                                                        model.AddToOVALOBJECTCOMPONENT(myOVALObjectComponent);
                                                        model.SaveChanges();
                                                    }
                                                    catch (Exception exAddToOVALOBJECTCOMPONENT)
                                                    {
                                                        Console.WriteLine("Exception exAddToOVALOBJECTCOMPONENT " + exAddToOVALOBJECTCOMPONENT.Message + " " + exAddToOVALOBJECTCOMPONENT.InnerException);
                                                    }
                                                }

                                                XORCISMModel.OVALOBJECTCOMPONENTFOROVALCOMPONENTGROUP myOVALObjectComponentForOVALComponentGroup;
                                                myOVALObjectComponentForOVALComponentGroup = model.OVALOBJECTCOMPONENTFOROVALCOMPONENTGROUP.FirstOrDefault(o => o.OVALComponentGroupID == myOVALComponentGroup.OVALComponentGroupID && o.OVALObjectComponentID == myOVALObjectComponent.OVALObjectComponentID);
                                                if (myOVALObjectComponentForOVALComponentGroup == null)
                                                {
                                                    myOVALObjectComponentForOVALComponentGroup = new OVALOBJECTCOMPONENTFOROVALCOMPONENTGROUP();
                                                    myOVALObjectComponentForOVALComponentGroup.OVALComponentGroupID = myOVALComponentGroup.OVALComponentGroupID;
                                                    myOVALObjectComponentForOVALComponentGroup.OVALObjectComponentID = myOVALObjectComponent.OVALObjectComponentID;
                                                    try
                                                    {
                                                        model.AddToOVALOBJECTCOMPONENTFOROVALCOMPONENTGROUP(myOVALObjectComponentForOVALComponentGroup);
                                                        //model.SaveChanges();
                                                    }
                                                    catch (Exception exAddToOVALOBJECTCOMPONENTFOROVALCOMPONENTGROUP)
                                                    {
                                                        Console.WriteLine("Exception exAddToOVALOBJECTCOMPONENTFOROVALCOMPONENTGROUP " + exAddToOVALOBJECTCOMPONENTFOROVALCOMPONENTGROUP.Message + " " + exAddToOVALOBJECTCOMPONENTFOROVALCOMPONENTGROUP.InnerException);
                                                    }
                                                }
                                                myOVALObjectComponentForOVALComponentGroup.timestamp = DateTimeOffset.Now;
                                                model.SaveChanges();
                                                break;
                                            default:
                                                Console.WriteLine("Missing code for VARIABLEDETAILCOMPONENT " + nodeVARIABLEDETAILCOMPONENT.Name);
                                                break;
                                        }
                                    }
                                    #endregion concat
                                    break;
                                case "value":
                                    //TODO
                                    string sVariableValue = nodeVARIABLEDETAIL.InnerText;
                                    XORCISMModel.OVALVARIABLEVALUE myOVALVariableValue;
                                    myOVALVariableValue = model.OVALVARIABLEVALUE.FirstOrDefault(o => o.OVALVariableID == myOVALVariable.OVALVariableID && o.VALUE.ValueValue == sVariableValue);
                                    if (myOVALVariableValue == null)
                                    {
                                        Console.WriteLine("Adding OVALVARIABLEVALUE");
                                        myOVALVariableValue = new OVALVARIABLEVALUE();

                                        XORCISMModel.VALUE myValue;
                                        myValue = model.VALUE.FirstOrDefault(o => o.ValueValue == sVariableValue);
                                        if (myValue == null)
                                        {
                                            Console.WriteLine("Adding VALUE");
                                            myValue = new VALUE();
                                            myValue.ValueValue = sVariableValue;
                                            try
                                            {
                                                model.AddToVALUE(myValue);
                                                model.SaveChanges();
                                            }
                                            catch (Exception exAddToVALUE)
                                            {
                                                Console.WriteLine("Exception exAddToVALUE " + exAddToVALUE.Message + " " + exAddToVALUE.InnerException);
                                            }
                                        }
                                        myOVALVariableValue.OVALVariableID = myOVALVariable.OVALVariableID;
                                        myOVALVariableValue.ValueID = myValue.ValueID;
                                        try
                                        {
                                            model.AddToOVALVARIABLEVALUE(myOVALVariableValue);
                                            model.SaveChanges();
                                        }
                                        catch (Exception exAddToOVALVARIABLEVALUE)
                                        {
                                            Console.WriteLine("Exception exAddToOVALVARIABLEVALUE " + exAddToOVALVARIABLEVALUE.Message + " " + exAddToOVALVARIABLEVALUE.InnerException);
                                        }
                                    }
                                    break;
                                default:
                                    Console.WriteLine("Missing code for variables " + nodeVARIABLEDETAIL.Name);
                                    break;
                            }
                        }
                    }
                }
            }
            #endregion ovalvariables

            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
        }

        public static void OVALParseCriteria(XORCISMEntities model, XmlNode node2, XORCISMModel.OVALCRITERIA myCriteria)
        {
            int iCptCriteria = 0;
            foreach (XmlNode nodeCriterion in node2.ChildNodes)
            {
                if (nodeCriterion.Name == "criteria")
                {
                    iCptCriteria++; //TODO: This is not perfect
                    //TODO
                    //OVALCRITERIAFOROVALCRITERIA
                    XORCISMModel.OVALCRITERIA myCriteriaSubject;
                    XORCISMModel.OVALCRITERIAFOROVALCRITERIA myCriteriaForCriteria;
                    myCriteriaForCriteria = model.OVALCRITERIAFOROVALCRITERIA.FirstOrDefault(o => o.OVALCriteriaRefID == myCriteria.OVALCriteriaID && o.CriteriaRank == iCptCriteria);
                    if (myCriteriaForCriteria == null)
                    {
                        Console.WriteLine("Adding new CRITERIA for CRITERIA");
                        myCriteriaSubject = new OVALCRITERIA();
                        myCriteriaSubject.OperatorValue = nodeCriterion.Attributes["operator"].InnerText;
                        //TODO: OperatorEnumerationID
                        try
                        {
                            myCriteriaSubject.comment = nodeCriterion.Attributes["comment"].InnerText;
                        }
                        catch (Exception ex)
                        {

                        }
                        //TODO
                        //negate
                        //applicabilitycheck
                        try
                        {
                            model.AddToOVALCRITERIA(myCriteriaSubject);
                            model.SaveChanges();
                        }
                        catch (Exception exAddToOVALCRITERIA)
                        {
                            Console.WriteLine("Exception exAddToOVALCRITERIA " + exAddToOVALCRITERIA.Message + " " + exAddToOVALCRITERIA.InnerException);
                        }

                        Console.WriteLine("Adding OVALCRITERIAFOROVALCRITERIA");
                        myCriteriaForCriteria = new OVALCRITERIAFOROVALCRITERIA();
                        myCriteriaForCriteria.OVALCriteriaRefID = myCriteria.OVALCriteriaID;
                        myCriteriaForCriteria.OVALCriteriaSubjectID = myCriteriaSubject.OVALCriteriaID;
                        myCriteriaForCriteria.CriteriaRank = iCptCriteria;
                        try
                        {
                            model.AddToOVALCRITERIAFOROVALCRITERIA(myCriteriaForCriteria);
                            model.SaveChanges();
                        }
                        catch (Exception exAddToOVALCRITERIAFOROVALCRITERIA)
                        {
                            Console.WriteLine("Exception exAddToOVALCRITERIAFOROVALCRITERIA " + exAddToOVALCRITERIAFOROVALCRITERIA.Message + " " + exAddToOVALCRITERIAFOROVALCRITERIA.InnerException);
                        }
                    }
                    else
                    {
                        myCriteriaSubject = model.OVALCRITERIA.FirstOrDefault(o => o.OVALCriteriaID == myCriteriaForCriteria.OVALCriteriaSubjectID);
                    }
                    myCriteriaForCriteria.timestamp = DateTimeOffset.Now;
                    model.SaveChanges();

                    //****************************************
                    OVALParseCriteria(model, nodeCriterion, myCriteriaSubject);

                    #region criterions1
                    //foreach (XmlNode nodeCriterionCC in nodeCriterion.ChildNodes)
                    //{
                    //    //TODO: this is "duplicate" code
                    //    if (nodeCriterionCC.Name == "criterion")
                    //    {
                    //        #region criterion
                    //        string sTestRef = nodeCriterionCC.Attributes["test_ref"].InnerText;
                    //        XORCISMModel.OVALCRITERION myCriterion;
                    //        myCriterion = model.OVALCRITERION.FirstOrDefault(o => o.OVALTestIDPattern == sTestRef); //&& comment=
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
                    //        myOVALCriterionForOVALCriteria = model.OVALCRITERIONFOROVALCRITERIA.FirstOrDefault(o => o.OVALCriteriaID == myCriteriaSubject.OVALCriteriaID && o.OVALCriterionID == myCriterion.OVALCriterionID);
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
                    //            myOVALExtendedDef = model.OVALDEFINITION.FirstOrDefault(o => o.OVALDefinitionIDPattern == sDefinitionExtendedRefID);
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
                    //            myOVALExtendDef = model.OVALEXTENDDEFINITION.FirstOrDefault(o => o.OVALDefinitionIDPattern == sDefinitionExtendedRefID);    //&& o.OVALDefinitionID == myOVALExtendedDef.OVALDefinitionID
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
                        XORCISMModel.OVALCRITERION myCriterion;
                        myCriterion = model.OVALCRITERION.FirstOrDefault(o => o.OVALTestIDPattern == sTestRef);
                        if (myCriterion == null)
                        {
                            Console.WriteLine(string.Format("Adding new OVALCRITERION [{0}] in table OVALCRITERION", sTestRef));
                            myCriterion = new OVALCRITERION();
                            myCriterion.OVALTestIDPattern = sTestRef;
                            //TODO: check if the test exists
                            myCriterion.negate = false;
                            try
                            {
                                if (nodeCriterion.Attributes["negate"].InnerText.ToLower() == "true")
                                {
                                    myCriterion.negate = true;
                                }
                            }
                            catch (Exception ex)
                            {

                            }

                            myCriterion.comment = nodeCriterion.Attributes["comment"].InnerText;
                            //myCriterion.applicabilitycheck;
                            try
                            {
                                model.AddToOVALCRITERION(myCriterion);
                                model.SaveChanges();
                            }
                            catch (Exception exAddToOVALCRITERION)
                            {
                                Console.WriteLine("Exception exAddToOVALCRITERION " + exAddToOVALCRITERION.Message + " " + exAddToOVALCRITERION.InnerException);
                            }
                        }

                        //OVALCRITERIONFOROVALCRITERIA
                        XORCISMModel.OVALCRITERIONFOROVALCRITERIA myOVALCriterionForOVALCriteria;
                        myOVALCriterionForOVALCriteria = model.OVALCRITERIONFOROVALCRITERIA.FirstOrDefault(o => o.OVALCriteriaID == myCriteria.OVALCriteriaID && o.OVALCriterionID == myCriterion.OVALCriterionID);
                        if (myOVALCriterionForOVALCriteria == null)
                        {
                            Console.WriteLine("Adding OVALCRITERIONFOROVALCRITERIA " + myCriteria.OVALCriteriaID +" - "+myCriterion.comment);
                            myOVALCriterionForOVALCriteria = new OVALCRITERIONFOROVALCRITERIA();
                            myOVALCriterionForOVALCriteria.OVALCriteriaID = myCriteria.OVALCriteriaID;
                            myOVALCriterionForOVALCriteria.OVALCriterionID = myCriterion.OVALCriterionID;
                            try
                            {
                                model.AddToOVALCRITERIONFOROVALCRITERIA(myOVALCriterionForOVALCriteria);
                                model.SaveChanges();
                            }
                            catch (Exception exAddToOVALCRITERIONFOROVALCRITERIA)
                            {
                                Console.WriteLine("Exception exAddToOVALCRITERIONFOROVALCRITERIA " + exAddToOVALCRITERIONFOROVALCRITERIA.Message + " " + exAddToOVALCRITERIONFOROVALCRITERIA.InnerException);
                            }
                        }
                        #endregion criterion
                    }
                    else
                    {
                        if (nodeCriterion.Name == "extend_definition")
                        {
                            #region extenddefinition
                            string sDefinitionExtendedRefID = nodeCriterion.Attributes["definition_ref"].InnerText;
                            XORCISMModel.OVALDEFINITION myOVALExtendedDef;
                            myOVALExtendedDef = model.OVALDEFINITION.FirstOrDefault(o => o.OVALDefinitionIDPattern == sDefinitionExtendedRefID);
                            if (myOVALExtendedDef == null)
                            {
                                Console.WriteLine("Import_oval the extended definition " + sDefinitionExtendedRefID + " does not exist");
                                Console.WriteLine("Adding new OVALDEFINITION " + sDefinitionExtendedRefID);
                                myOVALExtendedDef = new OVALDEFINITION();
                                myOVALExtendedDef.OVALDefinitionIDPattern = sDefinitionExtendedRefID;
                                //TODO: incomplete, will crash
                                try
                                {
                                    model.AddToOVALDEFINITION(myOVALExtendedDef);
                                    model.SaveChanges();
                                }
                                catch (Exception exAddToOVALDEFINITION)
                                {
                                    Console.WriteLine("Exception exAddToOVALDEFINITION " + exAddToOVALDEFINITION.Message + " " + exAddToOVALDEFINITION.InnerException);
                                }
                            }

                            XORCISMModel.OVALEXTENDDEFINITION myOVALExtendDef;
                            myOVALExtendDef = model.OVALEXTENDDEFINITION.FirstOrDefault(o => o.OVALDefinitionIDPattern == sDefinitionExtendedRefID);    //&& o.OVALDefinitionID == myOVALExtendedDef.OVALDefinitionID
                            if (myOVALExtendDef == null)
                            {
                                Console.WriteLine("Adding new OVALEXTENDDEFINITION " + sDefinitionExtendedRefID);
                                myOVALExtendDef = new OVALEXTENDDEFINITION();
                                myOVALExtendDef.OVALDefinitionID = myOVALExtendedDef.OVALDefinitionID;
                                myOVALExtendDef.OVALDefinitionIDPattern = sDefinitionExtendedRefID;
                                //myOVALExtendDef.negate
                                myOVALExtendDef.comment = nodeCriterion.Attributes["comment"].InnerText;
                                //myOVALExtendDef.applicabilitycheck
                                try
                                {
                                    model.AddToOVALEXTENDDEFINITION(myOVALExtendDef);
                                    model.SaveChanges();
                                }
                                catch (Exception exAddToOVALEXTENDDEFINITION)
                                {
                                    Console.WriteLine("Exception exAddToOVALEXTENDDEFINITION " + exAddToOVALEXTENDDEFINITION.Message + " " + exAddToOVALEXTENDDEFINITION.InnerException);
                                }
                            }

                            //TODO
                            //XORCISMModel.OVALEXTENDDEFINITIONFORCRITERIA

                            #endregion extenddefinition
                        }
                        else
                        {
                            Console.WriteLine("Import_oval missing code for definition criteria2 : " + nodeCriterion.Name);
                        }
                    }
                    #endregion criterions2
                }
            }
        }
    }
}
