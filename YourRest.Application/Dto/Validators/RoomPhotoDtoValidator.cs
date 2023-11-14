using FluentValidation;
using YourRest.Application.Dto.Models.Photo;
using Microsoft.AspNetCore.Http;

namespace YourRest.Application.Dto.Validators
{
    using FluentValidation;

    public class RoomPhotoDtoValidator : AbstractValidator<RoomPhotoUploadModel>
    {
        private const long MaxFileSize = 5 * 1024 * 1024; // 5 MB
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png" };

        public RoomPhotoDtoValidator()
        {
            RuleFor(photo => photo.Photo)
                .NotNull()
                .NotEmpty()
                .Must(BeAValidSize).WithMessage("File size must not exceed 5 MB")
                .Must(BeAValidExtension).WithMessage("Invalid file extension. Allowed extensions are .jpg, .jpeg, .png");

            RuleFor(photo => photo.RoomId)
                .NotNull()
                .NotEmpty();
        }

        private bool BeAValidSize(IFormFile file)
        {
            return file.Length <= MaxFileSize;
        }

        private bool BeAValidExtension(IFormFile file)
        {
            return AllowedExtensions.Any(ext => file.FileName.EndsWith(ext, StringComparison.OrdinalIgnoreCase));
        }
    }
}
