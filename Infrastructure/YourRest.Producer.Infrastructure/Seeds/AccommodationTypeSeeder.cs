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
                new AccommodationType { Id = 1, Name = "Гостиница" },
                new AccommodationType { Id = 2, Name = "Апартаменты" },
                new AccommodationType { Id = 3, Name = "Хостел" },
                new AccommodationType { Id = 4, Name = "Бутик-отель" },
                new AccommodationType { Id = 5, Name = "Мотель" },
                new AccommodationType { Id = 6, Name = "Пансионат" },
                new AccommodationType { Id = 7, Name = "Курортный отель" },
                new AccommodationType { Id = 8, Name = "Бед & Брекфаст" },
                new AccommodationType { Id = 9, Name = "Вилла" },
                new AccommodationType { Id = 10, Name = "Кемпинг" }
            );
            _context.SaveChanges();
        }
    }
}

