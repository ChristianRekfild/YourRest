using AutoMapper;
using Moq;
using System;
using System.Linq.Expressions;
using YourRest.Application.Dto.Mappers;
using YourRest.Application.Dto.Models;
using YourRest.Application.Dto.Models.HotelBooking;
using YourRest.Application.Dto.Models.Room;
using YourRest.Application.Exceptions;
using YourRest.Application.UseCases;
using YourRest.Application.UseCases.HotelBookingUseCase;
using YourRest.Domain.Entities;
using YourRest.Domain.Repositories;

namespace YourRest.Application.Tests.UseCases
{
    public class CreateHotelBookingUseCaseTests
    {
        private readonly Mock<IBookingRepository> _bookingRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;



        private readonly CreateBookingUseCase createHotelBookingUseCase;

        public CreateHotelBookingUseCaseTests()
        {
            _bookingRepositoryMock = new Mock<IBookingRepository>();
            _mapperMock = new Mock<IMapper>();

            createHotelBookingUseCase = new CreateBookingUseCase(
                _bookingRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Execute_WhenDataoccupied_ReturnsStatusCodeError()
        {
            //Arrange
            _bookingRepositoryMock
                .Setup(r => r.FindAsync(It.IsAny<Expression<Func<Booking, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new[] { new Booking()
                        {
                            StartDate = new DateTime(2025, 10, 5),
                            EndDate = new DateTime(2025, 10, 15),
                            Rooms = new List<Room>() { new Room { Id = 1} },
                            TotalAmount = 5000.0m,
                            AdultNumber = 2,
                            ChildrenNumber = 2
                        }
                });

            BookingDto newHotelBookingDateToIn = new BookingDto()
            {
                StartDate = new DateTime(2025, 10, 2),
                EndDate = new DateTime(2025, 10, 12),
                Rooms = new List<RoomWithIdDto>() { new RoomWithIdDto { Id = 1 } },
                TotalAmount = 5000.0m,
                AdultNumber = 2,
                ChildrenNumber = 2
            };
            BookingDto newHotelBookingDateFromIn = new BookingDto()
            {
                StartDate = new DateTime(2025, 10, 7),
                EndDate = new DateTime(2025, 10, 17),
                Rooms = new List<RoomWithIdDto>() { new RoomWithIdDto { Id = 1 } },
                TotalAmount = 5000.0m,
                AdultNumber = 2,
                ChildrenNumber = 2
            };
            BookingDto newHotelBookingAllIn = new BookingDto()
            {
                StartDate = new DateTime(2025, 10, 1),
                EndDate = new DateTime(2025, 10, 20),
                Rooms = new List<RoomWithIdDto>() { new RoomWithIdDto { Id = 1 } },
                TotalAmount = 5000.0m,
                AdultNumber = 2,
                ChildrenNumber = 2
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