using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZonaFl.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZonaFl.Controllers.Tests
{
    [TestClass()]
    public class ContratanteControllerTests
    {
        [TestMethod()]
        public void StartPhaseTest()
        {

            ContratanteController contr = new ContratanteController();
                contr.StartPhase(2419);
            Assert.Fail();
        }
    }
}