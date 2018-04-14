using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Library.ViewModels
{
    public class OrderViewModel//:BookOrder
    {
        //public BookOrder model { get; set; }
        public int Id { get; set; }
        public int BookId { get; set; }
        public string BorrowerId { get; set; }
        public int Status { get; set; }
        public DateTime? Created_At { get; set; }
        public DateTime? Updated_At { get; set; }
        public string BorrowerName { get; set; }
        public string BookTitle { get; set; }
    }
}