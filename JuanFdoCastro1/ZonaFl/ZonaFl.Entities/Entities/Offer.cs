

namespace ZonaFl.Persistence.Entities
{


    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using BB.SmsQuiz.Infrastructure.Domain;

    [Table("Offer")]
    public partial class Offer:  IAggregateRoot
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Offer()
        {
            this.OfferUsers = new HashSet<OfferUser>();
            this.Projects = new HashSet<Project>();
            this.OfferPhases = new HashSet<OfferPhases>();
        }

        [Key]
        public int Id { get; set; }
        public bool PrecioFijo { get; set; }
        public bool Sale { get; set; }
        public int ValueFixedProject { get; set; }
        public short NumberPhases { get; set; }
        public bool AutomaticValuePhases { get; set; }
        public string IdUser { get; set; }
        public int CategoryId { get; set; }
        public string TitleOffer { get; set; }
        public string Description { get; set; }
        public int RangoValor { get; set; }

        [Display(Name = "Status")]
        public StatusOffer Status { get; set; }


        public enum StatusOffer
        {
            SinIniciar = 0,
            EnCurso = 1,
            Finalizada = 2,
            Eliminada = 3,
        }



        public DateTime DateOffer { get; set; }

        public virtual AspNetUsers AspNetUser { get; set; }

       
        public virtual Category Category { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OfferUser> OfferUsers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Project> Projects { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OfferPhases> OfferPhases { get; set; }
        

    }
}
