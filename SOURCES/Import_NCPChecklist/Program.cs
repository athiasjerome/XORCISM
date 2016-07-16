using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Entity;
using System.Xml;
using XORCISMModel;

namespace Import_NCPChecklist
{
    class Program
    {
        /// <summary>
        /// Copyright (C) 2015-2016 Jerome Athias - frhack.org
        /// *** BETA VERSION ***
        /// Parser for National Checklist Program (NCP) Checklists feed XML file and import into an XORCISM database
        /// This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation; either version 2 of the License, or (at your option) any later version.
        /// 
        /// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
        /// 
        /// You should have received a copy of the GNU General Public License along with this program; if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
        /// </summary>
        /// 
        static void Main(string[] args)
        {
            //https://nvd.nist.gov/download.cfm#CVE_FEED
            //National Checklist Program (NCP) Checklists

            XORCISMEntities model = new XORCISMEntities();

            //VOCABULARIES
            
            int iVocabularyNCPID = 0;
            #region vocabularyncp
            try
            {
                //Hardcoded
                iVocabularyNCPID = model.VOCABULARY.Where(o => o.VocabularyName == "NCP").Select(o => o.VocabularyID).FirstOrDefault();
            }
            catch (Exception ex)
            {

            }
            if (iVocabularyNCPID <= 0)
            {
                XORCISMModel.VOCABULARY oVocabulary = new XORCISMModel.VOCABULARY();
                oVocabulary.CreatedDate = DateTimeOffset.Now;
                oVocabulary.VocabularyName = "NCP"; //Hardcoded
                model.VOCABULARY.Add(oVocabulary);
                model.SaveChanges();
                iVocabularyNCPID = oVocabulary.VocabularyID;
                Console.WriteLine("DEBUG iVocabularyNCPID=" + iVocabularyNCPID);
            }
            #endregion vocabularyncp
            
            //TODO: download if needed (if updated)
            string filepath = "checklist-0.1-feed.xml"; //Hardcoded

            Console.WriteLine("DEBUG " + DateTimeOffset.Now);
            XmlDocument docXML = new XmlDocument();
            //TODO: Security controls/checks
            //TODO: XSD validation
            //TODO: ...
            docXML.Load(filepath);

            XmlNodeList nodes;
            nodes = docXML.SelectNodes("/ncp");

            foreach (XmlNode nodeEntry in docXML.DocumentElement.ChildNodes)
            {
                //<entry ncp-checklist-id="7">
                string sChecklistVocabularyID = "";
                CHECKLIST oChecklist = null;
                int iChecklistID = 0;

                try
                {
                    sChecklistVocabularyID = nodeEntry.Attributes["ncp-checklist-id"].InnerText;
                }
                catch(Exception exsChecklistVocabularyID)
                {
                    Console.WriteLine("Exception: exiChecklistVocabularyID");
                }
                foreach (XmlNode nodeEntryInfo in nodeEntry.ChildNodes)
                {
                    switch (nodeEntryInfo.Name)
                    {
                        case "ncp:checklist-details":
                            //int iChecklistID = 0;
                            foreach (XmlNode nodeChecklistDetail in nodeEntryInfo.ChildNodes)
                            {
                                switch(nodeChecklistDetail.Name)
                                {
                                    case "ncp:title":
                                        string sChecklistName = "";
                                        string sChecklistVersion = "";
                                        foreach (XmlNode nodeTitle in nodeChecklistDetail.ChildNodes)
                                        {
                                            switch (nodeTitle.Name)
                                            {
                                                case "ncp:checklist-name":
                                                    sChecklistName = nodeTitle.InnerText;
                                                    break;
                                                case "ncp:version":
                                                    sChecklistVersion = nodeTitle.InnerText;
                                                    break;
                                                default:
                                                    Console.WriteLine("ERROR Missing code for nodeTitle.Name=" + nodeTitle.Name);
                                                    break;
                                            }
                                        }
                                        #region checklist
                                        
                                        try
                                        {
                                            //TODO? add ChecklistVersion
                                            oChecklist = model.CHECKLIST.Where(o => o.Title == sChecklistName).FirstOrDefault();
                                        }
                                        catch(Exception exiChecklistID)
                                        {

                                        }
                                        if (oChecklist!=null)
                                        {
                                            iChecklistID = oChecklist.ChecklistID;
                                            //Update CHECKLIST
                                            try
                                            {
                                                oChecklist.ChecklistVersion = sChecklistVersion;
                                                oChecklist.ChecklistVocabularyID = sChecklistVocabularyID;
                                                oChecklist.timestamp = DateTimeOffset.Now;
                                                model.SaveChanges();
                                            }
                                            catch (Exception exUpdateCHECKLIST)
                                            {
                                                Console.WriteLine("Exception: exUpdateCHECKLIST " + exUpdateCHECKLIST.Message + " " + exUpdateCHECKLIST.InnerException);
                                            }
                                            
                                        }
                                        else
                                        {
                                            Console.WriteLine("DEBUG Adding CHECKLIST");
                                            //NOTE: Model comes from OCIL   https://scap.nist.gov/specifications/ocil/
                                            try
                                            {
                                                oChecklist = new CHECKLIST();
                                                oChecklist.CreatedDate = DateTimeOffset.Now;
                                                oChecklist.Title = sChecklistName;
                                                oChecklist.ChecklistVersion = sChecklistVersion;
                                                //oChecklist.ChecklistCategoryID= //TODO
                                                //oChecklistOrganisationID  //Updated later
                                                oChecklist.ChecklistVocabularyID = sChecklistVocabularyID;
                                                oChecklist.VocabularyID = iVocabularyNCPID;
                                                oChecklist.timestamp = DateTimeOffset.Now;
                                                model.CHECKLIST.Add(oChecklist);
                                                model.SaveChanges();
                                                iChecklistID = oChecklist.ChecklistID;
                                            }
                                            catch (Exception exAddCHECKLIST)
                                            {
                                                Console.WriteLine("Exception: exAddCHECKLIST " + exAddCHECKLIST.Message + " " + exAddCHECKLIST.InnerException);
                                            }
                                        }
                                        #endregion checklist

                                        //TODO  CHECKLISTTAG    sChecklistName

                                        break;

                                    case "ncp:authority":
                                        #region authority
                                        string sOrganisationName = "";
                                        string sOrganisationReference = ""; //TODO
                                        string sOrganisationDescription = "";
                                        int iRoleID = 0;
                                        foreach (XmlNode nodeAuthorityDetail in nodeChecklistDetail.ChildNodes)
                                        {
                                            switch (nodeAuthorityDetail.Name)
                                            {
                                                case "ncp:organization":
                                                    //<ncp:organization system-id="http://www.disa.mil/" name="Defense Information Systems Agency">
                                                    sOrganisationName = nodeAuthorityDetail.Attributes["name"].InnerText;
                                                    sOrganisationReference = nodeAuthorityDetail.Attributes["system-id"].InnerText;
                                                    Console.WriteLine("DEBUG sOrganisationReference=" + sOrganisationReference);
                                                    foreach (XmlNode nodeOrganizationDetail in nodeAuthorityDetail.ChildNodes)
                                                    {
                                                        switch (nodeOrganizationDetail.Name)
                                                        {
                                                            case "ncp:description":
                                                                //Not provided.
                                                                sOrganisationDescription = nodeOrganizationDetail.InnerText;
                                                                break;
                                                            default:
                                                                Console.WriteLine("ERROR Missing code for nodeOrganizationDetail.Name=" + nodeOrganizationDetail.Name);
                                                                break;
                                                        }
                                                    }
                                                    break;
                                                case "ncp:type":
                                                    //GOVERNMENTAL_AUTHORITY
                                                    //Using the table ROLE
                                                    #region authorityrole
                                                    string sAuthority=nodeAuthorityDetail.InnerText;

                                                    try
                                                    {
                                                        iRoleID = model.ROLE.Where(o => o.RoleName == sAuthority).FirstOrDefault().RoleID;
                                                    }
                                                    catch(Exception ex)
                                                    {

                                                    }
                                                    if(iRoleID<=0)
                                                    {
                                                        Console.WriteLine("Adding ROLE/AUTHORITY");
                                                        try
                                                        {
                                                            ROLE oRole = new ROLE();
                                                            oRole.CreatedDate = DateTimeOffset.Now;
                                                            oRole.RoleName = sAuthority;
                                                            //oRole.RoleDescription //TODO  See https://web.nvd.nist.gov/view/ncp/repository/glossary
                                                            oRole.VocabularyID = iVocabularyNCPID;
                                                            oRole.timestamp = DateTimeOffset.Now;
                                                            model.ROLE.Add(oRole);
                                                            model.SaveChanges();
                                                            iRoleID = oRole.RoleID;
                                                        }
                                                        catch(Exception exAddRole)
                                                        {
                                                            Console.WriteLine("Exception: exAddRole " + exAddRole.Message + " " + exAddRole.InnerException);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //Update ROLE
                                                    }
                                                    #endregion authorityrole
                                                    break;
                                                default:
                                                    Console.WriteLine("ERROR Missing code for nodeAuthorityDetail.Name=" + nodeAuthorityDetail.Name);
                                                    break;
                                            }
                                        }

                                        int iOrganisationID = 0;
                                        #region organisation
                                        try
                                        {
                                            iOrganisationID = model.ORGANISATION.Where(o => o.OrganisationName == sOrganisationName || o.OrganisationKnownAs == sOrganisationName).FirstOrDefault().OrganisationID;
                                        }
                                        catch (Exception exiOrganisationID)
                                        {

                                        }
                                        if(iOrganisationID<=0)
                                        {
                                            Console.WriteLine("DEBUG Adding ORGANISATION");
                                            try
                                            {
                                                ORGANISATION oOrganisation = new ORGANISATION();
                                                oOrganisation.CreatedDate = DateTimeOffset.Now;
                                                oOrganisation.OrganisationName = sOrganisationName;
                                                oOrganisation.OrganisationDescription = sOrganisationDescription;
                                                oOrganisation.VocabularyID = iVocabularyNCPID;
                                                oOrganisation.timestamp = DateTimeOffset.Now;
                                                model.ORGANISATION.Add(oOrganisation);
                                                model.SaveChanges();
                                                iOrganisationID = oOrganisation.OrganisationID;
                                            }
                                            catch(Exception exAddORGANISATION)
                                            {
                                                Console.WriteLine("Exception: exAddORGANISATION " + exAddORGANISATION.Message + " " + exAddORGANISATION.InnerException);
                                            }
                                        }
                                        else
                                        {
                                            //Update ORGANISATION
                                            //TODO i.e. Description
                                        }
                                        #endregion organisation

                                        try
                                        {
                                            oChecklist.OrganisationID = iOrganisationID;
                                            oChecklist.timestamp = DateTimeOffset.Now;
                                            model.SaveChanges();
                                        }
                                        catch(Exception exChecklistOrganisationID)
                                        {
                                            Console.WriteLine("Exception: exChecklistOrganisationID " + exChecklistOrganisationID.Message + " " + exChecklistOrganisationID.InnerException);
                                        }

                                        //TODO
                                        //<ncp:organization system-id="http://www.disa.mil/" name="Defense Information Systems Agency">
                                        //ORGANISATIONREFERENCE or ORGANISATIONDOMAINNAME

                                        #region  CHECKLISTAUTHORITY
                                        int iChecklistAuthorityID = 0;
                                        //TODO? VocabularyID
                                        try
                                        {
                                            iChecklistAuthorityID = model.CHECKLISTAUTHORITY.Where(o => o.ChecklistID == iChecklistID && o.RoleID == iRoleID).FirstOrDefault().ChecklistAuthorityID;
                                        }
                                        catch(Exception ex)
                                        {

                                        }
                                        if(iChecklistAuthorityID<=0)
                                        {
                                            Console.WriteLine("DEBUG Adding CHECKLISTAUTHORITY");
                                            try
                                            {
                                                CHECKLISTAUTHORITY oChecklistAuthority = new CHECKLISTAUTHORITY();
                                                oChecklistAuthority.CreatedDate = DateTimeOffset.Now;
                                                oChecklistAuthority.ChecklistID = iChecklistID;
                                                oChecklistAuthority.OrganisationID = iOrganisationID;
                                                oChecklistAuthority.RoleID = iRoleID;
                                                oChecklistAuthority.VocabularyID = iVocabularyNCPID;
                                                oChecklistAuthority.timestamp = DateTimeOffset.Now;
                                                model.CHECKLISTAUTHORITY.Add(oChecklistAuthority);
                                                model.SaveChanges();
                                                iChecklistAuthorityID = oChecklistAuthority.ChecklistAuthorityID;
                                            }
                                            catch(Exception exAddChecklistAuthority)
                                            {
                                                Console.WriteLine("Exception: exAddChecklistAuthority " + exAddChecklistAuthority.Message + " " + exAddChecklistAuthority.InnerException);
                                            }
                                        }
                                        else
                                        {
                                            //Update CHECKLISTAUTHORITY
                                        }
                                        #endregion  CHECKLISTAUTHORITY
                                        #endregion authority
                                        break;
                                    case "ncp:resource":
                                        #region resource
                                        string sReferenceURL = "";
                                        int iReferenceAuthorID = 0;
                                        string sReferenceTitle = "";
                                        foreach (XmlNode nodeResource in nodeChecklistDetail.ChildNodes)
                                        {
                                            switch (nodeResource.Name)
                                            {
                                                case "ncp:reference":
                                                    try
                                                    {
                                                        sReferenceURL = nodeResource.Attributes["href"].InnerText;
                                                    }
                                                    catch(Exception )
                                                    {

                                                    }
                                                    break;
                                                case "ncp:author":
                                                    //<ncp:author system-id="http://www.disa.mil/" name="Defense Information Systems Agency">
                                                    //TODO
                                                    //iReferenceAuthorID
                                                    break;
                                                case "ncp:title":
                                                    //.NET Framework Security Checklist
                                                    sReferenceTitle = nodeResource.InnerText;
                                                    break;
                                                default:
                                                    //ncp:sha-1
                                                    //ncp:sha-256
                                                    //<ncp:type>Prose</ncp:type>
                                                    Console.WriteLine("ERROR Missing code for nodeResource.Name=" + nodeResource.Name);
                                                    break;
                                            }
                                        }

                                        //TODO Add REFERENCE    REFERENCEHASHVALUE  CHECKLISTREFERENCE

                                        #endregion resource
                                        break;
                                    case "ncp:target-product":
                                        #region targetproduct
                                        //<ncp:target-product fips-140-2-compliance-flag="true">
                                        string sProductName = string.Empty;
                                        string sCPEName = string.Empty;
                                        string sProductCategory = string.Empty;
                                        foreach (XmlNode nodeProduct in nodeChecklistDetail.ChildNodes)
                                        {
                                            switch(nodeProduct.Name)
                                            {
                                                case "ncp:name":
                                                    sProductName = nodeProduct.InnerText;
                                                    break;
                                                case "ncp:cpe-name":
                                                    sCPEName = nodeProduct.InnerText;
                                                    break;
                                                case "ncp:product-category":
                                                    sProductCategory = nodeProduct.InnerText;
                                                    break;
                                                default:
                                                    Console.WriteLine("ERROR Missing code for nodeProduct " + nodeProduct.Name);
                                                    break;
                                            }
                                        }
                                        Console.WriteLine("DEBUG sProductName=" + sProductName);    //Microsoft .NET Framework 1.0
                                        Console.WriteLine("DEBUG sCPEName=" + sCPEName);    //Microsoft .NET Framework 1.0
                                        Console.WriteLine("DEBUG sProductCategory=" + sProductCategory);    //
                                        //Operating System  //TODO? OS


                                        int iCategoryID = 0;
                                        #region category
                                        //TODO? + VocabularyID
                                        try
                                        {
                                            iCategoryID = model.CATEGORY.Where(o => o.CategoryName == sProductCategory).FirstOrDefault().CategoryID;
                                        }
                                        catch (Exception exiCategoryID)
                                        {

                                        }
                                        if(iCategoryID<=0)
                                        {
                                            Console.WriteLine("DEBUG Adding CATEGORY");
                                            try
                                            {
                                                CATEGORY oCategory = new CATEGORY();
                                                oCategory.CreatedDate = DateTimeOffset.Now;
                                                oCategory.CategoryName = sProductCategory;
                                                oCategory.VocabularyID = iVocabularyNCPID;
                                                oCategory.timestamp = DateTimeOffset.Now;
                                                model.CATEGORY.Add(oCategory);
                                                model.SaveChanges();
                                                iCategoryID = oCategory.CategoryID;
                                            }
                                            catch(Exception exAddCategory)
                                            {
                                                Console.WriteLine("Exception: exAddCategory " + exAddCategory.Message + " " + exAddCategory.InnerException);
                                            }
                                        }
                                        #endregion category

                                        int iProductCategoryID = 0;
                                        #region productcategory
                                        //TODO? + VocabularyID
                                        try
                                        {
                                            iProductCategoryID = model.PRODUCTCATEGORY.Where(o => o.ProductCategoryName == sProductCategory).FirstOrDefault().ProductCategoryID;
                                        }
                                        catch (Exception exiProductCategoryID)
                                        {

                                        }
                                        if (iProductCategoryID <= 0)
                                        {
                                            Console.WriteLine("DEBUG Adding PRODUCTCATEGORY");
                                            try
                                            {
                                                PRODUCTCATEGORY oProductCategory = new PRODUCTCATEGORY();
                                                oProductCategory.CreatedDate = DateTimeOffset.Now;
                                                oProductCategory.ProductCategoryName = sProductCategory;
                                                oProductCategory.CategoryID = iCategoryID;
                                                //TODO
                                                //oProductCategory.OrganisationID   //Defense Information Systems Agency
                                                oProductCategory.VocabularyID = iVocabularyNCPID;
                                                oProductCategory.timestamp = DateTimeOffset.Now;
                                                model.PRODUCTCATEGORY.Add(oProductCategory);
                                                model.SaveChanges();
                                                iProductCategoryID = oProductCategory.ProductCategoryID;
                                            }
                                            catch(Exception exAddProductCategory)
                                            {
                                                Console.WriteLine("Exception: exAddProductCategory " + exAddProductCategory.Message + " " + exAddProductCategory.InnerException);
                                            }
                                        }
                                        #endregion productcategory

                                        int iProductID = 0;
                                        #region product
                                        
                                        //Note: It seems that ProductNames are the 'same' in NCP and OVAL :-)
                                        try
                                        {
                                            iProductID = model.PRODUCT.Where(o => o.ProductName == sProductName).FirstOrDefault().ProductID;
                                        }
                                        catch(Exception exiProductID)
                                        {

                                        }
                                        if(iProductID<=0)
                                        {
                                            Console.WriteLine("DEBUG Adding PRODUCT");
                                            try
                                            {
                                                PRODUCT oProduct = new PRODUCT();
                                                oProduct.ProductName = sProductName;
                                                //TODO? Vendor...
                                                string sProductVendor = "";
                                                #region productvendor
                                                //Hardcoded
                                                if (sProductName.Contains("Microsoft")) sProductVendor = "Microsoft";
                                                if (sProductName.Contains("Windows")) sProductVendor = "Microsoft";
                                                if (sProductName.Contains("VBScript")) sProductVendor = "Microsoft";
                                                if (sProductName.Contains("Skype")) sProductVendor = "Microsoft";
                                                if (sProductName.Contains("Outlook")) sProductVendor = "Microsoft";

                                                if (sProductName.Contains("MSN Messenger")) sProductVendor = "Microsoft";
                                                if (sProductName.Contains("Internet Explorer")) sProductVendor = "Microsoft";
                                                //Print Spooler Service
                                                //Licence Logging Service
                                                //File and Print Sharing
                                                //Remote Desktop Client
                                                //Local Security Authority Subsystem Service (LSASS)
                                                //Task Scheduler
                                                //Kerberos
                                                //NetBIOS

                                                if (sProductName.Contains("Google")) sProductVendor = "Google";
                                                if (sProductName.Contains("Adobe")) sProductVendor = "Adobe";
                                                if (sProductName.Contains("Flash Player")) sProductVendor = "Adobe";

                                                if (sProductName.Contains("Apple")) sProductVendor = "Apple";
                                                if (sProductName.Contains("Mozilla")) sProductVendor = "Mozilla";
                                                if (sProductName.Contains("Oracle")) sProductVendor = "Oracle";
                                                if (sProductName.Contains("Solaris")) sProductVendor = "Oracle";
                                                //Oracle VirtualBox
                                                if (sProductName.Contains("Apache")) sProductVendor = "Apache";
                                                if (sProductName.Contains("OpenOffice")) sProductVendor = "Apache";

                                                if (sProductName.Contains("avast")) sProductVendor = "Avast";
                                                if (sProductName.Contains("TechSmith")) sProductVendor = "TechSmith";
                                                if (sProductName.Contains("Kaspersky")) sProductVendor = "Kaspersky";
                                                if (sProductName.Contains("Symantec")) sProductVendor = "Symantec";
                                                if (sProductName.Contains("Norton")) sProductVendor = "Symantec";   //Norton
                                                if (sProductName.Contains("McAfee")) sProductVendor = "McAfee";
                                                if (sProductName.Contains("MySQL")) sProductVendor = "MySQL";
                                                if (sProductName.Contains("Kodak")) sProductVendor = "Kodak";
                                                if (sProductName.Contains("Lotus")) sProductVendor = "Lotus";
                                                if (sProductName.Contains("VMware")) sProductVendor = "VMware";
                                                if (sProductName.Contains("Trend Micro")) sProductVendor = "Trend Micro";

                                                //Crystal Enterprise
                                                if (sProductName.Contains("Crystal Reports")) sProductVendor = "SAP";   //SAP AG?   SAP AE?

                                                if (sProductName.Contains("PostgreSQL")) sProductVendor = "DB Consulting Inc.";

                                                if (sProductVendor == "")
                                                {
                                                    if (sProductName.Contains("IBM")) sProductVendor = "IBM";
                                                    if (sProductName.Contains("Sun")) sProductVendor = "Oracle";
                                                }

                                                //Macrovision   Rovi Corporation
                                                //Opera
                                                //VLC
                                                //Winamp
                                                //VirtualBox
                                                //Perl
                                                //Python
                                                //RealPlayer
                                                //DirectX
                                                //DirectShow
                                                //...



                                                #endregion productvendor

                                                Console.WriteLine("DEBUG sProductVendor="+sProductVendor);
                                                oProduct.ProductVendor = sProductVendor;
                                                //TODO  OrganisationID

                                                oProduct.CPEName = sCPEName;
                                                oProduct.CreatedDate = DateTimeOffset.Now;
                                                oProduct.VocabularyID = iVocabularyNCPID;
                                                oProduct.timestamp = DateTimeOffset.Now;
                                                model.PRODUCT.Add(oProduct);
                                                model.SaveChanges();
                                                iProductID = oProduct.ProductID;
                                            }
                                            catch(Exception exAddProduct)
                                            {
                                                Console.WriteLine("Exception: exAddProduct " + exAddProduct.Message + " " + exAddProduct.InnerException);
                                            }

                                        }
                                        #endregion product

                                        int iCategoryForProductID = 0;
                                        #region PRODUCTCATEGORYFORPRODUCT
                                        try
                                        {
                                            iCategoryForProductID = model.PRODUCTCATEGORYFORPRODUCT.Where(o => o.ProductCategoryID == iProductCategoryID && o.ProductID == iProductID).FirstOrDefault().ProductCategoryForProductID;
                                        }
                                        catch(Exception ex)
                                        {

                                        }
                                        if(iCategoryForProductID<=0)
                                        {
                                            Console.WriteLine("Adding PRODUCTCATEGORYFORPRODUCT");
                                            try
                                            {
                                                PRODUCTCATEGORYFORPRODUCT oCategoryForProduct = new PRODUCTCATEGORYFORPRODUCT();
                                                oCategoryForProduct.CreatedDate = DateTimeOffset.Now;
                                                oCategoryForProduct.ProductCategoryID = iProductCategoryID;
                                                oCategoryForProduct.ProductID = iProductID;
                                                oCategoryForProduct.VocabularyID = iVocabularyNCPID;
                                                oCategoryForProduct.timestamp = DateTimeOffset.Now;
                                                model.PRODUCTCATEGORYFORPRODUCT.Add(oCategoryForProduct);
                                                model.SaveChanges();
                                            }
                                            catch(Exception exPRODUCTCATEGORYFORPRODUCT)
                                            {
                                                Console.WriteLine("Exception exPRODUCTCATEGORYFORPRODUCT " + exPRODUCTCATEGORYFORPRODUCT.Message + " " + exPRODUCTCATEGORYFORPRODUCT.InnerException);
                                            }
                                        }
                                        else
                                        {
                                            //Update PRODUCTCATEGORYFORPRODUCT
                                        }
                                        #endregion PRODUCTCATEGORYFORPRODUCT

                                        int iCPEID = 0;
                                        #region cpe
                                        try
                                        {
                                            iCPEID = model.CPE.Where(o => o.CPEName == sCPEName).FirstOrDefault().CPEID;
                                        }
                                        catch(Exception exCPEID)
                                        {

                                        }
                                        if(iCPEID<=0)
                                        {
                                            Console.WriteLine("ERROR CPE Unknown " + sCPEName);
                                            //Console.WriteLine("DEBUG Adding CPE");

                                        }
                                        #endregion cpe

                                        #endregion targetproduct
                                        break;
                                    case "ncp:other-link":
                                        #region link
                                        //<ncp:other-link dependency_flag="true">
                                        string sReference = "";
                                        string sReferenceLinkTitle = "";
                                        foreach (XmlNode nodeLink in nodeChecklistDetail.ChildNodes)
                                        {
                                            switch (nodeLink.Name)
                                            {
                                                case "ncp:reference":
                                                    //ncp:reference href="http://www.nsa.gov/ia/_files/app/I731-008R-2006.pdf"/>
                                                    //TODO? other attributes?
                                                    try
                                                    {
                                                        sReference = nodeLink.Attributes["href"].InnerText;
                                                    }
                                                    catch(Exception exhref)
                                                    {

                                                    }
                                                    break;
                                                case "ncp:title":
                                                    sReferenceLinkTitle = nodeLink.InnerText;
                                                    break;
                                                default:
                                                    Console.WriteLine("ERROR MISSING CODE FOR nodeLink.Name=" + nodeLink.Name);
                                                    break;
                                            }
                                        }
                                        if(sReference!="")
                                        {
                                            #region reference
                                            int iReferenceID = 0;
                                            try
                                            {
                                                iReferenceID = model.REFERENCE.Where(o => o.ReferenceURL == sReference).FirstOrDefault().ReferenceID;
                                            }
                                            catch(Exception exiReferenceID)
                                            {

                                            }
                                            if (iReferenceID <= 0)
                                            {
                                                Console.WriteLine("DEBUG Adding REFERENCE");
                                                try
                                                {
                                                    REFERENCE oReference = new REFERENCE();
                                                    oReference.CreatedDate = DateTimeOffset.Now;
                                                    oReference.ReferenceURL = sReference;
                                                    oReference.ReferenceTitle = sReferenceLinkTitle;
                                                    oReference.VocabularyID = iVocabularyNCPID;
                                                    oReference.timestamp = DateTimeOffset.Now;
                                                    model.REFERENCE.Add(oReference);
                                                    model.SaveChanges();
                                                    iReferenceID = oReference.ReferenceID;
                                                }
                                                catch (Exception exAddReference)
                                                {
                                                    Console.WriteLine("Exception: exAddReference " + exAddReference.Message + " " + exAddReference.InnerException);
                                                }
                                            }
                                            else
                                            {
                                                //Update REFERENCE
                                                //TODO Test if same Title

                                            }
                                            #endregion reference
                                        }

                                        #endregion link
                                        break;
                                    default:
                                        Console.WriteLine("ERROR Missing code for nodeChecklistDetail " + nodeChecklistDetail.Name);
                                        break;
                                }
                            }
                            break;
                        default:
                            Console.WriteLine("ERROR Missing code for nodeEntryInfo " + nodeEntryInfo.Name);
                            //<ncp:documentation>
                                //<ncp:checklist-role>Desktop Client</ncp:checklist-role>
                                //CHECKLISTCATEGORY
                                //<ncp:regulatory-compliance>DOD Directive 8500.</ncp:regulatory-compliance>
                                //<ncp:regulatory-compliance>TBD</ncp:regulatory-compliance>
                                //COMPLIANCE
                            break;
                    }
                }
            }
        }
    }
}
