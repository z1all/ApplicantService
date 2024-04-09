using DictionaryService.Core.Application.DTOs;
using DictionaryService.Core.Application.Interfaces.Services;
using DictionaryService.Core.Domain.Enum;
using DictionaryService.Core.Application.Interfaces.Repositories;
using Common.Models;

namespace DictionaryService.Core.Application.Services
{
    public class UpdateDictionaryService : IUpdateDictionaryService
    {
        private readonly IUpdateStatusRepository _updateStatusRepository;
        private readonly IExternalDictionaryService _externalDictionaryService;

        public UpdateDictionaryService(IUpdateStatusRepository updateStatusRepository, IExternalDictionaryService externalDictionaryService)
        {
            _updateStatusRepository = updateStatusRepository;
            _externalDictionaryService = externalDictionaryService;
        }

        public Task<ExecutionResult> UpdateAllDictionaryAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ExecutionResult> UpdateDictionaryAsync(DictionaryType dictionaryType)
        {
            return dictionaryType switch
            {
                DictionaryType.Faculty => await UpdateFacultyAsync(),
                DictionaryType.EducationLevel => new(),
                DictionaryType.EducationProgram => new(),
                DictionaryType.EducationDocumentType => new(),
                _ => throw new NotImplementedException(),
            };
        }

        public async Task<ExecutionResult<List<UpdateStatusDTO>>> GetUpdateStatusesAsync()
        {
            return new()
            {
                Result = (await _updateStatusRepository.GetAllAsync()).Select(updateStatus => new UpdateStatusDTO()
                {
                    DictionaryType = updateStatus.DictionaryType,
                    LastUpdate = updateStatus.LastUpdate,
                    Status = updateStatus.Status,
                }).ToList()
            };
        }

        private async Task<ExecutionResult> UpdateFacultyAsync()
        {
            var faculties = await _externalDictionaryService.GetFacultiesAsync();
            var educationDocuments = await _externalDictionaryService.GetEducationDocumentTypesAsync();
            var educationLevels = await _externalDictionaryService.GetEducationLevelsAsync();
            var educationProgram = await _externalDictionaryService.GetEducationProgramAsync();

            return new(isSuccess: true);
        }
    }
}
