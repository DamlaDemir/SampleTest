using sampleTest.model.entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace sampleTest.infrastructure.Services
{
    public interface IUserService
    {
       Task<bool> CheckUserExists(User user);
       Task<List<User>> GetAllUsers();
    }
}
