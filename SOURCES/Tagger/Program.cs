using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XORCISMModel;
using XATTACKModel;
using XOVALModel;
using XVULNERABILITYModel;

using System.Data;
using System.Linq;

namespace Tagger
{
    class Tagging
    {
        /// <summary>
        /// Copyright (C) 2014-2015 Jerome Athias
        /// Dirty tool to create (Hardcoded) TAGS/KEYWORDS (with a hierarchy) in an XORCISM database. It helps for cross-matching (mapping) between various data repositories. Also "optimize" the research a la Google (e.g. when searching References for a topic/term)
        /// Security Data Tagging
        /// This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation; either version 2 of the License, or (at your option) any later version.
        /// 
        /// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
        /// 
        /// You should have received a copy of the GNU General Public License along with this program; if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
        /// </summary>
        public static XORCISMEntities model = new XORCISMEntities();
        public static XATTACKEntities attack_model = new XATTACKEntities();
        public static XOVALEntities oval_model = new XOVALEntities();
        public static XVULNERABILITYEntities vuln_model = new XVULNERABILITYEntities();

        public static int iVocabularyXORCISMID = 0; //1050;

        static void Main(string[] args)
        {
            //model.Configuration.AutoDetectChangesEnabled = false;
            //model.Configuration.ValidateOnSaveEnabled = false;


            #region vocabularyxorcism
            try
            {
                //HARDCODED
                iVocabularyXORCISMID = model.VOCABULARY.Where(o => o.VocabularyName == "XORCISM").Select(o => o.VocabularyID).FirstOrDefault();
            }
            catch (Exception ex)
            {
                iVocabularyXORCISMID = 0;
            }
            if (iVocabularyXORCISMID <= 0)
            {
                XORCISMModel.VOCABULARY oVocabulary = new XORCISMModel.VOCABULARY();
                oVocabulary.CreatedDate = DateTimeOffset.Now;
                oVocabulary.VocabularyName = "XORCISM"; //HARDCODED
                model.VOCABULARY.Add(oVocabulary);
                model.SaveChanges();
                iVocabularyXORCISMID = oVocabulary.VocabularyID;
                Console.WriteLine("DEBUG iVocabularyXORCISMID=" + iVocabularyXORCISMID);    //DEBUG
            }
            #endregion vocabularyxorcism

            int iTagCookieID = 0;
            int iTagID = 0;

            //HARDCODED TAGS
            iTagCookieID = fAddTag("cookie");

            //***************************************************************************
            iTagID = fAddTag("HTTPOnly");
            fAddTagRelationship(iTagCookieID,iTagID,"Parent");  //For the Hierarchy of Tags
            int iTagID2 = fAddTag("http-only");
            fAddTagRelationship(iTagCookieID, iTagID, "Synonym");
            iTagID2 = fAddTag("HTTP only"); //TODO Review
            fAddTagRelationship(iTagCookieID, iTagID, "Synonym");

            iTagID = fAddTag("secure attribute");   //cookie SSL
            fAddTagRelationship(iTagCookieID, iTagID, "Parent");

            iTagID2 = fAddTag("secure flag");   //SSL
            fAddTagRelationship(iTagCookieID, iTagID2, "Parent");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");    //Cookie SSL

            iTagID = fAddTag("'secure' attribute");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");
            iTagID = fAddTag("'secure' flag");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");


            iTagID = fAddTag("X-Frame Options");
            iTagID2 = fAddTag("X-Frame-Options");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");
            iTagID2 = fAddTag("frame injection");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");
            iTagID2 = fAddTag("Clickjacking");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID = iTagID2;
            iTagID2 = fAddTag("Click jacking");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");
            iTagID2 = fAddTag("Click-jacking");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");

            iTagID = fAddTag("X-Content-Type Options"); //TODO

            iTagID = fAddTag("autocomplete");
            iTagID2 = fAddTag("auto-complete");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");

            iTagID = fAddTag("Remote Desktop");
            iTagID2 = fAddTag("rdesktop");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("RDP");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("crossdomain");
            iTagID2 = fAddTag("crossdomain.xml");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("Cross-Domain");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("Software");
            iTagID = fAddTag("Application");
            iTagID = fAddTag("Hardware");
            iTagID = fAddTag("Device");
            iTagID = fAddTag("Component");

            iTagID = fAddTag("Communication");
            iTagID2 = fAddTag("Communications");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("Communication Channel");
            fAddTagRelationship(iTagID, iTagID2, "Related");


            iTagID = fAddTag("Indeo");  //Microsoft Indeo Codec
            iTagID = fAddTag("Cinepak");
            iTagID = fAddTag("VP8 Codec");
            iTagID = fAddTag("K-Lite");
            iTagID = fAddTag("libvpx");
            iTagID = fAddTag("Winamp");
            iTagID = fAddTag("JetAudio");
            iTagID = fAddTag("Dovecot");
            iTagID = fAddTag("LISP");   //protocol
            iTagID = fAddTag("Collabtive");

            iTagID = fAddTag("blob");

            iTagID = fAddTag("Ogg");
            iTagID = fAddTag("vorbis");

            iTagID = fAddTag("DivX");
            iTagID = fAddTag("mpeg");
            iTagID = fAddTag("ffmpeg");

            iTagID = fAddTag("OpenType");

            iTagID = fAddTag("awstats");

            iTagID = fAddTag("phpgroupware");
            iTagID = fAddTag("egroupware");

            iTagID = fAddTag("lighttpd");

            iTagID = fAddTag("charset");

            iTagID = fAddTag("upload");
            iTagID2 = fAddTag("file upload");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("image upload");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("upload function");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("upload form");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("upload feature");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("upload function");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("infinite loop");
            iTagID2 = fAddTag("endless loop");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");

            iTagID = fAddTag("lockout");


            iTagID = fAddTag("business logic");
            iTagID = fAddTag("deadlock");
            iTagID = fAddTag("ActiveX");
                iTagID = fAddTag("inclusion");
            iTagID = fAddTag("HTTP Parameter");
            iTagID = fAddTag("HTTPS Parameter");

            iTagID = fAddTag("pointer dereference");
            iTagID2 = fAddTag("pointer de-reference");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");

            iTagID = fAddTag("behavioral");
            iTagID = fAddTag("viewstate");
            iTagID = fAddTag("environment");
            iTagID = fAddTag("environment variable");
            iTagID = fAddTag("external variable");
            iTagID = fAddTag("porous defense");
            iTagID = fAddTag("software fault pattern");
            iTagID = fAddTag("entry point");

            iTagID = fAddTag("LDAP");   //casesensitive?
            iTagID2 = fAddTag("LDAP Injection");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("Flash");
            iTagID = fAddTag("uninitialized");
            iTagID = fAddTag("improper control");

            iTagID = fAddTag("out of date");
            iTagID2 = fAddTag("outdated");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");
            iTagID2 = fAddTag("out-of-date");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");
            
            iTagID = fAddTag("open redirect");
            iTagID2 = fAddTag("unvalidated redirect");  //and forward (OWASP)
            fAddTagRelationship(iTagID, iTagID2, "Synonym");
            iTagID2 = fAddTag("URL Redirector Abuse");
            fAddTagRelationship(iTagID, iTagID2, "Related");


            iTagID = fAddTag("amplification");

            iTagID = fAddTag("race condition");

            iTagID = fAddTag("shatter attack");

            iTagID = fAddTag("handler");

            iTagID = fAddTag("null pointer");
            iTagID2 = fAddTag("null pointer dereference");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("vulnerability");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("mobile code");

            iTagID = fAddTag("TYPO3");
            iTagID = fAddTag("NodeJS");
            iTagID = fAddTag("JQuery");
            iTagID = fAddTag("Dundas");
            iTagID = fAddTag("OpenChart");
            iTagID = fAddTag("FreeChart");
            iTagID = fAddTag("hapi");   //server framework

            iTagID = fAddTag("eval injection");
            
            iTagID = fAddTag("malware");    //TODO
            iTagID2 = fAddTag("trojan");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("spyware");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("malicious application");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("malicious software");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("malicious product");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("malicious program");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("malicious code");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("malicious piece of code");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("trapdoor");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("backdoor");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("malware defense");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("Worm");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("Sasser");

            iTagID = fAddTag("hibernate");

            iTagID = fAddTag("best practice");
            iTagID = fAddTag("bad practice");

            iTagID = fAddTag("socket");
            iTagID = fAddTag("bypass");

            iTagID = fAddTag("XML External Entity");    //Expansion
            iTagID2 = fAddTag("XML External Entities");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");
            iTagID2 = fAddTag("XML Entity Expansion");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID = iTagID2;
            iTagID2 = fAddTag("XXE", true);
            fAddTagRelationship(iTagID, iTagID2, "Synonym");

            iTagID = fAddTag("directory listing");
            iTagID = fAddTag("directory indexing");

            iTagID = fAddTag("applet");
            iTagID = fAddTag("servlet");

            iTagID = fAddTag("MD5");
            iTagID = fAddTag("salt");
            iTagID = fAddTag("unsalted");
            iTagID = fAddTag("SHA1");
            iTagID = fAddTag("RC4");
            iTagID = fAddTag("NTLM");   //true?
            iTagID = fAddTag("bcrypt");
            iTagID = fAddTag("heimdal");

            iTagID = fAddTag("fckeditor");

            iTagID = fAddTag("LOFS");

            iTagID = fAddTag("ipsec");

            iTagID = fAddTag("side-channel attack");
            iTagID = fAddTag("private key");
            iTagID = fAddTag("key size");
            iTagID = fAddTag("public key");

            iTagID = fAddTag("sizeof");

            iTagID = fAddTag("cache");


            iTagID = fAddTag("array index error");

            iTagID = fAddTag("PDF file");
            iTagID2 = fAddTag("Acrobat Reader");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID = fAddTag("PDF document");   //TODO
            iTagID2 = fAddTag("Acrobat Reader");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("DOC file");
            //DOCX
            iTagID2 = fAddTag("Microsoft Word");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID = fAddTag("HTML file");
            iTagID = fAddTag("XLS file");
            iTagID2 = fAddTag("Excel");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("ZIP file");
            iTagID2 = fAddTag("WinZIP");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("ZIP archive");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("extract");   //Review
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("UnZIP");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("7zip");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("WinRAR");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("CUPS");

            iTagID = fAddTag("IPC"); //true
            iTagID = fAddTag("shared memory");

            
            iTagID = fAddTag("DNS server");
            iTagID2 = fAddTag("DNS client");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("DNS request");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("DNS lookup");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("DNS packet");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("DNS cache");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("BIND");   //ISC BIND

            iTagID = fAddTag("targeted web site");
            iTagID2 = fAddTag("targeted website");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");

            iTagID = fAddTag("Fedora");
            iTagID = fAddTag("Gentoo");

            iTagID = fAddTag("RTF");

            iTagID = fAddTag("exif");
            iTagID = fAddTag("setuid");
            iTagID = fAddTag("setgid");
            iTagID = fAddTag("inode");
            iTagID = fAddTag("Xorg");
            iTagID = fAddTag("Pixmap");
            iTagID = fAddTag("Pixmaps");
            iTagID = fAddTag("Pixel");
            
            iTagID = fAddTag("proxy");
            iTagID2 = fAddTag("Squid");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("Xscreensaver");

            iTagID = fAddTag("XSLT");

            iTagID = fAddTag("GSM");    //TODO
            iTagID = fAddTag("SMS");
            iTagID = fAddTag("MMS");

            iTagID = fAddTag("netfilter");

            iTagID = fAddTag("cdrom");

            

            iTagID = fAddTag("NETLINK");

            iTagID = fAddTag("netlogon");

            iTagID = fAddTag("Gaim");

            iTagID = fAddTag("KCMS");

            iTagID = fAddTag("ISAKMP");
            iTagID = fAddTag("Office Suite");
            iTagID = fAddTag("StarSuite");
            
            iTagID = fAddTag("64-bit"); //TODO
            iTagID = fAddTag("64bit");

            iTagID = fAddTag("32-bit"); //TODO
            iTagID = fAddTag("32bit");

            iTagID = fAddTag("HTTP response smuggling");


            iTagID = fAddTag("Oracle Java");
            iTagID2 = fAddTag("Java");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID = iTagID2;
            iTagID2 = fAddTag("Java Runtime Environment");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID = iTagID2;
            iTagID2 = fAddTag("JRE");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("Java");
            iTagID2 = fAddTag("JDK");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            
            iTagID2 = fAddTag("JAXB");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("Java Management Extensions");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("JMX");    //Java Management Extensions
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("SCSI");
            iTagID = fAddTag("CIFS");
            iTagID = fAddTag("umask");

            iTagID = fAddTag("GIF image");

            //TODO
            iTagID = fAddTag("JPG image");
            iTagID2 = fAddTag("JPEG image");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");
            iTagID2 = fAddTag("JPEG file");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("PNG image");
            iTagID = fAddTag("XPM image");
            iTagID = fAddTag(".lnk");
            iTagID = fAddTag("SVG");
            iTagID = fAddTag("Bitmap");
            iTagID = fAddTag("JBIG2");
            
            iTagID = fAddTag("symbolic link");
            iTagID2 = fAddTag("symlink");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");
            iTagID2 = fAddTag("symlink attack");
            fAddTagRelationship(iTagID, iTagID2, "Related");


            iTagID = fAddTag("log tampering");
            iTagID = fAddTag("tampering");
            iTagID = fAddTag("forgery");
            iTagID = fAddTag("repudiation");
            iTagID2 = fAddTag("non-repudiation");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("execution of arbitrary code");
            iTagID2 = fAddTag("arbitrary code execution");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");

            iTagID = fAddTag("forensics");
            iTagID = fAddTag("forensic");

            iTagID = fAddTag("drag and drop");
            iTagID2 = fAddTag("drag-and-drop");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");
            
            iTagID = fAddTag("mouse click");
            iTagID2 = fAddTag("click");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("onmouseover");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("onmousedown");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("freetype");

            iTagID = fAddTag("XFree86");
            iTagID = fAddTag("libxpm");

            iTagID = fAddTag("ACPI");
            iTagID = fAddTag("RPC");
            iTagID = fAddTag("DCOM");

            iTagID = fAddTag("COM object");
            iTagID = fAddTag("object COM");


            iTagID = fAddTag("printer");
            iTagID = fAddTag("ADSL");
            
            iTagID = fAddTag("Wi-Fi");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("WiFi");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");
            iTagID2 = fAddTag("wireless");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("Bluetooth");

            iTagID = fAddTag("driver");
            iTagID = fAddTag("firmware");
            iTagID = fAddTag("BIOS");
            iTagID = fAddTag("SELinux");    //TODO
            iTagID = fAddTag("SE Linux");

            iTagID = fAddTag("NULL pointer");
            iTagID = fAddTag("malformed file");
            iTagID = fAddTag("filesystem");
            iTagID = fAddTag("NTFS");
            iTagID = fAddTag("bitlock");
            iTagID = fAddTag("USB",true);
            iTagID = fAddTag("USB driver");

            iTagID = fAddTag("PostgreSQL");
            iTagID = fAddTag("MySQL");
            iTagID = fAddTag("SQLite");
            iTagID = fAddTag("WinDev");
            iTagID = fAddTag("hyperfile");

            iTagID = fAddTag("buffer over-read");
            iTagID = fAddTag("SDP server");
            iTagID = fAddTag("SDP packet");

            iTagID = fAddTag("../");
            iTagID = fAddTag("unsigned integer");

            iTagID = fAddTag("ISAKMP");

            iTagID = fAddTag("power consumption");
            iTagID = fAddTag("CPU consumption");
            iTagID = fAddTag("memory consumption");
            iTagID = fAddTag("space consumption");
            iTagID = fAddTag("insecure storage");
            iTagID = fAddTag("misconfiguration");

            iTagID = fAddTag("Server-Side Request Forgery");
            iTagID2 = fAddTag("SSRF");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");

            iTagID = fAddTag("denial of service");
            iTagID2 = fAddTag("DDOS");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");
            iTagID2 = fAddTag("denial-of-service");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");
            iTagID2 = fAddTag("DoS",true);
            fAddTagRelationship(iTagID, iTagID2, "Synonym");
            iTagID2 = fAddTag("application crash");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("exception handling");
            iTagID2 = fAddTag("error handling");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");
            iTagID2 = fAddTag("unhandled exception");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("debug error message");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID = iTagID2;
            iTagID2 = fAddTag("debugging error message");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");
            iTagID2 = fAddTag("debugging message");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("stack trace");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            //TODO

            iTagID = fAddTag("Session Management");
            iTagID2 = fAddTag("Session Prediction");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("session ID in URL");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID = iTagID2;
            iTagID2 = fAddTag("session ID");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID = iTagID2;
            iTagID = fAddTag("session identifier");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");
            iTagID2 = fAddTag("sessions ID");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("sessions identifier");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            //Each user must have unique UID

            iTagID = fAddTag("input validation");
            iTagID2 = fAddTag("input sanitization");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("input not properly sanitized");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("input not properly checked");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("input not correctly sanitized");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("input not correctly checked");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("input not sanitized");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("sanitization of input");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("unchecked input");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("input not checked");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("fail to sanitize");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("normalize input");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            

            iTagID = fAddTag("filtering");


            iTagID = fAddTag("validation framework");

            iTagID = fAddTag("throttl");

            iTagID = fAddTag("output encoding");    //TODO
            iTagID2 = fAddTag("Improper Encoding or Escaping of Output");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("crash");


            iTagID = fAddTag("HTTP TRACE"); //TRACE method verb
            iTagID2 = fAddTag("TRACE method");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("TRACE verb");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("metadata");   //TODO
            iTagID = fAddTag("registry");

            iTagID = fAddTag("LTSP");

            iTagID = fAddTag("webmail");
            iTagID2 = fAddTag("web-mail");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("privilege escalation");
            iTagID2 = fAddTag("privileges escalation");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");
            iTagID2 = fAddTag("escalation of privilege");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");
            iTagID2 = fAddTag("elevation of privilege");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");
            iTagID2 = fAddTag("exploitation of privilege");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");
            iTagID2 = fAddTag("elevate privileges");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("gain privileges");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("administrative privileges");
            iTagID = fAddTag("least privilege");

            iTagID = fAddTag("socket");

            iTagID = fAddTag("access management");
            iTagID2 = fAddTag("access control"); //TODO
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID = iTagID2;
            iTagID2 = fAddTag("controlled access");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("control access");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("retricted access");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("access restriction");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("Access Control List");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID = iTagID2;
            iTagID2 = fAddTag("ACL", true);
            fAddTagRelationship(iTagID, iTagID2, "Synonym");


            iTagID = fAddTag("documented");
            iTagID = fAddTag("undocumented");

            //Unsuccessful Logon Attempts
            //bruteforce
            iTagID = fAddTag("Unsuccessful Logon Attempt");
            iTagID2 = fAddTag("bruteforce");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID = iTagID2;
            iTagID2 = fAddTag("brute force");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");

            iTagID = fAddTag("Forceful Browsing");

            iTagID = fAddTag("Checksum");
            iTagID2 = fAddTag("Checksum Spoofing");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("Schema Poisoning");
            iTagID2 = fAddTag("XML Schema Poisoning");
            fAddTagRelationship(iTagID, iTagID2, "Related");


            iTagID = fAddTag("separation of duties");

            

            iTagID = fAddTag("data recovery");

            iTagID = fAddTag("confidentiality");
            iTagID = fAddTag("integrity");
            iTagID = fAddTag("availability");

            iTagID = fAddTag("decoy");


            iTagID = fAddTag("identity management");
                iTagID = fAddTag("identity");   //TODO

            iTagID = fAddTag("security assessment");
            iTagID = fAddTag("risk management");
            iTagID = fAddTag("business continuity");
            iTagID = fAddTag("data protection");
            iTagID = fAddTag("disaster recovery");
            iTagID = fAddTag("continuous monitoring");
            iTagID = fAddTag("forensic");
            iTagID = fAddTag("flow control");

            iTagID = fAddTag("Authentication");
            iTagID2 = fAddTag("Authentication Bypass");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("Authentication Abuse");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("bypass authentication");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("Exploitation of Authentication");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("session management");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("authorization");
            iTagID2 = fAddTag("authorize access");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("authorized access");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("unauthorize access");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("unauthorized access");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("exploitation of authorization");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("authorization bypass");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("bypass authorization");
            fAddTagRelationship(iTagID, iTagID2, "Related");


            iTagID = fAddTag("environment variable");

            iTagID = fAddTag("forced browsing");
            iTagID2 = fAddTag("force browse");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("spidering"); //TODO spider?
            fAddTagRelationship(iTagID, iTagID2, "Related");


            iTagID = fAddTag("encryption"); //TODO
            iTagID = fAddTag("cryptographic");
            iTagID = fAddTag("information protection");
            iTagID = fAddTag("attack surface");
            iTagID = fAddTag("3com");
            iTagID = fAddTag("codesys");
            iTagID = fAddTag("SCADA");
            iTagID = fAddTag("7-zip");  //TODO
            iTagID = fAddTag("acdsee");
            iTagID = fAddTag("XPM file");
            iTagID = fAddTag("illustrator");
            iTagID = fAddTag("autocad");

            iTagID = fAddTag("SOAP");
            iTagID2 = fAddTag("SOAP server");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("SOAP message");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("SOAP parameter");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("SOAP service");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("SOAP function");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("SOAP client");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("SOAP array abuse");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("forged request");
            iTagID = fAddTag("PageMaker");  //Adobe
            iTagID = fAddTag("Flash Player");
            iTagID = fAddTag("Media Player");
            iTagID = fAddTag("Shockwave");
            iTagID = fAddTag("altiris");    //Symantec
            iTagID = fAddTag("Alt-N");
            iTagID = fAddTag("WebView");    //Android
            iTagID = fAddTag("AOL");
            iTagID = fAddTag("ActiveX");
            iTagID = fAddTag("mode_rewrite");

            iTagID = fAddTag(".htaccess");
            iTagID2 = fAddTag(".htpassword");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("Arkeia");
            iTagID = fAddTag("ASUS");

            iTagID = fAddTag("Attack");
            iTagID2 = fAddTag("drive-by download");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("Autodesk");
            iTagID = fAddTag("BigAnt");

            iTagID = fAddTag("classification");
            iTagID = fAddTag("information classification");
            iTagID = fAddTag("data classification");    //TODO
            iTagID2 = fAddTag("classified information");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID = fAddTag("unclassified");

            iTagID = fAddTag("non-repudiation");
            
            
            iTagID = fAddTag("spoofing");
            iTagID2 = fAddTag("Content Spoofing");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("IP spoofing");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("ARP spoofing");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("ARP spoof");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("poisoning");
            iTagID2 = fAddTag("ARP poisoning");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("different IP addresses");

            iTagID = fAddTag("fingerprinting");
            iTagID2 = fAddTag("reconnaissance");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("discovery");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("footprinting");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("spidering");

            iTagID = fAddTag("fuzzing");
            iTagID = fAddTag("fuzzer");

            iTagID = fAddTag("e-mail");
            iTagID = fAddTag("email");

            iTagID = fAddTag("typo");
            iTagID = fAddTag("algorithm");
            iTagID = fAddTag("CGI program");
            iTagID = fAddTag("requirement");

            iTagID = fAddTag("copy and paste");
            iTagID2 = fAddTag("copy-paste");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");

            iTagID = fAddTag("TrueType");   //font

            iTagID = fAddTag("Man in the Middle");
            iTagID2 = fAddTag("MITM");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");
            iTagID2 = fAddTag("Man-in-the-Middle");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");

            iTagID = fAddTag("unicode");

            iTagID = fAddTag("logging");
            iTagID2 = fAddTag("audit trail");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            //auditing
            //logs
            iTagID2 = fAddTag("SIEM");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("procedures");
            iTagID2 = fAddTag("policies");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("tampering");

            iTagID = fAddTag("digital signature");
            iTagID2 = fAddTag("certificate");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("Supply Chain");
            iTagID2 = fAddTag("supplier");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID = fAddTag("Provider");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID = fAddTag("Integrator");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID = fAddTag("COTS");   //TODO
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID = fAddTag("third party");
            iTagID2 = fAddTag("third-party");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");
            iTagID2 = fAddTag("3rd party");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");
            iTagID2 = fAddTag("third parties");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID = iTagID2;
            iTagID = fAddTag("third-parties");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");
            iTagID = fAddTag("3rd parties");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");
            


            iTagID = fAddTag("information disclosure");
            iTagID2 = fAddTag("disclosure of information");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");

            iTagID = fAddTag("vulnerability");
            iTagID2 = fAddTag("vulnerabilities");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("start-up");

            iTagID = fAddTag("Excavation");
            iTagID = fAddTag("Interception");
            iTagID = fAddTag("Flooding");

            iTagID = fAddTag("Injection");
            iTagID2 = fAddTag("SQL Injection");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID = iTagID2;
            iTagID2 = fAddTag("SQLi");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");
            iTagID2 = fAddTag("Blind SQL Injection");
            fAddTagRelationship(iTagID, iTagID2, "Related");


            iTagID = fAddTag("Hijacking");


            iTagID = fAddTag("injection");
            iTagID2 = fAddTag("code injection");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID2 = fAddTag("eval injection");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID2 = fAddTag("XML injection");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("XPath injection");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("XQuery injection");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID2 = fAddTag("Argument injection");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID2 = fAddTag("Parameter injection");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID2 = fAddTag("Log injection");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID2 = fAddTag("Email injection");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID2 = fAddTag("Format String injection");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID2 = fAddTag("Reflection injection");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID2 = fAddTag("LDAP injection");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID2 = fAddTag("SSI injection");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID2 = fAddTag("DLL injection");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID2 = fAddTag("Object Relational Mapping Injection");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            //TODO



            iTagID = fAddTag("User State");

            iTagID = fAddTag("Cahce Poisoning");

            iTagID = fAddTag("Pharming");

            iTagID = fAddTag("Silverlight");
            iTagID = fAddTag("Kaspersky");
            

            iTagID = fAddTag("/etc/shadow");
            iTagID = fAddTag("/etc/password");
            iTagID = fAddTag("database server");
            iTagID = fAddTag("database");
            iTagID = fAddTag("port scanner");
            iTagID = fAddTag("web vulnerability scanner");
            iTagID = fAddTag("vulnerability scanner");

            iTagID = fAddTag("application software security");
            iTagID2 = fAddTag("application security");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");
            iTagID2 = fAddTag("appsec");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");

            iTagID = fAddTag("wireless device control");
            iTagID = fAddTag("boudary defense");

            iTagID = fAddTag("need to know");

            iTagID = fAddTag("data loss prevention");   //DLP

            iTagID = fAddTag("incident response");
            iTagID2 = fAddTag("security incident");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("ciphertext");
            iTagID = fAddTag("cipher");

            iTagID = fAddTag("account management");
            iTagID = fAddTag("password management");

            iTagID = fAddTag("inactive account");

            iTagID = fAddTag("period of inactivity");
            iTagID2 = fAddTag("inactivity logout");  //TODO
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("session expiration");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("usage conditions");

            iTagID = fAddTag("shared account");
            iTagID2 = fAddTag("shared credentials");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("shared password");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("wireless access");

            iTagID = fAddTag("Infrastructure");

            iTagID = fAddTag("audit information");

            iTagID = fAddTag("privileged function");

            iTagID = fAddTag("Failing Securely");
            iTagID2 = fAddTag("fail securely");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");

            iTagID = fAddTag("complete mediation");

            iTagID = fAddTag("security policy");
            iTagID = fAddTag("Law");
            iTagID = fAddTag("Regulation");

            

            int iTagSecurityControlID = fAddTag("security control");   //TODO
            fAddTagRelationship(iTagID, iTagSecurityControlID, "Related");

            iTagID2 = fAddTag("security requirement");
            fAddTagRelationship(iTagSecurityControlID, iTagID2, "Related");
            //environmental
            //physical
            iTagID = fAddTag("preventive security control");
            fAddTagRelationship(iTagSecurityControlID, iTagID, "Parent");
            iTagID2 = fAddTag("preventive control");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");
            iTagID2 = fAddTag("preventative security control");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");
            iTagID2 = fAddTag("preventative control");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");

            iTagID = fAddTag("corrective security control");
            fAddTagRelationship(iTagSecurityControlID, iTagID, "Parent");
            iTagID2 = fAddTag("corrective control");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");

            iTagID = fAddTag("directive security control");
            fAddTagRelationship(iTagSecurityControlID, iTagID, "Parent");
            iTagID2 = fAddTag("directive control");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");

            iTagID = fAddTag("detective security control");
            fAddTagRelationship(iTagSecurityControlID, iTagID, "Parent");
            iTagID2 = fAddTag("detective control");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");

            iTagID = fAddTag("Same Origin Policy");

            

            //anomalous
            //behavior

            iTagID = fAddTag("accountability"); //RACI

            iTagID = fAddTag("asset management");
            iTagID2 = fAddTag("assets management");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");
            iTagID2 = fAddTag("asset inventory");
            fAddTagRelationship(iTagID, iTagID2, "Parent");
            iTagID2 = fAddTag("assets inventory");
            fAddTagRelationship(iTagID, iTagID2, "Parent");
            iTagID2 = fAddTag("Inventory of Authorized and Unauthorized Devices");
            fAddTagRelationship(iTagID, iTagID2, "Parent");
            iTagID2 = fAddTag("Inventory of Authorized and Unauthorized Software");
            fAddTagRelationship(iTagID, iTagID2, "Parent");
            iTagID2 = fAddTag("inventory of asset");
            fAddTagRelationship(iTagID, iTagID2, "Parent");
            iTagID2 = fAddTag("ownership of asset");
            fAddTagRelationship(iTagID, iTagID2, "Parent");
            iTagID2 = fAddTag("Responsibility for assets");
            fAddTagRelationship(iTagID, iTagID2, "Parent");


            iTagID = fAddTag("defense in depth");
            iTagID2 = fAddTag("defense-in-depth");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");

            iTagID = fAddTag("sensitive data in transit");

            iTagID = fAddTag("economy of mechanism");

            iTagID = fAddTag("weakest link");

            iTagID = fAddTag("Compartmentalization");

            iTagID = fAddTag("secure by default");
            iTagID = fAddTag("default credentials");
            iTagID = fAddTag("default password");
            iTagID = fAddTag("deployment");

            iTagID = fAddTag("keep it simple"); //TODO KISS

            iTagID = fAddTag("privacy");

            iTagID = fAddTag("security awareness");
            iTagID = fAddTag("security culture");
            iTagID = fAddTag("security training");
            iTagID = fAddTag("training");
            iTagID = fAddTag("security certification");
            iTagID = fAddTag("certification");  //TODO

            iTagID = fAddTag("threat"); //TODO
            iTagID = fAddTag("insider threat");
            iTagID = fAddTag("threat agent");
            iTagID = fAddTag("threat actor");   //TODO
            iTagID = fAddTag("threat campaign");

            iTagID = fAddTag("kill chain");

            iTagID = fAddTag("compliance");
            iTagID = fAddTag("regulation");
            iTagID = fAddTag("laws");

            iTagID = fAddTag("Gather Information");
            iTagID2 = fAddTag("Information Gathering");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("fingerprinting");
            fAddTagRelationship(iTagID, iTagID2, "Related");


            iTagID = fAddTag("network segregation");
            iTagID2 = fAddTag("Segregation in networks");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("Segregation of network");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("QuickTime");
            iTagID = fAddTag("GStreamer");
            iTagID = fAddTag(".mov file");
            iTagID = fAddTag("media file");
            iTagID = fAddTag("video file");
            iTagID = fAddTag("image file");

            iTagID = fAddTag("ASCII");
            iTagID = fAddTag("UTF-8");  //TODO
            iTagID = fAddTag("UTF8");

            iTagID = fAddTag("symlink attack");

            iTagID = fAddTag("GnuTLS");
            iTagID = fAddTag("TLS");
            iTagID = fAddTag("PKCS");

            iTagID = fAddTag("RSA key");

            iTagID = fAddTag("X.509");

            iTagID = fAddTag("PowerPC");
            iTagID = fAddTag("concurrency");

            iTagID = fAddTag("500 error code");
            iTagID = fAddTag("400 error code");
            iTagID = fAddTag("404 error");

            iTagID = fAddTag("out-of-memory");
            iTagID = fAddTag("dissector");

            iTagID = fAddTag("kernel panic");
            iTagID = fAddTag("NFS client");
            iTagID = fAddTag("NFS server");
            iTagID = fAddTag("attack vector");
            iTagID = fAddTag("kill chain");
            iTagID = fAddTag("attack execution flow");
            iTagID = fAddTag("attack flow");

            iTagID = fAddTag("divide-by-zero");
            iTagID = fAddTag("Adobe Reader");
            iTagID = fAddTag("Acrobat");

            iTagID = fAddTag("access restriction");
            iTagID = fAddTag("double free");
            iTagID = fAddTag("use after free");
            iTagID2 = fAddTag("use-after-free");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");

            iTagID = fAddTag("Social Engineering");
            iTagID2 = fAddTag("phishing");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID = iTagID2;
            iTagID2 = fAddTag("phishing attack");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("spear phishing");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("Social Information");
            iTagID2 = fAddTag("Social Information Gathering");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID = fAddTag("Social Engineering");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("Identity Spoofing");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID = fAddTag("Spoofing");
            fAddTagRelationship(iTagID, iTagID2, "Related");


            iTagID = fAddTag("GTK");

            iTagID = fAddTag("malicious website");
            iTagID = fAddTag("malicious web page");
            iTagID = fAddTag("malicious");

            iTagID = fAddTag("LIST command");

            iTagID = fAddTag("shutdown");

            iTagID = fAddTag("pcap");
            iTagID = fAddTag("scap");
            iTagID = fAddTag("stix");
            iTagID = fAddTag("cwe");
            iTagID = fAddTag("oval");

            iTagID = fAddTag("Itanium");
            iTagID = fAddTag("s390");   //platform

            iTagID = fAddTag("FreeRADIUS");

            iTagID = fAddTag("ioctl");

            iTagID = fAddTag("NULL character");

            iTagID = fAddTag("SQL queries");
            iTagID = fAddTag("SQL query");

            iTagID = fAddTag("function");
            iTagID = fAddTag("command");
            iTagID = fAddTag("header");
            iTagID = fAddTag("protocol");

            iTagID = fAddTag("Probabilistic Technique");
            iTagID2 = fAddTag("Probabilistic");
            fAddTagRelationship(iTagID, iTagID2, "Parent");

            iTagID = fAddTag("configuration management");
            iTagID2 = fAddTag("secure configuration");
            fAddTagRelationship(iTagID, iTagID2, "Parent");
            iTagID2 = fAddTag("hardening");
            fAddTagRelationship(iTagID, iTagID2, "Parent");

            iTagID = fAddTag("weakness");
            iTagID = fAddTag("vulnerability management");
            iTagID2 = fAddTag("continuous vulnerability management");
            fAddTagRelationship(iTagID, iTagID2, "Parent");
            iTagID2 = fAddTag("Management of technical vulnerabilities");
            fAddTagRelationship(iTagID, iTagID2, "Parent");

            iTagID2 = fAddTag("penetration testing");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID = iTagID2;   //penetration testing
            iTagID2 = fAddTag("pentesting");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");
            iTagID2 = fAddTag("red team exercise");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("penetration test");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID = iTagID2;
            iTagID2 = fAddTag("pentest");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");

            iTagID = fAddTag("exploit");
            iTagID2 = fAddTag("0day");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("0 day");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("0-day");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("zero day");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("zero-day");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("testing phase");

            iTagID = fAddTag("maintenance");
            iTagID = fAddTag("monitoring");

            iTagID = fAddTag("information sharing");
            iTagID = fAddTag("sharing secret");
            iTagID = fAddTag("sharing account");    //TODO
            iTagID = fAddTag("sharing password");
            iTagID = fAddTag("sharing credential");

            //TODO (too short)
            iTagID = fAddTag("phishing");

            iTagID = fAddTag("attack");
            iTagID = fAddTag("defense");
            iTagID = fAddTag("defence");
            iTagID = fAddTag("cybersecurity");
            iTagID = fAddTag("framework");


            iTagID = fAddTag("Situational Awareness");

            iTagID = fAddTag("security event");
            iTagID = fAddTag("security alert");
            iTagID = fAddTag("audit trail");

            iTagID = fAddTag("session management"); //TODO
            iTagID2 = fAddTag("session fixation");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("session expiration");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("session prediction");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("session sidejacking");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("session hijacking");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("hijack session");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("session termination");
            fAddTagRelationship(iTagID, iTagID2, "Related");


            iTagID = fAddTag("quality management"); //QMS
            //TODO

            iTagID = fAddTag("information security management");    //ISMS

            
            iTagID = fAddTag("HTTP Request Splitting");
            iTagID2 = fAddTag("HTTP Response Splitting");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("HTTP Request Smuggling");
            iTagID2 = fAddTag("HTTP Response Smuggling");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("out-of-bound");
            iTagID = fAddTag("boundary error");
            iTagID = fAddTag("range error");    //TODO Related
            iTagID = fAddTag("confused deputy");

            iTagID = fAddTag("malloc");

            
            iTagID = fAddTag("sniffing");
            iTagID2 = fAddTag("sniffer");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("sniff");
            fAddTagRelationship(iTagID, iTagID2, "Related");


            iTagID = fAddTag("mumble");

            iTagID = fAddTag("apparmor");

            iTagID = fAddTag("MSN");
            iTagID = fAddTag("Skype");
            iTagID = fAddTag("Pidgin");
            iTagID = fAddTag("QEmu");
            iTagID = fAddTag("XEN");
                iTagID = fAddTag("IDS");    //TODO
                iTagID = fAddTag("Intrusion Detection System");
                iTagID = fAddTag("IPS");
                iTagID = fAddTag("Intrusion Prevention System");
                iTagID = fAddTag("Intrusion");
                iTagID = fAddTag("Breach");
                iTagID = fAddTag("indicator");
                iTagID = fAddTag("warning");
                iTagID = fAddTag("alert");
                iTagID = fAddTag("security event");
            iTagID = fAddTag("flooding");
            iTagID = fAddTag("Documentum");
            iTagID = fAddTag("EMC");
            iTagID = fAddTag("HTML5");
            iTagID = fAddTag("IPv6");
            iTagID = fAddTag("MAC address");
            iTagID = fAddTag("IP address");
            iTagID = fAddTag("phone");
            iTagID = fAddTag("XBOX");
            iTagID = fAddTag("Sony");
            iTagID = fAddTag("Playstation");
            iTagID = fAddTag("spamassassin");
            iTagID = fAddTag("CIFS");
            iTagID = fAddTag("hadoop");
            iTagID = fAddTag("VUPEN");
            iTagID = fAddTag("CSS");
            //ZDI
            iTagID = fAddTag("Outlook");
            iTagID = fAddTag("MHTML");


            iTagID = fAddTag("backdoor");

            iTagID = fAddTag("account");
            iTagID2 = fAddTag("credentials");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("username");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("login name");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("login ID");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("user ID");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            int iTagID3 = fAddTag("Password");
            fAddTagRelationship(iTagID, iTagID3, "Related");
            iTagID2 = fAddTag("default account");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID = iTagID2;
            iTagID2 = fAddTag("default credentials");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            

            iTagID2 = fAddTag("default password");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            fAddTagRelationship(iTagID3, iTagID2, "Related");
            iTagID = fAddTag("hardcoded password");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            fAddTagRelationship(iTagID3, iTagID2, "Related");
            iTagID = fAddTag("backdoor account");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID = fAddTag("hidden account");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("HotSpot");
            iTagID = fAddTag("XCF image");
            iTagID = fAddTag("ISO image");
            iTagID = fAddTag("DNS hostname");
            iTagID = fAddTag("hostname");
            iTagID = fAddTag("KVM");
            iTagID = fAddTag("debug register");
            iTagID = fAddTag("privilege level");
                iTagID = fAddTag("ATI");
                iTagID = fAddTag("AMD");
                iTagID = fAddTag("Intel");
            iTagID = fAddTag("processor");


            iTagID = fAddTag("HTTP referer");
            iTagID = fAddTag("HTTP referrer");
            iTagID = fAddTag("header");

            iTagID = fAddTag("XML");
            iTagID2 = fAddTag("XML document");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("XML file");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("libxml");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("XML Injection");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("XML Flood");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("XML Ping of the Death");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("Ping of the Death");

            iTagID = fAddTag("sensitive information");
            iTagID2 = fAddTag("sensitive data");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("array index error");

            iTagID = fAddTag("kernel buffer");
            iTagID2 = fAddTag("kernel memory");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID = fAddTag("Linux kernel");

            iTagID = fAddTag("NULL pointer dereference");
            iTagID = fAddTag("dereference");

            iTagID = fAddTag("protection mechanism");
            iTagID = fAddTag("protection");
            iTagID = fAddTag("mechanism");

            iTagID = fAddTag("Barracuda");


            iTagID = fAddTag("spam");

            iTagID = fAddTag("archive");
            iTagID = fAddTag("LHA archive");
            iTagID = fAddTag("LHA");
            iTagID = fAddTag("TAR archive");
            iTagID = fAddTag("TAR.GZ archive");
            
            iTagID = fAddTag("ZIP archive");
            iTagID2 = fAddTag("ZIP file");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("RAR archive");
            iTagID2 = fAddTag("RAR file");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("7zip");
            iTagID = fAddTag("WinZIP");
            iTagID = fAddTag("WinRAR");
            iTagID = fAddTag("MMSE");
            iTagID = fAddTag("Samsung");
            iTagID = fAddTag("Buffalo");
            iTagID = fAddTag("D-Link");
            iTagID = fAddTag("DLink");
            //TODO complete from CPE
            iTagID = fAddTag("ZoneAlarm");
            iTagID = fAddTag("Zope");
            iTagID = fAddTag("ZyXEL");
            iTagID = fAddTag("kloxo");
            iTagID = fAddTag("ddwrt");
            iTagID = fAddTag("astium");
            iTagID = fAddTag("alienvault");
            iTagID = fAddTag("netgear");
            iTagID = fAddTag("nginx");
            iTagID = fAddTag("pandora");
            iTagID = fAddTag("sophos");
            iTagID = fAddTag("zabbix");
                iTagID = fAddTag("pinapp");

            iTagID = fAddTag("jboss");
            iTagID = fAddTag("ubisoft");
            iTagID = fAddTag("arkeia");
                iTagID = fAddTag("sap");
            iTagID = fAddTag("tomcat");
            iTagID = fAddTag("viscom");
            iTagID = fAddTag("webex");
            iTagID = fAddTag("rails");
            iTagID = fAddTag("bitly");
            iTagID = fAddTag("Amazon");
            iTagID = fAddTag("France");
            iTagID = fAddTag("Telecom");
            iTagID = fAddTag("rootkit");
            iTagID = fAddTag("dropbox");
            iTagID = fAddTag("LD_PRELOAD");
            iTagID = fAddTag("ransomware");
            iTagID = fAddTag("exploit kit");
            iTagID = fAddTag("OpenXML");

            iTagID = fAddTag("VNC");    //TODO
            iTagID = fAddTag("winvnc");
            iTagID = fAddTag("UltraVNC");
            iTagID = fAddTag("TightVNC");


            iTagID = fAddTag("unicenter");
            iTagID = fAddTag("TFTP");   //Server    Client
            iTagID = fAddTag("telnet");
            iTagID = fAddTag("netbios");
            iTagID = fAddTag("LSASS");
            iTagID = fAddTag("LDAP");
            iTagID = fAddTag("LDAP query");
            iTagID = fAddTag("LDAP queries");
            iTagID = fAddTag("winlogon");
            iTagID = fAddTag("rpc-dcom");
            iTagID = fAddTag("webcam");
            iTagID = fAddTag("keylog"); //TODO
            iTagID = fAddTag("keystroke logger");

            iTagID = fAddTag("LANDesk");

            iTagID = fAddTag("desktop");
            iTagID = fAddTag("laptop");

            iTagID = fAddTag("ptrace");

            iTagID = fAddTag("mailbox");
            iTagID = fAddTag("mailto");

            iTagID = fAddTag("iNotes");

            iTagID = fAddTag("LPViewer");

            iTagID = fAddTag("crack");

            iTagID = fAddTag("MailEnable");

            iTagID = fAddTag("MaxDB");

            iTagID = fAddTag("MDAC");
            iTagID = fAddTag("HMAC");

            iTagID = fAddTag("MDaemon");

            iTagID = fAddTag("Dameware");

            iTagID = fAddTag("Measuresoft");

            iTagID = fAddTag("MERCUR");

            iTagID = fAddTag("DirectX");
            iTagID = fAddTag("DirectShow");
            iTagID = fAddTag("OpenGL");

            iTagID = fAddTag("Forefront");

            iTagID = fAddTag("HLP");

            iTagID = fAddTag("shatter attack");

            iTagID = fAddTag("NetWare");

            iTagID = fAddTag("OLE");

            iTagID = fAddTag("WMI");

            iTagID = fAddTag("Microsoft Works");

            iTagID = fAddTag("XMLHTTP");

            iTagID = fAddTag("Msgpack");

            iTagID = fAddTag("MPlayer");
            iTagID = fAddTag("Microsoft Access");
            iTagID = fAddTag(".NET Framework");
            iTagID = fAddTag("Microsoft Jet");
            iTagID = fAddTag("MDB");
            iTagID = fAddTag("PDB");
            iTagID = fAddTag("Movie Maker");    //Microsoft
            iTagID = fAddTag("FlashPix");
            iTagID = fAddTag("Groove");
            iTagID = fAddTag("OCX");
            iTagID = fAddTag("insecure library loading");
            iTagID = fAddTag("RTF");
            iTagID = fAddTag("Spreadsheet");
            iTagID = fAddTag("SharePoint");
            iTagID = fAddTag("PowerPoint");


            iTagID = fAddTag("Microsoft Office Visio");
            iTagID2 = fAddTag("Microsoft Visio");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");
            iTagID2 = fAddTag("Visio");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("Microsoft Office Visio Viewer");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID = iTagID2;
            iTagID2 = fAddTag("Microsoft Visio Viewer");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");


            iTagID = fAddTag("VSD");
            iTagID = fAddTag("Microsoft Communicator");
            iTagID = fAddTag("Groove");
            iTagID = fAddTag("Expression Design");
            iTagID = fAddTag("WordPad");
            iTagID = fAddTag("Notepad");
            iTagID = fAddTag("Notepad++");
            iTagID = fAddTag("yaSSL");
            iTagID = fAddTag("NetDDE");
            iTagID = fAddTag("NetMail");
            iTagID = fAddTag("web admin");
            iTagID = fAddTag("admin interface");
            iTagID = fAddTag("administrative interface");
            iTagID = fAddTag("NetSupport");
            iTagID = fAddTag("Handshake");
            iTagID = fAddTag("NetVault");
            iTagID = fAddTag("Norton");
            iTagID = fAddTag("file upload");
            iTagID = fAddTag("upload");
            iTagID = fAddTag("iManager");   //Novell
            iTagID = fAddTag("iPrint");
            iTagID = fAddTag("NetIQ");
                iTagID = fAddTag("OLE");
            iTagID = fAddTag("ADO");

            iTagID = fAddTag("Openwsman");
            iTagID = fAddTag("AutoVue");    //Oracle
            iTagID = fAddTag("Endeca");
            iTagID = fAddTag("Hyperion");

            iTagID = fAddTag("applet");

            iTagID = fAddTag("Rhino");

            iTagID = fAddTag("OLAP");

            iTagID = fAddTag("CDR file");

            iTagID = fAddTag("login.php");

            iTagID = fAddTag("Command Injection");
            iTagID2 = fAddTag("OS commanding");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("Spatial");    //Oracle

            iTagID = fAddTag("virtual");
            iTagID = fAddTag("VMware");
            iTagID2 = fAddTag("VMware ESX");
            fAddTagRelationship(iTagID, iTagID2, "Related");
                iTagID2 = fAddTag("ESX",true);
                fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("VirtuaBox");
            iTagID = fAddTag("VirtualBox");

            iTagID = fAddTag("Xen");
            iTagID = fAddTag("hypervisor");

            iTagID = fAddTag("Microsoft Virtual Server");
            iTagID2 = fAddTag("Microsoft Virtual PC");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("adaptec");

            iTagID = fAddTag("WareHouse");
            iTagID = fAddTag("Business");
            iTagID = fAddTag("WebCenter");  //Oracle
            iTagID = fAddTag("XDB");

            iTagID = fAddTag("Orbit");

            iTagID = fAddTag("SMB");
            iTagID = fAddTag("Plone");
            iTagID = fAddTag("ProFTP");
            iTagID = fAddTag("Promotic");
            iTagID = fAddTag("InTrust");
            iTagID = fAddTag("Marshal");
            iTagID = fAddTag("Content-Type");
            iTagID = fAddTag("TeXML");
            iTagID = fAddTag("ReGet");
            iTagID = fAddTag("Ricoh");
            iTagID = fAddTag("Ruby on Rails");
            iTagID = fAddTag("sadmind");
            iTagID = fAddTag("WebKit");  //Safari
            iTagID = fAddTag("SafeNet");
            iTagID = fAddTag("Sami");
            iTagID = fAddTag("SAP NetWeaver");
            iTagID = fAddTag("SAPgui");
            iTagID = fAddTag("Schneider");
            iTagID = fAddTag("Electric");
            iTagID = fAddTag("Trend Micro");
            iTagID = fAddTag("SHOUTcast");
            iTagID = fAddTag("sipXtapi");
            iTagID = fAddTag("DCE/RPC");
            iTagID = fAddTag("dcerpc");
            iTagID = fAddTag("cachefsd");
            iTagID = fAddTag("snmpXdmid");
            iTagID = fAddTag("telnetd");
            iTagID = fAddTag("SolarWinds");
            iTagID = fAddTag("SonicWall");
            iTagID = fAddTag("Splunk");
            iTagID = fAddTag("SQL Injection");
            iTagID = fAddTag("BIG-IP"); //F5
            iTagID = fAddTag("F5"); //Labs
            iTagID = fAddTag("StarTeam");   //Borland
            iTagID = fAddTag("VisualStudio");
            iTagID = fAddTag("Visual Studio");
            iTagID = fAddTag("sockd");
            iTagID = fAddTag("Sunway");
            iTagID = fAddTag("Sybase");
            iTagID = fAddTag("pcAnywhere");
            iTagID = fAddTag("Web Gateway");
            iTagID = fAddTag("System V");
            iTagID = fAddTag("TikiWiki");
            iTagID = fAddTag("Timbuktu");   //Motorola
            iTagID = fAddTag("Tivoli"); //IBM
            iTagID = fAddTag("Sawyer");
            iTagID = fAddTag("Touch22");
            iTagID = fAddTag("Traq");
            iTagID = fAddTag("OfficeScan");
            iTagID = fAddTag("TurboSoft");
            iTagID = fAddTag("TweakFS");
            iTagID = fAddTag("TWiki");
            iTagID = fAddTag("ikiwiki");
            iTagID = fAddTag("mediawiki");

            iTagID = fAddTag("UltraVNC");
            iTagID = fAddTag("VanDyke");
            iTagID = fAddTag("ViRobot");
            iTagID = fAddTag("Viscom");
            iTagID = fAddTag("Trillian");

            iTagID = fAddTag("URL spoofing");   //TODO
            iTagID = fAddTag("URL bar spoofing");

            iTagID = fAddTag("homographic");

            iTagID = fAddTag("End of Life");    //EOL
            iTagID = fAddTag("End-Of-Life");    //TODO

            iTagID = fAddTag("VBP");
            iTagID = fAddTag("Visual Basic");
            iTagID = fAddTag("Pascal");
            iTagID = fAddTag("FoxPro");
            iTagID = fAddTag("VideoLAN");
            iTagID = fAddTag("VLC");    //TODO
            iTagID = fAddTag("vTiger");
            iTagID = fAddTag("Webmin");
            iTagID = fAddTag("WellinTech");
            iTagID = fAddTag("WhatsUp");
            iTagID = fAddTag("Alpha");
            iTagID = fAddTag("Wibukey");
            iTagID = fAddTag("playlist");
            iTagID = fAddTag("cursor");
            iTagID = fAddTag("MSCOMCTL");
            iTagID = fAddTag("fax");
            iTagID = fAddTag("IE6");    //TODO Internet Explorer
            iTagID = fAddTag("IEv6");
            iTagID = fAddTag("IE7");
            iTagID = fAddTag("IE8");
            iTagID = fAddTag("IE9");
            iTagID = fAddTag("IE10");
            iTagID = fAddTag("IE11");
            iTagID = fAddTag("Vista");
            iTagID = fAddTag("Seven");
            iTagID = fAddTag("Media Encoder");
            iTagID = fAddTag("MIDI");
            iTagID = fAddTag("Unicast");
            iTagID = fAddTag("Metafile");
            iTagID = fAddTag("Plug and Play");
            iTagID = fAddTag("spooler");
            iTagID = fAddTag("RASMAN");

            iTagID = fAddTag("mahara");

            iTagID = fAddTag("RPC DCOM");   //TODO
            iTagID = fAddTag("RPC-DCOM");
            iTagID = fAddTag("RPC/DCOM");
            iTagID = fAddTag("COM+");

                iTagID = fAddTag("sudo");

            iTagID = fAddTag("X11");

            iTagID = fAddTag("xulrunner");

            iTagID = fAddTag("Network News Transport Protocol");
            iTagID2 = fAddTag("NNTP");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");

            iTagID = fAddTag("RRAS");
            iTagID = fAddTag("gina");
            iTagID = fAddTag("search-ms");
            iTagID = fAddTag("service");
            iTagID = fAddTag("LNK");
            iTagID = fAddTag("SMB2");
            iTagID = fAddTag("Telephony");
            iTagID = fAddTag("Task Scheduler");
            iTagID = fAddTag("Theme File");
            iTagID = fAddTag("IPC");
            iTagID = fAddTag("IPCConnect");
            iTagID = fAddTag("WINS");
            iTagID = fAddTag("FileView");
            iTagID = fAddTag("DECT");
            iTagID = fAddTag("PCAP");
            iTagID = fAddTag("LWRES");
            iTagID = fAddTag("Eudora");
            iTagID = fAddTag("WorldMail");
            iTagID = fAddTag("WPAD");
            iTagID = fAddTag("Xi Software");
            iTagID = fAddTag("Messenger");
            iTagID = fAddTag("Yahoo Messenger");
            iTagID = fAddTag("Facebook Messenger");
            iTagID = fAddTag("ypupdated");
            iTagID = fAddTag("ZENworks");   //Novell
            iTagID = fAddTag("Lenovo");
            iTagID = fAddTag("AzureWave");
            iTagID = fAddTag("Liteon");
            iTagID = fAddTag("Hon Hai");
            iTagID = fAddTag("EMF");
            iTagID = fAddTag("Puppet");

            iTagID = fAddTag("OOXML");

            iTagID = fAddTag("keyboard");
            iTagID = fAddTag("vtiger");
            iTagID = fAddTag("struts");
            iTagID = fAddTag("jenkins");
            iTagID = fAddTag("json");
            iTagID = fAddTag("glassfish");
            iTagID = fAddTag("telnet");
            iTagID = fAddTag("browser");
            iTagID = fAddTag("zenoss");
            iTagID = fAddTag("webcalendar");
            iTagID = fAddTag("accellion");
            iTagID = fAddTag("nagios");
            iTagID = fAddTag("postgres");
            iTagID = fAddTag("pop3");
            iTagID = fAddTag("upnp");

            iTagID = fAddTag("spoof");
            iTagID = fAddTag("KDE");
            iTagID = fAddTag("SYN packet");
            iTagID = fAddTag("ACK packet");
            iTagID = fAddTag("challenge-response");
            iTagID = fAddTag("challenge response");
            iTagID = fAddTag("Java for Business");

            iTagID = fAddTag("vim");

            iTagID = fAddTag("shell command");
            iTagID = fAddTag("shellescape");

            iTagID = fAddTag("escape sequence");

            iTagID = fAddTag("victim");

            iTagID = fAddTag("buffer");
            iTagID = fAddTag("stack");
            iTagID = fAddTag("heap");
            //heap-based out-of-bounds write or read

            iTagID = fAddTag("buffer overflow");
            iTagID2 = fAddTag("overflow buffer");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");
            iTagID2 = fAddTag("BO vulnerabilities");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("BO vulnerability");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID2 = fAddTag("buffer overrun");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID2 = fAddTag("stack overflow");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID = iTagID2;
            iTagID2 = fAddTag("stack-based overflow");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");
            iTagID2 = fAddTag("stack-based buffer overflow");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");
            iTagID2 = fAddTag("stack based overflow");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");
            iTagID2 = fAddTag("stack based buffer overflow");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");

            iTagID2 = fAddTag("heap overflow");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID = iTagID2;
            iTagID2 = fAddTag("heap-based overflow");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");
            iTagID2 = fAddTag("heap-based buffer overflow");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");
            iTagID2 = fAddTag("heap based overflow");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");
            iTagID2 = fAddTag("heap based buffer overflow");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");

            iTagID2 = fAddTag("memory buffer");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("buffer underflow");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("integer error");
            iTagID2 = fAddTag("integer overflow");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID = iTagID2;
            iTagID2 = fAddTag("integer underflow");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            

            iTagID = fAddTag("string error");
            iTagID = fAddTag("format string");
            iTagID = fAddTag("numeric error");
            

            iTagID = fAddTag("special element");


            iTagID = fAddTag("null byte");
            iTagID2 = fAddTag("null byte injection");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("RealPlayer"); //RealNetworks

            iTagID = fAddTag("CentOS");

            iTagID = fAddTag("neutralization"); //TODO CWE


                iTagID = fAddTag("sanitiz");    //TODO
                iTagID = fAddTag("sanitize");
                iTagID = fAddTag("canonicaliz");    //ze    zed zation
                iTagID = fAddTag("double decod");

            iTagID = fAddTag("source code");

            iTagID = fAddTag("comments");   //code

            iTagID = fAddTag("specification");
            iTagID = fAddTag("architecture");
            iTagID = fAddTag("design");

            iTagID = fAddTag("whitelist");
            iTagID = fAddTag("blacklist");

            iTagID = fAddTag("comparison");
            iTagID = fAddTag("conversion");
            iTagID = fAddTag("discrepancy");

            iTagID = fAddTag("Foscam");

            iTagID = fAddTag("directory traversal");
            iTagID2 = fAddTag("path traversal");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("command execution");
            iTagID = fAddTag("remote code execution");
            iTagID2 = fAddTag("RCE Vulnerability");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("local file inclusion");
            iTagID2 = fAddTag("remote file inclusion");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("sensitive data");

            iTagID = fAddTag("dangerous function");

            iTagID = fAddTag("regular expression");
            iTagID2 = fAddTag("regex");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");

            iTagID = fAddTag("misuse");

            iTagID = fAddTag("hard-coded");

            iTagID = fAddTag("resource management");

            iTagID = fAddTag("memory management");
            iTagID = fAddTag("memory exhaustion");
            iTagID = fAddTag("resource exhaustion");

            iTagID = fAddTag("Configuration Management");
            iTagID2 = fAddTag("configuration file");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID = iTagID2;
            iTagID2 = fAddTag("config file");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");
            iTagID2 = fAddTag("conf file");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");


            iTagID = fAddTag("sandbox");

            iTagID = fAddTag("permission"); //TODO
            iTagID = fAddTag("bypass permissions");

            iTagID = fAddTag("tcpdump");

            iTagID = fAddTag("metasploit");

            iTagID = fAddTag("wireshark");
            iTagID = fAddTag("Ethereal");   //TODO

            iTagID = fAddTag("Chrome");

            iTagID = fAddTag("BOOTP");
            iTagID = fAddTag("DICOM");
            iTagID = fAddTag("ftptls");
            iTagID = fAddTag("imap");

            iTagID = fAddTag("X.Org");
            iTagID = fAddTag("Xserver");
                iTagID = fAddTag("X server");   //XFree86

            iTagID = fAddTag("execve");
            iTagID = fAddTag("Quagga");
            iTagID = fAddTag("Gopher");
            iTagID = fAddTag("libpurple");
            iTagID = fAddTag("gzip");

            iTagID = fAddTag("chipset");

            iTagID = fAddTag("Racoon");

            iTagID = fAddTag("certificate");    //TODO
            iTagID = fAddTag("Certification Authority");

            iTagID = fAddTag("cleartext");

            iTagID = fAddTag("chroot");

            iTagID = fAddTag("jailbreak");

            iTagID = fAddTag("GnuPG");
            iTagID = fAddTag("PGP");

            iTagID = fAddTag("KINK");

            iTagID = fAddTag("entropy");
            iTagID2 = fAddTag("random number"); //generator
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("Itanium");
            iTagID = fAddTag("Montecito");

            iTagID = fAddTag("signedness"); //error

            iTagID = fAddTag("Tectia");

            iTagID = fAddTag("PRNG");
            iTagID = fAddTag("TOCTOU");

                iTagID = fAddTag("predictab");  //ble   bility


            iTagID = fAddTag("untrusted");

            iTagID = fAddTag("Tampering");
            iTagID2 = fAddTag("Parameter Tampering");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("Data Tampering");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("Forging");

            iTagID = fAddTag("JavaScript");
            iTagID2 = fAddTag("javascript:");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID2 = fAddTag("cross-site scripting");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID = iTagID2;
            iTagID2 = fAddTag("cross-site scripting");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");
            iTagID2 = fAddTag("XSS",true);
            fAddTagRelationship(iTagID, iTagID2, "Synonym");
            iTagID2 = fAddTag("Cross Site Scripting");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");

            iTagID = fAddTag("cross-site tracing");
            iTagID2 = fAddTag("Cross Site Tracing");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");

            iTagID = fAddTag("Cascading Style Sheets"); //CSS
            

            iTagID = fAddTag("cross-site request forgery");
            iTagID2 = fAddTag("cross site request forgery");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");
            iTagID2 = fAddTag("CSRF");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");
            iTagID2 = fAddTag("XSRF");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");

            iTagID = fAddTag("cross-domain");
            iTagID2 = fAddTag("cross-domain.xml");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("cross-domain policy");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("Kerberos");
            iTagID = fAddTag("RADIUS");

            iTagID = fAddTag("DHCP");
            iTagID2 = fAddTag("DHCP server");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("DHCP client");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("DHCP request");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("DHCP traffic");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID2 = fAddTag("dhclient");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("glibc");

            iTagID = fAddTag("DHTML");

            iTagID = fAddTag("peer-to-peer");   //P2P

            iTagID = fAddTag("device");
            iTagID = fAddTag("storage device");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID = fAddTag("embedded device");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID = fAddTag("portable device");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID = fAddTag("firewall");
            iTagID = fAddTag("router");
            iTagID = fAddTag("switch");

            iTagID = fAddTag("cisco");
            iTagID = fAddTag("juniper");
            iTagID = fAddTag("microsoft");
            iTagID = fAddTag("windows");
            iTagID = fAddTag("linux");  //GNU
            iTagID = fAddTag("unix");
            iTagID = fAddTag("bsd");
            iTagID = fAddTag("openbsd");
            iTagID = fAddTag("freebsd");
            iTagID = fAddTag("debian");
            iTagID = fAddTag("ubuntu");
            iTagID = fAddTag("solaris");
            iTagID = fAddTag("Trustix");
            iTagID = fAddTag("Linux");
            iTagID = fAddTag("UNIX");

            iTagID = fAddTag("popup");

            iTagID = fAddTag("redhat");
            iTagID2 = fAddTag("Red Hat");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("Red Hat Entreprise Linux");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("RHEL",true);
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("PostScript");
            iTagID = fAddTag("TIFF");
            iTagID = fAddTag("OpenOffice");
            iTagID = fAddTag("StarOffice");

            iTagID = fAddTag("proxy");

            iTagID = fAddTag("HTTP header");
            iTagID = fAddTag("HTTP response");
            iTagID = fAddTag("HTTP 1.1");

            iTagID = fAddTag("kernel memory");
            iTagID = fAddTag("memory corruption");
            iTagID = fAddTag("corruption");
            iTagID = fAddTag("Ruby");
            iTagID = fAddTag("Perl");

            iTagID = fAddTag("Python");
            iTagID2 = fAddTag("Django");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("Zend");
            iTagID = fAddTag("Visual Basic");
            iTagID = fAddTag("C#");
            iTagID = fAddTag("Csharp");
                iTagID = fAddTag(".NET");
                iTagID = fAddTag("ASP");
                iTagID = fAddTag("ASP.NET");

            iTagID = fAddTag("library");
            iTagID = fAddTag("libraries");
            iTagID = fAddTag("external librar");

            iTagID = fAddTag("WebGL");
            iTagID = fAddTag("OpenGL");

            iTagID = fAddTag("libXfont");
            iTagID = fAddTag("Xfont");

            iTagID = fAddTag("insecure temporary file creation");
            iTagID2 = fAddTag("temporary file");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("call");

            iTagID = fAddTag("bitmap");

            iTagID = fAddTag("xpdf");
            iTagID = fAddTag("kpdf");
            iTagID = fAddTag("poppler");
            iTagID = fAddTag("gpdf");

            iTagID = fAddTag("ImageMagic");

            iTagID = fAddTag("metacharacter");

            iTagID = fAddTag("plugin");

            iTagID = fAddTag("entropy");
            iTagID = fAddTag("seed");

            iTagID = fAddTag("x86");
            iTagID = fAddTag("x64");
            iTagID = fAddTag("AMD64");

            iTagID = fAddTag("apple");
            iTagID2 = fAddTag("iTunes");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("iPhoto");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("OSX");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("MAC OSX");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("Mac OS X");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("iphone"); //ios
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("Ekiga");

            iTagID = fAddTag("UDDI");

            iTagID = fAddTag("Tektronix");

            iTagID = fAddTag("android");

            iTagID = fAddTag("apache");
            iTagID2 = fAddTag("tomcat");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("modsecurity");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("Logitech");

            iTagID = fAddTag("websphere");
            iTagID = fAddTag("weblogic");
            iTagID = fAddTag("cold fusion");
            iTagID = fAddTag("ibm");
            iTagID = fAddTag("hp-ux");
                iTagID = fAddTag("dell");
            iTagID = fAddTag("SunOS");
            iTagID = fAddTag("Snort");
            iTagID = fAddTag("BroIDS");
            iTagID = fAddTag("Splunk");
            iTagID = fAddTag("Ethereal");
            iTagID = fAddTag("phpMyAdmin");
            iTagID = fAddTag("powershell");
                iTagID = fAddTag("shell");
            iTagID = fAddTag("Avaya");
            iTagID = fAddTag("Siemens");
            iTagID = fAddTag("Toshiba");
            iTagID = fAddTag("Thomson");
            iTagID = fAddTag("Terminal Server");
            iTagID = fAddTag("Nokia");
            iTagID = fAddTag("Macromedia");

            iTagID = fAddTag("BlueCoat");
            iTagID2 = fAddTag("Blue Coat");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");

            iTagID = fAddTag("BroadWin");
            iTagID = fAddTag("toolbar");

            iTagID = fAddTag("eTrust");
            iTagID = fAddTag("Total Defense");
            iTagID = fAddTag("XoSoft"); //CA
            iTagID = fAddTag("AnyConnect");
            iTagID = fAddTag("SecurID");
            iTagID = fAddTag("token");
            iTagID = fAddTag("camera");
            iTagID = fAddTag("Citadel");
            iTagID = fAddTag("Avast");
            iTagID = fAddTag("ClamAV");
            iTagID = fAddTag("CMailServer");
            iTagID = fAddTag("Computech");
            iTagID = fAddTag("packard");
            iTagID = fAddTag("Cool PDF Reader");
            iTagID = fAddTag("PDF Reader");
            iTagID = fAddTag("Acrobat Reader");
            iTagID2 = fAddTag("Macromedia");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("package");

            iTagID = fAddTag("Mailman");
            iTagID = fAddTag("Cyrus");
            iTagID = fAddTag("libcurl");

            iTagID = fAddTag("Corel");  //TODO
            iTagID = fAddTag("Corel PDF");
            iTagID = fAddTag("m3u");
            iTagID = fAddTag("CoolPlayer");
            iTagID = fAddTag("jmx");
            iTagID = fAddTag("bsp");
            iTagID = fAddTag("cgi");
            iTagID = fAddTag("Cyrus");
            iTagID = fAddTag("RealFlex");
            iTagID = fAddTag("RealWin");
            iTagID = fAddTag("Dell");
            iTagID = fAddTag("Disk Pulse");
            iTagID = fAddTag("DNS zone transfer");
            iTagID = fAddTag("DynDNS");
            iTagID = fAddTag("MX record");
            iTagID = fAddTag("registrar");
            iTagID = fAddTag("Easy Chat");
            iTagID = fAddTag("EasyMail");
            iTagID = fAddTag("AlphaStor");  //EMC

            iTagID = fAddTag("Group Policy");
            iTagID = fAddTag("GPO");
            iTagID = fAddTag("SYSVOL");

            iTagID = fAddTag("MSCOMCTL");
            iTagID = fAddTag("ASLR");
            iTagID = fAddTag("DEP");
            iTagID = fAddTag("SEH");

            iTagID = fAddTag("streaming");

            iTagID = fAddTag("Dotclear");
            iTagID = fAddTag("GetSimple");
            iTagID = fAddTag("CMS");
            iTagID = fAddTag("ORM");

            iTagID = fAddTag("token");
            iTagID = fAddTag("token reuse");
            iTagID = fAddTag("token kidnap");

            iTagID = fAddTag("datacenter");
            iTagID = fAddTag("cloud");

            iTagID = fAddTag("McAfee");
            iTagID2 = fAddTag("ePolicy orchestrator");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID = iTagID2;
            iTagID2 = fAddTag("ePolicy");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("Eureka");
            iTagID = fAddTag("ffdshow");
            iTagID = fAddTag("Bull");

            iTagID = fAddTag("China");
            iTagID = fAddTag("Chinese");
            iTagID = fAddTag("Russia");
            iTagID = fAddTag("Russian");
            iTagID = fAddTag("Korea");
            iTagID = fAddTag("Korean");
            iTagID = fAddTag("Syria");
            iTagID = fAddTag("Algeria");
            iTagID = fAddTag("Dubai");
            iTagID = fAddTag("Saudi");
            iTagID = fAddTag("Arabia");
            iTagID = fAddTag("French");
            iTagID = fAddTag("Arabic");
            
            //TODO...

            iTagID = fAddTag("ActionScript");

            iTagID = fAddTag("MIME",true);
            iTagID = fAddTag("ASCII");

            iTagID = fAddTag("UDP",true);
            iTagID = fAddTag("UDP datagram");
            iTagID = fAddTag("UDP traffic");
            iTagID = fAddTag("UDP packet");

            iTagID = fAddTag("autoplay");

            iTagID = fAddTag("MP3");
            iTagID = fAddTag("MP4");
            iTagID = fAddTag("MIDI");

            iTagID = fAddTag("network diagram");
            iTagID = fAddTag("diagram");
            iTagID = fAddTag("internal network");
            iTagID = fAddTag("firewall configuration");
            iTagID = fAddTag("DMZ");
            iTagID = fAddTag("configuration standard");
            iTagID = fAddTag("cardholder");

            iTagID = fAddTag("stylesheet");

            iTagID = fAddTag("OpenType");
                iTagID = fAddTag("font");

            iTagID = fAddTag("SWF");
            iTagID = fAddTag("FlashGet");
            iTagID = fAddTag("FlashFXP");
            iTagID = fAddTag("Fotoslate");
            iTagID = fAddTag("Download Manager");
            iTagID = fAddTag("FreeFloat");

            iTagID = fAddTag("SS7");
            iTagID = fAddTag("FreePBX");
            iTagID = fAddTag("PBX");
            iTagID = fAddTag("GnuRADIO");
            iTagID = fAddTag("radio");
            iTagID = fAddTag("FreeSSH");
            iTagID = fAddTag("FrontPage");
            iTagID = fAddTag("GIMP");
            iTagID = fAddTag("GoodTech");
            iTagID = fAddTag("GroupWise");  //Novell
            iTagID = fAddTag("hastymail");
            iTagID = fAddTag("Helix");
            iTagID = fAddTag("backtrack");
            iTagID = fAddTag("kali");
            iTagID = fAddTag("RealNetworks");
            iTagID = fAddTag("Honeywell");
            iTagID = fAddTag("honeypot");
            iTagID = fAddTag("exif");
            iTagID = fAddTag("LHA");
            iTagID = fAddTag("waterholing");
            iTagID = fAddTag("HP Data Protector");
            iTagID = fAddTag("Data Protector"); //TODO
            iTagID = fAddTag("Easy Printer");   //HP
            iTagID = fAddTag("SiteScope");  //HP
            iTagID = fAddTag("appliance");
            iTagID = fAddTag("LoadRunner"); //HP Mercury

            iTagID = fAddTag("CMDB");

            iTagID = fAddTag(".bak");
            iTagID = fAddTag(".sql");

            iTagID = fAddTag("Hummingbird");
            iTagID = fAddTag("Cognos"); //IBM
            iTagID = fAddTag("Rational");   //IBM

            iTagID = fAddTag("ntdll");
            iTagID = fAddTag("kernel32");

            iTagID = fAddTag("double decode");
            iTagID2 = fAddTag("double decoding");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID = fAddTag("double encode");  //TODO twice
            iTagID2 = fAddTag("double encoding");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("WebDAV");
            iTagID = fAddTag("ISAPI");

            iTagID = fAddTag("Indusoft");
            iTagID = fAddTag("Informix");

            iTagID = fAddTag("DB2");    //IBM
            iTagID = fAddTag("iSeries");
            iTagID = fAddTag("AS400");

            iTagID = fAddTag("Hadoop");
            iTagID = fAddTag("Cassandra");

            iTagID = fAddTag("Amazon");
            iTagID = fAddTag("eBay");

            iTagID = fAddTag("SCCP");

            iTagID = fAddTag("evince");

            iTagID = fAddTag("OMAP4");

            iTagID = fAddTag("OpenSafety");

            iTagID = fAddTag("MacroVision");
            iTagID = fAddTag("InstallShield");

            iTagID = fAddTag("Borland");
            iTagID = fAddTag("InterSystems");

            

            iTagID = fAddTag("Bean");   //java MBean
            iTagID = fAddTag("Sun Java");   //Oracle
            iTagID = fAddTag("Sun");

            iTagID = fAddTag("awt");
            iTagID = fAddTag("Kodak");
            iTagID = fAddTag("Epson");
            iTagID = fAddTag("Cannon");


            iTagID = fAddTag("opcode");
            iTagID = fAddTag("seh");
            iTagID = fAddTag("gadget");
            iTagID = fAddTag("ropchain");
            //nop
            iTagID = fAddTag("nopsled");

            iTagID = fAddTag("FTP server");
            iTagID2 = fAddTag("FTP client");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID2 = fAddTag("OpenSSL");

            iTagID = fAddTag("SSH server");
            iTagID2 = fAddTag("OpenSSH");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("SSH client");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID = iTagID2;
            iTagID2 = fAddTag("Putty");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("Exchange");   //TODO

            iTagID = fAddTag("Mail server");
            
            iTagID2 = fAddTag("Microsoft Exchange");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID2 = fAddTag("Mail client");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID = iTagID2;
            iTagID2 = fAddTag("Outlook");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("Thunderbird");   //Mozilla
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("Microsoft Exchange");
            iTagID2 = fAddTag("MS Exchange");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");

            iTagID = fAddTag("Microsoft InfoPath");

            iTagID = fAddTag("Microsoft Publisher");

                iTagID = fAddTag("server");
                iTagID = fAddTag("CGI");

            iTagID = fAddTag("jasper");

            iTagID = fAddTag("heimdal");

            iTagID = fAddTag("Samba");
            iTagID2 = fAddTag("smbd");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("Maverick");

            iTagID = fAddTag("iceweasel");

            iTagID = fAddTag("Enscript");

            iTagID = fAddTag("WordPress");
            iTagID = fAddTag("Drupal");
            iTagID = fAddTag("kernel"); //TODO
            iTagID = fAddTag("Linux kernel");
            iTagID = fAddTag("Marvell");    //DOVE
            iTagID = fAddTag("FreeBSD kernel");

            iTagID = fAddTag("Oracle");

            iTagID = fAddTag("SAP Netweaver");
            iTagID2 = fAddTag("Netweaver");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("Citrix");
            iTagID = fAddTag("Netscaler");

            iTagID = fAddTag("Browser");
            iTagID2 = fAddTag("Internet Explorer");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("Firefox");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("Safari");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("Chrome");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("browser engine");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("Netscape");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("K-Meleon");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("chromium");   //chromium-browser

            iTagID = fAddTag("Firebird");   //Interbase

            iTagID = fAddTag("Firebug");

            iTagID = fAddTag("Firefox");
            iTagID2 = fAddTag("Firefox ESR");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("Seamonkey");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("Gecko");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("XMLHttpRequest");

            iTagID = fAddTag("OpenGroup");
            iTagID = fAddTag("Pegasus");

            iTagID = fAddTag(".properties file");

            iTagID = fAddTag("assert error");

            iTagID = fAddTag("SSE2");

            iTagID = fAddTag("r8169"); //driver
            
            iTagID = fAddTag("IIS");    //TODO
            iTagID = fAddTag("Internet Information Server");
            iTagID = fAddTag("PCT");
            iTagID = fAddTag("postfix");
            iTagID = fAddTag("Norton 360");
            iTagID = fAddTag("SAML");   //TODO
            iTagID = fAddTag("opensaml");
            
            iTagID = fAddTag("SQL server");
            iTagID2 = fAddTag("Oracle");
            fAddTagRelationship(iTagID, iTagID2, "Related");
           
            iTagID2 = fAddTag("Microsoft SQL Server");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID = iTagID2;
            iTagID2 = fAddTag("MSSQL");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");
            iTagID2 = fAddTag("MS SQL");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");



            //TODO...
            iTagID = fAddTag("Struts");
            iTagID = fAddTag("JAVA");
            iTagID2 = fAddTag("J2EE");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("ASP.NET");
            iTagID = fAddTag("PHP");
            iTagID = fAddTag("Ruby");
            iTagID = fAddTag("Perl");
            iTagID = fAddTag("C++");
            iTagID = fAddTag("ActionForm");
            iTagID2 = fAddTag("Action Form");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("hook");
            iTagID = fAddTag("hijack");

            iTagID = fAddTag("alternate data stream");
            iTagID2 = fAddTag("NTFS ADS");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("OWASP");
            iTagID = fAddTag("MITRE");  //NIST
            iTagID = fAddTag("NIST");
            iTagID = fAddTag("IETF");

            iTagID = fAddTag("memory leak");
            iTagID = fAddTag("leakage");

            iTagID = fAddTag("posix");
            iTagID = fAddTag("PCRE");
            iTagID = fAddTag("packetstorm");
            iTagID = fAddTag("exploit-db");
            iTagID = fAddTag("milw0rm");
            iTagID = fAddTag("osvdb");
            iTagID = fAddTag("us-cert");
            iTagID = fAddTag("securitytracker");

            iTagID = fAddTag("critical resource");

            iTagID = fAddTag("cairo");
            iTagID = fAddTag("debuginfo");

            iTagID = fAddTag("MathML");
            iTagID = fAddTag("polygon");

            iTagID = fAddTag("logging");
            iTagID = fAddTag("algorithm");
            iTagID = fAddTag("IOCTL");
            iTagID = fAddTag("captcha");
            iTagID = fAddTag("direct object reference");

            iTagID = fAddTag("mcafee");
            iTagID = fAddTag("symantec");
            iTagID = fAddTag("avast");

            iTagID = fAddTag("WSDL");
            iTagID = fAddTag("PDF");
            iTagID = fAddTag("Adobe Acrobat Reader");
            iTagID2 = fAddTag("Acrobat");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("Adobe Acrobat");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("Acrobat Reader");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");
            iTagID2 = fAddTag("acroread");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("Adobe AIR");
            iTagID = fAddTag("Dreamweaver");

            iTagID = fAddTag("IRIX");
            iTagID = fAddTag("Macromedia");
            iTagID = fAddTag("Office");
            iTagID = fAddTag("Excel");
            iTagID = fAddTag("Mozilla");
            iTagID = fAddTag("Google");
            iTagID = fAddTag("imapd");
            iTagID = fAddTag("IMAP");
            iTagID = fAddTag("wu-ftp");
            iTagID = fAddTag("glibc");
            iTagID = fAddTag("LPRng");
            iTagID = fAddTag("Sendmail");
            iTagID = fAddTag("Squid");
            iTagID = fAddTag("Firebird");
            iTagID = fAddTag("Kerio");
            iTagID = fAddTag("phpBB");
            iTagID = fAddTag("vBulletin");
            iTagID = fAddTag("Internet Explorer");
            iTagID = fAddTag("GDI+");
            iTagID = fAddTag(".emf");
            iTagID = fAddTag("serv-u");
            iTagID = fAddTag("Kaspersky");
            iTagID = fAddTag("subversion");
            iTagID = fAddTag("xchat");
            iTagID = fAddTag("libpng");
            iTagID = fAddTag("ollydbg");
            iTagID = fAddTag("unreal tournament");
            iTagID = fAddTag("FIFA");
            iTagID = fAddTag("Lexmark");
            iTagID = fAddTag("Mercury");
            iTagID = fAddTag("portal");
            iTagID = fAddTag("forum");
            iTagID = fAddTag("citadel");
            iTagID = fAddTag("MailEnable");
            iTagID = fAddTag("compliance");
            iTagID = fAddTag("AWstat");
            iTagID = fAddTag("CERT");
            iTagID = fAddTag("Tenable");
            iTagID = fAddTag("Nessus");
            iTagID = fAddTag("HIPAA");
            iTagID = fAddTag("PCI DSS");
            iTagID = fAddTag("flash");
            iTagID = fAddTag("e107");
            iTagID = fAddTag("PHP-Nuke");
            iTagID2 = fAddTag("PHP nuke");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");

            iTagID = fAddTag("Berlios");
            iTagID = fAddTag("Yahoo");
            iTagID = fAddTag("Bing");
            iTagID = fAddTag("netcat");
            iTagID = fAddTag("wiki");

            iTagID = fAddTag("antivirus");
            iTagID2 = fAddTag("anti-virus");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");
            iTagID2 = fAddTag("anti virus");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");

            iTagID = fAddTag("Winamp");
            iTagID = fAddTag("Brother");
            iTagID = fAddTag("shopping");
                iTagID = fAddTag("shop");
                iTagID = fAddTag("cart");
                iTagID = fAddTag("blog");
            iTagID = fAddTag("E-cart");
            iTagID = fAddTag("twitter");
            iTagID = fAddTag("ICQ");
                iTagID = fAddTag("SOX");
            iTagID = fAddTag("IPSwitch");
            iTagID = fAddTag("vBulletin");
            iTagID = fAddTag("Golden");
            iTagID = fAddTag("PostNuke");

            iTagID = fAddTag("SMTP");
            iTagID2 = fAddTag("SMTP Server");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("SMTP Protocol");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("SMTP Client");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("SMTP Traffic");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("SMTP Packet");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("PHP-Fusion");

            iTagID = fAddTag("torrent");
            iTagID2 = fAddTag("Bittorrent");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("nero");
            iTagID = fAddTag("burning");

            iTagID = fAddTag("moodle");

            iTagID = fAddTag("sandbox");

            iTagID = fAddTag("Omnibox");

            iTagID = fAddTag("Cuckoo");
            
            iTagID = fAddTag("Lotus Domino Notes");
            iTagID2 = fAddTag("Lotus Notes");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("Lotus Domino");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("Domino Notes");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("Lotus");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            

            iTagID = fAddTag("BrightStor");
            iTagID = fAddTag("Novell");

            
            
            iTagID = fAddTag("Computer Associates");
            iTagID2 = fAddTag("Computer Associate");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("eDirectory");
            iTagID = fAddTag("OpenView");
            iTagID = fAddTag("Linksys");
            iTagID = fAddTag("FileZilla");
            iTagID = fAddTag("NewsBoard");

            iTagID = fAddTag("eclipse");

            iTagID = fAddTag("RSS");
            iTagID2 = fAddTag("feed");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("wget");

            iTagID = fAddTag("Foxit");
            iTagID = fAddTag("Motorola");
            iTagID = fAddTag("Verizon");
            iTagID = fAddTag("Orange");
            iTagID = fAddTag("Sparc");
            iTagID = fAddTag("B2B");
            iTagID = fAddTag("P2P");
            iTagID = fAddTag("CC");
            iTagID = fAddTag("SimpleBBS");
            
            iTagID = fAddTag("My Bulletin Board");
            iTagID2 = fAddTag("MyBB");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");
            iTagID2 = fAddTag("MyBulletinBoard");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");

            iTagID = fAddTag("Cisco PIX");
            iTagID = fAddTag("PIX");
            iTagID = fAddTag("Cisco ASA");
            iTagID = fAddTag("TYPSoft");
            iTagID = fAddTag("gateway");
            iTagID = fAddTag("iGateway");
            iTagID = fAddTag("Crystal");
            iTagID = fAddTag("Cristal");
            iTagID = fAddTag("Active Directory");
            iTagID = fAddTag("Veritas");
            iTagID = fAddTag("backup");
            iTagID = fAddTag("Flatnuke");
            iTagID = fAddTag("ARCserv");
            iTagID = fAddTag("jquery");
            iTagID = fAddTag("XOOPS");
            iTagID = fAddTag("SMTP server");
            iTagID = fAddTag("SquirrelMail");
            iTagID = fAddTag("Avahi");
            iTagID = fAddTag("H.323");  //protocol
            iTagID = fAddTag("LDT");
            iTagID = fAddTag("PhotoPost");
            iTagID = fAddTag("facebook");
            iTagID = fAddTag("linkedin");
            iTagID = fAddTag("gmail");
            iTagID = fAddTag("hotmail");
            iTagID = fAddTag("Joomla");
            iTagID = fAddTag("Fortinet");
            iTagID = fAddTag("Fortigate");
            iTagID = fAddTag("Fortiweb");
            iTagID = fAddTag("Foxmail");
            iTagID = fAddTag("Asterisk");
            iTagID = fAddTag("LD_LIBRARY_PATH");
            iTagID = fAddTag("Yokogawa");
            iTagID = fAddTag("RIPv1");
            iTagID = fAddTag("Quagga");
            iTagID = fAddTag("missing sanity check");
            iTagID = fAddTag("Montgomery");
            iTagID = fAddTag("shadow network");
            iTagID = fAddTag("Net-SNMP");
            iTagID = fAddTag("PunBB");
            

            //Exchange server
            iTagID = fAddTag("Manufacture");
            iTagID2 = fAddTag("Manufacturer");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("Factory");

            iTagID = fAddTag("Physical");
            iTagID2 = fAddTag("Physical Access");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("Physical Security");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("Physical Theft");
            fAddTagRelationship(iTagID, iTagID2, "Related");

            iTagID = fAddTag("counterfeit");

            iTagID = fAddTag("Distribution");
            iTagID = fAddTag("Deployment");

            iTagID = fAddTag("Malicious Logic");

            iTagID = fAddTag("Lateral Movement");
            iTagID2 = fAddTag("Lateral Escalation");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("Horizontal Escalation");
            fAddTagRelationship(iTagID, iTagID2, "Related");


            iTagID = fAddTag("contractor");
            iTagID = fAddTag("subcontractor");

            iTagID = fAddTag("fragmented");



            
            iTagID = fAddTag("XML-RPC");
            iTagID2 = fAddTag("XMLRPC");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");

            iTagID = fAddTag("IFRAME");
            iTagID = fAddTag("iFrame overlay"); //TODO Clickjacking
            iTagID = fAddTag("frame injection");

            iTagID = fAddTag("MQ Series");  //TODO
            iTagID = fAddTag("MQSeries");
            iTagID = fAddTag("Webmethod");
            iTagID = fAddTag("Webdynpro");
            iTagID = fAddTag("many laughs");    //TODO
            iTagID = fAddTag("billion laughs");
            iTagID = fAddTag("JSON");   //TODO
            iTagID = fAddTag("JSON Hijacking");
            iTagID = fAddTag("WSDL");
            iTagID = fAddTag("encryption key");
            iTagID = fAddTag("phpBanner");
            iTagID = fAddTag("Photoshop");
            iTagID = fAddTag("Power Board");
            iTagID = fAddTag("Serendipity");
            iTagID = fAddTag("game");
            iTagID = fAddTag("gaim");
            iTagID = fAddTag("claroline");
            iTagID = fAddTag("CesarFTP");
            iTagID = fAddTag("Merak");
            iTagID = fAddTag("IceWrap");
            iTagID = fAddTag("predictable");
            iTagID = fAddTag("rainbow table");
            iTagID = fAddTag("Maxtor");
            iTagID = fAddTag("Seagate");
            iTagID = fAddTag("EFTP");
            iTagID = fAddTag("sendmail");
            iTagID = fAddTag("fuzz");
            iTagID = fAddTag("fuzzer");
            iTagID = fAddTag("fuzzing");
            iTagID = fAddTag("LEAP");
            iTagID = fAddTag("LEAP server");
            iTagID = fAddTag("VoIP");
            iTagID = fAddTag("IPBX");
            iTagID = fAddTag("PowerDNS");
            iTagID = fAddTag("Hibernate");  //Struts
            iTagID = fAddTag("Cacti");
            iTagID = fAddTag("MVC");

            
            iTagID = fAddTag("Server-Side Include");
            iTagID2 = fAddTag("Server Side Include");    //SSI
            fAddTagRelationship(iTagID, iTagID2, "Synonym");

            iTagID = fAddTag("JDBC");   //Java

            
            iTagID = fAddTag("HP-UX");
            iTagID2 = fAddTag("HPUX");
            fAddTagRelationship(iTagID, iTagID2, "Synonym");

            iTagID = fAddTag("XDoS");   //XML
            iTagID = fAddTag("Global Variable");    //PHP

            iTagID = fAddTag(".dll");
            iTagID = fAddTag(".doc");
            iTagID = fAddTag(".docx");
            iTagID = fAddTag(".zip");
            iTagID = fAddTag(".rar");

            iTagID = fAddTag("XML");
            iTagID2 = fAddTag("XML validation");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID = iTagID2;
            iTagID2 = fAddTag("validate the XML");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID2 = fAddTag("XSD validation");
            fAddTagRelationship(iTagID, iTagID2, "Related");
            iTagID = fAddTag("XSD");
            fAddTagRelationship(iTagID2, iTagID, "Related");


            iTagID = fAddTag("regression error");
            iTagID = fAddTag("regression");
            iTagID = fAddTag("non-regression");

            iTagID = fAddTag("overwrite arbitrary files");

            iTagID = fAddTag("XFree86");

            //***************************************************************************
            //***************************************************************************
            //Create References
            REFERENCE oReference = new REFERENCE();
            try
            {
                string sReferenceURL = "http://owasp.org/index.php/HttpOnly";
                int iReferenceID = 0;
                try
                {
                    iReferenceID = model.REFERENCE.FirstOrDefault(o => o.ReferenceURL == sReferenceURL).ReferenceID;
                }
                catch(Exception ex)
                {

                }
                if (iReferenceID <= 0)
                {
                    oReference.CreatedDate = DateTimeOffset.Now;
                    oReference.ReferenceURL = sReferenceURL;
                    oReference.ReferenceTitle = "HttpOnly";
                    oReference.Source = "OWASP";

                    oReference.VocabularyID=iVocabularyXORCISMID;    //XORCISM
                    oReference.timestamp = DateTimeOffset.Now;
                    model.REFERENCE.Add(oReference);
                    model.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                //Already exists
            }
            //TODO


            //***************************************************************************
            //***************************************************************************
            IEnumerable<TAG> TAGS = model.TAG;
            foreach(TAG oTag in TAGS.ToList())
            {
                Console.WriteLine("TagValue: "+oTag.TagValue);
                string sTagValueLower=oTag.TagValue.ToLower();

                #region securityrequirementtags
                IEnumerable<SECURITYREQUIREMENT> SECURITYREQUIREMENTS = model.SECURITYREQUIREMENT;
                foreach(SECURITYREQUIREMENT oSecReq in SECURITYREQUIREMENTS.ToList())
                {
                    if (oSecReq.SecurityRequirementDescription.ToLower().Contains(sTagValueLower))
                    {
                        int iSecurityRequirementTagID = 0;
                        try
                        {
                            iSecurityRequirementTagID = model.SECURITYREQUIREMENTTAG.FirstOrDefault(o => o.SecurityRequirementID == oSecReq.SecurityRequirementID && o.TagID == oTag.TagID).SecurityRequirementTagID;
                        }
                        catch(Exception ex)
                        {

                        }
                        if(iSecurityRequirementTagID<=0)
                        {
                            SECURITYREQUIREMENTTAG oSecReqtag = new SECURITYREQUIREMENTTAG();
                            oSecReqtag.CreatedDate = DateTimeOffset.Now;
                            oSecReqtag.SecurityRequirementID = oSecReq.SecurityRequirementID;
                            oSecReqtag.TagID = oTag.TagID;


                            oSecReqtag.timestamp = DateTimeOffset.Now;
                            oSecReqtag.VocabularyID = iVocabularyXORCISMID;    //XORCISM
                            model.SECURITYREQUIREMENTTAG.Add(oSecReqtag);
                            model.SaveChanges();
                        }
                    }
                }
                #endregion securityrequirementtags

                #region securitycontroltags
                IEnumerable<SECURITYCONTROL> SECURITYCONTROLS = model.SECURITYCONTROL;
                foreach (SECURITYCONTROL oSecControl in SECURITYCONTROLS.ToList())
                {
                    if (oSecControl.SecurityControlName.ToLower().Contains(sTagValueLower))
                    {
                        int iSecurityControlTagID = 0;
                        try
                        {
                            iSecurityControlTagID = model.SECURITYCONTROLTAG.FirstOrDefault(o => o.SecurityControlID == oSecControl.SecurityControlID && o.TagID == oTag.TagID).SecurityControlTagID;
                        }
                        catch (Exception ex)
                        {

                        }
                        if (iSecurityControlTagID <= 0)
                        {
                            SECURITYCONTROLTAG oSecControlTag = new SECURITYCONTROLTAG();
                            oSecControlTag.CreatedDate = DateTimeOffset.Now;
                            oSecControlTag.SecurityControlID = oSecControl.SecurityControlID;
                            oSecControlTag.TagID = oTag.TagID;


                            oSecControlTag.timestamp = DateTimeOffset.Now;
                            oSecControlTag.VocabularyID = iVocabularyXORCISMID;    //XORCISM
                            model.SECURITYCONTROLTAG.Add(oSecControlTag);
                            model.SaveChanges();
                        }
                    }
                }
                #endregion securitycontroltags

                #region cwetags
                IEnumerable<CWE> CWES = model.CWE;
                foreach (CWE oCWE in CWES.ToList())
                {

                    if (oCWE.CWEName.ToLower().Contains(sTagValueLower) || oCWE.CWEDescriptionSummary.ToLower().Contains(sTagValueLower))   //TODO
                    {
                        int iCWETagID = 0;
                        try
                        {
                            iCWETagID = model.CWETAG.FirstOrDefault(o => o.CWEID == oCWE.CWEID && o.TagID == oTag.TagID).CWETagID;
                        }
                        catch (Exception ex)
                        {

                        }
                        if (iCWETagID <= 0)
                        {
                            CWETAG oCWETag = new CWETAG();
                            oCWETag.CreatedDate = DateTimeOffset.Now;
                            oCWETag.CWEID = oCWE.CWEID;
                            oCWETag.TagID = oTag.TagID;


                            oCWETag.timestamp = DateTimeOffset.Now;
                            oCWETag.VocabularyID=iVocabularyXORCISMID;    //XORCISM
                            model.CWETAG.Add(oCWETag);
                            model.SaveChanges();
                        }
                    }
                }
                #endregion cwetags

                //CAPEC
                #region attackpatterntags
                IEnumerable<ATTACKPATTERN> ATTACKPATTERNS = attack_model.ATTACKPATTERN;
                foreach (ATTACKPATTERN oAttackPattern in ATTACKPATTERNS.ToList())
                {

                    if (oAttackPattern.AttackPatternName.ToLower().Contains(sTagValueLower) || oAttackPattern.AttackPatternDescription.ToLower().Contains(sTagValueLower))   //TODO
                    {
                        int iAttackPatternTagID = 0;
                        try
                        {
                            iAttackPatternTagID = attack_model.ATTACKPATTERNTAG.FirstOrDefault(o => o.AttackPatternID == oAttackPattern.AttackPatternID && o.TagID == oTag.TagID).AttackPatternTagID;
                        }
                        catch (Exception ex)
                        {

                        }
                        if (iAttackPatternTagID <= 0)
                        {
                            try
                            {
                                ATTACKPATTERNTAG oAttackPatternTag = new ATTACKPATTERNTAG();
                                oAttackPatternTag.CreatedDate = DateTimeOffset.Now;
                                oAttackPatternTag.AttackPatternID = oAttackPattern.AttackPatternID;
                                oAttackPatternTag.TagID = oTag.TagID;


                                oAttackPatternTag.timestamp = DateTimeOffset.Now;
                                oAttackPatternTag.VocabularyID = iVocabularyXORCISMID;    //XORCISM
                                attack_model.ATTACKPATTERNTAG.Add(oAttackPatternTag);
                                attack_model.SaveChanges();
                            }
                            catch(Exception exAttackPattern)
                            {
                                Console.WriteLine("Exception exAttackPattern " + exAttackPattern.Message + " " + exAttackPattern.InnerException);
                            }
                        }
                    }
                }
                #endregion attackpatterntags


                #region ovaldefinitiontags
                IEnumerable<OVALDEFINITION> OVALDEFINITIONS = oval_model.OVALDEFINITION;
                foreach (OVALDEFINITION oOVALDefinition in OVALDEFINITIONS.ToList())
                {

                    if (oOVALDefinition.OVALDefinitionTitle.ToLower().Contains(sTagValueLower) || oOVALDefinition.OVALDefinitionDescription.ToLower().Contains(sTagValueLower))   //TODO
                    {
                        int iOVALDefinitionTagID = 0;
                        try
                        {
                            iOVALDefinitionTagID = oval_model.OVALDEFINITIONTAG.FirstOrDefault(o => o.OVALDefinitionID == oOVALDefinition.OVALDefinitionID && o.TagID == oTag.TagID).OVALDefinitionTagID;
                        }
                        catch (Exception ex)
                        {

                        }
                        if (iOVALDefinitionTagID <= 0)
                        {
                            OVALDEFINITIONTAG oOVALDefinitionTag = new OVALDEFINITIONTAG();
                            oOVALDefinitionTag.CreatedDate = DateTimeOffset.Now;
                            oOVALDefinitionTag.OVALDefinitionID = oOVALDefinition.OVALDefinitionID;
                            oOVALDefinitionTag.TagID = oTag.TagID;


                            oOVALDefinitionTag.timestamp = DateTimeOffset.Now;
                            oOVALDefinitionTag.VocabularyID = iVocabularyXORCISMID;    //XORCISM
                            oval_model.OVALDEFINITIONTAG.Add(oOVALDefinitionTag);
                            oval_model.SaveChanges();
                        }
                    }
                }
                #endregion ovaldefinitiontags

                #region ovaltesttags
                IEnumerable<OVALTEST> OVALTESTS = oval_model.OVALTEST;
                foreach (OVALTEST oOVALTest in OVALTESTS.ToList())
                {

                    if (oOVALTest.comment.ToLower().Contains(sTagValueLower))   //TODO
                    {
                        int iOVALTestTagID = 0;
                        try
                        {
                            iOVALTestTagID = oval_model.OVALTESTTAG.FirstOrDefault(o => o.OVALTestID == oOVALTest.OVALTestID && o.TagID == oTag.TagID).OVALTestTagID;
                        }
                        catch (Exception ex)
                        {

                        }
                        if (iOVALTestTagID <= 0)
                        {
                            OVALTESTTAG oOVALTestTag = new OVALTESTTAG();
                            oOVALTestTag.CreatedDate = DateTimeOffset.Now;
                            oOVALTestTag.OVALTestID = oOVALTest.OVALTestID;
                            oOVALTestTag.TagID = oTag.TagID;


                            oOVALTestTag.timestamp = DateTimeOffset.Now;
                            oOVALTestTag.VocabularyID = iVocabularyXORCISMID;    //XORCISM
                            oval_model.OVALTESTTAG.Add(oOVALTestTag);
                            oval_model.SaveChanges();
                            //iOVALTestTagID=
                        }
                    }
                }
                #endregion ovaltesttags

                #region ovalvariabletags
                IEnumerable<OVALVARIABLE> OVALVARIABLES = oval_model.OVALVARIABLE;
                foreach (OVALVARIABLE oOVALVariable in OVALVARIABLES.ToList())
                {

                    if (oOVALVariable.comment.ToLower().Contains(sTagValueLower))   //TODO
                    {
                        int iOVALTestTagID = 0;
                        try
                        {
                            iOVALTestTagID = oval_model.OVALVARIABLETAG.FirstOrDefault(o => o.OVALVariableID == oOVALVariable.OVALVariableID && o.TagID == oTag.TagID).OVALVariableTagID;
                        }
                        catch (Exception ex)
                        {

                        }
                        if (iOVALTestTagID <= 0)
                        {
                            OVALVARIABLETAG oOVALVariableTag = new OVALVARIABLETAG();
                            oOVALVariableTag.CreatedDate = DateTimeOffset.Now;
                            oOVALVariableTag.OVALVariableID = oOVALVariable.OVALVariableID;
                            oOVALVariableTag.TagID = oTag.TagID;

                            oOVALVariableTag.timestamp = DateTimeOffset.Now;
                            oOVALVariableTag.VocabularyID = iVocabularyXORCISMID;    //XORCISM
                            oval_model.OVALVARIABLETAG.Add(oOVALVariableTag);
                            oval_model.SaveChanges();
                        }
                    }
                }
                #endregion ovaltesttags


                #region vulnerabilitytags
                IEnumerable<VULNERABILITY> VULNERABILITYS = vuln_model.VULNERABILITY;
                foreach (VULNERABILITY oVulnerability in VULNERABILITYS.ToList())
                {

                    if (oVulnerability.VULName.ToLower().Contains(sTagValueLower) || oVulnerability.VULDescription.ToLower().Contains(sTagValueLower))   //TODO
                    {
                        int iVulnerabilityTagID = 0;
                        try
                        {
                            iVulnerabilityTagID = vuln_model.VULNERABILITYTAG.FirstOrDefault(o => o.VulnerabilityID == oVulnerability.VulnerabilityID && o.TagID == oTag.TagID).VulnerabilityTagID;
                        }
                        catch (Exception ex)
                        {

                        }
                        if (iVulnerabilityTagID <= 0)
                        {
                            VULNERABILITYTAG oVulnerabilityTag = new VULNERABILITYTAG();
                            oVulnerabilityTag.CreatedDate = DateTimeOffset.Now;
                            oVulnerabilityTag.VulnerabilityID = oVulnerability.VulnerabilityID;
                            oVulnerabilityTag.TagID = oTag.TagID;


                            oVulnerabilityTag.timestamp = DateTimeOffset.Now;
                            oVulnerabilityTag.VocabularyID = iVocabularyXORCISMID;
                            vuln_model.VULNERABILITYTAG.Add(oVulnerabilityTag);
                            vuln_model.SaveChanges();
                        }
                    }
                }
                #endregion vulnerabilitytags

                
                #region exploittags
                IEnumerable<EXPLOIT> EXPLOITS = model.EXPLOIT;
                foreach (EXPLOIT oExploit in EXPLOITS.ToList())
                {

                    if (oExploit.ExploitName.ToLower().Contains(sTagValueLower) || oExploit.ExploitDescription.ToLower().Contains(sTagValueLower))   //TODO
                    {
                        int iExploitTagID = 0;
                        try
                        {
                            iExploitTagID = model.EXPLOITTAG.FirstOrDefault(o => o.ExploitID == oExploit.ExploitID && o.TagID == oTag.TagID).ExploitTagID;
                        }
                        catch (Exception ex)
                        {

                        }
                        if (iExploitTagID <= 0)
                        {
                            EXPLOITTAG oExploitTag = new EXPLOITTAG();
                            oExploitTag.CreatedDate = DateTimeOffset.Now;
                            oExploitTag.ExploitID = oExploit.ExploitID;
                            oExploitTag.TagID = oTag.TagID;


                            oExploitTag.timestamp = DateTimeOffset.Now;
                            oExploitTag.VocabularyID = iVocabularyXORCISMID;
                            model.EXPLOITTAG.Add(oExploitTag);
                            model.SaveChanges();
                        }
                    }
                }
                #endregion exploittags
                
                //TODO



            }
        }

        public static int fAddTag(string sTagValue, bool casesensitive=false)
        {
            int iTagID = 0;
            //XORCISMEntities model = new XORCISMEntities();
            try
            {
                iTagID = model.TAG.FirstOrDefault(o => o.TagValue == sTagValue).TagID;
            }
            catch (Exception ex)
            {

            }
            if (iTagID <= 0)
            {
                try
                {
                    TAG oTAG = new TAG();
                    oTAG.TagValue = sTagValue;
                    oTAG.casesensitive = casesensitive;
                    oTAG.VocabularyID=iVocabularyXORCISMID;  //XORCISM
                    oTAG.TagType = "string";    //regex
                    oTAG.CreatedDate = DateTimeOffset.Now;
                    oTAG.timestamp = DateTimeOffset.Now;
                    model.TAG.Add(oTAG);
                    model.SaveChanges();
                    iTagID = oTAG.TagID;
                }
                catch (Exception exAddTag)
                {
                    Console.WriteLine("Exception exAddTag " + exAddTag.Message + " " + exAddTag.InnerException);
                }
            }
            //model.Dispose();
            return iTagID;
        }

        public static void fAddTagRelationship(int iTagParentID, int iTagSubjectID, string sRelationship="", int iConfidenceLevel=3)
        {
            int iTagTagID = 0;
            //XORCISMEntities model = new XORCISMEntities();
            try
            {
                iTagTagID = model.TAGTAG.FirstOrDefault(o => o.TagParentID==iTagParentID && o.TagSubjectID==iTagSubjectID).TagTagID;
            }
            catch (Exception ex)
            {

            }
            if (iTagTagID <= 0)
            {
                try
                {
                    TAGTAG oTAGRelationship = new TAGTAG();
                    oTAGRelationship.TagParentID = iTagParentID;
                    oTAGRelationship.TagSubjectID = iTagSubjectID;
                    oTAGRelationship.TagRelationship = sRelationship;    //TODO
                    oTAGRelationship.ConfidenceLevelID = iConfidenceLevel;  //Review
                    oTAGRelationship.VocabularyID=iVocabularyXORCISMID;  //XORCISM
                    oTAGRelationship.CreatedDate = DateTimeOffset.Now;
                    oTAGRelationship.timestamp = DateTimeOffset.Now;
                    model.TAGTAG.Add(oTAGRelationship);
                    model.SaveChanges();
                    iTagTagID = oTAGRelationship.TagTagID;
                }
                catch (Exception exAddTagRelationship)
                {
                    Console.WriteLine("Exception exAddTagRelationship " + exAddTagRelationship.Message + " " + exAddTagRelationship.InnerException);
                }
            }
            //model.Dispose();
            //return iTagTagID;
        }

    }
}
