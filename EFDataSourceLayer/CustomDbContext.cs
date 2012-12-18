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
            : base(connectionString) { }            

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            ///Каскадное удаление Recipients по Message
            modelBuilder.Entity<Message>().HasMany(a => a.Recipients).WithRequired(b => b.Message).WillCascadeOnDelete(true);
            ///Запрет на каскадное удаление Messages при удалении Employee
            modelBuilder.Entity<Employee>().HasMany(a => a.Sent).WithRequired(b => b.Sender).WillCascadeOnDelete(false);
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Recipient> Recipients { get; set; }
    }
}
