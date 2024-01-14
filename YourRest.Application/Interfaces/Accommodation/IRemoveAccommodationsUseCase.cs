using YourRest.Application.Dto;
using YourRest.Application.Dto.ViewModels;

namespace YourRest.Application.Interfaces
{
    public interface IRemoveAccommodationsUseCase
    {
        Task ExecuteAsync(int accommodationId, CancellationToken cancellationToken);
    }
}
