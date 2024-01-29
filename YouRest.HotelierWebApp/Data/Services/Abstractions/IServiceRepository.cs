namespace YouRest.HotelierWebApp.Data.Services.Abstractions
{
    public interface IServiceRepository
    {
        public IAddressService AddressService { get; }
        public IAuthorizationService AuthorizationService { get; }
        public ICityService CityService { get; }
        public ICountryService CountryService { get; }
        public IFileService FileService { get; }
        public IHotelService HotelService { get; }
        public IHotelTypeService HotelTypeService { get; }
        public IRegionService RegionService { get; }

    }
}
