using HotelManagementWebApi.ViewModels;

namespace YorRestServices.Abstracts
{
    public interface IRoomService
    {
        Task AddAdditionalRoomServiceAsync(AdditionalRoomServiceViewModel source);
    }
}
