using Newtonsoft.Json;

namespace YouRest.HotelierWebApp.Data.ViewModels
{
    public class SecurityTokenViewModel
    {
        public string? UserName { get; set; }
        public string AccessToken { get; set; }
        public DateTime? ExpiredAt { get; set; }
    }
}
