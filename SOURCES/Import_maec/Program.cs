using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

using XORCISMModel;
using XMALWAREModel;

namespace Import_maec
{
    static class Program
    {
        /// <summary>
        /// Copyright (C) 2014-2015 Jerome Athias
        /// Import the MITRE MAEC default vocabularies enumeration values in an XORCISM database
        /// This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation; either version 2 of the License, or (at your option) any later version.
        /// 
        /// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
        /// 
        /// You should have received a copy of the GNU General Public License along with this program; if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
        /// </summary>
        [STAThread]
        static void Main()
        {
            string sMAECVersion = "4.1";    //HARDCODED

            XORCISMEntities model= new XORCISMEntities();
            //https://stackoverflow.com/questions/5940225/fastest-way-of-inserting-in-entity-framework
            model.Configuration.AutoDetectChangesEnabled = false;
            model.Configuration.ValidateOnSaveEnabled = false;

            XMALWAREEntities malware_model = new XMALWAREEntities();
            malware_model.Configuration.AutoDetectChangesEnabled = false;
            malware_model.Configuration.ValidateOnSaveEnabled = false;


            int iVocabularyMAECID = 0;  // 12;  //MAEC
            #region vocabularymaec
            try
            {
                iVocabularyMAECID = model.VOCABULARY.Where(o => o.VocabularyName == "MAEC" && o.VocabularyVersion == sMAECVersion).Select(o => o.VocabularyID).FirstOrDefault();
            }
            catch (Exception ex)
            {

            }
            if (iVocabularyMAECID <= 0)
            {
                VOCABULARY oVocabulary = new VOCABULARY();
                oVocabulary.CreatedDate = DateTimeOffset.Now;
                oVocabulary.timestamp=DateTimeOffset.Now;
                oVocabulary.VocabularyName = "MAEC";    //HARDCODED
                oVocabulary.VocabularyVersion = sMAECVersion;
                model.VOCABULARY.Add(oVocabulary);
                model.SaveChanges();
                iVocabularyMAECID = oVocabulary.VocabularyID;
                Console.WriteLine("DEBUG iVocabularyMAECID=" + iVocabularyMAECID);
            }
            #endregion vocabularymaec

            //TODO
            //Download the MAEC vocabularies


            XmlDocument doc;
            doc = new XmlDocument();
            doc.Load(@"C:\nvdcve\maec_default_vocabularies.xsd");   //TODO HARDCODED!!!

            XmlNamespaceManager mgr = new XmlNamespaceManager(doc.NameTable);

            mgr.AddNamespace("xs", "http://www.w3.org/2001/XMLSchema");

            XmlNodeList nodes1;
            nodes1 = doc.SelectNodes("/xs:schema/xs:simpleType", mgr);
            Console.WriteLine(nodes1.Count);

            

            foreach (XmlNode node in nodes1)    //enumeration
            {
                //Console.WriteLine("DEBUG node.Name="+node.Name);   //xs:simpleType
                string sNodeName = node.Attributes["name"].InnerText;
                Console.WriteLine("DEBUG sNodeName="+sNodeName);


                if (sNodeName.Contains("Enum-"))
                {
                    bool bEnumerationProcessed = false;
                    //Get the EnumerationName and Version
                    //
                    string[] words = sNodeName.Split('-');
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
                        oVersion.VocabularyID = iVocabularyMAECID;
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
                        oEnumerationVersion.VocabularyID = iVocabularyMAECID;
                        model.ENUMERATIONVERSION.Add(oEnumerationVersion);
                        model.SaveChanges();
                        iEnumerationVersionID = oEnumerationVersion.EnumerationVersionID;
                    }
                    else
                    {
                        //Update ENUMERATIONVERSION
                    }

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("ActionObjectAssociationTypeEnum"))
                    {
                        #region actionobjectassociation
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.ACTIONOBJECTASSOCIATIONTYPE myActionObjectAssociationType;
                                    myActionObjectAssociationType = model.ACTIONOBJECTASSOCIATIONTYPE.FirstOrDefault(o => o.ActionObjectAssociationTypeName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (myActionObjectAssociationType == null)
                                    {
                                        Console.WriteLine("Adding new ACTIONOBJECTASSOCIATIONTYPE " + sEnumerationValue);
                                        myActionObjectAssociationType = new ACTIONOBJECTASSOCIATIONTYPE();
                                        myActionObjectAssociationType.ActionObjectAssociationTypeName = sEnumerationValue;
                                        myActionObjectAssociationType.VocabularyID = iVocabularyMAECID;
                                        myActionObjectAssociationType.CreatedDate = DateTimeOffset.Now;
                                        myActionObjectAssociationType.timestamp = DateTimeOffset.Now;
                                        myActionObjectAssociationType.EnumerationVersionID = iEnumerationVersionID;
                                        //myActionObjectAssociationType.ValidFromDate=;
                                        model.ACTIONOBJECTASSOCIATIONTYPE.Add(myActionObjectAssociationType);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                myActionObjectAssociationType.ActionObjectAssociationTypeDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion actionobjectassociation
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("AntiBehavioralAnalysisPropertiesEnum"))
                    {
                        #region antibehavioranalysis
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.ANTIBEHAVIORANALYSISPROPERTIES oAntiBehavioralAnalysis;
                                    oAntiBehavioralAnalysis = model.ANTIBEHAVIORANALYSISPROPERTIES.FirstOrDefault(o => o.AntiBehavioralAnalysisPropertiesName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (oAntiBehavioralAnalysis == null)
                                    {
                                        Console.WriteLine("Adding new ANTIBEHAVIORANALYSISPROPERTIES " + sEnumerationValue);
                                        oAntiBehavioralAnalysis = new ANTIBEHAVIORANALYSISPROPERTIES();
                                        oAntiBehavioralAnalysis.AntiBehavioralAnalysisPropertiesName = sEnumerationValue;
                                        oAntiBehavioralAnalysis.VocabularyID = iVocabularyMAECID;
                                        oAntiBehavioralAnalysis.CreatedDate = DateTimeOffset.Now;
                                        oAntiBehavioralAnalysis.timestamp = DateTimeOffset.Now;
                                        oAntiBehavioralAnalysis.EnumerationVersionID = iEnumerationVersionID;
                                        //oAntiBehavioralAnalysis.ValidFromDate=;
                                        model.ANTIBEHAVIORANALYSISPROPERTIES.Add(oAntiBehavioralAnalysis);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                oAntiBehavioralAnalysis.AntiBehavioralAnalysisPropertiesDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion antibehavioranalysis
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("AntiBehavioralAnalysisStrategicObjectivesEnum"))
                    {
                        #region antibehavioranalysisstrategic
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.ANTIBEHAVIORALANALYSISSTRATEGICOBJECTIVE oAntiBehavioralAnalysisStrategic;
                                    oAntiBehavioralAnalysisStrategic = model.ANTIBEHAVIORALANALYSISSTRATEGICOBJECTIVE.FirstOrDefault(o => o.AntiBehavioralAnalysisStrategicObjectiveName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (oAntiBehavioralAnalysisStrategic == null)
                                    {
                                        Console.WriteLine("Adding new ANTIBEHAVIORALANALYSISSTRATEGICOBJECTIVE " + sEnumerationValue);
                                        oAntiBehavioralAnalysisStrategic = new ANTIBEHAVIORALANALYSISSTRATEGICOBJECTIVE();
                                        oAntiBehavioralAnalysisStrategic.AntiBehavioralAnalysisStrategicObjectiveName = sEnumerationValue;
                                        oAntiBehavioralAnalysisStrategic.VocabularyID = iVocabularyMAECID;
                                        oAntiBehavioralAnalysisStrategic.CreatedDate = DateTimeOffset.Now;
                                        oAntiBehavioralAnalysisStrategic.timestamp = DateTimeOffset.Now;
                                        oAntiBehavioralAnalysisStrategic.EnumerationVersionID = iEnumerationVersionID;
                                        //oAntiBehavioralAnalysisStrategic.ValidFromDate=;
                                        model.ANTIBEHAVIORALANALYSISSTRATEGICOBJECTIVE.Add(oAntiBehavioralAnalysisStrategic);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                oAntiBehavioralAnalysisStrategic.AntiBehavioralAnalysisStrategicObjectiveDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion antibehavioranalysisstrategic
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("AntiBehavioralAnalysisTacticalObjectivesEnum"))
                    {
                        #region antibehavioranalysistactical
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.ANTIBEHAVIORALANALYSISTACTICALOBJECTIVE oAntiBehavioralAnalysisTactical;
                                    oAntiBehavioralAnalysisTactical = model.ANTIBEHAVIORALANALYSISTACTICALOBJECTIVE.FirstOrDefault(o => o.AntiBehavioralAnalysisTacticalObjectiveName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (oAntiBehavioralAnalysisTactical == null)
                                    {
                                        Console.WriteLine("Adding new ANTIBEHAVIORALANALYSISTACTICALOBJECTIVE " + sEnumerationValue);
                                        oAntiBehavioralAnalysisTactical = new ANTIBEHAVIORALANALYSISTACTICALOBJECTIVE();
                                        oAntiBehavioralAnalysisTactical.AntiBehavioralAnalysisTacticalObjectiveName = sEnumerationValue;
                                        oAntiBehavioralAnalysisTactical.VocabularyID = iVocabularyMAECID;
                                        oAntiBehavioralAnalysisTactical.CreatedDate = DateTimeOffset.Now;
                                        oAntiBehavioralAnalysisTactical.timestamp = DateTimeOffset.Now;
                                        oAntiBehavioralAnalysisTactical.EnumerationVersionID = iEnumerationVersionID;
                                        //oAntiBehavioralAnalysisTactical.ValidFromDate=;
                                        model.ANTIBEHAVIORALANALYSISTACTICALOBJECTIVE.Add(oAntiBehavioralAnalysisTactical);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                oAntiBehavioralAnalysisTactical.AntiBehavioralAnalysisTacticalObjectiveDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion antibehavioranalysistactical
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("AntiCodeAnalysisStrategicObjectivesEnum"))
                    {
                        #region anticodeanalysisstrategic
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.ANTICODEANALYSISSTRATEGICOBJECTIVE oAntiCodeAnalysisStrategic;
                                    oAntiCodeAnalysisStrategic = model.ANTICODEANALYSISSTRATEGICOBJECTIVE.FirstOrDefault(o => o.AntiCodeAnalysisStrategicObjectiveName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (oAntiCodeAnalysisStrategic == null)
                                    {
                                        Console.WriteLine("Adding new ANTICODEANALYSISSTRATEGICOBJECTIVE " + sEnumerationValue);
                                        oAntiCodeAnalysisStrategic = new ANTICODEANALYSISSTRATEGICOBJECTIVE();
                                        oAntiCodeAnalysisStrategic.AntiCodeAnalysisStrategicObjectiveName = sEnumerationValue;
                                        oAntiCodeAnalysisStrategic.VocabularyID = iVocabularyMAECID;
                                        oAntiCodeAnalysisStrategic.CreatedDate = DateTimeOffset.Now;
                                        oAntiCodeAnalysisStrategic.timestamp = DateTimeOffset.Now;
                                        oAntiCodeAnalysisStrategic.EnumerationVersionID = iEnumerationVersionID;
                                        //oAntiCodeAnalysisStrategic.ValidFromDate=;
                                        model.ANTICODEANALYSISSTRATEGICOBJECTIVE.Add(oAntiCodeAnalysisStrategic);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                oAntiCodeAnalysisStrategic.AntiCodeAnalysisStrategicObjectiveDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion anticodeanalysisstrategic
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("AntiCodeAnalysisTacticalObjectivesEnum"))
                    {
                        #region anticodeanalysistactical
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    try
                                    {
                                        string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                        XORCISMModel.ANTICODEANALYSISTACTICALOBJECTIVE oAntiCodeAnalysisTactical;
                                        oAntiCodeAnalysisTactical = model.ANTICODEANALYSISTACTICALOBJECTIVE.FirstOrDefault(o => o.AntiCodeAnalysisTacticalObjectiveName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                        if (oAntiCodeAnalysisTactical == null)
                                        {
                                            Console.WriteLine("Adding new ANTICODEANALYSISTACTICALOBJECTIVE " + sEnumerationValue);
                                            oAntiCodeAnalysisTactical = new ANTICODEANALYSISTACTICALOBJECTIVE();
                                            oAntiCodeAnalysisTactical.AntiCodeAnalysisTacticalObjectiveName = sEnumerationValue;
                                            oAntiCodeAnalysisTactical.VocabularyID = iVocabularyMAECID;
                                            oAntiCodeAnalysisTactical.CreatedDate = DateTimeOffset.Now;
                                            oAntiCodeAnalysisTactical.timestamp = DateTimeOffset.Now;
                                            oAntiCodeAnalysisTactical.EnumerationVersionID = iEnumerationVersionID;
                                            //oAntiCodeAnalysisTactical.ValidFromDate=;
                                            model.ANTICODEANALYSISTACTICALOBJECTIVE.Add(oAntiCodeAnalysisTactical);
                                            model.SaveChanges();
                                        }

                                        foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                        {
                                            foreach (XmlNode node4 in node3)  //xs:documentation
                                            {
                                                if (node4.Name == "xs:documentation")
                                                {
                                                    oAntiCodeAnalysisTactical.AntiCodeAnalysisTacticalObjectiveDescription = node4.InnerText;
                                                    model.SaveChanges();
                                                }
                                            }
                                        }
                                    }
                                    catch(Exception exANTICODEANALYSISTACTICALOBJECTIVE)
                                    {
                                        Console.WriteLine("Exception exANTICODEANALYSISTACTICALOBJECTIVE " + exANTICODEANALYSISTACTICALOBJECTIVE.Message + " " + exANTICODEANALYSISTACTICALOBJECTIVE.InnerException);
                                    }
                                }
                            }

                        }
                        #endregion anticodeanalysistactical
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("AntiDetectionStrategicObjectivesEnum"))
                    {
                        #region antidetectionstrategic
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.ANTIDETECTIONSTRATEGICOBJECTIVE oAntiDetectionStrategic;
                                    oAntiDetectionStrategic = model.ANTIDETECTIONSTRATEGICOBJECTIVE.FirstOrDefault(o => o.AntiDetectionStrategicObjectiveName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (oAntiDetectionStrategic == null)
                                    {
                                        Console.WriteLine("Adding new ANTIDETECTIONSTRATEGICOBJECTIVE " + sEnumerationValue);
                                        oAntiDetectionStrategic = new ANTIDETECTIONSTRATEGICOBJECTIVE();
                                        oAntiDetectionStrategic.AntiDetectionStrategicObjectiveName = sEnumerationValue;
                                        oAntiDetectionStrategic.VocabularyID = iVocabularyMAECID;
                                        oAntiDetectionStrategic.CreatedDate = DateTimeOffset.Now;
                                        oAntiDetectionStrategic.timestamp = DateTimeOffset.Now;
                                        oAntiDetectionStrategic.EnumerationVersionID = iEnumerationVersionID;
                                        //oAntiDetectionStrategic.ValidFromDate=;
                                        model.ANTIDETECTIONSTRATEGICOBJECTIVE.Add(oAntiDetectionStrategic);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                oAntiDetectionStrategic.AntiDetectionStrategicObjectiveDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion antidetectionstrategic
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("AntiDetectionTacticalObjectivesEnum"))
                    {
                        #region antidetectiontactical
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    try
                                    {
                                        string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                        XORCISMModel.ANTIDETECTIONTACTICALOBJECTIVE oAntiDetectionTactical;
                                        oAntiDetectionTactical = model.ANTIDETECTIONTACTICALOBJECTIVE.FirstOrDefault(o => o.AntiDetectionTacticalObjectiveName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                        if (oAntiDetectionTactical == null)
                                        {
                                            Console.WriteLine("Adding new ANTIDETECTIONTACTICALOBJECTIVE " + sEnumerationValue);
                                            oAntiDetectionTactical = new ANTIDETECTIONTACTICALOBJECTIVE();
                                            oAntiDetectionTactical.AntiDetectionTacticalObjectiveName = sEnumerationValue;
                                            oAntiDetectionTactical.VocabularyID = iVocabularyMAECID;
                                            oAntiDetectionTactical.CreatedDate = DateTimeOffset.Now;
                                            oAntiDetectionTactical.timestamp = DateTimeOffset.Now;
                                            oAntiDetectionTactical.EnumerationVersionID = iEnumerationVersionID;
                                            //oAntiDetectionTactical.ValidFromDate=;
                                            model.ANTIDETECTIONTACTICALOBJECTIVE.Add(oAntiDetectionTactical);
                                            model.SaveChanges();
                                        }

                                        foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                        {
                                            foreach (XmlNode node4 in node3)  //xs:documentation
                                            {
                                                if (node4.Name == "xs:documentation")
                                                {
                                                    oAntiDetectionTactical.AntiDetectionTacticalObjectiveDescription = node4.InnerText;
                                                    model.SaveChanges();
                                                }
                                            }
                                        }
                                    }
                                    catch(Exception exANTIDETECTIONTACTICALOBJECTIVE)
                                    {
                                        Console.WriteLine("Exception exANTIDETECTIONTACTICALOBJECTIVE " + exANTIDETECTIONTACTICALOBJECTIVE.Message + " " + exANTIDETECTIONTACTICALOBJECTIVE.InnerException);
                                    }
                                }
                            }

                        }
                        #endregion antidetectiontactical
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("AntiRemovalStrategicObjectivesEnum"))
                    {
                        #region antiremovalstrategic
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    try
                                    {
                                        string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                        XORCISMModel.ANTIREMOVALSTRATEGICOBJECTIVE oAntiRemovalStrategic;
                                        oAntiRemovalStrategic = model.ANTIREMOVALSTRATEGICOBJECTIVE.FirstOrDefault(o => o.AntiRemovalStrategicObjectiveName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                        if (oAntiRemovalStrategic == null)
                                        {
                                            Console.WriteLine("Adding new ANTIREMOVALSTRATEGICOBJECTIVE " + sEnumerationValue);
                                            oAntiRemovalStrategic = new ANTIREMOVALSTRATEGICOBJECTIVE();
                                            oAntiRemovalStrategic.AntiRemovalStrategicObjectiveName = sEnumerationValue;
                                            oAntiRemovalStrategic.VocabularyID = iVocabularyMAECID;
                                            oAntiRemovalStrategic.CreatedDate = DateTimeOffset.Now;
                                            oAntiRemovalStrategic.timestamp = DateTimeOffset.Now;
                                            oAntiRemovalStrategic.EnumerationVersionID = iEnumerationVersionID;
                                            //oAntiRemovalStrategic.ValidFromDate=;
                                            model.ANTIREMOVALSTRATEGICOBJECTIVE.Add(oAntiRemovalStrategic);
                                            model.SaveChanges();
                                        }

                                        foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                        {
                                            foreach (XmlNode node4 in node3)  //xs:documentation
                                            {
                                                if (node4.Name == "xs:documentation")
                                                {
                                                    oAntiRemovalStrategic.AntiRemovalStrategicObjectiveDescription = node4.InnerText;
                                                    model.SaveChanges();
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception exANTIREMOVALSTRATEGICOBJECTIVE)
                                    {
                                        Console.WriteLine("Exception exANTIREMOVALSTRATEGICOBJECTIVE " + exANTIREMOVALSTRATEGICOBJECTIVE.Message + " " + exANTIREMOVALSTRATEGICOBJECTIVE.InnerException);
                                    }
                                }
                            }

                        }
                        #endregion antiremovalstrategic
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("AntiRemovalTacticalObjectivesEnum"))
                    {
                        #region antiremovaltactical
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    try
                                    {
                                        string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                        XORCISMModel.ANTIREMOVALTACTICALOBJECTIVE oAntiRemovalTactical;
                                        oAntiRemovalTactical = model.ANTIREMOVALTACTICALOBJECTIVE.FirstOrDefault(o => o.AntiRemovalTacticalObjectiveName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                        if (oAntiRemovalTactical == null)
                                        {
                                            Console.WriteLine("Adding new ANTIREMOVALTACTICALOBJECTIVE " + sEnumerationValue);
                                            oAntiRemovalTactical = new ANTIREMOVALTACTICALOBJECTIVE();
                                            oAntiRemovalTactical.AntiRemovalTacticalObjectiveName = sEnumerationValue;
                                            oAntiRemovalTactical.VocabularyID = iVocabularyMAECID;
                                            oAntiRemovalTactical.CreatedDate = DateTimeOffset.Now;
                                            oAntiRemovalTactical.timestamp = DateTimeOffset.Now;
                                            oAntiRemovalTactical.EnumerationVersionID = iEnumerationVersionID;
                                            //oAntiRemovalTactical.ValidFromDate=;
                                            model.ANTIREMOVALTACTICALOBJECTIVE.Add(oAntiRemovalTactical);
                                            model.SaveChanges();
                                        }

                                        foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                        {
                                            foreach (XmlNode node4 in node3)  //xs:documentation
                                            {
                                                if (node4.Name == "xs:documentation")
                                                {
                                                    oAntiRemovalTactical.AntiRemovalTacticalObjectiveDescription = node4.InnerText;
                                                    model.SaveChanges();
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception exANTIREMOVALTACTICALOBJECTIVE)
                                    {
                                        Console.WriteLine("Exception exANTIREMOVALTACTICALOBJECTIVE " + exANTIREMOVALTACTICALOBJECTIVE.Message + " " + exANTIREMOVALTACTICALOBJECTIVE.InnerException);
                                    }
                                }
                            }

                        }
                        #endregion antiremovaltactical
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("AvailabilityViolationPropertiesEnum"))
                    {
                        #region availabilityviolation
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.AVAILABILITYVIOLATIONPROPERTIES oAvailabilityViolationProperty;
                                    oAvailabilityViolationProperty = model.AVAILABILITYVIOLATIONPROPERTIES.FirstOrDefault(o => o.AvailabilityViolationPropertiesName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (oAvailabilityViolationProperty == null)
                                    {
                                        Console.WriteLine("Adding new AVAILABILITYVIOLATIONPROPERTIES " + sEnumerationValue);
                                        oAvailabilityViolationProperty = new AVAILABILITYVIOLATIONPROPERTIES();
                                        oAvailabilityViolationProperty.AvailabilityViolationPropertiesName = sEnumerationValue;
                                        oAvailabilityViolationProperty.VocabularyID = iVocabularyMAECID;
                                        oAvailabilityViolationProperty.CreatedDate = DateTimeOffset.Now;
                                        oAvailabilityViolationProperty.timestamp = DateTimeOffset.Now;
                                        oAvailabilityViolationProperty.EnumerationVersionID = iEnumerationVersionID;
                                        //oAvailabilityViolationProperty.ValidFromDate=;
                                        model.AVAILABILITYVIOLATIONPROPERTIES.Add(oAvailabilityViolationProperty);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                oAvailabilityViolationProperty.AvailabilityViolationPropertiesDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion availabilityviolation
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("AvailabilityViolationStrategicObjectivesEnum"))
                    {
                        #region availabilityviolationstrategic
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.AVAILABILITYVIOLATIONSTRATEGICOBJECTIVE oAvailabilityViolationStrategic;
                                    oAvailabilityViolationStrategic = model.AVAILABILITYVIOLATIONSTRATEGICOBJECTIVE.FirstOrDefault(o => o.AvailabilityViolationStrategicObjectiveName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (oAvailabilityViolationStrategic == null)
                                    {
                                        Console.WriteLine("Adding new AVAILABILITYVIOLATIONSTRATEGICOBJECTIVE " + sEnumerationValue);
                                        oAvailabilityViolationStrategic = new AVAILABILITYVIOLATIONSTRATEGICOBJECTIVE();
                                        oAvailabilityViolationStrategic.AvailabilityViolationStrategicObjectiveName = sEnumerationValue;
                                        oAvailabilityViolationStrategic.VocabularyID = iVocabularyMAECID;
                                        oAvailabilityViolationStrategic.CreatedDate = DateTimeOffset.Now;
                                        oAvailabilityViolationStrategic.timestamp = DateTimeOffset.Now;
                                        oAvailabilityViolationStrategic.EnumerationVersionID = iEnumerationVersionID;
                                        //oAvailabilityViolationStrategic.ValidFromDate=;
                                        model.AVAILABILITYVIOLATIONSTRATEGICOBJECTIVE.Add(oAvailabilityViolationStrategic);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                oAvailabilityViolationStrategic.AvailabilityViolationStrategicObjectiveDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion availabilityviolationstrategic
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("AvailabilityViolationTacticalObjectivesEnum"))
                    {
                        #region availabilityviolationtactical
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.AVAILABILITYVIOLATIONTACTICALOBJECTIVE oAvailabilityViolationTactical;
                                    oAvailabilityViolationTactical = model.AVAILABILITYVIOLATIONTACTICALOBJECTIVE.FirstOrDefault(o => o.AvailabilityViolationTacticalObjectiveName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (oAvailabilityViolationTactical == null)
                                    {
                                        Console.WriteLine("Adding new AVAILABILITYVIOLATIONTACTICALOBJECTIVE " + sEnumerationValue);
                                        oAvailabilityViolationTactical = new AVAILABILITYVIOLATIONTACTICALOBJECTIVE();
                                        oAvailabilityViolationTactical.AvailabilityViolationTacticalObjectiveName = sEnumerationValue;
                                        oAvailabilityViolationTactical.VocabularyID = iVocabularyMAECID;
                                        oAvailabilityViolationTactical.CreatedDate = DateTimeOffset.Now;
                                        oAvailabilityViolationTactical.timestamp = DateTimeOffset.Now;
                                        oAvailabilityViolationTactical.EnumerationVersionID = iEnumerationVersionID;
                                        //oAvailabilityViolationTactical.ValidFromDate=;
                                        model.AVAILABILITYVIOLATIONTACTICALOBJECTIVE.Add(oAvailabilityViolationTactical);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                oAvailabilityViolationTactical.AvailabilityViolationTacticalObjectiveDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion availabilityviolationtactical
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("CapabilityObjectiveRelationshipEnum"))
                    {
                        #region capabilityobjectiverelation
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.CAPABILITYOBJECTIVERELATIONSHIP oCapabilityObjectiveRelationship;
                                    oCapabilityObjectiveRelationship = model.CAPABILITYOBJECTIVERELATIONSHIP.FirstOrDefault(o => o.CapabilityObjectiveRelashionshipName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (oCapabilityObjectiveRelationship == null)
                                    {
                                        Console.WriteLine("Adding new CAPABILITYOBJECTIVERELATIONSHIP " + sEnumerationValue);
                                        oCapabilityObjectiveRelationship = new CAPABILITYOBJECTIVERELATIONSHIP();
                                        oCapabilityObjectiveRelationship.CapabilityObjectiveRelashionshipName = sEnumerationValue;
                                        oCapabilityObjectiveRelationship.VocabularyID = iVocabularyMAECID;
                                        oCapabilityObjectiveRelationship.CreatedDate = DateTimeOffset.Now;
                                        oCapabilityObjectiveRelationship.timestamp = DateTimeOffset.Now;
                                        oCapabilityObjectiveRelationship.EnumerationVersionID = iEnumerationVersionID;
                                        //oCapabilityObjectiveRelationship.ValidFromDate=;
                                        model.CAPABILITYOBJECTIVERELATIONSHIP.Add(oCapabilityObjectiveRelationship);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                oCapabilityObjectiveRelationship.CapabilityObjectiveRelashionshipDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion capabilityobjectiverelation
                        bEnumerationProcessed = true;
                    }
                    //***


                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("CommandandControlPropertiesEnum"))
                    {
                        #region commandandcontrol
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.COMMANDANDCONTROLPROPERTIES oCommandControlProperty;
                                    oCommandControlProperty = model.COMMANDANDCONTROLPROPERTIES.FirstOrDefault(o => o.CommandandControlPropertiesName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (oCommandControlProperty == null)
                                    {
                                        Console.WriteLine("Adding new COMMANDANDCONTROLPROPERTIES " + sEnumerationValue);
                                        oCommandControlProperty = new COMMANDANDCONTROLPROPERTIES();
                                        oCommandControlProperty.CommandandControlPropertiesName = sEnumerationValue;
                                        oCommandControlProperty.VocabularyID = iVocabularyMAECID;
                                        oCommandControlProperty.CreatedDate = DateTimeOffset.Now;
                                        oCommandControlProperty.timestamp = DateTimeOffset.Now;
                                        oCommandControlProperty.EnumerationVersionID = iEnumerationVersionID;
                                        //oCommandControlProperty.ValidFromDate=;
                                        model.COMMANDANDCONTROLPROPERTIES.Add(oCommandControlProperty);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                oCommandControlProperty.CommandandControlPropertiesDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion commandandcontrol
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("CommandandControlStrategicObjectivesEnum"))
                    {
                        #region commandandcontrolstrategic
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.COMMANDANDCONTROLSTRATEGICOBJECTIVE oCommandControlStrategicObjective;
                                    oCommandControlStrategicObjective = model.COMMANDANDCONTROLSTRATEGICOBJECTIVE.FirstOrDefault(o => o.CommandandControlStrategicObjectiveName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (oCommandControlStrategicObjective == null)
                                    {
                                        Console.WriteLine("Adding new COMMANDANDCONTROLSTRATEGICOBJECTIVE " + sEnumerationValue);
                                        oCommandControlStrategicObjective = new COMMANDANDCONTROLSTRATEGICOBJECTIVE();
                                        oCommandControlStrategicObjective.CommandandControlStrategicObjectiveName = sEnumerationValue;
                                        oCommandControlStrategicObjective.VocabularyID = iVocabularyMAECID;
                                        oCommandControlStrategicObjective.CreatedDate = DateTimeOffset.Now;
                                        oCommandControlStrategicObjective.timestamp = DateTimeOffset.Now;
                                        oCommandControlStrategicObjective.EnumerationVersionID = iEnumerationVersionID;
                                        //oCommandControlStrategicObjective.ValidFromDate=;
                                        model.COMMANDANDCONTROLSTRATEGICOBJECTIVE.Add(oCommandControlStrategicObjective);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                oCommandControlStrategicObjective.CommandandControlStrategicObjectiveDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion commandandcontrolstrategic
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("CommandandControlTacticalObjectivesEnum"))
                    {
                        #region commandandcontroltactical
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.COMMANDANDCONTROLTACTICALOBJECTIVE oCommandControlTacticalObjective;
                                    oCommandControlTacticalObjective = model.COMMANDANDCONTROLTACTICALOBJECTIVE.FirstOrDefault(o => o.CommandandControlTacticalObjectiveName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (oCommandControlTacticalObjective == null)
                                    {
                                        Console.WriteLine("Adding new COMMANDANDCONTROLTACTICALOBJECTIVE " + sEnumerationValue);
                                        oCommandControlTacticalObjective = new COMMANDANDCONTROLTACTICALOBJECTIVE();
                                        oCommandControlTacticalObjective.CommandandControlTacticalObjectiveName = sEnumerationValue;
                                        oCommandControlTacticalObjective.VocabularyID = iVocabularyMAECID;
                                        oCommandControlTacticalObjective.CreatedDate = DateTimeOffset.Now;
                                        oCommandControlTacticalObjective.timestamp = DateTimeOffset.Now;
                                        oCommandControlTacticalObjective.EnumerationVersionID = iEnumerationVersionID;
                                        //oCommandControlTacticalObjective.ValidFromDate=;
                                        model.COMMANDANDCONTROLTACTICALOBJECTIVE.Add(oCommandControlTacticalObjective);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                oCommandControlTacticalObjective.CommandandControlTacticalObjectiveDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion commandandcontroltactical
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("CommonCapabilityPropertiesEnum"))
                    {
                        #region commoncapability
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.COMMONCAPABILITYPROPERTIES oCommonCapabilityProperty;
                                    oCommonCapabilityProperty = model.COMMONCAPABILITYPROPERTIES.FirstOrDefault(o => o.CommonCapabilityPropertiesName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (oCommonCapabilityProperty == null)
                                    {
                                        Console.WriteLine("Adding new COMMONCAPABILITYPROPERTIES " + sEnumerationValue);
                                        oCommonCapabilityProperty = new COMMONCAPABILITYPROPERTIES();
                                        oCommonCapabilityProperty.CommonCapabilityPropertiesName = sEnumerationValue;
                                        oCommonCapabilityProperty.VocabularyID = iVocabularyMAECID;
                                        oCommonCapabilityProperty.CreatedDate = DateTimeOffset.Now;
                                        oCommonCapabilityProperty.timestamp = DateTimeOffset.Now;
                                        oCommonCapabilityProperty.EnumerationVersionID = iEnumerationVersionID;
                                        //oCommonCapabilityProperty.ValidFromDate=;
                                        model.COMMONCAPABILITYPROPERTIES.Add(oCommonCapabilityProperty);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                oCommonCapabilityProperty.CommonCapabilityPropertiesDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion commoncapability
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("DataExfiltrationPropertiesEnum"))
                    {
                        #region dataexfiltrationproperty
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.DATAEXFILTRATIONPROPERTIES oDataExfiltrationProperty;
                                    oDataExfiltrationProperty = model.DATAEXFILTRATIONPROPERTIES.FirstOrDefault(o => o.DataExfiltrationPropertiesName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (oDataExfiltrationProperty == null)
                                    {
                                        Console.WriteLine("Adding new DATAEXFILTRATIONPROPERTIES " + sEnumerationValue);
                                        oDataExfiltrationProperty = new DATAEXFILTRATIONPROPERTIES();
                                        oDataExfiltrationProperty.DataExfiltrationPropertiesName = sEnumerationValue;
                                        oDataExfiltrationProperty.VocabularyID = iVocabularyMAECID;
                                        oDataExfiltrationProperty.CreatedDate = DateTimeOffset.Now;
                                        oDataExfiltrationProperty.timestamp = DateTimeOffset.Now;
                                        oDataExfiltrationProperty.EnumerationVersionID = iEnumerationVersionID;
                                        //oDataExfiltrationProperty.ValidFromDate=;
                                        model.DATAEXFILTRATIONPROPERTIES.Add(oDataExfiltrationProperty);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                oDataExfiltrationProperty.DataExfiltrationPropertiesDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion dataexfiltrationproperty
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("DataExfiltrationStrategicObjectivesEnum"))
                    {
                        #region dataexfiltrationstrategic
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.DATAEXFILTRATIONSTRATEGICOBJECTIVE oDataExfiltrationStrategic;
                                    oDataExfiltrationStrategic = model.DATAEXFILTRATIONSTRATEGICOBJECTIVE.FirstOrDefault(o => o.DataExfiltrationStrategicObjectiveName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (oDataExfiltrationStrategic == null)
                                    {
                                        Console.WriteLine("Adding new DATAEXFILTRATIONSTRATEGICOBJECTIVE " + sEnumerationValue);
                                        oDataExfiltrationStrategic = new DATAEXFILTRATIONSTRATEGICOBJECTIVE();
                                        oDataExfiltrationStrategic.DataExfiltrationStrategicObjectiveName = sEnumerationValue;
                                        oDataExfiltrationStrategic.VocabularyID = iVocabularyMAECID;
                                        oDataExfiltrationStrategic.CreatedDate = DateTimeOffset.Now;
                                        oDataExfiltrationStrategic.timestamp = DateTimeOffset.Now;
                                        oDataExfiltrationStrategic.EnumerationVersionID = iEnumerationVersionID;
                                        //oDataExfiltrationStrategic.ValidFromDate=;
                                        model.DATAEXFILTRATIONSTRATEGICOBJECTIVE.Add(oDataExfiltrationStrategic);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                oDataExfiltrationStrategic.DataExfiltrationStrategicObjectiveDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion dataexfiltrationstrategic
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("DataExfiltrationTacticalObjectivesEnum"))
                    {
                        #region dataexfiltrationtactical
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.DATAEXFILTRATIONTACTICALOBJECTIVE oDataExfiltrationTactical;
                                    oDataExfiltrationTactical = model.DATAEXFILTRATIONTACTICALOBJECTIVE.FirstOrDefault(o => o.DataExfiltrationTacticalObjectiveName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (oDataExfiltrationTactical == null)
                                    {
                                        Console.WriteLine("Adding new DATAEXFILTRATIONTACTICALOBJECTIVE " + sEnumerationValue);
                                        oDataExfiltrationTactical = new DATAEXFILTRATIONTACTICALOBJECTIVE();
                                        oDataExfiltrationTactical.DataExfiltrationTacticalObjectiveName = sEnumerationValue;
                                        oDataExfiltrationTactical.VocabularyID = iVocabularyMAECID;
                                        oDataExfiltrationTactical.CreatedDate = DateTimeOffset.Now;
                                        oDataExfiltrationTactical.timestamp = DateTimeOffset.Now;
                                        oDataExfiltrationTactical.EnumerationVersionID = iEnumerationVersionID;
                                        //oDataExfiltrationTactical.ValidFromDate=;
                                        model.DATAEXFILTRATIONTACTICALOBJECTIVE.Add(oDataExfiltrationTactical);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                oDataExfiltrationTactical.DataExfiltrationTacticalObjectiveDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion dataexfiltrationtactical
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("DataTheftPropertiesEnum"))
                    {
                        #region datatheft
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.DATATHEFTPROPERTIES oDataTheftProperty;
                                    oDataTheftProperty = model.DATATHEFTPROPERTIES.FirstOrDefault(o => o.DataTheftPropertiesName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (oDataTheftProperty == null)
                                    {
                                        Console.WriteLine("Adding new DATATHEFTPROPERTIES " + sEnumerationValue);
                                        oDataTheftProperty = new DATATHEFTPROPERTIES();
                                        oDataTheftProperty.DataTheftPropertiesName = sEnumerationValue;
                                        oDataTheftProperty.VocabularyID = iVocabularyMAECID;
                                        oDataTheftProperty.CreatedDate = DateTimeOffset.Now;
                                        oDataTheftProperty.timestamp = DateTimeOffset.Now;
                                        oDataTheftProperty.EnumerationVersionID = iEnumerationVersionID;
                                        //oDataTheftProperty.ValidFromDate=;
                                        model.DATATHEFTPROPERTIES.Add(oDataTheftProperty);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                oDataTheftProperty.DataTheftPropertiesDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion datatheft
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("DataTheftStrategicObjectivesEnum"))
                    {
                        #region datatheftstrategic
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.DATATHEFTSTRATEGICOBJECTIVE oDataTheftStrategic;
                                    oDataTheftStrategic = model.DATATHEFTSTRATEGICOBJECTIVE.FirstOrDefault(o => o.DataTheftStrategicObjectiveName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (oDataTheftStrategic == null)
                                    {
                                        Console.WriteLine("Adding new DATATHEFTSTRATEGICOBJECTIVE " + sEnumerationValue);
                                        oDataTheftStrategic = new DATATHEFTSTRATEGICOBJECTIVE();
                                        oDataTheftStrategic.DataTheftStrategicObjectiveName = sEnumerationValue;
                                        oDataTheftStrategic.VocabularyID = iVocabularyMAECID;
                                        oDataTheftStrategic.CreatedDate = DateTimeOffset.Now;
                                        oDataTheftStrategic.timestamp = DateTimeOffset.Now;
                                        oDataTheftStrategic.EnumerationVersionID = iEnumerationVersionID;
                                        //oDataTheftStrategic.ValidFromDate=;
                                        model.DATATHEFTSTRATEGICOBJECTIVE.Add(oDataTheftStrategic);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                oDataTheftStrategic.DataTheftStrategicObjectiveDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion datatheftstrategic
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("DataTheftTacticalObjectivesEnum"))
                    {
                        #region datatheftstrategic
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.DATATHEFTTACTICALOBJECTIVE oDataTheftTactical;
                                    oDataTheftTactical = model.DATATHEFTTACTICALOBJECTIVE.FirstOrDefault(o => o.DataTheftTacticalObjectiveName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (oDataTheftTactical == null)
                                    {
                                        Console.WriteLine("Adding new DATATHEFTTACTICALOBJECTIVE " + sEnumerationValue);
                                        oDataTheftTactical = new DATATHEFTTACTICALOBJECTIVE();
                                        oDataTheftTactical.DataTheftTacticalObjectiveName = sEnumerationValue;
                                        oDataTheftTactical.VocabularyID = iVocabularyMAECID;
                                        oDataTheftTactical.CreatedDate = DateTimeOffset.Now;
                                        oDataTheftTactical.timestamp = DateTimeOffset.Now;
                                        oDataTheftTactical.EnumerationVersionID = iEnumerationVersionID;
                                        //oDataTheftTactical.ValidFromDate=;
                                        model.DATATHEFTTACTICALOBJECTIVE.Add(oDataTheftTactical);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                oDataTheftTactical.DataTheftTacticalObjectiveDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion datatheftstrategic
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("DebuggingActionNameEnum"))
                    {
                        #region debugaction
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.DEBUGGINGACTIONNAME myDebugActionName;
                                    myDebugActionName = model.DEBUGGINGACTIONNAME.FirstOrDefault(o => o.DebuggingActionNameName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (myDebugActionName == null)
                                    {
                                        Console.WriteLine("Adding new DEBUGGINGACTIONNAME " + sEnumerationValue);
                                        myDebugActionName = new DEBUGGINGACTIONNAME();
                                        myDebugActionName.DebuggingActionNameName = sEnumerationValue;
                                        myDebugActionName.VocabularyID = iVocabularyMAECID;
                                        myDebugActionName.CreatedDate = DateTimeOffset.Now;
                                        myDebugActionName.timestamp = DateTimeOffset.Now;
                                        myDebugActionName.EnumerationVersionID = iEnumerationVersionID;
                                        //myDebugActionName.ValidFromDate=;
                                        model.DEBUGGINGACTIONNAME.Add(myDebugActionName);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                myDebugActionName.DebuggingActionNameDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion debugaction
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("DestructionPropertiesEnum"))
                    {
                        #region destruction
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.DESTRUCTIONPROPERTIES oDestructionProperty;
                                    oDestructionProperty = model.DESTRUCTIONPROPERTIES.FirstOrDefault(o => o.DestructionPropertiesName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (oDestructionProperty == null)
                                    {
                                        Console.WriteLine("Adding new DESTRUCTIONPROPERTIES " + sEnumerationValue);
                                        oDestructionProperty = new DESTRUCTIONPROPERTIES();
                                        oDestructionProperty.DestructionPropertiesName = sEnumerationValue;
                                        oDestructionProperty.VocabularyID = iVocabularyMAECID;
                                        oDestructionProperty.CreatedDate = DateTimeOffset.Now;
                                        oDestructionProperty.timestamp = DateTimeOffset.Now;
                                        oDestructionProperty.EnumerationVersionID = iEnumerationVersionID;
                                        //oDestructionProperty.ValidFromDate=;
                                        model.DESTRUCTIONPROPERTIES.Add(oDestructionProperty);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                oDestructionProperty.DestructionPropertiesDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion destruction
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("DestructionStrategicObjectivesEnum"))
                    {
                        #region destructionstrategic
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.DESTRUCTIONSTRATEGICOBJECTIVE oDestructionStrategic;
                                    oDestructionStrategic = model.DESTRUCTIONSTRATEGICOBJECTIVE.FirstOrDefault(o => o.DestructionStrategicObjectiveName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (oDestructionStrategic == null)
                                    {
                                        Console.WriteLine("Adding new DESTRUCTIONSTRATEGICOBJECTIVE " + sEnumerationValue);
                                        oDestructionStrategic = new DESTRUCTIONSTRATEGICOBJECTIVE();
                                        oDestructionStrategic.DestructionStrategicObjectiveName = sEnumerationValue;
                                        oDestructionStrategic.VocabularyID = iVocabularyMAECID;
                                        oDestructionStrategic.CreatedDate = DateTimeOffset.Now;
                                        oDestructionStrategic.timestamp = DateTimeOffset.Now;
                                        oDestructionStrategic.EnumerationVersionID = iEnumerationVersionID;
                                        //oDestructionStrategic.ValidFromDate=;
                                        model.DESTRUCTIONSTRATEGICOBJECTIVE.Add(oDestructionStrategic);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                oDestructionStrategic.DestructionStrategicObjectiveDestruction = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion destructionstrategic
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("DestructionTacticalObjectivesEnum"))
                    {
                        #region destructiontactical
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.DESTRUCTIONTACTICALOBJECTIVE oDestructionTactical;
                                    oDestructionTactical = model.DESTRUCTIONTACTICALOBJECTIVE.FirstOrDefault(o => o.DestructionTacticalObjectiveName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (oDestructionTactical == null)
                                    {
                                        Console.WriteLine("Adding new DESTRUCTIONTACTICALOBJECTIVE " + sEnumerationValue);
                                        oDestructionTactical = new DESTRUCTIONTACTICALOBJECTIVE();
                                        oDestructionTactical.DestructionTacticalObjectiveName = sEnumerationValue;
                                        oDestructionTactical.VocabularyID = iVocabularyMAECID;
                                        oDestructionTactical.CreatedDate = DateTimeOffset.Now;
                                        oDestructionTactical.timestamp = DateTimeOffset.Now;
                                        oDestructionTactical.EnumerationVersionID = iEnumerationVersionID;
                                        //oDestructionTactical.ValidFromDate=;
                                        model.DESTRUCTIONTACTICALOBJECTIVE.Add(oDestructionTactical);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                oDestructionTactical.DestructionTacticalObjectiveDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion destructiontactical
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("DeviceDriverActionNameEnum"))
                    {
                        #region devicedriver
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.DEVICEDRIVERACTIONNAME myDeviceDriverActionName;
                                    myDeviceDriverActionName = model.DEVICEDRIVERACTIONNAME.FirstOrDefault(o => o.DeviceDriverActionNameName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (myDeviceDriverActionName == null)
                                    {
                                        Console.WriteLine("Adding new DEVICEDRIVERACTIONNAME " + sEnumerationValue);
                                        myDeviceDriverActionName = new DEVICEDRIVERACTIONNAME();
                                        myDeviceDriverActionName.DeviceDriverActionNameName = sEnumerationValue;
                                        myDeviceDriverActionName.VocabularyID = iVocabularyMAECID;
                                        myDeviceDriverActionName.CreatedDate = DateTimeOffset.Now;
                                        myDeviceDriverActionName.timestamp = DateTimeOffset.Now;
                                        myDeviceDriverActionName.EnumerationVersionID = iEnumerationVersionID;
                                        //myDeviceDriverActionName.ValidFromDate=;
                                        model.DEVICEDRIVERACTIONNAME.Add(myDeviceDriverActionName);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                myDeviceDriverActionName.DeviceDriverActionNameDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion devicedriver
                        bEnumerationProcessed = true;
                    }
                    //***


                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("DirectoryActionNameEnum"))
                    {
                        #region diraction
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.DIRECTORYACTIONNAME myDirActionName;
                                    myDirActionName = model.DIRECTORYACTIONNAME.FirstOrDefault(o => o.DirectoryActionNameName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (myDirActionName == null)
                                    {
                                        Console.WriteLine("Adding new DIRECTORYACTIONNAME " + sEnumerationValue);
                                        myDirActionName = new DIRECTORYACTIONNAME();
                                        myDirActionName.DirectoryActionNameName = sEnumerationValue;
                                        myDirActionName.VocabularyID = iVocabularyMAECID;
                                        myDirActionName.CreatedDate = DateTimeOffset.Now;
                                        myDirActionName.timestamp = DateTimeOffset.Now;
                                        myDirActionName.EnumerationVersionID = iEnumerationVersionID;
                                        //myDirActionName.ValidFromDate=;
                                        model.DIRECTORYACTIONNAME.Add(myDirActionName);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                myDirActionName.DirectoryActionNameDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion diraction
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("DiskActionNameEnum"))
                    {
                        #region diskaction
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.DISKACTIONNAME myDiskActionName;
                                    myDiskActionName = model.DISKACTIONNAME.FirstOrDefault(o => o.DiskActionNameName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (myDiskActionName == null)
                                    {
                                        Console.WriteLine("Adding new DISKACTIONNAME " + sEnumerationValue);
                                        myDiskActionName = new DISKACTIONNAME();
                                        myDiskActionName.DiskActionNameName = sEnumerationValue;
                                        myDiskActionName.VocabularyID = iVocabularyMAECID;
                                        myDiskActionName.CreatedDate = DateTimeOffset.Now;
                                        myDiskActionName.timestamp = DateTimeOffset.Now;
                                        myDiskActionName.EnumerationVersionID = iEnumerationVersionID;
                                        //myDiskActionName.ValidFromDate=;
                                        model.DISKACTIONNAME.Add(myDiskActionName);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                myDiskActionName.DiskActionNameDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion diskaction
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("DNSActionNameEnum"))
                    {
                        #region dnsaction
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.DNSACTIONNAME myDNSActionName;
                                    myDNSActionName = model.DNSACTIONNAME.FirstOrDefault(o => o.DNSActionNameName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (myDNSActionName == null)
                                    {
                                        Console.WriteLine("Adding new DNSACTIONNAME " + sEnumerationValue);
                                        myDNSActionName = new DNSACTIONNAME();
                                        myDNSActionName.DNSActionNameName = sEnumerationValue;
                                        myDNSActionName.VocabularyID = iVocabularyMAECID;
                                        myDNSActionName.CreatedDate = DateTimeOffset.Now;
                                        myDNSActionName.timestamp = DateTimeOffset.Now;
                                        myDNSActionName.EnumerationVersionID = iEnumerationVersionID;
                                        //myDNSActionName.ValidFromDate=;
                                        model.DNSACTIONNAME.Add(myDNSActionName);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                myDNSActionName.DNSActionNameName = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion dnsaction
                        bEnumerationProcessed = true;
                    }
                    //***


                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("FileActionNameEnum"))
                    {
                        #region fileaction
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.FILEACTIONNAME myFileActionName;
                                    myFileActionName = model.FILEACTIONNAME.FirstOrDefault(o => o.FileActionNameName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (myFileActionName == null)
                                    {
                                        Console.WriteLine("Adding new FILEACTIONNAME " + sEnumerationValue);
                                        myFileActionName = new FILEACTIONNAME();
                                        myFileActionName.FileActionNameName = sEnumerationValue;
                                        myFileActionName.VocabularyID = iVocabularyMAECID;
                                        myFileActionName.CreatedDate = DateTimeOffset.Now;
                                        myFileActionName.timestamp = DateTimeOffset.Now;
                                        myFileActionName.EnumerationVersionID = iEnumerationVersionID;
                                        //myFileActionName.ValidFromDate=;
                                        model.FILEACTIONNAME.Add(myFileActionName);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                myFileActionName.FileActionNameDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion fileaction
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("FraudStrategicObjectivesEnum"))
                    {
                        #region fraudstrategic
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.FRAUDSTRATEGICOBJECTIVE oFraudStrategic;
                                    oFraudStrategic = model.FRAUDSTRATEGICOBJECTIVE.FirstOrDefault(o => o.FraudStrategicObjectiveName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (oFraudStrategic == null)
                                    {
                                        Console.WriteLine("Adding new FRAUDSTRATEGICOBJECTIVE " + sEnumerationValue);
                                        oFraudStrategic = new FRAUDSTRATEGICOBJECTIVE();
                                        oFraudStrategic.FraudStrategicObjectiveName = sEnumerationValue;
                                        oFraudStrategic.VocabularyID = iVocabularyMAECID;
                                        oFraudStrategic.CreatedDate = DateTimeOffset.Now;
                                        oFraudStrategic.timestamp = DateTimeOffset.Now;
                                        oFraudStrategic.EnumerationVersionID = iEnumerationVersionID;
                                        //oFraudStrategic.ValidFromDate=;
                                        model.FRAUDSTRATEGICOBJECTIVE.Add(oFraudStrategic);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                oFraudStrategic.FraudStrategicObjectiveDestruction = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion fraudstrategic
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("FraudTacticalObjectivesEnum"))
                    {
                        #region fraudtactical
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.FRAUDTACTICALOBJECTIVE oFraudTactical;
                                    oFraudTactical = model.FRAUDTACTICALOBJECTIVE.FirstOrDefault(o => o.FraudTacticalObjectiveName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (oFraudTactical == null)
                                    {
                                        Console.WriteLine("Adding new FRAUDTACTICALOBJECTIVE " + sEnumerationValue);
                                        oFraudTactical = new FRAUDTACTICALOBJECTIVE();
                                        oFraudTactical.FraudTacticalObjectiveName = sEnumerationValue;
                                        oFraudTactical.VocabularyID = iVocabularyMAECID;
                                        oFraudTactical.CreatedDate = DateTimeOffset.Now;
                                        oFraudTactical.timestamp = DateTimeOffset.Now;
                                        oFraudTactical.EnumerationVersionID = iEnumerationVersionID;
                                        //oFraudTactical.ValidFromDate=;
                                        model.FRAUDTACTICALOBJECTIVE.Add(oFraudTactical);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                oFraudTactical.FraudTacticalObjectiveDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion fraudtactical
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("FTPActionNameEnum"))
                    {
                        #region ftpaction
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.FTPACTIONNAME myFTPActionName;
                                    myFTPActionName = model.FTPACTIONNAME.FirstOrDefault(o => o.FTPActionNameName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (myFTPActionName == null)
                                    {
                                        Console.WriteLine("Adding new FTPACTIONNAME " + sEnumerationValue);
                                        myFTPActionName = new FTPACTIONNAME();
                                        myFTPActionName.FTPActionNameName = sEnumerationValue;
                                        myFTPActionName.VocabularyID = iVocabularyMAECID;
                                        myFTPActionName.CreatedDate = DateTimeOffset.Now;
                                        myFTPActionName.timestamp = DateTimeOffset.Now;
                                        myFTPActionName.EnumerationVersionID = iEnumerationVersionID;
                                        //myFTPActionName.ValidFromDate=;
                                        model.FTPACTIONNAME.Add(myFTPActionName);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                myFTPActionName.FTPActionNameDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion ftpaction
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("GroupingRelationshipEnum")) //GroupingRelationshipTypeEnum
                    {
                        #region groupingrelation
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.GROUPINGRELATIONSHIP myGroupingRelation;
                                    myGroupingRelation = model.GROUPINGRELATIONSHIP.FirstOrDefault(o => o.GroupingRelationshipName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (myGroupingRelation == null)
                                    {
                                        Console.WriteLine("Adding new GROUPINGRELATIONSHIP " + sEnumerationValue);
                                        myGroupingRelation = new GROUPINGRELATIONSHIP();
                                        myGroupingRelation.GroupingRelationshipName = sEnumerationValue;
                                        myGroupingRelation.VocabularyID = iVocabularyMAECID;
                                        myGroupingRelation.CreatedDate = DateTimeOffset.Now;
                                        myGroupingRelation.timestamp = DateTimeOffset.Now;
                                        myGroupingRelation.EnumerationVersionID = iEnumerationVersionID;
                                        //myGroupingRelation.ValidFromDate=;
                                        model.GROUPINGRELATIONSHIP.Add(myGroupingRelation);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                myGroupingRelation.GroupingRelationshipDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion groupingrelation
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("GUIActionNameEnum"))
                    {
                        #region guiaction
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.GUIACTIONNAME myGUIActionName;
                                    myGUIActionName = model.GUIACTIONNAME.FirstOrDefault(o => o.GUIActionNameName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (myGUIActionName == null)
                                    {
                                        Console.WriteLine("Adding new GUIACTIONNAME " + sEnumerationValue);
                                        myGUIActionName = new GUIACTIONNAME();
                                        myGUIActionName.GUIActionNameName = sEnumerationValue;
                                        myGUIActionName.VocabularyID = iVocabularyMAECID;
                                        myGUIActionName.CreatedDate = DateTimeOffset.Now;
                                        myGUIActionName.timestamp = DateTimeOffset.Now;
                                        myGUIActionName.EnumerationVersionID = iEnumerationVersionID;
                                        //myGUIActionName.ValidFromDate=;
                                        model.GUIACTIONNAME.Add(myGUIActionName);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                myGUIActionName.GUIActionNameDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion guiaction
                        bEnumerationProcessed = true;
                    }
                    //***


                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("HookingActionNameEnum"))
                    {
                        #region hookaction
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.HOOKINGACTIONNAME myHookActionName;
                                    myHookActionName = model.HOOKINGACTIONNAME.FirstOrDefault(o => o.HookingActionNameName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (myHookActionName == null)
                                    {
                                        Console.WriteLine("Adding new HOOKINGACTIONNAME " + sEnumerationValue);
                                        myHookActionName = new HOOKINGACTIONNAME();
                                        myHookActionName.HookingActionNameName = sEnumerationValue;
                                        myHookActionName.VocabularyID = iVocabularyMAECID;
                                        myHookActionName.CreatedDate = DateTimeOffset.Now;
                                        myHookActionName.timestamp = DateTimeOffset.Now;
                                        myHookActionName.EnumerationVersionID = iEnumerationVersionID;
                                        //myHookActionName.ValidFromDate=;
                                        model.HOOKINGACTIONNAME.Add(myHookActionName);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                myHookActionName.HookingActionNameName = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion hookaction
                        bEnumerationProcessed = true;
                    }
                    //***


                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("HTTPActionNameEnum"))
                    {
                        #region httpaction
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.HTTPACTIONNAME myHTTPActionName;
                                    myHTTPActionName = model.HTTPACTIONNAME.FirstOrDefault(o => o.HTTPActionNameName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (myHTTPActionName == null)
                                    {
                                        Console.WriteLine("Adding new HTTPACTIONNAME " + sEnumerationValue);
                                        myHTTPActionName = new HTTPACTIONNAME();
                                        myHTTPActionName.HTTPActionNameName = sEnumerationValue;
                                        myHTTPActionName.VocabularyID = iVocabularyMAECID;
                                        myHTTPActionName.CreatedDate = DateTimeOffset.Now;
                                        myHTTPActionName.timestamp = DateTimeOffset.Now;
                                        myHTTPActionName.EnumerationVersionID = iEnumerationVersionID;
                                        //myHTTPActionName.ValidFromDate=;
                                        model.HTTPACTIONNAME.Add(myHTTPActionName);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                myHTTPActionName.HTTPActionNameDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion httpaction
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("ImportanceTypeEnum"))
                    {
                        #region importancetype
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.IMPORTANCETYPE myImportanceType;
                                    myImportanceType = model.IMPORTANCETYPE.FirstOrDefault(o => o.ImportanceTypeName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (myImportanceType == null)
                                    {
                                        Console.WriteLine("Adding new IMPORTANCETYPE " + sEnumerationValue);
                                        myImportanceType = new IMPORTANCETYPE();
                                        myImportanceType.ImportanceTypeName = sEnumerationValue;
                                        myImportanceType.VocabularyID = iVocabularyMAECID;
                                        myImportanceType.CreatedDate = DateTimeOffset.Now;
                                        myImportanceType.timestamp = DateTimeOffset.Now;
                                        myImportanceType.EnumerationVersionID = iEnumerationVersionID;
                                        //myImportanceType.ValidFromDate=;
                                        model.IMPORTANCETYPE.Add(myImportanceType);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                myImportanceType.ImportanceTypeDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion importancetype
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("InfectionPropagationPropertiesEnum"))
                    {
                        #region infectionpropagation
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.INFECTIONPROPAGATIONPROPERTIES oInfectionPropagationProperty;
                                    oInfectionPropagationProperty = model.INFECTIONPROPAGATIONPROPERTIES.FirstOrDefault(o => o.InfectionPropagationPropertiesName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (oInfectionPropagationProperty == null)
                                    {
                                        Console.WriteLine("Adding new INFECTIONPROPAGATIONPROPERTIES " + sEnumerationValue);
                                        oInfectionPropagationProperty = new INFECTIONPROPAGATIONPROPERTIES();
                                        oInfectionPropagationProperty.InfectionPropagationPropertiesName = sEnumerationValue;
                                        oInfectionPropagationProperty.VocabularyID = iVocabularyMAECID;
                                        oInfectionPropagationProperty.CreatedDate = DateTimeOffset.Now;
                                        oInfectionPropagationProperty.timestamp = DateTimeOffset.Now;
                                        oInfectionPropagationProperty.EnumerationVersionID = iEnumerationVersionID;
                                        //oInfectionPropagationProperty.ValidFromDate=;
                                        model.INFECTIONPROPAGATIONPROPERTIES.Add(oInfectionPropagationProperty);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                oInfectionPropagationProperty.InfectionPropagationPropertiesDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion infectionpropagation
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("InfectionPropagationStrategicObjectivesEnum"))
                    {
                        #region infectionpropagationstrategic
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.INFECTIONPROPAGATIONSTRATEGICOBJECTIVE oInfectionPropagationStrategic;
                                    oInfectionPropagationStrategic = model.INFECTIONPROPAGATIONSTRATEGICOBJECTIVE.FirstOrDefault(o => o.InfectionPropagationStrategicObjectiveName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (oInfectionPropagationStrategic == null)
                                    {
                                        Console.WriteLine("Adding new INFECTIONPROPAGATIONSTRATEGICOBJECTIVE " + sEnumerationValue);
                                        oInfectionPropagationStrategic = new INFECTIONPROPAGATIONSTRATEGICOBJECTIVE();
                                        oInfectionPropagationStrategic.InfectionPropagationStrategicObjectiveName = sEnumerationValue;
                                        oInfectionPropagationStrategic.VocabularyID = iVocabularyMAECID;
                                        oInfectionPropagationStrategic.CreatedDate = DateTimeOffset.Now;
                                        oInfectionPropagationStrategic.timestamp = DateTimeOffset.Now;
                                        oInfectionPropagationStrategic.EnumerationVersionID = iEnumerationVersionID;
                                        //oInfectionPropagationStrategic.ValidFromDate=;
                                        model.INFECTIONPROPAGATIONSTRATEGICOBJECTIVE.Add(oInfectionPropagationStrategic);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                oInfectionPropagationStrategic.InfectionPropagationStrategicObjectiveDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion infectionpropagationstrategic
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("InfectionPropagationTacticalObjectivesEnum"))
                    {
                        #region infectionpropagationtactical
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.INFECTIONPROPAGATIONTACTICALOBJECTIVE oInfectionPropagationTactical;
                                    oInfectionPropagationTactical = model.INFECTIONPROPAGATIONTACTICALOBJECTIVE.FirstOrDefault(o => o.InfectionPropagationTacticalObjectiveName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (oInfectionPropagationTactical == null)
                                    {
                                        Console.WriteLine("Adding new INFECTIONPROPAGATIONTACTICALOBJECTIVE " + sEnumerationValue);
                                        oInfectionPropagationTactical = new INFECTIONPROPAGATIONTACTICALOBJECTIVE();
                                        oInfectionPropagationTactical.InfectionPropagationTacticalObjectiveName = sEnumerationValue;
                                        oInfectionPropagationTactical.VocabularyID = iVocabularyMAECID;
                                        oInfectionPropagationTactical.CreatedDate = DateTimeOffset.Now;
                                        oInfectionPropagationTactical.timestamp = DateTimeOffset.Now;
                                        oInfectionPropagationTactical.EnumerationVersionID = iEnumerationVersionID;
                                        //oInfectionPropagationTactical.ValidFromDate=;
                                        model.INFECTIONPROPAGATIONTACTICALOBJECTIVE.Add(oInfectionPropagationTactical);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                oInfectionPropagationTactical.InfectionPropagationTacticalObjectiveDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion infectionpropagationtactical
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("IntegrityViolationStrategicObjectivesEnum"))
                    {
                        #region integrityviolationstrategic
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.INTEGRITYVIOLATIONSTRATEGICOBJECTIVE oIntegrityViolationStrategic;
                                    oIntegrityViolationStrategic = model.INTEGRITYVIOLATIONSTRATEGICOBJECTIVE.FirstOrDefault(o => o.IntegrityViolationStrategicObjectiveName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (oIntegrityViolationStrategic == null)
                                    {
                                        Console.WriteLine("Adding new INTEGRITYVIOLATIONSTRATEGICOBJECTIVE " + sEnumerationValue);
                                        oIntegrityViolationStrategic = new INTEGRITYVIOLATIONSTRATEGICOBJECTIVE();
                                        oIntegrityViolationStrategic.IntegrityViolationStrategicObjectiveName = sEnumerationValue;
                                        oIntegrityViolationStrategic.VocabularyID = iVocabularyMAECID;
                                        oIntegrityViolationStrategic.CreatedDate = DateTimeOffset.Now;
                                        oIntegrityViolationStrategic.timestamp = DateTimeOffset.Now;
                                        oIntegrityViolationStrategic.EnumerationVersionID = iEnumerationVersionID;
                                        //oIntegrityViolationStrategic.ValidFromDate=;
                                        model.INTEGRITYVIOLATIONSTRATEGICOBJECTIVE.Add(oIntegrityViolationStrategic);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                oIntegrityViolationStrategic.IntegrityViolationStrategicObjectiveDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion integrityviolationstrategic
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("IntegrityViolationTacticalObjectivesEnum"))
                    {
                        #region integrityviolationtactical
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.INTEGRITYVIOLATIONTACTICALOBJECTIVE oIntegrityViolationTactical;
                                    oIntegrityViolationTactical = model.INTEGRITYVIOLATIONTACTICALOBJECTIVE.FirstOrDefault(o => o.IntegrityViolationTacticalObjectiveName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (oIntegrityViolationTactical == null)
                                    {
                                        Console.WriteLine("Adding new INTEGRITYVIOLATIONTACTICALOBJECTIVE " + sEnumerationValue);
                                        oIntegrityViolationTactical = new INTEGRITYVIOLATIONTACTICALOBJECTIVE();
                                        oIntegrityViolationTactical.IntegrityViolationTacticalObjectiveName = sEnumerationValue;
                                        oIntegrityViolationTactical.VocabularyID = iVocabularyMAECID;
                                        oIntegrityViolationTactical.CreatedDate = DateTimeOffset.Now;
                                        oIntegrityViolationTactical.timestamp = DateTimeOffset.Now;
                                        oIntegrityViolationTactical.EnumerationVersionID = iEnumerationVersionID;
                                        //oIntegrityViolationTactical.ValidFromDate=;
                                        model.INTEGRITYVIOLATIONTACTICALOBJECTIVE.Add(oIntegrityViolationTactical);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                oIntegrityViolationTactical.IntegrityViolationTacticalObjectiveDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion integrityviolationtactical
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("IPCActionNameEnum"))
                    {
                        #region ipcaction
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.IPCACTIONNAME myIPCActionName;
                                    myIPCActionName = model.IPCACTIONNAME.FirstOrDefault(o => o.IPCActionNameName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (myIPCActionName == null)
                                    {
                                        Console.WriteLine("Adding new IPCACTIONNAME " + sEnumerationValue);
                                        myIPCActionName = new IPCACTIONNAME();
                                        myIPCActionName.IPCActionNameName = sEnumerationValue;
                                        myIPCActionName.VocabularyID = iVocabularyMAECID;
                                        myIPCActionName.CreatedDate = DateTimeOffset.Now;
                                        myIPCActionName.timestamp = DateTimeOffset.Now;
                                        myIPCActionName.EnumerationVersionID = iEnumerationVersionID;
                                        //myIPCActionName.ValidFromDate=;
                                        model.IPCACTIONNAME.Add(myIPCActionName);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                myIPCActionName.IPCActionNameDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion ipcaction
                        bEnumerationProcessed = true;
                    }
                    //***


                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("IRCActionNameEnum"))
                    {
                        #region ircaction
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.IRCACTIONNAME myIRCActionName;
                                    myIRCActionName = model.IRCACTIONNAME.FirstOrDefault(o => o.IRCActionNameName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (myIRCActionName == null)
                                    {
                                        Console.WriteLine("Adding new IRCACTIONNAME " + sEnumerationValue);
                                        myIRCActionName = new IRCACTIONNAME();
                                        myIRCActionName.IRCActionNameName = sEnumerationValue;
                                        myIRCActionName.VocabularyID = iVocabularyMAECID;
                                        myIRCActionName.CreatedDate = DateTimeOffset.Now;
                                        myIRCActionName.timestamp = DateTimeOffset.Now;
                                        myIRCActionName.EnumerationVersionID = iEnumerationVersionID;
                                        //myIRCActionName.ValidFromDate=;
                                        model.IRCACTIONNAME.Add(myIRCActionName);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                myIRCActionName.IRCActionNameDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion ircaction
                        bEnumerationProcessed = true;
                    }
                    //***


                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("LibraryActionNameEnum"))
                    {
                        #region libaction
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.LIBRARYACTIONNAME myLibActionName;
                                    myLibActionName = model.LIBRARYACTIONNAME.FirstOrDefault(o => o.LibraryActionNameName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (myLibActionName == null)
                                    {
                                        Console.WriteLine("Adding new LIBRARYACTIONNAME " + sEnumerationValue);
                                        myLibActionName = new LIBRARYACTIONNAME();
                                        myLibActionName.LibraryActionNameName = sEnumerationValue;
                                        myLibActionName.VocabularyID = iVocabularyMAECID;
                                        myLibActionName.CreatedDate = DateTimeOffset.Now;
                                        myLibActionName.timestamp = DateTimeOffset.Now;
                                        myLibActionName.EnumerationVersionID = iEnumerationVersionID;
                                        //myLibActionName.ValidFromDate=;
                                        model.LIBRARYACTIONNAME.Add(myLibActionName);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                myLibActionName.LibraryActionNameDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion libaction
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("MachineAccessControlPropertiesEnum"))
                    {
                        #region machineaccessproperty
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.MACHINEACCESSCONTROLPROPERTIES oMachineAccessControlProperty;
                                    oMachineAccessControlProperty = model.MACHINEACCESSCONTROLPROPERTIES.FirstOrDefault(o => o.MachineAccessControlPropertiesName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (oMachineAccessControlProperty == null)
                                    {
                                        Console.WriteLine("Adding new MACHINEACCESSCONTROLPROPERTIES " + sEnumerationValue);
                                        oMachineAccessControlProperty = new MACHINEACCESSCONTROLPROPERTIES();
                                        oMachineAccessControlProperty.MachineAccessControlPropertiesName = sEnumerationValue;
                                        oMachineAccessControlProperty.VocabularyID = iVocabularyMAECID;
                                        oMachineAccessControlProperty.CreatedDate = DateTimeOffset.Now;
                                        oMachineAccessControlProperty.timestamp = DateTimeOffset.Now;
                                        oMachineAccessControlProperty.EnumerationVersionID = iEnumerationVersionID;
                                        //oMachineAccessControlProperty.ValidFromDate=;
                                        model.MACHINEACCESSCONTROLPROPERTIES.Add(oMachineAccessControlProperty);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                oMachineAccessControlProperty.MachineAccessControlPropertiesDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion machineaccessproperty
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("MachineAccessControlStrategicObjectivesEnum"))
                    {
                        #region machineaccessstrategic
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.MACHINEACCESSCONTROLSTRATEGICOBJECTIVE oMachineAccessControlStrategic;
                                    oMachineAccessControlStrategic = model.MACHINEACCESSCONTROLSTRATEGICOBJECTIVE.FirstOrDefault(o => o.MachineAccessControlStrategicObjectiveName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (oMachineAccessControlStrategic == null)
                                    {
                                        Console.WriteLine("Adding new MACHINEACCESSCONTROLSTRATEGICOBJECTIVE " + sEnumerationValue);
                                        oMachineAccessControlStrategic = new MACHINEACCESSCONTROLSTRATEGICOBJECTIVE();
                                        oMachineAccessControlStrategic.MachineAccessControlStrategicObjectiveName = sEnumerationValue;
                                        oMachineAccessControlStrategic.VocabularyID = iVocabularyMAECID;
                                        oMachineAccessControlStrategic.CreatedDate = DateTimeOffset.Now;
                                        oMachineAccessControlStrategic.timestamp = DateTimeOffset.Now;
                                        oMachineAccessControlStrategic.EnumerationVersionID = iEnumerationVersionID;
                                        //oMachineAccessControlStrategic.ValidFromDate=;
                                        model.MACHINEACCESSCONTROLSTRATEGICOBJECTIVE.Add(oMachineAccessControlStrategic);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                oMachineAccessControlStrategic.MachineAccessControlStrategicObjectiveDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion machineaccessstrategic
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("MachineAccessControlTacticalObjectivesEnum"))
                    {
                        #region machineaccesstactical
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.MACHINEACCESSCONTROLTACTICALOBJECTIVE oMachineAccessControlTactical;
                                    oMachineAccessControlTactical = model.MACHINEACCESSCONTROLTACTICALOBJECTIVE.FirstOrDefault(o => o.MachineAccessControlTacticalObjectiveName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (oMachineAccessControlTactical == null)
                                    {
                                        Console.WriteLine("Adding new MACHINEACCESSCONTROLTACTICALOBJECTIVE " + sEnumerationValue);
                                        oMachineAccessControlTactical = new MACHINEACCESSCONTROLTACTICALOBJECTIVE();
                                        oMachineAccessControlTactical.MachineAccessControlTacticalObjectiveName = sEnumerationValue;
                                        oMachineAccessControlTactical.VocabularyID = iVocabularyMAECID;
                                        oMachineAccessControlTactical.CreatedDate = DateTimeOffset.Now;
                                        oMachineAccessControlTactical.timestamp = DateTimeOffset.Now;
                                        oMachineAccessControlTactical.EnumerationVersionID = iEnumerationVersionID;
                                        //oMachineAccessControlTactical.ValidFromDate=;
                                        model.MACHINEACCESSCONTROLTACTICALOBJECTIVE.Add(oMachineAccessControlTactical);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                oMachineAccessControlTactical.MachineAccessControlTacticalObjectiveDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion machineaccesstactical
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    //***********************************************************************************************************************************
                    //***********************************************************************************************************************************

                    //CapabilityObjectiveRelationshipEnum

                    if (sNodeName.Contains("MalwareCapabilityEnum"))
                    {
                        #region malwarecapability
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XMALWAREModel.MALWARECAPABILITY oMalwareCapability;
                                    oMalwareCapability = malware_model.MALWARECAPABILITY.FirstOrDefault(o => o.MalwareCapabilityName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (oMalwareCapability == null)
                                    {
                                        Console.WriteLine("Adding new MALWARECAPABILITY " + sEnumerationValue);
                                        oMalwareCapability = new MALWARECAPABILITY();
                                        oMalwareCapability.MalwareCapabilityName = sEnumerationValue;
                                        oMalwareCapability.VocabularyID = iVocabularyMAECID;
                                        oMalwareCapability.CreatedDate = DateTimeOffset.Now;
                                        oMalwareCapability.timestamp = DateTimeOffset.Now;
                                        oMalwareCapability.EnumerationVersionID = iEnumerationVersionID;
                                        //oMalwareCapability.ValidFromDate=;
                                        malware_model.MALWARECAPABILITY.Add(oMalwareCapability);
                                        malware_model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                oMalwareCapability.MalwareCapabilityDescription = node4.InnerText;
                                                malware_model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion malwarecapability
                        bEnumerationProcessed = true;
                    }
                    //***


                    if (sNodeName.Contains("MalwareConfigurationParameterEnum"))
                    {
                        #region malwareconfigparameter
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XMALWAREModel.MALWARECONFIGURATIONPARAMETER myMalwareConfigParameter;
                                    myMalwareConfigParameter = malware_model.MALWARECONFIGURATIONPARAMETER.FirstOrDefault(o => o.MalwareConfigurationParameterName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (myMalwareConfigParameter == null)
                                    {
                                        Console.WriteLine("Adding new MALWARECONFIGURATIONPARAMETER " + sEnumerationValue);
                                        myMalwareConfigParameter = new MALWARECONFIGURATIONPARAMETER();
                                        myMalwareConfigParameter.MalwareConfigurationParameterName = sEnumerationValue;
                                        myMalwareConfigParameter.VocabularyID = iVocabularyMAECID;
                                        myMalwareConfigParameter.CreatedDate = DateTimeOffset.Now;
                                        myMalwareConfigParameter.timestamp = DateTimeOffset.Now;
                                        myMalwareConfigParameter.EnumerationVersionID = iEnumerationVersionID;
                                        //myMalwareConfigParameter.ValidFromDate=;
                                        malware_model.MALWARECONFIGURATIONPARAMETER.Add(myMalwareConfigParameter);
                                        malware_model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                myMalwareConfigParameter.MalwareConfigurationParameterDescription = node4.InnerText;
                                                malware_model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion malwareconfigparameter
                        bEnumerationProcessed = true;
                    }
                    //***



                    if (sNodeName.Contains("MalwareDevelopmentToolEnum"))
                    {
                        #region malwaredevtool
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XMALWAREModel.MALWAREDEVTOOL myMalwareDevTool;
                                    myMalwareDevTool = malware_model.MALWAREDEVTOOL.FirstOrDefault(o => o.MalwareDevelopmentToolName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (myMalwareDevTool == null)
                                    {
                                        Console.WriteLine("Adding new MALWAREDEVTOOL " + sEnumerationValue);
                                        myMalwareDevTool = new MALWAREDEVTOOL();
                                        myMalwareDevTool.MalwareDevelopmentToolName = sEnumerationValue;
                                        myMalwareDevTool.VocabularyID = iVocabularyMAECID;
                                        myMalwareDevTool.CreatedDate = DateTimeOffset.Now;
                                        myMalwareDevTool.timestamp = DateTimeOffset.Now;
                                        myMalwareDevTool.EnumerationVersionID = iEnumerationVersionID;
                                        //myMalwareDevTool.ValidFromDate=;
                                        malware_model.MALWAREDEVTOOL.Add(myMalwareDevTool);
                                        malware_model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                myMalwareDevTool.MalwareDevelopmentToolDescription = node4.InnerText;
                                                malware_model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion malwaredevtool
                        bEnumerationProcessed = true;
                    }
                    //***

                    
                    
                    if (sNodeName.Contains("MalwareEntityTypeEnum"))
                    {
                        #region malwareentity
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XMALWAREModel.MALWAREENTITY myMalwareEntity;
                                    myMalwareEntity = malware_model.MALWAREENTITY.FirstOrDefault(o => o.MalwareEntityName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (myMalwareEntity == null)
                                    {
                                        Console.WriteLine("Adding new MALWAREENTITY " + sEnumerationValue);
                                        myMalwareEntity = new MALWAREENTITY();
                                        myMalwareEntity.MalwareEntityName = sEnumerationValue;
                                        myMalwareEntity.VocabularyID = iVocabularyMAECID;
                                        myMalwareEntity.CreatedDate = DateTimeOffset.Now;
                                        myMalwareEntity.timestamp = DateTimeOffset.Now;
                                        myMalwareEntity.EnumerationVersionID = iEnumerationVersionID;
                                        //myMalwareEntity.ValidFromDate=;
                                        malware_model.MALWAREENTITY.Add(myMalwareEntity);
                                        malware_model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                myMalwareEntity.MalwareEntityDescription = node4.InnerText;
                                                malware_model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion malwareentity
                        bEnumerationProcessed = true;
                    }
                    //***

                    if (sNodeName.Contains("MalwareLabelEnum"))
                    {
                        #region malwarelabel
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XMALWAREModel.MALWARELABEL myMalwareLabel;
                                    myMalwareLabel = malware_model.MALWARELABEL.FirstOrDefault(o => o.MalwareLabelName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (myMalwareLabel == null)
                                    {
                                        Console.WriteLine("Adding new MALWARELABEL " + sEnumerationValue);
                                        myMalwareLabel = new MALWARELABEL();
                                        myMalwareLabel.MalwareLabelName = sEnumerationValue;
                                        myMalwareLabel.VocabularyID = iVocabularyMAECID;
                                        myMalwareLabel.CreatedDate = DateTimeOffset.Now;
                                        myMalwareLabel.timestamp = DateTimeOffset.Now;
                                        myMalwareLabel.EnumerationVersionID = iEnumerationVersionID;
                                        //myMalwareLabel.ValidFromDate=;
                                        malware_model.MALWARELABEL.Add(myMalwareLabel);
                                        malware_model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                myMalwareLabel.MalwareLabelDescription = node4.InnerText;
                                                malware_model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion malwarelabel
                        bEnumerationProcessed = true;
                    }
                    //***



                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("MalwareSubjectRelationshipEnum"))   //MalwareSubjectRelationshipTypeEnum
                    {
                        #region malwaresubject
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XMALWAREModel.MALWARESUBJECTRELATIONSHIP myMalwareSubjectRelation;
                                    myMalwareSubjectRelation = malware_model.MALWARESUBJECTRELATIONSHIP.FirstOrDefault(o => o.MalwareSubjectRelationshipName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (myMalwareSubjectRelation == null)
                                    {
                                        Console.WriteLine("Adding new MALWARESUBJECTRELATIONSHIP " + sEnumerationValue);
                                        myMalwareSubjectRelation = new MALWARESUBJECTRELATIONSHIP();
                                        myMalwareSubjectRelation.MalwareSubjectRelationshipName = sEnumerationValue;
                                        myMalwareSubjectRelation.VocabularyID = iVocabularyMAECID;
                                        myMalwareSubjectRelation.CreatedDate = DateTimeOffset.Now;
                                        myMalwareSubjectRelation.timestamp = DateTimeOffset.Now;
                                        myMalwareSubjectRelation.EnumerationVersionID = iEnumerationVersionID;
                                        //myMalwareSubjectRelation.ValidFromDate=;
                                        malware_model.MALWARESUBJECTRELATIONSHIP.Add(myMalwareSubjectRelation);
                                        malware_model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                myMalwareSubjectRelation.MalwareSubjectRelationshipDescription = node4.InnerText;
                                                malware_model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion malwaresubject
                        bEnumerationProcessed = true;
                    }
                    //***********************************************************************************************************************************
                    //***********************************************************************************************************************************
                    //***


                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("NetworkActionNameEnum"))
                    {
                        #region netaction
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.NETWORKACTIONNAME myNetworkActionName;
                                    myNetworkActionName = model.NETWORKACTIONNAME.FirstOrDefault(o => o.NetworkActionNameName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (myNetworkActionName == null)
                                    {
                                        Console.WriteLine("Adding new NETWORKACTIONNAME " + sEnumerationValue);
                                        myNetworkActionName = new NETWORKACTIONNAME();
                                        myNetworkActionName.NetworkActionNameName = sEnumerationValue;
                                        myNetworkActionName.VocabularyID = iVocabularyMAECID;
                                        myNetworkActionName.CreatedDate = DateTimeOffset.Now;
                                        myNetworkActionName.timestamp = DateTimeOffset.Now;
                                        myNetworkActionName.EnumerationVersionID = iEnumerationVersionID;
                                        //myNetworkActionName.ValidFromDate=;
                                        model.NETWORKACTIONNAME.Add(myNetworkActionName);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                myNetworkActionName.NetworkActionNameDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion netaction
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("NetworkShareActionNameEnum"))
                    {
                        #region netshareaction
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.NETWORKSHAREACTIONNAME myNetworkShareActionName;
                                    myNetworkShareActionName = model.NETWORKSHAREACTIONNAME.FirstOrDefault(o => o.NetworkShareActionNameName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (myNetworkShareActionName == null)
                                    {
                                        Console.WriteLine("Adding new NETWORKSHAREACTIONNAME " + sEnumerationValue);
                                        myNetworkShareActionName = new NETWORKSHAREACTIONNAME();
                                        myNetworkShareActionName.NetworkShareActionNameName = sEnumerationValue;
                                        myNetworkShareActionName.VocabularyID = iVocabularyMAECID;
                                        myNetworkShareActionName.CreatedDate = DateTimeOffset.Now;
                                        myNetworkShareActionName.timestamp = DateTimeOffset.Now;
                                        myNetworkShareActionName.EnumerationVersionID = iEnumerationVersionID;
                                        //myNetworkShareActionName.ValidFromDate=;
                                        model.NETWORKSHAREACTIONNAME.Add(myNetworkShareActionName);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                myNetworkShareActionName.NetworkShareActionNameDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion netshareaction
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("PersistencePropertiesEnum"))
                    {
                        #region persistenceproperty
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.PERSISTENCEPROPERTIES oPersistenceProperty;
                                    oPersistenceProperty = model.PERSISTENCEPROPERTIES.FirstOrDefault(o => o.PersistencePropertiesName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (oPersistenceProperty == null)
                                    {
                                        Console.WriteLine("Adding new PERSISTENCEPROPERTIES " + sEnumerationValue);
                                        oPersistenceProperty = new PERSISTENCEPROPERTIES();
                                        oPersistenceProperty.PersistencePropertiesName = sEnumerationValue;
                                        oPersistenceProperty.VocabularyID = iVocabularyMAECID;
                                        oPersistenceProperty.CreatedDate = DateTimeOffset.Now;
                                        oPersistenceProperty.timestamp = DateTimeOffset.Now;
                                        oPersistenceProperty.EnumerationVersionID = iEnumerationVersionID;
                                        //oPersistenceProperty.ValidFromDate=;
                                        model.PERSISTENCEPROPERTIES.Add(oPersistenceProperty);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                oPersistenceProperty.PersistencePropertiesDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion persistenceproperty
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("PersistenceStrategicObjectivesEnum"))
                    {
                        #region persistencestrategic
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.PERSISTENCESTRATEGICOBJECTIVE oPersistenceStrategic;
                                    oPersistenceStrategic = model.PERSISTENCESTRATEGICOBJECTIVE.FirstOrDefault(o => o.PersistenceStrategicObjectiveName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (oPersistenceStrategic == null)
                                    {
                                        Console.WriteLine("Adding new PERSISTENCESTRATEGICOBJECTIVE " + sEnumerationValue);
                                        oPersistenceStrategic = new PERSISTENCESTRATEGICOBJECTIVE();
                                        oPersistenceStrategic.PersistenceStrategicObjectiveName = sEnumerationValue;
                                        oPersistenceStrategic.VocabularyID = iVocabularyMAECID;
                                        oPersistenceStrategic.CreatedDate = DateTimeOffset.Now;
                                        oPersistenceStrategic.timestamp = DateTimeOffset.Now;
                                        oPersistenceStrategic.EnumerationVersionID = iEnumerationVersionID;
                                        //oPersistenceStrategic.ValidFromDate=;
                                        model.PERSISTENCESTRATEGICOBJECTIVE.Add(oPersistenceStrategic);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                oPersistenceStrategic.PersistenceStrategicObjectiveDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion persistencestrategic
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("PersistenceTacticalObjectivesEnum"))
                    {
                        #region persistencetactical
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.PERSISTENCETACTICALOBJECTIVE oPersistenceTactical;
                                    oPersistenceTactical = model.PERSISTENCETACTICALOBJECTIVE.FirstOrDefault(o => o.PersistenceTacticalObjectiveName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (oPersistenceTactical == null)
                                    {
                                        Console.WriteLine("Adding new PERSISTENCETACTICALOBJECTIVE " + sEnumerationValue);
                                        oPersistenceTactical = new PERSISTENCETACTICALOBJECTIVE();
                                        oPersistenceTactical.PersistenceTacticalObjectiveName = sEnumerationValue;
                                        oPersistenceTactical.VocabularyID = iVocabularyMAECID;
                                        oPersistenceTactical.CreatedDate = DateTimeOffset.Now;
                                        oPersistenceTactical.timestamp = DateTimeOffset.Now;
                                        oPersistenceTactical.EnumerationVersionID = iEnumerationVersionID;
                                        //oPersistenceTactical.ValidFromDate=;
                                        model.PERSISTENCETACTICALOBJECTIVE.Add(oPersistenceTactical);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                oPersistenceTactical.PersistenceTacticalObjectiveDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion persistencetactical
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("PrivilegeEscalationPropertiesEnum"))
                    {
                        #region privescalation
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.PRIVILEGEESCALATIONPROPERTIES oPrivilegeEscalationProperty;
                                    oPrivilegeEscalationProperty = model.PRIVILEGEESCALATIONPROPERTIES.FirstOrDefault(o => o.PrivilegeEscalationPropertiesName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (oPrivilegeEscalationProperty == null)
                                    {
                                        Console.WriteLine("Adding new PRIVILEGEESCALATIONPROPERTIES " + sEnumerationValue);
                                        oPrivilegeEscalationProperty = new PRIVILEGEESCALATIONPROPERTIES();
                                        oPrivilegeEscalationProperty.PrivilegeEscalationPropertiesName = sEnumerationValue;
                                        oPrivilegeEscalationProperty.VocabularyID = iVocabularyMAECID;
                                        oPrivilegeEscalationProperty.CreatedDate = DateTimeOffset.Now;
                                        oPrivilegeEscalationProperty.timestamp = DateTimeOffset.Now;
                                        oPrivilegeEscalationProperty.EnumerationVersionID = iEnumerationVersionID;
                                        //oPrivilegeEscalationProperty.ValidFromDate=;
                                        model.PRIVILEGEESCALATIONPROPERTIES.Add(oPrivilegeEscalationProperty);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                oPrivilegeEscalationProperty.PrivilegeEscalationPropertiesDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion privescalation
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("PrivilegeEscalationStrategicObjectivesEnum"))
                    {
                        #region privescalationstrategic
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.PRIVILEGEESCALATIONSTRATEGICOBJECTIVE oPrivilegeEscalationStrategic;
                                    oPrivilegeEscalationStrategic = model.PRIVILEGEESCALATIONSTRATEGICOBJECTIVE.FirstOrDefault(o => o.PrivilegeEscalationStrategicObjectiveName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (oPrivilegeEscalationStrategic == null)
                                    {
                                        Console.WriteLine("Adding new PRIVILEGEESCALATIONSTRATEGICOBJECTIVE " + sEnumerationValue);
                                        oPrivilegeEscalationStrategic = new PRIVILEGEESCALATIONSTRATEGICOBJECTIVE();
                                        oPrivilegeEscalationStrategic.PrivilegeEscalationStrategicObjectiveName = sEnumerationValue;
                                        oPrivilegeEscalationStrategic.VocabularyID = iVocabularyMAECID;
                                        oPrivilegeEscalationStrategic.CreatedDate = DateTimeOffset.Now;
                                        oPrivilegeEscalationStrategic.timestamp = DateTimeOffset.Now;
                                        oPrivilegeEscalationStrategic.EnumerationVersionID = iEnumerationVersionID;
                                        //oPrivilegeEscalationStrategic.ValidFromDate=;
                                        model.PRIVILEGEESCALATIONSTRATEGICOBJECTIVE.Add(oPrivilegeEscalationStrategic);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                oPrivilegeEscalationStrategic.PrivilegeEscalationStrategicObjectiveDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion privescalationstrategic
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("PrivilegeEscalationTacticalObjectivesEnum"))
                    {
                        #region privescalationstrategic
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.PRIVILEGEESCALATIONTACTICALOBJECTIVE oPrivilegeEscalationTactical;
                                    oPrivilegeEscalationTactical = model.PRIVILEGEESCALATIONTACTICALOBJECTIVE.FirstOrDefault(o => o.PrivilegeEscalationTacticalObjectiveName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (oPrivilegeEscalationTactical == null)
                                    {
                                        Console.WriteLine("Adding new PRIVILEGEESCALATIONTACTICALOBJECTIVE " + sEnumerationValue);
                                        oPrivilegeEscalationTactical = new PRIVILEGEESCALATIONTACTICALOBJECTIVE();
                                        oPrivilegeEscalationTactical.PrivilegeEscalationTacticalObjectiveName = sEnumerationValue;
                                        oPrivilegeEscalationTactical.VocabularyID = iVocabularyMAECID;
                                        oPrivilegeEscalationTactical.CreatedDate = DateTimeOffset.Now;
                                        oPrivilegeEscalationTactical.timestamp = DateTimeOffset.Now;
                                        oPrivilegeEscalationTactical.EnumerationVersionID = iEnumerationVersionID;
                                        //oPrivilegeEscalationTactical.ValidFromDate=;
                                        model.PRIVILEGEESCALATIONTACTICALOBJECTIVE.Add(oPrivilegeEscalationTactical);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                oPrivilegeEscalationTactical.PrivilegeEscalationTacticalObjectiveDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion privescalationstrategic
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("ProbingStrategicObjectivesEnum"))
                    {
                        #region probingstrategic
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.PROBINGSTRATEGICOBJECTIVE oProbingStrategicObjective;
                                    oProbingStrategicObjective = model.PROBINGSTRATEGICOBJECTIVE.FirstOrDefault(o => o.ProbingStrategicObjectiveName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (oProbingStrategicObjective == null)
                                    {
                                        Console.WriteLine("Adding new PROBINGSTRATEGICOBJECTIVE " + sEnumerationValue);
                                        oProbingStrategicObjective = new PROBINGSTRATEGICOBJECTIVE();
                                        oProbingStrategicObjective.ProbingStrategicObjectiveName = sEnumerationValue;
                                        oProbingStrategicObjective.VocabularyID = iVocabularyMAECID;
                                        oProbingStrategicObjective.CreatedDate = DateTimeOffset.Now;
                                        oProbingStrategicObjective.timestamp = DateTimeOffset.Now;
                                        oProbingStrategicObjective.EnumerationVersionID = iEnumerationVersionID;
                                        //oProbingStrategicObjective.ValidFromDate=;
                                        model.PROBINGSTRATEGICOBJECTIVE.Add(oProbingStrategicObjective);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                oProbingStrategicObjective.ProbingStrategicObjectiveDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion probingstrategic
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("ProbingTacticalObjectivesEnum"))
                    {
                        #region probingstrategic
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.PROBINGTACTICALOBJECTIVE oProbingTacticalObjective;
                                    oProbingTacticalObjective = model.PROBINGTACTICALOBJECTIVE.FirstOrDefault(o => o.ProbingTacticalObjectiveName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (oProbingTacticalObjective == null)
                                    {
                                        Console.WriteLine("Adding new PROBINGTACTICALOBJECTIVE " + sEnumerationValue);
                                        oProbingTacticalObjective = new PROBINGTACTICALOBJECTIVE();
                                        oProbingTacticalObjective.ProbingTacticalObjectiveName = sEnumerationValue;
                                        oProbingTacticalObjective.VocabularyID = iVocabularyMAECID;
                                        oProbingTacticalObjective.CreatedDate = DateTimeOffset.Now;
                                        oProbingTacticalObjective.timestamp = DateTimeOffset.Now;
                                        oProbingTacticalObjective.EnumerationVersionID = iEnumerationVersionID;
                                        //oProbingTacticalObjective.ValidFromDate=;
                                        model.PROBINGTACTICALOBJECTIVE.Add(oProbingTacticalObjective);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                oProbingTacticalObjective.ProbingTacticalObjectiveDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion probingstrategic
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("ProcessMemoryActionNameEnum"))
                    {
                        #region processmemaction
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.PROCESSMEMORYACTIONNAME myProcessMemActionName;
                                    myProcessMemActionName = model.PROCESSMEMORYACTIONNAME.FirstOrDefault(o => o.ProcessMemoryActionNameName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (myProcessMemActionName == null)
                                    {
                                        Console.WriteLine("Adding new PROCESSMEMORYACTIONNAME " + sEnumerationValue);
                                        myProcessMemActionName = new PROCESSMEMORYACTIONNAME();
                                        myProcessMemActionName.ProcessMemoryActionNameName = sEnumerationValue;
                                        myProcessMemActionName.VocabularyID = iVocabularyMAECID;
                                        myProcessMemActionName.CreatedDate = DateTimeOffset.Now;
                                        myProcessMemActionName.timestamp = DateTimeOffset.Now;
                                        myProcessMemActionName.EnumerationVersionID = iEnumerationVersionID;
                                        //myProcessMemActionName.ValidFromDate=;
                                        model.PROCESSMEMORYACTIONNAME.Add(myProcessMemActionName);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                myProcessMemActionName.ProcessMemoryActionNameDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion processmemaction
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("ProcessActionNameEnum"))
                    {
                        #region processaction
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.PROCESSACTIONNAME myProcessActionName;
                                    myProcessActionName = model.PROCESSACTIONNAME.FirstOrDefault(o => o.ProcessActionNameName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (myProcessActionName == null)
                                    {
                                        Console.WriteLine("Adding new PROCESSACTIONNAME " + sEnumerationValue);
                                        myProcessActionName = new PROCESSACTIONNAME();
                                        myProcessActionName.ProcessActionNameName = sEnumerationValue;
                                        myProcessActionName.VocabularyID = iVocabularyMAECID;
                                        myProcessActionName.CreatedDate = DateTimeOffset.Now;
                                        myProcessActionName.timestamp = DateTimeOffset.Now;
                                        myProcessActionName.EnumerationVersionID = iEnumerationVersionID;
                                        //myProcessActionName.ValidFromDate=;
                                        model.PROCESSACTIONNAME.Add(myProcessActionName);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                myProcessActionName.ProcessActionNameDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion processaction
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("ProcessThreadActionNameEnum"))
                    {
                        #region processthreadaction
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.PROCESSTHREADACTIONNAME myProcessThreadActionName;
                                    myProcessThreadActionName = model.PROCESSTHREADACTIONNAME.FirstOrDefault(o => o.ProcessThreadActionNameName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (myProcessThreadActionName == null)
                                    {
                                        Console.WriteLine("Adding new PROCESSTHREADACTIONNAME " + sEnumerationValue);
                                        myProcessThreadActionName = new PROCESSTHREADACTIONNAME();
                                        myProcessThreadActionName.ProcessThreadActionNameName = sEnumerationValue;
                                        myProcessThreadActionName.VocabularyID = iVocabularyMAECID;
                                        myProcessThreadActionName.CreatedDate = DateTimeOffset.Now;
                                        myProcessThreadActionName.timestamp = DateTimeOffset.Now;
                                        myProcessThreadActionName.EnumerationVersionID = iEnumerationVersionID;
                                        //myProcessThreadActionName.ValidFromDate=;
                                        model.PROCESSTHREADACTIONNAME.Add(myProcessThreadActionName);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                myProcessThreadActionName.ProcessThreadActionNameDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion processthreadaction
                        bEnumerationProcessed = true;
                    }
                    //***


                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("RegistryActionNameEnum"))
                    {
                        #region regaction
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.REGISTRYACTIONNAME myRegActionName;
                                    myRegActionName = model.REGISTRYACTIONNAME.FirstOrDefault(o => o.RegistryActionNameName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (myRegActionName == null)
                                    {
                                        Console.WriteLine("Adding new REGISTRYACTIONNAME " + sEnumerationValue);
                                        myRegActionName = new REGISTRYACTIONNAME();
                                        myRegActionName.RegistryActionNameName = sEnumerationValue;
                                        myRegActionName.VocabularyID = iVocabularyMAECID;
                                        myRegActionName.CreatedDate = DateTimeOffset.Now;
                                        myRegActionName.timestamp = DateTimeOffset.Now;
                                        myRegActionName.EnumerationVersionID = iEnumerationVersionID;
                                        //myRegActionName.ValidFromDate=;
                                        model.REGISTRYACTIONNAME.Add(myRegActionName);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                myRegActionName.RegistryActionNameDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion regaction
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("RemoteMachineManipulationStrategicObjectivesEnum"))
                    {
                        #region remotemachinestrategic
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.REMOTEMACHINEMANIPULATIONSTRATEGICOBJECTIVE oRemoteMachineManipulationStrategic;
                                    oRemoteMachineManipulationStrategic = model.REMOTEMACHINEMANIPULATIONSTRATEGICOBJECTIVE.FirstOrDefault(o => o.RemoteMachineManipulationStrategicObjectiveName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (oRemoteMachineManipulationStrategic == null)
                                    {
                                        Console.WriteLine("Adding new REMOTEMACHINEMANIPULATIONSTRATEGICOBJECTIVE " + sEnumerationValue);
                                        oRemoteMachineManipulationStrategic = new REMOTEMACHINEMANIPULATIONSTRATEGICOBJECTIVE();
                                        oRemoteMachineManipulationStrategic.RemoteMachineManipulationStrategicObjectiveName = sEnumerationValue;
                                        oRemoteMachineManipulationStrategic.VocabularyID = iVocabularyMAECID;
                                        oRemoteMachineManipulationStrategic.CreatedDate = DateTimeOffset.Now;
                                        oRemoteMachineManipulationStrategic.timestamp = DateTimeOffset.Now;
                                        oRemoteMachineManipulationStrategic.EnumerationVersionID = iEnumerationVersionID;
                                        //oRemoteMachineManipulationStrategic.ValidFromDate=;
                                        model.REMOTEMACHINEMANIPULATIONSTRATEGICOBJECTIVE.Add(oRemoteMachineManipulationStrategic);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                oRemoteMachineManipulationStrategic.RemoteMachineManipulationStrategicObjectiveDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion remotemachinestrategic
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("RemoteMachineManipulationTacticalObjectivesEnum"))
                    {
                        #region remotemachinestrategic
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.REMOTEMACHINEMANIPULATIONTACTICALOBJECTIVE oRemoteMachineManipulationTactical;
                                    oRemoteMachineManipulationTactical = model.REMOTEMACHINEMANIPULATIONTACTICALOBJECTIVE.FirstOrDefault(o => o.RemoteMachineManipulationTacticalObjectiveName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (oRemoteMachineManipulationTactical == null)
                                    {
                                        Console.WriteLine("Adding new REMOTEMACHINEMANIPULATIONTACTICALOBJECTIVE " + sEnumerationValue);
                                        oRemoteMachineManipulationTactical = new REMOTEMACHINEMANIPULATIONTACTICALOBJECTIVE();
                                        oRemoteMachineManipulationTactical.RemoteMachineManipulationTacticalObjectiveName = sEnumerationValue;
                                        oRemoteMachineManipulationTactical.VocabularyID = iVocabularyMAECID;
                                        oRemoteMachineManipulationTactical.CreatedDate = DateTimeOffset.Now;
                                        oRemoteMachineManipulationTactical.timestamp = DateTimeOffset.Now;
                                        oRemoteMachineManipulationTactical.EnumerationVersionID = iEnumerationVersionID;
                                        //oRemoteMachineManipulationStrategic.ValidFromDate=;
                                        model.REMOTEMACHINEMANIPULATIONTACTICALOBJECTIVE.Add(oRemoteMachineManipulationTactical);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                oRemoteMachineManipulationTactical.RemoteMachineManipulationTacticalObjectiveDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion remotemachinestrategic
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("SecondaryOperationPropertiesEnum"))
                    {
                        #region secondaryoperationproperty
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.SECONDARYOPERATIONPROPERTIES oSecondaryOperationProperty;
                                    oSecondaryOperationProperty = model.SECONDARYOPERATIONPROPERTIES.FirstOrDefault(o => o.SecondaryOperationPropertiesName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (oSecondaryOperationProperty == null)
                                    {
                                        Console.WriteLine("Adding new SECONDARYOPERATIONPROPERTIES " + sEnumerationValue);
                                        oSecondaryOperationProperty = new SECONDARYOPERATIONPROPERTIES();
                                        oSecondaryOperationProperty.SecondaryOperationPropertiesName = sEnumerationValue;
                                        oSecondaryOperationProperty.VocabularyID = iVocabularyMAECID;
                                        oSecondaryOperationProperty.CreatedDate = DateTimeOffset.Now;
                                        oSecondaryOperationProperty.timestamp = DateTimeOffset.Now;
                                        oSecondaryOperationProperty.EnumerationVersionID = iEnumerationVersionID;
                                        //oSecondaryOperationProperty.ValidFromDate=;
                                        model.SECONDARYOPERATIONPROPERTIES.Add(oSecondaryOperationProperty);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                oSecondaryOperationProperty.SecondaryOperationPropertiesDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion secondaryoperationproperty
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("SecondaryOperationStrategicObjectivesEnum"))
                    {
                        #region secondaryoperationstrategic
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.SECONDARYOPERATIONSTRATEGICOBJECTIVE oSecondaryOperationStrategic;
                                    oSecondaryOperationStrategic = model.SECONDARYOPERATIONSTRATEGICOBJECTIVE.FirstOrDefault(o => o.SecondaryOperationStrategicObjectiveName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (oSecondaryOperationStrategic == null)
                                    {
                                        Console.WriteLine("Adding new SECONDARYOPERATIONSTRATEGICOBJECTIVE " + sEnumerationValue);
                                        oSecondaryOperationStrategic = new SECONDARYOPERATIONSTRATEGICOBJECTIVE();
                                        oSecondaryOperationStrategic.SecondaryOperationStrategicObjectiveName = sEnumerationValue;
                                        oSecondaryOperationStrategic.VocabularyID = iVocabularyMAECID;
                                        oSecondaryOperationStrategic.CreatedDate = DateTimeOffset.Now;
                                        oSecondaryOperationStrategic.timestamp = DateTimeOffset.Now;
                                        oSecondaryOperationStrategic.EnumerationVersionID = iEnumerationVersionID;
                                        //oSecondaryOperationStrategic.ValidFromDate=;
                                        model.SECONDARYOPERATIONSTRATEGICOBJECTIVE.Add(oSecondaryOperationStrategic);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                oSecondaryOperationStrategic.SecondaryOperationStrategicObjectiveDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion secondaryoperationstrategic
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("SecondaryOperationTacticalObjectivesEnum"))
                    {
                        #region secondaryoperationtactical
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.SECONDARYOPERATIONTACTICALOBJECTIVE oSecondaryOperationTactical;
                                    oSecondaryOperationTactical = model.SECONDARYOPERATIONTACTICALOBJECTIVE.FirstOrDefault(o => o.SecondaryOperationTacticalObjectiveName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (oSecondaryOperationTactical == null)
                                    {
                                        Console.WriteLine("Adding new SECONDARYOPERATIONTACTICALOBJECTIVE " + sEnumerationValue);
                                        oSecondaryOperationTactical = new SECONDARYOPERATIONTACTICALOBJECTIVE();
                                        oSecondaryOperationTactical.SecondaryOperationTacticalObjectiveName = sEnumerationValue;
                                        oSecondaryOperationTactical.VocabularyID = iVocabularyMAECID;
                                        oSecondaryOperationTactical.CreatedDate = DateTimeOffset.Now;
                                        oSecondaryOperationTactical.timestamp = DateTimeOffset.Now;
                                        oSecondaryOperationTactical.EnumerationVersionID = iEnumerationVersionID;
                                        //oSecondaryOperationTactical.ValidFromDate=;
                                        model.SECONDARYOPERATIONTACTICALOBJECTIVE.Add(oSecondaryOperationTactical);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                oSecondaryOperationTactical.SecondaryOperationTacticalObjectiveDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion secondaryoperationtactical
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("SecurityDegradationPropertiesEnum"))
                    {
                        #region securitydegradation
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.SECURITYDEGRADATIONPROPERTIES oSecurityDegradationProperty;
                                    oSecurityDegradationProperty = model.SECURITYDEGRADATIONPROPERTIES.FirstOrDefault(o => o.SecurityDegradationPropertiesName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (oSecurityDegradationProperty == null)
                                    {
                                        Console.WriteLine("Adding new SECURITYDEGRADATIONPROPERTIES " + sEnumerationValue);
                                        oSecurityDegradationProperty = new SECURITYDEGRADATIONPROPERTIES();
                                        oSecurityDegradationProperty.SecurityDegradationPropertiesName = sEnumerationValue;
                                        oSecurityDegradationProperty.VocabularyID = iVocabularyMAECID;
                                        oSecurityDegradationProperty.CreatedDate = DateTimeOffset.Now;
                                        oSecurityDegradationProperty.timestamp = DateTimeOffset.Now;
                                        oSecurityDegradationProperty.EnumerationVersionID = iEnumerationVersionID;
                                        //oSecurityDegradationProperty.ValidFromDate=;
                                        model.SECURITYDEGRADATIONPROPERTIES.Add(oSecurityDegradationProperty);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                oSecurityDegradationProperty.SecurityDegradationPropertiesDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion securitydegradation
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("SecurityDegradationStrategicObjectivesEnum"))
                    {
                        #region securitydegradationstrategic
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.SECURITYDEGRADATIONSTRATEGICOBJECTIVE oSecurityDegradationStrategic;
                                    oSecurityDegradationStrategic = model.SECURITYDEGRADATIONSTRATEGICOBJECTIVE.FirstOrDefault(o => o.SecurityDegradationStrategicObjectiveName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (oSecurityDegradationStrategic == null)
                                    {
                                        Console.WriteLine("Adding new SECURITYDEGRADATIONSTRATEGICOBJECTIVE " + sEnumerationValue);
                                        oSecurityDegradationStrategic = new SECURITYDEGRADATIONSTRATEGICOBJECTIVE();
                                        oSecurityDegradationStrategic.SecurityDegradationStrategicObjectiveName = sEnumerationValue;
                                        oSecurityDegradationStrategic.VocabularyID = iVocabularyMAECID;
                                        oSecurityDegradationStrategic.CreatedDate = DateTimeOffset.Now;
                                        oSecurityDegradationStrategic.timestamp = DateTimeOffset.Now;
                                        oSecurityDegradationStrategic.EnumerationVersionID = iEnumerationVersionID;
                                        //oSecurityDegradationStrategic.ValidFromDate=;
                                        model.SECURITYDEGRADATIONSTRATEGICOBJECTIVE.Add(oSecurityDegradationStrategic);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                oSecurityDegradationStrategic.SecurityDegradationStrategicObjectiveDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion securitydegradationstrategic
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("SecurityDegradationTacticalObjectivesEnum"))
                    {
                        #region securitydegradationtactical
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.SECURITYDEGRADATIONTACTICALOBJECTIVE oSecurityDegradationTactical;
                                    oSecurityDegradationTactical = model.SECURITYDEGRADATIONTACTICALOBJECTIVE.FirstOrDefault(o => o.SecurityDegradationTacticalObjectiveName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (oSecurityDegradationTactical == null)
                                    {
                                        Console.WriteLine("Adding new SECURITYDEGRADATIONTACTICALOBJECTIVE " + sEnumerationValue);
                                        oSecurityDegradationTactical = new SECURITYDEGRADATIONTACTICALOBJECTIVE();
                                        oSecurityDegradationTactical.SecurityDegradationTacticalObjectiveName = sEnumerationValue;
                                        oSecurityDegradationTactical.VocabularyID = iVocabularyMAECID;
                                        oSecurityDegradationTactical.CreatedDate = DateTimeOffset.Now;
                                        oSecurityDegradationTactical.timestamp = DateTimeOffset.Now;
                                        oSecurityDegradationTactical.EnumerationVersionID = iEnumerationVersionID;
                                        //oSecurityDegradationTactical.ValidFromDate=;
                                        model.SECURITYDEGRADATIONTACTICALOBJECTIVE.Add(oSecurityDegradationTactical);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                oSecurityDegradationTactical.SecurityDegradationTacticalObjectiveDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion securitydegradationtactical
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("ServiceActionNameEnum"))
                    {
                        #region serviceaction
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.SERVICEACTIONNAME myServiceActionName;
                                    myServiceActionName = model.SERVICEACTIONNAME.FirstOrDefault(o => o.ServiceActionNameName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (myServiceActionName == null)
                                    {
                                        Console.WriteLine("Adding new SERVICEACTIONNAME " + sEnumerationValue);
                                        myServiceActionName = new SERVICEACTIONNAME();
                                        myServiceActionName.ServiceActionNameName = sEnumerationValue;
                                        myServiceActionName.VocabularyID = iVocabularyMAECID;
                                        myServiceActionName.CreatedDate = DateTimeOffset.Now;
                                        myServiceActionName.timestamp = DateTimeOffset.Now;
                                        myServiceActionName.EnumerationVersionID = iEnumerationVersionID;
                                        //myServiceActionName.ValidFromDate=;
                                        model.SERVICEACTIONNAME.Add(myServiceActionName);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                myServiceActionName.ServiceActionNameDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion serviceaction
                        bEnumerationProcessed = true;
                    }
                    //***


                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("SocketActionNameEnum"))
                    {
                        #region sockaction
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.SOCKETACTIONNAME mySocketActionName;
                                    mySocketActionName = model.SOCKETACTIONNAME.FirstOrDefault(o => o.SocketActionNameName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (mySocketActionName == null)
                                    {
                                        Console.WriteLine("Adding new SOCKETACTIONNAME " + sEnumerationValue);
                                        mySocketActionName = new SOCKETACTIONNAME();
                                        mySocketActionName.SocketActionNameName = sEnumerationValue;
                                        mySocketActionName.VocabularyID = iVocabularyMAECID;
                                        mySocketActionName.CreatedDate = DateTimeOffset.Now;
                                        mySocketActionName.timestamp = DateTimeOffset.Now;
                                        mySocketActionName.EnumerationVersionID = iEnumerationVersionID;
                                        //mySocketActionName.ValidFromDate=;
                                        model.SOCKETACTIONNAME.Add(mySocketActionName);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                mySocketActionName.SocketActionNameDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion sockaction
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("SpyingStrategicObjectivesEnum"))
                    {
                        #region spyingstrategic
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.SPYINGSTRATEGICOBJECTIVE oSpyingStrategic;
                                    oSpyingStrategic = model.SPYINGSTRATEGICOBJECTIVE.FirstOrDefault(o => o.SpyingStrategicObjectiveName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (oSpyingStrategic == null)
                                    {
                                        Console.WriteLine("Adding new SPYINGSTRATEGICOBJECTIVE " + sEnumerationValue);
                                        oSpyingStrategic = new SPYINGSTRATEGICOBJECTIVE();
                                        oSpyingStrategic.SpyingStrategicObjectiveName = sEnumerationValue;
                                        oSpyingStrategic.VocabularyID = iVocabularyMAECID;
                                        oSpyingStrategic.CreatedDate = DateTimeOffset.Now;
                                        oSpyingStrategic.timestamp = DateTimeOffset.Now;
                                        oSpyingStrategic.EnumerationVersionID = iEnumerationVersionID;
                                        //oSpyingStrategic.ValidFromDate=;
                                        model.SPYINGSTRATEGICOBJECTIVE.Add(oSpyingStrategic);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                oSpyingStrategic.SpyingStrategicObjectiveDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion spyingstrategic
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("SpyingTacticalObjectivesEnum"))
                    {
                        #region spyingtactical
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.SPYINGTACTICALOBJECTIVE oSpyingTactical;
                                    oSpyingTactical = model.SPYINGTACTICALOBJECTIVE.FirstOrDefault(o => o.SpyingTacticalObjectiveName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (oSpyingTactical == null)
                                    {
                                        Console.WriteLine("Adding new SPYINGTACTICALOBJECTIVE " + sEnumerationValue);
                                        oSpyingTactical = new SPYINGTACTICALOBJECTIVE();
                                        oSpyingTactical.SpyingTacticalObjectiveName = sEnumerationValue;
                                        oSpyingTactical.VocabularyID = iVocabularyMAECID;
                                        oSpyingTactical.CreatedDate = DateTimeOffset.Now;
                                        oSpyingTactical.timestamp = DateTimeOffset.Now;
                                        oSpyingTactical.EnumerationVersionID = iEnumerationVersionID;
                                        //oSpyingTactical.ValidFromDate=;
                                        model.SPYINGTACTICALOBJECTIVE.Add(oSpyingTactical);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                oSpyingTactical.SpyingTacticalObjectiveDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion spyingtactical
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("SynchronizationActionNameEnum"))
                    {
                        #region syncaction
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.SYNCHRONIZATIONACTIONNAME mySyncActionName;
                                    mySyncActionName = model.SYNCHRONIZATIONACTIONNAME.FirstOrDefault(o => o.SynchronizationActionNameName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (mySyncActionName == null)
                                    {
                                        Console.WriteLine("Adding new SYNCHRONIZATIONACTIONNAME " + sEnumerationValue);
                                        mySyncActionName = new SYNCHRONIZATIONACTIONNAME();
                                        mySyncActionName.SynchronizationActionNameName = sEnumerationValue;
                                        mySyncActionName.VocabularyID = iVocabularyMAECID;
                                        mySyncActionName.CreatedDate = DateTimeOffset.Now;
                                        mySyncActionName.timestamp = DateTimeOffset.Now;
                                        mySyncActionName.EnumerationVersionID = iEnumerationVersionID;
                                        //mySyncActionName.ValidFromDate=;
                                        model.SYNCHRONIZATIONACTIONNAME.Add(mySyncActionName);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                mySyncActionName.SynchronizationActionNameDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion syncaction
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("SystemActionNameEnum"))
                    {
                        #region systemaction
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.SYSTEMACTIONNAME mySystemActionName;
                                    mySystemActionName = model.SYSTEMACTIONNAME.FirstOrDefault(o => o.SystemActionNameName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (mySystemActionName == null)
                                    {
                                        Console.WriteLine("Adding new SYSTEMACTIONNAME " + sEnumerationValue);
                                        mySystemActionName = new SYSTEMACTIONNAME();
                                        mySystemActionName.SystemActionNameName = sEnumerationValue;
                                        mySystemActionName.VocabularyID = iVocabularyMAECID;
                                        mySystemActionName.CreatedDate = DateTimeOffset.Now;
                                        mySystemActionName.timestamp = DateTimeOffset.Now;
                                        mySystemActionName.EnumerationVersionID = iEnumerationVersionID;
                                        //mySystemActionName.ValidFromDate=;
                                        model.SYSTEMACTIONNAME.Add(mySystemActionName);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                mySystemActionName.SystemActionNameDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion systemaction
                        bEnumerationProcessed = true;
                    }
                    //***


                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("UserActionNameEnum"))
                    {
                        #region useraction
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.USERACTIONNAME myUserActionName;
                                    myUserActionName = model.USERACTIONNAME.FirstOrDefault(o => o.UserActionNameName == sEnumerationValue && o.EnumerationVersionID == iEnumerationVersionID);// && o.VocabularyID == iVocabularyMAECID);
                                    if (myUserActionName == null)
                                    {
                                        Console.WriteLine("Adding new USERACTIONNAME " + sEnumerationValue);
                                        myUserActionName = new USERACTIONNAME();
                                        myUserActionName.UserActionNameName = sEnumerationValue;
                                        myUserActionName.VocabularyID = iVocabularyMAECID;
                                        myUserActionName.CreatedDate = DateTimeOffset.Now;
                                        myUserActionName.timestamp = DateTimeOffset.Now;
                                        myUserActionName.EnumerationVersionID = iEnumerationVersionID;
                                        //myUserActionName.ValidFromDate=;
                                        model.USERACTIONNAME.Add(myUserActionName);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                myUserActionName.UserActionNameDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion useraction
                        bEnumerationProcessed = true;
                    }
                    //***




                    if (!bEnumerationProcessed)
                    {
                        Console.WriteLine("ERROR Missing Code for " + sNodeName);
                        //sCurrentEnum = "";
                    }
                }

            }

            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
        }
    }
}
