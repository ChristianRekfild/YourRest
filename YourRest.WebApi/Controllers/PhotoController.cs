using Microsoft.AspNetCore.Mvc;
using YourRest.Application.Interfaces.Photo;
using YourRest.Application.Dto.Models.Photo;
using YourRest.WebApi.Options;
using YourRest.WebApi.Responses;

namespace YourRest.WebApi.Controllers
{
    [Route("api/operator/accommodation-photo")]
    public class PhotoController : ControllerBase
    {
        private readonly IAccommodationPhotoUploadUseCase _accommodationPhotoUploadUseCase;
        private readonly AwsOptions _awsOptions;

        public PhotoController(
            IAccommodationPhotoUploadUseCase accommodationPhotoUploadUseCase,
            AwsOptions awsOptions
            )
        {
            _accommodationPhotoUploadUseCase = accommodationPhotoUploadUseCase;
            _awsOptions = awsOptions;

        }

        [HttpPost]
        public async Task<IActionResult> UploadPhoto([FromForm] PhotoUploadModel model)
        {
            List<string> validationErrors = new List<string>();

            if (model.Accommodation <= 0)
            {
                validationErrors.Add("Accommodation ID is required and must be greater than 0.");
            }

            if (model.Photo == null)
            {
                validationErrors.Add("Photo is required.");
            }
            else
            {
                if (model.Photo.Length > 5 * 1024 * 1024)
                {
                    validationErrors.Add("Photo must not exceed 5 MB.");
                }

                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                if (!allowedExtensions.Any(ext => model.Photo.FileName.EndsWith(ext, StringComparison.OrdinalIgnoreCase)))
                {
                    validationErrors.Add("Invalid photo format. Allowed formats are .jpg, .jpeg, .png.");
                }
            }

            if (validationErrors.Any())
            {
                return BadRequest(new { Errors = validationErrors });
            }
            
            var dto = await _accommodationPhotoUploadUseCase.Handle(model, _awsOptions.BucketName);
            
            return Ok(dto);
        }
    }
}