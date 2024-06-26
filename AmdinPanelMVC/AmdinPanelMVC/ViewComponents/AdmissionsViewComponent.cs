﻿using Microsoft.AspNetCore.Mvc;
using AmdinPanelMVC.Services.Interfaces;
using AmdinPanelMVC.Models;
using Common.Models.Models;
using Common.API.Helpers;
using Common.Models.DTOs.Admission;

namespace AmdinPanelMVC.ViewComponents
{
    public class AdmissionsViewComponent : ViewComponent
    {
        private readonly IAdmissionService _admissionService;

        public AdmissionsViewComponent(IAdmissionService admissionService)
        {
            _admissionService = admissionService;
        }

        public async Task<IViewComponentResult> InvokeAsync(AdmissionsFilterViewModel filter)
        {
            if (!HttpContext.TryGetUserId(out Guid managerId))
            {
                return View("Default");
            }

            ExecutionResult<ApplicantAdmissionPagedDTO> applicantAdmission = await _admissionService.GetAdmissionsAsync(new()
            {
                ApplicantFullName = filter.ApplicantFullName,
                CodeOrNameProgram = filter.CodeOrNameProgram,
                FacultiesId = filter.FacultiesId,
                AdmissionStatus = filter.AdmissionStatus,
                SortType = filter.SortType,
                ViewApplicantMode = filter.ViewApplicantMode,
                Page = filter.Page,
                Size = filter.Size
            }, managerId);
            
            return View("Default", applicantAdmission.Result ?? new() { ApplicantAdmissions = [], Pagination = new() { Count = 1, Current = 1, Size = 1} });
        }
    }
}
