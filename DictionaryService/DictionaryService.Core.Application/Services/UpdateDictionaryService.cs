using DictionaryService.Core.Application.DTOs;
using DictionaryService.Core.Application.Interfaces.Services;
using DictionaryService.Core.Domain.Enum;
using DictionaryService.Core.Application.Interfaces.Repositories;
using DictionaryService.Core.Domain;
using Common.Models;
using Common.Repositories;
using DictionaryService.Core.Application.Interfaces.Transaction;

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

#region private

        private async Task<ExecutionResult> CheckOtherUpdatingAsync()
        {
            bool existOtherUpdating = await _updateStatusRepository.CheckOtherUpdatingAsync();
            if (existOtherUpdating)
            {
                return new(keyError: "ExistOtherUpdating", error: "Another directory update is already underway, try it later.");
            }

            return new(isSuccess: true);
        }


        private async Task<ExecutionResult> UpdateFacultyAsync(bool saveChanges = true, bool deleteRelatedEntities = false)
        {
            // +++ если добавляем, то ничего не проверяем в других сущностях, если прилетела сущность и она есть в бд, но мягко удалена, то восстанавливаем ее
            // +++ если удаляем, то 1) если DeleteRelatedEntities == true, находим связанные записи в других сущностях и делаем мягкое удаление в них, или иначе 2) ошибка
            // +++ если обновляем, то ничего не проверяем в других сущностях

            GetEntityAsync<Faculty> getEntityAsync = _facultyRepository.GetAllAsync;
            GetExternalEntityAsync<FacultyExternalDTO> getExternalEntityAsync = _externalDictionaryService.GetFacultiesAsync;
            CompareKey<Faculty, FacultyExternalDTO> compareKey = (faculty, externalFaculty) => faculty.Id == externalFaculty.Id;
            OnUpdateEntity<Faculty, FacultyExternalDTO> onUpdateEntity = (faculty, externalFaculty) =>
            {
                faculty.Name = externalFaculty.Name;
                faculty.Deprecated = false;
            };
            OnAddEntity<Faculty, FacultyExternalDTO> onAddEntity = (externalFaculty) =>
            {
                Faculty newFaculty = new()
                {
                    Id = externalFaculty.Id,
                    Name = externalFaculty.Name,
                    Deprecated = false,
                };

                return newFaculty;
            };
            OnDeleteEntityAsync<Faculty> onDeleteEntityAsync = async (faculty, comments) =>
            {
                bool thereAreRelated = false;
                List<EducationProgram> educationPrograms = await _educationProgramRepository.GetAllByFacultyIdAsync(faculty.Id);
                thereAreRelated |= SoftDeleteEntityIf(deleteRelatedEntities, educationPrograms, comments);
                return thereAreRelated;
            };

            return await UpdateAsync(saveChanges, deleteRelatedEntities, _facultyRepository, compareKey, getEntityAsync, getExternalEntityAsync, onUpdateEntity, onAddEntity, onDeleteEntityAsync);
        }

        private async Task<ExecutionResult> UpdateEducationLevelAsync(bool saveChanges = true, bool deleteRelatedEntities = false)
        {
            // если добавляем, то ничего не проверяем в других сущностях, если прилетела сущность и она есть в бд, но мягко удалена, то восстанавливаем ее
            // если удаляем, то 1) если DeleteRelatedEntities == true, находим связанные записи в других сущностях и делаем мягкое удаление в них, или иначе 2) ошибка
            // если обновляем, то ничего не проверяем в других сущностях

            GetEntityAsync<EducationLevel> getEntityAsync = _educationLevelRepository.GetAllAsync;
            GetExternalEntityAsync<EducationLevelExternalDTO> getExternalEntityAsync = _externalDictionaryService.GetEducationLevelsAsync;
            CompareKey<EducationLevel, EducationLevelExternalDTO> compareKey = (educationLevel, externalEducationLevel) => educationLevel.ExternalId == externalEducationLevel.Id;
            OnUpdateEntity<EducationLevel, EducationLevelExternalDTO> onUpdateEntity = (educationLevel, externalEducationLevel) =>
            {
                educationLevel.Name = externalEducationLevel.Name;
                educationLevel.Deprecated = false;
            };
            OnAddEntity<EducationLevel, EducationLevelExternalDTO> onAddEntity = (externalEducationLevel) =>
            {
                EducationLevel newEducationLevel = new()
                {
                    ExternalId = externalEducationLevel.Id,
                    Name = externalEducationLevel.Name,
                    Deprecated = false,
                };

                return newEducationLevel;
            };
            OnDeleteEntityAsync<EducationLevel> onDeleteEntityAsync = async (educationLevel, comments) =>
            {
                bool thereAreRelated = false;

                List<EducationProgram> educationPrograms = await _educationProgramRepository.GetAllByEducationLevelIdAsync(educationLevel.Id);
                thereAreRelated |= SoftDeleteEntityIf(deleteRelatedEntities, educationPrograms, comments);

                List<EducationDocumentType> educationDocumentTypes = await _educationDocumentTypeRepository.GetAllByNextEducationLevelIdAsync(educationLevel.Id);
                thereAreRelated |= SoftDeleteEntityIf(deleteRelatedEntities, educationDocumentTypes, comments);

                return thereAreRelated;
            };

            return await UpdateAsync(saveChanges, deleteRelatedEntities, _educationLevelRepository, compareKey, getEntityAsync, getExternalEntityAsync, onUpdateEntity, onAddEntity, onDeleteEntityAsync);
        }

        private async Task<ExecutionResult> UpdateEducationProgramAsync(bool saveChanges = true, bool deleteRelatedEntities = false)
        {
            // если добавляем, то проверяем правильность внешних ссылок, иначе ошибка, если прилетела сущность и она есть в бд, но мягко удалена, то восстанавливаем ее
            // если удаляем, то ничего не проверяем в других сущностях
            // если обновляем, то проверяем правильность внешних ссылок при их изменении, иначе ошибка

            var educationProgram = await _externalDictionaryService.GetEducationProgramAsync();

            throw new NotImplementedException();
        }

        private async Task<ExecutionResult> UpdateEducationDocumentTypeAsync(bool saveChanges = true, bool deleteRelatedEntities = false)
        {
            // если добавляем, то проверяем правильность внешних ссылок, иначе ошибка, если прилетела сущность и она есть в бд, но мягко удалена, то восстанавливаем ее
            // если удаляем,  то ничего не проверяем в других сущностях
            // если обновляем, то проверяем правильность внешних ссылок при их изменении, иначе ошибка

            // в faculties есть, есть и в existFaculties -> Обновляем запись (проверка через FirstOrDefault по id)
            // в faculties есть, но нет в existFaculties -> Добавляем запись (проверка через facultiesForAdd)
            // в faculties нет, но есть в existFaculties -> Удаляем запись 

            var educationDocuments = await _externalDictionaryService.GetEducationDocumentTypesAsync();

            throw new NotImplementedException();
        }

        private bool SoftDeleteEntityIf<TEntity>(bool deleteRelatedEntities, List<TEntity> entities, List<string> comments) where TEntity : BaseDictionaryEntity
        {
            bool thereAreRelated = false;

            foreach (var entity in entities)
            {
                if (!deleteRelatedEntities && !entity.Deprecated)
                {
                    thereAreRelated = true;
                    comments.Add($"The educational program '{entity.Name}' refers to the education level '{entity.Name}'.");
                    continue;
                }

                entity.Deprecated = true;
            }

            return thereAreRelated;
        }

        private async Task<ExecutionResult> UpdateAsync<TEntity, TExternalEntity, TRepository>(
            bool saveChanges, bool deleteRelatedEntities, TRepository repository,
            CompareKey<TEntity, TExternalEntity> compareKey,
            GetEntityAsync<TEntity> getEntityAsync,
            GetExternalEntityAsync<TExternalEntity> getExternalEntityAsync,
            OnUpdateEntity<TEntity, TExternalEntity> onUpdateEntity,
            OnAddEntity<TEntity, TExternalEntity> onAddEntity,
            OnDeleteEntityAsync<TEntity> onDeleteEntity
        )
            where TEntity : BaseDictionaryEntity
            where TExternalEntity : class
            where TRepository : IBaseRepository<TEntity>
        {
            // Получаем данные из внешнего сервиса
            ExecutionResult<List<TExternalEntity>> getExternalEntityResult = await getExternalEntityAsync();
            if (!getExternalEntityResult.IsSuccess) return getExternalEntityResult;
            List<TExternalEntity> externalEntities = getExternalEntityResult.Result!;

            // Получаем данные в нашей базе данных
            List<TEntity> existEntities = await getEntityAsync();

            // Словарь для хранения тех сущностей, которые существуют только в нашей бд, то есть были удалены во внешнем сервисе
            Dictionary<Guid, TEntity> existEntitiesForRemove = existEntities.ToDictionary(entity => entity.Id);
            foreach (var externalEntity in externalEntities)
            {
                TEntity? existEntity = existEntities.FirstOrDefault(existEntity => compareKey(existEntity, externalEntity));
                if (existEntity is not null)
                {
                    // Обновляем запись...
                    onUpdateEntity(existEntity, externalEntity);
                    existEntitiesForRemove.Remove(existEntity.Id);
                }
                else
                {
                    // Добавляем запись...
                    TEntity newEntity = onAddEntity(externalEntity);
                    await repository.AddAsync(newEntity);
                }
            }

            // Пробуем удалить записи
            bool thereAreRelated = false;
            List<string> comments = new();
            foreach (var entityForRemove in existEntitiesForRemove.Values)
            {
                if (entityForRemove.Deprecated) continue;

                // Удаляем запись...
                TEntity? entity = await repository.GetByIdAsync(entityForRemove.Id);
                if (entity is null)
                {
                    return new(keyError: "UnknownError", error: "Unknown error.");
                }

                thereAreRelated = await onDeleteEntity(entity, comments);
                entity.Deprecated = true;
            }

            if (!deleteRelatedEntities && thereAreRelated)
            {
                return new(keyError: "DeleteRelatedError", error: "It is not possible to delete a record because it is referenced by other records.");
            }

            if (saveChanges) await repository.SaveChangesAsync();

            return new(isSuccess: true);
        }

        private delegate Task<List<TEntity>> GetEntityAsync<TEntity>();
        private delegate Task<ExecutionResult<List<TExternalEntity>>> GetExternalEntityAsync<TExternalEntity>();
        private delegate bool CompareKey<TEntity, TExternalEntity>(TEntity entity, TExternalEntity externalEntity);
        private delegate void OnUpdateEntity<TEntity, TExternalEntity>(TEntity entity, TExternalEntity externalEntity);
        private delegate TEntity OnAddEntity<TEntity, TExternalEntity>(TExternalEntity externalEntity);
        private delegate Task<bool> OnDeleteEntityAsync<TEntity>(TEntity entity, List<string> comments);

#endregion private
    }
}