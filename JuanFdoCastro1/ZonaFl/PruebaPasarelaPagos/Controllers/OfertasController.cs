using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZonaFl.Business.SubSystems;

namespace PruebaPasarelaPagos.Controllers
{
    public class OfertasController : Controller
    {
        // GET: Ofertas
        public ActionResult Index()
        {
            SOffer soffer = new SOffer();
           var ofertas= soffer.GetList();
            return View(ofertas);
        }


        public ActionResult Pagar(int id)
        {
            SOffer sofer = new SOffer();
           var oferta= sofer.GetById(id);
            return View();
        }

        // GET: Ofertas/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Ofertas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Ofertas/Create
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

        // GET: Ofertas/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Ofertas/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Ofertas/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Ofertas/Delete/5
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
