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
    [Route("api/[controller]")]
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

        public async Task<bool> CreateUser(User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await _userService.CheckUserExists(user)) return false;
                    _uow.Repository<User>().Add(user);
                    _uow.SaveChanges();
                    return true;
                }
                else
                    return false;              
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}