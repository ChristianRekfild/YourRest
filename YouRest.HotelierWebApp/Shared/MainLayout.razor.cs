using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using YouRest.HotelierWebApp.Data.ViewModels;

namespace YouRest.HotelierWebApp.Shared
{
    public partial class MainLayout
    {
        public FluentValidationValidator? FluentValidationValidator {get;set;}
        public AuthorizationViewModel Auth { get; set; } = new();
        public bool IsAuthorize { get; set; } = true;
        public void Login() => IsAuthorize = true;
        public void Logout() => IsAuthorize = false;
        public async Task SubmitFormAsync()
        {
            if (await FluentValidationValidator!.ValidateAsync())
            {
                IsAuthorize = true;
                Auth = new AuthorizationViewModel();
            }
        }
    }
}
