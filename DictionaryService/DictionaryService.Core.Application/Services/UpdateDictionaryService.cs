using DictionaryService.Core.Application.DTOs;
using DictionaryService.Core.Application.Interfaces.Services;
using DictionaryService.Core.Application.Interfaces.Repositories;
using DictionaryService.Core.Application.Interfaces.Transaction;
using DictionaryService.Core.Domain.Enum;
using DictionaryService.Core.Domain;
using DictionaryService.Core.Application.UpdateDictionaryTools.UpdateDictionaryHandler;
using DictionaryService.Core.Application.UpdateDictionaryTools.UpdateActionsCreators.Base;
using Common.Models;

namespace DictionaryService.Core.Application.Services
{
    public class UpdateDictionaryService : IUpdateDictionaryService
    {
        private readonly IUpdateStatusRepository _updateStatusRepository;
        private readonly ITransactionProvider _transactionProvider;

        private readonly UpdateActionsCreator<Faculty, FacultyExternalDTO> _updateFacultyActionsCreator;
        private readonly UpdateActionsCreator<EducationLevel, EducationLevelExternalDTO> _updateEducationLevelActionsCreator;
        private readonly UpdateActionsCreator<EducationProgram, EducationProgramExternalDTO> _updateEducationProgramActionsCreator;
        private readonly UpdateActionsCreator<EducationDocumentType, EducationDocumentTypeExternalDTO> _updateEducationDocumentTypeActionsCreator;

        public UpdateDictionaryService(
            IUpdateStatusRepository updateStatusRepository, ITransactionProvider transactionProvider,
            UpdateActionsCreator<Faculty, FacultyExternalDTO> updateFacultyActionsCreator,
            UpdateActionsCreator<EducationLevel, EducationLevelExternalDTO> updateEducationLevelActionsCreator,
            UpdateActionsCreator<EducationProgram, EducationProgramExternalDTO> updateEducationProgramActionsCreator,
            UpdateActionsCreator<EducationDocumentType, EducationDocumentTypeExternalDTO> updateEducationDocumentTypeActionsCreator)
        {
            _updateStatusRepository = updateStatusRepository;
            _transactionProvider = transactionProvider;

            _updateFacultyActionsCreator = updateFacultyActionsCreator;
            _updateEducationLevelActionsCreator = updateEducationLevelActionsCreator;
            _updateEducationProgramActionsCreator = updateEducationProgramActionsCreator;
            _updateEducationDocumentTypeActionsCreator = updateEducationDocumentTypeActionsCreator;
        }

        public async Task<ExecutionResult> UpdateAllDictionaryAsync()
        {
            bool existOtherUpdating = await _updateStatusRepository.TryBeganUpdatingForAllDictionaryAsync();
            if (existOtherUpdating)
            {
                return new(keyError: "ExistOtherUpdating", error: "Another dictionary update is already underway, try it later.");
            }

            return await UpdateDictionaryHandlerAsync(async () =>
            {
                ExecutionResult updateFacultyResult = await UpdateFacultyAsync(true);
                if (!updateFacultyResult.IsSuccess) return updateFacultyResult; 

                ExecutionResult updateEducationLevelResult = await UpdateEducationLevelAsync(true);
                if (!updateEducationLevelResult.IsSuccess) return updateEducationLevelResult;

                ExecutionResult updateEducationProgramResult = await UpdateEducationProgramAsync(true);
                if (!updateEducationProgramResult.IsSuccess) return updateEducationProgramResult;

                ExecutionResult updateEducationDocumentTypeResult = await UpdateEducationDocumentTypeAsync(true);
                if (!updateEducationDocumentTypeResult.IsSuccess) return updateEducationDocumentTypeResult;

                return new(isSuccess: true);
            }, async () =>
            {
                List<UpdateStatus> updateStatuses = await _updateStatusRepository.GetAllAsync();

                updateStatuses.ForEach(updateStatus =>
                {
                    updateStatus!.Status = UpdateStatusEnum.ErrorInUpdating;
                    updateStatus!.Comments = "Unknow error";
                });

                await _updateStatusRepository.SaveChangesAsync();
            });
        }

