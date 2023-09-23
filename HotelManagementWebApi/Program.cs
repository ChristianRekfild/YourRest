using HotelManagementWebApi.Application.UseCase.Reviews;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SharedKernel.Domain.Repositories;
using YourRest.Infrastructure.DbContexts;
using YourRest.Infrastructure.Repositories;

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
