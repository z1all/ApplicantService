using System.ComponentModel.DataAnnotations;

namespace UserService.Core.Application.DTOs
{
    public class CreateAdminRequestDTO
    {
        [Required]
        public string FullName { get; init; } = null!;
        [Required]
        [EmailAddress]
        public string Email { get; init; } = null!;
        [Required]
        public string Password { get; init; } = null!;
    }
}
