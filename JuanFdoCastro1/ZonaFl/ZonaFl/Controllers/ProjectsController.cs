using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZonaFl.Business.SubSystems;
using ZonaFl.Persistence.Entities;
using Omu.ValueInjecter;
using ZonaFl.Models;
using ZonaFl.Controllers.Filters;
using ZonaFl.Entities.CustomEntities;

namespace ZonaFl.Controllers
{
    public class ProjectsController : Controller
    {
        // GET: Project
       
        public ActionResult Index(string id)
        {
           
            int statusProject = 0;
            if (Request.QueryString["statusProject"] != null)
                statusProject = int.Parse(Request.QueryString["statusProject"]);
                 
      
             ViewBag.Status = statusProject;

            SOffer soffer = new SOffer();
            int pagenumber = 1;//int.Parse( Request.QueryString.Get("pagenumber"));
            int itemsperpage = 50;// int.Parse(Request.QueryString.Get("itemsperpage"));
            string conditions = "";
            ZonaFl.Persistence.Entities.AspNetUsers useru = null;
            ZonaFl.Business.SubSystems.SUser usern = new Business.SubSystems.SUser();
            useru = usern.GetUserById(new Guid(id));
            if (SessionBag.Current.User == null)
            {
                SessionBag.Current.User = useru;
            }
            //if (statusProject == 2 && !useru.Freelance)
            //    statusProject = 1;

            //    Publicada = 0,
            //EnCurso = 1,
            //Finalizada = 2,
            //Eliminada = 3
            if (statusProject == 0 )
            {
                if (!useru.Freelance)
                {
                    //conditions = " where O.Id NOT IN(SELECT Project.idoffer  from project) and O.iduser='" + id + "' and (OP.StatusPhase=" + statusProject + " "; //Request.QueryString.Get("conditions");
                    conditions = " where O.Id NOT IN(SELECT Project.idoffer  from project) and (O.iduser='" + id + "' and O.Status=0"; //and (OP.StatusPhase=" + statusProject + " "; //Request.QueryString.Get("conditions");
                }
                else
                {
                    conditions = " where O.Id NOT IN(SELECT Project.idoffer  from project) and (OU.iduser='" + id + "' and O.Status=0";// and (OP.StatusPhase=" + statusProject + " "; //Request.QueryString.Get("conditions");
                }
            }
            else if(statusProject >= 1)
            {
                if (!useru.Freelance)
                {
                    conditions = " where O.IdUser = '" + id + "' and (project.Status = " + statusProject+ " OR O.Status=" + statusProject;
                    //conditions = " where O.Id IN(SELECT Project.idoffer  from project) and (O.iduser='" + id + "' and project.Status="+ statusProject;

                }
                else 
                {
                    //conditions = " where O.IdUser = '" + id + "' and project.Status = " + statusProject;
                    conditions = " where O.Id IN(SELECT Project.idoffer  from project where  project.Status="+ statusProject+") and (OU.iduser='" + id +"'"; //Request.QueryString.Get("conditions");

                }

                }
            ///user session bag
            ZonaFl.Business.SubSystems.SProject userp = new Business.SubSystems.SProject();
            var projects = userp.GetProjectsEndedByUser(id);
            List<Models.Project> projetsm = new List<Models.Project>();

            projetsm = projects.Select(e => new Models.Project().InjectFrom(e)).Cast<Models.Project>().ToList();
            projetsm.Select(e => new Models.Project().InjectFrom(usern.GetUserByOffer(e.IdOffer, 1))).Cast<Models.Project>().ToList();
           
            ViewBag.Commentaries = projetsm;
            int calif = userp.GetCalificationAverageUser(id);
            //calif = 3;
            ViewBag.Calification = calif;



            RegisterBindingModel regbm = new RegisterBindingModel();
            regbm.InjectFrom(useru);
            regbm.Skills = useru.Skills.Select(e => new ZonaFl.Models.Skill().InjectFrom(e)).Cast<ZonaFl.Models.Skill>().ToList();
            SessionBag.Current.User = regbm;
            ViewBag.IdUser = id;
            ViewBag.NameUser = regbm.UserName;
            ViewBag.User = useru.UserName;
            ViewBag.ImageUser = useru.Image;
            ViewBag.EmailUserBuyer = regbm.Email;
            ViewBag.EmailConfirmed=(useru.EmailConfirmed)? @"verified": @"";
            ViewBag.PagosConfirmed = (useru.PagosConfirmed) ? @"verified" : @"";
           // ViewBag.MobileConfirmed=
            /////

            string order = Request.QueryString.Get("order");
            SCategory scat = new SCategory();
            List<ZonaFl.Persistence.Entities.Category> listcat = scat.FindAll();
            ViewBag.Categories = listcat;
            List<Persistence.Entities.Offer> lista = new List<Persistence.Entities.Offer>();
            if (!regbm.Freelance)
            {
                //if (statusProject == 0)
                //{
                //    conditions += " or OP.StatusPhase =" + 1+")";
                //}
                //else
                //{
                    conditions += " )";
                //}
                  lista = soffer.GetListPaged(pagenumber, itemsperpage, conditions+";", order,statusProject);
               
            }
            else
            {
                conditions += " )";
                lista = soffer.GetAppliedOfferByUserListPaged(pagenumber, itemsperpage, conditions, order);
            }

            
            
            List<Models.Offer> listoffers = lista.Select(e => new Models.Offer().InjectFrom(e)).Cast<Models.Offer>().ToList();
            SProject spro = new SProject();

            listoffers.ForEach(e => e.Comments = spro.GetByOffer(e.Id) != null ? spro.GetByOffer(e.Id).Comments : "");
            listoffers.ForEach(e => e.Qualification = spro.GetByOffer(e.Id) != null ? spro.GetByOffer(e.Id).Qualification : 0);
            if (statusProject == 1)
            {
                listoffers.ForEach(e => e.IsForFinally = spro.IsProjectForFinally(e.Id));
            }


            List<Persistence.Entities.Category> listcategories = lista.Select(e => e.Category).ToList();

            foreach (var offer in lista)
            {

                var offerget = soffer.GetById(offer.Id);
                if (offerget != null)
                    offer.OfferPhases = offerget.OfferPhases.Select(e => new Persistence.Entities.OfferPhases().InjectFrom(e)).Cast<Persistence.Entities.OfferPhases>().ToList();
               
                listoffers.FirstOrDefault(e => e.Id == offer.Id).OfferPhases = offer.OfferPhases.Where(e => e.IdOffer == offer.Id).ToList().Select(t=>new OfferPhase().InjectFrom(t)).Cast<OfferPhase>().ToList();   //new OfferPhase().InjectFrom(e)).Cast<OfferPhase>().ToList();
               //foreach(var of in offer.OfferPhases.Where(e => e.IdOffer == offer.Id).ToList())
               // {
               //     Models.OfferPhase newof = new Models.OfferPhase();
               //     newof.InjectFrom(of);
                   
               //     listoffers.FirstOrDefault(e => e.Id == offer.Id).OfferPhases.Add(newof);
               // }

                if (listoffers.FirstOrDefault(e => e.Id == offer.Id) != null)
                    listoffers.FirstOrDefault(e => e.Id == offer.Id).Category = new Models.Category();//.InjectFrom(offer.Category);
                listoffers.FirstOrDefault(e => e.Id == offer.Id).Category.InjectFrom(offer.Category);
                listoffers.FirstOrDefault(e => e.Id == offer.Id).ContractorCity = ((RegisterBindingModel)SessionBag.Current.User).City;
                listoffers.FirstOrDefault(e => e.Id == offer.Id).ContractorCountry = ((RegisterBindingModel)SessionBag.Current.User).Country;
                listoffers.FirstOrDefault(e => e.Id == offer.Id).NameContractor = ((RegisterBindingModel)SessionBag.Current.User).FirstMiddleName;
                listoffers.FirstOrDefault(e => e.Id == offer.Id).NoPostulados = soffer.GetNoPostuladosByOffer(offer.Id);




                var dateoferfase1 = soffer.GetPhaseInitial(offer.Id);
                if (dateoferfase1 == null)
                {
                   
                    listoffers.FirstOrDefault(e => e.Id == offer.Id).DateIniPhase1 = DateTime.Parse("01/01/1900");
                }
                else
                {
                    

                    ViewBag.InicioEst = dateoferfase1.InitPhase;
                    if (offer.OfferPhases.Count > 0)
                    {
                        var finest = offer.OfferPhases.LastOrDefault(e => e.FinishPhase != null).FinishPhase;
                        ViewBag.FinEst = finest;
                        listoffers.FirstOrDefault(e => e.Id == offer.Id).FinEst = finest;
                    }
                    else
                    {
                        ViewBag.FinEst = dateoferfase1.FinishPhase;
                        listoffers.FirstOrDefault(e => e.Id == offer.Id).FinEst= dateoferfase1.FinishPhase;
                    }
                    listoffers.FirstOrDefault(e => e.Id == offer.Id).DateIniPhase1 = dateoferfase1.InitPhase;
                    listoffers.FirstOrDefault(e => e.Id == offer.Id).InicioEst = dateoferfase1.InitPhase;
                    
                }
            }


            if (!regbm.Freelance)
            {
                return View("DetailsForEmployer", listoffers);
            }
            else
            {
                return View("DetailsForFreelance", listoffers);
            }
        }

       

