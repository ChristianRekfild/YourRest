using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace YourRest.Application.Dto.Models.Photo
{
    public class UserPhotoUploadModel
    { 
        public IFormFile Photo { get; set; }
    }
}