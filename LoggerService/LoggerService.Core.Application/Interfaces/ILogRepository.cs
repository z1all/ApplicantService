using LoggerService.Core.Domain;
using LoggerService.Core.Application.DTOs;
using Common.Repositories;

namespace LoggerService.Core.Application.Interfaces
{
    public interface ILogRepository : IBaseWithBaseEntityRepository<Log>
    {
        Task<List<Log>> GetAllByFiler(LogFilterDTO filter);
        Task<int> GetCountByFiler(LogFilterDTO filter);
    }
}
