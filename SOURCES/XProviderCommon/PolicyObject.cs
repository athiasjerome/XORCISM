using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XProviderCommon
{
    public class PolicyObject
    {
        private string m_name;
        private string m_target;

        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        public string Target
        {
            get { return m_target; }
            set { m_target = value; }
        }

    }
}
