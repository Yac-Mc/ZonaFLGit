//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ZonaFl.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class Experience
    {
        public int Id { get; set; }
        public string Company { get; set; }
        public System.DateTime DateIni { get; set; }
        public System.DateTime DateEnd { get; set; }
        public bool CurrentEmploy { get; set; }
        public string Position { get; set; }
        public string FunctionPosition { get; set; }
        public string UserId { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
    }
}
