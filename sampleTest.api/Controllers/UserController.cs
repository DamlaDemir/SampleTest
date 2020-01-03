using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sampleTest.common.DTOs;
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
        public async Task<ActionResult<JsonMessage>> CreateUser([FromBody]User user)
        {
            JsonMessage message = new JsonMessage();
            try
            {
                if (ModelState.IsValid)
                {
                    if (await _userService.CheckUserExists(user)) {
                        message.result = false;
                        message.message = StringMessages.ExistUser;
                        return BadRequest(message);
                    } 
                    _uow.Repository<User>().Add(user);
                    _uow.SaveChanges();
                    message.result = true;
                    message.message = StringMessages.SuccessSave;
                }
                else                
                    return BadRequest(ModelState);              
            }
            catch (Exception ex)
            {
                message.result = false;
                message.message = StringMessages.Error;
                return BadRequest(message);
            }
            return Ok(message);
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsers();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest("Exception");
            }
        }
    }
}