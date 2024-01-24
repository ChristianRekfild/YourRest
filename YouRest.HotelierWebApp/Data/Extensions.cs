using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using YouRest.HotelierWebApp.Data.Models;

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
    }
}
