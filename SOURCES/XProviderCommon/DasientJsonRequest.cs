using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XProviderCommon
{
    public class DasientJsonRequest
    {
        public string api_version
        {
            get { return m_api_version; }
            set { m_api_version = value; }
        }

        public Request request
        {
            get { return m_request; }
            set { m_request = value; }
        }

        private string  m_api_version;
        private Request m_request;

        public static string ConvertToJSON(DasientJsonRequest request)
        {
           string result;
           result = Newtonsoft.Json.JsonConvert.SerializeObject(request);
           result = result.Replace("_", "-");

           return result;
        }
    }

    public class Request
    {
        public string hostname
        {
            get { return m_hostname; }
            set { m_hostname = value; }
        }

        public string request_type
        {
            get { return m_request_type; }
            set { m_request_type = value; }
        }

        public string max_pages
        {
            get { return m_max_pages; }
            set { m_max_pages = value; }
        }
        
        public string response_url
        {
            get { return m_response_url; }
            set { m_response_url = value; }
        }

        private string  m_hostname;
        private string  m_request_type;
        private string  m_response_url;
        private string  m_max_pages;
    }
}
