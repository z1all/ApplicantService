using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DictionaryService.Core.Application.Interfaces.Services;
using Common.API.Controllers;
using Common.Models.DTOs.Dictionary;
using Common.Models.Enums;
using Common.API.DTOs;

namespace DictionaryService.Presentation.Web.Controllers
{
    [Route("api/dictionary")]
    [Authorize(Roles = $"{Role.Applicant}, {Role.Admin}")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public class DictionaryInfoController : BaseController
    {
        private readonly IDictionaryInfoService _dictionaryInfoService;

        public DictionaryInfoController(IDictionaryInfoService dictionaryInfoService)
        {
            _dictionaryInfoService = dictionaryInfoService;
        }

        [HttpGet("programs")]
        [ProducesResponseType(typeof(ProgramPagedDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProgramPagedDTO>> GetPrograms([FromQuery] EducationProgramFilterDTO filterDTO)
        {
            return await ExecutionResultHandlerAsync(async () =>
                await _dictionaryInfoService.GetProgramsAsync(filterDTO));
        }

        [HttpGet("education_levels")]
        [ProducesResponseType(typeof(List<EducationLevelDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<EducationLevelDTO>>> GetEducationLevels()
        {
            return await ExecutionResultHandlerAsync(_dictionaryInfoService.GetEducationLevelsAsync);
        }

        [HttpGet("document_types")]
        [ProducesResponseType(typeof(List<EducationDocumentTypeDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<EducationDocumentTypeDTO>>> GetDocumentTypes()
        {
            return await ExecutionResultHandlerAsync(_dictionaryInfoService.GetDocumentTypesAsync);
        }

        [HttpGet("faculties")]
        [ProducesResponseType(typeof(List<FacultyDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<FacultyDTO>>> GetFaculties()
        {
            return await ExecutionResultHandlerAsync(_dictionaryInfoService.GetFacultiesAsync);
        }
    }
}
