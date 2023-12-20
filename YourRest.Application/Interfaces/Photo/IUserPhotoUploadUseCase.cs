using YourRest.Application.Dto;
using  YourRest.Application.Dto.Models.Photo;

namespace YourRest.Application.Interfaces.Photo
{
    public interface IUserPhotoUploadUseCase
    {
        Task<PhotoUploadResponseDto> ExecuteAsync(UserPhotoUploadModel request, string bucketName, string userKeyCloakId, CancellationToken cancellationToken);
    }
}
