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
using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using YourRest.Producer.Infrastructure.Seeds;
using YourRest.Producer.Infrastructure.Keycloak.Settings;
using YourRest.WebApi.Options;
using Amazon.S3;
using MassTransit;
using Microsoft.AspNetCore.Http.Features;
using YourRest.Application.Services;
using YourRest.Domain.Events;
using YourRest.Producer.Infrastructure.Listeners;
using YourRest.Producer.Infrastructure.Messaging.RabbitMQ;
using YourRest.Producer.Infrastructure.Messaging.RabbitMQ.Consumers;
using YourRest.WebApi;

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

        if (configuration == null)
        {
            throw new Exception("Not fountproget onfiguration.");
        }
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

        services.Configure<KeycloakSetting>(configuration.GetSection("KeycloakSetting"));
        services.AddKeycloakInfrastructure();
        services.AddInfrastructure();
        services.AddApplication();
        services.AddWebApi();
        

        services.AddHttpClient();
        services.AddTransient<ICustomHttpClientFactory, CustomHttpClientFactory>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.Authority = configuration.GetValue<string>("KeycloakSetting:Authority");
            options.Audience = configuration.GetValue<string>("KeycloakSetting:ClientId");
            options.RequireHttpsMetadata = false;

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration.GetValue<string>("KeycloakSetting:Authority"),
                ValidAudience = configuration.GetValue<string>("KeycloakSetting:ClientId"),
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("KeycloakSetting:ClientSecret"))),
            };
        });
        
        services.AddMassTransit(x =>
        {
            x.AddConsumer<AccommodationCreatedConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(new Uri("rabbitmq://rabbitmq"), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ReceiveEndpoint("accommodation_created_queue", e =>
                {
                    e.ConfigureConsumer<AccommodationCreatedConsumer>(context);
                });
            });
        });
        
        services.AddSingleton<RabbitMqMessageProducer>(provider =>
        {
            string hostname = "rabbitmq";
            string queueName = "accommodation_created_queue";

            return new RabbitMqMessageProducer(hostname, queueName);
        });
        
        services.AddMassTransitHostedService();
        services.AddSingleton<IEventHandler<AccommodationCreatedEvent>, NotificationListener>();

        var awsOptions = configuration.GetSection("AWS").Get<AwsOptions>();
        if (awsOptions != null)
        {
            services.AddSingleton(awsOptions);


            var s3Config = new AmazonS3Config
            {
                ServiceURL = awsOptions.ServiceURL,
                ForcePathStyle = true
            };
            services.AddSingleton(s3Config);
            services.AddSingleton<IAmazonS3>(sp =>
            {
                var config = sp.GetRequiredService<AmazonS3Config>();
                var creds = new BasicAWSCredentials(
                    awsOptions.AccessKey,
                    awsOptions.SecretKey
                );
                return new AmazonS3Client(creds, config);
            });
        }
    }

    public static void Configure(IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseCors(builder => builder
            .AllowAnyOrigin()  // Not for production
            .AllowAnyMethod()
            .AllowAnyHeader());
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseMiddleware<ErrorHandlingMiddleware>();
        app.UseMiddleware<UserSavingMiddleware>();
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

        using var serviceScope = app.ApplicationServices.CreateScope();
        var context = serviceScope.ServiceProvider.GetService<SharedDbContext>();
        if (context != null)
        {
            var seeder = new DatabaseSeeder(context);
            seeder.Seed();
        }
    }
}
