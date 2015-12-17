using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace XProviderCommon
{
    public class InfosPolicy
    {
        // ### Network Capabilities ###
        private string m_RangePortTCP;
        public string RangePortTCP
        {
            get { return m_RangePortTCP; }
            set { m_RangePortTCP = value; }
        }

        private string m_RangePortUDP;
        public string RangePortUDP
        {
            get { return m_RangePortUDP; }
            set { m_RangePortUDP = value; }
        }

        private string m_HostPing;
        public string HostPing
        {
            get { return m_HostPing; }
            set { m_HostPing = value; }
        }

        private string m_TCPScan;
        public string TCPScan
        {
            get { return m_TCPScan; }
            set { m_TCPScan = value; }
        }

        private string m_SynScan;
        public string SynScan
        {
            get { return m_SynScan; }
            set { m_SynScan = value; }
        }

        private string m_SNMPScan;
        public string SNMPScan
        {
            get { return m_SNMPScan; }
            set { m_SNMPScan = value; }
        }

        // ### Credentials ###
        private string m_Auth_HTTP;
        public string Auth_HTTP
        {
            get { return m_Auth_HTTP; }
            set { m_Auth_HTTP = value; }
        }

        private string m_Auth_SMB;
        public string Auth_SMB
        {
            get { return m_Auth_SMB; }
            set { m_Auth_SMB = value; }
        }

        private string m_Auth_SSH;
        public string Auth_SSH
        {
            get { return m_Auth_SSH; }
            set { m_Auth_SSH = value; }
        }

        // ### Vulnerability DB ###
        private string m_Intrusive_Checks;
        public string Intrusive_Checks
        {
            get { return m_Intrusive_Checks; }
            set { m_Intrusive_Checks = value; }
        }

        private string m_Brute_Force_Checks;
        public string Brute_Force_Checks
        {
            get { return m_Brute_Force_Checks; }
            set { m_Brute_Force_Checks = value; }
        }

        // ### WAS Capabilities ###
        private string m_HTTPS_mode;
        public string HTTPS_mode
        {
            get { return m_HTTPS_mode; }
            set { m_HTTPS_mode = value; }
        }

        private string m_URI;
        public string URI
        {
            get { return m_URI; }
            set { m_URI = value; }
        }

        private string m_Port;
        public string Port
        {
            get { return m_Port; }
            set { m_Port = value; }
        }

        private string m_Crawling_technique;
        public string Crawling_technique
        {
            get { return m_Crawling_technique; }
            set { m_Crawling_technique = value; }
        }

        private string m_Pages_Depth;
        public string Pages_Depth
        {
            get { return m_Pages_Depth; }
            set { m_Pages_Depth = value; }
        }

        // ### Monitoring ###
        private string m_Frequency;
        public string Frequency
        {
            get { return m_Frequency; }
            set { m_Frequency = value; }
        }

        private string m_Protocol_scan;
        public string Protocol_scan
        {
            get { return m_Protocol_scan; }
            set { m_Protocol_scan = value; }
        }

        private string m_Alert_Email;
        public string Alert_Email
        {
            get { return m_Alert_Email; }
            set { m_Alert_Email = value; }
        }

        // ### VoIPAuditing DB ###
        // User entry

        // ### MalwareScan ###
        // User entry
        private string m_Policy;

        public InfosPolicy(string Policy)
        {
            m_Policy = Policy;
        }

        public void Parse_NetworkCapabilities()
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(m_Policy);
            XmlNodeList nlist = doc.SelectNodes("/Policy/PolicyProfiles/PolicyCategory[Title=\"NetworkCapbilities\"]/PolicyRules/PolicyRule");  //Hardcoded
            foreach (XmlNode n in nlist)
            {
                switch (n.ChildNodes[0].InnerText)
                {
                    case "Host Ping (ICMP Test)": m_HostPing = n.ChildNodes[1].InnerText; break;
                    case "Port Range TCP": m_RangePortTCP = n.ChildNodes[1].InnerText; break;
                    case "Port Range UDP": m_RangePortUDP = n.ChildNodes[1].InnerText; break;
                    case "TCP Scan": m_TCPScan = n.ChildNodes[1].InnerText; break;
                    case "Syn Scan": m_SynScan = n.ChildNodes[1].InnerText; break;
                    case "SNMP Scan": m_SNMPScan = n.ChildNodes[1].InnerText; break;
                }
            }
        }

        public void Parse_Credentials()
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(m_Policy);
            XmlNodeList nlist = doc.SelectNodes("/Policy/PolicyProfiles/PolicyCategory[Title=\"Credentials\"]/PolicyRules/PolicyRule"); //Hardcoded
            foreach (XmlNode n in nlist)
            {
                switch (n.ChildNodes[0].InnerText)
                {
                    case "Authentication HTTP": m_Auth_HTTP = n.ChildNodes[1].InnerText; break;
                    case "Authentication SMB": m_Auth_SMB = n.ChildNodes[1].InnerText; break;
                    case "Authentication SSH": m_Auth_SSH = n.ChildNodes[1].InnerText; break;
                }
            }
        }

        public void Parse_VulnerabilityDB()
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(m_Policy);
            XmlNodeList nlist = doc.SelectNodes("/Policy/PolicyProfiles/PolicyCategory[Title=\"VulnerabilityDB\"]/PolicyRules/PolicyRule"); //Hardcoded
            foreach (XmlNode n in nlist)
            {
                switch (n.ChildNodes[1].InnerText)
                {
                    case "Intrusive Checks disabled": m_Intrusive_Checks = (n.ChildNodes[1].InnerText.Split(new char[] { ' ' }))[2]; break;
                    case "Brute Force Checks disabled": m_Brute_Force_Checks = (n.ChildNodes[1].InnerText.Split(new char[] { ' ' }))[3]; break;
                }
            }
        }

        public void Parse_WAS_Capabilities()
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(m_Policy);
            XmlNodeList nlist = doc.SelectNodes("/Policy/PolicyProfiles/PolicyCategory[Title=\"WAS Capabilities\"]/PolicyRules/PolicyRule");    //Hardcoded
            foreach (XmlNode n in nlist)
            {
                switch (n.ChildNodes[0].InnerText)
                {
                    case "HTTPS mode": m_HTTPS_mode = n.ChildNodes[1].InnerText; break;
                    case "URI": m_URI = n.ChildNodes[1].InnerText; break;
                    case "Port": m_Port = n.ChildNodes[1].InnerText; break;
                    case "Crawling technique": m_Crawling_technique = n.ChildNodes[1].InnerText; break;
                    case "Pages Depth": m_Pages_Depth = n.ChildNodes[1].InnerText; break;
                }
            }
        }

        // TODO : User Entry
        public void Parse_VoIPAuditing()
        {
        }

        // TODO : User Entry
        public void Parse_MalwareScan()
        {
        }

        public void Parse_Monitoring()
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(m_Policy);
            XmlNodeList nlist = doc.SelectNodes("/Policy/PolicyProfiles/PolicyCategory[Title=\"Monitoring\"]/PolicyRules/PolicyRule");  //Hardcoded
            foreach (XmlNode n in nlist)
            {
                switch (n.ChildNodes[0].InnerText)
                {
                    case "Frequency": m_Frequency = n.ChildNodes[1].InnerText; break;
                    case "Protocol scan": m_Protocol_scan = n.ChildNodes[1].InnerText; break;
                    case "Alert Email": m_Alert_Email = n.ChildNodes[1].InnerText; break;
                }
            }
        }

    }
}
