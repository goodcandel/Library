using System;

namespace Library.Models
{
    public abstract class BaseEntity
    {
        public int Id  { get; set; }

        public Nullable< DateTime> Created_At { get; set; }

        public Nullable<DateTime> Updated_At { get; set; }
    }
}