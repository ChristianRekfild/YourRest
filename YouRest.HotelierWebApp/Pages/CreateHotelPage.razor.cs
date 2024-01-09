using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using YouRest.HotelierWebApp.Data.Services.Abstractions;
using YouRest.HotelierWebApp.Data.ViewModels;

namespace YouRest.HotelierWebApp.Pages
{
    public partial class CreateHotelPage : ComponentBase
    {
        [Inject] public IFileService FileService { get; set; }
        [Inject] public ICountryService CountryService { get; set; }
        [Inject] public IRegionService RegionService { get; set; }
        [Inject] public ICityService CityService { get; set; }
        [Inject] public IHotelService HotelService { get; set; }
        [Inject] public IHotelTypeService HotelTypeService { get; set; }
        [Parameter] public IEnumerable<CountryViewModel> Countries { get; set; } = new List<CountryViewModel>();
        [Parameter] public IEnumerable<RegionViewModel> Regions { get; set; } = new List<RegionViewModel>();
        [Parameter] public IEnumerable<CityViewModel> Cities { get; set; } = new List<CityViewModel>();
        [Parameter] public IEnumerable<HotelTypeViewModel> HotelTypes { get; set; } = new List<HotelTypeViewModel>();
        private string Photo { get; set; }
        private string SelectedCountry { get; set; }
        private string SelectedRegion { get; set; }
        private string SelectedCity { get; set; }
        private string HotelName { get; set; }
        public string SelectedHotelType { get; set; }
        public string SelectedRating { get; set; } = "Без рейтинга";

        protected async override Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Countries = await CountryService.FetchCountriesAsync();
            Regions = await RegionService.FetchRegionsAsync();
            Cities = await CityService.FetchCytiesAsync();
            HotelTypes = await HotelTypeService.FetchHotelTypesAsync();
            Photo = await FileService.FetchImg("8f23968f-686a-4388-9922-08277cb00298.jpg", "Accomodation");
        }
        public async Task CreateHotel()
        {
            var a = GetRatingValue(SelectedRating);
            var selectedCountry = Countries.SingleOrDefault(x => x.Name.Equals(SelectedCountry));
            await HotelService.CreateHotel(new HotelViewModel()
            {
                HotelTypeId = 1,
                Name = "Radisson",
                Stars = 5,
                Description = "The best of Egypt"
            });
        }
        public async Task LoadFiles(InputFileChangeEventArgs e)
        {
          await FileService.Upload(e.File);
        }
        private int GetRatingValue(string value) => value switch
        {
            "Без рейтинга" => 0,
            "Одна звезда" => 1,
            "Две звезды" => 2,
            "Три звезды" => 3,
            "Четыри звезды" => 4,
            "Пять звезд" => 5,
            _ => 0
        };
    }
}
