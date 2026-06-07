using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AutomatedTaskSchedulingSystem.Models.ViewModel
{
    public class GenerateScheduleVM
    {

        [Required(ErrorMessage = "Please select date")]
        [DataType(DataType.Date)]
        [Display(Name = "Schedule Date")]
        public DateTime ScheduleDate { get; set; } = DateTime.Today;
    }
}
