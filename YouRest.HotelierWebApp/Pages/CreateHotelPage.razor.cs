using BlazorBootstrap;
using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using YouRest.HotelierWebApp.Data.Models;
using YouRest.HotelierWebApp.Data.Services;
using YouRest.HotelierWebApp.Data.Services.Abstractions;
using YouRest.HotelierWebApp.Data.ViewModels.Interfaces;

namespace YouRest.HotelierWebApp.Pages
{
    public partial class CreateHotelPage : ComponentBase, IDisposable
    {
        #region Fields and Properties
        protected CancellationTokenSource _tokenSource = new();
        protected string inputFileId = Guid.NewGuid().ToString();
        public FluentValidationValidator? CreateFormValidator { get; set; }
        public FormHotelModel CreateHotelViewModel { get; set; } = new();
        #endregion

        #region Dependeny Injections
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public IServiceRepository ServiceRepository { get; set; }
        [Inject] public IHotelViewModel HotelViewModel { get; set; }
        #endregion

        [Parameter] public IEnumerable<CountryModel> Countries { get; set; } = new List<CountryModel>();
        [Parameter] public IEnumerable<RegionModel> Regions { get; set; } = new List<RegionModel>();
        [Parameter] public IEnumerable<CityModel> Cities { get; set; } = new List<CityModel>();
        [Parameter] public IEnumerable<HotelTypeModel> HotelTypes { get; set; } = new List<HotelTypeModel>();



        protected async override Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Countries = await ServiceRepository.CountryService.FetchCountriesAsync(_tokenSource.Token);
            Regions = await ServiceRepository.RegionService.FetchRegionsAsync(_tokenSource.Token);
            Cities = await ServiceRepository.CityService.FetchCytiesAsync(_tokenSource.Token);
            HotelTypes = await ServiceRepository.HotelTypeService.FetchHotelTypesAsync(_tokenSource.Token);
            //Images.Add(await FileService.FetchImg("b9bac2d9-0c83-429b-baa6-a44ed5db619d.jpg", "Accomodation"));
        }

        public async Task CreateHotel()
        {
            if (await CreateFormValidator!.ValidateAsync())
            {
                var createdHotel = await ServiceRepository.HotelService.CreateHotelAsync(
                    new HotelModel()
                    {
                        AccommodationTypeId = HotelTypes.Single(x => x.Name == CreateHotelViewModel.HotelType).Id,
                        Name = CreateHotelViewModel.HotelName,
                        Stars = ServiceRepository.HotelService.ConvertHotelRating(CreateHotelViewModel.HotelRating),
                        Description = CreateHotelViewModel.HotelDescription
                    },
                _tokenSource.Token);

                var createdAddress = await ServiceRepository.AddressService.CreateAddressAsync(
                     new AddressModel()
                     {
                         CityId = Cities.Single(s => s.Name == CreateHotelViewModel.City).Id,
                         Street = CreateHotelViewModel.Address,
                         ZipCode = CreateHotelViewModel.ZipCode,
                     }, createdHotel.Id,
                 _tokenSource.Token);
                HotelViewModel.Hotels.Add(new HotelModel
                {
                    Id = createdHotel.Id,
                    AccommodationTypeId = createdHotel.AccommodationTypeId,
                    Name = createdHotel.Name,
                    Description = createdHotel.Description,
                    Stars = createdHotel.Stars,
                    AccommodationType = createdHotel.AccommodationType,
                    Rooms = createdHotel.Rooms,
                    Address = createdAddress
                });
                if (CreateHotelViewModel.ImagesForLoad is not null)
                {
                    foreach (var image in CreateHotelViewModel.ImagesForLoad)
                    {
                        image.AccommodationId = createdHotel.Id;
                      var a =  await ServiceRepository.FileService.UploadAsync(image, _tokenSource.Token);
                    }
                }
                NavigationManager.NavigateTo("/hotels");
            }
        }

        public async Task LoadFiles(InputFileChangeEventArgs e)
        {
            CreateHotelViewModel.ImagesForLoad = new();
            var files = e.GetMultipleFiles(maximumFileCount: 5);
            foreach (var file in files)
            {
                using MemoryStream memoryStream = new();
                await file.OpenReadStream(file.Size).CopyToAsync(memoryStream);
                CreateHotelViewModel.ImagesForLoad.Add(new()
                {
                    FileName = file.Name,
                    Photo = $"{Convert.ToBase64String(memoryStream.ToArray())}"
                });

            }
            inputFileId = Guid.NewGuid().ToString();
        }

        public void Dispose()
        {
            _tokenSource.Cancel();
            _tokenSource.Dispose();
        }
    }
}
