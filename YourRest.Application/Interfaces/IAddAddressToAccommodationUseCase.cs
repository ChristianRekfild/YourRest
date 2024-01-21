using YourRest.Application.Dto.Models;

namespace YourRest.Application.Interfaces
{
    public interface IAddAddressToAccommodationUseCase
    {
        Task<ResultDto> ExecuteAsync(int accommodationId, AddressDto addressWithIdDto);
    }
}
