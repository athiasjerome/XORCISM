using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XORCISMModel;

namespace XUtils
{
    public class UtilsFunctions
    {
        /// <summary>
        /// Copyright (C) 2014-2015 Jerome Athias
        /// Utils Functions for the XORCISM system
        /// This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation; either version 2 of the License, or (at your option) any later version.
        /// 
        /// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
        /// 
        /// You should have received a copy of the GNU General Public License along with this program; if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
        /// </summary>
        static XORCISMEntities XModel = new XORCISMEntities();

        static void Main(string[] args)
        {

        }

        public int fAddSocketAddress(string sLayer4_Protocol, string sIPAddress, int iPortValue)
        {
            //TODO: Deprecate or use is_source, is_destination (legacy from NDDM)

            //XORCISMModel.NETWORKCONNECTION NetConnect = new NETWORKCONNECTION();
            
            #region Layer4
            //Make sure we have the Layer 4 "Transport Layer" in the Database
            int iProtoLayer4ID=XModel.OSILAYER.Where(o=>o.OSILayerName == "Transport Layer").Select(o=>o.OSILayerID).FirstOrDefault();  //Hardcoded
            if(iProtoLayer4ID<=0)
            {
                //TODO Create the OSILAYER
            }
            #endregion Layer4
            //Retrieve the Layer4_ProtocolID
            int iLayer4ProtocolID=XModel.PROTOCOL.Where(o=>o.ProtocolName == sLayer4_Protocol && o.OSILayerID == iProtoLayer4ID).Select(o=>o.ProtocolID).FirstOrDefault();
            if(iLayer4ProtocolID<=0)
            {
                //TODO ERROR
            }

            
            //Add the Address IP/Hostname
            #region AddAddress
            XORCISMModel.ADDRESS IPAddress=new ADDRESS();
            int iAddressID = 0;
            try
            {
                //Get the ipv4-addr (CybOX) CategoryID (compatibility with Asset Identification)
                int iAddCat = 0;
                iAddCat = XModel.ADDRESSCATEGORY.Where(o => o.AddressCategoryName == "ipv4-addr").Select(o => o.AddressCategoryID).FirstOrDefault();    //Hardcoded
                IPAddress.AddressCategoryID = iAddCat;

                //Check if already exists. TODO: same organisation? See ASSETADDRESS
                iAddressID = XModel.ADDRESS.Where(o => o.Address_Value == sIPAddress).Select(o => o.AddressID).FirstOrDefault();
                if (iAddressID <= 0)
                {
                    IPAddress.Address_Value = sIPAddress;
                    //TODO check if country... retrievable
                    //IPAddress.is_source = true;
                    //IPAddSource.is_destination=false;
                    XModel.ADDRESS.Add(IPAddress);
                    XModel.SaveChanges();
                    iAddressID = IPAddress.AddressID;
                }
            }
            catch(Exception exAddAddress)
            {
                Console.WriteLine("Exception exAddAddress: " + exAddAddress.Message + " " + exAddAddress.InnerException);
            }
            #endregion AddAddress
                
            //Add the Port
            #region AddPort
            int iPortID = XModel.PORT.Where(o => o.Port_Value == iPortValue && o.ProtocolID == iProtoLayer4ID).Select(o => o.PortID).FirstOrDefault();
            if (iPortID <= 0)
            {
                try
                {
                    XORCISMModel.PORT PortSource = new PORT();
                    //TODO check if already exists
                    PortSource.ProtocolID = iProtoLayer4ID;
                    PortSource.Port_Value = iPortValue;
                    //PortSource.VocabularyID=;
                    XModel.PORT.Add(PortSource);
                    XModel.SaveChanges();
                    iPortID = PortSource.PortID;
                }
                catch(Exception exAddPort)
                {
                    Console.WriteLine("Exception exAddPort: " + exAddPort.Message + " " + exAddPort.InnerException);
                }
            }
            #endregion AddPort

            //Add the SocketAddress
            int iSocketAddressID = 0;
            iSocketAddressID = XModel.SOCKETADDRESS.Where(o => o.AddressID == iAddressID && o.PortID == iPortID).Select(o => o.SocketAddressID).FirstOrDefault();
            try
            {
                XORCISMModel.SOCKETADDRESS oSockAddress = new SOCKETADDRESS();
                oSockAddress.AddressID = iAddressID;
                oSockAddress.PortID = iPortID;
                oSockAddress.CreatedDate = DateTimeOffset.Now;
                oSockAddress.timestamp = DateTimeOffset.Now;
                oSockAddress.isEncrypted = false;
                XModel.SOCKETADDRESS.Add(oSockAddress);
                XModel.SaveChanges();
                iSocketAddressID = oSockAddress.SocketAddressID;
            }
            catch(Exception exAddSocketAddress)
            {
                Console.WriteLine("Exception exAddSocketAddress: " + exAddSocketAddress.Message + " " + exAddSocketAddress.InnerException);
            }

            return iSocketAddressID;
        }
    }
}
