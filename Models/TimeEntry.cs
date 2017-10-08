using System;
using System.ComponentModel.DataAnnotations;

namespace timetracker.Models
{
    public class TimeEntry : BaseEntity
    {        
        [Required]
        public int MinutesWorked { get; set; }

        public long ProjectId { get; set; }
        public Project Project { get; set; }
    }
}