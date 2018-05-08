using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZonaFl.Models;
using Omu.ValueInjecter;
using ZonaFl.Persistence.Entities;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using ZonaFl.Controllers.Filters;
using ZonaFl.Business;
using log4net.Core;

namespace ZonaFl.Controllers
{
    public class ContratanteController : Controller
    {
        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // GET: Profile
        public ActionResult Index()
        {

            return View();
        }

        // GET: Profile/Details/5
        public ActionResult Details(int id)
        {
           
            return View();
        }


        [IsLogout]
        public ActionResult Read(string id)
        {
            ZonaFl.Persistence.Entities.AspNetUsers useru = null;
            ZonaFl.Business.SubSystems.SUser usern = new Business.SubSystems.SUser();
            ZonaFl.Business.SubSystems.SOffer offern = new Business.SubSystems.SOffer();
            useru = usern.GetUserById(new Guid(id));
            RegisterBindingModel regbm = new RegisterBindingModel();
            regbm.InjectFrom(useru);
           
            regbm.Offers = offern.GetList(new { IdUser = id }).Select(e => new Models.Offer().InjectFrom(e)).Cast<Models.Offer>().ToList();
            regbm.Offers.ForEach(e => e.OfferPhases = offern.GetOfferPhases(e.Id).Select(t => new Models.OfferPhase().InjectFrom(t)).Cast<Models.OfferPhase>().ToList());
            regbm.Offers.ForEach(e => e.Status = (Models.Offer.StatusOffer)offern.GetPhaseFinal(e.Id).StatusPhase);
            if (useru.Companies.Count > 0 && useru.Companies.FirstOrDefault() != null)
                regbm.Company = useru.Companies.Select(e => new Models.Company().InjectFrom(e)).Cast<Models.Company>().ToList();
            SessionBag.Current.User = regbm;
            //regbm.Skills = useru.Skills.Select(e => new ZonaFl.Models.Skill().InjectFrom(e)).Cast<ZonaFl.Models.Skill>().ToList();
            ViewBag.User = useru.UserName;
            ViewBag.IdUser = useru.Id;
            ViewBag.ImageUser = useru.Image;

            return View("ReadForAdmin", regbm);
        }


