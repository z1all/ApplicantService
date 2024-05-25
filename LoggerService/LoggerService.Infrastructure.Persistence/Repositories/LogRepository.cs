using Microsoft.EntityFrameworkCore;
using LoggerService.Core.Application.Interfaces;
using LoggerService.Core.Domain;
using LoggerService.Infrastructure.Persistence.Contexts;
using LoggerService.Core.Application.DTOs;
using LoggerService.Core.Application.Enums;
using Common.Repositories;

namespace LoggerService.Infrastructure.Persistence.Repositories
{
    public class LogRepository : BaseWithBaseEntityRepository<Log, AppDbContext>, ILogRepository
    {
        public LogRepository(AppDbContext dbContext) : base(dbContext) { }

        public async Task<List<Log>> GetAllByFiler(LogFilterDTO filter)
        {
            var query = _dbContext.Logs.AsQueryable();

            if (filter.ServiceName is not null) query = query.Where(log => log.ServiceName!.ToLower().Contains(filter.ServiceName!.ToLower()));
            if (filter.LoggerName is not null) query = query.Where(log => log.LoggerName!.ToLower().Contains(filter.LoggerName!.ToLower()));
            if (filter.LogLevel is not null) query = query.Where(log => log.LogLevel == filter.LogLevel);

            query = filter.SortType switch
            {
                LogSortType.DateTimeAsc => query.OrderBy(log => log.LogDateTime),
                LogSortType.DateTimeDesc => query = query.OrderByDescending(log => log.LogDateTime),
                _ => query
            };

            return await query
                .Skip((filter.Page - 1) * filter.Size)
                .Take(filter.Size)
                .ToListAsync();
        }

        public async Task<int> GetCountByFiler(LogFilterDTO filter)
        {
            var query = _dbContext.Logs.AsQueryable();

            if (filter.ServiceName is not null) query = query.Where(log => log.ServiceName!.ToLower().Contains(filter.ServiceName!.ToLower()));
            if (filter.LoggerName is not null) query = query.Where(log => log.LoggerName!.ToLower().Contains(filter.LoggerName!.ToLower()));
            if (filter.LogLevel is not null) query = query.Where(log => log.LogLevel == filter.LogLevel);

            query = filter.SortType switch
            {
                LogSortType.DateTimeAsc => query.OrderBy(log => log.LogDateTime),
                LogSortType.DateTimeDesc => query = query.OrderByDescending(log => log.LogDateTime),
                _ => query
            };

            return await query.CountAsync();
        }
    }
}
