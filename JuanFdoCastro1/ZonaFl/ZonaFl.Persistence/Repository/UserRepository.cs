using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using BB.SmsQuiz.Infrastructure.Encryption;
using ZonaFl.Persistence.Entities;
using Dapper;
using RepoWrapper;
using System.Data.SqlClient;
using System.Configuration;
using ZonaFl.Persistence.Generic;

namespace ZonaFl.Persistence.Repository
{
    //public sealed class RepositoryUser2 : DapperRepositoryBase
    //{
       
        //public  RepositoryUser2() : base(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString) { }

        //public void InsertSkillsUser(List<Skill> skills , AspNetUsers user)
        //{

           
        //    foreach (Skill skill in skills)
        //    {
        //        if (this.Select<Skill>(new Skill().Id == skill.Id) == null)
        //        {
        //            this.Insert<Skill>(skill);
        //            this.
        //        else
        //        {


        //        }
        //    }
        //}


    //&}
   
    public sealed class UserRepository : Repository<AspNetUsers>, IUserRepository
    {
        private const string TableName = "AspNetUsers";
        private const string TableNameUserSkills = "UserSkill";
        /// <summary>
        /// Initializes a new instance of the <see cref="CompetitionRepository" /> class.
        /// </summary>
        public UserRepository() : base(TableName) { }

        /// <summary>
        /// Mappings the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>A mapping object of sql parameters to values.</returns>
        internal override dynamic Mapping(AspNetUsers item)
        {

            return new
            {
                Id = item.Id,
                Username = item.UserName,
                PasswordHash = item.PasswordHash,
                City = item.City,
                Country = item.Country,
                DescUser = item.DescUser,
                Empresa = item.Empresa,
                Freelance = item.Freelance,
                PhoneNumber = item.PhoneNumber,
                EmailConfirmed = item.EmailConfirmed,
                Email = item.Email,
                SecurityStamp = item.SecurityStamp,
                PhoneNumberConfirmed = item.PhoneNumberConfirmed,
                TwoFactorEnabled = item.TwoFactorEnabled,
                LockoutEnabled = item.LockoutEnabled,
                AccessFailedCount = item.AccessFailedCount,
                Status= item.Status
               

            };
        }

        internal dynamic MappingSkill(Skill skill)
        {

            return new
            {
                Name = skill.Name,
                IdHtml = skill.IdHtml,
                CategoryId=skill.CategoryId

            };
        }

        internal dynamic MappingUserSkill(Skill skill, AspNetUsers user)
        {
            string iduser = "";
            if (user.Id == null)
            {
                iduser = this.FindByEmail(user.Email).Id;
                user.Id = iduser;
            }
            return new
            {
                UserId = user.Id,
                SkillId = skill.Id,


            };
        }

        public bool DeleteAccount(Guid guid)
        {
            using (IDbConnection cn = Connection)
            {
                cn.Open();

                string msg = "";
                int returnval=0;
                string sql = "EXEC DBO.[DeleteUser] '" + guid.ToString() + "', '" + msg + "'," + returnval.ToString();
                var result = cn.Query(sql).FirstOrDefault();

                return true;
               



            }
        }

        public AspNetUsers GetUserByOffer(int idOffer,short freelance)
        {
            AspNetUsers item = new AspNetUsers();

            using (IDbConnection cn = Connection)
            {
                cn.Open();
                string query = "";
                if(freelance==0)
                {
                    query = "select AspNetUsers.Id, Empresa, Freelance, DescUser, Country, City, Email, EmailConfirmed, PasswordHash, SecurityStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEndDateUtc, LockoutEnabled, AccessFailedCount, UserName, FirstMiddleName, Image, NombreEmpresa, RazonSocial, Nit, SecIndustrial, NoTrabajadores, DesEmpresa, UrlEmpresa, DateCreate, LastAccess, [Status] from AspNetUsers inner join offer on AspNetUsers.id = offer.iduser   where AspNetUsers.freelance = " + freelance + " and offer.id=@ID";
                }
                else
                {
                    query = "select AspNetUsers.Id, Empresa, Freelance, DescUser, Country, City, Email, EmailConfirmed, PasswordHash, SecurityStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEndDateUtc, LockoutEnabled, AccessFailedCount, UserName, FirstMiddleName, Image, NombreEmpresa, RazonSocial, Nit, SecIndustrial, NoTrabajadores, DesEmpresa, UrlEmpresa, DateCreate, LastAccess, [Status] from AspNetUsers inner join offeruser on AspNetUsers.id = offeruser.iduser   where AspNetUsers.freelance = " + freelance + " and offeruser.idoffer=@ID";
                }
                dynamic result = cn.Query(query, new { ID = idOffer }).SingleOrDefault();
                item = MapUser(result);

            }
           
            return item;

            
        }

