using YouRest.HotelierWebApp.Data.Services.Abstractions;

namespace YouRest.HotelierWebApp.Data.Services
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly IAddressService addressService;
        private readonly IAuthorizationService authorizationService;
        private readonly ICityService cityService;
        private readonly ICountryService countryService;
        private readonly IFileService fileService;
        private readonly IHotelService hotelService;
        private readonly IHotelTypeService hotelTypeService;
        private readonly IRegionService regionService;

        public ServiceRepository(IAddressService addressService,
            IAuthorizationService authorizationService,
            ICityService cityService,
            ICountryService countryService,
            IFileService fileService,
            IHotelService hotelService,
            IHotelTypeService hotelTypeService,
            IRegionService regionService)
        {
            this.addressService = addressService;
            this.authorizationService = authorizationService;
            this.cityService = cityService;
            this.countryService = countryService;
            this.fileService = fileService;
            this.hotelService = hotelService;
            this.hotelTypeService = hotelTypeService;
            this.regionService = regionService;
        }
        public IAddressService AddressService => addressService;

        public IAuthorizationService AuthorizationService => authorizationService;

        public ICityService CityService => cityService;

        public ICountryService CountryService => countryService;

        public IFileService FileService => fileService;

        public IHotelService HotelService => hotelService;

        public IHotelTypeService HotelTypeService => hotelTypeService;

        public IRegionService RegionService => regionService;
    }
}
