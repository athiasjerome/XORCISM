using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Runtime;
using System.Diagnostics;
using System.ComponentModel;
using System.Reflection;
using System.IO;
using System.Xml;

using XORCISMModel;
using XProviderCommon;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using XCommon;

namespace XProviderNmap
{
    /// <summary>
    /// Copyright (C) 2011-2015 Jerome Athias
    /// nmap plugin for XORCISM
    /// All trademarks and registered trademarks are the property of their respective owners.
    /// This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation; either version 2 of the License, or (at your option) any later version.
    /// 
    /// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
    /// 
    /// You should have received a copy of the GNU General Public License along with this program; if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
    /// </summary>
    public class Engine : MarshalByRefObject, XProviderCommon.IScannable
    {
        private const int ERROR_FILE_NOT_FOUND = 2;
        private const int ERROR_ACCESS_DENIED = 5;

        public List<ScannableAsset> Run(string target, string policy)
        {
           List<ScannableAsset> MyResult = new List<ScannableAsset>();
           return MyResult;
        }
        //public void Run(string target, string policy, out List<ScannableAsset> MyResult)
       //{

       //    XmlScannableOutPut = string.Empty;

       //    // ==============
       //    // Launches NMAP
       //    // ==============

       //    Assembly a;
       //    a = Assembly.GetExecutingAssembly();

       //    string program;
       //    program = Path.GetDirectoryName(a.Location) + "\\Nmap\\nmap.exe";  //Hardcoded


       //    Process process;
       //    process = new Process();
       //    process.StartInfo.UseShellExecute = true;

       //    try
       //    {
       //        process.StartInfo.FileName = program;
       //        process.StartInfo.Arguments = "--no-stylesheet -oX - " + target;   //Hardcoded
       //        process.StartInfo.UseShellExecute = false;
       //        process.StartInfo.RedirectStandardOutput = true;
       //        process.StartInfo.RedirectStandardError = false;
       //        process.StartInfo.CreateNoWindow = true;
       //        // process.EnableRaisingEvents = true;
       //        // process.Exited += new EventHandler(Process_Exited);
       //        process.Start();
       //        // Process.Start(vProgram,vIAnnotationLocal.Folder + vIAnnotationLocal.EntryPoint);


       //    }
       //    catch (Win32Exception vException)
       //    {
       //        if (vException.NativeErrorCode == ERROR_FILE_NOT_FOUND)
       //        {
       //            Trace.WriteLine(vException.Message + ". Check the path.");
       //        }
       //        else if (vException.NativeErrorCode == ERROR_ACCESS_DENIED)
       //        {
       //            // Note that if your word processor might generate exceptions such as this, which are handled first.
       //            Trace.WriteLine(vException.Message + ". You do not have permission to access this file.");
       //        }
       //    }

       //    // =====================
       //    // Get and parse results
       //    // =====================

       //    XmlDocument doc;
       //    doc = new XmlDocument();
        //  TODO: Input Validation
       //    doc.Load(process.StandardOutput);
       //    process.WaitForExit();
       //    string xpath;
       //    xpath = string.Format("/nmaprun/host/status[@state='up']");    //Hardcoded
       //    // xpath = string.Format("/nmaprun/host/status");

       //    XmlNodeList nodes;
       //    nodes = doc.SelectNodes(xpath);

       //    List<ScannableAsset> tabNmapAsset;
       //    tabNmapAsset = new List<ScannableAsset>();

       //    foreach (XmlNode node in nodes)
       //    {
       //        ScannableAsset nmapAsset;
       //        nmapAsset = new ScannableAsset();

       //        XmlNode nodeHost;
       //        nodeHost = node.ParentNode;

       //        // ====================
       //        // Handle the addresses
       //        // ====================

       //        XmlNodeList addressNodes;
       //        addressNodes = nodeHost.SelectNodes("address");    //Hardcoded
       //        foreach (XmlNode addressNode in addressNodes)
       //        {
       //            string addr = addressNode.Attributes["addr"].InnerText;
       //            ScannableAddress.ADDRESSTYPE addrtype = (ScannableAddress.ADDRESSTYPE)Enum.Parse(typeof(ScannableAddress.ADDRESSTYPE), addressNode.Attributes["addrtype"].InnerText);

