using FluentValidation;

namespace YouRest.HotelierWebApp.Data.Models
{
    public class AuthorizationModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
