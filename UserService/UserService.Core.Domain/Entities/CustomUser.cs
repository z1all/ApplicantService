using Microsoft.AspNetCore.Identity;

namespace UserService.Core.Domain.Entities
{
    public class CustomUser : IdentityUser
    {
        public string FullName { get; set; } = null!;
    }
}
