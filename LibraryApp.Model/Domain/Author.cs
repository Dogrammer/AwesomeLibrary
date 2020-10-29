using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LibraryApp.Model.Domain
{
    public class Author : BaseEntity
    {
        [MaxLength(100)]
        public string FirstName { get; set; }

        [MaxLength(100)]
        public string LastName { get; set; }

        [Required]
        public DateTimeOffset DateOfBirth { get; set; }
        public DateTimeOffset? DateOfDeath { get; set; }

        public ICollection<BookAuthor> BookAuthors { get; set; } = new HashSet<BookAuthor>();
    }
}