       //            nmapAsset.Addresses.Add(new ScannableAddress() { Address = addr, AddressType = addrtype });
       //        }

       //        // ================
       //        // Handle the ports
       //        // ================

       //        XmlNodeList portNodes;
       //        portNodes = nodeHost.SelectNodes("ports/port");    //Hardcoded
       //        foreach (XmlNode portNode in portNodes)
       //        {
       //            ScannablePort.PROTOCOL protocol = (ScannablePort.PROTOCOL)Enum.Parse(typeof(ScannablePort.PROTOCOL), portNode.Attributes["protocol"].InnerText);
       //            int portid = Convert.ToInt32(portNode.Attributes["portid"].InnerText);
       //            string service = portNode.SelectSingleNode("service/@name").InnerText;

       //            ScannablePort nmapPort;
       //            nmapPort = new ScannablePort() { Protocol = protocol, Port = portid, Service = service };

       //            nmapAsset.Ports.Add(nmapPort);
       //        }

       //        tabNmapAsset.Add(nmapAsset);
       //    }

       //    // Parse the List of ScallableAsset into an Standart XmlDocument 
       //    XmlScannableOutPut = ProcessIntoXmlDocument(tabNmapAsset);
       //    return null;
       //}

        private string ProcessIntoXmlDocument(List<ScannableAsset> scannableAssets)
        {
            string outPutXml=string.Empty;
            //Generate and Convert an XmlDocument into ScannableAssets Standart Xml .
            StringWriter scannableAssetWriter;
            scannableAssetWriter = new StringWriter();
            XmlSerializer ser;
            ser = new XmlSerializer(scannableAssets.GetType());

            ser.Serialize(scannableAssetWriter, scannableAssets);

            outPutXml = scannableAssetWriter.ToString();

            return outPutXml;
        }


        public void GetServicesVersionForSession(string target, int sessionid)
        {
            Utils.Helper_Trace("XORCISM PROVIDER NMAP", "Entering GetServicesVersionForSession");

            Utils.Helper_Trace("XORCISM PROVIDER NMAP", string.Format("Target is '{0}'. Session is {1}", target, sessionid));

            string outputXml = string.Empty;
            Assembly a;
            a = Assembly.GetCallingAssembly();

            string program;
            program = Path.GetDirectoryName(a.Location) + @"\Nmap\nmap.exe";    //Hardcoded

            Utils.Helper_Trace("XORCISM PROVIDER NMAP", string.Format("Using nmap at '{0}'", program));

            string nmapfile;
            nmapfile = string.Format("nmap{0}_{1}", DateTime.Now.Ticks, this.GetHashCode());

            //Some filters
            target = target.Replace("&", "");
            target = target.Replace("|", "");
            Regex check = new Regex("^[a-zA-Z0-9.-/]+$");
            if (!check.IsMatch(target))
            {
                Utils.Helper_Trace("XORCISM PROVIDER NMAP", "ERROR BAD CHARACTERS/FORMAT for target");
                return;
            }
            Utils.Helper_Trace("XORCISM PROVIDER NMAP", string.Format("Command line is [nmap {0}]", "--no-stylesheet -O -A -sC -P0 -oX " + nmapfile + ".xml " + target));   //Hardcoded

            Process process;
            process = new Process();

            process.StartInfo.UseShellExecute = true;

            try
            {
                process.StartInfo.FileName = program;
                process.StartInfo.Arguments = " --no-stylesheet -O -A -sC -P0 -oX " + nmapfile + ".xml " + target;  //Hardcoded
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = false;
                process.StartInfo.CreateNoWindow = true;
                // process.EnableRaisingEvents = true;
                // process.Exited += new EventHandler(Process_Exited);
                process.Start();
                // Process.Start(vProgram,vIAnnotationLocal.Folder + vIAnnotationLocal.EntryPoint);
            }
            catch (Win32Exception vException)
            {
                if (vException.NativeErrorCode == ERROR_FILE_NOT_FOUND)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER NMAP", string.Format("ERROR_FILE_NOT_FOUND : Exception = {0}", vException.Message));
                    //return null;
                }
                else if (vException.NativeErrorCode == ERROR_ACCESS_DENIED)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER NMAP", string.Format("ERROR_ACCESS_DENIED : Exception = {0}", vException.Message));
                    //return null;
                }
            }

