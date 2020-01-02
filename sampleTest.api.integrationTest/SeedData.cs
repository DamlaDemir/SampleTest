using sampleTest.model.context;
using sampleTest.model.entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace sampleTest.api.integrationTest
{
    public static class SeedData
    {
        public static void PopulateTestData(SampleTestContext dbContext)
        {
            dbContext.Users.Add(new User() { UserId = 1, Email = "dmldemirr@gmail.com",Password = "1234" });
            dbContext.Users.Add(new User() { UserId = 2, Email = "burakkcagriduba@gmail.com",Password = "1234" });
            dbContext.SaveChanges();
        }
    }
}
