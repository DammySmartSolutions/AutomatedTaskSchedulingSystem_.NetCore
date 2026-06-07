using AutomatedTaskSchedulingSystem.DataAccess.Repository.IRepository;
using AutomatedTaskSchedulingSystem.Models.Model;
using AutomatedTaskSchedulingSystem.Models.ViewModel;
using AutomatedTaskSchedulingSystem.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace AutomatedTaskSchedulingSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ManageUserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ManageUserController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> EditRole(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var roles = await _userManager.GetRolesAsync(user);

            var vm = new ManageUserVM
            {
                Id = user.Id,
                EmpID = user.EmpID,
                Email = user.Email,
                Role = roles.FirstOrDefault(),
                RoleList = _roleManager.Roles.Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Name
                })
            };

            return View(vm);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRole(ManageUserVM vm)
        {
            var user = await _userManager.FindByIdAsync(vm.Id);
            if (user == null) return NotFound();

            var oldRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, oldRoles);

            await _userManager.AddToRoleAsync(user, vm.Role);

            TempData["success"] = "User role updated successfully.";
            return RedirectToAction(nameof(Index));
        }



        public async Task<IActionResult> ResetPassword(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            ViewBag.UserId = user.Id;
            ViewBag.EmpID = user.EmpID;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(string id, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

            if (result.Succeeded)
            {
                TempData["success"] = "Password reset successfully.";
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            ViewBag.UserId = user.Id;
            ViewBag.EmpID = user.EmpID;
            return View();
        }




        #region API CALLS

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = _userManager.Users.ToList();

            var userList = new List<object>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                userList.Add(new
                {
                    id = user.Id,
                    empID = user.EmpID,
                    email = user.Email,
                    role = roles.FirstOrDefault() ?? "No Role",
                    emailConfirmed = user.EmailConfirmed,
                    isLocked = user.LockoutEnd != null && user.LockoutEnd > DateTimeOffset.Now
                });
            }

            return Json(new { data = userList });
        }


        [HttpPost]
        public async Task<IActionResult> LockUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return Json(new { success = false, message = "User not found." });

            await _userManager.SetLockoutEnabledAsync(user, true);
            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.Now.AddYears(100));

            return Json(new { success = true, message = "User locked successfully." });
        }

        [HttpPost]
        public async Task<IActionResult> UnlockUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return Json(new { success = false, message = "User not found." });

            await _userManager.SetLockoutEndDateAsync(user, null);

            return Json(new { success = true, message = "User unlocked successfully." });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return Json(new { success = false, message = "User not found." });

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
                return Json(new { success = true, message = "User deleted successfully." });

            return Json(new { success = false, message = "Unable to delete user." });
        }



        #endregion


    }
}
