using Moq;
using sampleTest.infrastructure.Services;
using sampleTest.model.entities;
using SampleTest.infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace sampleTest.infrastructure.test.Services
{
    public class UserServiceTest
    {
        //[ AutoMoqData]
        //public async Task CheckUserExists_Return_True_With_Valid_Email(Mock<IRepository<User>> _mockUserRepository, List<User> expected,UserService sut)
        //{
        //    ////Arrange
        //    var request = new User()
        //    {
        //        Email = "dmldemirr@gmail.com",
        //        Password = "1234"
        //    };

        //    _mockUserRepository.Setup(x => x.Query(It.IsAny<Expression<Func<User, bool>>>())).Returns(expected.AsQueryable);



        //}
    }
}
