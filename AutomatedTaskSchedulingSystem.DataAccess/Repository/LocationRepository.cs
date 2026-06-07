using AutomatedTaskSchedulingSystem.DataAccess.Data;
using AutomatedTaskSchedulingSystem.DataAccess.Repository.IRepository;
using AutomatedTaskSchedulingSystem.Models.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutomatedTaskSchedulingSystem.DataAccess.Repository
{
    public class LocationRepository : Repository<Location>, ILocationRepository
    {
        private readonly ApplicationDbContext _db;

        public LocationRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;

        }

        //public void Save()
        // {
        //     _db.SaveChanges();
        // }

        public void Update(Location obj)
        {
            _db.Location.Update(obj);
        }

    }
}
