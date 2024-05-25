using Common.Models.Attributes;
using System.ComponentModel.DataAnnotations;

namespace AmdinPanelMVC.Models
{
    public class CreateAndChangeManagerViewModel
    {
        public required Guid? Id { get; set; }
        [MinLength(5, ErrorMessage = "Имя должно иметь длину не менее 5 символов")]
        public required string FullName { get; set; }
        public required string Email { get; set; }
        [Password(ErrorMessage = "Пароль должен быть более 6 символов и содержать хотя бы одну цифру и хотя бы одну заглавную букву")]
        public required string? Password { get; set; }
        public Guid? FacultyId { get; set; } 
    }
}
