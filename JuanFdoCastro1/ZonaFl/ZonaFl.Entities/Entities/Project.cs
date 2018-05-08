using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZonaFl.Persistence.Entities
{
    public class Project
    {
        [Key]
        public int Id { get; set; }
        public int IdCategory { get; set; }
        public int IdOffer { get; set; }
        public string Comments { get; set; }
        public StatusProject Status { get; set; }
        public Nullable<short> Qualification { get; set; }
        public int Postulantes { get; set; }
        public virtual Offer Offer { get; set; }
        public virtual Category Category { get; set; }

    }

    public enum StatusProject
    {

        Publicada = 0,
        Aplicada = 1,
        EnCurso = 2,
        Finalizada = 3,
        Eliminada = 4

    }
}
