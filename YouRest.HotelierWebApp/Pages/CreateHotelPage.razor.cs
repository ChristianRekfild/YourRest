using Microsoft.AspNetCore.Components;
using YouRest.HotelierWebApp.Data.Services.Abstractions;
using YouRest.HotelierWebApp.Data.ViewModels;

namespace YouRest.HotelierWebApp.Pages
{
    public partial class CreateHotelPage : ComponentBase
    {
        [Inject] public ICountryService CountryService { get; set; }
        [Inject] public IRegionService RegionService { get; set; }
        [Inject] public ICityService CityService { get; set; }
        [Parameter] public IEnumerable<CountryViewModel> Countries { get; set; } = new List<CountryViewModel>();
        [Parameter] public IEnumerable<RegionViewModel> Regions { get; set; } = new List<RegionViewModel>();
        [Parameter] public IEnumerable<CityViewModel> Cities { get; set; } = new List<CityViewModel>();
        protected async override Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Countries = await CountryService.FetchCountriesAsync();
            Regions = await RegionService.FetchRegionsAsync();
            Cities = await CityService.FetchCytiesAsync();
        }
    }
}
