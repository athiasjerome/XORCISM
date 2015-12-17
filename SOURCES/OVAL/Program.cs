using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XORCISMModel;
using XOVALModel;

namespace OVAL
{
    class Program
    {
        /// <summary>
        /// Copyright (C) 2014-2015 Jerome Athias
        /// Demo/Debug tool to retrieve information regarding an OVAL definition from an XORCISM database
        /// This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation; either version 2 of the License, or (at your option) any later version.
        /// 
        /// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
        /// 
        /// You should have received a copy of the GNU General Public License along with this program; if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
        /// </summary>
        public static XORCISMEntities model = new XORCISMEntities();
        public static XOVALEntities oval_model = new XOVALEntities();

        static void Main(string[] args)
        {
            oval_model.Configuration.AutoDetectChangesEnabled = false;
            oval_model.Configuration.ValidateOnSaveEnabled = false;

            //string sOVALDefinitionIDPattern="oval:org.mitre.oval:def:9999"; //HARDCODED
            string sOVALDefinitionIDPattern = "oval:org.mitre.oval:def:7878";

            //string sVersion="5";

            OVALDEFINITION oOVALDefinition = oval_model.OVALDEFINITION.Where(o => o.OVALDefinitionIDPattern == sOVALDefinitionIDPattern).OrderByDescending(o => o.OVALDefinitionVersion).FirstOrDefault();
            if(oOVALDefinition==null)
            {
                Console.WriteLine("DEBUG " + DateTimeOffset.Now);
                Console.WriteLine("ERROR OVALDEFINITION not found");
                return;
            }
            else
            {
                Console.WriteLine("Definition Id: "+oOVALDefinition.OVALDefinitionIDPattern);
                Console.WriteLine("Version: "+oOVALDefinition.OVALDefinitionVersion);
                //Last Modified
                Console.WriteLine("Title: "+oOVALDefinition.OVALDefinitionTitle);
                Console.WriteLine("Description: "+oOVALDefinition.OVALDefinitionDescription);
                int iOVALDefinitionFamilyID = oval_model.OVALDEFINITIONFAMILY.FirstOrDefault(o => o.OVALDefinitionID == oOVALDefinition.OVALDefinitionID).OVALDefinitionFamilyID;
                int iOSFamilyID=(int)oval_model.OVALDEFINITIONFAMILY.FirstOrDefault(o => o.OVALDefinitionFamilyID == iOVALDefinitionFamilyID).OSFamilyID;

                //Console.WriteLine("Family: "+oOVALDefinition.OVALDEFINITIONFAMILY.FirstOrDefault().OSFAMILY.FamilyName);
                Console.WriteLine("Family: " + model.OSFAMILY.FirstOrDefault(o => o.OSFamilyID == iOSFamilyID).FamilyName);

                Console.WriteLine("Class: "+oOVALDefinition.OVALCLASSENUMERATION.ClassValue);
                Console.WriteLine("Status: "+oOVALDefinition.StatusName);
                //References:

                //TODO
                //foreach(OVALDEFINITIONVULNERABILITY oOVALDefVuln in oOVALDefinition.OVALDEFINITIONVULNERABILITY)

                Console.WriteLine("Platform(s): ");
                foreach (OVALDEFINITIONPLATFORM oOVALDefPlatform in oOVALDefinition.OVALDEFINITIONPLATFORM)
                {
                    //Console.WriteLine(oOVALDefPlatform.PLATFORM.PlatformName);
                    //Search the PlatformName
                    try
                    {
                        PLATFORM oPlatform = model.PLATFORM.Where(o => o.PlatformID == oOVALDefPlatform.PlatformID).FirstOrDefault();
                        if (oPlatform != null)
                        {
                            Console.WriteLine("DEBUG PlatformName=" + oPlatform.PlatformName);
                        }
                    }
                    catch (Exception exPlatformName)
                    {

                    }
                }

                Console.WriteLine("Product(s): ");
                foreach(OVALDEFINITIONCPE oOVALDefCPE in oOVALDefinition.OVALDEFINITIONCPE)
                {
                    //Console.WriteLine(oOVALDefCPE.CPE.CPEName);
                    try
                    {
                        string sCPEName = model.CPE.FirstOrDefault(o => o.CPEID == oOVALDefCPE.CPEID).CPEName;
                        Console.WriteLine(sCPEName);
                    }
                    catch(Exception exCPEName)
                    {
                        Console.WriteLine("Exceptiion exCPEName " + exCPEName.Message + " " + exCPEName.InnerException);
                    }
                }

                

                Console.WriteLine("Definition Synopsis: ");
                //Console.WriteLine(oOVALDefinition.OVALCRITERIA.OPERATORENUMERATION.OperatorValue+" "+oOVALDefinition.OVALCRITERIA.comment);
                //Console.WriteLine(oOVALDefinition.OVALCRITERIA.OPERATORENUMERATION.OperatorValue);

                fDisplayCriteria(oOVALDefinition.OVALCriteriaID.Value);

            }

        }