        public async Task<ExecutionResult> UpdateDictionaryAsync(DictionaryType dictionaryType)
        {
            bool existOtherUpdating = await _updateStatusRepository.TryBeganUpdatingForDictionaryAsync(dictionaryType);
            if (existOtherUpdating)
            {
                return new(keyError: "ExistOtherUpdating", error: "Another dictionary update is already underway, try it later.");
            }

            return await UpdateDictionaryHandlerAsync(async () =>
            {
                return dictionaryType switch
                {
                    DictionaryType.Faculty => await UpdateFacultyAsync(),
                    DictionaryType.EducationProgram => await UpdateEducationProgramAsync(),
                    DictionaryType.EducationLevel => await UpdateEducationLevelAsync(),
                    DictionaryType.EducationDocumentType => await UpdateEducationDocumentTypeAsync(),
                    _ => new(keyError: "WrongDictionaryType", error: "Wrong dictionary type"),
                };
            }, async () =>
            {
                UpdateStatus? updateStatus = await _updateStatusRepository.GetByDictionaryTypeAsync(dictionaryType);
                updateStatus!.Status = UpdateStatusEnum.ErrorInUpdating;
                updateStatus!.Comments = "Unknow error";
                await _updateStatusRepository.UpdateAsync(updateStatus);
            });
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

        private async Task<ExecutionResult> UpdateDictionaryHandlerAsync(Func<Task<ExecutionResult>> updateOperationAsync, Func<Task> onErrorAsync)
        {
            // Создаем транзакцию для отмены изменений, если что-то пошло не так
            using ITransaction transaction = await _transactionProvider.CreateTransactionScopeAsync();

            try
            {
                ExecutionResult executionResult = await updateOperationAsync();

                if (!executionResult.IsSuccess)
                {
                    await transaction.RollbackAsync();

                    await onErrorAsync();
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

                await onErrorAsync();

                throw;
            }
        }

        private async Task<ExecutionResult> UpdateFacultyAsync(bool deleteRelatedEntities = false)
        {
            UpdateDictionaryActions<Faculty, FacultyExternalDTO> updateFacultyActions 
                = _updateFacultyActionsCreator.CreateActions();

            return await UpdateDictionaryHandler<Faculty, FacultyExternalDTO>
                .UpdateAsync(deleteRelatedEntities, updateFacultyActions);
        }

        private async Task<ExecutionResult> UpdateEducationLevelAsync(bool deleteRelatedEntities = false)
        {
            UpdateDictionaryActions<EducationLevel, EducationLevelExternalDTO> updateEducationLevelActions
                = _updateEducationLevelActionsCreator.CreateActions();

            return await UpdateDictionaryHandler<EducationLevel, EducationLevelExternalDTO>
                .UpdateAsync(deleteRelatedEntities, updateEducationLevelActions);
        }

        private async Task<ExecutionResult> UpdateEducationProgramAsync(bool deleteRelatedEntities = false)
        {
            UpdateDictionaryActions<EducationProgram, EducationProgramExternalDTO> updateEducationProgramActions
                = _updateEducationProgramActionsCreator.CreateActions();

            return await UpdateDictionaryHandler<EducationProgram, EducationProgramExternalDTO>
               .UpdateAsync(deleteRelatedEntities, updateEducationProgramActions);
        }

        private async Task<ExecutionResult> UpdateEducationDocumentTypeAsync(bool deleteRelatedEntities = false)
        {
            UpdateDictionaryActions<EducationDocumentType, EducationDocumentTypeExternalDTO> updateEducationDocumentTypeActions
                = _updateEducationDocumentTypeActionsCreator.CreateActions();

            return await UpdateDictionaryHandler<EducationDocumentType, EducationDocumentTypeExternalDTO>
               .UpdateAsync(deleteRelatedEntities, updateEducationDocumentTypeActions);
        }
    }
}