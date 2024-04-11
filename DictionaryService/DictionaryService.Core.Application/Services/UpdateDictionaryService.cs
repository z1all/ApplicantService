using DictionaryService.Core.Application.DTOs;
using DictionaryService.Core.Application.Interfaces.Services;
using DictionaryService.Core.Application.Interfaces.Repositories;
using DictionaryService.Core.Application.Interfaces.Transaction;
using DictionaryService.Core.Domain.Enum;
using DictionaryService.Core.Domain;
using Common.Models;
using Common.Repositories;

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
                thereAreRelated |= SoftDeleteEntityIf(deleteRelatedEntities, educationPrograms, comments, entity =>
                    $"The educational program '{entity.Name}' refers to the education level '{faculty.Name}'.");

                return thereAreRelated;
            };

            return await UpdateAsync(saveChanges, deleteRelatedEntities, _facultyRepository, compareKey, getEntityAsync, getExternalEntityAsync, onUpdateEntity, onAddEntity, onDeleteEntityAsync);
        }

        private async Task<ExecutionResult> UpdateEducationLevelAsync(bool saveChanges = true, bool deleteRelatedEntities = false)
        {
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
                thereAreRelated |= SoftDeleteEntityIf(deleteRelatedEntities, educationPrograms, comments, entity => 
                    $"The educational program '{entity.Name}' refers to the education level '{educationLevel.Name}'.");

                List<EducationDocumentType> documentTypesRelatedWithNextLevel = await _educationDocumentTypeRepository.GetAllByNextEducationLevelIdAsync(educationLevel.Id);
                thereAreRelated |= SoftDeleteEntityIf(deleteRelatedEntities, documentTypesRelatedWithNextLevel, comments, entity =>
                    $"The educational document type '{entity.Name}' refers to the education level '{educationLevel.Name}'.");

                List<EducationDocumentType> documentTypesRelatedWithCurrentLevel = await _educationDocumentTypeRepository.GetByCurrentEducationLevelIdAsync(educationLevel.Id);
                thereAreRelated |= SoftDeleteEntityIf(deleteRelatedEntities, documentTypesRelatedWithCurrentLevel, comments, entity => 
                    $"The educational document type '{entity.Name}' refers to the education level '{educationLevel.Name}'.");

                return thereAreRelated;
            };

            return await UpdateAsync(saveChanges, deleteRelatedEntities, _educationLevelRepository, compareKey, getEntityAsync, getExternalEntityAsync, onUpdateEntity, onAddEntity, onDeleteEntityAsync);
        }

        private async Task<ExecutionResult> UpdateEducationProgramAsync(bool saveChanges = true, bool deleteRelatedEntities = false)
        {
            List<EducationLevel> educationLevelsCache = await _educationLevelRepository.GetAllAsync();
            List<Faculty> facultiesCache = await _facultyRepository.GetAllAsync();

            GetEntityAsync<EducationProgram> getEntityAsync = _educationProgramRepository.GetAllAsync;
            GetExternalEntityAsync<EducationProgramExternalDTO> getExternalEntityAsync = _externalDictionaryService.GetEducationProgramAsync;
            CompareKey<EducationProgram, EducationProgramExternalDTO> compareKey = (program, externalProgram) => program.Id == externalProgram.Id;
            OnUpdateEntity<EducationProgram, EducationProgramExternalDTO> onUpdateEntity = (program, externalProgram) =>
            {
                EducationLevel educationLevel = educationLevelsCache.First(educationLevel => educationLevel.ExternalId == externalProgram.EducationLevel.Id);

                program.CreatedTime = externalProgram.CreateTime.ToUniversalTime();
                program.Name = externalProgram.Name;
                program.Code = externalProgram.Code;
                program.Language = externalProgram.Language;
                program.EducationForm = externalProgram.EducationForm;
                program.EducationLevelId = educationLevel.Id;
                program.FacultyId = externalProgram.Faculty.Id;
                program.Deprecated = false;
            };
            OnAddEntity<EducationProgram, EducationProgramExternalDTO> onAddEntity = (externalProgram) =>
            {
                EducationLevel educationLevel = educationLevelsCache.First(educationLevel => educationLevel.ExternalId == externalProgram.EducationLevel.Id);

                EducationProgram newEducationLevel = new()
                {
                    Id = externalProgram.Id,
                    CreatedTime = externalProgram.CreateTime.ToUniversalTime(),
                    Name = externalProgram.Name,
                    Code = externalProgram.Code,
                    Language = externalProgram.Language,    
                    EducationForm = externalProgram.EducationForm,
                    EducationLevelId = educationLevel.Id,
                    FacultyId = externalProgram.Faculty.Id,
                    Deprecated = false,
                };

                return newEducationLevel;
            };
            CheckBeforeAddEntityAsync<EducationProgramExternalDTO> checkBeforeAdd = (externalProgram, comments) =>
            {
                bool facultyExist = facultiesCache.Any(faculty => faculty.Id == externalProgram.Faculty.Id);
                if (!facultyExist)
                {
                    comments.Add($"The education program '{externalProgram.Name}' refers to a non-existent faculty '{externalProgram.Faculty.Name}'.");
                }

                bool educationLevelExist = educationLevelsCache.Any(educationLevel => educationLevel.ExternalId == externalProgram.EducationLevel.Id);
                if (!educationLevelExist)
                {
                    comments.Add($"The education program '{externalProgram.Name}' refers to a non-existent education level '{externalProgram.EducationLevel.Name}'.");
                }

                return Task.FromResult(facultyExist && educationLevelExist);
            };
            CheckBeforeUpdateEntityAsync<EducationProgram, EducationProgramExternalDTO > checkBeforeUpdate = async (program, externalProgram, comments) =>
            {
                if (program.FacultyId == externalProgram.Faculty.Id) return true;
                return await checkBeforeAdd(externalProgram, comments);
            };
            
            return await UpdateAsync(saveChanges, deleteRelatedEntities, _educationProgramRepository, compareKey, getEntityAsync, getExternalEntityAsync, onUpdateEntity, onAddEntity, null, checkBeforeUpdate, checkBeforeAdd);
        }

        private async Task<ExecutionResult> UpdateEducationDocumentTypeAsync(bool saveChanges = true, bool deleteRelatedEntities = false)
        {
            List<EducationLevel> educationLevelsCache = await _educationLevelRepository.GetAllAsync();

            GetEntityAsync<EducationDocumentType> getEntityAsync = _educationDocumentTypeRepository.GetAllAsync;
            GetExternalEntityAsync<EducationDocumentTypeExternalDTO> getExternalEntityAsync = _externalDictionaryService.GetEducationDocumentTypesAsync;
            CompareKey<EducationDocumentType, EducationDocumentTypeExternalDTO> compareKey = (documentType, externalDocumentType) => documentType.Id == externalDocumentType.Id;
            OnUpdateEntity<EducationDocumentType, EducationDocumentTypeExternalDTO> onUpdateEntity = (documentType, externalDocumentType) =>
            {
                EducationLevel currentEducationLevel = educationLevelsCache.First(educationLevel => educationLevel.ExternalId == externalDocumentType.EducationLevel.Id);

                documentType.Name = externalDocumentType.Name;
                documentType.EducationLevelId = currentEducationLevel.Id;
                documentType.Deprecated = false;

                List<EducationLevel> addNextEducationLevels = new();
                foreach (var externalNextEducationLevel in externalDocumentType.NextEducationLevels)
                {
                    EducationLevel educationLevel = educationLevelsCache.First(educationLevel => educationLevel.ExternalId == externalNextEducationLevel.Id);

                    addNextEducationLevels.Add(educationLevel);
                }
                documentType.NextEducationLevels = addNextEducationLevels;
            };
            OnAddEntity<EducationDocumentType, EducationDocumentTypeExternalDTO> onAddEntity = (externalDocumentType) =>
            {
                EducationLevel currentEducationLevel = educationLevelsCache.First(educationLevel => educationLevel.ExternalId == externalDocumentType.EducationLevel.Id);

                EducationDocumentType documentType = new()
                {
                    Id = externalDocumentType.Id,
                    Name = externalDocumentType.Name,
                    EducationLevelId = currentEducationLevel.Id,
                    Deprecated = false,
                };

                List<EducationLevel> addNextEducationLevels = new();
                foreach (var externalNextEducationLevel in externalDocumentType.NextEducationLevels)
                {
                    EducationLevel educationLevel = educationLevelsCache.First(educationLevel => educationLevel.ExternalId == externalNextEducationLevel.Id);

                    addNextEducationLevels.Add(educationLevel);
                }
                documentType.NextEducationLevels = addNextEducationLevels;

                return documentType;
            };
            CheckBeforeAddEntityAsync<EducationDocumentTypeExternalDTO> checkBeforeAdd = (externalDocumentType, comments) =>
            {
                bool currentEducationLevelExist = educationLevelsCache.Any(educationLevel => educationLevel.ExternalId == externalDocumentType.EducationLevel.Id);
                if (!currentEducationLevelExist)
                {
                    comments.Add($"The education document type '{externalDocumentType.Name}' refers to a non-existent education level '{externalDocumentType.EducationLevel.Name}'.");
                }

                bool nextEducationLevelExist = true;
                foreach (var externalNextEducationLevel in externalDocumentType.NextEducationLevels)
                {
                    bool exist = educationLevelsCache.Any(educationLevel => educationLevel.ExternalId == externalNextEducationLevel.Id);
                    if(!exist)
                    {
                        comments.Add($"The education document type '{externalDocumentType.Name}' refers to a non-existent next education level '{externalNextEducationLevel.Name}'.");
                    }
                    nextEducationLevelExist &= exist;
                }

                return Task.FromResult(currentEducationLevelExist && nextEducationLevelExist);
            };
            CheckBeforeUpdateEntityAsync<EducationDocumentType, EducationDocumentTypeExternalDTO> checkBeforeUpdate = async (documentType, externalDocumentType, comments) =>
            {
                return await checkBeforeAdd(externalDocumentType, comments);
            };

            return await UpdateAsync(saveChanges, deleteRelatedEntities, _educationDocumentTypeRepository, compareKey, getEntityAsync, getExternalEntityAsync, onUpdateEntity, onAddEntity, null, checkBeforeUpdate, checkBeforeAdd);
        }

        private bool SoftDeleteEntityIf<TEntity>(bool deleteRelatedEntities, List<TEntity> relatedEntities, List<string> comments, OnAddErrorMessage<TEntity> onAddErrorMessage) 
            where TEntity : BaseDictionaryEntity
        {
            bool thereAreRelated = false;

            foreach (var relatedEntity in relatedEntities)
            {
                if (!deleteRelatedEntities && !relatedEntity.Deprecated)
                {
                    thereAreRelated = true;
                    comments.Add(onAddErrorMessage(relatedEntity));
                    continue;
                }

                relatedEntity.Deprecated = true;
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
            OnDeleteEntityAsync<TEntity>? onDeleteEntityAsync = null,
            CheckBeforeUpdateEntityAsync<TEntity, TExternalEntity>? checkBeforeUpdateEntityAsync = null,
            CheckBeforeAddEntityAsync<TExternalEntity>? checkBeforeAddEntityAsync = null
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

            // Пробуем обновлять и добавлять записи
            ExecutionResult<Dictionary<Guid, TEntity>> updatingAndAddingResult
                = await DoUpdateAndAddEntitiesAsync(repository, compareKey, onUpdateEntity, onAddEntity, checkBeforeUpdateEntityAsync, checkBeforeAddEntityAsync, externalEntities, existEntities);
            if (!updatingAndAddingResult.IsSuccess) return updatingAndAddingResult;
            Dictionary<Guid, TEntity> existEntitiesForRemove = updatingAndAddingResult.Result!;

            // Пробуем удалить записи
            ExecutionResult deletingResult
                = await DoDeleteEntitiesAsync(deleteRelatedEntities, repository, onDeleteEntityAsync, existEntitiesForRemove);
            if (!deletingResult.IsSuccess) return deletingResult;

            if (saveChanges) await repository.SaveChangesAsync();

            return new(isSuccess: true);
        }

        private async Task<ExecutionResult<Dictionary<Guid, TEntity>>> DoUpdateAndAddEntitiesAsync<TEntity, TExternalEntity, TRepository>(
            TRepository repository, CompareKey<TEntity, TExternalEntity> compareKey,
            OnUpdateEntity<TEntity, TExternalEntity> onUpdateEntity,
            OnAddEntity<TEntity, TExternalEntity> onAddEntity,
            CheckBeforeUpdateEntityAsync<TEntity, TExternalEntity>? checkBeforeUpdateEntityAsync,
            CheckBeforeAddEntityAsync<TExternalEntity>? checkBeforeAddEntityAsync,
            List<TExternalEntity> externalEntities, List<TEntity> existEntities 
        )
            where TEntity : BaseDictionaryEntity
            where TExternalEntity : class
            where TRepository : IBaseRepository<TEntity>
        {
            // Словарь для хранения тех сущностей, которые существуют только в нашей бд, то есть были удалены во внешнем сервисе
            Dictionary<Guid, TEntity> existEntitiesForRemove = existEntities.ToDictionary(entity => entity.Id);
            bool thereAreNotRelated = false;
            List<string> comments = new();
            foreach (var externalEntity in externalEntities)
            {
                TEntity? existEntity = existEntities.FirstOrDefault(existEntity => compareKey(existEntity, externalEntity));
                if (existEntity is not null)
                {
                    if (checkBeforeUpdateEntityAsync is not null && !(await checkBeforeUpdateEntityAsync(existEntity, externalEntity, comments)))
                    {
                        thereAreNotRelated = true;
                        continue;
                    }

                    // Обновляем запись...
                    onUpdateEntity(existEntity, externalEntity);
                    existEntitiesForRemove.Remove(existEntity.Id);
                }
                else
                {
                    if(checkBeforeAddEntityAsync is not null && !(await checkBeforeAddEntityAsync(externalEntity, comments)))
                    {
                        thereAreNotRelated = true;
                        continue;
                    }

                    // Добавляем запись...
                    TEntity newEntity = onAddEntity(externalEntity);
                    await repository.AddAsync(newEntity);
                }
            }

            if (thereAreNotRelated)
            {
                return new(
                    keyError: "UpdateOrAddEntityError",
                    error: $"It is not possible to update or add a record because it refers to a non-existent record.\n{string.Join('\n', comments)}"
                );
            }

            return new() { Result = existEntitiesForRemove };
        }

        private async Task<ExecutionResult> DoDeleteEntitiesAsync<TEntity, TRepository>(
            bool deleteRelatedEntities, TRepository repository,
            OnDeleteEntityAsync<TEntity>? onDeleteEntityAsync,
            Dictionary<Guid, TEntity> existEntitiesForRemove
        )
            where TEntity : BaseDictionaryEntity
            where TRepository : IBaseRepository<TEntity>
        {
            bool thereAreRelated = false;
            List<string> comments = new();
            foreach (var entityForRemove in existEntitiesForRemove.Values)
            {
                // Если удален, то пропускаем
                if (entityForRemove.Deprecated) continue;

                TEntity? entity = await repository.GetByIdAsync(entityForRemove.Id);
                if (entity is null)
                {
                    return new(keyError: "UnknownError", error: "Unknown error.");
                }

                // Удаляем запись...
                if (onDeleteEntityAsync is not null)
                {
                    thereAreRelated = await onDeleteEntityAsync(entity, comments);
                }

                entity.Deprecated = true;
            }

            if (!deleteRelatedEntities && thereAreRelated)
            {
                return new(
                    keyError: "DeleteEntityError", 
                    error: $"It is not possible to delete a record because it is referenced by other records.\n{string.Join('\n', comments)}"
                );
            }

            return new(isSuccess: true);
        }

        private delegate string OnAddErrorMessage<TEntity>(TEntity entity);

        private delegate Task<List<TEntity>> GetEntityAsync<TEntity>();
        private delegate Task<ExecutionResult<List<TExternalEntity>>> GetExternalEntityAsync<TExternalEntity>();
        private delegate bool CompareKey<TEntity, TExternalEntity>(TEntity entity, TExternalEntity externalEntity);
        private delegate Task<bool> CheckBeforeUpdateEntityAsync<TEntity, TExternalEntity>(TEntity entity, TExternalEntity externalEntity, List<string> comments);
        private delegate void OnUpdateEntity<TEntity, TExternalEntity>(TEntity entity, TExternalEntity externalEntity);
        private delegate Task<bool> CheckBeforeAddEntityAsync<TExternalEntity>(TExternalEntity externalEntity, List<string> comments);
        private delegate TEntity OnAddEntity<TEntity, TExternalEntity>(TExternalEntity externalEntity);
        private delegate Task<bool> OnDeleteEntityAsync<TEntity>(TEntity entity, List<string> comments);

#endregion private
    }
}