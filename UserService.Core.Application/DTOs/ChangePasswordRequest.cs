using System.ComponentModel.DataAnnotations;

namespace UserService.Core.Application.DTOs
{
    public class ChangePasswordRequest
    {
        [Required]
        public string NewPassword { get; set; } = null!;
    }
}
