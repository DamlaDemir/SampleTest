using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using sampleTest.model.context;
using System;
using Xunit;

namespace sampleTest.api.integrationTest
{
    //public class TestStartup :Startup
    //{
    //    public TestStartup(IConfiguration configuration) : base(configuration)
    //    {
    //    }

    //    public override void ConfigureDatabase(IServiceCollection services)
    //    {
    //        services.AddDbContext<SampleTestContext>(options =>
    //            options.UseInMemoryDatabase("sampleTest_db"));
    //    }

    //}
}
