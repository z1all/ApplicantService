using LoggerService.Core.Application.Interfaces;
using LoggerService.Core.Domain;
using LoggerService.Infrastructure.Persistence.Contexts;
using LoggerService.Core.Application.DTOs;
using Common.Repositories;

namespace LoggerService.Infrastructure.Persistence.Repositories
{
    public class LogRepository : BaseWithBaseEntityRepository<Log, AppDbContext>, ILogRepository
    {
        public LogRepository(AppDbContext dbContext) : base(dbContext) { }

        public Task<List<Log>> GetAllByFiler(LogFilterDTO filter)
        {
            throw new NotImplementedException();
        }
    }
}
