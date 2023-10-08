using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using YourRest.Application.Interfaces;
using YourRest.Application.UseCases;
using YourRest.Domain.Repositories;
using YourRest.Infrastructure.Core;
using YourRest.Infrastructure.Core.DbContexts;
using YourRest.Infrastructure.Repositories;
using YourRest.Producer.Infrastructure.Repositories;

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
        //TODO: Возможно это можно как-то улучшить
#pragma warning disable ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'
        var configuration = services.BuildServiceProvider().GetService<IConfiguration>();
#pragma warning restore ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'
                               //var environment = services.BuildServiceProvider().GetService<IWebHostEnvironment>();

        string? connectionString;
   
        connectionString = configuration?.GetConnectionString("DefaultConnection");

        services.AddDbContext<SharedDbContext>(options => options.UseNpgsql(connectionString));

        //services.AddSingleton<IDbContextFactory<SharedDbContext>>(serviceProvider =>
        //{
        //    var connString = configuration.GetConnectionString("DefaultConnection");
        //    return new AppDbContextFactory(connString);
        //});

        services.AddControllers();

        // Swagger/OpenAPI configuration
        services.AddEndpointsApiExplorer();
        //services.AddSwaggerGen();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
        });

        services.AddScoped<IGetCountryListUseCase, GetCountryListUseCase>();
        services.AddScoped<IGetCityByIdUseCase, GetCityByIdUseCase>();
        services.AddScoped<IGetCityListUseCase, GetCityListUseCase>();

        services.AddScoped<IGetCityByRegionIdUseCase, GetCityByRegionIdUseCase>();
        services.AddScoped<IGetCityByCountryIdUseCase, GetCityByCountryIdUseCase>();

        services.AddScoped<IGetRegionListUseCase, GetRegionListUseCase>();
        services.AddScoped<ICreateReviewUseCase, CreateReviewUseCase>();
        services.AddScoped<IAddAddressToAccommodationUseCase, AddAddressToAccommodationUseCase>();

        services.AddScoped<IBookingRepository, BookingRepository>();
        services.AddScoped<ICountryRepository, CountryRepository>();  
        services.AddScoped<ICustomerRepository, CustomerRepository>();      
        services.AddScoped<ICityRepository, CityRepository>();        
        services.AddScoped<IRegionRepository, RegionRepository>();        
        services.AddScoped<IReviewRepository, ReviewRepository>();
        services.AddScoped<IAccommodationRepository, AccommodationRepository>();
        services.AddScoped<IAddressRepository, AddressRepository>();
    }

    public static void Configure(IApplicationBuilder app)
    {
#pragma warning disable CS8604 // Возможно, аргумент-ссылка, допускающий значение NULL.
        if (app.ApplicationServices.GetService<IWebHostEnvironment>().IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
#pragma warning restore CS8604 // Возможно, аргумент-ссылка, допускающий значение NULL.

        app.UseHttpsRedirection();
        app.UseRouting(); // This is necessary for the endpoints to work.
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
