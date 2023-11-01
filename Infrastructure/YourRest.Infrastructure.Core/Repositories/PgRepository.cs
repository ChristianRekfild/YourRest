using Microsoft.EntityFrameworkCore;
using YourRest.Domain.Entities;
using YourRest.Domain.Repositories;
using System.Linq.Expressions;

namespace YourRest.Infrastructure.Core.Repositories
{
    public class PgRepository<T, U> : IRepository<T, U> where T : BaseEntity<U> where U : notnull
    {
        protected readonly DbContext _dataContext;

        public PgRepository(DbContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<T> AddAsync(T entity, bool saveChanges = true, CancellationToken cancellationToken = default)
        {
            var result = await _dataContext.Set<T>().AddAsync(entity, cancellationToken);

            if (saveChanges)
            {
                await _dataContext.SaveChangesAsync(cancellationToken);
            }
            return result.Entity;
        }

        public async Task AddRangeAsync(T[] entites, bool saveChanges = true, CancellationToken cancellationToken = default)
        {
            await _dataContext.Set<T>().AddRangeAsync(entites, cancellationToken);

            if (saveChanges)
            {
                await _dataContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default)
        {
            return await _dataContext.Set<T>().Where(expression).ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dataContext.Set<T>().ToListAsync(cancellationToken);
        }

        public async Task<T?> GetAsync(U id, CancellationToken cancellationToken = default)
        {
            return await _dataContext.Set<T>().FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
        }

        public async Task<IEnumerable<T>> GetWithIncludeAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includeProperties)
        {
            return await Include(includeProperties).Where(predicate).ToListAsync(cancellationToken);
        }
       
       public async Task<IEnumerable<T>> GetAllWithIncludeAsync(Expression<Func<T, object>> includeProperty, CancellationToken cancellationToken = default)
       {
           return await Include(includeProperty).ToListAsync(cancellationToken);
       }

        
        public async Task DeleteAsync(U id, bool saveChanges = true, CancellationToken cancellationToken = default)
        {
            var entity = await _dataContext.Set<T>().FindAsync(id, cancellationToken);

            if (entity != null)
            {
                _dataContext.Set<T>().Remove(entity);

                if (saveChanges)
                {
                    await _dataContext.SaveChangesAsync(cancellationToken);
                }
            }
        }

        public async Task<T> UpdateAsync(T entity, bool saveChanges = true, CancellationToken cancellationToken = default)
        {
            var oldEntity = await _dataContext.Set<T>().FindAsync(entity.Id, cancellationToken);

            if (oldEntity != null)
            {
                _dataContext.Entry(oldEntity).CurrentValues.SetValues(entity);

                if (saveChanges)
                {
                    await _dataContext.SaveChangesAsync(cancellationToken);
                }
            }

            return oldEntity;
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _dataContext.SaveChangesAsync(cancellationToken);
        }

        private IQueryable<T> Include(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _dataContext.Set<T>().AsNoTracking();
            return includeProperties
                .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }
    }
}