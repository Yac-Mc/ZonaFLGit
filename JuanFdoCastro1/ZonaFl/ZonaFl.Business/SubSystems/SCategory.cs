using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ZonaFl.Persistence;
using ZonaFl.Persistence.Repository;

namespace ZonaFl.Business.SubSystems
{
   public class SCategory
    {
        public SCategory()
        {
            ////ZonaFlContext zonafl = new ZonaFlContext();
            ////zonafl.SaveChanges();
            //using (var db = new ZonaFlContext())
            //{
            //    try {


            //        db.Categorias.Add(new ZonaFl.Persistence.Entities.Category { Name = "Páginas web y software" });
            //        //db.Categories.Add(new ZonaFl.Persistence.Entities.Category { Id = 0, Name = "Aplicaciones para Móviles" });
            //        //db.Categories.Add(new ZonaFl.Persistence.Entities.Category { Id = 0, Name = "Diseño" });
            //        //db.Categories.Add(new ZonaFl.Persistence.Entities.Category { Id = 0, Name = "Producción Multimedia" });
            //        //db.Categories.Add(new ZonaFl.Persistence.Entities.Category { Id = 0, Name = "Animación 3D" });
            //        //db.Categories.Add(new ZonaFl.Persistence.Entities.Category { Id = 0, Name = "Office" });
            //        //db.Categories.Add(new ZonaFl.Persistence.Entities.Category { Id = 0, Name = "Traducción y Redacción" });
            //        //db.Categories.Add(new ZonaFl.Persistence.Entities.Category { Id = 0, Name = "Ingeniería y arquitectura" });
            //    }

            //    catch (Exception er)
            //    {


            //    }


            //    db.SaveChanges();


            //}
        }

        public List<ZonaFl.Persistence.Entities.Category> FindAll()
        {

            CategoryRepository caterepo = new CategoryRepository();
            return caterepo.FindAll().ToList();


        }

        public ZonaFl.Persistence.Entities.Category FindCategoryByName(string Name)
        {

            CategoryRepository caterepo = new CategoryRepository();
            return caterepo.FindCategoryByName(Name);


        }
        public ZonaFl.Persistence.Entities.Category FindCategoryById(int Id)
        {

            CategoryRepository caterepo = new CategoryRepository();
            return caterepo.FindCategoryById(Id);


        }

        public ZonaFl.Persistence.Entities.Category InsertCategory(string categoryName)
        {
            CategoryRepository caterepo = new CategoryRepository();
            ZonaFl.Persistence.Entities.Category category = new Persistence.Entities.Category()
            {
                
                Name = categoryName
            };
            return caterepo.AddCategory(category);
           
        }
    }
     

    }

