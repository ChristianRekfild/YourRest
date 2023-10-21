using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

// �������� �� � ��������������
var people = new List<Person>
 {
    new Person("tom@gmail.com", "12345"),
    new Person("bob@gmail.com", "55555")
};

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// ���������� �������� ��������������
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)  // ����� �������������� - � ������� jwt-�������
    .AddJwtBearer(options =>      // ����������� �������������� � ������� jwt-�������
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // ���������, ����� �� �������������� �������� ��� ��������� ������
            ValidateIssuer = true,
            // ������, �������������� ��������
            ValidIssuer = AuthOptions.ISSUER,
            // ����� �� �������������� ����������� ������
            ValidateAudience = true,
            // ��������� ����������� ������
            ValidAudience = AuthOptions.AUDIENCE,
            // ����� �� �������������� ����� �������������
            ValidateLifetime = true,
            // ��������� ����� ������������
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
            // ��������� ����� ������������
            ValidateIssuerSigningKey = true,
        };
    });
builder.Services.AddAuthorization();            // ���������� �������� �����������

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();   // ���������� middleware �������������� 
app.UseAuthorization();   // ���������� middleware ����������� 

app.UseRouting();

app.Map("/login/{username}", (string username) =>
//app.Map("/login", (Person loginData) =>
{
    //// ������� ������������ 
    //Person? person = people.FirstOrDefault(p => p.Email == loginData.Email && p.Password == loginData.Password);
    //// ���� ������������ �� ������, ���������� ��������� ��� 401
    //if (person is null) return Results.Unauthorized();

    var claims = new List<Claim> { new Claim(ClaimTypes.Name, username) };
    // ������� JWT-�����
    var jwt = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

    //"Authorization": "Bearer " + token  // token - ���������� ����� jwt-�����
    return new JwtSecurityTokenHandler().WriteToken(jwt);
    //var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

    //// ��������� �����
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

app.Run();

public class AuthOptions
{
    public const string ISSUER = "MyAuthServer"; // �������� ������
    public const string AUDIENCE = "MyAuthClient"; // ����������� ������
    const string KEY = "mysupersecret_secretkey!123";   // ���� ��� ��������
    public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
}
record class Person(string Email, string Password);