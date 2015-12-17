using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XORCISMModel;
using XOVALModel;
using XVULNERABILITYModel;

namespace CVE2OVAL
{
    class Program
    {
        /// <summary>
        /// Copyright (C) 2014-2015 Jerome Athias
        /// TEST/DEBUG ONLY tool to play with an XORCISM database (check the proper import and relationships creation between CVE and OVAL)
        /// This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation; either version 2 of the License, or (at your option) any later version.
        /// 
        /// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
        /// 
        /// You should have received a copy of the GNU General Public License along with this program; if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
        /// </summary>
        static void Main(string[] args)
        {
            XORCISMEntities model = new XORCISMEntities();
            XOVALEntities oval_model = new XOVALEntities();
            XVULNERABILITYEntities vuln_model = new XVULNERABILITYEntities();


            string sCVEID = "CVE-2014-3802";    //HARDCODED
            VULNERABILITY oVulnerability = null;
            try
            {
                oVulnerability = vuln_model.VULNERABILITY.Where(o => o.VULReferentialID == sCVEID).FirstOrDefault();
            }
            catch(Exception ex)
            {

            }
            if (oVulnerability!=null)
            {
                //Check if we have an OVALDEFINITION for the VULNERABILITY
                int iOVALDEFINITIONVULNERABILITYID = 0;
                try
                {
                    iOVALDEFINITIONVULNERABILITYID = oval_model.OVALDEFINITIONVULNERABILITY.Where(o => o.VulnerabilityID == oVulnerability.VulnerabilityID).Select(o => o.OVALDefinitionVulnerabilityID).FirstOrDefault();
                }
                catch(Exception ex)
                {

                }
                if(iOVALDEFINITIONVULNERABILITYID>0)
                {
                    Console.WriteLine("DEBUG: We already have a definition");
                }
                else
                {
                    //Search a Product in the Vulnerability's Definition
                    foreach(PRODUCT oProduct in model.PRODUCT)
                    {
                        if(oVulnerability.VULDescription.ToLower().Contains(oProduct.ProductName.ToLower()))
                        {
                            Console.WriteLine("DEBUG: Potential Product: " + oProduct.ProductName);
                            //Platform

                            //CPE

                        }
                    }

                    //Search a Filename in the Vulnerability's Definition
                    foreach (FILE oFile in model.FILE)
                    {
                        if (oVulnerability.VULDescription.ToLower().Contains(oFile.FileName.ToLower()))
                        {
                            Console.WriteLine("DEBUG: Potential File: " + oFile.FileName);
                            
                        }
                    }
                    //regex .dll


                }

            }
            else
            {
                Console.WriteLine("ERROR: Vulnerability not found");
            }
        }
    }
}
