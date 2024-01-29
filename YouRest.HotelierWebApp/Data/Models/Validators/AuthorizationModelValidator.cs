using FluentValidation;

namespace YouRest.HotelierWebApp.Data.Models.Validators
{
    public class AuthorizationModelValidator : AbstractValidator<AuthorizationModel>
    {
        public AuthorizationModelValidator()
        {
            RuleFor(auth => auth.Username).NotEmpty().WithName("Поле Email адрес").EmailAddress().WithMessage("Не верный email адрес.");
            RuleFor(auth => auth.Password).NotEmpty().WithName("Поле Пароль").MinimumLength(6);
        }
    }
}
