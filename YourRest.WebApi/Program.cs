using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using YourRest.Application;
using YourRest.Infrastructure.Core.DbContexts;
using YourRest.Producer.Infrastructure;
using YourRest.Producer.Infrastructure.Middleware;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        ConfigureServices(builder.Services);

        var app = builder.Build();
        Configure(app);

        app.Run();
    }

    public static void ConfigureServices(IServiceCollection services)
    {
#pragma warning disable ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'
        var configuration = services.BuildServiceProvider().GetService<IConfiguration>();
#pragma warning restore ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'
        string? connectionString;
   
        connectionString = configuration?.GetConnectionString("DefaultConnection");
        var migrationsAssembly = typeof(InfrastructureDependencyInjections).Assembly.GetName().Name;

        services.AddDbContext<SharedDbContext>(options => options.UseNpgsql(connectionString,
            sql => sql.MigrationsAssembly(migrationsAssembly)));

        services.AddControllers();

        // Swagger/OpenAPI configuration
        services.AddEndpointsApiExplorer();
        //services.AddSwaggerGen();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
        });

        services.AddInfrastructure();
        services.AddApplication();
    }

    public static void Configure(IApplicationBuilder app)
    {
#pragma warning disable CS8604 // Code that generates warning CS8604 is written here and will be ignored by the compiler.
        if (app.ApplicationServices.GetService<IWebHostEnvironment>().IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
#pragma warning disable CS8604 // Code that generates warning CS8604 is written here and will be ignored by the compiler.
        app.UseHttpsRedirection();
        app.UseRouting(); // This is necessary for the endpoints to work.
        app.UseAuthorization();

        app.UseMiddleware<ErrorHandlingMiddleware>();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
