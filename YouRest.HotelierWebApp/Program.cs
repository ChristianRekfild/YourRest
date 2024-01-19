using FluentValidation;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Globalization;
using System.Security.Claims;
using YouRest.HotelierWebApp.Data.Services;
using YouRest.HotelierWebApp.Data.ViewModels;
using YouRest.HotelierWebApp.Data.ViewModels.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("ru");
builder.Services.AddValidatorsFromAssemblyContaining<AuthorizationViewModelValidator>();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpClient();
builder.Services.AddHotelierWebAppServices();
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, TokenAuthenticationStateProvider>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
internal class TokenAuthenticationStateProvider : AuthenticationStateProvider
{

    private readonly ProtectedLocalStorage localStorage;

    public TokenAuthenticationStateProvider(ProtectedLocalStorage protectedSessionStore)
    {
        this.localStorage = protectedSessionStore;
    }
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        AuthenticationState CreateAnonymous()
        {
            var anonymousIdentity = new ClaimsIdentity();
            var anonymousPrincipal = new ClaimsPrincipal(anonymousIdentity);
            return new AuthenticationState(anonymousPrincipal);
        }

        var token = (await localStorage.GetAsync<SecurityTokenViewModel>(nameof(SecurityTokenViewModel))).Value;

        if (token == null)
        {
            return CreateAnonymous();
        }

        if (string.IsNullOrEmpty(token.AccessToken) || token.ExpiredAt < DateTime.UtcNow)
        {
            return CreateAnonymous();
        }

        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Country,"Russia"),
            new Claim(ClaimTypes.Name,token.UserName),
            new Claim(ClaimTypes.Role,"Administrator"),
            new Claim(ClaimTypes.Role,"Manager"),
            new Claim(ClaimTypes.Expired,token.ExpiredAt.Value.ToLongDateString())
        };

        var identity = new ClaimsIdentity(claims, "Token");
        var principal = new ClaimsPrincipal(identity);
        return new AuthenticationState(principal);
    }
    public async Task MakeUserAnonymous()
    {
        await localStorage.DeleteAsync(nameof (SecurityTokenViewModel));
        var anonymousIdentity = new ClaimsIdentity();
        var anonymousPrincipal = new ClaimsPrincipal(anonymousIdentity);
        var authState = Task.FromResult(new AuthenticationState(anonymousPrincipal));
        NotifyAuthenticationStateChanged(authState);
    }
}