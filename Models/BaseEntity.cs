using System;

namespace timetracker.Models
{
    public class BaseEntity
    {
        public long Id { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool Deleted { get; set; }
    }
}