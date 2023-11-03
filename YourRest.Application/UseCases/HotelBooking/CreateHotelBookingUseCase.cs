using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourRest.Application.Dto;
using YourRest.Application.Dto.Models.HotelBooking;
using YourRest.Application.Interfaces.HotelBooking;
using YourRest.Domain.Entities;
using YourRest.Domain.Repositories;

namespace YourRest.Application.UseCases.HotelBooking
{
    public class CreateHotelBookingUseCase : ICreateHotelBookingUseCase
    { 
        private readonly IHotelBookingRepository hotelBookingRepository;

        public CreateHotelBookingUseCase(IHotelBookingRepository hotelBookingRepository)
        {
            this.hotelBookingRepository = hotelBookingRepository;
        }

        public async Task<HotelBookingWithIdDto> ExecuteAsync(HotelBookingDto hotelBookingDto, CancellationToken token = default)
        {
            HotelBooking hotelBooking = new HotelBooking()
            {

            }

        }

    }
}
