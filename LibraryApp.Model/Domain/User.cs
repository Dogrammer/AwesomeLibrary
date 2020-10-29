using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LibraryApp.Model.Domain
{
    public class User : BaseEntity
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public virtual ICollection<Loan> Loans { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        public virtual ICollection<Contact> Contacts { get; set; }
        public int TotalOverdue { get; set; }
        public bool IsValid { get; set; }
    }
}
