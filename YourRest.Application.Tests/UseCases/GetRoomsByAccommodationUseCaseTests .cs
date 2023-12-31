﻿using AutoMapper;
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
    public class GetRoomsByAccommodationUseCaseTests
    {
        private readonly Mock<IBookingRepository> _bookingRepositoryMock;
        private readonly Mock<IRoomRepository> _roomRepositoryMock;
        private readonly Mock<IAccommodationRepository> _accommodationRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;



        private readonly GetRoomsByAccommodationAndBookingDatesUseCase getRoomsByAccommodationAndBookingDatesUseCase;

        public GetRoomsByAccommodationUseCaseTests()
        {
            _roomRepositoryMock = new Mock<IRoomRepository>();
            _accommodationRepositoryMock = new Mock<IAccommodationRepository>();
            _mapperMock = new Mock<IMapper>();

            getRoomsByAccommodationAndBookingDatesUseCase = new GetRoomsByAccommodationAndBookingDatesUseCase(
                _mapperMock.Object,
                _roomRepositoryMock.Object,
                _accommodationRepositoryMock.Object);                        
        }

        [Fact]
        public async Task Execute_WhenNoAccommodation_ReturnsError()
        {
            //Arrange
            _accommodationRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Accommodation {
                    Id = 1,
                    Name = "heroHotelTest",
                    AddressId = 1,
                    AccommodationType = new AccommodationType() { Name = "Luxury" }
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
            var act = async () => await getRoomsByAccommodationAndBookingDatesUseCase.ExecuteAsync(startDate, endDate, 2);

            //Assert
            var exception= await Assert.ThrowsAsync<InvalidParameterException>(act);

            //The thrown exception can be used for even more detailed assertions.
            Assert.Equal("Отеля с таким ID не существует.", exception.Message);

        }
    }
}