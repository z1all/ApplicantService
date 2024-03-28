using Microsoft.AspNetCore.Identity;
using UserService.Core.Application.DTOs;
using UserService.Core.Application.Interfaces;
using UserService.Core.Application.Models;
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

        public Task<ExecutionResult> ChangeEmail(ChangeEmailRequest changeEmail, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<ExecutionResult> ChangePassword(ChangePasswordRequest changePassword, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<ExecutionResult> ChangeProfile(ChangeProfileRequest changeProfile, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<ExecutionResult> CreateManager(CreateManagerRequest createManager)
        {
            throw new NotImplementedException();
        }

        public Task<ExecutionResult> DeleteManager(Guid managerId)
        {
            throw new NotImplementedException();
        }
    }
}
