using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZonaFl.Persistence.Entities;
using ZonaFl.Entities.CustomEntities;
using System.ComponentModel.DataAnnotations;

namespace ZonaFl.Models
{
    public class OfferPhase
    {

        public int Id { get; set; }
        public int IdOffer { get; set; }
        public string Name { get; set; }
        public System.DateTime InitPhase { get; set; }
        public System.DateTime FinishPhase { get; set; }
        [DisplayFormat(DataFormatString ="{0:N}",ApplyFormatInEditMode =true)]
        public decimal Value { get; set; }
        public Persistence.Entities.StatusPhase StatusPhase { get; set; }
        public virtual Offer Offer { get; set; }
        public Pagos pagos { get; set; }
        public string Signature { get; set; }
    }

    
    public enum StatusPhase
    {
        SinIniciar=0,
        EnCurso=1,
        Finalizada=2,


       

    }
}
