using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZonaFl.Persistence;
using ZonaFl.Persistence.Entities;
using ZonaFl.Persistence.Repository;
using Omu.ValueInjecter;
namespace ZonaFl.Business.SubSystems
{
    public class SProject
    {
        public SProject()
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

       

        public Project Get(int id)
        {
            ProjectRepository<Project> Projectrepo = new ProjectRepository<Project>();
           return  Projectrepo.Get(id);
           

        }
       
        public List<Project> GetList()
        {
            ProjectRepository<Project> Projectrepo = new ProjectRepository<Project>();
           return  Projectrepo.GetList().ToList().Select(e => new Project().InjectFrom(e)).Cast<Project>().ToList();

        }

        public List<Project> GetList(object whereConditions)
        {
            ProjectRepository<Project> Projectrepo = new ProjectRepository<Project>();
            return Projectrepo.GetList(whereConditions).ToList().Select(e => new Project().InjectFrom(e)).Cast<Project>().ToList();

        }

        public int GetCalificationAverageUser(string idUser)
        {
            int calification = 0;
                ProjectRepository<Project> Projectrepo = new ProjectRepository<Project>();
           var projects=Projectrepo.GetProjectsEndedByUser(idUser);
            int cantproy = projects.Count;
            int calprotot = 0;
            if (cantproy > 0)
            {
                foreach (var project in projects)
                {
                    short calpro = 0;
                    if (project.Qualification != null)
                    {
                        calpro = (short)project.Qualification;
                        calprotot += calpro;
                    }
                }

                calification = (int)Math.Round((decimal)(calprotot / cantproy), MidpointRounding.AwayFromZero);
            }
            return calification;


        }
        public List<Project> GetProjectsEndedByUser(string idUser)
        {
            
            ProjectRepository<Project> Projectrepo = new ProjectRepository<Project>();
            var projects = Projectrepo.GetProjectsEndedByUser(idUser);
           
            return projects;


        }


        public List<Project> GetList(string whereConditions)
        {
            ProjectRepository<Project> Projectrepo = new ProjectRepository<Project>();
            return Projectrepo.GetList(whereConditions).ToList().Select(e => new Project().InjectFrom(e)).Cast<Project>().ToList();

        }



        public Project Update(Project Project)
        {

            ProjectRepository<Project> Projectrepo = new ProjectRepository<Project>();
            int? rta = Projectrepo.Update(Project);

            if (rta == null)
            {
                return null;
            }
            else
            {
               
                SUser suser = new SUser();
                SProject spro = new SProject();
               var user= suser.GetUserByOffer(Project.IdOffer, 1);
                //var user = suser.GetUserById(new Guid(Project.Offer.IdUser));
                
                try
                {
                    Log4NetLogger logger2 = new Log4NetLogger();
                    SCategory scate = new SCategory();
                    var cate = scate.FindCategoryById(Project.IdCategory);
                    logger2.Info("Creación Proyecto:" + Project.Id + "," + "UsuarioOrigen:" + user.UserName + ",Categoria:" + cate.Name);
                }
                catch
                {
                   
                }
              
                return Project;
            }
            

        }

        public Project Insert(Project Project, Persistence.Entities.ProjectUser projectuser)
        {

            ProjectRepository<Project> Projectrepo = new ProjectRepository<Project>();
          int? rta=  Projectrepo.Insert(Project, projectuser.IdUser);
            if(rta == null)
            {
                return null;
            }
            else
            {
                Log4NetLogger logger2 = new Log4NetLogger();

                logger2.Info("Creación Proyecto:" + Project.Id + "," + "UsuarioOrigen:" + projectuser.IdUser + ",Categoria:" + Project.IdCategory);
                return Project;
               
            }
          

        }

        public Project Insert(Project Project)
        {

            ProjectRepository<Project> Projectrepo = new ProjectRepository<Project>();
            int? rta = Projectrepo.Insert(Project);
            if (rta == null)
            {
                return null;
            }
            else
            {
                Log4NetLogger logger2 = new Log4NetLogger();
                SCategory scate = new SCategory();
                var cate = scate.FindCategoryById(Project.IdCategory);
                SOffer sofer = new SOffer();
               var offer= sofer.Get(Project.IdOffer);
                logger2.Info("Creación Proyecto:" + Project.Id + "," + "UsuarioOrigen:" + offer.IdUser + ",Categoria:" + cate.Name);
                return Project;
            }


        }


        public int?  Delete(int id)
        {

            ProjectRepository<Project> Projectrepo = new ProjectRepository<Project>();
            int? rta = Projectrepo.Delete(id);

            if (rta == null)
            {
                return null;
            }
            else
            {
                return rta;
            }


        }


        public List<Project> GetListPaged(int pagenumber, int itemsperpage, string conditions, string order)
        {
            ZonaFl.Persistence.Repository.ProjectRepository<Project> Projectrepo = new ProjectRepository<Project>();
            OfferUserRepository<OfferUser> OfferrepoUser = new OfferUserRepository<OfferUser>();
            OfferPhasRepository<OfferPhases> OfferrepoPhases = new OfferPhasRepository<OfferPhases>();
            UserRepository userrepo = new UserRepository();
           var lista= Projectrepo.GetListPaged<Project,Offer>(pagenumber, itemsperpage, conditions,order).ToList().Select(e => new Project().InjectFrom(e)).Cast<Project>().ToList();
            foreach(var project in lista)
            {
              var repoofferuser=  OfferrepoUser.GetList("where idoffer=" + project.IdOffer + " and iduser='" + project.Offer.IdUser+"'").ToList();
                var repoofferphases = OfferrepoPhases.GetList("where idoffer=" + project.IdOffer ).ToList();
                var user= userrepo.FindByID(new Guid(repoofferuser.FirstOrDefault().IdUser));
                project.Offer.OfferUsers = new List<OfferUser>();
                project.Offer.OfferUsers = repoofferuser;
                project.Offer.AspNetUser = new AspNetUsers();
                project.Offer.AspNetUser = user;
                project.Offer.OfferPhases = new List<OfferPhases>();
                project.Offer.OfferPhases = repoofferphases;
            }

            return lista;

           
        }

        public bool IsProjectForFinally(int idoffer)
        {
            ProjectRepository<Project> Projectrepo = new ProjectRepository<Project>();
            return Projectrepo.IsProjectForFinally(idoffer);
        }

        public bool GetStatusEndedProjectByOffer(int idoffer)
        {
            ProjectRepository<Project> Projectrepo = new ProjectRepository<Project>();
            return Projectrepo.GetStatusEndedProjectByOffer(idoffer);
        }

        public Project GetByOffer(int idoffer)
        {
            ProjectRepository<Project> Projectrepo = new ProjectRepository<Project>();
            return Projectrepo.GetByOffer(idoffer);
        }
    }
}
