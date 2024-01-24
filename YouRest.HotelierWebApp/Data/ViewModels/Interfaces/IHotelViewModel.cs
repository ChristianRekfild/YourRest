using YouRest.HotelierWebApp.Data.Models;

namespace YouRest.HotelierWebApp.Data.ViewModels.Interfaces
{
    public interface IHotelViewModel
    {
        public HotelModel CurrentHotel { get; set; }
        public List<HotelModel> Hotels { get; set; }
        Task Initialize();
        event Action? PropertyChenged;
        void AddHotel (HotelModel hotel);
        void RemoveHotel(HotelModel hotel);
        void UpdateHotel(HotelModel hotel);
    }
}
