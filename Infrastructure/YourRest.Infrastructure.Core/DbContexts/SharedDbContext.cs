using Microsoft.EntityFrameworkCore;
using YourRest.Domain.Entities;

namespace YourRest.Infrastructure.Core.DbContexts
{
    public class SharedDbContext : DbContext
    {
        public DbSet<Country> Countries { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Accommodation> Accommodations { get; set; }
        public DbSet<Room> Rooms { get; set; }


        static SharedDbContext()
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        public SharedDbContext() : base() { }

        public SharedDbContext(DbContextOptions<SharedDbContext> options) : base(options) {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string conn = "Host=localhost;Database=your_rest;Username=admin;Password=admin;Port=5433";
                optionsBuilder.UseNpgsql(conn);
            }
            base.OnConfiguring(optionsBuilder);
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Booking>().OwnsOne(
            //    b => b.StartDate,
            //    sa =>
            //    {
            //        sa.Property(p => p.Value).HasColumnName("StartDate");
            //    });

            //modelBuilder.Entity<Booking>().OwnsOne(
            //    b => b.EndDate,
            //    sa =>
            //    {
            //        sa.Property(p => p.Value).HasColumnName("EndDate");
            //    });

            //modelBuilder.Entity<Booking>().OwnsOne(
            //    b => b.Status,
            //    sa =>
            //    {
            //        sa.Property(p => p.Value).HasColumnName("Status");
            //    });

            modelBuilder.Entity<Review>(r =>
            {
                r.HasOne(r => r.Booking)
                    .WithMany()
                    .HasForeignKey(r => r.BookingId);
            });       

            //modelBuilder.Entity<Review>().OwnsOne(
            //    b => b.Comment,
            //    sa =>
            //    {
            //        sa.Property(p => p.Value).HasColumnName("Comment");
            //    });

            modelBuilder.Entity<Review>().OwnsOne(
                b => b.Rating,
                sa =>
                {
                    sa.Property(p => p.Value).HasColumnName("Rating");
                });
            
            modelBuilder.Entity<Accommodation>(r =>
            {
                r.HasOne(r => r.Address)
                    .WithOne()
                    .HasForeignKey<Accommodation>(r => r.AddressId);
            }); 
        }

        public void ClearAllTables()
        {
            Countries.RemoveRange(Countries);
            Bookings.RemoveRange(Bookings);
            Customers.RemoveRange(Customers);
            Cities.RemoveRange(Cities);
            Regions.RemoveRange(Regions);
            Reviews.RemoveRange(Reviews);
            Accommodations.RemoveRange(Accommodations);
            Addresses.RemoveRange(Addresses);
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