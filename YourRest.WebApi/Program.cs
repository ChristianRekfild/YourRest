using YourRest.Application;
using YourRest.Infrastructure.Core.DbContexts;
using YourRest.Producer.Infrastructure;
using YourRest.Producer.Infrastructure.Middleware;
using YourRest.Producer.Infrastructure.Keycloak;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using YourRest.Producer.Infrastructure.Keycloak.Http;
using System.Text;
using YourRest.Producer.Infrastructure.Seeds;

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
        var migrationsAssembly = typeof(ProducerInfrastructureDependencyInjections).Assembly.GetName().Name;

        services.AddDbContext<SharedDbContext>(options => options.UseNpgsql(connectionString,
            sql => sql.MigrationsAssembly(migrationsAssembly)));

        services.AddCors();
        services.AddControllers();

        // Swagger/OpenAPI configuration
        services.AddEndpointsApiExplorer();
        //services.AddSwaggerGen();
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
        
        services.AddKeycloakInfrastructure();
        services.AddInfrastructure();
        services.AddApplication();
        
        services.AddSingleton(_ => configuration.GetValue<string>("ClientId"));
        services.AddSingleton(_ => configuration.GetValue<string>("ClientSecret"));
        services.AddSingleton(_ => configuration.GetValue<string>("KeycloakUrl"));

        
        services.AddHttpClient();
        services.AddTransient<ICustomHttpClientFactory, CustomHttpClientFactory>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.Authority = configuration.GetValue<string>("Authority");
            options.Audience = configuration.GetValue<string>("ClientId");
            options.RequireHttpsMetadata = false;

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration.GetValue<string>("Authority"),
                ValidAudience = configuration.GetValue<string>("ClientId"),
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("ClientSecret"))),
            };
        });
    }

    public static void Configure(IApplicationBuilder app)
    {
#pragma warning disable CS8604 // Code that generates warning CS8604 is written here and will be ignored by the compiler.
        app.UseSwagger();
        app.UseSwaggerUI();
#pragma warning disable CS8604 // Code that generates warning CS8604 is written here and will be ignored by the compiler.

        app.UseCors(builder => builder
            .AllowAnyOrigin()  // Not for production
            .AllowAnyMethod()
            .AllowAnyHeader());
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseMiddleware<UserSavingMiddleware>();
        app.UseAuthorization();
        app.UseMiddleware<ErrorHandlingMiddleware>();
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        
        using var serviceScope = app.ApplicationServices.CreateScope();
        var context = serviceScope.ServiceProvider.GetService<SharedDbContext>();
        var seeder = new DatabaseSeeder(context);
        seeder.Seed();
    }
}
