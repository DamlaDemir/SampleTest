using Microsoft.EntityFrameworkCore;
using sampleTest.model.context;
using sampleTest.model.entities;
using SampleTest.infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace sampleTest.infrastructure.Services
{
    public class UserService : Repository<User>, IUserService
    {
        private readonly IRepository<User> _repository;
        public UserService(IRepository<User> repository, SampleTestContext context) : base(context)
        {
            _repository = repository;
        }

        public async Task<bool> CheckUserExists(User user)
        {
            if (user.Email == null)
                return false;
            var userData = await _repository.Query(x => x.Email == user.Email).ToListAsync();
            return userData.Count > 0;

        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _repository.Query().ToListAsync();
        }


    }
}
