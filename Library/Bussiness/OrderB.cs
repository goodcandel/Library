using Library.Models;
using Library.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Library.Helpers;
using Library.ViewModels;
using Library.Enums;

namespace Library.Bussiness
{
    public class OrderB
    {
        OrdersRepository OrderRepository;
        BookRepository BookRepository;
        LibraryEntities context;
        public OrderB()
        {
            context = new Models.LibraryEntities();
            OrderRepository = new  OrdersRepository(context);
            BookRepository = new BookRepository(context);

        }
        public List<BookOrder> GetDataFromDbase(string searchBy, int take, int skip, string sortBy, bool sortDir, out int filteredResultsCount, out int totalResultsCount)
        {
            var List = new List<BookOrder>();
            try
            {
                int totalCount = 0;
                if (searchBy == null)
                {
                    totalCount = OrderRepository.List().Count();

                    if (sortDir)
                    {
                        List = OrderRepository.List()
                                                   .OrderBy(sortBy)
                                                   .Skip(skip)//(pageNo - 1) * param.iDisplayLength)
                                                   .Take(take)//param.iDisplayLength)
                                                   .ToList()
                                               .Select(x => new BookOrder
                                               {
                                                   Id = x.Id,
                                                   BookId = x.BookId,
                                                   BorrowerId = x.BorrowerId,
                                                   Status = x.Status,
                                                   Created_At = x.Created_At,
                                                   Updated_At = x.Updated_At,
                                                   BookTitle = x.BookTitle,
                                                   BorrowerName = x.BorrowerName


                                               }
                                               ).ToList();
                    }
                    else
                    {
                        List = OrderRepository.List()
                              .OrderByDescending(sortBy)
                              .Skip(skip)//(pageNo - 1) * param.iDisplayLength)
                              .Take(take)//param.iDisplayLength)
                              .ToList()
                .Select(x => new BookOrder
                {
                    Id = x.Id,
                    BookId = x.BookId,
                    BorrowerId = x.BorrowerId,
                    Status = x.Status,
                    Created_At = x.Created_At,
                    Updated_At = x.Updated_At,
                    BookTitle = x.BookTitle,
                    BorrowerName = x.BorrowerName

                }
                                               ).ToList();

                    }

                }
                else
                {
                    totalCount = OrderRepository.List(
                      x => x. AspNetUser.UserName.Contains(searchBy)
                      || x.Book.Title.Contains(searchBy)
                    ).Count();

                    if (sortDir)
                    {
                        List = OrderRepository.List(
                       x => x.AspNetUser.UserName.Contains(searchBy)
                      || x.Book.Title.Contains(searchBy)
                            )
                                                   .OrderBy(x => sortBy)
                                                   .Skip(skip)//(pageNo - 1) * param.iDisplayLength)
                                                   .Take(take)//param.iDisplayLength)
                                                   .ToList()
                                          .Select(x => new BookOrder
                                          {
                                              Id = x.Id,
                                              BookId = x.BookId,
                                              BorrowerId = x.BorrowerId,
                                              Status = x.Status,
                                              Created_At = x.Created_At,
                                              Updated_At = x.Updated_At,
                                              BookTitle = x.BookTitle,
                                              BorrowerName = x.BorrowerName

                                          }
                                               ).ToList();
                    }
                    else
                    {
                        List = OrderRepository.List(
                       x => x.AspNetUser.UserName.Contains(searchBy)
                      || x.Book.Title.Contains(searchBy)
                            )
                              .OrderByDescending(x => sortBy)
                              .Skip(skip)//(pageNo - 1) * param.iDisplayLength)
                              .Take(take)//param.iDisplayLength)
                              .ToList()
                 .Select(x => new BookOrder
                 {
                     Id = x.Id,
                     BookId = x.BookId,
                     BorrowerId = x.BorrowerId,
                     Status = x.Status,
                     Created_At = x.Created_At,
                     Updated_At = x.Updated_At,
                     BookTitle = x.BookTitle,
                     BorrowerName = x.BorrowerName

                 }
                                               ).ToList();

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
        internal int AddOrder(BookOrder Order)
        {
            Book book = BookRepository.GetById(Order.BookId);
            if (book.CopiesNumber > 0)
            {

                OrderRepository.Insert(Order);
                book.CopiesNumber--;

            }
            else
                return 0;
            try
            {
                Save();

                return Order.Id;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        internal int AcceptOrder(int OrderId,bool Accepted)
        {

            BookOrder order = OrderRepository.GetById(OrderId);
            if (order!=null)
            {
                //status 
                if (Accepted)
                    order.Status =(int) Status.Accepted;
                else
                    order.Status = (int)Status.Rejected;

            }
            else
                return 0;
            try
            {
                Save();

                return OrderId;
            }
            catch (Exception ex)
            {
                return 0;
            }

        }
        public List<BookOrder> GetByUser(string userId)
        {
            return OrderRepository.List(x => x.BorrowerId == userId).ToList();
        }
        
        public int CancelOrder(int id)
        {
            OrderRepository.GetById(id).Status = (int)Status.CancelledByUser;
            Save();
            return 3;
        }
        public void Save()
        {
            context.SaveChangesAsync();
        }
    }
}