        [IsLogout]
        public ActionResult DetailsByEmail(string id)
        {
            RegisterBindingModel regbm = new RegisterBindingModel();
            try
            {
                ZonaFl.Persistence.Entities.AspNetUsers useru = null;
                ZonaFl.Business.SubSystems.SUser usern = new Business.SubSystems.SUser();
                
                useru=usern.GetUserByEmail(id);
                regbm.InjectFrom(useru);
                if (useru.Companies.Count > 0 && useru.Companies.FirstOrDefault() != null)
                    regbm.Company = useru.Companies.Select(e => new Models.Company().InjectFrom(e)).Cast<Models.Company>().ToList();
                SessionBag.Current.User = regbm;
                //regbm.Skills = useru.Skills.Select(e => new ZonaFl.Models.Skill().InjectFrom(e)).Cast<ZonaFl.Models.Skill>().ToList();
                ViewBag.User = useru.UserName;
                ViewBag.IdUser = useru.Id;
                ViewBag.ImageUser = useru.Image;

                ZonaFl.Business.SubSystems.SProject userp = new Business.SubSystems.SProject();
                var projects = userp.GetProjectsEndedByUser(useru.Id);
                List<Models.Project> projetsm = new List<Models.Project>();

                projetsm = projects.Select(e => new Models.Project().InjectFrom(e)).Cast<Models.Project>().ToList();
                projetsm.Select(e => new Models.Project().InjectFrom(usern.GetUserByOffer(e.IdOffer, 1))).Cast<Models.Project>().ToList();
                //-prueba

                // projetsm.Add(new Models.Project() { Comments = "Comentario Prueba", UserName = "Jaun fdo catro1", Image = "FotoJuanFdoCastro3.jpg" }
                // );
                // projetsm.Add(new Models.Project() { Comments = "Comentario Prueba", UserName = "Jaun fdo catro2", Image = "FotoJuanFdoCastro3.jpg" }
                //);
                // projetsm.Add(new Models.Project() { Comments = "Comentario Prueba", UserName = "Jaun fdo catro3", Image = "FotoJuanFdoCastro3.jpg" }
                //);
                // projetsm.Add(new Models.Project() { Comments = "Comentario Prueba", UserName = "Jaun fdo catro4", Image = "FotoJuanFdoCastro3.jpg" }
                //);
                // projetsm.Add(new Models.Project() { Comments = "Comentario Prueba", UserName = "Jaun fdo catro1", Image = "FotoJuanFdoCastro3.jpg" }
                //);
                // projetsm.Add(new Models.Project() { Comments = "Comentario Prueba", UserName = "Jaun fdo catro2", Image = "FotoJuanFdoCastro3.jpg" }
                //);
                // projetsm.Add(new Models.Project() { Comments = "Comentario Prueba", UserName = "Jaun fdo catro3", Image = "FotoJuanFdoCastro3.jpg" }
                //);
                // projetsm.Add(new Models.Project() { Comments = "Comentario Prueba", UserName = "Jaun fdo catro4", Image = "FotoJuanFdoCastro3.jpg" }
                //);
                //---
                ViewBag.Commentaries = projetsm;
                int calif = userp.GetCalificationAverageUser(useru.Id);
                //calif = 3;
                ViewBag.Calification = calif;
                return View("Details", regbm);
            }
            catch (Exception er)
            {



                Log4NetLogger logger2 = new Log4NetLogger();
                logger2.CurrentUser = SessionBag.Current.User.Id;
                if (Request != null)
                {
                    logger2.Error(er, Request.Path, Request.RawUrl);
                    throw new Exception(er.Message);
                }
                else
                {
                    logger2.Error(er);
                }




            }
            return View("Details", regbm);

        }



        
        public ActionResult DetailsById(string id)
        {
            RegisterBindingModel regbm = new RegisterBindingModel();
            try
            {
                ZonaFl.Persistence.Entities.AspNetUsers useru = null;
            ZonaFl.Business.SubSystems.SUser usern = new Business.SubSystems.SUser();
            useru = usern.GetUserById(new Guid(id));
                if (SessionBag.Current.User == null)
                {
                    SessionBag.Current.User = useru;
                }
                regbm.InjectFrom(useru);
            if(useru.Companies.Count>0 && useru.Companies.FirstOrDefault() !=null)
            regbm.Company = useru.Companies.Select(e=>new Models.Company().InjectFrom(e)).Cast<Models.Company>().ToList();
            SessionBag.Current.User = regbm;
            //regbm.Skills = useru.Skills.Select(e => new ZonaFl.Models.Skill().InjectFrom(e)).Cast<ZonaFl.Models.Skill>().ToList();
            ViewBag.User = useru.UserName;
            ViewBag.IdUser = useru.Id;
            ViewBag.ImageUser = useru.Image;

            ZonaFl.Business.SubSystems.SProject userp = new Business.SubSystems.SProject();
            var projects = userp.GetProjectsEndedByUser(id);
            List<Models.Project> projetsm = new List<Models.Project>();

            projetsm = projects.Select(e => new Models.Project().InjectFrom(e)).Cast<Models.Project>().ToList();
            projetsm.Select(e => new Models.Project().InjectFrom(usern.GetUserByOffer(e.IdOffer, 1))).Cast<Models.Project>().ToList();
            //-prueba

           // projetsm.Add(new Models.Project() { Comments = "Comentario Prueba", UserName = "Jaun fdo catro1", Image = "FotoJuanFdoCastro3.jpg" }
           // );
           // projetsm.Add(new Models.Project() { Comments = "Comentario Prueba", UserName = "Jaun fdo catro2", Image = "FotoJuanFdoCastro3.jpg" }
           //);
           // projetsm.Add(new Models.Project() { Comments = "Comentario Prueba", UserName = "Jaun fdo catro3", Image = "FotoJuanFdoCastro3.jpg" }
           //);
           // projetsm.Add(new Models.Project() { Comments = "Comentario Prueba", UserName = "Jaun fdo catro4", Image = "FotoJuanFdoCastro3.jpg" }
           //);
           // projetsm.Add(new Models.Project() { Comments = "Comentario Prueba", UserName = "Jaun fdo catro1", Image = "FotoJuanFdoCastro3.jpg" }
           //);
           // projetsm.Add(new Models.Project() { Comments = "Comentario Prueba", UserName = "Jaun fdo catro2", Image = "FotoJuanFdoCastro3.jpg" }
           //);
           // projetsm.Add(new Models.Project() { Comments = "Comentario Prueba", UserName = "Jaun fdo catro3", Image = "FotoJuanFdoCastro3.jpg" }
           //);
           // projetsm.Add(new Models.Project() { Comments = "Comentario Prueba", UserName = "Jaun fdo catro4", Image = "FotoJuanFdoCastro3.jpg" }
           //);
            //---
            ViewBag.Commentaries = projetsm;
            int calif = userp.GetCalificationAverageUser(id);
            //calif = 3;
            ViewBag.Calification = calif;
                return View("Details", regbm);
            }
            catch (Exception er)
            {



                Log4NetLogger logger2 = new Log4NetLogger();
                logger2.CurrentUser = SessionBag.Current.User.Id;
                if (Request != null)
                {
                    logger2.Error(er, Request.Path, Request.RawUrl);
                    throw new Exception(er.Message);
                }
                else
                {
                    logger2.Error(er);
                }

               


            }
            return View("Details", regbm);





        }
        public ActionResult Logout()
        {
            SessionBag.Current.User = null;
            SessionBag.Current.Logout = true;
            return RedirectToAction("Index", "Home");
        }

