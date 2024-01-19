using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace YouRest.HotelierWebApp.Shared
{
    public partial class MainLayout
    {
        [Inject] NavigationManager NavigationManager { get; set; }
        [Inject] AuthenticationStateProvider Provider { get; set; }
        [CascadingParameter] public Task<AuthenticationState> AuthStat { get; set; }
        
        protected async override Task OnInitializedAsync()
        {
            base.OnInitialized();
            var user = (await AuthStat).User;
            if (!user.Identity.IsAuthenticated)
            {
                NavigationManager.NavigateTo($"/login");
            }
        }

        public async Task LogoutAsync()
        {
            await ((TokenAuthenticationStateProvider)Provider).MakeUserAnonymous();
            NavigationManager.NavigateTo("/login");
        }
    }
}
