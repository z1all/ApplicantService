namespace DictionaryService.Core.Application.Interfaces.Transaction
{
    public interface ITransactionProvider
    {
        Task<ITransaction> CreateTransactionScopeAsync();
    }
}
