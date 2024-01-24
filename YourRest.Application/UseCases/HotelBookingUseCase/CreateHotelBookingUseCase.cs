﻿using AutoMapper;
using YourRest.Application.Dto.Models.HotelBooking;
using YourRest.Application.Exceptions;
using YourRest.Application.Interfaces.HotelBooking;
using YourRest.Infrastructure.Core.Contracts.Models;
using YourRest.Infrastructure.Core.Contracts.Repositories;

namespace YourRest.Application.UseCases.HotelBookingUseCase
{
    public class CreateBookingUseCase : ICreateBookingUseCase
    {
        private readonly IBookingRepository bookingRepository;
        private readonly IRoomRepository roomRepository;
        private readonly ICustomerRepository customerRepository;
        private readonly IMapper mapper;

        public CreateBookingUseCase(
            IBookingRepository bookingRepository,
            IMapper mapper,
            ICustomerRepository customerRepository,
            IRoomRepository roomRepository
            )
        {
            this.bookingRepository = bookingRepository;
            this.mapper = mapper;
            this.roomRepository = roomRepository;
            this.customerRepository = customerRepository;
        }

        public async Task<BookingWithIdDto> ExecuteAsync(Dto.Models.HotelBooking.BookingDto bookingDto, CancellationToken token = default)
        {

            //Booking hotelBookingToInsert = mapper.Map<Booking>(bookingDto);                              // Вот для чего нужен AutoMapper
            //BookingDto hotelBookingDto = mapper.Map<BookingDto>(hotelBookingToInsert);                   //                    AutoMapper
            //BookingWithIdDto hotelBookingWithIdDto = mapper.Map<BookingWithIdDto>(hotelBookingToInsert); //                    AutoMapper

            var rooms = (await roomRepository.FindAsync(t => bookingDto.Rooms.Contains(t.Id))).ToList();
             
            foreach (var roomId in bookingDto.Rooms)
            {               
                if (!rooms.Where(r => r.Id == roomId).Any())
                {
                    throw new InvalidParameterException("Бронируемой комнаты не существует.");
                }
            }

            //foreach (var room in rooms)
            //{
            //    var alreadyHaveBooking = await bookingRepository.FindAnyAsync(x => //x.Rooms.Contains(room) && 
            //        ((x.StartDate <= bookingDto.StartDate && bookingDto.StartDate < x.EndDate) ||
            //        (x.StartDate < bookingDto.EndDate && bookingDto.EndDate < x.EndDate) ||
            //        (bookingDto.StartDate <= x.StartDate && x.EndDate <= bookingDto.EndDate)), token);

            //    if (alreadyHaveBooking)
            //    {
            //        throw new InvalidParameterException("Бронирование на выбранные даты невозможно. Комната занята.");
            //    }
            //}

                var bookings = await bookingRepository.FindAsync(x => //x.Rooms.Contains(room) && 
                    ((x.StartDate <= bookingDto.StartDate && bookingDto.StartDate < x.EndDate) ||
                    (x.StartDate < bookingDto.EndDate && bookingDto.EndDate < x.EndDate) ||
                    (bookingDto.StartDate <= x.StartDate && x.EndDate <= bookingDto.EndDate)), token);
            var roomIds = bookings.SelectMany(b => b.Rooms).Select(r => r.Id);

            foreach (var roomId in bookingDto.Rooms)
            {
                if (roomIds.Contains(roomId))
                {
                    throw new InvalidParameterException("Бронирование на выбранные даты невозможно. Комната занята.");
                }
            }            

            var customer = await customerRepository.AddAsync(
                new CustomerDto() 
                    {
                        FirstName = bookingDto.FirstName,
                        MiddleName = bookingDto.MiddleName,
                        LastName = bookingDto.LastName,
                        DateOfBirth = bookingDto.DateOfBirth,
                        Email = bookingDto.Email,
                        PassportNumber = bookingDto.PassportNumber,
                        PhoneNumber = bookingDto.PhoneNumber
                    });

            var hotelBookingToInsert = new Infrastructure.Core.Contracts.Models.BookingDto() 
            { 
                StartDate = bookingDto.StartDate,
                EndDate = bookingDto.EndDate,
                Rooms = rooms,
                AdultNumber = bookingDto.AdultNumber,
                ChildrenNumber = bookingDto.ChildrenNumber,
                TotalAmount = bookingDto.TotalAmount,
                Status = BookingStatusDto.Pending,
                CustomerId = customer.Id,
                    
            };
           
            var savedHotelBooking1 = await bookingRepository.AddAsync(hotelBookingToInsert, true, token);
            var savedHotelBooking = (await bookingRepository.GetWithIncludeAsync(b => b.Id == savedHotelBooking1.Id, token, b => b.Customer, b => b.Rooms)).FirstOrDefault();
            var bookingWithIdDto = new BookingWithIdDto()
            {
                Id = savedHotelBooking.Id,
                StartDate = savedHotelBooking.StartDate,
                EndDate = savedHotelBooking.EndDate,
                Rooms = savedHotelBooking.Rooms.Select(t => t.Id).ToList(),
                AdultNumber = savedHotelBooking.AdultNumber,
                ChildrenNumber = savedHotelBooking.ChildrenNumber,
                TotalAmount = savedHotelBooking.TotalAmount,
                FirstName = savedHotelBooking.Customer.FirstName,
                MiddleName = savedHotelBooking.Customer.MiddleName,
                LastName = savedHotelBooking.Customer.LastName,
                DateOfBirth = savedHotelBooking.Customer.DateOfBirth
            };

            return bookingWithIdDto;
        }
    }
}

