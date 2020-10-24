using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryApp.Model.Domain
{
    public class BaseEntity
    {
        public long Id { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

    }
}
