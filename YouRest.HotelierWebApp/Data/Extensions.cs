using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Text.Json;
using YouRest.HotelierWebApp.Data.Models;
using YouRest.HotelierWebApp.Data.Services;
using YouRest.HotelierWebApp.Data.Services.Abstractions;
using YouRest.HotelierWebApp.Data.ViewModels.Interfaces;

namespace YouRest.HotelierWebApp.Data
{
    public static class Extensions
    {
        public static async Task<HttpClient> SetAccessToken(this HttpClient client, ProtectedLocalStorage localStorage)
        {
            var securityToken = await localStorage.GetAsync<SecurityTokenModel>(nameof(SecurityTokenModel));
            if (securityToken.Success && !string.IsNullOrEmpty(securityToken.Value?.AccessToken))
                client.SetBearerToken(securityToken.Value.AccessToken);
            return client;
        }
        public static string? GetJWTClaim(this JwtSecurityToken jwtSecurityToken, string jwtClaimType)
        {
            return jwtSecurityToken.Claims?.FirstOrDefault(x => x.Type == jwtClaimType)?.Value;
        }
        public static DateTime? UnixExpirationTimeToLocalDateTime(this string seconds)
        {
            long parseResult;
            if (long.TryParse(seconds, out parseResult))
            {
                return DateTimeOffset.FromUnixTimeSeconds(parseResult).LocalDateTime;
            }
            else return null;
        }
        public static T DeepCopy<T>(this T obj) => JsonSerializer.Deserialize<T>(JsonSerializer.Serialize<T>(obj));
        
        public static async Task FillHotelModelFormAsync(this HotelModel hotel, IServiceRepository serviceRepository, IHotelViewModel viewModel, CancellationToken cancellationToken = default)
        {

            viewModel.CurrentHotelModelForm.HotelName = hotel.Name;
            viewModel.CurrentHotelModelForm.HotelDescription = hotel.Description;
            viewModel.CurrentHotelModelForm.HotelRating = serviceRepository.HotelService.ConvertHotelRating(hotel.Stars);

            if (hotel.AccommodationType is null) return;
            viewModel.CurrentHotelModelForm.HotelType = hotel.AccommodationType.Name;
            if (hotel.Address is null) return;
            viewModel.CurrentHotelModelForm.Address = hotel.Address.Street;
            viewModel.CurrentHotelModelForm.ZipCode = hotel.Address.ZipCode;
            var hotelType = (await serviceRepository.HotelTypeService.FetchHotelTypesAsync(cancellationToken))?.FirstOrDefault(ht => ht.Id == hotel.AccommodationType?.Id);
            if (hotelType is null) return;
            viewModel.CurrentHotelModelForm.HotelType = hotelType.Name;
            var city = (await serviceRepository.CityService.FetchCytiesAsync(cancellationToken))?.FirstOrDefault(c => c.Id == hotel.Address.CityId);
            if (city is null) return;
            viewModel.CurrentHotelModelForm.City = city.Name;
            var region = (await serviceRepository.RegionService.FetchRegionsAsync(cancellationToken))?.FirstOrDefault(r => r.Id == city.RegionId);
            if (region is null) return;
            viewModel.CurrentHotelModelForm.Region = region.Name;
            var country = await serviceRepository.CountryService.FetchCountryAsync(region.CountryId, cancellationToken);
            if (country is null) return;
            viewModel.CurrentHotelModelForm.Country = country.Name;

        }
    }
}
