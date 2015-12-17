using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XORCISMModel;

using Microsoft.Office.Core;
using Microsoft.Office.Interop.Excel;

namespace Import_CyberSecurityFramework
{
    class Program
    {
        /// <summary>
        /// Copyright (C) 2015 Jerome Athias
        /// *** ALPHA VERSION ***
        /// Import the Cybersecurity Framework Excel file in an XORCISM database
        /// All trademarks and registered trademarks are the property of their respective owners.
        /// This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation; either version 2 of the License, or (at your option) any later version.
        /// 
        /// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
        /// 
        /// You should have received a copy of the GNU General Public License along with this program; if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
        /// </summary>
        
        static void Main(string[] args)
        {
            //Ref.: http://www.nist.gov/cyberframework/upload/framework-for-improving-critical-infrastructure-cybersecurity-core.xlsx

            
            XORCISMEntities model = new XORCISMEntities();
            //https://stackoverflow.com/questions/5940225/fastest-way-of-inserting-in-entity-framework
            model.Configuration.AutoDetectChangesEnabled = false;
            model.Configuration.ValidateOnSaveEnabled = false;

            int iVocabularyCSFID = 0;   // 7;

            #region vocabularycsf
            try
            {
                iVocabularyCSFID = model.VOCABULARY.Where(o => o.VocabularyName == "CCE").Select(o => o.VocabularyID).FirstOrDefault();
            }
            catch (Exception ex)
            {

            }
            if (iVocabularyCSFID <= 0)
            {
                VOCABULARY oVocabulary = new VOCABULARY();
                oVocabulary.CreatedDate = DateTimeOffset.Now;
                oVocabulary.VocabularyName = "Cybersecurity Framework";
                oVocabulary.timestamp = DateTimeOffset.Now;
                model.VOCABULARY.Add(oVocabulary);
                model.SaveChanges();
                iVocabularyCSFID = oVocabulary.VocabularyID;
                Console.WriteLine("DEBUG iVocabularyCSFID=" + iVocabularyCSFID);
            }
            #endregion vocabularcsf

            var ExcelObj = new Microsoft.Office.Interop.Excel.Application();
            //HARDCODED
            Workbook theWorkbook = ExcelObj.Workbooks.Open(@"C:\nvdcve\framework-for-improving-critical-infrastructure-cybersecurity-core.xlsx", 0, true, 5, "", "", true, XlPlatform.xlWindows, "\t", false, false, 0, true);
            Sheets sheets = theWorkbook.Worksheets;
            Worksheet worksheet = (Worksheet)sheets.get_Item(1);
            for (int i = 1; i <= 10; i++)
            {
                Range range = worksheet.get_Range("A" + i.ToString(), "J" + i.ToString());
                System.Array myvalues = (System.Array)range.Cells.Value;
                string[] strArray = ConvertToStringArray(myvalues);
                int iCol = 0;
                

                foreach (string sValue in strArray)
                {
                    iCol++;
                    
                    switch (iCol.ToString())
                    {
                        case "1":   //CCE ID
                            
                            break;
                        case "2":   //CCE Description

                            break;
                        default:
                            break;
                    }


                    Console.WriteLine(sValue);
                }
                Console.WriteLine("---------------------------------");
            }

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
