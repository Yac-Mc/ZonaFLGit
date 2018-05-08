namespace ZonaFl.Persistence.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using BB.SmsQuiz.Infrastructure.Domain;
    using ZonaFl.Entities;

    public partial class AspNetUsers: EntityBase, IAggregateRoot
    {
       

        public string Rol { get; set; }
        public string Accion { get; set; }
        public string Estilo { get; set; }
        public string TipoUser { get; set; }
        public string Url { get; set; }
        public bool PagosConfirmed { get; set; }
    }
}
