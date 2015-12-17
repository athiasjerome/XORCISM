using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;


namespace XManagerHost
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();

            if (!System.Diagnostics.EventLog.SourceExists("XORCISM COREvidence Manager"))
            {
                System.Diagnostics.EventLog.CreateEventSource("XORCISM COREvidence Manager", "Application");
            }
        }
    }
}
