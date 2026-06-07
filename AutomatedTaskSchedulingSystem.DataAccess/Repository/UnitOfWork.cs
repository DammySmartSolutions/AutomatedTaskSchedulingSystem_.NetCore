using AutomatedTaskSchedulingSystem.DataAccess.Data;
using AutomatedTaskSchedulingSystem.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace AutomatedTaskSchedulingSystem.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
     

        public IOrganizationRepository Organization { get; private set; }
        public ILocationRepository Location { get; private set; }

        public ITaskRepository Task { get; private set; }

       
        public IPositionRepository Position { get; private set; }

        public IEmployeeRepository Employee { get; private set; }

        public IScheduleRepository Schedule { get; private set; }

        public IEmployeeAvailabilityRepository EmployeeAvailability  { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
          
            Organization = new OrganizationRepository(_db);
            Location =     new LocationRepository(_db);
            Task = new TaskRepository(_db);
            Position = new PositionRepository(_db);
            Employee = new EmployeeRepository(_db);
            EmployeeAvailability = new EmployeeAvailabilityRepository(_db);
            Schedule = new ScheduleRepository(_db);
        }

        
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
