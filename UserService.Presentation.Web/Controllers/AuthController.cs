using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Core.Application.DTOs;
using UserService.Core.Application.Interfaces;
using UserService.Core.Application.Models;
using UserService.Infrastructure.Identity.Configurations.Authorization;
using UserService.Presentation.Web.Attributes;
using UserService.Presentation.Web.Helpers;

namespace UserService.Presentation.Web.Controllers
{
    [Route("api/auth")]
    [ApiController]
    [ValidateModelState]
    public class AuthController : ControllerBase
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

        private BadRequestObjectResult BadRequest(ExecutionResult executionResult, string? otherMassage = null)
        {
            return BadRequest(new ErrorResponse()
            {
                Title = otherMassage ?? "One or more errors occurred.",
                Status = 400,
                Errors = executionResult.Errors,
            });
        }

        //[HttpPatch("email")]
        //public async Task<ActionResult> ChangeEmailAsync(ChangeEmailRequest request)
        //{
        //    throw new NotImplementedException();
        //}

        //[HttpPatch("password")]
        //public async Task<ActionResult> ChangePasswordAsync(ChangePasswordRequest request)
        //{
        //    throw new NotImplementedException();
        //}

        //[HttpPatch("profile")]
        //public async Task<ActionResult> ChangeProfileAsync(ChangeProfileRequest request)
        //{
        //    throw new NotImplementedException();
        //}
    }
}