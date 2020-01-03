using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using sampleTest.api.integrationTestAS;
using sampleTest.common.DTOs;
using sampleTest.model.entities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace sampleTest.api.integrationTest
{
    public class UserControllerTest :
    IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Startup> _factory;
        public UserControllerTest(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task CreateUser_Should_Return_Success_With_Valid_Parameters()
        {
            JsonMessage expectedMessage = new JsonMessage() { 
                result = true,
                message = StringMessages.SuccessSave
            };
            var expectedStatusCode = HttpStatusCode.OK;

            // Arrange
            var user = new User()
            {
                Email = "sample@gmail.com",
                Password = "1234",
                IsDeleted = false
            };
            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            // Act
            var httpResponse = await _client.PostAsync("/api/User/CreateUser", content);
            var actualStatusCode = httpResponse.StatusCode;
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<JsonMessage>(stringResponse);

            // Assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(actual.result, expectedMessage.result);
            Assert.Equal(actual.message, expectedMessage.message);
        }

        [Fact]
        public async Task CreateUser_Should_Return_BadRequest_With_Empty_Parameters()
        {
            var expectedStatusCode = HttpStatusCode.BadRequest;

            // Arrange
            var user = new User()
            {
                Email = "sample@gmail.com",
                Password = null,
                IsDeleted = false
            };
            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            // Act
            var httpResponse = await _client.PostAsync("/api/User/CreateUser", content);
            var actualStatusCode = httpResponse.StatusCode;
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            //var actualResult = JsonConvert.DeserializeObject<bool>(stringResponse);

            // Assert
            Assert.Contains("The Password field is required", stringResponse);
            Assert.Equal(expectedStatusCode, actualStatusCode);
        }

        [Fact]
        public async Task CreateUser_Should_BadRequest_With_Exist_Parameters()
        {
            var expectedStatusCode = HttpStatusCode.BadRequest;
            JsonMessage expectedMessage = new JsonMessage()
            {
                result = false,
                message = StringMessages.ExistUser
            };
            // Arrange
            var user = new User()
            {
                Email = "dmldemirr@gmail.com",
                Password = "1234",
                IsDeleted = false
            };
            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            // Act
            var httpResponse = await _client.PostAsync("/api/User/CreateUser", content);
            var actualStatusCode = httpResponse.StatusCode;
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<JsonMessage>(stringResponse);

            // Assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(actual.result, expectedMessage.result);
            Assert.Equal(actual.message, expectedMessage.message);
            //Assert.Contains("Error ! Existing user", stringResponse);
        }

        [Fact]
        public async Task GetAllUsers_Should_Return_User_List()
        {
            var expectedStatusCode = HttpStatusCode.OK;
   
            // Act
            var httpResponse = await _client.GetAsync("/api/User/GetAllUsers");
            var actualStatusCode = httpResponse.StatusCode;
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<IEnumerable<User>>(stringResponse);

            // Assert
            Assert.Contains(users, x => x.Email == "dmldemirr@gmail.com");
            Assert.Equal(expectedStatusCode, actualStatusCode);
        }
    }
}
