using LibraryApp.Core.Contracts;
using LibraryApp.Core.RequestModels;
using LibraryApp.Core.RequestModels.LoanRequest;
using LibraryApp.Core.Uow;
using LibraryApp.Infrastructure.Context;
using LibraryApp.Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibraryApp.Core.Implementations
{
    public class BookInventoryRepository : Repository<BookInventory>, IBookInventoryRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;

        public BookInventoryRepository(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context)
        {
            _dbContext = context;
            _unitOfWork = unitOfWork;
        }

        public CheckInventoryResponse CheckInventory(ICollection<LoanBookRequest> loanBooks)
        {
            // foreachaj kroz sve
            // dohvati iz book inventory i provjeri jeli ima dovoljan current quantity
            // ako ima vrati true
            // ako samo jedan od njih nema iz generiraj string poruke i vrati false
            var retVal = new CheckInventoryResponse();

            var bookInventories = _dbContext.BookInventories.ToList();

            foreach (var lb in loanBooks)
            {
                if (lb.Quantity == 0)
                {
                    retVal.IsAvailable = false;
                    return retVal;
                }

                var test = bookInventories.FirstOrDefault(x => x.BookId == lb.BookId);

                if (test != null)
                {
                    if (test.CurrentQuantity - lb.Quantity < 0)
                    {
                        retVal.NotAvailableBooks = new List<long>();
                        retVal.NotAvailableBooks.Add(test.BookId);
                        retVal.IsAvailable = false;
                        return retVal;
                    }
                }

            }

            retVal.IsAvailable = true;

            return retVal;
            
        }

        public void UpdateBookInventory(ICollection<LoanBookRequest> loanBooks)
        {
            var inventoriesToUpdate = loanBooks.Select(x => x.BookId);
            var existingInv = _dbContext.BookInventories.Where(x => inventoriesToUpdate.Contains(x.BookId)).ToList();

            foreach (var item in existingInv)
            {
                item.CurrentQuantity = item.CurrentQuantity - loanBooks.FirstOrDefault(a => a.BookId == item.BookId).Quantity;
            }

            //_unitOfWork.Save();
            //_dbContext.SaveChangesAsync();
        }

        public void UpdateBookInventoryAfterReturn(ICollection<BookLoan> loanBooks)
        {
            var inventoriesToUpdate = loanBooks.Select(x => x.BookId);
            var existingInv = _dbContext.BookInventories.Where(x => inventoriesToUpdate.Contains(x.BookId)).ToList();

            foreach(var item in existingInv)
            {
                item.CurrentQuantity = item.CurrentQuantity + loanBooks.FirstOrDefault(a => a.BookId == item.BookId).Quantity;
            }

        }
    }
}
