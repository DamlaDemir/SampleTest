using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using sampleTest.infrastructure.Services;
using sampleTest.model;
using sampleTest.model.context;
using sampleTest.model.entities;
using SampleTest.infrastructure.Repository;
using spock.infrastructure.UnitOfWork;

namespace sampleTest.api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public virtual void ConfigureServices(IServiceCollection services)
        {
            var connStr = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<SampleTestContext>(options => options.UseSqlServer(connStr, b => {
                b.MigrationsAssembly("sampleTest.api");
                b.EnableRetryOnFailure(5, TimeSpan.FromSeconds(100), null);
            }));
            services.AddControllers();
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddScoped<IRepository<User>, Repository<User>>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserService, UserService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //SeedData.Seed(app);
        }
    }
}
