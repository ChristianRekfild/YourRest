using YourRest.Application.Dto;
using YourRest.Application.Dto.Models;
using Amazon.S3;
using Amazon.S3.Model;
using System.Text;

namespace YourRest.Application.Services;

public class FileService: IFileService
{
    private readonly IAmazonS3 _s3Client;

    public FileService(IAmazonS3 s3Client)
    {
        _s3Client = s3Client;
    }

    public async Task<FileDto> GetFileByPathAsync(string filePath, string bucketName, CancellationToken cancellationToken)
    {
        var response = await _s3Client.GetObjectAsync(bucketName, filePath, cancellationToken);
        if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
        {
            return null;
        }
        
        //using MemoryStream stream = new MemoryStream();
        //response.ResponseStream.CopyTo(stream);
        //var b = stream.ToArray();
        //var file = Convert.ToBase64String(b);
        
        return new FileDto
        {
            Stream = response.ResponseStream,
            MimeType = response.Headers.ContentType,
            FileName = Path.GetFileName(filePath),
            Img = string.Empty
        };
    }

    public async Task<string> AddPhotoAsync(FileData fileData, string bucketName, CancellationToken cancellationToken)
    {
        bool bucketExists = true;
        try
        {
            await _s3Client.GetBucketLocationAsync(
                new GetBucketLocationRequest
                {
                    BucketName = bucketName
                },
                cancellationToken
            );
        }
        catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            bucketExists = false;
        }


        if (!bucketExists)
        {
            await _s3Client.PutBucketAsync(
                new PutBucketRequest
                {
                    BucketName = bucketName
                },
                cancellationToken
            );
        }

        var stream = new MemoryStream();
        await fileData.Content.CopyToAsync(stream);

        var putRequest = new PutObjectRequest
        {
            BucketName = bucketName,
            Key = Guid.NewGuid().ToString() + Path.GetExtension(fileData.FileName),
            InputStream = stream
        };

        var response = await _s3Client.PutObjectAsync(putRequest, cancellationToken);
        return response.HttpStatusCode == System.Net.HttpStatusCode.OK 
            ? putRequest.Key
            : null;
    }
}
