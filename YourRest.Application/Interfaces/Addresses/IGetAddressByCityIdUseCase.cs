using YourRest.Application.Dto.Models;

namespace YourRest.Application.Interfaces.Addresses
{
    public interface IGetAddressByCityIdUseCase
    {
        Task<IEnumerable<AddressDto>> ExecuteAsync(int cityId, CancellationToken cancellationToken);
    }
}
