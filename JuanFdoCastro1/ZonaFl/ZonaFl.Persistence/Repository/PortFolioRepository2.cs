using System;
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
namespace ZonaFl.Persistence.Repository
{
    public class PortFolioRepository2<T> : RepositoryGeneric<PortFolio>
    {

        public int Insert(PortFolio portfolio)
        {



            SqlConnection _connection;
            using (_connection = Utilities.GetOpenConnection())
            {
                string processQuery = "INSERT INTO portfolio VALUES (@Titulo, @Userid,@CategoriaId,@Description,@Imagen,@TermCopy,@Cliente)";
                return _connection.Execute(processQuery, new { portfolio.Titulo, portfolio.UserId, portfolio.CategoriaId, portfolio.Description, portfolio.Imagen, portfolio.TermCopy, portfolio.Cliente });
            }


        }

        public int Update(PortFolio portfolio)
        {



            SqlConnection _connection;
            using (_connection = Utilities.GetOpenConnection())
            {
                string processQuery = "Update  portfolio SET Titulo='"+ portfolio.Titulo+ "',UserId='"+ portfolio.UserId+ "',CategoriaId='"+ portfolio.CategoriaId+ "',Description='"+ portfolio.Description+ "',Imagen='"+ portfolio.Imagen+ "',TermCopy='"+ portfolio.TermCopy+ "',Cliente='"+ portfolio.Cliente+"'  where id="+ portfolio.Id;
                return _connection.Execute(processQuery);
            }


        }

        public PortFolio Get(int id)
        {

            SqlConnection _connection;
            using (_connection = Utilities.GetOpenConnection())
            {

                return _connection.Query<PortFolio>("Select * from Portfolio where Id=" + id).FirstOrDefault();
            }


        }


        public bool Delete(int id)
        {
            SqlConnection _connection;
            using (_connection = Utilities.GetOpenConnection())
            {

                _connection.Query<PortFolio>("Delete Portfolio where id=" + id);
                return true;
            }

        }
    }
}