            Utils.Helper_Trace("XORCISM PROVIDER NMAP", string.Format("Nmap is running"));

            XmlDocument doc;

            try
            {
                Utils.Helper_Trace("XORCISM PROVIDER NMAP", string.Format("Waiting for Nmap to finish"));

                process.WaitForExit(600000);    //1 hour
            }
            catch (Exception vException)
            {
                Utils.Helper_Trace("XORCISM PROVIDER NMAP", string.Format("Exception = {0}", vException.Message));
                //return null;
            }

            Utils.Helper_Trace("XORCISM PROVIDER NMAP", "Nmap has finished");
            StreamReader SR = process.StandardOutput;
            string strOutput = SR.ReadToEnd();
            Utils.Helper_Trace("XORCISM PROVIDER NMAP", string.Format("Output: " + strOutput));

            if (strOutput.Contains("0 hosts up"))
            {
                Utils.Helper_Trace("XORCISM PROVIDER NMAP", "0 hosts up");
                //return null;
            }
            if (strOutput.Contains("Windows does not support scanning your own machine"))
            {
                Utils.Helper_Trace("XORCISM PROVIDER NMAP", "Windows does not support scanning your own machine");
                //return null;
            }

            doc = new XmlDocument();
            string m_data = string.Empty;
            try
            {
                //TODO: Input (XML) Validation
                //doc.Load(process.StandardOutput);
                doc.Load(nmapfile + ".xml");
                m_data = doc.InnerXml;
            }
            catch (Exception vException)
            {
                Utils.Helper_Trace("XORCISM PROVIDER NMAP", string.Format("Exception Load = {0}", vException.Message));
                process.Close();
                //return null;
            }
            process.Close();

