using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sampleTest.infrastructure.Services;
using sampleTest.model.entities;
using SampleTest.infrastructure.Repository;
using spock.infrastructure.UnitOfWork;

namespace sampleTest.api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;

        public UserController(IUnitOfWork uow,IUserService userService)
        {
            _uow = uow;
            _userService = userService;
        }

        [HttpPost]
        public async Task<ActionResult> CreateUser([FromBody]User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await _userService.CheckUserExists(user)) return BadRequest("Error ! Existing user");
                    _uow.Repository<User>().Add(user);
                    _uow.SaveChanges();
                    return Ok();
                }
                else
                    return BadRequest();              
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            try
            {
                var users = _uow.Repository<User>().Query().ToList();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(false);
            }
        }
    }
}