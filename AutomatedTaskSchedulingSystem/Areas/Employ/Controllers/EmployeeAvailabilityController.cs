using AutomatedTaskSchedulingSystem.DataAccess.Repository.IRepository;
using AutomatedTaskSchedulingSystem.Models.Model;
using AutomatedTaskSchedulingSystem.Models.ViewModel;
using AutomatedTaskSchedulingSystem.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data.SqlTypes;

namespace AutomatedTaskSchedulingSystem.Areas.Employ.Controllers
{
    [Area("Employ")]
    [Authorize(Roles = "Admin, Employee")]

    public class EmployeeAvailabilityController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        Utilities Utility = new Utilities();


        public EmployeeAvailabilityController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
         
        }


        public IActionResult Index()
        {
            List<EmployeeAvailability> objEmpAvailList = _unitOfWork.EmployeeAvailability.GetAll(includeProperties: "Employee").ToList();

            return View(objEmpAvailList);
        }


        public IActionResult Upsert(int? id)
        {
            EmployeeAvailVM employeeAvailVM = new()
            {
                EmployeeList = _unitOfWork.Employee.GetAll().Select(e => new SelectListItem
                {
                    Text = e.FirstName + " " + e.LastName + " - " + e.EmpID,
                    Value = e.EmpID
                }),
                EmployeeAvailability = new EmployeeAvailability()
            };

            if (id == null || id == 0)
            {
                employeeAvailVM.EmployeeAvailability.AvailDate = DateTime.Today;
                return View(employeeAvailVM);
            }

            employeeAvailVM.EmployeeAvailability =
                _unitOfWork.EmployeeAvailability.Get(u => u.AvailID == id);

            return View(employeeAvailVM);
        }


        [HttpPost]
        public IActionResult Upsert(EmployeeAvailVM employeeAvailVM)
        {
            if (!ModelState.IsValid)
            {
                employeeAvailVM.EmployeeList = _unitOfWork.Employee.GetAll().Select(e => new SelectListItem
                {
                    Text = e.FirstName + " " + e.LastName + " - " + e.EmpID,
                    Value = e.EmpID
                });

                return View(employeeAvailVM);
            }

            var existingRecord = _unitOfWork.EmployeeAvailability.Get(u =>
                u.EmpID == employeeAvailVM.EmployeeAvailability.EmpID &&
                u.AvailDate == employeeAvailVM.EmployeeAvailability.AvailDate);

            if (existingRecord != null)
            {
                existingRecord.Avail = employeeAvailVM.EmployeeAvailability.Avail;

                _unitOfWork.EmployeeAvailability.Update(existingRecord);
                TempData["success"] = "Employee Availability updated successfully.";
            }
            else
            {
                _unitOfWork.EmployeeAvailability.Add(employeeAvailVM.EmployeeAvailability);
                TempData["success"] = "New Employee Availability added successfully.";
            }

            _unitOfWork.Save();

            var newVM = new EmployeeAvailVM
            {
                EmployeeAvailability = new EmployeeAvailability
                {
                    AvailDate = DateTime.Today,
                    Avail = true
                },
                EmployeeList = _unitOfWork.Employee.GetAll().Select(e => new SelectListItem
                {
                    Text = e.FirstName + " " + e.LastName + " - " + e.EmpID,
                    Value = e.EmpID
                })
            };

            return View(newVM);
        }





        #region API CALLS

        public IActionResult GetAll()
        {
            List<EmployeeAvailability> objEmpAvailList = _unitOfWork.EmployeeAvailability.GetAll(includeProperties: "Employee").ToList();
            return Json(new { data = objEmpAvailList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var EmpToBeDeleted = _unitOfWork.EmployeeAvailability.Get(u => u.AvailID == id);
            if (EmpToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }




            _unitOfWork.EmployeeAvailability.Remove(EmpToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Employee Availability delete Successful" });
        }



        #endregion





    }
}
