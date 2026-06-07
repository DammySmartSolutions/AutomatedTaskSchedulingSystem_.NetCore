using System;
using System.Collections.Generic;
using System.Text;

namespace AutomatedTaskSchedulingSystem.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
            
        IOrganizationRepository Organization { get; }

        ILocationRepository Location   { get; }

        ITaskRepository Task { get; }

        IPositionRepository Position { get; }

        IEmployeeRepository Employee { get; }

        IScheduleRepository Schedule { get; }

        IEmployeeAvailabilityRepository EmployeeAvailability { get; }

        void Save();
    }
}
