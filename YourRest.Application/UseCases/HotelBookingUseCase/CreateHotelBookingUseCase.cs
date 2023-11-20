using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using YourRest.Application.Dto;
using YourRest.Application.Dto.Models.HotelBooking;
using YourRest.Application.Dto.Models.Room;
using YourRest.Application.Exceptions;
using YourRest.Application.Interfaces.HotelBooking;
using YourRest.Domain.Entities;
using YourRest.Domain.Repositories;


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

        public async Task<BookingWithIdDto> ExecuteAsync(BookingDto bookingDto, CancellationToken token = default)
        {

            //Booking hotelBookingToInsert = mapper.Map<Booking>(bookingDto);                              // Вот для чего нужен AutoMapper
            //BookingDto hotelBookingDto = mapper.Map<BookingDto>(hotelBookingToInsert);                   //                    AutoMapper
            //BookingWithIdDto hotelBookingWithIdDto = mapper.Map<BookingWithIdDto>(hotelBookingToInsert); //                    AutoMapper

            var rooms = new List<Domain.Entities.Room>();
            foreach (var roomId in bookingDto.Rooms)
            {
                var tempRoom = await roomRepository.GetAsync(roomId);
                if (tempRoom == null)
                {
                    throw new InvalidParameterException("Бронируемой комнаты не существует.");
                }
                rooms.Add(tempRoom);
            }

            if (!rooms.Any())
            {
                throw new InvalidParameterException("Пустая бронь - в брони нет комнат");
            }

            foreach (var room in rooms)
            {
                var alreadyHaveBooking = await bookingRepository.
                FindAsync(x => x.Rooms.Contains(room) && ((x.StartDate <= bookingDto.StartDate && bookingDto.StartDate < x.EndDate) ||
                    (x.StartDate < bookingDto.EndDate && bookingDto.EndDate < x.EndDate) ||
                    (bookingDto.StartDate <= x.StartDate && x.EndDate <= bookingDto.EndDate)), token);

                if (alreadyHaveBooking.Any())
                {
                    throw new InvalidParameterException("Бронирование на выбранные даты невозможно. Комната занята.");
                }
            }

            var customer = await customerRepository.AddAsync(
                new Customer() 
                    {
                        FirstName = bookingDto.FirstName,
                        MiddleName = bookingDto.MiddleName,
                        LastName = bookingDto.LastName,
                        DateOfBirth = bookingDto.DateOfBirth,
                        Email = bookingDto.Email,
                        PassportNumber = bookingDto.PassportNumber,
                        PhoneNumber = bookingDto.PhoneNumber
                    });

            Booking hotelBookingToInsert = new Booking() 
            { 
                StartDate = bookingDto.StartDate,
                EndDate = bookingDto.EndDate,
                Rooms = rooms,
                AdultNumber = bookingDto.AdultNumber,
                ChildrenNumber = bookingDto.ChildrenNumber,
                TotalAmount = bookingDto.TotalAmount,
                Status = BookingStatus.Pending,
                CustomerId = customer.Id,
                    
            };
           
            var savedHotelBooking = await bookingRepository.AddAsync(hotelBookingToInsert, true, token);
            BookingWithIdDto bookingWithIdDto = new BookingWithIdDto()
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

