using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;
using System.Xml.Linq;

using System.Data.Entity;

using XORCISMModel;
using XATTACKModel;

namespace Microsoft_TMT
{
    class Program
    {
        /// <summary>
        /// Copyright (C) 2014-2015 Jerome Athias
        /// Completely Alpha version Tool to manipulate (old version) of Microsoft Threat Modeling Tool "threat categories database"
        /// This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation; either version 2 of the License, or (at your option) any later version.
        /// 
        /// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
        /// 
        /// You should have received a copy of the GNU General Public License along with this program; if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
        /// </summary>
        static void Main(string[] args)
        {
            //Microsoft Threat Modeling Tool (TMT) 2014
            XmlDocument doc = new XmlDocument();

            doc.Load(@"C:\Program Files (x86)\Microsoft Threat Modeling Tool 2014\KnowledgeBase\ThreatCategories.xml");    //Hardcoded

            XORCISMEntities model = new XORCISMEntities();
            //https://stackoverflow.com/questions/5940225/fastest-way-of-inserting-in-entity-framework
            model.Configuration.AutoDetectChangesEnabled = false;
            model.Configuration.ValidateOnSaveEnabled = false;

            XATTACKEntities attack_model = new XATTACKEntities();
            attack_model.Configuration.AutoDetectChangesEnabled = false;
            attack_model.Configuration.ValidateOnSaveEnabled = false;
            

            XmlNodeList nodesThreatCategories = doc.SelectNodes("/ArrayOfThreatCategory/ThreatCategory");   //Hardcoded
            foreach (XmlNode nodeThreatCategory in nodesThreatCategories)
            {
                //(no attributes)
                foreach (XmlNode nodeThreatCategoryInfo in nodeThreatCategory.ChildNodes)
                {
                    //Console.WriteLine("DEBUG: " + nodeThreatCategoryInfo.Name);
                    //Name  Id  ShortDescription    LongDescription
                    switch(nodeThreatCategoryInfo.Name)
                    {
                        case "Name":
                            //Search a match in Attack Pattern (CAPEC)
                            string sThreatCategoryNameValue=nodeThreatCategoryInfo.InnerText;
                            Console.WriteLine("DEBUG: " + sThreatCategoryNameValue);

                            //Spoofing  Tampering   Repudiation
                            try
                            {
                                ATTACKPATTERN oAttackPattern = attack_model.ATTACKPATTERN.FirstOrDefault(o => o.AttackPatternName.Contains(sThreatCategoryNameValue));
                                if (oAttackPattern != null)
                                {
                                    Console.WriteLine("DEBUG: " + oAttackPattern.capec_id + " " + oAttackPattern.AttackPatternName);
                                }
                            }
                            catch(Exception exoAttackPattern)
                            {
                                Console.WriteLine("Exception exoAttackPattern " + exoAttackPattern.Message + " " + exoAttackPattern.InnerException);
                            }
                            break;

                        case "Id":

                            break;

                        case "ShortDescription":

                            break;

                        case "LongDescription":

                            break;

                        default:
                            Console.WriteLine("ERROR: Missing code for " + nodeThreatCategoryInfo.Name);
                            break;
                    }

                    
                    Console.WriteLine("DEBUG: " + nodeThreatCategoryInfo.InnerText);

                }

            }

        }
    }
}
