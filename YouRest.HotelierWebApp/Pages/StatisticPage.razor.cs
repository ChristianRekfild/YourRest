using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace YouRest.HotelierWebApp.Pages
{
    public partial class StatisticPage : ComponentBase
    {
        [Inject] AuthenticationStateProvider AuthenticationStateProvider { get; set; }
        public string UserId { get; set; }
        protected override void OnInitialized()
        {
        }

        public async Task CheckAuthState()
        {
            UserId = "User Id";
        }

    }
}
