using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ZonaFl.Controllers.Filters
{
    public class IsLogout: ActionFilterAttribute

    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (SessionBag.Current.Logout != null && SessionBag.Current.Logout)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                    { "controller", "Home" },
                    { "action", "Index" },
                   {"area","" }
                    });
            }

           
        }

    }
}