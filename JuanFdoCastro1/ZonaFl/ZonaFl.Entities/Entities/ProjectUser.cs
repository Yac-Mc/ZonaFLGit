﻿using BB.SmsQuiz.Infrastructure.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ZonaFl.Persistence.Entities
{
    public class ProjectUser: EntityBase, IAggregateRoot
    {

        public int Id { get; set; }
        public int IdProject { get; set; }
        public string IdUser { get; set; }

        public virtual AspNetUsers AspNetUser { get; set; }
        public virtual Project Project { get; set; }
    }
}
