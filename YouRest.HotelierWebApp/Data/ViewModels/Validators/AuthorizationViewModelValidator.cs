using FluentValidation;
using YouRest.HotelierWebApp.Data.ViewModels;

namespace YouRest.HotelierWebApp.Data.ViewModels.Validators
{
    public class AuthorizationViewModelValidator : AbstractValidator<AuthorizationViewModel>
    {
        public AuthorizationViewModelValidator()
        {
            RuleFor(auth => auth.Login).NotNull().NotEmpty().WithName("Поле Email адрес").EmailAddress().WithMessage("Не верный email адрес.");
            RuleFor(auth => auth.Password).NotNull().NotEmpty().WithName("Поле Пароль").MinimumLength(6);
        }
    }
}
