using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using log4net;
using log4net.Core;
using log4net.Repository;
using ZonaFl.Business.SubSystems;

namespace ZonaFl.Business
{

    public class Log4NetLogger : ILogger
    {

        public string CurrentUser {get; set;}
 
 private ILog _logger;
        private IlogCustom _loggerC;
 public Log4NetLogger()
 {
 _logger = LogManager.GetLogger(this.GetType());
            
        }
 
 public void Info(string message)
 {
 _logger.Info(message);

         
 }
 
 public void Warn(string message)
 {
 _logger.Warn(message);
 }
 
 public void Debug(string message)
 {
 _logger.Debug(message);
 }
 
 public void Error(string message)
 {
 _logger.Error(message);
 }
 
 public void Error(Exception x) 
 {
            if (string.IsNullOrEmpty(this.CurrentUser))
            {
                Error(LogUtility.BuildExceptionMessage(x), x);
            }
            else
            {
                Error(LogUtility.BuildExceptionMessage(x,CurrentUser), x);
            }
           
            // throw new NotImplementedException();
        }

 public void Error(Exception x, string Path, string RawUrl)
   {
            if (string.IsNullOrEmpty(this.CurrentUser))
            {
                Error(LogUtility.BuildExceptionMessage(x, Path, RawUrl), x);
            }
            else
            {
                Error(LogUtility.BuildExceptionMessage(x, Path, RawUrl, CurrentUser), x);
            }
           
            // throw new NotImplementedException();
   }

        public void Error(string message, Exception x)
 {
 _logger.Error(message, x);
            var smail = SMail.Instance;
            smail.Send("info@zonafl.com", "juanfercas2002@gmail.com", "Error en Zonafl", message);
            // throw new NotImplementedException();
        }

        public void Fatal(string message)
 {
 _logger.Fatal(message);
 }
 
 public void Fatal(Exception x)
 {
     throw new NotImplementedException();
 //Fatal(LogUtility.BuildExceptionMessage(x));
 }

           public void Log(Type callerStackBoundaryDeclaringType, Level level, object message, Exception exception)
           {
           
           

               throw new NotImplementedException();
           }

           public void Log(LoggingEvent logEvent)
           {
               throw new NotImplementedException();
           }

           public bool IsEnabledFor(Level level)
           {
               throw new NotImplementedException();
           }

           public string Name { get; private set; }
           public ILoggerRepository Repository { get; private set; }
 }

   interface IlogCustom:ILog
    {
        

        void Log(string message, string Entidad);

    }
}
