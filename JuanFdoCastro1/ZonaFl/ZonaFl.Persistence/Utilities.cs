using System.Configuration;
using System.Data.SqlClient;

namespace ZonaFl.Persistence
{
    public class Utilities
    {
        private static readonly ConnectionStringSettings Connection = ConfigurationManager.ConnectionStrings["DefaultConnection"];
        private static readonly string ConnectionString = Connection.ConnectionString;

        public static SqlConnection GetOpenConnection()
        {
            var connection = new SqlConnection(ConnectionString);
            connection.Open();
            return connection;
        }


    }
}
