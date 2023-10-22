using FluentValidation;
using YourRest.Application.Dto.ViewModels;

namespace YourRest.Application.Dto.Validators
{
    public class RouteViewModelValidator:AbstractValidator<RouteViewModel>
    {
        public RouteViewModelValidator()
        {
            RuleFor(route => route.Id).NotNull().NotEmpty().GreaterThan(0);
        }
    }
}
