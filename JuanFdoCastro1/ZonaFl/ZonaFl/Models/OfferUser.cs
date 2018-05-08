using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ZonaFl.Models
{
    [KnownType(typeof(Offer))]
    public class OfferUser 
    {

        public int Id { get; set; }
        public int IdOffer { get; set; }
        public string IdUser { get; set; }
        public string NameUser { get; set; }

        public virtual RegisterBindingModel AspNetUser { get; set; }
        public virtual Offer Offer { get; set; }
        public string Url { get; set; }
    }
}
