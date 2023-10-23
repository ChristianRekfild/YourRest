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
            _context.Cities.AddRange(
                new City { Id = 1, RegionId = 1, Name = "Москва" },
                new City { Id = 2, RegionId = 1, Name = "Зеленоград" },
                new City { Id = 3, RegionId = 1, Name = "Подольск" },
                
                new City { Id = 4, RegionId = 2, Name = "Санкт-Петербург" },
                new City { Id = 5, RegionId = 2, Name = "Гатчина" },
                
                new City { Id = 6, RegionId = 3, Name = "Екатеринбург" },
                new City { Id = 7, RegionId = 3, Name = "Нижний Тагил" },
                
                new City { Id = 8, RegionId = 4, Name = "Краснодар" },
                new City { Id = 9, RegionId = 4, Name = "Сочи" },
                new City { Id = 10, RegionId = 4, Name = "Анапа" }
            );
            _context.SaveChanges();
        }
    }
}

