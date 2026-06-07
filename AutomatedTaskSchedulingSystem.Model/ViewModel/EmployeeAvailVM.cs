using AutomatedTaskSchedulingSystem.Models.Model;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutomatedTaskSchedulingSystem.Models.ViewModel
{
    public class EmployeeAvailVM
    {

        public EmployeeAvailability EmployeeAvailability { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> EmployeeList { get; set; }
    }
}
