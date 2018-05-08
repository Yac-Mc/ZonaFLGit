using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ZonaFl
{
    public class RouteConfig
    {
       

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("Static/{*path}");
            routes.IgnoreRoute("localhost/");
            routes.IgnoreRoute("UploadedFiles/");
           

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );


            routes.MapRoute(
    "DetailsByName",
    "Profile/{userId}",
    new { controller = "Profile", action = "DetailsByName" },
    new { productId = @"\w+" }
 );

            routes.MapRoute(
  "EditById",
  "Profile/{userId}",
  new { controller = "Profile", action = "EditById" },
  new { productId = @"\w+" }
);


            routes.MapRoute(
"FindCitiesByCountry",
"Country/{countryname}",
new { controller = "Country", action = "FindCitiesByCountry" },
new { countryname = @"\w+" }
);


           

            routes.MapRoute(
                    name: "Get Cities",
                    url: "api/city",
                    defaults: new { controller = "City", action = "Index", id = UrlParameter.Optional }
                );

            //routes.MapPageRoute("Indexhtml", "IndexlUrl", "~/Static/index.html");
            //routes.MapPageRoute("ContratantePrincipalUrl", "ContratantePrincipalUrl", "~/Static/contratante-principal.html");
        }
    }
}
