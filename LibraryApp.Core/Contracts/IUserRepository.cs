using LibraryApp.Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibraryApp.Core
{
    public interface IUserRepository : IRepository<User>
    {
        IQueryable<User> SearchByLastName(IQueryable<User> query, string lastNameValue);
    }
}
