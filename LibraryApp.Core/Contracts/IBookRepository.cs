using LibraryApp.Model.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryApp.Core.Contracts
{
    public interface IBookRepository : IRepository<Book>
    {
    }
}
