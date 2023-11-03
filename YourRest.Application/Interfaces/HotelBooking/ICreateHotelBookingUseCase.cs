using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourRest.Application.Dto;
using YourRest.Application.Dto.Models.HotelBooking;

namespace YourRest.Application.Interfaces.HotelBooking
{
    public interface ICreateHotelBookingUseCase
    {
        Task<HotelBookingWithIdDto> ExecuteAsync(HotelBookingDto hotelBookingDto, CancellationToken token = default);
    }
}
