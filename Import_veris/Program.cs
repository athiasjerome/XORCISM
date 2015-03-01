using System;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Web;
using Jayrock;
using Jayrock.Json.Conversion;
using Jayrock.Json;

using System.Data;
using XORCISMModel;
using XTHREATModel;


namespace Import_veris
{
    static class Program
    {
        /// <summary>
        /// Copyright (C) 2014-2015 Jerome Athias
        /// Import the VERIZON VERIS default vocabularies enumeration values in an XORCISM database
        /// This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation; either version 2 of the License, or (at your option) any later version.
        /// 
        /// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
        /// 
        /// You should have received a copy of the GNU General Public License along with this program; if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());

            //https://raw.github.com/vz-risk/veris/master/verisc-enum.json

            try
            {
                WebClient wc = new WebClient();
                Console.WriteLine("Downloading verisc-enum.json");
                wc.DownloadFile("https://raw.github.com/vz-risk/veris/master/verisc-enum.json", "C:/nvdcve/verisc-enum.json");  //HARDCODED
                // 
                wc.Dispose();
                //Console.WriteLine("Download is completed", "info", MessageBoxButtons.OK, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while downloading\n" + ex.Message + " " + ex.InnerException);
            }

            // create reader & open file
            StreamReader tr = new StreamReader(@"C:\nvdcve\verisc-enum.json");  //HARDCODED

            // read the file
            string verisenum = tr.ReadToEnd();
            //Console.WriteLine(verisenum);

            // close the stream
            tr.Close();


            XORCISMEntities model= new XORCISMEntities();

            XTHREATEntities threat_model= new XTHREATEntities();


