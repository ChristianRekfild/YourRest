using YourRest.Application.Dto.Models;

namespace YourRest.Application.Interfaces.Addresses
{
    public interface IEditAddressUseCase
    {
        Task<AddressWithIdDto> ExecuteAsync(AddressWithIdDto addressWithIdDto, CancellationToken cancellationToken);
    }
}
