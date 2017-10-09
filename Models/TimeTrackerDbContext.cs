using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace timetracker.Models
{
    public class TimeTrackerDbContext : IdentityDbContext<IdentityUser>
    {
        public TimeTrackerDbContext(DbContextOptions<TimeTrackerDbContext> options) : base(options)
        {
        }
        
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<TimeEntry> TimeEntries { get; set; }
    }
}