using System.Linq.Expressions;
using YourRest.Infrastructure.Core.Contracts.Models;

namespace YourRest.Infrastructure.Core.Contracts.Repositories
{
    public interface IRepository<T, U> where T : BaseEntityDto<U> where U : notnull
    {
        Task<T> AddAsync(T entity, bool saveChanges = true, CancellationToken cancellationToken = default);

        Task AddRangeAsync(T[] entites, bool saveChanges = true, CancellationToken cancellationToken = default);

        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default);
    
        Task<bool> FindAnyAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default);

        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<IEnumerable<T>> GetAllWithIncludeAsync(Expression<Func<T, object>> includeProperty,
            CancellationToken cancellationToken = default);

        Task<T?> GetAsync(U id, CancellationToken cancellationToken = default);

        Task<T> UpdateAsync(T entity, bool saveChanges = true, CancellationToken cancellationToken = default);

        Task DeleteAsync(U id, bool saveChanges = true, CancellationToken cancellationToken = default);

        Task SaveChangesAsync(CancellationToken cancellationToken = default);

        Task<IEnumerable<T>> GetWithIncludeAsync(Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includeProperties);

        Task<IEnumerable<T>> GetWithIncludeAndTrackingAsync(Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includeProperties);
    }
}