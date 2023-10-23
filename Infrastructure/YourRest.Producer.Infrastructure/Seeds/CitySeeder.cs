using YourRest.Infrastructure.Core.DbContexts;
using YourRest.Domain.Entities;

namespace YourRest.Producer.Infrastructure.Seeds;

public class CitySeeder
{
    private readonly SharedDbContext _context;

    public CitySeeder(SharedDbContext context)
    {
        _context = context;
    }

    public void Seed()
    {
        if (!_context.Cities.Any()) 
        {
            var moscowRegionId = _context.Regions
                .FirstOrDefault(c => c.Name == "Московская область")?.Id;
            
            var leningradRegionId = _context.Regions
                .FirstOrDefault(c => c.Name == "Ленинградская область")?.Id;
            
            var sverdlovskRegionId = _context.Regions
                .FirstOrDefault(c => c.Name == "Свердловская область")?.Id;
            
            var krasnodarRegionId = _context.Regions
                .FirstOrDefault(c => c.Name == "Краснодарский край")?.Id;
            
            _context.Cities.AddRange(
                new City { RegionId = moscowRegionId.Value, Name = "Москва" },
                new City { RegionId = moscowRegionId.Value, Name = "Зеленоград" },
                new City { RegionId = moscowRegionId.Value, Name = "Подольск" },
                
                new City { RegionId = leningradRegionId.Value, Name = "Всеволожск" },
                new City { RegionId = leningradRegionId.Value, Name = "Гатчина" },
                
                new City { RegionId = sverdlovskRegionId.Value, Name = "Екатеринбург" },
                new City { RegionId = sverdlovskRegionId.Value, Name = "Нижний Тагил" },
                
                new City { RegionId = krasnodarRegionId.Value, Name = "Краснодар" },
                new City { RegionId = krasnodarRegionId.Value, Name = "Сочи" },
                new City { RegionId = krasnodarRegionId.Value, Name = "Анапа" }
            );
            _context.SaveChanges();
        }
    }
}

