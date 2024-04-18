using DictionaryService.Core.Application.DTOs;
using DictionaryService.Core.Application.Interfaces.Services;
using DictionaryService.Core.Application.Interfaces.Repositories;
using DictionaryService.Core.Application.Interfaces.Transaction;
using DictionaryService.Core.Domain;
using DictionaryService.Core.Application.UpdateDictionaryTools.UpdateDictionaryHandler;
using DictionaryService.Core.Application.UpdateDictionaryTools.UpdateActionsCreators.Base;
using Common.Enums;
using Common.Models;
using Common.DTOs;
using System.Collections.Generic;

namespace DictionaryService.Core.Application.Services
{
    public class UpdateDictionaryService : IUpdateDictionaryService
    {
        private readonly IUpdateStatusRepository _updateStatusRepository;
        private readonly ITransactionProvider _transactionProvider;
        private readonly INotificationService _notificationService;

        private readonly UpdateActionsCreator<Faculty, FacultyExternalDTO> _updateFacultyActionsCreator;
        private readonly UpdateActionsCreator<EducationLevel, EducationLevelExternalDTO> _updateEducationLevelActionsCreator;
        private readonly UpdateActionsCreator<EducationProgram, EducationProgramExternalDTO> _updateEducationProgramActionsCreator;
        private readonly UpdateActionsCreator<EducationDocumentType, EducationDocumentTypeExternalDTO> _updateEducationDocumentTypeActionsCreator;

