using Microsoft.AspNetCore.Mvc;
using AmdinPanelMVC.Filters;
using AmdinPanelMVC.DTOs;
using AmdinPanelMVC.Services.Interfaces;

namespace AmdinPanelMVC.Controllers
{
    [RequiredAuthorize]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet]
        public IActionResult Settings()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Dictionary()
        {
            return View();
        }

        [HttpPost]
        public async Task UpdateDictionary([FromBody] UpdateDictionaryDTO type)
        {
            if(type is null)
            {
                await _adminService.UpdateAllDictionaryAsync();
            }
            else
            {
                await _adminService.UpdateDictionaryAsync(type.DictionaryType);
            }
        }

        [HttpGet]
        public IActionResult DictionaryUpdateStatus()
        {
            return ViewComponent("DictionaryUpdateStatus");
        }

        [HttpGet]
        public IActionResult Managers()
        {
            return View();
        }
    }
}