        public IEnumerable<Experience> GetExperienceLabByUser(string iduser)
        {
            ExperienceRepository<Experience> exprepo = new ExperienceRepository<Experience>();
              
            return exprepo.GetList(" Where UserId = '"+iduser+"'" );
        }

        public IEnumerable<Certification> GetCertificationsByUser(string iduser)
        {
            CertificationRepository<Certification> certrepo = new CertificationRepository<Certification>();

            return certrepo.GetList(" Where UserId = '" + iduser + "'");
        }

        public IEnumerable<Language> GetLanguagesByUser(string iduser)
        {
            LanguageRepository<Language> certrepo = new LanguageRepository<Language>();

            return certrepo.GetList(" Where UserId = '" + iduser + "'");
        }

        public IEnumerable<Education> GetEducationByUser(string iduser)
        {
            EducationRepository<Education> edurepo = new EducationRepository<Education>();

            return edurepo.GetList(" Where UserId = '" + iduser + "'");
        }

        public void InsertExperienceLab(List<Experience> experiences)
        {

            ExperienceRepository<Experience> exprepo = new ExperienceRepository<Experience>();
           
            foreach (var expe in experiences)
                {
        
                if(!string.IsNullOrEmpty(expe.Company))
                exprepo.Insert(expe);
                }
          
        }

        public AspNetUsers FindByEmail(string email)
        {
            AspNetUsers item = new AspNetUsers();

            using (IDbConnection cn = Connection)
            {
                cn.Open();
                dynamic result = cn.Query("SELECT AspNetUsers.Id, Empresa, Freelance, DescUser, countrymaster.Name as Country, citymaster.Name as City, Email, EmailConfirmed, PasswordHash, SecurityStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEndDateUtc, LockoutEnabled, AccessFailedCount, UserName, FirstMiddleName, [Image]FROM AspNetUsers inner join countrymaster on AspNetUsers.Country=countrymaster.Name inner join citymaster on  AspNetUsers.City=citymaster.Name  WHERE AspNetUsers.Country is not null and AspNetUsers.City is not null and AspNetUsers.Email=@Email", new { Email = "'" + email + "'" }).FirstOrDefault();
                if (result == null)
                {
                    string query="SELECT AspNetUsers.Id, Empresa, Freelance, DescUser, Country, City, Email, EmailConfirmed, PasswordHash, SecurityStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEndDateUtc, LockoutEnabled, AccessFailedCount, UserName, FirstMiddleName, [Image] FROM AspNetUsers   WHERE AspNetUsers.Email=@Email";
                    result=cn.Query<AspNetUsers>(query, new { Email = email.Replace("\"", "") }).SingleOrDefault();
                    //result = cn.Query("SELECT AspNetUsers.Id, Empresa, Freelance, DescUser, Country, City, Email, EmailConfirmed, PasswordHash, SecurityStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEndDateUtc, LockoutEnabled, AccessFailedCount, UserName, FirstMiddleName, [Image] FROM AspNetUsers   WHERE AspNetUsers.Email=@Email", new { Email =  email  }).FirstOrDefault();
                }

               // SkillRepository skillrepo = new SkillRepository();

                //result.Categories = new List<Category>();
                //result.Categories = FindCategoriesByUserID(result.Id);
                //var allskills = skillrepo.FindAll();
                ////var skillsuser= FindSkillsByUserID(id);

                //result.Skills = allskills;
                if(result!=null)
                item = MapUser(result);
                return item;
            }
        }

        public void InsertLanguages(List<Language> languages)
        {
            LanguageRepository<Language> lanrepo = new LanguageRepository<Language>();
           
            foreach (var lan in languages)
            {
               
                if (!string.IsNullOrEmpty(lan.Name))
                    lanrepo.Insert(lan);
            };
        }

