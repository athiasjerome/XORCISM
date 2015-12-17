using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XProviderCommon
{
    public class Policy
    {
        public List<PolicyCategory> PolicyProfiles
        {
            get { return m_PolicyProfiles; }
            
            set {
                    m_PolicyProfiles = value;
                    if (m_PolicyProfiles.Count > 0)
                        m_Policystate = PolicyState.Enabled;
                }
        }
        public string Title
        {
            get { return m_Title; }
            set { m_Title = value; }
        }
        public PolicyState Status
        {
            get { return m_Policystate; }
            set { m_Policystate = value; }
        }

        private List<PolicyCategory> m_PolicyProfiles;
        private string m_Title;
        private PolicyState m_Policystate;
        public Policy()
        {
            m_PolicyProfiles = new List<PolicyCategory>();
            m_Title = string.Empty;
            m_Policystate = PolicyState.Disabled;
        }
        public Policy(List<PolicyCategory> policyProfiles,string title)
        {
            m_PolicyProfiles = policyProfiles;
            m_Title = title;
            m_Policystate = PolicyState.Enabled;
        }
    }
    public enum PolicyState
    {
        Enabled,
        Disabled
    }
}
