//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ZonaFl.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public partial class Certification
    {
      
        public int Id { get; set; }
        public string Certificate { get; set; }
        public string Otorgante { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public string DateCert { get; set; }
        public bool Actually { get; set; }
    
        public virtual RegisterBindingModel AspNetUser { get; set; }
        public string Certificate2 { get; set; }
        public string Otorgante2 { get; set; }
        public string Description2 { get; set; }
        public string UserId2 { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public string DateCert2 { get; set; }
        public bool Actually2 { get; set; }

        public string Certificate3 { get; set; }
        public string Otorgante3 { get; set; }
        public string Description3 { get; set; }
        public string UserId3 { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public string DateCert3 { get; set; }
        public bool Actually3 { get; set; }
    }
}
