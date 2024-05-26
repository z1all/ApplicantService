using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using UserService.Core.Application.DTOs;
using UserService.Core.Application.Interfaces;
using UserService.Core.Domain.Entities;
using UserService.Infrastructure.Identity.Extensions;
using Common.Models.Enums;
using Common.Models.Models;
using Common.Models.DTOs;
using Common.Models.DTOs.User;

namespace UserService.Infrastructure.Identity.Services
{
    internal class ProfileService : IProfileService
    {
        private readonly ILogger<ProfileService> _logger;
        private readonly UserManager<CustomUser> _userManager;
        private readonly IServiceBusProvider _serviceBusProvider;

        public ProfileService(ILogger<ProfileService> logger, UserManager<CustomUser> userManager, IServiceBusProvider serviceBusProvider)
        {
            _logger = logger;
            _userManager = userManager;
            _serviceBusProvider = serviceBusProvider;
        }

        public async Task<ExecutionResult> ChangeEmailAsync(ChangeEmailRequestDTO changeEmail, Guid userId, Guid? managerId)
        {
            var changeResult = await ChangeHandlerAsync(userId, managerId, (user) =>
            {
                if (user.Email == changeEmail.NewEmail) return false;
                user.Email = changeEmail.NewEmail;
                user.UserName = changeEmail.NewEmail;
                return true;
            });

            if (changeResult.IsSuccess)
            {
                _logger.LogInformation($"User with id {userId} change email");
            }

            return changeResult;
        }

        public async Task<ExecutionResult> ChangeProfileAsync(ChangeProfileRequestDTO changeProfile, Guid userId, Guid? managerId)
        {
            var changeResult = await ChangeHandlerAsync(userId, managerId, (user) =>
            {
                if (user.FullName == changeProfile.NewFullName) return false;
                user.FullName = changeProfile.NewFullName;
                return true;
            });

            if (changeResult.IsSuccess)
            {
                _logger.LogInformation($"User with id {userId} change profile");
            }

            return changeResult;
        }

        public async Task<ExecutionResult> ChangePasswordAsync(ChangePasswordDTO changePassword, Guid userId)
        {
            CustomUser? user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return new(StatusCodeExecutionResult.NotFound, keyError: "ChangeFail", error: "User not found!");
            }

            IdentityResult changingResult = await _userManager.ChangePasswordAsync(user, changePassword.CurrentPassword, changePassword.NewPassword);
            if (!changingResult.Succeeded) return changingResult.ToExecutionResultError();

            if(changingResult.Succeeded)
            {
                _logger.LogInformation($"User with id {userId} change password");
            }

            return new(isSuccess: true);
        }

        public async Task<ExecutionResult> CreateAdminAsync(CreateAdminRequestDTO createAdmin)
        {
            CustomUser user = new()
            {
                FullName = createAdmin.FullName,
                Email = createAdmin.Email,
                UserName = createAdmin.Email
            };

            IdentityResult creatingResult = await _userManager.CreateAsync(user, createAdmin.Password);
            if (!creatingResult.Succeeded) creatingResult.ToExecutionResultError();

            List<string> roles = [Role.Admin, Role.MainManager, Role.Applicant];
            IdentityResult addingRoleResult = await _userManager.AddToRolesAsync(user, roles);
            if (!addingRoleResult.Succeeded) return addingRoleResult.ToExecutionResultError();

            ExecutionResult creatingRequestResult = await _serviceBusProvider.Request.CreateManagerAsync(MapCustomUserToManager(user, null));
            if (!creatingRequestResult.IsSuccess)
            {
                return creatingRequestResult;
            }

            ExecutionResult sendNotification = await _serviceBusProvider.Notification.CreatedApplicantAsync(MapCustomUserToUser(user));
            if (!sendNotification.IsSuccess) return sendNotification;

            sendNotification = await _serviceBusProvider.Notification.CreatedManagerAsync(MapCustomUserToUser(user), createAdmin.Password);
            if (!sendNotification.IsSuccess) return sendNotification;

            _logger.LogInformation($"A new admin has been created with id {user.Id}");

            return new(isSuccess: true);
        }

        public async Task<ExecutionResult> CreateManagerAsync(CreateManagerRequestDTO createManager)
        {
            CustomUser user = new()
            {
                FullName = createManager.FullName,
                Email = createManager.Email,
                UserName = createManager.Email
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
                return new(StatusCodeExecutionResult.BadRequest, errors: deletingResult.Errors
                        .ToErrorDictionary()
                        .AddRange(creatingRequestResult.Errors));
            }

            _logger.LogInformation($"A new manager has been created with id {user.Id}");

            return creatingRequestResult;
        }

