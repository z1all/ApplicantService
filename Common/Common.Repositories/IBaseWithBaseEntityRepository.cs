namespace Common.Repositories
{
    public interface IBaseRepository<TEntity>
         where TEntity : class
    {
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task SaveChangesAsync();
    }

    public interface IBaseWithBaseEntityRepository<TEntity> : 
        IBaseRepository<TEntity> 
        where TEntity : BaseEntity
    {
        Task<TEntity?> GetByIdAsync(Guid id);
        Task<bool> AnyByIdAsync(Guid id);
    }
}
