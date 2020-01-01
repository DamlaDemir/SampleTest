using Microsoft.EntityFrameworkCore;
using sampleTest.model.entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace sampleTest.model.context
{
    public class SampleTestContext : DbContext
    {
        public SampleTestContext(DbContextOptions<SampleTestContext> options)
           : base(options)
        {

        }
        public virtual DbSet<User> Users { get; set; }

    }
}
