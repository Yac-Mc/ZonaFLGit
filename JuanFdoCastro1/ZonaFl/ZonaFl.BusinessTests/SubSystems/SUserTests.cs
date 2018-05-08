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
    public class SUserTests
    {
        [TestMethod()]
        public void DeleteUserTest()
        {
            SUser suser = new SUser();
            suser.DeleteUser(Guid.Parse("727491a4-5c14-42cb-b0af-9f93456ecbaf"));
            Assert.Fail();
        }
    }
}