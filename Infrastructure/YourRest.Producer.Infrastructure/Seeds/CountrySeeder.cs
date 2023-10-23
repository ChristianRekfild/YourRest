using YourRest.Infrastructure.Core.DbContexts;
using YourRest.Domain.Entities;

namespace YourRest.Producer.Infrastructure.Seeds;

public class CountrySeeder
{
    private readonly SharedDbContext _context;

    public CountrySeeder(SharedDbContext context)
    {
        _context = context;
    }

    public void Seed()
    {
        if (!_context.Countries.Any()) 
        {
            _context.Countries.AddRange(
                new Country { Id = 1, Name = "Россия" }
            );
            _context.SaveChanges();
        }
    }
}

