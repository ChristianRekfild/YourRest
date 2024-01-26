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
            
            var cities = new List<City>();

            if (moscowRegionId.HasValue)
            {
                cities.AddRange(new[]
                {
                    new City { RegionId = moscowRegionId.Value, Name = "Москва", Description = "Столица России, крупнейший культурный и экономический центр.", IsFavorite = true },
                    new City { RegionId = moscowRegionId.Value, Name = "Зеленоград", Description = "Административный округ Москвы, известный как научный центр.", IsFavorite = false },
                    new City { RegionId = moscowRegionId.Value, Name = "Подольск", Description = "Город в Московской области, с богатой историей и промышленностью.", IsFavorite = false }
                });
            }

            if (leningradRegionId.HasValue)
            {
                cities.AddRange(new[]
                {
                    new City { RegionId = leningradRegionId.Value, Name = "Всеволожск", Description = "Город в Ленинградской области, известный своими историческими достопримечательностями.", IsFavorite = false },
                    new City { RegionId = leningradRegionId.Value, Name = "Гатчина", Description = "Известен своим дворцово-парковым ансамблем и богатой историей.", IsFavorite = true }
                });
            }

            if (sverdlovskRegionId.HasValue)
            {
                cities.AddRange(new[]
                {
                    new City { RegionId = sverdlovskRegionId.Value, Name = "Екатеринбург", Description = "Четвёртый по величине город России, культурная столица Урала.", IsFavorite = true },
                    new City { RegionId = sverdlovskRegionId.Value, Name = "Нижний Тагил", Description = "Один из крупнейших промышленных центров Уральского региона.", IsFavorite = false }
                });
            }

            if (krasnodarRegionId.HasValue)
            {
                cities.AddRange(new[]
                {
                    new City { RegionId = krasnodarRegionId.Value, Name = "Краснодар", Description = "Административный центр Краснодарского края, ключевой экономический центр юга России.", IsFavorite = false },
                    new City { RegionId = krasnodarRegionId.Value, Name = "Сочи", Description = "Курортный город на побережье Черного моря, известен своими пляжами и горнолыжными трассами.", IsFavorite = true },
                    new City { RegionId = krasnodarRegionId.Value, Name = "Анапа", Description = "Популярный курортный город на побережье Черного моря.", IsFavorite = true }
                });
            }

            if (cities.Any())
            {
                _context.Cities.AddRange(cities);
                _context.SaveChanges();
            }
        }
    }
}
