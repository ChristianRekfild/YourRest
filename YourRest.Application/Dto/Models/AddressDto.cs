using System.ComponentModel.DataAnnotations;

namespace YourRest.Application.Dto.Models
{
    public class AddressDto
    {
        //[Required]
        //[MaxLength(100)]
        public string Street { get; set; }

        //[Required]
        //[RegularExpression(@"^\d{6}$", ErrorMessage = "Zip code must be 6 digits")]
        public string ZipCode { get; set; }

        //[Range(-180, 180)]
        public double Longitude { get; set; }

        //[Range(-90, 90)]
        public double Latitude { get; set; }

        //[Required]
        //[RegularExpression(@"^[1-9]\d*$", ErrorMessage = "City Id should be more than zero.")]
        public int CityId { get; set; }
    }
}