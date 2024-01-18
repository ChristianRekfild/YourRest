using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Net;
using YouRest.HotelierWebApp.Data.Services.Abstractions;
using YouRest.HotelierWebApp.Data.ViewModels;
using YouRest.HotelierWebApp.Shared;

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
        [Inject] ProtectedSessionStorage storage { get; set; }
        #endregion

        public async Task RegisterAsync()
        {
            if (await RegisterFormValidator.ValidateAsync())
            {
                var response = await AuthorizationService.RegistrationAsync(RegistrationData, tokenSource.Token);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var accessToken = (await response.Content.ReadFromJsonAsync<TokenViewModel>(cancellationToken: tokenSource.Token))?.AccessToken;
                    if (!string.IsNullOrEmpty(accessToken))
                    {
                        await storage.SetAsync("accessToken", accessToken);
                        RegistrationData = new RegistrationViewModel();
                        MainLayout.IsAuthorize = true;
                        Navigation.NavigateTo("statistic");
                    }
                }
            }

        }
    }
}
