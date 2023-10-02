using YourRest.Application.Dto;

namespace YourRest.Application.Interfaces
{
    public interface IAddAddressToAccommodationUseCase
    {
        Task<ResultDto> Execute(int accommodationId, AddressDto addressDto);
    }
}
