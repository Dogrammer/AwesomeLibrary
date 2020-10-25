using LibraryApp.Model.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryApp.Core
{
    public interface IUserRepository : IRepository<User>
    {
        IEnumerable<User> GetOverdueUsers();
    }
}
