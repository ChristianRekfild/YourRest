using Microsoft.EntityFrameworkCore;
using System.Reflection;
using YourRest.DAL.Contracts;

namespace YourRest.DAL.Postgre
{
    public class PgRepository<T, U> : IRepository<T, U> where T : BaseEntity<U> where U : notnull
    {
        private readonly DbContext _dataContext;

        public PgRepository(DbContext dataContext)
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
            var entity2 = await _dataContext.Set<T>().FirstOrDefaultAsync(x => x.Id.Equals(entity.Id));
            if (entity2 != null)
            {
                foreach (PropertyInfo propInfo in typeof(T).GetProperties())
                {
                    propInfo.SetValue(entity2, propInfo.GetValue(entity));
                }
                await _dataContext.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(U id)
        {
            var entity = await _dataContext.Set<T>().FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (entity != null)
            {
                _dataContext.Set<T>().Remove(entity);
                await _dataContext.SaveChangesAsync();
            }
        }
    }
}
