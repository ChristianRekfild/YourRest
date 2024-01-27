using Microsoft.AspNetCore.Mvc;
using YourRest.Application.Interfaces.Photo;
using YourRest.Application.Dto.Models.Photo;
using YourRest.Application.Services;
using YourRest.WebApi.Options;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Attributes;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace YourRest.WebApi.Controllers
{
    [ApiController]
    [FluentValidationAutoValidation]
    public class PhotoController : ControllerBase
    {
        private readonly IAccommodationPhotoUploadUseCase _accommodationPhotoUploadUseCase;
        private readonly IRoomPhotoUploadUseCase _roomPhotoUploadUseCase;
        private readonly IUserPhotoUploadUseCase _userPhotoUploadUseCase;
        private readonly ICityPhotoUploadUseCase _cityPhotoUploadUseCase;
        private readonly IGetUserPhotosUseCase _getUserPhotosUseCase;
        private readonly AwsOptions _awsOptions;
        private readonly IFileService _fileService;

        public PhotoController(
            IAccommodationPhotoUploadUseCase accommodationPhotoUploadUseCase,
            IRoomPhotoUploadUseCase roomPhotoUploadUseCase,
            IUserPhotoUploadUseCase userPhotoUploadUseCase,
            ICityPhotoUploadUseCase cityPhotoUploadUseCase,
            IGetUserPhotosUseCase getUserPhotosUseCase,
            AwsOptions awsOptions,
            IFileService fileService
        )
        {
            _accommodationPhotoUploadUseCase = accommodationPhotoUploadUseCase;
            _roomPhotoUploadUseCase = roomPhotoUploadUseCase;
            _userPhotoUploadUseCase = userPhotoUploadUseCase;
            _cityPhotoUploadUseCase = cityPhotoUploadUseCase;
            _getUserPhotosUseCase = getUserPhotosUseCase;
            _awsOptions = awsOptions;
            _fileService = fileService;
        }

        [HttpPost]
        [Route("api/accommodation-photo")]
        public async Task<IActionResult> UploadAccommodationPhotoAsync([FromBody] AccomondationPhotoModel model)
        {

            using MemoryStream stream = new MemoryStream(Convert.FromBase64String(model.Photo));
            var _model = new PhotoUploadModel()
            {
                AccommodationId = model.AccommodationId,
                Photo = new FormFile(stream, 0, stream.Length, model.FileName, model.FileName)
            };
            var dto = await _accommodationPhotoUploadUseCase.ExecuteAsync(_model, _awsOptions.BucketNames.Accommodation, HttpContext.RequestAborted);
            return Ok(dto);
        }

        [HttpPost]
        [Route("api/room-photo")]
        public async Task<IActionResult> UploadRoomPhotoAsync([FromForm] RoomPhotoUploadModel model)
        {
            var dto = await _roomPhotoUploadUseCase.ExecuteAsync(model, _awsOptions.BucketNames.Room, HttpContext.RequestAborted);
            return Ok(dto);
        }
        
        [HttpPost]
        [Route("api/city-photo")]
        public async Task<IActionResult> UploadCityPhotoAsync([FromForm] CityPhotoUploadModel model)
        {
            var dto = await _cityPhotoUploadUseCase.ExecuteAsync(model, _awsOptions.BucketNames.City, HttpContext.RequestAborted);
            return Ok(dto);
        }

        [Authorize]
        [HttpPost]
        [Route("api/user-photo")]
        public async Task<IActionResult> UploadUserPhotoAsync([FromForm] UserPhotoUploadModel model)
        {
            var user = HttpContext.User;
            var identity = user.Identity as ClaimsIdentity;
            var sub = identity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (sub == null)
            {
                return NotFound("User not found");
            }

            var dto = await _userPhotoUploadUseCase.ExecuteAsync(model, _awsOptions.BucketNames.User, sub, HttpContext.RequestAborted);
            return Ok(dto);
        }
        
        [Authorize]
        [HttpGet]
        [Route("api/user-photos")]
        public async Task<IActionResult> GetUserPhotosAsync()
        {
            var user = HttpContext.User;
            var identity = user.Identity as ClaimsIdentity;
            var sub = identity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (sub == null)
            {
                return NotFound("User not found");
            }
            
            var dto = await _getUserPhotosUseCase.ExecuteAsync(sub, HttpContext.RequestAborted);
            return Ok(dto);
        }

        [HttpGet]
        [Route("api/photo/{path}")]
        public async Task<IActionResult> DownloadFileByPathAsync(string path, [FromQuery] string bucketType, [FromQuery] bool shouldReturnStream = false)
        {
            string bucketName = bucketType switch
            {
                "Accommodation" => _awsOptions.BucketNames.Accommodation,
                "Room" => _awsOptions.BucketNames.Room,
                "User" => _awsOptions.BucketNames.User,
                "City" => _awsOptions.BucketNames.City,
                _ => throw new ArgumentException("Invalid bucket type")
            };

            var fileDto = await _fileService.GetFileByPathAsync(path, bucketName, HttpContext.RequestAborted);
            if (fileDto == null)
            {
                return NotFound();
            }

            if (shouldReturnStream)
            {
                if (fileDto.Stream.CanSeek)
                {
                    fileDto.Stream.Seek(0, SeekOrigin.Begin);
                }
                return File(fileDto.Stream, fileDto.MimeType, fileDto.FileName);
            }

            using MemoryStream stream = new MemoryStream();
            fileDto.Stream.CopyTo(stream);
            var b = stream.ToArray();
            var file = Convert.ToBase64String(b);
            return Ok($"data:{fileDto.MimeType};base64,{file}");
        }
        
        [Authorize]
        [HttpDelete]
        [Route("api/photo/{path}")]
        public async Task<IActionResult> DeletePhotoAsync(string path, [FromQuery] string bucketType)
        {
            string bucketName = bucketType switch
            {
                "Accommodation" => _awsOptions.BucketNames.Accommodation,
                "Room" => _awsOptions.BucketNames.Room,
                "User" => _awsOptions.BucketNames.User,
                _ => throw new ArgumentException("Invalid bucket type")
            };
            
            await _fileService.DeleteFileAsync(path, bucketName, HttpContext.RequestAborted);

            return Ok($"Photo with ID {path} has been successfully deleted.");
        }
    }
}
