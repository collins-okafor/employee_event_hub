using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class DataContext : DbContext
    {
        protected DataContext()
        {
        }
        public DataContext(DbContextOptions options) : base(options)
        {
        } 

        public DbSet<Role>? Roles { get; set; }
        public DbSet<User>? Users { get; set; }
        public DbSet<Employee>? Employees { get; set; }
        public DbSet<Event>? Events { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(
                new Employee() { EmployeeId = 1, EmployeeName = "Jon Wick", Address = "North Wing", City = "Abuja", Zipcode = "9001001",
                    Country = "Austria", Phone = "+104000555", Email = "jon@testing.com", Skillsets = "DevOps", Avatar = "/jkks/.png"},
                new Employee() { EmployeeId = 2, EmployeeName = "Slone Jack", Address = "North Wing", City = "Abuja", Zipcode = "9001001",
                    Country = "Austria", Phone = "+104000555", Email = "slone@testing.com", Skillsets = "DevOps", Avatar = "/jkks/.png"},
                new Employee() { EmployeeId = 3, EmployeeName = "Wilson Mavick", Address = "North Wing", City = "Abuja", Zipcode = "9001001",
                    Country = "Austria", Phone = "+104000555", Email = "willie@testing.com", Skillsets = "DevOps", Avatar = "/jkks/.png"}
            );

            modelBuilder.Entity<Role>().HasData(
                new Role() {RoleId = 1, RoleName = "Employee", Roledescription = "Junior Developer!"},
                new Role() {RoleId = 2, RoleName = "HR", Roledescription = "HR of POCHUB"}
            );
        }
    }
}




























