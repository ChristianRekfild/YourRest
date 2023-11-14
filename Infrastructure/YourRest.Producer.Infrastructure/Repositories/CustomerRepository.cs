using YourRest.Domain.Entities;
using YourRest.Domain.Repositories;
using YourRest.Infrastructure.Core.DbContexts;
using YourRest.Infrastructure.Core.Repositories;

namespace YourRest.Producer.Infrastructure.Repositories
{
    public class CustomerRepository : PgRepository<Customer, int>, ICustomerRepository
    {
        public CustomerRepository(SharedDbContext dbContext) : base(dbContext) { }
    }
}
