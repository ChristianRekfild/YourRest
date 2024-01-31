using Microsoft.AspNetCore.Components.Forms;
using YouRest.HotelierWebApp.Data.Models;

namespace YouRest.HotelierWebApp.Data.Services.Abstractions
{
    public interface IFileService
    {
        Task UploadAsync(HotelImgModel hotelImg, CancellationToken cancellationToken);
        Task<string> FetchImg(string filePath, string bucketType);
       
    }
}
