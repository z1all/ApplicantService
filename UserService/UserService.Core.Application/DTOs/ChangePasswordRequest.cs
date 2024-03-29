using System.ComponentModel.DataAnnotations;

namespace UserService.Core.Application.DTOs
{
    public class ChangePasswordRequest
    {
        [Required]
        public string CurrentPassword { get; set; } = null!;
        [Required]
        public string NewPassword { get; set; } = null!;
    }
}
