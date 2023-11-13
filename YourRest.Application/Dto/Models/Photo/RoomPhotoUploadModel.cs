using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace YourRest.Application.Dto.Models.Photo
{
    public class RoomPhotoUploadModel: BasePhotoUploadModel
    {
        [Required]
        public int RoomId { get; set; }
    }
}