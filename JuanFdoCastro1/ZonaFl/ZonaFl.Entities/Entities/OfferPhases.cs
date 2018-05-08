using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZonaFl.Entities.CustomEntities;

namespace ZonaFl.Persistence.Entities
{
    public partial class OfferPhases
    {
        public int Id { get; set; }
        public int IdOffer { get; set; }
        public string Name { get; set; }
        public DateTime InitPhase { get; set; }
        public DateTime FinishPhase { get; set; }
        
        public decimal Value { get; set; }
        [Editable(false, AllowInitialValue = false)]
        public string Value2 { get; set; }
        public StatusPhase StatusPhase { get; set; }

        public virtual Offer Offer { get; set; }
        public Pagos pagos { get; set; }
        [Editable(false, AllowInitialValue = false)]
        public string Signature  { get; set; }
        [Editable(false, AllowInitialValue = false)]
        public string buyerEmail { get; set; }

     


    }

    public enum StatusPhase
    {
        SinIniciar = 0,
        EnCurso = 1,
        Finalizada = 2,
        Eliminada = 3,
    }
}
