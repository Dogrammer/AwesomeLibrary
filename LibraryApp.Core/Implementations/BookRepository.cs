using LibraryApp.Core.Contracts;
using LibraryApp.Infrastructure.Context;
using LibraryApp.Model.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryApp.Core.Implementations
{
    public class BookRepository : Repository<Book>, IBookRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public BookRepository(ApplicationDbContext context) : base(context)
        {
            _dbContext = context;
        }
    }
}