        public void InsertCertifications(List<Certification> certifications)
        {
            CertificationRepository<Certification> certrepo = new CertificationRepository<Certification>();
           

            foreach (var cert in certifications)
            {
               if(cert.UserId==null)
                {
                    cert.UserId = certifications.FirstOrDefault().UserId;
                }
                if (!string.IsNullOrEmpty(cert.Description))
                    certrepo.Insert(cert);
            }
        }

        public void InsertEducation(List<Education> educations)
        {
            EducationRepository<Education> edurepo = new EducationRepository<Education>();




                foreach (var edu in educations)
            {


               
               
                if (!string.IsNullOrEmpty(edu.Institution))
                    edurepo.Insert(edu);
            }
        }


        /// <summary>
        /// Finds the by ID.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>A user by ID.</returns>
        public override AspNetUsers FindByID(Guid id)
        {
            AspNetUsers item = new AspNetUsers();

            using (IDbConnection cn = Connection)
            {
                cn.Open();
               dynamic result = cn.Query("SELECT AspNetUsers.Id, CONVERT(bit, AspNetUsers.Empresa) as Empresa , CONVERT(bit, AspNetUsers.Freelance) as Freelance, DescUser, countrymaster.Name as Country, citymaster.Name as City, Email, EmailConfirmed, PasswordHash, SecurityStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEndDateUtc, LockoutEnabled, AccessFailedCount, UserName, FirstMiddleName, [Image]FROM AspNetUsers inner join countrymaster on AspNetUsers.Country=countrymaster.Name inner join citymaster on  AspNetUsers.City=citymaster.Name  WHERE AspNetUsers.Country is not null and AspNetUsers.City is not null and AspNetUsers.ID=@ID", new { ID = id }).FirstOrDefault();
                if (result == null)
                {
                    result = cn.Query("SELECT AspNetUsers.Id, CONVERT(bit, AspNetUsers.Empresa) as Empresa , CONVERT(bit, AspNetUsers.Freelance) as Freelance, DescUser, Country, City, Email, EmailConfirmed, PasswordHash, SecurityStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEndDateUtc, LockoutEnabled, AccessFailedCount, UserName, FirstMiddleName, [Image]FROM AspNetUsers   WHERE AspNetUsers.ID=@ID", new { ID = id }).FirstOrDefault();
                }

                SkillRepository skillrepo = new SkillRepository();

                result.Categories = new List<Category>();
                result.Categories = FindCategoriesByUserID(id);
                var allskills=skillrepo.FindAll();
                //var skillsuser= FindSkillsByUserID(id);

                result.Skills = allskills;
                item = MapUser(result);



            }

            return item;
        }

        private dynamic FindCategoriesByUserID(Guid UserId)
        {
            List<Category> item = null;

            using (IDbConnection cn = Connection)
            {
                cn.Open();
                var results = cn.Query<Category>("select DISTINCT SKILL.CATEGORYID as Id,CATEGORY.NAME as Name from UserSKill INNER JOIN  SKILL ON USERSKILL.SKILLID= SKILL.Id INNER JOIN CATEGORY ON SKILL.CATEGORYID=CATEGORY.ID  WHERE UserSKILL.UserID=@UserId ORDER BY CATEGORYID", new { UserId = UserId }).ToList();
               foreach(Category cat in results)
                {
                    cat.Skills = cn.Query<Skill>("SELECT Skill.ID as Id, Skill.NAME as Name FROM USERSKILL inner join Skill ON USERSKILL.SKILLID=SKILL.ID WHERE USERID=@UserId AND SKILL.CATEGORYID=@CategoryId  ORDER BY SKILL.CATEGORYID", new { UserId = UserId, CategoryId= cat.Id }).ToList();
                }
                item = results;



            }

            return item;
        }

        /// <summary>
        /// Finds the by ID.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>A user by ID.</returns>
        public  AspNetUsers FindCurriculumByIDUser(Guid id)
        {
            AspNetUsers item = null;

            using (IDbConnection cn = Connection)
            {
                cn.Open();
                var result = cn.Query("select  AspNetUsers.Id, Empresa, Freelance, DescUser, Country,  City, Email, EmailConfirmed, PasswordHash, SecurityStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled,LockoutEndDateUtc, LockoutEnabled, AccessFailedCount, UserName, FirstMiddleName, [Image] FROM AspNetUsers    WHERE AspNetUsers.ID=@ID", new { ID = id }).SingleOrDefault();
                result.Skills = FindSkillsByUserID(id);
                 result.Experiences= FindExperirencesByUserID(id);
                result.Certifications = FindCertificatesByUserID(id);
                result.Educations = FindEducationsByUserID(id);
                result.Languages = FindLanguagesByUserID(id);
                if (result != null)
                {
                    item = MapUser(result);
                }
            }

            return item;
        }

