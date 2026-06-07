using AutomatedTaskSchedulingSystem.DataAccess.Data;
using AutomatedTaskSchedulingSystem.Models.Model;
using AutomatedTaskSchedulingSystem.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutomatedTaskSchedulingSystem.DataAccess.Repository
{
    public class OrganizationRepository : Repository<SetupOrganization>, IOrganizationRepository
    {

        private readonly ApplicationDbContext _db;

        public OrganizationRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;

        }

        //public void Save()
        // {
        //     _db.SaveChanges();
        // }

        public void Update(SetupOrganization obj)
        {
            _db.Organization.Update(obj);
        }

    }
}
