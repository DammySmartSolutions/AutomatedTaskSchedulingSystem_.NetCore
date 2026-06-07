using AutomatedTaskSchedulingSystem.Models.Model;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutomatedTaskSchedulingSystem.Models.ViewModel
{
    public class EmployeeVM
    {

        public Employee Employee { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> PositionList { get; set; }
    }
}
