using System;
using System.Net;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Serialization;
using System.Configuration;
using System.Threading;
using System.Diagnostics;
using System.Reflection;

using System.Text.RegularExpressions;

using XCommon;
using XProviderCommon;
using XORCISMModel;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace XProviderIviz
{
    /// <summary>
    /// Copyright (C) 2012-2015 Jerome Athias
    /// Iviz plugin for XORCISM
    /// All trademarks and registered trademarks are the property of their respective owners.
    /// This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation; either version 2 of the License, or (at your option) any later version.
    /// 
    /// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
    /// 
    /// You should have received a copy of the GNU General Public License along with this program; if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
    /// </summary>
    public class Engine : MarshalByRefObject, XProviderCommon.IVulnerabilityDetector
    {
        public Engine()
        {
            TextWriterTraceListener tw;
            tw = new TextWriterTraceListener("XProviderIviz.log");  //Hardcoded

            Trace.AutoFlush = true;
            Trace.IndentSize = 4;
            Trace.Listeners.Add(tw);
        }

        public void Run(string target, int jobID, string policy, string strategy)
        {
            Utils.Helper_Trace("XORCISM PROVIDER IVIZ", "Entering Run()");
            Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("Target = {0} , JobID = {1} , Policy = {2}, Strategy = {3}", target, jobID, policy, strategy));

            IVIZParser IvizParser = new IVIZParser(target, jobID, policy, strategy);

            IvizParser.DoIt(jobID);

            Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("Updating job {0} status to FINISHED", jobID));

            IvizParser.UpdateJob(jobID);

            Utils.Helper_Trace("XORCISM PROVIDER IVIZ", "Leaving Run()");
        }

        /*
        private VULNERABILITYFOUND PersisteVuln(string cve, string IVIZID, XmlNode diag, XmlNode consequence, XmlNode solution, int endPointID, string severity)
        {
            return null;
        }
        */

        private int SearchForIVIZID(string IVIZID)
        {
            return -1;
        }

        class IVIZParser
        {
            private string m_target;
            private int m_jobId;
            private string m_policy;
            private string m_data;
            private string m_strategy;

            public IVIZParser(string target, int jobID, string policy, string strategy)
            {
                m_target = target;
                m_jobId = jobID;
                m_policy = policy;
                m_strategy = strategy;
            }

            public void DoIt(int jobID)
            {
                Assembly a;
                a = Assembly.GetExecutingAssembly();

                XORCISMEntities model;
                model = new XORCISMEntities();

                Utils.Helper_Trace("XORCISM PROVIDER IVIZ", "Assembly location = " + a.Location);

                string URI = string.Empty;
                string UrlScan = string.Empty;
                string APIURL = string.Empty;

                //  http://www.csharpfr.com/code.aspx?ID=31186

                //string APIURL = "https://sandbox.ivizsecurity.com/apilogin?Login=testing@ivizsecurity.com&Password=changeme";
                HttpWebRequest request;
                //Stream newStream;
                string ResponseText = "";
                string MyCookie = "";
                StreamReader SR = null;
                HttpWebResponse response = null;
                Dictionary<string, string> Cookies = new Dictionary<string, string>();
                String cookieString = "";

                #region LOGIN ADMINISTRATOR

                cookieString=LoginAdministrator();
                
                #endregion

                //******************************************************************************************************************************************************
                
                #region USERS
                /*
                APIURL = "https://sandbox.ivizsecurity.com/users.xml";
                Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("Trying to retrieve the list of users"));

                //ASCIIEncoding encoding = new ASCIIEncoding();
                //string data = "Login=user@xorcism.org&Password=changeme";
                //byte[] Content = encoding.GetBytes(data);
                //HttpWebRequest request;
                //Stream newStream;
                //string ResponseText = "";
                SR = null;
                response = null;
                try
                {
                    
                    //  Voir:   http://www.codeproject.com/KB/IP/httpwebrequest_response.aspx
                    
                    //NameValueCollection collHeader = new NameValueCollection();
                    //if (MyCookie.Length > 0)
                    //{
                    //    collHeader.Add("Cookie", MyCookie);
                    //}
                    

                    request = (HttpWebRequest)HttpWebRequest.Create(APIURL);
                    //HttpWebRequest webrequest = CreateWebRequest(APIURL, collHeader, RequestMethod, NwCred);

                    //ServicePointManager.CertificatePolicy = new AcceptAllCertificatePolicy();
                    ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate);

                    request.Method = "GET";
                    request.ContentType = "application/xml";

                    if (cookieString.Length > 2)
                    {
                        String cookie = cookieString.Substring(0, cookieString.Length - 1);
                        request.Headers.Add("Cookie", cookie);
                    }

                    //request.ContentLength = data.Length;
                    response = (HttpWebResponse)request.GetResponse();
                    SR = new StreamReader(response.GetResponseStream());
                    ResponseText = SR.ReadToEnd();

                    Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("ListUsers response status : [{0}]", response.StatusCode + " - " + response.StatusDescription));
                    Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("ListUsers response headers : [{0}]", response.Headers.ToString()));
                    Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("ListUsers response received : [{0}]", ResponseText));
                }
                catch (Exception ex)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("Exception HttpWebRequestLISTUSERS = {0}", ex));
                }
                //XmlDocument doc = new XmlDocument();
                //doc.LoadXml(ResponseText);
                SR.Close();

                */
                #endregion


                //******************************************************************************************************************************************************

                #region SERVICES
                
                APIURL = "https://sandbox.ivizsecurity.com/customers/2/service_instances.xml";  //Hardcoded
                Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("Trying to retrieve the list of services"));

                SR = null;
                response = null;
                try
                {
                    request = (HttpWebRequest)HttpWebRequest.Create(APIURL);
                    
                    //ServicePointManager.CertificatePolicy = new AcceptAllCertificatePolicy();
                    ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate);

                    request.Method = "GET";
                    request.ContentType = "application/xml";
                    /*
                    if (MyCookie.Length > 0)
                    {
                        request.Headers.Add("Cookie", MyCookie);
                    }
                    */
                    if (cookieString.Length > 2)
                    {
                        String cookie = cookieString.Substring(0, cookieString.Length - 1);
                        Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("cookie = {0}", cookie));
                        request.Headers.Add("Cookie", cookie);
                    }
                    response = (HttpWebResponse)request.GetResponse();
                    SR = new StreamReader(response.GetResponseStream());
                    ResponseText = SR.ReadToEnd();

                    Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("ListServices response status : [{0}]", response.StatusCode + " - " + response.StatusDescription));
                    Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("ListServices response headers : [{0}]", response.Headers.ToString()));
                    Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("ListServices response received : [{0}]", ResponseText));
                }
                catch (Exception ex)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("Exception HttpWebRequestLISTSERVICES = {0}", ex));
                }
                //XmlDocument doc = new XmlDocument();
                //doc.LoadXml(ResponseText);
                SR.Close();
                
                
                #endregion


                //******************************************************************************************************************************************************

                #region REGISTERSERVICE
                /*
                APIURL = "https://sandbox.ivizsecurity.com/customers/2/service_instances/"; //Hardcoded
                Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("Trying to add a new service"));

                SR = null;
                response = null;
                try
                {
                    request = (HttpWebRequest)HttpWebRequest.Create(APIURL);
                    //request.Headers.Add("Login", "testing@ivizsecurity.com");
                    //request.Headers.Add("Password", "changeme");
                    //ServicePointManager.CertificatePolicy = new AcceptAllCertificatePolicy();
                    ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate);

                    request.Method = "POST";
                    request.ContentType = "application/xml";
                    string postData = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                    //scanner-type  100 Network scan
                    //scanner-type  400 Application scan
                    postData += "<service><scan_limits><customer-id type=\"integer\">2</customer-id><scan-limit type=\"string\">25</scan-limit><scanner-type type=\"integer\">100</scanner-type><valid-upto type=\"datetime\">2012-12-12T00:00:00Z</valid-upto></scan_limits><service-type type=\"integer\">100</service-type><targets type=\"integer\">20</targets></service>";
                    byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                    request.ContentLength = byteArray.Length;
                    Stream dataStream = request.GetRequestStream();
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();
                    if (MyCookie.Length > 0)
                    {
                        request.Headers.Add("Cookie", MyCookie);
                    }

                    response = (HttpWebResponse)request.GetResponse();
                    SR = new StreamReader(response.GetResponseStream());
                    ResponseText = SR.ReadToEnd();

                    Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("RegisterService response status : [{0}]", response.StatusCode + " - " + response.StatusDescription));
                    Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("RegisterService response headers : [{0}]", response.Headers.ToString()));
                    Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("RegisterService response received : [{0}]", ResponseText));
                }
                catch (Exception ex)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("Exception HttpWebRequestREGISTERSERVICE = {0}", ex));
                }
                //XmlDocument doc = new XmlDocument();
                //doc.LoadXml(ResponseText);
                SR.Close();

                */
                #endregion

                //******************************************************************************************************************************************************

                #region ASSETS

                Thread.Sleep(5000);
                APIURL = "https://sandbox.ivizsecurity.com/customers/registered/2.xml"; //Hardcoded
                Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("Trying to retrieve the list of assets"));

                SR = null;
                response = null;
                try
                {
                    request = (HttpWebRequest)HttpWebRequest.Create(APIURL);
                    
                    //ServicePointManager.CertificatePolicy = new AcceptAllCertificatePolicy();
                    ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate);

                    request.Method = "GET";
                    request.ContentType = "application/xml";

                    /*
                    if (MyCookie.Length > 0)
                    {
                        request.Headers.Add("Cookie", MyCookie);
                    }
                    */
                    if (cookieString.Length > 2)
                    {
                        String cookie = cookieString.Substring(0, cookieString.Length - 1);
                        request.Headers.Add("Cookie", cookie);
                    }
                    response = (HttpWebResponse)request.GetResponse();
                    SR = new StreamReader(response.GetResponseStream());
                    ResponseText = SR.ReadToEnd();

                    Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("ListAssets response status : [{0}]", response.StatusCode + " - " + response.StatusDescription));
                    Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("ListAssets response headers : [{0}]", response.Headers.ToString()));
                    Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("ListAssets response received : [{0}]", ResponseText));
                }
                catch (Exception ex)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("Exception HttpWebRequestLISTASSETS = {0}", ex));
                }

                SR.Close();

                //Checking if the asset already exists
                /*
                XmlDocument doc = new XmlDocument();
                try
                {
                    //TODO: Input Validation (XML)
                    doc.LoadXml(ResponseText);
                }
                catch (Exception ex)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("Exception = {0} / {1}", ex.Message, ex.InnerException == null ? "" : ex.InnerException.Message));
                }
                */
                /*
                <?xml version="1.0" encoding="UTF-8"?>
                <Registered Assets type="array">
                  <Registered Asset>
                    <address>http://192.168.0.35</address>
                    <asset-type type="integer">1000</asset-type>
                    <created-at type="datetime">2010-09-30T05:59:22Z</created-at>
                    <customer-id type="integer">2</customer-id>
                    <id type="integer">1</id>
                    <state>authorized</state>
                  </Registered Asset>
                  <Registered Asset>
                    <address>192.168.2.209</address>
                    <asset-type type="integer">100</asset-type>
                    <created-at type="datetime">2010-12-22T11:07:26Z</created-at>
                    <customer-id type="integer">2</customer-id>
                    <id type="integer">3</id>
                    <state>authorized</state>
                  </Registered Asset>
                  <Registered Asset>
                    <address>http://www.target.com</address>
                    <asset-type type="integer">1000</asset-type>
                    <created-at type="datetime">2011-02-07T13:18:25Z</created-at>
                    <customer-id type="integer">2</customer-id>
                    <id type="integer">4</id>
                    <state>authorized</state>
                  </Registered Asset>
                </Registered Assets>
                */
                bool registerasset = true;
                Regex RegexAddress = new Regex("<address>[^<>]*</address>");
                MatchCollection myAssets = RegexAddress.Matches(ResponseText);
                foreach (Match match in myAssets)
                {
                    foreach (Capture capture in match.Captures)
                    {
                        Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("Index={0}, Asset={1}", capture.Index, capture.Value));
                        string curAsset = capture.Value.Replace("<address>", "");
                        curAsset = curAsset.Replace("</address>", "");
                        if (curAsset == m_target)
                        {
                            registerasset = false;
                        }
                    }
                }

                /*
                string query = "/Registered Assets/Registered Asset";   //Hardcoded
                
                XmlNodeList myAssets;
                myAssets = null;
                try
                {
                    myAssets = doc.SelectNodes(query);
                }
                catch (Exception ex)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("Error SelectNodes({0}) : Exception = {1}", query, ex.Message));
                }
                
                foreach (XmlNode registeredAsset in myAssets)
                {
                    foreach (XmlNode n in registeredAsset.ChildNodes)
                    {
                        XmlNodeList Childs = n.ChildNodes;
                        string curAsset=HelperGetChildInnerText(n, "address");
                        if (curAsset == m_target)
                        {
                            registerasset = false;
                        }
                    }
                }
                */


                #endregion

                //******************************************************************************************************************************************************

                #region REGISTERASSET
                
                if (registerasset)
                {
                    APIURL = "https://sandbox.ivizsecurity.com/assets"; //Hardcoded
                    Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("Trying to add a new asset"));

                    SR = null;
                    response = null;
                    try
                    {
                        request = (HttpWebRequest)HttpWebRequest.Create(APIURL);
                        //request.Headers.Add("Login", "testing@ivizsecurity.com");
                        //request.Headers.Add("Password", "changeme");
                        //ServicePointManager.CertificatePolicy = new AcceptAllCertificatePolicy();
                        ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate);

                        request.Method = "POST";
                        request.ContentType = "application/xml";
                        string postData = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                        //asset-type    100     Network asset
                        //asset-type    1000    Web asset
                        //HARDCODED
                        if (IsValidIP(m_target))
                        {
                            postData += "<asset><address>" + m_target + "</address><address-type type=\"integer\">100</address-type><asset-type type=\"integer\">100</asset-type><customer-id type=\"integer\">2</customer-id><name>" +m_target+ "</name><state>authorized</state><user-id type=\"integer\">5</user-id></asset>";
                        }
                        else
                        {
                            postData += "<asset><address>" + m_target + "</address><address-type type=\"integer\">1000</address-type><asset-type type=\"integer\">1000</asset-type><customer-id type=\"integer\">2</customer-id><name>" +m_target+ "</name><state>authorized</state><user-id type=\"integer\">5</user-id></asset>";
                        }
                        byte[] byteArray = Encoding.ASCII.GetBytes(postData);
                        request.ContentLength = byteArray.Length;

                        //if (MyCookie.Length > 0)
                        //{
                        //    request.Headers.Add("Cookie", MyCookie);
                        //}

                        if (cookieString.Length > 2)
                        {
                            String cookie = cookieString.Substring(0, cookieString.Length - 1);
                            Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("cookie = {0}", cookie));
                            request.Headers.Add("Cookie", cookie);
                        }
                        Stream dataStream = request.GetRequestStream();
                        dataStream.Write(byteArray, 0, byteArray.Length);
                        dataStream.Close();


                        response = (HttpWebResponse)request.GetResponse();
                        SR = new StreamReader(response.GetResponseStream());
                        ResponseText = SR.ReadToEnd();

                        Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("RegisterAsset response status : [{0}]", response.StatusCode + " - " + response.StatusDescription));
                        Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("RegisterAsset response headers : [{0}]", response.Headers.ToString()));
                        Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("RegisterAsset response ContentType : [{0}]", response.ContentType));
                        Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("RegisterAsset response received : [{0}]", ResponseText));
                        /*
                            <?xml version="1.0" encoding="UTF-8"?>
                            <Asset type="array">
                              <Asset>
                                <id type="integer">6</id>
                                <address>91.121.122.10</address>
                              </Asset>
                            </Asset>
                        */
                    }
                    catch (Exception ex)
                    {
                        Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("Exception HttpWebRequestREGISTERASSET = {0}", ex));
                    }
                    //XmlDocument doc = new XmlDocument();
                    //doc.LoadXml(ResponseText);
                    SR.Close();
                    Thread.Sleep(5000);
                }

                #endregion


                //******************************************************************************************************************************************************
                //******************************************************************************************************************************************************

                #region LOGINUSER

                Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("Trying to login as User"));

                APIURL = "https://sandbox.ivizsecurity.com/apilogin";
                
                ResponseText = "";
                MyCookie = "";
                
                try
                {
                    request = (HttpWebRequest)HttpWebRequest.Create(APIURL);
                    
                    //USER
                    request.Headers.Add("Login", "test@ivizsecurity.com");  //Hardcoded
                    request.Headers.Add("Password", "changeme");    //Hardcoded
                    
                    ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate);

                    request.Method = "GET";
                    request.ContentType = "application/xml";
                    
                    response = (HttpWebResponse)request.GetResponse();
                    SR = new StreamReader(response.GetResponseStream());
                    ResponseText = SR.ReadToEnd();

                    Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("Login response status : [{0}]", response.StatusCode + " - " + response.StatusDescription));
                    Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("Login response headers : [{0}]", response.Headers.ToString()));
                    Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("Login response received : [{0}]", ResponseText));

                    WebHeaderCollection headers = response.Headers;
                    /*
                    if (headers["Set-Cookie"] != null)
                    {
                        MyCookie = headers["Set-Cookie"];
                        Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("Cookie received : [{0}]", MyCookie));
                    }
                    */
                    // Tokenize cookies
                    cookieString = "";
                    Cookies = new Dictionary<string, string>();
                    if (response.Headers["Set-Cookie"] != null)
                    {
                        String strheaders = response.Headers["Set-Cookie"].Replace("path=/,", ";").Replace("HttpOnly,", "");
                        foreach (String cookie in strheaders.Split(';'))
                        {
                            if (cookie.Contains("="))
                            {
                                String[] splitCookie = cookie.Split('=');
                                String cookieKey = splitCookie[0].Trim();
                                String cookieValue = splitCookie[1].Trim();

                                if (Cookies.ContainsKey(cookieKey))
                                    Cookies[cookieKey] = cookieValue;
                                else
                                    Cookies.Add(cookieKey, cookieValue);
                            }
                            else
                            {
                                if (Cookies.ContainsKey(cookie))
                                    Cookies[cookie] = "";
                                else
                                    Cookies.Add(cookie, "");
                            }
                        }

                        foreach (KeyValuePair<String, String> cookiePair in Cookies)
                            cookieString += cookiePair.Key + "=" + cookiePair.Value + ";";
                    }
                    Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("cookieString = {0}", cookieString));
                }
                catch (Exception ex)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("Exception HttpWebRequestLOGINUSER = {0}", ex));
                }
                SR.Close();
                

                #endregion

                #region CREATESCAN

                Thread.Sleep(5000); //Hardcoded
                string myscanID = CreateScan(cookieString, m_target);
                
                #endregion

                //*********************************************************************************************************************************
                #region WAIT UNTIL THE SCAN IS FINISHED

                Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("Waiting until the scan finishes"));

                WaitingScan(cookieString, myscanID);

                #endregion

                //*********************************************************************************************************************************
                #region DOWNLOADS REPORT

                Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("Downloading the report"));

                DownloadReport(cookieString, myscanID);

                #endregion

            }

            private string LoginAdministrator()
            {
                Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("Trying to login as Administrator"));

                string APIURL = "https://sandbox.ivizsecurity.com/apilogin";    //Hardcoded

                /*
                HttpClientCertificate cert = Request.ClientCertificate;
                if (cert.IsPresent)
                    certDataLabel.Text = cert.Get("SUBJECT O");
                else
                    certDataLabel.Text = "No certificate was found.";
                */

                //ASCIIEncoding encoding = new ASCIIEncoding();
                //string data = "Login=user@xorcism.org&Password=changeme"; //Hardcoded
                //byte[] Content = encoding.GetBytes(data);
                HttpWebRequest request;
                //Stream newStream;
                string ResponseText = "";
                //string MyCookie = "";
                StreamReader SR = null;
                HttpWebResponse response = null;
                Dictionary<string, string> Cookies = new Dictionary<string, string>();
                String cookieString = "";

                try
                {
                    // Set a default policy level for the "http:" and "https" schemes.
                    //HttpWebRequestCachePolicy policy = new HttpRequestCachePolicy(HttpRequestCacheLevel.Default);
                    //HttpWebRequest.DefaultCachePolicy = policy;

                    request = (HttpWebRequest)HttpWebRequest.Create(APIURL);

                    //Admin
                    request.Headers.Add("Login", "testing@ivizsecurity.com");   //Hardcoded
                    request.Headers.Add("Password", "changeme");    //Hardcoded

                    /*
                    //USER
                    request.Headers.Add("Login", "test@ivizsecurity.com");
                    request.Headers.Add("Password", "changeme");
                    */

                    //ServicePointManager.CertificatePolicy = new AcceptAllCertificatePolicy();
                    ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate);

                    request.Method = "GET";
                    request.ContentType = "application/xml";
                    //request.ContentLength = data.Length;
                    /*
                    CredentialCache wrCache = new CredentialCache();
                    wrCache.Add(new Uri(APIURL), "Basic", new NetworkCredential("testing@ivizsecurity.com", "changeme"));
                    request.Credentials = wrCache;
                    */

                    response = (HttpWebResponse)request.GetResponse();
                    SR = new StreamReader(response.GetResponseStream());
                    ResponseText = SR.ReadToEnd();

                    Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("Login response status : [{0}]", response.StatusCode + " - " + response.StatusDescription));
                    Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("Login response headers : [{0}]", response.Headers.ToString()));
                    Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("Login response received : [{0}]", ResponseText));

                    WebHeaderCollection headers = response.Headers;
                    /*
                    if (headers["Set-Cookie"] != null)
                    {
                        MyCookie = headers["Set-Cookie"];
                        Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("Cookie received : [{0}]", MyCookie));
                    }
                    */
                    // Tokenize cookies
                    cookieString = "";
                    Cookies = new Dictionary<string, string>();
                    if (response.Headers["Set-Cookie"] != null)
                    {
                        String strheaders = response.Headers["Set-Cookie"].Replace("path=/,", ";").Replace("HttpOnly,", "");
                        foreach (String cookie in strheaders.Split(';'))
                        {
                            if (cookie.Contains("="))
                            {
                                String[] splitCookie = cookie.Split('=');
                                String cookieKey = splitCookie[0].Trim();
                                String cookieValue = splitCookie[1].Trim();

                                if (Cookies.ContainsKey(cookieKey))
                                    Cookies[cookieKey] = cookieValue;
                                else
                                    Cookies.Add(cookieKey, cookieValue);
                            }
                            else
                            {
                                if (Cookies.ContainsKey(cookie))
                                    Cookies[cookie] = "";
                                else
                                    Cookies.Add(cookie, "");
                            }
                        }

                        foreach (KeyValuePair<String, String> cookiePair in Cookies)
                            cookieString += cookiePair.Key + "=" + cookiePair.Value + ";";
                    }
                    Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("cookieString = {0}", cookieString));
                }
                catch (Exception ex)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("Exception HttpWebRequestLOGIN = {0}", ex));
                    //System.Net.WebException: The operation has timed out
                    Regex RegexErrorLogin = new Regex("The operation has timed out");
                    string strTemp = RegexErrorLogin.Match(ex.ToString()).ToString();
                    if (strTemp != "")
                    {
                        Utils.Helper_Trace("XORCISM PROVIDER IVIZ", "ERROR TIME OUT: Retrying...");
                        Thread.Sleep(5000);
                        LoginAdministrator();
                        return "";
                    }
                }
                //XmlDocument doc = new XmlDocument();
                //doc.LoadXml(ResponseText);
                SR.Close();
                return cookieString;
            }

            //*****************************************************************
            private string CreateScan(string cookieString, string mytarget)
            {
                Thread.Sleep(5000);

                string ResponseText = "";
                StreamReader SR = null;
                HttpWebResponse response = null;
                HttpWebRequest request;
                
                string APIURL = "https://sandbox.ivizsecurity.com/scans";   //Hardcoded
                Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("Trying to launch a new scan"));

                SR = null;
                response = null;
                try
                {
                    request = (HttpWebRequest)HttpWebRequest.Create(APIURL);
                    //request.Headers.Add("Login", "testing@ivizsecurity.com");
                    //request.Headers.Add("Password", "changeme");
                //    request.Headers.Add("Login", "test@ivizsecurity.com");
                //    request.Headers.Add("Password", "changeme");
                    //ServicePointManager.CertificatePolicy = new AcceptAllCertificatePolicy();
                    ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate);

                    string MyDate = DateTime.Now.ToString("yyyy-MM-dd");
                    string MyMonth = DateTime.Now.ToString("MM");
                    string MyDay = DateTime.Now.ToString("dd");
                    request.Method = "POST";
                    request.ContentType = "application/xml";
                    /*
                    if (MyCookie.Length > 0)
                    {
                        request.Headers.Add("Cookie", MyCookie);
                    }
                    */
                    if (cookieString.Length > 2)
                    {
                        String cookie = cookieString.Substring(0, cookieString.Length - 1);
                        Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("cookie = {0}", cookie));
                        request.Headers.Add("Cookie", cookie);
                    }
                    string postData = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                    //Regex objNaturalPattern = new Regex("http");
                    //string strTemp = objNaturalPattern.Match(mytarget).ToString();
                    //if (strTemp != "")
                    if(!IsValidIP(mytarget))
                    {
                        //serviceID
                        //1         Premium Web Application Assessment
                        //2         Basic Web Application Assessment
                        //3         Standard Web Application Assessment
                        //4         Premium Network Assessment
                        //5         Standard Network Assessment
                        //6         Basic Network Assessment

                        //1) 1-65,535 2) Most common ports and 3) specific port.

                        //HARDCODED
                        postData += "<scan><start_date type=\"datetime\">" + MyDate + "</start_date><end_date_monthly></end_date_monthly><end_date_weekly></end_date_weekly><date>" + MyDate + "</date><time><hour>12</hour><time_zone>UTC</time_zone><month_date>" + MyDay + "</month_date></time><service_instance_id>2</service_instance_id><targets>" + mytarget + "</targets><week_interval>1</week_interval><recurring>onetime</recurring><url></url><username></username><additional_server></additional_server><malware_domains></malware_domains><password></password><include_path></include_path><comments></comments><port_scan_length>1</port_scan_length><depth_limit></depth_limit><exclude_path></exclude_path></scan>";
                      //  postData += "<scan><start_date type=\"datetime\">" + MyDate + "</start_date><end_date_monthly></end_date_monthly><end_date_weekly></end_date_weekly><date>" + MyDate + "</date><time><hour>12</hour><time_zone>UTC</time_zone><month_date>" + MyDay + "</month_date></time><service_instance_id>1</service_instance_id><targets>" + mytarget + "</targets><week_interval></week_interval><recurring>onetime</recurring><url></url><username></username><additional_server></additional_server><malware_domains></malware_domains><password></password><include_path></include_path><comments></comments><port_scan_length></port_scan_length><depth_limit></depth_limit><exclude_path></exclude_path></scan>";
                    //postData = "<?xml version=\"1.0\" encoding=\"UTF-8\"?> <scan> <start_date type=\"datetime\">2011-04-19</start_date> <end_date_monthly></end_date_monthly> <end_date_weekly></end_date_weekly> <date>2011-04-19</date> <time> <hour>12</hour> <time_zone>UTC</time_zone> <month_date>31</month_date> </time> <service_instance_id>4</service_instance_id> <targets>demo.testfire.org</targets> <week_interval>1</week_interval> <recurring>onetime</recurring> <url></url> <username></username> <additional_server></additional_server> <malware_domains></malware_domains> <password></password> <include_path></include_path> <comments></comments> <port_scan_length></port_scan_length> <depth_limit></depth_limit> <exclude_path></exclude_path> </scan>";
                    }
                    else
                    {
                        //HARDCODED
                        postData += "<scan><start_date type=\"datetime\">" + MyDate + "</start_date><end_date_monthly></end_date_monthly><end_date_weekly></end_date_weekly><date>" + MyDate + "</date><time><hour>12</hour><time_zone>UTC</time_zone><month_date>" + MyDay + "</month_date></time><service_instance_id>6</service_instance_id><targets>" + mytarget + "</targets><week_interval>1</week_interval><recurring>onetime</recurring><url></url><username></username><additional_server></additional_server><malware_domains></malware_domains><password></password><include_path></include_path><comments></comments><port_scan_length>1</port_scan_length><depth_limit></depth_limit><exclude_path></exclude_path></scan>";
                        //postData += "<scan><start_date type=\"datetime\">" + MyDate + "</start_date><end_date_monthly></end_date_monthly><end_date_weekly></end_date_weekly><date>" + MyDate + "</date><time><hour>12</hour><time_zone>UTC</time_zone><month_date>" + MyDay + "</month_date></time><service_instance_id>4</service_instance_id><targets>" + mytarget + "</targets><week_interval></week_interval><recurring>onetime</recurring><url></url><username></username><additional_server></additional_server><malware_domains></malware_domains><password></password><include_path></include_path><comments></comments><port_scan_length></port_scan_length><depth_limit></depth_limit><exclude_path></exclude_path></scan>";
                    }
                    Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("CreateScan postData= {0}", postData));
                    byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                    request.ContentLength = byteArray.Length;
                                        
                    Stream dataStream = request.GetRequestStream();
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();


                    response = (HttpWebResponse)request.GetResponse();
                    SR = new StreamReader(response.GetResponseStream());
                    ResponseText = SR.ReadToEnd();

                    Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("CreateScan response status : [{0}]", response.StatusCode + " - " + response.StatusDescription));
                    Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("CreateScan response headers : [{0}]", response.Headers.ToString()));
                    Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("CreateScan response received : [{0}]", ResponseText));
                    /*
                        <?xml version="1.0" encoding="UTF-8"?>
                        <Scan>
                          <id type="integer">7</id>
                          <service-instance-id type="integer">1</service-instance-id>
                        </Scan>
                    */

                    //Invalid Input / Service does not belong to you

                    //[port_scan_length: Provide Valid Port Scan Length

                    //Status: 500 Internal Server Error
                    //Content-Type: text/html

                    //<html><body><h1>500 Internal Server Error</h1></body></html>
                    Regex objNaturalPattern = new Regex("500 Internal Server Error");   //Hardcoded
                    string strTemp = objNaturalPattern.Match(ResponseText).ToString();
                    if (strTemp != "")
                    {
                        Utils.Helper_Trace("XORCISM PROVIDER IVIZ", "ERROR 500: Retrying the request...");
                        CreateScan(cookieString, m_target);
                        return "";
                    }

                }
                catch (Exception ex)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("Exception HttpWebRequestCREATESCAN = {0}", ex));
                }



                XmlDocument doc = new XmlDocument();
                //TODO: Input Validation (XML)
                doc.LoadXml(ResponseText);
                SR.Close();

                string query = "/Scan"; //Hardcoded
                XmlNode scan;
                scan = null;
                try
                {
                    scan = doc.SelectSingleNode(query);
                }
                catch (Exception ex)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("Error SelectNodes({0}) : Exception = {1}", query, ex.Message));
                }
                string scanID = HelperGetChildInnerText(scan, "id");
                Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("scanID = {0}", scanID));
                return scanID;
            }


            private void WaitingScan(string cookieString, string myscanID)
            {
                Thread.Sleep(60000);    //Hardcoded

                string APIURL = "https://sandbox.ivizsecurity.com/scans_status/" + myscanID + ".xml";   //Hardcoded

                bool NotYet = true;
                while (NotYet)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("Polling..."));
                    string ResponseText = "";
                    StreamReader SR = null;
                    HttpWebResponse response = null;
                    HttpWebRequest request;
                    
                    try
                    {
                        request = (HttpWebRequest)HttpWebRequest.Create(APIURL);

                        //ServicePointManager.CertificatePolicy = new AcceptAllCertificatePolicy();
                        ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate);

                        request.Method = "GET";
                        request.ContentType = "application/xml";
                        /*
                        if (MyCookie.Length > 0)
                        {
                            request.Headers.Add("Cookie", MyCookie);
                        }
                        */
                        if (cookieString.Length > 2)
                        {
                            String cookie = cookieString.Substring(0, cookieString.Length - 1);
                            request.Headers.Add("Cookie", cookie);
                        }

                        response = (HttpWebResponse)request.GetResponse();
                        SR = new StreamReader(response.GetResponseStream());
                        ResponseText = SR.ReadToEnd();

                        Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("ScanStatusResponse response status : [{0}]", response.StatusCode + " - " + response.StatusDescription));
                        Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("ScanStatusResponse response headers : [{0}]", response.Headers.ToString()));
                        Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("ScanStatusResponse response received : [{0}]", ResponseText));
                        /*
                            <?xml version="1.0" encoding="UTF-8"?>
                            <Scan>
                              <Scan-Reference-Id>10</Scan-Reference-Id>
                              <Temp-key-1>tripped</Temp-key-1>
                            </Scan>
                        */
                        /*
                            <?xml version="1.0" encoding="UTF-8"?>
                            <Scan>
                              <Scan-Reference-Id>17</Scan-Reference-Id>
                              <instance-id-1 >Running</instance-id-1 >
                            </Scan>
                        */
                    }
                    catch (Exception ex)
                    {
                        Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("Exception ScanStatusResponse = {0}", ex));
                        //The remote server returned an error: (404) Not Found.

                        Regex objNaturalPattern = new Regex("(404) Not Found");
                        string strTemp = objNaturalPattern.Match(ex.ToString()).ToString();
                        if (strTemp != "")
                        {
                            Utils.Helper_Trace("XORCISM PROVIDER IVIZ", "ERROR 404: Retrying the request...");
                            WaitingScan(cookieString, myscanID);
                            return;
                        }
                    }
                    //XmlDocument doc = new XmlDocument();
                    //doc.LoadXml(ResponseText);
                    SR.Close();

                    Thread.Sleep(60000);
                }
            }


            private void DownloadReport(string cookieString, string myscanID)
            {
                Thread.Sleep(10000);

                string APIURL = "https://sandbox.ivizsecurity.com/download_api";    //Hardcoded
                
                    Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("Downloading..."));
                    string ResponseText = "";
                    StreamReader SR = null;
                    HttpWebResponse response = null;
                    HttpWebRequest request;

                    try
                    {
                        request = (HttpWebRequest)HttpWebRequest.Create(APIURL);

                        //ServicePointManager.CertificatePolicy = new AcceptAllCertificatePolicy();
                        ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate);

                        request.Method = "POST";
                        request.ContentType = "application/xml";
                        /*
                        if (MyCookie.Length > 0)
                        {
                            request.Headers.Add("Cookie", MyCookie);
                        }
                        */
                        if (cookieString.Length > 2)
                        {
                            String cookie = cookieString.Substring(0, cookieString.Length - 1);
                            request.Headers.Add("Cookie", cookie);
                        }

                        //HARDCODED
                        string postData="<?xml version=\"1.0\" encoding=\"UTF-8\"?> <report> <selected_report>"+myscanID+"</selected_report> <report_type>full</report_type> </report>";
                        Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("DownloadReport postData= {0}", postData));
                        byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                        request.ContentLength = byteArray.Length;

                        Stream dataStream = request.GetRequestStream();
                        dataStream.Write(byteArray, 0, byteArray.Length);
                        dataStream.Close();

                        response = (HttpWebResponse)request.GetResponse();
                        SR = new StreamReader(response.GetResponseStream());
                        ResponseText = SR.ReadToEnd();

                        Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("DownloadReport response status : [{0}]", response.StatusCode + " - " + response.StatusDescription));
                        Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("DownloadReport response headers : [{0}]", response.Headers.ToString()));
                        Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("DownloadReport response received : [{0}]", ResponseText));
                        /*
                        <?xml version="1.0" encoding="UTF-8"?> <Response> <url>ftp://Returned_IP/dfsagf_900_full_9_Fri_Jun_13_13_07_39_UTC_201127652-0.pdf</url> </Response>
                        */



                        //Username-- ftp_users Password-- ivizsec2011
                    }
                    catch (Exception ex)
                    {
                        Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("Exception DownloadReport = {0}", ex));
                        //The remote server returned an error: (404) Not Found.

                        Regex objNaturalPattern = new Regex("(404) Not Found"); //Hardcoded
                        string strTemp = objNaturalPattern.Match(ex.ToString()).ToString();
                        if (strTemp != "")
                        {
                            Utils.Helper_Trace("XORCISM PROVIDER IVIZ", "ERROR 404: Retrying the request...");
                            DownloadReport(cookieString, myscanID);
                            return;
                        }
                    }
                    //XmlDocument doc = new XmlDocument();
                    //doc.LoadXml(ResponseText);
                    SR.Close();

                
            }


            private string HelperGetChildInnerText(XmlNode n, string ChildName)
            {
                foreach (XmlNode child in n.ChildNodes)
                {
                    if (child.Name.ToUpper() == ChildName.ToUpper())
                        return child.InnerText;
                }
                return string.Empty;
            }

            private string Helper_ListCVEToString(List<VulnerabilityFound.Item> list)
            {
                string s = "";

                foreach (VulnerabilityFound.Item item in list)
                    s = s + item.ID + ":" + item.Value + " / ";

                return s;
            }

            //private string HelperGetChildInnerText(XmlNode n, string ChildName)
            //{
            //    foreach (XmlNode child in n.ChildNodes)
            //    {
            //        if (child.Name.ToUpper() == ChildName.ToUpper())
            //            return child.InnerText;
            //    }

            //    return string.Empty;
            //}

            private List<VulnerabilityFound.Item> Helper_GetCVE(XmlNode node)
            {
                List<VulnerabilityFound.Item> l;
                l = new List<VulnerabilityFound.Item>();
                try
                {
                    XmlNodeList nodes = node.ChildNodes;
                    foreach (XmlNode n in nodes)
                    {
                        if (n.Attributes["type"] != null)
                        {
                            VulnerabilityFound.Item item = new VulnerabilityFound.Item();
                            item.ID = n.Attributes["type"].InnerText;
                            item.Value = n.InnerText;
                            l.Add(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER IVIZ", string.Format("Error in Helper_GetCVE : Exception = {0}", ex.Message));
                }
                return l;
            }

            public void UpdateJob(int JobId)
            {
                XORCISMEntities model = new XORCISMEntities();
                var Q = from o in model.JOB
                        where o.JobID == JobId
                        select o;
                JOB myJob = Q.FirstOrDefault();
                myJob.Status = XCommon.STATUS.FINISHED.ToString();
                myJob.DateEnd = DateTimeOffset.Now;
                model.SaveChanges();
            }

            public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            {
                //TODO
                //Always accepts !!!
                return true;
            }

            public bool IsValidIP(string addr)
            {
                //create our match pattern
                string pattern = @"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$";  //TODO (+IPv6)
                //create our Regular Expression object
                Regex check = new Regex(pattern);
                //boolean variable to hold the status
                bool valid = false;
                //check to make sure an ip address was provided
                if (addr == "")
                {
                    //no address provided so return false
                    valid = false;
                }
                else
                {
                    //address provided so use the IsMatch Method
                    //of the Regular Expression object
                    valid = check.IsMatch(addr, 0);
                }
                //return the results
                return valid;
            }

        }

        internal class AcceptAllCertificatePolicy : ICertificatePolicy
        {
            public AcceptAllCertificatePolicy()
            {
            }

            public bool CheckValidationResult(ServicePoint sPoint,
               X509Certificate cert, WebRequest wRequest, int certProb)
            {
                //TODO
                // *** Always accepts
                return true;
            }
        }


    }
}
