using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryApp.Model.Domain
{
    public class Library : BaseEntity
    {
        public string Name { get; set; }
        public string Address { get; set; }

    }
}
