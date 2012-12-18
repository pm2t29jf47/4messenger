using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Configuration;
using Entities;

namespace EFDataSourceLayer
{
    public class CustomDbContext : DbContext
    {
        public CustomDbContext(string connectionString)
            //: base(connectionString) { }
            : base(ConfigurationManager.ConnectionStrings["EFDB"].ConnectionString) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Recipient> Recipients { get; set; }
    }
}
