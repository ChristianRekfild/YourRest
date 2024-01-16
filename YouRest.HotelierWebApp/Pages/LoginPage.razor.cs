using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Net;
using YouRest.HotelierWebApp.Data.Services.Abstractions;
using YouRest.HotelierWebApp.Data.ViewModels;

namespace YouRest.HotelierWebApp.Pages
{
    public partial class LoginPage : ComponentBase
    {
        [Inject] IAuthorizationService AuthorizationService { get; set; }
        [Inject] ProtectedSessionStorage ProtectedSessionStore { get; set; }
        [Inject] NavigationManager Navigation { get; set; }
        protected FluentValidationValidator? LoginValidator { get; set; }
        protected FluentValidationValidator? RegisterValidator { get; set; }
        protected AuthorizationViewModel Auth { get; set; } = new();
        protected RegistrationViewModel Registration { get; set; }
        protected bool IsRegistration { get; set; }
        protected bool IsAuthorize { get; set; }
        protected async Task LoginAsync()
        {
            if (await LoginValidator!.ValidateAsync())
            {
                var response = await AuthorizationService.LoginAsync(Auth);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var accessToken = await response.Content.ReadFromJsonAsync<TokenViewModel>();
                    await ProtectedSessionStore.SetAsync("accessToken", accessToken.AccessToken);
                    IsAuthorize = true;
                    Navigation.NavigateTo("statistic");
                }
                else
                {
                    IsAuthorize = false;
                    Auth = new AuthorizationViewModel();
                }
            }
        }
        protected async Task LogoutAsync()
        {
            await ProtectedSessionStore.DeleteAsync("accessToken");
            IsAuthorize = false;
        }   
    }
}