using FluentValidation;

namespace YouRest.HotelierWebApp.Data.ViewModels
{
    public class AuthorizationViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
