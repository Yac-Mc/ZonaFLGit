using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZonaFl.Persistence;

using ZonaFl.Persistence.Repository;
using Omu.ValueInjecter;
using ZonaFl.Persistence.Entities;
namespace ZonaFl.Business.SubSystems
{
   public class SNotification
    {

        public List<Log4Net_Error> GetNotificationsByUserUser(string idUser)
        {
            NotificationRepository<Log4Net_Error> notirepo = new NotificationRepository<Log4Net_Error>();
            SUser suser = new SUser();
            

           var user= suser.GetUserById(new Guid(idUser));
            var categories = user.Skills.Where(e=>e.Visible).Select(e => e.Name).Distinct();
            StringBuilder sb = new StringBuilder();
            if (user.Empresa)
            {
                sb.Append("where ( [Message] like 'Postulación Oferta%')");
            }
            else
            {
                sb.Append("where ( [Message] like 'Postulación Oferta%' Or [Message] like 'Proyecto finalizado%')");
            }
            foreach(var cat in categories)
            {
                sb.Append("Or [Message] like '%Categoria:" + cat+"'");
            }
          return  notirepo.GetList(sb.ToString()).ToList();
        }
        public List<Log4Net_Error> GetNotificationsAdministrator(string idUser)
        {
            NotificationRepository<Log4Net_Error> notirepo = new NotificationRepository<Log4Net_Error>();
            SUser suser = new SUser();

            var user = suser.GetUserById(new Guid(idUser));
            var categories = user.Skills.Where(e => e.Visible).Select(e => e.Name).Distinct();
            StringBuilder sb = new StringBuilder();
            sb.Append("where ([Message] like 'Creación Oferta%' Or [Message] like 'Creación Proyecto%' OR [Message] like 'Registro Usuario%' Or [Message] like 'Proyecto finalizado%' ) ");
            foreach (var cat in categories)
            {
                sb.Append("Or [Message] like '%Categoria:" + cat + "'");
            }
            return notirepo.GetList(sb.ToString()).ToList();
        }

    }
}
