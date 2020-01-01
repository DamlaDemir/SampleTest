using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using sampleTest.model.context;

namespace sampleTest.model
{
    public static class SeedData
    {
        public static void Seed(IApplicationBuilder app)
        {
            //SampleTestContext context = app.ApplicationServices.GetRequiredService<SampleTestContext>();
            //context.Database.Migrate();
        }
    }
}