        public static void fDisplayCriteria(int iOVALCriteriaID, string sTab="")
        {
            string sTab2 = sTab;
            IEnumerable<OVALCRITERIAFOROVALCRITERIA> OVALDefinitionCriterias = oval_model.OVALCRITERIAFOROVALCRITERIA.Where(o => o.OVALCriteriaRefID == iOVALCriteriaID);
            int iCptCriteria = 0;
            foreach (OVALCRITERIAFOROVALCRITERIA oOVALCriteriaCriteria in OVALDefinitionCriterias)
            {
                sTab2 = sTab;
                iCptCriteria++;
                if (iCptCriteria == 1)
                {
                    Console.WriteLine(sTab + oOVALCriteriaCriteria.OVALCRITERIA1.comment);
                }
                else
                {
                    Console.WriteLine(sTab + oOVALCriteriaCriteria.OVALCRITERIA.OPERATORENUMERATION.OperatorValue + " " + oOVALCriteriaCriteria.OVALCRITERIA1.comment);
                }

                sTab2 += "  ";
                int iCptCriteriaDefinition = 0;
                foreach (OVALCRITERIAEXTENDDEFINITION oOVALCriteriaDefinition in oOVALCriteriaCriteria.OVALCRITERIA1.OVALCRITERIAEXTENDDEFINITION)
                {
                    iCptCriteriaDefinition++;
                    if (iCptCriteriaDefinition == 1)
                    {
                        Console.WriteLine(sTab + oOVALCriteriaDefinition.OVALDEFINITION.OVALDefinitionTitle);
                    }
                    else
                    {
                        Console.WriteLine(sTab + oOVALCriteriaDefinition.OVALCRITERIA.OPERATORENUMERATION.OperatorValue + " " + oOVALCriteriaDefinition.OVALDEFINITION.OVALDefinitionTitle);
                    }
                }

                int iCptCriterion = 0;
                foreach (OVALCRITERIACRITERION oOVALCriteriaCriterion in oOVALCriteriaCriteria.OVALCRITERIA1.OVALCRITERIACRITERION)
                {
                    iCptCriterion++;
                    if (iCptCriterion == 1)
                    {
                        Console.WriteLine(sTab + oOVALCriteriaCriterion.OVALTEST.comment);
                        //Console.WriteLine(sTab + oOVALCriteriaCriterion.OVALTEST.OVALTestIDPattern);
                    }
                    else
                    {
                        Console.WriteLine(sTab + oOVALCriteriaCriterion.OVALCRITERIA.OPERATORENUMERATION.OperatorValue + " " + oOVALCriteriaCriterion.OVALTEST.comment);
                    }
                }


                fDisplayCriteria(oOVALCriteriaCriteria.OVALCRITERIA1.OVALCriteriaID, sTab2);
            }
        }

    }
}
