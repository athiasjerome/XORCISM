using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XORCISMModel;
using XOVALModel;
using XVULNERABILITYModel;

namespace SearchCPEforOVALDefinition
{
    class Program
    {
        /// <summary>
        /// Copyright (C) 2014-2015 Jerome Athias
        /// Unfinished tool to retrieve OVAL Definitions corresponding to a CPE an XORCISM database
        /// This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation; either version 2 of the License, or (at your option) any later version.
        /// 
        /// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
        /// 
        /// You should have received a copy of the GNU General Public License along with this program; if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
        /// </summary>
        static void Main(string[] args)
        {
            //TODO FIX MODELS

            //Search the CPE fo OVALDEFINITIONs using the CPE list collected from CVE NVD
            XORCISMEntities model = new XORCISMEntities();
            model.Configuration.AutoDetectChangesEnabled = false;
            model.Configuration.ValidateOnSaveEnabled = false;

            XOVALEntities oval_model = new XOVALEntities();
            oval_model.Configuration.AutoDetectChangesEnabled = false;
            oval_model.Configuration.ValidateOnSaveEnabled = false;

            XVULNERABILITYEntities vuln_model = new XVULNERABILITYEntities();
            vuln_model.Configuration.AutoDetectChangesEnabled = false;
            vuln_model.Configuration.ValidateOnSaveEnabled = false;


            List<OVALDEFINITIONVULNERABILITY> ListOVALDefVulns = oval_model.OVALDEFINITIONVULNERABILITY.ToList();

            foreach (OVALDEFINITIONVULNERABILITY oOVALDefVuln in ListOVALDefVulns)
            {
                Console.WriteLine("DEBUG ************************************************************");
                Console.WriteLine("DEBUG " + oOVALDefVuln.OVALDEFINITION.OVALDefinitionIDPattern);
                int iVulnerabilityID = (int)oOVALDefVuln.VulnerabilityID;
                string sVULReferentialID = vuln_model.VULNERABILITY.FirstOrDefault(o => o.VulnerabilityID == oOVALDefVuln.VulnerabilityID).VULReferentialID;

                //Console.WriteLine("DEBUG " + oOVALDefVuln.VULNERABILITY.VULReferentialID);
                Console.WriteLine("DEBUG " + sVULReferentialID);
                //List<VULNERABILITYFORCPE> ListVulnCPEs = vuln_model.VULNERABILITYFORCPE.Where(o => o.VulnerabilityID == oOVALDefVuln.VULNERABILITY.VulnerabilityID).ToList();
                List<VULNERABILITYFORCPE> ListVulnCPEs = vuln_model.VULNERABILITYFORCPE.Where(o => o.VulnerabilityID == iVulnerabilityID).ToList();

                foreach(VULNERABILITYFORCPE oVulnCPE in ListVulnCPEs)
                {
                    //Console.WriteLine("DEBUG " + oVulnCPE.CPE.CPEName);
                    string sCPEName = model.CPE.FirstOrDefault(o => o.CPEID == oVulnCPE.CPEID).CPEName;
                    Console.WriteLine("DEBUG " + sCPEName);
                }
            }


            model.Dispose();

        }
    }
}