        private ICollection<Language> FindLanguagesByUserID(Guid id)
        {
           List<Language> item = null;

            using (IDbConnection cn = Connection)
            {
                cn.Open();
                var results = cn.Query<Language>("SELECT * FROM LANGUAGE WHERE UserId=@UserId", new { UserId = id }).ToList();
                item = results;



            }

            return item;
        }

        private ICollection<Education> FindEducationsByUserID(Guid id)
        {
            List<Education> item = null;

            using (IDbConnection cn = Connection)
            {
                cn.Open();
                var results = cn.Query<Education>("SELECT * FROM Education WHERE UserId=@UserId", new { UserId = id }).ToList();
                item = results;



            }

            return item;
        }

        private ICollection<Certification> FindCertificatesByUserID(Guid id)
        {
            List<Certification> item = null;

            using (IDbConnection cn = Connection)
            {
                cn.Open();
                var results = cn.Query<Certification>("SELECT * FROM CERTIFICATION WHERE UserId=@UserId", new { UserId = id }).ToList();
                item = results;



            }

            return item;
        }

        private ICollection<Experience> FindExperirencesByUserID(Guid id)
        {
            List<Experience> item = null;

            using (IDbConnection cn = Connection)
            {
                cn.Open();
                var results = cn.Query<Experience>("SELECT * FROM EXPERIENCE WHERE UserId=@UserId", new { UserId = id }).ToList();
                item = results;



            }

            return item;
        }

        /// <summary>
        /// Finds the by ID.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>A user by ID.</returns>
        public Skill FindSkillByIDHtml(string Idhtml)
        {
            Skill item = null;

            using (IDbConnection cn = Connection)
            {
                cn.Open();
                var results = cn.Query<Skill>("SELECT * FROM SKILL WHERE IDHTML=@IdHtml", new { IdHtml = Idhtml }).SingleOrDefault();
                item = results;



            }

            return item;
        }

        /// <summary>
        /// Finds the by ID.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>A user by ID.</returns>
        public  List<Skill> FindSkillsByUserID(Guid UserId)
        {
            List<Skill> item = null;

            using (IDbConnection cn = Connection)
            {
                cn.Open();
                var results = cn.Query<Skill>("SELECT Distinct skill.Id,skill.name, skill.CategoryId,skill.idhtml FROM UserSKILL inner join skill on UserSKILL.Skillid=skill.id  WHERE UserSKILL.UserID=@UserId", new { UserId = UserId }).ToList();
                item = results;



            }

            return item;
        }

        /// <summary>
        /// Finds all.
        /// </summary>
        /// <returns>A list of all users.</returns>
        public override IEnumerable<AspNetUsers> FindAll()
        {
            List<AspNetUsers> items = new List<AspNetUsers>();

            using (IDbConnection cn = Connection)
            {
                cn.Open();
                
                var results = cn.Query("select Id, Empresa, Freelance, DescUser, Country, City, Email, EmailConfirmed, PasswordHash, SecurityStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEndDateUtc, LockoutEnabled, AccessFailedCount, UserName, FirstMiddleName, Image, NombreEmpresa, RazonSocial, Nit, SecIndustrial, NoTrabajadores, DesEmpresa, UrlEmpresa, DateCreate, LastAccess,Case  when [Status]= '' then 'Habilitado' when [Status] is NULL then 'Habilitado' else [Status] end as [Status], Rol=case Empresa when 0 then 'Freelance' when 1 then 'Contratante' end, Case  when [Status]= '' then 'Suspender' when [Status] is NULL then 'Suspender' when [Status] = 'Suspendido' then 'Habilitar' when [Status] = 'Activo' then 'Suspender' else '' end as Accion , Case  when [Status]= '' then 'suspende' when [Status] is NULL then 'suspende' when [Status] = 'Suspendido' then 'activa' when [Status] = 'Activo' then 'suspende' else '' end as Estilo,TipoUser=case Empresa when 0 then 'Postulante' when 1 then 'Contratante' end,Url=case Empresa when 0 then '/Freelance/ReadForEmployer/'+Id when 1 then '/Contratante/Read/'+Id end from AspNetUsers where Freelance=1 OR Empresa=1");

                for (int i = 0; i < results.Count(); i++)
                {
                    items.Add(MapUser(results.ElementAt(i)));
                }
            }

            return items;
        }

