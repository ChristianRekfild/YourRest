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
        public AuthorizationViewModel AuthData { get; set; } = new();
        public RegistrationViewModel Registration { get; set; }
        #endregion

        #region Dependency Injection
        [Inject] IAuthorizationService AuthorizationService { get; set; }
        [Inject] ProtectedLocalStorage LocalStorage { get; set; }
        [Inject] NavigationManager Navigation { get; set; }
        #endregion
        public async Task LoginAsync()
        {
            if (await LoginValidator!.ValidateAsync())
            {
                var securityToken = new SecurityTokenViewModel()
                {
                    UserName = AuthData.Username,
                    AccessToken = AuthData.Password,
                    ExpiredAt = DateTime.UtcNow.AddDays(1),
                };
                await LocalStorage.SetAsync(nameof(SecurityTokenViewModel), securityToken);
                Navigation.NavigateTo("/", true);
                //var response = await AuthorizationService.LoginAsync(AuthData, tokenSource.Token);
                //if (response.StatusCode == HttpStatusCode.OK)
                //{
                //    var securityToken = await response.Content.ReadFromJsonAsync<SecurityTokenViewModel>(cancellationToken: tokenSource.Token);
                //    if (securityToken != null)
                //    {
                //        await LocalStorage.SetAsync(nameof(SecurityTokenViewModel), securityToken);
                //        Navigation.NavigateTo("/");
                //    }
                //}
                AuthData = new AuthorizationViewModel();
            }
        }

        public async Task LogoutAsync()
        {
            await LocalStorage.DeleteAsync(nameof(SecurityTokenViewModel));
            Navigation.NavigateTo("/login");
        }

        public void Dispose()
        {
            tokenSource.Cancel();
            tokenSource.Dispose();
        }
    }
}