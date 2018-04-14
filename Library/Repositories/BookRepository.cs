
using Library.Models;

namespace Library.Repositories
{
    public class BookRepository : GenericRepository<Book>
    {
        //public BookRepository(LibraryEntities dbContext)
        //{
        //    _dbContext = dbContext;
        //}
        public BookRepository(LibraryEntities dbContext) : base(dbContext)
        {
            _dbContext = dbContext;

        }
    }
}