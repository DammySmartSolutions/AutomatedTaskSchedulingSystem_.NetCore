using AutomatedTaskSchedulingSystem.Models.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutomatedTaskSchedulingSystem.DataAccess.Repository.IRepository
{
    public interface ITaskRepository : IRepository<SetupTask>
    {

             
            void Update(SetupTask obj);
        
    }
}
