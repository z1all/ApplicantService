using Microsoft.AspNetCore.Mvc;
using AmdinPanelMVC.Services.Interfaces;
using Common.Models.DTOs.Dictionary;
using Common.Models.Models;

namespace AmdinPanelMVC.ViewComponents
{
    public class DocumentTypesViewComponent : ViewComponent
    {
        private readonly IDictionaryService _dictionaryService;

        public DocumentTypesViewComponent(IDictionaryService dictionaryService)
        {
            _dictionaryService = dictionaryService;
        }

        public async Task<IViewComponentResult> InvokeAsync(Guid? selectedDocumentTypeId)
        {
            ExecutionResult<List<EducationDocumentTypeDTO>> documentTypes = await _dictionaryService.GetEducationDocumentTypesAsync();

            ViewBag.SelectedDocumentTypeId = selectedDocumentTypeId;

            return View("Default", documentTypes.Result ?? new List<EducationDocumentTypeDTO>());
        }
    }
}
