using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AutomatedTaskSchedulingSystem.Models.Model
{
    [Microsoft.EntityFrameworkCore.Index(nameof(EmpID), IsUnique = true)]
    public class Employee
    {

       
            [Key]
            public int ID { get; set; }

            [Required]
            [Display(Name = "Employee ID")]
            [StringLength(20, MinimumLength = 3, ErrorMessage = "Employee ID must be between 3 and 20 characters.")]
            public string EmpID { get; set; }

            [Required]
            [Display(Name = "First Name")]
            [StringLength(20, MinimumLength = 2, ErrorMessage = "First Name must be between 2 and 20 characters.")]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "Last Name")]
            [StringLength(20, MinimumLength = 2, ErrorMessage = "Last Name must be between 2 and 20 characters.")]
            public string LastName { get; set; }

            [Required]
            [Display(Name = "Sex")]
            public char Sex { get; set; }

            [Required]
            [Display(Name = "Employee Position")]
            public int PosId { get; set; }

            [ForeignKey("PosId")]
            [ValidateNever]
            public Position Position { get; set; }
     

    }
}
