using System.Collections.ObjectModel;
using YouRest.HotelierWebApp.Data.Models;
using YouRest.HotelierWebApp.Data.ViewModels.Interfaces;

namespace YouRest.HotelierWebApp.Data.ViewModels
{
    public class HotelViewModel : BaseViewModel, IHotelViewModel, IDisposable
    {
        private List<HotelModel> _hotels = new();
        private HotelModel _currentHotelModel;
        private FormHotelModel _currentFormHotelModel;

        public FormHotelModel CurrentHotelModelForm { get => _currentFormHotelModel; set => SetProperty(ref _currentFormHotelModel, value); }
        public HotelModel CurrentHotel { get => _currentHotelModel; set => SetProperty(ref _currentHotelModel, value); }
        public ObservableCollection<HotelModel>? Hotels { get; set; }
        
        public event Action? OnHotelChanged;

        public HotelViewModel()
        {
            OnChanged += NotifyHotelChanged;
            //Hotels?.CollectionChanged += Hotels_CollectionChanged;
        }

        private void Hotels_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            NotifyHotelChanged();
        }

        public Task Initialize(IEnumerable<HotelModel> hotels)
        {
            CurrentHotel = new();
            Hotels = new(hotels);
            CurrentHotelModelForm = new();
            NotifyHotelChanged();
            return Task.CompletedTask;
        }

        private void NotifyHotelChanged() => OnHotelChanged?.Invoke();

        public void Dispose()
        {
            OnChanged -= NotifyHotelChanged;
        }
    }
}
