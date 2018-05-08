using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Omu.ValueInjecter;
using ZonaFl.Models;
using ZonaFl.Controllers.Filters;

namespace ZonaFl.Controllers
{
    public class PortFolioController : Controller
    {
        // GET: PortFolio
        public ActionResult Index()
        {
            return View();
        }

        // GET: PortFolio/Details/5
       [IsLogout]
        public ActionResult Details(int id)
        {
            ZonaFl.Business.SubSystems.SPortfolio portF = new Business.SubSystems.SPortfolio();
          var portf=  portF.GetPortFolioById(id);
            Models.PortFolio portfm = new Models.PortFolio();
            portfm.InjectFrom(portf);
            return View(portfm);
        }

        [IsLogout]
        public ActionResult DetailsById(string id)
        {
            List<ZonaFl.Persistence.Entities.PortFolio> portsf = new List<Persistence.Entities.PortFolio>();
            ZonaFl.Business.SubSystems.SPortfolio portF = new Business.SubSystems.SPortfolio();

            portsf = portF.GetPortFolioByUserId(SessionBag.Current.User.Id);

            List<Models.PortFolio> porfsM = new List<Models.PortFolio>();
            if (portsf.Count > 0)
            {
                porfsM = portsf.Select(e => new Models.PortFolio().InjectFrom(e)).Cast<Models.PortFolio>().ToList();
                porfsM.ForEach(e => e.UserId = portsf.FirstOrDefault().UserId);
                              
            }
            ViewBag.UserId = SessionBag.Current.User.Id;
            ViewBag.ImageUser = SessionBag.Current.User.Image;
            ViewBag.IdUser = id;
            return View("Details", porfsM);

        }

        [IsLogout]
        public ActionResult EditById(string id)
        {
            //ZonaFl.Persistence.Entities.PortFolio portf = null;
            //ZonaFl.Business.SubSystems.SPortfolio portF = new Business.SubSystems.SPortfolio();
            //portf = portF.GetPortFolioByUserId(id,SessionBag.Current.User.Id);
            //if (portf == null)
            //    portf = new Persistence.Entities.PortFolio();
            //Models.PortFolio porfM = new Models.PortFolio();
            //porfM.InjectFrom(portf);

            //ViewBag.User = portf.AspNetUser;
           
            ViewBag.Idportf = id;
            return View("Editar");

        }

        [IsLogout]
        [HttpGet]
        public ActionResult GetPortfolioByUser(string id)
        {

            ZonaFl.Persistence.Entities.PortFolio portf = null;
            ZonaFl.Business.SubSystems.SPortfolio portF = new Business.SubSystems.SPortfolio();
            portf = portF.GetPortFolioByUserId(id.Replace("#nuevowork", ""),SessionBag.Current.User.Id);
            if (portf == null)
                portf = new Persistence.Entities.PortFolio();
            Models.PortFolio porfM = new Models.PortFolio();
            porfM.InjectFrom(portf);

            return Json(porfM, JsonRequestBehavior.AllowGet);
            
        }
        [IsLogout]
        [HttpGet]
        public ActionResult GetPortfolioByUserAnId(string id, string userid)
        {

            ZonaFl.Persistence.Entities.PortFolio portf = null;
            Business.SubSystems.SCategory scategory = new Business.SubSystems.SCategory();
            ZonaFl.Business.SubSystems.SPortfolio portF = new Business.SubSystems.SPortfolio();
            portf = portF.GetPortFolioByUserId(id, userid);
            if (portf == null)
                portf = new Persistence.Entities.PortFolio();
            Models.PortFolio porfM = new Models.PortFolio();
            porfM.InjectFrom(portf);
            var catp = scategory.FindCategoryById(portf.CategoriaId);
            if(catp!=null)
            porfM.Categoria=catp.Name;
            return Json(porfM, JsonRequestBehavior.AllowGet);

        }


