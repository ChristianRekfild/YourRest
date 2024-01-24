using FluentValidation;

namespace YouRest.HotelierWebApp.Data.ViewModels.Validators
{
    public class CreateHotelViewModelValidator: AbstractValidator<CreateHotelViewModel>
    {
        public CreateHotelViewModelValidator()
        {
            RuleFor(hotel => hotel.Country).NotEmpty().WithName("Поле Страна");
            RuleFor(hotel => hotel.Region).NotEmpty().WithName("Поле Регион");
            RuleFor(hotel => hotel.City).NotEmpty().WithName("Поле Населенный пункт");
            RuleFor(hotel => hotel.Address).NotEmpty().WithName("Поле Адресс");
            RuleFor(hotel => hotel.HotelName).NotEmpty().WithName("Название объекта");
            RuleFor(hotel => hotel.HotelDescription).NotEmpty().WithName("Описание объекта");
            RuleFor(hotel => hotel.HotelType).NotEmpty().WithName("Поле Тип объекта");
            RuleFor(hotel => hotel.HotelRating).NotEmpty().WithName("Поле Уровень комфорта");
            RuleFor(hotel => hotel.ZipCode).NotEmpty().WithName("Поле Индекс").MinimumLength(6).MaximumLength(6);
        }
    }
}
