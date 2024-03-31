using ApplicantService.Core.Application.DTOs;
using ApplicantService.Core.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApplicantService.Presentation.Web.Controllers
{
    [Route("api/applicant")]
    [ApiController]
    public class ApplicantController : ControllerBase
    {
        public readonly IApplicantProfileService _applicantProfileService;

        public ApplicantController(IApplicantProfileService applicantProfileService)
        {
            _applicantProfileService = applicantProfileService;
        }

        [HttpGet("profile")]
        [Authorize]
        public Task<ActionResult<ApplicantProfile>> GetApplicantProfile()
        {
            throw new NotImplementedException();
        }

        [HttpPut("profile")]
        [Authorize]
        public Task<ActionResult> EditApplicantProfile(EditApplicantProfile applicantProfile)
        {
            throw new NotImplementedException();
        }
    }
}
