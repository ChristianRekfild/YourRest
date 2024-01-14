using YourRest.Application.Dto.Models;

namespace YourRest.Application.Interfaces.Addresses
{
    public interface IAddAddressUseCase
    {
        Task<AddressDto> ExecuteAsync(AddressDto addressDto, CancellationToken cancellationToken);
    }
}
