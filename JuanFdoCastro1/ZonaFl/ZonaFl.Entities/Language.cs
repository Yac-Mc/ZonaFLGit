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
    
    public partial class Language
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LevelLang { get; set; }
        public Nullable<bool> Certificate { get; set; }
        public string UserId { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
    }
}
