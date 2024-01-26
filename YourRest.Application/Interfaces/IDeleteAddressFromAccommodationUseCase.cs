using YourRest.Application.Dto.Models;

namespace YourRest.Application.Interfaces
{
    public interface IDeleteAddressFromAccommodationUseCase
    {
        Task<bool> Execute(int accommodationId, int addressId);
    }
}
