using Microsoft.AspNetCore.Mvc;
using AmdinPanelMVC.Services.Interfaces;
using Common.Models.Models;
using Common.Models.DTOs.Admission;

namespace AmdinPanelMVC.ViewComponents
{
    public class AdmissionCompanyViewComponent : ViewComponent
    {
        private readonly IAdmissionService _admissionService;

        public AdmissionCompanyViewComponent(IAdmissionService admissionService)
        {
            _admissionService = admissionService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ExecutionResult<List<AdmissionCompanyDTO>> result = await _admissionService.GetAdmissionsCompaniesAsync();

            return View("Default", result?.Result ?? new());
        }
    }
}
