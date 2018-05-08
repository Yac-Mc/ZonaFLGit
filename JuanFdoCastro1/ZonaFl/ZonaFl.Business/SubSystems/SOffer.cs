using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZonaFl.Persistence;

using ZonaFl.Persistence.Repository;
using Omu.ValueInjecter;
using ZonaFl.Persistence.Entities;

namespace ZonaFl.Business.SubSystems
{
    public class SOffer
    {
        public SOffer()
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

        public Offer GetOfferUser(int idoffer)
        {
            OfferRepository<Offer> offerrepo = new OfferRepository<Offer>();
            return offerrepo.GetOfferUser<Offer, OfferUser>(idoffer);
        }

        public OfferUser GetOfferUserById(int idofferUser)
        {
            OfferRepository<Offer> offerrepo = new OfferRepository<Offer>();
            return offerrepo.GetOfferUser<OfferUser>(idofferUser);
        }


        public OfferPhases GetPhaseFinal(int idoffer)
        {
            OfferRepository<OfferPhases> Offerrepo = new OfferRepository<OfferPhases>();
            return Offerrepo.GetPhaseFinal(idoffer);
        }

        public List<OfferPhases> GetOfferPhases(int idoffer)
        {
            OfferPhasesRepository<OfferPhases> Offerrepo = new OfferPhasesRepository<OfferPhases>();

            return Offerrepo.GetPhasesByIdOffer(idoffer).ToList();
        }

        public Offer Get(int id)
        {
            OfferRepository<Offer> Offerrepo = new OfferRepository<Offer>();
            OfferPhasesRepository<OfferPhases> OfferPrepo = new OfferPhasesRepository<OfferPhases>();
            var offer=Offerrepo.Get(id);
            offer.OfferPhases = new List<OfferPhases>();
            offer.OfferPhases= OfferPrepo.GetPhasesByIdOffer(id).ToList();
            return offer;



        }
       
        public List<Offer> GetList()
        {
            OfferRepository<Offer> Offerrepo = new OfferRepository<Offer>();
           return  Offerrepo.GetList().ToList().Select(e => new Offer().InjectFrom(e)).Cast<Offer>().ToList();

        }

        public Offer GetOffer(int idoffer)
        {
            OfferRepository<Offer> offerrepo = new OfferRepository<Offer>();
            return offerrepo.GetOffer<Offer, Category>(idoffer);

        }

        public List<Offer> GetAppliedOfferByUserListPaged(int pagenumber, int itemsperpage, string conditions, string order)
        {
            OfferRepository<Offer> Offerrepo = new OfferRepository<Offer>();

            return Offerrepo.GetAppliedOfferByUserListPaged<Entities.Offer, Entities.Category>(pagenumber, itemsperpage, conditions, order).ToList().Select(e => new Offer().InjectFrom(e)).Cast<Offer>().ToList();
        }

        public List<Offer> GetList(object whereConditions)
        {
            OfferRepository<Offer> Offerrepo = new OfferRepository<Offer>();
            return Offerrepo.GetList(whereConditions).ToList().Select(e => new Offer().InjectFrom(e)).Cast<Offer>().ToList();

        }

        public List<Offer> GetList(string whereConditions)
        {
            OfferRepository<Offer> Offerrepo = new OfferRepository<Offer>();
            return Offerrepo.GetList(whereConditions).ToList().Select(e => new Offer().InjectFrom(e)).Cast<Offer>().ToList();

        }



        public Offer Update(Offer Offer)
        {

            OfferRepository<Offer> Offerrepo = new OfferRepository<Offer>();
            Offer.DateOffer = DateTime.Now;
            int? rta = Offerrepo.Update(Offer);
            Log4NetLogger logger2 = new Log4NetLogger();
            SUser suser = new SUser();
            var user = suser.GetUserById(new Guid(Offer.IdUser));
            SCategory scate = new SCategory();
            var cate = scate.FindCategoryById(Offer.CategoryId);
            logger2.Info("Actualización Oferta:" + Offer.TitleOffer + "," + "UsuarioOrigen:" + user.UserName + ",Categoria:" + cate.Name);

            if (rta == null)
            {
                return null;
            }
            else
            {
                return Offer;
            }
            

        }

        public Offer Insert(Offer Offer)
        {

            OfferRepository<Offer> Offerrepo = new OfferRepository<Offer>();
            SCategory scate = new SCategory();
            var cate=scate.FindCategoryById(Offer.CategoryId);
            Offer.DateOffer = DateTime.Now;
          int? rta=  Offerrepo.Insert(Offer);
            if(rta == null)
            {
                return null;
            }
            else
            {

                Offer.Id =(int)rta;
                Log4NetLogger logger2 = new Log4NetLogger();
                SUser suser = new SUser();
               var user= suser.GetUserById(new Guid(Offer.IdUser));
                logger2.Info("Creación Oferta:" + Offer.TitleOffer+","+"Por:"+ user.UserName+ ",Categoria:"+ cate.Name);
                return Offer;
            }
          

        }

