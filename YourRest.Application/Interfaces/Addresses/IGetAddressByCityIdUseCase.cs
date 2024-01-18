using YourRest.Application.Dto.Models;

namespace YourRest.Application.Interfaces.Addresses
{
    public interface IGetAddressByCityIdUseCase
    {
        Task<IEnumerable<AddressWithIdDto>> ExecuteAsync(int cityId, CancellationToken cancellationToken);
    }
}
