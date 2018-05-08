using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZonaFl.Business.SubSystems;
using ZonaFl.Persistence.Entities;
using ZonaFl.Models;
using ZonaFl.Controllers.Filters;

namespace ZonaFl.Areas.Administration.Controllers
{
    public class UsersController : Controller
    {
        // GET: Administration/Users
        [IsLogout]
        public ActionResult Index(string email)
        {

            //if (SessionBag.Current.Logout == true && SessionBag.Current.Logout!=null)
            //    return Redirect(Url.Content("~/Static/index.html"));

            if (!string.IsNullOrEmpty(email))
                {
                SUser suser = new SUser();
                var user = suser.GetUserByEmail(email);
                SessionBag.Current.User = user;
                ViewBag.IdUser = user.Id;
                if (user != null)
                {
                    return View(user);

                }
                else
                {
                    return RedirectToAction("Logout", "Freelance", new { area = "" });
                    /*return Redirect(Url.Content("~/Freelance/Logout"))*/;
                }

            }
            else
            {
                return RedirectToAction("Logout", "Freelance", new { area = "" });
                //return Redirect(Url.Content("~/Static/index.html"));
            }

            
           
        }

        public ActionResult Logout()
        {
            SessionBag.Current.User = null;
            SessionBag.Current.Logout = true;
            //Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.Cache.SetNoStore();

            //Session.Abandon();

            //Response.Buffer = true;
            //Response.ExpiresAbsolute = DateTime.Now.AddDays(-1d);
            //Response.Expires = -1500;
            //Response.CacheControl = "no-cache";


            return RedirectToAction("Index", "Home", new { area="" });
        }


        // GET: Administration/Users/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Administration/Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Administration/Users/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Administration/Users/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Administration/Users/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Administration/Users/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Administration/Users/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
