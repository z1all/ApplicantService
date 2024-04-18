﻿using ApplicantService.Core.Application.Interfaces.Repositories;
using ApplicantService.Core.Domain;
using ApplicantService.Infrastructure.Persistence.Contexts;
using Common.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ApplicantService.Infrastructure.Persistence.Repositories
{
    public class EducationDocumentTypeCacheRepository : BaseRepository<EducationDocumentTypeCache, AppDbContext>, IEducationDocumentTypeCacheRepository
    {
        public EducationDocumentTypeCacheRepository(AppDbContext dbContext) : base(dbContext) { }

        public async Task<bool> AnyByIdAsync(Guid id, bool getDeprecated)
        {
            return await _dbContext.EducationDocumentTypesCache
                .AnyAsync(documentType => documentType.Id == id && (getDeprecated ? true : !documentType.Deprecated));
        }
    }
}
