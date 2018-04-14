using Library.Bussiness;
using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using Library.Enums;
using Library.ViewModels;

namespace Library.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        BookB BookBussiness = new BookB();
        OrderB OrderBussiness = new OrderB();

        // GET: Order
        public ActionResult Index()
        {
            ViewBag.myOrders = OrderBussiness.GetByUser(User.Identity.GetUserId());
            return View();
        }
        // GET: Order
        [Authorize(Roles=Role.Admin)]

        public ActionResult GetAdminOrder()
        {
            return View("AdminOrders");
        }
        [HttpPost]
        [Authorize(Roles = Role.Admin)]

        public JsonResult GetBookRecord(DataTableAjaxPostModel model)
        {
            // action inside a standard controller
            int filteredResultsCount;
            int totalResultsCount;
            var result = YourCustomSearchFunc(model, out filteredResultsCount, out totalResultsCount);



            return Json(new
            {
                // this is what datatables wants sending back
                draw = model.draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = result
            });
        }

        public List<BookOrder> YourCustomSearchFunc(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
        {
            var searchBy = (model.search != null) ? model.search.value : null;
            var take = model.length;
            var skip = model.start;

            string sortBy = "";
            bool sortDir = true;

            if (model.order != null)
            {
                // in this example we just default sort on the 1st column
                sortBy = model.columns[model.order[0].column].data;
                sortDir = model.order[0].dir.ToLower() == "asc";
            }

            // search the dbase taking into consideration table sorting and paging
            var result = OrderBussiness.GetDataFromDbase(searchBy, take, skip, sortBy, sortDir, out filteredResultsCount, out totalResultsCount);
            if (result == null)
            {
                // empty collection...
                return new List<BookOrder>();
            }
            return result;
        }

        [HttpPost]
        public ActionResult Borrow(int id) 
        {


            //if (!ModelState.IsValid)
            //{
            //    //ModelState.AddModelError("key", "Some Validation error");
            //    return View("Index");
            //}
            try
            {
                //List<Department> list = db.Departments.ToList();
                //ViewBag.DepartmentList = new SelectList(list, "DepartmentId", "DepartmentName");

                if (id > 0)
                {

                    int added = OrderBussiness.AddOrder(new BookOrder() {
                        BookId = id,
                        BorrowerId = User.Identity.GetUserId(),
                        Created_At = DateTime.UtcNow,
                        Status = (int)Status.Initial,
                        BorrowerName=User.Identity.GetUserName(),
                         BookTitle=BookBussiness.GetById(id).Title
                });
                   // if(added<1)
                        //error message
                }
                
                return RedirectToAction("Index");
            }

            catch (Exception ex)
            {

                return RedirectToAction("Index");
            }

        }
        //AcceptOrder?BookId=" + BookId + "&Accepted=" + Accepted;
        [HttpPost]
        [Authorize(Roles = Role.Admin)]

        public ActionResult AcceptOrder(int OrderId, bool Accepted)
        {
 
            try
            {
                 
                if (OrderId > 0)
                {

                    int added = OrderBussiness.AcceptOrder(OrderId, Accepted);
                    // if(added<1)
                    //error message

                }
                // return View("AdminOrders");
                return RedirectToAction("GetAdminOrder");
            }

            catch (Exception ex)
            {
                return RedirectToAction("GetAdminOrder");

                //   return View("AdminOrders");
            }
            //finally
            //{

            //     //View("AdminOrders");
            //}
        }
        //Cancel
        //[HttpPost]
        public ActionResult Cancel(int OrderId)
        {

            try
            {

                if (OrderId > 0)
                {

                    int added = OrderBussiness.CancelOrder(OrderId);
                    // if(added<1)
                    //error message

                }
                return RedirectToAction("Index");

            }

            catch (Exception ex)
            {

                return RedirectToAction("Index");
            }
            //finally
            //{

            //     //View("AdminOrders");
            //}
        }
    }
}