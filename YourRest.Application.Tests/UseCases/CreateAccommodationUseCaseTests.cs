using Amazon.Auth.AccessControlPolicy;
using Moq;
using System.Linq.Expressions;
using YourRest.Application.Dto;
using YourRest.Application.Interfaces;
using YourRest.Application.UseCases;
using YourRest.Infrastructure.Core.Contracts.AuthModels;
using YourRest.Infrastructure.Core.Contracts.Models;
using YourRest.Infrastructure.Core.Contracts.Repositories;

namespace YourRest.Application.Tests.UseCases
{
    public class CreateAccommodationUseCaseTests
    {
        private readonly ICreateAccommodationUseCase _createAccommodationUseCase;

        private readonly Mock<IAccommodationTypeRepository> _mockAccommodationTypeRepository;
        private readonly Mock<IAccommodationRepository> _mockAccommodationRepository;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IUserAccommodationRepository> _mockUserAccommodationRepository;

        public CreateAccommodationUseCaseTests()
        {
            _mockAccommodationTypeRepository = new Mock<IAccommodationTypeRepository>();
            _mockAccommodationRepository = new Mock<IAccommodationRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockUserAccommodationRepository = new Mock<IUserAccommodationRepository>();

            _createAccommodationUseCase = new CreateAccommodationUseCase(
                _mockAccommodationTypeRepository.Object,
                _mockAccommodationRepository.Object,
                _mockUserRepository.Object,
                _mockUserAccommodationRepository.Object
                );
        }

        [Fact]
        public async Task CreateAccommodationUseCase_ExecuteAsync_Test()
        {
            // Arrange
            _mockAccommodationTypeRepository.Setup(m => m.GetAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Infrastructure.Core.Contracts.Models.AccommodationTypeDto());
            
            _mockAccommodationRepository.Setup(m => m.AddAsync(It.IsAny<Infrastructure.Core.Contracts.Models.AccommodationDto>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Infrastructure.Core.Contracts.Models.AccommodationDto());

            _mockUserRepository.Setup(m => m.FindAsync(It.IsAny<Expression<Func<Infrastructure.Core.Contracts.Models.UserDto, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Infrastructure.Core.Contracts.Models.UserDto>() { new Infrastructure.Core.Contracts.Models.UserDto() });

            _mockUserAccommodationRepository.Setup(m => m.AddAsync(It.IsAny<UserAccommodationDto>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new UserAccommodationDto());

            CreateAccommodationDto createAccommodationDto = new();
            string userKeyCloakId = string.Empty;

            // Act
            var dto = await _createAccommodationUseCase.ExecuteAsync(createAccommodationDto, userKeyCloakId, CancellationToken.None);

            // Assert
            Assert.NotNull(dto);
        }
    }
}
