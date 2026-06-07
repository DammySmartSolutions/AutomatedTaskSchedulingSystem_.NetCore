using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace AutomatedTaskSchedulingSystem.Models.Model
{
    public class SetupTask
    {
         

        [Key]
        public int TaskID { get; set; }

        [Required]
        [Display(Name = "Task Name")]
        public string TaskName { get; set; }

        [Display(Name = "Task Location")]
        public int LocId { get; set; }
        [ForeignKey("LocId")]

        [ValidateNever]
        public Location Location { get; set; }

        [Required]
        [Display(Name = "Min No. of Employees that can be assigned")]
        [Range(1, 10, ErrorMessage = "Min. Number of Person must be at least 1.")]
        public int MinEmployees { get; set; }

        [Required]
        [Display(Name = "Max No. of Employees that can be assigned")]
        [Range(1, 10, ErrorMessage = "Max. Number of Person must be at least 1.")]
        public int MaxEmployees { get; set; }

       
    }
}
