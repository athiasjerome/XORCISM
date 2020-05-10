using System;
using System.IO;
using System.Xml;

namespace OSCALParser
{
    // XML Parser for OSCAL NIST Special Publication 800-53: Security and Privacy Controls for Federal Information Systems and Organizations, Revision 5 Final Public Draft
    // Jerome Athias, 2020
    // (parsing with switch/cases for detecting updates in schema)
    // Ref. https://github.com/usnistgov/OSCAL/tree/master/content/nist.gov/SP800-53/rev5/xml
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("DEBUG "+DateTimeOffset.Now);
            XmlDocument docXML= new XmlDocument();
            docXML.Load("NIST_SP-800-53_rev5-FPD_catalog.xml");   //HARDCODED
            XmlNamespaceManager m = new XmlNamespaceManager(docXML.NameTable);
            m.AddNamespace("ns", "http://csrc.nist.gov/ns/oscal/1.0"); //Hardcoded

            Console.WriteLine("DEBUG OSCAL file loaded");

            XmlNodeList nodesFamily;
            nodesFamily = docXML.SelectNodes("ns:catalog/ns:group", m);

            foreach (XmlNode nodeFamily in nodesFamily){
                //Console.WriteLine("DEBUG ==================================================================================================================================");
                Console.WriteLine("==================================================================================================================================");
                //Console.WriteLine("DEBUG "+DateTimeOffset.Now);
                string sFamilyID = nodeFamily.Attributes["id"].InnerText;  //ac
                //Console.WriteLine("DEBUG FamilyID=" + sFamilyID.ToUpper());
                
                foreach (XmlNode nodeFamilyInfo in nodeFamily){
                    int iControlLevel=1;
                    switch (nodeFamilyInfo.Name)
                    {
                        case "title":
                            string sFamilyName = nodeFamilyInfo.InnerText;  //Access Control
                            //Console.WriteLine("DEBUG FamilyName="+sFamilyName);
                            Console.WriteLine(sFamilyName+" ("+sFamilyID.ToUpper()+")");
                            break;
                        case "control":
                            fParseControl(nodeFamilyInfo, iControlLevel);
                            
                            break;
                        default:
                            Console.WriteLine("ERROR Missing code for nodeFamilyInfo "+nodeFamilyInfo.Name);
                            break;
                    }
                }
            }
        }

        static private void fParseControl(XmlNode nodeFamilyInfo, int iControlLevel){
            try{
                string sControlClass = nodeFamilyInfo.Attributes["class"].InnerText;    //SP800-53
                string sControlID = nodeFamilyInfo.Attributes["id"].InnerText;    //ac-1
                string sTemp="";
                for(int i=0;i<iControlLevel;i++) sTemp+="  ";
                //Console.WriteLine("DEBUG Control="+sControlClass+" "+sControlID.ToUpper());
                //Console.WriteLine(sTemp+sControlClass+" "+sControlID.ToUpper());
                iControlLevel++;
                foreach (XmlNode nodeControlInfo in nodeFamilyInfo){
                    switch(nodeControlInfo.Name){
                        case "title":
                            string sControlName = nodeControlInfo.InnerText;
                            //Console.WriteLine("DEBUG ControlTitle="+sControlName);
                            //Console.WriteLine(sTemp+sControlName);
                            Console.WriteLine(sTemp+sControlClass+" "+sControlID.ToUpper()+" "+sControlName);
                            break;
                        case "param":
                        //    Console.WriteLine("TODO param");
                            break;
                        case "prop":
                        //    Console.WriteLine("TODO prop name");
                            //<prop name="label">AC-1</prop>
                            //<prop name="sort-id">AC-01</prop>
                            break;
                        case "link":
                        //    Console.WriteLine("TODO link"); //<link rel="reference" href="#OMB_A-130">[OMB A-130]</link>
                            break;
                        case "part":
                        //    Console.WriteLine("TODO part");
                            //<part name="statement" id="ac-1_smt">
                            //<part name="guidance" id="ac-1_gdn">

                            break;
                        case "control":
                            
                            fParseControl(nodeControlInfo, iControlLevel);
                            break;
                        default:
                            Console.WriteLine("ERROR Missing code for nodeControlInfo "+nodeControlInfo.Name);
                            break;
                    }
                }
            }
            catch(Exception exControl){
                Console.WriteLine("Exception exControl "+exControl.Message);
            }
        }
    }
}
