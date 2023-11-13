using YourRest.Application.Dto;
using YourRest.Application.Dto.Models;

namespace YourRest.Application.Services;
public interface IFileService
{
    Task<FileDto> GetFileByPathAsync(string path, string bucketName);

    Task<string> AddPhoto(FileData fileData, string bucketName);
}

