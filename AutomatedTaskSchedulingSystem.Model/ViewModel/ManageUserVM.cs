using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutomatedTaskSchedulingSystem.Models.ViewModel
{
    public class ManageUserVM
    {
        public string Id { get; set; }
        public string EmpID { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public IEnumerable<SelectListItem> RoleList { get; set; }
    }
}
