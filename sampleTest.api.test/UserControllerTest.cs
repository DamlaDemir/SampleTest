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
using System.Linq;
using sampleTest.common.DTOs;

namespace sampleTest.api.test
{
    public class UserControllerTest
    {

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
        public async Task CreateUser_Should_Return_Success_With_Valid_Parameters(Mock<IUserService> _mockUserService, Mock<IUnitOfWork> _mockUow) //autoMoqData attribute ile parametrede verilen objeler otomatik mocklanır.
        {
            //Arrange --> testte kullanılacak objelerin hazırlanması
            JsonMessage expectedMessage = new JsonMessage()
            {
                result = true,
                message = StringMessages.SuccessSave
            };
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

            //Act --> gerekli işlemin yapılması
            var sut = new UserController(_mockUow.Object, _mockUserService.Object);
            var result = await sut.CreateUser(user);

            //Assert --> test işleminin doğrulanması
            var apiOkResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var actual = apiOkResult.Value.Should().BeAssignableTo<JsonMessage>().Subject;
            Assert.Equal(actual.result, expectedMessage.result);
            Assert.Equal(actual.message, expectedMessage.message);

        }

        [Theory, AutoMoqData]
        public async Task CreateUser_Should_Return_BadRequest_With_Existing_Email(Mock<IUserService> _mockUserService, Mock<IUnitOfWork> _mockUow)
        {
            //Arrange 
            JsonMessage expectedMessage = new JsonMessage()
            {
                result = false,
                message = StringMessages.ExistUser
            };
            var user = new User()
            {
                Email = "dmldemirr@gmail.com",
                Password = "1234"
            };

            _mockUserService.Setup(x => x.CheckUserExists(It.IsAny<User>()))
                .ReturnsAsync(true);
            _mockUow.Setup(x => x.Repository<User>().Add(It.IsAny<User>()));

            //Act 
            var sut = new UserController(_mockUow.Object, _mockUserService.Object);
            var result = await sut.CreateUser(user);

            //Assert 
            var apiBadRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
            var actual = apiBadRequestResult.Value.Should().BeAssignableTo<JsonMessage>().Subject;
            Assert.Equal(actual.result, expectedMessage.result);
            Assert.Equal(actual.message, expectedMessage.message);
        }

        [Theory, AutoMoqData]
        public async Task Model_Validation_Return_Error_With_Empty_Email(Mock<IUserService> _mockUserService, Mock<IUnitOfWork> _mockUow)
        {
            //Arrange 
            var result = new List<ValidationResult>();
            var user = new User()
            {
                Password = "1234"
            };

            //Act
            var isValid = Validator.TryValidateObject(user, new ValidationContext(user), result);

            //Assert 
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
        public async Task Model_Validation__Return_Error_With_Invalid_Email(Mock<IUserService> _mockUserService, Mock<IUnitOfWork> _mockUow)
        {
            //Arrange 
            var result = new List<ValidationResult>();
            var user = new User()
            {
                Email = "test",
                Password = "1234"
            };

            //Act
            var isValid = Validator.TryValidateProperty("this is a invalid email", new ValidationContext(new User()) { MemberName = "Email" }, result);

            //Assert
            Assert.Contains("The Email field is not a valid e-mail address.", result[0].ErrorMessage);
            Assert.Equal(1, result.Count);
        }


        [Theory, AutoMoqData]
        public async Task GetAllUsers_Should_Return_User_List(Mock<IUserService> _mockUserService, Mock<IUnitOfWork> _mockUow, List<User> expected) 
        {
            //Arrange
            _mockUserService.Setup(x => x.GetAllUsers()).ReturnsAsync(expected); //parametrede expected değişkeninin belirlenmesi ile user listesi mocklanır.Mocklanan GetAllUsers fonksiyonunun sonucunda fake olarak bir user listesinin döneceği belirtilir.
            var sut = new UserController(_mockUow.Object, _mockUserService.Object);

            //Act
            var result = sut.GetAllUsers();

            //Assert
            _mockUserService.Verify(x => x.GetAllUsers()); //Bu method genel olarak assert yerine kullanılabilir. Verify edilen mock’lu methodların çağrılıp çağrılmadığı kontrol edilebilir.
            var apiOkResult = result.Result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var actual = apiOkResult.Value.Should().BeAssignableTo<List<User>>().Subject;
        }

        [Theory, AutoMoqData]
        public async Task GetAllUsers_With_Exception_Should_Return_BadRequest(Mock<IUserService> _mockUserService, Mock<IUnitOfWork> _mockUow, List<User> expected)
        {
            //Arrange
            _mockUserService.Setup(x => x.GetAllUsers()).Throws(new Exception("Some exception Message"));//getallusers fonskyionunda hata alıp exception fırlatması durumunda
            var sut = new UserController(_mockUow.Object, _mockUserService.Object);

            //Act
            var result = sut.GetAllUsers();

            //Assert
            var apiOkResult = result.Result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
            var actual = apiOkResult.Value.Should().BeAssignableTo<string>().Subject;

        }
    }
    //Method parameter olarak Automoq yapabilmek için kullanılan attribute
    public class AutoMoqDataAttribute : AutoDataAttribute
    {
        public AutoMoqDataAttribute()
            : base(new Fixture().Customize(new AutoMoqCustomization()))
        {
        }
    }
}
