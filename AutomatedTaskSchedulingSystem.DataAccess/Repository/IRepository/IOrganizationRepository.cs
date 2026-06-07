using AutomatedTaskSchedulingSystem.Models.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutomatedTaskSchedulingSystem.DataAccess.Repository.IRepository
{
    public interface IOrganizationRepository : IRepository<SetupOrganization>
    {
        void Update(SetupOrganization obj);
    }
}