            JsonObject jobj = (JsonObject)JsonConvert.Import(verisenum);
            JsonArray jarray = new JsonArray();
            foreach (string loc in jobj.Names)
            {
                //Console.WriteLine(loc);
                switch (loc)
                {
                    case "security_compromise":

                        break;
                    case "confidence":

                        break;
                    case "victim":
                        //Console.WriteLine(Convert.ToString(((JsonObject)jobj[loc])["employee_count"]));
                        jarray = (JsonArray)((JsonObject)jobj[loc])["employee_count"];
                        for (int cpt = 0; cpt < jarray.Length-1; cpt++)
                        {
                            //Console.WriteLine(Convert.ToString(jarray[cpt]));
                        }
                        break;
                    case "actor":

                        break;
                    case "action":
                        //Console.WriteLine(Convert.ToString(((JsonObject)jobj[loc])["malware"]));
                        //************* MALWARE **************************
                        JsonObject jobj2 = (JsonObject)JsonConvert.Import(Convert.ToString(((JsonObject)jobj[loc])["malware"]));
                        
                        foreach (string loc2 in jobj2.Names)
                        {
                            //Console.WriteLine(loc2);
                            //variety
                            //vector
                            switch (loc2)
                            {
                                case "variety":
                                    jarray = (JsonArray)(jobj2)["variety"];
                                    for (int cpt = 0; cpt < jarray.Length-1; cpt++)
                                    {
                                        string sThreatActionVarietyName = Convert.ToString(jarray[cpt]);
                                        //Console.WriteLine(sThreatActionVarietyName);
                                        //ThreatActionCategoryID=1  //malware
                                        XTHREATModel.THREATACTIONVARIETY tactionvariety = new THREATACTIONVARIETY();
                                        tactionvariety = threat_model.THREATACTIONVARIETY.FirstOrDefault(o => o.ThreatActionCategoryID == 1 && o.ThreatActionVarietyName == sThreatActionVarietyName);
                                        if (tactionvariety == null)
                                        {
                                            tactionvariety = new THREATACTIONVARIETY();
                                            tactionvariety.ThreatActionCategoryID = 1;  //malware
                                            tactionvariety.ThreatActionVarietyName = sThreatActionVarietyName;
                                            threat_model.THREATACTIONVARIETY.Add(tactionvariety);
                                            threat_model.SaveChanges();
                                        }
                                    }
                                    break;
                                case "vector":
                                    jarray = (JsonArray)(jobj2)["vector"];
                                    for (int cpt = 0; cpt < jarray.Length-1; cpt++)
                                    {
                                        string sThreatActionVectorName = Convert.ToString(jarray[cpt]);
                                        //Console.WriteLine(sThreatActionVectorName);
                                        //ThreatActionCategoryID=1  //malware
                                        XTHREATModel.THREATACTIONVECTOR tactionvector = new THREATACTIONVECTOR();
                                        tactionvector = threat_model.THREATACTIONVECTOR.FirstOrDefault(o => o.ThreatActionCategoryID == 1 && o.ThreatActionVectorName == sThreatActionVectorName);
                                        if (tactionvector == null)
                                        {
                                            tactionvector = new THREATACTIONVECTOR();
                                            tactionvector.ThreatActionCategoryID = 1;  //malware
                                            tactionvector.ThreatActionVectorName = sThreatActionVectorName;
                                            threat_model.THREATACTIONVECTOR.Add(tactionvector);
                                            threat_model.SaveChanges();
                                        }
                                    }
                                    break;
                                default:
                                    Console.WriteLine("ERROR "+loc2 + " is unknown for action.malware");
                                    break;
                            }
                        }

                        //************* HACKING **************************
                        jobj2 = (JsonObject)JsonConvert.Import(Convert.ToString(((JsonObject)jobj[loc])["hacking"]));
                        
                        foreach (string loc2 in jobj2.Names)
                        {
                            //Console.WriteLine(loc2);
                            //variety
                            //vector
                            switch (loc2)
                            {
                                case "variety":
                                    jarray = (JsonArray)(jobj2)["variety"];
                                    for (int cpt = 0; cpt < jarray.Length-1; cpt++)
                                    {
                                        string sThreatActionVarietyName = Convert.ToString(jarray[cpt]);
                                        //Console.WriteLine(sThreatActionVarietyName);
                                        //ThreatActionCategoryID=2  //hacking
                                        XTHREATModel.THREATACTIONVARIETY tactionvariety = new THREATACTIONVARIETY();
                                        tactionvariety = threat_model.THREATACTIONVARIETY.FirstOrDefault(o => o.ThreatActionCategoryID==2 && o.ThreatActionVarietyName == sThreatActionVarietyName);
                                        if (tactionvariety == null)
                                        {
                                            tactionvariety = new THREATACTIONVARIETY();
                                            tactionvariety.ThreatActionCategoryID = 2;  //hacking
                                            tactionvariety.ThreatActionVarietyName = sThreatActionVarietyName;
                                            threat_model.THREATACTIONVARIETY.Add(tactionvariety);
                                            threat_model.SaveChanges();
                                        }
                                    }
                                    break;
                                case "vector":
                                    jarray = (JsonArray)(jobj2)["vector"];
                                    for (int cpt = 0; cpt < jarray.Length-1; cpt++)
                                    {
                                        string sThreatActionVectorName = Convert.ToString(jarray[cpt]);
                                        //Console.WriteLine(sThreatActionVectorName);
                                        //ThreatActionCategoryID=2  //hacking
                                        XTHREATModel.THREATACTIONVECTOR tactionvector = new THREATACTIONVECTOR();
                                        tactionvector = threat_model.THREATACTIONVECTOR.FirstOrDefault(o => o.ThreatActionCategoryID == 2 && o.ThreatActionVectorName == sThreatActionVectorName);
                                        if (tactionvector == null)
                                        {
                                            tactionvector = new THREATACTIONVECTOR();
                                            tactionvector.ThreatActionCategoryID = 2;  //hacking
                                            tactionvector.ThreatActionVectorName = sThreatActionVectorName;
                                            threat_model.THREATACTIONVECTOR.Add(tactionvector);
                                            threat_model.SaveChanges();
                                        }
                                    }
                                    break;
                                default:
                                    Console.WriteLine(loc2 + " is unknown for action.hacking");
                                    break;
                            }
                        }

                        //************* SOCIAL **************************
                        jobj2 = (JsonObject)JsonConvert.Import(Convert.ToString(((JsonObject)jobj[loc])["social"]));

                        foreach (string loc2 in jobj2.Names)
                        {
                            //Console.WriteLine(loc2);
                            //variety
                            //vector
                            switch (loc2)
                            {
                                case "variety":
                                    jarray = (JsonArray)(jobj2)["variety"];
                                    for (int cpt = 0; cpt < jarray.Length - 1; cpt++)
                                    {
                                        string sThreatActionVarietyName = Convert.ToString(jarray[cpt]);
                                        //Console.WriteLine(sThreatActionVarietyName);
                                        //ThreatActionCategoryID=3  //social
                                        XTHREATModel.THREATACTIONVARIETY tactionvariety = new THREATACTIONVARIETY();
                                        tactionvariety = threat_model.THREATACTIONVARIETY.FirstOrDefault(o => o.ThreatActionCategoryID == 3 && o.ThreatActionVarietyName == sThreatActionVarietyName);
                                        if (tactionvariety == null)
                                        {
                                            tactionvariety = new THREATACTIONVARIETY();
                                            tactionvariety.ThreatActionCategoryID = 3;  //social
                                            tactionvariety.ThreatActionVarietyName = sThreatActionVarietyName;
                                            threat_model.THREATACTIONVARIETY.Add(tactionvariety);
                                            threat_model.SaveChanges();
                                        }
                                    }
                                    break;
                                case "vector":
                                    jarray = (JsonArray)(jobj2)["vector"];
                                    for (int cpt = 0; cpt < jarray.Length - 1; cpt++)
                                    {
                                        string sThreatActionVectorName = Convert.ToString(jarray[cpt]);
                                        //Console.WriteLine(sThreatActionVectorName);
                                        //ThreatActionCategoryID=3  //social
                                        XTHREATModel.THREATACTIONVECTOR tactionvector = new THREATACTIONVECTOR();
                                        tactionvector = threat_model.THREATACTIONVECTOR.FirstOrDefault(o => o.ThreatActionCategoryID == 3 && o.ThreatActionVectorName == sThreatActionVectorName);
                                        if (tactionvector == null)
                                        {
                                            tactionvector = new THREATACTIONVECTOR();
                                            tactionvector.ThreatActionCategoryID = 3;  //social
                                            tactionvector.ThreatActionVectorName = sThreatActionVectorName;
                                            threat_model.THREATACTIONVECTOR.Add(tactionvector);
                                            threat_model.SaveChanges();
                                        }
                                    }
                                    break;
                                case "target":
                                    jarray = (JsonArray)(jobj2)["target"];
                                    for (int cpt = 0; cpt < jarray.Length - 1; cpt++)
                                    {
                                        string sThreatActionTargetName = Convert.ToString(jarray[cpt]);
                                        //Console.WriteLine(sThreatActionTargetName);
                                        //ThreatActionCategoryID=3  //social
                                        XTHREATModel.THREATACTIONTARGET tactiontarget = new THREATACTIONTARGET();
                                        tactiontarget = threat_model.THREATACTIONTARGET.FirstOrDefault(o => o.ThreatActionCategoryID == 3 && o.ThreatActionTargetName == sThreatActionTargetName);
                                        if (tactiontarget == null)
                                        {
                                            tactiontarget = new THREATACTIONTARGET();
                                            tactiontarget.ThreatActionCategoryID = 3;  //social
                                            tactiontarget.ThreatActionTargetName = sThreatActionTargetName;
                                            threat_model.THREATACTIONTARGET.Add(tactiontarget);
                                            threat_model.SaveChanges();
                                        }
                                    }
                                    break;
                                default:
                                    Console.WriteLine(loc2 + " is unknown for action.social");
                                    break;
                            }
                        }

                        //************* SOCIAL **************************
                        jobj2 = (JsonObject)JsonConvert.Import(Convert.ToString(((JsonObject)jobj[loc])["social"]));

                        foreach (string loc2 in jobj2.Names)
                        {
                            //Console.WriteLine(loc2);
                            //variety
                            //vector
                            switch (loc2)
                            {
                                case "variety":
                                    jarray = (JsonArray)(jobj2)["variety"];
                                    for (int cpt = 0; cpt < jarray.Length - 1; cpt++)
                                    {
                                        string sThreatActionVarietyName = Convert.ToString(jarray[cpt]);
                                        //Console.WriteLine(sThreatActionVarietyName);
                                        //ThreatActionCategoryID=3  //social
                                        XTHREATModel.THREATACTIONVARIETY tactionvariety = new THREATACTIONVARIETY();
                                        tactionvariety = threat_model.THREATACTIONVARIETY.FirstOrDefault(o => o.ThreatActionCategoryID == 3 && o.ThreatActionVarietyName == sThreatActionVarietyName);
                                        if (tactionvariety == null)
                                        {
                                            tactionvariety = new THREATACTIONVARIETY();
                                            tactionvariety.ThreatActionCategoryID = 3;  //social
                                            tactionvariety.ThreatActionVarietyName = sThreatActionVarietyName;
                                            threat_model.THREATACTIONVARIETY.Add(tactionvariety);
                                            threat_model.SaveChanges();
                                        }
                                    }
                                    break;
                                case "vector":
                                    jarray = (JsonArray)(jobj2)["vector"];
                                    for (int cpt = 0; cpt < jarray.Length - 1; cpt++)
                                    {
                                        string sThreatActionVectorName = Convert.ToString(jarray[cpt]);
                                        //Console.WriteLine(sThreatActionVectorName);
                                        //ThreatActionCategoryID=3  //social
                                        XTHREATModel.THREATACTIONVECTOR tactionvector = new THREATACTIONVECTOR();
                                        tactionvector = threat_model.THREATACTIONVECTOR.FirstOrDefault(o => o.ThreatActionCategoryID == 3 && o.ThreatActionVectorName == sThreatActionVectorName);
                                        if (tactionvector == null)
                                        {
                                            tactionvector = new THREATACTIONVECTOR();
                                            tactionvector.ThreatActionCategoryID = 3;  //social
                                            tactionvector.ThreatActionVectorName = sThreatActionVectorName;
                                            threat_model.THREATACTIONVECTOR.Add(tactionvector);
                                            threat_model.SaveChanges();
                                        }
                                    }
                                    break;
                                default:
                                    Console.WriteLine(loc2 + " is unknown for action.social");
                                    break;
                            }
                        }

                        //************* MISUSE **************************
                        jobj2 = (JsonObject)JsonConvert.Import(Convert.ToString(((JsonObject)jobj[loc])["misuse"]));

                        foreach (string loc2 in jobj2.Names)
                        {
                            //Console.WriteLine(loc2);
                            //variety
                            //vector
                            switch (loc2)
                            {
                                case "variety":
                                    jarray = (JsonArray)(jobj2)["variety"];
                                    for (int cpt = 0; cpt < jarray.Length - 1; cpt++)
                                    {
                                        string sThreatActionVarietyName = Convert.ToString(jarray[cpt]);
                                        //Console.WriteLine(sThreatActionVarietyName);
                                        //ThreatActionCategoryID=4  //misuse
                                        XTHREATModel.THREATACTIONVARIETY tactionvariety = new THREATACTIONVARIETY();
                                        tactionvariety = threat_model.THREATACTIONVARIETY.FirstOrDefault(o => o.ThreatActionCategoryID == 4 && o.ThreatActionVarietyName == sThreatActionVarietyName);
                                        if (tactionvariety == null)
                                        {
                                            tactionvariety = new THREATACTIONVARIETY();
                                            tactionvariety.ThreatActionCategoryID = 4;  //misuse
                                            tactionvariety.ThreatActionVarietyName = sThreatActionVarietyName;
                                            threat_model.THREATACTIONVARIETY.Add(tactionvariety);
                                            threat_model.SaveChanges();
                                        }
                                    }
                                    break;
                                case "vector":
                                    jarray = (JsonArray)(jobj2)["vector"];
                                    for (int cpt = 0; cpt < jarray.Length - 1; cpt++)
                                    {
                                        string sThreatActionVectorName = Convert.ToString(jarray[cpt]);
                                        //Console.WriteLine(sThreatActionVectorName);
                                        //ThreatActionCategoryID=4  //misuse
                                        XTHREATModel.THREATACTIONVECTOR tactionvector = new THREATACTIONVECTOR();
                                        tactionvector = threat_model.THREATACTIONVECTOR.FirstOrDefault(o => o.ThreatActionCategoryID == 4 && o.ThreatActionVectorName == sThreatActionVectorName);
                                        if (tactionvector == null)
                                        {
                                            tactionvector = new THREATACTIONVECTOR();
                                            tactionvector.ThreatActionCategoryID = 4;  //misuse
                                            tactionvector.ThreatActionVectorName = sThreatActionVectorName;
                                            threat_model.THREATACTIONVECTOR.Add(tactionvector);
                                            threat_model.SaveChanges();
                                        }
                                    }
                                    break;
                                default:
                                    Console.WriteLine(loc2 + " is unknown for action.misuse");
                                    break;
                            }
                        }

                        //************* PHYSICAL **************************
                        jobj2 = (JsonObject)JsonConvert.Import(Convert.ToString(((JsonObject)jobj[loc])["physical"]));

                        foreach (string loc2 in jobj2.Names)
                        {
                            //Console.WriteLine(loc2);
                            //variety
                            //vector
                            switch (loc2)
                            {
                                case "variety":
                                    jarray = (JsonArray)(jobj2)["variety"];
                                    for (int cpt = 0; cpt < jarray.Length - 1; cpt++)
                                    {
                                        string sThreatActionVarietyName = Convert.ToString(jarray[cpt]);
                                        //Console.WriteLine(sThreatActionVarietyName);
                                        //ThreatActionCategoryID=5  //physical
                                        XTHREATModel.THREATACTIONVARIETY tactionvariety = new THREATACTIONVARIETY();
                                        tactionvariety = threat_model.THREATACTIONVARIETY.FirstOrDefault(o => o.ThreatActionCategoryID == 5 && o.ThreatActionVarietyName == sThreatActionVarietyName);
                                        if (tactionvariety == null)
                                        {
                                            tactionvariety = new THREATACTIONVARIETY();
                                            tactionvariety.ThreatActionCategoryID = 5;  //physical
                                            tactionvariety.ThreatActionVarietyName = sThreatActionVarietyName;
                                            threat_model.THREATACTIONVARIETY.Add(tactionvariety);
                                            threat_model.SaveChanges();
                                        }
                                    }
                                    break;
                                case "location":
                                    jarray = (JsonArray)(jobj2)["location"];
                                    for (int cpt = 0; cpt < jarray.Length - 1; cpt++)
                                    {
                                        string sThreatActionLocationName = Convert.ToString(jarray[cpt]);
                                        //Console.WriteLine(sThreatActionLocationName);
                                        //ThreatActionCategoryID=5  //physical
                                        XTHREATModel.THREATACTIONLOCATION tactionlocation = new THREATACTIONLOCATION();
                                        tactionlocation = threat_model.THREATACTIONLOCATION.FirstOrDefault(o => o.ThreatActionLocationName == sThreatActionLocationName);
                                        if (tactionlocation == null)
                                        {
                                            tactionlocation = new THREATACTIONLOCATION();
                                            //tactionlocation.ThreatActionCategoryID = 5;  //physical
                                            tactionlocation.ThreatActionLocationName = sThreatActionLocationName;
                                            threat_model.THREATACTIONLOCATION.Add(tactionlocation);
                                            threat_model.SaveChanges();
                                        }
                                    }
                                    break;
                                case "vector":
                                    jarray = (JsonArray)(jobj2)["vector"];
                                    for (int cpt = 0; cpt < jarray.Length - 1; cpt++)
                                    {
                                        string sThreatActionVectorName = Convert.ToString(jarray[cpt]);
                                        //Console.WriteLine(sThreatActionVectorName);
                                        //ThreatActionCategoryID=5  //physical
                                        XTHREATModel.THREATACTIONVECTOR tactionvector = new THREATACTIONVECTOR();
                                        tactionvector = threat_model.THREATACTIONVECTOR.FirstOrDefault(o => o.ThreatActionCategoryID == 5 && o.ThreatActionVectorName == sThreatActionVectorName);
                                        if (tactionvector == null)
                                        {
                                            tactionvector = new THREATACTIONVECTOR();
                                            tactionvector.ThreatActionCategoryID = 5;  //physical
                                            tactionvector.ThreatActionVectorName = sThreatActionVectorName;
                                            threat_model.THREATACTIONVECTOR.Add(tactionvector);
                                            threat_model.SaveChanges();
                                        }
                                    }
                                    break;
                                default:
                                    Console.WriteLine(loc2 + " is unknown for action.physical");
                                    break;
                            }
                        }

                        //************* ERROR **************************
                        jobj2 = (JsonObject)JsonConvert.Import(Convert.ToString(((JsonObject)jobj[loc])["error"]));

                        foreach (string loc2 in jobj2.Names)
                        {
                            //Console.WriteLine(loc2);
                            //variety
                            //vector
                            switch (loc2)
                            {
                                case "variety":
                                    jarray = (JsonArray)(jobj2)["variety"];
                                    for (int cpt = 0; cpt < jarray.Length - 1; cpt++)
                                    {
                                        string sThreatActionVarietyName = Convert.ToString(jarray[cpt]);
                                        //Console.WriteLine(sThreatActionVarietyName);
                                        //ThreatActionCategoryID=6  //error
                                        XTHREATModel.THREATACTIONVARIETY tactionvariety = new THREATACTIONVARIETY();
                                        tactionvariety = threat_model.THREATACTIONVARIETY.FirstOrDefault(o => o.ThreatActionCategoryID == 6 && o.ThreatActionVarietyName == sThreatActionVarietyName);
                                        if (tactionvariety == null)
                                        {
                                            tactionvariety = new THREATACTIONVARIETY();
                                            tactionvariety.ThreatActionCategoryID = 6;  //error
                                            tactionvariety.ThreatActionVarietyName = sThreatActionVarietyName;
                                            threat_model.THREATACTIONVARIETY.Add(tactionvariety);
                                            threat_model.SaveChanges();
                                        }
                                    }
                                    break;
                                case "vector":
                                    jarray = (JsonArray)(jobj2)["vector"];
                                    for (int cpt = 0; cpt < jarray.Length - 1; cpt++)
                                    {
                                        string sThreatActionVectorName = Convert.ToString(jarray[cpt]);
                                        //Console.WriteLine(sThreatActionVectorName);
                                        //ThreatActionCategoryID=6  //error
                                        XTHREATModel.THREATACTIONVECTOR tactionvector = new THREATACTIONVECTOR();
                                        tactionvector = threat_model.THREATACTIONVECTOR.FirstOrDefault(o => o.ThreatActionCategoryID == 6 && o.ThreatActionVectorName == sThreatActionVectorName);
                                        if (tactionvector == null)
                                        {
                                            tactionvector = new THREATACTIONVECTOR();
                                            tactionvector.ThreatActionCategoryID = 6;  //error
                                            tactionvector.ThreatActionVectorName = sThreatActionVectorName;
                                            threat_model.THREATACTIONVECTOR.Add(tactionvector);
                                            threat_model.SaveChanges();
                                        }
                                    }
                                    break;
                                default:
                                    Console.WriteLine(loc2 + " is unknown for action.error");
                                    break;
                            }
                        }

                        //************* ENVIRONMENTAL **************************
                        jobj2 = (JsonObject)JsonConvert.Import(Convert.ToString(((JsonObject)jobj[loc])["environmental"]));

                        foreach (string loc2 in jobj2.Names)
                        {
                            //Console.WriteLine(loc2);
                            //variety
                            switch (loc2)
                            {
                                case "variety":
                                    jarray = (JsonArray)(jobj2)["variety"];
                                    for (int cpt = 0; cpt < jarray.Length - 1; cpt++)
                                    {
                                        string sThreatActionVarietyName = Convert.ToString(jarray[cpt]);
                                        //Console.WriteLine(sThreatActionVarietyName);
                                        //ThreatActionCategoryID=7  //environmental
                                        XTHREATModel.THREATACTIONVARIETY tactionvariety = new THREATACTIONVARIETY();
                                        tactionvariety = threat_model.THREATACTIONVARIETY.FirstOrDefault(o => o.ThreatActionCategoryID == 7 && o.ThreatActionVarietyName == sThreatActionVarietyName);
                                        if (tactionvariety == null)
                                        {
                                            tactionvariety = new THREATACTIONVARIETY();
                                            tactionvariety.ThreatActionCategoryID = 7;  //environmental
                                            tactionvariety.ThreatActionVarietyName = sThreatActionVarietyName;
                                            threat_model.THREATACTIONVARIETY.Add(tactionvariety);
                                            threat_model.SaveChanges();
                                        }
                                    }
                                    break;
                                
                                default:
                                    Console.WriteLine(loc2 + " is unknown for action.environmental");
                                    break;
                            }
                        }

                        break;
                    case "asset":
                        jarray = (JsonArray)((JsonObject)jobj[loc])["variety"];
                        for (int cpt = 0; cpt < jarray.Length-1; cpt++)
                        {
                            string sAssetVarietyName = Convert.ToString(jarray[cpt]);
                            //Console.WriteLine(sAssetVarietyName);
                            XORCISMModel.ASSETVARIETY assetvariety = new ASSETVARIETY();
                            assetvariety = model.ASSETVARIETY.FirstOrDefault(o => o.AssetVarietyName == sAssetVarietyName);
                            if (assetvariety == null)
                            {
                                assetvariety = new ASSETVARIETY();
                                assetvariety.AssetVarietyName = sAssetVarietyName;
                                model.ASSETVARIETY.Add(assetvariety);
                                model.SaveChanges();
                            }
                        }
                        //cloud
                        break;
                    case "attribute":
                        
                        break;
                    case "timeline":

                        break;
                    case "discovery_method":

                        break;
                    case "cost_corrective_action":

                        break;
                    case "impact":

                        break;
                    case "country":
                        //ISOCOUNTRY    COUNTRYISO
                        //http://www.iso.org/iso/home/standards/country_codes/country_names_and_code_elements.htm

                        // create reader & open file
                        tr = new StreamReader(@"country_names_and_code_element.txt");   //HARDCODED

                        // read the file
                        string scountryline = tr.ReadLine();
                        //Console.WriteLine(verisenum);
                        //Country Name;ISO 3166-1-alpha-2 code
                        //Ignore the first line (headers)
                        scountryline = tr.ReadLine();
                        string[] row;
                        while (scountryline != null && scountryline!="")
                        {
                            row = scountryline.Split(';');
                            string sCountryName = row[0];
                            string sCountryCode = row[1];
                            XORCISMModel.COUNTRY country = new COUNTRY();
                            country = model.COUNTRY.FirstOrDefault(o => o.CountryName == sCountryName && o.CountryCode == sCountryCode);
                            if (country == null)
                            {
                                country = new COUNTRY();
                                country.CountryCode = sCountryCode;
                                country.CountryName = sCountryName;
                                model.COUNTRY.Add(country);
                                model.SaveChanges();

                            }
                            scountryline = tr.ReadLine();
                        }

                        // close the stream
                        tr.Close();



                        jarray = (JsonArray)jobj[loc];
                        for (int cpt = 0; cpt < jarray.Length-1; cpt++)
                        {
                            string scountry = Convert.ToString(jarray[cpt]).ToUpper();
                            //Console.WriteLine(scountry);
                            XORCISMModel.COUNTRY country = new COUNTRY();
                            country = model.COUNTRY.FirstOrDefault(o => o.CountryName == scountry);
                            if (country == null)
                            {
                                country = new COUNTRY();
                                //country.CountryCode = "";
                                country.CountryName = scountry;
                                Console.WriteLine("Country: "+scountry+" not found in the database.");
                                //model.AddToCOUNTRY(country);
                                //model.SaveChanges();

                            }
                        }
                        break;
                    case "iso_currency_code":
                        //jarray = (JsonArray)((JsonObject)jobj[loc])[0];
                        //jarray = new JsonArray(loc);
                        jarray = (JsonArray)jobj[loc];
                        for (int cpt = 0; cpt < jarray.Length-1; cpt++)
                        {
                            string scurrency = Convert.ToString(jarray[cpt]);
                            //Console.WriteLine(scurrency);
                            XORCISMModel.ISOCURRENCY currency = new ISOCURRENCY();
                            currency = model.ISOCURRENCY.FirstOrDefault(o => o.iso_currency_code == scurrency);
                            if (currency == null)
                            {
                                currency = new ISOCURRENCY();
                                currency.iso_currency_code = scurrency;
                                model.ISOCURRENCY.Add(currency);
                                model.SaveChanges();
                            }
                        }
                        break;
                    default:
                        break;

                }


                

                //wid.value = Convert.ToString(((JsonObject)jobj[loc])["name"]);
                //JsonArray coords = (JsonArray)((JsonObject)jobj[loc])["coords"];
                //wid.style.left = Convert.ToString(coords[0]);
                //wid.style.top = Convert.ToString(coords[1]);
            }
            //FREE
            model.Dispose();
            model = null;
        }
    }
}