        public async Task<ExecutionResult> ChangeManagerAsync(Manager manager)
        {
            CustomUser? user = await _userManager.FindByIdAsync(manager.Id.ToString());
            if (user is null)
            {
                return new(StatusCodeExecutionResult.NotFound, keyError: "ManagerNotFound", error: $"Manager with id {manager.Id} not found!");
            }

            if (manager.FacultyId is not null && await _userManager.IsInRoleAsync(user, Role.Admin))
            {
                return new(StatusCodeExecutionResult.BadRequest, keyError: "AdministratorWithFaculty", error: $"An administrator cannot have a faculty!");
            }

            user.Email = manager.Email;
            user.UserName = manager.Email;
            user.FullName = manager.FullName;

            IdentityResult updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded) return new(StatusCodeExecutionResult.BadRequest, errors: updateResult.Errors.ToErrorDictionary());

            string newRole = manager.FacultyId == null ? Role.MainManager : Role.Manager;
            if (await _userManager.IsInRoleAsync(user, Role.MainManager) && newRole == Role.Manager)
            {
                ExecutionResult result = await ChangeRoleAsync(user, Role.Manager, Role.MainManager);
                if (!result.IsSuccess) return result;
            }
            else if (await _userManager.IsInRoleAsync(user, Role.Manager) && newRole == Role.MainManager)
            {
                ExecutionResult result = await ChangeRoleAsync(user, Role.MainManager, Role.Manager);
                if (!result.IsSuccess) return result;
            }

            ExecutionResult creatingRequestResult = await _serviceBusProvider.Request.ChangeManagerAsync(manager);
            if (!creatingRequestResult.IsSuccess)
            {
                return creatingRequestResult;
            }

            _logger.LogInformation($"Manager with id {manager.Id} change profile");

            return new(isSuccess: true);
        }

        public async Task<ExecutionResult> DeleteManagerAsync(Guid managerId)
        {
            CustomUser? user = await _userManager.FindByIdAsync(managerId.ToString());
            if (user == null)
            {
                return new(StatusCodeExecutionResult.NotFound, keyError: "DeleteFail", error: "Manager not found!");
            }

            ExecutionResult checkResult = await CheckRolesForDeleteManagerAsync(user);
            if (!checkResult.IsSuccess) return checkResult;

            ExecutionResult deletingRequestResult = await _serviceBusProvider.Request.DeleteManagerAsync(managerId);
            if (!deletingRequestResult.IsSuccess) return deletingRequestResult;

            IdentityResult deletingResult = await _userManager.DeleteAsync(user);
            if (!deletingResult.Succeeded) return deletingResult.ToExecutionResultError();

            _logger.LogInformation($"Manager with id {managerId} was deleted");

            return new(isSuccess: true);
        }

        private async Task<ExecutionResult> ChangeRoleAsync(CustomUser user, string addRole, string removeRole)
        {
            IdentityResult removeRoleResult = await _userManager.RemoveFromRoleAsync(user, removeRole);
            if (!removeRoleResult.Succeeded) return removeRoleResult.ToExecutionResultError();
            IdentityResult addingRoleResult = await _userManager.AddToRoleAsync(user, addRole);
            if (!addingRoleResult.Succeeded) return addingRoleResult.ToExecutionResultError();
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

            if (errors.Count > 0) return new(StatusCodeExecutionResult.BadRequest, keyError: "BadRoles", errors.ToArray());
            return new(isSuccess: true);
        }

        private async Task<ExecutionResult> ChangeHandlerAsync(Guid userId, Guid? managerId, ChangeOperation changeOperation)
        {
            CustomUser? user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return new(StatusCodeExecutionResult.NotFound, keyError: "ChangeFail", error: "User not found!");
            }

            IList<string> roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains(Role.Applicant) && roles.Count == 1)
            {
                ExecutionResult canEdit = await _serviceBusProvider.Request.CheckPermissionsAsync(userId, managerId);
                if (!canEdit.IsSuccess)
                {
                    return new(canEdit.StatusCode, errors: canEdit.Errors);
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

        private UserDTO MapCustomUserToUser(CustomUser user)
        {
            return new UserDTO()
            {
                Id = Guid.Parse(user.Id),
                Email = user.Email!,
                FullName = user.FullName,
            };
        }
    }
}
