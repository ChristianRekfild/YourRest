using YourRest.Infrastructure.Core.DbContexts;
using YourRest.Domain.Entities;

namespace YourRest.Producer.Infrastructure.Seeds;

public class AccommodationTypeSeeder
{
    private readonly SharedDbContext _context;

    public AccommodationTypeSeeder(SharedDbContext context)
    {
        _context = context;
    }
    public void Seed()
    {
        if (!_context.AccommodationTypes.Any()) 
        {
            _context.AccommodationTypes.AddRange(
                new AccommodationType { Name = "Гостиница" },
                new AccommodationType { Name = "Апартаменты" },
                new AccommodationType { Name = "Хостел" },
                new AccommodationType { Name = "Бутик-отель" },
                new AccommodationType { Name = "Мотель" },
                new AccommodationType { Name = "Пансионат" },
                new AccommodationType { Name = "Курортный отель" },
                new AccommodationType { Name = "Бед & Брекфаст" },
                new AccommodationType { Name = "Вилла" },
                new AccommodationType { Name = "Кемпинг" }
            );
            _context.SaveChanges();
        }
    }
}

