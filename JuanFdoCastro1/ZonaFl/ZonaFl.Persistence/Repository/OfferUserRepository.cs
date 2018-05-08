using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZonaFl.Persistence.Entities;
using ZonaFl.Persistence.Generic;
using System.Data.Common;
using Dapper;
using System.Data.SqlClient;
using Z.Dapper.Plus;
using ZonaFl.Business.SubSystems;
using ZonaFl.Persistence;
using System;

namespace ZonaFl.Business.SubSystems
{
    public class OfferUserRepository<T> : RepositoryGeneric<OfferUser>
    {
        public Persistence.Entities.OfferUser GetOfferUser(int idoffer, string idUser)
        {
            SqlConnection _connection;

            Persistence.Entities.OfferUser result;
            using (_connection = Utilities.GetOpenConnection())
            {
                
                //string processQuery = "DELETE FROM OFFERPHASES WHERE IDOFFER IN )";
                string sqlQuery = "SELECT * FROM OfferUser WHERE IdOffer=@IdOffer and IdUser=@IdUser";
                result = _connection.Query<OfferUser>(sqlQuery, new { idoffer, idUser }).FirstOrDefault();
                _connection.Close();

            }
            return result;
        }
        public List<Persistence.Entities.OfferUser> GetOffersUser(string idUser)
        {
            SqlConnection _connection;

            List<Persistence.Entities.OfferUser> result;
            using (_connection = Utilities.GetOpenConnection())
            {

                //string processQuery = "DELETE FROM OFFERPHASES WHERE IDOFFER IN )";
                string sqlQuery = "SELECT * FROM OfferUser WHERE IdUser=@IdUser";
                result = _connection.Query<OfferUser>(sqlQuery, new {  idUser }).ToList();
                _connection.Close();

            }
            return result;
        }

        public bool HaveOfferUsers(int idOffer)
        {
            SqlConnection _connection;

            Persistence.Entities.OfferUser result;
            using (_connection = Utilities.GetOpenConnection())
            {

                //string processQuery = "DELETE FROM OFFERPHASES WHERE IDOFFER IN )";
                string sqlQuery = "SELECT * FROM OfferUser WHERE IdOffer=@IdOffer";
                result = _connection.Query<OfferUser>(sqlQuery, new { idOffer }).FirstOrDefault();
                _connection.Close();

            }
            if (result != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public int InsertUserOffer(int idoffer, string idUser)
        {
            SqlConnection _connection;
            if (GetOfferUser(idoffer, idUser) == null)
            {
                using (_connection = Utilities.GetOpenConnection())
                {

                    string processQuery = "INSERT INTO OFFERUSER VALUES (@IdOffer, @IdUser)";

                    return _connection.Execute(processQuery, new { idoffer, idUser });
                }
            }
            else
            {
                return -1;
            }
        }

        public AspNetUsers GtUserByEmail(string emailUser)
        {
            SqlConnection _connection;

            Persistence.Entities.AspNetUsers result;
            using (_connection = Utilities.GetOpenConnection())
            {

                //string processQuery = "DELETE FROM OFFERPHASES WHERE IDOFFER IN )";
                string sqlQuery = "SELECT * FROM AspNetUserS WHERE Email=@emailUser";
                result = _connection.Query<AspNetUsers>(sqlQuery, new { emailUser }).FirstOrDefault();
                _connection.Close();

            }
            return result;
        }
    }
}