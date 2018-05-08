using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZonaFl.Business.SubSystems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZonaFl.Business.SubSystems.Tests
{
    [TestClass()]
    public class SMailTests
    {
        [TestMethod()]
        public void SendTest()
        {
            var smail = SMail.Instance;
            string Url = "www.zonafl.com";
         try
            {
                smail.Send("contact@zonafl.com", "juanfercas2002@gmail.com", "Usuario Aplicó proyecto", "El usuario:" + "jua fdo castro" + " ha aplicado al proyecto " + "Proyecto prueba" + " favor ingresar al sitio web < a href =\"" + Url + "\">Aquí</a>);");
            }
            catch(Exception er)
            {
                Assert.Fail(er.Message);

            }
           
            
        }
    }
}