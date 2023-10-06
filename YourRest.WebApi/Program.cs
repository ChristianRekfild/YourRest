using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using YourRest.Application;
using YourRest.Infrastructure.Core.DbContexts;
using YourRest.Producer.Infrastructure;

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
        var configuration = services.BuildServiceProvider().GetService<IConfiguration>();
        var environment = services.BuildServiceProvider().GetService<IWebHostEnvironment>();

        string connectionString;

   
        connectionString = configuration.GetConnectionString("DefaultConnection");
        var migrationsAssembly = typeof(YourRest.Producer.Infrastructure.DependencyInjections).Assembly.GetName().Name;

        services.AddDbContext<SharedDbContext>(options => options.UseNpgsql(connectionString,
            sql => sql.MigrationsAssembly(migrationsAssembly)));

        //services.AddSingleton<IDbContextFactory<SharedDbContext>>(serviceProvider =>
        //{
        //    var connString = configuration.GetConnectionString("DefaultConnection");
        //    return new AppDbContextFactory(connString);
        //});

        services.AddControllers();

        // Swagger/OpenAPI configuration
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
        });

        services.AddInfrastructure();
        services.AddApplication();
    }

    public static void Configure(IApplicationBuilder app)
    {
        if (app.ApplicationServices.GetService<IWebHostEnvironment>().IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseRouting(); // This is necessary for the endpoints to work.
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
