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

        [HttpGet("reception_companies")]
        public async Task<ActionResult<List<AdmissionCompanyDTO>>> GetReceptionCompanies()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<ActionResult> CreateReceptionCompany()
        {
            throw new NotImplementedException();
        }

        [HttpGet("{admissionId}")]
        public async Task<ActionResult<ApplicantAdmissionDTO>> GetAdmission([FromRoute] Guid admissionId)
        {
            throw new NotImplementedException();
        }

        [HttpPost("{admissionId}/program")]
        public async Task<ActionResult> AddProgramToAdmission([FromRoute] Guid admissionId, AddProgramDTO program)
        {
            throw new NotImplementedException();
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
