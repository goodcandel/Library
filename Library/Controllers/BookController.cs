using Library.Bussiness;
using Library.Enums;
using Library.Models;
using Library.ViewModels;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Library.Controllers
{
    [Authorize]
    public class BookController : Controller
    {
        BookB BookBussiness = new BookB();
        
        
        LibraryEntities db = new  LibraryEntities();
        public ActionResult Index()
        {

            return View();
        }

    
    
        [HttpPost]
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

        public List<Book> YourCustomSearchFunc(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
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
            var result = BookBussiness.GetDataFromDbase(searchBy, take, skip, sortBy, sortDir, out filteredResultsCount, out totalResultsCount);
            if (result == null)
            {
                // empty collection...
                return new List<Book>();
            }
            return result;
        }


        
    [HttpPost]
    [Authorize(Roles = Role.Admin)]

        public async Task<ActionResult> Index( Book book) //BookCreateViewModel BookCreateViewModel)
    {


            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("key", "Some Validation error");
                return View(book);
            }
            try
            {
                //List<Department> list = db.Departments.ToList();
                //ViewBag.DepartmentList = new SelectList(list, "DepartmentId", "DepartmentName");
            
             if (book.Id > 0)
                {
                    //update
 
                    await BookBussiness.UpdateBook(book);

                }
                else
                {
                    //Insert
                  
                    BookBussiness.AddBook(book);
                }
                return View(book);
            }
        
        catch (Exception ex)
        {

          return View();
        }

    }
        [HttpPost]
        [Authorize(Roles = Role.Admin)]

        public async Task<ActionResult> Delete(int id) //BookCreateViewModel BookCreateViewModel)
        {
            await  BookBussiness.Delete(id);
            return View("Index");

        }
        public ActionResult AddEditBook(int BookId,bool delete=false,bool borrow=false)
    {
        //List<Department> list = db.Departments.ToList();
        //ViewBag.DepartmentList = new SelectList(list, "DepartmentId", "DepartmentName");

        Book model = new Book();

        if (BookId > 0)
        {

              Book book = db.Books.SingleOrDefault(x => x.Id == BookId);
              model.Id = book.Id;
              model.Title=        book.Title;   
              model.Auther=       book.Auther;  
              model.CopiesNumber= book.CopiesNumber;
              model.Updated_At = DateTime.UtcNow;
            }
             ViewBag.delete = delete;
            ViewBag.borrow = borrow;
             return PartialView("_PartialBook", model);
    }


}
}