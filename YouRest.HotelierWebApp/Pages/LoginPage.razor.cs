using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Net;
using YouRest.HotelierWebApp.Data.Services.Abstractions;
using YouRest.HotelierWebApp.Data.ViewModels;
using YouRest.HotelierWebApp.Shared;

namespace YouRest.HotelierWebApp.Pages
{
    public partial class LoginPage : ComponentBase
    {
        [Inject] IAuthorizationService AuthorizationService { get; set; }
        [Inject] ProtectedSessionStorage ProtectedSessionStore { get; set; }
        [Inject] NavigationManager Navigation { get; set; }
        public FluentValidationValidator? LoginValidator { get; set; }
        public FluentValidationValidator? RegisterValidator { get; set; }
        public AuthorizationViewModel Auth { get; set; } = new();
        public RegistrationViewModel Registration { get; set; }
        public async Task LoginAsync()
        {
            if (await LoginValidator!.ValidateAsync())
            {
                var response = await AuthorizationService.LoginAsync(Auth);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var accessToken = await response.Content.ReadFromJsonAsync<TokenViewModel>();
                    await ProtectedSessionStore.SetAsync("accessToken", accessToken.AccessToken);
                    MainLayout.IsAuthorize = true; 
                    Navigation.NavigateTo("statistic");
                    
                }
                else
                {
                    Auth = new AuthorizationViewModel();
                    MainLayout.IsAuthorize = true;
                }
            }
        }
        protected async Task LogoutAsync()
        {
            await ProtectedSessionStore.DeleteAsync("accessToken");
        }   
    }
}