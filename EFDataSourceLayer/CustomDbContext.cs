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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>().HasMany(a => a.Recipients).WithRequired(b => b.Message).WillCascadeOnDelete(true);
            modelBuilder.Entity<Employee>().HasMany(a => a.Sent).WithRequired(b => b.Sender).WillCascadeOnDelete(false);
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Recipient> Recipients { get; set; }
    }
}
