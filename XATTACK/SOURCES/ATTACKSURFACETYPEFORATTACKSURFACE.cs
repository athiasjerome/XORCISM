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
    
    public partial class ATTACKSURFACETYPEFORATTACKSURFACE
    {
        public int AttackSurfaceTypesID { get; set; }
        public int AttackSurfaceTypeID { get; set; }
        public int AttackSurfaceID { get; set; }
        public Nullable<System.DateTimeOffset> CreatedDate { get; set; }
        public Nullable<System.DateTimeOffset> timestamp { get; set; }
        public Nullable<int> VocabularyID { get; set; }
    
        public virtual ATTACKSURFACE ATTACKSURFACE { get; set; }
        public virtual ATTACKSURFACETYPE ATTACKSURFACETYPE { get; set; }
    }
}
