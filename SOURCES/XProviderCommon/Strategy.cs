using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XProviderCommon
{
    public class Strategy
    {
        private string m_name;
        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }
        public List<PolicyObject> Policies
        {
            get { return m_Policies; }
            set { m_Policies = value; }
        }

        private List<PolicyObject> m_Policies;
        public Strategy()
        {
            m_Policies = new List<PolicyObject>();
        }
    }
}
