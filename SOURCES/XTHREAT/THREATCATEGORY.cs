//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace XTHREATModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class THREATCATEGORY
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public THREATCATEGORY()
        {
            this.THREATCATEGORYDESCRIPTION1 = new HashSet<THREATCATEGORYDESCRIPTION>();
            this.THREATCATEGORYREFERENCE = new HashSet<THREATCATEGORYREFERENCE>();
            this.THREATCATEGORYTAG = new HashSet<THREATCATEGORYTAG>();
        }
    
        public int ThreatCategoryID { get; set; }
        public string ThreatCategoryGUID { get; set; }
        public Nullable<int> CategoryID { get; set; }
        public string ThreatCategoryName { get; set; }
        public string ThreatCategoryDescription { get; set; }
        public Nullable<System.DateTimeOffset> CreatedDate { get; set; }
        public Nullable<int> CreationObjectID { get; set; }
        public Nullable<System.DateTimeOffset> timestamp { get; set; }
        public Nullable<System.DateTimeOffset> ValidFromDate { get; set; }
        public Nullable<System.DateTimeOffset> ValidUntilDate { get; set; }
        public Nullable<int> VocabularyID { get; set; }
        public Nullable<int> ImportanceID { get; set; }
        public Nullable<int> ValidityID { get; set; }
        public Nullable<bool> isEncrypted { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<THREATCATEGORYDESCRIPTION> THREATCATEGORYDESCRIPTION1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<THREATCATEGORYREFERENCE> THREATCATEGORYREFERENCE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<THREATCATEGORYTAG> THREATCATEGORYTAG { get; set; }
    }
}