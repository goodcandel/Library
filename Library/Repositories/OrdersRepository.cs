
using Library.Models;

namespace Library.Repositories
{
    public class OrdersRepository : GenericRepository<BookOrder>
    {
        //public OrdersRepository(LibraryEntities dbContext)
        //{
        //    _dbContext = dbContext;
        //}
        public OrdersRepository(LibraryEntities dbContext) : base(dbContext)
        {
            _dbContext = dbContext;

        }
    }
}