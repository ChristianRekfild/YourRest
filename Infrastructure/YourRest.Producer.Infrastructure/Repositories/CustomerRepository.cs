using AutoMapper;
using YourRest.Infrastructure.Core.Contracts.Models;
using YourRest.Infrastructure.Core.Contracts.Repositories;
using YourRest.Producer.Infrastructure.DbContexts;
using YourRest.Producer.Infrastructure.Entities;

namespace YourRest.Producer.Infrastructure.Repositories
{
    public class CustomerRepository : PgRepository<Customer, int, CustomerDto>, ICustomerRepository
    {
        public CustomerRepository(SharedDbContext dbContext, IMapper mapper) : base(dbContext, mapper) { }
    }
}
