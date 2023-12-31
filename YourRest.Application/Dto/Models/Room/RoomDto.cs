using System.ComponentModel.DataAnnotations;

namespace YourRest.Application.Dto.Models.Room
{
    public class RoomDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "SquareInMeter should be more than zero.")]
        public double SquareInMeter { get; set; }

        [Required]
        public int RoomTypeId { get; set; }

        [Required]
        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "Capacity should be more than zero.")]
        public int Capacity { get; set; }
    }
}

