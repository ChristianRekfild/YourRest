using AutoMapper;
using YourRest.Application.Dto.Models.HotelBooking;
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
            //Booking hotelBookingToInsert = new Booking()              УДАЛИ КОМЕНТАРИЙ
            //{

            //    StartDate = bookingDto.StartDate,                     УДАЛИ КОМЕНТАРИЙ             
            //    EndDate = bookingDto.EndDate,
            //    TotalAmount = bookingDto.TotalAmount,                 УДАЛИ КОМЕНТАРИЙ
            //    AdultNr = bookingDto.AdultNr,
            //    ChildrenNr = bookingDto.ChildrenNr,                   УДАЛИ КОМЕНТАРИЙ
            //    Guest = new Guest()                                   
            //    {
            //        FirstName = bookingDto.FirstName,                 УДАЛИ КОМЕНТАРИЙ
            //        MiddleName = bookingDto.MiddleName,
            //        LastName = bookingDto.LastName,            
            //        DateOfBirth = bookingDto.DateOfBirth,             УДАЛИ КОМЕНТАРИЙ
            //        Email = bookingDto.Email,
            //        PassportNumber = bookingDto.PassportNumber,            
            //        PhoneNumber = bookingDto.PhoneNumber,             УДАЛИ КОМЕНТАРИЙ
            //        SystemId = bookingDto.SystemId,
            //        ExternalId = bookingDto.ExternalId                УДАЛИ КОМЕНТАРИЙ
            //    },
            //    Rooms = new List<RoomEntity>()          <==========   Это ещё одно место где надо создавать объекты
            //    {

            //    }
            //};


            Booking hotelBookingToInsert = mapper.Map<Booking>(bookingDto);                              // Вот для чего нужен AutoMapper
            BookingDto hotelBookingDto = mapper.Map<BookingDto>(hotelBookingToInsert);                   //                    AutoMapper
            BookingWithIdDto hotelBookingWithIdDto = mapper.Map<BookingWithIdDto>(hotelBookingToInsert); //                    AutoMapper



            var RoomIdBooking = await _bookingRepository.
                FindAsync(t => t.RoomId == bookingDto.Rooms., token);

            //  Exception: не может выбрать ниже описанное
            //&&
            //t.DateFrom >= bookingDto.DateFrom &&
            //t.DateFrom < bookingDto.DateTo;
            var AlreadyHaveBooking = RoomIdBooking.Select(x => x)
                .Where(x =>
                (x.DateFrom <= hotelBookingToInsert.StartDate && hotelBookingToInsert.StartDate < x.) ||
                (x.DateFrom < hotelBookingToInsert.DateTo && hotelBookingToInsert.DateTo < x.DateTo) ||
                (hotelBookingToInsert.DateFrom <= x.DateFrom && x.DateTo < hotelBookingToInsert.DateTo)
                ).ToList();

            if (AlreadyHaveBooking.Any())
            {
                throw new InvalidParameterException("Бронирование на выбранные даты невозможно. Комната занята.");
            }

            var savedHotelBooking = await bookingRepository.AddAsync(hotelBookingToInsert, true, token);

            //BookingWithIdDto hotelBookingWithIdDto = new BookingWithIdDto()                      УДАЛИ КОМЕНТАРИЙ
            //{
            //    Id = savedHotelBooking.Id,                                                       УДАЛИ КОМЕНТАРИЙ
            //    StartDate = savedHotelBooking.StartDate,
            //    EndDate = savedHotelBooking.EndDate,
            //    TotalAmount = savedHotelBooking.TotalAmount,                                     УДАЛИ КОМЕНТАРИЙ
            //    AdultNr = savedHotelBooking.AdultNr,
            //    ChildrenNr = savedHotelBooking.ChildrenNr,
            //    FirstName = savedHotelBooking.Guest.FirstName,
            //    MiddleName = savedHotelBooking.Guest.MiddleName,
            //    LastName = savedHotelBooking.Guest.LastName,
            //    DateOfBirth = savedHotelBooking.Guest.DateOfBirth,
            //    Email = savedHotelBooking.Guest.Email,
            //    PassportNumber = savedHotelBooking.Guest.PassportNumber,
            //    PhoneNumber = savedHotelBooking.Guest.PhoneNumber,
            //    SystemId = savedHotelBooking.Guest.SystemId,
            //    ExternalId = savedHotelBooking.Guest.ExternalId,
            //    //Rooms = savedHotelBooking.Rooms
            //};                                                                                    УДАЛИ КОМЕНТАРИЙ       

            return mapper.Map<BookingWithIdDto>(savedHotelBooking);
        }
    }
}
