using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XProviderCommon
{
    [Serializable]
    public class ScannableAddress
    {
        public enum ADDRESSTYPE
        {
            ipv4,
            ipv6,
            mac
        };

        public ADDRESSTYPE AddressType
        {
            get { return m_AddressType; }
            set { m_AddressType = value; }
        }

        public string Address
        {
            get { return m_Address; }
            set { m_Address = value; }
        }

        private ADDRESSTYPE m_AddressType;
        private string      m_Address;
        public ScannableAddress()
        {
            m_AddressType = ADDRESSTYPE.ipv4;
            m_Address = string.Empty;
        }
        public ScannableAddress(ADDRESSTYPE adressType,string adressValue)
        {
            m_AddressType = adressType;
            m_Address = adressValue;
        }
    }
}
