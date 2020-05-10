using System;
using System.IO;
using System.Xml;
//using System.Xml.Linq;

namespace CWEParser
{
    // XML Parser for MITRE Common Weaknesses Enumeration (CWE) v4
    // Jerome Athias, 2020
    // (parsing with switch/cases for detecting updates in schema)
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("DEBUG "+DateTimeOffset.Now);
            XmlDocument docXML= new XmlDocument();
            docXML.Load("cwec_v4.0.xml");   //HARDCODED
            //Console.WriteLine(docXML.DocumentElement.OuterXml);
            XmlNamespaceManager m = new XmlNamespaceManager(docXML.NameTable);
            m.AddNamespace("ns", "http://cwe.mitre.org/cwe-6"); //Hardcoded
            //Console.WriteLine(docXML.SelectSingleNode("ns:Weakness_Catalog/ns:Categories/ns:Category", m).InnerText);

            //Console.WriteLine("DEBUG "+DateTimeOffset.Now);
            Console.WriteLine("DEBUG CWE file loaded");

            XmlNodeList nodesCategory;
            nodesCategory = docXML.SelectNodes("ns:Weakness_Catalog/ns:Categories/ns:Category", m);
            //ImportFile_cwe(nodesCategory);

            XmlNodeList nodesWeakness;
            nodesWeakness = docXML.SelectNodes("ns:Weakness_Catalog/ns:Weaknesses/ns:Weakness", m);
            ImportFile_cwe(nodesWeakness);

