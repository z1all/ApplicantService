using DictionaryService.Core.Application.DTOs;
using DictionaryService.Core.Application.Interfaces.Services;
using DictionaryService.Core.Domain.Enum;
using DictionaryService.Core.Application.Interfaces.Repositories;
using DictionaryService.Core.Domain;
using Common.Models;
using Common.Repositories;

namespace DictionaryService.Core.Application.Services
{
    public class UpdateDictionaryService : IUpdateDictionaryService
    {
        private readonly IUpdateStatusRepository _updateStatusRepository;
        private readonly IFacultyRepository _facultyRepository;
        private readonly IEducationProgramRepository _educationProgramRepository;
        private readonly IExternalDictionaryService _externalDictionaryService;

        public UpdateDictionaryService(
            IUpdateStatusRepository updateStatusRepository, IFacultyRepository facultyRepository, 
            IEducationProgramRepository educationProgramRepository, IExternalDictionaryService externalDictionaryService)
        {
            _updateStatusRepository = updateStatusRepository;
            _facultyRepository = facultyRepository;
            _educationProgramRepository = educationProgramRepository;
            _externalDictionaryService = externalDictionaryService;
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

            return dictionaryType switch
            {
                DictionaryType.Faculty => await UpdateFacultyAsync(),
                DictionaryType.EducationLevel => await UpdateEducationLevelAsync(),
                DictionaryType.EducationProgram => await UpdateEducationProgramAsync(),
                DictionaryType.EducationDocumentType => await UpdateEducationDocumentTypeAsync(),
                _ => new(keyError: "WrongDictionaryType", error: "Wrong dictionary type"),
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
            // +++ если добавляем, то ничего не проверяем в других сущностях, если прилетела сущность и она есть в бд, но мягко удалена, то восстанавливаем ее
            // +++ если удаляем, то 1) если DeleteRelatedEntities == true, находим связанные записи в других сущностях и делаем мягкое удаление в них, или иначе 2) ошибка
            // +++ если обновляем, то ничего не проверяем в других сущностях

            GetEntityAsync<Faculty> getEntityAsync = _facultyRepository.GetAllAsync;
            GetExternalEntityAsync<FacultyExternalDTO> getExternalEntityAsync = _externalDictionaryService.GetFacultiesAsync;
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
                foreach (var educationProgram in educationPrograms)
                {
                    if (!deleteRelatedEntities)
                    {
                        thereAreRelated = true;
                        comments.Add($"The educational program {educationProgram.Name} refers to the faculty {faculty.Name}.");
                        continue;
                    }

                    educationProgram.Deprecated = true;
                }
                return thereAreRelated;
            };

            return await UpdateAsync(deleteRelatedEntities, _facultyRepository, getEntityAsync, getExternalEntityAsync, onUpdateEntity, onAddEntity, onDeleteEntityAsync);
        }

        private async Task<ExecutionResult> UpdateEducationLevelAsync(bool deleteRelatedEntities = false)
        {
            // если добавляем, то ничего не проверяем в других сущностях, если прилетела сущность и она есть в бд, но мягко удалена, то восстанавливаем ее
            // если удаляем, то 1) если DeleteRelatedEntities == true, находим связанные записи в других сущностях и делаем мягкое удаление в них, или иначе 2) ошибка
            // если обновляем, то ничего не проверяем в других сущностях

            // в faculties есть, есть и в existFaculties -> Обновляем запись (проверка через FirstOrDefault по id)
            // в faculties есть, но нет в existFaculties -> Добавляем запись (проверка через facultiesForAdd)
            // в faculties нет, но есть в existFaculties -> Удаляем запись 

            var educationLevels = await _externalDictionaryService.GetEducationLevelsAsync();

            throw new NotImplementedException();
        }

        private async Task<ExecutionResult> UpdateEducationProgramAsync(bool deleteRelatedEntities = false)
        {
            // если добавляем, то проверяем правильность внешних ссылок, иначе ошибка, если прилетела сущность и она есть в бд, но мягко удалена, то восстанавливаем ее
            // если удаляем, то ничего не проверяем в других сущностях
            // если обновляем, то проверяем правильность внешних ссылок при их изменении, иначе ошибка

            var educationProgram = await _externalDictionaryService.GetEducationProgramAsync();

            throw new NotImplementedException();
        }

        private async Task<ExecutionResult> UpdateEducationDocumentTypeAsync(bool deleteRelatedEntities = false)
        {
            // если добавляем, то проверяем правильность внешних ссылок, иначе ошибка, если прилетела сущность и она есть в бд, но мягко удалена, то восстанавливаем ее
            // если удаляем,  то ничего не проверяем в других сущностях
            // если обновляем, то проверяем правильность внешних ссылок при их изменении, иначе ошибка

            var educationDocuments = await _externalDictionaryService.GetEducationDocumentTypesAsync();

            throw new NotImplementedException();
        }

        private async Task<ExecutionResult> UpdateAsync<TEntity, TExternalEntity, TRepository>(
            bool deleteRelatedEntities, TRepository repository,
            GetEntityAsync<TEntity> getEntityAsync,
            GetExternalEntityAsync<TExternalEntity> getExternalEntityAsync,
            OnUpdateEntity<TEntity, TExternalEntity> onUpdateEntity,
            OnAddEntity<TEntity, TExternalEntity> onAddEntity,
            OnDeleteEntityAsync<TEntity> onDeleteEntity
        ) 
            where TEntity : BaseDictionaryEntity
            where TExternalEntity : BaseExternalDTO
            where TRepository : IBaseRepository<TEntity>
        {
            ExecutionResult<List<TExternalEntity>> getExternalEntityResult = await getExternalEntityAsync();
            if (!getExternalEntityResult.IsSuccess) return getExternalEntityResult;

            List<TExternalEntity> externalEntities = getExternalEntityResult.Result!;
            List<TEntity> existEntities = await getEntityAsync();

            Dictionary<Guid, TEntity> existEntitiesForRemove = existEntities.ToDictionary(entity => entity.Id);
            foreach (var externalEntity in externalEntities)
            {
                TEntity? existEntity = existEntities.FirstOrDefault(existEntity => existEntity.Id == externalEntity.Id);
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

            bool thereAreRelated = false;
            List<string> comments = new();
            foreach (var entityForRemove in existEntitiesForRemove.Values)
            {
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

            await repository.SaveChangesAsync();

            return new(isSuccess: true);
        }

        private delegate Task<List<TEntity>> GetEntityAsync<TEntity>();
        private delegate Task<ExecutionResult<List<TExternalEntity>>> GetExternalEntityAsync<TExternalEntity>();
        private delegate void OnUpdateEntity<TEntity, TExternalEntity>(TEntity entity, TExternalEntity externalEntity);
        private delegate TEntity OnAddEntity<TEntity, TExternalEntity>(TExternalEntity externalEntity);
        private delegate Task<bool> OnDeleteEntityAsync<TEntity>(TEntity entity, List<string> comments);
    }
}