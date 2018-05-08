using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ZonaFl.Models;
using ZonaFl.Business.SubSystems;
using Omu.ValueInjecter;
using static ZonaFl.Models.RegisterBindingModel;
using System.Threading.Tasks;
using ZonaFl.Persistence.Entities;

namespace ZonaFl.Controllers
{
    [Authorize]
    [RoutePrefix("api/NotificationsR")]
    public class NotificationsRController : ApiController
    {
        //to retrieve all the users by offer
        [AllowAnonymous]
        [Route("GetNotificationsByUser")]
        public List<Models.Log4Net_Error> GetNotificationsByUser(string userid)
        {
            List<Models.Log4Net_Error> notificatioosnlist = new List<Models.Log4Net_Error>();
            SNotification snoti = new SNotification();
           var notifications=snoti.GetNotificationsByUserUser(userid);
            
           
            if (notifications.Count>0)
            {
                notificatioosnlist = notifications.Select(e => new Models.Log4Net_Error().InjectFrom(e)).Cast<Models.Log4Net_Error>().ToList();
                notificatioosnlist.Where(e => e.ImageUser == null).ToList().ForEach(e => e.ImageUser = "/Static/images/user_default.png");
            }

            return notificatioosnlist;

        }


      
       
    }
}
