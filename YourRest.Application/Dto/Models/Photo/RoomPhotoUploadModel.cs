using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace YourRest.Application.Dto.Models.Photo
{
    public class RoomPhotoUploadModel
    { 
        public int RoomId { get; set; }
        public IFormFile Photo { get; set; }
    }
}