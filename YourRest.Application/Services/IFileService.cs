using YourRest.Application.Dto;
using YourRest.Application.Dto.Models;

namespace YourRest.Application.Services;
public interface IFileService
{
    Task<FileDto> GetFileByPathAsync(string path, string bucketName, CancellationToken cancellationToken);

    Task<string> AddPhotoAsync(FileData fileData, string bucketName, CancellationToken cancellationToken);
}

