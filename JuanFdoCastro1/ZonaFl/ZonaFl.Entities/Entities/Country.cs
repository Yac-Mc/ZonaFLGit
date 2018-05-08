

namespace ZonaFl.Persistence.Entities
{
  
        using System;
        using System.Collections.Generic;
        using System.ComponentModel.DataAnnotations;
        using System.ComponentModel.DataAnnotations.Schema;
        using System.Data.Entity.Spatial;
        using BB.SmsQuiz.Infrastructure.Domain;
    [Table("CountryMaster")]
    public partial class Country : EntityBase, IAggregateRoot
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Country()
        {
            States = new HashSet<State>();
        }

        public int Id { get; set; }
        public bool Selected { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(5)]
        public string CountryCode { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<State> States { get; set; }



    }

    public partial class State : EntityBase, IAggregateRoot
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public State()
        {
            Cities = new HashSet<City>();
        }

        public int Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(5)]
        public int CountryID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<City> Cities { get; set; }



    }

    public partial class City : EntityBase, IAggregateRoot
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public City()
        {

        }

        public int Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(5)]
        public int CountryCode { get; set; }

       



    }
}
