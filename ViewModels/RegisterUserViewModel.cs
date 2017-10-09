using System.ComponentModel.DataAnnotations;

namespace timetracker.ViewModels
{
    public class RegisterUserViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password{ get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}