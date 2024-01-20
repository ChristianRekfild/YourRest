using YourRest.Producer.Infrastructure.DbContexts;
using YourRest.Producer.Infrastructure.Entities;

namespace YourRest.Producer.Infrastructure.Seeds;

public class RoomTypeSeeder
{
    private readonly SharedDbContext _context;

    public RoomTypeSeeder(SharedDbContext context)
    {
        _context = context;
    }

    public void Seed()
    {
        if (!_context.RoomTypes.Any()) 
        {
            _context.RoomTypes.AddRange(
                new RoomType { Name = "Одноместный" },
                new RoomType { Name = "Двухместный" },
                new RoomType { Name = "Двухместный с 2 кроватями" },
                new RoomType { Name = "Люкс" },
                new RoomType { Name = "Семейный" },
                new RoomType { Name = "Студия" },
                new RoomType { Name = "Апартаменты" },
                new RoomType { Name = "Бунгало" },
                new RoomType { Name = "Пентхаус" },
                new RoomType { Name = "Делюкс" }
            );
            _context.SaveChanges();
        }
    }
}

