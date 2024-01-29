using System.Collections.ObjectModel;
using YouRest.HotelierWebApp.Data.Models;

namespace YouRest.HotelierWebApp.Data.ViewModels.Interfaces
{
    public interface IHotelViewModel:  IBaseViewModel
    {
        event Action? OnHotelChanged;
        public HotelModel CurrentHotel { get; set; }
        
        Task Initialize(IEnumerable<HotelModel> hotels);
        ObservableCollection<HotelModel> Hotels { get; set; }
        //void AddHotel(HotelModel hotel);
        //void RemoveHotel(HotelModel hotel);
        //void UpdateHotel(HotelModel hotel);
        //void AddRangeHotel(IEnumerable<HotelModel> hotels);
    }
}
