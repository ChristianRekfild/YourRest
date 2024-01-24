using FluentValidation;

namespace YouRest.HotelierWebApp.Data.ViewModels.Validators
{
    public class RegistrationViewModelValidator: AbstractValidator<RegistrationViewModel>
    {
        public RegistrationViewModelValidator()
        {
            RuleFor(r => r.Email).NotEmpty().EmailAddress();
            RuleFor(r => r.Password).NotEmpty().Length(6);
            RuleFor(r => r.UserName).NotEmpty();
            RuleFor(r => r.FirstName).NotEmpty();
            RuleFor(r => r.LastName).NotEmpty();
        }
    }
}
