using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryApp.Api.ApiHelpers.Pagination;
using LibraryApp.Core;
using LibraryApp.Core.RequestModels.User;
using LibraryApp.Core.Uow;
using LibraryApp.Model.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserRepository _userRepository;

        public UserController(IUnitOfWork uow, IUserRepository userRepository)
        {
            _uow = uow;
            _userRepository = userRepository;
        }

        //[HttpGet]
        //public async Task<IActionResult> GetAllUsers([FromQuery]UserParams userParams)
        //{
        //    var usersQuery = _userRepository.Queryable().Include(x => x.Contacts).Where(x => !x.IsDeleted && x.IsActive);

        //    var users = await PagedList<User>.CreateAsync(usersQuery, userParams.PageNumber, userParams.PageSize);
        //    Response.AddPaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);

        //    return Ok(users);
        //}


        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = _userRepository.Queryable().Include(x => x.Contacts).Where(x => !x.IsDeleted && x.IsActive).ToList();

            //var users = await PagedList<User>.CreateAsync(usersQuery, userParams.PageNumber, userParams.PageSize);
            //Response.AddPaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);

            return Ok(users);
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetUser(long id)
        {
            var user = _userRepository.Queryable().Include(x => x.Contacts).FirstOrDefault(x => x.Id == id);
            if (user != null)
            {
                return Ok(user);
            }

            return NotFound();
        }

        [HttpPost]
        [Route("users")]
        public async Task<IActionResult> AddUser(User user)
        {

            var newUser = new User
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth,
                Contacts = new List<Contact>(user.Contacts),
                IsActive = true,
                IsDeleted = false
            };

            _userRepository.Add(newUser);
            await _uow.Save();
            //var retVal = await _userService
            //    .Queryable()
            //    .AsNoTracking()
            //    .Where(py => !py.IsDeleted && py.IsActive)
            //    .ToList();


            return Ok();
        }

        

        [HttpPut]
        [Route("editUser/{id}")]
        public async Task<IActionResult> UpdateUser(long id, User user)
        {
            var existingUser = _userRepository.Queryable().Include(x => x.Contacts).FirstOrDefault(x => x.Id == id);

            if(existingUser != null)
            {
                existingUser.FirstName = user.FirstName;
                existingUser.LastName = user.LastName;
                existingUser.DateOfBirth = user.DateOfBirth;
                existingUser.Contacts = user.Contacts;

                await _uow.Save();

                return Ok();
            }

            return BadRequest();

        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteUser(long id)
        {
            var userToDelete = _userRepository.GetById(id);
            
            if (userToDelete != null)
            {
                //_userRepository.Remove(userToDelete);
                userToDelete.IsActive = false;
                userToDelete.IsDeleted = true;

                await _uow.Save();

                return NoContent();
            }

            return BadRequest();

        }
    }
}