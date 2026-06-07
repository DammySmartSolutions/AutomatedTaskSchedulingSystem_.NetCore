using AutomatedTaskSchedulingSystem.DataAccess.Repository.IRepository;
using AutomatedTaskSchedulingSystem.Models.Model;
using AutomatedTaskSchedulingSystem.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutomatedTaskSchedulingSystem.Areas.Admin.Controllers
{
   
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class LocationController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        Utilities Utility = new Utilities();

        public LocationController(IUnitOfWork db)
        {
            _unitOfWork = db;
        }


        public IActionResult Index()
        {

            List<Location> LocList = _unitOfWork.Location.GetAll().ToList();
            return View(LocList);
        }


        public IActionResult Upsert(int? id)
        {

            if (id == null || id == 0)
            {
                //create
                return View(new Location());
            }
            else
            {
                //update
                Location locObj = _unitOfWork.Location.Get(u => u.LocId == id);
                return View(locObj);
            }

        }


        [HttpPost]
        public IActionResult Upsert(Location Loc)
        {

            if (ModelState.IsValid)
            {
                if (Loc.LocId == 0)
                {
                    Loc.Name = Utility.ToSentenceCase(Loc.Name);
                   
                    _unitOfWork.Location.Add(Loc);
                    TempData["success"] = "Location created successfully";
                }
                else
                {
                    Loc.Name = Utility.ToSentenceCase(Loc.Name);
                   
                    _unitOfWork.Location.Update(Loc);
                    TempData["success"] = "Location updated successfully";
                }

                _unitOfWork.Save();


                return RedirectToAction("Index");

            }

            else
            {

                return View(Loc);
            }


        }

        #region API CALLS

        public IActionResult GetAll(int id)
        {
            List<Location> LocList = _unitOfWork.Location.GetAll().ToList();
            return Json(new { data = LocList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var LocToBeDeleted = _unitOfWork.Location.Get(u => u.LocId == id);
            if (LocToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }


            _unitOfWork.Location.Remove(LocToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }




        #endregion

    }
}
