using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LibraryApp.Core.Contracts;
using LibraryApp.Infrastructure.ApiModel;
using LibraryApp.Infrastructure.Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace LibraryApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly IStringLocalizer<LocalizationResources> _localizer;

        public BookController(IBookRepository bookRepository, IStringLocalizer<LocalizationResources> localizer)
        {
            _bookRepository = bookRepository;
            _localizer = localizer;
        }

        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            var books = _bookRepository
                .Queryable()
                .Where(x => !x.IsDeleted && x.IsActive).ToList();

            return Ok(LibraryResponse.CreateResponse(HttpStatusCode.OK, books));
        }
    }
}