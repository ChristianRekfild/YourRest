using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelManagementWebApi.Domain.Repositories;

namespace HotelManagementWebApi.Infrastructure.Repositories;

public class PgRepository<TEntity, TEntityIdType> : IPgRepository<TEntity, TEntityIdType> where TEntity : class
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

    public async Task<TEntity> GetAsync(TEntityIdType id)
    {
        return await _dataContext.Set<TEntity>().FindAsync(id);
    }

  public TEntity Add(TEntity entity)
    {
        var t = _dataContext.Set<TEntity>().Add(entity).Entity;
        return t;
    }

    public void Update(TEntity entity)
    {
        _dataContext.Entry(entity).State = EntityState.Modified;
    }

    public async Task SaveChangesAsync()
    {
        await _dataContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(TEntityIdType id)
    {
        var entity = await GetAsync(id);
        if (entity != null)
        {
            _dataContext.Set<TEntity>().Remove(entity);
            await _dataContext.SaveChangesAsync();
        }
    }
}
