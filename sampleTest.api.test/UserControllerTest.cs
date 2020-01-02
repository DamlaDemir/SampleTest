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
using AutoFixture.Xunit2;
using AutoFixture;
using AutoFixture.AutoMoq;
using System.ComponentModel.DataAnnotations;

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
        [Theory, AutoMoqData]
        public async Task CreateUser_Should_Return_Ok_With_Valid_Parameters(Mock<IUserService> _mockUserService, Mock<IUnitOfWork> _mockUow)
        {
            var user = new User()
            {
                Email = "sample@gmail.com",
                Password = "1234"
            };

            _mockUserService.Setup(x => x.CheckUserExists(It.IsAny<User>()))
                .ReturnsAsync(false);
                //.Returns<bool>(arg => Task.FromResult(arg))
                //.Callback<User>(arg => userList.Add(user));

            _mockUow.Setup(x => x.Repository<User>().Add(It.IsAny<User>()));
            var sut = new UserController(_mockUow.Object, _mockUserService.Object);
            var actual = await sut.CreateUser(user);
            actual.GetType().Should().Be(typeof(OkResult));

        }

        [Theory, AutoMoqData]
        public async Task CreateUser_Should_Return_BadRequest_With_Existing_Email(Mock<IUserService> _mockUserService, Mock<IUnitOfWork> _mockUow)
        {
            var user = new User()
            {
                Email = "dmldemirr@gmail.com",
                Password = "1234"
            };

            _mockUserService.Setup(x => x.CheckUserExists(It.IsAny<User>()))
                .ReturnsAsync(true);
            //.Returns<bool>(arg => Task.FromResult(arg))
            //.Callback<User>(arg => userList.Add(user));

            //_mockUow.Setup(x => x.Repository<User>().Add(It.IsAny<User>()));
            var sut = new UserController(_mockUow.Object, _mockUserService.Object);
            var actual = await sut.CreateUser(user);
            actual.GetType().Should().Be(typeof(BadRequestObjectResult));

        }

        [Theory, AutoMoqData]
        public async Task Model_Validation_With_Empty_Email(Mock<IUserService> _mockUserService, Mock<IUnitOfWork> _mockUow)
        {
            var result = new List<ValidationResult>();
            var user = new User()
            {
                Password = "1234"
            };
            var isValid = Validator.TryValidateObject(user, new ValidationContext(user), result);
            Assert.Equal("The Email field is required.", result[0].ErrorMessage);

            //_mockUserService.Setup(x => x.CheckUserExists(It.IsAny<User>()))
            //    .ReturnsAsync(false);
            //_mockUow.Setup(x => x.Repository<User>().Add(It.IsAny<User>()));
            //var sut = new UserController(_mockUow.Object, _mockUserService.Object);
            ////sut.ModelState.AddModelError("error", "The Password field is required");
            //var actual = await sut.CreateUser(user);
            //actual.GetType().Should().Be(typeof(BadRequestResult));
        }

        [Theory, AutoMoqData]
        public async Task Model_Validation_With_Invalid_Email(Mock<IUserService> _mockUserService, Mock<IUnitOfWork> _mockUow)
        {
            var result = new List<ValidationResult>();
            var user = new User()
            {
                Email = "test",
                Password = "1234"
            };
            var isValid = Validator.TryValidateProperty("this is a invalid email", new ValidationContext(new User()) { MemberName = "Email" }, result);
            Assert.Contains("The Email field is not a valid e-mail address.", result[0].ErrorMessage);
            Assert.Equal(1, result.Count);
        }
    }
    //Method parameter olarak Automoq yapabilmek için kullanacaðýmýz attribute
    public class AutoMoqDataAttribute : AutoDataAttribute
    {
        public AutoMoqDataAttribute()
            : base(new Fixture().Customize(new AutoMoqCustomization()))
        {
        }
    }
}
