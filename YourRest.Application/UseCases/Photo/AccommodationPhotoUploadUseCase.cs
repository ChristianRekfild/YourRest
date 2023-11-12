using Amazon.S3;
using Amazon.S3.Model;
using YourRest.Application.Interfaces.Photo;
using YourRest.Domain.Repositories;
using YourRest.Application.Dto.Models.Photo;
using YourRest.Domain.Entities;
using YourRest.Application.Exceptions;

namespace YourRest.Application.UseCases.Photo
{
    public class AccommodationPhotoUploadUseCase : IAccommodationPhotoUploadUseCase
    {
        private readonly IAccommodationPhotoRepository _photoRepository;
        private readonly IAccommodationRepository _accommodationRepository;
        private readonly IAmazonS3 _s3Client;

        public AccommodationPhotoUploadUseCase(
            IAccommodationPhotoRepository photoRepository, 
            IAccommodationRepository accommodationRepository,
            IAmazonS3 s3Client
            )
        {
            _photoRepository = photoRepository;
            _accommodationRepository = accommodationRepository;
            _s3Client = s3Client;
        }
        public async Task<PhotoUploadResponseDto> Handle(PhotoUploadModel photoUploadModel, string bucketName)
        {
            var accommodation = await _accommodationRepository.GetAsync(photoUploadModel.Accommodation);

            if (accommodation == null)
            {
                throw new EntityNotFoundException($"Accommodation with id {photoUploadModel.Accommodation} not found");
            }
            
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
             await photoUploadModel.Photo.CopyToAsync(stream);
            
             var putRequest = new PutObjectRequest
             {
                 BucketName = bucketName,
                 Key = Guid.NewGuid().ToString() + Path.GetExtension(photoUploadModel.Photo.FileName),
                 InputStream = stream
             };
            
             var response = await _s3Client.PutObjectAsync(putRequest);
            
             if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
             {
                 throw new ValidationException("Failed to upload the photo to storage.");
             }

            var photoPath = putRequest.Key;
            
            var photo = new AccommodationPhoto()
            {
                Accommodation = accommodation,
                FilePath = photoPath
            };

            var savedPhoto = await _photoRepository.AddAsync(photo, false);
            await _photoRepository.SaveChangesAsync();

            return new PhotoUploadResponseDto() { FilePath = savedPhoto.FilePath };
        }
    }
}
