using YourRest.Application.Interfaces;
using YourRest.Application.UseCases;
using YourRest.Domain.Repositories;
using YourRest.Infrastructure.Core;
using YourRest.Infrastructure.Core.DbContexts;
using YourRest.Infrastructure.Repositories;
using YourRest.Producer.Infrastructure.Repositories;
using YourRest.WebApi.Filters;
using YourRest.Producer.Infrastructure.Middlewares;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using IdentityModel;
using System.Text;
using Microsoft.IdentityModel.Logging;

public class Program
{
    public static IConfiguration Configuration { get; set; }
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        Configuration = builder.Configuration;
        
        ConfigureServices(builder.Services);

        var app = builder.Build();
        Configure(app);

        app.Run();
    }

    public static void ConfigureServices(IServiceCollection services)
    {  
        string connectionString = Configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<SharedDbContext>(options => options.UseNpgsql(connectionString));

        services.AddSingleton<IDbContextFactory<SharedDbContext>>(serviceProvider =>
        {
            return new AppDbContextFactory(connectionString);
        });
        services.AddCors();
        services.AddControllers();

        // Swagger/OpenAPI configuration
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            //c.OperationFilter<AuthorizeCheckOperationFilter>();

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
        });

        services.AddScoped<IGetCountryListUseCase, GetCountryListUseCase>();
        services.AddScoped<IGetCityByIdUseCase, GetCityByIdUseCase>();
        services.AddScoped<IGetCityListUseCase, GetCityListUseCase>();
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
        services.AddScoped<IUserRepository, UserRepository>();
        
        if (Configuration == null) throw new Exception("Configuration is null");
        if (string.IsNullOrEmpty(Configuration["JwtSettings:Authority"])) throw new Exception("JwtSettings:Authority is missing");
        
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.Authority = Configuration["JwtSettings:Authority"];
            options.Audience = Configuration["JwtSettings:Audience"];
            options.RequireHttpsMetadata = false;

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = Configuration["JwtSettings:Authority"],
                ValidAudience = Configuration["JwtSettings:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtSettings:SymmetricKey"])),
            };
        });
    }

    public static void Configure(IApplicationBuilder app)
    {
//        bool useSwagger = configuration.GetValue<bool>("UseSwagger");
//        if (env.IsDevelopment() || useSwagger)
//        {
//            app.UseSwagger();
//            app.UseSwaggerUI();
//        }

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseCors(builder => builder
                .AllowAnyOrigin()  // Not for production
                .AllowAnyMethod()
                .AllowAnyHeader());

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseMiddleware<UserSavingMiddleware>();
        app.UseAuthorization();
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
    
    public static void SetConfiguration(IConfiguration configuration)
    {
        Configuration = configuration;
    }
}
