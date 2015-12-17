using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;

using XCommon;

using Newtonsoft.Json;

namespace XProviderCommon
{
    public abstract class XJsonParser
    {
        protected string m_url;

        public class ActionParameter
        {
            public string Value
            {
                get { return m_parameterValue; }
                set { m_parameterValue = value; }
            }
            public string Name
            {
                get { return m_parameterName; }
                set { m_parameterName = value; }
            }
            private string m_parameterValue;
            private string m_parameterName;
            public ActionParameter(string parameterName, string parameterValue)
            {
                m_parameterName = parameterName;
                m_parameterValue = parameterValue;
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
                // *** Always accept!
                return true;
            }
        }

        public XJsonParser(string url)
        {
            m_url = url;
            //example  "api_id=user@xorcism.org&secret_key=6624d1bfde0cee860f1882aeb6c4dbdb&user_id=812691&partner_id=1070733&action="
        }

        public XmlDocument ExecuteAction_OLD(string actionName, List<ActionParameter> parameters,string JsonRootNode)
        {
            MemoryStream ms = new MemoryStream();
            StreamWriter sw = new StreamWriter(ms);
            string query = m_url+ actionName;
           if(parameters!=null)
                foreach (ActionParameter p in parameters)
                    query += "&" + p.Name + "=" + p.Value;

           //"?api_id=user@xorcism.org&secret_key=a624d1bfde0cuy860f4882aeb6c4dbdb&user_id=820991&partner_id=107783&action=get_domains"
            sw.Write(query);
            sw.Flush();

            byte[] buffer;
            buffer = ms.GetBuffer();

            HttpWebRequest request;
            request = (HttpWebRequest)HttpWebRequest.Create(query);
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";
            request.ContentLength = ms.Length;

            Stream s = request.GetRequestStream();
            s.Write(buffer, 0, (int)ms.Length);
            s.Close();

            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            string json = "";
            using (StreamReader reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    json += reader.ReadLine();
                }
            }
            Utils.Helper_Trace("XORCISM PROVIDER COMMON", string.Format("The current Json result ={0}", json));

         if (json.Contains(")"))
            {
                json = json.Substring(json.IndexOf('{'), json.Length - json.IndexOf('{'));
            }
            XmlDocument rawXml = new XmlDocument();
            rawXml = JsonConvert.DeserializeXmlNode(json,JsonRootNode);//JsonRootNode="results" by example
            Utils.Helper_Trace("XORCISM PROVIDER COMMON", string.Format("The current rawXml result ={0}", rawXml.InnerXml));
            return rawXml;
        }

        public XmlDocument ExecuteAction_OLD(string actionName, List<ActionParameter> parameters)
        {
            MemoryStream ms = new MemoryStream();
            StreamWriter sw = new StreamWriter(ms);
            string query = m_url + actionName;
            if (parameters != null)
                foreach (ActionParameter p in parameters)
                    query += "&" + p.Name + "=" + p.Value;

            //"?api_id=user@xorcism.org&secret_key=a624d1bfde0xze860f4882aeb6c4dbdb&user_id=0020391&partner_id=1034593&action=get_domains"
            sw.Write(query);
            sw.Flush();

            byte[] buffer;
            buffer = ms.GetBuffer();

            HttpWebRequest request;
            request = (HttpWebRequest)HttpWebRequest.Create(query);
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";
            request.ContentLength = ms.Length;

            Stream s = request.GetRequestStream();
            s.Write(buffer, 0, (int)ms.Length);
            s.Close();

            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            string json = "";
            using (StreamReader reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    json += reader.ReadLine();
                }
            }
            Utils.Helper_Trace("XORCISM PROVIDER COMMON", string.Format("The current Json result ={0}", json));

            


