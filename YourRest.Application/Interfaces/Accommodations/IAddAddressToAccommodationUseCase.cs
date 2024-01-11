using YourRest.Application.Dto.Models;

namespace YourRest.Application.Interfaces.Accommodation
{
    public interface IAddAddressToAccommodationUseCase
    {
        Task<ResultDto> Execute(int accommodationId, AddressDto addressDto);
    }
}
