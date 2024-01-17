using YourRest.Application.Dto.Models;

namespace YourRest.Application.Interfaces.Addresses
{
    public interface IAddAddressUseCase
    {
        Task<AddressWithIdDto> ExecuteAsync(AddressWithIdDto addressWithIdDto, CancellationToken cancellationToken);
    }
}
