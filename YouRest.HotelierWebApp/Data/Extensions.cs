using IdentityModel.Client;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace YouRest.HotelierWebApp.Data
{
    public static class Extensions
    {
        public static async Task<HttpClient> SetAccessToken(this HttpClient client, ProtectedSessionStorage sessionStorage)
        {
            var token = await sessionStorage.GetAsync<string>("accessToken");
            if (token.Success && !string.IsNullOrEmpty(token.Value))
                client.SetBearerToken(token.Value);
            return client;
        }
    }
}
