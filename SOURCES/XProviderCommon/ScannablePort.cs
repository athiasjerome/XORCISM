using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XProviderCommon
{
    public enum PROTOCOL
    {
        tcp,
        udp
    }

    [Serializable]
    public class ScannablePort
    {
        public PROTOCOL Protocol
        {
            get { return m_Protocol; }
            set { m_Protocol = value; }
        }

        public int Port
        {
            get { return m_Port; }
            set { m_Port = value; }
        }

        public string Service
        {
            get { return m_Service; }
            set { m_Service = value; }
        }

        public string Version
        {
            get { return m_Version; }
            set { m_Version = value; }
        }

        private PROTOCOL m_Protocol;
        private int         m_Port;
        private string      m_Service;
        private string m_Version;
        public ScannablePort()
        {
            m_Protocol = PROTOCOL.tcp;
        }
        public ScannablePort(PROTOCOL protocol, int port, string service, string version)
        {
            m_Protocol = protocol;
            m_Port = port;
            m_Service = service;
            m_Version = version;
        }
    }
}
