using Microsoft.AspNetCore.Mvc;

namespace YourRest.Application.Dto.ViewModels
{
    public class RouteViewModel
    {
        [FromRoute(Name = "id")]
        public int Id { get; set; }
    }
}
