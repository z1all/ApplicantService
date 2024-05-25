using System.ComponentModel.DataAnnotations;

namespace UserService.Core.Application.DTOs
{
    public class CreateManagerRequestDTO
    {
        [Required]
        [MinLength(5)]
        public string FullName { get; init; } = null!;
        [Required]
        [EmailAddress]
        public string Email { get; init; } = null!;
        [Required]
        public string Password { get; init; } = null!;
        public Guid? FacultyId { get; init; } = null;
    }
}
