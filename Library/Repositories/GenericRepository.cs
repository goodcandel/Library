using System;
using System.Linq;
using System.Threading.Tasks;
 using Library.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
namespace Library.Repositories
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity, new()

    {
        
        protected LibraryEntities _dbContext { get; set; }
        public GenericRepository(LibraryEntities dbContext)
        {
            _dbContext = dbContext;
            _dbContext.Configuration.ProxyCreationEnabled = false;


        }
        public virtual T GetById(int id)
        {
            return  _dbContext.Set<T>().Find(id);
        }

        public IQueryable<T> List()
        {
            return _dbContext.Set<T>().AsQueryable();
        }
        public virtual IQueryable<T> List(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return _dbContext.Set<T>()
                   .Where(predicate)
                   .AsQueryable();
        }
        public async Task InsertAsync(T entity)
        {
            entity.Created_At = DateTime.UtcNow;
            entity.Updated_At = DateTime.UtcNow;

            _dbContext.Set<T>().Add(entity);
            await _dbContext.SaveChangesAsync();
        }
        public void Insert(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            _dbContext.SaveChanges();
        }

        public async Task UpdateAsync(T entity)
        {
            entity.Updated_At = DateTime.UtcNow;

            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            T entity = new T() { Id = id };

            _dbContext.Entry(entity).State = EntityState.Deleted;
            await _dbContext.SaveChangesAsync();
        }
    }
}