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
    public class OfferPhasesRepository<T> : RepositoryGeneric<OfferPhases>
    {
        public IEnumerable<OfferPhases> GetPhasesByIdOffer(int idoffer)
        {
            SqlConnection _connection;
            IEnumerable<OfferPhases> result;


            using (_connection = Utilities.GetOpenConnection())
            {
                //result = _connection.GetListPaged<TEntity>(pagenumber, itemsperpage, conditions, order);



                var sql = "Select Id, IdOffer, Name, InitPhase, FinishPhase, Value, StatusPhase from OfferPhases where IdOffer=  " + idoffer.ToString() + " order by Id";

                result = _connection.Query<OfferPhases>(sql);
                return result;
            }

        }
    }
}