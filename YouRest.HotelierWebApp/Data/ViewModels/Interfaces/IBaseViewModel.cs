namespace YouRest.HotelierWebApp.Data.ViewModels.Interfaces
{
    public interface IBaseViewModel
    {
        string Title { get; set; }
        string SubTitle { get; set; }
        bool IsBusy { get; set; }
        bool IsNotBusy { get; set; }
        bool CanLoadMore { get; set; }
        event Action? OnChanged;
        bool SetProperty<T>(ref T backingStore, T value,
           Func<T, T, bool>? validateValue = null);
    }
}
