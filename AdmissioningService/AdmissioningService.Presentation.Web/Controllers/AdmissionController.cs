using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AdmissioningService.Core.Application.Interfaces.Services;
using Common.API.Controllers;
using Common.Models.DTOs.Admission;
using Common.Models.Enums;
using Common.API.DTOs;

namespace AdmissioningService.Presentation.Web.Controllers
{
    [Route("api/admissioning")]
    [ApiController]
    [Authorize(Roles = $"{Role.Applicant}, {Role.Admin}")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public class AdmissionController : BaseController
    {
        private readonly IAdmissionService _admissionService;

        public AdmissionController(IAdmissionService admissionService)
        {
            _admissionService = admissionService;
        }

        [HttpGet("admission_companies")]
        [ProducesResponseType(typeof(List<AdmissionCompanyDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<AdmissionCompanyDTO>>> GetAdmissionCompanies()
        {
            return await ExecutionResultHandlerAsync(_admissionService.GetAdmissionCompaniesWithApplicantAdmissionsAsync);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateAdmission()
        {
            return await ExecutionResultHandlerAsync(_admissionService.CreateAdmissionAsync);
        }

        [HttpGet("{admissionId}")]
        [ProducesResponseType(typeof(ApplicantAdmissionDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApplicantAdmissionDTO>> GetAdmission([FromRoute] Guid admissionId)
        {
            return await ExecutionResultHandlerAsync((applicantId) =>
                _admissionService.GetApplicantAdmissionAsync(applicantId, admissionId));
        }

        [HttpPost("program")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> AddProgramToCurrentAdmission(AddProgramDTO program)
        {
            return await ExecutionResultHandlerAsync((applicantId) =>
                _admissionService.AddProgramToCurrentAdmissionAsync(applicantId, program.ProgramId));
        }

        [HttpPut("program")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> ChangeCurrentAdmissionProgramPriority(ChangePrioritiesApplicantProgramDTO changePriorities)
        {
            return await ExecutionResultHandlerAsync((applicantId) =>
                _admissionService.ChangeAdmissionProgramPriorityAsync(applicantId, changePriorities));
        }

        [HttpDelete("program/{programId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteCurrentAdmissionProgram([FromRoute] Guid programId)
        {
            return await ExecutionResultHandlerAsync((applicantId) =>
               _admissionService.DeleteAdmissionProgramAsync(applicantId, programId));
        }
    }
}
