using LibraryApp.Model.Domain;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryApp.Core.RequestModels.User
{
    public class CreateUserRequest
    {
        public IFormFile File { get; set; }
        //public ICollection<Contact> Contacts { get; set; }
        public string Contacts { get; set; }


    }

}
