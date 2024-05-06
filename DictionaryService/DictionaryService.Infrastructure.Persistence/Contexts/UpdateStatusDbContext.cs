using DictionaryService.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace DictionaryService.Infrastructure.Persistence.Contexts
{
    public class UpdateStatusDbContext : DbContext
    {
        public UpdateStatusDbContext(DbContextOptions<UpdateStatusDbContext> dbContext) : base(dbContext) { }

        public DbSet<UpdateStatus> UpdateStatuses { get; private set; }
    }
}
