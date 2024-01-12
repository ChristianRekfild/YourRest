using YourRest.Application.Dto.Models;

namespace YourRest.Application.Interfaces.Addresses
{
    public interface IGetAddressByIdUseCase
    {
        Task<AddressDto> ExecuteAsync(int id, CancellationToken cancellationToken);
    }
}