        [AllowAnonymous]
        [HttpPost]
        public ActionResult UploadedFiles()
        {
            try
            {
                if (HttpContext.Request.Files.AllKeys.Any())
                {
                    // Get the uploaded image from the Files collection
                    var httpPostedFile = HttpContext.Request.Files["upload"];

                    if (httpPostedFile != null)
                    {
                        // Validate the uploaded image(optional)

                        // Get the complete file path
                        var fileSavePath = System.IO.Path.Combine(HttpContext.Server.MapPath("~/UploadedFiles/Portfolio"), httpPostedFile.FileName);

                        // Save the uploaded file to "UploadedFiles" folder
                        httpPostedFile.SaveAs(fileSavePath);
                        //AspNetUsers aspuser = new AspNetUsers();
                        //var useru = UserManager.FindByEmail(user.Email);

                        return Json(data: httpPostedFile.FileName);
                    }

                    return Json(data:  "Error Archivo.No se pudo descargar el archivo correctamente.");
                }

                return Json(data: "Error Archivo. No se ha encontrado archivo ha descargar.");
            }
            catch (Exception ex)
            {
                return Json(data: "Error Archivo. mientras se trató de descargar el archivo. Error Message: " + ex.Message);
            }
           
        }

        [IsLogout]
        public ActionResult InsertPortfolio(PortFolio portfolio)
        {
            ZonaFl.Business.SubSystems.SUser suser = new Business.SubSystems.SUser();

            //ZonaFl.Persistence.Entities.AspNetUsers useru = null;
            ZonaFl.Business.SubSystems.SPortfolio portn = new Business.SubSystems.SPortfolio();
            Persistence.Entities.PortFolio portfolioP = new Persistence.Entities.PortFolio();
            portfolioP.InjectFrom(portfolio);
            
            if(portn.InsertPortFolioByUser(portfolioP))
            {
                return Json(data: true);
            }
            else
            {
                return Json(data: false);
            }
          



            
          


        }

        [IsLogout]
        public ActionResult UpdatePortfolio(PortFolio portfolio)
        {
            ZonaFl.Business.SubSystems.SUser suser = new Business.SubSystems.SUser();

            //ZonaFl.Persistence.Entities.AspNetUsers useru = null;
            ZonaFl.Business.SubSystems.SPortfolio portn = new Business.SubSystems.SPortfolio();
            Persistence.Entities.PortFolio portfolioP = new Persistence.Entities.PortFolio();
            portfolioP.InjectFrom(portfolio);

            if (portn.UpdatePortFolioByUser(portfolioP))
            {
                return Json(data: true);
            }
            else
            {
                return Json(data: false);
            }








        }
        [HttpPost]
        [IsLogout]
        public ActionResult DeletePortfolio(string id)
        {
            

            //ZonaFl.Persistence.Entities.AspNetUsers useru = null;
            ZonaFl.Business.SubSystems.SPortfolio portn = new Business.SubSystems.SPortfolio();
         

            if (portn.DeletePortFolio(int.Parse(id)))
            {
                return Json(data: true);
            }
            else
            {
                return Json(data: false);
            }








        }

        
        public ActionResult CreateById(string id)
        {
            Business.SubSystems.SCategory scategory = new Business.SubSystems.SCategory();
            Models.PortFolio porfM = new Models.PortFolio();
            porfM.AspNetUser = new Models.RegisterBindingModel();
            porfM.AspNetUser.Id = id;
            porfM.UserId = id;
            porfM.Categories = scategory.FindAll().Select(e => new Category().InjectFrom(e)).Cast<Category>().ToList();
                        ViewBag.IdUser = id;
            return View("Create", porfM);

        }

        // GET: PortFolio/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PortFolio/Create
        [IsLogout]
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: PortFolio/Edit/5
        [IsLogout]
        public ActionResult Edit(int id)
        {
            ZonaFl.Business.SubSystems.SPortfolio portF = new Business.SubSystems.SPortfolio();
            var portf = portF.GetPortFolioById(id);
            Models.PortFolio portfm = new Models.PortFolio();
            portfm.InjectFrom(portf);
            return View(portfm);
           
        }

        // POST: PortFolio/Edit/5
        [IsLogout]
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection, Models.PortFolio portfolio)
        {
            try
            {

                ZonaFl.Business.SubSystems.SPortfolio portF = new Business.SubSystems.SPortfolio();
                Entities.PortFolio portfolioU = new Entities.PortFolio();
                portfolioU.InjectFrom(portfolio);
                portF.Update(portfolioU);
                // TODO: Add update logic here

                return RedirectToAction("Details", id);
            }
            catch
            {
                return View();
            }
        }

        // GET: PortFolio/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PortFolio/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
