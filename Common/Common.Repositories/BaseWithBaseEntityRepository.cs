﻿using Microsoft.EntityFrameworkCore;

namespace Common.Repositories
{
    public class BaseRepository<TEntity, TDbContext> : 
        IBaseRepository<TEntity>
        where TEntity : class
        where TDbContext : DbContext
    {
        protected readonly TDbContext _dbContext;

        public BaseRepository(TDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual async Task AddAsync(TEntity entity)
        {
            await _dbContext.Set<TEntity>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }

    public class BaseWithBaseEntityRepository<TEntity, TDbContext> : 
        BaseRepository<TEntity, TDbContext>, IBaseWithBaseEntityRepository<TEntity> 
        where TEntity : BaseEntity 
        where TDbContext : DbContext
    {
        public BaseWithBaseEntityRepository(TDbContext dbContext) : base(dbContext) { }

        public virtual async Task<TEntity?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Set<TEntity>()
               .FirstOrDefaultAsync(entity => entity.Id == id);
        }

        public virtual async Task<bool> AnyByIdAsync(Guid id)
        {
            return await _dbContext.Set<TEntity>()
               .AsNoTracking()
               .AnyAsync(entity => entity.Id == id);
        }
    }
}
