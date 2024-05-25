using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApplicantService.Core.Application.DTOs;
using ApplicantService.Core.Application.Interfaces.Services;
using Common.API.Controllers;
using Common.Models.DTOs.Applicant;
using Common.Models.Enums;
using Common.API.DTOs;

namespace ApplicantService.Presentation.Web.Controllers
{
    [Route("api/applicant")]
    [ApiController]
    [Authorize(Roles = $"{Role.Applicant}, {Role.Admin}")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public class ApplicantController : BaseController
    {
        public readonly IApplicantProfileService _applicantProfileService;

        public ApplicantController(IApplicantProfileService applicantProfileService)
        {
            _applicantProfileService = applicantProfileService;
        }

        [HttpGet("profile")]
        [ProducesResponseType(typeof(ApplicantProfile), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApplicantProfile>> GetApplicantProfile()
        {
            return await ExecutionResultHandlerAsync(_applicantProfileService.GetApplicantProfileAsync);
        }

        [HttpPut("profile")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> EditApplicantProfile(EditApplicantProfile applicantProfile)
        {
            return await ExecutionResultHandlerAsync(async userId =>
                await _applicantProfileService.EditApplicantProfileAsync(applicantProfile, userId));
        }
    }
}
