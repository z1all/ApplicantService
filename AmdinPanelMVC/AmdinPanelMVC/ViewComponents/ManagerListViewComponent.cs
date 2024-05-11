using Microsoft.AspNetCore.Mvc;
using AmdinPanelMVC.Services.Interfaces;
using Common.Models.DTOs;
using Common.Models.Models;

namespace AmdinPanelMVC.ViewComponents
{
    public class ManagerListViewComponent : ViewComponent
    {
        private readonly IAdminService _adminService;

        public ManagerListViewComponent(IAdminService adminService)
        {
            _adminService = adminService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ExecutionResult<List<ManagerProfileDTO>> managers = await _adminService.GetManagersAsync();

            return View("Default", managers.Result ?? new List<ManagerProfileDTO>());
        }
    }
}
