using HotelManagementWebApi.Application.UseCase.Reviews;
using HotelManagementWebApi.Domain.Repositories;
using HotelManagementWebApi.Infrastructure.Repositories;
using HotelManagementWebApi.Infrastructure.Repositories.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace HotelManagementWebApi;
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

        string connectionString;

   
        connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<HotelManagementDbContext>(options => options.UseNpgsql(connectionString));

        services.AddSingleton<IDbContextFactory<HotelManagementDbContext>>(serviceProvider =>
        {
            var connString = configuration.GetConnectionString("DefaultConnection");
            return new HotelManagementWebApi.Infrastructure.Repositories.DbContexts.AppDbContextFactory(connString);
        });

        services.AddControllers();

        // Swagger/OpenAPI configuration
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
        });

        services.AddScoped<ICreateReviewUseCase, CreateReviewUseCase>();
        services.AddScoped<IBookingRepository, BookingRepository>();
        services.AddScoped<IReviewRepository, ReviewRepository>();
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
