//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace XORCISMModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class ASSETREPORTCOLLECTION
    {
        public int AssetReportCollectionID { get; set; }
        public string ARFReportCollectionID { get; set; }
        public Nullable<int> ReportRequestsID { get; set; }
        public Nullable<int> AssetsID { get; set; }
        public Nullable<int> ReportsID { get; set; }
        public Nullable<int> ARFRelationshipsID { get; set; }
        public Nullable<int> ARFExtendedInfosID { get; set; }
    
        public virtual ARFEXTENDEDINFOS ARFEXTENDEDINFOS { get; set; }
        public virtual ARFRELATIONSHIPS ARFRELATIONSHIPS { get; set; }
        public virtual ASSETS ASSETS { get; set; }
        public virtual REPORTREQUESTS REPORTREQUESTS { get; set; }
        public virtual REPORTS REPORTS { get; set; }
    }
}
