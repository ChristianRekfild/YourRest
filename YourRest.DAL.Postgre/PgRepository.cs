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
            var oldEntity = await _dataContext.Set<T>().FirstOrDefaultAsync(x => x.Id.Equals(entity.Id));
            if (oldEntity != null)
            {
                foreach (PropertyInfo propInfo in typeof(T).GetProperties())
                {
                    var newFieldValue = propInfo.GetValue(entity);
                    if(newFieldValue != propInfo.GetValue(oldEntity))
                    propInfo.SetValue(oldEntity, newFieldValue);
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
