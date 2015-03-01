using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

using XORCISMModel;

using System.Data.Entity.Validation;

namespace Import_NIST_SP_800_53
{
    static class Program
    {
        /// <summary>
        /// Copyright (C) 2014-2015 Jerome Athias
        /// Import NIST SP 800-53 in an XORCISM database
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
            /*
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            */

            //TODO
            //https://web.nvd.nist.gov/view/800-53/home
            /*
            C:\nvdcve>wget --no-check-certificate https://nvd.nist.gov/static/feeds/xml/sp80053/rev4/800-53-controls.xml
            */
            Console.WriteLine("DEBUG Importing NIST SP 800-53");

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

            int iVocabularyNISTSP80053ID = 0;   // 15;  //NIST SP 800-53
            #region vocabularyNISTSP800-53
            try
            {
                iVocabularyNISTSP80053ID = model.VOCABULARY.Where(o => o.VocabularyName == "NIST SP 800-53").Select(o => o.VocabularyID).FirstOrDefault();
            }
            catch (Exception ex)
            {

            }
            if (iVocabularyNISTSP80053ID <= 0)
            {
                VOCABULARY oVocabulary = new VOCABULARY();
                oVocabulary.CreatedDate = DateTimeOffset.Now;
                oVocabulary.VocabularyName = "NIST SP 800-53";
                model.VOCABULARY.Add(oVocabulary);
                model.SaveChanges();
                iVocabularyNISTSP80053ID = oVocabulary.VocabularyID;
                Console.WriteLine("DEBUG iVocabularyNISTSP80053ID=" + iVocabularyNISTSP80053ID);
            }
            #endregion vocabularyNISTSP800-53


            //TODO: Download if needed
            //Hardcoded
            string sDownloadFileURL = "https://nvd.nist.gov/static/feeds/xml/sp80053/rev4/800-53-controls.xml";
            string sDownloadFileName = "800-53-controls.xml";
            string sDownloadLocalPath = "C:/nvdcve/";
            string sDownloadLocalFolder = @"C:\nvdcve\";
            string sDownloadLocalFile = "800-53-controls.xml";

            XmlDocument doc;
            doc = new XmlDocument();

            //NOTE: probably not the best/fastest way to parse XML but easy/clear enough

