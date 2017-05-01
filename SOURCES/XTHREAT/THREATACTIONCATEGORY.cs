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
    
    public partial class THREATACTIONCATEGORY
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public THREATACTIONCATEGORY()
        {
            this.THREATACTIONLOCATIONFORTHREATACTIONCATEGORY = new HashSet<THREATACTIONLOCATIONFORTHREATACTIONCATEGORY>();
            this.THREATACTIONTARGET = new HashSet<THREATACTIONTARGET>();
            this.THREATACTIONVARIETY = new HashSet<THREATACTIONVARIETY>();
            this.THREATACTIONVECTOR = new HashSet<THREATACTIONVECTOR>();
        }
    
        public int ThreatActionCategoryID { get; set; }
        public string ThreatActionCategoryName { get; set; }
        public int VocabularyID { get; set; }
        public Nullable<System.DateTimeOffset> CreatedDate { get; set; }
        public Nullable<System.DateTimeOffset> timestamp { get; set; }
        public Nullable<System.DateTimeOffset> ValidFromDate { get; set; }
        public Nullable<System.DateTimeOffset> ValidUntilDate { get; set; }
        public Nullable<bool> isEncrypted { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<THREATACTIONLOCATIONFORTHREATACTIONCATEGORY> THREATACTIONLOCATIONFORTHREATACTIONCATEGORY { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<THREATACTIONTARGET> THREATACTIONTARGET { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<THREATACTIONVARIETY> THREATACTIONVARIETY { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<THREATACTIONVECTOR> THREATACTIONVECTOR { get; set; }
    }
}