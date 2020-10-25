using LibraryApp.Infrastructure.Context;
using LibraryApp.Model.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryApp.Core
{
    public class LoanRepository : Repository<Loan>, ILoanRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public LoanRepository(ApplicationDbContext context) : base(context)
        {
            _dbContext = context;
        }

        // implement business specific methods
    }
}
