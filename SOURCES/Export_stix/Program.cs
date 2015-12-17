using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XORCISMModel;
using XUtils;

namespace Export_stix
{
    class Program
    {
        //Test project for writing out a STIX XML file
        static void Main(string[] args)
        {
            //*****************************************
            //Add a NetworkConnection to the database
            //HARDCODED
            //TODO
            string sSourceIP="192.168.1.15";
            //string sDestinationIP="192.168.1.10";
            string sLayer4_Protocol="TCP";
            int iSourcePort=5525;


            //XORCISMModel.NETWORKCONNECTION NetConnect = new NETWORKCONNECTION();
            XUtils.UtilsFunctions MyFunctions = new UtilsFunctions();
            int iSocketAddressID=MyFunctions.fAddSocketAddress(sLayer4_Protocol, sSourceIP, iSourcePort);

            Console.WriteLine("DEBUG iSocketAddressID: " + iSocketAddressID);
        }
    }
}
