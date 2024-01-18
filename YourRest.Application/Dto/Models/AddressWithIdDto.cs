using System.ComponentModel.DataAnnotations;

namespace YourRest.Application.Dto.Models
{
    public class AddressWithIdDto : AddressDto
    {
        public int Id { get; set; }
    }
}