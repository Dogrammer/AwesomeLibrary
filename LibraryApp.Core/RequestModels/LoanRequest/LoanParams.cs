using LibraryApp.Core.RequestModels.Pagination;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryApp.Core.RequestModels.LoanRequest
{
    public class LoanParams : PaginationParams
    {
        public long UserId { get; set; }
    }
}
