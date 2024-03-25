using Microsoft.AspNetCore.Mvc;
using UserService.Core.Application.DTOs;
using UserService.Core.Application.Interfaces;
using UserService.Core.Application.Models;
using UserService.Presentation.Web.Attributes;

namespace UserService.Presentation.Web.Controllers
{
    [Route("api/[controller]")]
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

            if(!response.IsSuccess)
            {
                return BadRequest(new ErrorResponse()
                {
                    Title = "One or more errors occurred.",
                    Status = 400,
                    Errors = response.Errors,
                });
            }

            return Ok(response.Result!);
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenResponse>> LoginAsync(LoginDTO request)
        {
            ExecutionResult<TokenResponse> response = await _authService.ApplicantLoginAsync(request);

            return Ok(response);
        }

        [HttpPost("logout")]
        public async Task<ActionResult> LogoutAsync()
        {
            throw new NotImplementedException();
        }

        [HttpPost("access")]
        public async Task<ActionResult<TokenResponse>> UpdateAccessTokenAsync(UpdateAccessRequest request)
        {
            throw new NotImplementedException();
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
