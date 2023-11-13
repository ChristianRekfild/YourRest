using Microsoft.AspNetCore.Mvc;
using YourRest.Application.Interfaces.Photo;
using YourRest.Application.Dto.Models.Photo;
using YourRest.Application.Services;
using YourRest.WebApi.Options;
using YourRest.WebApi.Responses;

namespace YourRest.WebApi.Controllers
{
    public class PhotoController : ControllerBase
    {
        private const long MaxFileSize = 5 * 1024 * 1024; // 5 MB
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png" };

        private readonly IAccommodationPhotoUploadUseCase _accommodationPhotoUploadUseCase;
        private readonly IRoomPhotoUploadUseCase _roomPhotoUploadUseCase;
        private readonly AwsOptions _awsOptions;
        private readonly IFileService _fileService;

        public PhotoController(
            IAccommodationPhotoUploadUseCase accommodationPhotoUploadUseCase,
            IRoomPhotoUploadUseCase roomPhotoUploadUseCase,
            AwsOptions awsOptions,
            IFileService fileService
        )
        {
            _accommodationPhotoUploadUseCase = accommodationPhotoUploadUseCase;
            _roomPhotoUploadUseCase = roomPhotoUploadUseCase;
            _awsOptions = awsOptions;
            _fileService = fileService;
        }

        [HttpPost]
        [Route("api/operator/accommodation-photo")]
        public async Task<IActionResult> UploadAccommodationPhoto([FromForm] PhotoUploadModel model)
        {
            var validationErrors = ValidatePhotoUploadModel(model, "Accommodation ID");
            if (validationErrors.Any())
            {
                return BadRequest(new { Errors = validationErrors });
            }

            var dto = await _accommodationPhotoUploadUseCase.Handle(model, _awsOptions.BucketName);
            return Ok(dto);
        }

        [HttpPost]
        [Route("api/operator/room-photo")]
        public async Task<IActionResult> UploadRoomPhoto([FromForm] RoomPhotoUploadModel model)
        {
            var validationErrors = ValidatePhotoUploadModel(model, "Room ID");
            if (validationErrors.Any())
            {
                return BadRequest(new { Errors = validationErrors });
            }

            var dto = await _roomPhotoUploadUseCase.Handle(model, _awsOptions.BucketName);
            return Ok(dto);
        }

        [HttpPost]
        [Route("api/operator/photo/{path}")]
        public async Task<IActionResult> DownloadFileByPath(string path)
        {
            var fileDto = await _fileService.GetFileByPathAsync(path, _awsOptions.BucketName);
            if (fileDto == null)
            {
                return NotFound();
            }

            if (fileDto.Stream.CanSeek)
            {
                fileDto.Stream.Seek(0, SeekOrigin.Begin);
            }

            return File(fileDto.Stream, fileDto.MimeType, fileDto.FileName);
        }

        private List<string> ValidatePhotoUploadModel(BasePhotoUploadModel model, string idFieldName)
        {
            var validationErrors = new List<string>();
            if (model is RoomPhotoUploadModel roomModel)
            {
                if (roomModel.RoomId <= 0)
                {
                    validationErrors.Add($"{idFieldName} is required and must be greater than 0.");
                }
            }
            
            if (model is PhotoUploadModel accommodationModel)
            {
                if (accommodationModel.AccommodationId <= 0)
                {
                    validationErrors.Add($"{idFieldName} is required and must be greater than 0.");
                }
            }

            if (model.Photo == null)
            {
                validationErrors.Add("Photo is required.");
            }
            else
            {
                if (model.Photo.Length > MaxFileSize)
                {
                    validationErrors.Add($"Photo must not exceed {MaxFileSize / (1024 * 1024)} MB.");
                }

                if (!AllowedExtensions.Any(ext => model.Photo.FileName.EndsWith(ext, StringComparison.OrdinalIgnoreCase)))
                {
                    validationErrors.Add($"Invalid photo format. Allowed formats are: {string.Join(", ", AllowedExtensions)}.");
                }
            }

            return validationErrors;
        }
    }
}
