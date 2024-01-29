using System.Collections.ObjectModel;
using YouRest.HotelierWebApp.Data.Models;

namespace YouRest.HotelierWebApp.Data.ViewModels.Interfaces
{
    public interface IHotelViewModel:  IBaseViewModel
    {
        event Action? OnHotelChanged;
        HotelModel CurrentHotel { get; set; }
        FormHotelModel CurrentHotelModelForm { get; set; }

        Task Initialize(IEnumerable<HotelModel> hotels);
        ObservableCollection<HotelModel> Hotels { get; set; }
    }
}
