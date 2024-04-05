using ApplicantService.Core.Application.Interfaces.Repositories;
using ApplicantService.Infrastructure.Persistence.Contexts;
using Common.Models;

namespace ApplicantService.Infrastructure.Persistence.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly AppDbContext _dbContext;

        public FileRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<ExecutionResult> DeleteAllFromDocumentAsync(Guid documentId)
        {
            // Удаляем файлы, а потом инфу о них
            throw new NotImplementedException();
        }
    }
}
