using DictionaryService.Core.Application.Interfaces.Transaction;
using Microsoft.EntityFrameworkCore.Storage;

namespace DictionaryService.Infrastructure.Persistence.Transaction
{
    public class EntityFrameworkTransaction : ITransaction
    {
        IDbContextTransaction _dbContextTransaction;

        public EntityFrameworkTransaction(IDbContextTransaction dbContextTransaction)
        {
            _dbContextTransaction = dbContextTransaction;
        }

        public async Task CommitAsync()
        {
            await _dbContextTransaction.CommitAsync();
        }

        public async Task RollbackAsync()
        {
            await _dbContextTransaction.RollbackAsync();
        }

        public void Dispose()
        {
            _dbContextTransaction.Dispose();
        }
    }
}
