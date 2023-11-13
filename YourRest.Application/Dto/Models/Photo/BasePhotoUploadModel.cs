using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace YourRest.Application.Dto.Models.Photo
{
    public class BasePhotoUploadModel
    {
        [Required]
        public IFormFile Photo { get; set; }
    }
}