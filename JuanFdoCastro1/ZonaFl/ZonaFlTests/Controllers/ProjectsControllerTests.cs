using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZonaFl.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using Moq;
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;

namespace ZonaFl.Controllers.Tests
{
    [TestClass()]
    public class ProjectsControllerTests
    {
        [TestMethod()]
        public void DetailsPayTest()
        {
                       var request = new Mock<HttpRequestBase>();
            var requestParams = new NameValueCollection
            {

                { "merchantId", "123456789101"},
                { "referenceCode", "2418"},
                { "currency" ,"4"},

            };

            
            request.SetupGet(r => r["merchantId"]).Returns("123456789101");
            request.SetupGet(r => r["referenceCode"]).Returns("2418");
            request.SetupGet(r => r["currency"]).Returns("COP");
            request.SetupGet(r => r["transactionState"]).Returns("4");

            request.SetupGet(x => x.Params).Returns(requestParams);

            //            ProjectsController

            //HTTP CONTEXT SET UP
            //var httpContext = new Mock<HttpContextBase>();

            //var routeData = new RouteData();
            //httpContext.Setup(c => c.Request.RequestContext.RouteData).Returns(routeData);

            //httpContext.Setup(c => c.Request.Form).Returns(delegate ()
            //{
            //    var nv = new NameValueCollection();
            //    nv.Add("FirstName", "John");
            //    nv.Add("LastName", "Smith");
            //    nv.Add("Email", "jsmith@host.com");
            //    nv.Add("Comments", "Comments are here...");
            //    nv.Add("ReceiveUpdates", "true");
            //    return nv;
            //});

            //httpContext.Setup(c => c.Request.Path).Returns("/projects/DetailsPay");

            //var subscriptionViewModel = new Mock<ISubscriptionViewModel>();

            //subscriptionViewModel.Setup(h => h.HttpContext).Returns(httpContext.Object);

            //subscriptionViewModel.Setup(h => h.FirstName).Returns(httpContext.Object.Request.Form["FirstName"]);
            //subscriptionViewModel.Setup(h => h.LastName).Returns(httpContext.Object.Request.Form["LastName"]);
            //subscriptionViewModel.Setup(h => h.Email).Returns(httpContext.Object.Request.Form["Email"]);
            //subscriptionViewModel.Setup(h => h.Comments).Returns(httpContext.Object.Request.Form["Comments"]);
            //subscriptionViewModel.Setup(h => h.InvestmentUpdates).Returns(bool.Parse(httpContext.Object.Request.Form["ReceiveUpdates"]));
            //ProjectsController target = new ProjectsController();

            //target.ControllerContext =
            //    new ControllerContext(httpContext.Object, new RouteData(), target);
            var context = new Mock<System.Web.HttpContextBase>();

            context.SetupGet(x => x.Request).Returns(request.Object);

            ProjectsController target = new ProjectsController();

            target.ControllerContext =
                new ControllerContext(context.Object, new RouteData(), target);
            target.DetailsPay();

            ContratanteController target2 = new ContratanteController();
            target2.StartPhase(2418);
            //var projectsController = new ProjectsController();

            //var result = projectsController.DetailsPay();

            //Assert.AreEqual(((ViewResult)result).ViewName, "Index");

        }
    }
}