            XmlNodeList nodesCompound;
            nodesCompound = docXML.SelectNodes("ns:Weakness_Catalog/ns:Compound_Elements/ns:Compound_Element", m);
            //ImportFile_cwe(nodesCompound);
            //TODO: Free...
            docXML = null;
        }

        static private void ImportFile_cwe(XmlNodeList nodes)
        {
            //Console.WriteLine("DEBUG "+DateTimeOffset.Now);
            Console.WriteLine("DEBUG ImportFile_cwe");
            foreach (XmlNode nodeCWE in nodes){
                Console.WriteLine("DEBUG ==================================================================================================================================");
                //Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                string sCWEID = "CWE-" + nodeCWE.Attributes["ID"].InnerText;
                Console.WriteLine("DEBUG " + sCWEID);
                string sCWEName = nodeCWE.Attributes["Name"].InnerText;
                Console.WriteLine("DEBUG sCWEName="+sCWEName);
                string sCWEStatus = nodeCWE.Attributes["Status"].InnerText;

                string sCWEAbstraction = string.Empty;
                //TODO: CWEURL
                try{
                    sCWEAbstraction = nodeCWE.Attributes["Weakness_Abstraction"].InnerText;
                    Console.WriteLine("DEBUG sCWEAbstraction="+sCWEAbstraction);
                }
                catch (Exception ex){
                    string sIgnoreWarning = ex.Message;
                    //Console.WriteLine("Exception: WeaknessAbstraction: " + ex.Message + " " + ex.InnerException);
                    //Object reference not set to an instance of an object. 
                }
                string sCWEDescription = nodeCWE.ChildNodes[0].ChildNodes[0].InnerText;
                //string sCWEDescriptionClean = CleaningCWEString(sCWEDescription);
                Console.WriteLine("DEBUG sCWEDescription="+sCWEDescription);
                string sCWECausalNature = string.Empty;  //TODO
                string sCWEURL = string.Empty;  //TODO
                
                foreach (XmlNode nodeCWEinfo in nodeCWE){
                    //Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                    //Console.WriteLine("DEBUG nodeCWEinfo: " + nodeCWEinfo.Name);
                    switch (nodeCWEinfo.Name)
                    {
                        case "Description":
                            //Done before
                            break;
                        case "Summary":
                            
                            break;
                        case "Extended_Description":
                            
                            break;
                        case "Related_Weaknesses":
                            foreach (XmlNode nodeCWERelationship in nodeCWEinfo){
                                string sCWERelationshipNature = "";
                                string sCWERelationshipTargetCWEID = "";
                                string sCWERelationshipViewID = "";
                                string sCWERelationshipOrdinal = "";
                                //Console.WriteLine("DEBUG nodeCWERelationship="+nodeCWERelationship.Name);   //Related_Weakness
                                try{
                                    Console.WriteLine("DEBUG nodeCWERelationshipNature="+nodeCWERelationship.Attributes["Nature"].InnerText);
                                    Console.WriteLine("DEBUG sCWERelationshipTargetCWEID="+nodeCWERelationship.Attributes["CWE_ID"].InnerText);
                                    Console.WriteLine("DEBUG sCWERelationshipViewID="+nodeCWERelationship.Attributes["View_ID"].InnerText);
                                    Console.WriteLine("DEBUG sCWERelationshipOrdinal="+nodeCWERelationship.Attributes["Ordinal"].InnerText);
                                }
                                catch(Exception exnodeCWERelationshipAttributes){
                                    //No Ordinal
                                }
                            }
                            break;
                        case "Relationships":
                            #region CWERelationships
                            try{
                                //TODO? Delete and recreate
                                foreach (XmlNode nodeCWERelationship in nodeCWEinfo){
                                    bool bCWERelationshipCategory = false;
                                    string sCWERelationshipNature = "";
                                    string sCWERelationshipTargetCWEID = "";
                                    foreach (XmlNode nodeCWERelationshipItem in nodeCWERelationship){
                                        if (nodeCWERelationshipItem.Name == "Relationship_Target_Form"){
                                            if (nodeCWERelationshipItem.InnerText == "Category"){
                                                bCWERelationshipCategory = true;
                                            }
                                        }
                                        else{
                                            if (nodeCWERelationshipItem.Name == "Relationship_Nature"){
                                                sCWERelationshipNature = nodeCWERelationshipItem.InnerText; //ChildOf
                                            }
                                            else{
                                                if (nodeCWERelationshipItem.Name == "Relationship_Target_ID"){
                                                    sCWERelationshipTargetCWEID = "CWE-" + nodeCWERelationshipItem.InnerText; //519
                                                    Console.WriteLine("DEBUG sCWERelationshipTargetCWEID="+sCWERelationshipTargetCWEID);
                                                }
                                            }
                                        }
                                    }
                                    if (bCWERelationshipCategory){
                                        
                                    }
                                }
                            }
                            catch (Exception exRelationships){
                                Console.WriteLine("Exception: exRelationships: " + exRelationships.Message + " " + exRelationships.InnerException);
                            }
                            #endregion CWERelationships
                            break;
                        case "Related_Attack_Patterns":
                            #region CWERelated_Attack_Patterns
                            try{
                                foreach (XmlNode nodeCWEAttackPattern in nodeCWEinfo){
                                    string sCAPECID = "";
                                    string sCAPECversion = "";
                                    try{
                                        sCAPECID = nodeCWEAttackPattern.Attributes["CAPEC_ID"].InnerText;  //2.1
                                        //Console.WriteLine("DEBUG sCAPECID="+sCAPECID);
                                    }
                                    catch (Exception exsCAPECID){
                                        string sIgnoreWarning = exsCAPECID.Message;
                                        //Console.WriteLine("ERROR: sCAPECID not found");
                                    }
                                    try{
                                        sCAPECversion = nodeCWEAttackPattern.Attributes["CAPEC_Version"].InnerText;  //2.1
                                        //Console.WriteLine("DEBUG sCAPECversion="+sCAPECversion);
                                    }
                                    catch (Exception exsCAPECversion){
                                        string sIgnoreWarning = exsCAPECversion.Message;
                                        //Console.WriteLine("ERROR: sCAPECversion not found");
                                    }
                                    foreach (XmlNode nodeCWECAPEC in nodeCWEAttackPattern){
                                        sCAPECID = "CAPEC-"+nodeCWECAPEC.InnerText;  //TODO: Check that it is a CAPEC
                                        Console.WriteLine("DEBUG sCAPECID="+sCAPECID);
                                        
                                    }
                                }
                            }
                            catch (Exception exCWEAttackPattern){
                                Console.WriteLine("Exception: exCWEAttackPattern: " + exCWEAttackPattern.Message + " " + exCWEAttackPattern.InnerException);
                            }
                            #endregion CWERelated_Attack_Patterns
                            break;
                        case "Taxonomy_Mappings":
                        #region CWETaxonomy_Mappings
                            try{
                                int iTaxonomyID = 0;
                                int iVocabularyID = 0;
                                foreach (XmlNode nodeCWETaxonomy in nodeCWEinfo){
                                    //Taxonomy_Mapping
                                    string sCWEMappedTaxonomyName = string.Empty;
                                    try{
                                        sCWEMappedTaxonomyName = CleaningCWEString(nodeCWETaxonomy.Attributes["Mapped_Taxonomy_Name"].InnerText);
                                        //Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                                        Console.WriteLine("DEBUG sCWEMappedTaxonomyName=" + sCWEMappedTaxonomyName);
                                        //OWASP Top Ten 2004
                                        //TODO? 2010    2013
                                    }
                                    catch (Exception exsCWEMappedTaxonomyName){
                                        try{
                                            sCWEMappedTaxonomyName = CleaningCWEString(nodeCWETaxonomy.Attributes["Taxonomy_Name"].InnerText);
                                            Console.WriteLine("DEBUG sCWEMappedTaxonomyName=" + sCWEMappedTaxonomyName);
                                        }
                                        catch(Exception exsCWEMappedTaxonomyName2){
                                            Console.WriteLine("Exception: exsCWEMappedTaxonomyName2: " + exsCWEMappedTaxonomyName2.Message + " " + exsCWEMappedTaxonomyName2.InnerException);
                                        }
                                    }

                                    switch(sCWEMappedTaxonomyName){
                                        case "WASC":    //2.0
                                            #region taxonomywasc
                                            //http://projects.webappsec.org/w/page/13246975/Threat%20Classification%20Taxonomy%20Cross%20Reference%20View
                                            foreach (XmlNode nodeCWEWASC in nodeCWETaxonomy){
                                                //<Mapped_Node_Name>Server Misconfiguration </Mapped_Node_Name>
                                                //<Mapped_Node_ID>14</Mapped_Node_ID>
                                                if (nodeCWEWASC.Name == "Mapped_Node_ID"){
                                                    string sWASCRefID = nodeCWEWASC.InnerText;
                                                    if (sWASCRefID.Length < 2){
                                                        sWASCRefID = "0" + sWASCRefID;
                                                    }
                                                    sWASCRefID = "WASC-" + sWASCRefID;
                                                    Console.WriteLine("DEBUG sWASCRefID=" + sWASCRefID);

                                                }
                                            }
                                            #endregion taxonomywasc
                                            break;
                                        case "OWASP Top Ten 2004":
                                            //Note: The generic Taxonomy mapping (see below) could be used
                                            #region taxonomyowasptop2004
                                            string sTaxonomyMappedNodeName = string.Empty;
                                            string sTaxonomyMappedNodeID = string.Empty;
                                            string sTaxonomyMappingFit = string.Empty;
                                            //OWASPTOP10 oOWASPTOP10 = new OWASPTOP10();
                                            int iOWASPTOP10ID = 0;
                                            foreach (XmlNode nodeCWEOWASPTOP2004 in nodeCWETaxonomy){
                                                switch (nodeCWEOWASPTOP2004.Name){
                                                    case "Mapped_Node_Name":
                                                    case "Entry_Name":
                                                        //Insecure Configuration Management
                                                        sTaxonomyMappedNodeName = CleaningCWEString(nodeCWEOWASPTOP2004.InnerText);
                                                        //Console.WriteLine("DEBUG " + sCWEID + " sTaxonomyMappedNodeName:" + sTaxonomyMappedNodeName);
                                                        break;
                                                    case "Mapped_Node_ID":
                                                    case "Entry_ID":
                                                        //A10
                                                        sTaxonomyMappedNodeID = CleaningCWEString(nodeCWEOWASPTOP2004.InnerText);
                                                        //Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                        Console.WriteLine("DEBUG " + sCWEID + " sTaxonomyMappedNodeID:" + sTaxonomyMappedNodeID);
                                                        
                                                        break;
                                                    case "Mapping_Fit":
                                                        sTaxonomyMappingFit = CleaningCWEString(nodeCWEOWASPTOP2004.InnerText);
                                                        break;
                                                    default:
                                                        //Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                        Console.WriteLine("ERROR: Missing code for CWETaxonomy_MappingNode: " + nodeCWEOWASPTOP2004.Name);
                                                        break;
                                                }
                                            }
                                            #endregion taxonomyowasptop2004
                                            break;
                                        //TODO
                                        /*
                                        case "PLOVER":

                                            break;
                                        */
                                        default:
                                            //7 Pernicious Kingdoms
                                            foreach (XmlNode nodeCWETaxonomyNode in nodeCWETaxonomy){
                                                //Entry_Name???
                                                string sTaxonomyNodeName=CleaningCWEString(nodeCWETaxonomyNode.InnerText);

                                                //Integer coercion error
                                                if (nodeCWETaxonomyNode.Name == "Entry_Name" || nodeCWETaxonomyNode.Name == "Mapped_Node_Name"){
                                                    Console.WriteLine("TODO TaxonomyEntryName");
                                                }
                                                else{
                                                    //TODO: switch
                                                    if (nodeCWETaxonomyNode.Name == "Entry_ID" || nodeCWETaxonomyNode.Name == "Mapped_Node_ID"){
                                                        //INT02-C
                                                        //Update TAXONOMYNODE
                                                        //oTaxonomyNode.TaxonomyMappedNodeID = nodeCWETaxonomyNode.InnerText;
                                                        
                                                    }
                                                    else{
                                                        if (nodeCWETaxonomyNode.Name == "Mapping_Fit"){
                                                            //Update CWETAXONOMYNODE
                                                            //oCWETaxonomyNode.Mapping_Fit = nodeCWETaxonomyNode.InnerText.Trim();
                                                            
                                                        }
                                                        else{
                                                            //Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                            Console.WriteLine("ERROR: Missing code for nodeCWETaxonomyNode " + nodeCWETaxonomyNode.Name);
                                                        }
                                                    }
                                                }
                                            }
                                            break;
                                    }
                                    /*
                                    else{
                                        Console.WriteLine("ERROR: Missing code for CWETaxonomy_Mapping: " + sCWEMappedTaxonomyName);
                                    }
                                    */
                                }
                            }
                            catch (Exception exCWETaxonomy_Mapping){
                                //Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                Console.WriteLine("Exception: exCWETaxonomy_Mapping: " + exCWETaxonomy_Mapping.Message + " " + exCWETaxonomy_Mapping.InnerException);
                            }
                            break;
                        #endregion CWETaxonomy_Mappings
                        case "Applicable_Platforms":
                        #region CWEApplicable_Platforms
                            try{
                                foreach (XmlNode nodeCWEPlatform in nodeCWEinfo){
                                    //<Languages>
                                    switch(nodeCWEPlatform.Name){
                                        case "Language":
                                            Console.WriteLine("TODO Language");
                                            break;
                                        case "Languages":
                                            #region languages
                                            foreach (XmlNode nodeCWEPlatformLanguage in nodeCWEPlatform){
                                                string sPrevalence = string.Empty;
                                                //<Language_Class Language_Class_Description="All"/>
                                                //<Language Prevalence="Often" Language_Name="C"/>
                                                //<Language Prevalence="Often" Language_Name="C++"/>
                                                //<Language Language_Name="Assembly"/>
                                                //<Language_Class Language_Class_Description="Languages without memory management support"/>
                                                switch (nodeCWEPlatformLanguage.Name){
                                                    case "Language":
                                                        try{
                                                            sPrevalence = nodeCWEPlatformLanguage.Attributes["Prevalence"].InnerText;
                                                        }
                                                        catch (Exception exNoPrevalence){
                                                            string sIgnoreWarning = exNoPrevalence.Message;
                                                        }
                                                        string sLanguage = nodeCWEPlatformLanguage.Attributes["Language_Name"].InnerText;
                                                        
                                                        break;
                                                    case "Language_Class":
                                                        string sLanguageClassDescription = nodeCWEPlatformLanguage.Attributes["Language_Class_Description"].InnerText;
                                                        
                                                        break;
                                                    default:
                                                        Console.WriteLine("ERROR: Missing code for nodeCWEPlatformLanguage " + nodeCWEPlatformLanguage.Name);
                                                        break;
                                                }
                                            }
                                            break;
                                            #endregion languages
                                        case "Technology_Classes":
                                        case "Technology":
                                            #region technologyclasses
                                            foreach (XmlNode nodeCWETechnologyClass in nodeCWEPlatform){
                                                string sTechnology = "";
                                                try{
                                                    sTechnology = nodeCWETechnologyClass.Attributes["Technology_Name"].InnerText;
                                                }
                                                catch(Exception exTechnology){
                                                    try{
                                                        sTechnology = nodeCWETechnologyClass.Attributes["Class"].InnerText;
                                                        //TODO  Prevalence="Undetermined"
                                                    }
                                                    catch(Exception exTechnology2){
                                                        Console.WriteLine("Exception exTechnology2 "+exTechnology2.Message);
                                                    }
                                                }

                                            }
                                            break;
                                            #endregion technologyclasses
                                        case "Platform_Notes":
                                            //CWEObject.Platform_Notes = CleaningCWEString(nodeCWEPlatform.InnerText);
                                            //model.SaveChanges();
                                            break;
                                        //Architectural_Paradigms
                                        //TODO
                                        //CWEARCHITECTURALPARADIGM
                                        case "Architectural_Paradigms":
                                            #region architecturalparadigm
                                            foreach (XmlNode nodeCWEArchitecturalParadigm in nodeCWEPlatform){
                                                if (nodeCWEArchitecturalParadigm.Name != "Architectural_Paradigm"){
                                                    Console.WriteLine("ERROR: Missing code for nodeCWEArchitecturalParadigm " + nodeCWEArchitecturalParadigm.Name);
                                                }
                                                else{
                                                    try{
                                                        string sArchitecturalParadigm=nodeCWEArchitecturalParadigm.Attributes["Architectural_Paradigm_Name"].InnerText;
                                                        
                                                    }
                                                    catch(Exception exnodeCWEArchitecturalParadigm){
                                                        Console.WriteLine("Exception: exnodeCWEArchitecturalParadigm " + exnodeCWEArchitecturalParadigm.Message + " " + exnodeCWEArchitecturalParadigm.InnerException);
                                                    }
                                                }
                                            }
                                            break;
                                            #endregion architecturalparadigm
                                        case "Operating_Systems":
                                            #region operatingsystems
                                            foreach (XmlNode nodeCWEOS in nodeCWEPlatform){
                                                switch(nodeCWEOS.Name){
                                                    case "Operating_System":
                                                        string sOperating_System_Name = nodeCWEOS.Attributes["Operating_System_Name"].InnerText;
                                                        //Windows 2000
                                                        //Windows XP
                                                        //Windows Vista
                                                        //Mac OS X
                                                        //TODO REVIEW
                                                        string sOSShortName=sOperating_System_Name.Replace("Windows ","");
                                                        sOSShortName=sOSShortName.Replace("Mac OS X", "OSX");
                                                        string sPrevalence = "";
                                                        try{
                                                            sPrevalence = nodeCWEOS.Attributes["Prevalence"].InnerText;
                                                        }
                                                        catch(Exception exOSPrevalence){
                                                            Console.WriteLine("ERROR: no Prevalence for OS " + sOperating_System_Name+" "+sCWEID);
                                                        }

                                                        break;
                                                    case "Operating_System_Class":
                                                        string sOperating_System_Class_Description = nodeCWEOS.Attributes["Operating_System_Class_Description"].InnerText;
                                                        
                                                        break;
                                                    default:
                                                        Console.WriteLine("ERROR: Missing code for nodeCWEOS " + nodeCWEOS.Name);
                                                        break;
                                                }
                                            }
                                            break;
                                            #endregion operatingsystems
                                        default:
                                            Console.WriteLine("ERROR: Missing code for nodeCWEPlatform " + nodeCWEPlatform.Name);
                                            //Operating_Systems
                                            //Platform_Notes
                                            break;
                                    }
                                }
                            }
                            catch (Exception exCWEPlatform){
                                Console.WriteLine("Exception: exCWEPlatform: " + exCWEPlatform.Message + " " + exCWEPlatform.InnerException);
                            }
                            break;
                        #endregion CWEApplicable_Platforms
                        case "Alternate_Terms":
                        #region CWEAlternate_Terms
                            foreach (XmlNode nodeCWEAlternateTerm in nodeCWEinfo){
                                //Console.WriteLine("DEBUG nodeCWEAlternateTerm");
                                foreach (XmlNode nodeTerm in nodeCWEAlternateTerm){
                                    switch (nodeTerm.Name){
                                        case "Term":
                                            string sAlternateTerm = CleaningCWEString(nodeTerm.InnerText); //API Abuse
                                            //Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                                            Console.WriteLine("DEBUG sAlternateTerm=" + sAlternateTerm);
                                            
                                            break;
                                        case "Description":
                                        case "Alternate_Term_Description":
                                            string sAlternateTermDefinition = CleaningCWEString(nodeTerm.InnerText);
                                            //Cleaning
                                            sAlternateTermDefinition = sAlternateTermDefinition.Replace("<Text>", "");
                                            sAlternateTermDefinition = sAlternateTermDefinition.Replace("</Text>", "");
                                            Console.WriteLine("DEBUG sAlternateTermDefinition="+sAlternateTermDefinition);
                                            //Update CWEALTERNATETERM
                                            //oCWEAlternateTerm.AlternateTermDescription = sAlternateTermDefinition;
                                            //model.SaveChanges();    //TEST PERFORMANCE
                                            break;
                                        default:
                                            Console.WriteLine("ERROR: Missing Code for nodeCWEAlternateTerm "+nodeTerm.Name);
                                            break;
                                    }
                                }
                            }
                            break;
                        #endregion CWEAlternate_Terms
                        case "Terminology_Notes":
                            string sTerminologyNotes = nodeCWEinfo.InnerText;
                            sTerminologyNotes = sTerminologyNotes.Replace("<Terminology_Note>", "");
                            sTerminologyNotes = sTerminologyNotes.Replace("</Terminology_Note>", "");
                            sTerminologyNotes = sTerminologyNotes.Replace("<Text>", "");
                            sTerminologyNotes = sTerminologyNotes.Replace("</Text>", "");
                            sTerminologyNotes = CleaningCWEString(sTerminologyNotes);
                            
                            break;
                        case "Time_of_Introduction":
                            #region CWETime_Introduction
                            foreach (XmlNode nodeCWEIntroductionPhase in nodeCWEinfo){
                                if (nodeCWEIntroductionPhase.Name == "Introductory_Phase"){
                                    //Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                                    Console.WriteLine("DEBUG nodeCWEIntroductionPhase");
                                    string sIntroductionPhase = nodeCWEIntroductionPhase.InnerText.Trim(); //Architecture and Design
                                    Console.WriteLine("DEBUG sIntroductionPhase=" + sIntroductionPhase);
                                    
                                }
                                else{
                                    Console.WriteLine("ERROR: Missing code for nodeCWEIntroductionPhase " + nodeCWEIntroductionPhase.Name);
                                }
                            }
                            #endregion CWETime_Introduction
                            break;
                        case "Likelihood_of_Exploit":
                        case "Likelihood_Of_Exploit":
                            #region CWELikelihood_of_Exploit
                            try{
                                string sLikelihood = nodeCWEinfo.InnerText; //High to Very High
                                Console.WriteLine("DEBUG sLikelihood=" + sLikelihood);
                                
                            }
                            catch (Exception exCWELikelihood_of_Exploit){
                                Console.WriteLine("Exception: exCWELikelihood_of_Exploit " + exCWELikelihood_of_Exploit.Message + " " + exCWELikelihood_of_Exploit.InnerException);
                            }
                            #endregion CWELikelihood_of_Exploit
                            break;
                        case "Common_Consequences":
                            #region CWECommon_Consequences
                            int iRankConsequence = 0;
                            foreach (XmlNode nodeCommonConsequence in nodeCWEinfo){
                                iRankConsequence++;
                                Console.WriteLine("DEBUG nodeCommonConsequence "+iRankConsequence);
                                
                                //************************************************************************
                                foreach (XmlNode nodeCommonConsequenceInfo in nodeCommonConsequence){
                                    switch (nodeCommonConsequenceInfo.Name){
                                        case "Consequence_Scope":
                                        case "Scope":
                                            string sConsequenceScope = CleaningCWEString(nodeCommonConsequenceInfo.InnerText);
                                            
                                            break;
                                        case "Consequence_Technical_Impact":
                                        case "Impact":
                                            string sTechnicalImpact = CleaningCWEString(nodeCommonConsequenceInfo.InnerText);
                                            
                                            break;
                                        case "Consequence_Note":
                                        case "Note":
                                            string sAttackConsequenceNoteText = string.Empty;
                                            sAttackConsequenceNoteText=nodeCommonConsequenceInfo.InnerText;
                                            sAttackConsequenceNoteText = sAttackConsequenceNoteText.Replace("<Text>","");
                                            sAttackConsequenceNoteText = sAttackConsequenceNoteText.Replace("</Text>", "");
                                            sAttackConsequenceNoteText = CleaningCWEString(sAttackConsequenceNoteText);
                                            
                                            break;
                                        default:
                                            Console.WriteLine("ERROR: Missing code for nodeCommonConsequenceInfo: " + nodeCommonConsequenceInfo.Name);
                                            break;
                                    }
                                }
                            }
                            #endregion CWECommon_Consequences
                            break;
                        case "Detection_Methods":
                            #region detectionmethod
                            foreach (XmlNode nodeCWEDetectionMethod in nodeCWEinfo){
                                if (nodeCWEDetectionMethod.Name != "Detection_Method"){
                                    Console.WriteLine("ERROR: Missing code for " + nodeCWEDetectionMethod.Name);
                                }
                                else{
                                    string sDetectionMethodID = string.Empty;
                                    try{
                                        sDetectionMethodID = nodeCWEDetectionMethod.Attributes["Detection_Method_ID"].InnerText; //DM-7
                                    }
                                    catch (Exception ex){
                                        string sIgnoreWarning = ex.Message;
                                    }
                                    
                                    foreach (XmlNode nodeCWEDetectionMethodInfo in nodeCWEDetectionMethod){
                                        switch (nodeCWEDetectionMethodInfo.Name){
                                            case "Method_Name":
                                            case "Method":
                                                string sDetectionMethodName = CleaningCWEString(nodeCWEDetectionMethodInfo.InnerText);  //Manual Static Analysis
                                                
                                                break;
                                            case "Method_Description":
                                            case "Description":
                                                string sDetectionMethodDescription = CleaningCWEString(nodeCWEDetectionMethodInfo.InnerText);
                                                sDetectionMethodDescription = sDetectionMethodDescription.Replace("<text>","");
                                                sDetectionMethodDescription = sDetectionMethodDescription.Replace("</text>", "");
                                                sDetectionMethodDescription = sDetectionMethodDescription.Replace("<Text>","");
                                                sDetectionMethodDescription = sDetectionMethodDescription.Replace("</Text>", "");
                                                
                                                break;
                                            case "Method_Effectiveness":
                                            case "Effectiveness":
                                                //oCWEDetectionMethod.CWEDetectionMethodEffectiveness = CleaningCWEString(nodeCWEDetectionMethodInfo.InnerText);  //Moderate
                                                
                                                break;
                                            case "Method_Effectiveness_Notes":
                                                string sMethodEffectivenessNotes = CleaningCWEString(nodeCWEDetectionMethodInfo.InnerText);
                                                sMethodEffectivenessNotes = sMethodEffectivenessNotes.Replace("<text>", "");
                                                sMethodEffectivenessNotes = sMethodEffectivenessNotes.Replace("</text>", "");
                                                sMethodEffectivenessNotes = sMethodEffectivenessNotes.Replace("<Text>", "");
                                                sMethodEffectivenessNotes = sMethodEffectivenessNotes.Replace("</Text>", "");
                                                
                                                break;
                                            default:
                                                Console.WriteLine("ERROR: Missing code for nodeCWEDetectionMethodInfo " + nodeCWEDetectionMethodInfo.Name);
                                                break;
                                        }
                                    }
                                }
                            }
                            break;
                            #endregion detectionmethod
                        case "Potential_Mitigations":
                            #region CWEMitigation
                            int iCWEMitigationCount = 0;
                            //MITIGATIONSTRATEGYFORMITIGATION
                            foreach (XmlNode nodeCWEMitigation in nodeCWEinfo){
                                if (nodeCWEMitigation.Name != "Mitigation"){
                                    Console.WriteLine("ERROR: Missing code for Mitigation " + nodeCWEMitigation.Name);

                                }
                                else{
                                    iCWEMitigationCount++;
                                    //MITIGATIONFORCWE oCWEMitigation;
                                    string sCWEMitigationPhaseName = string.Empty;
                                    string sCWEMitigationStrategyName = string.Empty;
                                    string sMitigationID = "";
                                    string sEffectiveness = "";
                                    string sMitigationEffectivenessNotes=string.Empty;
                                    try{
                                        sMitigationID = nodeCWEMitigation.Attributes["Mitigation_ID"].InnerText;
                                    }
                                    catch (Exception exsMitigationID){
                                        Console.WriteLine("DEBUG Note: no Mitigation_ID for " + sCWEID);
                                        sMitigationID = "JA-"+iCWEMitigationCount.ToString();   //TODO REVIEW (needs a CWE update)
                                    }
                                    
                                    foreach (XmlNode nodeCWEMitigationInfo in nodeCWEMitigation){
                                        //Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                                        Console.WriteLine("DEBUG nodeCWEMitigationInfo " + nodeCWEMitigationInfo.Name);
                                        if (nodeCWEMitigationInfo.Name != "Mitigation_Description" && nodeCWEMitigationInfo.Name != "Description"){
                                            switch(nodeCWEMitigationInfo.Name){
                                                case "Mitigation_Phase":
                                                case "Phase":
                                                    #region mitigationphase
                                                    sCWEMitigationPhaseName=CleaningCWEString(nodeCWEMitigationInfo.InnerText);
                                                    //Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                    Console.WriteLine("DEBUG sCWEMitigationPhaseName=" + sCWEMitigationPhaseName);
                                                    int iPhaseID = 0;
                                                    
                                                    #endregion mitigationphase
                                                    break;
                                                case "Mitigation_Strategy":
                                                case "Strategy":
                                                    #region mitigationstrategy
                                                    sCWEMitigationStrategyName = CleaningCWEString(nodeCWEMitigationInfo.InnerText);
                                                    //Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                    Console.WriteLine("DEBUG sCWEMitigationStrategyName=" + sCWEMitigationStrategyName);
                                                    
                                                    #endregion mitigationstrategy
                                                    break;
                                                case "Mitigation_Effectiveness":
                                                case "Effectiveness":
                                                    #region mitigationeffectiveness
                                                    //MITIGATIONEFFECTIVENESS
                                                    sEffectiveness=CleaningCWEString(nodeCWEMitigationInfo.InnerText);
                                                    //Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                    Console.WriteLine("DEBUG sEffectiveness=" + sEffectiveness);
                                                    
                                                    #endregion mitigationeffectiveness
                                                    break;
                                                case "Mitigation_Effectiveness_Notes":
                                                case "Effectiveness_Notes":
                                                    sMitigationEffectivenessNotes = CleaningCWEString(nodeCWEMitigationInfo.InnerText);
                                                    sMitigationEffectivenessNotes=sMitigationEffectivenessNotes.Replace("<Text>","");
                                                    sMitigationEffectivenessNotes=sMitigationEffectivenessNotes.Replace("</Text>","");
                                                    
                                                    break;
                                                default:
                                                    Console.WriteLine("ERROR: Missing code for nodeCWEMitigationInfo " + nodeCWEMitigationInfo.Name);
                                                    break;
                                            }
                                        }
                                        else{
                                            //Mitigation_Description
                                            #region mitigationdescription
                                            foreach (XmlNode nodeCWEMitigationDescriptionText in nodeCWEMitigationInfo){
                                                //TODO Review: Problem if many
                                                //Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                                                Console.WriteLine("DEBUG nodeCWEMitigationDescriptionText");
                                                if (nodeCWEMitigationDescriptionText.Name != "xhtml:p" && nodeCWEMitigationDescriptionText.Name != "#text" && nodeCWEMitigationDescriptionText.Name != "Text"){
                                                    //TODO
                                                    Console.WriteLine("ERROR: Missing code for nodeCWEMitigationDescriptionText " + nodeCWEMitigationDescriptionText.Name);
                                                    //<xhtml:p>
                                                    //Block
                                                    //<Block Block_Nature="Good_Code">
                                                    //<Block Block_Nature="Bad_Code">
                                                    //<Block Block_Nature="Numeric_List">
                                                    //MITIGATIONCODE
                                                }
                                                else{
                                                    string sCWEMitigationDescription = nodeCWEMitigationDescriptionText.InnerText;
                                                    string sCWEMitigationDescriptionClean = CleaningCWEString(sCWEMitigationDescription);

                                                }
                                            }
                                            #endregion mitigationdescription
                                        }
                                    }
                                }
                            }
                            break;
                        #endregion CWEMitigation
                        case "Weakness_Ordinalities":
                            #region CWEOrdinality
                            //CWEORDINALITY
                            foreach (XmlNode nodeCWEWeaknessOrdinality in nodeCWEinfo){
                                
                                if (nodeCWEWeaknessOrdinality.Name != "Weakness_Ordinality"){
                                    Console.WriteLine("ERROR: Missing code for nodeCWEWeaknessOrdinality " + nodeCWEWeaknessOrdinality.Name);
                                }
                                else{
                                    foreach (XmlNode nodeOrdinality in nodeCWEWeaknessOrdinality){
                                        if (nodeOrdinality.Name != "Ordinality"){
                                            if (nodeOrdinality.Name == "Ordinality_Description"){
                                                /*
                                                if (oCWEOrdinality != null){
                                                    oCWEOrdinality.Ordinality_Description = CleaningCWEString(nodeOrdinality.InnerText);
                                                }
                                                else{
                                                    Console.WriteLine("ERROR: for Ordinality_Description "+sCWEID);
                                                }
                                                */
                                            }
                                            else{
                                                Console.WriteLine("ERROR: Missing code for nodeOrdinality " + nodeOrdinality.Name);
                                            }
                                        }
                                        else{
                                            string sWeaknessOrdinality=CleaningCWEString(nodeOrdinality.InnerText);    //Primary
                                            
                                        }
                                    }
                                }
                            }
                            break;
                            #endregion CWEOrdinality
                        case "Background_Details":
                            string sCWEBackgroundDetails = nodeCWEinfo.InnerText;
                            
                            break;
                        case "Causal_Nature":
                            //CWEObject.CWECausalNature = CleaningCWEString(nodeCWEinfo.InnerText);  //Explicit
                            
                            break;
                        case "Demonstrative_Examples":
                            int iCptExample = 0;
                            #region CWEDemonstrativeExample
                            foreach (XmlNode nodeCWEDemonstrativeExample in nodeCWEinfo){
                                switch(nodeCWEDemonstrativeExample.Name)
                                {
                                    case "Demonstrative_Example":
                                        string sDemonstrativeExampleGUID = "";
                                        iCptExample++;
                                        try{
                                            sDemonstrativeExampleGUID = nodeCWEDemonstrativeExample.Attributes["Demonstrative_Example_ID"].InnerText;
                                        }
                                        catch (Exception exsDemonstrativeExampleGUID){
                                            string sIgnoreWarning = exsDemonstrativeExampleGUID.Message;
                                            //Console.WriteLine("Exception: exsDemonstrativeExampleGUID " + exsDemonstrativeExampleGUID.Message + " " + exsDemonstrativeExampleGUID.InnerException);
                                            sDemonstrativeExampleGUID = sCWEID + "-"+iCptExample; //TODO: Workaround
                                        }
                                        
                                        string sDemonstrativeExampleIntroText = string.Empty;
                                        string sDemonstrativeExampleBody = string.Empty;    //TODO: Review this/details in it
                                        foreach (XmlNode nodeCWEDemonstrativeExampleInfo in nodeCWEDemonstrativeExample){
                                            switch (nodeCWEDemonstrativeExampleInfo.Name)
                                            {
                                                case "Intro_Text":
                                                    sDemonstrativeExampleIntroText = CleaningCWEString(nodeCWEDemonstrativeExampleInfo.InnerText);
                                                    break;
                                                case "Example_Code":
                                                    Console.WriteLine("TODO Example_Code");
                                                    //DEMONSTRATIVEEXAMPLECODE
                                                    break;
                                                case "Example_Body":
                                                case "Body_Text":
                                                    sDemonstrativeExampleBody = CleaningCWEString(nodeCWEDemonstrativeExampleInfo.InnerText);
                                                    //TODO: Cleaning Review
                                                    //TODO: Language in it  <Code_Example_Language>C</Code_Example_Language>
                                                    //TODO: Code in it <Block Block_Nature="Bad_Code">  
                                                    //DEMONSTRATIVEEXAMPLECODE
                                                    break;
                                                case "Demonstrative_Example_References":
                                                    //foreach Reference
                                                    foreach (XmlNode nodeDemoExReference in nodeCWEDemonstrativeExampleInfo){
                                                        //List<int> ListAuthors=new List<int>();
                                                        string sReference_Title = "";
                                                        string sReference_Date = "";
                                                        string sReference_Link = "";    //URL
                                                        string sReference_Edition = "";
                                                        string sReference_Publisher = "";
                                                        string sReference_Publication = "";
                                                        string sReference_PubDate = "";
                                                        string sReference_Section = "";
                                                        //Others?
                                                        foreach (XmlNode nodeDemoExReferenceInfo in nodeDemoExReference){
                                                            switch(nodeDemoExReferenceInfo.Name)
                                                            {
                                                                case "Reference_Author":
                                                                    string sRefAuthor = CleaningCWEString(nodeDemoExReferenceInfo.InnerText);

                                                                    break;
                                                                case "Reference_Title":
                                                                    sReference_Title = CleaningCWEString(nodeDemoExReferenceInfo.InnerText);
                                                                    break;
                                                                case "Reference_Date":
                                                                    sReference_Date = CleaningCWEString(nodeDemoExReferenceInfo.InnerText);
                                                                    break;
                                                                case "Reference_Link":
                                                                    sReference_Link = CleaningCWEString(nodeDemoExReferenceInfo.InnerText);
                                                                    //TODO REVIEW
                                                                    //TODO Normalize
                                                                    sReference_Link = sReference_Link.Replace("http://www.", "http://");
                                                                    sReference_Link = sReference_Link.Replace("https://www.", "https://");
                                                                    break;
                                                                case "Reference_PubDate":
                                                                    sReference_PubDate = CleaningCWEString(nodeDemoExReferenceInfo.InnerText);
                                                                    break;
                                                                case "Reference_Publisher":
                                                                    sReference_Publisher = CleaningCWEString(nodeDemoExReferenceInfo.InnerText);
                                                                    break;
                                                                case "Reference_Publication":
                                                                    sReference_Publication = CleaningCWEString(nodeDemoExReferenceInfo.InnerText);
                                                                    break;
                                                                default:
                                                                    Console.WriteLine("ERROR: Missing code for nodeDemoExReferenceInfo " + nodeDemoExReferenceInfo.Name);
                                                                    break;
                                                            }
                                                        }
                                                    }
                                                    break;
                                                default:
                                                    Console.WriteLine("ERROR: Missing code for nodeCWEDemonstrativeExampleInfo " + nodeCWEDemonstrativeExampleInfo.Name);
                                                    break;
                                            }
                                        }
                                        try{
                                            /*
                                            //UPDATE DEMONSTRATIVEEXAMPLE
                                            oDemonstrativeExample.DemonstrativeExampleIntroText = sDemonstrativeExampleIntroText;
                                            oDemonstrativeExample.DemonstrativeExampleBody = sDemonstrativeExampleBody;
                                            oDemonstrativeExample.timestamp = DateTimeOffset.Now;
                                            model.SaveChanges();
                                            */
                                        }
                                        catch (Exception exDemonstrativeExample){
                                            Console.WriteLine("Exception: exDemonstrativeExample " + exDemonstrativeExample.Message + " " + exDemonstrativeExample.InnerException);
                                        }
                                        
                                        break;
                                    case "":
                                        break;
                                    default:
                                        Console.WriteLine("ERROR: Missing code for Demonstrative_Examples " + nodeCWEDemonstrativeExample.Name);
                                        break;
                                }
                            }
                            break;
                            #endregion CWEDemonstrativeExample
                        case "Observed_Examples":
                            foreach (XmlNode nodeCWEObservedExample in nodeCWEinfo){
                                foreach (XmlNode nodeCWEObservedExampleInfo in nodeCWEinfo){
                                    if (nodeCWEObservedExampleInfo.Name == "Observed_Example_Reference"){
                                        //CVE-2005-2146
                                        string sExampleRef = nodeCWEObservedExampleInfo.InnerText;
                                        if(sExampleRef.StartsWith("CVE-")){
                                            
                                        }
                                        else{
                                            //Not a CVE-
                                            Console.WriteLine("ERROR: Missing code for Observed_Example_Reference " + sExampleRef);
                                        }
                                    }
                                    //Observed_Example_Description
                                }
                            }
                            break;
                        case "Theoretical_Notes":
                            #region CWETheoreticalNotes
                            foreach (XmlNode nodeCWETheoreticalNote in nodeCWEinfo){
                                string sCWETheoreticalNote = CleaningCWEString(nodeCWETheoreticalNote.InnerText);

                            }
                            break;
                            #endregion CWETheoreticalNotes
                        case "Functional_Areas":
                            foreach (XmlNode nodeFunctionalArea in nodeCWEinfo){
                                if (nodeFunctionalArea.Name != "Functional_Area"){
                                    Console.WriteLine("ERROR: Missing code for Functional_Areas " + nodeFunctionalArea.Name);

                                }
                                else{
                                    string sFunctionalAreaName=CleaningCWEString(nodeFunctionalArea.InnerText).Trim();
                                    
                                }
                            }
                            break;
                        case "Affected_Resources":
                            #region CWEAffectedResource
                            foreach (XmlNode nodeCWEAffectedResource in nodeCWEinfo){
                                Console.WriteLine("DEBUG nodeCWEAffectedResource");
                                string sAffectedResource =CleaningCWEString(nodeCWEAffectedResource.InnerText); //Memory
                                Console.WriteLine("DEBUG sAffectedResource=" + sAffectedResource);

                            }
                            break;
                            #endregion CWEAffectedResource
                        case "References":
                            #region CWEReferences
                            foreach (XmlNode nodeCWEReference in nodeCWEinfo){
                                //Console.WriteLine("DEBUG nodeCWEReference");
                                string sCWEReferenceSourceID = "";
                                try{
                                    sCWEReferenceSourceID = nodeCWEReference.Attributes["Reference_ID"].InnerText;   //REF-17
                                }
                                catch (Exception ex){
                                    //Could be null
                                    string sIgnoreWarning = ex.Message;
                                }

                                if(sCWEReferenceSourceID!=""){
                                    string sReferenceSection = string.Empty;
                                    string sReferenceEdition = "";
                                    string sReferencePublisher = "";
                                    string sReferencePublication = "";
                                    string sReferencePubDate = "";
                                    string sReferenceDate = "";
                                    bool bReferenceExists=false;
                                    
                                    foreach (XmlNode nodeCWEReferenceInfo in nodeCWEReference){
                                        switch (nodeCWEReferenceInfo.Name)
                                        {
                                            case "Reference_Author":
                                                string sAuthor = CleaningCWEString(nodeCWEReferenceInfo.InnerText);
                                                
                                                break;
                                            case "Reference_Title":
                                                string sReferenceTitle = CleaningCWEString(nodeCWEReferenceInfo.InnerText);
                                                
                                                break;
                                            case "Reference_Link":
                                                string sURLReferenceLink=nodeCWEReferenceInfo.InnerText;
                                                //TODO NORMALIZE
                                                sURLReferenceLink = sURLReferenceLink.Replace("http://www.", "http://");
                                                sURLReferenceLink = sURLReferenceLink.Replace("https://www.", "https://");
                                                sURLReferenceLink = sURLReferenceLink.Replace("http://osvdb.org/displayvuln.php?osvdbid=", "http://osvdb.org/");
                                                sURLReferenceLink = sURLReferenceLink.Replace("http://osvdb.org/show/osvdb/", "http://osvdb.org/");
                                                sURLReferenceLink = sURLReferenceLink.Replace("securitytracker.com/id?", "securitytracker.com/id/");

                                                break;
                                            case "Reference_Section":
                                                sReferenceSection = CleaningCWEString(nodeCWEReferenceInfo.InnerText);
                                                //Console.WriteLine("DEBUG sReferenceSection=" + sReferenceSection);
                                                break;
                                            case "Reference_Publication":
                                                sReferencePublication = CleaningCWEString(nodeCWEReferenceInfo.InnerText);
                                                break;
                                            case "Reference_Publisher":
                                                sReferencePublisher = CleaningCWEString(nodeCWEReferenceInfo.InnerText);
                                                break;
                                            case "Reference_Edition":
                                                sReferenceEdition = CleaningCWEString(nodeCWEReferenceInfo.InnerText);
                                                break;
                                            case "Reference_PubDate":
                                                sReferencePubDate = CleaningCWEString(nodeCWEReferenceInfo.InnerText);
                                                break;
                                            
                                            case "Reference_Date":
                                                sReferenceDate=nodeCWEReferenceInfo.InnerText;
                                                break;
                                            
                                            default:
                                                Console.WriteLine("ERROR: Missing Code for nodeCWEReferenceInfo " + nodeCWEReferenceInfo.Name);
                                                break;
                                        }
                                    }
                                }
                            }
                            break;
                            #endregion CWEReferences
                        case "White_Box_Definitions":
                            foreach (XmlNode nodeWhiteBoxDefinition in nodeCWEinfo){
                                if (nodeWhiteBoxDefinition.Name != "White_Box_Definition"){
                                    Console.WriteLine("ERROR: Missing code for White_Box_Definitions " + nodeWhiteBoxDefinition.Name);
                                }
                                else{
                                    //TODO: Cleaning? <Text>    <Block>
                                    //Update CWE
                                //    CWEObject.White_Box_Definitions = nodeWhiteBoxDefinition.InnerText;
                                    //TODO: Multiple?
                                    //model.SaveChanges();
                                }
                            }
                            break;
                        case "Other_Notes":
                        case "Notes":
                            foreach (XmlNode nodeNote in nodeCWEinfo){
                                if (nodeNote.Name != "Note"){
                                    Console.WriteLine("ERROR: Missing code for Other_Notes " + nodeNote.Name);
                                }
                                else{
                                    string sNote = nodeNote.InnerText;
                                    //Cleaning
                                    sNote = sNote.Replace("<Text>", "");
                                    sNote = sNote.Replace("</Text>", "");
                                    sNote = sNote.Replace("<text>", "");
                                    sNote = sNote.Replace("</text>", "");
                                    sNote = CleaningCWEString(sNote);

                                    //Update CWE
                                    //CWEObject.Other_Notes = sNote;
                                    //model.SaveChanges();
                                }
                            }
                            break;
                        case "Research_Gaps":
                            foreach (XmlNode nodeResearchGap in nodeCWEinfo){
                                if (nodeResearchGap.Name != "Research_Gap"){
                                    Console.WriteLine("ERROR: Missing code for Research_Gaps " + nodeResearchGap.Name);
                                }
                                else{
                                    string sResearchGap = nodeResearchGap.InnerText;
                                    //Cleaning
                                    sResearchGap = sResearchGap.Replace("<Text>", "");
                                    sResearchGap = sResearchGap.Replace("</Text>", "");
                                    sResearchGap = CleaningCWEString(sResearchGap);

                                    //Update CWE
                                    //CWEObject.Research_Gaps = sResearchGap;
                                    //model.SaveChanges();
                                    //TODO: Multiple?
                                }
                            }
                            break;
                        case "Modes_of_Introduction":
                        case "Modes_Of_Introduction":
                            #region modesofintroduction
                            foreach (XmlNode nodeModeIntroduction in nodeCWEinfo){
                                if (nodeModeIntroduction.Name != "Introduction" && nodeModeIntroduction.Name != "Mode_of_Introduction"){
                                    Console.WriteLine("ERROR: Missing code for nodeModeIntroduction " + nodeModeIntroduction.Name);
                                    //Introduction
                                        //Phase
                                }
                                else{
                                    string sModeIntroduction=CleaningCWEString(nodeModeIntroduction.InnerText); //TODO Review

                                }
                            }
                            #endregion modesofintroduction
                            break;
                        case "Relevant_Properties":
                            #region relevantproperties
                            foreach (XmlNode nodeRelevantProperty in nodeCWEinfo){
                                //TODO? one table for this
                                if (nodeRelevantProperty.Name != "Relevant_Property"){
                                    Console.WriteLine("ERROR: Missing code for nodeRelevantProperty " + nodeRelevantProperty.Name);
                                }
                                else{
                                    //Cleaning?
                                    string sRelevantProperty=nodeRelevantProperty.InnerText;    //Equivalence   Uniqueness
                                    
                                }
                            }
                            #endregion relevantproperties
                            break;
                        case "Enabling_Factors_for_Exploitation":
                            #region factorexploitation
                            foreach (XmlNode nodeExploitationFactor in nodeCWEinfo){
                                if (nodeExploitationFactor.Name != "Enabling_Factor_for_Exploitation"){
                                    Console.WriteLine("ERROR: Missing code for nodeExploitationFactor " + nodeExploitationFactor.Name);
                                }
                                else{
                                    string sExploitationFactor = nodeExploitationFactor.InnerText;
                                    sExploitationFactor = sExploitationFactor.Replace("<Text>", "");
                                    sExploitationFactor = sExploitationFactor.Replace("</Text>", "");
                                    sExploitationFactor = sExploitationFactor.Replace("<text>", "");
                                    sExploitationFactor = sExploitationFactor.Replace("</text>", "");
                                    sExploitationFactor = CleaningCWEString(sExploitationFactor);
                                    Console.WriteLine("DEBUG sExploitationFactor=" + sExploitationFactor);
                                }
                            }
                            #endregion factorexploitation
                            break;
                        default:
                            //Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                            if (nodeCWEinfo.Name != "Content_History" && nodeCWEinfo.Name != "Maintenance_Notes"){
                                if (nodeCWEinfo.Name != "Relationship_Notes"){
                                    Console.WriteLine("ERROR: nodeCWEinfo Missing code for: " + nodeCWEinfo.Name);   //TODO
                                }
                            }
                            //Content_History
                            //Relationship_Notes
                            break;
                    }
                }
            }
        }

        static string CleaningCWEString(string sStringToClean)
        {
            //Cleaning
            //sStringToClean = sStringToClean.Replace("<Text>", "");
            //sStringToClean = sStringToClean.Replace("</Text>", "");
            sStringToClean = sStringToClean.Replace("<xhtml:p>", "");
            sStringToClean = sStringToClean.Replace("</xhtml:p>", "");
            //Remove CLRF
            sStringToClean = sStringToClean.Replace("\r\n", " ");
            sStringToClean = sStringToClean.Replace("\n", " ");
            sStringToClean = sStringToClean.Replace("\t", " "); //TAB
            /*
            //C# escape characters
            \' for a single quote
            \" for a double quote
            \\ for a backslash
            \0 for a null character
            \a for an alert character
            \b for a backspace
            \f for a form feed
            \n for a new line
            \r for a carriage return
            \t for a horizontal tab
            \v for a vertical tab
            \uxxxx for a unicode character hex value (e.g. \u0020)
            \x is the same as \u, but you don't need leading zeroes (.g. \x20)
            \Uxxxxxxxx for a unicode character hex value (longer form needed for generating surrogates)
            */
            while (sStringToClean.Contains("  "))
            {
                sStringToClean = sStringToClean.Replace("  ", " ");
            }
            return sStringToClean.Trim();
        }
    }
}
