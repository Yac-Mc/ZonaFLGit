using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZonaFl.Models;

namespace ZonaFl.Controllers
{
    public class PaisesController : Controller
    {
        // GET: Paises
        public ActionResult Index()
        {
            Country country = new Country()
            {
                CountryName = "Colombia"
            };

            List<Country> paises = new List<Country>();
            paises.Add(country);
            return View(paises);
        }

        // GET: Paises/Details/5
        public ActionResult Details(int id)
        {
            Country country = new Country()
            {
                CountryName = "Colombia"
            };

           return View(country);
        }

        // GET: Paises/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Paises/Create
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

        // GET: Paises/Edit/5
        public ActionResult Edit(int id)
        {
            
            return View();
        }

        // POST: Paises/Edit/5
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

        // GET: Paises/Delete/5
        public ActionResult Delete(int id)
        {
            
            return View();
        }

        // POST: Paises/Delete/5
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
