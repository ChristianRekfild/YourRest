using Microsoft.AspNetCore.Components;

namespace YouRest.HotelierWebApp.Pages
{
    public partial class HotelPage : ComponentBase
    {
        [Inject] public NavigationManager Navigation { get; set; }
        public void NavigateToCreate()
        {
            Navigation.NavigateTo("hotels/create");
        }
    }

}
