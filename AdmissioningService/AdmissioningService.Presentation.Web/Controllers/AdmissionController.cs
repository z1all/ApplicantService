using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AdmissioningService.Core.Application.Interfaces.Services;
using AdmissioningService.Core.Application.DTOs;
using Common.API.Controllers;

namespace AdmissioningService.Presentation.Web.Controllers
{
    [Route("api/admissioning")]
    [ApiController]
    [Authorize]
    public class AdmissionController : BaseController
    {
        private readonly IAdmissionService _admissionService;

        public AdmissionController(IAdmissionService admissionService) 
        {
            _admissionService = admissionService;
        }

        [HttpGet("admission_companies")]
        public async Task<ActionResult<List<AdmissionCompanyDTO>>> GetAdmissionCompanies()
        {
            return await ExecutionResultHandlerAsync(_admissionService.GetAdmissionCompaniesAsync);
        }

        [HttpPost]
        public async Task<ActionResult> CreateAdmission()
        {
            return await ExecutionResultHandlerAsync(_admissionService.CreateAdmissionAsync);
        }

        [HttpGet("{admissionId}")]
        public async Task<ActionResult<ApplicantAdmissionDTO>> GetAdmission([FromRoute] Guid admissionId)
        {
            return await ExecutionResultHandlerAsync((applicantId) => _admissionService.GetApplicantAdmissionAsync(applicantId, admissionId));
        }

        [HttpPost("{admissionId}/program")]
        public async Task<ActionResult> AddProgramToAdmission([FromRoute] Guid admissionId, AddProgramDTO program)
        {
            return await ExecutionResultHandlerAsync((applicantId) => 
                _admissionService.AddProgramToAdmissionAsync(applicantId, admissionId, program.ProgramId));
        }

        [HttpPut("{admissionId}/program")]
        public async Task<ActionResult> ChangeAdmissionProgramPriority([FromRoute] Guid admissionId, ChangePrioritiesApplicantProgramDTO changePriorities)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{admissionId}/program/{programId}")]
        public async Task<ActionResult> DeleteAdmissionProgram([FromRoute] Guid admissionId, [FromRoute] Guid programId)
        {
            throw new NotImplementedException();
        }
    }
}
