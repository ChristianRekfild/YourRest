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
            var russiaCountryId = _context.Countries
                .FirstOrDefault(c => c.Name == "Россия")?.Id;

            if (russiaCountryId.HasValue)
            {
                _context.Regions.AddRange(
                    new Region { CountryId = russiaCountryId.Value, Name = "Московская область" },
                    new Region { CountryId = russiaCountryId.Value, Name = "Ленинградская область" },
                    new Region { CountryId = russiaCountryId.Value, Name = "Свердловская область" },
                    new Region { CountryId = russiaCountryId.Value, Name = "Краснодарский край" }
                );
                _context.SaveChanges();
            }
        }
    }
}

