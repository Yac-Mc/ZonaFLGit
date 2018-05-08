using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet;
using System.Net.Mail;


namespace ZonaFl.Business.SubSystems
{
    public class SMail : SmtpClient
    {
        private static SmtpClient instance;
        public static new int Port { get; set; }
        public static new string Host { get; set; }

        private  SMail() {
           

        }
        public static SmtpClient Instance
        {
            get
            {
                if (instance == null)
                {
                   
                    instance = new SmtpClient("mail.zonafl.com");
                    //instance.Host = ;
                    //instance.Port = 587;
                    instance.Credentials = new System.Net.NetworkCredential("contact@zonafl.com", "zonafl12345*");
                    //instance.EnableSsl = true;
                   
                }
                return instance;
            }
        }

        public void Send(string from, string recipients, string subject, string body)
        {
            
            MailMessage mail = new MailMessage(from, recipients, subject, body);
            mail.IsBodyHtml = true;
            instance.Send(mail);

        }


    }
}
