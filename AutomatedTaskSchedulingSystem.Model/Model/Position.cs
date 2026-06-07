using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AutomatedTaskSchedulingSystem.Models.Model
{
    public class Position
    {

        [Key]
        public int PosId { get; set; }

        [Required]
        [Display(Name = "Position Name")]
        public string Name { get; set; }
    }
}
