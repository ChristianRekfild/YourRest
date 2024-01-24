using FluentValidation;

namespace YouRest.HotelierWebApp.Data.Models.Validators
{
    public class RegistrationModelValidator : AbstractValidator<RegistrationModel>
    {
        public RegistrationModelValidator()
        {
            RuleFor(r => r.Email).NotEmpty().EmailAddress();
            RuleFor(r => r.Password).NotEmpty().Length(6);
            RuleFor(r => r.UserName).NotEmpty();
            RuleFor(r => r.FirstName).NotEmpty();
            RuleFor(r => r.LastName).NotEmpty();
        }
    }
}
