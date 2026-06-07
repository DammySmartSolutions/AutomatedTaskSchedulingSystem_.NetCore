using AutomatedTaskSchedulingSystem.Models.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutomatedTaskSchedulingSystem.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {


        }


        public DbSet<SetupOrganization> Organization { get; set; }

        public DbSet<Location> Location { get; set; }

               
        public DbSet<SetupTask> Tasks { get; set; }

        public DbSet<Position> Positions { get; set; }

        public DbSet<Employee> Employee { get; set; }

        public DbSet<EmployeeAvailability> EmployeeAvailability { get; set; }

        public DbSet<Schedule> Schedules { get; set; }

        //public DbSet<ApplicationUser> ApplicationUsers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>()
                .HasAlternateKey(e => e.EmpID);

            modelBuilder.Entity<EmployeeAvailability>()
                .HasOne(ea => ea.Employee)
                .WithMany()
                .HasForeignKey(ea => ea.EmpID)
                .HasPrincipalKey(e => e.EmpID);
        }




    }
}
