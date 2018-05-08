using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZonaFl.Models
{
    public class Log4Net_Error
    {
        public int Id { get; set; }
        public System.DateTime Date { get; set; }
        public string Thread { get; set; }
        public string Level { get; set; }
        public string Logger { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
        public string Entidad { get; set; }
        public string ImageUser { get; set; }

    }
}