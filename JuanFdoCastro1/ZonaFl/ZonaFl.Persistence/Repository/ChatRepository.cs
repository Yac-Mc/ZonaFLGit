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
using ZonaFl.Business.SubSystems;

namespace ZonaFl.Persistence.Repository
{
    public class ChatUserDetailRepository<T> : RepositoryGeneric<ChatUserDetail>
    {
        public List<ChatUserDetail> GetUsersByOfferContractorid(string valueoffer)
        {
            SqlConnection _connection;
            IEnumerable<Entities.ChatUserDetail> result;


            using (_connection = Utilities.GetOpenConnection())
            {
                //result = _connection.GetListPaged<TEntity>(pagenumber, itemsperpage, conditions, order);


                if (valueoffer != "")
                {
                    var sql = "SELECT cud.Id,cud.ConnectionId,cud.UserName,cud.EmailID " +
                                "FROM ChatUserDetail cud " +
                            "INNER JOIN AspNetUsers anu ON anu.UserName = cud.UserName " +
                            "INNER JOIN OfferUser ou ON ou.IdUser = anu.Id " +
                            " WHERE anu.Freelance = 1 AND ou.IdOffer IN(" + valueoffer + ") ";


                    result = _connection.Query<Entities.ChatUserDetail>(
                  sql

                  ).AsEnumerable();

                    return result.ToList();
                }
                return new List<ChatUserDetail>();
            }

        }

        public List<ChatUserDetail> GetContractorsByOffers(string valueoffer)
        {
            SqlConnection _connection;
            IEnumerable<Entities.ChatUserDetail> result;


            using (_connection = Utilities.GetOpenConnection())
            {
                //result = _connection.GetListPaged<TEntity>(pagenumber, itemsperpage, conditions, order);
                string sql = "SELECT cud.Id,cud.ConnectionId,cud.UserName,cud.EmailID " +
                            "FROM ChatUserDetail cud " +
                        "INNER JOIN AspNetUsers anu ON anu.UserName = cud.UserName " +
                        "INNER JOIN Offer o ON o.IdUser = anu.Id " +
                        " WHERE anu.EMPRESA = 1";

                if (!string.IsNullOrEmpty(valueoffer))
                {
                    sql = "SELECT cud.Id,cud.ConnectionId,cud.UserName,cud.EmailID " +
                                "FROM ChatUserDetail cud " +
                            "INNER JOIN AspNetUsers anu ON anu.UserName = cud.UserName " +
                            "INNER JOIN Offer o ON o.IdUser = anu.Id " +
                            " WHERE anu.EMPRESA = 1 AND o.id IN(" + valueoffer + ") ";
                }
               

                result = _connection.Query<Entities.ChatUserDetail>(sql).AsEnumerable();
                return result.ToList();
            }
        }
    }
    public class ChatMessageDetailRepository<T> : RepositoryGeneric<ChatMessageDetail>
    {
    }

    public class ChatPrivateMessageDetailsRepository<T> : RepositoryGeneric<ChatPrivateMessageDetails>
    {
    }

    public class ChatPrivateMessageMasterRepository<T> : RepositoryGeneric<ChatPrivateMessageMaster>
    {
    }
}
