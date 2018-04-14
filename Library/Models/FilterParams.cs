using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Library.Models
{
    public class FilterParams
    {
      
            /// <summary>
            /// Current page number
            /// </summary>
            public int PageNumber { get; set; }
            /// <summary>
            /// Number of items per page
            /// </summary>
            public int ItemsPerPage { get; set; }
            /// <summary>
            /// Search Text
            /// </summary>
            public string Search { get; set; }
            public string SortKey { get; set; }
            public string SortDirection { get; set; }
      
    }
}