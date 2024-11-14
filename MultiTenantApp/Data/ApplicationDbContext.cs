namespace MultiTenantApp.Data
{
    using Microsoft.EntityFrameworkCore;
    using MultiTenantApp.Models;
    using System;
    using System.Collections.Generic;
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().ToTable("Employees");
            modelBuilder.Entity<Department>().ToTable("Departments");
        }


    }
}
