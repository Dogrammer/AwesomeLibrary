using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        //[HttpGet]
        //[Route("getUsers")]
        //public async Task<IActionResult> GetUsers()
        //{
        //    var retVal = await _userService
        //        .Queryable()
        //        .AsNoTracking()
        //        .Where(py => !py.IsDeleted && py.IsActive)
        //        .ToList();

        //    return Ok(Response.CreateResponse(retVal));
        //}
    }
}