            try
            {
                //doc.Load(@"C:\nvdcve\800-53-controls.xml");
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

            mgr.AddNamespace("controls", "http://scap.nist.gov/schema/sp800-53/feed/2.0");

            XmlNodeList nodesSecurityControls=null;
            try
            {
                nodesSecurityControls = doc.SelectNodes("controls:controls/controls:control", mgr);
                //Console.WriteLine("DEBUG " + nodesSecurityControls.Count + " security controls (nodes)");
            }
            catch(Exception exSelectNodes)
            {
                Console.WriteLine("Exception: exSelectNodes " + exSelectNodes.Message + " " + exSelectNodes.InnerException);
            }

            

            try
            {
                foreach (XmlNode nodeSecurityControl in nodesSecurityControls)    //controls:control
                {
                    SECURITYCONTROLFAMILY oSCFamily = null;
                    SECURITYCONTROL oSC = null;
                    //Note: we support 3 level of statements
                    int iSCParentLevel1ID = 0;
                    int iSCParentLevel2ID = 0;
                    int iSCParentLevel3ID = 0;

                    string sSCNumber = "";
                    SECURITYCONTROLPRIORITY oSCPriority = null;

                    foreach (XmlNode nodeSecurityControlInfo in nodeSecurityControl)
                    {
                        //Console.WriteLine("DEBUG nodeSecurityControlInfo.Name=" + nodeSecurityControlInfo.Name);
                        switch (nodeSecurityControlInfo.Name)
                        {
                            case "family":
                                #region family
                                oSCFamily = model.SECURITYCONTROLFAMILY.FirstOrDefault(o => o.SecurityControlFamilyName == nodeSecurityControlInfo.InnerText);
                                if (oSCFamily==null)
                                {
                                    try
                                    {
                                        Console.WriteLine("DEBUG " + DateTimeOffset.Now);

                                        oSCFamily = new SECURITYCONTROLFAMILY();
                                        oSCFamily.SecurityControlFamilyName = nodeSecurityControlInfo.InnerText;
                                        oSCFamily.CreatedDate = DateTimeOffset.Now;
                                        oSCFamily.VocabularyID = iVocabularyNISTSP80053ID;
                                        oSCFamily.timestamp = DateTimeOffset.Now;
                                        model.SECURITYCONTROLFAMILY.Add(oSCFamily);
                                        model.SaveChanges();
                                    }
                                    catch (DbEntityValidationException dbEx)
                                    {
                                        foreach (var validationErrors in dbEx.EntityValidationErrors)
                                        {
                                            foreach (var validationError in validationErrors.ValidationErrors)
                                            {
                                                Console.WriteLine("Exception: Class: {0}, Property: {1}, Error: {2}", validationErrors.Entry.Entity.GetType().FullName,
                                                              validationError.PropertyName, validationError.ErrorMessage);
                                            }
                                        }
                                    }
                                    catch(Exception ex)
                                    {
                                        Console.WriteLine("Exception: " + ex.Message + " " + ex.InnerException);
                                    }
                                }
                                else
                                {
                                    //Update SECURITYCONTROLFAMILY
                                }
                                #endregion family
                                break;

                            case "number":
                                //Level 1
                                sSCNumber = nodeSecurityControlInfo.InnerText;
                                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                Console.WriteLine("DEBUG sSCNumber Level1=" + sSCNumber);
                                oSC = model.SECURITYCONTROL.FirstOrDefault(o => o.SecurityControlVocabularyID == sSCNumber);    //TODO: VocabularyID?
                                if(oSC==null)
                                {
                                    try
                                    {
                                        Console.WriteLine("DEBUG " + DateTimeOffset.Now);

                                        oSC = new SECURITYCONTROL();
                                        oSC.CreatedDate = DateTimeOffset.Now;
                                        oSC.SecurityControlVocabularyID = sSCNumber;    //AC-1
                                        //oSC.SecurityControlAbbrevation=
                                        oSC.SecurityControlName = "";   //Required
                                        if (oSCFamily != null)
                                        {
                                            oSC.SecurityControlFamilyID = oSCFamily.SecurityControlFamilyID;
                                        }
                                        else
                                        {
                                            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                            Console.WriteLine("ERROR: oSCFamily not defined for " + sSCNumber);
                                        }
                                        if (iSCParentLevel1ID != 0)
                                        {
                                            oSC.SecurityControlParentID = iSCParentLevel1ID;
                                        }
                                        oSC.timestamp = DateTimeOffset.Now;
                                        oSC.VocabularyID = iVocabularyNISTSP80053ID;
                                        model.SECURITYCONTROL.Add(oSC);
                                        model.SaveChanges();

                                        //It becomes the new Parent
                                        iSCParentLevel1ID = oSC.SecurityControlID;

                                    }
                                    catch (DbEntityValidationException dbEx)
                                    {
                                        foreach (var validationErrors in dbEx.EntityValidationErrors)
                                        {
                                            foreach (var validationError in validationErrors.ValidationErrors)
                                            {
                                                Console.WriteLine("Exception: Class: {0}, Property: {1}, Error: {2}", validationErrors.Entry.Entity.GetType().FullName,
                                                              validationError.PropertyName, validationError.ErrorMessage);
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine("Exception: " + ex.Message + " " + ex.InnerException);
                                    }
                                }
                                else
                                {
                                    //Update SECURITYCONTROL
                                }
                                break;

                            case "title":
                                try
                                {
                                    //Update SECURITYCONTROL
                                    oSC.SecurityControlName = nodeSecurityControlInfo.InnerText;
                                    oSC.timestamp = DateTimeOffset.Now;
                                    model.SaveChanges();
                                }
                                catch (DbEntityValidationException dbEx)
                                {
                                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                                    {
                                        foreach (var validationError in validationErrors.ValidationErrors)
                                        {
                                            Console.WriteLine("Exception: Class: {0}, Property: {1}, Error: {2}", validationErrors.Entry.Entity.GetType().FullName,
                                                          validationError.PropertyName, validationError.ErrorMessage);
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Exception: " + ex.Message + " " + ex.InnerException);
                                }
                                break;

                            case "priority":
                                //P1
                                try
                                {
                                    PRIORITYLEVEL oPriorityLevel = model.PRIORITYLEVEL.FirstOrDefault(o => o.PriotityCode == nodeSecurityControlInfo.InnerText);    //VocabularyID
                                    if (oPriorityLevel == null)
                                    {
                                        try
                                        {
                                            Console.WriteLine("DEBUG " + DateTimeOffset.Now);

                                            oPriorityLevel = new PRIORITYLEVEL();
                                            oPriorityLevel.CreatedDate = DateTimeOffset.Now;
                                            oPriorityLevel.PriotityCode = nodeSecurityControlInfo.InnerText;    //P1
                                            oPriorityLevel.VocabularyID = iVocabularyNISTSP80053ID;
                                            oPriorityLevel.timestamp = DateTimeOffset.Now;
                                            model.PRIORITYLEVEL.Add(oPriorityLevel);
                                            model.SaveChanges();
                                        }
                                        catch (DbEntityValidationException dbEx)
                                        {
                                            foreach (var validationErrors in dbEx.EntityValidationErrors)
                                            {
                                                foreach (var validationError in validationErrors.ValidationErrors)
                                                {
                                                    Console.WriteLine("Exception: Class: {0}, Property: {1}, Error: {2}", validationErrors.Entry.Entity.GetType().FullName,
                                                                  validationError.PropertyName, validationError.ErrorMessage);
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine("Exception: " + ex.Message + " " + ex.InnerException);
                                        }
                                    }
                                    else
                                    {
                                        //Update PRIORITYLEVEL
                                    }

                                    oSCPriority = model.SECURITYCONTROLPRIORITY.FirstOrDefault(o => o.SecurityControlID == oSC.SecurityControlID && o.PriorityLevelID == oPriorityLevel.PriorityLevelID);
                                    if (oSCPriority == null)
                                    {
                                        try
                                        {
                                            Console.WriteLine("DEBUG " + DateTimeOffset.Now);

                                            oSCPriority = new SECURITYCONTROLPRIORITY();
                                            oSCPriority.CreatedDate = DateTimeOffset.Now;
                                            oSCPriority.SecurityControlID = oSC.SecurityControlID;
                                            oSCPriority.PriorityLevelID = oPriorityLevel.PriorityLevelID;
                                            oSCPriority.VocabularyID = iVocabularyNISTSP80053ID;
                                            oSCPriority.timestamp = DateTimeOffset.Now;
                                            model.SECURITYCONTROLPRIORITY.Add(oSCPriority);
                                            //model.SaveChanges();    //TEST PERFORMANCE

                                        }
                                        catch (DbEntityValidationException dbEx)
                                        {
                                            foreach (var validationErrors in dbEx.EntityValidationErrors)
                                            {
                                                foreach (var validationError in validationErrors.ValidationErrors)
                                                {
                                                    Console.WriteLine("Exception: Class: {0}, Property: {1}, Error: {2}", validationErrors.Entry.Entity.GetType().FullName,
                                                                  validationError.PropertyName, validationError.ErrorMessage);
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                            Console.WriteLine("Exception: " + ex.Message + " " + ex.InnerException);
                                        }
                                    }
                                    else
                                    {
                                        //Update SECURITYCONTROLPRIORITY
                                    }
                                }
                                catch (DbEntityValidationException dbEx)
                                {
                                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                                    {
                                        foreach (var validationError in validationErrors.ValidationErrors)
                                        {
                                            Console.WriteLine("Exception: Class: {0}, Property: {1}, Error: {2}", validationErrors.Entry.Entity.GetType().FullName,
                                                          validationError.PropertyName, validationError.ErrorMessage);
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Exception: " + ex.Message + " " + ex.InnerException);
                                }
                                break;

                            case "baseline-impact":
                                if (oSCPriority!=null)
                                {
                                    try
                                    {
                                        oSC.BaselineImpact = nodeSecurityControlInfo.InnerText; //LOW
                                        oSC.timestamp = DateTimeOffset.Now;
                                        model.SaveChanges();
                                    }
                                    catch (DbEntityValidationException dbEx)
                                    {
                                        foreach (var validationErrors in dbEx.EntityValidationErrors)
                                        {
                                            foreach (var validationError in validationErrors.ValidationErrors)
                                            {
                                                Console.WriteLine("Exception: Class: {0}, Property: {1}, Error: {2}", validationErrors.Entry.Entity.GetType().FullName,
                                                              validationError.PropertyName, validationError.ErrorMessage);
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine("Exception: " + ex.Message + " " + ex.InnerException);
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("ERROR: no SECURITYCONTROLPRIORITY for baseline-impact for " + sSCNumber);
                                }
                                break;

                            case "statement":
                                //TODO: Review. We consider a statement as a security control
                                foreach (XmlNode nodeSCStatement in nodeSecurityControlInfo)
                                {
                                    switch(nodeSCStatement.Name)
                                    {
                                        case "description":
                                            try
                                            {
                                                oSC.StatementDescription = nodeSCStatement.InnerText;   //The organization:
                                                oSC.timestamp = DateTimeOffset.Now;
                                                model.SaveChanges();
                                            }
                                            catch (DbEntityValidationException dbEx)
                                            {
                                                foreach (var validationErrors in dbEx.EntityValidationErrors)
                                                {
                                                    foreach (var validationError in validationErrors.ValidationErrors)
                                                    {
                                                        Console.WriteLine("Exception: Class: {0}, Property: {1}, Error: {2}", validationErrors.Entry.Entity.GetType().FullName,
                                                                      validationError.PropertyName, validationError.ErrorMessage);
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine("Exception: " + ex.Message + " " + ex.InnerException);
                                            }
                                            break;

                                        case "statement":
                                            //Level 2
                                            #region statementlevel2
                                            foreach (XmlNode nodeSCStatementInfo in nodeSCStatement)
                                            {
                                                switch(nodeSCStatementInfo.Name)
                                                {
                                                    case "number":
                                                        //Level 2
                                                        sSCNumber = nodeSCStatementInfo.InnerText;
                                                        Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                        Console.WriteLine("DEBUG sSCNumber Level 2=" + sSCNumber);
                                                        oSC = model.SECURITYCONTROL.FirstOrDefault(o => o.SecurityControlVocabularyID == sSCNumber);    //TODO: VocabularyID?
                                                        if(oSC==null)
                                                        {
                                                            try
                                                            {
                                                                oSC = new SECURITYCONTROL();
                                                                oSC.CreatedDate = DateTimeOffset.Now;
                                                                oSC.SecurityControlVocabularyID = sSCNumber;    //AC-1a.
                                                                oSC.SecurityControlName = "";   //Required
                                                                //oSC.SecurityControlAbbrevation=
                                                                if (oSCFamily != null)
                                                                {
                                                                    oSC.SecurityControlFamilyID = oSCFamily.SecurityControlFamilyID;
                                                                }
                                                                else
                                                                {
                                                                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                                    Console.WriteLine("ERROR: oSCFamily not defined for " + sSCNumber);
                                                                }
                                                                if (iSCParentLevel1ID != 0)
                                                                {
                                                                    oSC.SecurityControlParentID = iSCParentLevel1ID;
                                                                }
                                                                oSC.timestamp = DateTimeOffset.Now;
                                                                oSC.VocabularyID = iVocabularyNISTSP80053ID;
                                                                model.SECURITYCONTROL.Add(oSC);
                                                                model.SaveChanges();

                                                                //It becomes the new Parent Level2
                                                                iSCParentLevel2ID = oSC.SecurityControlID;
                                                            }
                                                            catch (DbEntityValidationException dbEx)
                                                            {
                                                                foreach (var validationErrors in dbEx.EntityValidationErrors)
                                                                {
                                                                    foreach (var validationError in validationErrors.ValidationErrors)
                                                                    {
                                                                        Console.WriteLine("Exception: Class: {0}, Property: {1}, Error: {2}", validationErrors.Entry.Entity.GetType().FullName,
                                                                                      validationError.PropertyName, validationError.ErrorMessage);
                                                                    }
                                                                }
                                                            }
                                                            catch (Exception ex)
                                                            {
                                                                Console.WriteLine("Exception: " + ex.Message + " " + ex.InnerException);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            //Update SECURITYCONTROL
                                                        }
                                                        break;

                                                    case "description":
                                                        try
                                                        {
                                                            oSC.StatementDescription = nodeSCStatementInfo.InnerText;   //Develops, documents, and disseminates to [Assignment: organization-defined personnel or roles]:
                                                            oSC.timestamp = DateTimeOffset.Now;
                                                            model.SaveChanges();
                                                        }
                                                        catch (DbEntityValidationException dbEx)
                                                        {
                                                            foreach (var validationErrors in dbEx.EntityValidationErrors)
                                                            {
                                                                foreach (var validationError in validationErrors.ValidationErrors)
                                                                {
                                                                    Console.WriteLine("Exception: Class: {0}, Property: {1}, Error: {2}", validationErrors.Entry.Entity.GetType().FullName,
                                                                                  validationError.PropertyName, validationError.ErrorMessage);
                                                                }
                                                            }
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                            Console.WriteLine("Exception: " + ex.Message + " " + ex.InnerException);
                                                        }
                                                        break;

                                                    case "statement":
                                                        //Level 3

                                                        #region statementlevel3
                                                        foreach (XmlNode nodeSCStatementInfo3 in nodeSCStatementInfo)
                                                        {
                                                            switch (nodeSCStatementInfo3.Name)
                                                            {
                                                                case "number":
                                                                    //Level 3
                                                                    sSCNumber = nodeSCStatementInfo3.InnerText; //AC-1a.1.
                                                                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                                    Console.WriteLine("DEBUG sSCNumber Level 3=" + sSCNumber);
                                                                    oSC = model.SECURITYCONTROL.FirstOrDefault(o => o.SecurityControlVocabularyID == sSCNumber);    //TODO: VocabularyID?
                                                                    if (oSC == null)
                                                                    {
                                                                        try
                                                                        {
                                                                            oSC = new SECURITYCONTROL();
                                                                            oSC.CreatedDate = DateTimeOffset.Now;
                                                                            oSC.SecurityControlVocabularyID = sSCNumber;    //AC-1a.1.
                                                                            //oSC.SecurityControlAbbrevation=
                                                                            oSC.SecurityControlName = "";   //Required
                                                                            if (oSCFamily != null)
                                                                            {
                                                                                oSC.SecurityControlFamilyID = oSCFamily.SecurityControlFamilyID;
                                                                            }
                                                                            else
                                                                            {
                                                                                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                                                Console.WriteLine("ERROR: oSCFamily not defined for " + sSCNumber);
                                                                            }
                                                                            if (iSCParentLevel2ID != 0)
                                                                            {
                                                                                oSC.SecurityControlParentID = iSCParentLevel2ID;
                                                                            }
                                                                            oSC.timestamp = DateTimeOffset.Now;
                                                                            oSC.VocabularyID = iVocabularyNISTSP80053ID;
                                                                            model.SECURITYCONTROL.Add(oSC);
                                                                            model.SaveChanges();

                                                                            //It becomes the new Parent Level3  //Note:not used
                                                                            iSCParentLevel3ID = oSC.SecurityControlID;
                                                                        }
                                                                        catch (DbEntityValidationException dbEx)
                                                                        {
                                                                            foreach (var validationErrors in dbEx.EntityValidationErrors)
                                                                            {
                                                                                foreach (var validationError in validationErrors.ValidationErrors)
                                                                                {
                                                                                    Console.WriteLine("Exception: Class: {0}, Property: {1}, Error: {2}", validationErrors.Entry.Entity.GetType().FullName,
                                                                                                  validationError.PropertyName, validationError.ErrorMessage);
                                                                                }
                                                                            }
                                                                        }
                                                                        catch (Exception ex)
                                                                        {
                                                                            Console.WriteLine("Exception: " + ex.Message + " " + ex.InnerException);
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        //Update SECURITYCONTROL
                                                                    }
                                                                    break;

                                                                case "description":
                                                                    try
                                                                    {
                                                                        //Update SECURITYCONTROL
                                                                        oSC.StatementDescription = nodeSCStatementInfo3.InnerText;   //Develops, documents, and disseminates to [Assignment: organization-defined personnel or roles]:
                                                                        oSC.timestamp = DateTimeOffset.Now;
                                                                        model.SaveChanges();
                                                                    }
                                                                    catch (DbEntityValidationException dbEx)
                                                                    {
                                                                        foreach (var validationErrors in dbEx.EntityValidationErrors)
                                                                        {
                                                                            foreach (var validationError in validationErrors.ValidationErrors)
                                                                            {
                                                                                Console.WriteLine("Exception: Class: {0}, Property: {1}, Error: {2}", validationErrors.Entry.Entity.GetType().FullName,
                                                                                              validationError.PropertyName, validationError.ErrorMessage);
                                                                            }
                                                                        }
                                                                    }
                                                                    catch (Exception ex)
                                                                    {
                                                                        Console.WriteLine("Exception: " + ex.Message + " " + ex.InnerException);
                                                                    }
                                                                    break;

                                                                case "statement":
                                                                    //Level 4
                                                                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                                    Console.WriteLine("ERROR: Missing code for statement Level 4 for " + sSCNumber);
                                                                    break;

                                                                default:
                                                                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                                    Console.WriteLine("ERROR: Missing code for nodeSCStatementInfo3.Name " + nodeSCStatementInfo3.Name + " for " + sSCNumber);
                                                                    break;
                                                            }
                                                        }
                                                        #endregion statementlevel3

                                                        break;

                                                    default:
                                                        Console.WriteLine("ERROR: Missing code for nodeSCStatementInfo.Name " + nodeSCStatementInfo.Name+" for "+sSCNumber);
                                                        break;
                                                }
                                            }
                                            #endregion statementlevel2
                                            break;

                                        default:
                                            Console.WriteLine("ERROR: Missing code for nodeSCStatement.Name " + nodeSCStatement.Name + " for " + sSCNumber);
                                            break;
                                    }
                                }
                                break;

                            case "supplemental-guidance":
                                //Going back to level 1
                                oSC = model.SECURITYCONTROL.FirstOrDefault(o => o.SecurityControlID == iSCParentLevel1ID);
                                if (oSC == null)
                                {
                                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                    Console.WriteLine("ERROR: SECURITYCONTROL Level1 not found for supplemental-guidance iSCParentLevel1ID=" + iSCParentLevel1ID);
                                }
                                else
                                {
                                    //Update SECURITYCONTROL
                                    foreach (XmlNode nodeGuidanceInfo in nodeSecurityControlInfo)
                                    {
                                        switch(nodeGuidanceInfo.Name)
                                        {
                                            case "description":
                                                try
                                                {
                                                    //Update SECURITYCONTROL
                                                    oSC.SecurityControlDescription = nodeGuidanceInfo.InnerText;    //Cleaning?
                                                    oSC.timestamp = DateTimeOffset.Now;
                                                    model.SaveChanges();
                                                }
                                                catch (DbEntityValidationException dbEx)
                                                {
                                                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                                                    {
                                                        foreach (var validationError in validationErrors.ValidationErrors)
                                                        {
                                                            Console.WriteLine("Exception: Class: {0}, Property: {1}, Error: {2}", validationErrors.Entry.Entity.GetType().FullName,
                                                                          validationError.PropertyName, validationError.ErrorMessage);
                                                        }
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    Console.WriteLine("Exception: " + ex.Message + " " + ex.InnerException);
                                                }
                                                break;

                                            case "related":
                                                string sSCNumberRelated=nodeGuidanceInfo.InnerText; //PM-9
                                                SECURITYCONTROL oSCRelated=model.SECURITYCONTROL.FirstOrDefault(o=>o.SecurityControlVocabularyID==sSCNumberRelated);    //VocabularyID?
                                                if (oSCRelated==null)
                                                {
                                                    //The Security Control related still does not exist in the database. Adding it
                                                    try
                                                    {
                                                        Console.WriteLine("DEBUG " + DateTimeOffset.Now);

                                                        oSCRelated = new SECURITYCONTROL();
                                                        oSCRelated.CreatedDate = DateTimeOffset.Now;
                                                        oSCRelated.SecurityControlVocabularyID = sSCNumberRelated;
                                                        oSCRelated.SecurityControlName = "";    //Required
                                                        oSCRelated.VocabularyID = iVocabularyNISTSP80053ID;
                                                        oSCRelated.timestamp = DateTimeOffset.Now;
                                                        model.SECURITYCONTROL.Add(oSCRelated);
                                                        model.SaveChanges();
                                                    }
                                                    catch (DbEntityValidationException dbEx)
                                                    {
                                                        foreach (var validationErrors in dbEx.EntityValidationErrors)
                                                        {
                                                            foreach (var validationError in validationErrors.ValidationErrors)
                                                            {
                                                                Console.WriteLine("Exception: Class: {0}, Property: {1}, Error: {2}", validationErrors.Entry.Entity.GetType().FullName,
                                                                              validationError.PropertyName, validationError.ErrorMessage);
                                                            }
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.WriteLine("Exception: " + ex.Message + " " + ex.InnerException);
                                                    }
                                                }
                                                else
                                                {
                                                    //Update SECURITYCONTROL
                                                }

                                                SECURITYCONTROLMAPPING oSCMapping = model.SECURITYCONTROLMAPPING.FirstOrDefault(o => o.SecurityControlRefID == oSC.SecurityControlID && o.SecurityControlSubjectID == oSCRelated.SecurityControlID);
                                                if (oSCMapping==null)
                                                {
                                                    try
                                                    {
                                                        oSCMapping = new SECURITYCONTROLMAPPING();
                                                        oSCMapping.CreatedDate = DateTimeOffset.Now;
                                                        oSCMapping.SecurityControlRefID = oSC.SecurityControlID;
                                                        oSCMapping.SecurityControlRelationship = "Related";
                                                        oSCMapping.SecurityControlSubjectID = oSCRelated.SecurityControlID;
                                                        oSCMapping.VocabularyID = iVocabularyNISTSP80053ID;
                                                        oSCMapping.timestamp = DateTimeOffset.Now;
                                                        model.SECURITYCONTROLMAPPING.Add(oSCMapping);
                                                        model.SaveChanges();    //TEST PERFORMANCE
                                                    }
                                                    catch (DbEntityValidationException dbEx)
                                                    {
                                                        foreach (var validationErrors in dbEx.EntityValidationErrors)
                                                        {
                                                            foreach (var validationError in validationErrors.ValidationErrors)
                                                            {
                                                                Console.WriteLine("Exception: Class: {0}, Property: {1}, Error: {2}", validationErrors.Entry.Entity.GetType().FullName,
                                                                              validationError.PropertyName, validationError.ErrorMessage);
                                                            }
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                        Console.WriteLine("Exception: " + ex.Message + " " + ex.InnerException);
                                                    }
                                                }
                                                else
                                                {
                                                    //Update SECURITYCONTROLMAPPING
                                                }
                                                break;
                                            default:
                                                Console.WriteLine("ERROR: Missing code for supplemental-guidance " + nodeGuidanceInfo.Name);
                                                break;
                                        }
                                    }
                                }
                                break;

                            case "references":
                                //Going back to level 1
                                oSC = model.SECURITYCONTROL.FirstOrDefault(o => o.SecurityControlID == iSCParentLevel1ID);
                                if (oSC == null)
                                {
                                    Console.WriteLine("ERROR: SECURITYCONTROL Level1 not found for references iSCParentLevel1ID=" + iSCParentLevel1ID);
                                }
                                else
                                {
                                    //Update SECURITYCONTROL
                                    foreach (XmlNode nodeReference in nodeSecurityControlInfo)
                                    {
                                        foreach (XmlNode nodeReferenceItem in nodeReference)
                                        {
                                            if(nodeReferenceItem.Name=="item")
                                            {
                                                string sReferenceURL = nodeReferenceItem.Attributes["href"].InnerText;
                                                //TODO NORMALIZE
                                                //TODO: Cleaning    replace("http://www.","http://");
                                                //TODO: replace("https://www.","https://");
                                                sReferenceURL = sReferenceURL.Replace("http://www.", "http://");
                                                sReferenceURL = sReferenceURL.Replace("https://www.", "https://").Trim();

                                                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                Console.WriteLine("DEBUG sReferenceURL=" + sReferenceURL);
                                                //TODO  nodeReferenceItem.Attributes["xml:lang"].InnerText  en-US
                                                REFERENCE oReference = model.REFERENCE.FirstOrDefault(o => o.ReferenceURL == sReferenceURL);
                                                if(oReference==null)
                                                {
                                                    try
                                                    {
                                                        Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                        Console.WriteLine("DEBUG Adding REFERENCE");
                                                        oReference = new REFERENCE();
                                                        oReference.CreatedDate = DateTimeOffset.Now;
                                                        oReference.ReferenceURL = sReferenceURL;
                                                        oReference.ReferenceTitle = nodeReferenceItem.InnerText;
                                                        oReference.VocabularyID = iVocabularyNISTSP80053ID;
                                                        oReference.timestamp = DateTimeOffset.Now;
                                                        model.REFERENCE.Add(oReference);
                                                        model.SaveChanges();
                                                    }
                                                    catch (DbEntityValidationException dbEx)
                                                    {
                                                        foreach (var validationErrors in dbEx.EntityValidationErrors)
                                                        {
                                                            foreach (var validationError in validationErrors.ValidationErrors)
                                                            {
                                                                Console.WriteLine("Exception: Class: {0}, Property: {1}, Error: {2}", validationErrors.Entry.Entity.GetType().FullName,
                                                                              validationError.PropertyName, validationError.ErrorMessage);
                                                            }
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.WriteLine("Exception: " + ex.Message + " " + ex.InnerException);
                                                    }
                                                }
                                                else
                                                {
                                                    //Update REFERENCE
                                                }

                                                SECURITYCONTROLREFERENCE oSCReference = model.SECURITYCONTROLREFERENCE.FirstOrDefault(o => o.SecurityControlID == oSC.SecurityControlID && o.ReferenceID == oReference.ReferenceID);
                                                if(oSCReference==null)
                                                {
                                                    try
                                                    {
                                                        oSCReference = new SECURITYCONTROLREFERENCE();
                                                        oSCReference.CreatedDate = DateTimeOffset.Now;
                                                        oSCReference.SecurityControlID = oSC.SecurityControlID;
                                                        oSCReference.ReferenceID = oReference.ReferenceID;
                                                        oSCReference.VocabularyID = iVocabularyNISTSP80053ID;
                                                        oSCReference.timestamp = DateTimeOffset.Now;
                                                        model.SECURITYCONTROLREFERENCE.Add(oSCReference);
                                                        //model.SaveChanges();    //TEST PERFORMANCE
                                                    }
                                                    catch (DbEntityValidationException dbEx)
                                                    {
                                                        foreach (var validationErrors in dbEx.EntityValidationErrors)
                                                        {
                                                            foreach (var validationError in validationErrors.ValidationErrors)
                                                            {
                                                                Console.WriteLine("Exception: Class: {0}, Property: {1}, Error: {2}", validationErrors.Entry.Entity.GetType().FullName,
                                                                              validationError.PropertyName, validationError.ErrorMessage);
                                                            }
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.WriteLine("Exception: " + ex.Message + " " + ex.InnerException);
                                                    }
                                                }
                                                else
                                                {
                                                    //Update SECURITYCONTROLREFERENCE
                                                }
                                            }
                                            else
                                            {
                                                Console.WriteLine("ERROR: Missing code for nodeReferenceItem " + nodeReferenceItem.Name);
                                            }
                                        }
                                    }
                                }
                                break;

                            case "control-enhancements":
                                foreach (XmlNode nodeSCEnhancement in nodeSecurityControlInfo)
                                {
                                    switch (nodeSCEnhancement.Name)
                                    {
                                        case "control-enhancement":
                                            foreach (XmlNode nodeSCEnhancementInfo in nodeSCEnhancement)
                                            {
                                                switch(nodeSCEnhancementInfo.Name)
                                                {
                                                    case "number":
                                                        sSCNumber = nodeSCEnhancementInfo.InnerText;
                                                        Console.WriteLine("DEBUG sSCNumber enhancement=" + sSCNumber);
                                                        oSC = model.SECURITYCONTROL.FirstOrDefault(o => o.SecurityControlVocabularyID == sSCNumber);    //TODO: VocabularyID?
                                                        if (oSC == null)
                                                        {
                                                            try
                                                            {
                                                                oSC = new SECURITYCONTROL();
                                                                oSC.CreatedDate = DateTimeOffset.Now;
                                                                oSC.SecurityControlVocabularyID = sSCNumber;    //CP-2 (1)
                                                                //oSC.SecurityControlAbbrevation=
                                                                oSC.SecurityControlName = "";   //Required
                                                                if (oSCFamily != null)
                                                                {
                                                                    oSC.SecurityControlFamilyID = oSCFamily.SecurityControlFamilyID;
                                                                }
                                                                else
                                                                {
                                                                    Console.WriteLine("ERROR: oSCFamily not defined for " + sSCNumber);
                                                                }
                                                                if (iSCParentLevel1ID != 0)
                                                                {
                                                                    oSC.SecurityControlParentID = iSCParentLevel1ID;
                                                                }
                                                                oSC.timestamp = DateTimeOffset.Now;
                                                                oSC.VocabularyID = iVocabularyNISTSP80053ID;
                                                                model.SECURITYCONTROL.Add(oSC);
                                                                model.SaveChanges();

                                                                //It becomes the new Parent
                                                                //TODO Review
                                                                //iSCParentLevel1ID = oSC.SecurityControlID;
                                                            }
                                                            catch (DbEntityValidationException dbEx)
                                                            {
                                                                foreach (var validationErrors in dbEx.EntityValidationErrors)
                                                                {
                                                                    foreach (var validationError in validationErrors.ValidationErrors)
                                                                    {
                                                                        Console.WriteLine("Exception40 Class: {0}, Property: {1}, Error: {2}", validationErrors.Entry.Entity.GetType().FullName,
                                                                                      validationError.PropertyName, validationError.ErrorMessage);
                                                                    }
                                                                }
                                                            }
                                                            catch (Exception ex)
                                                            {
                                                                Console.WriteLine("Exception: " + ex.Message + " " + ex.InnerException);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            //Update SECURITYCONTROL
                                                        }
                                                        break;

                                                    case "title":
                                                        try
                                                        {
                                                            //Update SECURITYCONTROL
                                                            oSC.SecurityControlName = nodeSCEnhancementInfo.InnerText;
                                                            oSC.timestamp = DateTimeOffset.Now;
                                                            model.SaveChanges();
                                                        }
                                                        catch (DbEntityValidationException dbEx)
                                                        {
                                                            foreach (var validationErrors in dbEx.EntityValidationErrors)
                                                            {
                                                                foreach (var validationError in validationErrors.ValidationErrors)
                                                                {
                                                                    Console.WriteLine("Exception41 Class: {0}, Property: {1}, Error: {2}", validationErrors.Entry.Entity.GetType().FullName,
                                                                                  validationError.PropertyName, validationError.ErrorMessage);
                                                                }
                                                            }
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            Console.WriteLine("Exception: " + ex.Message + " " + ex.InnerException);
                                                        }
                                                        break;
                                                    /*
                                                    case "baseline-impact":
                                                        //TODO: oSC.BaselineImpact=

                                                        break;
                                                    */


                                                    case "withdrawn":
                                                        foreach (XmlNode nodeSCEnhancementInfoWithdrawn in nodeSCEnhancementInfo)
                                                        {
                                                            if (nodeSCEnhancementInfoWithdrawn.Name == "incorporated-into")
                                                            {
                                                                //TODO

                                                            }
                                                            else
                                                            {
                                                                Console.WriteLine("ERROR: missing code for nodeSCEnhancementInfoWithdrawn " + nodeSCEnhancementInfoWithdrawn.Name);
                                                            }
                                                        }
                                                        break;


                                                    case "statement":
                                                        foreach (XmlNode nodeSCEnhancementInfoStatement in nodeSCEnhancementInfo)
                                                        {
                                                            switch(nodeSCEnhancementInfoStatement.Name)
                                                            {
                                                                case "description":
                                                                    try
                                                                    {
                                                                        //Update SECURITYCONTROL
                                                                        oSC.StatementDescription = nodeSCEnhancementInfoStatement.InnerText;    //Cleaning?
                                                                        oSC.timestamp = DateTimeOffset.Now;
                                                                        model.SaveChanges();
                                                                    }
                                                                    catch (DbEntityValidationException dbEx)
                                                                    {
                                                                        foreach (var validationErrors in dbEx.EntityValidationErrors)
                                                                        {
                                                                            foreach (var validationError in validationErrors.ValidationErrors)
                                                                            {
                                                                                Console.WriteLine("Exception: Class: {0}, Property: {1}, Error: {2}", validationErrors.Entry.Entity.GetType().FullName,
                                                                                              validationError.PropertyName, validationError.ErrorMessage);
                                                                            }
                                                                        }
                                                                    }
                                                                    catch (Exception ex)
                                                                    {
                                                                        Console.WriteLine("Exception: " + ex.Message + " " + ex.InnerException);
                                                                    }
                                                                    break;

                                                                default:
                                                                    Console.WriteLine("ERROR: missing code for nodeSCEnhancementInfoStatement " + nodeSCEnhancementInfoStatement.Name);
                                                                    break;
                                                            }
                                                        }
                                                        break;

                                                    case "supplemental-guidance":
                                                        foreach (XmlNode nodeSCEnhancementInfoGuidance in nodeSCEnhancementInfo)
                                                        {
                                                            switch(nodeSCEnhancementInfoGuidance.Name)
                                                            {
                                                                case "description":
                                                                    try
                                                                    {
                                                                        //Update SECURITYCONTROL
                                                                        oSC.SecurityControlDescription = nodeSCEnhancementInfoGuidance.InnerText;
                                                                        oSC.timestamp = DateTimeOffset.Now;
                                                                        model.SaveChanges();
                                                                    }
                                                                    catch (DbEntityValidationException dbEx)
                                                                    {
                                                                        foreach (var validationErrors in dbEx.EntityValidationErrors)
                                                                        {
                                                                            foreach (var validationError in validationErrors.ValidationErrors)
                                                                            {
                                                                                Console.WriteLine("Exception: Class: {0}, Property: {1}, Error: {2}", validationErrors.Entry.Entity.GetType().FullName,
                                                                                              validationError.PropertyName, validationError.ErrorMessage);
                                                                            }
                                                                        }
                                                                    }
                                                                    catch (Exception ex)
                                                                    {
                                                                        Console.WriteLine("Exception: " + ex.Message + " " + ex.InnerException);
                                                                    }
                                                                    break;

                                                                case "related":
                                                                    string sSCNumberRelated=nodeSCEnhancementInfoGuidance.InnerText; //PE-12
                                                                    SECURITYCONTROL oSCRelated=model.SECURITYCONTROL.FirstOrDefault(o=>o.SecurityControlVocabularyID==sSCNumberRelated);    //VocabularyID?
                                                                    if (oSCRelated==null)
                                                                    {
                                                                        //The Security Control related still does not exist in the database. Adding it
                                                                        try
                                                                        {
                                                                            oSCRelated = new SECURITYCONTROL();
                                                                            oSCRelated.CreatedDate = DateTimeOffset.Now;
                                                                            oSCRelated.SecurityControlVocabularyID = sSCNumberRelated;
                                                                            oSCRelated.SecurityControlName = "";    //Required
                                                                            oSCRelated.VocabularyID = iVocabularyNISTSP80053ID;
                                                                            oSCRelated.timestamp = DateTimeOffset.Now;
                                                                            model.SECURITYCONTROL.Add(oSCRelated);
                                                                            model.SaveChanges();
                                                                        }
                                                                        catch (DbEntityValidationException dbEx)
                                                                        {
                                                                            foreach (var validationErrors in dbEx.EntityValidationErrors)
                                                                            {
                                                                                foreach (var validationError in validationErrors.ValidationErrors)
                                                                                {
                                                                                    Console.WriteLine("Exception: Class: {0}, Property: {1}, Error: {2}", validationErrors.Entry.Entity.GetType().FullName,
                                                                                                  validationError.PropertyName, validationError.ErrorMessage);
                                                                                }
                                                                            }
                                                                        }
                                                                        catch (Exception ex)
                                                                        {
                                                                            Console.WriteLine("Exception: " + ex.Message + " " + ex.InnerException);
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        //Update SECURITYCONTROL
                                                                    }

                                                                    SECURITYCONTROLMAPPING oSCMapping = model.SECURITYCONTROLMAPPING.FirstOrDefault(o => o.SecurityControlRefID == oSC.SecurityControlID && o.SecurityControlSubjectID == oSCRelated.SecurityControlID);
                                                                    if (oSCMapping==null)
                                                                    {
                                                                        try
                                                                        {
                                                                            oSCMapping = new SECURITYCONTROLMAPPING();
                                                                            oSCMapping.CreatedDate = DateTimeOffset.Now;
                                                                            oSCMapping.SecurityControlRefID = oSC.SecurityControlID;
                                                                            oSCMapping.SecurityControlRelationship = "Related";
                                                                            oSCMapping.SecurityControlSubjectID = oSCRelated.SecurityControlID;
                                                                            oSCMapping.VocabularyID = iVocabularyNISTSP80053ID;
                                                                            oSCMapping.timestamp = DateTimeOffset.Now;
                                                                            model.SECURITYCONTROLMAPPING.Add(oSCMapping);
                                                                            //model.SaveChanges();    //TEST PERFORMANCE
                                                                        }
                                                                        catch (DbEntityValidationException dbEx)
                                                                        {
                                                                            foreach (var validationErrors in dbEx.EntityValidationErrors)
                                                                            {
                                                                                foreach (var validationError in validationErrors.ValidationErrors)
                                                                                {
                                                                                    Console.WriteLine("Exception: Class: {0}, Property: {1}, Error: {2}", validationErrors.Entry.Entity.GetType().FullName,
                                                                                                  validationError.PropertyName, validationError.ErrorMessage);
                                                                                }
                                                                            }
                                                                        }
                                                                        catch (Exception ex)
                                                                        {
                                                                            Console.WriteLine("Exception: " + ex.Message + " " + ex.InnerException);
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        //Update SECURITYCONTROLMAPPING
                                                                    }
                                                                    break;
                                                                default:
                                                                    Console.WriteLine("ERROR: missing code for nodeSCEnhancementInfoGuidance " + nodeSCEnhancementInfoGuidance.Name);
                                                                    break;
                                                            }
                                                        }
                                                        break;

                                                    default:
                                                        Console.WriteLine("ERROR: missing code for nodeSCEnhancementInfo " + nodeSCEnhancementInfo.Name);
                                                        //baseline-impact
                                                        break;
                                                }
                                            }
                                            break;

                                        default:
                                            Console.WriteLine("ERROR: missing code for nodeSCEnhancement " + nodeSCEnhancement.Name);
                                            break;
                                    }
                                }
                                break;

                            default:
                                Console.WriteLine("ERROR: missing code for nodeSecurityControlInfo " + nodeSecurityControlInfo.Name);
                                break;
                        }
                    }
                }
            }
            catch(Exception exnodesSecurityControls)
            {
                Console.WriteLine("Exception: exnodesSecurityControls " + exnodesSecurityControls.Message + " " + exnodesSecurityControls.InnerException);
            }

            //FREE
            model.SaveChanges();
            model.Dispose();
            model = null;

            Console.WriteLine("DEBUG "+DateTimeOffset.Now.ToString());
            Console.WriteLine("COMPLETED IMPORT NIST SP 800-53 FINISHED");

        }
    }
}
