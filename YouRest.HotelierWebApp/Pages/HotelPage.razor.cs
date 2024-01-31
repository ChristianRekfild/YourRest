using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
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
            if (HotelViewModel.Hotels is null)
            {
                await HotelViewModel.Initialize(_hotels);
            }
            HotelViewModel.Hotels?.Clear();
            foreach (var hotel in _hotels)
            {
                HotelViewModel.Hotels?.Add(hotel);
            }

        }

        public void NavigateToCreate() => Navigation.NavigateTo("hotels/create");

        private void HotelViewModel_PropertyChenged()
        {
            StateHasChanged();
        }
        public async Task SelectedHotel(HotelModel hotel)
        {
            HotelViewModel.CurrentHotel = hotel;
            await hotel.FillHotelModelFormAsync(ServiceRepository, HotelViewModel);
            if (hotel.FilesPath is not null)
            {
                if (HotelViewModel.CurrentHotelModelForm.ImagesForView is null)
                    HotelViewModel.CurrentHotelModelForm.ImagesForView = new();
                else HotelViewModel.CurrentHotelModelForm.ImagesForView.Clear();
                foreach (var file in hotel.FilesPath)
                {
                    var _photo = await ServiceRepository.FileService.FetchAccommodationImg(file);
                    if (_photo is null) { continue; }
                    HotelViewModel.CurrentHotelModelForm.ImagesForView.Add(_photo);
                }
            }
            Navigation.NavigateTo($"hotels/{hotel.Id}");
        }
        public async Task EditHotel(HotelModel hotel)
        {
            HotelViewModel.CurrentHotel = hotel;

            await hotel.FillHotelModelFormAsync(ServiceRepository, HotelViewModel);
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