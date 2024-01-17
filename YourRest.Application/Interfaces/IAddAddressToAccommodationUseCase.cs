using YourRest.Application.Dto.Models;

namespace YourRest.Application.Interfaces
{
    public interface IAddAddressToAccommodationUseCase
    {
        Task<ResultDto> Execute(int accommodationId, AddressDto addressWithIdDto);
    }
}
