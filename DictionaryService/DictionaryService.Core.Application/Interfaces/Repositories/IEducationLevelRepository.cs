﻿using DictionaryService.Core.Domain;
using Common.Repositories;

namespace DictionaryService.Core.Application.Interfaces.Repositories
{
    public interface IEducationLevelRepository : IBaseRepository<EducationLevel> 
    {
        Task<List<EducationLevel>> GetAllAsync();
        EducationLevel GetByExternalId(int externalId);
        Task<bool> AnyByExternalIdAsync(int externalId);
    }
}