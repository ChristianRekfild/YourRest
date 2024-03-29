﻿using Blazored.FluentValidation;
using IdentityModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using YouRest.HotelierWebApp.Data;
using YouRest.HotelierWebApp.Data.Services.Abstractions;
using YouRest.HotelierWebApp.Data.ViewModels;

namespace YouRest.HotelierWebApp.Pages
{
    public partial class RegisterPage : ComponentBase
    {
        #region Fields and Properties
        protected CancellationTokenSource tokenSource = new();
        public RegistrationViewModel RegistrationData { get; set; } = new();
        public FluentValidationValidator? RegisterFormValidator { get; set; }
        #endregion

        #region Dependency Injection
        [Inject] IAuthorizationService AuthorizationService { get; set; }
        [Inject] NavigationManager Navigation { get; set; }
        [Inject] ProtectedLocalStorage LocalStorage { get; set; }
        #endregion

        public async Task RegisterAsync()
        {
            if (await RegisterFormValidator.ValidateAsync())
            {
                var response = await AuthorizationService.RegistrationAsync(RegistrationData, tokenSource.Token);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var securityToken = (await response.Content.ReadFromJsonAsync<SecurityTokenViewModel>(cancellationToken: tokenSource.Token));
                    if (!string.IsNullOrEmpty(securityToken.AccessToken))
                    {
                        var jwtSecurity = new JwtSecurityToken(securityToken.AccessToken);
                        securityToken.UserName = jwtSecurity.GetJWTClaim(JwtClaimTypes.Subject);
                        securityToken.ExpiredAt = jwtSecurity.GetJWTClaim(JwtClaimTypes.Expiration)?.UnixExpirationTimeToLocalDateTime();
                        await LocalStorage.SetAsync(nameof(SecurityTokenViewModel), securityToken);
                        RegistrationData = new RegistrationViewModel();
                        Navigation.NavigateTo("/");
                    }
                }
            }
        }
    }
}
