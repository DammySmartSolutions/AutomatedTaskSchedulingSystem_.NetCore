using AutomatedTaskSchedulingSystem.DataAccess.Data;
using AutomatedTaskSchedulingSystem.DataAccess.Repository.IRepository;
using AutomatedTaskSchedulingSystem.Models.Model;
using AutomatedTaskSchedulingSystem.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutomatedTaskSchedulingSystem.Services
{
    public class GenerateScheduleService : IGenerateScheduleService
    {

        private readonly IUnitOfWork _unitOfWork;

        Utilities Utility = new Utilities();

        private readonly ApplicationDbContext _db;

        public GenerateScheduleService(ApplicationDbContext db) 
        {
            _db = db;
        }
       
        public string GenerateTaskSchedule(DateTime scheduleDate)
        {

            // 1. Check if schedule already exists for the date
            if (_db.Schedules.Any(s => s.SchDate == scheduleDate))
            {

                var tasksForDate = _db.Schedules.Where(s => s.SchDate == scheduleDate).ToList();

                if (tasksForDate.Any())
                {
                    _db.Schedules.RemoveRange(tasksForDate);
                    _db.SaveChanges();
                }

            }

            // 2. Load available employees for the day
            var availableEmployees = (from emp in _db.Employee
                                      join avail in _db.EmployeeAvailability on emp.EmpID equals avail.EmpID
                                      where emp.PosId == 1 && avail.AvailDate == scheduleDate && avail.Avail == true
                                      select new
                                      {
                                          emp.EmpID,
                                          FullName = emp.FirstName + " " + emp.LastName,
                                          emp.Sex
                                      }).ToList();



            
            if (!availableEmployees.Any())
            {

                return "No employee available";
            }


            // Shuffle employees to improve distribution
            availableEmployees = availableEmployees.OrderBy(e => Guid.NewGuid()).ToList();

            // 3. Get all tasks joined with their locations
            var tasks = (from task in _db.Tasks
                         join loc in _db.Location on task.LocId equals loc.LocId
                         orderby task.TaskID
                         select new
                         {
                             task.TaskName,
                             Location = loc.Name,
                             task.MinEmployees,
                             task.MaxEmployees
                         }).ToList();



            if (!tasks.Any())
            {

                return "No Task Setup";


            }

            // 4. Define equivalent task pairs
            var equivalentTasks = new Dictionary<string, string>
    {
        {"Trk Unloader", "Trailer Unloader"},
        {"Splitter Express", "Splitter Ground"},
        {"Smalls Sorter", "Shuttle Tls"},
        {"Smalls P-Scanner", "Ib Scanner"},
        {"Tls Express Small", "Trk Loader"},
        {"Cpost Express", "Cpost Ground"},
        {"Van Load Scanner Express", "Van Load Scanner Ground"},
        {"Van Loader Express", "Van Loader Ground"},
        {"P-Scanner", "Status Scan"},
        {"Tls  Scanner Express", "Tls Ground"},
        {"Express Floor Pallet Load", "Floor Pallet Load Ground"},
    };

            // 5. Determine previous schedule date if any
            var previousDate = _db.Schedules
                                  .Where(s => s.SchDate < scheduleDate)
                                  .OrderByDescending(s => s.SchDate)
                                  .Select(s => s.SchDate)
                                  .FirstOrDefault();

            var previousAssignments = _db.Schedules
                                         .Where(s => s.SchDate == previousDate)
                                         .AsEnumerable()
                                         .SelectMany(s => s.Name.Split(',').Select(name => new { name = name.Trim(), s.Task }))
                                         .GroupBy(x => x.name)
                                         .ToDictionary(g => g.Key, g => g.Select(x => x.Task).ToHashSet());

            var schedule = new List<Schedule>();
            var assignedEmployees = new HashSet<string>();

            foreach (var task in tasks)
            {
                if (equivalentTasks.ContainsValue(task.TaskName))
                    continue; // Skip equivalent pair (will be handled with main task)

                int min = (int)task.MinEmployees;
                int max = (int)task.MaxEmployees;

                // Select eligible employees
                var eligible = availableEmployees
                    .Where(e => !assignedEmployees.Contains(e.EmpID)
                             && !(task.TaskName == "Trailer Unloader" && e.Sex == 'F')
                             && (!previousAssignments.ContainsKey(e.FullName) || !previousAssignments[e.FullName].Contains(task.TaskName)))
                    .ToList();

                var selected = eligible.Take(max).ToList();

                if (selected.Count < min)
                {
                    // Try to fill with any unassigned employee (even if they repeated a task)
                    var fallback = availableEmployees
                        .Where(e => !assignedEmployees.Contains(e.EmpID)
                                 && !(task.TaskName == "Trailer Unloader" && e.Sex == 'F'))
                        .Except(selected)
                        .Take(min - selected.Count)
                        .ToList();

                    selected.AddRange(fallback);
                }

                if (selected.Count == 0) continue;

                var fullNames = selected.Select(e => e.FullName).ToList();
                foreach (var emp in selected)
                    assignedEmployees.Add(emp.EmpID);

                schedule.Add(new Schedule
                {
                    SchDate = scheduleDate,
                    Location = task.Location,
                    Task = task.TaskName,
                    Name = string.Join(", ", fullNames)
                });

                // Add equivalent task if applicable
                if (equivalentTasks.ContainsKey(task.TaskName))
                {
                    var eqTask = equivalentTasks[task.TaskName];
                    var eqLocation = tasks.FirstOrDefault(t => t.TaskName == eqTask)?.Location;
                    if (!string.IsNullOrEmpty(eqLocation))
                    {
                        schedule.Add(new Schedule
                        {
                            SchDate = scheduleDate,
                            Location = eqLocation,
                            Task = eqTask,
                            Name = string.Join(", ", fullNames)
                        });
                    }
                }
            }

            // 6. Save



            //_db.tblSchedule.AddRange(schedule);
            //_db.SaveChanges();

            //return "created";


            // 6. Save
            if (schedule.Any())
            {
                _db.Schedules.AddRange(schedule);
                _db.SaveChanges();
            }

            return "created";




        }
    }
}
