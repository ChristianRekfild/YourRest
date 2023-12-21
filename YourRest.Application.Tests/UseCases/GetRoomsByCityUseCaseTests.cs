using AutoMapper;
using Moq;
using System;
using System.Linq.Expressions;
using YourRest.Application.Dto.Mappers;
using YourRest.Application.Dto.Models;
using YourRest.Application.Dto.Models.HotelBooking;
using YourRest.Application.Dto.Models.Room;
using YourRest.Application.Exceptions;
using YourRest.Application.Interfaces.HotelBooking;
using YourRest.Application.UseCases;
using YourRest.Application.UseCases.HotelBookingUseCase;
using YourRest.Domain.Entities;
using YourRest.Domain.Repositories;

namespace YourRest.Application.Tests.UseCases
{
    public class GetRoomsByCityUseCaseTests
    {
        private readonly Mock<IBookingRepository> _bookingRepositoryMock;
        private readonly Mock<IRoomRepository> _roomRepositoryMock;
        private readonly Mock<ICityRepository> _cityRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;



        private readonly GetRoomsByCityAndBookingDatesUseCase getRoomsByCityAndBookingDatesUseCase;

        public GetRoomsByCityUseCaseTests()
        {
            _roomRepositoryMock = new Mock<IRoomRepository>();
            _cityRepositoryMock = new Mock<ICityRepository>();
            _mapperMock = new Mock<IMapper>();

            getRoomsByCityAndBookingDatesUseCase = new GetRoomsByCityAndBookingDatesUseCase(
                _mapperMock.Object,
                _roomRepositoryMock.Object,
                _cityRepositoryMock.Object);                        
        }

        [Fact]
        public async Task Execute_WhenNoAccommodation_ReturnsError()
        {
            //Arrange
            _cityRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new City() 
                {
                    Id = 1,
                    Name = "Moscow",
                    RegionId = 1
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

            var startDate = DateOnly.FromDateTime(DateTime.Now.AddDays(4));
            var endDate = DateOnly.FromDateTime(DateTime.Now.AddDays(10));

            //Act
            var act = async () => await getRoomsByCityAndBookingDatesUseCase.ExecuteAsync(startDate, endDate, 2);

            //Assert
            var exception= await Assert.ThrowsAsync<InvalidParameterException>(act);

            //The thrown exception can be used for even more detailed assertions.
            Assert.Equal("Города с таким ID не существует.", exception.Message);

        }
    }
}