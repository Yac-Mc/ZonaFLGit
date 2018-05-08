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
    public class SUser
    {
        public SUser()
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

        public List<AspNetUsers> GetAllUsers()
        {
            UserRepository userrepo = new UserRepository();
            return userrepo.FindAll().ToList();
        }

        public AspNetUsers GetUserById(Guid id)
        {
            UserRepository userrepo = new UserRepository();
            UserRepository2<AspNetUsers> userrepo2 = new UserRepository2<AspNetUsers>();
            CompanyRepository companyrepo = new CompanyRepository();
            var skillsuser=userrepo.FindSkillsByUserID(id);
            //AspNetUsers user= userrepo2.Get(id);
            AspNetUsers user= userrepo.FindByID(id);
            SCategory scate = new SCategory();
            foreach (var skilluser in skillsuser)
            {
                user.Skills.Where(e => e.Id == skilluser.Id).FirstOrDefault().Visible = true;
                user.Skills.Where(e => e.Id == skilluser.Id).FirstOrDefault().Category = scate.FindCategoryById(skilluser.CategoryId);
            }
            user.Companies = new List<Company>();
            user.Companies.Add(companyrepo.GetList(new { IdUser = id.ToString() }).FirstOrDefault());

            //UserRepository2<AspNetUsers> userrepo = new UserRepository2<AspNetUsers>();
            if (user.Image != null && user.Image.Contains("fakepath"))
            {
                user.Image = user.Image.Replace(@"C:\fakepath\", "");

            }

            user.PagosConfirmed = (user.Offers.Select(e => e.OfferPhases.Where(x => x.StatusPhase == StatusPhase.EnCurso)).FirstOrDefault()!=null)? true: false;
          


            return user;
           

        }

        public bool DeleteUser(Guid guid)
        {
            Business.Log4NetLogger logger2 = new Business.Log4NetLogger();

            UserRepository userrepo = new UserRepository();
            try
            {
                var rta = userrepo.DeleteAccount(guid);
                logger2.Info("Cuenta eliminada:" + guid);
                
                return true;
            }
            catch (Exception er)
            {
                var mes = er.Message;
                

                logger2.Error(er);
                return false;  
            }

            
        }

        public AspNetUsers GetUserByOffer(int idOffer, short freelance)
        {
            UserRepository userrepo = new UserRepository();
           return userrepo.GetUserByOffer(idOffer, freelance);
        }

        public AspNetUsers GetUserByEmail(string Email)
        {
            UserRepository userrepo = new UserRepository();
           
            AspNetUsers user = userrepo.FindByEmail(Email);
            var skillsuser = userrepo.FindSkillsByUserID(new Guid(user.Id));
           

            foreach (var skilluser in skillsuser)
            {

               var skill= user.Skills.Where(e => e.Id == skilluser.Id).FirstOrDefault();
                if(skill!=null)
                user.Skills.Where(e => e.Id == skilluser.Id).FirstOrDefault().Visible = true;

            }
            CompanyRepository companyrepo = new CompanyRepository();
            user.Companies = new List<Company>();
            user.Companies.Add(companyrepo.GetList(new { IdUser = user.Id.ToString() }).FirstOrDefault());

            //UserRepository2<AspNetUsers> userrepo = new UserRepository2<AspNetUsers>();

            return user;

        }

        public List<ZonaFl.Persistence.Entities.Experience> GetExperienceLabByUser(string iduser)
        {
            UserRepository userrepo = new UserRepository();
            List<ZonaFl.Persistence.Entities.Experience> experiences = new List<Experience>();
            try
            {
                experiences =userrepo.GetExperienceLabByUser(iduser).ToList();
                return experiences;

            }
            catch (Exception er)
            {
                return experiences;
            }
        }

        public List<Certification> GetCertificationsByUser(string iduser)
        {
            UserRepository userrepo = new UserRepository();
            List<ZonaFl.Persistence.Entities.Certification> certificaciones = new List<Certification>();
            try
            {
                certificaciones = userrepo.GetCertificationsByUser(iduser).ToList();
                return certificaciones;

            }
            catch (Exception er)
            {
                return certificaciones;
            }
        }

        public List<Language> GetLanguagesByUser(string iduser)
        {
            UserRepository userrepo = new UserRepository();
            List<ZonaFl.Persistence.Entities.Language> languages = new List<Language>();
            try
            {
                languages = userrepo.GetLanguagesByUser(iduser).ToList();
                return languages;

            }
            catch (Exception er)
            {
                return languages;
            }
        }

        public List<Education> GetEducationByUser(string iduser)
        {
            UserRepository userrepo = new UserRepository();
            List<ZonaFl.Persistence.Entities.Education> educations = new List<Education>();
            try
            {
                educations = userrepo.GetEducationByUser(iduser).ToList();
                return educations;

            }
            catch (Exception er)
            {
                return educations;
            }
        }

        public bool InsertExperienceLab(List<ZonaFl.Persistence.Entities.Experience> experiences)
        {
            UserRepository userrepo = new UserRepository();
            ExperienceRepository<Experience> exprepo = new ExperienceRepository<Experience>();
           var expact = userrepo.GetExperienceLabByUser(experiences.FirstOrDefault().UserId);
           
            foreach (var exp in expact)
            {
                exprepo.Delete(exp.Id);
            }
            try
            {
                userrepo.InsertExperienceLab(experiences);
               
            }
            catch(Exception  er)
            {
                return false;
            }

            return true;
        }

        public AspNetUsers GetCurriculumUserById(Guid id)
        {
            UserRepository userrepo = new UserRepository();


            return userrepo.FindCurriculumByIDUser(id);


        }

        public List<ZonaFl.Persistence.Entities.Skill> InsertSkillsByUser(List<ZonaFl.Persistence.Entities.Skill> skills,AspNetUsers user)
        {
            
            UserRepository userrepo = new UserRepository();
            userrepo.Update(user);
            return userrepo.AddUserSkills(skills, user).ToList();
          
        }

        public AspNetUsers UpdateUser(AspNetUsers user)
        {

            UserRepository userrepo = new UserRepository();
            //Persistence.Repository.UserRepository2<AspNetUsers> userrepo = new Persistence.Repository.UserRepository2<AspNetUsers>();
            userrepo.Update(user);
            return user;

        }

        public Company InsertCompany(Company company)
        {
            CompanyRepository comprepo = new CompanyRepository();

         var companyact = comprepo.GetList("where iduser='" + company.IdUser+"'").FirstOrDefault();
            if (companyact != null)
                comprepo.Delete(companyact.Id);
           int? companyid= comprepo.Insert(company);
            company= comprepo.Get((int)companyid);

            return company;

        }

        public bool InsertEducation(List<Education> educations)
        {
            UserRepository userrepo = new UserRepository();
            EducationRepository<Education> edurepo = new EducationRepository<Education>();
            try
            {
             var educationsact=this.GetEducationByUser(educations.FirstOrDefault().UserId);
               
                foreach (var edu in educationsact)
                {
                    edurepo.Delete(edu.Id);
                }

                userrepo.InsertEducation(educations);

            }
            catch (Exception er)
            {
                return false;
            }

            return true;
        }

        public bool InsertCertifications(List<Certification> certifications)
        {
            UserRepository userrepo = new UserRepository();
            CertificationRepository<Certification> certrepo = new CertificationRepository<Certification>();
           var certact= userrepo.GetCertificationsByUser(certifications.FirstOrDefault().UserId);


            foreach (var edu in certact)
            {
                certrepo.Delete(edu.Id);
            }
            try
            {
                userrepo.InsertCertifications(certifications);

            }
            catch (Exception er)
            {
                return false;
            }

            return true;
        }

        public bool InsertLanguages(List<Language> languages)
        {
            UserRepository userrepo = new UserRepository();
            LanguageRepository<Language> lanrepo = new LanguageRepository<Language>();
         var lengact=   userrepo.GetLanguagesByUser(languages.FirstOrDefault().UserId);


            foreach (var lan in lengact)
            {
                lanrepo.Delete(lan.Id);
            }

            try
            {
                userrepo.InsertLanguages(languages);

            }
            catch (Exception er)
            {
                return false;
            }

            return true;
        }
    }
}
