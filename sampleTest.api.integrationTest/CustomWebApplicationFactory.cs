using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using sampleTest.api.integrationTest;
using sampleTest.model.context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sampleTest.api.integrationTestAS
{
    public class CustomWebApplicationFactory<TStartup>
    : WebApplicationFactory<TStartup> where TStartup : class
    {      
        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            return WebHost.CreateDefaultBuilder(null)
                          .UseStartup<TStartup>();
        }
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration(config =>
            {
                var integrationConfig = new ConfigurationBuilder()
                    .AddJsonFile("integrationsettings.json")
                    .Build();

                config.AddConfiguration(integrationConfig);
            });
            builder.UseSolutionRelativeContentRoot("sampleTest.api");
            builder.ConfigureTestServices(services => //farklı assemblydeki controller'a api isteği atmayı sağlar
            {
                services.AddMvc().AddApplicationPart(typeof(Startup).Assembly);
            });
        }

    }
}
