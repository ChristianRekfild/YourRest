using YourRest.Application.Dto;
using YourRest.Application.Dto.ViewModels;

namespace YourRest.Application.Interfaces.Accommodations
{
    public interface IRemoveAccommodationsUseCase
    {
        Task ExecuteAsync(int accommodationId, CancellationToken cancellationToken);
    }
}