            try
            {
                string xpath = string.Empty;

                //if (hostStatus == HostStatus.Both)
                //    //xpath = string.Format("/nmaprun/host/status[@state='up']");
                    xpath = "/nmaprun/host/status"; //Hardcoded
                //else if (hostStatus == HostStatus.Alive)
                //    xpath = "/nmaprun/host/status[@state='up']";
                //else
                //    xpath = "/nmaprun/host/status[@state='down']";

                XmlNodeList nodes;
                nodes = doc.SelectNodes(xpath);

                foreach (XmlNode node in nodes)
                {
                //    ScannableAsset nmapAsset;
                //    nmapAsset = new ScannableAsset();

                    XmlNode nodeHost;
                    nodeHost = node.ParentNode;

                    // ====================
                    // Handle the addresses
                    // ====================

                    XmlNodeList addressNodes;
                    addressNodes = nodeHost.SelectNodes("address"); //Hardcoded
                    foreach (XmlNode addressNode in addressNodes)
                    {
                        string addr = addressNode.Attributes["addr"].InnerText; //Hardcoded
                    //    ScannableAddress.ADDRESSTYPE addrtype = (ScannableAddress.ADDRESSTYPE)Enum.Parse(typeof(ScannableAddress.ADDRESSTYPE), addressNode.Attributes["addrtype"].InnerText);

                    //    nmapAsset.Addresses.Add(new ScannableAddress() { Address = addr, AddressType = addrtype });
                    }


                    //===
                    // Scripts
                    //===
                    XmlNodeList ScriptsNodes;
                    ScriptsNodes = nodeHost.SelectNodes("hostscript/script");   //Hardcoded
                    string strTemp = string.Empty;
                    string sambaversion = string.Empty;
                    string sambaosversion = string.Empty;
                    string macaddress = string.Empty;
                    Regex myRegex = new Regex("");
                    foreach (XmlNode scriptNode in ScriptsNodes)
                    {
                        switch (scriptNode.Attributes["id"].InnerText)  //Hardcoded
                        {
                            case "nbstat":
                                //<script id="nbstat" output="NetBIOS name: xmachine, NetBIOS user: &lt;unknown&gt;, NetBIOS MAC: &lt;unknown&gt;"/>
                                //<script id="nbstat" output="NetBIOS name: XORCISM-LXTE4KS, NetBIOS user: &lt;unknown&gt;, NetBIOS MAC: 00:0c:29:23:14:b9 (VMware)&#xa;"/>
                                myRegex = new Regex("NetBIOS MAC: [^<>]*"); //Hardcoded
                                strTemp = myRegex.Match(scriptNode.Attributes["output"].InnerText).ToString();
                                if (strTemp != "")
                                {
                                    if (strTemp.Contains("unknown"))
                                    {

                                    }
                                    else
                                    {
                                        strTemp = strTemp.Replace("NetBIOS MAC: ", "");
                                        strTemp = strTemp.Replace("&#xa;", "");
                                        //Console.WriteLine("strTemp=" + strTemp);
                                        macaddress = strTemp;
                                        Utils.Helper_Trace("XORCISM PROVIDER NMAP", string.Format("macaddress = {0}", macaddress));
                                    }
                                }
                                break;
                            case "smbv2-enabled":
                                //<script id="smbv2-enabled" output="Server doesn&apos;t support SMBv2 protocol"/>
                                //<script id="smbv2-enabled" output="Server doesn&apos;t support SMBv2 protocol"/>

                                break;
                            case "smb-os-discovery":
                                //<script id="smb-os-discovery" output="&#xa;  OS: Unix (Samba 3.4.7)&#xa;  Name: Unknown\Unknown&#xa;  System time: 2011-04-14 15:14:49 UTC+2&#xa;"/>
                                //<script id="smb-os-discovery" output=" &#xa;  OS: Windows Server 2003 R2 3790 Service Pack 1 (Windows Server 2003 R2 5.2)&#xa;  Name: WORKGROUP\XORCISM-LXTE4KS&#xa;  System time: 2011-04-15 09:41:57 UTC+0&#xa;"/>
                                myRegex = new Regex(Regex.Escape("(") + "Samba [^<>]*" + Regex.Escape(")"));    //Hardcoded
                                strTemp = myRegex.Match(scriptNode.Attributes["output"].InnerText).ToString();
                                if (strTemp != "")
                                {
                                    strTemp = strTemp.Replace("(", "");
                                    strTemp = strTemp.Replace(")", "");
                                    //Console.WriteLine("strTemp=" + strTemp);
                                    sambaversion = strTemp;
                                }
                                myRegex = new Regex("OS: Windows [^<>]*" + Regex.Escape(")"));  //Hardcoded
                                strTemp = myRegex.Match(scriptNode.Attributes["output"].InnerText).ToString();
                                if (strTemp != "")
                                {
                                    strTemp = strTemp.Replace("OS: ", "");
                                    //Console.WriteLine("strTemp=" + strTemp);
                                    sambaosversion = strTemp;
                                }
                                break;
                            default:
                                break;
                        }
                    }

                    // ================
                    // Handle the ports
                    // ================
                    Utils.Helper_Trace("XORCISM PROVIDER NMAP", string.Format("Handle the ports"));

                    XmlNodeList portNodes;
                    portNodes = nodeHost.SelectNodes("ports/port"); //Hardcoded
                    foreach (XmlNode portNode in portNodes)
                    {
                        XProviderCommon.PROTOCOL protocol = (XProviderCommon.PROTOCOL)Enum.Parse(typeof(XProviderCommon.PROTOCOL), portNode.Attributes["protocol"].InnerText);
                        int portid = Convert.ToInt32(portNode.Attributes["portid"].InnerText);

                        string service = "";
                        XmlNode node1;
                        node1 = portNode.SelectSingleNode("service/@name");
                        if (node1 != null)
                            service = node1.InnerText.ToUpper();

                        string version = "";
                        node1 = portNode.SelectSingleNode("service/@product");
                        if (node1 != null)
                            version = node1.InnerText;
                        node1 = portNode.SelectSingleNode("service/@version");
                        if (node1 != null)
                            version = version + " " + node1.InnerText;
                        version = version.Trim();

                        //Check if we retrieved the exact version of the service with the scripts
                        if (sambaversion != "" && version.Contains("Samba") && version.Contains(".X"))
                        {
                            version = sambaversion;
                        }

                    //    ScannablePort nmapPort;
                    //    nmapPort = new ScannablePort() { Protocol = protocol, Port = portid, Service = service, Version = version };

                        //    nmapAsset.Ports.Add(nmapPort);

                        #region updateendpoint
                        //Searches the endpoint for this session and updates it
                        Utils.Helper_Trace("XORCISM PROVIDER NMAP", string.Format("Updating endpoints"));
                        XORCISMEntities model = new XORCISMEntities();
                        var endpoints = from e in model.ENDPOINT
                                        where e.SessionID==sessionid && e.PortNumber==portid
                                        select e;

                        foreach (ENDPOINT myendpoint in endpoints.ToList())
                        {
                            try
                            {
                                Utils.Helper_Trace("XORCISM PROVIDER NMAP", string.Format("Updating endpoint = {0} {1}/{2} ({3}) {4}", myendpoint.EndPointID, myendpoint.ProtocolName, myendpoint.PortNumber, myendpoint.Service, myendpoint.Version));
                                Utils.Helper_Trace("XORCISM PROVIDER NMAP", "New version=" + version);
                                myendpoint.Version = version;
                                model.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                Utils.Helper_Trace("XORCISM PROVIDER NMAP", "Exception in update endpoint = " + ex.Message);
                                //return null;
                            }
                        }

                        #endregion updateendpoint

                    }

                    //====
                    // OS 
                    //====
                    Utils.Helper_Trace("XORCISM PROVIDER NMAP", string.Format("Dealing with OSname"));
                    XmlNodeList OsNodes;
                    OsNodes = nodeHost.SelectNodes("os/osmatch");   //Hardcoded
                    XmlNode nodeOs = OsNodes[0];
                    if (nodeOs != null)
                    {
                        string name = nodeOs.Attributes["name"].InnerText;
                        int accuracy = Convert.ToInt32(nodeOs.Attributes["accuracy"].InnerText);
                        Utils.Helper_Trace("XORCISM PROVIDER NMAP", "OSName=" + name);
                        //nmapAsset.OS.OSName = name;
                        if (sambaosversion != "")
                        {
                        //    nmapAsset.OS.OSName = sambaosversion;
                            Utils.Helper_Trace("XORCISM PROVIDER NMAP", "sambaosversion=" + sambaosversion);
                        }
                        //nmapAsset.OS.Accuracy = accuracy;
                        //nmapAsset.OS.Description = "";
                    }

                    //tabNmapAsset.Add(nmapAsset);
                }
            }
            catch (Exception ex)
            {
                Utils.Helper_Trace("XORCISM PROVIDER NMAP", "Exception while parsing XML results = " + ex.Message);
                //return null;
            }

        }

        public List<ScannableAsset> DiscoverHost(string target, HostStatus hostStatus, int jobid)
        {
            Utils.Helper_Trace("XORCISM PROVIDER NMAP", "Entering DiscoverHost");

            Utils.Helper_Trace("XORCISM PROVIDER NMAP", string.Format("Target is '{0}'", target));

            string outputXml=string.Empty;
            Assembly a;
            a = Assembly.GetCallingAssembly();

            string program;
            program =Path.GetDirectoryName(a.Location) + @"\Nmap\nmap.exe"; //Hardcoded

            Utils.Helper_Trace("XORCISM PROVIDER NMAP", string.Format("Using nmap at '{0}'", program));

            string nmapfile;
            nmapfile = string.Format("nmap{0}_{1}", DateTime.Now.Ticks, this.GetHashCode());

            Utils.Helper_Trace("XORCISM PROVIDER NMAP", string.Format("Command line is [nmap {0}]", "--no-stylesheet -O -A -sC -P0 -oX "+nmapfile+".xml " + target));

            Process process;
            process = new Process();
          
            process.StartInfo.UseShellExecute = true;
          
            try
            {
                process.StartInfo.FileName                  = program;
                //process.StartInfo.WorkingDirectory = Path.GetDirectoryName(a.Location) + @"\Nmap\";
                process.StartInfo.Arguments                 = " --no-stylesheet -O -A -sC -P0 -oX "+nmapfile+".xml " + target;  //Hardcoded
                process.StartInfo.UseShellExecute           = false;
                process.StartInfo.RedirectStandardOutput    = true;
                process.StartInfo.RedirectStandardError     = false;
                process.StartInfo.CreateNoWindow            = true;
                // process.EnableRaisingEvents = true;
                // process.Exited += new EventHandler(Process_Exited);
                process.Start();
                // Process.Start(vProgram,vIAnnotationLocal.Folder + vIAnnotationLocal.EntryPoint);
            }
            catch (Win32Exception vException)
            {
                if (vException.NativeErrorCode == ERROR_FILE_NOT_FOUND)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER NMAP", string.Format("ERROR_FILE_NOT_FOUND : Exception = {0}", vException.Message));
                    //return null;
                }
                else if (vException.NativeErrorCode == ERROR_ACCESS_DENIED)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER NMAP", string.Format("ERROR_ACCESS_DENIED : Exception = {0}", vException.Message));
                    //return null;
                }
            }

            Utils.Helper_Trace("XORCISM PROVIDER NMAP", string.Format("Nmap is running"));

            XmlDocument doc;

            try
            {              
                Utils.Helper_Trace("XORCISM PROVIDER NMAP", string.Format("Waiting for Nmap to finish"));

                process.WaitForExit(600000);    //HARDCODED 1 hour
            }
            catch (Exception vException)
            {
                Utils.Helper_Trace("XORCISM PROVIDER NMAP", string.Format("Exception = {0}", vException.Message));
                //return null;
            }

            Utils.Helper_Trace("XORCISM PROVIDER NMAP", "Nmap has finished");
            StreamReader SR = process.StandardOutput;
            string strOutput = SR.ReadToEnd();
            Utils.Helper_Trace("XORCISM PROVIDER NMAP", string.Format("Output: " + strOutput));

            if(strOutput.Contains("0 hosts up"))
            {
                Utils.Helper_Trace("XORCISM PROVIDER NMAP", "0 hosts up");
                process.Close();
                //return null;
            }
            if (strOutput.Contains("Windows does not support scanning your own machine"))
            {
                Utils.Helper_Trace("XORCISM PROVIDER NMAP", "Windows does not support scanning your own machine");
                process.Close();
                //return null;
            }

            doc = new XmlDocument();
            string m_data = string.Empty;
            try
            {  
                //TODO: Input Validation (XML)
                //doc.Load(process.StandardOutput);
                doc.Load(nmapfile+".xml");
                m_data = doc.InnerXml;
            }
            catch (Exception vException)
            {
                Utils.Helper_Trace("XORCISM PROVIDER NMAP", string.Format("Exception Load = {0}", vException.Message));
                process.Close();
                //return null;
            }
            process.Close();
            //doc.Save("C:\\NmapResults.xml");

            //Utils.Helper_Trace("XORCISM PROVIDER NMAP", string.Format("XML results have been saved to 'C:\\NmapResults.xml'"));

            List<ScannableAsset> tabNmapAsset;
            tabNmapAsset = new List<ScannableAsset>();

            try
            {
                string xpath = string.Empty;

                if (hostStatus == HostStatus.Both)
                    //xpath = string.Format("/nmaprun/host/status[@state='up']");
                    xpath = "/nmaprun/host/status"; //Hardcoded
                else if (hostStatus == HostStatus.Alive)
                    xpath = "/nmaprun/host/status[@state='up']";
                else
                    xpath = "/nmaprun/host/status[@state='down']";

                XmlNodeList nodes;
                nodes = doc.SelectNodes(xpath);

                foreach (XmlNode node in nodes)
                {
                    ScannableAsset nmapAsset;
                    nmapAsset = new ScannableAsset();

                    XmlNode nodeHost;
                    nodeHost = node.ParentNode;

                    // ====================
                    // Handle the addresses
                    // ====================
                    Utils.Helper_Trace("XORCISM PROVIDER NMAP", "Handles the addresses");
                    XmlNodeList addressNodes;
                    addressNodes = nodeHost.SelectNodes("address"); //Hardcoded
                    foreach (XmlNode addressNode in addressNodes)
                    {
                        string addr = addressNode.Attributes["addr"].InnerText;
                        ScannableAddress.ADDRESSTYPE addrtype = (ScannableAddress.ADDRESSTYPE)Enum.Parse(typeof(ScannableAddress.ADDRESSTYPE), addressNode.Attributes["addrtype"].InnerText);

                        nmapAsset.Addresses.Add(new ScannableAddress() { Address = addr, AddressType = addrtype });
                    }

                    //===
                    // Scripts
                    //===
                    Utils.Helper_Trace("XORCISM PROVIDER NMAP", "Handles the scripts");
                    XmlNodeList ScriptsNodes;
                    ScriptsNodes = nodeHost.SelectNodes("hostscript/script");   //Hardcoded
                    string strTemp = string.Empty;
                    string sambaversion = string.Empty;
                    string sambaosversion = string.Empty;
                    string macaddress = string.Empty;
                    Regex myRegex = new Regex("");
                    foreach (XmlNode scriptNode in ScriptsNodes)
                    {
                        switch (scriptNode.Attributes["id"].InnerText)
                        {
                            case "nbstat":
                                //<script id="nbstat" output="NetBIOS name: xmachine, NetBIOS user: &lt;unknown&gt;, NetBIOS MAC: &lt;unknown&gt;"/>
                                //<script id="nbstat" output="NetBIOS name: XORCISM-LXTE4KS, NetBIOS user: &lt;unknown&gt;, NetBIOS MAC: 00:0c:29:23:13:b9 (VMware)&#xa;"/>
                                myRegex = new Regex("NetBIOS MAC: [^<>]*");
                                strTemp = myRegex.Match(scriptNode.Attributes["output"].InnerText).ToString();
                                if (strTemp != "")
                                {
                                    if (strTemp.Contains("unknown"))
                                    {

                                    }
                                    else
                                    {
                                        strTemp = strTemp.Replace("NetBIOS MAC: ", "");
                                        strTemp = strTemp.Replace("&#xa;", "");
                                        //Console.WriteLine("strTemp=" + strTemp);
                                        macaddress = strTemp;
                                        Utils.Helper_Trace("XORCISM PROVIDER NMAP", string.Format("macaddress = {0}", macaddress));
                                    }
                                }
                                break;
                            case "smbv2-enabled":
                                //<script id="smbv2-enabled" output="Server doesn&apos;t support SMBv2 protocol"/>
                                //<script id="smbv2-enabled" output="Server doesn&apos;t support SMBv2 protocol"/>

                                break;
                            case "smb-os-discovery":
                                //<script id="smb-os-discovery" output="&#xa;  OS: Unix (Samba 3.4.7)&#xa;  Name: Unknown\Unknown&#xa;  System time: 2011-04-14 15:14:49 UTC+2&#xa;"/>
                                //<script id="smb-os-discovery" output=" &#xa;  OS: Windows Server 2003 R2 3790 Service Pack 1 (Windows Server 2003 R2 5.2)&#xa;  Name: WORKGROUP\XORCISM-LXTE4KS&#xa;  System time: 2011-04-15 09:41:57 UTC+0&#xa;"/>
                                myRegex = new Regex(Regex.Escape("(") + "Samba [^<>]*" + Regex.Escape(")"));
                                strTemp = myRegex.Match(scriptNode.Attributes["output"].InnerText).ToString();
                                if (strTemp != "")
                                {
                                    strTemp = strTemp.Replace("(", "");
                                    strTemp = strTemp.Replace(")", "");
                                    //Console.WriteLine("strTemp=" + strTemp);
                                    sambaversion = strTemp;
                                }
                                myRegex = new Regex("OS: Windows [^<>]*" + Regex.Escape(")"));
                                strTemp = myRegex.Match(scriptNode.Attributes["output"].InnerText).ToString();
                                if (strTemp != "")
                                {
                                    strTemp = strTemp.Replace("OS: ", "");
                                    //Console.WriteLine("strTemp=" + strTemp);
                                    sambaosversion = strTemp;
                                }
                                break;
                            default:
                                break;
                        }
                    }

                    // ================
                    // Handles the ports
                    // ================
                    Utils.Helper_Trace("XORCISM PROVIDER NMAP", "Handle the ports");
                    XmlNodeList portNodes;
                    portNodes = nodeHost.SelectNodes("ports/port"); //Hardcoded
                    foreach (XmlNode portNode in portNodes)
                    {
                        XProviderCommon.PROTOCOL protocol = (XProviderCommon.PROTOCOL)Enum.Parse(typeof(XProviderCommon.PROTOCOL), portNode.Attributes["protocol"].InnerText);
                        int portid = Convert.ToInt32(portNode.Attributes["portid"].InnerText);

                        string service = "";
                        XmlNode node1;
                        node1 = portNode.SelectSingleNode("service/@name");
                        if(node1 != null)
                             service = node1.InnerText.ToUpper();

                        string version = "";
                        node1 = portNode.SelectSingleNode("service/@product");
                        if (node1 != null)
                            version = node1.InnerText;
                        node1 = portNode.SelectSingleNode("service/@version");
                        if (node1 != null)
                            version = version+ " "+node1.InnerText;
                            version = version.Trim();

                        //Check if we retrieved the exact version of the service with the scripts
                        if (sambaversion != "" && version.Contains("Samba") && version.Contains(".X"))
                        {
                            version = sambaversion;
                        }

                        ScannablePort nmapPort;
                        try
                        {
                            nmapPort = new ScannablePort() { Protocol = protocol, Port = portid, Service = service, Version = version };
                            nmapAsset.Ports.Add(nmapPort);
                        }
                        catch (Exception ex)
                        {
                            Utils.Helper_Trace("XORCISM PROVIDER NMAP", "Exception ScannablePort = " + ex.Message+" "+ex.InnerException);
                            //return null;
                        }
                    }

                    //====
                    // OS 
                    //====
                    Utils.Helper_Trace("XORCISM PROVIDER NMAP", "Looking at OSname");
                    XmlNodeList OsNodes;
                    OsNodes = nodeHost.SelectNodes("os/osmatch");   //Hardcoded
                    XmlNode nodeOs = OsNodes[0];
                    if (nodeOs != null)
                    {
                        string name = nodeOs.Attributes["name"].InnerText;
                        int accuracy = Convert.ToInt32(nodeOs.Attributes["accuracy"].InnerText);
                        nmapAsset.OS.OSName = name;
                        if (sambaosversion != "")
                        {
                            nmapAsset.OS.OSName = sambaosversion;
                        }
                        nmapAsset.OS.Accuracy = accuracy;
                        nmapAsset.OS.Description = "";
                    }
                    
                    tabNmapAsset.Add(nmapAsset);
                }
            }
            catch(Exception ex)
            {
                Utils.Helper_Trace("XORCISM PROVIDER NMAP", "Exception while parsing XML results = " + ex.Message+" "+ex.InnerException);
                //return null;
            }

            Utils.Helper_Trace("XORCISM PROVIDER NMAP", "Number of assets found = " + tabNmapAsset.Count);
            try
            {
                Utils.Helper_Trace("XORCISM PROVIDER NMAP", string.Format("Updating job {0} status to FINISHED", jobid));
                string status = XCommon.STATUS.FINISHED.ToString();
                XORCISMEntities model = new XORCISMEntities();
                var Q = from j in model.JOB
                        where j.JobID == jobid
                        select j;

                JOB myJob = Q.FirstOrDefault();
                myJob.Status = status;
                myJob.DateEnd = DateTimeOffset.Now;
                //image
                System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
                myJob.XmlResult = encoding.GetBytes(m_data);
                model.SaveChanges();
            }
            catch (Exception ex)
            {
                Utils.Helper_Trace("XORCISM PROVIDER NMAP", "Exception = " + ex.Message);
                return null;
            }

            Utils.Helper_Trace("XORCISM PROVIDER NMAP", "Leaving DiscoverHost");

            return tabNmapAsset;
        }

        private string SeriliazeInString(List<ScannableAsset> listAsset)
        {
            string outPutResult = string.Empty;
            StringWriter writer=new StringWriter();
            XmlSerializer ser = new XmlSerializer(listAsset.GetType());

          ser.Serialize(writer, listAsset);
            outPutResult=  writer.ToString();
            return outPutResult;
        }
    }
   
}
