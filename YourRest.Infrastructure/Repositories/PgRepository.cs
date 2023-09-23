﻿using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Repositories;
using System.Linq.Expressions;

namespace YourRest.Infrastructure.Repositories
{
    public class PgRepository<T, U> : IRepository<T, U> where T : BaseEntity<U> where U : notnull
    {
        protected readonly DbContext _dataContext;

        public PgRepository(DbContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddAsync(T entity, bool saveChanges = true, CancellationToken cancellationToken = default)
        {
            await _dataContext.Set<T>().AddAsync(entity, cancellationToken);

            if (saveChanges)
            {
                await _dataContext.SaveChangesAsync(cancellationToken);
            }
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

        public async Task UpdateAsync(T entity, bool saveChanges = true, CancellationToken cancellationToken = default)
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
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _dataContext.SaveChangesAsync(cancellationToken);
        }
    }
}