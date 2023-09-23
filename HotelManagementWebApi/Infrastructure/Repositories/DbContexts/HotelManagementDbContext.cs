using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using HotelManagementWebApi.Domain.Entities;

namespace HotelManagementWebApi.Infrastructure.Repositories.DbContexts
{
    public class HotelManagementDbContext : DbContext
    {
        private readonly IConfiguration configuration;

        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Review> Reviews { get; set; }

        public HotelManagementDbContext(DbContextOptions<HotelManagementDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>().OwnsOne(
                b => b.StartDate,
                sa =>
                {
                    sa.Property(p => p.Value).HasColumnName("StartDate");
                });

            modelBuilder.Entity<Booking>().OwnsOne(
                b => b.EndDate,
                sa =>
                {
                    sa.Property(p => p.Value).HasColumnName("EndDate");
                });

            modelBuilder.Entity<Booking>().OwnsOne(
                b => b.Status,
                sa =>
                {
                    sa.Property(p => p.Value).HasColumnName("Status");
                });

            modelBuilder.Entity<Review>(r =>
            {
                r.HasOne(r => r.Booking)
                    .WithMany()
                    .HasForeignKey(r => r.BookingId);
            });

            modelBuilder.Entity<Review>().OwnsOne(
                b => b.Comment,
                sa =>
                {
                    sa.Property(p => p.Value).HasColumnName("Comment");
                });

            modelBuilder.Entity<Review>().OwnsOne(
                b => b.Rating,
                sa =>
                {
                    sa.Property(p => p.Value).HasColumnName("Rating");
                });
        }

        public void ClearAllTables()
        {
            Bookings.RemoveRange(Bookings);
            Reviews.RemoveRange(Reviews);
            
            // Add other DbSet removals here
            // Example: 
            // Rooms.RemoveRange(Rooms);
            // Hotels.RemoveRange(Hotels);
            // ... and so on

            SaveChanges();
        }
    }
}