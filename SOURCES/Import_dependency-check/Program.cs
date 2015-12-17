using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

using XORCISMModel;
using XVULNERABILITY;


using Excel = Microsoft.Office.Interop.Excel;   //Requirement
using System.IO;

namespace Import_dependency_check
{
    class Program
    {
        /// <summary>
        /// Copyright (C) 2015 Jerome Athias
        /// *** ALPHA VERSION ***
        /// Parser for OWASP dependency-check results from an XML file and import into an XORCISM database
        /// This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation; either version 2 of the License, or (at your option) any later version.
        /// 
        /// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
        /// 
        /// You should have received a copy of the GNU General Public License along with this program; if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
        /// </summary>
        /// 

        public static int iRowIndex = 1;
        public static Excel.Worksheet xlWorkSheet;

        static void Main(string[] args)
        {
            //https://www.owasp.org/index.php/OWASP_Dependency_Check

            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;

            object misValue = System.Reflection.Missing.Value;

            xlApp = new Excel.Application();
            xlWorkBook = xlApp.Workbooks.Add(misValue);

            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            int iColumnIndex = 1;

            XORCISMEntities model = new XORCISMEntities();
            //https://stackoverflow.com/questions/5940225/fastest-way-of-inserting-in-entity-framework
            model.Configuration.AutoDetectChangesEnabled = false;
            model.Configuration.ValidateOnSaveEnabled = false;

            int iVocabularyOWASPdepcheckID = 0; // 11;
            string sOWASPdepcheckVersion = "1.3.1";   //HARDCODED TODO
            #region vocabularyowaspdepcheck
            try
            {
                iVocabularyOWASPdepcheckID = model.VOCABULARY.Where(o => o.VocabularyName == "OWASP dependency-check" && o.VocabularyVersion == sOWASPdepcheckVersion).Select(o => o.VocabularyID).FirstOrDefault();
            }
            catch (Exception ex)
            {

            }
            if (iVocabularyOWASPdepcheckID <= 0)
            {
                try
                {
                    VOCABULARY oVocabulary = new VOCABULARY();
                    oVocabulary.CreatedDate = DateTimeOffset.Now;
                    oVocabulary.VocabularyName = "OWASP dependency-check";   //HARDCODED
                    oVocabulary.VocabularyVersion = sOWASPdepcheckVersion;
                    model.VOCABULARY.Add(oVocabulary);
                    model.SaveChanges();
                    iVocabularyOWASPdepcheckID = oVocabulary.VocabularyID;
                    Console.WriteLine("DEBUG iVocabularyOWASPdepcheckID=" + iVocabularyOWASPdepcheckID);
                }
                catch (Exception ex)
                {

                }
            }
            #endregion vocabularyowaspdepcheck


            XmlDocument doc;
            doc = new XmlDocument();
            
            doc.Load(@"dependency-check-report.xml");  //HARDCODED
            //TODO security: Validate XSD

            //xmlvalidator xsd=new xmlvalidator()


            //Global variables
            string sTemp = "";
            string sProjectName = "";
            string sReportDate = "";
            string sDependencyFileName = "";
            string sDependencyFilePath = "";
            string sDependencyMD5 = "";
            string sDependencySHA1 = "";


            XmlNodeList nodes1;
            nodes1 = doc.DocumentElement.SelectNodes("/");
            Console.WriteLine(nodes1.Count);
            #region parsexml
            foreach (XmlNode node in nodes1)
            {
                foreach (XmlNode node2 in node)
                {
                    //Console.WriteLine(node2.Name);
                    if(node2.Name == "analysis")
                    {
                        foreach (XmlNode node3 in node2)
                        {
                            try
                            {
                                sTemp = node3.Name.Trim();
                                Console.WriteLine("DEBUG "+sTemp);
                                //scanInfo
                                //projectInfo
                                //dependencies
                                switch (sTemp)
                                {
                                    case "scanInfo":
                                        //TODO
                                        //engineVersion
                                        //dataSource
                                        //name
                                        //timestamp
                                        break;
                                    case "projectInfo":
                                        //TODO
                                        //name
                                        //reportDate
                                        //credits
                                        try
                                        {
                                            //sProjectName = node3.SelectSingleNode("name").InnerText;
                                            foreach (XmlNode nodeprojectInfo in node3)
                                            {
                                                if(nodeprojectInfo.Name.Trim()=="name")
                                                {
                                                    sProjectName = nodeprojectInfo.InnerText;
                                                }
                                                else
                                                {
                                                    if (nodeprojectInfo.Name.Trim() == "reportDate")
                                                    {
                                                        sReportDate = nodeprojectInfo.InnerText;
                                                    }
                                                }
                                            }
                                            Console.WriteLine("DEBUG sProjectName=" + sProjectName);
                                            Console.WriteLine("DEBUG sReportDate=" + sReportDate);
                                        }
                                        catch(Exception exprojectInfo)
                                        {
                                            Console.WriteLine("Exception exprojectInfo: " + exprojectInfo.Message + " " + exprojectInfo.InnerException);
                                        }
                                        break;
                                    case "dependencies":
                                        foreach (XmlNode nodeDependency in node3)   //dependency
                                        {
                                            sDependencyFileName = "";
                                            sDependencyFilePath = "";
                                            sDependencyMD5 = "";
                                            sDependencySHA1 = "";
                                            //sDependencyFileName = nodeDependency.SelectSingleNode("fileName").InnerText;
                                            foreach (XmlNode nodeDependencyInfo in nodeDependency)
                                            {
                                                sTemp = nodeDependencyInfo.Name.Trim();
                                                switch (sTemp)
                                                {
                                                    case "fileName":
                                                        sDependencyFileName = nodeDependencyInfo.InnerText;
                                                        break;
                                                    case "filePath":
                                                        sDependencyFilePath = nodeDependencyInfo.InnerText;
                                                        break;
                                                    case "md5":
                                                        sDependencyMD5 = nodeDependencyInfo.InnerText;
                                                        break;
                                                    case "sha1":
                                                        sDependencySHA1 = nodeDependencyInfo.InnerText;
                                                        break;
                                                    default:
                                                        //TODO
                                                        Console.WriteLine("ERROR1 " + sTemp + " not managed.");
                                                        //relatedDependencies
                                                        break;
                                                }
                                                /*
                                                Console.WriteLine("DEBUG sDependencyFileName=" + sDependencyFileName);
                                                Console.WriteLine("DEBUG sDependencyFilePath=" + sDependencyFilePath);
                                                Console.WriteLine("DEBUG sDependencyMD5=" + sDependencyMD5);
                                                Console.WriteLine("DEBUG sDependencySHA1=" + sDependencySHA1);
                                                */
                                            }

                                            xlWorkSheet.Cells[iRowIndex, 1] = sDependencyFileName;
                                            xlWorkSheet.Cells[iRowIndex, 2] = sDependencyFilePath;
                                            xlWorkSheet.Cells[iRowIndex, 3] = sDependencyMD5;
                                            xlWorkSheet.Cells[iRowIndex, 4] = sDependencySHA1;
                                            iRowIndex++;
                                        }
                                        break;
                                    default:
                                        Console.WriteLine("ERROR2 " + node3.Name + " not managed.");
                                        //TODO
                                        //evidenceCollected
                                        //identifiers
                                        //vulnerabilities
                                        break;
                                }
                            }
                            catch(Exception exnode3Name)
                            {
                                Console.WriteLine("Exception exnode3Name: " + exnode3Name.Message + " " + exnode3Name.InnerException);
                            }
                        }
                    }
                }
            }
            #endregion parsexml

            //*********************************************************
            string sCurrentPath = Directory.GetCurrentDirectory();
            //HARDCODED
            xlWorkBook.SaveAs(sCurrentPath + @"\"+sProjectName+"-dependencies.xls", Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();

            releaseObject(xlWorkSheet);
            releaseObject(xlWorkBook);
            releaseObject(xlApp);

            Console.WriteLine(sProjectName + "-dependencies.xls created.");
        }

        public static void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                //MessageBox.Show("Exception Occured while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }

    }
}
