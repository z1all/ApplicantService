using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Core.Application.DTOs;
using UserService.Core.Application.Interfaces;
using UserService.Infrastructure.Identity.Configurations.Authorization;
using Common.API.Controllers;
using Common.API.Helpers;
using Common.Models.Models;
using Common.Models.Enums;
using Common.API.DTOs;

namespace UserService.Presentation.Web.Controllers
{
    [Route("api/auth")]
    [ApiController]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("registration")]
        [ProducesResponseType(typeof(TokensResponseDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult<TokensResponseDTO>> RegistrationAsync(RegistrationDTO request)
        {
            ExecutionResult<TokensResponseDTO> response = await _authService.ApplicantRegistrationAsync(request);

            if (!response.IsSuccess) return ExecutionResultHandlerAsync(response);
            return Ok(response.Result!);
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(TokensResponseDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult<TokensResponseDTO>> LoginAsync(LoginDTO request)
        {
            ExecutionResult<TokensResponseDTO> response = await _authService.ApplicantLoginAsync(request);

            if (!response.IsSuccess) return ExecutionResultHandlerAsync(response);
            return Ok(response.Result!);
        }

        [HttpPost("logout")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{Role.Applicant}, {Role.Admin}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> LogoutAsync()
        {
            if (!HttpContext.TryGetAccessTokenJTI(out Guid accessTokenJTI))
            {
                return ExecutionResultHandlerAsync(new ExecutionResult(StatusCodeExecutionResult.InternalServer, "UnknowError", "Unknow error"));
            }

            ExecutionResult response = await _authService.LogoutAsync(accessTokenJTI);

            if (!response.IsSuccess) return ExecutionResultHandlerAsync(response);
            return NoContent();
        }

        [HttpPost("access")]
        [Authorize(AuthenticationSchemes = CustomJwtBearerDefaults.CheckOnlySignature, Roles = $"{Role.Applicant}, {Role.Admin}")]
        [ProducesResponseType(typeof(TokensResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<TokensResponseDTO>> UpdateAccessTokenAsync(UpdateAccessRequestDTO request)
        {
            if (!HttpContext.TryGetAccessTokenJTI(out Guid accessTokenJTI) || !HttpContext.TryGetUserId(out Guid userId))
            {
                return ExecutionResultHandlerAsync(new ExecutionResult(StatusCodeExecutionResult.InternalServer, "UnknowError", "Unknow error"));
            }

            ExecutionResult<TokensResponseDTO> response = await _authService.UpdateAccessTokenAsync(request.Refresh, accessTokenJTI, userId);

            if (!response.IsSuccess) return ExecutionResultHandlerAsync(response);
            return Ok(response.Result!);
        }
    }
}