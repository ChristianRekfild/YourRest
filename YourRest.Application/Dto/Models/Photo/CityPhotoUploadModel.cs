using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace YourRest.Application.Dto.Models.Photo
{
    public class CityPhotoUploadModel
    { 
        public int CityId { get; set; }
        public IFormFile Photo { get; set; }
    }
}