using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryApp.Core;
using LibraryApp.Core.Contracts;
using LibraryApp.Core.RequestModels;
using LibraryApp.Core.Uow;
using LibraryApp.Model.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanController : ControllerBase
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IBookInventoryRepository _bookInventoryRepository;
        private readonly IUnitOfWork _uow;

        public LoanController(ILoanRepository loanRepository, IBookInventoryRepository bookInventoryRepository, IUnitOfWork unitOfWork)
        {
            _loanRepository = loanRepository;
            _bookInventoryRepository = bookInventoryRepository;
            _uow = unitOfWork;
        }

        [HttpPost]
        [Route("createLoan")]
        public async Task<IActionResult> CreateLoan(CreateLoanRequest request)
        {
            if (request != null)
            {
                var checkInventory = _bookInventoryRepository.CheckInventory(request.LoanBookRequests);

                if (checkInventory.IsAvailable == true)
                {
                    _bookInventoryRepository.UpdateBookInventory(request.LoanBookRequests);

                    var newLoan = new Loan()
                    {
                        DateDue = request.DateDue,
                        DateLoaned = request.DateLoaned,
                        UserId = request.UserId,
                        LoanStatusId = request.LoanStatusId
                    };

                    foreach (var lb in request.LoanBookRequests)
                    {
                        var newBookLoanObject = new BookLoan()
                        {
                            BookId = lb.BookId,
                            Quantity = lb.Quantity
                        };

                        newLoan.BookLoans.Add(newBookLoanObject);

                    }

                    _loanRepository.Add(newLoan);
                    await _uow.Save();


                    return Ok();
                }

                return BadRequest(checkInventory);
            }

            return BadRequest();
        }

        [HttpPut]
        [Route("returnLoan/{loanId}")]
        public async Task<IActionResult> ReturnLoan(long loanId)
        {
            var loan = _loanRepository.GetById(loanId);

            if (loan != null)
            {
                loan.DateReturned = DateTimeOffset.UtcNow;
                loan.LoanStatusId = 4;

                //dohvati loanbook quantity
                var bookLoans = _loanRepository.Queryable().SelectMany(x => x.BookLoans).Where(x => x.LoanId == loanId).ToList();
                _bookInventoryRepository.UpdateBookInventoryAfterReturn(bookLoans);
                await _uow.Save();
                return Ok();
            }

            return BadRequest();
        }


    }
}