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
   
    public sealed class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private const string TableName = "Category";
     
        /// <summary>
        /// Initializes a new instance of the <see cref="CompetitionRepository" /> class.
        /// </summary>
        public CategoryRepository() : base(TableName) { }

        /// <summary>
        /// Mappings the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>A mapping object of sql parameters to values.</returns>
        internal override dynamic Mapping(Category item)
        {

            return new
            {
                Id = item.Id,
                Name=item.Name
             
               

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
                var result = cn.Query("SELECT * FROM Category WHERE ID=@ID", new { ID = id }).SingleOrDefault();

                if (result != null)
                {
                    item = MapCategory(result);
                }
            }

            return item;
        }
        ///// <summary>
        ///// Finds the by ID.
        ///// </summary>
        ///// <param name="id">The id.</param>
        ///// <returns>A user by ID.</returns>
        //public Skill FindSkillByIDHtml(string Idhtml)
        //{
        //    Category item = null;

        //    using (IDbConnection cn = Connection)
        //    {
        //        cn.Open();
        //        var result = cn.Query("SELECT * FROM Category WHERE IDHTML=@IdHtml", new { IdHtml = Idhtml }).SingleOrDefault();

        //        if (result != null)
        //        {
        //            item = MapUser(result);
        //        }
        //    }

        //    return item;
        //}

        /// <summary>
        /// Finds all.
        /// </summary>
        /// <returns>A list of all users.</returns>
        public override IEnumerable<Category> FindAll()
        {
            List<Category> items = new List<Category>();

            using (IDbConnection cn = Connection)
            {
                cn.Open();
                var results = cn.Query("SELECT * FROM Category");

                for (int i = 0; i < results.Count(); i++)
                {
                    items.Add(MapCategory(results.ElementAt(i)));
                }
            }

            return items;
        }

        /// <summary>
        /// Finds the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>A list of users that match the query.</returns>
        public override IEnumerable<Category> Find(Expression<Func<Category, bool>> predicate)
        {
            List<Category> items = new List<Category>();

            // extract the dynamic sql query and parameters from predicate
            QueryResult result = DynamicQuery.GetDynamicQuery("Category", predicate);

            using (IDbConnection cn = Connection)
            {
                cn.Open();
                var results = cn.Query(result.Sql, (object)result.Param);

                for (int i = 0; i < results.Count(); i++)
                {
                    items.Add(MapCategory(results.ElementAt(i)));
                }
            }

            return items;
        }
        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public Category AddCategory(Category item)
        {
            using (IDbConnection cn = Connection)
            {
               


                    cn.Open();

                   

                        var parameters = (object)Mapping(item);
                        InsertCategory(parameters, cn);
                
            }

            return FindCategoryByName(item.Name);
            
        }
        /// <summary>
        /// Maps the user.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns>A user entity from the dynamic result.</returns>
        private static Category MapCategory(dynamic result)
        {
            Category item = new Category();
            item.Id = result.Id;
            item.Name = result.Name;
         
            return item;
        }

      

        private void InsertCategory(object parameters, IDbConnection cn)
        {



            string sql = "INSERT CATEGORY(Name) VALUES(@Name) select cast(scope_identity() as int)";
            cn.Query(sql, parameters);

            //string cols = string.Join(",", parameters);
            //string cols_params = string.Join(",", parameters.Select(p => "@" + p));
            //var sql = "set nocount on insert " + "SKILL" + " (" + cols + ") values (" + cols_params + ") select cast(scope_identity() as int)";

            //return database.Query<int?>(sql, o).Single();

        }

        public Category FindCategoryByName(string name)
        {
            Category item = null;

            using (IDbConnection cn = Connection)
            {
                cn.Open();
                var result = cn.Query("SELECT * FROM CATEGORY WHERE Name=@Name", new { Name = name }).SingleOrDefault();

                if (result != null)
                {
                    item = MapCategory(result);
                }
            }

            return item;
        }
        public Category FindCategoryById(int Id)
        {
            Category item = null;

            using (IDbConnection cn = Connection)
            {
                cn.Open();
                var result = cn.Query("SELECT * FROM CATEGORY WHERE Id=@Id", new { Id = Id }).SingleOrDefault();

                if (result != null)
                {
                    item = MapCategory(result);
                }
            }

            return item;
        }
    }
   




}
