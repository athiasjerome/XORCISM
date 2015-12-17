using System;
using System.Collections.Generic;
using System.Linq;
using FSM.DotNetSSH;
using XCommon;
using System.Text.RegularExpressions;

namespace XProviderMetasploit
{
    /// <summary>
    /// Copyright (C) 2012-2015 Jerome Athias
    /// Metasploit Framework plugin for XORCISM (well just a chunk :p)
    /// All trademarks and registered trademarks are the property of their respective owners.
    /// This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation; either version 2 of the License, or (at your option) any later version.
    /// 
    /// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
    /// 
    /// You should have received a copy of the GNU General Public License along with this program; if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
    /// </summary>
    public class Program
    {
        //TODO
        //https://github.com/rapid7/metasploit_data_models/tree/master/app/models/mdm

        private static int port;
        private static string address, username, password;
        //private static string prompt = "root@xmachine:";
        //private static string prompt1 = prompt + "~#";
        //private static string promptend = "#";
        private static string prompt = "root";//@backtrack:";
        private static string prompt1 = prompt;// + "~$";
        private static string promptend = "";//"$";

        private static string PASS_FILE = "/home/root/tools/hydra-6.2-src/password.lst";    //Hardcoded

        static SshShell connect()
        {
            port = 22;
            /*
            address = "111.222.333.444";
            username = "root";
            password = "toor";
            */
            //HARDCODED
            address = "111.222.333.444";
            username = "root";
            password = "toor";

            SshShell sshShell;
            sshShell = new SshShell(address, username, password);
            sshShell.RemoveTerminalEmulationCharacters = true;

            //string prompt;
            //prompt = "root@xmachine:~#";

            //exec.Connect(address);
            //exec.Login(username, password);
            try
            {
                sshShell.Connect(port);
                sshShell.Expect(prompt1);
            }
            catch (Exception ex)
            {
                Utils.Helper_Trace("XORCISM PROVIDER METASPLOIT", string.Format("ConnectingERROR to METASPLOIT server at {0} : " + ex.Message + " " + ex.InnerException, address));
                //HARDCODED
                address = "111.222.333.444";
                username = "root";
                password = "toor";
                prompt = "root";//@backtrack:";
                sshShell = new SshShell(address, username, password);
                sshShell.RemoveTerminalEmulationCharacters = true;
                
                //prompt1 = prompt + "~$";
                Utils.Helper_Trace("XORCISM PROVIDER METASPLOIT", string.Format("Connecting to METASPLOIT server at {0}", address));
                try
                {
                    sshShell.Connect(port);
                    sshShell.Expect(prompt);
                }
                catch (Exception ex2)
                {
                    Utils.Helper_Trace("XORCISM PROVIDER METASPLOIT", string.Format("ConnectingERROR to METASPLOIT server at {0} : " + ex2.Message + " " + ex2.InnerException, address));
                }
            }

            Console.WriteLine("connected");

            string cmd1 = "cd /home/root/tools/metasploitsvn";  //Hardcoded

            sshShell.WriteLine(cmd1);
            prompt1 = prompt + "/home/root/tools/metasploitsvn" + promptend;    //Hardcoded
            string stdout = sshShell.Expect(prompt1);

            //==============================================

            cmd1 = "svn update";    //Hardcoded

            sshShell.WriteLine(cmd1);
            stdout = sshShell.Expect(prompt1);

            return sshShell;
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        //static void Main()
        static void blabla()
        {
            //int port;
//            string address, username, password;

            SshShell sshShell = connect();

            string stdout = "";
            //string stderr = "";

            string cmd1 = "cd /home/root/tools/metasploitsvn";  //Hardcoded

            sshShell.WriteLine(cmd1);
            stdout = sshShell.Expect(prompt);

            //==============================================

            cmd1 = "svn update";    //Hardcoded

            sshShell.WriteLine(cmd1);
            stdout = sshShell.Expect(prompt);

            Utils.Helper_Trace("XORCISM PROVIDER METASPLOIT", string.Format("svn update = {0}", stdout));

        }

        public void test(string teststring)
        {
            Console.WriteLine(teststring);
        }

        public void search_email_collector(string domainname)
        {
            SshShell sshShell=connect();

            string stdout = "";
            //string stderr = "";

            string cmd1 = "cd /home/root/tools/metasploitsvn";   //TODO Hardcoded
            prompt1 = prompt + "/home/root/tools/metasploitsvn" + promptend;

            sshShell.WriteLine(cmd1);
            stdout = sshShell.Expect(prompt1);

            cmd1 = "./msfcli auxiliary/gather/search_email_collector DOMAIN=" + domainname + " E";  //Hardcoded

            sshShell.WriteLine(cmd1);
            stdout = sshShell.Expect(prompt1);

            /*
            DOMAIN => target.com
            [*] Harvesting emails .....
            [*] Searching Google for email addresses from target.com
            [*] Extracting emails from Google search results...
            [*] Searching Bing email addresses from target.com
            [*] Extracting emails from Bing search results...
            [*] Searching Yahoo for email addresses from target.com
            [*] Extracting emails from Yahoo search results...
            [*] Located 2 email addresses for target.com
            [*]     contact@target.com
            [*]     boss@target.com
            [*] Auxiliary module execution completed
            */

            string[] mytab = Regex.Split(stdout, "\r\n");
            foreach (string line in mytab)
            {
                if (line.Contains("@" + domainname))
                {
                    Console.WriteLine(line.Replace("[*]", "").Trim());
                }
            }


            sshShell.Close();
        }

        //public void smb_enumusers(string mytarget)
        public List<string> smb_enumusers(string mytarget)
        {
            List<string> SMBusers = new List<string>();
            SshShell sshShell = connect();
            string stdout = "";
            //string stderr = "";

            string cmd1 = "cd /home/root/tools/metasploitsvn";  //Hardcoded
            prompt1 = prompt + "/home/root/tools/metasploitsvn" + promptend;

            //sshShell.WriteLine(cmd1);
            //stdout = sshShell.Expect(prompt1);

            cmd1 = "./msfcli auxiliary/scanner/smb/smb_enumusers RHOSTS=" + mytarget + " E";    //Hardcoded

            sshShell.WriteLine(cmd1);
            stdout = sshShell.Expect(prompt1);

            Console.WriteLine(stdout);
            /*
            RHOSTS => 111.222.333.444
            [*] 111.222.333.444 xmachine [ nobody, nxpgsql ] ( LockoutTries=0 PasswordMin=5
            )
            [*] Scanned 1 of 1 hosts (100% complete)
            [*] Auxiliary module execution completed
            */

            Regex myRegex = new Regex(Regex.Escape("[") + "[^<>]*" +Regex.Escape("]"));
            string[] mytab = Regex.Split(stdout, "\r\n");
            foreach (string line in mytab)
            {
                if (line.Contains(mytarget))
                {
                    string strTemp = myRegex.Match(line.Replace("[*]", "")).ToString();
                    if (strTemp != "")
                    {
                        //Console.WriteLine("users:" + strTemp);
                        //[ nobody, nxpgsql ]
                        strTemp = strTemp.Replace("[", "");
                        strTemp = strTemp.Replace("]", "").Trim();
                        string[] myusers = Regex.Split(strTemp, ",");
                        foreach (string user in myusers)
                        {
                            Console.WriteLine(user.Trim());
                            SMBusers.Add(user.Trim());
                        }
                    }
                }
            }

            sshShell.Close();
            return SMBusers;
        }

        public void dns_enum(string domainname)
        {
            SshShell sshShell = connect();
            string stdout = "";
            //string stderr = "";

            string cmd1 = "cd /home/root/tools/metasploitsvn";  //Hardcoded
            prompt1 = prompt + "/home/root/tools/metasploitsvn" + promptend;

            sshShell.WriteLine(cmd1);
            stdout = sshShell.Expect(prompt1);

            cmd1 = "./msfcli auxiliary/gather/dns_enum DOMAIN=" + domainname + " E";    //Hardcoded

            sshShell.WriteLine(cmd1);
            stdout = sshShell.Expect(prompt1);

            Console.Write(stdout);

            sshShell.Close();
        }

        public void vnc_login(string target)
        {
            SshShell sshShell = connect();
            string stdout = "";
            //string stderr = "";

            string cmd1 = "cd /home/root/tools/metasploitsvn";  //Hardcoded
            prompt1 = prompt + "/home/root/tools/metasploitsvn" + promptend;

            sshShell.WriteLine(cmd1);
            stdout = sshShell.Expect(prompt1);

            cmd1 = "./msfcli auxiliary/scanner/vnc/vnc_login RHOSTS=" + target + " PASS_FILE="+PASS_FILE+" STOP_ON_SUCCESS=true E"; //Hardcoded

            sshShell.WriteLine(cmd1);
            stdout = sshShell.Expect(prompt1);

            Console.Write(stdout);
            /*
            [*] 8.6.6.8:5900 - Attempting VNC login with password 'computer'
            [*] 8.6.6.8:5900, VNC server protocol version : 3.3
            [+] 8.6.6.8:5900, VNC server password : "computer"
            [*] Scanned 1 of 1 hosts (100% complete)
            [*] Auxiliary module execution completed
            */
            Regex myRegex = new Regex("VNC server password : "+Regex.Escape("\"") + "[^<>]*" + Regex.Escape("\""));
            string strTemp = myRegex.Match(stdout).ToString();
            if (strTemp != "")
            {
                strTemp = strTemp.Replace("VNC server password : ", "");
                strTemp = strTemp.Replace("\"", "");
                Console.Write(strTemp);
            }

            sshShell.Close();
        }

        public void ftp_version(string target)
        {
            /*
            msf > use auxiliary/scanner/ftp/ftp_version
            msf auxiliary(ftp_version) > show options

            Module options (auxiliary/scanner/ftp/ftp_version):

               Name     Current Setting      Required  Description
               ----     ---------------      --------  -----------
               FTPPASS  mozilla@example.com  no        The password for the specified userna
            me
               FTPUSER  anonymous            no        The username to authenticate as
               RHOSTS                        yes       The target address range or CIDR iden
            tifier
               RPORT    21                   yes       The target port
               THREADS  1                    yes       The number of concurrent threads

            msf auxiliary(ftp_version) > set RHOSTS 8.6.6.8
            RHOSTS => 8.6.6.8
            msf auxiliary(ftp_version) > exploit

            [*] Scanned 1 of 1 hosts (100% complete)
            [*] Auxiliary module execution completed
            msf auxiliary(ftp_version) > exploit

            [*] 8.6.6.8:21 FTP Banner: '220-Jgaa's Fan Club FTP service\x0d\x0a    WarF
            TPd 1.82.00-RC11 (Sep 22 2006) Ready\x0d\x0a'
            [*] Scanned 1 of 1 hosts (100% complete)
            [*] Auxiliary module execution completed
            */
        }


    }
}
