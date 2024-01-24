using YourRest.Producer.Infrastructure.DbContexts;
using YourRest.Producer.Infrastructure.Entities;

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
                new Country { Name = "Россия" }
            );
            _context.SaveChanges();
        }
    }
}