        /// <summary>
        /// Finds the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>A list of users that match the query.</returns>
        public override IEnumerable<AspNetUsers> Find(Expression<Func<AspNetUsers, bool>> predicate)
        {
            List<AspNetUsers> items = new List<AspNetUsers>();

            // extract the dynamic sql query and parameters from predicate
            QueryResult result = DynamicQuery.GetDynamicQuery("AspNetUsers", predicate);

            using (IDbConnection cn = Connection)
            {
                cn.Open();
                var results = cn.Query(result.Sql, (object)result.Param);

                for (int i = 0; i < results.Count(); i++)
                {
                    items.Add(MapUser(results.ElementAt(i)));
                }
            }

            return items;
        }

        /// <summary>
        /// Maps the user.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns>A user entity from the dynamic result.</returns>
        private static AspNetUsers MapUser(dynamic result)
        {
            AspNetUsers item = new AspNetUsers();
            item.Id = result.Id;
            item.UserName = result.UserName;
            item.City = result.City;
            item.Country = result.Country;
            item.DescUser = result.DescUser;
            item.Email = result.Email;
            item.Empresa = result.Empresa;
            item.Freelance = result.Freelance;
            item.PhoneNumber = result.PhoneNumber;
            item.Skills = result.Skills;
            item.Image = result.Image;
            item.FirstMiddleName = result.FirstMiddleName;
            item.Status = result.Status;
            item.Rol= result.Rol;
            item.TipoUser = result.TipoUser;
            item.Accion = result.Accion;
            item.Estilo = result.Estilo;
            item.Url = result.Url;
            item.SecurityStamp = result.SecurityStamp;
            item.Experiences = result.Experiences;
            item.Languages = result.Languages;
            item.Certifications = result.Certifications;
            item.Educations = result.Educations;
            /* The custom mapping */

            AspNetEncryptionService encripserv = new AspNetEncryptionService();
            item.PasswordHash = encripserv.Encrypt(result.PasswordHash); //new AspNetEncryptionService(result.PasswordHash).ToString();
            item.EmailConfirmed = result.EmailConfirmed;

            return item;
        }

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public ICollection<Skill> AddUserSkills(List<Skill> items, AspNetUsers user)
        {
            using (IDbConnection cn = Connection)
            {
                cn.Open();
                foreach (var skill in items)
                {



                  
                    Skill skillconsulta = FindSkillByIDHtml(skill.IdHtml);
                    if(skillconsulta != null)
                    {
                        var parameters = (object)MappingUserSkill(skillconsulta, user);
                        InsertUserSkills(parameters,cn);
                    }
                    else
                    {
                       
                        var parameters2 = (object)MappingSkill(skill);
                       int skillid= InsertSkills(parameters2, cn);
                        skill.Id = skillid;
                        var parameters = (object)MappingUserSkill(skill, user);
                        InsertUserSkills(parameters, cn);
                    }
                  

                }

                cn.Close();
                // SavePossibleAnswers(cn, item);
                //SaveEntrants(cn, item);

            }

            return items;
        }

        private void InsertUserSkills(object parameters, IDbConnection cn)
        {
            string sql = "INSERT INTO USERSKILL VALUES (@UserId,@SkillId)";
            cn.Execute(sql, parameters);

        }

        private int  InsertSkills(object parameters, IDbConnection cn)
        {



            string sql = "INSERT SKILL(Name,IdHtml,CategoryId) VALUES(@Name,@IdHtml,@CategoryId) select cast(scope_identity() as int)";
            var rta = cn.Query<int>(sql, parameters).Single();
         // int skillid= //int.Parse((((object[])(((System.Collections.Generic.IDictionary<string, object>)(rta[0])).Values))[0]).ToString());
            return rta;
            //string cols = string.Join(",", parameters);
            //string cols_params = string.Join(",", parameters.Select(p => "@" + p));
            //var sql = "set nocount on insert " + "SKILL" + " (" + cols + ") values (" + cols_params + ") select cast(scope_identity() as int)";

            //return database.Query<int?>(sql, o).Single();

        }
    }
   




}
