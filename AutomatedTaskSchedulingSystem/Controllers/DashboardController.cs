using AutomatedTaskSchedulingSystem.DataAccess.Repository.IRepository;
using AutomatedTaskSchedulingSystem.Models.ViewModel;
using AutomatedTaskSchedulingSystem.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutomatedTaskSchedulingSystem.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
      

     
        public DashboardController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
           

        }
        public IActionResult Index()
        {
            var vm = new DashboardVM
            {
               TotalEmployees = _unitOfWork.Employee.GetAll().Count(),
                TotalTasks = _unitOfWork.Task.GetAll().Count(),
                TotalLocations = _unitOfWork.Location.GetAll().Count(),
                TotalSchedules = _unitOfWork.Schedule.GetAll().Count()
            };

            return View(vm);


            
        }

        #region API Call


        [HttpGet]
        public IActionResult GetAvailabilityChart()
        {
            DateTime today = DateTime.Today;

            int available = _unitOfWork.EmployeeAvailability
                .GetAll(u => u.AvailDate == today && u.Avail == true)
                .Count();

            int unavailable = _unitOfWork.Employee.GetAll().Count() - available;

            return Json(new
            {
                labels = new[] { "Available", "Unavailable" },
                data = new[] { available, unavailable }
            });
        }




        #endregion




    }
}
