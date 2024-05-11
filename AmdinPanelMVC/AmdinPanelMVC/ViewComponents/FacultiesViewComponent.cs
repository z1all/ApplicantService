using Microsoft.AspNetCore.Mvc;
using AmdinPanelMVC.Services.Interfaces;
using Common.Models.DTOs;
using Common.Models.Models;

namespace AmdinPanelMVC.ViewComponents
{
    public class FacultiesViewComponent : ViewComponent
    {
        private readonly IDictionaryService _dictionaryService;

        public FacultiesViewComponent(IDictionaryService dictionaryService)
        {
            _dictionaryService = dictionaryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ExecutionResult<List<FacultyDTO>> faculties = await _dictionaryService.GetFacultiesAsync();

            return View("Default", faculties.Result ?? new List<FacultyDTO>());
        }
    }
}
