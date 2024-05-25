using Microsoft.AspNetCore.Mvc;
using System.Text;
using AmdinPanelMVC.Filters;
using AmdinPanelMVC.DTOs;
using AmdinPanelMVC.Services.Interfaces;
using AmdinPanelMVC.Models;
using Common.Models.Models;
using Common.Models.Enums;
using Common.API.DTOs;

namespace AmdinPanelMVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly IAdmissionService _admissionService;

        public AdminController(IAdminService adminService, IAdmissionService admissionService)
        {
            _adminService = adminService;
            _admissionService = admissionService;
        }

        [HttpGet]
        [RequiredAuthorize(Roles = [Role.MainManager, Role.Admin])]
        public IActionResult Settings()
        {
            return View();
        }

        #region Dictionary page

        [HttpGet]
        [RequiredAuthorize(Roles = [Role.Admin])]
        public IActionResult Dictionary()
        {
            return View();
        }

        [HttpPost]
        [RequiredAuthorize(Roles = [Role.Admin])]
        public async Task UpdateDictionary([FromBody] UpdateDictionaryDTO type)
        {
            if (type is null)
            {
                await _adminService.UpdateAllDictionaryAsync();
            }
            else
            {
                await _adminService.UpdateDictionaryAsync(type.DictionaryType);
            }
        }

        [HttpGet]
        [RequiredAuthorize(Roles = [Role.Admin])]
        public IActionResult DictionaryUpdateStatus()
        {
            return ViewComponent("DictionaryUpdateStatus");
        }

        #endregion

        #region Managers page

        [HttpGet]
        [RequiredAuthorize(Roles = [Role.MainManager, Role.Admin])]
        public IActionResult Managers()
        {
            return View();
        }

        [HttpGet]
        [RequiredAuthorize(Roles = [Role.MainManager, Role.Admin])]
        public IActionResult ManagerList()
        {
            return ViewComponent("ManagerList");
        }

        [HttpPost]
        [RequiredAuthorize(Roles = [Role.Admin])]
        public async Task<IActionResult> CreateManager(CreateAndChangeManagerViewModel manager)
        {
            if (!ModelState.IsValid)
            {
                return View("Managers", manager);
            }

            ExecutionResult result = await _adminService.AddManagerAsync(new()
            {
                Email = manager.Email,
                FullName = manager.FullName,
                FacultyId = manager.FacultyId
            }, manager.Password!);

            if (!result.IsSuccess)
            {
                ErrorsToModalState(result);

                return View("Managers", manager);
            }

            return Redirect("Managers");
        }

        [HttpPost]
        [RequiredAuthorize(Roles = [Role.Admin])]
        public async Task<IActionResult> ChangeManager(CreateAndChangeManagerViewModel manager)
        {
            if (manager.Id is null)
            {
                ModelState.AddModelError("", "Id пользователя не найден");
                return View("Managers", manager);
            }

            if (!ModelState.IsValid)
            {
                return View("Managers", manager);
            }

            ExecutionResult result = await _adminService.ChangeManagerAsync((Guid)manager.Id, new()
            {
                Email = manager.Email,
                FullName = manager.FullName,
                FacultyId = manager.FacultyId
            });

            if (!result.IsSuccess)
            {
                ErrorsToModalState(result);

                return View("Managers", manager);
            }

            return View("Managers", manager);
        }

        [HttpDelete]
        [RequiredAuthorize(Roles = [Role.Admin])]
        public async Task<IActionResult> DeleteManager([FromBody] DeleteManagerDTO manager)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            ExecutionResult result = await _adminService.DeleteManagerAsync(manager.ManagerId);
            if (!result.IsSuccess)
            {
                return BadRequest(new ErrorResponse()
                {
                    Title = "Delete manager fail",
                    Status = 400,
                    Errors = result.Errors,
                });
            }

            return Ok();
        }

        #endregion


        #region AdmissionCompanies

        [HttpGet]
        [RequiredAuthorize(Roles = [Role.MainManager, Role.Admin])]
        public IActionResult AdmissionsCompanies()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequiredAuthorize(Roles = [Role.Admin])]
        public async Task<IActionResult> CreateAdmissionsCompanies(CreateAdmissionCompany createAdmissionCompany)
        {
            if (!ModelState.IsValid)
            {
                return View("AdmissionsCompanies", createAdmissionCompany);
            }

            ExecutionResult result = await _admissionService.CreateAdmissionCompanyAsync(createAdmissionCompany.Year);
            if (!result.IsSuccess)
            {
                ErrorsToModalState(result);

                return View("Managers", createAdmissionCompany);
            }

            return Redirect("AdmissionsCompanies");
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
                else if (error.Key.Contains("Email") || error.Key.Contains("UserName"))
                {
                    ErrorsToModalState("Email", error.Value);
                }
                else if (error.Key.Contains("AdministratorWithFaculty"))
                {
                    ErrorsToModalState("FacultyId", ["Администратор не может иметь факультет"]);
                }
                else if (error.Key.Contains("Faculty"))
                {
                    ErrorsToModalState("FacultyId", ["Такого факультета не существует"]);
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
