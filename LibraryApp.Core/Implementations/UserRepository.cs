using LibraryApp.Infrastructure.Context;
using LibraryApp.Model.Domain;
using System;
using System.Collections.Generic;
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
        public IEnumerable<User> GetOverdueUsers()
        {
            throw new NotImplementedException();
        }
    }
}
