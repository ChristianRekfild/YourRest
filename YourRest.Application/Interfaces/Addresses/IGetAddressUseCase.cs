using YourRest.Application.Dto.Models;

namespace YourRest.Application.Interfaces.Addresses
{
    public interface IGetAddressUseCase
    {
        Task<IEnumerable<AddressWithIdDto>> ExecuteAsync(CancellationToken cancellationToken);
    }
}
