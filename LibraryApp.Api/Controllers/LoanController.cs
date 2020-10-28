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
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanController : ControllerBase
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IBookInventoryRepository _bookInventoryRepository;
        private readonly IUnitOfWork _uow;
        private readonly IUserRepository _userRepository;

        public LoanController(
            ILoanRepository loanRepository,
            IBookInventoryRepository bookInventoryRepository, 
            IUnitOfWork unitOfWork,
            IUserRepository userRepository)
        {
            _loanRepository = loanRepository;
            _bookInventoryRepository = bookInventoryRepository;
            _uow = unitOfWork;
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetLoans()
        {
            var loans = _loanRepository
                .Queryable()
                .Include(x => x.User)
                .Include(x => x.LoanStatus)
                .Include(x => x.BookLoans).ThenInclude(xy => xy.Book)
                .Where(x => !x.IsDeleted && x.IsActive).ToList();

            return Ok(loans);
        }

        [HttpGet]
        [Route("{userId}")]
        public async Task<IActionResult> GetLoansForUser(long userId)
        {
            var loans = _loanRepository
                .Queryable()
                .Include(x => x.User)
                .Include(x => x.LoanStatus)
                .Include(x => x.BookLoans).ThenInclude(xy => xy.Book)
                .Where(x => !x.IsDeleted && x.IsActive && x.UserId == userId).ToList();

            return Ok(loans);
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
                        IsActive = true,
                        IsDeleted = false,
                        LoanStatusId = 3 // loaned
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

        [HttpGet]
        [Route("returnLoan/{loanId}")]
        public async Task<IActionResult> ReturnLoan(long loanId)
        {
            var loan = _loanRepository.GetById(loanId);

            if (loan != null)
            {
                loan.DateReturned = DateTimeOffset.UtcNow;
                loan.LoanStatusId = 4;

                if ((loan.DateReturned.Value - loan.DateDue).TotalDays > 0)
                {
                    loan.Overdue = (int)(DateTimeOffset.UtcNow - loan.DateDue).TotalDays;

                    //get user and update total overdue
                    var user = _userRepository.Queryable().FirstOrDefault(x => x.Id == loan.UserId);
                    user.TotalOverdue += loan.Overdue;

                }
                else
                {
                    loan.Overdue = 0;
                }

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