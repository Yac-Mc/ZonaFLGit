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
using ZonaFl.Entities;

namespace ZonaFl.Persistence.Repository
{
    //public sealed class RepositoryUser2 : DapperRepositoryBase
    //{
       
        //public  RepositoryUser2() : base(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString) { }

        //public void InsertSkillsUser(List<Skill> skills , PortFolio user)
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
   
    public sealed class PortFolioRepository : Repository<Entities.PortFolio>, IPortFolioRepository
    {
        private const string TableName = "PortFolio";
       
        /// <summary>
        /// Initializes a new instance of the <see cref="CompetitionRepository" /> class.
        /// </summary>
        public PortFolioRepository() : base(TableName) { }

        /// <summary>
        /// Mappings the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>A mapping object of sql parameters to values.</returns>
        internal override dynamic Mapping(Entities.PortFolio item)
        {

            return new
            {
                Id = item.Id,
                Titulo = item.Titulo,
                UserId = item.UserId,
                CategoriaId = item.CategoriaId,
                Description = item.Description,
                Imagen = item.Imagen,
                TermCopy = item.TermCopy,
                Cliente=item.Cliente
               

            };
        }

        public ICollection<Entities.PortFolio> FindPortFoliosByUser(Guid guid)
        {
            List<Entities.PortFolio> item = null;

            using (IDbConnection cn = Connection)
            {
                cn.Open();
                var results = cn.Query<Entities.PortFolio>("SELECT * FROM PortFolio WHERE USERID=@USERID", new { USERID = guid}).ToList();

                item = results;
            }

            return item;
        }






        /// <summary>
        /// Finds the by ID.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>A user by ID.</returns>
        public override Entities.PortFolio FindByID(int id)
        {
            Entities.PortFolio item = null;

            using (IDbConnection cn = Connection)
            {
                cn.Open();
                var result = cn.Query("SELECT * FROM PortFolio where PortFolio.Id=@Id", new { Id = id }).SingleOrDefault();
                
                if (result != null)
                {
                    item = Mapping(result);
                }
            }

            return item;
        }

        public int  Update(ZonaFl.Entities.PortFolio portfolio)
        {
            int rowsAffected = 0;
            using (IDbConnection cn = Connection)
            {

                string sqlQuery = "UPDATE Portfolio SET Titulo = @Titulo, " +

                     " UserId = @UserId, CategoriaId=@CategoriaId, Description=@Description, Imagen=@Imagen, TermCopy=@TermCopy  " + "WHERE Id = @Id";

                rowsAffected = cn.Execute(sqlQuery, portfolio);


            }
            return  rowsAffected;
        }

        /// <summary>
        /// Finds the by ID.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>A user by ID.</returns>
        public Entities.PortFolio FindPortFolioByUser(int portf,Guid id)
        {
            Entities.PortFolio item = null;

            using (IDbConnection cn = Connection)
            {
                cn.Open();
                var results = cn.Query<Entities.PortFolio>("SELECT * FROM PortFolio WHERE USERID=@USERID" +" and Id=@ID", new { USERID = id, ID= portf }).FirstOrDefault();

                item = results;
            }

            return item;
        }

      

       


       

      

      

        /// <summary>
        /// Finds all.
        /// </summary>
        /// <returns>A list of all users.</returns>
        public override IEnumerable<Entities.PortFolio> FindAll()
        {
            List<Entities.PortFolio> items = new List<Entities.PortFolio>();

            using (IDbConnection cn = Connection)
            {
                cn.Open();
                var results = cn.Query("SELECT * FROM PortFolio");

                for (int i = 0; i < results.Count(); i++)
                {
                    items.Add(Mapping(results.ElementAt(i)));
                }
            }

            return items;
        }

        /// <summary>
        /// Finds the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>A list of users that match the query.</returns>
        public override IEnumerable<Entities.PortFolio> Find(Expression<Func<Entities.PortFolio, bool>> predicate)
        {
            List<Entities.PortFolio> items = new List<Entities.PortFolio>();

            // extract the dynamic sql query and parameters from predicate
            QueryResult result = DynamicQuery.GetDynamicQuery("PortFolio", predicate);

            using (IDbConnection cn = Connection)
            {
                cn.Open();
                var results = cn.Query(result.Sql, (object)result.Param);

                for (int i = 0; i < results.Count(); i++)
                {
                    items.Add(Mapping(results.ElementAt(i)));
                }
            }

            return items;
        }

       

       

        private void InsertUserPortfolio(object parameters, IDbConnection cn)
        {
            string sql = "INSERT INTO portfolio VALUES (@Titulo,@UserId,@CategoriaId,@Description,@Imagen,@TermCopy)";
            cn.Execute(sql, parameters);

        }

        //private int  InsertSkills(object parameters, IDbConnection cn)
        //{



        //    string sql = "INSERT SKILL(Name,IdHtml,CategoryId) VALUES(@Name,@IdHtml,@CategoryId) select cast(scope_identity() as int)";
        //    var rta = cn.Query<int>(sql, parameters).Single();
        // // int skillid= //int.Parse((((object[])(((System.Collections.Generic.IDictionary<string, object>)(rta[0])).Values))[0]).ToString());
        //    return rta;
        //    //string cols = string.Join(",", parameters);
        //    //string cols_params = string.Join(",", parameters.Select(p => "@" + p));
        //    //var sql = "set nocount on insert " + "SKILL" + " (" + cols + ") values (" + cols_params + ") select cast(scope_identity() as int)";

        //    //return database.Query<int?>(sql, o).Single();

        //}
    }
   




}
