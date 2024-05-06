using Microsoft.AspNetCore.Identity;
using UserService.Core.Application.DTOs;
using UserService.Core.Application.Interfaces;
using UserService.Core.Domain.Entities;
using UserService.Infrastructure.Identity.Extensions;
using Common.Models.Enums;
using Common.Models.Models;

namespace UserService.Infrastructure.Identity.Services
{
    internal class ProfileService : IProfileService
    {
        private readonly UserManager<CustomUser> _userManager;
        private readonly IServiceBusProvider _serviceBusProvider;

        public ProfileService(UserManager<CustomUser> userManager, IServiceBusProvider serviceBusProvider)
        {
            _userManager = userManager;
            _serviceBusProvider = serviceBusProvider;
        }

        public async Task<ExecutionResult> ChangeEmailAsync(ChangeEmailRequest changeEmail, Guid userId, Guid? managerId)
        {
            return await ChangeHandlerAsync(userId, managerId, (user) =>
            {
                if (user.Email == changeEmail.NewEmail) return false;
                user.Email = changeEmail.NewEmail;
                return true;
            });
        }

        public async Task<ExecutionResult> ChangeProfileAsync(ChangeProfileRequest changeProfile, Guid userId, Guid? managerId)
        {
            return await ChangeHandlerAsync(userId, managerId, (user) =>
            {
                if (user.FullName == changeProfile.NewFullName) return false;
                user.FullName = changeProfile.NewFullName;
                return true;
            });
        }

        public async Task<ExecutionResult> ChangePasswordAsync(ChangePasswordRequest changePassword, Guid userId)
        {
            CustomUser? user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return new(keyError: "ChangeFail", error: "User not found!");
            }

            IdentityResult changingResult = await _userManager.ChangePasswordAsync(user, changePassword.CurrentPassword, changePassword.NewPassword);
            if (!changingResult.Succeeded) return changingResult.ToExecutionResultError();

            return new(isSuccess: true);
        }

        public async Task<ExecutionResult> CreateAdminAsync(CreateAdminRequest createAdmin)
        {
            CustomUser user = new()
            {
                FullName = createAdmin.FullName,
                Email = createAdmin.Email,
                UserName = $"{createAdmin.FullName}_{Guid.NewGuid()}",
            };

            IdentityResult creatingResult = await _userManager.CreateAsync(user, createAdmin.Password);
            if (!creatingResult.Succeeded) creatingResult.ToExecutionResultError();

            List<string> roles = [Role.Admin, Role.MainManager, Role.Applicant];
            IdentityResult addingRoleResult = await _userManager.AddToRolesAsync(user, roles);
            if (!addingRoleResult.Succeeded) return addingRoleResult.ToExecutionResultError();

            ExecutionResult sendNotification = await _serviceBusProvider.Notification.CreatedManagerAsync(MapCustomUserToUser(user), createAdmin.Password);
            if(!sendNotification.IsSuccess) return sendNotification;

            sendNotification = await _serviceBusProvider.Notification.CreatedApplicantAsync(MapCustomUserToUser(user));
            if (!sendNotification.IsSuccess) return sendNotification;

            return new(isSuccess: true);
        }

        public async Task<ExecutionResult> CreateManagerAsync(CreateManagerRequest createManager)
        {
            CustomUser user = new()
            {
                FullName = createManager.FullName,
                Email = createManager.Email,
                UserName = $"{createManager.FullName}_{Guid.NewGuid()}",
            };

            IdentityResult creatingResult = await _userManager.CreateAsync(user, createManager.Password);
            if (!creatingResult.Succeeded) return creatingResult.ToExecutionResultError();

            string role = createManager.FacultyId == null ? Role.MainManager : Role.Manager;
            IdentityResult addingRoleResult = await _userManager.AddToRoleAsync(user, role);
            if (!addingRoleResult.Succeeded) return addingRoleResult.ToExecutionResultError();

            ExecutionResult creatingRequestResult = await _serviceBusProvider.Request.CreateManagerAsync(MapCustomUserToManager(user, createManager.FacultyId));
            if (creatingRequestResult.IsSuccess)
            {
                return await _serviceBusProvider.Notification.CreatedManagerAsync(MapCustomUserToUser(user), createManager.Password);
            }

            IdentityResult deletingResult = await _userManager.DeleteAsync(user);
            if (!deletingResult.Succeeded)
            {
                return new()
                {
                    Errors = deletingResult.Errors
                        .ToErrorDictionary()
                        .AddRange(creatingRequestResult.Errors)
                };
            }

            return creatingRequestResult;
        }

        public async Task<ExecutionResult> DeleteManagerAsync(Guid managerId)
        {
            CustomUser? user = await _userManager.FindByIdAsync(managerId.ToString());
            if (user == null)
            {
                return new(keyError: "DeleteFail", error: "Manager not found!");
            }

            ExecutionResult checkResult = await CheckRolesForDeleteManagerAsync(user);
            if(!checkResult.IsSuccess) return checkResult;

            ExecutionResult deletingRequestResult = await _serviceBusProvider.Request.DeleteManagerAsync(managerId);
            if (!deletingRequestResult.IsSuccess) return deletingRequestResult;

            IdentityResult deletingResult = await _userManager.DeleteAsync(user);
            if (!deletingResult.Succeeded) return deletingResult.ToExecutionResultError();

            return new(isSuccess: true);
        }

        private async Task<ExecutionResult> CheckRolesForDeleteManagerAsync(CustomUser user)
        {
            IList<string> userRoles = await _userManager.GetRolesAsync(user);
            List<string> errors = new();

            bool isManager = true;
            if (!userRoles.Contains(Role.Manager) && !userRoles.Contains(Role.MainManager))
            {
                errors.Add("This user is not manager or main manager!");
                isManager = false;
            }

            if (isManager && userRoles.Contains(Role.Admin))
            {
                errors.Add("You cannot delete a manager with the admin role!");
            }

            if (errors.Count > 0) return new(keyError: "BadRoles", errors.ToArray());
            return new(isSuccess: true);
        }

        private async Task<ExecutionResult> ChangeHandlerAsync(Guid userId, Guid? managerId, ChangeOperation changeOperation)
        {
            CustomUser? user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return new(keyError: "ChangeFail", error: "User not found!");
            }

            IList<string> roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains(Role.Applicant) && roles.Count == 1)
            {
                ExecutionResult canEdit = await _serviceBusProvider.Request.CheckPermissionsAsync(userId, managerId);
                if (!canEdit.IsSuccess)
                {
                    return new() { Errors = canEdit.Errors };
                }
            }

            bool wasUpdated = changeOperation(user);
            if (!wasUpdated) return new(isSuccess: true);

            IdentityResult updatingResult = await _userManager.UpdateAsync(user);
            if (!updatingResult.Succeeded) return updatingResult.ToExecutionResultError();

            return await _serviceBusProvider.Notification.UpdatedUserAsync(MapCustomUserToUser(user));
        }

        private delegate bool ChangeOperation(CustomUser user);

        private Manager MapCustomUserToManager(CustomUser user, Guid? facultyId)
        {
            return new Manager()
            {
                Id = Guid.Parse(user.Id),
                FullName = user.FullName,
                Email = user.Email!,
                FacultyId = facultyId,
            };
        }

        private User MapCustomUserToUser(CustomUser user)
        {
            return new User()
            {
                Id = Guid.Parse(user.Id),
                Email = user.Email!,
                FullName = user.FullName,
            };
        }
    }
}
