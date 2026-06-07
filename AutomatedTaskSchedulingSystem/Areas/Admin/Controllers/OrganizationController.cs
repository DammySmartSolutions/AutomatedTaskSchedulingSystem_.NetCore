using AutomatedTaskSchedulingSystem.DataAccess.Repository.IRepository;
using AutomatedTaskSchedulingSystem.Models.Model;
using AutomatedTaskSchedulingSystem.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Net.WebRequestMethods;

namespace AutomatedTaskSchedulingSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class OrganizationController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        Utilities Utility = new Utilities();

        public OrganizationController(IUnitOfWork db)
        {
            _unitOfWork = db;
        }


        public IActionResult Index()
        {

            List<SetupOrganization> OrgList = _unitOfWork.Organization.GetAll().ToList();
            return View(OrgList);
        }


        public IActionResult Upsert(int? id)
        {

            if (id == null || id == 0)
            {
                //create
                return View(new SetupOrganization());
            }
            else
            {
                //update
                SetupOrganization orgObj = _unitOfWork.Organization.Get(u => u.Id == id);
                return View(orgObj);
            }

        }


        [HttpPost]
        public IActionResult Upsert(SetupOrganization Org)
        {

            if (ModelState.IsValid)
            {
                if (Org.Id == 0)
                {
                    Org.Name = Utility.ToSentenceCase(Org.Name);
                    Org.Address = Utility.ToSentenceCase(Org.Address);
                    _unitOfWork.Organization.Add(Org);
                    TempData["success"] = "Organization created successfully";
                }
                else
                {
                    Org.Name = Utility.ToSentenceCase(Org.Name);
                    Org.Address = Utility.ToSentenceCase(Org.Address);
                    _unitOfWork.Organization.Update(Org);
                    TempData["success"] = "Organization updated successfully";
                }

                _unitOfWork.Save();


                return RedirectToAction("Index");

            }
                                    
            else
            {
                
                return View(Org);
            }


        }
        #region API CALLS

        public IActionResult GetAll(int id)
        {
            List<SetupOrganization> OrgList = _unitOfWork.Organization.GetAll().ToList();
            return Json(new { data = OrgList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var orgToBeDeleted = _unitOfWork.Organization.Get(u => u.Id == id);
            if (orgToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            
            _unitOfWork.Organization.Remove(orgToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }




        #endregion



    }
}
