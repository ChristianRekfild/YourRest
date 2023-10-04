﻿namespace YourRest.Domain.Entities
{
    public class Room : IntBaseEntity
    {
        public string Name { get; set; }
        public int SquareInMeter { get; set; }

        public string RoomType { get; set; }

        public int Capacity { get; set; }
        public int AccommodationId { get; set; }
        public virtual Accommodation Accommodation { get; set; }
    }
}
