using Microsoft.AspNetCore.Mvc;
using UserService.Core.Application.DTOs;
using UserService.Core.Application.Interfaces;

namespace UserService.Presentation.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
            TokenResponse response = await _authService.ApplicantRegistrationAsync(request);

            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenResponse>> LoginAsync(LoginDTO request)
        {
            TokenResponse response = await _authService.ApplicantLoginAsync(request);

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