        [HttpPost]
        public ActionResult Filter(string filter)
        {
            if (!string.IsNullOrEmpty(filter) && filter.EndsWith("or "))
            {
                filter = " and " + ReplaceLastOccurrence(filter, "or", "");

            }

            if (!string.IsNullOrEmpty(filter) && filter.EndsWith("and "))
            {
                filter = " and " + ReplaceLastOccurrence(filter, "and", "");

            }
            SProject sproject = new SProject();
            int pagenumber = 1;//int.Parse( Request.QueryString.Get("pagenumber"));
            int itemsperpage = 50;// int.Parse(Request.QueryString.Get("itemsperpage"));

            string conditions = " where iduser='" + ((RegisterBindingModel)SessionBag.Current.User).Id + "' " + filter + "; "; //Request.QueryString.Get("conditions");
            string order = Request.QueryString.Get("order");
            SCategory scat = new SCategory();
          
            List<Persistence.Entities.Project> lista = sproject.GetListPaged(pagenumber, itemsperpage, conditions, order);
            List<Models.Project> listprojects = lista.Select(e => new Models.Project().InjectFrom(e)).Cast<Models.Project>().ToList();
           
            if (((RegisterBindingModel)SessionBag.Current.User).Freelance)
            {
                return PartialView("PartialFilterOffer", listprojects);
            }
            else
            {
                return PartialView("PartialFilterOffer", listprojects);
            }



        }

