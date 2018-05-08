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
   
    public sealed class SkillRepository : Repository<Skill>, ISkillRepository
    {
        private const string TableName = "Skill";
        private const string TableNameUserSkills = "UserSkill";
        /// <summary>
        /// Initializes a new instance of the <see cref="CompetitionRepository" /> class.
        /// </summary>
        public SkillRepository() : base(TableName) { }

        /// <summary>
        /// Mappings the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>A mapping object of sql parameters to values.</returns>
        internal override dynamic Mapping(Skill item)
        {

            return new
            {
                Id = item.Id,
                Name=item.Name,
                IdHtml= item.IdHtml
               

            };
        }

        

        internal dynamic MappingUserSkill(Skill skill, AspNetUsers user)
        {

            return new
            {
                UserId = user.Id,
                SkillId = skill.Id,


            };
        }


        /// <summary>
        /// Finds the by ID.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>A user by ID.</returns>
        public  Skill FindByID(int id)
        {
            Skill item = null;

            using (IDbConnection cn = Connection)
            {
                cn.Open();
                var result = cn.Query("SELECT * FROM Skill WHERE ID=@ID", new { ID = id }).SingleOrDefault();

                if (result != null)
                {
                    item = MapSkill(result);
                }
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
                var result = cn.Query("SELECT * FROM SKILL WHERE IDHTML=@IdHtml", new { IdHtml = Idhtml }).SingleOrDefault();

                if (result != null)
                {
                    item = MapSkill(result);
                }
            }

            return item;
        }

        /// <summary>
        /// Finds all.
        /// </summary>
        /// <returns>A list of all users.</returns>
        public override IEnumerable<Skill> FindAll()
        {
            List<Skill> items = new List<Skill>();

            using (IDbConnection cn = Connection)
            {
                cn.Open();
                var results = cn.Query("SELECT * FROM Skill");

                for (int i = 0; i < results.Count(); i++)
                {
                    items.Add(MapSkill(results.ElementAt(i)));
                }
            }

            return items;
        }

        /// <summary>
        /// Finds the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>A list of users that match the query.</returns>
        public override IEnumerable<Skill> Find(Expression<Func<Skill, bool>> predicate)
        {
            List<Skill> items = new List<Skill>();

            // extract the dynamic sql query and parameters from predicate
            QueryResult result = DynamicQuery.GetDynamicQuery("Skill", predicate);

            using (IDbConnection cn = Connection)
            {
                cn.Open();
                var results = cn.Query(result.Sql, (object)result.Param);

                for (int i = 0; i < results.Count(); i++)
                {
                    items.Add(MapSkill(results.ElementAt(i)));
                }
            }

            return items;
        }

        /// <summary>
        /// Maps the user.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns>A user entity from the dynamic result.</returns>
        private static Skill MapSkill(dynamic result)
        {
            Skill item = new Skill();
            item.Id = result.Id;
            item.Name = result.Name;
            item.CategoryId = result.CategoryId;
            /* The custom mapping */
            item.IdHtml = result.IdHtml.Trim();
            item.Category = result.Category;
            return item;
        }

      

        private void InsertSkills(object parameters, IDbConnection cn)
        {



            string sql = "INSERT SKILL(Name,IdHtml) VALUES(@Name,@IdHtml) select cast(scope_identity() as int)";
            cn.Query(sql, parameters);

            //string cols = string.Join(",", parameters);
            //string cols_params = string.Join(",", parameters.Select(p => "@" + p));
            //var sql = "set nocount on insert " + "SKILL" + " (" + cols + ") values (" + cols_params + ") select cast(scope_identity() as int)";

            //return database.Query<int?>(sql, o).Single();

        }

        public Skill FindSkillByName(string name)
        {
            Skill item = null;

            using (IDbConnection cn = Connection)
            {
                cn.Open();
                var result = cn.Query("SELECT * FROM SKILL WHERE Name=@Name", new { Name = name }).SingleOrDefault();

                if (result != null)
                {
                    item = MapSkill(result);
                }
            }

            return item;
        }

        public List<Skill> FindSkillByCategory(int idcategory)
        {
            List<Skill> item = new List<Skill>();

            using (IDbConnection cn = Connection)
            {
                cn.Open();
                var result = cn.Query("SELECT * FROM SKILL WHERE Categoryid=@Categoryid", new { Categoryid = idcategory }).ToList();

                if (result != null && result.Count>0)
                {
                    foreach (var skill in result)
                    {

                        item.Add(MapSkill(skill));
                    }
                }
            }

            return item;
        }

        public List<Skill> FindSkillByCategoryEdit(int categoryid, string iduser)
        {
            List<Skill> item = new List<Skill>();

            using (IDbConnection cn = Connection)
            {
                cn.Open();
                var result = cn.Query("SELECT Id, Name, CategoryId, IdHtml FROM SKILL WHERE Categoryid=@Categoryid and Skill.id not in(select skillid from userskill where UserId=@UserId)", new { Categoryid = categoryid ,UserId= iduser }).ToList();

                if (result != null && result.Count > 0)
                {
                    foreach (var skill in result)
                    {

                        item.Add(MapSkill(skill));
                    }
                }
            }

            return item;
        }
    }
   




}
