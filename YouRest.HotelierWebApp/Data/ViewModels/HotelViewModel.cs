using YouRest.HotelierWebApp.Data.Models;
using YouRest.HotelierWebApp.Data.ViewModels.Interfaces;

namespace YouRest.HotelierWebApp.Data.ViewModels
{
    public class HotelViewModel : IHotelViewModel
    {
        private List<HotelModel> _hotels;
        private HotelModel _currentHotelModel;
        
        public HotelModel CurrentHotel
        {
            get => _currentHotelModel;
            set
            {
                _currentHotelModel = value;
                NotifyHotelChanged();
            }
        }
        public List<HotelModel> Hotels
        {
            get => _hotels;
            set
            {
                _hotels = value;
                NotifyHotelChanged();
            }
        }

        public event Action? PropertyChenged;

        public void AddHotel(HotelModel hotel)
        {
            Hotels.Add(hotel);
        }

        public Task Initialize()
        {
            CurrentHotel = new();
            _hotels = new();
            NotifyHotelChanged();
            return Task.CompletedTask;
        }

        public void RemoveHotel(HotelModel hotel)
        {
            Hotels.Remove(hotel);
            NotifyHotelChanged();
        }

        public void UpdateHotel(HotelModel hotel)
        {
            var foundedHotel = _hotels.SingleOrDefault(h => h.Id == hotel.Id);
            if(foundedHotel is not null)
            {
                foundedHotel = hotel;
            }
            NotifyHotelChanged();
        }
        private void NotifyHotelChanged() => PropertyChenged?.Invoke();
    }
}
