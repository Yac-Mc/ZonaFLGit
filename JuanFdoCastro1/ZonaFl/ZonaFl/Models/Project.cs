using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Web.Mvc;
using ZonaFl.Persistence.Entities;

namespace ZonaFl.Models
{
    public class Project
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Oferta")]
        public Offer Offer { get; set; }

       

        public  int IdOffer { get; set; }



        [Display(Name = "Comentarios")]
        public string Comments { get; set; }
        public string UserName { get; set; }


        [Display(Name = "Calificación proyecto")]
        public Int16 Qualification { get; set; }

        [Display(Name = "Status")]
        public StatusPhase Status { get; set; }

        [Display(Name = "No Postulados")]
        public int Postulados { get; set; }
        public  Category Category { get; set; }


        public DateTime Inicio { get; set; }
        public DateTime Fin { get; set; }
        public decimal Avance { get; set; }
        public ICollection<Models.OfferPhase> Phases { get; set; }
        public string Image { get; set; }
    }

    public enum StatusProject
    {
        SinIniciar = 0,
        EnCurso = 1,
        Finalizada = 2,
        Eliminada = 3,
    }


}