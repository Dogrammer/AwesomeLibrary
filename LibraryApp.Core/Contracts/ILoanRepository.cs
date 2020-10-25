using LibraryApp.Core.RequestModels;
using LibraryApp.Model.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryApp.Core
{
    public interface ILoanRepository : IRepository<Loan>
    {
        
    }
}
