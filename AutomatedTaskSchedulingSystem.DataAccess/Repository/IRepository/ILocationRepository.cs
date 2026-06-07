using AutomatedTaskSchedulingSystem.Models.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutomatedTaskSchedulingSystem.DataAccess.Repository.IRepository
{
    public interface ILocationRepository : IRepository<Location>
    {
        void Update(Location obj);
    }
}
