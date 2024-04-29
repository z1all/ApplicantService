using ApplicantService.Core.Application.Interfaces.Repositories;
using ApplicantService.Core.Domain;
using ApplicantService.Infrastructure.Persistence.Contexts;
using Common.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ApplicantService.Infrastructure.Persistence.Repositories
{
    public class DocumentRepository : BaseWithBaseEntityRepository<Document, AppDbContext>, IDocumentRepository
    {
        public DocumentRepository(AppDbContext dbContext) : base(dbContext) { }

        public async Task<Document?> GetByDocumentIdAndApplicantIdAsync(Guid documentId, Guid applicantId)
        {
            return await _dbContext.Documents
                .FirstOrDefaultAsync(document => document.Id == documentId && document.ApplicantId == applicantId);
        }

        public async Task<bool> AnyByDocumentIdAndApplicantIdAsync(Guid documentId, Guid applicantId)
        {
            return await _dbContext.Documents
                .AnyAsync(document => document.Id == documentId && document.ApplicantId == applicantId);
        }

        public async Task<List<Document>> GetAllByApplicantIdAsync(Guid applicantId)
        {
            return await _dbContext.Documents
                .Where(document => document.ApplicantId == applicantId)
                .ToListAsync();
        }
    }
}