        public int GetNoPostuladosByOffer(int idOffer)
        {
            ProjectRepository<Project> projectRepo = new ProjectRepository<Project>();
          return  projectRepo.GetNoPostuladosByOffer(idOffer);
        }

        public int?  Delete(int id)
        {

            OfferRepository<Offer> Offerrepo = new OfferRepository<Offer>();
            OfferRepository<OfferPhases> OfferrepoP = new OfferRepository<OfferPhases>();
            int rta2 = OfferrepoP.DeletePhases(id);
            int? rta = Offerrepo.Delete(id);
            
            if (rta == null)
            {
                return null;
            }
            else
            {
               
                return rta;
            }


        }
        public OfferPhases GetPhaseInitial(int IdOffer)
        {

            OfferRepository<OfferPhases> Offerrepo = new OfferRepository<OfferPhases>();
           return Offerrepo.GetPhaseInitial(IdOffer);
           

        }

        public List<Offer> GetListPaged(int pagenumber, int itemsperpage, string conditions, string order, int statusproject)
        {
            OfferRepository<Offer> Offerrepo = new OfferRepository<Offer>();
          List<Offer> lista= Offerrepo.GetListPaged<Entities.Offer, Entities.Category>(pagenumber, itemsperpage, conditions, order, statusproject).ToList().Select(e => new Offer().InjectFrom(e)).Cast<Offer>().OrderByDescending(e => e.DateOffer).ToList();
           

            return lista;

        }
        public List<Offer> GetListPaged(int pagenumber, int itemsperpage, string conditions, string order)
        {
            OfferRepository<Offer> Offerrepo = new OfferRepository<Offer>();

            return Offerrepo.GetListPaged<Entities.Offer, Entities.Category>(pagenumber, itemsperpage, conditions, order).ToList().Select(e => new Offer().InjectFrom(e)).Cast<Offer>().OrderByDescending(e => e.DateOffer).ToList();

        }

        public void InsertPhases(List<OfferPhases> listOfferPhases)
        {
            OfferRepository<OfferPhases> Offerrepo = new OfferRepository<OfferPhases>();
            listOfferPhases.ForEach(e => e.StatusPhase = StatusPhase.EnCurso);

            Offerrepo.InsertPhases(listOfferPhases);
           

        }

        public Offer GetById(int id)
        {
            OfferRepository<Offer> Offerrepo = new OfferRepository<Offer>();
            return Offerrepo.GetById(id);
        }

        public void UpdatePhases(List<OfferPhases> listOfferPhases, int IdOffer)
        {
            OfferRepository<OfferPhases> Offerrepo = new OfferRepository<OfferPhases>();
            Offerrepo.UpdatePhases(listOfferPhases, IdOffer);
            Log4NetLogger logger2 = new Log4NetLogger();
            SUser suser = new SUser();
           
            foreach (var oferfases in listOfferPhases)
            {
               var user =suser.GetUserById(new Guid(oferfases.Offer.IdUser));
                logger2.Info("Actualización Fases Oferta:" + IdOffer + "," + "UsuarioOrigen:" + user.UserName + ",Fase:" + oferfases.Name);
            }
        }

        public int? InsertUserOffer( int idoffer, string iduser, bool sendemail)
        {
            OfferUserRepository<OfferUser> OfferUrepo = new OfferUserRepository<OfferUser>();
            OfferPhasesRepository<OfferPhases> OfferPrepo = new OfferPhasesRepository<OfferPhases>();
            ProjectRepository<Project> Offerrepo = new ProjectRepository<Project>();
            if (OfferUrepo.GetOfferUser(idoffer, iduser) == null)
            {
                OfferUrepo.InsertUserOffer(idoffer, iduser);
            }
            else
            {
                return -1;
            }
           

            List<OfferPhases> lista = OfferPrepo.GetPhasesByIdOffer(idoffer).ToList();
               var offer= GetById(idoffer);
            SUser suser = new SUser();
           var contratante= suser.GetUserById(new Guid(offer.IdUser));
            var aplicante= suser.GetUserById(new Guid(iduser));

            //if (Offerrepo.GetByOffer(idoffer) == null)
            //{
            //    Project project = new Project();
            //    project.IdOffer = idoffer;
            //    project.IdCategory = offer.CategoryId;
            //    project.Postulantes = offer.OfferUsers.Count + 1;
            //    project.Qualification = 0;
            //    project.Status = Persistence.Entities.StatusPhase.Publicada;
            //    Offerrepo.Insert(project);
            //}


           



            if (sendemail && ChangeStatusPhases(lista, StatusPhase.EnCurso, offer) !=-1)
            {
                string Url = "http://zonafl.com/Static/index.html#iniciar";
                string body =  "El Usuario,"+ aplicante.UserName;
                body += " ha aplicado al proyecto " + offer.TitleOffer;
                body += " favor ingresar <a href = '" + Url + "'>aqui para ingresar al sitio.</a>";
                body += "<br /><br />Gracias";
              

                var smail= SMail.Instance;
                

                smail.Send("info@zonafl.com", contratante.Email, "Usuario Aplicó proyecto", body);
            }
            else
            {
                return -1;
            }
            return 1;
            Log4NetLogger logger2 = new Log4NetLogger();
            
            
            var user=suser.GetUserById(new Guid(iduser));
            logger2.Info("Postulación Oferta:" + idoffer + "," + "UsuarioOrigen:" + user.UserName);
            //Offerrepo.Insert()


        }

       

