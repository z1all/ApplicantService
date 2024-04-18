using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Core.Application.DTOs;
using UserService.Core.Application.Interfaces;
using UserService.Infrastructure.Identity.Configurations.Authorization;
using UserService.Presentation.Web.Helpers;
using Common.API.Controllers;
using Common.API.Helpers;
using Common.Models.Models;

namespace UserService.Presentation.Web.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("registration")]
        public async Task<ActionResult<TokenResponse>> RegistrationAsync(RegistrationDTO request)
        {
            ExecutionResult<TokenResponse> response = await _authService.ApplicantRegistrationAsync(request);

            if (!response.IsSuccess) return BadRequest(response);
            return Ok(response.Result!);
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenResponse>> LoginAsync(LoginDTO request)
        {
            ExecutionResult<TokenResponse> response = await _authService.ApplicantLoginAsync(request);

            if (!response.IsSuccess) return BadRequest(response);
            return Ok(response.Result!);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<ActionResult> LogoutAsync()
        {
            if (!HttpContext.TryGetAccessTokenJTI(out Guid accessTokenJTI))
            {
                return BadRequest(new ExecutionResult("UnknowError", "Unknow error"));
            }

            ExecutionResult response = await _authService.LogoutAsync(accessTokenJTI);

            if (!response.IsSuccess) return BadRequest(response);
            return NoContent();
        }

        [HttpPost("access")]
        [Authorize(AuthenticationSchemes = CustomJwtBearerDefaults.CheckOnlySignature)]
        public async Task<ActionResult<TokenResponse>> UpdateAccessTokenAsync(UpdateAccessRequest request)
        {
            if (!HttpContext.TryGetAccessTokenJTI(out Guid accessTokenJTI) || !HttpContext.TryGetUserId(out Guid userId))
            {
                return BadRequest(new ExecutionResult("UnknowError", "Unknow error"));
            }

            ExecutionResult<TokenResponse> response = await _authService.UpdateAccessTokenAsync(request.Refresh, accessTokenJTI, userId);

            if (!response.IsSuccess) return BadRequest(response);
            return Ok(response.Result!);
        }
    }
}