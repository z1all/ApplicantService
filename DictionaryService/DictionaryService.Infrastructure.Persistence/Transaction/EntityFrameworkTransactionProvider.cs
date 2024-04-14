using Microsoft.EntityFrameworkCore.Storage;
using DictionaryService.Core.Application.Interfaces.Transaction;
using DictionaryService.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using DictionaryService.Core.Domain;

namespace DictionaryService.Infrastructure.Persistence.Transaction
{
    public class EntityFrameworkTransactionProvider : ITransactionProvider
    {
        private readonly AppDbContext _dbContext;

        public EntityFrameworkTransactionProvider(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ITransaction> CreateTransactionScopeAsync()
        {
            IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();

            return new EntityFrameworkTransaction(transaction);
        }
    }
}
