namespace MultiTenantApp.Data
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Query;
    using MultiTenantApp.Models;
    using MultiTenantApp.Services;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public class ApplicationDbContext : DbContext
    {
        private readonly TenantService _tenantService;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, TenantService tenantService) : base(options)
        {

            _tenantService = tenantService;
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            Expression<Func<EntityBase, bool>> filterExpr = Entity => Entity.TenantId == _tenantService.GetTenantId();
            foreach (var mutableEntityType in modelBuilder.Model.GetEntityTypes())
            {
                
                if (mutableEntityType.ClrType.IsAssignableTo(typeof(EntityBase)))
                {
                    
                    var parameter = Expression.Parameter(mutableEntityType.ClrType);
                    var body = ReplacingExpressionVisitor.Replace(filterExpr.Parameters.First(), parameter, filterExpr.Body);
                    var lambdaExpression = Expression.Lambda(body, parameter);
                    modelBuilder.Entity(mutableEntityType.ClrType).HasQueryFilter(lambdaExpression);

                }
            }
            // modelBuilder.Entity<Employee>().ToTable("Employees");
            // modelBuilder.Entity<Department>().ToTable("Departments");

            base.OnModelCreating(modelBuilder);
        


        }
    }
}
