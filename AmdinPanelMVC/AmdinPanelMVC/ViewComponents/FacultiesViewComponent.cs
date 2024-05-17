using Microsoft.AspNetCore.Mvc;
using AmdinPanelMVC.Services.Interfaces;
using Common.Models.Models;
using Common.Models.DTOs.Dictionary;

namespace AmdinPanelMVC.ViewComponents
{
    public class FacultiesViewComponent : ViewComponent
    {
        private readonly IDictionaryService _dictionaryService;

        public FacultiesViewComponent(IDictionaryService dictionaryService)
        {
            _dictionaryService = dictionaryService;
        }

        public async Task<IViewComponentResult> InvokeAsync(Guid? selectedFacultyId)
        {
            ExecutionResult<List<FacultyDTO>> faculties = await _dictionaryService.GetFacultiesAsync();

            ViewBag.SelectedFacultyId = selectedFacultyId;

            return View("Default", faculties.Result ?? new List<FacultyDTO>());
        }
    }
}
