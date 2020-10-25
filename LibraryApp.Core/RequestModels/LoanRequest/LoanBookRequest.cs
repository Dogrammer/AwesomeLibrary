using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryApp.Core.RequestModels.LoanRequest
{
    public class LoanBookRequest
    {
        public int Quantity { get; set; }
        public long BookId { get; set; }

    }
}
