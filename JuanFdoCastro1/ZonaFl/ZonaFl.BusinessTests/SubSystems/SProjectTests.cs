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
    public class SProjectTests
    {
        [TestMethod()]
        public void GetCalificationAverageUserTest()
        {
            SProject spro = new SProject();
           var calif= spro.GetCalificationAverageUser("2bb615d9-0183-4f91-acf6-c06738a2ad53");
            Assert.Fail();
        }
    }
}