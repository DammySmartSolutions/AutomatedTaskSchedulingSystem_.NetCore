using AutomatedTaskSchedulingSystem.DataAccess.Data;
using AutomatedTaskSchedulingSystem.DataAccess.Repository.IRepository;
using AutomatedTaskSchedulingSystem.Models.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutomatedTaskSchedulingSystem.DataAccess.Repository
{
    public class EmployeeAvailabilityRepository : Repository<EmployeeAvailability>, IEmployeeAvailabilityRepository
    {
        private readonly ApplicationDbContext _db;

        public EmployeeAvailabilityRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;

        }

        //public void Save()
        // {
        //     _db.SaveChanges();
        // }

        public void Update(EmployeeAvailability obj)
        {
            _db.EmployeeAvailability.Update(obj);
        }

    }
}
