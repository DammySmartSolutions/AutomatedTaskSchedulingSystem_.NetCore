using System;
using System.Collections.Generic;
using System.Text;

namespace AutomatedTaskSchedulingSystem.Services
{
    public interface IGenerateScheduleService
    {
        string GenerateTaskSchedule(DateTime scheduleDate);
    }
}
