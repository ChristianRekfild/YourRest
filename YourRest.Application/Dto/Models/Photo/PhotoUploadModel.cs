using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace YourRest.Application.Dto.Models.Photo
{
    public class PhotoUploadModel: BasePhotoUploadModel
    {
        [Required]
        public int AccommodationId { get; set; }
    }
}