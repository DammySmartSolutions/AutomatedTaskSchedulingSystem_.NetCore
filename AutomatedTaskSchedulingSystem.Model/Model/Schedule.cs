using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutomatedTaskSchedulingSystem.Models.Model
{
    [Index(nameof(SchDate))]
    public class Schedule
    {
        public int Id { get; set; }
        public DateTime SchDate { get; set; }
        public string Location { get; set; }
        public string Task { get; set; }
        public string Name { get; set; }
    }
}
