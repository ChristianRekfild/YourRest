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
        private string inputFileId = Guid.NewGuid().ToString();
        private List<string> Images { get; set; } = new();
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
            //Images.Add(await FileService.FetchImg("b9bac2d9-0c83-429b-baa6-a44ed5db619d.jpg", "Accomodation"));

        }
        public async Task CreateHotel()
        {
            var a = GetRatingValue(SelectedRating);
            var selectedCountry = Countries.SingleOrDefault(x => x.Name.Equals(SelectedCountry));
            await HotelService.CreateHotel(new HotelViewModel()
            {
                HotelTypeId = 1,
                Name = HotelName,
                Stars = GetRatingValue(SelectedRating),
                Description = "The oldest and most famous hotel in Moscow"
            });
        }
        public async Task LoadFiles(InputFileChangeEventArgs e)
        {
            if(!Images.Any()) Images.Clear();
            var files = e.GetMultipleFiles(maximumFileCount: 5);
            foreach (var file in files)
            {
                using MemoryStream memoryStream = new();
                await file.OpenReadStream(file.Size).CopyToAsync(memoryStream);
                Images.Add($"data:{file.ContentType}; base64,{Convert.ToBase64String(memoryStream.ToArray())}");
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
            "Четыри звезды" => 4,
            "Пять звезд" => 5,
            _ => 0
        };
    }
}
