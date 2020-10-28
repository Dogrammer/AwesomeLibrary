using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryApp.Model.Domain
{
    public class Loan : BaseEntity
    {
        public long UserId { get; set; }
        public User User { get; set; }
        public DateTimeOffset DateLoaned { get; set; }
        public DateTimeOffset DateDue { get; set; }
        public DateTimeOffset? DateReturned { get; set; }
        public long LoanStatusId { get; set; }
        public LoanStatus LoanStatus { get; set; }

        public ICollection<BookLoan> BookLoans { get; set; } = new HashSet<BookLoan>();

        public int Overdue { get; set; }



    }
}
