using AutomatedTaskSchedulingSystem.DataAccess.Repository.IRepository;
using AutomatedTaskSchedulingSystem.Models.Model;
using AutomatedTaskSchedulingSystem.Models.ViewModel;
using AutomatedTaskSchedulingSystem.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AutomatedTaskSchedulingSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class TaskController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        Utilities Utility = new Utilities();


        public TaskController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
          
        }



        public IActionResult Index()
        {
            List<SetupTask> objTaskList = _unitOfWork.Task.GetAll(includeProperties: "Location").ToList();

            return View(objTaskList);

          
        }


        


        public IActionResult Upsert(int? id)
        {
            TaskVM taskVM = new()
            {
                LocationList = _unitOfWork.Location.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.LocId.ToString()
                }),
                Tasks = new SetupTask()
            };

            if (id == null || id == 0)
            {
                return View(taskVM);
            }

            taskVM.Tasks = _unitOfWork.Task.Get(u => u.TaskID == id);

            if (taskVM.Tasks == null)
            {
                return NotFound();
            }

            return View(taskVM);
        }



        [HttpPost]
        public IActionResult Upsert(TaskVM taskVM)
        {

            if (ModelState.IsValid)
            {
                if (taskVM.Tasks.TaskID == 0)
                {
                    taskVM.Tasks.TaskName = Utility.ToSentenceCase(taskVM.Tasks.TaskName);

                    _unitOfWork.Task.Add(taskVM.Tasks);
                    _unitOfWork.Save();
                    TempData["success"] = "Task created successfully";
                }
                else
                {
                    taskVM.Tasks.TaskName = Utility.ToSentenceCase(taskVM.Tasks.TaskName); 

                    _unitOfWork.Task.Update(taskVM.Tasks);
                    _unitOfWork.Save();

                    TempData["success"] = "Task updated successfully";
                }

               


                return RedirectToAction("Index");

            }

            else
            {
                taskVM.LocationList = _unitOfWork.Location.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.LocId.ToString()
                });
                return View(taskVM);


                
            }


        }






        #region API CALLS

        public IActionResult GetAll(int id)
        {
            List<SetupTask> objTaskList = _unitOfWork.Task.GetAll(includeProperties: "Location").ToList();
            return Json(new { data = objTaskList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var TaskToBeDeleted = _unitOfWork.Task.Get(u => u.TaskID == id);
            if (TaskToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }


            _unitOfWork.Task.Remove(TaskToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Task Delete Successful" });
        }




        #endregion





    }
}
