using Microsoft.EntityFrameworkCore;
using YourRest.Producer.Infrastructure.Entities;

namespace YourRest.Producer.Infrastructure.DbContexts
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
        
        public DbSet<AccommodationStarRating> AccommodationStarRatings { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomFacility> RoomFacilities { get; set; }
        
        public DbSet<AccommodationFacility> AccommodationFacility { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<User> Users { get; set; }    
        public DbSet<UserAccommodation> UserAccommodations { get; set; }
        public DbSet<AccommodationFacilityLink> AccommodationFacilities { get; set; }
        public DbSet<AccommodationType> AccommodationTypes { get; set; }
        public DbSet<AgeRange> AgeRanges { get; set; }
        
        public DbSet<AccommodationPhoto> AccommodationPhotos { get; set; }
        
        public DbSet<RoomPhoto> RoomPhotos { get; set; }
        public DbSet<UserPhoto> UserPhotos { get; set; }

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
                optionsBuilder.UseNpgsql(conn, sql => sql.MigrationsAssembly("YourRest.Producer.Infrastructure"))
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            }
            base.OnConfiguring(optionsBuilder);            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Review Entity Configuration
            modelBuilder.Entity<Review>(review =>
            {
                review.HasOne(r => r.Booking)
                      .WithMany()
                      .HasForeignKey(r => r.BookingId);

                review.OwnsOne(r => r.Rating, rating =>
                {
                    rating.Property(p => p.Value).HasColumnName("Rating");
                });
            });

            // Accommodation Entity Configuration
            modelBuilder.Entity<Accommodation>(accommodation =>
            {
                accommodation.HasOne(a => a.StarRating)
                             .WithOne(s => s.Accommodation)
                             .HasForeignKey<AccommodationStarRating>(s => s.AccommodationId);

                accommodation.HasOne(a => a.Address)
                             .WithOne()
                             .HasForeignKey<Accommodation>(a => a.AddressId);
            });

            // UserAccommodation Entity Configuration
            modelBuilder.Entity<UserAccommodation>(userAccommodation =>
            {
                userAccommodation.HasKey(ua => new { ua.UserId, ua.AccommodationId });

                userAccommodation.HasOne(ua => ua.User)
                                 .WithMany(u => u.UserAccommodations)
                                 .HasForeignKey(ua => ua.UserId);

                userAccommodation.HasOne(ua => ua.Accommodation)
                                 .WithMany(a => a.UserAccommodations)
                                 .HasForeignKey(ua => ua.AccommodationId);
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
            AccommodationStarRatings.RemoveRange(AccommodationStarRatings);
            UserAccommodations.RemoveRange(UserAccommodations);
            Accommodations.RemoveRange(Accommodations);
            Users.RemoveRange(Users);
            Addresses.RemoveRange(Addresses);
            Rooms.RemoveRange(Rooms);
            RoomFacilities.RemoveRange(RoomFacilities);
            AccommodationFacility.RemoveRange(AccommodationFacility);
            RoomTypes.RemoveRange(RoomTypes);
            AccommodationTypes.RemoveRange(AccommodationTypes);
            AgeRanges.RemoveRange(AgeRanges);
            AccommodationPhotos.RemoveRange(AccommodationPhotos);
            RoomPhotos.RemoveRange(RoomPhotos);
            UserPhotos.RemoveRange(UserPhotos);

            SaveChanges();
        }
    }

}