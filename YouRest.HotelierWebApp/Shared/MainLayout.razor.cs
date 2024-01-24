using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using YouRest.HotelierWebApp.Data.Providers;

namespace YouRest.HotelierWebApp.Shared
{
    public partial class MainLayout
    {
        [Inject] NavigationManager NavigationManager { get; set; }
        [Inject] AuthenticationStateProvider Provider { get; set; }
        public bool ShowRegistryPage { get; set; } = false;
        public async Task LogoutAsync()
        {
            await ((CustomAuthValidateProvider)Provider).MakeUserAnonymous();
            NavigationManager.NavigateTo("/login");
        }
        public void ToRegisterPage()
        {

            ShowRegistryPage = true;
        }
        public void ToLoginPage()
        {
           
            ShowRegistryPage = false;
        }
    }
}