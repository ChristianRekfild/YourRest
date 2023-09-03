using YourRest.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

public class AppDbContextFactory : IDbContextFactory<SharedDbContext>
{
    private readonly string _connectionString;

    public AppDbContextFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public SharedDbContext CreateDbContext()
    {
        var optionsBuilder = new DbContextOptionsBuilder<SharedDbContext>();
        optionsBuilder.UseNpgsql(_connectionString);
        return new SharedDbContext(optionsBuilder.Options);
    }
}
