using Common.Models.DTOs;
using System.ComponentModel.DataAnnotations;

namespace AmdinPanelMVC.Models
{
    public class ProfileViewModel
    {
        public required Guid Id { get; set; }
       // [MinLength(5, ErrorMessage = "Имя должно иметь длину не менее 5 символов")]
        public required string FullName { get; set; }
       // [MinLength(6, ErrorMessage = "Email должен иметь длину не менее 6 символов")]
        public required string Email { get; set; }
        public required FacultyDTO? Faculty { get; set; }

        public string? CurrentPassword { get; set; }
        public string? NewPassword { get; set; }
    }
}
