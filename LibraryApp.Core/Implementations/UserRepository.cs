using LibraryApp.Infrastructure.Context;
using LibraryApp.Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibraryApp.Core
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public UserRepository(ApplicationDbContext context) : base(context)
        {
            _dbContext = context;
        }

        public IQueryable<User> SearchByLastName(IQueryable<User> query, string lastNameValue)
        {
            var searchNameQuery = query.Where(x => x.LastName.ToLower().Contains(lastNameValue.Trim().ToLower()));

            return searchNameQuery;
        }
    }
}
