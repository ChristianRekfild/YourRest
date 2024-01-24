using BlazorBootstrap;
using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using YouRest.HotelierWebApp.Data.Models;
using YouRest.HotelierWebApp.Data.Services.Abstractions;

namespace YouRest.HotelierWebApp.Pages
{
    public partial class CreateHotelPage : ComponentBase, IDisposable
    {
        #region Fields and Properties
        protected CancellationTokenSource _tokenSource = new();
        protected string inputFileId = Guid.NewGuid().ToString();
        public FluentValidationValidator? CreateFormValidator { get; set; }
        public CreateHotelModel CreateHotelViewModel { get; set; } = new();
        #endregion

        #region Dependeny Injections
        [Inject] public IFileService FileService { get; set; }
        [Inject] public ICountryService CountryService { get; set; }
        [Inject] public IRegionService RegionService { get; set; }
        [Inject] public ICityService CityService { get; set; }
        [Inject] public IHotelService HotelService { get; set; }
        [Inject] public IHotelTypeService HotelTypeService { get; set; }
        [Inject] public IAddressService AddressService { get; set; }
        [Inject] public NavigationManager Navigation { get; set; }
        #endregion

        [Parameter] public IEnumerable<CountryModel> Countries { get; set; } = new List<CountryModel>();
        [Parameter] public IEnumerable<RegionModel> Regions { get; set; } = new List<RegionModel>();
        [Parameter] public IEnumerable<CityModel> Cities { get; set; } = new List<CityModel>();
        [Parameter] public IEnumerable<HotelTypeModel> HotelTypes { get; set; } = new List<HotelTypeModel>();

        

        protected async override Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Countries = await CountryService.FetchCountriesAsync();
            Regions = await RegionService.FetchRegionsAsync();
            Cities = await CityService.FetchCytiesAsync();
            HotelTypes = await HotelTypeService.FetchHotelTypesAsync();
            //Images.Add(await FileService.FetchImg("b9bac2d9-0c83-429b-baa6-a44ed5db619d.jpg", "Accomodation"));
        }

        public async Task CreateHotel()
        {
            if (await CreateFormValidator!.ValidateAsync())
            {
                var createdHotel = await HotelService.CreateHotelAsync(
                    new HotelModel()
                    {
                        AccommodationTypeId = HotelTypes.Single(x => x.Name == CreateHotelViewModel.HotelType).Id,
                        Name = CreateHotelViewModel.HotelName,
                        Stars = GetRatingValue(CreateHotelViewModel.HotelRating),
                        Description = CreateHotelViewModel.HotelDescription
                    },
                _tokenSource.Token);

                await AddressService.CreateAddressAsync(
                    new AddressModel()
                    {
                        CityId = Cities.Single(s => s.Name == CreateHotelViewModel.City).Id,
                        Street = CreateHotelViewModel.Address,
                        ZipCode = CreateHotelViewModel.ZipCode,
                    }, createdHotel.Id,
                _tokenSource.Token);

                CreateHotelViewModel = new CreateHotelModel();
                Navigation.NavigateTo("/hotels");
            }
        }

        public async Task LoadFiles(InputFileChangeEventArgs e)
        {
            if (!CreateHotelViewModel.Images.Any()) CreateHotelViewModel.Images.Clear();
            var files = e.GetMultipleFiles(maximumFileCount: 5);
            foreach (var file in files)
            {
                using MemoryStream memoryStream = new();
                await file.OpenReadStream(file.Size).CopyToAsync(memoryStream);
                CreateHotelViewModel.Images.Add($"data:{file.ContentType}; base64,{Convert.ToBase64String(memoryStream.ToArray())}");
            }
            //await FileService.Upload(e.File);
            inputFileId = Guid.NewGuid().ToString();
        }

        private int GetRatingValue(string value) => value switch
        {
            "Без рейтинга" => 0,
            "Одна звезда" => 1,
            "Две звезды" => 2,
            "Три звезды" => 3,
            "Четыре звезды" => 4,
            "Пять звезд" => 5,
            _ => 0
        };

        public void Dispose()
        {
            _tokenSource.Cancel();
            _tokenSource.Dispose();
        }
    }
}