        public int? ChangeStatusPhases(ICollection<OfferPhases> offerPhases, Persistence.Entities.StatusPhase status, Persistence.Entities.Offer offer)
        {
            int? rta = null;
            OfferUserRepository<OfferUser> OfferUrepo = new OfferUserRepository<OfferUser>();
            OfferRepository<Offer> Offerrepo = new OfferRepository<Offer>();
            //if (status == StatusPhase.Aplicada)
                //if (!OfferUrepo.HaveOfferUsers(offerPhases.FirstOrDefault().IdOffer) && status==StatusPhase.Aplicada )
                //{
                OfferPhasesRepository<OfferPhases> OfferPrepo = new OfferPhasesRepository<OfferPhases>();
            offer.Status = Offer.StatusOffer.Eliminada;
            Offerrepo.Update(offer);
                foreach (var offerphase in offerPhases)
                {
                    offerphase.StatusPhase = status;
                    rta = OfferPrepo.Update(offerphase);
                if(status==StatusPhase.Finalizada)
                {
                    Business.Log4NetLogger logger2 = new Business.Log4NetLogger();

                    logger2.Info("Proyecto finalizado:" + offerphase.IdOffer );
                }
                else if (status == StatusPhase.Eliminada)
                {
                    Business.Log4NetLogger logger2 = new Business.Log4NetLogger();

                    logger2.Info("Proyecto eliminado:" + offerphase.IdOffer);
                }
                    if (rta == null)
                        break;
                }
            //}
            return rta;
           
        }

        public int? ChangeStatusPhases(List<OfferPhases> ofertas)
        {
            int? rta = null;
            OfferUserRepository<OfferUser> OfferUrepo = new OfferUserRepository<OfferUser>();
            //if (status == StatusPhase.Aplicada)
            //if (!OfferUrepo.HaveOfferUsers(offerPhases.FirstOrDefault().IdOffer) && status==StatusPhase.Aplicada )
            //{
            OfferPhasesRepository<OfferPhases> OfferPrepo = new OfferPhasesRepository<OfferPhases>();
            int i = 0;
            bool finaliza = false;
          
            foreach (var offerphase in ofertas)
            {

              int countnofini=  ofertas.Count(e => e.StatusPhase != StatusPhase.Finalizada);
                if (finaliza && countnofini > 0)
                {
                    offerphase.StatusPhase = StatusPhase.EnCurso;
                }
                rta = OfferPrepo.Update(offerphase);
              
                
                if (rta == null)
                    break;
                if (offerphase.StatusPhase==StatusPhase.Finalizada)
                {
                    i += 1;
                    finaliza = true;
                  
                }
              
                
            }


            if(i== ofertas.Count & finaliza)
            {
                SProject spro = new SProject();
                var lista = spro.GetList(new { IdOffer = ofertas.FirstOrDefault().IdOffer });
                if(lista.Count>0)
                {
                    var project = lista.FirstOrDefault();
                    project.Status = StatusProject.Finalizada;
                    spro.Update(project);
                    Business.Log4NetLogger logger2 = new Business.Log4NetLogger();
                  
                    logger2.Info("Proyecto finalizado:" + project.IdOffer + "," + "Categoria:" + project.Category );

                }

                //var project= spro.GetList(new {IdOffer= ofertas.FirstOrDefault().IdOffer }).FirstOrDefault();

            }
            //}
            return rta;
        }

        public Offer GetOfferByPhase(int id)
        {
            OfferPhasesRepository<OfferPhases> OfferPrepo = new OfferPhasesRepository<OfferPhases>();
           var offerphase= OfferPrepo.Get(id);
           return  this.Get(offerphase.IdOffer);
        }

        public void CopyOffertoProject(List<OfferPhases> ofertas)
        {
            SProject sproject = new SProject();
            Project pro = new Project();

            pro.IdCategory = this.Get(ofertas.FirstOrDefault().IdOffer).CategoryId;
            pro.IdOffer = ofertas.FirstOrDefault().IdOffer;
            pro.Status = StatusProject.EnCurso;
            pro.Postulantes = 0;
            sproject.Insert(pro);
        }
    }
}
