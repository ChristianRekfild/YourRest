using  YourRest.Application.Dto.Models.Photo;

namespace YourRest.Application.Interfaces.Photo
{
    public interface IGetUserPhotosUseCase
    {
        Task<IEnumerable<PhotoPathResponseDto>> ExecuteAsync(string userKeyCloakId, CancellationToken cancellationToken);
    }
}
