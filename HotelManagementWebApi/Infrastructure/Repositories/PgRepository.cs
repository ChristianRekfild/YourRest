using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagementWebApi.Infrastructure.Repositories;

public class PgRepository<TEntity, U> where TEntity : class
{
    protected readonly DbContext _dataContext;

    public PgRepository(DbContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _dataContext.Set<TEntity>().ToListAsync();
    }

    public async Task<TEntity> GetAsync(U id)
    {
        return await _dataContext.Set<TEntity>().FindAsync(id);
    }

    public async Task AddAsync(TEntity entity)
    {
        await _dataContext.Set<TEntity>().AddAsync(entity);
        await _dataContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(TEntity entity)
    {
        _dataContext.Entry(entity).State = EntityState.Modified;
        await _dataContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(U id)
    {
        var entity = await GetAsync(id);
        if (entity != null)
        {
            _dataContext.Set<TEntity>().Remove(entity);
            await _dataContext.SaveChangesAsync();
        }
    }
}
