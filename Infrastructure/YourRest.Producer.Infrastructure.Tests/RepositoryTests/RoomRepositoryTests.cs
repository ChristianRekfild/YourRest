using YourRest.Domain.Entities;
using YourRest.Domain.Repositories;
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

        private readonly SingletonApiTest fixture;

        public RoomRepositoryTests(SingletonApiTest fixture)
        {
            this.fixture = fixture;
            _roomRepository = new RoomRepository(fixture.DbContext);
            _bookingRepository = new BookingRepository(fixture.DbContext);
            _accommodationRepository = new AccommodationRepository(fixture.DbContext);
            _addressRepository = new AddressRepository(fixture.DbContext);
            _cityRepository = new CityRepository(fixture.DbContext);
            _regionRepository = new RegionRepository(fixture.DbContext);
            _countryRepository = new CountryRepository(fixture.DbContext);
            _accommodationTypeRepository = new AccommodationTypeRepository(fixture.DbContext);
            _roomTypeRepository = new RoomTypeRepository(fixture.DbContext);
            _customerTypeRepository = new CustomerRepository(fixture.DbContext);
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
            var country = await _countryRepository.AddAsync(new Country { Name = "TestCountry" }, true, CancellationToken.None);
            var region = await _regionRepository.AddAsync(new Region { Name = "TestRegion", CountryId = country.Id }, true, CancellationToken.None);
            var city = await PrepareData1Async(new City { Name = "TestCity", RegionId = region.Id }, CancellationToken.None);

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
            var country = await _countryRepository.AddAsync(new Country { Name = "TestCountry" }, true, CancellationToken.None);
            var region = await _regionRepository.AddAsync(new Region { Name = "TestRegion", CountryId = country.Id }, true, CancellationToken.None);
            var city = await PrepareData2Async(new City { Name = "TestCity", RegionId = region.Id }, CancellationToken.None);

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
            var country = await _countryRepository.AddAsync(new Country { Name = "TestCountry" }, true, CancellationToken.None);
            var region = await _regionRepository.AddAsync(new Region { Name = "TestRegion", CountryId = country.Id }, true, CancellationToken.None);
            var accommodation = await PrepareData3Async(new City { Name = "TestCity", RegionId = region.Id }, CancellationToken.None);

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
            var country = await _countryRepository.AddAsync(new Country { Name = "TestCountry" }, true, CancellationToken.None);
            var region = await _regionRepository.AddAsync(new Region { Name = "TestRegion", CountryId = country.Id }, true, CancellationToken.None);
            var accommodation = await PrepareData4Async(new City { Name = "TestCity", RegionId = region.Id }, CancellationToken.None);

            // Act
            var rooms = await _roomRepository.GetRoomsByAccommodationAndDatesAsync(
                DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                DateOnly.FromDateTime(DateTime.Now.AddDays(3)),
                accommodation.Id,
                CancellationToken.None);

            // Assert
            Assert.Empty(rooms);
        }

        private async Task<City> PrepareData1Async(City city, CancellationToken cancellationToken = default)
        {
            // Add City
            var result = await _cityRepository.AddAsync(city, true, cancellationToken);
            // Add Address            
            var address = await _addressRepository.AddAsync(
                new Address
                {
                    CityId = result.Id,
                    Latitude = 1,
                    Longitude = 1,
                    Street = "TestStreet",
                    ZipCode = "123456"
                }, true, cancellationToken);
            // Add AccommodationType
            var accommodationType = await _accommodationTypeRepository.AddAsync(
                new AccommodationType { Name = "TestAccommodationType" },
                true,
                 cancellationToken
                );
            // Add Accommodation
            var accommodation = await _accommodationRepository.AddAsync(
                new Accommodation
                {
                    Name = "TestAccommodation",
                    AddressId = address.Id,
                    AccommodationTypeId = accommodationType.Id
                }, true, cancellationToken);

            // Add RoomType
            var roomType = await _roomTypeRepository.AddAsync(new RoomType { Name = "TestRoomType" }, true, cancellationToken);
            // Add Room
            var room = await _roomRepository.AddAsync(
                new Room
                {
                    Name = "TestRoom",
                    AccommodationId = accommodation.Id,
                    RoomType = roomType.Name
                }, true, cancellationToken);
            var customer = await _customerTypeRepository.AddAsync(new Customer()
            {
                FirstName = "Test",
                MiddleName = "Test1",
                LastName = "Test2",
                DateOfBirth = new DateTime(1950, 11, 5),
            }, true, cancellationToken);
            // Add Booking
            await _bookingRepository.AddAsync(
                new Booking
                {
                    StartDate = new DateOnly(2025, 10, 5),
                    EndDate = new DateOnly(2025, 10, 15),
                    Rooms = new List<Room> { room },
                    TotalAmount = 5000.0m,
                    AdultNumber = 2,
                    ChildrenNumber = 2,
                    CustomerId = customer.Id
                }, true, cancellationToken);
            // Save Changes
            await _cityRepository.SaveChangesAsync(cancellationToken);

            return result;
        }

        private async Task<City> PrepareData2Async(City city, CancellationToken cancellationToken = default)
        {
            // Add City
            var result = await _cityRepository.AddAsync(city, true, cancellationToken);
            // Add Address            
            var address = await _addressRepository.AddAsync(
                new Address
                {
                    CityId = result.Id,
                    Latitude = 1,
                    Longitude = 1,
                    Street = "TestStreet",
                    ZipCode = "123456"
                }, true, cancellationToken);
            // Add AccommodationType
            var accommodationType = await _accommodationTypeRepository.AddAsync(
                new AccommodationType { Name = "TestAccommodationType" },
                true,
                 cancellationToken
                );
            // Add Accommodation
            var accommodation = await _accommodationRepository.AddAsync(
                new Accommodation
                {
                    Name = "TestAccommodation",
                    AddressId = address.Id,
                    AccommodationTypeId = accommodationType.Id
                }, true, cancellationToken);

            // Add RoomType
            var roomType = await _roomTypeRepository.AddAsync(new RoomType { Name = "TestRoomType" }, true, cancellationToken);
            // Add Room
            var room = await _roomRepository.AddAsync(
                new Room
                {
                    Name = "TestRoom",
                    AccommodationId = accommodation.Id,
                    RoomType = roomType.Name
                }, true, cancellationToken);
            var customer = await _customerTypeRepository.AddAsync(new Customer()
            {
                FirstName = "Test",
                MiddleName = "Test1",
                LastName = "Test2",
                DateOfBirth = new DateTime(1950, 11, 5),
            }, true, cancellationToken);
            // Add Booking
            await _bookingRepository.AddAsync(
                new Booking
                {
                    StartDate = DateOnly.FromDateTime(DateTime.Now.AddDays(2)),
                    EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(4)),
                    Rooms = new List<Room> { room },
                    TotalAmount = 5000.0m,
                    AdultNumber = 2,
                    ChildrenNumber = 2,
                    CustomerId = customer.Id
                }, true, cancellationToken);
            // Save Changes
            await _cityRepository.SaveChangesAsync(cancellationToken);

            return result;
        }

        private async Task<Accommodation> PrepareData3Async(City city, CancellationToken cancellationToken = default)
        {
            // Add City
            var cityInDb = await _cityRepository.AddAsync(city, true, cancellationToken);
            // Add Address            
            var address = await _addressRepository.AddAsync(
                new Address
                {
                    CityId = cityInDb.Id,
                    Latitude = 1,
                    Longitude = 1,
                    Street = "TestStreet",
                    ZipCode = "123456"
                }, true, cancellationToken);
            // Add AccommodationType
            var accommodationType = await _accommodationTypeRepository.AddAsync(
                new AccommodationType { Name = "TestAccommodationType" },
                true,
                 cancellationToken
                );
            // Add Accommodation
            var accommodation = await _accommodationRepository.AddAsync(
                new Accommodation
                {
                    Name = "TestAccommodation",
                    AddressId = address.Id,
                    AccommodationTypeId = accommodationType.Id
                }, true, cancellationToken);

            // Add RoomType
            var roomType = await _roomTypeRepository.AddAsync(new RoomType { Name = "TestRoomType" }, true, cancellationToken);
            // Add Room
            var room = await _roomRepository.AddAsync(
                new Room
                {
                    Name = "TestRoom",
                    AccommodationId = accommodation.Id,
                    RoomType = roomType.Name
                }, true, cancellationToken);
            var customer = await _customerTypeRepository.AddAsync(new Customer()
            {
                FirstName = "Test",
                MiddleName = "Test1",
                LastName = "Test2",
                DateOfBirth = new DateTime(1950, 11, 5),
            }, true, cancellationToken);
            // Add Booking
            await _bookingRepository.AddAsync(
                new Booking
                {
                    StartDate = new DateOnly(2025, 10, 5),
                    EndDate = new DateOnly(2025, 10, 15),
                    Rooms = new List<Room> { room },
                    TotalAmount = 5000.0m,
                    AdultNumber = 2,
                    ChildrenNumber = 2,
                    CustomerId = customer.Id
                }, true, cancellationToken);
            // Save Changes
            await _cityRepository.SaveChangesAsync(cancellationToken);

            return accommodation;
        }
        private async Task<Accommodation> PrepareData4Async(City city, CancellationToken cancellationToken = default)
        {
            // Add City
            var cityInDb = await _cityRepository.AddAsync(city, true, cancellationToken);
            // Add Address            
            var address = await _addressRepository.AddAsync(
                new Address
                {
                    CityId = cityInDb.Id,
                    Latitude = 1,
                    Longitude = 1,
                    Street = "TestStreet",
                    ZipCode = "123456"
                }, true, cancellationToken);
            // Add AccommodationType
            var accommodationType = await _accommodationTypeRepository.AddAsync(
                new AccommodationType { Name = "TestAccommodationType" },
                true,
                 cancellationToken
                );
            // Add Accommodation
            var accommodation = await _accommodationRepository.AddAsync(
                new Accommodation
                {
                    Name = "TestAccommodation",
                    AddressId = address.Id,
                    AccommodationTypeId = accommodationType.Id
                }, true, cancellationToken);

            // Add RoomType
            var roomType = await _roomTypeRepository.AddAsync(new RoomType { Name = "TestRoomType" }, true, cancellationToken);
            // Add Room
            var room = await _roomRepository.AddAsync(
                new Room
                {
                    Name = "TestRoom",
                    AccommodationId = accommodation.Id,
                    RoomType = roomType.Name
                }, true, cancellationToken);
            var customer = await _customerTypeRepository.AddAsync(new Customer()
            {
                FirstName = "Test",
                MiddleName = "Test1",
                LastName = "Test2",
                DateOfBirth = new DateTime(1950, 11, 5),
            }, true, cancellationToken);
            // Add Booking
            await _bookingRepository.AddAsync(
                new Booking
                {
                    StartDate = DateOnly.FromDateTime(DateTime.Now.AddDays(2)),
                    EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(4)),
                    Rooms = new List<Room> { room },
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
