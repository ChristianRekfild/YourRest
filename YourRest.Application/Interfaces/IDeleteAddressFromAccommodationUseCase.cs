using YourRest.Application.Dto.Models;

namespace YourRest.Application.Interfaces
{
    public interface IDeleteAddressFromAccommodationUseCase
    {
        Task<bool> ExecuteAsync(int accommodationId, int addressId);
    }
}
