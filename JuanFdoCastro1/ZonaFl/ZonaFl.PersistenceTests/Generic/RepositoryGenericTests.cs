using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZonaFl.Persistence.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZonaFl.Entities;
using ZonaFl.Persistence.Repository;
using Omu.ValueInjecter;

namespace ZonaFl.Persistence.Generic.Tests
{
    [TestClass()]
    public class RepositoryGenericTests
    {
        [TestMethod()]
        public void GetListTest()
        {
            ZonaFl.Persistence.Repository.UserRepository2<AspNetUser> repo = new UserRepository2<AspNetUser>();
            var list = repo.GetList().ToList();
          var list2=  list.Select(e => new ZonaFl.Entities.AspNetUser().InjectFrom(e)).Cast<AspNetUser>().ToList();
        }

        [TestMethod()]
        public void GetListTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetListTest2()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void InsertTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void UpdateTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DeleteTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetListPagedTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void RecordCountTest()
        {
            Assert.Fail();
        }
    }
}