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
    
    public partial class ATTACKRESOURCE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ATTACKRESOURCE()
        {
            this.ATTACKRESOURCEFORATTACKPATTERN = new HashSet<ATTACKRESOURCEFORATTACKPATTERN>();
            this.ATTACKRESOURCETAG = new HashSet<ATTACKRESOURCETAG>();
        }
    
        public int AttackResourceID { get; set; }
        public string AttackResourceText { get; set; }
        public Nullable<int> VocabularyID { get; set; }
        public Nullable<System.DateTimeOffset> CreatedDate { get; set; }
        public Nullable<System.DateTimeOffset> timestamp { get; set; }
        public Nullable<bool> isEncrypted { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ATTACKRESOURCEFORATTACKPATTERN> ATTACKRESOURCEFORATTACKPATTERN { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ATTACKRESOURCETAG> ATTACKRESOURCETAG { get; set; }
    }
}
