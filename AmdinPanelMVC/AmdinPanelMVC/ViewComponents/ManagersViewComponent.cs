using Microsoft.AspNetCore.Mvc;
using AmdinPanelMVC.Services.Interfaces;
using Common.Models.DTOs.Admission;
using Common.Models.Models;

namespace AmdinPanelMVC.ViewComponents
{
    public class ManagersViewComponent : ViewComponent
    {
        private readonly IAdminService _adminService;

        public ManagersViewComponent(IAdminService adminService)
        {
            _adminService = adminService;
        }

        public async Task<IViewComponentResult> InvokeAsync(Guid? selectedManagerId)
        {
            ExecutionResult<List<ManagerProfileDTO>> managers = await _adminService.GetManagersAsync();

            ViewBag.SelectedManagerId = selectedManagerId;

            return View("Default", managers.Result ?? new List<ManagerProfileDTO>());
        }
    }
}
