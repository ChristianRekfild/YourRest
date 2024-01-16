using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace YouRest.HotelierWebApp.Shared
{
    public partial class MainLayout
    {
        
        [Inject] ProtectedSessionStorage ProtectedSessionStore { get; set; }
        protected bool IsAuthorize { get; set; }
        protected async Task LogoutAsync()
        {
            await ProtectedSessionStore.DeleteAsync("accessToken");
            IsAuthorize = false;
        }
    }
}
