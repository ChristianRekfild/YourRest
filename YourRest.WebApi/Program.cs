using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using YourRest.WebApi.BookingContext.Application.Ports;
using YourRest.WebApi.BookingContext.Application.UseCases;
using YourRest.WebApi.BookingContext.Domain.Ports;
using YourRest.WebApi.BookingContext.Infrastructure.Adapters.Repositories;
using YourRest.Infrastructure.DbContexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

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

        services.AddDbContext<SharedDbContext>(options => options.UseNpgsql(connectionString));

        services.AddSingleton<IDbContextFactory<SharedDbContext>>(serviceProvider =>
        {
            var connString = configuration.GetConnectionString("DefaultConnection");
            return new AppDbContextFactory(connString);
        });

        services.AddControllers();

        // Swagger/OpenAPI configuration
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
        });

        services.AddScoped<IGetCountryListUseCase, GetCountryListUseCase>();
        services.AddScoped<ICountryRepository, CountryRepository>();

        services.AddScoped<IGetRegionListUseCase, GetRegionListUseCase>();
        services.AddScoped<IRegionRepository, RegionRepository>();
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
