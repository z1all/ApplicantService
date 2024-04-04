using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApplicantService.Core.Application.DTOs;
using ApplicantService.Core.Application.Interfaces.Services;
using Common.Controllers;

namespace ApplicantService.Presentation.Web.Controllers
{
    [Route("api/applicant")]
    [ApiController]
    [Authorize]
    public class ApplicantController : BaseController
    {
        public readonly IApplicantProfileService _applicantProfileService;

        public ApplicantController(IApplicantProfileService applicantProfileService)
        {
            _applicantProfileService = applicantProfileService;
        }

        [HttpGet("profile")]
        public async Task<ActionResult<ApplicantProfile>> GetApplicantProfile()
        {
            return await ExecutionResultHandlerAsync(_applicantProfileService.GetApplicantProfileAsync);
        }

        [HttpPut("profile")]
        public async Task<ActionResult> EditApplicantProfile(EditApplicantProfile applicantProfile)
        {
            return await ExecutionResultHandlerAsync(async userId =>
                await _applicantProfileService.EditApplicantProfileAsync(applicantProfile, userId));
        }
    }
}
