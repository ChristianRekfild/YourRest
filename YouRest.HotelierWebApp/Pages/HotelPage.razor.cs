using Microsoft.AspNetCore.Components;
using YouRest.HotelierWebApp.Data.Services.Abstractions;
using YouRest.HotelierWebApp.Data.ViewModels;

namespace YouRest.HotelierWebApp.Pages
{
    public partial class HotelPage : ComponentBase, IDisposable
    {
        protected CancellationTokenSource tokenSource = new();
        [Inject] public NavigationManager Navigation { get; set; }
        [Inject] public IHotelService HotelService { get; set; }
        public List<HotelViewModel> Hotels { get; set; } = new();
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Hotels = await HotelService.FetchHotelsAsync(tokenSource.Token);
        }

        public void NavigateToCreate() => Navigation.NavigateTo("hotels/create");

        public void Dispose()
        {
            tokenSource.Cancel();
            tokenSource.Dispose();
        }
    }
}