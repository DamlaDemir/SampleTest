using Microsoft.EntityFrameworkCore;
using sampleTest.model.context;
using sampleTest.model.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sampleTest.api.integrationTest
{
    public static class SeedData
    {
        public static void PopulateTestData(SampleTestContext dbContext)
        {
            dbContext.Database.EnsureDeleted(); //veri tabanının silinmesi
            dbContext.Database.Migrate(); //tekrar veritabanı oluşturulması

            //dbContext.Database.ExecuteSqlCommand("TRUNCATE TABLE [Users]");
            //dbContext.Database.ExecuteSqlCommand("sp_MSForEachTable 'TRUNCATE TABLE ?'");

            if (!dbContext.Users.Any())
            {
                dbContext.Users.Add(new User() { Email = "dmldemirr@gmail.com", Password = "1234" });
                dbContext.Users.Add(new User() { Email = "burakcagriduba@gmail.com", Password = "1234" });
                dbContext.SaveChanges();
            }
      }
    }
}
