using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace AutomatedTaskSchedulingSystem.Models.Model
{
    public class ApplicationUser : IdentityUser
    {
       

        [Required]
        public string EmpID { get; set; }

            
  
        public string Role { get; set; }
    }
}
