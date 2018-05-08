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
    public class EducationRepository<T> : RepositoryGeneric<Education>
    {
    }
}
