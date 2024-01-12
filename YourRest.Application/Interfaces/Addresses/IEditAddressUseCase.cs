using YourRest.Application.Dto.Models;

namespace YourRest.Application.Interfaces.Addresses
{
    public interface IEditAddressUseCase
    {
        Task<AddressDto> ExecuteAsync(AddressDto addressDto, CancellationToken cancellationToken);
    }
}
