using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AutomatedTaskSchedulingSystem.Models.Model
{
    public class SetupOrganization
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Organization ID")]
        public string OrgID { get; set; }

        [Required]
        [Display(Name = "Organization Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Telephone Number")]
        [Phone]
        [StringLength(20, MinimumLength = 7, ErrorMessage = "Telephone number must be between 7 and 20 characters.")]
        public string Telephone { get; set; }

    }
}
