using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SharedKernel.Domain.Entities;

namespace YourRest.Infrastructure.DbContexts
{
    public class SharedDbContext : DbContext
    {
        private readonly IConfiguration configuration;

        public DbSet<Country> Countries { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<City> Cities { get; set; }

        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Region> Regions { get; set; }




        public SharedDbContext(DbContextOptions<SharedDbContext> options) : base(options)
        {
        }

        public void ClearAllTables()
        {
            Countries.RemoveRange(Countries);
            Bookings.RemoveRange(Bookings);
            Customers.RemoveRange(Customers);
            Cities.RemoveRange(Cities);
            Regions.RemoveRange(Regions);

            // Add other DbSet removals here
            // Example: 
            // Rooms.RemoveRange(Rooms);
            // Hotels.RemoveRange(Hotels);
            // ... and so on

            SaveChanges();
        }
    }
    
}