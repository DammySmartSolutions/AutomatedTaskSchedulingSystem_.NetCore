using AutomatedTaskSchedulingSystem.DataAccess.Repository.IRepository;
using AutomatedTaskSchedulingSystem.Models.ViewModel;
using AutomatedTaskSchedulingSystem.Services;
using AutomatedTaskSchedulingSystem.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stimulsoft.Report;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Mvc;
using Stimulsoft.Report.Web;
namespace AutomatedTaskSchedulingSystem.Areas.Employ.Controllers
{
    [Area("Employ")]
    [Authorize(Roles = "Admin, Employee")]

    public class ScheduleController : Controller
    {


        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenerateScheduleService _generateScheduleService;

        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        Utilities Utility = new Utilities();

        public ScheduleController(
             IGenerateScheduleService generateScheduleService,
             IUnitOfWork unitOfWork,
             IWebHostEnvironment webHostEnvironment,
             IConfiguration configuration)
        {
            _generateScheduleService = generateScheduleService;
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }



        public IActionResult Index()
        {
            var vm = new GenerateScheduleVM
            {
                ScheduleDate = DateTime.Today
            };

            return View(vm);

           
        }


               

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Generate(GenerateScheduleVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            DateTime scheduleDate = vm.ScheduleDate.Date;

            var result = _generateScheduleService.GenerateTaskSchedule(scheduleDate.Date);

            if (result == "created")
            {
                return RedirectToAction("Report", new { date = scheduleDate.ToString("yyyy-MM-dd") });
            }

            TempData["error"] = result;
            return RedirectToAction("Index");
        }


        

        public IActionResult Report(DateTime date)
        {
            ViewBag.ReportDate = date.ToString("yyyy-MM-dd");
            return View();
        }

        public IActionResult GetReport(DateTime date)
        {
            var report = new StiReport();

            var reportPath = Path.Combine(
                _webHostEnvironment.WebRootPath,
                "Report",
                "Schedule.mrt"
            );

            report.Load(reportPath);

            var connectionString = _configuration.GetConnectionString("ReportConnection");

            report.Dictionary.Databases.Clear();
            report.Dictionary.Databases.Add(
                new Stimulsoft.Report.Dictionary.StiSqlDatabase(
                    "ATSS",
                    "ATSS",
                    connectionString,
                    false
                )
            );

            report["@date"] = date.Date;



            return StiNetCoreViewer.GetReportResult(this, report);
        }

        public IActionResult ViewerEvent()
        {
            return StiNetCoreViewer.ViewerEventResult(this);
        }


        public IActionResult Print()
        {
            var vm = new GenerateScheduleVM
            {
                ScheduleDate = DateTime.Today
            };

            return View(vm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Print(GenerateScheduleVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            DateTime scheduleDate = vm.ScheduleDate.Date;

            bool scheduleExists = _unitOfWork.Schedule
                .GetAll(u => u.SchDate == scheduleDate)
                .Any();

            if (!scheduleExists)
            {
                TempData["error"] = "Schedule has not been generated for the selected date.";
                return View(vm);
            }

            return RedirectToAction("Report", new { date = scheduleDate.ToString("yyyy-MM-dd") });
        }
    }
}
