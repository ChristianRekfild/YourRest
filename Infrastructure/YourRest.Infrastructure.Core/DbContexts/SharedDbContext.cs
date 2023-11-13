using Microsoft.EntityFrameworkCore;
using YourRest.Domain.Entities;

namespace YourRest.Infrastructure.Core.DbContexts
{
    public class SharedDbContext : DbContext
    {
        public DbSet<Country> Countries { get; set; }
        public DbSet<Guest> Guests { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Accommodation> Accommodations { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomFacility> RoomFacilities { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<User> Users { get; set; }    
        public DbSet<UserAccommodation> UserAccommodations { get; set; }
        public DbSet<AccommodationType> AccommodationTypes { get; set; }
        public DbSet<AgeRange> AgeRanges { get; set; }



        static SharedDbContext()
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        public SharedDbContext() : base() { }

        public SharedDbContext(DbContextOptions<SharedDbContext> options) : base(options) {
            Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string conn = "Host=localhost;Database=your_rest;Username=admin;Password=admin;Port=5433";
                optionsBuilder.UseNpgsql(conn, sql => sql.MigrationsAssembly("YourRest.Producer.Infrastructure"));
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
            
            modelBuilder.Entity<UserAccommodation>()
                .HasKey(ug => new { ug.UserId, ug.AccommodationId });

            modelBuilder.Entity<UserAccommodation>()
                .HasOne(ug => ug.User)
                .WithMany(u => u.UserAccommodations)
                .HasForeignKey(ug => ug.UserId);

            modelBuilder.Entity<UserAccommodation>()
                .HasOne(ug => ug.Accommodation)
                .WithMany(g => g.UserAccommodations)
                .HasForeignKey(ug => ug.AccommodationId);
        }

        public void ClearAllTables()
        {
            Countries.RemoveRange(Countries);
            Bookings.RemoveRange(Bookings);
            Guests.RemoveRange(Guests);
            Cities.RemoveRange(Cities);
            Regions.RemoveRange(Regions);
            Reviews.RemoveRange(Reviews);
            Accommodations.RemoveRange(Accommodations);
            Addresses.RemoveRange(Addresses);
            Rooms.RemoveRange(Rooms);
            RoomFacilities.RemoveRange(RoomFacilities);
            RoomTypes.RemoveRange(RoomTypes);
            AccommodationTypes.RemoveRange(AccommodationTypes);
            AgeRanges.RemoveRange(AgeRanges);

            // Add other DbSet removals here
            // Example: 
            // Rooms.RemoveRange(Rooms);
            // Hotels.RemoveRange(Hotels);
            // ... and so on

            SaveChanges();
        }
    }

}