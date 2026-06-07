using AutomatedTaskSchedulingSystem.DataAccess.Repository.IRepository;
using AutomatedTaskSchedulingSystem.Models.Model;
using AutomatedTaskSchedulingSystem.Models.ViewModel;
using AutomatedTaskSchedulingSystem.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AutomatedTaskSchedulingSystem.Areas.Employ.Controllers
{
    [Area("Employ")]
    [Authorize(Roles = "Admin, Employee")]
    public class EmployeesController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        Utilities Utility = new Utilities();


        public EmployeesController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;

        }
        public IActionResult Index()
        {
            List<Employee> objEmpList = _unitOfWork.Employee.GetAll(includeProperties: "Position").ToList();

            return View(objEmpList);
        }

        public IActionResult Upsert(int? id)
        {
            EmployeeVM employeeVM = new()
            {
                PositionList = _unitOfWork.Position.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.PosId.ToString()
                }),
                Employee = new Employee()
            };

            if (id == null || id == 0)
            {
                return View(employeeVM);
            }

            employeeVM.Employee = _unitOfWork.Employee.Get(u => u.ID == id);

            if (employeeVM.Employee == null)
            {
                return NotFound();
            }

            return View(employeeVM);
        }



        [HttpPost]
        public IActionResult Upsert(EmployeeVM employeeVM)
        {
            if (!ModelState.IsValid)
            {
                employeeVM.PositionList = _unitOfWork.Position.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.PosId.ToString()
                });

                return View(employeeVM);
            }

            employeeVM.Employee.FirstName = Utility.ToSentenceCase(employeeVM.Employee.FirstName);
            employeeVM.Employee.LastName = Utility.ToSentenceCase(employeeVM.Employee.LastName);

            if (employeeVM.Employee.ID == 0)
            {
                var existingEmpID = _unitOfWork.Employee
                    .Get(u => u.EmpID == employeeVM.Employee.EmpID);

                if (existingEmpID != null)
                {
                    TempData["error"] = "Employee ID already exists.";
                    employeeVM.PositionList = _unitOfWork.Position.GetAll().Select(u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.PosId.ToString()
                    });

                    return View(employeeVM);
                }

                _unitOfWork.Employee.Add(employeeVM.Employee);
                TempData["success"] = "New Employee added successfully.";
            }
            else
            {
                _unitOfWork.Employee.Update(employeeVM.Employee);
                TempData["success"] = "Employee data updated successfully.";
            }

            _unitOfWork.Save();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                TempData["error"] = "Please select a CSV file.";
                return RedirectToAction("Upsert");
            }

            if (Path.GetExtension(file.FileName).ToLower() != ".csv")
            {
                TempData["error"] = "Only CSV files are allowed.";
                return RedirectToAction("Upsert");
            }

            var employees = new List<Employee>();

            using var reader = new StreamReader(file.OpenReadStream());

            bool isHeader = true;
            int rowNumber = 1;

            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();
                rowNumber++;

                if (isHeader)
                {
                    isHeader = false;
                    continue;
                }

                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var values = line.Split(',');

                if (values.Length < 5)
                {
                    TempData["error"] = $"Invalid data format at row {rowNumber}.";
                    return RedirectToAction("Upsert");
                }

                if (!int.TryParse(values[4].Trim(), out int PosId))
                {
                    TempData["error"] = $"Invalid PositionId at row {rowNumber}.";
                    return RedirectToAction("Upsert");
                }

                var sexValue = values[3].Trim();

                if (sexValue.Length != 1)
                {
                    TempData["error"] = $"Invalid Sex value at row {rowNumber}.";
                    return RedirectToAction("Upsert");
                }

                employees.Add(new Employee
                {
                    EmpID = values[0].Trim(),
                    FirstName = Utility.ToSentenceCase(values[1].Trim()),
                    LastName = Utility.ToSentenceCase(values[2].Trim()),
                    Sex = char.ToUpper(sexValue[0]),
                    PosId = PosId
                });
            }

            // Validate EmpID uniqueness

            var duplicateInFile = employees
                    .GroupBy(e => e.EmpID)
                    .Where(g => g.Count() > 1)
                    .Select(g => g.Key)
                    .ToList();

            if (duplicateInFile.Any())
            {
                TempData["error"] = $"Duplicate Employee ID found in file: {string.Join(", ", duplicateInFile)}";
                return RedirectToAction("Upsert");
            }

            var existingEmpIds = _unitOfWork.Employee.GetAll()
                .Select(e => e.EmpID)
                .ToHashSet();

            var duplicateInDatabase = employees
                .Where(e => existingEmpIds.Contains(e.EmpID))
                .Select(e => e.EmpID)
                .ToList();

            if (duplicateInDatabase.Any())
            {
                TempData["error"] = $"Employee ID already exists: {string.Join(", ", duplicateInDatabase)}";
                return RedirectToAction("Upsert");
            }




            // End validation

            foreach (var employee in employees)
            {
                _unitOfWork.Employee.Add(employee);
            }

            _unitOfWork.Save();

            TempData["success"] = $"{employees.Count} employees uploaded successfully.";
            return RedirectToAction("Index");
        }



        #region API CALLS

        public IActionResult GetAll()
        {
            List<Employee> objEmpList = _unitOfWork.Employee.GetAll(includeProperties: "Position").ToList();
            return Json(new { data = objEmpList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var EmpToBeDeleted = _unitOfWork.Employee.Get(u => u.ID == id);
            if (EmpToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

           


            _unitOfWork.Employee.Remove(EmpToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Employee delete Successful" });
        }



        #endregion


    }
}
