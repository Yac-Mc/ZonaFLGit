
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Net.Http;
using Microsoft.AspNet.Identity.Owin;
using ZonaFl.Models;
using System.Web.Http.Results;
using System.Web.Http.Controllers;
using System.Threading;
using ZonaFl.Controllers.Filters;

namespace ZonaFl.Controllers
{
   
    public class BaseApiController : ApiController
    {

        private ModelFactory _modelFactory;
        private ApplicationUserManager _AppUserManager = null;
        private ApplicationRoleManager _AppRoleManager = null;
        private ApplicationUser _member;
        protected ApplicationUserManager AppUserManager
        {
            get
            {
                return _AppUserManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        protected ApplicationRoleManager AppRoleManager
        {
            get
            {
                return _AppRoleManager ?? Request.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
        }

       

        public BaseApiController()
        {

        }

        protected ModelFactory TheModelFactory
        {
            get
            {
                if (_modelFactory == null)
                {
                    _modelFactory = new ModelFactory(this.Request, this.AppUserManager);
                }
                return _modelFactory;
            }
        }


        //protected  override  void Initialize(HttpControllerContext controllerContext)
        //{
        //    System.Globalization.CultureInfo colombianSpanishCi = System.Globalization.CultureInfo.GetCultureInfo("es-CO");
        //    try
        //    {
        //        System.Threading.Thread.CurrentThread.CurrentCulture = colombianSpanishCi;
        //        System.Threading.Thread.CurrentThread.CurrentUICulture = colombianSpanishCi;
        //    }
        //    catch(Exception er)
        //    {

        //    }

        //}

        
        protected IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }



        //public string GetMessage(InvalidModelStateResult mensaje)
        //{

        //    string message = "";
        //for (int i=0; i< mensaje.ModelState.Keys.Count; i++)
        //    {
              
        //    }
        //    return message;
        //}
    }
}