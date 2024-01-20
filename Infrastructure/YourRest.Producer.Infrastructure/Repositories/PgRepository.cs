using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using YourRest.Infrastructure.Core.Contracts.Entities;
using YourRest.Infrastructure.Core.Contracts.Models;
using YourRest.Infrastructure.Core.Contracts.Repositories;
using YourRest.Producer.Infrastructure.ExpressionHelper;

namespace YourRest.Producer.Infrastructure.Repositories
{
    public abstract class PgRepository<TEntity, U, TSourse> : IRepository<TSourse, U>
        where TEntity : BaseEntity<U>
        where U : notnull
        where TSourse : BaseEntityDto<U>
    {
        protected readonly DbContext _dataContext;
        protected readonly IMapper _mapper;

        public PgRepository(DbContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<TSourse> AddAsync(
            TSourse entity,
            bool saveChanges = true,
            CancellationToken cancellationToken = default)
        {

            var result = await _dataContext
                .Set<TEntity>()
                .AddAsync(_mapper.Map<TEntity>(entity), cancellationToken);

            if (saveChanges)
            {
                await _dataContext.SaveChangesAsync(cancellationToken);
            }
            return _mapper.Map<TSourse>(result.Entity);
        }

        public async Task AddRangeAsync(
            TSourse[] entites,
            bool saveChanges = true,
            CancellationToken cancellationToken = default)
        {
            await _dataContext
                .Set<TEntity>()
                .AddRangeAsync(_mapper.Map<IEnumerable<TEntity>>(entites), cancellationToken);

            if (saveChanges)
            {
                await _dataContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<TSourse>> FindAsync(
            Expression<Func<TSourse, bool>> expression,
            CancellationToken cancellationToken = default)
        {
            return _mapper.Map<IEnumerable<TSourse>>(
                await _dataContext
                .Set<TEntity>()
                .Where(expression.ToEntityExpression<TSourse, TEntity>())
                .ToListAsync(cancellationToken)
                );
        }

        public async Task<bool> FindAnyAsync(
            Expression<Func<TSourse, bool>> expression,
            CancellationToken cancellationToken = default)
        {
            return await _dataContext
                .Set<TEntity>()
                .Where(expression.ToEntityExpression<TSourse, TEntity>())
                .AnyAsync(cancellationToken);
        }

        public async Task<IEnumerable<TSourse>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return _mapper.Map<IEnumerable<TSourse>>(
                await _dataContext
                .Set<TEntity>()
                .ToListAsync(cancellationToken)
                );
        }

        public async Task<TSourse?> GetAsync(U id, CancellationToken cancellationToken = default)
        {
            return _mapper.Map<TSourse>(
                await _dataContext
                .Set<TEntity>()
                .FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken)
                );
        }

        public async Task<IEnumerable<TSourse>> GetWithIncludeAsync(
            Expression<Func<TSourse, bool>> predicate,
            CancellationToken cancellationToken = default,
            params Expression<Func<TSourse, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = _dataContext.Set<TEntity>().AsNoTracking();

            return _mapper.Map<IEnumerable<TSourse>>(
                await Include(query, includeProperties.ToEntityExpression<TSourse, TEntity>())
                .Where(predicate.ToEntityExpression<TSourse, TEntity>())
                .ToListAsync(cancellationToken));
        }

        public async Task<IEnumerable<TSourse>> GetWithIncludeAndTrackingAsync(
            Expression<Func<TSourse, bool>> predicate,
            CancellationToken cancellationToken = default,
            params Expression<Func<TSourse, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = _dataContext.Set<TEntity>().AsQueryable();

            return _mapper.Map<IEnumerable<TSourse>>(
                await Include(query, includeProperties.ToEntityExpression<TSourse, TEntity>())
                .Where(predicate.ToEntityExpression<TSourse, TEntity>())
                .ToListAsync(cancellationToken));
        }

        public async Task<IEnumerable<TSourse>> GetAllWithIncludeAsync(
            Expression<Func<TSourse, object>> includeProperty,
            CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = _dataContext.Set<TEntity>().AsNoTracking();

            return _mapper.Map<IEnumerable<TSourse>>(
                await Include(query, includeProperty.ToEntityExpression<TSourse, TEntity>())
                .ToListAsync(cancellationToken)
                );
        }

        public async Task DeleteAsync(
            U id,
            bool saveChanges = true,
            CancellationToken cancellationToken = default)
        {
            var entity = await _dataContext.Set<TSourse>().FindAsync(id, cancellationToken);

            if (entity != null)
            {
                _dataContext.Set<TSourse>().Remove(entity);

                if (saveChanges)
                {
                    await _dataContext.SaveChangesAsync(cancellationToken);
                }
            }
        }

        public async Task<TSourse> UpdateAsync(
            TSourse entity,
            bool saveChanges = true,
            CancellationToken cancellationToken = default)
        {
            var oldEntity = await _dataContext.Set<TSourse>().FindAsync(entity.Id, cancellationToken);

            if (oldEntity != null)
            {
                _dataContext.Entry(oldEntity).CurrentValues.SetValues(entity);

                if (saveChanges)
                {
                    await _dataContext.SaveChangesAsync(cancellationToken);
                }
            }

            return _mapper.Map<TSourse>(oldEntity);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _dataContext.SaveChangesAsync(cancellationToken);
        }
        private IQueryable<TEntity> Include(IQueryable<TEntity> query, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return includeProperties
                .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }
    }
}