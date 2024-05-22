using Microsoft.EntityFrameworkCore;
using ApplicantService.Core.Application.Interfaces.Repositories;
using ApplicantService.Core.Domain;
using ApplicantService.Infrastructure.Persistence.Contexts;
using Common.Repositories;

namespace ApplicantService.Infrastructure.Persistence.Repositories
{
    public class DocumentFileInfoRepository : BaseWithBaseEntityRepository<DocumentFileInfo, AppDbContext>, IDocumentFileInfoRepository
    {
        public DocumentFileInfoRepository(AppDbContext dbContext) : base(dbContext) { }

        public async Task<List<DocumentFileInfo>> GetAllByApplicantIdAndDocumentId(Guid applicantId, Guid documentId)
        {
            return await _dbContext.FilesInfo
                .Where(document => document.DocumentId == documentId && document.Document!.ApplicantId == applicantId)
                .ToListAsync();
        }
    }
}
