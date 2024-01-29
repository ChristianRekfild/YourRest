using Newtonsoft.Json;

namespace YouRest.HotelierWebApp.Data.Models
{
    public class SecurityTokenModel
    {
        public string? UserName { get; set; }
        public string AccessToken { get; set; }
        public DateTime? ExpiredAt { get; set; }
    }
}
