using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AutomatedTaskSchedulingSystem.Models.Model
{
    public class Location
    {

        [Key]
        public int LocId { get; set; }

        [Required]
        [Display(Name = "Location Name")]
        public string Name { get; set; }

      
    }
}
