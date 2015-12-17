using System;
using System.Collections.Generic;
using System.Linq;
using FSM.DotNetSSH;

namespace XProviderSqlmap
{

    public class Engine
    {
        /// <summary>
        /// Copyright (C) 2012-2015 Jerome Athias
        /// XORCISM Plugin for sqlmap
        /// All trademarks and registered trademarks are the property of their respective owners.
        /// This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation; either version 2 of the License, or (at your option) any later version.
        /// 
        /// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
        /// 
        /// You should have received a copy of the GNU General Public License along with this program; if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
        /// </summary>
        [STAThread]
        public void Batch(string Url, string parameters)
        {
            /*
            Usage: python ./sqlmap.py [options]

Options:
  --version             show program's version number and exit
  -h, --help            show this help message and exit
  -v VERBOSE            Verbosity level: 0-6 (default 1)

  Target:
    At least one of these options has to be specified to set the source to
    get target urls from.

    -d DIRECT           Direct connection to the database
    -u URL, --url=URL   Target url
    -l LIST             Parse targets from Burp or WebScarab proxy logs
    -r REQUESTFILE      Load HTTP request from a file
    -g GOOGLEDORK       Process Google dork results as target urls
    -c CONFIGFILE       Load options from a configuration INI file

  Request:
    These options can be used to specify how to connect to the target url.

    --data=DATA         Data string to be sent through POST
    --cookie=COOKIE     HTTP Cookie header
    --cookie-urlencode  URL Encode generated cookie injections
    --drop-set-cookie   Ignore Set-Cookie header from response
    --user-agent=AGENT  HTTP User-Agent header
    --random-agent      Use randomly selected HTTP User-Agent header
    --referer=REFERER   HTTP Referer header
    --headers=HEADERS   Extra HTTP headers newline separated
    --auth-type=ATYPE   HTTP authentication type (Basic, Digest or NTLM)
    --auth-cred=ACRED   HTTP authentication credentials (name:password)
    --auth-cert=ACERT   HTTP authentication certificate (key_file,cert_file)
    --proxy=PROXY       Use a HTTP proxy to connect to the target url
    --proxy-cred=PCRED  HTTP proxy authentication credentials (name:password)
    --ignore-proxy      Ignore system default HTTP proxy
    --delay=DELAY       Delay in seconds between each HTTP request
    --timeout=TIMEOUT   Seconds to wait before timeout connection (default 30)
    --retries=RETRIES   Retries when the connection timeouts (default 3)
    --scope=SCOPE       Regexp to filter targets from provided proxy log
    --safe-url=SAFURL   Url address to visit frequently during testing
    --safe-freq=SAFREQ  Test requests between two visits to a given safe url

  Optimization:
    These options can be used to optimize the performance of sqlmap.

    -o                  Turn on all optimization switches
    --predict-output    Predict common queries output
    --keep-alive        Use persistent HTTP(s) connections
    --null-connection   Retrieve page length without actual HTTP response body
    --threads=THREADS   Max number of concurrent HTTP(s) requests (default 1)

  Injection:
    These options can be used to specify which parameters to test for,
    provide custom injection payloads and optional tampering scripts.

    -p TESTPARAMETER    Testable parameter(s)
    --dbms=DBMS         Force back-end DBMS to this value
    --os=OS             Force back-end DBMS operating system to this value
    --prefix=PREFIX     Injection payload prefix string
    --suffix=SUFFIX     Injection payload suffix string
    --tamper=TAMPER     Use given script(s) for tampering injection data

  Detection:
    These options can be used to specify how to parse and compare page
    content from HTTP responses when using blind SQL injection technique.

    --level=LEVEL       Level of tests to perform (1-5, default 1)
    --risk=RISK         Risk of tests to perform (0-3, default 1)
    --string=STRING     String to match in page when the query is valid
    --regexp=REGEXP     Regexp to match in page when the query is valid
    --text-only         Compare pages based only on the textual content

  Techniques:
    These options can be used to tweak testing of specific SQL injection
    techniques.

    --technique=TECH    SQL injection techniques to test for (default BEUST)
    --time-sec=TIMESEC  Seconds to delay the DBMS response (default 5)
    --union-cols=UCOLS  Range of columns to test for UNION query SQL injection
    --union-char=UCHAR  Character to use for bruteforcing number of columns
  Fingerprint:
    -f, --fingerprint   Perform an extensive DBMS version fingerprint

  Enumeration:
    These options can be used to enumerate the back-end database
    management system information, structure and data contained in the
    tables. Moreover you can run your own SQL statements.

    -b, --banner        Retrieve DBMS banner
    --current-user      Retrieve DBMS current user
    --current-db        Retrieve DBMS current database
    --is-dba            Detect if the DBMS current user is DBA
    --users             Enumerate DBMS users
    --passwords         Enumerate DBMS users password hashes
    --privileges        Enumerate DBMS users privileges
    --roles             Enumerate DBMS users roles
    --dbs               Enumerate DBMS databases
    --tables            Enumerate DBMS database tables
    --columns           Enumerate DBMS database table columns
    --dump              Dump DBMS database table entries
    --dump-all          Dump all DBMS databases tables entries
    --search            Search column(s), table(s) and/or database name(s)
    -D DB               DBMS database to enumerate
    -T TBL              DBMS database table to enumerate
    -C COL              DBMS database table column to enumerate
    -U USER             DBMS user to enumerate
    --exclude-sysdbs    Exclude DBMS system databases when enumerating tables
    --start=LIMITSTART  First query output entry to retrieve
    --stop=LIMITSTOP    Last query output entry to retrieve
    --first=FIRSTCHAR   First query output word character to retrieve
    --last=LASTCHAR     Last query output word character to retrieve
    --sql-query=QUERY   SQL statement to be executed
    --sql-shell         Prompt for an interactive SQL shell

  Brute force:
    These options can be used to run brute force checks.

    --common-tables     Check existence of common tables
    --common-columns    Check existence of common columns

  User-defined function injection:
    These options can be used to create custom user-defined functions.
    --udf-inject        Inject custom user-defined functions
    --shared-lib=SHLIB  Local path of the shared library

  File system access:
    These options can be used to access the back-end database management
    system underlying file system.

    --file-read=RFILE   Read a file from the back-end DBMS file system
    --file-write=WFILE  Write a local file on the back-end DBMS file system
    --file-dest=DFILE   Back-end DBMS absolute filepath to write to

  Operating system access:
    These options can be used to access the back-end database management
    system underlying operating system.

    --os-cmd=OSCMD      Execute an operating system command
    --os-shell          Prompt for an interactive operating system shell
    --os-pwn            Prompt for an out-of-band shell, meterpreter or VNC
    --os-smbrelay       One click prompt for an OOB shell, meterpreter or VNC
    --os-bof            Stored procedure buffer overflow exploitation
    --priv-esc          Database process' user privilege escalation
    --msf-path=MSFPATH  Local path where Metasploit Framework 3 is installed
    --tmp-path=TMPPATH  Remote absolute path of temporary files directory

  Windows registry access:
    These options can be used to access the back-end database management
    system Windows registry.

    --reg-read          Read a Windows registry key value
    --reg-add           Write a Windows registry key value data
    --reg-del           Delete a Windows registry key value
    --reg-key=REGKEY    Windows registry key
    --reg-value=REGVAL  Windows registry key value
    --reg-data=REGDATA  Windows registry key value data
    --reg-type=REGTYPE  Windows registry key value type

  General:
    These options can be used to set some general working parameters.
    -t TRAFFICFILE      Log all HTTP traffic into a textual file
    -s SESSIONFILE      Save and resume all data retrieved on a session file
    --flush-session     Flush session file for current target
    --fresh-queries     Ignores query results stored in session file
    --eta               Display for each output the estimated time of arrival
    --update            Update sqlmap
    --save              Save options on a configuration INI file
    --batch             Never ask for user input, use the default behaviour

  Miscellaneous:
    --beep              Alert when sql injection found
    --check-payload     IDS detection testing of injection payloads
    --cleanup           Clean up the DBMS by sqlmap specific UDF and tables
    --forms             Parse and test forms on target url
    --gpage=GOOGLEPAGE  Use Google dork results from specified page number
    --page-rank         Display page rank (PR) for Google dork results
    --parse-errors      Parse DBMS error messages from response pages
    --replicate         Replicate dumped data into a sqlite3 database
    --tor               Use default Tor (Vidalia/Privoxy/Polipo) proxy address
    --wizard            Simple wizard interface for beginner users
            */
            int port;
            string address, username, password;

            //HARDCODED
            port = 22;
            address = "111.222.333.444";
            username = "root";
            password = "toor";

            SshShell sshShell;
            sshShell = new SshShell(address, username, password);
            sshShell.RemoveTerminalEmulationCharacters = true;

            string prompt;
            prompt = "root@xmachine:~#";

            //exec.Connect(address);
            //exec.Login(username, password);

            sshShell.Connect(port);
            sshShell.Expect(prompt);

            string stdout = "";
            //string stderr = "";

            string cmd1 = "cd /home/root/tools/sqlmap"; //Hardcoded
            //string prompt1 = "root@xmachine:/home/tools/sqlmap#";
            string prompt1 = "root@backtrack:~/tools/sqlmap$";  //Hardcoded
            sshShell.WriteLine(cmd1);
            stdout = sshShell.Expect(prompt1);

            //==============================================

            cmd1 = "svn update";    //Hardcoded

            sshShell.WriteLine(cmd1);
            stdout = sshShell.Expect(prompt1);



            //"./sqlmap.py -u http://10.13.102.203/login.php --forms --batch"
            cmd1 = "./sqlmap.py -u "+Url+" "+parameters+" --level=3 --batch";

            sshShell.WriteLine(cmd1);
            stdout = sshShell.Expect(prompt1);

            Console.WriteLine(stdout);

            string webos = string.Empty;
            string webapptechno = string.Empty;
            string mysgbd = string.Empty;

            string[] myLines = stdout.Split(new char[] { '\n' });
            for (int cpt = 1; cpt < myLines.Length - 1; cpt++)
            {
                if (myLines[cpt].Contains("web server operating system:"))  //Hardcoded
                {
                    webos = myLines[cpt].Replace("web server operating system:", "").Trim();
                    Console.WriteLine("webos:" + webos);
                    //Windows
                }
                if (myLines[cpt].Contains("web application technology:"))
                {
                    webapptechno = myLines[cpt].Replace("web application technology:", "").Trim();
                    Console.WriteLine("webapptechno:" + webapptechno);
                    //PHP 5.3.5, Apache 2.2.17
                }
                if (myLines[cpt].Contains("back-end DBMS:"))
                {
                    mysgbd = myLines[cpt].Replace("back-end DBMS:", "").Trim();
                    Console.WriteLine("mysgbd:" + mysgbd);
                    //MySQL 5.0
                }
            }

            sshShell.Close();
        }
    }
}
