using AutomatedTaskSchedulingSystem.Models.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutomatedTaskSchedulingSystem.DataAccess.Repository.IRepository
{
    public interface IPositionRepository : IRepository<Position>
    {
               
                 void Update(Position obj);
        
    }
}
