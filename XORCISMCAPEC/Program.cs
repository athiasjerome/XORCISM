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
using ICSharpCode.SharpZipLib.Zip;

namespace Import_capec
{
    static class Program
    {
        /// <summary>
        /// Copyright (C) 2013 Jerome Athias
        /// Parser for CAPEC XML file and import the values into an XORCISM database
        /// This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation; either version 2 of the License, or (at your option) any later version.
        /// 
        /// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
        /// 
        /// You should have received a copy of the GNU General Public License along with this program; if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
        /// </summary>
        [STAThread]
        static void Main()
        {
            //http://capec.mitre.org/data/archive/capec_v2.1.zip
            try
            {
                WebClient wc = new WebClient();
                Console.WriteLine("Downloading capec_v2.1.zip");
                //wc.DownloadFile("http://capec.mitre.org/data/archive/capec_v2.1.zip", "C:/nvdcve/capec_v2.1.zip");
                // 
                wc.Dispose();
                //Console.WriteLine("Download is completed");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while downloading\n" + ex.Message + " " + ex.InnerException);
            }

            //Extract Zip File
            FastZip fz = new FastZip();
//            fz.ExtractZip(@"C:\nvdcve\capec_v2.1.zip", @"C:\nvdcve\", "");
            Console.WriteLine("Extraction Complete !!!");

            XmlDocument doc;
            doc = new XmlDocument();

            //NOTE: probably not the best/fastest way to parse XML but easy/clear enough
            doc.Load(@"C:\nvdcve\capec_v2.1.xml");

            XmlNamespaceManager mgr = new XmlNamespaceManager(doc.NameTable);

            mgr.AddNamespace("capec", "http://capec.mitre.org/capec-2");

            XmlNodeList nodes1;
            //TODO  capec:Views

            nodes1 = doc.SelectNodes("capec:Attack_Pattern_Catalog/capec:Categories/capec:Category",mgr);
            //Console.WriteLine(nodes1.Count);

            XORCISMEntities model;
            model = new XORCISMEntities();

            #region capeccategory
            foreach (XmlNode node in nodes1)    //capec:Category
            {

                string description = node.ChildNodes[0].ChildNodes[0].InnerText;    //Description_Summary   //TODO
                //Console.WriteLine(description);

                XORCISMModel.CAPEC mycapec;
                string mycapecid = node.Attributes["ID"].InnerText;
                string capecidsearch = "CAPEC-" + mycapecid;
                //Cleaning
                string sCleanDescriptionSummary = description;
                //Remove CLRF
                sCleanDescriptionSummary = sCleanDescriptionSummary.Replace("\r\n", " ");
                sCleanDescriptionSummary = sCleanDescriptionSummary.Replace("\n", " ");
                while (sCleanDescriptionSummary.Contains("  "))
                {
                    sCleanDescriptionSummary = sCleanDescriptionSummary.Replace("  ", " ");
                }
                    
                //Console.WriteLine(mycapecid);
                mycapec = model.CAPEC.FirstOrDefault(o => o.capec_id == "CAPEC-"+mycapecid);
                if (mycapec == null)
                {
                    Console.WriteLine(string.Format("Adding new CAPEC [{0}] in table CAPEC", mycapecid));

                    mycapec = new CAPEC();
                    mycapec.capec_id = "CAPEC-" + mycapecid;
                    mycapec.CategoryName = node.Attributes["Name"].InnerText;
                    mycapec.CapecStatus = node.Attributes["Status"].InnerText;   //Draft
                    mycapec.DescriptionSummaryClean = sCleanDescriptionSummary;
                    mycapec.DescriptionSummary = description;
                    mycapec.timestamp = DateTimeOffset.Now;
                    //cpe.NVDID = Convert.ToInt32(nodeMetadata.Attributes["nvd-id"].InnerText);
                    //cpe.ModificationDate = DateTime.Parse(nodeMetadata.Attributes["modification-date"].InnerText, new System.Globalization.CultureInfo("EN-us"));
                    //cpe.Status = nodeMetadata.Attributes["status"].InnerText;
                    try
                    {
                        model.AddToCAPEC(mycapec);
                        model.SaveChanges();
                    }
                    catch (Exception exAddToCAPEC)
                    {
                        Console.WriteLine("Exception exAddToCAPEC " + exAddToCAPEC.Message + " " + exAddToCAPEC.InnerException);
                    }
                }
                else
                {
                    //Update
                    try
                    {
                        mycapec.CategoryName = node.Attributes["Name"].InnerText;   //Data Leakage Attacks
                        mycapec.CapecStatus = node.Attributes["Status"].InnerText;   //Draft
                        mycapec.DescriptionSummaryClean = sCleanDescriptionSummary;
                        mycapec.DescriptionSummary = description;
                        mycapec.timestamp = DateTimeOffset.Now;
                        model.SaveChanges();
                    }
                    catch (Exception exCAPECUpdate)
                    {
                        Console.WriteLine("Exception exCAPECUpdate " + exCAPECUpdate.Message + " " + exCAPECUpdate.InnerException);
                    }
                }

                //CWE
                foreach (XmlNode node2 in node.ChildNodes)
                {
                    //Console.WriteLine(node2.Name);
                    //capec:Description
                    //capec:Related_Weaknesses
                    //capec:Attack_Prerequisites
                    //capec:Resources_Required
                    //capec:Relationships
                    //capec:References
                    //capec:Content_History
                    if (node2.Name == "capec:Related_Weaknesses")
                    {
                        #region relatedweakness
                        foreach (XmlNode nodeWeakness in node2.ChildNodes)  //capec:Related_Weakness
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
                                    mycweid = node3.InnerText;
                                }
                                //capec:Weakness_Relationship_Type
                                if (node3.Name == "capec:Weakness_Relationship_Type")
                                {
                                    myrelationship = node3.InnerText;
                                    //Console.WriteLine(mycweid);
                                    //Console.WriteLine(myrelationship);

                                    XORCISMModel.CWEFORCAPEC mycweforcapec;
                                    mycweforcapec = model.CWEFORCAPEC.FirstOrDefault(o => o.capec_id == "CAPEC-" + mycapecid && o.CWEID == "CWE-"+mycweid);
                                    if (mycweforcapec == null)
                                    {
                                        Console.WriteLine(string.Format("Adding new CWEFORCAPEC [{0}] in table CWEFORCAPEC", mycapecid));

                                        mycweforcapec = new CWEFORCAPEC();
                                        mycweforcapec.capec_id = "CAPEC-" + mycapecid;
                                        mycweforcapec.CWEID = "CWE-" + mycweid;
                                        mycweforcapec.WeaknessRelationship = myrelationship;
                                        model.AddToCWEFORCAPEC(mycweforcapec);

                                        model.SaveChanges();
                                    }
                                }

                                


                            }

                        }
                        #endregion relatedweakness
                    }
                    if (node2.Name == "capec:Attack_Prerequisites")
                    {
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
                                    string sCleanPrerequisiteText = node4.InnerText;
                                    //Remove CLRF
                                    sCleanPrerequisiteText = sCleanPrerequisiteText.Replace("\r\n", " ");
                                    sCleanPrerequisiteText = sCleanPrerequisiteText.Replace("\n", " ");
                                    //sCleanPrerequisiteText = sCleanPrerequisiteText.Replace("                    ", " ");
                                    //sCleanPrerequisiteText = sCleanPrerequisiteText.Replace("                   ", " ");
                                    //sCleanPrerequisiteText = sCleanPrerequisiteText.Replace("                  ", " ");
                                    //sCleanPrerequisiteText = sCleanPrerequisiteText.Replace("                 ", " ");
                                    while (sCleanPrerequisiteText.Contains("  "))
                                    {
                                        sCleanPrerequisiteText = sCleanPrerequisiteText.Replace("  ", " ");
                                    }

                                    XORCISMModel.ATTACKPREREQUISITE myattackpreq;
                                    myattackpreq = model.ATTACKPREREQUISITE.FirstOrDefault(o => o.PrerequisiteTextRaw == node4.InnerText);    // && o.VocabularyID == 4);
                                    if (myattackpreq == null)
                                    {
                                        Console.WriteLine(string.Format("Adding new ATTACKPREREQUISITE [{0}] in table ATTACKPREREQUISITE", mycapecid));
                                        myattackpreq = new ATTACKPREREQUISITE();
                                        myattackpreq.PrerequisiteText = sCleanPrerequisiteText;
                                        myattackpreq.PrerequisiteTextRaw = node4.InnerText;
                                        myattackpreq.VocabularyID = 4;  //TODO Hardcoded
                                        try
                                        {
                                            model.AddToATTACKPREREQUISITE(myattackpreq);

                                            //model.SaveChanges();
                                        }
                                        catch (Exception exAddToATTACKPREREQUISITE)
                                        {
                                            Console.WriteLine("Exception exAddToATTACKPREREQUISITE " + exAddToATTACKPREREQUISITE.Message + " " + exAddToATTACKPREREQUISITE.InnerException);
                                        }
                                    }
                                    else
                                    {
                                        //Updating ATTACKPREREQUISITE
                                        Console.WriteLine("Updating ATTACKPREREQUISITE " + myattackpreq.AttackPrerequisiteID);
                                        
                                        
                                        myattackpreq.PrerequisiteText = sCleanPrerequisiteText;
                                        myattackpreq.PrerequisiteTextRaw = node4.InnerText;
                                    }
                                    try
                                    {
                                        model.SaveChanges();
                                    }
                                    catch (Exception exATTACKPREREQUISITE)
                                    {
                                        Console.WriteLine("Exception exATTACKPREREQUISITE " + exATTACKPREREQUISITE.Message + " " + exATTACKPREREQUISITE.InnerException);
                                    }

                                    XORCISMModel.ATTACKPREREQUISITEFORCAPEC myattackpreqforcapec;
                                    myattackpreqforcapec = model.ATTACKPREREQUISITEFORCAPEC.FirstOrDefault(o => o.capec_id == capecidsearch && o.AttackPrerequisiteID == myattackpreq.AttackPrerequisiteID);    // && o.VocabularyID == 4);
                                    if (myattackpreqforcapec == null)
                                    {
                                        Console.WriteLine(string.Format("Adding new ATTACKPREREQUISITEFORCAPEC [{0}] in table ATTACKPREREQUISITEFORCAPEC", mycapecid));
                                        myattackpreqforcapec = new ATTACKPREREQUISITEFORCAPEC();
                                        myattackpreqforcapec.capec_id = capecidsearch;
                                        myattackpreqforcapec.AttackPrerequisiteID = myattackpreq.AttackPrerequisiteID;
                                        myattackpreqforcapec.timestamp = DateTimeOffset.Now;
                                        try
                                        {
                                            model.AddToATTACKPREREQUISITEFORCAPEC(myattackpreqforcapec);

                                            model.SaveChanges();
                                        }
                                        catch (Exception exAddToATTACKPREREQUISITEFORCAPEC)
                                        {
                                            Console.WriteLine("Exception exAddToATTACKPREREQUISITEFORCAPEC " + exAddToATTACKPREREQUISITEFORCAPEC.Message + " " + exAddToATTACKPREREQUISITEFORCAPEC.InnerException);
                                        }
                                    }
                                }
                            }
                        }
                        #endregion AttackPrerequisite
                    }

                    //
                    if (node2.Name == "capec:Resources_Required")
                    {
                        #region resourcesrequired
                        foreach (XmlNode nodeResReq in node2.ChildNodes)  //capec:Resources_Required
                        {
                            
                                //Console.WriteLine(node4.Name);
                                //capec:Text
                                if (nodeResReq.Name == "capec:Text")
                                {
                                    //Cleaning
                                    string sCleanAttackResourceText = nodeResReq.InnerText;
                                    //Remove CLRF
                                    sCleanAttackResourceText = sCleanAttackResourceText.Replace("\r\n", " ");
                                    sCleanAttackResourceText = sCleanAttackResourceText.Replace("\n", " ");
                                    //sCleanAttackResourceText = sCleanAttackResourceText.Replace("                    ", " ");
                                    //sCleanAttackResourceText = sCleanAttackResourceText.Replace("                   ", " ");
                                    //sCleanAttackResourceText = sCleanAttackResourceText.Replace("                  ", " ");
                                    //sCleanAttackResourceText = sCleanAttackResourceText.Replace("                 ", " ");
                                    while(sCleanAttackResourceText.Contains("  "))
                                    {
                                        sCleanAttackResourceText = sCleanAttackResourceText.Replace("  ", " ");
                                    }
                                    XORCISMModel.ATTACKRESOURCE myattackres;
                                    myattackres = model.ATTACKRESOURCE.FirstOrDefault(o => o.AttackResourceTextRaw == nodeResReq.InnerText);    // && o.VocabularyID == 4);
                                    if (myattackres == null)
                                    {
                                        Console.WriteLine(string.Format("Adding new ATTACKRESOURCE [{0}] in table ATTACKRESOURCE", mycapecid));
                                        myattackres = new ATTACKRESOURCE();
                                        myattackres.AttackResourceText = sCleanAttackResourceText;
                                        myattackres.AttackResourceTextRaw = nodeResReq.InnerText;
                                        myattackres.VocabularyID = 4;  //TODO Hardcoded
                                        model.AddToATTACKRESOURCE(myattackres);

                                        //model.SaveChanges();
                                    }
                                    else
                                    {
                                        //Update
                                        myattackres.AttackResourceText = sCleanAttackResourceText;
                                    }
                                    model.SaveChanges();

                                    XORCISMModel.ATTACKRESOURCEFORCAPECS myattackresforcapec;
                                    myattackresforcapec = model.ATTACKRESOURCEFORCAPECS.FirstOrDefault(o => o.capec_id == capecidsearch && o.AttackResourceID == myattackres.AttackResourceID);    // && o.VocabularyID == 4);
                                    if (myattackresforcapec == null)
                                    {
                                        Console.WriteLine(string.Format("Adding new ATTACKRESOURCEFORCAPEC [{0}] in table ATTACKRESOURCEFORCAPEC", mycapecid));
                                        myattackresforcapec = new ATTACKRESOURCEFORCAPECS();
                                        myattackresforcapec.capec_id = capecidsearch;
                                        myattackresforcapec.AttackResourceID = myattackres.AttackResourceID;
                                        model.AddToATTACKRESOURCEFORCAPECS(myattackresforcapec);

                                        model.SaveChanges();
                                    }
                                }

                        }
                        #endregion resourcesrequired
                    }

                    if (node2.Name == "capec:Relationships")
                    {
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
                                    //if (sTargetForm == "Category")
                                    //{
                                        if (mycapecid != "126" && mycapecid != "224" && mycapecid != "278")  //Because error    TODO HARCODED
                                        {
                                            XORCISMModel.CAPECRELATIONSHIP mycapecrel;
                                            mycapecrel = model.CAPECRELATIONSHIP.FirstOrDefault(o => o.capec_id == capecidsearch && o.RelationshipTargetID == scapecidTarget);    // && o.VocabularyID == 4);
                                            if (mycapecrel == null)
                                            {
                                                Console.WriteLine(string.Format("Adding new CAPECRELATIONSHIP [{0}] in table CAPECRELATIONSHIP", mycapecid));
                                                mycapecrel = new CAPECRELATIONSHIP();
                                                mycapecrel.capec_id = capecidsearch;
                                                mycapecrel.RelationshipNature = sNature;
                                                mycapecrel.RelationshipTargetForm = sTargetForm;
                                                mycapecrel.RelationshipTargetID = scapecidTarget;

                                                //mycapecrel.VocabularyID = 4;  //TODO Hardcoded
                                                model.AddToCAPECRELATIONSHIP(mycapecrel);

                                                model.SaveChanges();
                                            }
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
                    }

                }

            }
            #endregion capeccategory

            nodes1 = doc.SelectNodes("capec:Attack_Pattern_Catalog/capec:Attack_Patterns/capec:Attack_Pattern", mgr);
            #region AttackPattern
            foreach (XmlNode node in nodes1)    //capec:Attack_Pattern
            {

                string description = node.ChildNodes[0].ChildNodes[0].InnerText;    //Description_Summary   //TODO
                //Console.WriteLine(description);   //DEBUG
                string mycapecid = node.Attributes["ID"].InnerText;
                string capecidsearch = "CAPEC-" + mycapecid;
                int myattackpatternid = 0;

                XORCISMModel.CAPEC mycapec;
                mycapec = model.CAPEC.FirstOrDefault(o => o.capec_id == capecidsearch);
                //Cleaning
                string sCleanDescriptionSummary = description;
                //Remove CLRF
                sCleanDescriptionSummary = sCleanDescriptionSummary.Replace("\r\n", " ");
                sCleanDescriptionSummary = sCleanDescriptionSummary.Replace("\n", " ");
                while (sCleanDescriptionSummary.Contains("  "))
                {
                    sCleanDescriptionSummary = sCleanDescriptionSummary.Replace("  ", " ");
                }
                if (mycapec == null)
                {
                    Console.WriteLine(string.Format("Adding new CAPEC [{0}] for AttackPattern in table CAPEC", mycapecid));

                    mycapec = new CAPEC();
                    mycapec.capec_id = capecidsearch;
                    mycapec.CategoryName = "";  // node.Attributes["Name"].InnerText;   //TODO?
                    mycapec.CapecStatus = node.Attributes["Status"].InnerText;   //Draft
                    mycapec.DescriptionSummaryClean = sCleanDescriptionSummary;
                    mycapec.DescriptionSummary = description;
                    mycapec.timestamp = DateTimeOffset.Now;
                    try
                    {
                        model.AddToCAPEC(mycapec);

                        model.SaveChanges();
                    }
                    catch (Exception exAddToCAPEC1)
                    {
                        Console.WriteLine("Exception exAddToCAPEC1 " + exAddToCAPEC1.Message + " " + exAddToCAPEC1.InnerException);
                    }
                }
                else
                {
                    //TODO  Update
                    mycapec.CapecStatus = node.Attributes["Status"].InnerText;   //Draft
                    mycapec.DescriptionSummaryClean = sCleanDescriptionSummary;
                    mycapec.DescriptionSummary = description;
                    mycapec.timestamp = DateTimeOffset.Now;
                    model.SaveChanges();
                }

                //TODO: model.SaveChanges(); here

                //**************************************************************************************************************
                //TODO: AttackPatternID into CAPEC
                XORCISMModel.ATTACKPATTERN myattackpat;
                
                //Console.WriteLine(mycapecid);
                myattackpat = model.ATTACKPATTERN.FirstOrDefault(o => o.capec_id == "CAPEC-" + mycapecid);
                string sAttackPatternDescription = node.ChildNodes[0].ChildNodes[0].ChildNodes[0].InnerText;    //<capec:Description><capec:Summary><capec:Text>   //TODO
                //Cleaning
                string sCleanAttackPatternDescription = sAttackPatternDescription;
                //Remove CLRF
                sCleanAttackPatternDescription = sCleanAttackPatternDescription.Replace("\r\n", " ");
                sCleanAttackPatternDescription = sCleanAttackPatternDescription.Replace("\n", " ");
                while (sCleanAttackPatternDescription.Contains("  "))
                {
                    sCleanAttackPatternDescription = sCleanAttackPatternDescription.Replace("  ", " ");
                }

                if (myattackpat == null)
                {
                    Console.WriteLine(string.Format("Adding new ATTACKPATTERN [{0}] in table ATTACKPATTERN", mycapecid));

                    myattackpat = new ATTACKPATTERN();
                    myattackpat.capec_id = "CAPEC-" + mycapecid;
                    myattackpat.AttackPatternName = node.Attributes["Name"].InnerText;
                    myattackpat.PatternAbstraction = node.Attributes["Pattern_Abstraction"].InnerText;
                    myattackpat.AttackPatternDescription = sCleanAttackPatternDescription;
                    //myattackpat.AttackPatternDescriptionRaw = sAttackPatternDescription;    //TODO: uncomment
                    try
                    {
                        myattackpat.PatternCompleteness = node.Attributes["Pattern_Completeness"].InnerText;
                    }
                    catch (Exception exPatternCompleteness)
                    {
                        Console.WriteLine(capecidsearch + " has no Pattern_Completeness");
                    }
                    myattackpat.PatternStatus = node.Attributes["Status"].InnerText;


                    model.AddToATTACKPATTERN(myattackpat);

                    //model.SaveChanges();
                }
                else
                {
                    //TODO  Update
                    myattackpat.AttackPatternName = node.Attributes["Name"].InnerText;
                    myattackpat.PatternAbstraction = node.Attributes["Pattern_Abstraction"].InnerText;
                    myattackpat.AttackPatternDescription = sCleanAttackPatternDescription;
                    //myattackpat.AttackPatternDescriptionRaw = sAttackPatternDescription;    //TODO: uncomment

                }
                try
                {
                    model.SaveChanges();
                }
                catch (Exception exATTACKPATTERN1)
                {
                    Console.WriteLine("Exception exATTACKPATTERN1 " + exATTACKPATTERN1.Message + " " + exATTACKPATTERN1.InnerException);
                }

                myattackpatternid = myattackpat.AttackPatternID;

                foreach (XmlNode nodeAP in node.ChildNodes)
                {
                    //Console.WriteLine(nodeAP.Name);
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
                    capec:Purposes
                    capec:CIA_Impact
                    capec:Technical_Context
                    capec:References
                    capec:Content_History
                    */

                    int iAttackMotivationConsequenceAdded = 0;
                    switch (nodeAP.Name)
                    {
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
                                        string sCleanPrerequisiteText = node4.InnerText;
                                        //Remove CLRF
                                        sCleanPrerequisiteText = sCleanPrerequisiteText.Replace("\r\n", " ");
                                        sCleanPrerequisiteText = sCleanPrerequisiteText.Replace("\n", " ");
                                        //sCleanPrerequisiteText = sCleanPrerequisiteText.Replace("                    ", " ");
                                        //sCleanPrerequisiteText = sCleanPrerequisiteText.Replace("                   ", " ");
                                        //sCleanPrerequisiteText = sCleanPrerequisiteText.Replace("                  ", " ");
                                        //sCleanPrerequisiteText = sCleanPrerequisiteText.Replace("                 ", " ");
                                        while (sCleanPrerequisiteText.Contains("  "))
                                        {
                                            sCleanPrerequisiteText = sCleanPrerequisiteText.Replace("  ", " ");
                                        }

                                        XORCISMModel.ATTACKPREREQUISITE myattackpreq;
                                        myattackpreq = model.ATTACKPREREQUISITE.FirstOrDefault(o => o.PrerequisiteTextRaw == node4.InnerText);    // && o.VocabularyID == 4);
                                        if (myattackpreq == null)
                                        {
                                            Console.WriteLine(string.Format("Adding new ATTACKPREREQUISITE [{0}] in table ATTACKPREREQUISITE", mycapecid));
                                            myattackpreq = new ATTACKPREREQUISITE();
                                            myattackpreq.PrerequisiteText = sCleanPrerequisiteText;
                                            myattackpreq.PrerequisiteTextRaw = node4.InnerText;
                                            myattackpreq.VocabularyID = 4;  //TODO Hardcoded
                                            model.AddToATTACKPREREQUISITE(myattackpreq);

                                            //model.SaveChanges();
                                        }
                                        else
                                        {
                                            //UPDATE
                                            myattackpreq.PrerequisiteText = sCleanPrerequisiteText;
                                        }
                                        model.SaveChanges();

                                        XORCISMModel.ATTACKPREREQUISITEFORCAPEC myattackpreqforcapec;
                                        myattackpreqforcapec = model.ATTACKPREREQUISITEFORCAPEC.FirstOrDefault(o => o.capec_id == capecidsearch && o.AttackPrerequisiteID == myattackpreq.AttackPrerequisiteID);    // && o.VocabularyID == 4);
                                        if (myattackpreqforcapec == null)
                                        {
                                            Console.WriteLine(string.Format("Adding new ATTACKPREREQUISITEFORCAPEC [{0}] in table ATTACKPREREQUISITEFORCAPEC", mycapecid));
                                            myattackpreqforcapec = new ATTACKPREREQUISITEFORCAPEC();
                                            myattackpreqforcapec.capec_id = capecidsearch;
                                            myattackpreqforcapec.AttackPrerequisiteID = myattackpreq.AttackPrerequisiteID;
                                            myattackpreqforcapec.timestamp = DateTimeOffset.Now;
                                            model.AddToATTACKPREREQUISITEFORCAPEC(myattackpreqforcapec);

                                            model.SaveChanges();
                                        }
                                    }
                                }
                            }
                            #endregion AttackPrerequisite

                            break;
                        case "capec:Typical_Severity":
                            mycapec.TypicalSeverity = nodeAP.InnerText;
                            model.SaveChanges();
                            myattackpat.TypicalSeverity = nodeAP.InnerText;
                            model.SaveChanges();
                            break;
                        case "capec:Attack_Motivation-Consequences":
                            #region Attack_Motivation-Consequences
                            int cptAttackCons = 0;
                            foreach (XmlNode nodeAMC in nodeAP.ChildNodes)  //capec:Attack_Motivation-Consequence
                            {
                                cptAttackCons++;
                                int consid = 0;
                                //capec:Attack_Motivation-Consequence
                                XORCISMModel.ATTACKCONSEQUENCE myattackcons = new ATTACKCONSEQUENCE();
                                //TODO
                                //Check for each ATTACKCONSEQUENCE for this CAPEC if the ATTACKCONSEQUENCE matches
                                var syn2 = from S in model.ATTACKCONSEQUENCEFORCAPEC
                                           where S.capec_id == capecidsearch
                                           select S;
                                if (syn2.Count() != 0)
                                {
                                    int cptline = 0;
                                    foreach (ATTACKCONSEQUENCEFORCAPEC conscapec in syn2.ToList())
                                    {
                                        cptline++;
                                        if (cptline == cptAttackCons)
                                        {
                                            consid = conscapec.AttackConsequenceID;
                                        }
                                    }
                                }
                                if(consid == 0)
                                {
                                    Console.WriteLine(string.Format("Adding new ATTACKCONSEQUENCE [{0}] in table ATTACKCONSEQUENCE", mycapecid));
                                    myattackcons = new ATTACKCONSEQUENCE();
                                    model.AddToATTACKCONSEQUENCE(myattackcons);
                                    model.SaveChanges();
                                    consid = myattackcons.AttackConsequenceID;
                                }

                                XORCISMModel.ATTACKCONSEQUENCEFORCAPEC myattackconsforcapec=new ATTACKCONSEQUENCEFORCAPEC();
                                myattackconsforcapec = model.ATTACKCONSEQUENCEFORCAPEC.FirstOrDefault(o => o.capec_id == capecidsearch);    // && o.VocabularyID == 4);
                                if (myattackconsforcapec == null || iAttackMotivationConsequenceAdded > 0)
                                {
                                    iAttackMotivationConsequenceAdded++;
                                    //Create a ATTACKCONSEQUENCEFORCAPEC
                                    

                                    Console.WriteLine(string.Format("Adding new ATTACKCONSEQUENCEFORCAPEC [{0}] in table ATTACKCONSEQUENCEFORCAPEC", mycapecid));
                                    myattackconsforcapec = new ATTACKCONSEQUENCEFORCAPEC();
                                    myattackconsforcapec.AttackConsequenceID = consid; //myattackcons.AttackConsequenceID;
                                    myattackconsforcapec.capec_id = capecidsearch;
                                    model.AddToATTACKCONSEQUENCEFORCAPEC(myattackconsforcapec);
                                    model.SaveChanges();
                                }
                                else
                                {
                                    //TODO
                                    //Check for each ATTACKCONSEQUENCEFORCAPEC for this CAPEC if the ATTACKCONSEQUENCE matches
                                    //myattackcons.AttackConsequenceID = myattackconsforcapec.AttackConsequenceID;

                                }


                                foreach (XmlNode nodeAMCchild in nodeAMC.ChildNodes)
                                {
                                    //Console.WriteLine(nodeAMCchild.Name);   //DEBUG
                                    //capec:Consequence_Scope
                                    //capec:Consequence_Technical_Impact
                                    switch (nodeAMCchild.Name)
                                    {
                                        case "capec:Consequence_Scope":
                                            XORCISMModel.ATTACKCONSEQUENCESCOPE myattackconsscope;
                                            myattackconsscope = model.ATTACKCONSEQUENCESCOPE.FirstOrDefault(o => o.ConsequenceScope == nodeAMCchild.InnerText);    // && o.VocabularyID == 4);
                                            if (myattackconsscope == null)
                                            {
                                                Console.WriteLine(string.Format("Adding new ATTACKCONSEQUENCESCOPE [{0}] in table ATTACKCONSEQUENCESCOPE", mycapecid));
                                                myattackconsscope = new ATTACKCONSEQUENCESCOPE();
                                                myattackconsscope.ConsequenceScope = nodeAMCchild.InnerText;
                                                myattackconsscope.VocabularyID = 4; //TODO HARDCODED
                                                model.AddToATTACKCONSEQUENCESCOPE(myattackconsscope);
                                                model.SaveChanges();
                                            }


                                            XORCISMModel.ATTACKCONSEQUENCESCOPEFORATTACKCONSEQUENCE myattackconsscopeforattackcons=new ATTACKCONSEQUENCESCOPEFORATTACKCONSEQUENCE();
                                            myattackconsscopeforattackcons = model.ATTACKCONSEQUENCESCOPEFORATTACKCONSEQUENCE.FirstOrDefault(o => o.AttackConsequenceID == consid && o.AttackConsequenceScopeID == myattackconsscope.AttackConsequenceScopeID);    // && o.VocabularyID == 4);
                                            if (myattackconsscopeforattackcons == null)
                                            {
                                                Console.WriteLine(string.Format("Adding new ATTACKCONSEQUENCESCOPEFORATTACKCONSEQUENCE [{0}] in table ATTACKCONSEQUENCESCOPEFORATTACKCONSEQUENCE", mycapecid));
                                                myattackconsscopeforattackcons = new ATTACKCONSEQUENCESCOPEFORATTACKCONSEQUENCE();
                                                myattackconsscopeforattackcons.AttackConsequenceID = consid; //myattackcons.AttackConsequenceID;
                                                myattackconsscopeforattackcons.AttackConsequenceScopeID = myattackconsscope.AttackConsequenceScopeID;
                                                model.AddToATTACKCONSEQUENCESCOPEFORATTACKCONSEQUENCE(myattackconsscopeforattackcons);
                                                model.SaveChanges();
                                            }
                                            break;
                                        case "capec:Consequence_Technical_Impact":
                                            //Cleaning
                                            string sCleanConsequenceTechnicalImpact = nodeAMCchild.InnerText;
                                            //Remove CLRF
                                            sCleanConsequenceTechnicalImpact = sCleanConsequenceTechnicalImpact.Replace("\r\n", " ");
                                            sCleanConsequenceTechnicalImpact = sCleanConsequenceTechnicalImpact.Replace("\n", " ");
                                            //sCleanConsequenceTechnicalImpact = sCleanConsequenceTechnicalImpact.Replace("                    ", " ");
                                            //sCleanConsequenceTechnicalImpact = sCleanConsequenceTechnicalImpact.Replace("                   ", " ");
                                            //sCleanConsequenceTechnicalImpact = sCleanConsequenceTechnicalImpact.Replace("                  ", " ");
                                            //sCleanConsequenceTechnicalImpact = sCleanConsequenceTechnicalImpact.Replace("                 ", " ");
                                            while (sCleanConsequenceTechnicalImpact.Contains("  "))
                                            {
                                                sCleanConsequenceTechnicalImpact = sCleanConsequenceTechnicalImpact.Replace("  ", " ");
                                            }

                                            XORCISMModel.ATTACKTECHNICALIMPACT myattacktechimpact = new ATTACKTECHNICALIMPACT();
                                            myattacktechimpact = model.ATTACKTECHNICALIMPACT.FirstOrDefault(o => o.ConsequenceTechnicalImpactRaw == nodeAMCchild.InnerText);    // && o.VocabularyID == 4);
                                            if (myattacktechimpact == null)
                                            {
                                                Console.WriteLine(string.Format("Adding new ATTACKTECHNICALIMPACT [{0}] in table ATTACKTECHNICALIMPACT", mycapecid));
                                                myattacktechimpact = new ATTACKTECHNICALIMPACT();
                                                myattacktechimpact.ConsequenceTechnicalImpact = sCleanConsequenceTechnicalImpact;
                                                myattacktechimpact.ConsequenceTechnicalImpactRaw = nodeAMCchild.InnerText;
                                                myattacktechimpact.VocabularyID = 4; //TODO HARDCODED
                                                model.AddToATTACKTECHNICALIMPACT(myattacktechimpact);

                                                //model.SaveChanges();
                                            }
                                            else
                                            {
                                                //Update
                                                myattacktechimpact.ConsequenceTechnicalImpact = sCleanConsequenceTechnicalImpact;
                                            }
                                            model.SaveChanges();

                                            XORCISMModel.ATTACKTECHNICALIMPACTFORATTACKCONSEQUENCE myattackimpactforattackcons = new ATTACKTECHNICALIMPACTFORATTACKCONSEQUENCE();
                                            myattackimpactforattackcons = model.ATTACKTECHNICALIMPACTFORATTACKCONSEQUENCE.FirstOrDefault(o => o.AttackConsequenceID == consid && o.AttackTechnicalImpactID == myattacktechimpact.AttackTechnicalImpactID);    // && o.VocabularyID == 4);
                                            if (myattackimpactforattackcons == null)
                                            {
                                                Console.WriteLine(string.Format("Adding new ATTACKTECHNICALIMPACTFORATTACKCONSEQUENCE [{0}] in table ATTACKTECHNICALIMPACTFORATTACKCONSEQUENCE", mycapecid));
                                                myattackimpactforattackcons = new ATTACKTECHNICALIMPACTFORATTACKCONSEQUENCE();
                                                myattackimpactforattackcons.AttackConsequenceID = consid; //myattackcons.AttackConsequenceID;
                                                myattackimpactforattackcons.AttackTechnicalImpactID = myattacktechimpact.AttackTechnicalImpactID;
                                                myattackimpactforattackcons.timestamp = DateTimeOffset.Now;
                                                model.AddToATTACKTECHNICALIMPACTFORATTACKCONSEQUENCE(myattackimpactforattackcons);
                                                model.SaveChanges();
                                            }
                                            break;
                                        default:
                                            Console.WriteLine("TODO: code for capecConsequence " + nodeAP.Name);
                                            break;
                                    }
                                }
                            }
                            #endregion Attack_Motivation-Consequences
                            break;
                        case "capec:Related_Attack_Patterns":
                            #region Related_Attack_Patterns
                            string sTargetForm = "";
                            string sNature = "";
                            //Console.WriteLine(nodeRelation.Name);   //capec:Relationship
                            foreach (XmlNode node3 in nodeAP.ChildNodes)
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
                                    //if (sTargetForm == "Category")
                                    //{
                                        //TODO
                                        if (mycapecid != "126" && mycapecid != "224" && mycapecid != "278")  //Because error    TODO HARCODED
                                        {
                                            XORCISMModel.CAPECRELATIONSHIP mycapecrel;
                                            mycapecrel = model.CAPECRELATIONSHIP.FirstOrDefault(o => o.capec_id == capecidsearch && o.RelationshipTargetID == scapecidTarget);    // && o.VocabularyID == 4);
                                            if (mycapecrel == null)
                                            {
                                                Console.WriteLine(string.Format("Adding new CAPECRELATIONSHIP [{0}] in table CAPECRELATIONSHIP", mycapecid));
                                                mycapecrel = new CAPECRELATIONSHIP();
                                                mycapecrel.capec_id = capecidsearch;
                                                mycapecrel.RelationshipNature = sNature;
                                                mycapecrel.RelationshipTargetForm = sTargetForm;
                                                mycapecrel.RelationshipTargetID = scapecidTarget;

                                                //mycapecrel.VocabularyID = 4;  //TODO Hardcoded
                                                model.AddToCAPECRELATIONSHIP(mycapecrel);

                                                model.SaveChanges();
                                            }
                                        }
                                    //}
                                    //if (sTargetForm == "Attack Pattern")
                                    //{
                                        //TODO

                                    //}
                                }

                            }
                            #endregion Related_Attack_Patterns
                            break;
                        case "capec:References":
                            #region CAPECReferences
                            foreach (XmlNode nodeRef in nodeAP.ChildNodes)  //capec:Reference
                            {
                                string sRefID = "";
                                try
                                {
                                    sRefID = nodeRef.Attributes["Reference_ID"].InnerText;
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("CAPECREFERENCE without Reference_ID for " + capecidsearch);
                                }
                                string sLocalRefID = "";
                                try
                                {
                                    sLocalRefID = nodeRef.Attributes["Local_Reference_ID"].InnerText;
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("CAPECREFERENCE without Local_Reference_ID for " + capecidsearch);
                                }


                                XORCISMModel.CAPECREFERENCE mycapecref = new CAPECREFERENCE();
                                mycapecref = model.CAPECREFERENCE.FirstOrDefault(o => o.Reference_ID == sRefID && o.Local_Reference_ID == sLocalRefID);    // && o.VocabularyID == 4);
                                if (mycapecref == null)
                                {
                                    Console.WriteLine(string.Format("Adding new CAPECREFERENCE [{0}] in table CAPECREFERENCE", mycapecid));
                                    mycapecref = new CAPECREFERENCE();
                                    mycapecref.Reference_ID = sRefID;
                                    mycapecref.Local_Reference_ID = sLocalRefID;

                                    //mycapecrel.VocabularyID = 4;  //TODO Hardcoded
                                    model.AddToCAPECREFERENCE(mycapecref);
                                    model.SaveChanges();
                                }

                                XORCISMModel.CAPECREFERENCEFORCAPEC mycapecrefforcapec = new CAPECREFERENCEFORCAPEC();
                                mycapecrefforcapec = model.CAPECREFERENCEFORCAPEC.FirstOrDefault(o => o.CapecReferenceID == mycapecref.CapecReferenceID && o.capec_id == capecidsearch);    // && o.VocabularyID == 4);
                                if (mycapecrefforcapec == null)
                                {
                                    Console.WriteLine(string.Format("Adding new CAPECREFERENCEFORCAPEC [{0}] in table CAPECREFERENCEFORCAPEC", mycapecid));
                                    mycapecrefforcapec = new CAPECREFERENCEFORCAPEC();
                                    mycapecrefforcapec.CapecReferenceID = mycapecref.CapecReferenceID;
                                    mycapecrefforcapec.capec_id = capecidsearch;

                                    //mycapecrel.VocabularyID = 4;  //TODO Hardcoded
                                    model.AddToCAPECREFERENCEFORCAPEC(mycapecrefforcapec);
                                    model.SaveChanges();
                                }

                                foreach (XmlNode nodeRefAtt in nodeRef.ChildNodes)
                                {
                                    switch (nodeRefAtt.Name)
                                    {
                                        case "capec:Reference_Author":
                                            string sRefAuthor = nodeRefAtt.InnerText;
                                            XORCISMModel.AUTHOR myrefauthor = new AUTHOR();
                                            myrefauthor = model.AUTHOR.FirstOrDefault(o => o.AuthorName == sRefAuthor);
                                            if (myrefauthor == null)
                                            {
                                                Console.WriteLine(string.Format("Adding new AUTHOR [{0}] in table AUTHOR", mycapecid));
                                                myrefauthor = new AUTHOR();
                                                myrefauthor.AuthorName = sRefAuthor;
                                                //TODO: check for PersonID in PERSON

                                                model.AddToAUTHOR(myrefauthor);
                                                model.SaveChanges();
                                            }

                                            XORCISMModel.CAPECREFERENCEAUTHOR mycapecrefauthor = new CAPECREFERENCEAUTHOR();
                                            mycapecrefauthor = model.CAPECREFERENCEAUTHOR.FirstOrDefault(o => o.AuthorID == myrefauthor.AuthorID && o.CapecReferenceID == mycapecref.CapecReferenceID);
                                            if (mycapecrefauthor == null)
                                            {
                                                Console.WriteLine(string.Format("Adding new CAPECREFERENCEAUTHOR [{0}] in table CAPECREFERENCEAUTHOR", mycapecid));
                                                mycapecrefauthor = new CAPECREFERENCEAUTHOR();
                                                mycapecrefauthor.AuthorID = myrefauthor.AuthorID;
                                                mycapecrefauthor.CapecReferenceID = mycapecref.CapecReferenceID;

                                                model.AddToCAPECREFERENCEAUTHOR(mycapecrefauthor);
                                                model.SaveChanges();
                                            }

                                            break;
                                        case "capec:Reference_Title":
                                            mycapecref.ReferenceTitle = nodeRefAtt.InnerText;
                                            model.SaveChanges();
                                            break;
                                        case "capec:Reference_Section":
                                            mycapecref.ReferenceSection = nodeRefAtt.InnerText;
                                            model.SaveChanges();
                                            break;
                                        case "capec:Reference_Edition":
                                            mycapecref.ReferenceEdition = nodeRefAtt.InnerText;
                                            model.SaveChanges();
                                            break;
                                        case "capec:Reference_Publisher":
                                            mycapecref.ReferencePublisher = nodeRefAtt.InnerText;
                                            model.SaveChanges();
                                            break;
                                        case "capec:Reference_Publication":
                                            mycapecref.ReferencePublication = nodeRefAtt.InnerText;
                                            model.SaveChanges();
                                            break;
                                        case "capec:Reference_PubDate":
                                            mycapecref.ReferencePubDate = nodeRefAtt.InnerText;
                                            model.SaveChanges();
                                            break;
                                        case "capec:Reference_Link":
                                            mycapecref.ReferenceLink = nodeRefAtt.InnerText;
                                            model.SaveChanges();
                                            break;
                                        default:
                                            Console.WriteLine("TODO: code for nodeRefAtt.Name " + nodeRefAtt.Name);
                                            break;
                                    }
                                }
                            }
                            #endregion CAPECReferences
                            break;
                        case "capec:CIA_Impact":
                            #region CAPECCIA_Impact
                            XORCISMModel.CIAIMPACTFORCAPEC myciaimpactforcapec;
                            myciaimpactforcapec = model.CIAIMPACTFORCAPEC.FirstOrDefault(o => o.capec_id == capecidsearch);
                            if (myciaimpactforcapec == null)
                            {
                                Console.WriteLine(string.Format("Adding new CIAIMPACTFORCAPEC [{0}] in table CIAIMPACTFORCAPEC", mycapecid));
                                myciaimpactforcapec = new CIAIMPACTFORCAPEC();
                                myciaimpactforcapec.capec_id = capecidsearch;

                                model.AddToCIAIMPACTFORCAPEC(myciaimpactforcapec);
                                model.SaveChanges();
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
                                        Console.WriteLine("Missing code for CIAImpactForCapec " + nodeCIA.Name);
                                        break;
                                }
                            }
                            model.SaveChanges();
                            #endregion CAPECCIA_Impact
                            break;
                        case "capec:Purposes":
                            #region CAPECPurposes
                            foreach (XmlNode nodePurpose in nodeAP.ChildNodes)
                            {
                                switch (nodePurpose.Name)
                                {
                                    case "capec:Purpose":
                                        string sAttackPurposeName = nodePurpose.InnerText;
                                        XORCISMModel.ATTACKPURPOSE myattackpurpose;
                                        myattackpurpose = model.ATTACKPURPOSE.FirstOrDefault(o => o.AttackPurposeName == sAttackPurposeName);   //&& o.VocabularyID == 4
                                        if (myattackpurpose == null)
                                        {
                                            Console.WriteLine(string.Format("Adding new ATTACKPURPOSE [{0}] in table ATTACKPURPOSE", mycapecid));
                                            myattackpurpose = new ATTACKPURPOSE();
                                            myattackpurpose.AttackPurposeName = sAttackPurposeName;
                                            myattackpurpose.VocabularyID = 4;   //TODO HARDCODED
                                            model.AddToATTACKPURPOSE(myattackpurpose);
                                            model.SaveChanges();
                                        }
                                        XORCISMModel.ATTACKPURPOSEFORATTACKPATTERN myattackpurposeforattackpattern;
                                        myattackpurposeforattackpattern = model.ATTACKPURPOSEFORATTACKPATTERN.FirstOrDefault(o => o.AttackPurposeID == myattackpurpose.AttackPurposeID && o.AttackPatternID == myattackpatternid);  //&& o.capec_id = capecidsearch
                                        if (myattackpurposeforattackpattern == null)
                                        {
                                            Console.WriteLine(string.Format("Adding new ATTACKPURPOSE [{0}] in table ATTACKPURPOSE", mycapecid));
                                            myattackpurposeforattackpattern = new ATTACKPURPOSEFORATTACKPATTERN();
                                            myattackpurposeforattackpattern.AttackPurposeID = myattackpurpose.AttackPurposeID;
                                            myattackpurposeforattackpattern.AttackPatternID = myattackpatternid;
                                            myattackpurposeforattackpattern.capec_id = capecidsearch;
                                            model.AddToATTACKPURPOSEFORATTACKPATTERN(myattackpurposeforattackpattern);
                                            model.SaveChanges();
                                        }

                                        break;
                                    default:
                                        Console.WriteLine("Missing code for capec:Purposes " + nodePurpose.Name);
                                        break;
                                }
                            }
                            #endregion CAPECPurposes
                            break;
                        case "capec:Technical_Context":
                            #region CAPECTechnical_Context
                            XORCISMModel.TECHNICALCONTEXT mytechnicalcontext;
                            mytechnicalcontext = model.TECHNICALCONTEXT.FirstOrDefault(o => o.capec_id == capecidsearch);
                            if (mytechnicalcontext == null)
                            {
                                Console.WriteLine(string.Format("Adding new TECHNICALCONTEXT [{0}] in table TECHNICALCONTEXT", mycapecid));
                                mytechnicalcontext = new TECHNICALCONTEXT();
                                mytechnicalcontext.capec_id = capecidsearch;
                                model.AddToTECHNICALCONTEXT(mytechnicalcontext);
                                model.SaveChanges();
                            }
                            int technicalcontextid = mytechnicalcontext.TechnicalContextID;
                            foreach (XmlNode nodeTechCon in nodeAP.ChildNodes)
                            {
                                switch (nodeTechCon.Name)
                                {
                                    case "capec:Architectural_Paradigms":
                                        foreach (XmlNode nodeTechCon2 in nodeTechCon.ChildNodes)
                                        {
                                            //TODO: test if nodeTechCon2.Name=="capec:Architectural_Paradigm"
                                            //TODO: nodeTechCon2.InnerText could be null
                                            XORCISMModel.ARCHITECTURALPARADIGM myarchiparadigm;
                                            myarchiparadigm = model.ARCHITECTURALPARADIGM.FirstOrDefault(o => o.ArchitecturalParadigmName == nodeTechCon2.InnerText);   //&& o.VocabularyID == 4
                                            if (myarchiparadigm == null)
                                            {
                                                Console.WriteLine(string.Format("Adding new ARCHITECTURALPARADIGM [{0}] in table ARCHITECTURALPARADIGM", mycapecid));
                                                myarchiparadigm = new ARCHITECTURALPARADIGM();
                                                myarchiparadigm.ArchitecturalParadigmName = nodeTechCon2.InnerText;
                                                myarchiparadigm.VocabularyID = 4;   //TODO  HARDCODED
                                                model.AddToARCHITECTURALPARADIGM(myarchiparadigm);
                                                model.SaveChanges();
                                            }
                                            XORCISMModel.ARCHITECTURALPARADIGMFORTECHNICALCONTEXT myarchiparadigmfortechcontext;
                                            myarchiparadigmfortechcontext = model.ARCHITECTURALPARADIGMFORTECHNICALCONTEXT.FirstOrDefault(o => o.ArchitecturalParadigmID == myarchiparadigm.ArchitecturalParadigmID && o.TechnicalContextID == technicalcontextid);
                                            if (myarchiparadigmfortechcontext == null)
                                            {
                                                Console.WriteLine(string.Format("Adding new ARCHITECTURALPARADIGMFORTECHNICALCONTEXT [{0}] in table ARCHITECTURALPARADIGMFORTECHNICALCONTEXT", mycapecid));
                                                myarchiparadigmfortechcontext = new ARCHITECTURALPARADIGMFORTECHNICALCONTEXT();
                                                myarchiparadigmfortechcontext.ArchitecturalParadigmID = myarchiparadigm.ArchitecturalParadigmID;
                                                myarchiparadigmfortechcontext.TechnicalContextID = technicalcontextid;
                                                model.AddToARCHITECTURALPARADIGMFORTECHNICALCONTEXT(myarchiparadigmfortechcontext);
                                                model.SaveChanges();
                                            }
                                        }
                                        break;
                                    case "capec:Frameworks":
                                        foreach (XmlNode nodeTechCon2 in nodeTechCon.ChildNodes)
                                        {
                                            //TODO: test if nodeTechCon2.Name=="capec:Framework"
                                            //TODO: nodeTechCon2.InnerText could be null
                                            XORCISMModel.FRAMEWORK myframework;
                                            myframework = model.FRAMEWORK.FirstOrDefault(o => o.FrameworkName == nodeTechCon2.InnerText);   //&& o.VocabularyID == 4
                                            if (myframework == null)
                                            {
                                                Console.WriteLine(string.Format("Adding new FRAMEWORK [{0}] in table FRAMEWORK", mycapecid));
                                                myframework = new FRAMEWORK();
                                                myframework.FrameworkName = nodeTechCon2.InnerText;
                                                myframework.VocabularyID = 4;   //TODO  HARDCODED
                                                model.AddToFRAMEWORK(myframework);
                                                model.SaveChanges();
                                            }
                                            XORCISMModel.FRAMEWORKFORTECHNICALCONTEXT myframeworkfortechcontext;
                                            myframeworkfortechcontext = model.FRAMEWORKFORTECHNICALCONTEXT.FirstOrDefault(o => o.FrameworkID == myframework.FrameworkID && o.TechnicalContextID == technicalcontextid);
                                            if (myframeworkfortechcontext == null)
                                            {
                                                Console.WriteLine(string.Format("Adding new FRAMEWORKFORTECHNICALCONTEXT [{0}] in table FRAMEWORKFORTECHNICALCONTEXT", mycapecid));
                                                myframeworkfortechcontext = new FRAMEWORKFORTECHNICALCONTEXT();
                                                myframeworkfortechcontext.FrameworkID = myframework.FrameworkID;
                                                myframeworkfortechcontext.TechnicalContextID = technicalcontextid;
                                                model.AddToFRAMEWORKFORTECHNICALCONTEXT(myframeworkfortechcontext);
                                                model.SaveChanges();
                                            }
                                        }
                                        break;
                                    case "capec:Platforms":
                                        foreach (XmlNode nodeTechCon2 in nodeTechCon.ChildNodes)
                                        {
                                            //TODO: test if nodeTechCon2.Name=="capec:Platform"
                                            //TODO: nodeTechCon2.InnerText could be null
                                            XORCISMModel.PLATFORM myplatform;
                                            myplatform = model.PLATFORM.FirstOrDefault(o => o.PlatformName == nodeTechCon2.InnerText);   //&& o.VocabularyID == 4
                                            if (myplatform == null)
                                            {
                                                Console.WriteLine(string.Format("Adding new PLATFORM [{0}] in table PLATFORM", mycapecid));
                                                myplatform = new PLATFORM();
                                                myplatform.PlatformName = nodeTechCon2.InnerText;
                                                myplatform.VocabularyID = 4;   //TODO  HARDCODED
                                                model.AddToPLATFORM(myplatform);
                                                model.SaveChanges();
                                            }
                                            XORCISMModel.PLATFORMFORTECHNICALCONTEXT myplatformfortechcontext;
                                            myplatformfortechcontext = model.PLATFORMFORTECHNICALCONTEXT.FirstOrDefault(o => o.PlatformID == myplatform.PlatformID && o.TechnicalContextID == technicalcontextid);
                                            if (myplatformfortechcontext == null)
                                            {
                                                Console.WriteLine(string.Format("Adding new PLATFORMFORTECHNICALCONTEXT [{0}] in table PLATFORMFORTECHNICALCONTEXT", mycapecid));
                                                myplatformfortechcontext = new PLATFORMFORTECHNICALCONTEXT();
                                                myplatformfortechcontext.PlatformID = myplatform.PlatformID;
                                                myplatformfortechcontext.TechnicalContextID = technicalcontextid;
                                                model.AddToPLATFORMFORTECHNICALCONTEXT(myplatformfortechcontext);
                                                model.SaveChanges();
                                            }
                                        }
                                        break;
                                    case "capec:Languages":
                                        foreach (XmlNode nodeTechCon2 in nodeTechCon.ChildNodes)
                                        {
                                            //TODO: test if nodeTechCon2.Name=="capec:Language"
                                            //TODO: nodeTechCon2.InnerText could be null
                                            XORCISMModel.LANGUAGE mylanguage;
                                            mylanguage = model.LANGUAGE.FirstOrDefault(o => o.LanguageName == nodeTechCon2.InnerText);   //&& o.VocabularyID == 4
                                            if (mylanguage == null)
                                            {
                                                Console.WriteLine(string.Format("Adding new LANGUAGE [{0}] in table LANGUAGE", mycapecid));
                                                mylanguage = new LANGUAGE();
                                                mylanguage.LanguageName = nodeTechCon2.InnerText;
                                                mylanguage.VocabularyID = 4;   //TODO  HARDCODED
                                                model.AddToLANGUAGE(mylanguage);
                                                model.SaveChanges();
                                            }
                                            XORCISMModel.LANGUAGEFORTECHNICALCONTEXT myplatformfortechcontext;
                                            myplatformfortechcontext = model.LANGUAGEFORTECHNICALCONTEXT.FirstOrDefault(o => o.LanguageID == mylanguage.LanguageID && o.TechnicalContextID == technicalcontextid);
                                            if (myplatformfortechcontext == null)
                                            {
                                                Console.WriteLine(string.Format("Adding new LANGUAGEFORTECHNICALCONTEXT [{0}] in table LANGUAGEFORTECHNICALCONTEXT", mycapecid));
                                                myplatformfortechcontext = new LANGUAGEFORTECHNICALCONTEXT();
                                                myplatformfortechcontext.LanguageID = mylanguage.LanguageID;
                                                myplatformfortechcontext.TechnicalContextID = technicalcontextid;
                                                model.AddToLANGUAGEFORTECHNICALCONTEXT(myplatformfortechcontext);
                                                model.SaveChanges();
                                            }
                                        }
                                        break;
                                    default:
                                        Console.WriteLine("Missing code for Technical_Context " + nodeTechCon.Name);
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
                                        mycweid = node3.InnerText;
                                    }
                                    //capec:Weakness_Relationship_Type
                                    if (node3.Name == "capec:Weakness_Relationship_Type")
                                    {
                                        myrelationship = node3.InnerText;
                                        //Console.WriteLine(mycweid);
                                        //Console.WriteLine(myrelationship);

                                        XORCISMModel.CWEFORCAPEC mycweforcapec;
                                        mycweforcapec = model.CWEFORCAPEC.FirstOrDefault(o => o.capec_id == "CAPEC-" + mycapecid && o.CWEID == "CWE-" + mycweid);
                                        if (mycweforcapec == null)
                                        {
                                            Console.WriteLine(string.Format("Adding new CWEFORCAPEC [{0}] in table CWEFORCAPEC", mycapecid));

                                            mycweforcapec = new CWEFORCAPEC();
                                            mycweforcapec.capec_id = "CAPEC-" + mycapecid;
                                            mycweforcapec.CWEID = "CWE-" + mycweid;
                                            mycweforcapec.WeaknessRelationship = myrelationship;
                                            model.AddToCWEFORCAPEC(mycweforcapec);

                                            model.SaveChanges();
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
                                XORCISMModel.ATTACKPAYLOAD mypayload;
                                mypayload = model.ATTACKPAYLOAD.FirstOrDefault(o => o.PayloadText == nodePayloadAtt.InnerText);   //&& o.VocabularyID == 4
                                if (mypayload == null)
                                {
                                    Console.WriteLine(string.Format("Adding new ATTACKPAYLOAD [{0}] in table ATTACKPAYLOAD", mycapecid));
                                    mypayload = new ATTACKPAYLOAD();
                                    mypayload.PayloadText = nodePayloadAtt.InnerText;
                                    mypayload.VocabularyID = 4;   //TODO  HARDCODED
                                    model.AddToATTACKPAYLOAD(mypayload);
                                    model.SaveChanges();
                                }
                                XORCISMModel.ATTACKPAYLOADFORATTACKPATTERN mypayloadforpattern;
                                mypayloadforpattern = model.ATTACKPAYLOADFORATTACKPATTERN.FirstOrDefault(o => o.AttackPayloadID == mypayload.AttackPayloadID && o.AttackPatternID == myattackpatternid);   //&& o.capec_id == capecidsearch
                                if (mypayloadforpattern == null)
                                {
                                    Console.WriteLine(string.Format("Adding new ATTACKPAYLOADFORATTACKPATTERN [{0}] in table ATTACKPAYLOADFORATTACKPATTERN", mycapecid));
                                    mypayloadforpattern = new ATTACKPAYLOADFORATTACKPATTERN();
                                    mypayloadforpattern.AttackPayloadID = mypayload.AttackPayloadID;
                                    mypayloadforpattern.AttackPatternID = myattackpatternid;
                                    mypayloadforpattern.capec_id = capecidsearch;
                                    model.AddToATTACKPAYLOADFORATTACKPATTERN(mypayloadforpattern);
                                    model.SaveChanges();
                                }
                            }
                            #endregion CAPECPayload
                            break;
                        case "capec:Typical_Likelihood_of_Exploit":
                            #region CAPECExploitLikelihood
                            EXPLOITLIKELIHOODFORATTACKPATTERN oCAPECLikelihood = new EXPLOITLIKELIHOODFORATTACKPATTERN();
                                
                            foreach (XmlNode nodeLikelihoodExploit in nodeAP.ChildNodes)
                            {
                                if (nodeLikelihoodExploit.Name == "capec:Likelihood")
                                {
                                    //Console.WriteLine("DEBUG TODO: code for AttackPattern " + nodeAP.Name);
                                    string sExploitLikelihood=nodeLikelihoodExploit.InnerText.Trim();
                                    EXPLOITLIKELIHOOD oExploitLikelihood = model.EXPLOITLIKELIHOOD.Where(o => o.Likelihood == sExploitLikelihood).FirstOrDefault();  //TODO: && o.VocabularyID==
                                    if (oExploitLikelihood != null)
                                    {

                                    }
                                    else
                                    {
                                        oExploitLikelihood = new EXPLOITLIKELIHOOD();
                                        oExploitLikelihood.Likelihood = sExploitLikelihood;
                                        oExploitLikelihood.VocabularyID = 4;   //TODO  HARDCODED
                                        try
                                        {
                                            model.AddToEXPLOITLIKELIHOOD(oExploitLikelihood);
                                            model.SaveChanges();
                                            Console.WriteLine("DEBUG AddToEXPLOITLIKELIHOOD " + sExploitLikelihood);
                                        }
                                        catch (Exception exAddToEXPLOITLIKELIHOOD)
                                        {
                                            Console.WriteLine("Exception exAddToEXPLOITLIKELIHOOD " + exAddToEXPLOITLIKELIHOOD.Message + " " + exAddToEXPLOITLIKELIHOOD.InnerException);
                                        }
                                    }

                                    oCAPECLikelihood = model.EXPLOITLIKELIHOODFORATTACKPATTERN.Where(o => o.ExploitLikelihoodID == oExploitLikelihood.ExploitLikelihoodID && o.capec_id == capecidsearch && o.AttackPatternID==myattackpatternid).FirstOrDefault();
                                    if (oCAPECLikelihood != null)
                                    {

                                    }
                                    else
                                    {
                                        oCAPECLikelihood.capec_id = capecidsearch;
                                        oCAPECLikelihood.ExploitLikelihoodID = oExploitLikelihood.ExploitLikelihoodID;
                                        oCAPECLikelihood.AttackPatternID = myattackpatternid;
                                        //TODO: timestamp...
                                        try
                                        {
                                            model.AddToEXPLOITLIKELIHOODFORATTACKPATTERN(oCAPECLikelihood);
                                            model.SaveChanges();
                                        }
                                        catch (Exception exAddToEXPLOITLIKELIHOODFORATTACKPATTERN)
                                        {
                                            Console.WriteLine("Exception exAddToEXPLOITLIKELIHOODFORATTACKPATTERN " + exAddToEXPLOITLIKELIHOODFORATTACKPATTERN.Message + " " + exAddToEXPLOITLIKELIHOODFORATTACKPATTERN.InnerException);
                                        }
                                    }

                                }
                                else
                                {
                                    //capec:Explanation
                                    Console.WriteLine("DEBUG TODO: code for AttackPattern Typical_Likelihood_of_Exploit " + nodeLikelihoodExploit.Name);
                                    //TODO: not clean
                                    //We assume that we will find only one <capec:Text>DEADBEEF</capec:Text>
                                    string sLikelihoodExplanation=nodeLikelihoodExploit.InnerText;
                                    sLikelihoodExplanation=sLikelihoodExplanation.Replace("<capec:Text>","");
                                    sLikelihoodExplanation=sLikelihoodExplanation.Replace("</capec:Text>","");
                                    //Cleaning
                                    //Remove CLRF
                                    sLikelihoodExplanation = sLikelihoodExplanation.Replace("\r\n", " ");
                                    sLikelihoodExplanation = sLikelihoodExplanation.Replace("\n", " ");
                                    while (sLikelihoodExplanation.Contains("  "))
                                    {
                                        sLikelihoodExplanation = sLikelihoodExplanation.Replace("  ", " ");
                                    }

                                    //oCAPECLikelihood.Explanation = sLikelihoodExplanation;  //TODO uncomment
                                    //TODO: timestamp
                                    try
                                    {
                                        model.SaveChanges();
                                    }
                                    catch (Exception exLikelihoodExplanation)
                                    {
                                        Console.WriteLine("Exception exLikelihoodExplanation " + exLikelihoodExplanation.Message + " " + exLikelihoodExplanation.InnerException);
                                    }
                                }
                            }
                            #endregion CAPECExploitLikelihood
                            break;
                        case "capec:Solutions_and_Mitigations":
                            foreach (XmlNode nodeSolutionMitigation in nodeAP.ChildNodes)
                            {
                                string sSolutionMitigationText = nodeSolutionMitigation.InnerText;
                                sSolutionMitigationText = sSolutionMitigationText.Replace("<capec:Text>", "");
                                sSolutionMitigationText = sSolutionMitigationText.Replace("</capec:Text>", "");
                                //Cleaning
                                //Remove CLRF
                                sSolutionMitigationText = sSolutionMitigationText.Replace("\r\n", " ");
                                sSolutionMitigationText = sSolutionMitigationText.Replace("\n", " ");
                                while (sSolutionMitigationText.Contains("  "))
                                {
                                    sSolutionMitigationText = sSolutionMitigationText.Replace("  ", " ");
                                }

                                MITIGATION oMitigation = model.MITIGATION.Where(o => o.SolutionMitigationText == sSolutionMitigationText).FirstOrDefault();
                                if (oMitigation != null)
                                {

                                }
                                else
                                {
                                    oMitigation.SolutionMitigationText = sSolutionMitigationText;
                                    oMitigation.VocabularyID = 4;   //TODO  HARDCODED
                                    //TODO: timestamp...
                                    try
                                    {
                                        model.AddToMITIGATION(oMitigation);
                                        model.SaveChanges();
                                        Console.WriteLine("DEBUG AddToMITIGATION " + sSolutionMitigationText);
                                    }
                                    catch (Exception exAddToMITIGATION)
                                    {
                                        Console.WriteLine("Exception exAddToMITIGATION " + exAddToMITIGATION.Message + " " + exAddToMITIGATION.InnerException);
                                    }
                                }

                                MITIGATIONFORATTACKPATTERN oCAPECMitigation = model.MITIGATIONFORATTACKPATTERN.Where(o => o.MitigationID == oMitigation.MitigationID && o.AttackPatternID == myattackpatternid && o.capec_id == capecidsearch).FirstOrDefault();  //TODO: && VocabularyID==
                                if (oCAPECMitigation != null)
                                {

                                }
                                else
                                {
                                    oCAPECMitigation.capec_id = capecidsearch;
                                    oCAPECMitigation.AttackPatternID = myattackpatternid;
                                    oCAPECMitigation.MitigationID = oMitigation.MitigationID;
                                    //TODO: timestamp...
                                    try
                                    {
                                        model.AddToMITIGATIONFORATTACKPATTERN(oCAPECMitigation);
                                        model.SaveChanges();
                                    }
                                    catch (Exception exAddToMITIGATIONFORATTACKPATTERN)
                                    {
                                        Console.WriteLine("Exception exAddToMITIGATIONFORATTACKPATTERN " + exAddToMITIGATIONFORATTACKPATTERN.Message + " " + exAddToMITIGATIONFORATTACKPATTERN.InnerException);
                                    }
                                }
                            }
                            break;

                        default:
                            Console.WriteLine("DEBUG TODO: code for AttackPattern "+nodeAP.Name);
                            break;
                    }


                }

            }
            #endregion AttackPattern

            #region Environment
            nodes1 = doc.SelectNodes("capec:Attack_Pattern_Catalog/capec:Environments/capec:Environment", mgr);
            foreach (XmlNode node in nodes1)    //capec:Environment
            {
                string sEnvID = node.Attributes["ID"].InnerText;

                XORCISMModel.ENVIRONMENT myenv;
                myenv = model.ENVIRONMENT.FirstOrDefault(o => o.CapecEnvironmentID == sEnvID);

                if (myenv == null)
                {
                    Console.WriteLine(string.Format("Adding new ENVIRONMENT [{0}] in table ENVIRONMENT", ""));

                    myenv = new ENVIRONMENT();
                    myenv.CapecEnvironmentID = sEnvID;
                    myenv.VocabularyID = 4; //TODO HARDCODED
                    foreach (XmlNode nodeEnvAtt in node)
                    {
                        switch (nodeEnvAtt.Name)
                        {
                            case "capec:Environment_Title":
                                myenv.EnvironmentTitle = nodeEnvAtt.InnerText;
                                break;
                            case "capec:Environment_Description":
                                myenv.EnvironmentDescription = nodeEnvAtt.InnerText;
                                break;
                            default:
                                Console.WriteLine("Missing code for capec:Environment " + nodeEnvAtt.Name);
                                break;
                        }
                    }

                    model.AddToENVIRONMENT(myenv);

                    model.SaveChanges();
                }
                else
                {
                    //TODO  Update
                }

            }

            #endregion Environment

            //FREE
            model.Dispose();
            model = null;


            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
        }
    }
}
