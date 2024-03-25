using Microsoft.AspNetCore.Identity;
using UserService.Core.Application.Interfaces;
using UserService.Core.Domain.Entities;

namespace UserService.Infrastructure.Persistence.Services
{
    internal class ProfileService : IProfileService
    {
        private readonly UserManager<CustomUser> _userManager;

        public ProfileService(UserManager<CustomUser> userManager)
        {
            _userManager = userManager;
        }
    }
}
