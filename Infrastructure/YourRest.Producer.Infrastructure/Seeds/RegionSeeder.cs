using YourRest.Infrastructure.Core.DbContexts;
using YourRest.Domain.Entities;

namespace YourRest.Producer.Infrastructure.Seeds;

public class RegionSeeder
{
    private readonly SharedDbContext _context;

    public RegionSeeder(SharedDbContext context)
    {
        _context = context;
    }

    public void Seed()
    {
        if (!_context.Regions.Any()) 
        {
            _context.Regions.AddRange(
                new Region { Id = 1, CountryId = 1, Name = "Московская область" },
                new Region { Id = 2, CountryId = 1, Name = "Ленинградская область" },
                new Region { Id = 3, CountryId = 1, Name = "Свердловская область" },
                new Region { Id = 4, CountryId = 1, Name = "Краснодарский край" }
            );
            _context.SaveChanges();
        }
    }
}

