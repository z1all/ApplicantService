using DictionaryService.Core.Application.DTOs;
using DictionaryService.Core.Application.Interfaces.Services;
using DictionaryService.Core.Application.Interfaces.Repositories;
using DictionaryService.Core.Application.Interfaces.Transaction;
using DictionaryService.Core.Domain.Enum;
using DictionaryService.Core.Domain;
using DictionaryService.Core.Application.UpdateDictionaryTools.UpdateActionsCreators;
using DictionaryService.Core.Application.UpdateDictionaryTools.UpdateDictionaryHandler;
using Common.Models;

namespace DictionaryService.Core.Application.Services
{
    public class UpdateDictionaryService : IUpdateDictionaryService
    {
        private readonly IUpdateStatusRepository _updateStatusRepository;
        private readonly IFacultyRepository _facultyRepository;
        private readonly IEducationLevelRepository _educationLevelRepository;
        private readonly IEducationProgramRepository _educationProgramRepository;
        private readonly IEducationDocumentTypeRepository _educationDocumentTypeRepository;
        private readonly IExternalDictionaryService _externalDictionaryService;
        private readonly ITransactionProvider _transactionProvider;

        public UpdateDictionaryService(
            IUpdateStatusRepository updateStatusRepository, IFacultyRepository facultyRepository,
            IEducationLevelRepository educationLevelRepository, IEducationDocumentTypeRepository educationDocumentTypeRepository,
            IEducationProgramRepository educationProgramRepository, IExternalDictionaryService externalDictionaryService,
            ITransactionProvider transactionProvider)
        {
            _updateStatusRepository = updateStatusRepository;
            _facultyRepository = facultyRepository;
            _educationLevelRepository = educationLevelRepository;
            _educationDocumentTypeRepository = educationDocumentTypeRepository;
            _educationProgramRepository = educationProgramRepository;
            _externalDictionaryService = externalDictionaryService;
            _transactionProvider = transactionProvider;
        }

        public async Task<ExecutionResult> UpdateAllDictionaryAsync()
        {
            ExecutionResult existOtherUpdating = await CheckOtherUpdatingAsync();
            if (!existOtherUpdating.IsSuccess) { return existOtherUpdating; }

            throw new NotImplementedException();
        }

        public async Task<ExecutionResult> UpdateDictionaryAsync(DictionaryType dictionaryType)
        {
            ExecutionResult existOtherUpdating = await CheckOtherUpdatingAsync();
            if (!existOtherUpdating.IsSuccess) { return existOtherUpdating; }

            // Создаем транзакцию для отмены изменений, если что-то пошло не так
            using ITransaction transaction = await _transactionProvider.CreateTransactionScopeAsync();

            try
            {
                ExecutionResult executionResult = dictionaryType switch
                {
                    DictionaryType.Faculty => await UpdateFacultyAsync(),
                    DictionaryType.EducationProgram => await UpdateEducationProgramAsync(),
                    DictionaryType.EducationLevel => await UpdateEducationLevelAsync(),
                    DictionaryType.EducationDocumentType => await UpdateEducationDocumentTypeAsync(),
                    _ => new(keyError: "WrongDictionaryType", error: "Wrong dictionary type"),
                };

                if (!existOtherUpdating.IsSuccess)
                {
                    await transaction.RollbackAsync();
                }
                else
                {
                    await transaction.CommitAsync();
                }

                return executionResult;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(); 
                throw;
            }
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
                    Comments = updateStatus.Comments,
                }).ToList()
            };
        }

        private async Task<ExecutionResult> CheckOtherUpdatingAsync()
        {
            bool existOtherUpdating = await _updateStatusRepository.CheckOtherUpdatingAsync();
            if (existOtherUpdating)
            {
                return new(keyError: "ExistOtherUpdating", error: "Another directory update is already underway, try it later.");
            }

            return new(isSuccess: true);
        }

        private async Task<ExecutionResult> UpdateFacultyAsync(bool deleteRelatedEntities = false)
        {
            UpdateDictionaryActions<Faculty, FacultyExternalDTO> updateFacultyActions 
                = new UpdateFacultyActionsCreator(_facultyRepository, _educationProgramRepository, _externalDictionaryService).CreateActions();

            return await UpdateDictionaryHandler<Faculty, FacultyExternalDTO, IFacultyRepository>
                .UpdateAsync(deleteRelatedEntities, _facultyRepository, updateFacultyActions);
        }

        private async Task<ExecutionResult> UpdateEducationLevelAsync(bool deleteRelatedEntities = false)
        {
            UpdateDictionaryActions<EducationLevel, EducationLevelExternalDTO> updateEducationLevelActions 
                = new UpdateEducationLevelActionsCreator(_educationLevelRepository, _educationProgramRepository, _educationDocumentTypeRepository, _externalDictionaryService).CreateActions();

            return await UpdateDictionaryHandler<EducationLevel, EducationLevelExternalDTO, IEducationLevelRepository>
                .UpdateAsync(deleteRelatedEntities, _educationLevelRepository, updateEducationLevelActions);
        }

        private async Task<ExecutionResult> UpdateEducationProgramAsync(bool deleteRelatedEntities = false)
        {
            UpdateDictionaryActions<EducationProgram, EducationProgramExternalDTO> updateEducationProgramActions
                = new UpdateEducationProgramActionsCreator(_facultyRepository, _educationLevelRepository, _educationProgramRepository, _externalDictionaryService).CreateActions();

            return await UpdateDictionaryHandler<EducationProgram, EducationProgramExternalDTO, IEducationProgramRepository>
               .UpdateAsync(deleteRelatedEntities, _educationProgramRepository, updateEducationProgramActions);
        }

        private async Task<ExecutionResult> UpdateEducationDocumentTypeAsync(bool deleteRelatedEntities = false)
        {
            UpdateDictionaryActions<EducationDocumentType, EducationDocumentTypeExternalDTO> updateEducationDocumentTypeActions
                = new UpdateEducationDocumentTypeActionsCreator(_educationLevelRepository, _educationDocumentTypeRepository, _externalDictionaryService).CreateActions();

            return await UpdateDictionaryHandler<EducationDocumentType, EducationDocumentTypeExternalDTO, IEducationDocumentTypeRepository>
               .UpdateAsync(deleteRelatedEntities, _educationDocumentTypeRepository, updateEducationDocumentTypeActions);
        }
    }
}