using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using YouRest.HotelierWebApp.Data.Providers;

namespace YouRest.HotelierWebApp.Shared
{
    public partial class MainLayout
    {
        [Inject] NavigationManager NavigationManager { get; set; }
        [Inject] AuthenticationStateProvider Provider { get; set; }

        public async Task LogoutAsync()
        {
            await ((CustomAuthValidateProvider)Provider).MakeUserAnonymous();
            NavigationManager.NavigateTo("/login");
        }
    }
}
