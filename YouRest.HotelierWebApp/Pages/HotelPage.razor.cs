using Microsoft.AspNetCore.Components;
using YouRest.HotelierWebApp.Data.ViewModels;

namespace YouRest.HotelierWebApp.Pages
{
    public partial class HotelPage : ComponentBase
    {
        [Inject] public NavigationManager Navigation { get; set; }
        public List<HotelViewModel> Hotels { get; set; } =
            new List<HotelViewModel>()
            {
                new() { Id = 1, Name= "Four Seasons", Stars = 5},
                new() { Id = 2, Name= "Руссотель", Stars = 3},
            };
        public void NavigateToCreate() => Navigation.NavigateTo("hotels/create");
    }
}