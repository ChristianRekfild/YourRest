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
        [Inject] IAuthorizationService AuthorizationService { get; set; }
        [Inject] NavigationManager Navigation { get; set; }
        [Inject] ProtectedSessionStorage ProtectedSessionStore { get; set; }

        public RegistrationViewModel RegistrationData { get; set; } = new();
        public FluentValidationValidator? RegisterValidator { get; set; }
        public async Task RegisterAsync()
        {
            if (await RegisterValidator.ValidateAsync())
            {
                var response = await AuthorizationService.RegistrationAsync(RegistrationData);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var accessToken = (await response.Content.ReadFromJsonAsync<TokenViewModel>())?.AccessToken;
                    await ProtectedSessionStore.SetAsync("accessToken", accessToken);
                    RegistrationData = new RegistrationViewModel();
                    MainLayout.IsAuthorize = true;
                    Navigation.NavigateTo("statistic");
                }
            }

        }
    }
}
