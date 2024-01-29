using System.Collections.ObjectModel;
using YouRest.HotelierWebApp.Data.Models;
using YouRest.HotelierWebApp.Data.ViewModels.Interfaces;

namespace YouRest.HotelierWebApp.Data.ViewModels
{
    public class HotelViewModel : BaseViewModel, IHotelViewModel,IDisposable 
    {
        private List<HotelModel> _hotels = new();
        private HotelModel _currentHotelModel;

        public event Action? OnHotelChanged;

        public HotelModel CurrentHotel { get => _currentHotelModel; set => SetProperty(ref _currentHotelModel, value); }
        public ObservableCollection<HotelModel> Hotels { get; set; } = new();

        public HotelViewModel()
        {
            OnChanged += NotifyHotelChanged;
            Hotels.CollectionChanged += Hotels_CollectionChanged;
        }

        private void Hotels_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            NotifyHotelChanged();
        }

        public Task Initialize(IEnumerable<HotelModel> hotels)
        {
            CurrentHotel = new();
            Hotels = new(hotels);
            NotifyHotelChanged();
            return Task.CompletedTask;
        }

        //public void AddHotel(HotelModel hotel)
        //{
        //    if (_hotels.Contains(hotel)) return;
        //    _hotels.Add(hotel);
        //    NotifyHotelChanged();
        //}

        //public void RemoveHotel(HotelModel hotel)
        //{
        //    if (!_hotels.Contains(hotel)) return;
        //    _hotels.Remove(hotel);
        //    NotifyHotelChanged();
        //}

        //public void UpdateHotel(HotelModel hotel)
        //{
        //    var foundedHotel = _hotels.SingleOrDefault(h => h.Id == hotel.Id);
        //    if (foundedHotel is null) return;
        //    foundedHotel = hotel;
        //    NotifyHotelChanged();
        //}
        //public void AddRangeHotel(IEnumerable<HotelModel> hotels)
        //{
        //    _hotels.AddRange(hotels);
        //    NotifyHotelChanged();
        //}

        private void NotifyHotelChanged() => OnHotelChanged?.Invoke();
        
        public void Dispose()
        {
            OnChanged -= NotifyHotelChanged;
        }
    }
}
