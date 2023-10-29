using Moq;
using YourRest.Application.UseCases;
using YourRest.Domain.Entities;
using YourRest.Domain.Repositories;
using YourRest.Application.Dto.ViewModels;
using YourRest.Application.Dto.Mappers;
using YourRest.Domain.Models;

namespace YourRest.Application.Tests.UseCases
{
    public class FetchHotelsUseCaseTests
    {
        private readonly Mock<IAccommodationRepository> _accommodationRepositoryMock;
        private readonly Mock<IAccommodationMapper> _mapperMock;

        private readonly FetchAccommodationsUseCase _fetchHotelsUseCase;

        public FetchHotelsUseCaseTests()
        {
            _accommodationRepositoryMock = new Mock<IAccommodationRepository>();
            _mapperMock = new Mock<IAccommodationMapper>();

            _fetchHotelsUseCase = new FetchAccommodationsUseCase(
                _accommodationRepositoryMock.Object,
                _mapperMock.Object
            );
        }

        [Fact]
        public async Task GivenValidViewModel_WhenExecutingUseCase_ThenShouldReturnCorrectDtos()
        {
            var accommodations = new List<Accommodation>
            {
                new Accommodation 
                { 
                    Id = 1, 
                    Name = "Hotel A",
                    Description = "A luxury hotel",
                    Address = new Address
                    {
                        Id = 1,
                        Street = "123 A Street",
                        ZipCode = "12345",
                        CityId = 1
                    },
                    AddressId = 1,
                    AccommodationType = new AccommodationType 
                    {
                        Id = 1,
                        Name = "5 Star"
                    },
                    AccommodationTypeId = 1,
                    Rooms = new List<Room>
                    {
                        new Room { Id = 1, Name = "Room A1", SquareInMeter = 30, RoomType = "Deluxe" },
                        new Room { Id = 2, Name = "Room A2", SquareInMeter = 50, RoomType = "Suite" }
                    }
                },
                new Accommodation 
                { 
                    Id = 2, 
                    Name = "Hotel B",
                    Description = "A budget hotel",
                    Address = new Address
                    {
                        Id = 2,
                        Street = "456 B Blvd",
                        ZipCode = "67890",
                        CityId = 2
                    },
                    AddressId = 2,
                    AccommodationType = new AccommodationType 
                    {
                        Id = 2,
                        Name = "3 Star"
                    },
                    AccommodationTypeId = 2,
                    Rooms = new List<Room>
                    {
                        new Room { Id = 3, Name = "Room B1", SquareInMeter = 20, RoomType = "Standard" },
                        new Room { Id = 4, Name = "Room B2", SquareInMeter = 25, RoomType = "Deluxe" }
                    }
                }
            };

            var viewModel = new FetchAccommodationsViewModel
            {
                CountryIds = new List<int> { 1, 2, 3 },
                CityIds = new List<int> { 10, 11, 12 },
                AccommodationTypesIds = new List<int> { 100, 101, 102 }
            };
            var mockFilterCriteria = new AccommodationFilterCriteria
            {
                CountryIds = new List<int> { 1, 2 },
                CityIds = new List<int> { 10, 11 },
                AccommodationTypesIds = new List<int> { 100, 101 }
            };

            _mapperMock.Setup(mapper => mapper.MapToFilterCriteria(It.IsAny<FetchAccommodationsViewModel>())).Returns(mockFilterCriteria);
            _accommodationRepositoryMock.Setup(repo => repo.GetHotelsByFilter(It.IsAny<AccommodationFilterCriteria>(), It.IsAny<CancellationToken>())).ReturnsAsync(accommodations);

            var result = await _fetchHotelsUseCase.Execute(viewModel);

            _mapperMock.Verify(mapper => mapper.MapToFilterCriteria(viewModel), Times.Once);

            Assert.Equal(accommodations.Count, result.Count());
            Assert.Contains(result, dto => dto.Name == "Hotel A");
            Assert.Contains(result, dto => dto.Name == "Hotel B");
        }
    }
}
