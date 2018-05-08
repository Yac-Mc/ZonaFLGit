using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZonaFl.Models
{
    public class Skill
    {
       


        public int Id { get; set; }
        public string Name { get; set; }
        public string CategorySkill { get; set; }
        public int CategoryId { get; set; }
        public bool Visible { get; set; }
        public string IdHtml { get; set; }

    }
}
