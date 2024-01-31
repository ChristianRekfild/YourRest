using Microsoft.AspNetCore.Components;
using YouRest.HotelierWebApp.Data.Services.Abstractions;
using YouRest.HotelierWebApp.Data.ViewModels.Interfaces;

namespace YouRest.HotelierWebApp.Pages
{
    public partial class CurrentHotelPage : ComponentBase, IDisposable
    {
        private CancellationTokenSource _tokenSource = new();
        [Inject] public IServiceRepository ServiceRepository { get; set; }
        [Inject] public IHotelViewModel HotelViewModel { get; set; }
        [Parameter] public int CurrentHotelId { get; set; }
        protected override void OnInitialized()
        {
            base.OnInitialized();
            HotelViewModel.OnHotelChanged += HotelViewModel_PropertyChenged;
        }

        private void HotelViewModel_PropertyChenged()
        {
            StateHasChanged();
        }

        public void Dispose()
        {
            _tokenSource.Cancel();
            HotelViewModel.OnHotelChanged -= HotelViewModel_PropertyChenged;
        }
    }
}
