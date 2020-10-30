using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LibraryApp.Api.ApiHelpers.Pagination;
using LibraryApp.Core;
using LibraryApp.Core.Contracts;
using LibraryApp.Core.RequestModels;
using LibraryApp.Core.RequestModels.LoanRequest;
using LibraryApp.Core.Uow;
using LibraryApp.Infrastructure.ApiModel;
using LibraryApp.Infrastructure.Localization;
using LibraryApp.Model.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace LibraryApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanController : ControllerBase
    {


        private readonly ILoanRepository _loanRepository;
        private readonly IBookInventoryRepository _bookInventoryRepository;
        private readonly IStringLocalizer<LocalizationResources> _localizer;
        private readonly IUnitOfWork _uow;
        private readonly IUserRepository _userRepository;

        public LoanController(
            ILoanRepository loanRepository,
            IBookInventoryRepository bookInventoryRepository,
            IStringLocalizer<LocalizationResources> localizer,
            IUnitOfWork unitOfWork,
            IUserRepository userRepository)
        {
            _loanRepository = loanRepository;
            _bookInventoryRepository = bookInventoryRepository;
            _localizer = localizer;
            _uow = unitOfWork;
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetLoans([FromQuery]LoanParams loanParams)
        {
            var loansQuery = _loanRepository
                .Queryable()
                .Include(x => x.User)
                .Include(x => x.LoanStatus)
                .Include(x => x.BookLoans).ThenInclude(xy => xy.Book)
                .Where(x => !x.IsDeleted && x.IsActive);

            if (loanParams.UserId > 0)
            {
                loansQuery = loansQuery.Where(x => x.UserId == loanParams.UserId);
            }

            // pagination 
            var loans = await PagedList<Loan>.CreateAsync(loansQuery, loanParams.PageNumber, loanParams.PageSize);
            Response.AddPaginationHeader(loans.CurrentPage, loans.PageSize, loans.TotalCount, loans.TotalPages);

            //return Ok(LibraryResponse.CreateResponse(HttpStatusCode.OK, loans));
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

            return Ok(LibraryResponse.CreateResponse(HttpStatusCode.OK, loans));
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
                        LoanStatusId = 1 // loaned
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


                    return Ok(LibraryResponse.CreateResponse(HttpStatusCode.OK, _localizer[LocalizationResources.AddedNewLoan]));
                }
                // in future should return which book is not available... 'CheckInventoryResponse' will be responsible for that
                // for now it's just basic response message
                return NotFound(LibraryResponse.CreateResponse(HttpStatusCode.NotFound, _localizer[LocalizationResources.BooksAreNotAvailable]));

            }

            return BadRequest(LibraryResponse.CreateResponse(HttpStatusCode.BadRequest));

        }

        [HttpGet]
        [Route("returnLoan/{loanId}")]
        public async Task<IActionResult> ReturnLoan(long loanId)
        {
            var loan = _loanRepository.GetById(loanId);

            if (loan != null)
            {
                loan.DateReturned = DateTimeOffset.UtcNow;
                loan.LoanStatusId = 2;

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
                return Ok(LibraryResponse.CreateResponse(HttpStatusCode.OK));
            }

            return NotFound(LibraryResponse.CreateResponse(HttpStatusCode.NotFound, _localizer[LocalizationResources.SpecificLoanNotFound]));
        }


    }
}