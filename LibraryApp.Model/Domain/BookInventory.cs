﻿using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryApp.Model.Domain
{
    public class BookInventory : BaseEntity
    {
        public long BookId { get; set; }
        public long Quantity { get; set; }
        public long CurrentQuantity { get; set; }

    }
}