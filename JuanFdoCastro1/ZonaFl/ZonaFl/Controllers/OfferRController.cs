using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ZonaFl.Models;
using ZonaFl.Business.SubSystems;
using Omu.ValueInjecter;
using static ZonaFl.Models.RegisterBindingModel;
using System.Threading.Tasks;

namespace ZonaFl.Controllers
{
    [Authorize]
    [RoutePrefix("api/OfferR")]
    public class OfferRController : ApiController
    {
        //to retrieve all the users by offer
        [AllowAnonymous]
        [Route("GetOfferUsers")]
        public List<OfferUser> GetOfferUsers(int offerid)
        {
            SOffer soffer = new SOffer();
            SUser suser = new SUser();
            Offer offerm = new Offer();

            var offere = soffer.GetOfferUser(offerid);
            if(offere!=null)
            offerm.InjectFrom(offere);
            offerm.OfferUsers = new List<OfferUser>();
            
            if (offere != null)
            {
                offerm.OfferUsers = offere.OfferUsers.Select(e => new OfferUser().InjectFrom(e)).Cast<OfferUser>().ToList();
                offerm.OfferUsers.ForEach(e => e.NameUser = suser.GetUserById(new Guid(e.IdUser)).UserName);
                offerm.OfferUsers.ForEach(e => e.Url = "/Freelance/ReadForEmployer/" + e.IdUser);
            }
            return offerm.OfferUsers;
        }
       


        [AllowAnonymous]
        [Route("GetUsers")]
        public List<ZonaFl.Persistence.Entities.AspNetUsers> GetUsers()
        {

            List<ZonaFl.Persistence.Entities.AspNetUsers> users = new List<ZonaFl.Persistence.Entities.AspNetUsers>();
            List<RegisterBindingModel> usersmodel = new List<RegisterBindingModel>();
            SUser suser = new SUser();
            users = suser.GetAllUsers();

            return users;

        }
        [AllowAnonymous]
        [Route("GetSuspendUser")]
        public List<ZonaFl.Persistence.Entities.AspNetUsers> GetSuspendUser(string Email, string Estado)
        {
            SUser suser = new SUser();
           var user= suser.GetUserByEmail(Email);
            if (Estado.IndexOf("Suspendido")==-1)
            {
                user.Status = State.Suspendido.ToString();
            }
            else
            {
                user.Status = State.Activo.ToString();
            }
            
            suser.UpdateUser(user);
            List<ZonaFl.Persistence.Entities.AspNetUsers> users = new List<ZonaFl.Persistence.Entities.AspNetUsers>();
            List<RegisterBindingModel> usersmodel = new List<RegisterBindingModel>();
            
            users = suser.GetAllUsers();
            return users;

        }


        [AllowAnonymous]
        [Route("GetSuspendUser")]
        public string GetSuspendUser(string id)
        {
            SUser suser = new SUser();
            var user = suser.GetUserById(Guid.Parse(id));
            
                user.Status = State.Suspendido.ToString();


          bool rta=  suser.DeleteUser(Guid.Parse(id));

            //suser.UpdateUser(user);
            if(rta)
            {
                return id;
            }
            else
            {
                return "";
            }
           

        }

        // GetSkills? categoryid
        //to retrieve all the users by offer
        [AllowAnonymous]
        [Route("GetSkills")]
        public List<Skill> GetSkills(int categoryid)
        {
            SSkill soffer = new SSkill();
            //SUser suser = new SUser();
            List<Skill> skillsm = new List<Skill>();
            var skills=soffer.FindSkillByCategory(categoryid);

            if (skills.Count > 0)
                skillsm= skills.Select(e => new Skill().InjectFrom(e)).Cast<Skill>().ToList();
               

          
            return skillsm;
        }

        // GetSkills? categoryid
        //to retrieve all the users by offer
        [AllowAnonymous]
        [Route("GetSkillsEdit")]
        public List<Skill> GetSkillsEdit(int categoryid,string iduser)
        {
            SSkill soffer = new SSkill();
            //SUser suser = new SUser();
            List<Skill> skillsm = new List<Skill>();
            var skills = soffer.FindSkillByCategoryEdit(categoryid, iduser);

            if (skills.Count > 0)
                skillsm = skills.Select(e => new Skill().InjectFrom(e)).Cast<Skill>().ToList();



            return skillsm;
        }


        //to retrieve the select User for adjudicar proyecto
        [AllowAnonymous]
        [Route("GetSetProjetToUser")]
        public List<OfferUser> GetSetProjetToUser(int Id)
        {
           
           
           
            //Guid userInfoId = new Guid(IdUser.ToString());
            SUser suser = new SUser();
            SProject sproject = new SProject();
            SOffer soffer = new SOffer();
           var offeruser= soffer.GetOfferUserById(Id);

            var user= suser.GetUserById(new Guid(offeruser.IdUser));
            var offere = soffer.GetOffer(offeruser.IdOffer);
           Persistence.Entities.Project project = new Persistence.Entities.Project();
            Persistence.Entities.ProjectUser projectuser = new Persistence.Entities.ProjectUser();
            projectuser.IdUser = offeruser.IdUser;

            project.IdCategory = offere.Category.Id;
            project.IdOffer = offere.Id;
            
            //project.Inicio = soffer.GetPhaseInitial(idoffer).InitPhase;
            //project.Fin= soffer.GetPhaseFinal(idoffer).FinishPhase;
            //project.Phases = new List<OfferPhase>();
            //project.Phases = soffer.GetOfferPhases(idoffer).Select(e=>new OfferPhase().InjectFrom(e)).Cast<OfferPhase>().ToList();
            project.Postulantes = soffer.GetOfferUser(offeruser.IdOffer).OfferUsers.Count();
            project.Status = Persistence.Entities.StatusProject.EnCurso;

            sproject.Insert(project, projectuser);

           return  GetOfferUsers(offeruser.IdOffer);
          
        }

        ////to retrieve the select User for adjudicar proyecto
        //[AllowAnonymous]
        //[Route("PostCreateUserAdmin")]
        //public async Task<ZonaFl.Models.UserAdmin> PostCreateUserAdmin(ZonaFl.Models.UserAdmin useradmin)
        //{

        //    AccountController userc = new AccountController();
        //    RegisterBindingModel user = new RegisterBindingModel();
        //    user.UserName = useradmin.useradmin;
        //    user.Email = useradmin.mailadmin;
        //    user.FirstMiddleName = useradmin.nameadmin;
        //    user.PasswordHash = useradmin.passadmin;
        //    await Register(user);
        //    //Guid userInfoId = new Guid(IdUser.ToString());


        //    return useradmin;

        //}

       
    }
}
