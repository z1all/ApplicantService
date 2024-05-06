using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Core.Application.DTOs;
using UserService.Core.Application.Interfaces;
using Common.API.Controllers;
using Common.API.Helpers;
using Common.Models.Models;

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
        [Authorize]
        public async Task<ActionResult> ChangeEmailAsync(ChangeEmailRequest changeEmail)
        {
            return await ChangeHandlerAsync(async (Guid userId) =>
                 await _profileService.ChangeEmailAsync(changeEmail, userId));
        }

        [HttpPatch("password")]
        [Authorize]
        public async Task<ActionResult> ChangePasswordAsync(ChangePasswordRequest changePassword)
        {
            return await ChangeHandlerAsync(async (Guid userId) =>
                 await _profileService.ChangePasswordAsync(changePassword, userId));
        }

        [HttpPatch("profile")]
        [Authorize]
        public async Task<ActionResult> ChangeProfileAsync(ChangeProfileRequest changeProfile)
        {
            return await ChangeHandlerAsync(async (Guid userId) =>
                 await _profileService.ChangeProfileAsync(changeProfile, userId));
        }

        private async Task<ActionResult> ChangeHandlerAsync(ChangeOperationAsync changeOperation)
        {
            if (!HttpContext.TryGetUserId(out Guid userId))
            {
                return BadRequest(new ExecutionResult("UnknowError", "Unknow error"));
            }

            ExecutionResult changingResult = await changeOperation(userId);
            if (!changingResult.IsSuccess)
            {
                return BadRequest(changingResult);
            }

            return NoContent();
        }

        private delegate Task<ExecutionResult> ChangeOperationAsync(Guid userId);

        [HttpPost("manager")]
        public async Task<ActionResult> CreateManagerAsync(CreateManagerRequest manager)
        {
            var res = await _profileService.CreateManagerAsync(manager);

            return Ok(res);
        }

        [HttpDelete("manager")]
        public async Task<ActionResult> DeleteManagerAsync([FromQuery] Guid managerId)
        {
            var res = await _profileService.DeleteManagerAsync(managerId);

            return Ok(res);
        }
    }
}
