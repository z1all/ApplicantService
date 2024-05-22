using System.ComponentModel.DataAnnotations;

namespace UserService.Core.Application.DTOs
{
    public class CreateAdminRequestDTO
    {
        [Required]
        public required string FullName { get; init; } 
        [Required]
        [EmailAddress]
        public required string Email { get; init; }
        [Required]
        public required string Password { get; init; }
    }
}
