using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.ServiceModel;
using System.Threading;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using XORCISMModel;
using XCommon;

using NCrontab;

namespace XManagerHost

{
    /// <summary>
    /// Copyright (C) 2012-2015 Jerome Athias
    /// Windows service to manage XORCISM on one Host (but you maybe want a cluster ;-))
    /// This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation; either version 2 of the License, or (at your option) any later version.
    /// 
    /// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
    /// 
    /// You should have received a copy of the GNU General Public License along with this program; if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
    /// </summary>
    public partial class Service1 : ServiceBase
    {
        private ServiceHost               m_ServiceHost;
        private XManagerService.Engine    m_Engine;

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Utils.Helper_Trace("MANAGER HOST", "OnStart");

            // ====================
            // Host the WCF service
            // ====================

            Utils.Helper_Trace("MANAGER HOST", "Hosting the WCF service");

            eventLog1.WriteEntry("Hosting the WCF service", EventLogEntryType.Information);

            try
            {
                m_ServiceHost = new ServiceHost(typeof(XManagerService.Service1));
                m_ServiceHost.Open();
            }
            catch (Exception ex)
            {
                Utils.Helper_Trace("MANAGER HOST", "Error while hosting the WCF service : " + ex.Message);

                eventLog1.WriteEntry(ex.Message, EventLogEntryType.Error);
                return;
            }

            // ==============================
            // Create the main polling thread
            // ==============================
            //Start your engines! :p
            Utils.Helper_Trace("MANAGER HOST", "Launching the engine");

            eventLog1.WriteEntry("Launching the engine", EventLogEntryType.Information);

            m_Engine = new XManagerService.Engine();
            m_Engine.Start();
        }

        protected override void OnStop()
        {
            Utils.Helper_Trace("MANAGER HOST", "OnStart");

            if (m_ServiceHost != null)
            {
                m_ServiceHost.Close();
            }

            m_ServiceHost = null;
        }

        
    }
}
