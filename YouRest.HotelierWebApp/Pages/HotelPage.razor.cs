using Microsoft.AspNetCore.Components;
using YouRest.HotelierWebApp.Data.Services.Abstractions;
using YouRest.HotelierWebApp.Data.ViewModels;

namespace YouRest.HotelierWebApp.Pages
{
    public partial class HotelPage : ComponentBase, IDisposable
    {
        [Inject] public NavigationManager Navigation { get; set; }
        [Inject] public IHotelService HotelService { get; set; }
        protected CancellationTokenSource _cts = new ();
        public List<HotelViewModel> Hotels { get; set; } = new();
        protected override async Task OnInitializedAsync()
        {
            Hotels = await HotelService.FetchHotelsAsync(_cts.Token);
            await base.OnInitializedAsync();
        }

        public void NavigateToCreate() => Navigation.NavigateTo("hotels/create");

        public void Dispose()
        {
            _cts.Cancel();
            _cts.Dispose();
        }
    }
}