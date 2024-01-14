using YourRest.Application.Dto.Models;

namespace YourRest.Application.Interfaces.Addresses
{
    public interface IGetAddressUseCase
    {
        Task<IEnumerable<AddressDto>> ExecuteAsync(CancellationToken cancellationToken);
    }
}
