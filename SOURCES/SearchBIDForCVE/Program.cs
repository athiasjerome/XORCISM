using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Text.RegularExpressions;
using System.IO;

namespace SearchBIDForCVE
{
    class Program
    {
        /// <summary>
        /// Copyright (C) 2014-2015 Jerome Athias
        /// *** DEPRECATED ***
        /// Tool retrieving Securityfocus.com identifiers (BIDs) matching a CVE
        /// This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation; either version 2 of the License, or (at your option) any later version.
        /// 
        /// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
        /// 
        /// You should have received a copy of the GNU General Public License along with this program; if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
        /// </summary>
        static void Main(string[] args)
        {
            //Request POST to http://www.securityfocus.com/bid
            string CVElook = "CVE-2011-3154";   //HARDCODED TEST
            string ResponseText = "";
            //string MyCookie = "";
            StreamReader SR = null;
            HttpWebResponse response = null;

            HttpWebRequest request;
            try
            {
                Console.WriteLine("DEBUG SearchBIDForCVE " + CVElook);
                string sURLSearchBIDForCVE = "http://www.securityfocus.com/bid";    //?CVE=" + CVElook; //HARDCODED
                Console.WriteLine("DEBUG Request to " + sURLSearchBIDForCVE);

                request = (HttpWebRequest)HttpWebRequest.Create(sURLSearchBIDForCVE);
                request.Method = "POST";
                string postData = "op=display_list&c=12&vendor=&title=&version=&CVE=" + CVElook;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();

                response = (HttpWebResponse)request.GetResponse();
                SR = new StreamReader(response.GetResponseStream());
                ResponseText = SR.ReadToEnd();

                //TODO
                //Console.WriteLine("DEBUG TODO ResponseText=" + ResponseText);

                /*
                <a href="/bid/50832"><span class="headline">Ubuntu Update Manager Insecure Temporary Directory Creation Vulnerability</span></a><br/>
	<span class="date">2011-11-28</span><br/>
	<a href="/bid/50832">http://www.securityfocus.com/bid/50832</a><br/><br/>
                */
                Regex myRegex = new Regex("<span class=\"headline\">(.*?)<br/><br/>",RegexOptions.Singleline);
                MatchCollection myBIDs = myRegex.Matches(ResponseText);
                foreach (Match matchBID in myBIDs)
                {
                    foreach (Capture capture01 in matchBID.Captures)
                    {
                        //Console.WriteLine(capture01.Value);

                        myRegex = new Regex("<span class=\"headline\">(.*?)</span>");
                        string sBIDTitle = myRegex.Match(capture01.Value).ToString();
                        sBIDTitle = sBIDTitle.Replace("<span class=\"headline\">", "");
                        sBIDTitle = sBIDTitle.Replace("</span>", "");
                        Console.WriteLine("DEBUG sBIDTitle=" + sBIDTitle);

                        myRegex = new Regex("<span class=\"date\">(.*?)</span>");
                        string sBIDDate = myRegex.Match(capture01.Value).ToString();
                        sBIDDate = sBIDDate.Replace("<span class=\"date\">", "");
                        sBIDDate = sBIDDate.Replace("</span>", "");
                        Console.WriteLine("DEBUG sBIDDate=" + sBIDDate);

                        myRegex = new Regex("bid/(.*?)\">");
                        string sBIDID = myRegex.Match(capture01.Value).ToString();
                        sBIDID = sBIDID.Replace("bid/", "");
                        sBIDID = sBIDID.Replace("\">", "");
                        Console.WriteLine("DEBUG sBIDID=" + sBIDID);

                    }
                }


                SR.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Exception = {0}. Retrying...", ex));
                //SearchBIDForCVE(CVElook, monStreamWriter);
                return;
            }
        }
    }
}
