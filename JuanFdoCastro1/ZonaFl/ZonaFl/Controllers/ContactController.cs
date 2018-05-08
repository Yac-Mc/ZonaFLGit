using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

using ZonaFl.Models;

namespace ZonaFl.Controllers
{ 

    [Authorize]
[RoutePrefix("api/Contact")]
public class ContactController : BaseApiController
    {


        [AllowAnonymous]
        [Route("SendContact")]
        [HttpPost]
        public async Task<IHttpActionResult> SendContact(Contact model ) { 
            const string emailregex = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
            var result = false;


            //ViewData["preventspam"] = preventspam;

            //if (string.IsNullOrEmpty(namecontact))
            //    ViewData.ModelState.AddModelError("name", "Please enter your name!");
            //if (string.IsNullOrEmpty(emailcontact))
            //    ViewData.ModelState.AddModelError("email", "Please enter your e-mail!");
            //if (!string.IsNullOrEmpty(emailcontact) && !Regex.IsMatch(emailcontact, emailregex))
            //    ViewData.ModelState.AddModelError("email", "Please enter a valid e-mail!");
            //if (string.IsNullOrEmpty(mensajecontact))
            //    ViewData.ModelState.AddModelError("comments", "Please enter a message!");
            ////if (string.IsNullOrEmpty(preventspam))
            ////    ViewData.ModelState.AddModelError("preventspam", "Please enter the total");
            //if (!ViewData.ModelState.IsValid)
            //    return View();

            //if (HttpContext.Session["random"] != null &&
            //  preventspam == HttpContext.Session["random"].ToString())
            //{
            var message = new MailMessage(model.Emailcontact, "contact@zonafl.com")
            {
                Subject = model.Asuntocontact + " " + model.Namecontact,
                IsBodyHtml = false,
                Body = "Mensaje: "+model.Mensajecontact + Environment.NewLine + "Ciudad:" + model.Citycontact + Environment.NewLine + "Telefono:" + model.Numbercontact
                };

            var client = new SmtpClient("mail.zonafl.com")
            {
                Credentials = new System.Net.NetworkCredential("contact@zonafl.com", "zonafl12345*"),
                
                EnableSsl = false,
                
            };
            try
                {
                    client.Send(message);
                    result = true;
                }
                catch (Exception e)    {

                result = false;
                }
            //}
            //if (Request.IsAjaxRequest())
            //{
            //    return Content(result.ToString());
            //}
            return Json(new { success = result });
        }

     
    }
}
