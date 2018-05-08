using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZonaFl.Models
{
    public class ProjectUser
    {

        public int Id { get; set; }
        public int IdProject { get; set; }
        public string IdUser { get; set; }

        public virtual RegisterBindingModel AspNetUser { get; set; }
        public virtual Project Project { get; set; }
    }
}
