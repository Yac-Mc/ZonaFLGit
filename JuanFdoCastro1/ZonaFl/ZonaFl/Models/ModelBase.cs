using BB.SmsQuiz.Infrastructure.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZonaFl.Models
{
   public abstract class ModelBase
    {

        public bool IsValid { get; set; }
        public ValidationErrors ValidationErrors { get; set; }
        public string MessageError { get; set; }
    }
}
