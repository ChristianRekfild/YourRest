using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using YouRest.HotelierWebApp.Data;
using YouRest.HotelierWebApp.Data.Models;
using YouRest.HotelierWebApp.Data.Services.Abstractions;
using YouRest.HotelierWebApp.Data.ViewModels.Interfaces;

namespace YouRest.HotelierWebApp.Pages
{
    public partial class EditHotelComponent : ComponentBase
    {
        #region Fields and Properties
        protected CancellationTokenSource _tokenSource = new();
        protected string inputFileId = Guid.NewGuid().ToString();
        public FluentValidationValidator? EditFormValidator { get; set; }
        public FormHotelModel EditHotelViewModel { get; set; } = new();
        #endregion

        #region Dependeny Injections
        [Inject] public IServiceRepository ServiceRepository { get; set; }
        [Inject] public IHotelViewModel HotelViewModel { get; set; }
        [Inject] public NavigationManager Navigation { get; set; }
        #endregion

        [Parameter] public int CurrentHotelId { get; set; }
        [Parameter] public IEnumerable<CountryModel> Countries { get; set; } = new List<CountryModel>();
        [Parameter] public IEnumerable<RegionModel> Regions { get; set; } = new List<RegionModel>();
        [Parameter] public IEnumerable<CityModel> Cities { get; set; } = new List<CityModel>();
        [Parameter] public IEnumerable<HotelTypeModel> HotelTypes { get; set; } = new List<HotelTypeModel>();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            int regionId = 0;
            int countryId = 0;

            if (HotelViewModel.CurrentHotel == null || HotelViewModel.CurrentHotel.Id == 0) return;

            Cities = await ServiceRepository.CityService.FetchCytiesAsync();
            var city = Cities.FirstOrDefault(c => c.Id == HotelViewModel.CurrentHotel.Address.CityId);
            if (city is not null)
            {
                HotelViewModel.CurrentHotel.City = city;
                HotelViewModel.CurrentHotelModelForm.City = city.Name;
                regionId = city.RegionId;
            }
            Regions = await ServiceRepository.RegionService.FetchRegionsAsync();
            var region = Regions.FirstOrDefault(r => r.Id == regionId);
            if (region is not null)
            {
                HotelViewModel.CurrentHotel.Region = region;
                HotelViewModel.CurrentHotelModelForm.Region = region.Name;
                countryId = region.CountryId;
            }
            Countries = await ServiceRepository.CountryService.FetchCountriesAsync();
            var country = Countries.FirstOrDefault(c => c.Id == countryId);
            if (country is not null)
            {
                HotelViewModel.CurrentHotel.Country = country;
                HotelViewModel.CurrentHotelModelForm.Country = country.Name;
            }

            HotelTypes = await ServiceRepository.HotelTypeService.FetchHotelTypesAsync();
            var hotelType = HotelTypes.SingleOrDefault(ht => ht.Id == HotelViewModel.CurrentHotel.AccommodationTypeId);
            if (hotelType is not null)
            {
                HotelViewModel.CurrentHotel.AccommodationType = hotelType;
                HotelViewModel.CurrentHotelModelForm.HotelType = hotelType.Name;
            }
            EditHotelViewModel = HotelViewModel.CurrentHotelModelForm.DeepCopy();
        }
        public void CancelEdit() => Navigation.NavigateTo("/hotels");
        public void EditHotel()
        {
            HotelViewModel.CurrentHotelModelForm = EditHotelViewModel;
            Navigation.NavigateTo("/hotels");
        }
        public async Task LoadFiles(InputFileChangeEventArgs e)
        {
            if (EditHotelViewModel.Images is null)
                EditHotelViewModel.Images = new();
            var files = e.GetMultipleFiles(maximumFileCount: 5);
            foreach (var file in files)
            {
                using MemoryStream memoryStream = new();
                await file.OpenReadStream(file.Size).CopyToAsync(memoryStream);
                EditHotelViewModel.Images.Add(new()
                {
                    AccommodationId = HotelViewModel.CurrentHotel.Id,
                    FileName = file.Name,
                    Photo = $"data:{file.ContentType}; base64,{Convert.ToBase64String(memoryStream.ToArray())}"
                });

            }
            inputFileId = Guid.NewGuid().ToString();
        }
    }
}
