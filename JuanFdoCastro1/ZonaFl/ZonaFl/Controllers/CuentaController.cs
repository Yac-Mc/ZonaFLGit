using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using ZonaFl.Models;
using ZonaFl.Providers;
using ZonaFl.Results;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http.Results;
using Omu.ValueInjecter;
using ZonaFl.Persistence.Entities;
using ZonaFl.Services;
using System.Net.Mail;
using System.Net.Mime;
using Microsoft.Owin.Testing;
using System.Web.Script.Serialization;
using ZonaFl.Business.SubSystems;
using System.Web.Mvc;
using ZonaFl.Controllers.Filters;

namespace ZonaFl.Controllers
{

    [NoCache]
    public class CuentaController : Controller
    {
        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;
        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }


        public CuentaController()
        {
        }

        public CuentaController(ApplicationUserManager userManager,
          ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // GET: Cuenta
        public ActionResult Index()
        {
            return View();
        }

       [CustomExceptionFilter]
        public ActionResult LoginUser(RegisterBindingModel model)
        {


            var user = new ApplicationUser() { UserName = model.UserName, Email = model.Email, Empresa = model.Empresa, Freelance = model.Freelance };


            AspNetUsers aspuser = new AspNetUsers();
            try
            {


                ZonaFl.Business.SubSystems.SUser usern = new Business.SubSystems.SUser();
                ApplicationUser useru = null;
                if (user.UserName.Contains("@"))
                {
                    useru = UserManager.FindByEmail(user.UserName);
                }
                else
                {

                    useru = UserManager.FindByName(user.UserName);
                }

                if(useru!=null)
                {
                    if (useru.EmailConfirmed)
                    {

                        bool validuser = false;
                        if (useru != null)
                        {
                            validuser = UserManager.CheckPassword(useru, model.PasswordHash);
                            if (validuser)
                            {
                                var identity = new System.Security.Principal.GenericIdentity(useru.UserName);
                                //SetPrincipal(new System.Security.Principal.GenericPrincipal(identity, null));
                            }
                            else
                            {

                                return Json(new { success = false, issue = user, errors = "Contraseña o usuario incorrecto, favor volver a ingresar los datos", tipo = user, UserEmail = aspuser.Email });
                            }

                            //var useri = HttpContext.Current.User;
                            //HttpContext.Current.User = useri;

                            //RegisterBindingModel regbm = new RegisterBindingModel();
                            //regbm.InjectFrom(useru);
                            //HttpSessionStateBase session = new HttpSessionStateWrapper(HttpContext.Current.Session);
                            //SessionBag.Current.User = regbm;

                        }

                        if (useru.Freelance)
                        {
                            //return RedirectToAction("Index", "Offer", new { id = useru.Id });
                            return Json(new { success = validuser, issue = useru, errors = "", tipo = useru, UserEmail = useru.Email, Url = Url.Content("/Offer/Index/" + useru.Id).ToString() });

                        }
                        else if (useru.Empresa)
                        {
                            return Json(new { success = validuser, issue = useru, errors = "", tipo = useru, UserEmail = useru.Email, Url = Url.Content("/Projects/Index/" + useru.Id).ToString() });
                        }
                        else
                        {

                            return Json(new { success = validuser, issue = useru, errors = "", tipo = useru, UserEmail = useru.Email, Url = Url.Content("/Administration/Users/Index?email=" + useru.Email).ToString() });
                        }
                   
                }
                else
                {
                    return Json(new { success = false, issue = user, errors = "Usuario dado de baja o correo electrónico no confirmado, favor comunicarse con el administrador del sistema", tipo = user, UserEmail = aspuser.Email });
                }
                }
                else
                {
                    return Json(new { success = false, issue = user, errors = "Usuario no encontrado, favor comunicarse con el administrador del sistema", tipo = user, UserEmail = aspuser.Email });
                }
            }

            catch (Exception ex)
            {

                //throw new Exception(ex.Message);
                return Json(new { success = false, issue = user, errors = ex.Message, tipo = user, UserEmail = aspuser.Email });

            }


            return Json(new { success = true, issue = model, errors = "", tipo = model, UserEmail = user.Email, Url = Url.Content("/Home/index").ToString() });
        }

        //private static void SetPrincipal(System.Security.Principal.IPrincipal principal)
        //{
        //    System.Threading.Thread.CurrentPrincipal = principal;
        //    if (HttpContext.Current != null)
        //    {
        //        HttpContext.Current.User = principal;
        //    }
        //}
    }
}