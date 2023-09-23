using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Repositories;
using YourRest.Infrastructure.DbContexts;

namespace YourRest.Infrastructure.Repositories
{
    public class CustomerRepository : PgRepository<Customer, int>, ICustomerRepository
    {
        public CustomerRepository(SharedDbContext dbContext) : base(dbContext) { }
    }
}
