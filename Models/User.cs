using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace timetracker.Models
{
    public class User : BaseEntity
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Username { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public ICollection<Project> Projects { get; private set; }
    }
}