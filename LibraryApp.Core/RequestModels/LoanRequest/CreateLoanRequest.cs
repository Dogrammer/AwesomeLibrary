using LibraryApp.Core.RequestModels.LoanRequest;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryApp.Core.RequestModels
{
    public class CreateLoanRequest
    {
        public long UserId { get; set; }
        public ICollection<LoanBookRequest> LoanBookRequests { get; set; }
        public DateTimeOffset DateLoaned { get; set; }
        public DateTimeOffset DateDue { get; set; }
        public DateTimeOffset? DateReturned { get; set; }
        public long LoanStatusId { get; set; }

        //public ICollection<BookLoan> BookLoans { get; set; } = new HashSet<BookLoan>();
    }
}
