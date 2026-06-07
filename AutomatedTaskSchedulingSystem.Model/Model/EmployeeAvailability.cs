using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AutomatedTaskSchedulingSystem.Models.Model
{
    [Index(nameof(EmpID), nameof(AvailDate))]
    public class EmployeeAvailability
    {

        [Key]
        public int AvailID { get; set; }

        [Required]
        [Display(Name = "Employee")]
        public string EmpID { get; set; }

        [ForeignKey("EmpID")]
        [ValidateNever]
        public Employee Employee { get; set; }

        [Required]
        [Display(Name = "Available Date")]
        [DataType(DataType.Date)]
        public DateTime AvailDate { get; set; }

        [Display(Name = "Available")]
        public bool Avail { get; set; }



    }
}