        // GET: Profile/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Profile/Create
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

        // GET: Profile/Edit/5
        [IsLogout]
        public ActionResult EditById(string id)
        {
            //if (SessionBag.Current.Logout != null && SessionBag.Current.Logout)
            //{

            //    SessionBag.Current.Logout = false;
            //    return RedirectToAction("Index", "Home");
            //}
            ZonaFl.Persistence.Entities.AspNetUsers useru = null;
            ZonaFl.Business.SubSystems.SUser usern = new Business.SubSystems.SUser();
            useru = usern.GetUserById(new Guid(id));

            if(SessionBag.Current.User == null)
            {
                SessionBag.Current.User = useru;
            }
            ZonaFl.Business.SubSystems.SCategory scategory = new Business.SubSystems.SCategory();
            RegisterBindingModel regbm = new RegisterBindingModel();
            regbm.InjectFrom(useru);
            regbm.Skills = useru.Skills.Select(e => new ZonaFl.Models.Skill().InjectFrom(e)).Cast<ZonaFl.Models.Skill>().ToList();
            if(useru.Companies.FirstOrDefault()!=null)
            regbm.Company=useru.Companies.Select(e=>new ZonaFl.Models.Company().InjectFrom(e)).Cast<ZonaFl.Models.Company>().ToList();
            
            List<Models.Category> categorysm = new List<Models.Category>();
            foreach (var skill in useru.Skills)
            {
                Models.Category categorym = new Models.Category();
                if (categorysm.Find(e => e.Id == skill.CategoryId) == null)
                {
                    var category = scategory.FindCategoryById(skill.CategoryId);
                    categorym.InjectFrom(category);
                    categorysm.Add(categorym);
                }
            }


            regbm.Categories = categorysm;
            //regbm.Categories.ForEach(e => e.Skills = useru.Skills.Select(y => new ZonaFl.Models.Skill().InjectFrom(y)).Cast<ZonaFl.Models.Skill>().ToList());
            ViewBag.User = useru.UserName;
            ViewBag.IdUser = useru.Id;
            ViewBag.ImageUser = useru.Image;
            ViewBag.EmailUser = useru.Email;
            //ViewBag.Country = useru.Country;
            ZonaFl.Business.SubSystems.SCountry scountry = new Business.SubSystems.SCountry();

            
            var itemciudad = scountry.FindCityByName(useru.City);
            var item = scountry.FindCountrybyName(useru.Country);

            var items = item != null ? scountry.FindAll().Where(e => e.Id != item.Id).ToList() : scountry.FindAll();
            if(item!=null)
            items.Insert(0, item);
           
            var countries = new SelectList(items, "ID", "Name").ToList();
            ViewBag.Countrysel = item != null ? item.Id: items.First().Id;
            var countryfirst= scountry.FindFirstCityByCountry(int.Parse(ViewBag.Countrysel.ToString()));
            ViewBag.Ciudadsel = string.IsNullOrEmpty(itemciudad.Name) ? countryfirst.Name : itemciudad.Name;
            ViewBag.Country = countries;

            if (regbm.Company == null)
            {
                regbm.Company = new List<Models.Company>();
                regbm.Company.Add(new Models.Company());
            }

            if (regbm.PhoneNumber == null)
                regbm.PhoneNumber = regbm.Company.FirstOrDefault().Telefono;
            ViewBag.Title = "Contratante";
            //TempData["SkillsUser"] = regbm.Skills;
            return View("Edit", regbm);

        }

       
        // POST: Profile/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, FormCollection collection, RegisterBindingModel reg)
        {
            try
            {
                // TODO: Add update logic here

                ZonaFl.Business.SubSystems.SUser usern = new Business.SubSystems.SUser();
                ZonaFl.Business.SubSystems.SCategory usersk = new Business.SubSystems.SCategory();
                ZonaFl.Business.SubSystems.SSkill sskill = new Business.SubSystems.SSkill();
                RegisterBindingModel user = reg;
                if (user.Empresa == null)
                {
                    user.Empresa = false;
                }

                if (user.Freelance == null)
                {
                    user.Freelance = false;
                }

                RegisterBindingModel rmb = new RegisterBindingModel();
                rmb.Skills = user.Skills;
                List<Persistence.Entities.Skill> skills = rmb.Skills.Select(e => new Persistence.Entities.Skill().InjectFrom(e)).Cast<Persistence.Entities.Skill>().ToList();
                AspNetUsers aspuser = new AspNetUsers();
                var useru = UserManager.FindByEmail(user.Email);
                for (int i = 0; i < skills.Count(); i++)
                {
                    ZonaFl.Persistence.Entities.Category category = null;
                    var skill = sskill.FindSkillByName(user.Skills[i].Name);
                    string[] stringSeparators = new string[] { "\n" };
                    string result = user.Skills[i].CategorySkill.Split(stringSeparators, StringSplitOptions.None)[0];

                    category = usersk.FindCategoryByName(result);

                    if (category == null)
                    {
                        try
                        {
                            category = usersk.InsertCategory(result);
                        }
                        catch (Exception er)
                        {
                            return Json(new { success = false, issue = user, errors = er.Message, tipo = user, UserEmail = useru.Email });
                        }
                    }

                    skills[i].IdHtml = user.Skills[i].IdHtml;
                    skills[i].Category = category;
                    skills[i].CategoryId = category.Id;
                }

                useru.City = user.City;
                useru.Country = user.Country;
                useru.UserName = user.UserName;
                useru.DescUser = user.DescUser;
                useru.PhoneNumber = user.PhoneNumber.ToString();
                useru.FirstMiddleName = user.FirstMiddleName;
                useru.Image = user.Image;

                aspuser.InjectFrom(useru);

                try
                {
                    skills = usern.InsertSkillsByUser(skills, aspuser);
                }
                catch (Exception er)
                {
                    return Json(new { success = false, issue = user, errors = er.Message, tipo = aspuser, UserEmail = useru.Email });
                }

                try
                {

                    IdentityResult result = UserManager.Update(useru);

                    if (!result.Succeeded)
                    {
                        return View("Error");
                    }
                }
                catch (Exception er)
                {
                    return View("Error");
                    //return Json(new { success = false, issue = user, errors = er.Message, tipo = user, UserEmail = useru.Email });
                    //return InternalServerError(er);

                }

                return RedirectToAction("EditById", new { id = id });
                //return Json(new { success = true, issue = user, errors = "", tipo = aspuser, UserEmail = useru.Email });
                //return Ok();
            }
            catch
            {
                return View("Error");
            }
        }

