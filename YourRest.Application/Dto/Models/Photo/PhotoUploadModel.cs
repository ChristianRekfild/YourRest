using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace YourRest.Application.Dto.Models.Photo
{
    public class PhotoUploadModel
    { 
        public int AccommodationId { get; set; }
        public IFormFile Photo { get; set; }
    }
}