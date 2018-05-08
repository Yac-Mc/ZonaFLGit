using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Web.Mvc;
using ZonaFl.Persistence.Entities;
using System.Runtime.Serialization;


namespace ZonaFl.Models
{
    [KnownType(typeof(OfferUser))]
    public class Offer : ModelBase
    {
        public bool Applicada { get; set; }

        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nombre Contratante")]
        public string NameContractor { get; set; }

        [Required]
        [Display(Name = "Precio Fijo")]
        public bool PrecioFijo { get; set; }

        [Required]
        [Display(Name = "Subasta")]
        public bool Sale { get; set; }

        [Required]
        [Display(Name = "Valor Fijo Proyecto")]
        public int ValueFixedProject { get; set; }

        [Required]
        [Display(Name = "Numero de fases")]
        public Int16 NumberPhases { get; set; }

        [Required]
        [Display(Name = "Valor Automatico de fases")]
        public bool AutomaticValuePhases { get; set; }


        [Required]
        [Display(Name = "Nombre de fase")]
        public string NamePhase { get; set; }

        [Required]
        [Display(Name = "Fecha de inicio fase")]
        public string DateIni { get; set; }

        [Required]
        [Display(Name = "Fecha de cierre fase")]
        public string DateClose { get; set; }


        [Required]
        [Display(Name = "Valor fijo fase ")]
        public int ValuePhase { get; set; }

        [Required]
        [Display(Name = "Valor fijo fase ")]
        public decimal PercentValueFase { get; set; }

        public Category Category { get; set; }

        public List<OfferPhase> OfferPhases { get; set; }

        public string Phase1Name { get; set; }


        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateIniPhase1 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateEndPhase1 { get; set; }
        public decimal ValuePhase1 { get; set; }
        public decimal  PercentValuePhase1 { get; set; }

        public string Phase2Name { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateIniPhase2 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateEndPhase2 { get; set; }
        public decimal ValuePhase2 { get; set; }
        public decimal PercentValuePhase2 { get; set; }

        public string Phase3Name { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateIniPhase3 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateEndPhase3 { get; set; }
        public decimal ValuePhase3 { get; set; }
        public decimal PercentValuePhase3 { get; set; }


        public string Phase4Name { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateIniPhase4 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateEndPhase4 { get; set; }
        public decimal ValuePhase4 { get; set; }
        public decimal PercentValuePhase4 { get; set; }
        public int RangoValor { get; set; }
        public List<OfferUser> OfferUsers { get; set; }
        public string TitleOffer { get; set; }
        public string Description { get; set; }
        public DateTime DateOffer { get; set; }
        public string  ContractorCountry { get; set; }
        public string ContractorCity { get; set; }
        public int NoPostulados { get; set; }

        public string Comments { get; set; }
        public Nullable<short> Qualification { get; set; }
        public bool IsForFinally { get; set; }

        public List<Persistence.Entities.Category> Categories { get; internal set; }
        //public short Status { get; set; }
        public DateTime InicioEst { get;  set; }
        public DateTime FinEst { get;  set; }

        [Display(Name = "Status")]
        public StatusOffer Status { get; set; }


        public enum StatusOffer
        {
            SinIniciar = 0,
            EnCurso = 1,
            Finalizada = 2,
            Eliminada = 3,
        }

    }

    
}