using Library.Models;
using Library.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Library.Helpers;
namespace Library.Bussiness
{
    public class BookB
    {
        BookRepository BookRepository;
        LibraryEntities context;
        public BookB()
        {
            context = new Models.LibraryEntities();
            BookRepository = new BookRepository(context);

        }

        public List<Book> GetDataFromDbase(string searchBy, int take, int skip, string sortBy, bool sortDir, out int filteredResultsCount, out int totalResultsCount)
        {
            var List = new List<Book>();
            try
            {
                int totalCount = 0;
                if (searchBy == null)
                {
                    totalCount = BookRepository.List().Count();

                    if (sortDir)
                    {
                        List = BookRepository.List()
                                                   .OrderBy(sortBy)
                                                   .Skip(skip)//(pageNo - 1) * param.iDisplayLength)
                                                   .Take(take)//param.iDisplayLength)
                                                   .ToList()
                                               .Select(x => new Book
                                               {
                                                   Auther = x.Auther,
                                                   CopiesNumber = x.CopiesNumber,
                                                   BookOrders = x.BookOrders,
                                                   Title = x.Title,
                                                   Id = x.Id,
                                                   Created_At = x.Created_At,
                                                   Updated_At = x.Updated_At

                                                }).ToList();
                    }
                    else
                    {
                        List = BookRepository.List()
                              .OrderByDescending(sortBy)
                              .Skip(skip)//(pageNo - 1) * param.iDisplayLength)
                              .Take(take)//param.iDisplayLength)
                              .ToList()
                          .Select(x => new Book
                          {
                              Auther = x.Auther,
                              CopiesNumber = x.CopiesNumber,
                              BookOrders = x.BookOrders,
                              Title = x.Title,
                              Id = x.Id,
                              Created_At = x.Created_At,
                              Updated_At = x.Updated_At

                          }).ToList();

                    }
                    
                }
                else
                {
                    totalCount = BookRepository.List(
                      x => x.Auther.Contains(searchBy) || x.Title.Contains(searchBy)
                     ).Count();

                    if (sortDir)
                    {
                        List = BookRepository.List(
                        x => x.Auther.Contains(searchBy) || x.Title.Contains(searchBy)

                            )
                                                   .OrderBy(x => sortBy)
                                                   .Skip(skip)//(pageNo - 1) * param.iDisplayLength)
                                                   .Take(take)//param.iDisplayLength)
                                                   .ToList()
                                               .Select(x => new Book
                                               {
                                                   Auther = x.Auther,
                                                   CopiesNumber = x.CopiesNumber,
                                                   BookOrders = x.BookOrders,
                                                   Title = x.Title,
                                                   Id = x.Id,
                                                   Created_At = x.Created_At,
                                                   Updated_At = x.Updated_At

                                               }).ToList();
                    }
                    else
                    {
                        List = BookRepository.List(
                         x => x.Auther.Contains(searchBy) || x.Title.Contains(searchBy)

                            )
                              .OrderByDescending(x => sortBy)
                              .Skip(skip)//(pageNo - 1) * param.iDisplayLength)
                              .Take(take)//param.iDisplayLength)
                              .ToList()
                          .Select(x => new Book
                          {
                              Auther = x.Auther,
                              CopiesNumber = x.CopiesNumber,
                              BookOrders = x.BookOrders,
                              Title = x.Title,
                              Id = x.Id,
                              Created_At = x.Created_At,
                              Updated_At = x.Updated_At

                          }).ToList();

                    }
                }

             

                // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
                filteredResultsCount = totalCount;
                totalResultsCount = totalCount;

                return List;

            }
            catch (Exception e)
            {
                filteredResultsCount = 0;
                totalResultsCount = 0;

                return null;

            }

         }

       

        internal IQueryable<Book> GetAll(FilterParams filterParams, out int pagesCount)
        {
            var Books = BookRepository.List();

            if (filterParams.SortKey == "")
            {
                filterParams.SortKey = "Id";
                filterParams.SortDirection = "asc";
            }

            var unOrderedList = Books.Select(a => new Book
            {
                Id = a.Id,
                Auther = a.Auther,
                Title = a.Title,
                CopiesNumber = a.CopiesNumber,
                Created_At = a.Created_At,
                Updated_At = a.Updated_At

            });//.AsNoTracking();
            pagesCount = unOrderedList.Count() / filterParams.ItemsPerPage;

            if (filterParams.SortKey != null && filterParams.SortDirection == "desc")

            {
                return unOrderedList.Where(a => a.Auther.Contains(filterParams.Search) || a.Title.Contains(filterParams.Search)).OrderByDescending(x => filterParams.SortKey)
             .Skip(filterParams.ItemsPerPage * (filterParams.PageNumber - 1)).Take(filterParams.ItemsPerPage);
                
            }
            else
            {
                return unOrderedList.Where(a => a.Auther.Contains(filterParams.Search) || a.Title.Contains(filterParams.Search)).OrderBy(x => filterParams.SortKey)
             .Skip(filterParams.ItemsPerPage * (filterParams.PageNumber - 1)).Take(filterParams.ItemsPerPage);
            }
        }
    
        internal int AddBook(Book Book)
        {
            Book.Created_At = DateTime.UtcNow;
            BookRepository.Insert(Book);

            try
            {
                Save();

                return Book.Id;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }


        ////
        
        //GetById
        internal Book GetById(int id)
        {
            var Book = BookRepository.GetById(id);

            return Book;
        }
        internal async Task <bool> UpdateBook(Book Book)
        {
            Book.Updated_At = DateTime.UtcNow;

            await BookRepository.UpdateAsync(Book);

            try
            {
                Save();
                //return Book.ID;
                return true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return false;
                // return 0;
            }
            catch (Exception e)
            {
                // return 0;
                return false;

            }
        }

        internal async Task<int> Delete(int id)
        {
           await BookRepository.DeleteAsync(id);

            try
            {
                Save();
                return id;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        internal void Dispose()
        {
            Dispose();
        }

        internal bool BookExist(int id)
        {
            return BookRepository.GetById(id) != null;

        }
        public void Save()
        {
            context.SaveChangesAsync();
        }

    }
}