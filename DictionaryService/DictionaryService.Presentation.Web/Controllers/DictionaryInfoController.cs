using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DictionaryService.Core.Application.Interfaces.Services;
using Common.API.Controllers;
using Common.Models.DTOs.Dictionary;
using Common.Models.Enums;

namespace DictionaryService.Presentation.Web.Controllers
{
    [Route("api/dictionary")]
    [Authorize(Roles = $"{Role.Applicant}, {Role.Admin}")]
    public class DictionaryInfoController : BaseController
    {
        private readonly IDictionaryInfoService _dictionaryInfoService;

        public DictionaryInfoController(IDictionaryInfoService dictionaryInfoService)
        {
            _dictionaryInfoService = dictionaryInfoService;
        }

        [HttpGet("programs")]
        public async Task<ActionResult<ProgramPagedDTO>> GetPrograms([FromQuery] EducationProgramFilterDTO filterDTO)
        {
            return await ExecutionResultHandlerAsync(async () =>
                await _dictionaryInfoService.GetProgramsAsync(filterDTO));
        }

        [HttpGet("education_levels")]
        public async Task<ActionResult<List<EducationLevelDTO>>> GetEducationLevels()
        {
            return await ExecutionResultHandlerAsync(_dictionaryInfoService.GetEducationLevelsAsync);
        }

        [HttpGet("document_types")]
        public async Task<ActionResult<List<EducationDocumentTypeDTO>>> GetDocumentTypes()
        {
            return await ExecutionResultHandlerAsync(_dictionaryInfoService.GetDocumentTypesAsync);
        }

        [HttpGet("faculties")]
        public async Task<ActionResult<List<FacultyDTO>>> GetFaculties()
        {
            return await ExecutionResultHandlerAsync(_dictionaryInfoService.GetFacultiesAsync);
        }
    }
}
