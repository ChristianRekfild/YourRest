using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using YourRestDomain.Entities;

namespace YourRestDataAccesLayer.DbContexts
{
    public class SharedDbContext : DbContext
    {
        private readonly IConfiguration configuration;
        public SharedDbContext(DbContextOptions<SharedDbContext> options) : base(options)
        {
        }

        public DbSet<AdditionalRoomServiceEntity> AdditionalRoomServices { get; set; }
        public DbSet<BookingEntity> Bookings { get; set; }
        public DbSet<CityEntity> Cities { get; set; }
        public DbSet<CountryEntity> Countries { get; set; }
        public DbSet<CustomerEntity> Customers { get; set; }
        public DbSet<RegionEntity> Regions { get; set; }
        public DbSet<RoomEntity> Rooms { get; set; }

        public void ClearAllTables()
        {

            AdditionalRoomServices.RemoveRange(AdditionalRoomServices);
            Bookings.RemoveRange(Bookings);
            Cities.RemoveRange(Cities);
            Countries.RemoveRange(Countries);
            Customers.RemoveRange(Customers);
            Regions.RemoveRange(Regions);
            Rooms.RemoveRange(Rooms);
            
            // Add other DbSet removals here
            // Example: 
            // Rooms.RemoveRange(Rooms);
            // Hotels.RemoveRange(Hotels);
            // ... and so on

            SaveChanges();
        }
    }
    
}