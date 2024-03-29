using FluentValidation;
using Microsoft.AspNetCore.Components.Authorization;
using System.Globalization;
using YouRest.HotelierWebApp.Data.Providers;
using YouRest.HotelierWebApp.Data.Services;
using YouRest.HotelierWebApp.Data.ViewModels.Validators;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient();
builder.Services.AddHotelierWebAppServices();
ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("ru");
builder.Services.AddValidatorsFromAssemblyContaining<AuthorizationViewModelValidator>();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthValidateProvider>();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
