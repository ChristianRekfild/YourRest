using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using YourRest.ClientIdentity.Infrastructure;
using YourRest.ClientIdentity.Infrastructure.Contracts.Entities;
using YourRest.Infrastructure.Core.DbContexts;


var builder = WebApplication.CreateBuilder(args);

#pragma warning disable ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'
var configuration = builder.Services.BuildServiceProvider().GetService<IConfiguration>();
#pragma warning restore ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'
string? connectionString;

connectionString = configuration?.GetConnectionString("IdentityConnection");
var migrationsAssembly = typeof(ClientIdentityInfrastructureDependencyInjections).Assembly.GetName().Name;
builder.Services.AddDbContext<ClientAppIdentityContext>(options => options.UseNpgsql(connectionString,
            sql => sql.MigrationsAssembly(migrationsAssembly)));

builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<ClientAppIdentityContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

// добавление сервисов аутентификации
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)  // схема аутентификации - с помощью jwt-токенов
    .AddJwtBearer(options =>      // подключение аутентификации с помощью jwt-токенов
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // указывает, будет ли валидироваться издатель при валидации токена
            ValidateIssuer = true,
            // строка, представляющая издателя
            ValidIssuer = AuthOptions.ISSUER,
            // будет ли валидироваться потребитель токена
            ValidateAudience = true,
            // установка потребителя токена
            ValidAudience = AuthOptions.AUDIENCE,
            // будет ли валидироваться время существования
            ValidateLifetime = true,
            // установка ключа безопасности
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
            // валидация ключа безопасности
            ValidateIssuerSigningKey = true,
        };
    });
builder.Services.AddAuthorization();            // добавление сервисов авторизации
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); //"outputPath": "dist/client-app",

app.UseAuthentication();   // добавление middleware аутентификации 
app.UseAuthorization();   // добавление middleware авторизации 

app.UseRouting();

app.Map("/login/{username}", (string username) =>
//app.Map("/login", (Person loginData) =>
{
    //// находим пользователя 
    //Person? person = people.FirstOrDefault(p => p.Email == loginData.Email && p.Password == loginData.Password);
    //// если пользователь не найден, отправляем статусный код 401
    //if (person is null) return Results.Unauthorized();

    var claims = new List<Claim> { new Claim(ClaimTypes.Name, username) };
    // создаем JWT-токен
    var jwt = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

    //"Authorization": "Bearer " + token  // token - полученный ранее jwt-токен
    return new JwtSecurityTokenHandler().WriteToken(jwt);
    //var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

    //// формируем ответ
    //var response = new
    //{
    //    access_token = encodedJwt,
    //    username = person.Email
    //};

    //return Results.Json(response);
});

app.Map("/data", [Authorize] (HttpContext context) => $"Hello World!");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapBlazorHub();
app.Run();

public class AuthOptions
{
    public const string ISSUER = "MyAuthServer"; // издатель токена
    public const string AUDIENCE = "MyAuthClient"; // потребитель токена
    const string KEY = "mysupersecret_secretkey!123";   // ключ для шифрации
    public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
}
