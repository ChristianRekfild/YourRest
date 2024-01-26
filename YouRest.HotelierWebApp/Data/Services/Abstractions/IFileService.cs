using Microsoft.AspNetCore.Components.Forms;

namespace YouRest.HotelierWebApp.Data.Services.Abstractions
{
    public interface IFileService
    {
        Task Upload(IBrowserFile file);
        Task<string> FetchImg(string filePath, string bucketType);
       
    }
}
