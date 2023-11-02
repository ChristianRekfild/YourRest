using Moq;
using System.Linq.Expressions;
using YourRest.Application.Dto.Models;
using YourRest.Application.Exceptions;
using YourRest.Application.UseCases;
using YourRest.Domain.Entities;
using YourRest.Domain.Repositories;

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
            var act = async () => await addAddressToAccommodationUseCase.Execute(accommodationId, new AddressDto());

            //Assert
            var exception = await Assert.ThrowsAsync<EntityNotFoundException>(act);

            //The thrown exception can be used for even more detailed assertions.
            Assert.Equal($"Accommodation with id {accommodationId} not found", exception.Message);

            _accommodationRepositoryMock
                .Verify(r => r.GetWithIncludeAsync(It.IsAny<Expression<Func<Accommodation, bool>>>(), It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Accommodation, object>>>()),
                Times.Once);
        }


        [Fact]
        public async Task Execute_ValidationException()
        {
            //Arrange
            _accommodationRepositoryMock
                .Setup(r => r.GetWithIncludeAsync(It.IsAny<Expression<Func<Accommodation, bool>>>(), It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Accommodation, object>>>()))
                .ReturnsAsync(new[] { new Accommodation() { Address = new Address() } });

            int accommodationId = 1;

            //Act
            var act = async () => await addAddressToAccommodationUseCase.Execute(accommodationId, new AddressDto());

            //Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(act);

            //The thrown exception can be used for even more detailed assertions.
            Assert.Equal($"Address for accommodation with id {accommodationId} already exists", exception.Message);

            _accommodationRepositoryMock
                .Verify(r => r.GetWithIncludeAsync(It.IsAny<Expression<Func<Accommodation, bool>>>(), It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Accommodation, object>>>()),
                Times.Once);
        }

        [Fact]
        public async Task Execute_City_EntityNotFoundException()
        {
            //Arrange
            _accommodationRepositoryMock
                .Setup(r => r.GetWithIncludeAsync(It.IsAny<Expression<Func<Accommodation, bool>>>(), It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Accommodation, object>>>()))
                .ReturnsAsync(new[] { new Accommodation() });

            int accommodationId = 1;
            var addressDto = new AddressDto { CityId = 1 };

            //Act
            var act = async () => await addAddressToAccommodationUseCase.Execute(accommodationId, addressDto);

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
                .Setup(r => r.GetWithIncludeAsync(It.IsAny<Expression<Func<Accommodation, bool>>>(), It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Accommodation, object>>>()))
                .ReturnsAsync(new[] { new Accommodation() });
            
            var addressDto = new AddressDto
            {
                CityId = 1,
                Street = "Street",
                ZipCode = "123456",
                Latitude = 0d,
                Longitude = 0d
            };

            _cityRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new City { Id = addressDto.CityId });

            _addressRepositoryMock
                .Setup(r => r.AddAsync(It.IsAny<Address>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Address {
                    Id = 1,
                    CityId = addressDto.CityId,
                    Street = addressDto.Street,
                    ZipCode = addressDto.ZipCode,
                    Latitude = addressDto.Latitude,
                    Longitude = addressDto.Longitude });

            int accommodationId = 1;            

            //Act
            var resultDto = await addAddressToAccommodationUseCase.Execute(accommodationId, addressDto);

            //Assert
            Assert.Equal(1, resultDto.Id);          
        }
    }
}