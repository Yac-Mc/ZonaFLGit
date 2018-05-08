using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.Common;

namespace ZonaFl.Persistence.Generic
{

    // para mas detalle https://github.com/ericdc1/Dapper.SimpleCRUD
    public abstract class RepositoryGeneric<TEntity> where TEntity: class
    {
        private DbConnection _connection;

        public virtual IEnumerable<TEntity> GetList()
        {
            IEnumerable<TEntity> result;
            using (_connection = Utilities.GetOpenConnection())
            {
                result = _connection.GetList<TEntity>();
            }

            return result;
        }
        //example usage var user = connection.GetList<User>(new { Age = 10 });  
        public virtual IEnumerable<TEntity> GetList(object whereConditions)
        {
            IEnumerable<TEntity> result;
            using (_connection = Utilities.GetOpenConnection())
            {
                result = _connection.GetList<TEntity>(whereConditions);
            }

            return result;
        }

        // example usage var user = connection.GetList<User>("where age = 10 or Name like '%Smith%'");
        public virtual IEnumerable<TEntity> GetList(string whereConditions)
        {
            IEnumerable<TEntity> result;
            using (_connection = Utilities.GetOpenConnection())
            {
                result = _connection.GetList<TEntity>(whereConditions);
            }

            return result;
        }

        public virtual TEntity Get(int id)
        {
            TEntity result;
            using (_connection = Utilities.GetOpenConnection())
            {
                result = _connection.Get<TEntity>(id);
            }

            return result;
        }

        public virtual int? Insert(TEntity model)
        {

            int? result = 0;
            
                using (_connection = Utilities.GetOpenConnection())
                {
                    result= _connection.Insert(model);
                }

           return  result;
            
        }

        public virtual int? Update(TEntity model)
        {
            int? result = 0;
            using (_connection = Utilities.GetOpenConnection())
            {
                result=_connection.Update(model);
            }

            return result;
        }

        public virtual int? Delete(int id)
        {

            int? result = 0;
            using (_connection = Utilities.GetOpenConnection())
            {
                result=_connection.Delete<TEntity>(id);
            }
            return result;
        }

        public virtual IEnumerable<TEntity> GetListPaged(int pagenumber, int itemsperpage, string conditions, string order)
        {
            IEnumerable<TEntity> result;
            using (_connection = Utilities.GetOpenConnection())
            {
                result = _connection.GetListPaged<TEntity>(pagenumber,itemsperpage,conditions,order);
               
            }

            return result;
        }
       


        public virtual int RecordCount(string conditions = "")
        {
            int result=0;
            using (_connection = Utilities.GetOpenConnection())
            {
                result = _connection.RecordCount<TEntity>(conditions);
            }

            return result;
        }

       


    }
}