            if (json.Contains(")"))
            {
                json = json.Substring(json.IndexOf('{'), json.Length - json.IndexOf('{'));
            }
            XmlDocument rawXml = new XmlDocument();
            rawXml = JsonConvert.DeserializeXmlNode(json, "results");//JsonRootNode="results" by example
            Utils.Helper_Trace("XORCISM PROVIDER COMMON", string.Format("The current rawXml result ={0}", rawXml.InnerXml));
            return rawXml;
        }

        public XmlDocument ExecuteActionWithUrl(string url,string actionName, List<ActionParameter> parameters)
        {
            MemoryStream ms = new MemoryStream();
            StreamWriter sw = new StreamWriter(ms);
            string query = url + actionName;
            if (parameters != null)
            {
                int i = 0;
                foreach (ActionParameter p in parameters)
                {  
                    if(i==0)
                        query += p.Name + "=" + p.Value;
                    else
                        query += "&" + p.Name + "=" + p.Value;
                    i++;
                }
            }
            //"?api_id=user@xorcism.org&secret_key=a624d1bfdklcee860f4882aeb6c4dbdb&user_id=8174391&partner_id=1079093&action=get_domains"
            Utils.Helper_Trace("XORCISM PROVIDER COMMON", string.Format("ExecuteActionWithUrl : Query = [{0}]", query));
            sw.Write(query);
            sw.Flush();

            byte[] buffer;
            buffer = ms.GetBuffer();

            HttpWebRequest request;
            request = (HttpWebRequest)HttpWebRequest.Create(query);
            request.ContentType     = "application/x-www-form-urlencoded";
            request.Method          = "POST";
            request.ContentLength   = ms.Length;
            request.Timeout         = 180000;   // 180 seconds = 3 minutes  //HARDCODED

            Stream s = request.GetRequestStream();
            s.Write(buffer, 0, (int)ms.Length);
            s.Close();

            WebResponse response = request.GetResponse();
            

            Stream stream = response.GetResponseStream();
            string json = "";
            using (StreamReader reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    json += reader.ReadLine();
                }
            }
            Utils.Helper_Trace("XORCISM PROVIDER COMMON", string.Format("The current Json result ={0}", json));
            if (json.Contains(")"))
            {
                json = json.Substring(json.IndexOf('{'), json.Length - json.IndexOf('{'));
            }
            XmlDocument rawXml = new XmlDocument();
            rawXml = JsonConvert.DeserializeXmlNode(json, "results");//JsonRootNode="results" by example
            if (rawXml == null)
                return null;
            Utils.Helper_Trace("XORCISM PROVIDER COMMON", string.Format("The current rawXml result ={0}", rawXml.InnerXml));
            //<status>FAILED</status><failure><code>100</code><reason>Domain Not Found</reason></failure></status>
            return rawXml;
        }

        public XmlDocument ExecuteAction_OLD(string actionName,List<ActionParameter> parameters,XmlDocument JsonContent)
        {
            MemoryStream ms = new MemoryStream();
            StreamWriter sw = new StreamWriter(ms);
            string query = m_url + actionName;
            if (parameters != null)
                foreach (ActionParameter p in parameters)
                    query += "&" + p.Name + "=" + p.Value;

            //"?api_id=user@xorcism.org&secret_key=a624d1bfde8uee860f4882aeb6c4dbdb&user_id=8190341&partner_id=1075502&action=get_domains"
            string content;
            content = JsonConvert.SerializeXmlNode(JsonContent, Newtonsoft.Json.Formatting.None);
            query += content;
            sw.Write(query);
            sw.Flush();

            byte[] buffer;
            buffer = ms.GetBuffer();

            HttpWebRequest request;
            request = (HttpWebRequest)HttpWebRequest.Create(query);
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";
            request.ContentLength = ms.Length;
            
            Stream s = request.GetRequestStream();
            s.Write(buffer, 0, (int)ms.Length);
            s.Close();

            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            string json = "";
            using (StreamReader reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    json += reader.ReadLine();
                }
            }
            Utils.Helper_Trace("XORCISM PROVIDER COMMON", string.Format("The current Json result ={0}", json));




            if (json.Contains(")"))
            {
                json = json.Substring(json.IndexOf('{'), json.Length - json.IndexOf('{'));
            }
            XmlDocument rawXml = new XmlDocument();
            rawXml = JsonConvert.DeserializeXmlNode(json, "results");//JsonRootNode="results" by example
            
            Utils.Helper_Trace("XORCISM PROVIDER COMMON", string.Format("The current rawXml result ={0}", rawXml.InnerXml));
            return rawXml;
        }

        public XmlDocument ExecuteAction(string actionName, string json, List<ActionParameter> parameters)
        {
            string query = m_url + actionName;
            if (parameters != null)
            {
                foreach (ActionParameter p in parameters)
                    query += "&" + p.Name + "=" + p.Value;
            }

            Utils.Helper_Trace("XORCISM PROVIDER COMMON", string.Format("ExecuteAction : Query = [{0}]", query));

            MemoryStream    ms = null;
            byte[]          buffer = null;
            if (json != null)
            {
                Utils.Helper_Trace("XORCISM PROVIDER COMMON", string.Format("ExecuteAction : JSON to post = [{0}]", json));

                ms = new MemoryStream();

                StreamWriter sw;
                sw = new StreamWriter(ms);

                sw.Write(json);
                sw.Flush();

                buffer = ms.GetBuffer();
            }

            ServicePointManager.CertificatePolicy = new AcceptAllCertificatePolicy();
//            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate);

            HttpWebRequest request;
            request = (HttpWebRequest)HttpWebRequest.Create(query);
            request.ContentType     = "application/json";
            request.Method          = "POST";
            request.Timeout         = 180000;   // 180 seconds = 3 minutes  //HARDCODED

            if (json != null)
                request.ContentLength   = ms.Length;

            if (json != null)
            {
                Stream s = request.GetRequestStream();
                s.Write(buffer, 0, (int)ms.Length);
                s.Close();
            }

            WebResponse response;
            response = request.GetResponse();

            Stream stream;
            stream = response.GetResponseStream();

            string jsonResponse = "";
            using (StreamReader reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    jsonResponse += reader.ReadLine();
                }
            }

            Utils.Helper_Trace("XORCISM PROVIDER COMMON", string.Format("ExecuteAction : Response as JSON = {0}", jsonResponse));   //ERROR: 54633

            if (jsonResponse.Contains(")"))
            {
                jsonResponse = jsonResponse.Substring(jsonResponse.IndexOf('{'), jsonResponse.Length - jsonResponse.IndexOf('{'));
            }

            XmlDocument rawXml = new XmlDocument();
            rawXml = JsonConvert.DeserializeXmlNode(jsonResponse, "results");   //JsonRootNode="results" by example
            if (rawXml == null)
                return null;

            Utils.Helper_Trace("XORCISM PROVIDER COMMON", string.Format("ExecuteAction : Response as XML = {0}", rawXml.InnerXml));

            return rawXml;
        }
    }
   
}
