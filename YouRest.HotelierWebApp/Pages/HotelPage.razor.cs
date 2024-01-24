using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using System.ComponentModel;
using YouRest.HotelierWebApp.Data.Models;
using YouRest.HotelierWebApp.Data.Services.Abstractions;
using YouRest.HotelierWebApp.Data.ViewModels.Interfaces;

namespace YouRest.HotelierWebApp.Pages
{
    public partial class HotelPage : ComponentBase, IDisposable
    {
        protected CancellationTokenSource tokenSource = new();
        [Inject] public NavigationManager Navigation { get; set; }
        [Inject] public IHotelService HotelService { get; set; }
        [Inject] public IHotelViewModel HotelViewModel { get; set; }

        //public List<HotelModel> Hotels { get; set; } = new();
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            HotelViewModel.PropertyChenged += HotelViewModel_PropertyChenged;
            await HotelViewModel.Initialize();
            HotelViewModel.Hotels = await HotelService.FetchHotelsAsync(tokenSource.Token);
        }

        public void NavigateToCreate() => Navigation.NavigateTo("hotels/create");

        private void HotelViewModel_PropertyChenged()
        {
            StateHasChanged();
        }
        public void Dispose()
        {
            HotelViewModel.PropertyChenged -= HotelViewModel_PropertyChenged;
            tokenSource.Cancel();
            tokenSource.Dispose();
        }
    }
}