        // GET: Profile/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Profile/Delete/5
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

        [HttpPost]
        public ActionResult EndPhase(int id)
        {
            try
            {
                // TODO: Add delete logic here

            string calification=    Request.QueryString["Cali"];
                string comentarios = Request.QueryString["Coment"];
                ZonaFl.Business.SubSystems.SOffer offern = new Business.SubSystems.SOffer();
                Persistence.Entities.Offer offer=offern.GetOfferByPhase(id);
                int? rta = 0;
                int endproject = 0;
                var ofertasfases = offern.GetOfferPhases(offer.Id);
                ofertasfases.Where(t => t.Id == id).ToList().ForEach(e => e.StatusPhase = Persistence.Entities.StatusPhase.Finalizada);
                //List<OfferPhases> ofertas = new List<OfferPhases>();
                //ofertas.Add(oferta);
                rta=offern.ChangeStatusPhases(ofertasfases);

               
                ZonaFl.Business.SubSystems.SProject sproject= new Business.SubSystems.SProject();
                if (sproject.GetStatusEndedProjectByOffer(offer.Id))
                {
                    endproject = 1;
                }
                foreach (var ofertap in ofertasfases)
                {
                    this.CommentandQualificationProject(ofertap.Id, ofertap.IdOffer,comentarios,calification,offer.IdUser);
                }

                return Json(new { success = rta, Oferta= ofertasfases.FirstOrDefault().IdOffer,Endproject= endproject });
                return Json(rta, JsonRequestBehavior.AllowGet);

                
            }
            catch( Exception  er)
            {
                return View();
            }
        }
       
