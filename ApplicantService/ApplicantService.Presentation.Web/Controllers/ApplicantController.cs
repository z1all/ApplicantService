using ApplicantService.Core.Application.DTOs;
using ApplicantService.Core.Application.Interfaces.Services;
using Common.Controllers;
using Common.Helpers;
using Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApplicantService.Presentation.Web.Controllers
{
    [Route("api/applicant")]
    [ApiController]
    public class ApplicantController : BaseController
    {
        public readonly IApplicantProfileService _applicantProfileService;

        public ApplicantController(IApplicantProfileService applicantProfileService)
        {
            _applicantProfileService = applicantProfileService;
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<ActionResult<ApplicantProfile>> GetApplicantProfile()
        {
            if(!HttpContext.TryGetUserId(out Guid userId))
            {
                return BadRequest(new ExecutionResult("UnknowError", "Unknow error"));
            }

            ExecutionResult<ApplicantProfile> response = await _applicantProfileService.GetApplicantProfileAsync(userId);

            if (!response.IsSuccess) return BadRequest(response);
            return Ok(response.Result!);
        }

        [HttpPut("profile")]
        [Authorize]
        public async Task<ActionResult> EditApplicantProfile(EditApplicantProfile applicantProfile)
        {
            if (!HttpContext.TryGetUserId(out Guid userId))
            {
                return BadRequest(new ExecutionResult("UnknowError", "Unknow error"));
            }

            ExecutionResult response = await _applicantProfileService.EditApplicantProfileAsync(applicantProfile, userId);

            if (!response.IsSuccess) return BadRequest(response);
            return NoContent();
        }
    }
}
