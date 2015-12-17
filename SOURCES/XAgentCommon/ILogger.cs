using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XAgentCommon
{
    public interface ILogger
    {
        //TODO: INSECURE :)
        void LogMessage(string message);
    }
}
