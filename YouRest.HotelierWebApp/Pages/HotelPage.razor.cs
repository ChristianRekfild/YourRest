using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using System.Net;
using YouRest.HotelierWebApp.Data;
using YouRest.HotelierWebApp.Data.Models;
using YouRest.HotelierWebApp.Data.Services.Abstractions;
using YouRest.HotelierWebApp.Data.ViewModels;
using YouRest.HotelierWebApp.Data.ViewModels.Interfaces;

namespace YouRest.HotelierWebApp.Pages
{
    public partial class HotelPage : ComponentBase, IDisposable
    {
        protected CancellationTokenSource tokenSource = new();
        [Inject] public NavigationManager Navigation { get; set; }
        [Inject] PreloadService PreloadService { get; set; }
        [Inject] public IServiceRepository ServiceRepository { get; set; }
        [Inject] public IHotelViewModel HotelViewModel { get; set; }

        //public List<HotelModel> Hotels { get; set; } = new();
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            HotelViewModel.OnHotelChanged += HotelViewModel_PropertyChenged;
            var _hotels = await ServiceRepository.HotelService.FetchHotelsAsync(tokenSource.Token);
            await HotelViewModel.Initialize(_hotels);
        }

        public void NavigateToCreate() => Navigation.NavigateTo("hotels/create");

        private void HotelViewModel_PropertyChenged()
        {
            StateHasChanged();
        }
        public void SelectedHotel(HotelModel hotel)
        {
            HotelViewModel.CurrentHotel = hotel;
            Navigation.NavigateTo($"hotels/{hotel.Id}");
        }
        public async Task EditHotel(HotelModel hotel)
        {
            HotelViewModel.CurrentHotel = hotel;
            HotelViewModel.CurrentHotelModelForm = await hotel.FillHotelModelFormAsync(ServiceRepository);
            Navigation.NavigateTo($"hotels/{hotel.Id}/edit");
        }
        public async Task RemoveHotel(HotelModel hotel)
        {

            PreloadService.Show(SpinnerColor.Light, $"Идёт удаление...");
            await Task.Delay(1000);
            if (hotel is null)
            {
                PreloadService.Hide();
                return;
            }
            //var response = await HotelService.RemoveHotelAsync(hotel.Address.Id, hotel.Id);
            //if (response.StatusCode == HttpStatusCode.OK)
            //{
                HotelViewModel.Hotels.Remove(hotel);  
            //}
            PreloadService.Hide();
        }
        public void Dispose()
        {
            HotelViewModel.OnHotelChanged -= HotelViewModel_PropertyChenged;
            tokenSource.Cancel();
            tokenSource.Dispose();
        }
    }
}