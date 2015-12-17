using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;

using XORCISMModel;

using System.Data.Entity.Validation;

namespace Import_CommonCriteria
{
    class Program
    {
        /// <summary>
        /// Copyright (C) 2015 Jerome Athias
        /// Imports the Common Criteria (CC) XML file in an XORCISM database
        /// All trademarks and registered trademarks are the property of their respective owners.
        /// This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation; either version 2 of the License, or (at your option) any later version.
        /// 
        /// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
        /// 
        /// You should have received a copy of the GNU General Public License along with this program; if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
        /// </summary>
        
        static void Main(string[] args)
        {
            //  Use cc3R4.xml from https://www.commoncriteriaportal.org/cc/

            Console.WriteLine("DEBUG Importing Common Criteria");

            XORCISMEntities model = null;
            try
            {
                model = new XORCISMEntities();
                Console.WriteLine("DEBUG Model loaded");
            }
            catch (Exception exModel)
            {
                Console.WriteLine("Exception: exModel " + exModel.Message + " " + exModel.InnerException);
            }

            int iVocabularyCommonCriteriaID = 0;
            #region vocabularyCommonCriteria
            try
            {
                iVocabularyCommonCriteriaID = model.VOCABULARY.Where(o => o.VocabularyName == "Common Criteria").Select(o => o.VocabularyID).FirstOrDefault();
            }
            catch (Exception ex)
            {

            }
            if (iVocabularyCommonCriteriaID <= 0)
            {
                VOCABULARY oVocabulary = new VOCABULARY();
                oVocabulary.CreatedDate = DateTimeOffset.Now;
                oVocabulary.VocabularyName = "Common Criteria";  //Hardcoded
                //TODO? Version
                model.VOCABULARY.Add(oVocabulary);
                model.SaveChanges();
                iVocabularyCommonCriteriaID = oVocabulary.VocabularyID;
                Console.WriteLine("DEBUG iVocabularyCommonCriteriaID=" + iVocabularyCommonCriteriaID);
            }
            else
            {
                //Update VocabularyCommonCriteria
            }
            #endregion vocabularyCommonCriteria


            //TODO: Download if needed
            //Hardcoded
            string sDownloadFileURL = "https://www.commoncriteriaportal.org/files/ccfiles/cc3R4.xml.zip";
            string sDownloadFileName = "cc3R4.xml.zip";
            string sDownloadLocalPath = "C:/nvdcve/";
            string sDownloadLocalFolder = @"C:\nvdcve\";
            string sDownloadLocalFile = "cc3R4.xml";

            XmlDocument doc;
            doc = new XmlDocument();

            //NOTE: probably not the best/fastest way to parse XML but easy/clear enough

            try
            {
                //doc.Load(@"X:\SOURCES\Import_CommonCriteria\bin\Release\cc3R4.xml");
                doc.Load(sDownloadLocalFolder + sDownloadLocalFile);
                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                Console.WriteLine("DEBUG XML file loaded");
            }
            catch (Exception exdocLoad)
            {
                Console.WriteLine("Exception: exdocLoad :\n" + exdocLoad.Message + " " + exdocLoad.InnerException);
            }

            
            XmlNamespaceManager mgr = null;
            try
            {
                mgr = new XmlNamespaceManager(doc.NameTable);
            }
            catch (Exception exXmlNamespaceManager)
            {
                Console.WriteLine("Exception: exXmlNamespaceManager :\n" + exXmlNamespaceManager.Message + " " + exXmlNamespaceManager.InnerException);
            }
            /*
            mgr.AddNamespace("clauses", "");
            */

            XmlNode XMLRoot = doc.DocumentElement;
            XmlNodeList XMLNodes=XMLRoot.ChildNodes;
            foreach(XmlNode XNode in XMLNodes)
            {
                //Console.WriteLine("DEBUG XNode.Name: " + XNode.Name);
                //<clause title="Terms and definitions, symbols and abbreviated terms" id="a-definitions" category="normative" type="normal">
                switch(XNode.Name)
                {
                    case "clause":
                        try
                        {
                            string XNodeTitle=XNode.Attributes["title"].InnerText;
                            Console.WriteLine("DEBUG XNode.title: " + XNodeTitle);
                            if(XNodeTitle.Contains("Terms and definitions"))    //HARDCODED
                            {

                                /*
                                <glossentry id="action">
                                    <glossterm>action</glossterm>
                                    <glossdef>evaluator action element of the CC Part 3</glossdef>
                                    <glossnote>
                                        These actions are either explicitly stated as evaluator actions or implicitly derived from developer actions (implied evaluator actions) within the CC Part 3 assurance components.
                                    </glossnote>
                                </glossentry>
                                */
                                //  We list all the gloassry definitions
                                XmlNodeList XMLGlossEntries = XNode.ChildNodes;
                                foreach(XmlNode XGlossEntry in XMLGlossEntries)
                                {
                                    try
                                    {
                                        Console.WriteLine("DEBUG XGlossEntry.id: " + XGlossEntry.Attributes["id"].InnerText);
                                    }
                                    catch (Exception exGlossEntryID)
                                    {
                                        Console.WriteLine("Exception exGlossEntryID: " + exGlossEntryID.Message + " " + exGlossEntryID.InnerException);
                                    }
                        
                                }

                            }
                        }
                        catch(Exception exXNodeTitle)
                        {
                            Console.WriteLine("Exception exXNodeTitle: " + exXNodeTitle.Message + " " + exXNodeTitle.InnerException);
                        }
                        
                        try
                        {
                            //Console.WriteLine("DEBUG XNode.id: " + XNode.Attributes["id"].InnerText);
                        }
                        catch (Exception exXNodeID)
                        {
                            Console.WriteLine("Exception exXNodeID: " + exXNodeID.Message + " " + exXNodeID.InnerException);
                        }
                        break;
                        
                        try
                        {
                            //Console.WriteLine("DEBUG XNode.category: " + XNode.Attributes["category"].InnerText);
                        }
                        catch (Exception exXNodeCategory)
                        {
                            Console.WriteLine("Exception exXNodeCategory: " + exXNodeCategory.Message + " " + exXNodeCategory.InnerException);
                        }
                        break;

                        //******************************************************************************************************************************************
                        //<subclause title="Organisation of CC Part 3" id="assurance-scope-organisation">
                        //TODO

                        
                    case "f-class":
                        //TODO
                        break;
                    case "a-class": //Assurance Class
                        //TODO
                        break;
                    case "eal":     //Evaluation Assurance Level
                        //TODO
                        break;
                    case "cap":     //?
                        //TODO
                        break;
                    case "patchinfo":
                        //TODO
                        break;
                    default:
                        Console.WriteLine("ERROR Missing code for " + XNode.Name);
                        break;
                }
            }

        }
    }
}
