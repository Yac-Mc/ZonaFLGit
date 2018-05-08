using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZonaFl.Persistence.Entities;
using ZonaFl.Persistence.Generic;
using System.Data.Common;
//using Dapper;
using System.Data.SqlClient;
using DapperExtensions;
namespace ZonaFl.Persistence.Repository
{
    public class UserRepository2<T>: RepositoryGeneric<AspNetUsers> 
    {
        

        public virtual AspNetUsers Get(Guid id)
        {
            AspNetUsers result;
            SqlConnection _connection;
//            var sql =
//@"
//SELECT * FROM AspNetUsers WHERE Id= @id
//SELECT InboundProperties_ExternalRef as ExternalRef, InboundProperties_ExternalRefPrevious as ExternalRefPrevious FROM Feeds as InboundProperties WHERE FeedId= @FeedId
//SELECT * FROM FeedFilterParameters WHERE FeedId = @FeedId
//SELECT * FROM TeamFeeds WHERE FeedId = @FeedId";

            using (_connection = Utilities.GetOpenConnection())
            {
               
                result = _connection.Get<AspNetUsers>(id);
            }

            return result;
        }
    }
}
