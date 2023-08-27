using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using YourRest.DAL.Postgre.Entities;

namespace YourRest.DAL.Postgre
{
    public class ApplicationContext : DbContext
    {
        private readonly IConfiguration configuration;

        public DbSet<Country> Countries { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<CustomerPhone> CustomersPhones { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<City> Cities { get; set; }

        public ApplicationContext() : base()
        {
        }
        //public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        //{
        //}

        //public ApplicationContext(IConfiguration configuration)
        //{
        //    this.configuration = configuration;
        //    Database.EnsureCreated();
        //}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("YourRestDbConnection"));
        }
    }
}