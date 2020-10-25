using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryApp.Core.RequestModels
{
    public class CheckInventoryResponse
    {
        public List<long> NotAvailableBooks { get; set; }
        public bool IsAvailable { get; set; }
    }
}
