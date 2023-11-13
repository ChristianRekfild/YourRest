using Moq;
using System;
using System.Linq.Expressions;
using YourRest.Application.Dto.Models;
using YourRest.Application.Dto.Models.HotelBooking;
using YourRest.Application.Exceptions;
using YourRest.Application.UseCases;
using YourRest.Application.UseCases.HotelBookingUseCase;
using YourRest.Domain.Entities;
using YourRest.Domain.Repositories;

namespace YourRest.Application.Tests.UseCases
{
    public class CreateHotelBookingUseCaseTests
    {
        private readonly Mock<IHotelBookingRepository> _hotelBookingRepositoryMock;


        private readonly CreateHotelBookingUseCase createHotelBookingUseCase;

        public CreateHotelBookingUseCaseTests()
        {
            _hotelBookingRepositoryMock = new Mock<IHotelBookingRepository>();

            createHotelBookingUseCase = new CreateHotelBookingUseCase(
                _hotelBookingRepositoryMock.Object);
        }

        [Fact]
        public async Task Execute_WhenDataoccupied_ReturnsStatusCodeError()
        {
            //Arrange
            _hotelBookingRepositoryMock
                .Setup(r => r.FindAsync(It.IsAny<Expression<Func<HotelBooking, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new[] { new HotelBooking()
                        {
                            AccommodationId = 1,
                            DateFrom = new DateTime(2025, 10, 5),
                            DateTo = new DateTime(2025, 10, 15),
                            RoomId = 1,
                            TotalAmount = 5000.0m,
                            AdultNr = 2,
                            ChildrenNr = 2
                        }
                });

            HotelBookingDto newHotelBookingDateToIn = new HotelBookingDto()
            {
                AccommodationId = 1,
                DateFrom = new DateTime(2025, 10, 2),
                DateTo = new DateTime(2025, 10, 12),
                RoomId = 1,
                TotalAmount = 5000.0m,
                AdultNr = 2,
                ChildrenNr = 2
            };
            HotelBookingDto newHotelBookingDateFromIn = new HotelBookingDto()
            {
                AccommodationId = 1,
                DateFrom = new DateTime(2025, 10, 7),
                DateTo = new DateTime(2025, 10, 17),
                RoomId = 1,
                TotalAmount = 5000.0m,
                AdultNr = 2,
                ChildrenNr = 2
            }; 
            HotelBookingDto newHotelBookingAllIn = new HotelBookingDto()
            {
                AccommodationId = 1,
                DateFrom = new DateTime(2025, 10, 1),
                DateTo = new DateTime(2025, 10, 20),
                RoomId = 1,
                TotalAmount = 5000.0m,
                AdultNr = 2,
                ChildrenNr = 2
            };

            //Act
            var actDateToIn = async () => await createHotelBookingUseCase.ExecuteAsync(newHotelBookingDateToIn);
            var actDateFromIn = async () => await createHotelBookingUseCase.ExecuteAsync(newHotelBookingDateFromIn);
            var actAllIn = async () => await createHotelBookingUseCase.ExecuteAsync(newHotelBookingAllIn);

            //Assert
            var exceptionDateToIn = await Assert.ThrowsAsync<InvalidParameterException>(actDateToIn);
            var exceptionDateFromIn = await Assert.ThrowsAsync<InvalidParameterException>(actDateFromIn);
            var exceptionAllIn = await Assert.ThrowsAsync<InvalidParameterException>(actAllIn);
        
            //The thrown exception can be used for even more detailed assertions.
            Assert.Equal("Бронирование на выбранные даты невозможно. Комната занята.", exceptionDateToIn.Message);
            Assert.Equal("Бронирование на выбранные даты невозможно. Комната занята.", exceptionDateFromIn.Message);
            Assert.Equal("Бронирование на выбранные даты невозможно. Комната занята.", exceptionAllIn.Message);
        }
    }
}