using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LibraryApp.Model.Domain
{
    public class Library : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        public string Address { get; set; }

    }
}
