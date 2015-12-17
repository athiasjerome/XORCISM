using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.RegularExpressions;
using System.IO;

namespace Import_osvdb
{
    class Program
    {
        static void Main(string[] args)
        {
            string sCurrentPath = Directory.GetCurrentDirectory();
            //System.IO.File.WriteAllText(sCurrentPath + @"\osvdb\" + sOSVDBID + ".txt", ResponseText);
            string sOSVDBID = "105465";//BIG Heartbleed // "77356";

            string sSourceFilePath = sCurrentPath + @"\osvdb\" + sOSVDBID + ".txt";
            StreamReader monStreamReader = new StreamReader(sSourceFilePath);
            string ligne = monStreamReader.ReadLine();
            //Console.WriteLine(ligne);
            string sInfoToSearch = "";
            string sOSVDBDescription = "";  //Could be multi lines
            string sOSVDBSolution = "";  //Could be multi lines
            Regex myRegex = null;
            string strTemp = string.Empty;
            while (ligne != null)
            {
                //Technique 1: line by line
                string ResponseText = ligne;

                if (ligne.Contains("<title>"))
                {
                    sInfoToSearch = "title";
                    Console.WriteLine("DEBUG OSVDB Parsing "+sInfoToSearch);
                }
                
                


                //***********************************************************************************
                switch(sInfoToSearch)
                {
                    case "products":
                        //TODO
                        //<td 
                        //href="/vendor/

                        //TODO: retrieve CPEs
                            sInfoToSearch = "";
                        break;
                    case "credit":
                        //TODO

                            sInfoToSearch = "";
                        break;
                    case "cvss":
                        //TODO

                        sInfoToSearch = "";
                        break;
                    case "references":
                        if (ligne != "</li>")
                        {
                            //TODO:
                            //<li>Exploit Database:
                            //<li>CVE ID:
                            //<li>Secunia Advisory ID:
                            //<li>Bugtraq ID:
                            //<li>Other Advisory URL:
                            //<li>Mail List Post:
                            //<li>Generic Exploit URL:
                            //<li>Vendor URL:
                            //<li>Vendor Specific News/Changelog Entry:

                            myRegex = new Regex(@"<a.*?href=[""'](.*?)[""'].*?>", RegexOptions.Singleline);
                            strTemp = myRegex.Match(ResponseText).ToString();
                            if (strTemp != "")
                            {
                                //Clean the URL
                                strTemp = strTemp.Replace("<a href=\"", "");
                                strTemp = strTemp.Replace("target=\"_blank\"", "");
                                strTemp = strTemp.Replace("\"", "");
                                strTemp = strTemp.Replace(">", "").Trim();
                                //Console.WriteLine("DEBUG Reference URL: " + strTemp);
                            }
                        }
                        if (ligne.Contains("</tbody>"))
                        {
                            sInfoToSearch = "";
                        }
                        break;
                    case "location":
                        if (ligne != "<br>")
                        {
                            strTemp = ligne.Trim();
                            Console.WriteLine("DEBUG OSVDB Location=" + strTemp);
                        }
                        else
                        {
                            sInfoToSearch = "";
                        }
                        break;
                    case "attacktype":
                        if(ligne!="<br>")
                        {
                            strTemp = ligne.Trim();
                            if (strTemp.EndsWith(","))
                            {
                                strTemp = strTemp.Replace(",", "");
                            }
                            Console.WriteLine("DEBUG OSVDB Attack Type=" + strTemp);
                            //Race Condition
                            //Input Manipulation
                            //TODO: map with CWE
                        }
                        else
                        {
                            sInfoToSearch = "";
                        }
                        break;
                    case "impact":
                        if (ligne != "<br>")
                        {
                            strTemp = ligne.Trim();
                            if (strTemp.EndsWith(","))
                            {
                                strTemp = strTemp.Replace(",", "");
                            }
                            Console.WriteLine("DEBUG OSVDB Impact=" + strTemp);
                            //TODO: mapping (e.g. VERIS?)
                        }
                        else
                        {
                            sInfoToSearch = "";
                        }
                        break;
                    case "solution":
                        if (ligne != "<br>")
                        {
                            strTemp = ligne.Trim();
                            if(strTemp.EndsWith(","))
                            {
                                strTemp = strTemp.Replace(",", "");
                            }
                            Console.WriteLine("DEBUG OSVDB Solution=" + strTemp);
                        }
                        else
                        {
                            sInfoToSearch = "";
                        }
                        break;
                    case "exploit":
                        if (ligne != "<br>")
                        {
                            strTemp = ligne.Trim();
                            if (strTemp.EndsWith(","))
                            {
                                strTemp = strTemp.Replace(",", "");
                            }
                            Console.WriteLine("DEBUG OSVDB Exploit=" + strTemp);
                        }
                        else
                        {
                            sInfoToSearch = "";
                        }
                        break;
                    case "disclosure":
                        if (ligne != "<br>")
                        {
                            strTemp = ligne.Trim();
                            if (strTemp.EndsWith(","))
                            {
                                strTemp = strTemp.Replace(",", "");
                            }
                            Console.WriteLine("DEBUG OSVDB Disclosure=" + strTemp);
                        }
                        else
                        {
                            sInfoToSearch = "";
                        }
                        break;
                    case "osvdb":
                        if (ligne != "<br>")
                        {
                            strTemp = ligne.Trim();
                            if (strTemp.EndsWith(","))
                            {
                                strTemp = strTemp.Replace(",", "");
                            }
                            Console.WriteLine("DEBUG OSVDB=" + strTemp);
                        }
                        else
                        {
                            sInfoToSearch = "";
                        }
                        break;
                    case "description":
                        myRegex = new Regex("<p>[^<>]*</p>");   //TODO: verify that it is always on 1 single line
                        strTemp = myRegex.Match(ResponseText).ToString();
                        if (strTemp != "")
                        {
                            strTemp = strTemp.Replace("<p>", "");
                            strTemp = strTemp.Replace("</p>", "");
                            if (sOSVDBDescription=="")
                            {
                                sOSVDBDescription = strTemp;
                            }
                            else
                            {
                                sOSVDBDescription += " " + strTemp;
                            }
                            //Console.WriteLine("DEBUG OSVDB Description=" + strTemp);
                            //sInfoToSearch = "";
                        }
                        if (ligne.Contains("</tbody>"))
                        {
                            sInfoToSearch = "";
                            Console.WriteLine("DEBUG OSVDB Description=" + sOSVDBDescription);
                        }
                        break;
                    case "disclosure date":
                        myRegex = new Regex("[1-2][0-9][0-9][0-9]-[0-9][0-9]-[0-9][0-9]");
                        strTemp = myRegex.Match(ResponseText).ToString();
                        if (strTemp != "")
                        {
                            Console.WriteLine("DEBUG OSVDB Disclosure Date=" + strTemp);
                            sInfoToSearch = "";
                        }
                        break;
                    case "solutiondescription":
                        myRegex = new Regex("<p>[^<>]*</p>");   //TODO: verify that it is always on 1 single line
                        strTemp = myRegex.Match(ResponseText).ToString();
                        if (strTemp != "")
                        {
                            strTemp = strTemp.Replace("<p>", "");
                            strTemp = strTemp.Replace("</p>", "");
                            if (sOSVDBSolution == "")
                            {
                                sOSVDBSolution = strTemp;
                            }
                            else
                            {
                                sOSVDBSolution += " " + strTemp;
                            }
                            //Console.WriteLine("DEBUG OSVDB Solution=" + strTemp);
                            //sInfoToSearch = "";
                        }
                        if (ligne.Contains("</tbody>"))
                        {
                            sInfoToSearch = "";
                            Console.WriteLine("DEBUG OSVDB Solution=" + sOSVDBSolution);
                        }
                        break;
                    case "title":
                        //OSVDB Title
                        myRegex = new Regex("<title>[^<>]*</title>");
                        strTemp = myRegex.Match(ResponseText).ToString();
                        if (strTemp != "")
                        {
                            string sReferenceOSVDBTitle = strTemp;
                            sReferenceOSVDBTitle = sReferenceOSVDBTitle.Replace("<title>", "");
                            sReferenceOSVDBTitle = sReferenceOSVDBTitle.Replace("</title>", "");
                            sReferenceOSVDBTitle = sReferenceOSVDBTitle.Replace(sOSVDBID + ":", "").Trim();
                            Console.WriteLine("DEBUG OSVDB Title=" + sReferenceOSVDBTitle);
                            //oReference.ReferenceTitle = sReferenceOSVDBTitle;
                            //oReference.timestamp = DateTimeOffset.Now;
                            //model.SaveChanges();
                            sInfoToSearch = "";
                        }
                        break;
                    default:
                        break;
                }

                //****************************************************************************
                if (ligne.Contains(">Disclosure Date<"))
                {
                    sInfoToSearch = "disclosure date";
                    Console.WriteLine("DEBUG OSVDB Parsing " + sInfoToSearch);
                }
                //Time to Patch
                //Days of Exposure
                if (ligne.Contains("Description</h1>"))
                {
                    sInfoToSearch = "description";
                    Console.WriteLine("DEBUG OSVDB Parsing " + sInfoToSearch);
                }
                //Classification</h1>
                if (ligne.Contains("<b>Location</b>"))
                {
                    sInfoToSearch = "location";
                    Console.WriteLine("DEBUG OSVDB Parsing " + sInfoToSearch);
                }
                if (ligne.Contains("<b>Attack Type</b>"))
                {
                    sInfoToSearch = "attacktype";
                    Console.WriteLine("DEBUG OSVDB Parsing " + sInfoToSearch);
                }
                if (ligne.Contains("<b>Impact</b>"))
                {
                    sInfoToSearch = "impact";
                    Console.WriteLine("DEBUG OSVDB Parsing " + sInfoToSearch);
                }
                if (ligne.Contains("<b>Solution</b>"))
                {
                    sInfoToSearch = "solution";
                    Console.WriteLine("DEBUG OSVDB Parsing " + sInfoToSearch);
                }
                if (ligne.Contains("<b>Exploit</b>"))
                {
                    sInfoToSearch = "exploit";
                    Console.WriteLine("DEBUG OSVDB Parsing " + sInfoToSearch);
                }
                if (ligne.Contains("<b>Disclosure</b>"))
                {
                    sInfoToSearch = "disclosure";
                    Console.WriteLine("DEBUG OSVDB Parsing " + sInfoToSearch);
                }
                if (ligne.Contains("<b>OSVDB</b>"))
                {
                    sInfoToSearch = "osvdb";
                    Console.WriteLine("DEBUG OSVDB Parsing " + sInfoToSearch);
                }
                if (ligne.Contains("Solution</h1>"))
                {
                    sInfoToSearch = "solutiondescription";
                    Console.WriteLine("DEBUG OSVDB Parsing " + sInfoToSearch);
                }
                if (ligne.Contains("Products</h1>"))
                {
                    sInfoToSearch = "products";
                    Console.WriteLine("DEBUG OSVDB Parsing " + sInfoToSearch);
                }
                if (ligne.Contains("References</h1>"))
                {
                    sInfoToSearch = "references";
                    Console.WriteLine("DEBUG OSVDB Parsing " + sInfoToSearch);
                }
                if (ligne.Contains("Credit</h1>"))
                {
                    sInfoToSearch = "credit";
                    Console.WriteLine("DEBUG OSVDB Parsing " + sInfoToSearch);
                }
                if (ligne.Contains("Score</h1>"))   //CVSSv2 Score
                {
                    sInfoToSearch = "score";
                    Console.WriteLine("DEBUG OSVDB Parsing " + sInfoToSearch);
                }
                if (ligne.Contains("Comments</h1>"))
                {
                    sInfoToSearch = "comments";
                    Console.WriteLine("DEBUG OSVDB Parsing " + sInfoToSearch);
                }

                ligne = monStreamReader.ReadLine();
            }
            monStreamReader.Close();
            Console.WriteLine("DEBUG OSVDB PARSED");
            
            
        }
    }
}
