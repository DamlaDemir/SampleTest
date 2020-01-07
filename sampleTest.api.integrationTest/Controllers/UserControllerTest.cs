using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using sampleTest.api.integrationTestAS;
using sampleTest.common.DTOs;
using sampleTest.model.entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Xunit;
namespace sampleTest.api.integrationTest
{
    public class UserControllerTest :
    IClassFixture<CustomWebApplicationFactory<TestStartup>>, IDisposable
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<TestStartup> _factory;

        public UserControllerTest(CustomWebApplicationFactory<TestStartup> factory) //her testin başlangıcında çalışır
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            //var projectDir = Directory.GetCurrentDirectory();
            //var configPath = Path.Combine(projectDir, "appsettings.json");

            //_factory = factory.WithWebHostBuilder(builder =>
            //{
            //    builder.UseSolutionRelativeContentRoot("sampleTest.api");

            //    //builder.ConfigureAppConfiguration(conf =>
            //    //{
            //    //    conf.AddJsonFile(configPath);
            //    //});
            //    builder.ConfigureAppConfiguration((context, conf) =>
            //    {
            //        conf.AddJsonFile(configPath);
            //    });

            //    builder.ConfigureTestServices(services =>
            //    {
            //        services.AddMvc().AddApplicationPart(typeof(Startup).Assembly);
            //    });
            //});
        }

        [Fact]
        //[TestBeforeAfter]
        public async Task CreateUser_Should_Return_Success_With_Valid_Parameters()
        {

            JsonMessage expectedMessage = new JsonMessage()
            {
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
            httpResponse.EnsureSuccessStatusCode(); // Status Code 200-299 olmasının kontrolü
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

        public void Dispose() //her test bittiğinde çalışır
        {
            //scope.Dispose();
        }
    }
}
