using System.ComponentModel.DataAnnotations;
using Common.Models.Attributes;
using Common.Models.DTOs.Dictionary;

namespace AmdinPanelMVC.Models
{
    public class ProfileViewModel
    {
        public required Guid Id { get; set; }
        [MinLength(5, ErrorMessage = "Имя должно иметь длину не менее 5 символов")]
        public required string FullName { get; set; }
        [MinLength(5, ErrorMessage = "Email должен иметь длину не менее 5 символов")]
        public required string Email { get; set; }
        public required FacultyDTO? Faculty { get; set; }

        public string? CurrentPassword { get; set; }
        [Password(ErrorMessage = "Пароль должен быть более 6 символов и содержать хотя бы одну цифру и хотя бы одну заглавную букву")]
        public string? NewPassword { get; set; }
    }
}
