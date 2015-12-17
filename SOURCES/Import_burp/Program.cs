using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;   //XmlDocument

namespace Import_burp
{
    class Program
    {
        /// <summary>
        /// Copyright (C) 2014-2015 Jerome Athias
        /// *** SKELETON ONLY (NOT WORKING) ***
        /// Importer for Burp Suite results into an XORCISM database
        /// This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation; either version 2 of the License, or (at your option) any later version.
        /// 
        /// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
        /// 
        /// You should have received a copy of the GNU General Public License along with this program; if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
        /// </summary>
        static void Main(string[] args)
        {
            XmlDocument doc;
            doc = new XmlDocument();

            //NOTE: probably not the best/fastest (XmlReader) way to parse XML but easy/clear enough

            try
            {
                doc.Load(@"burp_output.xml");
            }
            catch (Exception exdocLoad)
            {
                Console.WriteLine("Exception exdocLoad :\n" + exdocLoad.Message + " " + exdocLoad.InnerException);
                return;
            }

            XmlNodeList nodeIssues = doc.SelectNodes("issues/issue");
            //TODO: burpVersion exportTime
            int iCptIssue = 0;
            foreach (XmlNode nodeIssue in nodeIssues)    //issue
            {
                iCptIssue++;
                foreach (XmlNode nodeIssueInfo in nodeIssue)
                {
                    //Console.WriteLine("DEBUG " + nodeIssueInfo.Name);
                    switch(nodeIssueInfo.Name)
                    {
                        case "serialNumber":
                            Console.WriteLine("DEBUG serialNumber=" + nodeIssueInfo.InnerText);
                            break;
                        case "type":
                            //5245344
                            //5243392
                            break;
                        case "name":
                            //Frameable response (potential Clickjacking)
                            if (nodeIssueInfo.InnerText.ToLower().Contains("clickjacking"))
                            {
                                Console.WriteLine("DEBUG CAPEC-103");
                            }
                            //SSL cookie without secure flag set

                            break;
                        case "host":
                            string sIPAddressIPv4 = nodeIssueInfo.Attributes["ip"].InnerText;
                            Console.WriteLine("DEBUG sIPAddressIPv4=" + sIPAddressIPv4);

                            string sURL = nodeIssueInfo.InnerText;
                            Console.WriteLine("DEBUG sURL=" + sURL);
                            break;
                        case "path":
                            Console.WriteLine("DEBUG path=" + nodeIssueInfo.InnerText);
                            break;
                        case "location":

                            break;
                        case "severity":

                            break;
                        case "confidence":

                            break;
                        case "issueBackground":

                            break;
                        case "remediationBackground":

                            break;
                        case "issueDetail":

                            break;
                        case "requestresponse":

                            break;
                        
                        default:
                            Console.WriteLine("ERROR Missing code for " + nodeIssueInfo.Name);
                            break;
                    }
                }
            }
        }
    }
}
