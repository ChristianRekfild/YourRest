using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace YouRest.HotelierWebApp.Shared
{
    public partial class MainLayout
    {
        [Inject] NavigationManager Navigation { get; set; }
        [Inject] ProtectedSessionStorage ProtectedSessionStore { get; set; }
        public static bool IsAuthorize { get; set; }
        public async Task LogoutAsync()
        {
            await ProtectedSessionStore.DeleteAsync("accessToken");
            Navigation.NavigateTo("login");
            IsAuthorize = false;
        }
    }
}
