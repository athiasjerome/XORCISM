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
    
    public partial class ATTACKCATEGORY
    {
        public int AttackCategoryID { get; set; }
        public string AttackCategoryGUID { get; set; }
        public Nullable<int> CategoryID { get; set; }
        public string CategoryGUID { get; set; }
        public string AttackCategoryName { get; set; }
        public string AttackCategoryDescription { get; set; }
        public Nullable<int> VocabularyID { get; set; }
        public string VocabularyGUID { get; set; }
        public Nullable<System.DateTimeOffset> CreatedDate { get; set; }
        public Nullable<int> CreationObjectID { get; set; }
        public string CreationObjectGUID { get; set; }
        public Nullable<System.DateTimeOffset> timestamp { get; set; }
        public Nullable<System.DateTimeOffset> ValidFromDate { get; set; }
        public Nullable<System.DateTimeOffset> ValidUntilDate { get; set; }
        public Nullable<bool> isEncrypted { get; set; }
    }
}
