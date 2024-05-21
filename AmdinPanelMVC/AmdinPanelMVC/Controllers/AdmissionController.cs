using Microsoft.AspNetCore.Mvc;
using AmdinPanelMVC.Filters;
using AmdinPanelMVC.Models;
using AmdinPanelMVC.Services.Interfaces;
using AmdinPanelMVC.DTOs;
using Common.Models.Models;
using Common.Models.DTOs.Admission;
using Common.Models.DTOs.Applicant;
using Common.Models.Enums;
using Common.API.Attributes;
using AmdinPanelMVC.Controllers.Base;

namespace AmdinPanelMVC.Controllers
{
    [RequiredAuthorize]
    public class AdmissionController : BaseController
    {
        private readonly IAdmissionService _admissionService;
        private readonly IApplicantService _applicantService;
        private readonly IAuthService _authService;

        public AdmissionController(IAdmissionService admissionService, IApplicantService applicantService, IAuthService authService)
        {
            _admissionService = admissionService;
            _applicantService = applicantService;
            _authService = authService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetAdmission([FromBody] AdmissionsFilterViewModel filter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            return ViewComponent("Admissions", filter);
        }

        public async Task<IActionResult> ApplicantAdmission(Guid applicantId, Guid admissionId)
        {
            ExecutionResult<ApplicantAdmissionDTO> admission = await _admissionService.GetApplicantAdmissionAsync(applicantId, admissionId);
            if (!admission.IsSuccess)
            {
                return Redirect("/NotFound");
            }

            ExecutionResult<ApplicantInfo> applicant = await _applicantService.GetApplicantInfoAsync(applicantId);
            if (!applicant.IsSuccess)
            {
                return Redirect("/NotFound");
            }

            return View("ApplicantAdmission", new ApplicantAdmissionViewModel()
            {
                ApplicantAdmission = admission.Result!,
                ApplicantInfo = applicant.Result!
            });
        }

        [HttpPost]
        public async Task<IActionResult> AddManager([FromBody] AddManagerDTO manager)
        {
            return await RequestHandlerAsync((_) 
                => _admissionService.AddManagerToAdmissionAsync(manager.AdmissionId, manager.ManagerId));
        }

        [HttpPost]
        public async Task<IActionResult> RejectApplicant([FromBody] TakeAndRejectApplicant request)
        {
            return await RequestHandlerAsync((managerId) 
                => _admissionService.RejectApplicantAdmissionAsync(request.AdmissionId, managerId));
        }

        [HttpPost]
        public async Task<IActionResult> TakeApplicant([FromBody] TakeAndRejectApplicant request)
        {
            return await RequestHandlerAsync((managerId) 
                => _admissionService.TakeApplicantAdmissionAsync(request.AdmissionId, managerId));
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStatus([FromBody] ChangeAdmissionStatusViewModel change)
        {
            return await RequestHandlerAsync(
                (managerId) => _admissionService.ChangeAdmissionStatusAsync(change.AdmissionId, managerId, change.NewStatus),
                () => PartialView("_AdmissionStatus", (AdmissionStatus)change.NewStatus));
        }

        [HttpPost]
        [ValidateModelState]
        public async Task<IActionResult> ChangeBasicInfo([FromBody] ChangeBasicInfoDTO request)
        {
            return await RequestHandlerAsync((managerId) 
                => _authService.ChangeFullNameAsync(request.ApplicantId, request.FullName, managerId));
        }

        [HttpPost]
        [ValidateModelState]
        public async Task<IActionResult> ChangeAdditionInfo([FromBody] ChangeAdditionInfoDTO changeInfo)
        {
            return await RequestHandlerAsync((managerId)
                => _applicantService.ChangeAdditionInfoAsync(changeInfo, managerId));
        }

        [HttpPost]
        [ValidateModelState]
        public async Task<IActionResult> ChangePriorities([FromBody] ChangePrioritiesDTO change)
        {
            return await RequestHandlerAsync((managerId)
                => _applicantService.ChangePrioritiesAsync(change.ApplicantId, change.NewPriorities, managerId));
        }


        [HttpPost]
        [ValidateModelState]
        public async Task<IActionResult> DeleteProgram([FromBody] DeleteProgramDTO deleteProgram)
        {
            return await RequestHandlerAsync((managerId)
                => _applicantService.DeleteProgramAsync(deleteProgram.ApplicantId, deleteProgram.ProgramId, managerId));
        }
    }
}
