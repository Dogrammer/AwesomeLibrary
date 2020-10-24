using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryApp.Model.Domain
{
    public class BookAuthor
    {
        public virtual Book Book { get; set; }
        public virtual Author Author { get; set; }
        public long BookId { get; set; }
        public long AuthorId { get; set; }
    }
}