        public UpdateDictionaryService(
            IUpdateStatusRepository updateStatusRepository, ITransactionProvider transactionProvider,
            INotificationService notificationService,
            UpdateActionsCreator<Faculty, FacultyExternalDTO> updateFacultyActionsCreator,
            UpdateActionsCreator<EducationLevel, EducationLevelExternalDTO> updateEducationLevelActionsCreator,
            UpdateActionsCreator<EducationProgram, EducationProgramExternalDTO> updateEducationProgramActionsCreator,
            UpdateActionsCreator<EducationDocumentType, EducationDocumentTypeExternalDTO> updateEducationDocumentTypeActionsCreator)
        {
            _updateStatusRepository = updateStatusRepository;
            _transactionProvider = transactionProvider;
            _notificationService = notificationService;

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
                ExecutionResult<List<Faculty>> updateFacultyResult = await UpdateFacultyAsync(true);
                if (!updateFacultyResult.IsSuccess) return updateFacultyResult; 

                ExecutionResult<List<EducationLevel>> updateEducationLevelResult = await UpdateEducationLevelAsync(true);
                if (!updateEducationLevelResult.IsSuccess) return updateEducationLevelResult;

                ExecutionResult<List<EducationProgram>> updateEducationProgramResult = await UpdateEducationProgramAsync(true);
                if (!updateEducationProgramResult.IsSuccess) return updateEducationProgramResult;

                ExecutionResult<List<EducationDocumentType>> updateEducationDocumentTypeResult = await UpdateEducationDocumentTypeAsync(true);
                if (!updateEducationDocumentTypeResult.IsSuccess) return updateEducationDocumentTypeResult;

                return await SendNotificationHandlerAsync(updateFacultyResult, updateEducationLevelResult, updateEducationProgramResult, updateEducationDocumentTypeResult);
            }, async (changeStatus, message) =>
            {
                if (changeStatus)
                {
                    List<UpdateStatus> updateStatuses = await _updateStatusRepository.GetAllAsync();

                    updateStatuses.ForEach(updateStatus =>
                    {
                        updateStatus!.Status = UpdateStatusEnum.ErrorInUpdating;
                        updateStatus!.Comments = message;
                    });

                    await _updateStatusRepository.SaveChangesAsync();
                }
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
                var a = dictionaryType switch
                {
                    DictionaryType.Faculty => await SendNotificationHandlerAsync(
                        await UpdateFacultyAsync(), _notificationService.ChangedFacultiesAsync),
                    DictionaryType.EducationProgram => await SendNotificationHandlerAsync(
                        await UpdateEducationProgramAsync(), _notificationService.ChangedEducationProgramAsync),
                    DictionaryType.EducationLevel => await SendNotificationHandlerAsync(
                        await UpdateEducationLevelAsync(), _notificationService.ChangedEducationLevelAsync),
                    DictionaryType.EducationDocumentType => await SendNotificationHandlerAsync(
                        await UpdateEducationDocumentTypeAsync(), _notificationService.ChangedEducationDocumentTypeAsync),
                    _ => new(keyError: "WrongDictionaryType", error: "Wrong dictionary type"),
                };

                return a;
            }, async (changeStatus, message) =>
            {
                if(changeStatus)
                {
                    UpdateStatus? updateStatus = await _updateStatusRepository.GetByDictionaryTypeAsync(dictionaryType);
                    updateStatus!.Status = UpdateStatusEnum.ErrorInUpdating;
                    updateStatus!.Comments = message;
                    await _updateStatusRepository.UpdateAsync(updateStatus);
                }
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

        private async Task<ExecutionResult> SendNotificationHandlerAsync(
            ExecutionResult<List<Faculty>> updateFaculty, ExecutionResult<List<EducationLevel>> updateEducationLevel,
            ExecutionResult<List<EducationProgram>> updateEducationProgram, ExecutionResult<List<EducationDocumentType>> updateEducationDocumentType)
        {
            ExecutionResult result = await SendNotificationHandlerAsync(updateFaculty, _notificationService.ChangedFacultiesAsync);
            if (!result.IsSuccess) return result;

            result = await SendNotificationHandlerAsync(updateEducationLevel, _notificationService.ChangedEducationLevelAsync);
            if (!result.IsSuccess) return result;

            result = await SendNotificationHandlerAsync(updateEducationProgram, _notificationService.ChangedEducationProgramAsync);
            if (!result.IsSuccess) return result;

            result = await SendNotificationHandlerAsync(updateEducationDocumentType, _notificationService.ChangedEducationDocumentTypeAsync);
            if (!result.IsSuccess) return result;

            return new(isSuccess: true);
        }

        private async Task<ExecutionResult> SendNotificationHandlerAsync<TEntity>(ExecutionResult<List<TEntity>> changedEntities, Func<TEntity, Task<ExecutionResult>> notificationOperationAsync)
        {
            if (!changedEntities.IsSuccess) return changedEntities;

            foreach (var entity in changedEntities.Result!)
            {
                await notificationOperationAsync(entity);
            }

            return new(isSuccess: true);
        }

        private async Task<ExecutionResult> UpdateDictionaryHandlerAsync(Func<Task<ExecutionResult>> updateOperationAsync, Func<bool, string, Task> onErrorAsync)
        {
            // Создаем транзакцию для отмены изменений, если что-то пошло не так
            using ITransaction transaction = await _transactionProvider.CreateTransactionScopeAsync();

            try
            {
                ExecutionResult executionResult = await updateOperationAsync();

                if (!executionResult.IsSuccess)
                {
                    await transaction.RollbackAsync();

                    await onErrorAsync(false, "");
                }
                else
                {
                    await transaction.CommitAsync();
                }

                return executionResult;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                await onErrorAsync(true, ex.ToString());

                throw;
            }
        }

        private async Task<ExecutionResult<List<Faculty>>> UpdateFacultyAsync(bool deleteRelatedEntities = false)
        {
            UpdateDictionaryActions<Faculty, FacultyExternalDTO> updateFacultyActions 
                = _updateFacultyActionsCreator.CreateActions();

            return await UpdateDictionaryHandler<Faculty, FacultyExternalDTO>
                .UpdateAsync(deleteRelatedEntities, updateFacultyActions);
        }

        private async Task<ExecutionResult<List<EducationLevel>>> UpdateEducationLevelAsync(bool deleteRelatedEntities = false)
        {
            UpdateDictionaryActions<EducationLevel, EducationLevelExternalDTO> updateEducationLevelActions
                = _updateEducationLevelActionsCreator.CreateActions();

            return await UpdateDictionaryHandler<EducationLevel, EducationLevelExternalDTO>
                .UpdateAsync(deleteRelatedEntities, updateEducationLevelActions);
        }

        private async Task<ExecutionResult<List<EducationProgram>>> UpdateEducationProgramAsync(bool deleteRelatedEntities = false)
        {
            UpdateDictionaryActions<EducationProgram, EducationProgramExternalDTO> updateEducationProgramActions
                = _updateEducationProgramActionsCreator.CreateActions();

            return await UpdateDictionaryHandler<EducationProgram, EducationProgramExternalDTO>
               .UpdateAsync(deleteRelatedEntities, updateEducationProgramActions);
        }

        private async Task<ExecutionResult<List<EducationDocumentType>>> UpdateEducationDocumentTypeAsync(bool deleteRelatedEntities = false)
        {
            UpdateDictionaryActions<EducationDocumentType, EducationDocumentTypeExternalDTO> updateEducationDocumentTypeActions
                = _updateEducationDocumentTypeActionsCreator.CreateActions();

            return await UpdateDictionaryHandler<EducationDocumentType, EducationDocumentTypeExternalDTO>
               .UpdateAsync(deleteRelatedEntities, updateEducationDocumentTypeActions);
        }
    }
}