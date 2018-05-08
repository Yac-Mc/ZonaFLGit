using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZonaFl.Business.SubSystems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ZonaFl.Persistence.Entities;

namespace ZonaFl.Business.SubSystems.Tests
{
    [TestClass()]
    public class SNotificationTests
    {
        [TestMethod()]
        public void GetNotificationsByUserUserTest()
        {
            SNotification snoti = new SNotification();
          List<Log4Net_Error> notifications=  snoti.GetNotificationsByUserUser("ab580b7c-c15a-4e82-829f-91b91c1bed1a");


        }
    }
}