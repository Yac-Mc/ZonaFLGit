using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZonaFl.Models;
using Omu.ValueInjecter;

namespace ZonaFl.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //ViewBag.Title = "Home Page";
           // return Redirect(Url.Content("~/Static/index.html"));
            return View();
        }

        public JsonResult verifySessionUser()
        {
            bool validuser = false;
            RegisterBindingModel useru = new RegisterBindingModel();
            if (SessionBag.Current.User !=null)
            {
                useru.InjectFrom((object)SessionBag.Current.User);
            }
            if (useru.Email != null)
            {
                if (useru.Freelance)
                {
                    return Json(new { success = true, errors = "", tipo = useru, UserEmail = useru.Email, Url = Url.Content("/Offer/Index/" + useru.Id).ToString() });

                }
                else if (useru.Empresa)
                {
                    return Json(new { success = true, errors = "", tipo = useru, UserEmail = useru.Email, Url = Url.Content("/Projects/Index/" + useru.Id).ToString() });
                }
                else
                {

                    return Json(new { success = true, errors = "", tipo = useru, UserEmail = useru.Email, Url = Url.Content("/Administration/Users/Index?email=" + useru.Email).ToString() });
                }
            }
            else
            {
                return Json(new { success = true, errors = "", tipo = useru, UserEmail = "", Url = Url.Content("/Home/index").ToString() });

            }


            

        }
    }
}
