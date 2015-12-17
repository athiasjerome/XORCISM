using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

using XORCISMModel;
using XOVALModel;

using System.Text.RegularExpressions;

using Microsoft.Office.Core;
using Microsoft.Office.Interop.Excel;

namespace Import_cce
{
    static class Program
    {
        /// <summary>
        /// Copyright (C) 2014 Jerome Athias
        /// Parser for CCE XML file and import the values into an XORCISM database
        /// This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation; either version 2 of the License, or (at your option) any later version.
        /// 
        /// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
        /// 
        /// You should have received a copy of the GNU General Public License along with this program; if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
        /// </summary>
        [STAThread]
        static void Main()
        {
            //TODO
            //http://static.nvd.nist.gov/feeds/xml/cce/nvdcce-0.1-feed.xml (Mapping with SECURITYCONTROL NIST SP 800-53)

            XmlDocument doc;
            doc = new XmlDocument();

            //https://nvd.nist.gov/cce/index.cfm
            doc.Load(@"C:\nvdcve\cce-COMBINED-5.20130214.xml"); //Hardcoded

            XORCISMEntities model = new XORCISMEntities();
            //https://stackoverflow.com/questions/5940225/fastest-way-of-inserting-in-entity-framework
            model.Configuration.AutoDetectChangesEnabled = false;
            model.Configuration.ValidateOnSaveEnabled = false;

            XOVALEntities oval_model = new XOVALEntities();
            oval_model.Configuration.AutoDetectChangesEnabled = false;
            oval_model.Configuration.ValidateOnSaveEnabled = false;

            int iVocabularyCCEID = 0;   // 7;

            #region vocabularycce
            try
            {
                iVocabularyCCEID = model.VOCABULARY.Where(o => o.VocabularyName == "CCE").Select(o => o.VocabularyID).FirstOrDefault();
            }
            catch (Exception ex)
            {

            }
            if (iVocabularyCCEID <= 0)
            {
                VOCABULARY oVocabulary = new VOCABULARY();
                oVocabulary.CreatedDate = DateTimeOffset.Now;
                oVocabulary.VocabularyName = "CCE";
                oVocabulary.timestamp = DateTimeOffset.Now;
                model.VOCABULARY.Add(oVocabulary);
                model.SaveChanges();
                iVocabularyCCEID = oVocabulary.VocabularyID;
                Console.WriteLine("DEBUG iVocabularyCCEID=" + iVocabularyCCEID);
            }
            #endregion vocabularcce

            XmlNamespaceManager mgr = new XmlNamespaceManager(doc.NameTable);

            mgr.AddNamespace("cce", "http://cce.mitre.org");    //Hardcoded

            XmlNodeList nodes1;
            nodes1 = doc.SelectNodes("cce:cce_list", mgr);  //Hardcoded
            //Console.WriteLine(nodes1.Count);

            //TODO  change cce_id == myCCEID   to use CCEID
            
            #region CCE
            foreach (XmlNode nodeCCEList in nodes1)    //cce:cce_list
            {
                //Console.WriteLine(nodeCCEList.Name);
                foreach (XmlNode nodeCCES in nodeCCEList)    //cces
                {
                    //Console.WriteLine("DEBUG "+nodeCCES.Name);
                    switch (nodeCCES.Name)
                    {
                        case "cces":
                            #region cces
                            foreach (XmlNode nodeCCE in nodeCCES)    //cce
                            {
                                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                Console.WriteLine("DEBUG nodeCCE.Name=" + nodeCCE.Name);
                                string myCCEID = nodeCCE.Attributes["cce_id"].InnerText;
                                Console.WriteLine("DEBUG myCCEID=" + myCCEID);

                                XORCISMModel.CCE myCCE = new CCE();
                                myCCE = model.CCE.FirstOrDefault(o => o.cce_id == myCCEID);
                                if (myCCE == null)
                                {
                                    Console.WriteLine("DEBUG Adding CCE" + " " + myCCEID);
                                    myCCE = new CCE();
                                    myCCE.cce_id = myCCEID;
                                    myCCE.CreatedDate = DateTimeOffset.Now;
                                    myCCE.VocabularyID = iVocabularyCCEID;
                                    myCCE.timestamp = DateTimeOffset.Now;
                                    model.CCE.Add(myCCE);
                                    model.SaveChanges();
                                }
                                else
                                {
                                    //Update CCE
                                }

                                string sPlatform = nodeCCE.Attributes["platform"].InnerText;
                                //Update CCE
                                myCCE.platform = sPlatform; //TODO? Remove (see PLATFORMFORCCE)

                                //TODO: search for CPE
                                //XORCISMModel.PLATFORM myPlatform = new PLATFORM();
                                //myPlatform = model.PLATFORM.FirstOrDefault(o => o.PlatformName == sPlatform);   //&&o.VocabularyID=iVocabularyCCEID
                                int iPlatformID = 0;
                                try
                                {
                                    iPlatformID = model.PLATFORM.Where(o => o.PlatformName == sPlatform).Select(o=>o.PlatformID).FirstOrDefault(); 
                                }
                                catch(Exception ex)
                                {

                                }
                                //if (myPlatform == null)
                                if (iPlatformID<=0)
                                {
                                    Console.WriteLine("DEBUG Adding PLATFORM "+sPlatform);
                                    PLATFORM myPlatform = new PLATFORM();
                                    myPlatform.PlatformName = sPlatform;
                                    myPlatform.CreatedDate = DateTimeOffset.Now;
                                    myPlatform.VocabularyID = iVocabularyCCEID;
                                    myPlatform.timestamp = DateTimeOffset.Now;
                                    model.PLATFORM.Add(myPlatform);
                                    model.SaveChanges();
                                    iPlatformID = myPlatform.PlatformID;
                                }
                                else
                                {
                                    //Update PLATFORM
                                }

                                //TODO PLATFORMMAPPING or PRODUCT (e.g. ie7)

                                //XORCISMModel.PLATFORMFORCCE myPlatformForCCE = new PLATFORMFORCCE();
                                //myPlatformForCCE = model.PLATFORMFORCCE.FirstOrDefault(o => o.PlatformID == iPlatformID && o.cce_id == myCCEID);
                                int iCCEPlatformID = 0;
                                try
                                {
                                    iCCEPlatformID = model.PLATFORMFORCCE.Where(o => o.PlatformID == iPlatformID && o.cce_id == myCCEID).Select(o=>o.CCEPlatformID).FirstOrDefault();
                                }
                                catch(Exception ex)
                                {

                                }
                                //if (myPlatformForCCE == null)
                                if (iCCEPlatformID<=0)
                                {
                                    Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                    Console.WriteLine("DEBUG Adding PLATFORMFORCCE");
                                    PLATFORMFORCCE myPlatformForCCE = new PLATFORMFORCCE();
                                    myPlatformForCCE.CreatedDate = DateTimeOffset.Now;
                                    myPlatformForCCE.PlatformID = iPlatformID;  // myPlatform.PlatformID;
                                    myPlatformForCCE.cce_id = myCCEID;  //TODO Remove
                                    myPlatformForCCE.CCEID = myCCE.CCEID;
                                    myPlatformForCCE.VocabularyID = iVocabularyCCEID;
                                    myPlatformForCCE.timestamp = DateTimeOffset.Now;
                                    model.PLATFORMFORCCE.Add(myPlatformForCCE);
                                    //model.SaveChanges();    //TEST PERFORMANCE
                                    //iCCEPlatformID=
                                }
                                else
                                {
                                    //Update PLATFORMFORCCE
                                }

                                string modifieddate = nodeCCE.Attributes["modified"].InnerText;
                                //2009-04-30
                                //we need 30/04/2009
                                //converting the date
                                string[] format = { "yyyy-MM-dd" };
                                DateTime date;
                                if (DateTime.TryParseExact(modifieddate, format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out date))
                                {
                                    //valid
                                }
                                //Update CCE
                                myCCE.modified = date; //DateTimeOffset.Parse(modifieddate);

                                foreach (XmlNode nodeCCEAtt in nodeCCE)
                                {
                                    switch (nodeCCEAtt.Name)
                                    {
                                        case "description":
                                            //Update CCE
                                            myCCE.description = nodeCCEAtt.InnerText;   //TODO Cleaning?
                                            //TODO? Parsing the description with Regex

                                            break;
                                        case "parameters":
                                            #region cceparameters
                                            foreach (XmlNode nodeCCEParameter in nodeCCEAtt)
                                            {
                                                if (nodeCCEParameter.Name == "parameter")
                                                {
                                                    //XORCISMModel.CCEPARAMETER myCCEParameter = new CCEPARAMETER();
                                                    //myCCEParameter = model.CCEPARAMETER.FirstOrDefault(o => o.CCEParameterText == nodeCCEParameter.InnerText);
                                                    int iCCEParameterID = 0;
                                                    try
                                                    {
                                                        iCCEParameterID = model.CCEPARAMETER.Where(o => o.CCEParameterText == nodeCCEParameter.InnerText).Select(o=>o.CCEParameterID).FirstOrDefault();
                                                    }
                                                    catch(Exception ex)
                                                    {

                                                    }
                                                    //if (myCCEParameter == null)
                                                    if (iCCEParameterID<=0)
                                                    {
                                                        Console.WriteLine("DEBUG Adding CCEPARAMETER" + " " + myCCEID);
                                                        CCEPARAMETER myCCEParameter = new CCEPARAMETER();
                                                        myCCEParameter.CreatedDate = DateTimeOffset.Now;
                                                        myCCEParameter.CCEParameterText = nodeCCEParameter.InnerText;   //Cleaning?
                                                        myCCEParameter.VocabularyID = iVocabularyCCEID;
                                                        myCCEParameter.timestamp = DateTimeOffset.Now;
                                                        model.CCEPARAMETER.Add(myCCEParameter);
                                                        model.SaveChanges();
                                                        iCCEParameterID = myCCEParameter.CCEParameterID;
                                                    }
                                                    else
                                                    {
                                                        //Update CCEPARAMETER
                                                    }

                                                    //XORCISMModel.CCEPARAMETERFORCCE myCCEParameterFORCCE = new CCEPARAMETERFORCCE();
                                                    //myCCEParameterFORCCE = model.CCEPARAMETERFORCCE.FirstOrDefault(o => o.CCEParameterID == myCCEParameter.CCEParameterID && o.cce_id == myCCEID);
                                                    int iCCECCEParameterID = 0;
                                                    try
                                                    {
                                                        iCCECCEParameterID = model.CCEPARAMETERFORCCE.Where(o => o.CCEParameterID == iCCEParameterID && o.CCEID == myCCE.CCEID).Select(o => o.CCECCEParameterID).FirstOrDefault();
                                                    }
                                                    catch(Exception ex)
                                                    {

                                                    }
                                                    //if (myCCEParameterFORCCE == null)
                                                    if (iCCECCEParameterID<=0)
                                                    {
                                                        Console.WriteLine("DEBUG Adding CCEPARAMETERFORCCE" + " " + myCCEID);
                                                        CCEPARAMETERFORCCE myCCEParameterFORCCE = new CCEPARAMETERFORCCE();
                                                        myCCEParameterFORCCE.CreatedDate = DateTimeOffset.Now;
                                                        myCCEParameterFORCCE.CCEParameterID = iCCEParameterID;  // myCCEParameter.CCEParameterID;
                                                        //myCCEParameterFORCCE.cce_id = myCCEID;  //Removed
                                                        myCCEParameterFORCCE.CCEID = myCCE.CCEID;
                                                        myCCEParameterFORCCE.VocabularyID = iVocabularyCCEID;
                                                        myCCEParameterFORCCE.timestamp = DateTimeOffset.Now;
                                                        model.CCEPARAMETERFORCCE.Add(myCCEParameterFORCCE);
                                                        //model.SaveChanges();    //TEST PERFORMANCE
                                                        //iCCECCEParameterID=
                                                    }
                                                    else
                                                    {
                                                        //Update CCEPARAMETERFORCCE
                                                    }

                                                }
                                                else
                                                {
                                                    Console.WriteLine("ERROR Import_cce missing code for CCE parameters " + nodeCCEParameter.Name);
                                                }
                                            }
                                            #endregion cceparameters
                                            break;

                                        case "technical_mechanisms":
                                            #region ccetechnicalmechanisms
                                            foreach (XmlNode nodeCCETechMech in nodeCCEAtt)
                                            {
                                                if (nodeCCETechMech.Name == "technical_mechanism")
                                                {
                                                    //XORCISMModel.CCETECHNICALMECHANISM myCCETechMech = new CCETECHNICALMECHANISM();
                                                    //myCCETechMech = model.CCETECHNICALMECHANISM.FirstOrDefault(o => o.TechnicalMechanismText == nodeCCETechMech.InnerText);
                                                    int iCCETechnicalMechanismID = 0;
                                                    try
                                                    {
                                                        iCCETechnicalMechanismID = model.CCETECHNICALMECHANISM.Where(o => o.TechnicalMechanismText == nodeCCETechMech.InnerText).Select(o => o.CCETechnicalMechanismID).FirstOrDefault();
                                                    }
                                                    catch(Exception ex)
                                                    {

                                                    }
                                                    //if (myCCETechMech == null)
                                                    if (iCCETechnicalMechanismID<=0)
                                                    {
                                                        Console.WriteLine("DEBUG Adding CCETECHNICALMECHANISM" + " " + myCCEID);
                                                        CCETECHNICALMECHANISM myCCETechMech = new CCETECHNICALMECHANISM();
                                                        myCCETechMech.CreatedDate = DateTimeOffset.Now;
                                                        myCCETechMech.TechnicalMechanismText = nodeCCETechMech.InnerText;   //Cleaning? Regex?
                                                        myCCETechMech.VocabularyID = iVocabularyCCEID;
                                                        myCCETechMech.timestamp = DateTimeOffset.Now;
                                                        model.CCETECHNICALMECHANISM.Add(myCCETechMech);
                                                        model.SaveChanges();
                                                        iCCETechnicalMechanismID = myCCETechMech.CCETechnicalMechanismID;
                                                    }
                                                    else
                                                    {
                                                        //Update CCETECHNICALMECHANISM
                                                    }

                                                    //XORCISMModel.CCETECHNICALMECHANISMFORCCE myCCETechMechFORCCE = new CCETECHNICALMECHANISMFORCCE();
                                                    //myCCETechMechFORCCE = model.CCETECHNICALMECHANISMFORCCE.FirstOrDefault(o => o.CCETechnicalMechanismID == myCCETechMech.CCETechnicalMechanismID && o.cce_id == myCCEID);
                                                    int iCCECCETechnicalMechanismID = 0;
                                                    try
                                                    {
                                                        iCCECCETechnicalMechanismID = model.CCETECHNICALMECHANISMFORCCE.Where(o => o.CCETechnicalMechanismID == iCCETechnicalMechanismID && o.CCEID == myCCE.CCEID).Select(o => o.CCECCETechnicalMechanismID).FirstOrDefault();
                                                    }
                                                    catch(Exception ex)
                                                    {

                                                    }
                                                    //if (myCCETechMechFORCCE == null)
                                                    if (iCCECCETechnicalMechanismID<=0)
                                                    {
                                                        Console.WriteLine("DEBUG Adding CCETECHNICALMECHANISMFORCCE" + " " + myCCEID);
                                                        CCETECHNICALMECHANISMFORCCE myCCETechMechFORCCE = new CCETECHNICALMECHANISMFORCCE();
                                                        myCCETechMechFORCCE.CreatedDate = DateTimeOffset.Now;
                                                        myCCETechMechFORCCE.CCETechnicalMechanismID = iCCETechnicalMechanismID; // myCCETechMech.CCETechnicalMechanismID;
                                                        //myCCETechMechFORCCE.cce_id = myCCEID;   //Removed
                                                        myCCETechMechFORCCE.CCEID = myCCE.CCEID;
                                                        myCCETechMechFORCCE.VocabularyID = iVocabularyCCEID;
                                                        myCCETechMechFORCCE.timestamp = DateTimeOffset.Now;
                                                        model.CCETECHNICALMECHANISMFORCCE.Add(myCCETechMechFORCCE);
                                                        //model.SaveChanges();    //TEST PERFORMANCE
                                                        //iCCECCETechnicalMechanismID=
                                                    }
                                                    else
                                                    {
                                                        //Update CCETECHNICALMECHANISMFORCCE
                                                    }
                                                }
                                                else
                                                {
                                                    Console.WriteLine("ERROR Import_cce missing code for CCE technical_mechanisms " + nodeCCETechMech.Name);
                                                }
                                            }
                                            #endregion ccetechnicalmechanisms
                                            break;

                                        case "references":
                                            #region ccereferences
                                            foreach (XmlNode nodeCCERef in nodeCCEAtt)
                                            {
                                                if (nodeCCERef.Name == "reference")
                                                {
                                                    
                                                    string sCCERefResID = nodeCCERef.Attributes["resource_id"].InnerText;
                                                    string sCCERefText=nodeCCERef.InnerText;    //Cleaning?
                                                    //TODO: Extract URL and Normalize

                                                    //XORCISMModel.CCEREFERENCE myCCERef = new CCEREFERENCE();
                                                    //myCCERef = model.CCEREFERENCE.FirstOrDefault(o => o.resource_id == sCCERefResID && o.ReferenceText == sCCERefText);
                                                    int iCCEReferenceID = 0;
                                                    try
                                                    {
                                                        iCCEReferenceID = model.CCEREFERENCE.Where(o => o.resource_id == sCCERefResID && o.ReferenceText == sCCERefText).Select(o=>o.CCEReferenceID).FirstOrDefault();
                                                    }
                                                    catch(Exception ex)
                                                    {

                                                    }
                                                    //if (myCCERef == null)
                                                    if (iCCEReferenceID<=0)
                                                    {
                                                        Console.WriteLine("DEBUG Adding CCEREFERENCE" + " " + myCCEID);
                                                        CCEREFERENCE myCCERef = new CCEREFERENCE();
                                                        myCCERef.resource_id = sCCERefResID;    //TODO: delete, see below
                                                        myCCERef.ReferenceText = nodeCCERef.InnerText;  //Cleaning?
                                                        //OVALDEFINITIONCCE
                                                        //oval:org.mitre.oval:def:824, oval:org.mitre.oval:def:732
                                                        //Definition 'oval:gov.nist.usgcb.windowsseven:def:147'
                                                        Regex myRegexOVALDEFINITION = new Regex(@"oval:(.*?):def:[0-9]+"); //TODO Review
                                                        foreach (Match match in myRegexOVALDEFINITION.Matches(nodeCCERef.InnerText))
                                                        {
                                                            Console.WriteLine("DEBUG OVAL Definition found "+match.Value);
                                                            int iOVALDefinitionID = 0;
                                                            try
                                                            {
                                                                iOVALDefinitionID = oval_model.OVALDEFINITION.FirstOrDefault(o => o.OVALDefinitionIDPattern == match.Value).OVALDefinitionID;
                                                            }
                                                            catch
                                                            {

                                                            }
                                                            if (iOVALDefinitionID <= 0)
                                                            {
                                                                try
                                                                {
                                                                    OVALDEFINITION oOVALDefinition = new OVALDEFINITION();
                                                                    oOVALDefinition.CreatedDate = DateTimeOffset.Now;
                                                                    oOVALDefinition.OVALDefinitionIDPattern = match.Value;
                                                                    //NOTE: no criteria
                                                                    oOVALDefinition.VocabularyID = iVocabularyCCEID;
                                                                    oOVALDefinition.timestamp = DateTimeOffset.Now;
                                                                    oval_model.OVALDEFINITION.Add(oOVALDefinition);
                                                                    oval_model.SaveChanges();
                                                                    iOVALDefinitionID = oOVALDefinition.OVALDefinitionID;
                                                                }
                                                                catch (Exception exoOVALDefinition)
                                                                {
                                                                    Console.WriteLine("Exception exoOVALDefinition " + exoOVALDefinition.Message + " " + exoOVALDefinition.InnerException);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                //Update OVALDEFINITION
                                                            }

                                                            int iOVALDefinitionCCE = 0;
                                                            try
                                                            {
                                                                iOVALDefinitionCCE=oval_model.OVALDEFINITIONCCE.FirstOrDefault(o => o.OVALDefinitionID == iOVALDefinitionID && o.CCEID == myCCE.CCEID).OVALDefinitionCCEID;
                                                            }
                                                            catch
                                                            {

                                                            }
                                                            if(iOVALDefinitionCCE<=0)
                                                            {

                                                                OVALDEFINITIONCCE oOVALDefinitionCCE = new OVALDEFINITIONCCE();
                                                                oOVALDefinitionCCE.CreatedDate = DateTimeOffset.Now;
                                                                oOVALDefinitionCCE.OVALDefinitionID = iOVALDefinitionID;
                                                                oOVALDefinitionCCE.CCEID = myCCE.CCEID;

                                                                oOVALDefinitionCCE.VocabularyID = iVocabularyCCEID;
                                                                oOVALDefinitionCCE.timestamp = DateTimeOffset.Now;
                                                                oval_model.OVALDEFINITIONCCE.Add(oOVALDefinitionCCE);
                                                                oval_model.SaveChanges();    //TEST PERFORMANCE
                                                            }
                                                            else
                                                            {
                                                                //Update OVALDEFINITIONCCE
                                                            }
                                                        }


                                                        model.CCEREFERENCE.Add(myCCERef);
                                                        model.SaveChanges();
                                                        iCCEReferenceID = myCCERef.CCEReferenceID;
                                                    }
                                                    else
                                                    {
                                                        //Update CCEREFERENCE
                                                    }

                                                    //XORCISMModel.CCEREFERENCEFORCCE myCCERefFORCCE = new CCEREFERENCEFORCCE();
                                                    //myCCERefFORCCE = model.CCEREFERENCEFORCCE.FirstOrDefault(o => o.CCEReferenceID == myCCERef.CCEReferenceID && o.cce_id == myCCEID);
                                                    int iCCECCEReferenceID = 0;
                                                    try
                                                    {
                                                        iCCECCEReferenceID = model.CCEREFERENCEFORCCE.Where(o => o.CCEReferenceID == iCCEReferenceID && o.cce_id == myCCEID).Select(o=>o.CCECCEReferenceID).FirstOrDefault();
                                                    }
                                                    catch(Exception ex)
                                                    {

                                                    }
                                                    //if (myCCERefFORCCE == null)
                                                    if (iCCECCEReferenceID<=0)
                                                    {
                                                        Console.WriteLine("DEBUG Adding CCEREFERENCEFORCCE" + " " + myCCEID);
                                                        CCEREFERENCEFORCCE myCCERefFORCCE = new CCEREFERENCEFORCCE();
                                                        myCCERefFORCCE.CreatedDate = DateTimeOffset.Now;
                                                        myCCERefFORCCE.CCEReferenceID = iCCEReferenceID;    // myCCERef.CCEReferenceID;
                                                        myCCERefFORCCE.cce_id = myCCEID;
                                                        myCCERefFORCCE.VocabularyID = iVocabularyCCEID;
                                                        myCCERefFORCCE.timestamp = DateTimeOffset.Now;
                                                        model.CCEREFERENCEFORCCE.Add(myCCERefFORCCE);
                                                        //model.SaveChanges();    //TEST PERFORMANCE
                                                        //iCCECCEReferenceID=
                                                    }
                                                    else
                                                    {
                                                        //Update CCEREFERENCEFORCCE
                                                    }
                                                    
                                                    //see below
                                                    
                                                    //XORCISMModel.CCERESOURCE myCCEResource = new CCERESOURCE();
                                                    //myCCEResource = model.CCERESOURCE.FirstOrDefault(o => o.resource_id == sCCERefResID);
                                                    int iCCEResourceID = 0;
                                                    try
                                                    {
                                                        iCCEResourceID = model.CCERESOURCE.Where(o => o.resource_id == sCCERefResID).Select(o=>o.CCEResourceID).FirstOrDefault();
                                                    }
                                                    catch(Exception ex)
                                                    {

                                                    }
                                                    //if (myCCEResource == null)
                                                    if (iCCEResourceID<=0)
                                                    {
                                                        Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                        Console.WriteLine("DEBUG Adding CCERESOURCE for CCEREFERENCE");
                                                        CCERESOURCE myCCEResource = new CCERESOURCE();
                                                        myCCEResource.CreatedDate = DateTimeOffset.Now;
                                                        myCCEResource.resource_id = sCCERefResID;

                                                        myCCEResource.VocabularyID = iVocabularyCCEID;
                                                        myCCEResource.timestamp = DateTimeOffset.Now;
                                                        model.CCERESOURCE.Add(myCCEResource);
                                                        model.SaveChanges();
                                                        iCCEResourceID = myCCEResource.CCEResourceID;
                                                    }
                                                    else
                                                    {
                                                        //Update CCERESOURCE
                                                    }

                                                    //XORCISMModel.CCERESOURCEFORCCEREFERENCE myCCEResourceForRef = new CCERESOURCEFORCCEREFERENCE();
                                                    //myCCEResourceForRef = model.CCERESOURCEFORCCEREFERENCE.FirstOrDefault(o => o.CCEResourceID == myCCEResource.CCEResourceID && o.CCEReferenceID == myCCERef.CCEReferenceID);
                                                    int iCCERefCCEResourceID = 0;
                                                    try
                                                    {
                                                        iCCERefCCEResourceID = model.CCERESOURCEFORCCEREFERENCE.Where(o => o.CCEResourceID == iCCEResourceID && o.CCEReferenceID == iCCEReferenceID).Select(o=>o.CCEReferenceCCEResourceID).FirstOrDefault();
                                                    }
                                                    catch(Exception ex)
                                                    {

                                                    }
                                                    //if (myCCEResourceForRef == null)
                                                    if (iCCERefCCEResourceID<=0)
                                                    {
                                                        Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                        Console.WriteLine("DEBUG Adding CCERESOURCEFORCCEREFERENCE");
                                                        CCERESOURCEFORCCEREFERENCE myCCEResourceForRef = new CCERESOURCEFORCCEREFERENCE();
                                                        myCCEResourceForRef.CreatedDate = DateTimeOffset.Now;
                                                        myCCEResourceForRef.CCEResourceID = iCCEResourceID; // myCCEResource.CCEResourceID;
                                                        myCCEResourceForRef.CCEReferenceID = iCCEReferenceID;
                                                        myCCEResourceForRef.VocabularyID = iVocabularyCCEID;
                                                        myCCEResourceForRef.timestamp = DateTimeOffset.Now;
                                                        model.CCERESOURCEFORCCEREFERENCE.Add(myCCEResourceForRef);
                                                        //model.SaveChanges();    //TEST PERFORMANCE
                                                        //iCCERefCCEResourceID=
                                                    }
                                                    else
                                                    {
                                                        //Update CCERESOURCEFORCCEREFERENCE
                                                    }
                                                }
                                                else
                                                {
                                                    Console.WriteLine("ERROR: Import_cce missing code for CCE references " + nodeCCERef.Name);
                                                }
                                            }
                                            #endregion ccereferences
                                            break;

                                        default:
                                            Console.WriteLine("ERROR: Import_cce missing code for CCE " + nodeCCEAtt.Name);
                                            break;
                                    }
                                }
                                model.SaveChanges();

                            }
                            #endregion cces
                            break;
                        case "resources":
                            #region resources
                            foreach (XmlNode nodeResource in nodeCCES)    //resource
                            {
                                if (nodeResource.Name == "resource")
                                {
                                    XORCISMModel.CCERESOURCE myCCEResource = new CCERESOURCE();
                                    string sCCEResID = nodeResource.Attributes["resource_id"].InnerText;
                                    myCCEResource = model.CCERESOURCE.FirstOrDefault(o => o.resource_id == sCCEResID);
                                    if (myCCEResource == null)
                                    {
                                        Console.WriteLine("DEBUG Adding CCERESOURCE");
                                        myCCEResource = new CCERESOURCE();
                                        myCCEResource.CreatedDate = DateTimeOffset.Now;
                                        myCCEResource.resource_id = sCCEResID;
                                        myCCEResource.VocabularyID = iVocabularyCCEID;
                                        myCCEResource.timestamp = DateTimeOffset.Now;
                                        model.CCERESOURCE.Add(myCCEResource);
                                        model.SaveChanges();
                                    }
                                    else
                                    {
                                        //Update CCERESOURCE
                                    }

                                    //Update CCERESOURCE
                                    myCCEResource.modified = nodeResource.Attributes["modified"].InnerText;
                                    foreach (XmlNode nodeResAtt in nodeResource)
                                    {
                                        switch (nodeResAtt.Name)
                                        {
                                            case "dcterms:title":
                                                //Update CCERESOURCE
                                                myCCEResource.ResourceTitle = nodeResAtt.InnerText;
                                                break;

                                            case "dcterms:publisher":
                                                //Update CCERESOURCE
                                                myCCEResource.ResourcePublisher = nodeResAtt.InnerText;
                                                break;

                                            case "dcterms:issued":
                                                //Update CCERESOURCE
                                                myCCEResource.issued = nodeResAtt.InnerText;
                                                break;

                                            case "version":
                                                //Update CCERESOURCE
                                                myCCEResource.ResourceVersion = nodeResAtt.InnerText;
                                                break;

                                            case "dcterms:format":
                                                //Update CCERESOURCE
                                                myCCEResource.ResourceFormat = nodeResAtt.InnerText;
                                                break;

                                            case "dcterms:creator":
                                                #region CCERESOURCEAUTHOR
                                                #region AUTHOR
                                                string sAuthor = nodeResAtt.InnerText;  //Cleaning?

                                                //XORCISMModel.AUTHOR myAuthor = new AUTHOR();
                                                //myAuthor = model.AUTHOR.FirstOrDefault(o => o.AuthorName == sAuthor);
                                                int iAuthorID = 0;
                                                try
                                                {
                                                    iAuthorID = model.AUTHOR.Where(o => o.AuthorName == sAuthor).Select(o=>o.AuthorID).FirstOrDefault();
                                                }
                                                catch(Exception ex)
                                                {

                                                }
                                                //if (myAuthor == null)
                                                if(iAuthorID<=0)
                                                {
                                                    Console.WriteLine("DEBUG Adding AUTHOR");
                                                    AUTHOR myAuthor = new AUTHOR();
                                                    myAuthor.CreatedDate = DateTimeOffset.Now;
                                                    myAuthor.AuthorName = sAuthor;
                                                    myAuthor.VocabularyID = iVocabularyCCEID;
                                                    //TODO: Search PERSON, ORGANISATION
                                                    myAuthor.timestamp = DateTimeOffset.Now;
                                                    model.AUTHOR.Add(myAuthor);
                                                    model.SaveChanges();
                                                    iAuthorID = myAuthor.AuthorID;
                                                }
                                                else
                                                {
                                                    //Update AUTHOR
                                                }
                                                #endregion AUTHOR

                                                //XORCISMModel.CCERESOURCEAUTHOR myResourceAuthor = new CCERESOURCEAUTHOR();
                                                //myResourceAuthor = model.CCERESOURCEAUTHOR.FirstOrDefault(o => o.AuthorID == myAuthor.AuthorID && o.CCEResourceID == myCCEResource.CCEResourceID);
                                                int iCCEResourceAuthorID = 0;
                                                try
                                                {
                                                    iCCEResourceAuthorID = model.CCERESOURCEAUTHOR.Where(o => o.AuthorID == iAuthorID && o.CCEResourceID == myCCEResource.CCEResourceID).Select(o=>o.CCEResourceAuthorID).FirstOrDefault();
                                                }
                                                catch(Exception ex)
                                                {

                                                }
                                                //if (myResourceAuthor == null)
                                                if (iCCEResourceAuthorID<=0)
                                                {

                                                    Console.WriteLine("DEBUG Adding CCERESOURCEAUTHOR");
                                                    CCERESOURCEAUTHOR myResourceAuthor = new CCERESOURCEAUTHOR();
                                                    myResourceAuthor.CreatedDate = DateTimeOffset.Now;
                                                    myResourceAuthor.AuthorID = iAuthorID;  // myAuthor.AuthorID;
                                                    myResourceAuthor.CCEResourceID = myCCEResource.CCEResourceID;
                                                    myResourceAuthor.VocabularyID = iVocabularyCCEID;
                                                    myResourceAuthor.timestamp = DateTimeOffset.Now;
                                                    model.CCERESOURCEAUTHOR.Add(myResourceAuthor);
                                                    //model.SaveChanges();    //TEST PERFORMANCE
                                                    //iCCEResourceAuthorID=
                                                }
                                                else
                                                {
                                                    //Update CCERESOURCEAUTHOR
                                                }
                                                #endregion CCERESOURCEAUTHOR
                                                break;

                                            default:
                                                Console.WriteLine("ERROR: Import_cce missing code for CCE resource " + nodeResAtt.Name);
                                                break;
                                        }
                                    }
                                    model.SaveChanges();

                                }
                                else
                                {
                                    Console.WriteLine("ERROR: Import_cce missing code for CCE resources " + nodeResource.Name);
                                }
                            }
                            #endregion resources
                            break;

                        default:
                            Console.WriteLine("ERROR: Missing code for nodeCCES.Name=" + nodeCCES.Name);
                            break;
                    }
                }
                
            }
            #endregion CCE

           


            #region ReadExcel
            ////this.openFileDialog1.FileName = "*.xls";
            ////if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            ////{
            //    var ExcelObj = new Microsoft.Office.Interop.Excel.Application();
            //    //openFileDialog1.FileName
            //    Workbook theWorkbook = ExcelObj.Workbooks.Open(@"C:\nvdcve\cce-tomcat6-5.20130214.xls", 0, true, 5, "", "", true, XlPlatform.xlWindows, "\t", false, false, 0, true);
            //    Sheets sheets = theWorkbook.Worksheets;
            //    Worksheet worksheet = (Worksheet)sheets.get_Item(1);
            //    for (int i = 1; i <= 10; i++)
            //    {
            //        Range range = worksheet.get_Range("A" + i.ToString(), "J" + i.ToString());
            //        System.Array myvalues = (System.Array)range.Cells.Value;
            //        string[] strArray = ConvertToStringArray(myvalues);
            //        int iCol = 0;
            //        XORCISMModel.CCE myCCE = new CCE();
            //        string sCCEID = "";

            //        foreach (string sValue in strArray)
            //        {
            //            iCol++;
            //            //CCE ID
            //            //CCE Description
            //            //CCE Parameters
            //            //CCE Technical Mechanisms
            //            //...
            //            switch (iCol.ToString())
            //            {
            //                case "1":   //CCE ID
            //                    if (sValue.StartsWith("CCE-"))
            //                    {
            //                        sCCEID = sValue;
            //                    }
            //                    Console.WriteLine(sValue);
            //                    break;
            //                case "2":   //CCE Description

            //                    break;
            //                default:
            //                    break;
            //            }


            //            //Console.WriteLine(sValue);
            //        }
            //        Console.WriteLine("---------------------------------");
            //    }

            ////}
            #endregion ReadExcel



            //FREE
            model.SaveChanges();
            model.Dispose();
            model = null;



            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
        }

        public static string[] ConvertToStringArray(System.Array values)
        {

            // create a new string array
            string[] theArray = new string[values.Length];

            // loop through the 2-D System.Array and populate the 1-D String Array
            for (int i = 1; i <= values.Length; i++)
            {
                if (values.GetValue(1, i) == null)
                    theArray[i - 1] = "";
                else
                    theArray[i - 1] = (string)values.GetValue(1, i).ToString();
            }

            return theArray;
        }
    }
}
