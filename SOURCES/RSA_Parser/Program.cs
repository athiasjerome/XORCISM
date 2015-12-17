using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;
using System.Xml.Linq;

using Excel = Microsoft.Office.Interop.Excel;
using System.IO;

namespace RSA_Parser
{
    class Program
    {
        /// <summary>
        /// Copyright (C) 2015 Jerome Athias
        /// Do "something" with "RSA Secure Analytics" inputs
        /// This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation; either version 2 of the License, or (at your option) any later version.
        /// 
        /// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
        /// 
        /// You should have received a copy of the GNU General Public License along with this program; if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
        /// </summary>
        public static int iRowIndex = 1;
        public static Excel.Worksheet xlWorkSheet;

        static void Main(string[] args)
        {

            String[] fichiers = Directory.GetFiles(@"X:\XORCISM\SOURCES\RSA_Parser\bin\Release\ALL", "*.xml", SearchOption.AllDirectories);   //TODO Hardcoded

            for (int i = 0; i < fichiers.Length; i++)
            {
                iRowIndex = 1;
                XmlDocument doc = new XmlDocument();
                //string sFileName = "index-concentrator-custom";//Hardcoded
                //string sFileName = "table-map";//Hardcoded
                //string sFileName = "apachecustommsg";//Hardcoded
                string sFileName = fichiers[i];
                Console.WriteLine("DEBUG sFileName="+fichiers[i]);
                doc.Load(sFileName);    // + ".xml");


                Excel.Application xlApp;
                Excel.Workbook xlWorkBook;

                object misValue = System.Reflection.Missing.Value;

                xlApp = new Excel.Application();
                xlWorkBook = xlApp.Workbooks.Add(misValue);

                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);


                #region parser
                //XmlNodeList nodeRSAkeys = doc.SelectNodes("/DEVICEMESSAGES/HEADER");   //Hardcoded
                XmlNodeList nodeRSAkeys = doc.SelectNodes("/DEVICEMESSAGES/MESSAGE");   //Hardcoded

                xlWorkSheet.Cells[1, 1] = "id1";
                xlWorkSheet.Cells[1, 2] = "id2";
                xlWorkSheet.Cells[1, 3] = "content";
                /*
                level="1"
		        parse="1"
		        parsedefvalue="1"
		        tableid="89"
		        id1="httpd_elise_ssl_request_log"
		        id2="httpd_elise_ssl_request_log"
		        eventcategory="1200000000"
                */

                //string RegexPattern = "&lt;(.*?)&gt;";
                string RegexPattern = "<(.*?)>";

                foreach (XmlNode nodeRSAkey in nodeRSAkeys)
                {
                    iRowIndex++;
                    int iColumnIndex = 3;
                    try
                    {
                        xlWorkSheet.Cells[iRowIndex, 1] = nodeRSAkey.Attributes["id1"].InnerText;
                    }
                    catch (Exception ex)
                    {

                    }
                    try
                    {
                        xlWorkSheet.Cells[iRowIndex, 2] = nodeRSAkey.Attributes["id2"].InnerText;
                    }
                    catch (Exception ex)
                    {

                    }


                    try
                    {
                        //Extract the metadatas
                        // Find matches.
                        System.Text.RegularExpressions.MatchCollection matches
                            = System.Text.RegularExpressions.Regex.Matches(nodeRSAkey.Attributes["content"].InnerText, RegexPattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                        string[] MatchList = new string[matches.Count];
                        //Console.WriteLine("DEBUG matches.Count=" + matches.Count);

                        // Report on each match.
                        //int c = 0;
                        foreach (System.Text.RegularExpressions.Match match in matches)
                        {
                            //MatchList[c] = match.Groups["meta"].Value;
                            //Console.WriteLine("DEBUG " + match.Value);
                            Console.WriteLine("DEBUG iRowIndex=" + iRowIndex);
                            xlWorkSheet.Cells[iRowIndex, iColumnIndex] = match.Value.Replace("<", "").Replace(">", "");
                            iColumnIndex++;
                            //c++;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Exception " + ex.Message + " " + ex.InnerException);
                    }
                }
            
            #endregion parser


            #region mapping
            /*
            XmlNodeList nodeRSAkeys = doc.SelectNodes("/mappings/mapping");   //Hardcoded
            xlWorkSheet.Cells[1, 1] = "envisionName";
            xlWorkSheet.Cells[1, 2] = "nwName";
            xlWorkSheet.Cells[1, 3] = "flags";
            xlWorkSheet.Cells[1, 4] = "envisionDisplayName";
            xlWorkSheet.Cells[1, 5] = "failureKey";
            xlWorkSheet.Cells[1, 6] = "nullTokens";
            xlWorkSheet.Cells[1, 7] = "format";

            foreach (XmlNode nodeRSAkey in nodeRSAkeys)
            {
                iRowIndex++;


                Console.WriteLine("DEBUG: " + nodeRSAkey.Attributes["envisionName"].InnerText);
                xlWorkSheet.Cells[iRowIndex, 1] = nodeRSAkey.Attributes["envisionName"].InnerText;

                xlWorkSheet.Cells[iRowIndex, 2] = nodeRSAkey.Attributes["nwName"].InnerText;
                xlWorkSheet.Cells[iRowIndex, 3] = nodeRSAkey.Attributes["flags"].InnerText;
                try
                {
                    xlWorkSheet.Cells[iRowIndex, 4] = nodeRSAkey.Attributes["envisionDisplayName"].InnerText;
                }
                catch (Exception ex)
                {

                }
                try
                {
                    xlWorkSheet.Cells[iRowIndex, 5] = nodeRSAkey.Attributes["failureKey"].InnerText;
                }
                catch (Exception ex)
                {

                }
                try
                {
                    xlWorkSheet.Cells[iRowIndex, 6] = nodeRSAkey.Attributes["nullTokens"].InnerText;
                }
                catch (Exception ex)
                {

                }
                try
                {
                    xlWorkSheet.Cells[iRowIndex, 7] = nodeRSAkey.Attributes["format"].InnerText;
                }
                catch (Exception ex)
                {

                }

            }
            */
            #endregion mapping

            #region index
            /*
            XmlNodeList nodeRSAkeys = doc.SelectNodes("/language/key");   //Hardcoded
            xlWorkSheet.Cells[1, 1] = "description";
            xlWorkSheet.Cells[1, 2] = "format";
            xlWorkSheet.Cells[1, 3] = "level";
            xlWorkSheet.Cells[1, 4] = "name";
            xlWorkSheet.Cells[1, 5] = "valueMax";
            xlWorkSheet.Cells[1, 6] = "defaultAction";

            foreach (XmlNode nodeRSAkey in nodeRSAkeys)
            {
                iRowIndex++;


                Console.WriteLine("DEBUG: " + nodeRSAkey.Attributes["description"].InnerText);
                xlWorkSheet.Cells[iRowIndex, 1] = nodeRSAkey.Attributes["description"].InnerText;
                
                xlWorkSheet.Cells[iRowIndex, 2] = nodeRSAkey.Attributes["format"].InnerText;
                xlWorkSheet.Cells[iRowIndex, 3] = nodeRSAkey.Attributes["level"].InnerText;
                xlWorkSheet.Cells[iRowIndex, 4] = nodeRSAkey.Attributes["name"].InnerText;
                try
                {
                    xlWorkSheet.Cells[iRowIndex, 5] = nodeRSAkey.Attributes["valueMax"].InnerText;
                }
                catch (Exception ex)
                {

                }
                try
                {
                    xlWorkSheet.Cells[iRowIndex, 6] = nodeRSAkey.Attributes["defaultAction"].InnerText;
                }
                catch(Exception ex)
                {

                }


            }
            */
            #endregion index

            //*********************************************************
            string sCurrentPath = Directory.GetCurrentDirectory();
            //xlWorkBook.SaveAs(sCurrentPath + @"\" + sFileName +".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlWorkBook.SaveAs(sCurrentPath + @"\" + Path.GetFileName(sFileName) + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();

            releaseObject(xlWorkSheet);
            releaseObject(xlWorkBook);
            releaseObject(xlApp);
            }
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
