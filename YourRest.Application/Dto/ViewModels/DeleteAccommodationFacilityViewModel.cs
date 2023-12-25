using Microsoft.AspNetCore.Mvc;

namespace YourRest.Application.Dto.ViewModels
{
    public class DeleteAccommodationFacilityViewModel
    {
        [FromRoute(Name = "id")]
        public int Id { get; set; }
        
        [FromRoute(Name = "facility-id")]
        public int FacilityId { get; set; }
    }
}
