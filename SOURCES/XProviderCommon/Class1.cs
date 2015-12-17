using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XORCISMModel;

namespace XProviderCommon
{
    public interface IScannable
    {
        List<ScannableAsset> DiscoverHost(string target, HostStatus status, int jobid);
    }
    
    public interface IVulnerabilityDetector
    {
        void Run(string target, int jobID, string policy, string strategy);
    }

    public interface IVulnerabilityImporter
    {
        void Run(string data, int jobID, int AccountID);
    }

    public interface IWebSiteMonitor
    {
        string Run(string asset, int jobID, DateTime DD, DateTime DF, Guid userID);
    }

    public interface IBlacklistedDetector
    {
        void Run(string target, int jobID);
    }
    public interface IMalwareDetector
    {
        void Run(string target, int jobID,int MaxPages);
    }

}
