﻿namespace YourRest.Domain.Entities
{
    public class Accommodation : IntBaseEntity
    {
        public Accommodation() => Rooms = new List<Room>(); 
        public string Name { get; set; }

        public Address? Address { get; set; }
        public int? AddressId { get; set; }
        public ICollection<Room> Rooms { get; set; }
        
        public ICollection<UserAccommodation> UserAccommodations { get; set; } = new List<UserAccommodation>();
    }
}
