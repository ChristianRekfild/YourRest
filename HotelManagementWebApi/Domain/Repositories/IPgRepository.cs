using HotelManagementWebApi.Domain.Entities.Review;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagementWebApi.Domain.Repositories;

public interface IPgRepository <TEntity, TEntityIdType> where TEntity : class
{
    Task<IEnumerable<TEntity>> GetAllAsync();

    Task<TEntity> GetAsync(TEntityIdType id);

    Task<TEntity> AddAsync(TEntity entity);

    Task UpdateAsync(TEntity entity);

    Task SaveChangesAsync();

    Task DeleteAsync(TEntityIdType id);
}