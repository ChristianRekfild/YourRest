using YourRest.Infrastructure.Core.Contracts.Models;

namespace YourRest.Infrastructure.Core.Contracts.Repositories
{
    public interface ICustomerRepository : IRepository<CustomerDto, int>
    {
    }
}
