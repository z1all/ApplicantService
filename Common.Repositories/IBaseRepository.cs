namespace Common.Repositories
{
    public interface IBaseRepository
    {
        Task SaveChangesAsync();
    }

    public interface IBaseRepository<TEntity> : IBaseRepository where TEntity : BaseEntity
    {
        Task<TEntity?> GetByIdAsync(Guid id);
        Task<bool> AnyByIdAsync(Guid id);
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
    }
}
