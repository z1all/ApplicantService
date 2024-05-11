﻿using Microsoft.AspNetCore.Mvc;
using AmdinPanelMVC.Services.Interfaces;
using Common.Models.DTOs;
using Common.Models.Models;

namespace AmdinPanelMVC.ViewComponents
{
    public class DictionaryUpdateStatusViewComponent : ViewComponent
    {
        private readonly IAdminService _adminService;

        public DictionaryUpdateStatusViewComponent(IAdminService adminService) 
        { 
            _adminService = adminService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ExecutionResult<List<UpdateStatusDTO>> statuses = await _adminService.GetUpdateStatusesAsync();

            return View("Default", statuses.Result ?? new List<UpdateStatusDTO>());
        }
    }
}
