using LibraryApp.Core.RequestModels.Pagination;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryApp.Core.RequestModels.User
{
    public class UserParams : PaginationParams
    {
        public string OrderBy { get; set; } = "lastname_asc";

    }
}
