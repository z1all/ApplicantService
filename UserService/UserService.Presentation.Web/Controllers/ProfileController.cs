using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Core.Application.DTOs;
using UserService.Core.Application.Interfaces;
using Common.API.Controllers;
using Common.API.Helpers;
using Common.Models.Models;
using Common.Models.DTOs.User;
using Common.Models.Enums;
using Common.API.DTOs;

namespace UserService.Presentation.Web.Controllers
{
    [Route("api/user")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{Role.Applicant}, {Role.Admin}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(TokensResponseDTO), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TokensResponseDTO), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public class ProfileController : BaseController
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpPatch("email")]
        public async Task<ActionResult> ChangeEmailAsync(ChangeEmailRequestDTO changeEmail)
        {
            return await ChangeHandlerAsync(async (Guid userId) =>
                 await _profileService.ChangeEmailAsync(changeEmail, userId));
        }

        [HttpPatch("password")]
        public async Task<ActionResult> ChangePasswordAsync(ChangePasswordDTO changePassword)
        {
            return await ChangeHandlerAsync(async (Guid userId) =>
                 await _profileService.ChangePasswordAsync(changePassword, userId));
        }

        [HttpPatch("profile")]
        public async Task<ActionResult> ChangeProfileAsync(ChangeProfileRequestDTO changeProfile)
        {
            return await ChangeHandlerAsync(async (Guid userId) =>
                 await _profileService.ChangeProfileAsync(changeProfile, userId));
        }

        private async Task<ActionResult> ChangeHandlerAsync(Func<Guid, Task<ExecutionResult>> changeOperationAsync)
        {
            if (!HttpContext.TryGetUserId(out Guid userId))
            {
                return ExecutionResultHandlerAsync(new ExecutionResult(StatusCodeExecutionResult.InternalServer, "UnknowError", "Unknow error"));
            }

            ExecutionResult changingResult = await changeOperationAsync(userId);
            if (!changingResult.IsSuccess)
            {
                return ExecutionResultHandlerAsync(changingResult);
            }

            return NoContent();
        }
    }
}
