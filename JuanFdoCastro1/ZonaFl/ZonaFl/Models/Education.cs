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

    public partial class Education
    {
        public int Id { get; set; }
        public string Institution { get; set; }
        public string Country { get; set; }
        public string Title { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public string DateIniE { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public string DateEndE { get; set; }
        public bool Actually { get; set; }
        public string UserId { get; set; }
    
        public virtual RegisterBindingModel AspNetUser { get; set; }
        public string Institution2 { get;  set; }
        public string Country2 { get;  set; }
        public string Title2 { get;  set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public string DateIni2E { get;  set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public string DateEnd2E { get;  set; }
        public bool Actually2 { get;  set; }
        public string Institution3 { get; set; }
        public string Country3 { get; set; }
        public string Title3 { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public string DateIni3E { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public string DateEnd3E { get; set; }
        public bool Actually3 { get; set; }
    }
}
