using System.ComponentModel.DataAnnotations;

namespace UserService.Core.Application.DTOs
{
    public class ChangeEmailRequestDTO
    {
        [Required]
        [EmailAddress]
        public string NewEmail { get; set; } = null!;
    }
}
