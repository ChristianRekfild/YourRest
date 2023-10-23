using YourRest.Infrastructure.Core.DbContexts;
using YourRest.Domain.Entities;

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
                new RoomType { Id = 1, Name = "Одноместный" },
                new RoomType { Id = 2, Name = "Двухместный" },
                new RoomType { Id = 3, Name = "Двухместный с 2 кроватями" },
                new RoomType { Id = 4, Name = "Люкс" },
                new RoomType { Id = 5, Name = "Семейный" },
                new RoomType { Id = 6, Name = "Студия" },
                new RoomType { Id = 7, Name = "Апартаменты" },
                new RoomType { Id = 8, Name = "Бунгало" },
                new RoomType { Id = 9, Name = "Пентхаус" },
                new RoomType { Id = 10, Name = "Делюкс" }
            );
            _context.SaveChanges();
        }
    }
}

