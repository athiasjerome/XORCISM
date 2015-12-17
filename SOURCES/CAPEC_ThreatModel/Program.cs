using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XORCISMModel;
using XATTACKModel;

using Excel = Microsoft.Office.Interop.Excel;   //Requirement
using System.IO;

namespace CAPEC_ThreatModel
{
    class Program
    {
        /// <summary>
        /// Copyright (C) 2014-2015 Jerome Athias
        /// Tool for generating generic Threat Models (in Excel format) from MITRE CAPEC library, after import in XORCISM database
        /// All trademarks and registered trademarks are the property of their respective owners.
        /// This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation; either version 2 of the License, or (at your option) any later version.
        /// 
        /// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
        /// 
        /// You should have received a copy of the GNU General Public License along with this program; if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
        /// </summary>
        
        public static int iRowIndex = 1;
        public static Excel.Worksheet xlWorkSheet;
        public static bool bIncludeCWEs = false;  //HARDCODED
        public static XORCISMEntities model = new XORCISMEntities();

        static void Main(string[] args)
        {
            
            XATTACKEntities attack_model = new XATTACKEntities();

            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            
            object misValue = System.Reflection.Missing.Value;

            xlApp = new Excel.Application();
            xlWorkBook = xlApp.Workbooks.Add(misValue);

            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            int iColumnIndex = 1;
            
            //xlWorkSheet.Cells[1, 1] = "http://csharp.net-informations.com";
            
            //Notes
            //https://capec.mitre.org/data/graphs/3000.html
            //Start with one Domain of Attack
            //CAPEC-513 Software
            //int iAttackPatternID = 0;
            //ATTACKPATTERN oAttackPatternMaster = null;
            List<ATTACKPATTERN> ListAttackPatterns = new List<ATTACKPATTERN>();
            try
            {
                //TODO HARDCODED
                ////iAttackPatternID = attack_model.ATTACKPATTERN.Where(o => o.AttackPatternName == "Software").Select(o => o.AttackPatternID).FirstOrDefault();
                //oAttackPatternMaster = attack_model.ATTACKPATTERN.Where(o => o.AttackPatternName == "Software").FirstOrDefault();
                //oAttackPatternMaster = attack_model.ATTACKPATTERN.Where(o => o.AttackPatternName == "Supply Chain").FirstOrDefault();
                //oAttackPatternMaster = attack_model.ATTACKPATTERN.Where(o => o.AttackPatternName == "Hardware").FirstOrDefault();
                //oAttackPatternMaster = attack_model.ATTACKPATTERN.Where(o => o.AttackPatternName == "Injection").FirstOrDefault();
                //oAttackPatternMaster = attack_model.ATTACKPATTERN.Where(o => o.AttackPatternName == "Physical Security").FirstOrDefault();
                //oAttackPatternMaster = attack_model.ATTACKPATTERN.Where(o => o.AttackPatternName == "Social Engineering").FirstOrDefault();
                //oAttackPatternMaster = attack_model.ATTACKPATTERN.Where(o => o.AttackPatternName == "Phishing").FirstOrDefault();
                //ListAttackPatterns = attack_model.ATTACKPATTERN.Where(o => o.AttackPatternName.Contains("mail")).ToList();  //.FirstOrDefault();
                //ListAttackPatterns = attack_model.ATTACKPATTERN.Where(o => o.AttackPatternName == "Brute Force").ToList();  //.FirstOrDefault();
                //ListAttackPatterns = attack_model.ATTACKPATTERN.Where(o => o.AttackPatternName.Contains("DNS") || o.AttackPatternDescription.Contains("DNS")).ToList();  //.FirstOrDefault();
                //ListAttackPatterns = attack_model.ATTACKPATTERN.Where(o => o.AttackPatternName.Contains("mail") || o.AttackPatternDescription.Contains("mail")).ToList();  //.FirstOrDefault();
                //ListAttackPatterns = attack_model.ATTACKPATTERN.Where(o => o.AttackPatternName == "Physical Security").ToList();
                //ListAttackPatterns = attack_model.ATTACKPATTERN.Where(o => o.AttackPatternName == "Social Information Gathering Attacks").ToList();
                ListAttackPatterns = attack_model.ATTACKPATTERN.Where(o => o.AttackPatternName.Contains("Spoofing")).ToList();

            }
            catch(Exception ex)
            {

            }

            foreach (ATTACKPATTERN oAttackPatternMaster in ListAttackPatterns)
            {
                xlWorkSheet.Cells[iRowIndex, 1] = oAttackPatternMaster.capec_id + " " + oAttackPatternMaster.AttackPatternName;  //"CAPEC-513 Software";

                iColumnIndex = 2;

                fWriteChilds(oAttackPatternMaster, iColumnIndex);
                //iRowIndex++;
            }

            //*********************************************************
            string sCurrentPath = Directory.GetCurrentDirectory();
            //HARDCODED
            xlWorkBook.SaveAs(sCurrentPath+@"\CAPEC_ThreatModel.xls", Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();

            releaseObject(xlWorkSheet);
            releaseObject(xlWorkBook);
            releaseObject(xlApp);

        }

        public static void fWriteChilds(ATTACKPATTERN oAttackPatternMaster, int iColumnIndex)
        {
            int iRowIndexTemp = iRowIndex;  //Save this value to make sure that we increment it at least once
            Console.WriteLine("DEBUG in fWriteChilds iColumnIndex=" + iColumnIndex + " " + oAttackPatternMaster.AttackPatternID + " " + oAttackPatternMaster.AttackPatternName);

            /*
            CanAlsoBe
            CanFollow
            CanPrecede
            ChildOf
            HasMember
            Leverage
            ParentOf
            PeerOf
            */

            //If we want the parents of the oAttackPatternMaster
            //To the left
            //A ParentOf X => write A
            #region AttackPatternMasterParent
            foreach (ATTACKPATTERNRELATIONSHIP oAttackPatternRelation in oAttackPatternMaster.ATTACKPATTERNRELATIONSHIP1.Where(o => o.AttackPatternSubjectID == oAttackPatternMaster.AttackPatternID && (o.RelationshipName == "ParentOf" || o.RelationshipName == "HasMember" || o.RelationshipName == "CanPrecede")))
            {
                Console.WriteLine("DEBUG01 " + oAttackPatternRelation.ATTACKPATTERN.capec_id + " (" + oAttackPatternRelation.ATTACKPATTERN.AttackPatternID + ") " + oAttackPatternRelation.RelationshipName + " " + oAttackPatternMaster.capec_id + " (" + oAttackPatternMaster.AttackPatternID + ")");
                xlWorkSheet.Cells[iRowIndex, iColumnIndex] = oAttackPatternRelation.ATTACKPATTERN.capec_id + " " + oAttackPatternRelation.ATTACKPATTERN.AttackPatternName;
                //fWriteChilds(oAttackPatternRelation.ATTACKPATTERN, iColumnIndex + 1);   //TODO Review (-1?)
                iRowIndex++;
                if (bIncludeCWEs)
                {
                    //List the CWEs
                    foreach (ATTACKPATTERNCWE oAttackPatternCWE in oAttackPatternRelation.ATTACKPATTERN.ATTACKPATTERNCWE)
                    {
                        Console.WriteLine("DEBUG AttackPatternCWEID=" + oAttackPatternCWE.AttackPatternCWEID);
                        string sAttackPatternCWEID=oAttackPatternCWE.AttackPatternCWEID.ToString();
                        
                        //Search CWE informations in XORCISM
                        CWE oCWE = null;
                        try
                        {
                            oCWE = model.CWE.Where(o => o.CWEID == sAttackPatternCWEID).FirstOrDefault();
                            //xlWorkSheet.Cells[iRowIndex, iColumnIndex] = oAttackPatternCWE.CWE.CWEID + " " + oAttackPatternCWE.CWE.CWEName;
                            xlWorkSheet.Cells[iRowIndex, iColumnIndex] = oCWE.CWEID + " " + oCWE.CWEName;

                            iRowIndex++;
                        }
                        catch(Exception exCWE01)
                        {
                            Console.WriteLine("Exception exCWE01 " + exCWE01.Message + " " + exCWE01.InnerException);
                        }
                    }
                }
            }
            #endregion AttackPatternMasterParent

            //TODO?
            //PeerOf
            //CanAlsoBe


            //To the right
            //HasMember HARDCODED
            //A ParentOf X => write X
            foreach (ATTACKPATTERNRELATIONSHIP oAttackPatternRelation in oAttackPatternMaster.ATTACKPATTERNRELATIONSHIP.Where(o => o.AttackPatternRefID == oAttackPatternMaster.AttackPatternID && (o.RelationshipName == "ParentOf" || o.RelationshipName == "HasMember" || o.RelationshipName == "Leverage" || o.RelationshipName == "CanFollow")))
            {
                Console.WriteLine("DEBUG02 " + oAttackPatternMaster.capec_id + " (" + oAttackPatternMaster.AttackPatternID + ") " + oAttackPatternRelation.RelationshipName + " " + oAttackPatternRelation.ATTACKPATTERN1.capec_id + " (" + oAttackPatternRelation.ATTACKPATTERN1.AttackPatternID + ")");
                xlWorkSheet.Cells[iRowIndex, iColumnIndex] = oAttackPatternRelation.ATTACKPATTERN1.capec_id + " " + oAttackPatternRelation.ATTACKPATTERN1.AttackPatternName;
                fWriteChilds(oAttackPatternRelation.ATTACKPATTERN1, iColumnIndex + 1);
                iRowIndex++;
                if (bIncludeCWEs)
                {
                    //List the CWEs
                    foreach (ATTACKPATTERNCWE oAttackPatternCWE in oAttackPatternRelation.ATTACKPATTERN1.ATTACKPATTERNCWE)
                    {
                        Console.WriteLine("DEBUG AttackPatternCWEID=" + oAttackPatternCWE.AttackPatternCWEID);
                        string sAttackPatternCWEID = oAttackPatternCWE.AttackPatternCWEID.ToString();
                        
                        //xlWorkSheet.Cells[iRowIndex, iColumnIndex] = oAttackPatternCWE.CWE.CWEID + " " + oAttackPatternCWE.CWE.CWEName;

                        //Search CWE informations in XORCISM
                        CWE oCWE = null;
                        try
                        {
                            oCWE = model.CWE.Where(o => o.CWEID == sAttackPatternCWEID).FirstOrDefault();
                            xlWorkSheet.Cells[iRowIndex, iColumnIndex] = oCWE.CWEID + " " + oCWE.CWEName;

                            iRowIndex++;
                        }
                        catch (Exception exCWE02)
                        {
                            Console.WriteLine("Exception exCWE02 " + exCWE02.Message + " " + exCWE02.InnerException);
                        }
                    }
                }
            }

            //Leverage HARDCODED
            /*
            foreach (ATTACKPATTERNRELATIONSHIP oAttackPatternRelation in oAttackPatternMaster.ATTACKPATTERNRELATIONSHIP.Where(o => o.RelationshipName == "Leverage" && o.AttackPatternRefID == oAttackPatternMaster.AttackPatternID))
            {
                Console.WriteLine("DEBUG " + oAttackPatternMaster.capec_id + " Leverage " + oAttackPatternRelation.ATTACKPATTERN1.capec_id);
                xlWorkSheet.Cells[iRowIndex, iColumnIndex] = oAttackPatternRelation.ATTACKPATTERN1.capec_id + " " + oAttackPatternRelation.ATTACKPATTERN1.AttackPatternName;
                fWriteChilds(oAttackPatternRelation.ATTACKPATTERN1, iColumnIndex + 1);
                iRowIndex++;
                //List the CWEs
                foreach (ATTACKPATTERNCWE oAttackPatternCWE in oAttackPatternRelation.ATTACKPATTERN1.ATTACKPATTERNCWE)
                {
                    Console.WriteLine("DEBUG AttackPatternCWEID=" + oAttackPatternCWE.AttackPatternCWEID);
                    xlWorkSheet.Cells[iRowIndex, iColumnIndex] = oAttackPatternCWE.CWE.CWEID + " " + oAttackPatternCWE.CWE.CWEName;
                    iRowIndex++;
                }
            }
            */


            //CanAlsoBe     //TODO?
    

            //ChildOf   HARDCODED
            foreach (ATTACKPATTERNRELATIONSHIP oAttackPatternRelation in oAttackPatternMaster.ATTACKPATTERNRELATIONSHIP1.Where(o => o.AttackPatternSubjectID == oAttackPatternMaster.AttackPatternID && (o.RelationshipName == "ChildOf")))
            {
                Console.WriteLine("DEBUG03 " + oAttackPatternRelation.ATTACKPATTERN.capec_id + " (" + oAttackPatternRelation.ATTACKPATTERN.AttackPatternID + ") " + oAttackPatternRelation.RelationshipName + " " + oAttackPatternMaster.capec_id + " (" + oAttackPatternMaster.AttackPatternID + ")");
                xlWorkSheet.Cells[iRowIndex, iColumnIndex] = oAttackPatternRelation.ATTACKPATTERN.capec_id + " " + oAttackPatternRelation.ATTACKPATTERN.AttackPatternName;
                fWriteChilds(oAttackPatternRelation.ATTACKPATTERN, iColumnIndex + 1);
                iRowIndex++;
                if (bIncludeCWEs)
                {
                    //List the CWEs
                    foreach (ATTACKPATTERNCWE oAttackPatternCWE in oAttackPatternRelation.ATTACKPATTERN.ATTACKPATTERNCWE)
                    {
                        Console.WriteLine("DEBUG AttackPatternCWEID=" + oAttackPatternCWE.AttackPatternCWEID);
                        string sAttackPatternCWEID = oAttackPatternCWE.AttackPatternCWEID.ToString();

                        //xlWorkSheet.Cells[iRowIndex, iColumnIndex] = oAttackPatternCWE.CWE.CWEID + " " + oAttackPatternCWE.CWE.CWEName;

                        //Search CWE informations in XORCISM
                        CWE oCWE = null;
                        try
                        {
                            oCWE = model.CWE.Where(o => o.CWEID == sAttackPatternCWEID).FirstOrDefault();
                            xlWorkSheet.Cells[iRowIndex, iColumnIndex] = oCWE.CWEID + " " + oCWE.CWEName;

                            iRowIndex++;
                        }
                        catch (Exception exCWE03)
                        {
                            Console.WriteLine("Exception exCWE03 " + exCWE03.Message + " " + exCWE03.InnerException);
                        }
                    }
                }
            }


            if(iRowIndex==iRowIndexTemp)
            {
                //Nothing found but we increment anyway
                iRowIndex++;
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
