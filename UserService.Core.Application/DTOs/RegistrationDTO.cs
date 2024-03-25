using System.ComponentModel.DataAnnotations;

namespace UserService.Core.Application.DTOs
{
    public class RegistrationDTO
    {
        public string FullName { get; init; } = null!;
        [EmailAddress]
        public string Email { get; init; } = null!;
        public string Password { get; init; } = null!;
    }
}
