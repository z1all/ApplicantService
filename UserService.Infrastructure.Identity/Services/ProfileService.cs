using Microsoft.AspNetCore.Identity;
using UserService.Core.Application.Interfaces;

namespace UserService.Infrastructure.Persistence.Services
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<IdentityUser> _userManager;

        public ProfileService(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
    }
}