        public static string ReplaceLastOccurrence(string Source, string Find, string Replace)
        {
            int place = Source.LastIndexOf(Find);

            if (place == -1)
                return Source;

            string result = Source.Remove(place, Find.Length).Insert(place, Replace);
            return result;
        }


        [ValidateInput(false)]
        public ActionResult DetailsPay()
        {
            Pagos pagos = new Pagos();
            System.Text.StringBuilder strb = new System.Text.StringBuilder();

            ViewBag.transactionState= Request["transactionState"].ToString();



            foreach (var prop in pagos.GetType().GetProperties())
            {
                if(Request[prop.Name]!=null)
                {
                    try
                    {
                        pagos.GetType().GetProperty(prop.Name).SetValue(pagos, Request[prop.Name]);
                    }
                    catch( Exception er)
                    {
                        strb.Append(er.Message);
                        
                    }
                    
                }
            }
            ViewBag.Errors = strb.ToString();
           
            if (((string)ViewBag.transactionState)=="4" || ((string)ViewBag.transactionState) == "APPROVED")
            return RedirectToAction("StartPhase", "Contratante" ,new {id =pagos.referenceCode });
            return View(pagos);
        }


        // GET: Project/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Project/Create
        public ActionResult Create()
        {

            Models.Project Project = new Models.Project();
            return View(Project);
        }

        // POST: Project/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection, Models.Project Project)
        {
            try
            {
                // TODO: Add insert logic here
                SProject SProject = new SProject();

                Persistence.Entities.Project projectp = new Persistence.Entities.Project();
                projectp.InjectFrom(Project);
                SProject.Insert(projectp);
                return RedirectToAction("Index");
            }
            catch
            {
               
                return View();
            }
        }

        // GET: Project/Edit/5
        public ActionResult Edit(int id)
        {

            SProject SProject = new SProject();
            Persistence.Entities.Project Project = SProject.Get(id);
            return View(Project);
        }

        // POST: Project/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection, Models.Project Project)
        {
            try
            {
                
                SProject SProject = new SProject();
                Persistence.Entities.Project project = new Persistence.Entities.Project();
                project.InjectFrom(Project);
                Persistence.Entities.Project updaoofer = SProject.Update(project);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Project/Delete/5
        public ActionResult Delete(int id)
        {

            SProject SProject = new SProject();
            Persistence.Entities.Project Project = SProject.Get(id);
            
            return View(Project);
        }

        // POST: Project/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                SProject SProject = new SProject();
                
                SProject.Delete(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
