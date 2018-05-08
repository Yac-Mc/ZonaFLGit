using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Linq.Expressions;
namespace BB.SmsQuiz.Infrastructure.Domain
{
    public interface IZonaFlRepository<T> where T : EntityBase, IAggregateRoot
    {
        void Add(T item);
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        IEnumerable<T> FindAll();
        T FindByID(int id);
        void Remove(T item);
        void Update(T item);
    }

   
}
