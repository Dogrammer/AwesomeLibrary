using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryApp.Model.Domain
{
    public class Book
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public int YearPublished { get; set; }

        public long LibraryId { get; set; }
        public Library Library { get; set; }

        public ICollection<BookAuthor> BookAuthors { get; set; } = new HashSet<BookAuthor>();


    }
}



