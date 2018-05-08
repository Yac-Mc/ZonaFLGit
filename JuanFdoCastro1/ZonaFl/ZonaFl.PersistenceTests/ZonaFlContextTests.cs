using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZonaFl.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZonaFl.Persistence.Entities;

namespace ZonaFl.Persistence.Tests
{
    [TestClass()]
    public class ZonaFlContextTests
    {
        [TestMethod()]
        public void ZonaFlContextTest()
        {
            using (var db = new ZonaFlContext())
            {
                try
                {


                    db.Categorias.Add(new ZonaFl.Persistence.Entities.Category { Id = 0, Name = "Páginas web y software" });
                    //    db.Categories.Add(new ZonaFl.Persistence.Entities.Category { Id = 0, Name = "Aplicaciones para Móviles" });
                    //    db.Categories.Add(new ZonaFl.Persistence.Entities.Category { Id = 0, Name = "Diseño" });
                    //    db.Categories.Add(new ZonaFl.Persistence.Entities.Category { Id = 0, Name = "Producción Multimedia" });
                    //    db.Categories.Add(new ZonaFl.Persistence.Entities.Category { Id = 0, Name = "Animación 3D" });
                    //    db.Categories.Add(new ZonaFl.Persistence.Entities.Category { Id = 0, Name = "Office" });
                    //    db.Categories.Add(new ZonaFl.Persistence.Entities.Category { Id = 0, Name = "Traducción y Redacción" });
                    //    db.Categories.Add(new ZonaFl.Persistence.Entities.Category { Id = 0, Name = "Ingeniería y arquitectura" });
                }

                catch (Exception er)
                {


                }


                db.SaveChanges();


            }
        }


         
    }
}