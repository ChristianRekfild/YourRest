using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Net;
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
        public AuthorizationViewModel Auth { get; set; } = new();
        public RegistrationViewModel Registration { get; set; }
        #endregion

        #region Dependency Injection
        [Inject] IAuthorizationService AuthorizationService { get; set; }
        [Inject] ProtectedSessionStorage ProtectedSessionStore { get; set; }
        [Inject] NavigationManager Navigation { get; set; }
        #endregion
        public async Task LoginAsync()
        {
            if (await LoginValidator!.ValidateAsync())
            {
                var response = await AuthorizationService.LoginAsync(Auth, tokenSource.Token);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var token = await response.Content.ReadFromJsonAsync<TokenViewModel>(cancellationToken: tokenSource.Token);
                    if (token != null)
                    {
                        await ProtectedSessionStore.SetAsync("accessToken", token.AccessToken);
                        MainLayout.IsAuthorize = true;
                        Navigation.NavigateTo("statistic");
                    }
                }
                Auth = new AuthorizationViewModel();
            }
        }

        public async Task LogoutAsync()
        {
            await ProtectedSessionStore.DeleteAsync("accessToken");
            MainLayout.IsAuthorize = false;
            Navigation.NavigateTo("/");
        }

        public void Dispose()
        {
            tokenSource.Cancel();
            tokenSource.Dispose();
        }
    }
}