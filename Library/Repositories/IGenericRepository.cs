using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Library.Repositories
{
    public interface IGenericRepository<T>
    {
        T GetById(int id);

        IQueryable<T> List();
        IQueryable<T> List(Expression<Func<T, bool>> predicate);

        Task InsertAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(int id);
    }
}