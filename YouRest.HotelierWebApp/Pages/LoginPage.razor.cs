using Blazored.FluentValidation;
using IdentityModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using YouRest.HotelierWebApp.Data;
using YouRest.HotelierWebApp.Data.Models;
using YouRest.HotelierWebApp.Data.Services;
using YouRest.HotelierWebApp.Data.Services.Abstractions;
using YouRest.HotelierWebApp.Shared;

namespace YouRest.HotelierWebApp.Pages
{
    public partial class LoginPage : ComponentBase, IDisposable
    {
        #region Fields and Properties
        protected CancellationTokenSource tokenSource = new();
        public FluentValidationValidator? LoginValidator { get; set; }
        public FluentValidationValidator? RegisterValidator { get; set; }
        public AuthorizationModel AuthData { get; set; } = new();
        [Parameter] public EventCallback OnRegistry { get; set; }
        #endregion

        #region Dependency Injection
        [Inject] IServiceRepository ServiceRepository { get; set; }
        [Inject] ProtectedLocalStorage LocalStorage { get; set; }
        [Inject] NavigationManager Navigation { get; set; }
        #endregion
        protected override void OnInitialized()
        {
            base.OnInitialized();
            Navigation.NavigateTo("/login", false);
        }

        public async Task LoginAsync()
        {
            if (await LoginValidator!.ValidateAsync())
            {
                var response = await ServiceRepository.AuthorizationService.LoginAsync(AuthData, tokenSource.Token);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var securityToken = await response.Content.ReadFromJsonAsync<SecurityTokenModel>(cancellationToken: tokenSource.Token);
                    if (securityToken != null)
                    {
                        var jwtSecurity = new JwtSecurityToken(securityToken.AccessToken);
                        securityToken.UserName = jwtSecurity.GetJWTClaim(JwtClaimTypes.Subject);
                        securityToken.ExpiredAt = jwtSecurity.GetJWTClaim(JwtClaimTypes.Expiration)?.UnixExpirationTimeToLocalDateTime();
                        await LocalStorage.SetAsync(nameof(SecurityTokenModel), securityToken);
                        Navigation.NavigateTo("/", true);
                    }
                }
                AuthData = new AuthorizationModel();
            }
        }

        public void Dispose()
        {
            tokenSource.Cancel();
            tokenSource.Dispose();
        }
    }
}