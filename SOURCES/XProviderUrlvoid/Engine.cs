using System;
using System.Net;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Diagnostics;

using XORCISMModel;
using XCommon;
using XProviderCommon;

namespace XProviderUrlvoid
{
    /// <summary>
    /// Copyright (C) 2012-2015 Jerome Athias
    /// XORCISM Plugin for URLVoid
    /// All trademarks and registered trademarks are the property of their respective owners.
    /// This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation; either version 2 of the License, or (at your option) any later version.
    /// 
    /// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
    /// 
    /// You should have received a copy of the GNU General Public License along with this program; if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
    /// </summary>
    public class Engine : MarshalByRefObject, IBlacklistedDetector
    {
        public Engine()
        {
            TextWriterTraceListener tw;
            tw = new TextWriterTraceListener("XProviderUrlvoid.log");   //Hardcoded

            Trace.Listeners.Add(tw);
            Trace.AutoFlush = true;
            Trace.IndentSize = 4;
        }
        public void Run(string target, int jobID)
        {
            Utils.Helper_Trace("XORCISM PROVIDER URLVOID", "Entering Run()");

            Utils.Helper_Trace("XORCISM PROVIDER URLVOID", string.Format("Target = {0}", target));
            target = target.Replace("http://", "");
            target = target.Replace("https://", "");
            target = target.Replace("www.", "");
            Utils.Helper_Trace("XORCISM PROVIDER URLVOID", string.Format("Clean Target = {0}", target));

            string MD5domain = HashMD5(target);

            Utils.Helper_Trace("XORCISM PROVIDER URLVOID", string.Format("TargetMD5 = {0}", MD5domain));

            string url = "http://api.urlvoid.com/api.php?key=12345&domain=" + MD5domain;    //Hardcoded

            string ResponseText = "";
            StreamReader SR = null;
            HttpWebResponse response = null;
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "GET";
            response = (HttpWebResponse)request.GetResponse();
            SR = new StreamReader(response.GetResponseStream());
            ResponseText = SR.ReadToEnd();

            Utils.Helper_Trace("XORCISM PROVIDER URLVOID", string.Format("Response status : [{0}]", response.StatusCode + " - " + response.StatusDescription));
            Utils.Helper_Trace("XORCISM PROVIDER URLVOID", string.Format("Response headers : [{0}]", response.Headers.ToString()));
            Utils.Helper_Trace("XORCISM PROVIDER URLVOID", string.Format("Response received : [{0}]", ResponseText));
            //[U]   domain or subdomain has not yet been scanned in URLVoid
            //[4]

            Utils.Helper_Trace("XORCISM PROVIDER URLVOID", "Updating job status to FINISHED");

            XORCISMEntities model = new XORCISMEntities();
            var xJob = from j in model.JOB
                       where j.JobID == jobID
                       select j;

            JOB xJ = xJob.FirstOrDefault();
            xJ.Status = XCommon.STATUS.FINISHED.ToString();
            xJ.DateEnd = DateTime.Now;
            Utils.Helper_Trace("XORCISM PROVIDER URLVOID", "Job Finished at " + xJ.DateEnd.ToString());
            model.SaveChanges();

            Utils.Helper_Trace("XORCISM PROVIDER URLVOID", "Leaving Run()");
        }

        private string HashMD5(string Value)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.ASCII.GetBytes(Value);
            data = x.ComputeHash(data);
            string ret = "";
            for (int i = 0; i < data.Length; i++)
                ret += data[i].ToString("x2").ToLower();
            return ret;

            /*
                byte[] data = new MD5CryptoServiceProvider().ComputeHash(Encoding.ASCII.GetBytes(value));
                StringBuilder hashedString = new StringBuilder();
                for (int i = 0; i < data.Length; i )
                hashedString.Append(data[i].ToString("x2"));
                return hashedString.ToString();
            */
        }
    }
}
