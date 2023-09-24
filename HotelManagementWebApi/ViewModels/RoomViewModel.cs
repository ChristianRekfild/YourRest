namespace HotelManagementWebApi.ViewModels
{
    public class RoomViewModel
    {
        public RoomViewModel() => AdditionalRoomServices = new List<AdditionalRoomServiceViewModel>();

        public int Id { get; set; }
        public string RoomName { get; set; }
        public ICollection<AdditionalRoomServiceViewModel> AdditionalRoomServices { get; set; }
    }
}
