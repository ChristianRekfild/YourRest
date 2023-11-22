using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourRest.Application.Dto;
using YourRest.Application.Dto.Models.HotelBooking;
using YourRest.Domain.ValueObjects.Bookings;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace YourRest.Application.Interfaces.HotelBooking
{
    public interface IGetBookingDatesByRoomIdUseCase
    {
        Task<List<RoomOccupiedDateDto>> ExecuteAsync(int RoomId, CancellationToken token = default);
    }
}
