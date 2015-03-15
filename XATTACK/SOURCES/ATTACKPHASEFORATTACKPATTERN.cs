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
    
    public partial class ATTACKPHASEFORATTACKPATTERN
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ATTACKPHASEFORATTACKPATTERN()
        {
            this.ATTACKSTEP = new HashSet<ATTACKSTEP>();
        }
    
        public int AttackPatternAttackPhaseID { get; set; }
        public Nullable<int> AttackPatternID { get; set; }
        public string AttackPatternGUID { get; set; }
        public string AttackPhaseGUID { get; set; }
        public Nullable<int> AttackPhaseID { get; set; }
        public Nullable<int> AttackPhaseVocabularyID { get; set; }
        public Nullable<int> AttackPhaseOrder { get; set; }
        public string AttackPhaseDescription { get; set; }
        public Nullable<System.DateTimeOffset> CreatedDate { get; set; }
        public Nullable<int> CreationObjectID { get; set; }
        public Nullable<System.DateTimeOffset> timestamp { get; set; }
        public Nullable<int> VocabularyID { get; set; }
        public Nullable<bool> isEncrypted { get; set; }
        public Nullable<System.DateTimeOffset> ValidFromDate { get; set; }
        public Nullable<System.DateTimeOffset> ValidUntilDate { get; set; }
    
        public virtual ATTACKPATTERN ATTACKPATTERN { get; set; }
        public virtual ATTACKPHASE ATTACKPHASE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ATTACKSTEP> ATTACKSTEP { get; set; }
    }
}
