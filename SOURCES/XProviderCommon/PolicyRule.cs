using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XProviderCommon
{
    public class PolicyRule
    {
        public string Title
        {
            get { return m_Title; }
            set{m_Title= value;}
        }
        public object Value
        {
            get { return m_Value; }
            set { m_Value = value; }
        }

        private string m_Title;
        private object m_Value;
        public PolicyRule()
        {
            m_Title = string.Empty;
            m_Value = string.Empty;
           
        }
    }
}
