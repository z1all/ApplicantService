﻿using ApplicantService.Core.Domain;
using Common.Repositories;

namespace ApplicantService.Core.Application.Interfaces.Repositories
{
    public interface IDocumentRepository : IBaseRepository<Document> 
    {
        Task<Document?> GetByDocumentIdAndApplicantIdAsync(Guid documentId, Guid applicantId);
        Task<List<Document>> GetAllByApplicantIdAsync(Guid applicantId);
    }
}