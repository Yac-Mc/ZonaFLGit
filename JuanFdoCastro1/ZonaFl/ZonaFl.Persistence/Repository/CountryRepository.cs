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

    public sealed class CountryRepository : Repository<Country>, ICountryRepository
    {
        private const string TableName = "CountryMaster";

        /// <summary>
        /// Initializes a new instance of the <see cref="CountryRepository" /> class.
        /// </summary>
        public CountryRepository() : base(TableName) { }

        public City FindCityByName(string name)
        {
            City item = new City();

            using (IDbConnection cn = Connection)
            {
                cn.Open();

                var result = cn.Query("select Id, name, stateid from citymaster where Name=@Name", new { Name = name }).SingleOrDefault();
                if(result!=null)
                item = MapCity(result);
            }

            return item;
        }

        public City FindCityById(int id)
        {
            City item = new City();

            using (IDbConnection cn = Connection)
            {
                cn.Open();

                var result = cn.Query("select Id, name, countryid from statemaster where Id=@Id", new { Id = id }).SingleOrDefault();
                if (result != null)
                    item = MapCity(result);
            }
            return item;
        }

        /// <summary>
        /// Mappings the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>A mapping object of sql parameters to values.</returns>
        internal override dynamic Mapping(Country item)
        {

            return new
            {
                Id = item.Id,
                Name = item.Name



            };
        }






        /// <summary>
        /// Finds the by ID.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>A user by ID.</returns>
        public Country FindByID(int id)
        {
            Country item = null;

            using (IDbConnection cn = Connection)
            {
                cn.Open();
                var result = cn.Query("SELECT * FROM CountryMaster WHERE ID=@ID", new { ID = id }).SingleOrDefault();

                if (result != null)
                {
                    item = MapCountry(result);
                }
            }

            return item;
        }

        public IEnumerable<City> FindCitiesByCountry(int Countryid)
        {
            List<City> items = new List<City>();

            using (IDbConnection cn = Connection)
            {
                cn.Open();

                var results = cn.Query("select ci.name as NameCity, ci.ID as CodeCity,c.Name as Country from countrymaster c inner" +
                                                        " join statemaster s on c.id = s.countryid inner" +
                                                        " join CityMaster ci on s.id = ci.stateid" +
                                                        " WHERE c.Id=@ID order by ci.name", new { ID = Countryid }).ToList();


                for (int i = 0; i < results.Count(); i++)
                {
                    items.Add(MapCity2(results.ElementAt(i)));
                }
            }

            return items;
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
        public override IEnumerable<Country> FindAll()
        {
            List<Country> items = new List<Country>();

            using (IDbConnection cn = Connection)
            {
                cn.Open();
                var results = cn.Query("SELECT * FROM CountryMaster");

                for (int i = 0; i < results.Count(); i++)
                {
                    items.Add(MapCountry(results.ElementAt(i)));
                }
            }

            return items;
        }

        /// <summary>
        /// Finds the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>A list of users that match the query.</returns>
        public override IEnumerable<Country> Find(Expression<Func<Country, bool>> predicate)
        {
            List<Country> items = new List<Country>();

            // extract the dynamic sql query and parameters from predicate
            QueryResult result = DynamicQuery.GetDynamicQuery("CountryMaster", predicate);

            using (IDbConnection cn = Connection)
            {
                cn.Open();
                var results = cn.Query(result.Sql, (object)result.Param);

                for (int i = 0; i < results.Count(); i++)
                {
                    items.Add(MapCountry(results.ElementAt(i)));
                }
            }

            return items;
        }
        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public Country AddCategory(Country item)
        {
            using (IDbConnection cn = Connection)
            {



                cn.Open();



                var parameters = (object)Mapping(item);
                InsertCountry(parameters, cn);

            }

            return FindCountryByName(item.Name);

        }
        /// <summary>
        /// Maps the user.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns>A user entity from the dynamic result.</returns>
        private static Country MapCountry(dynamic result)
        {
            Country item = new Country();
            item.Id = result.ID;
            item.Name = result.Name;

            return item;
        }


        private static City MapCity(dynamic result)
        {
            City item = new City();
            item.Id = result.Id;
            item.Name = result.name;

            return item;
        }


        private static City MapCity2(dynamic result)
        {
            City item = new City();
            item.Id = result.CodeCity;
            item.Name = result.NameCity;

            return item;
        }


        private void InsertCountry(object parameters, IDbConnection cn)
        {



            string sql = "INSERT CountryMaster(Name) VALUES(@Name) select cast(scope_identity() as int)";
            cn.Query(sql, parameters);

            //string cols = string.Join(",", parameters);
            //string cols_params = string.Join(",", parameters.Select(p => "@" + p));
            //var sql = "set nocount on insert " + "SKILL" + " (" + cols + ") values (" + cols_params + ") select cast(scope_identity() as int)";

            //return database.Query<int?>(sql, o).Single();

        }

        public Country FindCountryByName(string name)
        {
            Country item = null;

            using (IDbConnection cn = Connection)
            {
                cn.Open();
                var result = cn.Query("SELECT * FROM CountryMaster WHERE Name=@Name", new { Name = name }).SingleOrDefault();

                if (result != null)
                {
                    item = MapCountry(result);
                }
            }

            return item;
        }

        public City FindFirstCityByCountry(int countryId)
        {
            City item = new City();

            using (IDbConnection cn = Connection)
            {
                cn.Open();

                var results = cn.Query("select Top 1 ci.name as NameCity, ci.ID as CodeCity,c.Name as Country from countrymaster c inner" +
                                                        " join statemaster s on c.id = s.countryid inner" +
                                                        " join CityMaster ci on s.id = ci.stateid" +
                                                        " WHERE c.Id=@ID order by ci.name", new { ID = countryId }).First();



                item = MapCity2(results);
            }
        
    

            return item;
        }
    }
   




}
