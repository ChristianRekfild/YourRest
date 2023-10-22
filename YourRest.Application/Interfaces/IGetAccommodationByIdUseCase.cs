using YourRest.Application.Dto;

namespace YourRest.Application.Interfaces
{
    public interface IGetAccommodationByIdUseCase
    {
        Task<AccommodationDto> Execute(int id);
    }
}
