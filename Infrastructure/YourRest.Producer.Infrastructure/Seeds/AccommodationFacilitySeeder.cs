using YourRest.Producer.Infrastructure.DbContexts;
using YourRest.Producer.Infrastructure.Entities;

namespace YourRest.Producer.Infrastructure.Seeds;

public class AccommodationFacilitySeeder
{
    private readonly SharedDbContext _context;

    public AccommodationFacilitySeeder(SharedDbContext context)
    {
        _context = context;
    }

    public void Seed()
    {
        if (!_context.AccommodationFacility.Any())
        {
            _context.AccommodationFacility.AddRange(
                new AccommodationFacility { Name = "Swimming Pool", Description = "Outdoor and indoor pools with lounge areas" },
                new AccommodationFacility { Name = "Fish Restaurant", Description = "Specializes in seafood and offers waterfront dining" },
                new AccommodationFacility { Name = "Tennis Court", Description = "Professional-grade courts with equipment rental available" },
                new AccommodationFacility { Name = "Spa Center", Description = "Full-service spa offering massages and treatments" },
                new AccommodationFacility { Name = "Fitness Center", Description = "Well-equipped gym with personal trainers" },
                new AccommodationFacility { Name = "Garden Area", Description = "Beautifully landscaped gardens for relaxation" },
                new AccommodationFacility { Name = "Conference Rooms", Description = "Facilities for business meetings and events" },
                new AccommodationFacility { Name = "Playground", Description = "Safe and fun area for children" },
                new AccommodationFacility { Name = "Private Beach Access", Description = "Direct access to a private beach area" },
                new AccommodationFacility { Name = "Rooftop Terrace", Description = "Terrace offering panoramic views" }
            );
            _context.SaveChanges();
        }
    }
}