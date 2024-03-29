﻿using Blazored.FluentValidation;
using IdentityModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using YouRest.HotelierWebApp.Data;
using YouRest.HotelierWebApp.Data.Services.Abstractions;
using YouRest.HotelierWebApp.Data.ViewModels;
using YouRest.HotelierWebApp.Shared;

namespace YouRest.HotelierWebApp.Pages
{
    public partial class LoginPage : ComponentBase, IDisposable
    {
        #region Fields and Properties
        protected CancellationTokenSource tokenSource = new();
        public FluentValidationValidator? LoginValidator { get; set; }
        public FluentValidationValidator? RegisterValidator { get; set; }
        public AuthorizationViewModel AuthData { get; set; } = new();
        public RegistrationViewModel Registration { get; set; }
        #endregion

        #region Dependency Injection
        [Inject] IAuthorizationService AuthorizationService { get; set; }
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
                var response = await AuthorizationService.LoginAsync(AuthData, tokenSource.Token);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var securityToken = await response.Content.ReadFromJsonAsync<SecurityTokenViewModel>(cancellationToken: tokenSource.Token);
                    if (securityToken != null)
                    {
                        var jwtSecurity = new JwtSecurityToken(securityToken.AccessToken);
                        securityToken.UserName = jwtSecurity.GetJWTClaim(JwtClaimTypes.Subject);
                        securityToken.ExpiredAt = jwtSecurity.GetJWTClaim(JwtClaimTypes.Expiration)?.UnixExpirationTimeToLocalDateTime();
                        await LocalStorage.SetAsync(nameof(SecurityTokenViewModel), securityToken);
                        Navigation.NavigateTo("/", true);
                    }
                }
                AuthData = new AuthorizationViewModel();
            }
        }

        public void Dispose()
        {
            tokenSource.Cancel();
            tokenSource.Dispose();
        }
    }
}