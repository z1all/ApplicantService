using System.ComponentModel.DataAnnotations;

namespace UserService.Core.Application.DTOs
{
    public class ChangeEmailRequest
    {
        [Required]
        public string NewEmail { get; set; } = null!;
    }
}
