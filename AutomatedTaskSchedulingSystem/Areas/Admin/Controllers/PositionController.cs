using AutomatedTaskSchedulingSystem.DataAccess.Repository.IRepository;
using AutomatedTaskSchedulingSystem.Models.Model;
using AutomatedTaskSchedulingSystem.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutomatedTaskSchedulingSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class PositionController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        Utilities Utility = new Utilities();

        public PositionController(IUnitOfWork db)
        {
            _unitOfWork = db;
        }

        public IActionResult Index()
        {
            List<Position> PosList = _unitOfWork.Position.GetAll().ToList();
            return View(PosList);
          
        }


        public IActionResult Upsert(int? id)
        {

            if (id == null || id == 0)
            {
                //create
                return View(new Position());
            }
            else
            {
                //update
                Position posObj = _unitOfWork.Position.Get(u => u.PosId == id);
                return View(posObj);
            }

        }


        [HttpPost]
        public IActionResult Upsert(Position Pos)
        {

            if (ModelState.IsValid)
            {
                if (Pos.PosId == 0)
                {
                    Pos.Name = Utility.ToSentenceCase(Pos.Name);

                    _unitOfWork.Position.Add(Pos);
                    TempData["success"] = "Position created successfully";
                }
                else
                {
                    Pos.Name = Utility.ToSentenceCase(Pos.Name);

                    _unitOfWork.Position.Update(Pos);
                    TempData["success"] = "Position updated successfully";
                }

                _unitOfWork.Save();


                return RedirectToAction("Index");

            }

            else
            {

                return View(Pos);
            }


        }

        #region API CALLS

        public IActionResult GetAll(int id)
        {
            List<Position> PosList = _unitOfWork.Position.GetAll().ToList();
            return Json(new { data = PosList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var PosToBeDeleted = _unitOfWork.Position.Get(u => u.PosId == id);
            if (PosToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }


            _unitOfWork.Position.Remove(PosToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Position deleted successfully" });
        }




        #endregion


    }
}
