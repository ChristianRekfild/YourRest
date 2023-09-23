using Microsoft.EntityFrameworkCore;

namespace HotelManagementWebApi.Infrastructure.Repositories.DbContexts;

public class AppDbContextFactory : IDbContextFactory<HotelManagementDbContext>
{
    private readonly string _connectionString;

    public AppDbContextFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public HotelManagementDbContext CreateDbContext()
    {
        var optionsBuilder = new DbContextOptionsBuilder<HotelManagementDbContext>();
        optionsBuilder.UseNpgsql(_connectionString);
        return new HotelManagementDbContext(optionsBuilder.Options);
    }
}

