//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace XATTACKModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class ATTACKTOOLMODULEAUTHENTICATIONTYPE
    {
        public int AttackToolModuleAuthenticationTypeID { get; set; }
        public Nullable<int> AttackToolModuleID { get; set; }
        public Nullable<int> AuthenticationTypeID { get; set; }
        public string AttackToolModuleAuthenticationTypeDescription { get; set; }
        public Nullable<int> ConfidenceLevelID { get; set; }
        public Nullable<System.DateTimeOffset> CreatedDate { get; set; }
        public Nullable<System.DateTimeOffset> ValidFromDate { get; set; }
        public Nullable<System.DateTimeOffset> ValidUntilDate { get; set; }
        public Nullable<System.DateTimeOffset> timestamp { get; set; }
    
        public virtual ATTACKTOOLMODULE ATTACKTOOLMODULE { get; set; }
    }
}
