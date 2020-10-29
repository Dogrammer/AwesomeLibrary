using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LibraryApp.Model.Domain
{
    public class Contact : BaseEntity
    {
        [MaxLength(10)]
        public string PhoneNumber { get; set; }
        [MaxLength(10)]
        public string MobileNumber { get; set; }
        [Required]
        [MaxLength(255)]
        public string EmailAddress { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }
    }
}
