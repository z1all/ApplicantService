﻿using UserService.Core.Application.DTOs;
using Common.Models.Models;
using Common.Models.DTOs.User;

namespace UserService.Core.Application.Interfaces
{
    public interface IProfileService
    {
        Task<ExecutionResult> ChangePasswordAsync(ChangePasswordDTO changePassword, Guid userId);
        Task<ExecutionResult> ChangeEmailAsync(ChangeEmailRequestDTO changeEmail, Guid userId, Guid? managerId = null);
        Task<ExecutionResult> ChangeProfileAsync(ChangeProfileRequestDTO changeProfile, Guid userId, Guid? managerId = null);
        Task<ExecutionResult> CreateAdminAsync(CreateAdminRequestDTO createAdmin);
        Task<ExecutionResult> CreateManagerAsync(CreateManagerRequestDTO createManager);
        Task<ExecutionResult> ChangeManagerAsync(Manager manager);
        Task<ExecutionResult> DeleteManagerAsync(Guid managerId);
    }
}
