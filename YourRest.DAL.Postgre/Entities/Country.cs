using System.ComponentModel.DataAnnotations;

namespace YourRest.DAL.Postgre.Entities
{
    public class Country
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
