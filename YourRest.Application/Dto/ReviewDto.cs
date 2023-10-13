using System.ComponentModel.DataAnnotations;

namespace YourRest.Application.Dto
{
    public class ReviewDto
    {
        [Required]
        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "Booking Id should be more than zero.")]
        public int BookingId { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }
        
        [Required]
        public string Comment { get; set; }
    }
}