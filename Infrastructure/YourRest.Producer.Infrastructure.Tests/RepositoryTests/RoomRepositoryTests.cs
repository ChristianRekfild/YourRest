using AutoMapper;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using YourRest.Infrastructure.Core.Contracts.Models;
using YourRest.Infrastructure.Core.Contracts.Repositories;
using YourRest.Producer.Infrastructure.Repositories;
using YourRest.Producer.Infrastructure.Tests.Fixtures;

namespace YourRest.Producer.Infrastructure.Tests.RepositoryTests
{
    [Collection(nameof(SingletonApiTest))]
    public class RoomRepositoryTests
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IAccommodationRepository _accommodationRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IRegionRepository _regionRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IAccommodationTypeRepository _accommodationTypeRepository;
        private readonly IRoomTypeRepository _roomTypeRepository;
        private readonly ICustomerRepository _customerTypeRepository;
        private readonly IMapper _mapper;

        private readonly SingletonApiTest fixture;

        public RoomRepositoryTests(SingletonApiTest fixture)
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<ProducerInfrastructureMapper>());
            _mapper = config.CreateMapper();
            
            this.fixture = fixture;
            _roomRepository = new RoomRepository(fixture.DbContext, _mapper);
            _bookingRepository = new BookingRepository(fixture.DbContext, _mapper);
            _accommodationRepository = new AccommodationRepository(fixture.DbContext, _mapper);
            _addressRepository = new AddressRepository(fixture.DbContext, _mapper);
            _cityRepository = new CityRepository(fixture.DbContext, _mapper);
            _regionRepository = new RegionRepository(fixture.DbContext, _mapper);
            _countryRepository = new CountryRepository(fixture.DbContext, _mapper);
            _accommodationTypeRepository = new AccommodationTypeRepository(fixture.DbContext, _mapper);
            _roomTypeRepository = new RoomTypeRepository(fixture.DbContext, _mapper);
            _customerTypeRepository = new CustomerRepository(fixture.DbContext, _mapper);
        }

        [Fact]
        public async Task EmptyDb_GetRoomsByCityAndDatesAsyncTest()
        {
            // Act
            var rooms = await _roomRepository.GetRoomsByCityAndDatesAsync(
                DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                DateOnly.FromDateTime(DateTime.Now.AddDays(3)),
                1,
                CancellationToken.None);

            // Assert
            Assert.Empty(rooms);
        }


        [Fact]
        public async Task GetRoomsByCityAndDatesAsyncWithFreeRoomTest()
        {
            // Arrange
            var country = await _countryRepository.AddAsync(new CountryDto { Name = "TestCountry" }, true, CancellationToken.None);
            var region = await _regionRepository.AddAsync(new RegionDto { Name = "TestRegion", CountryId = country.Id }, true, CancellationToken.None);
            var city = await PrepareData1Async(new CityDto { Name = "TestCity", RegionId = region.Id }, CancellationToken.None);

            // Act
            var rooms = await _roomRepository.GetRoomsByCityAndDatesAsync(
                DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                DateOnly.FromDateTime(DateTime.Now.AddDays(3)),
                city.Id,
                CancellationToken.None);

            // Assert
            Assert.NotEmpty(rooms);
        }

        [Fact]
        public async Task GetRoomsByCityAndDatesAsyncWithoutFreeRoomTest()
        {
            // Arrange
            var country = await _countryRepository.AddAsync(new CountryDto { Name = "TestCountry" }, true, CancellationToken.None);
            var region = await _regionRepository.AddAsync(new RegionDto { Name = "TestRegion", CountryId = country.Id }, true, CancellationToken.None);
            var city = await PrepareData2Async(new CityDto { Name = "TestCity", RegionId = region.Id }, CancellationToken.None);

            // Act
            var rooms = await _roomRepository.GetRoomsByCityAndDatesAsync(
                DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                DateOnly.FromDateTime(DateTime.Now.AddDays(3)),
                city.Id,
                CancellationToken.None);

            // Assert
            Assert.Empty(rooms);
        }

        [Fact]
        public async Task EmptyDb_GetRoomsByAccomodationAndDatesAsyncTest()
        {
            // Act
            var rooms = await _roomRepository.GetRoomsByAccommodationAndDatesAsync(
                DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                DateOnly.FromDateTime(DateTime.Now.AddDays(3)),
                1,
                CancellationToken.None);

            // Assert
            Assert.Empty(rooms);
        }


        [Fact]
        public async Task GetRoomsByAccomodationAndDatesAsyncWithFreeRoomTest()
        {
            // Arrange
            JsonSerializerOptions options = new()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true
            };           

            var country = await _countryRepository.AddAsync(new CountryDto { Name = "TestCountry" }, true, CancellationToken.None);
            Debug.WriteLine("Country {0}: {1}", country.GetType(), JsonSerializer.Serialize(country, options));
            var region = await _regionRepository.AddAsync(new RegionDto { Name = "TestRegion", CountryId = country.Id }, true, CancellationToken.None);
            Debug.WriteLine("Region {0}: {1}", region.GetType(), JsonSerializer.Serialize(region, options));

            var accommodation = await PrepareData3Async(new CityDto { Name = "TestCity", RegionId = region.Id }, CancellationToken.None);

            // Act
            var rooms = await _roomRepository.GetRoomsByAccommodationAndDatesAsync(
                DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                DateOnly.FromDateTime(DateTime.Now.AddDays(3)),
                accommodation.Id,
                CancellationToken.None);

            // Assert
            Assert.NotEmpty(rooms);
        }

        [Fact]
        public async Task GetRoomsByAccommodationAndDatesAsyncWithoutFreeRoomTest()
        {
            // Arrange
            var country = await _countryRepository.AddAsync(new CountryDto { Name = "TestCountry" }, true, CancellationToken.None);
            var region = await _regionRepository.AddAsync(new RegionDto { Name = "TestRegion", CountryId = country.Id }, true, CancellationToken.None);
            var accommodation = await PrepareData4Async(new CityDto { Name = "TestCity", RegionId = region.Id }, CancellationToken.None);

            // Act
            var rooms = await _roomRepository.GetRoomsByAccommodationAndDatesAsync(
                DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                DateOnly.FromDateTime(DateTime.Now.AddDays(3)),
                accommodation.Id,
                CancellationToken.None);

            // Assert
            Assert.Empty(rooms);
        }

        private async Task<CityDto> PrepareData1Async(CityDto city, CancellationToken cancellationToken = default)
        {
            // Add City
            var result = await _cityRepository.AddAsync(city, true, cancellationToken);
            // Add Address            
            var address = await _addressRepository.AddAsync(
                new AddressDto
                {
                    CityId = result.Id,
                    Latitude = 1,
                    Longitude = 1,
                    Street = "TestStreet",
                    ZipCode = "123456"
                }, true, cancellationToken);
            // Add AccommodationType
            var accommodationType = await _accommodationTypeRepository.AddAsync(
                new AccommodationTypeDto { Name = "TestAccommodationType" },
                true,
                 cancellationToken
                );
            // Add Accommodation
            var accommodation = await _accommodationRepository.AddAsync(
                new AccommodationDto
                {
                    Name = "TestAccommodation",
                    AddressId = address.Id,
                    AccommodationTypeId = accommodationType.Id
                }, true, cancellationToken);

            // Add RoomType
            var roomType = await _roomTypeRepository.AddAsync(new RoomTypeDto { Name = "TestRoomType" }, true, cancellationToken);
            // Add Room
            var room = await _roomRepository.AddAsync(
                new RoomDto
                {
                    Name = "TestRoom",
                    AccommodationId = accommodation.Id,
                    RoomTypeId = roomType.Id
                }, true, cancellationToken);
            var customer = await _customerTypeRepository.AddAsync(new CustomerDto()
            {
                FirstName = "Test",
                MiddleName = "Test1",
                LastName = "Test2",
                DateOfBirth = new DateTime(1950, 11, 5),
            }, true, cancellationToken);
            // Add Booking
            await _bookingRepository.AddAsync(
                new BookingDto
                {
                    StartDate = new DateOnly(2025, 10, 5),
                    EndDate = new DateOnly(2025, 10, 15),
                    Rooms = new List<RoomDto> { room },
                    TotalAmount = 5000.0m,
                    AdultNumber = 2,
                    ChildrenNumber = 2,
                    CustomerId = customer.Id
                }, true, cancellationToken);
            // Save Changes
            await _cityRepository.SaveChangesAsync(cancellationToken);

            return result;
        }

        private async Task<CityDto> PrepareData2Async(CityDto city, CancellationToken cancellationToken = default)
        {
            // Add City
            var result = await _cityRepository.AddAsync(city, true, cancellationToken);
            // Add Address            
            var address = await _addressRepository.AddAsync(
                new AddressDto
                {
                    CityId = result.Id,
                    Latitude = 1,
                    Longitude = 1,
                    Street = "TestStreet",
                    ZipCode = "123456"
                }, true, cancellationToken);
            // Add AccommodationType
            var accommodationType = await _accommodationTypeRepository.AddAsync(
                new AccommodationTypeDto { Name = "TestAccommodationType" },
                true,
                 cancellationToken
                );
            // Add Accommodation
            var accommodation = await _accommodationRepository.AddAsync(
                new AccommodationDto
                {
                    Name = "TestAccommodation",
                    AddressId = address.Id,
                    AccommodationTypeId = accommodationType.Id
                }, true, cancellationToken);

            // Add RoomType
            var roomType = await _roomTypeRepository.AddAsync(new RoomTypeDto { Name = "TestRoomType" }, true, cancellationToken);
            // Add Room
            var room = await _roomRepository.AddAsync(
                new RoomDto
                {
                    Name = "TestRoom",
                    AccommodationId = accommodation.Id,
                    RoomTypeId = roomType.Id
                }, true, cancellationToken);
            var customer = await _customerTypeRepository.AddAsync(new CustomerDto()
            {
                FirstName = "Test",
                MiddleName = "Test1",
                LastName = "Test2",
                DateOfBirth = new DateTime(1950, 11, 5),
            }, true, cancellationToken);
            // Add Booking
            await _bookingRepository.AddAsync(
                new BookingDto
                {
                    StartDate = DateOnly.FromDateTime(DateTime.Now.AddDays(2)),
                    EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(4)),
                    Rooms = new List<RoomDto> { room },
                    TotalAmount = 5000.0m,
                    AdultNumber = 2,
                    ChildrenNumber = 2,
                    CustomerId = customer.Id
                }, true, cancellationToken);
            // Save Changes
            await _cityRepository.SaveChangesAsync(cancellationToken);

            return result;
        }

        private async Task<AccommodationDto> PrepareData3Async(CityDto city, CancellationToken cancellationToken = default)
        {
            // Add City
            var cityInDb = await _cityRepository.AddAsync(city, true, cancellationToken);
            // Add Address            
            var address = await _addressRepository.AddAsync(
                new AddressDto
                {
                    CityId = cityInDb.Id,
                    Latitude = 1,
                    Longitude = 1,
                    Street = "TestStreet",
                    ZipCode = "123456"
                }, true, cancellationToken);
            // Add AccommodationType
            var accommodationType = await _accommodationTypeRepository.AddAsync(
                new AccommodationTypeDto { Name = "TestAccommodationType" },
                true,
                 cancellationToken
                );
            // Add Accommodation
            var accommodation = await _accommodationRepository.AddAsync(
                new AccommodationDto
                {
                    Name = "TestAccommodation",
                    AddressId = address.Id,
                    AccommodationTypeId = accommodationType.Id
                }, true, cancellationToken);

            // Add RoomType
            var roomType = await _roomTypeRepository.AddAsync(new RoomTypeDto { Name = "TestRoomType" }, true, cancellationToken);
            // Add Room
            var room = await _roomRepository.AddAsync(
                new RoomDto
                {
                    Name = "TestRoom",
                    AccommodationId = accommodation.Id,                    
                    RoomTypeId = roomType.Id
                }, true, cancellationToken);
            var customer = await _customerTypeRepository.AddAsync(new CustomerDto()
            {
                FirstName = "Test",
                MiddleName = "Test1",
                LastName = "Test2",
                DateOfBirth = new DateTime(1950, 11, 5),
            }, true, cancellationToken);
            // Add Booking
            await _bookingRepository.AddAsync(
                new BookingDto
                {
                    StartDate = new DateOnly(2025, 10, 5),
                    EndDate = new DateOnly(2025, 10, 15),
                    //Rooms = new List<RoomDto> { room },
                    TotalAmount = 5000.0m,
                    AdultNumber = 2,
                    ChildrenNumber = 2,
                    CustomerId = customer.Id
                }, true, cancellationToken);
            // Save Changes
            await _cityRepository.SaveChangesAsync(cancellationToken);

            return accommodation;
        }
        private async Task<AccommodationDto> PrepareData4Async(CityDto city, CancellationToken cancellationToken = default)
        {
            // Add City
            var cityInDb = await _cityRepository.AddAsync(city, true, cancellationToken);
            // Add Address            
            var address = await _addressRepository.AddAsync(
                new AddressDto
                {
                    CityId = cityInDb.Id,
                    Latitude = 1,
                    Longitude = 1,
                    Street = "TestStreet",
                    ZipCode = "123456"
                }, true, cancellationToken);
            // Add AccommodationType
            var accommodationType = await _accommodationTypeRepository.AddAsync(
                new AccommodationTypeDto { Name = "TestAccommodationType" },
                true,
                 cancellationToken
                );
            // Add Accommodation
            var accommodation = await _accommodationRepository.AddAsync(
                new AccommodationDto
                {
                    Name = "TestAccommodation",
                    AddressId = address.Id,
                    AccommodationTypeId = accommodationType.Id
                }, true, cancellationToken);

            // Add RoomType
            var roomType = await _roomTypeRepository.AddAsync(new RoomTypeDto { Name = "TestRoomType" }, true, cancellationToken);
            // Add Room
            var room = await _roomRepository.AddAsync(
                new RoomDto
                {
                    Name = "TestRoom",
                    AccommodationId = accommodation.Id,
                    RoomTypeId = roomType.Id
                }, true, cancellationToken);
            var customer = await _customerTypeRepository.AddAsync(new CustomerDto()
            {
                FirstName = "Test",
                MiddleName = "Test1",
                LastName = "Test2",
                DateOfBirth = new DateTime(1950, 11, 5),
            }, true, cancellationToken);
            // Add Booking
            await _bookingRepository.AddAsync(
                new BookingDto
                {
                    StartDate = DateOnly.FromDateTime(DateTime.Now.AddDays(2)),
                    EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(4)),
                    Rooms = new List<RoomDto> { room },
                    TotalAmount = 5000.0m,
                    AdultNumber = 2,
                    ChildrenNumber = 2,
                    CustomerId = customer.Id
                }, true, cancellationToken);
            // Save Changes
            await _cityRepository.SaveChangesAsync(cancellationToken);

            return accommodation;
        }
    }
}
