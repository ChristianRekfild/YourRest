using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain.Repositories;
using SharedKernel.Domain.Entities;
using YourRest.Infrastructure.DbContexts;

namespace YourRest.Infrastructure.Repositories
{
    public class PgRepository<T, U> : IRepository<T, U> where T : BaseEntity<U> where U : notnull
    {
        protected readonly SharedDbContext _dataContext;

        public PgRepository(SharedDbContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dataContext.Set<T>().ToListAsync();
        }

        public async Task<T?> GetAsync(U id)
        {
            return await _dataContext.Set<T>().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task AddAsync(T entity)
        {
            await _dataContext.Set<T>().AddAsync(entity);
            await _dataContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            var oldEntity = await _dataContext.Set<T>().FindAsync(entity.Id);
            if (oldEntity != null)
            {
                _dataContext.Entry(oldEntity).CurrentValues.SetValues(entity);
                await _dataContext.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(U id)
        {
            var entity = await _dataContext.Set<T>().FindAsync(id);
            if (entity != null)
            {
                _dataContext.Set<T>().Remove(entity);
                await _dataContext.SaveChangesAsync();
            }
        }
    }
}