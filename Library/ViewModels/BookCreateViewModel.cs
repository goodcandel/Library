using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Library.ViewModels
{
    public class BookCreateViewModel: Book
    {
        public Book model { get; set; }

        public bool isDelete { get; set; }
    }
}