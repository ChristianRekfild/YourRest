using YouRest.HotelierWebApp.Data.ViewModels.Interfaces;

namespace YouRest.HotelierWebApp.Data.ViewModels
{
    public class BaseViewModel: IBaseViewModel
    {
        string _title = string.Empty;
        string _subTitle = string.Empty;
        bool _isBusy;
        bool _isNotBusy;
        bool _canLoadMore;

        public string Title { get => _title; set => SetProperty(ref _title, value); }
        public string SubTitle { get => _subTitle; set => SetProperty(ref _subTitle, value); }
        public bool IsBusy { get => _isBusy; set => SetProperty(ref _isBusy, value); }
        public bool IsNotBusy { get => _isNotBusy; set => SetProperty(ref _isNotBusy, value); }
        public bool CanLoadMore { get => _canLoadMore; set => SetProperty(ref _canLoadMore, value); }

        public event Action? OnChanged;

        public bool SetProperty<T>(
           ref T backingStore, T value,
           Func<T, T, bool>? validateValue = null)
        {
            //if value didn't change
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            //if value changed but didn't validate
            if (validateValue != null && !validateValue(backingStore, value))
                return false;

            backingStore = value;
            OnChanged?.Invoke();
            return true;
        }
    }
}
