namespace DictionaryService.Core.Application.Interfaces.Transaction
{
    public interface ITransaction : IDisposable
    {
        Task CommitAsync();
        Task RollbackAsync();
    }
}
