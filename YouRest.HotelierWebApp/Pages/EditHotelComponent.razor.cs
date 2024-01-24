using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using YouRest.HotelierWebApp.Data.Services.Abstractions;
using YouRest.HotelierWebApp.Data.ViewModels;

namespace YouRest.HotelierWebApp.Pages
{
    public partial class EditHotelComponent: ComponentBase  
    {
        #region Fields and Properties
        protected CancellationTokenSource _tokenSource = new();
        protected string inputFileId = Guid.NewGuid().ToString();
        public FluentValidationValidator? EditFormValidator { get; set; }
        public CreateHotelViewModel EditHotelViewModel { get; set; } = new();
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

        [Parameter] public IEnumerable<CountryViewModel> Countries { get; set; } = new List<CountryViewModel>();
        [Parameter] public IEnumerable<RegionViewModel> Regions { get; set; } = new List<RegionViewModel>();
        [Parameter] public IEnumerable<CityViewModel> Cities { get; set; } = new List<CityViewModel>();
        [Parameter] public IEnumerable<HotelTypeViewModel> HotelTypes { get; set; } = new List<HotelTypeViewModel>();



        public void EditHotel()
        {

        }
        public async Task LoadFiles(InputFileChangeEventArgs e)
        {
            if (!EditHotelViewModel.Images.Any()) EditHotelViewModel.Images.Clear();
            var files = e.GetMultipleFiles(maximumFileCount: 5);
            foreach (var file in files)
            {
                using MemoryStream memoryStream = new();
                await file.OpenReadStream(file.Size).CopyToAsync(memoryStream);
                EditHotelViewModel.Images.Add($"data:{file.ContentType}; base64,{Convert.ToBase64String(memoryStream.ToArray())}");
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
    }
}
