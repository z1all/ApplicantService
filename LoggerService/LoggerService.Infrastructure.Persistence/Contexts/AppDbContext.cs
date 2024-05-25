using Microsoft.EntityFrameworkCore;
using LoggerService.Core.Domain;

namespace LoggerService.Infrastructure.Persistence.Contexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> dbContext) : base(dbContext) { }

        public DbSet<Log> Logs { get; private set; }
    }
}