        public ActionResult StartPhase(int id)
        {
            try
            {
                // TODO: Add delete logic here

                ZonaFl.Business.SubSystems.SOffer offern = new Business.SubSystems.SOffer();
                Persistence.Entities.Offer offer = offern.GetOfferByPhase(id);
                int? rta = 0;
                List<OfferPhases> ofertas = null;
               var offertasq= offern.GetOfferPhases(offer.Id);
                ofertas = offertasq.Count> 0? offertasq : new List<OfferPhases>(); ;
                ofertas.Where(t => t.Id == id).ToList().ForEach(e => e.StatusPhase = ZonaFl.Persistence.Entities.StatusPhase.EnCurso);
                 rta = offern.ChangeStatusPhases(ofertas);

              
                return RedirectToAction("DetailsById", new { id = offer.IdUser });
              


            }
            catch( Exception er)
            {
             
               
             
                Log4NetLogger logger2 = new Log4NetLogger();
                logger2.CurrentUser = SessionBag.Current.User.Id;
                if (Request != null)
                {
                    logger2.Error(er, Request.Path, Request.RawUrl);
                    throw new Exception(er.Message);
                }
                else
                {
                    logger2.Error(er);
                }

                return RedirectToAction("DetailsById", new { id = SessionBag.Current.User.Id });


            }
        }


        
        [HttpPost]
        public ActionResult CommentandQualificationProject(int id, int idOffer,string Comments, string Qualification, string iduser)
        {
            try
            {
               

                ZonaFl.Business.SubSystems.SProject projectn = new Business.SubSystems.SProject();
               var project= projectn.GetByOffer(idOffer);
                project.Comments = Comments;
                project.Qualification = short.Parse(Qualification);
                projectn.Update(project);
                return RedirectToAction("DetailsById",new {id=iduser });
            }
            catch
            {
                return View();
            }
        }




    }
}
