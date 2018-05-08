using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZonaFl.Persistence;
using ZonaFl.Persistence.Entities;
using ZonaFl.Persistence.Repository;

namespace ZonaFl.Business.SubSystems
{
    public class SPortfolio
    {
        public SPortfolio()
        {

            //using (var db = new ZonaFlContext())
            //{
            //    try
            //    {

            //        db.SaveChanges();
            //        db.Categories.Add(new ZonaFl.Persistence.Entities.Category {  Name = "Páginas web y software" });
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

                
            //}
            //  //  ZonaFlContext db = new ZonaFlContext();
           
        }

        public List<PortFolio> GetPortFolioByUserId(string userid)
        {
            PortFolioRepository portrepo = new PortFolioRepository();
            var portafoliofind = portrepo.FindPortFoliosByUser(Guid.Parse(userid));
            if (portafoliofind != null)
            {
                return new List<PortFolio>();
            }

            return portrepo.FindPortFoliosByUser(Guid.Parse(userid)).ToList();
        }

        public PortFolio GetPortFolioById(int id)
        {
            PortFolioRepository portrepo = new PortFolioRepository();
           return portrepo.FindByID(id);
           

        }

        public PortFolio GetPortFolioByUserId(string idportf,string  userid)
        {
            PortFolioRepository portrepo = new PortFolioRepository();
            return portrepo.FindPortFolioByUser(int.Parse(idportf),Guid.Parse(userid));


        }


        public List<ZonaFl.Persistence.Entities.PortFolio> InsertPortFolioByUser(List<ZonaFl.Persistence.Entities.PortFolio> portfolios,AspNetUsers user)
        {

            PortFolioRepository portrepo = new PortFolioRepository();
            List<PortFolio> portfoliosn = new List<PortFolio>();

            foreach (var por in portfolios)
            {
                por.AspNetUser = user;
                try
                {
                    portrepo.Add(por);
                    portfoliosn.Add(por);
                }
                catch (Exception er)
                {

                }
             
               
            }
            return portfoliosn;


        }

        public bool InsertPortFolioByUser(ZonaFl.Persistence.Entities.PortFolio portfolio)
        {
            PortFolioRepository2< ZonaFl.Persistence.Entities.PortFolio> portrepo = new PortFolioRepository2<ZonaFl.Persistence.Entities.PortFolio>();
            try
            {
               
                portrepo.Insert(portfolio);
            }
            catch (Exception er)
            {
                return false;
            }
            return true;

        }

        public bool UpdatePortFolioByUser(ZonaFl.Persistence.Entities.PortFolio portfolio)
        {
            PortFolioRepository2<ZonaFl.Persistence.Entities.PortFolio> portrepo = new PortFolioRepository2<ZonaFl.Persistence.Entities.PortFolio>();
            try
            {

                portrepo.Update(portfolio);
            }
            catch (Exception er)
            {
                return false;
            }
            return true;

        }

        public Entities.PortFolio Update(Entities.PortFolio portfolio)
        {

            PortFolioRepository portrepo = new PortFolioRepository();
           
           
            portrepo.Update(portfolio);
            return portfolio;

        }

        public bool DeletePortFolio(int id)
        {
            PortFolioRepository2<ZonaFl.Persistence.Entities.PortFolio> portrepo = new PortFolioRepository2<ZonaFl.Persistence.Entities.PortFolio>();
            try
            {

                return  portrepo.Delete(id);
            }
            catch (Exception er)
            {
                return false;
            }
          
        }
    }
}
