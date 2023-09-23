using System.Linq.Expressions;

namespace HotelManagementWebApi.Domain.Repositories;

public interface IPgRepository <TEntity, TEntityIdType> where TEntity : class
{
    Task<IEnumerable<TEntity>> GetAllAsync();

    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> expression);

    Task<TEntity> GetAsync(TEntityIdType id);

    TEntity Add(TEntity entity);

    void Update(TEntity entity);

    Task SaveChangesAsync();

    Task DeleteAsync(TEntityIdType id);
}