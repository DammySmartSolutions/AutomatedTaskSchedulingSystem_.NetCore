using AutomatedTaskSchedulingSystem.DataAccess.Data;
using AutomatedTaskSchedulingSystem.DataAccess.Repository.IRepository;
using AutomatedTaskSchedulingSystem.Models.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutomatedTaskSchedulingSystem.DataAccess.Repository
{
    public class ScheduleRepository : Repository<Schedule>, IScheduleRepository
    {

        private readonly ApplicationDbContext _db;

        public ScheduleRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;

        }

        //public void Save()
        // {
        //     _db.SaveChanges();
        // }

        public void Update(Schedule obj)
        {
            _db.Schedules.Update(obj);
        }

    }
}
