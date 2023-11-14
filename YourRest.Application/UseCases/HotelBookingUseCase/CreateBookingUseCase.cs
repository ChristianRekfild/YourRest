using AutoMapper;
using System.Linq;
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
        private readonly IMapper mapper;

        public CreateBookingUseCase(IBookingRepository bookingRepository, IMapper mapper)
        {
            this.bookingRepository = bookingRepository;
            this.mapper = mapper;
        }

        public async Task<BookingWithIdDto> ExecuteAsync(BookingDto bookingDto, CancellationToken token = default)
        {

            Booking hotelBookingToInsert = mapper.Map<Booking>(bookingDto);                              // Вот для чего нужен AutoMapper
            BookingDto hotelBookingDto = mapper.Map<BookingDto>(hotelBookingToInsert);                   //                    AutoMapper
            BookingWithIdDto hotelBookingWithIdDto = mapper.Map<BookingWithIdDto>(hotelBookingToInsert); //                    AutoMapper

            foreach (RoomWithIdDto RoomDto in hotelBookingDto.Rooms)
            {
                Domain.Entities.Room room = mapper.Map<Domain.Entities.Room>(RoomDto);
                var AlreadyHaveBooking = (await bookingRepository.
                FindAsync(x => x.Rooms.Contains(room)&&(
                (x.StartDate <= hotelBookingToInsert.StartDate && hotelBookingToInsert.EndDate < x.StartDate) ||
                (x.StartDate < hotelBookingToInsert.EndDate && hotelBookingToInsert.EndDate < x.EndDate) ||
                (hotelBookingToInsert.StartDate <= x.StartDate && x.EndDate < hotelBookingToInsert.EndDate)), token)).Any();

                if (AlreadyHaveBooking)
                {
                    throw new InvalidParameterException("Бронирование на выбранные даты невозможно. Комната занята.");
                }
            }

            var savedHotelBooking = await bookingRepository.AddAsync(hotelBookingToInsert, true, token);

            return mapper.Map<BookingWithIdDto>(savedHotelBooking);
        }
    }
}
