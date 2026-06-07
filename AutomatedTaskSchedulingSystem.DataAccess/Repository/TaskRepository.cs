using AutomatedTaskSchedulingSystem.DataAccess.Data;
using AutomatedTaskSchedulingSystem.DataAccess.Repository.IRepository;
using AutomatedTaskSchedulingSystem.Models.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutomatedTaskSchedulingSystem.DataAccess.Repository
{
    public class TaskRepository : Repository<SetupTask>, ITaskRepository
    {
        private readonly ApplicationDbContext _db;

        public TaskRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;

        }

        //public void Save()
        // {
        //     _db.SaveChanges();
        // }

        public void Update(SetupTask obj)
        {
            _db.Tasks.Update(obj);
        }

    }
}
