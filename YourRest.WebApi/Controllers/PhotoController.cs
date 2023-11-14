using Microsoft.AspNetCore.Mvc;
using YourRest.Application.Interfaces.Photo;
using YourRest.Application.Dto.Models.Photo;
using YourRest.Application.Services;
using YourRest.WebApi.Options;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Attributes;
namespace YourRest.WebApi.Controllers
{
    [ApiController]
    [FluentValidationAutoValidation]
    public class PhotoController : ControllerBase
    {
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
            var dto = await _accommodationPhotoUploadUseCase.Handle(model, _awsOptions.BucketName);
            return Ok(dto);
        }

        [HttpPost]
        [Route("api/operator/room-photo")]
        public async Task<IActionResult> UploadRoomPhoto([FromForm] RoomPhotoUploadModel model)
        {
            var dto = await _roomPhotoUploadUseCase.Handle(model, _awsOptions.BucketName);
            return Ok(dto);
        }

        [HttpGet]
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
    }
}
