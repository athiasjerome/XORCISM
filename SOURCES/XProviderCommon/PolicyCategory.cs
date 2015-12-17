using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XProviderCommon
{
    public class PolicyCategory
    {
        public string Title 
        {
            get { return m_Title; }
            set { m_Title = value; }
        }
        public List<PolicyRule> PolicyRules
        {
            get { return m_PolicyRules; }
            set { m_PolicyRules = value; }
        }

        private string m_Title;
        private List<PolicyRule> m_PolicyRules;
        
        public PolicyCategory()
        {
            m_Title = string.Empty;
            m_PolicyRules = new List<PolicyRule>();
        }
    }
}
