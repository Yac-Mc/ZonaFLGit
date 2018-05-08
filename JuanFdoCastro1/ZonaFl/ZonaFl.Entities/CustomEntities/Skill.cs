namespace ZonaFl.Persistence.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using BB.SmsQuiz.Infrastructure.Domain;
    using ZonaFl.Entities;

    public partial class Skill: EntityBase, IAggregateRoot
    {
        

        public bool Visible { get; set; }
        
    }
}
