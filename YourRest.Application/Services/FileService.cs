using YourRest.Application.Dto;
using YourRest.Application.Dto.Models;
using Amazon.S3;
using Amazon.S3.Model;

namespace YourRest.Application.Services;

public class FileService: IFileService
{
    private readonly IAmazonS3 _s3Client;

    public FileService(IAmazonS3 s3Client)
    {
        _s3Client = s3Client;
    }

    public async Task<FileDto> GetFileByPathAsync(string filePath, string bucketName)
    {
        var response = await _s3Client.GetObjectAsync(bucketName, filePath);
        if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
        {
            return null;
        }

        return new FileDto
        {
            Stream = response.ResponseStream,
            MimeType = response.Headers.ContentType,
            FileName = Path.GetFileName(filePath)
        };
    }

    public async Task<string> AddPhoto(FileData fileData, string bucketName)
    {
        bool bucketExists = true;
        try
        {
            await _s3Client.GetBucketLocationAsync(new GetBucketLocationRequest
            {
                BucketName = bucketName
            });
        }
        catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            bucketExists = false;
        }


        if (!bucketExists)
        {
            await _s3Client.PutBucketAsync(new PutBucketRequest
            {
                BucketName = bucketName
            });
        }

        var stream = new MemoryStream();
        await fileData.Content.CopyToAsync(stream);

        var putRequest = new PutObjectRequest
        {
            BucketName = bucketName,
            Key = Guid.NewGuid().ToString() + Path.GetExtension(fileData.FileName),
            InputStream = stream
        };

        var response = await _s3Client.PutObjectAsync(putRequest);
        return response.HttpStatusCode == System.Net.HttpStatusCode.OK 
            ? putRequest.Key
            : null;
    }
}
