using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace timetracker.Models
{
    public class Project : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }
        public ICollection<TimeEntry> TimeEntries { get; private set; }

        public Guid UserId { get; set; }
    }
}