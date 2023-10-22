using System.ComponentModel.DataAnnotations;

namespace YourRest.Domain.Entities
{
    public class Accommodation : IntBaseEntity
    {
        public string Name { get; set; }

        [MaxLength(255)]
        public string BankAccount { get; set; }
        public string Description { get; set; }

        public int? AddressId { get; set; }
        public Address? Address { get; set; }

        public int AccommodationTypeId { get; set; }
        public AccommodationType AccommodationType { get; set; }

        public List<Room> Rooms { get; set; }
    }
}
