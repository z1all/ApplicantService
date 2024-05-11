using Microsoft.AspNetCore.Mvc;
using AmdinPanelMVC.Filters;
using AmdinPanelMVC.DTOs;
using AmdinPanelMVC.Services.Interfaces;
using AmdinPanelMVC.Models;
using Common.Models.Models;
using System.Text;

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

        #region Dictionary page

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

        #endregion

        #region Managers page

        [HttpGet]
        public IActionResult Managers()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ManagerList()
        {
            return ViewComponent("ManagerList");
        }

        [HttpPost]
        public async Task<IActionResult> CreateManager(CreateManagerViewModel manager)
        {
            if(!ModelState.IsValid)
            {
                return View("Managers", manager);
            }

            ExecutionResult result = await _adminService.AddManagerAsync(new()
            {
                Email = manager.Email,
                FullName = manager.FullName,
                FacultyId = manager.FacultyId
            }, manager.Password!);

            if(!result.IsSuccess)
            {
                ErrorsToModalState(result);

                return View("Managers", manager);
            }

            return Redirect("Managers");
        }

        [HttpPost]
        public IActionResult ChangeManager(CreateManagerViewModel manager)
        {
            return Redirect("Managers");
        }

        #endregion

        private void ErrorsToModalState(ExecutionResult resultErrors)
        {
            foreach (var error in resultErrors.Errors)
            {
                if (error.Key.Contains("Password"))
                {
                    ErrorsToModalState("Password", error.Value);
                }
                else if(error.Key.Contains("Email"))
                {
                    ErrorsToModalState("Email", error.Value);
                }
                else if (error.Key.Contains("Faculty"))
                {
                    ErrorsToModalState("FacultyId", error.Value);
                }
                else
                {
                    ErrorsToModalState("", error.Value);
                }
            }

        }

        private void ErrorsToModalState(string key, List<string> errors)
        {
            StringBuilder stringBuilder = new();

            foreach (var error in errors)
            {
                stringBuilder.AppendLine(error);
            }

            ModelState.AddModelError(key, stringBuilder.ToString());
        }
    }
}
