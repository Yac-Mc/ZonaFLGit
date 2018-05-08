using System.Web.Http.Filters;
using System.Net.Http;
using System.Net;
using ZonaFl.Business;

namespace ZonaFl.Controllers.Filters
{
    public class CustomExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Response == null)
            {
                context.Response = new HttpResponseMessage();
            }
            context.Response.StatusCode = HttpStatusCode.NotImplemented;
            context.Response.Content = new StringContent("Error en la ejecución favor comunicarse con el administrador del sistema");
            Log4NetLogger logger2 = new Log4NetLogger();
            logger2.CurrentUser = SessionBag.Current.User.Id;
            logger2.Error(context.Exception);
           

            base.OnException(context);
        }

    }
}