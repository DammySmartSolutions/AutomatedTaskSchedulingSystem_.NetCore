using AutomatedTaskSchedulingSystem.DataAccess.Data;
using AutomatedTaskSchedulingSystem.DataAccess.Repository.IRepository;
using AutomatedTaskSchedulingSystem.Models.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutomatedTaskSchedulingSystem.DataAccess.Repository
{
    public class PositionRepository : Repository<Position>, IPositionRepository
    {
        private readonly ApplicationDbContext _db;

        public PositionRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;

        }

        //public void Save()
        // {
        //     _db.SaveChanges();
        // }

        public void Update(Position obj)
        {
            _db.Positions.Update(obj);
        }

    }
}
