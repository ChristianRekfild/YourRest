using Moq;
using System.Linq.Expressions;
using YourRest.Application.Dto.Models;
using YourRest.Application.Exceptions;
using YourRest.Application.UseCases;
using YourRest.Infrastructure.Core.Contracts.Models;
using YourRest.Infrastructure.Core.Contracts.Repositories;

namespace YourRest.Application.Tests.UseCases
{
    public class AddAddressToAccommodationUseCaseTests
    {
        private readonly Mock<IAccommodationRepository> _accommodationRepositoryMock;
        private readonly Mock<IAddressRepository> _addressRepositoryMock;
        private readonly Mock<ICityRepository> _cityRepositoryMock;

        private readonly AddAddressToAccommodationUseCase addAddressToAccommodationUseCase;

        public AddAddressToAccommodationUseCaseTests()
        {
            _accommodationRepositoryMock = new Mock<IAccommodationRepository>();
            _addressRepositoryMock = new Mock<IAddressRepository>();
            _cityRepositoryMock = new Mock<ICityRepository>();

            addAddressToAccommodationUseCase = new AddAddressToAccommodationUseCase(
                _accommodationRepositoryMock.Object,
                _addressRepositoryMock.Object,
                _cityRepositoryMock.Object);
        }

        [Fact]
        public async Task Execute_EntityNotFoundException()
        {
            //Arrange
            int accommodationId = 1;

            //Act
            var act = async () => await addAddressToAccommodationUseCase.ExecuteAsync(accommodationId, new AddressWithIdDto());

            //Assert
            var exception = await Assert.ThrowsAsync<EntityNotFoundException>(act);

            //The thrown exception can be used for even more detailed assertions.
            Assert.Equal($"Accommodation with id {accommodationId} not found", exception.Message);

            _accommodationRepositoryMock
                .Verify(r => r.GetWithIncludeAsync(It.IsAny<Expression<Func<AccommodationDto, bool>>>(), It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<AccommodationDto, object>>>()),
                Times.Once);
        }


        [Fact]
        public async Task Execute_ValidationException()
        {
            //Arrange
            _accommodationRepositoryMock
                .Setup(r => r.GetWithIncludeAsync(
                    It.IsAny<Expression<Func<AccommodationDto, bool>>>(),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Expression<Func<AccommodationDto, object>>>()))
                .ReturnsAsync(new[] { new AccommodationDto() { Address = new Infrastructure.Core.Contracts.Models.AddressDto() } });

            int accommodationId = 1;

            //Act
            var act = async () => await addAddressToAccommodationUseCase.ExecuteAsync(accommodationId, new AddressWithIdDto());

            //Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(act);

            //The thrown exception can be used for even more detailed assertions.
            Assert.Equal($"Address for accommodation with id {accommodationId} already exists", exception.Message);

            _accommodationRepositoryMock
                .Verify(r => r.GetWithIncludeAsync(
                    It.IsAny<Expression<Func<AccommodationDto, bool>>>(),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Expression<Func<AccommodationDto, object>>>()),
                Times.Once);
        }

        [Fact]
        public async Task Execute_City_EntityNotFoundException()
        {
            //Arrange
            _accommodationRepositoryMock
                .Setup(r => r.GetWithIncludeAsync(
                    It.IsAny<Expression<Func<AccommodationDto, bool>>>(),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Expression<Func<AccommodationDto, object>>>()))
                .ReturnsAsync(new[] { new AccommodationDto() });

            int accommodationId = 1;
            var addressDto = new AddressWithIdDto { CityId = 1 };

            //Act
            var act = async () => await addAddressToAccommodationUseCase.ExecuteAsync(accommodationId, addressDto);

            //Assert
            var exception = await Assert.ThrowsAsync<EntityNotFoundException>(act);

            //The thrown exception can be used for even more detailed assertions.
            Assert.Equal($"City with id {addressDto.CityId} not found", exception.Message);
        }

        [Fact]
        public async Task Execute_Successful()
        {
            //Arrange
            _accommodationRepositoryMock
                .Setup(r => r.GetWithIncludeAsync(
                    It.IsAny<Expression<Func<AccommodationDto, bool>>>(),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Expression<Func<AccommodationDto, object>>>()))
                .ReturnsAsync(new[] { new AccommodationDto() });
            
            var addressDto = new AddressWithIdDto
            {
                CityId = 1,
                Street = "Street",
                ZipCode = "123456",
                Latitude = 0d,
                Longitude = 0d
            };

            _cityRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CityDto { Id = addressDto.CityId });

            _addressRepositoryMock
                .Setup(r => r.AddAsync(It.IsAny<Infrastructure.Core.Contracts.Models.AddressDto>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Infrastructure.Core.Contracts.Models.AddressDto
                {
                    Id = 1,
                    CityId = addressDto.CityId,
                    Street = addressDto.Street,
                    ZipCode = addressDto.ZipCode,
                    Latitude = addressDto.Latitude,
                    Longitude = addressDto.Longitude });

            int accommodationId = 1;            

            //Act
            var resultDto = await addAddressToAccommodationUseCase.ExecuteAsync(accommodationId, addressDto);

            //Assert
            Assert.Equal(1, resultDto.Id);          
        }
    }
}