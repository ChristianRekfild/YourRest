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
        private readonly Mock<IRoomRepository> _roomRepositoryMock;
        private readonly Mock<ICustomerRepository> _customerRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;



        private readonly CreateBookingUseCase createHotelBookingUseCase;

        public CreateHotelBookingUseCaseTests()
        {
            _bookingRepositoryMock = new Mock<IBookingRepository>();
            _roomRepositoryMock = new Mock<IRoomRepository>();
            _customerRepositoryMock = new Mock<ICustomerRepository>();
            _mapperMock = new Mock<IMapper>();

            createHotelBookingUseCase = new CreateBookingUseCase(
                _bookingRepositoryMock.Object,
                _mapperMock.Object,
                _customerRepositoryMock.Object,
                _roomRepositoryMock.Object);
        }
        [Fact]
        public async Task Execute_WhenNoRoom_ReturnsError()
        {
            //Arrange
            _roomRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Room()
                {
                    Id = 1,
                    AccommodationId = 1,
                    Name = "DeluxeRoom",
                    RoomType = new RoomType() { Name = "ZBS" },
                    SquareInMeter = 1,
                    Capacity = 20
                }
             );

            BookingDto newBooking = new BookingDto()
            {
                StartDate = new DateOnly(2025, 10, 2),
                EndDate = new DateOnly(2025, 10, 12),
                Rooms = new List<int>() { 1, 2 },
                TotalAmount = 5000.0m,
                AdultNumber = 2,
                ChildrenNumber = 2
            };

            //Act
            var act = async () => await createHotelBookingUseCase.ExecuteAsync(newBooking);

            //Assert
            var exception= await Assert.ThrowsAsync<InvalidParameterException>(act);

            //The thrown exception can be used for even more detailed assertions.
            Assert.Equal("Бронируемой комнаты не существует.", exception.Message);

        }

        [Fact]
        public async Task Execute_WhenDataoccupied_ReturnsStatusCodeError()
        {
            //Arrange

            _bookingRepositoryMock
                .Setup(r => r.FindAnyAsync(It.IsAny<Expression<Func<Booking, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync( true
            );

            _roomRepositoryMock
                .Setup(r => r.FindAsync(It.IsAny<Expression<Func<Room, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new [] { new Room()
                {
                    Id = 1,
                    AccommodationId = 1,
                    Name = "DeluxeRoom",
                    RoomType = new RoomType() { Name = "ZBS" },
                    SquareInMeter = 1,
                    Capacity = 20
                } }
            );          

            BookingDto newHotelBookingDateToIn = new BookingDto()
            {
                StartDate = new DateOnly(2025, 10, 2),
                EndDate = new DateOnly(2025, 10, 12),
                Rooms = new List<int>() { 1 },
                TotalAmount = 5000.0m,
                AdultNumber = 2,
                ChildrenNumber = 2,
                FirstName = "Test",
                MiddleName = "Test",
                LastName = "Test",
                DateOfBirth = new DateTime(1950, 11, 5),
            };

            //Act
            var actDateToIn = async () => await createHotelBookingUseCase.ExecuteAsync(newHotelBookingDateToIn);

            //Assert
            var exceptionDateToIn = await Assert.ThrowsAsync<InvalidParameterException>(actDateToIn);

            //The thrown exception can be used for even more detailed assertions.
            Assert.Equal("Бронирование на выбранные даты невозможно. Комната занята.", exceptionDateToIn.Message);

        }
    }
}