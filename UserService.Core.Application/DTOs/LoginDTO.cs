using System.ComponentModel.DataAnnotations;

namespace UserService.Core.Application.DTOs
{
    public class LoginDTO
    {
        [EmailAddress]
        public string Email { get; init; } = null!;
        public string Password { get; init; } = null!;
    }
}
