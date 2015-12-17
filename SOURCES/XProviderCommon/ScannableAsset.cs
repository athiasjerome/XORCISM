using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XProviderCommon;

namespace XProviderCommon
{
    [Serializable]
    public class ScannableAsset
    {
        public List<ScannableAddress> Addresses
        {
            get { return m_NmapAddresses; }
            set { m_NmapAddresses = value; }
        }

        public List<ScannablePort> Ports
        {
            get { return m_NmapPorts; }
            set { m_NmapPorts = value; }
        }

        public ScannableOS OS
        {
            get { return m_NmapAssestOS; }
            set { m_NmapAssestOS = value; }
        }
           
        private List<ScannableAddress>  m_NmapAddresses;
        private List<ScannablePort>     m_NmapPorts;
        private ScannableOS             m_NmapAssestOS;

        public ScannableAsset()
        {
            m_NmapAddresses = new List<ScannableAddress>();
            m_NmapPorts     = new List<ScannablePort>();
            m_NmapAssestOS = new ScannableOS();
        }

        public ScannableAddress LookupAddress(string address)
        {
            foreach (ScannableAddress nmapAddress in m_NmapAddresses)
            {
                if (nmapAddress.Address == address)
                    return nmapAddress;
            }

            return null;
        }
    }
}
