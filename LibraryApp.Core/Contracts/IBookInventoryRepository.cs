using LibraryApp.Core.RequestModels;
using LibraryApp.Core.RequestModels.LoanRequest;
using LibraryApp.Model.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryApp.Core.Contracts
{
    public interface IBookInventoryRepository : IRepository<BookInventory>
    {
        CheckInventoryResponse CheckInventory(ICollection<LoanBookRequest> books);
        void UpdateBookInventory(ICollection<LoanBookRequest> books);
        void UpdateBookInventoryAfterReturn(ICollection<BookLoan> loanBooks);
    }
}
