using SharedKernel.Domain.Entities;

namespace SharedKernel.Domain.Repositories
{
    public interface IRepository<T, U> where T : BaseEntity<U> where U : notnull
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetAsync(U id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(U id);
    }
}