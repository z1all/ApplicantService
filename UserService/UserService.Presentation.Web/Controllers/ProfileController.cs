using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Core.Application.DTOs;
using UserService.Core.Application.Interfaces;
using Common.API.Controllers;
using Common.API.Helpers;
using Common.Models.Models;
using Common.Models.DTOs.User;
using Common.Models.Enums;

namespace UserService.Presentation.Web.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class ProfileController : BaseController
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpPatch("email")]
        [Authorize(Roles = $"{Role.Applicant}, {Role.Admin}")]
        public async Task<ActionResult> ChangeEmailAsync(ChangeEmailRequestDTO changeEmail)
        {
            return await ChangeHandlerAsync(async (Guid userId) =>
                 await _profileService.ChangeEmailAsync(changeEmail, userId));
        }

        [HttpPatch("password")]
        [Authorize(Roles = $"{Role.Applicant}, {Role.Admin}")]
        public async Task<ActionResult> ChangePasswordAsync(ChangePasswordDTO changePassword)
        {
            return await ChangeHandlerAsync(async (Guid userId) =>
                 await _profileService.ChangePasswordAsync(changePassword, userId));
        }

        [HttpPatch("profile")]
        [Authorize(Roles = $"{Role.Applicant}, {Role.Admin}")]
        public async Task<ActionResult> ChangeProfileAsync(ChangeProfileRequestDTO changeProfile)
        {
            return await ChangeHandlerAsync(async (Guid userId) =>
                 await _profileService.ChangeProfileAsync(changeProfile, userId));
        }

        private async Task<ActionResult> ChangeHandlerAsync(Func<Guid, Task<ExecutionResult>> changeOperationAsync)
        {
            if (!HttpContext.TryGetUserId(out Guid userId))
            {
                return BadRequest(new ExecutionResult(StatusCodeExecutionResult.InternalServer, "UnknowError", "Unknow error"));
            }

            ExecutionResult changingResult = await changeOperationAsync(userId);
            if (!changingResult.IsSuccess)
            {
                return BadRequest(changingResult);
            }

            return NoContent();
        }
    }
}
