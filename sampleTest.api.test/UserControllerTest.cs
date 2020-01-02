using sampleTest.model.entities;
using sampleTest.infrastructure;
using System;
using System.Collections.Generic;
using Xunit;
using sampleTest.infrastructure.Services;
using Moq;
using spock.infrastructure.UnitOfWork;
using sampleTest.api.Controllers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;

namespace sampleTest.api.test
{
    public class UserControllerTest
    {
        //private readonly IUserService _mockUserService;
        //private readonly IUnitOfWork _mockUow;
        private List<User> userList = new List<User>();
        public UserControllerTest()
        {
            var list = new List<User>
            {
                new User {UserId = 1, Email= "dmldemirr@gmail.com", Password = "1234"},
                new User {UserId = 1, Email= "burakcagriduba@gmail.com", Password = "1234"},
            };
            userList.AddRange(list);
        }
        [Fact]
        public async Task CreateUser_Should_Return_Ok_With_Valid_Parameters()
        {
            var user = new User()
            {
                Email = "sample@gmail.com",
                Password = "1234"
            };
            var _mockUserService = new Mock<IUserService>();
            var _mockUow = new Mock<IUnitOfWork>();

            _mockUserService.Setup(x => x.CheckUserExists(It.IsAny<User>()))
                .ReturnsAsync(false);
                //.Returns<bool>(arg => Task.FromResult(arg))
                //.Callback<User>(arg => userList.Add(user));

            _mockUow.Setup(x => x.Repository<User>().Add(It.IsAny<User>()));
            var sut = new UserController(_mockUow.Object, _mockUserService.Object);
            var actual = await sut.CreateUser(user);
            actual.GetType().Should().Be(typeof(OkResult));

        }

        [Fact]
        public async Task CreateUser_Should_Return_BadRequest_With_Existing_Email()
        {
            var user = new User()
            {
                Email = "dmldemirr@gmail.com",
                Password = "1234"
            };
            var _mockUserService = new Mock<IUserService>();
            var _mockUow = new Mock<IUnitOfWork>();

            _mockUserService.Setup(x => x.CheckUserExists(It.IsAny<User>()))
                .ReturnsAsync(true);
            //.Returns<bool>(arg => Task.FromResult(arg))
            //.Callback<User>(arg => userList.Add(user));

            //_mockUow.Setup(x => x.Repository<User>().Add(It.IsAny<User>()));
            var sut = new UserController(_mockUow.Object, _mockUserService.Object);
            var actual = await sut.CreateUser(user);
            actual.GetType().Should().Be(typeof(BadRequestObjectResult));

        }

        [Fact]
        public async Task CreateUser_Should_Return_BadRequest_With_Empty_Paremeter()
        {
            var user = new User()
            {
                Email = "",
                Password = ""
            };
            var _mockUserService = new Mock<IUserService>();
            var _mockUow = new Mock<IUnitOfWork>();
            _mockUserService.Setup(x => x.CheckUserExists(It.IsAny<User>()))
                .ReturnsAsync(false);
            _mockUow.Setup(x => x.Repository<User>().Add(It.IsAny<User>()));
            var sut = new UserController(_mockUow.Object, _mockUserService.Object);
            sut.ModelState.AddModelError("error", "The Password field is required");
            var actual = await sut.CreateUser(user);
            actual.GetType().Should().Be(typeof(BadRequestResult));
        }
    }
}
