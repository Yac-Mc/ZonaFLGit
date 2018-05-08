using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZonaFl.Models;
using Omu.ValueInjecter;
using ZonaFl.Persistence.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using ZonaFl.Controllers.Filters;
using ZonaFl.Business.SubSystems;
using ZonaFl.Business;

namespace ZonaFl.Controllers
{
    public class FreelanceController : Controller
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


        public ActionResult ReadForEmployer(string id)
        {
            SUser suser = new SUser();
            SPortfolio sportfolio = new SPortfolio();
            RegisterBindingModel regbm = new RegisterBindingModel();
            try
            {
                var portfolio = sportfolio.GetPortFolioByUserId(id);


                ZonaFl.Persistence.Entities.AspNetUsers useru = null;
                ZonaFl.Business.SubSystems.SUser usern = new Business.SubSystems.SUser();
                useru = usern.GetCurriculumUserById(new Guid(id));

                ZonaFl.Business.SubSystems.SProject userp = new Business.SubSystems.SProject();
                // trae las calificacion promedio del usuario por los proyectos
                int calif = userp.GetCalificationAverageUser(id);
                //calif = 3;


                var projects = userp.GetProjectsEndedByUser(id);
                List<Models.Project> projetsm = new List<Models.Project>();

                projetsm = projects.Select(e => new Models.Project().InjectFrom(e)).Cast<Models.Project>().ToList();
                projetsm.Select(e => new Models.Project().InjectFrom(usern.GetUserByOffer(e.IdOffer, 0))).Cast<Models.Project>().ToList();

                Curriculum curr = new Curriculum();
                
                regbm.InjectFrom(useru);
                regbm.Skills = useru.Skills.Select(e => new ZonaFl.Models.Skill().InjectFrom(e)).Cast<ZonaFl.Models.Skill>().ToList();
                curr.User = regbm;
                curr.Experiences = useru.Experiences.Select(e => new ZonaFl.Models.Experience().InjectFrom(e)).Cast<ZonaFl.Models.Experience>().ToList();
                curr.Experiences.ForEach(e => e.DateIni = useru.Experiences.Where(t => t.Id == e.Id).FirstOrDefault().DateIni.ToString());
                curr.Experiences.ForEach(e => e.DateEnd = useru.Experiences.Where(t => t.Id == e.Id).FirstOrDefault().DateEnd.ToString());
                curr.Experiences.ForEach(e => e.DateIni2 = useru.Experiences.Where(t => t.Id == e.Id).FirstOrDefault().DateIni.ToString());
                curr.Experiences.ForEach(e => e.DateEnd2 = useru.Experiences.Where(t => t.Id == e.Id).FirstOrDefault().DateEnd.ToString());
                curr.Experiences.ForEach(e => e.DateIni3 = useru.Experiences.Where(t => t.Id == e.Id).FirstOrDefault().DateIni.ToString());
                curr.Experiences.ForEach(e => e.DateEnd3 = useru.Experiences.Where(t => t.Id == e.Id).FirstOrDefault().DateEnd.ToString());

                curr.Certifications = useru.Certifications.Select(e => new ZonaFl.Models.Certification().InjectFrom(e)).Cast<ZonaFl.Models.Certification>().ToList();
                curr.Certifications.ForEach(e => e.DateCert = useru.Certifications.Where(t => t.Id == e.Id).FirstOrDefault().DateCert.ToString());
                curr.Certifications.ForEach(e => e.DateCert2 = useru.Certifications.Where(t => t.Id == e.Id).FirstOrDefault().DateCert.ToString());
                curr.Certifications.ForEach(e => e.DateCert3 = useru.Certifications.Where(t => t.Id == e.Id).FirstOrDefault().DateCert.ToString());



                curr.Educations = useru.Educations.Select(e => new ZonaFl.Models.Education().InjectFrom(e)).Cast<ZonaFl.Models.Education>().ToList();
                curr.Educations.ForEach(e => e.DateIniE = useru.Educations.Where(t => t.Id == e.Id).FirstOrDefault().DateIni.ToString());
                curr.Educations.ForEach(e => e.DateEndE = useru.Educations.Where(t => t.Id == e.Id).FirstOrDefault().DateEnd.ToString());
                curr.Educations.ForEach(e => e.DateIni2E = useru.Educations.Where(t => t.Id == e.Id).FirstOrDefault().DateIni.ToString());
                curr.Educations.ForEach(e => e.DateEnd2E = useru.Educations.Where(t => t.Id == e.Id).FirstOrDefault().DateEnd.ToString());
                curr.Educations.ForEach(e => e.DateIni3E = useru.Educations.Where(t => t.Id == e.Id).FirstOrDefault().DateIni.ToString());
                curr.Educations.ForEach(e => e.DateEnd3E = useru.Educations.Where(t => t.Id == e.Id).FirstOrDefault().DateEnd.ToString());


                curr.Languages = useru.Languages.Select(e => new ZonaFl.Models.Language().InjectFrom(e)).Cast<ZonaFl.Models.Language>().ToList();

                regbm.Curriculum = curr;




                ViewBag.UserId = id;
                ViewBag.User = SessionBag.Current.User.UserName;
                ViewBag.ImageUser = SessionBag.Current.User.Image;
                ViewBag.ImageUserFrelance = useru.Image;
                ViewBag.Portfolio = portfolio;
                ViewBag.Calification = calif;
                ViewBag.Commentaries = projetsm;



                
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

            return View(regbm);

        }

        [IsLogout]
        public ActionResult Read(string id)
        {
            ZonaFl.Persistence.Entities.AspNetUsers useru = null;
            ZonaFl.Business.SubSystems.SUser usern = new Business.SubSystems.SUser();
            useru = usern.GetUserById(new Guid(id));

            RegisterBindingModel regbm = new RegisterBindingModel();
            regbm.InjectFrom(useru);
            regbm.Skills = useru.Skills.Where(e => e.Visible).Select(e => new ZonaFl.Models.Skill().InjectFrom(e)).Cast<ZonaFl.Models.Skill>().ToList();
            SessionBag.Current.User = regbm;
            ViewBag.User = useru.UserName;
            ViewBag.IdUser = useru.Id;
            ViewBag.ImageUser = useru.Image;

            return View("ReadForAdmin", regbm);
        }

        [IsLogout]
        public ActionResult DetailsByEmail(string id)
        {




            ZonaFl.Persistence.Entities.AspNetUsers useru = null;
            ZonaFl.Business.SubSystems.SUser usern = new Business.SubSystems.SUser();
            ZonaFl.Business.SubSystems.SProject userp = new Business.SubSystems.SProject();
            // trae las calificacion promedio del usuario por los proyectos
            useru = usern.GetUserByEmail(id);
            int calif = userp.GetCalificationAverageUser(useru.Id);
            //calif = 3;
            ViewBag.Calification = calif;

            var projects = userp.GetProjectsEndedByUser(useru.Id);
            List<Models.Project> projetsm = new List<Models.Project>();

            projetsm = projects.Select(e => new Models.Project().InjectFrom(e)).Cast<Models.Project>().ToList();
            projetsm.Select(e => new Models.Project().InjectFrom(usern.GetUserByOffer(e.IdOffer, 0))).Cast<Models.Project>().ToList();
            //projetsm.ForEach(e => e.CommentsUser = usern.GetUserNameByOffer(e.IdOffer).UserName);
            //projetsm.ForEach(e => e.Image = usern.GetUserNameByOffer(e.IdOffer).Image);
            //-prueba

            // projetsm.Add(new Models.Project() {Comments="Comentario Prueba", UserName = "Jaun fdo catro1",Image= "FotoJuanFdoCastro3.jpg" }
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
            useru = usern.GetUserById(new Guid(id));
            if (useru.Image != null && useru.Image.Contains("fakepath"))
            {
                useru.Image = useru.Image.Replace(@"C:\fakepath\", "");

            }
            RegisterBindingModel regbm = new RegisterBindingModel();
            regbm.InjectFrom(useru);
            regbm.Skills = useru.Skills.Where(e => e.Visible).Select(e => new ZonaFl.Models.Skill().InjectFrom(e)).Cast<ZonaFl.Models.Skill>().ToList();
            SessionBag.Current.User = regbm;
            ViewBag.User = useru.UserName;
            ViewBag.IdUser = useru.Id;
            ViewBag.ImageUser = useru.Image;

            return View("Details", regbm);

        }

        [IsLogout]
        public ActionResult DetailsById(string id)
        {




            ZonaFl.Persistence.Entities.AspNetUsers useru = null;
            ZonaFl.Business.SubSystems.SUser usern = new Business.SubSystems.SUser();
            ZonaFl.Business.SubSystems.SProject userp = new Business.SubSystems.SProject();
            // trae las calificacion promedio del usuario por los proyectos
            int calif = userp.GetCalificationAverageUser(id);
            //calif = 3;
            ViewBag.Calification = calif;

            var projects = userp.GetProjectsEndedByUser(id);
            List<Models.Project> projetsm = new List<Models.Project>();

            projetsm = projects.Select(e => new Models.Project().InjectFrom(e)).Cast<Models.Project>().ToList();
            projetsm.Select(e => new Models.Project().InjectFrom(usern.GetUserByOffer(e.IdOffer,0))).Cast<Models.Project>().ToList();
            //projetsm.ForEach(e => e.CommentsUser = usern.GetUserNameByOffer(e.IdOffer).UserName);
            //projetsm.ForEach(e => e.Image = usern.GetUserNameByOffer(e.IdOffer).Image);
            //-prueba

           // projetsm.Add(new Models.Project() {Comments="Comentario Prueba", UserName = "Jaun fdo catro1",Image= "FotoJuanFdoCastro3.jpg" }
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
            useru = usern.GetUserById(new Guid(id));
             if(useru.Image!=null && useru.Image.Contains("fakepath"))
                 {
                useru.Image=useru.Image.Replace(@"C:\fakepath\", "");

            }
            RegisterBindingModel regbm = new RegisterBindingModel();
            regbm.InjectFrom(useru);
            regbm.Skills=useru.Skills.Where(e=>e.Visible).Select(e => new ZonaFl.Models.Skill().InjectFrom(e)).Cast<ZonaFl.Models.Skill>().ToList();
            SessionBag.Current.User = regbm;
            ViewBag.User = useru.UserName;
            ViewBag.IdUser = useru.Id;
            ViewBag.ImageUser = useru.Image;
            
                return View("Details", regbm);
           
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
            
            ZonaFl.Persistence.Entities.AspNetUsers useru = null;
            ZonaFl.Business.SubSystems.SUser usern = new Business.SubSystems.SUser();
            ZonaFl.Business.SubSystems.SCategory scategory = new Business.SubSystems.SCategory();

            ZonaFl.Business.SubSystems.SCountry scountry = new Business.SubSystems.SCountry();

           useru = usern.GetUserById(new Guid(id));
            if (SessionBag.Current.User == null)
            {
                SessionBag.Current.User = useru;
            }

            RegisterBindingModel regbm = new RegisterBindingModel();

            regbm.InjectFrom(useru);
            regbm.Skills = useru.Skills.Select(e => new ZonaFl.Models.Skill().InjectFrom(e)).Cast<ZonaFl.Models.Skill>().ToList();
            regbm.Categories = useru.Skills.Select(e=>new ZonaFl.Models.Category().InjectFrom(scategory.FindCategoryById(e.CategoryId))).Cast<ZonaFl.Models.Category>().GroupBy(t=>t.Id).Select(g=>g.First()).ToList();
           regbm.Categories.ForEach(e => e.Skills = useru.Skills.Where(w=>w.CategoryId==e.Id).Select(y => new ZonaFl.Models.Skill().InjectFrom(y)).Cast<ZonaFl.Models.Skill>().ToList());
           
                ViewBag.CountAllCategories = regbm.Skills!=null?regbm.Skills.Where(e => e.Visible).Count():0;

            //var query = regbm.Categories.Select(s => s.Skills.Where(v => v.Visible).GroupBy(c => c.CategoryId)).SelectMany(r => r).ToList();


            var query = from skill in regbm.Skills
                where skill.Visible
                group skill by skill.CategoryId
                into grouping
                select new {Key = grouping.Key, Count = grouping.Count()};
            foreach (var VARIABLE in query)
            {
                regbm.GetType().GetProperty("Count"+VARIABLE.Key.ToString()).SetValue(regbm, VARIABLE.Count);
            }

            ViewBag.Skills= regbm.Skills;
            ViewBag.Categories = regbm.Categories;
           ViewBag.User = useru.UserName;
            ViewBag.IdUser = useru.Id;
            ViewBag.ImageUser = useru.Image;
            Persistence.Entities.City itemciudad = null;
            Persistence.Entities.Country item = null;
            if (useru.City != null)
            {
                itemciudad = scountry.FindCityByName(useru.City);
            }
            else
            {
                itemciudad= scountry.FindCityByName("Bogota");
            }
            if (useru.Country != null)
            {
                item = scountry.FindCountrybyName(useru.Country);
            }
            else
            {
                item = scountry.FindCountrybyName("Colombia");
            }
            List<SelectListItem> countries = null;
            if (item != null)
            {
                var items = scountry.FindAll().Where(e => e.Id != item.Id).ToList();

                items.Insert(0, item);
                countries = new SelectList(items, "ID", "Name").ToList();
            }
            else
            {
                var items = scountry.FindAll();
                countries = new SelectList(items, "ID", "Name").ToList();
                item = items.FirstOrDefault();
            }
           
            ViewBag.Countrysel = item.Id;
            ViewBag.Ciudadsel = itemciudad.Name;
            ViewBag.Country = countries;
            TempData["SkillsUser"] = regbm.Skills;
            ViewBag.Title = "Freelance";
            return View("Edit", regbm);
           
        }
        
        public ActionResult Logout()
        {
            SessionBag.Current.User = null;
            SessionBag.Current.Logout = true;
            return RedirectToAction("Index","Home", new { accion = "logout" });
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
                List<Persistence.Entities.Skill> skills = new List<Persistence.Entities.Skill>();
                rmb.Skills = user.Skills;
                if (user.Skills != null)
                {
                   skills = rmb.Skills.Select(e => new Persistence.Entities.Skill().InjectFrom(e)).Cast<Persistence.Entities.Skill>().ToList();
                }
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

                    IdentityResult result =  UserManager.Update(useru);

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
            catch (Exception er)
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
    }
}
