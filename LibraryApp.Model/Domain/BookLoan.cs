using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryApp.Model.Domain
{
    public class BookLoan
    {
        public virtual Book Book { get; set; }
        public virtual Loan Loan { get; set; }
        public long BookId { get; set; }
        public long LoanId { get; set; }
        public int Quantity { get; set; }
    }
}
