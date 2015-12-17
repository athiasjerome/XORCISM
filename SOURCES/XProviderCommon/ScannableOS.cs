using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XProviderCommon
{
    [Serializable]
    public class ScannableOS
    {
        public string OSName
        {
            get { return m_OSName; }
            set { m_OSName = value; }
        }

        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }
        public int Accuracy
        {
            get { return m_Accuracy; }
            set { m_Accuracy = value; }
        }

        private string m_OSName;
        private string m_Description;
        private int m_Accuracy;
        public ScannableOS()
        {
        }
        public ScannableOS(string oSName,string description,int accruracy)
        {
            m_OSName = oSName;
            m_Description = description;
            m_Accuracy = accruracy;
        }
    }
}
