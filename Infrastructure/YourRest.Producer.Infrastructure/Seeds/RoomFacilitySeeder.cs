using YourRest.Producer.Infrastructure.DbContexts;
using YourRest.Producer.Infrastructure.Entities;

namespace YourRest.Producer.Infrastructure.Seeds;

public class RoomFacilitySeeder
{
    private readonly SharedDbContext _context;

    public RoomFacilitySeeder(SharedDbContext context)
    {
        _context = context;
    }

    public void Seed()
    {
        if (!_context.RoomFacilities.Any())
        {
            _context.RoomFacilities.AddRange(
                new RoomFacility { Name = "Safe"},
                new RoomFacility { Name = "Mini Bar"},
                new RoomFacility { Name = "Jacuzzi" },
                new RoomFacility { Name = "Air Conditioning"},
                new RoomFacility { Name = "Free Wi-Fi"},
                new RoomFacility { Name = "Flat-Screen TV"},
                new RoomFacility { Name = "Room Service" },
                new RoomFacility { Name = "Balcony" },
                new RoomFacility { Name = "Coffee Maker" },
                new RoomFacility { Name = "Work Desk"}
            );
            _context.SaveChanges();
        }
    }
}