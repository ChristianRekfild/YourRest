using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using YourRestDomain.Abstractions;

namespace YourRestDataAccesLayer.Repositories
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IBaseEntity<int>
    {
        private readonly DbContext context;
        public Repository(DbContext context) => this.context = context;
        
        public virtual async Task CreateAsync(TEntity entity)
                => await context.Set<TEntity>().AddAsync(entity);
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
                => await context.Set<TEntity>().AsNoTracking().ToListAsync();
        public virtual async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate)
                => await context.Set<TEntity>().AsNoTracking().SingleOrDefaultAsync(predicate);
        public virtual async Task<IEnumerable<TEntity>> GetWithIncludeAsync(params Expression<Func<TEntity, object>>[] includeProperties)
                => await Include(includeProperties).ToListAsync();
        public async Task<IEnumerable<TEntity>> GetWithIncludeAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
            => await Include(includeProperties).Where(predicate).ToListAsync();
        public virtual Task RemoveAsync(TEntity entity)
        {
            context.Set<TEntity>().Remove(entity);
            return Task.CompletedTask;
        }
        public virtual async Task UpdateAsync(TEntity entity)
        {
            TEntity foundEntity = await context.Set<TEntity>().FindAsync(entity.Id);
            if(foundEntity != null) 
            {
                context.Entry(foundEntity).CurrentValues.SetValues(entity);
            }
        }
        public async Task SaveAsync() => await context.SaveChangesAsync();

        private IQueryable<TEntity> Include(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = context.Set<TEntity>().AsNoTracking();
            return includeProperties
                .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }
    }
}

