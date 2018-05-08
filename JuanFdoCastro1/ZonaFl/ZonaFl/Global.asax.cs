using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ZonaFl.Models;

namespace ZonaFl
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Database.SetInitializer<ApplicationDbContext>(null);
            log4net.Config.XmlConfigurator.Configure();
        }

        protected void Application_BeginRequest()
        {

            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            //Response.Cache.SetNoStore();
        }


        //internal protected  void  Application_BeginRequest(object sender, EventArgs e)
        //{
        //    System.Globalization.CultureInfo colombianSpanishCi = System.Globalization.CultureInfo.GetCultureInfo("es-CO");
        //    try
        //    {
        //        System.Threading.Thread.CurrentThread.CurrentCulture = colombianSpanishCi;
        //        System.Threading.Thread.CurrentThread.CurrentUICulture = colombianSpanishCi;
        //    }
        //    catch (Exception er)
        //    {

        //    }
        //}
    }
}
