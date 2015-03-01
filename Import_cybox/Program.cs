using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

using XORCISMModel;

namespace Import_cybox
{
    static class Program
    {
        /// <summary>
        /// Copyright (C) 2014-2015 Jerome Athias
        /// Parser for MITRE CybOX default vocabularies enumeration values from an XML file and import into an XORCISM database
        /// This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation; either version 2 of the License, or (at your option) any later version.
        /// 
        /// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
        /// 
        /// You should have received a copy of the GNU General Public License along with this program; if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
        /// </summary>
        [STAThread]
        static void Main()
        {
            XORCISMEntities model=new XORCISMEntities();
            //https://stackoverflow.com/questions/5940225/fastest-way-of-inserting-in-entity-framework
            model.Configuration.AutoDetectChangesEnabled = false;
            model.Configuration.ValidateOnSaveEnabled = false;

            int iVocabularyCYBOXID = 0; // 11;
            string sCYBOXVersion = "2.1";   //HARDCODED TODO
            #region vocabularycybox
            try
            {
                iVocabularyCYBOXID = model.VOCABULARY.Where(o => o.VocabularyName == "CYBOX" && o.VocabularyVersion==sCYBOXVersion).Select(o => o.VocabularyID).FirstOrDefault();
            }
            catch (Exception ex)
            {

            }
            if (iVocabularyCYBOXID <= 0)
            {
                try
                {
                    VOCABULARY oVocabulary = new VOCABULARY();
                    oVocabulary.CreatedDate = DateTimeOffset.Now;
                    oVocabulary.VocabularyName = "CYBOX";
                    oVocabulary.VocabularyVersion = sCYBOXVersion;
                    model.VOCABULARY.Add(oVocabulary);
                    model.SaveChanges();
                    iVocabularyCYBOXID = oVocabulary.VocabularyID;
                    Console.WriteLine("DEBUG iVocabularyCYBOXID=" + iVocabularyCYBOXID);
                }
                catch (Exception ex)
                {

                }
            }
            #endregion vocabularycybox

            XmlDocument doc;
            doc = new XmlDocument();
            //TODO: download the file
            doc.Load(@"C:\nvdcve\cybox_default_vocabularies.xsd");  //HARDCODED
            //TODO: Validate XSD

            XmlNamespaceManager mgr = new XmlNamespaceManager(doc.NameTable);

            mgr.AddNamespace("xs", "http://www.w3.org/2001/XMLSchema");

            XmlNodeList nodes1;
            nodes1 = doc.SelectNodes("/xs:schema/xs:simpleType", mgr);
            Console.WriteLine(nodes1.Count);

            

            foreach (XmlNode node in nodes1)    //enumeration
            {
                //Console.WriteLine("DEBUG node.Name="+node.Name);
                string sNodeName = node.Attributes["name"].InnerText;
                Console.WriteLine(sNodeName);

                if (sNodeName.Contains("Enum-"))
                {
                    bool bEnumerationProcessed = false;
                    //Get the EnumerationName and Version
                    //ActionNameEnum-1.1
                    string[] words = sNodeName.Split('-');
                    string sEnumerationName = words[0];
                    string sEnumerationVersion = words[1];

                    //Check if we have this EnumerationVersion in the database
                    //First check the Version
                    XORCISMModel.VERSION oVersion;
                    int iVersionID = 0;
                    try
                    {
                        iVersionID=model.VERSION.FirstOrDefault(o => o.VersionValue == sEnumerationVersion).VersionID;
                    }
                    catch(Exception ex)
                    {

                    }
                    if(iVersionID<=0)
                    {
                        oVersion=new VERSION();
                        oVersion.VersionValue=sEnumerationVersion;
                        oVersion.VocabularyID=iVocabularyCYBOXID;
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
                    catch(Exception ex)
                    {

                    }
                    if(iEnumerationVersionID<=0)
                    {
                        oEnumerationVersion = new ENUMERATIONVERSION();
                        oEnumerationVersion.EnumerationName = sEnumerationName;
                        oEnumerationVersion.VersionID = iVersionID;
                        oEnumerationVersion.VocabularyID = iVocabularyCYBOXID;
                        model.ENUMERATIONVERSION.Add(oEnumerationVersion);
                        model.SaveChanges();
                        iEnumerationVersionID=oEnumerationVersion.EnumerationVersionID;
                    }
                    else
                    {
                        //Update ENUMERATIONVERSION
                    }


                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("ActionTypeEnum"))
                    {
                        #region actiontype
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.ACTIONTYPE myActionType;
                                    myActionType = model.ACTIONTYPE.FirstOrDefault(o => o.ActionTypeName == sEnumerationValue && o.EnumerationVersionID==iEnumerationVersionID);// && o.VocabularyID == iVocabularyCYBOXID);
                                    if (myActionType == null)
                                    {
                                        Console.WriteLine("DEBUG Adding new ACTIONTYPE " + sEnumerationValue);
                                        myActionType = new ACTIONTYPE();
                                        myActionType.ActionTypeName = sEnumerationValue;
                                        myActionType.VocabularyID = iVocabularyCYBOXID;
                                        myActionType.EnumerationVersionID = iEnumerationVersionID;
                                        model.ACTIONTYPE.Add(myActionType);
                                        model.SaveChanges();
                                    }
                                    else
                                    {
                                        //Update ACTIONTYPE
                                    }
                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                Console.WriteLine("DEBUG documentation=" + node4.InnerText);
                                                myActionType.ActionTypeDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion actiontype
                        bEnumerationProcessed = true;
                    }

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("ActionNameEnum"))
                    {
                        #region actionname
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.ACTIONNAME myActionName;
                                    myActionName = model.ACTIONNAME.FirstOrDefault(o => o.ActionNameName == sEnumerationValue && o.EnumerationVersionID==iEnumerationVersionID);// && o.VocabularyID == iVocabularyCYBOXID);
                                    if (myActionName == null)
                                    {
                                        Console.WriteLine("DEBUG Adding new ACTIONNAME " + sEnumerationValue);
                                        myActionName = new ACTIONNAME();
                                        myActionName.ActionNameName = sEnumerationValue;
                                        myActionName.VocabularyID = iVocabularyCYBOXID;
                                        myActionName.EnumerationVersionID = iEnumerationVersionID;
                                        model.ACTIONNAME.Add(myActionName);
                                        model.SaveChanges();
                                    }
                                    else
                                    {
                                        //Update ACTIONNAME
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                Console.WriteLine("DEBUG documentation=" + node4.InnerText);
                                                myActionName.ActionNameDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion actionname
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("ActionArgumentNameEnum"))
                    {
                        #region actionargument
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.ACTIONARGUMENTNAME myActionArgumentName;
                                    myActionArgumentName = model.ACTIONARGUMENTNAME.FirstOrDefault(o => o.ActionArgumentNameName == sEnumerationValue);// && o.VocabularyID == iVocabularyCYBOXID);
                                    if (myActionArgumentName == null)
                                    {
                                        Console.WriteLine("DEBUG Adding new ACTIONARGUMENTNAME " + sEnumerationValue);
                                        myActionArgumentName = new ACTIONARGUMENTNAME();
                                        myActionArgumentName.ActionArgumentNameName = sEnumerationValue;
                                        myActionArgumentName.VocabularyID = iVocabularyCYBOXID;
                                        model.ACTIONARGUMENTNAME.Add(myActionArgumentName);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                myActionArgumentName.ActionArgumentNameDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion actionargument
                        bEnumerationProcessed = true;
                    }
                    //***

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
                                    myActionObjectAssociationType = model.ACTIONOBJECTASSOCIATIONTYPE.FirstOrDefault(o => o.ActionObjectAssociationTypeName == sEnumerationValue);// && o.VocabularyID == iVocabularyCYBOXID);
                                    if (myActionObjectAssociationType == null)
                                    {
                                        Console.WriteLine("DEBUG Adding new ACTIONOBJECTASSOCIATIONTYPE " + sEnumerationValue);
                                        myActionObjectAssociationType = new ACTIONOBJECTASSOCIATIONTYPE();
                                        myActionObjectAssociationType.ActionObjectAssociationTypeName = sEnumerationValue;
                                        myActionObjectAssociationType.VocabularyID = iVocabularyCYBOXID;
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
                    if (sNodeName.Contains("ActionRelationshipTypeEnum"))
                    {
                        #region actionrelationtype
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.ACTIONRELATIONSHIPTYPE myActionRelationshipType;
                                    myActionRelationshipType = model.ACTIONRELATIONSHIPTYPE.FirstOrDefault(o => o.ActionRelationshipTypeName == sEnumerationValue);// && o.VocabularyID == iVocabularyCYBOXID);
                                    if (myActionRelationshipType == null)
                                    {
                                        Console.WriteLine("DEBUG Adding new ACTIONRELATIONSHIPTYPE " + sEnumerationValue);
                                        myActionRelationshipType = new ACTIONRELATIONSHIPTYPE();
                                        myActionRelationshipType.ActionRelationshipTypeName = sEnumerationValue;
                                        myActionRelationshipType.VocabularyID = iVocabularyCYBOXID;
                                        model.ACTIONRELATIONSHIPTYPE.Add(myActionRelationshipType);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                myActionRelationshipType.ActionRelationshipTypeDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion actionrelationtype
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("EventTypeEnum"))
                    {
                        #region eventtype
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.EVENTTYPE myEventType;
                                    myEventType = model.EVENTTYPE.FirstOrDefault(o => o.EventTypeName == sEnumerationValue);// && o.VocabularyID == iVocabularyCYBOXID);
                                    if (myEventType == null)
                                    {
                                        Console.WriteLine("DEBUG Adding new EVENTTYPE " + sEnumerationValue);
                                        myEventType = new EVENTTYPE();
                                        myEventType.EventTypeName = sEnumerationValue;
                                        myEventType.VocabularyID = iVocabularyCYBOXID;
                                        model.EVENTTYPE.Add(myEventType);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                myEventType.EventTypeDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion eventtype
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("ObjectRelationshipEnum"))
                    {
                        #region objectrelation
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.OBJECTRELATIONSHIP myObjectRelationship;
                                    myObjectRelationship = model.OBJECTRELATIONSHIP.FirstOrDefault(o => o.ObjectRelationshipName == sEnumerationValue);// && o.VocabularyID == iVocabularyCYBOXID);
                                    if (myObjectRelationship == null)
                                    {
                                        Console.WriteLine("DEBUG Adding new OBJECTRELATIONSHIP " + sEnumerationValue);
                                        myObjectRelationship = new OBJECTRELATIONSHIP();
                                        myObjectRelationship.ObjectRelationshipName = sEnumerationValue;
                                        myObjectRelationship.VocabularyID = iVocabularyCYBOXID;
                                        model.OBJECTRELATIONSHIP.Add(myObjectRelationship);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                myObjectRelationship.ObjectRelationshipDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion objectrelation
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("ObjectStateEnum"))
                    {
                        #region objectstate
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.OBJECTSTATE myObjectState;
                                    myObjectState = model.OBJECTSTATE.FirstOrDefault(o => o.ObjectStateName == sEnumerationValue);// && o.VocabularyID == iVocabularyCYBOXID);
                                    if (myObjectState == null)
                                    {
                                        Console.WriteLine("DEBUG Adding new OBJECTSTATE " + sEnumerationValue);
                                        myObjectState = new OBJECTSTATE();
                                        myObjectState.ObjectStateName = sEnumerationValue;
                                        myObjectState.VocabularyID = iVocabularyCYBOXID;
                                        model.OBJECTSTATE.Add(myObjectState);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                myObjectState.ObjectStateDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion objectstate
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("CharacterEncodingEnum"))
                    {
                        #region characterencoding
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.CHARACTERENCODING myCharacterEncoding;
                                    myCharacterEncoding = model.CHARACTERENCODING.FirstOrDefault(o => o.CharacterEncodingName == sEnumerationValue);// && o.VocabularyID == iVocabularyCYBOXID);
                                    if (myCharacterEncoding == null)
                                    {
                                        Console.WriteLine("DEBUG Adding new CHARACTERENCODING " + sEnumerationValue);
                                        myCharacterEncoding = new CHARACTERENCODING();
                                        myCharacterEncoding.CharacterEncodingName = sEnumerationValue;
                                        myCharacterEncoding.VocabularyID = iVocabularyCYBOXID;
                                        model.CHARACTERENCODING.Add(myCharacterEncoding);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                myCharacterEncoding.CharacterEncodingDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion characterencoding
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("InformationSourceTypeEnum"))
                    {
                        #region infosourcetype
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.INFORMATIONSOURCETYPE myInformationSourceType;
                                    myInformationSourceType = model.INFORMATIONSOURCETYPE.FirstOrDefault(o => o.InformationSourceTypeName == sEnumerationValue);// && o.VocabularyID == iVocabularyCYBOXID);
                                    if (myInformationSourceType == null)
                                    {
                                        Console.WriteLine("DEBUG Adding new INFORMATIONSOURCETYPE " + sEnumerationValue);
                                        myInformationSourceType = new INFORMATIONSOURCETYPE();
                                        myInformationSourceType.InformationSourceTypeName = sEnumerationValue;
                                        myInformationSourceType.VocabularyID = iVocabularyCYBOXID;
                                        model.INFORMATIONSOURCETYPE.Add(myInformationSourceType);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                myInformationSourceType.InformationSourceTypeDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion infosourcetype
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("HashNameEnum"))
                    {
                        #region hashname
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.HASHNAME myHashName;
                                    myHashName = model.HASHNAME.FirstOrDefault(o => o.HashingAlgorithmName == sEnumerationValue);// && o.VocabularyID == iVocabularyCYBOXID);
                                    if (myHashName == null)
                                    {
                                        Console.WriteLine("DEBUG Adding new HASHNAME " + sEnumerationValue);
                                        myHashName = new HASHNAME();
                                        myHashName.HashingAlgorithmName = sEnumerationValue;
                                        myHashName.VocabularyID = iVocabularyCYBOXID;
                                        model.HASHNAME.Add(myHashName);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                myHashName.HashingAlgorithmDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion hashname
                        bEnumerationProcessed = true;
                    }
                    //***

                    //***********************************************************************************************************************************
                    if (sNodeName.Contains("ToolTypeEnum"))
                    {
                        #region tooltype
                        foreach (XmlNode node2 in node)
                        {
                            //Console.WriteLine(node2.Name);
                            if (node2.Name == "xs:restriction")
                            {
                                foreach (XmlNode nodeEnumeration in node2)  //xs:enumeration
                                {
                                    string sEnumerationValue = nodeEnumeration.Attributes["value"].InnerText;
                                    XORCISMModel.TOOLTYPE myToolType;
                                    myToolType = model.TOOLTYPE.FirstOrDefault(o => o.ToolTypeName == sEnumerationValue);// && o.VocabularyID == iVocabularyCYBOXID);
                                    if (myToolType == null)
                                    {
                                        Console.WriteLine("DEBUG Adding new TOOLTYPE " + sEnumerationValue);
                                        myToolType = new TOOLTYPE();
                                        myToolType.ToolTypeName = sEnumerationValue;
                                        myToolType.VocabularyID = iVocabularyCYBOXID;
                                        model.TOOLTYPE.Add(myToolType);
                                        model.SaveChanges();
                                    }

                                    foreach (XmlNode node3 in nodeEnumeration)  //xs:annotation
                                    {
                                        foreach (XmlNode node4 in node3)  //xs:documentation
                                        {
                                            if (node4.Name == "xs:documentation")
                                            {
                                                myToolType.ToolTypeDescription = node4.InnerText;
                                                model.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        #endregion tooltype
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
