//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ZonaFl.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class Project
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Project()
        {
            this.ProjectUsers = new HashSet<ProjectUser>();
        }
    
        public int Id { get; set; }
        public int IdCategory { get; set; }
        public int IdOffer { get; set; }
        public string Comments { get; set; }
        public Nullable<short> Qualification { get; set; }
        public short Status { get; set; }
        public int Postulantes { get; set; }
    
        public virtual Category Category { get; set; }
        public virtual Offer Offer { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProjectUser> ProjectUsers { get; set; }
    }
}
