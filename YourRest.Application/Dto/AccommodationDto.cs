using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourRest.Domain.Entities;

namespace YourRest.Application.Dto
{
    public class AccommodationDto
    {
        // Id указан как nullable type для того, чтобы по нему можно было отслеживать - мы создали новый объект (null)
        // Или же получили с базы данных (not null)
        public int? Id { get; set; }

        public string Name { get; set; }

        [MaxLength(255)]
        public string BankAccount { get; set; }
        public string Description { get; set; }

        public int? AddressId { get; set; }
        public Address? Address { get; set; }
    }
}
