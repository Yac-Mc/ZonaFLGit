using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZonaFl.Business.SubSystems;
using ZonaFl.Controllers.Filters;
using ZonaFl.Models;

namespace ZonaFl.Controllers
{
    public class CurriculumController : Controller
    {
        // GET: Curriculum
        public ActionResult Index()
        {
            return View();
        }

        // GET: Curriculum/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        [IsLogout]
        public ActionResult DetailsById(string id)
        {

          
            ZonaFl.Persistence.Entities.AspNetUsers useru = null;
            ZonaFl.Business.SubSystems.SUser usern = new Business.SubSystems.SUser();
            useru = usern.GetCurriculumUserById(new Guid(id));
            if (SessionBag.Current.User == null)
            {
                SessionBag.Current.User = useru;
            }
            Curriculum curr = new Curriculum();
            RegisterBindingModel regbm = new RegisterBindingModel();
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
            if (useru.Image != null && useru.Image.Contains("fakepath"))
            {
                useru.Image = useru.Image.Replace(@"C:\fakepath\", "");

            }

            ViewBag.UserId = id;
            ViewBag.User = useru.UserName;
            ViewBag.ImageUser = useru.Image;
           
            ViewBag.IdUser = id;

                return View("Details", curr);
            


        }
        // GET: Profile/Edit/5
        [IsLogout]
        public ActionResult EditById(string id)
        {
            ZonaFl.Persistence.Entities.AspNetUsers useru = null;
            ZonaFl.Business.SubSystems.SUser usern = new Business.SubSystems.SUser();
            useru = usern.GetCurriculumUserById(new Guid(id));
            if (SessionBag.Current.User == null)
            {
                SessionBag.Current.User = useru;
            }
            Curriculum curr = new Curriculum();
            RegisterBindingModel regbm = new RegisterBindingModel();
            regbm.InjectFrom(useru);
            regbm.Skills = useru.Skills.Select(e => new ZonaFl.Models.Skill().InjectFrom(e)).Cast<ZonaFl.Models.Skill>().ToList();
            curr.User = regbm;
            curr.Experiences = useru.Experiences.Select(e => new ZonaFl.Models.Experience().InjectFrom(e)).Cast<ZonaFl.Models.Experience>().ToList();
            curr.Certifications = useru.Certifications.Select(e => new ZonaFl.Models.Certification().InjectFrom(e)).Cast<ZonaFl.Models.Certification>().ToList();
            curr.Educations = useru.Certifications.Select(e => new ZonaFl.Models.Education().InjectFrom(e)).Cast<ZonaFl.Models.Education>().ToList();
            curr.Languages = useru.Certifications.Select(e => new ZonaFl.Models.Language().InjectFrom(e)).Cast<ZonaFl.Models.Language>().ToList();

            ZonaFl.Business.SubSystems.SProject userp = new Business.SubSystems.SProject();
            // trae las calificacion promedio del usuario por los proyectos
            int calif = userp.GetCalificationAverageUser(id);
            //calif = 3;


            var projects = userp.GetProjectsEndedByUser(id);
            List<Models.Project> projetsm = new List<Models.Project>();

            projetsm = projects.Select(e => new Models.Project().InjectFrom(e)).Cast<Models.Project>().ToList();
            projetsm.Select(e => new Models.Project().InjectFrom(usern.GetUserByOffer(e.IdOffer, 0))).Cast<Models.Project>().ToList();

            ViewBag.Calification = calif;
            ViewBag.Commentaries = projetsm;


            ViewBag.User = useru.UserName;
            ViewBag.ImageUser = useru.Image;
            ViewBag.IdUser = id;
            return View("Edit", curr);

        }


        [HttpGet]
        [IsLogout]
        public ActionResult GetExperienceLabByUser()
        {

            var iduser = RouteData.Values["id"].ToString();
            ZonaFl.Business.SubSystems.SUser usern = new Business.SubSystems.SUser();
            List<Persistence.Entities.Experience> experiences = usern.GetExperienceLabByUser(iduser);
            Models.Experience experiencem= new Experience();
            int i = 0;
            foreach (var expe in experiences)
            {
                if (i == 0)
                {
                    experiencem.Company = expe.Company;
                    experiencem.CurrentEmploy = expe.CurrentEmploy;
                    experiencem.Position = expe.Position;
                    experiencem.DateIni = expe.DateIni.ToString();
                    experiencem.DateEnd = expe.DateEnd.ToString();
                    experiencem.FunctionPosition = expe.FunctionPosition;
                    experiencem.UserId = iduser;
                }
                else if (i == 1)
                {
                    experiencem.Company2 = expe.Company;
                    experiencem.CurrentEmploy2 = expe.CurrentEmploy;
                    experiencem.Position2 = expe.Position;
                    experiencem.DateIni2 = expe.DateIni.ToString();
                    experiencem.DateEnd2 = expe.DateEnd.ToString();
                    experiencem.FunctionPosition2 = expe.FunctionPosition;
                    
                }
                else
                {
                    experiencem.Company3 = expe.Company;
                    experiencem.CurrentEmploy3 = expe.CurrentEmploy;
                    experiencem.Position3 = expe.Position;
                    experiencem.DateIni3 = expe.DateIni.ToString();
                    experiencem.DateEnd3 = expe.DateEnd.ToString();
                    experiencem.FunctionPosition3 = expe.FunctionPosition;

                }
                i += 1;
            }


            //var rta= Json(data: experiences);

            return Json(experiencem, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        [IsLogout]
        public ActionResult GetEducationByUser()
        {

            var iduser = RouteData.Values["id"].ToString();
            ZonaFl.Business.SubSystems.SUser usern = new Business.SubSystems.SUser();
            List<Persistence.Entities.Education> educations = usern.GetEducationByUser(iduser);
            Models.Education educationsm = new Education();

            SCountry sco = new SCountry();
            int i = 0;
           foreach(var edu in educations)
            {
               

               if (i==0)
                {
                    int contry = edu.Country!=null?(int)edu.Country:-1;
                    educationsm.Institution = edu.Institution;
                    educationsm.Country = edu.Country != null?sco.FindCountrybyId(contry).Name:"";
                    educationsm.DateEndE =edu.DateEnd.ToString();
                    educationsm.DateIniE = edu.DateIni.ToString();
                    educationsm.Actually = edu.Actually==null? false : (bool)edu.Actually;
                    educationsm.Title = edu.Title;
                    educationsm.UserId = iduser;
                }

                if (i == 1)
                {
                    int contry2 = edu.Country != null ? (int)edu.Country : -1;
                    educationsm.Institution2 = edu.Institution;
                    educationsm.Country2 = edu.Country != null ? sco.FindCountrybyId(contry2).Name : "";
                    educationsm.DateEnd2E = edu.DateEnd.ToString();
                    educationsm.DateIni2E = edu.DateIni.ToString();
                    educationsm.Actually2 = edu.Actually == null ? false : (bool)edu.Actually;
                    educationsm.Title2 = edu.Title;
                   

                }

                if (i == 2)
                {
                    int contry3 = edu.Country != null ? (int)edu.Country : -1;
                    educationsm.Institution3 = edu.Institution;
                    educationsm.Country3 = edu.Country != null ? sco.FindCountrybyId(contry3).Name : "";
                    educationsm.DateEnd3E = edu.DateEnd.ToString();
                    educationsm.DateIni3E = edu.DateIni.ToString();
                    educationsm.Actually3 = edu.Actually == null ? false : (bool)edu.Actually;
                    educationsm.Title3 = edu.Title;
                }
                i += 1;

            }


            
            return Json(educationsm, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        [IsLogout]
        public ActionResult GetCertificationsByUser()
        {

            var iduser = RouteData.Values["id"].ToString();
            ZonaFl.Business.SubSystems.SUser usern = new Business.SubSystems.SUser();
            List<Persistence.Entities.Certification> experiences = usern.GetCertificationsByUser(iduser);
            Certification certm = new Certification();
            int i = 0;
            foreach (var edu in experiences)
            {
                if (i == 0)
                {
                    certm.Description = edu.Description;
                    certm.Certificate = edu.Certificate;
                    certm.Actually = edu.Actually;
                    certm.DateCert = edu.DateCert.ToString();
                    certm.Otorgante = edu.Otorgante;
                    certm.UserId = edu.Otorgante;
                }
                else if(i == 1)
                {
                    certm.Description2 = edu.Description;
                    certm.Certificate2 = edu.Certificate;
                    certm.Actually2 = edu.Actually;
                    certm.DateCert2 = edu.DateCert.ToString();
                    certm.Otorgante2 = edu.Otorgante;
                   
                }
                else if (i == 2)
                {
                    certm.Description3 = edu.Description;
                    certm.Certificate3 = edu.Certificate;
                    certm.Actually3 = edu.Actually;
                    certm.DateCert3 = edu.DateCert.ToString();
                    certm.Otorgante3 = edu.Otorgante;
                }
                i += 1;
            }
                return Json(certm, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        [IsLogout]
        public ActionResult GetLanguagesByUser()
        {
            var iduser = RouteData.Values["id"].ToString();
            ZonaFl.Business.SubSystems.SUser usern = new Business.SubSystems.SUser();
            List<Persistence.Entities.Language> languages = usern.GetLanguagesByUser(iduser);
            Language languagem = new Language();
            int i = 0;
            foreach (var edu in languages)
            {
                if (i == 0)
                {
                    if(edu.Certificate!=null)
                    languagem.Certificate = (bool)edu.Certificate;
                    languagem.LevelLang = edu.LevelLang;
                    languagem.Name = edu.Name;
                    languagem.UserId = edu.UserId;
                }
                else if(i==1)
                {
                    if (edu.Certificate != null)
                        languagem.Certificate2 = (bool)edu.Certificate;
                    languagem.LevelLang2 = edu.LevelLang;
                    languagem.Name2 = edu.Name;
                }
                else
                {
                    if (edu.Certificate != null)
                        languagem.Certificate3 = (bool)edu.Certificate;
                    languagem.LevelLang3 = edu.LevelLang;
                    languagem.Name3 = edu.Name;
                }
                i += 1;
            }

                    return Json(languagem, JsonRequestBehavior.AllowGet);
        }







        [IsLogout]
        public ActionResult InsertExperienceLab(Experience experience)
        {

            ZonaFl.Persistence.Entities.AspNetUsers useru = null;
            ZonaFl.Business.SubSystems.SUser usern = new Business.SubSystems.SUser();
            useru = usern.GetCurriculumUserById(new Guid(experience.UserId));
            List<Persistence.Entities.Experience> experiences = new List<Persistence.Entities.Experience>();
            if (experience.Company != null)
            {
                Persistence.Entities.Experience experiencen1 = new Persistence.Entities.Experience()
                {
                    Company = experience.Company,
                    CurrentEmploy = experience.CurrentEmploy,
                    DateEnd = DateTime.Parse(experience.DateEnd),
                    DateIni = DateTime.Parse(experience.DateIni),
                    FunctionPosition = experience.FunctionPosition,
                    Position = experience.Position,
                    UserId = experience.UserId

                };
                experiences.Add(experiencen1);
            }
            if(experience.Company2!=null)
            { 
                Persistence.Entities.Experience experiencen2 = new Persistence.Entities.Experience()
                {
                    Company = experience.Company2,
                    CurrentEmploy = experience.CurrentEmploy2,
                    DateEnd = DateTime.Parse(experience.DateEnd2),
                    DateIni = DateTime.Parse(experience.DateIni2),
                    FunctionPosition = experience.FunctionPosition2,
                    Position = experience.Position2,
                    UserId = experience.UserId

                };
                experiences.Add(experiencen2);

            }

            if (experience.Company3 != null)
            {
                Persistence.Entities.Experience experiencen3 = new Persistence.Entities.Experience()
                {
                    Company = experience.Company3,
                    CurrentEmploy = experience.CurrentEmploy3,
                    DateEnd = DateTime.Parse(experience.DateEnd3),
                    DateIni = DateTime.Parse(experience.DateIni3),
                    FunctionPosition = experience.FunctionPosition3,
                    Position = experience.Position3,
                    UserId = experience.UserId

                };
                experiences.Add(experiencen3);
            }
           
               
               
               
                bool rta = usern.InsertExperienceLab(experiences);
                return Json(data: rta);

            }




        [IsLogout]
        public ActionResult InsertEducation(Education education)
        {

            ZonaFl.Persistence.Entities.AspNetUsers useru = null;
            ZonaFl.Business.SubSystems.SUser usern = new Business.SubSystems.SUser();
            useru = usern.GetCurriculumUserById(new Guid(education.UserId));
            List<Persistence.Entities.Education> educations = new List<Persistence.Entities.Education>();
            CultureInfo provider = CultureInfo.CurrentCulture;
            SCountry sco = new SCountry();
            if (education.Institution != null)
            {
                Persistence.Entities.Education education1 = new ZonaFl.Persistence.Entities.Education()
                {

                    Institution = education.Institution,
                    Country = sco.FindCountrybyName(education.Country).Id,
                    Title = education.Title,
                    DateIni = DateTime.Parse(education.DateIniE, provider),
                    DateEnd = DateTime.Parse(education.DateEndE, provider),
                    Actually = education.Actually,
                    UserId = education.UserId


                };
                educations.Add(education1);
            }
            if (education.Institution2 != null)
            {
                Persistence.Entities.Education education2 = new ZonaFl.Persistence.Entities.Education()
                {

                    Institution = education.Institution2,
                    Country = education.Country2 != null ? sco.FindCountrybyName(education.Country2).Id:-1,
                    Title = education.Title2,
                    DateIni = DateTime.Parse(education.DateIni2E),
                    DateEnd = DateTime.Parse(education.DateEnd2E),
                    Actually = education.Actually2,
                    UserId = education.UserId


                };
                educations.Add(education2);
            }

            if (education.Institution3 != null)
            {
                Persistence.Entities.Education education3 = new ZonaFl.Persistence.Entities.Education()
                {

                    Institution = education.Institution3,
                    Country = education.Country3 != null ? sco.FindCountrybyName(education.Country3).Id : -1,
                    Title = education.Title3,
                    DateIni = DateTime.Parse(education.DateIni3E),
                    DateEnd = DateTime.Parse(education.DateEnd3E),
                    Actually = education.Actually3,
                    UserId = education.UserId


                };
                educations.Add(education3);
            }

           
           
            bool rta = usern.InsertEducation(educations);
            return Json(data: rta);


        }


        [IsLogout]
        public ActionResult InsertCertificationLab(Certification certification)
        {
            ZonaFl.Persistence.Entities.AspNetUsers useru = null;
            ZonaFl.Business.SubSystems.SUser usern = new Business.SubSystems.SUser();
            useru = usern.GetCurriculumUserById(new Guid(certification.UserId));
            List<Persistence.Entities.Certification> certifications = new List<Persistence.Entities.Certification>();

            if (certification.Certificate != null)
            {
                Persistence.Entities.Certification certification1 = new ZonaFl.Persistence.Entities.Certification()
                {
                    Certificate = certification.Certificate,
                    DateCert = DateTime.Parse( certification.DateCert),
                    Otorgante = certification.Otorgante,
                    Description = certification.Description,
                    UserId = certification.UserId,
                    Actually = certification.Actually



                };
                certifications.Add(certification1);
            }
            if (certification.Certificate2 != null)
            {
                Persistence.Entities.Certification certification2 = new ZonaFl.Persistence.Entities.Certification()
                {
                    Certificate = certification.Certificate2,
                    DateCert = DateTime.Parse(certification.DateCert2),
                    Otorgante = certification.Otorgante2,
                    Description = certification.Description2,
                    UserId = certification.UserId2,
                    Actually = certification.Actually2



                };
                certifications.Add(certification2);
            }

            if (certification.Certificate3 != null)
            {
                Persistence.Entities.Certification certification3 = new ZonaFl.Persistence.Entities.Certification()
                {
                    Certificate = certification.Certificate3,
                    DateCert = DateTime.Parse(certification.DateCert3),
                    Otorgante = certification.Otorgante3,
                    Description = certification.Description3,
                    UserId = certification.UserId3,
                    Actually = certification.Actually3



                };
                certifications.Add(certification3);

            }



            bool rta = usern.InsertCertifications(certifications);
            return Json(data: rta);
        }
        [IsLogout]
        public ActionResult InsertLanguageLab(Language language)
        {
            ZonaFl.Persistence.Entities.AspNetUsers useru = null;
            ZonaFl.Business.SubSystems.SUser usern = new Business.SubSystems.SUser();
            useru = usern.GetCurriculumUserById(new Guid(language.UserId));
            List<Persistence.Entities.Language> languages = new List<Persistence.Entities.Language>();

            if (language.Name != null)
            {
                Persistence.Entities.Language language1 = new ZonaFl.Persistence.Entities.Language()
                {
                    Name = language.Name,
                    LevelLang = language.LevelLang,
                    Certificate = language.Certificate,
                    UserId = language.UserId

                };
                languages.Add(language1);
            }

            if (language.Name2 != null)
            {
                Persistence.Entities.Language language2 = new ZonaFl.Persistence.Entities.Language()
                {
                    Name = language.Name2,
                    LevelLang = language.LevelLang2,
                    Certificate = language.Certificate2,
                    UserId = language.UserId

                };
                languages.Add(language2);
            }

            if (language.Name3 != null)
            {
                Persistence.Entities.Language language3 = new ZonaFl.Persistence.Entities.Language()
                {
                    Name = language.Name3,
                    LevelLang = language.LevelLang3,
                    Certificate = language.Certificate3,
                    UserId = language.UserId

                };
                languages.Add(language3);
            }

            
            
            bool rta = usern.InsertLanguages(languages);
            return Json(data: rta);
        }
        

        // GET: Curriculum/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Curriculum/Create
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

        // GET: Curriculum/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Curriculum/Edit/5
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

        // GET: Curriculum/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Curriculum/Delete/5
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
